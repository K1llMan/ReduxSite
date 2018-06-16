// Globals variables
var ajaxUrl = "wp-admin/admin-ajax.php";

var context = {
    'currentModule': null,
    'readyForDisplay': function( isReady ){
        var content = $('.main-content');
        var loading = $('.loading-container');
        
        if (isReady){
            content.addClass('visible');
            loading.removeClass('visible');
            return;
        }
        
        content.removeClass('visible');
        loading.addClass('visible');
    },
    'isLogged': false
}

function updatePath( url ){
    return 'wp-content/themes/redux/' + url;
}

String.prototype.format = String.prototype.f = function () {
    var args = arguments;
    return this.replace(/\{\{|\}\}|\{(\d+)\}/g, function (m, n) {
        if (m == "{{") { return "{"; }
        if (m == "}}") { return "}"; }
        return args[n];
    });
};

// These are called on page load
$(function () {
    var modules = {};
    var modulesPath = 'assets/js/modules/';
    var templatesPath = 'assets/templates/';
   
    // Load module script
    function loadModuleScript( script, params ) {
        $.getScript( updatePath(modulesPath + script), function( data, textStatus, jqxhr ) {
            // Init module after loading
            context.currentModule.init(params);
        });
    }

	//	Event handlers for frontend navigation
    function loadModule( module, params ) {
		// Hide whatever page is currently shown.
        var content = $('.main-content');
		content.html('');

        context.readyForDisplay(false);
        
        if (module.template)
            // Load templates first
            Templater.downloadTemplates(updatePath(templatesPath + module.template), function(){
                loadModuleScript(module.script, params);
            });
        else
            loadModuleScript(module.script, params);
    }
    
    // Global error handler    
    $( document ).ajaxError(function(event, request, settings) {
        console.log(event);
        renderErrorPage();
    });
    
    $(window).error(function() {
        renderErrorPage();     
    });     
    
    // An event handler with calls the render function on every hashchange.
    // The render function will show the appropriate content of out page.
    $(window).on('hashchange', function(){
        render(decodeURI(window.location.hash));
    });

    // Send the data using post 
    $.post( ajaxUrl, { action: 'get_context' })
        .done(function( data ) {
            var json = JSON.parse(data);

            context.isLogged = json.isLogged;
            modules = json.modules;

            // Generate navigation
            $.each(modules, function(key, value){
                var navRow = $('<div class="navRow" id="{0}"><span>{1}</span></div>'.format(key.replace('#', ''), value.displayName));
                navRow.click(function(){
                    window.location.hash = key;
                });
                
                $('.navigation').append(navRow);
            })

            // Manually trigger a hashchange to start the app.
            $(window).trigger('hashchange');
        });

	// Navigation
	function render(url) {
		// Get the keyword from the url.
		var temp = url.split('/')[0];

		// Execute the needed function depending on the url keyword (stored in temp).
		if(modules[temp]){
            $('.navigation').find('.selected').removeClass('selected');        
            $('.navigation').find('#' + temp.replace('#', '')).addClass('selected');
            
            loadModule(modules[temp]);
		}
		// If the keyword isn't listed in the above - render the error page.
		else {
			renderErrorPage();
		}
	}
    
	// Shows the error page.
	function renderErrorPage(){
        $('.main-content').html('<h3>Sorry, something went wrong :(</h3>');
	}    
});
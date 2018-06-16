$(function () {
	function generateHeroes( data ){
		var rows = JSON.parse(data);
		var div = $('<div></div>');

		var maxSum = Math.max.apply(null, $.map(rows, function(row){
			return parseInt(row.PickCount); 
		}));

		console.log(maxSum);
        $.each(rows, function(index, rowData){
            var row = $(Templater.useTemplate('hero', [{
                'caption': rowData.HeroName,
                'pickCount': rowData.PickCount, 
                'pickPerc': rowData.PickCount / maxSum * 100
            }]) );

            div.append(row);
        });

        return div;
	}

    var module = {
        'init': function() {
            var page = $('<div class="heroes"></div>');

            // Send the data using post 
            var posting = $.post( ajaxUrl, {
                    action: 'get_heroes'
                });

            posting.done(function( data ) {
                var div = generateHeroes(data);

                $('.heroes').append(div);              

                // Show table
                context.readyForDisplay(true);                
            });

            $('.main-content').append(page);
        }
    }
    
    context.currentModule = module;
});
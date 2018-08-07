$(function () {

    var module = {
        'init': function() {
            var page = $(Templater.useTemplate('players-page'));

            $('.main-content').append(page);

            // Show table
            context.readyForDisplay(true);

        }
    }
    
    context.currentModule = module;
}());
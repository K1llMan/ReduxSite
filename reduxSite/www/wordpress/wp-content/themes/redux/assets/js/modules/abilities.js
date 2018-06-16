$(function () {
	function generateAbilities( data ){
		var rows = JSON.parse(data);
		var div = $('<div></div>');

		var maxSum = Math.max.apply(null, $.map(rows, function(row){
			return parseInt(row.PickCount) + parseInt(row.BanCount); 
		}));

        $.each(rows, function(index, rowData){
            var row = $(Templater.useTemplate('ability', [{
                'caption': rowData.Name, 
                'pickCount': rowData.PickCount + '/' + rowData.BanCount, 
                'winrateCount': rowData.LossCount + '/' + rowData.WinCount, 
                'pickPerc': rowData.PickCount / maxSum * 100, 
                'banPerc': rowData.BanCount / maxSum * 100, 
                'lossPerc': rowData.LossCount / rowData.PickCount * 100, 
                'winPerc': rowData.WinCount / rowData.PickCount * 100
             }]) );

            div.append(row);
        });

        return div;	
	}
    
    // Clear abilities list
    function removeSelected(){
        // Send the data using post 
        var posting = $.post( ajaxUrl, { action: 'clear_abilities' })
        .done(function() {
            Materialize.toast('Cleared!', 2000);
            $('.main-content').html('');
            context.currentModule.init();
        });
    };    

    var module = {
        'init': function() {
            var page = $('<div class="abilities"></div>');

            // Send the data using post 
            var posting = $.post( ajaxUrl, { action: 'get_abilities'})
                .done(function( data ) {
                    var div = generateAbilities(data);

                    $('.abilities').append(div); 

                    if (context.isLogged){
                        var deleteButton = $(Templater.useTemplate('delete'));
                         
                        deleteButton.find('#removeSelectedMenu').click(removeSelected);
                        $('.abilities').append(deleteButton);
                    }
                    
                    // Show table
                    context.readyForDisplay(true);
                    $('.tooltipped').tooltip({delay: 50});
                });

            $('.main-content').append(page);
        }
    }
    
    context.currentModule = module;
});
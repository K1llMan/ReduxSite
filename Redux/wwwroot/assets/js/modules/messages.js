$(function () {
    // Handlers
    function openSteamProfile( steamID ) {
        window.open('http://steamcommunity.com/profiles/[U:1:' + steamID + ']', '_blank');
    };

    // Generate table
    function generateTable( data ){
        var rows = data.rows;

        var table = $(Templater.useTemplate('table'));
        
        if (context.isLogged())
            table.find('thead tr').prepend('<th style="min-width: 100px;">Selection</th>')
        
        $.each(rows, function(index, rowData){
            var row = $( Templater.useTemplate('row', [{
                'comment': decodeURIComponent(rowData.Comment),
                'steamID': rowData.SteamID,
                'name': decodeURIComponent(rowData.Nickname),
                'id': rowData.ID,
                'reply': !!rowData.Reply ? decodeURIComponent(rowData.Reply).replace(/\+/g, ' ') : ''
            }]) );

            if (context.isLogged())
                row.prepend(Templater.useTemplate('check', [{ 'id': rowData.ID }]));
            
            row.find('[data-steamid]').click(function(){
                openSteamProfile( $(this).attr('data-steamid') );
            });
            
            if (context.isLogged()) {
                row.find('[id]').click(function(){
                    // Handle clicks only in text fields
                    if ($('#hiddenDiv').find("#submitReply").length == 0)
                    {
                        if ($('#submitReply').parent().is(this))
                            return
                        $('#cancelButton').click();
                    }

                    replyText = $(this).text();
                    $(this).text('');

                    $('[name = reply]').val(replyText);

                    // Move form
                    var replyForm = $("#submitReply").detach();
                    $(this).append(replyForm);
                });
            }

            table.find('tbody').append(row);
        });
        
        return table;
    }

    // Batch remove
    function removeSelected(){
        var checked_list = $('[id ^= check ]:checked');
        if (checked_list.length == 0)
            return;

        // Create ids array
        var id_list = $.map(
            checked_list, 
            function(val, i) { return val.id.replace(/check_/, ''); }
        );
        
        // Delete data
        $.ajax('api/messages', {
            type: 'DELETE',
            data: JSON.stringify({
                ids: id_list
            }),
            success: function (data) {
                $.each(checked_list, function (index, item) {
                    $(item).parents('tr').remove();
                });
            }
        });
    };
    
    // Submit handler
    function submitChanges(event, action){
        event.stopPropagation();
        
        var panel = $("#submitReply");
        /* get some values from elements on the page: */
        var reply_value = panel.find( 'textarea[name="reply"]' ).val();

        // Get comment id
        var id = panel.parent().attr('id').replace(/id_/, '');

        switch(action)
        {
            case 'add_reply':
                if (reply_value.length > 1024)
                    break;

                // Update data
                $.ajax('api/messages', {
                    type: 'PUT',
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                            ID: id,
                            Reply: reply_value
                        }),
                    success: function (response) {
                        //...
                    },
                    error: function (jqXHR, exception) {
                        console.log(exception);
                    }
                });
                break;

            case 'remove_reply':
                $.ajax('api/messages', {
                    type: 'PUT',
                    data: JSON.stringify({
                        id: id,
                        reply: ""
                    }),
                    success: function (data) {
                        $('#id_' + id).parent().remove();
                    }
                });
                break;
        }

        setTimeout(function(){
            $('#id_' + id).text(reply_value);
        }, 10);

        var replyForm = $("#submitReply").detach();
        $('#hiddenDiv').append(replyForm);
    }

    // Hidden div
    function getHiddenDiv() {
        var div = $(Templater.useTemplate('hidden-div'));

        div.find('#sendButton').click(function(){
            submitChanges(event, 'add_reply');
        });

        div.find('#removeButton').click(function() {
            submitChanges(event, 'remove_reply');
        });

        div.find('#cancelButton').click(function() {
            submitChanges(event, '');
        });

        div.find('textarea#textarea').characterCounter();

        return div;
    }

    // Module initialization
    var module = {
        'init': function() {
            var page = $('<div class="messages"></div>');
            
            $.get( "api/messages")
            .done(function( data ) {
                var table = generateTable(data);

                if (context.isLogged()){
                    var deleteButton = $(Templater.useTemplate('delete'));
                         
                    deleteButton.find('#removeSelectedMenu').click(removeSelected);

                    $('.messages').append(getHiddenDiv());
                    $('.messages').append(deleteButton);
                }

                $('.messages').append(table);
                $('.messages').append($(Templater.useTemplate('pagination')));

                // Show table
                context.readyForDisplay(true);
                $('.tooltipped').tooltip({delay: 50});
            });
                
            $('.main-content').append(page);            
        }
    }
    
    // Update global module reference
    context.currentModule = module;
}());
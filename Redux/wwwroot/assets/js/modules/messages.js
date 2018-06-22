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
            var page = $('<div></div>');
            
            $.get( "api/messages")
            .done(function( data ) {
                // Show table
                context.readyForDisplay(true);

                var table = $(Templater.useTemplate('table'));

                page.append(table);

                $('.datatable').datatable({
                    'tableHeader': 'Messages',
                    'fields': {
                        'col1': {
                            'header': 'Column 1',
                            'tooltip': 'Column 1',
                            'hidden': false,
                            'editable': false
                        },
                        'col2': {
                            'header': 'Column 2',
                            'tooltip': 'Column 2',
                            'hidden': false,
                            'editable': false
                        },
                        'col3': {
                            'header': 'Column 3',
                            'tooltip': 'Column 3',
                            'hidden': false,
                            'editable': true,
                            'size': 10
                        },
                        'col4': {
                            'header': 'Column 4',
                            'tooltip': 'Column 4',
                            'hidden': false,
                            'editable': true,
                            'size': 1000,
                            'afterEdit': function (row) {
                                alert(row);
                            }
                        }
                    },
                    'hideHeader': false,
                    'hideFooter': false,
                    'hideSelection': false,
                    'getData': function (page, pageSize, sort, sortDir, updateData) {
                        var response = {
                            'total': 100,
                            'page': page,
                            'pageSize': pageSize,
                            'pageCount': 10,
                            'rows': [
                                { 'col1': 'data41', 'col2': 'data41', 'col3': 'data41', 'col4': 'data51', 'col5': 'ololo', 'col6': '434525', 'col7': 'Column 7', 'col8': 'Column 8', 'col9': 'Column 9', 'col10': 'Column 10 dfgsdg sdg sdg  sdgsdgwegsd segsd gsdg serg sdgwsegsdfg sergsdg segsdfgdLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.' },
                                { 'col1': 'data1', 'col2': 'data2', 'col3': 'data3' },
                                { 'col1': 'data41', 'col2': 'data41', 'col3': 'data41' },
                                { 'col1': 'data11', 'col2': 'data21', 'col3': 'data31' },
                                { 'col1': 'data41', 'col2': 'data41', 'col3': 'data41' },
                                { 'col1': 'data12', 'col2': 'data22', 'col3': 'data32' },
                                { 'col1': 'data41', 'col2': 'data41', 'col3': 'data41' }
                            ]
                        }

                        updateData(response);
                    },
                    'beforeDelete': function (rows) {

                    }                    
                });

                /*
                var table = generateTable(data);
                */
                if (context.isLogged()){
                    var deleteButton = $(Templater.useTemplate('delete'));
                         
                    deleteButton.find('#removeSelectedMenu').click(removeSelected);

                    $('.messages').append(deleteButton);
                }
            });
                
            $('.main-content').append(page);
        }
    }
    
    // Update global module reference
    context.currentModule = module;
}());
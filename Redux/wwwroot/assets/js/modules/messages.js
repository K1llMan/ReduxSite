$(function () {
    var dt;

    // Handlers
    function openSteamProfile( steamID ) {
        window.open('https://steamcommunity.com/profiles/' + steamID, '_blank');
    };

    // Module initialization
    var module = {
        'init': function() {
            var page = $('<div></div>');
            var table = $(Templater.useTemplate('table'));

            page.append(table);
            $('.main-content').append(page);

            dt = $('.datatable').datatable({
                'tableHeader': 'Messages',
                'fields': {
                    'comment': {
                        'header': 'Message',
                        'tooltip': 'Message',
                        'hidden': false,
                        'editable': false
                    },
                    'nickname': {
                        'header': 'Nickname',
                        'tooltip': 'Nickname',
                        'hidden': false,
                        'editable': false,
                        'init': function (cell, data) {
                            cell.html(data.nickname);
                            cell.addClass('clickable');
                            cell.click(function() {
                                openSteamProfile(data.steamid);
                            });
                        }
                    },
                    'reply': {
                        'header': 'Reply',
                        'tooltip': 'Reply',
                        'hidden': false,
                        'editable': context.isLogged(),
                        'size': 1000,
                        'afterEdit': function(row) {
                            // Update data
                            $.ajax('api/messages', {
                                type: 'PUT',
                                contentType: "application/json; charset=utf-8",
                                data: JSON.stringify({
                                    ID: row.id,
                                    Reply: row.reply
                                }),
                                success: function (response) {
                                    //...
                                },
                                error: function (jqXHR, exception) {
                                    console.log(exception);
                                }
                            });                                
                        }
                    }
                },
                'hideHeader': false,
                'hideFooter': false,
                'hideSelection': !context.isLogged(),
                'getData': function (page, pageSize, sort, sortDir, updateData) {
                    $.ajax('api/messages', {
                        type: 'GET',
                        success: function (data) {
                            updateData(data);
                        }
                    });
                },
                'beforeDelete': function (rows) {
                    // Delete data
                    $.ajax('api/messages', {
                        type: 'DELETE',
                        data: JSON.stringify({
                            ids: rows.map(function(el, i) {
                                return el.id;
                            })
                        }),
                        success: function (data) {
                        }
                    });
                }                    
            });

            if (context.isLogged()){
                var deleteButton = $(Templater.useTemplate('delete'));
                         
                deleteButton.find('#removeSelectedMenu').click(function() {
                    dt.table.removeSelected();
                });

                page.append(deleteButton);
            }

            // Show table
            context.readyForDisplay(true);
        }
    }
    
    // Update global module reference
    context.currentModule = module;
}());
$(function () {
    var dt;

    function secondsToHms(d) {
        d = Number(d);

        var h = Math.floor(d / 3600);
        var m = Math.floor(d % 3600 / 60);
        var s = Math.floor(d % 3600 % 60);

        return ('0' + h).slice(-2) + ":" + ('0' + m).slice(-2) + ":" + ('0' + s).slice(-2);
    }

    function formatDate(date) {
        var d = new Date(date);
        var month = ('' + (d.getMonth() + 1)).padStart(2, '0');
        var day = ('' + d.getDate()).padStart(2, '0');
        var year = d.getFullYear();

        var hours = ('' + d.getHours()).padStart(2, '0');
        var minutes = ('' + d.getMinutes()).padStart(2, '0');
        var second = ('' + d.getSeconds()).padStart(2, '0');

        return [year, month, day].join('-') + ' ' + [hours, minutes, second].join(':');
    }

    // Module initialization
    var module = {
        'init': function() {
            var page = $('<div></div>');
            
            $.get( "api/messages")
            .done(function( data ) {
                var table = $(Templater.useTemplate('table'));

                page.append(table);

                dt = $('.datatable').datatable({
                    'tableHeader': 'Matches',
                    'fields': {
                        'matchid': {
                            'header': 'Match ID',
                            'tooltip': 'MatchID',
                            'hidden': false,
                            'editable': false
                        },
                        'timestamp': {
                            'header': 'Timestamp',
                            'tooltip': 'Timestamp',
                            'hidden': false,
                            'editable': false,
                            'size': 1000,
                            'init': function (cell, data) {
                                cell.html(formatDate(data.timestamp));
                            }
                        },
                        'duration': {
                            'header': 'Duration',
                            'tooltip': 'Duration',
                            'hidden': false,
                            'editable': false,
                            'init': function (cell, data) {
                                cell.html(secondsToHms(data.duration));
                            }
                        },
                        'gamemode': {
                            'header': 'Gamemode',
                            'tooltip': 'Gamemode',
                            'hidden': false,
                            'editable': false,
                            'size': 1000
                        },
                        'fullInfo': {
                            'header': 'Full Info',
                            'tooltip': 'Show full info',
                            'hidden': false,
                            'editable': false,
                            'size': 1000,
                            'init': function (cell, data) {
                                cell.html('<i class="material-icons">fullscreen</i>');
                                cell.addClass('clickable');
                                cell.click(function () {
                                    
                                });
                            }
                        }
                    },
                    'hideHeader': false,
                    'hideFooter': false,
                    'hideSelection': !context.isLogged(),
                    'getData': function (page, pageSize, sort, sortDir, updateData) {
                        $.ajax('api/matches', {
                            type: 'GET',
                            success: function (data) {
                                updateData(data);
                            }
                        });
                    }                
                });

                // Show table
                context.readyForDisplay(true);
            });
                
            $('.main-content').append(page);
        }
    }
    
    // Update global module reference
    context.currentModule = module;
}());
$(function () {

    function updateRoles() {
        var ids = $('#steamIDs').val().split(',')
            .filter(function(s) {
                return s != '';
            });

        var roles = $('#roles label')
            .has('input:checked')
            .map(function(i, el) {
                return $(el).find('span').html();
            })
            .toArray();

        if (ids.length == 0 || roles.length == 0) {
            Materialize.toast('SteamID list and roles must be set.', 3000);
            return;
        }

        $.ajax('api/players/roles', {
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                'steamID': ids,
                'roles': roles
            }),
            success: function (response) {
                //...
            },
            error: function (jqXHR, exception) {
                console.log(exception);
            }
        });
    }

    var module = {
        'init': function() {
            var page = $(Templater.useTemplate('players-page'));
            $('.main-content').append(page);

            $('#updateRoles').click(updateRoles);

            // Show table
            context.readyForDisplay(true);
        }
    }
    
    context.currentModule = module;
}());
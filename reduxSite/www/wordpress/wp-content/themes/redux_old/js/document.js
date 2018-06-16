var ajaxUrl = "wp-admin/admin-ajax.php";

var replyText = '';
var action = '';

function OpenSteamProfile( steamID ) {
    window.open('http://steamcommunity.com/profiles/[U:1:' + steamID + ']', '_blank');
}

// Submit handler
function SubmitChanges(event){
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

            // Send the data using post
            var posting = $.post( ajaxUrl, {
                    action: action,
                    id: id,
                    reply: reply_value
                });

            break;

        case 'remove_reply':
            // Send the data using post 
            var posting = $.post( ajaxUrl, {
                    action: action,
                    id: id
                });

            posting.done(function( data )
            {
                $('#id_' + id).parent().remove();
            });

            break;
    }

    setTimeout(function(){
        $('#id_' + id).text(reply_value);
    }, 10);

    var replyForm = $("#submitReply").detach();
    $('#hiddenDiv').append(replyForm);

}

// Build file upload
function readSingleFile(e) {
    var file = e.target.files[0];
    if (!file)
        return;

    var reader = new FileReader();
    reader.onload = function(e) {
        var posting = $.post( ajaxUrl, {
            action: 'upload_builds',
            builds_data: e.target.result
        });

        posting.done(function(data)
        {
            $('#file-input').val('');
            Materialize.toast('Uploading done!', 2000);
        });
    };
    reader.readAsText(file);
}

$( document ).ready(function(){
    $('#file-input').change(readSingleFile);

    $('textarea#textarea').characterCounter();

    $('[data-steamid]').click(function(){
        OpenSteamProfile( $(this).attr('data-steamid') );
    });

    $('[id ^= id_').click(function(){
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


    $('#sendButton').click(function(){
        action = 'add_reply';
        SubmitChanges(event);
    });

    $('#removeButton').click(function() {
        action = 'remove_reply';
        SubmitChanges(event);
    });

    $('#cancelButton').click(function() {
        SubmitChanges(event);
    });

    $('#removeSelectedMenu').click(function(){
        var checked_list = $('[id ^= check ]:checked');
        if (checked_list.length == 0)
            return;

        // Create ids array
        var id_list = $.map(
            checked_list, 
            function(val, i) { return val.id.replace(/check_/, ''); }
        );
        
        // Send the data using post 
        var posting = $.post( ajaxUrl, {
                action: 'remove_selected',
                id_list: id_list
            });

        posting.done(function( data )
        {
            $.each(checked_list, function(index, item){
                $(item).parents('tr').remove();
            });
        });
    });
})
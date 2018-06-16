/*-----------------------------------------------------------------------------
                             JWT Auth module (jQuery 3.3.1)
-----------------------------------------------------------------------------*/

// Adding JWT header
$.ajaxSetup({
    beforeSend: function (xhr) {
        if (localStorage.getItem("token") !== null) {
            xhr.setRequestHeader('Authorization', 'Bearer ' + localStorage.getItem("token"));
        }
    }
});

function Authorization(user, pass) {
    $.post("api/token", $.param({ username: user, password: pass }))
        .done(function (token) {
            //save the token in local storage
            localStorage.setItem("token", token);
            //...
            //getModules();
        })
        .fail(function () {

        });
}

Authorization("Admin", "Admin");
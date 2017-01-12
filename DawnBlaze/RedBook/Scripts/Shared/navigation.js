$(document).ready(function(){
    navigation_object.activityListener();
});

var navigation_object = function(){

    function activityListener(){
        $('#test-logout').click(function(){
            logoutUser();
        });
    }

    function logoutUser(){
        $.ajax({
            url: 'Login/Logout',
            type: 'POST',
            contentType: false,
            processData: false,
            async: true,
            cache: false,
            success: function(data) {
                // Success Code
                location.reload(true);
            },
            error: function() {
                // Error Code
                // Create "Something's wrong popup"
            }
        });
    }

    return{
        'activityListener': activityListener
    }

}();
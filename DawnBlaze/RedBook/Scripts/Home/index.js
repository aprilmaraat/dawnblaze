$(document).ready(function(){
    home_object.activityListener();
});

var home_object = function(){

    function activityListener(){
        
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
                layout_object.toggleLoading();
            },
            error: function() {
                // Error Code
                // Create "Something's wrong popup"
                layout_object.toggleLoading();
            }
        });
    }

    return{
        'activityListener': activityListener
    }

}();
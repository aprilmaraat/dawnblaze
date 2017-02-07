$(document).ready(function(){
    // alert("Uvuvwevwevwe Onyetenyevwe Ugwemubwem Ossas");
    login_object.activityListener();
});

var login_object = function(){

    function activityListener(){
        loginListener();
    }

    function loginListener(){
        var login_button = $('.login-button');

        login_button.click(function(){
            isLoginError(false);
            layout_object.toggleLoading('Logging in...');

            var username = $('#username').val();
            var password = $('#password').val();

            if(username != "" && username != undefined && 
                password != "" && password != undefined){
                
                var login = {
                    'Username': username,
                    'Password': password
                };

                var formData = new FormData();
                formData.append('login', login);

                $.ajax({
                    url: 'Login/ValidateCredentials',
                    data: login,
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function(data) {
                        // Success Code

                        if(data == true){
                            window.location.href = '/Home';
                        }
                        else{
                            isLoginError(true);
                            layout_object.toggleLoading();
                        }

                    },
                    error: function() {
                        // Error Code
                        alert('Error. Unable to connect to server. Try again later.');
                        layout_object.toggleLoading();
                    }
                });

            }
            else{
                isLoginError(true);
                layout_object.toggleLoading();
            }

        });
    }

    

    function isLoginError(bool){
        var credential_error = $('.credential-error');

        if(bool == true){
            credential_error.removeClass('hidden');
        }
        else{
            credential_error.addClass('hidden');
        }
    }

    return{
        'activityListener': activityListener,
    }

}();


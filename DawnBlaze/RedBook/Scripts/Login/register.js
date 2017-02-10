$(document).ready(function(){
    register_object.activityListener();
});

var register_object = function(){

    function activityListener(){
        registerListener();
    }

    function registerListener(){
        var register_button = $('.register-button');

        register_button.click(function(){
            layout_object.toggleLoading("Creating account...");

            var username_element = $('#username');
            var email_element = $('#email');
            var password_element = $('#password');
            var password_confirm_element = $('#password-confirm');

            var register = {
                'Username': username_element.val(),
                'Password': password_element.val(),
                'ConfirmPassword': password_confirm_element.val(),
                'Email': email_element.val()
            };

            $.ajax({
                // url: 'Login/RegisterJson',
                url: 'RegisterJson',
                data: register,
                type: 'POST',
                async: true,
                cache: false,
                success: function(data) {
                    // Success Code

                    if(data.isSuccess == true){
                        // Redirect to success registration page and display instruction
                        // An confirmation link has been sent to you email.
                        // For now, redirect to Login
                        alert(data.isSuccess);
                        // window.location.href = '/Login';
                    }
                    else{

                        if(data.error.Username != ''){
                            registerCredentialError('username', true, data.error.Username);
                        }
                        else{
                            registerCredentialError('username', false);
                        }

                        if(data.error.Password != ''){
                            registerCredentialError('password', true, data.error.Password);
                        }
                        else{
                            registerCredentialError('password', false);
                        }

                        if(data.error.Email != ''){
                            registerCredentialError('email', true, data.error.Email);
                        }
                        else{
                            registerCredentialError('email', false);
                        }

                        if(data.error.ConfirmPassword != ''){
                            registerCredentialError('password-confirm', true, data.error.ConfirmPassword);
                        }
                        else{
                            registerCredentialError('password-confirm', false);
                        }

                        layout_object.toggleLoading();
                    }

                },
                error: function() {
                    // Error Code
                    alert('Error. Unable to connect to server. Try again later.');
                    layout_object.toggleLoading();
                }
            });

        });
    }

    function registerCredentialError(fieldname, isError, errorText){

        switch(fieldname) {
            case 'username':
                
                if(isError){
                    $('#username').addClass('input-error');
                    $('#username').parent().find('.credential-error').find('.error-message').text(errorText);
                    $('#username').parent().find('.credential-error').removeClass('hidden');
                }
                else{
                    $('#username').removeClass('input-error');
                    $('#username').parent().find('.credential-error').find('.error-message').text("");
                    $('#username').parent().find('.credential-error').addClass('hidden');
                }

                break;
            case 'email':
                
                if(isError){
                    $('#email').addClass('input-error');
                    $('#email').parent().find('.credential-error').find('.error-message').text(errorText);
                    $('#email').parent().find('.credential-error').removeClass('hidden');
                }
                else{
                    $('#email').removeClass('input-error');
                    $('#email').parent().find('.credential-error').find('.error-message').text("");
                    $('#email').parent().find('.credential-error').addClass('hidden');
                }

                break;
            case 'password':
                
                if(isError){
                    $('#password').addClass('input-error');
                    $('#password').parent().find('.credential-error').find('.error-message').text(errorText);
                    $('#password').parent().find('.credential-error').removeClass('hidden');
                }
                else{
                    $('#password').removeClass('input-error');
                    $('#password').parent().find('.credential-error').find('.error-message').text("");
                    $('#password').parent().find('.credential-error').addClass('hidden');
                }

                break;
            case 'password-confirm':

                if(isError){
                    $('#password-confirm').addClass('input-error');
                    $('#password-confirm').parent().find('.credential-error').find('.error-message').text(errorText);
                    $('#password-confirm').parent().find('.credential-error').removeClass('hidden');
                }
                else{
                    $('#password-confirm').removeClass('input-error');
                    $('#password-confirm').parent().find('.credential-error').find('.error-message').text("");
                    $('#password-confirm').parent().find('.credential-error').addClass('hidden');
                }

                break;
            default:
                break;
        }

    }

    return{
        activityListener: activityListener
    }

}();
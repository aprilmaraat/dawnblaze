$(document).ready(function(){
    forgot_password_object.activityListener();
});

var forgot_password_object = function(){

    function activityListener(){
        forgotPasswordListener();
    }

    function forgotPasswordListener(){
        var forgot_password_button = $('.forgot-password-button');

        forgot_password_button.click(function(){
            layout_object.toggleLoading("Submitting credentials.");
            isCredentialInvalid(false);
            var username_element = $('#username');
            var email_element = $('#email');

            var forgot_password = {
                'Username': username_element.val(),
                'Email': email_element.val()
            };

            $.ajax({
                url: 'ForgotPasswordJson',
                data: forgot_password,
                type: 'POST',
                async: true,
                cache: false,
                success: function(data) {
                    // Success Code

                    if(data.isSuccess){
                        // For now use alert, should be redirected to success page.
                        alert('Forgot password request sent.');
                        location.reload();
                    }
                    else{

                        if(data.error != undefined){

                            if(data.error.Username != ''){
                                forgotPasswordCredentialError('username', true, data.error.Username);
                            }
                            else{
                                forgotPasswordCredentialError('username', false);
                            }

                            if(data.error.Email != ''){
                                forgotPasswordCredentialError('email', true, data.error.Email);
                            }
                            else{
                                forgotPasswordCredentialError('email', false);
                            }

                        }
                        else{
                            isCredentialInvalid(true);
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

    function forgotPasswordCredentialError(fieldname, isError, errorText){
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
        }
    }

    function isCredentialInvalid(bool){
        var credential_invalid = $('.credential-invalid');

        if(bool == true){
            credential_invalid.removeClass('hidden');
        }
        else{
            credential_invalid.addClass('hidden');
        }
    }

    return {
        activityListener: activityListener
    }

}();
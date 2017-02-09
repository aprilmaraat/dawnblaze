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
                'ConfirmAddress': password_confirm_element.val(),
                'Email': email_element.val()
            };

            // if(username_element.val() == '' || username_element.val() == undefined){
            //     registerCredentialError('username', true);
            // }
            // else{
            //     registerCredentialError('username', false);
            // }

            // if(email_element.val() == '' || email_element.val() == undefined){
            //     registerCredentialError('email', true);
            // }
            // else{
            //     registerCredentialError('email', false);
            // }

            // if(password_element.val() == '' || password_element.val() == undefined){
            //     registerCredentialError('password', true);
            // }
            // else{
            //     registerCredentialError('password', false);
            // }

            $.ajax({
                // url: 'Login/RegisterJson',
                url: 'RegisterJson',
                data: register,
                type: 'POST',
                async: true,
                cache: false,
                success: function(data) {
                    // Success Code

                    // alert(JSON.stringify(data.error));

                    alert(JSON.stringify(data));

                },
                error: function() {
                    // Error Code
                    alert('Error. Unable to connect to server. Try again later.');
                }
            });
            
            layout_object.toggleLoading();

        });
    }

    function registerCredentialError(fieldname, isError){

        switch(fieldname) {
            case 'username':
                
                if(isError){
                    $('#username').addClass('input-error');
                    $('#username').parent().find('.credential-error').removeClass('hidden');
                }
                else{
                    $('#username').removeClass('input-error');
                    $('#username').parent().find('.credential-error').addClass('hidden');
                }

                break;
            case 'email':
                
                if(isError){
                    $('#email').addClass('input-error');
                    $('#email').parent().find('.credential-error').removeClass('hidden');
                }
                else{
                    $('#email').removeClass('input-error');
                    $('#email').parent().find('.credential-error').addClass('hidden');
                }

                break;
            case 'password':
                
                if(isError){
                    $('#password').addClass('input-error');
                    $('#password').parent().find('.credential-error').removeClass('hidden');
                }
                else{
                    $('#password').removeClass('input-error');
                    $('#password').parent().find('.credential-error').addClass('hidden');
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
var layout_object = function(){

    function toggleLoading(loading_message){
        
        var loading_container = $('.loader-container');

        if(loading_message != undefined){
            $('.loader-label').text(loading_message);
        }
        else{
            $('.loader-label').text('');
        }

        if(loading_container.hasClass('hidden')){
            loading_container.removeClass('hidden');
        }
        else{
            loading_container.addClass('hidden');
        }

    }

    return{
        'toggleLoading': toggleLoading
    }

}();
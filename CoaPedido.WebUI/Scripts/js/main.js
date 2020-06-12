$(function(){
    $('.hamb').on('click', function() {
        $('.hamb').toggleClass("on");
        
        $('.menu-mobile').slideToggle(300);
       
    });

    $('li.flex').on('click',function(){
        $('ul', this).slideToggle(300);
        $('i', this).toggleClass('cat-ativo');
    });
    /*$( window ).scroll(function() {
        if ($(document).scrollTop()  <= 200){  
            $('#para-topo').removeClass("fixo");
            console.log("aaa");
        }
        else{
            $('#para-topo').addClass("fixo");
        }
    });*/
    
});
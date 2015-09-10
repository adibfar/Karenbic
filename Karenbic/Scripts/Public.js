"use strict";

jQuery(document).ready(function($) {
    
    if (Function('/*@cc_on return document.documentMode===10@*/')()) {
        document.documentElement.className += ' ie10';
    }

    //$('*').on('contextmenu', function () { return false; });

    /*=-=-=-=-=-=-=-=-=-= Start Search Box Effect Handler =-=-=-=-=-=-=-=-=-=*/

    //click
    $('.searchbox .searchbox-icon,.searchbox .searchbox-inputtext').bind('click', function() {
        var $search_tbox = $('.searchbox .searchbox-inputtext');
        $search_tbox.css('width', '120px');
        $search_tbox.focus();
        $('.searchbox', this).addClass('searchbox-focus');
    });

    //blur
    $('.top-bar .searchbox-inputtext,body').bind('blur', function() {
        var $search_tbox = $('.searchbox .searchbox-inputtext');
        $search_tbox.css('width', '0px');
        $('.searchbox', this).removeClass('searchbox-focus');
    });

    /*=-=-=-=-=-=-=-=-=-= End Search Box Effect Handler =-=-=-=-=-=-=-=-=-=*/

    /*=-=-=-=-=-=-=-=-=-= Start Mobile Navigation =-=-=-=-=-=-=-=-=-=*/

    $('.header .mobile-nav ').append($('.navigation').html());

    $('.header .mobile-nav li').bind('click', function(e) {

        var $this = $(this);
        var $ulKid = $this.find('>ul');
        var $ulKidA = $this.find('>a');

        if ($ulKid.length === 0 && $ulKidA[0].nodeName.toLowerCase() === 'a') {
            window.location.href = $ulKidA.attr('href');
        }
        else {
            $ulKid.toggle(0, function() {
                if ($(this).css('display') === 'block') {
                    $ulKidA.find('.icon-chevron-down').removeClass('icon-chevron-down').addClass('icon-chevron-up');
                }
                else {
                    $ulKidA.find('.icon-chevron-up').removeClass('icon-chevron-up').addClass('icon-chevron-down');
                }
            });
        }

        e.stopPropagation();

        return false;
    });

    $('.mobile-menu-button').click(function() {
        $('.mobile-nav').toggle();
    });

    $('.header .mobile-nav .icon-chevron-right').each(function() {
        $(this).removeClass('icon-chevron-right').addClass('icon-chevron-down');
    });

    /*=-=-=-=-=-=-=-=-=-= End Mobile Navigation =-=-=-=-=-=-=-=-=-=*/

    //place holder fallback
    //$('input, textarea').placeholder();


    //Callout Box And Message Box Mobile Button
    $('.message-box ,.callout-box').each(function() {
        var $box = $(this);
        var $button = $box.find(".btn");
        $box.append('<button href="' + $button.attr("href") + '" class="' + $button.attr("class") + ' btn-mobile">' + $button.html() + '</button>');

    });

    //stickyMenu();

    if ($("html").hasClass("lt-ie9")) {

        //bread crumb last child fix for IE8
        $('.breadcrumbs li:last-child').addClass('last-child');
        $('.navigation > li:last-child').addClass('last-child-nav');
        $('.flickr_badge_wrapper .flickr_badge_image').addClass('flicker-ie');
        $('.flickr_badge_wrapper .flickr_badge_image:nth-child(3n+1)').addClass('last-child-flicker');
        $('.content-style3 ').css('width', '100%').css('width', '-=28px');
        $('.section-subscribe input[type=text]').css('width', '100%').css('width', '-=40px');
        $('.blog-search .blog-search-input').css('width', '100%').css('width', '-=40px');
    }


});


/* Portfolio */

var loaded = false, timeout = 20000;//loaded flag for timeout
setTimeout(function() {
    if (!loaded) {
        hideLoading();
    }
}, timeout);

$(window).load(function() {
    loaded = true;

    hideLoading();
});


/* Loading functions */
function hideLoading() {
    $('.loading-container').remove();
    $('.hide-until-loading').removeClass('hide-until-loading');
}
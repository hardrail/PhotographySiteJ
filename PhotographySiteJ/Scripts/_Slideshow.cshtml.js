/// <reference path = "/Scripts/jquery-1.5.1-vsdoc.js"/>
var images = [];
var totalImages;
var currentImage = 1;
var initialLoad = true;

$(document).ready(function () {
    var hideSlideshow = $('#hide-slideshow-lbl');

    $(hideSlideshow).click(function () {
        if ($('#slideshow-container').is(':hidden')) {
            $('#slideshow-container').slideDown('fast');
            $(hideSlideshow).html('Hide slideshow [-]');
        }
        else {
            $('#slideshow-container').slideUp('fast');
            $(hideSlideshow).html('Show slideshow [+]');
        }
    })

    $(hideSlideshow).hover(function () {
        $(hideSlideshow).css('text-shadow', '0px 0px 10px');
    })
    $(hideSlideshow).mouseleave(function () {
        $(hideSlideshow).css('text-shadow', '');
    })

    //Loop through each image element on the page and add it to the images array which we will iterate through
    //producing a slideshow effect
    //==============================================
    $('img').each(function () {
        images.push($(this).attr('data-fullpath'));
    })
    //==============================================

    //Decrement images length to get the true number of images in array, this is used to tell the slideshow
    //when to revert back to the first image
    //==============================
    totalImages = images.length - 1;
    //==============================

    //This function will set the slideshow interval to 3 seconds
    //In addition, it will set the slideshow img element to the next one
    //in the images array. When the function realizes it is at the end of the array it will go to the next img in the array "images"
    //==================================================================================
    setInterval(function () {
        if (initialLoad) {
            $('#slideshowimage').attr('src', images[currentImage]).show();
            $('#slideshow-container').slideDown('slow');
            $('#hide-slideshow').show();
            initialLoad = false;
            currentImage++;
        }
        else {
            $('#slideshowimage').hide().attr('src', images[currentImage]).fadeIn(1000);

            if (currentImage == totalImages)
                currentImage = 1;
            else
                currentImage++;
        }
    }, 3000);
    //===================================================================================
});

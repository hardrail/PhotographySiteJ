/// <reference path = "/Scripts/jquery-1.5.1-vsdoc.js"/>
/// <reference path = "/Scripts/jquery-ui-1.8.14.custom.min.js"/>
var hoveredPicture = null;
var currentThumbHeight = 60;
var currentThumbWidth = 60;

$(document).ready(function () {
    $('#IndexPictureList img').each(function () {
        $(this).mouseover(function () {
            if ($(hoveredPicture) != null) {
                if ($(hoveredPicture).attr('src') != $(this).attr('src')) {
                    $(hoveredPicture).stop().animate({ height: currentThumbHeight, width: currentThumbWidth }, 'fast');
                    hoveredPicture = $(this);

                    $(this).stop().animate({ height: currentThumbHeight + 15, width: currentThumbWidth + 15 }, 'fast');

                }
            }
        })
    })

    //Set slider
    $('#slider').slider({
        min: 20,
        max: 110,
        step: 5,
        value: 60,
        slide: function (event, ui) {
            currentThumbHeight = ui.value;
            currentThumbWidth = ui.value;
            $('#IndexPictureList img').animate({ height: currentThumbHeight, width: currentThumbWidth }, 'fast');
        }
    });

    $('.ui-slider').css({ width: '300px' });
    $('.ui-slider .ui-slider-handle').css({ width: '15px' });

    $('#IndexPictureList').mouseleave(function () {
        $(hoveredPicture).animate({ height: currentThumbHeight, width: currentThumbWidth }, '25');
        hoveredPicture = null;
    })
});

function getFileName(path) {
    return path.match(/[-_\w]+[.][\w]+$/i)[0];
}
/// <reference path = "/Scripts/jquery-1.5.1-vsdoc.js"/>
var hoveredPicture = null;
var fullSizePicPath = '../../Content/Images/FrontGallery/';

$(document).ready(function () {
    $('#IndexPictureList img').each(function () {
        $(this).click(function () {
//            var img_name = fullSizePicPath + getFileName($(this).attr('src'));

//            //remove existing image elements
//            $('#SelectedPic').remove();

//            //create new image element and appened to animation div container
//            $(document.createElement('img'))
//                        .attr({ src: img_name, title: getFileName($(this).attr('src')), id: 'SelectedPic' })
//                        .appendTo('#aniContain');

//            //if aniContain was hidden reshow
//            $('#aniContain').show();

//            //set a couple style attributes and then animate
//            $('#SelectedPic').stop().show().animate({ height: '50%', width: '50%' }, 'medium');

//            //set image to close itself
//            $('#SelectedPic').click(function () {
//                $('#aniContain').hide();
//            })

        })

        $(this).mouseover(function () {
            if ($(hoveredPicture) != null) {
                if ($(hoveredPicture).attr('src') != $(this).attr('src')) {
                    $(hoveredPicture).stop().animate({ height: '60', width: '60' }, 'fast');
                    hoveredPicture = $(this);

                    $(this).stop().animate({ height: '75', width: '75' }, 'fast');

                }
            }
        })
    })

    $('#IndexPictureList').mouseleave(function () {
        $(hoveredPicture).animate({ height: '60', width: '60' }, 'fast');
        hoveredPicture = null;
    })

});

function getFileName(path) {
    return path.match(/[-_\w]+[.][\w]+$/i)[0];
}
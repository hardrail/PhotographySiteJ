/// <reference path = "/Scripts/jquery-1.5.1-vsdoc.js"/>
var tempImgDel = null;

$(document).ready(function () {
    $(document).ready(function () {
        $('input[type=file]').live('change', function () {
            var fileInput = $(this);

            //Make a call to controller function that will determine if the user is trying to upload a picture into a
            //gallery where the picture already exists. If it does we will display a snippet next to the control
            //saying that the image already exists
            //==============================================================================================
            $.ajax({
                type: 'GET',
                traditional: true,
                url: '/Account/ajaxFileExists/',
                data: {
                    //set the values of the parameters in the controller function
                    gallery: $('#gallery-combo').val(),
                    file: $(fileInput).val()
                },
                dataType: 'json',
                success: function (returnJ) {
                    if (returnJ.toggle == true) {
                        //next here is giving us the browse button next to the input control. It is here that we
                        //want to display the message that the image already exists
                        $(fileInput).next().html('that image already exists in that gallery').fadeIn('slow');
                    }
                    else {
                        //if the image doesn't already exist and the message saying it does from an old attempt
                        //is still shown, we are going to fade out that message
                        if (!$(fileInput).is(':hidden')) {
                            $(fileInput).next().fadeOut('slow');
                        }
                    }

                    checkBlankUploaders(0);
                },
                error: function () {
                    alert('fail');
                }
                //===========================================================================================
            });
        })

        $('#add-fileupload').live('click', function () {
            checkBlankUploaders(1);
        })
    });

    $('#admin-gallery-table img').each(function () {
        $(this).click(function (e) {

            $('#del-menu').show('normal');
            $('#del-menu').css({ 'top': e.pageY - 10, 'left': e.pageX - 10 });

            //set the temp var if the user chooses to delete
            tempImgDel = $(this);
        })
    })

    $('#del-menu #del-menu-delete').click(function () {
        $('#del-menu').hide();

        var clickedRow = $(tempImgDel).closest('tr');
        var image = $(clickedRow).find('#actual-path');

        //If the user wants to remove an image from a gallery, we will do this
        //asynchronously by calling the "DeleteImage" method in our controller
        $.ajax({
            url: '/Account/DeleteImage/',
            data: { id: $(image).html() },
            type: 'POST',
            success: function () {
                //if no errors, we are simply going to fade out the image
                $(clickedRow).fadeOut(300, function () { $(this).remove(); });
            },
            error: function () {
                $('#message').html('There was an unexpected error.').show();
            }
        });
    })

    $('#gallery-filter').change(function () {
        var clickedGallery = $('#gallery-filter').val();

        $.ajax({
            url: '/Account/Admin/',
            traditional: true,
            type: 'POST',
            dataType: 'html',
            data: { gallery: clickedGallery },
            success: function (data) {
                $('#gallery-div').html(data);
            }
        });
    })

    $('#del-menu #del-menu-exit').click(function () {
        $('#del-menu').hide();
    })

    $('#admin-gallery-table').tablesorter({ widthFixed: true, sortList: [[0, 0]] }).tablesorterPager({ container: $('#pager'), size: $('.pagesize option:selected').val() });
});

function checkBlankUploaders(addhtml) {
    var blankInputs = 0;

    $('input[type=file]').each(function () {
        if ($(this).val() == "") {
            blankInputs++;
        }
    })

    if (blankInputs == 0) {
        //            var numFileUploaders = $('#upload-controls').find('input[type=file]').length + 1;
        //            var newFileID = "file" + numFileUploaders.toString();
        var addFileUploadHtml = '<p><label for="file-upload">Choose an image to upload: </label>';
        addFileUploadHtml += '<input type="file" id="file" value="...browse" name="file" />';
        addFileUploadHtml += '<label class="errors" id="existsError"></label></p>';

        if (addhtml == 1) {
            $(addFileUploadHtml).appendTo('#upload-controls').hide().fadeIn('slow');
            $('#submit').attr('disabled', 'disabled');
        }
        else {
            $('#submit').attr('disabled', '');
        }

        $('#click-label').hide().html('(click to add another upload control)').css('color', 'black').fadeIn('normal');
    }
    else {
        $('#click-label').hide().html('there is already an empty file uploader').css('color', 'red').fadeIn('normal');
        $('#submit').attr('disabled', 'disabled');
    }
}
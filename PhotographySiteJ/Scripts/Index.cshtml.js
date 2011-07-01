/// <reference path = "/Scripts/jquery-1.5.1-vsdoc.js"/>
var hoveredPicture = null;
var fullSizePicPath = '../../Content/Images/FrontGallery/';

$(document).ready(function () {


});

function getFileName(path) {
    return path.match(/[-_\w]+[.][\w]+$/i)[0];
}
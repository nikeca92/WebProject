function ShowImportFileFormatInfoLightBox() {
    //shows the light box and "disable" rest of the page
    document.getElementById("HidePage").style.display = "";
    document.getElementById("lboxImportFileFormatInfo").style.display = "";
    CenterLightBox("lboxImportFileFormatInfo");
}

//Close the light-box
function HideImportFileFormatInfoLightBox() {
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxImportFileFormatInfo").style.display = "none";
}
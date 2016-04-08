$(function () {
    $('body').on('change', '.file', function () {
        //$("#file").on("change", function () {
        console.log("hola from file");
        var file = this.files[0];
        var imagefile = file.type;
        var match = ["image/jpeg", "image/png", "image/jpg"];
        if (!((imagefile == match[0]) || (imagefile == match[1]) || (imagefile == match[2]))) {
            $('#previewing').attr('src', '../img/no-preview-available.png');
            console.log('<p>Please Select A valid Image File</p>' + '<h4>Note</h4>' + '<span id="error_message">Only jpeg, jpg and png Images type allowed</span>');
            return false;
        }
        else {
            var reader = new FileReader();
            reader.onload = imageIsLoaded;
            reader.readAsDataURL(this.files[0]);
        }
    });
});

function imageIsLoaded(e) {
    $("#file").css("color", "green");
    $('#image_preview').css("display", "block");
    $('#previewing').attr('src', e.target.result);
    $('#previewing').attr('width', 'auto');
    $('#previewing').attr('height', '213px');
};
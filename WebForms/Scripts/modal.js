$(function () {
    function msgBox(message) {
        $('#modal').load('~/MessageBox.aspx?message=' + message);

        var modalMarginTop = ($('#modal').height() + 80) / 2;
        var modalMarginLeft = ($('#modal').width() + 80) / 2;

        $('#modal').css({
            'margin-top': -modalMarginTop,
            'margin-left': -modalMarginLeft
        });

        $('#modal').toggle(800);
        $('body').append('<div id="modal-shade"></div>');
        $('#modal-shade').css('opacity', 0.7).fadeIn();
    }

    $('#msg-box-cancel').live('click', function () {
        $('#modal-shade, #modal').toggle(800, function () {
            $('#modal-shade').remove();
        });
        return false;
    });
});
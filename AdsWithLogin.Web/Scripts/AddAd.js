$(function () {
    $('#form').on('keyup', function () {
        console.log("title: " + $('#title').val());
        console.log("des: " + $('#description').val());
        console.log("pic: " + $('#picture').val());
        if ($('#title').val() === 0 || $('#description').val() === 0 || $('#picture').val() === null) {
            $('#btnAdd').prop('disabled', true);
        } else {
            $('#btnAdd').prop('disabled', false);
        }
    });
});
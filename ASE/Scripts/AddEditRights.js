function CheckAll(id) {
    if ($('.select' + id + ':checked').length === 0) {
        $('.selectall' + id).
        prop("indeterminate", false).
        prop('checked', false);
    } else if ($('.select' + id + ':not(:checked)').length === 0) {
        $('.selectall' + id).
        prop("indeterminate", false).
        prop('checked', true);
    } else {
        $('.selectall' + id).
        prop("indeterminate", true);
    }
}


$(function () {
    $('.select_all').change(function () {
        var id = $(this).attr('data-id');
        $('.select' + id).prop('checked', $(this).prop('checked'));
    });

    $('.select_one').change(function () {
        var id = $(this).attr('data-id');

        CheckAll(id);
    });

    $('.select_all').each(function () {
        var id = $(this).attr('data-id');

        CheckAll(id);
    });
});


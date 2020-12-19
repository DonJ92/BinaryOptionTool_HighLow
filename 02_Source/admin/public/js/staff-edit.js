var isRtl;

$(function () {
    isRtl = $('body').attr('dir') === 'rtl' || $('html').attr('dir') === 'rtl';

    // Date
    $('#birthday').datepicker({
        orientation: isRtl ? 'auto right' : 'auto left',
        format: 'yyyy-mm-dd',
    });
});

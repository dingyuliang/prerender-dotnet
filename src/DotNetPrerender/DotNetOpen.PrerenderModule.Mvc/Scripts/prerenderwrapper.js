(function (window, $) {
    $.extend($, {
        prerenderInit: function () {
            window.prerenderReady = false;
        },
        prerenderReady: function () {
            window.prerenderReady = true;
        }
    });

    $.prerenderInit();
})(window, jQuery)

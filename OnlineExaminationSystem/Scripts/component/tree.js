(function () {
    $('.panel.tree').each(function () {
        var $pnl = $(this);
        $pnl.find('.panel-heading').first().click(function () {
            $pnl.toggleClass('minimize', 500);
        });
    });
})();
var OneHUDDashEditor = function () {
    'use strict'

    var _gridSize = 10;

    function start() {
        drawGrid();
    }

    function stop() {
        deleteGrid();
    }

    function drawGrid(el) {

        if (el === undefined) {
            el = jQuery('#content');
        }
        var height = el.height();
        var width = el.width();
        var ratioW = Math.floor(width / _gridSize);
        var ratioH = Math.floor(height / _gridSize);

        deleteGrid();

        for (var i = 0; i <= ratioW; i++) { // vertical grid lines
            jQuery('<div />').css({
                'top': 0,
                'left': i * _gridSize,
                'width': 1,
                'height': height
            })
              .addClass('gridlines')
              .appendTo(el);
        }

        for (i = 0; i <= ratioH; i++) { // horizontal grid lines
            jQuery('<div />').css({
                'top': i * _gridSize,
                'left': 0,
                'width': width,
                'height': 1
            })
              .addClass('gridlines')
              .appendTo(el);
        }

        jQuery('.gridlines').show();
    }

    function deleteGrid() {
        jQuery('.gridlines').remove();
    }

    return {

        start: function () {
            return start();
        },

        stop: function () {
            return stop();
        }
    }
}();
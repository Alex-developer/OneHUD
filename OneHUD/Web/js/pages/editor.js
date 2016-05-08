var OneHUDDashEditor = function () {
    'use strict'

    var _gridSize = 10;
    var _dashInstance = null;
    var _savedDash = null;

    function start() {
        _dashInstance = OneHUDUI.getCurrentPage();
        var _savedDash = jQuery.extend(true, {}, _dashInstance.getDash());
        jQuery('#editor-save-dash').removeClass('disabled');
        jQuery('#editor-cancel').removeClass('disabled');
        drawGrid();
        startWidgets();
    }

    function stop() {
        jQuery('#editor-save-dash').addClass('disabled');
        jQuery('#editor-cancel').addClass('disabled');

        deleteGrid();
        stopWidgets();
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

    function startWidgets() {
        var _dash = _dashInstance.getDash();

        jQuery.each(_dash, function (pos, widget) {
            if (widget.element !== undefined) {
                var el = widget.element();
                jQuery(el).draggable();
                jQuery(el).resizable({
                    resize: function (event, ui) {
                        if (widget.resize !== undefined) {
                            widget.resize();
                        }
                    },
                    stop: function (event, ui) {
                    }
                });
                jQuery(el).addClass('border');

            }

            if (widget.startEditing !== undefined) {
                widget.startEditing();
            }
        })
    }

    function stopWidgets() {
        var _dash = _dashInstance.getDash();

        jQuery.each(_dash, function (pos, widget) {
            if (widget.element !== undefined) {
                var el = widget.element();
                jQuery(el).draggable('destroy');
                jQuery(el).resizable('destroy');
                jQuery(el).removeClass('border');
            }
            if (widget.stopEditing !== undefined) {
                widget.stopEditing();
            }
        })
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
var AGRacingViewDash = function () {
    'use strict'

    var _name = 'Dash';
    var _icon = 'images/pages/dash.png';
    var _description = 'The AGServer dashboard. This is the interesting bit where you can create and use your own custom dashboards using the built in editor';
    var _showVideo = false;
    var _order = 2;

    var _dashJson = {};
    var _dash = [];
    var _dashLoaded = false;

    var _defaultDashboard = {
        name: 'Default',
        device: 'ipad3',
        orientation: 'landscape',
        widgets: [
            {
                type: 'image',
                css: {
                    left: 0,
                    top: 0,
                    width: 614,
                    height: 324
                }
            },
            {
                type: 'rpm',
                gaugestyle: 'analogue',
                css: {
                    left: 156,
                    top: 10,
                    width: 300,
                    height: 235,
                    'font-family': 'ledfont',
                    'font-weight': 'bold',
                    color: 'white'
                }
            },
            {
                type: 'speed',
                gaugestyle: 'analogue',
                css: {
                    left: 310,
                    top: 0,
                    width: 300,
                    height: 300,
                    'font-family': 'ledfont',
                    'font-weight': 'bold',
                    color: 'white'
                }
            },
            {
                type: 'gear',
                css: {
                    left: 285,
                    top: 90,
                    width: 50,
                    height: 50,
                    'font-family': 'led',
                    'font-weight': 'bold',
                    color: 'white'
                }
            }
        ]
    };

    console.log(JSON.stringify(_defaultDashboard));

    function init() {
        var deferred = jQuery.Deferred();
        loadWidgets().done(function () {
            AGServerFile.loadDash().done(function (dash) {
                if (dash !== null) {
                    _dashJson = dash;
                } else {
                    _dashJson = _defaultDashboard;
                }
                buildUI();
            });
        });
        return deferred.promise();
    }

    function loadWidgets() {
        var deferred = jQuery.Deferred();

        var options = AGServerUI.options();
        var widgetsPaths = [];
        jQuery.each(options.Widgets, function (i, widgetInfo) {
            widgetsPaths.push(widgetInfo.Path);
        });

        head.load(widgetsPaths, function () {
            deferred.resolve();
        });

        return deferred.promise();
    }

    function buildUI() {
        buildToolbar();
        buildDash();
    }

    function buildDash() {
        _dash = [];
        jQuery.each(_dashJson.widgets, function (index, widget) {
            var widgetName = widget.type;
            var widgetClass = 'AGDash' + widgetName.toUpperCase() + 'Widget';

         //   try {
                var widgetController = new window[widgetClass]();

                if (widget.css === undefined) {
                    widget.css = {};
                }

                widget.css['position'] = 'absolute';
                widget.css['zindex'] = 100;

                var element = jQuery('<div>').css(widget.css)
                    .addClass('widget agselectable')
                    .attr('id', AGServerUI.getNextId())
                jQuery('#content').append(element);
                element.data('type', widgetController.name);

                widgetController.init(element, widget);
                _dash.push(widgetController);
         //   } catch (error) {
         //       console.log(error);
         //   }
        });
        _dashLoaded = true;
    }

    function buildToolbar() {
        var template = '\
            <i class="fi-page-edit large"></i>\
            <i class="fi-folder large"></i>\
            <i class="fi-save large"></i>\
        ';

        var html = Mustache.to_html(template);
        jQuery('#page-icons').html(html);
    }

    function update(type, data) {
        if (_dashLoaded) {
            jQuery.each(_dash, function (index, widget) {
                if (widget.update !== undefined) {
                    if (widget.messages !== undefined) {
                        if (widget.messages.indexOf(type) !== -1) {
                            widget.update(data);
                        }
                    }
                }
            });
        }
    }

    return {
        showVideo: _showVideo,

        init: function () {
            return init();
        },

        update: function (type, data) {
            update(type, data);
        }
    }
}
var OneHUDViewDash = function () {
    'use strict'

    var _name = 'Dash';
    var _icon = 'images/pages/dash.png';
    var _menuIcon = 'images/pages/dash-menu.png';
    var _editorMenuIcon = 'images/pages/editor-menu.png';
    var _openIcon = 'images/pages/open.png';
    var _saveIcon = 'images/pages/save.png';
    var _cancelIcon = 'images/pages/cancel.png';
    var _description = 'The AGServer dashboard. This is the interesting bit where you can create and use your own custom dashboards using the built in editor';
    var _showVideo = false;
    var _order = 2;

    var _dashJson = {};
    var _dash = [];
    var _dashLoaded = false;
    var _editing = false;

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
            OneHUDFile.loadDash().done(function (dash) {
                if (dash !== null) {
                    _dashJson = dash;
                } else {
                    _dashJson = _defaultDashboard;
                }
                buildUI();
                deferred.resolve();
            });
        });
        return deferred.promise();
    }

    function loadWidgets() {
        var deferred = jQuery.Deferred();

        var options = OneHUDUI.options();
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
        jQuery('body').css('background-color', '#222');
        buildToolbar();
        buildDash();
    }

    function buildDash() {
        _dash = [];
        jQuery.each(_dashJson.widgets, function (index, widget) {
            var widgetName = widget.type;
            var widgetClass = 'OneHUD' + widgetName.toUpperCase() + 'Widget';

         //   try {
                addWidget(widgetClass, widget);
         //   } catch (error) {
         //       console.log(error);
         //   }
        });
        _dashLoaded = true;
    }
    
    function clearDash() {
        if (_dashLoaded) {
            jQuery.each(_dash, function (index, widget) {
                if (widget.destroy !== undefined) {
                    widget.destroy(false);
                }
                if (widget.element !== undefined) {
                    var el = widget.element();
                    jQuery(el).remove();
                }
            });
            _dashLoaded = false;
            _dash = [];
        }
    }

    function addWidget(widgetClass, properties) {
        var widgetController = createWidget(widgetClass, properties);
        _dash.push(widgetController);
        return widgetController;
    }

    function createWidget(widgetClass, properties) {
        var widgetController = new window[widgetClass]();

        if (properties === undefined) {
            properties = widgetController.properties;
        }

        if (properties.css === undefined) {
            properties.css = {};
        }

        properties.css['position'] = 'absolute';
        properties.css['zindex'] = 100;

        var element = jQuery('<div>').css(properties.css)
            .addClass('widget agselectable')
            .attr('id', OneHUDUI.getNextId())
        jQuery('#content').append(element);
        element.data('type', widgetController.name);

        widgetController.init(element, properties);

        return widgetController;
    }

    function buildToolbar() {
        var template = '\
            <i class="fi-page-edit large"></i>\
            <i class="fi-folder large"></i>\
            <i class="fi-save large"></i>\
        ';

        template = '';
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

    function gotFocus(el) {
        var instance = this;
        var image = jQuery('<img>', { 'src': _editorMenuIcon, 'width': '40px' });
        var menuLink = jQuery('<a>', { 'href': '#', 'class': 'button hollow expanded clearfix', 'id': 'editor-start' }).append(image);
        jQuery(el).append(menuLink);

        jQuery('#editor-start').on('click', function (e) {

            var editorScripts = [
                '/js/pages/editor.js',
                '/js/jqPropertyGrid/jqPropertyGrid.js'//,
               // '/js/jqPropertyGrid/jqPropertyGrid.css'
            ];

            head.load(editorScripts, function () {

                if (!_editing) {
                    OneHUDDashEditor.start();
                    _editing = true;
                } else {
                    OneHUDDashEditor.stop();
                    _editing = false;
                }
                jQuery('#offCanvas').foundation('close');
            });
        });

        image = jQuery('<img>', { 'src': _openIcon, 'width': '40px' });
        menuLink = jQuery('<a>', { 'href': '#', 'class': 'button hollow float-left mr5', 'id': 'editor-load-dash' }).append(image);
        jQuery(el).append(menuLink);

        image = jQuery('<img>', { 'src': _saveIcon, 'width': '40px' });
        menuLink = jQuery('<a>', { 'href': '#', 'class': 'button hollow float-left mr5 disabled', 'id': 'editor-save-dash' }).append(image);
        jQuery(el).append(menuLink);

        image = jQuery('<img>', { 'src': _cancelIcon, 'width': '40px' });
        menuLink = jQuery('<a>', { 'href': '#', 'class': 'button hollow float-left mr5 disabled', 'id': 'editor-cancel' }).append(image);
        jQuery(el).append(menuLink);
        jQuery('#editor-cancel').on('click', function () {
            jQuery('#offCanvas').foundation('close');
            OneHUDDashEditor.stop();
            clearDash();
            buildDash();
        });
    }

    function lostFocus(el) {
        jQuery('#editor-start').remove();
        jQuery('#editor-load-dash').remove();
        jQuery('#editor-save-dash').remove();
        jQuery('#editor-cancel').remove();
    }

    return {
        showVideo: _showVideo,

        init: function () {
            return init();
        },

        update: function (type, data) {
            update(type, data);
        },

        gotFocus: function (el) {
            gotFocus(el);
        },

        lostFocus: function (el) {
            lostFocus(el);
        },

        getDash: function () {
            return _dash;
        },

        addWidget: function (widgetClass, properties) {
            return addWidget(widgetClass, properties);
        },

        createWidget: function(widgetClass, properties) {
            return createWidget(widgetClass, properties);
        }
    }
}
//# sourceURL=/js/pages/dash.js
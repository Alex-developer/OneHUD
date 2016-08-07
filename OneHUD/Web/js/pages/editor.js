var OneHUDDashEditor = function () {
    'use strict'

    var _gridSize = 10;
    var _dashInstance = null;
    var _savedDash = null;
    var _selected = {
        el: null,
        id: null,
        instance: null
    };
    var _toolbarWindow = null;
    var _propertiesWindow = null;

    function start() {
        _dashInstance = OneHUDUI.getCurrentPage();
        var _savedDash = jQuery.extend(true, {}, _dashInstance.getDash());
        jQuery('#editor-save-dash').removeClass('disabled');
        jQuery('#editor-cancel').removeClass('disabled');
        addEventHandlers();
        drawGrid();
        startWidgets();
        buildWindows();
    }

    function stop() {
        jQuery('#editor-save-dash').addClass('disabled');
        jQuery('#editor-cancel').addClass('disabled');

        destroyWindows();
        removeEventHandlers();
        deleteGrid();
        stopWidgets();
    }

    function buildWindows() {
        var tabs = {};
        var active = 'is-active';

        var options = OneHUDUI.options();

        jQuery.each(options.Widgets, function (pos, widget) {
            var tab = widget.Tab;
            if (tab !== null) {
                if (tabs[tab] === undefined) {
                    tabs[tab] = {
                        'name': tab,
                        'active' : active,
                        'widgets': []
                    };
                    active = '';
                }
                tabs[tab].widgets.push(widget);
            }
        });
        
        var mustacheFormattedData = { 'tabs': [] };

        for (var prop in tabs) {
            if (tabs.hasOwnProperty(prop)) {
                mustacheFormattedData['tabs'].push({
                    'key': prop,
                    'active': tabs[prop].active,
                    'value': tabs[prop]
                });
            }
        }


        var tabTemplate = '\
            <div class="row collapse">\
                <div class="medium-2 columns">\
                    <ul class="tabs vertical" id="example-vert-tabs" data-tabs>\
                        {{#tabs}}\
                        <li class="tabs-title {{active}}"><a href="#panel-{{key}}" aria-selected="true"><div class="tab tab-{{key}}"></div></a></li>\
                        {{/tabs}}\
                    </ul>\
                </div>\
                <div class="medium-10 columns">\
                    {{#tabs}}\
                    <div class="tabs-content vertical" data-tabs-content="example-vert-tabs">\
                        <div class="tabs-panel {{active}} clearfix" id="panel-{{key}}">\
                            <ul class="menu icon-top">\
                            {{#value.widgets}}\
                                <li class="add-widget" data-classname="{{ClassName}}"><a href="#"><img src="{{Icon}}" width=32 />\ <span>{{Name}}</span></a></li>\
                            {{/value.widgets}}\
                            </ul>\
                        </div>\
                    </div>\
                    {{/tabs}}\
                </div>\
            </div>';
        var html = Mustache.to_html(tabTemplate, mustacheFormattedData);

        _toolbarWindow = jQuery.jsPanel({
            'headerTitle': 'Widgets',
            content: html
        }).foundation();


        jQuery('.add-widget').on('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            var widgetClassName = jQuery(this).data('classname');
            var widget = _dashInstance.addWidget(widgetClassName);
            startWidget(widget);
        });

        _propertiesWindow = jQuery.jsPanel({
            'theme' : 'default',
            'headerTitle': 'Properties',
            contentSize: { width: 430, height: 200 }
        });
    }

    function destroyWindows() {
        _toolbarWindow.close();
        _propertiesWindow.close();
    }

    function addEventHandlers() {
        jQuery('#content').on('click', '.agselectable', function (e) {
            e.preventDefault();
            e.stopPropagation();
            clearSelectedWidget();
            jQuery(this).addClass('selected');
            _selected.el = this;
            _selected.id = jQuery(this).attr('id');
            var _dash = _dashInstance.getDash();
            jQuery.each(_dash, function (pos, widget) {
                if (widget.element !== undefined) {
                    if (widget.element().attr('id') === _selected.id) {
                        _selected.instance = widget;
                    }
                }
            });

            setupPropertiesWindows();
        });
    }

    function setupPropertiesWindows() {
        var className = 'OneHUDPropertyPageNOPROPERTIES';
        if (_selected.instance.propertypage !== undefined) {
            className = 'OneHUDPropertyPage' + _selected.instance.propertypage.toUpperCase();
        }
        if (window[className] === undefined) {
            var path = 'js/pages/editorpropertypages/noproperties.js';
            if (_selected.instance.propertypage !== undefined) {
                path = 'js/pages/editorpropertypages/' + _selected.instance.propertypage + '.js';
            }
            head.load(path, function (a, b) {
                window[className].init(_propertiesWindow, _selected);
            });
        } else {
            window[className].init(_propertiesWindow, _selected);
        }
    }

    function clearSelectedWidget() {
        jQuery('.agselectable').removeClass('selected');
        _selected = {
            el: null,
            id: null,
            instance: null
        };
    }

    function removeEventHandlers() {
        clearSelectedWidget();
        jQuery('#content').off('click', '.agselectable');
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
            startWidget(widget);
        })
    }

    function startWidget(widget) {
        if (widget.element !== undefined) {
            var el = widget.element();
            jQuery(el).draggable({
                stop: function (event, ui) {
                    updateWidgetPosition(widget);
                    setupPropertiesWindows();
                }
            });
            jQuery(el).resizable({
                resize: function (event, ui) {
                    if (widget.resize !== undefined) {
                        widget.resize();
                    }
                },
                stop: function (event, ui) {
                    updateWidgetPosition(widget);
                    setupPropertiesWindows();
                }
            });
            jQuery(el).addClass('border');

        }

        if (widget.startEditing !== undefined) {
            widget.startEditing();
        }
    }

    function updateWidgetPosition(widget) {
        var el = widget.element();
        var position = jQuery(el).position();
        widget.properties().css.left = position.left;
        widget.properties().css.top = position.top;
        widget.properties().css.width = jQuery(el).width();
        widget.properties().css.height = jQuery(el).height();
    }

    function stopWidgets() {
        var _dash = _dashInstance.getDash();

        jQuery.each(_dash, function (pos, widget) {
            if (widget.element !== undefined) {
                var el = widget.element();

                var draggable = jQuery(el).draggable("instance");
                if (draggable !== undefined) {
                    jQuery(el).draggable('destroy');
                }
                var resizeable = jQuery(el).draggable("instance");
                if (resizeable !== undefined) {
                    jQuery(el).resizable('destroy');
                }
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
//# sourceURL=/js/pages/editor.js
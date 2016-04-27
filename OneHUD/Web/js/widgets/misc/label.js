var OneHUDLABELWidget = function () {
    'use strict';

    var _name = 'Image';
    var _icon = '/images/widgets/label.png';

    var _initialised = false;
    var _el = null;
    var _elId = null;
    var _tab = 'Misc';
    var _supports = 'all';

    var _properties = {
        type: 'label',
        text: 'Label',
        align: 'left',
        css: {
            left: 0,
            top: 0,
            width: 614,
            height: 324,
            'font-family': 'LatoWeb',
            'font-weight': 'bold',
            'font-style': 'normal',
            color: 'white'
        }
    };
    var _messages = [];


    function init(element, properties) {
        _el = element;
        _properties = properties;
        buildUI();
    }

    function destroy() {
        _initialised = false;
        jQuery(_el).remove();
    }

    function buildUI() {
        jQuery(_el).css('font-family', _properties.fontfamily);
        jQuery(_el).css('color', '#' + _properties.color);
        _elId = OneHUDUI.getNextId();
        var element = jQuery('<span>').css({ 'pointer-events': 'none' }).text(_properties.text).attr('id', _elId);

        jQuery(_el).append(element);
        jQuery('#' + _elId).bigText({ horizontalAlign: _properties.align });
    }


    return {
        name: _name,
        icon: _icon,
        messages: _messages,
        tab: _tab,
        supports: _supports,

        element: function () {
            return _el;
        },

        init: function (element, settings) {
            return init(element, settings);
        },

        destroy: function () {
            destroy();
        }

    }
};
//# sourceURL=/js/widgets/misc/label.js
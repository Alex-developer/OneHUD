var OneHUDSTATUSWidget = function () {
    'use strict';

    var _name = 'Status';
    var _icon = '/images/widgets/label.png';

    var _initialised = false;
    var _el = null;
    var _element = null;
    var _tab = 'Misc';
    var _supports = 'all';

    var _properties = {
        type: 'status',
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
    var _messages = ['builtin'];

    function init(element, properties) {
        _el = element;
        _properties = properties;
        buildUI();
    }

    function destroy() {
        _initialised = false;
        jQuery(_element).remove();
        jQuery(_el).remove();
    }

    function buildUI() {
        jQuery(_el).css('font-family', _properties.fontfamily);
        jQuery(_el).css('color', '#' + _properties.color);
        _element = jQuery('<span>').css({ 'pointer-events': 'none' });
        update('Waiting');

        jQuery(_el).append(_element);
        jQuery(_element).bigText({ horizontalAlign: _properties.align });
    }

    function update(message) {
        jQuery(_element).html('<strong>Connected To:</strong> ' + message);
    }

    return {
        name: _name,
        icon: _icon,
        messages: _messages,
        tab: _tab,
        supports: _supports,
        properties: _properties,

        element: function () {
            return _el;
        },

        init: function (element, settings) {
            return init(element, settings);
        },

        destroy: function () {
            destroy();
        },

        update: function (data) {
            update(data);
        }

    }
};
//# sourceURL=/js/widgets/misc/status.js
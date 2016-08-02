var OneHUDIMAGEWidget = function () {
    'use strict';

    var _name = 'Image';
    var _icon = '/images/widgets/image.png';

    var _initialised = false;
    var _el = null;
    var _elImage = null;
    var _tab = 'Misc';
    var _supports = 'all';
    var _propertypage = 'image';

    var _properties = {
        type: 'image',
        css: {
            left: 0,
            top: 0,
            width: 614,
            height: 324
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
        _elImage = jQuery('<img>', { 'src': '/images/dash/dash.svg' }).css({ 'pointer-events': 'none' }).addClass('flag');
        jQuery(_el).append(_elImage);
        _initialised = true;
    }


    return {
        name: _name,
        icon: _icon,
        messages: _messages,
        tab: _tab,
        supports: _supports,
        propertypage: _propertypage,

        properties: function (properties) {

            if (properties !== undefined) {
                _properties = properties;
            }
            return _properties;
        },

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
//# sourceURL=/js/widgets/misc/image.js
var OneHUDSESSIONNAMEWidget = function () {
    'use strict';

    var _name = 'Session Name';
    var _icon = '/images/widgets/label.png';
    var _labels = ['Session Name'];
    var _tab = 'Session';
    var _supports = ['iRacing'];
    var _propertypage = 'text';

    var _initialised = false;
    var _el = null;
    var _textElement = null;
    var _lastSessionName = null;

    var _properties = {
        type: 'sessionname',
        css: {
            left: 3,
            top: 575,
            width: 50,
            height: 50,
            'font-family': 'Led',
            'font-weight': 'bold',
            color: 'white'
        }
    };
    var _messages = ['timing'];

    function init(element, properties) {
        _el = element;

        if (properties !== undefined) {
            _properties = properties;
        }

        buildUI();
    }

    function destroy() {
        _initialised = false;
        jQuery(_el).remove();
    }

    function buildUI() {
        _el.css('font-weight', _properties.fontweight);
        _el.css('color', '#' + _properties.color);
        _textElement = jQuery('<span>')
            .css({
                'pointer-events': 'none'
            })
            .html('&nbsp;');

        jQuery(_el).append(_textElement);
        _textElement.bigText();
        _initialised = true;
    }

    function update(data) {
        if (_initialised) {
            var sessionType = data.RaceInfo.SessionType;
            if (_lastSessionName !== sessionType) {
                _textElement.text(OneHUDDataProtocol.SessionName[sessionType]);
                _lastSessionName = sessionType;
            }
        }
    }

    function resize() {
        _textElement.bigText();
    }

    return {
        name: _name,
        icon: _icon,
        messages: _messages,
        labels: _labels,
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

        init: function (element, properties) {
            return init(element, properties);
        },

        destroy: function () {
            destroy();
        },

        update: function (data) {
            update(data);
        },

        resize: function () {
            resize();
        }

    }
};
//# sourceURL=/js/widgets/session/sessionname.js
var OneHUDGEARWidget = function () {
    'use strict';

    var _name = 'Gear';
    var _icon = '/images/widgets/gear.png';
    var _labels = ['Gear'];
    var _tab = 'Car';
    var _supports = ['iRacing', 'Project Cars', 'Assetto Corsa'];

    var _initialised = false;
    var _el = null;
    var _gearElement = null;
    var _lastGear = null;

    var _properties = {
        type: 'gear',
        css: {
            left: 0,
            top: 0,
            width: 50,
            height: 50,
            'font-family': 'Led',
            'font-weight': 'bold',
            color: 'black'
        }
    };
    var _messages = ['telemetry'];

    function init(element, properties) {
        _el = element;
        buildUI();
    }

    function destroy() {
        _initialised = false;
        jQuery(_el).remove();
    }

    function buildUI() {
        _el.css('font-weight', _properties.fontweight);
        _el.css('color', '#' + _properties.color);
        _gearElement = jQuery('<span>')
            .css({
                'pointer-events': 'none'
            })
            .text('N');

        jQuery(_el).append(_gearElement);
        _gearElement.bigText();
        _initialised = true;
    }

    function update(data) {
        if (_initialised) {
            if (_lastGear !== data.Car.Gear) {
                switch (data.Car.Gear) {
                    case -1:
                        _gearElement.text('R');
                        break;

                    case 0:
                        _gearElement.text('N');
                        break;

                    default:
                        _gearElement.text(data.Car.Gear);
                        break;
                }
                _lastGear = data.Car.Gear;
            }
        }
    }


    return {
        name: _name,
        icon: _icon,
        messages: _messages,
        labels: _labels,
        tab: _tab,
        supports: _supports,

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
        }

    }
};
//# sourceURL=/js/widgets/gear.js
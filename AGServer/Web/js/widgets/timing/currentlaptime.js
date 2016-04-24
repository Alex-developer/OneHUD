var AGDashCURRENTLAPTIMEWidget = function () {
    'use strict';

    var _name = 'Current';
    var _icon = '/images/widgets/stopwatch.png';
    var _labels = ['Current Lap', 'Current'];
    var _tab = 'Timing';
    var _supports = ['iRacing', 'Project Cars', 'Assetto Corsa', 'RaceRoom Experience'];

    var _el = null;
    var _elId = null;
    var _lastCurrentLapTime = null;

    var _properties = {
        type: 'currentlaptime',
        css: {
            left: 0,
            top: 0,
            width: 300,
            height: 50,
            'font-weight': 'bold',
            color: 'white'
        }
    };
    var _messages = ['telemetry'];

    function init(element, properties) {
        if (element !== undefined) {
            _el = element;
        }
        if (properties !== undefined) {
            _properties = properties;
        }
        buildUI();
    }

    function destroy() {
        jQuery(_el).remove();
    }

    function buildUI() {
        _elId = AGServerUI.getNextId();
        var element = jQuery('<span>').css({ 'pointer-events': 'none' }).html('--:--:--').attr('id', _elId).width(_properties.width).height(_properties.height);

        jQuery(_el).append(element);
        jQuery('#' + _elId).bigText();
    }

    function update(data) {
        if (data.Car.InCar) {
            if (_lastCurrentLapTime !== data.Timing.CurrentLapTime) {
                if (data.Timing.CurrentLapTime !== 0 && data.Timing.CurrentLapTime !== -1) {
                    jQuery('#' + _elId).html(data.Timing.CurrentLapTime.toHHMMSS(true));
                } else {
                    jQuery('#' + _elId).html('--:--:--');
                }
                _lastCurrentLapTime = data.Timing.CurrentLapTime;
            }
        } else {
            jQuery('#' + _elId).html('--:--:--');
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
//# sourceURL=/js/widgets/timing/currentlaptime.js
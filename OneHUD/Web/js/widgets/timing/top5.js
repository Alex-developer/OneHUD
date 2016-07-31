var OneHUDTOP5Widget = function () {
    'use strict';

    var _name = 'Top5';
    var _icon = '/images/widgets/stopwatch.png';
    var _labels = ['Current Lap', 'Current'];
    var _tab = 'Timing';
    var _supports = ['iRacing', 'Project Cars', 'Assetto Corsa', 'RaceRoom Experience'];

    var _el = null;
    var _elId = null;
    var _lastCurrentLapTime = null;

    var _properties = {
        type: 'top5',
        css: {
            left: 0,
            top: 0,
            width: 300,
            height: 50,
            'font-weight': 'bold',
            color: 'white'
        }
    };
    var _messages = ['timing'];

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
        _elId = OneHUDUI.getNextId();
        var html = '<div id="' + _elId + '" class="smsall-5 lasrge-5 colsumns">\
            <p>1&nbsp;&nbsp;<span class="position1"></span></p>\
            <p>2&nbsp;&nbsp;<span class="position2"></span></p>\
            <p>3&nbsp;&nbsp;<span class="position3"></span></p>\
            <p>4&nbsp;&nbsp;<span class="position4"></span></p>\
            <p>5&nbsp;&nbsp;<span class="position5"></span></p>\
        </div>';
        jQuery(_el).append(html);
    }

    function update(data) {
        var drivers = data.RaceInfo.Drivers;

        for (var i = 0; i < drivers.length; i++) {
            if (drivers[i].DriverType === 0) {
                jQuery('#' + _elId + ' .position' + (drivers[i].Position)).html(drivers[i].Name);
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
        properties: _properties,

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
//# sourceURL=/js/widgets/timing/top5.js
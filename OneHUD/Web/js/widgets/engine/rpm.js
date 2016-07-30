var OneHUDRPMWidget = function () {
    'use strict';

    var _name = 'RPM';
    var _icon = '/images/widgets/gauge.png';
    var _labels = ['RPM', 'Rev Counter'];
    var _description = 'Displays the engines rpm';
    var _tab = 'Engine';
    var _supports = ['iRacing', 'Project Cars', 'Assetto Corsa', 'RaceRoom Experience'];
    var _requires = ['/js/gauge/gauge.js'];
    var _messages = ['telemetry'];
    var _el;
    var _gaugeElement = null;
    var _lastRpm = 0;

    var _properties = {
        type: 'rpm',
        gaugestyle: 'analogue',
        align: 'left',
        css: {
            left: 0,
            top: 0,
            width: 300,
            height: 100,
            'font-family': 'ledfont',
            'font-weight': 'bold',
            color: 'white'
        }
    };

    var _gaugeRPM = null;

    function init(element, properties) {

        if (element !== undefined) {
            _el = element;
        }
        if (properties !== undefined) {
            _properties = properties;
        }
        head.load(_requires, function () {
            buildUI();
        });
    }

    function destroy(leaveElement) {
        if (leaveElement === undefined) {
            leaveElement = false;
        }

        jQuery(_gaugeRPM).remove();
        _gaugeRPM = null;
        jQuery(_gaugeElement).remove();

        if (!leaveElement) {
            jQuery(_el).remove();
        }
    }

    function buildUI() {
        if (_properties.gaugestyle === 'digital') {
            buildUIDigital();
        } else {
            buildUIAnalogue();
        }
    }

    function buildUIDigital() {

    }

    function resize() {
        destroy(true);
        buildUIAnalogue();
    }

    function buildUIAnalogue() {

        _gaugeElement = jQuery('<canvas>')
            .attr('id', OneHUDUI.getNextId())

        jQuery(_el).append(_gaugeElement);

        _gaugeRPM = new Gauge({
            renderTo: _gaugeElement.attr('id'),
            width: jQuery(_el).width(),
            height: jQuery(_el).height(),
            glow: false,
            units: 'RPM',
            title: false,
            minValue: 0,
            maxValue: 11000,
            majorTicks: ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11'],
            minorTicks: 5,
            strokeTicks: true,
            highlights: [{ from: 160, to: 220, color: 'rgba(200, 50, 50, .75)' }],
            colors: {
                plate: '#222',
                majorTicks: '#f5f5f5',
                minorTicks: '#ddd',
                title: '#fff',
                units: '#ccc',
                numbers: '#eee',
                needle: {
                    start: 'rgba(200, 50, 50, .75)',
                    end: 'rgba(200, 50, 50, .75)',
                    circle: {
                        outerStart: 'rgba(200, 200, 200, 1)',
                        outerEnd: 'rgba(200, 200, 200, 1)'
                    },
                    shadowUp: true,
                    shadowDown: false
                }
            },
            valueBox: {
                visible: true
            },
            valueText: {
                visible: true
            },
            valueFormat: {
                int: 5,
                dec: 0
            },
            circles: {
                outerVisible: false,
                middleVisible: false,
                innerVisible: false
            },
            needle: {
                type: 'arrow',
                width: 2,
                end: 72,
                circle: {
                    size: 7,
                    inner: false,
                    outer: true
                }
            },
            animation: false
        });

        _gaugeRPM.setRawValue(0);

        _gaugeRPM.draw();

    }

    function update(data) {
        if (_gaugeRPM !== null) {
            if (data.Car.InCar) {
                var rpm = data.Engine.RPM.toFixed(0);

                if (_lastRpm !== rpm) {
                    _gaugeRPM.setValue(rpm);
                    _lastRpm = rpm;
                }
            } else {
                _gaugeRPM.setValue(0);
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

        destroy: function (leaveElement) {
            destroy(leaveElement);
        },

        update: function (data) {
            update(data);
        },

        resize: function () {
            resize();
        }
    }
}
//# sourceURL=/js/widgets/engine/rpm.js
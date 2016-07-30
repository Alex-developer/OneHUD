var OneHUDWATERTEMPWidget = function () {
    'use strict';

    var _name = 'Water Temp';
    var _icon = '/images/widgets/gauge.png';
    var _labels = ['Fuel'];
    var _description = 'Displays The Engines Water Temperature';
    var _tab = 'Engine';
    var _supports = ['iRacing', 'Project Cars', 'RaceRoom Experience'];
    var _requires = ['/js/gauge/gauge.js'];
    var _messages = ['telemetry'];
    var _el;
    var _gaugeElement = null;
    var _lastFuel = -999;

    var _properties = {
        type: 'watertemp',
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

    var _gaugeWaterTemp = null;

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

        jQuery(_gaugeWaterTemp).remove();
        _gaugeWaterTemp = null;
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

        _gaugeWaterTemp = new Gauge({
            renderTo: _gaugeElement.attr('id'),
            width: jQuery(_el).width(),
            height: jQuery(_el).height(),
            glow: false,
            units: 'Water Temp.',
            title: false,
            minValue: 0,
            maxValue: 120,
            majorTicks: ['0', '25', '75', '100', '125'],
            minorTicks: 4,
            strokeTicks: false,
            startAngle: 180,
            ticksAngle: 180,
            highlights: [],
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
                },
                circle: {
                    shadow: false,
                    outerStart: '#333',
                    outerEnd: '#111',
                    middleStart: '#222',
                    middleEnd: '#111',
                    innerStart: '#111',
                    innerEnd: '#333'
                },
                valueBox: {
                    rectStart: '#222',
                    rectEnd: '#333',
                    background: '#babab2',
                    shadow: 'rgba(0, 0, 0, 1)'
                }
            },
            circles: {
                outerVisible: false,
                middleVisible: false,
                innerVisible: false
            },
            valueBox: {
                visible: false
            },
            valueText: {
                visible: false
            },
            valueFormat: {
                int: 3,
                dec: 0
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
            animation: false,
            updateValueOnAnimation: true
        });

        _gaugeWaterTemp.setRawValue(0);

        _gaugeWaterTemp.draw();

    }

    function update(data) {
        if (_gaugeWaterTemp !== null) {
            if (data.Car.InCar) {
                var fuelRemaining = data.Car.FuelRemaining.toFixed(0);
                var fuelCapacity = data.Car.FuelCapacity.toFixed(0);

                var fuel = (fuelRemaining / fuelCapacity) * 100;

                if (_lastFuel !== fuel) {
                    _gaugeWaterTemp.setValue(fuel);
                    _lastFuel = fuel;
                }
            } else {
                _gaugeWaterTemp.setValue(0);
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
//# sourceURL=/js/widgets/fuel.js
﻿var OneHUDSPEEDWidget = function () {
    'use strict';

    var _name = 'Speed';
    var _icon = '/images/widgets/gauge.png';
    var _labels = ['Speed'];
    var _description = 'Displays The Cars Speed';
    var _tab = 'Engine';
    var _supports = ['iRacing', 'Project Cars', 'Assetto Corsa', 'RaceRoom Experience'];
    var _requires = ['/js/gauge/gauge.js'];
    var _messages = ['telemetry'];
    var _el;
    var _gaugeElement = null;
    var _lastSpeed = -999;

    var _properties = {
        type: 'speed',
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

    var _gaugeSpeed = null;

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

        jQuery(_gaugeSpeed).remove();
        _gaugeSpeed = null;
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

        _gaugeSpeed = new Gauge({
            renderTo: _gaugeElement.attr('id'),
            width: jQuery(_el).width(),
            height: jQuery(_el).height(),
            glow: false,
            units: 'MPH',
            title: false,
            minValue: 0,
            maxValue: 160,
            majorTicks: ['0', '20', '40', '60', '80', '100', '120', '140', '160'],
            minorTicks: 2,
            strokeTicks: true,
            highlights: [{ from: 120, to: 160, color: 'rgba(200, 50, 50, .75)' }],
            ticksAngle: 165,
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
                visible: true
            },
            valueText: {
                visible: true
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

        _gaugeSpeed.setRawValue(0);

        _gaugeSpeed.draw();

    }

    function update(data) {
        if (_gaugeSpeed !== null) {
            if (data.Car.InCar) {

                var speed = data.Car.Speed.toFixed(0);

                if (_lastSpeed !== speed) {
                    _gaugeSpeed.setValue(speed);
                    _lastSpeed = speed;
                }
            } else {
                _gaugeSpeed.setValue(0);
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
//# sourceURL=/js/widgets/speed.js
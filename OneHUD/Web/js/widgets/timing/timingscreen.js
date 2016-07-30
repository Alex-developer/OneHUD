var OneHUDTIMINGSCREENWidget = function () {
    'use strict';

    var _name = 'Timing';
    var _icon = '/images/widgets/stopwatch.png';
    var _labels = ['Current Lap', 'Current'];
    var _tab = 'Timing';
    var _supports = ['iRacing'];
    var _messages = ['timing'];

    var _el = null;
    var _elId = null;
    var _lastCurrentLapTime = null;
    var _lastDriverCount = 0;
    var _timingEl;

    var _properties = {
        type: 'timingscreen',
        css: {
            left: 0,
            top: 0,
            width: 300,
            height: 50,
            'font-weight': 'bold',
            color: 'white'
        }
    };

    function init(element, properties) {
        if (element !== undefined) {
            _el = element;
        }
        if (properties !== undefined) {
            _properties = properties;
        }
    }

    function destroy() {
        jQuery(_el).remove();
    }

    function buildUI(drivers) {
        _elId = OneHUDUI.getNextId();

        var html = '<div class="row">';
        html += '<div class="small-1 large-1 columns">Pos</div>';
        html += '<div class="small-3 large-3 columns">Driver</div>';
        html += '<div class="small-2 large-2 columns">Lap</div>';
        html += '<div class="small-2 large-2 columns">Time</div>';
        html += '<div class="small-2 large-2 columns">Best</div>';
        html += '<div class="small-2 large-2 columns">Gap</div>';
        html += '</div>';

        for (var i = 0; i < drivers.length; i++) {
            html += '<div class="row">';
            html += '<div class="small-1 large-1 columns pos' + i + '">' + (i + 1) + '</div>';
            html += '<div class="small-3 large-3 columns driver' + i + '"></div>';
            html += '<div class="small-2 large-2 columns laps' + i + '"></div>';
            html += '<div class="small-2 large-2 columns laptime' + i + '"></div>';
            html += '<div class="small-2 large-2 columns best' + i + '"></div>';
            html += '<div class="small-2 large-2 columns gap' + i + '"></div>';
            html += '</div>';
        };

        jQuery(_timingEl).remove();
        _timingEl = jQuery('<div>').html(html).attr('id', _elId);
        jQuery(_el).css('overflow-y', 'scroll').css('overflow-x', 'hidden');
        jQuery(_el).append(_timingEl);
    }

    function update(data) {
        var drivers = data.RaceInfo.Drivers;
        if (_lastDriverCount !== drivers.length) {
            buildUI(drivers);
            _lastDriverCount = drivers.length;
        }

        var lastDriver = null;
        for (var i = 0; i < drivers.length; i++) {
            var position = i + 1;
            for (var j = 0; j < drivers.length; j++) {
                if (drivers[j].Position === position) {

                    jQuery('#' + _elId + ' .driver' + i).html(drivers[j].Name);
                    jQuery('#' + _elId + ' .laps' + i).html(drivers[j].Lap);
                    if (drivers[j].LapsDown === 0) {
                        if (position === 1) {
                            jQuery('#' + _elId + ' .laptime' + i).html(data.RaceInfo.SessionTime.toHHMMSS(true));
                        } else {
                            jQuery('#' + _elId + ' .laptime' + i).html(drivers[j].DeltaTime);
                        }
                    } else {
                        jQuery('#' + _elId + ' .laptime' + i).html("-" + drivers[j].LapsDown + "L");
                    }
                    jQuery('#' + _elId + ' .best' + i).html(drivers[j].FastestLapTime);
                    lastDriver = j;
                    break;
                }
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
//# sourceURL=/js/widgets/timing/timingscreen.js
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

        var html = '<div class="row timingheader expanded">';
        html += '<div class="small-3 large-3 columns">Driver</div>';
        html += '<div class="small-1 large-1 columns">Prac</div>';
        html += '<div class="small-1 large-1 columns">Time</div>';
        html += '<div class="small-1 large-1 columns">Qual</div>';
        html += '<div class="small-1 large-1 columns">Time</div>';
        html += '<div class="small-1 large-1 columns">Race</div>';
        html += '<div class="small-1 large-1 columns">Time</div>';
        html += '<div class="small-3 large-3 columns"></div>';
        html += '</div>';

        for (var i = 0; i < drivers.length; i++) {
            if (drivers[i].DriverType === 0) {
                html += '<div class="row timingrow expanded">';
                html += '<div class="small-3 large-3 columns driver' + i + '"></div>';
                html += '<div class="small-1 large-1 columns pracpos' + i + ' pos"></div>';
                html += '<div class="small-1 large-1 columns practime' + i + ' time"></div>';
                html += '<div class="small-1 large-1 columns qualpos' + i + ' pos"></div>';
                html += '<div class="small-1 large-1 columns qualtime' + i + ' time"></div>';
                html += '<div class="small-1 large-1 columns racepos' + i + ' pos"></div>';
                html += '<div class="small-1 large-1 columns racetime' + i + ' time"></div>';
                html += '<div class="small-3 large-3 columns"></div>';
                html += '</div>';
            }
        };

        jQuery(_timingEl).remove();
        _timingEl = jQuery('<div class="timingpage">').html(html).attr('id', _elId);
        jQuery(_el).css('overflow-y', 'scroll').css('overflow-x', 'hidden');
        jQuery(_el).append(_timingEl);
        jQuery('body').css('background-color', '#222');
    }

    function update(data) {
        var drivers = data.RaceInfo.Drivers;
        if (_lastDriverCount !== drivers.length) {
            buildUI(drivers);
            _lastDriverCount = drivers.length;
        }

        var position;
        for (var i = 0; i < drivers.length; i++) {
            if (drivers[i].DriverType === 0) {
                position = i + 1;
                for (var j = 0; j < drivers.length; j++) {
                    if (drivers[j].Position === position) {
                        jQuery('#' + _elId + ' .driver' + i).html(drivers[j].Name);
                        jQuery('#' + _elId + ' .racepos' + i).html(drivers[j].Position);
                        if (drivers[j].LapsDown === 0) {
                            if (position === 1) {
//                                jQuery('#' + _elId + ' .racetime' + i).html(data.RaceInfo.SessionTime.toMMSS(true));
                                jQuery('#' + _elId + ' .racetime' + i).html("--:--");
                            } else {
                                jQuery('#' + _elId + ' .racetime' + i).html(drivers[j].DeltaTime.toMMSS(true));
                            }
                        } else {
                            jQuery('#' + _elId + ' .racetime' + i).html("-" + drivers[j].LapsDown + "L");
                        }
                        jQuery('#' + _elId + ' .qualpos' + i).html(drivers[j].QualifyPosition);
                        jQuery('#' + _elId + ' .qualtime' + i).html(drivers[j].QualifyFastestLap.toMMSS(true));
                        jQuery('#' + _elId + ' .pracpos' + i).html(drivers[j].PracticePosition);
                        jQuery('#' + _elId + ' .practime' + i).html(drivers[j].PracticeFastestLap.toMMSS(true));
                        drivers[j].Updated = true;
                        break;
                    }
                }
            }
        }

        position--;
        for (var i = 0; i < drivers.length; i++) {
            if (drivers[i].DriverType === 0) {
                if (drivers[i].Updated === undefined) {
                    jQuery('#' + _elId + ' .driver' + position).html(drivers[i].Name);
                    jQuery('#' + _elId + ' .qualpos' + position).html(drivers[i].QualifyPosition);
                    jQuery('#' + _elId + ' .qualtime' + position).html(drivers[i].QualifyFastestLap.toMMSS(true));
                    jQuery('#' + _elId + ' .pracpos' + position).html(drivers[i].PracticePosition);
                    jQuery('#' + _elId + ' .practime' + position).html(drivers[i].PracticeFastestLap.toMMSS(true));
                    position++;
                }
            }
        }
/*
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
        */
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
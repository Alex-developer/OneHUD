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
        jQuery('body').css('background-color', '#222');
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
                        if (data.RaceInfo.SessionType === OneHUDDataProtocol.SessionType.Race) {
                            jQuery('#' + _elId + ' .racepos' + i).html(drivers[j].Position);
                            if (drivers[j].LapsDown === 0) {
                                if (position === 1) {
                                    //                                jQuery('#' + _elId + ' .racetime' + i).html(data.RaceInfo.SessionTime.toMMSS(true));
                                    jQuery('#' + _elId + ' .racetime' + i).html("--:--");
                                } else {
                                    jQuery('#' + _elId + ' .racetime' + i).html(getSessionTime(drivers[j].DeltaTime));
                                }
                            } else {
                                jQuery('#' + _elId + ' .racetime' + i).html("-" + drivers[j].LapsDown + "L");
                            }
                        } else {
                            jQuery('#' + _elId + ' .racetime' + i).html("");
                            jQuery('#' + _elId + ' .racepos' + i).html("");
                        }
                        if (data.RaceInfo.SessionType === OneHUDDataProtocol.SessionType.Race || data.RaceInfo.SessionType === OneHUDDataProtocol.SessionType.Qualifying) {
                            jQuery('#' + _elId + ' .qualpos' + i).html(getDriverPos(drivers[j].QualifyPosition));
                            jQuery('#' + _elId + ' .qualtime' + i).html(getSessionTime(drivers[j].QualifyFastestLap));
                        }
                        jQuery('#' + _elId + ' .pracpos' + i).html(getDriverPos(drivers[j].PracticePosition));
                        jQuery('#' + _elId + ' .practime' + i).html(getSessionTime(drivers[j].PracticeFastestLap));
                        drivers[j].Updated = true;
                        break;
                    }
                }
            }
        }

        for (var i = 0; i < drivers.length; i++) {
            if (jQuery('#' + _elId + ' .driver' + i).html() === '') {
                position = i;
                break;
            }
        }

        for (var i = 0; i < drivers.length; i++) {
            if (drivers[i].DriverType === 0) {
                if (drivers[i].Updated === undefined) {
                    jQuery('#' + _elId + ' .driver' + position).html(drivers[i].Name);
                    if (data.RaceInfo.SessionType === OneHUDDataProtocol.SessionType.Race || data.RaceInfo.SessionType === OneHUDDataProtocol.SessionType.Qualifying) {
                        jQuery('#' + _elId + ' .qualpos' + position).html(getDriverPos(drivers[i].QualifyPosition));
                        jQuery('#' + _elId + ' .qualtime' + position).html(getSessionTime(drivers[i].QualifyFastestLap));
                    }
                    jQuery('#' + _elId + ' .pracpos' + position).html(getDriverPos(drivers[i].PracticePosition));
                    jQuery('#' + _elId + ' .practime' + position).html(getSessionTime(drivers[i].PracticeFastestLap));
                    position++;
                }
            }
        }
    }

    function getDriverPos(pos) {
        var driverPos = '&nbsp;';

        if (pos !== 0) {
            driverPos = pos;
        }

        return driverPos;
    }

    function getSessionTime(time) {
        var sessionTime = '&nbsp';

        if (time !== 0) {
            sessionTime = time.toMMSS(true);
        }

        return sessionTime;
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

        destroy: function () {
            destroy();
        },

        update: function (data) {
            update(data);
        }

    }
};
//# sourceURL=/js/widgets/timing/timingscreen.js
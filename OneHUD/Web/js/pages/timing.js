var OneHUDViewTiming = function () {
    'use strict'

    var _name = 'Timing';
    var _icon = 'images/pages/timing.png';
    var _menuIcon = 'images/pages/timing-menu.png';
    var _description = 'The AGServer timing page. For supported games this displays real time timing information from the current session';
    var _showVideo = false;
    var _order = 3;
    var _requires = '/js/widgets/timing/timingscreen.js';
    var _timingScreen;

    function init() {
        var deferred = jQuery.Deferred();
        loadScripts().done(function () {
            buildUI();
            deferred.resolve();
        });
        return deferred.promise();
    }

    function loadScripts() {
        var deferred = jQuery.Deferred();

        head.load(_requires, function () {
            deferred.resolve();
        });

        return deferred.promise();
    }

    function buildUI() {
      //  jQuery('body').css('background-color', '#222');
        var properties = {
            type: 'timingscreen',
            css: {
                left: 0,
                top: 0,
                width: '100%',
                height: '100%'
            }
        };

        var element = jQuery('<div>').css(properties.css)
            .addClass('widget agselectable')
            .attr('id', OneHUDUI.getNextId())
        jQuery('#content').append(element);

        _timingScreen = new OneHUDTIMINGSCREENWidget();
        _timingScreen.init(element, properties);
    }

    function update(type, data) {
        if (_timingScreen.update !== undefined) {
            if (_timingScreen.messages !== undefined) {
                if (_timingScreen.messages.indexOf(type) !== -1) {
                    _timingScreen.update(data);
                }
            }
        }
    }

    return {
        showVideo: _showVideo,

        init: function () {
            return init();
        },

        update: function (type, data) {
            update(type, data);
        }
    }
}
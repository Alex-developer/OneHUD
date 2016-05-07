var OneHUDViewTiming = function () {
    'use strict'

    var _name = 'Timing';
    var _icon = 'images/pages/timing.png';
    var _menuIcon = 'images/pages/timing-menu.png';
    var _description = 'The AGServer timing page. For supported games this displays real time timing information from the current session';
    var _showVideo = false;
    var _order = 3;

    function init() {

    }

    return {
        showVideo: _showVideo,

        init: function () {
            return init();
        }
    }
}
var OneHUDViewRecorder = function () {
    'use strict'

    var _name = 'Recorder';
    var _icon = 'images/pages/track.png';
    var _description = 'The AgServer Track Recorder. From here you can record track layouts for use in dashboards and timing screens.';
    var _showVideo = false;
    var _order = 4;

    function init() {
    }


    return {
        showVideo: _showVideo,

        init: function () {
            return init();
        }
    }
}
var OneHUDViewSettings = function () {
    'use strict'

    var _name = 'Settings';
    var _icon = 'images/pages/settings.png';
    var _menuIcon = 'images/pages/settings-menu.png';
    var _description = 'The AGServer settings page. Manage settings for your device';
    var _showVideo = false;
    var _order = 5;

    function init() {

    }

    return {
        showVideo: _showVideo,

        init: function () {
            return init();
        }
    }
}
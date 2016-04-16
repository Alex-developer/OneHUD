var AGRacingViewDash = function () {
    'use strict'

    var _name = 'Dash';
    var _icon = 'images/pages/dash.png';
    var _description = 'The AGServer dashboard. This is the interesting bit where you can create and use your own custom dashboards using the built in editor';
    var _showVideo = false;
    var _order = 2;

    function init() {
        buildUI();
    }

    function buildUI() {
        buildToolbar();
    }

    function buildToolbar() {
        var template = '\
            <i class="fi-page-edit"></i>\
        ';

        var html = Mustache.to_html(template);
        jQuery('#page-icons').html(html);
    }

    function update(data) {

    }

    return {
        showVideo: _showVideo,

        init: function () {
            return init();
        },

        update: function (data) {
            update(data);
        }
    }
}
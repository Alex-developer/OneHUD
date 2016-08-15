var OneHUDViewSettings = function () {
    'use strict'

    var _name = 'Settings';
    var _icon = 'images/pages/settings.png';
    var _menuIcon = 'images/pages/settings-menu.png';
    var _description = 'The AGServer settings page. Manage settings for your device';
    var _showVideo = true;
    var _order = 5;

    function init() {
        buildPage();
        setSettings();
        addEventHandlers();
    }

    function buildPage() {
        var options = OneHUDUI.options();
        var template = '\
            <div class="row">\
               <div class="medium-6 columns callout halfOpacity" data-pg-collapsed>\
                  <form>\
                     <div class="row">\
                        <div class="columns medium-12">\
                           <h3>Client Settings</h3>\
                        </div>\
                     </div>\
                     <div class="row" data-pg-collapsed>\
                        <div class="columns medium-6">\
                           <h4>Play Videos</h4>\
                        </div>\
                        <div class="columns medium-6">\
                           <div class="switch">\
                              <input class="switch-input" id="showvideo" type="checkbox" name="showvideo">\
                              <label class="switch-paddle" for="showvideo">\
                              <span class="show-for-sr">Download Kittens</span>\
                              <span class="switch-active" aria-hidden="true">Yes</span>\
                              <span class="switch-inactive" aria-hidden="true">No</span>\
                              </label>\
                           </div>\
                        </div>\
                     </div>\
                     <div class="row" data-pg-collapsed>\
                        <div class="columns medium-6">\
                           <h4>Default Page</h4>\
                        </div>\
                        <div class="columns medium-6">\
                            <select id="defaultpage">\
                            </select>\
                        </div>\
                     </div>\
                  </form>\
               </div>\
               <div class="medium-6 columns">          </div>\
            </div>\
        ';

        var html = Mustache.to_html(template);
        jQuery('#content').html(html);

        var settings = OneHUDUI.getSetting();
        jQuery.each(options.Pages, function (index, page) {
            var option = jQuery('<option/>', { value: page.Name }).text(page.Name);
            jQuery('#defaultpage').append(option);
        });
        jQuery('#defaultpage option[value="' + settings.defaultPage + '"]').prop('selected', true);
    }

    function setSettings() {
        var settings = OneHUDUI.getSetting();

        if (settings.showVideos) {
            jQuery('input[name=showvideo]').prop('checked', true);
        } else {
            jQuery('input[name=showvideo]').prop('checked', false);
        }
    }

    function addEventHandlers() {
        jQuery('input[name=showvideo]').on("change", function () {
            if (jQuery(this).is(':checked')) {
                OneHUDUI.setSetting('showVideos', true);
            } else {
                OneHUDUI.setSetting('showVideos', false);
            }
            OneHUDUI.setupBackgroundVideo();
        });

        jQuery('#defaultpage').on('change', function (e) {
            var page = jQuery(this).val();
            OneHUDUI.setSetting('defaultPage', page);
        });
    }

    return {
        showVideo: _showVideo,

        init: function () {
            return init();
        }
    }
}
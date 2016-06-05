var OneHUDPropertyPageIMAGE = function () {
    'use strict';

    var _propertiesWindow = null;

    function init(propertiesWindow) {
        _propertiesWindow = propertiesWindow;
        clearPropertiesWindow();
        setupPropertiesWindow();
    }

    function clearPropertiesWindow() {
        _propertiesWindow.content.empty();
    }

    function setupPropertiesWindow() {

        // https://github.com/ValYouW/jqPropertyGrid
        var template = '\
            <div class="row collapse">\
            </div>';
        var html = Mustache.to_html(template, {});
        _propertiesWindow.content.append(html);
    }

    return {
        init: function (propertiesWindow) {
            init(propertiesWindow);
        }
    }
}();
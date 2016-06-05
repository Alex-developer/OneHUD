var OneHUDPropertyPageNOPROPERTIES = function () {
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
        var template = '\
            <div class="row collapse">\
                <h2>No Properties</h2>\
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
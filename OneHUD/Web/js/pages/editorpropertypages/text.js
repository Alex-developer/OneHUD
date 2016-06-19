var OneHUDPropertyPageTEXT = function () {
    'use strict';

    var _propertiesWindow = null;
    var _propertyGridId = null;

    function init(propertiesWindow) {
        _propertiesWindow = propertiesWindow;
        clearPropertiesWindow();
        setupPropertiesWindow();
    }

    function clearPropertiesWindow() {
        _propertiesWindow.content.empty();
    }

    function setupPropertiesWindow() {

        _propertyGridId = OneHUDUI.getNextId();

        var template = '\
            <div class="row collapse" id="' + _propertyGridId + '">\
            </div>';
        var html = Mustache.to_html(template, {});
        _propertiesWindow.content.append(html);

        var theMeta = {
            left: { group: 'Position', name: 'Left', type: 'number', options: { min: 0, max: 20000, step: 1 } },
            top: { group: 'Position', name: 'Top', type: 'number', options: { min: 0, max: 20000, step: 1 } },
            width: { group: 'Position', name: 'Width', type: 'number', options: { min: 0, max: 20000, step: 1 } },
            height: { group: 'Position', name: 'Height', type: 'number', options: { min: 0, max: 20000, step: 1 } },
            font: { group: 'Font', name: 'Font', description: 'The font name' },
            fontautosize: { group: 'Font', name: 'Auto Size',type: 'boolean', description: 'Auto size the font' },
            fontsize: { group: 'Font', name: 'Font Size', description: 'The font size name' }
        };

        var theObj = {
            left: 100,
            top: 250,
            width: 600,
            height: 400,
            font: 'LED',
            fontautosize: true,
            fontsize: '10'
        };

        jQuery('#' + _propertyGridId).jqPropertyGrid(theObj, theMeta);

    }

    return {
        init: function (propertiesWindow) {
            init(propertiesWindow);
        }
    }
}();
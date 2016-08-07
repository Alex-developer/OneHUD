var OneHUDPropertyPageIMAGE = function () {
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
            left: { group: 'Position', name: 'left', type: 'number', options: { min: 0, max: 20000, step: 1 } },
            top: { group: 'Position', name: 'top', type: 'number', options: { min: 0, max: 20000, step: 1 } },
            width: { group: 'Position', name: 'width', type: 'number', options: { min: 0, max: 20000, step: 1 } },
            height: { group: 'Position', name: 'height', type: 'number', options: { min: 0, max: 20000, step: 1 } },
            filename: { group: 'File', name: 'Filename', description: 'The image file name' },
            filename1: { group: 'File', name: 'Filename1', description: 'The image file name' }
        };

        var theObj = {
            left: 100,
            top: 250,
            width: 600,
            height: 400,
            filename: 'image.png',
            filename1: 'ff'
        };

        jQuery('#' + _propertyGridId).agProp(theObj, theMeta);

    }

    return {
        init: function (propertiesWindow) {
            init(propertiesWindow);
        }
    }
}();
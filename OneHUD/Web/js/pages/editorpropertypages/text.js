var OneHUDPropertyPageTEXT = function () {
    'use strict';

    var _propertiesWindow = null;
    var _propertyGridId = null;
    var _fonts = [
        { text: 'LED', value: 'Led' },
        { text: 'Georgia', value: 'Georgia' },
        { text: 'Arial', value: 'Arial' },
        { text: 'Tahoma', value: 'Tahoma' },
        { text: 'Verdana', value: 'Verdana' }
    ];
    var _alignment = [
        { text: 'Left', value: 'Left' },
        { text: 'Right', value: 'Right' },
        { text: 'Center', value: 'Center' }
    ];

    function init(propertiesWindow, data) {
        _propertiesWindow = propertiesWindow;
        clearPropertiesWindow();
        setupPropertiesWindow(data);
    }

    function clearPropertiesWindow() {
        _propertiesWindow.content.empty();
    }

    function setupPropertiesWindow(data) {

        _propertyGridId = OneHUDUI.getNextId();

        var template = '\
            <div class="row collapse" id="' + _propertyGridId + '">\
            </div>';
        var html = Mustache.to_html(template, {});
        _propertiesWindow.content.append(html);

        var propertyFields = {
            left: { group: 'Position', name: 'Left', type: 'number', options: { min: 0, max: 20000, step: 1 } },
            top: { group: 'Position', name: 'Top', type: 'number', options: { min: 0, max: 20000, step: 1 } },
            width: { group: 'Position', name: 'Width', type: 'number', options: { min: 0, max: 20000, step: 1 } },
            height: { group: 'Position', name: 'Height', type: 'number', options: { min: 0, max: 20000, step: 1 } },
            alignment: { group: 'Position', name: 'Alignment', type: 'options', options: _alignment },
            font: { group: 'Font', name: 'Font', type: 'options', options: _fonts },
            fontautosize: { group: 'Font', name: 'Auto Size',type: 'boolean', description: 'Auto size the font' },
            fontsize: { group: 'Font', name: 'Font Size', description: 'The font size name' }
        };

        var propertyData = {
            left: 100,
            top: 250,
            width: 600,
            height: 400,
            alignment: 'left',
            font: 'LED',
            fontautosize: true,
            fontsize: '10'
        };

        propertyData.left = data.css.left;
        propertyData.top = data.css.top;
        propertyData.width = data.css.width;
        propertyData.height = data.css.height;

        jQuery('#' + _propertyGridId).jqPropertyGrid(propertyData, propertyFields);

    }

    return {
        init: function (propertiesWindow, data) {
            init(propertiesWindow, data);
        }
    }
}();
var OneHUDPropertyPageTEXT = function () {
    'use strict';

    var _propertiesWindow = null;
    var _propertyGridId = null;
    var _widget = null;
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

    function setupPropertiesWindow(widget) {

        _widget = widget;
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
            'text-align': { group: 'Position', name: 'Alignment', type: 'options', options: _alignment },
            'font-family': { group: 'Font', name: 'Font', type: 'options', options: _fonts },
            fontautosize: { group: 'Font', name: 'Auto Size',type: 'boolean', description: 'Auto size the font' },
            fontsize: { group: 'Font', name: 'Font Size', description: 'The font size name' }
        };

        var propertyData = {
            rows: {
                left: 100,
                top: 250,
                width: 600,
                height: 400,
                alignment: 'left',
                'font-family': 'LED',
                fontautosize: true,
                fontsize: '10',
                'text-align': 'left'
            }
        };

        propertyData.left = _widget.instance.properties().css.left;
        propertyData.top = _widget.instance.properties().css.top;
        propertyData.width = _widget.instance.properties().css.width;
        propertyData.height = _widget.instance.properties().css.height;

        jQuery('#' + _propertyGridId).agProp(propertyData, propertyFields, propertyUpdated);

    }

    function propertyUpdated(grid, name, value) {
        if (typeof _widget.instance.properties().css[name] === 'number') {
            value = parseFloat(value);
        }
        _widget.instance.properties().css[name] = value;
        _widget.instance.element().css(_widget.instance.properties().css);
    }

    return {
        init: function (propertiesWindow, data) {
            init(propertiesWindow, data);
        }
    }
}();
//# sourceURL=/js/pages/editorpropertypages/text.js
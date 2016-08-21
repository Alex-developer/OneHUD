var OneHUDTRACKMAPWidget = function () {
    'use strict';

    var _name = 'Track Map';
    var _icon = '/images/widgets/gear.png';
    var _labels = ['Track Map'];
    var _tab = 'Environment';
    var _supports = ['Project Cars'];
    var _propertypage = 'trackmap';
    var _requires = ['/js/konva/konva.js'];

    var _el = null;
    var _uri = OneHUDUI.getURI();

    var _track = null;
    var _stage;
    var _trackLayer;
    var _carLayer;

    var _properties = {
        type: 'trackmap',
        css: {
            left: 0,
            top: 0,
            width: 100,
            height: 100,
            'font-family': 'Led',
            'font-weight': 'bold',
            'text-align': 'left',
            color: 'white'
        }
    };
    var _messages = ['telemetry'];

    function init(element, properties) {
        _el = element;

        if (properties !== undefined) {
            _properties = properties;
        }

        head.load(_requires, function () {
            loadTrack();
            buildUI();
        });
    }

    function loadTrack() {
        jQuery.ajax({
            url: _uri + 'TrackRecorder',
            cache: false,
            method: 'POST',
            data: {
                action: 'LoadTrack',
                data: {}
            }
        }).success(function (result) {
            _track = result;
            drawTrack();
        }).fail(function () {
            var tt = 56;
        });
    }

    function destroy() {
        _initialised = false;
        jQuery(_el).remove();
    }

    function buildUI() {
        setupStage();
    }

    function drawTrack() {

        if (_track !== null) {


            var width = jQuery(_el).width();
            var height = jQuery(_el).height();

            var xScale = width / _track.Track.TrackBounds.Width;
            var yScale = height / _track.Track.TrackBounds.Height;

            var points = [];
            jQuery.each(_track.Track.TrackPoints, function (index, point) {
                points.push(point.X * xScale);
                points.push(point.Y * yScale);
            });
            var trackMap = new Konva.Line({
                points: points,
                stroke: '#ff0000',
                strokeWidth: 2,
                lineCap: 'round'
            });
            _trackLayer.destroyChildren();
            _trackLayer.add(trackMap);
            _trackLayer.draw();
        }
    }

    function update(data) {

    }

    function resize() {
    }

    function setupStage() {
        _stage = new Konva.Stage({
            container: jQuery(_el).attr('id'),
            width: jQuery(_el).width(),
            height: jQuery(_el).height()
        });
        _trackLayer = new Konva.Layer();
        _carLayer = new Konva.Layer();
        _stage.add(_trackLayer);
        _stage.add(_carLayer);
    }

    return {
        name: _name,
        icon: _icon,
        messages: _messages,
        labels: _labels,
        tab: _tab,
        supports: _supports,
        propertypage: _propertypage,

        properties: function (properties) {

            if (properties !== undefined) {
                _properties = properties;
            }
            return _properties;
        },

        element: function () {
            return _el;
        },

        init: function (element, properties) {
            return init(element, properties);
        },

        destroy: function () {
            destroy();
        },

        update: function (data) {
            update(data);
        },

        resize: function () {
            resize();
        }

    }
};
//# sourceURL=/js/widgets/trackmap/trackmap.js
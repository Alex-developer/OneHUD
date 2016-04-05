var AGServerDataReader = function () {
    'use strict';

    var _socket = null;
    var _config = {
        urn: '',
        frequency: 0,
        eventName: ''
    };
    var _timer = null;
    var _usingWebSockets = null;
    var _processing = false;
    var _url;
    var _uri;

    function init(config) {
        _config = config;

        if (location.port !== '') {
            _url = location.host.replace(':' + location.port, '') + ':' + location.port;
            _uri = 'http://' + _url;
        } else {
            _url = location.host;
            _uri = 'http://' + _url + '/';
        }

        if (typeof WebSocket === 'function') {
            _usingWebSockets = true;
        } else {
            _usingWebSockets = false;
        }

        start();
    }

    function start() {
        if (_usingWebSockets) {
            startSocket();
        } else {
            startJSON();
        }
    }

    function stop() {
        clearInterval(_timer);
        if (_usingWebSockets) {
            stopSocket();
        } else {
            stopJSON();
        }
    }

    function startSocket() {
        _socket = new WebSocket('ws://' + _url + '/' +_config.urn);

        _socket.onopen = function (e) {
            _timer = setInterval(readData(), _config.frequency);
        }

        _socket.onclose = function (e) {
            debugger;
        }

        _socket.onerror = function (e) {
            sendMessage('error', _config.eventName, '');
        }

        _socket.onmessage = function (e) {
            var data = JSON.parse(e.data);
            if (data !== null) {
                sendMessage('data', _config.eventName, data);
            }
            _processing = false;
        }
    }

    function stopSocket() {
        if (_socket.readyState === 1) {
            _socket.close();
        }
    }

    function startJSON() {
        _timer = setInterval(readData(), _config.frequency);
    }

    function stopJSON() {

    }

    function readData() {
        if (!_processing) {
            _processing = true;

            if (_usingWebSockets) {
                _socket.send('.');
            } else {
                Twix.ajax({
                    url: _uri + '/' + _config.urn + '/' + '?nonce=' + (new Date()).getTime(),
                    success: function (data) {
                        _processing = false;
                        if (data !== null) {
                            sendMessage('data', _config.eventName, data);
                        }
                    },
                    error: function () {
                        _processing = false;
                    }
                });
            }
        }
    }

    function sendMessage(action, dataType, data) {
        var message = {
            'action': action,
            'datatype': dataType,
            'data': data
        };
        self.postMessage(JSON.stringify(message));
    }

    return {
        init: function (config) {
            init(config);
        },

        stop: function () {
            stop();
        }
    }
};

//# sourceURL=/js/framework/datareader.js
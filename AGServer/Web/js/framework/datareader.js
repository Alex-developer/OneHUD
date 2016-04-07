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
            setupSocket();
        } else {
            _usingWebSockets = false;
        }
    }

    function setupSocket() {
        _socket = new WebSocket('ws://' + _url + '/' + _config.urn);

        _socket.onopen = function (e) {
        }

        _socket.onclose = function (e) {
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

    function start() {
        _timer = setInterval(readData, _config.frequency);
    }

    function stop() {
        clearInterval(_timer);
    }

    function destroy() {
        if (_usingWebSockets) {
            if (_socket.readyState === 1) {
                _socket.close();
            }
        }
    }

    function readData() {
        if (!_processing) {
            _processing = true;

            if (_usingWebSockets) {
                if (_socket.readyState === 1) {
                    _socket.send('.');
                } else {
                    _processing = false;
                }
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

        start: function() {
            start();
        },

        stop: function () {
            stop();
        },

        destroy: function () {

        }
    }
};

//# sourceURL=/js/framework/datareader.js
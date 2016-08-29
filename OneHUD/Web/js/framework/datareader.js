/*
Copyright 2016 Alex Greenland

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 */
var OneHUDDataReader = function () {
    'use strict';

    importScripts('/js/twix/twix.js');

    var _socket             = null;     // The websocket handle
    var _config             = {         // Default config object for this data reader
        urn: '',                        // The urn for this data reader
        frequency: 0,                   // The data polling frequency in ms for this data reader
        eventName: '',                  // The event name to send with data
        persistant: false               // Flag to indicate this data reader should never be stopped and always attempt connections
    };
    var _timer              = null;     // The timer used for reading data
    var _usingWebSockets    = null;     // Flag to indicate if websockets or Ajax is being used
    var _processing         = false;    // Flag to indicate if a read is currentl in progress, prevents multiple reads due to fast timer
    var _url                = null;     // The url to open for a websocket
    var _uri                = null;     // The uri to send Ajax requests to
    var _datamanager        = null;     // The Datamanager that owns this data reader
    var _connecting         = false;    // Flag to indicate if a websocket is currently attempting a connection
    var _reading            = false;    // Flag to indicate if this data reader should be reading data or idle

    /**
    * Initialise the Data Reader
    *
    * @function
    * @param {object} config - The configuration for this instance
    * @param {object} datamanager - The data manager thats handling this instance
    */
    function init(config, datamanager) {
        _config = config;
        _datamanager = datamanager;

        if (location.port !== '') {
            _url = location.host.replace(':' + location.port, '') + ':' + location.port;
            _uri = 'http://' + _url;
        } else {
            _url = location.host;
            _uri = 'http://' + _url + '/';
        }

        if (typeof WebSocket !== undefined) {
            _usingWebSockets = true;
        } else {
            _usingWebSockets = false;
        }

        startTimer();
        if (_config.persistant) {
            _reading = true;
        }
    }

    /*
    * Start the data reader reading.
    * @function
    */
    function start() {
        _reading = true;
    }

    /*
    * Stop the data reader from reading. For persistant connections the data reader
    * is always reading.
    * @function
    */
    function stop() {
        if (!_config.persistant) {
            _reading = false;
        }
    }

    /*
    * Reads data from the server.
    *
    * This function is called by the timer associated with this data reader and is responsible
    * for ensuring that the websocket, if being used, is connected.
    *
    * @function
    */
    function readData() {
        if (_reading) {
            if (_usingWebSockets) {
                if (!_connecting) {
                    if (_socket !== null && _socket.readyState === 1) {
                        getData();
                    } else {
                        _connecting = true;
                        setupSocket();
                    }
                }
            } else {
                getData();
            }
        }
    }

    /*
    * Creates a web socket and binds the handlers to it.
    * @function
    */
    function setupSocket() {
        _socket = null;
        _socket = new WebSocket('ws://' + _url + '/' + _config.urn);
        _socket.onopen = function (e) {
            _connecting = false;
        };
        _socket.onclose = function (e) {
            _connecting = false;
            sendMessage('error', _config.eventName, '');
        };
        _socket.onerror = function (e) {
            _connecting = false;
            sendMessage('error', _config.eventName, '');
        };
        _socket.onmessage = function (e) {
            var data = JSON.parse(e.data);
            if (data.Data !== undefined) {
                data = data.Data;
            }
            if (data !== null) {
                sendMessage('data', _config.eventName, data);
            }
            _processing = false;
        };
    }

    /*
    * Starts the timer for this data reader
    * @function
    */
    function startTimer() {
        if (_timer === null) {
            _timer = setInterval(readData, _config.frequency);
        }
    }

    /*
    * Stops the timer for this data reader
    * @function
    */
    function stopTimer() {
        if (_timer !== null) {
            clearInterval(_timer);
            _timer = null;
        }
    }

    /*
    * Returns true or false depending upon the connected data of this data reader
    * @function
    * @returns {Boolean} result true if the data reader is connected false if not
    */
    function connected() {
        var result = true;
        if (_usingWebSockets) {
            if (_socket.readyState !== 1) {
                result = false;
            }
        }

        return result;
    }

    /*
    * Destroys this instance of the data reader
    * @function
    */
    function destroy() {
        stopTimer();
        if (_usingWebSockets) {
            if (_socket.readyState === 1) {
                _socket.close();
            }
        }
    }

    /*
    * Does the hard work and actually requests data.
    * @function
    */
    function getData() {
        if (!_processing) {
            _processing = true;

            if (_usingWebSockets) {
                if (_socket.readyState === 1) {
                    _socket.send('.');
                }
            } else {
                Twix.ajax({
                    url: _uri + _config.urn + '?nonce=' + (new Date()).getTime(),
                    type: 'POST',
                    success: function (result) {
                        _processing = false;
                        if (result !== null) {
                            var data = null;
                            if (result.Data !== undefined) {
                                data = result.Data;
                            } else {
                                data = result;
                            }
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

    /*
    * Sends a message to the datamanager
    * @function
    * @param {string} action - The action
    * @param {string} dataType - The dataType
    * @param {object} data - The data
    */
    function sendMessage(action, dataType, data) {
        var message = {
            'action': action,
            'datatype': dataType,
            'data': data
        };
        _datamanager.sendMessage(message);
    }

    return {
        init: function (config, datamanager) {
            init(config, datamanager);
        },

        start: function () {
            start();
        },

        stop: function () {
            stop();
        },

        destroy: function () {
            destroy();
        },

        persistant: function () {
            return _config.persistant;
        },

        connected: function () {
            connected();
        }
    };
};

//# sourceURL=/js/framework/datareader.js
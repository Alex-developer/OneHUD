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
var OneHUDDataManager = function () {
    'use strict';

    importScripts('/js/framework/datareader.js');
    importScripts('/js/framework/datareaderconfig.js');

    var _readers                    = [];       // Array of data readers
    var _connectedToOneHUDServer    = false;    // Flag indicating if the OneHUD server is connected
    var _connectedToGame            = false;    // Flag indicating if the OneHUD server is connected to a game

    /*
    * Initialise the Data manager.
    *
    * @function
    */
    function init() {
        createDataReader('heartbeat', true);
        createDataReader('timing');
        createDataReader('telemetry');
    }

    /*
    * Creates a data reader
    *
    * @function
    * @param {string} dataReader - The name of the data reader to create
    * @param {Boolean} start - true to start the datareader false to just create it
    */
    function createDataReader(dataReader, start) {

        if (start === undefined) {
            start = false;
        }

        var config = OneHUDConfig.getDataReaderConfig(dataReader);
        if (config !== null) {
            _readers[dataReader] = new OneHUDDataReader();
            _readers[dataReader].init(config, OneHUDDataManager);

            if (start) {
                _readers[dataReader].start();
            }
        }
    }
    
    /*
    * Start all of the data readers
    *
    * @function
    */
    function startReaders() {
        for (var reader in _readers) {
            _readers[reader].start();
        }
    }

    /*
    * Stop all of the data readers
    *
    * @function
    */
    function stopReaders() {
        for (var reader in _readers) {
            _readers[reader].stop();
        }
    }

    /*
    * Process a message. The messages are sent to us from the ui via
    * postmessage, since we are running in web worker
    *
    * @function
    * @param {object} e - The event containing the message to process
    */
    function processMessage(e) {
        var command = e.data;

        switch (command.cmd) {
            case 'start':
                startReaders();
                break;

            case 'stop':
                stopReaders();
                break;

            case 'init':
                init();
                break;
        }
    }

    /*
    * Sends a message to the data manager first amending the message as required. Here
    * we check for certain conditions and send the datamanager more sensible messages i.e.
    * if the heartbeat stops we send a serverdisconnected message.
    *
    * @function
    * @param {object} message - The message to send
    */
    function sendMessage(message) {

        switch (message.action) {
            case 'data':
                switch (message.datatype) {
                    case 'heartbeat':
                        message = handleHeartBeat(message);
                        break;
                }
                break;

            case 'error':
                switch (message.datatype) {
                    case 'heartbeat':
                        message = handleHeartBeat(message);
                        break;
                }
                break;
        }

        self.postMessage(JSON.stringify(message));
    }

    /*
    * Reacts to changes in the hearbeat message, modifying the message so that
    * we send a more sensible state to the datamanager
    *
    * @function
    * @param {object} message - The message to send
    */
    function handleHeartBeat(message) {
        switch (message.action) {
            case 'data':
                if (!_connectedToOneHUDServer) {
                    _connectedToOneHUDServer = true;
                    message = {
                        'action': 'serverconnected',
                        'datatype': '',
                        'data': ''
                    };
                } else {
                    if (!_connectedToGame) {
                        if (message.data.Game !== null) {
                            message.action = 'gameconnected';
                            _connectedToGame = true;
                        }
                    } else {
                        if (message.data.Game === null) { 
                            message.action = 'gamedisconnected';
                            _connectedToGame = false;
                        }
                    }
                }
                break;

            case 'error':
                if (_connectedToOneHUDServer) {
                    _connectedToOneHUDServer = false;
                    message = {
                        'action': 'serverdisconnected',
                        'datatype': '',
                        'data': ''
                    };
                    stopReaders();
                }
                break;

        }

        return message;
    }

    return {
        processMessage: function (e) {
            processMessage(e);
        },

        connect: function () {
            connect();
        },

        sendMessage: function (message) {
            sendMessage(message);
        }
    };
}();

self.addEventListener('message', OneHUDDataManager.processMessage, false);

//# sourceURL=/js/framework/datamanager.js
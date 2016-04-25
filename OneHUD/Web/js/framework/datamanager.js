var OneHUDDataManager = function () {
    'use strict';

    importScripts('/js/framework/datareader.js');
    importScripts('/js/framework/datareaderconfig.js');

    var _readers = [];

    setupDataReader('heartbeat', true);
    setupDataReader('telemetry');

    
    function setupDataReader(dataReader, start) {

        if (start === undefined) {
            start = false;
        }

        var config = OneHUDConfig.getDataReaderConfig(dataReader);
        if (config !== null) {
            _readers[dataReader] = new OneHUDDataReader();
            _readers[dataReader].init(config);

            if (start) {
                _readers[dataReader].start();
            }
        }
    }
    
    function startReaders() {
        for (var reader in _readers) {
            if (!_readers[reader].persistant) {
                _readers[reader].start();
            }
        };
    }

    function stopReaders() {
        for (var reader in _readers) {
            if (!_readers[reader].persistant) {
                _readers[reader].stop();
            }
        };
    }

    function processMessage(e) {
        var command = e.data;

        switch (command.cmd) {
            case 'start':
                startReaders();
                break;

            case 'stop':
                stopReaders();
                break;
        }
    }

    return {
        processMessage: function (e) {
            processMessage(e);
        },

        connect: function () {
            connect();
        }
    };
}();

self.addEventListener('message', OneHUDDataManager.processMessage, false);

//# sourceURL=/js/framework/datamanager.js
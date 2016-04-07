var AGServerDataManager = function () {
    'use strict';

    importScripts('/js/twix/twix.js');
    importScripts('/js/framework/datareader.js');
    importScripts('/js/framework/datareaderconfig.js');

    var _readers = [];

    setupDataReader('heartbeat', true);
    setupDataReader('telemetry');
  //  setupDataReader('timingdata');

    
    function setupDataReader(dataReader, start) {

        if (start === undefined) {
            start = false;
        }

        var config = AGServerConfig.getDataReaderConfig(dataReader);
        if (config !== null) {
            _readers[dataReader] = new AGServerDataReader();
            _readers[dataReader].init(config);

            if (start) {
                _readers[dataReader].start();
            }
        }
    }

    function processMessage(e) {

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

self.addEventListener('message', AGServerDataManager.processMessage, false);

//# sourceURL=/js/framework/datamanager.js
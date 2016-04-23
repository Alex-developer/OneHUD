var AGServerConfig = function () {
    'use strict';

    var _dataReaders = {
        heartbeat: {
            urn: 'HeartBeat',
            frequency: 500,
            eventName: 'heartbeat',
            persistant: true
        },
        telemetry: {
            urn: 'Telemetry',
            frequency: 50,
            eventName: 'telemetry',
            persistant: false
        },
        timingdata: {
            urn: 'Timing',
            frequency: 500,
            eventName: 'timing',
            persistant: false
        }
    };

    return {

        getDataReaderConfig: function (dataReader) {
            var result = null;
            
            if (_dataReaders[dataReader] !== undefined) {
                result = _dataReaders[dataReader];
            }
            return result;
        }
    }
}();

//# sourceURL=/js/framework/config.js
var AGServerConfig = function () {
    'use strict';

    var _dataReaders = {
        heartbeat: {
            urn: 'HeartBeat',
            frequency: 500,
            eventName: 'heartbeat'
        },
        cardata: {
            urn: 'Telemetry',
            frequency: 10,
            eventName: 'telemetry'
        },
        timingdata: {
            urn: 'Timing',
            frequency: 500,
            eventName: 'timing'
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
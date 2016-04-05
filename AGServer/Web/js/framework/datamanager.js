var AGServerDataManager = function () {
    'use strict';

    importScripts('/js/twix/twix.js');
    importScripts('/js/framework/datareader.js');

    var _readers = [];

    _readers['cardata'] = new AGServerDataReader();
    _readers['cardata'].init({
        urn: 'Handshake',
        frequency: 50,
        eventName: 'cardata'
    });

    function processMessage(e) {

    }

    return {
        processMessage: function (e) {
            processMessage(e);
        }
    };
}();

self.addEventListener('message', AGServerDataManager.processMessage, false);

//# sourceURL=/js/framework/datamanager.js
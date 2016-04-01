var AGServerDataReader = function () {
    'use strict';

    importScripts('/js/twix/twix.js');

    var _uri;
    var _allDataURI;


    if (location.port !== '') {
        _uri = location.host.replace(':' + location.port, '') + ':' + location.port;
        _allDataURI = 'http://' + _uri;
    } else {
        _uri = location.host;
        _allDataURI = 'http://' + _uri + '/';
    }

    var _pollingWebsocket = null;

    if (typeof WebSocket === 'function') {
        _usingWebSockets = true;
    } else {
        _usingWebSockets = false;
    }


    function pollTimer() {

    }

    function sendMessage(action, dataType, data) {

    }

    function processMessage(e) {

    }

    function stopDataReaders() {

    }

    function startDataReaders() {

    }

    function readData(timer) {

    }

    return {
        processMessage: function (e) {
            processMessage(e);
        }
    };
}();


self.addEventListener('message', AGServerDataReader.processMessage, false);
//# sourceURL=/js/framework/datareader.js
var AGServerBootstrap = function () {
    'use strict';

    var _worker = null;
    var _uri = null;
    if (location.port !== '') {
        _uri = location.protocol + '//' +  location.host.replace(':' + location.port, '') + ':' + location.port;
    } else {
        _uri = location.protocol + '//' + location.host + '/';
    }

    function init() {
        initDataReader();

        jQuery.ajax({
            url: _uri + 'Startup',
            cache: false
        }).done(function () {
            AGServerUI.init();
        });        
    }

    function initDataReader() {
        _worker = new Worker('/js/datareader.js');
        _worker.addEventListener('message', function (e) {
        });
    }

    return {
        run: function () {
            init();
        }
    }
}();
//# sourceURL=/js/framework/bootstrap.js
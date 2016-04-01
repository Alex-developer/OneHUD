var AGServerBootstrap = function () {
    'use strict';

    var _worker = null;

    function init() {
        initDataReader();

        AGServerUI.init();
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
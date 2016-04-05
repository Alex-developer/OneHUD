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

        getStartupData().done(function (options) {
            AGServerClassCache.init(options).done(function (options) {
                AGServerUI.init(options);
            });
        });
      
    }

    function getStartupData() {
        var deferred = jQuery.Deferred();

        jQuery.ajax({
            url: _uri + 'Startup',
            cache: false
        }).done(function (result) {
            var options = JSON.parse(result);
            deferred.resolve(options);
        });

        return deferred.promise();
    }

    function initDataReader() {
        _worker = new Worker('/js/framework/datamanager.js');
        _worker.addEventListener('message', function (e) {
            switch (e.data.action) {
                case 'error':
                    break;
            }
        });
    }

    return {
        run: function () {
            init();
        }
    }
}();
//# sourceURL=/js/framework/bootstrap.js
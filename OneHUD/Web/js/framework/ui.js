var OneHUDUI = function () {
    'use strict';

    var _options = null;
    var _currentPage = null;
    var _worker = null;
    var _currentGame = null;
    var _id = 0;
    var _connected = false;
    var _cookieName = 'OneHUD';
    var _settings = null;
    var _defaultSettings = {
        showVideos: true,
        defaultPage: 'Home',
        debugMode: false
    };

    function SetOffCanvasHeight() {
        var height = jQuery(window).height();
        var contentHeight = jQuery('.off-canvas-content').height();
        if (contentHeight > height) { height = contentHeight; }

        jQuery('.off-canvas-wrapper').height(height);
        jQuery('.off-canvas-wrapper-inner').height(height);
        jQuery('.off-canvas').height(height);
        jQuery('#content').height(height);
    }

    function run() {
        readSettings();
        blockUI();
        createDataReader();
        addGlobalEvents()
    }

    function init() {
        var deferred = jQuery.Deferred();
        getStartupData().done(function (options) {
            OneHUDClassCache.init(options).done(function (options) {
                _options = options;
                setupMenu();
                addHandlers();
                pageLoader(getBrowserHashPage());
                deferred.resolve();
            });
        });
        return deferred.promise();
    }

    function readSettings() {
        var cookieSettings = Cookies.getJSON(_cookieName);
        if (cookieSettings === undefined) {
            cookieSettings = _defaultSettings;
        }
        _settings = cookieSettings;

        if (_settings.DebugMode === undefined) {
            _settings.DebugMode = false;
        }
    }

    function saveSettings() {
        Cookies.set(_cookieName, _settings);
    }

    function getSetting(setting) {
        var result = _settings;
        if (setting !== undefined) {
            if (_settings[setting] !== undefined) {
                result = _settings[setting];
            } else {
                retult = null;
            }
        }

        return result;
    }

    function setSetting(setting, value) {
        _settings[setting] = value;
        saveSettings();
    }

    function createDataReader() {
        _worker = new Worker('/js/framework/datamanager.js');
        initDataReader();
    }

    function getStartupData() {
        var deferred = jQuery.Deferred();

        jQuery.ajax({
            url: getURI() + 'Startup',
            cache: false,
            method: 'POST',
            data: {
                width: jQuery(window).width(),
                height: jQuery(window).height()
            }
        }).done(function (options) {
            deferred.resolve(options);
        });

        return deferred.promise();
    }

    function addGlobalEvents() {
        jQuery(window)
            .load(function () {
                SetOffCanvasHeight();
            })
            .resize(function () {
                SetOffCanvasHeight();
            });

     /*   jQuery(window).swipe({
            swipe: function (event, direction, distance, duration, fingerCount, fingerData) {
                if (direction === 'right') {
                    jQuery('#offCanvas').foundation('open');
                }
            }
        });
        */
        _worker.addEventListener('message', function (e) {
            dataEvent(e);
        });
    }

    function getBrowserHashPage() {
        var page = null;
        var hash = location.hash.substring(location.hash.indexOf('#') + 1);
        if (hash !== '') {
            if (OneHUDClassCache.pageExists(hash)) {
                page = hash;
            }
        }
        return page;
    }

    function pageLoader(page) {

        if (page === undefined || page === null) {
            if (_settings.defaultPage !== undefined) {
                page = _settings.defaultPage;
            } else {
                page = _options.DefaultPage;
            }
        }

        location.hash = page;

        clearContent();
        pageLostFocus();
        _currentPage = OneHUDClassCache.getPage(page);
        var deferred = _currentPage.init();
        if (deferred === undefined) {
            updateHeader();
            setupBackgroundVideo();
            SetOffCanvasHeight();
            pageGotFocus();
            jQuery('#offCanvas').foundation('close');
            startDataReader();
        } else {
            deferred.done(function (result) {
                updateHeader();
                setupBackgroundVideo();
                SetOffCanvasHeight();
                pageGotFocus();
                jQuery('#offCanvas').foundation('close');
                startDataReader();
            });
        }
        
    }

    function pageGotFocus() {
        if (_currentPage !== null) {
            if (_currentPage.gotFocus !== undefined) {
                _currentPage.gotFocus('#page-custom-menu');
            }
        }
    }

    function pageLostFocus() {
        if (_currentPage !== null) {
            if (_currentPage.lostFocus !== undefined) {
                _currentPage.lostFocus('#page-custom-menu');
            }
        }
    }

    function updateHeader() {
        var text = 'Waiting';

        if (_currentGame !== null) {
            text = _currentGame.GameLongName;
        }
        jQuery('#connected-game').html(text);

        if (_currentPage.update !== undefined) {
            _currentPage.update('builtin', text);
        }

    }

    function setupMenu() {
        jQuery('#page-menu').html('');
        jQuery.each(_options.Pages, function (index, page) {
            var image = jQuery('<img>', { 'src': page.Menuicon, 'width': '40px' });
            var menuLink = jQuery('<a>', { 'href': '#', 'data-action': 'loadpage', 'data-page': page.Name, 'class': 'button hollow float-left mr5' }).append(image);
            jQuery('#page-menu').append(menuLink);
        });
    }

    function setupBackgroundVideo() {
        var showVideo = false;

        if (_settings.showVideos) {
            if (_currentPage.showVideo !== undefined) {
                if (_currentPage.showVideo) {
                    showVideo = true;
                }
            }
        }

        jQuery('#background-video').remove();
        if (showVideo) {
            jQuery('body').prepend('<video id="background-video" loop="loop" class="background-video" src="videos/background.mp4" type="video/webm" autoplay></video>');
        }
    }

    function clearContent() {
        jQuery('#content').html('');
        jQuery('#page-icons').html('');
        jQuery('.gridlines').remove();
    }

    function blockUI() {
        if (!_settings.debugMode) {
            jQuery.blockUI({
                css: {
                    width: '40%'
                },
                message: '<h3><img src="images/busy.gif" /> Waiting for OneHUD Server</h3>'
            });
        }
    }

    function unblockUI() {
        if (!_settings.debugMode) {
            jQuery.unblockUI();
        }
    }

    function addHandlers() {
        jQuery(document).off('click', 'a[data-action]');
        jQuery(document).on('click', 'a[data-action]', function (event) {
            var action = jQuery(this).data('action');

            switch (action) {
                case 'loadpage':
                    var page = jQuery(this).data('page');
                    pageLoader(page);
                    event.preventDefault();
                    break;
            }
        });
    }

    function getGameConfig(gameName) {
        for (var i = 0; i < _options.Plugins.length; i++) {
            if (_options.Plugins[i].GameShortName == gameName) {
                return _options.Plugins[i];
                break;
            }
        }
        return null;
    }

    function messageDataReader(command, data) {

        if (_worker !== null) {
            var message = {
                cmd: command,
                data: data
            };
            _worker.postMessage(message);
        }
    }

    function initDataReader() {
        messageDataReader('init');
    }

    function startDataReader(config) {
        messageDataReader('start', config);
    }

    function stopDataReader() {
        messageDataReader('stop', {});
    }

    function dataEvent(e) {

        var message = JSON.parse(e.data);

        switch (message.action) {
            case 'serverconnected':
                init().then(function () {
                    unblockUI();
                });
                break;

            case 'serverdisconnected':
                blockUI();
                break;

            case 'gameconnected':
                _currentGame = getGameConfig(message.data.Game);
                pageLoader(getBrowserHashPage());
                startDataReader();
                break;

            case 'gamedisconnected':
                _currentGame = null;
                stopDataReader();
                break;

            case 'data':
                switch (message.datatype) {
                    case 'heartbeat':
                        break;

                    case 'telemetry':
                    case 'timing':
                        if (_currentPage.update !== undefined) {
                            _currentPage.update(message.datatype, message.data);
                        }
                        break;
                }
                break;

            case 'error':
                switch (message.datatype) {
                    case 'heartbeat':
                        break;
                }

                break;
        }
    }

    function getNextId() {
        _id++;

        return 'ag_' + _id;
    }

    function getURI() {
        var uri = '';

        if (location.port !== '') {
            uri = location.protocol + '//' + location.host.replace(':' + location.port, '') + ':' + location.port;
        } else {
            uri = location.protocol + '//' + location.host + '/';
        }

        return uri;
    }

    return {

        run: function() {
            run();
        },

        options: function () {
            return _options;
        },

        getNextId: function () {
            return getNextId();
        },

        getCurrentPage: function () {
            return _currentPage;
        },

        getURI: function () {
            return getURI();
        },

        getSetting: function (setting) {
            return getSetting(setting);
        },

        setSetting: function (setting, value) {
            setSetting(setting, value);
        },

        setupBackgroundVideo: function () {
            setupBackgroundVideo();
        }
    }
}();
//# sourceURL=/js/framework/ui.js
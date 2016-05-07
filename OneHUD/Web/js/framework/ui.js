var OneHUDUI = function () {
    'use strict';

    var _options = null;
    var _currentPage = null;
    var _worker = null;
    var _currentGame = null;
    var _id = 0;

    function SetOffCanvasHeight() {
        var height = jQuery(window).height();
        var contentHeight = jQuery('.off-canvas-content').height();
        if (contentHeight > height) { height = contentHeight; }

        jQuery('.off-canvas-wrapper').height(height);
        jQuery('.off-canvas-wrapper-inner').height(height);
        jQuery('.off-canvas').height(height);
        jQuery('#content').height(height);
    }

    function init(options, worker) {
        _options = options;
        _worker = worker;

        _worker.addEventListener('message', function (e) {
            dataEvent(e);
        });

        jQuery(window)
            .load(function () {
                SetOffCanvasHeight();
            })
            .resize(function () {
                SetOffCanvasHeight();
            });

        jQuery(window).swipe({
            swipe: function (event, direction, distance, duration, fingerCount, fingerData) {
                if (direction === 'right') {
                    jQuery('#offCanvas').foundation('open');
                }
            }
        });

        setupMenu();
        addHandlers();
        pageLoader(getBrowserHashPage());
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
            page = _options.DefaultPage;
        }

        location.hash = page;

        clearContent();
        blockUI();
        _currentPage = OneHUDClassCache.getPage(page);
        var deferred = _currentPage.init();
        if (deferred === undefined) {
            updateHeader();
            setupBackgroundVideo();
            SetOffCanvasHeight();
            jQuery('#offCanvas').foundation('close');
            unblockUI();
        } else {
            deferred.done(function (result) {
                updateHeader();
                setupBackgroundVideo();
                SetOffCanvasHeight();
                jQuery('#offCanvas').foundation('close');
                unblockUI();
            });
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

        var pages = OneHUDClassCache.getPages();

        for (var page in pages) {
            if (pages.hasOwnProperty(page)) {
                if (pages[page].customMenu !== undefined) {
                    pages[page].customMenu('#page-menu');
                }
            }
        }

    }

    function setupBackgroundVideo() {
        var showVideo = false;

        if (_currentPage.showVideo !== undefined) {
            if (_currentPage.showVideo) {
                showVideo = true;
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
    }

    function blockUI() {
        jQuery.blockUI({ message: '<h1><img src="busy.gif" /> Just a moment...</h1>' });
    }

    function unblockUI() {
        jQuery.unblockUI();
    }

    function addHandlers() {
        jQuery(document).on('click','a[data-action]',function (event) {
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

    function startDataReader(config) {
        messageDataReader('start', config);
    }

    function stopDataReader() {
        messageDataReader('stop', {});
    }

    function dataEvent(e) {

        var message = JSON.parse(e.data);

        switch (message.action) {
            case 'data':
                switch (message.datatype) {
                    case 'heartbeat':
                        handleHeartBeat(message);
                        break;

                    case 'telemetry':
                        if (_currentPage.update !== undefined) {
                            _currentPage.update(message.datatype, message.data);
                        }
                        break;

                    case 'error':
                        break;
                }
                break;
        }
    }

    function handleHeartBeat(message) {
        if (message.data.Game !== null) {
            if (_currentGame === null || (_currentGame.GameShortName !== message.data.Game)) {
                _currentGame = getGameConfig(message.data.Game);
                pageLoader(getBrowserHashPage());
                startDataReader();

            }
        } else {
            if (_currentGame !== null) {
                _currentGame = null;
                stopDataReader();
                pageLoader(getBrowserHashPage());
            }
        }
    }

    function getNextId() {
        _id++;

        return 'ag_' + _id;
    }

    return {
        init: function (options, worker) {
            init(options, worker);
        },

        options: function () {
            return _options;
        },

        getNextId: function () {
            return getNextId();
        }
    }
}();
//# sourceURL=/js/framework/ui.js
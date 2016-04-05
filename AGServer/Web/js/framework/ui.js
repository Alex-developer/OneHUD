var AGServerUI = function () {
    'use strict';

    var _options = null;
    var _currentPage = null;

    function init(options) {
        _options = options;
        setupMenu();
        addHandlers();
        pageLoader(getBrowserHashPage());
    }

    function getBrowserHashPage() {
        var page = null;
        var hash = location.hash.substring(location.hash.indexOf('#') + 1);
        if (hash !== '') {
            if (AGServerClassCache.pageExists(hash)) {
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
        _currentPage = AGServerClassCache.getPage(page);
        setupBackgroundVideo();
        _currentPage.init();
        unblockUI();
        
    }

    function setupMenu() {
        jQuery('#page-menu').html('');
        jQuery.each(_options.Pages, function (index, page) {
            var menuLink = jQuery('<a>', { 'href': '#', 'data-action': 'loadpage', 'data-page': page.Name }).text(page.Name);
            var menuLi = jQuery('<li>', { class: 'spinner' }).append(menuLink);
            jQuery('#page-menu').append(menuLi);
        });
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


    return {
        init: function (options) {
            init(options);
        },

        options: function () {
            return _options;
        }
    }
}();
//# sourceURL=/js/framework/ui.js
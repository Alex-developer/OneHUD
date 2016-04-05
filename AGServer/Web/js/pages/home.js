var AGRacingViewHome = function () {
    'use strict'

    var _name = 'Home';
    var _icon = 'images/pages/home.png';
    var _description = 'The AgServer homepage. From here you can access all of the other available pages and function';
    var _showVideo = true;

    function init() {
        buildPage();
    }

    function buildPage() {
        var options = AGServerUI.options();
        var template = '\
        <div class="row" id="home-pages-row" data-equalizer data-equalize-on="medium">\
            <div class="home-pages">\
                {{#Pages}}\
                <div>\
                    <a href="#" data-action="loadpage" data-page="{{Name}}">\
                        <div class="callout {{Name}}-panel" data-equalizer-watch>\
                            <img src="{{Icon}}" class="float-center">\
                            <h2 class="text-center">{{Name}}</h2>\
                            <p>{{Description}}</p>\
                        </div>\
                    </a>\
                </div>\
                {{/Pages}}\
            </div>\
        </div>';

        var html = Mustache.to_html(template, options);
        jQuery('#content').html(html);

        jQuery('.home-pages').slick({
            infinite: true,
            accessibility: false,
            slidesToShow: 3,
            slidesToScroll: 3
        });
    }

    return {
        showVideo: _showVideo,

        init: function () {
            return init();
        }
    }
}
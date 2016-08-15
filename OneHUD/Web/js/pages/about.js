var OneHUDViewAbout = function () {
    'use strict'

    var _name = 'About';
    var _icon = 'images/pages/about.png';
    var _menuIcon = 'images/pages/about-menu.png';
    var _description = 'The AGServer help page. Got a problem? check this page for debug information and where to get help';
    var _showVideo = true;
    var _order = 5;

    function init() {
        buildPage();
    }

    function buildPage() {
        var options = OneHUDUI.options();
        var template = '\
        <div class="row" id="about-pages-row" data-equalizer data-equalize-on="medium">\
            <div class="row">\
                <div id="about-left" class="small-6 large-6 columns callout halfOpacity" data-equalizer-watch></div>\
                <div class="small-6 large-6 columns callout halfOpacity" data-equalizer-watch><h3>About OneHUD</h3>\
                    <p>Bacon ipsum dolor amet cupim kielbasa shoulder brisket pig strip steak bresaola rump. Short loin hamburger bresaola, pastrami pig tongue kevin doner tail. Beef ribs frankfurter ham pork belly corned beef tri-tip. Hamburger pastrami swine strip steak boudin frankfurter. Rump salami fatback tri-tip, swine tail ham bacon leberkas. Cupim chicken pancetta short loin, cow kielbasa sirloin alcatra meatloaf rump.</p>\
                    <p>T-bone meatloaf cow, ham frankfurter pancetta beef ribs tail landjaeger pork loin kevin short ribs. Pork loin ground round pork chop, spare ribs strip steak cow pork belly short ribs short loin cupim jerky kielbasa tri-tip brisket beef ribs. Shank short loin meatball pancetta, andouille pork belly pastrami. Corned beef turkey boudin pork belly porchetta pig, cow landjaeger fatback cupim. Prosciutto turkey ham hock venison. Pig short ribs venison chuck porchetta ribeye.</p>\
                    <p>Meatball venison ground round cupim ham strip steak ham hock ribeye pork chop shank shoulder picanha shankle kielbasa. Strip steak picanha pig kielbasa filet mignon tenderloin pastrami spare ribs t-bone alcatra turducken landjaeger porchetta. Sausage tongue pork loin spare ribs biltong swine chicken alcatra picanha. Hamburger ribeye turkey capicola biltong porchetta boudin turducken chuck picanha ground round. Shoulder pastrami capicola short ribs pancetta pig boudin chicken picanha cow filet mignon shankle.</p>\
                </div>\
            </div>\
        </div>';

        var html = Mustache.to_html(template);
        jQuery('#content').html(html);

        var pluginHTML = '<h3>Available Plugins</h3><div>';
        pluginHTML += '<div class="callout nintyOpacity success"><h5>OneHUD Server</h5><p>Version: ' + options.Version + ' <span> Author: Alex Greenland</span></p></div>';
        var linkHTML = '';
        for (var i = 0; i < options.Plugins.length; i++) {
            linkHTML = '';
            if (options.Plugins[i].URL !== null) {
                linkHTML = '<a target="_blank" href="' + options.Plugins[i].URL + '">&nbsp</a>'
            }
            pluginHTML += '<div class="callout nintyOpacity"><h5>' + options.Plugins[i].GameLongName + ' ' + linkHTML + '</h5><p>Version: ' + options.Plugins[i].PluginVersion + ' <span> Author: ' + options.Plugins[i].Author + '</span></p></div>';
        }
        pluginHTML += '</div>';
        jQuery('#about-left').html(pluginHTML);
    }

    return {
        showVideo: _showVideo,

        init: function () {
            return init();
        }
    }
}
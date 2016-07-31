String.prototype.toHHMMSS = function (displayFraction) {
    var fraction = '';
    if (displayFraction === undefined) {
        displayFraction = false;
    }
    var sec_num = parseInt(this, 10);

    if (displayFraction) {
        var fractionPart = this - sec_num;
        fraction = '.' + Math.ceil(((fractionPart < 1.0) ? fractionPart : (fractionPart % Math.floor(fractionPart))) * 1000)
    }
    var hours = Math.floor(sec_num / 3600);
    var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
    var seconds = sec_num - (hours * 3600) - (minutes * 60);

    if (hours < 10) { hours = "0" + hours; }
    if (minutes < 10) { minutes = "0" + minutes; }
    if (seconds < 10) { seconds = "0" + seconds; }

    var time = hours + ':' + minutes + ':' + seconds + fraction;
    return time;
}

String.prototype.toMMSS = function (displayFraction) {
    var fraction = '';
    if (displayFraction === undefined) {
        displayFraction = false;
    }
    var sec_num = parseInt(this, 10);

    if (displayFraction) {
        var fractionPart = this - sec_num;
        fraction = '.' + Math.ceil(((fractionPart < 1.0) ? fractionPart : (fractionPart % Math.floor(fractionPart))) * 1000)
    }
    var hours = Math.floor(sec_num / 3600);
    var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
    var seconds = sec_num - (hours * 3600) - (minutes * 60);

    if (hours < 10) { hours = "0" + hours; }
    if (minutes < 10) { minutes = "0" + minutes; }
    if (seconds < 10) { seconds = "0" + seconds; }

    var time = minutes + ':' + seconds + fraction;
    return time;
}

Number.prototype.toHHMMSS = function (displayFraction) {
    var fraction = '';
    if (displayFraction === undefined) {
        displayFraction = false;
    }
    var sec_num = parseInt(this, 10);

    if (displayFraction) {
        var fractionPart = this - sec_num;
        fraction = '.' + Math.ceil(((fractionPart < 1.0) ? fractionPart : (fractionPart % Math.floor(fractionPart))) * 1000)
    }
    var hours = Math.floor(sec_num / 3600);
    var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
    var seconds = sec_num - (hours * 3600) - (minutes * 60);

    if (hours < 10) { hours = "0" + hours; }
    if (minutes < 10) { minutes = "0" + minutes; }
    if (seconds < 10) { seconds = "0" + seconds; }

    var time = hours + ':' + minutes + ':' + seconds + fraction;
    return time;
}

Number.prototype.toMMSS = function (displayFraction) {
    var time;
    if (this >= 0) {
        var fraction = '';
        if (displayFraction === undefined) {
            displayFraction = false;
        }
        var sec_num = parseInt(this, 10);

        if (displayFraction) {
            var fractionPart = this - sec_num;
            fraction = '.' + Math.ceil(((fractionPart < 1.0) ? fractionPart : (fractionPart % Math.floor(fractionPart))) * 1000)
        }
        var hours = Math.floor(sec_num / 3600);
        var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
        var seconds = sec_num - (hours * 3600) - (minutes * 60);

        if (hours < 10) { hours = "0" + hours; }
        if (minutes < 10) { minutes = "0" + minutes; }
        if (seconds < 10) { seconds = "0" + seconds; }

        time = minutes + ':' + seconds + fraction;
    } else {
        time = "--:--"
    }
    return time;
}

var moment = require('moment');

Date.prototype.yyyymmdd = function() {
    var yyyy = this.getFullYear();
    var mm = this.getMonth() < 9 ? "0" + (this.getMonth() + 1) : (this.getMonth() + 1); // getMonth() is zero-based
    var dd  = this.getDate() < 10 ? "0" + this.getDate() : this.getDate();
    return "".concat(yyyy).concat(mm).concat(dd);
};

Date.prototype.yyyymmddHHMMSS = function() {
    var yyyy = this.getFullYear();
    var mm = this.getMonth() < 9 ? "0" + (this.getMonth() + 1) : (this.getMonth() + 1); // getMonth() is zero-based
    var dd  = this.getDate() < 10 ? "0" + this.getDate() : this.getDate();
    var HH = this.getHours() < 9 ? "0" + this.getHours() : this.getHours();
    var MM = this.getMinutes() < 9 ? "0" + this.getMinutes() : this.getMinutes();
    var SS = this.getSeconds() < 9 ? "0" + this.getSeconds() : this.getSeconds();
    return "".concat(yyyy).concat(mm).concat(dd).concat(HH).concat(MM).concat(SS);
};

String.prototype.padLeft = function (length, character) {
    return new Array(length - this.length + 1).join(character || '0') + this;
}

Number.prototype.toRealFixed = function(digits) {
    return Math.floor(this.valueOf() * Math.pow(10, digits)) / Math.pow(10, digits);
};

function GetNowTimeStr() {
    var nowMoment = moment();
    var nowTimeStr = nowMoment.format("YYYY-MM-DD HH:mm:ss");

    return nowTimeStr;
}

function GetNowTimeStrForLog() {
    var nowMoment = moment();
    var nowTimeStr = nowMoment.format("YYYY-MM-DD HH:mm:ss");

    return "[" + nowTimeStr + ":" + nowMoment.millisecond().toString().padLeft(3, '0') + "] ";
}

// Exports
module.exports = {
    GetNowTimeStr,
    GetNowTimeStrForLog,
}

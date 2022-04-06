String.prototype.padLeft = function (l, c) {
    return Array(l - this.length + 1).join(c || " ") + this
}

Date.prototype.DMY = function () {
    return this.getDate().toString().padLeft(2, '0') + "/" + (this.getMonth() + 1).toString().padLeft(2, '0') + "/" + this.getFullYear();
}

Date.prototype.MY = function () {
    return (this.getMonth() + 1).toString().padLeft(2, '0') + "/" + this.getFullYear();
}

Date.CriaDMY = function (value) {
    var parts = value.split('/');
    if (parts.length == 3) {
        var year = parseInt(parts[2]);
        var month = parseInt(parts[1] ? parts[1] - 1 : 0);
        var day = parseInt(parts[0]);

        return new Date(year, month, day);
    }
}

Date.isLeapYear = function (year) {
    return (((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0));
};

Date.getDaysInMonth = function (year, month) {
    return [31, (Date.isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
};

Date.prototype.isLeapYear = function () {
    return Date.isLeapYear(this.getFullYear());
};

Date.prototype.getDaysInMonth = function () {
    return Date.getDaysInMonth(this.getFullYear(), this.getMonth());
};

Date.prototype.addMonths = function (value) {
    var n = this.getDate();
    this.setDate(1);
    this.setMonth(this.getMonth() + value);
    this.setDate(Math.min(n, this.getDaysInMonth()));
    return this;
};
Date.prototype.addMonthsRef = function (value) {
    var n = this.getDate();

    var newDate = new Date(this.getFullYear(), this.getMonth() + value, 1);
    var date = Math.min(n, newDate.getDaysInMonth());
    newDate.setDate(date);
    return newDate;
};

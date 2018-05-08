window.onerror = function (errorMsg, url, lineNumber, column, errorObj) {

    var callback = function (stackframes) {
        var stringifiedStack = stackframes.map(function (sf) {
            return sf.toString();
        }).join('\n');
        console.error(stringifiedStack);
        JL('serverLog').fatalException({
            msg: 'Exception! ' + stringifiedStack,
            errorMsg: errorMsg,
            url: url,
            lineNumber: lineNumber,
            column: column
        }, errorObj);
    };
    var errback = function (err) { console.log(err.message); };

    StackTrace.fromError(errorObj).then(callback).catch(errback);

    // Tell browser to run its own error handler as well
    return false;
};
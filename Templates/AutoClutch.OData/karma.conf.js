// Karma configuration
// Generated on Thu Jul 20 2017 14:07:13 GMT-0400 (Eastern Daylight Time)

module.exports = function (config) {
    config.set({

        // base path that will be used to resolve all patterns (eg. files, exclude)
        basePath: '',


        // frameworks to use
        // available frameworks: https://npmjs.org/browse/keyword/karma-adapter
        frameworks: ['jasmine'],


        // list of files / patterns to load in the browser
        files: [
            //'wwwroot/**/*.js',
              "wwwroot/lib/angular/angular.min.js"
            , "wwwroot/lib/angular-route/angular-route.min.js"
            , "wwwroot/lib/angular-animate/angular-animate.min.js"
            , "wwwroot/lib/angular-sanitize/angular-sanitize.min.js"
            , './shared-directives/*.js',
            , './shared-directives/**/*.js',
            , 'wwwroot/lib/nya-bootstrap-select/dist/js/nya-bs-select.js'
            , './app/*.js',
            //'./app/**/*.js'
        ],

        //files: [
        //  'wwwwroot/lib/**/*.js',
        //  './wwwwroot/lib/**/*.js',
        //  './app/**/*.js',
        //  'wwwroot/**/*.js',
        //  '/app/**/*.spec.js',
        //  '/app/**/*.controller.spec.js',
        //  './app/**/*.controller.spec.js',
        //  './app/**/*.spec.js',
        //  './app/**/*.js',
        //  './wwwroot/**/*.js',
        //  '/app/reporting-categories/*.js',
        //  '/app/reporting-categories/*.spec.js',
        //  './app/reporting-categories/*.js',
        //  './app/reporting-categories/*.spec.js'
        //],


        // list of files to exclude
        exclude: [
        ],


        // preprocess matching files before serving them to the browser
        // available preprocessors: https://npmjs.org/browse/keyword/karma-preprocessor
        preprocessors: {
        },


        // test results reporter to use
        // possible values: 'dots', 'progress'
        // available reporters: https://npmjs.org/browse/keyword/karma-reporter
        reporters: config.angularCli && config.angularCli.codeCoverage
            ? ['progress', 'coverage-istanbul', 'junit']
            : ['progress', 'kjhtml', 'junit', 'htmlDetailed'],


        // notify karma of the available plugins
        plugins: [
          'karma-jasmine',
          'karma-phantomjs-launcher',
          'karma-html-detailed-reporter',
          require('karma-phantomjs-launcher'),
          require('karma-junit-reporter')
        ],

        junitReporter: {
            outputDir: '', // Results will be saved as $outputDir/$browserName.xml.
            outputFile: 'test.xml', // If included, results will be saved as $outputDir/$browserName/$outputFile.
        },

        // configure the HTML-Detailed-Reporter to put all results in one file    
        htmlDetailed: {
            splitResults: false
        },


        // web server port
        port: 9876,


        // enable / disable colors in the output (reporters and logs)
        colors: true,


        // level of logging
        // possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
        logLevel: config.LOG_INFO,


        // enable / disable watching file and executing tests whenever any file changes
        autoWatch: true,


        // start these browsers
        // available browser launchers: https://npmjs.org/browse/keyword/karma-launcher
        browsers: ['PhantomJS'],


        // Continuous Integration mode
        // if true, Karma captures browsers, runs the tests and exits
        singleRun: false,

        // Concurrency level
        // how many browser should be started simultaneous
        concurrency: Infinity
    })
}

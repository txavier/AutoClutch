(function () {
    'use strict';

    angular
        .module('app')
        .config(config);

    config.$inject = ['$routeProvider', '$locationProvider', '$httpProvider'];

    function config($routeProvider, $locationProvider, $httpProvider) {
        // Configure the security token bearer interceptor.
        $httpProvider.interceptors.push('authInterceptorService');

        $routeProvider
            .when('/update-author/:authorId', {
                templateUrl: 'app/author/update-author.html',
                controller: 'UpdateAuthorController',
                controllerAs: 'vm'
            })
            .when('/add-author', {
                templateUrl: 'app/author/add-author.html',
                controller: 'AddAuthorController',
                controllerAs: 'vm'
            })
            .when('/view-author/:authorId', {
                templateUrl: 'app/author/view-author.html',
                controller: 'ViewAuthorController',
                controllerAs: 'vm'
            })
            .when('/authors', {
                templateUrl: 'app/author/authors.html',
                controller: 'AuthorsController',
                controllerAs: 'vm'
            })
            .when('/dashboard', {
                templateUrl: 'app/user/dashboard.html',
                controller: 'DashboardController',
                controllerAs: 'vm'
            })
            .when('/update-blog-entry/:blogEntryId', {
                templateUrl: 'app/blog-entry/update-blog-entry.html',
                controller: 'UpdateBlogEntryController',
                controllerAs: 'vm'
            })
            .when('/add-blog-entry', {
                templateUrl: 'app/blog-entry/add-blog-entry.html',
                controller: 'AddBlogEntryController',
                controllerAs: 'vm'
            })
            .when('/view-blog-entry/:blogEntryId', {
                templateUrl: 'app/blog-entry/view-blog-entry.html',
                controller: 'ViewBlogEntryController',
                controllerAs: 'vm'
            })
            .when('/blog-entries', {
                templateUrl: 'app/blog-entry/blog-entries.html',
                controller: 'BlogEntriesController',
                controllerAs: 'vm'
            })
            .when('/home', {
                templateUrl: 'app/home/home.html',
                controller: 'HomeController',
                controllerAs: 'vm'
            })
            .otherwise({ redirectTo: 'home' });

    }

})();
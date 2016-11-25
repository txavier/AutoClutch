(function () {
    'use strict';

    angular
        .module('app', [
            'ngRoute',
            'toaster',
            'ngAnimate',
            'ngSessionStorage',
            'textAngular',
            'ng-file-model'
        ]);

})();

(function () {
    'use strict';

    //http://stackoverflow.com/questions/17063000/ng-model-for-input-type-file
    angular
        .module('app')
        .directive("fileread", [function () {
            return {
                scope: {
                    fileread: "="
                },
                link: function (scope, element, attributes) {
                    element.bind("change", function (changeEvent) {
                        var reader = new FileReader();
                        reader.onload = function (loadEvent) {
                            scope.$apply(function () {
                                scope.fileread = loadEvent.target.result;
                            });
                        }
                        reader.readAsDataURL(changeEvent.target.files[0]);
                    });
                }
            }
        }]);
})();

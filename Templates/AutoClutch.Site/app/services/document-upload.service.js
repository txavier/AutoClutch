(function () {
    'use strict';

    angular
        .module('app')
        .factory('documentUploadService', documentUploadService);

    documentUploadService.$inject = ['$log', '$timeout', 'toaster', 'Upload'];

    function documentUploadService($log, $timeout, toaster, Upload) {
        var loggedInUser = null;

        var service = {
            upload: upload,
            getFileInfoMessage: getFileInfoMessage
        };

        return service;

        function getFileInfoMessage(filename, file, fileId, fileInfo, tempDropFile) {
            var result = ((file || fileId) && (fileInfo.progressPercentage == 0 || fileInfo.progressPercentage == 100)) ?
                                        'Drop pdf here or click to replace current file' :
                                        (tempDropFile ?
                                            ((fileInfo.progressPercentage != 0 && fileInfo.progressPercentage != 100) ? 'Uploading... ' + fileInfo.progressPercentage + '%' : 'The file ' + (filename || '') + ' is loaded') :
                                            'Drop pdf here or click to upload new file');

            return result;
        }

        function upload(files, fileInfo) {
        	if (files && files.length) {
                for (var i = 0; i < files.length; i++) {
                    var file = files[i];

                    if (!file.$error) {
                        var upload = Upload.upload({
                            url: 'api/files',
                            data: {
                                file: file
                            }
                        });
                        
                        return upload.then(function (response) {
                            return $timeout(function () {
                                // If the response is not a number then
                                // that means its the entire body.
                                if (isNaN(response.data)) {
                                    fileInfo.filename = response.config.data.file[0].name;

                                    fileInfo.file = response.data;

                                    fileInfo.fileType = response.config.data.file[0].type;
                                }
                                // ...else this is a number, which means
                                // it is the file id of the file.
                                else {
                                    fileInfo.filename = response.config.data.file[0].name;

                                    fileInfo.fileId = response.data;
                                }

                                return fileInfo;
                            });
                        }, function(response) {
                            if(response.status > 0) {
                                var errorMsg = response.status + ': ' + response.data;
                            }
                        }, function (evt) {
                            fileInfo.progressPercentage = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
                        });
                    }
                }
        	}

        }

    }
})();
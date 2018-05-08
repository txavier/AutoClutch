(function () {
    'use strict';

    angular
        .module('app')
        .directive('depUpdateReceivingReport', depUpdateReceivingReport);

    function depUpdateReceivingReport() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'app/payments/update-receiving-report.html',
            require: 'ngModel',
            scope: {
                model: '=ngModel',
                ngChange: '&'
            },
            link: link,
            controller: UpdateReceivingReportController,
            controllerAs: 'vm',
            bindToController: true
        };

        return directive;

        function link(scope, element, attrs, vm) {
        }
    }

    UpdateReceivingReportController.$inject = ['$scope', '$location', '$routeParams', '$log', 'dataService', 'toaster', 'documentUploadService', 'authenticationService'];

    function UpdateReceivingReportController($scope, $location, $routeParams, $log, dataService, toaster, documentUploadService, authenticationService) {
        var vm = this;

        vm.payment = vm.model;
        vm.reportingCategories = [];
        vm.addReceivingReportDetail = addReceivingReportDetail;
        vm.receivingReportDetails = [];
        vm.refreshReceivingReportDetails = refreshReceivingReportDetails;
        vm.model.receivingReportDetails = [];
        vm.updatePayment = updatePayment;
        vm.removeReceivingReportDetail = removeReceivingReportDetail;
        vm.fileInfo = {};
        vm.deletePaymentFile = deletePaymentFile;
        vm.getFileInfoMessage = getFileInfoMessage;
        vm.sumPurchasingDataUnitPrice = sumPurchasingDataUnitPrice;
        vm.attachments = [];
        vm.paymentAttachments = [];
        vm.viewPaymentPdf = viewPaymentPdf;
        vm.receivingReportUnlock = receivingReportUnlock;
        vm.loggedInUser = {};

        activate();

        function activate() {
            var receivingReportDetailsSearchCriteria = {
                q: 'paymentId = ' + vm.payment.paymentId
            }

            getReceivingReportDetails(receivingReportDetailsSearchCriteria);

            var reportingCategoriesSearchCriteria = {
                currentPage: 1,
                itemsPerPage: 9999,
                orderBy: 'reportingCategoryDescription',
                searchText: null,
                includeProperties: null,
                q: null
            }

            getReportingCategories(reportingCategoriesSearchCriteria);

            getLoggedInUser().then(setView);
        }

        function getLoggedInUser() {
            return authenticationService.getLoggedInUser().then(function (data) {
                vm.loggedInUser = data;

                return vm.loggedInUser;
            });
        }

        function setView() {
            // If this report is locked and the logged in person is not in the procurement
            // section then disable all controls to not allow edit.
            if (vm.payment.receivingReportLocked && vm.loggedInUser.sectionId !== 5 && vm.loggedInUser.sectionId !== null) {
                vm.disableAll = true;
            }
        }

        // Unlock the receiving report so that edits can be made to it again.
        function receivingReportUnlock(payment) {
            payment.receivingReportLocked = vm.payment.receivingReportLocked = false;

            updatePayment(payment, true);
        }

        function sumPurchasingDataUnitPrice(receivingReportDetails) {
            var sum = 0;

            for (var i = 0; i < receivingReportDetails.length; i++) {
                sum += (receivingReportDetails[i].purchasingDataQuantity || 0) * (receivingReportDetails[i].purchasingDataUnitPrice || 0);
            }

            return sum;
        }

        function getReceivingReportDetails(searchCriteria) {
            return dataService.searchEntitiesOData('receivingReportDetails', searchCriteria).then(function (data) {
                vm.payment.receivingReportDetails = data;

                vm.receivingReportDetails = vm.payment.receivingReportDetails;
            });
        }

        function getReportingCategories(searchCriteria) {
            return dataService.searchEntitiesOData('reportingCategories', searchCriteria).then(function (data) {
                vm.reportingCategories = data;

                return vm.reportingCategories;
            });
        }

        function addReceivingReportDetail() {
            var itemNumber = vm.receivingReportDetails[vm.receivingReportDetails.length - 1]['itemNumber'];

            itemNumber++;

            vm.receivingReportDetails.push({ itemNumber: itemNumber, paymentId: -1 });
        }

        function removeReceivingReportDetail(itemNumber) {
            var tempArray = [];

            for (var i = 0; i < vm.receivingReportDetails; i++) {
                if (vm.receivingReportDetails[i]['itemNumber'] !== itemNumber) {
                    vm.receivingReportDetails[i].itemNumber = i + 1;

                    tempArray.push(vm.receivingReportDetails[i]);
                }
            }

            refreshReceivingReportDetails();
        }

        function refreshReceivingReportDetails() {
            vm.payment.receivingReportDetails = vm.receivingReportDetails;
        }

        // Begin section: payment file.

        function deletePaymentFile(paymentId) {
            return dataService.deletePaymentFile(paymentId).then(function (data) {
                activate();
            });
        }

        /**
         * https://github.com/danialfarid/ng-file-upload
         * https://github.com/stewartm83/angular-fileupload-sample
        */
        function upload(files) {
            if (files.length === 1) {
                return documentUploadService.upload(files, vm.fileInfo).then(function (data) {
                    vm.payment.filename = data.filename;

                    vm.payment.file = data.file;

                    vm.payment.fileType = data.fileType;

                    vm.payment.paymentPDFFileId = data.fileId;
                });
            }
            else {
                return documentUploadService.getBase64DataUrl(files).then(function (urls) {
                    for (var i = 0; i < urls.length; i++) {
                        vm.paymentAttachments.push({ paymentId: vm.payment.paymentId, base64DataUrl: urls[i], fileName: files[i].name });
                    }
                });
            }
        }

        function getFileInfoMessage(filename, file, fileId, fileInfo, tempDropFile) {
            var result = documentUploadService.getFileInfoMessage(filename, file, fileId, fileInfo, tempDropFile);

            return result;
        }

        $scope.$watch('vm.payment.tempDropFile', function () {
            if (vm.payment.tempDropFile != null && vm.payment.tempDropFile != undefined && vm.payment.tempDropFile.length === 1 && vm.payment.tempDropFile[0].name !== null) {
                var files = [vm.payment.tempDropFile];

                upload(files).then(function (data) {
                    // Autosave after the file has been dropped.
                    updatePayment(vm.payment);
                });
            }
        });

        $scope.$watch('vm.attachments', function () {
            if (vm.attachments != null && vm.attachments.length > 0 && vm.attachments[0].name !== null) {
                var files = vm.attachments;

                upload(files).then(function (data) {
                    // Autosave after the file has been dropped.
                    //updateEntity('payments', vm.payment.paymentId, vm.payment, true);
                });
            }
        });

        // End section: payment file.

        function viewPaymentPdf(payment) {
            $location.path('/' + $routeParams.sectionName + '/contracts/' + $routeParams.contractNumber + '/payments/' + payment.paymentId + '/payment-documents');
        }

        function updatePayment(payment, unlock) {
            refreshReceivingReportDetails();

            if (!vm.myForm.$valid) {
                var errors = [];

                for (var key in vm.myForm.$error) {
                    for (var index = 0; index < vm.myForm.$error[key].length; index++) {
                        errors.push(vm.myForm.$error[key][index].$name + ' is required.');
                    }
                }

                toaster.pop('warning', 'Information Missing', 'The ' + errors[0]);

                return;
            }

            if ((vm.payment.paymentPDFFileId || vm.payment.file) && !unlock) {
                vm.payment.receivingReportLocked = true;
            }

            var tempContract = payment.contract;

            payment.contract = null;

            var tempReportingCategory = payment.reportingCategory;

            payment.reportingCategory = null;

            // Dont try to save the payment with the receiving report details at the same time.
            // save them separately.
            var receivingReportDetails = payment.receivingReportDetails;

            payment.receivingReportDetails = undefined;

            payment.tempDropFile = undefined;

            // Save the payment then save all of the receiving reports inside.
            return dataService.updateEntityOData('payments', payment.paymentId, payment, true).then(function (data) {
                payment.paymentId = data.paymentId;

                if (payment.paymentId > 0) {
                    // For every receiving report save it to the database.
                    for (var i = 0; i < receivingReportDetails.length; i++) {
                        receivingReportDetails[i].paymentId = payment.paymentId;

                        dataService.updateEntityOData( 'receivingReportDetails', receivingReportDetails[i].receivingReportDetailId, receivingReportDetails[i]);
                    }

                    // Save the payment attachments.
                    for (var i = 0; i < vm.paymentAttachments.length; i++) {
                        if (!vm.paymentAttachments[i].paymentAttachmentId || !vm.paymentAttachments[i].paymentAttachmentId === 0) {
                            dataService.addEntityOData('paymentAttachments', vm.paymentAttachments[i]);
                        }
                    }
                }
            }, function (error) {
                $log.error('An error occurred.');

                // On an error, get the temporary values back in order to set up 
                // the view so that the user can make corrections as needed to 
                // correct the values for a subsequent submittal of data.
                payment.contract = tempContract;

                payment.reportingCategory = tempReportingCategory;
            });
        }

    }
})();

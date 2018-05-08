(function () {
    'use strict';

    angular
        .module('app')
        .directive('depAddReceivingReport', depAddReceivingReport);

    function depAddReceivingReport() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'app/payments/add-receiving-report.html',
            require: 'ngModel',
            scope: {
                model: '=ngModel',
                ngChange: '&'
            },
            link: link,
            controller: AddReceivingReportController,
            controllerAs: 'vm',
            bindToController: true
        };

        return directive;

        function link(scope, element, attrs, vm) {
        }
    }

    AddReceivingReportController.$inject = ['$scope', '$location', '$routeParams', '$log', 'dataService', 'toaster', 'paymentService', 'sharedService'];

    function AddReceivingReportController($scope, $location, $routeParams, $log, dataService, toaster, paymentService, sharedService) {
        var vm = this;

        vm.payment = vm.model;
        vm.payment.reportingCategoryId = -1;
        vm.reportingCategories = [];
        vm.addReceivingReportDetail = addReceivingReportDetail;
        vm.receivingReportDetails = [];
        vm.refreshReceivingReportDetails = refreshReceivingReportDetails;
        vm.payment.receivingReportDetails = [];
        vm.addPayment = addPayment;
        vm.removeReceivingReportDetail = removeReceivingReportDetail;
        vm.sumPurchasingDataUnitPrice = sumPurchasingDataUnitPrice;
        vm.setObject = setObject;
        vm.viewPaymentPdf = viewPaymentPdf;

        activate();

        function activate() {
            var tempContract = vm.payment.contract;

            paymentService.initializeNewPayment(vm.payment.contract.contractId).then(function (data) {
                vm.payment = data;

                vm.payment.contract = tempContract;

                vm.payment.reportingCategoryId = -1;
            });

            vm.receivingReportDetails = [{ itemNumber: 1, paymentId: -1 }];

            var reportingCategoriesSearchCriteria = {
                currentPage: 1,
                itemsPerPage: 9999,
                orderBy: 'reportingCategoryDescription',
                searchText: null,
                includeProperties: null,
                q: null
            }

            getReportingCategories(reportingCategoriesSearchCriteria);
        }

        function setObject(model, propertyName, propertyNameId, entityArray, id) {
            var result = sharedService.setObject(model, propertyName, propertyNameId, entityArray, id);

            return result;
        }

        function sumPurchasingDataUnitPrice(receivingReportDetails) {
            var sum = 0;

            for (var i = 0; i < receivingReportDetails.length; i++) {
                sum += (receivingReportDetails[i].purchasingDataQuantity || 0) * (receivingReportDetails[i].purchasingDataUnitPrice || 0);
            }

            return sum;
        }

        function getReportingCategories(searchCriteria) {
            return dataService.searchEntitiesOData('reportingCategories', searchCriteria).then(function (data) {
                vm.reportingCategories.push({ reportingCategoryId: -1, reportingCategoryDescription: 'Not Set' });

                vm.reportingCategories = vm.reportingCategories.concat(data);

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

        function viewPaymentPdf(payment) {
            $location.path('/' + $routeParams.sectionName + '/contracts/' + $routeParams.contractNumber + '/payments/' + payment.paymentId + '/payment-documents');
        }

        function addPayment(payment) {
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

            var tempContract = payment.contract;

            payment.contract = null;

            var tempReportingCategory = payment.reportingCategory;

            payment.reportingCategory = null;

            // Dont try to save the payment with the receiving report details at the same time.
            // save them separately.
            var receivingReportDetails = payment.receivingReportDetails;

            // These fields were removed to appease odata.
            payment.receivingReportDetails = undefined;

            payment.deductions = undefined;

            payment.workOrders = undefined;

            payment.paymentAttachments = undefined;

            // Save the payment then save all of the receiving reports inside.
            return dataService.addEntityOData('payments', payment, true).then(function (data) {
                payment = data;

                if (payment.paymentId > 0) {
                    // For every receiving report save it to the database.
                    for (var i = 0; i < receivingReportDetails.length; i++) {
                        vm.payment.paymentId = payment.paymentId;

                        receivingReportDetails[i].paymentId = payment.paymentId;

                        dataService.addEntityOData('receivingReportDetails', receivingReportDetails[i]);
                    }

                    $location.path('/' + $routeParams.sectionName + '/contracts/' + $routeParams.contractNumber + '/payments/' + payment.paymentId);
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

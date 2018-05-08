(function () {
    'use strict';

    angular
    .module('app')
    .config(config);

    config.$inject = ['$routeProvider', '$locationProvider', 'cfpLoadingBarProvider'];

    function config($routeProvider, $locationProvider, cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = false;
        $locationProvider.hashPrefix('');

        $routeProvider
            .when('/change-orders', {
                templateUrl: 'app/change-orders/change-orders.html',
                controller: 'ChangeOrdersController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber/change-orders', {
                templateUrl: 'app/change-orders/change-orders.html',
                controller: 'ChangeOrdersController',
                controllerAs: 'vm'
            })
            .when('/about', {
                templateUrl: 'app/about/about.html',
                controller: 'aboutController',
                controllerAs: 'vm'
            })
            .when('/search', {
                templateUrl: 'app/search/search.html',
                controller: 'searchController',
                controllerAs: 'vm'
            })
            .when('/search/:searchText', {
                templateUrl: 'app/search/search.html',
                controller: 'searchController',
                controllerAs: 'vm'
            })
            .when('/search/:entityDataStore/:searchText', {
                templateUrl: 'app/search/search.html',
                controller: 'searchController',
                controllerAs: 'vm'
            })
            .when('/search/:entityDataStore/:searchText/:q', {
                templateUrl: 'app/search/search.html',
                controller: 'searchController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber/work-orders/:workOrderNumber/work-order-history-pdf/:workOrderHistoryId', {
                templateUrl: 'app/work-order-histories/work-order-history-pdf.html',
                controller: 'WorkOrderHistoryPdfController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber/change-orders/:changeOrderId/change-order-pdf', {
                templateUrl: 'app/change-orders/change-order-pdf.html',
                controller: 'ChangeOrderPdfController',
                controllerAs: 'vm'
            })
            .when('/advanced-settings', {
                templateUrl: 'app/settings/settings.html',
                controller: 'settingsController',
                controllerAs: 'vm'
            })
            .when('/add-or-update-setting',
            {
                templateUrl: 'app/settings/add-or-update-setting.html',
                controller: 'addOrUpdateSettingController',
                controllerAs: 'vm'
            })
            .when('/add-or-update-setting/:id',
            {
                templateUrl: 'app/settings/add-or-update-setting.html',
                controller: 'addOrUpdateSettingController',
                controllerAs: 'vm'
            })
            .when('/user-action-logs', {
                templateUrl: 'app/user-action-logs/user-action-logs.html',
                controller: 'UserActionLogsController',
                controllerAs: 'vm'
            })
            .when('/contractors/:contractorName/add-contractor-contact-person', {
                templateUrl: 'app/contractor-contact-persons/add-contractor-contact-person.html',
                controller: 'AddContractorContactPersonController',
                controllerAs: 'vm'
            })
            .when('/contractors/:contractorName/update-contractor-contact-person/:contractorContactPersonId', {
                templateUrl: 'app/contractor-contact-persons/update-contractor-contact-person.html',
                controller: 'UpdateContractorContactPersonController',
                controllerAs: 'vm'
            })
            .when('/contractors/:contractorName/contractor-contact-persons', {
                templateUrl: 'app/contractor-contact-persons/contractor-contact-persons.html',
                controller: 'ContractorContactPersonsController',
                controllerAs: 'vm'
            })
            .when('/add-contractor', {
                templateUrl: 'app/contractors/add-contractor.html',
                controller: 'AddContractorController',
                controllerAs: 'vm'
            })
            .when('/update-contractor/:contractorName', {
                templateUrl: 'app/contractors/update-contractor.html',
                controller: 'UpdateContractorController',
                controllerAs: 'vm'
            })
            .when('/contractors', {
                templateUrl: 'app/contractors/contractors.html',
                controller: 'ContractorsController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contract-report', {
                templateUrl: 'app/contracts/contract-report.html',
                controller: 'contractReportController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber/kpi-report', {
                templateUrl: 'app/kpi/kpi-report.html',
                controller: 'kpiReportController',
                controllerAs: 'vm'
            })
            .when('/history/:typeFullName/:id', {
                templateUrl: 'app/histories/history.html',
                controller: 'historyController',
                controllerAs: 'vm'
            })
            .when('/histories', {
                templateUrl: 'app/histories/histories.html',
                controller: 'historiesController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/add-contract', {
                templateUrl: 'app/contracts/add-contract.html',
                controller: 'AddContractController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber/add-renewal-contract', {
                templateUrl: 'app/contracts/add-renewal-contract.html',
                controller: 'AddRenewalContractController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber', {
                templateUrl: 'app/contracts/contract.html',
                controller: 'ContractController',
                controllerAs: 'vm'
                //access: {
                //    loginRequired: true,
                //    requiredPermissions: ['admin', 'sameSection', 'owner'],
                //    permissionCheckType: 'AtLeastOne'
                //}
            })
            .when('/:sectionName/contracts/:contractNumber/work-orders/:workOrderNumber', {
                templateUrl: 'app/work-orders/update-work-order.html',
                controller: 'UpdateWorkOrderController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber/payments/:paymentId/payment-pdf', {
                templateUrl: 'app/payments/payment-pdf.html',
                controller: 'PaymentPdfController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber/evaluations/:evaluationId', {
                templateUrl: 'app/contracts/contract.html',
                controller: 'ContractController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber/add-evaluation', {
                templateUrl: 'app/evaluations/add-evaluation.html',
                controller: 'AddEvaluationController',
                controllerAs: 'vm'
            })
            .when('/contracts', {
                templateUrl: 'app/contracts/contracts.html',
                controller: 'ContractsController',
                controllerAs: 'vm'
            })
            .when('/contracts/:customQueryName', {
                templateUrl: 'app/contracts/contracts.html',
                controller: 'ContractsController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts', {
                templateUrl: 'app/contracts/contracts.html',
                controller: 'ContractsController',
                controllerAs: 'vm',
                reloadOnSearch: false
            })
            .when('/:sectionName/contracts/:contractNumber/add-change-order', {
                templateUrl: 'app/change-orders/add-change-order.html',
                controller: 'AddChangeOrderController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber/change-orders/:changeOrderId', {
                templateUrl: 'app/change-orders/update-change-order.html',
                controller: 'UpdateChangeOrderController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber/add-deduction', {
                templateUrl: 'app/deductions/add-deduction.html',
                controller: 'AddDeductionController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber/deductions/:deductionId', {
                templateUrl: 'app/deductions/deduction.html',
                controller: 'DeductionController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber/payments/:paymentId/payment-documents', {
                templateUrl: 'app/payments/payment-documents.html',
                controller: 'PaymentDocumentsController',
                controllerAs: 'vm'
            })
            .when('/contractCategories', {
                templateUrl: 'app/contract-categories/contract-categories.html',
                controller: 'ContractCategoriesController',
                controllerAs: 'vm'
            })
            .when('/contractCategory', {
                templateUrl: 'app/contract-categories/contract-category.html',
                controller: 'ContractCategoryController',
                controllerAs: 'vm'
            })
            .when('/contractCategory/:contractCategoryId', {
                templateUrl: 'app/contract-categories/contract-category.html',
                controller: 'ContractCategoryController',
                controllerAs: 'vm'
            })
            .when('/reportingCategories', {
                templateUrl: 'app/reporting-categories/reporting-categories.html',
                controller: 'ReportingCategoriesController',
                controllerAs: 'vm'
            })
            .when('/reportingCategory', {
                templateUrl: 'app/reporting-categories/reporting-category.html',
                controller: 'ReportingCategoryController',
                controllerAs: 'vm'
            })
             .when('/reportingCategory/:reportingCategoryId', {
                 templateUrl: 'app/reporting-categories/reporting-category.html',
                 controller: 'ReportingCategoryController',
                 controllerAs: 'vm'
             })
            .when('/location', {
                templateUrl: 'app/locations/location.html',
                controller: 'LocationController',
                controllerAs: 'vm'
            })
            .when('/location/:locationId', {
                templateUrl: 'app/locations/location.html',
                controller: 'LocationController',
                controllerAs: 'vm'
            })
            .when('/locations', {
                templateUrl: 'app/locations/locations.html',
                controller: 'LocationsController',
                controllerAs: 'vm'
            })
            .when('/engineer', {
                templateUrl: 'app/engineers/engineer.html',
                controller: 'EngineerController',
                controllerAs: 'vm'
            })
            .when('/engineer/:engineerId', {
                templateUrl: 'app/engineers/engineer.html',
                controller: 'EngineerController',
                controllerAs: 'vm'
            })
            .when('/engineers', {
                templateUrl: 'app/engineers/engineers.html',
                controller: 'EngineersController',
                controllerAs: 'vm',
                reloadOnSearch: false
            })
            .when('/home', {
                templateUrl: 'app/home/home.html',
                controller: 'HomeController',
                controllerAs: 'vm'
            })
            .when('/home/:reportmenu', {
                templateUrl: 'app/home/home.html',
                controller: 'HomeController',
                controllerAs: 'vm'
            })
            .when('/report-viewer', {
                templateUrl: 'app/reports/report-viewer.html',
                controller: 'ReportViewerController',
                controllerAs: 'vm'
            })
            .when('/report-viewer/:dummyReportNumber', {
                templateUrl: 'app/reports/report-viewer.html',
                controller: 'ReportViewerController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber/add-payment', {
                templateUrl: 'app/payments/add-payment.html',
                controller: 'AddPaymentController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber/payments/:paymentId', {
                templateUrl: 'app/payments/update-payment.html',
                controller: 'UpdatePaymentController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber/payments', {
                templateUrl: 'app/payments/payments.html',
                controller: 'PaymentsController',
                controllerAs: 'vm',
            })
            .when('/payments/:customQueryName', {
                templateUrl: 'app/payments/open-payments.html',
                controller: 'OpenPaymentsController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber/add-specification', {
                templateUrl: 'app/specifications/add-specification.html',
                controller: 'AddSpecificationController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber/specification', {
                templateUrl: 'app/specifications/specification.html',
                controller: 'SpecificationController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber/add-work-order', {
                templateUrl: 'app/work-orders/add-work-order.html',
                controller: 'AddWorkOrderController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber/update-work-order/:workOrderId', {
                templateUrl: 'app/work-orders/update-work-order.html',
                controller: 'UpdateWorkOrderController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber/work-orders', {
                templateUrl: 'app/work-orders/work-orders.html',
                controller: 'WorkOrdersController',
                controllerAs: 'vm',
                reloadOnSearch: false
            })
            .when('/work-orders', {
                templateUrl: 'app/work-orders/work-orders.html',
                controller: 'WorkOrdersController',
                controllerAs: 'vm'
            })
            .when('/:sectionName/contracts/:contractNumber/work-orders/:workOrderNumber/work-order-histories', {
                templateUrl: 'app/work-order-histories/work-order-histories.html',
                controller: 'WorkOrderHistoriesController',
                controllerAs: 'vm'
            })
            .otherwise({ redirectTo: 'home' });
    }
})();
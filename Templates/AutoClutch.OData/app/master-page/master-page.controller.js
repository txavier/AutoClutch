(function () {
    'use strict';

    angular
        .module('app')
        .controller('masterPageController', masterPageController);

    masterPageController.$inject = ['$scope', '$log', '$location', '$routeParams', 'authenticationService', 'dataService', 'contractService'];

    function masterPageController($scope, $log, $location, $routeParams, authenticationService, dataService, contractService) {
        var masterPageVm = this;

        masterPageVm.loggedInUser = {};
        masterPageVm.showPushOutMenu = false;
        masterPageVm.contract = {
            contractId: 0,
            workOrders: [],
            contractNumber: null
        };
        masterPageVm.showContracts = showContracts;
        masterPageVm.setPushOutMenu = setPushOutMenu;
        masterPageVm.navClass = navClass; 
        masterPageVm.expandMenuProcurement = expandMenuProcurement;
        masterPageVm.expandMenuInterceptorImprovements = expandMenuInterceptorImprovements;
        masterPageVm.expandMenuElectrical = expandMenuElectrical;
        masterPageVm.expandMenuMechanical = expandMenuMechanical;
        masterPageVm.expandMenuPBS = expandMenuPBS;
        masterPageVm.expandMenuProjmgmt = expandMenuProjmgmt;
        masterPageVm.expandMenuBiosolids = expandMenuBiosolids;

        masterPageVm.loadReport1 = false;
        masterPageVm.loadReport2 = false;
        masterPageVm.loadReportQ1 = false;
        masterPageVm.loadReportQ2 = false;
        masterPageVm.reportkpiClick = reportkpiClick;
        masterPageVm.report1Click = report1Click;
        masterPageVm.report2Click = report2Click;
        masterPageVm.reportQ1Click = reportQ1Click;
        masterPageVm.reportQ2Click = reportQ2Click;
        masterPageVm.reportmenu = false;

        activate();

        function activate() {
            masterPageVm.reportmenu = $location.search().reportmenu;

            setContractValues($location.path());

            return masterPageVm;
        }

        function reportkpiClick() {
            setOtherPageToFalse();
            masterPageVm.loadReportkpi = true;
        }

        function report1Click() {
            setOtherPageToFalse();
            masterPageVm.loadReport1 = true;
        }

        function report2Click() {
            setOtherPageToFalse();        
            masterPageVm.loadReport2 = true;
        }

        function reportQ1Click() {
            setOtherPageToFalse();
            masterPageVm.loadReportQ1 = true;
        }

        function reportQ2Click() {
            setOtherPageToFalse();           
            masterPageVm.loadReportQ2 = true;
        }

        function setOtherPageToFalse() {
            masterPageVm.loadReportkpi = false;
            masterPageVm.loadReport1 = false;
            masterPageVm.loadReport2 = false;
            masterPageVm.loadReportQ1 = false;
            masterPageVm.loadReportQ2 = false;

        }

        $scope.$on('$locationChangeStart', function (event) {
            // If the report is loaded and the user is navigating away from the '/report-viewer' path
            // then dont show the report.
            if (masterPageVm.loadReportkpi === true && $location.path() !== "/report-viewer/kpi") {
                masterPageVm.loadReportkpi = false;

            }
            if (masterPageVm.loadReport1 === true && $location.path() !== "/report-viewer/1") {
                masterPageVm.loadReport1 = false;
               
            }

            if (masterPageVm.loadReport2 === true && $location.path() !== "/report-viewer/2") {
                masterPageVm.loadReport2 = false;
            }

            if (masterPageVm.loadReportQ1 === true && $location.path() !== "/report-viewer/3") {
                masterPageVm.loadReportQ1 = false;
            }

            if (masterPageVm.loadReportQ2 === true && $location.path() !== "/report-viewer/4") {
                masterPageVm.loadReportQ2 = false;
            }
            setContractValues($location.path());
        });

        function setContractValues(path) {
            if (path.split('contracts/').length >= 2
                && path.split('contracts/')[1].split('/').length > 0
                && masterPageVm.contract) {

                // If the contract number is not being changed in the url, meaning the user is on
                // the same contract just a different view then do not reset the contract information
                // in the contract service.  The sidebar will need to continue to use this information
                // to keep the sidebar open and reflecting accurate information, i.e. Work Orders (33).
                masterPageVm.contract.contractNumber = path.split('contracts/')[1].split('/')[0];

                contractService.setContract(masterPageVm.contract.contractNumber).then(function (data) {
                    masterPageVm.contract = contractService.getContract();
                });
            }
            else {
                masterPageVm.contract = contractService.getContract();
            }
        }

        function navClass(page) {
            var currentRoute = $location.path().substring(1) || 'home';
            return page === currentRoute ? 'active' : '';
        }

        function expandMenuProcurement(inUl) {
            var currentRoute = $location.path().substring(1) || 'home';
            if (inUl === 'ul') {
                return currentRoute.split('/')[0] === 'Procurement' ? 'active in' : '';
            }
            return currentRoute.split('/')[0] === 'Procurement' ? 'active' : '';
        }

        function expandMenuInterceptorImprovements(inUl) {
            var currentRoute = $location.path().substring(1) || 'home';
            if (inUl === 'ul') {
                return currentRoute.split('/')[0] === 'Interceptor Improvements Contracts' ? 'active in' : '';
            }
            return currentRoute.split('/')[0] === 'Interceptor Improvements Contracts' ? 'active' : '';
        }

        function expandMenuElectrical(inUl) {
            var currentRoute = $location.path().substring(1) || 'home';
            if (inUl === 'ul')
            {
                return currentRoute.split('/')[0] === 'Electrical and Instrumentation Contracts' ? 'active in'  : '';
            }
            return currentRoute.split('/')[0] === 'Electrical and Instrumentation Contracts' ? 'active' : '';

        }

        function expandMenuMechanical(inUl) {
            var currentRoute = $location.path().substring(1) || 'home';
            if (inUl === 'ul') {
                return currentRoute.split('/')[0] === 'Mechanical Contracts' ? 'active in' : '';
            }
            return currentRoute.split('/')[0] === 'Mechanical Contracts' ? 'active' : '';
        }

        function expandMenuPBS(inUl) {
            var currentRoute = $location.path().substring(1) || 'home';
            if (inUl === 'ul') {
                return currentRoute.split('/')[0] === 'PBS-CBS Contracts' ? 'active in' : '';
            }
            return currentRoute.split('/')[0] === 'PBS-CBS Contracts' ? 'active' : '';
        }

        function expandMenuProjmgmt(inUl) {
            var currentRoute = $location.path().substring(1) || 'home';
            if (inUl === 'ul') {
                return currentRoute.split('/')[0] === 'Project Management' ? 'active in' : '';
            }
            return currentRoute.split('/')[0] === 'Project Management' ? 'active' : '';
        }

        function expandMenuBiosolids(inUl) {
            var currentRoute = $location.path().substring(1) || 'home';
            if (inUl === 'ul') {
                return currentRoute.split('/')[0] === 'Biosolids and Building Maintenance Contracts' ? 'active in' : '';
            }
            return currentRoute.split('/')[0] === 'Biosolids and Building Maintenance Contracts' ? 'active' : '';
        }
        function setPushOutMenu(menu) {
            // Reset everything.
            if (menu) {
                setPushOutMenu();
            }

            switch (menu) {
                case 'workOrders':
                    {
                        masterPageVm.showPushOutMenu = true;

                        masterPageVm.showWorkOrders = true;

                        $location.path($location.path().split('/contracts')[0] + '/contracts/' + masterPageVm.contract.contractNumber + '/work-orders');

                        break;
                    }
                case 'payments':
                    {
                        masterPageVm.showPushOutMenu = true;

                        masterPageVm.showPayments = true;

                        $location.path($location.path().split('/contracts')[0] + '/contracts/' + masterPageVm.contract.contractNumber + '/payments');

                        break;
                    }
                case 'changeOrders':
                    {   
                        masterPageVm.showPushOutMenu = true;

                        masterPageVm.showChangeOrders = true;

                        $location.path($location.path().split('/contracts')[0] + '/contracts/' + masterPageVm.contract.contractNumber + '/change-orders');

                        break;
                    }
                case 'evaluations':
                    {
                        masterPageVm.showPushOutMenu = true;

                        masterPageVm.showEvaluations = true;

                        break;
                    }
                default:
                    {
                        masterPageVm.showPushOutMenu = false;

                        masterPageVm.showWorkOrders = false;

                        masterPageVm.showPayments = false;

                        masterPageVm.showChangeOrders = false;

                        masterPageVm.showEvaluations = false;
                    }
            }
        }

        $scope.$watch('masterPageVm.contract.contractId', function (current, original) {
            // If there is a new contract then load it up.
            if (current && current != original) {
                if (masterPageVm.contract.workOrders && masterPageVm.contract.workOrders.length > 0) {
                    //masterPageVm.showPushOutMenu = true;
                }
                else {
                    masterPageVm.showPushOutMenu = false;
                }
            }
            else {
                if ($location.path().split('contracts/').length >= 2 && $location.path().split('contracts/')[1].split('/').length > 0) {

                    // If the contract number is not being changed in the url, meaning the user is on
                    // the same contract just a different view then do not reset the contract information
                    // in the contract service.  The sidebar will need to continue to use this information
                    // to keep the sidebar open and reflecting accurate information, i.e. Work Orders (33).
                    if (masterPageVm.contract && masterPageVm.contract.contractNumber == $location.path().split('contracts/')[1].split('/')[0]) {
                        return;
                    }
                }
                else {
                    masterPageVm.showPushOutMenu = false;
                }
            }
        });

        function showContracts(section) {
            $location.path('/' + section + '/contracts');
        }

    }

})();
(function () {
    'use strict';

    angular
        .module('app')
        .controller('HomeController', HomeController);

    HomeController.$inject = ['$scope', '$log', 'dataService', 'authenticationService'];

    function HomeController($scope, $log, dataService, authenticationService) {
        var vm = this;
        vm.expiringContracts = 0;
        vm.lowFundContracts = 0;
        vm.activeWorkOrders = 0;
        vm.openChangeOrders = 0;
        vm.openPayments = 0;
        vm.missingPaymentPdf = 0;
        vm.missingSpec = 0;
        vm.contractTotalsPerSection = {};
        vm.barLabels = [];          // i.e. ['Marc Burgess', 'Alvin Chavarro', 'James Competelli', 'Nelson Correa', 'Felix Jimenez', 'Donald Josey', 'George Psilakis'];
        vm.barSeries = [];          // i.e. ['This Year', 'Last Year'];
        vm.barData = [[]];          // i.e. [[65, 59, 80, 81, 56, 55, 40], [65, 59, 80, 81, 56, 55, 40]]
        vm.workOrdersPerContractBarLabels = [];          // i.e. ['Marc Burgess', 'Alvin Chavarro', 'James Competelli', 'Nelson Correa', 'Felix Jimenez', 'Donald Josey', 'George Psilakis'];
        vm.workOrdersPerContractBarSeries = [];          // i.e. ['This Year', 'Last Year'];
        vm.workOrdersPerContractBarData = [[]];          // i.e. [[65, 59, 80, 81, 56, 55, 40], [65, 59, 80, 81, 56, 55, 40]]
        vm.doughnutLabels = [];     // i.e. ["Bowery Bay", "Wards Island", "Newtown Creek"];
        vm.doughnutData = [];       // i.e. [2, 23, 1];
        vm.loggedInUser = {};
        vm.isTopLevelUser = isTopLevelUser;
        vm.userActionLogs = [];
        vm.isOverview = isOverview;
        vm.divisionChiefDashboard = false;
        vm.sectionChiefDashboard = false;
        vm.engineerDashboard = false;
        vm.isProcurementSection = false;

        activate();

        function activate() {
            getLoggedInUser().then(function (data) {
                // If this is a procurement section member then show the
                // procurement dashboard directive.
                if (vm.loggedInUser.sectionId === 5) {
                    vm.isProcurementSection = true;
                }

                setDashboardType(vm.loggedInUser);

                searchActiveWorkOrdersCount();

                searchOpenChangeOrdersCount();

                searchExpiringContractsCount();

                searchLowFundContractsCount();

                searchMissingPaymentPdfCount();

                searchMissingSpecCount();

                searchOpenPaymentsCount();

                getLatestUserActionLogs(5);

                //getContractTotalsPerSection();

                // If this is a top level user show them 
                // work orders per contract.
                // Else show this engineer his work orders per contract.
                if (isTopLevelUser()) {
                    getWorkOrdersPerEngineer();
                    getWorkOrdersPerContract();
                } else {
                    getWorkOrdersPerContract();
                }

            });
        }

        function setDashboardType(loggedInUser) {
            vm.divisionChiefDashboard = true;
            vm.sectionChiefDashboard = false;
            vm.engineerDashboard = false;

            if (loggedInUser.sectionChiefRole) {
                vm.divisionChiefDashboard = false;
                vm.sectionChiefDashboard = true;
                vm.engineerDashboard = false;
            }
            else if (loggedInUser.divisionChiefRole) {
                vm.divisionChiefDashboard = true;
                vm.sectionChiefDashboard = false;
                vm.engineerDashboard = false;
            }
            else if (loggedInUser.engineerRole) {
                vm.divisionChiefDashboard = false;
                vm.sectionChiefDashboard = false;
                vm.engineerDashboard = true;
            }
        }

        function isOverview() {
            if (vm.loggedInUser.sectionId) {
                return false;
            }
            else {
                return true;
            }
        }

        function getLatestUserActionLogs(take) {
            return dataService.getLatestUserActionLogs(take).then(function (data) {
                vm.userActionLogs = data;

                return vm.userActionLogs;
            });
        }

        function getLoggedInUser() {
            return authenticationService.getLoggedInUser().then(function (data) {
                vm.loggedInUser = data;

                return vm.loggedInUser;
            });
        }

        function searchActiveWorkOrdersCount() {
            var searchCriteria = {
                q: 'workOrderStatusId=1'
            }
            
            if (vm.loggedInUser && vm.loggedInUser.sectionId) {
                searchCriteria.q += ' and contract/sectionId='  + vm.loggedInUser.sectionId;
            }

            // If this is not a top level user then only show them information for their 
            // contracts.
            if (!isTopLevelUser() && vm.loggedInUser) {
                searchCriteria.q += ' and engineerId=' + vm.loggedInUser.engineerId;
            }

            return dataService.searchEntitiesCountOData('workOrders', searchCriteria).then(function (data) {
                vm.activeWorkOrders = data || 0;

                return vm.activeWorkOrders;
            });
        }

        function isTopLevelUser() {
            if(!vm.loggedInUser) {
                return false;
            }

            var result = vm.loggedInUser.sectionChiefRole || vm.loggedInUser.adminRole || vm.loggedInUser.areaEngineerRole || vm.loggedInUser.divisionChiefRole;

            return result;
        }

        function searchOpenChangeOrdersCount() {
            var searchCriteria = {
                q: 'registered=null'
            }

            // If this user is in the database.
            if (vm.loggedInUser && vm.loggedInUser.sectionId) {
                searchCriteria.q += ' and contract/sectionId=' + vm.loggedInUser.sectionId;
            }

            if (!isTopLevelUser() && vm.loggedInUser) {
                searchCriteria.q += ' and contract/engineerId=' + vm.loggedInUser.engineerId;
            }

            return dataService.searchEntitiesCountOData('changeOrders', searchCriteria).then(function (data) {
                vm.openChangeOrders = data || 0;

                return vm.openChangeOrders;
            });
        }

        function searchExpiringContractsCount() {
            var getLoggedInUser = null;

            if (!isTopLevelUser() && vm.loggedInUser) {
                getLoggedInUser = vm.loggedInUser.engineerId;
            }

            return dataService.getDashboardMetric('expiringContractsCount', getLoggedInUser).then(function (data) {
                vm.expiringContracts = data;

                return vm.expiringContracts;
            });
        }

        function searchLowFundContractsCount() {
            var getLoggedInUser = null;

            if (!isTopLevelUser() && vm.loggedInUser) {
                getLoggedInUser = vm.loggedInUser.engineerId;
            }

            return dataService.getDashboardMetric('lowFundContractsCount', getLoggedInUser).then(function (data) {
                vm.lowFundContracts = data;

                return vm.lowFundContracts;
            });
        }

        function searchOpenPaymentsCount() {
            var searchCriteria = {
                q: 'paymentOut==null'
            }

            // If this user is in the database.
            if (vm.loggedInUser && vm.loggedInUser.sectionId) {
                searchCriteria.q += ' and contract/sectionId=' + vm.loggedInUser.sectionId;
            }

            if (!isTopLevelUser() && vm.loggedInUser) {
                searchCriteria.q += ' and contract/engineerId=' + vm.loggedInUser.engineerId;
            }

            return dataService.searchEntitiesCountOData('payments', searchCriteria).then(function (data) {
                vm.openPayments = data || 0;

                return vm.openPayments;
            });
        }

        function searchMissingPaymentPdfCount() {
            var searchCriteria = {
                q: 'paymentOut!=null and paymentPDF==null and paymentPDFFileId==null'
            }

            // If this user is in the database.
            if (vm.loggedInUser && vm.loggedInUser.sectionId) {
                searchCriteria.q += ' and contract/sectionId=' + vm.loggedInUser.sectionId;
            }

            if (!isTopLevelUser() && vm.loggedInUser) {
                searchCriteria.q += ' and contract/engineerId=' + vm.loggedInUser.engineerId;
            }

            return dataService.searchEntitiesCountOData('payments', searchCriteria).then(function (data) {
                vm.missingPaymentPdf = data || 0;

                return vm.missingPaymentPdf;
            });
        }

        function searchMissingSpecCount() {
            var searchCriteria = {
                q: '(contractStatusId=4 or contractStatusId=6) and (specification=null and specificationFileId=null)'
            }

            // If this user is in the database.
            if (vm.loggedInUser && vm.loggedInUser.sectionId) {
                searchCriteria.q += ' and sectionId=' + vm.loggedInUser.sectionId;
            }

            return dataService.searchEntitiesCountOData('contracts', searchCriteria).then(function (data) {
                vm.missingSpec = data || 0;

                return vm.missingSpec;
            });
        }

        function getContractTotalsPerSection() {
            return dataService.getDashboardGraphData('contractTotalsPerSection').then(function (data) {
                vm.doughnutLabels = [];
                vm.doughnutData = [];

                for (var i = 0; i < data.length; i++) {
                    if (data[i].contractTotal > 0) {
                        vm.doughnutData.push(data[i].contractTotal);

                        vm.doughnutLabels.push(data[i].section);
                    }
                }
            });
        }

        function getContractsPerEngineer() {
            return dataService.getDashboardGraphData('contractsPerEngineer').then(function (data) {
                vm.barLabels = [];
                vm.barSeries = [];
                vm.barData = [[]];

                for (var j = 0; j < data.length; j++) {
                    if (data && data.length > 0) {
                        vm.barSeries.push(data[j][0].series);

                        for (var i = 0; i < data[j].length; i++) {
                            // Only add the labels if this is the first go around to 
                            // fill the label array.  Otherwise duplicate labels
                            // will go into the array.
                            if (j == 0) {
                                vm.barLabels.push(data[j][i].label);

                                // The below line is only here for testing purposes only.
                                vm.barData[j].push(data[j][i].data);
                            }
                        }
                    }
                }
            });
        }

        function getWorkOrdersPerEngineer() {
            return dataService.getDashboardGraphData('workOrdersInCurrentSectionPerEngineer').then(function (data) {
                vm.barLabels = [];
                vm.barSeries = [];
                vm.barData = [[]];

                for (var j = 0; j < data.length; j++) {
                    if (data && data.length > 0 && data[j][0]) {
                        vm.barSeries.push(data[j][0].series);

                        for (var i = 0; i < data[j].length; i++) {
                            // Only add the labels if this is the first go around to 
                            // fill the label array.  Otherwise duplicate labels
                            // will go into the array.
                            if (j == 0) {
                                vm.barLabels.push(data[j][i].label);

                                // The below line is only here for testing purposes only.
                                vm.barData[j].push(data[j][i].data);
                            }
                        }
                    }
                }
            });
        }

        function getWorkOrdersPerContract() {
            return dataService.getDashboardGraphData('workOrdersInCurrentSectionPerContractByEngineer', vm.loggedInUser ? vm.loggedInUser.engineerId : null).then(function (data) {
                vm.workOrdersPerContractBarLabels = [];
                vm.workOrdersPerContractBarSeries = [];
                vm.workOrdersPerContractBarData = [[],[]];

                for (var j = 0; j < data.length; j++) {

                    if (data && data.length > 0 && data[j][0]) {
                        vm.workOrdersPerContractBarSeries.push(data[j][0].series);

                        for (var i = 0; i < data[j].length; i++) {
                            // Only add the labels if this is the first go around to 
                            // fill the label array.  Otherwise duplicate labels
                            // will go into the array.
                            if (j == 0) {
                                vm.workOrdersPerContractBarLabels.push(data[j][i].label);
                            }

                            vm.workOrdersPerContractBarData[j].push(data[j][i].data);
                        }
                    }
                }
            });
        }

    }

})();
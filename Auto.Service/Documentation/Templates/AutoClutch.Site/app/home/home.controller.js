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
        vm.activeChangeOrders = 0;
        vm.openPayments = 0;
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

        activate();

        function activate() {
            getLoggedInUser().then(function (data) {
                //searchActiveWorkOrdersCount();

                //searchOpenChangeOrdersCount();

                //searchExpiringContractsCount();

                //searchLowFundContractsCount();

                //searchOpenPaymentsCount();

                //getLatestUserActionLogs(5);

                //getContractTotalsPerSection();

                // If this is a top level user show them 
                // work orders per contract.
                // Else show this engineer his work orders per contract.
                if (isTopLevelUser()) {
                    getWorkOrdersPerEngineer();
                } else {
                    //getWorkOrdersPerContract();
                }

            });
        }

        function getLatestUserActionLogs(take) {
            return dataService.getLatestUserActionLogs(take).then(function (data) {
                vm.userActionLogs = data.$values;

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

            if (vm.loggedInUser) {
                searchCriteria.q += ' AND contract.sectionId=' + vm.loggedInUser.sectionId;
            }

            // If this is not a top level user then only show them information for their 
            // contracts.
            if (!isTopLevelUser() && vm.loggedInUser) {
                searchCriteria.q += ' AND engineerId=' + vm.loggedInUser.engineerId;
            }

            return dataService.searchEntitiesCount('workOrders', searchCriteria).then(function (data) {
                vm.activeWorkOrders = data || 0;

                return vm.activeWorkOrders;
            });
        }

        function isTopLevelUser() {
            if (!vm.loggedInUser) {
                return false;
            }

            var result = vm.loggedInUser.sectionChiefRole || vm.loggedInUser.adminRole || vm.loggedInUser.areaEngineerRole;

            return result;
        }

        function searchOpenChangeOrdersCount() {
            var searchCriteria = {
                q: 'contract.contractStatusId != 4 AND contract.contractStatusId != 7'
            }

            // If this user is in the database.
            if (vm.loggedInUser) {
                searchCriteria.q += ' AND contract.sectionId=' + vm.loggedInUser.sectionId;
            }

            if (!isTopLevelUser() && vm.loggedInUser) {
                searchCriteria.q += ' AND contract.engineerId=' + vm.loggedInUser.engineerId;
            }

            return dataService.searchEntitiesCount('changeOrders', searchCriteria).then(function (data) {
                vm.openChangeOrders = data || 0;

                return vm.openChangeOrders;
            });
        }

        function searchExpiringContractsCount() {
            var getLoggedInUser = null;

            if (!isTopLevelUser() && vm.loggedInUser) {
                getLoggedInUser = vm.loggedInUser.engineerId;
            }

            return dataService.getDashboardMetric('expiringContracts', getLoggedInUser).then(function (data) {
                vm.expiringContracts = data;

                return vm.expiringContracts;
            });
        }

        function searchLowFundContractsCount() {
            var getLoggedInUser = null;

            if (!isTopLevelUser() && vm.loggedInUser) {
                getLoggedInUser = vm.loggedInUser.engineerId;
            }

            return dataService.getDashboardMetric('lowFundContracts', getLoggedInUser).then(function (data) {
                vm.lowFundContracts = data;

                return vm.lowFundContracts;
            });
        }

        function searchOpenPaymentsCount() {
            var searchCriteria = {
                q: 'paymentOut==null'
            }

            // If this user is in the database.
            if (vm.loggedInUser) {
                searchCriteria.q += ' AND contract.sectionId=' + vm.loggedInUser.sectionId;
            }

            if (!isTopLevelUser() && vm.loggedInUser) {
                searchCriteria.q += ' AND contract.engineerId=' + vm.loggedInUser.engineerId;
            }

            return dataService.searchEntitiesCount('payments', searchCriteria).then(function (data) {
                vm.openPayments = data || 0;

                return vm.openPayments;
            });
        }

        function getContractTotalsPerSection() {
            return dataService.getDashboardMetric('contractTotalsPerSection').then(function (data) {
                vm.doughnutLabels = [];
                vm.doughnutData = [];

                for (var i = 0; i < data.$values.length; i++) {
                    if (data.$values[i].contractTotal > 0) {
                        vm.doughnutData.push(data.$values[i].contractTotal);

                        vm.doughnutLabels.push(data.$values[i].section);
                    }
                }
            });
        }

        function getContractsPerEngineer() {
            return dataService.getDashboardMetric('contractsPerEngineer').then(function (data) {
                vm.barLabels = [];
                vm.barSeries = [];
                vm.barData = [[]];

                for (var j = 0; j < data.$values.length; j++) {
                    if (data && data.$values.length > 0) {
                        vm.barSeries.push(data.$values[j].$values[0].series);

                        for (var i = 0; i < data.$values[j].$values.length; i++) {
                            // Only add the labels if this is the first go around to 
                            // fill the label array.  Otherwise duplicate labels
                            // will go into the array.
                            if (j == 0) {
                                vm.barLabels.push(data.$values[j].$values[i].label);

                                // The below line is only here for testing purposes only.
                                vm.barData[j].push(data.$values[j].$values[i].data);
                            }
                        }
                    }
                }
            });
        }

        function getWorkOrdersPerEngineer() {
            return dataService.getDashboardMetric('workOrdersInCurrentSectionPerEngineer').then(function (data) {
                vm.barLabels = [];
                vm.barSeries = [];
                vm.barData = [[]];

                for (var j = 0; j < data.$values.length; j++) {
                    if (data && data.$values.length > 0) {
                        vm.barSeries.push(data.$values[j].$values[0].series);

                        for (var i = 0; i < data.$values[j].$values.length; i++) {
                            // Only add the labels if this is the first go around to 
                            // fill the label array.  Otherwise duplicate labels
                            // will go into the array.
                            if (j == 0) {
                                vm.barLabels.push(data.$values[j].$values[i].label);

                                // The below line is only here for testing purposes only.
                                vm.barData[j].push(data.$values[j].$values[i].data);
                            }
                        }
                    }
                }
            });
        }

        function getWorkOrdersPerContract() {
            return dataService.getDashboardMetric('workOrdersInCurrentSectionPerContractByEngineer', vm.loggedInUser ? vm.loggedInUser.engineerId : null).then(function (data) {
                vm.workOrdersPerContractBarLabels = [];
                vm.workOrdersPerContractBarSeries = [];
                vm.workOrdersPerContractBarData = [[]];

                for (var j = 0; j < data.$values.length; j++) {
                    if (data && data.$values.length > 0) {
                        vm.workOrdersPerContractBarSeries.push(data.$values[j].$values[0].series);

                        for (var i = 0; i < data.$values[j].$values.length; i++) {
                            // Only add the labels if this is the first go around to 
                            // fill the label array.  Otherwise duplicate labels
                            // will go into the array.
                            if (j == 0) {
                                vm.workOrdersPerContractBarLabels.push(data.$values[j].$values[i].label);

                                // The below line is only here for testing purposes only.
                                vm.workOrdersPerContractBarData[j].push(data.$values[j].$values[i].data);
                            }
                        }
                    }
                }
            });
        }

    }

})();
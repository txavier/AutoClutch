(function () {
    'use strict';
    
    /**
    * @desc Contract details directive for use above pages where the contract information is needed.
    * @example <dep-contract-details data-dep-contract-number="contractNumber"></dep-contract-details>
    */
    angular
        .module('shared.directives')
        .directive('depContractDetails', depContractDetails);

    function depContractDetails() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'app/contracts/contract-details.html',
            require: 'ngModel',
            scope: {
                title: '@',
                model: '=ngModel',
                ngChange: '&',
                isOpen: '='
            },
            link: link,
            controller: contractDetailsController,
            controllerAs: 'vm',
            bindToController: true // because the scope is isolated
        };

        return directive;

        function link(scope, element, attrs, vm) {
        }
    }

    contractDetailsController.$inject = ['$scope', '$timeout', '$routeParams', '$filter', '$location', 'contractService', 'dataService', 
        'authenticationService', 'toaster', 'sharedService'];

    function contractDetailsController($scope, $timeout, $routeParams, $filter, $location, contractService, dataService,  authenticationService, toaster,
        sharedService) {
        var vm = this;

        vm.updateModel = updateModel;
        vm.updateContract = updateContract;
        vm.disableAll = false;
        vm.model = {};
        vm.loggedInUser = {};
        vm.accessRoleDisable = false;
        vm.contractorContactPersons = [];
        vm.renewalClick = renewalClick;
        vm.contractNumber = null;
        vm.paidToDateTitle = 'Paid To Date (Line A)';
        vm.updateContractor = updateContractor;
        vm.objectCodes = [];
        vm.budgetCodes = [];
        vm.setObject = setObject;

        activate();

        function activate() {
            vm.contractNumber = $routeParams.contractNumber;

            vm.sectionName = $routeParams.sectionName;

            getContract(vm.contractNumber).then(function (data) {
                if (data) {
                    vm.contractNumberLink = (vm.model.contract11 && vm.model.contract11.length > 0) ?
                        vm.model.contract11[0].contractNumber : (vm.model.contract3 ? vm.model.contract3.contractNumber : '');

                    setViewDisabledBoxes(data);
                    setAuthorizationDisabledBoxes();
                    getEngineers();
                    getContractorContactPersons();
                    setPaidToDateTitle();
                }
            });

            getContractors();

            getContractStatuses();

            getObjectCodes({ orderBy: 'objectCodeDescription' });

            getBudgetCodes({ orderBy: 'budgetCodeDescription' });
        }

        function setObject(model, propertyName, propertyNameId, entityArray, id) {
            var result = sharedService.setObject(model, propertyName, propertyNameId, entityArray, id);

            return result;
        }

        function getObjectCodes(searchCriteria) {
            return dataService.searchEntitiesOData('objectCodes', searchCriteria).then(function (data) {
                vm.objectCodes = data;

                return vm.objectCodes;
            });
        }

        function getBudgetCodes(searchCriteria) {
            return dataService.searchEntitiesOData('budgetCodes', searchCriteria).then(function (data) {
                vm.budgetCodes = data;

                return vm.budgetCodes;
            });
        }

        function updateContractor(contractId, model) {
            updateContract(contractId, model).then(getContractorContactPersons);
        }

        // If the contract type is an open market order then dont show 'Line A' in the title as
        // there is no line A for open market orders.
        function setPaidToDateTitle() {
            // Contract Type 1 is an Open Market Order.
            vm.paidToDateTitle = vm.model.contractTypeId === 1 ? 'Paid To Date' : 'Paid To Date (Line A)';
        }

        function setAuthorizationDisabledBoxes() {
            authenticationService.getLoggedInUser().then(function (data) {
                vm.loggedInUser = data;

                if (vm.loggedInUser.engineerRole) {
                    vm.accessRoleDisable = true;
                }
            });
        }

        function getContractors() {
            var searchCriteria = {
                currentPage: 1,
                itemsPerPage: 100,
                orderBy: null,
                searchText: null,
                includeProperties: null
            }

            return dataService.searchEntitiesOData('contractors', searchCriteria, true).then(function (data) {
                vm.contractors = data;

                vm.contractors = $filter('orderBy')(vm.contractors, 'name');

                // Set the value on the contract so that the drop down box can properly find the right object.
                // Without this the drop down box wont know which value to match up with and it will show 'Nothing Selected'.
                vm.model.contractor = vm.contractors[vm.contractors.getIndexBy('contractorId', vm.model.contractorId)];

                return vm.contractors;
            });
        }

        function getEngineers() {
            var searchCriteria = {
                currentPage: 1,
                itemsPerPage: 100,
                orderBy: null,
                searchText: null,
                includeProperties: null,
                q: 'section/name="' + vm.sectionName + '"'
            }

            return dataService.searchEntitiesOData('engineers', searchCriteria, true).then(function (data) {
                vm.engineers = data;

                vm.engineers = vm.engineers.sort(compare);

                var engineerId = null;

                if (vm.model.currentEngineerIdDisplay) {
                    engineerId = vm.model.currentEngineerIdDisplay;
                }
                else {
                    engineerId = vm.model.engineerId;
                }

                // Set the value on the contract so that the drop down box can properly find the right object.
                // Without this the drop down box wont know which value to match up with and it will show 'Nothing Selected'.
                vm.model.engineer = vm.engineers[vm.engineers.getIndexBy('engineerId', engineerId)];

                return vm.engineers;
            });
        }

        // Helper function for sorting by last name where the last name is after the first name in the list
        // i.e. 'George Bush'.
        function compare(a, b) {
            var splitA = a['name'].split(" ");
            var splitB = b['name'].split(" ");
            var lastA = splitA[splitA.length - 1];
            var lastB = splitB[splitB.length - 1];

            if (lastA < lastB) return -1;
            if (lastA > lastB) return 1;
            return 0;
        }

        function getContractorContactPersons() {
            if (vm.model && vm.model.contractor && vm.model.contractor.contactPerson) {
                var searchCriteria = {
                    currentPage: 1,
                    itemsPerPage: 100,
                    orderBy: null,
                    searchText: null,
                    includeProperties: null,
                    q: 'contractorId=' + vm.model.contractorId
                }

                return dataService.searchEntitiesOData('contractorContactPersons', searchCriteria, true).then(function (data) {
                    vm.contractorContactPersons = [];

                    vm.contractorContactPersons.push(
                        {
                            contractorContactPersonId: 0,
                            nameDisplay: vm.model.contractor.contactPerson,
                            firstName: vm.model.contractor.contactPerson.split(' ')[0],
                            lastName: vm.model.contractor.contactPerson.split(' ')[1],
                            contractorId: vm.model.contractorId,
                            phoneNumber: vm.model.contractor.phoneNumber,
                            emailAddress: vm.model.contractor.emailAddress
                        });

                    vm.contractorContactPersons = vm.contractorContactPersons.concat(data);

                    vm.model.contractorContactPersonId = vm.contractorContactPersons[0].contractorContactPersonId;

                    // Set the current contractor contact person.
                    //if (vm.model.contractorContactPersonId) {
                    //    vm.model.contractorContactPerson =
                    //        vm.contractorContactPersons[vm.contractorContactPersons.getIndexBy('contractorContactPersonId', vm.model.contractorContactPersonId)];
                    //}

                    return vm.contractorContactPersons;
                });
            }
            else {
                return;
            }
        }

        function getContractStatuses() {
            return dataService.getEntitiesOData('contractStatuses', null, true).then(function (data) {
                var orderedData = $filter('orderBy')(data, 'sort');

                vm.contractStatuses = orderedData;

                //vm.model.contractStatus = vm.contractStatuses[vm.contractStatuses.getIndexBy('contractStatusId', vm.model.contractStatusId)];

                return vm.contractStatuses;
            });
        }

        Array.prototype.getIndexBy = function (name, value) {
            for (var i = 0; i < this.length; i++) {
                if (this[i][name] == value) {
                    return i;
                }
            }
        }

        function setViewDisabledBoxes(model) {
            // The contract status 'Closed' is contract status ID 5.
            // The contract status 'Cancelled' is contract status ID 7.
            // If this is a closed or cancelled contract then we are not allowing the 
            // modification of any of the elements.
            // Also disable if this contract in a contract status greater than 'Registered' as per Work Item 431
            if (model && (model.contractStatusId == 5 || model.contractStatusId == 7 || (model.contractStatus.sort > 4))) {
                vm.disableAll = true;
            }
            else {
                vm.disableAll = false;
            }

            return vm.disableAll;
        }

        function updateModel(item) {
            vm.ngModel = item;

            $timeout(vm.ngChange, 0);
        }

        function getContract(contractNumber) {
            var contractSearchCriteria = {
                currentPage: 1,
                itemsPerPage: 1,
                orderBy: null,
                searchText: null,
                includeProperties: 'engineerContracts($expand = engineer),contractor,contractStatus,contractType,contractCategory,workOrders,'
                    + 'workOrders($expand=location),payments($expand=paymentType),changeOrders($expand=changeOrderType),section,payments($expand=deductions),'
                    + 'workOrders($expand=serviceType),workOrders($expand=repairType),workOrders($expand=workOrderStatus),contract11,type,contract3,contract2',
                q: 'contractNumber="' + contractNumber + '"'
            };

            return dataService.searchEntitiesOData('contracts', contractSearchCriteria).then(function (data) {
                vm.model = data[0];

                return vm.model;
            });
        }

        function renewalClick() {
            if (vm.model.contract11 && vm.model.contract11.length > 0) {
                // Then this contract is the renewal and a click indicates that we want to 
                // take the user to the parent contract.
                $location.path('/' + vm.model.section.name + '/contracts/' + vm.model.contract11[0].contractNumber);
            }
            else if (vm.model.renewalContractNumber) {
                // Else this contract is a parent and a click indicates that we want to 
                // take the user to the renewal.
                $location.path('/' + vm.model.section.name + '/contracts/' + vm.model.renewalContractNumber);
            }
            else if (vm.model.section) {
                // Else this contract does not have a renewal and is not a renewal and
                // a click indicates that we want to take the user to a new renewal 
                // view.
                $location.path('/' + vm.model.section.name + '/contracts/' + vm.model.contractNumber + '/add-renewal-contract');
            }
        }

        function updateContract(id, contract) {
            // The contract status 'Closed' is contract status ID 5.
            // The contract status 'Cancelled' is contract status ID 7.
            // If this is a closed or cancelled contract then we are not allowing the 
            // modification of any of the elements.
            // Also disable if this contract in a contract status greater than 'Registered' as per Work Item 431
            if ((vm.disableAll || vm.accessRoleDisable) && !(vm.loggedInUser.sectionChiefRole || false)) {
                activate();

                toaster.pop('warning', 'This contract cannot be updated because it is in the ' + contract.contractStatus.name + ' status.');

                return;
            }

            //if (contract.contractor) {
            //    contract.contractorId = contract.contractor.contractorId;
            //}

            //contract.contractorContactPersonId = contract.contractorContactPerson ?
            //    contract.contractorContactPerson.contractorContactPersonId : contract.contractorContactPersonId;

            // Before cleaning all of the arrays away from the contract,
            // save this one and make sure that you reassign it after the 
            // updating is done.
            this.model.projectRetainage = parseFloat(this.model.projectRetainageDisplay) / 100.0;

            this.model.mOrWBERequirement = parseFloat(this.model.mOrWBERequirementDisplay) / 100.0;

            this.model.contractExpirationDate = this.model.contractExpirationDateDisplay;

            var tempContract = {};

            //tempContract.contractorContactPerson = contract.contractorContactPerson;

            tempContract.engineerContracts = contract.engineerContracts;

            tempContract.contractCategory = contract.contractCategory;

            tempContract.contractStatus = contract.contractStatus;

            tempContract.contractType = contract.contractType;

            tempContract.contractor = contract.contractor;

            tempContract.engineers = contract.engineers;

            tempContract.changeOrders = contract.changeOrders;

            tempContract.contractors = contract.contractors;

            tempContract.contracts = contract.contracts;

            tempContract.contract1 = contract.contract1;

            tempContract.contract12 = contract.contract12;

            tempContract.contract4 = contract.contract4;

            tempContract.contract2 = contract.contract2;

            tempContract.contract3 = contract.contract3;

            tempContract.contract11 = contract.contract11;

            tempContract.section = contract.section;

            tempContract.section.contracts = contract.section.contracts;

            tempContract.section.engineers = contract.section.engineers;

            tempContract.payments = contract.payments;

            tempContract.currentEngineerIdDisplay = contract.currentEngineerIdDisplay;

            tempContract.workOrders = contract.workOrders;

            //tempContract.engineer = contract.engineer;

            //if (contract.contractStatus) {
            //    contract.contractStatusId = contract.contractStatus.contractStatusId;
            //}

            //if (contract.engineer) {
            //    contract.engineerId = contract.engineer.engineerId;
            //}

            // If this contractor contact person already exists in the database,
            // as indicated by the contractorContactPersonId being there and 
            // not being null or if there is no last name, then there is no need to send the whole object 
            // to the server to be saved.
            //if ((contract.contractorContactPerson && contract.contractorContactPerson.contractorContactPersonId && contract.contractorContactPerson.contractContactPersonId != 0)
            //    || (contract.contractorContactPerson && !contract.contractorContactPerson.lastName)) {
            //    contract.contractorContactPerson = null;
            //}

            contract.contractorContactPerson = null;

            contract.contractorContactPersonId = null;

            contract.currentEngineerIdDisplay = null;

            contract.engineerContracts = null;

            contract.contractCategory.contracts = null;

            contract.contractStatus.contracts = null;

            contract.contractType.contracts = null;

            contract.contractor = null;

            contract.engineers = null;

            contract.changeOrders = null;

            contract.contractors = null;

            contract.contracts = null;

            contract.contract1 = null;

            contract.contract12 = null;

            contract.contract4 = null;

            contract.contract2 = null;

            contract.contract3 = null;

            contract.contract11 = null;

            contract.section.contracts = null;

            contract.section.engineers = null;

            contract.payments = null;

            contract.workOrders = null;

            contract.engineer = null;

            return dataService.updateEntity('contracts', id, contract).then(function () {
            //return dataService.updateEntityOData('contracts', id, contract).then(function () {
                //contract.contractorContactPerson = tempContract.contractorContactPerson;

                contract.currentEngineerIdDisplay = tempContract.engineerId;

                contract.engineerContracts = tempContract.engineerContracts;

                contract.contractCategory = tempContract.contractCategory;

                contract.contractStatus = tempContract.contractStatus;

                contract.contractType = tempContract.contractType;

                contract.contractor = tempContract.contractor;

                contract.engineers = tempContract.engineers;

                contract.changeOrders = tempContract.changeOrders;

                contract.contractors = tempContract.contractors;

                contract.contracts = tempContract.contracts;

                contract.contract1 = tempContract.contract1;

                contract.contract12 = tempContract.contract12;

                contract.contract4 = tempContract.contract4;

                contract.contract2 = tempContract.contract2;

                contract.contract3 = tempContract.contract3;

                contract.contract11 = tempContract.contract11;

                contract.section.contracts = tempContract.section.contracts;

                contract.section.engineers = tempContract.section.engineers;

                contract.payments = tempContract.payments;

                contract.workOrders = tempContract.workOrders;

                //contract.engineer = tempContract.engineer;

                // If the contract number itself was updated then set this 
                // local variable so that when the page is reloaded it is
                // reloading the correct contract number.
                if (vm.contractNumber != contract.contractNumber) {
                    $location.path('/' + vm.model.section.name + '/contracts/' + contract.contractNumber);
                }

                updateModel(vm.model);

                activate();
            }, function () {
                activate();
            });
        }
    }

})();
(function () {
    'use strict';

    angular
        .module('app')
        .factory('contractService', contractService);

    contractService.$inject = ['$timeout', '$filter', 'dataService'];

    // The purpose of this service is to hold contract information so that the current
    // contract being worked on by the user can have its information displayed on the 
    // sidebar as well as on the current page.  This service is the central place a 
    // loaded contract holds its information.
    function contractService($timeout, $filter, dataService) {
        var contract = {
            contractId: 0,
            workOrders: [],
            contractNumber: null,
            payments: [],
            changeOrders: [],
            evaluations: [],
            section: null
        };

        var service = {
            getContract: getContract,
            setContractId: setContractId,
            setContractWorkOrders: setContractWorkOrders,
            setContractNumber: setContractNumber,
            setPayments: setPayments,
            setChangeOrders: setChangeOrders,
            setEvaluations: setEvaluations,
            setSection: setSection,
            setContract: setContract
        };

        return service;

        function getContract() {
            return contract;
        }

        function setContract(contractNumber) {
            var contractSearchCriteria = {
                currentPage: 1,
                itemsPerPage: 1,
                orderBy: null,
                searchText: null,
                //includeProperties: 'engineerContracts.engineer, contractor, contractStatus, contractType, contractCategory, workOrders, '
                //    + 'workOrders.location, payments.paymentType, changeOrders.changeOrderType, section, payments.deductions.deductionType, '
                //    + 'workOrders.serviceType, workOrders.repairType, workOrders.workOrderStatus, contract11',
                includeProperties: 'engineerContracts($expand = engineer),contractor,contractStatus,contractType,contractCategory,workOrders,'
                    + 'workOrders($expand=location),payments($expand=paymentType),changeOrders($expand=changeOrderType),section,payments($expand=deductions),'
                    + 'workOrders($expand=serviceType),workOrders($expand=repairType),workOrders($expand=workOrderStatus),contract11',
                q: 'contractNumber="' + contractNumber + '"'
            }

            // TODO Check for this ** warning deduction type is missing ** //
            return dataService.searchEntitiesOData('contracts', contractSearchCriteria).then(function (data) {
                contract = data[0];

                if(contract) {
                    // Set the contract service contract information for use in the 
                    // 'pushOut' sidebar.
                    setContractId(contract.contractId);

                    setContractWorkOrders(contract.workOrders == null ? null : contract.workOrders);

                    setContractNumber(contract.contractNumber);

                    setPayments(contract.payments == null ? null : contract.payments);

                    setChangeOrders(contract.changeOrders == null ? null : contract.changeOrders);

                    setEvaluations(contract.evaluations == null ? null : contract.evaluations);

                    setSection(contract.section);
                }

                return contract;
            });
        }

        function setContractId(contractId) {
            if (contract) {
                contract.contractId = contractId;
            }
        }

        function setContractWorkOrders(workOrders) {
            if (contract) {
                contract.workOrders = workOrders;
            }
        }

        function setContractNumber(contractNumber) {
            if (contract) {
                contract.contractNumber = contractNumber;
            }
        }

        function setPayments(payments) {
            if (contract) {
                var result = $filter('filter')(payments, { futurePayment: null });

                var falseFuturePayments = $filter('filter')(payments, { futurePayment: false });

                result = result.concat(falseFuturePayments);

                contract.payments = result;
            }
        }

        function setChangeOrders(changeOrders) {
            if (contract) {
                contract.changeOrders = changeOrders;
            }
        }

        function setEvaluations(evaluations) {
            if (contract) {
                contract.evaluations = evaluations;
            }
        }

        function setSection(section) {
            if (contract) {
                contract.section = section;
            }
        }

    }
})();
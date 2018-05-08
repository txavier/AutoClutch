using AutoClutch.Controller;
using AutoClutch.Core.Interfaces;
using $safeprojectname$.Core.Models;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    public class ContractCategoriesODataController : ODataApiController<contractCategory>
    {
        private IService<contractCategory> _contractCategoryService;

        public ContractCategoriesODataController(IService<contractCategory> contractCategoryService)
            : base(contractCategoryService)
        {
            _contractCategoryService = contractCategoryService;
        }

    }
}

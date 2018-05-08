using AutoClutch.Core.Interfaces;
using $safeprojectname$.Core.Models;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/contractCategories")]
    public class ContractCategoriesController : BaseApiController<contractCategory>
    {
        private readonly IService<contractCategory> _contractCategoryService;

        public ContractCategoriesController(IService<contractCategory> contractCategoryService)
            : base(contractCategoryService)
        {
            _contractCategoryService = contractCategoryService;
        }

    }
}

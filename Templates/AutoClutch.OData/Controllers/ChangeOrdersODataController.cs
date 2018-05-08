using AutoClutch.Controller;
using AutoClutch.Core.Interfaces;
using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.Core.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    public class ChangeOrdersODataController : ODataApiController<changeOrder>
    {
        private IChangeOrderService _changeOrderService;

        public ChangeOrdersODataController(IChangeOrderService changeOrderService, ILogService<changeOrder> logService)
            : base(changeOrderService, logService)
        {
            _changeOrderService = changeOrderService;
        }

        //Everything below is copied 
        public async Task<IHttpActionResult> DeleteChangeOrderFile(int changeOrderId)
        {
            var result = await _changeOrderService
                .DeleteChangeOrderFileAsync(changeOrderId, User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault());

            return Ok();
        }

        
        public IHttpActionResult GetInitialChangeOrder(int contractId, int changeOrderTypeId)
        {
            var result = _changeOrderService.GetInitialChangeOrder(contractId, changeOrderTypeId);

            return Ok(result);
        }
    }
}

using AutoClutch.Core.Interfaces;
using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.Core.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/changeOrders")]
    public class ChangeOrdersController : BaseApiController<changeOrder>
    {
        private IChangeOrderService _changeOrderService;

        public ChangeOrdersController(IChangeOrderService changeOrderService, ILogService<changeOrder> logService)
            : base(changeOrderService, logService)
        {
            _changeOrderService = changeOrderService;
        }

        [Route("deleteChangeOrderFile/{changeOrderId}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteChangeOrderFile(int changeOrderId)
        {
            var result = await _changeOrderService
                .DeleteChangeOrderFileAsync(changeOrderId, User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault());

            return Ok();
        }

        [Route("count")]
        [HttpGet]
        public IHttpActionResult Count(string q = null)
        {
            var result = base.BaseCount(q);

            return Ok(result);
        }

        [Route("getInitialChangeOrder")]
        [HttpGet]
        public IHttpActionResult GetInitialChangeOrder(int contractId, int changeOrderTypeId)
        {
            var result = _changeOrderService.GetInitialChangeOrder(contractId, changeOrderTypeId);

            return Ok(result);
        }
    }
}

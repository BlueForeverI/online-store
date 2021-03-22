using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Http;
using System.Net.Http;
using System.Net;
using OnlineStore.Services;
using OnlineStore.Services.DTO;

namespace OnlineStore.WebUI.Apis
{
    [Authorize(Roles = "Admin")]
    public class OrderController : ApiController
    {
        private OrderService _service = new OrderService();

        // GET api/<controller>
        public List<OrderDTO> Get()
        {
            return _service.GetOrderDTOs();
        }
        [Route("api/Order/GetOrderItems/{id}")]
        public List<OrderItemDTO> GetOrderItems(int id)
        {
            return _service.GetOrderItemDTOs(id);
        }

        // GET: api/Order/GetCount/
        [Route("api/Order/GetCount")]
        public int GetCount()
        {
            return _service.Count();
        }

        public HttpResponseMessage Delete(int id)
        {
            var order = _service.Get(id);
            if (order == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No such order [" + id + "].");
            }
            _service.Delete(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
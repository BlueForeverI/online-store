using OnlineStore.Domain.Infrastructure;
using OnlineStore.Domain.Model;
using OnlineStore.Services.DTO;
using OnlineStore.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.Services
{
    public class OrderService : BaseService<Order>
    {
        public List<OrderDTO> GetOrderDTOs()
        {
            List<OrderDTO> list = new List<OrderDTO>();

            using (OnlineStoreDBContext context = new OnlineStoreDBContext())
            {
                var orders = from o in context.Orders
                             join u in context.Users
                               on o.UserId equals u.Id
                             orderby o.Id descending
                             select new { o.Id, o.UserId, u.UserName, o.FullName, o.Address, o.City, o.State, o.Zip, o.ConfirmationNumber, o.DeliveryDate };
                list = orders.Select(o => new OrderDTO { OrderId = o.Id, UserId = o.UserId, UserName = o.UserName, FullName = o.FullName, 
                    Address = o.Address, City = o.City, State = o.State, Zip = o.Zip, ConfirmationNumber = o.ConfirmationNumber, DeliveryDate = o.DeliveryDate }).ToList();
            }

            return list;
        }


        public List<OrderDTO> GetOrderDTOs(int productId)
        {
            using (OnlineStoreDBContext context = new OnlineStoreDBContext())
            {
                var orders = from o in context.Orders
                             join i in context.OrderItems
                               on o.Id equals i.OrderId
                             join u in context.Users
                               on o.UserId equals u.Id
                             where i.ProductId == productId
                             orderby o.Id descending
                             select new { o.Id, o.UserId, u.UserName, o.FullName, o.Address, o.City, o.State, o.Zip, o.ConfirmationNumber, o.DeliveryDate };
                return orders.Select(o => new OrderDTO { OrderId = o.Id, UserId = o.UserId, UserName = o.UserName, 
                    FullName = o.FullName, Address = o.Address, City = o.City, State = o.State, Zip = o.Zip, 
                    ConfirmationNumber = o.ConfirmationNumber, DeliveryDate = o.DeliveryDate }).ToList();
            }
        }
        public List<OrderItemDTO> GetOrderItemDTOs(int id)
        {
            List<OrderItemDTO> list = new List<OrderItemDTO>();
            using (OnlineStoreDBContext context = new OnlineStoreDBContext())
            {
                var orderitems = from i in context.OrderItems
                                 join p in context.Products
                                 on i.ProductId equals p.Id
                                 join c in context.Categories
                                 on p.CategoryId equals c.Id
                                 where i.OrderId == id
                                 select new { i.Id, i.OrderId, i.ProductId, p.ProductName, p.CategoryId, c.CategoryName, p.Price, p.Image, p.Condition, p.Discount, i.Quantity };
                list = orderitems.Select(o => new OrderItemDTO { OrderItemId = o.Id, OrderId = o.OrderId, ProductId = o.ProductId, ProductName = o.ProductName, CategoryId = o.CategoryId, CategoryName = o.CategoryName, Price = o.Price, Image = o.Image, Condition = o.Condition, Discount = o.Discount, Quantity = o.Quantity }).ToList();
            }

            return list;
        }

        public void AddOrderItem(OrderItem orderItem)
        {
            using (OnlineStoreDBContext context = new OnlineStoreDBContext())
            {
                context.OrderItems.Add(orderItem);
                context.SaveChanges();
            }
        }

        public OrderViewModel GetOrderViewModel(int orderId)
        {
            using (OnlineStoreDBContext context = new OnlineStoreDBContext())
            {
                var order = from o in context.Orders
                            join u in context.Users
                              on o.UserId equals u.Id
                            where o.Id == orderId
                            select new { o.Id, o.UserId, u.UserName, o.FullName, o.Address, o.City, o.State, o.Zip, o.ConfirmationNumber, o.DeliveryDate };
                var ord = order.FirstOrDefault();
                return new OrderViewModel { OrderId = ord.Id, UserId = ord.UserId, UserName = ord.UserName, FullName = ord.FullName, Address = ord.Address, City = ord.City, Zip = ord.Zip, ConfirmationNumber = ord.ConfirmationNumber, DeliveryDate = ord.DeliveryDate };
            }
        }

        public List<OrderItemViewModel> GetOrderItemViewModels(int orderId)
        {
            using (OnlineStoreDBContext context = new OnlineStoreDBContext())
            {
                var orderitems = from i in context.OrderItems
                                 join p in context.Products
                                   on i.ProductId equals p.Id
                                 join c in context.Categories
                                   on p.CategoryId equals c.Id
                                 where i.OrderId == orderId
                                 select new { i.Id, i.OrderId, i.ProductId, p.ProductName, p.CategoryId, c.CategoryName, p.Price, p.Image, p.Condition, p.Discount, i.Quantity };
                return orderitems.Select(o => new OrderItemViewModel { 
                    OrderItemId = o.Id, OrderId = o.OrderId, ProductId = o.ProductId, ProductName = o.ProductName, CategoryId = o.CategoryId, CategoryName = o.CategoryName, Price = o.Price, Image = o.Image, Condition = o.Condition, Discount = o.Discount, Quantity = o.Quantity
                }).ToList();
            }
        }

        public int CountUserOrders(string userId)
        {
            using (OnlineStoreDBContext context = new OnlineStoreDBContext())
            {
                return context.Orders.Where(o => o.UserId == userId).Count();
            }
        }
    }
}

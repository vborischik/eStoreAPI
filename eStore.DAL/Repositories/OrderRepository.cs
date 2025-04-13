using Dapper;
using eStore.DAL.eStore.DAL;
using eStore.DAL.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.DAL.Repositories
{
    public class OrderRepository : BaseDAL, IOrderRepository
    {
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderRepository(IConfiguration configuration, string connectionName, IOrderDetailRepository orderDetailRepository)
            : base(configuration, connectionName)
        {
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<int> AddOrder(OrderDTO order)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@p_CustomerID", order.CustomerID);
            parameters.Add("@p_OrderDate", order.OrderDate);
            parameters.Add("@p_TotalAmount", order.TotalAmount);
            parameters.Add("@p_OrderStatus", order.OrderStatus.ToString());
            parameters.Add("p_OrderID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await ExecuteAsync("AddOrder", parameters, commandType: CommandType.StoredProcedure);
            int orderId = parameters.Get<int>("p_OrderID");

            // Add order details if any
            if (order.OrderDetails != null && order.OrderDetails.Count > 0)
            {
                foreach (var detail in order.OrderDetails)
                {
                    detail.OrderID = orderId;
                    await _orderDetailRepository.AddOrderDetail(detail);
                }
            }

            return orderId;
        }

        public async Task<int> DeleteOrder(int id)
        {
            // First delete all order details
            await _orderDetailRepository.DeleteOrderDetailsByOrderId(id);

            // Then delete the order
            var sql = "DELETE FROM Orders WHERE OrderID = @Id";
            return await ExecuteAsync(sql, new { Id = id });
        }

public async Task<IEnumerable<OrderDTO>> GetAllOrders()
{
    var sql = @"
        SELECT
            o.OrderID,
            o.CustomerID,
            o.OrderDate,
            o.TotalAmount,
            o.OrderStatus,
            CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName
        FROM Orders o
        INNER JOIN Customers c ON o.CustomerID = c.CustomerID
        ORDER BY o.OrderDate DESC";

    var orders = await QueryAsync<OrderDTO>(sql);

    // Populate order details
    foreach (var order in orders)
    {
        order.OrderDetails = (await _orderDetailRepository.GetOrderDetailsByOrderId(order.OrderID)).ToList();
    }

    return orders;
}


      public async Task<IEnumerable<OrderDTO>> GetAllOrders(int pageNumber, int pageSize)
{
    var sql = @"
        SELECT
            o.OrderID,
            o.CustomerID,
            o.OrderDate,
            o.TotalAmount,
            o.OrderStatus,
            CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName
        FROM Orders o
        INNER JOIN Customers c ON o.CustomerID = c.CustomerID
        ORDER BY o.OrderDate DESC
        LIMIT @PageSize OFFSET @Offset";

    var orders = await QueryAsync<OrderDTO>(sql, new { PageSize = pageSize, Offset = (pageNumber - 1) * pageSize });

    // Optionally load order details
    // foreach (var order in orders)
    // {
    //     order.OrderDetails = (await _orderDetailRepository.GetOrderDetailsByOrderId(order.OrderID)).ToList();
    // }

    return orders;
}


        public async Task<int> GetTotalOrderCount()
        {
            var sql = "SELECT COUNT(*) FROM Orders";
            return await QuerySingleAsync<int>(sql);
        }

        public async Task<OrderDTO> GetOrderById(int id)
        {
            var sql = "SELECT * FROM Orders WHERE OrderID = @Id";
            var order = await QuerySingleAsync<OrderDTO>(sql, new { Id = id });

            if (order != null)
            {
                order.OrderDetails = (await _orderDetailRepository.GetOrderDetailsByOrderId(order.OrderID)).ToList();
            }

            return order ?? new OrderDTO();
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByCustomer(int customerId)
        {
            var sql = "SELECT * FROM Orders WHERE CustomerID = @CustomerId ORDER BY OrderDate DESC";
            var orders = await QueryAsync<OrderDTO>(sql, new { CustomerId = customerId });

            // Populate order details for each order
            foreach (var order in orders)
            {
                order.OrderDetails = (await _orderDetailRepository.GetOrderDetailsByOrderId(order.OrderID)).ToList();
            }

            return orders;
        }

        public async Task<int> UpdateOrderStatus(int orderId, string status)
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_OrderID", orderId, DbType.Int32);
            parameters.Add("p_OrderStatus", status, DbType.String);

            return await ExecuteAsync(
                "UpdateOrderStatus",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateOrder(OrderDTO order)
{
    var orderSql = @"
        UPDATE Orders
        SET CustomerID = @CustomerID,
            OrderDate = @OrderDate,
            TotalAmount = @TotalAmount,
            OrderStatus = @OrderStatus
        WHERE OrderID = @OrderID";

    var affected = await ExecuteAsync(orderSql, new
    {
        order.CustomerID,
        order.OrderDate,
        order.TotalAmount,
        OrderStatus = order.OrderStatus.ToString(),
        order.OrderID
    });

    // Возможность добавить логику по OrderDetails, если нужно

    return affected > 0 ? order.OrderID : 0;
}


    }
}

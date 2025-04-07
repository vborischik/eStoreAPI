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
    public class OrderDetailRepository : BaseDAL, IOrderDetailRepository
    {
        public OrderDetailRepository(IConfiguration configuration, string connectionName)
            : base(configuration, connectionName)
        {
        }

        public async Task<int> AddOrderDetail(OrderDetailDTO orderDetail)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@p_OrderID", orderDetail.OrderID);
            parameters.Add("@p_ProductID", orderDetail.ProductID);
            parameters.Add("@p_Quantity", orderDetail.Quantity);
            parameters.Add("@p_Price", orderDetail.Price);
            parameters.Add("p_OrderDetailID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await ExecuteAsync("AddOrderDetail", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("p_OrderDetailID");
        }

        public async Task<int> DeleteOrderDetail(int id)
        {
            var sql = "DELETE FROM OrderDetails WHERE OrderDetailID = @Id";
            return await ExecuteAsync(sql, new { Id = id });
        }

        public async Task<int> DeleteOrderDetailsByOrderId(int orderId)
        {
            var sql = "DELETE FROM OrderDetails WHERE OrderID = @OrderId";
            return await ExecuteAsync(sql, new { OrderId = orderId });
        }

        public async Task<OrderDetailDTO> GetOrderDetailById(int id)
        {
            var sql = @"SELECT od.*, p.ProductName, p.SKU 
                      FROM OrderDetails od
                      JOIN Products p ON od.ProductID = p.ProductID
                      WHERE od.OrderDetailID = @Id";
            return await QuerySingleAsync<OrderDetailDTO>(sql, new { Id = id }) ?? new OrderDetailDTO();
        }

        public async Task<IEnumerable<OrderDetailDTO>> GetOrderDetailsByOrderId(int orderId)
        {
            var sql = @"SELECT od.*, p.ProductName, p.SKU 
                      FROM OrderDetails od
                      JOIN Products p ON od.ProductID = p.ProductID
                      WHERE od.OrderID = @OrderId";
            return await QueryAsync<OrderDetailDTO>(sql, new { OrderId = orderId });
        }

        public async Task<int> UpdateOrderDetail(OrderDetailDTO orderDetail)
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_OrderDetailID", orderDetail.OrderDetailID, DbType.Int32);
            parameters.Add("p_ProductID", orderDetail.ProductID, DbType.Int32);
            parameters.Add("p_Quantity", orderDetail.Quantity, DbType.Int32);
            parameters.Add("p_Price", orderDetail.Price, DbType.Decimal);

            return await ExecuteAsync(
                "UpdateOrderDetail",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}

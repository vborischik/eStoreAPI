using Dapper;
using eStore.DAL.eStore.DAL;
using eStore.DAL.Models;
using eStore.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace eStore.DAL.Repositories
{

   public class OrderRepository : BaseDAL, IOrderRepository
    {
        public OrderRepository(IConfiguration configuration, string connectionName)
            : base(configuration, connectionName)
        {
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrders()
        {
            var sql = "SELECT * FROM Orders";
            return await QueryAsync<OrderDTO>(sql);
        }

        public async Task<OrderDTO> GetOrderById(int id)
        {
            var sql = "SELECT * FROM Orders WHERE OrderID = @Id";
            return await QuerySingleAsync<OrderDTO>(sql, new { Id = id });
        }

        public async Task<int> AddOrder(OrderDTO order)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@p_CustomerID", order.CustomerID);
            parameters.Add("@p_OrderDate", order.OrderDate);
            parameters.Add("@p_TotalAmount", order.TotalAmount);
            parameters.Add("@p_OrderStatus", order.OrderStatus);
            parameters.Add("@p_OrderID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await ExecuteAsync("AddOrder", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("p_OrderID");
        }

        public async Task<int> UpdateOrder(OrderDTO order)
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_OrderID", order.OrderID, DbType.Int32);
            parameters.Add("p_CustomerID", order.CustomerID, DbType.Int32);
            parameters.Add("p_OrderDate", order.OrderDate, DbType.DateTime);
            parameters.Add("p_TotalAmount", order.TotalAmount, DbType.Decimal);
            parameters.Add("p_OrderStatus", order.OrderStatus, DbType.String);

            return await ExecuteAsync("UpdateOrder", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DeleteOrder(int id)
        {
            var sql = "DELETE FROM Orders WHERE OrderID = @Id";
            return await ExecuteAsync(sql, new { Id = id });
        }
    }



}


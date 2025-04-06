using Dapper;
using eStore.DAL.eStore.DAL;
using eStore.DAL.Models;
using eStore.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace eStore.DAL.Repositories
{
public class CustomerRepository : BaseDAL, ICustomerRepository
{
    // No need for a separate _dbConnection field as BaseDAL handles connection creation.

    public CustomerRepository(IConfiguration configuration, string connectionName)
        : base(configuration, connectionName)
    {
    }

    public async Task<int> AddCustomer(CustomerDTO customer)
    {

        var parameters = new DynamicParameters();
        parameters.Add("@FirstName", customer.FirstName);
        parameters.Add("@LastName", customer.LastName);
        parameters.Add("@Email", customer.Email);
        parameters.Add("@Phone", customer.Phone);
        parameters.Add("p_CustomerID", dbType: DbType.Int32, direction: ParameterDirection.Output);


        await ExecuteAsync("AddCustomer", parameters, commandType: CommandType.StoredProcedure);

        return parameters.Get<int>("p_CustomerID");
    }


    public async Task<int> DeleteCustomer(int id)
    {
        var sql = "DELETE FROM Customers WHERE CustomerId = @Id";
        return await ExecuteAsync(sql, new { Id = id });
    }

    public async Task<IEnumerable<CustomerDTO>> GetAllCustomers()
    {
        var sql = "SELECT * FROM Customers";
        return await QueryAsync<CustomerDTO>(sql);
    }

    public async Task<IEnumerable<CustomerDTO>> GetAllCustomers(int pageNumber, int pageSize)
    {
        var sql = "SELECT * FROM Customers ORDER BY CustomerID LIMIT @PageSize OFFSET @Offset";
        return await QueryAsync<CustomerDTO>(sql, new { PageSize = pageSize, Offset = (pageNumber - 1) * pageSize });
    }

    public async Task<int> GetTotalCustomerCount()
    {
        var sql = "SELECT COUNT(*) FROM Customers";
        return await QuerySingleAsync<int>(sql);
    }

    public async Task<CustomerDTO> GetCustomerById(int id)
    {
        var sql = "SELECT * FROM Customers WHERE CustomerId = @Id";
        return await QuerySingleAsync<CustomerDTO>(sql, new { Id = id })?? new CustomerDTO();
    }

    public async Task<int> UpdateCustomer(CustomerDTO customer)
    {
       

        var parameters = new DynamicParameters();
        parameters.Add("p_CustomerID", customer.CustomerID, DbType.Int32);
        parameters.Add("p_FirstName", customer.FirstName, DbType.String);
        parameters.Add("p_LastName", customer.LastName, DbType.String);
        parameters.Add("p_Email", customer.Email, DbType.String);
        parameters.Add("p_Phone", customer.Phone, DbType.String);

        var t = await ExecuteAsync(
            "UpdateCustomer2",
            parameters,
            commandType: CommandType.StoredProcedure
        );


        return t;
    }



    public async Task<CustomerDTO> CheckCustomer(string email, string phone)
    {
        var sql = "SELECT * FROM Customers WHERE Email = @Email OR Phone = @Phone LIMIT 1;";
       
            return await QuerySingleAsync<CustomerDTO>(sql, new { Email = email,Phone=phone })??new CustomerDTO();       
    }


}
}
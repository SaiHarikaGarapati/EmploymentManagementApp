using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;

public class DbHelper
{
    private readonly string _connectionString;

    public DbHelper(IConfiguration configuration)
    {
        _connectionString = @"Server=localhost\SQLEXPRESS;Database=EmployeeDB;Trusted_Connection=True;TrustServerCertificate=True;";

    }

    public DataTable ExecuteQuery(string query)
    {
        using var con = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(query, con);
        using var adapter = new SqlDataAdapter(cmd);
        var dt = new DataTable();
        adapter.Fill(dt);
        return dt;
    }
}

using System.Data;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace IPB2.OnlineBookStoreSystem.WindowForm.Data;

public sealed class SqlDb
{
    private readonly string _connectionString;

    public SqlDb()
    {
        _connectionString = ConfigurationManager.ConnectionStrings["OnlineBookStoreDb"]?.ConnectionString
            ?? throw new InvalidOperationException("Missing OnlineBookStoreDb connection string in App.config.");
    }

    public DataTable Query(string sql, params SqlParameter[] parameters)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, conn);
        if (parameters.Length > 0)
        {
            cmd.Parameters.AddRange(parameters);
        }

        using var adapter = new SqlDataAdapter(cmd);
        var table = new DataTable();
        adapter.Fill(table);
        return table;
    }

    public int Execute(string sql, params SqlParameter[] parameters)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        if (parameters.Length > 0)
        {
            cmd.Parameters.AddRange(parameters);
        }

        return cmd.ExecuteNonQuery();
    }

    public object? Scalar(string sql, params SqlParameter[] parameters)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        if (parameters.Length > 0)
        {
            cmd.Parameters.AddRange(parameters);
        }

        return cmd.ExecuteScalar();
    }
}

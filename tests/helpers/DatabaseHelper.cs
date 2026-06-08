using Npgsql;
using System;
using System.Data;

namespace EcommerceTests.Helpers
{
    public class DatabaseHelper
    {
        private NpgsqlConnection _connection;

        public DatabaseHelper(string host, int port, string database, string username, string password)
        {
            var connectionString =
            $"Host=localhost;Port={port};Database={database};Username={username};Password={password};Pooling=false;SSL Mode=Disable";
            _connection = new NpgsqlConnection(connectionString);
        }

        public void Connect()
        {
            if (_connection.State != ConnectionState.Open)
            {
                Console.WriteLine(_connection.ConnectionString);
                _connection.Open();
            }
        }

        public void Disconnect()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        public Order GetOrderById(string orderId)
        {
            const string query = @"
                SELECT *
                FROM orders
                WHERE order_id = @orderId";

            using var command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("@orderId", orderId);

            using var reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            return new Order
            {
                OrderId = reader["order_id"].ToString(),
                CustomerEmail = reader["customer_email"].ToString(),
                OriginalAmount = Convert.ToDecimal(reader["original_amount"]),
                DiscountAmount = Convert.ToDecimal(reader["discount_amount"]),
                FinalAmount = Convert.ToDecimal(reader["final_amount"]),
                PromotionCode = reader["promotion_code"]?.ToString(),
                Status = reader["status"].ToString(),
                CreatedAt = Convert.ToDateTime(reader["created_at"])
            };
        }

        public AuditLog GetAuditLogByOrderId(string orderId)
        {
            const string query = @"
                SELECT *
                FROM promotion_audit_log
                WHERE order_id = @orderId";

            using var command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("@orderId", orderId);

            using var reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            return new AuditLog
            {
                AuditId = Convert.ToInt32(reader["audit_id"]),
                PromotionId = reader["promotion_id"].ToString(),
                OrderId = reader["order_id"].ToString(),
                DiscountApplied = Convert.ToDecimal(reader["discount_applied"]),
                UsedAt = Convert.ToDateTime(reader["used_at"])
            };
        }

        public void DeleteOrder(string orderId)
        {
            const string query = @"
                DELETE FROM orders
                WHERE order_id = @orderId";

            using var command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("@orderId", orderId);

            command.ExecuteNonQuery();
        }

        public bool VerifyOrderTotals(
            string orderId,
            decimal expectedOriginal,
            decimal expectedDiscount,
            decimal expectedFinal)
        {
            var order = GetOrderById(orderId);

            if (order == null)
                return false;

            return order.OriginalAmount == expectedOriginal
                   && order.DiscountAmount == expectedDiscount
                   && order.FinalAmount == expectedFinal;
        }
    }

    public class Order
    {
        public string OrderId { get; set; }
        public string CustomerEmail { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string PromotionCode { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class AuditLog
    {
        public int AuditId { get; set; }
        public string PromotionId { get; set; }
        public string OrderId { get; set; }
        public decimal DiscountApplied { get; set; }
        public DateTime UsedAt { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace Bitbucket.Api
{
    public class SqlRepository
    {
        private readonly string _connectionString;

        public SqlRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private void DeleteTable(string tableName)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand
                {
                    // eh... sql injection
                    CommandText = $"DELETE FROM {tableName}",
                    CommandType = CommandType.Text
                };

                cmd.Connection = conn;

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public void LoadTable(IEnumerable<Branch> rows)
        {
            var tableName = "[Branch]";

            DeleteTable(tableName);

            var dt = ConvertToDataTable(rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var sqlBulkCopy = new SqlBulkCopy(connection))
                {
                    sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                    sqlBulkCopy.ColumnMappings.Add("Author", "Author");
                    sqlBulkCopy.ColumnMappings.Add("LastModifiedDateTime", "LastModifiedDateTime");
                    sqlBulkCopy.ColumnMappings.Add("LastModifiedMessage", "LastModifiedMessage");

                    sqlBulkCopy.DestinationTableName = tableName;

                    sqlBulkCopy.WriteToServer(dt);
                }
            }
        }

        public void LoadTable(IEnumerable<Commit> rows)
        {
            // watch out, Commit is a sql keyword
            var tableName = "[Commit]";

            DeleteTable(tableName);

            var dt = ConvertToDataTable(rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var sqlBulkCopy = new SqlBulkCopy(connection))
                {
                    sqlBulkCopy.ColumnMappings.Add("Author", "Author");
                    sqlBulkCopy.ColumnMappings.Add("Date", "Date");
                    sqlBulkCopy.ColumnMappings.Add("Message", "Message");
                    sqlBulkCopy.ColumnMappings.Add("Hash", "Hash");
                    sqlBulkCopy.ColumnMappings.Add("Branch", "Branch");

                    sqlBulkCopy.DestinationTableName = tableName;

                    sqlBulkCopy.WriteToServer(dt);
                }
            }
        }

        public void LoadTable(IEnumerable<PullRequest> rows)
        {
            var tableName = "[PullRequest]";

            DeleteTable(tableName);

            var dt = ConvertToDataTable(rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var sqlBulkCopy = new SqlBulkCopy(connection))
                {
                    sqlBulkCopy.ColumnMappings.Add("Title", "Title");
                    sqlBulkCopy.ColumnMappings.Add("Description", "Description");
                    sqlBulkCopy.ColumnMappings.Add("Author", "Author");
                    sqlBulkCopy.ColumnMappings.Add("State", "State");
                    sqlBulkCopy.ColumnMappings.Add("LastUpdateDateTime", "LastUpdateDateTime");
                    sqlBulkCopy.ColumnMappings.Add("Branch", "Branch");

                    sqlBulkCopy.DestinationTableName = tableName;

                    sqlBulkCopy.WriteToServer(dt);
                }
            }
        }

        // refer: http://www.codeproject.com/Articles/424742/How-to-bulk-insert-efficiently
        private static DataTable ConvertToDataTable<T>(IEnumerable<T> list)
        {
            var propertyDescriptorCollection = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();

            for (var i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                var propertyDescriptor = propertyDescriptorCollection[i];
                var propType = propertyDescriptor.PropertyType;

                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    table.Columns.Add(propertyDescriptor.Name, Nullable.GetUnderlyingType(propType));
                }
                else
                {
                    table.Columns.Add(propertyDescriptor.Name, propType);
                }
            }

            var values = new object[propertyDescriptorCollection.Count];

            foreach (var listItem in list)
            {
                for (var i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(listItem);
                }

                table.Rows.Add(values);
            }

            return table;
        }
    }
}

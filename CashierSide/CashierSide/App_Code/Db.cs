using System;
using System.Configuration;
using System.Data;
using MySqlConnector;

namespace Azure
{
    public static class Db
    {
        private const string ConnectionName = "AzureCoffee";

        public static string ConnectionString
        {
            get
            {
                var setting = ConfigurationManager.ConnectionStrings[ConnectionName];
                if (setting == null)
                {
                    throw new ConfigurationErrorsException(
                        "Missing the \"" + ConnectionName + "\" connection string in Web.config.");
                }
                return setting.ConnectionString;
            }
        }

        public static MySqlConnection Open()
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        public static MySqlCommand Command(MySqlConnection connection, string sql)
        {
            var command = connection.CreateCommand();
            command.CommandText = sql;
            return command;
        }

        public static string Str(IDataRecord row, string column)
        {
            int i = row.GetOrdinal(column);
            return row.IsDBNull(i) ? null : row.GetString(i);
        }

        public static int Int(IDataRecord row, string column)
        {
            int i = row.GetOrdinal(column);
            return row.IsDBNull(i) ? 0 : Convert.ToInt32(row.GetValue(i));
        }

        public static decimal Dec(IDataRecord row, string column)
        {
            int i = row.GetOrdinal(column);
            return row.IsDBNull(i) ? 0m : Convert.ToDecimal(row.GetValue(i));
        }

        public static decimal? DecOrNull(IDataRecord row, string column)
        {
            int i = row.GetOrdinal(column);
            if (row.IsDBNull(i)) return null;
            return Convert.ToDecimal(row.GetValue(i));
        }

        public static bool Bool(IDataRecord row, string column)
        {
            int i = row.GetOrdinal(column);
            return !row.IsDBNull(i) && Convert.ToBoolean(row.GetValue(i));
        }

        public static DateTime Date(IDataRecord row, string column)
        {
            int i = row.GetOrdinal(column);
            return row.IsDBNull(i) ? DateTime.MinValue : Convert.ToDateTime(row.GetValue(i));
        }
    }
}

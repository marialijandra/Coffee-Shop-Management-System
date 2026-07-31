using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Azure
{
	public class Database
	{
		private readonly string connectionString =
			"server=localhost;port=3306;database=coffee_shop_db;uid=root;pwd=;";

		public MySqlConnection GetConnection()
		{
			return new MySqlConnection(connectionString);
		}
	}
}
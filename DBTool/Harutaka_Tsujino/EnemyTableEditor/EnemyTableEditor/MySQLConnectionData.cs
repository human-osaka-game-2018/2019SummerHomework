using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoLethalBlast_MySQL.Models.Services
{
	static class MySQLConnector
	{
		static MySQLConnector()
		{
			ReadConnectionStrings();
		}

		public static MySqlConnection Connect()
		{
			ConnectionData = new MySqlConnection(connectionString);

			ConnectionData.Open();

			return ConnectionData;
		}
		public static MySqlConnection ConnectionData { get; set; }

		public static void Close()
		{
			ConnectionData.Close();
		}

		public static void ReadConnectionStrings()
		{
			var key = ConfigurationManager.AppSettings["connectionKey"];

			connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
		}

		private static string connectionString;
	}
}

using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace MitrooPistV2.Data
{
    public static class ConnectionFactory
    {

        public static IDbConnection createConnection(string ConnectionString)
        {
            try
            {
                NpgsqlConnection DataConnection = new NpgsqlConnection(ConnectionString);
                return DataConnection;
            }
            catch (NpgsqlException ex)
            {
                return null;
            }
        }


        public static IDbConnection createConnection()
        {
            try
            {
                NpgsqlConnection DataConnection = new NpgsqlConnection();
                return DataConnection;
            }
            catch (NpgsqlException ex)
            {
                return null;
            }
        }
    }
}

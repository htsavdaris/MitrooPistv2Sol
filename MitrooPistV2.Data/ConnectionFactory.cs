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

            NpgsqlConnection DataConnection = new NpgsqlConnection(ConnectionString);
            return DataConnection;
        }
    }
}

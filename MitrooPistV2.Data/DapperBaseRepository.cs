﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Npgsql;
using NpgsqlTypes;
using Microsoft.Extensions.Logging;
namespace MitrooPistV2.Data
{
    public class DapperBaseRepository<TEntity> : IDisposable
            where TEntity : class

    {
        private string _connectionString;
        private IDbConnection _connection;
        public readonly ILogger _logger;
        protected readonly string _tableName;
        protected readonly string SqlSelectCommand;

        public string ConnectionString
        {
            get { return _connectionString; }
            protected set { _connectionString = value; }
        }

        public IDbConnection Connection
        {
            get { return _connection; }
            protected set { _connection = value; }
        }


        public DapperBaseRepository(string tableName, string ConnectionString, ILogger logger)
        {

            _tableName = tableName;
            SqlSelectCommand = "SELECT * FROM " + _tableName + " ";
            _logger = logger;
            Connection = ConnectionFactory.createConnection(ConnectionString);
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.PostgreSQL);
        }


        public DapperBaseRepository(string tableName, IDbConnection connection, ILogger logger)
        {
            _tableName = tableName;
            SqlSelectCommand = "SELECT * FROM " + _tableName + " ";
            _logger = logger;
            Connection = connection;
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.PostgreSQL);
        }
        public TEntity Get(long id)
        {
            _logger.LogTrace(1, "Get DB in BR is called");
            try
            {
                var obj = Connection.Get<TEntity>(id);
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, typeof(TEntity).FullName + " Get: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);                
                return null;
            }
        }

        public List<TEntity> GetAll()
        {
            try
            {
                var oList = Connection.GetList<TEntity>().AsList();
                return oList;
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogError(1, typeof(TEntity).FullName + " GetAll: NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }
        }

        public long? Insert(TEntity obj)
        {
            try
            {
                var identity = Connection.Insert<TEntity>(obj);
                if (identity != 0)
                    return identity;
                else
                    return 0;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, typeof(TEntity).FullName + " Insert: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return 0;
            }
        }

        public bool Update(TEntity obj)
        {
            try
            {
                var isSuccess = Connection.Update<TEntity>(obj);
                return (isSuccess > 0);
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, typeof(TEntity).FullName + " Update: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return false;
            }
        }

        public bool Delete(long id)
        {
            try
            {
                var isSuccess = Connection.Delete<TEntity>(id);
                return (isSuccess > 0);
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, typeof(TEntity).FullName + " Delete: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return false;
            }
        }


        public bool Delete(TEntity obj)
        {
            try
            {
                var isSuccess = Connection.Delete<TEntity>(obj);
                return (isSuccess > 0);
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, typeof(TEntity).FullName + " Delete: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return false;
            }
        }

        public void Dispose()
        {
            if (_connection != null)
                _connection.Close();

            _connection = null;
        }

    }
}

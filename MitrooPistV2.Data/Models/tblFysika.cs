using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;
using Npgsql;
using Microsoft.Extensions.Logging;

namespace MitrooPistV2.Data
{
    [Dapper.Contrib.Extensions.Table("tblfysika")]
    public class tblFysika
    {
        [Dapper.Contrib.Extensions.ExplicitKey]        
        public int fldam { get; set; }
        public string fldeponymo { get; set; }
        public string fldonoma { get; set; }
        public string fldpatronymo { get; set; }
        public string flddiefthinsi { get; set; }
        public string fldnomos { get; set; }
        public string fldemail { get; set; }
        public string fldtilefono { get; set; }
        public string fldcertification { get; set; }
        public string fldeidikotita { get; set; }
        public Boolean flda { get; set; }
        public Boolean fldb { get; set; }
        public Boolean fldc { get; set; }
        public Boolean fldd { get; set; }

    }


    public class tblFysikaDac : BaseDac
    {

        public const string SqlTableName = "tblfysika";
        public const string SqlSelectCommand = "SELECT * FROM " + SqlTableName + " ";
        private readonly ILogger _logger;

        public tblFysikaDac(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public tblFysikaDac(string ConnectionString, ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Connection = ConnectionFactory.createConnection(ConnectionString);
        }

        public tblFysikaDac(IDbConnection connection, ILogger<tblFysikaDac> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));//_logger = logger;
            Connection = connection;
        }

        public tblFysikaDac(IDbTransaction transaction, ILogger<tblFysikaDac> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Transaction = transaction;
            Connection = transaction.Connection;
        }

        public tblFysikaDac(BaseDac dapProvider, ILogger<tblFysikaDac> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
            Transaction = dapProvider.Transaction;
            Connection = dapProvider.Connection;
        }

        ~tblFysikaDac()
        {
            if (Connection != null)
                Connection.Close();
        }

        public tblFysika Get(long id)
        {
            try
            {
                var obj = Connection.Get<tblFysika>(id);
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }
        }

        public List<tblFysika> GetAll()
        {
            try
            {

                var oList = Connection.GetAll<tblFysika>().AsList();
                return oList;
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogError(1, "NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }
        }

        public tblFysika GetByEmail(string fldemail)
        {
            var obj = Connection.QueryFirstOrDefault<tblFysika>(SqlSelectCommand + " WHERE fldemail=@fldemail", new { fldemail = fldemail });
            return obj;
        }

        public long Insert(tblFysika obj)
        {
            try
            {
                var identity = Connection.Insert<tblFysika>(obj);
                if (identity == 0)
                    identity = obj.fldam;                
                return identity;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return 0;
            }
        }

        public bool Update(tblFysika obj)
        {
            try
            {
                var isSuccess = Connection.Update<tblFysika>(obj);
                return isSuccess;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return false;
            }
        }

        public bool Delete(tblFysika obj)
        {
            try
            {
                var isSuccess = Connection.Delete<tblFysika>(obj);
                return isSuccess;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return false;
            }
        }
    }
}

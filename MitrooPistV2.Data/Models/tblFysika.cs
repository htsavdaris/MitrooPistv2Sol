using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;
using Npgsql;


namespace MitrooPistV2.Data
{
    [Dapper.Contrib.Extensions.Table("tblfysika")]
    public class tblFysika
    {
        [Dapper.Contrib.Extensions.Key]        
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
        public Boolean flde { get; set; }
        public Boolean fldst { get; set; }
        

    }


    public class tblFysikaDac : BaseDac
    {

        public const string SqlTableName = "tblfysika";
        public const string SqlSelectCommand = "SELECT * FROM " + SqlTableName + " ";
        //private readonly ILogger _logger;

        public tblFysikaDac()
        {            
        }

        public tblFysikaDac(string ConnectionString)
        {
            //_logger = logger;
            Connection = ConnectionFactory.createConnection(ConnectionString);
            //_logger.LogTrace(1, ConnectionString);
        }

        public tblFysikaDac(IDbConnection connection)
        {
            //_logger = logger;
            Connection = connection;
        }

        public tblFysikaDac(IDbTransaction transaction)
        {
            Transaction = transaction;
            Connection = transaction.Connection;
        }

        public tblFysikaDac(BaseDac dapProvider)
        {
            Transaction = dapProvider.Transaction;
            Connection = dapProvider.Connection;
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
                //_logger.LogError(1, "List NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }
        }

        public tblFysika GetByEmail(string fldemail)
        {
            var obj = Connection.QueryFirstOrDefault<tblFysika>(SqlSelectCommand + " WHERE fldemail=@fldemail", new { fldemail = fldemail });
            return obj;
        }

        public long Insert(tblFysika crmUser)
        {
            try
            {
                var identity = Connection.Insert<tblFysika>(crmUser);
                return identity;
            }
            catch (NpgsqlException ex)
            {
                return 0;
            }
        }

        public bool Update(tblFysika crmUser)
        {
            try
            {
                var isSuccess = Connection.Update<tblFysika>(crmUser);
                return isSuccess;
            }
            catch (NpgsqlException ex)
            {
                return false;
            }
        }

        public bool Delete(tblFysika crmUser)
        {
            try
            {
                var isSuccess = Connection.Delete<tblFysika>(crmUser);
                return isSuccess;
            }
            catch (NpgsqlException ex)
            {
                return false;
            }
        }
    }
}

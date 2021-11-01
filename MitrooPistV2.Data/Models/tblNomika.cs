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
    [Dapper.Contrib.Extensions.Table("tblnomika")]
    public class tblNomika
    {
        [Dapper.Contrib.Extensions.Key]
        public int fldAM { get; set; }
        public string fldEponymia { get; set; }
        public string fldYpefthinos { get; set; }
        public string fldDieythynsi { get; set; }
        public string fldNomos { get; set; }
        public string fldTilefono { get; set; }
        public string fldEmail { get; set; }
        public Boolean fldA { get; set; }
        public Boolean fldB { get; set; }
        public Boolean fldC { get; set; }
        public Boolean fldD { get; set; }
    }

    public class tblNomikaDac : BaseDac
    {
        public const string SqlTableName = "tblnomika";
        public const string SqlSelectCommand = "SELECT * FROM " + SqlTableName + " ";

        public tblNomikaDac()
        {
        }

        public tblNomikaDac(string ConnectionString)
        {
            Connection = ConnectionFactory.createConnection(ConnectionString);
        }

        public tblNomikaDac(IDbConnection connection)
        {
            Connection = connection;
        }

        public tblNomikaDac(IDbTransaction transaction)
        {
            Transaction = transaction;
            Connection = transaction.Connection;
        }

        public tblNomikaDac(BaseDac dapProvider)
        {
            Transaction = dapProvider.Transaction;
            Connection = dapProvider.Connection;
        }

        public tblNomika Get(long id)
        {
            var obj = Connection.Get<tblNomika>(id);
            return obj;
        }

        public List<tblNomika> GetAll()
        {
            var oList = Connection.GetAll<tblNomika>().AsList();
            return oList;
        }

        public tblNomika GetByEmail(string email)
        {
            var obj = Connection.QueryFirstOrDefault<tblNomika>(SqlSelectCommand + " WHERE email=@email", new { email = email });
            return obj;
        }

       
        public long Insert(tblNomika crmUser)
        {
            var identity = Connection.Insert<tblNomika>(crmUser);
            return identity;
        }

        public bool Update(tblNomika crmUser)
        {
            var isSuccess = Connection.Update<tblNomika>(crmUser);
            return isSuccess;
        }

        public bool Delete(tblNomika crmUser)
        {
            var isSuccess = Connection.Delete<tblNomika>(crmUser);
            return isSuccess;
        }

    }
}

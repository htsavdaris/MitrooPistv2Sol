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
        public int fldam { get; set; }
        public string fldeponymia { get; set; }
        public string fldypefthinos { get; set; }
        public string flddieythynsi { get; set; }
        public string fldnomos { get; set; }
        public string fldtilefono { get; set; }
        public string fldemail { get; set; }
        public Boolean flda { get; set; }
        public Boolean fldb { get; set; }
        public Boolean fldc { get; set; }
        public Boolean fldd { get; set; }
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

        ~tblNomikaDac()
        {
            if (Connection != null)
                Connection.Close();
        }

        public tblNomika Get(long id)
        {
            try
            {
                var obj = Connection.Get<tblNomika>(id);
                return obj;
            }
            catch (NpgsqlException ex)
            {
                return null;
            }
        }

        public List<tblNomika> GetAll()
        {
            var oList = Connection.GetAll<tblNomika>().AsList();
            return oList;
        }

        public tblNomika GetByEmail(string email)
        {
            var obj = Connection.QueryFirst<tblNomika>(SqlSelectCommand + " WHERE email=@email", new { email = email });
            return obj;
        }

        public tblNomika GetByLogin(string login)
        {
            try
            {
                var obj = Connection.QueryFirst<tblNomika>(SqlSelectCommand + " WHERE login=@login", new { login = login });
                return obj;
            }
            catch (NpgsqlException ex)
            {
                return null;
            }
        }

        public long Insert(tblNomika crmUser)
        {
            try
            {
                var identity = Connection.Insert<tblNomika>(crmUser);
                return identity;
            }
            catch (NpgsqlException ex)
            {
                return 0;
            }
        }

        public bool Update(tblNomika crmUser)
        {
            try
            {
                var isSuccess = Connection.Update<tblNomika>(crmUser);
                return isSuccess;
            }
            catch (NpgsqlException ex)
            {
                return false;
            }
        }

        public bool Delete(tblNomika crmUser)
        {
            try
            {
                var isSuccess = Connection.Delete<tblNomika>(crmUser);
                return isSuccess;
            }
            catch (NpgsqlException ex)
            {
                return false;
            }
        }

    }
}

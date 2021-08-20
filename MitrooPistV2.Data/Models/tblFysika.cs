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

        public tblFysikaDac()
        {
        }

        public tblFysikaDac(string ConnectionString)
        {
            Connection = ConnectionFactory.createConnection(ConnectionString);
        }

        public tblFysikaDac(IDbConnection connection)
        {
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
            var obj = Connection.Get<tblFysika>(id);
            return obj;
        }

        public List<tblFysika> GetAll()
        {
            var oList = Connection.GetAll<tblFysika>().AsList();
            return oList;
        }

        public tblFysika GetByEmail(string fldemail)
        {
            var obj = Connection.QueryFirst<tblFysika>(SqlSelectCommand + " WHERE fldemail=@fldemail", new { fldemail = fldemail });
            return obj;
        }

        public long Insert(tblFysika crmUser)
        {
            var identity = Connection.Insert<tblFysika>(crmUser);
            return identity;
        }

        public bool Update(tblFysika crmUser)
        {
            var isSuccess = Connection.Update<tblFysika>(crmUser);
            return isSuccess;
        }

        public bool Delete(tblFysika crmUser)
        {
            var isSuccess = Connection.Delete<tblFysika>(crmUser);
            return isSuccess;
        }
    }
}

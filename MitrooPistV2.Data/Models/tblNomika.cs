using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Npgsql;
using Microsoft.Extensions.Logging;

namespace MitrooPistV2.Data
{
    [Table("tblnomika")]
    public class tblNomika
    {
        [Key, Required]
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

    public class tblNomikaDac : DapperBaseRepository<tblNomika>
    {
        private const string tableName = "tblnomika";


        public tblNomikaDac(string ConnectionString, ILogger logger): base(tableName, ConnectionString, logger)
        {

        }

        public tblNomikaDac(IDbConnection connection, ILogger logger): base(tableName, connection, logger)
        {

        }

        ~tblNomikaDac()
        {
            if (Connection != null)
                Connection.Close();
        }


    }
}

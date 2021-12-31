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
    [Table("tblfysika")]
    public class tblFysika
    {
        [Key, Required]
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


    public class tblFysikaDac : DapperBaseRepository<tblFysika>
    {
        private const string tableName = "tblfysika";
        

        public tblFysikaDac(string ConnectionString, ILogger logger):base(tableName, ConnectionString, logger)
        {
           
        }

        public tblFysikaDac(IDbConnection connection, ILogger<tblFysikaDac> logger) : base(tableName, connection, logger)
        {
           
        }
        ~tblFysikaDac()
        {
            if (Connection != null)
                Connection.Close();
        }
               
        public tblFysika GetByEmail(string fldemail)
        {
            var obj = Connection.QueryFirstOrDefault<tblFysika>(SqlSelectCommand + " WHERE fldemail=@fldemail", new { fldemail = fldemail });
            return obj;
        }

       
    }
}

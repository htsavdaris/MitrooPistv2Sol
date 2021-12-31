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
    [Table("tblUser")]
    public class tblUser
    {
        [Key, Required]
        public int flduserid { get; set; }
		public string fldname { get; set; }
		public string fldlogin { get; set; }
		public string fldpassword { get; set; }
	
	}


    public class tblUserDac : DapperBaseRepository<tblUser>
    {
        private const string tableName = "tbluser";

        public tblUserDac(string ConnectionString, ILogger logger) : base(tableName, ConnectionString, logger)
        {
            
        }

        public tblUserDac(IDbConnection connection, ILogger logger) : base(tableName, connection, logger)
        {

        }


        public tblUser GetByEmail(string email)
        {
            try
            {
                var obj = Connection.QueryFirst<tblUser>(SqlSelectCommand + " WHERE email=@email", new { email = email });
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "GetByEmail: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }
        }

        public tblUser GetByLogin(string fldlogin)
        {
            try
            {
                var obj = Connection.QueryFirstOrDefault<tblUser>(SqlSelectCommand + " WHERE fldlogin=@fldlogin", new { fldlogin = fldlogin });
                if (obj !=null)
                    return obj;
                else
                {                    
                    return null;
                }
            }
            catch (Npgsql.NpgsqlException ex)
            {
                _logger.LogError(1, "GetByLogin: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }
            
        }

        public tblUser Authenticate(string login, string providedpassword)
        {
            var user = GetByLogin(login);
            if (user != null)
            {
                //PasswordHasher<string> pw = new PasswordHasher<string>();
                //PasswordVerificationResult res = pw.VerifyHashedPassword(login, user.password, providedpassword);
                //if (res == PasswordVerificationResult.Success)
                if (user.fldpassword ==  providedpassword)
                {
                    return user;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        public bool Register(tblUser oUser)
        {
            if (oUser != null)
            {
                PasswordHasher<string> pw = new PasswordHasher<string>();
                string hashedpass = pw.HashPassword(oUser.fldlogin, oUser.fldpassword);
                oUser.fldpassword = hashedpass;
                try
                {
                    var identity = Connection.Insert<tblUser>(oUser);
                    return true;
                }
                catch (NpgsqlException ex)
                {
                    _logger.LogError(1, "Register: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                    return false;
                }
            }
            else
                throw new Exception("User in Registration is null");
        }

        public bool ChangePassword(string login, string oldpass, string newpass)
        {
            PasswordHasher<string> pw = new PasswordHasher<string>();
            tblUser user = Authenticate(login, oldpass);
            if (user != null)
            {
                string hashedpass = pw.HashPassword(user.fldlogin, newpass);
                user.fldpassword = hashedpass;
                int rows = Connection.Update<tblUser>(user);
                return (rows > 0);
            }
            else
            {
                return false;
            }
        }
    }

}

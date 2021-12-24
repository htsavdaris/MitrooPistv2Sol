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
    [Dapper.Contrib.Extensions.Table("tblUser")]
    public class tblUser
    {
        [Dapper.Contrib.Extensions.Key]
        public int flduserid { get; set; }
		public string fldname { get; set; }
		public string fldlogin { get; set; }
		public string fldpassword { get; set; }
	
	}


    public class tblUserDac : BaseDac
    {

        public const string SqlTableName = "tbluser";
        public const string SqlSelectCommand = "SELECT * FROM " + SqlTableName + " ";
        private readonly ILogger _logger;

        public tblUserDac(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public tblUserDac(string ConnectionString, ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Connection = ConnectionFactory.createConnection(ConnectionString);
        }

        public tblUserDac(IDbConnection connection, ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Connection = connection;
        }

        public tblUserDac(IDbTransaction transaction, ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Transaction = transaction;
            Connection = transaction.Connection;
        }

        public tblUserDac(BaseDac dapProvider)
        {
            Transaction = dapProvider.Transaction;
            Connection = dapProvider.Connection;
        }

        public tblUser Get(long id)
        {
            try
            {
                var obj = Connection.Get<tblUser>(id);
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }
        }

        public List<tblUser> GetAll()
        {
            try
            {
                var oList = Connection.GetAll<tblUser>().AsList();
                return oList;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }  
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
                _logger.LogError(1, "Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
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
                _logger.LogError(1, "Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }
            
        }

        public long Insert(tblUser oUser)
        {
            try
            {
                var obj = Connection.Insert<tblUser>(oUser);
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return 0;
            }
        }

        public bool Update(tblUser oUser)
        {
            try
            {
                var isSuccess = Connection.Update<tblUser>(oUser);
                return isSuccess;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return false;
            }
        }

        public bool Delete(tblUser crmUser)
        {
            try
            {
                var isSuccess = Connection.Delete<tblUser>(crmUser);
                return isSuccess;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return false;
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
                    _logger.LogError(1, "Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
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
                bool succ = Connection.Update<tblUser>(user);
                return succ;
            }
            else
            {
                return false;
            }
        }
    }

}

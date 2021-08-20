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
    [Dapper.Contrib.Extensions.Table("tblUser")]
    public class tblUser
    {
        [Dapper.Contrib.Extensions.Key]
        public int flduserid { get; set; }
		public string fldname { get; set; }
		public string fldlogin { get; set; }
		public string fldpassword { get; set; }
		

		//public crmUser(int userid_, string login_, string passwd_, string firstname_, string lastname_, string mobile_, string email_, bool is_active_, bool is_locked_, bool resetpass_, DateTime created_at_, DateTime updated_at_)
		//{
		//	this.userid = userid_;
		//	this.login = login_;
		//	this.passwd = passwd_;
		//	this.firstname = firstname_;
		//	this.lastname = lastname_;
		//	this.mobile = mobile_;
		//	this.email = email_;
		//	this.is_active = is_active_;
		//	this.is_locked = is_locked_;
		//	this.resetpass = resetpass_;
		//	this.created_at = created_at_;
		//	this.updated_at = updated_at_;
		//}
	}



    public class tblUserDac : BaseDac
    {

        public const string SqlTableName = "tbluser";
        public const string SqlSelectCommand = "SELECT * FROM " + SqlTableName + " ";

        public tblUserDac()
        {
        }

        public tblUserDac(string ConnectionString)
        {
            Connection = ConnectionFactory.createConnection(ConnectionString);
        }

        public tblUserDac(IDbConnection connection)
        {
            Connection = connection;
        }

        public tblUserDac(IDbTransaction transaction)
        {
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
            var obj = Connection.Get<tblUser>(id);
            return obj;
        }

        public List<tblUser> GetAll()
        {
            var oList = Connection.GetAll<tblUser>().AsList();
            return oList;
        }

        //public tblUser GetByEmail(string email)
        //{
        //    var obj = Connection.QueryFirst<tblUser>(SqlSelectCommand + " WHERE email=@email", new { email = email });
        //    return obj;
        //}

        public tblUser GetByLogin(string fldlogin)
        {
            var obj = Connection.QueryFirst<tblUser>(SqlSelectCommand + " WHERE fldlogin=@fldlogin", new { fldlogin = fldlogin });
            return obj;
        }

        public long Insert(tblUser oUser)
        {
            var obj = Connection.Insert<tblUser>(oUser);
            return obj;
        }

        public bool Update(tblUser oUser)
        {
            var isSuccess = Connection.Update<tblUser>(oUser);
            return isSuccess;
        }

        public bool Delete(tblUser crmUser)
        {
            var isSuccess = Connection.Delete<tblUser>(crmUser);
            return isSuccess;
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

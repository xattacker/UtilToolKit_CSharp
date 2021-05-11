using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using Xattacker.Database.Data;
using Xattacker.Utility.Except;

namespace Xattacker.Utility.Database
{
    public enum DBModuleType : short
    {
        UNKNOW = -1,

        MSSQL = 0,
        ORACLE = 1,
        MYSQL = 2
    }


    public class DbConnector : IDisposable
    {
        #region data member

	    private DbModule db;

        #endregion


        #region constructor and destructor

	    public DbConnector(DBModuleType type, string name)
	    {
            this.DBType = type;
	        this.Name = name;

	        this.Account = string.Empty;
	        this.Password = string.Empty;
	        this.Host = string.Empty;
            this.DBName = string.Empty;
            this.MaxPoolSize = 255;

            this.db = this.CreateDB(type);
	        this.IsConnected = false;
	    }

	    public DbConnector(DbConnector connector)
	    {
            this.DBType = connector.DBType;
	        this.Name = connector.Name;

	        this.Account = connector.Account;
	        this.Password = connector.Password;
	        this.Host = connector.Host;
            this.DBName = connector.DBName;
            this.MaxPoolSize = connector.MaxPoolSize;

            this.db = this.CreateDB(connector.DBType);
	        this.IsConnected = false;
	    }

        ~DbConnector()
        {
            this.db = null;
        }

        #endregion


        #region data member related function

	    public DBModuleType DBType { get; private set; }

	    public string Name { get; set; }

	    public string Account { get; set; }

	    public string Password { get; set; }

	    public string Host { get; set; }

	    public string DBName { get; set; }

        public int MaxPoolSize { get; set; }

	    public bool IsConnected { get; private set; }

        #endregion


        #region implement from IDisposable

        public void Dispose()
        {
            if (this.db != null)
            {
                try
                {
                    this.Disconnect();
                }
                finally
                {
                    this.db = null;
                }
            }
        }

        #endregion


        #region public function

        public bool Connect()
        {
            try
            {
                this.IsConnected = this.db.InitConnectBuilder
                                   (
                                   this.Host, 
                                   this.DBName, 
                                   this.Account, 
                                   this.Password,
                                   this.MaxPoolSize
                                   );
            }
            catch (Exception ex)
            {
                throw new CustomException(ErrorId.DATABASE_CONNECTION_FAILED, this.GetType(), ex);
            }

            return this.IsConnected;
        }

        public bool Disconnect()
        {
            bool result = this.db.ResetConnectBuilder();

            this.IsConnected = !result;

            return result;
        }

        public bool ExecCommand(string sql)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new CustomException(ErrorId.INVALID_PARARMETER, GetType());
            }

            if (!this.IsConnected)
            {
                throw new CustomException(ErrorId.UNCONNECTED, GetType());
            }

            return this.db.ExecCommand(sql);
        }

        public bool ExecCommand(string sql, List<DbSqlParameter> parameters)
        {
            return this.db.ExecCommand(sql, parameters);
        }

        public bool ExecCommands(List<string> sqls)
        {
            Exception ex = null;

            return this.ExecCommands(sqls, ref ex);
        }

        public bool ExecCommands(List<string> sqls, ref Exception exception)
        {
            if (sqls == null || sqls.Count == 0)
            {
                throw new CustomException(ErrorId.INVALID_PARARMETER, GetType());
            }

            return this.db.ExecCommands(sqls, ref exception);
        }

        public bool ExecCommands(List<DbSqlCommand> sqls)
        {
            Exception ex = null;

            return this.ExecCommands(sqls, ref ex);
        }

        public bool ExecCommands(List<DbSqlCommand> sqls, ref Exception exception)
        {
            if (sqls == null || sqls.Count == 0)
            {
                throw new CustomException(ErrorId.INVALID_PARARMETER, GetType());
            }

            return this.db.ExecCommands(sqls, ref exception);
        }

        public DataTable GetResult(string sql)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new CustomException(ErrorId.INVALID_PARARMETER, GetType());
            }

            if (!this.IsConnected)
            {
                throw new CustomException(ErrorId.UNCONNECTED, GetType());
            }

            return this.db.GetResult(sql);
        }

        public DataTable GetResult(DbSqlCommand xcmd, int timeout = 30)
        {
            return this.db.GetResult(xcmd, timeout);
        }

        public DataTable GetSchema(string name)
        {
            DataTable table = null;

            if (string.IsNullOrEmpty(name))
            {
                throw new CustomException(ErrorId.INVALID_PARARMETER, GetType());
            }

            if (!this.IsConnected)
            {
                throw new CustomException(ErrorId.UNCONNECTED, GetType());
            }


            using (DbConnection con = this.db.Connection)
            {
                try
                {
                    con.Open();
                    table = con.GetSchema(name);
                }
                catch (Exception ex)
                {
                    con.Close();

                    throw ex;
                }
            }

            return table;
        }

        public DataTable GetSchema(string name, string[] restrictions)
        {
            DataTable table = null;

            if (string.IsNullOrEmpty(name) || restrictions == null || restrictions.Length == 0)
            {
                throw new CustomException(ErrorId.INVALID_PARARMETER, GetType());
            }

            if (!this.IsConnected)
            {
                throw new CustomException(ErrorId.UNCONNECTED, GetType());
            }


            using (DbConnection con = this.db.Connection)
            {
                try
                {
                    con.Open();
                    table = con.GetSchema(name, restrictions);
                }
                catch (Exception ex)
                {
                    con.Close();

                    throw ex;
                }
            }

            return table;
        }

        #endregion


        #region private function

        private DbModule CreateDB(DBModuleType type)
	    {
	        DbModule db = null;
	   
	        switch (type)
	        {
	            case DBModuleType.MSSQL:
	                db = new MSSQLModule();
	                break;

	            case DBModuleType.ORACLE:
	                db = new OracleModule();
	                break;

                case DBModuleType.MYSQL:
                    db = new MySqlModule();
                    break;
	        }

	        return db;
	    }

        #endregion
    }
}

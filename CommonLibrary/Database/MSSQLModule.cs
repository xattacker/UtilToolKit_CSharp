using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

using Xattacker.Database.Data;

namespace Xattacker.Database
{
    public sealed class MSSQLModule : DbModule
    {
        #region data member

        private SqlConnectionStringBuilder builder;

        #endregion

        
        #region destructor

        ~MSSQLModule()
        {
            this.builder = null;
        }

        #endregion


        #region implement from IXDB

        public override bool InitConnectBuilder
        (
        string host, 
        string dbName, 
        string account, 
        string password, 
        int maxPoolSize
        )
        {
            this.builder = new SqlConnectionStringBuilder();
            this.builder.DataSource = host;
            this.builder.InitialCatalog = dbName;
            this.builder.UserID = account;
            this.builder.Password = password;

            return true;
        }

        public override bool ResetConnectBuilder()
        {
            this.builder = null;

            return true;
        }

        public override DbConnection Connection
        {
            get
            {
                return new SqlConnection(this.builder.ConnectionString);
            }
        }

        public override DbCommand Command
        {
            get
            {
                return new SqlCommand();
            }
        }

        protected override void HandleParameters(DbCommand cmd, string sql, List<DbSqlParameter> parameters, bool executed)
        {
            if (parameters != null && parameters.Count > 0)
            {
                StringBuilder builder = new StringBuilder(sql);
                DbSqlParameter parameter = null;
                SqlParameter sql_para = null;
                string PREFIX = "@";

                if (cmd.Parameters.Count > 0)
                {
                    cmd.Parameters.Clear();
                }

                for (int i = 0, size = parameters.Count; i < size; i++)
                {
                    parameter = parameters[i];
                    if (parameter != null && parameter.Name != null && parameter.Value != null)
                    {
                        sql_para = new SqlParameter(parameter.Name, this.ConvertType(parameter.Type));
                        sql_para.Value = parameter.Value;

                        if (!parameter.Name.StartsWith(PREFIX))
                        {
                            sql_para.ParameterName = PREFIX + parameter.Name;
                        }

                        cmd.Parameters.Add(sql_para);
                    }
                }

                cmd.CommandText = builder.ToString();
            }
            else
            {
                cmd.CommandText = sql;
            }

            if (executed)
            {
                cmd.ExecuteNonQuery();
            }
        }

        #endregion


        #region private function

        private SqlDbType ConvertType(DBParameterType type)
        {
            SqlDbType converted = SqlDbType.VarChar;

            switch (type)
            {
                case DBParameterType.BLOB:
                    converted = SqlDbType.Binary;
                    break;

                case DBParameterType.CLOB:
                case DBParameterType.TEXT:
                    converted = SqlDbType.Text;
                    break;

                case DBParameterType.CHAR:
                    converted = SqlDbType.Char;
                    break;

                case DBParameterType.VARCHAR:
                    converted = SqlDbType.VarChar;
                    break;


                case DBParameterType.NVARCHAR:
                    converted = SqlDbType.NVarChar;
                    break;

                case DBParameterType.DATETIME:
                    converted = SqlDbType.DateTime;
                    break;

                case DBParameterType.BOOLEAN:
                    converted = SqlDbType.Bit;
                    break;

                case DBParameterType.INTEGER:
                    converted = SqlDbType.Int;
                    break;

                case DBParameterType.SHORT:
                    converted = SqlDbType.SmallInt;
                    break;


                case DBParameterType.LONG:
                    converted = SqlDbType.BigInt;
                    break;

                case DBParameterType.FLOAT:
                    converted = SqlDbType.Float;
                    break;

                case DBParameterType.DOUBLE:
                    converted = SqlDbType.Real;
                    break;
            }

            return converted;
        }

        #endregion
    }
}

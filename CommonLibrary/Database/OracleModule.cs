using System.Collections.Generic;
using System.Data;
using System;
using System.Data.Common;
using System.Data.OracleClient;
using System.Text;

using Xattacker.Database.Data;

namespace Xattacker.Database
{
    public sealed class OracleModule : DbModule
    {
        #region data member

        private OracleConnectionStringBuilder builder;

        #endregion

              
        #region destructor

        ~OracleModule()
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
            this.builder = new OracleConnectionStringBuilder();
            this.builder.DataSource = host + "/" + dbName;
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
                return new OracleConnection(this.builder.ConnectionString);
            }
        }

        public override DbCommand Command 
        {
            get
            {
                return new OracleCommand();
            }
        }

        protected override void HandleParameters(DbCommand cmd, string sql, List<DbSqlParameter> parameters, bool executed)
        {
            if (parameters != null && parameters.Count > 0)
            {
                StringBuilder builder = new StringBuilder(sql);
                DbSqlParameter parameter = null;
                OracleParameter oracle_para = null;
                string PREFIX = ":";

                if (cmd.Parameters.Count > 0)
                {
                    cmd.Parameters.Clear();
                }

                for (int i = 0, size = parameters.Count; i < size; i++)
                {
                    parameter = parameters[i];
                    if (parameter != null && parameter.Name != null && parameter.Value != null)
                    {
                        oracle_para = new OracleParameter(parameter.Name, this.ConvertType(parameter.Type));
                        oracle_para.Value = parameter.Value;

                        if (!parameter.Name.StartsWith(PREFIX))
                        {
                            oracle_para.ParameterName = PREFIX + parameter.Name;
                        }

                        cmd.Parameters.Add(oracle_para);
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

        private OracleType ConvertType(DBParameterType type)
        { 
            OracleType converted = OracleType.VarChar;

            switch (type)
            { 
                case DBParameterType.BLOB:
                    converted = OracleType.Blob;
                    break;

                case DBParameterType.CLOB:
                case DBParameterType.TEXT:
                    converted = OracleType.Clob;
                    break;

                case DBParameterType.CHAR:
                    converted = OracleType.Char;
                    break;

                case DBParameterType.VARCHAR:
                    converted = OracleType.VarChar;
                    break;

                case DBParameterType.NVARCHAR:
                    converted = OracleType.NVarChar;
                    break;


                case DBParameterType.DATETIME:
                    converted = OracleType.DateTime;
                    break;

                case DBParameterType.INTEGER:
                    converted = OracleType.Int32;
                    break;

                case DBParameterType.SHORT:
                    converted = OracleType.Int16;
                    break;

                case DBParameterType.LONG:
                    converted = OracleType.Number;
                    break;

                case DBParameterType.FLOAT:
                    converted = OracleType.Float;
                    break;

                case DBParameterType.DOUBLE:
                    converted = OracleType.Double;
                    break;
            }

            return converted;
        }

        #endregion
    }
}

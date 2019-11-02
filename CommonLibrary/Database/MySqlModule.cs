using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

using MySql.Data;
using MySql.Data.MySqlClient;

using Xattacker.Database.Data;
using Xattacker.Utility.Except;

namespace Xattacker.Utility.Database
{
    public sealed class MySqlModule : DbModule
    {
        #region data member

        private MySqlConnectionStringBuilder builder;

        #endregion

               
        #region destructor

        ~MySqlModule()
        {
            this.builder = null;
        }

        #endregion


        #region implement from DbModule

        public override bool InitConnectBuilder
        (
        string host, 
        string dbName, 
        string account, 
        string password, 
        int maxPoolSize
        )
        {
            this.builder = new MySqlConnectionStringBuilder();
            this.builder.Server = host;
            this.builder.Database = dbName;
            this.builder.UserID = account;
            this.builder.Password = password;

            return true;
        }

        public override bool InitConnectBuilderByWinAuth
        (
        string host,
        string dbName,
        int maxPoolSize
        )
        {
            throw new CustomException(ErrorId.UNSUPPORTED, this.GetType());
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
                return new MySqlConnection(this.builder.ConnectionString);
            }
        }

        public override DbCommand Command
        {
            get
            {
                return new MySqlCommand();
            }
        }

        protected override void HandleParameters(DbCommand cmd, string sql, List<DbSqlParameter> parameters, bool executed)
        {
            if (parameters != null && parameters.Count > 0)
            {
                StringBuilder builder = new StringBuilder(sql);
                DbSqlParameter parameter = null;
                MySqlParameter mysql_para = null;
                string PREFIX = "?";

                if (cmd.Parameters.Count > 0)
                {
                    cmd.Parameters.Clear();
                }

                for (int i = 0, size = parameters.Count; i < size; i++)
                {
                    parameter = parameters[i];
                    if (parameter != null && parameter.Name != null && parameter.Value != null)
                    {
                        mysql_para = new MySqlParameter(parameter.Name, this.ConvertType(parameter.Type));
                        mysql_para.Value = parameter.Value;

                        if (!parameter.Name.StartsWith(PREFIX))
                        {
                            mysql_para.ParameterName = PREFIX + parameter.Name;
                        }

                        cmd.Parameters.Add(mysql_para);
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

        private MySqlDbType ConvertType(DBParameterType type)
        {
            MySqlDbType converted = MySqlDbType.VarChar;

            switch (type)
            {
                case DBParameterType.BLOB:
                    converted = MySqlDbType.Blob;
                    break;

                case DBParameterType.CLOB:
                case DBParameterType.TEXT:
                    converted = MySqlDbType.Text;
                    break;

                case DBParameterType.CHAR:
                    converted = MySqlDbType.String;
                    break;

                case DBParameterType.VARCHAR:
                    converted = MySqlDbType.VarChar;
                    break;


                case DBParameterType.NVARCHAR:
                    converted = MySqlDbType.VarChar;
                    break;

                case DBParameterType.DATETIME:
                    converted = MySqlDbType.DateTime;
                    break;

                case DBParameterType.BOOLEAN:
                    converted = MySqlDbType.Bit;
                    break;

                case DBParameterType.INTEGER:
                    converted = MySqlDbType.Int32;
                    break;

                case DBParameterType.SHORT:
                    converted = MySqlDbType.Int16;
                    break;


                case DBParameterType.LONG:
                    converted = MySqlDbType.Int64;
                    break;

                case DBParameterType.FLOAT:
                    converted = MySqlDbType.Float;
                    break;

                case DBParameterType.DOUBLE:
                    converted = MySqlDbType.Double;
                    break;
            }

            return converted;
        }

        #endregion
    }
}

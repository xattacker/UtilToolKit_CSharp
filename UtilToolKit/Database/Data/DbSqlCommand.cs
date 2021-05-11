using System;
using System.Collections.Generic;

using Xattacker.Utility.Except;

namespace Xattacker.Database.Data
{
    [Serializable]
    public class DbSqlCommand
    {
        #region constructor and destructor

        public DbSqlCommand()
        {
            this.Parameters = new List<DbSqlParameter>();
        }

        public DbSqlCommand(string sql)
        {
            this.Sql = sql;
            this.Parameters = new List<DbSqlParameter>();
        }

        public DbSqlCommand(string sql, List<DbSqlParameter> parameters)
        {
            this.Sql = sql;
            this.Parameters = parameters;
        }

        ~DbSqlCommand()
        {
            if (this.Parameters != null)
            {
                this.Parameters.Clear();
                this.Parameters = null;
            }
        }

        #endregion


        #region data member related function

        public string Sql { get; set; }
        public List<DbSqlParameter> Parameters { get; private set; }

        #endregion
    }
}

using System;

namespace Xattacker.Database.Data
{
    public enum DBParameterType
    { 
        CHAR,
        VARCHAR,
        NVARCHAR,
        BLOB,
        CLOB,

        TEXT,
        DATETIME,
        BOOLEAN,
        SHORT,
        INTEGER,

        LONG,
        FLOAT,
        DOUBLE,
    }
    

    [Serializable]
    public class DbSqlParameter
    {
        #region constructor and destructor

        public DbSqlParameter(DBParameterType type, string name)
        {
            this.Type = type;
            this.Name = name;
        }

        ~DbSqlParameter()
        {
            this.Value = null;
        }

        #endregion


        #region data member related function

        public DBParameterType Type { get; set; }

        public string Name { get; set; }

        public object Value { get; set; }

        #endregion
    }
}

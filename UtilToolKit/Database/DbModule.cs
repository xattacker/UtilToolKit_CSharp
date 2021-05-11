using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using Xattacker.Database.Data;

namespace Xattacker.Utility.Database
{
    public abstract class DbModule
    {
        #region public function

        public bool ExecCommand(string sql)
        {
            bool result = false;

            using (DbConnection con = this.Connection)
            {
                try
                {
                    con.Open();

                    using (DbCommand cmd = this.Command)
                    {
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        result = true;
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        con.Close();
                    }
                    catch
                    {
                    }

                    throw ex;
                }
                finally
                {
                    try
                    {
                        con.Close();
                    }
                    catch
                    {
                    }
                }
            }

            return result;
        }

        public bool ExecCommand(string sql, List<DbSqlParameter> parameters)
        {
            bool result = false;

            using (DbConnection con = this.Connection)
            {
                try
                {
                    con.Open();

                    using (DbCommand cmd = this.Command)
                    {
                        cmd.Connection = con;

                        this.HandleParameters(cmd, sql, parameters, true);
                        result = true;
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        con.Close();
                    }
                    catch
                    {
                    }

                    throw ex;
                }
                finally
                {
                    try
                    {
                        con.Close();
                    }
                    catch
                    {
                    }
                }
            }

            return result;
        }

        public bool ExecCommands(List<string> sqls, ref Exception exception)
        {
            bool result = false;

            using (DbConnection con = this.Connection)
            {
                try
                {
                    con.Open();

                    using (DbTransaction trans = con.BeginTransaction())
                    {
                        using (DbCommand cmd = this.Command)
                        {
                            cmd.Connection = con;
                            cmd.Transaction = trans;

                            try
                            {
                                string sql = null;

                                for (int i = 0, size = sqls.Count; i < size; i++)
                                {
                                    sql = sqls[i];
                                    if (!string.IsNullOrEmpty(sql))
                                    {
                                        cmd.CommandText = sql;
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                trans.Commit();
                                result = true;
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                exception = ex;
                                result = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        con.Close();
                    }
                    catch
                    {
                    }

                    exception = ex;
                    result = false;
                }
                finally
                {
                    try
                    {
                        con.Close();
                    }
                    catch
                    {
                    }
                }
            }

            return result;
        }

        public bool ExecCommands(List<DbSqlCommand> sqls, ref Exception exception)
        {
            bool result = false;

            using (DbConnection con = this.Connection)
            {
                try
                {
                    con.Open();

                    using (DbTransaction trans = con.BeginTransaction())
                    {
                        using (DbCommand cmd = this.Command)
                        {
                            cmd.Connection = con;
                            cmd.Transaction = trans;

                            try
                            {
                                DbSqlCommand comcmd = null;

                                for (int i = 0, size = sqls.Count; i < size; i++)
                                {
                                    comcmd = sqls[i];
                                    if (!string.IsNullOrEmpty(comcmd.Sql))
                                    {
                                        this.HandleParameters(cmd, comcmd.Sql, comcmd.Parameters, true);
                                    }
                                }

                                trans.Commit();
                                result = true;
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                exception = ex;
                                result = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        con.Close();
                    }
                    catch
                    {
                    }

                    exception = ex;
                    result = false;
                }
                finally
                {
                    try
                    {
                        con.Close();
                    }
                    catch
                    {
                    }
                }
            }

            return result;
        }

        public DataTable GetResult(string sql)
        {
            DataTable table = null;

            using (DbConnection con = this.Connection)
            {
                try
                {
                    con.Open();

                    //MySqlDataAdapter adapter = new MySqlDataAdapter(sql, this.connection);
                    //adapter.Fill(table);

                    using (DbCommand cmd = this.Command)
                    {
                        cmd.Connection = con;

                        DbSqlCommand comcmd = new DbSqlCommand();
                        comcmd.Sql = sql;
                        this.HandleParameters(cmd, comcmd.Sql, comcmd.Parameters, false);

                        table = new DataTable();

                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            table.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        con.Close();
                    }
                    catch
                    {
                    }

                    throw ex;
                }
                finally
                {
                    try
                    {
                        con.Close();
                    }
                    catch
                    {
                    }
                }
            }

            return table;
        }

        public DataTable GetResult(DbSqlCommand comcmd, int timeout)
        {
            DataTable table = null;

            using (DbConnection con = this.Connection)
            {
                try
                {
                    con.Open();

                    using (DbCommand cmd = this.Command)
                    {
                        cmd.Connection = con;
                        cmd.CommandTimeout = timeout;

                        this.HandleParameters(cmd, comcmd.Sql, comcmd.Parameters, false);

                        // MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        //adapter.Fill(table);

                        table = new DataTable();

                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            table.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        con.Close();
                    }
                    catch
                    {
                    }

                    throw ex;
                }
                finally
                {
                    try
                    {
                        con.Close();
                    }
                    catch
                    {
                    }
                }
            }

            return table;
        }

        #endregion


        #region abstract function

        public abstract bool InitConnectBuilder
        (
        string host,
        string dbName,
        string account,
        string password,
        int maxPoolSize
        );

        public abstract bool InitConnectBuilderByWinAuth
        (
        string host,
        string dbName,
        int maxPoolSize
        );

        public abstract bool ResetConnectBuilder();

        public abstract DbConnection Connection { get; }
        public abstract DbCommand Command { get; }

        protected abstract void HandleParameters(DbCommand cmd, string sql, List<DbSqlParameter> parameters, bool executed);

        #endregion
    }
}

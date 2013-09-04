using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Data.SqlTypes;

namespace PaymentGateWayAsia.DAL
{
    public enum DBConnections
    {
        PIS
    }

    public static class DBConnect
    {
        public static DataObject ExecuteReader(DBConnections objDBConnection, string StoredProcedure, Hashtable ht, bool IsInjectionSafe)
        {
            SqlDataReader dataresult = null;

            try
            {
                SqlConnection Connection = new SqlConnection(GetConnectionString(objDBConnection));
                SqlCommand Command = new SqlCommand();
                Command.Connection = Connection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = StoredProcedure;

                Command = AddParameters(Command, ht, IsInjectionSafe);

                Connection.Open();

                dataresult = Command.ExecuteReader(CommandBehavior.CloseConnection);

                DataObject dataResult = new DataObject(dataresult);

                return dataResult;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static DataObject ExecuteReader(string ConnectionString, string StoredProcedure, Hashtable ht, bool IsInjectionSafe)
        {
            SqlDataReader dataresult = null;

            try
            {
                SqlConnection Connection = new SqlConnection(ConnectionString);
                SqlCommand Command = new SqlCommand();
                Command.Connection = Connection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = StoredProcedure;

                Command = AddParameters(Command, ht, IsInjectionSafe);

                Connection.Open();

                dataresult = Command.ExecuteReader(CommandBehavior.CloseConnection);

                DataObject dataResult = new DataObject(dataresult);

                return dataResult;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static DataObject ExecuteReaderCommand(DBConnections objDBConnection, string StoredProcedure, Hashtable ht)
        {
            SqlDataReader dataresult = null;

            try
            {
                SqlConnection Connection = new SqlConnection(GetConnectionString(objDBConnection));
                SqlCommand Command = new SqlCommand();
                Command.Connection = Connection;
                Command.CommandType = CommandType.Text;
                Command.CommandText = StoredProcedure;

                Command = AddParameters(Command, ht, false);

                Connection.Open();

                dataresult = Command.ExecuteReader(CommandBehavior.CloseConnection);

                DataObject dataResult = new DataObject(dataresult);

                return dataResult;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static DataObject ExecuteReaderCommand(DBConnections objDBConnection, string CommandString)
        {
            SqlDataReader dataresult = null;

            try
            {
                SqlConnection Connection = new SqlConnection(GetConnectionString(objDBConnection));
                SqlCommand Command = new SqlCommand();
                Command.Connection = Connection;
                Command.CommandType = CommandType.Text;
                Command.CommandText = CommandString;

                Connection.Open();

                dataresult = Command.ExecuteReader(CommandBehavior.CloseConnection);

                DataObject dataResult = new DataObject(dataresult);

                return dataResult;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static DataObject ExecuteReaderCommand(string ConnectionString, string CommandString)
        {
            SqlDataReader dataresult = null;

            try
            {
                SqlConnection Connection = new SqlConnection(ConnectionString);
                SqlCommand Command = new SqlCommand();
                Command.Connection = Connection;
                Command.CommandType = CommandType.Text;
                Command.CommandText = CommandString;

                Connection.Open();

                dataresult = Command.ExecuteReader(CommandBehavior.CloseConnection);

                DataObject dataResult = new DataObject(dataresult);
               // Connection.Close();
                return dataResult;
            }
            catch (Exception e)
            {
              throw e;
            }
        }

        public static void ExecuteNonQuery(string ConnectionString, string CommandString)
        {
            SqlDataReader dataresult = null;
            try
            {
                SqlConnection Connection = new SqlConnection(ConnectionString);
                SqlCommand Command = new SqlCommand();
                Command.Connection = Connection;
                Command.CommandType = CommandType.Text;
                Command.CommandText = CommandString;

                Connection.Open();

                Command.ExecuteNonQuery();

               // DataObject dataResult = new DataObject(dataresult);
                 Connection.Close();
               // return dataResult;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public static Object ExecuteScalar(DBConnections objDBConnection, string StoredProcedure, Hashtable ht, bool IsInjectionSafe)
        {
            SqlConnection Connection = new SqlConnection(GetConnectionString(objDBConnection));
            SqlCommand Command = new SqlCommand();
            Command.Connection = Connection;
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = StoredProcedure;

            Command = AddParameters(Command, ht, IsInjectionSafe);

            Connection.Open();
            Object objObject = Command.ExecuteScalar();
            Connection.Close();

            return objObject;
        }

        public static void ExecuteNonQuery(DBConnections objDBConnection, string StoredProcedure, Hashtable ht, bool IsInjectionSafe)
        {
            SqlConnection Connection = new SqlConnection(GetConnectionString(objDBConnection));
            SqlCommand Command = new SqlCommand();
            Command.Connection = Connection;
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = StoredProcedure;

            Command = AddParameters(Command, ht, IsInjectionSafe);

            Connection.Open();

            Command.ExecuteNonQuery();
            Connection.Close();
        }

        public static void ExecuteNonQuery(string ConnectionString, string StoredProcedure, Hashtable ht, bool IsInjectionSafe)
        {
            SqlConnection Connection = new SqlConnection(ConnectionString);
            SqlCommand Command = new SqlCommand();
            Command.Connection = Connection;
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = StoredProcedure;

            Command = AddParameters(Command, ht, IsInjectionSafe);

            Connection.Open();

            Command.ExecuteNonQuery();
            Connection.Close();
        }

        #region "private methods"

        private static SqlCommand AddParameters(SqlCommand objCommand, Hashtable ht, bool IsInjectionSafe)
        {
            if (ht != null)
            {
                if (IsInjectionSafe == false)
                {
                    foreach (string s in ht.Keys)
                    {
                        string input = ht[s].ToString();
                        if (objCommand.CommandText == "Core_Errors_Add")
                        {
                            if (s.ToLowerInvariant() != "exception" && s.ToLowerInvariant() != "bloguserid" && s.ToLowerInvariant() != "created")
                            {
                                input = input.ToLowerInvariant().Replace("'", "&#39;").Replace("cast", "-CST-").Replace("exec", "-EKSEKVER-").Replace("execute", "-EKSEKVER-");
                            }

                            objCommand.Parameters.Add(new SqlParameter(s, input));
                        }
                        else
                        {
                            if (input.Contains("'") == true || input.ToLowerInvariant().Contains("cast") == true && input.ToLowerInvariant().Contains("exec") == true || input.ToLowerInvariant().Contains("execute") == true)
                            {
                                try
                                {
                                    objCommand.Parameters.Add(new SqlParameter(s, ht[s].ToString().Replace("'", "&#39;")));
                                }
                                catch (Exception) { }
                            }
                            else
                            {
                                objCommand.Parameters.Add(new SqlParameter(s, ht[s]));
                            }
                        }
                    }
                }
                else
                {
                    foreach (string s in ht.Keys)
                    {
                        objCommand.Parameters.Add(new SqlParameter(s, ht[s]));
                    }
                }
            }
            return objCommand;
        }

        private static string GetConnectionString(DBConnections ConnectionString)
        {
            string ConnStr = "";
            switch (ConnectionString)
            {
              
                case DBConnections.PIS:
                    ConnStr = "Data Source=cibdbuser.db.11702767.hostedresource.com;Initial Catalog=cibdbuser;Persist Security Info=True;User ID=cibdbuser; pwd=CIBdb123!;";             
                   break;               
                default:
                    break;
            }
            return ConnStr;
        }

        #endregion
    }
}
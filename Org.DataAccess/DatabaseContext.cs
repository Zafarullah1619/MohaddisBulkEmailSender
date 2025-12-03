using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.DataAccess
{

    public interface IDatabaseContextFactory
    {
        IDatabaseContext Context(string ConnectionStringName = "DefaultConnection");
    }

    public class DatabaseContextFactory : IDatabaseContextFactory
    {
        private IDatabaseContext dataContext;

        public IDatabaseContext Context(string ConnectionStringName = "DefaultConnection")
        {
            return dataContext ?? (dataContext = new DatabaseContext(ConnectionStringName));
        }


        public void Dispose()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }



    public interface IDatabaseContext
    {
        SqlConnection Connection { get; }
        SqlCommand Command { get; }
        SqlDataReader ExecuteProcedure(String commandText, params SqlParameter[] parameters);
        SqlDataReader ExecuteProcedure(String commandText);

        MySqlDataReader ExecuteMySqlProcedure(String commandText, params SqlParameter[] parameters);
        void Dispose();
    }

    public class DatabaseContext : IDatabaseContext
    {

        public DatabaseContext(string ConnectionStringName = "DefaultConnection")
        {
            try
            {
                string _connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
                Connection = new SqlConnection(_connectionString);

                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                    if (Connection.State == ConnectionState.Open)
                    {
                        //Org.Utils.Logger.LogMessage("Connection Open", "");
                        Command = new SqlCommand();
                        Command.Connection = Connection;
                        Command.CommandTimeout = 300;
                    }

                }
            }
            catch (Exception ex)
            {
                if (Connection != null)
                {
                    Connection.Close();
                }
                Org.Utils.Logger.LogRelativeMessage("DatabaseContext Exception:: " + ex.ToString(), true);
                throw ex;
            }
        }

        public DatabaseContext(string ConnectionStringName, string CustomConnection)
        {
            try
            {
                //string _connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
                Connection = new SqlConnection(ConnectionStringName);

                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                    if (Connection.State == ConnectionState.Open)
                    {
                        //Org.Utils.Logger.LogMessage("Connection Open", "");
                        Command = new SqlCommand();
                        Command.Connection = Connection;
                        Command.CommandTimeout = 300;
                    }

                }
            }
            catch (Exception ex)
            {
                if (Connection != null)
                {
                    Connection.Close();
                }
                Org.Utils.Logger.LogRelativeMessage("DatabaseContext Exception:: " + ex.ToString(), true);
                throw ex;
            }
        }

        public SqlConnection Connection { get; set; }
        public SqlCommand Command { get; set; }

        public MySqlConnection MySqlConnection { get; set; }
        public MySqlCommand mySqlCommand { get; set; }

        //

        public MySqlDataReader ExecuteMySqlProcedure(String commandText, params SqlParameter[] parameters)
        {
            try
            {

                if (MySqlConnection.State == ConnectionState.Open)
                {
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.CommandText = commandText;
                    mySqlCommand.Parameters.AddRange(parameters);
                    MySqlDataReader reader = mySqlCommand.ExecuteReader();
                    return reader;
                }
                else
                {
                    throw new Exception("Connection is not opened");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public SqlDataReader ExecuteProcedure(String commandText, params SqlParameter[] parameters)
        {
            try
            {

                if (Connection.State == ConnectionState.Open)
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.CommandText = commandText;
                    Command.Parameters.AddRange(parameters);
                    //Command.CommandText = "GetSampleData";
                    SqlDataReader reader = Command.ExecuteReader();
                    return reader;
                }
                else
                {

                    throw new Exception("Connection is not opened");

                }

                //SqlConnection sqlConnection1 = new SqlConnection("Data Source=BHATTI;Initial Catalog=General;User ID=sa;Password=sa@123");
                //SqlCommand cmd = new SqlCommand();
                //SqlDataReader reader;

                //cmd.CommandText = "GetSampleData";
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Connection = sqlConnection1;

                //sqlConnection1.Open();

                //reader = cmd.ExecuteReader();
                //// Data is accessible through the DataReader object here.

                //sqlConnection1.Close();

                //return reader;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public SqlDataReader ExecuteProcedure(String commandText)
        {
            try
            {

                if (Connection.State == ConnectionState.Open)
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.CommandText = commandText;
                    SqlDataReader reader = Command.ExecuteReader(CommandBehavior.CloseConnection);
                    return reader;
                }
                else
                {

                    throw new Exception("Connection is not opened");

                }


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Dispose()
        {
            if (Command != null)
            {
                Command.Dispose();
            }

            if (Connection != null)
            {
                //Org.Utils.Logger.LogMessage("Connection Close", "");
                Connection.Close();
            }
        }
    }

}

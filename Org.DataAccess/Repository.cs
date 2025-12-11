using MySql.Data.MySqlClient;
using Org.DataAccess.DataAccess;
using Org.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace Org.DataAccess
{

    public interface IRepository<T> where T : class
    {
        long Insert(T entity);
        int Update(T entity, Filters Filter);
        List<T> LoadSP(Filters Filter);
        List<T> LoadSP(string ProcedureName, Filters Filter);
        List<T> LoadMySqlSP(string ProcedureName, Filters Filter);
        bool ExecuteSP(Filters Filter);
        bool ExecuteSP(string ProcedureName, Filters Filter);
        T ExecuteMSSQLScalar(string connectionString, string sqlQuery);
        T ExecuteMYSQLScalar(string connectionString, string sqlQuery);
        List<Dictionary<string, object>> ExecuteMYSQLQuery(string connectionString, string sqlQuery);
        List<Dictionary<string, object>> ExecuteMSSQLQuery(string connectionString, string sqlQuery);
        List<string> GetMSSQLColumnNames(string connectionString, string sqlQuery);
        List<string> GetMySQLColumnNames(string connectionString, string sqlQuery);
        List<T> Load(T Entity);
        void Delete(Int64 id);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        IDatabaseContext _dbContext;
        public Repository(IDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public long Insert(T Entity)
        {

            Type type = Entity.GetType();
            QueryBuilder qBuilder = null;

            var props = type.GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(DataFieldAttribute)));

            var KeyFields = type.GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(KeyFieldAttribute)));

            var dataTable = (DataTableAttribute[])type.GetCustomAttributes(typeof(DataTableAttribute), true);

            if (dataTable.Count() == 0)
            {
                throw new Exception("Proper Database Table name should be mapped on " + type.Name);
            }

            if (qBuilder == null)
            {
                qBuilder = new QueryBuilder(dataTable[0].TableName);
            }


            foreach (var p in props)
            {

                var attr = (DataFieldAttribute)p.GetCustomAttribute(typeof(DataFieldAttribute), true);

                var value = p.GetValue(Entity, null);

                if (string.IsNullOrEmpty(Convert.ToString(value)) && attr.IsDBNull == false)
                {
                    throw new Exception("Property " + p.Name + " is not nullable");
                }
                if (!string.IsNullOrEmpty(Convert.ToString(value)))
                {
                    if (attr.Type == DbType.String)
                    {
                        value = WebUtility.HtmlEncode(value.ToString());
                    }
                    qBuilder.Add(p.Name, value);
                }
            }


            try
            {
                if (KeyFields.Count<PropertyInfo>() > 0)
                {
                    _dbContext.Command.CommandText = qBuilder.ToString() + ";SELECT SCOPE_IDENTITY();";
                    return Convert.ToInt64(_dbContext.Command.ExecuteScalar());
                }
                else
                {
                    _dbContext.Command.CommandText = qBuilder.ToString();
                    return _dbContext.Command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(T Entity, Filters Filter)
        {
            Type type = Entity.GetType();
            QueryBuilder qBuilder = null;

            var props = type.GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(DataFieldAttribute)));

            var dataTable = (DataTableAttribute[])type.GetCustomAttributes(typeof(DataTableAttribute), true);

            if (dataTable.Count() == 0)
            {
                throw new Exception("Proper Database Table name should be mapped on " + type.Name);
            }

            if (qBuilder == null)
            {
                qBuilder = new QueryBuilder(dataTable[0].TableName, Filter.ToString());
            }


            foreach (var p in props)
            {
                if(p.Name == "RecCreatedDt" || p.Name == "RecCreatedBy" || p.Name == "RecCreatedDte")
                {
                    continue;
                }
                var attr = (DataFieldAttribute)p.GetCustomAttribute(typeof(DataFieldAttribute), true);

                var value = p.GetValue(Entity, null);


                // Old Code Start *************************************************************************
                //if (string.IsNullOrEmpty(value.ToString()) && attr.IsDBNull == false)
                //{
                //    throw new Exception("Property " + p.Name + " is not nullable");
                //}

                //qBuilder.Add(p.Name, value);
                // Old Code End *************************************************************************

                if (string.IsNullOrEmpty(Convert.ToString(value)) && attr.IsDBNull == false)
                {
                    throw new Exception("Property " + p.Name + " is not nullable");
                }
                if (attr.Type == DbType.String)
                {
                    value = WebUtility.HtmlEncode(Convert.ToString(value));
                }

                qBuilder.Add(p.Name, value);

            }

            try
            {

                _dbContext.Command.CommandText = qBuilder.ToString();
                return _dbContext.Command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(Int64 Id)
        {


        }

        public List<T> LoadSP(Filters Filter)
        {

            try
            {
                Type type = typeof(T);

                List<T> ListOfType = new List<T>();

                var attr = (StoreProcedureAttribute)type.GetCustomAttribute(typeof(StoreProcedureAttribute), true);

                if (attr == null)
                {

                    throw new Exception("Procedue Name attribute must be mapped on " + type.Name);
                }

                var results = _dbContext.ExecuteProcedure(attr.ProcedureName, Filter.SqlParameters.ToArray());


                while (results.Read())
                {
                    dynamic o = Mapper.MappingFromReader(results, type);
                    ListOfType.Add(o);
                }

                return ListOfType;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        //
        public List<T> LoadMySqlSP(string ProcedureName, Filters Filter)
        {

            try
            {
                Type type = typeof(T);

                List<T> ListOfType = new List<T>();

                var results = _dbContext.ExecuteMySqlProcedure(ProcedureName, Filter.SqlParameters.ToArray());


                while (results.Read())
                {
                    dynamic o = Mapper.MappingFromReader(results, type);
                    ListOfType.Add(o);
                }

                return ListOfType;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        public List<T> LoadSP(string ProcedureName, Filters Filter)
        {

            try
            {
                Type type = typeof(T);

                List<T> ListOfType = new List<T>();

                var results = _dbContext.ExecuteProcedure(ProcedureName, Filter.SqlParameters.ToArray());


                while (results.Read())
                {
                    dynamic o = Mapper.MappingFromReader(results, type);
                    ListOfType.Add(o);
                }

                return ListOfType;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public bool ExecuteSP(Filters Filter)
        {

            try
            {
                Type type = typeof(T);

                List<T> ListOfType = new List<T>();

                var attr = (StoreProcedureAttribute)type.GetCustomAttribute(typeof(StoreProcedureAttribute), true);

                if (attr == null)
                {

                    throw new Exception("Procedue Name attribute must be mapped on " + type.Name);
                }

                var results = _dbContext.ExecuteProcedure(attr.ProcedureName, Filter.SqlParameters.ToArray());
                //if (results.RecordsAffected >= 0)
                //    return true;
                //else
                //    return false;

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public bool ExecuteSP(string ProcedureName, Filters Filter)
        {

            try
            {
                var results = _dbContext.ExecuteProcedure(ProcedureName, Filter.SqlParameters.ToArray());
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }


        public T ExecuteMSSQLScalar(string connectionString, string sqlQuery)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = sqlQuery;
                        object result = command.ExecuteScalar();
                        return (T)result;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and log errors
                Logger.LogRelativeMessage("Error executing scalar query: " + ex.Message, true);
                return default(T);
            }
        }

        public T ExecuteMYSQLScalar(string connectionString, string sqlQuery)
        {
            try
            {
                using (IDbConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = sqlQuery;
                        object result = command.ExecuteScalar();
                        return (T)result;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and log errors
                Logger.LogRelativeMessage("Error executing scalar query: " + ex.Message, true);
                return default(T);
            }
        }

        public List<Dictionary<string, object>> ExecuteMSSQLQuery(string connectionString, string sqlQuery)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, object> row = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetName(i);
                                    object columnValue;
                                    
                                    // Check if the value is null
                                    if (reader.IsDBNull(i))
                                    {
                                        columnValue = DBNull.Value;
                                    }
                                    else
                                    {
                                        // Get the field type
                                        Type fieldType = reader.GetFieldType(i);
                                        
                                        // For string types, use GetString() to ensure proper Unicode handling
                                        // This works correctly for both NVARCHAR (Unicode) and VARCHAR columns
                                        if (fieldType == typeof(string))
                                        {
                                            columnValue = reader.GetString(i);
                                        }
                                        else
                                        {
                                            columnValue = reader.GetValue(i);
                                        }
                                    }
                                    
                                    row[columnName] = columnValue;
                                }
                                results.Add(row);
                            }
                        }
                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and log errors
                Logger.LogRelativeMessage("Error executing query: " + ex.Message, true);
                return new List<Dictionary<string, object>>();
            }
        }

        public List<Dictionary<string, object>> ExecuteMYSQLQuery(string connectionString, string sqlQuery)
        {
            try
            {
                // Ensure connection string has utf8mb4 charset if not already present
                MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder(connectionString);
                // Add charset to connection string if not already present
                if (!connectionString.ToLower().Contains("charset"))
                {
                    csb["CharSet"] = "utf8mb4";
                }
                connectionString = csb.ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    
                    // Explicitly set the connection charset to utf8mb4 for Unicode support
                    using (MySqlCommand setCharsetCmd = new MySqlCommand("SET NAMES utf8mb4 COLLATE utf8mb4_unicode_ci", connection))
                    {
                        setCharsetCmd.ExecuteNonQuery();
                    }
                    
                    // Also set character set for the connection and session
                    using (MySqlCommand setSessionCmd = new MySqlCommand("SET CHARACTER SET utf8mb4", connection))
                    {
                        setSessionCmd.ExecuteNonQuery();
                    }

                    using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                    {
                        command.CommandTimeout = 300; // Set timeout if needed
                        
                        List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, object> row = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetName(i);
                                    object columnValue;
                                    
                                    // Check if the value is null
                                    if (reader.IsDBNull(i))
                                    {
                                        columnValue = DBNull.Value;
                                    }
                                    else
                                    {
                                        // Get the field type
                                        Type fieldType = reader.GetFieldType(i);
                                        
                                        // For string types, use GetString() to ensure proper Unicode handling
                                        if (fieldType == typeof(string))
                                        {
                                            try
                                            {
                                                // Use GetString() which should preserve Unicode
                                                columnValue = reader.GetString(i);
                                            }
                                            catch
                                            {
                                                // Fallback to GetValue if GetString fails
                                                columnValue = reader.GetValue(i);
                                            }
                                        }
                                        else if (fieldType == typeof(byte[]))
                                        {
                                            // If it's a byte array, decode as UTF-8
                                            byte[] bytes = (byte[])reader.GetValue(i);
                                            columnValue = Encoding.UTF8.GetString(bytes);
                                        }
                                        else
                                        {
                                            columnValue = reader.GetValue(i);
                                        }
                                    }
                                    
                                    row[columnName] = columnValue;
                                }
                                results.Add(row);
                            }
                        }
                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and log errors
                Logger.LogRelativeMessage("Error executing query: " + ex.Message, true);
                return new List<Dictionary<string, object>>();
            }
        }

        public List<string> GetMSSQLColumnNames(string connectionString, string sqlQuery)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = sqlQuery; // Wrap the original query to fetch the first row
                        using (IDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly))
                        {
                            DataTable schemaTable = reader.GetSchemaTable();
                            List<string> columnNames = schemaTable.Rows
                                .Cast<DataRow>()
                                .Select(row => row["ColumnName"].ToString())
                                .ToList();

                            return columnNames;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and log errors
                Logger.LogRelativeMessage("Error fetching column names: " + ex.Message, true);
                return new List<string>();
            }
        }


        public List<string> GetMySQLColumnNames(string connectionString, string sqlQuery)
        {
            try
            {
                using (IDbConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = sqlQuery; // Wrap the original query to fetch the first row
                        using (IDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly))
                        {
                            DataTable schemaTable = reader.GetSchemaTable();
                            List<string> columnNames = schemaTable.Rows
                                .Cast<DataRow>()
                                .Select(row => row["ColumnName"].ToString())
                                .ToList();

                            return columnNames;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and log errors
                Logger.LogRelativeMessage("Error fetching column names: " + ex.Message, true);
                return new List<string>();
            }
        }


        public List<T> Load(T Entity)
        {
            return null;
        }

    }

}










/* public List<T> ExecuteMYSQLScalar(string connectionString, string sqlQuery)
         {
             try
             {
                 using (IDbConnection connection = new MySqlConnection(connectionString))
                 {
                     connection.Open();

                     using (IDbCommand command = connection.CreateCommand())
                     {
                         command.CommandText = sqlQuery;
                         List<T> results = new List<T>();

                         using (IDataReader reader = command.ExecuteReader())
                         {
                             while (reader.Read())
                             {
                                 T result = (T)reader[0];
                                 results.Add(result);
                             }
                         }

                         return results;
                     }
                 }
             }
             catch (Exception ex)
             {
                 // Handle exceptions and log errors
                 Logger.LogRelativeMessage("Error executing query: " + ex.Message, true);
                 return new List<T>();
             }
         }*/












/*public T ExecuteScalar(string connectionString, string sqlQuery)
{
    try
    {
        using (IDbConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = sqlQuery;
                object result = command.ExecuteScalar();
                return (T)result;
            }
        }
    }
    catch (Exception ex)
    {
        // Handle exceptions and log errors
        // LogError("Error executing scalar query: " + ex.Message);
        return default(T);
    }
}*/
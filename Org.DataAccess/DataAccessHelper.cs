namespace Org.DataAccess.DataAccess
{
    //public class DataAccessHelper
    //{
    //    //private static string AppDatabaseName;
    //    private static string stConnName;
    //    private static string stProvider;
    //    private static Database oDb = null;
    //    public DataAccessHelper()
    //    {

    //    }

    //    private static void CreateDatabase()
    //    {
    //        try
    //        {
    //            if (oDb == null)
    //            {
    //                CommandTimeOut = 1800;
    //                stConnName = ConfigurationManager.AppSettings["MyConn"];
    //                stProvider = ConfigurationManager.ConnectionStrings[stConnName].ProviderName;
    //                oDb = DatabaseFactory.CreateDatabase(stConnName);
    //            }
    //        }

    //        catch (Exception ex)
    //        {
    //            throw new Exception("Database Connection Problem," + ex.Message);
    //        }
    //    }

    //    public static int CommandTimeOut { get; set; }

    //   /* public static List<object> Load(string SPName, List<SqlParameter> Param, SqlConnection Conn) {




    //        try
    //        {


    //            using (SqlCommand cmd = new SqlCommand(SPName, Conn))
    //            {

    //                cmd.CommandType = CommandType.StoredProcedure;

    //            }

    //        }
    //        catch (SqlException ex)
    //        {
    //            throw ex;
    //        }
    //        catch (Exception ex)
    //        {

    //            throw ex;
    //        }
    //        finally
    //        {
    //            if (internalOpen)
    //                Conn.Close();
    //        }

    //    }
    //    */

    //    public static List<object> LoadObject(object p_object, string p_sSqlString, Filters p_oFilter)
    //    {
    //        CreateDatabase();

    //        if (p_object == null)
    //            throw new Exception(" Object is not instantiated in Business Load Method");


    //        Type type = p_object.GetType();

    //        List<object> objectList = new List<object>();
    //        object obj = null;

    //        try
    //        {
    //            string stIfPagingQuery = p_oFilter.GetIfPagingQuery(p_sSqlString, stProvider);
    //            DbCommand oCmd = CreateSqlStringCommand(stIfPagingQuery);



    //            using (IDataReader dr = oDb.ExecuteReader(oCmd))
    //            {

    //                int nFieldsCount = dr.FieldCount < p_object.GetType().GetProperties().Length ? dr.FieldCount : p_object.GetType().GetProperties().Length;
    //                if (dr!=null)
    //                while (dr.Read())
    //                {
    //                    obj = Mapper.MappingFromReader(dr, type, nFieldsCount);
    //                    objectList.Add(obj);
    //                }
    //            }
    //            return objectList;
    //        }

    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //public static List<object> LoadObject(object p_object, Filters p_oFilter)
    //{

    //    if (p_object == null)
    //        throw new Exception(" Object is not instantiated in Business Load Method");

    //    CreateDatabase();

    //    Type type = p_object.GetType();


    //    PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

    //    DataTableAttribute[] dataTable = (DataTableAttribute[])type.GetCustomAttributes(typeof(DataTableAttribute), true);

    //    if (dataTable == null)
    //    {
    //        throw new Exception("Proper Database Table name should be mapped on " + type.Name);
    //    }

    //    StringBuilder sb = new StringBuilder("SELECT ");
    //    for (int i = 0; i < properties.Length; i++)
    //    {
    //        BaseFieldAttribute[] fields = (BaseFieldAttribute[])properties[i].GetCustomAttributes(typeof(BaseFieldAttribute), true);

    //        if (fields.Length > 0)
    //        {

    //            sb.Append(fields[0].ColumnName);
    //            sb.Append(" , ");

    //        }
    //    }
    //    // remove the last ','
    //    sb.Remove(sb.Length - 2, 2);

    //    sb.Append(" FROM ");

    //    sb.Append(!string.IsNullOrEmpty(dataTable[0].ViewName) ? dataTable[0].ViewName : dataTable[0].TableName);

    //    List<object> objectList = new List<object>();

    //    object obj = null;

    //    try
    //    {
    //        string stIfPagingQuery = p_oFilter.GetIfPagingQuery(sb.ToString());
    //        DbCommand oCmd = CreateSqlStringCommand(stIfPagingQuery);

    //        using (IDataReader dr = oDb.ExecuteReader(oCmd))
    //        {
    //            if (dr != null)
    //                while (dr.Read())
    //                {
    //                    obj = Mapper.MappingFromReader(dr, type, properties.Length);
    //                    objectList.Add(obj);
    //                }
    //        }

    //        return objectList;
    //    }

    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public static List<object> LoadObject(object p_object, Filters p_oFilter, int p_nNumberOfFields)
    //    {

    //        if (p_object == null)
    //            throw new Exception(" Object is not instantiated in Business Load Method");
    //        CreateDatabase();

    //        Type type = p_object.GetType();


    //        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

    //        DataTableAttribute[] dataTable = (DataTableAttribute[])type.GetCustomAttributes(typeof(DataTableAttribute), true);

    //        if (dataTable == null)
    //        {
    //            throw new Exception("Proper Database Table name should be mapped on " + type.Name);
    //        }

    //        StringBuilder sb = new StringBuilder("SELECT ");

    //        if (p_nNumberOfFields > properties.Length || p_nNumberOfFields == 0)
    //        {
    //            p_nNumberOfFields = properties.Length;
    //        }

    //        for (int i = 0; i < p_nNumberOfFields; i++)
    //        {
    //            BaseFieldAttribute[] fields = (BaseFieldAttribute[])properties[i].GetCustomAttributes(typeof(BaseFieldAttribute), true);

    //            if (fields.Length > 0)
    //            {

    //                sb.Append(fields[0].ColumnName);
    //                sb.Append(" , ");

    //            }
    //        }
    //        // remove the last ','
    //        sb.Remove(sb.Length - 2, 2);

    //        sb.Append(" FROM ");
    //        sb.Append(!string.IsNullOrEmpty(dataTable[0].ViewName) ? dataTable[0].ViewName : dataTable[0].TableName);


    //        List<object> objectList = new List<object>();

    //        object obj = null;

    //        try
    //        {

    //            string stIfPagingQuery = p_oFilter.GetIfPagingQuery(sb.ToString(), stProvider);
    //            DbCommand oCmd = CreateSqlStringCommand(stIfPagingQuery);
    //            using (IDataReader dr = oDb.ExecuteReader(oCmd))
    //            {
    //                if (dr!=null)
    //                while (dr.Read())
    //                {
    //                    obj = Mapper.MappingFromReader(dr, type, p_nNumberOfFields);
    //                    objectList.Add(obj);
    //                }
    //            }
    //            return objectList;
    //        }

    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public static List<object> LoadObject(object p_object, DbCommand p_oDbCommand)
    //    {

    //        if (p_object == null)
    //            throw new Exception(" Object is not instantiated in Business Load Method");

    //        CreateDatabase();


    //        Type type = p_object.GetType();

    //        if (p_oDbCommand == null)
    //            throw new Exception(" DbCommand cannot be null");


    //        List<object> objectList = new List<object>();

    //        object obj = null;

    //        try
    //        {

    //            using (IDataReader dr = oDb.ExecuteReader(p_oDbCommand))
    //            {
    //                int nFieldsCount = dr.FieldCount < p_object.GetType().GetProperties().Length ? dr.FieldCount : p_object.GetType().GetProperties().Length;

    //                if (dr!=null)
    //                while (dr.Read())
    //                {
    //                    obj = Mapper.MappingFromReader(dr, type, nFieldsCount);
    //                    objectList.Add(obj);
    //                }
    //            }
    //            return objectList;
    //        }

    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public static DataTable LoadDataTable(string p_sSqlString)
    //    {
    //        CreateDatabase();
    //        DataTable oDataTable = new DataTable("dt");


    //        try
    //        {
    //            DbCommand oCmd = CreateSqlStringCommand(p_sSqlString);

    //            using (IDataReader dr = oDb.ExecuteReader(oCmd))
    //            {
    //                oDataTable.Load(dr);

    //            }
    //            return oDataTable;
    //        }

    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }
    //    public static long LoadObjectCount(object p_object, Filters p_oFilter)
    //    {
    //        CreateDatabase();

    //        if (p_object == null)
    //            throw new Exception(" Object is not instantiated in Business Load Method");

    //        Type type = p_object.GetType();


    //        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

    //        DataTableAttribute[] dataTable = (DataTableAttribute[])type.GetCustomAttributes(typeof(DataTableAttribute), true);

    //        if (dataTable == null)
    //        {
    //            throw new Exception("Proper Database Table name should be mapped on " + type.Name);
    //        }


    //        StringBuilder sb = new StringBuilder("SELECT Count(*) ");


    //        sb.Append(" FROM ");
    //        sb.Append(!string.IsNullOrEmpty(dataTable[0].ViewName) ? dataTable[0].ViewName : dataTable[0].TableName);
    //        sb.Append(p_oFilter);
    //        List<object> objectList = new List<object>();
    //        long count = 0;

    //        try
    //        {

    //            using (IDataReader dr = oDb.ExecuteReader(CreateSqlStringCommand(sb.ToString())))
    //            {
    //                if(dr!=null)
    //                while (dr.Read())
    //                {
    //                    object oObj = dr[0];
    //                    count = Int64.Parse(oObj.ToString());
    //                }
    //            }
    //            return count;
    //        }

    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public static object LoadObjectMax<T>(object p_object, Expression<Func<T>> p_expPropertyName, Filters p_oFilter)
    //    {

    //        if (p_object == null)
    //            throw new Exception(" Object is not instantiated in Business Load Method");

    //        CreateDatabase();

    //        Type type = p_object.GetType();


    //        MemberInfo memberInfo = (p_expPropertyName.Body as MemberExpression).Member;

    //        System.Reflection.PropertyInfo p = p_object.GetType().GetProperty(memberInfo.Name);



    //        BaseFieldAttribute[] filed = (BaseFieldAttribute[])memberInfo.GetCustomAttributes(typeof(BaseFieldAttribute), false);

    //        if (filed.Length == 0)
    //        {
    //            throw new Exception("Invalid column " + memberInfo.Name);
    //        }

    //        DataTableAttribute[] dataTable = (DataTableAttribute[])type.GetCustomAttributes(typeof(DataTableAttribute), true);

    //        if (dataTable == null)
    //        {
    //            throw new Exception("Proper Database Table name should be mapped on " + type.Name);
    //        }


    //        StringBuilder sb = new StringBuilder("SELECT MAX(" + filed[0].ColumnName + ") ");


    //        sb.Append(" FROM ");
    //        sb.Append(!string.IsNullOrEmpty(dataTable[0].ViewName) ? dataTable[0].ViewName : dataTable[0].TableName);
    //        sb.Append(p_oFilter);
    //        List<object> objectList = new List<object>();
    //        object obj = null;


    //        try
    //        {
    //            using (IDataReader dr = oDb.ExecuteReader(CreateSqlStringCommand(sb.ToString())))
    //            {

    //                if (dr != null)
    //                {
    //                    while (dr.Read())
    //                    {
    //                        obj = dr[0];
    //                    }


    //                }
    //            }
    //            return  obj == null ? 0 : obj;
    //           // return string.IsNullOrEmpty(obj.ToString()) ? 0 : obj;
    //        }

    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }

    //    }

    //    public static object LoadAggregateValue(string p_sSqlString, Filters p_oFilter)
    //    {
    //        CreateDatabase();



    //        try
    //        {
    //            DbCommand oCmd = CreateSqlStringCommand(p_sSqlString + p_oFilter);
    //            object obj = 0;
    //            using (IDataReader dr = oDb.ExecuteReader(oCmd))
    //            {


    //                if (dr != null)
    //                {
    //                    while (dr.Read())
    //                    {
    //                        obj = dr[0];
    //                    }


    //                }
    //            }
    //             return string.IsNullOrEmpty(obj.ToString()) ? 0 : obj;
    //        }

    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public static bool SaveObject(object p_object, int p_nNumberOfFields)
    //    {

    //        if (p_object == null)
    //            throw new Exception(" Object is not instantiated in Business Save Method");

    //        CreateDatabase();

    //        Type type = p_object.GetType();

    //        QueryBuilder qBuilder = null;


    //        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

    //        DataTableAttribute[] dataTable = (DataTableAttribute[])type.GetCustomAttributes(typeof(DataTableAttribute), true);

    //        if (dataTable == null)
    //        {
    //            throw new Exception("Proper Database Table name should be mapped on " + type.Name);
    //        }

    //        if (qBuilder == null)
    //        {
    //            qBuilder = new QueryBuilder((!string.IsNullOrEmpty(dataTable[0].TableName) ? dataTable[0].TableName : dataTable[0].ViewName));
    //            qBuilder.ProviderName = stProvider;
    //        }

    //        if (p_nNumberOfFields > properties.Length || p_nNumberOfFields == 0)
    //        {
    //            p_nNumberOfFields = properties.Length;
    //        }

    //        for (int i = 0; i < p_nNumberOfFields; i++)
    //        {
    //            DataFieldAttribute[] fields = (DataFieldAttribute[])properties[i].GetCustomAttributes(typeof(DataFieldAttribute), true);

    //            if (fields.Length > 0)
    //            {
    //                object value = properties[i].GetValue(p_object, null);

    //                if (value != null && !value.Equals("") && fields[0].IsDBNull == false)
    //                {

    //                    qBuilder.Add(fields[0].ColumnName, value);
    //                }
    //                else if ((value == null || value.Equals("")) && fields[0].IsDBNull == true)
    //                {
    //                    qBuilder.Add(fields[0].ColumnName, null);
    //                }
    //                else if ((value != null && !value.Equals("")) && fields[0].IsDBNull == true)
    //                {
    //                    qBuilder.Add(fields[0].ColumnName, value);
    //                }

    //            }
    //        }
    //        try
    //        {
    //            DbCommand oCmd = CreateSqlStringCommand(qBuilder.ToString());
    //            bool isSaved = ExecuteNonQuery(oCmd);
    //            return isSaved;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public static bool SaveMDObject(object p_oMasterObject, int p_nMNumberOfFields, List<object> p_lDetailObject, int p_nDNumberOfFields)
    //    {

    //        using (var scope =
    //                new TransactionScope(TransactionScopeOption.RequiresNew))
    //        {


    //            bool isMSaved = SaveObject(p_oMasterObject, p_nMNumberOfFields);

    //            foreach (object o in p_lDetailObject)
    //            {

    //                SaveObject(o, p_nDNumberOfFields);

    //            }
    //            scope.Complete();


    //        }
    //        return true;
    //    }


    //    public static bool UpdateObject(object p_object, Filters p_oFilter, int p_nNumberOfFields)
    //    {

    //        if (p_object == null)
    //            throw new Exception(" Object is not instantiated in Business Update Method");
    //        CreateDatabase();


    //        Type type = p_object.GetType();

    //        QueryBuilder qBuilder = null;


    //        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

    //        DataTableAttribute[] dataTable = (DataTableAttribute[])type.GetCustomAttributes(typeof(DataTableAttribute), true);

    //        if (dataTable.Length == 0)
    //        {
    //            throw new Exception("A Valid Database Table name should be mapped on " + type.Name + " Class");
    //        }

    //        if (qBuilder == null)
    //        {
    //            qBuilder = new QueryBuilder
    //                ((!string.IsNullOrEmpty(dataTable[0].TableName) ? dataTable[0].TableName : dataTable[0].ViewName), 
    //                p_oFilter.ToString());
    //            qBuilder.ProviderName = stProvider;
    //        }

    //        if (p_nNumberOfFields > properties.Length || p_nNumberOfFields == 0)
    //        {
    //            p_nNumberOfFields = properties.Length;
    //        }

    //        for (int i = 0; i < p_nNumberOfFields; i++)
    //        {
    //            DataFieldAttribute[] fields = (DataFieldAttribute[])properties[i].GetCustomAttributes(typeof(DataFieldAttribute), true);

    //            if (fields.Length > 0)
    //            {
    //                object value = properties[i].GetValue(p_object, null);

    //                if (value != null && !value.Equals("") && fields[0].IsDBNull == false)
    //                {

    //                    qBuilder.Add(fields[0].ColumnName, value);
    //                }
    //                else if ((value == null || value.Equals("")) && fields[0].IsDBNull == true)
    //                {
    //                    qBuilder.Add(fields[0].ColumnName, null);
    //                }
    //                else if ((value != null && !value.Equals("")) && fields[0].IsDBNull == true)
    //                {
    //                    qBuilder.Add(fields[0].ColumnName, value);
    //                }

    //            }
    //        }
    //        try
    //        {
    //            bool isUpdated = ExecuteNonQuery(CreateSqlStringCommand(qBuilder.ToString()));
    //            return isUpdated;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }

    //    }
    //    public static bool DeleteObject(object p_object, Filters p_oFilter)
    //    {

    //        if (p_object == null)
    //            throw new Exception(" Object is not instantiated in Business Delete Method");

    //        CreateDatabase();

    //        Type type = p_object.GetType();

    //        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

    //        DataTableAttribute[] dataTable = (DataTableAttribute[])type.GetCustomAttributes(typeof(DataTableAttribute), true);

    //        if (dataTable.Length == 0)
    //        {
    //            throw new Exception("A Valid Database Table name should be mapped on " + type.Name + " Class");
    //        }

    //        String sqlString = "Delete from " + dataTable[0].TableName + p_oFilter.ToString();

    //        try
    //        {
    //            bool isDeleted = ExecuteNonQuery(CreateSqlStringCommand(sqlString));
    //            return isDeleted;
    //        }

    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public static bool DeleteObject(string p_stObjectName, Filters p_oFilter)
    //    {
    //        CreateDatabase();

    //        String sqlString = "Delete from " + p_stObjectName + p_oFilter.ToString();

    //        try
    //        {
    //            bool isDeleted = ExecuteNonQuery(CreateSqlStringCommand(sqlString));
    //            return isDeleted;
    //        }

    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }


    //    public static bool IsChildRecordExists<T>(object p_oTargetObj, Expression<Func<T>> p_eRefField, long p_nKeyFieldValue)
    //    {

    //        if (p_oTargetObj == null)
    //            throw new Exception(" Object is not instantiated in Business Method");

    //        CreateDatabase();
    //        Filters oFilter = new Filters();
    //        oFilter.AddParameters(p_eRefField, OperatorsList.Equal, p_nKeyFieldValue);
    //        return LoadObjectCount(p_oTargetObj, oFilter) == 0 ? false : true;

    //    }
    //    private static bool ExecuteNonQuery(DbCommand p_oDbCommand)
    //    {
    //        //CreateDatabase();
    //        try
    //        {
    //            oDb.ExecuteNonQuery(p_oDbCommand);

    //            return true;
    //        }
    //        catch (SqlException ex)
    //        {
    //            throw new Exception("There is some error while saving the record" + ex.Message);

    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public static void CreateInParameter(DbCommand oCmd, string p_pramName, DbType p_dbType, object p_pramValue)
    //    {
    //       CreateDatabase();
    //        try
    //        {
    //            oDb.AddInParameter(oCmd, p_pramName, p_dbType, p_pramValue);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    private static DbCommand CreateSqlStringCommand(string p_sSqlString)
    //    {
    //       // CreateDatabase();
    //        try
    //        {
    //            return oDb.GetSqlStringCommand(p_sSqlString);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public static DbCommand CreateStoredProcCommand(string p_spName)
    //    {
    //        CreateDatabase();
    //        try
    //        {
    //            return oDb.GetStoredProcCommand(p_spName);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public static DbCommand CreateStoredProcCommand(string p_spName, object[] p_spParams)
    //    {
    //        CreateDatabase();
    //        try
    //        {
    //            return oDb.GetStoredProcCommand(p_spName, p_spParams);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }
    //}
}


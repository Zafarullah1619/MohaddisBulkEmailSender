using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

using System.Configuration;
using System.Data.SqlClient;

namespace Org.Utils
{
    public enum OperatorsList
    {
        And,
        OR,
        Between,
        IN,
        NotIn,
        Equal,
        LessThan,
        LessThanEqualTo,
        GreaterThan,
        GreaterThanEqualTo,
        BeginsWith,
        DoesNotBeginWith,
        EndsWith,
        DoesNotEndWith,
        Like,
        NotLike,
        NotEqual,
        Is,
        IsNot
    }

    public class Filters
    {

        private ArrayList ColumnName;	//It contains the column name with the table name
        private ArrayList Operators;	//It contain the name of the operator
        private ArrayList Values;		//It contain the values
        private ArrayList OrderBy;		//It contain the name of the column on which he want to order by
        private ArrayList FilterVal;
        private long? m_olStartRowIndex;
        private long? m_olMaximumRecords;
        private string Expression;

        private List<SqlParameter> SqlParams;

        private const string SPACE = " ";
        //private const string "," = ",";
        private const string DOT = ".";


        //  private string m_szUniqueColName;
        // private long m_lUniqueColValue;


        public Filters()
        {
            ColumnName = new ArrayList();
            Operators = new ArrayList();
            Values = new ArrayList();
            OrderBy = new ArrayList();
            FilterVal = new ArrayList();
            SqlParams = new List<SqlParameter>();
            SqlParameters = SqlParams;

        }


    public static string AscendingOrder
        {
            get
            {
                return "ASC";
            }
        }
        public static string DescendingOrder
        {
            get
            {
                return "DESC";
            }
        }

        public  List<SqlParameter> SqlParameters { get; set; }
        public void AddColumnsInOrderBy<T>(Expression<Func<T>> p_expPropertyName, string p_stSortType)
        {

            MemberInfo memberInfo = GetPropertyInfo(p_expPropertyName);
            if (memberInfo == null)
                throw new Exception("Invalid Property");

                string columnName = memberInfo.Name;
                OrderBy.Add(columnName + SPACE + p_stSortType);
            
        }
        
        
        public void AddColumnsInOrderBy(string p_stSortExpression)
        {
            OrderBy.Add(p_stSortExpression); //Add in to the OrderBy clause
        }
        public void SetPagingOptions(long p_nStartingIndex, long p_nRowsPerPage)
        {
            m_olStartRowIndex = p_nStartingIndex;
            m_olMaximumRecords = p_nRowsPerPage;
        }

        public string GetIfPagingQuery(string p_stSqlQuery)
        {


            StringBuilder oSb = new StringBuilder();

            if (m_olMaximumRecords.HasValue && m_olStartRowIndex.HasValue)
            {
                
                        oSb.Append("Select * from (");
                        oSb.Append(p_stSqlQuery + this.WhereClause);
                        oSb.Insert(21, SPACE + "ROW_NUMBER() OVER (" + this.OrderByClause + ") AS ROWID,");
                        oSb.Append(")tab");
                        oSb.Append(SPACE + "Where ROWID>=" + m_olStartRowIndex + SPACE + "And" + SPACE + "ROWID<=" + m_olMaximumRecords);
                       
            }
            else
            {
                oSb.Append(p_stSqlQuery);
                oSb.Append(this.WhereClause);
                oSb.Append(this.OrderByClause);
            }

            return oSb.ToString();
        }


        public string OrderByClause
        {
            get
            {
                if (OrderBy.Count == 0)
                {
                    return string.Empty;
                }

                StringBuilder orderByClause = new StringBuilder();

                orderByClause.Append(SPACE + "ORDER BY" + SPACE);

                for (int orderByCounter = 0; orderByCounter < OrderBy.Count; orderByCounter++)
                {
                    orderByClause.Append(OrderBy[orderByCounter]);
                    if (orderByCounter < OrderBy.Count - 1)
                        orderByClause.Append(SPACE + "," + SPACE);
                }


                return orderByClause.ToString();
            }
        }
        /* This function adds a new parameter for search criteria
         * */

        public void AddSqlParameters<T>(Expression<Func<T>> propertyName, dynamic val) {

            MemberInfo memberInfo = GetPropertyInfo(propertyName);

            var attr = (BaseFieldAttribute)memberInfo.GetCustomAttribute(typeof(BaseFieldAttribute), true);

            if (attr != null)
            {
                SqlParameter p = new SqlParameter();
                p.ParameterName = "@"+memberInfo.Name;
                p.DbType = attr.Type;
             //   p.Size = attr.Size;
                p.Value = val;
                SqlParams.Add(p);
            }
            else {
                throw new Exception("Data Type Should be defined on property "+ memberInfo);
            }

        }

        public void AddSqlParameters<T>(Expression<Func<T>> propertyName, dynamic val, System.Data.ParameterDirection dir)
        {

            MemberInfo memberInfo = GetPropertyInfo(propertyName);

            var attr = (BaseFieldAttribute)memberInfo.GetCustomAttribute(typeof(BaseFieldAttribute), true);

            if (attr != null)
            {
                SqlParameter p = new SqlParameter();
                p.ParameterName = "@" + memberInfo.Name;
                p.DbType = attr.Type;
                p.Direction = dir;
                //   p.Size = attr.Size;
                p.Value = val;
                SqlParams.Add(p);
            }
            else {
                throw new Exception("Data Type Should be defined on property " + memberInfo);
            }

        }
        public void AddParameters<T>(Expression<Func<T>> propertyName, OperatorsList oOperator, ArrayList values)
        {
            //It append the name of the table with the column name

            MemberInfo memberInfo = GetPropertyInfo(propertyName);
          
            
                string columnName = memberInfo.Name;
                ColumnName.Add(columnName);
                Operators.Add(oOperator);
                Values.Add(values);
            

        }
        public void AddParameters<T>(Expression<Func<T>> propertyName, OperatorsList oOperator, dynamic val)
        {
            //It append the name of the table with the column name
            MemberInfo memberInfo = GetPropertyInfo(propertyName);

          
                string columnName = memberInfo.Name;
                ArrayList oValLst = new ArrayList();
                oValLst.Add(val);
                FilterVal.Add(val);
                ColumnName.Add(columnName);
                Operators.Add(oOperator);
                Values.Add(oValLst);
            
        }
        public void AddParameters<T>(T Entity, string p_stPropertyName, string p_stOperatorName, object val)
        {
            if (Entity == null)
                throw new Exception("Entity can't be null");

            OperatorsList op;
            var propertyInfo = Entity.GetType().GetProperty(p_stPropertyName);
           
            try
            {
                op = GetOperator(p_stOperatorName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

                
                ArrayList oValLst = new ArrayList();
                oValLst.Add(val);
                FilterVal.Add(val);
                ColumnName.Add(propertyInfo.Name);
                Operators.Add(op);
                Values.Add(oValLst);
           
        }
       
        public void AddParameters(string p_stExpression)
        {
            Expression = p_stExpression;
        }


        private static string ResolveOperator(OperatorsList Operator)
        {
            //string sReturnValue = string.Empty;

            switch (Operator)
            {
                case OperatorsList.And: return "AND";
                case OperatorsList.Between: return "BETWEEN";
                case OperatorsList.Equal: return "=";
                case OperatorsList.GreaterThan: return ">";
                case OperatorsList.GreaterThanEqualTo: return ">=";
                case OperatorsList.IN: return "IN";
                case OperatorsList.LessThan: return "<";
                case OperatorsList.LessThanEqualTo: return "<=";
                case OperatorsList.Like: return "LIKE";
                case OperatorsList.NotLike: return "NOT LIKE";
                case OperatorsList.NotIn: return "NOT IN";
                case OperatorsList.OR: return "OR";
                case OperatorsList.NotEqual: return "!=";
                case OperatorsList.Is: return "is";
                case OperatorsList.IsNot: return "is not";
                case OperatorsList.BeginsWith: return "LIKE";
                case OperatorsList.DoesNotBeginWith: return "NOT LIKE";
                case OperatorsList.EndsWith: return "LIKE";
                case OperatorsList.DoesNotEndWith: return "NOT LIKE";

            }

            return string.Empty;
        }


        public OperatorsList GetOperator(string p_stOpString)
        {
            //string sReturnValue = string.Empty;

            switch (p_stOpString)
            {
                case "eq": return OperatorsList.Equal;
                case "ne": return OperatorsList.NotEqual;
                case "cn": return OperatorsList.Like;
                case "lt": return OperatorsList.LessThan;
                case "gt": return OperatorsList.GreaterThan;
                case "le": return OperatorsList.LessThanEqualTo;
                case "ge": return OperatorsList.GreaterThanEqualTo;
                case "bw": return OperatorsList.BeginsWith;
                case "bn": return OperatorsList.DoesNotBeginWith;
                case "ew": return OperatorsList.EndsWith;
                case "en": return OperatorsList.DoesNotEndWith;
                case "nc": return OperatorsList.NotLike;
                case "in": return OperatorsList.IN;
                case "ni": return OperatorsList.NotIn;
            }

            throw new Exception("Specified Opertor not found");
        }

        private string GetWhereClause()
        {

            StringBuilder strClause = new StringBuilder();
            int noOfParameters = GetParametersCount();			//Gets the number of Parameter sent to the function


            if (!string.IsNullOrEmpty(Expression))
            {
                strClause.Append(Expression);
                strClause.Append(SPACE + "AND" + SPACE);
            }
            
            

            if (noOfParameters == 0 && !string.IsNullOrEmpty(Expression))
            {
               strClause.Remove(strClause.Length - 4, 4);
               return strClause.ToString().Equals("") ? "" : SPACE + "Where" + SPACE + strClause.ToString();
            }

            if (noOfParameters == 0)
            {
                return null;
            }
            

            for (int columnCounter = 0; columnCounter < ColumnName.Count; columnCounter++)
            {
                strClause.Append(SPACE + ColumnName[columnCounter] + SPACE);
                strClause.Append(ResolveOperator((OperatorsList)Operators[columnCounter]) + SPACE);


                if (((OperatorsList)Operators[columnCounter] == OperatorsList.Is || (OperatorsList)Operators[columnCounter] == OperatorsList.IsNot)
                    && ((ArrayList)Values[columnCounter])[0] == DBNull.Value)
                {
                    strClause.Append(SPACE + "null" + SPACE);
                }
                else
                {
                    if ((OperatorsList)Operators[columnCounter] == OperatorsList.IN || (OperatorsList)Operators[columnCounter] == OperatorsList.NotIn)
                        strClause.Append("(");

                    ArrayList columnValues = (ArrayList)Values[columnCounter];
                    for (int valueCounter = 0; valueCounter < columnValues.Count; valueCounter++)
                    {

                        if ((OperatorsList)Operators[columnCounter] == OperatorsList.Like
                            && !columnValues[valueCounter].ToString().Contains("%"))
                        {
                            columnValues[valueCounter] = "%" + columnValues[valueCounter] + "%";
                        }

                        if ((OperatorsList)Operators[columnCounter] == OperatorsList.NotLike
                           && !columnValues[valueCounter].ToString().Contains("%"))
                        {
                            columnValues[valueCounter] = "%" + columnValues[valueCounter] + "%";
                        }

                        if ((OperatorsList)Operators[columnCounter] == OperatorsList.BeginsWith
                          && !columnValues[valueCounter].ToString().Contains("%"))
                        {
                            columnValues[valueCounter] = columnValues[valueCounter] + "%";
                        }

                        if ((OperatorsList)Operators[columnCounter] == OperatorsList.EndsWith
                         && !columnValues[valueCounter].ToString().Contains("%"))
                        {
                            columnValues[valueCounter] = "%" + columnValues[valueCounter];
                        }

                        if ((OperatorsList)Operators[columnCounter] == OperatorsList.DoesNotBeginWith
                         && !columnValues[valueCounter].ToString().Contains("%"))
                        {
                            columnValues[valueCounter] = columnValues[valueCounter] + "%";
                        }

                        if ((OperatorsList)Operators[columnCounter] == OperatorsList.DoesNotEndWith
                         && !columnValues[valueCounter].ToString().Contains("%"))
                        {
                            columnValues[valueCounter] = "%" + columnValues[valueCounter];
                        }

                        if ((OperatorsList)Operators[columnCounter] == OperatorsList.Between && valueCounter != 0)
                            strClause.Append(SPACE + "AND" + SPACE);
                        else if (((OperatorsList)Operators[columnCounter] == OperatorsList.IN || (OperatorsList)Operators[columnCounter] == OperatorsList.NotIn) && valueCounter != 0)
                            strClause.Append(SPACE + "," + SPACE);

                        object columnValue = columnValues[valueCounter];
                        if (columnValue.GetType() == typeof(string) || columnValue.GetType() == typeof(DateTime) || columnValue.GetType() == typeof(Guid))
                            strClause.Append("'" + columnValue + "'");
                        else
                            strClause.Append(columnValue);

                    }

                    if ((OperatorsList)Operators[columnCounter] == OperatorsList.IN || (OperatorsList)Operators[columnCounter] == OperatorsList.NotIn)
                        strClause.Append(")");
                }
                strClause.Append(SPACE + "AND" + SPACE);
            }
            strClause.Remove(strClause.Length - 4, 4);
            return strClause.ToString().Equals("") ? "" : SPACE + "Where" + SPACE + strClause.ToString();
        }

        private int GetParametersCount()
        {
            int parameterCount = 0;
            for (int valueCounter = 0; valueCounter < Values.Count; valueCounter++)
            {
                ArrayList valueParameters = (ArrayList)Values[valueCounter];
                parameterCount += valueParameters.Count;
            }
            return parameterCount;
        }
        public override string ToString()
        {
            return WhereClause + SPACE + OrderByClause;
        }
        public string WhereClause
        {
            get { return GetWhereClause(); }
        }

        private MemberInfo GetPropertyInfo<T>(Expression<Func<T>> propertyExpression)
        {
            return (propertyExpression.Body as MemberExpression).Member;
        }


    }
}

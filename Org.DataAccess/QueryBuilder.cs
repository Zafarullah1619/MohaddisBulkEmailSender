using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Org.DataAccess.DataAccess
{
    internal enum QueryType { Insert, Update };

    internal struct QueryItem
    {
        public object value;
        public string name;

        public string GetSafeValue()
        {
            if (value != null)
            {
                if (value.GetType() == typeof(DateTime))
                {
                    return Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                if (value.GetType() == typeof(DateTimeOffset))
                {
                    return DateTimeOffset.Parse(value.ToString()).ToString("yyyy-MM-dd HH:mm:ss.fff zzz");
                }
                else
                {
                    return value.ToString();
                }
            }

            return "";
        }
    }

    public class QueryBuilder
    {
        ArrayList fields = new ArrayList();
        string tableName;
        QueryType queryType = QueryType.Insert;
        string clause;
        

        public QueryBuilder(string tableName)
        {
            this.tableName = tableName;
        }

        

        public QueryBuilder(string tableName, string clause)
        {
            queryType = QueryType.Update;
            this.tableName = tableName;
            this.clause = clause;
        }

       
        public bool IsInserting
        {
            get { return queryType == QueryType.Insert; }
        }

        public void Add(string field, object value)
        {
            QueryItem item;
            item.value = value;
            item.name = field;

            fields.Add(item);
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (IsInserting)
            {
                sb.Append("INSERT INTO ");
                sb.Append(tableName);
                sb.Append(" (");

                StringBuilder sbFields = new StringBuilder();
                StringBuilder sbValues = new StringBuilder();

                foreach (QueryItem item in fields)
                {

                    sbFields.Append(item.name);
                    sbFields.Append(", ");

                    if (item.value == null) {

                        sbValues.Append("null");
                        sbValues.Append(", ");
                        continue;
                    }

                    if (item.value.GetType() == typeof(DateTime) || item.value.GetType() == typeof(DateTimeOffset))
                    {
                        
                            sbValues.Append("'");
                            sbValues.Append(item.GetSafeValue());
                            sbValues.Append("', ");
                        
                    }

                    else if (item.value.GetType() == typeof(Boolean))
                    {
                        sbValues.Append(((bool)item.value) ? 1 : 0);
                        sbValues.Append(", ");
                    }

                    else if (item.value.GetType() == typeof(string) || item.value.GetType() == typeof(char) ||
                        item.value.GetType() == typeof(Guid))
                    {
                        sbValues.Append("'");
                        sbValues.Append(item.GetSafeValue());
                        sbValues.Append("', ");
                    }
                    else
                    {
                        sbValues.Append(item.GetSafeValue());
                        sbValues.Append(", ");
                    }
                }

                sbFields.Remove(sbFields.Length - 2, 2);
                sbValues.Remove(sbValues.Length - 2, 2);

                sb.Append(sbFields.ToString());

                sb.Append(") VALUES (");
                sb.Append(sbValues.ToString());
                sb.Append(")");
            }
            else
            {
                sb.Append("UPDATE ");
                sb.Append(tableName);
                sb.Append(" SET ");

                foreach (QueryItem item in fields)
                {

                    sb.Append(item.name);
                    sb.Append(" = ");

                    if (item.value == null)
                    {

                        sb.Append("null");
                        sb.Append(", ");
                        continue;
                    }


                    if (item.value.GetType() == typeof(DateTime) || item.value.GetType() == typeof(DateTimeOffset))
                    {
                        
                            sb.Append("'");
                            sb.Append(item.GetSafeValue());
                            sb.Append("', ");
                        
                    }
                    else if (item.value.GetType() == typeof(Boolean))
                    {
                        sb.Append(((bool)item.value) ? 1 : 0);
                        sb.Append(", ");
                    }
                    else if (item.value.GetType() == typeof(string) || item.value.GetType() == typeof(char) || item.value.GetType() == typeof(Guid))
                    {
                        sb.Append("'");
                        sb.Append(item.GetSafeValue());
                        sb.Append("', ");
                    }
                    else
                    {
                        sb.Append(item.GetSafeValue());
                        sb.Append(", ");
                    }
                }

                sb.Remove(sb.Length - 2, 2);
                sb.Append(clause);
            }


            return sb.ToString();
        }
    }
 }


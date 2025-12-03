using System;
using System.Linq;
using System.Data;
using Org.Utils;
using System.Net;

namespace Org.DataAccess.DataAccess
{
    public class Mapper
    {
        public static object MappingFromReader(IDataReader reader, Type Entity)
        {

            object instance = Activator.CreateInstance(Entity, true);
            string TypeName = null;


            var props = Entity.GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(DataFieldAttribute)) || Attribute.IsDefined(prop, typeof(KeyFieldAttribute)) || Attribute.IsDefined(prop, typeof(ReadOnlyFieldAttribute)));
            ////using (TextWriter output = File.AppendText(System.Web.Hosting.HostingEnvironment.MapPath("~/ProcLog.txt")))
            //{
            //    Org.Utils.Logger.LogRelativeMessage(( "Count ::" + props.Count()));
            //}

            try
            {
                ////using (TextWriter output = File.AppendText(System.Web.Hosting.HostingEnvironment.MapPath("~/ProcLog.txt")))
                //{
                //    Org.Utils.Logger.LogRelativeMessage(( " Field Count ::" + reader.FieldCount));
                //}
                //for (int i = 0; i < reader.FieldCount; i++)
                //{
                //    //using (TextWriter output = File.AppendText(System.Web.Hosting.HostingEnvironment.MapPath("~/ProcLog.txt")))
                //    {
                //        Org.Utils.Logger.LogRelativeMessage(( Convert.ToString(i) + " Field Name ::" + reader.GetName(i) + " Value :: " + reader.GetValue(i)));
                //    }
                //}
                object[] values = new object[reader.FieldCount];
                reader.GetValues(values);

                //foreach(var value in values)
                //{
                //    //using (TextWriter output = File.AppendText(System.Web.Hosting.HostingEnvironment.MapPath("~/ProcLog.txt")))
                //    {
                //        Org.Utils.Logger.LogRelativeMessage(( " Value from Array ::" + Convert.ToString(value)));
                //    }
                //}

                foreach (var p in props)
                {
                    
                    object value = reader[p.Name];
                    ////using (TextWriter output = File.AppendText(System.Web.Hosting.HostingEnvironment.MapPath("~/ProcLog.txt")))
                    //{
                    //    Org.Utils.Logger.LogRelativeMessage(( "value ::" + value));
                    //    Org.Utils.Logger.LogRelativeMessage(( "Name ::" + p.Name));
                    //}

                    value = value.GetType()
                     == typeof(DBNull) ? null : value;

                    TypeName = p.PropertyType.Name;

                    switch (TypeName)
                    {
                        case "Boolean":

                            value = Convert.ToBoolean(value);
                            break;
                        case "Int64":

                            value = Convert.ToInt64(value);
                            break;
                        case "UInt64":

                            value = Convert.ToUInt64(value);
                            break;
                        case "Int32":
                            value = Convert.ToInt32(value);
                            break;

                        case "UInt32":
                            value = Convert.ToUInt32(value);
                            break;

                        case "Int16":
                            value = Convert.ToInt16(value);
                            break;
                        case "UInt16":
                            value = Convert.ToInt16(value);
                            break;

                        case "Single":
                            value = Convert.ToSingle(value);
                            break;

                        case "Double":
                            value = Convert.ToDouble(value);
                            break;
                        case "SByte":
                            value = Convert.ToSByte(value);
                            break;
                        case "Byte":
                            value = Convert.ToByte(value);
                            break;
                            //case "String":
                            //    value = WebUtility.HtmlDecode(value.ToString());
                            //    break;
                    }
                    if (TypeName == "String")
                    {
                        value = WebUtility.HtmlDecode(Convert.ToString(value));
                    }
                    p.SetValue(instance, value, null);


                }
            }
            catch (Exception ex)
            {
                throw new Exception("No database column exists against given property " + ex.Message);
            }
            return instance;
        }
    }
}

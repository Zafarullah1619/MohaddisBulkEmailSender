using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Org.Utils
{
    public class BaseFieldAttribute:Attribute
    {
        
        public BaseFieldAttribute()
        {
            
            Type = DbType.String;
            Size = 0;
           
        }
        
        public DbType Type { get; set; }
        public int Size { get; set; }

    }


    [AttributeUsage(AttributeTargets.Property)]
    public class DataFieldAttribute : BaseFieldAttribute
    {


        public DataFieldAttribute()
            : base()
        {
            IsDBNull = true;
        }

        public bool IsDBNull { get; set; }

    };

    [AttributeUsage(AttributeTargets.Property)]
    public class NonDBFieldAttribute : BaseFieldAttribute
    {


        public NonDBFieldAttribute()
            : base()
        {

        }


    };

    [AttributeUsage(AttributeTargets.Property)]
    public class ReadOnlyFieldAttribute : BaseFieldAttribute
    {


        public ReadOnlyFieldAttribute()
            : base()
        {

        }

        public bool IsDBNull { get; set; }
    };

    [AttributeUsage(AttributeTargets.Property)]
    public class KeyFieldAttribute : BaseFieldAttribute
    {
        public KeyFieldAttribute()
            : base()
        {

        }
    };

    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignKeyFieldAttribute : BaseFieldAttribute
    {
        public ForeignKeyFieldAttribute()
            : base()
        {

        }

        public bool IsDBNull { get; set; }
    };

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class DataTableAttribute : Attribute
    {
        public DataTableAttribute(string TabName) {

            TableName = TabName;
        }
        public string TableName{get;set;}

    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class StoreProcedureAttribute : Attribute
    {

    public  StoreProcedureAttribute (string ProcName) {

            ProcedureName = ProcName;
        }
        
        
        public string ProcedureName { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Business.Objects
{
    public class JsonModel
    {
        public string HtmlString { get; set; }
        public bool IsLastPage { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}

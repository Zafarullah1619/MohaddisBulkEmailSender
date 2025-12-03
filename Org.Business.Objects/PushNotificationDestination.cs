using Org.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Management.Instrumentation;

namespace Org.Business.Objects
{
    public class PushNotificationDestination : BaseEntity
    {
        [DataField(Type = DbType.String)]
        public string UserId { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool IsEnabled { get; set; }

        [DataField(Type = DbType.Boolean)]
        public bool IsDefault { get; set; }

        [DataField(Type = DbType.String)]
        public string FromName { get; set; }

        [DataField(Type = DbType.String)]
        public string DestinationAddress { get; set; }

        [DataField(Type = DbType.String)]
        public string DestinationType { get; set; }
    
        [DataField(Type = DbType.String)]
        public string DestinationSettings { get; set; }

        [DataField(Type = DbType.String)]
        public string RecCreatedBy { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public Nullable<System.DateTimeOffset> RecCreatedDt { get; set; }

        [DataField(Type = DbType.String)]
        public string RecUpdatedBy { get; set; }

        [DataField(Type = DbType.DateTimeOffset)]
        public Nullable<System.DateTimeOffset> RecUpdatedDt { get; set; }
    }
}

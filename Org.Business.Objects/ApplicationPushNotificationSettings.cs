using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Org.Business.Objects
{
    public interface IPushNotificationDeliveryProvider
    {
        string DestinationType { get; }

        IDeliveryProviderConfiguration Configuration { get; set; }

        Task<object> GenerateMessageAsync(PushNotificationItem notificationItem, CancellationToken ct);

        Task SendReadyMessageAsync(PushNotificationItem notificationItem, int RetryMax, int RetryIntv, CancellationToken ct);
    }

    [StoreProcedure("sp_GetApplicationPushNotificationSettings")]
    public class ApplicationPushNotificationSetting
    {

        public ApplicationPushNotificationSetting()
        {
            ApplicationName = "ASE";
            IsEnabled = false;
            DeliveryIntervalMinutes = 2;
            AntiNoiseSettings = new AntiNoiseSetting();
            RetryAttempts = 5;
            RetryIntervalMinutes = 2;
        }

        [DataField(Type = DbType.String)]
        public string ApplicationName { get; set; }

        [DataField(Type = DbType.String)]
        public string PushNotificationSettingsJson { get; set; }

        [JsonIgnore]
        [NonDBField]
        public string Serialized
        {
            get { return JsonConvert.SerializeObject(this); }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                var jsettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
                var jData = JsonConvert.DeserializeObject<ApplicationPushNotificationSetting>(value, jsettings);
                IsEnabled = jData.IsEnabled;
                DeliveryIntervalMinutes = jData.DeliveryIntervalMinutes;
                RetryAttempts = jData.RetryAttempts;
                RetryIntervalMinutes = jData.RetryIntervalMinutes;
                DeliveryProviderSettings = jData.DeliveryProviderSettings;
                AntiNoiseSettings = jData.AntiNoiseSettings;
            }
        }

        [NonDBField]
        public ICollection<PushNotificationDeliveryProviderSetting> DeliveryProviderSettings { get; set; }


        [NonDBField]
        private static ICollection<IPushNotificationDeliveryProvider> _deliveryProviders { get; set; }

        public static IPushNotificationDeliveryProvider CreateDeliveryProviderInstance(PushNotificationDeliveryProviderSetting settings)
        {
            var provType = Type.GetType(settings.ProviderAssemblyQualifiedName);
            if (provType != null)
            {
                var ci = provType.GetConstructor(new[] { typeof(JObject) });
                if (ci != null)
                {
                    return (IPushNotificationDeliveryProvider)ci.Invoke(new object[] { settings.ProviderConfigurationData });
                }
            }
            return null;
        }

        [JsonIgnore]
        [NonDBField]
        public ICollection<IPushNotificationDeliveryProvider> DeliveryProviders
        {
            get
            {
                if (DeliveryProviderSettings == null || _deliveryProviders == null)
                {
                    _deliveryProviders = new List<IPushNotificationDeliveryProvider>();
                    var providerConfigs = DeliveryProviderSettings;
                    foreach (var prov in DeliveryProviderSettings)
                    {
                        if (prov.IsEnabled)
                        {
                            _deliveryProviders.Add(CreateDeliveryProviderInstance(prov));
                        }
                    }
                }
                return _deliveryProviders;
            }
            set { _deliveryProviders = value; }
        }

        [NonDBField(Type = DbType.Boolean)]
        public bool IsEnabled { get; set; }

        [NonDBField(Type = DbType.Int32)]
        public int DeliveryIntervalMinutes { get; set; }

        [NonDBField(Type = DbType.Int32)]
        public int RetryAttempts { get; set; }

        [NonDBField(Type = DbType.Int32)]
        public int RetryIntervalMinutes { get; set; }

        [NonDBField]
        public AntiNoiseSetting AntiNoiseSettings { get; set; }
    }

    public class PushNotificationDeliveryProviderSetting
    {
        public PushNotificationDeliveryProviderSetting()
        {
            IsEnabled = false;
        }

        [NonDBField(Type = DbType.String)]
        public string ProviderAssemblyQualifiedName { get; set; }

        [NonDBField(Type = DbType.Boolean)]
        public bool IsEnabled { get; set; }

        [NonDBField]
        public JObject ProviderConfigurationData { get; set; }

        public static PushNotificationDeliveryProviderSetting FromProvider(IPushNotificationDeliveryProvider provider)
        {
            return new PushNotificationDeliveryProviderSetting()
            {
                IsEnabled = false,
                ProviderAssemblyQualifiedName = provider.GetType().AssemblyQualifiedName,
                ProviderConfigurationData = JObject.FromObject(provider.Configuration)
            };
        }
    }

    public class AntiNoiseSetting
    {
        public AntiNoiseSetting()
        {
            IsConsolidationEnabled = true;
            InitialConsolidationDelayMinutes = 6;
            MaxConsolidationDelayMinutes = 16;
            ExcludeSubscriberEvents = true;
        }

        [NonDBField]
        public bool IsConsolidationEnabled { get; set; }

        [NonDBField]
        public int InitialConsolidationDelayMinutes { get; set; }

        [NonDBField]
        public int MaxConsolidationDelayMinutes { get; set; }

        [NonDBField]
        public bool ExcludeSubscriberEvents { get; set; }
    }
}

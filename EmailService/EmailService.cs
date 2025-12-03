using System.Diagnostics;
using System.ServiceProcess;
using System.Configuration;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace EmailService
{
    public partial class AfterSalesEmailService : ServiceBase
    {
        private Timer stateTimer;
        private TimerCallback timerDelegate;
        private int DueTime = 0;
        private int Period = 0;
        private int LoggingTimeInterval = 0;
        private string message = null;
        private int PerDayLimit = 0;
        private int PerHourLimit = 0;
        private int BadgeLimit = 0;
        private int BadgeInterval = 0;

        public AfterSalesEmailService()
        {
            InitializeComponent();

            eventLogger = new EventLog();
            if (!EventLog.SourceExists("EmailCampaignBuilderSource"))
            {
                EventLog.CreateEventSource("EmailCampaignBuilderSource", "EmailCampaignBuilderLog");
            }
            eventLogger.Source = "EmailCampaignBuilderSource";
            eventLogger.Log = "EmailCampaignBuilderLog";

            DueTime = Convert.ToInt32(ConfigurationManager.AppSettings["DueTime"]);
            Period = Convert.ToInt32(ConfigurationManager.AppSettings["Period"]);
            PerDayLimit = Convert.ToInt32(ConfigurationManager.AppSettings["PerDayLimit"]);
            PerHourLimit = Convert.ToInt32(ConfigurationManager.AppSettings["PerHourLimit"]);
            BadgeLimit = Convert.ToInt32(ConfigurationManager.AppSettings["BadgeLimit"]);
            BadgeInterval = Convert.ToInt32(ConfigurationManager.AppSettings["BadgeInterval"]);
            LoggingTimeInterval = Convert.ToInt32(ConfigurationManager.AppSettings["LoggingTimeInterval"]);
        }

        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            ////Debugger.Launch();
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLogger.WriteEntry("In OnStart");

            try
            {
                // Set up a timer to trigger every minute.
                timerDelegate = new TimerCallback(EmailProcessor.ProcessEmail);
                stateTimer = new Timer(timerDelegate, eventLogger, DueTime, Period);


                // Set up a timer to trigger every minute.
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = LoggingTimeInterval; // 60 seconds
                timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
                timer.Start();
            }
            catch (Exception ex)
            {
                eventLogger.WriteEntry("Exception Occured: (" + Convert.ToInt64(EmailProcessor.EventId) + ") Message: " + ex.Message);
                if (ex.InnerException != null)
                {
                    eventLogger.WriteEntry("Inner Exception: " + ex.InnerException.Message);
                }
            }

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLogger.WriteEntry("End OnStart (" + EmailProcessor.Events + ")");
        }

        protected override void OnContinue()
        {
            // Update the service state to Continue Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_CONTINUE_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLogger.WriteEntry("In OnContinue. (" + EmailProcessor.Events + ")");

            try
            {
                stateTimer = new Timer(timerDelegate, null, DueTime, Period);
            }
            catch (Exception ex)
            {
                eventLogger.WriteEntry("Exception Occured: (" + Convert.ToInt64(EmailProcessor.EventId) + ") Message: " + ex.Message);
                if (ex.InnerException != null)
                {
                    eventLogger.WriteEntry("Inner Exception: " + ex.InnerException.Message);
                }
            }

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            eventLogger.WriteEntry("End OnContinue.");
        }

        protected override void OnPause()
        {
            // Update the service state to Paused Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_PAUSE_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLogger.WriteEntry("In OnPause. (" + EmailProcessor.Events + ")");

            try
            {
                stateTimer.Dispose();
                EmailProcessor.Dispose("PAUSED");
            }
            catch (Exception ex)
            {
                message = "Exception Occured OnPause Function: (" + Convert.ToInt64(EmailProcessor.EventId) + ") Message: " + ex.Message;

                eventLogger.WriteEntry("Exception Occured OnPause Function: (" + Convert.ToInt64(EmailProcessor.EventId) + ") Message: " + ex.Message);
                if (ex.InnerException != null)
                {
                    eventLogger.WriteEntry("Inner Exception: " + ex.InnerException.Message);
                    message += "_____Inner Exception: " + ex.InnerException.Message;
                }

                //fileName = "Log_BaseService";
                EmailProcessor.WriteLogs(message);
            }

            // Update the service state to Paused.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_PAUSED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLogger.WriteEntry("End OnPause.");
        }

        protected override void OnStop()
        {
            // Update the service state to Stop Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLogger.WriteEntry("In OnStop  (" + EmailProcessor.Events + ")");
            try
            {
                EmailProcessor.Dispose("STOPPED");
            }
            catch (Exception ex)
            {
                message = "Exception Occured OnStop Function: (" + Convert.ToInt64(EmailProcessor.EventId) + ") Message: " + ex.Message;
                eventLogger.WriteEntry("Exception Occured OnStop Function: (" + Convert.ToInt64(EmailProcessor.EventId) + ") Message: " + ex.Message);
                if (ex.InnerException != null)
                {
                    eventLogger.WriteEntry("Inner Exception: " + ex.InnerException.Message);
                    message += "_____Inner Exception: " + ex.InnerException.Message;
                }

                //fileName = "Log_BaseService";
                EmailProcessor.WriteLogs(message);
            }

            // Update the service state to Stopped.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLogger.WriteEntry("End OnStop");
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.
            //timerDelegate = new TimerCallback(EmailProcessor.ProcessEmail);
            //stateTimer = new Timer(timerDelegate, eventLogger, DueTime, Period);
            if (EmailProcessor.TotalEmails > 0)
            {
                eventLogger.WriteEntry("Event (" + EmailProcessor.Events + ") Total Emails: " + Convert.ToString(EmailProcessor.TotalEmails));

                if ((EmailProcessor.TotalEmails - EmailProcessor.ProcessedEmails) > 0)
                {
                    eventLogger.WriteEntry("Event (" + EmailProcessor.Events + ") Currently Processed: " + Convert.ToString(EmailProcessor.ProcessedEmails));
                }
            }
            else
            {
                eventLogger.WriteEntry("No Emails are available in Queue (" + EmailProcessor.Events + ")");
            }
        }
    }

    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public long dwServiceType;
        public ServiceState dwCurrentState;
        public long dwControlsAccepted;
        public long dwWin32ExitCode;
        public long dwServiceSpecificExitCode;
        public long dwCheckPoint;
        public long dwWaitHint;
    };
}

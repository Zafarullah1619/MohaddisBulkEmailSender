namespace EmailService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FunnelServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.EmailServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // FunnelServiceProcessInstaller
            // 
            this.FunnelServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.FunnelServiceProcessInstaller.Password = null;
            this.FunnelServiceProcessInstaller.Username = null;
            // 
            // EmailServiceInstaller
            // 
            this.EmailServiceInstaller.Description = "ECB Email Handling Service";
            this.EmailServiceInstaller.DisplayName = "EmailCampaignBuilder";
            this.EmailServiceInstaller.ServiceName = "EmailCampaignBuilder";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.FunnelServiceProcessInstaller,
            this.EmailServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller FunnelServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller EmailServiceInstaller;
    }
}
using System.Windows.Forms;
using System.Drawing;

namespace MunicipalServicesApp
{
    partial class MainMenuForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblWelcome;           // Welcome message at top
        private FlowLayoutPanel flowPanel;  // Container for buttons
        private Button btnReportIssues;     // Main functional button
        private Button btnEvents;           // Disabled feature
        private Button btnServiceStatus;    // Disabled feature

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblWelcome = new Label();
            this.flowPanel = new FlowLayoutPanel();
            this.btnReportIssues = new Button();
            this.btnEvents = new Button();
            this.btnServiceStatus = new Button();
            this.SuspendLayout();

            // lblWelcome
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcome.Text = "Welcome! Select a task below.";
            this.lblWelcome.ForeColor = Color.DarkBlue;
            this.lblWelcome.Dock = DockStyle.Top;
            this.lblWelcome.TextAlign = ContentAlignment.MiddleCenter;
            this.lblWelcome.Padding = new Padding(10);

            // flowPanel
            this.flowPanel.Dock = DockStyle.Fill;
            this.flowPanel.FlowDirection = FlowDirection.TopDown;
            this.flowPanel.WrapContents = false;
            this.flowPanel.Padding = new Padding(20);
            this.flowPanel.AutoScroll = true;
            this.flowPanel.BackColor = Color.Transparent;

            // btnReportIssues
            this.btnReportIssues.Text = "Report Issues";
            this.btnReportIssues.AutoSize = true;
            this.btnReportIssues.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.btnReportIssues.Margin = new Padding(10);
            this.btnReportIssues.Padding = new Padding(20, 10, 20, 10);
            this.btnReportIssues.ForeColor = Color.Blue;
            this.btnReportIssues.BackColor = Color.White;
            this.btnReportIssues.Click += new System.EventHandler(this.btnReportIssues_Click);

            // btnEvents
            this.btnEvents.Text = "Local Events and Announcements";
            this.btnEvents.Enabled = false;
            this.btnEvents.AutoSize = true;
            this.btnEvents.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.btnEvents.Margin = new Padding(10);
            this.btnEvents.Padding = new Padding(20, 10, 20, 10);
            this.btnEvents.ForeColor = Color.Blue;
            this.btnEvents.BackColor = Color.White;

            // btnServiceStatus
            this.btnServiceStatus.Text = "Service Request Status";
            this.btnServiceStatus.Enabled = false;
            this.btnServiceStatus.AutoSize = true;
            this.btnServiceStatus.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.btnServiceStatus.Margin = new Padding(10);
            this.btnServiceStatus.Padding = new Padding(20, 10, 20, 10);
            this.btnServiceStatus.ForeColor = Color.Blue;
            this.btnServiceStatus.BackColor = Color.White;

            // flowPanel controls
            this.flowPanel.Controls.Add(this.btnReportIssues);
            this.flowPanel.Controls.Add(this.btnEvents);
            this.flowPanel.Controls.Add(this.btnServiceStatus);

            // MainMenuForm
            this.Text = "Municipal Services Application";
            this.BackColor = Color.LightBlue;
            this.MinimumSize = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Controls.Add(this.flowPanel);
            this.Controls.Add(this.lblWelcome);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}



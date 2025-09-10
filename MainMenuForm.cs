using System;
using System.Drawing;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    /// <summary>
    /// Main menu form with navigation options
    /// </summary>
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Opens the issue reporting form as a modal dialog
        /// </summary>
        private void btnReportIssues_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (var form = new ReportIssuesForm())
            {
                form.ShowDialog(this);
            }
            this.Show();
        }
    }
}



using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    public partial class ReportIssuesForm : Form
    {
        private string attachedFilePath; // Path to the attached file
        private bool suppressProgressUpdates; // Flag to prevent progress updates during form reset

        public ReportIssuesForm()
        {
            InitializeComponent();
            InitializeTooltips();
        }

        private void InitializeTooltips()
        {
            var toolTip = new ToolTip();
            toolTip.SetToolTip(txtLocation, "Enter the address or area");
            toolTip.SetToolTip(cmbCategory, "Select the relevant service area");
            toolTip.SetToolTip(rtbDescription, "Describe the issue clearly");
            toolTip.SetToolTip(btnAttach, "Attach a photo or document (optional)");
            toolTip.SetToolTip(btnSubmit, "Submit your issue report");
            toolTip.SetToolTip(btnBack, "Return to the main menu");
        }

        /// <summary>
        /// Updates the progress bar and encouragement text based on form completion
        /// </summary>
        private void UpdateProgress()
        {
            if (suppressProgressUpdates)
            {
                return;
            }
            // Calculate progress: 25% per completed field
            int progress = 0;
            if (!string.IsNullOrWhiteSpace(txtLocation.Text)) progress += 25;
            if (cmbCategory.SelectedIndex >= 0) progress += 25;
            if (!string.IsNullOrWhiteSpace(rtbDescription.Text)) progress += 25;
            if (!string.IsNullOrWhiteSpace(attachedFilePath)) progress += 25;

            if (progress > 100) progress = 100;
            pbEngagement.Value = progress;

            if (progress >= 100)
            {
                lblEncouragement.Text = "Great! Ready to submit.";
            }
            else if (progress >= 75)
            {
                lblEncouragement.Text = "Keep going! Almost there.";
            }
            else if (progress >= 50)
            {
                lblEncouragement.Text = "Good progress!";
            }
            else if (progress > 0)
            {
                lblEncouragement.Text = "Let's get started.";
            }
            else
            {
                lblEncouragement.Text = string.Empty;
            }
        }

        /// <summary>
        /// Opens file dialog to attach media files
        /// </summary>
        private void btnAttach_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Select a file to attach";
                dialog.Filter = "Images|*.jpg;*.png|Documents|*.pdf;*.docx|All Files|*.*";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    attachedFilePath = dialog.FileName;
                    MessageBox.Show(this, "File attached: " + Path.GetFileName(attachedFilePath), "Attachment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateProgress();
                }
            }
        }

        /// <summary>
        /// Validates and submits the issue report
        /// </summary>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string location = txtLocation.Text.Trim();
            string category = cmbCategory.SelectedItem as string;
            string description = rtbDescription.Text.Trim();

            // Validate required fields
            if (string.IsNullOrWhiteSpace(location) || string.IsNullOrWhiteSpace(category) || string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show(this, "Please fill all required fields (Location, Category, Description).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Create and store the issue
            var issue = new Issue
            {
                Location = location,
                Category = category,
                Description = description,
                AttachedFilePath = attachedFilePath
            };

            IssueManager.AddIssue(issue);
            var guid = Guid.NewGuid().ToString();
            MessageBox.Show(this, "Issue reported successfully! Tracking ID: " + guid, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Award points for successful submission
            int totalPoints = IssueManager.AddPoints(10);

            // Show full completion, then reset fields without wiping the encouragement text
            pbEngagement.Value = 100;

            // Clear form fields while suppressing progress updates
            suppressProgressUpdates = true;
            txtLocation.Clear();
            cmbCategory.SelectedIndex = -1;
            rtbDescription.Clear();
            attachedFilePath = null;
            suppressProgressUpdates = false;

            // Reset progress for next report
            pbEngagement.Value = 0;

            // Show cumulative points earned
            lblEncouragement.Text = "You've earned " + totalPoints + " points total!";
        }

        /// <summary>
        /// Returns to the main menu
        /// </summary>
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Event handlers for real-time progress updates
        private void txtLocation_TextChanged(object sender, EventArgs e)
        {
            UpdateProgress();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateProgress();
        }

        private void rtbDescription_TextChanged(object sender, EventArgs e)
        {
            UpdateProgress();
        }
    }
}



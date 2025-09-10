using System.Windows.Forms;
using System.Drawing;

namespace MunicipalServicesApp
{
    partial class ReportIssuesForm
    {
        private System.ComponentModel.IContainer components = null;
        private TableLayoutPanel tableLayout;  // Main layout container
        private Label lblLocation;             // Location field label
        private TextBox txtLocation;           // Location input
        private Label lblCategory;             // Category field label
        private ComboBox cmbCategory;          // Category selection
        private Label lblDescription;          // Description field label
        private RichTextBox rtbDescription;    // Description input
        private Button btnAttach;              // File attachment button
        private Button btnSubmit;              // Submit report button
        private Button btnBack;                // Return to main menu
        private Label lblProgress;             // Progress bar label
        private ProgressBar pbEngagement;      // Gamification progress bar
        private Label lblEncouragement;        // Points and encouragement text

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
            this.tableLayout = new TableLayoutPanel();
            this.lblLocation = new Label();
            this.txtLocation = new TextBox();
            this.lblCategory = new Label();
            this.cmbCategory = new ComboBox();
            this.lblDescription = new Label();
            this.rtbDescription = new RichTextBox();
            this.btnAttach = new Button();
            this.btnSubmit = new Button();
            this.btnBack = new Button();
            this.lblProgress = new Label();
            this.pbEngagement = new ProgressBar();
            this.lblEncouragement = new Label();
            this.SuspendLayout();

            // tableLayout
            this.tableLayout.ColumnCount = 2;
            this.tableLayout.RowCount = 7;
            this.tableLayout.Dock = DockStyle.Fill;
            this.tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            this.tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            for (int i = 0; i < 7; i++)
            {
                this.tableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }
            this.tableLayout.Padding = new Padding(12);

            // Labels and inputs styling
            Font labelFont = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

            // lblLocation
            this.lblLocation.Text = "Location:";
            this.lblLocation.Font = labelFont;
            this.lblLocation.AutoSize = true;
            this.lblLocation.Anchor = AnchorStyles.Left;

            // txtLocation
            this.txtLocation.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.txtLocation.TextChanged += new System.EventHandler(this.txtLocation_TextChanged);

            // lblCategory
            this.lblCategory.Text = "Category:";
            this.lblCategory.Font = labelFont;
            this.lblCategory.AutoSize = true;
            this.lblCategory.Anchor = AnchorStyles.Left;

            // cmbCategory
            this.cmbCategory.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCategory.Items.AddRange(new object[] { "Sanitation", "Roads", "Utilities" });
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);

            // lblDescription
            this.lblDescription.Text = "Description:";
            this.lblDescription.Font = labelFont;
            this.lblDescription.AutoSize = true;
            this.lblDescription.Anchor = AnchorStyles.Left;

            // rtbDescription
            this.rtbDescription.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.rtbDescription.Height = 120;
            this.rtbDescription.TextChanged += new System.EventHandler(this.rtbDescription_TextChanged);

            // btnAttach
            this.btnAttach.Text = "Attach Media";
            this.btnAttach.Anchor = AnchorStyles.Left;
            this.btnAttach.ForeColor = Color.Blue;
            this.btnAttach.BackColor = Color.White;
            this.btnAttach.Click += new System.EventHandler(this.btnAttach_Click);

            // btnSubmit
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.Anchor = AnchorStyles.Left;
            this.btnSubmit.ForeColor = Color.Blue;
            this.btnSubmit.BackColor = Color.White;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);

            // btnBack
            this.btnBack.Text = "Back to Main Menu";
            this.btnBack.Anchor = AnchorStyles.Left;
            this.btnBack.ForeColor = Color.Blue;
            this.btnBack.BackColor = Color.White;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);

            // lblProgress
            this.lblProgress.Text = "Reporting Progress:";
            this.lblProgress.Font = labelFont;
            this.lblProgress.AutoSize = true;
            this.lblProgress.Anchor = AnchorStyles.Left;

            // pbEngagement
            this.pbEngagement.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.pbEngagement.Minimum = 0;
            this.pbEngagement.Maximum = 100;

            // lblEncouragement
            this.lblEncouragement.Text = string.Empty;
            this.lblEncouragement.AutoSize = true;
            this.lblEncouragement.ForeColor = Color.DarkGreen;
            this.lblEncouragement.Anchor = AnchorStyles.Left;

            // Add controls to tableLayout
            int r = 0;
            this.tableLayout.Controls.Add(this.lblLocation, 0, r);
            this.tableLayout.Controls.Add(this.txtLocation, 1, r++);
            this.tableLayout.Controls.Add(this.lblCategory, 0, r);
            this.tableLayout.Controls.Add(this.cmbCategory, 1, r++);
            this.tableLayout.Controls.Add(this.lblDescription, 0, r);
            this.tableLayout.Controls.Add(this.rtbDescription, 1, r++);
            this.tableLayout.Controls.Add(this.btnAttach, 1, r++);
            this.tableLayout.Controls.Add(this.lblProgress, 0, r);
            this.tableLayout.Controls.Add(this.pbEngagement, 1, r++);
            this.tableLayout.Controls.Add(this.lblEncouragement, 1, r++);
            this.tableLayout.Controls.Add(this.btnSubmit, 1, r++);
            this.tableLayout.Controls.Add(this.btnBack, 1, r++);

            // ReportIssuesForm
            this.Text = "Report an Issue";
            this.BackColor = Color.LightBlue;
            this.MinimumSize = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Controls.Add(this.tableLayout);
            this.ResumeLayout(false);
        }
    }
}



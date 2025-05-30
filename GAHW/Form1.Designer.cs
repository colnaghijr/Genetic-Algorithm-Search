namespace GeneticAlgorithm {
    partial class MainForm { // Renamed frmMain to MainForm
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.txtLookFor = new System.Windows.Forms.TextBox();
            this.lblLookFor = new System.Windows.Forms.Label();
            this.lblMaxGenerations = new System.Windows.Forms.Label();
            this.txtMaxGenerations = new System.Windows.Forms.TextBox();
            this.lblPopulationPerGeneration = new System.Windows.Forms.Label();
            this.txtPopulationPerGeneration = new System.Windows.Forms.TextBox();
            this.toolTipHelp = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(16, 126);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(660, 35);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(18, 171);
            this.txtOutput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput.Size = new System.Drawing.Size(658, 358);
            this.txtOutput.TabIndex = 1;
            // 
            // txtLookFor
            // 
            this.txtLookFor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLookFor.Location = new System.Drawing.Point(225, 18);
            this.txtLookFor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtLookFor.Name = "txtLookFor";
            this.txtLookFor.Size = new System.Drawing.Size(450, 26);
            this.txtLookFor.TabIndex = 2;
            this.txtLookFor.Text = "Hello World";
            // 
            // lblLookFor
            // 
            this.lblLookFor.AutoSize = true;
            this.lblLookFor.Location = new System.Drawing.Point(18, 23);
            this.lblLookFor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLookFor.Name = "lblLookFor";
            this.lblLookFor.Size = new System.Drawing.Size(76, 20);
            this.lblLookFor.TabIndex = 3;
            this.lblLookFor.Text = "Look For:";
            this.toolTipHelp.SetToolTip(this.lblLookFor, "Final genome (string) to look for");
            // 
            // lblMaxGenerations
            // 
            this.lblMaxGenerations.AutoSize = true;
            this.lblMaxGenerations.Location = new System.Drawing.Point(18, 59);
            this.lblMaxGenerations.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaxGenerations.Name = "lblMaxGenerations";
            this.lblMaxGenerations.Size = new System.Drawing.Size(130, 20);
            this.lblMaxGenerations.TabIndex = 5;
            this.lblMaxGenerations.Text = "Max Generations";
            this.toolTipHelp.SetToolTip(this.lblMaxGenerations, "Maximum number of generations to use when trying to get to the optimal genome");
            // 
            // txtMaxGenerations
            // 
            this.txtMaxGenerations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMaxGenerations.Location = new System.Drawing.Point(225, 54);
            this.txtMaxGenerations.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMaxGenerations.Name = "txtMaxGenerations";
            this.txtMaxGenerations.Size = new System.Drawing.Size(450, 26);
            this.txtMaxGenerations.TabIndex = 4;
            this.txtMaxGenerations.Text = "300";
            this.txtMaxGenerations.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMaxGenerations_KeyPress);
            // 
            // lblPopulationPerGeneration
            // 
            this.lblPopulationPerGeneration.AutoSize = true;
            this.lblPopulationPerGeneration.Location = new System.Drawing.Point(18, 95);
            this.lblPopulationPerGeneration.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPopulationPerGeneration.Name = "lblPopulationPerGeneration";
            this.lblPopulationPerGeneration.Size = new System.Drawing.Size(199, 20);
            this.lblPopulationPerGeneration.TabIndex = 7;
            this.lblPopulationPerGeneration.Text = "Population per Generation:";
            this.toolTipHelp.SetToolTip(this.lblPopulationPerGeneration, "Maximum population per generation. Larger populations enable more diversity at th" +
        "e cost of performance. Might be useful for large search strings.");
            // 
            // txtPopulationPerGeneration
            // 
            this.txtPopulationPerGeneration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPopulationPerGeneration.Location = new System.Drawing.Point(225, 90);
            this.txtPopulationPerGeneration.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPopulationPerGeneration.Name = "txtPopulationPerGeneration";
            this.txtPopulationPerGeneration.Size = new System.Drawing.Size(450, 26);
            this.txtPopulationPerGeneration.TabIndex = 6;
            this.txtPopulationPerGeneration.Text = "10000";
            this.txtPopulationPerGeneration.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPopulationPerGeneration_KeyPress);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 550);
            this.Controls.Add(this.lblPopulationPerGeneration);
            this.Controls.Add(this.txtPopulationPerGeneration);
            this.Controls.Add(this.lblMaxGenerations);
            this.Controls.Add(this.txtMaxGenerations);
            this.Controls.Add(this.lblLookFor);
            this.Controls.Add(this.txtLookFor);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnStart);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(439, 431);
            this.Name = "MainForm"; // Renamed frmMain to MainForm
            this.Text = "Genetic Algorithm Search";
            this.Load += new System.EventHandler(this.MainForm_Load); // Corrected to MainForm_Load
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown); // Corrected to MainForm_KeyDown
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.TextBox txtLookFor;
        private System.Windows.Forms.Label lblLookFor;
        private System.Windows.Forms.Label lblMaxGenerations;
        private System.Windows.Forms.TextBox txtMaxGenerations;
        private System.Windows.Forms.Label lblPopulationPerGeneration;
        private System.Windows.Forms.TextBox txtPopulationPerGeneration;
        private System.Windows.Forms.ToolTip toolTipHelp;
    }
}


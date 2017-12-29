namespace CustomerManager
{
    partial class MainWindow
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCompleted = new System.Windows.Forms.Button();
            this.btnCancel1 = new System.Windows.Forms.Button();
            this.btnToReady = new System.Windows.Forms.Button();
            this.btnToInProgress = new System.Windows.Forms.Button();
            this.btnMoveToInProcess = new System.Windows.Forms.Button();
            this.btnCancel2 = new System.Windows.Forms.Button();
            this.btnMoveToStandingBy = new System.Windows.Forms.Button();
            this.btnCancel3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(56, 532);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(2072, 407);
            this.dataGridView1.TabIndex = 0;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(56, 999);
            this.dataGridView2.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(2072, 407);
            this.dataGridView2.TabIndex = 1;
            this.dataGridView2.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            // 
            // dataGridView3
            // 
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Location = new System.Drawing.Point(56, 1469);
            this.dataGridView3.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.Size = new System.Drawing.Size(2072, 407);
            this.dataGridView3.TabIndex = 2;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.helpToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(3118, 49);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(75, 45);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(178, 46);
            this.exitMenuItem.Text = "Exit";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(99, 45);
            this.helpToolStripMenuItem.Text = "Tools";
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(92, 45);
            this.helpToolStripMenuItem1.Text = "Help";
            // 
            // btnCompleted
            // 
            this.btnCompleted.AutoSize = true;
            this.btnCompleted.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnCompleted.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnCompleted.FlatAppearance.BorderSize = 4;
            this.btnCompleted.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCompleted.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCompleted.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCompleted.Location = new System.Drawing.Point(2255, 532);
            this.btnCompleted.Name = "btnCompleted";
            this.btnCompleted.Size = new System.Drawing.Size(511, 108);
            this.btnCompleted.TabIndex = 5;
            this.btnCompleted.Text = "Completed";
            this.btnCompleted.UseVisualStyleBackColor = false;
            // 
            // btnCancel1
            // 
            this.btnCancel1.BackColor = System.Drawing.Color.Crimson;
            this.btnCancel1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel1.Location = new System.Drawing.Point(2255, 679);
            this.btnCancel1.Name = "btnCancel1";
            this.btnCancel1.Size = new System.Drawing.Size(511, 108);
            this.btnCancel1.TabIndex = 6;
            this.btnCancel1.Text = "Cancel";
            this.btnCancel1.UseVisualStyleBackColor = false;
            // 
            // btnToReady
            // 
            this.btnToReady.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnToReady.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnToReady.Location = new System.Drawing.Point(2260, 999);
            this.btnToReady.Name = "btnToReady";
            this.btnToReady.Size = new System.Drawing.Size(511, 108);
            this.btnToReady.TabIndex = 8;
            this.btnToReady.Text = "Ready";
            this.btnToReady.UseVisualStyleBackColor = false;
            // 
            // btnToInProgress
            // 
            this.btnToInProgress.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnToInProgress.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnToInProgress.Location = new System.Drawing.Point(2260, 1456);
            this.btnToInProgress.Name = "btnToInProgress";
            this.btnToInProgress.Size = new System.Drawing.Size(511, 108);
            this.btnToInProgress.TabIndex = 9;
            this.btnToInProgress.Text = "To In Progress";
            this.btnToInProgress.UseVisualStyleBackColor = false;
            // 
            // btnMoveToInProcess
            // 
            this.btnMoveToInProcess.BackColor = System.Drawing.Color.Orange;
            this.btnMoveToInProcess.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMoveToInProcess.Location = new System.Drawing.Point(2260, 831);
            this.btnMoveToInProcess.Name = "btnMoveToInProcess";
            this.btnMoveToInProcess.Size = new System.Drawing.Size(511, 108);
            this.btnMoveToInProcess.TabIndex = 11;
            this.btnMoveToInProcess.Text = "Move to In Progress";
            this.btnMoveToInProcess.UseVisualStyleBackColor = false;
            // 
            // btnCancel2
            // 
            this.btnCancel2.BackColor = System.Drawing.Color.Crimson;
            this.btnCancel2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel2.Location = new System.Drawing.Point(2260, 1147);
            this.btnCancel2.Name = "btnCancel2";
            this.btnCancel2.Size = new System.Drawing.Size(511, 108);
            this.btnCancel2.TabIndex = 12;
            this.btnCancel2.Text = "Cancel";
            this.btnCancel2.UseVisualStyleBackColor = false;
            // 
            // btnMoveToStandingBy
            // 
            this.btnMoveToStandingBy.BackColor = System.Drawing.Color.Orange;
            this.btnMoveToStandingBy.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMoveToStandingBy.Location = new System.Drawing.Point(2260, 1298);
            this.btnMoveToStandingBy.Name = "btnMoveToStandingBy";
            this.btnMoveToStandingBy.Size = new System.Drawing.Size(511, 108);
            this.btnMoveToStandingBy.TabIndex = 13;
            this.btnMoveToStandingBy.Text = "Move to Standby";
            this.btnMoveToStandingBy.UseVisualStyleBackColor = false;
            // 
            // btnCancel3
            // 
            this.btnCancel3.BackColor = System.Drawing.Color.Crimson;
            this.btnCancel3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel3.Location = new System.Drawing.Point(2260, 1609);
            this.btnCancel3.Name = "btnCancel3";
            this.btnCancel3.Size = new System.Drawing.Size(511, 108);
            this.btnCancel3.TabIndex = 15;
            this.btnCancel3.Text = "Cancel";
            this.btnCancel3.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(50, 493);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(234, 32);
            this.label1.TabIndex = 16;
            this.label1.Text = "Completion State";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(50, 960);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(232, 32);
            this.label2.TabIndex = 17;
            this.label2.Text = "In Progress State";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(50, 1430);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(194, 32);
            this.label3.TabIndex = 18;
            this.label3.Text = "Standby State";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Navy;
            this.ClientSize = new System.Drawing.Size(3118, 1903);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel3);
            this.Controls.Add(this.btnMoveToStandingBy);
            this.Controls.Add(this.btnCancel2);
            this.Controls.Add(this.btnMoveToInProcess);
            this.Controls.Add(this.btnToInProgress);
            this.Controls.Add(this.btnToReady);
            this.Controls.Add(this.btnCancel1);
            this.Controls.Add(this.btnCompleted);
            this.Controls.Add(this.dataGridView3);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "MainWindow";
            this.Text = "CustomerManager";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.Button btnCompleted;
        private System.Windows.Forms.Button btnCancel1;
        private System.Windows.Forms.Button btnToReady;
        private System.Windows.Forms.Button btnToInProgress;
        private System.Windows.Forms.Button btnMoveToInProcess;
        private System.Windows.Forms.Button btnCancel2;
        private System.Windows.Forms.Button btnMoveToStandingBy;
        private System.Windows.Forms.Button btnCancel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}


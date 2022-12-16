namespace Battleship
{
    partial class Form1
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
            this.MainMap = new System.Windows.Forms.PictureBox();
            this.EnemyMap = new System.Windows.Forms.PictureBox();
            this.LabelObjective = new System.Windows.Forms.Label();
            this.LabelInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.MainMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EnemyMap)).BeginInit();
            this.SuspendLayout();
            // 
            // MainMap
            // 
            this.MainMap.Location = new System.Drawing.Point(25, 26);
            this.MainMap.Name = "MainMap";
            this.MainMap.Size = new System.Drawing.Size(320, 320);
            this.MainMap.TabIndex = 0;
            this.MainMap.TabStop = false;
            this.MainMap.Click += new System.EventHandler(this.MainMap_Click);
            this.MainMap.Paint += new System.Windows.Forms.PaintEventHandler(this.MainMap_Paint);
            // 
            // EnemyMap
            // 
            this.EnemyMap.Location = new System.Drawing.Point(418, 26);
            this.EnemyMap.Name = "EnemyMap";
            this.EnemyMap.Size = new System.Drawing.Size(320, 320);
            this.EnemyMap.TabIndex = 1;
            this.EnemyMap.TabStop = false;
            this.EnemyMap.Paint += new System.Windows.Forms.PaintEventHandler(this.EnemyMap_Paint);
            // 
            // LabelObjective
            // 
            this.LabelObjective.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelObjective.AutoSize = true;
            this.LabelObjective.Font = new System.Drawing.Font("Stencil", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelObjective.Location = new System.Drawing.Point(77, 383);
            this.LabelObjective.Name = "LabelObjective";
            this.LabelObjective.Size = new System.Drawing.Size(119, 25);
            this.LabelObjective.TabIndex = 2;
            this.LabelObjective.Text = "Objective";
            this.LabelObjective.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelInfo
            // 
            this.LabelInfo.AutoSize = true;
            this.LabelInfo.Font = new System.Drawing.Font("Stencil", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelInfo.Location = new System.Drawing.Point(300, 387);
            this.LabelInfo.Name = "LabelInfo";
            this.LabelInfo.Size = new System.Drawing.Size(45, 19);
            this.LabelInfo.TabIndex = 3;
            this.LabelInfo.Text = "Info";
            this.LabelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(785, 426);
            this.Controls.Add(this.LabelInfo);
            this.Controls.Add(this.LabelObjective);
            this.Controls.Add(this.EnemyMap);
            this.Controls.Add(this.MainMap);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MainMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EnemyMap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox MainMap;
        private System.Windows.Forms.PictureBox EnemyMap;
        private System.Windows.Forms.Label LabelObjective;
        private System.Windows.Forms.Label LabelInfo;
    }
}


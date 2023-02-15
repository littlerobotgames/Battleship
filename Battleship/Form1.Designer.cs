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
            this.P1TurnLabel = new System.Windows.Forms.Label();
            this.P2TurnLabel = new System.Windows.Forms.Label();
            this.outcome_text = new System.Windows.Forms.Label();
            this.button_again = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MainMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EnemyMap)).BeginInit();
            this.SuspendLayout();
            // 
            // MainMap
            // 
            this.MainMap.Location = new System.Drawing.Point(24, 43);
            this.MainMap.Name = "MainMap";
            this.MainMap.Size = new System.Drawing.Size(320, 320);
            this.MainMap.TabIndex = 0;
            this.MainMap.TabStop = false;
            this.MainMap.Click += new System.EventHandler(this.MainMap_Click);
            this.MainMap.Paint += new System.Windows.Forms.PaintEventHandler(this.MainMap_Paint);
            this.MainMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainMap_mouseMove);
            // 
            // EnemyMap
            // 
            this.EnemyMap.Location = new System.Drawing.Point(436, 43);
            this.EnemyMap.Name = "EnemyMap";
            this.EnemyMap.Size = new System.Drawing.Size(320, 320);
            this.EnemyMap.TabIndex = 1;
            this.EnemyMap.TabStop = false;
            this.EnemyMap.Click += new System.EventHandler(this.EnemyMap_Click);
            this.EnemyMap.Paint += new System.Windows.Forms.PaintEventHandler(this.EnemyMap_Paint);
            // 
            // LabelObjective
            // 
            this.LabelObjective.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelObjective.AutoSize = true;
            this.LabelObjective.Font = new System.Drawing.Font("Stencil", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelObjective.Location = new System.Drawing.Point(19, 378);
            this.LabelObjective.Name = "LabelObjective";
            this.LabelObjective.Size = new System.Drawing.Size(119, 25);
            this.LabelObjective.TabIndex = 2;
            this.LabelObjective.Text = "Objective";
            this.LabelObjective.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // P1TurnLabel
            // 
            this.P1TurnLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.P1TurnLabel.AutoSize = true;
            this.P1TurnLabel.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.P1TurnLabel.Location = new System.Drawing.Point(146, 9);
            this.P1TurnLabel.Name = "P1TurnLabel";
            this.P1TurnLabel.Size = new System.Drawing.Size(70, 20);
            this.P1TurnLabel.TabIndex = 4;
            this.P1TurnLabel.Text = "Your Turn";
            // 
            // P2TurnLabel
            // 
            this.P2TurnLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.P2TurnLabel.AutoSize = true;
            this.P2TurnLabel.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.P2TurnLabel.Location = new System.Drawing.Point(554, 9);
            this.P2TurnLabel.Name = "P2TurnLabel";
            this.P2TurnLabel.Size = new System.Drawing.Size(83, 20);
            this.P2TurnLabel.TabIndex = 5;
            this.P2TurnLabel.Text = "Enemy Turn";
            // 
            // outcome_text
            // 
            this.outcome_text.AutoSize = true;
            this.outcome_text.Font = new System.Drawing.Font("Stencil", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outcome_text.Location = new System.Drawing.Point(300, 378);
            this.outcome_text.Name = "outcome_text";
            this.outcome_text.Size = new System.Drawing.Size(186, 47);
            this.outcome_text.TabIndex = 6;
            this.outcome_text.Text = "Vic/Loss";
            this.outcome_text.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_again
            // 
            this.button_again.Font = new System.Drawing.Font("Stencil", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_again.Location = new System.Drawing.Point(308, 428);
            this.button_again.Name = "button_again";
            this.button_again.Size = new System.Drawing.Size(178, 29);
            this.button_again.TabIndex = 7;
            this.button_again.Text = "Play Again?";
            this.button_again.UseVisualStyleBackColor = true;
            this.button_again.Click += new System.EventHandler(this.button_again_clicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(785, 488);
            this.Controls.Add(this.button_again);
            this.Controls.Add(this.outcome_text);
            this.Controls.Add(this.P2TurnLabel);
            this.Controls.Add(this.P1TurnLabel);
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
        private System.Windows.Forms.Label P1TurnLabel;
        private System.Windows.Forms.Label P2TurnLabel;
        private System.Windows.Forms.Label outcome_text;
        private System.Windows.Forms.Button button_again;
    }
}


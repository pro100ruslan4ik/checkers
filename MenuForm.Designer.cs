namespace Checkers
{
    partial class MenuForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuForm));
            titleLabel = new Label();
            easyButton = new Button();
            pvpButton = new Button();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.Anchor = AnchorStyles.Top;
            titleLabel.AutoSize = true;
            titleLabel.BackColor = Color.FromArgb(0, 0, 0, 0);
            titleLabel.Font = new Font("Ink Free", 47.9999962F, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline, GraphicsUnit.Point);
            titleLabel.Location = new Point(29, 26);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(457, 79);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Checkers Delta";
            // 
            // easyButton
            // 
            easyButton.Anchor = AnchorStyles.Top;
            easyButton.AutoSize = true;
            easyButton.BackColor = Color.FromArgb(230, 253, 230);
            easyButton.Cursor = Cursors.Hand;
            easyButton.Font = new Font("Arial Narrow", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            easyButton.ForeColor = SystemColors.ActiveCaptionText;
            easyButton.Location = new Point(181, 128);
            easyButton.Name = "easyButton";
            easyButton.Size = new Size(144, 43);
            easyButton.TabIndex = 1;
            easyButton.Text = "Coming Soon👍";
            easyButton.UseVisualStyleBackColor = false;
            easyButton.Click += EasyButton_Click;
            // 
            // pvpButton
            // 
            pvpButton.BackColor = Color.FromArgb(253, 230, 230);
            pvpButton.Cursor = Cursors.Hand;
            pvpButton.Font = new Font("Arial Narrow", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            pvpButton.ForeColor = SystemColors.ActiveCaptionText;
            pvpButton.Location = new Point(181, 206);
            pvpButton.Margin = new Padding(3, 2, 3, 2);
            pvpButton.Name = "pvpButton";
            pvpButton.Size = new Size(144, 44);
            pvpButton.TabIndex = 2;
            pvpButton.Text = "😡 vs 😡";
            pvpButton.UseVisualStyleBackColor = false;
            pvpButton.Click += PvP_Button_Click;
            // 
            // MenuForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(509, 340);
            Controls.Add(pvpButton);
            Controls.Add(easyButton);
            Controls.Add(titleLabel);
            ForeColor = SystemColors.ControlLightLight;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MenuForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label titleLabel;
        private Button easyButton;
        private Button pvpButton;
    }
}
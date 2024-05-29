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
            hardButton = new Button();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.Anchor = AnchorStyles.Top;
            titleLabel.AutoSize = true;
            titleLabel.BackColor = Color.FromArgb(0, 0, 0, 0);
            titleLabel.BorderStyle = BorderStyle.Fixed3D;
            titleLabel.Font = new Font("Ink Free", 47.9999962F, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline, GraphicsUnit.Point);
            titleLabel.Location = new Point(12, 35);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(569, 101);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Checkers Delta";
            // 
            // easyButton
            // 
            easyButton.Anchor = AnchorStyles.Top;
            easyButton.AutoSize = true;
            easyButton.Cursor = Cursors.Hand;
            easyButton.Font = new Font("Arial Narrow", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            easyButton.ForeColor = SystemColors.ActiveCaptionText;
            easyButton.Location = new Point(207, 171);
            easyButton.Margin = new Padding(3, 4, 3, 4);
            easyButton.Name = "easyButton";
            easyButton.Size = new Size(155, 57);
            easyButton.TabIndex = 1;
            easyButton.Text = "EASY👍";
            easyButton.UseVisualStyleBackColor = true;
            easyButton.Click += EasyButton_Click;
            // 
            // hardButton
            // 
            hardButton.Cursor = Cursors.Hand;
            hardButton.Font = new Font("Arial Narrow", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            hardButton.ForeColor = SystemColors.ActiveCaptionText;
            hardButton.Location = new Point(207, 274);
            hardButton.Name = "hardButton";
            hardButton.Size = new Size(155, 58);
            hardButton.TabIndex = 2;
            hardButton.Text = "HARD😡";
            hardButton.UseVisualStyleBackColor = true;

            // 
            // MenuForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(582, 453);
            Controls.Add(hardButton);
            Controls.Add(easyButton);
            Controls.Add(titleLabel);
            ForeColor = SystemColors.ControlLightLight;
            Margin = new Padding(3, 4, 3, 4);
            Name = "MenuForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label titleLabel;
        private Button easyButton;
        private Button hardButton;
    }
}
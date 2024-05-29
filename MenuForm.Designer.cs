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
            titleLabel = new Label();
            button1 = new Button();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Ink Free", 27.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            titleLabel.Location = new Point(42, 54);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(269, 46);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Checkers Delta";
            // 
            // button1
            // 
            button1.Location = new Point(104, 141);
            button1.Name = "button1";
            button1.Size = new Size(136, 43);
            button1.TabIndex = 1;
            button1.Text = "Easy";
            button1.UseVisualStyleBackColor = true;
            // 
            // MenuForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(359, 450);
            Controls.Add(button1);
            Controls.Add(titleLabel);
            Name = "MenuForm";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label titleLabel;
        private Button button1;
    }
}
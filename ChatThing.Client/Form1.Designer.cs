namespace ChatThing.Client
{
    partial class Form1
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
            this._tbName = new System.Windows.Forms.TextBox();
            this._tbMessage = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this._tbText = new System.Windows.Forms.TextBox();
            this._tbDebug = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _tbName
            // 
            this._tbName.Location = new System.Drawing.Point(12, 12);
            this._tbName.Name = "_tbName";
            this._tbName.Size = new System.Drawing.Size(125, 27);
            this._tbName.TabIndex = 0;
            this._tbName.TextChanged += new System.EventHandler(this._tbName_TextChanged);
            // 
            // _tbMessage
            // 
            this._tbMessage.Location = new System.Drawing.Point(12, 45);
            this._tbMessage.Name = "_tbMessage";
            this._tbMessage.Size = new System.Drawing.Size(676, 27);
            this._tbMessage.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(694, 43);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 29);
            this.button1.TabIndex = 2;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // _tbText
            // 
            this._tbText.Location = new System.Drawing.Point(12, 78);
            this._tbText.Multiline = true;
            this._tbText.Name = "_tbText";
            this._tbText.ReadOnly = true;
            this._tbText.Size = new System.Drawing.Size(776, 360);
            this._tbText.TabIndex = 3;
            // 
            // _tbDebug
            // 
            this._tbDebug.BackColor = System.Drawing.SystemColors.MenuText;
            this._tbDebug.ForeColor = System.Drawing.Color.LawnGreen;
            this._tbDebug.Location = new System.Drawing.Point(795, 12);
            this._tbDebug.Multiline = true;
            this._tbDebug.Name = "_tbDebug";
            this._tbDebug.Size = new System.Drawing.Size(285, 426);
            this._tbDebug.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1092, 450);
            this.Controls.Add(this._tbDebug);
            this.Controls.Add(this._tbText);
            this.Controls.Add(this.button1);
            this.Controls.Add(this._tbMessage);
            this.Controls.Add(this._tbName);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox _tbName;
        private TextBox _tbMessage;
        private Button button1;
        private TextBox _tbText;
        private TextBox _tbDebug;
    }
}
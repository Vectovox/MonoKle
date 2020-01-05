namespace ScriptTester
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.codeBox = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.newButton = new System.Windows.Forms.Button();
            this.executeButton = new System.Windows.Forms.Button();
            this.compileAllButton = new System.Windows.Forms.Button();
            this.compileSelectedButton = new System.Windows.Forms.Button();
            this.outputBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.outputBox);
            this.splitContainer1.Size = new System.Drawing.Size(876, 404);
            this.splitContainer1.SplitterDistance = 224;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.codeBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listBox1);
            this.splitContainer2.Panel2.Controls.Add(this.panel1);
            this.splitContainer2.Size = new System.Drawing.Size(876, 224);
            this.splitContainer2.SplitterDistance = 434;
            this.splitContainer2.TabIndex = 0;
            // 
            // codeBox
            // 
            this.codeBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeBox.Location = new System.Drawing.Point(0, 0);
            this.codeBox.Multiline = true;
            this.codeBox.Name = "codeBox";
            this.codeBox.Size = new System.Drawing.Size(434, 224);
            this.codeBox.TabIndex = 0;
            this.codeBox.TextChanged += new System.EventHandler(this.codeBox_TextChanged);
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(438, 186);
            this.listBox1.TabIndex = 1;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.newButton);
            this.panel1.Controls.Add(this.executeButton);
            this.panel1.Controls.Add(this.compileAllButton);
            this.panel1.Controls.Add(this.compileSelectedButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 186);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(438, 38);
            this.panel1.TabIndex = 0;
            // 
            // newButton
            // 
            this.newButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.newButton.Location = new System.Drawing.Point(75, 0);
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(77, 38);
            this.newButton.TabIndex = 3;
            this.newButton.Text = "New";
            this.newButton.UseVisualStyleBackColor = true;
            this.newButton.Click += new System.EventHandler(this.newButton_Click);
            // 
            // executeButton
            // 
            this.executeButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.executeButton.Location = new System.Drawing.Point(288, 0);
            this.executeButton.Name = "executeButton";
            this.executeButton.Size = new System.Drawing.Size(75, 38);
            this.executeButton.TabIndex = 2;
            this.executeButton.Text = "Execute";
            this.executeButton.UseVisualStyleBackColor = true;
            this.executeButton.Click += new System.EventHandler(this.executeButton_Click);
            // 
            // compileAllButton
            // 
            this.compileAllButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.compileAllButton.Location = new System.Drawing.Point(0, 0);
            this.compileAllButton.Name = "compileAllButton";
            this.compileAllButton.Size = new System.Drawing.Size(75, 38);
            this.compileAllButton.TabIndex = 1;
            this.compileAllButton.Text = "Compile all";
            this.compileAllButton.UseVisualStyleBackColor = true;
            this.compileAllButton.Click += new System.EventHandler(this.compileAllButton_Click);
            // 
            // compileSelectedButton
            // 
            this.compileSelectedButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.compileSelectedButton.Location = new System.Drawing.Point(363, 0);
            this.compileSelectedButton.Name = "compileSelectedButton";
            this.compileSelectedButton.Size = new System.Drawing.Size(75, 38);
            this.compileSelectedButton.TabIndex = 0;
            this.compileSelectedButton.Text = "Compile";
            this.compileSelectedButton.UseVisualStyleBackColor = true;
            this.compileSelectedButton.Click += new System.EventHandler(this.compileSelectedButton_Click);
            // 
            // outputBox
            // 
            this.outputBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputBox.Location = new System.Drawing.Point(0, 0);
            this.outputBox.Multiline = true;
            this.outputBox.Name = "outputBox";
            this.outputBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputBox.Size = new System.Drawing.Size(876, 176);
            this.outputBox.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 404);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox codeBox;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button executeButton;
        private System.Windows.Forms.Button compileAllButton;
        private System.Windows.Forms.Button compileSelectedButton;
        private System.Windows.Forms.TextBox outputBox;
        private System.Windows.Forms.Button newButton;
    }
}


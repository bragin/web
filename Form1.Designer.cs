namespace SkiaSharpOpenGLBenchmark
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
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            skglControl1 = new SkiaSharp.Views.Desktop.SKGLControl();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(20, 23);
            button1.Margin = new Padding(6, 7, 6, 7);
            button1.Name = "button1";
            button1.Size = new Size(66, 43);
            button1.TabIndex = 0;
            button1.Text = "10";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(94, 23);
            button2.Margin = new Padding(6, 7, 6, 7);
            button2.Name = "button2";
            button2.Size = new Size(66, 43);
            button2.TabIndex = 1;
            button2.Text = "1k";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(170, 23);
            button3.Margin = new Padding(6, 7, 6, 7);
            button3.Name = "button3";
            button3.Size = new Size(66, 43);
            button3.TabIndex = 2;
            button3.Text = "10k";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(246, 23);
            button4.Margin = new Padding(6, 7, 6, 7);
            button4.Name = "button4";
            button4.Size = new Size(66, 43);
            button4.TabIndex = 3;
            button4.Text = "100k";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // skglControl1
            // 
            skglControl1.BackColor = System.Drawing.Color.Purple;
            skglControl1.Location = new Point(20, 78);
            skglControl1.Margin = new Padding(9, 12, 9, 12);
            skglControl1.Name = "skglControl1";
            skglControl1.Size = new Size(1000, 1000);
            skglControl1.TabIndex = 4;
            skglControl1.VSync = false;
            skglControl1.PaintSurface += skglControl1_PaintSurface;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1087, 1092);
            Controls.Add(skglControl1);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(6, 7, 6, 7);
            Name = "Form1";
            Text = "A Browser";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private SkiaSharp.Views.Desktop.SKGLControl skglControl1;
        private SkiaSharp.Views.Desktop.SKGLControl skglControl2;
    }
}


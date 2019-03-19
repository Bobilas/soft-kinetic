namespace SmartInstructor
{
	partial class MainForm
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
			this.Button_Demo = new System.Windows.Forms.Button();
			this.PictureBox_Camera = new System.Windows.Forms.PictureBox();
			this.Label_FrameInfo = new System.Windows.Forms.Label();
			this.PictureBox_DepthMap = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.PictureBox_Camera)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PictureBox_DepthMap)).BeginInit();
			this.SuspendLayout();
			// 
			// Button_Demo
			// 
			this.Button_Demo.Location = new System.Drawing.Point(664, 470);
			this.Button_Demo.Name = "Button_Demo";
			this.Button_Demo.Size = new System.Drawing.Size(75, 28);
			this.Button_Demo.TabIndex = 0;
			this.Button_Demo.Text = "Demo";
			this.Button_Demo.UseVisualStyleBackColor = true;
			this.Button_Demo.Click += new System.EventHandler(this.Button_Demo_Click);
			// 
			// PictureBox_Camera
			// 
			this.PictureBox_Camera.Location = new System.Drawing.Point(12, 12);
			this.PictureBox_Camera.Name = "PictureBox_Camera";
			this.PictureBox_Camera.Size = new System.Drawing.Size(640, 480);
			this.PictureBox_Camera.TabIndex = 1;
			this.PictureBox_Camera.TabStop = false;
			// 
			// Label_FrameInfo
			// 
			this.Label_FrameInfo.AutoSize = true;
			this.Label_FrameInfo.Location = new System.Drawing.Point(661, 454);
			this.Label_FrameInfo.Name = "Label_FrameInfo";
			this.Label_FrameInfo.Size = new System.Drawing.Size(27, 13);
			this.Label_FrameInfo.TabIndex = 3;
			this.Label_FrameInfo.Text = "N/A";
			// 
			// PictureBox_DepthMap
			// 
			this.PictureBox_DepthMap.Location = new System.Drawing.Point(745, 12);
			this.PictureBox_DepthMap.Name = "PictureBox_DepthMap";
			this.PictureBox_DepthMap.Size = new System.Drawing.Size(640, 480);
			this.PictureBox_DepthMap.TabIndex = 4;
			this.PictureBox_DepthMap.TabStop = false;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1401, 505);
			this.Controls.Add(this.PictureBox_DepthMap);
			this.Controls.Add(this.Label_FrameInfo);
			this.Controls.Add(this.Button_Demo);
			this.Controls.Add(this.PictureBox_Camera);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "MainForm";
			this.Text = "Demo";
			((System.ComponentModel.ISupportInitialize)(this.PictureBox_Camera)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PictureBox_DepthMap)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button Button_Demo;
		private System.Windows.Forms.PictureBox PictureBox_Camera;
		private System.Windows.Forms.Label Label_FrameInfo;
		private System.Windows.Forms.PictureBox PictureBox_DepthMap;
	}
}


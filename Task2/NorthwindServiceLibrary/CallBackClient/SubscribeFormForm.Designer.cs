namespace CallBackClient
{
	partial class SubscribeFormForm
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose (bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent ()
		{
			this.btnSubscribe = new System.Windows.Forms.Button();
			this.btnUnSubscribe = new System.Windows.Forms.Button();
			this.tbMessages = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnSubscribe
			// 
			this.btnSubscribe.Location = new System.Drawing.Point(12, 12);
			this.btnSubscribe.Name = "btnSubscribe";
			this.btnSubscribe.Size = new System.Drawing.Size(75, 23);
			this.btnSubscribe.TabIndex = 0;
			this.btnSubscribe.Text = "Subscribe";
			this.btnSubscribe.UseVisualStyleBackColor = true;
			this.btnSubscribe.Click += new System.EventHandler(this.btnSubscribe_Click);
			// 
			// btnUnSubscribe
			// 
			this.btnUnSubscribe.Location = new System.Drawing.Point(93, 12);
			this.btnUnSubscribe.Name = "btnUnSubscribe";
			this.btnUnSubscribe.Size = new System.Drawing.Size(75, 23);
			this.btnUnSubscribe.TabIndex = 1;
			this.btnUnSubscribe.Text = "Unsubscribe";
			this.btnUnSubscribe.UseVisualStyleBackColor = true;
			this.btnUnSubscribe.Click += new System.EventHandler(this.btnUnSubscribe_Click);
			// 
			// tbMessages
			// 
			this.tbMessages.Location = new System.Drawing.Point(12, 41);
			this.tbMessages.Multiline = true;
			this.tbMessages.Name = "tbMessages";
			this.tbMessages.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbMessages.Size = new System.Drawing.Size(453, 345);
			this.tbMessages.TabIndex = 2;
			// 
			// SubscribeFormForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(477, 398);
			this.Controls.Add(this.tbMessages);
			this.Controls.Add(this.btnUnSubscribe);
			this.Controls.Add(this.btnSubscribe);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SubscribeFormForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Change order status subscription";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnSubscribe;
		private System.Windows.Forms.Button btnUnSubscribe;
		private System.Windows.Forms.TextBox tbMessages;
	}
}


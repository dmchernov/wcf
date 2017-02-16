using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CallBackClient
{
	partial class SubscribeForm
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private IContainer components = null;

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
			this.btnSubscribe = new Button();
			this.btnUnSubscribe = new Button();
			this.tbMessages = new TextBox();
			this.SuspendLayout();
			// 
			// btnSubscribe
			// 
			this.btnSubscribe.Location = new Point(12, 12);
			this.btnSubscribe.Name = "btnSubscribe";
			this.btnSubscribe.Size = new Size(75, 23);
			this.btnSubscribe.TabIndex = 0;
			this.btnSubscribe.Text = "Subscribe";
			this.btnSubscribe.UseVisualStyleBackColor = true;
			this.btnSubscribe.Click += new EventHandler(this.btnSubscribe_Click);
			// 
			// btnUnSubscribe
			// 
			this.btnUnSubscribe.Location = new Point(93, 12);
			this.btnUnSubscribe.Name = "btnUnSubscribe";
			this.btnUnSubscribe.Size = new Size(75, 23);
			this.btnUnSubscribe.TabIndex = 1;
			this.btnUnSubscribe.Text = "Unsubscribe";
			this.btnUnSubscribe.UseVisualStyleBackColor = true;
			this.btnUnSubscribe.Click += new EventHandler(this.btnUnSubscribe_Click);
			// 
			// tbMessages
			// 
			this.tbMessages.Location = new Point(12, 41);
			this.tbMessages.Multiline = true;
			this.tbMessages.Name = "tbMessages";
			this.tbMessages.ScrollBars = ScrollBars.Both;
			this.tbMessages.Size = new Size(453, 345);
			this.tbMessages.TabIndex = 2;
			// 
			// SubscribeFormForm
			// 
			this.AutoScaleDimensions = new SizeF(6F, 13F);
			this.AutoScaleMode = AutoScaleMode.Font;
			this.ClientSize = new Size(477, 398);
			this.Controls.Add(this.tbMessages);
			this.Controls.Add(this.btnUnSubscribe);
			this.Controls.Add(this.btnSubscribe);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SubscribeFormForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Change order status subscription";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Button btnSubscribe;
		private Button btnUnSubscribe;
		private TextBox tbMessages;
	}
}


namespace CategoriesApplication
{
	partial class CategoriesForm
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnClear = new System.Windows.Forms.Button();
			this.btnSet = new System.Windows.Forms.Button();
			this.btnGet = new System.Windows.Forms.Button();
			this.cbCategories = new System.Windows.Forms.ComboBox();
			this.lblCategory = new System.Windows.Forms.Label();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.tbPath = new System.Windows.Forms.TextBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.pbCategoryImage = new System.Windows.Forms.PictureBox();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbCategoryImage)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnClear);
			this.panel1.Controls.Add(this.btnSet);
			this.panel1.Controls.Add(this.btnGet);
			this.panel1.Controls.Add(this.cbCategories);
			this.panel1.Controls.Add(this.lblCategory);
			this.panel1.Controls.Add(this.btnBrowse);
			this.panel1.Controls.Add(this.tbPath);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(711, 69);
			this.panel1.TabIndex = 0;
			// 
			// btnClear
			// 
			this.btnClear.Location = new System.Drawing.Point(624, 35);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(75, 23);
			this.btnClear.TabIndex = 7;
			this.btnClear.Text = "Clear";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// btnSet
			// 
			this.btnSet.Location = new System.Drawing.Point(543, 35);
			this.btnSet.Name = "btnSet";
			this.btnSet.Size = new System.Drawing.Size(75, 23);
			this.btnSet.TabIndex = 6;
			this.btnSet.Text = "Set img";
			this.btnSet.UseVisualStyleBackColor = true;
			this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
			// 
			// btnGet
			// 
			this.btnGet.Location = new System.Drawing.Point(462, 35);
			this.btnGet.Name = "btnGet";
			this.btnGet.Size = new System.Drawing.Size(75, 23);
			this.btnGet.TabIndex = 5;
			this.btnGet.Text = "Get img";
			this.btnGet.UseVisualStyleBackColor = true;
			this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
			// 
			// cbCategories
			// 
			this.cbCategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbCategories.Location = new System.Drawing.Point(81, 37);
			this.cbCategories.Name = "cbCategories";
			this.cbCategories.Size = new System.Drawing.Size(375, 21);
			this.cbCategories.TabIndex = 4;
			// 
			// lblCategory
			// 
			this.lblCategory.AutoSize = true;
			this.lblCategory.Location = new System.Drawing.Point(12, 40);
			this.lblCategory.Name = "lblCategory";
			this.lblCategory.Size = new System.Drawing.Size(63, 13);
			this.lblCategory.TabIndex = 3;
			this.lblCategory.Text = "Категория:";
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(624, 10);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 1;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// tbPath
			// 
			this.tbPath.Location = new System.Drawing.Point(12, 12);
			this.tbPath.Name = "tbPath";
			this.tbPath.ReadOnly = true;
			this.tbPath.Size = new System.Drawing.Size(606, 20);
			this.tbPath.TabIndex = 0;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.pbCategoryImage);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 69);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(711, 422);
			this.panel2.TabIndex = 1;
			// 
			// pbCategoryImage
			// 
			this.pbCategoryImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbCategoryImage.Location = new System.Drawing.Point(0, 0);
			this.pbCategoryImage.Name = "pbCategoryImage";
			this.pbCategoryImage.Size = new System.Drawing.Size(711, 422);
			this.pbCategoryImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pbCategoryImage.TabIndex = 0;
			this.pbCategoryImage.TabStop = false;
			// 
			// CategoriesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(711, 491);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CategoriesForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Images for categories";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pbCategoryImage)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox tbPath;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Button btnSet;
		private System.Windows.Forms.Button btnGet;
		private System.Windows.Forms.ComboBox cbCategories;
		private System.Windows.Forms.Label lblCategory;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.PictureBox pbCategoryImage;
		private System.Windows.Forms.Button btnClear;
	}
}


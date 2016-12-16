using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using CategoriesApplication.CategoryService;

namespace CategoriesApplication
{
	public partial class Form1 : Form
	{
		private CategoryServiceClient service = new CategoryServiceClient();
		public Form1 ()
		{
			InitializeComponent();
			InitComboBox();
		}

		private void btnBrowse_Click (object sender, EventArgs e)
		{
			var ofDiag = new OpenFileDialog
			{
				Multiselect = false,
				Filter = @"BMP files (*.bmp) | *.bmp"
			};

			if (ofDiag.ShowDialog() == DialogResult.OK)
			{
				tbPath.Text = ofDiag.FileName;
				pbCategoryImage.Image = Image.FromFile(ofDiag.FileName);
			}
		}

		private void InitComboBox()
		{
			cbCategories.DataSource = service.GetCategories();
			cbCategories.ValueMember = "CategoryID";
			cbCategories.DisplayMember = "CategoryName";
		}
		
		//TODO метод, где не получается сохранить изображение из БД
		private void btnGet_Click (object sender, EventArgs e)
		{
			int size = 0;
			Stream stream = new MemoryStream();
			int id = (int) cbCategories.SelectedValue;
			service.GetCategoryImage(ref id, ref size, ref stream);

			//TODO при сохранении в файл он не читается
			//var fileName = @"D:\Temp\" + Guid.NewGuid() + ".bmp";
			//var fileStream = new FileStream(fileName, FileMode.Create);

			byte[] buffer = new byte[size];

			//stream.Read(buffer, 0, size);
			//fileStream.Write(buffer, 0, size);

			//stream.Close();
			//fileStream.Close();

			pbCategoryImage.Image = null;
			pbCategoryImage.Image = Image.FromStream(stream);
			//TODO Если брать изображение из сохраненного файла, то падает с System.OutOfMemoryException
			//pbCategoryImage.Image = Image.FromFile(fileName);
			stream.Close();
		}

		private void btnClear_Click (object sender, EventArgs e)
		{
			pbCategoryImage.Image = null;
		}

		private void btnSet_Click (object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(tbPath.Text))
			{
				MessageBox.Show(@"Не выбрано изображение.");
				return;
			}

			using (var serviceClient = new CategoryServiceClient())
			{
				using (var stream = new MemoryStream())
				{
					pbCategoryImage.Image.Save(stream, ImageFormat.Bmp);
					serviceClient.SetCategoryImage((int)cbCategories.SelectedValue, (int)stream.Length, stream);
				}
			}
			MessageBox.Show(@"Изображение отправлено");
		}
	}
}

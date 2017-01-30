using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CategoriesApplication.CategoryService;

namespace CategoriesApplication
{
	public partial class CategoriesForm : Form
	{
		private CategoryServiceClient _service = new CategoryServiceClient();
		public CategoriesForm ()
		{
			InitializeComponent();
			InitComboBox();
		}

		private void btnBrowse_Click (object sender, EventArgs e)
		{
			var ofDiag = new OpenFileDialog
			{
				Multiselect = false,
				Filter = @"Image files (*.jpg, *.bmp) | *.jpg;*.bmp"
			};

			if (ofDiag.ShowDialog() == DialogResult.OK)
			{
				tbPath.Text = ofDiag.FileName;
				pbCategoryImage.Image = Image.FromFile(ofDiag.FileName);
			}
		}

		private void InitComboBox()
		{
			cbCategories.DataSource = _service.GetCategories();
			cbCategories.ValueMember = "CategoryID";
			cbCategories.DisplayMember = "CategoryName";
		}
		
		//TODO метод, где не получается сохранить изображение из БД
		private void btnGet_Click (object sender, EventArgs e)
		{
			int size = 0;
			Stream stream = new MemoryStream();
			int id = (int) cbCategories.SelectedValue;
			_service.GetCategoryImage(ref id, ref size, ref stream);

			#region Сохранение в файл
			/*FileStream targetStream = null;
			Stream sourceStream = stream;

			string filePath = @"D:\Temp\getFile.jpg";

			using (targetStream = new FileStream(filePath, FileMode.Create,
				FileAccess.Write, FileShare.None))
			{
				const int bufferLen = 4096;
				byte[] buffer1 = new byte[bufferLen];
				int count = 0;
				while ((count = sourceStream.Read(buffer1, 0, bufferLen)) > 0)
				{
					targetStream.Write(buffer1, 0, count);
				}
				targetStream.Close();
				sourceStream.Close();
			}*/
			#endregion

			MemoryStream targetMemoryStream;
			var imageCat = new byte[size];

			using (targetMemoryStream = new MemoryStream(imageCat))
			{
				const int bufferLen = 4096;
				byte[] buffer1 = new byte[bufferLen];
				int count;
				while ((count = stream.Read(buffer1, 0, bufferLen)) > 0)
				{
					targetMemoryStream.Write(buffer1, 0, count);
				}

				try
				{
					pbCategoryImage.Image = Image.FromStream(targetMemoryStream);
				}
				catch
				{
					MessageBox.Show(@"Ошибка при показе изображения.");
					Clear();
				}
				targetMemoryStream.Close();
				stream.Close();
			}
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
				using (var stream = new FileStream(tbPath.Text, FileMode.Open, FileAccess.Read))
				{
					serviceClient.SetCategoryImage((int)cbCategories.SelectedValue, (int)stream.Length, stream);
				}
			}
			MessageBox.Show(@"Изображение отправлено");
		}

		private void btnClear_Click (object sender, EventArgs e)
		{
			Clear();
		}

		private void Clear()
		{
			pbCategoryImage.Image = null;
			tbPath.Text = String.Empty;
		}
	}
}

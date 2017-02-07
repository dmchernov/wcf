using System;
using System.Drawing;
using System.IO;
using System.ServiceModel;
using System.Windows.Forms;
using NorthwindService.Contracts.ServiceContracts;

namespace CategoriesApplication
{
	public partial class CategoriesForm : Form
	{
		private readonly ChannelFactory<ICategoryChannelService> _factory;
		public CategoriesForm ()
		{
			_factory = new ChannelFactory<ICategoryChannelService>("http");
			InitializeComponent();
			InitComboBox();

			Closed += OnClosed;
		}

		private void OnClosed(object sender, EventArgs eventArgs)
		{
			if (_factory?.State == CommunicationState.Faulted)
				_factory?.Abort();
			else
				_factory?.Close();
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
			using (var service = _factory.CreateChannel())
			{
				try
				{
					cbCategories.DataSource = service.GetCategories();
				}
				catch
				{
					if (service.State == CommunicationState.Faulted)
						service.Abort();
					else
						service.Close();
				}
			}
			cbCategories.ValueMember = "CategoryID";
			cbCategories.DisplayMember = "CategoryName";
		}
		
		private void btnGet_Click (object sender, EventArgs e)
		{
			Stream stream = new MemoryStream();
			int id = (int) cbCategories.SelectedValue;
			var img = new CategoryImage {ImageStream = stream, CategoryId = id};
			using (var service = _factory.CreateChannel())
			{
				try
				{
					img = service.GetCategoryImage(img);
				}
				catch
				{
					if (service.State == CommunicationState.Faulted)
						service.Abort();
					else
						service.Close();
				}
			}

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
			var imageCat = new byte[img.Size];

			using (targetMemoryStream = new MemoryStream(imageCat))
			{
				const int bufferLen = 4096;
				byte[] buffer1 = new byte[bufferLen];
				int count;
				while ((count = img.ImageStream.Read(buffer1, 0, bufferLen)) > 0)
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
				img.ImageStream.Close();
			}
		}

		private void btnSet_Click (object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(tbPath.Text))
			{
				MessageBox.Show(@"Не выбрано изображение.");
				return;
			}

			using (var service = _factory.CreateChannel())
			{
				using (var stream = new FileStream(tbPath.Text, FileMode.Open, FileAccess.Read))
				{
					var image = new CategoryImage { CategoryId = (int)cbCategories.SelectedValue, Size = (int)stream.Length, ImageStream = stream };
					try
					{
						service.SetCategoryImage(image);
					}
					catch
					{
						MessageBox.Show(@"Изображение не отправлено.");
						if (service.State == CommunicationState.Faulted)
							service.Abort();
						else
							service.Close();
						return;
					}
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

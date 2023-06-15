using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using WPH.Models.PatientImage;

namespace WPH.Helper
{
    public class FileAttachments
    {

        public string CreateThumbnail(IFormFile file, string path, string filaName)
        {
            try
            {
                var date = DateTime.Now;
                var url = $"\\Uploads\\thumbnail\\{date.Year}\\{date.Month}\\{date.Day}\\";
                var thumbPath = Path.Combine(path + url);

                if (!Directory.Exists(thumbPath))
                    Directory.CreateDirectory(thumbPath);

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    memoryStream.Position = 0;

                    using (var img = Image.FromStream(memoryStream))
                    {
                        int MAX_WIDTH = 120;
                        int MAX_HEIGHT = 120;

                        int width = img.Width;
                        int height = img.Height;

                        if (width > height)
                        {
                            if (width > MAX_WIDTH)
                            {
                                height = (int)Math.Round(height * ((float)MAX_WIDTH / (float)width));
                                width = MAX_WIDTH;
                            }
                        }
                        else
                        {
                            if (height > MAX_HEIGHT)
                            {
                                width = (int)Math.Round(width * ((float)MAX_HEIGHT / (float)height));
                                height = MAX_HEIGHT;
                            }
                        }
                        Image thumb = img.GetThumbnailImage(width, height, () => false, IntPtr.Zero);
                        string finalPath = Path.Combine(thumbPath, filaName);
                        thumb.Save(finalPath);
                        thumb.Dispose();
                        return url + filaName;
                    }
                }
            }
            catch
            {
                return "";
            }
        }

        public PatientImageViewModel UploadFiles(IFormFile file, string path, Guid? receptionId, Guid? patientId, int? attachmentTypeId)
        {
            try
            {
                PatientImageViewModel patientImageViewModel = null;
                var date = DateTime.Now;
                var url = $"\\Uploads\\main\\{date.Year}\\{date.Month}\\{date.Day}\\";
                var basePath = Path.Combine(path + url);

                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                var fileName = Guid.NewGuid();

                var extension = Path.GetExtension(file.FileName);

                var filePath = Path.Combine(basePath, fileName + extension);

                var thumPath = CreateThumbnail(file, path, fileName + extension);

                if (!File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        stream.Position = 0;
                        file.CopyTo(stream);
                    }

                    patientImageViewModel = new PatientImageViewModel
                    {
                        Guid = fileName,
                        PatientId = patientId,
                        ReceptionId = receptionId,
                        ThumbNailAddress = thumPath,
                        FileName = fileName + extension,
                        ImageAddress = url + fileName + extension,
                        ImageDateTime = date,
                        AttachmentTypeId = attachmentTypeId
                    };

                }
                return patientImageViewModel;
            }
            catch
            {
                return null;
            }
        }

        public string UploadBanner(IFormFile file, string rootPath, string oldPath, string fileName, string url)
        {
            try
            {
                var basePath = Path.Combine(rootPath + url);

                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                var extension = Path.GetExtension(file.FileName);

                var filePath = Path.Combine(basePath, fileName + extension);

                oldPath = Path.Combine(rootPath + oldPath);
                if (File.Exists(oldPath))
                    File.Delete(oldPath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    stream.Position = 0;
                    file.CopyTo(stream);
                }
                return url + fileName + extension;
            }
            catch
            {
                return "";
            }
        }

        public void DeleteBanner(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        public void DeleteFile(PatientImageViewModel viewModel)
        {
            if (File.Exists(viewModel.ImageAddress))
                File.Delete(viewModel.ImageAddress);

            if (File.Exists(viewModel.ThumbNailAddress))
                File.Delete(viewModel.ThumbNailAddress);
        }

        public void DeleteAllFiles(IEnumerable<PatientImageViewModel> viewModels, string rootPath)
        {
            foreach (var item in viewModels)
            {
                try
                {
                    item.ImageAddress = Path.Combine(rootPath + item.ImageAddress);
                    item.ThumbNailAddress = Path.Combine(rootPath + item.ThumbNailAddress);
                    DeleteFile(item);
                }
                catch
                { }
            }
        }

        public void DeleteAllBanners(IEnumerable<string> pathes, string rootPath)
        {
            foreach (var item in pathes)
            {
                try
                {
                    DeleteBanner(Path.Combine(rootPath + item));
                }
                catch
                { }
            }
        }


        public string ReduceOpacity(IFormFile file, string rootPath, string oldPath, string fileName, string url)
        {
            try
            {
                Bitmap bmp;
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    using (var image = Image.FromStream(memoryStream))
                    {
                        bmp = new Bitmap(image.Width, image.Height);

                        //create a graphics object from the image  
                        using (Graphics gfx = Graphics.FromImage(bmp))
                        {

                            //create a color matrix object  
                            ColorMatrix matrix = new ColorMatrix();

                            //set the opacity  
                            matrix.Matrix33 = 0.3f;

                            //create image attributes  
                            ImageAttributes attributes = new ImageAttributes();

                            //set the color(opacity) of the image  
                            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                            //now draw the image  
                            gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                        }
                    }
                }

                ImageConverter converter = new ImageConverter();
                var res = (byte[])converter.ConvertTo(bmp, typeof(byte[]));

                MemoryStream stream = new MemoryStream(res);

                IFormFile opacity_file = new FormFile(stream, 0, res.Length, file.FileName, file.FileName);

                return UploadBanner(opacity_file, rootPath, oldPath, fileName, url);
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}

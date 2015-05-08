using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyChy.Frame.Common.Helper
{
    public sealed class Thumbnail
    {
        private int width = 100;
        private string file = string.Empty;
        private string _newfile = string.Empty;


        private string thumbnailImage = "ThumbnailImages";
        private string thumbnailPath = string.Empty;
        private bool IsthumbnailImage = false;



        public Thumbnail(string file)
        {
            this.file = file;
        }

        #region 属性

        /// <summary>
        /// 是否使用缩微图文件夹
        /// </summary>
        public bool IsthumbnailImage1
        {
            get { return IsthumbnailImage; }
            set { IsthumbnailImage = value; }
        }

        /// <summary>
        /// 生成的新文件名
        /// </summary>
        public string Newfile
        {
            get { return _newfile; }
            set { _newfile = value; }
        }

        /// <summary>
        /// 图片宽
        /// </summary>
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        private int height = 100;

        /// <summary>
        /// 图片高
        /// </summary>
        public int Height
        {
            get { return height; }
            set { height = value; }
        }


        /// <summary>
        /// 图片文件地址
        /// </summary>
        public string File
        {
            get { return file; }
            set { file = value; }
        }

        /// <summary>
        /// 缩微图文件夹
        /// </summary>
        public string ThumbnailImage
        {
            get { return thumbnailImage; }
            set { thumbnailImage = value; }
        }

        public Thumbnail()
        {

        }
        #endregion

        #region 共有方法
        /// <summary>
        /// 生成缩略图 提供文件
        /// </summary>
        /// <param name="file">文件</param>
        public void thumbnailFile(string newfile)
        {
            this.ThumbnailFile(newfile, this.width, this.height);
        }

        /// <summary>
        /// 生成缩略图 提供文件
        /// </summary>
        /// <param name="files">文件</param>
        /// <param name="width">生成的缩微图的宽</param>
        /// <param name="height">生成的缩微图的高</param>
        public void thumbnailFile(string newfile, int width, int height)
        {
            this.ThumbnailFile(newfile, width, height);
        }
        #endregion

        #region 私有方法

        private void ThumbnailFile(string newfile, int width, int height)
        {
            this.width = width;
            this.height = height;
            if (IsthumbnailImage)
            {
                this.file = newfile;
            }
            else
            {
                this._newfile = newfile;
            }

            if (!string.IsNullOrEmpty(file))
            {
                if (IsthumbnailImage)
                {
                    this._newfile = IOFiles.Combine(IOFiles.GetFolderName(file), this.thumbnailImage);
                    IOFiles.CreatedFolder(this._newfile);
                }
                Thread trd = new Thread(new ThreadStart(TimedProgress));
                trd.Start();

                //TimedProgress();
            }
        }

        //根据原图片,缩微图大小等比例缩放 文件
        private void TimedProgress()
        {
            ImageCodecInfo icf = ImageHelper.GetImageCodecInfo(ImageFormat.Jpeg);

            Image image, thumbnailImage;
            string thumbnailImageFilename;

            if (SafeCheck.IsJPGstr(file))
            {
                string extension;
                extension = IOFiles.GetExtension(file);
                try
                {
                    using (image = Image.FromFile(file))
                    {
                        Size imageSize = GetImageSize(image);

                        using (thumbnailImage = ImageHelper.GetThumbnailImage(image, imageSize.Width, imageSize.Height))
                        {
                            if (IsthumbnailImage)
                            {
                                thumbnailImageFilename = IOFiles.Combine(thumbnailPath, IOFiles.GetFileNameWithoutExtension(file) + extension);
                            }
                            else
                            {
                                thumbnailImageFilename = this._newfile;
                            }

                            ImageHelper.SaveImage(thumbnailImage, thumbnailImageFilename, icf);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string em = ex.Message;
                }
            }

        }

        /// <summary>
        /// 等比例求出缩微图大小
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        private Size GetImageSize(Image picture)
        {
            Size imageSize;

            imageSize = new Size(width, height);

            if ((picture.Height > imageSize.Height) || (picture.Width > imageSize.Width))
            {

                double heightRatio = (double)picture.Height / picture.Width;
                double widthRatio = (double)picture.Width / picture.Height;

                int desiredHeight = imageSize.Height;
                int desiredWidth = imageSize.Width;


                imageSize.Height = desiredHeight;
                if (widthRatio > 0)
                    imageSize.Width = Convert.ToInt32(imageSize.Height * widthRatio);

                if (imageSize.Width > desiredWidth)
                {
                    imageSize.Width = desiredWidth;
                    imageSize.Height = Convert.ToInt32(imageSize.Width * heightRatio);
                }
            }
            else
            {
                imageSize.Width = picture.Width;
                imageSize.Height = picture.Height;
            }

            return imageSize;
        }

        #endregion

    }
}

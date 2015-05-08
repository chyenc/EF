using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MyChy.Frame.Common.Extensions;
using MyChy.Frame.Common.Helper;

namespace MyChy.Frame.Common.MvcHelper
{
    /// <summary>
    /// 文件上传帮助
    /// </summary>
    public sealed class UploadMvc
    {
        private bool _IsOther;
        private bool _IsSuccess;
        private bool _IsSwf;
        private bool _IsThumbnail;
        private string _OtherExt;
        private string _uploadFileExt;
        private string _uploadFormat;

        private string _uploadPath;
        private UploadUnitType _uploadtype;
        private IList<int> _uploadWith;
        private IList<int> _uploadHigth;


        public UploadMvc(string uploadPath)
        {
            this._uploadtype = UploadUnitType.成功;
            this._IsSuccess = false;
            this._uploadFileExt = string.Empty;
            this._uploadPath = string.Empty;
            this._uploadFormat = "yyyyMM";
            this._IsThumbnail = false;
            this._IsSwf = false;
            this._IsOther = false;
            this._OtherExt = "";
            UploadLength = 30 * 1024 * 1024;
            this._uploadWith = null;
            this._uploadHigth = null;
            this._uploadPath = uploadPath;
            this._uploadWith = new List<int>(0);
            this._uploadHigth = new List<int>(0);
        }

        private bool CheckExt(string Files)
        {
            bool flag = false;
            if (!this.IsOther)
            {
                if (SafeCheck.IsJPGstr(Files, this.FileExt))
                {
                    return true;
                }
                this._IsThumbnail = false;
                return flag;
            }
            if (SafeCheck.IsOther(Files, this.FileExt))
            {
                flag = true;
            }
            return flag;
        }

        private bool CheckFilePath()
        {
            if (!SafeCheck.IsMap(this._uploadPath))
            {
                this._uploadPath = IOFiles.GetFolderMapPath(this._uploadPath);
            }
            IOFiles.CreatedFolder(this._uploadPath);
            if (IOFiles.IsFolder(this._uploadPath))
            {
                return true;
            }
            this._uploadtype = UploadUnitType.存储文件夹错误;
            return false;
        }

        private bool CheckFile(HttpPostedFileBase fileUpload)
        {
            if (fileUpload.ContentLength == 0)
            {
                this._uploadtype = UploadUnitType.无文件上传;
                return false;
            }
            if (fileUpload.ContentLength > UploadLength)
            {
                this._uploadtype = UploadUnitType.文件过大;
                return false;
            }
            string Files = fileUpload.FileName;
            if (((this._uploadWith.Count > 0) && (this._uploadWith.Count == this._uploadHigth.Count)))
            {
                this._IsThumbnail = true;
            }
            if (!this.CheckExt(Files))
            {
                this._uploadtype = UploadUnitType.文件格式错误;
                return false;
            }
            this._uploadFileExt = IOFiles.GetExtension(Files).ToLower();
            return true;
        }

        private string SaveFile(HttpPostedFileBase fileUpload)
        {
            string NewFile = string.Empty;
            try
            {
                string date = DateTime.Now.Ticks.ToString();
                string filedate = string.Empty;
                string dateFormat = DateTime.Now.ToString(this._uploadFormat);
                string datapathyuan = IOFiles.CreatedFolderData(this._uploadPath, dateFormat, out filedate);
                string fileName = filedate + date + this._uploadFileExt;
                fileUpload.SaveAs(this._uploadPath + fileName);
                this._IsSuccess = true;
                if (this.IsThumbnail)
                {
                    Thumbnail thumbnail = null;
                    if ((this._uploadWith.Count > 0) && (this._uploadHigth.Count > 0))
                    {
                        int count = 0;
                        if (this._uploadWith.Count > this._uploadHigth.Count)
                        {
                            count = this._uploadHigth.Count;
                        }
                        else
                        {
                            count = this._uploadWith.Count;
                        }
                        string resultfile = fileName;
                        for (int i = 0; i < count; i++)
                        {
                            thumbnail = new Thumbnail(this._uploadPath + fileName);
                            NewFile = string.Concat(new object[] { filedate, date, "_", i, this._uploadFileExt });
                            thumbnail.thumbnailFile(this._uploadPath + NewFile, this._uploadWith[i], this._uploadHigth[i]);
                            if (i == 0)
                            {
                                resultfile = NewFile;
                            }
                        }
                        fileName = resultfile;
                    }
                }
                return fileName;
            }
            catch
            {
                this._uploadtype = UploadUnitType.保存文件出错;
                return "";
            }
        }

        public string UpLoadFile(HttpFileCollectionBase fileUpload)
        {
            if (fileUpload != null && fileUpload.Count > 0)
            {
                HttpPostedFileBase file = fileUpload[0];
                string newfile = string.Empty;
                if (!this.CheckFilePath() || !this.CheckFile(file))
                {
                    return newfile;
                }

                if (this.IsOther == false && this.IsSwf == false)
                {
                    if (!string.IsNullOrEmpty(this.UploadHigth) && !string.IsNullOrEmpty(this.UploadWith))
                    {
                        string[] hs = this.UploadHigth.Trim().Split(new char[] { ',' });
                        string[] ws = this.UploadWith.Trim().Split(new char[] { ',' });
                        if ((ws.Length >= 0) && (hs.Length == ws.Length))
                        {
                            int wi = 0;
                            int hi = 0;
                            for (int i = 0; i < ws.Length; i++)
                            {
                                wi = ws[i].ToT<int>(0);
                                hi = hs[i].ToT<int>(0);
                                if ((wi != 0) && (hi != 0))
                                {
                                    this._uploadWith.Add(wi);
                                    this._uploadHigth.Add(hi);
                                }
                            }
                            this.IsThumbnail = true;
                        }
                    }
                }

                return this.SaveFile(file);
            }
            return "";
        }

        public bool IsOther
        {
            get
            {
                return this._IsOther;
            }
            set
            {
                this._IsOther = value;
            }
        }

        public bool IsSuccess
        {
            get
            {
                return this._IsSuccess;
            }
            set
            {
                this._IsSuccess = value;
            }
        }

        public bool IsSwf
        {
            get
            {
                return this._IsSwf;
            }
            set
            {
                this._IsSwf = value;
            }
        }

        public bool IsThumbnail
        {
            get
            {
                return this._IsThumbnail;
            }
            set
            {
                this._IsThumbnail = value;
            }
        }

        public string OtherExt
        {
            get
            {
                return this._OtherExt;
            }
            set
            {
                this._OtherExt = value;
            }
        }

        public string UploadFormat
        {
            get
            {
                return this._uploadFormat;
            }
            set
            {
                this._uploadFormat = value;
            }
        }



        public string UploadPath
        {
            get
            {
                return this._uploadPath;
            }
            set
            {
                this._uploadPath = value;
            }
        }

        public UploadUnitType Uploadtype
        {
            get
            {
                return this._uploadtype;
            }
            set
            {
                this._uploadtype = value;
            }
        }

        public string UploadWith { get; set; }

        public string UploadHigth { get; set; }

        public int UploadLength { get; set; }

        public string FileExt { get; set; }
    }

    public enum UploadUnitType
    {
        存储文件夹错误 = 1,
        文件过大 = 2,
        文件格式错误 = 3,
        保存文件出错 = 4,
        成功 = 5,
        无文件上传 = 6,
    }
}

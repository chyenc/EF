using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyChy.Frame.Common.Helper
{
    public sealed class IOFiles
    {
        /// <summary>
        /// 判断这个文件夹是否存在
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static bool IsFolder(string files)
        {
            if (files.Length == 0 || !Directory.Exists(files))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 判断文件是否存在 真 存在
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static bool IsFile(string files)
        {
            if (files.Length == 0 || !File.Exists(files))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// 返回文件列表 不包括文件夹
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static string[] File_List(string files)
        {
            string[] Files;
            Files = Directory.GetFiles(files);
            return Files;

        }

        /// <summary>
        /// 返回文件夹列表
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static string[] Folder_List(string files)
        {
            string[] Files;
            Files = Directory.GetDirectories(files);
            return Files;
        }

        /// <summary>
        /// 返回文件扩展名
        /// </summary>
        /// <param name="Files"></param>
        /// <returns></returns>
        public static string GetExtension(string Files)
        {
            return (Path.GetExtension(Files));
        }

        /// <summary>
        /// 返回所在文件夹
        /// </summary>
        /// <param name="Files"></param>
        /// <returns></returns>
        public static string GetFolderName(string Files)
        {
            return (Path.GetDirectoryName(Files));
        }

        /// <summary>
        /// 是否存在不存在建立文件夹
        /// </summary>
        /// <param name="files"></param>
        public static void CreatedFolder(string files)
        {
            //是否存在
            if (!IsFolder(files))
            {
                try
                {
                    //建立文件夹
                    Directory.CreateDirectory(files);
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// 创建日期格式文件夹返回
        /// </summary>
        /// <param name="Folder"></param>
        public static string CreatedFolderData(string Folder)
        {
            string date = DateTime.Now.ToString("yyyyMM");
            return CreatedFolderData(Folder, date);
        }

        /// <summary>
        /// 创建日期格式文件夹返回
        /// </summary>
        /// <param name="Folder"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string CreatedFolderData(string Folder, string date)
        {
            string FolderData = string.Empty;
            return CreatedFolderData(Folder, date, out FolderData);
        }

        /// <summary>
        /// 创建日期格式文件夹返回
        /// </summary>
        /// <param name="Folder"></param>
        public static string CreatedFolderData(string Folder, out string FolderData)
        {
            string date = DateTime.Now.ToString("yyyyMM");
            return CreatedFolderData(Folder, date, out FolderData);
        }

        /// <summary>
        /// 创建日期格式文件夹返回
        /// </summary>
        /// <param name="Folder"></param>
        /// <param name="date"></param>
        /// <param name="FolderData"></param>
        /// <returns></returns>
        public static string CreatedFolderData(string Folder, string date, out string FolderData)
        {
            //  FolderData = date;
            string res = string.Empty;
            CreatedFolder(Folder);
            FolderData = date + "/";
            res = Folder + FolderData;
            CreatedFolder(res);
            return res;
        }



        /// <summary>
        /// 返回不带扩展名的文件地址
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static string GetFileNameWithoutExtension(string files)
        {
            return Path.GetFileNameWithoutExtension(files);
        }

        /// <summary>
        /// 检查是否是否数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNum(String str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] < '0' || str[i] > '9')
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 合并文件路径
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static string Combine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="cfile"></param>
        /// <param name="tfile"></param>
        public static bool Copy(string cfile, string tfile)
        {
            try
            {
                if (IsFile(cfile))
                {
                    File.Copy(cfile, tfile);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 返回.前的文件地址
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static string GetFileNameoutExtension(string files)
        {
            int x = files.LastIndexOf(".");
            if (x > 0)
            {
                return files.Substring(0, files.LastIndexOf("."));
            }
            return files;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="file"></param>
        public static void DelFile(string file)
        {
            if (IsFile(file))
            {
                try
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(file);
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// 返回文件的物理地址
        /// </summary>
        /// <param name="file"></param>
        public static string GetFileMapPath(string file)
        {
            HttpContext context = HttpContext.Current;
            string filename = string.Empty;
            if (context != null)
            {
                filename = context.Server.MapPath(file);
                if (!IsFile(filename))
                {
                    filename = context.Server.MapPath("/" + file);
                }
            }
            else
            {
                filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
            }
            return filename;
        }


        /// <summary>
        /// 返回文件夹的物理地址
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetFolderMapPath(string file)
        {
            HttpContext context = HttpContext.Current;
            string filename = string.Empty;

            if (context != null)
            {
                filename = context.Server.MapPath(file);
                if (!IsFolder(filename))
                {
                    filename = context.Server.MapPath("/" + file);
                }
            }
            else
            {
                filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
            }
            return filename;
        }

        /// <summary>
        /// 是否为物理地址
        /// </summary>
        /// <param name="Files"></param>
        /// <returns></returns>
        public static bool IsMap(string Files)
        {
            Files = Files.Replace("//", ":");
            if (Files.IndexOf(':') > 0)
            {
                return true;
            }
            return false;

        }


    }
}

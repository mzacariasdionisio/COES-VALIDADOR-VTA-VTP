// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.FileServer.FileServer
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using COES.Storage.App.FileManager;
using COES.Storage.App.Servicio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace COES.Storage.App.FileServer
{
  public class FileServer : IFileManager
  {
        private string UserLogin = ConfigurationManager.AppSettings["UserFS"];
        private string Domain = ConfigurationManager.AppSettings["DomainFS"];
        private string Password = ConfigurationManager.AppSettings["PasswordFS"];
        public string LocalDirectory = ConfigurationManager.AppSettings[nameof (LocalDirectory)];

    public bool UploadFromStream(Stream stream, string url, string targetBlobName)
    {
      using (new Impersonator(this.UserLogin, this.Domain, this.Password))
      {
        string path1 = this.LocalDirectory + url;
        if (!Directory.Exists(path1))
          return false;
        string path2 = path1 + targetBlobName;
        if (!File.Exists(path2))
        {
          using (FileStream fileStream = new FileStream(path2, FileMode.Create, FileAccess.Write))
            stream.CopyTo((Stream) fileStream);
        }
        return true;
      }
    }

    public bool UploadFromFileDirectory(string path, string url, string targetBlobName)
    {
      using (new Impersonator(this.UserLogin, this.Domain, this.Password))
      {
        string path1 = this.LocalDirectory + url;
        if (!Directory.Exists(path1))
          return false;
        string str = path1 + targetBlobName;
        if (!File.Exists(str))
          File.Copy(path, str);
        return true;
      }
    }

    public Stream DownloadToStream(string sourceBlobName)
    {
      using (new Impersonator(this.UserLogin, this.Domain, this.Password))
      {
        Stream stream = (Stream) new MemoryStream();
        string path = this.LocalDirectory + sourceBlobName;
        if (File.Exists(path))
          stream = (Stream) File.OpenRead(path);
        return stream;
      }
    }

    public byte[] DownloadToArrayBite(string sourceBlobName)
    {
      using (new Impersonator(this.UserLogin, this.Domain, this.Password))
      {
        byte[] numArray = (byte[]) null;
        string path = this.LocalDirectory + sourceBlobName;
        if (File.Exists(path))
          numArray = File.ReadAllBytes(path);
        return numArray;
      }
    }

    public bool RenameBlob(string url, string blobName, string newBlobName)
    {
      using (new Impersonator(this.UserLogin, this.Domain, this.Password))
      {
        if (!string.IsNullOrEmpty(url))
          url += "/";
        string str1 = this.LocalDirectory + url + blobName;
        if (File.Exists(str1))
        {
          string str2 = this.LocalDirectory + url + newBlobName;
          if (!File.Exists(str2))
          {
            File.Move(str1, str2);
            return true;
          }
        }
        return true;
      }
    }

    public void DeleteBlob(string blobName)
    {
      using (new Impersonator(this.UserLogin, this.Domain, this.Password))
      {
        string path = this.LocalDirectory + blobName;
        if (!File.Exists(path))
          return;
        File.Delete(path);
      }
    }

    public List<FileData> ListBlobs(string relativePath)
    {
      using (new Impersonator(this.UserLogin, this.Domain, this.Password))
      {
        List<FileData> fileDataList = new List<FileData>();
        DirectoryInfo directoryInfo = new DirectoryInfo(this.LocalDirectory + relativePath);
        foreach (FileInfo file in directoryInfo.GetFiles().OrderBy(X => X.Name))
        {
          if (file.Name != "Thumbs.db")
          {
            FileData fileData = new FileData()
            {
              FileName = file.Name,
              Extension = file.Extension,
              FileSize = ((Decimal) file.Length / 1024M).ToString("#,###.##") + " KB",
              FileType = "F",
              FileUrl = relativePath + file.Name,
              Icono = Tools.ObtenerIcono("F", file.Extension)
            };
            fileData.FileUrl = fileData.FileUrl.Replace('\\', '/');
            fileDataList.Add(fileData);
          }
        }
        foreach (DirectoryInfo directory in directoryInfo.GetDirectories().OrderBy(X => X.Name))
        {
          FileData fileData = new FileData()
          {
            FileName = directory.Name,
            FileType = "D",
            FileUrl = directory.FullName.Replace(this.LocalDirectory, string.Empty) + "/"
          };
          fileData.FileUrl = fileData.FileUrl.Replace('\\', '/');
          fileData.Icono = Tools.ObtenerIcono("D", string.Empty);
          fileDataList.Add(fileData);
        }
        return fileDataList;
      }
    }

    public bool CreateFolder(string url, string folderName)
    {
      using (new Impersonator(this.UserLogin, this.Domain, this.Password))
      {
        if (!string.IsNullOrEmpty(url))
          url += "/";
        string path = this.LocalDirectory + url + folderName;
        if (Directory.Exists(path))
          return false;
        Directory.CreateDirectory(path);
        return true;
      }
    }

    public bool DeleteFolder(string url)
    {
      using (new Impersonator(this.UserLogin, this.Domain, this.Password))
      {
        string path = this.LocalDirectory + url;
        if (!Directory.Exists(path))
          return false;
        Directory.Delete(path, true);
        return true;
      }
    }

    public bool RenameFolder(string url, string folderName, string folderAnterior)
    {
      using (new Impersonator(this.UserLogin, this.Domain, this.Password))
      {
        if (!(folderName.ToLower() != folderAnterior.ToLower()))
          return true;
        if (!string.IsNullOrEmpty(url))
          url += "/";
        string str1 = this.LocalDirectory + url + folderAnterior;
        if (Directory.Exists(str1))
        {
          string str2 = this.LocalDirectory + url + folderName;
          if (!Directory.Exists(str2))
          {
            Directory.Move(str1, str2);
            return true;
          }
        }
        return false;
      }
    }

    public int CopiarFile(string origen, string destino, string file)
    {
      try
      {
        using (new Impersonator(this.UserLogin, this.Domain, this.Password))
        {
          string str1 = this.LocalDirectory + origen + file;
          string str2 = this.LocalDirectory + destino + file;
          if (File.Exists(str1))
          {
            if (File.Exists(str2))
              return 2;
            File.Copy(str1, str2);
          }
          return 1;
        }
      }
      catch
      {
        return -1;
      }
    }

    public int CortarFile(string origen, string destino, string file)
    {
      try
      {
        using (new Impersonator(this.UserLogin, this.Domain, this.Password))
        {
          string str1 = this.LocalDirectory + origen + file;
          string str2 = this.LocalDirectory + destino + file;
          if (File.Exists(str1))
          {
            if (File.Exists(str2))
              return 2;
            File.Move(str1, str2);
          }
          return 1;
        }
      }
      catch
      {
        return -1;
      }
    }

    public int CopiarDirectory(string origen, string destino, string folder)
    {
      try
      {
        using (new Impersonator(this.UserLogin, this.Domain, this.Password))
        {
          string path1 = this.LocalDirectory + origen + folder;
          string path2 = this.LocalDirectory + destino + folder;
          if (Directory.Exists(path1))
          {
            if (Directory.Exists(path2))
              return 2;
            this.CopyAll(new DirectoryInfo(path1), new DirectoryInfo(path2));
          }
          return 1;
        }
      }
      catch
      {
        return -1;
      }
    }

    public int CortarDirectory(string origen, string destino, string folder)
    {
      try
      {
        using (new Impersonator(this.UserLogin, this.Domain, this.Password))
        {
          string str1 = this.LocalDirectory + origen + folder;
          string str2 = this.LocalDirectory + destino + folder;
          if (Directory.Exists(str1))
          {
            if (Directory.Exists(str2))
              return 2;
            Directory.Move(str1, str2);
          }
          return 1;
        }
      }
      catch
      {
        return -1;
      }
    }

    private void CopyAll(DirectoryInfo source, DirectoryInfo target)
    {
      using (new Impersonator(this.UserLogin, this.Domain, this.Password))
      {
        if (!Directory.Exists(target.FullName))
          Directory.CreateDirectory(target.FullName);
        foreach (FileInfo file in source.GetFiles())
          file.CopyTo(Path.Combine(target.FullName, file.Name), true);
        foreach (DirectoryInfo directory in source.GetDirectories())
        {
          DirectoryInfo subdirectory = target.CreateSubdirectory(directory.Name);
          this.CopyAll(directory, subdirectory);
        }
      }
    }

    public FileInfo ObtenerFileInfo(string url)
    {
      using (new Impersonator(this.UserLogin, this.Domain, this.Password))
      {
        if (File.Exists(this.LocalDirectory + url))
          return new FileInfo(this.LocalDirectory + url);
      }
      return (FileInfo) null;
    }

    public FileData ObtenerFile(string url)
    {
      using (new Impersonator(this.UserLogin, this.Domain, this.Password))
      {
        FileData fileData = (FileData) null;
        string str = this.LocalDirectory + url;
        if (File.Exists(str))
        {
          FileInfo fileInfo = new FileInfo(str);
          fileData = new FileData()
          {
            FileName = fileInfo.Name,
            Extension = fileInfo.Extension,
            FileSize = ((Decimal) fileInfo.Length / 1024M).ToString("#,###.##") + " KB",
            FileType = "F",
            FileUrl = str,
            Icono = Tools.ObtenerIcono("F", fileInfo.Extension)
          };
          fileData.FileUrl = fileData.FileUrl.Replace(this.LocalDirectory, string.Empty);
          fileData.FileUrl = fileData.FileUrl.Replace('\\', '/');
        }
        return fileData;
      }
    }

    public bool VerificarExistenciaFile(string url, string targetBlobName)
    {
      using (new Impersonator(this.UserLogin, this.Domain, this.Password))
        return File.Exists(this.LocalDirectory + url + targetBlobName);
    }
  }
}

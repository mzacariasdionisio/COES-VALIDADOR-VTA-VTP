// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.FileManager.IFileManager
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using System.Collections.Generic;
using System.IO;

namespace COES.Storage.App.FileManager
{
  public interface IFileManager
  {
    bool UploadFromStream(Stream stream, string url, string targetBlobName);

    bool UploadFromFileDirectory(string path, string url, string targetBlobName);

    Stream DownloadToStream(string sourceBlobName);

    byte[] DownloadToArrayBite(string sourceBlobName);

    bool RenameBlob(string url, string blobName, string newBlobName);

    void DeleteBlob(string blobName);

    List<FileData> ListBlobs(string relativePath);

    bool CreateFolder(string url, string folderName);

    bool DeleteFolder(string url);

    bool RenameFolder(string url, string folderName, string folderAnterior);

    int CopiarFile(string origen, string destino, string file);

    int CortarFile(string origen, string destino, string file);

    int CopiarDirectory(string origen, string destino, string folder);

    int CortarDirectory(string origen, string destino, string folder);

    FileInfo ObtenerFileInfo(string url);

    FileData ObtenerFile(string url);

    bool VerificarExistenciaFile(string url, string targetBlobName);
  }
}

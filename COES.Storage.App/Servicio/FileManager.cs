// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Servicio.FileManager
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata;
using COES.Storage.App.Metadata.Data;
using COES.Storage.App.Metadata.Entidad;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace COES.Storage.App.Servicio
{
  public class FileManager
  {
    public bool UploadFromStream(
      Stream stream,
      string url,
      string targetBlobName,
      string user,
      string indReemplazo,
      string fileReemplazo)
    {
      if (indReemplazo != "S")
      {
        new COES.Storage.App.Servicio.Servicio().GrabarBlob(url, targetBlobName, stream.Length.ToString(), "F", user);
        return FactoryFile.GetFileManager().UploadFromStream(stream, url, targetBlobName);
      }
      new COES.Storage.App.Servicio.Servicio().ActualizarUrlFile(url, targetBlobName, stream.Length.ToString(), user, fileReemplazo);
      FactoryFile.GetFileManager().DeleteBlob(fileReemplazo);
      return FactoryFile.GetFileManager().UploadFromStream(stream, url, targetBlobName);
    }

    public bool UploadFromFileDirectory(
      string path,
      string url,
      string targetBlobName,
      string user,
      string longitud,
      string indReemplazo,
      string fileReemplazo)
    {
      if (indReemplazo != "S")
      {
        new COES.Storage.App.Servicio.Servicio().GrabarBlob(url, targetBlobName, longitud, "F", user);
        return FactoryFile.GetFileManager().UploadFromFileDirectory(path, url, targetBlobName);
      }
      new COES.Storage.App.Servicio.Servicio().ActualizarUrlFile(url, targetBlobName, longitud, user, fileReemplazo);
      FactoryFile.GetFileManager().DeleteBlob(fileReemplazo);
      return FactoryFile.GetFileManager().UploadFromFileDirectory(path, url, targetBlobName);
    }

    public Stream DownloadToStream(string sourceBlobName) => FactoryFile.GetFileManager().DownloadToStream(sourceBlobName);

    public byte[] DownloadToArrayByte(string sourceBlobName) => FactoryFile.GetFileManager().DownloadToArrayBite(sourceBlobName);

    public int DownloadAsZip(List<string> sourcesBlob, string filename)
    {
      try
      {
        using (ZipFile zipFile = new ZipFile())
        {
          foreach (string sourceBlobName in sourcesBlob)
          {
            int num = sourceBlobName.LastIndexOf('/');
            string empty = string.Empty;
            string str = num < 0 ? sourceBlobName : sourceBlobName.Substring(num + 1, sourceBlobName.Length - num - 1);
            Stream stream = this.DownloadToStream(sourceBlobName);
            zipFile.AddEntry(str, stream);
          }
          if (File.Exists(filename))
            File.Delete(filename);
          zipFile.Save(filename);
          return 1;
        }
      }
      catch
      {
        return -1;
      }
    }

    public int RenameBlob(int id, string url, string filename, string extension, string user)
    {
      try
      {
        filename = filename + "." + extension;
        string blobname = FactoryDb.GetWbBlobRepository().GetById(id).Blobname;
        if (!(filename.ToLower() != blobname.ToLower()))
          return 1;
        if (FactoryFile.GetFileManager().ListBlobs(url).Where<FileData>((Func<FileData, bool>) (x => x.FileName == filename)).Count<FileData>() != 0)
          return 2;
        new COES.Storage.App.Servicio.Servicio().RenombrarArchivo(id, url, filename, user);
        FactoryFile.GetFileManager().RenameBlob(url, blobname, filename);
        return 1;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public void DeleteBlob(string blobName) => FactoryFile.GetFileManager().DeleteBlob(blobName);

    public List<FileData> GetBlobListForRelPathType(string relativePath, string user)
    {
      List<FileData> list = FactoryFile.GetFileManager().ListBlobs(relativePath);
      new COES.Storage.App.Servicio.Servicio().VerificarEstructura(ref list, user);
      return list;
    }

    public List<FileData> GetBlobListForRelPathType(
      string relativePath,
      string user,
      int idFuente)
    {
      List<FileData> list = FactoryFile.GetFileManager().ListBlobs(relativePath);
      new COES.Storage.App.Servicio.Servicio().VerificarEstructura(ref list, user, idFuente);
      return list;
    }

    public int CreateFolder(
      int id,
      string url,
      string folderName,
      string user,
      string indicador,
      int? tipoFolder)
    {
      List<FileData> source = FactoryFile.GetFileManager().ListBlobs(url);
      if (id == 0)
      {
        if (source.Where<FileData>((Func<FileData, bool>) (x => x.FileName == folderName)).Count<FileData>() != 0)
          return 2;
        string url1 = url + folderName;
        WbBlobDTO result = new WbBlobDTO();
        new COES.Storage.App.Servicio.Servicio().GenerarCarpeta(url1, user, indicador, tipoFolder, out result);
        return FactoryFile.GetFileManager().CreateFolder(url, folderName) ? 1 : -1;
      }
      string url2 = url + folderName;
      string folderAnterior = new COES.Storage.App.Servicio.Servicio().ActualizarDatosCarpeta(id, url2, user, indicador, tipoFolder, folderName);
      if (!(folderAnterior != folderName))
        return 1;
      if (source.Where<FileData>((Func<FileData, bool>) (x => x.FileName == folderName)).Count<FileData>() != 0)
        return 2;
      return FactoryFile.GetFileManager().RenameFolder(url, folderName, folderAnterior) ? 1 : -1;
    }

    public List<BreadCrumb> ObtenerBreadCrumb(
      string urlBase,
      string url,
      string initialLink)
    {
      List<BreadCrumb> breadCrumbList = new List<BreadCrumb>();
      breadCrumbList.Add(new BreadCrumb()
      {
        Name = !string.IsNullOrEmpty(initialLink) ? initialLink : "Inicio",
        Url = urlBase
      });
      if (urlBase != url)
      {
        string[] strArray = (!string.IsNullOrEmpty(urlBase) ? url.Replace(urlBase, string.Empty) : url).Split('/');
        string str1 = urlBase;
        foreach (string str2 in strArray)
        {
          str1 = str1 + str2 + '/'.ToString();
          BreadCrumb breadCrumb = new BreadCrumb();
          breadCrumb.Name = str2;
          breadCrumb.Url = str1;
          if (!string.IsNullOrEmpty(str2))
            breadCrumbList.Add(breadCrumb);
        }
      }
      return breadCrumbList;
    }

    public int DeleteMultiple(List<string> urls)
    {
      try
      {
        foreach (string url in urls)
        {
          FactoryFile.GetFileManager().DeleteBlob(url);
          new COES.Storage.App.Servicio.Servicio().EliminarBlob(url.Replace("\\", "/"));
        }
        return 1;
      }
      catch
      {
        return -1;
      }
    }

    public void DeleteFolder(string url)
    {
      try
      {
        FactoryFile.GetFileManager().DeleteFolder(url);
        new COES.Storage.App.Servicio.Servicio().EliminarCarpeta(url);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public int PegarBlob(List<string> arreglo, string origen, string destino, string operacion)
    {
      try
      {
        foreach (string str in arreglo)
        {
          string url = origen + str;
          WbBlobDTO blobByUrl = FactoryDb.GetWbBlobRepository().GetBlobByUrl(url);
          if (blobByUrl.Blobtype == "F")
          {
            if (operacion == "C")
            {
              int num = FactoryFile.GetFileManager().CopiarFile(origen, destino, str);
              if (num == 2)
                return num;
            }
            if (operacion == "X")
            {
              int num = FactoryFile.GetFileManager().CortarFile(origen, destino, str);
              if (num == 2)
                return num;
              FactoryDb.GetWbBlobRepository().Delete(blobByUrl.Blobcodi);
            }
          }
          else
          {
            if (operacion == "C")
            {
              int num = FactoryFile.GetFileManager().CopiarDirectory(origen, destino, str);
              if (num == 2)
                return num;
            }
            if (operacion == "X")
            {
              int num = FactoryFile.GetFileManager().CortarDirectory(origen, destino, str);
              if (num == 2)
                return num;
              new COES.Storage.App.Servicio.Servicio().EliminarCarpeta(origen + str);
            }
          }
        }
        return 1;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public List<FileData> HabilitarVistaDatos(
      string url,
      out List<WbBlobcolumnDTO> listColumnas,
      string user,
      out string ordenamiento,
      out string indicadorArbol)
    {
      List<FileData> listForRelPathType = this.GetBlobListForRelPathType(url, user);
      WbBlobDTO blobByUrl = FactoryDb.GetWbBlobRepository().GetBlobByUrl(url);
      ordenamiento = string.Empty;
      indicadorArbol = string.Empty;
      listColumnas = new List<WbBlobcolumnDTO>();
      if (blobByUrl != null)
      {
        WbBlobconfigRepository blobconfigRepository = FactoryDb.GetWbBlobconfigRepository();
        int? nullable = blobByUrl.Configcodi;
        int configcodi = nullable.Value;
        WbBlobconfigDTO byId = blobconfigRepository.GetById(configcodi);
        ordenamiento = byId.Configorder;
        indicadorArbol = blobByUrl.Indtree;
        WbBlobcolumnRepository blobcolumnRepository = FactoryDb.GetWbBlobcolumnRepository();
        nullable = blobByUrl.Configcodi;
        int idConfig = nullable.Value;
        List<WbBlobcolumnDTO> wbBlobcolumnDtoList = blobcolumnRepository.ObtenerColumnasVista(idConfig);
        List<WbBlobmetadataDTO> byCriteria = FactoryDb.GetWbBlobmetadataRepository().GetByCriteria(blobByUrl.Blobcodi);
        foreach (FileData fileData in listForRelPathType)
        {
          FileData item = fileData;
          WbBlobmetadataDTO wbBlobmetadataDto = byCriteria.Where<WbBlobmetadataDTO>((Func<WbBlobmetadataDTO, bool>) (x => x.Blobcodi == item.Blobcodi)).FirstOrDefault<WbBlobmetadataDTO>();
          List<FileMetadato> fileMetadatoList = new List<FileMetadato>();
          foreach (WbBlobcolumnDTO wbBlobcolumnDto in wbBlobcolumnDtoList)
          {
            FileMetadato fileMetadato = new FileMetadato();
            fileMetadato.ColumnCodi = wbBlobcolumnDto.Columncodi;
            string str = string.Empty;
            if (wbBlobcolumnDto.Columnstate == "B" && wbBlobcolumnDto.Columncodi != 6)
            {
              switch (wbBlobcolumnDto.Columncodi)
              {
                case 1:
                  str = item.FirstUser;
                  break;
                case 2:
                  str = item.FirstDate.ToString("dd/MM/yyyy");
                  break;
                case 3:
                  str = item.LastUser;
                  break;
                case 4:
                  str = item.LastDate.ToString("dd/MM/yyyy");
                  break;
                case 5:
                  str = item.FileUrl;
                  break;
                case 7:
                  str = item.FileName;
                  break;
                case 9:
                  str = item.FileSize;
                  break;
              }
            }
            else if (wbBlobmetadataDto != null)
            {
              object obj = wbBlobmetadataDto.GetType().GetProperty(wbBlobcolumnDto.Columnunique).GetValue((object) wbBlobmetadataDto, (object[]) null);
              if (obj != null)
              {
                nullable = wbBlobcolumnDto.Typecodi;
                int num = 5;
                str = nullable.GetValueOrDefault() == num & nullable.HasValue ? Convert.ToDateTime(obj).ToString("dd/MM/yyyy") : obj.ToString();
              }
              if (wbBlobcolumnDto.Columncodi == 6 && string.IsNullOrEmpty(str))
                str = item.FileName;
            }
            else if (wbBlobcolumnDto.Columncodi == 6 && string.IsNullOrEmpty(str))
              str = item.FileName;
            fileMetadato.ColumnValor = str;
            fileMetadatoList.Add(fileMetadato);
          }
          item.Metadatos = fileMetadatoList;
        }
        listColumnas = wbBlobcolumnDtoList;
      }
      return listForRelPathType;
    }

    public List<FileData> HabilitarVistaDatosWeb(
      string url,
      out List<WbBlobcolumnDTO> listColumnas,
      string user,
      out string ordenamiento,
      out string issuuind,
      out string issuulink,
      out string issuupos,
      out string issuulenx,
      out string issuuleny,
      string indicador,
      out bool indHeader,
      out bool indicadorTree)
    {
      List<FileData> listForRelPathType = this.GetBlobListForRelPathType(url, user);
      bool flag = true;
      if (indicador == "S" && listForRelPathType.Where<FileData>((Func<FileData, bool>) (x => x.FileType == "F")).Count<FileData>() == 0)
        flag = false;
      indHeader = flag;
      WbBlobDTO blobByUrl = FactoryDb.GetWbBlobRepository().GetBlobByUrl(url);
      ordenamiento = string.Empty;
      issuuind = blobByUrl.Blobissuu;
      issuulink = blobByUrl.Blobissuulink;
      issuupos = blobByUrl.Blobissuupos;
      issuulenx = blobByUrl.Blobissuulenx;
      issuuleny = blobByUrl.Blobissuuleny;
      indicadorTree = blobByUrl.Indtree == "S";
      listColumnas = new List<WbBlobcolumnDTO>();
      if (blobByUrl != null)
      {
        WbBlobconfigDTO configuracion = FactoryDb.GetWbBlobconfigRepository().GetById(blobByUrl.Configcodi.Value);
        ordenamiento = configuracion.Configorder;
        string configespecial = configuracion.Configespecial;
        List<WbBlobcolumnDTO> source1 = FactoryDb.GetWbBlobcolumnRepository().ObtenerColumnasVista(blobByUrl.Configcodi.Value);
        List<WbBlobcolumnDTO> source2 = flag ? source1 : (!(configespecial == "S") ? source1.Where<WbBlobcolumnDTO>((Func<WbBlobcolumnDTO, bool>) (x => x.Columncodi == 7)).ToList<WbBlobcolumnDTO>() : source1.Where<WbBlobcolumnDTO>((Func<WbBlobcolumnDTO, bool>) (x => x.Columncodi == 6 || x.Columncodi == 7)).ToList<WbBlobcolumnDTO>());
        List<WbBlobmetadataDTO> byCriteria = FactoryDb.GetWbBlobmetadataRepository().GetByCriteria(blobByUrl.Blobcodi);
        foreach (FileData fileData in listForRelPathType)
        {
          FileData item = fileData;
          WbBlobmetadataDTO wbBlobmetadataDto = byCriteria.Where<WbBlobmetadataDTO>((Func<WbBlobmetadataDTO, bool>) (x => x.Blobcodi == item.Blobcodi)).FirstOrDefault<WbBlobmetadataDTO>();
          List<FileMetadato> source3 = new List<FileMetadato>();
          item.Columnorder = (object) item.FileName;
          foreach (WbBlobcolumnDTO wbBlobcolumnDto in source2)
          {
            FileMetadato fileMetadato = new FileMetadato();
            fileMetadato.ColumnCodi = wbBlobcolumnDto.Columncodi;
            string str = string.Empty;
            DateTime? nullable = new DateTime?();
            if (wbBlobcolumnDto.Columnstate == "B" && wbBlobcolumnDto.Columncodi != 6)
            {
              switch (wbBlobcolumnDto.Columncodi)
              {
                case 1:
                  str = item.FirstUser;
                  break;
                case 2:
                  str = item.FirstDate.ToString("dd/MM/yyyy");
                  nullable = new DateTime?(item.FirstDate);
                  break;
                case 3:
                  str = item.LastUser;
                  break;
                case 4:
                  str = item.LastDate.ToString("dd/MM/yyyy");
                  nullable = new DateTime?(item.LastDate);
                  break;
                case 5:
                  str = item.FileUrl;
                  break;
                case 7:
                  str = item.FileName;
                  break;
                case 9:
                  str = item.FileSize;
                  break;
              }
            }
            else if (wbBlobmetadataDto != null)
            {
              object obj = wbBlobmetadataDto.GetType().GetProperty(wbBlobcolumnDto.Columnunique).GetValue((object) wbBlobmetadataDto, (object[]) null);
              if (obj != null)
              {
                int? typecodi = wbBlobcolumnDto.Typecodi;
                int num = 5;
                if (!(typecodi.GetValueOrDefault() == num & typecodi.HasValue))
                {
                  str = obj.ToString();
                }
                else
                {
                  str = Convert.ToDateTime(obj).ToString("dd/MM/yyyy");
                  nullable = new DateTime?(Convert.ToDateTime(obj));
                }
              }
              if (wbBlobcolumnDto.Columncodi == 6 && string.IsNullOrEmpty(str))
                str = item.FileName;
            }
            else if (wbBlobcolumnDto.Columncodi == 6 && string.IsNullOrEmpty(str))
              str = item.FileName;
            fileMetadato.ColumnValor = str;
            fileMetadato.Fecha = nullable;
            source3.Add(fileMetadato);
          }
          item.Metadatos = source3;
          if (configuracion.Columncodi.HasValue & indHeader)
          {
            var data = source3.Where<FileMetadato>((Func<FileMetadato, bool>) (x => x.ColumnCodi == configuracion.Columncodi.Value)).Select(x => new
            {
              ColumnValor = x.ColumnValor,
              Fecha = x.Fecha
            }).FirstOrDefault();
            if (data != null)
            {
              DateTime? fecha = data.Fecha;
              item.Columnorder = !fecha.HasValue ? (object) data.ColumnValor : (object) data.Fecha;
            }
          }
        }
        listColumnas = source2;
        if (configespecial == "S")
        {
          foreach (FileData fileData in listForRelPathType)
          {
            FileMetadato fileMetadato1 = fileData.Metadatos.Where<FileMetadato>((Func<FileMetadato, bool>) (x => x.ColumnCodi == 7)).FirstOrDefault<FileMetadato>();
            FileMetadato fileMetadato2 = fileData.Metadatos.Where<FileMetadato>((Func<FileMetadato, bool>) (x => x.ColumnCodi == 6)).FirstOrDefault<FileMetadato>();
            if (fileMetadato2 != null && !string.IsNullOrEmpty(fileMetadato2.ColumnValor))
              fileMetadato1.ColumnValor = fileMetadato2.ColumnValor;
            fileData.Columnorder = (object) fileMetadato1.ColumnValor;
          }
          WbBlobcolumnDTO wbBlobcolumnDto = source2.Where<WbBlobcolumnDTO>((Func<WbBlobcolumnDTO, bool>) (x => x.Columncodi == 6)).FirstOrDefault<WbBlobcolumnDTO>();
          if (wbBlobcolumnDto != null)
            source2.Remove(wbBlobcolumnDto);
        }
      }
      return listForRelPathType;
    }

    public List<FileData> HabilitarVistaDatosWeb(
      string url,
      out List<WbBlobcolumnDTO> listColumnas,
      string user,
      out string ordenamiento,
      out string issuuind,
      out string issuulink,
      out string issuupos,
      out string issuulenx,
      out string issuuleny,
      string indicador,
      out bool indHeader,
      out bool indicadorTree,
      int idFuente)
    {
      List<FileData> listForRelPathType = this.GetBlobListForRelPathType(url, user, idFuente);
      bool flag = true;
      if (indicador == "S" && listForRelPathType.Where<FileData>((Func<FileData, bool>) (x => x.FileType == "F")).Count<FileData>() == 0)
        flag = false;
      indHeader = flag;
      WbBlobDTO blobByUrl = FactoryDb.GetWbBlobRepository().GetBlobByUrl(url, idFuente);
      ordenamiento = string.Empty;
      issuuind = blobByUrl.Blobissuu;
      issuulink = blobByUrl.Blobissuulink;
      issuupos = blobByUrl.Blobissuupos;
      issuulenx = blobByUrl.Blobissuulenx;
      issuuleny = blobByUrl.Blobissuuleny;
      indicadorTree = blobByUrl.Indtree == "S";
      listColumnas = new List<WbBlobcolumnDTO>();
      if (blobByUrl != null)
      {
        WbBlobconfigRepository blobconfigRepository = FactoryDb.GetWbBlobconfigRepository();
        int? nullable1 = blobByUrl.Configcodi;
        int configcodi = nullable1.Value;
        WbBlobconfigDTO configuracion = blobconfigRepository.GetById(configcodi);
        ordenamiento = configuracion.Configorder;
        string configespecial = configuracion.Configespecial;
        WbBlobcolumnRepository blobcolumnRepository = FactoryDb.GetWbBlobcolumnRepository();
        nullable1 = blobByUrl.Configcodi;
        int idConfig = nullable1.Value;
        List<WbBlobcolumnDTO> wbBlobcolumnDtoList = blobcolumnRepository.ObtenerColumnasVista(idConfig);
        this.ReemplazarCodigosColumnas(wbBlobcolumnDtoList);
        List<WbBlobcolumnDTO> source1 = flag ? wbBlobcolumnDtoList : (!(configespecial == "S") ? wbBlobcolumnDtoList.Where<WbBlobcolumnDTO>((Func<WbBlobcolumnDTO, bool>) (x => x.Columncodi == 7)).ToList<WbBlobcolumnDTO>() : wbBlobcolumnDtoList.Where<WbBlobcolumnDTO>((Func<WbBlobcolumnDTO, bool>) (x => x.Columncodi == 6 || x.Columncodi == 7)).ToList<WbBlobcolumnDTO>());
        List<WbBlobmetadataDTO> byCriteria = FactoryDb.GetWbBlobmetadataRepository().GetByCriteria(blobByUrl.Blobcodi);
        foreach (FileData fileData in listForRelPathType)
        {
          FileData item = fileData;
          WbBlobmetadataDTO wbBlobmetadataDto = byCriteria.Where<WbBlobmetadataDTO>((Func<WbBlobmetadataDTO, bool>) (x => x.Blobcodi == item.Blobcodi)).FirstOrDefault<WbBlobmetadataDTO>();
          List<FileMetadato> source2 = new List<FileMetadato>();
          item.Columnorder = (object) item.FileName;
          foreach (WbBlobcolumnDTO wbBlobcolumnDto in source1)
          {
            FileMetadato fileMetadato = new FileMetadato();
            fileMetadato.ColumnCodi = wbBlobcolumnDto.Columncodi;
            string str = string.Empty;
            DateTime? nullable2 = new DateTime?();
            if (wbBlobcolumnDto.Columnstate == "B" && wbBlobcolumnDto.Columncodi != 6)
            {
              switch (wbBlobcolumnDto.Columncodi)
              {
                case 1:
                  str = item.FirstUser;
                  break;
                case 2:
                  str = item.FirstDate.ToString("dd/MM/yyyy");
                  nullable2 = new DateTime?(item.FirstDate);
                  break;
                case 3:
                  str = item.LastUser;
                  break;
                case 4:
                  str = item.LastDate.ToString("dd/MM/yyyy");
                  nullable2 = new DateTime?(item.LastDate);
                  break;
                case 5:
                  str = item.FileUrl;
                  break;
                case 7:
                  str = item.FileName;
                  break;
                case 9:
                  str = item.FileSize;
                  break;
              }
            }
            else if (wbBlobmetadataDto != null)
            {
              var propertyInfo = wbBlobmetadataDto.GetType().GetProperty(wbBlobcolumnDto.Columnunique);
              if (propertyInfo != null) {
                 object obj = wbBlobmetadataDto.GetType().GetProperty(wbBlobcolumnDto.Columnunique).GetValue((object) wbBlobmetadataDto, (object[]) null);
                  if (obj != null)
                  {
                    nullable1 = wbBlobcolumnDto.Typecodi;
                    int num = 5;
                    if (!(nullable1.GetValueOrDefault() == num & nullable1.HasValue))
                    {
                      str = obj.ToString();
                    }
                    else
                    {
                      str = Convert.ToDateTime(obj).ToString("dd/MM/yyyy");
                      nullable2 = new DateTime?(Convert.ToDateTime(obj));
                    }
                  }           
              }
              
              if (wbBlobcolumnDto.Columncodi == 6 && string.IsNullOrEmpty(str))
                str = item.FileName;
            }
            else if (wbBlobcolumnDto.Columncodi == 6 && string.IsNullOrEmpty(str))
              str = item.FileName;
            fileMetadato.ColumnValor = str;
            fileMetadato.Fecha = nullable2;
            source2.Add(fileMetadato);
          }
          item.Metadatos = source2;
          if (configuracion.Columncodi.HasValue & indHeader)
          {
            var data = source2.Where<FileMetadato>((Func<FileMetadato, bool>) (x => x.ColumnCodi == configuracion.Columncodi.Value)).Select(x => new
            {
              ColumnValor = x.ColumnValor,
              Fecha = x.Fecha
            }).FirstOrDefault();
            if (data != null)
            {
              DateTime? fecha = data.Fecha;
              item.Columnorder = !fecha.HasValue ? (object) data.ColumnValor : (object) data.Fecha;
            }
          }
        }
        listColumnas = source1;
        if (configespecial == "S")
        {
          foreach (FileData fileData in listForRelPathType)
          {
            FileMetadato fileMetadato1 = fileData.Metadatos.Where<FileMetadato>((Func<FileMetadato, bool>) (x => x.ColumnCodi == 7)).FirstOrDefault<FileMetadato>();
            FileMetadato fileMetadato2 = fileData.Metadatos.Where<FileMetadato>((Func<FileMetadato, bool>) (x => x.ColumnCodi == 6)).FirstOrDefault<FileMetadato>();
            if (fileMetadato2 != null && !string.IsNullOrEmpty(fileMetadato2.ColumnValor))
              fileMetadato1.ColumnValor = fileMetadato2.ColumnValor;
            if (blobByUrl.Blobcodi != 92499)
              fileData.Columnorder = (object) fileMetadato1.ColumnValor;
          }
          WbBlobcolumnDTO wbBlobcolumnDto = source1.Where<WbBlobcolumnDTO>((Func<WbBlobcolumnDTO, bool>) (x => x.Columncodi == 6)).FirstOrDefault<WbBlobcolumnDTO>();
          if (wbBlobcolumnDto != null)
            source1.Remove(wbBlobcolumnDto);
          if (blobByUrl.Blobcodi == 92499)
            source1.Where<WbBlobcolumnDTO>((Func<WbBlobcolumnDTO, bool>) (x => x.Columncodi == 7)).FirstOrDefault<WbBlobcolumnDTO>().Columnshow = "Nombre / Descripción";
        }
      }
      return listForRelPathType;
    }

    public void ReemplazarCodigosColumnas(List<WbBlobcolumnDTO> list)
    {
      foreach (WbBlobcolumnDTO wbBlobcolumnDto in list)
      {
        if (wbBlobcolumnDto.Columncodi == 55)
          wbBlobcolumnDto.Columncodi = 1;
        if (wbBlobcolumnDto.Columncodi == 56)
          wbBlobcolumnDto.Columncodi = 4;
        if (wbBlobcolumnDto.Columncodi == 57)
          wbBlobcolumnDto.Columncodi = 5;
        if (wbBlobcolumnDto.Columncodi == 58)
          wbBlobcolumnDto.Columncodi = 6;
        if (wbBlobcolumnDto.Columncodi == 59)
          wbBlobcolumnDto.Columncodi = 7;
        if (wbBlobcolumnDto.Columncodi == 61)
          wbBlobcolumnDto.Columncodi = 1;
        if (wbBlobcolumnDto.Columncodi == 62)
          wbBlobcolumnDto.Columncodi = 4;
        if (wbBlobcolumnDto.Columncodi == 63)
          wbBlobcolumnDto.Columncodi = 5;
        if (wbBlobcolumnDto.Columncodi == 64)
          wbBlobcolumnDto.Columncodi = 6;
        if (wbBlobcolumnDto.Columncodi == 65)
          wbBlobcolumnDto.Columncodi = 7;
      }
    }

    public List<FileData> EditarMetadatosLibreria(
      string url,
      out List<WbBlobcolumnDTO> listColumnas,
      string user)
    {
      List<FileData> listForRelPathType = this.GetBlobListForRelPathType(url, user);
      WbBlobDTO blobByUrl = FactoryDb.GetWbBlobRepository().GetBlobByUrl(url);
      listColumnas = new List<WbBlobcolumnDTO>();
      if (blobByUrl != null)
      {
        WbBlobcolumnRepository blobcolumnRepository = FactoryDb.GetWbBlobcolumnRepository();
        int? nullable = blobByUrl.Configcodi;
        int idConfig = nullable.Value;
        List<WbBlobcolumnDTO> wbBlobcolumnDtoList = blobcolumnRepository.ObtenerColumnasVista(idConfig);
        foreach (WbBlobcolumnDTO wbBlobcolumnDto in wbBlobcolumnDtoList)
        {
          nullable = wbBlobcolumnDto.Typecodi;
          if (nullable.Value == 6)
            wbBlobcolumnDto.ListaItems = FactoryDb.GetWbColumnitemRepository().GetByCriteria(wbBlobcolumnDto.Columncodi);
        }
        List<WbBlobmetadataDTO> byCriteria = FactoryDb.GetWbBlobmetadataRepository().GetByCriteria(blobByUrl.Blobcodi);
        List<FileMetadato> source = new List<FileMetadato>();
        foreach (FileData fileData in listForRelPathType)
        {
          FileData item = fileData;
          WbBlobmetadataDTO wbBlobmetadataDto = byCriteria.Where<WbBlobmetadataDTO>((Func<WbBlobmetadataDTO, bool>) (x => x.Blobcodi == item.Blobcodi)).FirstOrDefault<WbBlobmetadataDTO>();
          List<FileMetadato> fileMetadatoList = new List<FileMetadato>();
          foreach (WbBlobcolumnDTO wbBlobcolumnDto in wbBlobcolumnDtoList)
          {
            FileMetadato fileMetadato = new FileMetadato();
            fileMetadato.ColumnCodi = wbBlobcolumnDto.Columncodi;
            wbBlobcolumnDto.ReadOnly = false;
            string str = string.Empty;
            if (wbBlobcolumnDto.Columnstate == "B" && wbBlobcolumnDto.Columncodi != 6)
            {
              switch (wbBlobcolumnDto.Columncodi)
              {
                case 1:
                  str = item.FirstUser;
                  break;
                case 2:
                  str = item.FirstDate.ToString("dd/MM/yyyy");
                  break;
                case 3:
                  str = item.LastUser;
                  break;
                case 4:
                  str = item.LastDate.ToString("dd/MM/yyyy");
                  break;
                case 5:
                  str = item.FileUrl;
                  break;
                case 7:
                  str = item.FileName;
                  break;
                case 9:
                  str = item.FileSize;
                  break;
              }
              wbBlobcolumnDto.ReadOnly = true;
            }
            else if (wbBlobmetadataDto != null)
            {
              object obj = wbBlobmetadataDto.GetType().GetProperty(wbBlobcolumnDto.Columnunique).GetValue((object) wbBlobmetadataDto, (object[]) null);
              if (obj != null)
              {
                int? typecodi = wbBlobcolumnDto.Typecodi;
                int num = 5;
                str = typecodi.GetValueOrDefault() == num & typecodi.HasValue ? Convert.ToDateTime(obj).ToString("dd/MM/yyyy") : obj.ToString();
              }
              if (wbBlobcolumnDto.Columncodi == 6 && string.IsNullOrEmpty(str))
                str = item.FileName;
            }
            else if (wbBlobcolumnDto.Columncodi == 6 && string.IsNullOrEmpty(str))
              str = item.FileName;
            fileMetadato.ColumnValor = str;
            fileMetadatoList.Add(fileMetadato);
            if (!string.IsNullOrEmpty(str))
              fileMetadato.Longitud = str.Length;
            source.Add(fileMetadato);
          }
          item.Metadatos = fileMetadatoList;
        }
        foreach (WbBlobcolumnDTO wbBlobcolumnDto1 in wbBlobcolumnDtoList)
        {
          WbBlobcolumnDTO columna = wbBlobcolumnDto1;
          WbBlobcolumnDTO wbBlobcolumnDto2 = columna;
          int? typecodi = columna.Typecodi;
          string str1 = Helper.ObtenerTipoCelda(typecodi.Value);
          wbBlobcolumnDto2.Type = str1;
          columna.Lista = columna.ListaItems != null ? columna.ListaItems.Select<WbColumnitemDTO, string>((Func<WbColumnitemDTO, string>) (x => x.Itemvalue)).OrderBy<string, string>((Func<string, string>) (x => x)).ToArray<string>() : new List<string>().ToArray();
          WbBlobcolumnDTO wbBlobcolumnDto3 = columna;
          typecodi = columna.Typecodi;
          int num1 = 5;
          string str2 = typecodi.GetValueOrDefault() == num1 & typecodi.HasValue ? "DD/MM/YYYY" : string.Empty;
          wbBlobcolumnDto3.Dateformat = str2;
          WbBlobcolumnDTO wbBlobcolumnDto4 = columna;
          typecodi = columna.Typecodi;
          int num2 = 5;
          string str3 = typecodi.GetValueOrDefault() == num2 & typecodi.HasValue ? DateTime.Now.ToString("dd/MM/yyyy") : string.Empty;
          wbBlobcolumnDto4.Defaultdate = str3;
          WbBlobcolumnDTO wbBlobcolumnDto5 = columna;
          typecodi = columna.Typecodi;
          int num3 = 2;
          string str4 = typecodi.GetValueOrDefault() == num3 & typecodi.HasValue ? "0,0.00" : string.Empty;
          wbBlobcolumnDto5.Format = str4;
          typecodi = columna.Typecodi;
          int num4 = 7;
          if (!(typecodi.GetValueOrDefault() == num4 & typecodi.HasValue))
          {
            columna.Width = source.Where<FileMetadato>((Func<FileMetadato, bool>) (x => x.ColumnCodi == columna.Columncodi)).Max<FileMetadato>((Func<FileMetadato, int>) (x => x.Longitud));
            if (columna.Width < columna.Columnshow.Length)
              columna.Width = columna.Columnshow.Length;
            typecodi = columna.Typecodi;
            int num5 = 6;
            if (typecodi.GetValueOrDefault() == num5 & typecodi.HasValue)
            {
              int num6 = ((IEnumerable<string>) columna.Lista).Select<string, int>((Func<string, int>) (x => x.Length)).Max();
              if (columna.Width < num6)
                columna.Width = num6;
            }
            columna.Width *= 10;
          }
          else
            columna.Width = 500;
        }
        listColumnas = wbBlobcolumnDtoList;
      }
      return listForRelPathType;
    }

    public int GrabarMetadata(string baseDirectory, string url, string[][] datos)
    {
      try
      {
        WbBlobDTO blobByUrl = FactoryDb.GetWbBlobRepository().GetBlobByUrl(url);
        List<WbBlobcolumnDTO> wbBlobcolumnDtoList = FactoryDb.GetWbBlobcolumnRepository().ObtenerColumnasVista(blobByUrl.Configcodi.Value);
        List<WbBlobDTO> wbBlobDtoList = FactoryDb.GetWbBlobRepository().ObtenerPorPadre(blobByUrl.Blobcodi);
        string[][] arreglo = datos;
        foreach (WbBlobDTO wbBlobDto in wbBlobDtoList)
        {
          List<BlobDatoDTO> list = new List<BlobDatoDTO>();
          List<string> stringList = this.ObtenerFilaDatos(arreglo, wbBlobDto.Blobname);
          if (stringList.Count > 0)
          {
            int index = 0;
            foreach (WbBlobcolumnDTO wbBlobcolumnDto in wbBlobcolumnDtoList)
            {
              if (!(wbBlobcolumnDto.Columnstate == "B") || wbBlobcolumnDto.Columncodi == 6)
                list.Add(new BlobDatoDTO()
                {
                  Valor = stringList[index],
                  Campo = wbBlobcolumnDto.Columnunique,
                  TipoDato = wbBlobcolumnDto.Typecodi.Value
                });
              ++index;
            }
            new COES.Storage.App.Servicio.Servicio().GrabarMetadato(wbBlobDto.Blobcodi, list);
          }
        }
        return 1;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public List<string> ObtenerFilaDatos(string[][] arreglo, string fileName)
    {
      for (int index = 0; index < arreglo.Length; ++index)
      {
        if ((uint) arreglo[index].Length > 0U && arreglo[index][0].ToLower() == fileName.ToLower())
          return ((IEnumerable<string>) arreglo[index]).ToList<string>();
      }
      return new List<string>();
    }

    public string[][] ObtenerDatos(string datos, int nroColumnas)
    {
      List<string> list = ((IEnumerable<string>) datos.Split(Convert.ToChar(","))).ToList<string>();
      int length = list.Count / nroColumnas;
      string[][] strArray = new string[length][];
      for (int index1 = 0; index1 < length; ++index1)
      {
        strArray[index1] = new string[nroColumnas];
        for (int index2 = 0; index2 < nroColumnas; ++index2)
          strArray[index1][index2] = list[index1 * nroColumnas + index2];
      }
      return strArray;
    }

    public int ObtenerNroRegistrosBusqueda(string url, List<DatoItem> datos)
    {
      int num = 0;
      WbBlobDTO blobByUrl = FactoryDb.GetWbBlobRepository().GetBlobByUrl(url);
      if (blobByUrl != null)
      {
        List<WbBlobcolumnDTO> source = FactoryDb.GetWbBlobcolumnRepository().ObtenerColumnasVista(blobByUrl.Configcodi.Value);
        foreach (DatoItem dato in datos)
        {
          DatoItem item = dato;
          item.Columna = source.Where<WbBlobcolumnDTO>((Func<WbBlobcolumnDTO, bool>) (x => x.Columncodi == item.Id)).Select<WbBlobcolumnDTO, string>((Func<WbBlobcolumnDTO, string>) (x => x.Columnunique)).FirstOrDefault<string>();
          item.TipoDato = source.Where<WbBlobcolumnDTO>((Func<WbBlobcolumnDTO, bool>) (x => x.Columncodi == item.Id)).Select<WbBlobcolumnDTO, int?>((Func<WbBlobcolumnDTO, int?>) (x => x.Typecodi)).FirstOrDefault<int?>().Value;
        }
        num = FactoryDb.GetWbBlobRepository().ObtenerNroRegistroBusqueda(url, datos);
      }
      return num;
    }

    public int ObtenerNroRegistrosBusqueda(string url, List<DatoItem> datos, int idFuente)
    {
      int num = 0;
      WbBlobDTO blobByUrl = FactoryDb.GetWbBlobRepository().GetBlobByUrl(url, idFuente);
      if (blobByUrl != null)
      {
        List<WbBlobcolumnDTO> source = FactoryDb.GetWbBlobcolumnRepository().ObtenerColumnasVista(blobByUrl.Configcodi.Value);
        foreach (DatoItem dato in datos)
        {
          DatoItem item = dato;
          item.Columna = source.Where<WbBlobcolumnDTO>((Func<WbBlobcolumnDTO, bool>) (x => x.Columncodi == item.Id)).Select<WbBlobcolumnDTO, string>((Func<WbBlobcolumnDTO, string>) (x => x.Columnunique)).FirstOrDefault<string>();
          item.TipoDato = source.Where<WbBlobcolumnDTO>((Func<WbBlobcolumnDTO, bool>) (x => x.Columncodi == item.Id)).Select<WbBlobcolumnDTO, int?>((Func<WbBlobcolumnDTO, int?>) (x => x.Typecodi)).FirstOrDefault<int?>().Value;
        }
        num = FactoryDb.GetWbBlobRepository().ObtenerNroRegistroBusqueda(url, datos, idFuente);
      }
      return num;
    }

    public List<FileData> BuscarArchivos(
      string url,
      out List<WbBlobcolumnDTO> listColumnas,
      List<DatoItem> datos,
      int nroPagina,
      int pageSize)
    {
      List<FileData> fileDataList = new List<FileData>();
      WbBlobDTO blobByUrl = FactoryDb.GetWbBlobRepository().GetBlobByUrl(url);
      listColumnas = new List<WbBlobcolumnDTO>();
      if (blobByUrl != null)
      {
        List<WbBlobcolumnDTO> source1 = FactoryDb.GetWbBlobcolumnRepository().ObtenerColumnasVista(blobByUrl.Configcodi.Value);
        WbBlobconfigRepository blobconfigRepository = FactoryDb.GetWbBlobconfigRepository();
        int? nullable = blobByUrl.Configcodi;
        int configcodi = nullable.Value;
        string configespecial = blobconfigRepository.GetById(configcodi).Configespecial;
        foreach (DatoItem dato in datos)
        {
          DatoItem item = dato;
          item.Columna = source1.Where<WbBlobcolumnDTO>((Func<WbBlobcolumnDTO, bool>) (x => x.Columncodi == item.Id)).Select<WbBlobcolumnDTO, string>((Func<WbBlobcolumnDTO, string>) (x => x.Columnunique)).FirstOrDefault<string>();
          DatoItem datoItem = item;
          nullable = source1.Where<WbBlobcolumnDTO>((Func<WbBlobcolumnDTO, bool>) (x => x.Columncodi == item.Id)).Select<WbBlobcolumnDTO, int?>((Func<WbBlobcolumnDTO, int?>) (x => x.Typecodi)).FirstOrDefault<int?>();
          int num = nullable.Value;
          datoItem.TipoDato = num;
        }
        foreach (WbBlobDTO wbBlobDto in FactoryDb.GetWbBlobRepository().BuscarArchivos(url, datos).Skip<WbBlobDTO>(pageSize * (nroPagina - 1)).Take<WbBlobDTO>(pageSize).ToList<WbBlobDTO>())
        {
          FileData fileData = FactoryFile.GetFileManager().ObtenerFile(wbBlobDto.Bloburl);
          if (fileData != null)
          {
            WbBlobmetadataDTO wbBlobmetadataDto = FactoryDb.GetWbBlobmetadataRepository().ObtenerMetadato(wbBlobDto.Blobcodi);
            List<FileMetadato> source2 = new List<FileMetadato>();
            foreach (WbBlobcolumnDTO wbBlobcolumnDto in source1)
            {
              FileMetadato fileMetadato = new FileMetadato();
              fileMetadato.ColumnCodi = wbBlobcolumnDto.Columncodi;
              string str = string.Empty;
              if (wbBlobcolumnDto.Columnstate == "B" && wbBlobcolumnDto.Columncodi != 6)
              {
                switch (wbBlobcolumnDto.Columncodi)
                {
                  case 1:
                    str = fileData.FirstUser;
                    break;
                  case 2:
                    str = fileData.FirstDate.ToString("dd/MM/yyyy");
                    break;
                  case 3:
                    str = fileData.LastUser;
                    break;
                  case 4:
                    str = fileData.LastDate.ToString("dd/MM/yyyy");
                    break;
                  case 5:
                    str = fileData.FileUrl;
                    break;
                  case 7:
                    str = fileData.FileName;
                    break;
                  case 9:
                    str = fileData.FileSize;
                    break;
                }
              }
              else if (wbBlobmetadataDto != null)
              {
                object obj = wbBlobmetadataDto.GetType().GetProperty(wbBlobcolumnDto.Columnunique).GetValue((object) wbBlobmetadataDto, (object[]) null);
                if (obj != null)
                {
                  int? typecodi = wbBlobcolumnDto.Typecodi;
                  int num = 5;
                  str = typecodi.GetValueOrDefault() == num & typecodi.HasValue ? Convert.ToDateTime(obj).ToString("dd/MM/yyyy") : obj.ToString();
                }
                if (wbBlobcolumnDto.Columncodi == 6 && string.IsNullOrEmpty(str))
                  str = fileData.FileName;
              }
              else if (wbBlobcolumnDto.Columncodi == 6 && string.IsNullOrEmpty(str))
                str = fileData.FileName;
              fileMetadato.ColumnValor = str;
              source2.Add(fileMetadato);
            }
            if (configespecial == "S")
            {
              FileMetadato fileMetadato1 = source2.Where<FileMetadato>((Func<FileMetadato, bool>) (x => x.ColumnCodi == 7)).FirstOrDefault<FileMetadato>();
              FileMetadato fileMetadato2 = source2.Where<FileMetadato>((Func<FileMetadato, bool>) (x => x.ColumnCodi == 6)).FirstOrDefault<FileMetadato>();
              if (fileMetadato2 != null && !string.IsNullOrEmpty(fileMetadato2.ColumnValor))
                fileMetadato1.ColumnValor = fileMetadato2.ColumnValor;
            }
            fileData.Metadatos = source2;
            fileDataList.Add(fileData);
          }
        }
        if (configespecial == "S")
        {
          WbBlobcolumnDTO wbBlobcolumnDto = source1.Where<WbBlobcolumnDTO>((Func<WbBlobcolumnDTO, bool>) (x => x.Columncodi == 6)).FirstOrDefault<WbBlobcolumnDTO>();
          if (wbBlobcolumnDto != null)
            source1.Remove(wbBlobcolumnDto);
        }
        listColumnas = source1;
      }
      return fileDataList;
    }

    public List<FileData> BuscarArchivos(
      string url,
      out List<WbBlobcolumnDTO> listColumnas,
      List<DatoItem> datos,
      int nroPagina,
      int pageSize,
      int idFuente)
    {
      List<FileData> fileDataList = new List<FileData>();
      WbBlobDTO blobByUrl = FactoryDb.GetWbBlobRepository().GetBlobByUrl(url, idFuente);
      listColumnas = new List<WbBlobcolumnDTO>();
      if (blobByUrl != null)
      {
        List<WbBlobcolumnDTO> source1 = FactoryDb.GetWbBlobcolumnRepository().ObtenerColumnasVista(blobByUrl.Configcodi.Value);
        string configespecial = FactoryDb.GetWbBlobconfigRepository().GetById(blobByUrl.Configcodi.Value).Configespecial;
        foreach (DatoItem dato in datos)
        {
          DatoItem item = dato;
          item.Columna = source1.Where<WbBlobcolumnDTO>((Func<WbBlobcolumnDTO, bool>) (x => x.Columncodi == item.Id)).Select<WbBlobcolumnDTO, string>((Func<WbBlobcolumnDTO, string>) (x => x.Columnunique)).FirstOrDefault<string>();
          item.TipoDato = source1.Where<WbBlobcolumnDTO>((Func<WbBlobcolumnDTO, bool>) (x => x.Columncodi == item.Id)).Select<WbBlobcolumnDTO, int?>((Func<WbBlobcolumnDTO, int?>) (x => x.Typecodi)).FirstOrDefault<int?>().Value;
        }
        foreach (WbBlobDTO wbBlobDto in FactoryDb.GetWbBlobRepository().BuscarArchivos(url, datos, idFuente).Skip<WbBlobDTO>(pageSize * (nroPagina - 1)).Take<WbBlobDTO>(pageSize).ToList<WbBlobDTO>())
        {
          FileData fileData = FactoryFile.GetFileManager().ObtenerFile(wbBlobDto.Bloburl);
          if (fileData != null)
          {
            WbBlobmetadataDTO wbBlobmetadataDto = FactoryDb.GetWbBlobmetadataRepository().ObtenerMetadato(wbBlobDto.Blobcodi);
            List<FileMetadato> source2 = new List<FileMetadato>();
            foreach (WbBlobcolumnDTO wbBlobcolumnDto in source1)
            {
              FileMetadato fileMetadato = new FileMetadato();
              fileMetadato.ColumnCodi = wbBlobcolumnDto.Columncodi;
              string str = string.Empty;
              if (wbBlobcolumnDto.Columnstate == "B" && wbBlobcolumnDto.Columncodi != 6)
              {
                switch (wbBlobcolumnDto.Columncodi)
                {
                  case 1:
                    str = fileData.FirstUser;
                    break;
                  case 2:
                    str = fileData.FirstDate.ToString("dd/MM/yyyy");
                    break;
                  case 3:
                    str = fileData.LastUser;
                    break;
                  case 4:
                    str = fileData.LastDate.ToString("dd/MM/yyyy");
                    break;
                  case 5:
                    str = fileData.FileUrl;
                    break;
                  case 7:
                    str = fileData.FileName;
                    break;
                  case 9:
                    str = fileData.FileSize;
                    break;
                }
              }
              else if (wbBlobmetadataDto != null)
              {
                object obj = wbBlobmetadataDto.GetType().GetProperty(wbBlobcolumnDto.Columnunique).GetValue((object) wbBlobmetadataDto, (object[]) null);
                if (obj != null)
                {
                  int? typecodi = wbBlobcolumnDto.Typecodi;
                  int num = 5;
                  str = typecodi.GetValueOrDefault() == num & typecodi.HasValue ? Convert.ToDateTime(obj).ToString("dd/MM/yyyy") : obj.ToString();
                }
                if (wbBlobcolumnDto.Columncodi == 6 && string.IsNullOrEmpty(str))
                  str = fileData.FileName;
              }
              else if (wbBlobcolumnDto.Columncodi == 6 && string.IsNullOrEmpty(str))
                str = fileData.FileName;
              fileMetadato.ColumnValor = str;
              source2.Add(fileMetadato);
            }
            if (configespecial == "S")
            {
              FileMetadato fileMetadato1 = source2.Where<FileMetadato>((Func<FileMetadato, bool>) (x => x.ColumnCodi == 7)).FirstOrDefault<FileMetadato>();
              FileMetadato fileMetadato2 = source2.Where<FileMetadato>((Func<FileMetadato, bool>) (x => x.ColumnCodi == 6)).FirstOrDefault<FileMetadato>();
              if (fileMetadato2 != null && !string.IsNullOrEmpty(fileMetadato2.ColumnValor))
                fileMetadato1.ColumnValor = fileMetadato2.ColumnValor;
            }
            fileData.Metadatos = source2;
            fileDataList.Add(fileData);
          }
        }
        if (configespecial == "S")
        {
          WbBlobcolumnDTO wbBlobcolumnDto = source1.Where<WbBlobcolumnDTO>((Func<WbBlobcolumnDTO, bool>) (x => x.Columncodi == 6)).FirstOrDefault<WbBlobcolumnDTO>();
          if (wbBlobcolumnDto != null)
            source1.Remove(wbBlobcolumnDto);
        }
        listColumnas = source1;
      }
      return fileDataList;
    }

    public int ObtenerNroRegistroBusquedaPortal(string texto, string extension)
    {
      int[] numArray = new int[4]{ 3, 4, 6, 7 };
      List<WbColumntypeDTO> byCriteria = FactoryDb.GetWbColumntypeRepository().GetByCriteria(((IEnumerable<int>) numArray).ToList<int>());
      return FactoryDb.GetWbBlobRepository().ObtenerNroRegistrosConsultaPortal(texto, extension, byCriteria);
    }

    public int ObtenerNroRegistroBusquedaPortal(string texto, string extension, int idFuente)
    {
      int[] numArray = new int[4]{ 3, 4, 6, 7 };
      List<WbColumntypeDTO> byCriteria = FactoryDb.GetWbColumntypeRepository().GetByCriteria(((IEnumerable<int>) numArray).ToList<int>());
      return FactoryDb.GetWbBlobRepository().ObtenerNroRegistrosConsultaPortal(texto, extension, byCriteria, idFuente);
    }

    public List<FileData> BusquedaPortal(
      string texto,
      string extension,
      int nroPagina,
      int pageSize)
    {
      int[] numArray = new int[4]{ 3, 4, 6, 7 };
      List<WbColumntypeDTO> byCriteria = FactoryDb.GetWbColumntypeRepository().GetByCriteria(((IEnumerable<int>) numArray).ToList<int>());
      List<WbBlobDTO> wbBlobDtoList = FactoryDb.GetWbBlobRepository().BuscarArchivosPortal(texto, extension, byCriteria, nroPagina, pageSize);
      List<FileData> fileDataList = new List<FileData>();
      foreach (WbBlobDTO wbBlobDto in wbBlobDtoList)
      {
        FileData fileData = FactoryFile.GetFileManager().ObtenerFile(wbBlobDto.Bloburl);
        if (fileData != null)
        {
          fileData.Metadata = wbBlobDto.Metadata;
          fileDataList.Add(fileData);
        }
      }
      return fileDataList;
    }

    public List<FileData> BusquedaPortal(
      string texto,
      string extension,
      int nroPagina,
      int pageSize,
      int idFuente)
    {
      int[] numArray = new int[4]{ 3, 4, 6, 7 };
      List<WbColumntypeDTO> byCriteria = FactoryDb.GetWbColumntypeRepository().GetByCriteria(((IEnumerable<int>) numArray).ToList<int>());
      List<WbBlobDTO> wbBlobDtoList = FactoryDb.GetWbBlobRepository().BuscarArchivosPortal(texto, extension, byCriteria, nroPagina, pageSize, idFuente);
      List<FileData> fileDataList = new List<FileData>();
      foreach (WbBlobDTO wbBlobDto in wbBlobDtoList)
      {
        FileData fileData = FactoryFile.GetFileManager().ObtenerFile(wbBlobDto.Bloburl);
        if (fileData != null)
        {
          fileData.Metadata = wbBlobDto.Metadata;
          fileDataList.Add(fileData);
        }
      }
      return fileDataList;
    }

    public FileInfo ObtenerFileInfo(string url) => FactoryFile.GetFileManager().ObtenerFileInfo(url);
  }
}

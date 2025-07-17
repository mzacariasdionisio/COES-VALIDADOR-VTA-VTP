// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Servicio.Servicio
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata.Data;
using COES.Storage.App.Metadata.Entidad;
using System;
using System.Collections.Generic;
using System.Linq;

namespace COES.Storage.App.Servicio
{
  public class Servicio
  {
    public List<int> BlobCollection = (List<int>) null;

    public List<WbBlobcolumnDTO> ObtenerColumnas() => FactoryDb.GetWbBlobcolumnRepository().List();

    public List<WbColumntypeDTO> ListarTipos() => FactoryDb.GetWbColumntypeRepository().List();

    public WbBlobcolumnDTO ObtenerColumnaPorId(int id) => FactoryDb.GetWbBlobcolumnRepository().GetById(id);

    public List<WbColumnitemDTO> ObtenerItemsColumna(int idColumna) => FactoryDb.GetWbColumnitemRepository().GetByCriteria(idColumna);

    public int GrabarColumna(WbBlobcolumnDTO entity, out int maximo)
    {
      try
      {
        int num1 = 1;
        int columncodi = 0;
        maximo = 0;
        if (entity.Columncodi == 0)
        {
          WbColumntypeRepository columntypeRepository = FactoryDb.GetWbColumntypeRepository();
          int? nullable = entity.Typecodi;
          int typecodi = nullable.Value;
          WbColumntypeDTO byId = columntypeRepository.GetById(typecodi);
          nullable = byId.Typemaxcount;
          int num2;
          if (!nullable.HasValue)
          {
            num2 = 0;
          }
          else
          {
            nullable = byId.Typemaxcount;
            num2 = nullable.Value;
          }
          int num3 = num2;
          int num4 = FactoryDb.GetWbBlobcolumnRepository().ObtenerCantidadPorTipo(byId.Typecodi);
          if (num4 < num3)
          {
            entity.Columnunique = byId.Typeunique + (num4 + 1).ToString();
            columncodi = FactoryDb.GetWbBlobcolumnRepository().Save(entity);
          }
          else
          {
            num1 = 2;
            maximo = num3;
          }
        }
        else
        {
          FactoryDb.GetWbBlobcolumnRepository().Update(entity);
          columncodi = entity.Columncodi;
          FactoryDb.GetWbColumnitemRepository().Delete(columncodi);
        }
        foreach (WbColumnitemDTO listaItem in entity.ListaItems)
          FactoryDb.GetWbColumnitemRepository().Save(new WbColumnitemDTO()
          {
            Itemvalue = listaItem.Itemvalue,
            Columncodi = columncodi,
            Itemcodi = listaItem.Itemcodi
          });
        return num1;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public int ValidarEliminacionColumna(int idColumna) => FactoryDb.GetWbConfigcolumnRepository().ValidarEliminacionColumna(idColumna);

    public void EliminarColumna(int idColumna)
    {
      try
      {
        FactoryDb.GetWbConfigcolumnRepository().Delete(idColumna);
        FactoryDb.GetWbColumnitemRepository().Delete(idColumna);
        FactoryDb.GetWbBlobcolumnRepository().Delete(idColumna);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public List<WbBlobconfigDTO> ObtenerTiposLibreria() => FactoryDb.GetWbBlobconfigRepository().List();

    public WbBlobconfigDTO ObtenerBlobConfigPorId(int idLibreria) => FactoryDb.GetWbBlobconfigRepository().GetById(idLibreria);

    public List<WbBlobcolumnDTO> ObtenerColumnasPorLibreria(int idLibreria) => FactoryDb.GetWbBlobcolumnRepository().ObtenerColumnasPorLibreria(idLibreria);

    public WbColumntypeDTO ObtenerTipoDatoPorId(int idTipo) => FactoryDb.GetWbColumntypeRepository().GetById(idTipo);

    public int GrabarTipoLibreria(
      WbBlobconfigDTO entity,
      List<WbBlobcolumnDTO> columnas,
      string orden)
    {
      try
      {
        int idConfig;
        if (entity.Configcodi == 0)
        {
          if (FactoryDb.GetWbBlobconfigRepository().ValidarNombreNuevo(entity.Configname) != 0)
            return 2;
          idConfig = FactoryDb.GetWbBlobconfigRepository().Save(entity);
        }
        else
        {
          WbBlobconfigDTO byId = FactoryDb.GetWbBlobconfigRepository().GetById(entity.Configcodi);
          int num = 0;
          if (byId.Configname.Trim().ToLower() != entity.Configname.Trim().ToLower())
            num = FactoryDb.GetWbBlobconfigRepository().ValidarNombreEdicion(entity.Configcodi, entity.Configname);
          if (num != 0)
            return 3;
          FactoryDb.GetWbBlobconfigRepository().Update(entity);
          idConfig = entity.Configcodi;
        }
        FactoryDb.GetWbConfigcolumnRepository().EliminarColumnasLibreria(idConfig);
        List<OrdenColumnaDTO> source = new List<OrdenColumnaDTO>();
        string str1 = orden;
        char[] chArray = new char[1]{ ',' };
        foreach (string str2 in str1.Split(chArray))
        {
          if (!string.IsNullOrEmpty(str2))
          {
            string[] strArray = str2.Split('_');
            if (strArray.Length == 2)
              source.Add(new OrdenColumnaDTO()
              {
                Orden = int.Parse(strArray[0]),
                IdColumna = int.Parse(strArray[1])
              });
          }
        }
        foreach (WbBlobcolumnDTO columna1 in columnas)
        {
          WbBlobcolumnDTO columna = columna1;
          WbConfigcolumnDTO entity1 = new WbConfigcolumnDTO();
          entity1.Configcodi = idConfig;
          entity1.Columncodi = columna.Columncodi;
          entity1.Columnvisible = !string.IsNullOrEmpty(columna.Columnvisible) ? columna.Columnvisible : "N";
          entity1.Columnbusqueda = !string.IsNullOrEmpty(columna.Columnbusqueda) ? columna.Columnbusqueda : "N";
          if (source.Where<OrdenColumnaDTO>((Func<OrdenColumnaDTO, bool>) (x => x.IdColumna == columna.Columncodi)).Count<OrdenColumnaDTO>() == 1)
          {
            int num = source.Where<OrdenColumnaDTO>((Func<OrdenColumnaDTO, bool>) (x => x.IdColumna == columna.Columncodi)).Select<OrdenColumnaDTO, int>((Func<OrdenColumnaDTO, int>) (x => x.Orden)).First<int>();
            entity1.Columnorder = new int?(num);
          }
          else
            entity1.Columnorder = new int?(columna.Columnorder);
          FactoryDb.GetWbConfigcolumnRepository().Save(entity1);
        }
        return 1;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public int ConfigurarVisualizacion(
      int id,
      string baseName,
      string indicador,
      string orderColumn)
    {
      try
      {
        FactoryDb.GetWbBlobRepository().ActualizarVisualizacion(id, baseName, indicador, orderColumn);
        return 1;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public int ValidarEliminacionLibreria(int idConfig) => FactoryDb.GetWbBlobconfigRepository().ValidarEliminacionLibreria(idConfig);

    public void EliminarLibreria(int idCondig)
    {
      try
      {
        FactoryDb.GetWbConfigcolumnRepository().EliminarColumnasLibreria(idCondig);
        FactoryDb.GetWbBlobconfigRepository().Delete(idCondig);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public WbBlobDTO GrabarBlob(
      string url,
      string fileName,
      string size,
      string tipo,
      string user)
    {
      try
      {
        url = url.Replace("\\", "/");
        if (string.IsNullOrEmpty(url))
          url = "/";
        WbBlobDTO blobByUrl1 = FactoryDb.GetWbBlobRepository().GetBlobByUrl(url);
        int num1;
        int num2;
        if (blobByUrl1 != null)
        {
          num1 = blobByUrl1.Configcodi.Value;
          num2 = blobByUrl1.Blobcodi;
        }
        else
        {
          num1 = 1;
          WbBlobDTO result = new WbBlobDTO();
          num2 = this.GenerarCarpeta(url, user, "N", new int?(0), out result);
        }
        WbBlobDTO entity = new WbBlobDTO();
        entity.Blobdatecreated = new DateTime?(DateTime.Now);
        entity.Blobdateupdate = new DateTime?(DateTime.Now);
        entity.Blobname = fileName;
        entity.Bloburl = url != "/" ? url + fileName : fileName;
        entity.Blobusercreate = user;
        entity.Blobuserupdate = user;
        entity.Blobtype = "F";
        entity.Blobsize = size.ToString();
        entity.Blobstate = "A";
        entity.Padrecodi = new int?(num2);
        entity.Configcodi = new int?(num1);
        WbBlobDTO blobByUrl2 = FactoryDb.GetWbBlobRepository().GetBlobByUrl(entity.Bloburl);
        if (blobByUrl2 != null)
        {
          FactoryDb.GetWbBlobRepository().Update(entity);
          entity.Blobcodi = blobByUrl2.Blobcodi;
        }
        else
        {
          int num3 = FactoryDb.GetWbBlobRepository().Save(entity);
          entity.Blobcodi = num3;
        }
        return entity;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public WbBlobDTO GrabarBlob(
      string url,
      string fileName,
      string size,
      string tipo,
      string user,
      int idFuente)
    {
      try
      {
        url = url.Replace("\\", "/");
        if (string.IsNullOrEmpty(url))
          url = "/";
        WbBlobDTO blobByUrl1 = FactoryDb.GetWbBlobRepository().GetBlobByUrl(url, idFuente);
        int num1;
        int num2;
        if (blobByUrl1 != null)
        {
          num1 = blobByUrl1.Configcodi.Value;
          num2 = blobByUrl1.Blobcodi;
        }
        else
        {
          num1 = Helper.ObtenerLibreriaPorFuente(idFuente);
          WbBlobDTO result = new WbBlobDTO();
          num2 = this.GenerarCarpeta(url, user, "N", new int?(0), out result, new int?(idFuente));
        }
        WbBlobDTO entity = new WbBlobDTO();
        entity.Blobdatecreated = new DateTime?(DateTime.Now);
        entity.Blobdateupdate = new DateTime?(DateTime.Now);
        entity.Blobname = fileName;
        entity.Bloburl = url != "/" ? url + fileName : fileName;
        entity.Blobusercreate = user;
        entity.Blobuserupdate = user;
        entity.Blobtype = "F";
        entity.Blobsize = size.ToString();
        entity.Blobstate = "A";
        entity.Padrecodi = new int?(num2);
        entity.Configcodi = new int?(num1);
        entity.Blofuecodi = new int?(idFuente);
        WbBlobDTO blobByUrl2 = FactoryDb.GetWbBlobRepository().GetBlobByUrl(entity.Bloburl, idFuente);
        if (blobByUrl2 != null)
        {
          FactoryDb.GetWbBlobRepository().Update(entity);
          entity.Blobcodi = blobByUrl2.Blobcodi;
        }
        else
        {
          int num3 = FactoryDb.GetWbBlobRepository().Save(entity);
          entity.Blobcodi = num3;
        }
        return entity;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public void ActualizarUrlFile(
      string url,
      string targetBlobName,
      string longitud,
      string user,
      string fileReemplazo)
    {
      try
      {
        WbBlobDTO blobByUrl = FactoryDb.GetWbBlobRepository().GetBlobByUrl(fileReemplazo);
        if (blobByUrl == null)
          return;
        blobByUrl.Blobname = targetBlobName;
        blobByUrl.Bloburl = url != "/" ? url + targetBlobName : targetBlobName;
        blobByUrl.Blobuserupdate = user;
        blobByUrl.Blobsize = longitud;
        FactoryDb.GetWbBlobRepository().ActualizarUrlFile(blobByUrl);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public string ActualizarDatosCarpeta(
      int id,
      string url,
      string user,
      string indicador,
      int? tipoFolder,
      string nuevoFolder)
    {
      try
      {
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        char[] separator = new char[1]{ '/' };
        string[] strArray = url.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        if ((uint) strArray.Length > 0U)
          empty2 = strArray[strArray.Length - 1];
        if (url.Length > 0 && url[url.Length - 1] != '/')
          url += "/";
        string blobname = FactoryDb.GetWbBlobRepository().GetById(id).Blobname;
        FactoryDb.GetWbBlobRepository().Update(new WbBlobDTO()
        {
          Blobname = empty2,
          Bloburl = url,
          Blobuserupdate = user,
          Configcodi = tipoFolder,
          Blobcodi = id,
          Blobfoldertype = indicador
        });
        this.ActualizarEstructuraHijos(id, tipoFolder.Value, blobname, nuevoFolder);
        return blobname;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    private void ActualizarEstructuraHijos(
      int id,
      int configId,
      string folderAnterior,
      string nuevoFolder)
    {
      try
      {
        this.BlobCollection = new List<int>();
        this.ObtenerBlobRecursivo(id);
        string ids = string.Join<int>(",", (IEnumerable<int>) this.BlobCollection);
        FactoryDb.GetWbBlobRepository().ActualizarTipoLibreria(ids, configId, folderAnterior, nuevoFolder);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public int GenerarCarpeta(
      string url,
      string user,
      string indicador,
      int? tipoFolder,
      out WbBlobDTO result)
    {
      try
      {
        string str = string.Empty;
        int? nullable = new int?(0);
        int num1;
        if (string.IsNullOrEmpty(url) || url == "/")
        {
          str = "Inicio";
          nullable = new int?(1);
          num1 = -1;
          url = "/";
        }
        else
        {
          char[] separator = new char[1]{ '/' };
          string[] strArray = url.Split(separator, StringSplitOptions.RemoveEmptyEntries);
          if ((uint) strArray.Length > 0U)
            str = strArray[strArray.Length - 1];
          string url1 = string.Empty;
          for (int index = 0; index < strArray.Length - 1; ++index)
            url1 = url1 + strArray[index] + "/";
          if (string.IsNullOrEmpty(url1))
            url1 = "/";
          WbBlobDTO blobByUrl = FactoryDb.GetWbBlobRepository().GetBlobByUrl(url1);
          if (blobByUrl != null)
          {
            nullable = !(indicador != "S") ? tipoFolder : new int?(blobByUrl.Configcodi.Value);
            num1 = blobByUrl.Blobcodi;
          }
          else
          {
            nullable = !(indicador != "S") ? tipoFolder : new int?(1);
            WbBlobDTO result1 = new WbBlobDTO();
            num1 = this.GenerarCarpeta(url1, user, "N", new int?(0), out result1);
          }
        }
        if (url.Length > 0 && url[url.Length - 1] != '/')
          url += "/";
        WbBlobDTO entity = new WbBlobDTO();
        entity.Blobdatecreated = new DateTime?(DateTime.Now);
        entity.Blobdateupdate = new DateTime?(DateTime.Now);
        entity.Blobname = str;
        entity.Blobstate = "A";
        entity.Blobtype = "D";
        entity.Bloburl = url;
        entity.Blobusercreate = user;
        entity.Blobuserupdate = user;
        entity.Configcodi = nullable;
        entity.Padrecodi = new int?(num1);
        entity.Blobsize = 0.ToString();
        entity.Blobfoldertype = indicador;
        int num2 = FactoryDb.GetWbBlobRepository().Save(entity);
        entity.Blobcodi = num2;
        result = entity;
        return num2;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public int GenerarCarpeta(
      string url,
      string user,
      string indicador,
      int? tipoFolder,
      out WbBlobDTO result,
      int? idFuente)
    {
      try
      {
        string str = string.Empty;
        int? nullable = new int?(0);
        int num1;
        if (string.IsNullOrEmpty(url) || url == "/")
        {
          str = "Inicio";
          nullable = new int?(Helper.ObtenerLibreriaPorFuente(idFuente.Value));
          num1 = -1;
          url = "/";
        }
        else
        {
          char[] separator = new char[1]{ '/' };
          string[] strArray = url.Split(separator, StringSplitOptions.RemoveEmptyEntries);
          if ((uint) strArray.Length > 0U)
            str = strArray[strArray.Length - 1];
          string url1 = string.Empty;
          for (int index = 0; index < strArray.Length - 1; ++index)
            url1 = url1 + strArray[index] + "/";
          if (string.IsNullOrEmpty(url1))
            url1 = "/";
          WbBlobDTO blobByUrl = FactoryDb.GetWbBlobRepository().GetBlobByUrl(url1, idFuente.Value);
          if (blobByUrl != null)
          {
            nullable = !(indicador != "S") ? tipoFolder : new int?(blobByUrl.Configcodi.Value);
            num1 = blobByUrl.Blobcodi;
          }
          else
          {
            nullable = !(indicador != "S") ? tipoFolder : new int?(Helper.ObtenerLibreriaPorFuente(idFuente.Value));
            WbBlobDTO result1 = new WbBlobDTO();
            num1 = this.GenerarCarpeta(url1, user, "N", new int?(0), out result1, idFuente);
          }
        }
        if (url.Length > 0 && url[url.Length - 1] != '/')
          url += "/";
        WbBlobDTO entity = new WbBlobDTO();
        entity.Blobdatecreated = new DateTime?(DateTime.Now);
        entity.Blobdateupdate = new DateTime?(DateTime.Now);
        entity.Blobname = str;
        entity.Blobstate = "A";
        entity.Blobtype = "D";
        entity.Bloburl = url;
        entity.Blobusercreate = user;
        entity.Blobuserupdate = user;
        entity.Configcodi = nullable;
        entity.Padrecodi = new int?(num1);
        entity.Blobsize = 0.ToString();
        entity.Blobfoldertype = indicador;
        entity.Blofuecodi = idFuente;
        int num2 = FactoryDb.GetWbBlobRepository().Save(entity);
        entity.Blobcodi = num2;
        result = entity;
        return num2;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public void VerificarEstructura(ref List<FileData> list, string user)
    {
      List<WbBlobconfigDTO> source = FactoryDb.GetWbBlobconfigRepository().List();
      foreach (FileData fileData1 in list)
      {
        WbBlobDTO blob = FactoryDb.GetWbBlobRepository().GetBlobByUrl(fileData1.FileUrl);
        if (blob == null)
        {
          WbBlobDTO result = (WbBlobDTO) null;
          if (fileData1.FileType == "F")
          {
            string[] strArray = fileData1.FileUrl.Split(new char[1]
            {
              '/'
            }, StringSplitOptions.RemoveEmptyEntries);
            string url = string.Empty;
            for (int index = 0; index < strArray.Length - 1; ++index)
              url = url + strArray[index] + "/";
            result = this.GrabarBlob(url, fileData1.FileName, fileData1.FileSize, fileData1.FileType, user);
          }
          else if (fileData1.FileType == "D")
            this.GenerarCarpeta(fileData1.FileUrl, user, "N", new int?(0), out result);
          blob = result;
        }
        if (blob != null)
        {
          FileData fileData2 = fileData1;
          DateTime? nullable = blob.Blobdateupdate;
          DateTime dateTime = nullable.Value;
          fileData2.LastDate = dateTime;
          fileData1.LastUser = blob.Blobuserupdate;
          fileData1.TipoLibreria = source.Where<WbBlobconfigDTO>((Func<WbBlobconfigDTO, bool>) (x =>
          {
            int configcodi1 = x.Configcodi;
            int? configcodi2 = blob.Configcodi;
            int valueOrDefault = configcodi2.GetValueOrDefault();
            return configcodi1 == valueOrDefault & configcodi2.HasValue;
          })).FirstOrDefault<WbBlobconfigDTO>().Configname;
          fileData1.TipoFolder = blob.Blobfoldertype == "S" ? "Principal" : string.Empty;
          fileData1.Blobcodi = blob.Blobcodi;
          fileData1.FirstUser = blob.Blobusercreate;
          FileData fileData3 = fileData1;
          nullable = blob.Blobdatecreated;
          DateTime lastDate;
          if (!nullable.HasValue)
          {
            lastDate = fileData1.LastDate;
          }
          else
          {
            nullable = blob.Blobdatecreated;
            lastDate = nullable.Value;
          }
          fileData3.FirstDate = lastDate;
          fileData1.FileHide = blob.Blobhide;
          fileData1.TreePadre = blob.Blobtreepadre;
        }
      }
    }

    public void VerificarEstructura(ref List<FileData> list, string user, int idFuente)
    {
      List<WbBlobconfigDTO> source = FactoryDb.GetWbBlobconfigRepository().List(idFuente);
      foreach (FileData fileData1 in list)
      {
                if (fileData1.FileName.Contains("07 Determinación de los Costos Marginales"))
                {

                }
        WbBlobDTO blob = FactoryDb.GetWbBlobRepository().GetBlobByUrl(fileData1.FileUrl, idFuente);
        if (blob == null)
        {
          WbBlobDTO result = (WbBlobDTO) null;
          if (fileData1.FileType == "F")
          {
            string[] strArray = fileData1.FileUrl.Split(new char[1]
            {
              '/'
            }, StringSplitOptions.RemoveEmptyEntries);
            string url = string.Empty;
            for (int index = 0; index < strArray.Length - 1; ++index)
              url = url + strArray[index] + "/";
            result = this.GrabarBlob(url, fileData1.FileName, fileData1.FileSize, fileData1.FileType, user, idFuente);
          }
          else if (fileData1.FileType == "D")
            this.GenerarCarpeta(fileData1.FileUrl, user, "N", new int?(0), out result, new int?(idFuente));
          blob = result;
        }
        if (blob != null)
        {
          FileData fileData2 = fileData1;
          DateTime? nullable = blob.Blobdateupdate;
          DateTime dateTime = nullable.Value;
          fileData2.LastDate = dateTime;
          fileData1.LastUser = blob.Blobuserupdate;
          fileData1.TipoLibreria = source.Where<WbBlobconfigDTO>((Func<WbBlobconfigDTO, bool>) (x =>
          {
            int configcodi1 = x.Configcodi;
            int? configcodi2 = blob.Configcodi;
            int valueOrDefault = configcodi2.GetValueOrDefault();
            return configcodi1 == valueOrDefault & configcodi2.HasValue;
          })).FirstOrDefault<WbBlobconfigDTO>().Configname;
          fileData1.TipoFolder = blob.Blobfoldertype == "S" ? "Principal" : string.Empty;
          fileData1.Blobcodi = blob.Blobcodi;
          fileData1.FirstUser = blob.Blobusercreate;
          FileData fileData3 = fileData1;
          nullable = blob.Blobdatecreated;
          DateTime lastDate;
          if (!nullable.HasValue)
          {
            lastDate = fileData1.LastDate;
          }
          else
          {
            nullable = blob.Blobdatecreated;
            lastDate = nullable.Value;
          }
          fileData3.FirstDate = lastDate;
          fileData1.FileHide = blob.Blobhide;
          fileData1.TreePadre = blob.Blobtreepadre;
        }
      }
    }

    public void EliminarBlob(string url)
    {
      try
      {
        WbBlobDTO blobByUrl = FactoryDb.GetWbBlobRepository().GetBlobByUrl(url);
        if (blobByUrl == null)
          return;
        FactoryDb.GetWbBlobmetadataRepository().Delete(blobByUrl.Blobcodi);
        FactoryDb.GetWbBlobRepository().Delete(blobByUrl.Blobcodi);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public void OcultarBlob(string url, string indicador)
    {
      try
      {
        WbBlobDTO blobByUrl = FactoryDb.GetWbBlobRepository().GetBlobByUrl(url);
        if (blobByUrl == null)
          return;
        FactoryDb.GetWbBlobRepository().OcultarBlob(blobByUrl.Blobcodi, indicador);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public void EliminarCarpeta(string url)
    {
      try
      {
        this.BlobCollection = new List<int>();
        url = url.Replace("\\", "/");
        WbBlobDTO blobByUrl = FactoryDb.GetWbBlobRepository().GetBlobByUrl(url);
        if (blobByUrl == null)
          return;
        this.BlobCollection.Add(blobByUrl.Blobcodi);
        foreach (WbBlobDTO wbBlobDto in FactoryDb.GetWbBlobRepository().ObtenerPorPadre(blobByUrl.Blobcodi))
        {
          this.BlobCollection.Add(wbBlobDto.Blobcodi);
          if (wbBlobDto.Blobtype == "D")
            this.ObtenerBlobRecursivo(wbBlobDto.Blobcodi);
        }
        if (this.BlobCollection.Count >= 0)
        {
          string ids = string.Join<int>(",", (IEnumerable<int>) this.BlobCollection);
          FactoryDb.GetWbBlobRepository().EliminarRecursivo(ids);
        }
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    private void ObtenerBlobRecursivo(int blobcodi)
    {
      foreach (WbBlobDTO wbBlobDto in FactoryDb.GetWbBlobRepository().ObtenerPorPadre(blobcodi))
      {
        this.BlobCollection.Add(wbBlobDto.Blobcodi);
        if (wbBlobDto.Blobtype == "D")
          this.ObtenerBlobRecursivo(wbBlobDto.Blobcodi);
      }
    }

    public WbBlobDTO ObtenerBlobPorUrl(string url) => FactoryDb.GetWbBlobRepository().GetBlobByUrl(url);

    public WbBlobDTO ObtenerBlobPorUrl(string url, int idFuente) => FactoryDb.GetWbBlobRepository().GetBlobByUrl(url, idFuente);

    public void ActualizarIssuu(
      string url,
      string issuuind,
      string issuulink,
      string issuupos,
      string issuulenx,
      string issuuleny)
    {
      try
      {
        WbBlobDTO wbBlobDto = this.ObtenerBlobPorUrl(url);
        FactoryDb.GetWbBlobRepository().ActualizarIssuu(wbBlobDto.Blobcodi, issuuind, issuulink, issuupos, issuulenx, issuuleny);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public void ActualizarIssuu(
      string url,
      string issuuind,
      string issuulink,
      string issuupos,
      string issuulenx,
      string issuuleny,
      int idFuente)
    {
      try
      {
        WbBlobDTO wbBlobDto = this.ObtenerBlobPorUrl(url, idFuente);
        FactoryDb.GetWbBlobRepository().ActualizarIssuu(wbBlobDto.Blobcodi, issuuind, issuulink, issuupos, issuulenx, issuuleny);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public void ActualizarVistaArbol(int id, string indTree)
    {
      try
      {
        FactoryDb.GetWbBlobRepository().ActualizarVistaArbol(id, indTree);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public void ActualizarPadreArbol(int opcionId, int padreId)
    {
      try
      {
        FactoryDb.GetWbBlobRepository().ActualizarPadreArbol(opcionId, padreId);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public void RenombrarArchivo(int id, string url, string filename, string user)
    {
      try
      {
        FactoryDb.GetWbBlobRepository().ActualizarArchivo(new WbBlobDTO()
        {
          Blobname = filename,
          Blobcodi = id,
          Bloburl = url + filename,
          Blobuserupdate = user
        });
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public bool VerificarFolderPrincipal(string url)
    {
      char[] separator = new char[1]{ '/' };
      string[] strArray = url.Split(separator, StringSplitOptions.RemoveEmptyEntries);
      string url1 = string.Empty;
      for (int index = 0; index < strArray.Length; ++index)
      {
        url1 = url1 + strArray[index] + "/";
        WbBlobDTO blobByUrl = FactoryDb.GetWbBlobRepository().GetBlobByUrl(url1);
        if (blobByUrl != null && blobByUrl.Blobfoldertype == "S")
          return false;
      }
      return true;
    }

    public void GrabarMetadato(int blobCodi, List<BlobDatoDTO> list)
    {
      try
      {
        if (FactoryDb.GetWbBlobmetadataRepository().ObtenerPorBlob(blobCodi) > 0)
          FactoryDb.GetWbBlobmetadataRepository().Update(blobCodi, list);
        else
          FactoryDb.GetWbBlobmetadataRepository().Save(blobCodi, list);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    public List<WbColumnitemDTO> ObtenerListaItemColumna(int columncodi) => FactoryDb.GetWbColumnitemRepository().GetByCriteria(columncodi);
  }
}

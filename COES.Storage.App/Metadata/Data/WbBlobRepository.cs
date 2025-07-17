// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Data.WbBlobRepository
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata.Entidad;
using COES.Storage.App.Metadata.Helper;
using System;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace COES.Storage.App.Metadata.Data
{
  public class WbBlobRepository : RepositoryBase
  {
    private WbBlobHelper helper = new WbBlobHelper();

    public WbBlobRepository(string strConn)
      : base(strConn)
    {
    }

    public int Save(WbBlobDTO entity)
    {
      object obj = this.dbProvider.ExecuteScalar(this.dbProvider.GetSqlStringCommand(this.helper.SqlGetMaxId));
      int num = 1;
      if (obj != null)
        num = Convert.ToInt32(obj);
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlSave);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobcodi, DbType.Int32, (object) num);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configcodi, DbType.Int32, (object) entity.Configcodi);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Bloburl, DbType.String, (object) entity.Bloburl);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Padrecodi, DbType.Int32, (object) entity.Padrecodi);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobname, DbType.String, (object) entity.Blobname);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobsize, DbType.String, (object) entity.Blobsize);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobdatecreated, DbType.DateTime, (object) entity.Blobdatecreated);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobusercreate, DbType.String, (object) entity.Blobusercreate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobdateupdate, DbType.DateTime, (object) entity.Blobdateupdate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobuserupdate, DbType.String, (object) entity.Blobuserupdate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobstate, DbType.String, (object) entity.Blobstate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobtype, DbType.String, (object) entity.Blobtype);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobfoldertype, DbType.String, (object) entity.Blobfoldertype);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
      return num;
    }

    public void Update(WbBlobDTO entity)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlUpdate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Configcodi, DbType.Int32, (object) entity.Configcodi);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobuserupdate, DbType.String, (object) entity.Blobuserupdate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobname, DbType.String, (object) entity.Blobname);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Bloburl, DbType.String, (object) entity.Bloburl);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobfoldertype, DbType.String, (object) entity.Blobfoldertype);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobcodi, DbType.Int32, (object) entity.Blobcodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public void ActualizarUrlFile(WbBlobDTO entity)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlActualizarUrl);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Bloburl, DbType.String, (object) entity.Bloburl);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobname, DbType.String, (object) entity.Blobname);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobsize, DbType.String, (object) entity.Blobsize);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobuserupdate, DbType.String, (object) entity.Blobuserupdate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobcodi, DbType.Int32, (object) entity.Blobcodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public void Delete(int blobcodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlDelete);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobcodi, DbType.Int32, (object) blobcodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public void OcultarBlob(int blobcodi, string indicador)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlOcultarBlob);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobhide, DbType.String, (object) indicador);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobcodi, DbType.Int32, (object) blobcodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public WbBlobDTO GetById(int blobcodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlGetById);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobcodi, DbType.Int32, (object) blobcodi);
      WbBlobDTO wbBlobDto = (WbBlobDTO) null;
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        if (dr.Read())
          wbBlobDto = this.helper.Create(dr);
      }
      return wbBlobDto;
    }

    public System.Collections.Generic.List<WbBlobDTO> List()
    {
      System.Collections.Generic.List<WbBlobDTO> wbBlobDtoList = new System.Collections.Generic.List<WbBlobDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(this.helper.SqlList)))
      {
        while (dr.Read())
          wbBlobDtoList.Add(this.helper.Create(dr));
      }
      return wbBlobDtoList;
    }

    public System.Collections.Generic.List<WbBlobDTO> GetByCriteria()
    {
      System.Collections.Generic.List<WbBlobDTO> wbBlobDtoList = new System.Collections.Generic.List<WbBlobDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(this.helper.SqlGetByCriteria)))
      {
        while (dr.Read())
          wbBlobDtoList.Add(this.helper.Create(dr));
      }
      return wbBlobDtoList;
    }

    public WbBlobDTO GetBlobByUrl(string url)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlObtenerBlobByUrl);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Bloburl, DbType.String, (object) url);
      WbBlobDTO wbBlobDto = (WbBlobDTO) null;
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        if (dr.Read())
        {
          wbBlobDto = this.helper.Create(dr);
          int ordinal1 = dr.GetOrdinal(this.helper.Blobissuu);
          if (!dr.IsDBNull(ordinal1))
            wbBlobDto.Blobissuu = dr.GetString(ordinal1);
          int ordinal2 = dr.GetOrdinal(this.helper.Blobhiddcol);
          if (!dr.IsDBNull(ordinal2))
            wbBlobDto.Blobhiddcol = dr.GetString(ordinal2);
          int ordinal3 = dr.GetOrdinal(this.helper.Blobbreadname);
          if (!dr.IsDBNull(ordinal3))
            wbBlobDto.Blobbreadname = dr.GetString(ordinal3);
          int ordinal4 = dr.GetOrdinal(this.helper.Blobissuulink);
          if (!dr.IsDBNull(ordinal4))
            wbBlobDto.Blobissuulink = dr.GetString(ordinal4);
          int ordinal5 = dr.GetOrdinal(this.helper.Blobissuupos);
          if (!dr.IsDBNull(ordinal5))
            wbBlobDto.Blobissuupos = dr.GetString(ordinal5);
          int ordinal6 = dr.GetOrdinal(this.helper.Blobissuulenx);
          if (!dr.IsDBNull(ordinal6))
            wbBlobDto.Blobissuulenx = dr.GetString(ordinal6);
          int ordinal7 = dr.GetOrdinal(this.helper.Blobissuuleny);
          if (!dr.IsDBNull(ordinal7))
            wbBlobDto.Blobissuuleny = dr.GetString(ordinal7);
          int ordinal8 = dr.GetOrdinal(this.helper.Bloborderfolder);
          if (!dr.IsDBNull(ordinal8))
            wbBlobDto.Bloborderfolder = dr.GetString(ordinal8);
          int ordinal9 = dr.GetOrdinal(this.helper.Blobhide);
          if (!dr.IsDBNull(ordinal9))
            wbBlobDto.Blobhide = dr.GetString(ordinal9);
          int ordinal10 = dr.GetOrdinal(this.helper.Indtree);
          if (!dr.IsDBNull(ordinal10))
            wbBlobDto.Indtree = dr.GetString(ordinal10);
          int ordinal11 = dr.GetOrdinal(this.helper.Blobtreepadre);
          if (!dr.IsDBNull(ordinal11))
            wbBlobDto.Blobtreepadre = new int?(Convert.ToInt32(dr.GetValue(ordinal11)));
        }
      }
      return wbBlobDto;
    }

    public WbBlobDTO GetBlobByUrl(string url, int idFuente)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlObtenerBlobByUrl2);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Bloburl, DbType.String, (object) url);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blofuecodi, DbType.Int32, (object) idFuente);
      WbBlobDTO wbBlobDto = (WbBlobDTO) null;
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        if (dr.Read())
        {
          wbBlobDto = this.helper.Create(dr);
          int ordinal1 = dr.GetOrdinal(this.helper.Blobissuu);
          if (!dr.IsDBNull(ordinal1))
            wbBlobDto.Blobissuu = dr.GetString(ordinal1);
          int ordinal2 = dr.GetOrdinal(this.helper.Blobhiddcol);
          if (!dr.IsDBNull(ordinal2))
            wbBlobDto.Blobhiddcol = dr.GetString(ordinal2);
          int ordinal3 = dr.GetOrdinal(this.helper.Blobbreadname);
          if (!dr.IsDBNull(ordinal3))
            wbBlobDto.Blobbreadname = dr.GetString(ordinal3);
          int ordinal4 = dr.GetOrdinal(this.helper.Blobissuulink);
          if (!dr.IsDBNull(ordinal4))
            wbBlobDto.Blobissuulink = dr.GetString(ordinal4);
          int ordinal5 = dr.GetOrdinal(this.helper.Blobissuupos);
          if (!dr.IsDBNull(ordinal5))
            wbBlobDto.Blobissuupos = dr.GetString(ordinal5);
          int ordinal6 = dr.GetOrdinal(this.helper.Blobissuulenx);
          if (!dr.IsDBNull(ordinal6))
            wbBlobDto.Blobissuulenx = dr.GetString(ordinal6);
          int ordinal7 = dr.GetOrdinal(this.helper.Blobissuuleny);
          if (!dr.IsDBNull(ordinal7))
            wbBlobDto.Blobissuuleny = dr.GetString(ordinal7);
          int ordinal8 = dr.GetOrdinal(this.helper.Bloborderfolder);
          if (!dr.IsDBNull(ordinal8))
            wbBlobDto.Bloborderfolder = dr.GetString(ordinal8);
          int ordinal9 = dr.GetOrdinal(this.helper.Blobhide);
          if (!dr.IsDBNull(ordinal9))
            wbBlobDto.Blobhide = dr.GetString(ordinal9);
          int ordinal10 = dr.GetOrdinal(this.helper.Indtree);
          if (!dr.IsDBNull(ordinal10))
            wbBlobDto.Indtree = dr.GetString(ordinal10);
          int ordinal11 = dr.GetOrdinal(this.helper.Blobtreepadre);
          if (!dr.IsDBNull(ordinal11))
            wbBlobDto.Blobtreepadre = new int?(Convert.ToInt32(dr.GetValue(ordinal11)));
        }
      }
      return wbBlobDto;
    }

    public void ActualizarIssuu(
      int blobcodi,
      string issuuind,
      string issuulink,
      string issuupos,
      string issuulenx,
      string issuuleny)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlActualizarIssuu);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobissuu, DbType.String, (object) issuuind);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobissuulink, DbType.String, (object) issuulink);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobissuupos, DbType.String, (object) issuupos);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobissuulenx, DbType.String, (object) issuulenx);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobissuuleny, DbType.String, (object) issuuleny);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobcodi, DbType.Int32, (object) blobcodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public void ActualizarVistaArbol(int id, string indTree)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlActualizarArbol);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Indtree, DbType.String, (object) indTree);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobcodi, DbType.Int32, (object) id);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public void ActualizarPadreArbol(int opcionId, int padreId)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlActualizarPadreArbol);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobtreepadre, DbType.Int32, (object) padreId);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobcodi, DbType.Int32, (object) opcionId);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public System.Collections.Generic.List<WbBlobDTO> ObtenerPorPadre(int blobcodi)
    {
      System.Collections.Generic.List<WbBlobDTO> wbBlobDtoList = new System.Collections.Generic.List<WbBlobDTO>();
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlObtenerPorPadre);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Padrecodi, DbType.Int32, (object) blobcodi);
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        while (dr.Read())
          wbBlobDtoList.Add(this.helper.Create(dr));
      }
      return wbBlobDtoList;
    }

    public void EliminarRecursivo(string ids)
    {
      this.dbProvider.ExecuteNonQuery(this.dbProvider.GetSqlStringCommand(string.Format(this.helper.SqlEliminarMetadataRecursivo, (object) ids)));
      this.dbProvider.ExecuteNonQuery(this.dbProvider.GetSqlStringCommand(string.Format(this.helper.SqlEliminarRecursivo, (object) ids)));
    }

    public void ActualizarArchivo(WbBlobDTO entity)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlActualizarArchivo);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobuserupdate, DbType.String, (object) entity.Blobuserupdate);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobname, DbType.String, (object) entity.Blobname);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Bloburl, DbType.String, (object) entity.Bloburl);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobcodi, DbType.Int32, (object) entity.Blobcodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public void ActualizarVisualizacion(
      int id,
      string baseName,
      string indicador,
      string orderColumn)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlActualizarVisualizacion);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobbreadname, DbType.String, (object) baseName);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobhiddcol, DbType.String, (object) indicador);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Bloborderfolder, DbType.String, (object) orderColumn);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobcodi, DbType.Int32, (object) id);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public void ActualizarTipoLibreria(
      string ids,
      int condigId,
      string folderAnterior,
      string folderNuevo)
    {
      if (string.IsNullOrEmpty(ids))
        return;
      this.dbProvider.ExecuteNonQuery(this.dbProvider.GetSqlStringCommand(string.Format(this.helper.SqlActualizarTipoLibreria, (object) ids, (object) condigId, (object) folderAnterior, (object) folderNuevo)));
    }

    public System.Collections.Generic.List<WbBlobDTO> ObtenerFoldersPrincipales()
    {
      System.Collections.Generic.List<WbBlobDTO> wbBlobDtoList = new System.Collections.Generic.List<WbBlobDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(this.helper.SqlObtenerFoldersPrincipales)))
      {
        while (dr.Read())
        {
          WbBlobDTO wbBlobDto = this.helper.Create(dr);
          wbBlobDto.IndCheck = true;
          wbBlobDtoList.Add(wbBlobDto);
        }
      }
      return wbBlobDtoList;
    }

    public System.Collections.Generic.List<WbBlobDTO> ObtenerCarpetasUsuario(
      int userCode)
    {
      System.Collections.Generic.List<WbBlobDTO> wbBlobDtoList = new System.Collections.Generic.List<WbBlobDTO>();
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlObtenerCarpetasPorUsuario);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.SqlObtenerCarpetasPorUsuario, DbType.Int32, (object) userCode);
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        while (dr.Read())
        {
          WbBlobDTO wbBlobDto = this.helper.Create(dr);
          wbBlobDto.IndCheck = true;
          wbBlobDtoList.Add(wbBlobDto);
        }
      }
      return wbBlobDtoList;
    }

    public System.Collections.Generic.List<WbBlobDTO> ObtenerBlobs(string codigos)
    {
      System.Collections.Generic.List<WbBlobDTO> wbBlobDtoList = new System.Collections.Generic.List<WbBlobDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(string.Format(this.helper.SqlObtenerBlobs, (object) codigos))))
      {
        while (dr.Read())
          wbBlobDtoList.Add(this.helper.Create(dr));
      }
      return wbBlobDtoList;
    }

    public System.Collections.Generic.List<WbBlobDTO> BuscarArchivos(
      string url,
      System.Collections.Generic.List<DatoItem> datos)
    {
      string str = "select bl.* from \r\n                wb_blob bl left join wb_blobmetadata metadata\r\n                on bl.blobcodi = metadata.blobcodi where ";
      foreach (DatoItem dato in datos)
      {
        if (dato.TipoDato == 1 || dato.TipoDato == 2)
          str = str + "metadata." + dato.Columna + " = " + dato.Valor + " AND ";
        else if (dato.TipoDato == 6 || dato.TipoDato == 3 || dato.TipoDato == 4)
        {
          if (dato.Id != 7)
            str = str + "lower(metadata." + dato.Columna + ") like lower('%" + dato.Valor + "%') AND ";
          else
            str = str + "(lower(metadata." + dato.Columna + ") like lower('%" + dato.Valor + "%') or lower(bl.blobname) like lower('%" + dato.Valor + "%')) AND ";
        }
        else if (dato.TipoDato == 5)
          str = str + "metadata." + dato.Columna + string.Format(">= TO_DATE('{0}', 'YYYY-MM-DD')", (object) DateTime.ParseExact(dato.Valor, "dd/MM/yyyy", (IFormatProvider) CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")) + " AND ";
      }
      string query = str + " bl.bloburl like '" + url + "%' and bl.blobtype = 'F'";
      System.Collections.Generic.List<WbBlobDTO> wbBlobDtoList = new System.Collections.Generic.List<WbBlobDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(query)))
      {
        while (dr.Read())
          wbBlobDtoList.Add(this.helper.Create(dr));
      }
      return wbBlobDtoList;
    }

    public System.Collections.Generic.List<WbBlobDTO> BuscarArchivos(
      string url,
      System.Collections.Generic.List<DatoItem> datos,
      int idFuente)
    {
      string str = "select bl.* from wb_blob bl left join wb_blobmetadata metadata on bl.blobcodi = metadata.blobcodi where ";
      foreach (DatoItem dato in datos)
      {
        if (dato.TipoDato == 1 || dato.TipoDato == 2)
          str = str + "metadata." + dato.Columna + " = " + dato.Valor + " AND ";
        else if (dato.TipoDato == 6 || dato.TipoDato == 3 || dato.TipoDato == 4)
        {
          if (dato.Id != 7)
            str = str + "lower(metadata." + dato.Columna + ") like lower('%" + dato.Valor + "%') AND ";
          else
            str = str + "(lower(metadata." + dato.Columna + ") like lower('%" + dato.Valor + "%') or lower(bl.blobname) like lower('%" + dato.Valor + "%')) AND ";
        }
        else if (dato.TipoDato == 5)
          str = str + "metadata." + dato.Columna + string.Format(">= TO_DATE('{0}', 'YYYY-MM-DD')", (object) DateTime.ParseExact(dato.Valor, "dd/MM/yyyy", (IFormatProvider) CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")) + " AND ";
      }
      string query = str + " bl.bloburl like '" + url + "%' and bl.blobtype = 'F' and bl.blobstate = 'A' and bl.blofuecodi = " + idFuente.ToString();
      System.Collections.Generic.List<WbBlobDTO> wbBlobDtoList = new System.Collections.Generic.List<WbBlobDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(query)))
      {
        while (dr.Read())
          wbBlobDtoList.Add(this.helper.Create(dr));
      }
      return wbBlobDtoList;
    }

    public int ObtenerNroRegistroBusqueda(string url, System.Collections.Generic.List<DatoItem> datos)
    {
      string str = "select count(*) from \r\n                wb_blob bl left join wb_blobmetadata metadata\r\n                on bl.blobcodi = metadata.blobcodi where ";
      foreach (DatoItem dato in datos)
      {
        if (dato.TipoDato == 1 || dato.TipoDato == 2)
          str = str + "metadata." + dato.Columna + " = " + dato.Valor + " AND ";
        else if (dato.TipoDato == 6 || dato.TipoDato == 3 || dato.TipoDato == 4)
        {
          if (dato.Id != 7)
            str = str + "lower(metadata." + dato.Columna + ") like lower('%" + dato.Valor + "%') AND ";
          else
            str = str + "(lower(metadata." + dato.Columna + ") like lower('%" + dato.Valor + "%') or lower(bl.blobname) like lower('%" + dato.Valor + "%')) AND ";
        }
        else if (dato.TipoDato == 5)
          str = str + "metadata." + dato.Columna + string.Format(">= TO_DATE('{0}', 'YYYY-MM-DD')", (object) DateTime.ParseExact(dato.Valor, "dd/MM/yyyy", (IFormatProvider) CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")) + " AND ";
      }
      string query = str + " bl.bloburl like '" + url + "%' and bl.blobtype = 'F'";
      System.Collections.Generic.List<WbBlobDTO> wbBlobDtoList = new System.Collections.Generic.List<WbBlobDTO>();
      object obj = this.dbProvider.ExecuteScalar(this.dbProvider.GetSqlStringCommand(query));
      return obj != null ? Convert.ToInt32(obj) : 0;
    }

    public int ObtenerNroRegistroBusqueda(string url, System.Collections.Generic.List<DatoItem> datos, int idFuente)
    {
      string str = "select count(*) from wb_blob bl left join wb_blobmetadata metadata on bl.blobcodi = metadata.blobcodi where ";
      foreach (DatoItem dato in datos)
      {
        if (dato.TipoDato == 1 || dato.TipoDato == 2)
          str = str + "metadata." + dato.Columna + " = " + dato.Valor + " AND ";
        else if (dato.TipoDato == 6 || dato.TipoDato == 3 || dato.TipoDato == 4)
        {
          if (dato.Id != 7)
            str = str + "lower(metadata." + dato.Columna + ") like lower('%" + dato.Valor + "%') AND ";
          else
            str = str + "(lower(metadata." + dato.Columna + ") like lower('%" + dato.Valor + "%') or lower(bl.blobname) like lower('%" + dato.Valor + "%')) AND ";
        }
        else if (dato.TipoDato == 5)
          str = str + "metadata." + dato.Columna + string.Format(">= TO_DATE('{0}', 'YYYY-MM-DD')", (object) DateTime.ParseExact(dato.Valor, "dd/MM/yyyy", (IFormatProvider) CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")) + " AND ";
      }
      string query = str + " bl.bloburl like '" + url + "%' and bl.blobtype = 'F' and bl.blobstate = 'A' and bl.blofuecodi = " + idFuente.ToString();
      System.Collections.Generic.List<WbBlobDTO> wbBlobDtoList = new System.Collections.Generic.List<WbBlobDTO>();
      object obj = this.dbProvider.ExecuteScalar(this.dbProvider.GetSqlStringCommand(query));
      return obj != null ? Convert.ToInt32(obj) : 0;
    }

    public int ObtenerNroRegistrosConsultaPortal(
      string texto,
      string extension,
      System.Collections.Generic.List<WbColumntypeDTO> columnas)
    {
      string str1 = "select count(*) from \r\n                wb_blob bl left join wb_blobmetadata metadata\r\n                on bl.blobcodi = metadata.blobcodi where bl.blobtype='F' and (upper(bl.blobname) like upper('%{0}%.%{1}%') or upper(bl.bloburl) like upper('%{0}%.%{1}%')  ";
      using (System.Collections.Generic.List<WbColumntypeDTO>.Enumerator enumerator = columnas.GetEnumerator())
      {
label_5:
        while (enumerator.MoveNext())
        {
          WbColumntypeDTO current = enumerator.Current;
          int num1 = 1;
          while (true)
          {
            int num2 = num1;
            int? typemaxcount = current.Typemaxcount;
            int valueOrDefault = typemaxcount.GetValueOrDefault();
            if (num2 <= valueOrDefault & typemaxcount.HasValue)
            {
              str1 = str1 + " or upper(metadata." + current.Typeunique + num1.ToString() + ") like upper('%{0}%{2}')";
              ++num1;
            }
            else
              goto label_5;
          }
        }
      }
      string format = str1 + ")";
      string str2 = !string.IsNullOrEmpty(extension) ? ".%" + extension + "%" : string.Empty;
      object obj = this.dbProvider.ExecuteScalar(this.dbProvider.GetSqlStringCommand(string.Format(format, (object) texto, (object) extension, (object) str2)));
      return obj != null ? Convert.ToInt32(obj) : 0;
    }

    public int ObtenerNroRegistrosConsultaPortal(
      string texto,
      string extension,
      System.Collections.Generic.List<WbColumntypeDTO> columnas,
      int idFuente)
    {
      string str1 = "select count(*) from wb_blob bl left join wb_blobmetadata metadata on bl.blobcodi = metadata.blobcodi where bl.blobtype='F' and bl.blobstate = 'A' and bl.blofuecodi = {3} and (upper(bl.blobname) like upper('%{0}%.%{1}%') or upper(bl.bloburl) like upper('%{0}%.%{1}%')  ";
      using (System.Collections.Generic.List<WbColumntypeDTO>.Enumerator enumerator = columnas.GetEnumerator())
      {
label_5:
        while (enumerator.MoveNext())
        {
          WbColumntypeDTO current = enumerator.Current;
          int num1 = 1;
          while (true)
          {
            int num2 = num1;
            int? typemaxcount = current.Typemaxcount;
            int valueOrDefault = typemaxcount.GetValueOrDefault();
            if (num2 <= valueOrDefault & typemaxcount.HasValue)
            {
              str1 = str1 + " or upper(metadata." + current.Typeunique + num1.ToString() + ") like upper('%{0}%{2}')";
              ++num1;
            }
            else
              goto label_5;
          }
        }
      }
      string format = str1 + ")";
      string str2 = !string.IsNullOrEmpty(extension) ? ".%" + extension + "%" : string.Empty;
      object obj = this.dbProvider.ExecuteScalar(this.dbProvider.GetSqlStringCommand(string.Format(format, (object) texto, (object) extension, (object) str2, (object) idFuente)));
      return obj != null ? Convert.ToInt32(obj) : 0;
    }

    public System.Collections.Generic.List<WbBlobDTO> BuscarArchivosPortal(
      string texto,
      string extension,
      System.Collections.Generic.List<WbColumntypeDTO> columnas,
      int nroPagina,
      int nroFilas)
    {
      string str1 = "select * from (select bl.* ";
      using (System.Collections.Generic.List<WbColumntypeDTO>.Enumerator enumerator = columnas.GetEnumerator())
      {
label_5:
        while (enumerator.MoveNext())
        {
          WbColumntypeDTO current = enumerator.Current;
          int num1 = 1;
          while (true)
          {
            int num2 = num1;
            int? typemaxcount = current.Typemaxcount;
            int valueOrDefault = typemaxcount.GetValueOrDefault();
            if (num2 <= valueOrDefault & typemaxcount.HasValue)
            {
              str1 = str1 + ", metadata." + current.Typeunique + num1.ToString();
              ++num1;
            }
            else
              goto label_5;
          }
        }
      }
      string str2 = str1 + ", (row_number() over (order by bl.blobcodi asc)) as r from \r\n                wb_blob bl left join wb_blobmetadata metadata\r\n                on bl.blobcodi = metadata.blobcodi where bl.blobtype='F' and (upper(bl.blobname) like upper('%{0}%.%{3}%') or upper(bl.bloburl) like upper('%{0}%.%{3}%')   ";
      string str3 = !string.IsNullOrEmpty(extension) ? ".%" + extension + "%" : string.Empty;
      using (System.Collections.Generic.List<WbColumntypeDTO>.Enumerator enumerator = columnas.GetEnumerator())
      {
label_12:
        while (enumerator.MoveNext())
        {
          WbColumntypeDTO current = enumerator.Current;
          int num1 = 1;
          while (true)
          {
            int num2 = num1;
            int? typemaxcount = current.Typemaxcount;
            int valueOrDefault = typemaxcount.GetValueOrDefault();
            if (num2 <= valueOrDefault & typemaxcount.HasValue)
            {
              str2 = str2 + " or upper(metadata." + current.Typeunique + num1.ToString() + ") like upper('%{0}%{4}')";
              ++num1;
            }
            else
              goto label_12;
          }
        }
      }
      string query = string.Format(str2 + ")) where (r >= ((({1}-1) * {2}) + 1) and r < (({1} * {2}) + 1 )) or ({1} = -1 and {2} = -1)", (object) texto, (object) nroPagina, (object) nroFilas, (object) extension, (object) str3);
      System.Collections.Generic.List<WbBlobDTO> wbBlobDtoList = new System.Collections.Generic.List<WbBlobDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(query)))
      {
        while (dr.Read())
        {
          WbBlobDTO wbBlobDto = this.helper.Create(dr);
          string str4 = string.Empty;
          using (System.Collections.Generic.List<WbColumntypeDTO>.Enumerator enumerator = columnas.GetEnumerator())
          {
label_25:
            while (enumerator.MoveNext())
            {
              WbColumntypeDTO current = enumerator.Current;
              int num1 = 1;
              while (true)
              {
                int num2 = num1;
                int? typemaxcount = current.Typemaxcount;
                int valueOrDefault = typemaxcount.GetValueOrDefault();
                if (num2 <= valueOrDefault & typemaxcount.HasValue)
                {
                  int ordinal = dr.GetOrdinal(current.Typeunique + num1.ToString());
                  if (!dr.IsDBNull(ordinal))
                  {
                    string str5 = dr.GetString(ordinal);
                    if (!string.IsNullOrEmpty(str5))
                      str4 = str4 + str5 + " ";
                  }
                  ++num1;
                }
                else
                  goto label_25;
              }
            }
          }
          wbBlobDto.Metadata = str4;
          wbBlobDtoList.Add(wbBlobDto);
        }
      }
      return wbBlobDtoList;
    }

    public System.Collections.Generic.List<WbBlobDTO> BuscarArchivosPortal(
      string texto,
      string extension,
      System.Collections.Generic.List<WbColumntypeDTO> columnas,
      int nroPagina,
      int nroFilas,
      int idFuente)
    {
      string str1 = "select * from (select bl.* ";
      using (System.Collections.Generic.List<WbColumntypeDTO>.Enumerator enumerator = columnas.GetEnumerator())
      {
label_5:
        while (enumerator.MoveNext())
        {
          WbColumntypeDTO current = enumerator.Current;
          int num1 = 1;
          while (true)
          {
            int num2 = num1;
            int? typemaxcount = current.Typemaxcount;
            int valueOrDefault = typemaxcount.GetValueOrDefault();
            if (num2 <= valueOrDefault & typemaxcount.HasValue)
            {
              str1 = str1 + ", metadata." + current.Typeunique + num1.ToString();
              ++num1;
            }
            else
              goto label_5;
          }
        }
      }
      string str2 = str1 + ", (row_number() over (order by bl.blobcodi asc)) as r from \r\n                wb_blob bl left join wb_blobmetadata metadata\r\n                on bl.blobcodi = metadata.blobcodi where bl.blobtype='F' and bl.blobstate = 'A' and bl.blofuecodi = {5} and (upper(bl.blobname) like upper('%{0}%.%{3}%') or upper(bl.bloburl) like upper('%{0}%.%{3}%')   ";
      string str3 = !string.IsNullOrEmpty(extension) ? ".%" + extension + "%" : string.Empty;
      using (System.Collections.Generic.List<WbColumntypeDTO>.Enumerator enumerator = columnas.GetEnumerator())
      {
label_12:
        while (enumerator.MoveNext())
        {
          WbColumntypeDTO current = enumerator.Current;
          int num1 = 1;
          while (true)
          {
            int num2 = num1;
            int? typemaxcount = current.Typemaxcount;
            int valueOrDefault = typemaxcount.GetValueOrDefault();
            if (num2 <= valueOrDefault & typemaxcount.HasValue)
            {
              str2 = str2 + " or upper(metadata." + current.Typeunique + num1.ToString() + ") like upper('%{0}%{4}')";
              ++num1;
            }
            else
              goto label_12;
          }
        }
      }
      string query = string.Format(str2 + ")) where (r >= ((({1}-1) * {2}) + 1) and r < (({1} * {2}) + 1 )) or ({1} = -1 and {2} = -1)", (object) texto, (object) nroPagina, (object) nroFilas, (object) extension, (object) str3, (object) idFuente);
      System.Collections.Generic.List<WbBlobDTO> wbBlobDtoList = new System.Collections.Generic.List<WbBlobDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(query)))
      {
        while (dr.Read())
        {
          WbBlobDTO wbBlobDto = this.helper.Create(dr);
          string str4 = string.Empty;
          using (System.Collections.Generic.List<WbColumntypeDTO>.Enumerator enumerator = columnas.GetEnumerator())
          {
label_25:
            while (enumerator.MoveNext())
            {
              WbColumntypeDTO current = enumerator.Current;
              int num1 = 1;
              while (true)
              {
                int num2 = num1;
                int? typemaxcount = current.Typemaxcount;
                int valueOrDefault = typemaxcount.GetValueOrDefault();
                if (num2 <= valueOrDefault & typemaxcount.HasValue)
                {
                  int ordinal = dr.GetOrdinal(current.Typeunique + num1.ToString());
                  if (!dr.IsDBNull(ordinal))
                  {
                    string str5 = dr.GetString(ordinal);
                    if (!string.IsNullOrEmpty(str5))
                      str4 = str4 + str5 + " ";
                  }
                  ++num1;
                }
                else
                  goto label_25;
              }
            }
          }
          wbBlobDto.Metadata = str4;
          wbBlobDtoList.Add(wbBlobDto);
        }
      }
      return wbBlobDtoList;
    }
  }
}

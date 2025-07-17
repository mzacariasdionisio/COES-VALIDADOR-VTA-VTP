// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Data.WbBlobmetadataRepository
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata.Entidad;
using COES.Storage.App.Metadata.Helper;
using System;
using System.Data;
using System.Data.Common;

namespace COES.Storage.App.Metadata.Data
{
  public class WbBlobmetadataRepository : RepositoryBase
  {
    private WbBlobmetadataHelper helper = new WbBlobmetadataHelper();

    public WbBlobmetadataRepository(string strConn)
      : base(strConn)
    {
    }

    public void Save(int blobCodi, System.Collections.Generic.List<BlobDatoDTO> list)
    {
      object obj = this.dbProvider.ExecuteScalar(this.dbProvider.GetSqlStringCommand(this.helper.SqlGetMaxId));
      int num = 1;
      if (obj != null)
        num = Convert.ToInt32(obj);
      string str1 = "insert into wb_blobmetadata(";
      foreach (BlobDatoDTO blobDatoDto in list)
        str1 = str1 + blobDatoDto.Campo + ",";
      string str2 = str1 + " blobcodi, metadatacodi) values (";
      foreach (BlobDatoDTO blobDatoDto in list)
        str2 = str2 + this.ObtenerValorTipoDato(blobDatoDto.TipoDato, blobDatoDto.Valor) + ",";
      this.dbProvider.ExecuteNonQuery(this.dbProvider.GetSqlStringCommand(str2 + blobCodi.ToString() + "," + num.ToString() + ")"));
    }

    public void Update(int blobCodi, System.Collections.Generic.List<BlobDatoDTO> list)
    {
      string str = "update wb_blobmetadata set ";
      int num = 0;
      foreach (BlobDatoDTO blobDatoDto in list)
      {
        str = str + blobDatoDto.Campo + " = " + this.ObtenerValorTipoDato(blobDatoDto.TipoDato, blobDatoDto.Valor);
        if (num < list.Count - 1)
          str += ",";
        ++num;
      }
      this.dbProvider.ExecuteNonQuery(this.dbProvider.GetSqlStringCommand(str + " where blobcodi = " + blobCodi.ToString()));
    }

    public string ObtenerValorTipoDato(int tipoDato, string valor)
    {
      if (tipoDato == 6 || tipoDato == 3 || tipoDato == 4 || tipoDato == 7)
        return "'" + valor + "'";
      return tipoDato == 5 ? string.Format("TO_DATE('{0}','DD/MM/YYYY')", (object) valor) : valor;
    }

    public void Delete(int blobcodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlDelete);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobcodi, DbType.Int32, (object) blobcodi);
      this.dbProvider.ExecuteNonQuery(sqlStringCommand);
    }

    public WbBlobmetadataDTO GetById(int metadatacodi, int blobcodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlGetById);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Metadatacodi, DbType.Int32, (object) metadatacodi);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobcodi, DbType.Int32, (object) blobcodi);
      WbBlobmetadataDTO wbBlobmetadataDto = (WbBlobmetadataDTO) null;
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        if (dr.Read())
          wbBlobmetadataDto = this.helper.Create(dr);
      }
      return wbBlobmetadataDto;
    }

    public System.Collections.Generic.List<WbBlobmetadataDTO> List()
    {
      System.Collections.Generic.List<WbBlobmetadataDTO> wbBlobmetadataDtoList = new System.Collections.Generic.List<WbBlobmetadataDTO>();
      using (IDataReader dr = this.dbProvider.ExecuteReader(this.dbProvider.GetSqlStringCommand(this.helper.SqlList)))
      {
        while (dr.Read())
          wbBlobmetadataDtoList.Add(this.helper.Create(dr));
      }
      return wbBlobmetadataDtoList;
    }

    public System.Collections.Generic.List<WbBlobmetadataDTO> GetByCriteria(
      int idPadre)
    {
      System.Collections.Generic.List<WbBlobmetadataDTO> wbBlobmetadataDtoList = new System.Collections.Generic.List<WbBlobmetadataDTO>();
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlGetByCriteria);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobcodi, DbType.Int32, (object) idPadre);
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        while (dr.Read())
          wbBlobmetadataDtoList.Add(this.helper.Create(dr));
      }
      return wbBlobmetadataDtoList;
    }

    public int ObtenerPorBlob(int blobCodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlObtenerPorBlob);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobcodi, DbType.Int32, (object) blobCodi);
      object obj = this.dbProvider.ExecuteScalar(sqlStringCommand);
      return obj != null ? Convert.ToInt32(obj) : 0;
    }

    public WbBlobmetadataDTO ObtenerMetadato(int blobcodi)
    {
      DbCommand sqlStringCommand = this.dbProvider.GetSqlStringCommand(this.helper.SqlObtenerMetadato);
      this.dbProvider.AddInParameter(sqlStringCommand, this.helper.Blobcodi, DbType.Int32, (object) blobcodi);
      WbBlobmetadataDTO wbBlobmetadataDto = (WbBlobmetadataDTO) null;
      using (IDataReader dr = this.dbProvider.ExecuteReader(sqlStringCommand))
      {
        if (dr.Read())
          wbBlobmetadataDto = this.helper.Create(dr);
      }
      return wbBlobmetadataDto;
    }
  }
}

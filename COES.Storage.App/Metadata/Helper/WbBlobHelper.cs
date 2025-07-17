// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Helper.WbBlobHelper
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata.Entidad;
using System;
using System.Data;

namespace COES.Storage.App.Metadata.Helper
{
  public class WbBlobHelper : HelperBase
  {
    public string Blobcodi = "BLOBCODI";
    public string Configcodi = "CONFIGCODI";
    public string Bloburl = "BLOBURL";
    public string Padrecodi = "PADRECODI";
    public string Blobname = "BLOBNAME";
    public string Blobsize = "BLOBSIZE";
    public string Blobdatecreated = "BLOBDATECREATED";
    public string Blobusercreate = "BLOBUSERCREATE";
    public string Blobdateupdate = "BLOBDATEUPDATE";
    public string Blobuserupdate = "BLOBUSERUPDATE";
    public string Blobstate = "BLOBSTATE";
    public string Blobtype = "BLOBTYPE";
    public string Blobfoldertype = "BLOBFOLDERTYPE";
    public string Usercode = "USERCODE";
    public string Blobissuu = "BLOBISSUU";
    public string Blobissuulink = "BLOBISSUULINK";
    public string Blobissuupos = "BLOBISSUUPOS";
    public string Blobissuulenx = "BLOBISSUULENX";
    public string Blobissuuleny = "BLOBISSUULENY";
    public string Blobhiddcol = "BLOBHIDDCOL";
    public string Blobbreadname = "BLOBBREADNAME";
    public string Bloborderfolder = "BLOBORDERFOLDER";
    public string Blobhide = "BLOBHIDE";
    public string Indtree = "INDTREE";
    public string Blobtreepadre = "BLOBTREEPADRE";
    public string Blobfuente = "BLOBFUENTE";
    public string Blofuecodi = "BLOFUECODI";

    public WbBlobHelper()
      : base(Consultas.WbBlobSql)
    {
    }

    public WbBlobDTO Create(IDataReader dr)
    {
      WbBlobDTO wbBlobDto = new WbBlobDTO();
      int ordinal1 = dr.GetOrdinal(this.Blobcodi);
      if (!dr.IsDBNull(ordinal1))
        wbBlobDto.Blobcodi = Convert.ToInt32(dr.GetValue(ordinal1));
      int ordinal2 = dr.GetOrdinal(this.Configcodi);
      if (!dr.IsDBNull(ordinal2))
        wbBlobDto.Configcodi = new int?(Convert.ToInt32(dr.GetValue(ordinal2)));
      int ordinal3 = dr.GetOrdinal(this.Bloburl);
      if (!dr.IsDBNull(ordinal3))
        wbBlobDto.Bloburl = dr.GetString(ordinal3);
      int ordinal4 = dr.GetOrdinal(this.Padrecodi);
      if (!dr.IsDBNull(ordinal4))
        wbBlobDto.Padrecodi = new int?(Convert.ToInt32(dr.GetValue(ordinal4)));
      int ordinal5 = dr.GetOrdinal(this.Blobname);
      if (!dr.IsDBNull(ordinal5))
        wbBlobDto.Blobname = dr.GetString(ordinal5);
      int ordinal6 = dr.GetOrdinal(this.Blobsize);
      if (!dr.IsDBNull(ordinal6))
        wbBlobDto.Blobsize = dr.GetString(ordinal6);
      int ordinal7 = dr.GetOrdinal(this.Blobdatecreated);
      if (!dr.IsDBNull(ordinal7))
        wbBlobDto.Blobdatecreated = new DateTime?(dr.GetDateTime(ordinal7));
      int ordinal8 = dr.GetOrdinal(this.Blobusercreate);
      if (!dr.IsDBNull(ordinal8))
        wbBlobDto.Blobusercreate = dr.GetString(ordinal8);
      int ordinal9 = dr.GetOrdinal(this.Blobdateupdate);
      if (!dr.IsDBNull(ordinal9))
        wbBlobDto.Blobdateupdate = new DateTime?(dr.GetDateTime(ordinal9));
      int ordinal10 = dr.GetOrdinal(this.Blobuserupdate);
      if (!dr.IsDBNull(ordinal10))
        wbBlobDto.Blobuserupdate = dr.GetString(ordinal10);
      int ordinal11 = dr.GetOrdinal(this.Blobstate);
      if (!dr.IsDBNull(ordinal11))
        wbBlobDto.Blobstate = dr.GetString(ordinal11);
      int ordinal12 = dr.GetOrdinal(this.Blobtype);
      if (!dr.IsDBNull(ordinal12))
        wbBlobDto.Blobtype = dr.GetString(ordinal12);
      int ordinal13 = dr.GetOrdinal(this.Blobfoldertype);
      if (!dr.IsDBNull(ordinal13))
        wbBlobDto.Blobfoldertype = dr.GetString(ordinal13);
      int ordinal14 = dr.GetOrdinal(this.Blofuecodi);
      if (!dr.IsDBNull(ordinal14))
        wbBlobDto.Blofuecodi = new int?(Convert.ToInt32(dr.GetValue(ordinal14)));
      return wbBlobDto;
    }

    public string SqlObtenerBlobByUrl => this.GetSqlXml("ObtenerBlobByUrl");

    public string SqlObtenerBlobByUrl2 => this.GetSqlXml("ObtenerBlobByUrl2");

    public string SqlObtenerPorPadre => this.GetSqlXml("ObtenerPorPadre");

    public string SqlObtenerFoldersPrincipales => this.GetSqlXml("ObtenerFoldersPrincipales");

    public string SqlObtenerCarpetasPorUsuario => this.GetSqlXml("ObtenerCarpetasPorUsuario");

    public string SqlObtenerBlobs => this.GetSqlXml("ObtenerBlobs");

    public string SqlEliminarRecursivo => this.GetSqlXml("EliminarRecursivo");

    public string SqlActualizarArchivo => this.GetSqlXml("ActualizarArchivo");

    public string SqlActualizarTipoLibreria => this.GetSqlXml("ActualizarTipoLibreria");

    public string SqlEliminarMetadataRecursivo => this.GetSqlXml("EliminarMetadataRecursivo");

    public string SqlActualizarUrl => this.GetSqlXml("ActualizarUrl");

    public string SqlActualizarIssuu => this.GetSqlXml("ActualizarIssuu");

    public string SqlActualizarVisualizacion => this.GetSqlXml("ActualizarVisualizacion");

    public string SqlOcultarBlob => this.GetSqlXml("OcultarBlob");

    public string SqlActualizarArbol => this.GetSqlXml("ActualizarArbol");

    public string SqlActualizarPadreArbol => this.GetSqlXml("ActualizarPadreArbol");

  }
}

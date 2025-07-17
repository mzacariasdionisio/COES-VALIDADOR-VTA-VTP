// Decompiled with JetBrains decompiler
// Type: COES.Storage.App.Metadata.Helper.WbBlobmetadataHelper
// Assembly: COES.Storage.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 08842ED7-FE70-4C24-AA93-4783F0BAE549
// Assembly location: C:\Users\jose.delgado\source\repos\ENPRUEBAS\MASTER\COES.MVC.Publico\bin\COES.Storage.App.dll

using COES.Storage.App.Base.Core;
using COES.Storage.App.Metadata.Entidad;
using System;
using System.Data;

namespace COES.Storage.App.Metadata.Helper
{
  public class WbBlobmetadataHelper : HelperBase
  {
    public string Metadatacodi = "METADATACODI";
    public string Blobcodi = "BLOBCODI";
    public string COLUMNINT1 = nameof (COLUMNINT1);
    public string COLUMNNUMBER1 = nameof (COLUMNNUMBER1);
    public string COLUMNVARCHAR1 = nameof (COLUMNVARCHAR1);
    public string COLUMNDATE1 = nameof (COLUMNDATE1);
    public string COLUMNINT2 = nameof (COLUMNINT2);
    public string COLUMNINT3 = nameof (COLUMNINT3);
    public string COLUMNINT4 = nameof (COLUMNINT4);
    public string COLUMNINT5 = nameof (COLUMNINT5);
    public string COLUMNINT6 = nameof (COLUMNINT6);
    public string COLUMNNUMBER2 = nameof (COLUMNNUMBER2);
    public string COLUMNNUMBER3 = nameof (COLUMNNUMBER3);
    public string COLUMNNUMBER4 = nameof (COLUMNNUMBER4);
    public string COLUMNNUMBER5 = nameof (COLUMNNUMBER5);
    public string COLUMNNUMBER6 = nameof (COLUMNNUMBER6);
    public string COLUMNVARCHAR2 = nameof (COLUMNVARCHAR2);
    public string COLUMNVARCHAR3 = nameof (COLUMNVARCHAR3);
    public string COLUMNVARCHAR4 = nameof (COLUMNVARCHAR4);
    public string COLUMNVARCHAR5 = nameof (COLUMNVARCHAR5);
    public string COLUMNVARCHAR6 = nameof (COLUMNVARCHAR6);
    public string COLUMNVARCHAR7 = nameof (COLUMNVARCHAR7);
    public string COLUMNVARCHAR8 = nameof (COLUMNVARCHAR8);
    public string COLUMNVARCHAR9 = nameof (COLUMNVARCHAR9);
    public string COLUMNVARCHAR10 = nameof (COLUMNVARCHAR10);
    public string COLUMNDATE2 = nameof (COLUMNDATE2);
    public string COLUMNDATE3 = nameof (COLUMNDATE3);
    public string COLUMNDATE4 = nameof (COLUMNDATE4);
    public string COLUMNDATE5 = nameof (COLUMNDATE5);
    public string COLUMNDATE6 = nameof (COLUMNDATE6);
    public string COLUMNTEXT1 = nameof (COLUMNTEXT1);
    public string COLUMNTEXT2 = nameof (COLUMNTEXT2);
    public string COLUMNTEXT3 = nameof (COLUMNTEXT3);
    public string COLUMNTEXT4 = nameof (COLUMNTEXT4);
    public string COLUMNTEXT5 = nameof (COLUMNTEXT5);
    public string Lastuser = "LASTUSER";
    public string Lastdate = "LASTDATE";
    public string COLUMNLIST1 = nameof (COLUMNLIST1);
    public string COLUMNLIST2 = nameof (COLUMNLIST2);
    public string COLUMNLIST3 = nameof (COLUMNLIST3);
    public string COLUMNLIST4 = nameof (COLUMNLIST4);
    public string COLUMNLIST5 = nameof (COLUMNLIST5);
    public string COLUMNLIST6 = nameof (COLUMNLIST6);
    public string COLUMNLIST7 = nameof (COLUMNLIST7);
    public string COLUMNLIST8 = nameof (COLUMNLIST8);
    public string COLUMNLIST9 = nameof (COLUMNLIST9);
    public string COLUMNLIST10 = nameof (COLUMNLIST10);
    public string COLUMNLIST11 = nameof (COLUMNLIST11);
    public string COLUMNLIST12 = nameof (COLUMNLIST12);
    public string COLUMNLIST13 = nameof (COLUMNLIST13);
    public string COLUMNLIST14 = nameof (COLUMNLIST14);
    public string COLUMNLIST15 = nameof (COLUMNLIST15);
    public string COLUMNLIST16 = nameof (COLUMNLIST16);
    public string COLUMNLIST17 = nameof (COLUMNLIST17);
    public string COLUMNLIST18 = nameof (COLUMNLIST18);
    public string COLUMNLIST19 = nameof (COLUMNLIST19);
    public string COLUMNLIST20 = nameof (COLUMNLIST20);
    public string COLUMNLIST21 = nameof (COLUMNLIST21);
    public string COLUMNLIST22 = nameof (COLUMNLIST22);
    public string COLUMNLIST23 = nameof (COLUMNLIST23);
    public string COLUMNLIST24 = nameof (COLUMNLIST24);
    public string COLUMNLIST25 = nameof (COLUMNLIST25);
    public string COLUMNLIST26 = nameof (COLUMNLIST26);
    public string COLUMNCLOB1 = nameof (COLUMNCLOB1);
    public string COLUMNTEXT6 = nameof (COLUMNTEXT6);

    public WbBlobmetadataHelper()
      : base(Consultas.WbBlobmetadataSql)
    {
    }

    public WbBlobmetadataDTO Create(IDataReader dr)
    {
      WbBlobmetadataDTO wbBlobmetadataDto = new WbBlobmetadataDTO();
      int ordinal1 = dr.GetOrdinal(this.Metadatacodi);
      if (!dr.IsDBNull(ordinal1))
        wbBlobmetadataDto.Metadatacodi = Convert.ToInt32(dr.GetValue(ordinal1));
      int ordinal2 = dr.GetOrdinal(this.Blobcodi);
      if (!dr.IsDBNull(ordinal2))
        wbBlobmetadataDto.Blobcodi = Convert.ToInt32(dr.GetValue(ordinal2));
      int ordinal3 = dr.GetOrdinal(this.COLUMNINT1);
      if (!dr.IsDBNull(ordinal3))
        wbBlobmetadataDto.COLUMNINT1 = new int?(Convert.ToInt32(dr.GetValue(ordinal3)));
      int ordinal4 = dr.GetOrdinal(this.COLUMNNUMBER1);
      if (!dr.IsDBNull(ordinal4))
        wbBlobmetadataDto.COLUMNNUMBER1 = new Decimal?(dr.GetDecimal(ordinal4));
      int ordinal5 = dr.GetOrdinal(this.COLUMNVARCHAR1);
      if (!dr.IsDBNull(ordinal5))
        wbBlobmetadataDto.COLUMNVARCHAR1 = dr.GetString(ordinal5);
      int ordinal6 = dr.GetOrdinal(this.COLUMNDATE1);
      if (!dr.IsDBNull(ordinal6))
        wbBlobmetadataDto.COLUMNDATE1 = new DateTime?(dr.GetDateTime(ordinal6));
      int ordinal7 = dr.GetOrdinal(this.COLUMNINT2);
      if (!dr.IsDBNull(ordinal7))
        wbBlobmetadataDto.COLUMNINT2 = new int?(Convert.ToInt32(dr.GetValue(ordinal7)));
      int ordinal8 = dr.GetOrdinal(this.COLUMNINT3);
      if (!dr.IsDBNull(ordinal8))
        wbBlobmetadataDto.COLUMNINT3 = new int?(Convert.ToInt32(dr.GetValue(ordinal8)));
      int ordinal9 = dr.GetOrdinal(this.COLUMNINT4);
      if (!dr.IsDBNull(ordinal9))
        wbBlobmetadataDto.COLUMNINT4 = new int?(Convert.ToInt32(dr.GetValue(ordinal9)));
      int ordinal10 = dr.GetOrdinal(this.COLUMNINT5);
      if (!dr.IsDBNull(ordinal10))
        wbBlobmetadataDto.COLUMNINT5 = new int?(Convert.ToInt32(dr.GetValue(ordinal10)));
      int ordinal11 = dr.GetOrdinal(this.COLUMNINT6);
      if (!dr.IsDBNull(ordinal11))
        wbBlobmetadataDto.COLUMNINT6 = new int?(Convert.ToInt32(dr.GetValue(ordinal11)));
      int ordinal12 = dr.GetOrdinal(this.COLUMNNUMBER2);
      if (!dr.IsDBNull(ordinal12))
        wbBlobmetadataDto.COLUMNNUMBER2 = new Decimal?(dr.GetDecimal(ordinal12));
      int ordinal13 = dr.GetOrdinal(this.COLUMNNUMBER3);
      if (!dr.IsDBNull(ordinal13))
        wbBlobmetadataDto.COLUMNNUMBER3 = new Decimal?(dr.GetDecimal(ordinal13));
      int ordinal14 = dr.GetOrdinal(this.COLUMNNUMBER4);
      if (!dr.IsDBNull(ordinal14))
        wbBlobmetadataDto.COLUMNNUMBER4 = new Decimal?(dr.GetDecimal(ordinal14));
      int ordinal15 = dr.GetOrdinal(this.COLUMNNUMBER5);
      if (!dr.IsDBNull(ordinal15))
        wbBlobmetadataDto.COLUMNNUMBER5 = new Decimal?(dr.GetDecimal(ordinal15));
      int ordinal16 = dr.GetOrdinal(this.COLUMNNUMBER6);
      if (!dr.IsDBNull(ordinal16))
        wbBlobmetadataDto.COLUMNNUMBER6 = new Decimal?(dr.GetDecimal(ordinal16));
      int ordinal17 = dr.GetOrdinal(this.COLUMNVARCHAR2);
      if (!dr.IsDBNull(ordinal17))
        wbBlobmetadataDto.COLUMNVARCHAR2 = dr.GetString(ordinal17);
      int ordinal18 = dr.GetOrdinal(this.COLUMNVARCHAR3);
      if (!dr.IsDBNull(ordinal18))
        wbBlobmetadataDto.COLUMNVARCHAR3 = dr.GetString(ordinal18);
      int ordinal19 = dr.GetOrdinal(this.COLUMNVARCHAR4);
      if (!dr.IsDBNull(ordinal19))
        wbBlobmetadataDto.COLUMNVARCHAR4 = dr.GetString(ordinal19);
      int ordinal20 = dr.GetOrdinal(this.COLUMNVARCHAR5);
      if (!dr.IsDBNull(ordinal20))
        wbBlobmetadataDto.COLUMNVARCHAR5 = dr.GetString(ordinal20);
      int ordinal21 = dr.GetOrdinal(this.COLUMNVARCHAR6);
      if (!dr.IsDBNull(ordinal21))
        wbBlobmetadataDto.COLUMNVARCHAR6 = dr.GetString(ordinal21);
      int ordinal22 = dr.GetOrdinal(this.COLUMNVARCHAR7);
      if (!dr.IsDBNull(ordinal22))
        wbBlobmetadataDto.COLUMNVARCHAR7 = dr.GetString(ordinal22);
      int ordinal23 = dr.GetOrdinal(this.COLUMNVARCHAR8);
      if (!dr.IsDBNull(ordinal23))
        wbBlobmetadataDto.COLUMNVARCHAR8 = dr.GetString(ordinal23);
      int ordinal24 = dr.GetOrdinal(this.COLUMNVARCHAR9);
      if (!dr.IsDBNull(ordinal24))
        wbBlobmetadataDto.COLUMNVARCHAR9 = dr.GetString(ordinal24);
      int ordinal25 = dr.GetOrdinal(this.COLUMNVARCHAR10);
      if (!dr.IsDBNull(ordinal25))
        wbBlobmetadataDto.COLUMNVARCHAR10 = dr.GetString(ordinal25);
      int ordinal26 = dr.GetOrdinal(this.COLUMNDATE2);
      if (!dr.IsDBNull(ordinal26))
        wbBlobmetadataDto.COLUMNDATE2 = new DateTime?(dr.GetDateTime(ordinal26));
      int ordinal27 = dr.GetOrdinal(this.COLUMNDATE3);
      if (!dr.IsDBNull(ordinal27))
        wbBlobmetadataDto.COLUMNDATE3 = new DateTime?(dr.GetDateTime(ordinal27));
      int ordinal28 = dr.GetOrdinal(this.COLUMNDATE4);
      if (!dr.IsDBNull(ordinal28))
        wbBlobmetadataDto.COLUMNDATE4 = new DateTime?(dr.GetDateTime(ordinal28));
      int ordinal29 = dr.GetOrdinal(this.COLUMNDATE5);
      if (!dr.IsDBNull(ordinal29))
        wbBlobmetadataDto.COLUMNDATE5 = new DateTime?(dr.GetDateTime(ordinal29));
      int ordinal30 = dr.GetOrdinal(this.COLUMNDATE6);
      if (!dr.IsDBNull(ordinal30))
        wbBlobmetadataDto.COLUMNDATE6 = new DateTime?(dr.GetDateTime(ordinal30));
      int ordinal31 = dr.GetOrdinal(this.COLUMNTEXT1);
      if (!dr.IsDBNull(ordinal31))
        wbBlobmetadataDto.COLUMNTEXT1 = dr.GetString(ordinal31);
      int ordinal32 = dr.GetOrdinal(this.COLUMNTEXT2);
      if (!dr.IsDBNull(ordinal32))
        wbBlobmetadataDto.COLUMNTEXT2 = dr.GetString(ordinal32);
      int ordinal33 = dr.GetOrdinal(this.COLUMNTEXT3);
      if (!dr.IsDBNull(ordinal33))
        wbBlobmetadataDto.COLUMNTEXT3 = dr.GetString(ordinal33);
      int ordinal34 = dr.GetOrdinal(this.COLUMNTEXT4);
      if (!dr.IsDBNull(ordinal34))
        wbBlobmetadataDto.COLUMNTEXT4 = dr.GetString(ordinal34);
      int ordinal35 = dr.GetOrdinal(this.COLUMNTEXT5);
      if (!dr.IsDBNull(ordinal35))
        wbBlobmetadataDto.COLUMNTEXT5 = dr.GetString(ordinal35);
      int ordinal36 = dr.GetOrdinal(this.COLUMNLIST1);
      if (!dr.IsDBNull(ordinal36))
        wbBlobmetadataDto.COLUMNLIST1 = dr.GetString(ordinal36);
      int ordinal37 = dr.GetOrdinal(this.COLUMNLIST2);
      if (!dr.IsDBNull(ordinal37))
        wbBlobmetadataDto.COLUMNLIST2 = dr.GetString(ordinal37);
      int ordinal38 = dr.GetOrdinal(this.COLUMNLIST3);
      if (!dr.IsDBNull(ordinal38))
        wbBlobmetadataDto.COLUMNLIST3 = dr.GetString(ordinal38);
      int ordinal39 = dr.GetOrdinal(this.COLUMNLIST4);
      if (!dr.IsDBNull(ordinal39))
        wbBlobmetadataDto.COLUMNLIST4 = dr.GetString(ordinal39);
      int ordinal40 = dr.GetOrdinal(this.COLUMNLIST5);
      if (!dr.IsDBNull(ordinal40))
        wbBlobmetadataDto.COLUMNLIST5 = dr.GetString(ordinal40);
      int ordinal41 = dr.GetOrdinal(this.COLUMNLIST6);
      if (!dr.IsDBNull(ordinal41))
        wbBlobmetadataDto.COLUMNLIST6 = dr.GetString(ordinal41);
      int ordinal42 = dr.GetOrdinal(this.COLUMNLIST7);
      if (!dr.IsDBNull(ordinal42))
        wbBlobmetadataDto.COLUMNLIST7 = dr.GetString(ordinal42);
      int ordinal43 = dr.GetOrdinal(this.COLUMNLIST8);
      if (!dr.IsDBNull(ordinal43))
        wbBlobmetadataDto.COLUMNLIST8 = dr.GetString(ordinal43);
      int ordinal44 = dr.GetOrdinal(this.COLUMNLIST9);
      if (!dr.IsDBNull(ordinal44))
        wbBlobmetadataDto.COLUMNLIST9 = dr.GetString(ordinal44);
      int ordinal45 = dr.GetOrdinal(this.COLUMNLIST10);
      if (!dr.IsDBNull(ordinal45))
        wbBlobmetadataDto.COLUMNLIST10 = dr.GetString(ordinal45);
      int ordinal46 = dr.GetOrdinal(this.COLUMNLIST11);
      if (!dr.IsDBNull(ordinal46))
        wbBlobmetadataDto.COLUMNLIST11 = dr.GetString(ordinal46);
      int ordinal47 = dr.GetOrdinal(this.COLUMNLIST12);
      if (!dr.IsDBNull(ordinal47))
        wbBlobmetadataDto.COLUMNLIST12 = dr.GetString(ordinal47);
      int ordinal48 = dr.GetOrdinal(this.COLUMNLIST13);
      if (!dr.IsDBNull(ordinal48))
        wbBlobmetadataDto.COLUMNLIST13 = dr.GetString(ordinal48);
      int ordinal49 = dr.GetOrdinal(this.COLUMNLIST14);
      if (!dr.IsDBNull(ordinal49))
        wbBlobmetadataDto.COLUMNLIST14 = dr.GetString(ordinal49);
      int ordinal50 = dr.GetOrdinal(this.COLUMNLIST15);
      if (!dr.IsDBNull(ordinal50))
        wbBlobmetadataDto.COLUMNLIST15 = dr.GetString(ordinal50);
      int ordinal51 = dr.GetOrdinal(this.COLUMNLIST16);
      if (!dr.IsDBNull(ordinal51))
        wbBlobmetadataDto.COLUMNLIST16 = dr.GetString(ordinal51);
      int ordinal52 = dr.GetOrdinal(this.COLUMNLIST17);
      if (!dr.IsDBNull(ordinal52))
        wbBlobmetadataDto.COLUMNLIST17 = dr.GetString(ordinal52);
      int ordinal53 = dr.GetOrdinal(this.COLUMNLIST18);
      if (!dr.IsDBNull(ordinal53))
        wbBlobmetadataDto.COLUMNLIST18 = dr.GetString(ordinal53);
      int ordinal54 = dr.GetOrdinal(this.COLUMNLIST19);
      if (!dr.IsDBNull(ordinal54))
        wbBlobmetadataDto.COLUMNLIST19 = dr.GetString(ordinal54);
      int ordinal55 = dr.GetOrdinal(this.COLUMNLIST20);
      if (!dr.IsDBNull(ordinal55))
        wbBlobmetadataDto.COLUMNLIST20 = dr.GetString(ordinal55);
      int ordinal56 = dr.GetOrdinal(this.COLUMNLIST21);
      if (!dr.IsDBNull(ordinal56))
        wbBlobmetadataDto.COLUMNLIST21 = dr.GetString(ordinal56);
      int ordinal57 = dr.GetOrdinal(this.COLUMNLIST22);
      if (!dr.IsDBNull(ordinal57))
        wbBlobmetadataDto.COLUMNLIST22 = dr.GetString(ordinal57);
      int ordinal58 = dr.GetOrdinal(this.COLUMNLIST23);
      if (!dr.IsDBNull(ordinal58))
        wbBlobmetadataDto.COLUMNLIST23 = dr.GetString(ordinal58);
      int ordinal59 = dr.GetOrdinal(this.COLUMNLIST24);
      if (!dr.IsDBNull(ordinal59))
        wbBlobmetadataDto.COLUMNLIST24 = dr.GetString(ordinal59);
      int ordinal60 = dr.GetOrdinal(this.COLUMNLIST25);
      if (!dr.IsDBNull(ordinal60))
        wbBlobmetadataDto.COLUMNLIST25 = dr.GetString(ordinal60);
      int ordinal61 = dr.GetOrdinal(this.COLUMNLIST26);
      if (!dr.IsDBNull(ordinal61))
        wbBlobmetadataDto.COLUMNLIST26 = dr.GetString(ordinal61);
      int ordinal62 = dr.GetOrdinal(this.Lastuser);
      if (!dr.IsDBNull(ordinal62))
        wbBlobmetadataDto.Lastuser = dr.GetString(ordinal62);
      int ordinal63 = dr.GetOrdinal(this.Lastdate);
      if (!dr.IsDBNull(ordinal63))
        wbBlobmetadataDto.Lastdate = new DateTime?(dr.GetDateTime(ordinal63));
      int ordinal64 = dr.GetOrdinal(this.COLUMNCLOB1);
      if (!dr.IsDBNull(ordinal64))
        wbBlobmetadataDto.COLUMNCLOB1 = dr.GetString(ordinal64);
      int ordinal65 = dr.GetOrdinal(this.COLUMNTEXT6);
      if (!dr.IsDBNull(ordinal65))
        wbBlobmetadataDto.COLUMNTEXT6 = dr.GetString(ordinal65);
      return wbBlobmetadataDto;
    }

    public string SqlObtenerPorBlob => this.GetSqlXml("ObtenerPorBlob");

    public string SqlObtenerMetadato => this.GetSqlXml("ObtenerMetadato");
  }
}

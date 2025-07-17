using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_BLOB
    /// </summary>
    public class WbBlobHelper : HelperBase
    {
        public WbBlobHelper() : base(Consultas.WbBlobSql)
        {
        }

        public WbBlobDTO Create(IDataReader dr)
        {
            WbBlobDTO entity = new WbBlobDTO();

            int iBlobcodi = dr.GetOrdinal(this.Blobcodi);
            if (!dr.IsDBNull(iBlobcodi)) entity.Blobcodi = Convert.ToInt32(dr.GetValue(iBlobcodi));

            int iConfigcodi = dr.GetOrdinal(this.Configcodi);
            if (!dr.IsDBNull(iConfigcodi)) entity.Configcodi = Convert.ToInt32(dr.GetValue(iConfigcodi));

            int iBloburl = dr.GetOrdinal(this.Bloburl);
            if (!dr.IsDBNull(iBloburl)) entity.Bloburl = dr.GetString(iBloburl);

            int iPadrecodi = dr.GetOrdinal(this.Padrecodi);
            if (!dr.IsDBNull(iPadrecodi)) entity.Padrecodi = Convert.ToInt32(dr.GetValue(iPadrecodi));

            int iBlobname = dr.GetOrdinal(this.Blobname);
            if (!dr.IsDBNull(iBlobname)) entity.Blobname = dr.GetString(iBlobname);

            int iBlobsize = dr.GetOrdinal(this.Blobsize);
            if (!dr.IsDBNull(iBlobsize)) entity.Blobsize = dr.GetString(iBlobsize);

            int iBlobdatecreated = dr.GetOrdinal(this.Blobdatecreated);
            if (!dr.IsDBNull(iBlobdatecreated)) entity.Blobdatecreated = dr.GetDateTime(iBlobdatecreated);

            int iBlobusercreate = dr.GetOrdinal(this.Blobusercreate);
            if (!dr.IsDBNull(iBlobusercreate)) entity.Blobusercreate = dr.GetString(iBlobusercreate);

            int iBlobdateupdate = dr.GetOrdinal(this.Blobdateupdate);
            if (!dr.IsDBNull(iBlobdateupdate)) entity.Blobdateupdate = dr.GetDateTime(iBlobdateupdate);

            int iBlobuserupdate = dr.GetOrdinal(this.Blobuserupdate);
            if (!dr.IsDBNull(iBlobuserupdate)) entity.Blobuserupdate = dr.GetString(iBlobuserupdate);

            int iBlobstate = dr.GetOrdinal(this.Blobstate);
            if (!dr.IsDBNull(iBlobstate)) entity.Blobstate = dr.GetString(iBlobstate);

            int iBlobtype = dr.GetOrdinal(this.Blobtype);
            if (!dr.IsDBNull(iBlobtype)) entity.Blobtype = dr.GetString(iBlobtype);

            int iBlobfoldertype = dr.GetOrdinal(this.Blobfoldertype);
            if (!dr.IsDBNull(iBlobfoldertype)) entity.Blobfoldertype = dr.GetString(iBlobfoldertype);

            int iBlobissuu = dr.GetOrdinal(this.Blobissuu);
            if (!dr.IsDBNull(iBlobissuu)) entity.Blobissuu = dr.GetString(iBlobissuu);

            int iBlobissuulink = dr.GetOrdinal(this.Blobissuulink);
            if (!dr.IsDBNull(iBlobissuulink)) entity.Blobissuulink = dr.GetString(iBlobissuulink);

            int iBlobissuupos = dr.GetOrdinal(this.Blobissuupos);
            if (!dr.IsDBNull(iBlobissuupos)) entity.Blobissuupos = dr.GetString(iBlobissuupos);

            int iBlobissuulenx = dr.GetOrdinal(this.Blobissuulenx);
            if (!dr.IsDBNull(iBlobissuulenx)) entity.Blobissuulenx = dr.GetString(iBlobissuulenx);

            int iBlobissuuleny = dr.GetOrdinal(this.Blobissuuleny);
            if (!dr.IsDBNull(iBlobissuuleny)) entity.Blobissuuleny = dr.GetString(iBlobissuuleny);

            int iBlobhiddcol = dr.GetOrdinal(this.Blobhiddcol);
            if (!dr.IsDBNull(iBlobhiddcol)) entity.Blobhiddcol = dr.GetString(iBlobhiddcol);

            int iBlobbreadname = dr.GetOrdinal(this.Blobbreadname);
            if (!dr.IsDBNull(iBlobbreadname)) entity.Blobbreadname = dr.GetString(iBlobbreadname);

            int iBloborderfolder = dr.GetOrdinal(this.Bloborderfolder);
            if (!dr.IsDBNull(iBloborderfolder)) entity.Bloborderfolder = dr.GetString(iBloborderfolder);

            int iBlobhide = dr.GetOrdinal(this.Blobhide);
            if (!dr.IsDBNull(iBlobhide)) entity.Blobhide = dr.GetString(iBlobhide);

            int iIndtree = dr.GetOrdinal(this.Indtree);
            if (!dr.IsDBNull(iIndtree)) entity.Indtree = dr.GetString(iIndtree);

            int iBlobtreepadre = dr.GetOrdinal(this.Blobtreepadre);
            if (!dr.IsDBNull(iBlobtreepadre)) entity.Blobtreepadre = Convert.ToInt32(dr.GetValue(iBlobtreepadre));

            int iBlobfuente = dr.GetOrdinal(this.Blobfuente);
            if (!dr.IsDBNull(iBlobfuente)) entity.Blobfuente = Convert.ToInt32(dr.GetValue(iBlobfuente));

            int iBlofuecodi = dr.GetOrdinal(this.Blofuecodi);
            if (!dr.IsDBNull(iBlofuecodi)) entity.Blofuecodi = Convert.ToInt32(dr.GetValue(iBlofuecodi));

            int iBlobconfidencial = dr.GetOrdinal(this.Blobconfidencial);
            if (!dr.IsDBNull(iBlobconfidencial)) entity.Blobconfidencial = Convert.ToInt32(dr.GetValue(iBlobconfidencial));

            return entity;
        }

        #region Mapeo de Campos

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
        public string Blobconfidencial = "BLOBCONFIDENCIAL";

        #endregion

        public string SqlObtenerBlobByUrl
        {
            get { return base.GetSqlXml("ObtenerBlobByUrl"); }
        }

        public string SqlObtenerBlobByUrl2
        {
            get { return base.GetSqlXml("ObtenerBlobByUrl2"); }
        }

        public string SqlObtenerPorPadre
        {
            get { return base.GetSqlXml("ObtenerPorPadre"); }
        }
        public string SqlGetByCodiPadre
        {
            get { return base.GetSqlXml("GetByCodiPadre"); }
        }
        
    }
}

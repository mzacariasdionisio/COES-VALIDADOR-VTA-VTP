using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_IMPUGNACION
    /// </summary>
    public class WbImpugnacionHelper : HelperBase
    {
        public WbImpugnacionHelper(): base(Consultas.WbImpugnacionSql)
        {
        }

        public WbImpugnacionDTO Create(IDataReader dr)
        {
            WbImpugnacionDTO entity = new WbImpugnacionDTO();

            int iImpgnombre = dr.GetOrdinal(this.Impgnombre);
            if (!dr.IsDBNull(iImpgnombre)) entity.Impgnombre = dr.GetString(iImpgnombre);

            int iImpgtitulo = dr.GetOrdinal(this.Impgtitulo);
            if (!dr.IsDBNull(iImpgtitulo)) entity.Impgtitulo = dr.GetString(iImpgtitulo);

            int iImpgnumeromes = dr.GetOrdinal(this.Impgnumeromes);
            if (!dr.IsDBNull(iImpgnumeromes)) entity.Impgnumeromes = Convert.ToInt32(dr.GetValue(iImpgnumeromes));

            int iImpgcodi = dr.GetOrdinal(this.Impgcodi);
            if (!dr.IsDBNull(iImpgcodi)) entity.Impgcodi = Convert.ToInt32(dr.GetValue(iImpgcodi));

            int iImpgregsgdoc = dr.GetOrdinal(this.Impgregsgdoc);
            if (!dr.IsDBNull(iImpgregsgdoc)) entity.Impgregsgdoc = dr.GetString(iImpgregsgdoc);

            int iImpginpugnante = dr.GetOrdinal(this.Impginpugnante);
            if (!dr.IsDBNull(iImpginpugnante)) entity.Impginpugnante = dr.GetString(iImpginpugnante);

            int iImpgdescinpugnad = dr.GetOrdinal(this.Impgdescinpugnad);
            if (!dr.IsDBNull(iImpgdescinpugnad)) entity.Impgdescinpugnad = dr.GetString(iImpgdescinpugnad);

            int iImpgpetitorio = dr.GetOrdinal(this.Impgpetitorio);
            if (!dr.IsDBNull(iImpgpetitorio)) entity.Impgpetitorio = dr.GetString(iImpgpetitorio);

            int iImpgfechrecep = dr.GetOrdinal(this.Impgfechrecep);
            if (!dr.IsDBNull(iImpgfechrecep)) entity.Impgfechrecep = dr.GetDateTime(iImpgfechrecep);

            int iImpgfechpubli = dr.GetOrdinal(this.Impgfechpubli);
            if (!dr.IsDBNull(iImpgfechpubli)) entity.Impgfechpubli = dr.GetDateTime(iImpgfechpubli);

            int iImpgplazincorp = dr.GetOrdinal(this.Impgplazincorp);
            if (!dr.IsDBNull(iImpgplazincorp)) entity.Impgplazincorp = dr.GetDateTime(iImpgplazincorp);

            int iImpgincorpresent = dr.GetOrdinal(this.Impgincorpresent);
            if (!dr.IsDBNull(iImpgincorpresent)) entity.Impgincorpresent = dr.GetString(iImpgincorpresent);

            int iImpgdescdirecc = dr.GetOrdinal(this.Impgdescdirecc);
            if (!dr.IsDBNull(iImpgdescdirecc)) entity.Impgdescdirecc = dr.GetString(iImpgdescdirecc);

            int iImpgfechdesc = dr.GetOrdinal(this.Impgfechdesc);
            if (!dr.IsDBNull(iImpgfechdesc)) entity.Impgfechdesc = dr.GetDateTime(iImpgfechdesc);

            int iImpgdiastotaten = dr.GetOrdinal(this.Impgdiastotaten);
            if (!dr.IsDBNull(iImpgdiastotaten)) entity.Impgdiastotaten = Convert.ToInt32(dr.GetValue(iImpgdiastotaten));

            int iImpgusuariocreacion = dr.GetOrdinal(this.Impgusuariocreacion);
            if (!dr.IsDBNull(iImpgusuariocreacion)) entity.Impgusuariocreacion = dr.GetString(iImpgusuariocreacion);

            int iImpgrutaarch = dr.GetOrdinal(this.Impgrutaarch);
            if (!dr.IsDBNull(iImpgrutaarch)) entity.Impgrutaarch = dr.GetString(iImpgrutaarch);

            int iTimpgcodi = dr.GetOrdinal(this.Timpgcodi);
            if (!dr.IsDBNull(iTimpgcodi)) entity.Timpgcodi = Convert.ToInt32(dr.GetValue(iTimpgcodi));

            int iImpgfechacreacion = dr.GetOrdinal(this.Impgfechacreacion);
            if (!dr.IsDBNull(iImpgfechacreacion)) entity.Impgfechacreacion = dr.GetDateTime(iImpgfechacreacion);

            int iImpgusuarioupdate = dr.GetOrdinal(this.Impgusuarioupdate);
            if (!dr.IsDBNull(iImpgusuarioupdate)) entity.Impgusuarioupdate = dr.GetString(iImpgusuarioupdate);

            int iImpgfechaupdate = dr.GetOrdinal(this.Impgfechaupdate);
            if (!dr.IsDBNull(iImpgfechaupdate)) entity.Impgfechaupdate = dr.GetDateTime(iImpgfechaupdate);

            int iImpgmesanio = dr.GetOrdinal(this.Impgmesanio);
            if (!dr.IsDBNull(iImpgmesanio)) entity.Impgmesanio = dr.GetDateTime(iImpgmesanio);

            int iImpgextension = dr.GetOrdinal(this.Impgextension);
            if (!dr.IsDBNull(iImpgextension)) entity.Impgextension = dr.GetString(iImpgextension);

            return entity;
        }


        #region Mapeo de Campos

        public string Impgnombre = "IMPGNOMBRE";
        public string Impgtitulo = "IMPGTITULO";
        public string Impgnumeromes = "IMPGNUMEROMES";
        public string Impgcodi = "IMPGCODI";
        public string Impgregsgdoc = "IMPGREGSGDOC";
        public string Impginpugnante = "IMPGINPUGNANTE";
        public string Impgdescinpugnad = "IMPGDESCINPUGNAD";
        public string Impgpetitorio = "IMPGPETITORIO";
        public string Impgfechrecep = "IMPGFECHRECEP";
        public string Impgfechpubli = "IMPGFECHPUBLI";
        public string Impgplazincorp = "IMPGPLAZINCORP";
        public string Impgincorpresent = "IMPGINCORPRESENT";
        public string Impgdescdirecc = "IMPGDESCDIRECC";
        public string Impgfechdesc = "IMPGFECHDESC";
        public string Impgdiastotaten = "IMPGDIASTOTATEN";
        public string Impgusuariocreacion = "IMPGUSUARIOCREACION";
        public string Impgrutaarch = "IMPGRUTAARCH";
        public string Timpgcodi = "TIMPGCODI";
        public string Impgfechacreacion = "IMPGFECHACREACION";
        public string Impgusuarioupdate = "IMPGUSUARIOUPDATE";
        public string Impgfechaupdate = "IMPGFECHAUPDATE";
        public string Impgmesanio = "IMPGMESANIO";
        public string Impgextension = "IMPGEXTENSION";

        #endregion
    }
}

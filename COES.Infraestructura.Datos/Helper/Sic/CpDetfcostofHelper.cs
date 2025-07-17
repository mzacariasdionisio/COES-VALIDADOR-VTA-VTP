using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_DETFCOSTOF
    /// </summary>
    public class CpDetfcostofHelper : HelperBase
    {
        public CpDetfcostofHelper(): base(Consultas.CpDetfcostofSql)
        {
        }

        public CpDetfcostofDTO Create(IDataReader dr)
        {
            CpDetfcostofDTO entity = new CpDetfcostofDTO();

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            int iDetfcfncorte = dr.GetOrdinal(this.Detfcfncorte);
            if (!dr.IsDBNull(iDetfcfncorte)) entity.Detfcfncorte = Convert.ToInt32(dr.GetValue(iDetfcfncorte));

            int iDetfcfvalores = dr.GetOrdinal(this.Detfcfvalores);
            if (!dr.IsDBNull(iDetfcfvalores)) entity.Detfcfvalores = dr.GetString(iDetfcfvalores);

            return entity;
        }


        #region Mapeo de Campos

        public string Topcodi = "TOPCODI";
        public string Detfcfncorte = "DETFCFNCORTE";
        public string Detfcfvalores = "DETFCFVALORES";

        #endregion

        public string SqlCrearCopia
        {
            get { return base.GetSqlXml("CrearCopia"); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CAI_SDDP_DURACION
    /// </summary>
    public class CaiSddpDuracionHelper : HelperBase
    {
        public CaiSddpDuracionHelper(): base(Consultas.CaiSddpDuracionSql)
        {
        }

        public CaiSddpDuracionDTO Create(IDataReader dr)
        {
            CaiSddpDuracionDTO entity = new CaiSddpDuracionDTO();

            int iSddpducodi = dr.GetOrdinal(this.Sddpducodi);
            if (!dr.IsDBNull(iSddpducodi)) entity.Sddpducodi = Convert.ToInt32(dr.GetValue(iSddpducodi));

            int iCaiajcodi = dr.GetOrdinal(this.Caiajcodi);
            if (!dr.IsDBNull(iCaiajcodi)) entity.Caiajcodi = Convert.ToInt32(dr.GetValue(iCaiajcodi));

            int iSddpduetapa = dr.GetOrdinal(this.Sddpduetapa);
            if (!dr.IsDBNull(iSddpduetapa)) entity.Sddpduetapa = Convert.ToInt32(dr.GetValue(iSddpduetapa));

            int iSddpduserie = dr.GetOrdinal(this.Sddpduserie);
            if (!dr.IsDBNull(iSddpduserie)) entity.Sddpduserie = Convert.ToInt32(dr.GetValue(iSddpduserie));

            int iSddpdubloque = dr.GetOrdinal(this.Sddpdubloque);
            if (!dr.IsDBNull(iSddpdubloque)) entity.Sddpdubloque = Convert.ToInt32(dr.GetValue(iSddpdubloque));

            int iSddpduduracion = dr.GetOrdinal(this.Sddpduduracion);
            if (!dr.IsDBNull(iSddpduduracion)) entity.Sddpduduracion = Convert.ToDecimal(dr.GetValue(iSddpduduracion));

            int iSddpduusucreacion = dr.GetOrdinal(this.Sddpduusucreacion);
            if (!dr.IsDBNull(iSddpduusucreacion)) entity.Sddpduusucreacion = dr.GetString(iSddpduusucreacion);

            int iSddpdufeccreacion = dr.GetOrdinal(this.Sddpdufeccreacion);
            if (!dr.IsDBNull(iSddpdufeccreacion)) entity.Sddpdufeccreacion = dr.GetDateTime(iSddpdufeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Sddpducodi = "SDDPDUCODI";
        public string Caiajcodi = "CAIAJCODI";
        public string Sddpduetapa = "SDDPDUETAPA";
        public string Sddpduserie = "SDDPDUSERIE";
        public string Sddpdubloque = "SDDPDUBLOQUE";
        public string Sddpduduracion = "SDDPDUDURACION";
        public string Sddpduusucreacion = "SDDPDUUSUCREACION";
        public string Sddpdufeccreacion = "SDDPDUFECCREACION";

        #endregion
        
        public string SqlListPorEtapa
        {
            get { return base.GetSqlXml("ListByEtapa"); }
        }
    }
}

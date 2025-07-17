using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCE_CFG_VALIDA_CONCEPTO
    /// </summary>
    public class VceCfgValidaConceptoHelper : HelperBase
    {
        public VceCfgValidaConceptoHelper()
            : base(Consultas.VceCfgValidaConceptoSql)
        {
        }

        public VceCfgValidaConceptoDTO Create(IDataReader dr)
        {
            VceCfgValidaConceptoDTO entity = new VceCfgValidaConceptoDTO();

            int iCrcvalcodi = dr.GetOrdinal(this.Crcvalcodi);
            if (!dr.IsDBNull(iCrcvalcodi)) entity.Crcvalcodi = Convert.ToInt32(dr.GetValue(iCrcvalcodi));

            int iCrcvaldescripcion = dr.GetOrdinal(this.Crcvaldescripcion);
            if (!dr.IsDBNull(iCrcvaldescripcion)) entity.Crcvaldescripcion = dr.GetString(iCrcvaldescripcion);

            int iConcepcodi = dr.GetOrdinal(this.Concepcodi);
            if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

            int iCrcvalcondicion = dr.GetOrdinal(this.Crcvalcondicion);
            if (!dr.IsDBNull(iCrcvalcondicion)) entity.Crcvalcondicion = dr.GetString(iCrcvalcondicion);

            int iCrcvalvalorref = dr.GetOrdinal(this.Crcvalvalorref);
            if (!dr.IsDBNull(iCrcvalvalorref)) entity.Crcvalvalorref = dr.GetDecimal(iCrcvalvalorref);

            int iCrgexusucreacion = dr.GetOrdinal(this.Crgexcusucreacion);
            if (!dr.IsDBNull(iCrgexusucreacion)) entity.Crgexcusucreacion = dr.GetString(iCrgexusucreacion);

            int iCrgexcfeccreacion = dr.GetOrdinal(this.Crgexcfeccreacion);
            if (!dr.IsDBNull(iCrgexcfeccreacion)) entity.Crgexcfeccreacion = dr.GetDateTime(iCrgexcfeccreacion);

            int iCrgexcusumodificacion = dr.GetOrdinal(this.Crgexcusumodificacion);
            if (!dr.IsDBNull(iCrgexcusumodificacion)) entity.Crgexcusumodificacion = dr.GetString(iCrgexcusumodificacion);

            int iCrgexcfecmodificacion = dr.GetOrdinal(this.Crgexcfecmodificacion);
            if (!dr.IsDBNull(iCrgexcfecmodificacion)) entity.Crgexcfecmodificacion = dr.GetDateTime(iCrgexcfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Crcvalcodi = "CRCVALCODI";
        public string Crcvaldescripcion = "CRCVALDESCRIPCION";
        public string Concepcodi = "CONCEPCODI";
        public string Crcvalcondicion = "CRCVALCONDICION";
        public string Crcvalvalorref = "CRCVALVALORREF";
        public string Crgexcusucreacion = "CRGEXCUSUCREACION";
        public string Crgexcfeccreacion = "CRGEXCFECCREACION";
        public string Crgexcusumodificacion = "CRGEXCUSUMODIFICACION";
        public string Crgexcfecmodificacion = "CRGEXCFECMODIFICACION";
      

        #endregion

        public string SqlListConceptos
        {
            get { return base.GetSqlXml("ListConceptos"); }
        }
    }
}

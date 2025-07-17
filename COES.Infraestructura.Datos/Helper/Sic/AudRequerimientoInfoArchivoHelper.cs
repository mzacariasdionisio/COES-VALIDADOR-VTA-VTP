using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AUD_REQUERIMIENTO_INFORM
    /// </summary>
    public class AudRequerimientoInfoArchivoHelper : HelperBase
    {
        public AudRequerimientoInfoArchivoHelper(): base(Consultas.AudRequerimientoInfoArchivoSql)
        {
        }


        public AudRequerimientoInfoArchivoDTO Create(IDataReader dr)
        {
            AudRequerimientoInfoArchivoDTO entity = new AudRequerimientoInfoArchivoDTO();

            int iReqicodiarch = dr.GetOrdinal(this.Reqicodiarch);
            if (!dr.IsDBNull(iReqicodiarch)) entity.Reqicodiarch = Convert.ToInt32(dr.GetValue(iReqicodiarch));

            int iReqicodi = dr.GetOrdinal(this.Reqicodi);
            if (!dr.IsDBNull(iReqicodi)) entity.Reqicodi = Convert.ToInt32(dr.GetValue(iReqicodi));

            int iArchcodi = dr.GetOrdinal(this.Archcodi);
            if (!dr.IsDBNull(iArchcodi)) entity.Archcodi = Convert.ToInt32(dr.GetValue(iArchcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Reqicodiarch = "Reqicodiarch";
        public string Reqicodi = "Reqicodi";
        public string Archcodi = "Archcodi";
        
        #endregion
    }
}

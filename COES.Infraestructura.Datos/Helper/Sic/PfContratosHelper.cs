using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PF_CONTRATOS
    /// </summary>
    public class PfContratosHelper : HelperBase
    {
        public PfContratosHelper(): base(Consultas.PfContratosSql)
        {
        }

        public PfContratosDTO Create(IDataReader dr)
        {
            PfContratosDTO entity = new PfContratosDTO();

            int iPfcontcodi = dr.GetOrdinal(this.Pfcontcodi);
            if (!dr.IsDBNull(iPfcontcodi)) entity.Pfcontcodi = Convert.ToInt32(dr.GetValue(iPfcontcodi));

            int iPfcontcantidad = dr.GetOrdinal(this.Pfcontcantidad);
            if (!dr.IsDBNull(iPfcontcantidad)) entity.Pfcontcantidad = dr.GetDecimal(iPfcontcantidad);

            int iPfcontvigenciaini = dr.GetOrdinal(this.Pfcontvigenciaini);
            if (!dr.IsDBNull(iPfcontvigenciaini)) entity.Pfcontvigenciaini = dr.GetDateTime(iPfcontvigenciaini);

            int iPfcontvigenciafin = dr.GetOrdinal(this.Pfcontvigenciafin);
            if (!dr.IsDBNull(iPfcontvigenciafin)) entity.Pfcontvigenciafin = dr.GetDateTime(iPfcontvigenciafin);

            int iPfcontobservacion = dr.GetOrdinal(this.Pfcontobservacion);
            if (!dr.IsDBNull(iPfcontobservacion)) entity.Pfcontobservacion = dr.GetString(iPfcontobservacion);

            int iPfcontcedente = dr.GetOrdinal(this.Pfcontcedente);
            if (!dr.IsDBNull(iPfcontcedente)) entity.Pfcontcedente = Convert.ToInt32(dr.GetValue(iPfcontcedente));

            int iPfcontcesionario = dr.GetOrdinal(this.Pfcontcesionario);
            if (!dr.IsDBNull(iPfcontcesionario)) entity.Pfcontcesionario = Convert.ToInt32(dr.GetValue(iPfcontcesionario));

            int iPfpericodi = dr.GetOrdinal(this.Pfpericodi);
            if (!dr.IsDBNull(iPfpericodi)) entity.Pfpericodi = Convert.ToInt32(dr.GetValue(iPfpericodi));

            int iPfverscodi = dr.GetOrdinal(this.Pfverscodi);
            if (!dr.IsDBNull(iPfverscodi)) entity.Pfverscodi = Convert.ToInt32(dr.GetValue(iPfverscodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Pfcontcodi = "PFCONTCODI";
        public string Pfcontcantidad = "PFCONTCANTIDAD";
        public string Pfcontvigenciaini = "PFCONTVIGENCIAINI";
        public string Pfcontvigenciafin = "PFCONTVIGENCIAFIN";
        public string Pfcontobservacion = "PFCONTOBSERVACION";
        public string Pfcontcedente = "PFCONTCEDENTE";
        public string Pfcontcesionario = "PFCONTCESIONARIO";
        public string Pfpericodi = "PFPERICODI";
        public string Pfverscodi = "PFVERSCODI";

        //campos adicionales
        public string Cedente = "CEDENTE";
        public string Cesionario = "CESIONARIO";
        #endregion

        #region Mapeo de Consultas
        public string SqlListarContratosCVFiltro
        {
            get { return base.GetSqlXml("ListarContratosCVFiltro"); }
        }
        #endregion
    }
}

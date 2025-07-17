using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_PROCESO_CONTACTO
    /// </summary>
    public class WbProcesoContactoHelper : HelperBase
    {
        public WbProcesoContactoHelper() : base(Consultas.WbProcesoContactoSql)
        {
        }

        public WbProcesoContactoDTO Create(IDataReader dr)
        {
            WbProcesoContactoDTO entity = new WbProcesoContactoDTO();

            int iContaccodi = dr.GetOrdinal(this.Contaccodi);
            if (!dr.IsDBNull(iContaccodi)) entity.Contaccodi = Convert.ToInt32(dr.GetValue(iContaccodi));

            int iProcesocodi = dr.GetOrdinal(this.Procesocodi);
            if (!dr.IsDBNull(iProcesocodi)) entity.Procesocodi = Convert.ToInt32(dr.GetValue(iProcesocodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Contaccodi = "CONTACCODI";
        public string Procesocodi = "PROCESOCODI";
        public string Descomite = "DESCOMITE";
        public string Indicador = "INDICADOR";

        #endregion
    }
}

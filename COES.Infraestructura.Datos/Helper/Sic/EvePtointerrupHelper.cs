using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_PTOINTERRUP
    /// </summary>
    public class EvePtointerrupHelper : HelperBase
    {
        public EvePtointerrupHelper(): base(Consultas.EvePtointerrupSql)
        {
        }

        public EvePtointerrupDTO Create(IDataReader dr)
        {
            EvePtointerrupDTO entity = new EvePtointerrupDTO();

            int iPtointerrcodi = dr.GetOrdinal(this.Ptointerrcodi);
            if (!dr.IsDBNull(iPtointerrcodi)) entity.Ptointerrcodi = Convert.ToInt32(dr.GetValue(iPtointerrcodi));

            int iPtoentregacodi = dr.GetOrdinal(this.Ptoentregacodi);
            if (!dr.IsDBNull(iPtoentregacodi)) entity.Ptoentregacodi = Convert.ToInt32(dr.GetValue(iPtoentregacodi));

            int iPtointerrupnomb = dr.GetOrdinal(this.Ptointerrupnomb);
            if (!dr.IsDBNull(iPtointerrupnomb)) entity.Ptointerrupnomb = dr.GetString(iPtointerrupnomb);

            int iPtointerrupsectip = dr.GetOrdinal(this.Ptointerrupsectip);
            if (!dr.IsDBNull(iPtointerrupsectip)) entity.Ptointerrupsectip = Convert.ToInt32(dr.GetValue(iPtointerrupsectip));

            return entity;
        }


        #region Mapeo de Campos

        public string Ptointerrcodi = "PTOINTERRCODI";
        public string Ptoentregacodi = "PTOENTREGACODI";
        public string Ptointerrupnomb = "PTOINTERRUPNOMB";
        public string Ptointerrupsectip = "PTOINTERRUPSECTIP";
        public string Ptoentrenomb = "PTOENTRENOMB";
        public string Clientecodi = "CLIENTECODI";
        public string Equicodi = "EQUICODI";
        public string Emprnomb = "EMPRNOMB";

        #endregion
    }
}

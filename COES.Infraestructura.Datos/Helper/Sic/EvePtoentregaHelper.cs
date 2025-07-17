using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_PTOENTREGA
    /// </summary>
    public class EvePtoentregaHelper : HelperBase
    {
        public EvePtoentregaHelper(): base(Consultas.EvePtoentregaSql)
        {
        }

        public EvePtoentregaDTO Create(IDataReader dr)
        {
            EvePtoentregaDTO entity = new EvePtoentregaDTO();

            int iClientecodi = dr.GetOrdinal(this.Clientecodi);
            if (!dr.IsDBNull(iClientecodi)) entity.Clientecodi = Convert.ToInt32(dr.GetValue(iClientecodi));

            int iPtoentregacodi = dr.GetOrdinal(this.Ptoentregacodi);
            if (!dr.IsDBNull(iPtoentregacodi)) entity.Ptoentregacodi = Convert.ToInt32(dr.GetValue(iPtoentregacodi));

            int iPtoentrenomb = dr.GetOrdinal(this.Ptoentrenomb);
            if (!dr.IsDBNull(iPtoentrenomb)) entity.Ptoentrenomb = dr.GetString(iPtoentrenomb);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Clientecodi = "CLIENTECODI";
        public string Ptoentregacodi = "PTOENTREGACODI";
        public string Ptoentrenomb = "PTOENTRENOMB";
        public string Equicodi = "EQUICODI";

        #endregion
    }
}


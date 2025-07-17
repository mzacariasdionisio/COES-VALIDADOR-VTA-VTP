using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MP_TIPORELACION
    /// </summary>
    public class MpTiporelacionHelper : HelperBase
    {
        public MpTiporelacionHelper(): base(Consultas.MpTiporelacionSql)
        {
        }

        public MpTiporelacionDTO Create(IDataReader dr)
        {
            MpTiporelacionDTO entity = new MpTiporelacionDTO();

            int iMtrelcodi = dr.GetOrdinal(this.Mtrelcodi);
            if (!dr.IsDBNull(iMtrelcodi)) entity.Mtrelcodi = Convert.ToInt32(dr.GetValue(iMtrelcodi));

            int iMtrelnomb = dr.GetOrdinal(this.Mtrelnomb);
            if (!dr.IsDBNull(iMtrelnomb)) entity.Mtrelnomb = dr.GetString(iMtrelnomb);

            return entity;
        }


        #region Mapeo de Campos

        public string Mtrelcodi = "MTRELCODI";
        public string Mtrelnomb = "MTRELNOMB";

        #endregion
    }
}

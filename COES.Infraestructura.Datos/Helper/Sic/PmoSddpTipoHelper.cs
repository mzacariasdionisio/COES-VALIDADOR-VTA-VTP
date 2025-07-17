using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PMO_SDDP_TIPO
    /// </summary>
    public class PmoSddpTipoHelper : HelperBase
    {
        public PmoSddpTipoHelper() : base(Consultas.PmoSddpTipoSql)
        {
        }

        public PmoSddpTipoDTO Create(IDataReader dr)
        {
            PmoSddpTipoDTO entity = new PmoSddpTipoDTO();

            int iTsddpcodi = dr.GetOrdinal(this.Tsddpcodi);
            if (!dr.IsDBNull(iTsddpcodi)) entity.Tsddpcodi = Convert.ToInt32(dr.GetValue(iTsddpcodi));

            int iTsddpnomb = dr.GetOrdinal(this.Tsddpnomb);
            if (!dr.IsDBNull(iTsddpnomb)) entity.Tsddpnomb = dr.GetString(iTsddpnomb);

            int iTsddpabrev = dr.GetOrdinal(this.Tsddpabrev);
            if (!dr.IsDBNull(iTsddpabrev)) entity.Tsddpabrev = dr.GetString(iTsddpabrev);

            return entity;
        }


        #region Mapeo de Campos

        public string Tsddpcodi = "TSDDPCODI";
        public string Tsddpnomb = "TSDDPNOMB";
        public string Tsddpabrev = "TSDDPABREV";

        #endregion
    }
}

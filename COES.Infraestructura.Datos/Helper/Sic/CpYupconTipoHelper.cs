using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_YUPCON_TIPO
    /// </summary>
    public class CpYupconTipoHelper : HelperBase
    {
        public CpYupconTipoHelper() : base(Consultas.CpYupconTipoSql)
        {
        }

        public CpYupconTipoDTO Create(IDataReader dr)
        {
            CpYupconTipoDTO entity = new CpYupconTipoDTO();

            int iTyupcodi = dr.GetOrdinal(this.Tyupcodi);
            if (!dr.IsDBNull(iTyupcodi)) entity.Tyupcodi = Convert.ToInt32(dr.GetValue(iTyupcodi));

            int iTyupnombre = dr.GetOrdinal(this.Tyupnombre);
            if (!dr.IsDBNull(iTyupnombre)) entity.Tyupnombre = dr.GetString(iTyupnombre);

            return entity;
        }


        #region Mapeo de Campos

        public string Tyupcodi = "TYUPCODI";
        public string Tyupnombre = "TYUPNOMBRE";

        #endregion
    }
}

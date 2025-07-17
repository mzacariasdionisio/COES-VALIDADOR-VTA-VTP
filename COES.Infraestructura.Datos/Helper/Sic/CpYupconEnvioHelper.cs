using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_YUPCON_ENVIO
    /// </summary>
    public class CpYupconEnvioHelper : HelperBase
    {
        public CpYupconEnvioHelper() : base(Consultas.CpYupconEnvioSql)
        {
        }

        public CpYupconEnvioDTO Create(IDataReader dr)
        {
            CpYupconEnvioDTO entity = new CpYupconEnvioDTO();

            int iCyupcodi = dr.GetOrdinal(this.Cyupcodi);
            if (!dr.IsDBNull(iCyupcodi)) entity.Cyupcodi = Convert.ToInt32(dr.GetValue(iCyupcodi));

            int iCyupfecha = dr.GetOrdinal(this.Cyupfecha);
            if (!dr.IsDBNull(iCyupfecha)) entity.Cyupfecha = dr.GetDateTime(iCyupfecha);

            int iCyupbloquehorario = dr.GetOrdinal(this.Cyupbloquehorario);
            if (!dr.IsDBNull(iCyupbloquehorario)) entity.Cyupbloquehorario = Convert.ToInt32(dr.GetValue(iCyupbloquehorario));

            int iCyupusuregistro = dr.GetOrdinal(this.Cyupusuregistro);
            if (!dr.IsDBNull(iCyupusuregistro)) entity.Cyupusuregistro = dr.GetString(iCyupusuregistro);

            int iCyupfecregistro = dr.GetOrdinal(this.Cyupfecregistro);
            if (!dr.IsDBNull(iCyupfecregistro)) entity.Cyupfecregistro = dr.GetDateTime(iCyupfecregistro);

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            int iTyupcodi = dr.GetOrdinal(this.Tyupcodi);
            if (!dr.IsDBNull(iTyupcodi)) entity.Tyupcodi = Convert.ToInt32(dr.GetValue(iTyupcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Cyupcodi = "CYUPCODI";
        public string Cyupfecha = "CYUPFECHA";
        public string Cyupbloquehorario = "CYUPBLOQUEHORARIO";
        public string Cyupusuregistro = "CYUPUSUREGISTRO";
        public string Cyupfecregistro = "CYUPFECREGISTRO";
        public string Topcodi = "TOPCODI";
        public string Tyupcodi = "TYUPCODI";

        #endregion
    }
}

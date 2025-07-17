using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_YUPCON_CFG
    /// </summary>
    public class CpYupconCfgHelper : HelperBase
    {
        public CpYupconCfgHelper() : base(Consultas.CpYupconCfgSql)
        {
        }

        public CpYupconCfgDTO Create(IDataReader dr)
        {
            CpYupconCfgDTO entity = new CpYupconCfgDTO();

            int iYupcfgtipo = dr.GetOrdinal(this.Yupcfgtipo);
            if (!dr.IsDBNull(iYupcfgtipo)) entity.Yupcfgtipo = dr.GetString(iYupcfgtipo);

            int iYupcfgfecha = dr.GetOrdinal(this.Yupcfgfecha);
            if (!dr.IsDBNull(iYupcfgfecha)) entity.Yupcfgfecha = dr.GetDateTime(iYupcfgfecha);

            int iYupcfgbloquehorario = dr.GetOrdinal(this.Yupcfgbloquehorario);
            if (!dr.IsDBNull(iYupcfgbloquehorario)) entity.Yupcfgbloquehorario = Convert.ToInt32(dr.GetValue(iYupcfgbloquehorario));

            int iYupcfgcodi = dr.GetOrdinal(this.Yupcfgcodi);
            if (!dr.IsDBNull(iYupcfgcodi)) entity.Yupcfgcodi = Convert.ToInt32(dr.GetValue(iYupcfgcodi));

            int iTyupcodi = dr.GetOrdinal(this.Tyupcodi);
            if (!dr.IsDBNull(iTyupcodi)) entity.Tyupcodi = Convert.ToInt32(dr.GetValue(iTyupcodi));

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            int iYupcfgusuregistro = dr.GetOrdinal(this.Yupcfgusuregistro);
            if (!dr.IsDBNull(iYupcfgusuregistro)) entity.Yupcfgusuregistro = dr.GetString(iYupcfgusuregistro);

            int iYupcfgfecregistro = dr.GetOrdinal(this.Yupcfgfecregistro);
            if (!dr.IsDBNull(iYupcfgfecregistro)) entity.Yupcfgfecregistro = dr.GetDateTime(iYupcfgfecregistro);

            return entity;
        }


        #region Mapeo de Campos

        public string Yupcfgtipo = "YUPCFGTIPO";
        public string Yupcfgfecha = "YUPCFGFECHA";
        public string Yupcfgbloquehorario = "YUPCFGBLOQUEHORARIO";
        public string Yupcfgcodi = "YUPCFGCODI";
        public string Tyupcodi = "TYUPCODI";
        public string Topcodi = "TOPCODI";
        public string Yupcfgusuregistro = "YUPCFGUSUREGISTRO";
        public string Yupcfgfecregistro = "YUPCFGFECREGISTRO";

        #endregion
    }
}

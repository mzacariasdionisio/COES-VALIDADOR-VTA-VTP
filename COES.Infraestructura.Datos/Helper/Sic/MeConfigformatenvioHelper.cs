using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_CONFIGFORMATENVIO
    /// </summary>
    public class MeConfigformatenvioHelper : HelperBase
    {
        public MeConfigformatenvioHelper()
            : base(Consultas.MeConfigformatenvioSql)
        {
        }

        public MeConfigformatenvioDTO Create(IDataReader dr)
        {
            MeConfigformatenvioDTO entity = new MeConfigformatenvioDTO();

            int iCfgenvcodi = dr.GetOrdinal(this.Cfgenvcodi);
            if (!dr.IsDBNull(iCfgenvcodi)) entity.Cfgenvcodi = Convert.ToInt32(dr.GetValue(iCfgenvcodi));

            int iCfgenvptos = dr.GetOrdinal(this.Cfgenvptos);
            if (!dr.IsDBNull(iCfgenvptos)) entity.Cfgenvptos = dr.GetString(iCfgenvptos);

            int iCfgenvorden = dr.GetOrdinal(this.Cfgenvorden);
            if (!dr.IsDBNull(iCfgenvorden)) entity.Cfgenvorden = dr.GetString(iCfgenvorden);

            int iCfgenvfecha = dr.GetOrdinal(this.Cfgenvfecha);
            if (!dr.IsDBNull(iCfgenvfecha)) entity.Cfgenvfecha = dr.GetDateTime(iCfgenvfecha);

            int iFormatcodi = dr.GetOrdinal(this.Formatcodi);
            if (!dr.IsDBNull(iFormatcodi)) entity.Formatcodi = Convert.ToInt32(dr.GetValue(iFormatcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iCfgenvtipoinf = dr.GetOrdinal(this.Cfgenvtipoinf);
            if (!dr.IsDBNull(iCfgenvtipoinf)) entity.Cfgenvtipoinf = dr.GetString(iCfgenvtipoinf);
            entity.Cfgenvtipoinf = !string.IsNullOrEmpty(entity.Cfgenvtipoinf) ? entity.Cfgenvtipoinf.Trim() : string.Empty;

            int iCfgenvhojas = dr.GetOrdinal(this.Cfgenvhojas);
            if (!dr.IsDBNull(iCfgenvhojas)) entity.Cfgenvhojas = dr.GetString(iCfgenvhojas);
            entity.Cfgenvhojas = !string.IsNullOrEmpty(entity.Cfgenvhojas) ? entity.Cfgenvhojas.Trim() : string.Empty;

            int iCfgenvtipopto = dr.GetOrdinal(this.Cfgenvtipopto);
            if (!dr.IsDBNull(iCfgenvtipopto)) entity.Cfgenvtipopto = dr.GetString(iCfgenvtipopto);
            entity.Cfgenvtipopto = !string.IsNullOrEmpty(entity.Cfgenvtipopto) ? entity.Cfgenvtipopto.Trim() : string.Empty;

            return entity;
        }


        #region Mapeo de Campos

        public string Cfgenvcodi = "CFGENVCODI";
        public string Cfgenvptos = "CFGENVPTOS";
        public string Cfgenvorden = "CFGENVORDEN";
        public string Cfgenvfecha = "CFGENVFECHA";
        public string Formatcodi = "FORMATCODI";
        public string Emprcodi = "EMPRCODI";
        public string Cfgenvtipoinf = "CFGENVTIPOINF";
        public string Cfgenvhojas = "CFGENVHOJAS";
        public string Cfgenvtipopto = "CFGENVTIPOPTO";

        #endregion
    }
}

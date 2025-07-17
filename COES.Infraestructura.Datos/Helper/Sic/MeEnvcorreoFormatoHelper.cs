using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_ENVCORREO_FORMATO
    /// </summary>
    public class MeEnvcorreoFormatoHelper : HelperBase
    {
        public MeEnvcorreoFormatoHelper(): base(Consultas.MeEnvcorreoFormatoSql)
        {
        }

        public MeEnvcorreoFormatoDTO Create(IDataReader dr)
        {
            MeEnvcorreoFormatoDTO entity = new MeEnvcorreoFormatoDTO();

            int iEcformcodi = dr.GetOrdinal(this.Ecformcodi);
            if (!dr.IsDBNull(iEcformcodi)) entity.Ecformcodi = Convert.ToInt32(dr.GetValue(iEcformcodi));

            int iFormatcodi = dr.GetOrdinal(this.Formatcodi);
            if (!dr.IsDBNull(iFormatcodi)) entity.Formatcodi = Convert.ToInt32(dr.GetValue(iFormatcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEcformhabilitado = dr.GetOrdinal(this.Ecformhabilitado);
            if (!dr.IsDBNull(iEcformhabilitado)) entity.Ecformhabilitado = dr.GetString(iEcformhabilitado);

            int iEcformusucreacion = dr.GetOrdinal(this.Ecformusucreacion);
            if (!dr.IsDBNull(iEcformusucreacion)) entity.Ecformusucreacion = dr.GetString(iEcformusucreacion);

            int iEcformfeccreacion = dr.GetOrdinal(this.Ecformfeccreacion);
            if (!dr.IsDBNull(iEcformfeccreacion)) entity.Ecformfeccreacion = dr.GetDateTime(iEcformfeccreacion);

            int iEcformusumodificacion = dr.GetOrdinal(this.Ecformusumodificacion);
            if (!dr.IsDBNull(iEcformusumodificacion)) entity.Ecformusumodificacion = dr.GetString(iEcformusumodificacion);

            int iEcformfecmodificacion = dr.GetOrdinal(this.Ecformfecmodificacion);
            if (!dr.IsDBNull(iEcformfecmodificacion)) entity.Ecformfecmodificacion = dr.GetDateTime(iEcformfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Ecformcodi = "ECFORMCODI";
        public string Formatcodi = "FORMATCODI";
        public string Emprcodi = "EMPRCODI";
        public string Ecformhabilitado = "ECFORMHABILITADO";
        public string Ecformusucreacion = "ECFORMUSUCREACION";
        public string Ecformfeccreacion = "ECFORMFECCREACION";
        public string Ecformusumodificacion = "ECFORMUSUMODIFICACION";
        public string Ecformfecmodificacion = "ECFORMFECMODIFICACION";
        public string Modcodi = "MODCODI";
        public string Useremail = "USEREMAIL";


        #endregion

        public string SqlObtenerEmpresas
        {
            get { return base.GetSqlXml("ObtenerEmpresas"); }
        }
    }
}

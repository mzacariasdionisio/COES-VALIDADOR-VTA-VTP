using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RPF_ENVIO
    /// </summary>
    public class RpfEnvioHelper : HelperBase
    {
        public RpfEnvioHelper(): base(Consultas.RpfEnvioSql)
        {
        }

        public RpfEnvioDTO Create(IDataReader dr)
        {
            RpfEnvioDTO entity = new RpfEnvioDTO();

            int iRpfenvcodi = dr.GetOrdinal(this.Rpfenvcodi);
            if (!dr.IsDBNull(iRpfenvcodi)) entity.Rpfenvcodi = Convert.ToInt32(dr.GetValue(iRpfenvcodi));

            int iRpfenvfecha = dr.GetOrdinal(this.Rpfenvfecha);
            if (!dr.IsDBNull(iRpfenvfecha)) entity.Rpfenvfecha = dr.GetDateTime(iRpfenvfecha);

            int iRpfenvestado = dr.GetOrdinal(this.Rpfenvestado);
            if (!dr.IsDBNull(iRpfenvestado)) entity.Rpfenvestado = dr.GetString(iRpfenvestado);

            int iRpfenvusucreacion = dr.GetOrdinal(this.Rpfenvusucreacion);
            if (!dr.IsDBNull(iRpfenvusucreacion)) entity.Rpfenvusucreacion = dr.GetString(iRpfenvusucreacion);

            int iRpfenvfeccreacion = dr.GetOrdinal(this.Rpfenvfeccreacion);
            if (!dr.IsDBNull(iRpfenvfeccreacion)) entity.Rpfenvfeccreacion = dr.GetDateTime(iRpfenvfeccreacion);

            int iRpfenvusumodificacion = dr.GetOrdinal(this.Rpfenvusumodificacion);
            if (!dr.IsDBNull(iRpfenvusumodificacion)) entity.Rpfenvusumodificacion = dr.GetString(iRpfenvusumodificacion);

            int iRpfenvfecmodificacion = dr.GetOrdinal(this.Rpfenvfecmodificacion);
            if (!dr.IsDBNull(iRpfenvfecmodificacion)) entity.Rpfenvfecmodificacion = dr.GetDateTime(iRpfenvfecmodificacion);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Rpfenvcodi = "RPFENVCODI";
        public string Rpfenvfecha = "RPFENVFECHA";
        public string Rpfenvestado = "RPFENVESTADO";
        public string Rpfenvusucreacion = "RPFENVUSUCREACION";
        public string Rpfenvfeccreacion = "RPFENVFECCREACION";
        public string Rpfenvusumodificacion = "RPFENVUSUMODIFICACION";
        public string Rpfenvfecmodificacion = "RPFENVFECMODIFICACION";
        public string Emprcodi = "EMPRCODI";

        public string SqlObtenerPorFecha
        {
            get { return base.GetSqlXml("ObtenerPorFecha"); }
        }

        public string SqlObtenerEnviosPorFecha
        {
            get { return base.GetSqlXml("ObtenerEnviosPorFecha"); }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla NR_PERIODO
    /// </summary>
    public class NrPeriodoHelper : HelperBase
    {
        public NrPeriodoHelper(): base(Consultas.NrPeriodoSql)
        {
        }

        public NrPeriodoDTO Create(IDataReader dr)
        {
            NrPeriodoDTO entity = new NrPeriodoDTO();

            int iNrpercodi = dr.GetOrdinal(this.Nrpercodi);
            if (!dr.IsDBNull(iNrpercodi)) entity.Nrpercodi = Convert.ToInt32(dr.GetValue(iNrpercodi));

            int iNrpermes = dr.GetOrdinal(this.Nrpermes);
            if (!dr.IsDBNull(iNrpermes)) entity.Nrpermes = dr.GetDateTime(iNrpermes);

            int iNrpereliminado = dr.GetOrdinal(this.Nrpereliminado);
            if (!dr.IsDBNull(iNrpereliminado)) entity.Nrpereliminado = dr.GetString(iNrpereliminado);

            int iNrperusucreacion = dr.GetOrdinal(this.Nrperusucreacion);
            if (!dr.IsDBNull(iNrperusucreacion)) entity.Nrperusucreacion = dr.GetString(iNrperusucreacion);

            int iNrperfeccreacion = dr.GetOrdinal(this.Nrperfeccreacion);
            if (!dr.IsDBNull(iNrperfeccreacion)) entity.Nrperfeccreacion = dr.GetDateTime(iNrperfeccreacion);

            int iNrperusumodificacion = dr.GetOrdinal(this.Nrperusumodificacion);
            if (!dr.IsDBNull(iNrperusumodificacion)) entity.Nrperusumodificacion = dr.GetString(iNrperusumodificacion);

            int iNrperfecmodificacion = dr.GetOrdinal(this.Nrperfecmodificacion);
            if (!dr.IsDBNull(iNrperfecmodificacion)) entity.Nrperfecmodificacion = dr.GetDateTime(iNrperfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Nrpercodi = "NRPERCODI";
        public string Nrpermes = "NRPERMES";
        public string Nrpereliminado = "NRPERELIMINADO";
        public string Nrperusucreacion = "NRPERUSUCREACION";
        public string Nrperfeccreacion = "NRPERFECCREACION";
        public string Nrperusumodificacion = "NRPERUSUMODIFICACION";
        public string Nrperfecmodificacion = "NRPERFECMODIFICACION";

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        #endregion
    }
}

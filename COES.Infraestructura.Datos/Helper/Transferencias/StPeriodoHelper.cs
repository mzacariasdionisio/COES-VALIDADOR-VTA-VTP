using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ST_PERIODO
    /// </summary>
    public class StPeriodoHelper : HelperBase
    {
        public StPeriodoHelper()
            : base(Consultas.StPeriodoSql)
        {
        }

        public StPeriodoDTO Create(IDataReader dr)
        {
            StPeriodoDTO entity = new StPeriodoDTO();

            int iStpercodi = dr.GetOrdinal(this.Stpercodi);
            if (!dr.IsDBNull(iStpercodi)) entity.Stpercodi = Convert.ToInt32(dr.GetValue(iStpercodi));

            int iStperanio = dr.GetOrdinal(this.Stperanio);
            if (!dr.IsDBNull(iStperanio)) entity.Stperanio = Convert.ToInt32(dr.GetValue(iStperanio));

            int iStpermes = dr.GetOrdinal(this.Stpermes);
            if (!dr.IsDBNull(iStpermes)) entity.Stpermes = Convert.ToInt32(dr.GetValue(iStpermes));

            int iStperaniomes = dr.GetOrdinal(this.Stperaniomes);
            if (!dr.IsDBNull(iStperaniomes)) entity.Stperaniomes = Convert.ToInt32(dr.GetValue(iStperaniomes));

            int iStpernombre = dr.GetOrdinal(this.Stpernombre);
            if (!dr.IsDBNull(iStpernombre)) entity.Stpernombre = dr.GetString(iStpernombre);

            int iStperusucreacion = dr.GetOrdinal(this.Stperusucreacion);
            if (!dr.IsDBNull(iStperusucreacion)) entity.Stperusucreacion = dr.GetString(iStperusucreacion);

            int iStperfeccreacion = dr.GetOrdinal(this.Stperfeccreacion);
            if (!dr.IsDBNull(iStperfeccreacion)) entity.Stperfeccreacion = dr.GetDateTime(iStperfeccreacion);

            int iStperusumodificacion = dr.GetOrdinal(this.Stperusumodificacion);
            if (!dr.IsDBNull(iStperusumodificacion)) entity.Stperusumodificacion = dr.GetString(iStperusumodificacion);

            int iStperfecmodificacion = dr.GetOrdinal(this.Stperfecmodificacion);
            if (!dr.IsDBNull(iStperfecmodificacion)) entity.Stperfecmodificacion = dr.GetDateTime(iStperfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Stpercodi = "STPERCODI";
        public string Stperanio = "STPERANIO";
        public string Stpermes = "STPERMES";
        public string Stperaniomes = "STPERANIOMES";
        public string Stpernombre = "STPERNOMBRE";
        public string Stperusucreacion = "STPERUSUCREACION";
        public string Stperfeccreacion = "STPERFECCREACION";
        public string Stperusumodificacion = "STPERUSUMODIFICACION";
        public string Stperfecmodificacion = "STPERFECMODIFICACION";

        #endregion

        public string SqlGetByIdPeriodoAnterior
        {
            get { return base.GetSqlXml("GetByIdPeriodoAnterior"); }
        }
    }
}

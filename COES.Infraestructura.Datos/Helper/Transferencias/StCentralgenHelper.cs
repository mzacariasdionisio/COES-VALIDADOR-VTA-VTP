using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ST_CENTRALGEN
    /// </summary>
    public class StCentralgenHelper : HelperBase
    {
        public StCentralgenHelper(): base(Consultas.StCentralgenSql)
        {
        }

        public StCentralgenDTO Create(IDataReader dr)
        {
            StCentralgenDTO entity = new StCentralgenDTO();

            int iStcntgcodi = dr.GetOrdinal(this.Stcntgcodi);
            if (!dr.IsDBNull(iStcntgcodi)) entity.Stcntgcodi = Convert.ToInt32(dr.GetValue(iStcntgcodi));

            int iStgenrcodi = dr.GetOrdinal(this.Stgenrcodi);
            if (!dr.IsDBNull(iStgenrcodi)) entity.Stgenrcodi = Convert.ToInt32(dr.GetValue(iStgenrcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iStcntgusucreacion = dr.GetOrdinal(this.Stcntgusucreacion);
            if (!dr.IsDBNull(iStcntgusucreacion)) entity.Stcntgusucreacion = dr.GetString(iStcntgusucreacion);

            int iStcntgfeccreacion = dr.GetOrdinal(this.Stcntgfeccreacion);
            if (!dr.IsDBNull(iStcntgfeccreacion)) entity.Stcntgfeccreacion = dr.GetDateTime(iStcntgfeccreacion);

            int iStcntgusumodificacion = dr.GetOrdinal(this.Stcntgusumodificacion);
            if (!dr.IsDBNull(iStcntgusumodificacion)) entity.Stcntgusumodificacion = dr.GetString(iStcntgusumodificacion);

            int iStcntgfecmodificacion = dr.GetOrdinal(this.Stcntgfecmodificacion);
            if (!dr.IsDBNull(iStcntgfecmodificacion)) entity.Stcntgfecmodificacion = dr.GetDateTime(iStcntgfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Stcntgcodi = "STCNTGCODI";
        public string Stgenrcodi = "STGENRCODI";
        public string Equicodi = "EQUICODI";
        public string Barrcodi = "BARRCODI";
        public string Stcntgusucreacion = "STCNTGUSUCREACION";
        public string Stcntgfeccreacion = "STCNTGFECCREACION";
        public string Stcntgusumodificacion = "STCNTGUSUMODIFICACION";
        public string Stcntgfecmodificacion = "STCNTGFECMODIFICACION";
        //ATRIBUTOS PARA CONSULTA
        public string Strecacodi = "STRECACODI";
        public string Stcompcodi = "STCOMPCODI";
        public string Equinomb = "EQUINOMB";
        public string Barrnomb = "BARRNOMB";
        //PARA EL CRITERIO DE REPORTES
        public string Emprnomb = "EMPRNOMB";
        public string Degeledistancia = "DEGELEDISTANCIA";
        public string Stenrgrgia = "STENRGRGIA";
        public string Gwhz = "GWHZ";
        public string Stcompcodelemento = "STCOMPCODELEMENTO";
        #endregion

        public string GetByCentNombre
        {
            get { return base.GetSqlXml("GetByCentNomb"); }
        }

        public string GetByCriteriaReporte
        {
            get { return base.GetSqlXml("GetByCriteriaReporte"); }
        }

        public string SqlDeleteVersion
        {
            get { return base.GetSqlXml("DeleteVersion"); }
        }

        #region SIOSEIN2

        public string SqlReporteGeneradoresCompensacion
        {
            get { return base.GetSqlXml("ReporteGeneradoresCompensacion"); }
        }

        #endregion
    }
}

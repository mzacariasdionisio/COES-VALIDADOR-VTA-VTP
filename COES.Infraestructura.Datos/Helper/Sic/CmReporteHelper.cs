using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_REPORTE
    /// </summary>
    public class CmReporteHelper : HelperBase
    {
        public CmReporteHelper(): base(Consultas.CmReporteSql)
        {
        }

        public CmReporteDTO Create(IDataReader dr)
        {
            CmReporteDTO entity = new CmReporteDTO();

            int iCmrepcodi = dr.GetOrdinal(this.Cmrepcodi);
            if (!dr.IsDBNull(iCmrepcodi)) entity.Cmrepcodi = Convert.ToInt32(dr.GetValue(iCmrepcodi));

            int iCmpercodi = dr.GetOrdinal(this.Cmpercodi);
            if (!dr.IsDBNull(iCmpercodi)) entity.Cmpercodi = Convert.ToInt32(dr.GetValue(iCmpercodi));

            int iCmurcodi = dr.GetOrdinal(this.Cmurcodi);
            if (!dr.IsDBNull(iCmurcodi)) entity.Cmurcodi = Convert.ToInt32(dr.GetValue(iCmurcodi));

            int iCmrepversion = dr.GetOrdinal(this.Cmrepversion);
            if (!dr.IsDBNull(iCmrepversion)) entity.Cmrepversion = Convert.ToInt32(dr.GetValue(iCmrepversion));

            int iCmrepfecha = dr.GetOrdinal(this.Cmrepfecha);
            if (!dr.IsDBNull(iCmrepfecha)) entity.Cmrepfecha = dr.GetDateTime(iCmrepfecha);

            int iCmrepestado = dr.GetOrdinal(this.Cmrepestado);
            if (!dr.IsDBNull(iCmrepestado)) entity.Cmrepestado = dr.GetString(iCmrepestado);

            int iCmrepusucreacion = dr.GetOrdinal(this.Cmrepusucreacion);
            if (!dr.IsDBNull(iCmrepusucreacion)) entity.Cmrepusucreacion = dr.GetString(iCmrepusucreacion);

            int iCmrepfeccreacion = dr.GetOrdinal(this.Cmrepfeccreacion);
            if (!dr.IsDBNull(iCmrepfeccreacion)) entity.Cmrepfeccreacion = dr.GetDateTime(iCmrepfeccreacion);

            int iCmrepusumodificacion = dr.GetOrdinal(this.Cmrepusumodificacion);
            if (!dr.IsDBNull(iCmrepusumodificacion)) entity.Cmrepusumodificacion = dr.GetString(iCmrepusumodificacion);

            int iCmrepfecmodificacion = dr.GetOrdinal(this.Cmrepfecmodificacion);
            if (!dr.IsDBNull(iCmrepfecmodificacion)) entity.Cmrepfecmodificacion = dr.GetDateTime(iCmrepfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Cmrepcodi = "CMREPCODI";
        public string Cmpercodi = "CMPERCODI";
        public string Cmurcodi = "CMURCODI";
        public string Cmrepversion = "CMREPVERSION";
        public string Cmrepfecha = "CMREPFECHA";
        public string Cmrepestado = "CMREPESTADO";
        public string Cmrepusucreacion = "CMREPUSUCREACION";
        public string Cmrepfeccreacion = "CMREPFECCREACION";
        public string Cmrepusumodificacion = "CMREPUSUMODIFICACION";
        public string Cmrepfecmodificacion = "CMREPFECMODIFICACION";

        #endregion

        public string SqlObtenerNroVersion
        {
            get { return base.GetSqlXml("ObtenerNroVersion"); }
        }
    }
}

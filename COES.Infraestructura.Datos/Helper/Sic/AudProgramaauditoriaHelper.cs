using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AUD_PROGRAMAAUDITORIA
    /// </summary>
    public class AudProgramaauditoriaHelper : HelperBase
    {
        public AudProgramaauditoriaHelper() : base(Consultas.AudProgramaauditoriaSql)
        {
        }

        public string SqlGetByCriteriaEjecucion
        {
            get { return base.GetSqlXml("GetByCriteriaEjecucion"); }
        }
        
        public AudProgramaauditoriaDTO Create(IDataReader dr)
        {
            AudProgramaauditoriaDTO entity = new AudProgramaauditoriaDTO();

            int iProgacodi = dr.GetOrdinal(this.Progacodi);
            if (!dr.IsDBNull(iProgacodi)) entity.Progacodi = Convert.ToInt32(dr.GetValue(iProgacodi));

            int iAudicodi = dr.GetOrdinal(this.Audicodi);
            if (!dr.IsDBNull(iAudicodi)) entity.Audicodi = Convert.ToInt32(dr.GetValue(iAudicodi));

            int iTabcdcoditipoactividad = dr.GetOrdinal(this.Tabcdcoditipoactividad);
            if (!dr.IsDBNull(iTabcdcoditipoactividad)) entity.Tabcdcoditipoactividad = Convert.ToInt32(dr.GetValue(iTabcdcoditipoactividad));

            int iProgafecha = dr.GetOrdinal(this.Progafecha);
            if (!dr.IsDBNull(iProgafecha)) entity.Progafecha = dr.GetDateTime(iProgafecha);

            int iProgahorainicio = dr.GetOrdinal(this.Progahorainicio);
            if (!dr.IsDBNull(iProgahorainicio)) entity.Progahorainicio = dr.GetString(iProgahorainicio);

            int iProgahorafin = dr.GetOrdinal(this.Progahorafin);
            if (!dr.IsDBNull(iProgahorafin)) entity.Progahorafin = dr.GetString(iProgahorafin);

            int iProgaactivo = dr.GetOrdinal(this.Progaactivo);
            if (!dr.IsDBNull(iProgaactivo)) entity.Progaactivo = dr.GetString(iProgaactivo);

            int iProgahistorico = dr.GetOrdinal(this.Progahistorico);
            if (!dr.IsDBNull(iProgahistorico)) entity.Progahistorico = dr.GetString(iProgahistorico);

            int iProgausucreacion = dr.GetOrdinal(this.Progausucreacion);
            if (!dr.IsDBNull(iProgausucreacion)) entity.Progausucreacion = dr.GetString(iProgausucreacion);

            int iProgafeccreacion = dr.GetOrdinal(this.Progafeccreacion);
            if (!dr.IsDBNull(iProgafeccreacion)) entity.Progafeccreacion = dr.GetDateTime(iProgafeccreacion);

            int iProgausumodificacion = dr.GetOrdinal(this.Progausumodificacion);
            if (!dr.IsDBNull(iProgausumodificacion)) entity.Progausumodificacion = dr.GetString(iProgausumodificacion);

            int iProgafecmodificacion = dr.GetOrdinal(this.Progafecmodificacion);
            if (!dr.IsDBNull(iProgafecmodificacion)) entity.Progafecmodificacion = dr.GetDateTime(iProgafecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Progacodi = "PROGACODI";
        public string Audicodi = "AUDICODI";
        public string Tabcdcoditipoactividad = "TABCDCODITIPOACTIVIDAD";
        public string Tabcdcodiestadoactividad = "TABCDCODIESTADOACTIVIDAD";
        public string Progafecha = "PROGAFECHA";
        public string Progahorainicio = "PROGAHORAINICIO";
        public string Progahorafin = "PROGAHORAFIN";
        public string Progaactivo = "PROGAACTIVO";
        public string Progahistorico = "PROGAHISTORICO";
        public string Progausucreacion = "PROGAUSUCREACION";
        public string Progafeccreacion = "PROGAFECCREACION";
        public string Progausumodificacion = "PROGAUSUMODIFICACION";
        public string Progafecmodificacion = "PROGAFECMODIFICACION";

        public string Tipoactividad = "tipoactividad";
        public string Tipoelemento = "tipoelemento";
        public string Tipoelementocodi = "tipoelementocodi";

        public string Elemcodi = "Elemcodi";
        public string Elemdescripcion = "elemdescripcion";
        public string Elemcodigo = "Elemcodigo";
        public string Equipo = "equipo";
        public string Responsables = "responsables";

        public string Progaecodi = "Progaecodi";
        

        public string Areacodi = "areacodi";
        #endregion
    }
}

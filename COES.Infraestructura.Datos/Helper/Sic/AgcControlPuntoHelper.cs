using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AGC_CONTROL_PUNTO
    /// </summary>
    public class AgcControlPuntoHelper : HelperBase
    {
        public AgcControlPuntoHelper(): base(Consultas.AgcControlPuntoSql)
        {
        }

        public AgcControlPuntoDTO Create(IDataReader dr)
        {
            AgcControlPuntoDTO entity = new AgcControlPuntoDTO();

            int iAgccpcodi = dr.GetOrdinal(this.Agccpcodi);
            if (!dr.IsDBNull(iAgccpcodi)) entity.Agccpcodi = Convert.ToInt32(dr.GetValue(iAgccpcodi));

            int iAgcccodi = dr.GetOrdinal(this.Agcccodi);
            if (!dr.IsDBNull(iAgcccodi)) entity.Agcccodi = Convert.ToInt32(dr.GetValue(iAgcccodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iAgccpb2 = dr.GetOrdinal(this.Agccpb2);
            if (!dr.IsDBNull(iAgccpb2)) entity.Agccpb2 = dr.GetString(iAgccpb2);

            int iAgccpb3 = dr.GetOrdinal(this.Agccpb3);
            if (!dr.IsDBNull(iAgccpb3)) entity.Agccpb3 = dr.GetString(iAgccpb3);

            int iAgccpusucreacion = dr.GetOrdinal(this.Agccpusucreacion);
            if (!dr.IsDBNull(iAgccpusucreacion)) entity.Agccpusucreacion = dr.GetString(iAgccpusucreacion);

            int iAgccpfeccreacion = dr.GetOrdinal(this.Agccpfeccreacion);
            if (!dr.IsDBNull(iAgccpfeccreacion)) entity.Agccpfeccreacion = dr.GetDateTime(iAgccpfeccreacion);

            int iAgccpusumodificacion = dr.GetOrdinal(this.Agccpusumodificacion);
            if (!dr.IsDBNull(iAgccpusumodificacion)) entity.Agccpusumodificacion = dr.GetString(iAgccpusumodificacion);

            int iAgccpfecmodificacion = dr.GetOrdinal(this.Agccpfecmodificacion);
            if (!dr.IsDBNull(iAgccpfecmodificacion)) entity.Agccpfecmodificacion = dr.GetDateTime(iAgccpfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Agccpcodi = "AGCCPCODI";
        public string Agcccodi = "AGCCCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Equicodi = "EQUICODI";
        public string Agccpb2 = "AGCCPB2";
        public string Agccpb3 = "AGCCPB3";
        public string Agccpusucreacion = "AGCCPUSUCREACION";
        public string Agccpfeccreacion = "AGCCPFECCREACION";
        public string Agccpusumodificacion = "AGCCPUSUMODIFICACION";
        public string Agccpfecmodificacion = "AGCCPFECMODIFICACION";
        public string Agcctipo = "AGCCTIPO";
        public string Ptomedibarranomb = "PTOMEDIBARRANOMB";
        public string Equiabrev = "EQUIABREV";

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        public string SqlObtenerPorControl
        {
            get { return base.GetSqlXml("ObtenerPorControl"); }
        }
        

        #endregion
    }
}

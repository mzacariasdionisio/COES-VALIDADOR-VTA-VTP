using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AUD_PROCESO
    /// </summary>
    public class AudProcesoHelper : HelperBase
    {
        public AudProcesoHelper(): base(Consultas.AudProcesoSql)
        {
        }

        public string SqlListProcesoSuperior
        {
            get { return base.GetSqlXml("ListProcesoSuperior"); }
        }

        public string SqlObtenerNroRegistroBusqueda
        {
            get { return base.GetSqlXml("ObtenerNroRegistroBusqueda"); }
        }

        public string SqlGetByProcesoPorArea
        {
            get { return base.GetSqlXml("GetByProcesoPorArea"); }
        }

        public string SqlGetByProcesoValidacion
        {
            get { return base.GetSqlXml("GetByProcesoValidacion"); }
        }

        public string SqlGetByProcesoPorEstado
        {
            get { return base.GetSqlXml("GetByProcesoPorEstado"); }
        }

        public string GetByCodigo
        {
            get { return base.GetSqlXml("GetByCodigo"); }
        }

        public AudProcesoDTO Create(IDataReader dr, bool lista = false)
        {
            AudProcesoDTO entity = new AudProcesoDTO();

            int iProccodi = dr.GetOrdinal(this.Proccodi);
            if (!dr.IsDBNull(iProccodi)) entity.Proccodi = Convert.ToInt32(dr.GetValue(iProccodi));

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iProccodigo = dr.GetOrdinal(this.Proccodigo);
            if (!dr.IsDBNull(iProccodigo)) entity.Proccodigo = dr.GetString(iProccodigo);

            int iProcdescripcion = dr.GetOrdinal(this.Procdescripcion);
            if (!dr.IsDBNull(iProcdescripcion)) entity.Procdescripcion = dr.GetString(iProcdescripcion);

            int iProctienesuperior = dr.GetOrdinal(this.Proctienesuperior);
            if (!dr.IsDBNull(iProctienesuperior)) entity.Proctienesuperior = Convert.ToInt32(dr.GetValue(iProctienesuperior));

            int iProcprocesosuperior = dr.GetOrdinal(this.Procprocesosuperior);
            if (!dr.IsDBNull(iProcprocesosuperior)) entity.Procprocesosuperior = Convert.ToInt32(dr.GetValue(iProcprocesosuperior));

            int iProcactivo = dr.GetOrdinal(this.Procactivo);
            if (!dr.IsDBNull(iProcactivo)) entity.Procactivo = dr.GetString(iProcactivo);

            int iProchistorico = dr.GetOrdinal(this.Prochistorico);
            if (!dr.IsDBNull(iProchistorico)) entity.Prochistorico = dr.GetString(iProchistorico);

            int iProcusucreacion = dr.GetOrdinal(this.Procusucreacion);
            if (!dr.IsDBNull(iProcusucreacion)) entity.Procusucreacion = dr.GetString(iProcusucreacion);

            int iProcfeccreacion = dr.GetOrdinal(this.Procfeccreacion);
            if (!dr.IsDBNull(iProcfeccreacion)) entity.Procfeccreacion = dr.GetDateTime(iProcfeccreacion);

            int iProcusumodificacion = dr.GetOrdinal(this.Procusumodificacion);
            if (!dr.IsDBNull(iProcusumodificacion)) entity.Procusumodificacion = dr.GetString(iProcusumodificacion);

            int iProcfecmodificacion = dr.GetOrdinal(this.Procfecmodificacion);
            if (!dr.IsDBNull(iProcfecmodificacion)) entity.Procfecmodificacion = dr.GetDateTime(iProcfecmodificacion);

            int iProcsuperior = dr.GetOrdinal(this.Procsuperior);
            if (!dr.IsDBNull(iProcsuperior)) entity.Procsuperior = dr.GetString(iProcsuperior);

            return entity;
        }

        #region Mapeo de Campos

        public string Proccodi = "PROCCODI";
        public string Areacodi = "AREACODI";
        public string Proccodigo = "PROCCODIGO";
        public string Procdescripcion = "PROCDESCRIPCION";
        public string Proctienesuperior = "PROCTIENESUPERIOR";
        public string Procprocesosuperior = "PROCPROCESOSUPERIOR";
        public string Procactivo = "PROCACTIVO";
        public string Prochistorico = "PROCHISTORICO";
        public string Procusucreacion = "PROCUSUCREACION";
        public string Procfeccreacion = "PROCFECCREACION";
        public string Procusumodificacion = "PROCUSUMODIFICACION";
        public string Procfecmodificacion = "PROCFECMODIFICACION";
        public string Procsuperior = "PROCSUPERIOR";

        public string Areanomb = "AREANOMB";
        public string Procsuperiordescripcion = "PROCSUPERIORDESCRIPCION";

        public string Existerelacion = "Existerelacion";

        public string Validacionmensaje = "ValidacionMensaje";

        #endregion

        

    }
   
}

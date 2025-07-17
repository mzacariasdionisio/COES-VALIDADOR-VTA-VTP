using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Scada
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TR_OBSERVACION
    /// </summary>
    public class TrObservacionHelper : HelperBase
    {
        public TrObservacionHelper() : base(Consultas.TrObservacionSql)
        {
        }

        public TrObservacionDTO Create(IDataReader dr)
        {
            TrObservacionDTO entity = new TrObservacionDTO();

            int iObscancodi = dr.GetOrdinal(this.Obscancodi);
            if (!dr.IsDBNull(iObscancodi)) entity.Obscancodi = Convert.ToInt32(dr.GetValue(iObscancodi));

            int iObscanusucreacion = dr.GetOrdinal(this.Obscanusucreacion);
            if (!dr.IsDBNull(iObscanusucreacion)) entity.Obscanusucreacion = dr.GetString(iObscanusucreacion);

            int iObscanusumodificacion = dr.GetOrdinal(this.Obscanusumodificacion);
            if (!dr.IsDBNull(iObscanusumodificacion)) entity.Obscanusumodificacion = dr.GetString(iObscanusumodificacion);

            int iObscanfeccreacion = dr.GetOrdinal(this.Obscanfeccreacion);
            if (!dr.IsDBNull(iObscanfeccreacion)) entity.Obscanfeccreacion = dr.GetDateTime(iObscanfeccreacion);

            int iObscanfecmodificacion = dr.GetOrdinal(this.Obscanfecmodificacion);
            if (!dr.IsDBNull(iObscanfecmodificacion)) entity.Obscanfecmodificacion = dr.GetDateTime(iObscanfecmodificacion);

            int iObscanestado = dr.GetOrdinal(this.Obscanestado);
            if (!dr.IsDBNull(iObscanestado)) entity.Obscanestado = dr.GetString(iObscanestado);

            int iObscancomentario = dr.GetOrdinal(this.Obscancomentario);
            if (!dr.IsDBNull(iObscancomentario)) entity.Obscancomentario = dr.GetString(iObscancomentario);

            int iObscantipo = dr.GetOrdinal(this.Obscantipo);
            if (!dr.IsDBNull(iObscantipo)) entity.Obscantipo = dr.GetString(iObscantipo);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iObscancomentarioagente = dr.GetOrdinal(this.Obscancomentarioagente);
            if (!dr.IsDBNull(iObscancomentarioagente)) entity.Obscancomentarioagente = dr.GetString(iObscancomentarioagente);

            return entity;
        }


        #region Mapeo de Campos

        public string Obscancodi = "OBSCANCODI";
        public string Obscanusucreacion = "OBSCANUSUCREACION";
        public string Obscanusumodificacion = "OBSCANUSUMODIFICACION";
        public string Obscanfeccreacion = "OBSCANFECCREACION";
        public string Obscanfecmodificacion = "OBSCANFECMODIFICACION";
        public string Obscanestado = "OBSCANESTADO";
        public string Obscancomentario = "OBSCANCOMENTARIO";
        public string Obscantipo = "OBSCANTIPO";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRENOMB";
        public string Zonacodi = "ZONACODI";
        public string Zonanomb = "ZONANOMB";
        public string Canalcodi = "CANALCODI";
        public string Canalnomb = "CANALNOMB";
        public string Canaliccp = "CANALICCP";
        public string Canalunidad = "CANALUNIDAD";
        public string Canalabrev = "CANALABREV";
        public string Canalpointtype = "CANALPOINTTYPE";
        public string Emprcodisic = "EMPRCODISIC";
        public string Obscancomentarioagente = "OBSCANCOMENTARIOAGENTE";

        #endregion

        public string SqlObtenerEmpresasScada
        {
            get { return base.GetSqlXml("ObtenerEmpresasScada"); }
        }

        public string SqlObtenerZonasPorEmpresa
        {
            get { return base.GetSqlXml("ObtenerZonasPorEmpresa"); }
        }

        public string SqlObtenerCanales
        {
            get { return base.GetSqlXml("ObtenerCanales"); }
        }

        public string SqlObtenerNroFilaBusquedaCanal
        {
            get { return base.GetSqlXml("ObtenerNroFilaBusquedaCanal"); }
        }

        public string SqlObtenerCanalesPorCodigo
        {
            get { return base.GetSqlXml("ObtenerCanalesPorCodigo"); }
        }

        public string SqlActualizarEstado
        {
            get { return base.GetSqlXml("ActualizarEstado"); }
        }

        public string SqlObtenerEmpresa
        {
            get { return base.GetSqlXml("ObtenerEmpresa"); }
        }

        #region "FIT Señales no Disponibles"

        public string Motivo = "MOTIVO";
        public string Tiempo = "TIEMPO";
        public string Calidad = "CALIDAD";
        public string Caida = "CAIDA";
        public string Canalfhora = "CANALFHORA";
        public string Canalfhora2 = "CANALFHORA2";

        public string Obscanproceso = "OBSCANPROCESO";

        public string SqlObtenerNroFilaBusquedaCanalSenalesObservadasBusqueda
        {
            get { return base.GetSqlXml("ObtenerNroFilaBusquedaCanalSenalesObservadasBusqueda"); }
        }

        public string SqlObtenerCanalesSenalesObservadasBusqueda
        {
            get { return base.GetSqlXml("ObtenerCanalesSenalesObservadasBusqueda"); }
        }

        public string SqlObtenerSenalesObservadas
        {
            get { return base.GetSqlXml("ObtenerSenalesObservadas"); }
        }

        public string SqlObtenerSenalesObservadasReportadas
        {
            get { return base.GetSqlXml("ObtenerSenalesObservadasReportadas"); }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AUD_ELEMENTO
    /// </summary>
    public class AudElementoHelper : HelperBase
    {
        public AudElementoHelper(): base(Consultas.AudElementoSql)
        {
        }
        
        public AudElementoDTO Create(IDataReader dr)
        {
            AudElementoDTO entity = new AudElementoDTO();

            int iElemcodi = dr.GetOrdinal(this.Elemcodi);
            if (!dr.IsDBNull(iElemcodi)) entity.Elemcodi = Convert.ToInt32(dr.GetValue(iElemcodi));

            int iTabcdcoditipoelemento = dr.GetOrdinal(this.Tabcdcoditipoelemento);
            if (!dr.IsDBNull(iTabcdcoditipoelemento)) entity.Tabcdcoditipoelemento = Convert.ToInt32(dr.GetValue(iTabcdcoditipoelemento));

            int iProccodi = dr.GetOrdinal(this.Proccodi);
            if (!dr.IsDBNull(iProccodi)) entity.Proccodi = Convert.ToInt32(dr.GetValue(iProccodi));

            int iElemcodigo = dr.GetOrdinal(this.Elemcodigo);
            if (!dr.IsDBNull(iElemcodigo)) entity.Elemcodigo = dr.GetString(iElemcodigo);

            int iElemdescripcion = dr.GetOrdinal(this.Elemdescripcion);
            if (!dr.IsDBNull(iElemdescripcion)) entity.Elemdescripcion = dr.GetString(iElemdescripcion);

            int iElemactivo = dr.GetOrdinal(this.Elemactivo);
            if (!dr.IsDBNull(iElemactivo)) entity.Elemactivo = dr.GetString(iElemactivo);

            int iElemhistorico = dr.GetOrdinal(this.Elemhistorico);
            if (!dr.IsDBNull(iElemhistorico)) entity.Elemhistorico = dr.GetString(iElemhistorico);

            int iElemusucreacion = dr.GetOrdinal(this.Elemusucreacion);
            if (!dr.IsDBNull(iElemusucreacion)) entity.Elemusucreacion = dr.GetString(iElemusucreacion);

            int iElemfeccreacion = dr.GetOrdinal(this.Elemfeccreacion);
            if (!dr.IsDBNull(iElemfeccreacion)) entity.Elemfeccreacion = dr.GetDateTime(iElemfeccreacion);

            int iElemusumodificacion = dr.GetOrdinal(this.Elemusumodificacion);
            if (!dr.IsDBNull(iElemusumodificacion)) entity.Elemusumodificacion = dr.GetString(iElemusumodificacion);

            int iElemfecmodificacion = dr.GetOrdinal(this.Elemfecmodificacion);
            if (!dr.IsDBNull(iElemfecmodificacion)) entity.Elemfecmodificacion = dr.GetDateTime(iElemfecmodificacion);

            //int iAreaCodi = dr.GetOrdinal(this.AreaCodi);
            //if (!dr.IsDBNull(iAreaCodi)) entity.AreaCodi = dr.GetString(iAreaCodi);

            //int iAreaNom = dr.GetOrdinal(this.AreaNom);
            //if (!dr.IsDBNull(iAreaNom)) entity.AreaNom = dr.GetString(iAreaNom);
            return entity;
        }


        #region Mapeo de Campos

        public string Elemcodi = "ELEMCODI";
        public string Tabcdcoditipoelemento = "TABCDCODITIPOELEMENTO";
        public string Proccodi = "PROCCODI";
        public string Elemcodigo = "ELEMCODIGO";
        public string Elemdescripcion = "ELEMDESCRIPCION";
        public string Elemactivo = "ELEMACTIVO";
        public string Elemhistorico = "ELEMHISTORICO";
        public string Elemusucreacion = "ELEMUSUCREACION";
        public string Elemfeccreacion = "ELEMFECCREACION";
        public string Elemusumodificacion = "ELEMUSUMODIFICACION";
        public string Elemfecmodificacion = "ELEMFECMODIFICACION";
        public string AreaCodi = "AREACODI";
        public string AreaNom = "AREANOM";

        public string Audppcodi = "audppcodi";
        public string Validacionmensaje = "ValidacionMensaje";
        public string Existeprogaudielemento = "existeprogaudielemento";
        
        #endregion


        public string SqlGetByElementoPorArea
        {
            get { return base.GetSqlXml("GetByAreaElemento"); }
        }

        public string SqlGetByProcesoElemento
        {
            get { return base.GetSqlXml("GetByProcesoElemento"); }
        }

        public string SqlGetByProcesoPorElemento
        {
            get { return base.GetSqlXml("GetByProcesoPorElementos"); }
        }

        public string SqlGetByElementosPorProceso
        {
            get { return base.GetSqlXml("GetByElementosPorProceso"); }
        }

        public string SqlGetByElementosPorProcesoAP
        {
            get { return base.GetSqlXml("GetByElementosPorProcesoAP"); }
        }
        
        public string SqlGetByElementosPorTipo
        {
            get { return base.GetSqlXml("GetByElementosPorTipo"); }
        }

        public string SqlGetByElementosPorProcesoTipo
        {
            get { return base.GetSqlXml("GetByElementosPorProcesoTipo"); }
        }
        
        public string SqlObtenerNroRegistroBusqueda
        {
            get { return base.GetSqlXml("ObtenerNroRegistroBusqueda"); }
        }

        public string SqlGetByElementoValidacion
        {
            get { return base.GetSqlXml("GetByElementoValidacion"); }
        }
        
    }
}

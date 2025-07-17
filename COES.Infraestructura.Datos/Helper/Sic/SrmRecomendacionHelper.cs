using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SRM_RECOMENDACION
    /// </summary>
    public class SrmRecomendacionHelper : HelperBase
    {
        public SrmRecomendacionHelper(): base(Consultas.SrmRecomendacionSql)
        {
        }

        public SrmRecomendacionDTO Create(IDataReader dr)
        {
            SrmRecomendacionDTO entity = new SrmRecomendacionDTO();

            int iSrmreccodi = dr.GetOrdinal(this.Srmreccodi);
            if (!dr.IsDBNull(iSrmreccodi)) entity.Srmreccodi = Convert.ToInt32(dr.GetValue(iSrmreccodi));

            int iEvencodi = dr.GetOrdinal(this.Evencodi);
            if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iSrmcrtcodi = dr.GetOrdinal(this.Srmcrtcodi);
            if (!dr.IsDBNull(iSrmcrtcodi)) entity.Srmcrtcodi = Convert.ToInt32(dr.GetValue(iSrmcrtcodi));

            int iSrmstdcodi = dr.GetOrdinal(this.Srmstdcodi);
            if (!dr.IsDBNull(iSrmstdcodi)) entity.Srmstdcodi = Convert.ToInt32(dr.GetValue(iSrmstdcodi));

            int iUsercode = dr.GetOrdinal(this.Usercode);
            if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

            int iSrmrecfecharecomend = dr.GetOrdinal(this.Srmrecfecharecomend);
            if (!dr.IsDBNull(iSrmrecfecharecomend)) entity.Srmrecfecharecomend = dr.GetDateTime(iSrmrecfecharecomend);

            int iSrmrecfechavencim = dr.GetOrdinal(this.Srmrecfechavencim);
            if (!dr.IsDBNull(iSrmrecfechavencim)) entity.Srmrecfechavencim = dr.GetDateTime(iSrmrecfechavencim);

            int iSrmrecdianotifplazo = dr.GetOrdinal(this.Srmrecdianotifplazo);
            if (!dr.IsDBNull(iSrmrecdianotifplazo)) entity.Srmrecdianotifplazo = Convert.ToInt32(dr.GetValue(iSrmrecdianotifplazo));

            int iSrmrectitulo = dr.GetOrdinal(this.Srmrectitulo);
            if (!dr.IsDBNull(iSrmrectitulo)) entity.Srmrectitulo = dr.GetString(iSrmrectitulo);

            int iSrmrecrecomendacion = dr.GetOrdinal(this.Srmrecrecomendacion);
            if (!dr.IsDBNull(iSrmrecrecomendacion)) entity.Srmrecrecomendacion = dr.GetString(iSrmrecrecomendacion);

            int iSrmrecactivo = dr.GetOrdinal(this.Srmrecactivo);
            if (!dr.IsDBNull(iSrmrecactivo)) entity.Srmrecactivo = dr.GetString(iSrmrecactivo);

            int iSrmrecusucreacion = dr.GetOrdinal(this.Srmrecusucreacion);
            if (!dr.IsDBNull(iSrmrecusucreacion)) entity.Srmrecusucreacion = dr.GetString(iSrmrecusucreacion);

            int iSrmrecfeccreacion = dr.GetOrdinal(this.Srmrecfeccreacion);
            if (!dr.IsDBNull(iSrmrecfeccreacion)) entity.Srmrecfeccreacion = dr.GetDateTime(iSrmrecfeccreacion);

            int iSrmrecusumodificacion = dr.GetOrdinal(this.Srmrecusumodificacion);
            if (!dr.IsDBNull(iSrmrecusumodificacion)) entity.Srmrecusumodificacion = dr.GetString(iSrmrecusumodificacion);

            int iSrmrecfecmodificacion = dr.GetOrdinal(this.Srmrecfecmodificacion);
            if (!dr.IsDBNull(iSrmrecfecmodificacion)) entity.Srmrecfecmodificacion = dr.GetDateTime(iSrmrecfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Srmreccodi = "SRMRECCODI";
        public string Evencodi = "EVENCODI";
        public string Equicodi = "EQUICODI";
        public string Srmcrtcodi = "SRMCRTCODI";
        public string Srmstdcodi = "SRMSTDCODI";
        public string Usercode = "USERCODE";
        public string Srmrecfecharecomend = "SRMRECFECHARECOMEND";
        public string Srmrecfechavencim = "SRMRECFECHAVENCIM";
        public string Srmrecdianotifplazo = "SRMRECDIANOTIFPLAZO";
        public string Srmrectitulo = "SRMRECTITULO";
        public string Srmrecrecomendacion = "SRMRECRECOMENDACION";
        public string Srmrecactivo = "SRMRECACTIVO";
        public string Srmrecusucreacion = "SRMRECUSUCREACION";
        public string Srmrecfeccreacion = "SRMRECFECCREACION";
        public string Srmrecusumodificacion = "SRMRECUSUMODIFICACION";
        public string Srmrecfecmodificacion = "SRMRECFECMODIFICACION";
        public string Evenini = "EVENINI";
        public string Equiabrev = "EQUIABREV";
        public string Srmcrtdescrip = "SRMCRTDESCRIP";
        public string Srmstddescrip = "SRMSTDDESCRIP";
        public string Username = "USERNAME";

        public string Tipo = "TIPO";
        public string Emprnomb = "EMPRNOMB";
        public string Areanomb = "AREANOMB";        
        public string Evenfin = "EVENFIN";
        public string EvenAsunto = "EVENASUNTO";
        public string Equinomb = "EQUINOMB";
        public string Srmstdcolor = "SRMSTDCOLOR";
        public string Srmcrtcolor = "SRMCRTCOLOR";
        public string Comentario = "COMENTARIO";
        public string Emprcodi = "EMPRCODI";
        public string Famcodi = "FAMCODI";
        public string Famnomb = "FAMNOMB";
        public string Registros = "REGISTROS";

        public string Avencer = "AVENCER";
        public string Vencido = "VENCIDO";
        public string Ciclico = "CICLICO";
        public string ConComentario = "CONCOMENTARIO";

        public string Evenrcmctaf = "EVENRCMCTAF";
        public string Afrrec = "AFRREC";

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        public string ObtenerListadoGestion
        {
            get { return base.GetSqlXml("ObtenerListadoGestion"); }
        }

        public string TotalRegistrosGestion
        {
            get { return base.GetSqlXml("TotalRegistrosGestion"); }
        }


        public string ObtenerListadoGestionFaltante
        {
            get { return base.GetSqlXml("ObtenerListadoGestionFaltante"); }
        }

        public string TotalRegistrosGestionFaltante
        {
            get { return base.GetSqlXml("TotalRegistrosGestionFaltante"); }
        }

        public string ObtenerListadoRecomendacion
        {
            get { return base.GetSqlXml("ObtenerListadoRecomendacion"); }
        }

        public string ObtenerListadoReporte
        {
            get { return base.GetSqlXml("ObtenerListadoReporte"); }
        }

        public string TotalRegistrosListadoReporte
        {
            get { return base.GetSqlXml("TotalRegistrosListadoReporte"); }
        }

        public string ObtenerListadoEmpresaCriticidad
        {
            get { return base.GetSqlXml("ObtenerListadoEmpresaCriticidad"); }
        }

        public string ObtenerListadoEmpresaEstado
        {
            get { return base.GetSqlXml("ObtenerListadoEmpresaEstado"); }
        }

        public string ObtenerListadoTipoEquipoCriticidad
        {
            get { return base.GetSqlXml("ObtenerListadoTipoEquipoCriticidad"); }
        }
        public string ObtenerListadoTipoEquipoEstado
        {
            get { return base.GetSqlXml("ObtenerListadoTipoEquipoEstado"); }
        }

        public string ObtenerListadoEstado
        {
            get { return base.GetSqlXml("ObtenerListadoEstado"); }
        }

        public string ObtenerListadoEstadoCriticidad
        {
            get { return base.GetSqlXml("ObtenerListadoEstadoCriticidad"); }
        }

        public string ObtenerListadoCriticidad
        {
            get { return base.GetSqlXml("ObtenerListadoCriticidad"); }
        }

        public string TotalRegistrosRec
        {
            get { return base.GetSqlXml("TotalRegistrosRec"); }
        }

        public string ObtenerListadoAlarma
        {
            get { return base.GetSqlXml("ObtenerListadoAlarma"); }
        }
        public string TotalRegistrosCtaf
        {
            get { return base.GetSqlXml("TotalRegistrosCtaf"); }
        }
        public string ObtenerListadoGestionCtaf
        {
            get { return base.GetSqlXml("ObtenerListadoGestionCtaf"); }
        }
        public string ListadoRecomendacionesEventosCtaf
        {
            get { return base.GetSqlXml("ListadoRecomendacionesEventosCtaf"); }
        }
        public string ValidaRecomendacionxEventoEstado
        {
            get { return base.GetSqlXml("ValidaRecomendacionxEventoEstado"); }
        }
        public string ObtenerRecomendacionEvento
        {
            get { return base.GetSqlXml("ObtenerRecomendacionEvento"); }
        }
        #endregion
    }
}

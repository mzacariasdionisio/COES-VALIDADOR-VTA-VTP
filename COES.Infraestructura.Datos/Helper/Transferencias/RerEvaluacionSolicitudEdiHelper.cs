using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_EVALUACION_SOLICITUDEDI
    /// </summary>
    public class RerEvaluacionSolicitudEdiHelper : HelperBase
    {
        #region Mapeo de columnas
        //table
        public string Reresecodi = "RERESECODI";
        public string Rerevacodi = "REREVACODI";
        public string Rersedcodi = "RERSEDCODI";
        public string Rercencodi = "RERCENCODI";
        public string Emprcodi = "EMPRCODI";
        public string Ipericodi = "IPERICODI";
        public string Reroricodi = "RERORICODI";
        public string Reresefechahorainicio = "RERESEFECHAHORAINICIO";
        public string Reresefechahorafin = "RERESEFECHAHORAFIN";
        public string Reresedesc = "RERESEDESC";
        public string Reresetotenergia = "RERESETOTENERGIA";
        public string Reresesustento = "RERESESUSTENTO";
        public string Rereseestadodeenvio = "RERESEESTADODEENVIO";
        public string Rereseeliminado = "RERESEELIMINADO";
        public string Rereseusucreacionext = "RERESEUSUCREACIONEXT";
        public string Reresefeccreacionext = "RERESEFECCREACIONEXT";
        public string Rereseusumodificacionext = "RERESEUSUMODIFICACIONEXT";
        public string Reresefecmodificacionext = "RERESEFECMODIFICACIONEXT";
        public string Rereseusucreacion = "RERESEUSUCREACION";
        public string Reresefeccreacion = "RERESEFECCREACION";
        public string Rereseusumodificacion = "RERESEUSUMODIFICACION";
        public string Reresefecmodificacion = "RERESEFECMODIFICACION";
        public string Reresetotenergiaestimada = "RERESETOTENERGIAESTIMADA";
        public string Rereseediaprobada = "RERESEEDIAPROBADA";
        public string Rereserfpmc = "RERESERFPMC";
        public string Rereseresdesc = "RERESERESDESC";
        public string Rereseresestado = "RERESERESESTADO";

        //additional
        public string Equicodi = "EQUICODI";
        public string Equinomb = "EQUINOMB";
        public string Emprnomb = "EMPRNOMB";
        public string Reroridesc = "RERORIDESC";
        public string Rercenestado = "RERCENESTADO";
        public string Iperianio = "IPERIANIO";
        public string Iperimes = "IPERIMES";
        #endregion

        public RerEvaluacionSolicitudEdiHelper() : base(Consultas.RerEvaluacionSolicitudEdiSql)
        {
        }

        public RerEvaluacionSolicitudEdiDTO CreateById(IDataReader dr)
        {
            RerEvaluacionSolicitudEdiDTO entity = new RerEvaluacionSolicitudEdiDTO();
            SetCreate(dr, entity);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iEquinomb = dr.GetOrdinal(this.Equinomb);
            if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            return entity;
        }

        public RerEvaluacionSolicitudEdiDTO CreateByList(IDataReader dr)
        {
            RerEvaluacionSolicitudEdiDTO entity = new RerEvaluacionSolicitudEdiDTO();
            SetCreate(dr, entity);

            return entity;
        }

        public RerEvaluacionSolicitudEdiDTO CreateByCriteria(IDataReader dr)
        {
            RerEvaluacionSolicitudEdiDTO entity = new RerEvaluacionSolicitudEdiDTO();
            SetCreate(dr, entity);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iEquinomb = dr.GetOrdinal(this.Equinomb);
            if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            int iReroridesc = dr.GetOrdinal(this.Reroridesc);
            if (!dr.IsDBNull(iReroridesc)) entity.Reroridesc = dr.GetString(iReroridesc);

            return entity;
        }

        public RerEvaluacionSolicitudEdiDTO CreateByEvaluacionByEliminadoByEstado(IDataReader dr)
        {
            RerEvaluacionSolicitudEdiDTO entity = new RerEvaluacionSolicitudEdiDTO();
            SetCreate(dr, entity);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iRercenestado = dr.GetOrdinal(this.Rercenestado);
            if (!dr.IsDBNull(iRercenestado)) entity.Equinomb = dr.GetString(iRercenestado);

            int iIperianio = dr.GetOrdinal(this.Iperianio);
            if (!dr.IsDBNull(iIperianio)) entity.Iperianio = Convert.ToInt32(dr.GetValue(iIperianio));

            int iIperimes = dr.GetOrdinal(this.Iperimes);
            if (!dr.IsDBNull(iIperimes)) entity.Iperimes = Convert.ToInt32(dr.GetValue(iIperimes));

            return entity;
        }

        private void SetCreate(IDataReader dr, RerEvaluacionSolicitudEdiDTO entity)
        {
            int iReresecodi = dr.GetOrdinal(this.Reresecodi);
            if (!dr.IsDBNull(iReresecodi)) entity.Reresecodi = Convert.ToInt32(dr.GetValue(iReresecodi));

            int iRerevacodi = dr.GetOrdinal(this.Rerevacodi);
            if (!dr.IsDBNull(iRerevacodi)) entity.Rerevacodi = Convert.ToInt32(dr.GetValue(iRerevacodi));

            int iRersedcodi = dr.GetOrdinal(this.Rersedcodi);
            if (!dr.IsDBNull(iRersedcodi)) entity.Rersedcodi = Convert.ToInt32(dr.GetValue(iRersedcodi));

            int iRercencodi = dr.GetOrdinal(this.Rercencodi);
            if (!dr.IsDBNull(iRercencodi)) entity.Rercencodi = Convert.ToInt32(dr.GetValue(iRercencodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iIpericodi = dr.GetOrdinal(this.Ipericodi);
            if (!dr.IsDBNull(iIpericodi)) entity.Ipericodi = Convert.ToInt32(dr.GetValue(iIpericodi));

            int iReroricodi = dr.GetOrdinal(this.Reroricodi);
            if (!dr.IsDBNull(iReroricodi)) entity.Reroricodi = Convert.ToInt32(dr.GetValue(iReroricodi));

            int iReresefechahorainicio = dr.GetOrdinal(this.Reresefechahorainicio);
            if (!dr.IsDBNull(iReresefechahorainicio)) entity.Reresefechahorainicio = dr.GetDateTime(iReresefechahorainicio);

            int iReresefechahorafin = dr.GetOrdinal(this.Reresefechahorafin);
            if (!dr.IsDBNull(iReresefechahorafin)) entity.Reresefechahorafin = dr.GetDateTime(iReresefechahorafin);

            int iReresedesc = dr.GetOrdinal(this.Reresedesc);
            if (!dr.IsDBNull(iReresedesc)) entity.Reresedesc = dr.GetString(iReresedesc);

            int iReresetotenergia = dr.GetOrdinal(this.Reresetotenergia);
            if (!dr.IsDBNull(iReresetotenergia)) entity.Reresetotenergia = Convert.ToDecimal(dr.GetValue(iReresetotenergia));

            int iReresesustento = dr.GetOrdinal(this.Reresesustento);
            if (!dr.IsDBNull(iReresesustento)) entity.Reresesustento = dr.GetString(iReresesustento);

            int iRereseestadodeenvio = dr.GetOrdinal(this.Rereseestadodeenvio);
            if (!dr.IsDBNull(iRereseestadodeenvio)) entity.Rereseestadodeenvio = dr.GetString(iRereseestadodeenvio);

            int iRereseeliminado = dr.GetOrdinal(this.Rereseeliminado);
            if (!dr.IsDBNull(iRereseeliminado)) entity.Rereseeliminado = dr.GetString(iRereseeliminado);

            int iRereseusucreacionext = dr.GetOrdinal(this.Rereseusucreacionext);
            if (!dr.IsDBNull(iRereseusucreacionext)) entity.Rereseusucreacionext = dr.GetString(iRereseusucreacionext);

            int iReresefeccreacionext = dr.GetOrdinal(this.Reresefeccreacionext);
            if (!dr.IsDBNull(iReresefeccreacionext)) entity.Reresefeccreacionext = dr.GetDateTime(iReresefeccreacionext);

            int iRereseusumodificacionext = dr.GetOrdinal(this.Rereseusumodificacionext);
            if (!dr.IsDBNull(iRereseusumodificacionext)) entity.Rereseusumodificacionext = dr.GetString(iRereseusumodificacionext);

            int iReresefecmodificacionext = dr.GetOrdinal(this.Reresefecmodificacionext);
            if (!dr.IsDBNull(iReresefecmodificacionext)) entity.Reresefecmodificacionext = dr.GetDateTime(iReresefecmodificacionext);

            int iRereseusucreacion = dr.GetOrdinal(this.Rereseusucreacion);
            if (!dr.IsDBNull(iRereseusucreacion)) entity.Rereseusucreacion = dr.GetString(iRereseusucreacion);

            int iReresefeccreacion = dr.GetOrdinal(this.Reresefeccreacion);
            if (!dr.IsDBNull(iReresefeccreacion)) entity.Reresefeccreacion = dr.GetDateTime(iReresefeccreacion);

            int iRereseusumodificacion = dr.GetOrdinal(this.Rereseusumodificacion);
            if (!dr.IsDBNull(iRereseusumodificacion)) entity.Rereseusumodificacion = dr.GetString(iRereseusumodificacion);

            int iReresefecmodificacion = dr.GetOrdinal(this.Reresefecmodificacion);
            if (!dr.IsDBNull(iReresefecmodificacion)) entity.Reresefecmodificacion = dr.GetDateTime(iReresefecmodificacion);

            int iReresetotenergiaestimada = dr.GetOrdinal(this.Reresetotenergiaestimada);
            if (!dr.IsDBNull(iReresetotenergiaestimada)) entity.Reresetotenergiaestimada = Convert.ToDecimal(dr.GetValue(iReresetotenergiaestimada));

            int iRereseediaprobada = dr.GetOrdinal(this.Rereseediaprobada);
            if (!dr.IsDBNull(iRereseediaprobada)) entity.Rereseediaprobada = Convert.ToDecimal(dr.GetValue(iRereseediaprobada));

            int iRereserfpmc = dr.GetOrdinal(this.Rereserfpmc);
            if (!dr.IsDBNull(iRereserfpmc)) entity.Rereserfpmc = Convert.ToDecimal(dr.GetValue(iRereserfpmc));

            int iRereseresdesc = dr.GetOrdinal(this.Rereseresdesc);
            if (!dr.IsDBNull(iRereseresdesc)) entity.Rereseresdesc = dr.GetString(iRereseresdesc);

            int iRereseresestado = dr.GetOrdinal(this.Rereseresestado);
            if (!dr.IsDBNull(iRereseresestado)) entity.Rereseresestado = dr.GetString(iRereseresestado);
        }

        public string SqlUpdateFields
        {
            get { return base.GetSqlXml("UpdateFields"); }
        }

        public string SqlUpdateFieldsForResults
        {
            get { return base.GetSqlXml("UpdateFieldsForResults"); }
        }

        public string SqlUpdateEnergiaEstimada
        {
            get { return base.GetSqlXml("UpdateEnergiaEstimada"); }
        }

        public string SqlGetByEvaluacionByEliminadoByEstado
        {
            get { return base.GetSqlXml("GetByEvaluacionByEliminadoByEstado"); }
        }

    }
}
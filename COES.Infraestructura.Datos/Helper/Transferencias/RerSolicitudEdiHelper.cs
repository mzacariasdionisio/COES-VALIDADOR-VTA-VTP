using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_SOLICITUDEDI
    /// </summary>
    public class RerSolicitudEdiHelper : HelperBase
    {
        public RerSolicitudEdiHelper() : base(Consultas.RerSolicitudEdiSql)
        {
        }

        public RerSolicitudEdiDTO Create(IDataReader dr)
        {
            RerSolicitudEdiDTO entity = new RerSolicitudEdiDTO();

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

            int iRersedfechahorainicio = dr.GetOrdinal(this.Rersedfechahorainicio);
            if (!dr.IsDBNull(iRersedfechahorainicio)) entity.Rersedfechahorainicio = dr.GetDateTime(iRersedfechahorainicio);

            int iRersedfechahorafin = dr.GetOrdinal(this.Rersedfechahorafin);
            if (!dr.IsDBNull(iRersedfechahorafin)) entity.Rersedfechahorafin = dr.GetDateTime(iRersedfechahorafin);

            int iRerseddesc = dr.GetOrdinal(this.Rerseddesc);
            if (!dr.IsDBNull(iRerseddesc)) entity.Rerseddesc = dr.GetString(iRerseddesc);

            int iRersedtotenergia = dr.GetOrdinal(this.Rersedtotenergia);
            if (!dr.IsDBNull(iRersedtotenergia)) entity.Rersedtotenergia = dr.GetDecimal(iRersedtotenergia);

            int iRersedsustento = dr.GetOrdinal(this.Rersedsustento);
            if (!dr.IsDBNull(iRersedsustento)) entity.Rersedsustento = dr.GetString(iRersedsustento);

            int iRersedestadodeenvio = dr.GetOrdinal(this.Rersedestadodeenvio);
            if (!dr.IsDBNull(iRersedestadodeenvio)) entity.Rersedestadodeenvio = dr.GetString(iRersedestadodeenvio);

            int iRersedeliminado = dr.GetOrdinal(this.Rersedeliminado);
            if (!dr.IsDBNull(iRersedeliminado)) entity.Rersedeliminado = dr.GetString(iRersedeliminado);

            int iRersedusucreacion = dr.GetOrdinal(this.Rersedusucreacion);
            if (!dr.IsDBNull(iRersedusucreacion)) entity.Rersedusucreacion = dr.GetString(iRersedusucreacion);

            int iRersedfeccreacion = dr.GetOrdinal(this.Rersedfeccreacion);
            if (!dr.IsDBNull(iRersedfeccreacion)) entity.Rersedfeccreacion = dr.GetDateTime(iRersedfeccreacion);

            int iRersedusumodificacion = dr.GetOrdinal(this.Rersedusumodificacion);
            if (!dr.IsDBNull(iRersedusumodificacion)) entity.Rersedusumodificacion = dr.GetString(iRersedusumodificacion);

            int iRersedfecmodificacion = dr.GetOrdinal(this.Rersedfecmodificacion);
            if (!dr.IsDBNull(iRersedfecmodificacion)) entity.Rersedfecmodificacion = dr.GetDateTime(iRersedfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Rersedcodi = "RERSEDCODI";
        public string Rercencodi = "RERCENCODI";
        public string Emprcodi = "EMPRCODI";
        public string Ipericodi = "IPERICODI";
        public string Reroricodi = "RERORICODI";
        public string Rersedfechahorainicio = "RERSEDFECHAHORAINICIO";
        public string Rersedfechahorafin = "RERSEDFECHAHORAFIN";
        public string Rerseddesc = "RERSEDDESC";
        public string Rersedtotenergia = "RERSEDTOTENERGIA";
        public string Rersedsustento = "RERSEDSUSTENTO";
        public string Rersedestadodeenvio = "RERSEDESTADODEENVIO";
        public string Rersedeliminado = "RERSEDELIMINADO";
        public string Rersedusucreacion = "RERSEDUSUCREACION";
        public string Rersedfeccreacion = "RERSEDFECCREACION";
        public string Rersedusumodificacion = "RERSEDUSUMODIFICACION";
        public string Rersedfecmodificacion = "RERSEDFECMODIFICACION";

        public string Emprnomb = "EMPRNOMB";
        public string Equicodi = "EQUICODI";
        public string Equinomb = "EQUINOMB";
        public string Reroridesc = "RERORIDESC";
        #endregion

        public string SqlListaPorEmpresaYPeriodo
        {
            get { return base.GetSqlXml("ListaPorEmpresaYPeriodo"); }
        }

        public string SqlLogicalDelete
        {
            get { return base.GetSqlXml("LogicalDelete"); }
        }

        public string SqlGetByIdView
        {
            get { return base.GetSqlXml("GetByIdView"); }
        }

        public string SqlListByPeriodo
        {
            get { return base.GetSqlXml("ListByPeriodo");  }
        }
    }
}
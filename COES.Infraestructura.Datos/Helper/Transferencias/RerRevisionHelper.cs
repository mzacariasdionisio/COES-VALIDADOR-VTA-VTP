using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_REVISION
    /// </summary>
    public class RerRevisionHelper : HelperBase
    {
        #region Mapeo de Campos
        //table
        public string Rerrevcodi = "RERREVCODI";
        public string Ipericodi = "IPERICODI";
        public string Rerrevnombre = "RERREVNOMBRE";
        public string Rerrevtipo = "RERREVTIPO";
        public string Rerrevfecha = "RERREVFECHA";
        public string Rerrevestado = "RERREVESTADO";
        public string Rerrevusucreacion = "RERREVUSUCREACION";
        public string Rerrevfeccreacion = "RERREVFECCREACION";
        public string Rerrevusumodificacion = "RERREVUSUMODIFICACION";
        public string Rerrevfecmodificacion = "RERREVFECMODIFICACION";

        //additional
        public string Iperinombre = "IPERINOMBRE";
        public string Iperianio = "IPERIANIO";
        public string Iperimes = "IPERIMES";
        public string RerrevfechaentregaEDI = "RERREVFECHAENTREGAEDI";

        #endregion

        public RerRevisionHelper() : base(Consultas.RerRevisionSql)
        {
        }

        public RerRevisionDTO Create(IDataReader dr)
        {
            RerRevisionDTO entity = new RerRevisionDTO();
            SetCreate(dr, entity);
            return entity;
        }

        public RerRevisionDTO CreateListPeriodosConUltimaRevision(IDataReader dr)
        {
            RerRevisionDTO entity = new RerRevisionDTO();

            //table: ind_periodo
            int iIpericodi = dr.GetOrdinal(this.Ipericodi);
            if (!dr.IsDBNull(iIpericodi)) entity.Ipericodi = Convert.ToInt32(dr.GetValue(iIpericodi));

            int iIperinombre = dr.GetOrdinal(this.Iperinombre);
            if (!dr.IsDBNull(iIperinombre)) entity.Iperinombre = dr.GetString(iIperinombre);

            int iIperianio = dr.GetOrdinal(this.Iperianio);
            if (!dr.IsDBNull(iIperianio)) entity.Iperianio = Convert.ToInt32(dr.GetValue(iIperianio));

            int iIperimes = dr.GetOrdinal(this.Iperimes);
            if (!dr.IsDBNull(iIperimes)) entity.Iperimes = Convert.ToInt32(dr.GetValue(iIperimes));


            //table: rer_revision
            int iRerrevcodi = dr.GetOrdinal(this.Rerrevcodi);
            if (!dr.IsDBNull(iRerrevcodi)) entity.Rerrevcodi = Convert.ToInt32(dr.GetValue(iRerrevcodi));

            int iRerrevnombre = dr.GetOrdinal(this.Rerrevnombre);
            if (!dr.IsDBNull(iRerrevnombre)) entity.Rerrevnombre = dr.GetString(iRerrevnombre);

            int iRerrevfecha = dr.GetOrdinal(this.Rerrevfecha);
            if (!dr.IsDBNull(iRerrevfecha)) entity.Rerrevfecha = dr.GetDateTime(iRerrevfecha);

            int iRerrevestado = dr.GetOrdinal(this.Rerrevestado);
            if (!dr.IsDBNull(iRerrevestado)) entity.Rerrevestado = dr.GetString(iRerrevestado);

            //additional
            int iRerrevfechaentregaEDI = dr.GetOrdinal(this.RerrevfechaentregaEDI);
            if (!dr.IsDBNull(iRerrevfechaentregaEDI)) entity.RerrevfechaentregaEDI = dr.GetDateTime(iRerrevfechaentregaEDI);

            return entity;
        }

        private void SetCreate(IDataReader dr, RerRevisionDTO entity)
        {
            int iRerrevcodi = dr.GetOrdinal(this.Rerrevcodi);
            if (!dr.IsDBNull(iRerrevcodi)) entity.Rerrevcodi = Convert.ToInt32(dr.GetValue(iRerrevcodi));

            int iIpericodi = dr.GetOrdinal(this.Ipericodi);
            if (!dr.IsDBNull(iIpericodi)) entity.Ipericodi = Convert.ToInt32(dr.GetValue(iIpericodi));

            int iRerrevnombre = dr.GetOrdinal(this.Rerrevnombre);
            if (!dr.IsDBNull(iRerrevnombre)) entity.Rerrevnombre = dr.GetString(iRerrevnombre);

            int iRerrevtipo = dr.GetOrdinal(this.Rerrevtipo);
            if (!dr.IsDBNull(iRerrevtipo)) entity.Rerrevtipo = dr.GetString(iRerrevtipo);

            int iRerrevfecha = dr.GetOrdinal(this.Rerrevfecha);
            if (!dr.IsDBNull(iRerrevfecha)) entity.Rerrevfecha = dr.GetDateTime(iRerrevfecha);

            int iRerrevestado = dr.GetOrdinal(this.Rerrevestado);
            if (!dr.IsDBNull(iRerrevestado)) entity.Rerrevestado = dr.GetString(iRerrevestado);

            int iRerrevusucreacion = dr.GetOrdinal(this.Rerrevusucreacion);
            if (!dr.IsDBNull(iRerrevusucreacion)) entity.Rerrevusucreacion = dr.GetString(iRerrevusucreacion);

            int iRerrevfeccreacion = dr.GetOrdinal(this.Rerrevfeccreacion);
            if (!dr.IsDBNull(iRerrevfeccreacion)) entity.Rerrevfeccreacion = dr.GetDateTime(iRerrevfeccreacion);

            int iRerrevusumodificacion = dr.GetOrdinal(this.Rerrevusumodificacion);
            if (!dr.IsDBNull(iRerrevusumodificacion)) entity.Rerrevusumodificacion = dr.GetString(iRerrevusumodificacion);

            int iRerrevfecmodificacion = dr.GetOrdinal(this.Rerrevfecmodificacion);
            if (!dr.IsDBNull(iRerrevfecmodificacion)) entity.Rerrevfecmodificacion = dr.GetDateTime(iRerrevfecmodificacion);
        }

        public string SqlUpdateEstado
        {
            get { return base.GetSqlXml("UpdateEstado"); }
        }

        public string SqlListPeriodosConUltimaRevision
        {
            get { return base.GetSqlXml("ListPeriodosConUltimaRevision"); }
        }

        public string SqlGetCantidadRevisionesTipoRevision
        {
            get { return base.GetSqlXml("GetCantidadRevisionesTipoRevision"); }
        }
        
    }
}
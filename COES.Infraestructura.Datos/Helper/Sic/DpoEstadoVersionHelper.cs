using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class DpoEstadoVersionHelper : HelperBase
    {
        public DpoEstadoVersionHelper() : base(Consultas.DpoEstadoVersionSql)
        {
        }

        public DpoEstadoVersionDTO Create(IDataReader dr)
        {
            DpoEstadoVersionDTO entity = new DpoEstadoVersionDTO();

            int iDpoevscodi = dr.GetOrdinal(this.Dpoevscodi);
            if (!dr.IsDBNull(iDpoevscodi)) entity.Dpoevscodi = Convert.ToInt32(dr.GetValue(iDpoevscodi));

            int iVergrpcodi = dr.GetOrdinal(this.Vergrpcodi);
            if (!dr.IsDBNull(iVergrpcodi)) entity.Vergrpcodi = Convert.ToInt32(dr.GetValue(iVergrpcodi));

            int iDpoevspadre = dr.GetOrdinal(this.Dpoevspadre);
            if (!dr.IsDBNull(iDpoevspadre)) entity.Dpoevspadre = Convert.ToInt32(dr.GetValue(iDpoevspadre));

            int iDpoevsrepvegt = dr.GetOrdinal(this.Dpoevsrepvegt);
            if (!dr.IsDBNull(iDpoevsrepvegt)) entity.Dpoevsrepvegt = dr.GetString(iDpoevsrepvegt);

            int iDpoevsrepindt = dr.GetOrdinal(this.Dpoevsrepindt);
            if (!dr.IsDBNull(iDpoevsrepindt)) entity.Dpoevsrepindt = dr.GetString(iDpoevsrepindt);

            int iDpoevsrepdesp = dr.GetOrdinal(this.Dpoevsrepdesp);
            if (!dr.IsDBNull(iDpoevsrepdesp)) entity.Dpoevsrepdesp = dr.GetString(iDpoevsrepdesp);

            int iDpoevsusucreacion = dr.GetOrdinal(this.Dpoevsusucreacion);
            if (!dr.IsDBNull(iDpoevsusucreacion)) entity.Dpoevsusucreacion = dr.GetString(iDpoevsusucreacion);

            int iDpoevsfeccreacion = dr.GetOrdinal(this.Dpoevsfeccreacion);
            if (!dr.IsDBNull(iDpoevsfeccreacion)) entity.Dpoevsfeccreacion = dr.GetDateTime(iDpoevsfeccreacion);

            int iDpoevsusumodificacion = dr.GetOrdinal(this.Dpoevsusumodificacion);
            if (!dr.IsDBNull(iDpoevsusumodificacion)) entity.Dpoevsusumodificacion = dr.GetString(iDpoevsusumodificacion);

            int iDpoevsfecmodificacion = dr.GetOrdinal(this.Dpoevsfecmodificacion);
            if (!dr.IsDBNull(iDpoevsfecmodificacion)) entity.Dpoevsfecmodificacion = dr.GetDateTime(iDpoevsfecmodificacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Dpoevscodi = "DPOEVSCODI";
        public string Vergrpcodi = "VERGRPCODI";
        public string Dpoevspadre = "DPOEVSPADRE";
        public string Dpoevsrepvegt = "DPOEVSREPVEGT";
        public string Dpoevsrepindt = "DPOEVSREPINDT";
        public string Dpoevsrepdesp = "DPOEVSREPDESP";
        public string Dpoevsusucreacion = "DPOEVSUSUCREACION";
        public string Dpoevsfeccreacion = "DPOEVSFECCREACION";
        public string Dpoevsusumodificacion = "DPOEVSUSUMODIFICACION";
        public string Dpoevsfecmodificacion = "DPOEVSFECMODIFICACION";
        #endregion

        #region Consultas DB
        public string SqlGetMaxId
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }
        public string SqlGetById
        {
            get { return base.GetSqlXml("GetById"); }
        }
        #endregion

    }
}

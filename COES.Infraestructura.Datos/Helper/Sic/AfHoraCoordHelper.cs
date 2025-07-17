using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AF_HORA_COORD
    /// </summary>
    public class AfHoraCoordHelper : HelperBase
    {
        public AfHoraCoordHelper() : base(Consultas.AfHoraCoordSql)
        {
        }

        public AfHoraCoordDTO Create(IDataReader dr)
        {
            AfHoraCoordDTO entity = new AfHoraCoordDTO();

            int iAfhofecmodificacion = dr.GetOrdinal(this.Afhofecmodificacion);
            if (!dr.IsDBNull(iAfhofecmodificacion)) entity.Afhofecmodificacion = dr.GetDateTime(iAfhofecmodificacion);

            int iAfhousumodificacion = dr.GetOrdinal(this.Afhousumodificacion);
            if (!dr.IsDBNull(iAfhousumodificacion)) entity.Afhousumodificacion = dr.GetString(iAfhousumodificacion);

            int iAfhofeccreacion = dr.GetOrdinal(this.Afhofeccreacion);
            if (!dr.IsDBNull(iAfhofeccreacion)) entity.Afhofeccreacion = dr.GetDateTime(iAfhofeccreacion);

            int iAfhousucreacion = dr.GetOrdinal(this.Afhousucreacion);
            if (!dr.IsDBNull(iAfhousucreacion)) entity.Afhousucreacion = dr.GetString(iAfhousucreacion);

            int iAfhofecha = dr.GetOrdinal(this.Afhofecha);
            if (!dr.IsDBNull(iAfhofecha)) entity.Afhofecha = dr.GetDateTime(iAfhofecha);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iAfhocodi = dr.GetOrdinal(this.Afhocodi);
            if (!dr.IsDBNull(iAfhocodi)) entity.Afhocodi = Convert.ToInt32(dr.GetValue(iAfhocodi));

            int iAfecodi = dr.GetOrdinal(this.Afecodi);
            if (!dr.IsDBNull(iAfecodi)) entity.Afecodi = Convert.ToInt32(dr.GetValue(iAfecodi));

            int iFdatcodi = dr.GetOrdinal(this.Fdatcodi);
            if (!dr.IsDBNull(iFdatcodi)) entity.Fdatcodi = Convert.ToInt32(dr.GetValue(iFdatcodi));

            int iAfhmotivo = dr.GetOrdinal(this.Afhmotivo);
            if (!dr.IsDBNull(iAfhmotivo)) entity.Afhmotivo = dr.GetString(iAfhmotivo);

            return entity;
        }


        #region Mapeo de Campos

        public string Afhofecmodificacion = "AFHOFECMODIFICACION";
        public string Afhousumodificacion = "AFHOUSUMODIFICACION";
        public string Afhofeccreacion = "AFHOFECCREACION";
        public string Afhousucreacion = "AFHOUSUCREACION";
        public string Afhofecha = "AFHOFECHA";
        public string Emprcodi = "EMPRCODI";
        public string Afhocodi = "AFHOCODI";
        public string Afecodi = "AFECODI";
        public string Fdatcodi = "FDATCODI";
        public string Afhmotivo = "AFHMOTIVO";
        public string Intsumcodi = "INTSUMCODI";

        public string Afeanio = "AFEANIO";
        public string Afecorr = "AFECORR";

        public string Eracmfsuministrador = "ERACMFSUMINISTRADOR";

        #endregion

        /// <summary>
        /// Retorna las sentencias SQL
        /// </summary>
        #region Sentencias SQL

        public string SqlListHoraCoord
        {
            get { return GetSqlXml("ListHoraCoord"); }
        }
        public string SqlListHoraCoordCTAF
        {
            get { return GetSqlXml("ListHoraCoordCTAF"); }
        }
        public string SqlDeleteHoraCoord
        {
            get { return GetSqlXml("DeleteHoraCoord"); }
        }

        public string SqlListHoraCoordSuministradora
        {
            get { return GetSqlXml("ListHoraCoordSuministradora"); }
        }
        public string SqlListEmpClixSuministradora
        {
            get { return GetSqlXml("ListEmpClixSuministradora"); }
        }
        #endregion
    }
}

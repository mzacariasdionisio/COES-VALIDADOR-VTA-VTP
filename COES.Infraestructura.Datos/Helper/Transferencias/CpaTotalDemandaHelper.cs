using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_TOTAL_DEMANDA
    /// </summary>
    public class CpaTotalDemandaHelper : HelperBase
    {
        public CpaTotalDemandaHelper() : base(Consultas.CpaTotalDemandaSql)
        {
        }

        public CpaTotalDemandaDTO Create(IDataReader dr)
        {
            CpaTotalDemandaDTO entity = new CpaTotalDemandaDTO();


            int iCpatdcodi = dr.GetOrdinal(this.Cpatdcodi);
            if (!dr.IsDBNull(iCpatdcodi)) entity.Cpatdcodi = Convert.ToInt32(dr.GetValue(iCpatdcodi));

            int iCpatdanio = dr.GetOrdinal(this.Cpatdanio);
            if (!dr.IsDBNull(iCpatdanio)) entity.Cpatdanio = Convert.ToInt32(dr.GetValue(iCpatdanio));

            int iCpatdajuste = dr.GetOrdinal(this.Cpatdajuste);
            if (!dr.IsDBNull(iCpatdajuste)) entity.Cpatdajuste = dr.GetString(iCpatdajuste);

            int iCparcodi = dr.GetOrdinal(this.Cparcodi);
            if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = Convert.ToInt32(dr.GetValue(iCparcodi));

            int iCpatdtipo = dr.GetOrdinal(this.Cpatdtipo);
            if (!dr.IsDBNull(iCpatdtipo)) entity.Cpatdtipo = dr.GetString(iCpatdtipo);

            int iCpatdmes = dr.GetOrdinal(this.Cpatdmes);
            if (!dr.IsDBNull(iCpatdmes)) entity.Cpatdmes = Convert.ToInt32(dr.GetValue(iCpatdmes));

            int iCpatdusucreacion = dr.GetOrdinal(this.Cpatdusucreacion);
            if (!dr.IsDBNull(iCpatdusucreacion)) entity.Cpatdusucreacion = dr.GetString(iCpatdusucreacion);

            int iCpatdfeccreacion = dr.GetOrdinal(this.Cpatdfeccreacion);
            if (!dr.IsDBNull(iCpatdfeccreacion)) entity.Cpatdfeccreacion = dr.GetDateTime(iCpatdfeccreacion);

            int iCpatdusumodificacion = dr.GetOrdinal(this.Cpatdusumodificacion);
            if (!dr.IsDBNull(iCpatdusumodificacion)) entity.Cpatdusumodificacion = dr.GetString(iCpatdusumodificacion);

            int iCpatdfecmodificacion = dr.GetOrdinal(this.Cpatdfecmodificacion);
            if (!dr.IsDBNull(iCpatdfecmodificacion)) entity.Cpatdfecmodificacion = dr.GetDateTime(iCpatdfecmodificacion);


            return entity;
        }

        #region Mapeo de Campos
        public string Cpatdcodi = "CPATDCODI";
        public string Cpatdanio = "CPATDANIO";
        public string Cpatdajuste = "CPATDAJUSTE";
        public string Cparcodi = "CPARCODI";
        public string Cpatdtipo = "CPATDTIPO";
        public string Cpatdmes = "CPATDMES";
        public string Cpatdusucreacion = "CPATDUSUCREACION";
        public string Cpatdfeccreacion = "CPATDFECCREACION";
        public string Cpatdusumodificacion = "CPATDUSUMODIFICACION";
        public string Cpatdfecmodificacion = "CPATDFECMODIFICACION";
        #endregion

        #region Querys
        public string SqlObtenerNroRegistroEnvios
        {
            get { return base.GetSqlXml("ObtenerNroRegistroEnvios"); }
        }

        public string SqlObtenerNroRegistroEnviosFiltros
        {
            get { return base.GetSqlXml("ObtenerNroRegistroEnviosFiltros"); }
        }

        public string SqlObtenerEnvios
        {
            get { return base.GetSqlXml("ObtenerEnvios"); }
        }

        public string SqlObtenerEstadoRevisionDemanda
        {
            get { return base.GetSqlXml("ObtenerEstadoRevisionDemanda"); }
        }

        public string SqlObtenerNroRegistrosCPPEJDemanda
        {
            get { return base.GetSqlXml("ObtenerNroRegistrosCPPEJDemanda"); }
        }

        public string SqlDeleteCPPEJDemanda
        {
            get { return base.GetSqlXml("DeleteCPPEJDemanda"); }
        }

        public string SqlObtenerTipoEmpresaCPAPorNombre
        {
            get { return base.GetSqlXml("ObtenerTipoEmpresaCPAPorNombre"); }
        }
        #endregion
    }
}


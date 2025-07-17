using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_TOTAL_TRANSMISORES
    /// </summary>
    public class CpaTotalTransmisoresHelper : HelperBase
    {
        public CpaTotalTransmisoresHelper() : base(Consultas.CpaTotalTransmisoresSql)
        {
        }

        public CpaTotalTransmisoresDTO Create(IDataReader dr)
        {
            CpaTotalTransmisoresDTO entity = new CpaTotalTransmisoresDTO();


            int iCpattcodi = dr.GetOrdinal(this.Cpattcodi);
            if (!dr.IsDBNull(iCpattcodi)) entity.Cpattcodi = Convert.ToInt32(dr.GetValue(iCpattcodi));

            int iCpattanio = dr.GetOrdinal(this.Cpattanio);
            if (!dr.IsDBNull(iCpattanio)) entity.Cpattanio = Convert.ToInt32(dr.GetValue(iCpattanio));

            int iCpattajuste = dr.GetOrdinal(this.Cpattajuste);
            if (!dr.IsDBNull(iCpattajuste)) entity.Cpattajuste = dr.GetString(iCpattajuste);

            int iCparcodi = dr.GetOrdinal(this.Cparcodi);
            if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = Convert.ToInt32(dr.GetValue(iCparcodi));

            int iCpattusucreacion = dr.GetOrdinal(this.Cpattusucreacion);
            if (!dr.IsDBNull(iCpattusucreacion)) entity.Cpattusucreacion = dr.GetString(iCpattusucreacion);

            int iCpattfeccreacion = dr.GetOrdinal(this.Cpattfeccreacion);
            if (!dr.IsDBNull(iCpattfeccreacion)) entity.Cpattfeccreacion = dr.GetDateTime(iCpattfeccreacion);

            int iCpattusumodificacion = dr.GetOrdinal(this.Cpattusumodificacion);
            if (!dr.IsDBNull(iCpattusumodificacion)) entity.Cpattusumodificacion = dr.GetString(iCpattusumodificacion);

            int iCpattfecmodificacion = dr.GetOrdinal(this.Cpattfecmodificacion);
            if (!dr.IsDBNull(iCpattfecmodificacion)) entity.Cpattfecmodificacion = dr.GetDateTime(iCpattfecmodificacion);


            return entity;
        }

        #region Mapeo de Campos
        public string Cpattcodi = "CPATTCODI";
        public string Cpattanio = "CPATTANIO";
        public string Cpattajuste = "CPATTAJUSTE";
        public string Cparcodi = "CPARCODI";
        public string Cpattusucreacion = "CPATTUSUCREACION";
        public string Cpattfeccreacion = "CPATTFECCREACION";
        public string Cpattusumodificacion = "CPATTUSUMODIFICACION";
        public string Cpattfecmodificacion = "CPATTFECMODIFICACION";
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

        public string SqlObtenerEstadoRevisionTransmisores
        {
            get { return base.GetSqlXml("ObtenerEstadoRevisionTransmisores"); }
        }

        public string SqlObtenerNroRegistrosCPPEJTransmisores
        {
            get { return base.GetSqlXml("ObtenerNroRegistrosCPPEJTransmisores"); }
        }

        public string SqlDeleteCPPEJTransmisores
        {
            get { return base.GetSqlXml("DeleteCPPEJTransmisores"); }
        }

        public string SqlObtenerTipoEmpresaCPAPorNombre
        {
            get { return base.GetSqlXml("ObtenerTipoEmpresaCPAPorNombre"); }
        }
        #endregion
    }
}


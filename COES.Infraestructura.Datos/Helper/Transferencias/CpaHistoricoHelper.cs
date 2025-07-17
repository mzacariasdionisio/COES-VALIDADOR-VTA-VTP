using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_HISTORICO
    /// </summary>
    public class CpaHistoricoHelper : HelperBase
    {
        public CpaHistoricoHelper() : base(Consultas.CpaHistoricoSql)
        {
        }

        public CpaHistoricoDTO Create(IDataReader dr)
        {
            CpaHistoricoDTO entity = new CpaHistoricoDTO();

            int iCpahcodi = dr.GetOrdinal(Cpahcodi);
            if (!dr.IsDBNull(iCpahcodi)) entity.Cpahcodi = dr.GetInt32(iCpahcodi);

            int iCparcodi = dr.GetOrdinal(Cparcodi);
            if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

            int iCpahtipo = dr.GetOrdinal(Cpahtipo);
            if (!dr.IsDBNull(iCpahtipo)) entity.Cpahtipo = dr.GetString(iCpahtipo);

            int iCpahusumodificacion = dr.GetOrdinal(Cpahusumodificacion);
            if (!dr.IsDBNull(iCpahusumodificacion)) entity.Cpahusumodificacion = dr.GetString(iCpahusumodificacion);

            int iCpahfecmodificacion = dr.GetOrdinal(Cpahfecmodificacion);
            if (!dr.IsDBNull(iCpahfecmodificacion)) entity.Cpahfecmodificacion = dr.GetDateTime(iCpahfecmodificacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Cpahcodi = "CPAHCODI";
        public string Cparcodi = "CPARCODI";
        public string Cpahtipo = "CPAHTIPO";
        public string Cpahusumodificacion = "CPAHUSUMODIFICACION";
        public string Cpahfecmodificacion = "CPAHFECMODIFICACION";
        #endregion
    }
}

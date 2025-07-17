using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PMO_QN_CONFENV
    /// </summary>
    public class PmoQnConfenvHelper : HelperBase
    {
        public PmoQnConfenvHelper() : base(Consultas.PmoQnConfenvSql)
        {
        }

        public PmoQnConfenvDTO Create(IDataReader dr)
        {
            PmoQnConfenvDTO entity = new PmoQnConfenvDTO();

            int iQncfgecodi = dr.GetOrdinal(this.Qncfgecodi);
            if (!dr.IsDBNull(iQncfgecodi)) entity.Qncfgecodi = Convert.ToInt32(dr.GetValue(iQncfgecodi));

            int iQnlectcodi = dr.GetOrdinal(this.Qnlectcodi);
            if (!dr.IsDBNull(iQnlectcodi)) entity.Qnlectcodi = Convert.ToInt32(dr.GetValue(iQnlectcodi));

            int iQncfgesddps = dr.GetOrdinal(this.Qncfgesddps);
            if (!dr.IsDBNull(iQncfgesddps)) entity.Qncfgesddps = dr.GetString(iQncfgesddps);

            int iQncfgeusucreacion = dr.GetOrdinal(this.Qncfgeusucreacion);
            if (!dr.IsDBNull(iQncfgeusucreacion)) entity.Qncfgeusucreacion = dr.GetString(iQncfgeusucreacion);

            int iQncfgefeccreacion = dr.GetOrdinal(this.Qncfgefeccreacion);
            if (!dr.IsDBNull(iQncfgefeccreacion)) entity.Qncfgefeccreacion = dr.GetDateTime(iQncfgefeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Qncfgecodi = "QNCFGECODI";
        public string Qnlectcodi = "QNLECTCODI";
        public string Qncfgesddps = "QNCFGESDDPS";
        public string Qncfgeusucreacion = "QNCFGEUSUCREACION";
        public string Qncfgefeccreacion = "QNCFGEFECCREACION";

        #endregion
    }
}

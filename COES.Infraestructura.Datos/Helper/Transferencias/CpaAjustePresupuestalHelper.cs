using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_AJUSTEPRESUPUESTAL
    /// </summary>
    public class CpaAjustePresupuestalHelper : HelperBase
    {
        public CpaAjustePresupuestalHelper() : base(Consultas.CpaAjustePresupuestalSql)
        {
        }

        public CpaAjustePresupuestalDTO Create(IDataReader dr)
        {
            CpaAjustePresupuestalDTO entity = new CpaAjustePresupuestalDTO();

            int iCpaapcodi = dr.GetOrdinal(Cpaapcodi);
            if (!dr.IsDBNull(iCpaapcodi)) entity.Cpaapcodi = dr.GetInt32(iCpaapcodi);

            int iCpaapanio = dr.GetOrdinal(Cpaapanio);
            if (!dr.IsDBNull(iCpaapanio)) entity.Cpaapanio = dr.GetInt32(iCpaapanio);

            int iCpaapajuste = dr.GetOrdinal(Cpaapajuste);
            if (!dr.IsDBNull(iCpaapajuste)) entity.Cpaapajuste = dr.GetString(iCpaapajuste);

            int iCpaapanioejercicio = dr.GetOrdinal(Cpaapanioejercicio);
            if (!dr.IsDBNull(iCpaapanioejercicio)) entity.Cpaapanioejercicio = dr.GetInt32(iCpaapanioejercicio);

            int iCpaapusucreacion = dr.GetOrdinal(Cpaapusucreacion);
            if (!dr.IsDBNull(iCpaapusucreacion)) entity.Cpaapusucreacion = dr.GetString(iCpaapusucreacion);

            int iCpaapfeccreacion = dr.GetOrdinal(Cpaapfeccreacion);
            if (!dr.IsDBNull(iCpaapfeccreacion)) entity.Cpaapfeccreacion = dr.GetDateTime(iCpaapfeccreacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Cpaapcodi = "CPAAPCODI";
        public string Cpaapanio = "CPAAPANIO";
        public string Cpaapajuste = "CPAAPAJUSTE";
        public string Cpaapanioejercicio = "CPAAPANIOEJERCICIO";
        public string Cpaapusucreacion = "CPAAPUSUCREACION";
        public string Cpaapfeccreacion = "CPAAPFECCREACION";
        #endregion
    }
}


using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_CALCULO_CENTRAL
    /// </summary>
    public class CpaCalculoCentralHelper : HelperBase
    {
        #region Mapeo de Campos
        public string Cpacccodi = "CPACCCODI";
        public string Cpacecodi = "CPACECODI";
        public string Cpaccodi = "CPACCODI";
        public string Cparcodi = "CPARCODI";
        public string Equicodi = "EQUICODI";
        public string Barrcodi = "BARRCODI";
        public string Cpacctotenemwh = "CPACCTOTENEMWH";
        public string Cpacctotenesoles = "CPACCTOTENESOLES";
        public string Cpacctotpotmwh = "CPACCTOTPOTMWH";
        public string Cpacctotpotsoles = "CPACCTOTPOTSOLES";
        public string Cpaccusucreacion = "CPACCUSUCREACION";
        public string Cpaccfeccreacion = "CPACCFECCREACION";
        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "EQUINOMB";
        public string BarrBarraTransferencia = "BARRBARRATRANSFERENCIA";
        #endregion

        public CpaCalculoCentralHelper() : base(Consultas.CpaCalculoCentralSql)
        {
        }

        public CpaCalculoCentralDTO Create(IDataReader dr)
        {
            CpaCalculoCentralDTO entity = new CpaCalculoCentralDTO();

            int iCpacccodi = dr.GetOrdinal(Cpacccodi);
            if (!dr.IsDBNull(iCpacccodi)) entity.Cpacccodi = dr.GetInt32(iCpacccodi);

            int iCpacecodi = dr.GetOrdinal(Cpacecodi);
            if (!dr.IsDBNull(iCpacecodi)) entity.Cpacecodi = dr.GetInt32(iCpacecodi);

            int iCpaccodi = dr.GetOrdinal(Cpaccodi);
            if (!dr.IsDBNull(iCpaccodi)) entity.Cpaccodi = dr.GetInt32(iCpaccodi);

            int iCparcodi = dr.GetOrdinal(Cparcodi);
            if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

            int iEquicodi = dr.GetOrdinal(Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

            int iBarrcodi = dr.GetOrdinal(Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = dr.GetInt32(iBarrcodi);

            int iCpacctotenemwh = dr.GetOrdinal(Cpacctotenemwh);
            if (!dr.IsDBNull(iCpacctotenemwh)) entity.Cpacctotenemwh = dr.GetDecimal(iCpacctotenemwh);

            int iCpacctotenesoles = dr.GetOrdinal(Cpacctotenesoles);
            if (!dr.IsDBNull(iCpacctotenesoles)) entity.Cpacctotenesoles = dr.GetDecimal(iCpacctotenesoles);

            int iCpacctotpotmwh = dr.GetOrdinal(Cpacctotpotmwh);
            if (!dr.IsDBNull(iCpacctotpotmwh)) entity.Cpacctotpotmwh = dr.GetDecimal(iCpacctotpotmwh);

            int iCpacctotpotsoles = dr.GetOrdinal(Cpacctotpotsoles);
            if (!dr.IsDBNull(iCpacctotpotsoles)) entity.Cpacctotpotsoles = dr.GetDecimal(iCpacctotpotsoles);

            int iCpaccusucreacion = dr.GetOrdinal(Cpaccusucreacion);
            if (!dr.IsDBNull(iCpaccusucreacion)) entity.Cpaccusucreacion = dr.GetString(iCpaccusucreacion);

            int iCpaccfeccreacion = dr.GetOrdinal(Cpaccfeccreacion);
            if (!dr.IsDBNull(iCpaccfeccreacion)) entity.Cpaccfeccreacion = dr.GetDateTime(iCpaccfeccreacion);

            return entity;
        }

        public string SqlDeleteByRevision
        {
            get { return base.GetSqlXml("DeleteByRevision"); }
        }
    }
}

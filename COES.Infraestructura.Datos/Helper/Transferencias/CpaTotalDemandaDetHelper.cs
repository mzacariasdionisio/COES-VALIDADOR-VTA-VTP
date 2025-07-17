using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_TOTAL_DEMANDADET
    /// </summary>
    public class CpaTotalDemandaDetHelper : HelperBase
    {
        public CpaTotalDemandaDetHelper() : base(Consultas.CpaTotalDemandaDetSql)
        {
        }

        #region Metodos
        public CpaTotalDemandaDetDTO Create(IDataReader dr)
        {
            CpaTotalDemandaDetDTO entity = new CpaTotalDemandaDetDTO();


            int iCpatddcodi = dr.GetOrdinal(this.Cpatddcodi);
            if (!dr.IsDBNull(iCpatddcodi)) entity.Cpatddcodi = Convert.ToInt32(dr.GetValue(iCpatddcodi));

            int iCpatdcodi = dr.GetOrdinal(this.Cpatdcodi);
            if (!dr.IsDBNull(iCpatdcodi)) entity.Cpatdcodi = Convert.ToInt32(dr.GetValue(iCpatdcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iCpatddtotenemwh = dr.GetOrdinal(this.Cpatddtotenemwh);
            if (!dr.IsDBNull(iCpatddtotenemwh)) entity.Cpatddtotenemwh = dr.GetDecimal(iCpatddtotenemwh);

            int iCpatddtotenesoles = dr.GetOrdinal(this.Cpatddtotenesoles);
            if (!dr.IsDBNull(iCpatddtotenesoles)) entity.Cpatddtotenesoles = dr.GetDecimal(iCpatddtotenesoles);

            int iCpatddtotpotmw = dr.GetOrdinal(this.Cpatddtotpotmw);
            if (!dr.IsDBNull(iCpatddtotpotmw)) entity.Cpatddtotpotmw = dr.GetDecimal(iCpatddtotpotmw);

            int iCpatddtotpotsoles = dr.GetOrdinal(this.Cpatddtotpotsoles);
            if (!dr.IsDBNull(iCpatddtotpotsoles)) entity.Cpatddtotpotsoles = dr.GetDecimal(iCpatddtotpotsoles);

            int iCpatddusucreacion = dr.GetOrdinal(this.Cpatddusucreacion);
            if (!dr.IsDBNull(iCpatddusucreacion)) entity.Cpatddusucreacion = dr.GetString(iCpatddusucreacion);

            int iCpatddfeccreacion = dr.GetOrdinal(this.Cpatddfeccreacion);
            if (!dr.IsDBNull(iCpatddfeccreacion)) entity.Cpatddfeccreacion = dr.GetDateTime(iCpatddfeccreacion);

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);


            return entity;
        }

        #endregion

        #region Mapeo de Campos
        public string Cpatddcodi = "CPATDDCODI";
        public string Cpatdcodi = "CPATDCODI";
        public string Emprcodi = "EMPRCODI";
        public string Cpatddtotenemwh = "CPATDDTOTENEMWH";
        public string Cpatddtotenesoles = "CPATDDTOTENESOLES";
        public string Cpatddtotpotmw = "CPATDDTOTPOTMW";
        public string Cpatddtotpotsoles = "CPATDDTOTPOTSOLES";
        public string Cpatddusucreacion = "CPATDDUSUCREACION";
        public string Cpatddfeccreacion = "CPATDDFECCREACION";
        public string Emprnomb = "EMPRNOMB";

        /* CU17: INICIO */
        public string Cparcodi = "CPARCODI";
        public string Cpatdtipo = "CPATDTIPO";
        public string Cpatdmes = "CPATDMES";
        public string Cpatdestado = "CPATDESTADO";
        /* CU17: FIN */
        #endregion

        #region Querys
        public string SqlGetByIdDemanda
        {
            get { return base.GetSqlXml("GetByIdDemanda"); }
        }

        public string SqlFilter
        {
            get { return base.GetSqlXml("Filter"); }
        }

        public string SqlGetLastEnvio
        {
            get { return base.GetSqlXml("GetLastEnvio"); }
        }

        public string SqlGetFirstEnvio
        {
            get { return base.GetSqlXml("GetFirstEnvio"); }
        }

        public string SqlGetEnvioVacio
        {
            get { return base.GetSqlXml("GetEnvioVacio"); }
        }

        /*CU17: INICIO */
        public string SqlListLastByRevision
        {
            get { return base.GetSqlXml("ListLastByRevision"); }
        }

        public string SqlListByCpatdcodi
        {
            get { return base.GetSqlXml("ListByCpatdcodi"); }
        }
        /*CU17: FIN */
        #endregion
    }
}


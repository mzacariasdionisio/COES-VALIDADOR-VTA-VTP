using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_TOTAL_TRANSMISORESDET
    /// </summary>
    public class CpaTotalTransmisoresDetHelper : HelperBase
    {
        public CpaTotalTransmisoresDetHelper() : base(Consultas.CpaTotalTransmisoresDetSql)
        {
        }

        public CpaTotalTransmisoresDetDTO Create(IDataReader dr)
        {
            CpaTotalTransmisoresDetDTO entity = new CpaTotalTransmisoresDetDTO();


            int iCpattdcodi = dr.GetOrdinal(this.Cpattdcodi);
            if (!dr.IsDBNull(iCpattdcodi)) entity.Cpattdcodi = Convert.ToInt32(dr.GetValue(iCpattdcodi));

            int iCpattcodi = dr.GetOrdinal(this.Cpattcodi);
            if (!dr.IsDBNull(iCpattcodi)) entity.Cpattcodi = Convert.ToInt32(dr.GetValue(iCpattcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iCpattdtotmes01 = dr.GetOrdinal(this.Cpattdtotmes01);
            if (!dr.IsDBNull(iCpattdtotmes01)) entity.Cpattdtotmes01 = dr.GetDecimal(iCpattdtotmes01);

            int iCpattdtotmes02 = dr.GetOrdinal(this.Cpattdtotmes02);
            if (!dr.IsDBNull(iCpattdtotmes02)) entity.Cpattdtotmes02 = dr.GetDecimal(iCpattdtotmes02);

            int iCpattdtotmes03 = dr.GetOrdinal(this.Cpattdtotmes03);
            if (!dr.IsDBNull(iCpattdtotmes03)) entity.Cpattdtotmes03 = dr.GetDecimal(iCpattdtotmes03);

            int iCpattdtotmes04 = dr.GetOrdinal(this.Cpattdtotmes04);
            if (!dr.IsDBNull(iCpattdtotmes04)) entity.Cpattdtotmes04 = dr.GetDecimal(iCpattdtotmes04);

            int iCpattdtotmes05 = dr.GetOrdinal(this.Cpattdtotmes05);
            if (!dr.IsDBNull(iCpattdtotmes05)) entity.Cpattdtotmes05 = dr.GetDecimal(iCpattdtotmes05);

            int iCpattdtotmes06 = dr.GetOrdinal(this.Cpattdtotmes06);
            if (!dr.IsDBNull(iCpattdtotmes06)) entity.Cpattdtotmes06 = dr.GetDecimal(iCpattdtotmes06);

            int iCpattdtotmes07 = dr.GetOrdinal(this.Cpattdtotmes07);
            if (!dr.IsDBNull(iCpattdtotmes07)) entity.Cpattdtotmes07 = dr.GetDecimal(iCpattdtotmes07);

            int iCpattdtotmes08 = dr.GetOrdinal(this.Cpattdtotmes08);
            if (!dr.IsDBNull(iCpattdtotmes08)) entity.Cpattdtotmes08 = dr.GetDecimal(iCpattdtotmes08);

            int iCpattdtotmes09 = dr.GetOrdinal(this.Cpattdtotmes09);
            if (!dr.IsDBNull(iCpattdtotmes09)) entity.Cpattdtotmes09 = dr.GetDecimal(iCpattdtotmes09);

            int iCpattdtotmes10 = dr.GetOrdinal(this.Cpattdtotmes10);
            if (!dr.IsDBNull(iCpattdtotmes10)) entity.Cpattdtotmes10 = dr.GetDecimal(iCpattdtotmes10);

            int iCpattdtotmes11 = dr.GetOrdinal(this.Cpattdtotmes11);
            if (!dr.IsDBNull(iCpattdtotmes11)) entity.Cpattdtotmes11 = dr.GetDecimal(iCpattdtotmes11);

            int iCpattdtotmes12 = dr.GetOrdinal(this.Cpattdtotmes12);
            if (!dr.IsDBNull(iCpattdtotmes12)) entity.Cpattdtotmes12 = dr.GetDecimal(iCpattdtotmes12);

            int iCpattdusucreacion = dr.GetOrdinal(this.Cpattdusucreacion);
            if (!dr.IsDBNull(iCpattdusucreacion)) entity.Cpattdusucreacion = dr.GetString(iCpattdusucreacion);

            int iCpattdfeccreacion = dr.GetOrdinal(this.Cpattdfeccreacion);
            if (!dr.IsDBNull(iCpattdfeccreacion)) entity.Cpattdfeccreacion = dr.GetDateTime(iCpattdfeccreacion);

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);


            return entity;
        }

        #region Mapeo de Campos
        public string Cpattdcodi = "CPATTDCODI";
        public string Cpattcodi = "CPATTCODI";
        public string Emprcodi = "EMPRCODI";
        public string Cpattdtotmes01 = "CPATTDTOTMES01";
        public string Cpattdtotmes02 = "CPATTDTOTMES02";
        public string Cpattdtotmes03 = "CPATTDTOTMES03";
        public string Cpattdtotmes04 = "CPATTDTOTMES04";
        public string Cpattdtotmes05 = "CPATTDTOTMES05";
        public string Cpattdtotmes06 = "CPATTDTOTMES06";
        public string Cpattdtotmes07 = "CPATTDTOTMES07";
        public string Cpattdtotmes08 = "CPATTDTOTMES08";
        public string Cpattdtotmes09 = "CPATTDTOTMES09";
        public string Cpattdtotmes10 = "CPATTDTOTMES10";
        public string Cpattdtotmes11 = "CPATTDTOTMES11";
        public string Cpattdtotmes12 = "CPATTDTOTMES12";
        public string Cpattdusucreacion = "CPATTDUSUCREACION";
        public string Cpattdfeccreacion = "CPATTDFECCREACION";
        public string Emprnomb = "EMPRNOMB";

        /* CU17: INICIO */
        //public string Emprcodi = "EMPRCODI";
        public string Cpattdtotal = "CPATTDTOTAL";
        public string Cparcodi = "CPARCODI";
        /* CU17: FIN */
        #endregion

        #region Querys
        public string SqlGetByIdTransmisores
        {
            get { return base.GetSqlXml("GetByIdTransmisores"); }
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

        /* CU17: INICIO */
        public string SqlListLastByRevision
        {
            get { return base.GetSqlXml("ListLastByRevision"); }
        }

        public string SqlListByCpattcodi
        {
            get { return base.GetSqlXml("ListByCpattcodi"); }
        }
        /* CU17: FIN */
        #endregion
    }
}


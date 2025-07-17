using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_INSUMO
    /// </summary>
    public class RerInsumoHelper : HelperBase
    {
        public RerInsumoHelper() : base(Consultas.RerInsumoSql)
        {
        }

        public RerInsumoDTO Create(IDataReader dr)
        {
            RerInsumoDTO entity = new RerInsumoDTO();

            int iRerinscodi = dr.GetOrdinal(this.Rerinscodi);
            if (!dr.IsDBNull(iRerinscodi)) entity.Rerinscodi = Convert.ToInt32(dr.GetValue(iRerinscodi));

            int iReravcodi = dr.GetOrdinal(this.Reravcodi);
            if (!dr.IsDBNull(iReravcodi)) entity.Reravcodi = Convert.ToInt32(dr.GetValue(iReravcodi));

            int iRerinstipinsumo = dr.GetOrdinal(this.Rerinstipinsumo);
            if (!dr.IsDBNull(iRerinstipinsumo)) entity.Rerinstipinsumo = dr.GetString(iRerinstipinsumo);

            int iRerinstipproceso = dr.GetOrdinal(this.Rerinstipproceso);
            if (!dr.IsDBNull(iRerinstipproceso)) entity.Rerinstipproceso = dr.GetString(iRerinstipproceso);

            int iRerinslog = dr.GetOrdinal(this.Rerinslog);
            if (!dr.IsDBNull(iRerinslog)) entity.Rerinslog = dr.GetString(iRerinslog);

            int iRerinsusucreacion = dr.GetOrdinal(this.Rerinsusucreacion);
            if (!dr.IsDBNull(iRerinsusucreacion)) entity.Rerinsusucreacion = dr.GetString(iRerinsusucreacion);

            int iRerinsfeccreacion = dr.GetOrdinal(this.Rerinsfeccreacion);
            if (!dr.IsDBNull(iRerinsfeccreacion)) entity.Rerinsfeccreacion = dr.GetDateTime(iRerinsfeccreacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Rerinscodi = "RERINSCODI";
        public string Reravcodi = "RERAVCODI";
        public string Rerinstipinsumo = "RERINSTIPINSUMO";
        public string Rerinstipproceso = "RERINSTIPPROCESO";
        public string Rerinslog = "RERINSLOG";
        public string Rerinsusucreacion = "RERINSUSUCREACION";
        public string Rerinsfeccreacion = "RERINSFECCREACION";
        #endregion

        #region Mapeo de Campos RER_INSUMO_CM_TEMP
        public string Rerfecinicio = "RERFECINICIO";
        public string Reretapa = "RERETAPA";
        public string Rerbloque = "RERBLOQUE";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Ptomedidesc = "PTOMEDIDESC";
        public string Rervalor = "RERVALOR";

        public string Perianiomes = "PMPERIANIOMES";
        #endregion

        #region Querys
        public string SqlGetByReravcodiByRerinstipinsumo
        {
            get { return base.GetSqlXml("GetByReravcodiByRerinstipinsumo"); }
        }


        public string SqlGetIdPeriodoPmpoByAnioMes
        {
            get { return base.GetSqlXml("GetIdPeriodoPmpoByAnioMes"); }
        }


        public string SqlTruncateTablaTemporal
        {
            get { return base.GetSqlXml("TruncateTablaTemporal"); }
        }

        public string SqlInsertTablaTemporal
        {
            get { return base.GetSqlXml("InsertTablaTemporal"); }
        }

        public string SqlListTablaTemporal
        {
            get { return base.GetSqlXml("ListTablaTemporal"); }
        }
        #endregion
    }
}


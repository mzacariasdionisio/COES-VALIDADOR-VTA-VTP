using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_INSUMO_MES
    /// </summary>
    public class RerInsumoMesHelper : HelperBase
    {
        public RerInsumoMesHelper() : base(Consultas.RerInsumoMesSql)
        {
        }

        public RerInsumoMesDTO Create(IDataReader dr)
        {
            RerInsumoMesDTO entity = new RerInsumoMesDTO();

            int iRerinmmescodi = dr.GetOrdinal(this.Rerinmmescodi);
            if (!dr.IsDBNull(iRerinmmescodi)) entity.Rerinmmescodi = Convert.ToInt32(dr.GetValue(iRerinmmescodi));

            int iRerinscodi = dr.GetOrdinal(this.Rerinscodi);
            if (!dr.IsDBNull(iRerinscodi)) entity.Rerinscodi = Convert.ToInt32(dr.GetValue(iRerinscodi));

            int iRerpprcodi = dr.GetOrdinal(this.Rerpprcodi);
            if (!dr.IsDBNull(iRerpprcodi)) entity.Rerpprcodi = Convert.ToInt32(dr.GetValue(iRerpprcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iRerinmanio = dr.GetOrdinal(this.Rerinmanio);
            if (!dr.IsDBNull(iRerinmanio)) entity.Rerinmanio = Convert.ToInt32(dr.GetValue(iRerinmanio));

            int iRerinmmes = dr.GetOrdinal(this.Rerinmmes);
            if (!dr.IsDBNull(iRerinmmes)) entity.Rerinmmes = Convert.ToInt32(dr.GetValue(iRerinmmes));

            int iRerinmtipresultado = dr.GetOrdinal(this.Rerinmtipresultado);
            if (!dr.IsDBNull(iRerinmtipresultado)) entity.Rerinmtipresultado = dr.GetString(iRerinmtipresultado);

            int iRerinmtipinformacion = dr.GetOrdinal(this.Rerinmtipinformacion);
            if (!dr.IsDBNull(iRerinmtipinformacion)) entity.Rerinmtipinformacion = dr.GetString(iRerinmtipinformacion);

            int iRerinmdetalle = dr.GetOrdinal(this.Rerinmdetalle);
            if (!dr.IsDBNull(iRerinmdetalle)) entity.Rerinmdetalle = dr.GetString(iRerinmdetalle);

            int iRerinmmestotal = dr.GetOrdinal(this.Rerinmmestotal);
            if (!dr.IsDBNull(iRerinmmestotal)) entity.Rerinmmestotal = dr.GetDecimal(iRerinmmestotal);

            int iRerinmmesusucreacion = dr.GetOrdinal(this.Rerinmmesusucreacion);
            if (!dr.IsDBNull(iRerinmmesusucreacion)) entity.Rerinmmesusucreacion = dr.GetString(iRerinmmesusucreacion);

            int iRerinmmesfeccreacion = dr.GetOrdinal(this.Rerinmmesfeccreacion);
            if (!dr.IsDBNull(iRerinmmesfeccreacion)) entity.Rerinmmesfeccreacion = dr.GetDateTime(iRerinmmesfeccreacion);

            return entity;
        }

        public RerInsumoMesDTO CreateByTipoResultadoByPeriodo(IDataReader dr)
        {
            RerInsumoMesDTO entity = Create(dr);

            int iEquinomb = dr.GetOrdinal(this.Equinomb);
            if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

            return entity;
        }

        #region Mapeo de Campos
        public string Rerinmmescodi = "RERINMMESCODI";
        public string Rerinscodi = "RERINSCODI";
        public string Rerpprcodi = "RERPPRCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Rerinmanio = "RERINMANIO";
        public string Rerinmmes = "RERINMMES";
        public string Rerinmtipresultado = "RERINMTIPRESULTADO";
        public string Rerinmtipinformacion = "RERINMTIPINFORMACION";
        public string Rerinmdetalle = "RERINMDETALLE";
        public string Rerinmmestotal = "RERINMMESTOTAL";
        public string Rerinmmesusucreacion = "RERINMMESUSUCREACION";
        public string Rerinmmesfeccreacion = "RERINMMESFECCREACION";

        //Atributos de consulta
        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "EQUINOMB";

        #endregion

        public string SqlDeleteByParametroPrimaAndTipo
        {
            get { return base.GetSqlXml("DeleteByParametroPrimaAndTipo"); }
        }

        public string SqlGetByAnioTarifario
        {
            get { return base.GetSqlXml("GetByAnioTarifario"); }
        }

        public string SqlGetByTipoResultadoByPeriodo
        {
            get { return base.GetSqlXml("GetByTipoResultadoByPeriodo"); }
        }
    }
}


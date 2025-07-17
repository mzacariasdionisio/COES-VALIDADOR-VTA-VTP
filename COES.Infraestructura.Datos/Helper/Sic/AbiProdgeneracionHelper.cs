using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ABI_PRODGENERACION
    /// </summary>
    public class AbiProdgeneracionHelper : HelperBase
    {
        public AbiProdgeneracionHelper() : base(Consultas.AbiProdgeneracionSql)
        {
        }

        public AbiProdgeneracionDTO Create(IDataReader dr)
        {
            AbiProdgeneracionDTO entity = new AbiProdgeneracionDTO();

            int iPgenfecmodificacion = dr.GetOrdinal(this.Pgenfecmodificacion);
            if (!dr.IsDBNull(iPgenfecmodificacion)) entity.Pgenfecmodificacion = dr.GetDateTime(iPgenfecmodificacion);

            int iPgenusumodificacion = dr.GetOrdinal(this.Pgenusumodificacion);
            if (!dr.IsDBNull(iPgenusumodificacion)) entity.Pgenusumodificacion = dr.GetString(iPgenusumodificacion);

            int iPgentipogenerrer = dr.GetOrdinal(this.Pgentipogenerrer);
            if (!dr.IsDBNull(iPgentipogenerrer)) entity.Pgentipogenerrer = dr.GetString(iPgentipogenerrer);

            int iPgenintegrante = dr.GetOrdinal(this.Pgenintegrante);
            if (!dr.IsDBNull(iPgenintegrante)) entity.Pgenintegrante = dr.GetString(iPgenintegrante);

            int iPgenvalor = dr.GetOrdinal(this.Pgenvalor);
            if (!dr.IsDBNull(iPgenvalor)) entity.Pgenvalor = dr.GetDecimal(iPgenvalor);

            int iPgenfecha = dr.GetOrdinal(this.Pgenfecha);
            if (!dr.IsDBNull(iPgenfecha)) entity.Pgenfecha = dr.GetDateTime(iPgenfecha);

            int iEquipadre = dr.GetOrdinal(this.Equipadre);
            if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iFenergcodi = dr.GetOrdinal(this.Fenergcodi);
            if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

            int iTgenercodi = dr.GetOrdinal(this.Tgenercodi);
            if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

            int iPgencodi = dr.GetOrdinal(this.Pgencodi);
            if (!dr.IsDBNull(iPgencodi)) entity.Pgencodi = Convert.ToInt32(dr.GetValue(iPgencodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Pgenfecmodificacion = "PGENFECMODIFICACION";
        public string Pgenusumodificacion = "PGENUSUMODIFICACION";
        public string Pgentipogenerrer = "PGENTIPOGENERRER";
        public string Pgenintegrante = "PGENINTEGRANTE";
        public string Pgenvalor = "PGENVALOR";
        public string Pgenfecha = "PGENFECHA";
        public string Equipadre = "EQUIPADRE";
        public string Equicodi = "EQUICODI";
        public string Grupocodi = "GRUPOCODI";
        public string Emprcodi = "EMPRCODI";
        public string Fenergcodi = "FENERGCODI";
        public string Tgenercodi = "TGENERCODI";
        public string Pgencodi = "PGENCODI";

        public string Tgenernomb = "TGENERNOMB";
        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "EQUINOMB";
        public string Fenergnomb = "FENERGNOMB";
        public string Central = "CENTRAL";

        #endregion

        public string SqlDeleteByRango
        {
            get { return base.GetSqlXml("DeleteByRango"); }
        }
    }
}

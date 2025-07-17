using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ABI_POTEFEC
    /// </summary>
    public class AbiPotefecHelper : HelperBase
    {
        public AbiPotefecHelper() : base(Consultas.AbiPotefecSql)
        {
        }

        public AbiPotefecDTO Create(IDataReader dr)
        {
            AbiPotefecDTO entity = new AbiPotefecDTO();

            int iPefecfecmodificacion = dr.GetOrdinal(this.Pefecfecmodificacion);
            if (!dr.IsDBNull(iPefecfecmodificacion)) entity.Pefecfecmodificacion = dr.GetDateTime(iPefecfecmodificacion);

            int iPefecusumodificacion = dr.GetOrdinal(this.Pefecusumodificacion);
            if (!dr.IsDBNull(iPefecusumodificacion)) entity.Pefecusumodificacion = dr.GetString(iPefecusumodificacion);

            int iPefectipogenerrer = dr.GetOrdinal(this.Pefectipogenerrer);
            if (!dr.IsDBNull(iPefectipogenerrer)) entity.Pefectipogenerrer = dr.GetString(iPefectipogenerrer);

            int iPefecintegrante = dr.GetOrdinal(this.Pefecintegrante);
            if (!dr.IsDBNull(iPefecintegrante)) entity.Pefecintegrante = dr.GetString(iPefecintegrante);

            int iPefecvalorpinst = dr.GetOrdinal(this.Pefecvalorpinst);
            if (!dr.IsDBNull(iPefecvalorpinst)) entity.Pefecvalorpinst = dr.GetDecimal(iPefecvalorpinst);

            int iPefecvalorpe = dr.GetOrdinal(this.Pefecvalorpe);
            if (!dr.IsDBNull(iPefecvalorpe)) entity.Pefecvalorpe = dr.GetDecimal(iPefecvalorpe);

            int iPefecfechames = dr.GetOrdinal(this.Pefecfechames);
            if (!dr.IsDBNull(iPefecfechames)) entity.Pefecfechames = dr.GetDateTime(iPefecfechames);

            int iEquipadre = dr.GetOrdinal(this.Equipadre);
            if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

            int iCtgdetcodi2 = dr.GetOrdinal(this.Ctgdetcodi2);
            if (!dr.IsDBNull(iCtgdetcodi2)) entity.Ctgdetcodi2 = Convert.ToInt32(dr.GetValue(iCtgdetcodi2));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iCtgdetcodi = dr.GetOrdinal(this.Ctgdetcodi);
            if (!dr.IsDBNull(iCtgdetcodi)) entity.Ctgdetcodi = Convert.ToInt32(dr.GetValue(iCtgdetcodi));

            int iTgenercodi = dr.GetOrdinal(this.Tgenercodi);
            if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

            int iFenergcodi = dr.GetOrdinal(this.Fenergcodi);
            if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

            int iPefeccodi = dr.GetOrdinal(this.Pefeccodi);
            if (!dr.IsDBNull(iPefeccodi)) entity.Pefeccodi = Convert.ToInt32(dr.GetValue(iPefeccodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Pefecfecmodificacion = "PEFECFECMODIFICACION";
        public string Pefecusumodificacion = "PEFECUSUMODIFICACION";
        public string Pefectipogenerrer = "PEFECTIPOGENERRER";
        public string Pefecintegrante = "PEFECINTEGRANTE";
        public string Pefecvalorpinst = "PEFECVALORPINST";
        public string Pefecvalorpe = "PEFECVALORPE";
        public string Pefecfechames = "PEFECFECHAMES";
        public string Equipadre = "EQUIPADRE";
        public string Ctgdetcodi2 = "CTGDETCODI2";
        public string Equicodi = "EQUICODI";
        public string Grupocodi = "GRUPOCODI";
        public string Emprcodi = "EMPRCODI";
        public string Ctgdetcodi = "CTGDETCODI";
        public string Tgenercodi = "TGENERCODI";
        public string Fenergcodi = "FENERGCODI";
        public string Pefeccodi = "PEFECCODI";

        #endregion

        public string SqlDeleteByMes
        {
            get { return base.GetSqlXml("DeleteByMes"); }
        }

        public string SqlListaPorMes
        {
            get { return base.GetSqlXml("ListPorMes"); }
        }
    }
}

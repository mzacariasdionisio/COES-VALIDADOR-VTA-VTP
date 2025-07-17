using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_HISEMPGRUPO_DATA
    /// </summary>
    public class SiHisempgrupoDataHelper : HelperBase
    {
        public SiHisempgrupoDataHelper() : base(Consultas.SiHisempgrupoDataSql)
        {
        }

        public SiHisempgrupoDataDTO Create(IDataReader dr)
        {
            SiHisempgrupoDataDTO entity = new SiHisempgrupoDataDTO();

            int iHgrdatfecha = dr.GetOrdinal(this.Hgrdatfecha);
            if (!dr.IsDBNull(iHgrdatfecha)) entity.Hgrdatfecha = dr.GetDateTime(iHgrdatfecha);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iGrupocodiold = dr.GetOrdinal(this.Grupocodiold);
            if (!dr.IsDBNull(iGrupocodiold)) entity.Grupocodiold = Convert.ToInt32(dr.GetValue(iGrupocodiold));

            int iGrupocodiactual = dr.GetOrdinal(this.Grupocodiactual);
            if (!dr.IsDBNull(iGrupocodiactual)) entity.Grupocodiactual = Convert.ToInt32(dr.GetValue(iGrupocodiactual));

            int iHgrdatestado = dr.GetOrdinal(this.Hgrdatestado);
            if (!dr.IsDBNull(iHgrdatestado)) entity.Hgrdatestado = dr.GetString(iHgrdatestado);

            int iHgrdatcodi = dr.GetOrdinal(this.Hgrdatcodi);
            if (!dr.IsDBNull(iHgrdatcodi)) entity.Hgrdatcodi = Convert.ToInt32(dr.GetValue(iHgrdatcodi));

            int iHgrdatusucreacion = dr.GetOrdinal(this.Hgrdatusucreacion);
            if (!dr.IsDBNull(iHgrdatusucreacion)) entity.Hgrdatusucreacion = dr.GetString(iHgrdatusucreacion);

            int iHgrdatfeccreacion = dr.GetOrdinal(this.Hgrdatfeccreacion);
            if (!dr.IsDBNull(iHgrdatfeccreacion)) entity.Hgrdatfeccreacion = dr.GetDateTime(iHgrdatfeccreacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Hgrdatfecha = "HGRDATFECHA";
        public string Grupocodi = "GRUPOCODI";
        public string Emprcodi = "EMPRCODI";
        public string Grupocodiold = "GRUPOCODIOLD";
        public string Grupocodiactual = "GRUPOCODIACTUAL";
        public string Hgrdatestado = "HGRDATESTADO";
        public string Hgrdatcodi = "HGRDATCODI";
        public string Hgrdatusucreacion = "HGRDATUSUCREACION";
        public string Hgrdatfeccreacion = "HGRDATFECCREACION";

        public string Gruponomb = "GRUPONOMB";
        public string Emprnomb = "EMPRNOMB";

        #endregion

        //anular trasferencia
        public string SqlDeleteXAnulacionMigra
        {
            get { return base.GetSqlXml("DeleteXAnulacionMigra"); }
        }
        public string SqlUpdateGrupoActual
        {
            get { return base.GetSqlXml("UpdateGrupoActual"); }
        }
    }
}
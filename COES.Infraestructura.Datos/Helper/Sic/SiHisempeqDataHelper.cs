using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_HISEMPEQ_DATA
    /// </summary>
    public class SiHisempeqDataHelper : HelperBase
    {
        public SiHisempeqDataHelper() : base(Consultas.SiHisempeqDataSql)
        {
        }

        public SiHisempeqDataDTO Create(IDataReader dr)
        {
            SiHisempeqDataDTO entity = new SiHisempeqDataDTO();

            int iHeqdatfecha = dr.GetOrdinal(this.Heqdatfecha);
            if (!dr.IsDBNull(iHeqdatfecha)) entity.Heqdatfecha = dr.GetDateTime(iHeqdatfecha);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodiold = dr.GetOrdinal(this.Equicodiold);
            if (!dr.IsDBNull(iEquicodiold)) entity.Equicodiold = Convert.ToInt32(dr.GetValue(iEquicodiold));

            int iEquicodiactual = dr.GetOrdinal(this.Equicodiactual);
            if (!dr.IsDBNull(iEquicodiactual)) entity.Equicodiactual = Convert.ToInt32(dr.GetValue(iEquicodiactual));

            int iHeqdatestado = dr.GetOrdinal(this.Heqdatestado);
            if (!dr.IsDBNull(iHeqdatestado)) entity.Heqdatestado = dr.GetString(iHeqdatestado);

            int iHeqdatcodi = dr.GetOrdinal(this.Heqdatcodi);
            if (!dr.IsDBNull(iHeqdatcodi)) entity.Heqdatcodi = Convert.ToInt32(dr.GetValue(iHeqdatcodi));

            int iHeqdatusucreacion = dr.GetOrdinal(this.Heqdatusucreacion);
            if (!dr.IsDBNull(iHeqdatusucreacion)) entity.Heqdatusucreacion = dr.GetString(iHeqdatusucreacion);

            int iHeqdatfeccreacion = dr.GetOrdinal(this.Heqdatfeccreacion);
            if (!dr.IsDBNull(iHeqdatfeccreacion)) entity.Heqdatfeccreacion = dr.GetDateTime(iHeqdatfeccreacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Heqdatfecha = "HEQDATFECHA";
        public string Equicodi = "EQUICODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodiold = "EQUICODIOLD";
        public string Equicodiactual = "EQUICODIACTUAL";
        public string Heqdatestado = "HEQDATESTADO";
        public string Heqdatcodi = "HEQDATCODI";
        public string Heqdatusucreacion = "HEQDATUSUCREACION";
        public string Heqdatfeccreacion = "HEQDATFECCREACION";
        public string Equinomb = "EQUINOMB";
        public string Emprnomb = "EMPRNOMB";

        #endregion

        //anular trasferencia
        public string SqlDeleteXAnulacionMigra
        {
            get { return base.GetSqlXml("DeleteXAnulacionMigra"); }
        }

        public string SqlUpdateEquipoActual
        {
            get { return base.GetSqlXml("UpdateEquipoActual"); }
        }
    }
}

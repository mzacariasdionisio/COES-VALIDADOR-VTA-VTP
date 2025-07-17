using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_HISEMPPTO_DATA
    /// </summary>
    public class SiHisempptoDataHelper : HelperBase
    {
        public SiHisempptoDataHelper() : base(Consultas.SiHisempptoDataSql)
        {
        }

        public SiHisempptoDataDTO Create(IDataReader dr)
        {
            SiHisempptoDataDTO entity = new SiHisempptoDataDTO();

            int iHptdatfecha = dr.GetOrdinal(this.Hptdatfecha);
            if (!dr.IsDBNull(iHptdatfecha)) entity.Hptdatfecha = dr.GetDateTime(iHptdatfecha);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iHptdatptoestado = dr.GetOrdinal(this.Hptdatptoestado);
            if (!dr.IsDBNull(iHptdatptoestado)) entity.Hptdatptoestado = dr.GetString(iHptdatptoestado);

            int iPtomedicodiold = dr.GetOrdinal(this.Ptomedicodiold);
            if (!dr.IsDBNull(iPtomedicodiold)) entity.Ptomedicodiold = Convert.ToInt32(dr.GetValue(iPtomedicodiold));

            int iPtomedicodiactual = dr.GetOrdinal(this.Ptomedicodiactual);
            if (!dr.IsDBNull(iPtomedicodiactual)) entity.Ptomedicodiactual = Convert.ToInt32(dr.GetValue(iPtomedicodiactual));

            int iHptdatcodi = dr.GetOrdinal(this.Hptdatcodi);
            if (!dr.IsDBNull(iHptdatcodi)) entity.Hptdatcodi = Convert.ToInt32(dr.GetValue(iHptdatcodi));

            int iHptdatusucreacion = dr.GetOrdinal(this.Hptdatusucreacion);
            if (!dr.IsDBNull(iHptdatusucreacion)) entity.Hptdatusucreacion = dr.GetString(iHptdatusucreacion);

            int iHptdatfeccreacion = dr.GetOrdinal(this.Hptdatfeccreacion);
            if (!dr.IsDBNull(iHptdatfeccreacion)) entity.Hptdatfeccreacion = dr.GetDateTime(iHptdatfeccreacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Hptdatfecha = "HPTDATFECHA";
        public string Emprcodi = "EMPRCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Hptdatptoestado = "HPTDATPTOESTADO";
        public string Ptomedicodiold = "PTOMEDICODIOLD";
        public string Ptomedicodiactual = "PTOMEDICODIACTUAL";
        public string Hptdatcodi = "HPTDATCODI";
        public string Hptdatusucreacion = "HPTDATUSUCREACION";
        public string Hptdatfeccreacion = "HPTDATFECCREACION";

        public string Ptomedidesc = "PTOMEDIDESC";
        public string Emprnomb = "EMPRNOMB";
        #endregion

        //anular trasferencia
        public string SqlDeleteXAnulacionMigra
        {
            get { return base.GetSqlXml("DeleteXAnulacionMigra"); }
        }

        public string SqlUpdatePuntoActual
        {
            get { return base.GetSqlXml("UpdatePuntoActual"); }
        }

    }
}
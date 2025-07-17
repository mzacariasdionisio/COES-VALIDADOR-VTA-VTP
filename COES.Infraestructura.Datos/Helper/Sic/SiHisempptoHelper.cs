using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_HISEMPPTO
    /// </summary>
    public class SiHisempptoHelper : HelperBase
    {
        public SiHisempptoHelper() : base(Consultas.SiHisempptoSql)
        {
        }

        public SiHisempptoDTO Create(IDataReader dr)
        {
            SiHisempptoDTO entity = new SiHisempptoDTO();

            int iHempptcodi = dr.GetOrdinal(this.Hempptcodi);
            if (!dr.IsDBNull(iHempptcodi)) entity.Hempptcodi = Convert.ToInt32(dr.GetValue(iHempptcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iHempptfecha = dr.GetOrdinal(this.Hempptfecha);
            if (!dr.IsDBNull(iHempptfecha)) entity.Hempptfecha = dr.GetDateTime(iHempptfecha);

            int iMigracodi = dr.GetOrdinal(this.Migracodi);
            if (!dr.IsDBNull(iMigracodi)) entity.Migracodi = Convert.ToInt32(dr.GetValue(iMigracodi));

            int iPtomedicodiold = dr.GetOrdinal(this.Ptomedicodiold);
            if (!dr.IsDBNull(iPtomedicodiold)) entity.Ptomedicodiold = Convert.ToInt32(dr.GetValue(iPtomedicodiold));

            int iHempptestado = dr.GetOrdinal(this.Hempptestado);
            if (!dr.IsDBNull(iHempptestado)) entity.Hempptestado = dr.GetString(iHempptestado);

            int iHempptdeleted = dr.GetOrdinal(this.Hempptdeleted);
            if (!dr.IsDBNull(iHempptdeleted)) entity.Hempptdeleted = Convert.ToInt32(dr.GetValue(iHempptdeleted));

            return entity;
        }

        #region Mapeo de Campos

        public string Hempptcodi = "HEMPPTCODI";
        public string Emprcodi = "EMPRCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Hempptfecha = "HEMPPTFECHA";
        public string Migracodi = "MIGRACODI";
        public string Ptomedicodiold = "PTOMEDICODIOLD";
        public string Hempptestado = "HEMPPTESTADO";
        public string Hempptdeleted = "HEMPPTDELETED";
        public string Ptomedidesc = "PTOMEDIDESC";

        #endregion

        public string SqlDeleteLogico
        {
            get { return base.GetSqlXml("DeleteLogico"); }
        }

        public string SqlListPtsXMigracion
        {
            get { return base.GetSqlXml("ListPtsXMigracion"); }
        }

        public string SqlupdateAnular
        {
            get { return base.GetSqlXml("UpdateAnular"); }
        }

        public string SqlConsultarPtosMigracion
        {
            get { return base.GetSqlXml("ConsultarPtosMigracion"); }
        }
    }
}
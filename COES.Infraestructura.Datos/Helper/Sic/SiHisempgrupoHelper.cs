using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_HISEMPGRUPO
    /// </summary>
    public class SiHisempgrupoHelper : HelperBase
    {
        public SiHisempgrupoHelper() : base(Consultas.SiHisempgrupoSql)
        {
        }

        public SiHisempgrupoDTO Create(IDataReader dr)
        {
            SiHisempgrupoDTO entity = new SiHisempgrupoDTO();

            int iGrupocodiold = dr.GetOrdinal(this.Grupocodiold);
            if (!dr.IsDBNull(iGrupocodiold)) entity.Grupocodiold = Convert.ToInt32(dr.GetValue(iGrupocodiold));

            int iHempgrcodi = dr.GetOrdinal(this.Hempgrcodi);
            if (!dr.IsDBNull(iHempgrcodi)) entity.Hempgrcodi = Convert.ToInt32(dr.GetValue(iHempgrcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iMigracodi = dr.GetOrdinal(this.Migracodi);
            if (!dr.IsDBNull(iMigracodi)) entity.Migracodi = Convert.ToInt32(dr.GetValue(iMigracodi));

            int iHempgrfecha = dr.GetOrdinal(this.Hempgrfecha);
            if (!dr.IsDBNull(iHempgrfecha)) entity.Hempgrfecha = dr.GetDateTime(iHempgrfecha);

            int iHempgrestado = dr.GetOrdinal(this.Hempgrestado);
            if (!dr.IsDBNull(iHempgrestado)) entity.Hempgrestado = dr.GetString(iHempgrestado);

            int iHempgrdeleted = dr.GetOrdinal(this.Hempgrdeleted);
            if (!dr.IsDBNull(iHempgrdeleted)) entity.Hempgrdeleted = Convert.ToInt32(dr.GetValue(iHempgrdeleted));

            return entity;
        }

        #region Mapeo de Campos

        public string Grupocodiold = "GRUPOCODIOLD";
        public string Hempgrcodi = "HEMPGRCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Emprcodi = "EMPRCODI";
        public string Migracodi = "MIGRACODI";
        public string Hempgrfecha = "HEMPGRFECHA";
        public string Hempgrestado = "HEMPGRESTADO";
        public string Hempgrdeleted = "HEMPGRDELETED";
        public string Gruponomb = "GRUPONOMB";

        public string SqlDeleteLogico
        {
            get { return base.GetSqlXml("DeleteLogico"); }
        }
        #endregion

        public string SqlupdateAnular
        {
            get { return base.GetSqlXml("UpdateAnular"); }
        }

        public string SqlListGrupsXMigracion
        {
            get { return base.GetSqlXml("ListGrupsXMigracion"); }
        }
        public string SqlConsultarGrpsMigracion
        {
            get { return base.GetSqlXml("ConsultarGrpsMigracion"); }
        }
    }
}
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_MIGRAEMPRORIGEN
    /// </summary>
    public class SiMigraemprorigenHelper : HelperBase
    {
        public SiMigraemprorigenHelper()
            : base(Consultas.SiMigraemprorigenSql)
        {
        }

        public SiMigraemprOrigenDTO Create(IDataReader dr)
        {
            SiMigraemprOrigenDTO entity = new SiMigraemprOrigenDTO();

            int iMigempcodi = dr.GetOrdinal(this.Migempcodi);
            if (!dr.IsDBNull(iMigempcodi)) entity.Migempcodi = Convert.ToInt32(dr.GetValue(iMigempcodi));

            int iMigracodi = dr.GetOrdinal(this.Migracodi);
            if (!dr.IsDBNull(iMigracodi)) entity.Migracodi = Convert.ToInt32(dr.GetValue(iMigracodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iMigempusucreacion = dr.GetOrdinal(this.Migempusucreacion);
            if (!dr.IsDBNull(iMigempusucreacion)) entity.Migempusucreacion = dr.GetString(iMigempusucreacion);

            int iMigempfeccreacion = dr.GetOrdinal(this.Migempfeccreacion);
            if (!dr.IsDBNull(iMigempfeccreacion)) entity.Migempfeccreacion = dr.GetDateTime(iMigempfeccreacion);

            int iMigempusumodificacion = dr.GetOrdinal(this.Migempusumodificacion);
            if (!dr.IsDBNull(iMigempusumodificacion)) entity.Migempusumodificacion = dr.GetString(iMigempusumodificacion);

            int iMigempfecmodificacion = dr.GetOrdinal(this.Migempfecmodificacion);
            if (!dr.IsDBNull(iMigempfecmodificacion)) entity.Migempfecmodificacion = dr.GetDateTime(iMigempfecmodificacion);

            int iMigempestadoOrig = dr.GetOrdinal(this.MigempestadoOrig);
            if (!dr.IsDBNull(iMigempestadoOrig)) entity.MigempestadoDest = dr.GetString(iMigempestadoOrig);

            return entity;
        }


        #region Mapeo de Campos

        public string Migempcodi = "MIGEMPCODI";
        public string Migracodi = "MIGRACODI";
        public string Emprcodi = "EMPRCODI";
        public string Migempusucreacion = "MIGEMPUSUCREACION";
        public string Migempfeccreacion = "MIGEMPFECCREACION";
        public string Migempusumodificacion = "MIGEMPUSUMODIFICACION";
        public string Migempfecmodificacion = "MIGEMPFECMODIFICACION";

        public string MigempestadoOrig = "MIGEMPESTADOORIG";
        public string MigempcodosinergminDest = "MigempcodosinergminDest";
        public string MigempscadacodiDest = "MigempscadacodiDest";
        public string MigempnombrecomercialDest = "MigempnombrecomercialDest";
        public string MigempdomiciliolegalDest = "MigempdomiciliolegalDest";
        public string MigempnumpartidaregDest = "MigempnumpartidaregDest";
        public string MigempabrevDest = "MigempabrevDest";
        public string MigempordenDest = "MigempordenDest";
        public string MigemptelefonoDest = "MigemptelefonoDest";
        public string MigempestadoregistroDest = "MigempestadoregistroDest";
        public string MigempfecinscripcionDest = "MigempfecinscripcionDest";
        public string MigempcondicionDest = "MigempcondicionDest";
        public string MigempnroconstanciaDest = "MigempnroconstanciaDest";
        public string MigempfecingresoDest = "MigempfecingresoDest";
        public string MigempnroregistroDest = "MigempnroregistroDest";
        public string MigempindusutramiteDest = "MigempindusutramiteDest";

        public string EMPRESANOMBREDESTINO = "EMPRESANOMBREDESTINO";
        public string EMPRESANOMBREORIGEN = "EMPRESANOMBREORIGEN";
        #endregion



        public string SqlListadoTransferenciaXEmprOrigenXEmprDestino
        {
            get { return base.GetSqlXml("SqlListadoTransferenciaXEmprOrigenXEmprDestino"); }
        }

    }
}

using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_MIGRACION
    /// </summary>
    public class SiMigracionHelper : HelperBase
    {
        public SiMigracionHelper()
            : base(Consultas.SiMigracionSql)
        {
        }

        public SiMigracionDTO Create(IDataReader dr)
        {
            SiMigracionDTO entity = new SiMigracionDTO();

            int iMigracodi = dr.GetOrdinal(this.Migracodi);
            if (!dr.IsDBNull(iMigracodi)) entity.Migracodi = Convert.ToInt32(dr.GetValue(iMigracodi));


            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iTmopercodi = dr.GetOrdinal(this.Tmopercodi);
            if (!dr.IsDBNull(iTmopercodi)) entity.Tmopercodi = Convert.ToInt32(dr.GetValue(iTmopercodi));

            int iMigradescripcion = dr.GetOrdinal(this.Migradescripcion);
            if (!dr.IsDBNull(iMigradescripcion)) entity.Migradescripcion = dr.GetString(iMigradescripcion);

            int iMigrafeccorte = dr.GetOrdinal(this.Migrafeccorte);
            if (!dr.IsDBNull(iMigrafeccorte)) entity.Migrafeccorte = dr.GetDateTime(iMigrafeccorte);

            int iMigrausucreacion = dr.GetOrdinal(this.Migrausucreacion);
            if (!dr.IsDBNull(iMigrausucreacion)) entity.Migrausucreacion = dr.GetString(iMigrausucreacion);

            int iMigrafeccreacion = dr.GetOrdinal(this.Migrafeccreacion);
            if (!dr.IsDBNull(iMigrafeccreacion)) entity.Migrafeccreacion = dr.GetDateTime(iMigrafeccreacion);

            int iMigrausumodificacion = dr.GetOrdinal(this.Migrausumodificacion);
            if (!dr.IsDBNull(iMigrausumodificacion)) entity.Migrausumodificacion = dr.GetString(iMigrausumodificacion);

            int iMigrafecmodificacion = dr.GetOrdinal(this.Migrafecmodificacion);
            if (!dr.IsDBNull(iMigrafecmodificacion)) entity.Migrafecmodificacion = dr.GetDateTime(iMigrafecmodificacion);

            int iMigradeleted = dr.GetOrdinal(this.Migradeleted);
            if (!dr.IsDBNull(iMigradeleted)) entity.Migradeleted = Convert.ToInt32(dr.GetValue(iMigradeleted));

            int iMigrareldeleted = dr.GetOrdinal(this.Migrareldeleted);
            if (!dr.IsDBNull(iMigrareldeleted)) entity.Migrareldeleted = Convert.ToInt32(dr.GetValue(iMigrareldeleted));

            int iMigraflagstr = dr.GetOrdinal(this.Migraflagstr);
            if (!dr.IsDBNull(iMigraflagstr)) entity.Migraflagstr = Convert.ToInt32(dr.GetValue(iMigraflagstr));

            return entity;
        }

        #region Mapeo de Campos

        public string Migracodi = "MIGRACODI";
        //public string Emprcodidestino = "EMPRCODIDESTINO";
        public string Emprcodi = "EMPRCODI";

        public string Tmopercodi = "TMOPERCODI";
        public string Migradescripcion = "MIGRADESCRIPCION";
        public string Migrafeccorte = "MIGRAFECCORTE";
        public string MigrafeccorteSTR = "MIGRAFECCORTESTR";
        public string Migrausucreacion = "MIGRAUSUCREACION";
        public string Migrafeccreacion = "MIGRAFECCREACION";
        public string Migrausumodificacion = "MIGRAUSUMODIFICACION";
        public string Migrafecmodificacion = "MIGRAFECMODIFICACION";
        public string Migradeleted = "MIGRADELETED";
        public string Migrareldeleted = "MIGRARELDELETED";
        public string Migraflagstr = "MIGRAFLAGSTR";

        //Datos de la empresa:ORIGEN Y DESTINO
        public string Emprnomborigen = "Emprnomborigen";
        public string Emprnombdestino = "Emprnombdestino";
        public string Emprestadoorigen = "Emprestadoorigen";
        public string Emprabrevorigen = "Emprabrevorigen";

        //DATOS DE TIPO MIGRACION OPERACION
        public string Tmoperdescripcion = "TMOPERDESCRIPCION";

        public string Total = "TOTAL";

        #endregion

        public string SqlListarTransferenciasXEmpresaOrigenXEmpresaDestino
        {
            get { return base.GetSqlXml("ListarTransferenciasXEmpresaOrigenXEmpresaDestino"); }
        }

        public string SqlListarHistoricoEstadoEmpresa
        {
            get { return base.GetSqlXml("ListarHistoricoEstadoEmpresa"); }
        }

        #region siosein2
        public string SqlListarTransferenciasXTipoMigracion
        {
            get { return base.GetSqlXml("ListarTransferenciasXTipoMigracion"); }
        }
        #endregion

        public string SqlUpdateMigraAnulacion
        {
            get { return base.GetSqlXml("UpdateMigraAnulacion"); }
        }

        public string SqlUpdateMigraProcesoPendiente
        {
            get { return base.GetSqlXml("UpdateMigraProcesoPendiente"); }
        }

    }
}
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_MIGRACIONLOG
    /// </summary>
    public class SiMigracionlogHelper : HelperBase
    {
        public SiMigracionlogHelper() : base(Consultas.SiMigracionlogSql)
        {
        }

        public SiMigracionlogDTO Create(IDataReader dr)
        {
            SiMigracionlogDTO entity = new SiMigracionlogDTO();

            int iLogmigcodi = dr.GetOrdinal(this.Logmigcodi);
            if (!dr.IsDBNull(iLogmigcodi)) entity.Logmigcodi = Convert.ToInt32(dr.GetValue(iLogmigcodi));

            int iMigracodi = dr.GetOrdinal(this.Migracodi);
            if (!dr.IsDBNull(iMigracodi)) entity.Migracodi = Convert.ToInt32(dr.GetValue(iMigracodi));

            int iMqxtopcodi = dr.GetOrdinal(this.Mqxtopcodi);
            if (!dr.IsDBNull(iMqxtopcodi)) entity.Mqxtopcodi = Convert.ToInt32(dr.GetValue(iMqxtopcodi));

            int iLogmigoperacion = dr.GetOrdinal(this.Logmigoperacion);
            if (!dr.IsDBNull(iLogmigoperacion)) entity.Logmigoperacion = dr.GetString(iLogmigoperacion);

            int iLogmicodigo = dr.GetOrdinal(this.Logmicodigo);
            if (!dr.IsDBNull(iLogmicodigo)) entity.Logmicodigo = dr.GetString(iLogmicodigo);

            return entity;
        }


        #region Mapeo de Campos

        public string Logmigcodi = "LOGMICODI";
        public string Migracodi = "MIGRACODI";
        public string Mqxtopcodi = "MQXTOPCODI";
        public string Logmigoperacion = "LOGMIOPERACION";
        public string Logmicodigo = "LOGMICODIGO";

        #endregion

        //Cantidad de registros alterados por querys migración para el DBA  
        public string SqlCantRegistrosMigraQuery
        {
            get { return base.GetSqlXml("CantRegistrosMigraQuery"); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_MIGRALOGDBA
    /// </summary>
    public class SiMigralogdbaHelper : HelperBase
    {
        public SiMigralogdbaHelper(): base(Consultas.SiMigralogdbaSql)
        {
        }

        public SiMigralogdbaDTO Create(IDataReader dr)
        {
            SiMigralogdbaDTO entity = new SiMigralogdbaDTO();

            int iMigracodi = dr.GetOrdinal(this.Migracodi);
            if (!dr.IsDBNull(iMigracodi)) entity.Migracodi = Convert.ToInt32(dr.GetValue(iMigracodi));

            int iMigdbacodi = dr.GetOrdinal(this.Migdbacodi);
            if (!dr.IsDBNull(iMigdbacodi)) entity.Migdbacodi = Convert.ToInt32(dr.GetValue(iMigdbacodi));

            int iMigdbaquery = dr.GetOrdinal(this.Migdbaquery);
            if (!dr.IsDBNull(iMigdbaquery)) entity.Migdbaquery = dr.GetString(iMigdbaquery);

            int iMigdbalogquery = dr.GetOrdinal(this.Migdbalogquery);
            if (!dr.IsDBNull(iMigdbalogquery)) entity.Migdbalogquery = dr.GetString(iMigdbalogquery);

            int iMigdbausucreacion = dr.GetOrdinal(this.Migdbausucreacion);
            if (!dr.IsDBNull(iMigdbausucreacion)) entity.Migdbausucreacion = dr.GetString(iMigdbausucreacion);

            int iMigdbafeccreacion = dr.GetOrdinal(this.Migdbafeccreacion);
            if (!dr.IsDBNull(iMigdbafeccreacion)) entity.Migdbafeccreacion = dr.GetDateTime(iMigdbafeccreacion);

            int iMqxtopcodi = dr.GetOrdinal(this.Mqxtopcodi);
            if (!dr.IsDBNull(iMqxtopcodi)) entity.Mqxtopcodi = Convert.ToInt32(dr.GetValue(iMqxtopcodi));


            return entity;
        }

        #region Mapeo de Campos

        public string Migracodi = "MIGRACODI";
        public string Migdbacodi = "MIGDBACODI";
        public string Migdbaquery = "MIGDBAQUERY";

        public string Migdbalogquery = "MIGDBALOGQUERY";
        public string Migdbausucreacion = "MIGDBAUSUCREACION";
        public string Migdbafeccreacion = "MIGDBAFECCREACION";
        public string Mqxtopcodi = "MQXTOPCODI";
        public string Miqubanomtabla = "MIQUBANOMTABLA";

        #endregion

        public string SqlSaveTransferencia
        {
            get { return base.GetSqlXml("SaveTransferencia"); }
        }

        //listar querys dba
        public string SqlListLogDbaByMigracion
        {
            get { return base.GetSqlXml("ListLogDbaByMigracion"); }
        }
    }
}

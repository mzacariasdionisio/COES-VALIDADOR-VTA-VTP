using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_MIGRAPARAMETRO
    /// </summary>
    public class SiMigraparametroHelper : HelperBase
    {
        public SiMigraparametroHelper() : base(Consultas.SiMigraparametroSql)
        {
        }

        public SiMigraParametroDTO Create(IDataReader dr)
        {
            SiMigraParametroDTO entity = new SiMigraParametroDTO();

            int iMigparcodi = dr.GetOrdinal(this.Migparcodi);
            if (!dr.IsDBNull(iMigparcodi)) entity.Migparcodi = Convert.ToInt32(dr.GetValue(iMigparcodi));

            int iMigparnomb = dr.GetOrdinal(this.Migparnomb);
            if (!dr.IsDBNull(iMigparnomb)) entity.Migparnomb = dr.GetString(iMigparnomb);

            int iMigpartipo = dr.GetOrdinal(this.Migpartipo);
            if (!dr.IsDBNull(iMigpartipo)) entity.Migpartipo = Convert.ToInt32(dr.GetValue(iMigpartipo));

            int iMigpardesc = dr.GetOrdinal(this.Migpardesc);
            if (!dr.IsDBNull(iMigpardesc)) entity.Migpardesc = dr.GetString(iMigpardesc);

            int iMigparusucreacion = dr.GetOrdinal(this.Migparusucreacion);
            if (!dr.IsDBNull(iMigparusucreacion)) entity.Migparusucreacion = dr.GetString(iMigparusucreacion);

            int iMigparfeccreacion = dr.GetOrdinal(this.Migparfeccreacion);
            if (!dr.IsDBNull(iMigparfeccreacion)) entity.Migparfeccreacion = dr.GetDateTime(iMigparfeccreacion);

            return entity;
        }


        #region Mapeo de Campos
        public string Migparcodi = "MIGPARCODI";
        public string Migparnomb = "MIGPARNOMB";
        public string Migpartipo = "MIGPARTIPO";
        public string Migpardesc = "MIGPARDESC";
        public string Migparusucreacion = "MIGPARUSUCREACION";
        public string Migparfeccreacion = "MIGPARFECCREACION";

        public string Miqubacodi = "MIQUBACODI";
        public string Miqubanomtabla = "MIQUBANOMTABLA";
        #endregion

        public string SqlObtenerByTipoOperacion
        {
            get { return base.GetSqlXml("ObtenerByTipoOperacion"); }

        }


    }
}

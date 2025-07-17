using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_MIGRAQUERYPARAMETRO
    /// </summary>
    public class SiMigraqueryparametroHelper : HelperBase
    {
        public SiMigraqueryparametroHelper() : base(Consultas.SiMigraqueryparametroSql)
        {
        }

        public SiMigraqueryparametroDTO Create(IDataReader dr)
        {
            SiMigraqueryparametroDTO entity = new SiMigraqueryparametroDTO();

            int iMgqparcodi = dr.GetOrdinal(this.Mgqparcodi);
            if (!dr.IsDBNull(iMgqparcodi)) entity.Mgqparcodi = Convert.ToInt32(dr.GetValue(iMgqparcodi));

            int iMiqubacodi = dr.GetOrdinal(this.Miqubacodi);
            if (!dr.IsDBNull(iMiqubacodi)) entity.Miqubacodi = Convert.ToInt32(dr.GetValue(iMiqubacodi));

            int iMigparcodi = dr.GetOrdinal(this.Migparcodi);
            if (!dr.IsDBNull(iMigparcodi)) entity.Migparcodi = Convert.ToInt32(dr.GetValue(iMigparcodi));

            int iMgqparusucreacion = dr.GetOrdinal(this.Mgqparusucreacion);
            if (!dr.IsDBNull(iMgqparusucreacion)) entity.Mgqparusucreacion = dr.GetString(iMgqparusucreacion);

            int iMgqparfeccreacion = dr.GetOrdinal(this.Mgqparfeccreacion);
            if (!dr.IsDBNull(iMgqparfeccreacion)) entity.Mgqparfeccreacion = dr.GetDateTime(iMgqparfeccreacion);

            int iMgqparactivo = dr.GetOrdinal(this.Mgqparactivo);
            if (!dr.IsDBNull(iMgqparactivo)) entity.Mgqparactivo = Convert.ToInt32(dr.GetValue(iMgqparactivo));

            int iMgqparvalor = dr.GetOrdinal(this.Mgqparvalor);
            if (!dr.IsDBNull(iMgqparvalor)) entity.Mgqparvalor = dr.GetString(iMgqparvalor);

            return entity;
        }


        #region Mapeo de Campos

        public string Mgqparcodi = "MGQPARCODI";
        public string Miqubacodi = "MIQUBACODI";
        public string Migparcodi = "MIGPARCODI";
        public string Mgqparvalor = "MGQPARVALOR";
        public string Mgqparactivo = "MGQPARACTIVO";
        public string Mgqparusucreacion = "MGQPARUSUCREACION";
        public string Mgqparfeccreacion = "MGQPARFECCREACION";



        #endregion
    }
}

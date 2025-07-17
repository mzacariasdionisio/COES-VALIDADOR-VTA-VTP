using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_MIGRAQUERYPLANTTOP
    /// </summary>
    public class SiMigraqueryplanttopHelper : HelperBase
    {
        public SiMigraqueryplanttopHelper() : base(Consultas.SiMigraqueryplanttopSql)
        {
        }

        public SiMigraqueryplanttopDTO Create(IDataReader dr)
        {
            SiMigraqueryplanttopDTO entity = new SiMigraqueryplanttopDTO();

            int iMiqplacodi = dr.GetOrdinal(this.Miqplacodi);
            if (!dr.IsDBNull(iMiqplacodi)) entity.Miqplacodi = Convert.ToInt32(dr.GetValue(iMiqplacodi));

            int iTmopercodi = dr.GetOrdinal(this.Tmopercodi);
            if (!dr.IsDBNull(iTmopercodi)) entity.Tmopercodi = Convert.ToInt32(dr.GetValue(iTmopercodi));

            int iMiptopcodi = dr.GetOrdinal(this.Miptopcodi);
            if (!dr.IsDBNull(iMiptopcodi)) entity.Miptopcodi = Convert.ToInt32(dr.GetValue(iMiptopcodi));

            int iMiptopactivo = dr.GetOrdinal(this.Miptopactivo);
            if (!dr.IsDBNull(iMiptopactivo)) entity.Miptopactivo = Convert.ToInt32(dr.GetValue(iMiptopactivo));

            int iMiptopusucreacion = dr.GetOrdinal(this.Miptopusucreacion);
            if (!dr.IsDBNull(iMiptopusucreacion)) entity.Miptopusucreacion = dr.GetString(iMiptopusucreacion);

            int iMiptopfeccreacion = dr.GetOrdinal(this.Miptopfeccreacion);
            if (!dr.IsDBNull(iMiptopfeccreacion)) entity.Miptopfeccreacion = dr.GetDateTime(iMiptopfeccreacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Miqplacodi = "MIQPLACODI";
        public string Tmopercodi = "TMOPERCODI";
        public string Miptopcodi = "MIPTOPCODI";
        public string Miptopactivo = "MIPTOPACTIVO";
        public string Miptopusucreacion = "MIPTOPUSUCREACION";
        public string Miptopfeccreacion = "MIPTOPFECCREACION";

        #endregion
    }
}

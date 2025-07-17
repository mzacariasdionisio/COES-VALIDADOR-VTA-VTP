using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_MIGRAQUERYPLANTILLA
    /// </summary>
    public class SiMigraqueryplantillaHelper : HelperBase
    {
        public SiMigraqueryplantillaHelper() : base(Consultas.SiMigraqueryplantillaSql)
        {
        }

        public SiMigraqueryplantillaDTO Create(IDataReader dr)
        {
            SiMigraqueryplantillaDTO entity = new SiMigraqueryplantillaDTO();

            int iMiqplacodi = dr.GetOrdinal(this.Miqplacodi);
            if (!dr.IsDBNull(iMiqplacodi)) entity.Miqplacodi = Convert.ToInt32(dr.GetValue(iMiqplacodi));

            int iMiqplanomb = dr.GetOrdinal(this.Miqplanomb);
            if (!dr.IsDBNull(iMiqplanomb)) entity.Miqplanomb = dr.GetString(iMiqplanomb);

            int iMiqpladesc = dr.GetOrdinal(this.Miqpladesc);
            if (!dr.IsDBNull(iMiqpladesc)) entity.Miqpladesc = dr.GetString(iMiqpladesc);

            int iMiqplacomentario = dr.GetOrdinal(this.Miqplacomentario);
            if (!dr.IsDBNull(iMiqplacomentario)) entity.Miqplacomentario = dr.GetString(iMiqplacomentario);

            int iMiqplausucreacion = dr.GetOrdinal(this.Miqplausucreacion);
            if (!dr.IsDBNull(iMiqplausucreacion)) entity.Miqplausucreacion = dr.GetString(iMiqplausucreacion);

            int iMiqplafeccreacion = dr.GetOrdinal(this.Miqplafeccreacion);
            if (!dr.IsDBNull(iMiqplafeccreacion)) entity.Miqplafeccreacion = dr.GetDateTime(iMiqplafeccreacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Miqplacodi = "MIQPLACODI";
        public string Miqpladesc = "MIQPLADESC";
        public string Miqplanomb = "Miqplanomb";
        public string Miqplacomentario = "Miqplacomentario";
        public string Miqplausucreacion = "MIQPLAUSUCREACION";
        public string Miqplafeccreacion = "MIQPLAFECCREACION";

        #endregion
    }
}

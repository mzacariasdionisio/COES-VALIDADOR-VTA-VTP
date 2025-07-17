using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_MIGRAQUERYPLANTPARAM
    /// </summary>
    public class SiMigraqueryplantparamHelper : HelperBase
    {
        public SiMigraqueryplantparamHelper() : base(Consultas.SiMigraqueryplantparamSql)
        {
        }

        public SiMigraqueryplantparamDTO Create(IDataReader dr)
        {
            SiMigraqueryplantparamDTO entity = new SiMigraqueryplantparamDTO();

            int iMiplprcodi = dr.GetOrdinal(this.Miplprcodi);
            if (!dr.IsDBNull(iMiplprcodi)) entity.Miplprcodi = Convert.ToInt32(dr.GetValue(iMiplprcodi));

            int iMiplpractivo = dr.GetOrdinal(this.Miplpractivo);
            if (!dr.IsDBNull(iMiplpractivo)) entity.Miplpractivo = Convert.ToInt32(dr.GetValue(iMiplpractivo));

            int iMiplprusucreacion = dr.GetOrdinal(this.Miplprusucreacion);
            if (!dr.IsDBNull(iMiplprusucreacion)) entity.Miplprusucreacion = dr.GetString(iMiplprusucreacion);

            int iMiplprfeccreacion = dr.GetOrdinal(this.Miplprfeccreacion);
            if (!dr.IsDBNull(iMiplprfeccreacion)) entity.Miplprfeccreacion = dr.GetDateTime(iMiplprfeccreacion);

            int iMiqplacodi = dr.GetOrdinal(this.Miqplacodi);
            if (!dr.IsDBNull(iMiqplacodi)) entity.Miqplacodi = Convert.ToInt32(dr.GetValue(iMiqplacodi));

            int iMigparcodi = dr.GetOrdinal(this.Migparcodi);
            if (!dr.IsDBNull(iMigparcodi)) entity.Migparcodi = Convert.ToInt32(dr.GetValue(iMigparcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Miplprcodi = "MIPLPRCODI";
        public string Miplpractivo = "MIPLPRACTIVO";
        public string Miplprusucreacion = "MIPLPRUSUCREACION";
        public string Miplprfeccreacion = "MIPLPRFECCREACION";
        public string Miqplacodi = "MIQPLACODI";
        public string Migparcodi = "MIGPARCODI";

        #endregion
    }
}

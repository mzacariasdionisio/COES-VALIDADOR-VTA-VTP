using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_MIGRAQUERYXTIPOOPERACION
    /// </summary>
    public class SiMigraqueryxtipooperacionHelper : HelperBase
    {
        public SiMigraqueryxtipooperacionHelper() : base(Consultas.SiMigraqueryxtipooperacionSql)
        {
        }

        public SiMigraqueryxtipooperacionDTO Create(IDataReader dr)
        {
            SiMigraqueryxtipooperacionDTO entity = new SiMigraqueryxtipooperacionDTO();

            int iMqxtopcodi = dr.GetOrdinal(this.Mqxtopcodi);
            if (!dr.IsDBNull(iMqxtopcodi)) entity.Mqxtopcodi = Convert.ToInt32(dr.GetValue(iMqxtopcodi));

            int iMiqubacodi = dr.GetOrdinal(this.Miqubacodi);
            if (!dr.IsDBNull(iMiqubacodi)) entity.Miqubacodi = Convert.ToInt32(dr.GetValue(iMiqubacodi));

            int iTmopercodi = dr.GetOrdinal(this.Tmopercodi);
            if (!dr.IsDBNull(iTmopercodi)) entity.Tmopercodi = Convert.ToInt32(dr.GetValue(iTmopercodi));

            int iMqxtoporden = dr.GetOrdinal(this.Mqxtoporden);
            if (!dr.IsDBNull(iMqxtoporden)) entity.Mqxtoporden = Convert.ToInt32(dr.GetValue(iMqxtoporden));

            int iMqxtopactivo = dr.GetOrdinal(this.Mqxtopactivo);
            if (!dr.IsDBNull(iMqxtopactivo)) entity.Mqxtopactivo = Convert.ToInt32(dr.GetValue(iMqxtopactivo));

            int iMqxtopusucreacion = dr.GetOrdinal(this.Mqxtopusucreacion);
            if (!dr.IsDBNull(iMqxtopusucreacion)) entity.Mqxtopusucreacion = dr.GetString(iMqxtopusucreacion);

            int iMqxtopfeccreacion = dr.GetOrdinal(this.Mqxtopfeccreacion);
            if (!dr.IsDBNull(iMqxtopfeccreacion)) entity.Mqxtopfeccreacion = dr.GetDateTime(iMqxtopfeccreacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Mqxtopcodi = "MQXTOPCODI";
        public string Miqubacodi = "MIQUBACODI";
        public string Tmopercodi = "TMOPERCODI";
        public string Mqxtoporden = "MQXTOPORDEN";
        public string Mqxtopactivo = "MQXTOPACTIVO";
        public string Mqxtopusucreacion = "MQXTOPUSUCREACION";
        public string Mqxtopfeccreacion = "MQXTOPFECCREACION";

        #endregion


        public string SqlListarMqxXTipoOperacionMigracion
        {
            get { return base.GetSqlXml("ListarMqxXTipoOperacionMigracion"); }
        }
    }
}

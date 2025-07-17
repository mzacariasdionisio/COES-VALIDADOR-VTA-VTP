using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_MIGRAQUERYBASE
    /// </summary>
    public class SiMigraquerybaseHelper : HelperBase
    {
        public SiMigraquerybaseHelper() : base(Consultas.SiMigraquerybaseSql)
        {
        }

        public SiMigraquerybaseDTO Create(IDataReader dr)
        {
            SiMigraquerybaseDTO entity = new SiMigraquerybaseDTO();

            int iMiqubacodi = dr.GetOrdinal(this.Miqubacodi);
            if (!dr.IsDBNull(iMiqubacodi)) entity.Miqubacodi = Convert.ToInt32(dr.GetValue(iMiqubacodi));

            int iMiqplacodi = dr.GetOrdinal(this.Miqplacodi);
            if (!dr.IsDBNull(iMiqplacodi)) entity.Miqplacodi = Convert.ToInt32(dr.GetValue(iMiqplacodi));

            int iMiqubaquery = dr.GetOrdinal(this.Miqubaquery);
            if (!dr.IsDBNull(iMiqubaquery)) entity.Miqubaquery = dr.GetString(iMiqubaquery);

            int iMiqubanomtabla = dr.GetOrdinal(this.Miqubanomtabla);
            if (!dr.IsDBNull(iMiqubanomtabla)) entity.Miqubanomtabla = dr.GetString(iMiqubanomtabla);

            int iMiqubamensaje = dr.GetOrdinal(this.Miqubamensaje);
            if (!dr.IsDBNull(iMiqubamensaje)) entity.Miqubamensaje = dr.GetString(iMiqubamensaje);

            int iMiqubaflag = dr.GetOrdinal(this.Miqubaflag);
            if (!dr.IsDBNull(iMiqubaflag)) entity.Miqubaflag = Convert.ToInt32(dr.GetValue(iMiqubaflag));

            int iMiqubaactivo = dr.GetOrdinal(this.Miqubaactivo);
            if (!dr.IsDBNull(iMiqubaactivo)) entity.Miqubaactivo = Convert.ToInt32(dr.GetValue(iMiqubaactivo));

            int iMiqubastr = dr.GetOrdinal(this.Miqubastr);
            if (!dr.IsDBNull(iMiqubastr)) entity.Miqubastr = dr.GetString(iMiqubastr);

            int iMiqubaflagtbladicional = dr.GetOrdinal(this.Miqubaflagtbladicional);
            if (!dr.IsDBNull(iMiqubaflagtbladicional)) entity.Miqubaflagtbladicional = dr.GetString(iMiqubaflagtbladicional);

            int iMiqubausucreacion = dr.GetOrdinal(this.Miqubausucreacion);
            if (!dr.IsDBNull(iMiqubausucreacion)) entity.Miqubausucreacion = dr.GetString(iMiqubausucreacion);

            int iMiqubafeccreacion = dr.GetOrdinal(this.Miqubafeccreacion);
            if (!dr.IsDBNull(iMiqubafeccreacion)) entity.Miqubafeccreacion = dr.GetDateTime(iMiqubafeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Miqplacodi = "MIQPLACODI";
        public string Miqubacodi = "MIQUBACODI";
        public string Miqubaquery = "MIQUBAQUERY";
        public string Miqubausucreacion = "MIQUBAUSUCREACION";
        public string Miqubafeccreacion = "MIQUBAFECCREACION";
        public string Miqubamensaje = "MIQUBAMENSAJE";
        public string Miqubaflag = "MIQUBAFLAG";
        public string Miqubaactivo = "MIQUBAACTIVO";
        public string Miqubastr = "MIQUBASTR";
        public string Miqubaflagtbladicional = "Miqubaflagtbladicional";
        public string Miqubanomtabla = "MIQUBANOMTABLA";

        public string Miqplanomb = "Miqplanomb";
        public string Mqxtopcodi = "mqxtopcodi";

        #endregion

        public string SqlListarMigraQueryXTipoOperacion
        {
            get { return base.GetSqlXml("ListarMigraQueryXTipoOperacion"); }
        }

    }
}

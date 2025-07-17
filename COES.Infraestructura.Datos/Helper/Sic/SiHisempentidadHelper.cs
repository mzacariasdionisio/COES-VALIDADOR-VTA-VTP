using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_HISEMPENTIDAD
    /// </summary>
    public class SiHisempentidadHelper : HelperBase
    {
        public SiHisempentidadHelper() : base(Consultas.SiHisempentidadSql)
        {
        }

        public SiHisempentidadDTO Create(IDataReader dr)
        {
            SiHisempentidadDTO entity = new SiHisempentidadDTO();

            int iMigracodi = dr.GetOrdinal(this.Migracodi);
            if (!dr.IsDBNull(iMigracodi)) entity.Migracodi = Convert.ToInt32(dr.GetValue(iMigracodi));

            int iHempencodi = dr.GetOrdinal(this.Hempencodi);
            if (!dr.IsDBNull(iHempencodi)) entity.Hempencodi = Convert.ToInt32(dr.GetValue(iHempencodi));

            int iHempentitulo = dr.GetOrdinal(this.Hempentitulo);
            if (!dr.IsDBNull(iHempentitulo)) entity.Hempentitulo = dr.GetString(iHempentitulo);

            int iHempencampoid = dr.GetOrdinal(this.Hempencampoid);
            if (!dr.IsDBNull(iHempencampoid)) entity.Hempencampoid = dr.GetString(iHempencampoid);

            int iHempentablename = dr.GetOrdinal(this.Hempentablename);
            if (!dr.IsDBNull(iHempentablename)) entity.Hempentablename = dr.GetString(iHempentablename);

            int iHempencampodesc = dr.GetOrdinal(this.Hempencampodesc);
            if (!dr.IsDBNull(iHempencampodesc)) entity.Hempencampodesc = dr.GetString(iHempencampodesc);

            int iHempencampodesc2 = dr.GetOrdinal(this.Hempencampodesc2);
            if (!dr.IsDBNull(iHempencampodesc2)) entity.Hempencampodesc2 = dr.GetString(iHempencampodesc2);

            int iHempencampoestado = dr.GetOrdinal(this.Hempencampoestado);
            if (!dr.IsDBNull(iHempencampoestado)) entity.Hempencampoestado = dr.GetString(iHempencampoestado);

            return entity;
        }

        #region Mapeo de Campos

        public string Migracodi = "MIGRACODI";
        public string Hempencodi = "HEMPENCODI";
        public string Hempencampoid = "HEMPENCAMPOID";
        public string Hempentitulo = "HEMPENTITULO";
        public string Hempentablename = "HEMPENTABLENAME";
        public string Hempencampodesc = "HEMPENCAMPODESC";
        public string Hempencampodesc2 = "HEMPENCAMPODESC2";
        public string Hempencampoestado = "HEMPENCAMPOESTADO";

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_ARCHIVO
    /// </summary>
    public class MeArchivoHelper : HelperBase
    {
        public MeArchivoHelper(): base(Consultas.MeArchivoSql)
        {
        }

        public MeArchivoDTO Create(IDataReader dr)
        {
            MeArchivoDTO entity = new MeArchivoDTO();

            int iArchcodi = dr.GetOrdinal(this.Archcodi);
            if (!dr.IsDBNull(iArchcodi)) entity.Archcodi = Convert.ToInt32(dr.GetValue(iArchcodi));

            int iFormatcodi = dr.GetOrdinal(this.Formatcodi);
            if (!dr.IsDBNull(iFormatcodi)) entity.Formatcodi = Convert.ToInt32(dr.GetValue(iFormatcodi));

            int iArchsize = dr.GetOrdinal(this.Archsize);
            if (!dr.IsDBNull(iArchsize)) entity.Archsize = dr.GetDecimal(iArchsize);

            int iArchnomborig = dr.GetOrdinal(this.Archnomborig);
            if (!dr.IsDBNull(iArchnomborig)) entity.Archnomborig = dr.GetString(iArchnomborig);

            int iArchnombpatron = dr.GetOrdinal(this.Archnombpatron);
            if (!dr.IsDBNull(iArchnombpatron)) entity.Archnombpatron = dr.GetString(iArchnombpatron);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Archcodi = "ARCHCODI";
        public string Formatcodi = "FORMATCODI";
        public string Archsize = "ARCHSIZE";
        public string Archnomborig = "ARCHNOMBORIG";
        public string Archnombpatron = "ARCHNOMBPATRON";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_INTERCONEXION
    /// </summary>
    public class InInterconexionHelper : HelperBase
    {
        public InInterconexionHelper(): base(Consultas.InInterconexionSql)
        {
        }

        public InInterconexionDTO Create(IDataReader dr)
        {
            InInterconexionDTO entity = new InInterconexionDTO();

            int iIntercodi = dr.GetOrdinal(this.Intercodi);
            if (!dr.IsDBNull(iIntercodi)) entity.Intercodi = Convert.ToInt32(dr.GetValue(iIntercodi));

            int iInterdecrip = dr.GetOrdinal(this.Interdecrip);
            if (!dr.IsDBNull(iInterdecrip)) entity.Interdecrip = dr.GetString(iInterdecrip);

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iPtomedicodisecun = dr.GetOrdinal(this.Ptomedicodisecun);
            if (!dr.IsDBNull(iPtomedicodisecun)) entity.Ptomedicodisecun = Convert.ToInt32(dr.GetValue(iPtomedicodisecun));

            int iInterenlace = dr.GetOrdinal(this.Interenlace);
            if (!dr.IsDBNull(iInterenlace)) entity.Interenlace = dr.GetString(iInterenlace);

            int iInternomlinea = dr.GetOrdinal(this.Internomlinea);
            if (!dr.IsDBNull(iInternomlinea)) entity.Internomlinea = dr.GetString(iInternomlinea);

            return entity;
        }


        #region Mapeo de Campos

        public string Intercodi = "INTERCODI";
        public string Interdecrip = "INTERDECRIP";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Interenlace = "INTERENLACE";
        public string Ptomedicodisecun = "PTOMEDICODISECUN";
        public string Internomlinea = "INTERNOMLINEA";

        #endregion
    }
}

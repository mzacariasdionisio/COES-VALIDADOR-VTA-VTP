using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_CONJUNTOENLACE
    /// </summary>
    public class CmConjuntoenlaceHelper : HelperBase
    {
        public CmConjuntoenlaceHelper(): base(Consultas.CmConjuntoenlaceSql)
        {
        }

        public CmConjuntoenlaceDTO Create(IDataReader dr)
        {
            CmConjuntoenlaceDTO entity = new CmConjuntoenlaceDTO();

            int iCnjenlcodi = dr.GetOrdinal(this.Cnjenlcodi);
            if (!dr.IsDBNull(iCnjenlcodi)) entity.Cnjenlcodi = Convert.ToInt32(dr.GetValue(iCnjenlcodi));

            int iConfigcodi = dr.GetOrdinal(this.Configcodi);
            if (!dr.IsDBNull(iConfigcodi)) entity.Configcodi = Convert.ToInt32(dr.GetValue(iConfigcodi));

            int iGrulincodi = dr.GetOrdinal(this.Grulincodi);
            if (!dr.IsDBNull(iGrulincodi)) entity.Grulincodi = Convert.ToInt32(dr.GetValue(iGrulincodi));

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Cnjenlcodi = "CNJENLCODI";
        public string Configcodi = "CONFIGCODI";
        public string Grulincodi = "GRULINCODI";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";

        #endregion
    }
}

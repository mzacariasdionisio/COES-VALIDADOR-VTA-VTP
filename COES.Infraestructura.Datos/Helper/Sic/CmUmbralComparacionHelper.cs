using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_UMBRAL_COMPARACION
    /// </summary>
    public class CmUmbralComparacionHelper : HelperBase
    {
        public CmUmbralComparacionHelper(): base(Consultas.CmUmbralComparacionSql)
        {
        }

        public CmUmbralComparacionDTO Create(IDataReader dr)
        {
            CmUmbralComparacionDTO entity = new CmUmbralComparacionDTO();

            int iCmumcocodi = dr.GetOrdinal(this.Cmumcocodi);
            if (!dr.IsDBNull(iCmumcocodi)) entity.Cmumcocodi = Convert.ToInt32(dr.GetValue(iCmumcocodi));

            int iCmumcohopdesp = dr.GetOrdinal(this.Cmumcohopdesp);
            if (!dr.IsDBNull(iCmumcohopdesp)) entity.Cmumcohopdesp = dr.GetDecimal(iCmumcohopdesp);

            int iCmumcoemsdesp = dr.GetOrdinal(this.Cmumcoemsdesp);
            if (!dr.IsDBNull(iCmumcoemsdesp)) entity.Cmumcoemsdesp = dr.GetDecimal(iCmumcoemsdesp);

            int iCmuncodemanda = dr.GetOrdinal(this.Cmuncodemanda);
            if (!dr.IsDBNull(iCmuncodemanda)) entity.Cmuncodemanda = dr.GetDecimal(iCmuncodemanda);

            int iCmumcousucreacion = dr.GetOrdinal(this.Cmumcousucreacion);
            if (!dr.IsDBNull(iCmumcousucreacion)) entity.Cmumcousucreacion = dr.GetString(iCmumcousucreacion);

            int iCmumcofeccreacion = dr.GetOrdinal(this.Cmumcofeccreacion);
            if (!dr.IsDBNull(iCmumcofeccreacion)) entity.Cmumcofeccreacion = dr.GetDateTime(iCmumcofeccreacion);

            int iCmuncousumodificacion = dr.GetOrdinal(this.Cmuncousumodificacion);
            if (!dr.IsDBNull(iCmuncousumodificacion)) entity.Cmuncousumodificacion = dr.GetString(iCmuncousumodificacion);

            int iCmuncofecmodificacion = dr.GetOrdinal(this.Cmuncofecmodificacion);
            if (!dr.IsDBNull(iCmuncofecmodificacion)) entity.Cmuncofecmodificacion = dr.GetDateTime(iCmuncofecmodificacion);

            int iCmumcoci = dr.GetOrdinal(this.Cmumcoci);
            if (!dr.IsDBNull(iCmumcoci)) entity.Cmumcoci = dr.GetDecimal(iCmumcoci);

            int iCmumconumiter = dr.GetOrdinal(this.Cmumconumiter);
            if (!dr.IsDBNull(iCmumconumiter)) entity.Cmumconumiter = dr.GetDecimal(iCmumconumiter);

            int iCmumcovarang = dr.GetOrdinal(this.Cmumcovarang);
            if (!dr.IsDBNull(iCmumcovarang)) entity.Cmumcovarang = dr.GetDecimal(iCmumcovarang);

            return entity;
        }


        #region Mapeo de Campos

        public string Cmumcocodi = "CMUMCOCODI";
        public string Cmumcohopdesp = "CMUMCOHOPDESP";
        public string Cmumcoemsdesp = "CMUMCOEMSDESP";
        public string Cmuncodemanda = "CMUNCODEMANDA";
        public string Cmumcousucreacion = "CMUMCOUSUCREACION";
        public string Cmumcofeccreacion = "CMUMCOFECCREACION";
        public string Cmuncousumodificacion = "CMUNCOUSUMODIFICACION";
        public string Cmuncofecmodificacion = "CMUNCOFECMODIFICACION";
        public string Cmumcoci = "CMUMCOCI";
        public string Cmumconumiter = "CMUMCONUMITER";
        public string Cmumcovarang = "CMUMCOVARANG";

        #endregion
    }
}

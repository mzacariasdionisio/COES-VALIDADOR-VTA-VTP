using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CCC_VERSION
    /// </summary>
    public class CccVersionHelper : HelperBase
    {
        public CccVersionHelper() : base(Consultas.CccVersionSql)
        {
        }

        public CccVersionDTO Create(IDataReader dr)
        {
            CccVersionDTO entity = new CccVersionDTO();

            int iCccvercodi = dr.GetOrdinal(this.Cccvercodi);
            if (!dr.IsDBNull(iCccvercodi)) entity.Cccvercodi = Convert.ToInt32(dr.GetValue(iCccvercodi));

            int iCccverhorizonte = dr.GetOrdinal(this.Cccverhorizonte);
            if (!dr.IsDBNull(iCccverhorizonte)) entity.Cccverhorizonte = dr.GetString(iCccverhorizonte);

            int iCccverfecha = dr.GetOrdinal(this.Cccverfecha);
            if (!dr.IsDBNull(iCccverfecha)) entity.Cccverfecha = dr.GetDateTime(iCccverfecha);

            int iCccvernumero = dr.GetOrdinal(this.Cccvernumero);
            if (!dr.IsDBNull(iCccvernumero)) entity.Cccvernumero = Convert.ToInt32(dr.GetValue(iCccvernumero));

            int iCccverestado = dr.GetOrdinal(this.Cccverestado);
            if (!dr.IsDBNull(iCccverestado)) entity.Cccverestado = dr.GetString(iCccverestado);

            int iCccverobs = dr.GetOrdinal(this.Cccverobs);
            if (!dr.IsDBNull(iCccverobs)) entity.Cccverobs = dr.GetString(iCccverobs);

            int iCccverrptcodis = dr.GetOrdinal(this.Cccverrptcodis);
            if (!dr.IsDBNull(iCccverrptcodis)) entity.Cccverrptcodis = dr.GetString(iCccverrptcodis);

            int iCccverusucreacion = dr.GetOrdinal(this.Cccverusucreacion);
            if (!dr.IsDBNull(iCccverusucreacion)) entity.Cccverusucreacion = dr.GetString(iCccverusucreacion);

            int iCccverfeccreacion = dr.GetOrdinal(this.Cccverfeccreacion);
            if (!dr.IsDBNull(iCccverfeccreacion)) entity.Cccverfeccreacion = dr.GetDateTime(iCccverfeccreacion);

            int iCccverusumodificacion = dr.GetOrdinal(this.Cccverusumodificacion);
            if (!dr.IsDBNull(iCccverusumodificacion)) entity.Cccverusumodificacion = dr.GetString(iCccverusumodificacion);

            int iCccverfecmodificacion = dr.GetOrdinal(this.Cccverfecmodificacion);
            if (!dr.IsDBNull(iCccverfecmodificacion)) entity.Cccverfecmodificacion = dr.GetDateTime(iCccverfecmodificacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Cccvercodi = "CCCVERCODI";
        public string Cccverhorizonte = "CCCVERHORIZONTE";
        public string Cccverfecha = "CCCVERFECHA";
        public string Cccvernumero = "CCCVERNUMERO";
        public string Cccverestado = "CCCVERESTADO";
        public string Cccverobs = "CCCVEROBS";
        public string Cccverrptcodis = "CCCVERRPTCODIS";
        public string Cccverusucreacion = "CCCVERUSUCREACION";
        public string Cccverfeccreacion = "CCCVERFECCREACION";
        public string Cccverusumodificacion = "CCCVERUSUMODIFICACION";
        public string Cccverfecmodificacion = "CCCVERFECMODIFICACION";

        #endregion
    }
}

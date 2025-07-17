using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_FICTECPROP
    /// </summary>
    public class FtFictecPropHelper : HelperBase
    {
        public FtFictecPropHelper()
            : base(Consultas.FtFictecPropSql)
        {
        }

        public FtFictecPropDTO Create(IDataReader dr)
        {
            FtFictecPropDTO entity = new FtFictecPropDTO();

            int iFtpropcodi = dr.GetOrdinal(this.Ftpropcodi);
            if (!dr.IsDBNull(iFtpropcodi)) entity.Ftpropcodi = Convert.ToInt32(dr.GetValue(iFtpropcodi));

            int iFtpropnomb = dr.GetOrdinal(this.Ftpropnomb);
            if (!dr.IsDBNull(iFtpropnomb)) entity.Ftpropnomb = dr.GetString(iFtpropnomb);

            int iFtpropestado = dr.GetOrdinal(this.Ftpropestado);
            if (!dr.IsDBNull(iFtpropestado)) entity.Ftpropestado = Convert.ToInt32(dr.GetValue(iFtpropestado));

            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iCatecodi = dr.GetOrdinal(this.Catecodi);
            if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

            int iFtproptipo = dr.GetOrdinal(this.Ftproptipo);
            if (!dr.IsDBNull(iFtproptipo)) entity.Ftproptipo = dr.GetString(iFtproptipo);

            int iFtpropunidad = dr.GetOrdinal(this.Ftpropunidad);
            if (!dr.IsDBNull(iFtpropunidad)) entity.Ftpropunidad = dr.GetString(iFtpropunidad);

            int iFtpropdefinicion = dr.GetOrdinal(this.Ftpropdefinicion);
            if (!dr.IsDBNull(iFtpropdefinicion)) entity.Ftpropdefinicion = dr.GetString(iFtpropdefinicion);

            return entity;
        }

        #region Mapeo de Campos

        public string Ftpropcodi = "FTPROPCODI";
        public string Ftpropnomb = "FTPROPNOMB";
        public string Ftpropestado = "FTPROPESTADO";
        public string Famcodi = "FAMCODI";
        public string Catecodi = "CATECODI";
        public string Ftproptipo = "FTPROPTIPO";
        public string Ftpropunidad = "FTPROPUNIDAD";
        public string Ftpropdefinicion = "FTPROPDEFINICION";

        #endregion
    }
}

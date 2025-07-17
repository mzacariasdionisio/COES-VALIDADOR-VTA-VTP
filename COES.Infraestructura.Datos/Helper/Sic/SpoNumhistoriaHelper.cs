using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SPO_NUMHISTORIA
    /// </summary>
    public class SpoNumhistoriaHelper : HelperBase
    {
        public SpoNumhistoriaHelper(): base(Consultas.SpoNumhistoriaSql)
        {
        }

        public SpoNumhistoriaDTO Create(IDataReader dr)
        {
            SpoNumhistoriaDTO entity = new SpoNumhistoriaDTO();

            int iNumhiscodi = dr.GetOrdinal(this.Numhiscodi);
            if (!dr.IsDBNull(iNumhiscodi)) entity.Numhiscodi = Convert.ToInt32(dr.GetValue(iNumhiscodi));

            int iNumecodi = dr.GetOrdinal(this.Numecodi);
            if (!dr.IsDBNull(iNumecodi)) entity.Numecodi = Convert.ToInt32(dr.GetValue(iNumecodi));

            int iNumhisdescripcion = dr.GetOrdinal(this.Numhisdescripcion);
            if (!dr.IsDBNull(iNumhisdescripcion)) entity.Numhisdescripcion = dr.GetString(iNumhisdescripcion);

            int iNumhisabrev = dr.GetOrdinal(this.Numhisabrev);
            if (!dr.IsDBNull(iNumhisabrev)) entity.Numhisabrev = dr.GetString(iNumhisabrev);

            int iNumhisfecha = dr.GetOrdinal(this.Numhisfecha);
            if (!dr.IsDBNull(iNumhisfecha)) entity.Numhisfecha = dr.GetDateTime(iNumhisfecha);

            int iNumhisusuario = dr.GetOrdinal(this.Numhisusuario);
            if (!dr.IsDBNull(iNumhisusuario)) entity.Numhisusuario = dr.GetString(iNumhisusuario);

            return entity;
        }


        #region Mapeo de Campos

        public string Numhiscodi = "NUMHISCODI";
        public string Numecodi = "NUMECODI";
        public string Numhisdescripcion = "NUMHISDESCRIPCION";
        public string Numhisabrev = "NUMHISABREV";
        public string Numhisfecha = "NUMHISFECHA";
        public string Numhisusuario = "NUMHISUSUARIO";

        #endregion
    }
}

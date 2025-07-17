using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_DNOTAS
    /// </summary>
    public class PrDnotasHelper : HelperBase
    {
        public PrDnotasHelper(): base(Consultas.PrDnotasSql)
        {
        }

        public PrDnotasDTO Create(IDataReader dr)
        {
            PrDnotasDTO entity = new PrDnotasDTO();

            int iFecha = dr.GetOrdinal(this.Fecha);
            if (!dr.IsDBNull(iFecha)) entity.Fecha = dr.GetDateTime(iFecha);

            int iLectcodi = dr.GetOrdinal(this.Lectcodi);
            if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

            int iNotaitem = dr.GetOrdinal(this.Notaitem);
            if (!dr.IsDBNull(iNotaitem)) entity.Notaitem = Convert.ToInt32(dr.GetValue(iNotaitem));

            int iNotadesc = dr.GetOrdinal(this.Notadesc);
            if (!dr.IsDBNull(iNotadesc)) entity.Notadesc = dr.GetString(iNotadesc);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Fecha = "FECHA";
        public string Lectcodi = "LECTCODI";
        public string Notaitem = "NOTAITEM";
        public string Notadesc = "NOTADESC";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";

        #endregion
    }
}

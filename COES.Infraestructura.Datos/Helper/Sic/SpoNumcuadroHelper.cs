using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SPO_NUMCUADRO
    /// </summary>
    public class SpoNumcuadroHelper : HelperBase
    {
        public SpoNumcuadroHelper(): base(Consultas.SpoNumcuadroSql)
        {
        }

        public SpoNumcuadroDTO Create(IDataReader dr)
        {
            SpoNumcuadroDTO entity = new SpoNumcuadroDTO();

            int iNumccodi = dr.GetOrdinal(this.Numccodi);
            if (!dr.IsDBNull(iNumccodi)) entity.Numccodi = Convert.ToInt32(dr.GetValue(iNumccodi));

            int iNumecodi = dr.GetOrdinal(this.Numecodi);
            if (!dr.IsDBNull(iNumecodi)) entity.Numecodi = Convert.ToInt32(dr.GetValue(iNumecodi));

            int iNumcdescrip = dr.GetOrdinal(this.Numcdescrip);
            if (!dr.IsDBNull(iNumcdescrip)) entity.Numcdescrip = dr.GetString(iNumcdescrip);

            int iNumctitulo = dr.GetOrdinal(this.Numctitulo);
            if (!dr.IsDBNull(iNumctitulo)) entity.Numctitulo = dr.GetString(iNumctitulo);

            return entity;
        }


        #region Mapeo de Campos

        public string Numccodi = "NUMCCODI";
        public string Numecodi = "NUMECODI";
        public string Numcdescrip = "NUMCDESCRIP";
        public string Numctitulo = "NUMCTITULO";

        #endregion
    }
}

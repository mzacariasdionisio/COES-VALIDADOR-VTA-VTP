using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_MENSAJEENVIO
    /// </summary>
    public class MeMensajeenvioHelper : HelperBase
    {
        public MeMensajeenvioHelper(): base(Consultas.MeMensajeenvioSql)
        {
        }

        public MeMensajeenvioDTO Create(IDataReader dr)
        {
            MeMensajeenvioDTO entity = new MeMensajeenvioDTO();

            int iMencodi = dr.GetOrdinal(this.Mencodi);
            if (!dr.IsDBNull(iMencodi)) entity.Mencodi = Convert.ToInt32(dr.GetValue(iMencodi));

            int iMenabrev = dr.GetOrdinal(this.Menabrev);
            if (!dr.IsDBNull(iMenabrev)) entity.Menabrev = dr.GetString(iMenabrev);

            int iMendescrip = dr.GetOrdinal(this.Mendescrip);
            if (!dr.IsDBNull(iMendescrip)) entity.Mendescrip = dr.GetString(iMendescrip);

            return entity;
        }


        #region Mapeo de Campos

        public string Mencodi = "MENCODI";
        public string Menabrev = "MENABREV";
        public string Mendescrip = "MENDESCRIP";

        #endregion
    }
}

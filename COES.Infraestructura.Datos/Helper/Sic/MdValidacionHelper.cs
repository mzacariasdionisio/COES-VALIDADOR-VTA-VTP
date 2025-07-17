using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MD_VALIDACION
    /// </summary>
    public class MdValidacionHelper : HelperBase
    {
        public MdValidacionHelper(): base(Consultas.MdValidacionSql)
        {
        }

        public MdValidacionDTO Create(IDataReader dr)
        {
            MdValidacionDTO entity = new MdValidacionDTO();

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iValidames = dr.GetOrdinal(this.Validames);
            if (!dr.IsDBNull(iValidames)) entity.Validames = dr.GetDateTime(iValidames);

            int iValidafecha = dr.GetOrdinal(this.Validafecha);
            if (!dr.IsDBNull(iValidafecha)) entity.Validafecha = dr.GetDateTime(iValidafecha);

            int iValidaestado = dr.GetOrdinal(this.Validaestado);
            if (!dr.IsDBNull(iValidaestado)) entity.Validaestado = dr.GetString(iValidaestado);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Emprcodi = "EMPRCODI";
        public string Validames = "VALIDAMES";
        public string Validafecha = "VALIDAFECHA";
        public string Validaestado = "VALIDAESTADO";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Tipoemprcodi = "TIPOEMPRCODI";

        #endregion
    }
}

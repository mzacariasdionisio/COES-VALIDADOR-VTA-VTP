using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_CAMBIO_TURNO_AUDIT
    /// </summary>
    public class SiCambioTurnoAuditHelper : HelperBase
    {
        public SiCambioTurnoAuditHelper(): base(Consultas.SiCambioTurnoAuditSql)
        {
        }

        public SiCambioTurnoAuditDTO Create(IDataReader dr)
        {
            SiCambioTurnoAuditDTO entity = new SiCambioTurnoAuditDTO();

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iDesaccion = dr.GetOrdinal(this.Desaccion);
            if (!dr.IsDBNull(iDesaccion)) entity.Desaccion = dr.GetString(iDesaccion);

            int iCambioturnocodi = dr.GetOrdinal(this.Cambioturnocodi);
            if (!dr.IsDBNull(iCambioturnocodi)) entity.Cambioturnocodi = Convert.ToInt32(dr.GetValue(iCambioturnocodi));

            int iTurnoauditcodi = dr.GetOrdinal(this.Turnoauditcodi);
            if (!dr.IsDBNull(iTurnoauditcodi)) entity.Turnoauditcodi = Convert.ToInt32(dr.GetValue(iTurnoauditcodi));

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            return entity;
        }


        #region Mapeo de Campos

        public string Lastdate = "LASTDATE";
        public string Desaccion = "DESACCION";
        public string Cambioturnocodi = "CAMBIOTURNOCODI";
        public string Turnoauditcodi = "TURNOAUDITCODI";
        public string Lastuser = "LASTUSER";

        #endregion
    }
}

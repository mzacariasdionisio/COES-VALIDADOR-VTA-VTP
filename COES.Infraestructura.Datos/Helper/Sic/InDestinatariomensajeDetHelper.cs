using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_DESTINATARIOMENSAJE_DET
    /// </summary>
    public class InDestinatariomensajeDetHelper : HelperBase
    {
        public InDestinatariomensajeDetHelper() : base(Consultas.InDestinatariomensajeDetSql)
        {
        }

        public InDestinatariomensajeDetDTO Create(IDataReader dr)
        {
            InDestinatariomensajeDetDTO entity = new InDestinatariomensajeDetDTO();

            int iEvenclasecodi = dr.GetOrdinal(this.Evenclasecodi);
            if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = Convert.ToInt32(dr.GetValue(iEvenclasecodi));

            int iIndmdecodi = dr.GetOrdinal(this.Indmdecodi);
            if (!dr.IsDBNull(iIndmdecodi)) entity.Indmdecodi = Convert.ToInt32(dr.GetValue(iIndmdecodi));

            int iIndemecodi = dr.GetOrdinal(this.Indemecodi);
            if (!dr.IsDBNull(iIndemecodi)) entity.Indemecodi = Convert.ToInt32(dr.GetValue(iIndemecodi));

            int iIndmdeacceso = dr.GetOrdinal(this.Indmdeacceso);
            if (!dr.IsDBNull(iIndmdeacceso)) entity.Indmdeacceso = Convert.ToInt32(dr.GetValue(iIndmdeacceso));

            return entity;
        }


        #region Mapeo de Campos

        public string Evenclasecodi = "EVENCLASECODI";
        public string Indmdecodi = "INDMDECODI";
        public string Indemecodi = "INDEMECODI";
        public string Indmdeacceso = "INDMDEACCESO";

        #endregion
    }
}

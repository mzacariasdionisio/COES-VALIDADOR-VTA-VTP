using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_MENSAJE_REL_ARCHIVO
    /// </summary>
    public class InMensajeRelArchivoHelper : HelperBase
    {
        public InMensajeRelArchivoHelper() : base(Consultas.InMensajeRelArchivoSql)
        {
        }

        public InMensajeRelArchivoDTO Create(IDataReader dr)
        {
            InMensajeRelArchivoDTO entity = new InMensajeRelArchivoDTO();

            int iMsgcodi = dr.GetOrdinal(this.Msgcodi);
            if (!dr.IsDBNull(iMsgcodi)) entity.Msgcodi = Convert.ToInt32(dr.GetValue(iMsgcodi));

            int iInarchcodi = dr.GetOrdinal(this.Inarchcodi);
            if (!dr.IsDBNull(iInarchcodi)) entity.Inarchcodi = Convert.ToInt32(dr.GetValue(iInarchcodi));

            int iIrmearcodi = dr.GetOrdinal(this.Irmearcodi);
            if (!dr.IsDBNull(iIrmearcodi)) entity.Irmearcodi = Convert.ToInt32(dr.GetValue(iIrmearcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Msgcodi = "MSGCODI";
        public string Inarchcodi = "INARCHCODI";
        public string Irmearcodi = "IRMEARCODI";

        #endregion

    }
}

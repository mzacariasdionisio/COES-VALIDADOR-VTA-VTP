using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Scada
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TR_OBSERVACION_ESTADO
    /// </summary>
    public class TrObservacionEstadoHelper : HelperBase
    {
        public TrObservacionEstadoHelper(): base(Consultas.TrObservacionEstadoSql)
        {
        }

        public TrObservacionEstadoDTO Create(IDataReader dr)
        {
            TrObservacionEstadoDTO entity = new TrObservacionEstadoDTO();

            int iObscancodi = dr.GetOrdinal(this.Obscancodi);
            if (!dr.IsDBNull(iObscancodi)) entity.Obscancodi = Convert.ToInt32(dr.GetValue(iObscancodi));

            int iObsestcodi = dr.GetOrdinal(this.Obsestcodi);
            if (!dr.IsDBNull(iObsestcodi)) entity.Obsestcodi = Convert.ToInt32(dr.GetValue(iObsestcodi));

            int iObsestestado = dr.GetOrdinal(this.Obsestestado);
            if (!dr.IsDBNull(iObsestestado)) entity.Obsestestado = dr.GetString(iObsestestado);

            int iObsestcomentario = dr.GetOrdinal(this.Obsestcomentario);
            if (!dr.IsDBNull(iObsestcomentario)) entity.Obsestcomentario = dr.GetString(iObsestcomentario);

            int iObsestusuario = dr.GetOrdinal(this.Obsestusuario);
            if (!dr.IsDBNull(iObsestusuario)) entity.Obsestusuario = dr.GetString(iObsestusuario);

            int iObsestfecha = dr.GetOrdinal(this.Obsestfecha);
            if (!dr.IsDBNull(iObsestfecha)) entity.Obsestfecha = dr.GetDateTime(iObsestfecha);

            return entity;
        }


        #region Mapeo de Campos

        public string Obscancodi = "OBSCANCODI";
        public string Obsestcodi = "OBSESTCODI";
        public string Obsestestado = "OBSESTESTADO";
        public string Obsestcomentario = "OBSESTCOMENTARIO";
        public string Obsestusuario = "OBSESTUSUARIO";
        public string Obsestfecha = "OBSESTFECHA";

        #endregion
    }
}

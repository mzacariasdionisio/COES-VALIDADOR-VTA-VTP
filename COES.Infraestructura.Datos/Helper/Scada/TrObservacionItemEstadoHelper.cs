using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Scada
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TR_OBSERVACION_ITEM_ESTADO
    /// </summary>
    public class TrObservacionItemEstadoHelper : HelperBase
    {
        public TrObservacionItemEstadoHelper(): base(Consultas.TrObservacionItemEstadoSql)
        {
        }

        public TrObservacionItemEstadoDTO Create(IDataReader dr)
        {
            TrObservacionItemEstadoDTO entity = new TrObservacionItemEstadoDTO();

            int iObsitecodi = dr.GetOrdinal(this.Obsitecodi);
            if (!dr.IsDBNull(iObsitecodi)) entity.Obsitecodi = Convert.ToInt32(dr.GetValue(iObsitecodi));

            int iObitescodi = dr.GetOrdinal(this.Obitescodi);
            if (!dr.IsDBNull(iObitescodi)) entity.Obitescodi = dr.GetInt32(iObitescodi);

            int iObitesestado = dr.GetOrdinal(this.Obitesestado);
            if (!dr.IsDBNull(iObitesestado)) entity.Obitesestado = dr.GetString(iObitesestado);

            int iObitescomentario = dr.GetOrdinal(this.Obitescomentario);
            if (!dr.IsDBNull(iObitescomentario)) entity.Obitescomentario = dr.GetString(iObitescomentario);

            int iObitesusuario = dr.GetOrdinal(this.Obitesusuario);
            if (!dr.IsDBNull(iObitesusuario)) entity.Obitesusuario = dr.GetString(iObitesusuario);

            int iObitesfecha = dr.GetOrdinal(this.Obitesfecha);
            if (!dr.IsDBNull(iObitesfecha)) entity.Obitesfecha = dr.GetDateTime(iObitesfecha);

            return entity;
        }


        #region Mapeo de Campos

        public string Obsitecodi = "OBSITECODI";
        public string Obitescodi = "OBITESCODI";
        public string Obitesestado = "OBITESESTADO";
        public string Obitescomentario = "OBITESCOMENTARIO";
        public string Obitesusuario = "OBITESUSUARIO";
        public string Obitesfecha = "OBITESFECHA";

        #endregion
    }
}

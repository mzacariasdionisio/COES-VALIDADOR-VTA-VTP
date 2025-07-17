
using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Scada
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TR_OBSERVACION_ITEM
    /// </summary>
    public class TrObservacionItemHelper : HelperBase
    {
        public TrObservacionItemHelper(): base(Consultas.TrObservacionItemSql)
        {
        }

        public TrObservacionItemDTO Create(IDataReader dr)
        {
            TrObservacionItemDTO entity = new TrObservacionItemDTO();

            int iCanalcodi = dr.GetOrdinal(this.Canalcodi);
            if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

            int iObsitecodi = dr.GetOrdinal(this.Obsitecodi);
            if (!dr.IsDBNull(iObsitecodi)) entity.Obsitecodi = Convert.ToInt32(dr.GetValue(iObsitecodi));

            int iObsiteestado = dr.GetOrdinal(this.Obsiteestado);
            if (!dr.IsDBNull(iObsiteestado)) entity.Obsiteestado = dr.GetString(iObsiteestado);

            int iObsitecomentario = dr.GetOrdinal(this.Obsitecomentario);
            if (!dr.IsDBNull(iObsitecomentario)) entity.Obsitecomentario = dr.GetString(iObsitecomentario);

            int iObscancodi = dr.GetOrdinal(this.Obscancodi);
            if (!dr.IsDBNull(iObscancodi)) entity.Obscancodi = Convert.ToInt32(dr.GetValue(iObscancodi));

            int iObsiteusuario = dr.GetOrdinal(this.Obsiteusuario);
            if (!dr.IsDBNull(iObsiteusuario)) entity.Obsiteusuario = dr.GetString(iObsiteusuario);

            int iObsitefecha = dr.GetOrdinal(this.Obsitefecha);
            if (!dr.IsDBNull(iObsitefecha)) entity.Obsitefecha = dr.GetDateTime(iObsitefecha);

            int iObsitecomentarioagente = dr.GetOrdinal(this.Obsitecomentarioagente);
            if (!dr.IsDBNull(iObsitecomentarioagente)) entity.Obsitecomentarioagente = dr.GetString(iObsitecomentarioagente);

            return entity;
        }


        #region Mapeo de Campos

        public string Canalcodi = "CANALCODI";
        public string Obsitecodi = "OBSITECODI";
        public string Obsiteestado = "OBSITEESTADO";
        public string Obsitecomentario = "OBSITECOMENTARIO";
        public string Obscancodi = "OBSCANCODI";
        public string Obsiteusuario = "OBSITEUSUARIO";
        public string Obsitefecha = "OBSITEFECHA";
        public string Canalnomb = "CANALNOMB";
        public string Canaliccp = "CANALICCP";
        public string Canalunidad = "CANALUNIDAD";
        public string Canalabrev = "CANALABREV";
        public string Canalpointtype = "CANALPOINTTYPE";
        public string Emprnomb = "EMPRENOMB";
        public string Zonanomb = "ZONANOMB";
        public string Obsitecomentarioagente = "OBSITECOMENTARIOAGENTE";

        #endregion

        public string SqlObtenerReporteSeniales
        {
            get { return base.GetSqlXml("ObtenerReporteSeniales"); }
        }

        #region "FIT Señales no Disponibles"

        public string Obsiteproceso = "OBSITEPROCESO";

        #endregion
    }
}

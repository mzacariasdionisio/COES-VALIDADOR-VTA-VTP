using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_MODELO_EMBALSE
    /// </summary>
    public class CmModeloEmbalseHelper : HelperBase
    {
        public CmModeloEmbalseHelper() : base(Consultas.CmModeloEmbalseSql)
        {
        }

        public CmModeloEmbalseDTO Create(IDataReader dr)
        {
            CmModeloEmbalseDTO entity = new CmModeloEmbalseDTO();

            int iModembcodi = dr.GetOrdinal(this.Modembcodi);
            if (!dr.IsDBNull(iModembcodi)) entity.Modembcodi = Convert.ToInt32(dr.GetValue(iModembcodi));

            int iRecurcodi = dr.GetOrdinal(this.Recurcodi);
            if (!dr.IsDBNull(iRecurcodi)) entity.Recurcodi = Convert.ToInt32(dr.GetValue(iRecurcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iModembindyupana = dr.GetOrdinal(this.Modembindyupana);
            if (!dr.IsDBNull(iModembindyupana)) entity.Modembindyupana = dr.GetString(iModembindyupana);

            int iModembfecvigencia = dr.GetOrdinal(this.Modembfecvigencia);
            if (!dr.IsDBNull(iModembfecvigencia)) entity.Modembfecvigencia = dr.GetDateTime(iModembfecvigencia);

            int iModembestado = dr.GetOrdinal(this.Modembestado);
            if (!dr.IsDBNull(iModembestado)) entity.Modembestado = dr.GetString(iModembestado);

            int iModembusucreacion = dr.GetOrdinal(this.Modembusucreacion);
            if (!dr.IsDBNull(iModembusucreacion)) entity.Modembusucreacion = dr.GetString(iModembusucreacion);

            int iModembfeccreacion = dr.GetOrdinal(this.Modembfeccreacion);
            if (!dr.IsDBNull(iModembfeccreacion)) entity.Modembfeccreacion = dr.GetDateTime(iModembfeccreacion);

            int iModembusumodificacion = dr.GetOrdinal(this.Modembusumodificacion);
            if (!dr.IsDBNull(iModembusumodificacion)) entity.Modembusumodificacion = dr.GetString(iModembusumodificacion);

            int iModembfecmodificacion = dr.GetOrdinal(this.Modembfecmodificacion);
            if (!dr.IsDBNull(iModembfecmodificacion)) entity.Modembfecmodificacion = dr.GetDateTime(iModembfecmodificacion);

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Modembcodi = "MODEMBCODI";
        public string Recurcodi = "RECURCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Modembindyupana = "MODEMBINDYUPANA";
        public string Modembfecvigencia = "MODEMBFECVIGENCIA";
        public string Modembestado = "MODEMBESTADO";
        public string Modembusucreacion = "MODEMBUSUCREACION";
        public string Modembfeccreacion = "MODEMBFECCREACION";
        public string Modembusumodificacion = "MODEMBUSUMODIFICACION";
        public string Modembfecmodificacion = "MODEMBFECMODIFICACION";
        public string Topcodi = "TOPCODI";

        public string Recurnombre = "RECURNOMBRE";
        public string Ptomedidesc = "PTOMEDIDESC";
        public string Ptomedielenomb = "PTOMEDIELENOMB";
        public string Ptomedibarranomb = "PTOMEDIBARRANOMB";

        #endregion

        public string SqlListHistorialXRecurso
        {
            get { return GetSqlXml("ListHistorialXRecurso"); }
        }

    }
}

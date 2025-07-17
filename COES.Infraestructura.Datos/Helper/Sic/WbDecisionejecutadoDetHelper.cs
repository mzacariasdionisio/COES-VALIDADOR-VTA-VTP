using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_DECISIONEJECUTADO_DET
    /// </summary>
    public class WbDecisionejecutadoDetHelper : HelperBase
    {
        public WbDecisionejecutadoDetHelper(): base(Consultas.WbDecisionejecutadoDetSql)
        {
        }

        public WbDecisionejecutadoDetDTO Create(IDataReader dr)
        {
            WbDecisionejecutadoDetDTO entity = new WbDecisionejecutadoDetDTO();

            int iDejdetcodi = dr.GetOrdinal(this.Dejdetcodi);
            if (!dr.IsDBNull(iDejdetcodi)) entity.Dejdetcodi = Convert.ToInt32(dr.GetValue(iDejdetcodi));

            int iDejdetdescripcion = dr.GetOrdinal(this.Dejdetdescripcion);
            if (!dr.IsDBNull(iDejdetdescripcion)) entity.Dejdetdescripcion = dr.GetString(iDejdetdescripcion);

            int iDejdetfile = dr.GetOrdinal(this.Dejdetfile);
            if (!dr.IsDBNull(iDejdetfile)) entity.Dejdetfile = dr.GetString(iDejdetfile);

            int iDesejecodi = dr.GetOrdinal(this.Desejecodi);
            if (!dr.IsDBNull(iDesejecodi)) entity.Desejecodi = Convert.ToInt32(dr.GetValue(iDesejecodi));

            int iDejdettipo = dr.GetOrdinal(this.Dejdettipo);
            if (!dr.IsDBNull(iDejdettipo)) entity.Dejdettipo = dr.GetString(iDejdettipo);

            int iDejdetestado = dr.GetOrdinal(this.Dejdetestado);
            if (!dr.IsDBNull(iDejdetestado)) entity.Dejdetestado = dr.GetString(iDejdetestado);

            int iDesdetextension = dr.GetOrdinal(this.Desdetextension);
            if (!dr.IsDBNull(iDesdetextension)) entity.Desdetextension = dr.GetString(iDesdetextension);

            return entity;
        }


        #region Mapeo de Campos

        public string Dejdetcodi = "DEJDETCODI";
        public string Dejdetdescripcion = "DEJDETDESCRIPCION";
        public string Dejdetfile = "DEJDETFILE";
        public string Desejecodi = "DESEJECODI";
        public string Dejdettipo = "DEJDETTIPO";
        public string Dejdetestado = "DEJDETESTADO";
        public string Desdetextension = "DESDETEXTENSION";

        #endregion

        public string SqlDeleteItem
        {
            get { return base.GetSqlXml("DeleteItem"); }
        }

        public string SqlActualizarDescripcion
        {
            get { return base.GetSqlXml("ActualizarDescripcion"); }
        }
    }
}

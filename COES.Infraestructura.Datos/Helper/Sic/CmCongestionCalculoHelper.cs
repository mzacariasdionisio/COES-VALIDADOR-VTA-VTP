using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_CONGESTION_CALCULO
    /// </summary>
    public class CmCongestionCalculoHelper : HelperBase
    {
        public CmCongestionCalculoHelper(): base(Consultas.CmCongestionCalculoSql)
        {
        }

        public CmCongestionCalculoDTO Create(IDataReader dr)
        {
            CmCongestionCalculoDTO entity = new CmCongestionCalculoDTO();

            int iCmcongcodi = dr.GetOrdinal(this.Cmcongcodi);
            if (!dr.IsDBNull(iCmcongcodi)) entity.Cmcongcodi = Convert.ToInt32(dr.GetValue(iCmcongcodi));

            int iConfigcodi = dr.GetOrdinal(this.Configcodi);
            if (!dr.IsDBNull(iConfigcodi)) entity.Configcodi = Convert.ToInt32(dr.GetValue(iConfigcodi));

            int iGrulincodi = dr.GetOrdinal(this.Grulincodi);
            if (!dr.IsDBNull(iGrulincodi)) entity.Grulincodi = Convert.ToInt32(dr.GetValue(iGrulincodi));

            int iRegsegcodi = dr.GetOrdinal(this.Regsegcodi);
            if (!dr.IsDBNull(iRegsegcodi)) entity.Regsegcodi = Convert.ToInt32(dr.GetValue(iRegsegcodi));

            int iCmconfecha = dr.GetOrdinal(this.Cmconfecha);
            if (!dr.IsDBNull(iCmconfecha)) entity.Cmconfecha = dr.GetDateTime(iCmconfecha);

            int iCmcongperiodo = dr.GetOrdinal(this.Cmcongperiodo);
            if (!dr.IsDBNull(iCmcongperiodo)) entity.Cmcongperiodo = Convert.ToInt32(dr.GetValue(iCmcongperiodo));

            int iCmgncorrelativo = dr.GetOrdinal(this.Cmgncorrelativo);
            if (!dr.IsDBNull(iCmgncorrelativo)) entity.Cmgncorrelativo = Convert.ToInt32(dr.GetValue(iCmgncorrelativo));

            int iCmconglimite = dr.GetOrdinal(this.Cmconglimite);
            if (!dr.IsDBNull(iCmconglimite)) entity.Cmconglimite = dr.GetDecimal(iCmconglimite);

            int iCmcongenvio = dr.GetOrdinal(this.Cmcongenvio);
            if (!dr.IsDBNull(iCmcongenvio)) entity.Cmcongenvio = dr.GetDecimal(iCmcongenvio);

            int iCmcongrecepcion = dr.GetOrdinal(this.Cmcongrecepcion);
            if (!dr.IsDBNull(iCmcongrecepcion)) entity.Cmcongrecepcion = dr.GetDecimal(iCmcongrecepcion);

            int iCmcongcongestion = dr.GetOrdinal(this.Cmcongcongestion);
            if (!dr.IsDBNull(iCmcongcongestion)) entity.Cmcongcongestion = dr.GetDecimal(iCmcongcongestion);

            int iCmconggenlimite = dr.GetOrdinal(this.Cmconggenlimite);
            if (!dr.IsDBNull(iCmconggenlimite)) entity.Cmconggenlimite = dr.GetDecimal(iCmconggenlimite);

            int iCmconggeneracion = dr.GetOrdinal(this.Cmconggeneracion);
            if (!dr.IsDBNull(iCmconggeneracion)) entity.Cmconggeneracion = dr.GetDecimal(iCmconggeneracion);

            int iCmcongusucreacion = dr.GetOrdinal(this.Cmcongusucreacion);
            if (!dr.IsDBNull(iCmcongusucreacion)) entity.Cmcongusucreacion = dr.GetString(iCmcongusucreacion);

            int iCmcongfeccreacion = dr.GetOrdinal(this.Cmcongfeccreacion);
            if (!dr.IsDBNull(iCmcongfeccreacion)) entity.Cmcongfeccreacion = dr.GetDateTime(iCmcongfeccreacion);

            int iCmcongusumodificacion = dr.GetOrdinal(this.Cmcongusumodificacion);
            if (!dr.IsDBNull(iCmcongusumodificacion)) entity.Cmcongusumodificacion = dr.GetString(iCmcongusumodificacion);

            int iCmcongfecmodificacion = dr.GetOrdinal(this.Cmcongfecmodificacion);
            if (!dr.IsDBNull(iCmcongfecmodificacion)) entity.Cmcongfecmodificacion = dr.GetDateTime(iCmcongfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Cmcongcodi = "CMCONGCODI";
        public string Configcodi = "CONFIGCODI";
        public string Grulincodi = "GRULINCODI";
        public string Regsegcodi = "REGSEGCODI";
        public string Cmconfecha = "CMCONFECHA";
        public string Cmcongperiodo = "CMCONGPERIODO";
        public string Cmgncorrelativo = "CMGNCORRELATIVO";
        public string Cmconglimite = "CMCONGLIMITE";
        public string Cmcongenvio = "CMCONGENVIO";
        public string Cmcongrecepcion = "CMCONGRECEPCION";
        public string Cmcongcongestion = "CMCONGCONGESTION";
        public string Cmconggenlimite = "CMCONGGENLIMITE";
        public string Cmconggeneracion = "CMCONGGENERACION";
        public string Cmcongusucreacion = "CMCONGUSUCREACION";
        public string Cmcongfeccreacion = "CMCONGFECCREACION";
        public string Cmcongusumodificacion = "CMCONGUSUMODIFICACION";
        public string Cmcongfecmodificacion = "CMCONGFECMODIFICACION";
        public string Famnomb = "FAMNOMB";
        public string Equinomb = "EQUINOMB";
        public string Congesfecinicio = "CONGESFECINICIO";
        public string Congesfecfin = "CONGESFECFIN";
        public string Tipo = "TIPO";

        #endregion

        public string SqlObtenerRegistroCongestion
        {
            get { return base.GetSqlXml("ObtenerRegistroCongestion"); }
        }

        public string SqlObtenerCongestionProceso
        {
            get { return base.GetSqlXml("ObtenerCongestionProceso"); }
        }

        public string SqlObtenerCongestionPorLinea
        {
            get { return base.GetSqlXml("ObtenerCongestionPorLinea"); }
        }
    }
}

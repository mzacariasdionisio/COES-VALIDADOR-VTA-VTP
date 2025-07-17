using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class GmmGarantiaHelper : HelperBase
    {
        public GmmGarantiaHelper()
            : base(Consultas.GmmGarantiaSql)
        {
        }
        #region Mapeo de Campos
        public string Garacodi = "GARACODI";
        public string Garafecinicio = "GARAFECINICIO";
        public string Garafecfin = "GARAFECFIN";
        public string Garamontogarantia = "GARAMONTOGARANTIA";
        public string Garaarchivo = "GARAARCHIVO";
        public string Garaestado = "GARAESTADO";
        public string Empgcodi = "EMPGCODI";
        public string Emprcodi = "EMPRCODI";
        public string Tcercodi = "TCERCODI";
        public string Tmodcodi = "TMODCODI";
        public string Garausucreacion = "GARAUSUCREACION";
        public string Garafeccreacion = "GARAFECCREACION";
        public string Garausumodificacion = "GARAUSUMODIFICACION";
        public string Garafecmodificacion = "GARAFECMODIFICACION";

        public string Pericodi = "pericodi";
        public string Mensaje1 = "mensaje1";
        public string Mensaje2 = "mensaje2";
        public string Mensaje3 = "mensaje3";

        public string Mensaje = "Mensaje";
        public string OrdenMensaje = "OrdenMensaje";

        public string TIPOCAMBIO = "TCAMBIO";
        public string SSCC = "SSCC";
        public string MRESERVA = "MRESERVA";
        public string TINFLEX = "TINFLEX";
        public string TEXCESO = "TEXCESO";

        public string EMPRESA = "EMPRESA";

        #endregion


        public GmmGarantiaDTO Create(IDataReader dr)
        {
            GmmGarantiaDTO entity = new GmmGarantiaDTO();
            #region Garantía
            int iGaracodi = dr.GetOrdinal(this.Garacodi);
            if (!dr.IsDBNull(iGaracodi)) entity.GARACODI = Convert.ToInt32(dr.GetValue(iGaracodi));

            int iGarafecinicio = dr.GetOrdinal(this.Garafecinicio);
            if (!dr.IsDBNull(iGarafecinicio)) entity.GARAFECINICIO = dr.GetDateTime(iGarafecinicio);

            int iGarafecfin = dr.GetOrdinal(this.Garafecfin);
            if (!dr.IsDBNull(iGarafecfin)) entity.GARAFECFIN = dr.GetDateTime(iGarafecfin);

            int iGaramontogarantia = dr.GetOrdinal(this.Garamontogarantia);
            if (!dr.IsDBNull(iGaramontogarantia)) entity.GARAMONTOGARANTIA = dr.GetDecimal(iGaramontogarantia);

            int iGaraarchivo = dr.GetOrdinal(this.Garaarchivo);
            if (!dr.IsDBNull(iGaraarchivo)) entity.GARAARCHIVO = dr.GetString(iGaraarchivo);

            int iGaraestado = dr.GetOrdinal(this.Garaestado);
            if (!dr.IsDBNull(iGaraestado)) entity.GARAESTADO = dr.GetString(iGaraestado);

            int iEmpgcodi = dr.GetOrdinal(this.Empgcodi);
            if (!dr.IsDBNull(iEmpgcodi)) entity.EMPGCODI = dr.GetInt32(iEmpgcodi);

            int iTcercodi = dr.GetOrdinal(this.Tcercodi);
            if (!dr.IsDBNull(iTcercodi)) entity.TCERCODI = dr.GetString(iTcercodi);

            int iTmodcodi = dr.GetOrdinal(this.Tmodcodi);
            if (!dr.IsDBNull(iTmodcodi)) entity.TMODCODI = dr.GetString(iTmodcodi);

            int iGarausucreacion = dr.GetOrdinal(this.Garausucreacion);
            if (!dr.IsDBNull(iGarausucreacion)) entity.GARAUSUCREACION = dr.GetString(iGarausucreacion);

            int iGarafeccreacion = dr.GetOrdinal(this.Garafeccreacion);
            if (!dr.IsDBNull(iGarafeccreacion)) entity.GARAFECCREACION = dr.GetDateTime(iGarafeccreacion);

            int iGarausumodificacion = dr.GetOrdinal(this.Garausumodificacion);
            if (!dr.IsDBNull(iGarausumodificacion)) entity.GARAUSUMODIFICACION = dr.GetString(iGarausumodificacion);

            int iGarafecmodificacion = dr.GetOrdinal(this.Garafecmodificacion);
            if (!dr.IsDBNull(iGarafecmodificacion)) entity.GARAFECMODIFICACION = dr.GetDateTime(iGarafecmodificacion);
            #endregion
            return entity;
        }
        public string SqlGetByEmpgcodi
        {
            get { return base.GetSqlXml("GetByEmpgcodi"); }
        }

        public string SqlGetListadoProcesamiento
        {
            get { return base.GetSqlXml("ListadoProcesamiento"); }
        }
        public string SqlGetListadoProcesamientoParticipante
        {
            get { return base.GetSqlXml("ListadoProcesamientoParticipante"); }
        }

        public GmmGarantiaDTO CreateListadoProcesamiento(IDataReader dr)
        {
            GmmGarantiaDTO entitygen = new GmmGarantiaDTO();

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entitygen.Pericodi = dr.GetInt16(iPericodi);

            int iEmpgCodi = dr.GetOrdinal(this.Empgcodi);
            if (!dr.IsDBNull(iEmpgCodi)) entitygen.EMPGCODI = dr.GetInt32(iEmpgCodi);

            int iEmprCodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprCodi)) entitygen.EMPRCODI = dr.GetInt32(iEmprCodi);

            int iMensaje1 = dr.GetOrdinal(this.Mensaje1);
            if (!dr.IsDBNull(iMensaje1)) entitygen.Mensaje1 = dr.GetString(iMensaje1);

            int iEmpresa = dr.GetOrdinal(this.EMPRESA);
            if (!dr.IsDBNull(iEmpresa)) entitygen.EMPRESA = dr.GetString(iEmpresa);

            int iMensaje2 = dr.GetOrdinal(this.Mensaje2);
            if (!dr.IsDBNull(iMensaje2)) entitygen.Mensaje2 = dr.GetString(iMensaje2);

            int iMensaje3 = dr.GetOrdinal(this.Mensaje3);
            if (!dr.IsDBNull(iMensaje3)) entitygen.Mensaje3 = dr.GetString(iMensaje3);

            int iTIPOCAMBIO = dr.GetOrdinal(this.TIPOCAMBIO);
            if (!dr.IsDBNull(iTIPOCAMBIO)) entitygen.TCAMBIO = dr.GetDecimal(iTIPOCAMBIO);

            int iSSCC = dr.GetOrdinal(this.SSCC);
            if (!dr.IsDBNull(iSSCC)) entitygen.SSCC = dr.GetDecimal(iSSCC);

            int iMRESERVA = dr.GetOrdinal(this.MRESERVA);
            if (!dr.IsDBNull(iMRESERVA)) entitygen.MRESERVA = dr.GetDecimal(iMRESERVA);

            int iTINFLEX = dr.GetOrdinal(this.TINFLEX);
            if (!dr.IsDBNull(iTINFLEX)) entitygen.TINFLEX = dr.GetDecimal(iTINFLEX);

            int iTEXCESO = dr.GetOrdinal(this.TEXCESO);
            if (!dr.IsDBNull(iTEXCESO)) entitygen.TEXCESO = dr.GetDecimal(iTEXCESO);

            return entitygen;
        }

    }
}

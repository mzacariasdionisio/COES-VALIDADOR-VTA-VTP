using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ST_FACTORPAGO
    /// </summary>
    public class StFactorpagoHelper : HelperBase
    {
        public StFactorpagoHelper()
            : base(Consultas.StFactorpagoSql)
        {
        }

        public StFactorpagoDTO Create(IDataReader dr)
        {
            StFactorpagoDTO entity = new StFactorpagoDTO();

            int iFacpagcodi = dr.GetOrdinal(this.Facpagcodi);
            if (!dr.IsDBNull(iFacpagcodi)) entity.Facpagcodi = Convert.ToInt32(dr.GetValue(iFacpagcodi));

            int iStrecacodi = dr.GetOrdinal(this.Strecacodi);
            if (!dr.IsDBNull(iStrecacodi)) entity.Strecacodi = Convert.ToInt32(dr.GetValue(iStrecacodi));

            int iStcntgcodi = dr.GetOrdinal(this.Stcntgcodi);
            if (!dr.IsDBNull(iStcntgcodi)) entity.Stcntgcodi = Convert.ToInt32(dr.GetValue(iStcntgcodi));

            int iStcompcodi = dr.GetOrdinal(this.Stcompcodi);
            if (!dr.IsDBNull(iStcompcodi)) entity.Stcompcodi = Convert.ToInt32(dr.GetValue(iStcompcodi));

            int iFacpagFggl = dr.GetOrdinal(this.Facpagfggl);
            if (!dr.IsDBNull(iFacpagFggl)) entity.Facpagfggl = dr.GetDecimal(iFacpagFggl);

            int iFacpagreajuste = dr.GetOrdinal(this.Facpagreajuste);
            if (!dr.IsDBNull(iFacpagreajuste)) entity.Facpagreajuste = dr.GetDecimal(iFacpagreajuste);

            int iFacpagfgglajuste = dr.GetOrdinal(this.Facpagfgglajuste);
            if (!dr.IsDBNull(iFacpagfgglajuste)) entity.Facpagfgglajuste = dr.GetDecimal(iFacpagfgglajuste);

            int iFacpagusucreacion = dr.GetOrdinal(this.Facpagusucreacion);
            if (!dr.IsDBNull(iFacpagusucreacion)) entity.Facpagusucreacion = dr.GetString(iFacpagusucreacion);

            int iFacpagfeccreacion = dr.GetOrdinal(this.Facpagfeccreacion);
            if (!dr.IsDBNull(iFacpagfeccreacion)) entity.Facpagfeccreacion = dr.GetDateTime(iFacpagfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Facpagcodi = "FACPAGCODI";
        public string Strecacodi = "STRECACODI";
        public string Stcntgcodi = "STCNTGCODI";
        public string Stcompcodi = "STCOMPCODI";
        public string Facpagfggl = "FACPAGFGGL";
        public string Facpagreajuste = "FACPAGREAJUSTE";
        public string Facpagfgglajuste = "FACPAGFGGLAJUSTE";
        public string Facpagusucreacion = "FACPAGUSUCREACION";
        public string Facpagfeccreacion = "FACPAGFECCREACION";
        //Variables para los reportes
        public string Equinomb = "EQUINOMB";
        public string Elecmpmonto = "ELECMPMONTO";
        public string Stcompcodelemento = "STCOMPCODELEMENTO";
        public string Pagasgcmggl = "PAGASGCMGGL";


        #endregion

        public string SqlGetByCriteriaInicialReporte
        {
            get { return base.GetSqlXml("GetByCriteriaInicialReporte"); }
        }

        public string SqlGetByCriteriaReporte
        {
            get { return base.GetSqlXml("GetByCriteriaReporte"); }
        }

        public string SqlGetByCriteriaReporteFactorPago
        {
            get { return base.GetSqlXml("GetByCriteriaReporteFactorPago"); }
        }


        public string SqlGetByIdFK
        {
            get { return base.GetSqlXml("GetByIdFK"); }
        }

        #region SIOSEIN2

        public string SqlObtenerFactorPagoParticipacion
        {
            get { return base.GetSqlXml("ObtenerFactorPagoParticipacion"); }
        }

        public string SqlObtenerCompensacionMensual
        {
            get { return base.GetSqlXml("ObtenerCompensacionMensual"); }
        }

        #endregion
    }
}

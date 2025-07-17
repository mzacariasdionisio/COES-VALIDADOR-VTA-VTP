using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_CALCULO_MENSUAL
    /// </summary>
    public class RerCalculoMensualHelper : HelperBase
    {
        public RerCalculoMensualHelper() : base(Consultas.RerCalculoMensualSql)
        {
        }

        public RerCalculoMensualDTO Create(IDataReader dr)
        {
            RerCalculoMensualDTO entity = new RerCalculoMensualDTO();

            int iRercmcodi = dr.GetOrdinal(this.Rercmcodi);
            if (!dr.IsDBNull(iRercmcodi)) entity.Rercmcodi = Convert.ToInt32(dr.GetValue(iRercmcodi));

            int iRerpprcodi = dr.GetOrdinal(this.Rerpprcodi);
            if (!dr.IsDBNull(iRerpprcodi)) entity.Rerpprcodi = Convert.ToInt32(dr.GetValue(iRerpprcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iRercmfatipintervalo = dr.GetOrdinal(this.Rercmfatipintervalo);
            if (!dr.IsDBNull(iRercmfatipintervalo)) entity.Rercmfatipintervalo = dr.GetString(iRercmfatipintervalo);

            int iRercmfafecintervalo = dr.GetOrdinal(this.Rercmfafecintervalo);
            if (!dr.IsDBNull(iRercmfafecintervalo)) entity.Rercmfafecintervalo = dr.GetDateTime(iRercmfafecintervalo);

            int iRercmfavalintervalo = dr.GetOrdinal(this.Rercmfavalintervalo);
            if (!dr.IsDBNull(iRercmfavalintervalo)) entity.Rercmfavalintervalo = dr.GetDecimal(iRercmfavalintervalo);

            int iRercmtaradj = dr.GetOrdinal(this.Rercmtaradj);
            if (!dr.IsDBNull(iRercmtaradj)) entity.Rercmtaradj = dr.GetDecimal(iRercmtaradj);

            int iRercmsummulinfa = dr.GetOrdinal(this.Rercmsummulinfa);
            if (!dr.IsDBNull(iRercmsummulinfa)) entity.Rercmsummulinfa = dr.GetDecimal(iRercmsummulinfa);

            int iRercminggarantizado = dr.GetOrdinal(this.Rercminggarantizado);
            if (!dr.IsDBNull(iRercminggarantizado)) entity.Rercminggarantizado = dr.GetDecimal(iRercminggarantizado);

            int iRercminsingpotencia = dr.GetOrdinal(this.Rercminsingpotencia);
            if (!dr.IsDBNull(iRercminsingpotencia)) entity.Rercminsingpotencia = dr.GetDecimal(iRercminsingpotencia);

            int iRercmsumfadivn = dr.GetOrdinal(this.Rercmsumfadivn);
            if (!dr.IsDBNull(iRercmsumfadivn)) entity.Rercmsumfadivn = dr.GetDecimal(iRercmsumfadivn);

            int iRercmingpotencia = dr.GetOrdinal(this.Rercmingpotencia);
            if (!dr.IsDBNull(iRercmingpotencia)) entity.Rercmingpotencia = dr.GetDecimal(iRercmingpotencia);

            int iRercmingprimarer = dr.GetOrdinal(this.Rercmingprimarer);
            if (!dr.IsDBNull(iRercmingprimarer)) entity.Rercmingprimarer = dr.GetDecimal(iRercmingprimarer);

            int iRercmingenergia = dr.GetOrdinal(this.Rercmingenergia);
            if (!dr.IsDBNull(iRercmingenergia)) entity.Rercmingenergia = dr.GetDecimal(iRercmingenergia);

            int iRercmsaldovtea = dr.GetOrdinal(this.Rercmsaldovtea);
            if (!dr.IsDBNull(iRercmsaldovtea)) entity.Rercmsaldovtea = dr.GetDecimal(iRercmsaldovtea);

            int iRercmsaldovtp = dr.GetOrdinal(this.Rercmsaldovtp);
            if (!dr.IsDBNull(iRercmsaldovtp)) entity.Rercmsaldovtp = dr.GetDecimal(iRercmsaldovtp);

            int iRercmtipocambio = dr.GetOrdinal(this.Rercmtipocambio);
            if (!dr.IsDBNull(iRercmtipocambio)) entity.Rercmtipocambio = dr.GetDecimal(iRercmtipocambio);

            int iRercmimcp = dr.GetOrdinal(this.Rercmimcp);
            if (!dr.IsDBNull(iRercmimcp)) entity.Rercmimcp = dr.GetDecimal(iRercmimcp);

            int iRercmsalmencompensar = dr.GetOrdinal(this.Rercmsalmencompensar);
            if (!dr.IsDBNull(iRercmsalmencompensar)) entity.Rercmsalmencompensar = dr.GetDecimal(iRercmsalmencompensar);

            int iRercmusucreacion = dr.GetOrdinal(this.Rercmusucreacion);
            if (!dr.IsDBNull(iRercmusucreacion)) entity.Rercmusucreacion = dr.GetString(iRercmusucreacion);

            int iRercmfeccreacion = dr.GetOrdinal(this.Rercmfeccreacion);
            if (!dr.IsDBNull(iRercmfeccreacion)) entity.Rercmfeccreacion = dr.GetDateTime(iRercmfeccreacion);

            return entity;
        }

        public RerCalculoMensualDTO CreateByAnioTarifario(IDataReader dr)
        {
            RerCalculoMensualDTO entity = Create(dr);

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            int iEquinomb = dr.GetOrdinal(this.Equinomb);
            if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

            int iRerpprmes = dr.GetOrdinal(this.Rerpprmes);
            if (!dr.IsDBNull(iRerpprmes)) entity.Rerpprmes = Convert.ToInt32(dr.GetValue(iRerpprmes));

            return entity;
        }

        #region Mapeo de Campos
        public string Rercmcodi = "RERCMCODI";
        public string Rerpprcodi = "RERPPRCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Rercmfatipintervalo = "RERCMFATIPINTERVALO";
        public string Rercmfafecintervalo = "RERCMFAFECINTERVALO";
        public string Rercmfavalintervalo = "RERCMFAVALINTERVALO";
        public string Rercmtaradj = "RERCMTARADJ";
        public string Rercmsummulinfa = "RERCMSUMMULINFA";
        public string Rercminggarantizado = "RERCMINGGARANTIZADO";
        public string Rercminsingpotencia = "RERCMINSINGPOTENCIA";
        public string Rercmsumfadivn = "RERCMSUMFADIVN";
        public string Rercmingpotencia = "RERCMINGPOTENCIA";
        public string Rercmingprimarer = "RERCMINGPRIMARER";
        public string Rercmingenergia = "RERCMINGENERGIA";
        public string Rercmsaldovtea = "RERCMSALDOVTEA";
        public string Rercmsaldovtp = "RERCMSALDOVTP";
        public string Rercmtipocambio = "RERCMTIPOCAMBIO";
        public string Rercmimcp = "RERCMIMCP";
        public string Rercmsalmencompensar = "RERCMSALMENCOMPENSAR";
        public string Rercmusucreacion = "RERCMUSUCREACION";
        public string Rercmfeccreacion = "RERCMFECCREACION";

        //Additional
        public string Reravcodi = "RERAVCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "EQUINOMB";
        public string Rerpprmes = "RERPPRMES";
        #endregion

        public string SqlDeleteByAnioVersion
        {
            get { return base.GetSqlXml("DeleteByAnioVersion"); }
        }

        public string SqlGetByAnioTarifario
        {
            get { return base.GetSqlXml("GetByAnioTarifario"); }
        }
        
    }
}


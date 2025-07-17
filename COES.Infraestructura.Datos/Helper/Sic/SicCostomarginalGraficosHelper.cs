using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class SicCostomarginalGraficosHelper : HelperBase
    {
        public SicCostomarginalGraficosHelper() : base(Consultas.SicCostomarginalGraficosSql)
        {
        }


        #region Mapeo de Campos

        public string Barrcodi = "BARRCODI";
        public string Barrnomb = "BARRNOMBRE";
        public string PeriCodi = "PERICODI";
        public string Version = "VERSION";
        public string TipCosto = "tipcosto";

        public string PericodiInicio = "pericodiInicio";
        public string PericodiFin = "pericodiFin";
        public string MesInicio = "mesInicio";
        public string MesFin = "mesFin";
        public string DiaInicio = "diaInicio";
        public string DiaFin = "diaFin";
        #region SIOSEIN
        public string Osinergcodi = "OSINERGCODI";
        #endregion

        #endregion

        public string SqlListarBarrasPorCostoMarginal
        {
            get { return base.GetSqlXml("ListarBarrasPorCostoMarginal"); }
        }
        public string SqlListarBarrasPorCostoMarginalProm
        {
            get { return base.GetSqlXml("ListarBarrasPorCostoMarginalProm"); }
        }
        public string SqlListarBarrasPorCostoMarginalMensual
        {
            get { return base.GetSqlXml("ListarBarrasPorCostoMarginalMensual"); }
        }
        public string SqlListarCostoMarginalTotalPorBarras
        {
            get { return base.GetSqlXml("ListarCostoMarginalTotalPorBarras"); }
        }
        public string SqlListarCostoMarginalTotalPorBarrasNew
        {
            get { return base.GetSqlXml("ListarCostoMarginalTotalPorBarras_NEW"); }
        }
        public string SqlListarCostoMarginalCongestionPorBarras
        {
            get { return base.GetSqlXml("ListarCostoMarginalCongestionPorBarras"); }
        }
        public string SqlListarCostoMarginalCongestionPorBarrasNew
        {
            get { return base.GetSqlXml("ListarCostoMarginalCongestionPorBarras_New"); }
        }
        public string SqlListarCostoMarginalDesviacion
        {
            get { return base.GetSqlXml("ListarCostoMarginalDesviacion"); }
        }
        public string SqlListarCostoMarginalCongestionDesviacion
        {
            get { return base.GetSqlXml("ListarCostoMarginalCongestionDesviacion"); }
        }
        public string SqlListarCostoMarginalEnergiaPorBarras
        {
            get { return base.GetSqlXml("ListarCostoMarginalEnergiaPorBarras"); }
        }
        public string SqlListarPromedioMarginalTotalDiario
        {
            get { return base.GetSqlXml("ListarPromedioMarginalTotalDiario"); }
        }
        public string SqlListarPromedioMarginalTotalMensual
        {
            get { return base.GetSqlXml("ListarPromedioMarginalTotalMensual"); }
        }
        public string SqlListarPromedioMarginalCongeneMensual
        {
            get { return base.GetSqlXml("ListarPromedioMarginalCongeneMensual"); }
        }
        public string SqListarPromedioMarginalCongeneDiario
        {
            get { return base.GetSqlXml("ListarPromedioMarginalCongeneDiario"); }
        }
        public string SqlListarBarrasPorCodigo
        {
            get { return base.GetSqlXml("ListarBarrasPorCodigo"); }
        }
    }
}

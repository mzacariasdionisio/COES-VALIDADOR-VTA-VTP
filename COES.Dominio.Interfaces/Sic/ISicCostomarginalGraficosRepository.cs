using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    public interface ISicCostomarginalGraficosRepository
    {
        List<BarraDTO> ListarBarrasPorCMG(CostoMarginalDTO parametro);
        List<BarraDTO> ListarBarrasPorCMGDiario(CostoMarginalDTO parametro);
        List<BarraDTO> ListarBarrasPorCMGMensual(CostoMarginalDTO parametro);
        List<BarraDTO> ListarBarrasPorArray(CostoMarginalDTO parametro);
        DataTable ListarCostoMarginalTotalPorBarras_NEW(CostoMarginalDTO parametro);
        List<CostoMarginalDTO> ListarCostoMarginalTotalPorBarras(CostoMarginalDTO parametro);
        List<CostoMarginalDTO> ListarCostoMarginalCongestionPorBarras(CostoMarginalDTO parametro);
        DataTable ListarCostoMarginalCongestionPorBarras_NEW(int tipoCosto, CostoMarginalDTO parametro);
        List<CostoMarginalDTO> ListarCostoMarginalEnergiaPorBarras(CostoMarginalDTO parametro);
        DataTable ListarCostoMarginalDesviacion(CostoMarginalDesviacionDTO parametro);
        DataTable ListarCostoMarginalCongestionDesviacion(CostoMarginalDesviacionDTO parametro);
        List<CostoMarginalGraficoValoresDTO> ListarPromedioMarginalTotalDiario(CostoMarginalDTO parametro);
        List<CostoMarginalGraficoValoresDTO> ListarPromedioMarginalTotalMensual(CostoMarginalDTO parametro);
        List<CostoMarginalGraficoValoresDTO> ListarPromedioMarginalCongeneMensual(CostoMarginalDTO parametro);
        List<CostoMarginalGraficoValoresDTO> ListarPromedioMarginalCongeneDiario(CostoMarginalDTO parametro);
        

    }
}

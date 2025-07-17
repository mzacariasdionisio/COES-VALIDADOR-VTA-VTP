using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace COES.Dominio.Interfaces.Sic
{
    public interface IDpoProcesoPronosticoRepository
    {
        List<MeMedicion48DTO> ObtenerGeneracionPorFechas(string str);
        List<DpoFormulaDTO> ObtenerFormulas();
        List<DpoEstimadorRawDTO> ObtenerDemandaPorFechas(
            string nomTabla, string fecIni, string fecFin, string listaIds);
        List<DpoRelacionScoIeod> RelacionScoIeod();
        List<MePtomedicionDTO> ListaBarras();
        List<PrnMedicion48DTO> ObtenerDemandaSRP(string fecIni,
            string fecFin, string prnmgrtipo, string vergrpcodi);
        List<DpoEstimadorRawDTO> ObtenerDemandaUltimaHora(
            string diaHora, string hora24, string dia, 
            string diaHoraSig, string listaIds);
    }
}

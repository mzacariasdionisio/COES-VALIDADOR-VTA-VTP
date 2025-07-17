using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace COES.Dominio.Interfaces.Transferencias
{
    public interface IActualizarTrasEmpFusionRepository
    {
        List<ActualizarTrasEmpFusionDTO> GetListaSaldosSobrantes(int? pericodi);
        ActualizarTrasEmpFusionDTO DeleteSaldosSobrantes();
        List<ActualizarTrasEmpFusionDTO> GetListaSaldosSobrantesVTP(int? pericodi);
        ActualizarTrasEmpFusionDTO SaveUpdate(ActualizarTrasEmpFusionDTO entity);
        List<ActualizarTrasEmpFusionDTO> GetLista(int? pericodi, string salsovteavtp);
        List<ActualizarTrasEmpFusionDTO> GetListaVTP(int? pericodi, string salsovteavtp);
        ActualizarTrasEmpFusionDTO SaveUpdateSaldos(ActualizarTrasEmpFusionDTO entity);
        ActualizarTrasEmpFusionDTO SaveUpdateSaldosVTP(ActualizarTrasEmpFusionDTO entity);
        List<ActualizarTrasEmpFusionDTO> GetListaSaldosNoIdentificados(int? pericodi);
        List<ActualizarTrasEmpFusionDTO> GetListaSaldosNoIdentificadosVTP(int? pericodi);
    }
}

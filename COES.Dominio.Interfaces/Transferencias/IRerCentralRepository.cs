using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RER_CENTRAL
    /// </summary>
    public interface IRerCentralRepository
    {
        int Save(RerCentralDTO entity);
        void Update(RerCentralDTO entity);
        void Delete(int rerCenCodi);
        RerCentralDTO GetById(int rerCenCodi);
        List<RerCentralDTO> List();
        List<RerCentralDTO> ListNombreCentralEmpresaBarra();
        List<RerCentralDTO> ListByFiltros(int equicodi, int emprcodi, int ptomedicodi, string fechaini, string fechafin, string estado, string codEntrega, int barrcodi);
        List<RerCentralDTO> ListCentralREREmpresas();
        List<RerCentralDTO> ListByEmprcodi(int emprcodi);
        List<RerCentralDTO> ListByEquiEmprFecha(int rercencodi, int equicodi, int emprcodi, string fechaini, string fechafin);
        List<RerCentralDTO> ListByFechasEstado(string rercenfechainicio, string rercenfechafin, string rercenestado);
        //CU21
        List<RerCentralDTO> ListCentralByFecha(DateTime dRerCenFecha);
        List<RerCentralDTO> ListCentralByFechaLVTP(DateTime dRerCenFecha);
        List<RerCentralDTO> ListCodigoEntregaYBarraTransferencia();


        List<RerCentralDTO> ListCentralByIds(string idsCentrales);
    }
}

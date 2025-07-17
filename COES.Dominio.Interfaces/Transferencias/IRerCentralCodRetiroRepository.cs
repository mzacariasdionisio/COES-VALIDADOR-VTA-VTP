using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RER_CENTRAL_CODRETIRO
    /// </summary>
    public interface IRerCentralCodRetiroRepository
    {
        int Save(RerCentralCodRetiroDTO entity);
        void Update(RerCentralCodRetiroDTO entity);
        void Delete(int rerCcrCodi);
        void DeleteAllByRerpprcodiRercencodi(int Rerpprcodi, int Rercencodi); 
        RerCentralCodRetiroDTO GetById(int rerCcrCodi);
        List<RerCentralCodRetiroDTO> List();
        List<RerCentralCodRetiroDTO> ListCantidadByRerpprcodi(int rerpprcodi);
        //CU21
        string ListaCodigoRetiroByEquipo(int iRerPPrCodi, int iEquiCodi);
    }
}

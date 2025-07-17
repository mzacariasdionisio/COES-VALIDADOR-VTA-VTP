using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RER_CENTRAL_LVTP
    /// </summary>
    public interface IRerCentralLvtpRepository
    {
        int Save(RerCentralLvtpDTO entity);
        void Update(RerCentralLvtpDTO entity);
        void Delete(int rerCtpCodi);
        RerCentralLvtpDTO GetById(int rerCtpCodi);
        List<RerCentralLvtpDTO> List();
        List<RerCentralLvtpDTO> ListByRercencodi(int Rercencodi);
        void DeleteAllByRercencodi(int Rercencodi); 
    }
}

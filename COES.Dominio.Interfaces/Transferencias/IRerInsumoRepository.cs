using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface IRerInsumoRepository
    {
        int Save(RerInsumoDTO entity);
        void Update(RerInsumoDTO entity);
        void Delete(int rerInsumoId);
        RerInsumoDTO GetById(int rerInsumoId);
        RerInsumoDTO GetByReravcodiByRerinstipinsumo(int Reravcodi, string Rerinstipinsumo);
        List<RerInsumoDTO> List();


        int GetIdPeriodoPmpoByAnioMes(int iPeriAnioMes);
        void TruncateTablaTemporal(string nombreTabla);
        void SaveTablaTemporal(RerInsumoTemporalDTO entity);
        void BulkInsertTablaTemporal(List<RerInsumoTemporalDTO> entitys, string nombreTabla);
        List<RerInsumoTemporalDTO> ListTablaTemporal();
    }
}


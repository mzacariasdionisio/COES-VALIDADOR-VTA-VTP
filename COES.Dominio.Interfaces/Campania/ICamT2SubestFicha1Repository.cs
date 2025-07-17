using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamT2SubestFicha1Repository
    {

        List<T2SubestFicha1DTO> GetT2SubestFicha1(int id);

        bool SaveT2SubestFicha1(T2SubestFicha1DTO T2SubestFicha1ADTO);

        bool DeleteT2SubestFicha1ById(int id, string usuario);

        int GetLastT2SubestFicha1Id();

        T2SubestFicha1DTO GetT2SubestFicha1ById(int id);

        List<T2SubestFicha1DTO> GetT2SubFicha1ByFilter(string plancodi, string empresa, string estado);

        List<T2SubestFicha1TransDTO> GetT2SubFicha1TransByFilter(string plancodi, string empresa, string estado);

        List<T2SubestFicha1EquiDTO> GetT2SubFicha1EquiByFilter(string plancodi, string empresa, string estado);

    }
}

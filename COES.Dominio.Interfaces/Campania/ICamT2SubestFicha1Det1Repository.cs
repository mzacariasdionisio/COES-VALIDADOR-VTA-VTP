using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamT2SubestFicha1Det1Repository
    {

        List<T2SubestFicha1Det1DTO> GetT2SubestFicha1Det1(int id);

        bool SaveT2SubestFicha1Det1(T2SubestFicha1Det1DTO T2SubestFicha1det1ADTO);

        bool DeleteT2SubestFicha1Det1ById(int id, string usuario);

        int GetLastT2SubestFicha1Det1Id();

        List<T2SubestFicha1Det1DTO> GetT2SubestFicha1Det1ById(int id);
    }
}

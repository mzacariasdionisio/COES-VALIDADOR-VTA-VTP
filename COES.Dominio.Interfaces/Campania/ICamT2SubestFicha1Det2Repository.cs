using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamT2SubestFicha1Det2Repository
    {

        List<T2SubestFicha1Det2DTO> GetT2SubestFicha1Det2(int id);

        bool SaveT2SubestFicha1Det2(T2SubestFicha1Det2DTO T2SubestFicha1det2ADTO);

        bool DeleteT2SubestFicha1Det2ById(int id, string usuario);

        int GetLastT2SubestFicha1Det2Id();

        List<T2SubestFicha1Det2DTO> GetT2SubestFicha1Det2ById(int id);
    }
}

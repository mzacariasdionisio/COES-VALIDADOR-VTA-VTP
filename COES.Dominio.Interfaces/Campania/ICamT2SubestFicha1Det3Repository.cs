using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamT2SubestFicha1Det3Repository
    {

        List<T2SubestFicha1Det3DTO> GetT2SubestFicha1Det3(int id);

        bool SaveT2SubestFicha1Det3(T2SubestFicha1Det3DTO T2SubestFicha1det3ADTO);

        bool DeleteT2SubestFicha1Det3ById(int id, string usuario);

        int GetLastT2SubestFicha1Det3Id();

        List<T2SubestFicha1Det3DTO> GetT2SubestFicha1Det3ById(int id);
    }
}

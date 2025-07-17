using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamSubestFicha1Det3Repository
    {

        List<SubestFicha1Det3DTO> GetSubestFicha1Det3(int id);

        bool SaveSubestFicha1Det3(SubestFicha1Det3DTO subestFicha1det3ADTO);

        bool DeleteSubestFicha1Det3ById(int id, string usuario);

        int GetLastSubestFicha1Det3Id();

        List<SubestFicha1Det3DTO> GetSubestFicha1Det3ById(int id);
    }
}

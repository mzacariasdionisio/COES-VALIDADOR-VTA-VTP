using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamSubestFicha1Det1Repository
    {

        List<SubestFicha1Det1DTO> GetSubestFicha1Det1(int id);

        bool SaveSubestFicha1Det1(SubestFicha1Det1DTO subestFicha1det1ADTO);

        bool DeleteSubestFicha1Det1ById(int id, string usuario);

        int GetLastSubestFicha1Det1Id();

        List<SubestFicha1Det1DTO> GetSubestFicha1Det1ById(int id);
    }
}

using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamSubestFicha1Repository
    {

        List<SubestFicha1DTO> GetSubestFicha1(int id);

        bool SaveSubestFicha1(SubestFicha1DTO subestFicha1ADTO);

        bool DeleteSubestFicha1ById(int id, string usuario);

        int GetLastSubestFicha1Id();

        SubestFicha1DTO GetSubestFicha1ById(int id);

    }
}

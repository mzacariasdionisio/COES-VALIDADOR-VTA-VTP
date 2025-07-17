using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamSubestFicha1Det2Repository
    {

        List<SubestFicha1Det2DTO> GetSubestFicha1Det2(int id);

        bool SaveSubestFicha1Det2(SubestFicha1Det2DTO subestFicha1det2ADTO);

        bool DeleteSubestFicha1Det2ById(int id, string usuario);

        int GetLastSubestFicha1Det2Id();

        List<SubestFicha1Det2DTO> GetSubestFicha1Det2ById(int id);
    }
}

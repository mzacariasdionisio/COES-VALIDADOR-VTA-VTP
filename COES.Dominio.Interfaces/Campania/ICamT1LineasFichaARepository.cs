using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamT1LineasFichaARepository
    {

        List<T1LinFichaADTO> GetLineasFichaACodi(int proyCodi);
        bool SaveLineasFichaA(T1LinFichaADTO T1LinFichaADTO);
        int GetLastLineasFichaAId();
        bool DeleteLineasFichaAById(int id, string usuario);
        T1LinFichaADTO GetLineasFichaAById(int id);
    }
}

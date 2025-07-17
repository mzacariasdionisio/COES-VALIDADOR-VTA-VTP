using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamT1LineasFichaADet1Repository
    {

        List<T1LinFichaADet1DTO> GetLineasFichaADet1Codi(int fichaACodi);
        bool SaveLineasFichaADet1(T1LinFichaADet1DTO T1LinFichaADet1DTO);
        int GetLastLineasFichaADet1Id();
        bool DeleteLineasFichaADet1ById(int id, string usuario);
        T1LinFichaADet1DTO GetLineasFichaADet1ById(int id);
    }
}

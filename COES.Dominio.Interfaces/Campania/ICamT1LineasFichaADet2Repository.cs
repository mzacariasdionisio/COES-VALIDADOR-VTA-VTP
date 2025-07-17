using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamT1LineasFichaADet2Repository
    {
        List<T1LinFichaADet2DTO> GetLineasFichaADet2Codi(int fichaACodi);
        bool SaveLineasFichaADet2(T1LinFichaADet2DTO T2LinFichaADet2DTO);
        int GetLastLineasFichaADet2Id();
        bool DeleteLineasFichaADet2ById(int id, string usuario);
        T1LinFichaADet2DTO GetLineasFichaADet2ById(int id);
    }
}

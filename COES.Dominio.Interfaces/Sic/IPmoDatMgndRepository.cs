using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPmoDatMgndRepository
    {
        List<PmoDatMgndDTO> List();
        List<PrGrupoDTO> ListBarraMgnd();
        PmoDatMgndDTO GetById(int id);
        int Update(PmoDatMgndDTO entity);
        List<PmoDatMgndDTO> ListDatMgnd();
        int CountDatMgnd(int periCodi);
    }
}

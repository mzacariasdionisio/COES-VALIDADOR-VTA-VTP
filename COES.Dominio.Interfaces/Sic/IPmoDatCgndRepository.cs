using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPmoDatCgndRepository
    {
        List<PmoDatCgndDTO> List();
        int Update(PmoDatCgndDTO entity);
        List<PrGrupoDTO> ListBarra();
        PmoDatCgndDTO GetById(int id);
        List<PmoDatCgndDTO> ListDatCgnd();
        int CountDatCgnd(int periCodi);
        List<PmoDatCgndDTO> ListGrupoCodig();
    }
}

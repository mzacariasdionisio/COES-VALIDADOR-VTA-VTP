using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface IInfoDesviacionRepository: IRepositoryBase
    {
        List<InfoDesviacionDTO> GetByListaCodigo(int iPeriCodi, int iVersion, int iBarrCodi);
        InfoDesviacionDTO GetByEnergiaByBarraCodigo(int iPeriCodi, int iVersion, int iBarrCodi, string sCodigo);
    }
}

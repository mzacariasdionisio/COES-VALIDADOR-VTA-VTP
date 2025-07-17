using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface IInfoFaltanteRepository: IRepositoryBase
    {
        List<InfoFaltanteDTO> GetByCriteria(Int32 PeriCodi);
        List<InfoFaltanteDTO> ListByPeriodoVersion(int iPericodi, int iVersion);
    }
}

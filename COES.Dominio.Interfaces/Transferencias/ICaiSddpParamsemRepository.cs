using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CAI_SDDP_PARAMSEM
    /// </summary>
    public interface ICaiSddpParamsemRepository
    {
        int Save(CaiSddpParamsemDTO entity);
        List<CaiSddpParamsemDTO> GetByCriteria(int caiajcodi);
        void Delete();
    }
}
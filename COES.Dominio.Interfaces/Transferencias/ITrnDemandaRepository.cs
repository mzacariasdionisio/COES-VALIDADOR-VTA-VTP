using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface ITrnDemandaRepository
    {
        int Save(TrnDemandaDTO entity);
        void Delete(TrnDemandaDTO entity);
        TrnDemandaDTO ObtenerTrnDemanda(string periodo, int emprcodi);
    }
}

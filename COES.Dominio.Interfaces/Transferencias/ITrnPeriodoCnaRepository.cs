using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface ITrnPeriodoCnaRepository : IRepositoryBase
    {
        int Save(TrnPeriodoCnaDTO entity);
        TrnPeriodoCnaDTO ObtenerSemana(string semanaperiodo);
        void Delete(TrnPeriodoCnaDTO entity);
    }
}

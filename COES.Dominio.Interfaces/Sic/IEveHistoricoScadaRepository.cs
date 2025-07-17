using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IEveHistoricoScadaRepository
    {
        int Save(EveHistoricoScadaDTO entity);
        List<EveHistoricoScadaDTO> List(int evencodi);
        void Delete(EveHistoricoScadaDTO entity);
        void DeleteAll(int evencodi);
        EveHistoricoScadaDTO GetById(int evehistscdacodi);
    }
}

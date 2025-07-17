using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IEveAnalisisEventoRepository
    {
        int Save(EveAnalisisEventoDTO entity);
        List<EveAnalisisEventoDTO> List(int evencodi);
        void Delete(int evencodi);
        void Update(EveAnalisisEventoDTO entity);
        void UpdateDescripcion(EveAnalisisEventoDTO entity);
        EveAnalisisEventoDTO GetById(int eveanaevecodi);
        bool ValidarTipoNumeralxAnalisisEvento(int evetipnumcodi);
        void DeleteAnalisis (int eveanaevecodi);
    }
}

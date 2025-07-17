using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IEveRecomobservRepository
    {
        int Save(EveRecomobservDTO entity);
        List<EveRecomobservDTO> List(int evencodi, int tipo);
        void Delete(int everecomobservcodi);
    }
}

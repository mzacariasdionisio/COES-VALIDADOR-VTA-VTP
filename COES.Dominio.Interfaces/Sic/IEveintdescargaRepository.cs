using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IEveintdescargaRepository
    {
        int Save(EveintdescargaDTO entity);
        List<EveintdescargaDTO> List(int evencodi, int tipo);
        void Delete(int evencodi, int tipo);
    }
}

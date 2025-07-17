using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IEveCondPreviaRepository
    {
        int Save(EveCondPreviaDTO entity);
        List<EveCondPreviaDTO> List(int evencodi, string tipo);
        void Delete(int evecondprcodi);
        EveCondPreviaDTO GetById(int evecondprcodi);
        void Update(EveCondPreviaDTO entity);
        EveCondPreviaDTO GetByIdCanalZona(int evencodi, string evecondprtipo, int zona, string evecondprcodigounidad);
    }
}

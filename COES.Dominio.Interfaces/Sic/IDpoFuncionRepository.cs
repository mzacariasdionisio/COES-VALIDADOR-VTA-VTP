using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IDpoFuncionRepository
    {
        void Save(DpoFuncionDTO entity);
        void Update(DpoFuncionDTO entity);
        void Delete(int id);
        DpoFuncionDTO GetById(int id);
        List<DpoFuncionDTO> List();
    }
}

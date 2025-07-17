using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IDpoEstadoVersionRepository
    {
        void Save(DpoEstadoVersionDTO entity);
        void Update(DpoEstadoVersionDTO entity);
        void Delete(int dpoevscodi);
        DpoEstadoVersionDTO GetById(int dpoevscodi);
        List<DpoEstadoVersionDTO> List();
    }
}

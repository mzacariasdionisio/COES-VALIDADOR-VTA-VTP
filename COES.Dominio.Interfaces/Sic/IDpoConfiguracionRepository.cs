using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IDpoConfiguracionRepository
    {
        void Save(DpoConfiguracionDTO entity);
        void Update(DpoConfiguracionDTO entity);
        void Delete(int dpocngcodi);
        DpoConfiguracionDTO GetById(int dpocngcodi);
        List<DpoConfiguracionDTO> List();
        DpoConfiguracionDTO GetByVersion(int vergrpcodi);
    }
}

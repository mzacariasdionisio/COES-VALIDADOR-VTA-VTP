using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface ICrEmpresaResponsableRepository
    {
        List<CrEmpresaResponsableDTO> ListrEmpresaResponsable(int cretapacodi);
        void Save(CrEmpresaResponsableDTO entity);
        bool ValidarEmpresaResponsable(int cretapacodi, int emprcodi);
        void Delete(int crrespemprcodi);
        void DeletexEtapa(int cretapacodi);
        List<CrEmpresaResponsableDTO> SqlObtenerEmpresaResponsablexEvento(int CREVENCODI);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnFormularelRepository
    {
        void Save(PrnFormularelDTO entity);
        void Update(PrnFormularelDTO entity);
        void Delete(int ptomedicodi, int ptomedicodicalc);
        List<PrnFormularelDTO> List();
        PrnFormularelDTO GetById(int ptomedicodi, int ptomedicodicalc);
        List<PrnFormularelDTO> ListFormulasByUsuario(string usuario);
        List<PrnFormularelDTO> ListFormulasRelacionadas(int ptomedicodi, string usuario);
        void DeleteByPtomedicodi(int ptomedicodi);
    }
}

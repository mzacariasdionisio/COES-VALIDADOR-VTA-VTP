using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnAgrupacionFormulasRepository
    {
        void Save(PrnAgrupacionFormulasDTO entity);
        void Delete(int agrupacion);
        List<PrnAgrupacionFormulasDTO> List();
        List<PrnAgrupacionFormulasDTO> GetListAgrupacionFormulasByAgrupacion(int agrupacion);
    }
}

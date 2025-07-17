using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnAreamedicionRepository
    {
        void Save(PrnAreamedicionDTO entity);
        void Update(PrnAreamedicionDTO entity);
        void Delete(int aremedcodi);
        void UpdateEstado(PrnAreamedicionDTO entity);
        List<PrnAreamedicionDTO> List();
        List<PrnAreamedicionDTO> ListVarexoCiudad();
    }    
}

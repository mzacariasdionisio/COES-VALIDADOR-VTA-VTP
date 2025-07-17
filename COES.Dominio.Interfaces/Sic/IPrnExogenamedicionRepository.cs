using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnExogenamedicionRepository
    {
        void Save(PrnExogenamedicionDTO entity);
        void Update(PrnExogenamedicionDTO entity);
        void Delete(int exmedicodi);
        List<PrnExogenamedicionDTO> List();
        PrnExogenamedicionDTO GetById(int varexocodi, int aremedcodi, DateTime fecha);
        List<PrnExogenamedicionDTO> ListExomedicionByCiudadDate(int aremedcodi, DateTime fecha);
        List<PrnHorasolDTO> ListHorasol();
    }
}

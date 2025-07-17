using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IDpoCasoRepository
    {
        int Save(DpoCasoDTO entity);
        void Update(DpoCasoDTO entity);
        void Delete(int id);
        DpoCasoDTO GetById(int id);
        List<DpoCasoDTO> List();
        List<DpoCasoDTO> Filter(string nombre, string areaOperativa, string usuario);
        List<DpoNombreCasoDTO> ListNombreCasos();
        List<DpoUsuarioDTO> ListUsuarios();
    }
}

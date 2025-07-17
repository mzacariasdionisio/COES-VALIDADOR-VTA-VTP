using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IDpoFeriadosRepository
    {
        void Save(DpoFeriadosDTO entity);
        void Update(DpoFeriadosDTO entity);
        void Delete(int id);
        List<DpoFeriadosDTO> List();
        DpoFeriadosDTO GetById(int id);
        List<DpoFeriadosDTO> GetByAnio(int anio);
        List<DpoFeriadosDTO> GetByFecha(string fecha);
        void UpdateById(int id, string descripcion, string spl, string sco, string fecha);
        List<DateTime> ObtenerFeriadosSpl();
        List<DateTime> ObtenerFeriadosSco();

        List<DateTime> ObtenerFeriadosPorAnio(int dpoferanio);

        List<DpoFeriadosDTO> GetByAnioRango(int anioIni, int anioFin);
    }
}

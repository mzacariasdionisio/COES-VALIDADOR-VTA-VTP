using COES.Dominio.DTO.Busqueda;
using COES.Dominio.Interfaces.Busqueda;
using COES.Infraestructura.Datos.Repositorio.Busqueda.BD;
using System.Linq;

namespace COES.Infraestructura.Datos.Repositorio.Busqueda
{
    public class BCDRangosRepository : IBCDRangosRepository
    {
        private readonly CSDocDJR _context = new CSDocDJR();
        private readonly BDMappers _bDMappers = new BDMappers();

        public BCDRangosDTO BuscarRango(string calificacion)
        {
            BCD_Rangos rango = _context.BCD_Rangos.FirstOrDefault(a => a.Range == calificacion);
            return _bDMappers.BCD_Rangos2RangoDTO(rango);
        }
    }
}

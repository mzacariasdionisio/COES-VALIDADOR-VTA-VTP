using COES.Dominio.DTO.Busqueda;
using COES.Dominio.Interfaces.Busqueda;
using COES.Infraestructura.Datos.Repositorio.Busqueda.BD;
using System.Linq;

namespace COES.Infraestructura.Datos.Repositorio.Busqueda
{
    public class BCDCalificacionBusquedaRepository : IBCDCalificacionBusquedaRepository
    {
        private readonly CSDocDJR _context = new CSDocDJR();
        private readonly BDMappers _bDMappers = new BDMappers();

        public void AddCalificacion(BCDCalificacionBusquedaDTO calificacion)
        {
            BCD_Calificacion_busqueda registro = _bDMappers.BCDCalificacionBusquedaDTO2BCD_Calificacion_busqueda(calificacion);
            _context.BCD_Calificacion_busqueda.Add(registro);
            _context.SaveChanges();
        }

        public void RemoveCalificacion(BCDCalificacionBusquedaDTO calificacion)
        {
            var existente = _context.BCD_Calificacion_busqueda
                .FirstOrDefault(c => c.Id_qualification == calificacion.Id_qualification);

            _context.BCD_Calificacion_busqueda.Remove(existente);
            _context.SaveChanges();
        }

        public void UpdateCalificacion(BCDCalificacionBusquedaDTO calificacion)
        {
            var existente = _context.BCD_Calificacion_busqueda
                .FirstOrDefault(c => c.Id_qualification == calificacion.Id_qualification);

            existente.Id_range_q = calificacion.Id_range_q;
            _context.SaveChanges();
        }

        public BCDCalificacionBusquedaDTO BuscarCalificacionPorIdBusqueda(int Id_search_q)
        {
            BCD_Calificacion_busqueda registro = _context.BCD_Calificacion_busqueda.FirstOrDefault(b => b.Id_search_q == Id_search_q);
            return _bDMappers.BCD_Calificacion_busqueda2BCDCalificacionBusquedaDTO(registro);
        }
    }
}

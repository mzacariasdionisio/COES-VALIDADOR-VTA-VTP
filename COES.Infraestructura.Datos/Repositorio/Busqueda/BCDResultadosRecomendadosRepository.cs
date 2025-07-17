using COES.Dominio.DTO.Busqueda;
using COES.Dominio.Interfaces.Busqueda;
using COES.Infraestructura.Datos.Repositorio.Busqueda.BD;
using System.Linq;

namespace COES.Infraestructura.Datos.Repositorio.Busqueda
{
    public class BCDResultadosRecomendadosRepository : IBCDResultadosRecomendadosRepository
    {
        private readonly CSDocDJR _context = new CSDocDJR();
        private readonly BDMappers _bDMappers = new BDMappers();

        public bool AddRecomendacion(BCDResultadosRecomendadosDTO resultado)
        {
            BCD_Resultados_recomendados registro = _bDMappers.BCDResultadosRecomendadosDTO2BCD_Resultados_recomendados(resultado);
            _context.BCD_Resultados_recomendados.Add(registro);
            _context.SaveChanges();
            return true;
        }

        public void DeleteRecomendacion(BCDResultadosRecomendadosDTO resultado)
        {
            var entidad = _context.BCD_Resultados_recomendados.Find(resultado.Id_document);
            if (entidad != null)
            {
                _context.BCD_Resultados_recomendados.Remove(entidad);
                _context.SaveChanges();
            }
        }

        public BCDResultadosRecomendadosDTO BuscarRecomendacion(BCDResultadosRecomendadosDTO resultado)
        {
            BCD_Resultados_recomendados registro = _context.BCD_Resultados_recomendados.FirstOrDefault(a => a.Id_search_rr == resultado.Id_search_rr &&
            a.Index_rowkey == resultado.Index_rowkey && a.Recommend_by == resultado.Recommend_by);

            return _bDMappers.BCD_Resultados_recomendados2BCDResultadosRecomendadosDTO(registro);
        }
    }
}

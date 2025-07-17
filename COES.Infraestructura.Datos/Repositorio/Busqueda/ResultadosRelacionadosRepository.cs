using COES.Dominio.DTO.Busqueda;
using COES.Dominio.Interfaces.Busqueda;
using COES.Infraestructura.Datos.Repositorio.Busqueda.BD;
using System.Linq;

namespace COES.Infraestructura.Datos.Repositorio.Busqueda
{
    public class ResultadosRelacionadosRepository : IResultadosRelacionadosRepository
    {
        private readonly CSDocDJR _context = new CSDocDJR();
        private readonly BDMappers _bDMappers = new BDMappers();

        public void RemoveRelacionado(BCDResultadosRecomendadosDTO registro)
        {
            Resultados_relacionados reg = _context.Resultados_relacionados.Find(registro.Id_document);
            _context.Resultados_relacionados.Remove(reg);
            _context.SaveChanges();
        }
        public void AddRelacionado(BCDResultadosRecomendadosDTO registro)
        {
            Resultados_relacionados reg = _bDMappers.BCDResultadosRecomendadosDTO2Resultados_relacionados(registro);
            _context.Resultados_relacionados.Add(reg);
            _context.SaveChanges();
        }

        public BCDResultadosRecomendadosDTO BuscarRelacionado(BCDResultadosRecomendadosDTO resultado)
        {
            Resultados_relacionados registro = _context.Resultados_relacionados.FirstOrDefault(a => a.Id_search_recommend == resultado.Id_search_rr &&
            a.Index_rowkey_recommend == resultado.Index_rowkey && a.Recommend_for == resultado.Recommend_by);
            return _bDMappers.Resultados_relacionados2BCDResultadosRecomendadosDTO(registro);
        }
    }
}

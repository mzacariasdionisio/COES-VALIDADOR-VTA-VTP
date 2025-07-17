using COES.Dominio.DTO.Busqueda;
using COES.Dominio.Interfaces.Busqueda;
using COES.Infraestructura.Datos.Repositorio.Busqueda.BD;
using System.Linq;

namespace COES.Infraestructura.Datos.Repositorio.Busqueda
{
    public class DocumentosAbiertosRepository : IDocumentosAbiertosRepository
    {
        private readonly CSDocDJR _context = new CSDocDJR();
        private readonly BDMappers _bDMappers = new BDMappers();

        public void AddDocumentoAbierto(BCDResultadosRecomendadosDTO result)
        {
            Documentos_abiertos registro = _bDMappers.BCDResultadosRecomendadosDTO2Documentos_abiertos(result);
            _context.Documentos_abiertos.Add(registro);
            _context.SaveChanges();
        }

        public BCDResultadosRecomendadosDTO BuscarDocumentoAbierto(BCDResultadosRecomendadosDTO result)
        {
            Documentos_abiertos registro = _context.Documentos_abiertos.FirstOrDefault(a => a.Id_search_open == result.Id_search_rr &&
            a.Index_rowkey_open == result.Index_rowkey && a.Open_by == result.Recommend_by);

            return _bDMappers.Documentos_abiertos2BCDResultadosRecomendadosDTO(registro);
        }
    }
}

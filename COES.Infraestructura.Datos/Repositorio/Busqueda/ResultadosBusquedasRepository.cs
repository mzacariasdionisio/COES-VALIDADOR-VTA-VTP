using COES.Dominio.DTO.Busqueda;
using COES.Dominio.Interfaces.Busqueda;
using COES.Infraestructura.Datos.Repositorio.Busqueda.BD;
using System.Collections.Generic;

namespace COES.Infraestructura.Datos.Repositorio.Busqueda
{
    public class ResultadosBusquedasRepository : IResultadosBusquedasRepository
    {
        private readonly CSDocDJR _context = new CSDocDJR();
        private readonly BDMappers _bDMappers = new BDMappers();

        public void GuardarTopResultados(List<BCDResultadosRecomendadosDTO> resultados)
        {
            foreach (var elemento in resultados)
            {
                Resultados_busquedas registro = _bDMappers.BCDResultadosRecomendadosDTO2Resultados_busquedas(elemento);
                _context.Resultados_busquedas.Add(registro);
            }
            _context.SaveChanges();
        }
    }
}

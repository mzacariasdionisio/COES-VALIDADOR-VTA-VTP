using COES.Dominio.DTO.Busqueda;
using COES.Dominio.Interfaces.Busqueda;
using COES.Infraestructura.Datos.Repositorio.Busqueda.BD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace COES.Infraestructura.Datos.Repositorio.Busqueda
{
    public class BCDBusquedasRepository : IBCDBusquedasRepository
    {
        private readonly CSDocDJR _context = new CSDocDJR();
        private readonly BDMappers _bDMappers = new BDMappers();

        public int Add(BCDBusquedasDTO busqueda)
        {
            busqueda.Search_date = DateTime.Now;
            BCD_Busquedas search = _bDMappers.BusquedaDtoToEntity(busqueda);
            _context.BCD_Busquedas.Add(search);
            _context.SaveChanges();
            return search.Id_search;
        }

        public void UpdateBusqueda(BCDBusquedasDTO registro)
        {
            BCD_Busquedas reg = _context.BCD_Busquedas.FirstOrDefault(b => b.Id_search == registro.Id_search);
            reg.Search_relation = registro.Search_relation;
            _context.SaveChanges();
        }

        public bool BuscarBusqueda(int idBusqueda)
        {
            return _context.BCD_Busquedas.Any(a => a.Id_search == idBusqueda);
        }

        public List<string> QuizaQuisoDecir(string termino)
        {
            try
            {
                var quizaQuisoDecir = _context.Database.SqlQuery<string>(
                $"SELECT search_Text FROM BCD_Busquedas WHERE DIFFERENCE(search_Text, '{termino}') > 2 and search_type = '1'"
                )
                .Distinct()
                .Take(10)
                .ToList();
                return quizaQuisoDecir;
            }
            catch
            {
                return null;
            }
        }

        public BCDBusquedasDTO BusquedaPorId(int idBusqueda)
        {
            BCD_Busquedas registro = _context.BCD_Busquedas.FirstOrDefault(b => b.Id_search == idBusqueda);
            return _bDMappers.BCD_Busquedas2BCDBusquedaDTO(registro);
        }

        public List<BCDBusquedasDTO> ObtenerBusquedas(DateTime start_date, DateTime end_date)
        {
            return _context.BCD_Busquedas
                .AsNoTracking()
                .Where(l => l.Search_date >= start_date && l.Search_date <= end_date)
                .Select(busqueda => new BCDBusquedasDTO
                {
                    Search_date = busqueda.Search_date,
                    Search_user = busqueda.Search_user,
                    Search_text = busqueda.Search_text,
                    Key_words = busqueda.Key_words,
                    Key_concepts = busqueda.Key_concepts,
                    Result_number = busqueda.Result_number,
                    Search_start_date = busqueda.Search_start_date,
                    Search_end_date = busqueda.Search_end_date,
                    Search_relation = busqueda.Search_relation,
                    Search_type = busqueda.Search_type
                })
                .ToList();
        }
    }
}

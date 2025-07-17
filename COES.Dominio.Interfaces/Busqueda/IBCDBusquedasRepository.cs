using COES.Dominio.DTO.Busqueda;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Busqueda
{
    public interface IBCDBusquedasRepository
    {
        int Add(BCDBusquedasDTO busqueda);
        bool BuscarBusqueda(int idBusqueda);
        BCDBusquedasDTO BusquedaPorId(int idBusqueda);
        List<BCDBusquedasDTO> ObtenerBusquedas(DateTime start_date, DateTime end_date);
        List<string> QuizaQuisoDecir(string term);
        void UpdateBusqueda(BCDBusquedasDTO registro);
    }
}

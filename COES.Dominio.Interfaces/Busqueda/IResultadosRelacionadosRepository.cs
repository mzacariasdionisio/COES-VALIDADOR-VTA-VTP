using COES.Dominio.DTO.Busqueda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Busqueda
{
    public interface IResultadosRelacionadosRepository
    {
        void AddRelacionado(BCDResultadosRecomendadosDTO registro);
        BCDResultadosRecomendadosDTO BuscarRelacionado(BCDResultadosRecomendadosDTO resultado);
        void RemoveRelacionado(BCDResultadosRecomendadosDTO registro);
    }
}

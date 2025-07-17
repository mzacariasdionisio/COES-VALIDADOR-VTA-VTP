using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la vista EQ_AREA
    /// </summary>
    public interface IAreaRepository
    {
        AreaDTO GetById(System.Int32 id);
        List<AreaDTO> List();
        List<AreaDTO> GetByCriteria(string nombre);
        List<AreaDTO> ListArea();
        List<AreaDTO> ListAreaProceso();
    }
}


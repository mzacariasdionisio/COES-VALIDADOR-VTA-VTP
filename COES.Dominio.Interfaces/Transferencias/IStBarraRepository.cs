using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ST_RECALCULO
    /// </summary>

    public interface IStBarraRepository
    {

        int Save(StBarraDTO entity);
        void Update(StBarraDTO entity);
        void Delete(int strecacodi);
        void DeleteVersion(int strecacodi);
        List<StBarraDTO> List(int strecacodi);
        StBarraDTO GetById(int stbarrcodi);
        List<StBarraDTO> GetByCriteria(int strecacodi);
        List<StBarraDTO> ListByStBarraVersion(int strecacodi);
        void DeleteDstEleDet(int barrcodi, int strecacodi);
        //StBarraDTO GetBySisBarrNomb(string BarrNomb);
        
    }
}

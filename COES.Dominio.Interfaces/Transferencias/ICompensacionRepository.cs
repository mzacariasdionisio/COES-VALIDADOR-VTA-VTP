using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_CABE_COMPENSACION
    /// </summary>
    public interface ICompensacionRepository
    {
        int Save(CompensacionDTO entity);
        void Update(CompensacionDTO entity);
        void Delete(System.Int32 id);
        CompensacionDTO GetById(System.Int32 id);
        CompensacionDTO GetByCodigo(string nombre, int pericodi);
        List<CompensacionDTO> List(int id);
        List<CompensacionDTO> GetByCriteria(string nombre);
        List<CompensacionDTO> ListBase(int pericodi);
        List<CompensacionDTO> ListReporte(int pericodi);
    }
}

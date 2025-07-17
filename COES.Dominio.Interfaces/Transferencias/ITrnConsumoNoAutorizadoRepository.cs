using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface ITrnConsumoNoAutorizadoRepository : IRepositoryBase
    {
        int Save(TrnConsumoNoAutorizadoDTO entity);
        void Delete(int emprcodi, string fechacna);
        List<TrnConsumoNoAutorizadoDTO> ListTrnConsumoNoAutorizado(string fechainicio, string fechafin);
    }
}

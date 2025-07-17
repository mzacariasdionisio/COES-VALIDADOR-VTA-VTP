using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EQ_CATEGORIA_DETALLE
    /// </summary>
    public interface IEqCategoriaDetRepository
    {
        int Save(EqCategoriaDetDTO entity);
        void Update(EqCategoriaDetDTO entity);
        void Delete(int ctgdetcodi);
        EqCategoriaDetDTO GetById(int ctgdetcodi);
        List<EqCategoriaDetDTO> ListByCategoriaAndEstado(int ctgcodi, string estado);        
        List<EqCategoriaDetDTO> ListByCategoriaAndEstadoAndEmpresa(int ctgcodi, string estado, int emprcodi);
        List<EqCategoriaDetDTO> GetByCriteria(int ctgcodi);
    }
}

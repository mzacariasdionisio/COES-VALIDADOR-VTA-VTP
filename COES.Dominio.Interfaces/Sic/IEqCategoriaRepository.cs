using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EQ_CATEGORIA
    /// </summary>
    public interface IEqCategoriaRepository
    {
        int Save(EqCategoriaDTO entity);
        void Update(EqCategoriaDTO entity);
        void Delete(int ctgcodi);
        EqCategoriaDTO GetById(int ctgcodi);
        List<EqCategoriaDTO> ListPadre(int famcodi, int ctgcodi);
        List<EqCategoriaDTO> ListByFamiliaAndEstado(int famcodi, string estado);
        List<EqCategoriaDTO> ListaCategoriaClasificacionByFamiliaAndEstado(int famcodi, string estado);
        List<EqCategoriaDTO> ListCategoriaHijoByIdPadre(int famcodi, int ctgpadrecodi);
        List<EqCategoriaDTO> ListCategoriaHijoByIdPadreAndEmpresa(int famcodi, int ctgpadrecodi, int emprcodi);
        List<EqCategoriaDTO> GetByCriteriaEqCategorias();
    }
}

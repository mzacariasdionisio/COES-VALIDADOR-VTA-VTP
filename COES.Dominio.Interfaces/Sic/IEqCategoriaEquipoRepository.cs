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
    /// Interface de acceso a datos de la tabla EQ_CATEGORIA_EQUIPO
    /// </summary>
    public interface IEqCategoriaEquipoRepository
    {
        void Save(EqCategoriaEquipoDTO entity);
        void Update(EqCategoriaEquipoDTO entity);
        void Delete(int ctgequicodi);
        EqCategoriaEquipoDTO GetById(int ctgdetcodi, int equicodi);
        EqCategoriaEquipoDTO GetByIdEquipo(int ctgcodi, int equicodi);
        List<EqCategoriaEquipoDTO> List();
        List<EqCategoriaEquipoDTO> ListaPaginado(int iEmpresa, int iFamilia, int iTipoEmpresa, int iEquipo, int iCategoria, int iSubclasificacion, string nombre, int nroPagina, int nroFilas);
        int TotalClasificacion(int iEmpresa, int iFamilia, int iTipoEmpresa, int iEquipo, int iCategoria, int iSubclasificacion, string nombre);
        List<EqCategoriaEquipoDTO> ListaClasificacionByCategoriaAndEquipo(int ctgdetcodi, int equicodi);
        List<EqCategoriaEquipoDTO> ListaClasificacionByCategoriaAndEmpresa(int ctgdetcodi, int emprcodi);
        List<EqCategoriaEquipoDTO> ListaClasificacionByCategoriaPadreAndEquipo(int ctgpadrecodi, int equicodi);
        //inicio agregado
        List<EqCategoriaEquipoDTO> ListaClasificacionByCategoriaDetalle(int ctgdetcodi);
        //fin agregado
    }
}

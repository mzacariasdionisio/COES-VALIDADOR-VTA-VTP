using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_ING_POTENCIA
    /// </summary>
    public interface IIngresoPotenciaRepository
    {
        int Save(IngresoPotenciaDTO entity);
        void Delete(System.Int32 PeriCodi, System.Int32 IngrPoteVersion);
        List<IngresoPotenciaDTO> GetByCodigo(int? pericodi,int? version);
        List<IngresoPotenciaDTO> GetByCriteria(int pericodi, int version);
        List<IngresoPotenciaDTO> ListByPeriodoVersion(int iPericodi, int iVersion);
        IngresoPotenciaDTO GetByPeriodoVersionEmpresa(int iPericodi, int iIngrPoteVersion, int iEmprCodi);
    }
}


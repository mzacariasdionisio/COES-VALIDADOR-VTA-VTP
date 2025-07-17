using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla DAI_APORTANTE
    /// </summary>
    public interface IDaiAportanteRepository
    {
        int Save(DaiAportanteDTO entity);
        void Update(DaiAportanteDTO entity);
        void Delete(int aporcodi);
        void DeleteByPresupuesto(DaiAportanteDTO aportante);
        DaiAportanteDTO GetById(int aporcodi);
        List<DaiAportanteDTO> List();
        List<DaiAportanteDTO> ListCuadroDevolucion(decimal igv, int anio, int estado);
        List<DaiAportanteDTO> GetByCriteria(int prescodi, int tabcdcodiestado);
        List<DaiAportanteDTO> GetByCriteriaAportanteLiquidacion(int prescodi, int tabcdcodiestado);
        List<DaiAportanteDTO> GetByCriteriaAportanteCronograma(int anio, string tabcdcodiestado);
        List<DaiAportanteDTO> GetByCriteriaAniosCronograma(int prescodi, int estado);
        List<DaiAportanteDTO> GetByCriteriaFinalizar(DaiAportanteDTO aportante);
    }
}

using System.Collections.Generic;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IIO_PERIODO_SICLI
    /// </summary>
    public interface IIioPeriodoSicliRepository
    {
        List<IioPeriodoSicliDTO> GetByCriteria(string anio);
        IioPeriodoSicliDTO GetById(IioPeriodoSicliDTO iioPeriodoSicliDto);
        void Save(IioPeriodoSicliDTO iioPeriodoSicliDto);
        List<string> ListAnios();
        void Update(IioPeriodoSicliDTO iioPeriodoSicliDto);

        //- pr16.HDT - 01/04/2018: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite listar los periodos Sicli activos.
        /// </summary>
        /// <returns></returns>
        List<IioPeriodoSicliDTO> ListaPeriodoActivo();


        //- pr16.HDT - 01/04/2018: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener el periodo de acuerdo con su código.
        /// </summary>
        /// <param name="pSicliCodi"></param>
        /// <returns></returns>
        IioPeriodoSicliDTO PeriodoGetByCodigo(int pSicliCodi);

        /// <summary>
        /// Permite obtener un registro en la tabla IIO_PERIODO_SICLI segun el periodo
        /// </summary>
        /// <param name="periodoSicli"></param>
        /// <returns></returns>
        IioPeriodoSicliDTO PeriodoGetByCodigo(string periodoSicli);
    }
}
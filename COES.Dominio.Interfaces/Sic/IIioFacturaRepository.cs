using System.Collections.Generic;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IIO_FACTURA
    /// </summary>
    public interface IIioFacturaRepository
    {
        void BulkInsert(List<IioFacturaDTO> entitys);
        void Delete(int psiclicodi, int emprcodi);

        //- alpha.HDT - 09/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite crear nuestros registros de la tabla factura.
        /// </summary>
        /// <param name="entities"></param>
        void Save(List<IioFacturaDTO> entities);
    }
}
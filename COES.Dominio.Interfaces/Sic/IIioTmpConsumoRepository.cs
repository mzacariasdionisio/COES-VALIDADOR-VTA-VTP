using System.Collections.Generic;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IIO_TMP_CONSUMO
    /// </summary>
    public interface IIioTmpConsumoRepository
    {
        void BulkInsert(List<IioTmpConsumoDTO> entitys);
        void Delete();
        string MigrateMeMedicion96(int lectCodi, int tipoInfoCodi, string periodo);

        //- alpha.HDT - 10/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite actualizar la columna IIO_TMP_CONSUMO.PTOMEDICODI para realizar la migración desde la tabla en cuestión
        /// hacia la tabla ME_MEDICION96.
        /// </summary>
        void UpdatePtoMediCodiTmpConsumo();

    }
}

using System.Collections.Generic;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IIO_OSIG_CONSUMO_UL
    /// </summary>
    public interface IIioOsigConsumoUlRepository
    {
        void BulkInsert(List<IioOsigConsumoUlDTO> entitys);
        void Delete(int psiclicodi, string empresa);
        void MigrateMeMedicion96(int lectCodi, int tipoInfoCodi, string periodo, int psiclicodi, string empresa);

        //- alpha.HDT - 10/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite actualizar la columna IIO_OSIG_CONSUMO_UL.PTOMEDICODI para realizar la migración desde la tabla en cuestión
        /// hacia la tabla ME_MEDICION96.
        /// </summary>
        void UpdatePtoMediCodiOsigConsumo(int emprcodisuministrador, int psiclicodi, string empresa);

        string ValidarMigracionMeMedicion96(int psiclicodi, string empresa);

        void GenerarOsigConsumoLogImportacionPtoMedicion(int psiclicodi, string periodo, string usuario, string tabla, string empresas);

        void SaveOsigConsumo(string usuario);
    }
}

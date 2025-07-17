using System.Collections.Generic;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IIO_OSIG_SUMINISTRO_UL
    /// </summary>
    public interface IIioOsigSuministroUlRepository
    {
        void BulkInsert(List<IioOsigSuministroUlDTO> entitys);
        void Delete(int psiclicodi, string empresa);
        void UpdateOsigSuministro(int psiclicodi, string empresa);
        string ValidarOsigSuministroEquipos(int psiclicodi, string empresa);

        void GenerarOsigSuministroLogImportacionEquipo(int psiclicodi, string periodo, string usuario, string tabla, string empresas);
    }
}

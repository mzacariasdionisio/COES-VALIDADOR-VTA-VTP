using System.Collections.Generic;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IIO_OSIG_FACTURA_UL
    /// </summary>
    public interface IIioOsigFacturaUlRepository
    {
        void BulkInsert(List<IioOsigFacturaUlDTO> entitys);
        void Delete(int psiclicodi, string ulfactcodempresa);
               
        /// <summary>
        /// Permite crear nuestros registros de la tabla factura.
        /// </summary>
        /// <param name="entities"></param>
        void Save(List<IioOsigFacturaUlDTO> entities);

        void UpdatePtoMediCodiOsigFactura(int emprcodisuministrador, int psiclicodi, string empresa);

        void UpdateEmprcodiOsigFactura(int emprcodisuministrador, int psiclicodi, string empresa);

        string ValidarOsigFacturaPuntoMedicion(int psiclicodi, string empresa);

        void SaveIioFactura(int psiclicodi, string empresa);

        void GenerarOsigFacturaLogImportacionEmpresa(int psiclicodi, string periodo, string usuario, string tabla, string empresas);

        void GenerarOsigFacturaLogImportacionPtoMedicion(int psiclicodi, string periodo, string usuario, string tabla, string empresas);

        void ActualizarControlImportacionNoOK(int psiclicodi, string empresas, string usuario, string tabla);

        void ActualizarControlImportacionOK(int psiclicodi, string empresas, string usuario, string tabla);
        void ActualizarRegistrosImportacion(int psiclicodi, string empresas, string usuario, string tabla);
        void ActualizarPeridoFechaSincCoes(int psiclicodi);
        int ValidarOsigFacturaTablaEmpresas(int psiclicodi, string empresa);
    }
}
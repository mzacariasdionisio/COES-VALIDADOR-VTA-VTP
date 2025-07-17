using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VTP_PEAJE_EGRESO_DETALLE
    /// </summary>
    public interface IVtpPeajeEgresoDetalleRepository
    {
        int Save(VtpPeajeEgresoDetalleDTO entity);
        void Update(VtpPeajeEgresoDetalleDTO entity);
        void Delete(int pegrdcodi);
        VtpPeajeEgresoDetalleDTO GetById(int pegrdcodi);
        List<VtpPeajeEgresoDetalleDTO> List();
        List<VtpPeajeEgresoDetalleDTO> GetByCriteria(int pegrcodi);
        List<VtpPeajeEgresoDetalleDTO> GetByCriteriaPeriodoAnterior(int emprcodi, int periCodi);
        List<VtpPeajeEgresoDetalleDTO> ListView(int pegrcodi);
        VtpPeajeEgresoDetalleDTO GetByIdMinfo(int pegrcodi, int emprcodi, int barrcodi, string pegrdtipousuario);
        void DeleteByCriteria(int pericodi, int recpotcodi);
        List<VtpPeajeEgresoDetalleDTO> ListarCodigosByEmprcodi(int emprcodi, int periCodi);
        VtpPeajeEgresoDetalleDTO GetByPegrCodiAndCodVtp(int pegrcodi, string coregecodvtp);
    }
}

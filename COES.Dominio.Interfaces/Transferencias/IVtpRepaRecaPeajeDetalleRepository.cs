using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VTP_REPA_RECA_PEAJE_DETALLE
    /// </summary>
    public interface IVtpRepaRecaPeajeDetalleRepository
    {
        int Save(VtpRepaRecaPeajeDetalleDTO entity);
        void Update(VtpRepaRecaPeajeDetalleDTO entity);
        void Delete(int rrpdcodi);
        VtpRepaRecaPeajeDetalleDTO GetById(int rrpdcodi);
        List<VtpRepaRecaPeajeDetalleDTO> List(int pericodi, int recpotcodi, int rrpecodi);
        List<VtpRepaRecaPeajeDetalleDTO> GetByCriteria(int pericodi,int recpotcodi);
        int GetMaxNumEmpresas(int pericodi, int recpotcodi);
        void DeleteByCriteria(int pericodi, int recpotcodi);
        void DeleteByCriteriaRRPE(int pericodi, int recpotcodi, int rrpecodi);
    }
}

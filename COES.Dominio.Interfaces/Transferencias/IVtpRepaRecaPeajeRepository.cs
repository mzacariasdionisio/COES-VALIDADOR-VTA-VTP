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
    /// Interface de acceso a datos de la tabla VTP_REPA_RECA_PEAJE
    /// </summary>
    public interface IVtpRepaRecaPeajeRepository
    {
        int Save(VtpRepaRecaPeajeDTO entity);
        void Update(VtpRepaRecaPeajeDTO entity);
        void Delete(int pericodi, int recpotcodi, int rrpecodi);
        VtpRepaRecaPeajeDTO GetById(int pericodi, int recpotcodi, int rrpecodi);
        List<VtpRepaRecaPeajeDTO> List();
        List<VtpRepaRecaPeajeDTO> GetByCriteria(int pericodi,int recpotcodi);
        VtpRepaRecaPeajeDTO GetByNombre(int pericodi, int recpotcodi, string rrpenombre);
        void DeleteByCriteria(int pericodi, int recpotcodi);
    }
}

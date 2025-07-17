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
    /// Interface de acceso a datos de la tabla VTP_PEAJE_EMPRESA
    /// </summary>
    public interface IVtpPeajeEmpresaRepository
    {
        int Save(VtpPeajeEmpresaDTO entity);
        void Update(VtpPeajeEmpresaDTO entity);
        void Delete(int pempcodi);
        VtpPeajeEmpresaDTO GetById(int pempcodi);
        List<VtpPeajeEmpresaDTO> List();
        List<VtpPeajeEmpresaDTO> GetByCriteria();
        void DeleteByCriteria(int pericodi, int recpotcodi);
    }
}

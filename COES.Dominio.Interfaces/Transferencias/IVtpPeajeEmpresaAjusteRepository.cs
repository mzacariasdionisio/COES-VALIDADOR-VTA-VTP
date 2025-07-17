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
    /// Interface de acceso a datos de la tabla VTP_PEAJE_EMPRESA_AJUSTE
    /// </summary>
    public interface IVtpPeajeEmpresaAjusteRepository
    {
        int Save(VtpPeajeEmpresaAjusteDTO entity);
        void Update(VtpPeajeEmpresaAjusteDTO entity);
        void Delete(int pempajcodi);
        void DeleteByCriteria(int pericodi);
        VtpPeajeEmpresaAjusteDTO GetById(int pempajcodi);
        List<VtpPeajeEmpresaAjusteDTO> List();
        List<VtpPeajeEmpresaAjusteDTO> GetByCriteria(int pericodi);
        decimal GetAjuste(int pericodi, int emprcodipeaje, int pingcodi, int emprcodicargo);
    }
}

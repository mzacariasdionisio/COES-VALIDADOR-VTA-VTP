using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RER_INSUMO_VTP
    /// </summary>
    public interface IRerInsumoVtpRepository
    {
        int Save(RerInsumoVtpDTO entity);
        void Update(RerInsumoVtpDTO entity);
        void Delete(int rerInsumoVtpId);
        RerInsumoVtpDTO GetById(int rerInsumoVtpId);
        List<RerInsumoVtpDTO> List();
        void DeleteByParametroPrimaAndMes(int iRerpprcodi, int iRerpprmes);
        List<RerInsumoVtpDTO> GetByPeriodo(int reravcodi, string rerpprmes);
        decimal ObtenerSaldoVtpByInsumoVTP(int iRerpprcodi, int iEmprcodi, int iEquicodi);
    }
}


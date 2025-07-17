using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AF_HORA_COORD
    /// </summary>
    public interface IAfHoraCoordRepository
    {
        int Save(AfHoraCoordDTO entity);
        void Update(AfHoraCoordDTO entity);
        void Delete(int afhocodi);
        AfHoraCoordDTO GetById(int afhocodi);
        List<AfHoraCoordDTO> List();
        List<AfHoraCoordDTO> GetByCriteria();

        #region Intranet CTAF
        List<AfHoraCoordDTO> ListHoraCoord(int afecodi, int fdatcodi);
        List<AfHoraCoordDTO> ListHoraCoordSuministradora(int afecodi);
        List<AfHoraCoordDTO> ListHoraCoordCtaf(string afeanio, string afecorr, int fdatcodi);
        void DeleteHoraCoord(int afecodi, int fdatcodi, int emprcodi, IDbConnection connection, IDbTransaction transaction);
        int Save(AfHoraCoordDTO horacoord, IDbConnection connection, IDbTransaction transaction);
        void UpdateHoraCoordSuministradora(AfHoraCoordDTO entity, IDbConnection connection, IDbTransaction transaction);
        List<AfHoraCoordDTO> ListEmpClixSuministradora(string eracmfsuministrador);
        #endregion
    }
}

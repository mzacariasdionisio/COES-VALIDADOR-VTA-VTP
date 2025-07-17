using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AF_INTERRUP_SUMINISTRO
    /// </summary>
    public interface IAfInterrupSuministroRepository
    {
        int Save(AfInterrupSuministroDTO entity);
        void Update(AfInterrupSuministroDTO entity);
        void Delete(int intsumcodi);
        AfInterrupSuministroDTO GetById(int intsumcodi);
        List<AfInterrupSuministroDTO> List();
        List<AfInterrupSuministroDTO> GetByCriteria(int afecodi, int emprcodi, int fdatcodi, int enviocodi);
        int Save(AfInterrupSuministroDTO entity, IDbConnection connection, IDbTransaction transaction);
        void UpdateAEstadoBajaXEmpresa(int afecodi, int emprcodi, int fdatcodi, IDbConnection connection, IDbTransaction transaction);
        List<AfInterrupSuministroDTO> ObtenerUltimoEnvio(int afecodi, int emprcodi, int fdatcodi);
        List<AfInterrupSuministroDTO> ListarReporteExtranetCTAF(int afecodi, int emprcodi, int fdatcodi);
        List<AfInterrupSuministroDTO> ListarReporteInterrupcionesCTAF(string anio, string correlativo, int emprcodi, int fdatcodi);
    }
}

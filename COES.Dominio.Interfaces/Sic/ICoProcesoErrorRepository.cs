using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_PROCESO_ERROR
    /// </summary>
    public interface ICoProcesoErrorRepository
    {
        int Save(CoProcesoErrorDTO entity);
        void Update(CoProcesoErrorDTO entity);
        void Delete(int proerrcodi);
        CoProcesoErrorDTO GetById(int proerrcodi);
        List<CoProcesoErrorDTO> List();
        List<CoProcesoErrorDTO> ListarPorDia(int prodiacodi);
        List<CoProcesoErrorDTO> GetByCriteria();
        List<CoProcesoErrorDTO> ListarTablas(string tablas);
        int Save(CoProcesoErrorDTO entity, IDbConnection connection, IDbTransaction transaction);
        int GetMaximoID();
        void GrabarDatosXBloques(List<CoProcesoErrorDTO> entitys);
        void EliminarProcesosError(string strProdiacodis);
    }
}

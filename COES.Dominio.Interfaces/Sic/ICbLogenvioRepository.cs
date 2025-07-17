using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CB_LOGENVIO
    /// </summary>
    public interface ICbLogenvioRepository
    {
        int Save(CbLogenvioDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(CbLogenvioDTO entity);
        void Delete(int logenvcodi);
        CbLogenvioDTO GetById(int logenvcodi);
        List<CbLogenvioDTO> List();
        List<CbLogenvioDTO> GetByCriteria(int cbenvcodi);
        List<CbLogenvioDTO> ListarPorEnviosYEstado(string envioscodis, string estados);
        List<CbLogenvioDTO> ListarLogGaseosoPorEmpresaYRango(string empresas, string fecIni, string fecFin);
    }
}

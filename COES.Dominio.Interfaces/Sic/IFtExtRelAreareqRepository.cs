using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_REL_AREAREQ
    /// </summary>
    public interface IFtExtRelAreareqRepository
    {
        int Save(FtExtRelAreareqDTO entity);
        void Update(FtExtRelAreareqDTO entity);
        void Delete(int frracodi);
        FtExtRelAreareqDTO GetById(int frracodi);
        List<FtExtRelAreareqDTO> List();
        List<FtExtRelAreareqDTO> GetByCriteria();
        List<FtExtRelAreareqDTO> ListarPorAreas(string estado, string famercodis);
    }
}

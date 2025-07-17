using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_AREACONCEPUSER
    /// </summary>
    public interface IPrAreaConcepUserRepository
    {
        int Save(PrAreaConcepUserDTO entity);
        void Update(PrAreaConcepUserDTO entity);
        void Delete(int aconuscodi);
        PrAreaConcepUserDTO GetById(int aconuscodi);
        List<PrAreaConcepUserDTO> List();
        List<PrAreaConcepUserDTO> GetByCriteria(int concepcodi, string arconactivo, string aconusactivo);
        List<PrAreaConcepUserDTO> ListarConcepcodiByUsercode(string usercode);
    }
}

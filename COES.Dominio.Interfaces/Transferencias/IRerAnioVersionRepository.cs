using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface IRerAnioVersionRepository
    {
        int Save(RerAnioVersionDTO entity);
        void Update(RerAnioVersionDTO entity);
        void Delete(int rerAnioVersionId);
        RerAnioVersionDTO GetById(int rerAnioVersionId);
        List<RerAnioVersionDTO> List();
        RerAnioVersionDTO GetByAnnioAndVersion(int reravaniotarif, string reravversion);

        #region CU21
        RerAnioVersionDTO GetByAnioVersion(int iRerAVAnioTarif, int iRerAVVersion);
        List<RerAnioVersionDTO> ListRerAnioVersionesByAnio(int iRerAVAnioTarif);
        #endregion
    }
}


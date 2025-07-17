using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SMA_USUARIO_URS
    /// </summary>
    public interface ISmaUsuarioUrsRepository
    {
        int Save(SmaUsuarioUrsDTO entity);
        void Update(SmaUsuarioUrsDTO entity); // (SmaUsuarioUrsDTO entity);

        void UpdateUsuAct(int uurscodi);
        void Delete(int uurscodi, string user);
        SmaUsuarioUrsDTO GetById(int uurscodi);
        List<SmaUsuarioUrsDTO> List();
        List<SmaUsuarioUrsDTO> GetByCriteria(int usercode);

        List<SmaUsuarioUrsDTO> GetByCriteriaMO(int usercode);
        List<SmaUsuarioUrsDTO> GetUsuUrsAct(SmaUsuarioUrsDTO entity, string estado);
        
    }
}

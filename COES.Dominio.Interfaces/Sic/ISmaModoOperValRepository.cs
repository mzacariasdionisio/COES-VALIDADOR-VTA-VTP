using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SMA_MODO_OPER_VAL
    /// </summary>
    public interface ISmaModoOperValRepository
    {
        int Save(SmaModoOperValDTO entity);

        int GetNumVal();
        void Update(SmaModoOperValDTO entity);
        void Delete(string user, int mopvcodi);
        SmaModoOperValDTO GetById(int mopvcodi);
        List<SmaModoOperValDTO> List(string grupocodi);
        List<SmaModoOperValDTO> ListMOVal(int? movpgrupval, int urscodi);
        
        List<SmaModoOperValDTO> ListAll();
        List<SmaModoOperValDTO> GetByCriteria();

        List<SmaModoOperValDTO> GetListMOValxUrs(int urscodi);

    }
}

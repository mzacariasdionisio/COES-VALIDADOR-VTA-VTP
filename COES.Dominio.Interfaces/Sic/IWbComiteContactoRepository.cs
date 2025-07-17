using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_COMITE_CONTACTO
    /// </summary>
    public interface IWbComiteContactoRepository
    {
        void Save(WbComiteContactoDTO entity);
        void Update(WbComiteContactoDTO entity);
        void Delete(int contaccodi,int comitecodi);
        WbComiteContactoDTO GetById(int contaccodi, int comitecodi);
        List<WbComiteContactoDTO> List();
        List<WbComiteContactoDTO> GetByCriteria(int idContacto);
    }
}

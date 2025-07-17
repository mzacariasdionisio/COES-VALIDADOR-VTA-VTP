using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_COMITE_CONTACTO
    /// </summary>
    public interface IWbComiteListaContactoRepository
    {
        void Save(WbComiteListaContactoDTO entity);
        void Update(WbComiteListaContactoDTO entity);
        void Delete(int contaccodi,int comitelistacodi);
        WbComiteListaContactoDTO GetById(int contaccodi, int comitecodi);
        List<WbComiteListaContactoDTO> List();
        List<WbComiteListaContactoDTO> GetByCriteria(int idContacto);
    }
}

using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_PROCESO_CONTACTO
    /// </summary>
    public interface IWbProcesoContactoRepository
    {
        void Save(WbProcesoContactoDTO entity);
        void Update(WbProcesoContactoDTO entity);
        void Delete(int contaccodi, int procesocodi);
        WbProcesoContactoDTO GetById(int contaccodi, int procesocodi);
        List<WbProcesoContactoDTO> List();
        List<WbProcesoContactoDTO> GetByCriteria(int idContacto);
    }
}

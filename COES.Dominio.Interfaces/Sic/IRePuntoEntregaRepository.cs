using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_PUNTO_ENTREGA
    /// </summary>
    public interface IRePuntoEntregaRepository
    {
        int Save(RePuntoEntregaDTO entity);
        void Update(RePuntoEntregaDTO entity);
        void Delete(int repentcodi);
        RePuntoEntregaDTO GetById(int repentcodi);
        List<RePuntoEntregaDTO> List();
        List<RePuntoEntregaDTO> GetByCriteria();        
    }
}

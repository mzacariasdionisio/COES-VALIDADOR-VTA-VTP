using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EPR_PROYECTO_ACTEQP
    /// </summary>
    public interface IEprPropCatalogoDataRepository
    {
        int Save(EprPropCatalogoDataDTO entity);
        void Update(EprPropCatalogoDataDTO entity);
        void Delete_UpdateAuditoria(EprPropCatalogoDataDTO entity);
        EprPropCatalogoDataDTO GetById(int Epareacodi);
        List<EprPropCatalogoDataDTO> List(int Eqcatccodi);
        List<EprPropCatalogoDataDTO> ListMaestroMarcaProteccion();
    }
}

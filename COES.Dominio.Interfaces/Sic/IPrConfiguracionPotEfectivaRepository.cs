using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_CONFIGURACION_POT_EFECTIVA
    /// </summary>
    public interface IPrConfiguracionPotEfectivaRepository
    {
        void Save(PrConfiguracionPotEfectivaDTO entity);
        void Update(PrConfiguracionPotEfectivaDTO entity);
        void Delete(int grupocodi);
        void DeleteAll();
        void SaveAll(List<PrConfiguracionPotEfectivaDTO> listEntity);
        PrConfiguracionPotEfectivaDTO GetById(int grupocodi);
        List<PrConfiguracionPotEfectivaDTO> List();
        List<PrConfiguracionPotEfectivaDTO> GetByCriteria();
    }
}

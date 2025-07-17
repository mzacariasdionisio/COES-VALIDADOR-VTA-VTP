using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_EVENTO
    /// </summary>
    public interface IIndEventoRepository
    {
        int Save(IndEventoDTO entity);
        void Update(IndEventoDTO entity);
        void Delete(int ieventcodi);
        IndEventoDTO GetById(int ieventcodi);
        List<IndEventoDTO> List();
        List<IndEventoDTO> ListHistoricoByEvencodi(int evencodi);
        List<IndEventoDTO> GetByCriteria(DateTime fechaIni, DateTime fechaFin, string idsEmpresa, string idsTipoEquipo, string idstipoMantto);
    }
}

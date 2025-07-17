using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_IEODCUADRO
    /// </summary>
    public interface IIndIeodcuadroRepository
    {
        int Save(IndIeodcuadroDTO entity);
        void Update(IndIeodcuadroDTO entity);
        void Delete(int iiccocodi);
        IndIeodcuadroDTO GetById(int iiccocodi);
        List<IndIeodcuadroDTO> List();
        List<IndIeodcuadroDTO> ListHistoricoByIccodi(int iccodi);
        List<IndIeodcuadroDTO> GetByCriteria(DateTime fechaIni, DateTime fechaFin, string idsEmpresa, string idsTipoEquipo, string idstipoMantto);
    }
}

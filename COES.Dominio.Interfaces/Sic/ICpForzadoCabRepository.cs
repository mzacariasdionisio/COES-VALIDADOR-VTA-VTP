using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_FORZADO_CAB
    /// </summary>
    public interface ICpForzadoCabRepository
    {
        int Save(CpForzadoCabDTO entity);
        void Update(CpForzadoCabDTO entity);
        void Delete(int cpfzcodi);
        CpForzadoCabDTO GetById(int cpfzcodi);
        List<CpForzadoCabDTO> List();
        List<CpForzadoCabDTO> GetByCriteria();
        CpForzadoCabDTO GetByDate(DateTime fechaHora);
        List<CpForzadoCabDTO> GetListByDate(DateTime fechaHora);
    }
}

using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MMM_FACTABLE
    /// </summary>
    public interface IMmmDatoRepository
    {
        int Save(MmmDatoDTO entity);
        void Update(MmmDatoDTO entity);
        void Delete(int Mmmdatocodi);
        MmmDatoDTO GetById(int Mmmdatocodi);
        List<MmmDatoDTO> List();
        List<MmmDatoDTO> GetByCriteria();
        int MaxSidFacTable();
        List<MmmDatoDTO> ListPeriodo(DateTime fechaInicio, DateTime fechaFin);
    }
}

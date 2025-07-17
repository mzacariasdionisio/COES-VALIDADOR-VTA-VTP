using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_PTOENTREGA
    /// </summary>
    public interface IEvePtoentregaRepository
    {
        int Save(EvePtoentregaDTO entity);
        void Update(EvePtoentregaDTO entity);
        void Delete(int ptoentregacodi);
        EvePtoentregaDTO GetById(int ptoentregacodi);
        List<EvePtoentregaDTO> List();
        List<EvePtoentregaDTO> GetByCriteria();
    }
}


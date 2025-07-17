using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnPtoEstimadorRepository
    {
        List<PrnPtoEstimadorDTO> List();
        PrnPtoEstimadorDTO GetById(int codigo);
        void Delete(int codigo);
        void Save(PrnPtoEstimadorDTO entity);
        void Update(PrnPtoEstimadorDTO entity);               
    }
}

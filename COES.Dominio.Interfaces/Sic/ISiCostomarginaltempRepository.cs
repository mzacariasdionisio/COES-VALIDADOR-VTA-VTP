using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    public interface ISiCostomarginaltempRepository
    {
        void Save(SiCostomarginaltempDTO entity);
        void Delete(int enviocodi);
        void SaveSiCostomarginaltempMasivo(List<SiCostomarginaltempDTO> ListObj);
    }
}

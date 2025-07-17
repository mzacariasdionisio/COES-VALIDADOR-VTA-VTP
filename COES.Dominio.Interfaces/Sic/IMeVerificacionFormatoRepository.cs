using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_VERIFICACION_FORMATO
    /// </summary>
    public interface IMeVerificacionFormatoRepository
    {
        void Save(MeVerificacionFormatoDTO entity);
        void Update(MeVerificacionFormatoDTO entity);
        void Delete(int formatcodi, int verifcodi);
        MeVerificacionFormatoDTO GetById(int formatcodi, int verifcodi);
        List<MeVerificacionFormatoDTO> List();
        List<MeVerificacionFormatoDTO> GetByCriteria();
        List<MeVerificacionFormatoDTO> ListByFormato(int formatcodi);
    }
}

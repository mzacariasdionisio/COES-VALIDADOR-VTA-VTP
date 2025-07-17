using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_YUPCON_ENVIO
    /// </summary>
    public interface ICpYupconEnvioRepository
    {
        int Save(CpYupconEnvioDTO entity);
        void Update(CpYupconEnvioDTO entity);
        void Delete(int cyupcodi);
        CpYupconEnvioDTO GetById(int cyupcodi);
        List<CpYupconEnvioDTO> List();
        List<CpYupconEnvioDTO> GetByCriteria(int tyupcodi, DateTime fecha, int hora);
    }
}

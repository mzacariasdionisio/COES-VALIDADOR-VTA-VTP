using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_YUPCON_TIPO
    /// </summary>
    public interface ICpYupconTipoRepository
    {
        int Save(CpYupconTipoDTO entity);
        void Update(CpYupconTipoDTO entity);
        void Delete(int tyupcodi);
        CpYupconTipoDTO GetById(int tyupcodi);
        List<CpYupconTipoDTO> List();
        List<CpYupconTipoDTO> GetByCriteria();
    }
}

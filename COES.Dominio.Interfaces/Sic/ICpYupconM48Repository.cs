using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_YUPCON_M48
    /// </summary>
    public interface ICpYupconM48Repository
    {
        int Save(CpYupconM48DTO entity);
        void Update(CpYupconM48DTO entity);
        void Delete(int dyupcodi);
        CpYupconM48DTO GetById(int dyupcodi);
        List<CpYupconM48DTO> List();
        List<CpYupconM48DTO> GetByCriteria(int cyupcodi);
    }
}

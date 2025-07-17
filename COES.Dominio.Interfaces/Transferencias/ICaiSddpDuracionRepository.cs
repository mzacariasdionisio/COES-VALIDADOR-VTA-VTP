using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CAI_SDDP_DURACION
    /// </summary>
    public interface ICaiSddpDuracionRepository
    {
        int Save(CaiSddpDuracionDTO entity);
        void Update(CaiSddpDuracionDTO entity);
        void Delete();
        CaiSddpDuracionDTO GetById(int sddpducodi);
        List<CaiSddpDuracionDTO> List();
        List<CaiSddpDuracionDTO> GetByCriteria();
        List<CaiSddpDuracionDTO> ListByEtapa(int sddpduetapa);
    }
}

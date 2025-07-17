using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_UMBRALREPORTE
    /// </summary>
    public interface ICmUmbralreporteRepository
    {
        int Save(CmUmbralreporteDTO entity);
        void Update(CmUmbralreporteDTO entity);
        void Delete(int cmurcodi);
        CmUmbralreporteDTO GetById(int cmurcodi);
        List<CmUmbralreporteDTO> List();
        List<CmUmbralreporteDTO> GetByCriteria(DateTime fecha);
        List<CmUmbralreporteDTO> ObtenerHistorico();
    }
}

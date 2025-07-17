using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_BARRA_RELACION
    /// </summary>
    public interface ICmBarraRelacionRepository
    {
        int Save(CmBarraRelacionDTO entity);
        void Update(CmBarraRelacionDTO entity);
        void Delete(int cmbarecodi);
        CmBarraRelacionDTO GetById(int cmbarecodi);
        List<CmBarraRelacionDTO> List();
        List<CmBarraRelacionDTO> GetByCriteria(DateTime fecha);

        List<CmBarraRelacionDTO> GetByCriteria(string tipoRegistro, int barra);
        List<CmBarraRelacionDTO> ObtenerHistorico(int barra, string tipoRegistro);
    }
}

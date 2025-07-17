using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RCA_PARAM_ESQUEMA
    /// </summary>
    public interface IRcaParamEsquemaRepository
    {
        int Save(RcaParamEsquemaDTO entity);
        void Update(RcaParamEsquemaDTO entity);
        void Delete(int rcparecodi);
        RcaParamEsquemaDTO GetById(int rcparecodi);
        List<RcaParamEsquemaDTO> List();
        List<RcaParamEsquemaDTO> GetByCriteria();
        List<RcaParamEsquemaDTO> ListarPorFiltros(string anio, string tipoEmpresa);
        List<int> ListarAniosParametroEsquema();
        List<RcaParamEsquemaDTO> ListarPorPuntoMedicion(string listaPuntoMedicion);
    }
}

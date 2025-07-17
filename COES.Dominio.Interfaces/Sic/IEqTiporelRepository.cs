using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EQ_TIPOREL
    /// </summary>
    public interface IEqTiporelRepository
    {
        int Save(EqTiporelDTO entity);
        void Update(EqTiporelDTO entity);
        void Delete(int tiporelcodi);
        EqTiporelDTO GetById(int tiporelcodi);
        List<EqTiporelDTO> List();
        List<EqTiporelDTO> GetByCriteria();
        List<EqTiporelDTO> ListarTipoRelxFiltro(string strNombreTiporel, string strEstado, int nroPagina, int nroFilas);
        int CantidadTipoRelxFiltro(string strNombreTiporel, string strEstado);
    }
}


using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RCA_CUADRO_EJEC_USUARIO
    /// </summary>
    public interface IRcaCuadroEjecUsuarioRepository
    {
        int Save(RcaCuadroEjecUsuarioDTO entity);
        void Update(RcaCuadroEjecUsuarioDTO entity);
        void Delete(int rcejeucodi);
        RcaCuadroEjecUsuarioDTO GetById(int rcejeucodi);
        List<RcaCuadroEjecUsuarioDTO> List();
        List<RcaCuadroEjecUsuarioDTO> GetByCriteria(int rcproucodi);
        List<RcaCuadroEjecUsuarioDTO> ListFiltro(string programa, string cuadroPrograma, string codigoSuministrador, string tipoReporte);
    }
}

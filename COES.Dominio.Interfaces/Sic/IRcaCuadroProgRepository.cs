using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RCA_CUADRO_PROG
    /// </summary>
    public interface IRcaCuadroProgRepository
    {
        int Save(RcaCuadroProgDTO entity);
        void Update(RcaCuadroProgDTO entity);
        void Delete(int rccuadcodi);
        RcaCuadroProgDTO GetById(int rccuadcodi);
        List<RcaCuadroProgDTO> List();
        List<RcaCuadroProgDTO> GetByCriteria(string programa, string estadoCuadro);
        List<RcaCuadroProgDTO> ListRcaCuadroProgFiltro(string horizonte, string configuracion, string estado, string fecIni, string fecFin, string energiaRechazadaInicio, 
            string energiaRechazadaFin, int sinPrograma);
        List<RcaHorizonteProgDTO> ListHorizonteProg();
        List<RcaConfiguracionProgDTO> ListConfiguracionProg();
        List<RcaCuadroProgDTO> ListCuadroEnvioArchivoPorPrograma(int rcprogcodi);
        void UpdateCuadroProgramaEjecucion(RcaCuadroProgDTO entity);

        List<RcaCuadroEstadoDTO> ListEstadoCuadroProg();
        void UpdateCuadroEstado(RcaCuadroProgDTO entity);

        void UpdateCuadroEvento(RcaCuadroProgDTO entity);


    }
}

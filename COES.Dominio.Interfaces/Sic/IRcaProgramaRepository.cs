using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RCA_PROGRAMA
    /// </summary>
    public interface IRcaProgramaRepository
    {
        int Save(RcaProgramaDTO entity);

        int CrearPrograma(RcaProgramaDTO entity);
        void Update(RcaProgramaDTO entity);

        void ActualizarPrograma(RcaProgramaDTO entity);
        void Delete(int rcprogcodi, string usuario);
        RcaProgramaDTO GetById(int rcprogcodi);
        List<RcaProgramaDTO> List();
        List<RcaProgramaDTO> GetByCriteria(string codigoProgramaAbrev);
        List<RcaProgramaDTO> ListProgramaEnvioArchivo(DateTime fechaReferencia);
        List<RcaProgramaDTO> ListProgramaRechazoCarga(bool muestraVigentes);

        List<RcaProgramaDTO> ListProgramaFiltro(int horizonte, string codigoPrograma, string nombrePrograma, int reprograma);
    }
}

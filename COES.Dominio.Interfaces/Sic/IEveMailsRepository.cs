using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_MAILS
    /// </summary>
    public interface IEveMailsRepository
    {
        int Save(EveMailsDTO entity);
        void Update(EveMailsDTO entity);
        void Delete(int mailcodi);
        EveMailsDTO GetById(int mailcodi);
        List<EveMailsDTO> List();
        List<EveMailsDTO> GetByCriteria();
        List<EveMailsDTO> BuscarOperaciones(int subCausacodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize);
        int ObtenerNroFilas(int subCausacodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize);
        List<EveMailsDTO> GetListaReprogramado(DateTime fechaInicio);
        List<EveMailsDTO> ExportarEnvioCorreos(int? idTipoOperacion, DateTime fechaInicio, DateTime fechaFin);
        List<EveMailsDTO> BuscarOperacionesDelTipoReProgramaPorFecha(string fecha);

        EveMailsDTO GetFechaMaxProgramaEmitido(DateTime fecha);
    }
}

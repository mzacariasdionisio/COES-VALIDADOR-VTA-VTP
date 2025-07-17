using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla INF_ARCHIVO_AGENTE
    /// </summary>
    public interface IInfArchivoAgenteRepository
    {
        int Save(InfArchivoAgenteDTO entity);
        void Update(InfArchivoAgenteDTO entity);
        void Delete(int archicodi);
        InfArchivoAgenteDTO GetById(int archicodi);
        List<InfArchivoAgenteDTO> List();
        List<InfArchivoAgenteDTO> GetByCriteria();
        List<InfArchivoAgenteDTO> ListarArchivosPorEmpresa(int iEmpresa, int nroPagina, int nroFilas);
        List<InfArchivoAgenteDTO> ListarArchivosPorFiltro(int iEmpresa, DateTime dtFechaInicio, DateTime dtFechaFin, int nroPagina, int nroFilas);
        int TotalListarArchivosPorEmpresa(int iEmpresa);
        int TotalListarArchivosPorFiltro(int iEmpresa, DateTime dtFechaInicio, DateTime dtFechaFin);
        int CantidadArchivosPorNombre(string sNombreArchivo);

    }
}

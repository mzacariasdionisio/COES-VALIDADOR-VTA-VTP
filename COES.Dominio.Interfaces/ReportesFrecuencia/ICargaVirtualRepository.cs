using COES.Base.Core;
using COES.Dominio.DTO.ReportesFrecuencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.ReportesFrecuencia
{
    public interface ICargaVirtualRepository
    {
        CargaVirtualDTO GetById(int IdCarga);
        List<CargaVirtualDTO> GetListaCargaVirtual(string FechaInicial, string FechaFinal, string CodEquipo);
        List<LecturaVirtualDTO> GetListaLecturaVirtual(int IdCarga);
        List<SiEmpresaDTO> GetListaEmpresasCargaVirtual();
        List<CentralDTO> GetListaCentralPorEmpresa(int CodEmpresa);
        List<UnidadDTO> GetListaUnidadPorCentralEmpresa(int CodEmpresa, string Central);
        CargaVirtualDTO SaveUpdate(CargaVirtualDTO entity);
        CargaVirtualDTO SaveUpdateExterno(CargaVirtualDTO entity);
        LecturaVirtualDTO SaveLecturaVirtual(LecturaVirtualDTO entity);
        string SaveLecturaVirtualString(LecturaVirtualDTO entity);
        LecturaDTO SaveLectura(LecturaDTO entity);
        string SaveLecturaString(LecturaDTO entity);
        int SaveLecturaQuery(string query);
    }
}

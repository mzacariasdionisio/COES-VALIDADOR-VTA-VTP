using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
namespace COES.Dominio.Interfaces.Sic
{
    public interface IEveEventoEquipoRepository
    {
        int Save(EveEventoEquipoDTO entity);
        int Aprobar(EveEventoEquipoDTO entity);

        List<EveEventoEquipoDTO> ListarDetalleEquiposSEIN(string empresas,
            int nroPaginas, int pageSize, string idsFamilia, string campo, string orden);

        List<EveEventoEquipoDTO> ListarPendientesEquiposSEIN(string empresas,
            int nroPaginas, int pageSize, string idsFamilia, string fechaini, string fechafin, string orden);

        int AprobarE(int codigo, int idempresa,
            int idFamilia, int idequipo, int estado, int idmotivo, string motivoabrev, string ifecha, int idubicacion, string usuario);
        List<EveEventoEquipoDTO> ListarDetalleEquiposSEIN02(string equipos,string sTipoEquipo,  DateTime fechaini, DateTime fechafin);
        List<EveEventoEquipoDTO> ListarIngresoSalidaOperacionComercialSEIN(int subcausacodi, DateTime fechaini, DateTime fechafin);
    }
}

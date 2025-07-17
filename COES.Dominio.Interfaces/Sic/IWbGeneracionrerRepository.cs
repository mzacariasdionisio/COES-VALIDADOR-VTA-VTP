using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_GENERACIONRER
    /// </summary>
    public interface IWbGeneracionrerRepository
    {
        void Save(WbGeneracionrerDTO entity);
        void Update(WbGeneracionrerDTO entity);
        void Delete(int ptomedicodi, string lastUser, DateTime lastDate);
        WbGeneracionrerDTO GetById(int ptomedicodi);
        List<WbGeneracionrerDTO> List();
        List<WbGeneracionrerDTO> GetByCriteria();
        List<WbGeneracionrerDTO> ObtenerPuntosEmpresas();        
        List<WbGeneracionrerDTO> ObtenerPuntosCentrales(int idEmpresa);
        List<WbGeneracionrerDTO> ObtenerPuntosUnidades(int ptoCentral);
        List<WbGeneracionrerDTO> ObtenerPuntoFormato(int idEmpresa);
        int ValidarExistencia(int ptoMediCodi);
        int ValidarExistenciaUnidad(int ptoMediCodi);
        int ValidarExistenciaGeneral(int ptoMediCodi);
        string ObtenerEmpresaPorUsuario(string userLogin);
        void GrabarConfiguracion(int ptomedicodi, decimal? minimo, decimal? maximo, string lastuser);
    }
}


using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Dominio.DTO.Scada;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla C_POINTS
    /// </summary>
    public interface ICPointsRepository
    {
        void Update(CPointsDTO entity);
        void Delete();
        CPointsDTO GetById();
        List<CPointsDTO> List();
        List<CPointsDTO> GetByCriteria();
        List<TrCanalSp7DTO> ObtenerCanalesTrcoes();
        void CrearCanalConDataDeScada(string usuario, CPointsDTO cpoint);
        void ActualizarCanalConDataDeScada(string usuario, CPointsDTO cpoint);
        void EliminadoLogicoDeCanales(string usuario, int canalcodi);
        int GetMaxCodigoTrCargaArchXmlSp7();
        void GenerarRegistroTrCargaArchXmlSp7SiHayActualizacionDeCanales(int codigoMax, DateTime fechaInicio, int cantidadActualizados, string usuario, string fileXml, int tipo);
        void UpdateCanalCambioSiHayActualizacionDeCanales(int codigoMax, DateTime fechaInicio);
        List<TrEmpresaSp7DTO> ObtenerEmpresasDesdeTrcoes();
        void GenerarEmpresasEnTrcoes(int emprcodi, string emprdescripcion, string siid, string usuario);
        void ActualizarCanalesIccpXml(string canaliccp, string pathb, string usuario, string canalremota, string canalcontenedor, string canalenlace, int canalcodigo);
        String ObtenerTotalZonasPorZonaId(int zonaId);
        String ObtenerTotalEmpresaPorEmprcodi(int emprcodi);
        void GenerarRegistroZona(int zonacodi, string zonaabrev, string zonanomb, int emprcodi, int zonasiid, string usuario);
        void ActualizarRegistroZona(string zonaabrev, string zonanomb, int emprcodi, int zonasiid, string usuario, int zonacodi);
        void ActualizarCanalSiid(int emprcodi, int zonacodi, string canalunidad, string usuario, int canalcodi);
    }
}

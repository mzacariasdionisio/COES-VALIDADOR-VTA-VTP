using COES.Base.Core;
using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using DevExpress.Office.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Scada
{
    public class ScadaServicio : AppServicioBase
    {

        /// <summary>
        /// Devuelve listado de canales de la tabla TR_POINT_PRI
        /// </summary>
        /// <returns></returns>
        public List<CPointsDTO> ObtenerCanalesScada()
        {
            return FactorySic.GetCPointsRepository().List();
        }

        /// <summary>
        /// Devuelve listado de canales de la tabla TR_CANAL_SP7
        /// </summary>
        /// <returns></returns>
        public List<TrCanalSp7DTO> ObtenerCanalesTrcoes()
        {
            return FactorySic.GetCPointsRepository().ObtenerCanalesTrcoes();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public void CrearCanalConDataDeScada(string usuario, CPointsDTO cpoint)
        {
            FactorySic.GetCPointsRepository().CrearCanalConDataDeScada(usuario, cpoint);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public void ActualizarCanalConDataDeScada(string usuario, CPointsDTO cpoint)
        {
            FactorySic.GetCPointsRepository().ActualizarCanalConDataDeScada(usuario, cpoint);
        }

        /// <summary>
        /// Eliminado lógico de los Registros con (ACTIVE = ‘F’) que existen en TRCOES
        /// </summary>
        /// <returns></returns>
        public void EliminadoLogicoDeCanales(string usuario, int canalcodi)
        {
            FactorySic.GetCPointsRepository().EliminadoLogicoDeCanales(usuario, canalcodi);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public int GetMaxCodigoTrCargaArchXmlSp7()
        {
            return FactorySic.GetCPointsRepository().GetMaxCodigoTrCargaArchXmlSp7();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public void GenerarRegistroTrCargaArchXmlSp7SiHayActualizacionDeCanales(int codigoMax, DateTime fechaInicio, int cantidadActualizados, string usuario, string fileXml, int tipo)
        {
            FactorySic.GetCPointsRepository().GenerarRegistroTrCargaArchXmlSp7SiHayActualizacionDeCanales(codigoMax, fechaInicio, cantidadActualizados, usuario, fileXml, tipo);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public void UpdateCanalCambioSiHayActualizacionDeCanales(int codigoMax, DateTime fechaInicio)
        {
            FactorySic.GetCPointsRepository().UpdateCanalCambioSiHayActualizacionDeCanales(codigoMax, fechaInicio);
        }

        public List<TrEmpresaSp7DTO> ObtenerEmpresasDesdeTrcoes()
        {
            return FactorySic.GetCPointsRepository().ObtenerEmpresasDesdeTrcoes();
        }

        public void GenerarEmpresasEnTrcoes(int emprcodi, string emprdescripcion, string siid, string usuario)
        {
            FactorySic.GetCPointsRepository().GenerarEmpresasEnTrcoes(emprcodi, emprdescripcion, siid, usuario);
        }

        public void ActualizarCanalesIccpXml(string canaliccp, string pathb, string usuario, string canalremota, string canalcontenedor, string canalenlace, int canalcodigo)
        {
            FactorySic.GetCPointsRepository().ActualizarCanalesIccpXml(canaliccp, pathb, usuario, canalremota, canalcontenedor, canalenlace, canalcodigo);
        }

        public String ObtenerTotalZonasPorZonaId(int zonaId)
        {
            return FactorySic.GetCPointsRepository().ObtenerTotalZonasPorZonaId(zonaId);
        }

        public String ObtenerTotalEmpresaPorEmprcodi(int emprcodi)
        {
            return FactorySic.GetCPointsRepository().ObtenerTotalEmpresaPorEmprcodi(emprcodi);
        }

        public void GenerarRegistroZona(int zonacodi, string zonaabrev, string zonanomb, int emprcodi, int zonasiid, string usuario)
        {
            FactorySic.GetCPointsRepository().GenerarRegistroZona(zonacodi, zonaabrev, zonanomb, emprcodi, zonasiid, usuario);
        }

        public void ActualizarRegistroZona(string zonaabrev, string zonanomb, int emprcodi, int zonasiid, string usuario, int zonacodi)
        {
            FactorySic.GetCPointsRepository().ActualizarRegistroZona(zonaabrev, zonanomb, emprcodi, zonasiid, usuario, zonacodi);
        }

        public void ActualizarCanalSiid(int emprcodi, int zonacodi, string canalunidad, string usuario, int canalcodi)
        {
            FactorySic.GetCPointsRepository().ActualizarCanalSiid(emprcodi, zonacodi, canalunidad, usuario, canalcodi);
        }
    }
}

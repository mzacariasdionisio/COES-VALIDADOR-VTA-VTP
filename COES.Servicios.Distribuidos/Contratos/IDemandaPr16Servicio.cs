using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using COES.Dominio.DTO.Sic;

namespace COES.Servicios.Distribuidos.Contratos
{
    /// <summary>
    /// Servicios que concentra los métodos de recepción y validación de Remisión de Demanda Cliente libres y Distribuidores (PR16)
    /// </summary>
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface IDemandaPr16Servicio
    {
        /// <summary>
        /// Obtiene el listado de Puntos de medición PR16 para una empresa por periodo
        /// </summary>
        /// <param name="emprcodi">código de empresa</param>
        /// <param name="periodo">Periodo mes-año</param>
        /// <returns>Listado de Puntos de medición PR16 de la empresa para el periodo</returns>
        [OperationContract]
        List<MeHojaptomedDTO> ConsultaPuntoMedicion(int emprcodi, string periodo);

        /// <summary>
        /// Obtiene los datos de máxima demanda
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <returns></returns>
        [OperationContract]
        MaximaDemandaDTO ObtenerDatosDemandaMaxima(string periodo);

        /// <summary>
        /// Método que determina si la empresa está habilitada para enviar einformación en el periodo
        /// </summary>
        /// <param name="emprCodi">Código de empre</param>
        /// <param name="periodo">Periodo de envío</param>
        /// <returns>True o False</returns>
        [OperationContract]
        bool ValidarFechaPr16(int emprCodi, string periodo);

        /// <summary>
        /// Método que registra los datos de demanda cada 15 minutos
        /// </summary>
        /// <param name="entitys">Listado de datos de demanda</param>
        /// <param name="periodo">Periodo de envío</param>
        /// <param name="usuario">Login de usuario</param>
        /// <param name="idEmpresa">Código de empresa</param>
        /// <returns></returns>
        [OperationContract]
        int GrabarValoresDemandaPr16(List<MeMedicion96DTO> entitys, string periodo, string usuario, int idEmpresa);

        /// <summary>
        /// Método que notifica a los involucrados del envío
        /// </summary>
        /// <param name="envioCodi">Código de envío</param>
        /// <param name="emprCodi">Código de empresa</param>
        /// <param name="periodo">Periodo de envío</param>
        [OperationContract]
        void EnvioNotificacion(int envioCodi, int emprCodi, string periodo, string usuario);

        /// <summary>
        /// Método que retorno los datos de demanda segpun el código de envío
        /// </summary>
        /// <param name="envioCodi">Código de envío</param>
        /// <returns>Listado de demanda por cada 15 min</returns>
        [OperationContract]
        List<MeMedicion96DTO> ObtenerRemision(int envioCodi);
    }
}

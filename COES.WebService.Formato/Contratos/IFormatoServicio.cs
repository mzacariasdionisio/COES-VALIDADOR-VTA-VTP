using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.WebService.Formato.Contratos
{
    /// <summary>
    /// Servicio que expone la funcionalidad de formatos de envío
    /// </summary>
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface IFormatoServicio
    {
        /// <summary>
        /// Método que retorna los datos de un formato
        /// </summary>
        /// <param name="formatcodi">Código de formato</param>
        /// <returns></returns>
        [OperationContract]
        MeFormatoDTO GetFormato(int formatcodi);
        /// <summary>
        /// Método que registra los valores cada 15 min para el formato, empresa y fechas indicadas.
        /// </summary>
        /// <param name="entitys">Listado de valores cada 15 minutos</param>
        /// <param name="usuario">nombre de usuario</param>
        /// <param name="emprcodi">código de empresa</param>
        /// <param name="formatcodi">código de formato</param>
        /// <param name="fecha">Día</param>
        /// <param name="semana">Número Semana</param>
        /// <param name="mes">Mes</param>
        /// <returns>Código de envío</returns>
        [OperationContract]
        int GrabarValores96(List<MeMedicion96DTO> entitys, string usuario, int emprcodi, int formatcodi, string fecha, string semana, string mes);

        /// <summary>
        /// Método que valida si el plazo está vigente
        /// </summary>
        /// <param name="formatcodi">Código de formato</param>
        /// <param name="emprcodi">Código de empresa</param>
        /// <param name="fecha">Día</param>
        /// <param name="semana">Número de semana</param>
        /// <param name="mes">Mes de envío</param>
        /// <returns>Si el plazo es válido o no</returns>
        [OperationContract]
        bool ValidaPlazoEnvio(int formatcodi, int emprcodi, string fecha, string semana, string mes);

        /// <summary>
        /// Método que consulta los datos de un envío por empresa y formato
        /// </summary>
        /// <param name="emprcodi">Código de empresa</param>
        /// <param name="enviocodi">Códido de envío</param>
        /// <param name="formatcodi">Código de formato</param>
        /// <returns>Listado de valores cada 15 minuotos del envío consultado</returns>
        [OperationContract]
        List<MeMedicion96DTO> ConsultarEnvio15min(int emprcodi, int enviocodi, int formatcodi);
    }
}

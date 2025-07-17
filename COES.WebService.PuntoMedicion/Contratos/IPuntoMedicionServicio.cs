using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.WebService.PuntoMedicion.Contratos
{
    /// <summary>
    /// Servicio que expone funcionalidad de puntos de medición
    /// </summary>
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface IPuntoMedicionServicio
    {
        /// <summary>
        /// Método que retorna el listado de Puntos de Medicion Hoja por empresa y formato
        /// </summary>
        /// <param name="emprcodi">Código de empresa</param>
        /// <param name="formatcodi">Código de Formato</param>
        /// <returns></returns>
        [OperationContract]
        List<MeHojaptomedDTO> GetPuntosMedicion(int emprcodi, int formatcodi);

        /// <summary>
        /// Método que devuelve el equipo relacionado el Punto de Medición
        /// </summary>
        /// <param name="ptomedicodi">Código de Punto de medición</param>
        /// <returns></returns>
        [OperationContract]
        EqEquipoDTO GetEquipoPorPuntoMedicion(int ptomedicodi);

        /// <summary>
        /// Listado de Tipos de Punto de Medición filtrado por tipo de información
        /// </summary>
        /// <param name="tipoinfocodi">Código de Tipo de información</param>
        /// <returns>Listado de Tipos de punto de medición</returns>
        [OperationContract]
        List<MeTipopuntomedicionDTO> GetTipoPuntoMedicionPorTipoInformacion(int tipoinfocodi);
    }
}

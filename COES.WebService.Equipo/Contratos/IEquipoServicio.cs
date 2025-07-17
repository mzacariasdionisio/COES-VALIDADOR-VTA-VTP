using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.WebService.Equipo.Contratos
{
    /// <summary>
    /// Servicio que expone la funcionalidad de equipos
    /// </summary>
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface IEquipoServicio
    {
        /// <summary>
        /// Listado de todas las centrales filtrado por puntos de medición
        /// </summary>
        /// <param name="listaPuntos">Listado de Puntos de medición</param>
        /// <returns></returns>
        [OperationContract]
        List<EqEquipoDTO> GetCentralesPorPuntosMedicion(List<MeHojaptomedDTO> listaPuntos);
    }
}

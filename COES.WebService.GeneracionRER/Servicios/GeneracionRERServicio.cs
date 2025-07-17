using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Mediciones;
using COES.WebService.GeneracionRER.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace COES.WebService.GeneracionRER.Servicios
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class GeneracionRERServicio: IGeneracionRERServicio
    {
        /// <summary>
        /// Instancia de clase para acceso a datos
        /// </summary>
        GeneracionRERAppServicio logic = new GeneracionRERAppServicio();

        /// <summary>
        /// Permite obtener los codigos de carga
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<CodigoCargaRER> ObtenerCodigosDeCarga(string userLogin)
        {
            return this.logic.ObtenerCodigosDeCarga(userLogin);
        }

        /// <summary>
        /// Permite realizar la carga retornando una lista de validaciones en caso existieran
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="fecha"></param>
        /// <param name="nroSemana"></param>
        /// <param name="valores"></param>
        /// <returns>
        /// -1: Ha ocurrido un error en la carga de datos.
        ///  1: Carga dentro del plazo
        ///  2: Carga fuera del plazo
        ///  3: Carga dentro del plazo, se reemplazaron datos
        ///  4: Carga fuera de plazo, se reemplazaron datos
        ///  5: La fecha enviada y la fecha de los datos no coinciden.
        ///  6: Se deben enviar datos de un día.
        ///  7: LaS fechas de los datos no corresponden a las fechas de la semana.
        ///  8: Se deben enviar datos de siete días.
        ///  9: Los códigos de carga no son los correctos
        /// </returns>
        public int CargarDatos(int horizonte, DateTime fecha, int anio, int nroSemana, List<Medicion48> valores, string userLogin)
        {
            return this.logic.CargarDatos(horizonte, fecha, anio, nroSemana, valores, userLogin);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Web.Services;
using log4net;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Helper;
using System.Globalization;
using COES.Servicios.Distribuidos.Models;

namespace COES.Servicios.Distribuidos.Servicios
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class DigsilentServicio : IDigsilentServicio
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ProcesoServicio));
        DigSilentAppServicio servicio = new DigSilentAppServicio();

        /// <summary>
        /// Proceso carga de informacion generacion, manttos, demanda, transformadores, lineas
        /// </summary>
        /// <param name="program"></param>
        /// <param name="fecha"></param>
        /// <param name="rdchk"></param>
        /// <param name="bloq"></param>
        /// <param name="fuente"></param>
        /// <param name="topcodiYupana"></param>
        /// <returns></returns>
        public MigracionesModel ProcesarDigsilent(string program, string fecha, int rdchk, string bloq, int fuente, int topcodiYupana)
        {
            MigracionesModel modelo = new MigracionesModel();
            try
            {
                var validacion = servicio.ValidaCampos(program, fecha, rdchk, bloq, fuente, topcodiYupana);

                if (validacion.nRegistros == -1)
                {
                    modelo.nRegistros = -1;
                    modelo.Mensaje = validacion.Mensaje;
                    return modelo;
                }
                else
                {
                    DateTime fechaPeriodo = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                    servicio.ProcesarDIgSILENT(fechaPeriodo, program, rdchk, bloq, fuente, topcodiYupana, out string resultado, out string comentario, out string configuracionOpera, out string validacionDuplicadoForeignKey);
                    modelo.nRegistros = 1;
                    modelo.Resultado = resultado;
                    modelo.Comentario = comentario;
                    modelo.Resultado2 = configuracionOpera;
                    modelo.Resultado3 = validacionDuplicadoForeignKey;

                    //Generar Archivo .dle
                    servicio.SaveDigSilent(modelo.Resultado, fecha);

                }
            }
            catch (Exception ex)
            {
                modelo.nRegistros = -1;
                modelo.Mensaje = ex.Message;
                modelo.Detalle = ex.StackTrace;
            }
            return modelo;
        }
    }
}
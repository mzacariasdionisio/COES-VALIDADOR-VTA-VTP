using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.DemandaBarras.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.DemandaBarras
{
    /// <summary>
    /// Clase para manejo de lógica de las notificaciones por cumplimiento
    /// </summary>
    public class NotificacionAppServicio
    {
        /// <summary>
        /// Tipos de empresa
        /// </summary>
        private int[] TiposEmpresa = { 2, 4 };

        /// <summary>
        /// Modulo demanda en barras
        /// </summary>
        private int ModudoDemandaBarras = 2;

        /// <summary>
        /// Codigo formato demanda diario
        /// </summary>
        private int FormatoDemandaBarraDiario = 55;

        /// <summary>
        /// Codigo formato demanda semanal
        /// </summary>
        private int FormatoDemandaBarraSemanal = 56;

        /// <summary>
        /// Plantilla para el incumplimiento diario
        /// </summary>
        private int PlantillaIncumplimientoDiario = 7;

        /// <summary>
        /// Plantilla para el incumplimiento semanal
        /// </summary>
        private int PlantillaIncumplimientoSemanal = 8;
      
        /// <summary>
        /// Permite listar los tipos de empresas demanda en barras
        /// </summary>
        /// <returns></returns>
        public List<SiTipoempresaDTO> ListarTipoEmpresas()
        {
            return FactorySic.GetSiTipoempresaRepository().List().Where
                (x => TiposEmpresa.Any(y => x.Tipoemprcodi == y)).ToList();
        }

        /// <summary>
        /// Permite listar las empresas por tipo
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresas(string tipoEmpresas)
        {
            if (string.IsNullOrEmpty(tipoEmpresas)) tipoEmpresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(),
                this.TiposEmpresa);

            List<SiEmpresaDTO> entitys = FactorySic.GetSiEmpresaRepository().GetByCriteria(tipoEmpresas);
            List<int> idsPermitidos = FactorySic.GetSiEmpresaCorreoRepository().ObtenerEmpresasDisponibles();

            return entitys.Where(x => idsPermitidos.Any(y => x.Emprcodi == y)).ToList();
        }

        #region Cuentas adicionales

        /// <summary>
        /// Permite obtener las cuentas de los 
        /// </summary>
        /// <param name="tipoEmpresa"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public List<SiEmpresaCorreoDTO> ListarCuentaEmpresa(int tipoEmpresas, int empresa)
        {
            string parameter = tipoEmpresas.ToString();
            if (tipoEmpresas == -1) parameter  = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(),
                this.TiposEmpresa);
            return FactorySic.GetSiEmpresaCorreoRepository().GetByCriteria(this.ModudoDemandaBarras, parameter, empresa);
        }

        /// <summary>
        /// Permite obtener los datos de una cuenta adicional
        /// </summary>
        /// <param name="idCuenta"></param>
        /// <returns></returns>
        public SiEmpresaCorreoDTO ObtenerEntidadCuenta(int idCuenta)
        {
            return FactorySic.GetSiEmpresaCorreoRepository().GetById(idCuenta);
        }

        /// <summary>
        /// Permite grabar o editar una cuenta de correo adicional
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int GrabarCuenta(SiEmpresaCorreoDTO entity)
        {
            try
            {
                int id = 0;
                entity.Modcodi = this.ModudoDemandaBarras;
                if (entity.Empcorcodi == 0)
                {
                    id = FactorySic.GetSiEmpresaCorreoRepository().Save(entity);
                }
                else
                {
                    FactorySic.GetSiEmpresaCorreoRepository().Update(entity);
                    id = entity.Empcorcodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite eliminar la cuenta
        /// </summary>
        /// <param name="idCuenta"></param>
        public void EliminarCuenta(int idCuenta)
        {
            try
            {
                FactorySic.GetSiEmpresaCorreoRepository().Delete(idCuenta);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Plazos de envio

        /// <summary>
        /// Permite obtener los plazos
        /// </summary>
        /// <param name="plazoDiario"></param>
        /// <param name="plazoSemanal"></param>
        public void ObtenerPlazoFormato(int idEmpresa, DateTime fecha, out int plazoDiario, out int plazoSemanal)
        {
            MeFormatoDTO formatoDemandaDiaria = FactorySic.GetMeFormatoRepository().GetById(this.FormatoDemandaBarraDiario);
            MeFormatoDTO formatoDemandaSemanal = FactorySic.GetMeFormatoRepository().GetById(this.FormatoDemandaBarraSemanal);
            plazoDiario = formatoDemandaDiaria.Formatminplazo;
            plazoSemanal = formatoDemandaSemanal.Formatminplazo;

            MeAmpliacionfechaDTO ampliacionDiaria = FactorySic.GetMeAmpliacionfechaRepository().GetById(fecha, idEmpresa, this.FormatoDemandaBarraDiario);
            MeAmpliacionfechaDTO ampliacionSemanal = FactorySic.GetMeAmpliacionfechaRepository().GetById(fecha, idEmpresa, this.FormatoDemandaBarraSemanal);

            if (ampliacionDiaria != null)
            {
                TimeSpan ts = ampliacionDiaria.Amplifechaplazo.Subtract(ampliacionDiaria.Amplifecha);
                int minutos = Convert.ToInt32(ts.TotalMinutes);
                plazoDiario = minutos;
            }

            if (ampliacionSemanal != null)
            {
                TimeSpan ts = ampliacionSemanal.Amplifechaplazo.Subtract(ampliacionSemanal.Amplifecha);
                int minutos = Convert.ToInt32(ts.TotalMinutes);
                plazoSemanal = minutos;
            }
        }

        /// <summary>
        /// Permite enviar la notificación a la empresas que incumplieron carga
        /// </summary>
        public void NotificacionCargaDatos()
        {
            // Obtenemos todas las cuentas de todos las empreas
            List<SiEmpresaCorreoDTO> cuentas = this.ListarCuentaEmpresa(-1, -1).Where(x => x.Emprcodi != 34 &&
                x.Emprcodi != 107 && x.Indnotificacion == ConstantesAppServicio.SI).Select(x => new SiEmpresaCorreoDTO
            {
                Emprcodi = x.Emprcodi,
                Empcoremail = x.Empcoremail
            }).ToList();

            // Verificamos si existen registros para el diario
            //VERSION ANTIGUA 20200227 List<SiEmpresaCorreoDTO> empresasDiario = FactorySic.GetSiEmpresaCorreoRepository().ObtenerEmpresasIncumplimiento(2,
            //    DateTime.Now);
            List<SiEmpresaCorreoDTO> empresasDiario = FactorySic.GetSiEmpresaCorreoRepository().ObtenerEmpresasIncumplimiento(95,
                DateTime.Now);

            // Enviamos las notificaciones del programa diario
            this.EnviarNotificacion(empresasDiario, this.PlantillaIncumplimientoDiario, cuentas, this.FormatoDemandaBarraDiario);
                        
            // Verificamos si existen registros para el semanal
            if ((int)DateTime.Now.DayOfWeek == 2)
            {
                // Obtenemos la fecha de inicio de semana siguiente
                DateTime fechaSemana = DateTime.Now.AddDays(4);
                // VERSION ANTIGUA 20200227  List<SiEmpresaCorreoDTO> empresasSemanal = FactorySic.GetSiEmpresaCorreoRepository().ObtenerEmpresasIncumplimiento(3,
                //       fechaSemana);
                List<SiEmpresaCorreoDTO> empresasSemanal = FactorySic.GetSiEmpresaCorreoRepository().ObtenerEmpresasIncumplimiento(71, fechaSemana);
              // Enviamos las notificaciones del programa semanal
              this.EnviarNotificacion(empresasSemanal, this.PlantillaIncumplimientoSemanal, cuentas, this.FormatoDemandaBarraSemanal);
            }
        }

        /// <summary>
        /// Permite obtener el listado de empresas a notificar
        /// </summary>
        public List<SiEmpresaCorreoDTO> ConfigurarEmpresasNotificacion()
        {
            // Obtenemos todas las cuentas de todos las empreas
            List<SiEmpresaCorreoDTO> cuentas = this.ListarCuentaEmpresa(-1, -1).Where(x => x.Emprcodi != 34 &&
                x.Emprcodi != 107).Select(x => new SiEmpresaCorreoDTO
                {
                    Emprcodi = x.Emprcodi,
                    Empcoremail = x.Empcoremail,
                    Indnotificacion = x.Indnotificacion,
                    Emprnomb = x.Emprnomb
                }).ToList();

            List<SiEmpresaCorreoDTO> result = new List<SiEmpresaCorreoDTO>();

            var cuentasDistinct = cuentas.Select(x => new { x.Emprcodi, x.Emprnomb, x.Indnotificacion }).Distinct().ToList();

            foreach (var item in cuentasDistinct)
            {
                result.Add(new SiEmpresaCorreoDTO
                {
                    Emprcodi = item.Emprcodi,
                    Emprnomb = item.Emprnomb,
                    Indnotificacion = item.Indnotificacion
                });
            }

            return result;
        }

        /// <summary>
        /// Permite indicar las empresas que recibiran notificacion
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="indicador"></param>
        /// <param name="lastuser"></param>
        public void EstablecerNotificacion(int emprcodi, string indicador, string lastuser)
        {
            try
            {
                FactorySic.GetSiEmpresaCorreoRepository().ActualizarIndNotifacion(emprcodi, indicador, lastuser);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite actualizar las notificaciones para las empresas
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="lastUser"></param>
        public void ActualizarNotificacionEmpresas(List<int> idsEmpresa, string lastUser)
        {
            try
            {
                List<SiEmpresaCorreoDTO> entitys = this.ConfigurarEmpresasNotificacion();

                foreach (SiEmpresaCorreoDTO item in entitys)
                {
                    string indicador = ConstantesAppServicio.NO;
                    if (idsEmpresa.Where(x => x == item.Emprcodi).Count() > 0)
                    {
                        indicador = ConstantesAppServicio.SI;
                    }
                    FactorySic.GetSiEmpresaCorreoRepository().ActualizarIndNotifacion(item.Emprcodi, indicador, lastUser);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar la configuración del proceso de notificaciones
        /// </summary>
        /// <param name="estado"></param>
        public void ConfigurarProceso(string estado)
        {
            try
            {
                int idProceso = ConstantesDemandaBarras.IdProcesoNotificacion;
                FactorySic.GetSiProcesoRepository().ActualizarEstado(idProceso, estado);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener el estado actual del proceso
        /// </summary>
        /// <returns></returns>
        public string ObtenerEstadoProceso()
        {
            SiProcesoDTO entity = FactorySic.GetSiProcesoRepository().GetById(ConstantesDemandaBarras.IdProcesoNotificacion);
            return entity.Prcsestado;
        }


        /// <summary>
        /// Permite enviar la notificaciona a las cuentas creadas
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="cuentas"></param>
        public void EnviarNotificacion(List<SiEmpresaCorreoDTO> empresas, int idPlantilla, List<SiEmpresaCorreoDTO> cuentas, int idFormato)
        {
            SiPlantillacorreoDTO plantilla = FactorySic.GetSiPlantillacorreoRepository().GetById(idPlantilla);

            foreach (var empresa in empresas)
            {
                List<string> cuentaEmpresa = cuentas.Where(x => x.Emprcodi == empresa.Emprcodi
                    && COES.Base.Tools.Util.ValidarEmail(x.Empcoremail)).Select(x => x.Empcoremail).ToList();

                // Enviamos la notificacion
                string asunto = plantilla.Plantasunto;
                string contenido = string.Format(plantilla.Plantcontenido, empresa.Emprnomb);

                if (cuentaEmpresa.Count > 0)
                {
                    string indicador = ConfigurationManager.AppSettings[ConstantesAppServicio.IndicadorNotificacionPR03];

                    if (indicador == ConstantesAppServicio.SI)
                    {
                        COES.Base.Tools.Util.SendEmail(cuentaEmpresa, new List<string>(), new List<string>(), asunto, contenido,
                            plantilla.PlanticorreoFrom);
                    }

                    SiCorreoDTO correo = new SiCorreoDTO
                    {
                        Corrasunto = asunto,
                        Corrcontenido = contenido,
                        Corrto = string.Join(ConstantesAppServicio.CaracterComa.ToString(), cuentaEmpresa),
                        Emprcodi = empresa.Emprcodi,
                        Plantcodi = idPlantilla,
                        Corrfrom = plantilla.PlanticorreoFrom,
                        Corrfechaenvio = DateTime.Now
                    };

                    // Grabamos el logs de correos
                    FactorySic.GetSiCorreoRepository().Save(correo);

                    // Insertamos el registro de amplicación de plazo
                    MeAmpliacionfechaDTO ampliacion = new MeAmpliacionfechaDTO
                    {
                        Emprcodi = empresa.Emprcodi,
                        Amplifecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                        Amplifechaplazo = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)).AddMinutes(8.5 * 60),
                        Formatcodi = idFormato,
                        Lastdate = DateTime.Now
                    };

                    FactorySic.GetMeAmpliacionfechaRepository().Save(ampliacion);
                }
            }
        }

        /// <summary>
        /// Permite listar los logs de correos enviados
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<SiCorreoDTO> ListarLogCorreo(DateTime fecha) 
        {
            List<int> ids = new List<int>();
            ids.Add(this.PlantillaIncumplimientoDiario);
            ids.Add(this.PlantillaIncumplimientoSemanal);

            return FactorySic.GetSiCorreoRepository().ListarLogCorreo(fecha, string.Join<int>(
                ConstantesAppServicio.CaracterComa.ToString(), ids));
        }


        #endregion
    }
}

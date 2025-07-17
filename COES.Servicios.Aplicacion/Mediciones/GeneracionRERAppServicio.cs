using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using System.Linq;
using log4net;
using System.Text;
using COES.Framework.Base.Tools;
using System.Globalization;
using COES.Servicios.Aplicacion.Mediciones.Helper;

namespace COES.Servicios.Aplicacion.Mediciones
{
    public class GeneracionRERAppServicio
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(GeneracionRERAppServicio));

        #region Métodos Tabla WB_GENERACIONRER

        /// <summary>
        /// Inserta un registro de la tabla WB_GENERACIONRER
        /// </summary>
        public int SaveWbGeneracionrer(int indicador, int ptoCentral, int? ptoUnidad, string userName)
        {
            try
            {
                int resultado = 0;
                int ptoMedicion = 0;
                string indCentral = string.Empty;

                if (indicador == 1)
                {
                    int countUnidad = FactorySic.GetWbGeneracionrerRepository().ValidarExistencia((int)ptoUnidad);
                    if (countUnidad == 0)
                    {
                        int countCentral = FactorySic.GetWbGeneracionrerRepository().ValidarExistencia(ptoCentral);
                        if (countCentral == 0)
                        {
                            ptoMedicion = (int)ptoUnidad;
                            resultado = 1;      
                            indCentral = ConstantesAppServicio.NO;
                        }
                        else 
                        {
                            resultado = 2; //existe central
                        }
                    }
                    else 
                    {
                        resultado = 3; //ya existe unidad
                    }
                }
                else 
                {
                    int countCentral = FactorySic.GetWbGeneracionrerRepository().ValidarExistencia(ptoCentral);
                    if (countCentral == 0)
                    {
                        int countUnidad = FactorySic.GetWbGeneracionrerRepository().ValidarExistenciaUnidad(ptoCentral);
                        if (countUnidad == 0)
                        {
                            ptoMedicion = ptoCentral;
                            resultado = 1;    
                            indCentral = ConstantesAppServicio.SI;
                        }
                        else 
                        {
                            resultado = 4; //existen unidades agregadas
                        }
                    }
                    else 
                    {
                        resultado = 5; //existe central
                    }
                }
                
                if (resultado == 1)
                {

                    int validacionGeneral = FactorySic.GetWbGeneracionrerRepository().ValidarExistenciaGeneral(ptoMedicion);
                                        
                    WbGeneracionrerDTO entity = new WbGeneracionrerDTO();
                    entity.Ptomedicodi = ptoMedicion;
                    entity.IndPorCentral = indCentral;
                    entity.Feccreate = DateTime.Now;
                    entity.Fecupdate = DateTime.Now;
                    entity.Usercreate = userName;
                    entity.Userupdate = userName;
                    entity.Estado = ConstantesAppServicio.Activo;

                    if (validacionGeneral == 0)
                    {
                        FactorySic.GetWbGeneracionrerRepository().Save(entity);
                    }
                    else 
                    {
                        FactorySic.GetWbGeneracionrerRepository().Update(entity);
                    }
                }
                
                return resultado;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla WB_GENERACIONRER
        /// </summary>
        public void UpdateWbGeneracionrer(WbGeneracionrerDTO entity)
        {
            try
            {
                FactorySic.GetWbGeneracionrerRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla WB_GENERACIONRER
        /// </summary>
        public void DeleteWbGeneracionrer(int ptomedicodi, string lastUser)
        {
            try
            {
                FactorySic.GetWbGeneracionrerRepository().Delete(ptomedicodi, lastUser, DateTime.Now);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void GrabarConfiguracion(int ptomedicodi, decimal? minimo, decimal? maximo, string lastuser)
        {
            try
            {
                FactorySic.GetWbGeneracionrerRepository().GrabarConfiguracion(ptomedicodi, minimo, maximo, lastuser);
            }
            catch (Exception ex)
            {

                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_GENERACIONRER
        /// </summary>
        public WbGeneracionrerDTO GetByIdWbGeneracionrer(int ptomedicodi)
        {
            return FactorySic.GetWbGeneracionrerRepository().GetById(ptomedicodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_GENERACIONRER
        /// </summary>
        public List<WbGeneracionrerDTO> ListWbGeneracionrers()
        {
            return FactorySic.GetWbGeneracionrerRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla WbGeneracionrer
        /// </summary>
        public List<WbGeneracionrerDTO> GetByCriteriaWbGeneracionrers()
        {
            return FactorySic.GetWbGeneracionrerRepository().GetByCriteria();
        }


        /// <summary>
        /// Permite obtener las empresas que reportarán
        /// </summary>
        /// <returns></returns>
        public List<WbGeneracionrerDTO> ObtenerPuntosEmpresas()
        {
            return FactorySic.GetWbGeneracionrerRepository().ObtenerPuntosEmpresas();
        }

        /// <summary>
        /// Permite obtener las centrales por empresa seleccianda
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<WbGeneracionrerDTO> ObtenerPuntosCentrales(int idEmpresa)
        {
            return FactorySic.GetWbGeneracionrerRepository().ObtenerPuntosCentrales(idEmpresa);
        }

        /// <summary>
        /// Permite obtener las unidades por cada central seleccionada
        /// </summary>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        public List<WbGeneracionrerDTO> ObtenerPuntosUnidades(int ptoCentral)
        {
            return FactorySic.GetWbGeneracionrerRepository().ObtenerPuntosUnidades(ptoCentral);
        }

        /// <summary>
        /// Permite obtner la lista de puntos para elaborar formato
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<WbGeneracionrerDTO> ObtenerListaFormato(int idEmpresa)
        {
            return FactorySic.GetWbGeneracionrerRepository().ObtenerPuntoFormato(idEmpresa);
        }

        #endregion

        #region Metodos Servicio Web

      
        /// <summary>
        /// Permite obtener los puntos de medicion RER por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<CodigoCargaRER> ObtenerCodigosDeCarga(string userLogin)
        {
            string empresas = FactorySic.GetWbGeneracionrerRepository().ObtenerEmpresaPorUsuario(userLogin);

            if (!string.IsNullOrEmpty(empresas))
            {
                List<WbGeneracionrerDTO> list = FactorySic.GetWbGeneracionrerRepository().GetByCriteria();
                List<int> ids = empresas.Split(',').Select(x => x.Trim()).Select(x => Int32.Parse(x)).ToList();
                List<WbGeneracionrerDTO> entitys = list.Where(x => ids.Any(y => y == x.EmprCodi)).ToList();

                List<CodigoCargaRER> resultado = new List<CodigoCargaRER>();

                foreach (WbGeneracionrerDTO entity in entitys)
                {
                    resultado.Add(new CodigoCargaRER
                    {
                        Central = entity.Central,
                        Empresa = entity.EmprNomb,
                        Indicador = entity.IndPorCentral,
                        PuntoMedicion = entity.Ptomedicodi,
                        Unidad = entity.EquiNomb
                    });
                }

                return resultado;
            }

            return new List<CodigoCargaRER>();
        }

        /// <summary>
        /// Permite realizar la carga de datos retornando una lista de validaciones en caso existieran
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="fecha"></param>
        /// <param name="nroSemana"></param>
        /// <param name="valores"></param>
        /// <returns></returns>
        public int CargarDatos(int horizonte, DateTime fecha, int anio, int nroSemana, List<Medicion48> valores, string userLogin)
        {
            try
            {
                int estado = 0;
                int countExistente = 0;
                List<CodigoCargaRER> listCodigos = this.ObtenerCodigosDeCarga(userLogin);
                List<int> codigosPermitidos = listCodigos.Select(x => x.PuntoMedicion).Distinct().ToList();
                List<int> codigosEnviados = valores.Select(x => x.PtoMedicion).Distinct().ToList();

                int contador = codigosEnviados.Where(x => codigosPermitidos.Any(y => y == x)).Count();

                if (contador == codigosEnviados.Count)
                {
                    int lectCodi = (horizonte == 0) ? ConstantesMedicion.LecturaProgDiaraRER : ConstantesMedicion.LecturaProgSemanalRER;
                    List<string> validaciones = new List<string>();

                    if (horizonte == 0)
                    {
                        if (codigosEnviados.Count == valores.Count)
                        {
                            DateTime fechaProceso = valores[0].Medifecha;

                            if (fechaProceso.Year == fecha.Year && fechaProceso.Month == fecha.Month && fechaProceso.Day == fecha.Day)
                            {
                                estado = this.GrabarDatosRER(horizonte, lectCodi, valores, fechaProceso, fechaProceso, anio, nroSemana, userLogin, out countExistente);

                                if (countExistente > 0)
                                {
                                    if (estado == 1)
                                    {
                                        estado = 3;
                                    }
                                    else if (estado == 2)
                                    {
                                        estado = 4;
                                    }
                                }

                            }
                            else
                            {
                                estado = 5;
                            }
                        }
                        else
                        {
                            estado = 6;
                        }
                    }
                    if (horizonte == 1)
                    {
                        DateTime fechaInicio = EPDate.f_fechainiciosemana(anio, nroSemana);
                        DateTime fechaFin = fechaInicio.AddDays(6);

                        if (codigosEnviados.Count * 7 == valores.Count)
                        {
                            List<Medicion48> list = valores.OrderBy(x => x.Medifecha).ToList();
                            DateTime inicio = list[0].Medifecha;
                            DateTime fin = list[valores.Count -1].Medifecha;

                            if ((fechaInicio.Year == inicio.Year && fechaInicio.Month == inicio.Month && fechaInicio.Day == inicio.Day) &&
                                (fechaFin.Year == fin.Year && fechaFin.Month == fin.Month && fechaFin.Day == fin.Day))
                            {
                                estado = this.GrabarDatosRER(horizonte, lectCodi, valores, fechaInicio, fechaFin, anio, nroSemana, userLogin, out countExistente);

                                if (countExistente > 0)
                                {
                                    if (estado == 1)
                                    {
                                        estado = 3;
                                    }
                                    else if (estado == 2)
                                    {
                                        estado = 4;
                                    }
                                }
                            }
                            else
                            {
                                estado = 7;
                            }
                        }
                        else
                        {
                            estado = 8;
                        }
                    }
                }
                else
                {
                    estado = 9;
                }
                return estado;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Almacena en base de datos los datos de generacion
        /// </summary>
        /// <param name="lectCodi"></param>
        /// <param name="valores"></param>
        public int GrabarDatosRER(int horizonte, int lectCodi, List<Medicion48> valores, DateTime fechaInicio, 
            DateTime fechaFin, int anio, 
            int nroSemana, string userLogin, out int countExistente)
        {
            try
            {
                List<int> puntos = valores.Select(x => x.PtoMedicion).Distinct().ToList();
                countExistente = FactorySic.GetMeMedicion48Repository().EliminarValoresCargadosPorPunto(puntos, lectCodi, fechaInicio, fechaFin);
                int ptoMedicion = 0;

                foreach (Medicion48 item in valores)
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();
                    entity.Tipoinfocodi = ConstantesMedicion.TipoInformacionRER;
                    entity.Lectcodi = lectCodi;
                    entity.Lastdate = DateTime.Now;
                    entity.Ptomedicodi = item.PtoMedicion;
                    entity.Medifecha = item.Medifecha;
                    ptoMedicion = item.PtoMedicion;

                    decimal suma = 0;
                    decimal valor = 0;
                    for (int i = 1; i <= 48; i++)
                    {
                        valor = Convert.ToDecimal(item.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(item, null));
                        entity.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(entity, valor);
                        suma = suma + valor;
                    }
                    entity.Meditotal = suma;

                    FactorySic.GetMeMedicion48Repository().Save(entity);
                }

                int idEmpresa = FactorySic.GetMeMedicion48Repository().ObtenerEmpresaPorPuntoMedicion(ptoMedicion);
                bool validacion = this.ValidarFecha(anio, horizonte, fechaInicio, nroSemana);
                int estado = (validacion) ? 1 : 5;

                ExtLogenvioDTO log = new ExtLogenvioDTO();
                log.Emprcodi = idEmpresa;
                log.Estenvcodi = estado;
                log.Feccarga = fechaInicio;
                log.Nrosemana = nroSemana;
                log.Filenomb = ConstantesMedicion.ViaWebService;
                log.Lastdate = DateTime.Now;
                log.Lastuser = userLogin;
                log.Fecproceso = DateTime.Now;
                log.NroAnio = anio;
                log.Origlectcodi = ConstantesMedicion.OrigenLecturaGeneracionRER;
                log.Lectcodi = lectCodi;
                FactorySic.GetExtLogenvioRepository().Save(log);

                if (estado == 5) estado = 2;
                return estado;
            }
            catch (Exception)
            {
                countExistente = 0;
                return -1;
            }
        }
        
        /// <summary>
        /// Permite validar una fecha para el ingreso
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="mensaje"></param>
        protected bool ValidarFecha(int anio, int horizonte, DateTime fecha, int? nroSemama)
        {
            if (horizonte == 0)
            {
                DateTime fechaActual = DateTime.ParseExact(DateTime.Now.ToString(ConstantesMedicion.FormatoFecha),
                    ConstantesMedicion.FormatoFecha, CultureInfo.InvariantCulture);

                TimeSpan ts = fecha.Subtract(fechaActual);

                if (ts.Days > 1)
                {
                    return true;
                }
                else if (ts.Days < 1)
                {
                    return false;
                }
                else
                {
                    if (DateTime.Now.Hour * 60 + DateTime.Now.Minute > 545)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                int semanaActual = EPDate.f_numerosemana(DateTime.Now);

                if (anio >= DateTime.Now.Year)
                {
                    if (anio == DateTime.Now.Year)
                    {
                        if (semanaActual < nroSemama)
                        {
                            if (semanaActual == nroSemama - 1)
                            {
                                int dia = (int)DateTime.Now.DayOfWeek;

                                if ((int)DateTime.Now.DayOfWeek < 2)
                                {
                                    return true;
                                }
                                else if ((int)DateTime.Now.DayOfWeek == 2)
                                {
                                    if (DateTime.Now.Hour * 60 + DateTime.Now.Minute > 885)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        return true;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else if (semanaActual >= nroSemama)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (anio == DateTime.Now.Year + 1)
                        {
                            int dia = (int)DateTime.Now.DayOfWeek;

                            if ((int)DateTime.Now.DayOfWeek < 2)
                            {
                                return true;
                            }
                            else if ((int)DateTime.Now.DayOfWeek == 2)
                            {
                                if (DateTime.Now.Hour * 60 + DateTime.Now.Minute > 885)
                                {
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        #endregion

        #region CargaDatosRer

        /// <summary>
        /// Obtener datos RER cargados de la Extranet
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="horizonte"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ConsultaDatosRer(int idEmpresa, int horizonte, DateTime fechaIni, DateTime fechaFin)
        {
            int lectCodi = (horizonte == 0) ? ConstantesMedicion.LecturaProgDiaraRER : ConstantesMedicion.LecturaProgSemanalRER;

            if (idEmpresa == -1) idEmpresa = 0;
            //cosulta de Gneración rer
            var list = FactorySic.GetMeMedicion48Repository().ObtenerGeneracionRER(idEmpresa, lectCodi, fechaIni, fechaFin);

            return list;
        }

        #endregion

        #region Mejoras RDO
        /// <summary>
        /// Obtener datos RERNC cargados de la Extranet
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="horizonte"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<Object> ConsultaDatosRerNC(int idEmpresa, int horizonte, DateTime fechaIni, DateTime fechaFin)
        {
            int lectCodi = (horizonte == 0) ? ConstantesMedicion.LecturaProgDiaraRER : ConstantesMedicion.LecturaProgSemanalRER;
            List<Object> listaGeneracionRer = new List<Object>();
            if (idEmpresa == -1) idEmpresa = 0;
            //cosulta de Gneración rer
            var list = FactorySic.GetMeMedicion48Repository().ObtenerGeneracionRERNC(idEmpresa, lectCodi, fechaIni, fechaFin);

            foreach (var reg in list)
            {
                listaGeneracionRer.Add(reg);
            }
            return listaGeneracionRer;
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using COES.Servicios.Distribuidos.Contratos;
using COES.Dominio.DTO.Sic;
using log4net;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System.ServiceModel.Web;
using System.Net;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Distribuidos.SeguridadServicio;
using COES.Servicios.Distribuidos.Resultados;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Mediciones.Helper;

namespace COES.Servicios.Distribuidos.Servicios
{
    /// <summary>
    /// Servicio que expone la funcionalidad de formatos de envío
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class FormatoServicio : IFormatoServicio
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(FormatoServicio));
        public const string FormatoFecha = "dd/MM/yyyy";

        /// <summary>
        /// Constuctor
        /// </summary>
        public FormatoServicio()
        {
            log4net.Config.XmlConfigurator.Configure();

        }

        public MeFormatoDTO GetFormato(int formatcodi)
        {
            if (formatcodi < 1)
            {
                throw new FaultException(new FaultReason("El código de formato no es válido."), new FaultCode("1"));
            }
            MeFormatoDTO formato = new MeFormatoDTO();
            try
            {
                FormatoMedicionAppServicio formatoServicio = new FormatoMedicionAppServicio();
                formato = formatoServicio.GetByIdMeFormato(formatcodi);
            }
            catch (Exception ex)
            {
                log.Error("GetFormato", ex);
                throw new FaultException(new FaultReason("Ocurrió un error al consultar formato."), new FaultCode("3"));
            }
            if (formato == null)
            {
                throw new FaultException(new FaultReason("El formato consultado no existe."), new FaultCode("2"));
            }
            return formato;
        }

        public List<MeMedicion96DTO> ConsultarEnvio15min(int emprcodi, int enviocodi, int formatcodi)
        {
            if (formatcodi < 1)
            {
                throw new FaultException(new FaultReason("El código de formato no es válido."), new FaultCode("1"));
            }
            if (emprcodi < 1)
            {
                throw new FaultException(new FaultReason("El código de empresa no es válido."), new FaultCode("2"));
            }
            if (enviocodi < 1)
            {
                throw new FaultException(new FaultReason("El código de envío no es válido."), new FaultCode("3"));
            }
            FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
            List<MeMedicion96DTO> resultado = new List<MeMedicion96DTO>();
            List<MeMedicion96DTO> lista96 = new List<MeMedicion96DTO>();
            try
            {

                var envioAnt = servFormato.GetByIdMeEnvio(enviocodi);
                var formato = servFormato.GetByIdMeFormato(formatcodi);

                if (envioAnt != null)
                {
                    formato.FechaEnvio = envioAnt.Enviofecha;                    
                    formato.FechaProceso = (DateTime)envioAnt.Enviofechaperiodo;
                }
                else
                    formato.FechaProceso = DateTime.MinValue;

                FormatoMedicionAppServicio.GetSizeFormato(formato);
                resultado = servFormato.GetDataFormato96(lista96, envioAnt.Emprcodi.Value, formato, enviocodi, -1);                
            }
            catch (Exception ex)
            {
                log.Error("ConsultarEnvio15min", ex);
                throw new FaultException(new FaultReason("Ocurrió un error al consultar datos de envío."), new FaultCode("5"));
            }
            if (resultado == null || resultado.Count == 0)
            {
                throw new FaultException(new FaultReason("No existen datos para el envío consultado."), new FaultCode("4"));
            }
            return resultado;
        }

        public int GrabarValores96(List<MeMedicion96DTO> entitys, string usuario, int emprcodi, int formatcodi, string fecha, string semana, string mes)
        {
            int idEnvio;
        
                if (formatcodi < 1)
                {
                    throw new FaultException(new FaultReason("El código de formato no es válido."), new FaultCode("1"));
                }
                if (emprcodi < 1)
                {
                    throw new FaultException(new FaultReason("El código de empresa no es válido."), new FaultCode("2"));
                }
                if (entitys == null || entitys.Count == 0)
                {
                    throw new FaultException(new FaultReason("No hay valores que grabar."), new FaultCode("3"));
                }
                if (usuario.Trim().Length == 0)
                {
                    throw new FaultException(new FaultReason("Debe especificar un usuario válido."), new FaultCode("4"));
                }
                if ((fecha == null && semana == null && mes == null) || (fecha.Trim().Length == 0 && semana.Trim().Length == 0 && mes.Trim().Length == 0))
                {
                    throw new FaultException(new FaultReason("Debe especificar un valor para día, semana o mes."), new FaultCode("5"));
                }

                if (mes != null && mes.Trim().Length > 1)
                {
                    if (mes.Trim().Length != 7)
                    {
                        throw new FaultException(new FaultReason("El periodo mensual debe estar en el formato 'MM YYYY'"), new FaultCode("6"));
                    }
                    else
                    {
                        try
                        {
                            int imes = Int32.Parse(mes.Substring(0, 2));
                            int ianho = Int32.Parse(mes.Substring(3, 4));
                        }
                        catch (Exception)
                        {
                            throw new FaultException(new FaultReason("El periodo mensual debe estar en el formato 'MM YYYY'"), new FaultCode("6"));
                        }
                    }
                }

                FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
                MeFormatoDTO formato = servFormato.GetByIdMeFormato(formatcodi);
                formato.ListaHoja = servFormato.GetByCriteriaMeHoja(formatcodi);
                formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, semana, fecha, "dd/MM/yyyy");
                formato.Emprcodi = emprcodi;


                var cabecera = servFormato.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
                formato.Formatcols = cabecera.Cabcolumnas;
                formato.Formatrows = cabecera.Cabfilas;
                formato.Formatheaderrow = cabecera.Cabcampodef;
                int filaHead = formato.Formatrows;
                int colHead = formato.Formatcols;

                /////////////// Obtiene Fecha Inicio y Fecha Fin del Proceso //////////////
                formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, semana, fecha, "dd/MM/yyyy");
                FormatoMedicionAppServicio.GetSizeFormato(formato);


                MeConfigformatenvioDTO config = new MeConfigformatenvioDTO();
                config.Formatcodi = formatcodi;
                config.Emprcodi = emprcodi;
                config.FechaInicio = formato.FechaFin;
                int idConfig = servFormato.GrabarConfigFormatEnvio(config);
                Boolean enPlazo = servFormato.ValidarPlazo(formato);

                //Validación de grabar datos de Medidores de Generación
                if (ConstantesMedidores.IdFormatoCargaCentralPotReactiva == formatcodi)
                {
                    if (!enPlazo)
                    {
                        //verificar si existe data de Potencia Reactiva
                        var listaEnvios = servFormato.GetByCriteriaMeEnvios(emprcodi, ConstantesMedidores.IdFormatoCargaCentralPotActiva, formato.FechaProceso);
                        if (listaEnvios.Count == 0)
                        {
                            throw new FaultException(new FaultReason("No existen datos de  Potencia Activa"), new FaultCode("7"));
                        }
                    }
                }
                if (ConstantesMedidores.IdFormatoCargaServAuxPotActiva == formatcodi)
                {
                    if (!enPlazo)
                    {
                        //verificar si existe data de Potencia Reactiva
                        var listaEnvios = servFormato.GetByCriteriaMeEnvios(emprcodi, ConstantesMedidores.IdFormatoCargaCentralPotActiva, formato.FechaProceso);
                        if (listaEnvios.Count == 0)
                        {
                            throw new FaultException(new FaultReason("No existen datos de  Potencia Activa"), new FaultCode("7"));
                        }
                    }
                }

                MeEnvioDTO envio = new MeEnvioDTO();
                envio.Archcodi = 0;
                envio.Emprcodi = emprcodi;
                envio.Enviofecha = DateTime.Now;
                envio.Enviofechaperiodo = formato.FechaProceso;
                envio.Enviofechaini = formato.FechaInicio;
                envio.Enviofechafin = formato.FechaFin;
                envio.Envioplazo = (enPlazo) ? "P" : "F";
                envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
                envio.Lastdate = DateTime.Now;
                envio.Lastuser = usuario;
                envio.Userlogin = usuario;
                envio.Formatcodi = formatcodi;
                envio.Fdatcodi = 0;
                envio.Cfgenvcodi = idConfig;
                idEnvio = servFormato.SaveMeEnvio(envio);
                //var cabecera = servFormato.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();

                //obtener el formato del periodo anterior para comparar la similitud de data                        
                MeFormatoDTO formatoAnterior = servFormato.GetByIdMeFormato(formatcodi);
                formatoAnterior.Formatcols = cabecera.Cabcolumnas;
                formatoAnterior.Formatrows = cabecera.Cabfilas;
                formatoAnterior.Formatheaderrow = cabecera.Cabcampodef;
                formatoAnterior.FechaProceso = servFormato.GetFechaProcesoAnterior(formatoAnterior.Formatperiodo, formato.FechaProceso);
                FormatoMedicionAppServicio.GetSizeFormato(formatoAnterior);
                bool existePeriodoAnteriorSimilar96 = false && servFormato.ValidarDataPeriodoAnterior96(entitys, emprcodi, formatoAnterior);

                if (!existePeriodoAnteriorSimilar96)
                {
                //grabar
                    try
                    {
                    log.Info(formato.Formatcodi + " " + formato.Lectcodi + " " + formato.FechaInicio + " " + formato.FechaFin);
                        servFormato.GrabarValoresCargados96(entitys, usuario, idEnvio, emprcodi, formato);
                    }
                    catch (Exception ex)
                    {
                        log.Error("GrabarValores96", ex);
                        throw new FaultException(new FaultReason("Ocurrió un error al grabar valores."), new FaultCode("9"));
                    }
                }
                else
                {
                    throw new FaultException(new FaultReason("Los datos del periodo anterior son similares al periodo seleccionado"), new FaultCode("8"));
                }            
            return idEnvio;
        }

        public bool ValidaPlazoEnvio(int formatcodi, int emprcodi, string fecha, string semana, string mes)
        {
            if (formatcodi < 1)
            {
                throw new FaultException(new FaultReason("El código de formato no es válido"), new FaultCode("1"));
            }
            if (emprcodi < 1)
            {
                throw new FaultException(new FaultReason("El código de empresa no es válido"), new FaultCode("2"));
            }
            FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
            bool bValido = false;

            try
            {
                var formato = servFormato.GetByIdMeFormato(formatcodi);
                formato.Emprcodi = emprcodi;
                formato.FechaProceso= EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, semana, fecha, FormatoFecha);
                FormatoMedicionAppServicio.GetSizeFormato(formato);
                bValido = servFormato.ValidarPlazo(formato);
            }
            catch (Exception ex)
            {
                log.Error("ValidaPlazoEnvio", ex);
                throw new FaultException(new FaultReason("Ocurrió un error al validar plazo de envío."), new FaultCode("3"));
            }

            return bValido;
        }
    }
}
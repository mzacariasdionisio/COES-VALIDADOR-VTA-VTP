// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: alpha
//
// Fecha creacion: 24/10/2016
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================

using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper
{

    public class SincronizaCuenca : SincronizaCOESEquipo
    {

        #region CAMPOS: Variables de la clase.

        /// <summary>
        /// Miembro de la clase que sostiene las cuencas de la sincronización.
        /// </summary>
        private roObtenerMaestroCuenca roObtenerCuencaOsi = null;

        #endregion

        #region CONSTRUCTORES: Definicion de constructores de la clase.

        public SincronizaCuenca()
            : base()
        {
            
        }

        public SincronizaCuenca(SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(servicioOsiSincroniza)
        {

        }

        public SincronizaCuenca(DateTime fechaSincroniza, SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(fechaSincroniza, servicioOsiSincroniza)
        {
            this.roObtenerCuencaOsi = this.servicioOsiSincronizacion.obtenerMaestroCuenca();
            if (this.roObtenerCuencaOsi.listaCuencas == null)
            {
                this.roObtenerCuencaOsi.listaCuencas = new cuencaDTO[] { };
            }
        }

        #endregion

        #region METODOS: Metodos de la clase.

        public override EntidadSincroniza ObtenerEntidadSincroniza()
        {
            return EntidadSincroniza.Cuenca;
        }

        public override int ObtenerIdFamilia()
        {
            return int.Parse(FamiliaCodi.CuarentaYUno.ToString("D"));
        }

        public override int IniciarSincronizacion()
        {
            if (this.roObtenerCuencaOsi.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                this.listaError.Add(new ErrorSincroniza(this.roObtenerCuencaOsi.codigoMensaje
                                                      , this.roObtenerCuencaOsi.mensajeResultante));
                return 0;
            }

            return base.IniciarSincronizacion();
        }

        protected override int CrearRegistroEnOsinergmin(EntityBase registroEntidad)
        {
            EqEquipoDTO equipoCOES = (EqEquipoDTO)registroEntidad;

            //- 1. Calculando el correlativo a utilizar para el nuevo registro.
            //- ===============================================================

            if (!this.correlativoCodigoOsinergmin.HasValue)
            {
                //- El correlativo aún no fue identificado, entonces se procede con el cálculo:
                var cuencasOsiFiltrados = from cuencasOsi in this.roObtenerCuencaOsi.listaCuencas
                                          select cuencasOsi;

                if (cuencasOsiFiltrados.Count() == 0)
                {
                    //- No se ha encontrador registro alguno.
                    this.correlativoCodigoOsinergmin = 0;
                }
                else
                {
                    cuencaDTO actualCuencaOsi = null;

                    try
                    {
                        actualCuencaOsi = cuencasOsiFiltrados.OrderByDescending(cuencaOsi
                        =>
                        {
                            if (string.IsNullOrEmpty(cuencaOsi.codigoCuenca))
                            {
                                this.RegistrarAsignacion(equipoCOES
                                                        , MotivoPendiente.CuencaOsiSinCodigoCuenca
                                                        , string.Empty);
                                //- Basta que una de las cuencas no tenga código convertible a número no se podrá ordenar.
                                throw new SincronizaExcepcion("La Cuenca Osinergmin no tiene código de Cuenca");
                            }

                            string codigoCuenca = cuencaOsi.codigoCuenca;
                            int numeroCuenca = 0;
                            if (!int.TryParse(codigoCuenca, out numeroCuenca))
                            {
                                this.RegistrarAsignacion(equipoCOES
                                                        , MotivoPendiente.FallaConversionAlCalcularCorrelativoOsi
                                                        , "Código Osinergmin = " + cuencaOsi.codigoCuenca);
                                throw new SincronizaExcepcion("No se ha podido identificar el correlativo para el equipo");
                            }

                            return numeroCuenca;
                        }).First();
                    }
                    catch (SincronizaExcepcion)
                    {
                        //- Nada que hacer porque ya se realizó el registro del problema.
                        return 0;
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    string codigoActualCuenca = actualCuencaOsi.codigoCuenca;
                    int maxCodigoCuenca;
                    if (!int.TryParse(codigoActualCuenca, out maxCodigoCuenca))
                    {
                        //- No se pudo realizar la conversión.
                        this.RegistrarAsignacion(equipoCOES
                                               , MotivoPendiente.FallaAlCalcularCorrelativoOsi
                                               , string.Empty);
                        return 0;
                    }

                    this.correlativoCodigoOsinergmin = maxCodigoCuenca;
                }
            }

            this.correlativoCodigoOsinergmin = this.correlativoCodigoOsinergmin.Value + 1;

            //- 2. Creando la nueva Cuenca en el Osinergmin.
            //- ===========================================================

            string nuevoCodigoOsinergmin = this.correlativoCodigoOsinergmin.ToString().PadLeft(3, '0');

            riInsertarCuenca inInsertarCuenca = new riInsertarCuenca();
            inInsertarCuenca.codigoCuenca = nuevoCodigoOsinergmin;
            inInsertarCuenca.nombreCuenca = equipoCOES.Equinomb;
            inInsertarCuenca.codigoCoes = equipoCOES.Equicodi.ToString();
            inInsertarCuenca.usuario = ConstantesIntercambio.UsuarioTareaAutomatica;
            inInsertarCuenca.terminal = ConstantesIntercambio.TerminalTareaAutomatica;

            roInsertarCuenca roInsertCuenca = this.servicioOsiSincronizacion.insertarCuenca(inInsertarCuenca);

            if (roInsertCuenca.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                //- Se registra la asignación como pendiente debido a que la actualización en Osinergmin falló.
                this.RegistrarAsignacion(equipoCOES
                                       , MotivoPendiente.FallaAlActualizarEntidadOsi
                                       , "Mensaje WS Osinergmin = Código: " + roInsertCuenca.codigoMensaje
                                                                            + " | Mensaje Resultante: " + roInsertCuenca.mensajeResultante);
                return 0;
            }

            //- 3. Actualizando el código Osinergmin de la Cuenca en el COES.
            //- ============================================================================
            equipoCOES.Osinergcodi = nuevoCodigoOsinergmin;
            equipoCOES.Lastuser = ConstantesIntercambio.UsuarioTareaAutomatica;
            equipoCOES.Lastdate = DateTime.Now;
            FactorySic.GetEqEquipoRepository().UpdateOsinergmin(equipoCOES);

            return 1;
        }

        protected override int ActualizarRegistroEnOsinergmin(EntityBase registroEntidad)
        {
            EqEquipoDTO equipoCOES = (EqEquipoDTO)registroEntidad;

            var cuencasOsiFiltrados = from cuencasOsi in this.roObtenerCuencaOsi.listaCuencas
                                      where cuencasOsi.codigoCuenca.Trim() == equipoCOES.Osinergcodi
                                      select cuencasOsi;

            if (cuencasOsiFiltrados.Count() == 0)
            {
                this.RegistrarAsignacion(equipoCOES
                                       , MotivoPendiente.CodigoOsiNoExisteEnOsi
                                       , "El código que no existe es = " + equipoCOES.Osinergcodi);
                return 0;
            }

            cuencaDTO cuencaOSI = cuencasOsiFiltrados.ElementAt(0);

            riUpdateCuenca inUpdateCuenca = new riUpdateCuenca();
            inUpdateCuenca.codigoCuenca = cuencaOSI.codigoCuenca;
            inUpdateCuenca.codigoCoes = equipoCOES.Equicodi.ToString();
            inUpdateCuenca.terminal = ConstantesIntercambio.TerminalTareaAutomatica;
            inUpdateCuenca.usuario = ConstantesIntercambio.UsuarioTareaAutomatica;
#warning HDT.31.10.2016 - Inicio:  Falta actualizar el estado de la Cuenca en Osinergmin.

#warning HDT Fin

            roUpdateCuenca ouUpdateCuenca = this.servicioOsiSincronizacion.updateCuenca(inUpdateCuenca);

            if (ouUpdateCuenca.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                //- Se registra la asignación como pendiente debido a que la actualización en Osinergmin falló.
                this.RegistrarAsignacion(equipoCOES
                                       , MotivoPendiente.FallaAlActualizarEntidadOsi
                                       , "Mensaje WS Osinergmin = Código: " + ouUpdateCuenca.codigoMensaje
                                                                            + " | Mensaje Resultante: " + ouUpdateCuenca.mensajeResultante);
                return 0;
            }

            return 1;
        }

        protected override int AsegurarCoherenciaEstado(EntityBase registroEntidad)
        {
            EqEquipoDTO equipoCOES = (EqEquipoDTO)registroEntidad;

            var cuencasOsiFiltrados = from cuencasOsi in this.roObtenerCuencaOsi.listaCuencas
                                      where cuencasOsi.codigoCuenca != null
                                      && cuencasOsi.codigoCuenca.Trim() == equipoCOES.Osinergcodi
                                      select cuencasOsi;

            if (cuencasOsiFiltrados.Count() == 0)
            {
                //- No se ha encontado la cuenca.
                return 0;
            }

            cuencaDTO cuencaOSI = cuencasOsiFiltrados.ElementAt(0);

#warning HDT.31.10.2016 - Inicio:  Falta el estado Osinergmin para realizar la aseguración de la coherencia de estados.
            //if (grupoCOES.Grupoactivo != grupoGeneracionOSI.estado)
            //{ 
            //this.ActualizarEnOsinergmin(grupoCOES)
            //}
#warning HDT Fin

            return 0;
        }

        public override Dictionary<string, string> ObtenerValoresHomologacion()
        {
            Dictionary<string, string> valoresHomologacion = new Dictionary<string, string>();

            this.roObtenerCuencaOsi = this.servicioOsiSincronizacion.obtenerMaestroCuenca();

            if (roObtenerCuencaOsi.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                throw new ApplicationException("El servicio web no ha respondido a la petición. Detalle del error. Detalle del mensaje "
                                             + roObtenerCuencaOsi.mensajeResultante);
            }

            foreach (cuencaDTO cuenca in roObtenerCuencaOsi.listaCuencas)
            {
                valoresHomologacion.Add(cuenca.codigoCuenca, cuenca.nombreCuenca);
            }


            return valoresHomologacion;
        }

        public override void ActualizarVinculoEnOsinergmin(string codigoOsinergmin, string codigoCOES)
        {
            riUpdateCuenca inUpdateCuenca = new riUpdateCuenca();
            inUpdateCuenca.codigoCuenca = codigoOsinergmin;
            inUpdateCuenca.codigoCoes = codigoCOES;
            inUpdateCuenca.terminal = ConstantesIntercambio.TerminalTareaAutomatica;
            inUpdateCuenca.usuario = ConstantesIntercambio.UsuarioTareaAutomatica;

            roUpdateCuenca ouUpdateCuenca = this.servicioOsiSincronizacion.updateCuenca(inUpdateCuenca);

            if (ouUpdateCuenca.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                //- Se registra la asignación como pendiente debido a que la actualización en Osinergmin falló.
                throw new ApplicationException("No se pudo realizar la actualización en el Osinergmin. Mensaje WS Osinergmin = Código: " + ouUpdateCuenca.codigoMensaje
                                                                            + " | Mensaje Resultante: " + ouUpdateCuenca.mensajeResultante);
            }
        }

        #endregion

    }

}

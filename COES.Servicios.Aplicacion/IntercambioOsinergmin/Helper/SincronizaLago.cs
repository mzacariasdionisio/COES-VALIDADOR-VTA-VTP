// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: alpha
//
// Fecha creacion: 01/11/2016
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

    public class SincronizaLago : SincronizaCOESEquipo
    {

        #region CAMPOS: Variables de la clase.

        /// <summary>
        /// Miembro de la clase que sostiene los lagos de la sincronización.
        /// </summary>
        private roObtenerMaestroLago roObtenerLagoOsi = null;

        #endregion

        #region CONSTRUCTORES: Definicion de constructores de la clase.

        public SincronizaLago()
            : base()
        {
            
        }

        public SincronizaLago(SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(servicioOsiSincroniza)
        {

        }

        public SincronizaLago(DateTime fechaSincroniza, SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(fechaSincroniza, servicioOsiSincroniza)
        {
            this.roObtenerLagoOsi = this.servicioOsiSincronizacion.obtenerMaestroLago();
            if (this.roObtenerLagoOsi.listaLago == null)
            {
                this.roObtenerLagoOsi.listaLago = new lagoDTO[0];
            }
        }

        #endregion

        #region METODOS: Metodos de la clase.

        public override EntidadSincroniza ObtenerEntidadSincroniza()
        {
            return EntidadSincroniza.Lago;
        }

        public override int ObtenerIdFamilia()
        {
            return int.Parse(FamiliaCodi.CuarentaYSeite.ToString("D"));
        }

        public override int IniciarSincronizacion()
        {
            if (this.roObtenerLagoOsi.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                this.listaError.Add(new ErrorSincroniza(this.roObtenerLagoOsi.codigoMensaje
                                                      , this.roObtenerLagoOsi.mensajeResultante));
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
                var lagosOsiFiltrados = from lagosOsi in this.roObtenerLagoOsi.listaLago
                                        select lagosOsi;

                if (lagosOsiFiltrados.Count() == 0)
                {
                    //- No se ha encontrador registro alguno.
                    this.correlativoCodigoOsinergmin = 0;
                }
                else
                {
                    lagoDTO actualLagoOsi = null;

                    try
                    {
                        actualLagoOsi = lagosOsiFiltrados.OrderByDescending(lagoOsi
                        =>
                        {
                            if (string.IsNullOrEmpty(lagoOsi.codigoLago))
                            {
                                this.RegistrarAsignacion(equipoCOES
                                                       , MotivoPendiente.LagoOsiSinCodigoLago
                                                       , string.Empty);
                                //- Basta que uno de los lagos no tenga código convertible a número no se podrá ordenar.
                                throw new SincronizaExcepcion("El Lago Osinergmin no tiene código de Embalse");
                            }

                            string codigoLago = lagoOsi.codigoLago.Replace(ConstantesIntercambio.LagoOsiPrefijoCodigo, string.Empty);
                            int numeroLago = 0;
                            if (!int.TryParse(codigoLago, out numeroLago))
                            {
                                this.RegistrarAsignacion(equipoCOES
                                                       , MotivoPendiente.FallaConversionAlCalcularCorrelativoOsi
                                                       , "Código Osinergmin = " + lagoOsi.codigoLago);
                                throw new SincronizaExcepcion("No se ha podido identificar el correlativo para el equipo");
                            }

                            return numeroLago;
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

                    string codigoActualLago = actualLagoOsi.codigoLago.Replace(ConstantesIntercambio.LagoOsiPrefijoCodigo, string.Empty);
                    int maxCodigoLago;
                    if (!int.TryParse(codigoActualLago, out maxCodigoLago))
                    {
                        //- No se pudo realizar la conversión.
                        this.RegistrarAsignacion(equipoCOES
                                               , MotivoPendiente.FallaAlCalcularCorrelativoOsi
                                               , string.Empty);
                        return 0;
                    }

                    this.correlativoCodigoOsinergmin = maxCodigoLago;
                }
            }

            this.correlativoCodigoOsinergmin = this.correlativoCodigoOsinergmin.Value + 1;

            //- 2. Creando el nuevo Lago en el Osinergmin.
            //- ===========================================================

            string nuevoCodigoOsinergmin = string.Format("{0}{1}"
                                                       , ConstantesIntercambio.LagoOsiPrefijoCodigo
                                                       , this.correlativoCodigoOsinergmin.ToString().PadLeft(2, '0'));

            riInsertarLago inInsertarLago = new riInsertarLago();
            inInsertarLago.codigoLago = nuevoCodigoOsinergmin;
            inInsertarLago.nombreLago = equipoCOES.Equinomb;
            inInsertarLago.codigoCoes = equipoCOES.Equicodi.ToString();
            inInsertarLago.usuario = ConstantesIntercambio.UsuarioTareaAutomatica;
            inInsertarLago.terminal = ConstantesIntercambio.TerminalTareaAutomatica;

            roInsertarLago roInsertLago = this.servicioOsiSincronizacion.insertarLago(inInsertarLago);

            if (roInsertLago.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                //- Se registra la asignación como pendiente debido a que la actualización en Osinergmin falló.
                this.RegistrarAsignacion(equipoCOES
                                       , MotivoPendiente.FallaAlActualizarEntidadOsi
                                       , "Mensaje WS Osinergmin = Código: " + roInsertLago.codigoMensaje
                                                                            + " | Mensaje Resultante: " + roInsertLago.mensajeResultante);
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

            var lagosOsiFiltrados = from lagosOsi in this.roObtenerLagoOsi.listaLago
                                    where lagosOsi.codigoLago.Trim() == equipoCOES.Osinergcodi
                                    select lagosOsi;

            if (lagosOsiFiltrados.Count() == 0)
            {
                this.RegistrarAsignacion(equipoCOES
                                       , MotivoPendiente.CodigoOsiNoExisteEnOsi
                                       , "El código que no existe es = " + equipoCOES.Osinergcodi);
                return 0;
            }

            lagoDTO lagoOSI = lagosOsiFiltrados.ElementAt(0);

            riUpdateLago inUpdateLago = new riUpdateLago();
            inUpdateLago.codigoLago = lagoOSI.codigoLago;
            inUpdateLago.codigoCoes = equipoCOES.Equicodi.ToString();
            inUpdateLago.terminal = ConstantesIntercambio.TerminalTareaAutomatica;
            inUpdateLago.usuario = ConstantesIntercambio.UsuarioTareaAutomatica;
#warning HDT.01.11.2016 - Inicio:  Falta actualizar el estado del Lago en Osinergmin.

#warning HDT Fin

            roUpdateLago ouUpdateLago = this.servicioOsiSincronizacion.updateLago(inUpdateLago);

            if (ouUpdateLago.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                //- Se registra la asignación como pendiente debido a que la actualización en Osinergmin falló.
                this.RegistrarAsignacion(equipoCOES
                                       , MotivoPendiente.FallaAlActualizarEntidadOsi
                                       , "Mensaje WS Osinergmin = Código: " + ouUpdateLago.codigoMensaje
                                                                            + " | Mensaje Resultante: " + ouUpdateLago.mensajeResultante);
                return 0;
            }

            return 1;
        }

        protected override int AsegurarCoherenciaEstado(EntityBase registroEntidad)
        {
            EqEquipoDTO equipoCOES = (EqEquipoDTO)registroEntidad;

            var lagosOsiFiltrados = from lagosOsi in this.roObtenerLagoOsi.listaLago
                                    where lagosOsi.codigoLago != null
                                    && lagosOsi.codigoLago.Trim() == equipoCOES.Osinergcodi
                                    select lagosOsi;

            if (lagosOsiFiltrados.Count() == 0)
            {
                //- No se ha encontado el lago.
                return 0;
            }

            lagoDTO lagoOSI = lagosOsiFiltrados.ElementAt(0);

#warning HDT.01.11.2016 - Inicio:  Falta el estado Osinergmin para realizar la aseguración de la coherencia de estados.
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

            this.roObtenerLagoOsi = this.servicioOsiSincronizacion.obtenerMaestroLago();

            if (roObtenerLagoOsi.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                throw new ApplicationException("El servicio web no ha respondido a la petición. Detalle del error. Detalle del mensaje "
                                             + roObtenerLagoOsi.mensajeResultante);
            }

            foreach (lagoDTO lago in roObtenerLagoOsi.listaLago)
            {
                valoresHomologacion.Add(lago.codigoLago, lago.nombreLago);
            }
            
            return valoresHomologacion;
        }

        public override void ActualizarVinculoEnOsinergmin(string codigoOsinergmin, string codigoCOES)
        {
            riUpdateLago inUpdateLago = new riUpdateLago();
            inUpdateLago.codigoLago = codigoOsinergmin;
            inUpdateLago.codigoCoes = codigoCOES;
            inUpdateLago.terminal = ConstantesIntercambio.TerminalTareaAutomatica;
            inUpdateLago.usuario = ConstantesIntercambio.UsuarioTareaAutomatica;

            roUpdateLago ouUpdateLago = this.servicioOsiSincronizacion.updateLago(inUpdateLago);

            if (ouUpdateLago.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                //- Se registra la asignación como pendiente debido a que la actualización en Osinergmin falló.
                throw new ApplicationException("No se pudo realizar la actualización en el Osinergmin. Mensaje WS Osinergmin = Código: " + ouUpdateLago.codigoMensaje
                                                                            + " | Mensaje Resultante: " + ouUpdateLago.mensajeResultante);
            }
        }

        #endregion

    }
}

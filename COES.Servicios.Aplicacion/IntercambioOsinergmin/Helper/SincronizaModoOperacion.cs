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
    public class SincronizaModoOperacion : SincronizaCOESGrupo
    {

        #region CAMPOS: Variables de la clase.

        /// <summary>
        /// Miembro de la clase que sostiene los grupos de generación de la sincronización.
        /// </summary>
        private roObtenerMaestroModoOperacion roObtenerModoOperacionOsi = null;

        #endregion

        #region CONSTRUCTORES: Definicion de constructores de la clase.

        public SincronizaModoOperacion()
            : base()
        {
            
        }

        public SincronizaModoOperacion(SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(servicioOsiSincroniza)
        {

        }

        public SincronizaModoOperacion(DateTime fechaSincroniza, SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(fechaSincroniza, servicioOsiSincroniza)
        {
            this.roObtenerModoOperacionOsi = this.servicioOsiSincronizacion.obtenerMaestroModoOperacion();
            if (this.roObtenerModoOperacionOsi.listaModosOperacion == null)
            {
                this.roObtenerModoOperacionOsi.listaModosOperacion = new modoOperacionDTO[] { };
            }
        }

        #endregion

        #region METODOS: Metodos de la clase.

        public override EntidadSincroniza ObtenerEntidadSincroniza()
        {
            return EntidadSincroniza.ModoOperacion;
        }

        public override List<int> ObtenerListaCategorias()
        {
            List<int> listaCategorias = new List<int>();
            listaCategorias.Add(2);
            listaCategorias.Add(9);

            return listaCategorias;
        }

        public override int IniciarSincronizacion()
        {
            if (this.roObtenerModoOperacionOsi.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                this.listaError.Add(new ErrorSincroniza(this.roObtenerModoOperacionOsi.codigoMensaje
                                                      , this.roObtenerModoOperacionOsi.mensajeResultante));
                return 0;
            }

            return base.IniciarSincronizacion();
        }

        protected override int CrearRegistroEnOsinergmin(EntityBase registroEntidad)
        {
            PrGrupoDTO grupoCOES = (PrGrupoDTO)registroEntidad;

            //- 1. Calculando el correlativo a utilizar para el nuevo registro.
            //- ===============================================================

            if (!this.correlativoCodigoOsinergmin.HasValue)
            {
                //- El correlativo aún no fue identificado, entonces se procede con el cálculo:
                var modosOperacionOsi = from gruposOsi in this.roObtenerModoOperacionOsi.listaModosOperacion
                                        select gruposOsi;

                if (modosOperacionOsi.Count() == 0)
                {
                    //- No se ha encontrador registro alguno.
                    this.correlativoCodigoOsinergmin = 0;
                }
                else
                {
                    modoOperacionDTO actualModoOperacionOsi = null;
                    try
                    {
                        actualModoOperacionOsi = modosOperacionOsi.OrderByDescending(modoOsi
                        =>
                        {
                            if (string.IsNullOrEmpty(modoOsi.codigoModoOperacion))
                            {
                                this.RegistrarAsignacion(grupoCOES
                                                       , MotivoPendiente.ModoOperacionOsiSinCodigoModoOperacion
                                                       , string.Empty);
                                //- Basta que uno de los modos de operación no tenga código convertible a número no se podrá ordenar.
                                throw new SincronizaExcepcion("El Modo de Operación Osinergmin no tiene código de Modo de Operación");
                            }

                            string codigoModoOperacion = modoOsi.codigoModoOperacion.Replace(ConstantesIntercambio.ModoOperacionOsiPrefijoCodigo, string.Empty);
                            int numeroModoOperacion = 0;
                            if (!int.TryParse(codigoModoOperacion, out numeroModoOperacion))
                            {
                                this.RegistrarAsignacion(grupoCOES
                                                       , MotivoPendiente.FallaConversionAlCalcularCorrelativoOsi
                                                       , "Código Osinergmin = " + modoOsi.codigoModoOperacion);
                                throw new SincronizaExcepcion("No se ha podido identificar el correlativo para el grupo");
                            }

                            return numeroModoOperacion;

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

                    string codigoActualModoOperacion
                        = actualModoOperacionOsi.codigoModoOperacion.Replace(ConstantesIntercambio.ModoOperacionOsiPrefijoCodigo, string.Empty);
                    int maxCodigoModoOperacion;
                    if (!int.TryParse(codigoActualModoOperacion, out maxCodigoModoOperacion))
                    {
                        //- No se pudo realizar la conversión.
                        this.RegistrarAsignacion(grupoCOES
                                               , MotivoPendiente.FallaAlCalcularCorrelativoOsi
                                               , string.Empty);
                        return 0;
                    }

                    this.correlativoCodigoOsinergmin = maxCodigoModoOperacion;
                }
            }

            this.correlativoCodigoOsinergmin = this.correlativoCodigoOsinergmin.Value + 1;

            //- 2. Creando el nuevo Modo de Operación en el Osinergmin.
            //- ===========================================================

            string nuevoCodigoOsinergmin = string.Format("{0}{1}"
                                                       , ConstantesIntercambio.ModoOperacionOsiPrefijoCodigo
                                                       , this.correlativoCodigoOsinergmin.ToString().PadLeft(4, '0'));


            riInsertarModoOperacion inInsertarModoOperacion = new riInsertarModoOperacion();
            inInsertarModoOperacion.codigoModoOperacion = nuevoCodigoOsinergmin;
            inInsertarModoOperacion.descripcionModoOperacion = grupoCOES.Gruponomb;
            inInsertarModoOperacion.codigoCoes = grupoCOES.Grupocodi.ToString();
            inInsertarModoOperacion.usuario = ConstantesIntercambio.UsuarioTareaAutomatica;
            inInsertarModoOperacion.terminal = ConstantesIntercambio.TerminalTareaAutomatica;

            roInsertarModoOperacion roInsertModoOperacion = this.servicioOsiSincronizacion.insertarModoOperacion(inInsertarModoOperacion);

            if (roInsertModoOperacion.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                //- Se registra la asignación como pendiente debido a que la actualización en Osinergmin falló.
                this.RegistrarAsignacion(grupoCOES
                                       , MotivoPendiente.FallaAlActualizarEntidadOsi
                                       , "Mensaje WS Osinergmin = Código: " + roInsertModoOperacion.codigoMensaje
                                                                            + " | Mensaje Resultante: " + roInsertModoOperacion.mensajeResultante);
                return 0;
            }

            //- 3. Actualizando el código Osinergmin del Modo de Operación en el COES.
            //- ============================================================================
            grupoCOES.Osinergcodi = nuevoCodigoOsinergmin;
            grupoCOES.Lastuser = ConstantesIntercambio.UsuarioTareaAutomatica;
            grupoCOES.Lastdate = DateTime.Now;
            FactorySic.GetPrGrupoRepository().UpdateOsinergmin(grupoCOES);

            return 1;
        }

        protected override int ActualizarRegistroEnOsinergmin(EntityBase registroEntidad)
        {
            PrGrupoDTO grupoCOES = (PrGrupoDTO)registroEntidad;

            var modosOperacionOsi = from modosOsi in this.roObtenerModoOperacionOsi.listaModosOperacion
                                    where modosOsi.codigoModoOperacion.Trim() == grupoCOES.Osinergcodi
                                    select modosOsi;

            if (modosOperacionOsi.Count() == 0)
            {
                this.RegistrarAsignacion(grupoCOES
                                       , MotivoPendiente.CodigoOsiNoExisteEnOsi
                                       , "El código que no existe es = " + grupoCOES.Osinergcodi);
                return 0;
            }

            modoOperacionDTO modoOperacionOSI = modosOperacionOsi.ElementAt(0);

            riUpdateModoOperacion inUpdateModoOperacion = new riUpdateModoOperacion();
            inUpdateModoOperacion.codigoModoOperacion = modoOperacionOSI.codigoModoOperacion;
            inUpdateModoOperacion.codigoCoes = grupoCOES.Grupocodi.ToString();
            inUpdateModoOperacion.terminal = ConstantesIntercambio.TerminalTareaAutomatica;
            inUpdateModoOperacion.usuario = ConstantesIntercambio.UsuarioTareaAutomatica;
#warning HDT.31.10.2016 - Inicio:  Falta actualizar el estado del modo de operación en Osinergmin.

#warning HDT Fin

            roUpdateModoOperacion ouUpdateModoOperacion
                = this.servicioOsiSincronizacion.updateModoOperacion(inUpdateModoOperacion);

            if (ouUpdateModoOperacion.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                //- Se registra la asignación como pendiente debido a que la actualización en Osinergmin falló.
                this.RegistrarAsignacion(grupoCOES
                                       , MotivoPendiente.FallaAlActualizarEntidadOsi
                                       , "Mensaje WS Osinergmin = Código: " + ouUpdateModoOperacion.codigoMensaje
                                                                            + " | Mensaje Resultante: " + ouUpdateModoOperacion.mensajeResultante);
                return 0;
            }

            return 1;
        }

        protected override int AsegurarCoherenciaEstado(EntityBase registroEntidad)
        {
            PrGrupoDTO grupoCOES = (PrGrupoDTO)registroEntidad;

            var modosOperacionOsi = from modosOsi in this.roObtenerModoOperacionOsi.listaModosOperacion
                                    where modosOsi.codigoModoOperacion != null
                                    && modosOsi.codigoModoOperacion.Trim() == grupoCOES.Osinergcodi
                                    select modosOsi;

            if (modosOperacionOsi.Count() == 0)
            {
                //- No se ha encontado el grupo.
                return 0;
            }

            modoOperacionDTO modoOperacionOSI = modosOperacionOsi.ElementAt(0);

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

            this.roObtenerModoOperacionOsi = this.servicioOsiSincronizacion.obtenerMaestroModoOperacion();

            if (roObtenerModoOperacionOsi.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                throw new ApplicationException("El servicio web no ha respondido a la petición. Detalle del error. Detalle del mensaje "
                                             + roObtenerModoOperacionOsi.mensajeResultante);
            }

            foreach (modoOperacionDTO modo in roObtenerModoOperacionOsi.listaModosOperacion)
            {
                valoresHomologacion.Add(modo.codigoModoOperacion, modo.descripcionModoOperacion);
            }


            return valoresHomologacion;
        }

        public override void ActualizarVinculoEnOsinergmin(string codigoOsinergmin, string codigoCOES)
        {
            riUpdateModoOperacion inUpdateModoOperacion = new riUpdateModoOperacion();
            inUpdateModoOperacion.codigoModoOperacion = codigoOsinergmin;
            inUpdateModoOperacion.codigoCoes = codigoCOES;
            inUpdateModoOperacion.terminal = ConstantesIntercambio.TerminalTareaAutomatica;
            inUpdateModoOperacion.usuario = ConstantesIntercambio.UsuarioTareaAutomatica;

            roUpdateModoOperacion ouUpdateModoOperacion
                = this.servicioOsiSincronizacion.updateModoOperacion(inUpdateModoOperacion);

            if (ouUpdateModoOperacion.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                //- Se registra la asignación como pendiente debido a que la actualización en Osinergmin falló.
                throw new ApplicationException("No se pudo realizar la actualización en el Osinergmin. Mensaje WS Osinergmin = Código: " + ouUpdateModoOperacion.codigoMensaje
                                                                            + " | Mensaje Resultante: " + ouUpdateModoOperacion.mensajeResultante);
            }
        }

        #endregion

    }
}

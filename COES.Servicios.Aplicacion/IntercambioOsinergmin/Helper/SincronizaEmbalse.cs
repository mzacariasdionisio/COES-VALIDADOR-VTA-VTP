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
    public class SincronizaEmbalse : SincronizaCOESEquipo
    {

        #region CAMPOS: Variables de la clase.

        /// <summary>
        /// Miembro de la clase que sostiene los embalses de la sincronización.
        /// </summary>
        private roObtenerMaestroEmbalse roObtenerEmbalseOsi = null;

        #endregion

        #region CONSTRUCTORES: Definicion de constructores de la clase.

        public SincronizaEmbalse()
            : base()
        {

        }

        public SincronizaEmbalse(SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(servicioOsiSincroniza)
        {

        }

        public SincronizaEmbalse(DateTime fechaSincroniza, SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(fechaSincroniza, servicioOsiSincroniza)
        {
            this.roObtenerEmbalseOsi = this.servicioOsiSincronizacion.obtenerMaestroEmbalse();
            if (this.roObtenerEmbalseOsi.listaEmbalses == null)
            {
                this.roObtenerEmbalseOsi.listaEmbalses = new embalseDTO[] { };
            }
        }

        #endregion

        #region METODOS: Metodos de la clase.

        public override EntidadSincroniza ObtenerEntidadSincroniza()
        {
            return EntidadSincroniza.Embalse;
        }

        public override int ObtenerIdFamilia()
        {
            return int.Parse(FamiliaCodi.Embalse.ToString("D"));
        }

        public override int IniciarSincronizacion()
        {
            if (this.roObtenerEmbalseOsi.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                this.listaError.Add(new ErrorSincroniza(this.roObtenerEmbalseOsi.codigoMensaje
                                                      , this.roObtenerEmbalseOsi.mensajeResultante));
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
                var embalsesOsiFiltrados = from embalsesOsi in this.roObtenerEmbalseOsi.listaEmbalses
                                           select embalsesOsi;

                if (embalsesOsiFiltrados.Count() == 0)
                {
                    //- No se ha encontrador registro alguno.
                    this.correlativoCodigoOsinergmin = 0;
                }
                else
                {
                    embalseDTO actualEmbalseOsi = null;

                    try
                    {
                        actualEmbalseOsi = embalsesOsiFiltrados.OrderByDescending(embalseOsi
                        =>
                        {
                            if (string.IsNullOrEmpty(embalseOsi.codigoEmbalse))
                            {
                                this.RegistrarAsignacion(equipoCOES
                                                       , MotivoPendiente.EmbalseOsiSinCodigoEmbalse
                                                       , string.Empty);
                                //- Basta que uno de los embalse no tenga código convertible a número no se podrá ordenar.
                                throw new SincronizaExcepcion("El Embalse Osinergmin no tiene código de Embalse");
                            }

                            string codigoEmbalse = embalseOsi.codigoEmbalse.Replace(ConstantesIntercambio.EmbalseOsiPrefijoCodigo, string.Empty);
                            int numeroEmbalse = 0;
                            if (!int.TryParse(codigoEmbalse, out numeroEmbalse))
                            {
                                this.RegistrarAsignacion(equipoCOES
                                                       , MotivoPendiente.FallaConversionAlCalcularCorrelativoOsi
                                                       , "Código Osinergmin = " + embalseOsi.codigoEmbalse);
                                throw new SincronizaExcepcion("No se ha podido identificar el correlativo para el equipo");
                            }

                            return numeroEmbalse;
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

                    string codigoActualEmbalse = actualEmbalseOsi.codigoEmbalse.Replace(ConstantesIntercambio.EmbalseOsiPrefijoCodigo, string.Empty);
                    int maxCodigoEmbalse;
                    if (!int.TryParse(codigoActualEmbalse, out maxCodigoEmbalse))
                    {
                        //- No se pudo realizar la conversión.
                        this.RegistrarAsignacion(equipoCOES
                                               , MotivoPendiente.FallaAlCalcularCorrelativoOsi
                                               , string.Empty);
                        return 0;
                    }

                    this.correlativoCodigoOsinergmin = maxCodigoEmbalse;
                }
            }

            this.correlativoCodigoOsinergmin = this.correlativoCodigoOsinergmin.Value + 1;

            //- 2. Creando el nuevo Embalse en el Osinergmin.
            //- ===========================================================

            string nuevoCodigoOsinergmin = string.Format("{0}{1}"
                                                       , ConstantesIntercambio.EmbalseOsiPrefijoCodigo
                                                       , this.correlativoCodigoOsinergmin.ToString().PadLeft(2, '0'));

            riInsertarEmbalse inInsertarEmbalse = new riInsertarEmbalse();
            inInsertarEmbalse.codigoEmbalse = nuevoCodigoOsinergmin;
            inInsertarEmbalse.nombreEmbalse = equipoCOES.Equinomb;
            inInsertarEmbalse.codigoCoes = equipoCOES.Equicodi.ToString();
            inInsertarEmbalse.usuario = ConstantesIntercambio.UsuarioTareaAutomatica;
            inInsertarEmbalse.terminal = ConstantesIntercambio.TerminalTareaAutomatica;

            roInsertarEmbalse roInsertEmbalse = this.servicioOsiSincronizacion.insertarEmbalse(inInsertarEmbalse);

            if (roInsertEmbalse.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                //- Se registra la asignación como pendiente debido a que la actualización en Osinergmin falló.
                this.RegistrarAsignacion(equipoCOES
                                       , MotivoPendiente.FallaAlActualizarEntidadOsi
                                       , "Mensaje WS Osinergmin = Código: " + roInsertEmbalse.codigoMensaje
                                                                            + " | Mensaje Resultante: " + roInsertEmbalse.mensajeResultante);
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

            var embalsesOsiFiltrados = from embalsesOsi in this.roObtenerEmbalseOsi.listaEmbalses
                                       where embalsesOsi.codigoEmbalse.Trim() == equipoCOES.Osinergcodi
                                       select embalsesOsi;

            if (embalsesOsiFiltrados.Count() == 0)
            {
                this.RegistrarAsignacion(equipoCOES
                                       , MotivoPendiente.CodigoOsiNoExisteEnOsi
                                       , "El código que no existe es = " + equipoCOES.Osinergcodi);
                return 0;
            }

            embalseDTO embalseOSI = embalsesOsiFiltrados.ElementAt(0);

            riUpdateEmbalse inUpdateEmbalse = new riUpdateEmbalse();
            inUpdateEmbalse.codigoEmbalse = embalseOSI.codigoEmbalse;
            inUpdateEmbalse.codigoCoes = equipoCOES.Equicodi.ToString();
            inUpdateEmbalse.terminal = ConstantesIntercambio.TerminalTareaAutomatica;
            inUpdateEmbalse.usuario = ConstantesIntercambio.UsuarioTareaAutomatica;
#warning HDT.01.11.2016 - Inicio:  Falta actualizar el estado del Embalse en Osinergmin.

#warning HDT Fin

            roUpdateEmbalse ouUpdateEmbalse = this.servicioOsiSincronizacion.updateEmbalse(inUpdateEmbalse);

            if (ouUpdateEmbalse.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                //- Se registra la asignación como pendiente debido a que la actualización en Osinergmin falló.
                this.RegistrarAsignacion(equipoCOES
                                       , MotivoPendiente.FallaAlActualizarEntidadOsi
                                       , "Mensaje WS Osinergmin = Código: " + ouUpdateEmbalse.codigoMensaje
                                                                            + " | Mensaje Resultante: " + ouUpdateEmbalse.mensajeResultante);
                return 0;
            }

            return 1;
        }

        protected override int AsegurarCoherenciaEstado(EntityBase registroEntidad)
        {
            EqEquipoDTO equipoCOES = (EqEquipoDTO)registroEntidad;

            var embalsesOsiFiltrados = from embalsesOsi in this.roObtenerEmbalseOsi.listaEmbalses
                                       where embalsesOsi.codigoEmbalse != null
                                       && embalsesOsi.codigoEmbalse.Trim() == equipoCOES.Osinergcodi
                                       select embalsesOsi;

            if (embalsesOsiFiltrados.Count() == 0)
            {
                //- No se ha encontado el embalse.
                return 0;
            }

            embalseDTO embalseOSI = embalsesOsiFiltrados.ElementAt(0);

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

            this.roObtenerEmbalseOsi = this.servicioOsiSincronizacion.obtenerMaestroEmbalse();

            if (roObtenerEmbalseOsi.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                throw new ApplicationException("El servicio web no ha respondido a la petición. Detalle del error. Detalle del mensaje "
                                             + roObtenerEmbalseOsi.mensajeResultante);
            }

            foreach (embalseDTO embalse in roObtenerEmbalseOsi.listaEmbalses)
            {
                valoresHomologacion.Add(embalse.codigoEmbalse, embalse.nombreEmbalse);
            }

            return valoresHomologacion;
        }

        public override void ActualizarVinculoEnOsinergmin(string codigoOsinergmin, string codigoCOES)
        {
            riUpdateEmbalse inUpdateEmbalse = new riUpdateEmbalse();
            inUpdateEmbalse.codigoEmbalse = codigoOsinergmin;
            inUpdateEmbalse.codigoCoes = codigoCOES;
            inUpdateEmbalse.terminal = ConstantesIntercambio.TerminalTareaAutomatica;
            inUpdateEmbalse.usuario = ConstantesIntercambio.UsuarioTareaAutomatica;

            roUpdateEmbalse ouUpdateEmbalse = this.servicioOsiSincronizacion.updateEmbalse(inUpdateEmbalse);

            if (ouUpdateEmbalse.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                //- Se registra la asignación como pendiente debido a que la actualización en Osinergmin falló.
                throw new ApplicationException("No se pudo realizar la actualización en el Osinergmin. Mensaje WS Osinergmin = Código: " + ouUpdateEmbalse.codigoMensaje
                                                                            + " | Mensaje Resultante: " + ouUpdateEmbalse.mensajeResultante);
            }
        }

        #endregion

    }
}

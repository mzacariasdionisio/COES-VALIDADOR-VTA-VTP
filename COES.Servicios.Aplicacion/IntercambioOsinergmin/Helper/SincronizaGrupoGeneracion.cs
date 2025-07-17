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
    public class SincronizaGrupoGeneracion : SincronizaCOESGrupo
    {

        #region CAMPOS: Variables de la clase.

        /// <summary>
        /// Miembro de la clase que sostiene los grupos de generación de la sincronización.
        /// </summary>
        private roObtenerMaestroGrupoGeneracion roObtenerGrupoGeneracionOsi = null;

        #endregion

        #region CONSTRUCTORES: Definicion de constructores de la clase.


        public SincronizaGrupoGeneracion()
            : base()
        {
        }

        public SincronizaGrupoGeneracion(SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(servicioOsiSincroniza)
        {

        }


        public SincronizaGrupoGeneracion(DateTime fechaSincroniza, SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(fechaSincroniza, servicioOsiSincroniza)
        {
            this.roObtenerGrupoGeneracionOsi = this.servicioOsiSincronizacion.obtenerMaestroGrupoGeneracion();
            if (this.roObtenerGrupoGeneracionOsi.listaGruposGeneracion == null)
            {
                this.roObtenerGrupoGeneracionOsi.listaGruposGeneracion = new grupoGeneracionDTO[] { };
            }
        }

        #endregion

        #region METODOS: Metodos de la clase.

        public override EntidadSincroniza ObtenerEntidadSincroniza()
        {
            return EntidadSincroniza.GrupoGeneracion;
        }

        public override List<int> ObtenerListaCategorias()
        {
            List<int> listaCategorias = new List<int>();
            listaCategorias.Add(3);
            listaCategorias.Add(5);

            return listaCategorias;
        }

        public override int IniciarSincronizacion()
        {
            if (this.roObtenerGrupoGeneracionOsi.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                this.listaError.Add(new ErrorSincroniza(this.roObtenerGrupoGeneracionOsi.codigoMensaje
                                                      , this.roObtenerGrupoGeneracionOsi.mensajeResultante));
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
                var gruposGeneracionOsi = from gruposOsi in this.roObtenerGrupoGeneracionOsi.listaGruposGeneracion
                                          where gruposOsi.codigoGrupoGeneracion.Trim() != ConstantesIntercambio.GrupoGeneracionCodigoExcepcion
                                          select gruposOsi;

                if (gruposGeneracionOsi.Count() == 0)
                {
                    //- No se ha encontrador registro alguno.
                    this.correlativoCodigoOsinergmin = 0;
                }
                else
                {
                    grupoGeneracionDTO actualGrupoGeneracionOsi = null;

                    try
                    {
                        actualGrupoGeneracionOsi = gruposGeneracionOsi.OrderByDescending(grupoOsi
                        =>
                        {
                            if (string.IsNullOrEmpty(grupoOsi.codigoGrupoGeneracion))
                            {
                                this.RegistrarAsignacion(grupoCOES
                                                       , MotivoPendiente.GrupoOsiSinCodigoGrupoGeneracion
                                                       , string.Empty);
                                //- Basta que uno de los grupos de generación no tenga código convertible a número no se podrá ordenar.
                                throw new SincronizaExcepcion("El Grupo de Generación Osinergmin no tiene código de Grupo de Generación");
                            }

                            string codigoGrupoGeneracion = grupoOsi.codigoGrupoGeneracion.Replace(ConstantesIntercambio.GrupoGeneracionOsiPrefijoCodigo, string.Empty);
                            int numeroGrupoGeneracion = 0;
                            if (!int.TryParse(codigoGrupoGeneracion, out numeroGrupoGeneracion))
                            {
                                this.RegistrarAsignacion(grupoCOES
                                                       , MotivoPendiente.FallaConversionAlCalcularCorrelativoOsi
                                                       , "Código Osinergmin = " + grupoOsi.codigoGrupoGeneracion);
                                throw new SincronizaExcepcion("No se ha podido identificar el correlativo para el grupo");
                            }

                            return numeroGrupoGeneracion;
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

                    string codigoActualGrupoGeneracion
                        = actualGrupoGeneracionOsi.codigoGrupoGeneracion.Replace(ConstantesIntercambio.GrupoGeneracionOsiPrefijoCodigo, string.Empty);
                    int maxCodigoGrupoGeneracion;
                    if (!int.TryParse(codigoActualGrupoGeneracion, out maxCodigoGrupoGeneracion))
                    {
                        //- No se pudo realizar la conversión.
                        this.RegistrarAsignacion(grupoCOES
                                               , MotivoPendiente.FallaAlCalcularCorrelativoOsi
                                               , string.Empty);
                        return 0;
                    }

                    this.correlativoCodigoOsinergmin = maxCodigoGrupoGeneracion;
                }
            }

            this.correlativoCodigoOsinergmin = this.correlativoCodigoOsinergmin.Value + 1;

            //- 2. Creando el nuevo Grupo de Generación en el Osinergmin.
            //- ===========================================================

            string nuevoCodigoOsinergmin = string.Format("{0}{1}"
                                                       , ConstantesIntercambio.GrupoGeneracionOsiPrefijoCodigo
                                                       , this.correlativoCodigoOsinergmin.ToString().PadLeft(4, '0'));

            string codigoTipoCombustible = grupoCOES.Fueosinergcodi.Trim();

            if (codigoTipoCombustible == string.Empty)
            {
                this.RegistrarAsignacion(grupoCOES
                                       , MotivoPendiente.GrupoCombustibleCOESNoExisteEnOsi
                                       , "Valor encontrado: " + codigoTipoCombustible);
                return 0;
            }

            if (!grupoCOES.Grupopadre.HasValue)
            { 
                //- Si el grupo de generación no tiene una central de generación no se puede continuar.
                this.RegistrarAsignacion(grupoCOES
                                       , MotivoPendiente.GrupoGeneracionCOESSinCentralGeneracion
                                       , string.Empty);
                return 0;
            }

            PrGrupoDTO grupoCOESCentralGeneracion = FactorySic.GetPrGrupoRepository().GetByIdOsinergmin(grupoCOES.Grupopadre.Value);

            if (grupoCOESCentralGeneracion == null)
            {
                //- Si la central de generación especificada en el grupo de generación no existe
                //- entonces se registra como asignación pendiente.
                this.RegistrarAsignacion(grupoCOES
                                       , MotivoPendiente.CentralGeneracionCOESNoExiste
                                       , "GrupoPadre = " + grupoCOES.Grupopadre.Value);
                return 0;
            }

            if (grupoCOESCentralGeneracion.Catecodi <= 0)
            {
                //- Si la central de generación no tiene definido un código de categoría se registra como pendiente.
                this.RegistrarAsignacion(grupoCOES
                                       , MotivoPendiente.GrupoPadreSinCodigoCategoria
                                       , string.Empty);
                return 0;
            }

            SincronizaCentralGeneracion sincCentralGeneracion = new SincronizaCentralGeneracion();
            List<int> categoriasCG  = sincCentralGeneracion.ObtenerListaCategorias();
            if (!categoriasCG.Contains(grupoCOESCentralGeneracion.Catecodi))
            {
                //- Si el grupo de generación no es de la categoría esperada de acuerdo con la configuración
                //- entonces se registra la asignación como pendiente.
                this.RegistrarAsignacion(grupoCOES
                                       , MotivoPendiente.GrupoCOESNoEsCentralGeneracion
                                       , "CateCodi = " + grupoCOESCentralGeneracion.Catecodi);
                return 0;
            }

            if (string.IsNullOrEmpty(grupoCOESCentralGeneracion.Osinergcodi))
            {
                //- Si el grupo de generación no tiene un código Osinergmin no se puede continuar.
                this.RegistrarAsignacion(grupoCOES
                                       , MotivoPendiente.EntidadCOESSinCodigoOsi
                                       , "Grupo Padre = " + grupoCOESCentralGeneracion.Grupocodi);
                return 0;
            }

            riInsertarGrupoGeneracion inIinsertarGrupoGeneracion = new riInsertarGrupoGeneracion();
            inIinsertarGrupoGeneracion.codigoGrupoGeneracion = nuevoCodigoOsinergmin;
            inIinsertarGrupoGeneracion.codigoCentralGeneracion = grupoCOESCentralGeneracion.Osinergcodi;
            inIinsertarGrupoGeneracion.codigoCoes = grupoCOES.Grupocodi.ToString();
            inIinsertarGrupoGeneracion.nombreGrupoGeneracion = grupoCOES.Gruponomb;
            inIinsertarGrupoGeneracion.codigoTipoCombustible = codigoTipoCombustible;
            inIinsertarGrupoGeneracion.usuario = ConstantesIntercambio.UsuarioTareaAutomatica;
            inIinsertarGrupoGeneracion.terminal = ConstantesIntercambio.TerminalTareaAutomatica;

            roInsertarGrupoGeneracion roInsertGrupoGeneracion = this.servicioOsiSincronizacion.insertarGrupoGeneracion(inIinsertarGrupoGeneracion);

            if (roInsertGrupoGeneracion.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                //- Se registra la asignación como pendiente debido a que la actualización en Osinergmin falló.
                this.RegistrarAsignacion(grupoCOES
                                       , MotivoPendiente.FallaAlActualizarEntidadOsi
                                       , "Mensaje WS Osinergmin = Código: " + roInsertGrupoGeneracion.codigoMensaje
                                                                            + " | Mensaje Resultante: " + roInsertGrupoGeneracion.mensajeResultante);
                return 0;
            }

            //- 3. Actualizando el código Osinergmin del Grupo de Generación en el COES.
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

            var gruposGeneracionOsi = from gruposOsi in this.roObtenerGrupoGeneracionOsi.listaGruposGeneracion
                                      where gruposOsi.codigoGrupoGeneracion.Trim() == grupoCOES.Osinergcodi
                                      select gruposOsi;

            if (gruposGeneracionOsi.Count() == 0)
            {
                this.RegistrarAsignacion(grupoCOES
                                       , MotivoPendiente.CodigoOsiNoExisteEnOsi
                                       , "El código que no existe es = " + grupoCOES.Osinergcodi);
                return 0;
            }

            grupoGeneracionDTO grupoGeneracionOSI = gruposGeneracionOsi.ElementAt(0);

            riUpdateGrupoGeneracion inUpdateGrupoGeneracion = new riUpdateGrupoGeneracion();
            inUpdateGrupoGeneracion.codigoGrupoGeneracion = grupoGeneracionOSI.codigoGrupoGeneracion;
            inUpdateGrupoGeneracion.codigoCoes = grupoCOES.Grupocodi.ToString();
            inUpdateGrupoGeneracion.terminal = ConstantesIntercambio.TerminalTareaAutomatica;
            inUpdateGrupoGeneracion.usuario = ConstantesIntercambio.UsuarioTareaAutomatica;
#warning HDT.31.10.2016 - Inicio:  Falta actualizar el estado del grupo en Osinergmin.

#warning HDT Fin

            roUpdateGrupoGeneracion ouUpdateGrupoGeneracion
                = this.servicioOsiSincronizacion.updateGrupoGeneracion(inUpdateGrupoGeneracion);

            if (ouUpdateGrupoGeneracion.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                //- Se registra la asignación como pendiente debido a que la actualización en Osinergmin falló.
                this.RegistrarAsignacion(grupoCOES
                                       , MotivoPendiente.FallaAlActualizarEntidadOsi
                                       , "Mensaje WS Osinergmin = Código: " + ouUpdateGrupoGeneracion.codigoMensaje
                                                                            + " | Mensaje Resultante: " + ouUpdateGrupoGeneracion.mensajeResultante);
                return 0;
            }

            return 1;
        }

        protected override int AsegurarCoherenciaEstado(EntityBase registroEntidad)
        {
            PrGrupoDTO grupoCOES = (PrGrupoDTO)registroEntidad;

            var gruposGeneracionOsi = from gruposOsi in this.roObtenerGrupoGeneracionOsi.listaGruposGeneracion
                                      where gruposOsi.codigoGrupoGeneracion != null
                                      && gruposOsi.codigoGrupoGeneracion.Trim() == grupoCOES.Osinergcodi
                                      select gruposOsi;

            if (gruposGeneracionOsi.Count() == 0)
            {
                //- No se ha encontrado el grupo.
                return 0;
            }

            grupoGeneracionDTO grupoGeneracionOSI = gruposGeneracionOsi.ElementAt(0);

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

            this.roObtenerGrupoGeneracionOsi = this.servicioOsiSincronizacion.obtenerMaestroGrupoGeneracion();

            if (roObtenerGrupoGeneracionOsi.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                throw new ApplicationException("El servicio web no ha respondido a la petición. Detalle del error. Detalle del mensaje "
                                             + roObtenerGrupoGeneracionOsi.mensajeResultante);
            }

            foreach (grupoGeneracionDTO grupo in roObtenerGrupoGeneracionOsi.listaGruposGeneracion)
            {
                valoresHomologacion.Add(grupo.codigoGrupoGeneracion, grupo.nombreGrupoGeneracion);
            }

            return valoresHomologacion;
        }

        public override void ActualizarVinculoEnOsinergmin(string codigoOsinergmin, string codigoCOES)
        {
            riUpdateGrupoGeneracion inUpdateGrupoGeneracion = new riUpdateGrupoGeneracion();
            inUpdateGrupoGeneracion.codigoGrupoGeneracion = codigoOsinergmin;
            inUpdateGrupoGeneracion.codigoCoes = codigoCOES;
            inUpdateGrupoGeneracion.terminal = ConstantesIntercambio.TerminalTareaAutomatica;
            inUpdateGrupoGeneracion.usuario = ConstantesIntercambio.UsuarioTareaAutomatica;

            roUpdateGrupoGeneracion ouUpdateGrupoGeneracion
                = this.servicioOsiSincronizacion.updateGrupoGeneracion(inUpdateGrupoGeneracion);

            if (ouUpdateGrupoGeneracion.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                //- Se registra la asignación como pendiente debido a que la actualización en Osinergmin falló.
                throw new ApplicationException("No se pudo realizar la actualización en el Osinergmin. Mensaje WS Osinergmin = Código: " + ouUpdateGrupoGeneracion.codigoMensaje
                                                                            + " | Mensaje Resultante: " + ouUpdateGrupoGeneracion.mensajeResultante);
            }

        }

        #endregion

    }
}

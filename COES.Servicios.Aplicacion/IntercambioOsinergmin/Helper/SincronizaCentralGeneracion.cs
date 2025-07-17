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

    public class SincronizaCentralGeneracion : SincronizaCOESGrupo
    {

        #region CAMPOS: Variables de la clase.

        /// <summary>
        /// Enumerado que sostiene los valores del Tipo de Grupo.
        /// </summary>
        private enum GrupoTipoCOES
        {
            Ninguno = 0,
            /// <summary>
            /// Térmico
            /// </summary>
            T = 4,
            /// <summary>
            /// Hidráulico
            /// </summary>
            H = 6
        }

        /// <summary>
        /// Miembro de la clase que sostiene las centrales de generación de la sincronización.
        /// </summary>
        private roObtenerMaestroCentralGeneracion roObtenerCentralGeneracionOsi = null;

        /// <summary>
        /// Mapeo entre los tipos COES y los tipos Osinergmin.
        /// <remarks>La clave es el código COES y el valor es el código Osinergmin.</remarks>
        /// </summary>
        private Dictionary<int, string> mapeoGrupoTipo = null;

        #endregion

        #region CONSTRUCTORES: Definicion de constructores de la clase.

        public SincronizaCentralGeneracion(SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(servicioOsiSincroniza)
        {

        }

        public SincronizaCentralGeneracion(DateTime fechaSincroniza, SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(fechaSincroniza, servicioOsiSincroniza)
        {
            this.roObtenerCentralGeneracionOsi = this.servicioOsiSincronizacion.obtenerMaestroCentralGeneracion();

            if (this.roObtenerCentralGeneracionOsi.listaCentrales == null)
            {
                this.roObtenerCentralGeneracionOsi.listaCentrales = new centralGeneracionDTO[] { };
            }

            this.mapeoGrupoTipo = new Dictionary<int, string>();
            this.mapeoGrupoTipo.Add(int.Parse(GrupoTipoCOES.H.ToString("D")), "H");
            this.mapeoGrupoTipo.Add(int.Parse(GrupoTipoCOES.T.ToString("D")), "T");
        }

        public SincronizaCentralGeneracion()
            : base(DateTime.Now, null)
        {

        }

        #endregion

        #region METODOS: Metodos de la clase.

        public override EntidadSincroniza ObtenerEntidadSincroniza()
        {
            return EntidadSincroniza.CentralGeneracion;
        }

        public override List<int> ObtenerListaCategorias()
        {
            List<int> listaCategorias = new List<int>();
            listaCategorias.Add(4);
            listaCategorias.Add(6);

            return listaCategorias;
        }

        public override int IniciarSincronizacion()
        {
            if (this.roObtenerCentralGeneracionOsi.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                this.listaError.Add(new ErrorSincroniza(this.roObtenerCentralGeneracionOsi.codigoMensaje
                                                      , this.roObtenerCentralGeneracionOsi.mensajeResultante));
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
                var centralesGeneracionOsi = from centralesOsi in this.roObtenerCentralGeneracionOsi.listaCentrales
                                             where centralesOsi.codigoCentralGeneracion.Trim() != ConstantesIntercambio.CentralGeneracionCodigoExcepcion
                                             select centralesOsi;

                if (centralesGeneracionOsi.Count() == 0)
                {
                    //- No se ha encontrador registro alguno.
                    this.correlativoCodigoOsinergmin = 0;
                }
                else
                {
                    centralGeneracionDTO actualCentralGeneracionOsi = null;

                    try
                    {
                        actualCentralGeneracionOsi = centralesGeneracionOsi.OrderByDescending(centralOsi
                        =>
                        {
                            if (string.IsNullOrEmpty(centralOsi.codigoCentralGeneracion))
                            {
                                this.RegistrarAsignacion(grupoCOES
                                                       , MotivoPendiente.CentralGeneracionOsiSinCodigoCentralGeneracion
                                                       , string.Empty);
                                //- Basta que uno de las centrales de generación no tenga código convertible a número no se podrá ordenar.
                                throw new SincronizaExcepcion("La Central de Generación Osinergmin no tiene código de Central de Generación");
                            }

                            string codigoCentralGeneracion = centralOsi.codigoCentralGeneracion.Replace(ConstantesIntercambio.CentralGeneracionOsiPrefijoCodigo, string.Empty);
                            int numeroCentralGeneracion = 0;
                            if (!int.TryParse(codigoCentralGeneracion, out numeroCentralGeneracion))
                            {
                                this.RegistrarAsignacion(grupoCOES
                                                       , MotivoPendiente.FallaConversionAlCalcularCorrelativoOsi
                                                       , "Código Osinergmin = " + centralOsi.codigoCentralGeneracion);
                                throw new SincronizaExcepcion("No se ha podido identificar el correlativo para el grupo");
                            }

                            return numeroCentralGeneracion;
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

                    string codigoActualCentralGeneracion
                        = actualCentralGeneracionOsi.codigoCentralGeneracion.Replace(ConstantesIntercambio.CentralGeneracionOsiPrefijoCodigo, string.Empty);
                    int maxCodigoCentralGeneracion;
                    if (!int.TryParse(codigoActualCentralGeneracion, out maxCodigoCentralGeneracion))
                    {
                        //- No se pudo realizar la conversión.
                        this.RegistrarAsignacion(grupoCOES
                                               , MotivoPendiente.FallaAlCalcularCorrelativoOsi
                                               , string.Empty);
                        return 0;
                    }

                    this.correlativoCodigoOsinergmin = maxCodigoCentralGeneracion;
                }
            }

            this.correlativoCodigoOsinergmin = this.correlativoCodigoOsinergmin.Value + 1;

            //- 2. Creando la nueva Central de Generación en el Osinergmin.
            //- ===========================================================

            string nuevoCodigoOsinergmin = string.Format("{0}{1}"
                                                       , ConstantesIntercambio.CentralGeneracionOsiPrefijoCodigo
                                                       , this.correlativoCodigoOsinergmin.ToString().PadLeft(4, '0'));

            SiEmpresaDTO empresaCOES = this.ObtenerEmpresa(grupoCOES);
            if (empresaCOES == null)
            {
                this.RegistrarAsignacion(grupoCOES
                                       , MotivoPendiente.GrupoCOESConEmpresaCOESNoExistente
                                       , "Código de Empresa COES " + grupoCOES.Emprcodi);
                return 0;
            }

            if (string.IsNullOrEmpty(empresaCOES.EmprCodOsinergmin))
            {
                this.RegistrarAsignacion(grupoCOES
                                       , MotivoPendiente.EmpresaCOESSinCodigoOsi
                                       , "Código de Empresa COES " + grupoCOES.Emprcodi);
                return 0;
            }

            string codTipoCentral = string.Empty;
            if (string.IsNullOrEmpty(grupoCOES.TipoGenerRer) || grupoCOES.TipoGenerRer == "N")
            {
                //- Si es nulo o vacío entonces NO es Energía Renovable (R), por ello,
                //- a continuación se evalúa si la energía es Térmica o Hidráulica.
                if (!this.mapeoGrupoTipo.ContainsKey(grupoCOES.Catecodi))
                {
                    this.RegistrarAsignacion(grupoCOES
                                           , MotivoPendiente.TipoEnergiaCOESNoExisteEnOsi
                                           , "Valor encontrado: " + grupoCOES.Catecodi.ToString());
                    return 0;
                } 
                else
                {
                    codTipoCentral = this.mapeoGrupoTipo[grupoCOES.Catecodi];
                }
            }
            else if(grupoCOES.TipoGenerRer == "S")
            {
                codTipoCentral = "R"; //- Energía Renovable.
            }
            else
            {
                 this.RegistrarAsignacion(grupoCOES
                                        , MotivoPendiente.TipoEnergiaCOESNoExisteEnOsi
                                        , "Valor encontrado: " + grupoCOES.TipoGenerRer);
                 return 0;
            }

            riInsertarCentralGeneracion inIinsertarCentralGeneracion = new riInsertarCentralGeneracion();
            inIinsertarCentralGeneracion.codigoCentralGeneracion = nuevoCodigoOsinergmin;
            inIinsertarCentralGeneracion.codigoCoes = grupoCOES.Grupocodi.ToString();
            inIinsertarCentralGeneracion.nombreCentralGeneracion = grupoCOES.Gruponomb.Trim();
            inIinsertarCentralGeneracion.codigoEmpresa = empresaCOES.EmprCodOsinergmin;
            inIinsertarCentralGeneracion.codigoTipoCentral = codTipoCentral;
            inIinsertarCentralGeneracion.usuario = ConstantesIntercambio.UsuarioTareaAutomatica;
            inIinsertarCentralGeneracion.terminal = ConstantesIntercambio.TerminalTareaAutomatica;

            roInsertarCentralGeneracion roInsertCentralGeneracion = this.servicioOsiSincronizacion.insertarCentralGeneracion(inIinsertarCentralGeneracion);

            if (roInsertCentralGeneracion.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                //- Se registra la asignación como pendiente debido a que la actualización en Osinergmin falló.
                this.RegistrarAsignacion(grupoCOES
                                       , MotivoPendiente.FallaAlActualizarEntidadOsi
                                       , "Mensaje WS Osinergmin = Código: " + roInsertCentralGeneracion.codigoMensaje
                                                                            + " | Mensaje Resultante: " + roInsertCentralGeneracion.mensajeResultante);
                return 0;
            }

            //- 3. Actualizando el código Osinergmin de la Central de Generación en el COES.
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

            var centralesGeneracionOsi = from centralesOsi in this.roObtenerCentralGeneracionOsi.listaCentrales
                                         where centralesOsi.codigoCentralGeneracion.Trim() == grupoCOES.Osinergcodi
                                         select centralesOsi;

            if (centralesGeneracionOsi.Count() == 0)
            {
                this.RegistrarAsignacion(grupoCOES
                                       , MotivoPendiente.CodigoOsiNoExisteEnOsi
                                       , "El código que no existe es = " + grupoCOES.Osinergcodi);
                return 0;
            }

            centralGeneracionDTO centralGeneracionOSI = centralesGeneracionOsi.ElementAt(0);

            riUpdateCentralGeneracion inUpdateCentralGeneracion = new riUpdateCentralGeneracion();
            inUpdateCentralGeneracion.codigoCentralGeneracion = centralGeneracionOSI.codigoCentralGeneracion;
            inUpdateCentralGeneracion.codigoCoes = grupoCOES.Grupocodi.ToString();
            inUpdateCentralGeneracion.terminal = ConstantesIntercambio.TerminalTareaAutomatica;
            inUpdateCentralGeneracion.usuario = ConstantesIntercambio.UsuarioTareaAutomatica;
#warning HDT.31.10.2016 - Inicio:  Falta actualizar el estado de la central en Osinergmin.

#warning HDT Fin

            roUpdateCentralGeneracion ouUpdateCentralGeneracion
                = this.servicioOsiSincronizacion.updateCentralGeneracion(inUpdateCentralGeneracion);

            if (ouUpdateCentralGeneracion.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                //- Se registra la asignación como pendiente debido a que la actualización en Osinergmin falló.
                this.RegistrarAsignacion(grupoCOES
                                       , MotivoPendiente.FallaAlActualizarEntidadOsi
                                       , "Mensaje WS Osinergmin = Código: " + ouUpdateCentralGeneracion.codigoMensaje
                                                                            + " | Mensaje Resultante: " + ouUpdateCentralGeneracion.mensajeResultante);
                return 0;
            }

            return 1;
        }

        protected override int AsegurarCoherenciaEstado(EntityBase registroEntidad)
        {
            PrGrupoDTO grupoCOES = (PrGrupoDTO)registroEntidad;

            var centralesGeneracionOsi = from centralesOsi in this.roObtenerCentralGeneracionOsi.listaCentrales
                                         where centralesOsi.codigoCentralGeneracion != null
                                         && centralesOsi.codigoCentralGeneracion.Trim() == grupoCOES.Osinergcodi
                                         select centralesOsi;

            if (centralesGeneracionOsi.Count() == 0)
            {
                //- No se ha encontado la central.
                return 0;
            }

            centralGeneracionDTO centralGeneracionOSI = centralesGeneracionOsi.ElementAt(0);

#warning HDT.31.10.2016 - Inicio:  Falta el estado Osinergmin para realizar la aseguración de la coherencia de estados.
            //if (grupoCOES.Grupoactivo != centralGeneracionOSI.estado)
            //{ 
            //this.ActualizarEnOsinergmin(grupoCOES)
            //}
#warning HDT Fin

            return 0;
        }

        public override Dictionary<string, string> ObtenerValoresHomologacion()
        {
            Dictionary<string, string> valoresHomologacion = new Dictionary<string, string>();

            this.roObtenerCentralGeneracionOsi = this.servicioOsiSincronizacion.obtenerMaestroCentralGeneracion();

            if (roObtenerCentralGeneracionOsi.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                throw new ApplicationException("El servicio web no ha respondido a la petición. Detalle del error. Detalle del mensaje "
                                             + roObtenerCentralGeneracionOsi.mensajeResultante);
            }

            foreach (centralGeneracionDTO central in roObtenerCentralGeneracionOsi.listaCentrales)
            {
                valoresHomologacion.Add(central.codigoCentralGeneracion, central.nombreCentralGeneracion);
            }


            return valoresHomologacion;
        }

        public override void ActualizarVinculoEnOsinergmin(string codigoOsinergmin, string codigoCOES)
        {
            riUpdateCentralGeneracion inUpdateCentralGeneracion = new riUpdateCentralGeneracion();
            inUpdateCentralGeneracion.codigoCentralGeneracion = codigoOsinergmin;
            inUpdateCentralGeneracion.codigoCoes = codigoCOES;
            inUpdateCentralGeneracion.terminal = ConstantesIntercambio.TerminalTareaAutomatica;
            inUpdateCentralGeneracion.usuario = ConstantesIntercambio.UsuarioTareaAutomatica;

            roUpdateCentralGeneracion ouUpdateCentralGeneracion
                = this.servicioOsiSincronizacion.updateCentralGeneracion(inUpdateCentralGeneracion);

            if (ouUpdateCentralGeneracion.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                throw new ApplicationException("No se pudo realizar la actualización en el Osinergmin. Mensaje WS Osinergmin = Código: " + ouUpdateCentralGeneracion.codigoMensaje
                                                                            + " | Mensaje Resultante: " + ouUpdateCentralGeneracion.mensajeResultante);
            }
        }

        #endregion

    }
}

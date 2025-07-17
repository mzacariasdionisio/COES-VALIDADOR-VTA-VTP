// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: alpha
//
// Fecha creacion: 25/10/2016
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

    /// <summary>
    /// Enumerado que registra las entidades suceptibles para sincronizar con maestros.
    /// </summary>
    public enum EntidadSincroniza
    {
        Ninguno = 0,
        Empresa = 1,
        UsuarioLibre = 2,
        Suministro = 3,
        Barra = 4,
        CentralGeneracion = 5,
        GrupoGeneracion = 6,
        ModoOperacion = 7,
        Cuenca = 8,
        Embalse = 9,
        Lago = 10
    }

    /// <summary>
    /// Columnas para la sincronización
    /// </summary>
    public enum ColumnaSincroniza
    {
        Ninguno = 0,
        EMPRCODOSINERGMIN = 1,
        EMPRRUC = 2,
        OSINERGCODI = 3
    }

    /// <summary>
    /// Enumerado que registra los motivos o estados posibles para las asignaciones de las entidades
    /// en el proceso de sincronización de maestros con el Osinergmin.
    /// </summary>
    public enum MotivoPendiente
    {
        Ninguno = 0,

        //- Motivos para la entidad Empresa.
        EmpresaCOESSinRUC = 1,
        EmpresasOsiConMismoRUC = 2,
        NoExisteEmpresaOsiConRUC = 3,

        //- Motivos para los Grupos COES
        EmpresaCOESSinCodigoOsi = 20,
        TipoEnergiaCOESNoExisteEnOsi = 21,
        GrupoOriginalCOESNoExiste = 22,
        GrupoCombustibleCOESNoExisteEnOsi = 23,
        GrupoOsiSinCodigoGrupoGeneracion = 24,
        GrupoGeneracionCOESSinCentralGeneracion = 25,
        GrupoCOESNoEsCentralGeneracion = 26,
        CentralGeneracionCOESNoExiste = 27,
        GrupoPadreSinCodigoCategoria = 28,
        ModoOperacionOsiSinCodigoModoOperacion = 29,
        CentralGeneracionOsiSinCodigoCentralGeneracion = 30,
        GrupoCOESConEmpresaCOESNoExistente = 31,

        //- Motivos para los Equipos COES
        CuencaOsiSinCodigoCuenca = 50,
        EquipoOriginalCOESNoExiste = 51,
        EmbalseOsiSinCodigoEmbalse = 52,
        LagoOsiSinCodigoLago= 53,

        //- Motivos generales.
        EntidadOsiConCodigoCOESDistinto = 100,
        FallaAlActualizarEntidadOsi = 101,
        EntidadCOESSinCodigoOsi = 102,
        EntidadCOESInactivaSinCodigoCOESOriginal = 103,
        FallaAlCalcularCorrelativoOsi = 104,
        CodigoOsiNoExisteEnOsi = 105,
        FallaConversionAlCalcularCorrelativoOsi = 106
    }

    /// <summary>
    /// Clase base para la implementación de la sincronización de maestros.
    /// </summary>
    public abstract class Sincroniza
    {

        #region CAMPOS: Variables de la clase.

        /// <summary>
        /// Correlativo de la generación de los códigos Osinergmin en el COES.
        /// </summary>
        protected int? correlativoCodigoOsinergmin = null;

        /// <summary>
        /// Lista de errores ocurridos en la sincronización de la entidad.
        /// </summary>
        protected List<ErrorSincroniza> listaError = new List<ErrorSincroniza>();

        /// <summary>
        /// Entidad especifica de la sincronización.
        /// </summary>
        protected EntidadSincroniza entidadSincroniza = EntidadSincroniza.Ninguno;

        /// <summary>
        /// Fecha de la sincronización.
        /// Esta fecha se mantiene para todos los registros de la tarea de sincronización.
        /// </summary>
        protected DateTime fechaSincronizacion;

        /// <summary>
        /// Servicio web de Osinergmin para realizar la sincronización de maestros.
        /// </summary>
        protected SincronizacionMaestrosEndpointService servicioOsiSincronizacion = null;

        /// <summary>
        /// Lista de motivos pendientes de la sincronización con su correspondiente descripción.
        /// </summary>
        private Dictionary<MotivoPendiente, string> listaMotivoPendiente = null;

        #endregion

        #region CONSTRUCTORES: Definicion de constructores de la clase.

        /// <summary>
        /// Constructor base para la sincronización.
        /// </summary>
        public Sincroniza()
        {

        }

        public Sincroniza(SincronizacionMaestrosEndpointService servicioOsiSincroniza)
        {
            this.servicioOsiSincronizacion = servicioOsiSincroniza;
        }

        /// <summary>
        /// Constructor de la clase que recibe la fecha de la sincronización.
        /// </summary>
        /// <param name="fechaSincroniza">Con esta fecha se crearán los registros en la tabla de pendientes</param>
        /// <param name="servicioOsiSincroniza">Esta es la referencia al servicio web de Osinergmin para la sincronización de maestros</param>
        public Sincroniza(DateTime fechaSincroniza, SincronizacionMaestrosEndpointService servicioOsiSincroniza)
        {
            this.fechaSincronizacion = fechaSincroniza;
            this.servicioOsiSincronizacion = servicioOsiSincroniza;
            this.ConfigurarSincronizacion();
        }

        #endregion

        #region METODOS: Metodos de la clase.

        /// <summary>
        /// Configura la clase para la sincronización.
        /// </summary>
        private void ConfigurarSincronizacion()
        {
            //- Mensajes para la entidad Empresa
            //- ================================
            listaMotivoPendiente = new Dictionary<MotivoPendiente, string>();
            listaMotivoPendiente.Add(MotivoPendiente.EmpresaCOESSinRUC
                                   , "La Empresa COES no tiene RUC");

            listaMotivoPendiente.Add(MotivoPendiente.EmpresasOsiConMismoRUC
                                   , "Existe más de una Empresa Osinergmin con el mismo RUC");

            listaMotivoPendiente.Add(MotivoPendiente.NoExisteEmpresaOsiConRUC
                                   , "No existe una empresa Osinergmin con el RUC registrado en el COES");

            //- Motivos para los Grupos COES
            listaMotivoPendiente.Add(MotivoPendiente.EmpresaCOESSinCodigoOsi
                                   , "La propiedad 'Empresa COES' del Grupo no tiene Código Osinergmin");

            listaMotivoPendiente.Add(MotivoPendiente.TipoEnergiaCOESNoExisteEnOsi
                                   , "El Tipo de Energía del Grupo COES no es reconocido por el Osinergmin");

            listaMotivoPendiente.Add(MotivoPendiente.GrupoOriginalCOESNoExiste
                                   , "El Grupo COES original no existe en el COES");

            listaMotivoPendiente.Add(MotivoPendiente.CodigoOsiNoExisteEnOsi
                                   , "El Código Osinergmin registrado en el COES no existe en el Osinergmin");

            listaMotivoPendiente.Add(MotivoPendiente.GrupoCombustibleCOESNoExisteEnOsi
                                   , "El Grupo COES tiene un valor en la propiedad GrupoCombustible que no es reconocido por el Osinergmin");

            listaMotivoPendiente.Add(MotivoPendiente.GrupoOsiSinCodigoGrupoGeneracion
                                   , "La propiedad Código del Grupo de Generación de Osinergmin no tiene valor alguno. Esto ha ocurrido en la generación del correlativo para un nuevo registro.");

            listaMotivoPendiente.Add(MotivoPendiente.GrupoGeneracionCOESSinCentralGeneracion
                                   , "El Grupo COES no tiene una Central de Generación");

            listaMotivoPendiente.Add(MotivoPendiente.GrupoCOESNoEsCentralGeneracion
                                   , "El Grupo COES no es una Central de Generación");

            listaMotivoPendiente.Add(MotivoPendiente.CentralGeneracionCOESNoExiste
                                   , "La Central de Generación del Grupo de Generación no existe en el COES");

            listaMotivoPendiente.Add(MotivoPendiente.GrupoPadreSinCodigoCategoria
                                   , "El Grupo Padre COES no tiene Categoría");            

            listaMotivoPendiente.Add(MotivoPendiente.ModoOperacionOsiSinCodigoModoOperacion
                                   , "La propiedad Código Modo de Operación de Osinergmin no tiene valor alguno");

            listaMotivoPendiente.Add(MotivoPendiente.CentralGeneracionOsiSinCodigoCentralGeneracion
                                   , "La propiedad Código de la Central de Generación de Osinergmin no tiene valor alguno. Esto ha ocurrido en la generación del correlativo para un nuevo registro");

            listaMotivoPendiente.Add(MotivoPendiente.GrupoCOESConEmpresaCOESNoExistente
                                   , "El Grupo COES tiene un código de Empresa que no existe en la tabla de Empresas del COES");            
            
            //- Motivos para los Equipos COES
            listaMotivoPendiente.Add(MotivoPendiente.CuencaOsiSinCodigoCuenca
                                   , "La propiedad Código Cuenca de Osinergmin no tiene valor alguno");

            listaMotivoPendiente.Add(MotivoPendiente.EquipoOriginalCOESNoExiste
                                   , "El Equipo COES original no existe en el COES");

            listaMotivoPendiente.Add(MotivoPendiente.EmbalseOsiSinCodigoEmbalse
                                   , "La propiedad Código Embalse de Osinergmin no tiene valor alguno");
            
            //- Mensajes generales
            //- ==================
            listaMotivoPendiente.Add(MotivoPendiente.EntidadOsiConCodigoCOESDistinto
                                   , "Existe una empresa Osinergmin con el mismo RUC pero con un código COES distinto al esperado");
            
            listaMotivoPendiente.Add(MotivoPendiente.FallaAlActualizarEntidadOsi
                                   , "Ocurrió un error al intentar actualizar la entidad Osinergmin");
            
            listaMotivoPendiente.Add(MotivoPendiente.EntidadCOESSinCodigoOsi
                                   , "El registro no tiene el código Osinergmin correspondiente");

            listaMotivoPendiente.Add(MotivoPendiente.EntidadCOESInactivaSinCodigoCOESOriginal
                                  , "El registro COES está inactivo pero no tiene un código COES original aun cuando este es requerido para las reasignaciones");

            listaMotivoPendiente.Add(MotivoPendiente.FallaAlCalcularCorrelativoOsi
                                  , "No se ha podido calcular el correlativo de la entidad Osinergmin");

            listaMotivoPendiente.Add(MotivoPendiente.FallaConversionAlCalcularCorrelativoOsi
                                  , "Falla al obtener el último correlativo de Osinergmin porque no se pudo determinar alguno de los números a partir del código alfanumérico Osinergmin");
            
            
        }

        /// <summary>
        /// Lanza el proceso de sincronización para la entidad.
        /// </summary>
        /// <returns></returns>
        public abstract int IniciarSincronizacion();

        /// <summary>
        /// Obtiene el valor del enumerado de la entidad a sincronizar.
        /// </summary>
        /// <returns></returns>
        public abstract EntidadSincroniza ObtenerEntidadSincroniza();

      
        /// <summary>
        /// Registra la asignación.
        /// Vale decir que la asignación puede ser pendiente o lista.
        /// </summary>
        /// <param name="registroEntidad"></param>
        /// <param name="motivo"></param>
        /// <param name="mensajeAdicionalMotivo"></param>
        protected void RegistrarAsignacion(EntityBase registroEntidad, MotivoPendiente motivo, string mensajeAdicionalMotivo)
        {
            string codigo = this.ObtenerCodigoRegistroEntidad(registroEntidad);
            string etiqueta = this.ObtenerEtiquetaRegistroEntidad(registroEntidad);

            string descripcionMotivo = string.Empty;

            if (!this.listaMotivoPendiente.TryGetValue(motivo, out descripcionMotivo))
            {
                if (motivo == MotivoPendiente.Ninguno)
                {
                    descripcionMotivo = "Conforme";
                }
                else
                {
                    descripcionMotivo = motivo.ToString();
                }                
            }

            Sincroniza oSincroniza = (Sincroniza)this;
            EntidadSincroniza eSincroniza = oSincroniza.ObtenerEntidadSincroniza();

            IioAsignacionPendienteDTO asignacionPendiente = new IioAsignacionPendienteDTO();
            asignacionPendiente.Mapenentidad = eSincroniza.ToString();
            asignacionPendiente.Mapencodigo = codigo;
            asignacionPendiente.Mapendescripcion = etiqueta;
            asignacionPendiente.Mapenestado = int.Parse(motivo.ToString("D"));
            asignacionPendiente.Mapenindicacionestado = descripcionMotivo + (string.IsNullOrEmpty(mensajeAdicionalMotivo) 
                                                                            ? string.Empty
                                                                            : ". Detalle: " + mensajeAdicionalMotivo);
            asignacionPendiente.Mapenestregistro = "1";
            asignacionPendiente.Mapenusucreacion = ConstantesIntercambio.UsuarioTareaAutomatica;
            asignacionPendiente.Mapenfeccreacion = this.fechaSincronizacion;
            asignacionPendiente.Mapenusumodificacion = ConstantesIntercambio.UsuarioTareaAutomatica;
            asignacionPendiente.Mapenfecmodificacion = DateTime.Now;

            FactorySic.GetIioAsignacionPendienteRepository().Save(asignacionPendiente);
        }

        /// <summary>
        /// Obtiene la lista de errores encontrados en la sincronización.
        /// </summary>
        public List<ErrorSincroniza> ObtenerListaErrores()
        {
            return this.listaError;
        }

        /// <summary>
        /// Obtiene el código del registro de la entindad COES que se usa para registrar asignaciones.
        /// </summary>
        /// <param name="registroEntidad"></param>
        /// <returns></returns>
        protected abstract string ObtenerCodigoRegistroEntidad(EntityBase registroEntidad);

        /// <summary>
        /// Obtiene la etiqueta del registro de la entindad COES que se usa para registrar asignaciones.
        /// Vale decir que esta etiqueta permite identificar desde la perspectiva de negocio al registro de la entidad COES.
        /// </summary>
        /// <param name="registroEntidad"></param>
        /// <returns></returns>
        protected abstract string ObtenerEtiquetaRegistroEntidad(EntityBase registroEntidad);

        /// <summary>
        /// Realizar la reasignación del código Osinergmin.
        /// <remarks>La reasignación solo debe realizarse si la propiedad Código Osinergmin está vacía, caso contrario se asumen
        /// que el registro ya fue sincronizado.</remarks>
        /// </summary>
        /// <param name="registroEntidad"></param>
        /// <returns></returns>
        protected virtual int ReasignarCodigoOsinergmin(EntityBase registroEntidad)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Actualizar el estado y el codigo COES del grupo correspondiente en Osinergmin.
        /// </summary>
        /// <param name="registroEntidad"></param>
        /// <returns></returns>
        protected virtual int ActualizarRegistroEnOsinergmin(EntityBase registroEntidad)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Permite asegurar que existe coherencia entre el estado del Grupo COES y el estado del registro
        /// correspondiente en el Osinergmin.
        /// </summary>
        /// <param name="grupoOsi"></param>
        protected virtual int AsegurarCoherenciaEstado(EntityBase registroEntidad)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Permite crear un nuevo registro en Osinergmin.
        /// <remarks>Debido a que el correlativo Osinergmin se genera en el COES se debe considerar que solo se obtiene
        /// los registros Osinergmin al iniciar la tarea sobre cada entidad</remarks>
        /// </summary>
        /// <param name="registroEntidad"></param>
        /// <returns>Se retorna 1 si tuvo éxito y 0 en caso contrario</returns>
        protected virtual int CrearRegistroEnOsinergmin(EntityBase registroEntidad)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Permite obtener los valores para la homologación.
        /// </summary>
        /// <returns>Valores para realizar la homologación</returns>
        public virtual Dictionary<string, string> ObtenerValoresHomologacion()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Permite actualizar el enlace existente entre el registro COES y el Osinergmin.
        /// <remarks>El método se usa principalmente para realizar la opción Quitar asignación</remarks>
        /// </summary>
        /// <param name="codigoOsinergmin"></param>
        /// <param name="codigoCOES"></param>
        public virtual void ActualizarVinculoEnOsinergmin(string codigoOsinergmin, string codigoCOES)
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}

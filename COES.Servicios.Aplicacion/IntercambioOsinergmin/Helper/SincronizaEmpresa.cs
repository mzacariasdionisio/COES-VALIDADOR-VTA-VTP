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

    /// <summary>
    /// Enumera los tipos de empresas.
    /// </summary>
    public enum TipoEmpresa
    {
        Ninguno = 0,
        Transmision = 1,
        Distribucion = 2,
        Generacion = 3,
        UsuarioLibre = 4
    }

    /// <summary>
    /// Clase que implementa la lógica necesaria para realizar la sincronización para la entidad Empresa.
    /// </summary>
    public class SincronizaEmpresa : Sincroniza
    {

        #region CONSTRUCTORES: Definicion de constructores de la clase.

        public SincronizaEmpresa()
            : base()
        { 
        }

        public SincronizaEmpresa(SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(servicioOsiSincroniza)
        {
        }

        public SincronizaEmpresa(DateTime fechaSincroniza, SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(fechaSincroniza, servicioOsiSincroniza)
        {

        }

        #endregion

        #region METODOS: Metodos de la clase.

        public override EntidadSincroniza ObtenerEntidadSincroniza()
        {
            return EntidadSincroniza.Empresa;
        }

        protected override string ObtenerCodigoRegistroEntidad(EntityBase registroEntidad)
        {
            SiEmpresaDTO registroEntidadC = (SiEmpresaDTO)registroEntidad;
            return registroEntidadC.Emprcodi.ToString();
        }

        protected override string ObtenerEtiquetaRegistroEntidad(EntityBase registroEntidad)
        {
            SiEmpresaDTO registroEntidadC = (SiEmpresaDTO)registroEntidad;
            return registroEntidadC.Emprnomb.Trim();
        }

        public override int IniciarSincronizacion()
        {
            List<int> listaIdTipoEmpresa = new List<int>();
            listaIdTipoEmpresa.Add(int.Parse(TipoEmpresa.Transmision.ToString("D")));
            listaIdTipoEmpresa.Add(int.Parse(TipoEmpresa.Distribucion.ToString("D")));
            listaIdTipoEmpresa.Add(int.Parse(TipoEmpresa.Generacion.ToString("D")));

            List<SiEmpresaDTO> lEmpresaDTO = FactorySic.GetSiEmpresaRepository().ListarEmpresasPorTipo(listaIdTipoEmpresa);

            roObtenerMaestroEmpresa roObtenerEmpresaOsi = this.servicioOsiSincronizacion.obtenerMaestroEmpresas();

            if (roObtenerEmpresaOsi.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                this.listaError.Add(new ErrorSincroniza(roObtenerEmpresaOsi.codigoMensaje, roObtenerEmpresaOsi.mensajeResultante));
                return 0;
            }

            empresaDTO empresaOsiDTO = null;
            roUpdateEmpresa roUpdateEmpresaOsi = null;
            string codigoRegistroEntidad = string.Empty;
            string etiquetaRegistroEntidad = string.Empty;

            foreach (SiEmpresaDTO empresaDTO in lEmpresaDTO)
            {
                //- Solo se deben procesar las empresas activas.
                if (empresaDTO.Emprestado != ConstantesIntercambio.EmpresaCOESEstadoActivo)
                {
                    continue;
                }

                //- Si el registro ya tiene código Osinergmin entonces ya no se realiza acción alguna.
                //- Esto porque se asume que ya fue homologado.
                if (!string.IsNullOrEmpty(empresaDTO.EmprCodOsinergmin))
                {
                    continue;
                }

                codigoRegistroEntidad = this.ObtenerCodigoRegistroEntidad(empresaDTO);
                etiquetaRegistroEntidad = this.ObtenerEtiquetaRegistroEntidad(empresaDTO);

                //- 1. Se valida si la empresa COES tiene RUC.
                //- ==========================================
                if (String.IsNullOrEmpty(empresaDTO.Emprruc))
                {
                    //- De no tener RUC se registra como pendiente de sincronización.
                    this.RegistrarAsignacion(empresaDTO
                                           , MotivoPendiente.EmpresaCOESSinRUC
                                           , string.Empty);
                    continue;
                }

                //- 2. Se selecciona la empresa Osinergmin que tenga el mismo RUC que la empresa COES.
                //- ==================================================================================
                var empresasOsiEncontradas = from empresaOsi in roObtenerEmpresaOsi.listaEmpresas
                                             where empresaOsi.ruc == empresaDTO.Emprruc
                                             select empresaOsi;

                if (empresasOsiEncontradas.Count() == 0)
                {
                    //- De no encontrar coincidencia se registra como pendiente de sincronización.
                    this.RegistrarAsignacion(empresaDTO
                                           , MotivoPendiente.NoExisteEmpresaOsiConRUC
                                           , string.Empty);
                    continue;
                }
                else if (empresasOsiEncontradas.Count() > 1)
                {
                    //- De encontrar más de una empresa Osinergmin con el mismo RUC se registra como pendiente de sincronización.
                    this.RegistrarAsignacion(empresaDTO
                                           , MotivoPendiente.EmpresasOsiConMismoRUC
                                           , "N° de empresas = " + empresasOsiEncontradas.Count().ToString());
                    continue;
                }

                empresaOsiDTO = empresasOsiEncontradas.ElementAt(0);

                //- 3. Se valida si existen diferencias entre el código COES y el Código COES en Osinergmin.
                //- ========================================================================================
                if (!String.IsNullOrEmpty(empresaOsiDTO.codigoCoes))
                {
                    if (empresaOsiDTO.codigoCoes != empresaDTO.Emprcodi.ToString())
                    {
                        //- De encontrar diferencias se registra como pendiente.
                        this.RegistrarAsignacion(empresaDTO
                                               , MotivoPendiente.EntidadOsiConCodigoCOESDistinto
                                               , "Código COES en Osinergmin = " + empresaOsiDTO.codigoCoes);
                    }
                    //- Se continua porque solo se puede actualizar si la empresa Osi no tiene código COES.
                    continue;
                }

                //- 4. Se actualiza la entidad en COES.
                //- ===================================
                empresaDTO.EmprCodOsinergmin = empresaOsiDTO.codigoEmpresa;
                empresaDTO.Lastdate = DateTime.Now;
                empresaDTO.Lastuser = ConstantesIntercambio.UsuarioTareaAutomatica;
                FactorySic.GetSiEmpresaRepository().UpdateOsinergmin(empresaDTO);

                //- 5. Se actualiza la entidad en Osinergmin.
                //- =========================================                
                riUpdateEmpresa inUpdateEmpresa = new riUpdateEmpresa();
                inUpdateEmpresa.codigoCoes = codigoRegistroEntidad;
                inUpdateEmpresa.codigoEmpresa = empresaOsiDTO.codigoEmpresa;
                inUpdateEmpresa.terminal = ConstantesIntercambio.TerminalTareaAutomatica;
                inUpdateEmpresa.usuario = ConstantesIntercambio.UsuarioTareaAutomatica;

                roUpdateEmpresaOsi = this.servicioOsiSincronizacion.updateEmpresa(inUpdateEmpresa);
                if (roUpdateEmpresaOsi.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
                {
                    this.listaError.Add(new ErrorSincroniza(roUpdateEmpresaOsi.codigoMensaje, roUpdateEmpresaOsi.mensajeResultante));

                    //- Se registra la asignación como pendiente debido a que la actualización en Osinergmin falló.
                    this.RegistrarAsignacion(empresaDTO
                                           , MotivoPendiente.FallaAlActualizarEntidadOsi
                                           , "Mensaje WS Osinergmin = Código: " + roUpdateEmpresaOsi.codigoMensaje
                                                                                + " | Mensaje Resultante: " + roUpdateEmpresaOsi.mensajeResultante);
                    return 0;
                }

                //- 6. Se crea la asignación lista.
                //- ===============================
                this.RegistrarAsignacion(empresaDTO
                                       , MotivoPendiente.Ninguno
                                       , string.Empty);
            }

            return 1;
        }

        public override Dictionary<string, string> ObtenerValoresHomologacion()
        {
            Dictionary<string, string> valoresHomologacion = new Dictionary<string, string>();

            roObtenerMaestroEmpresa roObtenerEmpresaOsi = this.servicioOsiSincronizacion.obtenerMaestroEmpresas();

            if (roObtenerEmpresaOsi.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                throw new ApplicationException("El servicio web no ha respondido a la petición. Detalle del error. Detalle del mensaje " 
                                             + roObtenerEmpresaOsi.mensajeResultante);
            }

            foreach (empresaDTO empresa in roObtenerEmpresaOsi.listaEmpresas)
            {
                valoresHomologacion.Add(empresa.codigoEmpresa, empresa.descEmpresa);
            }


            return valoresHomologacion;
        }    

        #endregion        

    }
}

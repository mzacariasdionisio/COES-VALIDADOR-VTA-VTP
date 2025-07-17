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

    public class SincronizaUsuarioLibre : Sincroniza
    {


        #region CONSTRUCTORES: Definicion de constructores de la clase.

        public SincronizaUsuarioLibre()
            : base()
        {

        }

        public SincronizaUsuarioLibre(SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(servicioOsiSincroniza)
        {

        }

        public SincronizaUsuarioLibre(DateTime fechaSincroniza, SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(fechaSincroniza, servicioOsiSincroniza)
        {

        }

        #endregion

        #region METODOS: Metodos de la clase.

        public override EntidadSincroniza ObtenerEntidadSincroniza()
        {
            return EntidadSincroniza.UsuarioLibre;
        }

        protected override string ObtenerCodigoRegistroEntidad(EntityBase registroEntidad)
        {
            SiEmpresaDTO registroEntidadC = (SiEmpresaDTO)registroEntidad;
            return registroEntidadC.Emprcodi.ToString();
        }


        protected override string ObtenerEtiquetaRegistroEntidad(Base.Core.EntityBase registroEntidad)
        {
            SiEmpresaDTO registroEntidadC = (SiEmpresaDTO)registroEntidad;
            return registroEntidadC.Emprnomb.Trim();
        }

        public override int IniciarSincronizacion()
        {
            List<int> listaIdTipoEmpresa = new List<int>();
            listaIdTipoEmpresa.Add(int.Parse(TipoEmpresa.UsuarioLibre.ToString("D")));

            List<SiEmpresaDTO> lEmpresaDTO = FactorySic.GetSiEmpresaRepository().ListarEmpresasPorTipo(listaIdTipoEmpresa);

            string etiquetaRegistroEntidad = string.Empty;

            foreach (SiEmpresaDTO empresaDTO in lEmpresaDTO)
            {
                //- Solo se deben procesar las empresas activas.
                if (empresaDTO.Emprestado != ConstantesIntercambio.EmpresaCOESEstadoActivo)
                {
                    continue;
                }

                etiquetaRegistroEntidad = this.ObtenerEtiquetaRegistroEntidad(empresaDTO);

                //- 1. Se valida si la empresa COES tiene RUC.
                //- ==========================================
                if (String.IsNullOrEmpty(empresaDTO.Emprruc))
                {
                    //- De no tener RUC se registra como pendiente de sincronización.
                    this.RegistrarAsignacion(empresaDTO
                                           , MotivoPendiente.EmpresaCOESSinRUC
                                           , string.Empty);
                }
            }

            return 1;
        }

        public override Dictionary<string, string> ObtenerValoresHomologacion()
        {
            Dictionary<string, string> valoresHomologacion = new Dictionary<string, string>();

            roObtenerMaestroUsuarioLibre roObtenerUsuariooLibreOsi = this.servicioOsiSincronizacion.obtenerMaestroUsuarioLibre(); ;

            if (roObtenerUsuariooLibreOsi.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                throw new ApplicationException("El servicio web no ha respondido a la petición. Detalle del error. Detalle del mensaje "
                                             + roObtenerUsuariooLibreOsi.mensajeResultante);
            }

            foreach (usuarioLibreDTO usuarioLibre in roObtenerUsuariooLibreOsi.listaUsuariosLibres)
            {
                valoresHomologacion.Add(usuarioLibre.codigoUsuarioLibre, usuarioLibre.razonSocial);
            }

            return valoresHomologacion;
        }

        #endregion
       
    }
}

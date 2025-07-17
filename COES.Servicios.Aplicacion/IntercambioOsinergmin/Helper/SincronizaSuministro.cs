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
    public class SincronizaSuministro : Sincroniza
    {

        public SincronizaSuministro()
            : base()
        {

        }

        public SincronizaSuministro(SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(servicioOsiSincroniza)
        {

        }

        public SincronizaSuministro(DateTime fechaSincroniza, SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(fechaSincroniza, servicioOsiSincroniza)
        {

        }

        public override int IniciarSincronizacion()
        {

            int codigoFamilia = int.Parse(FamiliaCodi.Suministro.ToString("D"));

            List<EqEquipoDTO> lEquipoDTO = FactorySic.GetEqEquipoRepository().ListarEquiposPorFamilia(codigoFamilia.ToString());

            string etiquetaRegistroEntidad = string.Empty;

            foreach (EqEquipoDTO equipoDTO in lEquipoDTO)
            {
                //- Solo se deben procesar los equipos activos.
                if (equipoDTO.Equiestado != ConstantesIntercambio.EquipoCOESEstadoActivo)
                {
                    continue;
                }

                //- Si el registro ya tiene código Osinergmin entonces ya no se realiza acción alguna.
                //- Esto porque se asume que ya fue homologado.
                if (!string.IsNullOrEmpty(equipoDTO.Osinergcodi))
                {
                    continue;
                }

                etiquetaRegistroEntidad = this.ObtenerEtiquetaRegistroEntidad(equipoDTO);

                //- 1. Se valida si el equipo tiene el código Osinergmin.
                //- ====================================================
                if (String.IsNullOrEmpty(equipoDTO.Osinergcodi))
                {
                    //- De no tener código Osinergmin se registra como pendiente de sincronización.
                    this.RegistrarAsignacion(equipoDTO
                                           , MotivoPendiente.EntidadCOESSinCodigoOsi
                                           , string.Empty);
                }
            }

            return 1;
        }

        public override EntidadSincroniza ObtenerEntidadSincroniza()
        {
            return EntidadSincroniza.Suministro;
        }

        protected override string ObtenerCodigoRegistroEntidad(EntityBase registroEntidad)
        {
            EqEquipoDTO registroEntidadC = (EqEquipoDTO)registroEntidad;
            return registroEntidadC.Equicodi.ToString();
        }

        protected override string ObtenerEtiquetaRegistroEntidad(EntityBase registroEntidad)
        {
            EqEquipoDTO registroEntidadC = (EqEquipoDTO)registroEntidad;
            return registroEntidadC.Equinomb.Trim();
        }

        public override Dictionary<string, string> ObtenerValoresHomologacion()
        {
            Dictionary<string, string> valoresHomologacion = new Dictionary<string, string>();

            roObtenerMaestroSuministroUsuario roObtenerSuministroOsi = this.servicioOsiSincronizacion.obtenerMaestroSuministroUsuario();

            if (roObtenerSuministroOsi.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                throw new ApplicationException("El servicio web no ha respondido a la petición. Detalle del error. Detalle del mensaje "
                                             + roObtenerSuministroOsi.mensajeResultante);
            }

            foreach (suministroUsuarioDTO suministro in roObtenerSuministroOsi.listaSuministrosUsuario)
            {
                valoresHomologacion.Add(suministro.codigoSuministroUsuario, suministro.nombreUsuarioLibre);
            }

            return valoresHomologacion;
        }

        //- alpha.HDT - 14/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener los RUC de cada Suministro Usuario del Osinergmin.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, suministroUsuarioDTO> ObtenerSuministroXSuministroUsuario()
        {

            Dictionary<string, suministroUsuarioDTO> dicSuministroXSuministroUsuario = new Dictionary<string, suministroUsuarioDTO>();

            roObtenerMaestroSuministroUsuario roObtenerSuministroOsi = this.servicioOsiSincronizacion.obtenerMaestroSuministroUsuario();

            if (roObtenerSuministroOsi.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                throw new ApplicationException("El servicio web no ha respondido a la petición. Detalle del error. Detalle del mensaje "
                                             + roObtenerSuministroOsi.mensajeResultante);
            }

            foreach (suministroUsuarioDTO suministro in roObtenerSuministroOsi.listaSuministrosUsuario)
            {
                if (!dicSuministroXSuministroUsuario.ContainsKey(suministro.codigoSuministroUsuario)) {
                    dicSuministroXSuministroUsuario.Add(suministro.codigoSuministroUsuario, suministro);
                }                
            }

            return dicSuministroXSuministroUsuario;
        }
       
    }

}

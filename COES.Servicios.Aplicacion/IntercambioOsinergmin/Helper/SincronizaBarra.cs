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

    public class SincronizaBarra : Sincroniza
    {

        public SincronizaBarra()
            : base()
        {

        }

        public SincronizaBarra(SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(servicioOsiSincroniza)
        {

        }

        public SincronizaBarra(DateTime fechaSincroniza, SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(fechaSincroniza, servicioOsiSincroniza)
        {

        }

        public override int IniciarSincronizacion()
        {
            List<PrBarraDTO> lBarraDTO = FactorySic.GetPrBarraRepository().List();

            string etiquetaRegistroEntidad = string.Empty;

            foreach (PrBarraDTO barraDTO in lBarraDTO)
            {
                //- Solo se deben procesar las empresas activas.
                if (barraDTO.Barrestado != ConstantesIntercambio.BarraCOESEstadoActivo)
                {
                    continue;
                }

                etiquetaRegistroEntidad = this.ObtenerEtiquetaRegistroEntidad(barraDTO);

                //- 1. Se valida si el equipo tiene el código osinergmin.
                //- ====================================================
                if (String.IsNullOrEmpty(barraDTO.Osinergcodi) || barraDTO.Osinergcodi == "0")
                {
                    //- De no tener código osinergmin se registra como pendiente de sincronización.
                    this.RegistrarAsignacion(barraDTO
                                           , MotivoPendiente.EntidadCOESSinCodigoOsi
                                           , string.Empty);
                }
            }

            return 1;
        }

        public override EntidadSincroniza ObtenerEntidadSincroniza()
        {
            return EntidadSincroniza.Barra;
        }

        protected override string ObtenerCodigoRegistroEntidad(EntityBase registroEntidad)
        {
            PrBarraDTO registroEntidadC = (PrBarraDTO)registroEntidad;
            return registroEntidadC.Barrcodi.ToString();
        }

        protected override string ObtenerEtiquetaRegistroEntidad(EntityBase registroEntidad)
        {
            PrBarraDTO registroEntidadC = (PrBarraDTO)registroEntidad;
            return registroEntidadC.Barrnombre.Trim();
        }

        public override Dictionary<string, string> ObtenerValoresHomologacion()
        {
            #warning HDT.09.01.2017 - Inicio:  Los servicios web del Osinergmin actualmente no soportan la obtención de las barras.
            throw new ApplicationException("Los servicios web del Osinergmin actualmente no soportan la obtención de las barras");
            #warning HDT Fin
        }


        internal List<barraDTO> ObtenerBarras()
        {
            List<barraDTO> lBarraDTO = new List<barraDTO>();

            roObtenerMaestroBarra roObtenerMaestroBarraOsi = this.servicioOsiSincronizacion.obtenerMaestroBarra();

            if (roObtenerMaestroBarraOsi.valorResultante != ConstantesIntercambio.ValorResultanteOkOsiWS)
            {
                throw new ApplicationException("El servicio web no ha respondido a la petición. Detalle del error. Detalle del mensaje "
                                             + roObtenerMaestroBarraOsi.mensajeResultante);
            }

            if (roObtenerMaestroBarraOsi.listaBarra != null)
            {
                lBarraDTO = roObtenerMaestroBarraOsi.listaBarra.ToList();
            }

            return lBarraDTO;
        }
    }

}

// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: alpha
//
// Fecha creacion: 30/10/2016
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
    /// Clase que abstrae el comportamiento para la sincronización de los grupos COES que son creados en el COES y deben
    /// ser replicados al Osinergmin, vale decir que el COES es el que rige la sincronización.
    /// </summary>
    public class SincronizaCOESGrupo : Sincroniza
    {

        #region CONSTRUCTORES: Definicion de constructores de la clase.

        public SincronizaCOESGrupo()
            : base()
        {
        }

        public SincronizaCOESGrupo(SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(servicioOsiSincroniza)
        {
        }

        public SincronizaCOESGrupo(DateTime fechaSincroniza, SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(fechaSincroniza, servicioOsiSincroniza)
        {
        }

        #endregion

        #region METODOS: Metodos de la clase.

        public override EntidadSincroniza ObtenerEntidadSincroniza()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtiene la lista de categorías aplicables
        /// </summary>
        /// <returns></returns>
        public virtual List<int> ObtenerListaCategorias()
        {
            throw new NotImplementedException();
        }

        protected override string ObtenerCodigoRegistroEntidad(EntityBase registroEntidad)
        {
            PrGrupoDTO registroEntidadC = (PrGrupoDTO)registroEntidad;
            return registroEntidadC.Grupocodi.ToString();
        }

        protected override string ObtenerEtiquetaRegistroEntidad(EntityBase registroEntidad)
        {
            PrGrupoDTO registroEntidadC = (PrGrupoDTO)registroEntidad;
            return registroEntidadC.Gruponomb.Trim();
        }

        public override int IniciarSincronizacion()
        {
            List<int> listaCategorias = this.ObtenerListaCategorias();

            List<PrGrupoDTO> lGrupoDTO = FactorySic.GetPrGrupoRepository().ListarGruposPorCategoria(listaCategorias);
            bool tieneCodigoOrigen = false;

            foreach (PrGrupoDTO grupoCOES in lGrupoDTO)
            {

                if (grupoCOES.Grupoactivo == ConstantesIntercambio.GrupoCOESEstadoActivo)
                {
                    //- El registro está Activo
                    //- =======================

                    if (string.IsNullOrEmpty(grupoCOES.Osinergcodi))
                    {
                        //- El registro está Activo y No tiene código Osinergmin, entonces se procede a registrar el nuevo registro.

                        if (this.CrearRegistroEnOsinergmin(grupoCOES) == 1)
                        {
                            this.RegistrarAsignacion(grupoCOES
                                                   , MotivoPendiente.Ninguno
                                                   , "Se ha creado un nuevo registro en el Osinergmin a partir del registro del COES");
                        }
                    }
                    else
                    {
                        //- El registro ya fue sincronizado anteriormente, entonces evaluar si requiere cambio de estado.

                        if (this.AsegurarCoherenciaEstado(grupoCOES) == 1)
                        {
                            this.RegistrarAsignacion(grupoCOES
                                                   , MotivoPendiente.Ninguno
                                                   , "Se ha realizado el cambio de estado en el Osinergmin de acuerdo al estado en el COES");
                        }
                    }
                }
                else
                {
                    //- El registro está Inactivo
                    //- =========================

                    if (string.IsNullOrEmpty(grupoCOES.Osinergcodi))
                    {
                        tieneCodigoOrigen = false;

                        if (grupoCOES.Grupocodi2.HasValue)
                        {
                            if (grupoCOES.Grupocodi2.Value != 0)
                            {
                                tieneCodigoOrigen = true;
                            }
                        }

                        if (tieneCodigoOrigen)
                        {
                            //- Sí tiene código COES origen. Entonces se realiza la reasignación.
                            if (this.ReasignarCodigoOsinergmin(grupoCOES) == 1)
                            {
                                //- Si la reasignación COES tuvo éxito entonces se procede a actualizar los datos del Osinergmin
                                if (this.ActualizarRegistroEnOsinergmin(grupoCOES) == 1)
                                {
                                    //- Si la actualización en Osinergmin tuvo éxito, entonces se registra la asignación
                                    //- exitosa.
                                    this.RegistrarAsignacion(grupoCOES
                                                           , MotivoPendiente.Ninguno
                                                           , "Se ha realizado la reasignación de código COES en el Osinergmin");
                                }
                            }
                        }
                        else
                        {
                            //- No tiene código COES origen. Entonces se registra como pendiente de sincronización.

                            this.RegistrarAsignacion(grupoCOES
                                                   , MotivoPendiente.EntidadCOESInactivaSinCodigoCOESOriginal
                                                   , string.Empty);
                        }
                    }
                    else
                    {
                        //- El registro ya fue sincronizado anteriormente, entonces evaluar si requiere cambio de estado.

                        if (this.AsegurarCoherenciaEstado(grupoCOES) == 1)
                        {
                            this.RegistrarAsignacion(grupoCOES
                                                   , MotivoPendiente.Ninguno
                                                   , "Se ha realizado el cambio de estado en el Osinergmin de acuerdo al estado en el COES");
                        }

                    }
                }
            }

            return 1;
        }

        protected override int ReasignarCodigoOsinergmin(EntityBase registroEntidad)
        {
            PrGrupoDTO grupoCOESInactivo = (PrGrupoDTO)registroEntidad;

            PrGrupoDTO grupoCOESOriginal = FactorySic.GetPrGrupoRepository().GetByIdOsinergmin(grupoCOESInactivo.Grupocodi2.Value);

            if (grupoCOESOriginal == null)
            {
                //- Si el grupo original no existe.
                this.RegistrarAsignacion(grupoCOESInactivo
                                       , MotivoPendiente.GrupoOriginalCOESNoExiste
                                       , "Código del grupo original = " + grupoCOESInactivo.Grupocodi2.Value);
                return 0;
            }

            if (grupoCOESOriginal.Grupoactivo != ConstantesIntercambio.GrupoCOESEstadoActivo)
            {
                //- El grupo COES original no está activo entonces no se realiza acción alguna.
                return 0;
            }

            if (string.IsNullOrEmpty(grupoCOESOriginal.Osinergcodi))
            {
                //- El grupo COES original no tiene código Osinergmin entonces no se realiza acción alguna.
                return 0;
            }

            //- Copiando el código Osinergmin desde el grupo original.
            grupoCOESInactivo.Osinergcodi = grupoCOESOriginal.Osinergcodi;
            grupoCOESInactivo.Lastdate = DateTime.Now;
            grupoCOESInactivo.Lastuser = ConstantesIntercambio.UsuarioTareaAutomatica;

            FactorySic.GetPrGrupoRepository().UpdateOsinergmin(grupoCOESInactivo);

            //- Quitando el código Osinergmin al grupo original.
            grupoCOESOriginal.Osinergcodi = string.Empty;
            grupoCOESOriginal.Lastdate = DateTime.Now;
            grupoCOESOriginal.Lastuser = ConstantesIntercambio.UsuarioTareaAutomatica;

            FactorySic.GetPrGrupoRepository().UpdateOsinergmin(grupoCOESOriginal);

            return 1;
        }

        protected SiEmpresaDTO ObtenerEmpresa(PrGrupoDTO grupoOsi)
        {
            SiEmpresaDTO empresaCOES = FactorySic.GetSiEmpresaRepository().GetByIdOsinergmin(grupoOsi.Emprcodi.Value);

            return empresaCOES;
        }

        #endregion

    }
}

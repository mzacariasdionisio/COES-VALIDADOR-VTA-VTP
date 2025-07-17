// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: alpha
//
// Fecha creacion: 31/10/2016
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
    /// Enumera el código de la familia.
    /// </summary>
    public enum FamiliaCodi
    {
        Ninguno = 0,
        CuarentaYUno = 41,
        Suministro = 45,
        CuarentaYSeite = 47,
        Embalse = 19
    }

    /// <summary>
    /// Clase que abstrae el comportamiento para la sincronización de los equipos COES que son creados en el COES y deben
    /// ser replicados al Osinergmin, vale decir que el COES es el que rige la sincronización.
    /// </summary>
    public class SincronizaCOESEquipo : Sincroniza
    {

        #region CONSTRUCTORES: Definicion de constructores de la clase.

        public SincronizaCOESEquipo()
            : base()
        {
        }

        public SincronizaCOESEquipo(SincronizacionMaestrosEndpointService servicioOsiSincroniza)
            : base(servicioOsiSincroniza)
        {
        }

        public SincronizaCOESEquipo(DateTime fechaSincroniza, SincronizacionMaestrosEndpointService servicioOsiSincroniza)
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
        /// Obtiene la lista de familias aplicables
        /// </summary>
        /// <returns></returns>
        public virtual int ObtenerIdFamilia()
        {
            throw new NotImplementedException();
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

        public override int IniciarSincronizacion()
        {
            int idFamilia = this.ObtenerIdFamilia();

            List<EqEquipoDTO> lEquipoDTO = FactorySic.GetEqEquipoRepository().ListarEquiposPorFamilia(idFamilia.ToString());
            bool tieneCodigoOrigen = false;

            foreach (EqEquipoDTO equipoCOES in lEquipoDTO)
            {
                if (equipoCOES.Equiestado != ConstantesIntercambio.EquipoCOESEstadoActivo
                    && equipoCOES.Equiestado != ConstantesIntercambio.EquipoCOESEstadoBaja)
                {
                    //- Si los estados no son Activo ni Baja, entonces no se realiza acción alguna.
                    continue;
                }

                if (equipoCOES.Equiestado == ConstantesIntercambio.EquipoCOESEstadoActivo)
                {
                    //- El registro está Activo
                    //- =======================

                    if (string.IsNullOrEmpty(equipoCOES.Osinergcodi) || equipoCOES.Osinergcodi == "0")
                    {
                        //- Entonces se procede a registrar el nuevo registro.
                        if (this.CrearRegistroEnOsinergmin(equipoCOES) == 1)
                        {
                            this.RegistrarAsignacion(equipoCOES
                                                   , MotivoPendiente.Ninguno
                                                   , "Se ha creado un nuevo registro en el Osinergmin a partir del registro del COES");
                        }
                    }
                    else
                    {
                        //- El registro está activo. Entonces evaluar si requiere cambio de estado.
                        if (this.AsegurarCoherenciaEstado(equipoCOES) == 1)
                        {
                            this.RegistrarAsignacion(equipoCOES
                                                   , MotivoPendiente.Ninguno
                                                   , "Se ha realizado el cambio de estado en el Osinergmin de acuerdo al estado en el COES");
                        }
                    }
                }
                else
                {
                    //- El registro está Inactivo
                    //- =========================
                    if (string.IsNullOrEmpty(equipoCOES.Osinergcodi) || equipoCOES.Osinergcodi == "0")
                    {
                        tieneCodigoOrigen = false;

                        if (equipoCOES.Lastcodi.HasValue)
                        {
                            if (equipoCOES.Lastcodi.Value != 0 && equipoCOES.Lastcodi.Value != -1)
                            {
                                tieneCodigoOrigen = true;
                            }
                        }

                        if (tieneCodigoOrigen)
                        {
                            //- Sí tiene código COES anterior. Entonces se realiza la reasignación.
                            if (this.ReasignarCodigoOsinergmin(equipoCOES) == 1)
                            {
                                //- Si la reasignación COES tuvo éxito entonces se procede a actualizar Osinergmin
                                if (this.ActualizarRegistroEnOsinergmin(equipoCOES) == 1)
                                {
                                    //- Si la actualización en Osinergmin tuvo éxito, entonces se registra la asignación
                                    //- exitosa.
                                    this.RegistrarAsignacion(equipoCOES
                                                           , MotivoPendiente.Ninguno
                                                           , "Se ha realizado la reasignación de código COES en el Osinergmin");
                                }
                            }
                        }
                        else
                        {
                            //- No tiene código COES origen. Entonces se registra como pendiente de sincronización.
                            this.RegistrarAsignacion(equipoCOES
                                                   , MotivoPendiente.EntidadCOESInactivaSinCodigoCOESOriginal
                                                   , string.Empty);
                        }

                    }
                    else
                    {
                        //- El registro ya fue sincronizado anteriormente, entonces evaluar si requiere cambio de estado.

                        if (this.AsegurarCoherenciaEstado(equipoCOES) == 1)
                        {
                            this.RegistrarAsignacion(equipoCOES
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
            EqEquipoDTO equipoCOESInactivo = (EqEquipoDTO)registroEntidad;

            EqEquipoDTO equipoCOESOriginal = FactorySic.GetEqEquipoRepository().GetByIdOsinergmin(equipoCOESInactivo.Lastcodi.Value);

            if (equipoCOESOriginal == null)
            {
                //- Si el equipo original no existe.
                this.RegistrarAsignacion(equipoCOESInactivo
                                       , MotivoPendiente.EquipoOriginalCOESNoExiste
                                       , "Código de la cuenca original = " + equipoCOESInactivo.Lastcodi.Value);
                return 0;
            }

            if (equipoCOESOriginal.Equiestado == ConstantesIntercambio.EquipoCOESEstadoBaja)
            {
                //- El equipo COES original no está activo entonces no se realiza acción alguna.
                return 0;
            }

            //- Copiando el código Osinergmin desde el equipo original.
            equipoCOESInactivo.Osinergcodi = equipoCOESOriginal.Osinergcodi;
            equipoCOESInactivo.Lastdate = DateTime.Now;
            equipoCOESInactivo.Lastuser = ConstantesIntercambio.UsuarioTareaAutomatica;

            FactorySic.GetEqEquipoRepository().UpdateOsinergmin(equipoCOESInactivo);

            //- Quitando el código Osinergmin al equipo original.
            equipoCOESOriginal.Osinergcodi = string.Empty;
            equipoCOESOriginal.Lastdate = DateTime.Now;
            equipoCOESOriginal.Lastuser = ConstantesIntercambio.UsuarioTareaAutomatica;

            FactorySic.GetEqEquipoRepository().UpdateOsinergmin(equipoCOESOriginal);

            return 1;
        }

        #endregion

    }
}

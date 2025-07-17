using COES.Base.Core;
using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Evento;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.Interconexiones.Helper;
using COES.Servicios.Aplicacion.Intervenciones;
using COES.Servicios.Aplicacion.Intervenciones.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.Medidores;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using COES.Servicios.Aplicacion.Pruebaunidad;
using COES.Servicios.Aplicacion.StockCombustibles;
using COES.Servicios.Aplicacion.TiempoReal;
using COES.Servicios.Aplicacion.Titularidad;
using COES.Servicios.Aplicacion.YupanaContinuo.Helper;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using static COES.Servicios.Aplicacion.Migraciones.Helper.UtilCdispatch;

namespace COES.Servicios.Aplicacion.Migraciones
{
    /// <summary>
    /// Clases con métodos de proyecto migraciones
    /// </summary>
    public class MigracionesAppServicio : AppServicioBase
    {
        FichaTecnicaAppServicio servFictec = new FichaTecnicaAppServicio();
        HorasOperacionAppServicio servHO = new HorasOperacionAppServicio();
        EjecutadoAppServicio servEjec = new EjecutadoAppServicio();
        ParametroAppServicio servParametro = new ParametroAppServicio();
        DespachoAppServicio servDespacho = new DespachoAppServicio();

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MigracionesAppServicio));

        #region Métodos Tabla ME_MEDICION1

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <param name="ff1"></param>
        /// <param name="ff2"></param>
        /// <returns></returns>
        public List<MeMedicion1DTO> GetListaMedicion1(int lectcodi, DateTime ff1, DateTime ff2)
        {
            return FactorySic.GetMeMedicion1Repository().GetListaMedicion1(lectcodi, ff1, ff2);
        }

        /// <summary>
        /// Guardar datos en Medicion1
        /// </summary>
        /// <param name="obj"></param>
        public void SaveMemedicion1(MeMedicion1DTO obj)
        {
            try
            {
                FactorySic.GetMeMedicion1Repository().Save(obj);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Delete datos en Medicion1
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <param name="fecha"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="ptomedicodi"></param>
        public void DeleteMemedicion1(int lectcodi, DateTime fecha, int tipoinfocodi, int ptomedicodi)
        {
            try
            {
                FactorySic.GetMeMedicion1Repository().Delete(lectcodi, fecha, tipoinfocodi, ptomedicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla ME_MEDICION48

        /// <summary>
        /// Obtener el listado de medicion48 segun lectura
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetObtenerHistoricoMedicion48(int lectcodi, DateTime fechaIni, DateTime fechaFin)
        {
            List<MeMedicion48DTO> lista48 = FactorySic.GetMeMedicion48Repository().ListarMeMedicion48ByFiltro(lectcodi.ToString(), fechaIni, fechaFin, ConstantesAppServicio.ParametroDefecto);

            return lista48;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetListaMedicion48xlectcodi(int lectcodi)
        {
            return FactorySic.GetMeMedicion48Repository().GetListaMedicion48xlectcodi(lectcodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ins"></param>
        public void SaveMemedicion48(MeMedicion48DTO ins)
        {
            try
            {
                FactorySic.GetMeMedicion48Repository().Save(ins);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <param name="medifecha"></param>
        /// <param name="ptomedicodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="med"></param>
        public void DeleteMemedicionmasivo48(int lectcodi, DateTime medifecha, string ptomedicodi, string tipoinfocodi)
        {
            try
            {
                FactorySic.GetMeMedicion48Repository().DeleteMasivo(lectcodi, medifecha, tipoinfocodi, ptomedicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla SI_ACTIVIDAD

        /// <summary>
        /// Inserta un registro de la tabla SI_ACTIVIDAD
        /// </summary>
        public void SaveSiActividad(SiActividadDTO entity)
        {
            try
            {
                FactorySic.GetSiActividadRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_ACTIVIDAD
        /// </summary>
        public void UpdateSiActividad(SiActividadDTO entity)
        {
            try
            {
                FactorySic.GetSiActividadRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_ACTIVIDAD
        /// </summary>
        public void DeleteSiActividad(int actcodi)
        {
            try
            {
                FactorySic.GetSiActividadRepository().Delete(actcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_ACTIVIDAD
        /// </summary>
        public SiActividadDTO GetByIdSiActividad(int actcodi)
        {
            return FactorySic.GetSiActividadRepository().GetById(actcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_ACTIVIDAD
        /// </summary>
        public List<SiActividadDTO> ListSiActividads()
        {
            return FactorySic.GetSiActividadRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiActividad
        /// </summary>
        public List<SiActividadDTO> GetByCriteriaSiActividads()
        {
            return FactorySic.GetSiActividadRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SI_AREA

        /// <summary>
        /// Inserta un registro de la tabla SI_AREA
        /// </summary>
        public void SaveSiArea(SiAreaDTO entity)
        {
            try
            {
                FactorySic.GetSiAreaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_AREA
        /// </summary>
        public void UpdateSiArea(SiAreaDTO entity)
        {
            try
            {
                FactorySic.GetSiAreaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_AREA
        /// </summary>
        public void DeleteSiArea(int areacodi)
        {
            try
            {
                FactorySic.GetSiAreaRepository().Delete(areacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_AREA
        /// </summary>
        public SiAreaDTO GetByIdSiArea(int areacodi)
        {
            return FactorySic.GetSiAreaRepository().GetById(areacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_AREA
        /// </summary>
        public List<SiAreaDTO> ListSiAreas()
        {
            return FactorySic.GetSiAreaRepository().List();
        }

        #region GESTPROTECT
        /// <summary>
        /// Permite listar todos los registros de la tabla SI_AREA
        /// </summary>
        public List<SiAreaDTO> ListSiAreasSGOCOES()
        {
            return FactorySic.GetSiAreaRepository().ListSGOCOES();
        }
        #endregion
        
        /// <summary>
        /// Permite realizar búsquedas en la tabla SiArea
        /// </summary>
        public List<SiAreaDTO> GetByCriteriaSiAreas()
        {
            return FactorySic.GetSiAreaRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SI_ROL_TURNO

        /// <summary>
        /// Inserta un registro de la tabla SI_ROL_TURNO
        /// </summary>
        public void SaveSiRolTurno(SiRolTurnoDTO entity)
        {
            try
            {
                FactorySic.GetSiRolTurnoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_ROL_TURNO
        /// </summary>
        public void UpdateSiRolTurno(SiRolTurnoDTO entity)
        {
            try
            {
                FactorySic.GetSiRolTurnoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_ROL_TURNO
        /// </summary>
        public void DeleteSiRolTurno(DateTime roltfecha, int actcodi, DateTime lastdate, int percodi)
        {
            try
            {
                FactorySic.GetSiRolTurnoRepository().Delete(roltfecha, actcodi, lastdate, percodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_ROL_TURNO
        /// </summary>
        public SiRolTurnoDTO GetByIdSiRolTurno(DateTime roltfecha, int actcodi, DateTime lastdate, int percodi)
        {
            return FactorySic.GetSiRolTurnoRepository().GetById(roltfecha, actcodi, lastdate, percodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_ROL_TURNO
        /// </summary>
        public List<SiRolTurnoDTO> ListSiRolTurnos()
        {
            return FactorySic.GetSiRolTurnoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiRolTurno
        /// </summary>
        public List<SiRolTurnoDTO> GetByCriteriaSiRolTurnos()
        {
            return FactorySic.GetSiRolTurnoRepository().GetByCriteria();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Rols"></param>
        public void SaveSiRolTurnoMasivo(List<SiRolTurnoDTO> Rols)
        {
            FactorySic.GetSiRolTurnoRepository().SaveSiRolTurnoMasivo(Rols);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <param name="percodi"></param>
        public void DeleteSiRolTurnoMasivo(DateTime fecIni, DateTime fecFin, string percodi)
        {
            FactorySic.GetSiRolTurnoRepository().DeleteSiRolTurnoMasivo(fecIni, fecFin, percodi);
        }

        /// <summary>
        /// Listar movimientos
        /// </summary>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <returns></returns>
        public List<SiRolTurnoDTO> ListaMovimientos(DateTime fecIni, DateTime fecFin)
        {
            List<SiRolTurnoDTO> lista = FactorySic.GetSiRolTurnoRepository().ListaMovimientos(fecIni, fecFin);
            foreach (var reg in lista)
            {
                reg.RoltfechaactualizacionDesc = reg.Lastdate.ToString(ConstantesAppServicio.FormatoFechaHoraAMPM);
            }

            return lista;
        }

        #endregion

        #region Métodos Tabla SI_PERSONA

        public List<SiPersonaDTO> GetListaPersonalRol(string areacodi, DateTime fecIni, DateTime fecFin)
        {
            return FactorySic.GetSiPersonaRepository().ListaPersonalRol(areacodi, fecIni, fecFin);
        }

        #endregion

        #region Métodos Tabla PR_AGRUPACION

        /// <summary>
        /// Inserta un registro de la tabla PR_AGRUPACION
        /// </summary>
        public int SavePrAgrupacion(PrAgrupacionDTO entity)
        {
            return FactorySic.GetPrAgrupacionRepository().Save(entity);
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_AGRUPACION
        /// </summary>
        public void UpdatePrAgrupacion(PrAgrupacionDTO entity)
        {
            try
            {
                FactorySic.GetPrAgrupacionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PR_AGRUPACION
        /// </summary>
        public void DeletePrAgrupacion(PrAgrupacionDTO entity)
        {
            try
            {
                FactorySic.GetPrAgrupacionRepository().Delete(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_AGRUPACION
        /// </summary>
        public PrAgrupacionDTO GetByIdPrAgrupacion(int agrupcodi)
        {
            return FactorySic.GetPrAgrupacionRepository().GetById(agrupcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrAgrupacion
        /// </summary>
        public List<PrAgrupacionDTO> GetByCriteriaPrAgrupacions(int agrupfuente, string estado)
        {
            List<PrAgrupacionDTO> lista = FactorySic.GetPrAgrupacionRepository().GetByCriteria(agrupfuente).OrderBy(x => x.Agrupnombre).ToList();
            if (ConstantesAppServicio.ParametroDefecto != estado) lista = lista.Where(x => x.Agrupestado == estado).ToList();

            foreach (var reg in lista)
            {
                reg.Agrupnombre = reg.Agrupnombre != null ? reg.Agrupnombre.Trim() : string.Empty;
                reg.AgrupfeccreacionDesc = reg.Agrupfeccreacion != null ? reg.Agrupfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
                reg.AgrupfecmodificacionDesc = reg.Agrupfecmodificacion != null ? reg.Agrupfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
                reg.AgrupestadoDesc = Util.EstadoDescripcion(reg.Agrupestado);
            }

            return lista;
        }

        #endregion

        #region Métodos Tabla PR_AGRUPACIONCONCEPTO

        /// <summary>
        /// Inserta un registro de la tabla PR_AGRUPACIONCONCEPTO
        /// </summary>
        public void SavePrAgrupacionConcepto(PrAgrupacionConceptoDTO entity)
        {
            try
            {
                FactorySic.GetPrAgrupacionConceptoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_AGRUPACIONCONCEPTO
        /// </summary>
        public void UpdatePrAgrupacionConcepto(PrAgrupacionConceptoDTO entity)
        {
            try
            {
                FactorySic.GetPrAgrupacionConceptoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PR_AGRUPACIONCONCEPTO
        /// </summary>
        public void DeletePrAgrupacionConcepto(int agrconcodi)
        {
            try
            {
                FactorySic.GetPrAgrupacionConceptoRepository().Delete(agrconcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_AGRUPACIONCONCEPTO
        /// </summary>
        public PrAgrupacionConceptoDTO GetByIdPrAgrupacionConcepto(int agrconcodi)
        {
            return FactorySic.GetPrAgrupacionConceptoRepository().GetById(agrconcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrAgrupacionConcepto
        /// </summary>
        public List<PrAgrupacionConceptoDTO> GetByCriteriaPrAgrupacionConceptos(int estado, int agrupcodi)
        {
            List<PrAgrupacionConceptoDTO> lista = FactorySic.GetPrAgrupacionConceptoRepository().GetByCriteria(agrupcodi)
                                                .Where(x => estado == -1 || x.Agrconactivo == estado).OrderBy(x => x.Concepcodi).ToList();

            foreach (var reg in lista)
            {
                reg.Concepdesc = reg.Concepdesc != null ? reg.Concepdesc.Trim() : string.Empty;
                reg.Concepabrev = reg.Concepabrev != null ? reg.Concepabrev.Trim() : string.Empty;
                reg.Concepnombficha = reg.Concepnombficha != null ? reg.Concepnombficha.Trim() : string.Empty;
                reg.Concepunid = reg.Concepunid != null ? reg.Concepunid.Trim() : string.Empty;
                reg.Conceptipo = reg.Conceptipo != null ? reg.Conceptipo.Trim() : string.Empty;
                reg.Catenomb = reg.Catenomb ?? "";
                reg.Cateabrev = reg.Cateabrev ?? "";

                reg.Conceporigen = reg.Concepcodi == null ? 2 : 1;

                reg.Orden = reg.Propcodi.GetValueOrDefault(0) + reg.Concepcodi.GetValueOrDefault(0); //orden por codigo de parametro
            }

            return lista.OrderBy(x => x.Orden).ToList();
        }

        #endregion

        #region Métodos Tabla PR_AREACONCEPTO

        /// <summary>
        /// Inserta un registro de la tabla PR_AREACONCEPTO
        /// </summary>
        public void SavePrAreaConcepto(PrAreaConceptoDTO entity)
        {
            try
            {
                FactorySic.GetPrAreaConceptoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_AREACONCEPTO
        /// </summary>
        public void UpdatePrAreaConcepto(PrAreaConceptoDTO entity)
        {
            try
            {
                FactorySic.GetPrAreaConceptoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_AREACONCEPTO
        /// </summary>
        public PrAreaConceptoDTO GetByIdPrAreaConcepto(int arconcodi)
        {
            return FactorySic.GetPrAreaConceptoRepository().GetById(arconcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_AREACONCEPTO
        /// </summary>
        public List<PrAreaConceptoDTO> ListPrAreaConceptos()
        {
            return FactorySic.GetPrAreaConceptoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrAreaConcepto
        /// </summary>
        public List<PrAreaConceptoDTO> GetByCriteriaPrAreaConceptos(int concepcodi, string arconactivo)
        {
            return FactorySic.GetPrAreaConceptoRepository().GetByCriteria(concepcodi, arconactivo);
        }

        #endregion

        #region Métodos Tabla PR_AREACONCEPUSER

        /// <summary>
        /// Inserta un registro de la tabla PR_AREACONCEPUSER
        /// </summary>
        public void SavePrAreaConcepUser(PrAreaConcepUserDTO entity)
        {
            try
            {
                FactorySic.GetPrAreaConcepUserRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                //throw new Exception(ex.Message, ex); TODO quitar el comentario, en la BD de prueba no existe algunos usuarios
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_AREACONCEPUSER
        /// </summary>
        public void UpdatePrAreaConcepUser(PrAreaConcepUserDTO entity)
        {
            try
            {
                FactorySic.GetPrAreaConcepUserRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                //throw new Exception(ex.Message, ex); TODO quitar el comentario, en la BD de prueba no existe algunos usuarios
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_AREACONCEPUSER
        /// </summary>
        public PrAreaConcepUserDTO GetByIdPrAreaConcepUser(int aconuscodi)
        {
            return FactorySic.GetPrAreaConcepUserRepository().GetById(aconuscodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_AREACONCEPUSER
        /// </summary>
        public List<PrAreaConcepUserDTO> ListPrAreaConcepUsers()
        {
            return FactorySic.GetPrAreaConcepUserRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrAreaConcepUser
        /// </summary>
        public List<PrAreaConcepUserDTO> GetByCriteriaPrAreaConcepUsers(int concepcodi, string arconactivo, string aconusactivo)
        {
            return FactorySic.GetPrAreaConcepUserRepository().GetByCriteria(concepcodi, arconactivo, aconusactivo);
        }

        #endregion

        #region Métodos Tabla PR_GRUPO_EQUIPO_VAL

        /// <summary>
        /// Inserta un registro de la tabla PR_GRUPO_EQUIPO_VAL
        /// </summary>
        public void SavePrGrupoEquipoVal(PrGrupoEquipoValDTO entity)
        {
            try
            {
                FactorySic.GetPrGrupoEquipoValRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_GRUPO_EQUIPO_VAL
        /// </summary>
        public void UpdatePrGrupoEquipoVal(PrGrupoEquipoValDTO entity)
        {
            try
            {
                FactorySic.GetPrGrupoEquipoValRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PR_GRUPO_EQUIPO_VAL
        /// </summary>
        public void DeletePrGrupoEquipoVal(int grupocodi, int concepcodi, int equicodi, DateTime greqvafechadat, int greqvadeleted)
        {
            try
            {
                FactorySic.GetPrGrupoEquipoValRepository().Delete(grupocodi, concepcodi, equicodi, greqvafechadat, greqvadeleted);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_GRUPO_EQUIPO_VAL
        /// </summary>
        public PrGrupoEquipoValDTO GetByIdPrGrupoEquipoVal(int grupocodi, int concepcodi, int equicodi, DateTime greqvafechadat, int greqvadeleted)
        {
            return FactorySic.GetPrGrupoEquipoValRepository().GetById(grupocodi, concepcodi, equicodi, greqvafechadat, greqvadeleted);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_GRUPO_EQUIPO_VAL
        /// </summary>
        public List<PrGrupoEquipoValDTO> ListPrGrupoEquipoVals()
        {
            return FactorySic.GetPrGrupoEquipoValRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrGrupoEquipoVal
        /// </summary>
        public List<PrGrupoEquipoValDTO> GetByCriteriaPrGrupoEquipoVals()
        {
            return FactorySic.GetPrGrupoEquipoValRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla PR_GRUPO

        /// <summary>
        /// Inserta un registro de la tabla PR_GRUPO
        /// </summary>
        public int SavePrGrupo(PrGrupoDTO entity)
        {
            try
            {
                return FactorySic.GetPrGrupoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_GRUPO
        /// </summary>
        public void UpdatePrGrupo(PrGrupoDTO entity)
        {
            try
            {
                FactorySic.GetPrGrupoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PR_GRUPO
        /// </summary>
        public void DeletePrGrupo(int grupocodi, string username)
        {
            try
            {
                FactorySic.GetPrGrupoRepository().Delete(grupocodi);
                FactorySic.GetPrGrupoRepository().Delete_UpdateAuditoria(grupocodi, username);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_GRUPO
        /// </summary>
        public PrGrupoDTO GetByIdPrGrupo(int grupocodi)
        {
            var obj = FactorySic.GetPrGrupoRepository().GetById(grupocodi);

            if (obj != null) FormatearPrGrupo(obj);

            return obj;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_GRUPO
        /// </summary>
        public List<PrGrupoDTO> ListPrGrupos()
        {
            return FactorySic.GetPrGrupoRepository().List();
        }

        private void FormatearPrGrupo(PrGrupoDTO obj)
        {
            obj.GrupofeccreacionDesc = "";
            obj.GrupofecmodificacionDesc = "";
            if (obj.Grupofeccreacion != null) obj.GrupofeccreacionDesc = obj.Grupofeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull);
            if (obj.Grupofecmodificacion != null) obj.GrupofecmodificacionDesc = obj.Grupofecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull);

            obj.Grupousucreacion = obj.Grupousucreacion ?? "";
            obj.Grupousumodificacion = obj.Grupousumodificacion ?? "";

            obj.Gruponomb = obj.Gruponomb ?? "";
            obj.Grupoabrev = obj.Grupoabrev ?? "";
        }

        #endregion

        #region Métodos Tabla PR_GRUPOEQ

        /// <summary>
        /// Inserta un registro de la tabla PR_GRUPOEQ
        /// </summary>
        public void SavePrGrupoeq(PrGrupoeqDTO entity)
        {
            try
            {
                FactorySic.GetPrGrupoeqRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_GRUPOEQ
        /// </summary>
        public void UpdatePrGrupoeq(PrGrupoeqDTO entity)
        {
            try
            {
                FactorySic.GetPrGrupoeqRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PR_GRUPOEQ
        /// </summary>
        public void DeletePrGrupoeq(int geqcodi)
        {
            try
            {
                FactorySic.GetPrGrupoeqRepository().Delete(geqcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_GRUPOEQ
        /// </summary>
        public PrGrupoeqDTO GetByIdPrGrupoeq(int geqcodi)
        {
            return FactorySic.GetPrGrupoeqRepository().GetById(geqcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_GRUPOEQ
        /// </summary>
        public List<PrGrupoeqDTO> ListPrGrupoeqs()
        {
            return FactorySic.GetPrGrupoeqRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrGrupoeq
        /// </summary>
        public List<PrGrupoeqDTO> GetByCriteriaPrGrupoeqs(int grupocodi)
        {
            return FactorySic.GetPrGrupoeqRepository().GetByCriteria(grupocodi, -1);
        }

        #endregion

        #region Métodos Tabla SI_FUENTEENERGIA

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_FUENTEENERGIA
        /// </summary>
        public List<SiFuenteenergiaDTO> ListSiFuenteenergias()
        {
            return FactorySic.GetSiFuenteenergiaRepository().List();
        }

        #endregion

        #region Métodos Tabla SI_EMPRESA

        /// <summary>
        /// Permite obtener las empresa de generación
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasGeneradoras()
        {
            return FactorySic.GetSiEmpresaRepository().ListarEmpresasxTipoEquipos(ConstantesHorasOperacion.CodFamilias, ConstantesAppServicio.ParametroDefecto);
        }

        #endregion

        #region Métodos Tabla PR_HTRABAJO_ESTADO

        /// <summary>
        /// Inserta un registro de la tabla PR_HTRABAJO_ESTADO
        /// </summary>
        public void SavePrHtrabajoEstado(PrHtrabajoEstadoDTO entity)
        {
            try
            {
                FactorySic.GetPrHtrabajoEstadoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_HTRABAJO_ESTADO
        /// </summary>
        public void UpdatePrHtrabajoEstado(PrHtrabajoEstadoDTO entity)
        {
            try
            {
                FactorySic.GetPrHtrabajoEstadoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PR_HTRABAJO_ESTADO
        /// </summary>
        public void DeletePrHtrabajoEstado(int htestcodi)
        {
            try
            {
                FactorySic.GetPrHtrabajoEstadoRepository().Delete(htestcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_HTRABAJO_ESTADO
        /// </summary>
        public PrHtrabajoEstadoDTO GetByIdPrHtrabajoEstado(int htestcodi)
        {
            return FactorySic.GetPrHtrabajoEstadoRepository().GetById(htestcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_HTRABAJO_ESTADO
        /// </summary>
        public List<PrHtrabajoEstadoDTO> ListPrHtrabajoEstados()
        {
            return FactorySic.GetPrHtrabajoEstadoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrHtrabajoEstado
        /// </summary>
        public List<PrHtrabajoEstadoDTO> GetByCriteriaPrHtrabajoEstados()
        {
            return FactorySic.GetPrHtrabajoEstadoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla PR_RESERVA

        /// <summary>
        /// Inserta un registro de la tabla PR_RESERVA
        /// </summary>
        public void SavePrReserva(PrReservaDTO entity)
        {
            try
            {
                FactorySic.GetPrReservaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta un registro de la tabla PR_RESERVA
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int SavePrReservaTransaccional(PrReservaDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                return FactorySic.GetPrReservaRepository().SaveTransaccional(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_RESERVA
        /// </summary>
        public void UpdatePrReserva(PrReservaDTO entity)
        {
            try
            {
                FactorySic.GetPrReservaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_RESERVA
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <exception cref="Exception"></exception>
        public void UpdatePrReservaTransaccional(PrReservaDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetPrReservaRepository().UpdateTransaccional(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Elimina un registro de la tabla PR_RESERVA
        /// </summary>
        public void DeletePrReserva(int prsvcodi)
        {
            try
            {
                FactorySic.GetPrReservaRepository().Delete(prsvcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_RESERVA
        /// </summary>
        public PrReservaDTO GetByIdPrReserva(int prsvcodi)
        {
            return FactorySic.GetPrReservaRepository().GetById(prsvcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_RESERVA
        /// </summary>
        public List<PrReservaDTO> ListPrReservas()
        {
            return FactorySic.GetPrReservaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrReserva
        /// </summary>
        public List<PrReservaDTO> GetByCriteriaPrReservas(DateTime fecha, string tipo)
        {
            return FactorySic.GetPrReservaRepository().GetByCriteria(fecha, tipo);
        }

        #endregion

        #region Registro de Actividades

        /// <summary>
        /// Get informacion de lista de actividad BD
        /// </summary>
        /// <param name="areacodi"></param>
        /// <returns></returns>
        public List<SiActividadDTO> GetListaActividadesPersonal(string areacodi)
        {
            return FactorySic.GetSiActividadRepository().GetListaActividadesPersonal(areacodi);
        }

        /// <summary>
        /// Reporte lista de actividades de personal
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ruta"></param>
        /// <returns></returns>
        public string ListaActividadesPersonalHtml(List<SiActividadDTO> data, string ruta)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='pretty tabla-icono' id='tb_Actividades'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>Acciones</th>");
            strHtml.Append("<th>Area</th>");
            strHtml.Append("<th>Abreviatura</th>");
            strHtml.Append("<th>Descripcion</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var list in data)
            {
                strHtml.Append("<tr>");
                strHtml.Append(string.Format(
                    "<td><a href='#' onclick='edit_(" + list.Actcodi + ", 1);'><img src='{0}Content/Images/Visualizar.png' /></a>&nbsp;<a href='#' onclick='edit_(" + list.Actcodi + ", 2);'><img src='{0}Content/Images/Pen.png' /></a>&nbsp;<a href='#' onclick='delete_(" + list.Actcodi + ");'><img src='{0}Content/Images/Trash.png' /></a></td>"
                    , ruta));
                strHtml.Append(string.Format("<td>{0}</td>", list.Areaabrev));
                strHtml.Append(string.Format("<td>{0}</td>", list.Actabrev));
                strHtml.Append(string.Format("<td>{0}</td>", list.Actnomb));
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        #endregion

        #region Registro / Edición de Grupo

        /// <summary>
        /// Guardar información de PR_GRUPO
        /// </summary>
        /// <param name="objGrupo"></param>
        /// <param name="usuario"></param>
        /// <param name="listaEquicodis"></param>
        public void GuardarPrGrupo(PrGrupoDTO objGrupo, string usuario, List<int> listaEquicodisActivo)
        {
            bool esNuevo = objGrupo.Grupocodi <= 0;
            if (esNuevo)
            {
                objGrupo.Lastuser = usuario;
                objGrupo.Lastdate = DateTime.Now;
                objGrupo.Grupousucreacion = usuario;
                objGrupo.Grupofeccreacion = DateTime.Now;
                objGrupo.Grupousumodificacion = usuario;
                objGrupo.Grupofecmodificacion = DateTime.Now;

                objGrupo.Grupocodi = SavePrGrupo(objGrupo);
            }
            else
            {
                PrGrupoDTO objBD = GetByIdPrGrupo(objGrupo.Grupocodi);
                objBD.Gruponomb = objGrupo.Gruponomb;
                objBD.Grupoabrev = objGrupo.Grupoabrev;

                objBD.Emprcodi = objGrupo.Emprcodi;
                //objBD.Catecodi = objGrupo.Catecodi;
                objBD.Grupotipomodo = objGrupo.Grupotipomodo;
                objBD.Areacodi = objGrupo.Areacodi;
                objBD.Grupopadre = objGrupo.Grupopadre;

                objBD.Grupointegrante = objGrupo.Grupointegrante;
                objBD.TipoGenerRer = objGrupo.TipoGenerRer;
                objBD.Grupotipocogen = objGrupo.Grupotipocogen;
                objBD.Gruponodoenergetico = objGrupo.Gruponodoenergetico;
                objBD.Gruporeservafria = objGrupo.Gruporeservafria;
                objBD.Osinergcodi = objGrupo.Osinergcodi;

                objBD.Fenergcodi = objGrupo.Fenergcodi;

                objBD.GrupoEstado = objGrupo.GrupoEstado;
                objBD.Grupoactivo = objGrupo.Grupoactivo;

                objBD.Lastuser = usuario;
                objBD.Lastdate = DateTime.Now;
                objBD.Grupousumodificacion = usuario;
                objBD.Grupofecmodificacion = DateTime.Now;

                UpdatePrGrupo(objBD);
            }

            //guardar en tabla TTIE
            (new TitularidadAppServicio()).SaveSiHisempgrupoDataInicial(objGrupo.Emprcodi ?? 0, objGrupo.Grupocodi, objGrupo.GrupoEstado, usuario);

            //guardar historico de Codigo Osinergmin
            int concepcodi = 0;
            if (objGrupo.Catecodi > 0)
            {
                //obtener el concepcodi de la categoria del grupo
                List<PrConceptoDTO> listaConcpOsi = FactorySic.GetPrConceptoRepository().GetByCriteria(ConstantesMigraciones.ConcepcodisCodigoOsinergmin);
                var objConcp = listaConcpOsi.Find(x => x.Catecodi == objGrupo.Catecodi);
                if (objConcp != null)
                    concepcodi = objConcp.Concepcodi;
            }

            if (concepcodi > 0)
            {
                PrGrupodatDTO reg = new PrGrupodatDTO();
                reg.Grupocodi = objGrupo.Grupocodi;
                reg.Formuladat = objGrupo.Osinergcodi;
                reg.Concepcodi = concepcodi;
                reg.Fechadat = DateTime.Today;
                reg.Lastuser = usuario;
                reg.Fechaact = DateTime.Now;
                reg.Deleted = ConstantesMigraciones.GrupodatActivo;

                var servDespacho = new DespachoAppServicio();
                if (esNuevo)
                    servDespacho.SavePrGrupodat(reg);
                else
                {
                    PrGrupodatDTO objDat = servDespacho.GetByIdPrGrupodat(reg.Fechadat.Value, reg.Concepcodi, reg.Grupocodi, ConstantesMigraciones.GrupodatActivo);
                    if (objDat != null)
                    {
                        objDat.Formuladat = reg.Formuladat;
                        objDat.Lastuser = usuario;
                        objDat.Fechaact = DateTime.Now;

                        servDespacho.UpdatePrGrupodat(reg);
                    }
                    else
                    {
                        servDespacho.SavePrGrupodat(reg);
                    }
                }
            }

            //Guardar relacion entre modo de operación y sus generadores
            if ((int)ConstantesMigraciones.Catecodi.ModoOperacionTermico == objGrupo.Catecodi)
            {
                List<PrGrupoeqDTO> listaRelacionEquipoBD = GetByCriteriaPrGrupoeqs(objGrupo.Grupocodi);

                List<PrGrupoeqDTO> listaNuevo = new List<PrGrupoeqDTO>();
                List<PrGrupoeqDTO> listaUpdate = new List<PrGrupoeqDTO>();

                foreach (var equicodi in listaEquicodisActivo)
                {
                    //si no existe en bd entonces es nuevo
                    if (listaRelacionEquipoBD.Find(x => x.Equicodi == equicodi) == null)
                    {
                        listaNuevo.Add(new PrGrupoeqDTO()
                        {
                            Grupocodi = objGrupo.Grupocodi,
                            Equicodi = equicodi,
                            Geqfeccreacion = DateTime.Now,
                            Gequsucreacion = usuario,
                            Geqactivo = 1
                        });
                    }
                }
                foreach (var obj in listaRelacionEquipoBD)
                {
                    if (listaEquicodisActivo.Contains(obj.Equicodi))
                    {
                        //el que estaba inactivo pasa a activo
                        if (obj.Geqactivo == 0)
                        {
                            obj.Geqfecmodificacion = DateTime.Now;
                            obj.Gequsumodificacion = usuario;
                            obj.Geqactivo = 1;
                            listaUpdate.Add(obj);
                        }
                    }
                    else
                    {
                        //si el equipo no forma parte del grupo entonces dar de baja
                        obj.Geqfecmodificacion = DateTime.Now;
                        obj.Gequsumodificacion = usuario;
                        obj.Geqactivo = 0;
                        listaUpdate.Add(obj);
                    }
                }

                foreach (var obj in listaNuevo) SavePrGrupoeq(obj);
                foreach (var obj in listaUpdate) UpdatePrGrupoeq(obj);
            }
        }

        /// <summary>
        /// Obtener todos los generadores termolectricos
        /// </summary>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarEquipoTermoelectrico()
        {
            List<EqEquipoDTO> listaEqBD = FactorySic.GetEqEquipoRepository().ListarEquiposPorFamilia(ConstantesHorasOperacion.IdGeneradorTemoelectrico.ToString());
            listaEqBD = listaEqBD.Where(x => x.Equiestado != ConstantesAppServicio.Eliminado && x.Equiestado != ConstantesAppServicio.Anulado).ToList();

            listaEqBD = listaEqBD.OrderBy(x => x.Equiestado).ThenBy(x => x.Areanomb).ThenBy(x => x.Equiabrev).ToList();

            foreach (var reg in listaEqBD)
            {
                reg.EstadoDesc = EquipamientoHelper.EstadoDescripcion(reg.Equiestado);
            }

            return listaEqBD;
        }

        #endregion

        #region Parámetro de Grupos/Mop

        public void GuardarAgrupacion(int idAgrp, List<int> listaConcepcodi, string usuario, DateTime fecha)
        {
            GuardarAgrupacion(idAgrp, listaConcepcodi, null, usuario, fecha);
        }

        /// <summary>
        /// GuardarAgrupacion
        /// </summary>
        /// <param name="idAgrp"></param>
        /// <param name="listaConcepcodi"></param>
        /// <param name="usuario"></param>
        /// <param name="fecha"></param>
        public void GuardarAgrupacion(int idAgrp, List<int> listaConcepcodi, List<int> listaPropcodi, string usuario, DateTime fecha)
        {
            List<PrAgrupacionConceptoDTO> listaAgrpConcepto = this.GetByCriteriaPrAgrupacionConceptos(-1, idAgrp);
            var listaAgrpCnpSelect = listaAgrpConcepto.Where(x => x.Concepcodi > 0).Where(x => listaConcepcodi.Contains(x.Concepcodi ?? 0)).ToList();
            var listaAgrpCnpNoSelect = listaAgrpConcepto.Where(x => x.Concepcodi > 0).Where(x => !listaConcepcodi.Contains(x.Concepcodi ?? 0)).ToList();
            var listaAgrpPropSelect = listaAgrpConcepto.Where(x => x.Propcodi > 0).Where(x => listaPropcodi.Contains(x.Propcodi ?? 0)).ToList();
            var listaAgrpPropNoSelect = listaAgrpConcepto.Where(x => x.Propcodi > 0).Where(x => !listaPropcodi.Contains(x.Propcodi ?? 0)).ToList();

            //Actualizar
            var lista1 = new List<PrAgrupacionConceptoDTO>();
            lista1.AddRange(listaAgrpCnpSelect);
            lista1.AddRange(listaAgrpPropSelect);
            foreach (var select in lista1)
            {
                select.Agrconactivo = ConstantesMigraciones.Activo;
                select.Agrconfecmodificacion = fecha;
                select.Agrconusumodificacion = usuario;

                this.UpdatePrAgrupacionConcepto(select);
            }

            //Eliminar
            var lista2 = new List<PrAgrupacionConceptoDTO>();
            lista2.AddRange(listaAgrpCnpNoSelect);
            lista2.AddRange(listaAgrpPropNoSelect);
            foreach (var noselect in lista2)
            {
                noselect.Agrconactivo = ConstantesMigraciones.Inactivo;
                noselect.Agrconfecmodificacion = fecha;
                noselect.Agrconusumodificacion = usuario;

                this.UpdatePrAgrupacionConcepto(noselect);
            }

            //Nuevo
            var listaConcepcodiRegistrados = listaAgrpCnpSelect.Select(x => x.Concepcodi).ToList();
            var listaConcepcodiNuevo = listaConcepcodi.Where(x => !listaConcepcodiRegistrados.Contains(x)).Select(x => x).ToList();

            var listaPropcodiRegistrados = listaAgrpPropSelect.Select(x => x.Propcodi).ToList();

            PrAgrupacionConceptoDTO reg;

            //solo si existe lista de propcodis
            if (listaPropcodi != null)
            {
                var listaPropcodiNuevo = listaPropcodi.Where(x => !listaPropcodiRegistrados.Contains(x)).Select(x => x).ToList();

                foreach (var item in listaPropcodiNuevo)
                {
                    reg = new PrAgrupacionConceptoDTO();
                    reg.Agrupcodi = idAgrp;
                    reg.Propcodi = item;
                    reg.Agrconfeccreacion = fecha;
                    reg.Agrconusucreacion = usuario;
                    reg.Agrconactivo = ConstantesMigraciones.Activo;

                    this.SavePrAgrupacionConcepto(reg);
                }
            }

            foreach (var item in listaConcepcodiNuevo)
            {
                reg = new PrAgrupacionConceptoDTO();
                reg.Agrupcodi = idAgrp;
                reg.Concepcodi = item;
                reg.Agrconfeccreacion = fecha;
                reg.Agrconusucreacion = usuario;
                reg.Agrconactivo = ConstantesMigraciones.Activo;

                this.SavePrAgrupacionConcepto(reg);
            }
        }

        /// <summary>
        /// ListarGrupodatGrupoOModoXFiltro
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <param name="fecha"></param>
        /// <param name="unidad"></param>
        /// <param name="idAgrup"></param>
        /// <param name="filtroFicha"></param>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public List<PrGrupodatDTO> ListarGrupodatGrupoOModoXFiltro(int grupocodi, DateTime fecha, string unidad, int idAgrup, string filtroFicha, int usercode)
        {
            var listaData = this.ListaPrGrupoDatByGrupoYFecha(grupocodi, fecha);

            if (filtroFicha == "1")
                listaData = listaData.Where(x => x.Concepfichatec == "S").ToList(); // filtrar solo los que son de ficha técnica

            if (idAgrup != -1)
            {
                var listaAgrupacionConcepto = this.GetByCriteriaPrAgrupacionConceptos(ConstantesMigraciones.Activo, idAgrup);
                var listaConcepcodis = listaAgrupacionConcepto.Select(x => x.Concepcodi).ToList();
                listaData = listaData.Where(x => listaConcepcodis.Contains(x.Concepcodi)).ToList();
            }

            var listaGrupodat = listaData;
            if (usercode > 0)
            {
                List<int> listaconcepcodiAll = this.ListarConcepcodiRegistrados();
                List<int> listaconcepcodiUserActivo = this.ListarConcepcodiByUsuario(usercode.ToString()).Where(x => x.Aconusactivo == ConstantesMigraciones.Activo).Select(x => x.Concepcodi).ToList();
                List<int> listaconcepcodiUserInactivo = this.ListarConcepcodiByUsuario(usercode.ToString()).Where(x => x.Aconusactivo == ConstantesMigraciones.Inactivo).Select(x => x.Concepcodi).ToList();

                List<int> listaconcepcodiInactivo = listaconcepcodiAll.Where(x => !listaconcepcodiUserActivo.Contains(x)).ToList();

                listaGrupodat = listaData.Where(x => !listaconcepcodiInactivo.Contains(x.Concepcodi)).ToList();
            }

            if (ConstantesAppServicio.ParametroDefecto != unidad)
            {
                listaGrupodat = listaGrupodat.Where(x => x.ConcepUni.ToUpper() == unidad.ToUpper()).ToList();
            }

            listaGrupodat = this.OrdenarListaGrupodat(listaGrupodat);

            //formatear orden según ficha maestra 
            List<FtFictecItemDTO> listaFicTemItems = new List<FtFictecItemDTO>();
            if (filtroFicha == "1")
            {
                int catecodi = this.servFictec.GetByIdPrGrupo(grupocodi).Catecodi;
                //Obtiene lista de todos los Item de las ficha maestra
                listaFicTemItems = servDespacho.ObtenerItemFichaXtipoEquipoOficial(catecodi);

                //formatear orden
                if (listaFicTemItems.Any())
                {
                    foreach (var item in listaGrupodat)
                    {
                        var fictecitem = listaFicTemItems.Find(x => x.Concepcodi == item.Concepcodi);
                        item.Conceporden = fictecitem != null ? fictecitem.OrdenNumerico : 999999999;
                    }
                }
            }

            //Orden de acuerdo al filtro de ficha técnica
            listaGrupodat = filtroFicha == "1" ? listaGrupodat.OrderBy(x => x.OrdenCatecodi).ThenBy(x => x.Conceporden).ToList()
                                                : listaGrupodat.OrderBy(x => x.OrdenCatecodi).ThenBy(x => x.Concepcodi).ToList();

            return listaGrupodat;
        }

        /// <summary>
        /// Listar Prgrupodat del modo, grupo, central
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<PrGrupodatDTO> ListaPrGrupoDatByGrupoYFecha(int grupocodi, DateTime fecha)
        {
            PrGrupoDTO grupo = this.servFictec.GetByIdPrGrupo(grupocodi);

            List<int> grupos = new List<int>();
            int grupodespacho = -1;
            int grupocentral = 0;

            //obtener grupos relacionados
            if (grupo.Catecodi == 2 || grupo.Catecodi == 9)  //modos
            {
                grupodespacho = grupo.Grupopadre.GetValueOrDefault(-1);
                grupocentral = grupo.GrupoCentral;
            }
            if (grupo.Catecodi == 3 || grupo.Catecodi == 5) //grupos despacho
            {
                grupodespacho = grupocodi;
                grupocentral = grupo.Grupopadre.GetValueOrDefault(-1);
            }
            if (grupo.Catecodi == 4 || grupo.Catecodi == 6) //centrales
            {
                grupocentral = grupocodi;
            }

            grupos.Add(grupocodi);
            if (grupodespacho > 0) grupos.Add(grupodespacho);
            if (grupocentral > 0) grupos.Add(grupocentral);
            grupos = grupos.Distinct().ToList();

            List<PrGrupodatDTO> lista = FactorySic.GetPrGrupodatRepository().ParametrosConfiguracionPorFecha(fecha, string.Join(",", grupos), "-1");

            //completar con conceptos del grupo que no han tenido valores en prgrupodat
            List<int> listaConcepcodiDat = lista.Select(x => x.Concepcodi).Distinct().ToList();

            string catecodis = grupo.Catecodi.ToString();
            if (grupo.Catecodi == (int)ConstantesMigraciones.Catecodi.ModoOperacionTermico) catecodis += ",3,4";
            if (grupo.Catecodi == (int)ConstantesMigraciones.Catecodi.ModoOperacionHidro) catecodis += ",5,6";
            if (grupo.Catecodi == (int)ConstantesMigraciones.Catecodi.CentralTermico) catecodis += "";
            catecodis += ",0"; //incluir conceptos

            List<PrConceptoDTO> listaConceptoGrupo = this.servFictec.ListPrConceptoByCatecodi(catecodis, false);
            List<int> listaConcepcodiAll = listaConceptoGrupo.Select(x => x.Concepcodi).Distinct().ToList();
            List<int> listaConcepcodiDatFaltante = listaConcepcodiAll.Where(x => !listaConcepcodiDat.Contains(x)).ToList();

            List<PrGrupodatDTO> listaFaltante = new List<PrGrupodatDTO>();
            foreach (var concepcodi in listaConcepcodiDatFaltante)
            {
                if ((grupocodi == 289 || grupocodi == 290 || grupocodi == 291) && concepcodi == 16)
                    continue;

                PrConceptoDTO concepto = listaConceptoGrupo.Find(x => x.Concepcodi == concepcodi);

                int codigoGrupo = grupocodi;
                if (concepto.Catecodi == 3 || concepto.Catecodi == 5) codigoGrupo = grupodespacho;
                if (concepto.Catecodi == 4 || concepto.Catecodi == 6) codigoGrupo = grupocentral;

                PrGrupodatDTO reg = new PrGrupodatDTO();
                reg.Concepcodi = concepcodi;
                reg.ConcepDesc = concepto.Concepdesc;
                reg.Concepnombficha = concepto.Concepnombficha;
                reg.ConcepUni = concepto.Concepunid;
                reg.Concepabrev = concepto.Concepabrev;
                reg.Grupocodi = codigoGrupo;
                reg.Catecodi = concepto.Catecodi;
                reg.Lastuser = "";
                reg.Concepfichatec = concepto.Concepfichatec;

                listaFaltante.Add(reg);
            }

            lista.AddRange(listaFaltante);

            var listaConceptoDuplicado = lista.GroupBy(x => x.Concepcodi).Where(x => x.Count() >= 2).Select(x => x.Key).ToList();

            //Formatear
            foreach (var reg in lista)
            {
                bool tieneConceptoDuplicado = listaConceptoDuplicado.Where(x => x == reg.Concepcodi).Count() >= 1;
                FormatearPrGrupodat(reg);

                if (reg.Grupocodi == grupocodi)
                {
                    reg.ConcepDesc = tieneConceptoDuplicado ? "GRUPO - " + reg.ConcepDesc : reg.ConcepDesc;
                }
                if (reg.Grupocodi == grupodespacho)
                {
                    reg.TipogrupoDesc = "padre";
                    reg.ConcepDesc = tieneConceptoDuplicado ? "GRUPO - " + reg.ConcepDesc : reg.ConcepDesc;
                }
                if (reg.Grupocodi == grupocentral)
                {
                    reg.TipogrupoDesc = "central";
                    reg.ConcepDesc = tieneConceptoDuplicado ? "CENTRAL - " + reg.ConcepDesc : reg.ConcepDesc;
                }

                if (!string.IsNullOrEmpty(reg.Gdatsustento))
                    reg.EsSustentoConfidencial = reg.Gdatsustento.ToUpper().Contains("DescargarSustentoConfidencial?".ToUpper());

                var regCnp = listaConceptoGrupo.Find(x => x.Concepcodi == reg.Concepcodi);
                if (regCnp != null) reg.Catecodi = regCnp.Catecodi;

                //setear color y orden
                reg.Color = string.Empty;
                reg.OrdenCatecodi = 0;
                if (reg.Catecodi == 0)
                {
                    reg.OrdenCatecodi = -1;
                }
                if ((new List<int>() { 3, 5 }).Contains(reg.Catecodi))
                {
                    reg.Color = ConstantesFichaTecnica.ColorCategoriaGrupo;
                    reg.OrdenCatecodi = 1;
                }
                if ((new List<int>() { 4, 6 }).Contains(reg.Catecodi))
                {
                    reg.Color = ConstantesFichaTecnica.ColorCategoriaCentral;
                    reg.OrdenCatecodi = 2;
                }
            }

            return lista;
        }

        public static void FormatearPrGrupodat(PrGrupodatDTO reg)
        {
            reg.ConcepDesc = reg.ConcepDesc != null ? reg.ConcepDesc : string.Empty;
            reg.Concepnombficha = reg.Concepnombficha != null ? reg.Concepnombficha : string.Empty;
            reg.Concepfichatec = reg.Concepfichatec != null ? reg.Concepfichatec : string.Empty;
            reg.Gdatcomentario = reg.Gdatcomentario != null ? reg.Gdatcomentario : string.Empty;
            reg.Gdatsustento = reg.Gdatsustento != null ? reg.Gdatsustento : string.Empty;
            reg.Gdatcheckcero = reg.Gdatcheckcero ?? 0;
            reg.ConcepUni = reg.ConcepUni != null ? reg.ConcepUni : string.Empty;
            reg.Concepabrev = reg.Concepabrev != null ? reg.Concepabrev : string.Empty;
            reg.FechadatDesc = reg.Fechadat != null ? reg.Fechadat.Value.ToString(ConstantesAppServicio.FormatoFecha) : string.Empty;
            reg.Lastuser = reg.Lastuser != null ? reg.Lastuser.Trim() : "SISTEMA";
            reg.FechaactDesc = reg.Fechaact != null ? reg.Fechaact.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
            reg.Formuladat = reg.Formuladat ?? "";
        }

        /// <summary>
        /// Listar historico de prgrupodat
        /// </summary>
        /// <param name="concepcodi"></param>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        public List<PrGrupodatDTO> ListarGrupodatHistoricoValores(int concepcodi, int grupocodi)
        {
            List<PrGrupodatDTO> lista = FactorySic.GetPrGrupodatRepository().ListarHistoricoValores(concepcodi.ToString(), grupocodi)
                                                    .OrderByDescending(x => x.Fechadat).ThenBy(x => (x.Deleted == 0 ? 1 : 2)).ThenByDescending(x => x.Fechaact).ToList();

            foreach (var reg in lista)
            {
                reg.FechadatDesc = reg.Fechadat != null ? reg.Fechadat.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
                reg.FechaactDesc = reg.Fechaact != null ? reg.Fechaact.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
                reg.EstadoDesc = reg.Deleted == ConstantesMigraciones.GrupodatActivo ? ConstantesAppServicio.ActivoDesc : ConstantesAppServicio.BajaDesc;
                reg.Lastuser = reg.Lastuser != null ? reg.Lastuser.Trim() : "SISTEMA";
                reg.Formuladat = reg.Formuladat ?? "";
                reg.Gdatcomentario = reg.Gdatcomentario ?? "";
                reg.Gdatsustento = reg.Gdatsustento ?? "";

                if (!string.IsNullOrEmpty(reg.Gdatsustento))
                    reg.EsSustentoConfidencial = reg.Gdatsustento.ToUpper().Contains("DescargarSustentoConfidencial?".ToUpper());

            }

            return lista;
        }

        /// <summary>
        /// Listar historico de grupoequipoval
        /// </summary>
        /// <param name="concepcodi"></param>
        /// <param name="equicodi"></param>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        public List<PrGrupoEquipoValDTO> ListarGrupoEquipoValHistoricoValores(int concepcodi, int equicodi, int grupocodi)
        {
            List<PrGrupoEquipoValDTO> lista = FactorySic.GetPrGrupoEquipoValRepository().ListarHistoricoValores(concepcodi.ToString(), equicodi.ToString(), grupocodi.ToString());

            foreach (var reg in lista)
            {
                reg.GreqvafechadatDesc = reg.Greqvafechadat.ToString(ConstantesAppServicio.FormatoFechaFull2);
                string fecnuevo = reg.Greqvafeccreacion != null ? reg.Greqvafeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
                string fecmodif = reg.Greqvafecmodificacion != null ? reg.Greqvafecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
                reg.FechaactDesc = reg.Greqvafecmodificacion != null ? fecmodif : fecnuevo;
                reg.Lastuser = reg.Greqvafecmodificacion != null ? reg.Greqvausumodificacion : reg.Greqvausucreacion;
                reg.Lastuser = reg.Lastuser != null ? reg.Lastuser : "SISTEMA";
                reg.EstadoDesc = reg.Greqvadeleted == ConstantesMigraciones.GrupodatActivo ? ConstantesAppServicio.ActivoDesc : ConstantesAppServicio.BajaDesc;
                reg.Greqvaformuladat = reg.Greqvaformuladat ?? "";
                reg.Greqvacomentario = reg.Greqvacomentario ?? "";
                reg.Greqvasustento = reg.Greqvasustento ?? "";

                if (!string.IsNullOrEmpty(reg.Greqvasustento))
                    reg.EsSustentoConfidencial = reg.Greqvasustento.ToUpper().Contains("DescargarSustentoConfidencial?".ToUpper());
            }

            return lista;
        }

        /// <summary>
        /// Listar las unidades de medida por grupo
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<string> ListaUnidadByGrupoYFecha(int grupocodi, DateTime fecha)
        {
            List<PrGrupodatDTO> lista = this.ListaPrGrupoDatByGrupoYFecha(grupocodi, fecha);
            List<string> listaUnidad = lista.Where(x => x.ConcepUni != null && x.ConcepUni.Trim() != string.Empty).GroupBy(x => x.ConcepUni.Trim().ToUpper()).Select(x => x.Key).OrderBy(x => x).ToList();

            return listaUnidad;
        }

        /// <summary>
        /// Listar areas
        /// </summary>
        /// <param name="areacodis"></param>
        /// <returns></returns>
        public List<FwAreaDTO> ListarAreaByListacodi(List<int> areacodis)
        {
            var lista = FactorySic.GetAreaRepository().ListarArea();
            lista = lista.Where(x => areacodis.Contains(x.Areacode)).ToList();

            foreach (var reg in lista)
            {
                reg.Areaname = reg.Areaname != null ? reg.Areaname.Trim() : string.Empty;
                reg.Areaabrev = reg.Areaabrev != null ? reg.Areaabrev.Trim() : string.Empty;
            }

            return lista;
        }

        /// <summary>
        /// Guardar la  configuracion del concepto
        /// </summary>
        /// <param name="concepcodi"></param>
        /// <param name="inlistaUsuariocodi"></param>
        /// <param name="usuario"></param>
        /// <param name="listaUsuario"></param>
        public void GuardarConfiguracionAreaConceptoUser(int concepcodi, List<int> inlistaUsuariocodi, string usuario, List<UsuarioParametro> listaUsuario)
        {
            DateTime fecha = DateTime.Now;

            //input
            List<UsuarioParametro> listaUsuarioSelect = listaUsuario.Where(x => inlistaUsuariocodi.Contains(x.UserCode)).ToList();
            List<int> listaUsuariocodi = listaUsuarioSelect.Select(x => x.UserCode).ToList();
            List<int> listaAreacodi = listaUsuarioSelect.Select(x => x.AreaCode).Distinct().ToList();

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// PrAreaConceptoDTO
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //bd
            List<PrAreaConceptoDTO> listaAreaXConcepto = this.GetByCriteriaPrAreaConceptos(concepcodi, ConstantesAppServicio.ParametroDefecto);
            var listaAreaSelect = listaAreaXConcepto.Where(x => listaAreacodi.Contains(x.Areacode)).ToList();
            var listaAreaNoSelect = listaAreaXConcepto.Where(x => !listaAreacodi.Contains(x.Areacode)).ToList();

            //Actualizar
            foreach (var select in listaAreaSelect)
            {
                select.Arconactivo = ConstantesMigraciones.Activo;
                select.Arconfecmodificacion = fecha;
                select.Arconusumodificacion = usuario;

                this.UpdatePrAreaConcepto(select);
            }

            //Eliminar
            foreach (var noselect in listaAreaNoSelect)
            {
                noselect.Arconactivo = ConstantesMigraciones.Inactivo;
                noselect.Arconfecmodificacion = fecha;
                noselect.Arconusumodificacion = usuario;

                this.UpdatePrAreaConcepto(noselect);
            }

            //Nuevo a partir de la lista de usuarios seleccionados
            var listaAreaRegistrados = listaAreaSelect.Select(x => x.Areacode).ToList();
            var listaAreacodiNuevo = listaAreacodi.Where(x => !listaAreaRegistrados.Contains(x)).Select(x => x).ToList();
            foreach (var areacodi in listaAreacodiNuevo)
            {
                PrAreaConceptoDTO reg = new PrAreaConceptoDTO();
                reg.Concepcodi = concepcodi;
                reg.Areacode = areacodi;
                reg.Arconfeccreacion = fecha;
                reg.Arconusucreacion = usuario;
                reg.Arconactivo = ConstantesMigraciones.Activo;

                this.SavePrAreaConcepto(reg);
            }

            //Nuevo (cuando se registra sin ningun usuario)
            if (listaAreaXConcepto.Count == 0 && listaAreacodiNuevo.Count == 0)
            {
                List<int> areasCoes = ConstantesMigraciones.AreacoesParaVisualizacion.Split(',').Select(x => int.Parse(x)).ToList();
                var listaArea = this.ListarAreaByListacodi(areasCoes);
                foreach (var area in listaArea)
                {
                    PrAreaConceptoDTO reg = new PrAreaConceptoDTO();
                    reg.Concepcodi = concepcodi;
                    reg.Areacode = area.Areacode;
                    reg.Arconfeccreacion = fecha;
                    reg.Arconusucreacion = usuario;
                    reg.Arconactivo = ConstantesMigraciones.Inactivo;

                    this.SavePrAreaConcepto(reg);
                }
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// PrAreaConceptoDTO
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //bd
            listaAreaXConcepto = this.GetByCriteriaPrAreaConceptos(concepcodi, ConstantesAppServicio.ParametroDefecto);
            List<PrAreaConcepUserDTO> listaAreaUserXConcepto = this.GetByCriteriaPrAreaConcepUsers(concepcodi, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto);
            var listaAreaUserSelect = listaAreaUserXConcepto.Where(x => listaUsuariocodi.Contains(x.Usercode)).ToList();
            var listaAreauserNoSelect = listaAreaUserXConcepto.Where(x => !listaUsuariocodi.Contains(x.Usercode)).ToList();

            //Actualizar
            foreach (var select in listaAreaUserSelect)
            {
                select.Aconusactivo = ConstantesMigraciones.Activo;
                select.Aconusfecmodificacion = fecha;
                select.Aconususumodificacion = usuario;
                select.Arconcodi = this.GetArconcodiFromUsurioServicio(select.Usercode, listaAreaXConcepto, listaUsuario);

                this.UpdatePrAreaConcepUser(select);
            }

            //Eliminar
            foreach (var noselect in listaAreauserNoSelect)
            {
                noselect.Aconusactivo = ConstantesMigraciones.Inactivo;
                noselect.Aconusfecmodificacion = fecha;
                noselect.Aconususumodificacion = usuario;
                noselect.Arconcodi = this.GetArconcodiFromUsurioServicio(noselect.Usercode, listaAreaXConcepto, listaUsuario);

                this.UpdatePrAreaConcepUser(noselect);
            }

            //Nuevo
            var listaAreaUserRegistrados = listaAreaUserSelect.Select(x => x.Usercode).ToList();
            var listaAreaUsercodiNuevo = listaUsuariocodi.Where(x => !listaAreaUserRegistrados.Contains(x)).Select(x => x).ToList();
            foreach (var usercodi in listaAreaUsercodiNuevo)
            {
                PrAreaConcepUserDTO reg = new PrAreaConcepUserDTO();
                reg.Usercode = usercodi;
                reg.Arconcodi = this.GetArconcodiFromUsurioServicio(usercodi, listaAreaXConcepto, listaUsuario);
                reg.Aconusfeccreacion = fecha;
                reg.Aconususucreacion = usuario;
                reg.Aconusactivo = ConstantesMigraciones.Activo;

                this.SavePrAreaConcepUser(reg);
            }
        }



        /// <summary>
        /// Obtener codigo del area
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="listaAreaXConcepto"></param>
        /// <param name="listaUsuario"></param>
        /// <returns></returns>
        private int GetArconcodiFromUsurioServicio(int usercode, List<PrAreaConceptoDTO> listaAreaXConcepto, List<UsuarioParametro> listaUsuario)
        {
            UsuarioParametro u = listaUsuario.Find(x => x.UserCode == usercode);
            if (u != null)
            {
                PrAreaConceptoDTO area = listaAreaXConcepto.Find(x => x.Areacode == u.AreaCode);
                return area != null ? area.Arconcodi : -1;
            }

            return -1;
        }

        /// <summary>
        /// Listar conceptos por usuario
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public List<PrAreaConcepUserDTO> ListarConcepcodiByUsuario(string usercode)
        {
            return FactorySic.GetPrAreaConcepUserRepository().ListarConcepcodiByUsercode(usercode).ToList();
        }

        /// <summary>
        /// Listar conceptos registrados en el sistema
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public List<int> ListarConcepcodiRegistrados()
        {
            return FactorySic.GetPrAreaConceptoRepository().ListarConcepcodiRegistrados();
        }

        /// <summary>
        /// Permite listar los conceptos de las categoria de grupo
        /// </summary>
        /// <param name="catecodi"></param>
        /// <returns></returns>
        public List<PrConceptoDTO> ListConceptoYConfiguracionVisualizacion(string catecodi)
        {
            List<PrConceptoDTO> l = this.servFictec.ListPrConceptoByCatecodi(catecodi, false);
            List<int> listaconcepcodiAll = this.ListarConcepcodiRegistrados();

            foreach (var reg in l)
            {
                reg.ConfiguradoVisualizacionDesc = listaconcepcodiAll.Contains(reg.Concepcodi) ? ConstantesAppServicio.SIDesc : ConstantesAppServicio.NODesc;
            }

            return l;
        }

        /// <summary>
        /// Ordenación de Grupodat
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public List<PrGrupodatDTO> OrdenarListaGrupodat(List<PrGrupodatDTO> lista)
        {
            lista = lista.OrderBy(x => x.Concepcodi).ToList();

            List<PrGrupodatDTO> listaOrd = new List<PrGrupodatDTO>();

            if (lista.Find(x => x.Catecodi == 9) != null) //hidro
            {
                var lista9 = lista.Where(x => x.Catecodi == 9);//modo
                var lista5 = lista.Where(x => x.Catecodi == 5);//grupo
                var lista6 = lista.Where(x => x.Catecodi == 6);//central
                var listaOtro = lista.Where(x => x.Catecodi != 5 && x.Catecodi != 6 && x.Catecodi != 9).ToList();

                listaOrd.AddRange(listaOtro);
                listaOrd.AddRange(lista9);
                listaOrd.AddRange(lista5);
                listaOrd.AddRange(lista6);

                return listaOrd;
            }

            if (lista.Find(x => x.Catecodi == 2) != null) //termo
            {
                var lista2 = lista.Where(x => x.Catecodi == 2);//modo
                var lista3 = lista.Where(x => x.Catecodi == 3);//grupo
                var lista4 = lista.Where(x => x.Catecodi == 4);//central
                var listaOtro = lista.Where(x => x.Catecodi != 2 && x.Catecodi != 3 && x.Catecodi != 4).ToList();

                listaOrd.AddRange(lista2);
                listaOrd.AddRange(lista3);
                listaOrd.AddRange(lista4);
                listaOrd.AddRange(listaOtro);

                return listaOrd;
            }

            if (lista.Find(x => x.Catecodi == 11) != null) //caldero
            {
                var lista11 = lista.Where(x => x.Catecodi == 11);//caldero
                var lista4 = lista.Where(x => x.Catecodi == 4);//central
                var listaOtro = lista.Where(x => x.Catecodi != 4 && x.Catecodi != 11).ToList();

                listaOrd.AddRange(lista4);
                listaOrd.AddRange(lista11);
                listaOrd.AddRange(listaOtro);

                return listaOrd;
            }

            return lista;
        }

        #region EXPORTAR / IMPORTAR

        /// <summary>
        /// Generar Excel de reporte plantilla ParametrosMop
        /// </summary>
        /// <param name="listaConceptosParametros"></param>
        /// <param name="grupo"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="repcodi"></param>
        public void GenerarExcelPlantillaParametros(List<PrGrupodatDTO> listaConceptosParametros, PrGrupoDTO grupo, string path, string fileName, int? repcodi)
        {
            try
            {
                string file = path + fileName;

                FileInfo fi = new FileInfo(file);
                // Revisar si existe
                if (!fi.Exists)
                {
                    throw new Exception("Archivo " + file + "No existe");
                }

                using (ExcelPackage xlPackage = new ExcelPackage(fi))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets[ConstantesFichaTecnica.HojaPlantillaExcel];
                    //ExcelWorksheet ws = xlPackage.Workbook.Worksheets.First();

                    int row = 6;
                    //int numeroItem = 1;
                    int columnCodigo = 2;
                    int columnConcepto = columnCodigo + 1;
                    int columnNombFicha = columnConcepto + 1;
                    int columnAbrev = columnNombFicha + 1;
                    int columnUnidad = columnAbrev + 1;
                    int columFecVigen = columnUnidad + 1;
                    int columnFormula = columFecVigen + 1;
                    int columValCero = columnFormula + 1;
                    int columComentario = columValCero + 1;
                    int columSustento = columComentario + 1;
                    int columUsuario = columSustento + 1;
                    int columFecMod = columUsuario + 1;
                    //int columnLong2 = columnLong1 + 1;

                    ws.Cells[2, columnCodigo - 1].Value = "CATEGORÍA DE GRUPO";
                    ws.Cells[3, columnCodigo - 1].Value = "MODO DE OPERACIÓN";
                    ws.Cells[4, columnCodigo - 1].Value = "CÓDIGO DE GRUPO";
                    ws.Cells[2, columnCodigo - 1, 4, columnCodigo - 1].Style.Font.Bold = true;

                    var categoria = FactorySic.GetPrCategoriaRepository().GetById(grupo.Catecodi);
                    ws.Cells[2, columnCodigo + 1].Value = categoria.Catenomb != null ? categoria.Catenomb.Trim() : string.Empty;
                    ws.Cells[3, columnCodigo + 1].Value = (grupo.GruponombPadre ?? "").Trim() + " " + (grupo.Gruponomb ?? "").Trim();
                    //PONER EL GRUPOCODI Y EL REPCODI QUE SERVIRÁ PARA LA IMPORTACIÓN
                    ws.Cells[4, columnCodigo + 1].Value = grupo.Grupocodi;
                    ws.Cells[2, columnCodigo + 1, 4, columnCodigo + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    if (repcodi != null)
                    {
                        PrRepcvDTO oRepCv = servDespacho.GetByIdPrRepcv(repcodi.Value);
                        //PONER NOMBRE EVENTO FECHA Y REPCODI 
                        ws.Cells[2, 5].Value = "EVENTO COSTO VARIABLE";
                        ws.Cells[3, 5].Value = "FECHA";
                        ws.Cells[4, 5].Value = "CÓDIGO DE EVENTO";
                        ws.Cells[2, 5, 4, 5].Style.Font.Bold = true;
                        ws.Cells[2, 7].Value = (oRepCv.Repnomb ?? "").Trim();
                        ws.Cells[3, 7].Value = oRepCv.Repfecha.ToString(ConstantesAppServicio.FormatoFecha);
                        ws.Cells[4, 7].Value = repcodi;
                        ws.Cells[2, 2, 4, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }

                    ws.Cells[5, 7].Value = "FECHA DE VIGENCIA (DD/MM/YYYY)";
                    ws.Cells[5, 7].Style.WrapText = true;


                    foreach (var item in listaConceptosParametros)
                    {
                        //ws.Cells[row, columnNroItem].Value = numeroItem;
                        ws.Cells[row, columnCodigo].Value = item.Concepcodi;
                        ws.Cells[row, columnConcepto].Value = item.ConcepDesc;
                        ws.Cells[row, columnNombFicha].Value = item.Concepnombficha;
                        ws.Cells[row, columnAbrev].Value = item.Concepabrev;
                        ws.Cells[row, columnUnidad].Value = item.ConcepUni;
                        //ws.Cells[row, columFecVigen].Style.Numberformat.Format = "dd/MM/yyyy";
                        ws.Cells[row, columFecVigen].Value = item.Fechadat != null ? item.Fechadat.Value.Date.ToString(ConstantesAppServicio.FormatoFecha) : "";
                        ws.Cells[row, columnFormula].Value = item.Formuladat;
                        ws.Cells[row, columValCero].Value = item.Formuladat == "0" ? item.Gdatcheckcero == ConstantesFichaTecnica.ValorSi ? "SI" : "NO" : null;
                        ws.Cells[row, columComentario].Value = item.Gdatcomentario;
                        ws.Cells[row, columSustento].Value = item.Gdatsustento;
                        ws.Cells[row, columUsuario].Value = item.Lastuser;
                        ws.Cells[row, columFecMod].Value = item.FechaactDesc;

                        UtilExcel.BorderCeldasLineaDelgada(ws, row, columnCodigo, row, columFecMod, "#000000", true);

                        row++;
                        //numeroItem++;
                    }

                    var colorColumnas = System.Drawing.ColorTranslator.FromHtml("#e1e2e3");
                    ws.Cells[6, columnCodigo, row - 1, columnUnidad].SetBackgroundColor(colorColumnas);
                    ws.Cells[6, columUsuario, row - 1, columFecMod].SetBackgroundColor(colorColumnas);

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Metodo para la validacion de los datos al importar carga masiva parámetros Mop
        /// </summary>
        public void ValidarParametrosMasivoAImportar(string path, string fileName, string strUsuario,
                                               out List<PrGrupodatDTO> lstRegparametrosCorrectos,
                                               out List<PrGrupodatDTO> lstRegParametrosErroneos,
                                               out List<PrGrupodatDTO> listaNuevo,
                                               out List<PrGrupodatDTO> listaModificado)
        {
            //Validación de archivo
            string extension = (System.IO.Path.GetExtension(fileName) ?? "").ToLower();

            List<string> lExtensionPermitido = new List<string>() { ".xlsm", ".xlsx" }; // corregir
            if (!lExtensionPermitido.Contains(extension))
            {
                throw new ArgumentException("Está cargando un archivo de extensión no permitida. Debe ingresar un archivo " + string.Join(", ", lExtensionPermitido));
            }

            DateTime fechaRegistro = DateTime.Now;

            #region Archivo xlsx

            string savePath = path + fileName;
            List<FilaExcelParametrosGrupoMop> listaFilaMacro = ImportToDataTableParametrosGrupMop(savePath);
            ValidarRegistrosMacro(fechaRegistro, listaFilaMacro, strUsuario, out lstRegparametrosCorrectos, out lstRegParametrosErroneos, out listaNuevo, out listaModificado);

            #endregion
        }

        public void ValidarRegistrosMacro(DateTime fechaRegistro, List<FilaExcelParametrosGrupoMop> listaFilaMacro, string strUsuario, out List<PrGrupodatDTO> lstRegparametrosCorrectos, out List<PrGrupodatDTO> lstRegParametrosErroneos, out List<PrGrupodatDTO> listaNuevo, out List<PrGrupodatDTO> listaModificado)
        {

            //Validación de registros macro
            lstRegparametrosCorrectos = new List<PrGrupodatDTO>();
            lstRegParametrosErroneos = new List<PrGrupodatDTO>();

            listaNuevo = new List<PrGrupodatDTO>();
            listaModificado = new List<PrGrupodatDTO>();

            #region Obtener grupos (modo de operación es un caso especial)

            var objFilaGrupo = listaFilaMacro.FirstOrDefault(x => x.Grupocodi > 0);
            if (objFilaGrupo != null)
            {
                int grupocodi = objFilaGrupo.Grupocodi;
                DateTime fecha = DateTime.Today;
                if (objFilaGrupo.Repcodi != null)
                {
                    PrRepcvDTO oRepCv = servDespacho.GetByIdPrRepcv(objFilaGrupo.Repcodi.Value);
                    fecha = oRepCv.Repfecha.Date;
                }

                //obtener la plantilla 
                var listaGrupodat = ListarGrupodatGrupoOModoXFiltro(grupocodi, fecha, "-1", -1, "-1", 0);

                foreach (var regFila in listaFilaMacro)
                {
                    var objDat = listaGrupodat.Find(x => x.Concepcodi == regFila.Concepcodi);
                    if (objDat != null) regFila.Grupocodi = objDat.Grupocodi;
                }
            }
            else
            {
                throw new ArgumentException("No existe código de grupo en la plantilla.");
            }

            #endregion

            foreach (var regFila in listaFilaMacro)
            {
                if (regFila.Concepcodi == 80)
                { }
                //Validaciones al leer la macro (comprobar si los datos del excel son validos)
                string mensajeValidacion = this.ValidarLecturaGrupoDatExcel(regFila);

                PrGrupodatDTO entity = new PrGrupodatDTO();
                entity.NroItem = regFila.Row;
                entity.Repcodi = regFila.Repcodi; //  Repcodi
                entity.Grupocodi = regFila.Grupocodi; // Código de grupo (puede ser del modo, despacho o central en un mismo archivo)
                entity.Concepcodi = regFila.Concepcodi; // Código de concepto 
                entity.Fechadat = regFila.Fechadat; // fecha
                entity.FechadatDesc = regFila.Fechadat != null ? regFila.Fechadat.Value.ToString(ConstantesAppServicio.FormatoFecha) : ""; // fecha
                entity.Formuladat = (regFila.Formuladat ?? "").Trim();
                entity.Gdatcomentario = regFila.Gdatcomentario != null ? regFila.Gdatcomentario.Trim() : string.Empty;
                entity.Gdatsustento = regFila.Gdatsustento != null ? regFila.Gdatsustento.Trim() : string.Empty;

                //Capturar valor correcto según el valor
                if (entity.Formuladat == "0")
                {
                    entity.Gdatcheckcero = regFila.StrGdatcheckcero == "SI" ? ConstantesFichaTecnica.ValorSi : ConstantesFichaTecnica.ValorNo;
                    entity.GdatcheckceroDesc = regFila.StrGdatcheckcero;
                }
                else
                    entity.Gdatcheckcero = ConstantesFichaTecnica.ValorNo;

                //nuevos registros
                entity.Lastuser = strUsuario; // Usuario de creacion del registro
                entity.Fechaact = fechaRegistro; // Fecha de creacion del registro

                // Si los datos son correctos VALIDO LA INFORMACION DE ACUERDO AL NEGOCIO
                if (mensajeValidacion == "")
                {
                    //Validar parámetros según la fecha de vigencia
                    if (entity.Fechadat != null && entity.Formuladat != null && entity.Formuladat.Trim() != "")
                    {
                        bool esFechaCVDistinto = false;
                        string msjFechaCVDistinto = "";
                        if (entity.Repcodi != null)
                        {
                            PrRepcvDTO oRepCv = servDespacho.GetByIdPrRepcv(entity.Repcodi.Value);
                            DateTime fecha = oRepCv.Repfecha.Date;

                            if (entity.Fechadat.Value.Date != fecha)
                            {
                                esFechaCVDistinto = true;
                                msjFechaCVDistinto = "La Fecha de Vigencia no es válida. Debe ser igual a: " + fecha.ToString(ConstantesAppServicio.FormatoFechaFull2);
                            }
                        }

                        // Lista histórico para obtener el último vigente hasta el día que quiero registrar 
                        List<PrGrupodatDTO> listaDat = FactorySic.GetPrGrupodatRepository().ListarHistoricoValores(entity.Concepcodi.ToString(), entity.Grupocodi);
                        List<PrGrupodatDTO> listaHistXCnp = listaDat.Where(x => x.Fechadat <= entity.Fechadat).OrderByDescending(x => x.Fechadat).ToList();
                        //obtener activo
                        PrGrupodatDTO regActivoHist = listaHistXCnp.Find(x => x.Deleted == 0);

                        if (entity.Fechadat >= DateTime.Today.AddDays(1))
                        {
                            if (regActivoHist != null && regActivoHist.Fechadat == entity.Fechadat)
                            {
                                bool existeDif = (entity.Formuladat != regActivoHist.Formuladat); // solo si se cambia el valor se considera que hay diferencia

                                if (existeDif)
                                {
                                    PrGrupodatDTO regActivo = (PrGrupodatDTO)regActivoHist.Clone();
                                    regActivo.Formuladat = entity.Formuladat;
                                    regActivo.Gdatcheckcero = entity.Gdatcheckcero;
                                    regActivo.Gdatcomentario = entity.Gdatcomentario;
                                    regActivo.Gdatsustento = entity.Gdatsustento;
                                    regActivo.Deleted = 0;
                                    regActivo.Deleted2 = 0;
                                    regActivo.Fechaact = entity.Fechaact;
                                    regActivo.Lastuser = entity.Lastuser;
                                    listaModificado.Add(regActivo);

                                    regActivoHist.Deleted = listaHistXCnp.Max(x => x.Deleted) + 1;
                                    listaNuevo.Add(regActivoHist);

                                    if (esFechaCVDistinto) //validacion CV
                                    {
                                        entity.Observaciones = msjFechaCVDistinto;
                                        lstRegParametrosErroneos.Add(entity);
                                    }
                                }
                                else
                                {
                                    //si hay cambios en columnas opcionales
                                    if (entity.Gdatcheckcero != regActivoHist.Gdatcheckcero || entity.Gdatcomentario != (regActivoHist.Gdatcomentario ?? "") || entity.Gdatsustento != (regActivoHist.Gdatsustento ?? ""))
                                    {
                                        PrGrupodatDTO regActivo = (PrGrupodatDTO)regActivoHist.Clone();
                                        regActivo.Gdatcheckcero = entity.Gdatcheckcero;
                                        regActivo.Gdatcomentario = entity.Gdatcomentario;
                                        regActivo.Gdatsustento = entity.Gdatsustento;
                                        regActivo.Deleted2 = regActivo.Deleted;
                                        listaModificado.Add(regActivo);
                                    }
                                }
                            }
                            else
                            {
                                entity.Deleted = 0;
                                listaNuevo.Add(entity);
                            }
                        }
                        else
                        {
                            bool existeModificacionPasado = true;
                            string msjModificacionPasado = "Fecha de vigencia: Se está registrando / editando información con fecha de vigencia actual o anterior.";

                            if (regActivoHist != null && regActivoHist.Fechadat == entity.Fechadat)
                            {
                                existeModificacionPasado = entity.Formuladat != regActivoHist.Formuladat;
                                if (existeModificacionPasado) msjModificacionPasado = "Valor: Se está registrando / editando información con fecha de vigencia actual o anterior.";

                                //SI CAMBIA EL VALOR NO SE TOMARÁ EN CUENTA POR NO SER FECHA FUTURA
                                //Solo si hay cambios en columnas opcionales se actualizará esos campos
                                //si hay cambios en columnas opcionales
                                if (entity.Gdatcheckcero != regActivoHist.Gdatcheckcero
                                    || entity.Gdatcomentario != (regActivoHist.Gdatcomentario ?? string.Empty)
                                    || entity.Gdatsustento != (regActivoHist.Gdatsustento ?? string.Empty))
                                {
                                    existeModificacionPasado = false;

                                    PrGrupodatDTO regActivo = (PrGrupodatDTO)regActivoHist.Clone();
                                    regActivo.Gdatcheckcero = entity.Gdatcheckcero;
                                    regActivo.Gdatcomentario = entity.Gdatcomentario;
                                    regActivo.Gdatsustento = entity.Gdatsustento;
                                    regActivo.Deleted2 = regActivo.Deleted;
                                    listaModificado.Add(regActivo);
                                }
                            }

                            if (existeModificacionPasado)
                            {
                                // Van los registros con formato incorrecto
                                entity.Observaciones = msjModificacionPasado + (esFechaCVDistinto ? " " + msjFechaCVDistinto : "");
                                lstRegParametrosErroneos.Add(entity);
                            }
                        }
                    }
                }
                else
                {
                    // Van los registros con formato incorrecto
                    entity.Observaciones = mensajeValidacion;

                    lstRegParametrosErroneos.Add(entity);
                }
            }

            lstRegparametrosCorrectos.AddRange(listaNuevo);
            lstRegparametrosCorrectos.AddRange(listaModificado);
        }

        /// <summary>
        /// Importa registros de un DataTable
        /// </summary>
        /// <param name="filePath">Directorio de archivos</param>  
        /// <returns>devuelve una cadena</returns>
        public static List<FilaExcelParametrosGrupoMop> ImportToDataTableParametrosGrupMop(string filePath)
        {
            List<FilaExcelParametrosGrupoMop> listaMacro = new List<FilaExcelParametrosGrupoMop>();

            // Check if the file exists
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Exists)
            {
                throw new Exception("File " + filePath + " Does Not Exists");
            }

            //int indexItem = 1;
            int indexCodConcepto = 2;
            int indexNomb = indexCodConcepto + 1;
            int indexNombFicha = indexNomb + 1;
            int indexAbrev = indexNombFicha + 1;
            int indexUnidad = indexAbrev + 1;
            int indexFecVigencia = indexUnidad + 1;
            int indexFormula = indexFecVigencia + 1;
            int indexValorCeroCorrecto = indexFormula + 1;
            int indexComentario = indexValorCeroCorrecto + 1;
            int indexSustento = indexComentario + 1;
            int iColumnaCodGrupo = 3;
            int iColumnaRepcodi = 7;

            using (ExcelPackage xlPackage = new ExcelPackage(fi))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[ConstantesFichaTecnica.HojaPlantillaExcel];
                //ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.First();

                // Fetch the WorkSheet size
                ExcelCellAddress startCell = worksheet.Dimension.Start;
                ExcelCellAddress endCell = worksheet.Dimension.End;

                int rowStart = 6;
                for (int row = rowStart; row <= endCell.Row; row++)
                {
                    var codigoConcepto = worksheet.Cells[row, indexCodConcepto].Value.ToString();
                    if (string.IsNullOrEmpty(codigoConcepto))
                    {
                        continue;
                    }

                    var sFechVigencia = string.Empty;
                    if (worksheet.Cells[row, indexFecVigencia].Value != null) sFechVigencia = worksheet.Cells[row, indexFecVigencia].Value.ToString();

                    var sValor = string.Empty;
                    if (worksheet.Cells[row, indexFormula].Value != null) sValor = worksheet.Cells[row, indexFormula].Value.ToString();

                    //si no tiene fecha o valor no se considera
                    if (string.IsNullOrEmpty(sFechVigencia) || string.IsNullOrEmpty(sValor))
                    {
                        continue;
                    }

                    var sValCeroCorrecto = string.Empty;
                    if (worksheet.Cells[row, indexValorCeroCorrecto].Value != null) sValCeroCorrecto = worksheet.Cells[row, indexValorCeroCorrecto].Value.ToString();

                    var sComentario = string.Empty;
                    if (worksheet.Cells[row, indexComentario].Value != null) sComentario = worksheet.Cells[row, indexComentario].Value.ToString();

                    var sSustento = string.Empty;
                    if (worksheet.Cells[row, indexSustento].Value != null) sSustento = worksheet.Cells[row, indexSustento].Value.ToString();

                    int concepcodi = 0;
                    int grupocodi = 0;
                    int? repcodi = null;
                    DateTime? fechaVigencia = null;
                    try
                    {
                        sFechVigencia = (sFechVigencia ?? "").Trim();
                        sValor = (sValor ?? "").Trim();
                        sValCeroCorrecto = (sValCeroCorrecto ?? "").Trim();
                        sComentario = (sComentario ?? "").Trim();
                        sSustento = (sSustento ?? "").Trim();
                        grupocodi = (int)(((double?)worksheet.Cells[4, iColumnaCodGrupo].Value));
                        repcodi = (int?)(((double?)worksheet.Cells[4, iColumnaRepcodi].Value));
                        concepcodi = (int)(((double?)worksheet.Cells[row, indexCodConcepto].Value));

                        // convertir a datetime
                        if (sFechVigencia != "")
                        {
                            var objFec = worksheet.Cells[row, indexFecVigencia].Value;
                            if (objFec is DateTime)
                                fechaVigencia = (DateTime?)objFec;
                            else
                                fechaVigencia = DateTime.ParseExact(sFechVigencia, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    if (repcodi != null)
                    {
                        DespachoAppServicio servDesp = new DespachoAppServicio();
                        PrRepcvDTO oRepCv = servDesp.GetByIdPrRepcv(repcodi.Value);
                        DateTime fecha = oRepCv.Repfecha.Date;

                        if (fechaVigencia != null && fechaVigencia.Value.Date != fecha)
                        {
                        }
                    }

                    var regpropequipo = new FilaExcelParametrosGrupoMop()
                    {
                        Row = row,
                        //NumItem = numItem,
                        Formuladat = sValor,
                        StrFechadat = sFechVigencia,
                        Gdatsustento = (sSustento ?? "").Trim(),
                        Gdatcomentario = (sComentario ?? "").Trim(),
                        StrGdatcheckcero = sValCeroCorrecto.ToUpper(),
                        Concepcodi = concepcodi,
                        Grupocodi = grupocodi,
                        Repcodi = repcodi,
                        Fechadat = fechaVigencia
                    };

                    listaMacro.Add(regpropequipo);
                }
            }

            return listaMacro;
        }

        /// <summary>
        /// Validación de los parámetros carga masiva
        /// </summary>
        /// <param name="filaExcel"></param>
        /// <param name="listaFamilias"></param>
        /// <returns></returns>
        public string ValidarLecturaGrupoDatExcel(FilaExcelParametrosGrupoMop filaExcel)
        {
            string columnFechVigencia = "Fecha vigencia: ";
            string columnValor = "Formula: ";
            string columnValorCheckCero = "Valor(0) correcto: ";
            string columnComentario = "Comentario: ";
            string columnSustento = "Sustento: ";

            List<string> lMsgValidacion = new List<string>();

            List<string> listaValorCheckCero = new List<string>();
            listaValorCheckCero.Add("SI");
            listaValorCheckCero.Add("NO");

            // Validar Valor
            if (filaExcel.Formuladat != null && filaExcel.Formuladat == "0")
            {
                if (String.IsNullOrEmpty(filaExcel.StrGdatcheckcero))
                {
                    lMsgValidacion.Add(columnValorCheckCero + "Esta vacío o en blanco");
                }
                else
                {
                    if (!listaValorCheckCero.Contains(filaExcel.StrGdatcheckcero))
                    {
                        lMsgValidacion.Add(columnValorCheckCero + "Valor no valido");
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(filaExcel.StrGdatcheckcero))
                {
                    lMsgValidacion.Add(columnValorCheckCero + "no puede tener valor check(0)");
                }
            }

            //Validar comentario
            if (!string.IsNullOrEmpty(filaExcel.Gdatcomentario))
            {
                if (filaExcel.Gdatcomentario.Length > 500)
                    lMsgValidacion.Add(columnComentario + "no puede tener más de 500 caracteres");
            }

            //Validar sustento
            if (!string.IsNullOrEmpty(filaExcel.Gdatsustento))
            {
                if (filaExcel.Gdatsustento.Length > 400)
                    lMsgValidacion.Add(columnSustento + "no puede tener más de 400 caracteres");
            }

            return string.Join(". ", lMsgValidacion);
        }

        /// <summary>
        /// Guarda y actualiza parámetros de grupos Mop masivamente
        /// </summary>
        /// <param name="listaNuevo"></param>
        /// <param name="listaModificado"></param>
        /// <param name="usuario"></param>
        public void CargaMasivaParametrosMop(List<PrGrupodatDTO> listaNuevo, List<PrGrupodatDTO> listaModificado, string usuario)
        {
            var UoW = FactorySic.UnitOfWork();
            using (var connection = UoW.BeginConnection())
            {
                using (var transaction = UoW.StartTransaction(connection))
                {
                    try
                    {
                        //Guarda Registros nuevos masivamente
                        if (listaNuevo != null && listaNuevo.Any())
                        {
                            foreach (var item in listaNuevo)
                            {
                                FactorySic.GetPrGrupodatRepository().Save(item, connection, transaction);
                            }
                        }

                        //Actualiza Registros masivamente
                        if (listaModificado != null && listaModificado.Any())
                        {
                            foreach (var item in listaModificado)
                            {
                                item.Fechadat = item.Fechadat != null ? item.Fechadat : DateTime.Now;
                                item.Fechaact = item.Fechaact != null ? item.Fechaact : DateTime.Now;
                                FactorySic.GetPrGrupodatRepository().Update(item, connection, transaction);
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Genera log de parámetros de grupos mop erroneos
        /// </summary>
        /// <param name="path"></param>
        /// <param name="lstRegPropiedadesErroneos"></param>
        /// <returns></returns>
        public string GenerarArchivoLogParametrosErroneosCSV(string path, List<PrGrupodatDTO> lstRegPropiedadesErroneos)
        {
            string sLine = string.Empty;
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string fileNameCsv = "";
            fileNameCsv = sFecha + "_LogPropiedadesImport" + ".csv";

            using (var dbProviderWriter = new StreamWriter(new FileStream(path + fileNameCsv, FileMode.OpenOrCreate), Encoding.UTF8))
            {
                sLine = "FILA;OBSERVACIONES;FECHA DE VIGENCIA;FORMULA;VALOR CERO(0) CORRECTO;COMENTARIO;SUSTENTO";
                dbProviderWriter.WriteLine(sLine);
                foreach (PrGrupodatDTO entity in lstRegPropiedadesErroneos)
                {
                    sLine = this.CreateFilaErroneaParametroString(entity);
                    dbProviderWriter.WriteLine(sLine);
                }
                dbProviderWriter.Close();
            }
            return fileNameCsv;
        }

        /// <summary>
        /// Metodo que escribe una fila del archivo .CSV de parámetros GrupoMop Erroneas
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string CreateFilaErroneaParametroString(PrGrupodatDTO entity)
        {
            string sLine = string.Empty;

            sLine += entity.NroItem.ToString() + ConstantesFichaTecnica.SeparadorCampoCSV;
            sLine += ((entity.Observaciones != null) ? entity.Observaciones.ToString().Replace(',', ';') : "") + ConstantesFichaTecnica.SeparadorCampoCSV;

            sLine += ((entity.Fechadat != null) ? entity.Fechadat.Value.ToString(ConstantesAppServicio.FormatoFecha) : "") + ConstantesFichaTecnica.SeparadorCampoCSV;
            sLine += ((entity.Formuladat != null) ? entity.Formuladat.ToString() : "") + ConstantesFichaTecnica.SeparadorCampoCSV;
            sLine += ((entity.GdatcheckceroDesc != null) ? entity.GdatcheckceroDesc.ToString() : "") + ConstantesFichaTecnica.SeparadorCampoCSV;
            sLine += ((entity.Gdatcomentario != null) ? entity.Gdatcomentario.ToString() : "") + ConstantesFichaTecnica.SeparadorCampoCSV;
            sLine += ((entity.Gdatsustento != null) ? entity.Gdatsustento.ToString() : "") + ConstantesFichaTecnica.SeparadorCampoCSV;

            return sLine;
        }

        /// <summary>
        /// Eliminar archivos que están en la carpeta reporte Cada vez que se ingrese al módulo de Parametros GrupoMop
        /// </summary>
        public void EliminarArchivosReporte()
        {
            try
            {
                string pathAlternativo = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes;
                var listaDocumentos = FileServer.ListarArhivos(null, pathAlternativo);

                if (listaDocumentos.Any())
                {
                    foreach (var item in listaDocumentos)
                    {
                        //los archivos se guardan con prefijo 2018, 2019, 2020, 2021, 2022 ... entonces se eliminaran
                        if (item.FileName.StartsWith("201") || item.FileName.StartsWith("202"))
                        {
                            FileServer.DeleteBlob(item.FileName, pathAlternativo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException("No se pudo eliminar el archivo del servidor.", ex);
            }
        }

        #endregion

        #endregion

        #region Ingreso / Retiro Operación Comercial de Modos de Operación

        public List<PrGrupoDTO> ListarModoOperacionTienenOpComercial(DateTime fechaIni, DateTime fechaFin, bool flagSoloDatVigente = false)
        {
            List<PrGrupoDTO> listaModoOpComercial = new List<PrGrupoDTO>();

            //
            List<PrGrupoDTO> listaGrupo = FactorySic.GetPrGrupoRepository().List("2,3,4");
            listaGrupo = listaGrupo.Where(x => x.GrupoEstado != ConstantesAppServicio.Anulado).ToList();

            List<PrGrupoDTO> listaGrupoModo = listaGrupo.Where(x => x.Catecodi == (int)ConstantesMigraciones.Catecodi.ModoOperacionTermico).ToList();

            //Operación comercial       
            List<PrGrupodatDTO> listaOperacionComercial = ListarGrupodatXConcepto(ConstantesMigraciones.ConcepcodiOpComercial.ToString(), "-1", flagSoloDatVigente, fechaIni);

            foreach (var regModo in listaGrupoModo)
            {
                if (regModo.Grupocodi == 199)
                { }

                //datos de la central
                GetGrupoCentral(regModo.Grupopadre, listaGrupo, out string nombreCentral, out int codigoCentral);
                regModo.Central = nombreCentral;
                regModo.GrupoCentral = codigoCentral;

                //datos de operación comercial
                SetValorOperacionComercial(regModo.Grupocodi, fechaIni, fechaFin, listaOperacionComercial, out string opComercial, out DateTime? fechaInicio, out DateTime? fechaRetiro);

                regModo.TipogenerrerDesc = regModo.TipoGenerRer == ConstantesAppServicio.SI ? ConstantesAppServicio.SIDesc : string.Empty;
                regModo.GrupotipocogenDesc = regModo.Grupotipocogen == ConstantesAppServicio.SI ? ConstantesAppServicio.SIDesc : string.Empty;
                regModo.Gruponomb = (regModo.Gruponomb ?? "").Trim();
                regModo.Grupoabrev = (regModo.Grupoabrev ?? "").Trim();

                if (ConstantesAppServicio.SI == opComercial)
                {
                    regModo.Fechiniopcom = fechaInicio;
                    regModo.Fechfinopcom = fechaRetiro;
                    regModo.TieneNuevoIngresoOpComercial = fechaIni <= fechaInicio && fechaInicio <= fechaFin;
                    regModo.TieneNuevoRetiroOpComercial = fechaIni <= fechaRetiro && fechaRetiro <= fechaFin;
                    listaModoOpComercial.Add(regModo);
                }
            }

            return listaModoOpComercial;
        }

        private List<PrGrupodatDTO> ListarGrupodatXConcepto(string concepcodis, string catecodis, bool flagSoloDatVigente, DateTime fechaVigencia)
        {
            List<PrGrupodatDTO> lista;
            if (flagSoloDatVigente)
            {
                lista = FactorySic.GetPrGrupodatRepository().ParametrosConfiguracionPorFecha(fechaVigencia, "-1", concepcodis);
            }
            else
            {
                lista = FactorySic.GetPrGrupodatRepository().ListarHistoricoValores(concepcodis, -1)
                                                              .Where(x => x.Deleted == 0).OrderByDescending(x => x.Fechadat).ToList();
            }

            return lista;
        }

        /// <summary>
        /// Establecer el valor de Operacion comercial
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaGrupodat"></param>
        /// <param name="opComercial"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaRetiro"></param>
        private void SetValorOperacionComercial(int grupocodi, DateTime fechaIni, DateTime fechaFin, List<PrGrupodatDTO> listaGrupodat
                                                        , out string opComercial, out DateTime? fechaInicio, out DateTime? fechaRetiro)
        {
            opComercial = ConstantesAppServicio.NO;
            fechaInicio = null;
            fechaRetiro = null;

            var regPrimerDia = listaGrupodat.Find(x => x.Fechadat <= fechaIni && x.Grupocodi == grupocodi);
            var regExisteOpEnMes = listaGrupodat.Find(x => x.Formuladat == ConstantesAppServicio.SI && fechaIni < x.Fechadat && x.Fechadat <= fechaFin && x.Grupocodi == grupocodi);
            var regRetiroOpEnMes = listaGrupodat.Find(x => x.Formuladat == ConstantesAppServicio.NO && fechaIni <= x.Fechadat && x.Fechadat <= fechaFin.AddDays(1) && x.Grupocodi == grupocodi);

            //verificar si en el primer dia del mes tiene o no Op comercial
            if (regPrimerDia != null && regPrimerDia.Formuladat == ConstantesAppServicio.SI)
            {
                opComercial = ConstantesAppServicio.SI;
                fechaInicio = regPrimerDia.Fechadat;
            }

            //si en el mes inicia operacion comercial
            if (regExisteOpEnMes != null)
            {
                opComercial = ConstantesAppServicio.SI;
                fechaInicio = regExisteOpEnMes.Fechadat;
            }

            if (regRetiroOpEnMes != null)
                fechaRetiro = regRetiroOpEnMes.Fechadat;
        }
        /// <summary>
        /// Obtener nombre de la central
        /// </summary>
        /// <param name="grupopadre"></param>
        /// <param name="listaGrupo"></param>
        /// <returns></returns>
        private void GetGrupoCentral(int? grupopadre, List<PrGrupoDTO> listaGrupo, out string nombreCentral, out int codigoCentral)
        {
            nombreCentral = "";
            codigoCentral = -1;

            var objPadre = listaGrupo.Find(x => x.Grupocodi == grupopadre);
            if (objPadre != null)
            {
                var objCentral = listaGrupo.Find(x => x.Grupocodi == objPadre.Grupopadre);
                if (objCentral != null)
                {
                    nombreCentral = objCentral.Gruponomb;
                    codigoCentral = objCentral.Grupocodi;
                }
            }
        }

        #endregion

        #region Reporte de Control de Cambios

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="idAgrup"></param>
        /// <param name="catecodi"></param>
        /// <returns></returns>
        public List<PrGrupodatDTO> ListaPrGrupoDatByGrupoYFecha(DateTime fecha, int idAgrup, int catecodi)
        {
            List<PrGrupodatDTO> listaData = FactorySic.GetPrGrupodatRepository().ReporteControlCambios(fecha);

            if (idAgrup != -1)
            {
                var listaAgrupacionConcepto = this.GetByCriteriaPrAgrupacionConceptos(ConstantesMigraciones.Activo, idAgrup);
                var listaConcepcodis = listaAgrupacionConcepto.Select(x => x.Concepcodi).ToList();
                listaData = listaData.Where(x => listaConcepcodis.Contains(x.Concepcodi)).ToList();
            }

            if (catecodi != -2)
            {
                switch (catecodi)
                {
                    case 4:
                        listaData = listaData.Where(x => x.Catecodi == 4 || x.Catecodi == 3 || x.Catecodi == 2).ToList();
                        break;
                    case 6:
                        listaData = listaData.Where(x => x.Catecodi == 6 || x.Catecodi == 5 || x.Catecodi == 9).ToList();
                        break;
                }
            }

            foreach (var reg in listaData)
            {
                reg.GrupoNomb = reg.GrupoNomb != null ? reg.GrupoNomb.Trim() : string.Empty;
                reg.ConcepDesc = reg.ConcepDesc != null ? reg.ConcepDesc.Trim() : string.Empty;
                reg.ConcepUni = reg.ConcepUni != null ? reg.ConcepUni.Trim() : string.Empty;
                reg.Concepabrev = reg.Concepabrev != null ? reg.Concepabrev.Trim() : string.Empty;
                reg.FechadatDesc = reg.Fechadat != null ? reg.Fechadat.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
                reg.FechaactDesc = reg.Fechaact != null ? reg.Fechaact.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
            }

            return listaData;
        }

        /// <summary>
        /// Metodo 
        /// <param name="data"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ListarReporteControlCambiosHtml(DateTime fecha, int idAgrup, int catecodi)
        {
            List<PrGrupodatDTO> listaData = this.ListaPrGrupoDatByGrupoYFecha(fecha, idAgrup, catecodi);
            var listaConcepto = listaData.Where(x => x.Cambio == 1).ToList();

            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' width='100%' id='tabla'>");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style='' colspan='2'>Grupo / Modo de Operación</th>");
            str.Append("<th style='' colspan='3'>Concepto</th>");
            str.Append("<th style='' class='cambio_actual' colspan='4'>Actual</th>");
            str.Append("<th style='' colspan='4'>Anterior</th>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.Append("<th style=''>Código</th>");
            str.Append("<th style=''>Nombre</th>");

            str.Append("<th style=''>Código</th>");
            str.Append("<th style=''>Concepto</th>");
            str.Append("<th style=''>Abreviatura </th>");

            str.Append("<th style='' class='cambio_actual'>Fecha</th>");
            str.Append("<th style='' class='cambio_actual'>Valor</th>");
            str.Append("<th style='' class='cambio_actual'>Usuario</th>");
            str.Append("<th style='' class='cambio_actual'>Fecha actualización</th>");

            str.Append("<th style=''>Fecha</th>");
            str.Append("<th style=''>Valor</th>");
            str.Append("<th style=''>Usuario</th>");
            str.Append("<th style=''>Fecha actualización</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");
            #region cuerpo

            if (listaConcepto.Count > 0)
            {
                foreach (var reg in listaConcepto)
                {
                    PrGrupodatDTO anterior = listaData.Find(x => x.Grupocodi == reg.Grupocodi && x.Concepcodi == reg.Concepcodi && x.Cambio == 2);

                    str.AppendFormat("<td class=''>{0}</td>", reg.Grupocodi);
                    str.AppendFormat("<td style='text-align: left !important;'>{0}</td>", reg.GrupoNomb);

                    str.AppendFormat("<td class=''>{0}</td>", reg.Concepcodi);
                    str.AppendFormat("<td class=''>{0}</td>", reg.ConcepDesc);
                    str.AppendFormat("<td class=''>{0}</td>", reg.Concepabrev);

                    str.AppendFormat("<td class=''>{0}</td>", reg.FechadatDesc);
                    str.AppendFormat("<td class=''>{0}</td>", reg.Formuladat);
                    str.AppendFormat("<td class=''>{0}</td>", reg.Lastuser);
                    str.AppendFormat("<td class=''>{0}</td>", reg.FechaactDesc);

                    str.AppendFormat("<td class=''>{0}</td>", anterior != null ? anterior.FechadatDesc : string.Empty);
                    str.AppendFormat("<td class=''>{0}</td>", anterior != null ? anterior.Formuladat : string.Empty);
                    str.AppendFormat("<td class=''>{0}</td>", anterior != null ? anterior.Lastuser : string.Empty);
                    str.AppendFormat("<td class=''>{0}</td>", anterior != null ? anterior.FechaactDesc : string.Empty);

                    str.AppendFormat("</td>");

                    str.Append("</tr>");
                }
            }

            #endregion
            str.Append("</tbody>");

            str.Append("</table>");

            return str.ToString();
        }

        /// <summary>
        /// Reporte Excel 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public string GenerarFileExcelReporteControlCambio(DateTime fecha)
        {
            List<PrGrupodatDTO> listaData = this.ListaPrGrupoDatByGrupoYFecha(fecha, -1, -2);
            var listaConcepto = listaData.Where(x => x.Cambio == 1).ToList();

            string fileExcel = string.Empty;

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet wsHist = xlPackage.Workbook.Worksheets.Add(ConstantesMigraciones.RptControlCambiosNombreHoja);
                wsHist.View.ShowGridLines = false;

                var data = listaData;
                string fechaIniFiltro = fecha.ToString(ConstantesAppServicio.FormatoFecha);

                #region Hoja histórico

                int row = 4;
                int column = 2;

                int rowTitulo = 2;
                wsHist.Cells[rowTitulo, column + 2].Value = ConstantesMigraciones.RptControlCambiosTituloHoja;
                wsHist.Cells[rowTitulo, column + 2].Style.Font.SetFromFont(new Font("Calibri", 16));
                wsHist.Cells[rowTitulo, column + 2].Style.Font.Bold = true;

                #region filtros
                int rowIniFiltro = row + 1;

                row++;
                wsHist.Cells[row, column].Value = "Fecha Consulta:";
                wsHist.Cells[row, column].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, column + 1].Value = fechaIniFiltro;

                using (var range = wsHist.Cells[rowIniFiltro, column, row, column])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                }

                #endregion

                row += 2;
                #region cabecera

                //primera fila
                int rowIni1 = row;
                int colIniGrupo = column;
                int colFinGrupo = colIniGrupo + 1;
                int colIniConcepto = colFinGrupo + 1;
                int colFinConcepto = colIniConcepto + 2;
                int colIniActual = colFinConcepto + 1;
                int colFinActual = colIniActual + 3;
                int colIniAnterior = colFinActual + 1;
                int colFinAnterior = colIniAnterior + 3;

                wsHist.Cells[row, colIniGrupo].Value = "Grupo / Modo de Operación";
                wsHist.Cells[row, colIniGrupo, row, colFinGrupo].Merge = true;
                wsHist.Cells[row, colIniGrupo, row, colFinGrupo].Style.WrapText = true;
                wsHist.Cells[row, colIniGrupo, row, colFinGrupo].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                wsHist.Cells[row, colIniConcepto].Value = "Concepto";
                wsHist.Cells[row, colIniConcepto, row, colFinConcepto].Merge = true;
                wsHist.Cells[row, colIniConcepto, row, colFinConcepto].Style.WrapText = true;
                wsHist.Cells[row, colIniConcepto, row, colFinConcepto].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                wsHist.Cells[row, colIniActual].Value = "Actual";
                wsHist.Cells[row, colIniActual, row, colFinActual].Merge = true;
                wsHist.Cells[row, colIniActual, row, colFinActual].Style.WrapText = true;
                wsHist.Cells[row, colIniActual, row, colFinActual].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                wsHist.Cells[row, colIniAnterior].Value = "Anterior";
                wsHist.Cells[row, colIniAnterior, row, colFinAnterior].Merge = true;
                wsHist.Cells[row, colIniAnterior, row, colFinAnterior].Style.WrapText = true;
                wsHist.Cells[row, colIniAnterior, row, colFinAnterior].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                row++;
                //segunda fila
                int rowIni2 = row;
                int colIniGrupocodigo = column;
                int colIniGruponombre = colIniGrupocodigo + 1;
                int colIniConceptocodigo = colIniGruponombre + 1;
                int colIniConceptonombre = colIniConceptocodigo + 1;
                int colIniConceptoabrev = colIniConceptonombre + 1;

                int colIniActualFecha = colIniConceptoabrev + 1;
                int colIniActualValor = colIniActualFecha + 1;
                int colIniActualUsuariomodif = colIniActualValor + 1;
                int colIniActualFechamodif = colIniActualUsuariomodif + 1;

                int colIniAnteriorFecha = colIniActualFechamodif + 1;
                int colIniAnteriorValor = colIniAnteriorFecha + 1;
                int colIniAnteriorUsuariomodif = colIniAnteriorValor + 1;
                int colIniAnteriorFechamodif = colIniAnteriorUsuariomodif + 1;

                wsHist.Cells[row, colIniGrupocodigo].Value = "Código";
                wsHist.Cells[row, colIniGrupocodigo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniGruponombre].Value = "Nombre";
                wsHist.Cells[row, colIniGruponombre].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                wsHist.Cells[row, colIniConceptocodigo].Value = "Código";
                wsHist.Cells[row, colIniConceptocodigo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniConceptonombre].Value = "Concepto";
                wsHist.Cells[row, colIniConceptonombre].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniConceptoabrev].Value = "Abreviatura";
                wsHist.Cells[row, colIniConceptoabrev].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                wsHist.Cells[row, colIniActualFecha].Value = "Fecha";
                wsHist.Cells[row, colIniActualFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniActualValor].Value = "Valor";
                wsHist.Cells[row, colIniActualValor].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniActualUsuariomodif].Value = "Usuario";
                wsHist.Cells[row, colIniActualUsuariomodif].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniActualFechamodif].Value = "Fecha actualización";
                wsHist.Cells[row, colIniActualFechamodif].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                wsHist.Cells[row, colIniAnteriorFecha].Value = "Fecha";
                wsHist.Cells[row, colIniAnteriorFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniAnteriorValor].Value = "Valor";
                wsHist.Cells[row, colIniAnteriorValor].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniAnteriorUsuariomodif].Value = "Usuario";
                wsHist.Cells[row, colIniAnteriorUsuariomodif].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniAnteriorFechamodif].Value = "Fecha actualización";
                wsHist.Cells[row, colIniAnteriorFechamodif].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                using (var range = wsHist.Cells[rowIni1, colIniGrupo, rowIni2, colFinConcepto])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                }

                using (var range = wsHist.Cells[rowIni1, colIniActual, rowIni2, colFinActual])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#31869B"));
                    range.Style.Font.Color.SetColor(Color.White);
                }

                using (var range = wsHist.Cells[rowIni1, colIniAnterior, rowIni2, colFinAnterior])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                }

                #endregion

                #region cuerpo
                int rowIni = row;
                foreach (var reg in listaConcepto)
                {
                    PrGrupodatDTO anterior = listaData.Find(x => x.Grupocodi == reg.Grupocodi && x.Concepcodi == reg.Concepcodi && x.Cambio == 2);
                    row++;
                    wsHist.Cells[row, colIniGrupocodigo].Value = reg.Grupocodi;
                    wsHist.Cells[row, colIniGrupocodigo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniGrupocodigo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsHist.Cells[row, colIniGruponombre].Value = reg.GrupoNomb;
                    wsHist.Cells[row, colIniGruponombre].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    wsHist.Cells[row, colIniConceptocodigo].Value = reg.Concepcodi;
                    wsHist.Cells[row, colIniConceptocodigo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniConceptocodigo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsHist.Cells[row, colIniConceptonombre].Value = reg.ConcepDesc;
                    wsHist.Cells[row, colIniConceptonombre].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniConceptonombre].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsHist.Cells[row, colIniConceptoabrev].Value = reg.Concepabrev;
                    wsHist.Cells[row, colIniConceptoabrev].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniConceptoabrev].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    wsHist.Cells[row, colIniActualFecha].Value = reg.FechadatDesc;
                    wsHist.Cells[row, colIniActualFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniActualFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsHist.Cells[row, colIniActualValor].Value = reg.Formuladat;
                    wsHist.Cells[row, colIniActualValor].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniActualValor].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsHist.Cells[row, colIniActualUsuariomodif].Value = reg.Lastuser;
                    wsHist.Cells[row, colIniActualUsuariomodif].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniActualUsuariomodif].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsHist.Cells[row, colIniActualFechamodif].Value = reg.FechaactDesc;
                    wsHist.Cells[row, colIniActualFechamodif].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniActualFechamodif].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    wsHist.Cells[row, colIniAnteriorFecha].Value = anterior != null ? anterior.FechadatDesc : string.Empty;
                    wsHist.Cells[row, colIniAnteriorFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniAnteriorFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsHist.Cells[row, colIniAnteriorValor].Value = anterior != null ? anterior.Formuladat : string.Empty;
                    wsHist.Cells[row, colIniAnteriorValor].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniAnteriorValor].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsHist.Cells[row, colIniAnteriorUsuariomodif].Value = anterior != null ? anterior.Lastuser : string.Empty;
                    wsHist.Cells[row, colIniAnteriorUsuariomodif].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniAnteriorUsuariomodif].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsHist.Cells[row, colIniAnteriorFechamodif].Value = anterior != null ? anterior.FechaactDesc : string.Empty;
                    wsHist.Cells[row, colIniAnteriorFechamodif].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniAnteriorFechamodif].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                wsHist.Column(1).Width = 3;
                wsHist.Column(colIniGrupocodigo).Width = 15;
                wsHist.Column(colIniGruponombre).Width = 35;
                wsHist.Column(colIniConceptocodigo).Width = 9;
                wsHist.Column(colIniConceptonombre).Width = 30;
                wsHist.Column(colIniConceptoabrev).Width = 17;
                wsHist.Column(colIniActualFecha).Width = 17;
                wsHist.Column(colIniActualValor).Width = 20;
                wsHist.Column(colIniActualUsuariomodif).Width = 24;
                wsHist.Column(colIniActualFechamodif).Width = 17;
                wsHist.Column(colIniAnteriorFecha).Width = 17;
                wsHist.Column(colIniAnteriorValor).Width = 20;
                wsHist.Column(colIniAnteriorUsuariomodif).Width = 24;
                wsHist.Column(colIniAnteriorFechamodif).Width = 17;

                wsHist.View.ZoomScale = 85;
                #endregion

                #region logo

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = wsHist.Drawings.AddPicture("Imagen", img);
                picture.From.Column = 1;
                picture.From.Row = 1;

                #endregion

                #endregion

                fileExcel = System.IO.Path.GetTempFileName();
                xlPackage.SaveAs(new FileInfo(fileExcel));
            }

            return fileExcel;
        }

        #endregion

        #region DIgSILENT

        /// <summary>
        /// Get informacion me_lectura BD
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public List<MeLecturaDTO> GetByCriteriaMeLectura(string lectcodi)
        {
            return FactorySic.GetMeLecturaRepository().GetByCriteria(lectcodi);
        }

        /// <summary>
        /// Get informacion equirel digsilent
        /// </summary>
        /// <returns></returns>
        public List<EqEquirelDTO> GetListaEquirelDigsilent(string tiporelcodi)
        {
            return FactorySic.GetEqEquirelRepository().GetByCriteria(int.Parse(ConstantesAppServicio.ParametroDefecto), tiporelcodi);
        }

        /// <summary>
        /// Get informacion generacion que opero
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <param name="tipoFuenteReprog"></param>
        /// <param name="topcodiYupana"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> GetListaGeneracionDIgSILENT(string lectcodi, int tipoFuenteReprog, int topcodiYupana, DateTime fecha)
        {
            //obtener configuración Digsilent, Potencia máxima, potencia mínima, potencia efectiva
            var lista = FactorySic.GetMeMedicion24Repository().ListaGeneracionDIgSILENT(fecha);

            //Obtener data de medición
            bool reprogAprobado = true;
            List<MeMedicion48DTO> listaDespachoYupanaReprog = new List<MeMedicion48DTO>(), listaDespachoYupanaReprogHidroXEq = new List<MeMedicion48DTO>();

            //Reprog Yupana y preliminar
            if (ConstantesAppServicio.LectcodiReprogDiario == lectcodi && tipoFuenteReprog == 2 && topcodiYupana > 0)
            {
                listaDespachoYupanaReprog = this.DespachoYupana(fecha, fecha, topcodiYupana, string.Empty, ConstantesBase.IdlectDespachoReprog, out listaDespachoYupanaReprogHidroXEq);

                listaDespachoYupanaReprog = listaDespachoYupanaReprog.Where(x => x.Meditotal != 0).ToList();
                listaDespachoYupanaReprogHidroXEq = listaDespachoYupanaReprogHidroXEq.Where(x => x.Meditotal != 0).ToList();

                reprogAprobado = false;
            }

            List<MeMedicion48DTO> listaMe48 = reprogAprobado ? this.GetObtenerHistoricoMedicion48(Int32.Parse(lectcodi), fecha, fecha) : listaDespachoYupanaReprog;
            List<MeMedicion48DTO> listaMe48HidroXGen = reprogAprobado ? new List<MeMedicion48DTO>() : listaDespachoYupanaReprogHidroXEq;
            listaMe48 = listaMe48.Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMW).ToList();

            //Volver a calcular el meditotal
            int grupocodiFicticio = 100000; //el ficticio es para que no se distribuya en generadores que sí tienen despacho x equipo
            foreach (var m24 in lista)
            {
                decimal meditotal = 0;

                //buscar en medicion48 si existe el equipo
                var regm48 = listaMe48.Find(x => x.Ptomedicodi == m24.Ptomedicodi);
                bool existePtoXgen = listaMe48HidroXGen.Find(x => x.Ptomedicodi == m24.Ptomedicodi) != null; //si existe algun equipo distribuido en yupana

                MeMedicion48DTO reg48bd = regm48;
                if (existePtoXgen)
                {
                    //obtener datos de medias horas
                    reg48bd = listaMe48HidroXGen.Find(x => x.Ptomedicodi == m24.Ptomedicodi && x.Equicodi == m24.Equicodi);

                    //asignar nuevo codigo al grupocodi
                    m24.Grupocodi = grupocodiFicticio;
                    grupocodiFicticio++;
                }

                if (reg48bd != null)
                {
                    for (int h = 1; h <= 24; h++)
                    {
                        //El valor del H de medicion48 es el mismo para todos los equipos del grupo despacho
                        decimal valorH = ((decimal?)reg48bd.GetType().GetProperty(ConstantesAppServicio.CaracterH + h * 2).GetValue(reg48bd, null)).GetValueOrDefault(0);
                        m24.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(m24, valorH);

                        meditotal += valorH;
                    }
                }

                if (meditotal > 0)
                    m24.Meditotal = meditotal;
            }

            //Completar información RE-10650
            lista = this.CompletarDigSilentEolicaSolares(lista);

            lista = lista.Where(x => ConstantesHorasOperacion.IdTipoEolica != x.Famcodi && ConstantesHorasOperacion.IdTipoSolar != x.Famcodi).ToList();

            //Formatear data
            foreach (var reg in lista)
            {
                reg.Digsilent = !string.IsNullOrEmpty(reg.Digsilent) ? reg.Digsilent.Trim() : string.Empty;
                reg.FechapropequiMinDesc = reg.FechapropequiMin != null ? reg.FechapropequiMin.Value.ToString(ConstantesAppServicio.FormatoFecha) : string.Empty;
                reg.FechapropequiPotefecDesc = reg.FechapropequiPotefec != null ? reg.FechapropequiPotefec.Value.ToString(ConstantesAppServicio.FormatoFecha) : string.Empty;
            }

            return lista;
        }

        public List<MeMedicion24DTO> CompletarDigSilentEolicaSolares(List<MeMedicion24DTO> lista)
        {
            List<MeMedicion24DTO> listaFinal = new List<MeMedicion24DTO>();
            List<MeMedicion24DTO> listaConfUnidadesFicticas = this.ListarEquiposEolicoSolarRE10650();

            foreach (var reg in lista)
            {
                if (ConstantesHorasOperacion.IdTipoEolica == reg.Famcodi
                    || ConstantesHorasOperacion.IdTipoSolar == reg.Famcodi)
                {
                    if (reg.Equicodi == 14426 || reg.Equicodi == 18074)
                    { }
                    var listaUnidadFict = listaConfUnidadesFicticas.Where(x => x.Equipadre == reg.Equicodi).ToList();
                    decimal potenciaTotalMVA = listaUnidadFict.Sum(x => x.NumUnidadesXGrupo * x.MVAxUnidad.GetValueOrDefault(0));

                    var listaDataXEq = new List<MeMedicion24DTO>();
                    foreach (var regEq in listaUnidadFict)
                    {
                        listaDataXEq.Add(this.CopiarDataDigsilent24(reg, regEq, potenciaTotalMVA));
                    }

                    listaFinal.AddRange(listaDataXEq);
                }
                else
                {
                    listaFinal.Add(reg);
                }
            }

            return listaFinal;
        }

        /// <summary>
        /// Listar las unidades para las unidades eólicas y solares
        /// </summary>
        /// <returns></returns>
        public List<MeMedicion24DTO> ListarEquiposEolicoSolarRE10650()
        {
            MeMedicion24DTO obj = null;
            List<MeMedicion24DTO> l = new List<MeMedicion24DTO>();

            //TALARA_E
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 14426;
            obj.Digsilent = "PE_Talara";
            obj.NumUnidadesXGrupo = 17;
            obj.MVAxUnidad = 2.0m;
            l.Add(obj);

            //CUPISNIQUE_E
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 14407;
            obj.Digsilent = "PE_Cupisnique";
            obj.NumUnidadesXGrupo = 45;
            obj.MVAxUnidad = 2.0m;
            l.Add(obj);

            //WAYRA1_E
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 18306;
            obj.Digsilent = "CE_Wayra I";
            obj.NumUnidadesXGrupo = 42;
            obj.MVAxUnidad = 3.9m;
            l.Add(obj);

            //MAJES_S
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 13402;
            obj.Digsilent = "PS_Majes1";
            obj.NumUnidadesXGrupo = 16;
            obj.MVAxUnidad = 0.625m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 13402;
            obj.Digsilent = "PS_Majes2";
            obj.NumUnidadesXGrupo = 16;
            obj.MVAxUnidad = 0.625m;
            l.Add(obj);

            //REPARTICIO_S
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 13399;
            obj.Digsilent = "PS_Reparticion1";
            obj.NumUnidadesXGrupo = 16;
            obj.MVAxUnidad = 0.625m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 13399;
            obj.Digsilent = "PS_Reparticion2";
            obj.NumUnidadesXGrupo = 16;
            obj.MVAxUnidad = 0.625m;
            l.Add(obj);

            //TACNA_S
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 13503;
            obj.Digsilent = "PS_Tacna1";
            obj.NumUnidadesXGrupo = 16;
            obj.MVAxUnidad = 0.625m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 13503;
            obj.Digsilent = "PS_Tacna2";
            obj.NumUnidadesXGrupo = 16;
            obj.MVAxUnidad = 0.625m;
            l.Add(obj);

            //PANAMERICA_S
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 13533;
            obj.Digsilent = "PS_Panamericana 2";
            obj.NumUnidadesXGrupo = 16;
            obj.MVAxUnidad = 0.625m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 13533;
            obj.Digsilent = "PS_Panamericana1";
            obj.NumUnidadesXGrupo = 16;
            obj.MVAxUnidad = 0.625m;
            l.Add(obj);

            //MOQUEGUA_S
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 14762;
            obj.Digsilent = "PS_MoqueguaFV 1";
            obj.NumUnidadesXGrupo = 10;
            obj.MVAxUnidad = 0.8m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 14762;
            obj.Digsilent = "PS_MoqueguaFV 2";
            obj.NumUnidadesXGrupo = 10;
            obj.MVAxUnidad = 0.8m;
            l.Add(obj);

            //INTIPAMPA_S
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 17796;
            obj.Digsilent = "CS_Intipampa1";
            obj.NumUnidadesXGrupo = 9;
            obj.MVAxUnidad = 2.5m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 17796;
            obj.Digsilent = "CS_Intipampa2";
            obj.NumUnidadesXGrupo = 9;
            obj.MVAxUnidad = 2.5m;
            l.Add(obj);

            //RUBI_S
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 18074;
            obj.Digsilent = "CS_Rubi1";
            obj.NumUnidadesXGrupo = 40;
            obj.MVAxUnidad = 1.025m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 18074;
            obj.Digsilent = "CS_Rubi2";
            obj.NumUnidadesXGrupo = 40;
            obj.MVAxUnidad = 1.025m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 18074;
            obj.Digsilent = "CS_Rubi3";
            obj.NumUnidadesXGrupo = 42;
            obj.MVAxUnidad = 1.025m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 18074;
            obj.Digsilent = "CS_Rubi4";
            obj.NumUnidadesXGrupo = 42;
            obj.MVAxUnidad = 1.025m;
            l.Add(obj);

            //MARCONA_E
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 14160;
            obj.Digsilent = "PE_Marcona1";
            obj.NumUnidadesXGrupo = 8;
            obj.MVAxUnidad = 3.15m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 14160;
            obj.Digsilent = "PE_Marcona2";
            obj.NumUnidadesXGrupo = 3;
            obj.MVAxUnidad = 2.3m;
            l.Add(obj);

            //TRES_HERMANA
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 15160;
            obj.Digsilent = "PE_3 Hermanas 1";
            obj.NumUnidadesXGrupo = 25;
            obj.MVAxUnidad = 3.15m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 15160;
            obj.Digsilent = "PE_3 Hermanas 2";
            obj.NumUnidadesXGrupo = 8;
            obj.MVAxUnidad = 2.3m;
            l.Add(obj);


            //////////////////////////////////////////////////////
            ///Asignar un equicodi ficticio

            int maxEquicodi = -10000;
            foreach (var reg in l)
            {
                reg.PotenciaTotalMVA = reg.NumUnidadesXGrupo * reg.MVAxUnidad;
                reg.Equicodi = maxEquicodi;
                maxEquicodi--;
            }

            return l;
        }

        /// <summary>
        /// Copiar informacion de la central a sus unidades
        /// </summary>
        /// <param name="m24"></param>
        /// <param name="regEquipo"></param>
        /// <returns></returns>
        private MeMedicion24DTO CopiarDataDigsilent24(MeMedicion24DTO m24, MeMedicion24DTO regEquipo, decimal totalMVACentral)
        {
            MeMedicion24DTO reg = new MeMedicion24DTO();
            reg.Medifecha = m24.Medifecha;
            reg.Ptomedicodi = m24.Ptomedicodi;
            reg.Emprcodi = m24.Emprcodi;
            reg.Lectcodi = m24.Lectcodi;
            reg.Tipoinfocodi = m24.Tipoinfocodi;
            for (int h = 1; h <= 24; h++)
            {
                decimal valorCentral = ((decimal?)m24.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m24, null)).GetValueOrDefault(0);

                decimal mwXUnidadGenerador = valorCentral * regEquipo.PotenciaTotalMVA.GetValueOrDefault(0) / totalMVACentral;
                //Para el archivo DLE (Potencia entre el número de unidades de cada grupo)
                decimal valorUnidadGenerador = mwXUnidadGenerador / regEquipo.NumUnidadesXGrupo;

                reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(reg, valorUnidadGenerador);
            }
            reg.Meditotal = m24.Meditotal;

            reg.Grupocodi = m24.Grupocodi;
            reg.Gruponomb = m24.Gruponomb;
            reg.Grupoabrev = m24.Grupoabrev;
            reg.Grupotipo = m24.Grupotipo;
            reg.Equicodi = regEquipo.Equicodi;
            reg.Equipadre = regEquipo.Equipadre;
            reg.Central = m24.Central;
            reg.Famcodi = m24.Famcodi == ConstantesHorasOperacion.IdTipoEolica ? ConstantesHorasOperacion.IdGeneradorEolica : ConstantesHorasOperacion.IdGeneradorSolar;
            reg.Equiabrev = "--";
            reg.Equinomb = "--";
            reg.Minimo = 0; //actualmente las centrales eolicas y solares no tienen una propiedad mínima
            reg.PotenciaEfectiva = regEquipo.PotenciaTotalMVA;
            reg.PotenciaEfectiva = Math.Round(reg.PotenciaEfectiva.GetValueOrDefault(0), 3);
            reg.FechapropequiMin = null;
            reg.FechapropequiPotefec = m24.FechapropequiPotefec;
            reg.Digsilent = regEquipo.Digsilent;
            reg.Emprnomb = m24.Emprnomb;

            return reg;
        }

        /// <summary>
        /// Get informacion lineas digsilent
        /// </summary>
        /// <param name="propcodiDigsilente"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> GetListaLineasDigsilent(string propcodi, string famcodi)
        {
            return FactorySic.GetEqEquipoRepository().ListaLineasDigsilent(propcodi, famcodi);
        }

        /// <summary>
        /// Get informacion mantenimientos digsilent
        /// </summary>
        /// <param name="evenclasecodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<EveManttoDTO> GetListaManttosDigsilent(string evenclasecodi, DateTime fecha)
        {
            return FactorySic.GetEveManttoRepository().ListaManttosDigsilent(evenclasecodi, fecha);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propcodi"></param>
        /// <param name="famcodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> GetListaDemandaDigsilent(string propcodi, string famcodi, DateTime fecha)
        {
            return FactorySic.GetMeMedicion24Repository().ListaDemandaDigsilent(propcodi, famcodi, fecha);
        }

        /// <summary>
        /// Txt Digsilent Html
        /// </summary>
        /// <param name="listaEqequipo"></param>
        /// <returns></returns>
        public string ArchivoDigsilentHtml(List<EqEquipoDTO> listaEqequipo)
        {
            StringBuilder srtHtml = new StringBuilder();

            foreach (var d in listaEqequipo)
            {
                srtHtml.Append("set/fkey obj=" + d.Valor + " val=" + d.Formuladat + "<br>");
            }

            return srtHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siparcodiDigSdemanda"></param>
        /// <returns></returns>
        public SiParametroDTO GetByCriteriaSiparametro(int siparcodiDigSdemanda)
        {
            return FactorySic.GetSiParametroRepository().GetById(siparcodiDigSdemanda);
        }

        /// <summary>
        /// Proceso digsilent
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="program"></param>
        /// <param name="rdchk"></param>
        /// <param name="bloq"></param>
        /// <param name="fuenteReprog"></param>
        /// <param name="topcodiYupana"></param>
        /// <param name="resultado"></param>
        /// <param name="comentario"></param>
        /// <param name="configuracion"></param>
        /// <param name="validacionDuplicadoForeignKey"></param>
        public void ProcesarDIgSILENT(DateTime fechaPeriodo, string program, int rdchk, string bloq, int fuenteReprog, int topcodiYupana
            , out string resultado, out string comentario
            , out string configuracion, out string validacionDuplicadoForeignKey)
        {
            List<EqEquirelDTO> ListaEquirel = new List<EqEquirelDTO>();
            List<MeMedicion24DTO> ListaGeneracionOpera = new List<MeMedicion24DTO>();
            List<MeMedicion24DTO> ListaGeneracionNoOpera = new List<MeMedicion24DTO>();
            List<MeMedicion24DTO> ListaDemanda = new List<MeMedicion24DTO>();
            List<EqEquipoDTO> ListaTrafo3d = new List<EqEquipoDTO>();
            List<EqEquipoDTO> ListaTrafo2d = new List<EqEquipoDTO>();
            List<EqEquipoDTO> ListaSvc = new List<EqEquipoDTO>();
            List<EqEquipoDTO> ListaLineas = new List<EqEquipoDTO>();
            List<EveManttoDTO> ListaManttos = new List<EveManttoDTO>();

            configuracion = string.Empty;
            validacionDuplicadoForeignKey = string.Empty;

            int siparcodiDigSdemanda = ConstantesMigraciones.SiparcodiDigsilentDemanda;
            string tiporelcodi = ConstantesMigraciones.TiporelcodiDigsilent, evenclasecodi = string.Empty;
            string propcodiDigsilentLinea = ConstantesMigraciones.PropcodiDigsilentLinea, propcodiDigsilentSvc = ConstantesMigraciones.PropcodiDigsilentSvc;
            string propcodiDigsilentTrafo2d = ConstantesMigraciones.PropcodiDigsilentTrafo2d, propcodiDigsilentTrafo3d = ConstantesMigraciones.PropcodiDigsilentTrafo3d;
            SiParametroDTO siparametros = this.GetByCriteriaSiparametro(siparcodiDigSdemanda);
            string propcodiDigsilentDemanda = (siparametros != null ? siparametros.Sipardescripcion : ConstantesMigraciones.PropcodiDigsilentDemanda);

            string famcodiLinea = ConstantesAppServicio.FamcodiLinea, famcodiSvc = ConstantesAppServicio.FamcodiSvc;
            string famcodiTran2d = ConstantesAppServicio.Famcoditrafo2d, famcodiTran3d = ConstantesAppServicio.Famcoditrafo3d;
            string famcodiDemanda = ConstantesAppServicio.FamcodiDemanda;

            int lectcodiOrigr = int.Parse(program);

            evenclasecodi = UtilCdispatch.GetEvenclasecodiByLectcodi(lectcodiOrigr).ToString();

            ListaEquirel = this.GetListaEquirelDigsilent(tiporelcodi);//Equirel
            ListaManttos = this.GetListaManttosDigsilent(evenclasecodi, fechaPeriodo);//Manttos
            if (rdchk == 0 || rdchk == 1)
            {
                ListaLineas = GetListaLineasDigsilent(propcodiDigsilentLinea, famcodiLinea); //Lineas
            }
            if (rdchk == 0 || rdchk == 2)
            {
                List<MeMedicion24DTO> listaM24Digsilent = this.GetListaGeneracionDIgSILENT(program, fuenteReprog, topcodiYupana, fechaPeriodo);
                ListaGeneracionNoOpera = listaM24Digsilent.Where(x => x.Meditotal == null).ToList();//Generacion No Opera
                ListaGeneracionOpera = listaM24Digsilent.Where(x => x.Meditotal != null).ToList(); //Generacion Opera

                configuracion = this.ReporteConfiguracionUnidadesOperaHtml(listaM24Digsilent);
                validacionDuplicadoForeignKey = this.VerificarDuplicadosForeignKey(listaM24Digsilent);
            }
            if (rdchk == 0 || rdchk == 3)
            {
                ListaTrafo2d = GetListaLineasDigsilent(propcodiDigsilentTrafo2d, famcodiTran2d);//Transformador 2d
                ListaTrafo3d = GetListaLineasDigsilent(propcodiDigsilentTrafo3d, famcodiTran3d);//Transformador 3d
            }
            if (rdchk == 0 || rdchk == 4)
            {
                ListaSvc = GetListaLineasDigsilent(propcodiDigsilentSvc, famcodiSvc); //Svc
            }
            if (rdchk == 0 || rdchk == 5)
            {
                ListaDemanda = GetListaDemandaDigsilent(propcodiDigsilentDemanda, famcodiDemanda, fechaPeriodo); //Demanda
            }

            //Procesar Horas
            StringBuilder result_ = new StringBuilder();
            StringBuilder coment_ = new StringBuilder();

            result_.AppendFormat("dole/dbupd/fkey<br>");
            result_.AppendFormat("cls/out<br>");
            result_.AppendFormat("ac/de all<br>");
            result_.AppendFormat("ac {0}.IntPrj<br>", this.GetFechaDigsilent(fechaPeriodo));
            result_.AppendFormat("cd {0} <br>", this.GetFechaDigsilent(fechaPeriodo));

            List<string> horas = bloq.Split(',').ToList();

            foreach (var h in horas)
            {

                result_.Append("ac " + h + "h.IntCase<br>");
                if (rdchk == 0 || rdchk == 1)
                {
                    #region lineas
                    List<EqEquipoDTO> lineas_ = new List<EqEquipoDTO>();

                    foreach (var l in ListaLineas)
                    {
                        if (l.Equicodi == 1073)
                        {

                        }
                        lineas_.AddRange(ListaDigSilentEqequipo((int.Parse(h) - 1), l, ListaManttos, ListaEquirel));
                    }

                    result_.Append("echo Cargando datos de disponibilidad de lineas...<br>set/def obj=ElmLne var=outserv<br>");
                    result_.Append(ArchivoDigsilentHtml(lineas_.OrderBy(x => x.Correlativo).ToList()));
                    #endregion
                }

                if (rdchk == 0 || rdchk == 2)
                {
                    #region generadores no operan
                    result_.Append("echo Cargando datos de generacion...<br>");
                    result_.Append("set/def obj=ElmGenstat var=pgini,outserv<br>");
                    result_.Append("set/def obj=ElmSym var=pgini,outserv<br>");

                    foreach (var d in ListaGeneracionNoOpera)
                    {
                        if (string.IsNullOrEmpty(d.Digsilent))
                            coment_.Append(d.Gruponomb + " " + d.Equiabrev + " Definicion Digsilent no existe<br>");
                        else
                            result_.Append("set/fkey obj=" + d.Digsilent + " val=0,1<br>");
                    }
                    #endregion

                    #region generadores operan

                    foreach (var agrupGrupoDespacho in ListaGeneracionOpera.GroupBy(x => x.Grupocodi))
                    {
                        int grupocodi = agrupGrupoDespacho.Key;
                        List<MeMedicion24DTO> listaGeneracionOperaXGrupo = agrupGrupoDespacho.ToList();

                        if (grupocodi == 961)
                        { }

                        try
                        {
                            this.ListaDigSilentPrgrupo(grupocodi, int.Parse(h) - 1, ref result_, ref coment_, listaGeneracionOperaXGrupo, ListaManttos);
                        }
                        catch (Exception ex)
                        { }

                    }
                    #endregion
                }

                if (rdchk == 0 || rdchk == 3)
                {
                    #region trafo 2d y 3d
                    List<EqEquipoDTO> trafo2d_ = new List<EqEquipoDTO>();
                    List<EqEquipoDTO> trafo3d_ = new List<EqEquipoDTO>();

                    //2D
                    result_.Append("echo Cargando datos de disponibilidad de transformadores...<br>set/def obj=ElmTr2 var=outserv<br>");
                    foreach (var l in ListaTrafo2d)
                    {
                        trafo2d_.AddRange(ListaDigSilentEqequipo((int.Parse(h) - 1), l, ListaManttos, ListaEquirel));
                    }
                    result_.Append(ArchivoDigsilentHtml(trafo2d_.OrderBy(x => x.Correlativo).ToList()));

                    //3D
                    result_.Append("set/def obj=ElmTr3 var=outserv<br>");
                    foreach (var l in ListaTrafo3d)
                    {
                        trafo3d_.AddRange(ListaDigSilentEqequipo((int.Parse(h) - 1), l, ListaManttos, ListaEquirel));
                    }
                    result_.Append(ArchivoDigsilentHtml(trafo3d_.OrderBy(x => x.Correlativo).ToList()));
                    #endregion
                }

                if (rdchk == 0 || rdchk == 4)
                {
                    #region svc
                    List<EqEquipoDTO> svc_ = new List<EqEquipoDTO>();

                    foreach (var l in ListaSvc)
                    {
                        svc_.AddRange(ListaDigSilentEqequipo(int.Parse(h), l, ListaManttos, ListaEquirel));
                    }

                    result_.Append("echo Cargando datos de disponibilidad de SVC...<br>set/def obj=ElmSvs var=outserv<br>");
                    result_.Append(ArchivoDigsilentHtml(svc_.OrderBy(x => x.Correlativo).ToList()));
                    #endregion
                }

                if (rdchk == 0 || rdchk == 5)
                {
                    #region demanda
                    decimal MW = 0, MVAR = 0;

                    result_.Append("echo Cargando datos de Demanda...<br>set/def obj=ElmFeeder var=i_scalepf,Qset,outserv<br>");

                    var listaEquipo = ListaDemanda.GroupBy(x => new { x.Equicodi, x.Digsilent }).Select(x => new { x.Key.Equicodi, x.Key.Digsilent }).ToList();

                    for (int m = 0; m < listaEquipo.Count; m++)
                    {
                        string digsilent_ = listaEquipo[m].Digsilent;
                        var det = ListaDemanda.Where(x => x.Equicodi == listaEquipo[m].Equicodi).ToList();
                        var regMW = det.Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMW).FirstOrDefault();
                        var regMVAR = det.Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMVAR).FirstOrDefault();

                        MW = regMW != null ? ((decimal?)regMW.GetType().GetProperty("H" + int.Parse(h).ToString()).GetValue(regMW, null)).GetValueOrDefault(0) : 0;
                        MVAR = regMVAR != null ? ((decimal?)regMVAR.GetType().GetProperty("H" + int.Parse(h).ToString()).GetValue(regMVAR, null)).GetValueOrDefault(0) : 0;
                        int valor = 0;
                        if (MW > 0 && MVAR != 0)
                        {
                            valor = 0;
                        }
                        else
                        {
                            if (MW == 0 && MVAR == 0)
                            {
                                valor = 1;
                            }
                            else
                            {
                                valor = 0;
                            }
                        }

                        result_.Append("set/fkey obj=" + digsilent_ + " val=3," + MW + ",1," + MVAR + "," + valor + "<br>");
                    }

                    if (ListaDemanda.Count == 0)
                    {
                        coment_.Append("Datos de demanda NO fueron cargados. Se obtienen de Demanda Típica<br>");
                    }

                    #endregion
                }
            }

            resultado = result_.ToString();
            comentario = coment_.ToString();
        }

        /// <summary>
        /// Obtener fecha en formato digsilent
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private string GetFechaDigsilent(DateTime fecha)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            DateTimeFormatInfo mfi = new DateTimeFormatInfo();
            string strMonthName = mfi.GetMonthName(fecha.Month).ToString(); //August
            string shortMonthName = strMonthName.Substring(0, 3); //Aug

            string strfecha = fecha.ToString("dd");
            strfecha += textInfo.ToTitleCase(shortMonthName.ToLower());
            strfecha += fecha.ToString("yyyy");

            return strfecha;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="equicodi_"></param>
        /// <returns></returns>
        private string VerificaChkLineas(int h, int equicodi_, List<EveManttoDTO> ListaManttos)
        {
            DateTime evenini_, evenfin_;
            DateTime horaini_, horafin_;
            foreach (var d in ListaManttos)
            {
                var equicodi = Convert.ToInt32(d.Equicodi);

                if (equicodi_ == equicodi)
                {
                    evenini_ = d.Evenini.Value;
                    evenfin_ = d.Evenfin.Value;

                    if (h == 23)
                    {
                        horaini_ = Convert.ToDateTime(evenini_.ToString("yyyy-MM-dd") + " " + h + ":00:00");
                        horafin_ = evenini_.AddDays(1);
                    }
                    else
                    {
                        horaini_ = Convert.ToDateTime(evenini_.ToString("yyyy-MM-dd") + " " + h + ":00:00");
                        horafin_ = Convert.ToDateTime(evenini_.ToString("yyyy-MM-dd") + " " + (h + 1) + ":00:00");
                    }

                    if (((evenini_ < horaini_) && (horaini_ < evenfin_)) || ((evenini_ < horafin_) && (horafin_ < evenfin_)))
                    {
                        return "1";
                    }
                }
            }

            return "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grupo_old"></param>
        /// <param name="filaini"></param>
        /// <param name="filafin"></param>
        /// <param name="bloq"></param>
        private void ListaDigSilentPrgrupo(int grupocodi, int bloq, ref StringBuilder strArchivoDigsilent, ref StringBuilder strObservacion, List<MeMedicion24DTO> listaM24, List<EveManttoDTO> listaManttos)
        {
            int famcodi = listaM24.First().Famcodi;
            int totalElementos = listaM24.Count();

            bool[] arr_Mantto = new bool[totalElementos];
            double cociente = 1.0;
            //if (grupo_old == 405) {cociente = 20.0;} //trujillo norte

            //indisponibilidad por gr
            for (int i = 0; i < totalElementos; i++)
            {
                if (!arr_Mantto[i])
                {
                    if (this.GetManttoEquipo(bloq, listaM24[i].Equicodi, listaM24[i].Equipadre, listaManttos))
                    {
                        arr_Mantto[i] = true;
                    }
                }
            }

            //reporta equipos sin datos completos
            bool procesagrupo = true;

            for (int i = 0; i < totalElementos; i++)
            {
                if (string.IsNullOrEmpty(listaM24[i].Digsilent))
                {
                    strObservacion.Append(listaM24[i].Gruponomb.Trim() + " " + listaM24[i].Equiabrev + " Definicion Digsilent no existe<br>");
                }

                if (listaM24[i].Minimo == null)
                {
                    strObservacion.Append(listaM24[i].Gruponomb.Trim() + " " + listaM24[i].Equiabrev + " Potencia minima no existe<br>");
                }

                if (listaM24[i].PotenciaEfectiva == null)
                {
                    strObservacion.Append(listaM24[i].Gruponomb.Trim() + " " + listaM24[i].Equiabrev + " Potencia efectiva no existe<br>");
                    strObservacion.Append("No se proceso " + listaM24[i].Gruponomb.Trim() + ". No tiene todas su P.Efectivas<br>");
                    // procesagrupo = false;
                }
                else
                {
                    if (listaM24[i].PotenciaEfectiva == 0)
                    {
                        strObservacion.Append("No se proceso " + listaM24[i].Gruponomb.Trim() + ". No tiene todas su P.Efectivas<br>");
                        //     procesagrupo = false;
                    }
                }
            }

            //reporta eq en mantenimiento
            //Los equipos que no tienen manto irán a la repartición de potencia
            List<MeMedicion24DTO> listaM24SinMantto = new List<MeMedicion24DTO>();
            for (int i = 0; i < totalElementos; i++)
            {
                if (arr_Mantto[i] || string.IsNullOrEmpty(listaM24[i].Digsilent))
                {
                    if (!string.IsNullOrEmpty(listaM24[i].Digsilent))
                    {
                        strArchivoDigsilent.Append("set/fkey obj=" + listaM24[i].Digsilent + " val=0,1<br>");
                    }

                    //si no tiene ForeignKey no se procesará
                }
                else
                {
                    listaM24SinMantto.Add(listaM24[i]);
                }
            }
            totalElementos = listaM24SinMantto.Count();

            //reparto
            if (procesagrupo && totalElementos > 0)
            {
                if (ConstantesHorasOperacion.IdTipoEolica != famcodi && ConstantesHorasOperacion.IdTipoSolar != famcodi
                    && ConstantesHorasOperacion.IdGeneradorEolica != famcodi && ConstantesHorasOperacion.IdGeneradorSolar != famcodi)
                    this.RepartirPotencia(bloq, cociente, ref strArchivoDigsilent, ref strObservacion, listaM24SinMantto);
                else
                    this.RepartirMVA(bloq, cociente, ref strArchivoDigsilent, ref strObservacion, listaM24SinMantto);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filaini"></param>
        /// <param name="filafin"></param>
        /// <param name="bloq"></param>
        /// <param name="arr_Mantto"></param>
        /// <param name="arr_Mantto_idx"></param>
        /// <param name="nro_elem"></param>
        /// <param name="cociente"></param>
        /// <param name="strArchivoDigsilent"></param>
        private void RepartirPotencia(int bloq, double cociente, ref StringBuilder strArchivoDigsilent, ref StringBuilder strObservacion, List<MeMedicion24DTO> listaM24)
        {
            MeMedicion24DTO regPrimerGen = listaM24[0];
            //El valor del H de medicion48 es el mismo para todos los equipos del grupo despacho
            decimal? va_ = (decimal?)regPrimerGen.GetType().GetProperty("H" + (bloq + 1).ToString()).GetValue(regPrimerGen, null);
            double valorH = Convert.ToDouble(va_);
            int nro_elem = listaM24.Count();

            if (nro_elem == 1)
            {
                if (va_ != null)
                {
                    if (valorH == 0)
                    {
                        strArchivoDigsilent.Append("set/fkey obj=" + regPrimerGen.Digsilent + " val=0,1<br>");
                    }
                    else
                    {
                        strArchivoDigsilent.Append("set/fkey obj=" + regPrimerGen.Digsilent + " val=" + Math.Round(valorH / cociente, 3).ToString() + ",0<br>");
                    }
                }
                else
                {
                    strArchivoDigsilent.Append("set/fkey obj=" + regPrimerGen.Digsilent + " val=" + Math.Round(valorH / cociente, 3).ToString() + ",0<br>");
                }
                return;
            }


            //sum Pmin
            double sumPmin = 0;//suma potencia minima
            double sumPef = 0;//suma potencia efectiva
            double sumRango = 0;

            for (int i = 0; i < nro_elem; i++)
            {
                if (listaM24[i].Minimo == null || listaM24[i].PotenciaEfectiva == null)
                    return;
                double pmin = Convert.ToDouble(listaM24[i].Minimo);
                double pmax = Convert.ToDouble(listaM24[i].PotenciaEfectiva);

                sumPmin += pmin;
                sumPef += pmax;
                sumRango += (pmax - pmin);
            }


            if (sumPmin > Convert.ToDouble(va_))
            {
                //parar a la unidad de menor potencia efectiva y/o en mantto
                strArchivoDigsilent.Append("set/fkey obj=" + listaM24[0].Digsilent + " val=0,1<br>");

                int nuevoNroElem = nro_elem - 1;
                if (nuevoNroElem > 0)
                {
                    List<MeMedicion24DTO> nuevaListaGeneracionOpera = listaM24.GetRange(nuevoNroElem >= 1 ? 1 : 0, nuevoNroElem);
                    this.RepartirPotencia(bloq, cociente, ref strArchivoDigsilent, ref strObservacion, nuevaListaGeneracionOpera);
                }
            }
            else
            {
                //reparto en ff. de potencia efectiva
                double prop_ = 0;
                double peff_ = 0;

                prop_ = valorH;

                //se comento esta seccion a requerimiento de SCO Jorge Cabrera 27.02.2020
                //if (sumPef < valorH)
                //{
                //    strObservacion.Append("Se ha programado la central con una potencia mayor a la potencia efectiva (" + regPrimerGen.Digsilent + "). Se reprogramará cada unidad a su potencia efectiva<br>");

                //    for (int i = 0; i < nro_elem; i++)
                //    {
                //        peff_ = Convert.ToDouble(listaM24[i].PotenciaEfectiva);

                //        if (peff_ == 0)
                //        {
                //            strArchivoDigsilent.Append("set/fkey obj=" + listaM24[i].Digsilent + " val=0,1<br>");
                //        }
                //        else
                //        {
                //            strArchivoDigsilent.Append("set/fkey obj=" + listaM24[i].Digsilent + " val=" + Math.Round(peff_ / cociente, 3).ToString() + ",0<br>");
                //        }
                //    }
                //}
                //else
                //{
                if (sumPmin >= 0) //2009-10-26
                {
                    prop_ -= sumPmin;

                    for (int i = 0; i < nro_elem; i++)
                    {
                        double _valor1 = Convert.ToDouble(listaM24[i].Minimo);
                        double _valor2 = Convert.ToDouble(listaM24[i].PotenciaEfectiva);

                        peff_ = _valor1 + (valorH - sumPmin) * (_valor2 - _valor1) / (sumRango * 1.0);

                        if (peff_ == 0)
                        {
                            strArchivoDigsilent.Append("set/fkey obj=" + listaM24[i].Digsilent + " val=0,1<br>");
                        }
                        else
                        {
                            strArchivoDigsilent.Append("set/fkey obj=" + listaM24[i].Digsilent + " val=" + Math.Round(peff_ / cociente, 3).ToString() + ",0<br>");
                        }
                    }
                    //}
                }
            }
        }

        /// <summary>
        /// Función para repartir Potencia Total por unidad (MVA) de las Centrales solares y eólicas
        /// </summary>
        /// <param name="bloq"></param>
        /// <param name="cociente"></param>
        /// <param name="strArchivoDigsilent"></param>
        /// <param name="strObservacion"></param>
        /// <param name="listaM24"></param>
        private void RepartirMVA(int bloq, double cociente, ref StringBuilder strArchivoDigsilent, ref StringBuilder strObservacion, List<MeMedicion24DTO> listaM24)
        {
            //El valor del H de medicion48 es el mismo para todos los equipos del grupo despacho
            int nro_elem = listaM24.Count();

            foreach (var reg in listaM24)
            {
                decimal? va_ = (decimal?)reg.GetType().GetProperty("H" + (bloq + 1).ToString()).GetValue(reg, null);
                double valorH = Convert.ToDouble(va_);

                if (va_ != null)
                {
                    if (valorH == 0)
                    {
                        strArchivoDigsilent.Append("set/fkey obj=" + reg.Digsilent + " val=0,1<br>");
                    }
                    else
                    {
                        strArchivoDigsilent.Append("set/fkey obj=" + reg.Digsilent + " val=" + Math.Round(valorH, 3).ToString() + ",0<br>");
                    }
                }
                else
                {
                    strArchivoDigsilent.Append("set/fkey obj=" + reg.Digsilent + " val=" + Math.Round(valorH, 3).ToString() + ",0<br>");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="bloq"></param>
        /// <returns></returns>
        private bool GetManttoEquipo(int h, int equicodi, int equipadre, List<EveManttoDTO> listaManttos)
        {
            DateTime _evenini, _evenfin;
            DateTime _horaini, _horafin;

            List<EveManttoDTO> subLista = listaManttos.Where(x => x.Equicodi == equicodi || x.Equicodi == equipadre).ToList();

            foreach (var d in subLista)
            {
                _evenini = Convert.ToDateTime(d.Evenini);
                _evenfin = Convert.ToDateTime(d.Evenfin);

                if (h == 23)
                {
                    _horaini = Convert.ToDateTime(_evenini.ToString("yyyy-MM-dd") + " " + h.ToString() + ":00:00");
                    _horafin = _evenini.AddDays(1);
                }
                else
                {
                    _horaini = Convert.ToDateTime(_evenini.ToString("yyyy-MM-dd") + " " + h.ToString() + ":00:00");
                    _horafin = Convert.ToDateTime(_evenini.ToString("yyyy-MM-dd") + " " + (h + 1).ToString() + ":00:00");
                }

                if (((_evenini < _horaini) && (_horaini < _evenfin)) || ((_evenini < _horafin) && (_horafin < _evenfin)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Lista DigSilent tabla Eqequipo
        /// </summary>
        /// <param name="h">bloque</param>
        /// <param name="l">EqEquipoDTO</param>
        /// <returns></returns>
        private List<EqEquipoDTO> ListaDigSilentEqequipo(int h, EqEquipoDTO l, List<EveManttoDTO> ListaManttos, List<EqEquirelDTO> ListaEquirel)
        {
            List<EqEquipoDTO> lista_ = new List<EqEquipoDTO>();

            string valor_ = VerificaChkLineas(h, l.Equicodi, ListaManttos);
            if (valor_ == "1")
            {
                l.Formuladat = valor_;
                l.Correlativo = h;
                lista_.Add(l);
            }
            else
            {
                var dat = ListaEquirel.Find(x => x.Equicodi1 == l.Equicodi);
                if (dat != null)
                {
                    valor_ = VerificaChkLineas(h, dat.Equicodi2, ListaManttos);
                    lista_.Add(new EqEquipoDTO()
                    {
                        Equicodi = dat.Equicodi2,
                        Valor = l.Valor,
                        Correlativo = h,
                        Formuladat = valor_
                    });
                }
                else
                {
                    dat = ListaEquirel.Find(x => x.Equicodi2 == l.Equicodi);
                    if (dat != null)
                    {
                        valor_ = VerificaChkLineas(h, dat.Equicodi1, ListaManttos);
                        lista_.Add(new EqEquipoDTO()
                        {
                            Equicodi = dat.Equicodi1,
                            Valor = l.Valor,
                            Correlativo = h,
                            Formuladat = valor_
                        });
                    }
                    else
                    {
                        l.Formuladat = valor_;
                        l.Correlativo = h;
                        lista_.Add(l);
                    }
                }
            }

            return lista_;
        }

        /// <summary>
        /// reporte
        /// </summary>
        /// <param name="listaGeneracionOpera"></param>
        /// <returns></returns>
        public string ReporteConfiguracionUnidadesOperaHtml(List<MeMedicion24DTO> listaGeneracionOpera)
        {
            var listaData = listaGeneracionOpera;
            if (listaGeneracionOpera.Count == 0) return string.Empty;

            StringBuilder str = new StringBuilder();

            //
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' width='auto' id='tablaConfOpera'>");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th colspan='8' style='width: 600px;'>Equipo</th>");
            str.Append("<th colspan='2' style='width: 220px; background: #ba80d8 !important'>Potencia mínima</th>");
            str.Append("<th colspan='2' style='width: 220px; background: #8829b9 !important' >Potencia máxima</th>");
            str.Append("<th colspan='1' style='width: 200px; background: #17b6bd !important'>DIgSILENT</th>");
            str.Append("</tr>");

            str.Append("<tr>");

            str.Append("<th style='width: 100px'>Empresa</th>");
            str.Append("<th style='width:  20px'>Cód. grupo<br>despacho</th>");
            str.Append("<th style='width:  70px'>Abrev. grupo</th>");
            str.Append("<th style='width:  20px'>Operó grupo<br>despacho</th>");
            str.Append("<th style='width:  20px'>Cód. pto<br>despacho</th>");
            str.Append("<th style='width: 100px'>Central</th>");
            str.Append("<th style='width:  20px'>Cód. eq</th>");
            str.Append("<th style='width:  70px'>Abrev. eq</th>");

            str.Append("<th style='width:  70px; background: #ba80d8 !important'>Fecha Vigencia</th>");
            str.Append("<th style='width:  40px; background: #ba80d8 !important'>Valor</th>");

            str.Append("<th style='width:  70px; background: #8829b9 !important'>Fecha Vigencia</th>");
            str.Append("<th style='width:  40px; background: #8829b9 !important'>Valor</th>");

            str.Append("<th style='width: 100px; background: #17b6bd !important;'>Foreign Key</th>");

            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");
            #region cuerpo

            foreach (var reg in listaData)
            {
                str.Append("<tr>");
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Emprnomb);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Grupocodi);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Grupoabrev);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Meditotal != null ? "<b>" + ConstantesAppServicio.SIDesc + "</b>" : ConstantesAppServicio.NODesc);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Ptomedicodi);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Central);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Equicodi > 0 ? reg.Equicodi + "" : "--");
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Equiabrev);

                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.FechapropequiMinDesc);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Minimo);

                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.FechapropequiPotefecDesc);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.PotenciaEfectiva);

                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Digsilent);

                str.Append("</tr>");
            }

            #endregion
            str.Append("</tbody>");

            str.Append("</table>");

            return str.ToString();
        }

        /// <summary>
        /// Obtener valores duplicados de Foreign Key
        /// </summary>
        /// <param name="listaM24Digsilent"></param>
        /// <returns></returns>
        public string VerificarDuplicadosForeignKey(List<MeMedicion24DTO> listaM24Digsilent)
        {
            string msj = string.Empty;

            var listaDuplicados = listaM24Digsilent.Where(x => x.Digsilent != null && x.Digsilent.Length > 0)
                .GroupBy(x => new { x.Digsilent })
                .Where(x => x.Count() >= 2)
                .Select(x => new { x.Key.Digsilent, Total = x.Count(), Desc = x.Key.Digsilent + "(" + x.Count() + ")" })
                .OrderBy(x => x.Digsilent).ToList();

            var listaSinDigsilent = listaM24Digsilent.Where(x => x.Digsilent == null || x.Digsilent.Length == 0).ToList();
            if (listaSinDigsilent.Any())
                msj += string.Format("Existe(n) {0} equipo(s) sin DIgSILENT Foreign Key", listaSinDigsilent.Count()) + ".";
            if (listaDuplicados.Any())
                msj += (msj.Length > 0 ? "<br>" : string.Empty) + "Existen varios equipos que tiene el mismo DIgSILENT Foreign Key: " + string.Join(", ", listaDuplicados.Select(x => x.Desc).ToList()) + ". ";

            return msj;
        }

        #region RDO-YUPANA

        /// <summary>
        /// Listar topologia por fecha
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public List<CpTopologiaDTO> ListarTopologiaXFecha(DateTime fechaConsulta)
        {
            CortoPlazoAppServicio servCP = new CortoPlazoAppServicio();

            //PROGRAMADO final
            CpTopologiaDTO regTopProg = FactorySic.GetCpTopologiaRepository().GetByFechaTopfinal(fechaConsulta.Date, ConstantesCortoPlazo.TopologiaDiario);
            int topcodiProg = regTopProg != null ? regTopProg.Topcodi : 0;
            List<CpReprogramaDTO> listaReprogramados = topcodiProg > 0 ? FactorySic.GetCpReprogramaRepository().GetByCriteria(topcodiProg) : new List<CpReprogramaDTO>();

            //ultimo preliminar
            int topcodiUltimoPrel = 0;
            CpTopologiaDTO regTop = FactorySic.GetCpTopologiaRepository().ObtenerUltimoEscenarioReprogramado(fechaConsulta.Date);
            if (regTop != null && regTop.Topfinal == 0) topcodiUltimoPrel = regTop.Topcodi;

            //Lista de Escenarios reprogramados del dia de consulta
            List<CpTopologiaDTO> lista = servCP.ListarEscenario(fechaConsulta.Date, fechaConsulta.Date, ConstantesCortoPlazo.TopologiaReprograma);

            var listaRsf = lista.Where(x => x.Topsinrsf > 0).ToList();
            var listaFinal = lista.Where(x => x.Topsinrsf == 0).ToList();

            foreach (CpTopologiaDTO rs in listaFinal)
            {
                var find = listaRsf.Find(x => x.Topsinrsf == rs.Topcodi && x.Topfinal == 1);
                if (find != null)
                {
                    rs.Topcodisinrsf = find.Topcodi;
                }
                else
                {
                    find = listaRsf.Find(x => x.Topsinrsf == rs.Topcodi);
                    if (find != null)
                    {
                        rs.Topcodisinrsf = find.Topcodi;
                    }
                }

                if (rs.Topfechadespacho == DateTime.MinValue) rs.Topfechadespacho = null;
                rs.EsUltimoPreliminar = rs.Topcodi == topcodiUltimoPrel;
                rs.EsOficial = listaReprogramados.Find(x => x.Topcodi2 == rs.Topcodi) != null;
            }

            return listaFinal.OrderBy(x => x.Topcodi).ToList();
        }

        /// <summary>
        /// Generar reporte html de topologias
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public string GenerarTablaHtmlReprograma(DateTime fechaConsulta)
        {
            List<CpTopologiaDTO> listaTopologia = ListarTopologiaXFecha(fechaConsulta);

            StringBuilder strHtml = new StringBuilder();
            strHtml.Append(@"
                <table id='tabla_topologia' border='1' class='pretty tabla-adicional' cellspacing='0' style='width: auto;'>
                    <thead>
                        <tr>
                            <th colspan='9'>Escenarios Yupana</th>
                            <th colspan='2'>Cargado en despacho</th>
                        </tr>
                        <tr>
                            <th>Seleccionar</th>
                            <th>Código</th>
                            <th>Oficial</th>
                            <th>Escenario</th>
                            <th>Fecha</th>
                            <th>Hora</th>
                            <th>Escenario sin RSF</th>
                            <th>Usuario modificación</th>
                            <th>Fecha modificación</th>

                            <th>Usuario</th>
                            <th>Fecha</th>
                        </tr>
                    </thead>
                    <tbody>
            ");

            foreach (var item in listaTopologia)
            {
                string esChecked = item.EsUltimoPreliminar ? "checked='checked'" : string.Empty;
                string esFileSelected = item.EsUltimoPreliminar ? "selected" : string.Empty;

                string esOficial = item.EsOficial ? ConstantesAppServicio.SIDesc : "-";
                string hora = item.EsOficial ? item.Topfecha.AddMinutes(30 * (item.Topiniciohora - 1)).ToString(ConstantesAppServicio.FormatoHora) : string.Empty;
                string sinRsf = item.Topcodisinrsf > 0 ? "Generado" : string.Empty;

                strHtml.AppendFormat(@"
                    <tr id='fila_esc_{0}' class='{12}'>
                        <td style='text-align: center;'> <input type='radio' name='cbTopcodi' value='{0}' {1}> </td>

                        <td style='text-align: center;'>{2}</td>
                        <td style='text-align: center;'>{3}</td> 
                        <td style=''>{4}</td>
                        <td style=''>{5}</td>
                        <td style=''>{6}</td>
                        <td style='text-align: center;'>{7}</td>
                        <td style='text-align: center;'>{8}</td>
                        <td style='text-align: center;'>{9}</td>

                        <td style='text-align: center;'>{10}</td>
                        <td style='text-align: center;'>{11}</td>

                    </tr>
                ", item.Topcodi, esChecked

                , item.Topcodi //2
                , esOficial
                , item.Topnombre
                , item.Topfecha.ToString(ConstantesAppServicio.FormatoFecha)
                , hora
                , sinRsf
                , item.Lastuser ?? "" //8
                , (item.Lastdate?.ToString(ConstantesAppServicio.FormatoFechaFull)) ?? ""

                , item.Topuserdespacho ?? ""
                , (item.Topfechadespacho?.ToString(ConstantesAppServicio.FormatoFechaFull)) ?? ""
                , esFileSelected
                );
            }

            strHtml.Append(@"
                    <tbody>
                </table>"
            );

            return strHtml.ToString();
        }

        #endregion

        #endregion

        #region Configuracion SP7 - Coes

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usu"></param>
        /// <returns></returns>
        public int ObtenerDatosSP7aCOES(string usuario)
        {
            List<CPointsDTO> canalesFromScada = FactorySic.GetCPointsRepository().List(); ;
            canalesFromScada = canalesFromScada.Where(x => x.PointType == "A" || x.PointType == "D").ToList();
            List<CPointsDTO> canalesFromScadaActivos = canalesFromScada.Where(x => x.Active == "T").ToList();
            List<CPointsDTO> canalesFromScadaInactivos = canalesFromScada.Where(x => x.Active == "F").ToList();
            int registros = 0;
            //usu = "InterfazSP7";
            //TrCanalSp7DTO
            //var ListaCpuntos = new List<CPointsDTO>();
            /*ListaCpuntos.Add(new CPointsDTO()
            {
                PointNumber = 1533094,
                PointName = "/CHLaVirg/138     /BARRA A /Frequ   /MvMoment",
                PointType = "A"
            });*/
            var ListaCanales = FactoryScada.GetTrCanalSp7Repository().List();

            foreach (var d in canalesFromScada)
            {
                try
                {
                    var det = ListaCanales.Find(x => x.Canalcodi == d.PointNumber);

                    if (det == null)
                    {
                        FactoryScada.GetTrCanalSp7Repository().Save(new TrCanalSp7DTO()
                        {
                            Canalcodi = d.PointNumber.Value,
                            Canalnomb = d.PointName,
                            PointType = d.PointType,
                            Lastuser = usuario,
                            Lastdate = DateTime.Now,
                            Canalfeccreacion = DateTime.Now,
                            Canalusucreacion = usuario,
                            Canalabrev = d.PointName.Split('/')[3]
                        });

                        registros++;
                    }
                    else
                    {
                        if (d.PointName != det.Canalnomb || d.PointType != det.PointType)
                        {
                            det.Canalnomb = d.PointName;
                            det.PointType = d.PointType;
                            det.Lastuser = usuario;
                            det.Lastdate = DateTime.Now;
                            det.Canalabrev = d.PointName.Split('/')[3].Trim();

                            FactoryScada.GetTrCanalSp7Repository().Update(det);
                            registros++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    registros = -1;
                }
            }

            return registros;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<CPointsDTO> GetListaCpoints()
        {
            return FactorySic.GetCPointsRepository().List();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TrEmpresaSp7DTO> ListaTrEmpresasp7()
        {
            return FactoryScada.GetTrEmpresaSp7Repository().List();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aor"></param>
        /// <param name="name"></param>
        /// <param name="siid"></param>
        public int InsertTrEmpresasp7(string aor, string name, string siid, string usu)
        {
            return FactoryScada.GetTrEmpresaSp7Repository().Save(new TrEmpresaSp7DTO() { Emprcodi = int.Parse(aor), Emprabrev = name, Emprenomb = name, Emprsiid = int.Parse(siid), Emprusucreacion = usu, Emprfeccreacion = DateTime.Now });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TrCanalSp7DTO> ListaTrCanalsp7(string canalcodi)
        {
            return FactoryScada.GetTrCanalSp7Repository().GetByIds(canalcodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="siid"></param>
        /// <param name="pathb"></param>
        /// <param name="usu"></param>
        /// <returns></returns>
        public void UpdateTrCanalsp7(string name, string siid, string pathb, string usu, string aor, string zona, string rdfid, string gisid)
        {
            if (pathb != null)
            {
                FactoryScada.GetTrCanalSp7Repository().Update(new TrCanalSp7DTO() { Canaliccp = name, Pathb = pathb, Lastuser = usu, Lastdate = DateTime.Now, Canalcodi = int.Parse(siid) });
            }
            else
            {
                FactoryScada.GetTrCanalSp7Repository().Update(new TrCanalSp7DTO() { Canalunidad = name, Lastuser = usu, Lastdate = DateTime.Now, Canalcodi = int.Parse(siid), Emprcodi = int.Parse(aor), Zonacodi = int.Parse(zona), Rdfid = rdfid, Gisid = int.Parse(gisid) });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TrZonaSp7DTO> ListaTrZonasp7()
        {
            return FactoryScada.GetTrZonaSp7Repository().List();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aor"></param>
        /// <param name="name"></param>
        /// <param name="siid"></param>
        /// <param name="descr"></param>
        /// <param name="zona"></param>
        /// <param name="usu"></param>
        public void InsertTrZonasp7(string aor, string name, string siid, string descr, string zona, string usu)
        {
            FactoryScada.GetTrZonaSp7Repository().Save(new TrZonaSp7DTO() { Zonaabrev = name, Zonanomb = descr, Emprcodi = int.Parse(aor), Zonasiid = int.Parse(siid), Zonausumodificacion = usu, Zonafecmodificacion = DateTime.Now, Zonacodi = int.Parse(zona) });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aor"></param>
        /// <param name="name"></param>
        /// <param name="siid"></param>
        /// <param name="descr"></param>
        /// <param name="zona"></param>
        /// <param name="usu"></param>
        public void UpdateTrZonasp7(string aor, string name, string siid, string descr, string zona, string usu)
        {
            FactoryScada.GetTrZonaSp7Repository().Update(new TrZonaSp7DTO() { Zonaabrev = name, Zonanomb = descr, Emprcodi = int.Parse(aor), Zonasiid = int.Parse(siid), Zonausumodificacion = usu, Zonafecmodificacion = DateTime.Now, Zonacodi = int.Parse(zona) });
        }
        #endregion

        #region Rol de Turnos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowsHead"></param>
        /// <param name="nFil"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public string[][] InicializacionMatriz(int rowsHead, int nFil, int nCol)
        {
            string[][] matriz = new string[nFil + rowsHead][];
            for (int i = 0; i < nFil + rowsHead; i++)
            {
                matriz[i] = new string[nCol];
                for (int j = 0; j < nCol; j++)
                {
                    matriz[i][j] = string.Empty;
                }
            }
            return matriz;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matriz"></param>
        /// <param name="ncol"></param>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <param name="listaPer"></param>
        /// <param name="lista"></param>
        public void GeneraExcelWebRolTurnos(string[][] matriz, string[][] matrizComment, int ncol, DateTime fecIni, DateTime fecFin, List<SiPersonaDTO> listaPer, List<SiRolTurnoDTO> Rols, List<SiRolTurnoDTO> listComment)
        {
            matriz[0] = new string[ncol];
            DateTime fechita = fecIni.AddDays(-1);

            matriz[0][0] = "";
            matriz[0][1] = "";
            matriz[0][2] = "";
            for (int x = 3; x < ncol; x++)
            {
                fechita = fechita.AddDays(1);
                matriz[0][x] = fechita.ToString("ddd", CultureInfo.GetCultureInfo("es-PE")).Substring(0, 1).ToUpper();
            }
            matriz[1][0] = "DNI";
            matriz[1][1] = "RESPONSABLE";
            matriz[1][2] = "";
            for (int x = 3; x < ncol; x++)
            {
                matriz[1][x] = (x - 2).ToString();
            }

            int i = 2;
            foreach (var d in listaPer)
            {
                matriz[i][0] = d.Perdni;
                matriz[i][1] = d.Pernomb;
                matriz[i][2] = d.Percodi.ToString();

                for (int y = 3; y < ncol; y++)
                {
                    var det = Rols.Where(x => x.Percodi == d.Percodi && x.Roltfecha.Day == (y - 2)).ToList(); //el ordenamiento viene de BD

                    var detComment = listComment.Where(x => x.Percodi == d.Percodi && x.Roltfecha.Day == (y - 2)).ToList();

                    if (detComment.Count > 1)
                    {
                        matrizComment[i][y] = "1";
                    }

                    if (det.Count > 0)
                    {
                        var det_ = det.First();
                        matriz[i][y] = (det_ != null ? det_.Actabrev : "");
                    }
                }
                i++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <param name="percodi"></param>
        /// <returns></returns>
        public List<SiRolTurnoDTO> ListaRols(DateTime fecIni, DateTime fecFin, string percodi)
        {
            return FactorySic.GetSiRolTurnoRepository().ListaRols(fecIni, fecFin, percodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listaPer"></param>
        public void FillListaPer(List<SiPersonaDTO> listaPer, ref List<SiRolTurnoDTO> Rols)
        {
            int i = 0;
            foreach (var d in listaPer)
            {
                var det = Rols.Find(x => x.Percodi == d.Percodi);
                if (det != null)
                {
                    det.Pernomb = d.Pernomb;
                    d.Perorden = i;
                }
                i++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="listaAct"></param>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <param name="usu"></param> 
        /// <returns></returns>
        public List<SiRolTurnoDTO> GetDataRols(string[][] data, List<SiRolTurnoDTO> ListaRolConsulta, List<SiActividadDTO> listaAct, DateTime fecIni, DateTime fecFin, string usu)
        {
            List<SiRolTurnoDTO> Rols = new List<SiRolTurnoDTO>();
            DateTime fechaActual = DateTime.Now;

            for (int x = 2; x < data.Length; x++)
            {
                DateTime f_ = fecIni.Date;
                for (int y = fecIni.Day; y <= fecFin.Day; y++)
                {
                    f_ = fecIni.Date.AddDays(-1).AddDays(y);
                    //ultimo turno que se muestra en web antes de los cambios del usuario
                    SiRolTurnoDTO rolTurno = ListaRolConsulta.Find(c => c.Percodi == Convert.ToInt32(data[x][2]) && c.Roltfecha == f_ && c.Roltestado == ConstantesAppServicio.SI);

                    SiActividadDTO actcodi = null;
                    if (!string.IsNullOrEmpty(data[x][y + 2]))
                    {
                        actcodi = listaAct.Find(c => c.Actabrev.Trim().ToUpper() == data[x][y + 2].Trim().ToUpper());
                    }

                    if (actcodi == null) //Cuando la persona no tiene actividad, solo actualizar los que tengan estado S
                    {
                        var listaRolTurno = ListaRolConsulta.Where(c => c.Percodi == Convert.ToInt32(data[x][2]) && c.Roltfecha == f_)
                            .Where(z => z.Roltestado == ConstantesAppServicio.SI).ToList();

                        foreach (var reg in listaRolTurno)
                        {
                            Rols.Add(new SiRolTurnoDTO()
                            {
                                Percodi = int.Parse(data[x][2]),
                                Pernomb = data[x][1],
                                Roltfecha = f_,
                                Actcodi = reg.Actcodi,
                                Lastuser = usu,
                                Lastdate = reg.Lastdate,
                                Roltfechaactualizacion = fechaActual,
                                Roltestado = ConstantesAppServicio.NO,
                                Tipoproceso = 2
                            });
                        }

                        continue;
                    };

                    if (rolTurno != null) //Cuando la persona tiene actividad
                    {
                        if (actcodi.Actcodi != rolTurno.Actcodi)
                        {
                            var listaRolTurno = ListaRolConsulta.Where(c => c.Percodi == Convert.ToInt32(data[x][2]) && c.Roltfecha == f_)
                            .Where(z => z.Roltestado == ConstantesAppServicio.SI).ToList();

                            foreach (var reg in listaRolTurno)
                            {
                                Rols.Add(new SiRolTurnoDTO()
                                {
                                    Percodi = int.Parse(data[x][2]),
                                    Pernomb = data[x][1],
                                    Roltfecha = f_,
                                    Actcodi = reg.Actcodi,
                                    Lastuser = usu,
                                    Lastdate = reg.Lastdate,
                                    Roltfechaactualizacion = fechaActual,
                                    Roltestado = ConstantesAppServicio.NO,
                                    Tipoproceso = 2
                                });
                            }

                            Rols.Add(new SiRolTurnoDTO()
                            {
                                Percodi = int.Parse(data[x][2]),
                                Pernomb = data[x][1],
                                Roltfecha = f_,
                                Actcodi = actcodi.Actcodi,
                                Lastuser = usu,
                                Lastdate = fechaActual,
                                Roltfechaactualizacion = fechaActual,
                                Roltestado = ConstantesAppServicio.SI,
                                Tipoproceso = 1
                            });
                        }
                    }
                    else
                    {
                        //No existe 
                        Rols.Add(new SiRolTurnoDTO()
                        {
                            Percodi = int.Parse(data[x][2]),
                            Pernomb = data[x][1],
                            Roltfecha = f_,
                            Actcodi = actcodi.Actcodi,
                            Lastuser = usu,
                            Lastdate = fechaActual,
                            Roltfechaactualizacion = fechaActual,
                            Roltestado = ConstantesAppServicio.SI,
                            Tipoproceso = 1
                        });

                    }
                }
            }

            return Rols;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListaMovi"></param>
        /// <returns></returns>
        public string ListaMovimientosHtml(List<SiRolTurnoDTO> data)
        {
            StringBuilder strHtml = new StringBuilder();

            data = data.OrderByDescending(x => x.Lastdate).ThenByDescending(x => x.Roltfechaactualizacion).ToList(); //ordenamiento de BD

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='pretty tabla-icono' id='tb_movimientos'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>Responsable</th>");
            strHtml.Append("<th>Fecha</th>");
            strHtml.Append("<th>Abreviatura</th>");
            strHtml.Append("<th>Descripcion</th>");
            strHtml.Append("<th>Ultimo Usuario</th>");
            strHtml.Append("<th>Ultima Actualizacion</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var list in data)
            {
                strHtml.Append("<tr>");
                strHtml.Append(string.Format("<td>{0}</td>", list.Pernomb));
                strHtml.Append(string.Format("<td>{0}</td>", list.Roltfecha.ToString(ConstantesAppServicio.FormatoFecha)));
                strHtml.Append(string.Format("<td>{0}</td>", list.Actabrev));
                strHtml.Append(string.Format("<td>{0}</td>", list.Actnomb));
                strHtml.Append(string.Format("<td>{0}</td>", list.Lastuser));
                strHtml.Append(string.Format("<td>{0}</td>", list.RoltfechaactualizacionDesc));
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("<tfoot><tr> <th id='0'>Responsable</th> <td></td> <td></td> <td></td> <td></td> <td></td></tr></tfoot>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <param name="listaPer"></param>
        /// <param name="Rols"></param>
        /// <param name="rutaFile"></param>
        public void GenerarArchivoExcelRols(DateTime fecIni, DateTime fecFin, List<SiPersonaDTO> listaPer, List<SiRolTurnoDTO> Rols, int areacodi, string rutaFile)
        {
            FileInfo newFile = new FileInfo(rutaFile);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Rol_de_Turno");
                ws = xlPackage.Workbook.Worksheets["Rol_de_Turno"];

                SiAreaDTO area = FactorySic.GetSiAreaRepository().GetById(areacodi);
                string titulo = "COES : " + area.Areaabrev + " - " + area.Areanomb;
                this.GeneraRptRols(ws, titulo, listaPer, Rols, fecIni, fecFin, 1, 2);

                xlPackage.Save();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="titulo"></param>
        /// <param name="nroColumn"></param>
        private void ExcelCabGeneral(ref ExcelWorksheet ws, ExcelPackage xlPackage, string nameWS, string titulo, int nroColumn)
        {
            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];
            ws.View.ShowGridLines = false;

            ws.Cells[1, 3].Value = titulo;

            var font = ws.Cells[1, 3].Style.Font;
            font.Size = 16;
            font.Bold = true;
            font.Name = "Calibri";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="ncol"></param>
        /// <param name="listaPer"></param>
        /// <param name="Rols"></param>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        private void GeneraRptRols(ExcelWorksheet ws, string titulo, List<SiPersonaDTO> listaPer, List<SiRolTurnoDTO> Rols, DateTime fecIni, DateTime fecFin, int filIni, int colIni)
        {
            #region titulo

            int filIniArea = filIni + 2;
            ws.Cells[filIniArea, colIni].Value = titulo;
            ws.Cells[filIniArea, colIni].Style.Font.Bold = true;

            ws.Cells[filIniArea + 1, colIni].Value = COES.Base.Tools.Util.ObtenerNombreMes(fecIni.Month) + "-" + fecIni.Year;
            ws.Cells[filIniArea + 1, colIni].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

            ws.Cells[filIniArea + 2, colIni].Value = DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaHora);
            ws.Cells[filIniArea + 2, colIni].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells[filIniArea + 2, colIni].Style.Font.Bold = true;

            #endregion

            #region cabecera

            TimeSpan ts = fecFin - fecIni;
            int totalDia = ts.Days;

            int sizeFont = 11;
            int filIniLetraDia = filIniArea + 3;
            int colIniLetraDia = colIni + 2;
            int colIniDni = colIni;
            int colIniResponsable = colIniDni + 1;
            int colFinLetraDia = colIniLetraDia + totalDia;
            this.formatoCabecera(ws, filIniLetraDia, colIniLetraDia, filIniLetraDia, colFinLetraDia, sizeFont);
            this.formatoCabecera(ws, filIniLetraDia + 1, colIniDni, filIniLetraDia + 1, colFinLetraDia, sizeFont);
            DateTime fechita = fecIni.AddDays(-1);

            for (int x = colIniLetraDia; x <= colFinLetraDia; x++)
            {
                fechita = fechita.AddDays(1);
                ws.Cells[filIniLetraDia, x].Value = fechita.ToString("ddd", CultureInfo.GetCultureInfo("es-PE")).Substring(0, 1).ToUpper();
            }
            ws.Cells[filIniLetraDia + 1, 2].Value = "DNI";
            ws.Cells[filIniLetraDia + 1, 3].Value = "RESPONSABLE";
            for (int x = colIniLetraDia; x <= colFinLetraDia; x++)
            {
                ws.Cells[filIniLetraDia + 1, x].Value = x - 3;
            }
            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            int rowIniData = filIniLetraDia + 2;
            int i = rowIniData;
            foreach (var d in listaPer)
            {
                ws.Cells[i, colIniDni].Value = d.Perdni;
                ws.Cells[i, colIniResponsable].Value = d.Pernomb;

                for (int y = colIniLetraDia; y <= colFinLetraDia; y++)
                {
                    var det = Rols.Find(x => x.Percodi == d.Percodi && x.Roltfecha.Day == (y - 3));
                    ws.Cells[i, y].Value = (det != null ? det.Actabrev : "");
                    ws.Cells[i, y].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells[i, y].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
                i++;
            }
            #endregion

            fechita = fecIni.AddDays(-1);

            ws.Column(colIniResponsable).Width = 25;
            for (int x = colIniLetraDia; x <= colFinLetraDia; x++)
            {
                ws.Column(x).Width = 9;

                fechita = fechita.AddDays(1);
                string letra = fechita.ToString("ddd", CultureInfo.GetCultureInfo("es-PE")).Substring(0, 1).ToUpper();
                if (letra == "S" || letra == "D")
                {
                    using (ExcelRange r1 = ws.Cells[rowIniData, x, listaPer.Count + rowIniData - 1, x])
                    {
                        r1.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        r1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        r1.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r1.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(220, 230, 242));
                        r1.Style.WrapText = true;
                    }
                }
            }

            this.borderCeldas(ws, rowIniData - 2, colIniDni + 1, rowIniData - 2, colFinLetraDia);
            this.borderCeldas(ws, rowIniData - 1, colIniDni, listaPer.Count + rowIniData - 1, colFinLetraDia);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        /// <param name="sizeFont"></param>
        public void formatoCabecera(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin, int sizeFont)
        {
            using (ExcelRange r1 = ws.Cells[rowIni, colIni, rowFin, colFin])
            {
                r1.Style.Font.Color.SetColor(Color.White);
                r1.Style.Font.Size = sizeFont;
                r1.Style.Font.Bold = true;
                r1.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                r1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                r1.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r1.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                r1.Style.WrapText = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="Ini"></param>
        /// <param name="Fin"></param>
        public void AddAutoWidthColumn(ExcelWorksheet ws, int Ini, int Fin)
        {
            for (int z = Ini; z <= Fin; z++) { ws.Column(z).AutoFit(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        public void borderCeldas(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin)
        {
            var borderTabla = ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border;
            borderTabla.Bottom.Style = borderTabla.Top.Style = borderTabla.Left.Style = borderTabla.Right.Style = ExcelBorderStyle.Hair;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Rols"></param>
        /// <returns></returns>
        public string ListaLeyendaAct(List<SiActividadDTO> data)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='pretty tabla-icono' id='tb_leyenda'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th></th>");
            strHtml.Append("<th>Leyenda</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var list in data)
            {
                strHtml.Append("<tr>");
                strHtml.Append(string.Format("<td>{0}</td>", list.Actabrev.ToUpper()));
                strHtml.Append(string.Format("<td>{0}</td>", list.Actnomb));
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        #endregion

        #region Informes para OSINERGMIN (Mantenimientos y Horas de Operación)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<EveManttoDTO> GetMantenimientos(DateTime fecha)
        {
            List<EveManttoDTO> Lista = new List<EveManttoDTO>();
            DateTime _dia = fecha.Date;
            int _id = 0;
            int _clase = 2;

            while (++_id <= 7)
            {
                _dia = _dia.AddDays(1);

                Lista.AddRange(this.wf_FillMantto(_dia, _dia.AddDays(1), _clase));

                if (_id == 1) { _clase++; }
                else if (_dia.DayOfWeek == DayOfWeek.Friday) { _clase++; }
            }

            foreach (var reg in Lista)
            {
                reg.TEOsinerg = (reg.Tareacodi >= 3 && reg.Tareacodi <= 6) ? "G" : "T";
            }

            Lista = Lista.OrderBy(x => x.Emprnomb).ThenBy(x => x.Areanomb).ThenBy(x => x.Equiabrev).ThenBy(x => x.Evenini).ToList();

            return Lista;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="evenclasecodi"></param>
        /// <returns></returns>
        private List<EveManttoDTO> wf_FillMantto(DateTime fechaini, DateTime fechafin, int evenclasecodi)
        {
            List<EveManttoDTO> Lista = new List<EveManttoDTO>();
            string evenclasedesc;

            switch (evenclasecodi)
            {
                case 1: evenclasedesc = "E"; break;
                case 2: evenclasedesc = "D"; break;
                case 3: evenclasedesc = "S"; break;
                case 4: evenclasedesc = "M"; break;
                default: evenclasedesc = "X"; break;
            }

            return FactorySic.GetEveManttoRepository().ListaMantenimientos25(evenclasecodi, evenclasedesc, fechaini, fechafin);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string InformacionP25Html(List<EveManttoDTO> data)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='pretty tabla-icono' id='tb_info'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>Empresa</th>");
            strHtml.Append("<th>Area</th>");
            strHtml.Append("<th>Equipo</th>");
            strHtml.Append("<th>Codigo Eq.</th>");
            strHtml.Append("<th style ='width: 120px'>Fecha Inicial</th>");
            strHtml.Append("<th style ='width: 120px'>Fecha Final</th>");
            strHtml.Append("<th>Tipo Evento</th>");
            strHtml.Append("<th>Descripcion</th>");
            strHtml.Append("<th>Tarea</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var list in data)
            {
                strHtml.Append("<tr>");
                strHtml.Append(string.Format("<td>{0}</td>", list.Emprnomb));
                strHtml.Append(string.Format("<td>{0}</td>", list.Areanomb));
                strHtml.Append(string.Format("<td>{0}</td>", list.Equiabrev));
                strHtml.Append(string.Format("<td>{0}</td>", list.Equicodi));
                strHtml.Append(string.Format("<td>{0}</td>", list.Evenini.Value.ToString(ConstantesAppServicio.FormatoFechaHoraAMPM)));
                strHtml.Append(string.Format("<td>{0}</td>", list.Evenfin.Value.ToString(ConstantesAppServicio.FormatoFechaHoraAMPM)));
                strHtml.Append(string.Format("<td>{0}</td>", list.Evenclaseabrev));
                strHtml.Append(string.Format("<td>{0}</td>", list.Evendescrip));
                strHtml.Append(string.Format("<td>{0}</td>", list.Tareacodi));
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string InformacionP25Calendario90Html(List<EveManttoDTO> data)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='pretty tabla-icono' id='tb_info'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>Empresa</th>");
            strHtml.Append("<th>Area</th>");
            strHtml.Append("<th>Equipo</th>");
            strHtml.Append("<th>Codigo Eq.</th>");
            strHtml.Append("<th style ='width: 120px'>Fecha Inicial</th>");
            strHtml.Append("<th style ='width: 120px'>Fecha Final</th>");
            strHtml.Append("<th>Tipo Evento</th>");
            strHtml.Append("<th>Descripcion</th>");
            strHtml.Append("<th>Tarea</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var list in data)
            {
                strHtml.Append("<tr>");
                strHtml.Append(string.Format("<td>{0}</td>", list.Emprnomb));
                strHtml.Append(string.Format("<td>{0}</td>", list.Areanomb));
                strHtml.Append(string.Format("<td>{0}</td>", list.Equiabrev));
                strHtml.Append(string.Format("<td>{0}</td>", list.Equicodi));
                strHtml.Append(string.Format("<td>{0}</td>", list.Evenini.Value.ToString(ConstantesAppServicio.FormatoFechaHoraAMPM)));
                strHtml.Append(string.Format("<td>{0}</td>", list.Evenfin.Value.ToString(ConstantesAppServicio.FormatoFechaHoraAMPM)));
                strHtml.Append(string.Format("<td>{0}</td>", list.Evenclaseabrev));
                strHtml.Append(string.Format("<td>{0}</td>", list.Evendescrip));
                strHtml.Append(string.Format("<td>{0}</td>", list.Tareacodi));
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// Listar el estado operativo (Horas de Operacion y los Mantenimientos) wf_generarEstadoOperativo, wf_FillManttoGT
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> GetEstadoOpe(DateTime fechaIni, DateTime fechaFin)
        {
            List<EveHoraoperacionDTO> lista = FactorySic.GetEveHoraoperacionRepository().ListaEstadoOperacion(fechaIni, fechaFin);

            foreach (var reg in lista)
            {
                SetearTextoCalificacionEstadoOperativo(reg);
            }

            return lista;
        }

        /// <summary>
        /// Listar el estado operativo (Horas de Operacion y los Mantenimientos) wf_generarEstadoOperativo, wf_FillManttoGT
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> GetEstadoOpe90(DateTime fechaIni, DateTime fechaFin)
        {
            List<EveHoraoperacionDTO> lista = FactorySic.GetEveHoraoperacionRepository().ListaEstadoOperacion90(fechaIni, fechaFin);

            foreach (var reg in lista)
            {
                SetearTextoCalificacionEstadoOperativo(reg);
            }

            return lista;
        }

        private void SetearTextoCalificacionEstadoOperativo(EveHoraoperacionDTO reg)
        {
            switch (reg.Subcausacodi)
            {
                case 101:
                case 102:
                case 103:
                case 104:
                    reg.Subcausadesc = ConstantesMigraciones.SubcausaReqSistema;
                    break;
                case 106: //POR PRUEBAS
                case 113: //POR REQUERIMIENTO PROPIO SUSTENTADO
                case ConstantesSubcausaEvento.SubcausaPorRestricOpTemporal: //POR RESTRICCION OPERATIVA TEMPORAL
                    reg.Subcausadesc = ConstantesMigraciones.SubcausaReqPropio;
                    break;
                case 114:
                    reg.Subcausadesc = ConstantesMigraciones.SubcausaPruebaAleat;
                    break;
                case 117:
                    reg.Subcausadesc = ConstantesMigraciones.SubcausaTension;
                    break;
                case 120:
                    reg.Subcausadesc = ConstantesMigraciones.SubcausaMantto;
                    break;
                default:
                    reg.Subcausadesc = ConstantesMigraciones.SubcausaNoDefinido;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string EstadoOperativoHtml(List<EveHoraoperacionDTO> data)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='pretty tabla-icono' id='tb_info'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>Empresa</th>");
            strHtml.Append("<th>Área</th>");
            strHtml.Append("<th>Equipo</th>");
            strHtml.Append("<th>Codigo Eq.</th>");
            strHtml.Append("<th>Fecha Inicial</th>");
            strHtml.Append("<th>Fecha Final</th>");
            strHtml.Append("<th>Código</th>");
            strHtml.Append("<th>Situación</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var list in data)
            {
                strHtml.Append("<tr>");
                strHtml.Append(string.Format("<td>{0}</td>", list.Emprnomb));
                strHtml.Append(string.Format("<td>{0}</td>", list.Areanomb));
                strHtml.Append(string.Format("<td>{0}</td>", list.Equiabrev));
                strHtml.Append(string.Format("<td>{0}</td>", list.Equicodi));
                strHtml.Append(string.Format("<td>{0}</td>", list.Hophorini.Value.ToString(ConstantesAppServicio.FormatoFechaFull2)));
                strHtml.Append(string.Format("<td>{0}</td>", list.Hophorfin.Value.ToString(ConstantesAppServicio.FormatoFechaFull2)));
                strHtml.Append(string.Format("<td>{0}</td>", list.Subcausacodi));
                strHtml.Append(string.Format("<td>{0}</td>", list.Subcausadesc));
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="rutaFile"></param>
        public void GenerarArchivoExcelInfoP25(List<EveManttoDTO> lista, DateTime fecha, string rutaFile)
        {
            FileInfo newFile = new FileInfo(rutaFile);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Mantenimientos");
                ws = xlPackage.Workbook.Worksheets["Mantenimientos"];

                this.GeneraRptInfoP25(ws, lista, fecha, "Mantenimiento Programado para los siguientes 7 dias", 2, 3);

                xlPackage.Save();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        private void GeneraRptInfoP25(ExcelWorksheet ws, List<EveManttoDTO> lista, DateTime fecha, string titulo, int filIni, int colIni)
        {
            #region Titulo

            ws.Cells[filIni, colIni].Value = titulo;
            ws.Cells[filIni, colIni].Style.Font.SetFromFont(new Font("Calibri", 12));
            ws.Cells[filIni, colIni].Style.Font.Bold = true;

            ws.Cells[filIni + 1, colIni + 1].Value = fecha.ToString(ConstantesBase.FormatoFechaPE);
            ws.Cells[filIni + 1, colIni + 1].Style.Font.SetFromFont(new Font("Calibri", 12));
            ws.Cells[filIni + 1, colIni + 1].Style.Font.Bold = true;
            ws.Cells[filIni + 1, colIni + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

            #endregion

            #region cabecera

            int colIniEmpresa = colIni;
            int colIniUbicacion = colIniEmpresa + 1;
            int colIniEquipo = colIniUbicacion + 1;
            int colIniInicio = colIniEquipo + 1;
            int colIniFinal = colIniInicio + 1;
            int colIniClase = colIniFinal + 1;
            int colIniDescripcion = colIniClase + 1;
            int colIniCodEq = colIniDescripcion + 1;
            int colIniTEOsinerg = colIniCodEq + 1;

            int rowIniEmpresa = colIni + 2;

            ws.Cells[rowIniEmpresa, colIniEmpresa].Value = "Empresa";
            ws.Cells[rowIniEmpresa, colIniUbicacion].Value = "Ubicación";
            ws.Cells[rowIniEmpresa, colIniEquipo].Value = "Equipo";
            ws.Cells[rowIniEmpresa, colIniInicio].Value = "Inicio";
            ws.Cells[rowIniEmpresa, colIniFinal].Value = "Final";
            ws.Cells[rowIniEmpresa, colIniClase].Value = "Clase";
            ws.Cells[rowIniEmpresa, colIniDescripcion].Value = "Descripción";
            ws.Cells[rowIniEmpresa, colIniCodEq].Value = "CodEq";
            ws.Cells[rowIniEmpresa, colIniTEOsinerg].Value = "TE_Osinerg";

            using (ExcelRange r1 = ws.Cells[rowIniEmpresa, colIniEmpresa, rowIniEmpresa, colIniTEOsinerg])
            {
                r1.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#000080"));
                r1.Style.Font.Size = 10;
                r1.Style.Font.Bold = true;
                r1.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                r1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                r1.Style.WrapText = true;
            }

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            int i = rowIniEmpresa + 1;
            foreach (var list in lista)
            {
                ws.Cells[i, colIniEmpresa].Value = list.Emprnomb.Trim();
                ws.Cells[i, colIniUbicacion].Value = list.Areanomb.Trim();
                ws.Cells[i, colIniEquipo].Value = list.Equiabrev;
                ws.Cells[i, colIniInicio].Value = list.Evenini.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
                ws.Cells[i, colIniInicio].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[i, colIniFinal].Value = list.Evenfin.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
                ws.Cells[i, colIniFinal].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[i, colIniClase].Value = list.Tipoevendesc;
                ws.Cells[i, colIniClase].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[i, colIniDescripcion].Value = list.Evendescrip;
                ws.Cells[i, colIniCodEq].Value = list.Equicodi;
                ws.Cells[i, colIniTEOsinerg].Value = list.TEOsinerg;
                ws.Cells[i, colIniTEOsinerg].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                i++;
            }
            #endregion

            using (var range = ws.Cells[rowIniEmpresa + 1, colIniEmpresa, i, colIniTEOsinerg])
            {
                range.Style.Font.Size = 11;
            }

            ws.Column(colIniEmpresa).Width = 51;
            ws.Column(colIniUbicacion).Width = 35;
            ws.Column(colIniEquipo).Width = 18;
            ws.Column(colIniInicio).Width = 19;
            ws.Column(colIniFinal).Width = 19;
            ws.Column(colIniClase).Width = 7;
            ws.Column(colIniDescripcion).Width = 32;
            ws.Column(colIniCodEq).Width = 10;
            ws.Column(colIniTEOsinerg).Width = 10;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="rutaFile"></param>
        public void GenerarArchivoExcelEstOpe(List<EveHoraoperacionDTO> lista, DateTime fecha, string rutaFile)
        {
            FileInfo newFile = new FileInfo(rutaFile);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                this.GeneraRptEstOpe(xlPackage, fecha, lista, "Estado_Operativo", "Estado Operativo de las Unidades Térmicas de los últimos 90 dias", 2, 3);

                xlPackage.Save();
            }
        }

        public void GenerarArchivoExcelEstOpe90(List<EveHoraoperacionDTO> lista, DateTime fecha, string rutaFile)
        {
            FileInfo newFile = new FileInfo(rutaFile);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                this.GeneraRptEstOpe(xlPackage, fecha, lista, "Estado_Operativo", "Estado Operativo de las Unidades Térmicas de los últimos 30 dias", 2, 3);

                xlPackage.Save();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        private void GeneraRptEstOpe(ExcelPackage xlPackage, DateTime fecha, List<EveHoraoperacionDTO> listaData, string nameWS, string titulo, int filIni, int colIni)
        {
            List<EveHoraoperacionDTO> lista = this.servHO.ListarReporteEstadoOperativoExcel(listaData);

            #region Titulo

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Cells[filIni, colIni].Value = titulo;
            ws.Cells[filIni, colIni].Style.Font.SetFromFont(new Font("Calibri", 12));
            ws.Cells[filIni, colIni].Style.Font.Bold = true;

            ws.Cells[filIni + 1, colIni + 1].Value = fecha.ToString(ConstantesBase.FormatoFechaPE);
            ws.Cells[filIni + 1, colIni + 1].Style.Font.SetFromFont(new Font("Calibri", 12));
            ws.Cells[filIni + 1, colIni + 1].Style.Font.Bold = true;
            ws.Cells[filIni + 1, colIni + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

            #endregion

            #region cabecera

            int colIniEmpresa = colIni;
            int colIniCentral = colIniEmpresa + 1;
            int colIniGrupo = colIniCentral + 1;
            int colIniEnParalelo = colIniGrupo + 1;
            int colIniFueraParalelo = colIniEnParalelo + 1;
            int colIniTipoOp = colIniFueraParalelo + 1;
            int colIniCodEq = colIniTipoOp + 1;

            int rowIniEmpresa = colIni + 2;

            ws.Cells[rowIniEmpresa, colIniEmpresa].Value = "Empresa";
            ws.Cells[rowIniEmpresa, colIniCentral].Value = "Central";
            ws.Cells[rowIniEmpresa, colIniGrupo].Value = "Grupo";
            ws.Cells[rowIniEmpresa, colIniEnParalelo].Value = "Inicio";
            ws.Cells[rowIniEmpresa, colIniFueraParalelo].Value = "Final";
            ws.Cells[rowIniEmpresa, colIniTipoOp].Value = "Situación";
            ws.Cells[rowIniEmpresa, colIniCodEq].Value = "CodEq";

            using (ExcelRange r1 = ws.Cells[rowIniEmpresa, colIniEmpresa, rowIniEmpresa, colIniCodEq])
            {
                r1.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#000080"));
                r1.Style.Font.Size = 10;
                r1.Style.Font.Bold = true;
                r1.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                r1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                r1.Style.WrapText = true;
            }

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            int i = rowIniEmpresa + 1;
            foreach (var list in lista)
            {
                ws.Cells[i, colIniEmpresa].Value = list.Emprnomb;
                ws.Cells[i, colIniCentral].Value = list.Areanomb;
                ws.Cells[i, colIniGrupo].Value = list.Equiabrev;
                ws.Cells[i, colIniEnParalelo].Value = list.Hophorini.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
                ws.Cells[i, colIniEnParalelo].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[i, colIniFueraParalelo].Value = list.Hophorfin.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
                ws.Cells[i, colIniFueraParalelo].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[i, colIniTipoOp].Value = list.Subcausadesc;
                ws.Cells[i, colIniCodEq].Value = list.Equicodi;

                i++;
            }
            #endregion

            using (var range = ws.Cells[rowIniEmpresa + 1, colIniEmpresa, i, colIniCodEq])
            {
                range.Style.Font.Size = 11;
            }

            ws.Column(colIniEmpresa).Width = 51;
            ws.Column(colIniCentral).Width = 35;
            ws.Column(colIniGrupo).Width = 13;
            ws.Column(colIniEnParalelo).Width = 19;
            ws.Column(colIniFueraParalelo).Width = 19;
            ws.Column(colIniTipoOp).Width = 32;
            ws.Column(colIniCodEq).Width = 7;
        }

        #endregion

        #region Restricciones Operativas

        /// <summary>
        /// Exportacion a archivo Excel de Restricciones Operativas
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="rutaFile"></param>
        public void GenerarArchivoExcelRptRestricOp(DateTime fecha, string rutaFile)
        {
            List<EveIeodcuadroDTO> listaRestric = this.GetListaBitacora(fecha, ConstantesAppServicio.SubcausacodiRestriccionesOpe, ConstantesAppServicio.ParametroDefecto);

            FileInfo newFile = new FileInfo(rutaFile);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("RestricOp");
                ws = xlPackage.Workbook.Worksheets["RestricOp"];
                ws.Cells.Style.Font.Name = "Arial";
                ws.Cells.Style.Font.Size = 8;

                this.GeneraRptRestricOp(ws, listaRestric, fecha, "COES SINAC", "Restricciones operativas", 2, 1);

                ws.View.ZoomScale = 125;

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Exportacion a archivo Excel de Restricciones Operativas
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        private void GeneraRptRestricOp(ExcelWorksheet ws, List<EveIeodcuadroDTO> lista, DateTime fecha, string tituloCoes, string titulo, int filIni, int colIni)
        {
            #region Titulo

            ws.Cells[filIni, colIni].Value = tituloCoes;
            ws.Cells[filIni, colIni].Style.Font.SetFromFont(new Font("Arial", 14));

            ws.Cells[filIni + 1, colIni].Value = fecha.ToString(ConstantesBase.FormatoFechaPE);
            ws.Cells[filIni + 1, colIni].Style.Font.Bold = true;

            ws.Cells[filIni + 2, colIni].Value = titulo;
            ws.Cells[filIni + 2, colIni].Style.Font.Bold = true;

            #endregion

            #region cabecera

            int colIniEmpresa = colIni;
            int colIniUbicacion = colIniEmpresa + 1;
            int colIniEquipo = colIniUbicacion + 1;
            int colIniInicio = colIniEquipo + 1;
            int colIniFinal = colIniInicio + 1;
            int colIniMotivo = colIniFinal + 1;

            int rowIniEmpresa = filIni + 3;

            ws.Cells[rowIniEmpresa, colIniEmpresa].Value = "Empresa";
            ws.Cells[rowIniEmpresa, colIniUbicacion].Value = "Ubicación";
            ws.Cells[rowIniEmpresa, colIniEquipo].Value = "Equipo";
            ws.Cells[rowIniEmpresa, colIniInicio].Value = "Inicio";
            ws.Cells[rowIniEmpresa, colIniFinal].Value = "Final";
            ws.Cells[rowIniEmpresa, colIniMotivo].Value = "Motivo";

            using (ExcelRange r1 = ws.Cells[rowIniEmpresa, colIniEmpresa, rowIniEmpresa, colIniMotivo])
            {
                r1.Style.Font.Bold = true;
            }
            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            int i = rowIniEmpresa + 1;
            foreach (var list in lista)
            {
                string horaIni = list.Ichorini != null ? list.Ichorini.Value.ToString(ConstantesAppServicio.FormatoFechaFull) : string.Empty;
                string horaFin = list.Ichorfin != null ? list.Ichorfin.Value.ToString(ConstantesAppServicio.FormatoHora) : string.Empty;
                horaFin = horaFin == "00:00" ? "24:00" : horaFin;
                ws.Cells[i, colIniEmpresa].Value = list.Emprabrev.Trim();
                ws.Cells[i, colIniUbicacion].Value = list.Areanomb.Trim();
                ws.Cells[i, colIniEquipo].Value = list.Equiabrev;
                ws.Cells[i, colIniInicio].Value = horaIni;
                ws.Cells[i, colIniFinal].Value = horaFin;
                ws.Cells[i, colIniMotivo].Value = list.Icdescrip1;

                i++;
            }
            #endregion

            using (var range = ws.Cells[rowIniEmpresa, colIniEmpresa, i - 1, colIniMotivo])
            {
                range.Style.Border.Bottom.Style = range.Style.Border.Top.Style = range.Style.Border.Left.Style = range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.WrapText = true;
            }

            ws.Column(colIniEmpresa).Width = 15;
            ws.Column(colIniUbicacion).Width = 25;
            ws.Column(colIniEquipo).Width = 30;
            ws.Column(colIniInicio).Width = 20;
            ws.Column(colIniFinal).Width = 10;
            ws.Column(colIniMotivo).Width = 85;
        }

        #endregion

        #region Generar IDCOS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecIni"></param>
        /// <returns></returns>
        public List<EveIeodcuadroDTO> GetListaBitacora(DateTime fecIni, string subcausacodi, string famcodi)
        {
            var lista = FactorySic.GetEveIeodcuadroRepository().ListaBitacora(fecIni, subcausacodi, famcodi);

            foreach (var reg in lista)
            {
                reg.Emprabrev = !string.IsNullOrEmpty(reg.Emprabrev) ? reg.Emprabrev : string.Empty;
            }

            return lista;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecIni"></param>
        /// <returns></returns>
        public List<EveEventoDTO> GetListaEventosImportantes(DateTime fecIni)
        {
            return FactorySic.GetEveEventoRepository().ListaEventosImportantes(fecIni);
        }

        /// <summary>
        /// Generación por tipo de generación de cada empresa (ejecutado y reprogramado)
        /// </summary>
        /// <param name="fecIni"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetListaGeneCentralSein(DateTime fecIni)
        {
            //Información de BD
            List<MeMedicion48DTO> listConEjec48 = this.servEjec.ListaDataMDGeneracionConsolidado48(fecIni, fecIni, ConstantesMedicion.IdTipogrupoCOES
                , ConstantesMedicion.IdTipoGeneracionTodos.ToString(), ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos, ConstantesMedicion.IdTipoRecursoTodos.ToString(), false
                , Int32.Parse(ConstantesAppServicio.LectcodiEjecutadoHisto));

            List<MeMedicion48DTO> listConReprog48 = this.servEjec.ListaDataMDGeneracionConsolidado48(fecIni, fecIni, ConstantesMedicion.IdTipogrupoCOES
                , ConstantesMedicion.IdTipoGeneracionTodos.ToString(), ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos, ConstantesMedicion.IdTipoRecursoTodos.ToString(), false
                , Int32.Parse(ConstantesAppServicio.LectcodiReprogDiario));

            List<MeMedicion48DTO> listConProg48 = this.servEjec.ListaDataMDGeneracionConsolidado48(fecIni, fecIni, ConstantesMedicion.IdTipogrupoCOES
                , ConstantesMedicion.IdTipoGeneracionTodos.ToString(), ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos, ConstantesMedicion.IdTipoRecursoTodos.ToString(), false
                , Int32.Parse(ConstantesAppServicio.LectcodiProgDiario));

            //Información final
            List<MeMedicion48DTO> listaEjec = listConEjec48, listaProg = listConReprog48;
            decimal? totalReprog = listConReprog48.Sum(x => x.Meditotal);
            if (totalReprog.GetValueOrDefault(0) == 0)
                listaProg = listConProg48;

            List<MeMedicion48DTO> lista48 = new List<MeMedicion48DTO>();
            lista48.AddRange(listaEjec);
            lista48.AddRange(listaProg);

            //Proceso
            List<MeMedicion48DTO> listaEmpresaXTgener = (from t in lista48
                                                         group t by new { t.Medifecha, t.Emprcodi, t.Tgenercodi }
                                                        into destino
                                                         select new MeMedicion48DTO()
                                                         {
                                                             Medifecha = destino.Key.Medifecha,
                                                             Emprcodi = destino.Key.Emprcodi,
                                                             Emprnomb = destino.First().Emprnomb,
                                                             Emprabrev = destino.First().Emprabrev,
                                                             Tgenercodi = destino.Key.Tgenercodi,
                                                             Tgenernomb = destino.First().Tgenernomb
                                                         }).ToList();

            listaEmpresaXTgener = listaEmpresaXTgener.OrderBy(x => x.Emprnomb).ThenBy(x => x.Tgenercodi).ToList();

            foreach (var regEmp in listaEmpresaXTgener)
            {
                var listaEjecXEmpyTgen = listaEjec.Where(x => x.Emprcodi == regEmp.Emprcodi && x.Tgenercodi == regEmp.Tgenercodi);
                decimal? totalEjecXTgen = listaEjecXEmpyTgen.Sum(x => x.Meditotal);

                var listaProgXEmpyTgen = listaProg.Where(x => x.Emprcodi == regEmp.Emprcodi && x.Tgenercodi == regEmp.Tgenercodi);
                decimal? totalProgXTgen = listaProgXEmpyTgen.Sum(x => x.Meditotal);

                regEmp.H1 = totalEjecXTgen.GetValueOrDefault(0) / 2.0m;
                regEmp.H2 = totalProgXTgen.GetValueOrDefault(0) / 2.0m;

                switch (regEmp.Tgenercodi)
                {
                    case ConstantesPR5ReportesServicio.TgenercodiHidro:
                        regEmp.Tgenernomb = ConstantesMigraciones.FamnombHidro;
                        break;
                    case ConstantesPR5ReportesServicio.TgenercodiTermo: regEmp.Tgenernomb = ConstantesMigraciones.FamnombTermo; break;
                    case ConstantesPR5ReportesServicio.TgenercodiEolica: regEmp.Tgenernomb = ConstantesMigraciones.FamnombEolic; break;
                    case ConstantesPR5ReportesServicio.TgenercodiSolar: regEmp.Tgenernomb = ConstantesMigraciones.FamnombSolar; break;
                }
            }

            //considerar las empresas que tengan data

            //Output
            return listaEmpresaXTgener;
        }

        /// <summary>
        /// Produccion x Empresa
        /// </summary>
        /// <param name="fecIni"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetListaProduccionXEmpresa(DateTime fecIni)
        {
            List<MeMedicion48DTO> listConEjec48 = this.servEjec.ListaDataMDGeneracionConsolidado48(fecIni, fecIni, ConstantesMedicion.IdTipogrupoCOES
                , ConstantesMedicion.IdTipoGeneracionTodos.ToString(), ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos, ConstantesMedicion.IdTipoRecursoTodos.ToString(), false
                , Int32.Parse(ConstantesAppServicio.LectcodiEjecutadoHisto));


            List<MeMedicion48DTO> listGenEmpresa = (from t in listConEjec48
                                                    group t by new { t.Medifecha, t.Emprcodi, t.Emprnomb }
                                                        into destino
                                                    select new MeMedicion48DTO()
                                                    {
                                                        Medifecha = destino.Key.Medifecha,
                                                        Emprcodi = destino.Key.Emprcodi,
                                                        Emprnomb = destino.Key.Emprnomb,
                                                        H1 = destino.Sum(t => t.H1),
                                                        H2 = destino.Sum(t => t.H2),
                                                        H3 = destino.Sum(t => t.H3),
                                                        H4 = destino.Sum(t => t.H4),
                                                        H5 = destino.Sum(t => t.H5),
                                                        H6 = destino.Sum(t => t.H6),
                                                        H7 = destino.Sum(t => t.H7),
                                                        H8 = destino.Sum(t => t.H8),
                                                        H9 = destino.Sum(t => t.H9),
                                                        H10 = destino.Sum(t => t.H10),

                                                        H11 = destino.Sum(t => t.H11),
                                                        H12 = destino.Sum(t => t.H12),
                                                        H13 = destino.Sum(t => t.H13),
                                                        H14 = destino.Sum(t => t.H14),
                                                        H15 = destino.Sum(t => t.H15),
                                                        H16 = destino.Sum(t => t.H16),
                                                        H17 = destino.Sum(t => t.H17),
                                                        H18 = destino.Sum(t => t.H18),
                                                        H19 = destino.Sum(t => t.H19),
                                                        H20 = destino.Sum(t => t.H20),

                                                        H21 = destino.Sum(t => t.H21),
                                                        H22 = destino.Sum(t => t.H22),
                                                        H23 = destino.Sum(t => t.H23),
                                                        H24 = destino.Sum(t => t.H24),
                                                        H25 = destino.Sum(t => t.H25),
                                                        H26 = destino.Sum(t => t.H26),
                                                        H27 = destino.Sum(t => t.H27),
                                                        H28 = destino.Sum(t => t.H28),
                                                        H29 = destino.Sum(t => t.H29),
                                                        H30 = destino.Sum(t => t.H30),

                                                        H31 = destino.Sum(t => t.H31),
                                                        H32 = destino.Sum(t => t.H32),
                                                        H33 = destino.Sum(t => t.H33),
                                                        H34 = destino.Sum(t => t.H34),
                                                        H35 = destino.Sum(t => t.H35),
                                                        H36 = destino.Sum(t => t.H36),
                                                        H37 = destino.Sum(t => t.H37),
                                                        H38 = destino.Sum(t => t.H38),
                                                        H39 = destino.Sum(t => t.H39),
                                                        H40 = destino.Sum(t => t.H40),

                                                        H41 = destino.Sum(t => t.H41),
                                                        H42 = destino.Sum(t => t.H42),
                                                        H43 = destino.Sum(t => t.H43),
                                                        H44 = destino.Sum(t => t.H44),
                                                        H45 = destino.Sum(t => t.H45),
                                                        H46 = destino.Sum(t => t.H46),
                                                        H47 = destino.Sum(t => t.H47),
                                                        H48 = destino.Sum(t => t.H48)

                                                    }).ToList();

            List<decimal> listaH;
            foreach (var reg in listGenEmpresa)
            {
                listaH = new List<decimal>();
                for (int i = 1; i <= 48; i++)
                {
                    decimal valor = ((decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null)).GetValueOrDefault(0);
                    listaH.Add(valor);
                }
                reg.Meditotal = listaH.Sum(x => x);
            }

            return listGenEmpresa;
        }

        /// <summary>
        /// Data de la produccion x empresa para grafico idcos
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetListaProduccionXEmpresaDataGrafico(List<MeMedicion48DTO> data)
        {
            var dataFinal = new List<MeMedicion48DTO>();
            var dataMayor2 = new List<MeMedicion48DTO>();
            var dataOtros = new List<MeMedicion48DTO>();

            data = data.OrderByDescending(x => x.Meditotal).ToList();
            decimal totalData = data.Sum(x => x.Meditotal.GetValueOrDefault(0));
            decimal totalOtros = 0;

            if (totalData > 0)
            {
                foreach (var reg in data)
                {
                    double prt = Math.Round(Convert.ToDouble(reg.Meditotal.Value) / (Convert.ToDouble(totalData) * 1.0) * 100 * 10) / 10.0;
                    if (prt < 2)
                    {
                        totalOtros += reg.Meditotal.GetValueOrDefault(0);
                    }
                    else
                    {
                        dataMayor2.Add(reg);
                    }
                }
            }

            dataFinal.AddRange(dataMayor2);

            if (totalOtros > 0)
            {
                MeMedicion48DTO m = new MeMedicion48DTO();
                m.Emprnomb = "OTROS";
                m.Meditotal = totalOtros;
                dataFinal.Add(m);
            }

            return dataFinal;
        }

        /// <summary>
        /// Diagrama de carga
        /// </summary>
        /// <param name="listaReporteInput"></param>
        /// <returns></returns>
        public GraficoWeb GraficoRecursosEnergeticosDiagramaCarga(List<MeMedicion48DTO> listaReporteInput)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            PR5ReportesAppServicio servPR5 = new PR5ReportesAppServicio();
            List<SiFuenteenergiaDTO> lista = servPR5.ListarFuenteEnergiaIDCOS();

            //Asignar orden y nombre
            foreach (var reg in listaReporteInput)
            {
                var regFE = lista.Find(x => x.Fenergcodi == reg.Fenergcodi);
                reg.Orden = regFE != null ? regFE.Fenergorden : 100;
                reg.Fenergnomb = regFE != null ? regFE.Fenergnomb : reg.Fenergnomb;
                reg.Fenergnomb = textInfo.ToTitleCase(reg.Fenergnomb.ToLower());
            }

            List<MeMedicion48DTO> listaReporte = listaReporteInput.OrderByDescending(x => x.Orden).ToList();
            decimal? valor;

            var grafico = new GraficoWeb();
            grafico.Series = new List<RegistroSerie>();
            grafico.SeriesType = new List<string>();
            grafico.SeriesName = new List<string>();
            grafico.YAxixTitle = new List<string>();
            grafico.SerieDataS = new DatosSerie[listaReporte.Count][];

            DateTime horas = new DateTime(2013, 9, 15, 0, 0, 0);
            for (int i = 0; i < 48; i++)
            {
                horas = horas.AddMinutes(30);
                grafico.SeriesName.Add(string.Format("{0:H:mm}", horas));
            }

            decimal total = listaReporte.Sum(x => x.Meditotal.GetValueOrDefault(0));

            foreach (var item in listaReporte)
            {
                RegistroSerie regSerie = new RegistroSerie();
                regSerie.Name = item.Fenergnomb;
                regSerie.Type = "area";
                regSerie.Color = item.Fenercolor;
                var listadata = new List<DatosSerie>();
                for (var j = 1; j <= 48; j++)
                {
                    valor = (decimal?)item.GetType().GetProperty(ConstantesAppServicio.CaracterH + j).GetValue(item, null);
                    listadata.Add(new DatosSerie() { Y = valor });
                }
                regSerie.Data = listadata;
                regSerie.Porcentaje = total > 0 ? item.Meditotal.GetValueOrDefault(0) / total : 0;
                grafico.Series.Add(regSerie);
            }

            grafico.TitleText = " DIAGRAMA DE CARGA POR TIPO DE RECURSO";
            if (listaReporte.Count > 0)
            {
                grafico.YaxixTitle = "MW";
                grafico.XAxisCategories = new List<string>();
                grafico.SeriesType = new List<string>();
                grafico.SeriesYAxis = new List<int>();
            }
            return grafico;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecIni"></param>
        /// <param name="valorMaximaDemanda"></param>
        /// <param name="fechaMaxDemanda"></param>
        /// <param name="porcentajeEjecProg"></param>
        /// <param name="factorCarga"></param>
        public void GetFactorCargaYMaximaDemanda(DateTime fecIni, out decimal valorMaximaDemanda, out DateTime fechaMaxDemanda, out decimal? porcentajeEjecProg, out decimal factorCarga)
        {
            //////////////////Data Generación ejecutada
            List<MeMedicion48DTO> listConEjec48 = this.servEjec.ListaDataMDGeneracionConsolidado48(fecIni, fecIni, ConstantesMedicion.IdTipogrupoCOES
                , ConstantesMedicion.IdTipoGeneracionTodos.ToString(), ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos, ConstantesMedicion.IdTipoRecursoTodos.ToString(), false
                , Int32.Parse(ConstantesAppServicio.LectcodiEjecutadoHisto));
            List<MeMedicion48DTO> listaDemandaGenEjec48 = this.servEjec.ListaDataMDGeneracionFromConsolidado48(fecIni, fecIni, listConEjec48);

            //Data Interconexion
            List<MeMedicion48DTO> listaInterconexion48 = this.servEjec.ListaDataMDInterconexion48(fecIni, fecIni);

            //Data Total
            List<MeMedicion48DTO> listaMedicionTotal48 = this.servEjec.ListaDataMDTotalSEIN48(listaDemandaGenEjec48, listaInterconexion48);

            //Parametros para calculo de MD
            List<SiParametroValorDTO> listaRangoNormaHP = servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoPeriodoHP);
            List<SiParametroValorDTO> listaBloqueHorario = servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroHPPotenciaActiva);

            //Hora de la maxima demanda
            MeMedicion48DTO dmdMaximaDia = listaMedicionTotal48.FirstOrDefault();
            MeMedicion48DTO dmdInterconexionDia = listaInterconexion48.FirstOrDefault();

            this.servEjec.GetDiaMaximaDemandaFromDataMD48(fecIni, fecIni, ConstantesRepMaxDemanda.TipoMDNormativa, listaMedicionTotal48, listaRangoNormaHP, listaBloqueHorario,
                                                        out fechaMaxDemanda, out DateTime fechaDia48, out int h);
            valorMaximaDemanda = (dmdMaximaDia != null) ? ((decimal?)dmdMaximaDia.GetType().GetProperty(ConstantesAppServicio.CaracterH + h.ToString()).GetValue(dmdMaximaDia, null)).GetValueOrDefault(0) : 0;
            //decimal valorInter = (dmdInterconexionDia != null) ? ((decimal?)dmdInterconexionDia.GetType().GetProperty(ConstantesAppServicio.CaracterH + h.ToString()).GetValue(dmdInterconexionDia, null)).GetValueOrDefault(0) : 0M;
            decimal valorMaximaDemandaEcuador = 0.0m; //El valor esta en 0 en el aplicativo SGOCOES

            /////////////////////Data Generación programada
            /*List<MeMedicion48DTO> listConProg48 = this.servEjec.ListaDataMDGeneracionConsolidado48(fecIni, fecIni, ConstantesMedicion.IdTipogrupoCOES
                , ConstantesMedicion.IdTipoGeneracionTodos.ToString(), ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos, ConstantesMedicion.IdTipoRecursoTodos.ToString(), false
                , Int32.Parse(ConstantesAppServicio.LectcodiProgDiario));
            List<MeMedicion48DTO> listaDemandaGenProg48 = this.servEjec.ListaDataMDGeneracionFromConsolidado48(fecIni, fecIni, listConProg48);*/

            MeMedicion48DTO dmdProgDia = dmdMaximaDia;
            decimal valorProgTotal = dmdProgDia != null ? dmdProgDia.Meditotal.GetValueOrDefault(0) : 0.0m;
            decimal valorProgTotalEcuador = 0.0m; //El valor esta en 0 en el aplicativo SGOCOES

            /////////////////////Data Generación Reprogramada
            List<MeMedicion48DTO> listConReProg48 = this.servEjec.ListaDataMDGeneracionConsolidado48(fecIni, fecIni, ConstantesMedicion.IdTipogrupoCOES
                , ConstantesMedicion.IdTipoGeneracionTodos.ToString(), ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos, ConstantesMedicion.IdTipoRecursoTodos.ToString(), false
                , Int32.Parse(ConstantesAppServicio.LectcodiReprogDiario));
            List<MeMedicion48DTO> listaDemandaGenReProg48 = this.servEjec.ListaDataMDGeneracionFromConsolidado48(fecIni, fecIni, listConReProg48);

            //Data programada Interconexion
            List<MeMedicion48DTO> listaInterconexionProg48 = this.servEjec.ListaDataDemandaProgramadaInterconexion48(fecIni, fecIni);

            MeMedicion48DTO dmdReprogDia = listaDemandaGenReProg48.FirstOrDefault();
            MeMedicion48DTO dmdInterconexionProgDia = listaInterconexion48.FirstOrDefault();

            decimal valorReprogDemanda = (dmdReprogDia != null) ? ((decimal?)dmdReprogDia.GetType().GetProperty(ConstantesAppServicio.CaracterH + h.ToString()).GetValue(dmdReprogDia, null)).GetValueOrDefault(0) : 0.0m;
            decimal valorProgInter = (dmdInterconexionProgDia != null) ? ((decimal?)dmdInterconexionProgDia.GetType().GetProperty(ConstantesAppServicio.CaracterH + h.ToString()).GetValue(dmdInterconexionProgDia, null)).GetValueOrDefault(0) : 0M;

            ////////////////////// ////////////////////// ////////////////////// ////////////////////// ////////////////////// 
            ////////////////////// Calculo de Factor de carga
            ////////////////////// ////////////////////// ////////////////////// ////////////////////// ////////////////////// 

            factorCarga = 0.0m;
            if (valorMaximaDemanda - valorMaximaDemandaEcuador != 0)
            {
                factorCarga = (valorProgTotal - valorProgTotalEcuador) / ((valorMaximaDemanda - valorMaximaDemandaEcuador) * 1.0m) / 24.0m / 2.0m;
                factorCarga = Math.Truncate(10000 * factorCarga) / 10000;
            }

            ////////////////////// ////////////////////// ////////////////////// ////////////////////// ////////////////////// 
            ////////////////////// Porcentaje
            ////////////////////// ////////////////////// ////////////////////// ////////////////////// ////////////////////// 

            porcentajeEjecProg = null;
            if (valorReprogDemanda - valorProgInter != 0)
            {
                porcentajeEjecProg = ((valorMaximaDemanda - valorMaximaDemandaEcuador) / ((valorReprogDemanda - valorProgInter) * 1.0m) - 1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecIni"></param>
        /// <param name="lectcodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="ptomedicodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetListaObtenerMedicion48(DateTime fecIni, DateTime fecFin, string lectcodi, int tipoinfocodi, string ptomedicodi)
        {
            return FactorySic.GetMeMedicion48Repository().GetByCriteria(fecIni, fecFin, lectcodi, tipoinfocodi, ptomedicodi);
        }

        /// <summary>
        /// Obtener datos de medicion48 para servicio web
        /// </summary>
        /// <param name="fecIni"></param>
        /// <param name="lectcodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="ptomedicodi"></param>
        /// <returns></returns>
        public List<Medicion48> GetListaObtenerMedicion48WS(DateTime fecIni, DateTime fecFin, string lectcodi, int tipoinfocodi, int fuente, int topcodi, string ptomedicodi)
        {
            List<Medicion48> l = new List<Medicion48>();

            var lista = this.ListarM48CdispatchYYupana(fecIni, Int32.Parse(lectcodi), tipoinfocodi, fuente, topcodi);

            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //StringBuilder jsonLista = new StringBuilder();
            //serializer.Serialize(lista, jsonLista);
            ////Debug.WriteLine("Json:\n"+ jsonLista.ToString());

            foreach (var reg in lista)
            {
                Medicion48 m = new Medicion48();
                m.Ptomedicodi = reg.Ptomedicodi;
                m.Tipoinfocodi = reg.Tipoinfocodi;
                m.Lectcodi = reg.Lectcodi;
                m.MedifechaStr = reg.Medifecha.ToString(ConstantesAppServicio.FormatoFecha);

                for (int i = 1; i <= 48; i++)
                {
                    decimal? valor = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(reg, null);
                    m.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(m, valor);
                }

                l.Add(m);
            }

            return l;
        }

        /// <summary>
        /// Obtener la información del Despacho cuando se exporta la data
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListarM48CdispatchYYupana(DateTime fecha, int lectcodi, int tipoinfocodi, int fuente, int topcodi)
        {
            bool reprogAprobado = true;
            var listaDespachoYupanaReprog = new List<MeMedicion48DTO>();
            if (ConstantesAppServicio.LectcodiReprogDiario == lectcodi.ToString() && fuente == 2)//Reprog Yupana y preliminar
            {
                CpTopologiaDTO regTop = topcodi > 0 ? FactorySic.GetCpTopologiaRepository().GetById(topcodi) : FactorySic.GetCpTopologiaRepository().ObtenerUltimoEscenarioReprogramado(fecha.Date);
                if (regTop != null)
                {
                    reprogAprobado = topcodi > 0 ? false : regTop.Topfinal != 0;

                    if (!reprogAprobado)
                    {
                        listaDespachoYupanaReprog = this.DespachoYupana(fecha, fecha, regTop.Topcodi, string.Empty, ConstantesBase.IdlectDespachoReprog, out List<MeMedicion48DTO> listaDespachoHidroXEq);
                        listaDespachoYupanaReprog = listaDespachoYupanaReprog.Where(x => x.Meditotal != 0).ToList();
                    }
                }
            }
            List<MeMedicion48DTO> listaMe48 = reprogAprobado ? this.GetObtenerHistoricoMedicion48(lectcodi, fecha, fecha) : listaDespachoYupanaReprog;

            //Calcular las columnas de Reserva, Totales
            this.Load_Dispatch(fecha, fecha, lectcodi, ConstantesAppServicio.ParametroDefecto, false, false, false, listaMe48, out CDespachoGlobal regCDespacho);

            switch (tipoinfocodi)
            {
                case ConstantesAppServicio.TipoinfocodiMVAR:
                    listaMe48 = regCDespacho.ListaAllMe48.Where(x => x.Medifecha == fecha && x.Tipoinfocodi != ConstantesAppServicio.TipoinfocodiMW).ToList();
                    break;
                case ConstantesAppServicio.TipoinfocodiMW:
                    listaMe48 = regCDespacho.ListaAllMe48.Where(x => x.Medifecha == fecha && x.Tipoinfocodi != ConstantesAppServicio.TipoinfocodiMVAR).ToList();
                    break;
            }

            return listaMe48;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecIni"></param>
        /// <param name="lectcodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="ptomedicodi"></param>
        /// <returns></returns>
        public List<MeMedicion1DTO> GetListaCostoTotalOpe(DateTime fecIni, string lectcodi, int tipoinfocodi, int ptomedicodi)
        {
            return FactorySic.GetMeMedicion1Repository().GetByCriteria(fecIni, fecIni, int.Parse(lectcodi), tipoinfocodi, ptomedicodi.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecIni"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> GetListaOperacionTension(DateTime fecIni)
        {
            return FactorySic.GetEveHoraoperacionRepository().ListaOperacionTension(fecIni);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <returns></returns>
        public List<EveIeodcuadroDTO> GetListaReqPropios(DateTime fecIni, DateTime fecFin)
        {
            return FactorySic.GetEveIeodcuadroRepository().ListaReqPropios(fecIni, fecFin);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecIni"></param>
        /// <returns></returns>
        public List<EveInterrupcionDTO> GetListaCalidadSuministro(DateTime fecIni)
        {
            return FactorySic.GetEveInterrupcionRepository().ListaCalidadSuministro(fecIni);
        }

        /// <summary>
        /// Reprogramaciones del día
        /// </summary>
        /// <param name="fecIni"></param>
        /// <returns></returns>
        public List<EveMailsDTO> GetListaReprogramado(DateTime fecIni)
        {
            List<EveMailsDTO> lista = FactorySic.GetEveMailsRepository().GetListaReprogramado(fecIni);

            foreach (var reg in lista)
            {
                reg.Mailfecha = reg.Mailfecha.Date.AddMinutes(reg.Mailbloquehorario.GetValueOrDefault(0) * 30);
            }

            return lista;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecIni"></param>
        /// <param name="subcausacodi"></param>
        /// <param name="evenclasecodi"></param>
        /// <returns></returns>
        public List<EveObservacionDTO> GetListaEveObservaciones(DateTime fecIni, string subcausacodi, string evenclasecodi)
        {
            return FactorySic.GetEveObservacionRepository().GetByCriteria(fecIni, subcausacodi, evenclasecodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecIni"></param>
        /// <param name="indiccodi"></param>
        /// <returns></returns>
        public List<FIndicadorDTO> GetListaIndicador(DateTime fecIni, string indiccodi)
        {
            return FactorySic.GetFIndicadorRepository().ListaIndicador(fecIni, indiccodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecIni"></param>
        /// <param name="indiccodi"></param>
        /// <returns></returns>
        public List<FIndicadorDTO> ListaIndicadorAcu(DateTime fecIni, string indiccodi)
        {
            return FactorySic.GetFIndicadorRepository().ListaIndicadorAcu(fecIni, indiccodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecIni"></param>
        /// <returns></returns>
        public List<PrLogsorteoDTO> GetListaLogSorteo(DateTime fecIni)
        {
            return FactorySic.GetPrLogsorteoRepository().ListaLogSorteo(fecIni);
        }

        /// <summary>
        /// Pruebas Aleatorias
        /// </summary>
        /// <param name="fecIni"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> GetListaPruebasAleatorias(DateTime fecIni)
        {
            List<EqEquipoDTO> lista = FactorySic.GetEqEquipoRepository().ListaPruebasAleatorias(fecIni);
            lista = lista.OrderBy(x => x.EMPRNOMB).ThenBy(x => x.Equinomb).ThenBy(x => x.Equiabrev).ToList();
            return lista;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecIni"></param>
        /// <param name="logtipo"></param>
        /// <returns></returns>
        public int GetTotalConteoTipo(DateTime fecIni, string logtipo)
        {
            return FactorySic.GetPrLogsorteoRepository().ConteoCorreoTipo(logtipo, fecIni);
        }

        /// <summary>
        /// Resumen Generacion de las centrales del sein
        /// </summary>
        /// <param name="listaGeneCentralSein"></param>
        public List<SiEmpresaDTO> ResumenGeneracionCentralesSein(List<MeMedicion48DTO> listaGeneCentralSein)
        {
            PR5ReportesAppServicio servPR5 = new PR5ReportesAppServicio();
            List<SiEmpresaDTO> listaEmpresaBD = servPR5.ListarEmpresasXID(listaGeneCentralSein.Select(x => x.Emprcodi).Distinct().ToList());
            List<SiEmpresaDTO> listaEmp = UtilAnexoAPR5.ListarEmpresaFromM48(listaGeneCentralSein, listaEmpresaBD);

            List<MeMedicion48DTO> listaTotalXTgen = new List<MeMedicion48DTO>();

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// Generación por Empresa
            foreach (var regEmp in listaEmp)
            {
                List<MeMedicion48DTO> listaTgenxEmp = listaGeneCentralSein.Where(x => x.Emprcodi == regEmp.Emprcodi).ToList();
                string descripEmp = string.Empty;
                int totalTgenXEmp = listaTgenxEmp.Count();
                int contador = 0;

                foreach (var regData48 in listaTgenxEmp)
                {
                    decimal totalEjec = regData48.H1.GetValueOrDefault(0);
                    decimal totalProg = regData48.H2.GetValueOrDefault(0);
                    decimal difXTgen = totalEjec - totalProg;

                    if (Math.Truncate(difXTgen * 10) / 10 != 0) { difXTgen = Math.Truncate(difXTgen * 10) / 10; }
                    else { difXTgen = Math.Truncate(difXTgen * 100) / 100; }

                    if (totalEjec != 0 || totalProg != 0)
                    {
                        string descSubGenero = contador == 0 ? "Subgeneró " : (contador != (totalTgenXEmp - 1) ? ", subgeneró " : " y subgeneró ");
                        string descSobreGenero = contador == 0 ? "Sobregeneró " : (contador != (totalTgenXEmp - 1) ? ", sobregeneró " : " y sobregeneró ");

                        if (difXTgen < 0) { descripEmp += descSubGenero + Math.Abs(Math.Round(difXTgen, 0)).ToString() + " MWh " + regData48.Tgenernomb; }
                        else { descripEmp += descSobreGenero + Math.Abs(Math.Round(difXTgen, 0)).ToString() + " MWh " + regData48.Tgenernomb; }

                        //Incluir data a Total
                        var regTotalxTgen = listaTotalXTgen.Find(x => x.Tgenercodi == regData48.Tgenercodi);
                        if (regTotalxTgen != null)
                        {
                            regTotalxTgen.H1 += totalEjec;
                            regTotalxTgen.H2 += totalProg;
                        }
                        else
                        {
                            regTotalxTgen = new MeMedicion48DTO() { Tgenercodi = regData48.Tgenercodi, Tgenernomb = regData48.Tgenernomb, H1 = totalEjec, H2 = totalProg };
                            listaTotalXTgen.Add(regTotalxTgen);
                        }
                    }

                    contador++;
                }

                if (descripEmp == string.Empty)
                {
                    descripEmp = "El CC " + regEmp.Emprabrev + " no envió su IDCC.";
                }
                else { descripEmp += "."; }

                regEmp.Descripcion = descripEmp;
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// Generación COES
            SiEmpresaDTO pie = new SiEmpresaDTO();
            pie.Emprnomb = "COES";
            pie.Emprabrev = "COES";
            string descripCoes = String.Empty;

            decimal totalProgCoes = listaTotalXTgen.Sum(x => x.H2).GetValueOrDefault(0);
            decimal totalEjecCoes = listaTotalXTgen.Sum(x => x.H1).GetValueOrDefault(0);
            decimal strResultado = 0;

            //La energía fue 1247.3MWh menos de la prevista.
            strResultado = totalEjecCoes - totalProgCoes;
            strResultado = Math.Truncate(strResultado * 10) / 10;
            if (strResultado != 0)
            {
                if (strResultado < 0)
                    descripCoes = "La energía fue " + Math.Abs(Math.Round(strResultado, 0)).ToString() + " MWh menos de la prevista.\r\n";
                else
                    descripCoes = "La energía fue " + Math.Abs(Math.Round(strResultado, 0)).ToString() + " MWh más de la prevista.\r\n";
            }

            //Se produjeron XX MWh térmicos más de lo programado.
            listaTotalXTgen = listaTotalXTgen.OrderBy(x => x.Tgenercodi).ToList();
            int contadorCoes = 0;
            foreach (var tgener in listaTotalXTgen)
            {
                strResultado = tgener.H1.GetValueOrDefault(0) - tgener.H2.GetValueOrDefault(0);
                strResultado = Math.Truncate(strResultado * 10) / 10;
                if (strResultado != 0)
                {
                    if (strResultado < 0)
                        descripCoes += (contadorCoes == 0 ? string.Empty : "\r\n") + "Se produjeron " + Math.Abs(Math.Round(strResultado, 0)).ToString() + " MWh " + tgener.Tgenernomb + " menos de lo programado.";
                    else
                        descripCoes += (contadorCoes == 0 ? string.Empty : "\r\n") + "Se produjeron " + Math.Abs(Math.Round(strResultado, 0)).ToString() + " MWh " + tgener.Tgenernomb + " más de lo programado.";
                    contadorCoes++;
                }
            }

            pie.Descripcion = descripCoes;

            listaEmp.Add(pie);

            return listaEmp;
        }

        /// <summary>
        /// Obtener el reporte de Coordinadores, Especialistas y Analistas de turno del día
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<List<string>> ListarItem16RolTurno(DateTime fecha)
        {
            List<SiPersonaDTO> listaPer = this.GetListaPersonalRol(ConstantesMigraciones.AreacodiSubdirCoord.ToString(), fecha, fecha);
            List<SiRolTurnoDTO> listaRolTurno = this.ListaRols(fecha, fecha, string.Join(",", listaPer.Select(x => x.Percodi).Distinct().ToList()))
                    .Where(x => x.Roltestado == ConstantesAppServicio.SI).ToList();
            foreach (var reg in listaRolTurno)
            {
                var p = listaPer.Find(x => x.Percodi == reg.Percodi);
                string nombre = p != null ? (!string.IsNullOrEmpty(p.Pernomb) ? p.Pernomb.Trim() : ConstantesMigraciones.RolTurnoNoExistente)
                    : ConstantesMigraciones.RolTurnoNoExistente;
                reg.Pernomb = nombre;
            }

            SiRolTurnoDTO c1 = listaRolTurno.Find(x => x.Actcodi == ConstantesMigraciones.ActcodiC1);
            SiRolTurnoDTO c2 = listaRolTurno.Find(x => x.Actcodi == ConstantesMigraciones.ActcodiC2);
            SiRolTurnoDTO c3 = listaRolTurno.Find(x => x.Actcodi == ConstantesMigraciones.ActcodiC3);
            string persc1 = c1 != null ? c1.Pernomb : ConstantesMigraciones.RolTurnoNoExistente;
            string persc2 = c2 != null ? c2.Pernomb : ConstantesMigraciones.RolTurnoNoExistente;
            string persc3 = c3 != null ? c3.Pernomb : ConstantesMigraciones.RolTurnoNoExistente;

            SiRolTurnoDTO e1 = listaRolTurno.Find(x => x.Actcodi == ConstantesMigraciones.ActcodiE1);
            SiRolTurnoDTO e2 = listaRolTurno.Find(x => x.Actcodi == ConstantesMigraciones.ActcodiE2);
            SiRolTurnoDTO e3 = listaRolTurno.Find(x => x.Actcodi == ConstantesMigraciones.ActcodiE3);
            string perse1 = e1 != null ? e1.Pernomb : ConstantesMigraciones.RolTurnoNoExistente;
            string perse2 = e2 != null ? e2.Pernomb : ConstantesMigraciones.RolTurnoNoExistente;
            string perse3 = e3 != null ? e3.Pernomb : ConstantesMigraciones.RolTurnoNoExistente;

            SiRolTurnoDTO a1 = listaRolTurno.Find(x => x.Actcodi == ConstantesMigraciones.ActcodiA1);
            SiRolTurnoDTO a2 = listaRolTurno.Find(x => x.Actcodi == ConstantesMigraciones.ActcodiA2);
            SiRolTurnoDTO a3 = listaRolTurno.Find(x => x.Actcodi == ConstantesMigraciones.ActcodiA3);
            string persa1 = a1 != null ? a1.Pernomb : ConstantesMigraciones.RolTurnoNoExistente;
            string persa2 = a2 != null ? a2.Pernomb : ConstantesMigraciones.RolTurnoNoExistente;
            string persa3 = a3 != null ? a3.Pernomb : ConstantesMigraciones.RolTurnoNoExistente;

            List<List<string>> lista = new List<List<string>>();

            //23:00 h - 07:00 h
            List<string> listaTurno1 = new List<string>();

            listaTurno1.Add("23:00 h - 07:00 h");
            listaTurno1.Add(persc1);
            listaTurno1.Add(perse1);
            listaTurno1.Add(persa1);

            //07:00 h - 15:00 h
            List<string> listaTurno2 = new List<string>();
            listaTurno2.Add("07:00 h - 15:00 h");
            listaTurno2.Add(persc2);
            listaTurno2.Add(perse2);
            listaTurno2.Add(persa2);

            //15:00 h - 23:00 h

            List<string> listaTurno3 = new List<string>();
            listaTurno3.Add("15:00 h - 23:00 h");
            listaTurno3.Add(persc3);
            listaTurno3.Add(perse3);
            listaTurno3.Add(persa3);

            //
            lista.Add(listaTurno1);
            lista.Add(listaTurno2);
            lista.Add(listaTurno3);

            return lista;
        }

        /// <summary>
        /// Numero del IEOD
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public string GetNumeroIeod(DateTime fecha)
        {
            DateTime ls_fecha_temp1 = Convert.ToDateTime(fecha.ToString("yyyy-MM-dd"));
            DateTime ls_fecha_temp2 = Convert.ToDateTime(fecha.Year + "-01-01");

            TimeSpan TS_Dif = ls_fecha_temp1.Subtract(ls_fecha_temp2);
            TS_Dif = TS_Dif.Add(new TimeSpan(24, 0, 0));

            string ls_cad = "000" + TS_Dif.Days.ToString();

            return ls_cad.Substring(ls_cad.Length - 3, 3);
        }

        /// <summary>
        /// 5. Regulación primaria (RPF) y secundaria (RSF) de frecuencia
        /// </summary>
        /// <param name="fecIni"></param>
        /// <param name="listaCabeceraFinal"></param>
        /// <param name="listaDataFinal"></param>
        public void GetRPFyRSF(DateTime fecIni, out List<string> listaCabeceraFinal, out List<List<string>> listaDataFinal)
        {
            RsfAppServicio servRsf = new RsfAppServicio();

            List<string> listaCabecera = new List<string>();
            List<List<string>> listaData = new List<List<string>>();

            int longitud = 0;
            List<int> indices = new List<int>();
            int count = 0;
            int columnas = 0;
            List<RsfLimite> limites = new List<RsfLimite>();
            string[][] datos = servRsf.ObtenerEstructura(fecIni, out longitud, out indices, true, out columnas, 0, out count, out limites);

            string[] cabecera = datos[0];
            int numCol = cabecera.Length;
            int numFil = datos.Length;

            if (cabecera.Length >= 4)
            {
                listaCabecera.Add(cabecera[3].Trim());
                listaCabecera.Add(cabecera[2].Trim());
            }
            for (int m = 4; m < numCol; m++)
            {
                string fechaHora = cabecera[m] != null ? cabecera[m].Trim() : string.Empty;
                fechaHora = fechaHora.Length >= 19 ? fechaHora.Substring(0, 5) + " - " + fechaHora.Substring(11, 5) : fechaHora;
                listaCabecera.Add(fechaHora);
            }

            for (int m = 1; m < numFil; m++)
            {
                List<string> listaFila = new List<string>();

                for (int n = 3; n < numCol; n++)
                {
                    if ((m == numFil - 2) && n < 4) //total
                    {
                        listaFila.Add(string.Empty);
                        listaFila.Add(datos[m][0].Trim());
                    }
                    else
                    {
                        //formateo de columnas
                        if (n == 3)
                        {
                            //URS, Central
                            listaFila.Add(datos[m][3].Trim());
                            listaFila.Add(datos[m][2].Trim());
                        }
                        else
                        {
                            if (m == numFil - 1)
                            {
                                //comentarios
                            }
                            else
                            {
                                if (datos[m][n] == null)
                                    listaFila.Add("FALTA DATO!");
                                else
                                    listaFila.Add(datos[m][n].Trim());
                            }
                        }
                    }
                }

                int numVacio = listaFila.Where(x => string.IsNullOrEmpty(x)).Count();
                if (listaFila.Count > 2 && numVacio != listaFila.Count - 2)
                {
                    listaData.Add(listaFila);
                }
            }

            listaCabeceraFinal = listaCabecera;
            listaDataFinal = listaData;
        }

        #endregion

        #region Reporte de Produccion CCO

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoinfocodi"></param>
        /// <returns></returns>
        public List<SiTipoinformacionDTO> GetListaTipoInfo(string tipoinfocodi)
        {
            return FactorySic.GetSiTipoinformacionRepository().GetByCriteria(tipoinfocodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public string CargarProduccionccoHtml(DateTime fecha, List<MeMedicion48DTO> lista)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//
            var empresas = lista.Select(x => x.Emprcodi).Distinct().ToList();
            var centrales = lista.Select(x => x.Equipadre).Distinct().ToList();
            strHtml.Append("<table class='pretty tabla-icono' id='tb_info'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th rowspan='2'>&nbsp;&nbsp;&nbsp;Fecha Hora&nbsp;&nbsp;&nbsp;</th>");
            strHtml.Append("<th rowspan='2'>&nbsp;&nbsp;&nbsp;&nbsp;Total&nbsp;&nbsp;&nbsp;&nbsp;</th>");
            foreach (var d in empresas)
            {
                var listaGrupo = lista.Where(x => x.Emprcodi == d).ToList();
                strHtml.Append("<th colspan='" + listaGrupo.Count + "'>" + listaGrupo.First().Emprnomb + "</th>");
            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            foreach (var d in empresas)
            {
                var listaGrupo = lista.Where(x => x.Emprcodi == d).ToList();
                foreach (var dd in listaGrupo)
                {
                    strHtml.Append("<th>" + dd.Grupoabrev + "</th>");
                }
            }
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            if (lista.Count > 0)
            {
                for (int i = 1; i <= 48; i++)
                {
                    fecha = fecha.AddMinutes(30);
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td>{0}</td>", fecha.ToString(ConstantesAppServicio.FormatoFechaHora)));
                    decimal valor_ = 0;

                    //Columna total
                    foreach (var m48 in lista)
                    {
                        decimal? valor = (decimal?)m48.GetType().GetProperty("H" + i).GetValue(m48, null);
                        if (valor != null) { valor_ += (decimal)valor; }
                    }
                    strHtml.Append(string.Format("<td>{0}</td>", valor_.ToString("N", nfi)));

                    //Valor de cada H por grupo
                    foreach (var d in empresas)
                    {
                        var listaGrupo = lista.Where(x => x.Emprcodi == d).ToList();
                        foreach (var dd in listaGrupo)
                        {
                            valor_ = 0;
                            var m48 = lista.Find(x => x.Grupocodi == dd.Grupocodi);
                            decimal? valor = (decimal?)m48.GetType().GetProperty("H" + i).GetValue(m48, null);
                            if (valor != null) { valor_ += (decimal)valor; }
                            strHtml.Append(string.Format("<td>{0}</td>", valor_.ToString("N", nfi)));
                        }
                    }
                    strHtml.Append("</tr>");
                }
            }
            else
            {
                strHtml.Append("<td colspan='2'>Sin informacion</td>");

            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecIni"></param>
        /// <param name="lectcodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetListaProduccioncco(DateTime fecIni, string lectcodi, int tipoinfocodi)
        {
            return servEjec.ListaDataGeneracion48(fecIni, fecIni, ConstantesAppServicio.TipogrupoCOES, ConstantesMedicion.IdTipoGeneracionTodos.ToString(),
                                                ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos,
                                                ConstantesMedicion.IdTipoRecursoTodos.ToString(), false, tipoinfocodi, Int32.Parse(lectcodi));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="rutaFile"></param>
        public void GenerarArchivoExcelProduccioncco(List<MeMedicion48DTO> lista, string rutaFile)
        {
            FileInfo newFile = new FileInfo(rutaFile);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;

                this.ExcelCabGeneral(ref ws, xlPackage, "Produccion CCO", "", 0);
                this.ExcelDetGeneralProduccioncco(ws, lista);
                ws.View.ShowGridLines = false;

                xlPackage.Save();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        private void ExcelDetGeneralProduccioncco(ExcelWorksheet ws, List<MeMedicion48DTO> lista)
        {
            int colIni = 2, colum = 2, sizeFont = 8, rowIni = 4, rowFin = 6;
            var empresas = lista.Select(x => x.Emprcodi).Distinct().ToList();
            var centrales = lista.Select(x => x.Equipadre).Distinct().ToList();
            var fechas = lista.GroupBy(x => x.Medifecha).ToList();

            #region cabecera

            int contCab = 0;
            ws.Cells[rowIni, colum].Value = "Fecha Hora";
            ExcelRange rg = ws.Cells[rowIni, colum, rowIni + 1, colum++];
            rg.Merge = true;
            rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            ws.Cells[rowIni, colum].Value = "Total";
            rg = ws.Cells[rowIni, colum, rowIni + 1, colum++];
            rg.Merge = true;
            rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            foreach (var d in empresas)
            {
                var listaGrupo = lista.Where(x => x.Emprcodi == d).ToList();
                if (listaGrupo.Count > 1)
                {
                    ExcelRange rg_2 = ws.Cells[rowIni, contCab + 4, rowIni, listaGrupo.Count + contCab + 3];
                    rg_2.Merge = true;
                    rg_2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                ws.Cells[rowIni, 4 + contCab].Value = listaGrupo.First().Emprnomb;

                contCab += listaGrupo.Count;
            }

            rowIni = 5;
            colum = 4;
            foreach (var d in empresas)
            {
                var listaGrupo = lista.Where(x => x.Emprcodi == d).ToList();
                foreach (var dd in listaGrupo)
                {
                    ws.Cells[rowIni, colum++].Value = dd.Grupoabrev;
                }
            }

            rowIni = 4;
            formatoCabecera(ws, rowIni, colIni, rowIni + 1, colum - 1, sizeFont);

            #endregion

            #region cuerpo

            for (int i = 1; i <= 48; i++)
            {
                colum = 2;

                if (i == 48) { ws.Cells[rowFin, colum++].Value = fechas[0].Key.AddMinutes(i * 30).AddMinutes(-1).ToString(ConstantesAppServicio.FormatoFechaHora); }
                else { ws.Cells[rowFin, colum++].Value = fechas[0].Key.AddMinutes(i * 30).ToString(ConstantesAppServicio.FormatoFechaHora); }
                decimal valor_ = 0;
                //Columna total
                foreach (var m48 in lista)
                {
                    decimal? valor = (decimal?)m48.GetType().GetProperty("H" + i).GetValue(m48, null);
                    if (valor != null) { valor_ += (decimal)valor; }
                }
                ws.Cells[rowFin, colum].Value = valor_;
                ws.Cells[rowFin, colum++].Style.Numberformat.Format = "0.000";

                //Valor de cada H por grupo
                foreach (var d in empresas)
                {
                    var listaGrupo = lista.Where(x => x.Emprcodi == d).ToList();
                    foreach (var dd in listaGrupo)
                    {
                        valor_ = 0;
                        var m48 = lista.Find(x => x.Grupocodi == dd.Grupocodi);
                        decimal? valor = (decimal?)m48.GetType().GetProperty("H" + i).GetValue(m48, null);
                        if (valor != null) { valor_ += (decimal)valor; }
                        ws.Cells[rowFin, colum].Value = valor_;
                        ws.Cells[rowFin, colum++].Style.Numberformat.Format = "0.000";
                    }
                }
                rowFin++;
            }

            #endregion

            #region estilo rpt

            using (var range = ws.Cells[rowIni, colIni, rowFin - 1, colum - 1])
            {
                range.Style.Font.Size = sizeFont;
            }

            ws.Column(2).Width = 12;
            this.AddAutoWidthColumn(ws, colIni, colum - 1);
            borderCeldas(ws, rowIni, colIni, rowFin - 1, colum - 1);

            #endregion
        }

        #endregion

        #region Dato Minuto SP7

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TrEmpresaSp7DTO> ListTrEmpresaSp7s()
        {
            return (new ScadaSp7AppServicio()).ListTrEmpresaSp7s();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public List<TrZonaSp7DTO> GetListaTrZonasp7(int emprcodi)
        {
            return FactoryScada.GetTrZonaSp7Repository().GetByCriteria(emprcodi.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoinfocodi"></param>
        /// <returns></returns>
        public SiTipoinformacionDTO GetTipoinformacionByTipoinfocodi(int tipoinfocodi)
        {
            return (new MedidoresAppServicio()).GetTipoinformacionByTipoinfocodi(tipoinfocodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canalpointtype"></param>
        /// <param name="zonacodi"></param>
        /// <param name="canalunidad"></param>
        /// <returns></returns>
        public List<TrCanalSp7DTO> ListTrCanalSp7sByZonaAndUnidad(string canalpointtype, int zonacodi, string canalunidad)
        {
            return (new ScadaSp7AppServicio()).ListTrCanalSp7sByZonaAndUnidad(canalpointtype, -1, zonacodi, canalunidad);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public string ListarCanalesHtml(List<TrCanalSp7DTO> lista)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//
            strHtml.Append("<table class='pretty tabla-icono' id='tb_info'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>Codigo</th>");
            strHtml.Append("<th>ICCP</th>");
            strHtml.Append("<th>Abrev.</th>");
            strHtml.Append("<th>Nombre</th>");
            strHtml.Append("<th>Unidad</th>");
            strHtml.Append("<th><input type='checkbox' onchange='checkAll(this)' /></th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody id='tbSeleccionados'>");

            foreach (var d in lista)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td>" + d.Canalcodi + "</td>");
                strHtml.Append("<td>" + d.Canaliccp + "</td>");
                strHtml.Append("<td>" + d.Canalabrev + "</td>");
                strHtml.Append("<td>" + d.Canalnomb + "</td>");
                strHtml.Append("<td>" + d.Canalunidad + "</td>");
                strHtml.Append("<td><input type='checkbox' id='" + d.Canalcodi + "' /></td>");
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f_"></param>
        /// <param name="canalcodi"></param>
        /// <returns></returns>
        public List<MeScadaSp7DTO> ListTrScadasp7(DateTime f_, string canalcodi)
        {
            return FactoryScada.GetMeScadaSp7Repository().GetByCriteria(f_, f_, canalcodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public string ListarTrScadasp7Html(List<MeScadaSp7DTO> lista, List<TrCanalSp7DTO> cabecera)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";


            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//
            var canales = lista.GroupBy(x => x.Canalcodi).ToList();
            strHtml.Append("<table class='pretty tabla-icono' id='tb_info'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>HORA</th>");
            int cc = 1;
            foreach (var d in canales)
            {
                var det = cabecera.Find(x => x.Canalcodi == d.Key);
                strHtml.Append("<th>" + det.Canalnomb + "</th>");
                strHtml.Append("<th>Calidad" + cc + "</th>");
                cc++;
            }
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");
            var fecha = lista.First().Medifecha.Date;

            for (int i = 1; i <= 96; i++)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td>" + fecha.AddMinutes(i * 15).ToString("HH:mm") + "</td>");
                foreach (var d in canales)
                {
                    var m96 = lista.Find(x => x.Canalcodi == d.Key);
                    decimal? valor = (decimal?)m96.GetType().GetProperty("H" + i).GetValue(m96, null);
                    strHtml.Append("<td>" + (valor != null ? ((decimal)valor).ToString("N", nfi) : "0") + "</td>");
                    strHtml.Append("<td>" + (m96 != null ? GetCanalCalidad(m96.Canalcalidad) : "") + "</td>");
                }
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        private string GetCanalCalidad(int canalcalidad)
        {
            if ((canalcalidad >= 0 && canalcalidad <= 15) || (canalcalidad >= 64 && canalcalidad <= 79) || (canalcalidad >= 128 && canalcalidad <= 143) || (canalcalidad >= 192 && canalcalidad <= 207)) { return "VALIDO"; }
            if ((canalcalidad >= 16 && canalcalidad <= 31) || (canalcalidad >= 80 && canalcalidad <= 95) || (canalcalidad >= 144 && canalcalidad <= 159) || (canalcalidad >= 208 && canalcalidad <= 223)) { return "CONGELADO"; }
            if ((canalcalidad >= 32 && canalcalidad <= 47) || (canalcalidad >= 96 && canalcalidad <= 111) || (canalcalidad >= 160 && canalcalidad <= 175) || (canalcalidad >= 224 && canalcalidad <= 239)) { return "INDETERMINADO"; }

            return "NO VALIDO";
        }

        #endregion

        #region Publicacion comunicados

        /// <summary>
        /// Lista tabla WbComunicados
        /// </summary>
        /// <returns></returns>
        public List<WbComunicadosDTO> GetListaComunicados()
        {
            return FactorySic.GetWbComunicadosRepository().List();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comcodi"></param>
        public void EliminarWbComunicados(int comcodi)
        {
            FactorySic.GetWbComunicadosRepository().Delete(comcodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wbComunicadosDTO"></param>
        public int InsertarWbComunicados(WbComunicadosDTO obj)
        {
            return FactorySic.GetWbComunicadosRepository().Save(obj);
        }

        /// <summary>
        /// Actualizar tabla WbComunicados
        /// </summary>
        /// <param name="reg"></param>
        public void ActualizarWbComunicados(WbComunicadosDTO reg)
        {
            FactorySic.GetWbComunicadosRepository().Update(reg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ruta"></param>
        /// <param name="typ"></param>
        /// <returns></returns>
        public string ListaComunicadosHtml(List<WbComunicadosDTO> data, string ruta, int typ)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='pretty tabla-icono' id='tb_info" + typ + "'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            if (typ == 1)
            {
                strHtml.Append("<th>Orden</th>");
            }
            strHtml.Append("<th>Fecha</th>");
            strHtml.Append("<th>Titulo</th>");
            strHtml.Append("<th>Descripcion</th>");
            strHtml.Append("<th>Link</th>");
            strHtml.Append("<th>Inicio</th>");
            strHtml.Append("<th>Posicion</th>");
            strHtml.Append("<th>Acciones</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var list in data)
            {
                strHtml.Append("<tr id='" + list.Comorden + "'>");
                if (typ == 1)
                {
                    strHtml.Append(string.Format("<td>{0}</td>", list.Comorden));
                }
                strHtml.Append(string.Format("<td>{0}</td>", list.Comfecha.Value.ToString(ConstantesAppServicio.FormatoFecha)));
                strHtml.Append(string.Format("<td>{0}</td>", list.Comtitulo.Trim()));
                strHtml.Append(string.Format("<td>{0}</td>", list.Comdesc.Trim()));
                strHtml.Append("<td>" + (list.Comlink != null ? list.Comlink.Trim() : "") + "</td>");
                strHtml.Append(string.Format("<td>{0}</td>", list.Comfechaini.Value.ToString(ConstantesAppServicio.FormatoFecha)));
                strHtml.Append("<td><input type='checkbox' " + (list.Composition != null ? (list.Composition == 1 ? "checked" : "") : "") + " onclick='edit_(" + list.Comcodi + ",1,\"" + (list.Composition != null ? (list.Composition == 1 ? "checked" : "") : "") + "\");' />");
                strHtml.Append(string.Format(
                    "<td><a href='#' onclick='edit_(" + list.Comcodi + ",2,\"\");'><img src='{0}Content/Images/btn-edit.png' /></a>&nbsp;<a href='#' onclick='delete_(" + list.Comcodi + ");'><img src='{0}Content/Images/Trash.png' /></a></td>"
                    , ruta));
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }
        /// <summary>
        /// Genera el html con la tabla del listado de los comunicados de sala de prensa
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ruta"></param>
        /// <param name="typ"></param>
        /// <returns></returns>
        public string ListaComunicadosSalaPrensaHtml(List<WbComunicadosDTO> data, string ruta, int typ)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='pretty tabla-icono display compact' id='tb_info" + typ + "'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 90px'>ACCIONES</th>");
            strHtml.Append("<th style='width: 90px'>FECHA</th>");
            strHtml.Append("<th style='width: 100px'>TÍTULO</th>");
            strHtml.Append("<th>DESCRIPCIÓN</th>");
            strHtml.Append("<th style='width: 90px'>INICIO</th>");
            strHtml.Append("<th style='width: 90px'>TÉRMINO</th>");

            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var list in data)
            {
                strHtml.Append("<tr id='" + list.Comorden + "'>");
                strHtml.Append(string.Format(
                    "<td><a href='#' onclick='edit_(" + list.Comcodi + ",2,\"\");'><img src='{0}Content/Images/btn-edit.png' /></a>&nbsp;&nbsp;&nbsp;&nbsp;<a href='#' onclick='delete_(" + list.Comcodi + ");'><img src='{0}Content/Images/btn-cancel.png' /></a></td>"
                    , ruta));
                strHtml.Append(string.Format("<td>{0}</td>", list.Comfecha.Value.ToString(ConstantesAppServicio.FormatoFecha)));
                strHtml.Append(string.Format("<td>{0}</td>", list.Comtitulo.Trim()));
                strHtml.Append(string.Format("<td>{0}</td>", list.Comdesc.Trim()));
                strHtml.Append(string.Format("<td>{0}</td>", list.Comfechaini.Value.ToString(ConstantesAppServicio.FormatoFecha)));
                strHtml.Append(string.Format("<td>{0}</td>", list.Comfechafin.Value.ToString(ConstantesAppServicio.FormatoFecha)));

                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }



        #endregion

        #region Cdispatch NCP

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="entitys"></param>
        //public void SaveMemedicion24masivo(List<MeMedicion24DTO> entitys)
        //{
        //    FactorySic.GetMeMedicion24Repository().SaveMemedicion24masivo(entitys);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="entitys"></param>
        //public void SaveMemedicion48masivo(List<MeMedicion48DTO> entitys)
        //{
        //    FactorySic.GetMeMedicion48Repository().SaveMemedicion48masivo(entitys);
        //}

        ///// <summary>
        ///// Inicializa la cabecera de la matriz (Punto medicodi y Tipoinfocodi)
        ///// </summary>
        ///// <param name="path"></param>
        ///// <returns></returns>
        //public List<string> GetGeneraCsvTemp(List<string> lista, int result, List<PrGrupoDTO> ListaPtos, ref string[][] matriz)
        //{
        //    var caracteCabeceraDelete = 0;
        //    string[] splitExcel = null;

        //    List<string> listaStrCabecera = new List<string>();
        //    if (lista.Count > 0) { listaStrCabecera = lista[0].ToUpper().Split(',').ToList(); }
        //    for (int a = 0; a < ListaPtos.Count; a++)
        //    {
        //        if (listaStrCabecera.Find(x => x.Trim().Contains(ListaPtos[a].Gruponombncp.Replace(" ", "").ToUpper())) != null)
        //        {
        //            matriz[0][a] = ListaPtos[a].Ptomedicodi.ToString();
        //            matriz[1][a] = ListaPtos[a].Tipoinfocodi.ToString();
        //        }
        //    }

        //    if (result > 0)
        //    {
        //        for (int b = 0; b < lista.Count; b++)
        //        {
        //            if (b == 0)
        //            {
        //                splitExcel = lista[b].Trim().Split(',');
        //                for (int c = 0; c < 3; c++)
        //                {
        //                    caracteCabeceraDelete += splitExcel[c].Length;
        //                }
        //                //Obtenemos la cabecera correcta
        //                lista[b] = lista[b].ToUpper().Trim().Substring(caracteCabeceraDelete + 3, lista[b].Trim().Length - caracteCabeceraDelete - 3);
        //            }
        //            else
        //            {
        //                lista[b] = lista[b].Replace(" ", "").Trim();
        //                caracteCabeceraDelete = 0;
        //                splitExcel = lista[b].Trim().Split(',');
        //                for (int d = 0; d < 3; d++)
        //                {
        //                    caracteCabeceraDelete += splitExcel[d].Length;
        //                }
        //                //Obtenemos los datos correctos
        //                lista[b] = lista[b].Substring(caracteCabeceraDelete + 3, lista[b].Length - caracteCabeceraDelete - 3);
        //            }
        //        }
        //    }

        //    return lista;
        //}

        ///// <summary>
        ///// Metodo ProcesoArchivosCsv - Oliver
        ///// Se verifica la cabecera de la BD con la cabecera del objeto lista.
        ///// Una vez obtenido cabecera de objeto lista por medio de la posicion
        ///// recuperamos y llenamos matriz para programados y reprogramados
        ///// </summary>
        ///// <param name="listaCSVnew">Objeto Lista Programados y Reprogramados</param>
        ///// <param name="tipoProgramacion">PDO, RDO</param>
        ///// <param name="MatrizExcelTemp">Matriz para Reprogramados</param>
        ///// <returns>MatrizExcelTemp</returns>
        //public string[][] ProcesoArchivosCsv(List<string> listaCSVnew, string tipoProgramacion, string[][] MatrizExcelTemp, List<PrGrupoDTO> ListaPtos, int tipMe)
        //{
        //    //Consultamos la cabecera que se mostrara en Matriz
        //    for (int a = 0; a < ListaPtos.Count; a++)
        //    {
        //        List<string> listaStrCabecera = listaCSVnew[0].ToUpper().Split(',').ToList();
        //        //Verificamos si objeto lista contiene datos de la cabecera
        //        if (listaStrCabecera.Find(x => x.Trim().Contains(ListaPtos[a].Gruponombncp.Replace(" ", "").ToUpper())) != null)
        //        {
        //            //Obtenemos la posicion dato de la cabecera
        //            int posicion = listaCSVnew[0].Substring(0, listaCSVnew[0].IndexOf(ListaPtos[a].Gruponombncp.Replace(" ", "").ToUpper())).Split(',').Length - 1;

        //            //Recorremos la lista para obtener datos Programados y Reprogramados
        //            if (tipMe == 48)
        //            {
        //                int cc = listaCSVnew.Count;
        //                for (int b = tipMe; b > 0; b--)
        //                {
        //                    if (cc == 1) { break; }
        //                    MatrizExcelTemp[b + 1][a] = listaCSVnew[cc - 1].Split(',')[posicion].Trim();
        //                    cc--;
        //                }
        //            }
        //            else
        //            {
        //                int cc = 2;
        //                for (int b = 2; b <= listaCSVnew.Count; b += 2) { MatrizExcelTemp[cc][a] = listaCSVnew[b - 1].Split(',')[posicion].Trim(); cc++; }
        //            }
        //        }
        //    }

        //    return MatrizExcelTemp;
        //}

        ///// <summary>
        ///// Listar los puntos de medicion (ptomedicodi, grupocodi, tipoinfocodi)
        ///// </summary>
        ///// <returns></returns>
        //public List<PrGrupoDTO> GetListaPtos(string filename, List<PrGrupoDTO> ptosCab, CDespachoDTO cdespacho)
        //{
        //    List<PrGrupoDTO> lista = new List<PrGrupoDTO>();

        //    switch (filename)
        //    {
        //        case ConstantesMigraciones.Cirflwcp:
        //            var det = ptosCab.Where(x => x.Tipoinfocodi == 8 && x.Equinomb == "FLUJO EN LINEAS").ToList();
        //            foreach (var d in det)
        //            {
        //                if (d.Equiabrev.Split(',').Length > 1)
        //                {
        //                    foreach (var ln in d.Equiabrev.Split(','))
        //                    {
        //                        lista.Add(new PrGrupoDTO() { Ptomedicodi = d.Ptomedicodi, Tipoinfocodi = (int)d.Tipoinfocodi, Gruponombncp = ln });
        //                    }
        //                }
        //                else { lista.Add(new PrGrupoDTO() { Ptomedicodi = d.Ptomedicodi, Tipoinfocodi = (int)d.Tipoinfocodi, Gruponombncp = d.Equiabrev }); }
        //            }
        //            break;
        //        case ConstantesMigraciones.Gerhidcp:
        //        case ConstantesMigraciones.Gergndcp:
        //        case ConstantesMigraciones.Gerunicp:
        //            lista = cdespacho.ListaPrgrupo.Where(x => x.Grupoactivo == "S" && (x.Gruponombncp != "" && x.Gruponombncp != null)).ToList();
        //            foreach (var d in lista) { d.Tipoinfocodi = ConstantesAppServicio.TipoinfocodiMW; }
        //            break;
        //        case ConstantesMigraciones.Resaghcp:
        //        case ConstantesMigraciones.Resagtcp:
        //            var list = cdespacho.ListaPtoMedicion.Where(x => x.Tipoinfocodi == 49).ToList();
        //            lista = list.Select(x => new PrGrupoDTO() { Ptomedicodi = x.Ptomedicodi, Tipoinfocodi = (int)x.Tipoinfocodi, Gruponombncp = x.Ptomedidesc }).ToList();
        //            break;
        //        case ConstantesMigraciones.Gertercp:
        //            lista = cdespacho.ListaPrgrupo.Where(x => x.Grupoactivo == "S").ToList();
        //            foreach (var d in lista) { d.Tipoinfocodi = ConstantesAppServicio.TipoinfocodiMW; }
        //            break;
        //        case ConstantesMigraciones.Defcitcp:
        //            var puntoDef = ptosCab.Find(x => x.Ptomedicodi == 1224);
        //            if (puntoDef != null)
        //            {
        //                lista.Add(new PrGrupoDTO() { Ptomedicodi = puntoDef.Ptomedicodi, Tipoinfocodi = (int)puntoDef.Tipoinfocodi, Gruponombncp = "Peru" });
        //            }
        //            break;
        //        case ConstantesMigraciones.Cmgdemcp:
        //            var puntoCmgdemcp = ptosCab.Find(x => x.Ptomedicodi == 1217);
        //            if (puntoCmgdemcp != null)
        //            {
        //                lista.Add(new PrGrupoDTO() { Ptomedicodi = puntoCmgdemcp.Ptomedicodi, Tipoinfocodi = (int)puntoCmgdemcp.Tipoinfocodi, Gruponombncp = "Peru" });
        //            }
        //            break;
        //        case ConstantesMigraciones.Cmgbuscp:
        //            det = ptosCab.Where(x => x.Tipoinfocodi == 21 && x.Equiabrev != "0").ToList();
        //            foreach (var d in det)
        //            {
        //                lista.Add(new PrGrupoDTO() { Ptomedicodi = d.Ptomedicodi, Tipoinfocodi = (int)d.Tipoinfocodi, Gruponombncp = d.Equiabrev });
        //            }
        //            break;
        //        case ConstantesMigraciones.Ncpcope: lista.Add(new PrGrupoDTO() { Ptomedicodi = ConstantesAppServicio.PtomedicodiCostoOpeDia, Tipoinfocodi = ConstantesAppServicio.TipoinfocodiSoles, Gruponombncp = "COSTOS DE OPERACIÓN" }); break;
        //    }

        //    return lista;
        //}

        ///// <summary>
        ///// Listar los puntos de medicion de la cabecera Gertercp
        ///// </summary>
        ///// <param name="lineaCabecera"></param>
        ///// <param name="listaPto"></param>
        ///// <returns></returns>
        //public void ListarGrupoByCabeceraGertercpNCP(string lineaCabecera, List<PrGrupoDTO> listaPto, out List<string> listaModoValido, out List<string> listaPtomedicodi)
        //{
        //    List<string> listanew = new List<string>();
        //    List<string> listaModoNew = new List<string>();

        //    List<PrGrupoDTO> listaDataMop = this.ListarDataPrGrupoDTO();
        //    string[] tokens = lineaCabecera.Split(',');

        //    foreach (var reg in tokens)
        //    {
        //        PrGrupoDTO modop = listaDataMop.Find(x => x.ModoOperacion == reg.Trim().ToUpper());
        //        if (modop != null)
        //        {
        //            listaModoNew.Add(reg.Trim().ToUpper());
        //            foreach (var i in modop.ListaPrgrupoDespacho)
        //            {
        //                var punto = listaPto.Find(x => x.Grupocodi == i.Grupocodi);
        //                if (punto != null)
        //                {
        //                    listanew.Add(punto.Ptomedicodi.ToString());
        //                }
        //            }
        //        }
        //        else
        //        {
        //            PrGrupoDTO grupo = listaPto.Find(x => x.Gruponombncp != null && x.Gruponombncp.Replace(" ", "").Contains(reg.Trim().ToUpper()));
        //            if (grupo != null) //el nombrencp puede tener comas p.e. VENTA-TG4,VENTA-TG4D
        //            {
        //                //if (grupo.Gruponombncp.Contains("CHILCA-TG4")) { grupo.Gruponombncp = "CC-CHILCATG4"; }
        //                //if (grupo.Gruponombncp.Contains("OLLEROS TG1") || grupo.Gruponombncp.Contains("OLLEROSTG1")) { grupo.Gruponombncp = "CC_OLLEROS"; }

        //                string nombreNcp = grupo.Gruponombncp;
        //                List<string> listaNombreNcp = nombreNcp.Split(',').ToList();
        //                if (listaNombreNcp.Find(x => x.Trim().ToUpper() == reg.Trim().ToUpper()) != null)
        //                {
        //                    listaModoNew.Add(reg.Trim().ToUpper());
        //                    listanew.Add(grupo.Ptomedicodi.ToString());
        //                }
        //            }
        //        }
        //    }

        //    listaModoValido = listaModoNew;
        //    listaPtomedicodi = listanew;
        //}

        ///// <summary>
        ///// Genera matriz para guardar en medicion48
        ///// </summary>
        ///// <param name="rowsHead"></param>
        ///// <param name="nFil"></param>
        ///// <param name="listaNew"></param>
        ///// <param name="listaModoValido"></param>
        ///// <param name="lista"></param>
        ///// <param name="listaPto"></param>
        ///// <param name="tipoinfocodi"></param>
        ///// <returns></returns>
        //public string[][] GetMatrizGertercpNCP(int rowsHead, int nFil, List<string> listaNew, List<string> listaModoValido, List<string> lista, List<PrGrupoDTO> listaPto, int tipoinfocodi)
        //{
        //    string[][] dataFinal = this.InicializaMatriz(rowsHead, nFil + 1, listaNew.Count);

        //    var caracteCabeceraDelete = 0;
        //    string[] splitExcel = null;

        //    //formater filas del csv
        //    string lineaCabecera = string.Empty;
        //    for (int b = 0; b < lista.Count; b++)
        //    {
        //        lista[b] = lista[b].Replace(" ", "").Trim();
        //        caracteCabeceraDelete = 0;
        //        splitExcel = lista[b].Trim().Split(',');
        //        for (int d = 0; d < 3; d++)
        //        {
        //            caracteCabeceraDelete += splitExcel[d].Length;
        //        }
        //        //Obtenemos los datos correctos
        //        lista[b] = lista[b].Substring(caracteCabeceraDelete + 3, lista[b].Length - caracteCabeceraDelete - 3);

        //        if (b == 0)
        //        {
        //            //generar cabecera matriz
        //            lineaCabecera = lista[0];

        //            for (int posicion = 0; posicion < listaNew.Count; posicion++)
        //            {
        //                dataFinal[0][posicion] = listaNew[posicion].ToString();
        //                dataFinal[1][posicion] = tipoinfocodi.ToString();
        //            }
        //        }
        //        else
        //        {
        //            int columna = 0;

        //            //
        //            List<PrGrupoDTO> listaDataMop = this.ListarDataPrGrupoDTO();
        //            string[] tokens = lista[0].Split(',');
        //            var carhuCabecera = tokens.Where(x => x.ToUpper().Contains("CHILCA")).ToList();
        //            if (carhuCabecera.Count > 0)
        //            {
        //            }
        //            for (int posicion = 0; posicion < tokens.Length; posicion++)
        //            {
        //                if (listaModoValido.Find(x => x == tokens[posicion].Trim().ToUpper()) == null)
        //                {
        //                    continue;
        //                }

        //                string dataPosicion = lista[b].Split(',')[posicion].Trim();
        //                PrGrupoDTO modop = listaDataMop.Find(x => x.ModoOperacion == tokens[posicion].ToUpper().Trim());
        //                if (modop != null)
        //                {
        //                    foreach (var i in modop.ListaPrgrupoDespacho)
        //                    {
        //                        var punto = listaPto.Find(x => x.Grupocodi == i.Grupocodi);
        //                        if (punto != null)
        //                        {
        //                            decimal valor = 0;
        //                            if (/*//Try parsing in the current culture
        //                                                !decimal.TryParse(val, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out valorParse)
        //                                                 &&  //Then try in US english
        //                                                !decimal.TryParse(val, System.Globalization.NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out valorParse)
        //                                                &&  //Then in neutral language*/
        //                                !decimal.TryParse(dataPosicion, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out valor))
        //                            {
        //                                throw new Exception("Error parse" + dataPosicion);
        //                            }
        //                            else
        //                            {
        //                                valor = valor * i.Factor;
        //                            }

        //                            dataFinal[b + 1][columna] = valor.ToString();
        //                            columna++;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    PrGrupoDTO grupo = listaPto.Find(x => x.Gruponombncp != null && x.Gruponombncp.Replace(" ", "").Contains(tokens[posicion].ToUpper().Trim()));
        //                    if (grupo != null)
        //                    {
        //                        string nombreNcp = grupo.Gruponombncp;
        //                        List<string> listaNombreNcp = nombreNcp.Split(',').ToList();
        //                        if (listaNombreNcp.Find(x => x.Trim().ToUpper() == tokens[posicion].Trim().ToUpper()) != null)
        //                        {
        //                            dataFinal[b + 1][columna] = dataPosicion;
        //                            columna++;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return dataFinal;
        //}

        ///// <summary>
        ///// Reacomodar matriz para casos que sean menores de 48 (reprogramados)
        ///// </summary>
        ///// <param name="matriz"></param>
        ///// <param name="count"></param>
        ///// <returns></returns>
        //public string[][] AcomodarPosicionMatriz(int rowsHead, int nFil, string[][] matriz, int count)
        //{
        //    string[][] data = this.InicializaMatriz(rowsHead, nFil + 1, matriz[0].Length);

        //    for (int i = 0; i <= 1; i++)
        //    {
        //        for (int y = 0; y < matriz[i].Length; y++) { data[i][y] = matriz[i][y]; }
        //    }

        //    int cc = count;
        //    for (int z = 48; z > 0; z--)
        //    {
        //        if (cc == 2) { break; }
        //        data[z + 2] = matriz[cc];
        //        cc--;
        //    }

        //    return data;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="rowsHead"></param>
        ///// <param name="nFil"></param>
        ///// <param name="nCol"></param>
        ///// <returns></returns>
        //public string[][] InicializaMatriz(int rowsHead, int nFil, int nCol)
        //{
        //    string[][] matriz = new string[nFil + rowsHead][];
        //    for (int i = 0; i <= nFil; i++)
        //    {
        //        matriz[i] = new string[nCol];
        //        for (int j = 0; j < nCol; j++)
        //        {
        //            matriz[i][j] = string.Empty;
        //        }
        //    }
        //    return matriz;
        //}

        ///// <summary>
        ///// Modos de operacion, grupos despacho y factor
        ///// </summary>
        ///// <returns></returns>
        //public List<PrGrupoDTO> ListarDataPrGrupoDTO()
        //{
        //    List<PrGrupoDTO> listaRel = new List<PrGrupoDTO>();

        //    PrGrupoDTO obj = null;
        //    obj = new PrGrupoDTO() { ModoOperacion = "VENTA-CC", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 113, Factor = 0.639m });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 114, Factor = 0.639m });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 193, Factor = 0.361m });
        //    listaRel.Add(obj);

        //    obj = new PrGrupoDTO() { ModoOperacion = "VENTA-CCTG3", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 113, Factor = 0.639m });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 193, Factor = 0.361m });
        //    listaRel.Add(obj);

        //    obj = new PrGrupoDTO() { ModoOperacion = "VENTA-CCTG4", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 114, Factor = 0.639m });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 193, Factor = 0.361m });
        //    listaRel.Add(obj);

        //    obj = new PrGrupoDTO() { ModoOperacion = "CC-KALLPA", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 197, Factor = 2m / 9 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 404, Factor = 2m / 9 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 410, Factor = 2m / 9 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 430, Factor = 1m / 3 });
        //    listaRel.Add(obj);

        //    obj = new PrGrupoDTO() { ModoOperacion = "KALLPA-CCTG1", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 197, Factor = 2m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 430, Factor = 1m / 3 });
        //    listaRel.Add(obj);

        //    obj = new PrGrupoDTO() { ModoOperacion = "KALLPA-CCTG2", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 404, Factor = 2m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 430, Factor = 1m / 3 });
        //    listaRel.Add(obj);

        //    obj = new PrGrupoDTO() { ModoOperacion = "KALLPA-CCTG3", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 410, Factor = 2m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 430, Factor = 1m / 3 });
        //    listaRel.Add(obj);

        //    obj = new PrGrupoDTO() { ModoOperacion = "KALLPA-CTG12", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 197, Factor = 1m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 404, Factor = 1m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 430, Factor = 1m / 3 });
        //    listaRel.Add(obj);

        //    obj = new PrGrupoDTO() { ModoOperacion = "KALLPA-CTG23", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 404, Factor = 1m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 410, Factor = 1m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 430, Factor = 1m / 3 });
        //    listaRel.Add(obj);

        //    obj = new PrGrupoDTO() { ModoOperacion = "KALLPA-CTG31", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 197, Factor = 1m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 410, Factor = 1m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 430, Factor = 1m / 3 });
        //    listaRel.Add(obj);

        //    obj = new PrGrupoDTO() { ModoOperacion = "CC-CHILCA", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 194, Factor = 2m / 9 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 196, Factor = 2m / 9 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 407, Factor = 2m / 9 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 436, Factor = 1m / 3 });
        //    listaRel.Add(obj);

        //    obj = new PrGrupoDTO() { ModoOperacion = "CHILCA-CCTG1", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 194, Factor = 2m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 436, Factor = 1m / 3 });
        //    listaRel.Add(obj);

        //    obj = new PrGrupoDTO() { ModoOperacion = "CHILCA-CCTG2", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 196, Factor = 2m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 436, Factor = 1m / 3 });
        //    listaRel.Add(obj);

        //    obj = new PrGrupoDTO() { ModoOperacion = "CHILCA-CCTG3", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 407, Factor = 2m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 436, Factor = 1m / 3 });
        //    listaRel.Add(obj);

        //    obj = new PrGrupoDTO() { ModoOperacion = "CHILCA-CTG12", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 194, Factor = 1m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 196, Factor = 1m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 436, Factor = 1m / 3 });
        //    listaRel.Add(obj);

        //    obj = new PrGrupoDTO() { ModoOperacion = "CHILCA-CTG23", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 196, Factor = 1m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 407, Factor = 1m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 436, Factor = 1m / 3 });
        //    listaRel.Add(obj);

        //    obj = new PrGrupoDTO() { ModoOperacion = "CHILCA-CTG31", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 194, Factor = 1m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 407, Factor = 1m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 436, Factor = 1m / 3 });
        //    listaRel.Add(obj);

        //    obj = new PrGrupoDTO() { ModoOperacion = "CC-ChilcaTG4", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 857, Factor = 2m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 538, Factor = 1m / 3 });
        //    listaRel.Add(obj);

        //    obj = new PrGrupoDTO() { ModoOperacion = "CCFENIX-TV", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 454, Factor = 2m / 6 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 451, Factor = 2m / 6 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 452, Factor = 2m / 6 });
        //    listaRel.Add(obj);

        //    obj = new PrGrupoDTO() { ModoOperacion = "CCFENIX-1TV", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 454, Factor = 2m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 452, Factor = 1m / 3 });
        //    listaRel.Add(obj);

        //    obj = new PrGrupoDTO() { ModoOperacion = "CCFENIX-2TV", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 451, Factor = 2m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 452, Factor = 1m / 3 });
        //    listaRel.Add(obj);

        //    obj = new PrGrupoDTO() { ModoOperacion = "CC_OLLEROS", ListaPrgrupoDespacho = new List<PrGrupoDTO>() };
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 450, Factor = 2m / 3 });
        //    obj.ListaPrgrupoDespacho.Add(new PrGrupoDTO() { Grupocodi = 695, Factor = 1m / 3 });
        //    listaRel.Add(obj);

        //    return listaRel;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public List<PrGrupoDTO> GetListaPtosNCP()
        //{
        //    return FactorySic.GetPrGrupoRepository().GetByListaModosOperacionNCP().OrderBy(x => x.Grupoabrev).ToList();
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="ptomedicodi"></param>
        ///// <returns></returns>
        //public List<MePtomedicionDTO> GetByCriteriaMeptomedicion(string ptomedicodi)
        //{
        //    return FactorySic.GetMePtomedicionRepository().List(ptomedicodi, ConstantesAppServicio.ParametroDefecto);
        //}

        ///// <summary>
        ///// Funcion de eliminacion de zip
        ///// </summary>
        ///// <param name="NombreFile"></param>
        ///// <param name="file"></param>
        ///// <param name="path"></param>
        //public void DeleteTmpZip(string NombreFile, string file, string path)
        //{
        //    #region Eliminamos Carpeta Temporal (Archivo y Carpetas extraidas del ZIP)

        //    if (NombreFile.ToLower().Contains(".zip"))
        //    {
        //        List<string> archivos = new List<string>();
        //        using (FileStream zipToOpen = new FileStream(file, FileMode.Open))
        //        {
        //            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
        //            {
        //                for (int i = 0; i < archive.Entries.Count; i++)
        //                {
        //                    foreach (var item in archive.Entries)
        //                    {
        //                        archivos.Add(item.ToString());
        //                        item.Delete();
        //                        break; //needed to break out of the loop
        //                    }

        //                    i = -1;
        //                }
        //            }
        //        }
        //        System.IO.File.Delete(file);
        //        List<string> directorys = new List<string>();
        //        string direc = string.Empty;
        //        foreach (var d in archivos)
        //        {
        //            if (d.Contains("/"))
        //            {
        //                if (direc.Split('/')[0] != d.Split('/')[0])
        //                {
        //                    System.IO.DirectoryInfo di = new DirectoryInfo(path + d);

        //                    foreach (FileInfo de in di.GetFiles())
        //                    {
        //                        de.Delete();
        //                    }
        //                    foreach (DirectoryInfo dir in di.GetDirectories())
        //                    {
        //                        dir.Delete(true);
        //                    }
        //                    direc = d;
        //                    directorys.Add(direc.Split('/')[0]);
        //                }
        //            }
        //            else { System.IO.File.Delete(path + d); }
        //        }
        //        foreach (var d in directorys)
        //        {
        //            System.IO.Directory.Delete(path + d);
        //        }
        //    }
        //    #endregion
        //}

        //public void LeerFileUpzip(string NombreFile, string path, string lectcodi, int chk, DateTime fechaPeriodo, ref CDespachoDTO regCDespacho, ref List<MeMedicion48DTO> ListaMe48, ref List<MeMedicion1DTO> ListaMe1
        //    , out int NroMostrar, out string Comentario, out List<string> ListaComment, out string msj)
        //{
        //    ListaComment = new List<string>();

        //    string file = string.Empty;
        //    msj = string.Empty;
        //    Comentario = string.Empty;
        //    NroMostrar = 0;
        //    int nroDias = 1;
        //    bool vali = false;

        //    List<string> listaFiles = NombreFile.Split(',').Select(x => x).ToList();
        //    if (listaFiles.Count != 2) { msj = "Debe seleccionar dos archivos (Zip NCP y Demanda NCP)"; }

        //    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //    // Obtener la lista de archivos seleccionados y ordenarlos para los procesos siguientes
        //    List<string> listaFilesTmp = NombreFile.Split(',').Select(x => x).ToList();
        //    string strZip = listaFilesTmp.Where(x => x.Contains(".zip")).Count() > 0 ? listaFilesTmp.Where(x => x.Contains(".zip")).FirstOrDefault() : string.Empty;
        //    string strDemanda = listaFilesTmp.Where(x => x.Contains("DEMANDA")).Count() > 0 ? listaFilesTmp.Where(x => x.Contains("DEMANDA")).FirstOrDefault() : string.Empty;

        //    listaFiles = new List<string>();
        //    if (!string.IsNullOrEmpty(strZip))
        //        listaFiles.Add(strZip);
        //    if (!string.IsNullOrEmpty(strDemanda))
        //        listaFiles.Add(strDemanda);

        //    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //    if (msj == "")
        //    {
        //        List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();

        //        foreach (var arch in listaFiles)
        //        {
        //            if (arch.Contains(".zip"))
        //            {
        //                file = path + arch;
        //                try
        //                {
        //                    ZipFile.ExtractToDirectory(file, path);

        //                    List<string> listaCsv = new List<string>();
        //                    listaCsv.Add(ConstantesMigraciones.Cirflwcp);
        //                    listaCsv.Add(ConstantesMigraciones.Gerhidcp);
        //                    listaCsv.Add(ConstantesMigraciones.Gergndcp);
        //                    listaCsv.Add(ConstantesMigraciones.Gertercp);
        //                    listaCsv.Add(ConstantesMigraciones.Defcitcp);
        //                    listaCsv.Add(ConstantesMigraciones.Cmgbuscp);
        //                    listaCsv.Add(ConstantesMigraciones.Cmgdemcp);
        //                    listaCsv.Add(ConstantesMigraciones.Resaghcp);
        //                    listaCsv.Add(ConstantesMigraciones.Resagtcp);
        //                    //listaCsv.Add(ConstantesMigraciones.Defbuscp);
        //                    listaCsv.Add(ConstantesMigraciones.Ncpcope);
        //                    //listaCsv.Add(ConstantesMigraciones.Gerunicp);

        //                    List<string> listaDat = new List<string>();
        //                    listaDat.Add(ConstantesMigraciones.Cpdexbus30);
        //                    listaDat.Add(ConstantesMigraciones.Caudcp);
        //                    //listaDat.Add(ConstantesMigraciones.Cpde30PE);

        //                    //Leemos el archivo ZIP
        //                    using (ZipArchive archive = ZipFile.OpenRead(file))
        //                    {
        //                        #region Archivos Csv

        //                        foreach (var archivoCsv in listaCsv)
        //                        {
        //                            List<MeMedicion48DTO> entitysXArchivo = new List<MeMedicion48DTO>();
        //                            List<PrGrupoDTO> listaPtos = this.GetListaPtos(archivoCsv, this.FillCabeceraCdispatchHistorico(), regCDespacho);
        //                            var datos = archive.Entries.Where(x => x.FullName.Contains(archivoCsv)).ToList();

        //                            int rowsCab = 1, rows = chk, nroDataCsv = 0;

        //                            if (listaPtos.Count > 0 && datos.Count > 0)
        //                            {
        //                                #region inicio de lectura de archivo

        //                                int _result = 0;
        //                                //Leemos el archivo CSV
        //                                string[] lineasCSV = System.IO.File.ReadAllLines(path + datos.First().FullName, System.Text.Encoding.Default);

        //                                List<string> listanew = new List<string>();
        //                                List<string> listaCabeceraValida = new List<string>();
        //                                List<string> lista = new List<string>(lineasCSV);

        //                                //Eliminamos 3 filas de la lista - filas que no se usaran para la MatrizExcel
        //                                lista.RemoveAt(0);
        //                                if (archivoCsv != ConstantesMigraciones.Ncpcope)
        //                                {
        //                                    lista.RemoveAt(0);
        //                                    lista.RemoveAt(0);

        //                                    //Consultamos si el objeto lista tiene datos que no estan relacionados con la cabecera de la Matriz
        //                                    if (lista.Count > 0) { lista[0] = lista[0].Replace(" ", ""); }
        //                                }

        //                                string[][] matriz = null;
        //                                switch (archivoCsv)
        //                                {
        //                                    case ConstantesMigraciones.Gertercp:

        //                                        this.ListarGrupoByCabeceraGertercpNCP(lista[0].ToUpper(), listaPtos, out listaCabeceraValida, out listanew);
        //                                        var matriz_ = this.GetMatrizGertercpNCP(rowsCab, rows + 1, listanew, listaCabeceraValida, lista, listaPtos, 1);
        //                                        nroDataCsv = lista.Count - 1;
        //                                        if (lista.Count < 48) { matriz = this.AcomodarPosicionMatriz(rowsCab, rows + 1, matriz_, lista.Count); }
        //                                        else { matriz = matriz_; }

        //                                        break;
        //                                    case ConstantesMigraciones.Ncpcope:

        //                                        decimal tot = 0;
        //                                        ListaMe1 = new List<MeMedicion1DTO>();
        //                                        foreach (var da in lista)
        //                                        {
        //                                            tot += decimal.Parse(da.Split(',')[1]);
        //                                        }
        //                                        ListaMe1.Add(new MeMedicion1DTO() { H1 = tot * 1000, Ptomedicodi = ConstantesAppServicio.PtomedicodiCostoOpeDia, Tipoinfocodi = ConstantesAppServicio.TipoinfocodiSoles });

        //                                        break;
        //                                    default:

        //                                        List<string> listaStrCabecera = new List<string>();
        //                                        var listaNewPtos = new List<PrGrupoDTO>();
        //                                        if (lista.Count > 0) { listaStrCabecera = lista[0].ToUpper().Split(',').ToList(); }
        //                                        for (int a = 0; a < listaPtos.Count; a++)
        //                                        {
        //                                            if (listaPtos[a].Gruponombncp.ToUpper().Contains("PATAPO")) { listaPtos[a].Gruponombncp = "PATAPO"; }
        //                                            var arrNPCs = listaPtos[a].Gruponombncp.Trim().Split(',');
        //                                            if (arrNPCs.Length > 1)
        //                                            {
        //                                                var list = regCDespacho.ListaPtoMedicion.Where(x => x.Tipoinfocodi == 49).ToList();
        //                                                foreach (var arr in arrNPCs)
        //                                                {
        //                                                    if (listaStrCabecera.Find(x => x.Trim().Contains(arr.Replace(" ", "").ToUpper())) != null)
        //                                                    {
        //                                                        var ptomedi = list.Find(x => x.Ptomedidesc.ToUpper().Trim() == arr.ToUpper());
        //                                                        if (ptomedi != null)
        //                                                        {
        //                                                            _result++;
        //                                                            listaNewPtos.Add(new PrGrupoDTO() { Gruponombncp = ptomedi.Ptomedidesc, Ptomedicodi = ptomedi.Ptomedicodi, Tipoinfocodi = listaPtos[a].Tipoinfocodi });
        //                                                        }
        //                                                    }
        //                                                }
        //                                            }
        //                                            else
        //                                            {
        //                                                if (listaPtos[a].Gruponombncp.Contains("CARHUAC")) { listaPtos[a].Gruponombncp = "CARHUAC"; }
        //                                                if (listaStrCabecera.Find(x => x.Trim().Contains(listaPtos[a].Gruponombncp.Trim().Replace(" ", "").ToUpper())) != null)
        //                                                {
        //                                                    _result++;
        //                                                    listanew.Add(listaPtos[a].Ptomedicodi.ToString());
        //                                                }
        //                                            }
        //                                        }

        //                                        if (listanew.Count > 0) { listaPtos = listaPtos.Where(x => listanew.Contains(x.Ptomedicodi.ToString())).ToList(); }
        //                                        if (listaNewPtos.Count > 0) { listaPtos.AddRange(listaNewPtos); }

        //                                        //iniciamos matriz
        //                                        matriz = this.InicializaMatriz(rowsCab, rows + 1, listaPtos.Count);

        //                                        // Adecuamos archivo CSV a estructura de la Matriz y generanos un objeto lista listaCSVnew
        //                                        var listaCSV = this.GetGeneraCsvTemp(lista, _result, listaPtos, ref matriz);

        //                                        //Proceso de Lectura y grabado de datos de los archivo Csv
        //                                        nroDataCsv = listaCSV.Count - 1;
        //                                        matriz = this.ProcesoArchivosCsv(listaCSV, "", matriz, listaPtos, rows);

        //                                        break;
        //                                }

        //                                #endregion

        //                                if (archivoCsv != ConstantesMigraciones.Ncpcope)
        //                                {
        //                                    #region medicion48
        //                                    if (chk == 48)
        //                                    {
        //                                        for (int z = 0; z < matriz[0].Length; z++)
        //                                        {
        //                                            string ptomedicodi_ = matriz[0][z];
        //                                            if (ptomedicodi_ != "")
        //                                            {
        //                                                int ptomedicodi = int.Parse(ptomedicodi_);
        //                                                int tipoinfocodi = int.Parse(matriz[1][z]);

        //                                                if (ptomedicodi == 1310)
        //                                                { }

        //                                                switch (ptomedicodi)
        //                                                {
        //                                                    case 40099:
        //                                                    case 40100: ptomedicodi = 170; break;//Aricota
        //                                                    case 40115:
        //                                                    case 40112: ptomedicodi = 118; break;//Oroya-Pachacha
        //                                                    default: break;
        //                                                }

        //                                                MeMedicion48DTO entity = entitysXArchivo.Find(x => x.Medifecha == fechaPeriodo & x.Ptomedicodi == ptomedicodi && x.Tipoinfocodi == tipoinfocodi);
        //                                                bool existeRegM48 = entity != null;

        //                                                if (!existeRegM48)
        //                                                {
        //                                                    entity = new MeMedicion48DTO();
        //                                                    entity.Ptomedicodi = ptomedicodi;
        //                                                    entity.Fuente = archivoCsv;
        //                                                    entity.Hojacodi = nroDataCsv;
        //                                                    entity.Lectcodi = int.Parse(lectcodi);
        //                                                    entity.Tipoinfocodi = tipoinfocodi;
        //                                                    entity.Medifecha = fechaPeriodo;

        //                                                    //asignar código de empresa
        //                                                    var regPto = listaPtos.Find(x => x.Ptomedicodi == entity.Ptomedicodi);
        //                                                    entity.Emprcodi = regPto != null ? regPto.Emprcodi ?? -1 : -1;
        //                                                }

        //                                                decimal total = 0;
        //                                                for (int y = 1; y <= chk; y++)
        //                                                {
        //                                                    string val = (matriz[y + 1][z] == "" ? "0" : matriz[y + 1][z]).Trim();
        //                                                    if (!decimal.TryParse(val, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out decimal valorParse)) throw new Exception("Error parse" + val);
        //                                                    else
        //                                                    {
        //                                                        decimal valActual = ((decimal?)entity.GetType().GetProperty(ConstantesAppServicio.CaracterH + y).GetValue(entity, null)).GetValueOrDefault(0);
        //                                                        decimal valH = valorParse + valActual;
        //                                                        entity.GetType().GetProperty(ConstantesAppServicio.CaracterH + y).SetValue(entity, valH);
        //                                                        total += (valH);
        //                                                    }
        //                                                }

        //                                                entity.Meditotal = total;

        //                                                if (!existeRegM48 && entity.Meditotal.GetValueOrDefault(0) != 0)
        //                                                    entitysXArchivo.Add(entity);
        //                                            }
        //                                        }

        //                                    }
        //                                    #endregion

        //                                    #region medicion24
        //                                    if (chk == 24)
        //                                    {
        //                                        string val = string.Empty;
        //                                        var entitys24 = new List<MeMedicion24DTO>();
        //                                        for (int z = 0; z < matriz[0].Length; z++)
        //                                        {
        //                                            decimal total = 0;
        //                                            var entity = new MeMedicion24DTO();

        //                                            entity.Lectcodi = int.Parse(lectcodi);
        //                                            entity.Medifecha = fechaPeriodo;
        //                                            entity.Ptomedicodi = int.Parse(matriz[0][z]);
        //                                            entity.Tipoinfocodi = int.Parse(matriz[1][z]);
        //                                            for (int y = 1; y <= chk; y++)
        //                                            {
        //                                                val = (matriz[y + 1][z] == "" ? "0" : matriz[y + 1][z]);
        //                                                entity.GetType().GetProperty(ConstantesAppServicio.CaracterH + y).SetValue(entity, decimal.Parse(val));
        //                                                total += decimal.Parse(val);
        //                                            }
        //                                            entity.Meditotal = total;

        //                                            entitys24.Add(entity);
        //                                        }
        //                                        //comentado temporalmente hasta determinar exactamente su funcionalidad
        //                                        //servicio.DeleteMemedicionmasivo(int.Parse(prog), sFecha, string.Join(",", ListaPtos.Select(x => x.Ptomedicodi).Distinct().ToList()), string.Join(",", ListaPtos.Select(x => x.Tipoinfocodi).Distinct().ToList()), chk);
        //                                        //foreach (var ins in entitys) { servicio.SaveMemedicion24(ins); }
        //                                    }
        //                                    #endregion
        //                                }
        //                            }

        //                            entitys.AddRange(entitysXArchivo);
        //                        }

        //                        #endregion

        //                        #region Archivos Dat

        //                        foreach (var archivoDat in listaDat)
        //                        {
        //                            List<MeMedicion48DTO> entitysXArchivo = new List<MeMedicion48DTO>();

        //                            nroDias = 1;
        //                            switch (lectcodi)
        //                            {
        //                                case ConstantesAppServicio.LectcodiProgSemanal: nroDias = 2; break;
        //                                case ConstantesAppServicio.LectcodiProgDiario: break;
        //                                case ConstantesAppServicio.LectcodiReprogDiario: break;
        //                            }

        //                            if (archivoDat == ConstantesMigraciones.Caudcp)
        //                            {
        //                                nroDias = 9;
        //                            }

        //                            //Obtenemos solo los archivos que necesitamos para obtener informacion
        //                            var datos = archive.Entries.Where(x => x.FullName.Contains(archivoDat)).ToList();

        //                            //Proceso de Lectura y grabado de datos de los archivo DAT
        //                            string[][] matriz = ProcesoArchivosDat(datos, (archivoDat == ConstantesMigraciones.Cpde30PE ? 1 : chk), ref nroDias, archivoDat, ref ListaComment);

        //                            if (matriz.Length > 0 && matriz[0] != null)
        //                            {
        //                                int nro_ = 0;

        //                                #region medicion48

        //                                if (chk == 48)
        //                                {
        //                                    if (archivoDat != ConstantesMigraciones.Cpde30PE)
        //                                    {
        //                                        for (int d = 1; d <= nroDias; d++)
        //                                        {
        //                                            for (int z = 1; z < matriz[0].Length; z++)
        //                                            {
        //                                                if (matriz[0][z] != "-1")
        //                                                {
        //                                                    var arr_ = matriz[0][z].Split(',');
        //                                                    int ptomedicodi = int.Parse(arr_[0]);
        //                                                    int tipoinfocodi = int.Parse(arr_[1]);
        //                                                    string strFecha = matriz[1 + (d - 1) * chk][0];
        //                                                    DateTime fechaArchivo = DateTime.ParseExact(strFecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

        //                                                    if (ptomedicodi == 1402)
        //                                                    {
        //                                                    }

        //                                                    MeMedicion48DTO entity = entitysXArchivo.Find(x => x.Medifecha == fechaArchivo & x.Ptomedicodi == ptomedicodi && x.Tipoinfocodi == tipoinfocodi);
        //                                                    bool existeRegM48 = entity != null;

        //                                                    if (!existeRegM48)
        //                                                    {
        //                                                        entity = new MeMedicion48DTO();
        //                                                        entity.Ptomedicodi = ptomedicodi;
        //                                                        entity.Fuente = archivoDat;
        //                                                        entity.Lectcodi = int.Parse(lectcodi);
        //                                                        entity.Tipoinfocodi = tipoinfocodi;
        //                                                        entity.Medifecha = fechaArchivo;

        //                                                        //asignar código de empresa
        //                                                        //var regPto = listaPtos.Find(x => x.Ptomedicodi == entity.Ptomedicodi);
        //                                                        //entity.Emprcodi = regPto != null ? regPto.Emprcodi ?? -1 : -1;
        //                                                    }

        //                                                    decimal total = 0;
        //                                                    for (int y = 1; y <= 48; y++)
        //                                                    {
        //                                                        string val = (matriz[y + nro_][z] == "" ? "0" : matriz[y + nro_][z]).Trim();
        //                                                        if (!decimal.TryParse(val, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out decimal valorParse)) throw new Exception("Error parse" + val);
        //                                                        else
        //                                                        {
        //                                                            decimal valActual = ((decimal?)entity.GetType().GetProperty(ConstantesAppServicio.CaracterH + y).GetValue(entity, null)).GetValueOrDefault(0);
        //                                                            decimal valH = valorParse + valActual;
        //                                                            entity.GetType().GetProperty(ConstantesAppServicio.CaracterH + y).SetValue(entity, valH);
        //                                                            total += (valH);
        //                                                        }
        //                                                    }

        //                                                    entity.Meditotal = total;

        //                                                    if (!existeRegM48 && entity.Meditotal.GetValueOrDefault(0) != 0)
        //                                                        entitysXArchivo.Add(entity);
        //                                                }
        //                                            }
        //                                            nro_ += 48;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        var obj = new MeMedicion48DTO();
        //                                        obj.Ptomedicodi = ConstantesAppServicio.PtomedicodiDemandaNCP;
        //                                        obj.Medifecha = DateTime.ParseExact(matriz[1][0], ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
        //                                        obj.Tipoinfocodi = ConstantesAppServicio.TipoinfocodiDemanda;
        //                                        obj.Lectcodi = int.Parse(lectcodi);
        //                                        for (int y = 1; y <= 48; y++)
        //                                        {
        //                                            obj.GetType().GetProperty(ConstantesAppServicio.CaracterH + y).SetValue(obj, decimal.Parse(matriz[1][y].ToString()));
        //                                        }
        //                                        entitys.Add(obj);
        //                                    }
        //                                    //if (d == ConstantesMigraciones.Cpdexbus30)
        //                                    //{
        //                                    //    var obj = new MeMedicion48DTO();
        //                                    //    for (int i = 1; i <= 48; i++)
        //                                    //    {
        //                                    //        decimal tot = 0;
        //                                    //        foreach (var me in entitys)
        //                                    //        {
        //                                    //            var val = (decimal?)me.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(me, null);
        //                                    //            tot += (val ?? 0);
        //                                    //        }
        //                                    //        obj.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(obj, tot);
        //                                    //    }
        //                                    //    obj.Ptomedicodi = 1221;
        //                                    //    obj.Medifecha = entitys.First().Medifecha;
        //                                    //    obj.Tipoinfocodi = ConstantesAppServicio.TipoinfocodiDemanda;
        //                                    //    obj.Lectcodi = int.Parse(prog);
        //                                    //    entitys.Add(obj);
        //                                    //}
        //                                }

        //                                #endregion

        //                                #region medicion24

        //                                if (chk == 24)
        //                                {
        //                                    List<MeMedicion24DTO> entitys24 = new List<MeMedicion24DTO>();
        //                                    for (int x = 1; x <= nroDias; x++)
        //                                    {
        //                                        for (int z = 1; z < matriz[0].Length; z++)
        //                                        {
        //                                            if (matriz[0][z] != "-1")
        //                                            {
        //                                                decimal total = 0;
        //                                                var arr_ = matriz[0][z].Split(',');
        //                                                int ptomedicodi_ = int.Parse(arr_[0]);
        //                                                int tipoinfocodi_ = int.Parse(arr_[1]);

        //                                                var entity = new MeMedicion24DTO();

        //                                                entity.Lectcodi = int.Parse(lectcodi);
        //                                                entity.Medifecha = fechaPeriodo.AddDays(x);
        //                                                entity.Ptomedicodi = ptomedicodi_;
        //                                                entity.Tipoinfocodi = tipoinfocodi_;
        //                                                for (int y = 1; y <= 24; y++)
        //                                                {
        //                                                    entity.GetType().GetProperty(ConstantesAppServicio.CaracterH + y).SetValue(entity, decimal.Parse(matriz[y + nro_][z].ToString()));
        //                                                    total += decimal.Parse(matriz[y][z].ToString());
        //                                                }
        //                                                entity.Meditotal = total;

        //                                                entitys24.Add(entity);
        //                                            }
        //                                        }
        //                                        nro_ += 24;
        //                                    }

        //                                    //for (int x = 1; x <= nroDias; x++)
        //                                    //{
        //                                    //    DateTime f_ = sFecha.AddDays(x);
        //                                    //    servicio.DeleteMemedicionmasivo(int.Parse(prog), f_, string.Join(",", ListPtomedicodi.Distinct().ToList()), string.Join(",", ListTipoinfocodi.Distinct().ToList()), 24);
        //                                    //}
        //                                    //foreach (var ins in entitys) { servicio.SaveMemedicion24(ins); }
        //                                }

        //                                #endregion
        //                            }

        //                            entitys.AddRange(entitysXArchivo);
        //                        }

        //                        #endregion

        //                    }

        //                    //Eliminamos Carpeta Temporal (Archivo y Carpetas extraidas del ZIP)
        //                    this.DeleteTmpZip(NombreFile, file, path);
        //                }
        //                catch (Exception ex)
        //                {
        //                    ListaComment.Add(ex.Message);
        //                    Logger.Error(ConstantesAppServicio.LogError, ex);
        //                    this.DeleteTmpZip(NombreFile, file, path); NombreFile = "";
        //                }
        //            }

        //            if (arch.Contains("DEMANDA"))
        //            {
        //                vali = true;
        //                file = path + arch;
        //                var ListaPtoMedicion = this.ListarPtoMedicion(ConstantesAppServicio.ParametroDefecto).Where(x => x.Tipoinfocodi > 0).ToList();
        //                int filaData = 0, columnPtos = 0, filaFecha = 0, columnFecha = 0, rowPto = 1;
        //                List<MeMedicion48DTO> data48 = new List<MeMedicion48DTO>();

        //                #region pestaña demanda ncp

        //                FileInfo fileInfo = new FileInfo(file);
        //                using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
        //                {
        //                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets["CONTROL"];
        //                    if (ws != null)
        //                    {
        //                        filaData = 12;
        //                        columnPtos = 7;
        //                        filaFecha = 4;
        //                        columnFecha = 6;
        //                        data48 = UtilCdispatch.SetDatosMe48(ref regCDespacho, ws, int.Parse(lectcodi), ConstantesAppServicio.TipoinfocodiDemanda.ToString(), filaData, columnPtos, filaFecha, columnFecha, rowPto
        //                            , ConstantesAppServicio.LectcodiAjusteDiario, 48, 7);
        //                    }
        //                }

        //                #endregion

        //                entitys.AddRange(data48);

        //                System.IO.File.Delete(file);
        //                if (data48.Count > 0)
        //                {
        //                    Comentario = this.FechasDemandaNCP(data48.Select(x => x.Medifecha).Min());
        //                }
        //                NroMostrar = 1;
        //            }
        //        }

        //        //si existen duplicados, quedarse con el último
        //        List<MeMedicion48DTO> listaFinal = new List<MeMedicion48DTO>();
        //        List<MeMedicion48DTO> listaAEliminar = new List<MeMedicion48DTO>();//duplicados

        //        var subLista48 = entitys.GroupBy(x => new { x.Medifecha, x.Ptomedicodi, x.Tipoinfocodi }).ToList();

        //        foreach (var subLPto in subLista48)
        //        {
        //            if (subLPto.Key.Ptomedicodi == 1310)
        //            { }

        //            List<MeMedicion48DTO> listaXPto = subLPto.ToList();
        //            int totalXPto = listaXPto.Count();
        //            if (totalXPto > 1)
        //            {
        //                MeMedicion48DTO regUltimo = listaXPto.Last();
        //                listaFinal.Add(regUltimo);

        //                listaAEliminar.AddRange(listaXPto.GetRange(0, totalXPto - 1));
        //            }
        //            else
        //            {
        //                listaFinal.AddRange(listaXPto);
        //            }
        //        }

        //        ListaMe48.AddRange(listaFinal);
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="datos"></param>
        ///// <param name="fecha"></param>
        ///// <param name="prog"></param>
        ///// <param name="chk"></param>
        ///// <returns></returns>
        //private string[][] ProcesoArchivosDat(IEnumerable<ZipArchiveEntry> datos, int chk, ref int nroDias, string namefile, ref List<string> ListaComment)
        //{
        //    string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
        //    int fin = 1;
        //    DateTime f1 = DateTime.MinValue, f2 = DateTime.MinValue;

        //    if (nroDias > 1)
        //    {
        //        foreach (ZipArchiveEntry entry in datos)
        //        {
        //            if (fin == 1)
        //            {
        //                using (StreamReader dbProviderReader2 = new StreamReader(path + entry.FullName))
        //                {
        //                    int i = 0;
        //                    string sLine = string.Empty;
        //                    while (sLine != null)
        //                    {
        //                        sLine = dbProviderReader2.ReadLine();
        //                        if (sLine != null)
        //                        {
        //                            if (!sLine.Contains("Infxbloque"))
        //                            {
        //                                var arrLine = sLine.Split(' ').ToList();

        //                                //Eliminamos campos vacios
        //                                arrLine = arrLine.Where(x => !string.IsNullOrEmpty(x)).ToList();

        //                                if (!sLine.Contains("dd/mm/aaaa"))
        //                                {
        //                                    if (i == 2) { f1 = DateTime.ParseExact(arrLine[0].Split(':')[0], ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture); }
        //                                    else { f2 = DateTime.ParseExact(arrLine[0].Split(':')[0], ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture); }
        //                                }
        //                            }
        //                            i++;
        //                        }
        //                        else { break; }
        //                    }
        //                }
        //            }
        //            else { break; }
        //            fin++;
        //        }
        //        TimeSpan ts = f2 - f1;
        //        nroDias = ts.Days + 1;
        //    }
        //    string[][] matriz = new string[(chk * nroDias) + 1][];

        //    fin = 1;
        //    foreach (ZipArchiveEntry entry in datos)
        //    {
        //        if (fin == 1)
        //        {
        //            using (StreamReader dbProviderReader2 = new StreamReader(path + entry.FullName))
        //            {
        //                int i = 0, rows = (namefile == ConstantesMigraciones.Caudcp ? 24 : (namefile == ConstantesMigraciones.Cpde30PE ? 1 : 48));
        //                int cc = 1;
        //                string sLine = string.Empty;
        //                while (sLine != null)
        //                {
        //                    if (i <= (rows * nroDias) + 1)
        //                    {
        //                        sLine = dbProviderReader2.ReadLine();
        //                        if (sLine != null)
        //                        {
        //                            if (!sLine.Contains("Infxbloque"))
        //                            {
        //                                var arrLine = sLine.Split(' ').ToList();

        //                                //Eliminamos campos vacios
        //                                arrLine = arrLine.Where(x => !string.IsNullOrEmpty(x)).ToList();

        //                                if (sLine.Contains("dd/mm/aaaa"))
        //                                {
        //                                    arrLine.RemoveAt(0);

        //                                    List<MePtomedicionDTO> listaNCP = new List<MePtomedicionDTO>();

        //                                    switch (namefile)
        //                                    {
        //                                        case ConstantesMigraciones.Caudcp:
        //                                            listaNCP = this.GetListaMePtomedicion(ConstantesAppServicio.OriglectcodiDespachomediahora).Where(x => arrLine.Contains(x.Codref.ToString())).ToList();
        //                                            break;
        //                                        case ConstantesMigraciones.Cpde30PE:
        //                                        case ConstantesMigraciones.Cpdexbus30:
        //                                            listaNCP = this.GetListaMePtomedicion(ConstantesAppServicio.OriglectcodiDespachomediahora).Where(x => x.Ptomedicodi == ConstantesAppServicio.PtomedicodiDemandaNCP).ToList();
        //                                            break;
        //                                        default:
        //                                            listaNCP = this.GetByCriteriaMeptomedicion(string.Join(",", arrLine.Select(x => x.ToString()).ToList()));
        //                                            break;
        //                                    }

        //                                    matriz[i - 1] = new string[arrLine.Count + 1];
        //                                    for (int z = 1; z <= arrLine.Count; z++)
        //                                    {
        //                                        MePtomedicionDTO det = new MePtomedicionDTO();

        //                                        switch (namefile)
        //                                        {
        //                                            case ConstantesMigraciones.Caudcp:
        //                                                det = listaNCP.Find(x => x.Codref == int.Parse(arrLine[z - 1]));
        //                                                break;
        //                                            case ConstantesMigraciones.Cpdexbus30:
        //                                                det = listaNCP.Find(x => x.Ptomedicodi == ConstantesAppServicio.PtomedicodiDemandaNCP);
        //                                                break;
        //                                            default:
        //                                                det = listaNCP.Find(x => x.Ptomedicodi == int.Parse(arrLine[z - 1]));
        //                                                break;
        //                                        }

        //                                        matriz[i - 1][z] = (det != null ? det.Ptomedicodi + "," + det.Tipoinfocodi : "-1");
        //                                        if (det == null) { if (int.Parse(arrLine[z - 1]) != -1) { ListaComment.Add("Punto de medicion " + arrLine[z - 1] + " no encontrado!!!!"); } }
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    if (chk == 1)
        //                                    {
        //                                        matriz[cc] = new string[arrLine.Count];
        //                                        for (int z = 0; z < arrLine.Count; z++)
        //                                        {
        //                                            matriz[cc][z] = arrLine[z];
        //                                        }
        //                                    }
        //                                    if (chk == 48)
        //                                    {
        //                                        if (namefile == ConstantesMigraciones.Caudcp)
        //                                        {
        //                                            if (cc <= (48 * nroDias))
        //                                            {
        //                                                matriz[cc] = new string[arrLine.Count];
        //                                                for (int z = 0; z < arrLine.Count; z++)
        //                                                {
        //                                                    //if (cc == 1 && z == 0) { if (DateTime.ParseExact(arrLine[z].Split(':')[0], ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture) != fecha) { msj = msj_; break; } }
        //                                                    if (z == 0) { matriz[cc][z] = arrLine[z].Split(':')[0]; }
        //                                                    else { matriz[cc][z] = arrLine[z]; }
        //                                                }
        //                                                matriz[cc + 1] = new string[arrLine.Count];
        //                                                for (int z = 0; z < arrLine.Count; z++)
        //                                                {
        //                                                    if (z == 0) { matriz[cc + 1][z] = arrLine[z].Split(':')[0]; }
        //                                                    else { matriz[cc + 1][z] = arrLine[z]; }
        //                                                }
        //                                                cc += 2;
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            matriz[cc] = new string[arrLine.Count];
        //                                            for (int z = 0; z < arrLine.Count; z++)
        //                                            {
        //                                                //if (cc == 1 && z == 0) { if (DateTime.ParseExact(arrLine[z].Split(':')[0], ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture) != fecha) { msj = msj_; break; } }
        //                                                if (z == 0) { matriz[cc][z] = arrLine[z].Split(':')[0]; }
        //                                                else { matriz[cc][z] = arrLine[z]; }
        //                                            }
        //                                            cc++;
        //                                        }
        //                                    }
        //                                    if (chk == 24)
        //                                    {
        //                                        matriz[cc] = new string[arrLine.Count];
        //                                        if (namefile == ConstantesMigraciones.Caudcp)
        //                                        {
        //                                            for (int z = 0; z < arrLine.Count; z++)
        //                                            {
        //                                                //if (cc == 1 && z == 0) { if (DateTime.ParseExact(arrLine[z].Split(':')[0], ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture) != fecha) { msj = msj_; break; } }
        //                                                if (z == 0) { matriz[cc][z] = arrLine[z].Split(':')[0]; }
        //                                                else { matriz[cc][z] = arrLine[z]; }
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            for (int z = 0; z < arrLine.Count; z += 2)
        //                                            {
        //                                                if (z == 0) { matriz[cc][z] = arrLine[z].Split(':')[0]; }
        //                                                else { matriz[cc][z] = arrLine[z]; }
        //                                            }
        //                                        }
        //                                        cc++;
        //                                    }
        //                                }
        //                            }
        //                            i++;
        //                        }
        //                        else { break; }
        //                    }
        //                    else { break; }
        //                }
        //            }
        //        }
        //        else { break; }
        //        fin++;
        //    }

        //    return matriz;
        //}

        #endregion

        #region Yupana

        /// <summary>
        /// Devuelve una lista de resultados de Yupana en MeMedicion48
        /// </summary>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="topcodi"></param>
        /// <param name="usuario"></param>
        /// <param name="lectcodi"></param>
        /// <param name="listaDespachoHidroXEq"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> DespachoYupana(DateTime fechaini, DateTime fechafin, int topcodi, string usuario, int lectcodi, out List<MeMedicion48DTO> listaDespachoHidroXEq)
        {
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            string equiestado = "'A','F','P','B'", ptomediestado = "'A'", grupoactivo = "'S','N'", emprestado = "A";
            List<PrGrupoDTO> listaGrupo = new List<PrGrupoDTO>();
            List<MePtomedicionDTO> listaPtoDespacho30min = new List<MePtomedicionDTO>();
            var AllListaGrupo = FactorySic.GetPrGrupoRepository().ListarAllGrupoGeneracion(fechaini, grupoactivo, emprestado);

            var listaPtoDespacho = FactorySic.GetMePtomedicionRepository().GetByCriteria(ConstantesAppServicio.ParametroDefecto, "2", ConstantesAppServicio.ParametroDefecto);
            listaPtoDespacho30min.AddRange(listaPtoDespacho.Where(x => x.Grupoactivo == "S").ToList());

            listaGrupo.AddRange(AllListaGrupo.Where(x => x.Grupoactivo == "S").ToList());
            var listaOrder = listaGrupo;
            listaGrupo = new List<PrGrupoDTO>();
            listaGrupo.AddRange(listaOrder);
            listaGrupo.AddRange(this.FillCabeceraCdispatchHistorico().Where(x => x.Equiabrev != "").ToList());

            //Convertir Lista Pr_Grupo a Ptomedicion
            var lpto = listaGrupo.Where(x => x.Emprcodi == 9992).Select(x => x.Ptomedicodi).ToArray();
            var lptoCm = listaGrupo.Where(x => x.Emprcodi == 9993).Select(x => x.Ptomedicodi).ToArray();
            var lptos = String.Join(", ", lpto);
            var listaPtos = FactorySic.GetMePtomedicionRepository().List(lptos, ConstantesAppServicio.ParametroDefecto);
            var lptoCms = String.Join(", ", lptoCm);
            var listaPtosCm = FactorySic.GetMePtomedicionRepository().List(lptoCms, ConstantesAppServicio.ParametroDefecto);

            var listaFlujo = ListaFlujosMcp(topcodi, listaPtos, fechaini, fechafin, usuario, lectcodi);
            CompletarTotalMeMecicion48(ref listaFlujo);
            lista.AddRange(listaFlujo);

            var listaCostoMarginal = ListaCMarginalMcp(topcodi, listaPtosCm, fechaini, fechafin, usuario, lectcodi);
            CompletarTotalMeMecicion48(ref listaCostoMarginal);
            lista.AddRange(listaCostoMarginal);

            List<MeMedicion48DTO> listaMe48 = ListaHidroMcp(topcodi, usuario, lectcodi, out listaDespachoHidroXEq).Where(x => x.Medifecha >= fechaini && x.Medifecha <= fechafin).ToList();
            CompletarTotalMeMecicion48(ref listaMe48);
            lista.AddRange(listaMe48);
            listaDespachoHidroXEq = listaDespachoHidroXEq.Where(x => x.Medifecha >= fechaini && x.Medifecha <= fechafin).ToList();

            var ltermicas = ListaTermicaMcp(topcodi, usuario, lectcodi).Where(x => x.Medifecha >= fechaini && x.Medifecha <= fechafin).ToList();
            var ltermicasCC = ListaTermicasCCMcp(topcodi, fechaini, fechafin, usuario, lectcodi);
            UnirTotalTermicas(ref ltermicas, ltermicasCC);
            CompletarTotalMeMecicion48(ref ltermicas);
            lista.AddRange(ltermicas);

            var lrer = ListaPlantasRerMcp(topcodi, ConstantesBase.SresPotUncr.ToString(), usuario, lectcodi).Where(x => x.Medifecha >= fechaini && x.Medifecha <= fechafin).ToList();
            CompletarTotalMeMecicion48(ref lrer);
            lista.AddRange(lrer);

            var ltotalSein = ListaTotalSein(topcodi, usuario, lectcodi).Where(x => x.Medifecha >= fechaini && x.Medifecha <= fechafin).ToList();
            CompletarTotalMeMecicion48(ref ltotalSein);
            lista.AddRange(ltotalSein);

            return lista;
        }

        /// <summary>
        /// Obtener total de generación de escenario Yupana
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="topologias"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListarTotalXEscenario(DateTime fecha, List<CpTopologiaDTO> topologias)
        {
            List<MeMedicion48DTO> listaData = new List<MeMedicion48DTO>();

            foreach (var item in topologias)
            {
                listaData.Add(GetTotalGenXEscenario(item, fecha));
            }

            return listaData;
        }

        public MeMedicion48DTO GetTotalGenXEscenario(CpTopologiaDTO topologia, DateTime fecha)
        {
            int topcodi = topologia.Topcodi;
            string usuario = "";
            DateTime fechaini = fecha;
            DateTime fechafin = fecha;
            int lectcodi = 5;

            //Coes, no Coes
            List<PrGrupodatDTO> listaOperacionCoes = FactorySic.GetPrGrupodatRepository().ListarHistoricoValores(ConstantesPR5ReportesServicio.PropGrupoOperacionCoes.ToString(), -1)
                                                        .Where(x => x.Deleted == 0).OrderByDescending(x => x.Fechadat).ToList();
            /*
            //Hidro sin rer
            List<MeMedicion48DTO> listaMe48 = ListaHidroMcp(topcodi, usuario, lectcodi, out List<MeMedicion48DTO> listaDespachoHidroXEq).Where(x => x.Medifecha >= fechaini && x.Medifecha <= fechafin).ToList();
            CompletarTotalMeMecicion48(ref listaMe48);
            lista.AddRange(listaMe48);

            //Termo sin rer
            var ltermicas = ListaTermicaMcp(topcodi, usuario, lectcodi).Where(x => x.Medifecha >= fechaini && x.Medifecha <= fechafin).ToList();
            var ltermicasCC = ListaTermicasCCMcp(topcodi, fechaini, fechafin, usuario, lectcodi);
            UnirTotalTermicas(ref ltermicas, ltermicasCC);
            CompletarTotalMeMecicion48(ref ltermicas);
            lista.AddRange(ltermicas);
            */

            //plantas Hidro Integrante
            var listaHidro = FactorySic.GetCpMedicion48Repository().GetByCriteriaRecurso(topcodi, ConstantesBase.SresPotHidro.ToString(), ConstantesYupanaContinuo.ParametroDefecto)
                .Where(x => x.Medifecha >= fechaini && x.Medifecha <= fechafin).ToList();

            //plantas TERMO Integrante
            var listaTermo = FactorySic.GetCpMedicion48Repository().GetByCriteriaRecurso(topcodi, ConstantesBase.SresPotTermica.ToString(), ConstantesYupanaContinuo.ParametroDefecto)
                .Where(x => x.Medifecha >= fechaini && x.Medifecha <= fechafin).ToList();

            //Rer, no convenciones y no integrante (Integrante y no Integrante)
            var lrer = FactorySic.GetCpMedicion48Repository().ObtieneRegistrosToDespachoRerPrGrupo(topcodi, ConstantesBase.SresPotUncr.ToString())
                .Where(x => x.Medifecha >= fechaini && x.Medifecha <= fechafin).ToList();
            //Determinar que el dato es integrante o no del COES para ese dia 
            foreach (var reg48 in lrer)
            {
                reg48.Grupointegrante = ReporteMedidoresAppServicio.SetValorCentralIntegrante(reg48.Grupocodi, reg48.Medifecha, reg48.Grupointegrante, listaOperacionCoes);
            }
            lrer = lrer.OrderBy(x => x.Grupointegrante).GroupBy(x => x.Ptomedicodi).Select(x => x.First()).ToList(); //quitar duplicados y priorizar los no integrantes
            lrer = lrer.Where(x => x.Grupointegrante == ConstantesAppServicio.SI).ToList();

            //totalizar
            List<CpMedicion48DTO> lista = new List<CpMedicion48DTO>();
            lista.AddRange(listaHidro);
            lista.AddRange(listaTermo);
            lista.AddRange(lrer);

            MeMedicion48DTO regTotal = new MeMedicion48DTO() { Medifecha = fecha };
            SetMeditotalXLista(regTotal, lista);

            return regTotal;
        }

        private void SetMeditotalXLista(MeMedicion48DTO regTotal, List<CpMedicion48DTO> lista)
        {
            decimal totalReg = 0;
            for (int h = 1; h <= 48; h++)
            {
                decimal total = 0;

                foreach (var m48 in lista)
                {
                    decimal? valor = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);
                    total += valor.GetValueOrDefault(0);
                }

                if (total > 0)
                    regTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(regTotal, total);

                totalReg += total;
            }

            regTotal.Meditotal = totalReg;
        }

        /// <summary>
        /// Obtener la lista de escenario reprograma por fecha
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<CpTopologiaDTO> ListarEscenarioReprograma(DateTime fecha)
        {
            var lista = FactorySic.GetCptopologiaRepository().ListEscenarioReprograma(fecha).OrderByDescending(x => x.Topcodi).ToList();
            return lista.Select(x => new CpTopologiaDTO() { Topcodi = x.Topcodi, Topfecha = x.Topfecha, Topnombre = x.Topnombre }).ToList();
        }

        /// <summary>
        /// Obtiene Total de Sein, tal como Demanda, Perdidas,deficit y generacion Sein en MeMedicion48
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="usuario"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaTotalSein(int topcodi, string usuario, int lectcodi)
        {
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            MeMedicion48DTO regdespacho;
            decimal? total, valor;


            string srestric = ConstantesBase.SresGeneracionSEIN.ToString() + "," + ConstantesBase.SresDemandaSEIN.ToString() + "," +
                ConstantesBase.SresPerdidasSEIN.ToString() + "," + ConstantesBase.SresDeficitSEIN.ToString();
            var lista = FactorySic.GetCpMedicion48Repository().GetByCriteriaRecurso(topcodi, srestric.ToString(), 0);
            foreach (var reg in lista)
            {
                regdespacho = new MeMedicion48DTO();
                total = null;
                for (int i = 1; i <= 48; i++)
                {
                    valor = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                    if (valor != null)
                    {
                        regdespacho.GetType().GetProperty("H" + i.ToString()).SetValue(regdespacho, valor);
                        if (total == null)
                            total = valor;
                        else
                            total += valor;
                    }
                }
                regdespacho.Meditotal = total;
                regdespacho.Medifecha = reg.Medifecha;
                switch (reg.Srestcodi)
                {
                    case ConstantesBase.SresGeneracionSEIN:
                        regdespacho.Ptomedicodi = ConstantesBase.PtoGeneracionSEIN;
                        break;
                    case ConstantesBase.SresDemandaSEIN:
                        regdespacho.Ptomedicodi = ConstantesBase.PtoDemandaSEIN;
                        break;
                    case ConstantesBase.SresPerdidasSEIN:
                        regdespacho.Ptomedicodi = ConstantesBase.PtoPerdidasSEIN;
                        break;
                    case ConstantesBase.SresDeficitSEIN:
                        regdespacho.Ptomedicodi = ConstantesBase.PtoDeficitSEIN;
                        break;
                }

                regdespacho.Tipoinfocodi = ConstantesBase.TipoInfoMWDemanda;
                regdespacho.Lastuser = usuario;
                regdespacho.Lastdate = DateTime.Now;
                regdespacho.Lectcodi = lectcodi;
                listaDespacho.Add(regdespacho);
            }


            return listaDespacho;
        }

        /// <summary>
        /// Obtiene Plantas Rer de Yupana en memedicion48
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="srestriccodi"></param>
        /// <param name="usuario"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaPlantasRerMcp(int topcodi, string srestriccodi, string usuario, int lectcodi)
        {
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            MeMedicion48DTO regdespacho;
            decimal? total, valor;
            var listaplanta = FactorySic.GetCpMedicion48Repository().ObtieneRegistrosToDespachoRerPrGrupo(topcodi, srestriccodi);
            foreach (var reg in listaplanta)
            {
                regdespacho = new MeMedicion48DTO();
                total = null;
                for (int i = 1; i <= 48; i++)
                {
                    valor = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                    if (valor != null)
                    {
                        regdespacho.GetType().GetProperty("H" + i.ToString()).SetValue(regdespacho, valor);
                        if (total == null)
                            total = valor;
                        else
                            total += valor;
                    }
                }
                regdespacho.Meditotal = total;
                regdespacho.Medifecha = reg.Medifecha;
                regdespacho.Ptomedicodi = reg.Ptomedicodi;
                regdespacho.Tipoinfocodi = 1;
                regdespacho.Lastuser = usuario;
                regdespacho.Lastdate = DateTime.Now;
                regdespacho.Lectcodi = lectcodi;
                listaDespacho.Add(regdespacho);
            }
            return listaDespacho;
        }

        /// <summary>
        /// Une Termicas con Termicas CicloCombinado
        /// </summary>
        /// <param name="lista1"></param>
        /// <param name="lista2"></param>
        public void UnirTotalTermicas(ref List<MeMedicion48DTO> lista1, List<MeMedicion48DTO> lista2)
        {
            decimal? valor, valor1;
            decimal valTot;

            foreach (var reg in lista2)
            {
                var find = lista1.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Medifecha == reg.Medifecha);
                if (find == null)
                {
                    lista1.Add(reg);
                }
                else
                {
                    for (var i = 1; i <= 48; i++)
                    {
                        valor = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                        valor1 = (decimal?)find.GetType().GetProperty("H" + i.ToString()).GetValue(find, null);
                        valTot = (valor == null) ? ((valor1 == null) ? 0 : (decimal)valor1) : ((valor1 == null) ? (decimal)valor : (decimal)valor + (decimal)valor1);
                        find.GetType().GetProperty("H" + i.ToString()).SetValue(find, valTot);

                    }
                }
            }
            return;
        }
        /// <summary>
        /// Completa el campo Meditotal
        /// </summary>
        /// <param name="lista"></param>
        public void CompletarTotalMeMecicion48(ref List<MeMedicion48DTO> lista)
        {
            decimal total = 0;
            decimal? valor;
            foreach (var reg in lista)
            {
                total = 0;
                for (int i = 1; i <= 48; i++)
                {
                    valor = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                    total += (decimal)((valor != null) ? valor : 0);
                }
                reg.Meditotal = total;
            }

        }

        /// <summary>
        /// Obtiene Termica de Ciclo Combiana de Yupana en MeMedicion48
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="usuario"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaTermicasCCMcp(int topcodi, DateTime fechaini, DateTime fechafin, string usuario, int lectcodi)
        {
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            short idPotTermica = ConstantesBase.SresPotTermica;

            var listaTermica2 = FactorySic.GetCpMedicion48Repository().ObtieneRegistrosToDespachoPTermica2(topcodi, idPotTermica);
            listaTermica2 = listaTermica2.Where(x => x.Medifecha >= fechaini && x.Medifecha <= fechafin).ToList();
            var listaCentral = listaTermica2.Where(x => x.Ptomedicodi == ConstantesBase.IdPtoFenixTv).ToList();
            listaDespacho = ActualizarDespachoFenix(listaCentral, lectcodi, fechaini, fechafin, usuario);

            listaCentral = listaTermica2.Where(x => x.Ptomedicodi == ConstantesBase.IdPtoKallpaTv).ToList();
            var lkallpa = ActualizarDespachoKallpa(listaCentral, lectcodi, fechaini, fechafin, usuario);

            listaCentral = listaTermica2.Where(x => x.Ptomedicodi == ConstantesBase.IdPtoVentanillaTv).ToList();
            var lVentanilla = ActualizarDespachoVentanilla(listaCentral, lectcodi, fechaini, fechafin, usuario);

            listaCentral = listaTermica2.Where(x => x.Ptomedicodi == ConstantesBase.IdPtoChilca1Tv).ToList();
            var lChila1 = ActualizarDespachoChica1(listaCentral, lectcodi, fechaini, fechafin, usuario);

            listaCentral = listaTermica2.Where(x => x.Ptomedicodi == ConstantesBase.IdPtoChilca2Tv).ToList();
            var lChilca2 = ActualizarDespachoChica2(listaCentral, lectcodi, fechaini, fechafin, usuario);

            listaCentral = listaTermica2.Where(x => x.Ptomedicodi == ConstantesBase.IdPtoOllerosTv).ToList();
            var lOlleros = ActualizarDespachoOlleros(listaCentral, lectcodi, fechaini, fechafin, usuario);

            listaCentral = listaTermica2.Where(x => x.Ptomedicodi == ConstantesBase.IdPtoLasFloresTv).ToList();
            var lLasFlores = ActualizarDespachoLasFlores(listaCentral, lectcodi, fechaini, fechafin, usuario);

            listaDespacho = listaDespacho.Union(lkallpa).ToList();
            listaDespacho = listaDespacho.Union(lVentanilla).ToList();
            listaDespacho = listaDespacho.Union(lChila1).ToList();
            listaDespacho = listaDespacho.Union(lChilca2).ToList();
            listaDespacho = listaDespacho.Union(lOlleros).ToList();
            listaDespacho = listaDespacho.Union(lLasFlores).ToList();

            return listaDespacho;
        }

        public List<MeMedicion48DTO> ActualizarDespachoOlleros(List<CpMedicion48DTO> lista, int lectcodi, DateTime fechaini, DateTime fechafin, string usuario)
        {
            MeMedicion48DTO regdespacho, regdespachotv;
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            for (var f = fechaini; f <= fechafin; f = f.AddDays(1))
            {

                regdespacho = new MeMedicion48DTO(); // Tg1
                regdespacho.Medifecha = f;
                regdespacho.Ptomedicodi = ConstantesBase.IdPtoOllerosTg1;
                regdespacho.Tipoinfocodi = 1;
                regdespacho.Lastuser = usuario;
                regdespacho.Lastdate = DateTime.Now;
                regdespacho.Lectcodi = lectcodi;
                listaDespacho.Add(regdespacho);

                regdespachotv = new MeMedicion48DTO(); // Tv
                regdespachotv.Medifecha = f;
                regdespachotv.Ptomedicodi = ConstantesBase.IdPtoOllerosTv;
                regdespachotv.Tipoinfocodi = 1;
                regdespachotv.Lastuser = usuario;
                regdespachotv.Lastdate = DateTime.Now;
                regdespachotv.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotv);

            }
            foreach (var reg in lista)
            {
                switch (reg.Grupocodi)
                {
                    case ConstantesBase.IdOllerosCC:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoOllerosTg1, ConstantesBase.IdPtoOllerosTv);
                        break;
                }
            }
            return listaDespacho;
        }

        public List<MeMedicion48DTO> ActualizarDespachoLasFlores(List<CpMedicion48DTO> lista, int lectcodi, DateTime fechaini, DateTime fechafin, string usuario)
        {
            MeMedicion48DTO regdespacho, regdespachotv;
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            for (var f = fechaini; f <= fechafin; f = f.AddDays(1))
            {

                regdespacho = new MeMedicion48DTO(); // Tg1
                regdespacho.Medifecha = f;
                regdespacho.Ptomedicodi = ConstantesBase.IdPtoLasFloresTg1;
                regdespacho.Tipoinfocodi = 1;
                regdespacho.Lastuser = usuario;
                regdespacho.Lastdate = DateTime.Now;
                regdespacho.Lectcodi = lectcodi;
                listaDespacho.Add(regdespacho);

                regdespachotv = new MeMedicion48DTO(); // Tv
                regdespachotv.Medifecha = f;
                regdespachotv.Ptomedicodi = ConstantesBase.IdPtoLasFloresTv;
                regdespachotv.Tipoinfocodi = 1;
                regdespachotv.Lastuser = usuario;
                regdespachotv.Lastdate = DateTime.Now;
                regdespachotv.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotv);

            }
            foreach (var reg in lista)
            {
                switch (reg.Grupocodi)
                {
                    case ConstantesBase.IdLasFloresCC:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoLasFloresTg1, ConstantesBase.IdPtoLasFloresTv);
                        break;
                }
            }
            return listaDespacho;
        }

        /// <summary>
        /// Despacho Fenix
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="usuario"></param>
        /// <param name="modo"></param>
        public List<MeMedicion48DTO> ActualizarDespachoFenix(List<CpMedicion48DTO> lista, int lectcodi, DateTime fechaini, DateTime fechafin, string usuario)
        {
            MeMedicion48DTO regdespacho, regdespachotg2, regdespachotv;
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            decimal? total, total2, totaltv, valor, x6;
            for (var f = fechaini; f <= fechafin; f = f.AddDays(1))
            {

                regdespacho = new MeMedicion48DTO(); // Tg1
                regdespacho.Medifecha = f;
                regdespacho.Ptomedicodi = ConstantesBase.IdPtoFenixTg1;
                regdespacho.Tipoinfocodi = 1;
                regdespacho.Lastuser = usuario;
                regdespacho.Lastdate = DateTime.Now;
                regdespacho.Lectcodi = lectcodi;
                listaDespacho.Add(regdespacho);

                regdespachotg2 = new MeMedicion48DTO(); // Tg2
                regdespachotg2.Medifecha = f;
                regdespachotg2.Ptomedicodi = ConstantesBase.IdPtoFenixTg2;
                regdespachotg2.Tipoinfocodi = 1;
                regdespachotg2.Lastuser = usuario;
                regdespachotg2.Lastdate = DateTime.Now;
                regdespachotg2.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotg2);

                regdespachotv = new MeMedicion48DTO(); // Tv
                regdespachotv.Medifecha = f;
                regdespachotv.Ptomedicodi = ConstantesBase.IdPtoFenixTv;
                regdespachotv.Tipoinfocodi = 1;
                regdespachotv.Lastuser = usuario;
                regdespachotv.Lastdate = DateTime.Now;
                regdespachotv.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotv);

            }
            foreach (var reg in lista)
            {

                total = null;
                totaltv = null;
                total2 = null;
                switch (reg.Grupocodi)
                {
                    case ConstantesBase.IdFenixCCTg1:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoFenixTg1, ConstantesBase.IdPtoFenixTv);
                        break;
                    case ConstantesBase.IdFenixCCTg2:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoFenixTg2, ConstantesBase.IdPtoFenixTv);
                        break;
                    case ConstantesBase.IdFenixCCTg12:
                        regdespacho = listaDespacho.Find(x => x.Ptomedicodi == ConstantesBase.IdPtoFenixTg1 && x.Medifecha == reg.Medifecha);//new MeMedicion48DTO(); // Tg1
                        regdespachotg2 = listaDespacho.Find(x => x.Ptomedicodi == ConstantesBase.IdPtoFenixTg2 && x.Medifecha == reg.Medifecha);//new MeMedicion48DTO(); // Tg2
                        regdespachotv = listaDespacho.Find(x => x.Ptomedicodi == ConstantesBase.IdPtoFenixTv && x.Medifecha == reg.Medifecha);//new MeMedicion48DTO(); // Tv

                        decimal valorAnt = 0, valorAnt2 = 0, valorAnt3 = 0;
                        for (int i = 1; i <= 48; i++)
                        {
                            valor = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                            if (valor != null)
                                if (valor != 0)
                                {
                                    if (valor > 510)
                                        x6 = 170;
                                    else
                                    {
                                        if (valor > 330 && valor < 265)
                                            x6 = 90;
                                        else
                                            x6 = valor / 3;
                                    }
                                    var valorReg = regdespacho.GetType().GetProperty("H" + i.ToString()).GetValue(regdespacho, null);
                                    if (valorReg != null)
                                    {
                                        valorAnt = (decimal)valorReg;
                                    }
                                    regdespacho.GetType().GetProperty("H" + i.ToString()).SetValue(regdespacho, valorAnt + x6);

                                    valorReg = regdespachotg2.GetType().GetProperty("H" + i.ToString()).GetValue(regdespachotg2, null);
                                    if (valorReg != null)
                                    {
                                        valorAnt2 = (decimal)valorReg;
                                    }
                                    regdespachotg2.GetType().GetProperty("H" + i.ToString()).SetValue(regdespachotg2, valorAnt2 + x6);

                                    valorReg = regdespachotv.GetType().GetProperty("H" + i.ToString()).GetValue(regdespachotv, null);
                                    if (valorReg != null)
                                    {
                                        valorAnt3 = (decimal)valorReg;
                                    }
                                    regdespachotv.GetType().GetProperty("H" + i.ToString()).SetValue(regdespachotv, valorAnt3 + valor - 2 * x6);
                                    if (total == null)
                                        total = x6;
                                    else
                                        total += valorAnt + x6;
                                    if (total2 == null)
                                        total2 = x6;
                                    else
                                        total2 += valorAnt2 + x6;

                                    if (totaltv == null)
                                        totaltv = valor - 2 * x6;
                                    else
                                        totaltv += valorAnt + valor - 2 * x6;
                                }
                        }
                        if (total != null)
                            regdespacho.Meditotal = total;
                        regdespachotg2.Meditotal = total;
                        if (totaltv != null)
                            regdespachotv.Meditotal = totaltv;

                        break;

                }
            }
            //foreach (var reg in listaDespacho)
            //{
            //    FactorySic.GetMeMedicion48Repository().Save(reg);
            //}
            return listaDespacho;
        }

        /// <summary>
        /// Despacho Kallpa
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="usuario"></param>
        /// <param name="modo"></param>
        public List<MeMedicion48DTO> ActualizarDespachoKallpa(List<CpMedicion48DTO> lista, int lectcodi, DateTime fechaini, DateTime fechafin, string usuario)
        {
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            MeMedicion48DTO regdespacho, regdespachotg2, regdespachotg3, regdespachotv;
            for (var f = fechaini; f <= fechafin; f = f.AddDays(1))
            {

                regdespacho = new MeMedicion48DTO(); // Tg1
                regdespacho.Medifecha = f;
                regdespacho.Ptomedicodi = ConstantesBase.IdPtoKallpaTg1;
                regdespacho.Tipoinfocodi = 1;
                regdespacho.Lastuser = usuario;
                regdespacho.Lastdate = DateTime.Now;
                regdespacho.Lectcodi = lectcodi;
                listaDespacho.Add(regdespacho);

                regdespachotg2 = new MeMedicion48DTO(); // Tg2
                regdespachotg2.Medifecha = f;
                regdespachotg2.Ptomedicodi = ConstantesBase.IdPtoKallpaTg2;
                regdespachotg2.Tipoinfocodi = 1;
                regdespachotg2.Lastuser = usuario;
                regdespachotg2.Lastdate = DateTime.Now;
                regdespachotg2.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotg2);

                regdespachotg3 = new MeMedicion48DTO(); // Tg3
                regdespachotg3.Medifecha = f;
                regdespachotg3.Ptomedicodi = ConstantesBase.IdPtoKallpaTg3;
                regdespachotg3.Tipoinfocodi = 1;
                regdespachotg3.Lastuser = usuario;
                regdespachotg3.Lastdate = DateTime.Now;
                regdespachotg3.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotg3);

                regdespachotv = new MeMedicion48DTO(); // Tv
                regdespachotv.Medifecha = f;
                regdespachotv.Ptomedicodi = ConstantesBase.IdPtoKallpaTv;
                regdespachotv.Tipoinfocodi = 1;
                regdespachotv.Lastuser = usuario;
                regdespachotv.Lastdate = DateTime.Now;
                regdespachotv.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotv);

            }

            foreach (var reg in lista)
            {
                switch (reg.Grupocodi)
                {
                    case ConstantesBase.IdKallpaCCTg1:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoKallpaTg1, ConstantesBase.IdPtoKallpaTv);
                        break;
                    case ConstantesBase.IdKallpaCCTg2:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoKallpaTg2, ConstantesBase.IdPtoKallpaTv);
                        break;
                    case ConstantesBase.IdKallpaCCTg3:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoKallpaTg3, ConstantesBase.IdPtoKallpaTv);
                        break;
                    case ConstantesBase.IdKallpaCCTg12:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoKallpaTg1, ConstantesBase.IdPtoKallpaTg2, ConstantesBase.IdPtoKallpaTv);
                        break;
                    case ConstantesBase.IdKallpaCCTg23:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoKallpaTg2, ConstantesBase.IdPtoKallpaTg3, ConstantesBase.IdPtoKallpaTv);
                        break;
                    case ConstantesBase.IdKallpaCCTg31:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoKallpaTg1, ConstantesBase.IdPtoKallpaTg3, ConstantesBase.IdPtoKallpaTv);
                        break;
                    case ConstantesBase.IdKallpaCCTg123:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoKallpaTg1, ConstantesBase.IdPtoKallpaTg2, ConstantesBase.IdPtoKallpaTg3, ConstantesBase.IdPtoKallpaTv);
                        break;

                }
            }
            //foreach (var reg in listaDespacho)
            //{
            //    //Borrar registro
            //    FactorySic.GetMeMedicion48Repository().Delete(reg.Lectcodi, reg.Medifecha, reg.Tipoinfocodi, reg.Ptomedicodi);
            //    FactorySic.GetMeMedicion48Repository().Save(reg);
            //}
            return listaDespacho;
        }

        /// <summary>
        /// Despacho Ventanilla
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="usuario"></param>
        /// <param name="modo"></param>
        public List<MeMedicion48DTO> ActualizarDespachoVentanilla(List<CpMedicion48DTO> lista, int lectcodi, DateTime fechaini, DateTime fechafin, string usuario)
        {
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            MeMedicion48DTO regdespacho, regdespachotg2, regdespachotv;
            for (var f = fechaini; f <= fechafin; f = f.AddDays(1))
            {

                regdespacho = new MeMedicion48DTO(); // Tg3
                regdespacho.Medifecha = f;
                regdespacho.Ptomedicodi = ConstantesBase.IdPtoVentanillaTg3;
                regdespacho.Tipoinfocodi = 1;
                regdespacho.Lastuser = usuario;
                regdespacho.Lastdate = DateTime.Now;
                regdespacho.Lectcodi = lectcodi;
                listaDespacho.Add(regdespacho);

                regdespachotg2 = new MeMedicion48DTO(); // Tg4
                regdespachotg2.Medifecha = f;
                regdespachotg2.Ptomedicodi = ConstantesBase.IdPtoVentanillaTg4;
                regdespachotg2.Tipoinfocodi = 1;
                regdespachotg2.Lastuser = usuario;
                regdespachotg2.Lastdate = DateTime.Now;
                regdespachotg2.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotg2);

                regdespachotv = new MeMedicion48DTO(); // Tv
                regdespachotv.Medifecha = f;
                regdespachotv.Ptomedicodi = ConstantesBase.IdPtoVentanillaTv;
                regdespachotv.Tipoinfocodi = 1;
                regdespachotv.Lastuser = usuario;
                regdespachotv.Lastdate = DateTime.Now;
                regdespachotv.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotv);

            }
            foreach (var reg in lista)
            {
                switch (reg.Grupocodi)
                {
                    case ConstantesBase.IdVentanillaCCTg3:
                    case ConstantesBase.IdVentanillaCCTg3FD:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoVentanillaTg3, ConstantesBase.IdPtoVentanillaTv);
                        break;
                    case ConstantesBase.IdVentanillaCCTg4:
                    case ConstantesBase.IdVentanillaCCTg4FD:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoVentanillaTg4, ConstantesBase.IdPtoVentanillaTv);
                        break;
                    case ConstantesBase.IdVentanillaCCTg34:
                    case ConstantesBase.IdVentanillaCCTg34FD:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoVentanillaTg3, ConstantesBase.IdPtoVentanillaTg4, ConstantesBase.IdPtoVentanillaTv);
                        break;

                }
            }
            //foreach (var reg in listaDespacho)
            //{
            //    FactorySic.GetMeMedicion48Repository().Delete(reg.Lectcodi, reg.Medifecha, reg.Tipoinfocodi, reg.Ptomedicodi);
            //    FactorySic.GetMeMedicion48Repository().Save(reg);
            //}
            return listaDespacho;
        }

        public List<MeMedicion48DTO> ActualizarDespachoChica1(List<CpMedicion48DTO> lista, int lectcodi, DateTime fechaini, DateTime fechafin, string usuario)
        {
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            MeMedicion48DTO regdespacho, regdespachotg2, regdespachotg3, regdespachotv;
            for (var f = fechaini; f <= fechafin; f = f.AddDays(1))
            {

                regdespacho = new MeMedicion48DTO(); // Tg1
                regdespacho.Medifecha = f;
                regdespacho.Ptomedicodi = ConstantesBase.IdPtoChilca1Tg1;
                regdespacho.Tipoinfocodi = 1;
                regdespacho.Lastuser = usuario;
                regdespacho.Lastdate = DateTime.Now;
                regdespacho.Lectcodi = lectcodi;
                listaDespacho.Add(regdespacho);

                regdespachotg2 = new MeMedicion48DTO(); // Tg2
                regdespachotg2.Medifecha = f;
                regdespachotg2.Ptomedicodi = ConstantesBase.IdPtoChilca1Tg2;
                regdespachotg2.Tipoinfocodi = 1;
                regdespachotg2.Lastuser = usuario;
                regdespachotg2.Lastdate = DateTime.Now;
                regdespachotg2.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotg2);

                regdespachotg3 = new MeMedicion48DTO(); // Tg3
                regdespachotg3.Medifecha = f;
                regdespachotg3.Ptomedicodi = ConstantesBase.IdPtoChilca1Tg3;
                regdespachotg3.Tipoinfocodi = 1;
                regdespachotg3.Lastuser = usuario;
                regdespachotg3.Lastdate = DateTime.Now;
                regdespachotg3.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotg3);

                regdespachotv = new MeMedicion48DTO(); // Tv
                regdespachotv.Medifecha = f;
                regdespachotv.Ptomedicodi = ConstantesBase.IdPtoChilca1Tv;
                regdespachotv.Tipoinfocodi = 1;
                regdespachotv.Lastuser = usuario;
                regdespachotv.Lastdate = DateTime.Now;
                regdespachotv.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotv);

            }
            foreach (var reg in lista)
            {
                switch (reg.Grupocodi)
                {
                    case ConstantesBase.IdChilca1CCTg1:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoChilca1Tg1, ConstantesBase.IdPtoChilca1Tv);
                        break;
                    case ConstantesBase.IdChilca1CCTg2:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoChilca1Tg2, ConstantesBase.IdPtoChilca1Tv);
                        break;
                    case ConstantesBase.IdChilca1CCTg3:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoChilca1Tg3, ConstantesBase.IdPtoChilca1Tv);
                        break;
                    case ConstantesBase.IdChilca1CCTg12:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoChilca1Tg1, ConstantesBase.IdPtoChilca1Tg2, ConstantesBase.IdPtoChilca1Tv);
                        break;
                    case ConstantesBase.IdChilca1CCTg23:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoChilca1Tg2, ConstantesBase.IdPtoChilca1Tg3, ConstantesBase.IdPtoChilca1Tv);
                        break;
                    case ConstantesBase.IdChilca1CCTg31:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoChilca1Tg1, ConstantesBase.IdPtoChilca1Tg3, ConstantesBase.IdPtoChilca1Tv);
                        break;
                    case ConstantesBase.IdChilca1CCTg123:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoChilca1Tg1, ConstantesBase.IdPtoChilca1Tg2, ConstantesBase.IdPtoChilca1Tg3, ConstantesBase.IdPtoChilca1Tv);
                        break;

                }
            }
            //foreach (var reg in listaDespacho)
            //{
            //    FactorySic.GetMeMedicion48Repository().Delete(reg.Lectcodi, reg.Medifecha, reg.Tipoinfocodi, reg.Ptomedicodi);
            //    FactorySic.GetMeMedicion48Repository().Save(reg);
            //}
            return listaDespacho;
        }

        public List<MeMedicion48DTO> ActualizarDespachoChica2(List<CpMedicion48DTO> lista, int lectcodi, DateTime fechaini, DateTime fechafin, string usuario)
        {

            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            MeMedicion48DTO regdespacho, regdespachotv;
            for (var f = fechaini; f <= fechafin; f = f.AddDays(1))
            {

                regdespacho = new MeMedicion48DTO(); // Tg4
                regdespacho.Medifecha = f;
                regdespacho.Ptomedicodi = ConstantesBase.IdPtoChilca2Tg4;
                regdespacho.Tipoinfocodi = 1;
                regdespacho.Lastuser = usuario;
                regdespacho.Lastdate = DateTime.Now;
                regdespacho.Lectcodi = lectcodi;
                listaDespacho.Add(regdespacho);

                regdespachotv = new MeMedicion48DTO(); // Tv
                regdespachotv.Medifecha = f;
                regdespachotv.Ptomedicodi = ConstantesBase.IdPtoChilca2Tv;
                regdespachotv.Tipoinfocodi = 1;
                regdespachotv.Lastuser = usuario;
                regdespachotv.Lastdate = DateTime.Now;
                regdespachotv.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotv);

            }
            foreach (var reg in lista)
            {
                switch (reg.Grupocodi)
                {
                    case ConstantesBase.IdChilca2CCTg4:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoChilca2Tg4, ConstantesBase.IdPtoChilca2Tv);
                        break;
                }
            }
            //foreach (var reg in listaDespacho)
            //{
            //    FactorySic.GetMeMedicion48Repository().Delete(reg.Lectcodi, reg.Medifecha, reg.Tipoinfocodi, reg.Ptomedicodi);
            //    FactorySic.GetMeMedicion48Repository().Save(reg);
            //}
            return listaDespacho;
        }

        /// <summary>
        /// Distribuye la potencia del modo de operacion entre una Tg y La Tv
        /// </summary>
        /// <param name="listaDespacho"></param>
        /// <param name="registro"></param>
        /// <param name="usuario"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fecha"></param>
        /// <param name="ptoTg1"></param>
        /// <param name="ptoTv"></param>
        public void DistribuyeModoAGrupos(ref List<MeMedicion48DTO> listaDespacho, CpMedicion48DTO registro, string usuario, int lectcodi, int ptoTg1, int ptoTg2, int ptoTv)
        {
            MeMedicion48DTO regdespacho, regdespachotg2, regdespachotv;
            decimal? total = null, total2 = null, totaltv = null, valor;
            decimal valorAnt = 0, valorAnt2 = 0, valorAnt3 = 0;
            regdespacho = listaDespacho.Find(x => x.Ptomedicodi == ptoTg1 && x.Medifecha == registro.Medifecha); // new MeMedicion48DTO(); // Tg1
            regdespachotg2 = listaDespacho.Find(x => x.Ptomedicodi == ptoTg2 && x.Medifecha == registro.Medifecha); // new MeMedicion48DTO(); // Tg2
            regdespachotv = listaDespacho.Find(x => x.Ptomedicodi == ptoTv && x.Medifecha == registro.Medifecha); // new MeMedicion48DTO(); // Tv

            for (int i = 1; i <= 48; i++)
            {
                valor = (decimal?)registro.GetType().GetProperty("H" + i.ToString()).GetValue(registro, null);
                if (valor != null)
                    if (valor != 0)
                    {
                        var valorReg = regdespacho.GetType().GetProperty("H" + i.ToString()).GetValue(regdespacho, null);
                        if (valorReg != null)
                        {
                            valorAnt = (decimal)valorReg;
                        }
                        regdespacho.GetType().GetProperty("H" + i.ToString()).SetValue(regdespacho, valorAnt + 1 * valor / 3);
                        valorReg = regdespachotg2.GetType().GetProperty("H" + i.ToString()).GetValue(regdespachotg2, null);
                        if (valorReg != null)
                        {
                            valorAnt2 = (decimal)valorReg;
                        }
                        regdespachotg2.GetType().GetProperty("H" + i.ToString()).SetValue(regdespachotg2, valorAnt2 + 1 * valor / 3);
                        valorReg = regdespachotv.GetType().GetProperty("H" + i.ToString()).GetValue(regdespachotv, null);
                        if (valorReg != null)
                        {
                            valorAnt3 = (decimal)valorReg;
                        }
                        regdespachotv.GetType().GetProperty("H" + i.ToString()).SetValue(regdespachotv, valorAnt3 + 1 * valor / 3);
                        if (total == null)
                            total = 1 * valor / 3;
                        else
                            total += valorAnt + 1 * valor / 3;
                        if (total2 == null)
                            total2 = 1 * valor / 3;
                        else
                            total2 += valorAnt2 + 1 * valor / 3;
                        if (totaltv == null)
                            totaltv = valor / 3;
                        else
                            totaltv += valorAnt3 + valor / 3;

                    }
            }
            if (total != null)
                regdespacho.Meditotal = total;
            if (total2 != null)
                regdespachotg2.Meditotal = total2;
            if (totaltv != null)
                regdespachotv.Meditotal = totaltv;
        }

        public void DistribuyeModoAGrupos(ref List<MeMedicion48DTO> listaDespacho, CpMedicion48DTO registro, string usuario, int lectcodi, int ptoTg1, int ptoTv)
        {
            MeMedicion48DTO regdespacho, regdespachotv;
            decimal? total = null, totaltv = null, valor;
            regdespacho = listaDespacho.Find(x => x.Ptomedicodi == ptoTg1 && x.Medifecha == registro.Medifecha); // Tg1
            regdespachotv = listaDespacho.Find(x => x.Ptomedicodi == ptoTv && x.Medifecha == registro.Medifecha); // Tv
            decimal valorAnt = 0, valorAnt2 = 0;
            for (int i = 1; i <= 48; i++)
            {
                valor = (decimal?)registro.GetType().GetProperty("H" + i.ToString()).GetValue(registro, null);
                if (valor != null)
                    if (valor != 0)
                    {
                        var valorReg = regdespacho.GetType().GetProperty("H" + i.ToString()).GetValue(regdespacho, null);
                        if (valorReg != null)
                        {
                            valorAnt = (decimal)valorReg;
                        }
                        regdespacho.GetType().GetProperty("H" + i.ToString()).SetValue(regdespacho, valorAnt + 2 * valor / 3);
                        valorReg = regdespachotv.GetType().GetProperty("H" + i.ToString()).GetValue(regdespachotv, null);
                        if (valorReg != null)
                        {
                            valorAnt2 = (decimal)valorReg;
                        }
                        regdespachotv.GetType().GetProperty("H" + i.ToString()).SetValue(regdespachotv, valorAnt2 + valor / 3);
                        if (total == null)
                            total = 2 * valor / 3;
                        else
                            total += valorAnt + 2 * valor / 3;
                        if (totaltv == null)
                            totaltv = valor / 3;
                        else
                            totaltv += valorAnt2 + valor / 3;

                    }
            }
            if (total != null)
                regdespacho.Meditotal = total;
            if (totaltv != null)
                regdespachotv.Meditotal = totaltv;
        }

        public void DistribuyeModoAGrupos(ref List<MeMedicion48DTO> listaDespacho, CpMedicion48DTO registro, string usuario, int lectcodi, int ptoTg1, int ptoTg2, int ptoTg3, int ptoTv)
        {
            MeMedicion48DTO regdespacho, regdespachotg2, regdespachotg3, regdespachotv;
            decimal? total = null, total2 = null, total3 = null, totaltv = null, valor;
            decimal valorAnt = 0, valorAnt2 = 0, valorAnt3 = 0, valorAnt4 = 0;
            regdespacho = listaDespacho.Find(x => x.Ptomedicodi == ptoTg1 && x.Medifecha == registro.Medifecha); //new MeMedicion48DTO(); // Tg1
            regdespachotg2 = listaDespacho.Find(x => x.Ptomedicodi == ptoTg2 && x.Medifecha == registro.Medifecha); //new MeMedicion48DTO(); // Tg2
            regdespachotg3 = listaDespacho.Find(x => x.Ptomedicodi == ptoTg3 && x.Medifecha == registro.Medifecha); //new MeMedicion48DTO(); // Tg3
            regdespachotv = listaDespacho.Find(x => x.Ptomedicodi == ptoTv && x.Medifecha == registro.Medifecha);//new MeMedicion48DTO(); // Tv

            for (int i = 1; i <= 48; i++)
            {
                valor = (decimal?)registro.GetType().GetProperty("H" + i.ToString()).GetValue(registro, null);
                if (valor != null)
                    if (valor != 0)
                    {
                        var valorReg = regdespacho.GetType().GetProperty("H" + i.ToString()).GetValue(regdespacho, null);
                        if (valorReg != null)
                        {
                            valorAnt = (decimal)valorReg;
                        }
                        regdespacho.GetType().GetProperty("H" + i.ToString()).SetValue(regdespacho, valorAnt + 2 * valor / 9);
                        valorReg = regdespachotg2.GetType().GetProperty("H" + i.ToString()).GetValue(regdespachotg2, null);
                        if (valorReg != null)
                        {
                            valorAnt2 = (decimal)valorReg;
                        }
                        regdespachotg2.GetType().GetProperty("H" + i.ToString()).SetValue(regdespachotg2, valorAnt2 + 2 * valor / 9);
                        valorReg = regdespachotg3.GetType().GetProperty("H" + i.ToString()).GetValue(regdespachotg3, null);
                        if (valorReg != null)
                        {
                            valorAnt3 = (decimal)valorReg;
                        }
                        regdespachotg3.GetType().GetProperty("H" + i.ToString()).SetValue(regdespachotg3, valorAnt3 + 2 * valor / 9);
                        valorReg = regdespachotv.GetType().GetProperty("H" + i.ToString()).GetValue(regdespachotv, null);
                        if (valorReg != null)
                        {
                            valorAnt4 = (decimal)valorReg;
                        }
                        regdespachotv.GetType().GetProperty("H" + i.ToString()).SetValue(regdespachotv, valorAnt4 + 1 * valor / 3);
                        if (total == null)
                            total = 2 * valor / 9;
                        else
                            total += valorAnt + 2 * valor / 9;
                        if (total2 == null)
                            total2 = 2 * valor / 9;
                        else
                            total2 += valorAnt2 + 2 * valor / 9;
                        if (total3 == null)
                            total3 = 2 * valor / 9;
                        else
                            total3 += valorAnt3 + 2 * valor / 9;
                        if (totaltv == null)
                            totaltv = valor / 3;
                        else
                            totaltv += valorAnt4 + valor / 3;

                    }
            }
            if (total != null)
                regdespacho.Meditotal = total;
            if (total2 != null)
                regdespachotg2.Meditotal = total2;
            if (total3 != null)
                regdespachotg3.Meditotal = total3;
            if (totaltv != null)
                regdespachotv.Meditotal = totaltv;
        }

        /// <summary>
        /// Lista de Despacho Hidro
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="usuario"></param>
        /// <param name="lectcodi"></param>
        /// <param name="listaDespachoXEq"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaHidroMcp(int topcodi, string usuario, int lectcodi, out List<MeMedicion48DTO> listaDespachoXEq)
        {
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            listaDespachoXEq = new List<MeMedicion48DTO>();

            MeMedicion48DTO regdespacho;
            decimal? total, valor, valorParcial;
            short idPotHidro = ConstantesBase.SresPotHidro;
            var listaHidro = FactorySic.GetCpMedicion48Repository().ObtieneRegistrosPHToDespacho(topcodi, idPotHidro, ConstantesBase.IdOrigLectDespacho);

            foreach (var reg in listaHidro)
            {

                total = null;
                bool nuevoreg = false;

                //agrupar por punto de medición 
                regdespacho = listaDespacho.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Lectcodi == lectcodi && x.Medifecha == reg.Medifecha);
                if (regdespacho == null)
                {
                    regdespacho = new MeMedicion48DTO();
                    nuevoreg = true;
                }
                for (int i = 1; i <= 48; i++)
                {
                    valor = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                    if (!nuevoreg)
                    {
                        valorParcial = (decimal?)regdespacho.GetType().GetProperty("H" + i.ToString()).GetValue(regdespacho, null);
                        if (valorParcial != null)
                        {
                            valor = (valor == null) ? valorParcial : valor + valorParcial;
                        }
                    }
                    if (valor != null)
                    {
                        regdespacho.GetType().GetProperty("H" + i.ToString()).SetValue(regdespacho, valor);
                        if (total == null)
                            total = valor;
                        else
                            total += valor;
                    }
                }

                if (nuevoreg)
                {
                    regdespacho.Meditotal = total;
                    regdespacho.Medifecha = reg.Medifecha;
                    regdespacho.Ptomedicodi = reg.Ptomedicodi;
                    regdespacho.Tipoinfocodi = 1;
                    regdespacho.Lastuser = usuario;
                    regdespacho.Lastdate = DateTime.Now;
                    regdespacho.Lectcodi = lectcodi;
                    listaDespacho.Add(regdespacho);
                }

                //la planta hidráulica es un generador de CH
                if (reg.Recurfamsic == 2)
                {
                    regdespacho = new MeMedicion48DTO();
                    total = null;

                    for (int i = 1; i <= 48; i++)
                    {
                        valor = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                        if (valor != null)
                        {
                            regdespacho.GetType().GetProperty("H" + i.ToString()).SetValue(regdespacho, valor);
                            if (total == null)
                                total = valor;
                            else
                                total += valor;
                        }
                    }

                    regdespacho.Meditotal = total;
                    regdespacho.Medifecha = reg.Medifecha;
                    regdespacho.Ptomedicodi = reg.Ptomedicodi;
                    regdespacho.Equicodi = reg.Recurcodisicoes;
                    regdespacho.Tipoinfocodi = 1;
                    regdespacho.Lastuser = usuario;
                    regdespacho.Lastdate = DateTime.Now;
                    regdespacho.Lectcodi = lectcodi;

                    listaDespachoXEq.Add(regdespacho);
                }
            }
            //var lcalculado = listaHidro.Where(x => x.Ptomedicalculado == "S").ToList();
            //foreach (var p in lcalculado)
            //{
            //    var find = listaDespacho.Find(x => x.Ptomedicodi == p.Ptomedicodi && x.Medifecha == p.MEDIFECHA);
            //    if (find != null)
            //    {
            //        CalculadoHidro(listaDespacho, ref find);

            //    }
            //}

            return listaDespacho;
        }

        /// <summary>
        /// Devuelve Lista de Termicas de Yupana en MeMedicion48
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="usuario"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaTermicaMcp(int topcodi, string usuario, int lectcodi)
        {
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            MeMedicion48DTO regdespacho;
            decimal? total, valor;
            short idPotTermica = ConstantesBase.SresPotTermica;

            var listaTermica1 = FactorySic.GetCpMedicion48Repository().ObtieneRegistrosToDespachoPTermica1(topcodi, idPotTermica);
            foreach (var reg in listaTermica1)
            {
                regdespacho = new MeMedicion48DTO();
                total = null;
                for (int i = 1; i <= 48; i++)
                {
                    valor = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                    if (valor != null)
                    {
                        regdespacho.GetType().GetProperty("H" + i.ToString()).SetValue(regdespacho, valor);
                        if (total == null)
                            total = valor;
                        else
                            total += valor;
                    }
                }
                regdespacho.Meditotal = total;
                regdespacho.Medifecha = reg.Medifecha;
                regdespacho.Ptomedicodi = reg.Ptomedicodi;
                regdespacho.Tipoinfocodi = 1;
                regdespacho.Lastuser = usuario;
                regdespacho.Lastdate = DateTime.Now;
                regdespacho.Lectcodi = lectcodi;
                listaDespacho.Add(regdespacho);
            }
            return listaDespacho;
        }

        /// <summary>
        /// Lista de Flujos
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaFlujosMcp(int topcodi, List<MePtomedicionDTO> lPuntos, DateTime fechaInicio, DateTime fechaFin, string usuario, int lectcodi)
        {
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            short idFlujo = ConstantesBase.SresFlujos;
            var listaFlujo = FactorySic.GetCpMedicion48Repository().ObtieneRegistrosToDespacho(topcodi, idFlujo, ConstantesAppServicio.OriglectcodiFlujos).Where(x => x.Medifecha >= fechaInicio && x.Medifecha <= fechaFin).ToList();

            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            foreach (var regCalc in lPuntos)
            {

                var listaPunto = this.GetListaCalculadaM48Recursivo(listaFlujo, fechaInicio, fechaFin, regCalc.Ptomedicodi, lectcodi, usuario);
                listaDespacho.AddRange(listaPunto);
            }

            return listaDespacho;
        }

        /// <summary>
        /// Funcion recursiva que devuelve la data de un punto calculado
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="reg"></param>
        /// <param name="nivel"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetListaCalculadaM48Recursivo(List<CpMedicion48DTO> listaFlujo, DateTime fechaInicio, DateTime fechaFin, int pto, int lectcodi, string usuario)
        {
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            var allPtos = FactorySic.GetMeRelacionptoRepository().GetByCriteria(pto.ToString(), "-1");
            decimal? valor, valorAcum;
            bool considera = false;
            //Data de los puntos de medicion
            for (var fecha = fechaInicio; fecha <= fechaFin; fecha = fecha.AddDays(1))
            {
                considera = false;
                MeMedicion48DTO flujo = new MeMedicion48DTO();
                foreach (var regMed in allPtos)
                {
                    flujo.Ptomedicodi = pto;
                    flujo.Lectcodi = lectcodi;
                    flujo.Tipoinfocodi = ConstantesAppServicio.TipoinfocodiFlujo;
                    flujo.Medifecha = fecha;
                    flujo.Lastuser = usuario;
                    flujo.Lastdate = DateTime.Now;

                    var listaDataXPtoMedicionTmp = listaFlujo.Where(x => x.Ptomedicodi == regMed.Ptomedicodi2 && x.Medifecha == fecha);
                    decimal factor = (regMed.Relptofactor != null) ? (decimal)regMed.Relptofactor : 1;
                    foreach (var tmp in listaDataXPtoMedicionTmp)
                    {
                        considera = true;
                        //CAlcular Hs para registro reg
                        for (int i = 1; i <= 48; i++)
                        {
                            valor = (decimal?)tmp.GetType().GetProperty("H" + i.ToString()).GetValue(tmp, null);
                            valorAcum = (decimal?)flujo.GetType().GetProperty("H" + i.ToString()).GetValue(flujo, null);
                            if (valor != null)
                            {
                                if (valorAcum != null)
                                {
                                    valorAcum += factor * valor;
                                }
                                else
                                {
                                    valorAcum = factor * valor;
                                }
                            }
                            flujo.GetType().GetProperty("H" + i.ToString()).SetValue(flujo, valorAcum);
                        }
                    }
                }
                if (considera)
                {
                    lista.Add(flujo);
                }
            }
            return lista;
        }

        /// <summary>
        /// Lista de Costo Marginales
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaCMarginalMcp(int topcodi, List<MePtomedicionDTO> lPuntos, DateTime fechaInicio, DateTime fechaFin, string usuario, int lectcodi)
        {
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            decimal? valor;
            short idFCmar = ConstantesBase.SresCostoMarginalBarra;
            var listaCmar = FactorySic.GetCpMedicion48Repository().ObtieneRegistrosToBarra(topcodi, idFCmar, ConstantesAppServicio.OriglectcodiDespachomediahora).Where(x => x.Medifecha >= fechaInicio && x.Medifecha <= fechaFin).ToList();

            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            var listaFinal = listaCmar.Where(x => lPuntos.Any(y => y.Ptomedicodi == x.Ptomedicodi)).ToList();
            foreach (var regCalc in listaFinal)
            {
                MeMedicion48DTO reg = new MeMedicion48DTO();
                reg.Ptomedicodi = regCalc.Ptomedicodi;
                reg.Medifecha = regCalc.Medifecha;
                reg.Tipoinfocodi = ConstantesAppServicio.TipoinfocodiDolares;
                reg.Lectcodi = lectcodi;
                reg.Lastuser = usuario;
                for (var i = 1; i <= 48; i++)
                {
                    valor = (decimal?)regCalc.GetType().GetProperty("H" + i.ToString()).GetValue(regCalc, null);
                    reg.GetType().GetProperty("H" + i.ToString()).SetValue(reg, valor);
                }
                listaDespacho.Add(reg);
            }

            return listaDespacho;
        }

        /// <summary>
        /// Obtener topcodi oficial
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public int GetTopcodiOficialProgramado(DateTime fechaConsulta, string tipo)
        {
            //si no existe reprogramado, utilizar el programado diario
            CpTopologiaDTO regTopProg = FactorySic.GetCpTopologiaRepository().GetByFechaTopfinal(fechaConsulta.Date, tipo);
            if (regTopProg != null)
                return regTopProg.Topcodi;

            return 0;
        }

        #endregion

        #region Reporte demanda x área (migración CDispatch)

        public List<MeMedicion48DTO> ListarDataReporteDemandaXAreaXPrincipal(DateTime f_1, DateTime f_2)
        {
            //insumos
            List<MePtomedicionDTO> listaPtoFromReporte = FactorySic.GetMeReporptomedRepository().ListarPuntoReporte(ConstantesMigraciones.IdReporteDemandaAreaPrincipal, f_1.Date)
                                                .OrderBy(x => x.Repptoorden).Select(x => new MePtomedicionDTO()
                                                {
                                                    Ptomedicodi = x.Ptomedicodi,
                                                    Ptomedielenomb = x.Repptonomb
                                                }).ToList();
            List<MeMedicion48DTO> listaInsumo = GetListaObtenerMedicion48(f_1, f_2, ConstantesAppServicio.LectcodiDemandaxarea.ToString(), ConstantesAppServicio.TipoinfocodiMW, "-1");

            //reporte
            List<MeMedicion48DTO> listaSalida = new List<MeMedicion48DTO>();

            foreach (var d in listaPtoFromReporte)
            {
                listaSalida.AddRange(this.FillDatosDemandaArea(f_1, f_2, listaInsumo, listaPtoFromReporte, d.Ptomedicodi));
            }

            return listaSalida;
        }

        public List<MeMedicion48DTO> ListarDataReporteDemandaXAreaXHorizonte(int tipo, DateTime f_1, DateTime f_2, string anio, string sem1, string sem2)
        {
            var listaPtos = new List<MeMedicion48DTO>();
            var listaDetalle = new List<MeMedicion48DTO>();
            var listaTotal = new List<MeMedicion48DTO>();

            #region principal, diario y semanal

            listaPtos.Add(new MeMedicion48DTO() { Ptomedicodi = 3004 });
            listaPtos.Add(new MeMedicion48DTO() { Ptomedicodi = 3005 });
            listaPtos.Add(new MeMedicion48DTO() { Ptomedicodi = 3006 });

            if (listaPtos.Count > 0)
            {
                var list = GetListaObtenerMedicion48(f_1, f_2, ConstantesAppServicio.ParametroDefecto, int.Parse(ConstantesAppServicio.ParametroDefecto), string.Join(",", listaPtos.Select(x => x.Ptomedicodi).ToList()));

                var listNombPtos = GetListaMePtomedicionxPtos(string.Join(",", listaPtos.Select(x => x.Ptomedicodi).ToList())).OrderBy(x => x.Ptomedicodi).ToList();

                foreach (var d in listaPtos)
                {
                    if (tipo == 2)
                    {
                        listaDetalle.AddRange(this.FillDatosDemandaArea(f_1, f_2, list, listNombPtos, d.Ptomedicodi));
                    }
                    if (tipo == 3)
                    {
                        for (int s = int.Parse(sem1); s <= int.Parse(sem2); s++)
                        {
                            DateTime ff1 = EPDate.f_fechainiciosemana(int.Parse(anio), s);
                            DateTime ff2 = EPDate.f_fechafinsemana(int.Parse(anio), s);

                            var list_ = this.FillDatosDemandaArea(ff1, ff2, list, listNombPtos, d.Ptomedicodi);
                            var ent = new MeMedicion48DTO();
                            decimal meditotal_ = 0, maximadem_ = 0;
                            DateTime fec_ = DateTime.MinValue;
                            foreach (var dd in list_)
                            {
                                meditotal_ += (dd.Meditotal != null ? (decimal)dd.Meditotal : 0);
                                if (maximadem_ <= dd.MaxDemanda)
                                {
                                    maximadem_ = (decimal)dd.MaxDemanda;
                                    fec_ = dd.Medifecha;
                                }
                                ent.Ptomedinomb = dd.Ptomedinomb;
                            }
                            ent.Meditotal = meditotal_;
                            ent.MaxDemanda = maximadem_;
                            ent.Medifecha = fec_;
                            ent.Ptomedicodi = d.Ptomedicodi;
                            ent.Semana = s.ToString();
                            ent.Anio = anio;
                            listaDetalle.Add(ent);
                        }
                    }
                    if (tipo == 4)
                    {
                        for (int a = f_1.Year; a <= f_2.Year; a++)
                        {
                            for (int s = f_1.Month; s < f_2.AddDays(1).Month; s++)
                            {
                                DateTime ff1 = new DateTime(a, s, 1);
                                DateTime ff2 = ff1.AddMonths(1).AddDays(-1);

                                var list_ = this.FillDatosDemandaArea(ff1, ff2, list, listNombPtos, d.Ptomedicodi);
                                var ent = new MeMedicion48DTO();
                                decimal meditotal_ = 0, maximadem_ = 0;
                                DateTime fec_ = DateTime.MinValue;
                                foreach (var dd in list_)
                                {
                                    meditotal_ += (dd.Meditotal != null ? (decimal)dd.Meditotal : 0);
                                    if (maximadem_ <= dd.MaxDemanda)
                                    {
                                        maximadem_ = (decimal)dd.MaxDemanda;
                                        fec_ = dd.Medifecha;
                                    }
                                    ent.Ptomedinomb = dd.Ptomedinomb;
                                }
                                ent.Meditotal = meditotal_;
                                ent.MaxDemanda = maximadem_;
                                ent.Medifecha = fec_;
                                ent.Ptomedicodi = d.Ptomedicodi;
                                ent.Mes = s.ToString();
                                ent.Anio = a.ToString();
                                listaDetalle.Add(ent);
                            }
                        }
                    }
                    if (tipo == 5)
                    {
                        for (int s = f_1.Year; s <= f_2.Year; s++)
                        {
                            DateTime ff1 = new DateTime(s, 1, 1);
                            DateTime ff2 = ff1.AddYears(1).AddDays(-1);

                            var list_ = this.FillDatosDemandaArea(ff1, ff2, list, listNombPtos, d.Ptomedicodi);
                            var ent = new MeMedicion48DTO();
                            decimal meditotal_ = 0, maximadem_ = 0;
                            DateTime fec_ = DateTime.MinValue;
                            foreach (var dd in list_)
                            {
                                meditotal_ += (dd.Meditotal != null ? (decimal)dd.Meditotal : 0);
                                if (maximadem_ <= dd.MaxDemanda)
                                {
                                    maximadem_ = (decimal)dd.MaxDemanda;
                                    fec_ = dd.Medifecha;
                                }
                                ent.Ptomedinomb = dd.Ptomedinomb;
                            }
                            ent.Meditotal = meditotal_;
                            ent.MaxDemanda = maximadem_;
                            ent.Medifecha = fec_;
                            ent.Ptomedicodi = d.Ptomedicodi;
                            ent.Anio = s.ToString();
                            listaDetalle.Add(ent);
                        }
                    }
                }
            }

            #endregion

            #region Calculo para registro SEIN TOTAL


            if (tipo == 2)
            {
                for (var s = f_1; s <= f_2; s = s.AddDays(1))
                {
                    var det = listaDetalle.Where(x => x.Medifecha == s).ToList();
                    listaTotal.AddRange(det);
                    decimal meditotal_ = 0, maximadem_ = 0;
                    DateTime fec_ = DateTime.MinValue;
                    var obj = new MeMedicion48DTO();
                    decimal maxdem = 0;
                    foreach (var d in det)
                    {
                        meditotal_ += (decimal)d.Meditotal;
                        maximadem_ += (decimal)d.MaxDemanda;
                        fec_ = d.Medifecha;
                    }
                    for (int i = 1; i <= 48; i++)
                    {
                        decimal val_ = 0;
                        foreach (var d in det)
                        {
                            decimal? val = (decimal?)d.GetType().GetProperty("H" + i).GetValue(d, null);
                            val_ += (val != null) ? (decimal)val : 0;
                        }
                        obj.GetType().GetProperty("H" + i).SetValue(obj, val_);
                        if (maxdem <= val_)
                        {
                            maxdem = val_;
                        }
                    }
                    obj.Ptomedinomb = "SEIN TOTAL";
                    obj.Medifecha = fec_;
                    obj.Meditotal = meditotal_;
                    obj.MaxDemanda = maxdem;
                    listaTotal.Add(obj);
                }
            }
            if (tipo == 3)
            {
                for (int s = int.Parse(sem1); s <= int.Parse(sem2); s++)
                {
                    DateTime ff1 = EPDate.f_fechainiciosemana(int.Parse(anio), s);
                    DateTime ff2 = EPDate.f_fechafinsemana(int.Parse(anio), s);

                    var det = listaDetalle.Where(x => x.Medifecha >= ff1 && x.Medifecha <= ff2).ToList();
                    listaTotal.AddRange(det);
                    decimal meditotal_ = 0, maximadem_ = 0;
                    DateTime fec_ = DateTime.MinValue;
                    foreach (var d in det)
                    {
                        meditotal_ += (decimal)d.Meditotal;
                        maximadem_ += (decimal)d.MaxDemanda;
                        fec_ = d.Medifecha;
                    }
                    listaTotal.Add(new MeMedicion48DTO()
                    {
                        Ptomedinomb = "SEIN TOTAL",
                        Medifecha = fec_,
                        Meditotal = meditotal_,
                        MaxDemanda = maximadem_,
                        Semana = s.ToString(),
                        Anio = anio
                    });
                }
            }
            if (tipo == 4)
            {
                for (int a = f_1.Year; a <= f_2.Year; a++)
                {
                    for (int s = f_1.Month; s < f_2.AddDays(1).Month; s++)
                    {
                        var det = listaDetalle.Where(x => x.Medifecha.Year == a && x.Medifecha.Month == s).ToList();
                        listaTotal.AddRange(det);
                        decimal meditotal_ = 0, maximadem_ = 0;
                        DateTime fec_ = DateTime.MinValue;
                        foreach (var d in det)
                        {
                            meditotal_ += (decimal)d.Meditotal;
                            maximadem_ += (decimal)d.MaxDemanda;
                            fec_ = d.Medifecha;
                        }
                        listaTotal.Add(new MeMedicion48DTO()
                        {
                            Ptomedinomb = "SEIN TOTAL",
                            Medifecha = fec_,
                            Meditotal = meditotal_,
                            MaxDemanda = maximadem_,
                            Mes = s.ToString(),
                            Anio = a.ToString()
                        });
                    }
                }
            }
            if (tipo == 5)
            {
                for (int s = f_1.Year; s < f_2.AddYears(1).Year; s++)
                {
                    var det = listaDetalle.Where(x => x.Medifecha.Year == s).ToList();
                    listaTotal.AddRange(det);
                    decimal meditotal_ = 0, maximadem_ = 0;
                    DateTime fec_ = DateTime.MinValue;
                    foreach (var d in det)
                    {
                        meditotal_ += (decimal)d.Meditotal;
                        maximadem_ += (decimal)d.MaxDemanda;
                        fec_ = d.Medifecha;
                    }
                    listaTotal.Add(new MeMedicion48DTO()
                    {
                        Ptomedinomb = "SEIN TOTAL",
                        Medifecha = fec_,
                        Meditotal = meditotal_,
                        MaxDemanda = maximadem_,
                        Anio = s.ToString()
                    });
                }
            }

            #endregion

            return listaTotal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ff1"></param>
        /// <param name="ff2"></param>
        /// <param name="list"></param>
        /// <param name="listNombPtos"></param>
        /// <param name="ptomedicodi"></param>
        /// <returns></returns>
        private List<MeMedicion48DTO> FillDatosDemandaArea(DateTime ff1, DateTime ff2, List<MeMedicion48DTO> list, List<MePtomedicionDTO> listNombPtos, int ptomedicodi)
        {
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();

            for (var f_ = ff1; f_ <= ff2; f_ = f_.AddDays(1))
            {
                decimal mtotal = 0, maxdemanda = 0;
                var obj = new MeMedicion48DTO();
                var det = list.Where(x => x.Ptomedicodi == ptomedicodi && x.Medifecha == f_).ToList();
                var nom = listNombPtos.Find(x => x.Ptomedicodi == ptomedicodi);

                obj.Ptomedinomb = (nom != null) ? nom.Ptomedielenomb : "";
                obj.Medifecha = f_;
                obj.Ptomedicodi = ptomedicodi;
                decimal maxdem = 0;


                for (int i = 1; i <= 48; i++)
                {
                    decimal val_ = 0;
                    foreach (var m in det)
                    {
                        decimal? val = (decimal?)m.GetType().GetProperty("H" + i).GetValue(m, null);
                        val_ += (val != null) ? (decimal)val : 0;

                    }
                    obj.GetType().GetProperty("H" + i).SetValue(obj, val_);
                    if (maxdem <= val_)
                    {
                        maxdem = val_;
                    }
                    mtotal += val_;
                }
                obj.MaxDemanda = maxdem;
                obj.Meditotal = mtotal;
                lista.Add(obj);
            }

            return lista;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> GetListaMePtomedicionxPtos(string ptomedicodi)
        {
            return FactorySic.GetMePtomedicionRepository().ListarPtoMedicion(ptomedicodi);
        }

        /// <summary>
        /// demanda principal
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string InformacionDemandaHtml(List<MeMedicion48DTO> data)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            var fechas = data.Select(x => x.Medifecha).Distinct().ToList();
            var listaPtos = data.Select(x => x.Ptomedicodi).Distinct().ToList();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//
            var ancho = listaPtos.Count * 85;
            strHtml.AppendFormat("<table class='pretty tabla-icono' id='tb_info' style='width: {0}px'>", ancho);

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 150px;height: 70px;'>Fecha</th>");
            foreach (var d in listaPtos)
            {
                var det = data.Find(x => x.Ptomedicodi == d);
                strHtml.AppendFormat("<th style='width: 100px;overflow-wrap: break-word; white-space: normal'>{0}</th>", (det != null ? det.Ptomedinomb : ""));
            }
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            if (data.Count > 0)
            {
                foreach (var d in fechas)
                {
                    for (int i = 1; i <= 48; i++)
                    {
                        strHtml.Append("<tr>");
                        if (i == 48) { strHtml.Append(string.Format("<td style='width: 150px;' >{0}</td>", d.ToString(ConstantesAppServicio.FormatoFecha) + " 24:00")); }
                        else { strHtml.Append(string.Format("<td style='width: 150px;' >{0}</td>", d.AddMinutes(30 * i).ToString(ConstantesAppServicio.FormatoFechaHora))); }
                        foreach (var p in listaPtos)
                        {
                            decimal? val = 0;
                            var det = data.Find(x => x.Medifecha == d && x.Ptomedicodi == p);
                            if (det != null)
                            {
                                val = (decimal?)det.GetType().GetProperty("H" + i).GetValue(det, null);
                            }
                            strHtml.Append(string.Format("<td style='width: 100px;' >{0}</td>", (val != null ? (decimal)val : 0).ToString("N", nfi)));
                        }
                        strHtml.Append("</tr>");
                    }
                }
            }
            else
            {
                strHtml.Append("<td>Sin informacion</td>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// demanda diaria
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string InformacionDemandaDiaHtml(List<MeMedicion48DTO> data)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            var fechas = data.Select(x => x.Medifecha).Distinct().ToList();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='pretty tabla-icono' id='tb_info' style='table-layout: fixed;'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 80px'>Area</th>");
            strHtml.Append("<th style='width: 70px'>Dia</th>");
            strHtml.Append("<th style='width: 50px'>Mes</th>");
            strHtml.Append("<th style='width: 40px'>A&ntilde;o</th>");
            strHtml.Append("<th style='width: 80px'>Energ&iacute;a (GWh)</th>");
            strHtml.Append("<th style='width: 110px'>Max. Demanda (MW)</th>");
            if (fechas.Count > 0)
            {
                for (int i = 1; i <= 48; i++)
                {
                    if (i == 48) { strHtml.Append("<th>" + fechas[0].AddMinutes(i * 30).AddMinutes(-1).ToString("HH:mm") + "</th>"); }
                    else { strHtml.Append("<th>" + fechas[0].AddMinutes(i * 30).ToString("HH:mm") + "</th>"); }
                }
            }
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            if (data.Count > 0)
            {
                foreach (var d in data)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td style='width: 80px'>{0}</td>", d.Ptomedinomb));
                    strHtml.Append(string.Format("<td>{0}</td>", d.Medifecha.ToString(ConstantesAppServicio.FormatoFecha)));
                    strHtml.Append(string.Format("<td>{0}</td>", d.Medifecha.ToString(ConstantesAppServicio.FormatoMesanio)));
                    strHtml.Append(string.Format("<td>{0}</td>", d.Medifecha.ToString(ConstantesAppServicio.FormatoAnio)));
                    strHtml.Append(string.Format("<td>{0}</td>", ((decimal)d.Meditotal / 2000).ToString("N", nfi)));
                    strHtml.Append(string.Format("<td>{0}</td>", ((decimal)d.MaxDemanda).ToString("N", nfi)));
                    for (int i = 1; i <= 48; i++)
                    {
                        var val = (decimal?)d.GetType().GetProperty("H" + i).GetValue(d, null);
                        strHtml.Append(string.Format("<td>{0}</td>", (val != null ? val : 0)));
                    }
                    strHtml.Append("</tr>");
                }
            }
            else
            {
                strHtml.Append("<td colspan='6'>Sin informacion</td>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string InformacionDemandaSemMesAnioHtml(List<MeMedicion48DTO> data, int tip)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='pretty tabla-icono' id='tb_info'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>Area</th>");
            if (tip == 1)
            {
                strHtml.Append("<th>Semana</th>");
                strHtml.Append("<th>Fechas</th>");
            }
            if (tip == 2)
            {
                strHtml.Append("<th>Mes</th>");
            }
            if (tip == 3)
            {
                strHtml.Append("<th>A&ntilde;o</th>");
            }
            strHtml.Append("<th>Energ&iacute;a (GWh)</th>");
            strHtml.Append("<th>Max. Demanda (MW)</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            if (data.Count > 0)
            {
                foreach (var d in data)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td>{0}</td>", d.Ptomedinomb));
                    if (tip == 1)
                    {
                        strHtml.Append(string.Format("<td>{0}</td>", d.Semana + " - " + d.Anio));
                        strHtml.Append(string.Format("<td>{0}</td>", EPDate.f_fechainiciosemana(int.Parse(d.Anio), int.Parse(d.Semana)).ToString(ConstantesAppServicio.FormatoFecha) + " - " + EPDate.f_fechafinsemana(int.Parse(d.Anio), int.Parse(d.Semana)).ToString(ConstantesAppServicio.FormatoFecha)));
                    }
                    if (tip == 2)
                    {
                        strHtml.Append(string.Format("<td>{0}</td>", d.Mes + " - " + d.Anio));
                    }
                    if (tip == 3)
                    {
                        strHtml.Append(string.Format("<td>{0}</td>", d.Anio));
                    }
                    strHtml.Append(string.Format("<td>{0}</td>", ((decimal)d.Meditotal / 2000).ToString("N", nfi)));
                    strHtml.Append(string.Format("<td>{0}</td>", ((decimal)d.MaxDemanda).ToString("N", nfi)));
                    strHtml.Append("</tr>");
                }
            }
            else
            {
                strHtml.Append("<td colspan='" + (tip == 1 ? 5 : 4) + "'>Sin informacion</td>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="rutaFile"></param>
        /// <param name="fec1"></param>
        /// <param name="fec2"></param>
        /// <param name="anio"></param>
        /// <param name="sem1"></param>
        /// <param name="sem2"></param>
        /// <param name="mes1"></param>
        /// <param name="mes2"></param>
        /// <param name="anio1"></param>
        /// <param name="anio2"></param>
        /// <param name="tip"></param>
        public void GenerarArchivoExcelDemandaArea(List<MeMedicion48DTO> lista, string rutaFile, string fec1, string fec2, string anio, string sem1, string sem2, string mes1, string mes2, string anio1, string anio2, int tip, int posi)
        {
            FileInfo newFile = new FileInfo(rutaFile);

            DateTime f_1 = DateTime.MinValue, f_2 = DateTime.MinValue;

            #region seteo de fecha

            switch (tip)
            {
                case 1:
                case 2:
                    f_1 = DateTime.ParseExact(fec1, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    f_2 = DateTime.ParseExact(fec2, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture); break;
                case 3:
                    f_1 = EPDate.f_fechainiciosemana(int.Parse(anio), int.Parse(sem1));
                    f_2 = EPDate.f_fechafinsemana(int.Parse(anio), int.Parse(sem2)); break;
                case 4:
                    f_1 = DateTime.ParseExact("01/" + mes1, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    f_2 = DateTime.ParseExact("01/" + mes2, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    f_2 = f_2.AddMonths(1).AddDays(-1); break;
                case 5:
                    f_1 = DateTime.ParseExact("01/01/" + anio1, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    f_2 = DateTime.ParseExact("01/01/" + anio2, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    f_2 = f_2.AddYears(1).AddDays(-1); break;
            }

            #endregion

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                switch (tip)
                {
                    case 1: this.ExcelCabGeneralDemArea(ref ws, xlPackage, "Principal", "Demanda por Area Principal del " + fec1 + " al " + fec2, 4); this.ExcelDetGeneralDemAreaPrin(ws, lista, posi); break;
                    case 2: this.ExcelCabGeneralDemArea(ref ws, xlPackage, "Diaria", "Demanda por Area Diaria del " + fec1 + " al " + fec2, 4); this.ExcelDetGeneralDemAreaDia(ws, lista, posi); break;
                    case 3: this.ExcelCabGeneralDemArea(ref ws, xlPackage, "Semanal", "Demanda por Area Semanal del " + sem1 + "/" + anio + " al " + sem2 + "/" + anio, 4); this.ExcelDetGeneralDemArea(ws, lista, posi, tip, sem1 + anio, sem2 + anio); break;
                    case 4: this.ExcelCabGeneralDemArea(ref ws, xlPackage, "Mensual", "Demanda por Area Mensual del " + mes1 + " al " + mes2, 4); this.ExcelDetGeneralDemArea(ws, lista, posi, tip, sem1, sem2); break;
                    case 5: this.ExcelCabGeneralDemArea(ref ws, xlPackage, "Anual", "Demanda por Area Anual del " + anio1 + " al " + anio2, 4); this.ExcelDetGeneralDemArea(ws, lista, posi, tip, sem1, sem2); break;
                }
                ws.View.ShowGridLines = false;



                AddImage(ws, 0);
                xlPackage.Save();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        private void ExcelDetGeneralDemAreaPrin(ExcelWorksheet ws, List<MeMedicion48DTO> data, int posi)
        {
            int colIni = 2, colum = 2, sizeFont = 8, rowIni = 5, rowFin = 6;
            var fechas = data.Select(x => x.Medifecha).Distinct().ToList();
            var listaPtos = data.Select(x => x.Ptomedicodi).Distinct().ToList();

            if (posi == 1)
            {
                #region cabecera

                ws.Cells[rowIni, colum++].Value = "Fecha Hora";
                foreach (var d in listaPtos)
                {
                    var det = data.Find(x => x.Ptomedicodi == d);
                    ws.Cells[rowIni, colum++].Value = (det != null ? det.Ptomedinomb : "");
                }

                formatoCabecera(ws, rowIni, colIni, rowIni, colum - 1, sizeFont);

                #endregion

                #region cuerpo

                foreach (var d in fechas)
                {
                    for (int i = 1; i <= 48; i++)
                    {
                        colum = 2;
                        if (i == 48) { ws.Cells[rowFin, colum++].Value = d.ToString(ConstantesAppServicio.FormatoFecha) + " 24:00"; }
                        else { ws.Cells[rowFin, colum++].Value = d.AddMinutes(30 * i).ToString(ConstantesAppServicio.FormatoFechaHora); }
                        foreach (var p in listaPtos)
                        {
                            decimal? val = 0;
                            var det = data.Find(x => x.Medifecha == d && x.Ptomedicodi == p);
                            if (det != null)
                            {
                                val = (decimal?)det.GetType().GetProperty("H" + i).GetValue(det, null);
                            }
                            ws.Cells[rowFin, colum].Value = val != null ? (decimal)val : 0;
                            ws.Cells[rowFin, colum++].Style.Numberformat.Format = "0.000";
                        }
                        rowFin++;
                    }
                }

                #endregion
            }
            else
            {
                #region cabecera

                ws.Cells[rowIni, colum++].Value = "Puntos";
                ws.Cells[rowIni, colum++].Value = "Dia";
                ws.Cells[rowIni, colum++].Value = "Mes";
                ws.Cells[rowIni, colum++].Value = "Año";
                if (fechas.Count > 0)
                {
                    for (int i = 1; i <= 48; i++)
                    {
                        if (i == 48) { ws.Cells[rowIni, colum++].Value = "24:00"; }
                        else { ws.Cells[rowIni, colum++].Value = fechas[0].AddMinutes(30 * i).ToString(ConstantesAppServicio.FormatoOnlyHora); }
                    }
                }

                formatoCabecera(ws, rowIni, colIni, rowIni, colum - 1, sizeFont);

                #endregion

                #region cuerpo

                foreach (var d in fechas)
                {
                    foreach (var p in listaPtos)
                    {
                        colum = 2;
                        var det = data.Find(x => x.Medifecha == d && x.Ptomedicodi == p);
                        if (det != null)
                        {
                            ws.Cells[rowFin, colum++].Value = det.Ptomedinomb;
                            ws.Cells[rowFin, colum++].Value = d.ToString(ConstantesAppServicio.FormatoFecha);
                            ws.Cells[rowFin, colum++].Value = d.ToString(ConstantesAppServicio.FormatoMesanio);
                            ws.Cells[rowFin, colum++].Value = d.Year;

                            decimal? val = 0;
                            for (int i = 1; i <= 48; i++)
                            {
                                val = (decimal?)det.GetType().GetProperty("H" + i).GetValue(det, null);
                                ws.Cells[rowFin, colum].Value = val != null ? (decimal)val : 0;
                                ws.Cells[rowFin, colum++].Style.Numberformat.Format = "0.000";
                            }
                        }
                        rowFin++;
                    }
                }

                #endregion
            }

            #region estilo rpt

            using (var range = ws.Cells[rowIni, colIni, rowFin - 1, colum - 1])
            {
                range.Style.Font.Size = sizeFont;
            }

            ws.Column(2).Width = 14;
            ws.Column(3).Width = 12;
            ws.Column(4).Width = 12;
            AddAutoWidthColumn(ws, colIni + 3, colum - 1);
            borderCeldas(ws, rowIni, colIni, rowFin - 1, colum - 1);

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="data"></param>
        private void ExcelDetGeneralDemAreaDia(ExcelWorksheet ws, List<MeMedicion48DTO> data, int posi)
        {
            int colIni = 2, colum = 2, sizeFont = 8, rowIni = 5, rowFin = 6;
            var listaPtos = data.Select(x => x.Ptomedicodi).Distinct().ToList();
            var fechas = data.Select(x => x.Medifecha).Distinct().ToList();

            if (posi == 1)
            {
                #region cabecera

                ws.Cells[rowIni, colum++].Value = "Area";
                ws.Cells[rowIni, colum++].Value = "Dia";
                ws.Cells[rowIni, colum++].Value = "Mes";
                ws.Cells[rowIni, colum++].Value = "Año";
                ws.Cells[rowIni, colum++].Value = "Energía (GWh)";
                ws.Cells[rowIni, colum++].Value = "Max. Demanda (MW)";

                if (fechas.Count > 0)
                {
                    for (int i = 1; i <= 48; i++)
                    {
                        if (i == 48) { ws.Cells[rowIni, colum++].Value = fechas[0].AddMinutes(i * 30).AddMinutes(-1).ToString("HH:mm"); }
                        else { ws.Cells[rowIni, colum++].Value = fechas[0].AddMinutes(i * 30).ToString("HH:mm"); }
                    }
                }

                formatoCabecera(ws, rowIni, colIni, rowIni, colum - 1, sizeFont);

                #endregion

                #region cuerpo

                foreach (var d in data)
                {
                    colum = 2;

                    ws.Cells[rowFin, colum++].Value = d.Ptomedinomb;
                    ws.Cells[rowFin, colum++].Value = d.Medifecha.ToString(ConstantesAppServicio.FormatoFecha);
                    ws.Cells[rowFin, colum++].Value = d.Medifecha.ToString(ConstantesAppServicio.FormatoMesanio);
                    ws.Cells[rowFin, colum++].Value = d.Medifecha.Year;
                    ws.Cells[rowFin, colum].Value = d.Meditotal / 2000;
                    ws.Cells[rowFin, colum++].Style.Numberformat.Format = "0.000";
                    ws.Cells[rowFin, colum].Value = d.MaxDemanda;
                    ws.Cells[rowFin, colum++].Style.Numberformat.Format = "0.000";
                    for (int i = 1; i <= 48; i++)
                    {
                        var val = (decimal?)d.GetType().GetProperty("H" + i).GetValue(d, null);
                        ws.Cells[rowFin, colum].Value = (val != null ? val : 0);
                        ws.Cells[rowFin, colum++].Style.Numberformat.Format = "0.000";
                    }

                    rowFin++;
                }

                #endregion
            }
            else
            {
                #region cabecera

                ws.Cells[rowIni, colum++].Value = "Fecha";
                ws.Cells[rowIni, colum++].Value = "Hora";

                for (int z = 0; z < 2; z++)
                {
                    foreach (var d in listaPtos)
                    {
                        var det = data.Find(x => x.Ptomedicodi == d);
                        ws.Cells[rowIni, colum++].Value = (det != null ? det.Ptomedinomb + (z == 0 ? " (MW)" : " (GWh)") : "");
                    }
                }

                formatoCabecera(ws, rowIni, colIni, rowIni, colum - 1, sizeFont);

                #endregion

                #region cuerpo

                foreach (var d in fechas)
                {
                    for (int i = 1; i <= 48; i++)
                    {
                        colum = 2;
                        ws.Cells[rowFin, colum++].Value = d.ToString(ConstantesAppServicio.FormatoFecha);
                        if (i == 48) { ws.Cells[rowFin, colum++].Value = d.AddMinutes(i * 30).AddMinutes(-1).ToString(ConstantesAppServicio.FormatoOnlyHora); }
                        else { ws.Cells[rowFin, colum++].Value = d.AddMinutes(i * 30).ToString(ConstantesAppServicio.FormatoOnlyHora); }
                        for (int z = 0; z < 2; z++)
                        {
                            foreach (var p in listaPtos)
                            {
                                var det = data.Find(x => x.Medifecha == d && x.Ptomedicodi == p);
                                if (det != null)
                                {
                                    decimal? val = 0;

                                    val = (decimal?)det.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(det, null);
                                    ws.Cells[rowFin, colum].Value = val != null ? (decimal)(z == 0 ? val : val / 2000) : 0;
                                    ws.Cells[rowFin, colum++].Style.Numberformat.Format = "0.000";
                                }
                            }
                        }
                        rowFin++;
                    }
                }

                #endregion
            }

            #region estilo rpt

            using (var range = ws.Cells[rowIni, colIni, rowFin - 1, colum - 1])
            {
                range.Style.Font.Size = sizeFont;
            }

            ws.Column(2).Width = 12;
            ws.Column(3).Width = 10;
            ws.Column(4).Width = 10;
            AddAutoWidthColumn(ws, colIni + 3, colum - 1);
            borderCeldas(ws, rowIni, colIni, rowFin - 1, colum - 1);

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lista"></param>
        /// <param name="tip"></param>
        private void ExcelDetGeneralDemArea(ExcelWorksheet ws, List<MeMedicion48DTO> data, int posi, int tip, string sem01, string sem02)
        {
            int colIni = 2, colum = 2, sizeFont = 8, rowIni = 5, rowFin = 6, sem1 = 0, sem2 = 0;
            DateTime f_ = DateTime.MinValue;
            var listaPtos = data.Select(x => x.Ptomedicodi).Distinct().ToList();
            var fechas = data.Select(x => x.Medifecha).Distinct().ToList();

            if (posi == 1)
            {
                #region cabecera

                ws.Cells[rowIni, colum++].Value = "Area";
                if (tip == 3)
                {
                    ws.Cells[rowIni, colum++].Value = "Semana";
                    ws.Cells[rowIni, colum++].Value = "Fechas";
                }
                if (tip == 4)
                {
                    ws.Cells[rowIni, colum++].Value = "Mes";
                }
                if (tip == 5)
                {
                    ws.Cells[rowIni, colum++].Value = "Año";
                }
                ws.Cells[rowIni, colum++].Value = "Energía (GWh)";
                ws.Cells[rowIni, colum++].Value = "Max. Demanda (MW)";

                formatoCabecera(ws, rowIni, colIni, rowIni, colum - 1, sizeFont);

                #endregion

                #region cuerpo

                foreach (var d in data)
                {
                    colum = 2;

                    ws.Cells[rowFin, colum++].Value = d.Ptomedinomb;
                    if (tip == 3)
                    {
                        ws.Cells[rowFin, colum++].Value = d.Semana + " - " + d.Anio;
                        ws.Cells[rowFin, colum++].Value = EPDate.f_fechainiciosemana(int.Parse(d.Anio), int.Parse(d.Semana)).ToString(ConstantesAppServicio.FormatoFecha) + " - " + EPDate.f_fechafinsemana(int.Parse(d.Anio), int.Parse(d.Semana)).ToString(ConstantesAppServicio.FormatoFecha);
                    }
                    if (tip == 4)
                    {
                        ws.Cells[rowFin, colum++].Value = d.Mes + " - " + d.Anio;
                    }
                    if (tip == 5)
                    {
                        ws.Cells[rowFin, colum++].Value = d.Anio;
                    }
                    ws.Cells[rowFin, colum].Value = d.Meditotal / 2000;
                    ws.Cells[rowFin, colum++].Style.Numberformat.Format = "0.000";
                    ws.Cells[rowFin, colum].Value = d.MaxDemanda;
                    ws.Cells[rowFin, colum++].Style.Numberformat.Format = "0.000";

                    rowFin++;
                }

                #endregion
            }
            else
            {
                #region cabecera

                if (tip == 3)
                {
                    ws.Cells[rowIni, colum++].Value = "Semana";
                    ws.Cells[rowIni, colum++].Value = "Fecha";
                }
                if (tip == 4)
                {
                    ws.Cells[rowIni, colum++].Value = "Mes / Año";
                }
                if (tip == 5)
                {
                    ws.Cells[rowIni, colum++].Value = "Año";
                }

                for (int z = 0; z < 2; z++)
                {
                    foreach (var d in listaPtos)
                    {
                        var det = data.Find(x => x.Ptomedicodi == d);
                        ws.Cells[rowIni, colum++].Value = (det != null ? det.Ptomedinomb + (z == 0 ? " (MW)" : " (GWh)") : "");
                    }
                }

                formatoCabecera(ws, rowIni, colIni, rowIni, colum - 1, sizeFont);

                #endregion

                #region cuerpo

                switch (tip)
                {
                    case 3: //sem1 = int.Parse(sem01); sem2 = int.Parse(sem02); 
                        f_ = fechas.First(); break;
                    case 4: break;
                    case 5: sem1 = EPDate.f_numerosemana(fechas.Min()); sem2 = EPDate.f_numerosemana(fechas.Max()); break;
                }
                if (tip == 3)
                {
                    int semanaIni = Int32.Parse(sem01.Substring(0, sem01.Length - 4));
                    int semanaFin = Int32.Parse(sem02.Substring(0, sem02.Length - 4));
                    int anio = Int32.Parse(sem01.Substring(sem01.Length - 4, 4));
                    for (int s = semanaIni; s <= semanaFin; s++)
                    {
                        colum = 2;

                        DateTime ff1 = EPDate.f_fechainiciosemana(anio, s);
                        DateTime ff2 = EPDate.f_fechafinsemana(anio, s);

                        ws.Cells[rowFin, colum++].Value = s + " - " + anio;
                        ws.Cells[rowFin, colum++].Value = ff1.ToString(ConstantesAppServicio.FormatoFecha) + " - " + ff2.ToString(ConstantesAppServicio.FormatoFecha);
                        for (int z = 0; z < 2; z++)
                        {
                            foreach (var p in listaPtos)
                            {
                                var det = data.Find(x => x.Semana == s.ToString() && x.Ptomedicodi == p);
                                if (det != null)
                                {
                                    ws.Cells[rowFin, colum].Value = (z == 0 ? det.Meditotal : det.Meditotal / 2000);
                                    ws.Cells[rowFin, colum++].Style.Numberformat.Format = "0.000";
                                }
                            }
                        }
                        rowFin++;
                    }
                }
                if (tip == 4)
                {
                    var meses = data.Select(x => x.Medifecha.ToString(ConstantesAppServicio.FormatoMesanio)).Distinct().ToList();

                    foreach (var d in meses)
                    {
                        colum = 2;

                        ws.Cells[rowFin, colum++].Value = d;
                        for (int z = 0; z < 2; z++)
                        {
                            foreach (var p in listaPtos)
                            {
                                var det = data.Find(x => x.Mes == int.Parse(d.Split('/')[0]).ToString() && x.Anio == d.Split('/')[1] && x.Ptomedicodi == p);
                                if (det != null)
                                {
                                    ws.Cells[rowFin, colum].Value = (z == 0 ? det.Meditotal : det.Meditotal / 2000);
                                    ws.Cells[rowFin, colum++].Style.Numberformat.Format = "0.000";
                                }
                            }
                        }
                        rowFin++;
                    }
                }
                if (tip == 5)
                {
                    var anios = data.Select(x => x.Medifecha.ToString("yyyy")).Distinct().ToList();

                    foreach (var d in anios)
                    {
                        colum = 2;

                        ws.Cells[rowFin, colum++].Value = int.Parse(d);
                        for (int z = 0; z < 2; z++)
                        {
                            foreach (var p in listaPtos)
                            {
                                var det = data.Find(x => x.Anio == d && x.Ptomedicodi == p);
                                if (det != null)
                                {
                                    ws.Cells[rowFin, colum].Value = (z == 0 ? det.Meditotal : det.Meditotal / 2000);
                                    ws.Cells[rowFin, colum++].Style.Numberformat.Format = "0.000";
                                }
                            }
                        }
                        rowFin++;
                    }
                }

                #endregion
            }

            #region estilo rpt

            using (var range = ws.Cells[rowIni, colIni, rowFin - 1, colum - 1])
            {
                range.Style.Font.Size = sizeFont;
            }

            if ((tip == 3 || tip == 4 || tip == 5) && posi == 1)
            {
                posi = -1;
            }

            borderCeldas(ws, rowIni, colIni, rowFin - 1, colum - (posi == 1 ? 2 : 1));

            colum = 2;
            ws.Column(colum++).Width = 12;
            if (tip == 3)
            {
                ws.Column(colum++).Width = 18;
            }
            ws.Column(colum++).Width = 16;
            ws.Column(colum++).Width = 16;
            ws.Column(colum++).Width = 16;
            ws.Column(colum++).Width = 16;
            #endregion
        }

        /// <summary>
        /// Configura la imagen que ira en el excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="tipo"></param>
        public void AddImage(ExcelWorksheet ws, int tipo, int ancho = 180, int alto = 75)
        {
            HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
            ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
            picture.From.Column = 0;
            picture.From.Row = 0;
            picture.SetSize(ancho, alto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="titulo"></param>
        /// <param name="nroColumn"></param>
        private void ExcelCabGeneralDemArea(ref ExcelWorksheet ws, ExcelPackage xlPackage, string nameWS, string titulo, int nroColumn)
        {
            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];
            ws.View.ShowGridLines = false;

            ws.Cells[3, nroColumn].Value = titulo;

            var font = ws.Cells[3, nroColumn].Style.Font;
            font.Size = 14;
            font.Bold = true;
            font.Name = "Calibri";
        }

        #endregion

        #region reporte costo marginal corto plazo

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="ptomedicodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetRptCmgCortoPlazo(string lectcodi, int tipoinfocodi, int ptomedicodi, DateTime fecha1, DateTime fecha2)
        {
            return FactorySic.GetMeMedicion48Repository().RptCmgCortoPlazo(lectcodi, tipoinfocodi, ptomedicodi, fecha1, fecha2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public string RptCmgCortoPlazoHtml(List<MeMedicion48DTO> data)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            var fechas = data.Select(x => x.Medifecha).Distinct().ToList();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='pretty tabla-icono' id='tb_info'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>Fecha Hora</th>");
            strHtml.Append("<th>Barra</th>");
            strHtml.Append("<th>Cmg Corto Plazo (S/. /MWh)</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            if (data.Count > 0)
            {
                foreach (var f in fechas)
                {
                    var det = data.Find(x => x.Medifecha == f);
                    if (det != null)
                    {
                        if (det.Ptomedinomb == "CmgCP") { det.Ptomedinomb = "BARRA SANTA ROSA 220"; }
                        for (int i = 1; i <= 48; i++)
                        {
                            strHtml.Append("<tr>");
                            strHtml.Append(string.Format("<td>{0}</td>", f.AddMinutes(i * 30).ToString(ConstantesAppServicio.FormatoFechaHora)));
                            strHtml.Append("<td>" + det.Ptomedinomb + "</td>");
                            var val = (decimal?)det.GetType().GetProperty("H" + i).GetValue(det, null);
                            strHtml.Append(string.Format("<td>{0}</td>", ((decimal)(val != null ? val : 0)).ToString("N", nfi)));
                            strHtml.Append("</tr>");
                        }
                    }
                }
            }
            else
            {
                strHtml.Append("<td colspan='3'>Sin informacion</td>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="fecInicio"></param>
        /// <param name="fecFin"></param>
        /// <param name="rutaFile"></param>
        public void GenerarArchivoExcelCmgCortoPlazo(List<MeMedicion48DTO> lista, DateTime fecInicio, DateTime fecFin, string rutaFile)
        {
            FileInfo newFile = new FileInfo(rutaFile);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;

                this.ExcelCabGeneral(ref ws, xlPackage, "Cmg_SantaRosa220", "", 0);
                this.ExcelDetGeneralCmgCortoPlazo(ws, lista, fecInicio, fecFin);
                ws.View.ShowGridLines = false;

                xlPackage.Save();
            }
        }

        private void ExcelDetGeneralCmgCortoPlazo(ExcelWorksheet ws, List<MeMedicion48DTO> lista, DateTime fecInicio, DateTime fecFin)
        {
            var fechas = lista.Select(x => x.Medifecha).Distinct().ToList();

            ws.Cells[5, 2].Value = "EVOLUCIÓN MEDIO HORARIO DE LOS COSTOS MARGINALES DE CORTO PLAZO DEL SEIN (S/./MWh)";
            ws.Cells[5, 2].Style.Font.Bold = true;
            ws.Cells[7, 2].Value = "Desde " + fecInicio.ToString(ConstantesAppServicio.FormatoFecha) + " al " + fecFin.ToString(ConstantesAppServicio.FormatoFecha);
            ws.Cells[7, 2].Style.Font.Bold = true;

            int row = 9, columIni = 2, colum = 2;
            #region cabecera
            ws.Cells[row, colum++].Value = "Fecha Hora";
            ws.Cells[row, colum++].Value = "Barra";
            ws.Cells[row, colum++].Value = "Cmg Corto Plazo (S/. /MWh)";

            ExcelRange rg = ws.Cells[row, columIni, row, colum - 1];
            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
            rg.Style.Font.Color.SetColor(Color.White);
            rg.Style.Font.Size = 10;
            rg.Style.Font.Bold = true;
            rg.Style.WrapText = true;
            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            #endregion

            #region detalle
            row++;
            foreach (var f in fechas)
            {
                var det = lista.Find(x => x.Medifecha == f);
                if (det != null)
                {
                    if (det.Ptomedinomb == "CmgCP") { det.Ptomedinomb = "BARRA SANTA ROSA 220"; }
                    for (int i = 1; i <= 48; i++)
                    {
                        colum = 2;
                        ws.Cells[row, colum].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[row, colum++].Value = f.AddMinutes(i * 30).ToString(ConstantesAppServicio.FormatoFechaHora);
                        ws.Cells[row, colum].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[row, colum++].Value = det.Ptomedinomb;

                        var val = (decimal?)det.GetType().GetProperty("H" + i).GetValue(det, null);
                        ws.Cells[row, colum].Value = (val != null ? val : 0);
                        ws.Cells[row, colum++].Style.Numberformat.Format = "#,##0.000";

                        row++;
                    }
                }
            }

            int sizeFont = 8;
            using (var range = ws.Cells[9, columIni, row - 1, colum - 1])
            {
                range.Style.Font.Size = sizeFont;
            }
            ws.Column(2).Width = 15;
            ws.Column(3).Width = 20;
            ws.Column(4).Width = 12;
            this.borderCeldas(ws, 9, columIni, row - 1, colum - 1);
            #endregion

            HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
            ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
            picture.From.Column = 1;
            picture.From.Row = 1;
            picture.To.Column = 2;
            picture.To.Row = 2;
            picture.SetSize(120, 55);
        }

        #endregion

        #region Reporte medidores de generacion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="umbrales"></param>
        /// <param name="listCuadros"></param>
        /// <returns></returns>
        public string ReporteMedidoresGeneracionHtml(MedicionReporteDTO umbrales, List<MedicionReporteDTO> listCuadros)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            bool tieneMD = listCuadros.Where(x => !x.IndicadorTotalGeneral).Count() > 0;

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            if (tieneMD)
            {
                strHtml.Append("<div class='leyenda-reporte' style='float:right; width:auto'>");
                strHtml.Append("<strong>Máxima Demanda:</strong> el " + umbrales.FechaMaximaDemanda.ToString(ConstantesAppServicio.FormatoFecha) + " a las " + umbrales.MaximaDemandaHora.ToString(ConstantesAppServicio.FormatoOnlyHora) + " h" + "<br />");
                strHtml.Append("<strong>Mínima Demanda:&nbsp;</strong> el " + umbrales.FechaMinimaDemanda.ToString(ConstantesAppServicio.FormatoFecha) + " a las " + umbrales.MinimaDemandaHora.ToString(ConstantesAppServicio.FormatoOnlyHora) + " h");
                strHtml.Append("</div>");
            }

            strHtml.Append("<div style='clear:both'></div>");

            strHtml.Append("<table class='tabla-formulario tabla-adicional' id='tbRecurso'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>N°</th>");
            strHtml.Append("<th>Empresa</th>");
            strHtml.Append("<th>Central</th>");
            strHtml.Append("<th>Unidad</th>");
            strHtml.Append("<th>Energía (MWh)</th>");
            strHtml.Append("<th>Máxima Demanda (MW)</th>");
            strHtml.Append("<th>Mínima Demanda (MW)</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var item in listCuadros)
            {
                if (!item.IndicadorTotal && !item.IndicadorTotalGeneral)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append("<td>" + item.NroItem + "</td>");
                    strHtml.Append("<td>" + item.Emprnomb + "</td>");
                    strHtml.Append("<td>" + item.Central + "</td>");
                    strHtml.Append("<td>" + item.Unidad + "</td>");
                    //strHtml.Append("<td>" + item.Fenergnomb + "</td>");
                    strHtml.Append("<td style='text-align:right'>" + item.Total.ToString("N", nfi) + "</td>");
                    strHtml.Append("<td style='text-align:right'>" + item.MaximaDemanda.ToString("N", nfi) + "</td>");
                    strHtml.Append("<td style='text-align:right'>" + item.MinimaDemanda.ToString("N", nfi) + "</td>");
                    strHtml.Append("</tr>");
                }
                else
                {
                    if (item.IndicadorTotal)
                    {
                        strHtml.Append("<tr class='table-subtotal'>");
                        strHtml.Append("<td colspan='4' style='text-align:right'>TOTAL " + item.Emprnomb + "</td>");
                        strHtml.Append("<td style='text-align:right' title='Energía (MWh)'>" + item.Total.ToString("N", nfi) + "</td>");
                        strHtml.Append("<td style='text-align:right' title='Máxima Demanda (MW)'>" + item.MaximaDemanda.ToString("N", nfi) + "</td>");
                        strHtml.Append("<td style='text-align:right' title='Mínima Demanda (MW)'>" + item.MinimaDemanda.ToString("N", nfi) + "</td>");
                        strHtml.Append("</tr>");
                    }
                    if (item.IndicadorTotalGeneral)
                    {
                        strHtml.Append("<tr class='table-total'>");
                        strHtml.Append("<td colspan='4' style='text-align:right'>TOTAL GENERAL</td>");
                        strHtml.Append("<td style='text-align:right' title='Energía (MWh)'>" + item.Total.ToString("N", nfi) + "</td>");
                        strHtml.Append("<td style='text-align:right' title='Máxima Demanda (MW)'>" + item.MaximaDemanda.ToString("N", nfi) + "</td>");
                        strHtml.Append("<td style='text-align:right' title='Mínima Demanda (MW)'>" + item.MinimaDemanda.ToString("N", nfi) + "</td>");
                        strHtml.Append("</tr>");
                    }
                }
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="umbrales"></param>
        /// <param name="listCuadros"></param>
        /// <param name="rutaFile"></param>
        public void GenerarArchivoExcelMedidoresGeneracion(MedicionReporteDTO umbrales, List<MedicionReporteDTO> listCuadros, DateTime fecInicio, DateTime fecFin, string rutaFile)
        {
            FileInfo newFile = new FileInfo(rutaFile);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;

                this.ExcelCabGeneral(ref ws, xlPackage, "Medidores de Generacion", "", 0);
                this.ExcelDetGeneralMedidoresGeneracion(ws, umbrales, listCuadros, fecInicio, fecFin);
                ws.View.ShowGridLines = false;

                xlPackage.Save();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="umbrales"></param>
        /// <param name="listCuadros"></param>
        private void ExcelDetGeneralMedidoresGeneracion(ExcelWorksheet ws, MedicionReporteDTO umbrales, List<MedicionReporteDTO> list, DateTime fecInicio, DateTime fecFin)
        {
            bool tieneMD = list.Where(x => !x.IndicadorTotalGeneral).Count() > 0;
            ws.Cells[5, 2].Value = "REPORTE MEDIDORES DE GENERACION DEL " + fecInicio.ToString(ConstantesAppServicio.FormatoFecha) + " al " + fecFin.ToString(ConstantesAppServicio.FormatoFecha);

            if (tieneMD)
            {
                ws.Cells[6, 8].Value = "Máxima Demanda: " + umbrales.FechaMaximaDemanda.ToString(ConstantesAppServicio.FormatoFecha) + " a las " + umbrales.MaximaDemandaHora.ToString(ConstantesAppServicio.FormatoOnlyHora) + " h";
                ws.Cells[6, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }

            ExcelRange rg = ws.Cells[5, 2, 6, 9];
            rg.Style.Font.Size = 13;
            rg.Style.Font.Bold = true;

            int row = 7;

            ws.Cells[row, 2].Value = "N°";
            ws.Cells[row, 3].Value = "Empresa";
            ws.Cells[row, 4].Value = "Central";
            ws.Cells[row, 5].Value = "Unidad";
            ws.Cells[row, 6].Value = "Energía (MWh)";
            ws.Cells[row, 7].Value = "Máxima Demanda(MW)";
            ws.Cells[row, 8].Value = "Mínima Demanda(MW)";

            rg = ws.Cells[row, 2, row, 8];
            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
            rg.Style.Font.Color.SetColor(Color.White);
            rg.Style.Font.Size = 10;
            rg.Style.Font.Bold = true;

            row++;

            foreach (MedicionReporteDTO item in list)
            {
                if (!item.IndicadorTotal && !item.IndicadorTotalGeneral)
                {
                    ws.Cells[row, 2].Value = item.NroItem;
                    ws.Cells[row, 3].Value = item.Emprnomb;
                    ws.Cells[row, 4].Value = item.Central;
                    ws.Cells[row, 5].Value = item.Unidad;
                    ws.Cells[row, 6].Value = item.Total;
                    ws.Cells[row, 7].Value = item.MaximaDemanda;
                    ws.Cells[row, 8].Value = item.MinimaDemanda;

                    rg = ws.Cells[row, 2, row, 8];
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));
                }
                else
                {
                    if (item.IndicadorTotal)
                    {
                        ws.Cells[row, 2].Value = "TOTAL: " + item.Emprnomb.Trim();
                        ws.Cells[row, 2, row, 5].Merge = true;
                        ws.Cells[row, 6].Value = item.Total;
                        ws.Cells[row, 7].Value = item.MaximaDemanda;
                        ws.Cells[row, 8].Value = item.MinimaDemanda;

                        rg = ws.Cells[row, 2, row, 8];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#E8F6FF"));
                        rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#245C86"));
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    }
                    if (item.IndicadorTotalGeneral)
                    {
                        ws.Cells[row, 2].Value = "TOTAL GENERAL";
                        ws.Cells[row, 2, row, 5].Merge = true;
                        ws.Cells[row, 6].Value = item.Total;
                        ws.Cells[row, 7].Value = item.MaximaDemanda;
                        ws.Cells[row, 8].Value = item.MinimaDemanda;

                        rg = ws.Cells[row, 2, row, 8];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2E8DCD"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                }

                row++;
            }


            rg = ws.Cells[8, 6, row, 8];
            rg.Style.Numberformat.Format = "#,##0.000";


            rg = ws.Cells[7, 2, row - 1, 8];
            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

            ws.Column(2).Width = 10;

            rg = ws.Cells[7, 3, row, 8];
            rg.AutoFitColumns();

            ws.Column(6).Width = 17;

            HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
            ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
            picture.From.Column = 1;
            picture.From.Row = 1;
            picture.To.Column = 2;
            picture.To.Row = 2;
            picture.SetSize(120, 55);
        }

        #endregion

        #region WebAPI Mediciones

        /// <summary>
        /// Listar data del despacho
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetListaMedicion48WebApiMediciones(int lectcodi, DateTime fechaIni, DateTime fechaFin)
        {
            //MW, MVar aplicar filtro  ConstantesAppServicio.TipoinfocodiMW 
            var listaAllMe48 = GetObtenerHistoricoMedicion48(lectcodi, fechaIni, fechaFin);
            listaAllMe48 = listaAllMe48.Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMW || x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMVAR).ToList();

            //setear valor defecto a los estados
            foreach (var reg48bd in listaAllMe48)
            {
                for (int h = 1; h <= 48; h++)
                {
                    int valorDefault = 0; //celda en blanco
                    int? valorT = (int?)reg48bd.GetType().GetProperty(ConstantesAppServicio.CaracterT + h).GetValue(reg48bd, null);
                    if (valorT.GetValueOrDefault(0) <= 0)
                        reg48bd.GetType().GetProperty(ConstantesAppServicio.CaracterT + h).SetValue(reg48bd, valorDefault);
                }
            }

            // Lista de puntos (Origen de lectura 2    Mediciones Despacho 1/2 hora)
            List<MePtomedicionDTO> listaPto = FactorySic.GetMePtomedicionRepository().GetByCriteria(ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.OriglectcodiDespachomediahora.ToString(), ConstantesAppServicio.ParametroDefecto);
            listaPto = listaPto.Where(x => x.Ptomediestado != ConstantesAppServicio.Anulado && x.Ptomedicodi > 0).ToList();

            //Obtener los equipos
            listaPto = listaPto.Where(x => x.Equicodi != null).ToList();
            var listaEquiposIds = listaPto.Select(x => x.Equicodi.ToString()).Distinct().ToList();
            var listaEquipos = FactorySic.GetEqEquipoRepository().ListarEquiposXIds(string.Join(",", listaEquiposIds.ToArray()));

            //obtener valores de los puntos
            foreach (var item in listaAllMe48)
            {
                var pto = listaPto.Find(x => x.Ptomedicodi == item.Ptomedicodi);
                if (pto != null)
                {
                    item.Emprcodi = pto.Emprcodi.Value;
                    item.Emprnomb = pto.Emprnomb;
                    item.Tipoinfoabrev = item.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMW ? "MW" : "MVar";

                    var eqpo = listaEquipos.Find(x => x.Equicodi == pto.Equicodi);
                    if (eqpo != null)
                    {
                        item.Equinomb = eqpo.Equinomb;
                        item.Equicodi = eqpo.Equicodi;
                        item.Equitension = eqpo.Equitension;
                        item.Areacodi = eqpo.Areacodi.Value;
                        item.Areanomb = eqpo.Areanomb;
                    }
                }
            }

            return listaAllMe48.OrderBy(x => x.Emprnomb).ThenBy(x => x.Areanomb).ThenBy(x => x.Equinomb).ThenBy(x => x.Tipoinfocodi).ToList();
        }

        /// <summary>
        /// Listar puntos de medición del Despacho
        /// </summary>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListarPtoMedicionDespacho()
        {
            CDespachoInput regInput = new CDespachoInput()
            {
                FechaIni = DateTime.Today,
                FechaFin = DateTime.Today,
                Lectcodi = Convert.ToInt32(ConstantesAppServicio.LectcodiEjecutadoHisto),
                Empresas = "-1",
                ListaAllMe48 = new List<MeMedicion48DTO>(),
            };

            var regCDespacho = new CDespachoGlobal();
            CargarInsumoPlantillaGeneracionCDispatch(regInput, ref regCDespacho);
            UtilCdispatch.AsignarVariablesPlantillaCDispatch(regInput, ref regCDespacho);

            return regCDespacho.ListaAllPtoPlantilla;
        }

        #endregion

        #region CDispatch

        #region CDispatch - Insumos

        private List<MePtomedicionDTO> ListarAllPtoDespachoConTTIE(DateTime fechaPeriodo)
        {
            // Lista de puntos
            List<MePtomedicionDTO> listaPto = FactorySic.GetMePtomedicionRepository().GetByCriteria(ConstantesAppServicio.ParametroDefecto,
                                                    ConstantesAppServicio.OriglectcodiDespachomediahora.ToString(), ConstantesAppServicio.ParametroDefecto)
                                                    .Where(x => x.Ptomediestado != ConstantesAppServicio.Anulado && x.Ptomedicodi > 0).ToList();

            TitularidadAppServicio servTitEmp = new TitularidadAppServicio();

            //Consulta el histórico de relación entre los puntos de medición y las empresas
            List<SiHisempptoDataDTO> listaHist = servTitEmp.ListSiHisempptoDatas(string.Join(",", listaPto.Select(x => x.Ptomedicodi).ToList()))
                                                .Where(x => x.Hptdatfecha <= fechaPeriodo).ToList();

            servTitEmp.SetTTIEptoToMePtomedicion(listaPto, listaHist);

            return listaPto;
        }

        private List<EveHoraoperacionDTO> ListarHOFromYupanaProgramadoDiario(string tipo, DateTime fechaIni, DateTime fechaFin, List<PrGrupoDTO> listaAllGrupo)
        {
            List<EveHoraoperacionDTO> listaHO = new List<EveHoraoperacionDTO>();
            int hopcodi = 1;

            for (var day = fechaIni; day <= fechaFin; day = day.AddDays(1))
            {
                DateTime fechaConsultaYup = ConstantesCortoPlazo.TopologiaDiario == tipo ? day : EPDate.f_fechainiciosemana(day);
                int topcodi = GetTopcodiOficialProgramado(fechaConsultaYup, tipo);

                if (topcodi > 0)
                {
                    List<CpMedicion48DTO> listaCcomb = FactorySic.GetCpMedicion48Repository().GetByCriteria(topcodi.ToString(), day, ConstantesBase.SresPotTermica.ToString());

                    foreach (var reg in listaCcomb)
                    {
                        listaHO.AddRange(UtilCdispatch.ListarObjYupanaHoxM48(reg, listaAllGrupo, ref hopcodi));
                    }
                }
            }

            return listaHO;
        }

        private void ListarMantenimientosXHorizonte(int evenclasecodi, DateTime fechaIni, DateTime fechaFin, out List<EveManttoDTO> listaManttoGen, out List<EveManttoDTO> listaManttoCamisea)
        {
            var servIntervencion = new IntervencionesAppServicio();

            IntervencionFiltro objFiltro = servIntervencion.GetFiltroConsulta2(new IntervencionFiltro()
            {
                Progrcodi = 0,
                Evenclasecodi = evenclasecodi,
                FechaIni = fechaIni,
                FechaFin = fechaFin,
                StrIdsEmpresa = ConstantesIntervencionesAppServicio.sFiltroTodos,
                EsReporteExcel = true, //intervenciones válidas
                AgruparIntervencion = false, //dividido por dia
                StrIdsEstados = ConstantesIntervencionesAppServicio.InEstadoConforme.ToString() + "," + ConstantesIntervencionesAppServicio.InEstadoAprobado.ToString() + "," + ConstantesIntervencionesAppServicio.sEstadoEnProceso.ToString(), //registrados por el agente y coordinador
            });
            objFiltro.StrIdsFamilias = "3,5,2,4,34,33,37,36,39,38"; //generacion, planta y gaseoducto
            objFiltro.StrIdsDisponibilidad = ConstantesIntervencionesAppServicio.sFS; //fuera de servicio

            List<InIntervencionDTO> listarIntervencion = servIntervencion.ConsultarIntervencionesTabulares(objFiltro);

            //convertir a EveMantto
            List<EveManttoDTO> listaMantto = new List<EveManttoDTO>();
            foreach (var reg in listarIntervencion)
            {
                var mantto = IntervencionesAppServicio.ConvertirManto(reg);
                listaMantto.Add(mantto);
            }

            //salidas
            List<int> listaEquicodiCamisea = UtilCdispatch.ListaEquicodiCamisea();

            listaManttoGen = listaMantto.Where(x => !listaEquicodiCamisea.Contains(x.Equicodi ?? 0)).ToList();
            listaManttoCamisea = listaMantto.Where(x => listaEquicodiCamisea.Contains(x.Equicodi ?? 0)).ToList();
        }

        private List<MeMedicion1DTO> ListarStockProgramado(DateTime fechaIni, DateTime fechaFin)
        {
            var servStockComb = new StockCombustibleAppServicio();

            int tipoinfocodiGas = 46; //Gas natural (Mm3) millones de metros cubicos

            //este metodo retorna todos los puntos de medicion asi no tengan datos en BD
            List<MeMedicion1DTO> listaSucad = servStockComb.ObtenerConsultaStock(fechaIni, fechaFin, -1, -1, tipoinfocodiGas);

            foreach (var reg in listaSucad)
            {
                //if (reg.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiGas)
                //{
                if (reg.Tipoinfocodi == 46) //Millones de metros cubicos
                {
                    reg.H1 = reg.H1.GetValueOrDefault(0) * 1000.0m;
                }
                //}
            }

            List<int> lPtomedicodi = listaSucad.Select(x => x.Ptomedicodi).Distinct().ToList();

            //replicar para los siguientes dias (semanal)
            for (DateTime fecha = fechaIni.AddDays(1); fecha <= fechaFin; fecha = fecha.AddDays(1))
            {
                var listaDiaAnt = listaSucad.Where(x => x.Medifecha == fecha.AddDays(-1)).ToList();
                var listaDiaAct = listaSucad.Where(x => x.Medifecha == fecha).ToList();

                foreach (var regDiaAct in listaDiaAct)
                {
                    var regDiaAnt = listaDiaAnt.Find(x => x.Ptomedicodi == regDiaAct.Ptomedicodi);
                    if (regDiaAct.H1.GetValueOrDefault(0) == 0 && regDiaAnt != null)
                    {
                        //buscar en el día anterior
                        regDiaAct.H1 = regDiaAnt.H1;
                    }
                }
            }

            return listaSucad;
        }

        /// <summary>
        /// Obtener el valor del tipo de cambio
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public decimal GetTipoCambio(DateTime fecha)
        {
            decimal tipocambio = 0;

            var listaParamGenerales = FactorySic.GetPrGrupodatRepository().ParametrosConfiguracionPorFecha(fecha, "0", "1");
            if (listaParamGenerales.Count > 0)
            {
                var tipcambio = listaParamGenerales.Find(x => x.Concepabrev == "TCambio");
                if (tipcambio != null) { tipocambio = decimal.Parse(tipcambio.Formuladat); }
            }

            return tipocambio;
        }

        #endregion

        #region CDispatch - Plantilla Reporte

        public List<PrGrupoDTO> FillCabeceraCdispatchActual()
        {
            //>>>>>>>>>>>>>>>>>>>>Flujos de Potencia>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            var listaPtosReporteFl = FactorySic.GetMeReporptomedRepository().GetByCriteria(ConstantesMigraciones.ReporteCdispatchFl, -1).OrderBy(x => x.Repptoorden).ToList();

            var listaPtosReporteCmg = FactorySic.GetMeReporptomedRepository().GetByCriteria(ConstantesMigraciones.ReporteCdispatchCmg, -1).OrderBy(x => x.Repptoorden).ToList();

            return UtilCdispatch.FillCabeceraCdispatchActual(listaPtosReporteFl, listaPtosReporteCmg);
        }

        public List<PrGrupoDTO> FillCabeceraCdispatchHistorico()
        {
            List<MePtomedicionDTO> listaPtoMediaHora = FactorySic.GetMePtomedicionRepository().GetByCriteria3(2, "-1", ConstantesAppServicio.TipoinfocodiFlujo + "," + ConstantesAppServicio.TipoinfocodiDolar);
            var listaPtosReporteFl = FactorySic.GetMeReporptomedRepository().GetByCriteria(ConstantesMigraciones.ReporteCdispatchFl, -1).OrderBy(x => x.Repptoorden).ToList();
            var listaPtosReporteCmg = FactorySic.GetMeReporptomedRepository().GetByCriteria(ConstantesMigraciones.ReporteCdispatchCmg, -1).OrderBy(x => x.Repptoorden).ToList();

            List<MePtomedicionDTO> listaPto = FactorySic.GetMePtomedicionRepository().List(ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.OriglectcodiDespachomediahora.ToString())
                .Where(x => x.Tipoinfocodi > 0 && x.Grupocodi.GetValueOrDefault(0) <= 0).ToList();

            return UtilCdispatch.FillCabeceraCdispatchHistorico(listaPto, listaPtoMediaHora, listaPtosReporteFl, listaPtosReporteCmg);
        }

        #endregion

        #region CDispatch - Notificación

        private void EnviarNotificacionCargaDespacho(DateTime fechaPeriodo, DateTime fechaRegistro, string usuarioLogin, string nombreUsuario)
        {
            EnviarNotificacionCDispatch(ConstantesMigraciones.PlantcodiNotificacionCargaDespacho, fechaPeriodo, fechaRegistro, usuarioLogin, nombreUsuario);
        }

        private void EnviarNotificacionRecalculoCostoOp(DateTime fechaPeriodo, DateTime fechaRegistro, string usuarioLogin, string nombreUsuario)
        {
            EnviarNotificacionCDispatch(ConstantesMigraciones.PlantcodiNotificacionRecalculoCostoOp, fechaPeriodo, fechaRegistro, usuarioLogin, nombreUsuario);
        }

        private void EnviarNotificacionCDispatch(int plantcodi, DateTime fechaPeriodo, DateTime fechaRegistro, string usuarioLogin, string nombreUsuario)
        {
            CorreoAppServicio servCorreo = new CorreoAppServicio();

            //Generar Tupla de Variable y valor
            var mapaVariable = new Dictionary<string, string>();
            mapaVariable[ConstantesMigraciones.VariableNombreUsuario] = nombreUsuario;
            mapaVariable[ConstantesMigraciones.VariableFechaSistema] = fechaRegistro.ToString(ConstantesAppServicio.FormatoFechaFull);
            mapaVariable[ConstantesMigraciones.VariableFechaProceso] = fechaPeriodo.ToString(ConstantesAppServicio.FormatoFecha);

            try
            {
                SiPlantillacorreoDTO plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                string from = TipoPlantillaCorreo.MailFrom;
                string to = CorreoAppServicio.GetTextoSinVariable(plantilla.Planticorreos, mapaVariable);
                string cc = CorreoAppServicio.GetTextoSinVariable(plantilla.PlanticorreosCc, mapaVariable);
                string bcc = CorreoAppServicio.GetTextoSinVariable(plantilla.PlanticorreosBcc, mapaVariable);
                string asunto = CorreoAppServicio.GetTextoSinVariable(plantilla.Plantasunto, mapaVariable);
                string contenido = CorreoAppServicio.GetTextoSinVariable(plantilla.Plantcontenido, mapaVariable);

                List<string> listaTo = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(to);
                List<string> listaCC = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(cc);
                List<string> listaBCC = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(bcc, true, true);
                string asuntoEmail = CorreoAppServicio.GetTextoAsuntoSegunAmbiente(asunto);
                to = string.Join(";", listaTo);
                cc = string.Join(";", listaCC);
                bcc = string.Join(";", listaBCC);

                //Enviar correo
                COES.Base.Tools.Util.SendEmail(listaTo, listaCC, listaBCC, asuntoEmail, contenido, null);

                SiCorreoDTO correo = new SiCorreoDTO();
                correo.Corrasunto = asunto;
                correo.Corrcontenido = contenido;
                correo.Corrfechaenvio = fechaRegistro;
                correo.Corrfechaperiodo = fechaPeriodo;
                correo.Corrfrom = from;
                correo.Corrto = to;
                correo.Corrcc = cc;
                correo.Corrbcc = bcc;
                correo.Emprcodi = 1; //COES
                correo.Plantcodi = plantilla.Plantcodi;
                correo.Corrusuenvio = usuarioLogin;
                servCorreo.SaveSiCorreo(correo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
        }

        #endregion

        #region CDispatch - Carga Excel

        /// <summary>
        /// LeerFileUpxls
        /// </summary>
        /// <param name="nombreFile"></param>
        /// <param name="path"></param>
        /// <param name="lectcodi"></param>
        /// <param name="listaMe48"></param>
        /// <param name="listaOK"></param>
        /// <param name="listaError"></param>
        public void LeerFileUpxls(string nombreFile, string path, int lectcodi, HtFiltro flagCarga, bool flagAutomatico, out List<MeMedicion48DTO> listaMe48,
                                out List<string> listaOK, out List<string> listaError, out List<HtError> listaError2)
        {
            listaMe48 = new List<MeMedicion48DTO>();
            listaOK = new List<string>();
            listaError = new List<string>();
            listaError2 = new List<HtError>();

            string rutaFile = path + nombreFile;
            FileInfo fileInfo = new FileInfo(rutaFile);

            //puntos de medición Generadores
            List<MePtomedicionDTO> listaAllPtos = new List<MePtomedicionDTO>();

            //puntos de medición No generadores
            List<PrGrupoDTO> listaPtoNoGeneradores = FillCabeceraCdispatchHistorico();

            //Leer las hojas
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                int filaData = 0, columnPtos = 0, filaFecha = 0, columnFecha = 0, rowPto = 1, filaNombPto = 8;
                ExcelWorksheet ws = null;

                DateTime fechaPeriodo = DateTime.Today;

                if (lectcodi.ToString() == ConstantesAppServicio.LectcodiEjecutadoHisto)
                {
                    List<MeMedicion48DTO> datosMW = new List<MeMedicion48DTO>();
                    List<MeMedicion48DTO> datosMVar = new List<MeMedicion48DTO>();
                    List<MeMedicion48DTO> datosFlujos = new List<MeMedicion48DTO>();
                    List<MeMedicion48DTO> datosHidro = new List<MeMedicion48DTO>();

                    #region pestaña activa

                    ws = xlPackage.Workbook.Worksheets["Activa"];
                    if (ws != null)
                    {
                        if (flagCarga.FlagCDispatchCargaActiva)
                        {
                            filaData = 9;
                            filaNombPto = 8;
                            columnPtos = 2;
                            filaFecha = 7;
                            columnFecha = 1;
                            rowPto = 1;
                            fechaPeriodo = UtilCdispatch.GetFechaHTrabajo(ws, filaFecha, columnFecha);
                            listaAllPtos = ListarAllPtoDespachoConTTIE(fechaPeriodo);

                            var listaColores = ListPrHtrabajoEstados();

                            datosMW = UtilCdispatch.LeerDatosMe48FromHoja(ws, lectcodi, fechaPeriodo, ConstantesAppServicio.TipoinfocodiMW.ToString(), filaData, filaNombPto, columnPtos, rowPto
                                , 48, 1, listaAllPtos, listaPtoNoGeneradores, true, flagAutomatico, listaColores, out List<HtError> lstErrorColores);
                            listaError2.AddRange(lstErrorColores);
                            listaOK.Add("Pestaña \"Activa\" ingresada correctamente");
                        }
                    }
                    else { listaError.Add("No se encontro pestaña \"Activa\""); }

                    #endregion

                    #region pestaña reactiva

                    ws = xlPackage.Workbook.Worksheets["Reactiva"];
                    if (ws != null)
                    {
                        if (flagCarga.FlagCDispatchCargaReactiva)
                        {
                            filaData = 9;
                            filaNombPto = 8;
                            columnPtos = 2;
                            rowPto = 1;
                            datosMVar = UtilCdispatch.LeerDatosMe48FromHoja(ws, lectcodi, fechaPeriodo, ConstantesAppServicio.TipoinfocodiMVAR.ToString(), filaData, filaNombPto, columnPtos, rowPto
                                , 48, 1, listaAllPtos, listaPtoNoGeneradores, false, flagAutomatico, new List<PrHtrabajoEstadoDTO>(), out List<HtError> lstErrorColores);
                            listaError2.AddRange(lstErrorColores);
                            listaOK.Add("Pestaña \"Reactiva\" ingresada correctamente");
                        }

                    }
                    else { listaError.Add("Hoja 'REAC': NO EXISTE!!"); }

                    #endregion

                    #region pestaña flujos

                    ws = xlPackage.Workbook.Worksheets["Flujos"];
                    if (ws != null)
                    {
                        filaData = 12;
                        filaNombPto = 11;
                        columnPtos = 3;
                        rowPto = 2;

                        datosFlujos = UtilCdispatch.LeerDatosMe48FromHoja(ws, lectcodi, fechaPeriodo, ConstantesAppServicio.TipoinfocodiFlujo.ToString(), filaData, filaNombPto, columnPtos, rowPto
                            , 48, 1, listaAllPtos, listaPtoNoGeneradores, false, flagAutomatico, new List<PrHtrabajoEstadoDTO>(), out List<HtError> lstErrorColores);
                        listaError2.AddRange(lstErrorColores);
                        listaOK.Add("Pestaña \"Flujo\" ingresada correctamente");
                    }
                    else { listaError.Add("Hoja 'FLUJOS': NO EXISTE!!"); }

                    #endregion

                    #region pestaña Hidrologia

                    ws = xlPackage.Workbook.Worksheets["Hidrologia"];
                    if (ws != null)
                    {
                        if (flagCarga.FlagCDispatchCargaHidrologia)
                        {
                            filaData = 11;
                            filaNombPto = 4;
                            columnPtos = 2;
                            rowPto = 1;

                            datosHidro = UtilCdispatch.LeerDatosMe48FromHoja(ws, lectcodi, fechaPeriodo, "11,13,19", filaData, filaNombPto, columnPtos, rowPto
                                , 48, 1, listaAllPtos, listaPtoNoGeneradores, false, flagAutomatico, new List<PrHtrabajoEstadoDTO>(), out List<HtError> lstErrorColores);
                            listaError2.AddRange(lstErrorColores);
                            listaOK.Add("Pestaña \"Hidro\" ingresada correctamente");
                        }
                    }
                    else { listaError.Add("Hoja 'HIDRO': NO EXISTE!!"); }

                    #endregion

                    listaMe48.AddRange(datosMW);
                    listaMe48.AddRange(datosMVar);
                    listaMe48.AddRange(datosFlujos);
                    listaMe48.AddRange(datosHidro);
                }

                if (lectcodi.ToString() == ConstantesAppServicio.LectcodiReprogDiario)
                {
                    List<MeMedicion48DTO> datosReprog = new List<MeMedicion48DTO>();

                    #region pestaña activa

                    ws = xlPackage.Workbook.Worksheets["Activa"];
                    if (ws != null)
                    {
                        filaFecha = 7;
                        columnFecha = 1;
                        fechaPeriodo = UtilCdispatch.GetFechaHTrabajo(ws, filaFecha, columnFecha);
                    }
                    else
                    { listaError.Add("No se encontro pestaña \"Activa\""); }

                    #endregion

                    #region pestaña reprogramado

                    ws = xlPackage.Workbook.Worksheets["Reprograma"];
                    if (ws != null)
                    {
                        if (flagCarga.FlagCDispatchCargaReprograma)
                        {
                            filaData = 9;
                            filaNombPto = 8;
                            columnPtos = 2;
                            rowPto = 1;
                            listaAllPtos = ListarAllPtoDespachoConTTIE(fechaPeriodo);

                            datosReprog = UtilCdispatch.LeerDatosMe48FromHoja(ws, lectcodi, fechaPeriodo, ConstantesAppServicio.TipoinfocodiMW.ToString(), filaData, filaNombPto, columnPtos, rowPto
                                , 48, 1, listaAllPtos, listaPtoNoGeneradores, false, flagAutomatico, new List<PrHtrabajoEstadoDTO>(), out List<HtError> lstErrorColores);
                            listaError2.AddRange(lstErrorColores);
                            listaOK.Add("Pestaña \"Reprograma\" ingresada correctamente");
                        }
                    }
                    else { listaError.Add("No se encontro pestaña \"Reprograma\""); }

                    #endregion

                    listaMe48.AddRange(datosReprog);
                }
            }

            //filtrar errores distinto a colores
            var listaAlertas = new List<ErrorHtrabajo>();
            foreach (var listaAgrupada in listaError2.Where(x => !(x.Descripcion ?? "").ToUpper().Contains(" COLOR ")).GroupBy(x => new { x.Ptomedicion, x.Descripcion }))
            {
                var celdas = listaAgrupada.ToList().Select(x => x.Posicion).ToList();
                var posicion = string.Join(", ", celdas);
                listaAlertas.Add(new ErrorHtrabajo { Ptomedicion = listaAgrupada.Key.Ptomedicion, Posicion = posicion, Descripcion = listaAgrupada.Key.Descripcion });
            }
            listaError.AddRange(listaAlertas.Select(x => string.Format("Punto {0}, Celdas: {1}, Mensaje: {2}", x.Ptomedicion, x.Posicion, x.Descripcion)).ToList());

            //calcular meditotal de forma temporal
            listaMe48 = UtilCdispatch.FormatearLista48(listaMe48, "SISTEMA", DateTime.Now);
        }

        public void LoadDispatchFromHtrabajo(int lectcodi, List<MeMedicion48DTO> lista48Input, out CDespachoGlobal regCDespacho)
        {
            regCDespacho = null;

            if (lista48Input.Any())
            {
                List<DateTime> lFecha = lista48Input.Select(x => x.Medifecha).Distinct().OrderBy(x => x).ToList();
                DateTime fechaIni = lFecha.First();
                DateTime fechaFin = lFecha.Last();

                this.Load_Dispatch(fechaIni, fechaFin, lectcodi, ConstantesAppServicio.ParametroDefecto, false, true, true, lista48Input, out regCDespacho);

            }
        }

        #endregion

        #region CDispatch - Cargar BD (MeMedicion48 y MeMedicion1)

        /// <summary>
        /// Grabar datos en medicion48 y medicion1 de HTrabajo
        /// </summary>
        /// <param name="regCDespacho"></param>
        /// <param name="usuarioLogin"></param>
        /// <param name="usuario"></param>
        public void GrabarCDispatch(CDespachoGlobal regCDespacho, string usuarioLogin, string usuario, bool flagAutomatico)
        {
            int lectcodi = regCDespacho.Lectcodi;
            DateTime fechaProceso = regCDespacho.FechaIni;
            CDespachoDiario regDia = regCDespacho.ListaCDespachoDiario.Find(x => x.Fecha == fechaProceso);

            List<MeMedicion48DTO> listaMe48 = regDia.ListaMe48XDia ?? new List<MeMedicion48DTO>();
            List<MeMedicion1DTO> listaMe1 = regDia.ListaMe1XDia ?? new List<MeMedicion1DTO>();

            if (listaMe48.Any() && listaMe1.Any())
            {
                DateTime fechaActualizacion = DateTime.Now;

                //Almacenar MW y Mvar
                List<int> listaTipoinfocodiGrabar = new List<int>() { ConstantesAppServicio.TipoinfocodiMW, ConstantesAppServicio.TipoinfocodiMVAR,
                    ConstantesAppServicio.TipoinfocodiM3s,ConstantesAppServicio.TipoinfocodiMm3,ConstantesAppServicio.Tipoinfocodim,ConstantesAppServicio.TipoinfocodiDemanda };

                //solo guardar los puntos de medición mayor que cero (no incluye los calculos en memoria)
                List<MeMedicion48DTO> lista48Guardar = listaMe48.Where(x => x.Medifecha == fechaProceso && x.Ptomedicodi > 0 && listaTipoinfocodiGrabar.Contains(x.Tipoinfocodi)).ToList();

                //calcular meditotal
                lista48Guardar = UtilCdispatch.FormatearLista48(lista48Guardar, usuario, fechaActualizacion);

                //en Despacho no se guardan los que tienen cero todo el día
                lista48Guardar = lista48Guardar.Where(x => x.Meditotal.GetValueOrDefault(0) != 0).ToList();

                //proceso de guardar informacion a BD
                if (lista48Guardar.Any())
                {
                    //1. Grabar registros cada 30minutos
                    DeleteMemedicionmasivo48(lectcodi, fechaProceso, "-2147483647", "-2147483647");
                    foreach (var ins in lista48Guardar)
                    {
                        //SaveMemedicion48(ins);
                        FactorySic.GetMeMedicion48Repository().SaveInfoAdicional(ins);
                    }

                    //2. Grabar costo de operación                    
                    MeMedicion1DTO regCostoXDia = listaMe1.Find(x => x.Medifecha == fechaProceso && x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiSoles && x.Ptomedicodi == ConstantesAppServicio.PtomedicodiCostoOpeDia);

                    if (regCostoXDia != null && regCostoXDia.H1 > 0)
                    {
                        DeleteMemedicion1(lectcodi, fechaProceso, ConstantesAppServicio.TipoinfocodiSoles, ConstantesAppServicio.PtomedicodiCostoOpeDia);
                        regCostoXDia.Lastdate = fechaActualizacion;
                        regCostoXDia.Lastuser = usuario;
                        SaveMemedicion1(regCostoXDia);
                    }

                    if (!flagAutomatico)
                    {
                        //3. Enviar notificación a usuarios SGI
                        EnviarNotificacionCargaDespacho(regCDespacho.FechaIni, fechaActualizacion, usuarioLogin, usuario);
                    }

                    //4. Actualizar valores de Demanda x áreas
                    if (lectcodi.ToString() == ConstantesAppServicio.LectcodiEjecutadoHisto)
                    {
                        (new CargaDatosAppServicio()).CopiarDataALecturaCargaIEOD(fechaProceso, usuarioLogin, fechaActualizacion);
                    }
                }
            }
        }

        /// <summary>
        /// Grabar datos en medicion1 
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="costoOpAnterior"></param>
        /// <param name="costoOpNuevo"></param>
        /// <param name="usuarioLogin"></param>
        /// <param name="nombreUsuario"></param>
        public void GrabarRecalculoCostoOperacionEjecutado(DateTime fechaPeriodo, decimal costoOpAnterior, decimal costoOpNuevo, string usuarioLogin, string nombreUsuario)
        {
            //la base de datos permite 5 decimales
            costoOpAnterior = Math.Round(costoOpAnterior, 5);
            costoOpNuevo = Math.Round(costoOpNuevo, 5);

            if (costoOpAnterior != costoOpNuevo)
            {
                DateTime fechaRegistro = DateTime.Now;

                int ptomedicodi = ConstantesAppServicio.PtomedicodiCostoOpeDia;
                int tipoinfocodi = ConstantesAppServicio.TipoinfocodiSoles;
                int lectcodi = Convert.ToInt32(ConstantesAppServicio.LectcodiEjecutadoHisto);

                //registro BD
                MeMedicion1DTO regCostoXDia = new MeMedicion1DTO()
                {
                    Medifecha = fechaPeriodo,
                    Ptomedicodi = ptomedicodi,
                    Tipoinfocodi = tipoinfocodi,
                    Lectcodi = lectcodi,
                    H1 = costoOpNuevo
                };
                if (regCostoXDia != null && regCostoXDia.H1 > 0)
                {
                    this.DeleteMemedicion1(lectcodi, fechaPeriodo, tipoinfocodi, ptomedicodi);
                    regCostoXDia.Lastdate = fechaRegistro;
                    regCostoXDia.Lastuser = usuarioLogin;
                    this.SaveMemedicion1(regCostoXDia);
                }

                //notificación
                EnviarNotificacionRecalculoCostoOp(fechaPeriodo, fechaRegistro, usuarioLogin, nombreUsuario);
            }
        }

        #endregion

        #region CDispatch - Reserva Fría, Reserva Rotante, MWxMantto

        /// <summary>
        /// Obtener los datos Cdispatch
        /// </summary>
        public void Load_Dispatch(DateTime fechaIni, DateTime fechaFin, int lectcodi, string empresas, bool esDespachoBaseDatos, bool esCalcularCostos, bool esCalcularRFria
                                   , List<MeMedicion48DTO> lista48Memoria, out CDespachoGlobal regCDespacho)
        {
            esCalcularCostos = esCalcularCostos && (ConstantesAppServicio.LectcodiEjecutadoHisto == lectcodi.ToString() || ConstantesAppServicio.LectcodiReprogDiario == lectcodi.ToString());

            //utilizar datos BD o importados del excel en memoria
            List<MeMedicion48DTO> listaAllMe48;
            List<MeMedicion1DTO> listaAllMe1 = new List<MeMedicion1DTO>();
            if (esDespachoBaseDatos)
            {
                //MW, MVar, 
                listaAllMe48 = GetObtenerHistoricoMedicion48(lectcodi, fechaIni, fechaFin);
                listaAllMe1 = GetListaMedicion1(lectcodi, fechaIni, fechaFin);
            }
            else
            {
                listaAllMe48 = lista48Memoria ?? new List<MeMedicion48DTO>();
            }

            CDespachoInput regInput = new CDespachoInput()
            {
                FechaIni = fechaIni,
                FechaFin = fechaFin,
                Lectcodi = lectcodi,
                Empresas = empresas,
                ListaAllMe48 = listaAllMe48,
                ListaAllMe1 = listaAllMe1,
                EsCalcularCostos = esCalcularCostos,
                EsCalcularRFria = esCalcularRFria,
            };

            regCDespacho = new CDespachoGlobal();

            //Consultas a BD para obtener los insumos
            CargarInsumoPlantillaGeneracionCDispatch(regInput, ref regCDespacho);
            CargarInsumoCalculoCDispatch(regInput, ref regCDespacho);

            //Setear datos por días
            UtilCdispatch.AsignarVariablesPlantillaCDispatch(regInput, ref regCDespacho);
            UtilCdispatch.AsignarVariablesCalculoXUnidadGeneracion(ref regCDespacho);

            //Cálculo de la Reserva Fría x Punto de medición
            UtilCdispatch.ListarReporteCDispatch(regCDespacho, out ResultadoCDespacho reporteRFriaOut2);

            //Cálculo del Costo de la Operación x Punto de medición
            UtilCdispatch.ListarReporteCostoOperacionCDispatch(regCDespacho, ref reporteRFriaOut2);

            regCDespacho.ReporteRFria = reporteRFriaOut2;

            //Cálculo de la Oferta diaria (COES y no COES), Renovable, Cogeneración, Generación COES
            UtilCdispatch.ListarPuntoMedicionMemoriaCDispatch(ref regCDespacho);

            //Actualizar datos en memoria
            UtilCdispatch.ActualizarConDatoCalculado(regInput, ref regCDespacho);

        }

        public void CargarInsumoPlantillaGeneracionCDispatch(CDespachoInput regInput, ref CDespachoGlobal regCDespacho)
        {
            //parámetros de entrada
            int lectcodi = regInput.Lectcodi;
            DateTime fechaIni = regInput.FechaIni;
            DateTime fechaFin = regInput.FechaFin;
            List<MeMedicion48DTO> listaAllMe48 = regInput.ListaAllMe48 ?? new List<MeMedicion48DTO>();
            List<MeMedicion1DTO> listaAllMe1 = regInput.ListaAllMe1 ?? new List<MeMedicion1DTO>();

            //variables
            //string equiestado = "'A','F','P','B'", ptomediestado = "'A','B','P','F'", grupoactivo = "'S','N'";
            List<string> listaAllEstado = new List<string>() { "A", "P", "F", "B" };
            List<string> listaEstadoValido = new List<string>() { "A", "P", "F" };
            List<int> catecodiCentral = new List<int>() { 4, 6, 16, 18 }; //CENTRAL TERMICA, CENTRAL HIDRAULICA
            List<int> catecodiGrupos = new List<int>() { 3, 5, 15, 17 }; //GRUPO TERMICO, GRUPO HIDRAULICO
            //List<int> catecodiGrupoFunc = new List<int>() { 2, 9 }; //MODO DE OPERACION TERMICO, MODO DE OPERACION HIDRO
            List<int> listaCatecodiConData = new List<int>();
            listaCatecodiConData.AddRange(catecodiCentral);
            listaCatecodiConData.AddRange(catecodiGrupos);
            //listaCatecodiConData.AddRange(catecodiGrupoFunc);
            string catecodisData = string.Join(",", listaCatecodiConData);

            //variables debug
            DateTime fechaHoraIni = DateTime.Now;
            int contadorIni = 1;

            //
            regCDespacho.Lectcodi = lectcodi;
            regCDespacho.ListaCDespachoDiario = new List<CDespachoDiario>(); //Detalle de cada día
            regCDespacho.FechaIni = fechaIni;
            regCDespacho.FechaFin = fechaFin;
            regCDespacho.ListaFecha = new List<DateTime>();
            regCDespacho.ListaCatecodiConData = listaCatecodiConData;

            regCDespacho.ListaAllMe48 = listaAllMe48;
            regCDespacho.ListaAllMe1 = listaAllMe1;
            regCDespacho.ListaAllMe1BD = listaAllMe1;

            //Lista de empresas de la base de datos del COES, para la presentacion
            regCDespacho.ListaSiEmpresaBD = ListarEmpresasGeneradoras();
            foreach (var reg in regCDespacho.ListaSiEmpresaBD)
            {
                reg.Emprorden = reg.Emprorden != null ? reg.Emprorden.Value : 9999999;
            }

            UtilCdispatch.LogDiffSegundos(fechaHoraIni, contadorIni, out fechaHoraIni, out contadorIni);

            //Lista de equipos (se incluyen a los que tiene estado de proyecto)
            List<EqEquipoDTO> listaAllEquipo = FactorySic.GetEqEquipoRepository().ListarEquiposPorFamilia(ConstantesHorasOperacion.CodFamilias + "," + ConstantesHorasOperacion.CodFamiliasGeneradores)
                                                                        .Where(x => listaAllEstado.Contains(x.Equiestado)).ToList();

            //Categorizar equipos
            List<EqCategoriaDetDTO> listaSubcategoriaXGas = FactorySic.GetEqCategoriaDetalleRepository().ListByCategoriaAndEstado(ConstantesPR5ReportesServicio.CategoriaRecursoGas, ConstantesAppServicio.Activo);
            foreach (var reg in listaSubcategoriaXGas)
            {
                List<EqCategoriaEquipoDTO> listaEquipoCtg = FactorySic.GetEqCategoriaEquipoRepository().ListaClasificacionByCategoriaDetalle(reg.Ctgdetcodi);
                List<int> listaEquicodi = listaEquipoCtg.Select(x => x.Equicodi).Where(x => x > 0).Distinct().ToList();

                foreach (var equicodi in listaEquicodi)
                {
                    var regEq = listaAllEquipo.Find(x => x.Equicodi == equicodi);
                    if (regEq != null) regEq.Ctgdetcodi = reg.Ctgdetcodi;
                }
            }
            List<int> listaEquicodiTieneConexionCamisea = listaAllEquipo.Where(x => x.Ctgdetcodi == ConstantesPR5ReportesServicio.SubCategoriaRecursoGasNatural).Select(x => x.Equicodi).ToList();
            regCDespacho.ListaEquicodiTieneConexionCamisea = listaEquicodiTieneConexionCamisea;

            UtilCdispatch.LogDiffSegundos(fechaHoraIni, contadorIni, out fechaHoraIni, out contadorIni);

            //Lista de grupos
            INDAppServicio servInd = new INDAppServicio();
            List<PrGrupoDTO> listaAllGrupo = servInd.ListarGrupoValido(catecodisData);
            regCDespacho.ListaAllGrupoBD = listaAllGrupo;
            regCDespacho.ListaGrupoXGrupoDespachoHistorico = listaAllGrupo.Where(x => listaCatecodiConData.Contains(x.Catecodi) && x.Grupocodi > 0).ToList();
            regCDespacho.ListaGrupoXGrupoDespachoActivo = listaAllGrupo.Where(x => catecodiGrupos.Contains(x.Catecodi) && x.Grupocodi > 0).ToList();

            // Lista de puntos (Origen de lectura 2	Mediciones Despacho 1/2 hora)
            string origlectcodi = ConstantesAppServicio.OriglectcodiDespachomediahora.ToString();
            if (ConstantesPR5ReportesServicio.LectDespachoEjecutado == lectcodi) origlectcodi = ConstantesPR5ReportesServicio.OrigLecturaPR05IEOD;
            List<MePtomedicionDTO> listaPtoUniverso = FactorySic.GetMePtomedicionRepository().GetByCriteria(ConstantesAppServicio.ParametroDefecto, origlectcodi, ConstantesAppServicio.ParametroDefecto)
                                                        .Where(x=>x.Ptomedicodi>0).ToList();

            //filtrar puntos
            List<MePtomedicionDTO> listaPto;
            if (lectcodi == ConstantesPR5ReportesServicio.LectDespachoEjecutado)
            {
                var listaHojaPto = FactorySic.GetMeHojaptomedRepository().ListarHojaPtoByFormatoAndEmpresa(-1, ConstantesHard.IdFormatoDespacho.ToString()).Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMW).ToList();
                var listaPtomedicodiData = listaAllMe48.Select(x => x.Ptomedicodi).Distinct().ToList();
                var listaPtomedicodiFmt = listaHojaPto.Select(x => x.Ptomedicodi).Distinct().ToList();
                listaPto = listaPtoUniverso.Where(x => listaPtomedicodiData.Contains(x.Ptomedicodi) ||
                                            (listaPtomedicodiFmt.Contains(x.Ptomedicodi) && x.Ptomediestado != ConstantesAppServicio.Anulado)).ToList();
            }
            else
            {
                var listaPtomedicodiData = listaAllMe48.Select(x => x.Ptomedicodi).Distinct().ToList();
                listaPto = listaPtoUniverso.Where(x => listaPtomedicodiData.Contains(x.Ptomedicodi) || x.Ptomediestado != ConstantesAppServicio.Anulado).ToList();
            }

            foreach (var regPto in listaPto)
            {
                if (regPto.Ptomedicodi == 62326) 
                { }
                var regEq = listaAllEquipo.Find(x => x.Equicodi == regPto.Equicodi);
                if (regEq != null)
                {
                    bool esEqCentral = (regEq.Famcodi == ConstantesHorasOperacion.IdTipoSolar || regEq.Famcodi == ConstantesHorasOperacion.IdTipoEolica 
                                                || regEq.Famcodi == ConstantesHorasOperacion.IdTipoHidraulica || regPto.Famcodi == ConstantesHorasOperacion.IdTipoTermica);
                    bool esEqGen = (regEq.Famcodi == ConstantesHorasOperacion.IdGeneradorSolar || regEq.Famcodi == ConstantesHorasOperacion.IdGeneradorEolica
                                                || regEq.Famcodi == ConstantesHorasOperacion.IdGeneradorHidroelectrico || regPto.Famcodi == ConstantesHorasOperacion.IdGeneradorTemoelectrico);
                    
                    if (esEqCentral || esEqGen)
                    {
                        //determinar la categoria del equipo
                        if (regEq.Ctgdetcodi > 0) regPto.Ctgdetcodi = regEq.Ctgdetcodi;

                        //determinar la central del punto
                        regPto.Equipadre = esEqCentral ? regEq.Equicodi : regEq.Equipadre ?? 0;
                        var regPadre = listaAllEquipo.Find(x => x.Equicodi == regPto.Equipadre);
                        if (regPadre != null)
                        {
                            regPto.Padrenomb = regPadre.Equinomb;
                            regPto.Central = regPadre.Equinomb;

                            //determinar la categoria del equipo
                            if (regPadre.Ctgdetcodi > 0) regPto.Ctgdetcodi = regPadre.Ctgdetcodi;
                        }

                        //verificar que el punto de medición tiene grupo despacho
                        if (regPto.Grupocodi.GetValueOrDefault(0) <= 0)
                        {
                            var grupocodiCandidato= regEq.Grupocodi;

                            var regGr = regCDespacho.ListaAllGrupoBD.Find(x => grupocodiCandidato > 0 && x.Grupocodi == grupocodiCandidato);
                            if (regGr != null)
                            {
                                var regMsj = new ResultadoValidacionAplicativo() { TipoValidacion = ConstantesPR5ReportesServicio.MensajeError, Descripcion = string.Format("[{0}, {2}]El punto de medición {0} {1} no tiene asociado un Grupo despacho. Puede actualizarlo con el grupo {2} {3} que está asociado al equipo.", regPto.Ptomedicodi, regPto.Ptomedidesc, regGr.Grupocodi, regGr.Gruponomb) };
                                regCDespacho.ListaMensajeValidacion.Add(regMsj);
                            }
                            else
                            {
                                var regMsj = new ResultadoValidacionAplicativo() { TipoValidacion = ConstantesPR5ReportesServicio.MensajeError, Descripcion = string.Format("El equipo {0} {1} no tiene asociado un Grupo despacho.", regEq.Equicodi, regEq.Equinomb) };
                                regCDespacho.ListaMensajeValidacion.Add(regMsj);
                            }
                        }

                        if (regPto.Catecodi>0 && !catecodiGrupos.Contains(regPto.Catecodi))
                        {
                            var regMsj = new ResultadoValidacionAplicativo() { TipoValidacion = ConstantesPR5ReportesServicio.MensajeError, Descripcion = string.Format("El punto de medición {0} {1} no tiene asociado un Grupo despacho sino a otro tipo de grupo (Modo de operación).", regPto.Ptomedicodi, regPto.Ptomedidesc, regPto.Catenomb) };
                            regCDespacho.ListaMensajeValidacion.Add(regMsj);
                        }
                    }
                }
            }

            //Propiedad de los grupos Coes, no Coes para cada día de la data
            regCDespacho.ListaOperacionCoes = FactorySic.GetPrGrupodatRepository().ListarHistoricoValores(ConstantesPR5ReportesServicio.PropGrupoOperacionCoes.ToString(), -1)
                                                        .Where(x => x.Deleted == 0).OrderByDescending(x => x.Fechadat).ToList();

            //asignar propiedades de grupo y equipo al punto
            foreach (var reg48 in listaAllMe48)
            {
                var regPto = listaPto.Find(x => x.Ptomedicodi == reg48.Ptomedicodi);
                if (regPto != null)
                {
                    reg48.Grupocodi = regPto.Grupocodi ?? 0;
                    reg48.Catecodi = regPto.Catecodi;
                    reg48.Equicodi = regPto.Equicodi ?? 0;
                    reg48.Famcodi = regPto.Famcodi;

                    var regGrupo = listaAllGrupo.Find(x => x.Grupocodi == regPto.Grupocodi);
                    if (regGrupo != null)
                    {
                        reg48.Grupointegrante = ReporteMedidoresAppServicio.SetValorCentralIntegrante(regGrupo.Grupocodi, reg48.Medifecha, regGrupo.Grupointegrante, regCDespacho.ListaOperacionCoes);
                        reg48.Grupoabrev = regGrupo.Grupoabrev;
                        reg48.Gruponomb = regGrupo.Gruponomb;
                        reg48.Catecodi = regGrupo.Catecodi;
                    }
                }
                else 
                {
                    var regptoBD = FactorySic.GetMePtomedicionRepository().GetById(reg48.Ptomedicodi);
                    if (regptoBD != null)
                    {
                        string origenDesc = "Mediciones Despacho 1/2 hora";
                        if (origlectcodi != ConstantesAppServicio.OriglectcodiDespachomediahora.ToString()) origenDesc = "PR-05IEOD";

                        if (regptoBD.Origlectcodi.ToString() != origlectcodi)
                        {
                            var regMsjPtoNoUniverso = new ResultadoValidacionAplicativo()
                            {
                                TipoValidacion = ConstantesPR5ReportesServicio.MensajeError,
                                Descripcion = string.Format("El punto de medición {0} {1} no pertenece al módulo {2}. Debe crear un nuevo punto de medición y pasar los datos a ese nuevo punto.", regptoBD.Ptomedicodi, regptoBD.Ptomedidesc, origenDesc)
                            };
                            regCDespacho.ListaMensajeValidacion.Add(regMsjPtoNoUniverso);
                        }

                        //agregar punto que no existe en el aplicativo
                        listaPto.Add(regptoBD);
                    }
                }
            }

            regCDespacho.ListaPtoOrig30min = listaPto.Where(x => x.Emprcodi > 1).ToList();

            regCDespacho.ListaPtoXGrupoDespacho = listaPto.Where(x => catecodiGrupos.Contains(x.Catecodi)).ToList();
            regCDespacho.ListaPtoXGrupoDespachoValido = regCDespacho.ListaPtoXGrupoDespacho.Where(x => listaEstadoValido.Contains(x.Ptomediestado)).ToList();

            UtilCdispatch.LogDiffSegundos(fechaHoraIni, contadorIni, out fechaHoraIni, out contadorIni);

            for (DateTime fecha = fechaIni; fecha <= fechaFin; fecha = fecha.AddDays(1))
            {
                regCDespacho.ListaFecha.Add(fecha);

                CDespachoDiario regCDespachoXDia = new CDespachoDiario();
                regCDespachoXDia.Fecha = fecha;

                //Equipamiento sin eliminados
                regCDespachoXDia.ListaAllEquipo = listaAllEquipo;
                //Grupos sin eliminados
                regCDespachoXDia.ListaAllGrupo = listaAllGrupo;

                //despacho y costo de operación
                regCDespachoXDia.ListaMe48XDia = regCDespacho.ListaAllMe48.Where(x => x.Medifecha == fecha).ToList();
                regCDespachoXDia.ListaMe1XDia = regCDespacho.ListaAllMe1.Where(x => x.Medifecha == fecha).ToList();
                //regCDespachoXDia.ListaCostoXDia = regCDespacho.ListaAllCosto.Where(x => x.Medifecha == fecha).ToList();
                //regCDespachoXDia.ListaPotenciaXDia = regCDespacho.ListaAllPotencia.Where(x => x.Medifecha == fecha).ToList();

                //agregar datos del día al global
                regCDespacho.ListaCDespachoDiario.Add(regCDespachoXDia);
            }
        }

        private void CargarInsumoCalculoCDispatch(CDespachoInput regInput, ref CDespachoGlobal regCDespacho)
        {
            //parámetros de entrada
            int lectcodi = regInput.Lectcodi;
            DateTime fechaIni = regInput.FechaIni;
            DateTime fechaFin = regInput.FechaFin;
            List<MeMedicion48DTO> listaAllMe48 = regInput.ListaAllMe48;
            bool esCalcularRFria = regInput.EsCalcularRFria;
            bool esCalcularCostos = regInput.EsCalcularCostos;

            bool aplicarTTIE = false;
            bool soloDatVigente = true;

            //variables debug
            DateTime fechaHoraIni = DateTime.Now;
            int contadorIni = 1;

            MeLecturaDTO lectura = FactorySic.GetMeLecturaRepository().GetById(lectcodi);
            regCDespacho.Lectnomb = lectura.Lectnomb;
            regCDespacho.Lectcodi = lectcodi;
            regCDespacho.ListaTipoGrupo = FactorySic.GetPrTipogrupoRepository().List().Where(x => x.Tipogrupocodi > 0).OrderBy(x => x.Tipogrupocodi).ToList();

            if (esCalcularRFria || esCalcularCostos)
            {
                #region Insumos

                //Lista de propiedades por equipo
                regCDespacho.ListaPropequi = INDAppServicio.ListarEqPropequiHistoricoDecimalValido("1530,1563,164,319,46,1710,1602,49,197,301,299", fechaIni, soloDatVigente);

                UtilCdispatch.LogDiffSegundos(fechaHoraIni, contadorIni, out fechaHoraIni, out contadorIni);

                // Lista de mantenimientos y restricciones operativas 
                ListarMantenimientosXHorizonte(UtilCdispatch.GetEvenclasecodiByLectcodi(lectcodi), fechaIni, fechaFin.AddDays(1),
                                                    out List<EveManttoDTO> listaManttoGen, out List<EveManttoDTO> listaManttoCamisea);
                regCDespacho.ListaManttoTermico = listaManttoGen;
                regCDespacho.ListaManttoTermicoCamisea = listaManttoCamisea;

                UtilCdispatch.LogDiffSegundos(fechaHoraIni, contadorIni, out fechaHoraIni, out contadorIni);

                //stock de combustible programado
                regCDespacho.ListaStockCombustibleProg = ListarStockProgramado(fechaIni, fechaFin);

                UtilCdispatch.LogDiffSegundos(fechaHoraIni, contadorIni, out fechaHoraIni, out contadorIni);

                // Horas de Operación, Intervenciones, Stock programado
                switch (lectcodi.ToString())
                {
                    case ConstantesAppServicio.LectcodiEjecutadoHisto: //6
                    case ConstantesAppServicio.LectcodiEjecutado: //93
                    case ConstantesAppServicio.LectcodiReprogDiario: //5

                        List<EveHoraoperacionDTO> listaHOP = servHO.ListarHorasOperacionByCriteria(fechaIni.AddDays(-1), fechaFin.AddDays(1), ConstantesHorasOperacion.ParamEmpresaTodos, ConstantesHorasOperacion.ParamCentralTodos, ConstantesHorasOperacion.TipoListadoSoloTermico);

                        UtilCdispatch.LogDiffSegundos(fechaHoraIni, contadorIni, out fechaHoraIni, out contadorIni);

                        listaHOP = servHO.CompletarListaHoraOperacionTermo(listaHOP);

                        regCDespacho.ListaHOHoy = listaHOP;
                        regCDespacho.ListaHOAyer = listaHOP;

                        break;
                    case ConstantesAppServicio.LectcodiProgDiario: //4
                    case ConstantesAppServicio.LectcodiAjusteDiario: //7

                        //modos de operación de Yupana cada media hora
                        var listaHopYup = ListarHOFromYupanaProgramadoDiario(ConstantesCortoPlazo.TopologiaDiario, fechaIni.AddDays(-1), fechaFin, regCDespacho.ListaAllGrupoBD);
                        listaHopYup = servHO.CompletarListaHoraOperacionTermo(listaHopYup);
                        regCDespacho.ListaHOHoy = listaHopYup;

                        //para el programado diario, el día anterior sí hubo ejecutado
                        List<EveHoraoperacionDTO> listaHOPAyer = servHO.ListarHorasOperacionByCriteria(fechaIni.AddDays(-1), fechaFin, ConstantesHorasOperacion.ParamEmpresaTodos, ConstantesHorasOperacion.ParamCentralTodos, ConstantesHorasOperacion.TipoListadoSoloTermico);
                        listaHOPAyer = servHO.CompletarListaHoraOperacionTermo(listaHOPAyer);
                        regCDespacho.ListaHOAyer = listaHOPAyer;

                        break;
                    case ConstantesAppServicio.LectcodiProgSemanal: //3

                        //modos de operación de Yupana cada media hora
                        var listaHopYupSem = ListarHOFromYupanaProgramadoDiario(ConstantesCortoPlazo.TopologiaSemanal, fechaIni.AddDays(-1), fechaFin, regCDespacho.ListaAllGrupoBD);
                        listaHopYup = servHO.CompletarListaHoraOperacionTermo(listaHopYupSem);
                        regCDespacho.ListaHOHoy = listaHopYup;

                        List<EveHoraoperacionDTO> listaHOPAyerSem = servHO.ListarHorasOperacionByCriteria(fechaIni.AddDays(-1), fechaIni, ConstantesHorasOperacion.ParamEmpresaTodos, ConstantesHorasOperacion.ParamCentralTodos, ConstantesHorasOperacion.TipoListadoSoloTermico);
                        listaHOPAyerSem = servHO.CompletarListaHoraOperacionTermo(listaHOPAyerSem);
                        regCDespacho.ListaHOAyer = listaHOPAyerSem;

                        break;
                }

                #endregion
            }

            UtilCdispatch.LogDiffSegundos(fechaHoraIni, contadorIni, out fechaHoraIni, out contadorIni);

            INDAppServicio servInd = new INDAppServicio();
            for (DateTime fecha = fechaIni; fecha <= fechaFin; fecha = fecha.AddDays(1))
            {
                CDespachoDiario regCDespachoXDia = regCDespacho.ListaCDespachoDiario.Find(x => x.Fecha == fecha);
                regCDespachoXDia.ListaTipoGrupo = regCDespacho.ListaTipoGrupo;

                //insumos por dia
                regCDespachoXDia.ListaManttoTermicoXDia = regCDespacho.ListaManttoTermico.Where(x => (x.Evenini >= fecha && x.Evenini <= fecha.AddDays(1))).ToList();
                regCDespachoXDia.ListaManttoTermicoCamiseaXDia = regCDespacho.ListaManttoTermicoCamisea.Where(x => (x.Evenini >= fecha && x.Evenini <= fecha.AddDays(1))).ToList();
                regCDespachoXDia.ListaEquicodiTieneConexionCamisea = regCDespacho.ListaEquicodiTieneConexionCamisea;
                regCDespachoXDia.ListaHOXDiaAyer = regCDespacho.ListaHOAyer.Where(x => x.Hophorini.Value.Date == fecha.AddDays(-1)).ToList();
                if (!regCDespachoXDia.ListaHOXDiaAyer.Any()) regCDespachoXDia.ListaHOXDiaAyer = regCDespacho.ListaHOHoy.Where(x => x.Hophorini.Value.Date == fecha.AddDays(-1)).ToList();
                regCDespachoXDia.ListaHOXDiaHoy = regCDespacho.ListaHOHoy.Where(x => x.Hophorini.Value.Date == fecha).ToList();

                //Unidades térmicas con operación comercial, necesario para Reserva Fría.
                servInd.ListarUnidadTermicoOpComercialCDispatch(fecha, string.Join(",", regCDespacho.ListaCatecodiConData)
                                                , out List<EqEquipoDTO> listaUnidadesTermo, out List<EqEquipoDTO> listaUnidadesEspTermo, out List<EqEquipoDTO> listaEquiposTermicos
                                                , out List<PrGrupoDTO> listaGrupoModo, out List<PrGrupoDTO> listaGrupoDespacho
                                                , out List<ResultadoValidacionAplicativo> listaMsj44, aplicarTTIE);

                UtilCdispatch.LogDiffSegundos(fechaHoraIni, contadorIni, out fechaHoraIni, out contadorIni);

                regCDespachoXDia.ListaUnidadOC = listaUnidadesTermo;
                regCDespachoXDia.ListaUnidadEspOC = listaUnidadesEspTermo;
                regCDespachoXDia.ListaModoOC = listaGrupoModo;
                regCDespachoXDia.ListaDespachoOC = listaGrupoDespacho;

                //Equipos y modos de operación con Operación Comercial
                List<EqEquipoDTO> listaEqGen = listaEquiposTermicos.Where(x => x.Famcodi == ConstantesHorasOperacion.IdGeneradorTemoelectrico).ToList();
                List<EqEquipoDTO> listaEqCentral = listaEquiposTermicos.Where(x => x.Famcodi == ConstantesHorasOperacion.IdTipoTermica).ToList();

                //List<EqEquipoDTO> listaEquiposHidro = servInd.ListarEquipoOpComercial(fecha, fecha, ConstantesHorasOperacion.IdTipoHidraulica, out List<ResultadoValidacionAplicativo> listaMsjEq2, aplicarTTIE, soloDatVigente);
                //List<EqEquipoDTO> listaEquiposSolar = servInd.ListarEquipoOpComercial(fecha, fecha, ConstantesHorasOperacion.IdTipoSolar, out List<ResultadoValidacionAplicativo> listaMsjEq3, aplicarTTIE, soloDatVigente);
                //List<EqEquipoDTO> listaEquiposEolico = servInd.ListarEquipoOpComercial(fecha, fecha, ConstantesHorasOperacion.IdTipoEolica, out List<ResultadoValidacionAplicativo> listaMsjEq4, aplicarTTIE, soloDatVigente);

                regCDespachoXDia.ListaEquipoOC.AddRange(listaEquiposTermicos);
                //regCDespachoXDia.ListaEquipoOC.AddRange(listaEquiposHidro);
                //regCDespachoXDia.ListaEquipoOC.AddRange(listaEquiposSolar);
                //regCDespachoXDia.ListaEquipoOC.AddRange(listaEquiposEolico);

                //tipo de cambio
                regCDespachoXDia.TipoCambio = GetTipoCambio(fechaIni);

                //datos de los modos de operación para Reserva fría y Stock de combustible
                var listaGrupodatCurva = INDAppServicio.ListarPrGrupodatHistoricoDecimalValido(ConstantesIndisponibilidades.ConcepcodisCurvaCombModo, fecha, soloDatVigente);
                listaGrupodatCurva = servInd.ActualizarDatGrupoModoCaracteristicaAdicional(listaGrupodatCurva);

                //obtener curva de consumo de combustible de los modos de operación
                List<ConsumoHorarioCombustible> listaCurvaXDia = INDAppServicio.ListarCurvaConsumoXDia(fecha, listaGrupoModo
                                                                            , new List<EqEquipoDTO>()
                                                                            , listaGrupodatCurva, new List<PrGrupoEquipoValDTO>());
                regCDespachoXDia.ListaCurvaXDia = listaCurvaXDia;

                UtilCdispatch.LogDiffSegundos(fechaHoraIni, contadorIni, out fechaHoraIni, out contadorIni);

                //obtener combustible disponible caso especial
                regCDespachoXDia.ListaDispCombustible = UtilCdispatch.ListarDisponibilidadCombustibleXDia(fecha, regCDespacho.ListaStockCombustibleProg);

                //Cálculo de costos de operación
                if (esCalcularCostos)
                {
                    //obtener costo variables
                    List<PrGrupodatDTO> lParametrosGenerales = FactorySic.GetPrGrupodatRepository().ParametrosGeneralesPorFecha(fecha);
                    List<PrGrupodatDTO> lParametrosFuncionales = FactorySic.GetPrGrupodatRepository().ParametrosPorFecha(fecha); //parametros de todos los modos, grupos termicos y central

                    regCDespachoXDia.ListaCostoVariableDia = UtilCdispatch.ListarCostoVariableEvaluacionFormula(listaGrupoModo, lParametrosGenerales, lParametrosFuncionales);

                    UtilCdispatch.LogDiffSegundos(fechaHoraIni, contadorIni, out fechaHoraIni, out contadorIni);
                }
            }

            //Presentacion web y excel
            regCDespacho.ListaCabeceraCdispatchActual = FillCabeceraCdispatchActual();
            regCDespacho.ListaCabeceraCdispatchHistorico = FillCabeceraCdispatchHistorico();
            regCDespacho.TipoCambioIni = regCDespacho.ListaCDespachoDiario.FirstOrDefault().TipoCambio;

            UtilCdispatch.LogDiffSegundos(fechaHoraIni, contadorIni, out fechaHoraIni, out contadorIni);
        }

        /// <summary>
        /// Reserva fría para el Anexo A
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="empresas"></param>
        /// <param name="reporteRFriaOut"></param>
        /// <param name="regDespachoDiario"></param>
        public void ListarReporteCDispatchAnexoA(int lectcodi, DateTime fechaIni, DateTime fechaFin, string empresas
                                            , out ResultadoCDespacho reporteRFriaOut, out CDespachoDiario regDespachoDiario)
        {
            Load_Dispatch(fechaIni, fechaFin, lectcodi, empresas, true, false, true, new List<MeMedicion48DTO>(), out CDespachoGlobal regDespacho);
            regDespachoDiario = regDespacho.ListaCDespachoDiario.First();
            reporteRFriaOut = regDespacho.ReporteRFria;
        }

        /// <summary>
        /// Retorna el reporte de Reserva Rotante, Reserva Fria, MWxMantto para el numeral 5.10 de Siosein
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="listaDespacho"></param>
        /// <param name="lectcodi"></param>
        /// <param name="reporteRFriaOut"></param>
        public void ListaReporteCDisptachMedicion48(DateTime fechaInicial, DateTime fechaFinal, List<MeMedicion48DTO> listaDespacho, int lectcodi, out ResultadoCDespacho reporteRFriaOut)
        {
            CDespachoGlobal regCDespacho = new CDespachoGlobal();
            regCDespacho.FechaIni = fechaInicial;
            regCDespacho.FechaFin = fechaFinal;
            regCDespacho.ListaAllMe48 = listaDespacho;
            regCDespacho.Lectcodi = lectcodi;

            UtilCdispatch.ListarReporteCDispatch(regCDespacho, out reporteRFriaOut);
        }

        #endregion

        #endregion

        #region anexo a para idcos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListaReqPropios"></param>
        /// <returns></returns>
        public string ListaReqPropiosHtml(List<EveIeodcuadroDTO> ListaReqPropios)
        {

            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='tabla-formulario tabla-adicional' id='tb_info'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>Empresa</th>");
            strHtml.Append("<th>Central</th>");
            strHtml.Append("<th>Unidad</th>");
            strHtml.Append("<th>Inicio</th>");
            strHtml.Append("<th>Final</th>");
            strHtml.Append("<th>Descripcion</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var d in ListaReqPropios)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td>" + d.Emprabrev + "</td>");
                strHtml.Append("<td>" + d.Areanomb + "</td>");
                strHtml.Append("<td>" + d.Equiabrev + "</td>");
                strHtml.Append("<td>" + d.Ichorini.Value.ToString(ConstantesAppServicio.FormatoFecha) + "</td>");
                strHtml.Append("<td>" + d.Ichorfin.Value.ToString(ConstantesAppServicio.FormatoFecha) + "</td>");
                strHtml.Append("<td>" + d.Icdescrip1 + "</td>");
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compen"></param>
        /// <returns></returns>
        public string GenerarHtmlCompensacionPruebasAleatorias(List<EqEquipoDTO> compen, int tipo)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append(this.CabeceraPruebasAleatorias(tipo));

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var list in compen)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td>" + list.EMPRNOMB + "</td>");
                strHtml.Append("<td>" + list.Equinomb + "</td>");
                strHtml.Append("<td>" + list.Equiabrev + "</td>");
                if (tipo == 2) { strHtml.Append("<td><a href='#' onclick='viewResumen(\"" + list.Equicodi + "\");'><img src='/Areas/Migraciones/Content/Images/Visualizar.png' /></a></td>"); }
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public StringBuilder CabeceraPruebasAleatorias(int tipo)
        {
            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<table id='mytable' class='pretty tabla-icono'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>Empresa</th>");
            strHtml.Append("<th>Central</th>");
            strHtml.Append("<th>Unidad de Generacion</th>");
            if (tipo == 2) { strHtml.Append("<th></th>"); }
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            return strHtml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="origlectcodi"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> GetMePtomedicionXEq(string equicodi, string origlectcodi)
        {
            return FactorySic.GetMePtomedicionRepository().GetByCriteria2(equicodi, origlectcodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pruebasUnidad"></param>
        /// <param name="pruebasUnidadDet"></param>
        /// <param name="medidores"></param>
        /// <returns></returns>
        public string GenerarHtmlResumenPruebasAleatorias(List<EqEquipoDTO> pruebasUnidad, List<EvePruebaunidadDTO> pruebasUnidadDet, List<MeMedicion96DTO> medidores)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            #region Datos Generales

            strHtml.Append("<div>");
            strHtml.Append("<fieldset><legend> 1. Datos Generales </legend>");

            strHtml.Append("<table class='pretty tabla-icono'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr><th>Propietario</th><td style='border: 1px solid #dddddd;'>" + pruebasUnidad[0].EMPRNOMB + "</td></tr>");
            strHtml.Append("<tr><th>Central</th><td style='border: 1px solid #dddddd;'>" + pruebasUnidad[0].Equinomb + "</td></tr>");
            strHtml.Append("<tr><th>Unidad</th><td style='border: 1px solid #dddddd;'>" + pruebasUnidad[0].Equiabrev + "</td></tr>");
            strHtml.Append("</thead>");
            strHtml.Append("</table>");

            strHtml.Append("</fieldset></div>");

            #endregion

            #region Resumen de la Prueba Aleatoria

            strHtml.Append("<div style='clear:both; height:15px'></div>");
            strHtml.Append("<div>");
            strHtml.Append("<fieldset><legend>2. Resumen de la Prueba Aleatoria </legend>");

            strHtml.Append("<table id='mytable' class='pretty tabla-icono'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th>Orden de Arranque</th>");
            strHtml.Append("<th>Primer Arranque</th>");
            strHtml.Append("<th>Segundo Arranque</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");
            strHtml.Append("<tr><th style='color:white;border: 1px solid #dddddd;'>Sincronizacion</th><td>" + pruebasUnidadDet[0].Prundhorasincronizacion + "</td><td>" + pruebasUnidadDet[0].Prundhorasincronizacion2 + "</td></tr>");
            strHtml.Append("<tr><th style='color:white;border: 1px solid #dddddd;'>Carga Base</th><td>" + pruebasUnidadDet[0].Prundhorainiplenacarga + "</td><td>" + pruebasUnidadDet[0].Prundhorainiplenacarga2 + "</td></tr>");
            strHtml.Append("<tr><th style='color:white;border: 1px solid #dddddd;'>Orden de Parada</th><td></td><td></td></tr>");
            strHtml.Append("<tr><th style='color:white;border: 1px solid #dddddd;'>Fuera de Sincronismo</th><td></td><td></td></tr>");
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            strHtml.Append("</fieldset></div>");

            #endregion

            #region Medidores

            strHtml.Append("<div style='clear:both; height:15px'></div>");
            strHtml.Append("<div>");
            strHtml.Append("<fieldset><legend>3. Potencia Activa Generada cada 15 minutos </legend>");

            strHtml.Append("<table id='mytable' class='pretty tabla-icono'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th>Periodo</th>");
            strHtml.Append("<th>Potencia (MW)</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");
            if (medidores.Count == 1)
            {
                DateTime fe_ = DateTime.Now.Date;
                foreach (var d in medidores)
                {
                    for (int i = 72; i <= 78; i++)
                    {
                        decimal? val = (decimal?)d.GetType().GetProperty("H" + i).GetValue(d, null);

                        strHtml.Append("<tr>");
                        strHtml.Append("<td>" + fe_.AddMinutes(i * 15).ToString(ConstantesAppServicio.FormatoHora) + "</td>");
                        strHtml.Append("<td>" + (val != null ? (decimal)val : 0).ToString("N", nfi) + "</td>");
                        strHtml.Append("</tr>");
                    }
                }
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            strHtml.Append("</fieldset></div>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        public void GeneraRptRequerimientosPropios(ExcelWorksheet ws, DateTime fecha1, DateTime fecha2)
        {
            var ListaReqPropios = this.GetListaReqPropios(fecha1, fecha2);//09
            int colum_ = 2, row_ = 5, row_2 = 0;

            #region cabecera1
            ws.Cells[row_, colum_++].Value = "Empresa";
            ws.Cells[row_, colum_++].Value = "Central";
            ws.Cells[row_, colum_++].Value = "Unidad";
            ws.Cells[row_, colum_++].Value = "Inicio";
            ws.Cells[row_, colum_++].Value = "Final";
            ws.Cells[row_, colum_++].Value = "Descripcion";

            int sizeFont = 8;
            formatoCabecera(ws, 5, 2, 5, colum_ - 1, sizeFont);
            #endregion

            #region detalle1
            row_ = 6;
            foreach (var d in ListaReqPropios)
            {
                colum_ = 2;
                ws.Cells[row_, colum_++].Value = d.Emprabrev;
                ws.Cells[row_, colum_++].Value = d.Areanomb;
                ws.Cells[row_, colum_++].Value = d.Equiabrev;
                ws.Cells[row_, colum_++].Value = d.Ichorini.Value.ToString(ConstantesAppServicio.FormatoFecha);
                ws.Cells[row_, colum_++].Value = d.Ichorfin.Value.ToString(ConstantesAppServicio.FormatoFecha);
                ws.Cells[row_, colum_++].Value = d.Icdescrip1;
                row_++;
            }

            if (ListaReqPropios.Count > 0)
                using (var range = ws.Cells[6, 2, row_ - 1, colum_ - 1])
                {
                    range.Style.Font.Size = sizeFont;
                }

            AddAutoWidthColumn(ws, 2, colum_ - 1);
            borderCeldas(ws, 5, 2, row_ - 1, colum_ - 1);
            #endregion

            var ListaLogSorteo = this.GetListaLogSorteo(fecha1);//09
            if (ListaLogSorteo.Count > 0)
            {
                var ListaPruebasAleatorias = this.GetListaPruebasAleatorias(fecha1);//09
                if (ListaPruebasAleatorias.Count > 0)
                {
                    colum_ = 2;
                    row_++;

                    #region cabecera2
                    ws.Cells[row_, colum_++].Value = "Empresa";
                    ws.Cells[row_, colum_++].Value = "Central";
                    ws.Cells[row_, colum_++].Value = "Unidad de Generacion";

                    formatoCabecera(ws, row_, 2, row_, colum_ - 1, sizeFont);
                    #endregion

                    #region detalle2
                    row_2 = row_;
                    foreach (var d in ListaPruebasAleatorias)
                    {
                        colum_ = 2;
                        ws.Cells[row_, colum_++].Value = d.EMPRNOMB;
                        ws.Cells[row_, colum_++].Value = d.Equinomb;
                        ws.Cells[row_, colum_++].Value = d.Equiabrev;
                        row_++;
                    }

                    using (var range = ws.Cells[row_2, 2, row_ - 1, colum_ - 1])
                    {
                        range.Style.Font.Size = sizeFont;
                    }

                    AddAutoWidthColumn(ws, 2, colum_ - 1);
                    borderCeldas(ws, row_2, 2, row_ - 1, colum_ - 1);
                    #endregion

                    int TotalSorteo = (new MigracionesAppServicio()).GetTotalConteoTipo(fecha1, "XEQ");//09
                    if (TotalSorteo > 0)
                    {
                        colum_ = 2;
                        ws.Cells[row_, colum_].Value = "Resultado del sorteo: Balota Negra - Día de Prueba...";
                        ws.Cells[row_, colum_].Style.Font.Size = sizeFont;
                        ws.Cells[row_, colum_].Style.Font.Bold = true;

                        row_ += 2;
                        ListaPruebasAleatorias = ListaPruebasAleatorias.Where(x => x.Ecodigo == "S").ToList();
                        if (ListaPruebasAleatorias.Count > 0)
                        {
                            #region cabecera3
                            ws.Cells[row_, colum_++].Value = "Empresa";
                            ws.Cells[row_, colum_++].Value = "Central";
                            ws.Cells[row_, colum_++].Value = "Unidad de Generacion";

                            formatoCabecera(ws, row_, 2, row_, colum_ - 1, sizeFont);
                            #endregion

                            #region detalle3
                            row_++;
                            colum_ = 2;
                            row_2 = row_;
                            foreach (var d in ListaPruebasAleatorias)
                            {
                                colum_ = 2;
                                ws.Cells[row_, colum_++].Value = d.EMPRNOMB;
                                ws.Cells[row_, colum_++].Value = d.Equinomb;
                                ws.Cells[row_, colum_++].Value = d.Equiabrev;
                                row_++;
                            }

                            using (var range = ws.Cells[row_2, 2, row_ - 1, colum_ - 1])
                            {
                                range.Style.Font.Size = sizeFont;
                            }

                            AddAutoWidthColumn(ws, 2, colum_ - 1);
                            borderCeldas(ws, row_2 - 1, 2, row_ - 1, colum_ - 1);
                            #endregion

                            if (ListaPruebasAleatorias.Count > 0)
                            {
                                #region Resumen1
                                row_++;
                                colum_ = 2;
                                ws.Cells[row_, colum_].Value = "Resumen de la Prueba de verificacion de Disponibilidades de Unidades Termicas mediante Pruebas Aleatorias:";
                                ws.Cells[row_, colum_].Style.Font.Bold = true;
                                row_++;
                                ws.Cells[row_, colum_].Value = "1. Datos Generales";
                                ws.Cells[row_, colum_].Style.Font.Size = sizeFont;
                                ws.Cells[row_, colum_].Style.Font.Bold = true;
                                row_++;
                                row_2 = row_;
                                ws.Cells[row_, colum_].Value = "Propietario";
                                formatoCabecera(ws, row_, 2, row_, colum_++, sizeFont);
                                ws.Cells[row_, colum_].Value = ListaPruebasAleatorias[0].EMPRNOMB;
                                ws.Cells[row_++, colum_].Style.Font.Size = sizeFont;

                                colum_ = 2;
                                ws.Cells[row_, colum_].Value = "Central";
                                formatoCabecera(ws, row_, 2, row_, colum_++, sizeFont);
                                ws.Cells[row_, colum_].Value = ListaPruebasAleatorias[0].Equinomb;
                                ws.Cells[row_++, colum_].Style.Font.Size = sizeFont;

                                colum_ = 2;
                                ws.Cells[row_, colum_].Value = "Unidad";
                                formatoCabecera(ws, row_, 2, row_, colum_++, sizeFont);
                                ws.Cells[row_, colum_].Value = ListaPruebasAleatorias[0].Equiabrev;
                                ws.Cells[row_++, colum_].Style.Font.Size = sizeFont;

                                borderCeldas(ws, row_2, 2, row_ - 1, colum_);
                                #endregion

                                var pruebasUnidadDet = (new PruebaunidadAppServicio()).GetByCriteriaEvePruebaunidads(fecha1, fecha2);
                                if (pruebasUnidadDet.Count > 0)
                                {
                                    #region Resumen2
                                    colum_ = 2;
                                    ws.Cells[row_, colum_].Value = "2. Resumen de la Prueba Aleatoria";
                                    ws.Cells[row_, colum_].Style.Font.Size = sizeFont;
                                    ws.Cells[row_, colum_].Style.Font.Bold = true;
                                    row_++;
                                    ws.Cells[row_, colum_++].Value = "Orden de Arranque";
                                    ws.Cells[row_, colum_++].Value = "Primer Arranque";
                                    ws.Cells[row_, colum_++].Value = "Segundo Arranque";

                                    formatoCabecera(ws, row_, 2, row_, colum_ - 1, sizeFont);

                                    ///Inicio Detalle
                                    row_++;
                                    colum_ = 2;
                                    row_2 = row_;
                                    ws.Cells[row_, colum_].Value = "Sincronizacion";
                                    formatoCabecera(ws, row_, 2, row_, colum_++, sizeFont);
                                    ws.Cells[row_, colum_].Value = pruebasUnidadDet[0].Prundhorasincronizacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull);
                                    ws.Cells[row_++, colum_].Style.Font.Size = sizeFont;

                                    colum_ = 2;
                                    ws.Cells[row_, colum_].Value = "Carga Base";
                                    formatoCabecera(ws, row_, 2, row_, colum_++, sizeFont);
                                    ws.Cells[row_, colum_].Value = pruebasUnidadDet[0].Prundhorainiplenacarga.Value.ToString(ConstantesAppServicio.FormatoFechaFull);
                                    ws.Cells[row_++, colum_].Style.Font.Size = sizeFont;

                                    colum_ = 2;
                                    ws.Cells[row_, colum_].Value = "Orden de Parada";
                                    formatoCabecera(ws, row_, 2, row_, colum_++, sizeFont);
                                    ws.Cells[row_, colum_].Value = "";
                                    ws.Cells[row_++, colum_].Style.Font.Size = sizeFont;

                                    colum_ = 2;
                                    ws.Cells[row_, colum_].Value = "Fuera de Sincronismo";
                                    formatoCabecera(ws, row_, 2, row_, colum_++, sizeFont);
                                    ws.Cells[row_, colum_].Value = "";
                                    ws.Cells[row_++, colum_].Style.Font.Size = sizeFont;
                                    ///Fin Detalle

                                    borderCeldas(ws, row_2 - 1, 2, row_ - 1, colum_ + 1);

                                    #endregion
                                }

                                var objPtomedicion = (new MigracionesAppServicio()).GetMePtomedicionXEq(ListaPruebasAleatorias[0].Equicodi.ToString(), ConstantesAppServicio.OriglectcodiMedidoresGene.ToString());
                                if (objPtomedicion.Count > 0)
                                {
                                    #region Resumen3
                                    colum_ = 2;
                                    ws.Cells[row_, colum_].Value = "3. Potencia Activa Generada cada 15 minutos";
                                    ws.Cells[row_, colum_].Style.Font.Size = sizeFont;
                                    ws.Cells[row_, colum_].Style.Font.Bold = true;
                                    row_++;
                                    ws.Cells[row_, colum_++].Value = "Periodo";
                                    ws.Cells[row_, colum_++].Value = "Potencia (MW)";

                                    formatoCabecera(ws, row_, 2, row_, colum_ - 1, sizeFont);
                                    #endregion

                                    var medidores = (new PruebaunidadAppServicio()).GetByCriteriaMedicion96(ConstantesAppServicio.TipoinfocodiMW, objPtomedicion[0].Ptomedicodi, ConstantesAppServicio.LectcodiMedidoresGene, fecha1, fecha2);
                                    if (medidores.Count > 0)
                                    {
                                        #region Resumen3 Detalle
                                        row_++;
                                        row_2 = row_;
                                        DateTime fe_ = DateTime.Now.Date;
                                        foreach (var d in medidores)
                                        {
                                            for (int i = 72; i <= 78; i++)
                                            {
                                                decimal? val = (decimal?)d.GetType().GetProperty("H" + i).GetValue(d, null);

                                                colum_ = 2;
                                                ws.Cells[row_, colum_++].Value = fe_.AddMinutes(i * 15).ToString(ConstantesAppServicio.FormatoHora);
                                                ws.Cells[row_, colum_].Value = (val != null ? (decimal)val : 0);
                                                ws.Cells[row_, colum_++].Style.Numberformat.Format = "#,##0.000";
                                                row_++;
                                            }
                                        }

                                        using (var range = ws.Cells[row_2, 2, row_ - 1, colum_ - 1])
                                        {
                                            range.Style.Font.Size = sizeFont;
                                        }

                                        //AddAutoWidthColumn(ws, 2, colum_ - 1);
                                        borderCeldas(ws, row_2 - 1, 2, row_ - 1, colum_ - 1);
                                        #endregion
                                    }
                                }
                            }
                        }
                        else { }
                    }
                    else { }
                }
                else { }
            }
            else { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        public void GeneraRptRequerimientosQuemaGasNoEmpleado(ExcelWorksheet ws, DateTime fecha1, DateTime fecha2)
        {
            var servicioStockComb = new StockCombustiblesAppServicio();
            var listaReporte = servicioStockComb.ListaMedxIntervQuema(ConstantesStockCombustibles.LectCodiQuemaGas, ConstantesStockCombustibles.Origlectcodi, "-1", fecha1, fecha2.AddDays(1), "-1");

            int nfil = listaReporte.Count;
            int xFil = 6;
            int ncol = 7;
            using (ExcelRange r = ws.Cells[xFil, 2, xFil, 8])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }

            ws.Cells[xFil, 2].Value = "FECHA";
            ws.Cells[xFil, 3].Value = "EMPRESA";
            ws.Cells[xFil, 4].Value = "CENTRAL";
            ws.Cells[xFil, 5].Value = "TIPO COMBUSTIBLE";
            ws.Cells[xFil, 6].Value = "INICIO";
            ws.Cells[xFil, 7].Value = "FINAL";
            ws.Cells[xFil, 8].Value = "VOLUMEN DE GAS (Mm3)";

            ws.Column(1).Width = 5;
            ws.Column(2).Width = 15;
            ws.Column(3).Width = 35;
            ws.Column(4).Width = 35;
            ws.Column(5).Width = 20;
            ws.Column(6).Width = 10;
            ws.Column(7).Width = 10;
            ws.Column(8).Width = 20;
            //****************************************
            var i = 1;
            foreach (var reg in listaReporte)
            {

                ws.Cells[xFil + i, 2].Value = reg.Medintfechaini.ToString(ConstantesAppServicio.FormatoFecha);

                ws.Cells[xFil + i, 3].Value = reg.Emprnomb;

                ws.Cells[xFil + i, 4].Value = reg.Equinomb;
                ws.Cells[xFil + i, 5].Value = reg.Tipoptomedinomb;

                ws.Cells[xFil + i, 6].Value = reg.Medintfechaini.ToString(ConstantesAppServicio.FormatoOnlyHora);

                ws.Cells[xFil + i, 7].Value = reg.Medintfechafin.ToString(ConstantesAppServicio.FormatoOnlyHora);
                decimal? valor = 0;

                if (reg.Medinth1 != null)
                    valor = reg.Medinth1;
                ws.Cells[xFil + i, 8].Value = valor;
                i++;
            }
            ////////////// Formato de Celdas Valores
            using (var range = ws.Cells[6, 8, 6 + nfil, 8])
            {
                range.Style.Numberformat.Format = @"0.000";
            }

            var fontTabla = ws.Cells[6, 2, 6 + nfil, 1 + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";

            var border = ws.Cells[6, 2, 6 + nfil, 1 + ncol].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        public void GeneraRptDisponibilidadGas(ExcelWorksheet ws, DateTime fecha1, DateTime fecha2)
        {
            var servicioStockComb = new StockCombustiblesAppServicio();
            var listaReporte = servicioStockComb.
                ListaMedxIntervDisponibilidad(ConstantesStockCombustibles.LectCodiDisponibilidad, ConstantesStockCombustibles.Origlectcodi, "-1", fecha1, fecha2, "-1", ConstantesAppServicio.YacimientoGasCodi, "-1");

            int nfil = listaReporte.Count + 1;
            int xFil = 8;
            int ncol = 10;

            ws.Cells[3, 3].Value = fecha1.ToString(ConstantesAppServicio.FormatoFecha);
            ws.Cells[4, 3].Value = fecha2.ToString(ConstantesAppServicio.FormatoFecha);
            var i = 0;

            ws.Cells[xFil - 1, 2].Value = "FECHA";
            ws.Cells[xFil - 1, 3].Value = "EMPRESA";
            ws.Cells[xFil - 1, 4].Value = "GASEODUCTO";
            ws.Cells[xFil - 1, 5].Value = "VOLUMEN DE GAS (Mm3)";
            ws.Cells[xFil - 1, 6].Value = "INICIO";
            ws.Cells[xFil - 1, 7].Value = "FINAL";
            ws.Cells[xFil - 1, 8].Value = "FECHA HORA ENVIO";
            ws.Cells[xFil - 1, 9].Value = "ESTADO";
            ws.Cells[xFil - 1, 10].Value = "OBSERVACIONES";
            ws.Cells[xFil - 1, 11].Value = "USUARIO";

            ws.Column(1).Width = 5;
            ws.Column(2).Width = 15;
            ws.Column(3).Width = 35;
            ws.Column(4).Width = 35;
            ws.Column(5).Width = 20;
            ws.Column(6).Width = 10;
            ws.Column(7).Width = 10;
            ws.Column(8).Width = 20;
            ws.Column(9).Width = 20;
            ws.Column(10).Width = 20;
            ws.Column(11).Width = 20;



            foreach (var reg in listaReporte)
            {

                ws.Cells[xFil + i, 2].Value = reg.Medintfechaini.ToString(ConstantesAppServicio.FormatoFecha);
                ws.Cells[xFil + i, 2].StyleID = ws.Cells[8, 2].StyleID;
                ws.Cells[xFil + i, 3].Value = reg.Emprnomb;
                ws.Cells[xFil + i, 3].StyleID = ws.Cells[8, 3].StyleID;
                ws.Cells[xFil + i, 4].Value = reg.Equinomb;
                ws.Cells[xFil + i, 4].StyleID = ws.Cells[8, 4].StyleID;
                //************Datos para el PDO *****************
                decimal? valor = 0;
                if (reg.Medinth1 != null)
                    valor = (decimal?)reg.Medinth1;
                ws.Cells[xFil + i, 5].Value = valor;
                ws.Cells[xFil + i, 5].StyleID = ws.Cells[8, 5].StyleID;
                ws.Cells[xFil + i, 6].Value = reg.Medintfechaini.ToString(ConstantesAppServicio.FormatoFecha) + " 6:00";
                ws.Cells[xFil + i, 6].StyleID = ws.Cells[8, 6].StyleID;
                ws.Cells[xFil + i, 7].Value = reg.Medintfechaini.AddDays(1).ToString(ConstantesAppServicio.FormatoFecha) + " 6:00";
                ws.Cells[xFil + i, 7].StyleID = ws.Cells[8, 7].StyleID;

                ws.Cells[xFil + i, 8].Value = reg.Medintfechafin.ToString(ConstantesAppServicio.FormatoFechaHora);
                ws.Cells[xFil + i, 8].StyleID = ws.Cells[8, 8].StyleID;

                string estado = "";
                if (reg.Medestcodi == 1) { estado = "Declaró"; }
                if (reg.Medestcodi == 2) { estado = "Renominó"; }

                ws.Cells[xFil + i, 9].Value = estado;
                ws.Cells[xFil + i, 9].StyleID = ws.Cells[8, 9].StyleID;

                ws.Cells[xFil + i, 10].Value = reg.Medintdescrip;
                ws.Cells[xFil + i, 10].StyleID = ws.Cells[8, 10].StyleID;

                ws.Cells[xFil + i, 11].Value = reg.Medintusumodificacion;
                ws.Cells[xFil + i, 11].StyleID = ws.Cells[8, 11].StyleID;

                //************DAtos tiempo Real *****************


                /*   ws.Cells[xFil + i, 8].Value = valor;
                   ws.Cells[xFil + i, 8].StyleID = ws.Cells[8, 8].StyleID;
                   ws.Cells[xFil + i, 9].Value = reg.Medintfechaini.ToString(ConstantesAppServicio.FormatoOnlyHora);
                   ws.Cells[xFil + i, 9].StyleID = ws.Cells[8, 9].StyleID;
                   ws.Cells[xFil + i, 10].Value = reg.Medintfechafin.ToString(ConstantesAppServicio.FormatoOnlyHora);
                   ws.Cells[xFil + i, 10].StyleID = ws.Cells[8, 10].StyleID;
                   */
                //*******************
                i++;
            }
            ////////////////
            using (ExcelRange r = ws.Cells[xFil - 1, 2, xFil - 1, 11])
            {
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }



            ////////////// Formato de Celdas Valores
            using (var range = ws.Cells[7, 5, 7 + nfil - 1, ncol - 1])
            {
                range.Style.Numberformat.Format = @"0.000";
            }

            var fontTabla = ws.Cells[7, 2, 7 + nfil, 1 + ncol].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";

            var border = ws.Cells[7, 2, 7 + nfil - 1, 1 + ncol].Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        }

        /// <summary>
        /// GeneraRptRestriccionSuministros
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="nombreReporte"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        public void GeneraRptRestriccionSuministros(ExcelWorksheet ws, String nombreReporte, DateTime fecha1, DateTime fecha2)
        {
            var servicioEvento = new EventosAppServicio();
            PR5ReportesAppServicio servPR5 = new PR5ReportesAppServicio();
            servPR5.ReporteEventosDataReporte(fecha1, fecha2, "SI", ConstantesAppServicio.ParametroDefecto, out List<EventoDTO> data, out List<EveInterrupcionDTO> listaInterrup, out List<EqEquipoDTO> listaEq);
            data = data.Where(x => x.Interrmanualr == "S" && x.Interrracmf == "S").ToList();

            int row = 4;
            #region cabecera
            int rowIniNombreReporte = row;
            int colIniNombreReporte = 5;
            int rowIniEmpresa = rowIniNombreReporte + 1;
            int colIniEmpresa = colIniNombreReporte;
            int colIniUbicacion = colIniEmpresa + 1;
            int colIniEquipo = colIniUbicacion + 1;
            int colIniInicio = colIniEquipo + 1;
            int colIniFinal = colIniInicio + 1;
            int colIniDescripcion = colIniFinal + 1;
            int colIniObs = colIniDescripcion + 1;

            ws.Cells[rowIniEmpresa, colIniEmpresa].Value = "EMPRESA";
            ws.Cells[rowIniEmpresa, colIniUbicacion].Value = "UBICACIÓN";
            ws.Cells[rowIniEmpresa, colIniEquipo].Value = "EQUIPO";
            ws.Cells[rowIniEmpresa, colIniInicio].Value = "INICIO";
            ws.Cells[rowIniEmpresa, colIniFinal].Value = "FINAL";
            ws.Cells[rowIniEmpresa, colIniDescripcion].Value = "DESCRIPCIÓN";
            ws.Cells[rowIniEmpresa, colIniObs].Value = "OBSERVACIÓN";

            //Nombre Reporte
            int colFinNombreReporte = colIniObs;
            ws.Cells[rowIniNombreReporte, colIniNombreReporte].Value = nombreReporte;
            ws.Cells[rowIniNombreReporte, colIniNombreReporte].Style.Font.Size = 18;
            ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte].Merge = true;
            ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte].Style.WrapText = true;
            ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            var colorBorder = Color.White;
            var classTipoEmpresa = "#538DD5";
            using (var range = ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniEmpresa, colFinNombreReporte])
            {
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Font.Bold = true;
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colorBorder);
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Top.Color.SetColor(colorBorder);
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Color.SetColor(colorBorder);
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Color.SetColor(colorBorder);
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Color.SetColor(colorBorder);
                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(classTipoEmpresa));
                range.Style.Font.Color.SetColor(Color.White);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
            ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells[rowIniNombreReporte, colIniNombreReporte, rowIniNombreReporte, colFinNombreReporte].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            #endregion

            row = rowIniEmpresa + 1;
            #region cuerpo

            ws.Row(rowIniNombreReporte).Height = 30;
            ws.Row(rowIniEmpresa).Height = 20;
            ws.Column(colIniEmpresa).Width = 30;
            ws.Column(colIniUbicacion).Width = 35;
            ws.Column(colIniEquipo).Width = 25;
            ws.Column(colIniInicio).Width = 15;
            ws.Column(colIniFinal).Width = 15;
            ws.Column(colIniDescripcion).Width = 100;
            ws.Column(colIniObs).Width = 100;

            if (data.Count > 0)
            {
                int rowIniData = row;
                int rowFinData = row;
                foreach (var list in data)
                {
                    ws.Cells[row, colIniEmpresa].Value = list.EMPRNOMB;
                    ws.Cells[row, colIniUbicacion].Value = list.AREADESC.Trim();
                    ws.Cells[row, colIniEquipo].Value = list.EQUIABREV;
                    ws.Cells[row, colIniInicio].Value = list.EVENINI.Value.ToString(ConstantesBase.FormatoFechaHora);
                    ws.Cells[row, colIniFinal].Value = list.EVENFIN.Value.ToString(ConstantesBase.FormatoFechaHora);
                    ws.Cells[row, colIniDescripcion].Value = list.EVENASUNTO;
                    ws.Cells[row, colIniObs].Value = servicioEvento.GetByIdEveEvento((int)list.EVENCODI).Evencomentarios;

                    rowFinData = row;
                    row++;
                }

                var colorCeldaDatos = ColorTranslator.FromHtml("#1F497D");
                using (var range = ws.Cells[rowIniData, colIniNombreReporte, rowFinData, colFinNombreReporte])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Font.Size = 8;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.WrapText = true;

                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(colorCeldaDatos);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(colorCeldaDatos);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(colorCeldaDatos);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(colorCeldaDatos);
                }
            }
            #endregion

            ws.View.FreezePanes(rowIniEmpresa + 1, 1);
            ws.View.ZoomScale = 100;

        }

        #endregion

        #region Reporte Demanda en Barra

        /// <summary>
        /// ListarEmpresasDemandaBarrra
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasDemandaBarrra(int idTipoEmpresa)
        {
            return FactorySic.GetSiEmpresaRepository().ListEmprResponsables(idTipoEmpresa.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> GetListaPuntoMedicionPorEmpresa(string emprcodi, DateTime fechaPeriodoIni, DateTime fechaPeriodoFin)
        {
            return FactorySic.GetMePtomedicionRepository().GetListaPuntoMedicionPorEmpresa(emprcodi, fechaPeriodoIni, fechaPeriodoFin);
        }

        /// <summary>
        /// Lista de puntos de medicion de Demanda en barra
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> GetListaPuntoMedicionDemandaBarra(string emprcodi, DateTime fechaPeriodoIni, DateTime fechaPeriodoFin)
        {
            string empresas = string.IsNullOrEmpty(emprcodi) ? ConstantesAppServicio.ParametroDefecto : emprcodi;
            int[] listaEmpresas = empresas.Split(',').Select(x => int.Parse(x)).ToArray();

            var listaPuntosMedicion = this.GetListaPuntoMedicionPorEmpresa(ConstantesAppServicio.ParametroDefecto, fechaPeriodoIni, fechaPeriodoFin).Where(x => x.Origlectcodi == ConstantesAppServicio.OriglectcodiDemandaBarra).ToList();
            listaPuntosMedicion = listaPuntosMedicion.Where(x => listaEmpresas.Contains(x.Emprcodi.Value)).ToList();
            listaPuntosMedicion = listaPuntosMedicion.Where(x => x.Ptomedicodi > 20000).OrderBy(x => x.Emprcodi).ThenBy(x => x.Ptomedicodi).ToList();

            return listaPuntosMedicion;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fecInicio"></param>
        /// <param name="fecFin"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetListaDemandaBarras(string ptomedicodi, string lectcodi, DateTime fecInicio, DateTime fecFin)
        {
            return FactorySic.GetMeMedicion48Repository().GetListaDemandaBarras(ptomedicodi, lectcodi, fecInicio, fecFin);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listaPuntosMedicion"></param>
        /// <param name="lista"></param>
        /// <returns></returns>
        public string ReporteDemadaBarrasHtml(List<MePtomedicionDTO> listaPuntosMedicion, List<MeMedicion48DTO> lista)
        {
            listaPuntosMedicion = this.ListarPuntosDemandaBarraSegunDataReporte(listaPuntosMedicion, lista);

            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            var listafechas = lista.GroupBy(x => x.Medifecha).Select(x => x.Key).Distinct().ToList();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='pretty tabla-icono' id='tblDemandaBarras'>");

            var builderEquicodi = new StringBuilder();
            var builderPtomedicodi = new StringBuilder();
            var builderPtomedielenomb = new StringBuilder();
            var builderEmprnomb = new StringBuilder();
            var builderAreanomb = new StringBuilder();
            var builderEquitension = new StringBuilder();
            var builderAreaOperativa = new StringBuilder();



            foreach (var dto in listaPuntosMedicion)
            {
                builderEquicodi.Append(string.Concat("<th>", dto.Equicodi, "</th>"));
                builderPtomedicodi.Append(string.Concat("<th>", dto.Ptomedicodi, "</th>"));
                builderPtomedielenomb.Append(string.Concat("<th>", dto.Ptomedielenomb, "</th>"));
                builderEmprnomb.Append(string.Concat("<th>", dto.Emprnomb, "</th>"));
                builderAreanomb.Append(string.Concat("<th>", dto.Areanomb, "</th>"));
                builderEquitension.Append(string.Concat("<th>", dto.Equitension, "</th>"));
                builderAreaOperativa.Append(string.Concat("<th>", dto.AreaOperativa, "</th>"));
            }

            strHtml.Append("<thead>");
            strHtml.Append("<tr><th>CODIGO EQUIPO</th>" + builderEquicodi + "</tr>");
            strHtml.Append("<tr><th>PTO. MEDICION</th>" + builderPtomedicodi + "</tr>");
            strHtml.Append("<tr><th>NOMBRE</th>" + builderPtomedielenomb + "</tr>");
            strHtml.Append("<tr><th>EMPRESA</th>" + builderEmprnomb + "</tr>");
            strHtml.Append("<tr><th>SUBESTACION</th>" + builderAreanomb + "</tr>");
            strHtml.Append("<tr><th>TENSION (KV)</th>" + builderEquitension + "</tr>");
            strHtml.Append("<tr><th>AREA OPERATIVA</th>" + builderAreaOperativa + "</tr>");
            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var item in listafechas)
            {
                for (int i = 1; i <= 48; i++)
                {
                    strHtml.Append("<tr>");


                    var fecha = item.AddMinutes(i * 30);

                    strHtml.Append("<td>" + fecha.ToString(ConstantesAppServicio.FormatoFechaHora) + "</td>");

                    foreach (var cab in listaPuntosMedicion)
                    {
                        var d = lista.Find(x => x.Medifecha == item && x.Ptomedicodi == cab.Ptomedicodi && x.Emprcodi == cab.Emprcodi);
                        if (d != null)
                        {
                            var val = (decimal?)d.GetType().GetProperty("H" + i).GetValue(d, null);
                            strHtml.Append("<td>" + (val ?? 0).ToString("N", nfi) + "</td>");
                        }
                        else { strHtml.Append("<td></td>"); }
                    }
                    strHtml.Append("</tr>");
                }


            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// Listar puntos de medicion para reporte
        /// </summary>
        /// <param name="listaPuntosMedicion"></param>
        /// <param name="lista"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListarPuntosDemandaBarraSegunDataReporte(List<MePtomedicionDTO> listaPuntosMedicion, List<MeMedicion48DTO> lista)
        {
            List<MePtomedicionDTO> puntosFinal = new List<MePtomedicionDTO>();

            List<int> listaPtomedicodiData = lista.Select(x => x.Ptomedicodi).ToList();

            puntosFinal = listaPuntosMedicion.Where(x => listaPtomedicodiData.Contains(x.Ptomedicodi) || x.Ptomediestado == ConstantesAppServicio.Activo).ToList();
            puntosFinal = puntosFinal.OrderBy(x => x.Emprcodi).ThenBy(x => x.Ptomedicodi).ToList();

            return puntosFinal;
        }

        /// <summary>
        /// Nombre de archivo de Demanda Barra
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="emprcodi"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string NombreArchivoReporteDemandaBarra(int idTipoEmpresa, string emprcodi, int lectcodi, DateTime fechaInicio, DateTime fechaFin)
        {
            string nombreFile = string.Empty;

            string empresas = string.IsNullOrEmpty(emprcodi) ? ConstantesAppServicio.ParametroDefecto : emprcodi;
            int[] listaEmpresas = empresas.Split(',').Select(x => int.Parse(x)).ToArray();

            string nombreEmpresa = string.Empty;
            if (listaEmpresas.Length == 1)
            {
                SiEmpresaDTO emp = (new IEODAppServicio()).GetByIdEmpresa(listaEmpresas[0]);
                nombreEmpresa = emp != null && emp.Emprnomb != null ? emp.Emprnomb.Replace(" ", "_") : string.Empty;
            }
            else
            {
                switch (idTipoEmpresa)
                {
                    case 1:
                        nombreEmpresa = "ET";
                        break;
                    case 2:
                        nombreEmpresa = "ED";
                        break;
                    case 3:
                        nombreEmpresa = "EG";
                        break;
                    case 4:
                        nombreEmpresa = "UL";
                        break;
                    default:
                        nombreEmpresa = " ";
                        break;
                }
            }

            string nombreLectura = string.Empty;
            switch (lectcodi)
            {
                case 45:
                    nombreLectura = "HISTORICO-DIARIO";
                    break;
                case 46:
                    nombreLectura = "PREVISTO-DIARIO"; break;
                case 47:
                    nombreLectura = "PREVISTO-SEMANAL"; break;
            }

            nombreFile = nombreLectura + "-" + nombreEmpresa + "-" + fechaInicio.Date.ToString(ConstantesAppServicio.FormatoFechaDMY) + "-" + fechaFin.Date.ToString(ConstantesAppServicio.FormatoFechaDMY);

            return nombreFile.Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listaPuntosMedicion"></param>
        /// <param name="listaDemandaBarras"></param>
        /// <param name="rutaFile"></param>
        /// <param name="fecInicio"></param>
        /// <param name="fecFin"></param>
        public void GenerarArchivoExcelDemandaBarras(List<MePtomedicionDTO> listaPuntosMedicion, List<MeMedicion48DTO> listaDemandaBarras, string rutaFile, string nombreHoja, DateTime fecInicio, DateTime fecFin)
        {
            var newFile = new FileInfo(rutaFile);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (var xlPackage = new ExcelPackage(newFile))
            {
                nombreHoja = nombreHoja.Length > 31 ? nombreHoja.Substring(0, 31) : nombreHoja;
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHoja);
                ws = xlPackage.Workbook.Worksheets[nombreHoja];
                ws.View.ShowGridLines = false;

                var column = 1;
                this.GeneraRptDemandaBarra(ws, column, listaPuntosMedicion, listaDemandaBarras, fecInicio, fecFin);

                xlPackage.Save();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="ncol"></param>
        /// <param name="listaPuntosMedicion"></param>
        /// <param name="listaDemandaBarras"></param>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        private void GeneraRptDemandaBarra(ExcelWorksheet ws, int ncol, List<MePtomedicionDTO> listaPuntosMedicion, List<MeMedicion48DTO> listaDemandaBarras, DateTime fecIni, DateTime fecFin)
        {
            listaPuntosMedicion = this.ListarPuntosDemandaBarraSegunDataReporte(listaPuntosMedicion, listaDemandaBarras);

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//
            int sizeFont = 8;

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            int colInit = ncol;
            int rowIni = 1;

            ws.Cells[rowIni++, colInit].Value = "CODIGO EQUIPO";
            ws.Cells[rowIni++, colInit].Value = "PTO. MEDICION";
            ws.Cells[rowIni++, colInit].Value = "NOMBRE";
            ws.Cells[rowIni++, colInit].Value = "EMPRESA";
            ws.Cells[rowIni++, colInit].Value = "SUBESTACION";
            ws.Cells[rowIni++, colInit].Value = "TENSION (KV)";
            ws.Cells[rowIni++, colInit].Value = "AREA OPERATIVA";

            colInit++;
            foreach (var dto in listaPuntosMedicion)
            {
                rowIni = 1;

                ws.Cells[rowIni++, colInit].Value = dto.Equicodi;
                ws.Cells[rowIni++, colInit].Value = dto.Ptomedicodi;
                ws.Cells[rowIni++, colInit].Value = dto.Ptomedielenomb;
                ws.Cells[rowIni++, colInit].Value = dto.Emprnomb;
                ws.Cells[rowIni++, colInit].Value = dto.Areanomb;
                ws.Cells[rowIni++, colInit].Value = dto.Equitension;
                ws.Cells[rowIni++, colInit].Value = dto.AreaOperativa;
                colInit++;
            }

            this.formatoCabecera(ws, 1, 1, rowIni - 1, colInit - 1, sizeFont);
            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//

            var listafechas = listaDemandaBarras.GroupBy(x => x.Medifecha).Select(x => x.Key).Distinct().ToList();

            var row = rowIni++;

            var col = 0;
            foreach (var item in listafechas)
            {


                for (int i = 1; i <= 48; i++)
                {
                    col = ncol;
                    var fecha = item.AddMinutes(i * 30);
                    ws.Cells[row, col++].Value = fecha.ToString(ConstantesAppServicio.FormatoFechaHora);

                    foreach (var cab in listaPuntosMedicion)
                    {
                        var CabDet = listaDemandaBarras.Find(x => x.Ptomedicodi == cab.Ptomedicodi && x.Emprcodi == cab.Emprcodi);
                        if (CabDet != null)
                        {
                            var detalle = listaDemandaBarras.Where(x => x.Medifecha == item && x.Emprcodi == cab.Emprcodi && x.Ptomedicodi == cab.Ptomedicodi).ToList();
                            if (detalle.Any())
                            {

                                foreach (var d in detalle)
                                {
                                    var val = (decimal?)d.GetType().GetProperty("H" + i).GetValue(d, null);
                                    if (val != null) ws.Cells[row, col].Value = val;
                                }
                            }
                        }
                        else
                        {
                            ws.Cells[row, col].Value = 0;
                        }
                        ws.Cells[row, col++].Style.Numberformat.Format = "#,##0.000";
                    }

                    row++;
                }

            }

            #endregion

            if (listafechas.Count > 0)
            {
                using (var range = ws.Cells[8, ncol, 9 + 48 * listafechas.Count, col - 1])
                {
                    range.Style.Font.Size = sizeFont;
                }

                ws.Column(1).Width = 15;
                ws.Column(2).Width = 10;
                this.AddAutoWidthColumn(ws, 4, col - 1);
                this.borderCeldas(ws, 1, ncol, 8 + 48 * listafechas.Count - 1, col - 1);
            }
        }

        #endregion

        #region Reporte producc programacion despacho maxima demanda

        /// <summary>
        /// 
        /// </summary>
        /// <param name="umbrales"></param>
        /// <param name="listCuadros"></param>
        /// <returns></returns>
        public string ReporteDespachoHtml(MedicionReporteDTO umbrales, List<MedicionReporteDTO> listCuadros)
        {
            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<div class='leyenda-reporte' style='float:right; width: auto'>");
            strHtml.Append("<strong>Máxima Demanda:</strong> el " + umbrales.FechaMaximaDemanda.ToString(ConstantesAppServicio.FormatoFecha) + " a las " + umbrales.MaximaDemandaHora.ToString(ConstantesAppServicio.FormatoOnlyHora) + " h" + "<br />");
            strHtml.Append("</div>");

            strHtml.Append("<div style='clear:both'></div>");

            strHtml.Append("<table class='tabla-formulario tabla-adicional' id='tbRecurso'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>N°</th>");
            strHtml.Append("<th>Empresa</th>");
            strHtml.Append("<th>Central</th>");
            strHtml.Append("<th>Unidad</th>");
            strHtml.Append("<th>E. Hidraulica (MWh)</th>");
            strHtml.Append("<th>E. Termica (MWh)</th>");
            strHtml.Append("<th>Total Empresa</th>");
            strHtml.Append("<th>Máxima Demanda (MW)</th>");
            //strHtml.Append("<th>Mínima Demanda (MW)</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            decimal totalH = 0, totalT = 0;
            foreach (var item in listCuadros)
            {
                if (!item.IndicadorTotal && !item.IndicadorTotalGeneral)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append("<td>" + item.NroItem + "</td>");
                    strHtml.Append("<td>" + item.Emprnomb + "</td>");
                    strHtml.Append("<td>" + item.Central + "</td>");
                    strHtml.Append("<td>" + item.Unidad + "</td>");
                    if (item.Tgenercodi == 1) { strHtml.Append("<td style='text-align:right'>" + item.Total.ToString("N", nfi) + "</td><td></td>"); totalH += item.Total; }
                    else { strHtml.Append("<td></td><td style='text-align:right'>" + item.Total.ToString("N", nfi) + "</td>"); totalT += item.Total; }
                    //strHtml.Append("<td>" + item.Fenergnomb + "</td>");
                    strHtml.Append("<td style='text-align:right'>" + item.Total.ToString("N", nfi) + "</td>");
                    strHtml.Append("<td style='text-align:right'>" + item.MaximaDemanda.ToString("N", nfi) + "</td>");
                    //strHtml.Append("<td style='text-align:right'>" + item.MinimaDemanda.ToString("N", nfi) + "</td>");
                    strHtml.Append("</tr>");
                }
                else
                {
                    if (item.IndicadorTotal)
                    {
                        strHtml.Append("<tr class='table-subtotal'>");
                        strHtml.Append("<td colspan='4' style='text-align:right'>TOTAL " + item.Emprnomb + "</td>");
                        strHtml.Append("<td style='text-align:right' title='E. Hidraulica (MWh)'>" + totalH.ToString("N", nfi) + "</td>");
                        strHtml.Append("<td style='text-align:right' title='E. Termica (MWh)'>" + totalT.ToString("N", nfi) + "</td>");
                        strHtml.Append("<td style='text-align:right' title='Total Empresa'>" + item.Total.ToString("N", nfi) + "</td>");
                        strHtml.Append("<td style='text-align:right' title='Máxima Demanda (MW)'>" + item.MaximaDemanda.ToString("N", nfi) + "</td>");
                        //strHtml.Append("<td style='text-align:right' title='Mínima Demanda (MW)'>" + item.MinimaDemanda.ToString("N", nfi) + "</td>");
                        strHtml.Append("</tr>");
                        totalH = 0;
                        totalT = 0;
                    }
                    if (item.IndicadorTotalGeneral)
                    {
                        strHtml.Append("<tr class='table-total'>");
                        strHtml.Append("<td colspan='6' style='text-align:right'>TOTAL GENERAL</td>");
                        strHtml.Append("<td style='text-align:right' title='Energía (MWh)'>" + item.Total.ToString("N", nfi) + "</td>");
                        strHtml.Append("<td style='text-align:right' title='Máxima Demanda (MW)'>" + item.MaximaDemanda.ToString("N", nfi) + "</td>");
                        //strHtml.Append("<td style='text-align:right' title='Mínima Demanda (MW)'>" + item.MinimaDemanda.ToString("N", nfi) + "</td>");
                        strHtml.Append("</tr>");
                    }
                }
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listCuadros"></param>
        /// <param name="fecInicio"></param>
        /// <param name="fecFin"></param>
        /// <param name="rutaFile"></param>
        public void GenerarArchivoExcelDespacho(MedicionReporteDTO umbrales, List<MedicionReporteDTO> listCuadros, DateTime fecInicio, DateTime fecFin, string rutaFile)
        {
            FileInfo newFile = new FileInfo(rutaFile);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;

                this.ExcelCabGeneral(ref ws, xlPackage, "Produccion_DespachoMD", "", 0);
                this.ExcelDetGeneralDespacho(ws, umbrales, listCuadros, fecInicio, fecFin);
                ws.View.ShowGridLines = false;

                xlPackage.Save();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="listCuadros"></param>
        /// <param name="fecInicio"></param>
        /// <param name="fecFin"></param>
        private void ExcelDetGeneralDespacho(ExcelWorksheet ws, MedicionReporteDTO umbrales, List<MedicionReporteDTO> list, DateTime fecInicio, DateTime fecFin)
        {
            ws.Cells[5, 2].Value = "REPORTE PRODUCCION DESPACHO MAXIMA DEMANDA DEL " + fecInicio.ToString(ConstantesAppServicio.FormatoFecha) + " al " + fecFin.ToString(ConstantesAppServicio.FormatoFecha);
            ws.Cells[6, 9].Value = "Máxima Demanda: " + umbrales.FechaMaximaDemanda.ToString(ConstantesAppServicio.FormatoFecha) + " a las " + umbrales.MaximaDemandaHora.ToString(ConstantesAppServicio.FormatoOnlyHora) + " h";
            ws.Cells[6, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            ExcelRange rg = ws.Cells[5, 2, 6, 9];
            rg.Style.Font.Size = 13;
            rg.Style.Font.Bold = true;

            int row = 7;

            ws.Cells[row, 2].Value = "N°";
            ws.Cells[row, 3].Value = "Empresa";
            ws.Cells[row, 4].Value = "Central";
            ws.Cells[row, 5].Value = "Unidad";
            ws.Cells[row, 6].Value = "E. Hidraulica (MWh)";
            ws.Cells[row, 7].Value = "E. Termica (MWh)";
            ws.Cells[row, 8].Value = "Total Empresa";
            ws.Cells[row, 9].Value = "Máxima Demanda(MW)";

            rg = ws.Cells[row, 2, row, 9];
            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
            rg.Style.Font.Color.SetColor(Color.White);
            rg.Style.Font.Size = 10;
            rg.Style.Font.Bold = true;

            row++;

            decimal totalH = 0, totalT = 0;
            foreach (MedicionReporteDTO item in list)
            {
                if (!item.IndicadorTotal && !item.IndicadorTotalGeneral)
                {
                    ws.Cells[row, 2].Value = item.NroItem;
                    ws.Cells[row, 3].Value = item.Emprnomb;
                    ws.Cells[row, 4].Value = item.Central;
                    ws.Cells[row, 5].Value = item.Unidad;
                    if (item.Tgenercodi == 1) { ws.Cells[row, 6].Value = item.Total; ws.Cells[row, 7].Value = ""; totalH += item.Total; }
                    else { ws.Cells[row, 6].Value = ""; ws.Cells[row, 7].Value = item.Total; totalT += item.Total; }
                    ws.Cells[row, 8].Value = item.Total;
                    ws.Cells[row, 9].Value = item.MaximaDemanda;

                    rg = ws.Cells[row, 2, row, 9];
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));
                }
                else
                {
                    if (item.IndicadorTotal)
                    {
                        ws.Cells[row, 2].Value = "TOTAL: " + item.Emprnomb.Trim();
                        ws.Cells[row, 2, row, 5].Merge = true;
                        ws.Cells[row, 6].Value = totalH;
                        ws.Cells[row, 7].Value = totalT;
                        ws.Cells[row, 8].Value = item.Total;
                        ws.Cells[row, 9].Value = item.MaximaDemanda;

                        rg = ws.Cells[row, 2, row, 9];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#E8F6FF"));
                        rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#245C86"));
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    }
                    if (item.IndicadorTotalGeneral)
                    {
                        ws.Cells[row, 2].Value = "TOTAL GENERAL";
                        ws.Cells[row, 2, row, 7].Merge = true;
                        ws.Cells[row, 8].Value = item.Total;
                        ws.Cells[row, 9].Value = item.MaximaDemanda;

                        rg = ws.Cells[row, 2, row, 9];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2E8DCD"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                }

                row++;
            }


            rg = ws.Cells[8, 6, row, 9];
            rg.Style.Numberformat.Format = "#,##0.000";


            rg = ws.Cells[7, 2, row - 1, 9];
            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

            ws.Column(2).Width = 10;

            rg = ws.Cells[7, 3, row, 9];
            rg.AutoFitColumns();

            HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
            ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
            picture.From.Column = 1;
            picture.From.Row = 1;
            picture.To.Column = 2;
            picture.To.Row = 2;
            picture.SetSize(120, 50);
        }

        #endregion

        #region Generar FileZip Pr21

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public int ObtenerGenerarFileZip(DateTime fechaInicio, DateTime fechaFinal)
        {
            var Formato = (new FormatoMedicionAppServicio()).GetByIdMeFormato(ConstantesHard.IdFormatoRpf);
            var cabecera = (new FormatoMedicionAppServicio()).GetListMeCabecera().FirstOrDefault(x => x.Cabcodi == Formato.Cabcodi);
            var ListaHojaPto = (new FormatoMedicionAppServicio()).GetListaPtos(DateTime.Now.Date, 0, -1, ConstantesHard.IdFormatoRpf, cabecera.Cabquery);

            string path = FileServer.GetDirectory() + "Intranet/Pr21/";

            return FactorySic.GetMeMedicion60Repository().ObtenerGenerarFileZip(fechaInicio, fechaFinal, path, ListaHojaPto.Select(x => x.Ptomedicodi).Distinct().ToList());
        }

        #endregion

        #region CONVOCATORIAS

        public List<WbConvocatoriasDTO> GetListaConvocatorias()
        {
            return FactorySic.GetWbConvocatoriasRepository().List();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="convcodi"></param>
        public void EliminarWbConvocatorias(int convcodi)
        {
            FactorySic.GetWbConvocatoriasRepository().Delete(convcodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="WbConvocatoriasDTO"></param>
        public int InsertarWbConvocatorias(WbConvocatoriasDTO obj)
        {
            return FactorySic.GetWbConvocatoriasRepository().Save(obj);
        }

        /// <summary>
        /// Actualizar tabla WbComunicados
        /// </summary>
        /// <param name="reg"></param>
        public void ActualizarWbConvocatorias(WbConvocatoriasDTO reg)
        {
            FactorySic.GetWbConvocatoriasRepository().Update(reg);
        }

        public string ListaConvocatoriasHtml(List<WbConvocatoriasDTO> data, string ruta, int typ)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='pretty tabla-icono display compact' id='tb_info" + typ + "'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 30px'>ACCIONES</th>");
            strHtml.Append("<th style='width: 200px'>CODIGO</th>");
            strHtml.Append("<th style='width: 400px'>NOMBRE</th>");
            strHtml.Append("<th>DESCRIPCIÓN</th>");
            strHtml.Append("<th style='width: 100px'>INICIO</th>");
            strHtml.Append("<th style='width: 100px'>TÉRMINO</th>");

            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            foreach (var list in data)
            {
                strHtml.Append("<tr id='" + list.Convcodi + "'>");
                strHtml.Append(string.Format(
                    "<td><a href='#' onclick='edit_(" + list.Convcodi + ");'><img src='{0}Content/Images/btn-edit.png' /></a>&nbsp;&nbsp;&nbsp;&nbsp;<a href='#' onclick='delete_(" + list.Convcodi + ");'><img src='{0}Content/Images/btn-cancel.png' /></a></td>"
                    , ruta));
                strHtml.Append(string.Format("<td>{0}</td>", list.Convabrev));
                strHtml.Append(string.Format("<td>{0}</td>", list.Convnomb));
                strHtml.Append(string.Format("<td>{0}</td>", list.Convdesc));
                strHtml.Append(string.Format("<td>{0}</td>", list.Convfechaini.Value.ToString(ConstantesAppServicio.FormatoFecha)));
                strHtml.Append(string.Format("<td>{0}</td>", list.Convfechafin.Value.ToString(ConstantesAppServicio.FormatoFecha)));

                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        #endregion

        #region Ficha tecnica 2023

        /// <summary>
        /// Devuelve los datos de cierto grupo
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        public PrGrupoDTO GetDatosGrupo(int grupocodi)
        {
            return FactorySic.GetPrGrupoRepository().ObtenerDatosGrupo(grupocodi);
        }

        /// <summary>
        /// Devuelve el listado de proyectos asciados a un grupo
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        public List<FtExtReleqpryDTO> ObtenerListadoProyectosPorGrupo(int grupocodi)
        {
            return servFictec.ObtenerListadoProyectosPorGrupo(grupocodi);
        }

        /// <summary>
        /// Guarda los cambios en asignacion del grupo
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <param name="strCambiosPE"></param>
        /// <param name="usuario"></param>
        public void GuardarDatosAsignacionFT(int grupocodi, string strCambiosPE, string usuario)
        {
            List<FtExtReleqpryDTO> lstTotalPE = FactorySic.GetFtExtReleqpryRepository().ListarSoloGrupos();

            //Obtenemos listas de nuevos y editados para proyectos extranet
            ObtenerListaAgregadosProyectosExtranet(strCambiosPE, lstTotalPE, grupocodi, usuario, out List<FtExtReleqpryDTO> lstPENuevos, out List<FtExtReleqpryDTO> lstPEEditados);

            GuardarDatosDeAsignacionTransaccionalmente(lstPENuevos, lstPEEditados);

        }

        /// <summary>
        /// Actualiza los cambios de asignacion de grupos/MO en proyectos en el modulo de asignacion de proyectos con etapas
        /// </summary>       
        /// <param name="strCambiosPE"></param>
        /// <param name="usuario"></param>
        public void ActualizarCambiosEnAsignacionDeProyectos(string strCambiosPE, string usuario)
        {

            if (strCambiosPE.Length > 0)
            {
                string[] separadorGrupo = { "??" };
                string[] separadorUnidad = { "$$" };
                string[] lstCambios = strCambiosPE.Split(separadorGrupo, System.StringSplitOptions.RemoveEmptyEntries);

                //Obtengo los FtExtEtempdetpry para los proyectos en lista
                List<int> lstFtprycodis = new List<int>();
                List<FtExtEtempdetpryDTO> lstRelProyectoEmpEtapa = new List<FtExtEtempdetpryDTO>();
                foreach (var cambio in lstCambios)
                {
                    string[] lstDatos = cambio.Split(separadorUnidad, System.StringSplitOptions.RemoveEmptyEntries);

                    if (lstDatos.Length > 0)
                    {
                        int miFtprycodi_ = Int32.Parse(lstDatos[1]);
                        lstFtprycodis.Add(miFtprycodi_);
                    }
                }
                string strFtprycodis = string.Join(",", lstFtprycodis);
                List<FtExtEtempdetpryDTO> lstRelPryEmpEtapa = strFtprycodis != "" ? FactorySic.GetFtExtEtempdetpryRepository().GetByProyectos(strFtprycodis) : new List<FtExtEtempdetpryDTO>();
                List<FtExtProyectoDTO> lstProyectosEnRel = strFtprycodis != "" ? FactorySic.GetFtExtProyectoRepository().ListarGrupo(strFtprycodis) : new List<FtExtProyectoDTO>();
                List<FtExtRelempetapaDTO> lstRelEE = lstRelPryEmpEtapa.Any() ? FactorySic.GetFtExtRelempetapaRepository().GetByProyectos(strFtprycodis) : new List<FtExtRelempetapaDTO>();

                //Por cada cambio (agrega o baja proyectos) al grupo/MO  
                foreach (var cambio in lstCambios)
                {
                    string[] lstDatos = cambio.Split(separadorUnidad, System.StringSplitOptions.RemoveEmptyEntries);

                    if (lstDatos.Length > 0)
                    {
                        FtExtReleqpryDTO obj = new FtExtReleqpryDTO();

                        int miFtreqpcodi = Int32.Parse(lstDatos[0]);
                        int miFtprycodi = Int32.Parse(lstDatos[1]);
                        int miEstado = Int32.Parse(lstDatos[2]);
                        int esAgregado = Int32.Parse(lstDatos[3]);
                        int esEditado = Int32.Parse(lstDatos[4]);

                        //busco las etapas para cada proyecto, solo activos
                        List<FtExtEtempdetpryDTO> relPryEmpEtapaPorProyecto = lstRelPryEmpEtapa.Where(x => x.Ftprycodi == miFtprycodi && x.Feepryestado == ConstantesFichaTecnica.EstadoStrActivo).ToList();

                        //solo actualizo cambios para aquellos que tengan relacion con alguna etapa (1 o mas)
                        if (relPryEmpEtapaPorProyecto.Any())
                        {
                            //para cada etapa, actualizo
                            foreach (var rel in relPryEmpEtapaPorProyecto)
                            {
                                int fetempcodi = rel.Fetempcodi;
                                FtExtRelempetapaDTO ree = lstRelEE.Find(x => x.Fetempcodi == fetempcodi);

                                FtExtRelempetapaDTO grup = servFictec.ObtenerInformacionRelEmpresaEtapa(fetempcodi);

                                List<FtExtEtempdetpryDTO> lstProyectos = grup.ListaProyectos;
                                int accion = ConstantesFichaTecnica.AccionEditar;
                                int emprcodi = ree.Emprcodi;
                                int idEtapa = ree.Ftetcodi;

                                //Actualiza ventana de asignacion de proyectos
                                servFictec.GuardarDatosProyectoYRelaciones(accion, fetempcodi, emprcodi, idEtapa, lstProyectos, new List<FTRelacionEGP>(), new List<FTRelacionEGP>(), usuario);

                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  Devuelve el listado de registros nuevos y editados de proyectos extranet
        /// </summary>
        /// <param name="strCambiosPE"></param>
        /// <param name="lstTotalPE"></param>
        /// <param name="grupocodi"></param>
        /// <param name="usuario"></param>
        /// <param name="lstNuevos"></param>
        /// <param name="lstEditados"></param>
        private void ObtenerListaAgregadosProyectosExtranet(string strCambiosPE, List<FtExtReleqpryDTO> lstTotalPE, int grupocodi, string usuario, out List<FtExtReleqpryDTO> lstNuevos, out List<FtExtReleqpryDTO> lstEditados)
        {
            lstNuevos = new List<FtExtReleqpryDTO>();
            lstEditados = new List<FtExtReleqpryDTO>();

            if (strCambiosPE.Length > 0)
            {
                string[] separadorGrupo = { "??" };
                string[] separadorUnidad = { "$$" };
                string[] lstCambios = strCambiosPE.Split(separadorGrupo, System.StringSplitOptions.RemoveEmptyEntries);

                foreach (var cambio in lstCambios)
                {
                    string[] lstDatos = cambio.Split(separadorUnidad, System.StringSplitOptions.RemoveEmptyEntries);

                    if (lstDatos.Length > 0)
                    {
                        FtExtReleqpryDTO obj = new FtExtReleqpryDTO();

                        int miFtreqpcodi = Int32.Parse(lstDatos[0]);
                        int miFtprycodi = Int32.Parse(lstDatos[1]);
                        int miEstado = Int32.Parse(lstDatos[2]);
                        int esAgregado = Int32.Parse(lstDatos[3]);
                        int esEditado = Int32.Parse(lstDatos[4]);

                        if (miFtreqpcodi == -1) //Es agregado
                        {
                            //obj.Ftreqpcodi { get; set; }
                            obj.Equicodi = null;
                            obj.Ftprycodi = miFtprycodi;
                            obj.Ftreqpestado = miEstado;
                            obj.Ftreqpusucreacion = usuario;
                            obj.Ftreqpfeccreacion = DateTime.Now;
                            obj.Ftreqpusumodificacion = usuario;
                            obj.Ftreqpfecmodificacion = DateTime.Now;
                            obj.Grupocodi = grupocodi;

                            lstNuevos.Add(obj);
                        }
                        else //es Editado
                        {
                            FtExtReleqpryDTO objEditado = lstTotalPE.Find(x => x.Ftreqpcodi == miFtreqpcodi);

                            //Agrego solo si su estado sufrio cambios
                            if (objEditado.Ftreqpestado != miEstado)
                            {
                                obj = objEditado;
                                obj.Ftreqpestado = miEstado;
                                obj.Ftreqpusumodificacion = usuario;
                                obj.Ftreqpfecmodificacion = DateTime.Now;

                                lstEditados.Add(obj);
                            }

                        }
                    }
                }
            }
        }



        /// <summary>
        /// Guarda la informacion transaccionalmente
        /// </summary>
        /// <param name="lstPENuevos"></param>
        /// <param name="lstPEEditados"></param>
        public void GuardarDatosDeAsignacionTransaccionalmente(List<FtExtReleqpryDTO> lstPENuevos, List<FtExtReleqpryDTO> lstPEEditados)
        {
            DbTransaction tran = null;

            var UoW = FactorySic.UnitOfWork();

            using (var connection = UoW.BeginConnection())
            {
                using (var transaction = UoW.StartTransaction(connection))
                {
                    try
                    {
                        //Guardar los nuevos registros de proyectos extranet 
                        foreach (FtExtReleqpryDTO regNuevoPE in lstPENuevos)
                        {
                            int cod1 = FactorySic.GetFtExtReleqpryRepository().Save(regNuevoPE, connection, transaction);
                        }

                        //Actualizar los registros editados de proyectos extranet 
                        foreach (FtExtReleqpryDTO regEditadoPE in lstPEEditados)
                        {
                            FactorySic.GetFtExtReleqpryRepository().Update(regEditadoPE, connection, transaction);
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

        }

        public List<MeMedicion96DTO> LeerMedidores(DateTime fechaInicio)
        {
            return FactorySic.GetMePtomedicionRepository().LeerMedidores(fechaInicio);
        }

        #endregion

        #region AddIn Htrabajo - Costo Variable

        /// <summary>
        ///  Devuelve el listado de potencia para las hojas ACTIVA y REACTIVA
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="lstCabecera"></param>
        /// <param name="hoja"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListarDatosPotenciaScada(DateTime fecha, List<int> lstCabecera, string hoja)
        {
            List<MeMedicion48DTO> salida = new List<MeMedicion48DTO>();

            int tipoinfocodi = 0;
            switch (hoja)
            {
                case ConstantesMigraciones.HojaActiva: tipoinfocodi = ConstantesAppServicio.TipoinfocodiMW; break;
                case ConstantesMigraciones.HojaReactiva: tipoinfocodi = ConstantesAppServicio.TipoinfocodiMVAR; break;
            }

            //Obtengo toda la data a usar:
            string strCanalesTotales = "";
            List<ScadaDTO> lstDataCanales = new List<ScadaDTO>();
            List<int> lstCanalesTotales = new List<int>();
            List<MeScadaSp7DTO> lstScadaTotales = new List<MeScadaSp7DTO>();

            if (lstCabecera.Any())
            {
                lstCabecera = lstCabecera.OrderBy(x => x).Distinct().ToList();
                string strPtosMedicodisExcel = string.Join(",", lstCabecera.Select(x => x));

                List<MePtomedcanalDTO> listaEquivalencia = strPtosMedicodisExcel != "" ? FactorySic.GetMePtomedcanalRepository().ObtenerEquivalencia(strPtosMedicodisExcel, tipoinfocodi) : new List<MePtomedcanalDTO>();
                lstCanalesTotales = listaEquivalencia.Select(x => x.Canalcodi).Distinct().ToList();

                //Obtengo la data de scada para todos 
                strCanalesTotales = string.Join(",", lstCanalesTotales.Select(x => x).Distinct().ToList());
                lstScadaTotales = strCanalesTotales != "" ? FactoryScada.GetMeScadaSp7Repository().GetByCriteria(fecha, fecha, strCanalesTotales) : new List<MeScadaSp7DTO>();

                //Genero med48
                List<MeMedicion48DTO> lstData = ObtenerRegistrosEquivalenciaPtoMedScada(tipoinfocodi, lstCabecera, listaEquivalencia, lstScadaTotales);
                salida.AddRange(lstData);
            }

            return salida;
        }

        /// <summary>
        /// Devuelve el listado de potencia para las hojas FLUJOS
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="lstCabecera"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListarDatosFlujoScada(DateTime fecha, List<int> lstCabecera)
        {
            List<MeMedicion48DTO> salida = new List<MeMedicion48DTO>();

            //Obtengo toda la data a usar:
            string strCanalesTotales = "";
            List<int> lstCanalesTotales = new List<int>();
            List<MeScadaSp7DTO> lstScadaTotales = new List<MeScadaSp7DTO>();

            if (lstCabecera.Any())
            {
                List<int> lstCabFlujosNegativos = lstCabecera.Where(x => x < 0).ToList();
                List<int> lstCabFlujosPositivos = lstCabecera.Where(x => x >= 0).ToList();
                List<int> lstCabFlujosExNegativos = lstCabFlujosNegativos.Select(y => y * (-1)).ToList();

                lstCanalesTotales.AddRange(lstCabFlujosExNegativos);
                lstCanalesTotales.AddRange(lstCabFlujosPositivos);


                //Obtengo la data de scada para todos 
                strCanalesTotales = string.Join(",", lstCanalesTotales.Select(x => x).Distinct().ToList());
                lstScadaTotales = strCanalesTotales != "" ? FactoryScada.GetMeScadaSp7Repository().GetByCriteria(fecha, fecha, strCanalesTotales) : new List<MeScadaSp7DTO>();

                //Genero med48
                List<MeMedicion48DTO> lstData1 = ObtenerRegistrosEquivalenciaFlujosScada(lstCabFlujosPositivos, lstScadaTotales, 1);
                List<MeMedicion48DTO> lstData2 = ObtenerRegistrosEquivalenciaFlujosScada(lstCabFlujosExNegativos, lstScadaTotales, -1);

                if (lstData1.Any()) salida.AddRange(lstData1);
                if (lstData2.Any()) salida.AddRange(lstData2);
            }

            return salida;
        }

        /// <summary>
        /// Obtiene los registros Med48 a mostrarse en el excel addin por cada 30min por cada canalcodi
        /// </summary>
        /// <param name="lstCanalcodisExcel"></param>
        /// <param name="lstScada"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        private List<MeMedicion48DTO> ObtenerRegistrosEquivalenciaFlujosScada(List<int> lstCanalcodisExcel, List<MeScadaSp7DTO> lstScada, decimal factor)
        {
            List<MeMedicion48DTO> lstSalida = new List<MeMedicion48DTO>();

            List<MeScadaSp7DTO> listaDataCanales = lstScada.Where(x => lstCanalcodisExcel.Contains(x.Canalcodi)).Distinct().ToList();

            foreach (MeScadaSp7DTO regScada in listaDataCanales)
            {
                int canalcodi = regScada.Canalcodi;
                //Armo registro
                MeMedicion48DTO regGuardarFlujo = new MeMedicion48DTO();
                regGuardarFlujo.Canalcodi = canalcodi;
                regGuardarFlujo.Tipoinfodesc = ConstantesMigraciones.HojaFlujos;

                for (var j = 1; j <= 48; j++)
                {
                    decimal? valorTemp = (decimal?)regScada.GetType().GetProperty("H" + (j * 2)).GetValue(regScada, null);

                    if (valorTemp != null)
                    {
                        valorTemp = valorTemp * (factor);
                        regGuardarFlujo.GetType().GetProperty("H" + j.ToString()).SetValue(regGuardarFlujo, valorTemp);
                    }
                }
                lstSalida.Add(regGuardarFlujo);
            }

            return lstSalida;
        }

        /// <summary>
        /// Obtiene los registros Med48 a mostrarse en el excel addin por cada 30min por cada ptomedicion
        /// </summary>
        /// <param name="lstPtosMedicodisExcel"></param>
        /// <param name="lstEquivalenciaPtoMedCanal"></param>
        /// <param name="lstScada"></param>
        /// <returns></returns>
        private List<MeMedicion48DTO> ObtenerRegistrosEquivalenciaPtoMedScada(int tipoinfocodi, List<int> lstPtosMedicodisExcel, List<MePtomedcanalDTO> lstEquivalenciaPtoMedCanal, List<MeScadaSp7DTO> lstScada)
        {
            bool esHojaActiva = ConstantesAppServicio.TipoinfocodiMW == tipoinfocodi;

            List<MeMedicion48DTO> lstSalida = new List<MeMedicion48DTO>();

            foreach (int ptomedicodi in lstPtosMedicodisExcel)
            {
                List<MePtomedcanalDTO> listaDataRelacion = lstEquivalenciaPtoMedCanal.Where(x => x.Ptomedicodi == ptomedicodi).ToList();
                List<int> lstCanales = listaDataRelacion.Select(x => x.Canalcodi).Distinct().ToList();
                List<MeScadaSp7DTO> listaDataCanalesPorIdCabecera = lstScada.Where(x => lstCanales.Contains(x.Canalcodi)).Distinct().ToList();

                //Armo registro
                MeMedicion48DTO regGuardar = new MeMedicion48DTO();
                regGuardar.Ptomedicodi = ptomedicodi;
                regGuardar.Tipoinfodesc = ConstantesMigraciones.HojaActiva;

                if (listaDataCanalesPorIdCabecera.Any())
                {
                    decimal tsuma = 0;
                    decimal valMin = 0.2m;
                    for (var j = 1; j <= 48; j++)
                    {
                        tsuma = 0;

                        foreach (MeScadaSp7DTO reg in listaDataCanalesPorIdCabecera)
                        {
                            int canalcodi = reg.Canalcodi;
                            MePtomedcanalDTO regVal = listaDataRelacion.Find(x => x.Canalcodi == canalcodi);
                            decimal factorCanal = regVal != null ? (regVal.Pcanfactor != null ? regVal.Pcanfactor.Value : 1) : 1;

                            decimal? valorTemp = (decimal?)reg.GetType().GetProperty("H" + (j * 2)).GetValue(reg, null);

                            if (valorTemp != null)
                            {
                                if (Math.Abs(valorTemp.Value) >= valMin)
                                {
                                    tsuma += valorTemp.Value * factorCanal;
                                }
                            }
                        }

                        decimal sumaAbsoluto = Math.Abs(tsuma);
                        if (sumaAbsoluto > 0)
                        {
                            //la hoja activa siempre muestra valores positivos
                            if (esHojaActiva) tsuma = Math.Abs(tsuma);

                            regGuardar.GetType().GetProperty("H" + j.ToString()).SetValue(regGuardar, tsuma);
                        }
                    }
                }

                lstSalida.Add(regGuardar);
            }

            return lstSalida;
        }

        /// <summary>
        /// Obtener la información TNA para la hoja Activa del AddIn
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="ptomedicodis"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListarDataTNAxPtomedicion(DateTime fechaPeriodo, string ptomedicodis)
        {
            List<MeMedicion48DTO> listaDataXPto = new List<MeMedicion48DTO>();

            if (string.IsNullOrEmpty(ptomedicodis))
                return listaDataXPto;

            //famcodis
            List<int> lfamcodiCentral = new List<int>() { ConstantesHorasOperacion.IdTipoEolica, ConstantesHorasOperacion.IdTipoSolar, ConstantesHorasOperacion.IdTipoHidraulica, ConstantesHorasOperacion.IdTipoTermica };
            List<int> lfamcodiUnidad = new List<int>() { ConstantesHorasOperacion.IdGeneradorEolica, ConstantesHorasOperacion.IdGeneradorSolar, ConstantesHorasOperacion.IdGeneradorHidroelectrico, ConstantesHorasOperacion.IdGeneradorTemoelectrico };

            //insumo Ptos de medicion (solo generación de centrales y unidades, excluir de reserva fría y otros)
            List<int> listaPtomedicodi = ptomedicodis.Split(',').Select(x => int.Parse(x)).ToList();
            List<MePtomedicionDTO> listaPtoAddin = FactorySic.GetMePtomedicionRepository().List(ptomedicodis, "-1");
            listaPtoAddin = listaPtoAddin.Where(x => lfamcodiCentral.Contains(x.Famcodi) || lfamcodiUnidad.Contains(x.Famcodi)).ToList();

            //insumo TNA cada 30min (solo es a nivel de unidades)
            List<CmGeneracionEmsDTO> listaEmsDia = FactorySic.GetCmGeneracionEmsRepository().GetListaGeneracionByEquipoFecha(fechaPeriodo, fechaPeriodo, "-2", string.Join(",", lfamcodiCentral))
                .Where(x => x.Genemsgeneracion != null).ToList();

            //generar data
            foreach (var itemPto in listaPtoAddin)
            {
                if (itemPto.Ptomedicodi == 113)
                {
                }

                List<CmGeneracionEmsDTO> listaEmsXPto = new List<CmGeneracionEmsDTO>();

                //si el punto de medición es central entonces obtener de las unidades 
                if (lfamcodiCentral.Contains(itemPto.Famcodi))
                {
                    listaEmsXPto = listaEmsDia.Where(x => x.Equipadre == itemPto.Equicodi).ToList();
                }
                else
                {
                    listaEmsXPto = listaEmsDia.Where(x => x.Equicodi == itemPto.Equicodi).ToList();
                }

                //Verificar existencia de EMS para el equipo
                if (listaEmsXPto.Any())
                {
                    listaDataXPto.Add(ObtenerSumaMedicion48FromListamEms(itemPto.Ptomedicodi, fechaPeriodo, listaEmsXPto));
                }
                else
                {
                    //el equipo no tiene configuración EMS
                }
            }

            return listaDataXPto;
        }

        private MeMedicion48DTO ObtenerSumaMedicion48FromListamEms(int ptomedicodi, DateTime fecha, List<CmGeneracionEmsDTO> listaEmsXPto)
        {
            MeMedicion48DTO regMW = new MeMedicion48DTO();
            regMW.Ptomedicodi = ptomedicodi;
            regMW.Medifecha = fecha;
            regMW.Tipoinfocodi = ConstantesAppServicio.TipoinfocodiMW;

            for (int h = 1; h <= 48; h++)
            {
                //valor MW
                decimal? valorH = null;
                List<CmGeneracionEmsDTO> sublista = new List<CmGeneracionEmsDTO>();
                if (h != 48) sublista = listaEmsXPto.Where(x => x.Genemsfecha == fecha.AddMinutes(h * 30)).ToList();
                if (h == 48) sublista = listaEmsXPto.Where(x => x.Genemsfecha == fecha.AddMinutes(h * 30).AddMinutes(-1)).ToList();

                if (sublista.Any())
                {
                    valorH = sublista.Sum(x => x.Genemsgeneracion ?? 0);
                }

                regMW.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(regMW, valorH);
            }

            return regMW;
        }

        /// <summary>
        /// Devuelve los eventos de CV para un rango de fechas
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public List<PrRepcvDTO> ListarDataEventosCVPorFechas(DateTime fechaInicial, DateTime fechaFinal)
        {
            DespachoAppServicio appDespacho = new DespachoAppServicio();
            List<PrRepcvDTO> listadoEventos = appDespacho.GetByCriteriaPrRepcvs("-1", fechaInicial, fechaFinal);

            foreach (var item in listadoEventos)
            {
                string fec = item.RepfechaDesc != "" ? item.RepfechaDesc.Trim() : "";
                string tipo = item.ReptipoDesc != "" ? item.ReptipoDesc.Trim() : "";
                string nombre = item.Repnomb != "" ? item.Repnomb.Trim() : "";
                string detalle = item.Repdetalle != "" ? item.Repdetalle.Trim() : "";

                item.OpcionEvento = fec + " - " + tipo + " - " + nombre + " - " + detalle;
            }
            listadoEventos = listadoEventos.OrderByDescending(x => x.Repfecha).ThenBy(x => x.ReptipoDesc).ThenBy(x => x.Repnomb).ThenBy(x => x.Repdetalle).ToList();

            return listadoEventos;
        }

        /// <summary>
        /// Devuelve parametros generales
        /// </summary>
        /// <param name="repcodi"></param>
        /// <returns></returns>
        public Base.Tools.n_parameter ListarDataParametrosGeneralesCV(int repcodi)
        {
            DespachoAppServicio appDespacho = new DespachoAppServicio();
            PrRepcvDTO objCV = appDespacho.GetByIdPrRepcv(repcodi);

            DateTime fechaProceso = objCV.Repfecha;

            List<PrGrupodatDTO> lParametrosGenerales = FactorySic.GetPrGrupodatRepository().ParametrosGeneralesPorFecha(fechaProceso).OrderBy(x => x.Concepcodi).ToList();

            //iniciailizar n_parametrer		
            Base.Tools.n_parameter l_PGenerales = new Base.Tools.n_parameter();
            foreach (PrGrupodatDTO drParam in lParametrosGenerales)
            {
                l_PGenerales.SetData(drParam.Concepabrev.Trim(), drParam.Formuladat.Trim());
            }

            return l_PGenerales;
        }

        /// <summary>
        /// Devuelve listado de los parametros de modos con su respectivo valor
        /// </summary>
        /// <param name="repcodi"></param>
        /// <param name="FlagIncluirMONoActivos"></param>
        /// <param name="lstParametrosModo"></param>
        /// <param name="conceptosInvalidos"></param>
        /// <returns></returns>
        public List<AddInColModo> ListarDataParametrosModoCV(int repcodi, string FlagIncluirMONoActivos, List<AddInColModo> lstParametrosModo, out string strConceptosInvalidos)
        {
            List<AddInColModo> salida = new List<AddInColModo>();
            List<string> conceptosInvalidos = new List<string>();

            DespachoAppServicio appDespacho = new DespachoAppServicio();
            PrRepcvDTO objCV = appDespacho.GetByIdPrRepcv(repcodi);

            DateTime fechaProceso = objCV.Repfecha;
            int filaInicial = 14;

            //Las 4 primeras columnas llevarán asterisco (se reemplazan al pintar en el excel)
            foreach (AddInColModo item in lstParametrosModo)
            {
                if (item.Columna == 0 || item.Columna == 1 || item.Columna == 2 || item.Columna == 3)
                {
                    item.Asterisco = "S";
                }
            }

            //Solo los que tienen asterisco seran tomados en cuenta al buscar su valor
            List<AddInColModo> lstParametrosModoConAsterisco = lstParametrosModo.Where(x => x.Asterisco == "S").ToList();

            List<PrGrupodatDTO> lParametrosGenerales = FactorySic.GetPrGrupodatRepository().ParametrosGeneralesPorFecha(fechaProceso);
            List<PrGrupodatDTO> lParametrosFuncionales = FactorySic.GetPrGrupodatRepository().ParametrosPorFecha(fechaProceso); //parametros de todos los modos, grupos termicos y central
            List<PrGrupoDTO> listaModo = FactorySic.GetPrGrupoRepository().ListadoModosFuncionalesCostosVariables(fechaProceso, ConstantesAppServicio.SI);

            if (FlagIncluirMONoActivos == "S")

            {
                List<PrGrupoDTO> listaModoNA = FactorySic.GetPrGrupoRepository().ListadoModosFuncionalesCostosVariables(fechaProceso, ConstantesAppServicio.NO);
                listaModo.AddRange(listaModoNA);
            }


            if (listaModo.Any())
            {

                //ordeno el listado que sera mostrado en el excel 
                listaModo = listaModo.OrderByDescending(x => x.Grupoactivo).ThenBy(x => x.EmprNomb.Trim()).ThenBy(x => x.Gruponomb.Trim()).ThenBy(x => x.Grupocodi).ToList();

                //Genero el listado a pintar en el excel
                foreach (PrGrupoDTO oGrupoFunc in listaModo)
                {
                    int grupoCodi = oGrupoFunc.Grupocodi; //codigo modo
                    int grupoPadre = oGrupoFunc.Grupopadre.Value; //codigo grupo termico
                    int grupoCentral = oGrupoFunc.GrupoCentral; //codigo central

                    Base.Tools.n_parameter paramXModo = new Base.Tools.n_parameter();

                    //Necesita conocer los parametros generales que posiblemente seran usados en los parametos funcionales
                    foreach (PrGrupodatDTO drParam in lParametrosGenerales)
                    {
                        paramXModo.SetData(drParam.Concepabrev.Trim(), drParam.Formuladat.Trim());
                    }

                    List<PrGrupodatDTO> lParametrosFuncionalesParaElModo = lParametrosFuncionales.Where(x => x.Grupocodi == grupoCodi || x.Grupocodi == grupoPadre || x.Grupocodi == grupoCentral).ToList();
                    foreach (PrGrupodatDTO drLast in lParametrosFuncionalesParaElModo)
                    {
                        paramXModo.SetData(drLast.Concepabrev.Trim(), drLast.Formuladat.Trim());
                    }

                    foreach (AddInColModo regCelda in lstParametrosModoConAsterisco)
                    {
                        AddInColModo regNuevo = new AddInColModo();

                        regNuevo.AbrevModo = regCelda.AbrevModo;
                        regNuevo.Fila = filaInicial;
                        regNuevo.Columna = regCelda.Columna;
                        regNuevo.Asterisco = regCelda.Asterisco;

                        if (regCelda.Columna < 4)
                        {
                            if (regCelda.Columna == 0) regNuevo.Formula = (filaInicial - 13).ToString();
                            if (regCelda.Columna == 1) regNuevo.Formula = grupoCodi.ToString();
                            if (regCelda.Columna == 2) regNuevo.Formula = oGrupoFunc.EmprNomb != null ? oGrupoFunc.EmprNomb.Trim() : "";
                            if (regCelda.Columna == 3) regNuevo.Formula = oGrupoFunc.Gruponomb != null ? oGrupoFunc.Gruponomb.Trim() : "";
                        }
                        else
                        {
                            if (paramXModo.Contains(regCelda.AbrevModo))
                            {
                                regNuevo.Formula = paramXModo.EvaluateFormula(regCelda.AbrevModo);
                            }
                            else
                            {
                                regNuevo.Formula = "";
                                if (regCelda.AbrevModo.Trim().ToUpper() == "TIPO")
                                {
                                    regNuevo.Formula = "T";
                                }

                            }

                        }

                        salida.Add(regNuevo);
                    }

                    filaInicial++;
                }
            }

            //los conceptos que son invalidos a excepcion de TIPO y CVTOTAL seran mostrados en el logMensajes
            foreach (AddInColModo regCelda in lstParametrosModoConAsterisco)
            {
                string concepto = regCelda.AbrevModo.Trim().ToUpper();
                if (regCelda.Columna >= 4)
                {
                    if (concepto != "TIPO" && concepto != "CVTOTAL")
                    {
                        //lo busco en parametros generales
                        PrGrupodatDTO reg1 = lParametrosGenerales.Where(x => x.Concepabrev != null).ToList().Find(x => x.Concepabrev.Trim().ToUpper() == concepto);
                        PrGrupodatDTO reg2 = lParametrosFuncionales.Where(x => x.Concepabrev != null).ToList().Find(x => x.Concepabrev.Trim().ToUpper() == concepto);

                        if (reg1 == null && reg2 == null)
                        {
                            conceptosInvalidos.Add(regCelda.AbrevModo);

                        }
                    }
                }

            }

            strConceptosInvalidos = string.Join(", ", conceptosInvalidos.Distinct().ToList());

            return salida;
        }

        #endregion

        #region Migraciones 2024

        #region Reporte Produccion
        /// <summary>
        /// Devuelve el listado del reporte de produccion
        /// </summary>
        /// <param name="tipoInfo"></param>
        /// <param name="fecInicial"></param>
        /// <param name="fecFinal"></param>
        /// <param name="tipoEmpresas"></param>
        /// <param name="fechaMDCoes"></param>
        /// <param name="hMDCoes"></param>
        /// <param name="fecMDListado"></param>
        /// <returns></returns>
        public ReporteProduccion ListarReporteProduccion(int tipoInfo, DateTime fecInicial, DateTime fecFinal, int tipoEmpresas, DateTime? fechaMDCoes, int? hMDCoes, out DateTime fecMDListado, out int hMDListado)
        {
            ReporteProduccion salida = new ReporteProduccion();
            NumberFormatInfo nfi3 = UtilAnexoAPR5.GenerarNumberFormatInfo3();

            List<RegistroRepProduccion> lstRegistros = new List<RegistroRepProduccion>();
            DateTime fecMD = new DateTime();
            int hMD = -1;
            decimal? totalSein = 0;
            bool hayTotal = false;

            EjecutadoAppServicio servicioEjecutado = new EjecutadoAppServicio();
            //Obtenemos Insumo despacho  (sin cruce de horas de operación)
            List<MeMedicion48DTO> listaM48RangoDespacho = servicioEjecutado.ListaDataMDGeneracionConsolidado48(fecInicial, fecFinal, tipoEmpresas,
                ConstantesMedicion.IdTipoGeneracionTodos.ToString(), ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos,
                ConstantesMedicion.IdTipoRecursoTodos.ToString(), false, tipoInfo);

            //agrupar por dia	
            List<MeMedicion48DTO> listaDemandaGen48XDia = servicioEjecutado.ListaDataMDGeneracionFromConsolidado48(fecInicial, fecFinal, listaM48RangoDespacho);

            int tipoDespachoEjec = 1;
            //[Salida]Obtener detalle del despacho (Filas de la tabla web)
            List<MeDespachoProdgenDTO> listaDespachoDiaXGrupoYTgen = listaM48RangoDespacho
                        .GroupBy(x => new { x.Grupocodi, x.Emprcodi })
                        .Select(x => new MeDespachoProdgenDTO()
                        {
                            Dpgentipo = tipoDespachoEjec,
                            Dpgenvalor = x.Sum(y => y.Meditotal ?? 0),
                            Dpgenintegrante = x.First().Grupointegrante,
                            Dpgenrer = x.First().Tipogenerrer,
                            Emprcodi = x.Key.Emprcodi,
                            Equipadre = x.First().Equipadre,
                            Grupocodi = x.Key.Grupocodi,

                            Tgenercodi = x.First().Tgenercodi,
                            Emprnomb = x.First().Emprnomb,
                            Gruponomb = x.First().Gruponomb,
                            Ptomedicodi = x.First().Ptomedicodi,

                        }).ToList();

            //[Salida] resumen de datos de centrales integrantes COES (última fila de la tabla web)
            List<MeDespachoResumenDTO> listaTotalEjec = listaDemandaGen48XDia.GroupBy(x => x.Medifecha)
                        .Select(x => new MeDespachoResumenDTO()
                        {
                            Dregentipo = tipoDespachoEjec,
                            Dregenfecha = x.Key,
                            Dregentotalsein = x.Sum(y => y.Meditotal ?? 0)
                        }).ToList();

            List<MeMedicion48DTO> listaInterconexion48Ejec = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaTotalExp = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaTotalImp = new List<MeMedicion48DTO>();

            //Interconexiones Internacionales (solo para ejecutado)
            if (tipoInfo.ToString() == ConstantesAppServicio.LectcodiEjecutadoHisto)
            {
                servicioEjecutado.ListaFlujo30minInterconexion48(ConstantesInterconexiones.FuenteTIEFlujoOldDesktop, fecInicial, fecFinal, out listaInterconexion48Ejec, out listaTotalExp, out listaTotalImp);
            }

            //Sumatoria de Generación e Intercambios
            List<MeMedicion48DTO> listaMedicionTotal48 = servicioEjecutado.ListaDataMDTotalSEIN48(listaDemandaGen48XDia, listaInterconexion48Ejec);

            //ejecutados
            foreach (MeDespachoResumenDTO regDia in listaTotalEjec)
            {
                //Máxima demanda del SEIN
                servicioEjecutado.GetDiaMaximaDemandaFromDataMD48(regDia.Dregenfecha.Date, regDia.Dregenfecha.Date, ConstantesRepMaxDemanda.TipoMaximaTodoDia, listaMedicionTotal48, null, null, 
                                                                                out DateTime fechaHoraMDSein, out DateTime fechaDia48S, out int hMax48S);

                regDia.Dregenmdhora = fechaHoraMDSein;
                regDia.HMaxD48 = hMax48S;
                //regDia.Dregenmdsein = servicioEjecutado.GetValorH(fechaHoraMDSein, listaMedicionTotal48); //valor decimal de la maxima demanda
                //decimal? regenmdsein = servicioEjecutado.GetValorH(fechaMDCoes.Value, listaMedicionTotal48); //valor decimal de la maxima demanda
                regDia.Dregenmdexp = servicioEjecutado.GetValorH(fechaHoraMDSein, listaTotalExp);
                regDia.Dregenmdimp = servicioEjecutado.GetValorH(fechaHoraMDSein, listaTotalImp);

                if (fechaMDCoes != null && hMDCoes != null) //para las empresas NO COES, uso la fecha de MD de las empresa COES
                {
                    regDia.Dregenmdsein = servicioEjecutado.GetValorH(fechaMDCoes.Value, listaMedicionTotal48);
                }
                else
                {
                    regDia.Dregenmdsein = servicioEjecutado.GetValorH(fechaHoraMDSein, listaMedicionTotal48);//valor decimal de la maxima demanda
                }

                if (regDia.Dregentotalsein != null)
                    hayTotal = true;
                totalSein = totalSein + regDia.Dregentotalsein;
            }


            //Obtenemos fecha y hora de la max demanda (si son emprsas NO COES, se usa la fecha de MD de las empresas COES)
            MeDespachoResumenDTO regMD = new MeDespachoResumenDTO();
            DateTime dechaDefault = new DateTime(1900, 1, 1);
            if (fechaMDCoes == null)
            {
                regMD = listaTotalEjec.Any() ? listaTotalEjec.OrderByDescending(x => x.Dregenmdsein).First() : null;
            }
            else //para las empresas NO COES, uso la fecha de MD de las empresa COES
            {
                regMD = fechaMDCoes != dechaDefault ? (listaTotalEjec.Any() ? listaTotalEjec.Find(x => x.Dregenfecha.Date == fechaMDCoes.Value.Date) : null) : null;
            }
            fecMDListado = regMD != null ? regMD.Dregenmdhora : dechaDefault;
            hMDListado = regMD != null ? regMD.HMaxD48 : -1;
            List<MeMedicion48DTO> listaM48RangoPorFechaMD = new List<MeMedicion48DTO>();
            if (regMD != null)
            {
                listaM48RangoPorFechaMD = listaM48RangoDespacho.Where(x => x.Medifecha.Date == regMD.Dregenfecha.Date).ToList();
            }

            //Generamos los registros del reporte 
            foreach (var fila in listaDespachoDiaXGrupoYTgen)
            {
                //num de filas por empresa
                List<MeDespachoProdgenDTO> lstEmpresa = listaDespachoDiaXGrupoYTgen.Where(x => x.Emprcodi == fila.Emprcodi).ToList();

                //Total
                decimal valorHidro = 0;
                if (fila.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiHidro) valorHidro = fila.Dpgenvalor ?? 0;
                decimal valorTermo = 0;
                if (fila.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiTermo
                    || fila.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiEolica
                    || fila.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiSolar) valorTermo = fila.Dpgenvalor ?? 0;

                //MD
                List<MeMedicion48DTO> lstPorGrupo = listaM48RangoPorFechaMD.Where(x => x.Grupocodi == fila.Grupocodi && x.Emprcodi == fila.Emprcodi).ToList();
                MeMedicion48DTO objMD = lstPorGrupo.FirstOrDefault();

                decimal? valor = null;
                if (regMD != null && objMD != null)
                {
                    if (hMDCoes == null)
                    {
                        valor = ((decimal?)objMD.GetType().GetProperty(ConstantesAppServicio.CaracterH + regMD.HMaxD48).GetValue(objMD, null));
                    }
                    else //para las empresas NO COES, uso la fecha de MD de las empresa COES
                    {
                        valor = hMDCoes != -1 ? ((decimal?)objMD.GetType().GetProperty(ConstantesAppServicio.CaracterH + hMDCoes).GetValue(objMD, null)) : null;
                    }
                }

                RegistroRepProduccion regF = new RegistroRepProduccion();
                regF.NumReg = lstEmpresa.Any() ? lstEmpresa.Count() : 1;
                regF.Emprcodi = fila.Emprcodi;
                regF.Ptomedicodi = fila.Ptomedicodi ?? 0;
                regF.IdUnidad = fila.Grupocodi;
                regF.FechaMaxDemanda = regMD != null ? (regMD.Dregenmdhora != null ? regMD.Dregenmdhora : regMD.Dregenmdhora) : new DateTime();
                regF.StrFechaMaxDemanda = regMD != null ? (regMD.Dregenmdhora != null ? regMD.Dregenmdhora.ToString(ConstantesAppServicio.FormatoFechaHora) : "") : "";
                regF.Emprnomb = fila.Emprnomb;
                regF.UnidadNomb = fila.Gruponomb;
                regF.EHidro = valorHidro / 2.0m;
                regF.StrEHidro = UtilAnexoAPR5.ImprimirValorTotalSinOcultar0Html(regF.EHidro, nfi3);
                regF.ETermo = valorTermo / 2.0m;
                regF.StrETermo = UtilAnexoAPR5.ImprimirValorTotalSinOcultar0Html(regF.ETermo, nfi3);
                regF.ETotal = regF.EHidro + regF.ETermo;
                regF.StrETotal = UtilAnexoAPR5.ImprimirValorTotalSinOcultar0Html(regF.ETotal, nfi3);
                regF.MaxDemanda = (objMD != null && regMD != null) ? (valor) : (null);
                regF.StrMaxDemanda = UtilAnexoAPR5.ImprimirValorTotalSinOcultar0Html(regF.MaxDemanda, nfi3);

                lstRegistros.Add(regF);
            }

            lstRegistros = lstRegistros.OrderBy(x => x.Emprnomb).ThenBy(x => x.UnidadNomb).ToList();

            salida.ListadoRegistros = lstRegistros;
            salida.TotalSein = hayTotal ? (totalSein / 2) : null;
            salida.StrTotalSein = UtilAnexoAPR5.ImprimirValorTotalSinOcultar0Html(salida.TotalSein, nfi3);
            salida.TotalMD = regMD != null ? regMD.Dregenmdsein : null;
            salida.StrTotalMD = UtilAnexoAPR5.ImprimirValorTotalSinOcultar0Html(salida.TotalMD, nfi3);

            return salida;
        }

        /// <summary>
        /// Exporta reporte de produccion de hidrologia
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pathLogo"></param>
        /// <param name="nameFile"></param>
        /// <param name="tipo"></param>
        /// <param name="fecInicial"></param>
        /// <param name="fecFinal"></param>
        public void GenerarExportacionRP(string ruta, string pathLogo, string nameFile, string tipoInfo, int tipo, DateTime fecInicial, DateTime fecFinal)
        {
            DateTime fecMDCoes;
            int hMDcoes;
            ReporteProduccion datosRepGeneracionCoes = ListarReporteProduccion(tipo, fecInicial, fecFinal, ConstantesMedicion.IdTipogrupoCOES, null, null, out fecMDCoes, out hMDcoes);
            ReporteProduccion datosRepGeneracionNoCoes = ListarReporteProduccion(tipo, fecInicial, fecFinal, ConstantesMedicion.IdTipogrupoNoIntegrante, fecMDCoes, hMDcoes, out DateTime fec2, out int hMD2);

            //Descargo archivo segun requieran
            string rutaFile = ruta + nameFile;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarArchivoExcelRP(xlPackage, pathLogo, tipoInfo, datosRepGeneracionCoes, datosRepGeneracionNoCoes, fecInicial, fecFinal);

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Genera la estructura de la tabla general a exportar
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        /// <param name="datosRepGeneracionCoes"></param>
        /// <param name="datosRepGeneracionNoCoes"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        private void GenerarArchivoExcelRP(ExcelPackage xlPackage, string pathLogo, string tipoInfo, ReporteProduccion datosRepGeneracionCoes, ReporteProduccion datosRepGeneracionNoCoes, DateTime fechaInicio, DateTime fechaFin)
        {
            string nameWS = "Reporte de Producción";
            string titulo = "Reporte de Generación - Energía MWh";
            List<RegistroRepProduccion> ListadoRegistrosC = datosRepGeneracionCoes.ListadoRegistros;
            decimal? totalSeinC = datosRepGeneracionCoes.TotalSein;
            decimal? totalMDC = datosRepGeneracionCoes.TotalSein;
            List<RegistroRepProduccion> ListadoRegistrosNC = datosRepGeneracionNoCoes.ListadoRegistros;
            decimal? totalSeinNC = datosRepGeneracionNoCoes.TotalSein;
            decimal? totalMDNC = datosRepGeneracionNoCoes.TotalSein;


            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;

            UtilExcel.AddImageLocal(ws, 1, 0, pathLogo, 120, 70);

            #region  Filtros y Cabecera

            int colIniTitulo = 2;
            int rowIniTitulo = 1;

            int colIniFecha = colIniTitulo + 1;
            int rowIniFecha = rowIniTitulo + 2;

            int colIniTable = colIniTitulo + 1;
            int rowIniTabla = rowIniTitulo + 7;

            int colEmpresa = colIniTable;
            int colCodigo = colIniTable + 1;
            int colUnidad = colIniTable + 2;
            int colEHidro = colIniTable + 3;
            int colETermo = colIniTable + 4;
            int colETotal = colIniTable + 5;
            int colMD = colIniTable + 6;
            int colUltima = colIniTable + 7;

            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            ws.Cells[rowIniFecha, colIniFecha].Value = "Fecha de consulta:";
            ws.Cells[rowIniFecha, colIniFecha + 1].Value = DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaHora);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniFecha, colIniFecha, rowIniFecha, colIniFecha, "Derecha");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniFecha, colIniFecha + 2, rowIniFecha, colIniFecha + 2, "Calibri", 10);

            ws.Cells[rowIniFecha + 1, colIniFecha].Value = "Informacion:";
            ws.Cells[rowIniFecha + 1, colIniFecha + 1].Value = tipoInfo;
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniFecha + 1, colIniFecha, rowIniFecha + 1, colIniFecha, "Derecha");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniFecha + 1, colIniFecha, rowIniFecha + 1, colIniFecha + 1, "Calibri", 10);

            ws.Cells[rowIniFecha + 2, colIniFecha].Value = "Desde:";
            ws.Cells[rowIniFecha + 2, colIniFecha + 1].Value = fechaInicio.ToString(ConstantesAppServicio.FormatoFecha);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniFecha + 2, colIniFecha, rowIniFecha + 2, colIniFecha, "Derecha");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniFecha + 2, colIniFecha, rowIniFecha + 2, colIniFecha + 1, "Calibri", 10);

            ws.Cells[rowIniFecha + 3, colIniFecha].Value = "Hasta:";
            ws.Cells[rowIniFecha + 3, colIniFecha + 1].Value = fechaFin.ToString(ConstantesAppServicio.FormatoFecha);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniFecha + 3, colIniFecha, rowIniFecha + 3, colIniFecha, "Derecha");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniFecha + 3, colIniFecha, rowIniFecha + 3, colIniFecha + 1, "Calibri", 10);

            //Estilos titulo
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colIniTitulo, "Calibri", 16);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colIniTitulo);
            UtilExcel.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colUltima);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colUltima, "Centro");

            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniFecha, colIniFecha, rowIniFecha, colIniFecha + 1, "Calibri", 10);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniFecha, colIniFecha, rowIniFecha + 3, colIniFecha);

            int rowDataT1 = GenerarTablaReporteProd(1, datosRepGeneracionCoes, ws, rowIniTabla, colEmpresa, colCodigo, colUnidad, colEHidro, colETermo, colETotal, colMD, colUltima);

            int rowData = GenerarTablaReporteProd(2, datosRepGeneracionNoCoes, ws, rowDataT1 + 4, colEmpresa, colCodigo, colUnidad, colEHidro, colETermo, colETotal, colMD, colUltima);
            //filter
            //ws.Cells[rowIniTabla, colEmpresa, rowIniTabla, colUltima - 1].AutoFilter = true;
            ws.Cells[rowIniTabla + 1, colIniTable, rowData, colUltima - 1].AutoFitColumns();
            ws.Column(colEmpresa).Width = 42;
            ws.Column(colUnidad).Width = 28;
            ws.Column(colEHidro).Width = 11;
            ws.Column(colETermo).Width = 11;
            ws.Column(colETotal).Width = 11;
            ws.Column(colMD).Width = 11;
            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            ws.View.FreezePanes(rowIniFecha + 7, 1);
        }

        /// <summary>
        /// Genera la estructura de la tabla a exportar
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="datosRep"></param>
        /// <param name="ws"></param>
        /// <param name="rowIniTabla"></param>
        /// <param name="colEmpresa"></param>
        /// <param name="colCodigo"></param>
        /// <param name="colUnidad"></param>
        /// <param name="colEHidro"></param>
        /// <param name="colETermo"></param>
        /// <param name="colETotal"></param>
        /// <param name="colMD"></param>
        /// <param name="colUltima"></param>
        /// <returns></returns>
        private int GenerarTablaReporteProd(int tipo, ReporteProduccion datosRep, ExcelWorksheet ws, int rowIniTabla,
            int colEmpresa, int colCodigo, int colUnidad, int colEHidro, int colETermo, int colETotal, int colMD, int colUltima)
        {
            int numDecimales = 3;
            List<RegistroRepProduccion> ListadoRegistros = datosRep.ListadoRegistros;
            decimal? totalSein = datosRep.TotalSein;
            decimal? totalMD = datosRep.TotalMD;

            if (datosRep.ListadoRegistros.Any() && tipo == 1)
            {
                RegistroRepProduccion objR = datosRep.ListadoRegistros.First();
                if (objR.StrFechaMaxDemanda != "")
                {
                    ws.Cells[rowIniTabla - 1, colEmpresa].Value = "* Máxima Demanda: " + objR.StrFechaMaxDemanda + "h";
                    UtilExcel.CeldasExcelAgrupar(ws, rowIniTabla - 1, colEmpresa, rowIniTabla - 1, colMD);
                    UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla - 1, colEmpresa, rowIniTabla - 1, colMD, "Derecha");
                    UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla - 1, colEmpresa, rowIniTabla - 1, colMD, "Calibri", 10);
                }
            }


            ws.Row(rowIniTabla).Height = 20;
            ws.Cells[rowIniTabla, colEmpresa].Value = tipo == 1 ? "REPORTE DE EMPRESAS COES" : "REPORTE DE EMPRESAS NO COES";
            UtilExcel.CeldasExcelAgrupar(ws, rowIniTabla, colEmpresa, rowIniTabla, colMD);

            ws.Row(rowIniTabla + 1).Height = 26;
            ws.Cells[rowIniTabla + 1, colEmpresa].Value = "Empresa";
            ws.Cells[rowIniTabla + 1, colCodigo].Value = "Código Punto GD";
            ws.Cells[rowIniTabla + 1, colUnidad].Value = "Central/Unidad";
            ws.Cells[rowIniTabla + 1, colEHidro].Value = "E. Hidráulica \r\n MWh.";
            ws.Cells[rowIniTabla + 1, colETermo].Value = "E. Térmica \r\n MWh.";
            ws.Cells[rowIniTabla + 1, colETotal].Value = "Total \r\n MWh.";
            ws.Cells[rowIniTabla + 1, colMD].Value = "Máx. Dem. (*)\r\n MW.";



            //Estilos cabecera
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colEmpresa, rowIniTabla + 1, colUltima - 1, "Calibri", 11);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colEmpresa, rowIniTabla + 1, colUltima - 1, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colEmpresa, rowIniTabla + 1, colUltima - 1, "Centro");
            UtilExcel.CeldasExcelColorFondo(ws, rowIniTabla, colEmpresa, rowIniTabla + 1, colUltima - 1, "#2980B9");
            UtilExcel.CeldasExcelColorTexto(ws, rowIniTabla, colEmpresa, rowIniTabla + 1, colUltima - 1, "#FFFFFF");

            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 2;

            int idEmpresa = -1;
            foreach (var item in ListadoRegistros)
            {
                int numReg = item.NumReg;
                int emprcodi = item.Emprcodi;

                if (idEmpresa != emprcodi)
                {
                    ws.Cells[rowData, colEmpresa].Value = item.Emprnomb;

                    if (numReg > 1)
                        UtilExcel.CeldasExcelAgrupar(ws, rowData, colEmpresa, rowData + numReg - 1, colEmpresa);
                }

                ws.Cells[rowData, colCodigo].Value = item.Ptomedicodi;
                ws.Cells[rowData, colUnidad].Value = item.UnidadNomb;
                ws.Cells[rowData, colEHidro].Value = item.EHidro;
                ws.Cells[rowData, colEHidro].Style.Numberformat.Format = FormatoNumDecimales(numDecimales);
                ws.Cells[rowData, colETermo].Value = item.ETermo;
                ws.Cells[rowData, colETermo].Style.Numberformat.Format = FormatoNumDecimales(numDecimales);
                ws.Cells[rowData, colETotal].Value = item.ETotal;
                ws.Cells[rowData, colETotal].Style.Numberformat.Format = FormatoNumDecimales(numDecimales);
                ws.Cells[rowData, colMD].Value = item.MaxDemanda;
                ws.Cells[rowData, colMD].Style.Numberformat.Format = FormatoNumDecimales(numDecimales);



                idEmpresa = emprcodi;
                rowData++;

            }
            if (!ListadoRegistros.Any())
            {
                rowData++;
                ws.Cells[rowIniTabla + 2, colEmpresa].Value = "No existen registros";
                UtilExcel.CeldasExcelAgrupar(ws, rowIniTabla + 2, colEmpresa, rowData - 1, colMD);
            }

            //Estilos registros
            UtilExcel.CeldasExcelWrapText(ws, rowIniTabla + 1, colEmpresa, rowData - 1, colUltima - 1);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 1, colEmpresa, rowData - 1, colUltima - 1, "Calibri", 8);
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniTabla + 1, colEmpresa, rowData - 1, colUltima - 1, "Centro");
            UtilExcel.BorderCeldasThin(ws, rowIniTabla, colEmpresa, rowData, colUltima - 1, "#000000");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 1, colEmpresa, rowData - 1, colUnidad, "Centro");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 1, colEHidro, rowData - 1, colUltima - 1, "Derecho");

            #endregion

            #region pie de tabla
            if (totalSein != null)
            {
                ws.Cells[rowData, colETotal].Value = totalSein.Value;
                ws.Cells[rowData, colETotal].Style.Numberformat.Format = FormatoNumDecimales(numDecimales);
            }
            if (totalMD != null)
            {
                ws.Cells[rowData, colMD].Value = totalMD.Value;
                ws.Cells[rowData, colMD].Style.Numberformat.Format = FormatoNumDecimales(numDecimales);
            }
            ws.Cells[rowData, colEmpresa].Value = "TOTAL";
            UtilExcel.CeldasExcelColorFondo(ws, rowData, colEmpresa, rowData, colUltima - 1, "#2980B9");
            UtilExcel.CeldasExcelColorTexto(ws, rowData, colEmpresa, rowData, colUltima - 1, "#FFFFFF");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowData, colEmpresa, rowData, colUltima - 1, "Calibri", 8);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowData, colEmpresa, rowData, colEmpresa, "Centro");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowData, colETotal, rowData, colETotal, "Derecho");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowData, colMD, rowData, colMD, "Derecho");

            #endregion
            return rowData;
        }


        #endregion

        #region Reporte historico semanal de hidrologia

        /// <summary>
        /// Devuelve el listado del reporte historico semanal de hidroligia
        /// </summary>
        /// <param name="fecInicial"></param>
        /// <param name="fecFinal"></param>
        /// <returns></returns>
        public List<RegistroRepHidrologia> ListarReporteHidrologia(DateTime fecInicial, DateTime fecFinal)
        {
            List<RegistroRepHidrologia> lstSalida = new List<RegistroRepHidrologia>();
            List<MeMedicion1DTO> lstTemp = new List<MeMedicion1DTO>();

            //Obtenemos la data del reporte para las fechas
            int lectcodi = 20;
            List<MeMedicion1DTO> insumosParaReporte = FactorySic.GetMeMedicion1Repository().GetByCriteria(fecInicial, fecFinal, lectcodi, -1, "-1");

            //obtenemos los puntos de medicion del reporte
            List<MeReporptomedDTO> lstPuntosReporte = FactorySic.GetMeReporptomedRepository().GetByCriteria(ConstantesMigraciones.ReporteHistoricoSemanalHidrologia, -1);

            if (lstPuntosReporte.Any())
            {
                List<int> lstPtomedicodisReporte = lstPuntosReporte.Where(x => x.Ptomedicodi != null).Select(x => x.Ptomedicodi).Distinct().ToList();
                List<MeMedicion1DTO> lstEnReporte = insumosParaReporte.Where(x => lstPtomedicodisReporte.Contains(x.Ptomedicodi)).ToList();

                foreach (MeMedicion1DTO reg in lstEnReporte)
                {
                    MeReporptomedDTO objRep = lstPuntosReporte.Find(x => x.Ptomedicodi == reg.Ptomedicodi);

                    if (objRep != null)
                    {
                        reg.OrigenPtomedidesc = objRep.Ptomedidesc != null ? objRep.Ptomedidesc.Trim() : "";
                        reg.Tipoinfodesc = objRep.Tipoinfoabrev != null ? objRep.Tipoinfoabrev.Trim() : "";
                        reg.MedifechaPto = reg.Medifecha != null ? reg.Medifecha.ToString(ConstantesAppServicio.FormatoFecha) : "";
                        reg.LastdateDesc = reg.Lastdate != null ? reg.Lastdate.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";

                    }
                }

                //Ordenamos el listado
                lstTemp = lstEnReporte.OrderBy(x => x.OrigenPtomedidesc).ThenBy(x => x.Medifecha).ToList();
            }

            foreach (var fila in lstTemp)
            {
                RegistroRepHidrologia reg = new RegistroRepHidrologia();
                reg.H1 = fila.H1;
                reg.MedifechaPto = fila.MedifechaPto;
                reg.OrigenPtomedidesc = fila.OrigenPtomedidesc;
                reg.Tipoinfodesc = fila.Tipoinfodesc;
                reg.Lastuser = fila.Lastuser;
                reg.LastdateDesc = fila.LastdateDesc;

                lstSalida.Add(reg);
            }

            return lstSalida;
        }

        /// <summary>
        ///  Genera el archivo a exportar con el listado del reporte historico de hidrologia
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pathLogo"></param>
        /// <param name="nameFile"></param>
        /// <param name="fecInicial"></param>
        /// <param name="fecFinal"></param>
        public void GenerarExportacionRH(string ruta, string pathLogo, string nameFile, DateTime fecInicial, DateTime fecFinal)
        {
            List<RegistroRepHidrologia> listado = ListarReporteHidrologia(fecInicial, fecFinal);

            ////Descargo archivo segun requieran
            string rutaFile = ruta + nameFile;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarArchivoExcelRH(xlPackage, pathLogo, listado, fecInicial, fecFinal);

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Genera la estructura de la tabla a exportar
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        /// <param name="lstRegistrosTotales"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        private void GenerarArchivoExcelRH(ExcelPackage xlPackage, string pathLogo, List<RegistroRepHidrologia> lstRegistrosTotales, DateTime fechaInicio, DateTime fechaFin)
        {
            string nameWS = "Reporte de Hidrología";
            string titulo = "Información Histórica (Semanal)";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;

            UtilExcel.AddImageLocal(ws, 1, 0, pathLogo, 120, 70);

            #region  Filtros y Cabecera

            int colIniTitulo = 2;
            int rowIniTitulo = 1;

            int colIniFecha = colIniTitulo + 2;
            int rowIniFecha = rowIniTitulo + 2;

            int colIniTable = colIniTitulo + 1;
            int rowIniTabla = rowIniTitulo + 6;

            int colFecha = colIniTable;
            int colElementoH = colIniTable + 1;
            int colUnidad = colIniTable + 2;
            int colValor = colIniTable + 3;
            int colUsuario = colIniTable + 4;
            int colRegistro = colIniTable + 5;
            int colUltima = colIniTable + 6;

            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            ws.Cells[rowIniFecha, colIniFecha].Value = "Fecha de consulta:";
            ws.Cells[rowIniFecha, colIniFecha + 1].Value = DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaHora);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniFecha, colIniFecha, rowIniFecha, colIniFecha, "Derecha");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniFecha, colIniFecha + 2, rowIniFecha, colIniFecha + 2, "Calibri", 10);

            ws.Cells[rowIniFecha + 1, colIniFecha].Value = "Desde:";
            ws.Cells[rowIniFecha + 1, colIniFecha + 1].Value = fechaInicio.ToString(ConstantesAppServicio.FormatoFecha);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniFecha + 1, colIniFecha, rowIniFecha + 1, colIniFecha, "Derecha");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniFecha + 1, colIniFecha, rowIniFecha + 1, colIniFecha + 1, "Calibri", 10);

            ws.Cells[rowIniFecha + 2, colIniFecha].Value = "Hasta:";
            ws.Cells[rowIniFecha + 2, colIniFecha + 1].Value = fechaFin.ToString(ConstantesAppServicio.FormatoFecha);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniFecha + 2, colIniFecha, rowIniFecha + 2, colIniFecha, "Derecha");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniFecha + 2, colIniFecha, rowIniFecha + 2, colIniFecha + 1, "Calibri", 10);

            ws.Row(rowIniTabla).Height = 20;
            ws.Cells[rowIniTabla, colFecha].Value = "Fecha";
            ws.Cells[rowIniTabla, colElementoH].Value = "Elemento Hidrológico";
            ws.Cells[rowIniTabla, colUnidad].Value = "Unidad";
            ws.Cells[rowIniTabla, colValor].Value = "Valor";
            ws.Cells[rowIniTabla, colUsuario].Value = "Usuario";
            ws.Cells[rowIniTabla, colRegistro].Value = "Hora Registro";

            //Estilos titulo
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colIniTitulo, "Calibri", 16);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colIniTitulo);
            UtilExcel.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colUltima);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colUltima, "Centro");

            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniFecha, colIniFecha, rowIniFecha, colIniFecha + 1, "Calibri", 10);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniFecha, colIniFecha, rowIniFecha + 2, colIniFecha);

            //Estilos cabecera
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colFecha, rowIniTabla, colUltima - 1, "Calibri", 11);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colFecha, rowIniTabla, colUltima - 1, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colFecha, rowIniTabla, colUltima - 1, "Centro");
            UtilExcel.CeldasExcelColorFondo(ws, rowIniTabla, colFecha, rowIniTabla, colUltima - 1, "#2980B9");
            UtilExcel.CeldasExcelColorTexto(ws, rowIniTabla, colFecha, rowIniTabla, colUltima - 1, "#FFFFFF");
            UtilExcel.BorderCeldasHair(ws, rowIniTabla, colFecha, rowIniTabla, colUltima - 1, "#000000");

            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;
            int numDecimales = 3;

            foreach (var item in lstRegistrosTotales)
            {
                ws.Cells[rowData, colFecha].Value = item.MedifechaPto;
                ws.Cells[rowData, colElementoH].Value = item.OrigenPtomedidesc;
                ws.Cells[rowData, colUnidad].Value = item.Tipoinfodesc;
                ws.Cells[rowData, colValor].Value = item.H1;
                ws.Cells[rowData, colValor].Style.Numberformat.Format = FormatoNumDecimales(numDecimales);
                ws.Cells[rowData, colUsuario].Value = item.Lastuser;
                ws.Cells[rowData, colRegistro].Value = item.LastdateDesc;


                rowData++;
            }
            if (!lstRegistrosTotales.Any())
            {
                rowData++;
                ws.Cells[rowIniTabla + 1, colFecha].Value = "No existen registros";
                UtilExcel.CeldasExcelAgrupar(ws, rowIniTabla + 1, colFecha, rowData - 1, colRegistro);
            }

            //Estilos registros
            UtilExcel.CeldasExcelWrapText(ws, rowIniTabla + 1, colFecha, rowData - 1, colUltima - 1);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 1, colFecha, rowData - 1, colUltima - 1, "Calibri", 8);
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniTabla + 1, colFecha, rowData - 1, colUltima - 1, "Centro");
            UtilExcel.BorderCeldasThin(ws, rowIniTabla + 1, colFecha, rowData - 1, colUltima - 1, "#000000");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 1, colFecha, rowData - 1, colUltima - 1, "Centro");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 1, colElementoH, rowData - 1, colUltima - 1, "Centro");

            #endregion

            //filter
            //ws.Cells[rowIniTabla, colFecha, rowIniTabla, colUltima - 1].AutoFilter = true;
            ws.Cells[rowIniTabla, colIniTable, rowData, colUltima - 1].AutoFitColumns();
            ws.Column(colElementoH).Width = 35;
            ws.Column(colUsuario).Width = 25;
            ws.Column(colRegistro).Width = 16;
            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            ws.View.FreezePanes(rowIniTabla + 1, 1);
        }

        private string FormatoNumDecimales(int numDecimales)
        {
            string salida = "";
            switch (numDecimales)
            {
                case 1: salida = "#,#0.0"; break;
                case 2: salida = "#,##0.00"; break;
                case 3: salida = "#,###0.000"; break;
                case 4: salida = "#,####0.0000"; break;
                case 5: salida = "#,#####0.00000"; break;
                case 6: salida = "#,######0.000000"; break;
                case 7: salida = "#,#######0.0000000"; break;
                case 8: salida = "#,########0.00000000"; break;
                default:
                    break;
            }
            return salida;
        }
        #endregion

        #endregion


        #region Reserva Primaria y Secundaria para Yupana ()

        public List<PrReservaDTO> ListarReservaSecundaria(DateTime fecha)
        {
            List<PrReservaDTO> lstReserva = ListPrReservas().Where(x => x.Prsvtipo != ConstantesMigraciones.RpfTipo).ToList();
            if (lstReserva.Any())
                ObtenerRegistroActivo(lstReserva, fecha);

            foreach (var reg in lstReserva)
            {
                FormatearReserva(reg);
            }

            lstReserva = lstReserva.OrderByDescending(x => x.Prsvfechavigencia).ToList();

            return lstReserva;
        }

        private void ObtenerRegistroActivo(List<PrReservaDTO> lstReserva, DateTime fecha)
        {
            lstReserva = lstReserva.OrderByDescending(x => x.Prsvfechavigencia).ToList();

            var lstActivos = lstReserva.Where(x => x.Prsvactivo == 1).ToList();
            lstActivos = lstActivos.OrderByDescending(x => x.Prsvfechavigencia).ToList();

            var subirActivo = lstActivos.Find(x => x.Prsvtipo == ConstantesMigraciones.RsftipoSubir && x.Prsvfechavigencia.Date <= fecha); // subir
            var bajarActivo = lstActivos.Find(x => x.Prsvtipo == ConstantesMigraciones.RsftipoBajar && x.Prsvfechavigencia.Date <= fecha); // bajar

            if (subirActivo != null)
                subirActivo.EstadoReserva = "Vigente";

            if (bajarActivo != null)
                bajarActivo.EstadoReserva = "Vigente";
        }

        private void FormatearReserva(PrReservaDTO reg)
        {
            if (reg != null)
            {
                reg.PrsvfeccreacionDesc = reg.Prsvfeccreacion != null ? reg.Prsvfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
                reg.PrsvfecmodificacionDesc = reg.Prsvfecmodificacion != null ? reg.Prsvfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
                reg.PrsvfechavigenciaDesc = reg.Prsvfechavigencia != null ? reg.Prsvfechavigencia.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
                reg.Prsvusucreacion = reg.Prsvusucreacion != null ? reg.Prsvusucreacion.Trim() : string.Empty;
                reg.Prsvusumodificacion = reg.Prsvusumodificacion != null ? reg.Prsvusumodificacion.Trim() : string.Empty;
                reg.TipoReserva = reg.Prsvtipo == ConstantesMigraciones.RpfTipo ? "" : reg.Prsvtipo == ConstantesMigraciones.RsftipoSubir ? "RSF UP" : "RSF DOWN";
                reg.EstadoReserva = reg.Prsvactivo == 1 ? reg.EstadoReserva : "Eliminado";

                if (reg.EstadoReserva == null) // si no se ha asignado como vigente ni eliminado
                    reg.EstadoReserva = "No Vigente";
            }
        }

        /// <summary>
        /// Obtener Datos RSF
        /// </summary>
        /// <param name="fechaVigencia"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public Datos48Reserva ObtenerDatosRSF(DateTime fechaVigencia, DateTime fecha)
        {
            Datos48Reserva salida = new Datos48Reserva();

            List<PrReservaDTO> listaReservas = FactorySic.GetPrReservaRepository().GetByCriteria(fechaVigencia, "2,3");

            if (listaReservas.Any())
            {
                List<PrReservaDTO> lstReserva = ListPrReservas();
                ObtenerRegistroActivo(lstReserva, fecha);

                foreach (var item in listaReservas)
                {
                    var reg = lstReserva.Find(x => x.Prsvcodi == item.Prsvcodi);
                    if (reg != null)
                        item.EstadoReserva = reg.EstadoReserva;
                }

                salida.DatosSubir = listaReservas.Find(x => x.Prsvtipo == ConstantesMigraciones.RsftipoSubir);
                salida.DatosBajar = listaReservas.Find(x => x.Prsvtipo == ConstantesMigraciones.RsftipoBajar);

                FormatearReserva(salida.DatosSubir);
                FormatearReserva(salida.DatosBajar);

                FormatearDatosReserva48(salida.DatosSubir);
                FormatearDatosReserva48(salida.DatosBajar);
            }

            return salida;
        }

        private void FormatearDatosReserva48(PrReservaDTO entidad)
        {
            //lista de valores 
            var listaValores = new List<decimal>();
            listaValores = entidad.Prsvdato.Split(';').Select(x => decimal.Parse(x)).ToList();


            int i = 1;
            foreach (var item in listaValores)
            {
                entidad.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(entidad, item);
                i++;
            }
        }

        /// <summary>
        /// Guardar datos de RPF o RSF
        /// </summary>
        /// <param name="fechaVigencia"></param>
        /// <param name="datosAGuardar"></param>
        /// <param name="usuario"></param>
        /// <param name="opcion"></param>
        /// <returns></returns>
        public int GuardarReserva(DateTime fechaVigencia, Datos48Reserva datosAGuardar, string usuario, int opcion)
        {
            int salida = 1; //solo guarda
            List<PrReservaDTO> listaDatos = new List<PrReservaDTO>();

            Datos48Reserva datosRSFGuardar = FormatearInformacionData(datosAGuardar);

            if (opcion == 1)
            {
                var listaReservas = FactorySic.GetPrReservaRepository().GetByCriteria(fechaVigencia.Date, "2,3");

                if (listaReservas.Any())
                {
                    salida = 2; //error de fecha de vigencia duplicada
                    return salida;
                }

                datosRSFGuardar.DatosSubir.Prsvactivo = 1;
                datosRSFGuardar.DatosSubir.Prsvtipo = ConstantesMigraciones.RsftipoSubir;
                datosRSFGuardar.DatosSubir.Prsvusucreacion = usuario;
                datosRSFGuardar.DatosSubir.Prsvfeccreacion = DateTime.Now;
                datosRSFGuardar.DatosSubir.Prsvfechavigencia = fechaVigencia;

                datosRSFGuardar.DatosBajar.Prsvactivo = 1;
                datosRSFGuardar.DatosBajar.Prsvtipo = ConstantesMigraciones.RsftipoBajar;
                datosRSFGuardar.DatosBajar.Prsvusucreacion = usuario;
                datosRSFGuardar.DatosBajar.Prsvfeccreacion = DateTime.Now;
                datosRSFGuardar.DatosBajar.Prsvfechavigencia = fechaVigencia;
            }
            else
            {
                //obtener registros actualizar
                List<PrReservaDTO> listaReservas = FactorySic.GetPrReservaRepository().GetByCriteria(fechaVigencia, "2,3");

                var datosSubirBD = listaReservas.Find(x => x.Prsvtipo == ConstantesMigraciones.RsftipoSubir);
                var datosBajarBD = listaReservas.Find(x => x.Prsvtipo == ConstantesMigraciones.RsftipoBajar);
                datosSubirBD.Prsvdato = datosRSFGuardar.DatosSubir.Prsvdato;// extraigo los valores obtenidos de la web
                datosBajarBD.Prsvdato = datosRSFGuardar.DatosBajar.Prsvdato;// extraigo los valores obtenidos de la web
                datosSubirBD.Prsvusumodificacion = usuario;
                datosSubirBD.Prsvfecmodificacion = DateTime.Now;
                datosBajarBD.Prsvusumodificacion = usuario;
                datosBajarBD.Prsvfecmodificacion = DateTime.Now;

                datosRSFGuardar.DatosSubir = datosSubirBD;
                datosRSFGuardar.DatosBajar = datosBajarBD;
            }

            if (opcion == 1)
                this.GuardarReservaTransaccional(datosRSFGuardar);//Guardar Nuevo
            else
                this.ActualizarReservaTransaccional(datosRSFGuardar); //editar

            return salida;
        }

        private Datos48Reserva FormatearInformacionData(Datos48Reserva datosAGuardar)
        {
            Datos48Reserva salida = new Datos48Reserva();

            PrReservaDTO datosSubir = datosAGuardar.DatosSubir;
            PrReservaDTO datosBajar = datosAGuardar.DatosBajar;

            var datoSubir = new List<string>();
            var datoBajar = new List<string>();
            for (int i = 1; i <= 48; i++)
            {
                decimal valorSubir = ((decimal?)datosSubir.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(datosSubir, null)).GetValueOrDefault(0);
                decimal valorBajar = ((decimal?)datosBajar.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(datosBajar, null)).GetValueOrDefault(0);
                datoSubir.Add(valorSubir.ToString());
                datoBajar.Add(valorBajar.ToString());
            }
            // Conmvierte la lista en una cadena separada por comas
            string cadenaSubir = string.Join(";", datoSubir);
            string cadenaBajar = string.Join(";", datoBajar);

            datosSubir.Prsvdato = cadenaSubir;
            datosBajar.Prsvdato = cadenaBajar;

            salida.DatosSubir = datosSubir;
            salida.DatosBajar = datosBajar;

            return salida;
        }

        private int GuardarReservaTransaccional(Datos48Reserva datosRSFGuardar)
        {
            int prsvcodi = 0;
            IDbConnection conn = null;
            DbTransaction tran = null;

            try
            {
                conn = FactorySic.GetPrReservaRepository().BeginConnection();
                tran = FactorySic.GetPrReservaRepository().StartTransaction(conn);

                //Guardar datos
                prsvcodi = SavePrReservaTransaccional(datosRSFGuardar.DatosSubir, conn, tran);
                prsvcodi = SavePrReservaTransaccional(datosRSFGuardar.DatosBajar, conn, tran);

                //guardar definitivamente
                tran.Commit();
            }
            catch (Exception ex)
            {
                prsvcodi = 0;
                if (tran != null)
                    tran.Rollback();
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException("Ocurrió un error al momento de guardar los datos.");
            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
            }

            return prsvcodi;
        }

        private int ActualizarReservaTransaccional(Datos48Reserva datosRSFGuardar)
        {
            int prsvcodi = 0;
            IDbConnection conn = null;
            DbTransaction tran = null;

            try
            {
                conn = FactorySic.GetPrReservaRepository().BeginConnection();
                tran = FactorySic.GetPrReservaRepository().StartTransaction(conn);

                //Actualizar datos
                UpdatePrReservaTransaccional(datosRSFGuardar.DatosSubir, conn, tran);
                UpdatePrReservaTransaccional(datosRSFGuardar.DatosBajar, conn, tran);

                //guardar definitivamente
                tran.Commit();
            }
            catch (Exception ex)
            {
                prsvcodi = 0;
                if (tran != null)
                    tran.Rollback();
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException("Ocurrió un error al momento de guardar los dato.");
            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
            }

            return prsvcodi;
        }

        /// <summary>
        /// Actualizar Estado Baja
        /// </summary>
        /// <param name="fechaVigencia"></param>
        /// <param name="modulo"></param>
        /// <param name="usuario"></param>
        /// <exception cref="Exception"></exception>
        public void ActualizarEstadoBaja(DateTime fechaVigencia, int modulo, string usuario)
        {
            try
            {
                if (modulo != ConstantesMigraciones.ReservaPrimaria)
                {
                    List<PrReservaDTO> listaReservas = FactorySic.GetPrReservaRepository().GetByCriteria(fechaVigencia, "2,3");
                    var subir = listaReservas.Find(x => x.Prsvtipo == ConstantesMigraciones.RsftipoSubir);
                    var bajar = listaReservas.Find(x => x.Prsvtipo == ConstantesMigraciones.RsftipoBajar);

                    subir.Prsvactivo = 0;
                    subir.Prsvusumodificacion = usuario;
                    subir.Prsvfecmodificacion = DateTime.Now;
                    bajar.Prsvactivo = 0;
                    bajar.Prsvusumodificacion = usuario;
                    bajar.Prsvfecmodificacion = DateTime.Now;

                    FactorySic.GetPrReservaRepository().ActualizarEstadoRegistro(subir);
                    FactorySic.GetPrReservaRepository().ActualizarEstadoRegistro(bajar);
                }
                else
                {
                    List<PrReservaDTO> listaReservas = FactorySic.GetPrReservaRepository().GetByCriteria(fechaVigencia, "1");
                    var reg = listaReservas.First(); // solo existe un registro activo de RPF con fecha de vigencia
                    if (reg != null)
                    {
                        reg.Prsvactivo = 0;
                        reg.Prsvusumodificacion = usuario;
                        reg.Prsvfecmodificacion = DateTime.Now;
                        reg.Prsvusumodificacion = usuario;
                        reg.Prsvfecmodificacion = DateTime.Now;

                        FactorySic.GetPrReservaRepository().ActualizarEstadoRegistro(reg);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Listar Reserva Primaria
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<PrReservaDTO> ListarReservaPrimaria(DateTime fecha)
        {
            List<PrReservaDTO> lstReserva = ListPrReservas();
            lstReserva = lstReserva.Where(x => x.Prsvtipo == ConstantesMigraciones.RpfTipo).ToList();
            var lstActivos = lstReserva.Where(x => x.Prsvactivo == 1).ToList();
            lstActivos = lstActivos.OrderByDescending(x => x.Prsvfechavigencia).ToList();

            var regActivo = lstActivos.Find(x => x.Prsvfechavigencia.Date <= fecha); // subir
            if (regActivo != null)
                regActivo.EstadoReserva = "Vigente";

            foreach (var reg in lstReserva)
            {
                FormatearReserva(reg);
            }

            lstReserva = lstReserva.OrderByDescending(x => x.Prsvfechavigencia).ToList();

            return lstReserva;
        }

        /// <summary>
        /// Guardar Reserva Primaria
        /// </summary>
        /// <param name="fechaVigencia"></param>
        /// <param name="datosAGuardar"></param>
        /// <param name="usuario"></param>
        /// <param name="opcion"></param>
        /// <returns></returns>
        public int GuardarReservaPrimaria(DateTime fechaVigencia, PrReservaDTO datosAGuardar, string usuario, int opcion)
        {
            int salida = 1; //solo guardar
            var datoSubir = new List<string>();
            for (int i = 1; i <= 48; i++)
            {
                decimal valordata = ((decimal?)datosAGuardar.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(datosAGuardar, null)).GetValueOrDefault(0);
                datoSubir.Add(valordata.ToString());
            }
            // Conmvierte la lista en una cadena separada por comas
            string cadenaSubir = string.Join(";", datoSubir);
            datosAGuardar.Prsvdato = cadenaSubir;

            if (opcion == 1)
            {
                var listaReservas = FactorySic.GetPrReservaRepository().GetByCriteria(fechaVigencia.Date, "1");

                if (listaReservas.Any())
                {
                    salida = 2; //error de fecha de vigencia duplicada
                    return salida;
                }

                datosAGuardar.Prsvactivo = 1;
                datosAGuardar.Prsvtipo = ConstantesMigraciones.RpfTipo;
                datosAGuardar.Prsvusucreacion = usuario;
                datosAGuardar.Prsvfeccreacion = DateTime.Now;
                datosAGuardar.Prsvfechavigencia = fechaVigencia;

            }
            else
            {
                //obtener registros actualizar
                List<PrReservaDTO> listaReservas = FactorySic.GetPrReservaRepository().GetByCriteria(fechaVigencia, "1");
                var datosBD = listaReservas.Find(x => x.Prsvtipo == ConstantesMigraciones.RpfTipo);
                datosBD.Prsvdato = datosAGuardar.Prsvdato;// extraigo los valores obtenidos de la web
                datosBD.Prsvusumodificacion = usuario;
                datosBD.Prsvfecmodificacion = DateTime.Now;
                datosAGuardar = datosBD;

            }

            if (opcion == 1)
                this.SavePrReserva(datosAGuardar);//Guardar Nuevo
            else
                this.UpdatePrReserva(datosAGuardar); //editar

            return salida;
        }

        /// <summary>
        /// Obtener Datos RPF
        /// </summary>
        /// <param name="fechaVigencia"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public PrReservaDTO ObtenerDatosRPF(DateTime fechaVigencia, DateTime fecha)
        {
            PrReservaDTO salida = new PrReservaDTO();

            List<PrReservaDTO> listaReservas = FactorySic.GetPrReservaRepository().GetByCriteria(fechaVigencia, "1");
            var entidadRpf = listaReservas.First();

            if (entidadRpf != null)
            {
                List<PrReservaDTO> lstReservaBD = ListPrReservas().Where(x => x.Prsvtipo == ConstantesMigraciones.RpfTipo && x.Prsvactivo == 1).ToList();
                lstReservaBD = lstReservaBD.OrderByDescending(x => x.Prsvfechavigencia).ToList();
                var regActivo = lstReservaBD.Find(x => x.Prsvfechavigencia.Date <= fecha);
                if (regActivo != null)
                    regActivo.EstadoReserva = "Vigente"; //asignar al vigente según la fecha de consulta

                var reg = lstReservaBD.Find(x => x.Prsvcodi == entidadRpf.Prsvcodi);
                if (reg != null)
                    entidadRpf.EstadoReserva = reg.EstadoReserva;

                FormatearReserva(entidadRpf);
                FormatearDatosReserva48(entidadRpf);

                salida = entidadRpf;
            }

            return salida;
        }

        #endregion

    }
}
using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System.Linq;
using COES.Servicios.Aplicacion.RegionesDeSeguridad.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;

namespace COES.Servicios.Aplicacion.RegionesDeSeguridad
{
    public class RegionesDeSeguridadAppServicio : AppServicioBase
    {
        CortoPlazoAppServicio servicioCp = new CortoPlazoAppServicio();
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(RegionesDeSeguridadAppServicio));

        #region Métodos Tabla SEG_COORDENADA

        /// <summary>
        /// Graba Coordenada de Region de Seguridad y ademas crea Region de Seguridad de Congestion
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int GrabarCoordenadaRegionSeguridad(SegCoordenadaDTO entity, string usuario)
        {
            int resultado = 0;
            if (entity.Segcocodi == 0) // Nuevo
            {
                var lultimaCoordenada = GetByCriteriaSegCoordenadas((int)entity.Regcodi, (int)entity.Segcotipo, ConstantesAppServicio.ParametroDefecto).OrderByDescending(x => x.Segconro).ToList();
                if (lultimaCoordenada.Count > 0)
                {
                    var ultimaCoordenada = lultimaCoordenada.First();
                    var ultimoNro = ultimaCoordenada != null ? ultimaCoordenada.Segconro + 1 : 1;
                    entity.Segconro = ultimoNro;
                }
                else
                    entity.Segconro = 1;
                resultado = SaveSegCoordenada(entity);
                entity.Segcocodi = resultado;
                if (entity.Zoncodi != 0) // Crear Region en Congestiones
                {
                    var reg = GetByIdSegRegion((int)entity.Regcodi);
                    string strTipo = (entity.Segcotipo == 0) ? "MIN" : (entity.Segcotipo == 1) ? "MED" : "MAX"; 
                    decimal? deltaGen = entity.Segcogener2 - entity.Segcogener1;
                    decimal? deltaFlu = entity.Segcoflujo2 - entity.Segcoflujo1;

                    CmRegionseguridadDTO cmregion = new CmRegionseguridadDTO();
                    cmregion.Regsegestado = entity.Segcoestado;
                    cmregion.Regsegdirec = (entity.Zoncodi == 1) ? "2" : "1";
                    cmregion.Regsegvalorm = (deltaFlu == 0) ? 1000 : deltaGen / deltaFlu;
                    cmregion.Regsegusucreacion = usuario;
                    cmregion.Regsegusumodificacion = usuario;
                    cmregion.Regsegfeccreacion = DateTime.Now;
                    cmregion.Regsegfecmodificacion = DateTime.Now;
                    cmregion.Regsegnombre = reg.Regnombre.Trim() + "-" + strTipo + "-" + entity.Segconro;
                    cmregion.Regsegcodi = FactorySic.GetCmRegionseguridadRepository().Save(cmregion);

                    entity.Regsegcodi = cmregion.Regsegcodi;
                    UpdateSegCoordenada(entity);
                }
            }
            else //Editar
            {
                var restriccion = GetByIdSegCoordenada(entity.Segcocodi);
                restriccion.Segcoflujo1 = entity.Segcoflujo1;
                restriccion.Segcogener1 = entity.Segcogener1;
                restriccion.Segcoflujo2 = entity.Segcoflujo2;
                restriccion.Segcogener2 = entity.Segcogener2;
                restriccion.Segcofecmodificacion = entity.Segcofecmodificacion;
                restriccion.Segcousumodificacion = entity.Segcousumodificacion;
                restriccion.Zoncodi = entity.Zoncodi;

                //if (restriccion.Segcotipo != entity.Segcotipo)
                //{
                //    restriccion.Segcotipo = entity.Segcotipo;
                //    var ultimaCoordenada = GetByCriteriaSegCoordenadas((int)entity.Regcodi, (int)entity.Segcotipo, ConstantesAppServicio.ParametroDefecto).OrderByDescending(x => x.Segconro).ToList().First();
                //    restriccion.Segconro = ultimaCoordenada != null ? ultimaCoordenada.Segconro + 1 : 1;

                //}
                UpdateSegCoordenada(restriccion);

                ///Actualiza region de congestion
                var regCongest = FactorySic.GetCmRegionseguridadRepository().GetById(restriccion.Regsegcodi);
                if (regCongest != null)
                {
                    decimal? deltaGen = restriccion.Segcogener2 - restriccion.Segcogener1;
                    decimal? deltaFlu = restriccion.Segcoflujo2 - restriccion.Segcoflujo1;
                    if (entity.Zoncodi != 0)
                    {
                        regCongest.Regsegestado = restriccion.Segcoestado;
                        regCongest.Regsegdirec = (restriccion.Zoncodi == 1) ? "2" : "1";
                        regCongest.Regsegvalorm = (deltaFlu == 0) ? 1000 : deltaGen / deltaFlu;
                    }
                    else
                    {
                        regCongest.Regsegdirec = "0";
                        regCongest.Regsegestado = ConstantesAppServicio.Baja;
                        regCongest.Regsegvalorm = null;
                    }

                    regCongest.Regsegusucreacion = usuario;
                    regCongest.Regsegusumodificacion = usuario;
                    regCongest.Regsegfeccreacion = DateTime.Now;
                    regCongest.Regsegfecmodificacion = DateTime.Now;
                    FactorySic.GetCmRegionseguridadRepository().Update(regCongest);
                }
                else
                {
                    if (entity.Zoncodi != 0)
                    {
                        var reg = GetByIdSegRegion((int)entity.Regcodi);
                        string strTipo = (restriccion.Segcotipo == 0) ? "MIN" : (restriccion.Segcotipo == 1) ? "MED" : "MAX";
                        decimal? deltaGen = entity.Segcogener2 - entity.Segcogener1;
                        decimal? deltaFlu = entity.Segcoflujo2 - entity.Segcoflujo1;

                        CmRegionseguridadDTO cmregion = new CmRegionseguridadDTO();
                        cmregion.Regsegestado = entity.Segcoestado;
                        cmregion.Regsegdirec = (entity.Zoncodi == 1) ? "2" : "1";
                        cmregion.Regsegvalorm = (deltaFlu == 0) ? 1000 : deltaGen / deltaFlu;
                        cmregion.Regsegusucreacion = usuario;
                        cmregion.Regsegusumodificacion = usuario;
                        cmregion.Regsegfeccreacion = DateTime.Now;
                        cmregion.Regsegfecmodificacion = DateTime.Now;
                        cmregion.Regsegnombre = reg.Regnombre.Trim() + "-" + strTipo + "-" + restriccion.Segconro;
                        cmregion.Regsegcodi = FactorySic.GetCmRegionseguridadRepository().Save(cmregion);
                        entity.Regsegcodi = cmregion.Regsegcodi;
                        UpdateSegCoordenada(entity);
                    }
                }

            }

            return resultado;
        }
        /// <summary>
        /// Inserta un registro de la tabla SEG_COORDENADA
        /// </summary>
        public int SaveSegCoordenada(SegCoordenadaDTO entity)
        {
            try
            {
                return FactorySic.GetSegCoordenadaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SEG_COORDENADA
        /// </summary>
        public void UpdateSegCoordenada(SegCoordenadaDTO entity)
        {
            try
            {
                FactorySic.GetSegCoordenadaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SEG_COORDENADA
        /// </summary>
        public void DeleteSegCoordenada(int segcocodi)
        {
            try
            {
                FactorySic.GetSegCoordenadaRepository().Delete(segcocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina Coordenada, cambia de estado a Baja y actualiza estado de Region de Seguridad de Congestiones
        /// </summary>
        /// <param name="segcocodi"></param>
        /// <param name="estado"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int EliminarSegCoordenada(int segcocodi, string estado, string usuario)
        {
            int resultado = 0;
            DateTime fecha = DateTime.Now;
            SegCoordenadaDTO registro = GetByIdSegCoordenada(segcocodi);
            registro.Segcofecmodificacion = fecha;
            registro.Segcousumodificacion = usuario;
            registro.Segcoestado = estado;
            UpdateSegCoordenada(registro);
            if (registro.Regsegcodi > 0) // Actualizar Region de Seguridad de Consgestiones
            {
                var regRegionCong = FactorySic.GetCmRegionseguridadRepository().GetById(registro.Regsegcodi);
                regRegionCong.Regsegestado = estado;
                regRegionCong.Regsegusumodificacion = usuario;
                regRegionCong.Regsegfecmodificacion = fecha;
                FactorySic.GetCmRegionseguridadRepository().Update(regRegionCong);
            }
            return resultado;
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SEG_COORDENADA
        /// </summary>
        public SegCoordenadaDTO GetByIdSegCoordenada(int segcocodi)
        {
            return FactorySic.GetSegCoordenadaRepository().GetById(segcocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SEG_COORDENADA
        /// </summary>
        public List<SegCoordenadaDTO> ListSegCoordenadas()
        {
            return FactorySic.GetSegCoordenadaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SegCoordenada
        /// </summary>
        public List<SegCoordenadaDTO> GetByCriteriaSegCoordenadas(int regcodi, int idtipo, string estado)
        {
            List<SegCoordenadaDTO> lista = FactorySic.GetSegCoordenadaRepository().GetByCriteria(regcodi, idtipo);
            if (estado != ConstantesAppServicio.ParametroDefecto)
            {
                lista = lista.Where(x => x.Segcoestado == estado).ToList();
            }

            foreach (var reg in lista)
            {
                switch (reg.Segcotipo)
                {
                    case ConstantesRegionSeguridad.MinDemanda:
                        reg.TipoDesc = ConstantesRegionSeguridad.MinimaDemanda;
                        break;

                    case ConstantesRegionSeguridad.MedDemanda:
                        reg.TipoDesc = ConstantesRegionSeguridad.MediaDemanda;
                        break;

                    case ConstantesRegionSeguridad.MaxDemanda:
                        reg.TipoDesc = ConstantesRegionSeguridad.MaximaDemanda;
                        break;
                }
            }
            return lista;
        }

        #endregion

        #region Métodos Tabla SEG_REGION

        /// <summary>
        /// Inserta un registro de la tabla SEG_REGION
        /// </summary>
        public int SaveSegRegion(SegRegionDTO entity)
        {
            try
            {
                return FactorySic.GetSegRegionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SEG_REGION
        /// </summary>
        public void UpdateSegRegion(SegRegionDTO entity)
        {
            try
            {
                FactorySic.GetSegRegionRepository().Update(entity);
                //Actualiza Congestion
                List<SegCoordenadaDTO> lista = FactorySic.GetSegCoordenadaRepository().GetByCriteria(entity.Regcodi, -1);
                List<CmRegionseguridadDTO> listaRegiones = FactorySic.GetCmRegionseguridadRepository().GetByCriteriaCoordenada(entity.Regcodi);
                string nombre;
                foreach (var reg in lista)
                {
                    if(reg.Zoncodi > 0)
                    {
                        string strTipo = (reg.Segcotipo == 0) ? "MIN" : (reg.Segcotipo == 1) ? "MED" : "MAX";
                        var findReg = listaRegiones.Find(x => x.Regsegcodi == reg.Regsegcodi);
                        if(findReg != null)
                        {
                            findReg.Regsegusumodificacion = reg.Segcousumodificacion;
                            findReg.Regsegfecmodificacion = DateTime.Now;
                            findReg.Regsegnombre = entity.Regnombre.Trim() + "-" + strTipo + "-" + reg.Segconro;
                            FactorySic.GetCmRegionseguridadRepository().Update(findReg);
                        }
                        
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
        /// Elimina un registro de la tabla SEG_REGION
        /// </summary>
        public void DeleteSegRegion(int regcodi, string estado, string usuario)
        {
            try
            {
                SegRegionDTO region = GetByIdSegRegion(regcodi);
                region.Regestado = estado;
                region.Regfecmodificacion = DateTime.Now;
                region.Regusumodificacion = usuario;
                UpdateSegRegion(region);

                ///Actualizar regiones de congestion
                FactorySic.GetSegRegionRepository().ActualizarCongestion(regcodi, usuario, estado);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SEG_REGION
        /// </summary>
        public SegRegionDTO GetByIdSegRegion(int regcodi)
        {
            return FactorySic.GetSegRegionRepository().GetById(regcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SEG_REGION
        /// </summary>
        public List<SegRegionDTO> ListSegRegions()
        {
            return FactorySic.GetSegRegionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SegRegion
        /// </summary>
        public List<SegRegionDTO> GetByCriteriaSegRegions(string estado)
        {

            List<SegRegionDTO> lista = FactorySic.GetSegRegionRepository().GetByCriteria().OrderBy(x => x.Regcodi).ToList();
            var listaTotal = FactorySic.GetSegCoordenadaRepository().Totalrestriccion();
            if (estado != ConstantesAppServicio.ParametroDefecto)
            {
                lista = lista.Where(x => x.Regestado == estado).OrderBy(x => x.Regcodi).ToList();
            }

            foreach (var reg in lista)
            {
                reg.Regnombre = reg.Regnombre != null ? reg.Regnombre.Trim() : string.Empty;
                reg.RegfecmodificacionDesc = reg.Regfecmodificacion != null ? reg.Regfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
                reg.RegestadoDesc = Util.EstadoDescripcion(reg.Regestado);
                reg.Listatipo = listaTotal.Where(x => x.Regcodi == reg.Regcodi).ToList();

            }
            return lista;
        }

        /// <summary>
        /// Graba los datos de la carga excel
        /// </summary>
        /// <param name="lregion"></param>
        /// <param name="lcoordenada"></param>
        /// <param name="lequipo"></param>
        /// <param name="lgrupo"></param>
        /// <param name="lCmRegion"></param>
        /// <param name="lCmRegDet"></param>
        /// <returns></returns>
        public int CargaFormatoRegiones(List<SegRegionDTO> lregion,List<SegCoordenadaDTO>lcoordenada,List<SegRegionequipoDTO> lequipo, List<SegRegiongrupoDTO> lgrupo,
            List<CmRegionseguridadDTO> lCmRegion, List<CmRegionseguridadDetalleDTO> lCmRegDet)
        {
            int retorno = 1;
            int idRegion, idCorrdenada,idCmreg;

            if(lregion != null)
                foreach(var reg in lregion)
                {
                    idRegion = SaveSegRegion(reg);
                    reg.Regcodi = reg.Regcodi;
                    reg.Regcodi = idRegion;
                    if(lcoordenada != null)
                    {
                        var lcoor = lcoordenada.Where(x => x.RegcodiExcel == reg.RegcodiExcel);
                        foreach (var p in lcoor)
                        {
                            p.Regcodi = idRegion;
                        }
                    }
                    if(lequipo != null)
                    {
                        var lequ = lequipo.Where(x => x.RegcodiExcel == reg.RegcodiExcel);
                        foreach (var p in lequ)
                        {
                            p.Regcodi = idRegion;
                        }
                    }
                    if (lgrupo != null)
                    {
                        var lgru = lgrupo.Where(x => x.RegcodiExcel == reg.RegcodiExcel);
                        foreach (var p in lgru)
                        {
                            p.Regcodi = idRegion;
                        }

                    }
                }
            if(lCmRegion != null)            
                foreach(var reg in lCmRegion)
                {
                    idCmreg = FactorySic.GetCmRegionseguridadRepository().Save(reg);
                    reg.Regsegcodi = idCmreg;
                    if(lCmRegDet != null)
                    {
                        var lcmrdet = lCmRegDet.Where(x => x.RegsegcodiExcel == reg.RegsegcodiExcel);
                        foreach (var p in lcmrdet)
                        {
                            p.Regsegcodi = idCmreg;
                        }
                    }
                    if(lcoordenada != null)
                    {
                        var lcoor = lcoordenada.Where(x => x.RegsegcodiExcel == reg.RegsegcodiExcel);
                        foreach (var p in lcoor)
                        {
                            p.Regsegcodi = idCmreg;
                        }
                    }
                }
            if(lcoordenada != null)
                foreach(var reg in lcoordenada)
                {
                    idCorrdenada = SaveSegCoordenada(reg);
                    reg.Segcocodi = idCorrdenada;
                }
            if(lCmRegDet != null)
                foreach(var reg in lCmRegDet)
                {
                    servicioCp.SaveCmRegionseguridadDetalle(reg);
                }
            if(lequipo != null)
                foreach(var reg in lequipo)
                {
                    SaveSegRegionequipo(reg);
                }
            if(lgrupo != null)
                foreach (var reg in lgrupo)
                {
                    SaveSegRegiongrupo(reg);
                }

            return retorno;
        }

        #endregion

        #region Métodos Tabla SEG_REGIONEQUIPO

        /// <summary>
        /// Inserta un registro de la tabla SEG_REGIONEQUIPO
        /// </summary>
        public int SaveSegRegionequipo(SegRegionequipoDTO entity)
        {
            try
            {
                int resultado = 1;

                List<SegRegionequipoDTO> list = GetByCriteriaSegRegionequipos(entity.Regcodi, entity.Segcotipo).Where(x => x.Equicodi == entity.Equicodi).ToList();

                if (list.Count == 0)
                {
                    FactorySic.GetSegRegionequipoRepository().Save(entity);
                }
                else
                {
                    resultado = 2;
                }

                return resultado;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SEG_REGIONEQUIPO
        /// </summary>
        public void UpdateSegRegionequipo(SegRegionequipoDTO entity)
        {
            try
            {
                FactorySic.GetSegRegionequipoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SEG_REGIONEQUIPO
        /// </summary>
        public void DeleteSegRegionequipo(int regcodi, int equicodi, int segcotipo)
        {
            try
            {
                FactorySic.GetSegRegionequipoRepository().Delete(regcodi, equicodi, segcotipo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SEG_REGIONEQUIPO
        /// </summary>
        public SegRegionequipoDTO GetByIdSegRegionequipo()
        {
            return FactorySic.GetSegRegionequipoRepository().GetById();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SEG_REGIONEQUIPO
        /// </summary>
        public List<SegRegionequipoDTO> ListSegRegionequipos()
        {
            return FactorySic.GetSegRegionequipoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SegRegionequipo
        /// </summary>
        public List<SegRegionequipoDTO> GetByCriteriaSegRegionequipos(int regcodi, int tipo)
        {
            return FactorySic.GetSegRegionequipoRepository().GetByCriteria(regcodi, tipo);
        }

        /// <summary>
        /// Graba el equipo seleccionado en la region de seguridad, ademas graba el equipo en la region de seguridad de congestion
        /// </summary>
        /// <param name="entity"></param>
        public int GrabarEquipoRegionSeguridad(SegRegionequipoDTO entity, int tipoEquipo)
        {
            List<EqEquipoDTO> listaUnidades = new List<EqEquipoDTO>();
            int resultado = SaveSegRegionequipo(entity);
            //Grabar equipo en las regiones de congestion relacionadas
            List<CmRegionseguridadDTO> listaRegiones = FactorySic.GetCmRegionseguridadRepository().GetByCriteriaCoordenada(entity.Regcodi);
            if(tipoEquipo == 3) // Es Central Hidraulica
                listaUnidades = FactorySic.GetEqEquipoRepository().ListarUnidadesxPlanta2(entity.Equicodi.ToString(), 2);
            foreach (var reg in listaRegiones)
            {
                if (tipoEquipo != 3)//No es Central Hidraulica
                {
                    CmRegionseguridadDetalleDTO regdet = new CmRegionseguridadDetalleDTO();
                    regdet.Equicodi = entity.Equicodi;
                    regdet.Regsegcodi = reg.Regsegcodi;
                    regdet.Regsegfeccreacion = DateTime.Now;
                    regdet.Regsegusucreacion = reg.Regsegusumodificacion;
                    servicioCp.SaveCmRegionseguridadDetalle(regdet);
                }
                else
                {
                    foreach(var p in listaUnidades)
                    {
                        CmRegionseguridadDetalleDTO regdet = new CmRegionseguridadDetalleDTO();
                        regdet.Equicodi = p.Equicodi;
                        regdet.Regsegcodi = reg.Regsegcodi;
                        regdet.Regsegfeccreacion = DateTime.Now;
                        regdet.Regsegusucreacion = reg.Regsegusumodificacion;
                        servicioCp.SaveCmRegionseguridadDetalle(regdet);
                    }
                }
            }
            return resultado;
        }

        /// <summary>
        /// Graba el grupo seleccionado en la region de seguridad, ademas graba el equipo en la region de seguridad de congestion
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int GrabarGrupoRegionSeguridad(SegRegiongrupoDTO entity)
        {
            int resultado = SaveSegRegiongrupo(entity);
            var equiposTermicos = FactorySic.GetPrGrupoRepository().ListaEquiposXModoOperacion(entity.Grupocodi.ToString());
            List<CmRegionseguridadDTO> listaRegiones = FactorySic.GetCmRegionseguridadRepository().GetByCriteriaCoordenada(entity.Regcodi);
            foreach (var reg in listaRegiones)
            {
                foreach (var e in equiposTermicos)
                {
                    CmRegionseguridadDetalleDTO regdet = new CmRegionseguridadDetalleDTO();
                    regdet.Equicodi = e.Equicodi;
                    regdet.Regsegcodi = reg.Regsegcodi;
                    regdet.Regsegfeccreacion = DateTime.Now;
                    regdet.Regsegusucreacion = reg.Regsegusumodificacion;
                    FactorySic.GetCmRegionseguridadDetalleRepository().Save(regdet);
                }
            }

            return resultado;
        }

        /// <summary>
        /// Elimina equipo de la region de seguridad, ademas elimina en la region de seguridad de congestiones
        /// </summary>
        /// <param name="regcodi"></param>
        /// <param name="equicodi"></param>
        /// <param name="segcotipo"></param>
        public void EliminarEquipoRegionSeguridad(int regcodi, int equicodi, int segcotipo)
        {
            List<EqEquipoDTO> listaUnidades = new List<EqEquipoDTO>();
            DeleteSegRegionequipo(regcodi, equicodi, segcotipo);
            var equipo = FactorySic.GetEqEquipoRepository().GetById(equicodi);
            List<CmRegionseguridadDTO> listaRegiones = FactorySic.GetCmRegionseguridadRepository().GetByCriteriaCoordenada(regcodi);
            if (equipo.Famcodi == 4) // Es Central Hidraulica
                listaUnidades = FactorySic.GetEqEquipoRepository().ListarUnidadesxPlanta2(equicodi.ToString(), 2);
            foreach (var reg in listaRegiones)
            {
                if (equipo.Famcodi != 4)//No es Central Hidraulica
                {
                    servicioCp.DeleteCmRegionseguridadDetalle(reg.Regsegcodi, equicodi);
                }
                else
                {
                    foreach (var p in listaUnidades)
                    {
                        servicioCp.DeleteCmRegionseguridadDetalle(reg.Regsegcodi, p.Equicodi);
                    }
                }
            }
        }

        /// <summary>
        /// Elimina grupo de la region de seguridad, ademas elimina en la region de seguridad de congestiones
        /// </summary>
        /// <param name="regcodi"></param>
        /// <param name="grupocodi"></param>
        /// <param name="segcotipo"></param>
        public void EliminarGrupoRegionSeguridad(int regcodi, int grupocodi, int segcotipo)
        {
            DeleteSegRegiongrupo(regcodi, grupocodi, segcotipo);
            var equiposTermicos = FactorySic.GetPrGrupoRepository().ListaEquiposXModoOperacion(grupocodi.ToString());
            List<CmRegionseguridadDTO> listaRegiones = FactorySic.GetCmRegionseguridadRepository().GetByCriteriaCoordenada(regcodi);
            foreach (var reg in listaRegiones)
            {
                foreach (var p in equiposTermicos)
                {
                    servicioCp.DeleteCmRegionseguridadDetalle(reg.Regsegcodi, p.Equicodi);
                }
            }
        }

        #endregion

        #region Métodos Tabla SEG_REGIONGRUPO

        /// <summary>
        /// Inserta un registro de la tabla SEG_REGIONGRUPO
        /// </summary>
        public int SaveSegRegiongrupo(SegRegiongrupoDTO entity)
        {
            try
            {
                int resultado = 1;

                List<SegRegiongrupoDTO> list = GetByCriteriaSegRegiongrupos(entity.Regcodi, entity.Segcotipo).Where(x => x.Grupocodi == entity.Grupocodi).ToList();

                if (list.Count == 0)
                {
                    FactorySic.GetSegRegiongrupoRepository().Save(entity);
                }
                else
                {
                    resultado = 2;
                }

                return resultado;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SEG_REGIONGRUPO
        /// </summary>
        public void UpdateSegRegiongrupo(SegRegiongrupoDTO entity)
        {
            try
            {
                FactorySic.GetSegRegiongrupoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SEG_REGIONGRUPO
        /// </summary>
        public void DeleteSegRegiongrupo(int regcodi, int grupocodi, int segcotipo)
        {
            try
            {
                FactorySic.GetSegRegiongrupoRepository().Delete(regcodi,grupocodi,segcotipo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SEG_REGIONGRUPO
        /// </summary>
        public SegRegiongrupoDTO GetByIdSegRegiongrupo()
        {
            return FactorySic.GetSegRegiongrupoRepository().GetById();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SEG_REGIONGRUPO
        /// </summary>
        public List<SegRegiongrupoDTO> ListSegRegiongrupos()
        {
            return FactorySic.GetSegRegiongrupoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SegRegiongrupo
        /// </summary>
        public List<SegRegiongrupoDTO> GetByCriteriaSegRegiongrupos(int regcodi, int tipo)
        {
            return FactorySic.GetSegRegiongrupoRepository().GetByCriteria(regcodi,tipo);
        }

        #endregion

        #region Métodos Tabla SEG_ZONA

        /// <summary>
        /// Inserta un registro de la tabla SEG_ZONA
        /// </summary>
        public void SaveSegZona(SegZonaDTO entity)
        {
            try
            {
                FactorySic.GetSegZonaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SEG_ZONA
        /// </summary>
        public void UpdateSegZona(SegZonaDTO entity)
        {
            try
            {
                FactorySic.GetSegZonaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SEG_ZONA
        /// </summary>
        public void DeleteSegZona()
        {
            try
            {
                FactorySic.GetSegZonaRepository().Delete();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SEG_ZONA
        /// </summary>
        public SegZonaDTO GetByIdSegZona()
        {
            return FactorySic.GetSegZonaRepository().GetById();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SEG_ZONA
        /// </summary>
        public List<SegZonaDTO> ListSegZonas()
        {
            return FactorySic.GetSegZonaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SegZona
        /// </summary>
        public List<SegZonaDTO> GetByCriteriaSegZonas()
        {
            return FactorySic.GetSegZonaRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla Cm_RegionSeguridadDetalle
        /// <summary>
        /// Permite obtener los equipos
        /// </summary>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        public List<CmRegionseguridadDetalleDTO> ObtenerEquiposConjunto(int tipo)
        {
            List<CmRegionseguridadDetalleDTO> lista = new List<CmRegionseguridadDetalleDTO>();
            switch (tipo)
            {
                case 1:
                case 2:
                    lista = FactorySic.GetCmRegionseguridadDetalleRepository().ObtenerEquiposLinea(tipo);
                    break;
                case 3:
                    lista = FactorySic.GetCmRegionseguridadDetalleRepository().ObtenerEquiposCentral();
                    break;
                case 4:
                    lista = FactorySic.GetCmRegionseguridadDetalleRepository().ObtenerModoOperacion();
                    break;
            }

            return lista;
        }

        #endregion

        #region Tabla EqEquipo

        public List<PrGrupoDTO> ListaUnidades(string recurcodis)
        {
            List<PrGrupoDTO> listaUnidades = FactorySic.GetPrGrupoRepository().ListaEquiposXModoOperacion(recurcodis);
           return listaUnidades;
        }

        #endregion
    }
}

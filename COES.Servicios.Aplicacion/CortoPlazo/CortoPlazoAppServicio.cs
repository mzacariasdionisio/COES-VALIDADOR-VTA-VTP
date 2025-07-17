using System;
using System.Collections.Generic;
using System.Linq;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using System.Configuration;
using COES.Framework.Base.Tools;
using COES.Base.Tools;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using COES.Servicios.Aplicacion.OperacionesVarias;
using System.Globalization;
using COES.Servicios.Aplicacion.YupanaContinuo;
using System.Runtime.Remoting;

namespace COES.Servicios.Aplicacion.CortoPlazo
{
    public class CortoPlazoAppServicio
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CortoPlazoAppServicio));

        private readonly CostoMarginalAppServicio _servicioCmg;

        public CortoPlazoAppServicio()
        {
            _servicioCmg = new CostoMarginalAppServicio();
        }

        #region Métodos Tabla EQ_RELACION

        /// <summary>
        /// Permite listar las empresas de la relación
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasRelacion()
        {
            return FactorySic.GetEqRelacionRepository().ListarEmpresas(ConstantesCortoPlazo.FuenteGeneracion);
        }

        /// <summary>
        /// Permite obtener las empresas generadoras
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasGeneradoras()
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasSEIN().Where(x => x.Tipoemprcodi == 3 || x.Emprcodi == 13 || x.Emprcodi == 67).OrderBy(x => x.Emprnomb).ToList();
        }

        /// <summary>
        /// Permite obtener los equipos generadores por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarEquiposPorEmpresa(int idEmpresa)
        {
            return FactorySic.GetEqRelacionRepository().ObtenerEquiposRelacion(idEmpresa);
        }

        /// <summary>
        /// Inserta un registro de la tabla EQ_RELACION
        /// </summary>
        public int SaveEqRelacion(EqRelacionDTO entity)
        {
            try
            {
                entity.Indfuente = ConstantesCortoPlazo.FuenteGeneracion;
                int resultado = 1;
                int id = 0;
                if (entity.Relacioncodi == 0)
                {
                    int count = FactorySic.GetEqRelacionRepository().ObtenerPorEquipo((int)entity.Equicodi, ConstantesCortoPlazo.FuenteGeneracion);

                    if (count == 0)
                    {
                        id = FactorySic.GetEqRelacionRepository().Save(entity);
                    }
                    else
                    {
                        resultado = 2;
                    }
                }
                else
                {
                    FactorySic.GetEqRelacionRepository().Update(entity);
                    id = entity.Relacioncodi;
                }

                if (entity.Indtnaadicional == ConstantesAppServicio.SI)
                {
                    this.SaveEqRelacionTna(id, entity.EquipoAdicionalTNA, entity.Usucreacion);
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
        /// Elimina un registro de la tabla EQ_RELACION
        /// </summary>
        public void DeleteEqRelacion(int relacioncodi)
        {
            try
            {
                FactorySic.GetEqRelacionTnaRepository().Delete(relacioncodi);
                FactorySic.GetEqRelacionRepository().Delete(relacioncodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EQ_RELACION
        /// </summary>
        public EqRelacionDTO GetByIdEqRelacion(int relacioncodi)
        {
            return FactorySic.GetEqRelacionRepository().GetById(relacioncodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_RELACION
        /// </summary>
        public List<EqRelacionDTO> ListEqRelacions()
        {
            return FactorySic.GetEqRelacionRepository().List(ConstantesCortoPlazo.FuenteGeneracion);
        }

        /// <summary>
        /// Permite listar las relaciones de tipo hidraulico
        /// </summary>
        /// <returns></returns>
        public List<EqRelacionDTO> ListEqRelacionsHidraulico()
        {
            return FactorySic.GetEqRelacionRepository().ListHidraulico(ConstantesCortoPlazo.FuenteGeneracion);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EqRelacion
        /// </summary>
        public List<EqRelacionDTO> GetByCriteriaEqRelacions(int idEmpresa, string estado)
        {
            return FactorySic.GetEqRelacionRepository().GetByCriteria(idEmpresa, estado, ConstantesCortoPlazo.FuenteGeneracion);
        }

        /// <summary>
        /// Permite obtener la lista de propiedades de las unidades
        /// </summary>
        /// <param name="indicador"></param>
        /// <returns></returns>
        public List<EqRelacionDTO> ObtenerPropiedadesConfiguracion(int indicador)
        {
            return FactorySic.GetEqRelacionRepository().ObtenerPropiedadesConfiguracion(indicador);
        }

        #endregion

        #region Métodos Tabla EQ_CONGESTION_CONFIG

        /// <summary>
        /// Inserta un registro de la tabla EQ_CONGESTION_CONFIG
        /// </summary>
        public int SaveEqCongestionConfig(EqCongestionConfigDTO entity)
        {
            try
            {
                int idResultado = 1;

                ValidarCodigoScada(entity);

                if (entity.Configcodi == 0)
                {
                    int count = FactorySic.GetEqCongestionConfigRepository().ValidarExistencia(entity.Equicodi);

                    if (count == 0)
                    {
                        FactorySic.GetEqCongestionConfigRepository().Save(entity);
                    }
                    else
                    {
                        idResultado = 2;
                    }
                }
                else
                {
                    FactorySic.GetEqCongestionConfigRepository().Update(entity);
                }

                return idResultado;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// valida el campo Código Scada
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private void ValidarCodigoScada(EqCongestionConfigDTO entity)
        {
            var codigoScada = entity.Canalcodi;

            if (codigoScada != null)
            {
                /*** validar existencia de codigo Scada  ***/
                var objCanal = FactoryScada.GetTrCanalSp7Repository().GetById(codigoScada.Value);
                if (objCanal == null) throw new ArgumentException("El código SCADA ingresado no existe, por favor ingrese un código correcto.");

                /*** validar que el código no este conectada a otra linea ***/
                List<EqCongestionConfigDTO> congestionActivos = FactorySic.GetEqCongestionConfigRepository().List().Where(x => x.Estado == "ACTIVO").ToList();

                //Es registro
                if (entity.Configcodi == 0)
                {
                    EqCongestionConfigDTO obj = congestionActivos.Find(x => x.Canalcodi == codigoScada);
                    if (obj != null) throw new ArgumentException("El código SCADA ingresado ya ha sido relacionado con otra Línea EMS.");
                }
                else //Es actualización
                {
                    EqCongestionConfigDTO obj = congestionActivos.Find(x => x.Canalcodi == codigoScada);
                    if (obj != null)  //ya existe el codigoScada en la BD
                    {
                        //Verifico si el codigoScada es del mismo equipo
                        int equicodiObj = entity.Equicodi;

                        if (obj.Equicodi != equicodiObj)
                        {
                            throw new ArgumentException("El código SCADA ingresado ya ha sido relacionado con otra Línea EMS.");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EQ_CONGESTION_CONFIG
        /// </summary>
        public void UpdateEqCongestionConfig(EqCongestionConfigDTO entity)
        {
            try
            {
                FactorySic.GetEqCongestionConfigRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EQ_CONGESTION_CONFIG
        /// </summary>
        public void DeleteEqCongestionConfig(int configcodi)
        {
            try
            {
                FactorySic.GetEqCongestionConfigRepository().Delete(configcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EQ_CONGESTION_CONFIG
        /// </summary>
        public EqCongestionConfigDTO GetByIdEqCongestionConfig(int configcodi)
        {
            return FactorySic.GetEqCongestionConfigRepository().GetById(configcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_CONGESTION_CONFIG
        /// </summary>
        public List<EqCongestionConfigDTO> ListEqCongestionConfigs()
        {
            return FactorySic.GetEqCongestionConfigRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EqCongestionConfig
        /// </summary>
        public List<EqCongestionConfigDTO> GetByCriteriaEqCongestionConfigs(int idEmpresa, string estado, int idGrupo, int idFamilia)
        {
            return FactorySic.GetEqCongestionConfigRepository().GetByCriteria(idEmpresa, estado, idGrupo, idFamilia);
        }

        /// <summary>
        /// Permite obtener las empresas del filtro
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresaFiltroLinea()
        {
            return FactorySic.GetEqCongestionConfigRepository().ObtenerEmpresasFiltro();
        }

        /// <summary>
        /// Permite buscar las empresas que tienen lineas
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListEmpresasLineas()
        {
            return FactorySic.GetEqCongestionConfigRepository().ObtenerEmpresasLineas();
        }

        /// <summary>
        /// Permite obtener las lineas por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarEquipoLineaPorEmpresa(int idEmpresa, int idFamilia)
        {
            return FactorySic.GetEqCongestionConfigRepository().ListarEquipoLineaPorEmpresa(idEmpresa, idFamilia);
        }


        #endregion

        #region Métodos Tabla EQ_GRUPO_LINEA

        /// <summary>
        /// Inserta un registro de la tabla EQ_GRUPO_LINEA
        /// </summary>
        public void SaveEqGrupoLinea(EqGrupoLineaDTO entity)
        {
            try
            {
                if (entity.Grulincodi == 0)
                {
                    FactorySic.GetEqGrupoLineaRepository().Save(entity);
                }
                else
                {
                    FactorySic.GetEqGrupoLineaRepository().Update(entity);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EQ_GRUPO_LINEA
        /// </summary>
        public void UpdateEqGrupoLinea(EqGrupoLineaDTO entity)
        {
            try
            {
                FactorySic.GetEqGrupoLineaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EQ_GRUPO_LINEA
        /// </summary>
        public int DeleteEqGrupoLinea(int grulincodi)
        {
            try
            {
                List<EqCongestionConfigDTO> list = FactorySic.GetEqCongestionConfigRepository().ObtenerPorGrupo(grulincodi);

                if (list.Count == 0)
                {
                    FactorySic.GetEqGrupoLineaRepository().Delete(grulincodi);
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EQ_GRUPO_LINEA
        /// </summary>
        public EqGrupoLineaDTO GetByIdEqGrupoLinea(int grulincodi)
        {
            return FactorySic.GetEqGrupoLineaRepository().GetById(grulincodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_GRUPO_LINEA
        /// </summary>
        public List<EqGrupoLineaDTO> ListEqGrupoLineas()
        {
            return FactorySic.GetEqGrupoLineaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EqGrupoLinea
        /// </summary>
        public List<EqGrupoLineaDTO> GetByCriteriaEqGrupoLineas(string tipo)
        {
            return FactorySic.GetEqGrupoLineaRepository().GetByCriteria(tipo);
        }

        /// <summary>
        /// Permite obtener las subcausa de eventos
        /// </summary>
        /// <returns></returns>
        public List<EveSubcausaeventoDTO> ListaSubCausaEvento()
        {
            return FactorySic.GetEveSubcausaeventoRepository().ObtenerPorCausa(100);
        }

        #endregion

        #region Métodos Tabla PR_GENFORZADA_MAESTRO

        /// <summary>
        /// Inserta un registro de la tabla PR_GENFORZADA_MAESTRO
        /// </summary>
        public int SavePrGenforzadaMaestro(PrGenforzadaMaestroDTO entity)
        {
            try
            {
                int idResultado = 1;

                if (entity.Genformaecodi == 0)
                {
                    int contador = FactorySic.GetPrGenforzadaMaestroRepository().ValidarExistenciaPorRelacion((int)entity.Relacioncodi);
                    if (contador == 0)
                    {
                        FactorySic.GetPrGenforzadaMaestroRepository().Save(entity);
                    }
                    else
                    {
                        idResultado = 2;
                    }
                }
                else
                {
                    FactorySic.GetPrGenforzadaMaestroRepository().Update(entity);
                }

                return idResultado;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_GENFORZADA_MAESTRO
        /// </summary>
        public void UpdatePrGenforzadaMaestro(PrGenforzadaMaestroDTO entity)
        {
            try
            {
                FactorySic.GetPrGenforzadaMaestroRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PR_GENFORZADA_MAESTRO
        /// </summary>
        public void DeletePrGenforzadaMaestro(int genformaecodi)
        {
            try
            {
                FactorySic.GetPrGenforzadaMaestroRepository().Delete(genformaecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_GENFORZADA_MAESTRO
        /// </summary>
        public PrGenforzadaMaestroDTO GetByIdPrGenforzadaMaestro(int genformaecodi)
        {
            return FactorySic.GetPrGenforzadaMaestroRepository().GetById(genformaecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_GENFORZADA_MAESTRO
        /// </summary>
        public List<PrGenforzadaMaestroDTO> ListPrGenforzadaMaestros()
        {
            return FactorySic.GetPrGenforzadaMaestroRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrGenforzadaMaestro
        /// </summary>
        public List<PrGenforzadaMaestroDTO> GetByCriteriaPrGenforzadaMaestros()
        {
            return FactorySic.GetPrGenforzadaMaestroRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla PR_GENFORZADA

        /// <summary>
        /// Inserta un registro de la tabla PR_GENFORZADA
        /// </summary>
        public int SavePrGenforzada(PrGenforzadaDTO entity)
        {
            try
            {
                int idResultado = 1;

                if (((DateTime)entity.Genforfin).Subtract((DateTime)entity.Genforinicio).TotalSeconds >= 0)
                {
                    if (entity.Genforcodi == 0)
                    {
                        FactorySic.GetPrGenforzadaRepository().Save(entity);
                    }
                    else
                    {
                        FactorySic.GetPrGenforzadaRepository().Update(entity);
                    }
                }
                else
                {
                    idResultado = 2;
                }

                return idResultado;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_GENFORZADA
        /// </summary>
        public void UpdatePrGenforzada(PrGenforzadaDTO entity)
        {
            try
            {
                FactorySic.GetPrGenforzadaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PR_GENFORZADA
        /// </summary>
        public void DeletePrGenforzada(int genforcodi)
        {
            try
            {
                FactorySic.GetPrGenforzadaRepository().Delete(genforcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_GENFORZADA
        /// </summary>
        public PrGenforzadaDTO GetByIdPrGenforzada(int genforcodi)
        {
            return FactorySic.GetPrGenforzadaRepository().GetById(genforcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_GENFORZADA
        /// </summary>
        public List<PrGenforzadaDTO> ListPrGenforzadas()
        {
            return FactorySic.GetPrGenforzadaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrGenforzada
        /// </summary>
        public List<PrGenforzadaDTO> GetByCriteriaPrGenforzadas(DateTime fechaInicio, DateTime fechaFin)
        {
            return FactorySic.GetPrGenforzadaRepository().GetByCriteria(fechaInicio, fechaFin);
        }

        #endregion

        #region Métodos Tabla PR_CONGESTION

        /// <summary>
        /// Inserta un registro de la tabla PR_CONGESTION
        /// </summary>
        public int SavePrCongestion(PrCongestionDTO entity)
        {
            try
            {
                int idResultado = 1;

                if (((DateTime)entity.Congesfecfin).Subtract((DateTime)entity.Congesfecinicio).TotalSeconds >= 0)
                {
                    //- Llenado los datos de la congestion de operaciones varias
                    EveIeodcuadroDTO operacion = null;
                    if (entity.Configcodi != null || entity.Grulincodi != null)
                    {
                        int idLinea = 0;

                        if (entity.Configcodi != null)
                        {
                            EqCongestionConfigDTO configuracion = FactorySic.GetEqCongestionConfigRepository().GetById((int)entity.Configcodi);
                            idLinea = configuracion.Equicodi;
                        }
                        else if (entity.Grulincodi != null)
                        {
                            EqGrupoLineaDTO configuracion = FactorySic.GetEqGrupoLineaRepository().GetById((int)entity.Grulincodi);
                            if (configuracion != null && configuracion.Equicodi != null)
                                idLinea = (int)configuracion.Equicodi;
                        }

                        if (idLinea > 0)
                        {
                            operacion = new EveIeodcuadroDTO
                            {
                                Equicodi = idLinea,
                                Ichorini = entity.Congesfecinicio,
                                Ichorfin = entity.Congesfecfin,
                                Icdescrip2 = entity.Congesmotivo,
                                Iccheck1 = ConstantesAppServicio.NO,
                                Icvalor1 = 0,
                                Iccheck2 = ConstantesAppServicio.NO,
                                Evenclasecodi = 1,
                                Iccheck3 = ConstantesAppServicio.NO,
                                Iccheck4 = ConstantesAppServicio.NO,
                                Icvalor2 = 0,
                                Lastuser = entity.Lastuser,
                                Lastdate = (DateTime)entity.Lastdate,
                                Subcausacodi = COES.Servicios.Aplicacion.Eventos.Helper.ConstantesOperacionesVarias.Subcausacodicongestion,
                                Icestado = ConstantesAppServicio.Activo
                            };
                        }
                    }

                    if (entity.Congescodi == 0)
                    {
                        //- Debemos grabar los datos en operaciones varias solo si es linea de congestion

                        if (operacion != null)
                        {
                            int idIccodi = FactorySic.GetEveIeodcuadroRepository().Save(operacion);
                            entity.Iccodi = idIccodi;

                            foreach (string item in entity.ListaGrupoDespacho)
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    EveCongesgdespachoDTO entityOV = new EveCongesgdespachoDTO();

                                    entityOV.Iccodi = entity.Iccodi;
                                    entityOV.Grupocodi = int.Parse(item);
                                    entityOV.Congdefechaini = operacion.Ichorini;
                                    entityOV.Congdefechafin = operacion.Ichorfin;
                                    entityOV.Congdeusucreacion = entity.Lastuser;
                                    entityOV.Congdefeccreacion = DateTime.Now;
                                    entityOV.Congdeestado = 1;

                                    FactorySic.GetEveCongesgdespachoRepository().Save(entityOV);
                                }
                            }

                        }

                        //- Grabamos la congestion en el modulo de cm
                        int id = FactorySic.GetPrCongestionRepository().Save(entity);

                        foreach (string item in entity.ListaGrupoDespacho)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                PrCongestionGrupoDTO grupoCongestion = new PrCongestionGrupoDTO();
                                grupoCongestion.Congescodi = id;
                                grupoCongestion.Grupocodi = int.Parse(item);
                                grupoCongestion.Lastdate = DateTime.Now;
                                grupoCongestion.Lastuser = entity.Lastuser;
                                FactorySic.GetPrCongestionGrupoRepository().Save(grupoCongestion);
                            }
                        }
                    }
                    else
                    {
                        //- Actualizamos tambien en operaciones varias
                        if (operacion != null)
                        {
                            if (entity.Iccodi != null)
                            {
                                operacion.Iccodi = (int)entity.Iccodi;
                                FactorySic.GetEveIeodcuadroRepository().Update(operacion);

                                (new OperacionesVariasAppServicio()).DeleteEveCongesgdespacho(operacion.Iccodi);

                                foreach (string item in entity.ListaGrupoDespacho)
                                {
                                    if (!string.IsNullOrEmpty(item))
                                    {
                                        EveCongesgdespachoDTO entityOV = new EveCongesgdespachoDTO();

                                        entityOV.Iccodi = operacion.Iccodi;
                                        entityOV.Grupocodi = int.Parse(item);
                                        entityOV.Congdefechaini = operacion.Ichorini;
                                        entityOV.Congdefechafin = operacion.Ichorfin;
                                        entityOV.Congdeusucreacion = entity.Lastuser;
                                        entityOV.Congdefeccreacion = DateTime.Now;
                                        entityOV.Congdeestado = 1;

                                        FactorySic.GetEveCongesgdespachoRepository().Save(entityOV);
                                    }
                                }


                            }
                        }

                        //- Actualizamos la congestion en el modulo de cm
                        FactorySic.GetPrCongestionRepository().Update(entity);
                        int id = entity.Congescodi;

                        //- Debemos eliminar primero lo registrado previamente
                        FactorySic.GetPrCongestionGrupoRepository().Delete(id);

                        foreach (string item in entity.ListaGrupoDespacho)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                PrCongestionGrupoDTO grupoCongestion = new PrCongestionGrupoDTO();
                                grupoCongestion.Congescodi = id;
                                grupoCongestion.Grupocodi = int.Parse(item);
                                grupoCongestion.Lastdate = DateTime.Now;
                                grupoCongestion.Lastuser = entity.Lastuser;
                                FactorySic.GetPrCongestionGrupoRepository().Save(grupoCongestion);
                            }
                        }


                    }

                }
                else
                {
                    idResultado = 2;
                }

                return idResultado;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_CONGESTION
        /// </summary>
        public void UpdatePrCongestion(PrCongestionDTO entity)
        {
            try
            {
                FactorySic.GetPrCongestionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PR_CONGESTION
        /// </summary>
        public void DeletePrCongestion(int congescodi)
        {
            try
            {
                PrCongestionDTO entity = FactorySic.GetPrCongestionRepository().GetById(congescodi);

                FactorySic.GetPrCongestionRepository().Delete(congescodi);

                if (entity.Iccodi != null)
                {
                    FactorySic.GetEveIeodcuadroRepository().Delete((int)entity.Iccodi);
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_CONGESTION
        /// </summary>
        public PrCongestionDTO GetByIdPrCongestion(int congescodi)
        {
            return FactorySic.GetPrCongestionRepository().GetById(congescodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_CONGESTION
        /// </summary>
        public List<PrCongestionDTO> ListPrCongestions()
        {
            return FactorySic.GetPrCongestionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrCongestion de lineas simple
        /// </summary>
        public List<PrCongestionDTO> ObtenerCongestionSimple(DateTime fechaInicio, DateTime fechaFin)
        {
            return FactorySic.GetPrCongestionRepository().ObtenerCongestionSimple(fechaInicio, fechaFin);
        }

        /// <summary>
        /// Permite realizar búsqueda de lineas conjuntas de congestión
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<PrCongestionDTO> ObtenerCongestionConjunto(DateTime fechaInicio, DateTime fechaFin)
        {
            return FactorySic.GetPrCongestionRepository().ObtenerCongestionConjunto(fechaInicio, fechaFin);
        }

        /// <summary>
        /// Permite obtener los datos de congestión
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="relacion"></param>
        /// <returns></returns>
        public List<PrCongestionDTO> ObtenerCongestion(DateTime fechaini, DateTime fechafin)
        {
            //- Debemos hacer el cambio aqui
            List<PrCongestionDTO> entitys = FactorySic.GetPrCongestionRepository().ObtenerCongestion(fechaini, fechafin);

            foreach (PrCongestionDTO item in entitys)
            {
                List<PrCongestionGrupoDTO> itemsGrupo = FactorySic.GetPrCongestionGrupoRepository().GetByCriteria(item.Congescodi);
                item.ListaGrupoDespacho = itemsGrupo.Select(x => x.Grupocodi.ToString()).ToList();
            }

            return entitys;
        }


        /// <summary>
        /// Permite obtener la congestión programada PDO y RDO vigentes
        /// </summary>
        /// <param name="fechaTR"></param>
        public List<PrCongestionDTO> ObtenerCongestionProgramada(DateTime fechaTR)
        {
            List<PrCongestionDTO> congestion = new List<PrCongestionDTO>();

            //obtener escenario yupana vigente
            List<CpMedicion48DTO> list = ListarDataCongestionYupanaAcumulada(fechaTR.Date, fechaTR);
            if (list.Any())
            {
                foreach (CpMedicion48DTO item in list)
                {
                    bool flagInicio = true;
                    int periodoInicio = 0;
                    int periodoFin = 0;
                    bool flag = false;

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valor = Convert.ToDecimal(item.GetType().GetProperty("H" + i).GetValue(item));

                        if (valor != 0)
                        {
                            if (flagInicio)
                            {
                                periodoInicio = i;
                                flagInicio = false;
                                flag = true;
                            }
                            periodoFin = i;
                        }
                        else
                        {
                            flagInicio = true;
                            if (flag)
                            {
                                congestion.Add(this.AgregarItemCongestion(item, fechaTR.Date, periodoInicio, periodoFin));
                                flag = false;
                            }
                        }

                        if (i == 48 && !flagInicio)
                        {
                            periodoFin = i;

                            if (flag)
                            {
                                congestion.Add(this.AgregarItemCongestion(item, fechaTR.Date, periodoInicio, periodoFin));
                            }
                        }
                    }

                }
            }

            return congestion;
        }

        private List<CpMedicion48DTO> ListarDataCongestionYupanaAcumulada(DateTime fecha, DateTime fechaTR)
        {
            int hLineaVerde = COES.Servicios.Aplicacion.Helper.Util.GetPosicionHoraInicial48(fechaTR)[0];

            List<CpTopologiaDTO> topologias = (new McpAppServicio()).ObtenerTopologias(fecha);

            //Obtener la data de todas las topologias 
            List<CpMedicion48DTO> listData = new List<CpMedicion48DTO>();
            foreach (CpTopologiaDTO topologia in topologias)
            {
                List<CpMedicion48DTO> list = FactorySic.GetCpMedicion48Repository().ObtenerCongestionProgramada(topologia.Topcodi);
                foreach (var item in list)
                {
                    item.Topcodi = topologia.Topcodi;

                    //redondear valor numérico a 4 decimales

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal? valorH = (decimal?)item.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(item);

                        if (valorH != null)
                        {
                            item.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(item, Math.Round(valorH.Value, 4));
                        }
                    }
                }

                listData.AddRange(list);
            }

            var listRecursos = listData.Select(x => new { Equicodi = x.Equicodi, Famcodi = x.Famcodi }).Distinct().ToList();

            foreach (CpTopologiaDTO topologia in topologias)
            {
                List<CpMedicion48DTO> list = listData.Where(x => x.Topcodi == topologia.Topcodi).ToList();
                var subList = listRecursos.Where(x => !list.Any(y => x.Equicodi == y.Equicodi)).ToList();

                foreach (var itemList in subList)
                {
                    CpMedicion48DTO itemMedicion = new CpMedicion48DTO();
                    itemMedicion.Topcodi = topologia.Topcodi;
                    itemMedicion.Equicodi = itemList.Equicodi;
                    itemMedicion.Famcodi = itemList.Famcodi;
                    listData.Add(itemMedicion);
                }
            }

            //listado unico de equipos
            List<CpMedicion48DTO> listaRecurso = listData.GroupBy(x => x.Equicodi).Select(x => new CpMedicion48DTO()
            {
                Famcodi = x.First().Famcodi,
                Equicodi = x.Key
            }).ToList();

            //todos las topologias en unico registro
            foreach (CpTopologiaDTO topologia in topologias)
            {
                List<CpMedicion48DTO> listDataXTop = listData.Where(x => x.Topcodi == topologia.Topcodi).OrderBy(x => x.Recurnombre).ToList();

                foreach (var objRec in listaRecurso)
                {
                    CpMedicion48DTO objDataXTopyRec = listDataXTop.Find(x => x.Equicodi == objRec.Equicodi);
                    if (objDataXTopyRec != null)
                    {
                        for (int i = 1; i <= 48; i++)
                        {
                            decimal? valorH = (decimal?)objDataXTopyRec.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(objDataXTopyRec);

                            if (i >= topologia.HoraReprograma)
                            {
                                objRec.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(objRec, valorH);
                            }
                        }
                    }
                }
            }

            return listaRecurso;
        }

        /// <summary>
        /// Permite obtener la capacidad nominal o limite de transmision del yupana
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <returns></returns>
        public List<PrCongestionDTO> ObtenerCapacidadNominal(DateTime fechaProceso, string horizonte)
        {
            List<PrCongestionDTO> congestion = new List<PrCongestionDTO>();
            CpTopologiaDTO entity = FactorySic.GetCpTopologiaRepository().GetByFechaTopfinal(fechaProceso, horizonte);
            if (entity == null) return congestion;
            List<CpMedicion48DTO> list = FactorySic.GetCpMedicion48Repository().ObtenerCapacidadNominal(entity.Topcodi);

            foreach (CpMedicion48DTO item in list)
            {
                bool flagInicio = true;
                int periodoInicio = 0;
                int periodoFin = 0;
                bool flag = false;

                for (int i = 1; i <= 48; i++)
                {
                    decimal valor = Convert.ToDecimal(item.GetType().GetProperty("H" + i).GetValue(item));

                    if (valor < (decimal)-0.1)
                    {
                        if (flagInicio)
                        {
                            periodoInicio = i;
                            flagInicio = false;
                            flag = true;
                        }
                        periodoFin = i;
                    }
                    else
                    {
                        flagInicio = true;
                        if (flag)
                        {
                            congestion.Add(this.AgregarItemCongestion2(item, fechaProceso, periodoInicio, periodoFin));
                            flag = false;
                        }
                    }

                    if (i == 48 && !flagInicio)
                    {
                        periodoFin = i;

                        if (flag)
                        {
                            congestion.Add(this.AgregarItemCongestion2(item, fechaProceso, periodoInicio, periodoFin));
                        }
                    }
                }

            }

            return congestion;
        }
        /// <summary>
        /// Obtiene el consumo de gas natural del caso oficial de Yupana
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <param name="srestcodi"></param>
        /// <returns></returns>
        public List<CpMedicion48DTO> ObtenerConsumoGasNatural(DateTime fechaProceso, int srestcodi, string tipoPlazo)
        {
            List<CpMedicion48DTO> consumogas = new List<CpMedicion48DTO>();
            CpTopologiaDTO entity = FactorySic.GetCpTopologiaRepository().GetByFechaTopfinal(fechaProceso, tipoPlazo);
            if (entity == null) return consumogas;
            consumogas = FactorySic.GetCpMedicion48Repository().ObtenerConsumoGasNatural(entity.Topcodi, srestcodi);
            return consumogas;
        }

        /// <summary>
        /// Permite agregar un item al listado de congestiones programadas
        /// </summary>
        /// <param name="item"></param>
        /// <param name="periodoInicio"></param>
        /// <param name="periodoFin"></param>
        protected PrCongestionDTO AgregarItemCongestion(CpMedicion48DTO item, DateTime fechaProceso, int periodoInicio, int periodoFin)
        {
            PrCongestionDTO entity = new PrCongestionDTO();

            if (item.Famcodi != 0)
            {
                entity = new PrCongestionDTO
                {
                    Fuente = UtilCortoPlazo.ObtenerFuenteCongestion(item.Famcodi),
                    Grulincodi = null,
                    Configcodi = item.Equicodi,
                    Congesfecinicio = UtilCortoPlazo.ObtenerRangoFecha(fechaProceso, periodoInicio, 1),
                    Congesfecfin = UtilCortoPlazo.ObtenerRangoFecha(fechaProceso, periodoFin, 0)
                };
            }
            else
            {
                entity = new PrCongestionDTO
                {
                    Fuente = UtilCortoPlazo.ObtenerFuenteCongestion(ConstantesCortoPlazo.IdConjuntoLinea),
                    Grulincodi = item.Equicodi,
                    Configcodi = null,
                    Congesfecinicio = UtilCortoPlazo.ObtenerRangoFecha(fechaProceso, periodoInicio, 1),
                    Congesfecfin = UtilCortoPlazo.ObtenerRangoFecha(fechaProceso, periodoFin, 0)
                };
            }

            return entity;
        }

        /// <summary>
        /// Permite agregar un item al listado de congestiones programadas 2
        /// </summary>
        /// <param name="item"></param>
        /// <param name="fechaProceso"></param>
        /// <param name="periodoInicio"></param>
        /// <param name="periodoFin"></param>
        /// <returns></returns>
        protected PrCongestionDTO AgregarItemCongestion2(CpMedicion48DTO item, DateTime fechaProceso, int periodoInicio, int periodoFin)
        {
            PrCongestionDTO entity = new PrCongestionDTO();

            if (item.Famcodi != 0)
            {
                entity = new PrCongestionDTO
                {
                    Fuente = UtilCortoPlazo.ObtenerFuenteCongestion(item.Famcodi),
                    Grulincodi = null,
                    Configcodi = item.Equicodi,
                    Congesfecinicio = UtilCortoPlazo.ObtenerRangoFecha(fechaProceso, periodoInicio, 1),
                    Congesfecfin = UtilCortoPlazo.ObtenerRangoFecha(fechaProceso, periodoFin, 0),
                    Equinomb = item.Equiabrev,
                    Areanomb = item.Areanomb,
                    Emprnomb = item.Emprnomb,
                    Equicodi = item.Equicodi
                };
            }
            else
            {
                entity = new PrCongestionDTO
                {
                    Fuente = UtilCortoPlazo.ObtenerFuenteCongestion(ConstantesCortoPlazo.IdConjuntoLinea),
                    Grulincodi = item.Equicodi,
                    Configcodi = null,
                    Congesfecinicio = UtilCortoPlazo.ObtenerRangoFecha(fechaProceso, periodoInicio, 1),
                    Congesfecfin = UtilCortoPlazo.ObtenerRangoFecha(fechaProceso, periodoFin, 0),
                    Equinomb = item.Equiabrev,
                    Areanomb = item.Areanomb
                };
            }

            return entity;
        }

        #endregion

        #region Métodos Tabla CM_CONFIGBARRA

        /// <summary>
        /// Inserta un registro de la tabla CM_CONFIGBARRA
        /// </summary>
        public int SaveCmConfigbarra(CmConfigbarraDTO entity)
        {
            int result = 0;
            bool flag = true;
            try
            {
                if (entity.Canalcodi != null)
                {
                    int count = FactorySic.GetCmConfigbarraRepository().ValidarCodigoScada((int)entity.Canalcodi);

                    if (count == 0)
                    {
                        result = 2;
                        flag = false;
                    }
                    else
                    {
                        count = FactorySic.GetCmConfigbarraRepository().ValidarRegistro(-1, -1, (int)entity.Canalcodi,
                            entity.Cnfbarcodi);

                        if (count > 0)
                        {
                            result = 3;
                            flag = false;
                        }
                    }
                }

                if (entity.Recurcodi != null)
                {
                    int count = FactorySic.GetCmConfigbarraRepository().ValidarRegistro((int)entity.Recurcodi,
                        ConstantesCortoPlazo.Topcodi, -1, entity.Cnfbarcodi);

                    if (count > 0)
                    {
                        result = 4;
                        flag = false;
                    }
                }

                if (flag)
                {
                    int id = 0;

                    if (entity.Recurcodi != null)
                    {
                        entity.Topcodi = ConstantesCortoPlazo.Topcodi;
                    }

                    if (entity.Cnfbarcodi == 0)
                    {
                        id = FactorySic.GetCmConfigbarraRepository().Save(entity);
                    }
                    else
                    {
                        FactorySic.GetCmConfigbarraRepository().Update(entity);
                        id = entity.Cnfbarcodi;
                    }

                    result = 1;

                }

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                result = -1;
            }

            return result;
        }

        /// <summary>
        /// Actualiza un registro de la tabla CM_CONFIGBARRA
        /// </summary>
        public void UpdateCmConfigbarra(CmConfigbarraDTO entity)
        {
            try
            {
                FactorySic.GetCmConfigbarraRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite actualizar las coordendas de las barras
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateCoordenada(CmConfigbarraDTO entity)
        {
            try
            {
                FactorySic.GetCmConfigbarraRepository().UpdateCoordenada(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CM_CONFIGBARRA
        /// </summary>
        public void DeleteCmConfigbarra(int cnfbarcodi)
        {
            try
            {
                FactorySic.GetCmConfigbarraRepository().Delete(cnfbarcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CM_CONFIGBARRA
        /// </summary>
        public CmConfigbarraDTO GetByIdCmConfigbarra(int cnfbarcodi)
        {
            return FactorySic.GetCmConfigbarraRepository().GetById(cnfbarcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CM_CONFIGBARRA
        /// </summary>
        public List<CmConfigbarraDTO> ListCmConfigbarras()
        {
            return FactorySic.GetCmConfigbarraRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CmConfigbarra
        /// </summary>
        public List<CmConfigbarraDTO> GetByCriteriaCmConfigbarras(string estado, string publicacion)
        {
            return FactorySic.GetCmConfigbarraRepository().GetByCriteria(estado, publicacion);
        }

        public List<CmConfigbarraDTO> ObtenerBarrasYupana()
        {
            return FactorySic.GetCmConfigbarraRepository().ObtenerBarrasYupana(ConstantesCortoPlazo.Topcodi, ConstantesCortoPlazo.Catcodi);
        }

        #endregion

        #region Métodos Tabla CM_COSTOMARGINAL

        /// <summary>
        /// Inserta un registro de la tabla CM_COSTOMARGINAL
        /// </summary>
        public void SaveCmCostomarginal(CmCostomarginalDTO entity)
        {
            try
            {
                FactorySic.GetCmCostomarginalRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CM_COSTOMARGINAL
        /// </summary>
        public void UpdateCmCostomarginal(CmCostomarginalDTO entity)
        {
            try
            {
                FactorySic.GetCmCostomarginalRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CM_COSTOMARGINAL
        /// </summary>
        public void DeleteCmCostomarginal(int cmgncodi)
        {
            try
            {
                FactorySic.GetCmCostomarginalRepository().Delete(cmgncodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CM_COSTOMARGINAL
        /// </summary>
        public CmCostomarginalDTO GetByIdCmCostomarginal(int cmgncodi)
        {
            return FactorySic.GetCmCostomarginalRepository().GetById(cmgncodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CM_COSTOMARGINAL
        /// </summary>
        public List<CmCostomarginalDTO> ListCmCostomarginals()
        {
            return FactorySic.GetCmCostomarginalRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CmCostomarginal
        /// </summary>
        public List<CmCostomarginalDTO> GetByCriteriaCmCostomarginals()
        {
            return FactorySic.GetCmCostomarginalRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener los distintos resultados de costos marginales
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<CmCostomarginalDTO> ObtenerResultadoCostosMarginales(DateTime fecha, int tipoProceso)
        {
            List<CmCostomarginalDTO> lstSalida = new List<CmCostomarginalDTO>();
            List<CmCostomarginalDTO> lstFiltrada = new List<CmCostomarginalDTO>();
            List<CmCostomarginalDTO> lst = FactorySic.GetCmCostomarginalRepository().ObtenerResultadoCostoMarginal(fecha);
            if (tipoProceso != 0)
                lstFiltrada = lst.Where(x => x.TipoProceso == tipoProceso.ToString()).ToList();
            else
                lstFiltrada = lst;

            foreach (var item in lstFiltrada)
            {
                var fec = item.Cmgnfecha;
                item.CmgnfechaDesc = fec.ToString(ConstantesAppServicio.FormatoFecha);
                lstSalida.Add(item);
            }
            return lstSalida;
        }

        /// <summary>
        /// Permite obtener los distintos resultados de costos marginales
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<CmCostomarginalDTO> ObtenerResultadoCostosMarginalesWeb(DateTime fecha, int version)
        {
            return FactorySic.GetCmCostomarginalRepository().ObtenerResultadoCostoMarginalWeb(fecha, version);
        }

        public List<CmCostomarginalDTO> ObtenerResultadoCostosMarginalesExtranet(DateTime fecha, int tipoProceso)
        {
            List<CmCostomarginalDTO> lstSalida = new List<CmCostomarginalDTO>();
            List<CmCostomarginalDTO> lstFiltrada = new List<CmCostomarginalDTO>();
            List<CmCostomarginalDTO> lst = FactorySic.GetCmCostomarginalRepository().ObtenerResultadoCostoMarginalExtranet(fecha);
            if (tipoProceso != 0)
                lstFiltrada = lst.Where(x => x.TipoProceso == tipoProceso.ToString()).ToList();
            else
                lstFiltrada = lst;

            foreach (var item in lstFiltrada)
            {
                var fec = item.Cmgnfecha;
                item.CmgnfechaDesc = fec.ToString(ConstantesAppServicio.FormatoFecha);
                lstSalida.Add(item);
            }
            return lstSalida;
        }

        /// <summary>
        /// Permite obtener las corridas de un dia
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public List<CmCostomarginalDTO> ObtenerCorridasPorFecha(DateTime fechaConsulta, int version)
        {
            List<CmCostomarginalDTO> list = this.ObtenerResultadoCostosMarginalesWeb(fechaConsulta, version);

            foreach (CmCostomarginalDTO item in list)
            {
                item.FechaProceso = item.Cmgnfecha.ToString(ConstantesAppServicio.FormatoOnlyHora);
                item.Indicador = item.Cmgnfecha.Hour * 60 + item.Cmgnfecha.Minute;
            }

            List<int> corridas = list.Select(x => x.Indicador).Distinct().ToList();

            //- Debemos mostrar solo los últimos reprocesos
            List<CmCostomarginalDTO> result = new List<CmCostomarginalDTO>();

            foreach (int corrida in corridas)
            {
                CmCostomarginalDTO item = list.Where(x => x.Indicador == corrida).OrderByDescending(x => x.Cmgncorrelativo).FirstOrDefault();
                result.Add(item);
            }

            return result;
        }

        /// <summary>
        /// Permite obtener el listado de costos marginal por corrida
        /// </summary>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        public List<CmCostomarginalDTO> ObtenerDatosCostoMarginalCorrida(int correlativo)
        {
            return FactorySic.GetCmCostomarginalRepository().ObtenerDatosCostoMarginalCorrida(correlativo);
        }

        /// <summary>
        /// Permite eliminar los datos 
        /// </summary>
        /// <param name="correlativo"></param>
        public void EliminarCorridaCostoMarginal(int correlativo)
        {
            try
            {
                FactorySic.GetCmCostomarginalRepository().EliminarCorridaCostoMarginal(correlativo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener el reporte masivo de costos marginales nodales
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<CmCostomarginalDTO> ObtenerReporteCostosMarginales(DateTime fechaInicio, DateTime fechaFin, string estimador,
            string fuentepd, int version)
        {
            return FactorySic.GetCmCostomarginalRepository().ObtenerReporteCostosMarginales(fechaInicio, fechaFin, estimador, fuentepd, version);
        }

        /// <summary>
        /// Permite obtener el reporte de Flujos de Líneas y Trafos 2D
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<CmFlujoPotenciaDTO> ObtenerReporteFlujoPotencia(DateTime fechaInicio, DateTime fechaFin)
        {
            return FactorySic.GetCmFlujoPotenciaRepository().ObtenerReporteFlujoPotencia(fechaInicio, fechaFin);
        }

        /// <summary>
        /// Permite obtener el reporte masivo de costos marginales nodales
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<CmCostomarginalDTO> ObtenerReporteCostosMarginalesTR(DateTime fechaInicio, DateTime fechaFin)
        {
            return FactorySic.GetCmCostomarginalRepository().ObtenerReporteCostosMarginalesTR(fechaInicio, fechaFin);
        }

        /// <summary>
        /// Permite obtener el reporte de costos marginales para el portal web
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="defecto"></param>
        /// <returns></returns>
        public List<CmCostomarginalDTO> ObtenerReporteCostosMarginalesWeb(DateTime fechaInicio, DateTime fechaFin, string defecto)
        {
            return FactorySic.GetCmCostomarginalRepository().ObtenerReporteCostosMarginalesWeb(fechaInicio, fechaFin, defecto);
        }

        /// <summary>
        /// Permite obtener los costos marginales en un mapa de google
        /// </summary>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        public List<CmCostomarginalDTO> ObtenerCostosMarginalesMapa(int correlativo)
        {
            List<CmCostomarginalDTO> list = FactorySic.GetCmCostomarginalRepository().ObtenerDatosCostoMarginalCorrida(correlativo);
            return list.Where(x => x.Cnfbarindpublicacion == ConstantesAppServicio.SI && !string.IsNullOrEmpty(x.Cnfbarcoorx) && !string.IsNullOrEmpty(x.Cnfbarcoory)).ToList();
        }

        /// <summary>
        /// Permite obtener la ultima corrida de un dia
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public CmCostomarginalDTO ObtenerUltimaCorridaCostosMarginales(DateTime fecha)
        {
            List<CmCostomarginalDTO> list = this.ObtenerResultadoCostosMarginalesWeb(fecha, 1);

            foreach (CmCostomarginalDTO item in list)
            {
                item.FechaProceso = item.Cmgnfecha.ToString(ConstantesCortoPlazo.FormatoHoraMinuto);
                item.Indicador = item.Cmgnfecha.Hour * 60 + item.Cmgnfecha.Minute;
            }

            List<int> corridas = list.Select(x => x.Indicador).Distinct().ToList();

            //- Debemos mostrar solo los últimos reprocesos
            List<CmCostomarginalDTO> result = new List<CmCostomarginalDTO>();

            foreach (int corrida in corridas)
            {
                CmCostomarginalDTO item = list.Where(x => x.Indicador == corrida).OrderByDescending(x => x.Cmgncorrelativo).FirstOrDefault();
                result.Add(item);
            }

            List<CmCostomarginalDTO> entitys = result.OrderByDescending(x => x.Indicador).ToList();

            if (entitys.Count() > 0)
            {
                return entitys[0];
            }

            return null;
        }

        /// <summary>
        /// Permite obtener el resumen de los costos marginales
        /// </summary>
        /// <returns></returns>
        public CmCostomarginalDTO ObtenerResumenCostoMarginal()
        {
            return FactorySic.GetCmCostomarginalRepository().ObtenerResumenCM();
        }

        /// <summary>
        /// Muestra los datos de la ultima corrida del proceso
        /// </summary>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        public List<CmCostomarginalDTO> ObtenerMapaCostoMarginal(int correlativo)
        {
            string resultado = string.Empty;

            List<CmCostomarginalDTO> list = this.ObtenerCostosMarginalesMapa(correlativo);
            List<CmParametroDTO> listColores = this.ListCmParametros();
            List<CmCostomarginalDTO> listCM = this.FiltrarPorRango(list, listColores);

            return listCM;
        }

        /// <summary>
        /// Permite obtener el valor
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public List<CmCostomarginalDTO> FiltrarPorRango(List<CmCostomarginalDTO> list, List<CmParametroDTO> colores)
        {
            foreach (CmCostomarginalDTO item in list)
            {
                decimal total = (decimal)item.Cmgntotal;
                foreach (CmParametroDTO itemColor in colores)
                {
                    if (total >= itemColor.Cmparinferior && total < itemColor.Cmparsuperior)
                    {
                        item.Color = itemColor.Cmparvalor;
                        break;
                    }
                }
            }

            return list.Where(x => !string.IsNullOrEmpty(x.Color)).ToList();
        }

        /// <summary>
        /// Permite almacenar en la DB los costos marginales nodales
        /// </summary>
        /// <param name="lista">Lista de resultados</param>
        /// <param name="fechaProceso">Fecha de proceso</param>
        /// <param name="correlativo">Correlativo del proceso</param>
        /// <param name="demandas">Listado de demandas</param>
        public void AlmacenarCostosMarginales(List<ResultadoGams> lista, DateTime fechaProceso, int correlativo, List<NombreCodigoBarra> demandas,
            bool reproceso, bool flagWeb, out bool flagValidacionDato, int indicadorMovil, string usuario, bool flagMD)
        {
            //- Obtenemos el listado de barras
            List<CmConfigbarraDTO> configuracion = FactorySic.GetCmConfigbarraRepository().List();
            DateTime fechaRegistro = DateTime.Now;

            DateTime fechaDatos = (fechaProceso.Hour == 0 && fechaProceso.Minute == 0) ? fechaProceso.AddMinutes(-1) : fechaProceso;
            decimal cmgrepresentativo = 0;

            string indicadorMostrarWeb = ConstantesAppServicio.NO;
            if (reproceso)
            {
                indicadorMostrarWeb = ConstantesAppServicio.SI;

                if (flagWeb)
                {
                    indicadorMostrarWeb = ConstantesAppServicio.NO;
                }
            }

            //if (flagMD)
            //{
            //    indicadorMostrarWeb = ConstantesAppServicio.SI;
            //    indicadorMovil = 0;
            //}

            //-Validamos casos demasiado grandes o pequeños

            bool validacionValores = true;
            if (lista.Where(x => x.Total > 1000 || x.Total < -10).Count() > 0)
            {
                validacionValores = false;
            }

            if (!validacionValores)
                indicadorMostrarWeb = ConstantesAppServicio.SI;

            int indOperacion = 1;

            bool flagMovil = false;

            foreach (ResultadoGams item in lista)
            {
                indOperacion = 1;
                if (item.Nombarra.Contains("�")) item.Nombarra = item.Nombarra.Replace("�", "Ñ");
                CmConfigbarraDTO config = configuracion.Where(x => x.Cnfbarnodo.Trim() == item.Nombarra.Trim()).FirstOrDefault();
                NombreCodigoBarra demandaBarra = demandas.Where(x => x.NombBarra.Trim() == item.Nombarra.Trim() && x.Indicador == false).FirstOrDefault();
                if (demandaBarra != null)
                {
                    demandaBarra.Indicador = true;
                    if (demandaBarra.VoltajePU == 1 && demandaBarra.Angulo == 0)
                    {
                        indOperacion = 0;
                    }
                }

                if (config != null)
                {
                    CmCostomarginalDTO entity = new CmCostomarginalDTO
                    {
                        Cmgncorrelativo = correlativo,
                        Cnfbarcodi = config.Cnfbarcodi,
                        Cmgnenergia = item.Energia,
                        Cmgncongestion = item.Congestion,
                        Cmgntotal = item.Total,
                        Cmgnfeccreacion = fechaRegistro,
                        Cmgnfecha = fechaDatos,
                        Cmgndemanda = (demandaBarra != null) ? (decimal?)demandaBarra.barraPc : null,
                        Cmgnreproceso = indicadorMostrarWeb,
                        Cmgnoperativo = indOperacion,
                        Cmgnusucreacion = usuario
                    };

                    FactorySic.GetCmCostomarginalRepository().Save(entity);

                    if (config.Cnfbarcodi == ConstantesCortoPlazo.CodigoBarraRepresentativo)
                    {
                        cmgrepresentativo = item.Total;
                        flagMovil = true;
                    }
                }
            }

            try
            {
                if (indicadorMovil == 1 && flagMovil)
                {
                    //- Grabando el correlativo a mostrar en WEB
                    if (indicadorMostrarWeb == ConstantesAppServicio.NO && validacionValores)
                    {
                        //- Validamos fechas en los que se ejecutaron los procesos
                        if (fechaRegistro.Subtract(fechaProceso).TotalMinutes < 30)
                        {
                            FactorySic.GetCmCostomarginalRepository().GrabarRepresentativo(correlativo, cmgrepresentativo, fechaProceso);
                        }
                    }
                }
            }
            catch
            {

            }

            flagValidacionDato = validacionValores;
        }

        /// <summary>
        /// Permite generar el indicador para las alertas de valores negativos
        /// </summary>
        /// <param name="indicador"></param>
        public void GenerarAlertaValoresNegativos(string indicador, decimal maxCM, decimal maxCMCongestion, decimal maxCMSinCongestion,
            decimal maxCICongestion, decimal maxCISinCongestion, DateTime fechaProceso)
        {
            try
            {
                FactorySic.GetCmAlertavalorRepository().Update(indicador, maxCM, maxCMCongestion, maxCMSinCongestion, maxCICongestion, maxCISinCongestion, fechaProceso);
            }
            catch (Exception)
            {

            }
        }

        #endregion

        #region Métodos Tabla CM_EQUIVALENCIAMODOP

        /// <summary>
        /// Inserta un registro de la tabla CM_EQUIVALENCIAMODOP
        /// </summary>
        public void SaveCmEquivalenciamodop(CmEquivalenciamodopDTO entity)
        {
            try
            {
                FactorySic.GetCmEquivalenciamodopRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CM_EQUIVALENCIAMODOP
        /// </summary>
        public void UpdateCmEquivalenciamodop(CmEquivalenciamodopDTO entity)
        {
            try
            {
                FactorySic.GetCmEquivalenciamodopRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CM_EQUIVALENCIAMODOP
        /// </summary>
        public void DeleteCmEquivalenciamodop(int equimocodi)
        {
            try
            {
                FactorySic.GetCmEquivalenciamodopRepository().Delete(equimocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CM_EQUIVALENCIAMODOP
        /// </summary>
        public CmEquivalenciamodopDTO GetByIdCmEquivalenciamodop(int equimocodi)
        {
            return FactorySic.GetCmEquivalenciamodopRepository().GetById(equimocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CM_EQUIVALENCIAMODOP
        /// </summary>
        public List<CmEquivalenciamodopDTO> ListCmEquivalenciamodops()
        {
            return FactorySic.GetCmEquivalenciamodopRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CmEquivalenciamodop
        /// </summary>
        public List<CmEquivalenciamodopDTO> GetByCriteriaCmEquivalenciamodops()
        {
            return FactorySic.GetCmEquivalenciamodopRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla CM_EQUIVALENCIAMODOP
        /// </summary>
        public int SaveCmEquivalenciamodopId(CmEquivalenciamodopDTO entity)
        {
            return FactorySic.GetCmEquivalenciamodopRepository().SaveCmEquivalenciamodopId(entity);
        }
        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla CM_EQUIVALENCIAMODOP
        /// </summary>
        public List<CmEquivalenciamodopDTO> BuscarOperaciones(int grupocodi, int nroPage, int pageSize)
        {
            return FactorySic.GetCmEquivalenciamodopRepository().BuscarOperaciones(grupocodi, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla CM_EQUIVALENCIAMODOP
        /// </summary>
        public int ObtenerNroFilas(int grupocodi)
        {
            return FactorySic.GetCmEquivalenciamodopRepository().ObtenerNroFilas(grupocodi);
        }

        #endregion

        #region Métodos Tabla CM_OPERACIONREGISTRO

        /// <summary>
        /// Inserta un registro de la tabla CM_OPERACIONREGISTRO
        /// </summary>
        public void SaveCmOperacionregistro(CmOperacionregistroDTO entity)
        {
            try
            {
                FactorySic.GetCmOperacionregistroRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CM_OPERACIONREGISTRO
        /// </summary>
        public void UpdateCmOperacionregistro(CmOperacionregistroDTO entity)
        {
            try
            {
                FactorySic.GetCmOperacionregistroRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CM_OPERACIONREGISTRO
        /// </summary>
        public void DeleteCmOperacionregistro(int operegcodi)
        {
            try
            {
                FactorySic.GetCmOperacionregistroRepository().Delete(operegcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CM_OPERACIONREGISTRO
        /// </summary>
        public CmOperacionregistroDTO GetByIdCmOperacionregistro(int operegcodi)
        {
            return FactorySic.GetCmOperacionregistroRepository().GetById(operegcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CM_OPERACIONREGISTRO
        /// </summary>
        public List<CmOperacionregistroDTO> ListCmOperacionregistros()
        {
            return FactorySic.GetCmOperacionregistroRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CmOperacionregistro
        /// </summary>
        public List<CmOperacionregistroDTO> GetByCriteriaCmOperacionregistros()
        {
            return FactorySic.GetCmOperacionregistroRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla CM_OPERACIONREGISTRO
        /// </summary>
        public int SaveCmOperacionregistroId(CmOperacionregistroDTO entity)
        {
            return FactorySic.GetCmOperacionregistroRepository().SaveCmOperacionregistroId(entity);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla CM_OPERACIONREGISTRO
        /// </summary>
        public List<CmOperacionregistroDTO> BuscarOperaciones(int grupocodi, int subcausaCodi, DateTime operegFecinicio, DateTime operegFecfin, int nroPage, int pageSize)
        {
            return FactorySic.GetCmOperacionregistroRepository().BuscarOperaciones(grupocodi, subcausaCodi, operegFecinicio, operegFecfin, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla CM_OPERACIONREGISTRO
        /// </summary>
        public int ObtenerNroFilas(int grupocodi, int subcausaCodi, DateTime operegFecinicio, DateTime operegFecfin)
        {
            return FactorySic.GetCmOperacionregistroRepository().ObtenerNroFilas(grupocodi, subcausaCodi, operegFecinicio, operegFecfin);
        }

        /// <summary>
        /// Permite obtener la programación de los modos de operación
        /// </summary>
        /// <param name="fechaProceso"></param>
        public List<CmOperacionregistroDTO> ObtenerOperacionRegistroProgramado(DateTime fechaProceso)
        {
            //- Estableciendo carpeta de archivos y nombres
            string pathTrabajo = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathCostosMarginales];
            string path = fechaProceso.Year + @"\" +
                          fechaProceso.Day.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) +
                          fechaProceso.Month.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) + @"\";

            string fileProgramacion = path + ConstantesCortoPlazo.FileModosOperacion;

            //- Instancia de la clase de lectura de archivos
            FileHelper fileHelper = new FileHelper();

            try
            {
                //- Leyendo la congestión programada
                List<RelacionModoOperacion> modosOperacion = fileHelper.ObtenerDatoModosOperacion(fileProgramacion, pathTrabajo);

                //- Debemos relacionar con su respectivo equivalente
                List<CmOperacionregistroDTO> resultado = new List<CmOperacionregistroDTO>();

                List<CmEquivalenciamodopDTO> configuracionModo = FactorySic.GetCmEquivalenciamodopRepository().List();

                //- Relacionamos las congestiones lineales
                foreach (RelacionModoOperacion item in modosOperacion)
                {
                    CmEquivalenciamodopDTO itemConfig = configuracionModo.Where(x => x.Equimonombrencp.Trim().ToUpper() == item.CodigoNCP.Trim().ToUpper()).FirstOrDefault();
                    if (itemConfig != null)
                    {
                        //- Debemos agregar el registro de la congestión de la linea
                        resultado.Add(new CmOperacionregistroDTO
                        {
                            Grupocodi = itemConfig.Grupocodi,
                            Gruponomb = itemConfig.Gruponomb,
                            Operegfecinicio = UtilCortoPlazo.ObtenerRangoFecha(fechaProceso, item.PeriodoInicio, 1),
                            Operegfecfin = UtilCortoPlazo.ObtenerRangoFecha(fechaProceso, item.PeriodoFin, 0)
                        });
                    }
                }

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite exportar los archivos .DAT o .RAW
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public string ExportarArchivosDAT(List<CmCostomarginalDTO> entitys, DateTime fechaProceso, string pathExportacion, int tipo)
        {
            //- Estableciendo carpeta de archivos y nombres
            string pathTrabajo = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathCostosMarginales];

            List<string> blobs = new List<string>();

            List<string> names = new List<string>();
            foreach (CmCostomarginalDTO entity in entitys)
            {
                DateTime fecha = (entity.Cmgnfecha.Hour == 23 && entity.Cmgnfecha.Minute == 59) ?
                    entity.Cmgnfecha.AddMinutes(1) : entity.Cmgnfecha;

                string path = fecha.Year + @"\" +
                             fecha.Day.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) +
                             fecha.Month.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) + @"\Corrida_" + entity.Cmgncorrelativo + @"\";

                List<FileData> files = FileServerScada.ListarArhivos(path, pathTrabajo);

                if (tipo == 1)
                {
                    FileData dat = files.Where(x => (x.Extension.Contains("DAT") || x.Extension.Contains("dat")) && !(x.FileName.Contains("chidroPE")) &&
                        !(x.FileName.Contains("dbus")) && !(x.FileName.Contains("AC_ENTRADA"))).FirstOrDefault();

                    if (dat != null)
                    {
                        entity.Folder = path + dat.FileName;

                        blobs.Add(entity.Folder);
                        names.Add("ENTRADAGAMS_" + entity.Cmgnfecha.ToString("yyyyMMddHHmm") + ".DAT");
                    }
                }
                else if (tipo == 2)
                {
                    FileData dat = files.Where(x => (x.Extension.Contains("RAW") || x.Extension.Contains("raw"))).FirstOrDefault();

                    if (dat != null)
                    {
                        entity.Folder = path + dat.FileName;

                        blobs.Add(entity.Folder);
                        names.Add("PSSE_" + entity.Cmgnfecha.ToString("yyyyMMddHHmm") + ".raw");
                    }
                }
                else if (tipo == 3)
                {
                    FileData dat = files.Where(x => (x.Extension.Contains("csv") || x.Extension.Contains("CSV")) &&
                        x.FileName.Contains("RESULTADOGAMS")).FirstOrDefault();

                    if (dat != null)
                    {
                        entity.Folder = path + dat.FileName;

                        blobs.Add(entity.Folder);
                        names.Add("RESULTADOGAMS_" + entity.Cmgnfecha.ToString("yyyyMMddHHmm") + ".csv");
                    }
                }
                else if (tipo == 4)
                {
                    FileData dat = files.Where(x => (x.Extension.Contains("csv") || x.Extension.Contains("CSV")) &&
                        x.FileName.Contains("RESULTADO_GAMS_ANALISIS")).FirstOrDefault();

                    if (dat != null)
                    {
                        entity.Folder = path + dat.FileName;

                        blobs.Add(entity.Folder);
                        names.Add("RESULTADO_GAMS_ANALISIS_" + entity.Cmgnfecha.ToString("yyyyMMddHHmm") + ".csv");
                    }
                }
                else if (tipo == 5)
                {
                    FileData dat = files.Where(x => (x.Extension.Contains("gen") || x.Extension.Contains("GEN")) &&
                      x.FileName.Contains("PREPROCESADOR_")).FirstOrDefault();

                    if (dat != null)
                    {
                        entity.Folder = path + dat.FileName;

                        blobs.Add(entity.Folder);
                        names.Add("PREPROCESADOR_" + entity.Cmgnfecha.ToString("yyyyMMddHHmm") + ".gen");
                    }
                }
            }

            string folerZip = string.Empty;

            if (tipo == 1)
                folerZip = "ENTRADAGAMS_";
            else if (tipo == 2)
                folerZip = "PSSE_";
            else if (tipo == 3)
                folerZip = "RESULTADOGAMS_";
            else if (tipo == 4)
                folerZip = "RESULTADOGAMS_ANALISIS_";
            else if (tipo == 5)
                folerZip = "PREPROCESADOR_";

            //- Generamos el archivo dat            
            string result = folerZip + fechaProceso.ToString("yyyyMMdd") + ".zip";
            string fileName = pathExportacion + result;

            FileServerScada.DownloadAsZipRenombadro(blobs, names, fileName, pathTrabajo);

            return result;
        }


        #endregion

        #region Métodos Tabla CM_PARAMETRO

        /// <summary>
        /// Inserta un registro de la tabla CM_PARAMETRO
        /// </summary>
        public int SaveCmParametro(CmParametroDTO entity)
        {
            try
            {
                int id = 0;
                if (entity.Cmparcodi == 0)
                {
                    id = FactorySic.GetCmParametroRepository().Save(entity);
                }
                else
                {
                    FactorySic.GetCmParametroRepository().Update(entity);
                    id = entity.Cmparcodi;
                }
                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CM_PARAMETRO
        /// </summary>
        public void UpdateCmParametro(CmParametroDTO entity)
        {
            try
            {
                FactorySic.GetCmParametroRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CM_PARAMETRO
        /// </summary>
        public void DeleteCmParametro(int cmparcodi)
        {
            try
            {
                FactorySic.GetCmParametroRepository().Delete(cmparcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CM_PARAMETRO
        /// </summary>
        public CmParametroDTO GetByIdCmParametro(int cmparcodi)
        {
            return FactorySic.GetCmParametroRepository().GetById(cmparcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CM_PARAMETRO
        /// </summary>
        public List<CmParametroDTO> ListCmParametros()
        {
            return FactorySic.GetCmParametroRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CmParametro
        /// </summary>
        public List<CmParametroDTO> GetByCriteriaCmParametros()
        {
            return FactorySic.GetCmParametroRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla CM_GENERACION_EMS

        /// <summary>
        /// Inserta un registro de la tabla CM_GENERACION_EMS
        /// </summary>
        public void SaveCmGeneracionEms(CmGeneracionEmsDTO entity)
        {
            try
            {
                FactorySic.GetCmGeneracionEmsRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CM_GENERACION_EMS
        /// </summary>
        public void UpdateCmGeneracionEms(CmGeneracionEmsDTO entity)
        {
            try
            {
                FactorySic.GetCmGeneracionEmsRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CM_GENERACION_EMS
        /// </summary>
        public void DeleteCmGeneracionEms(int genemscodi)
        {
            try
            {
                FactorySic.GetCmGeneracionEmsRepository().Delete(genemscodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CM_GENERACION_EMS
        /// </summary>
        public CmGeneracionEmsDTO GetByIdCmGeneracionEms(int genemscodi)
        {
            return FactorySic.GetCmGeneracionEmsRepository().GetById(genemscodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listaGeneracion">Listado de data con Generacion EMS</param>
        /// <param name="fechaProceso">Fecha hora a la que pertenece el proceso</param>
        /// <param name="correlativo">Correlativo del proceso</param>
        public void AlmacenarDataGeneracionEms(List<EqRelacionDTO> listaGeneracion, DateTime fechaProceso, int correlativo, string estimador)
        {
            FactorySic.GetCmGeneracionEmsRepository().DeleteByFecha(fechaProceso, estimador);

            DateTime fechaDatos = new DateTime(fechaProceso.Year, fechaProceso.Month, fechaProceso.Day, fechaProceso.Hour, fechaProceso.Minute, 0);
            DateTime fechaDatosFinal = (fechaDatos.Hour == 0 && fechaDatos.Minute == 0) ? fechaDatos.AddMinutes(-1) : fechaDatos;

            foreach (var item in listaGeneracion)
            {
                CmGeneracionEmsDTO oCmGeneracionEmsDTO = new CmGeneracionEmsDTO
                {
                    Equicodi = item.Equicodi ?? -1,
                    Emprcodi = item.Emprcodi,
                    Genemsfecha = fechaDatosFinal,
                    Cmgncorrelativo = correlativo,
                    Genemsfechacreacion = DateTime.Now,
                    Genemsusucreacion = "procesoCMN",
                    Genemsgeneracion = (decimal?)item.PotGenerada,
                    Genemsoperativo = Convert.ToInt32(item.IndOperacion),
                    Genemstipoestimador = estimador,
                    Genemspotmax = item.PotMax,
                    Genemspotmin = item.PotMin

                };
                FactorySic.GetCmGeneracionEmsRepository().Save(oCmGeneracionEmsDTO);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="listaGeneracion">Listado de data con Generacion EMS</param>
        /// <param name="fechaProceso">Fecha hora a la que pertenece el proceso</param>
        /// <param name="correlativo">Correlativo del proceso</param>
        public void AlmacenarFlujoLineas(List<CmFlujoPotenciaDTO> listaFlujo, DateTime fechaProceso)
        {
            DateTime fechaDatos = new DateTime(fechaProceso.Year, fechaProceso.Month, fechaProceso.Day, fechaProceso.Hour, fechaProceso.Minute, 0);
            DateTime fechaDatosFinal = (fechaDatos.Hour == 0 && fechaDatos.Minute == 0) ? fechaDatos.AddMinutes(-1) : fechaDatos;

            foreach (CmFlujoPotenciaDTO item in listaFlujo)
            {
                item.Flupotfecha = fechaDatosFinal;
                item.Flupotusucreacion = "procesoCMN";
                item.Flupotfechacreacion = DateTime.Now;

                FactorySic.GetCmFlujoPotenciaRepository().Save(item);
            }
        }


        /// <summary>
        /// Listado de Generación EMS por equipos Generadores
        /// </summary>
        /// <param name="correlativo">Correlativo de proceso de Cálculo de C. Marginal Nodal</param>
        /// <returns></returns>
        public List<CmGeneracionEmsDTO> ObtenerGeneracionEmsPorCorrelativo(int correlativo)
        {
            return FactorySic.GetCmGeneracionEmsRepository().ObtenerGeneracionPorCorrelativo(correlativo);
        }
        /// <summary>
        /// Método para obtener la Generación EMS en un rango de fechas.
        /// </summary>
        /// <param name="fechaIicio">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>Listado de generacion ems</returns>
        public List<CmGeneracionEmsDTO> ObtenerGeneracionEmsEntreFechas(DateTime fechaIicio, DateTime fechaFin, string estimador)
        {
            return FactorySic.GetCmGeneracionEmsRepository().ObtenerGeneracionPorFechas(fechaIicio, fechaFin, estimador);
        }

        #endregion

        #region Métodos Tabla CM_FLUJO_POTENCIA

        /// <summary>
        /// Inserta un registro de la tabla CM_FLUJO_POTENCIA
        /// </summary>
        public void SaveCmFlujoPotencia(CmFlujoPotenciaDTO entity)
        {
            try
            {
                FactorySic.GetCmFlujoPotenciaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CM_FLUJO_POTENCIA
        /// </summary>
        public void UpdateCmFlujoPotencia(CmFlujoPotenciaDTO entity)
        {
            try
            {
                FactorySic.GetCmFlujoPotenciaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CM_FLUJO_POTENCIA
        /// </summary>
        public void DeleteCmFlujoPotencia(int flupotcodi)
        {
            try
            {
                FactorySic.GetCmFlujoPotenciaRepository().Delete(flupotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CM_FLUJO_POTENCIA
        /// </summary>
        public CmFlujoPotenciaDTO GetByIdCmFlujoPotencia(int flupotcodi)
        {
            return FactorySic.GetCmFlujoPotenciaRepository().GetById(flupotcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CM_FLUJO_POTENCIA
        /// </summary>
        public List<CmFlujoPotenciaDTO> ListCmFlujoPotencias()
        {
            return FactorySic.GetCmFlujoPotenciaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CmFlujoPotencia
        /// </summary>
        public List<CmFlujoPotenciaDTO> GetByCriteriaCmFlujoPotencias()
        {
            return FactorySic.GetCmFlujoPotenciaRepository().GetByCriteria();
        }

        #endregion

        #region Metodos Adicionales

        /// <summary>
        /// Permite obtener la ruta de los archivos con la corrida
        /// </summary>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        public string GetPathCorrida(DateTime fecha, int correlativo)
        {
            int indicador = FactorySic.GetCmCostomarginalRepository().ObtenerIndicadorHora(correlativo);

            DateTime fechaProceso = (indicador == 1) ? fecha.AddDays(1) : fecha;

            //- Seteamos la carpeta correspondiente al dia
            string pathDia = fechaProceso.Year + @"/" +
                      fechaProceso.Day.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) +
                      fechaProceso.Month.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) + @"/";

            //- Ruta de los archivos de la corrdida
            string pathCorrida = pathDia + ConstantesCortoPlazo.FolderCorrida + correlativo + @"/";

            return pathCorrida;
        }

        /// <summary>
        /// Permite obtener la carpeta princiap de costos marginales
        /// </summary>
        /// <returns></returns>
        public string GetPathPrincipal()
        {
            //- Definimos la carpeta raiz
            string pathRaiz = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathCostosMarginales];
            return pathRaiz;
        }

        /// <summary>
        /// Permite almacenar en base de datos los costos marginales de barras programados
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void CargarCostosMarginalesProgramados(DateTime fechaInicio, DateTime fechaFin, string usuario)
        {
            try
            {
                List<CmCostomarginalprogDTO> listProgramado = new List<CmCostomarginalprogDTO>();
                List<CmConfigbarraDTO> list = FactorySic.GetCmConfigbarraRepository().GetByCriteria((-1).ToString(), ConstantesAppServicio.SI);
                int nroDias = (int)fechaFin.Subtract(fechaInicio).TotalDays;

                for (int i = 0; i <= nroDias; i++)
                {
                    DateTime fecha = new DateTime(fechaInicio.Year, fechaInicio.Month, fechaInicio.Day).AddDays(i);

                    //- Listado de formulas generales
                    List<PrGrupodatDTO> formulasGeneral = FactorySic.GetPrGrupodatRepository().ObtenerParametroGeneral(fecha);

                    n_parameter parameterGeneral = new n_parameter();
                    foreach (PrGrupodatDTO itemConcepto in formulasGeneral)
                    {
                        if (!string.IsNullOrEmpty(itemConcepto.Concepabrev) && !string.IsNullOrEmpty(itemConcepto.Formuladat))
                            parameterGeneral.SetData(itemConcepto.Concepabrev, itemConcepto.Formuladat);
                    }

                    double tipoCambio = parameterGeneral.GetEvaluate(ConstantesCortoPlazo.PropTipoCambio);


                    string[][] data = FileHelper.ObtenerCostosMarginalesProgramados(fecha);

                    if (data.Length == 49)
                    {
                        foreach (CmConfigbarraDTO item in list)
                        {
                            int index = 0;

                            if (!string.IsNullOrEmpty(item.Cnfbarnombncp))
                            {
                                for (int k = 0; k < data[0].Length; k++)
                                {
                                    if (data[0][k].Trim() == item.Cnfbarnombncp)
                                    {
                                        index = k;
                                        break;
                                    }
                                }
                            }

                            if (index > 0)
                            {
                                for (int j = 1; j <= 48; j++)
                                {
                                    decimal total = 0;
                                    if (decimal.TryParse(data[j][index], out total)) { };

                                    CmCostomarginalprogDTO cmprog = new CmCostomarginalprogDTO
                                    {
                                        Cmarprtotal = total * (decimal)tipoCambio,
                                        Cnfbarcodi = item.Cnfbarcodi,
                                        Cmarprfecha = fecha.AddMinutes(30 * j),
                                        Cmarprlastuser = usuario,
                                        Cmarprlastdate = DateTime.Now
                                    };

                                    if (((DateTime)cmprog.Cmarprfecha).Hour == 23 && ((DateTime)cmprog.Cmarprfecha).Minute == 59)
                                    {
                                        cmprog.Cmarprfecha = ((DateTime)cmprog.Cmarprfecha).AddMinutes(-1);
                                    }

                                    listProgramado.Add(cmprog);
                                }
                            }
                        }
                    }
                }

                //- Insertamos los valores del cm programado

                foreach (CmCostomarginalprogDTO entity in listProgramado)
                {
                    FactorySic.GetCmCostomarginalprogRepository().Save(entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #region Ticket IMME
        /// <summary>
        /// Permite almacenar en base de datos los costos marginales de barras programados de Yupana
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="usuario"></param>
        public void CargarCostosMarginalesProgramadosYupana(DateTime fechaInicio, DateTime fechaFin, string usuario)
        {
            decimal? costo;
            List<CpMedicion48DTO> listaCm;
            try
            {
                List<CmCostomarginalprogDTO> listProgramado = new List<CmCostomarginalprogDTO>();

                List<CpTopologiaDTO> ltop = FactorySic.GetCpTopologiaRepository().ListaTopFinalDiario(fechaInicio, fechaFin);
                for (var fecha = fechaInicio; fecha <= fechaFin; fecha = fecha.AddDays(1))
                {
                    //PubSub<object>.RaiseEvent("MensajeWaitWindowSend", this, new PubSubEventArgs<object>("Procesando dia " + fecha.Day.ToString(), 1));
                    //listProgramado.Clear();
                    var findEsc = ltop.Find(x => x.Topfecha == fecha);
                    if (findEsc != null)
                    {

                        listaCm = FactorySic.GetCpMedicion48Repository().ObtieneCostoMarginalBarraEscenario(findEsc.Topcodi, ConstantesBase.SresCostoMarginalBarra, fecha);
                        //- Listado de formulas generales
                        List<PrGrupodatDTO> formulasGeneral = FactorySic.GetPrGrupodatRepository().ParametrosGeneralesPorFecha(fecha);

                        n_parameter parameterGeneral = new n_parameter();
                        foreach (PrGrupodatDTO itemConcepto in formulasGeneral)
                        {
                            if (!string.IsNullOrEmpty(itemConcepto.Concepabrev) && !string.IsNullOrEmpty(itemConcepto.Formuladat))
                                parameterGeneral.SetData(itemConcepto.Concepabrev, itemConcepto.Formuladat);
                        }

                        double tipoCambio = parameterGeneral.GetEvaluate(ConstantesCortoPlazo.PropTipoCambio);

                        var lista = listaCm.Where(x => x.Medifecha == fecha);
                        foreach (var reg in lista)
                        {
                            for (int k = 1; k <= 48; k++)
                            {
                                costo = (decimal?)reg.GetType().GetProperty("H" + k.ToString()).GetValue(reg, null);
                                if (costo != null)
                                {
                                    CmCostomarginalprogDTO cmprog = new CmCostomarginalprogDTO
                                    {
                                        Cmarprtotal = costo * (decimal)tipoCambio,
                                        Cnfbarcodi = reg.Cnfbarcodi, //Cnfbarcodi
                                        Cmarprfecha = (k < 48) ? fecha.AddMinutes(30 * k) : fecha.AddMinutes(30 * k - 1),
                                        Cmarprlastuser = usuario,
                                        Cmarprlastdate = DateTime.Now
                                    };
                                    listProgramado.Add(cmprog);
                                }

                            }
                        }
                        //FactorySic.GetCmCostomarginalprogRepository().DeleteDia(fecha);
                        //foreach (CmCostomarginalprogDTO entity in listProgramado)
                        //{
                        //    FactorySic.GetCmCostomarginalprogRepository().Save(entity);
                        //}
                    }
                }
                // Falta Borrar registros de la misma fecha
                FactorySic.GetCmCostomarginalprogRepository().GrabarDatosBulk(listProgramado, fechaInicio, fechaFin);
                //- Insertamos los valores del cm programado


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///SIOSEIN-PRIE-2021
        /// <summary>
        /// Obtener los costos marginales de barras programados de Yupana para una barra en específica
        /// </summary>
        /// <param name="barrcodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="usuario"></param>
        public List<CmCostomarginalprogDTO> ObtenerCostosMarginalesProgramadosYupanaParaUnaBarra(int barrcodi, DateTime fechaInicio, DateTime fechaFin, string usuario)
        {
            decimal? costo;
            List<CpMedicion48DTO> listaCm;
            try
            {
                List<CmCostomarginalprogDTO> listProgramado = new List<CmCostomarginalprogDTO>();

                List<CpTopologiaDTO> ltop = FactorySic.GetCpTopologiaRepository().ListaTopFinalDiario(fechaInicio, fechaFin);
                for (var fecha = fechaInicio; fecha <= fechaFin; fecha = fecha.AddDays(1))
                {
                    //PubSub<object>.RaiseEvent("MensajeWaitWindowSend", this, new PubSubEventArgs<object>("Procesando dia " + fecha.Day.ToString(), 1));
                    //listProgramado.Clear();
                    var findEsc = ltop.Find(x => x.Topfecha == fecha);
                    if (findEsc != null)
                    {

                        listaCm = FactorySic.GetCpMedicion48Repository().ObtieneCostoMarginalBarraEscenarioParaUnaBarra(barrcodi, findEsc.Topcodi, ConstantesBase.SresCostoMarginalBarra, fecha);
                        //- Listado de formulas generales
                        List<PrGrupodatDTO> formulasGeneral = FactorySic.GetPrGrupodatRepository().ParametrosGeneralesPorFecha(fecha);

                        n_parameter parameterGeneral = new n_parameter();
                        foreach (PrGrupodatDTO itemConcepto in formulasGeneral)
                        {
                            if (!string.IsNullOrEmpty(itemConcepto.Concepabrev) && !string.IsNullOrEmpty(itemConcepto.Formuladat))
                                parameterGeneral.SetData(itemConcepto.Concepabrev, itemConcepto.Formuladat);
                        }

                        double tipoCambio = parameterGeneral.GetEvaluate(ConstantesCortoPlazo.PropTipoCambio);

                        var lista = listaCm.Where(x => x.Medifecha == fecha);
                        foreach (var reg in lista)
                        {
                            for (int k = 1; k <= 48; k++)
                            {
                                costo = (decimal?)reg.GetType().GetProperty("H" + k.ToString()).GetValue(reg, null);
                                if (costo != null)
                                {
                                    CmCostomarginalprogDTO cmprog = new CmCostomarginalprogDTO
                                    {
                                        Cmarprtotal = costo * (decimal)tipoCambio,
                                        Cnfbarcodi = reg.Cnfbarcodi, //Cnfbarcodi
                                        Cmarprfecha = (k < 48) ? fecha.AddMinutes(30 * k) : fecha.AddMinutes(30 * k - 1),
                                        Cmarprlastuser = usuario,
                                        Cmarprlastdate = DateTime.Now,
                                        Osinergcodi = reg.Osinergcodi,
                                        Cnfbarnombre = reg.Cnfbarnombre
                                    };

                                    listProgramado.Add(cmprog);
                                }

                            }
                        }
                    }
                }

                return listProgramado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        ///SIOSEIN-PRIE-2021

        /// <summary>
        /// Devuelve lista de cantidad de registros por mes
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <returns></returns>
        public List<CmCostomarginalprogDTO> ListaTotalXDia(DateTime fechaInicio)
        {
            DateTime fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
            List<CmCostomarginalprogDTO> entitys = FactorySic.GetCmCostomarginalprogRepository().ListaTotalXDia(fechaInicio, fechaFin);
            return entitys;
        }

        #endregion

        /// <summary>
        /// Permite determinar el modo de operación de una unidad
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="usuario"></param>
        public void DeterminarModosOperacion(DateTime fechaInicio, DateTime fechaFin, string usuario)
        {
            try
            {
                //- Listamos las relaciones
                List<EqRelacionDTO> relacion = FactorySic.GetEqRelacionRepository().ObtenerConfiguracionProceso(ConstantesCortoPlazo.FuenteGeneracion);

                int nroDias = (int)fechaFin.Subtract(fechaInicio).TotalDays;

                for (int i = 0; i <= nroDias; i++)
                {
                    DateTime fecha = new DateTime(fechaInicio.Year, fechaInicio.Month, fechaInicio.Day).AddDays(i);

                    for (int j = 1; j <= 48; j++)
                    {
                        DateTime fechaProceso = fecha.AddMinutes(j * 30);

                        //- Lista los modos de operación activos en la hora
                        List<int> modosOperacion = FactorySic.GetEqRelacionRepository().ObtenerModosOperacion(fechaProceso);

                        if (fechaProceso.Hour == 0 && fechaProceso.Minute == 0)
                        {
                            fechaProceso = fechaProceso.AddMinutes(-1);
                        }

                        List<CmGeneracionEmsDTO> result = new List<CmGeneracionEmsDTO>();

                        foreach (EqRelacionDTO entity in relacion)
                        {
                            if (entity.IndTipo == ConstantesCortoPlazo.TipoTermica)
                            {
                                int idModoOperacion = 0;

                                //- Buscamos el modo de operación en las horas de operación
                                if (!string.IsNullOrEmpty(entity.Modosoperacion))
                                {
                                    List<int> modos = entity.Modosoperacion.Split(ConstantesAppServicio.CaracterComa).Select(int.Parse).ToList();
                                    foreach (int modo in modos)
                                    {
                                        if (modosOperacion.Where(x => x == modo).Count() > 0)
                                        {
                                            idModoOperacion = modo;
                                            break;
                                        }
                                    }
                                }
                                entity.IdModoOperacion = idModoOperacion;

                                //- Actualizamos para la hora respectiva el modo de operación que utilizó el generador
                                CmGeneracionEmsDTO generacion = new CmGeneracionEmsDTO();
                                generacion.Grupocodi = idModoOperacion;
                                generacion.Equicodi = (int)entity.Equicodi;
                                generacion.Genemsfecha = fechaProceso;
                                generacion.Genemsusucreacion = usuario;
                                generacion.IndModoOperacion = (entity.Indcc == 1) ? true : false;
                                generacion.IndTv = (entity.Indtvcc == ConstantesAppServicio.SI) ? true : false;
                                generacion.IdCicloComb = (entity.Ccombcodi != null) ? (int)entity.Ccombcodi : 0;

                                if (idModoOperacion > 0)
                                {
                                    result.Add(generacion);
                                }
                            }
                        }

                        //- Verificamos para los modos de operación
                        List<CmGeneracionEmsDTO> cicloscombinados = result.Where(x => x.IndModoOperacion && x.IndTv).ToList();

                        foreach (CmGeneracionEmsDTO itemGenn in cicloscombinados)
                        {
                            List<EqRelacionDTO> equipos = relacion.Where(x => x.Ccombcodi == itemGenn.IdCicloComb && x.IdModoOperacion == 0).ToList();

                            foreach (EqRelacionDTO subItemGen in equipos)
                            {
                                CmGeneracionEmsDTO generacion = new CmGeneracionEmsDTO();
                                generacion.Grupocodi = itemGenn.Grupocodi;
                                generacion.Equicodi = (int)subItemGen.Equicodi;
                                generacion.Genemsfecha = fechaProceso;
                                generacion.Genemsusucreacion = usuario;
                                result.Add(generacion);
                            }
                        }

                        foreach (CmGeneracionEmsDTO generacion in result)
                        {
                            FactorySic.GetCmGeneracionEmsRepository().ActualizarModoOperacion(generacion);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener la relacion entre generador y barra
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="usuario"></param>
        public void ObtenerRelacionBarraGenerador(DateTime fechaInicio, DateTime fechaFin, string usuario)
        {
            try
            {
                int nroDias = (int)fechaFin.Subtract(fechaInicio).TotalDays;
                List<CmConfigbarraDTO> listBarra = FactorySic.GetCmConfigbarraRepository().GetByCriteria((-1).ToString(), ConstantesAppServicio.SI).
                    Where(x => !string.IsNullOrEmpty(x.Cnfbarnombncp)).ToList();

                for (int i = 0; i <= nroDias; i++)
                {
                    DateTime fecha = new DateTime(fechaInicio.Year, fechaInicio.Month, fechaInicio.Day).AddDays(i);
                    List<EqRelacionDTO> list = (new CostoMarginalAppServicio()).ObtenerRelacionEquivalencia(fecha);
                    string[][] equivalencias = FileHelper.ObtenerRelacionBarraGenerador(fecha);

                    List<CmBarrageneradorDTO> result = new List<CmBarrageneradorDTO>();

                    foreach (EqRelacionDTO item in list)
                    {
                        if (!string.IsNullOrEmpty(item.Nombrencp))
                        {
                            item.Codbarrancp = FileHelper.ObtenerEquivalenciaCentral(item.Nombrencp, equivalencias.ToList());

                            if (!string.IsNullOrEmpty(item.Codbarrancp))
                            {
                                CmConfigbarraDTO barra = listBarra.Where(x => x.Cnfbarnombncp.Trim() == item.Codbarrancp.Trim()).FirstOrDefault();

                                if (barra != null)
                                {
                                    CmBarrageneradorDTO equivalencia = new CmBarrageneradorDTO
                                    {
                                        Bargerfeccreacion = DateTime.Now,
                                        Bargerfecha = fecha,
                                        Bargerusucreacion = usuario,
                                        Cnfbarcodi = barra.Cnfbarcodi,
                                        Relacioncodi = item.Relacioncodi
                                    };

                                    result.Add(equivalencia);
                                }
                            }
                        }
                    }

                    foreach (CmBarrageneradorDTO item in result)
                    {
                        FactorySic.GetCmBarrageneradorRepository().Save(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite obtener el reprograma vigente
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <returns></returns>
        public string ObtenerReprogramaActual(DateTime fechaProceso)
        {
            DateTime fechaProcesoOrigen;

            if (fechaProceso.Hour == 0 && fechaProceso.Minute == 0)
                fechaProcesoOrigen = (new DateTime(fechaProceso.Year, fechaProceso.Month, fechaProceso.Day)).AddDays(-1).AddHours(23).AddMinutes(59);
            else
                fechaProcesoOrigen = fechaProceso;

            List<EveMailsDTO> reprogramas = FactorySic.GetEveMailsRepository().GetListaReprogramado(fechaProcesoOrigen).
                OrderByDescending(x => x.Mailbloquehorario).ToList();

            int bloqueActual = fechaProcesoOrigen.Hour * 2 + fechaProcesoOrigen.Minute % 30;
            string reprograma = string.Empty;

            if (reprogramas.Count > 0)
            {
                foreach (EveMailsDTO item in reprogramas)
                {
                    if (bloqueActual >= (int)item.Mailbloquehorario)
                    {
                        reprograma = item.Mailhoja;
                        break;
                    }
                }
            }

            return reprograma;
        }

        #endregion

        #region Métodos Tabla CM_CONJUNTOENLACE

        /// <summary>
        /// Permite obtener los enlaces pertenecientes a un grupo
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <returns></returns>
        public List<EqCongestionConfigDTO> ObtenerLineasPorGrupo(int idGrupo)
        {
            return FactorySic.GetEqCongestionConfigRepository().ObtenerLineasPorGrupo(idGrupo);
        }

        /// <summary>
        /// Inserta un registro de la tabla CM_CONJUNTOENLACE
        /// </summary>
        public int SaveCmConjuntoenlace(CmConjuntoenlaceDTO entity)
        {
            try
            {
                int resultado = 1;

                List<CmConjuntoenlaceDTO> list = FactorySic.GetCmConjuntoenlaceRepository().GetByCriteria(entity.Grulincodi, entity.Configcodi);

                if (list.Count == 0)
                {
                    FactorySic.GetCmConjuntoenlaceRepository().Save(entity);
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
        /// Elimina un registro de la tabla CM_CONJUNTOENLACE
        /// </summary>
        public void DeleteCmConjuntoenlace(int idGrupo, int idLinea)
        {
            try
            {
                FactorySic.GetCmConjuntoenlaceRepository().Delete(idGrupo, idLinea);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }




        #endregion

        #region Consulta de Datos

        /// <summary>
        /// Permite exportar los datos exportados
        /// </summary>
        /// <param name="opcion"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void ObtenerConsultaDatos(int opcion, DateTime fechaInicio, DateTime fechaFin, string path, string filename)
        {
            string texto = "";
            List<CmCostomarginalDTO> listadoDemanda = new List<CmCostomarginalDTO>();
            List<CmFlujoPotenciaDTO> listadoFlujo = new List<CmFlujoPotenciaDTO>();
            List<CmGeneracionEmsDTO> listadoGeneracion = new List<CmGeneracionEmsDTO>();

            if (opcion == 1)
            {
                listadoDemanda = this.ObtenerReporteCostosMarginales(fechaInicio, fechaFin, "P", "N", 1);
                texto = "DEMANDA EN BARRAS";
            }
            else if (opcion == 2)
            {
                listadoFlujo = this.ObtenerReporteFlujoPotencia(fechaInicio, fechaFin);
                texto = "FLUJO EN LÍNEAS";
            }
            else if (opcion == 3)
            {
                listadoGeneracion = this.ObtenerGeneracionEmsEntreFechas(fechaInicio, fechaFin, "P");
                texto = "GENERACIÓN";
            }

            try
            {
                string file = path + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "REPORTE DE DATOS - " + texto;

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;
                        int lastColumn = 5;
                        if (opcion == 1)
                        {
                            ws.Cells[index, 2].Value = "FECHA HORA";
                            ws.Cells[index, 3].Value = "NODO EMS";
                            ws.Cells[index, 4].Value = "NOMBRE BARRA";
                            ws.Cells[index, 5].Value = "DEMANDA";
                            lastColumn = 5;
                        }
                        else if (opcion == 2)
                        {
                            ws.Cells[index, 2].Value = "FECHA HORA";
                            ws.Cells[index, 3].Value = "EMPRESA";
                            ws.Cells[index, 4].Value = "NODO 1";
                            ws.Cells[index, 5].Value = "NODO 2";
                            ws.Cells[index, 6].Value = "FLUJO 1";
                            ws.Cells[index, 7].Value = "FLUJO 2";
                            lastColumn = 7;
                        }
                        else
                        {
                            ws.Cells[index, 2].Value = "FECHA HORA";
                            ws.Cells[index, 3].Value = "EMPRESA";
                            ws.Cells[index, 4].Value = "EQUIPO";
                            ws.Cells[index, 5].Value = "ABREVIATURA";
                            ws.Cells[index, 6].Value = "GENERACIÓN";
                            lastColumn = 6;
                        }

                        rg = ws.Cells[index, 2, index, lastColumn];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 6;


                        if (opcion == 1)
                        {
                            foreach (CmCostomarginalDTO item in listadoDemanda)
                            {
                                ws.Cells[index, 2].Value = item.Cmgnfecha.ToString("dd/MM/yyyy HH:mm");
                                ws.Cells[index, 3].Value = item.Cnfbarnodo;
                                ws.Cells[index, 4].Value = item.Cnfbarnombre;
                                ws.Cells[index, 5].Value = item.Cmgndemanda;
                                rg = ws.Cells[index, 2, index, lastColumn];
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                index++;
                            }
                        }
                        else if (opcion == 2)
                        {
                            foreach (CmFlujoPotenciaDTO item in listadoFlujo)
                            {
                                ws.Cells[index, 2].Value = ((DateTime)item.Flupotfecha).ToString("dd/MM/yyyy HH:mm");
                                ws.Cells[index, 3].Value = item.Emprnomb;
                                ws.Cells[index, 4].Value = item.Nodobarra1;
                                ws.Cells[index, 5].Value = item.Nodobarra2;
                                ws.Cells[index, 6].Value = item.Flupotvalor1;
                                ws.Cells[index, 7].Value = item.Flupotvalor2;
                                rg = ws.Cells[index, 2, index, lastColumn];
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                index++;
                            }
                        }
                        else if (opcion == 3)
                        {
                            foreach (CmGeneracionEmsDTO item in listadoGeneracion)
                            {
                                ws.Cells[index, 2].Value = item.Genemsfecha.ToString("dd/MM/yyyy HH:mm");
                                ws.Cells[index, 3].Value = item.Emprnomb;
                                ws.Cells[index, 4].Value = item.Equinomb;
                                ws.Cells[index, 5].Value = item.Equiabrev;
                                ws.Cells[index, 6].Value = item.Genemsgeneracion;
                                rg = ws.Cells[index, 2, index, lastColumn];
                                rg.Style.Font.Size = 10;
                                rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                index++;
                            }
                        }

                        rg = ws.Cells[5, 2, index - 1, lastColumn];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;

                        rg = ws.Cells[5, 3, index, lastColumn];
                        rg.AutoFitColumns();

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("Logo", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }


        #endregion

        #region TABLAS DE ANALISIS GAMS

        /// <summary>
        /// DeleteCmCompensacion 
        /// </summary>
        /// <param name="intervalo"></param>
        /// <param name="fecha"></param>
        public void DeleteCmCompensacion(int intervalo, DateTime fecha)
        {
            try
            {
                FactorySic.GetCmCompensacionRepository().DeleteByCriteria(intervalo, fecha);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// DeleteCmGeneracion
        /// </summary>
        /// <param name="intervalo"></param>
        /// <param name="fecha"></param>
        public void DeleteCmGeneracion(int intervalo, DateTime fecha)
        {
            try
            {
                FactorySic.GetCmGeneracionRepository().DeleteByCriteria(intervalo, fecha);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// DeleteCmDemandaTotal
        /// </summary>
        /// <param name="intervalo"></param>
        /// <param name="fecha"></param>
        public void DeleteCmDemandaTotal(int intervalo, DateTime fecha)
        {
            try
            {
                FactorySic.GetCmDemandatotalRepository().DeleteByCriteria(intervalo, fecha);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Guarda en la tabla CM_COMPENSACION
        /// </summary>
        /// <param name="entity"></param>
        public int SaveCmCompensacion(CmCompensacionDTO entity)
        {
            int rtn = -1;
            try
            {
                rtn = FactorySic.GetCmCompensacionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            return rtn;
        }
        /// <summary>
        ///  Guarda en la tabla CM_DEMANDATOTAL
        /// </summary>
        /// <param name="entity"></param>
        public int SaveCmDemandatotal(CmDemandatotalDTO entity)
        {
            int rtn = -1;
            try
            {
                rtn = FactorySic.GetCmDemandatotalRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            return rtn;
        }
        /// <summary>
        ///  Guarda en la tabla CM_GENERACION
        /// </summary>
        /// <param name="entity"></param>
        public int SaveCmGeneracion(CmGeneracionDTO entity)
        {
            int rtn = -1;
            try
            {
                rtn = FactorySic.GetCmGeneracionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            return rtn;
        }

        public int GetCodByAbrev(string calificacion)
        {
            return FactorySic.GetEveSubcausaeventoRepository().GetCodByAbrev(calificacion);
        }

        /// <summary>
        /// ObtenerCostosMarginalesValorizacionDiaria
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="path"></param>
        /// <param name="relacion"></param>
        /// <returns></returns>
        public bool ObtenerCostosMarginalesValorizacionDiaria(DateTime fechaProceso, int periodo, string fileName, string path, List<EqRelacionDTO> relacion,
            List<PrCongestionDTO> congestionSimple, List<PrCongestionDTO> congestionConjunto, List<PrCongestionDTO> congestionRegiones,
            List<RegistroGenerado> resultadoGeneracion, int correlativo)
        {
            try
            {
                //LEER ARCHIVO CSV RESULTADO GAMS ANALISIS
                ResultadoGamsAnalisis listaResultadoAnalisis = new ResultadoGamsAnalisis();
                listaResultadoAnalisis = FileHelper.ObtenerResultadoGamsAnalisis(fileName, path, relacion);

                DateTime fechaDatos = (fechaProceso.Hour == 0 && fechaProceso.Minute == 0) ? fechaProceso.AddMinutes(-1) : fechaProceso;


                DeleteCmCompensacion(periodo, fechaDatos);

                //CM_COMPENSACION
                List<ResultadoCompensacionesTermicas> lct = listaResultadoAnalisis.ListaCompensacionTermica;
                foreach (var item in lct)
                {
                    //int codSubCausaEven = GetCodByAbrev(item.Calificacion);

                    CmCompensacionDTO compensacion = new CmCompensacionDTO
                    {
                        Equicodi = item.CodigoEquipo,
                        Subcausaevencodi = int.Parse(item.Calificacion),
                        Compfecha = fechaDatos,
                        Compintervalo = periodo,
                        Compvalor = item.Compensacion,
                        Compsucreacion = "webapp",
                        Compfeccreacion = DateTime.Now,
                        Compusumodificacion = "webapp",
                        Compfecmodificacion = DateTime.Now
                    };
                    //Guardar en la Tabla CM_COMPENSACION
                    SaveCmCompensacion(compensacion);
                }

                //CM_GENERACION

                DeleteCmGeneracion(periodo, fechaDatos);

                List<ResultadoGeneracionTermica> gt = listaResultadoAnalisis.ListaGeneracionTermica;
                foreach (var item in gt)
                {
                    CmGeneracionDTO generacionT = new CmGeneracionDTO
                    {
                        Equicodi = item.CodigoEquipo,
                        Genefecha = fechaDatos,
                        Geneintervalo = periodo,
                        Genevalor = item.PgMW,
                        Genesucreacion = "webapp",
                        Genefeccreacion = DateTime.Now,
                        Geneusumodificacion = "webapp",
                        Genefecmodificacion = DateTime.Now
                    };
                    //Guardar en la Tabla CM_GENERACION
                    SaveCmGeneracion(generacionT);
                }

                //CM_GENERACION
                List<ResultadoGeneracionHidraulica> gh = listaResultadoAnalisis.ListaGeneracionHidraulica;
                foreach (var item in gh)
                {
                    CmGeneracionDTO generacionH = new CmGeneracionDTO
                    {
                        Equicodi = item.CodigoEquipo,
                        Genefecha = fechaDatos,
                        Geneintervalo = periodo,
                        Genevalor = item.PhMW,
                        Genesucreacion = "webapp",
                        Genefeccreacion = DateTime.Now,
                        Geneusumodificacion = "webapp",
                        Genefecmodificacion = DateTime.Now
                    };
                    //Guardar en la Tabla CM_DEMANDATOTAL
                    SaveCmGeneracion(generacionH);
                }

                //CM_DEMANDATOTAL
                DeleteCmDemandaTotal(periodo, fechaDatos);

                ResultadoResumen dt = listaResultadoAnalisis.Resumen;
                CmDemandatotalDTO demandatotal = new CmDemandatotalDTO
                {
                    Demafecha = fechaDatos,
                    Demaintervalo = periodo,
                    Dematermica = dt.GeneracionTermica,
                    Demahidraulica = dt.GeneracionHidraulica,
                    Dematotal = dt.GeneracionDemandaTotal,
                    Demasucreacion = "webapp",
                    Demafeccreacion = DateTime.Now,
                    Demausumodificacion = "webapp",
                    Demafecmodificacion = DateTime.Now
                };
                //Guardar en la Tabla CM_DEMANDATOTAL
                SaveCmDemandatotal(demandatotal);

                //- Almacenar congestiones de grupos
                List<ResultadoCongestion> congestiones = new List<ResultadoCongestion>();
                congestiones.AddRange(listaResultadoAnalisis.ListaCongestion);
                congestiones.AddRange(listaResultadoAnalisis.ListaCongestionConjunta);
                congestiones.AddRange(listaResultadoAnalisis.ListaCongestionRegionArriba);
                congestiones.AddRange(listaResultadoAnalisis.ListaCongestionRegionAbajo);

                FactorySic.GetCmCongestionCalculoRepository().Delete(periodo, fechaDatos);

                foreach (ResultadoCongestion itemCongestion in congestiones)
                {
                    int? idCongif = null;
                    int? idGrupo = null;
                    int? idRegSeg = null;

                    if (itemCongestion.Tipo == 0)
                    {
                        PrCongestionDTO cs = congestionSimple.Where(x => "\"" + x.NombreResultado + "\"" == itemCongestion.NombreCongestion).FirstOrDefault();
                        if (cs != null) idCongif = cs.Configcodi;
                    }
                    else if (itemCongestion.Tipo == 1)
                    {
                        PrCongestionDTO cc = congestionConjunto.Where(x => "\"" + x.NombreResultado + "\"" == itemCongestion.NombreCongestion).FirstOrDefault();
                        if (cc != null) idGrupo = cc.Grulincodi;
                    }
                    else
                    {
                        PrCongestionDTO rs = congestionRegiones.Where(x => "\"" + x.NombreResultado + "\"" == itemCongestion.NombreCongestion).FirstOrDefault();
                        if (rs != null) idRegSeg = rs.Regsegcodi;
                    }

                    CmCongestionCalculoDTO congestion = new CmCongestionCalculoDTO
                    {
                        Cmconfecha = fechaDatos,
                        Cmcongcongestion = itemCongestion.Congestion,
                        Cmcongenvio = itemCongestion.Envio,
                        Cmconggeneracion = itemCongestion.Generacion,
                        Cmconggenlimite = itemCongestion.GenLimite,
                        Cmconglimite = itemCongestion.Limite,
                        Cmcongperiodo = periodo,
                        Cmgncorrelativo = correlativo,
                        Cmcongrecepcion = itemCongestion.Recepcion,
                        Configcodi = idCongif,
                        Grulincodi = idGrupo,
                        Regsegcodi = idRegSeg,
                        Cmcongusucreacion = "webapp",
                        Cmcongfeccreacion = DateTime.Now
                    };

                    FactorySic.GetCmCongestionCalculoRepository().Save(congestion);
                }

                FactorySic.GetCmCostoIncrementalRepository().Delete(periodo, fechaDatos);

                //- Almacenamos costos incrementales
                List<RegistroGenerado> listTermica = resultadoGeneracion.Where(x => x.IndRegistro == 1).ToList();

                foreach (RegistroGenerado item in listTermica)
                {
                    CmCostoIncrementalDTO costoIncremental = new CmCostoIncrementalDTO
                    {
                        Cmcifecha = fechaDatos,
                        Cmciperiodo = periodo,
                        Cmcitramo1 = (!string.IsNullOrEmpty(item.Ci1)) ? decimal.Parse(item.Ci1) : 0,
                        Cmcitramo2 = (!string.IsNullOrEmpty(item.Ci2)) ? decimal.Parse(item.Ci2) : (!string.IsNullOrEmpty(item.Ci1)) ? decimal.Parse(item.Ci1) : 0,
                        Equicodi = item.Equicodi,
                        Grupocodi = item.ModoOperacion,
                        Cmgncorrelativo = correlativo,
                        Cmciusucreacion = "webapp",
                        Cmcifeccreacion = DateTime.Now
                    };

                    FactorySic.GetCmCostoIncrementalRepository().Save(costoIncremental);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("ObtenerCostosMarginalesValorizacionDiaria" + ex);

                return false;
            }
        }



        #endregion

        /// <summary>
        /// Obtiene las versiones de las restricciones utilizadas
        /// </summary>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        public List<CmRestriccionDTO> ObtenerRestriccionesPorCorrida(int correlativo)
        {
            return FactorySic.GetCmRestriccionRepository().ObtenerRestriccionPorCorrida(correlativo);
        }

        /// <summary>
        /// Obtiene la version del programa utilizado
        /// </summary>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        public CmVersionprogramaDTO ObtenerVersionPrograma(int correlativo)
        {
            return FactorySic.GetCmVersionprogramaRepository().GetById(correlativo);
        }

        #region Métodos tabla CP_TOPOLOGIA

        /// <summary>
        /// Actualiza  la tabla CP_ESCTOPOLOGIA
        /// </summary>
        //public void UpdateCpEsctopologia(CpTopologiaDTO entity, int modo)
        //{
        //    try
        //    {
        //        if (modo == ConstantesBase.ModoOnLine)
        //            FactorySic.GetCptopologiaRepository().Update(entity);
        //        else
        //            (new EscenarioRepositorioCSV(0)).Save(entity);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ConstantesAppServicio.LogError, ex);
        //        throw new Exception(ex.Message, ex);
        //    }
        //}

        /// <summary>
        /// Obtiene el registro del escenario solicitado
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="modo"></param>
        /// <returns></returns>
        //public CpTopologiaDTO ObtenerEscenario(int topcodi, int modo)
        //{
        //    if (modo == ConstantesBase.ModoOnLine)
        //        return FactorySic.GetCptopologiaRepository().GetById(topcodi);
        //    else
        //        return (new EscenarioRepositorioCSV(0)).LeerRegistro();
        //}


        /// <summary>
        /// Obtiene el registro del escenario solicitado
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="modo"></param>
        /// <returns></returns>
        public CpTopologiaDTO ObtenerEscenarioSinRsf(int topcodi)
        {

            return FactorySic.GetCptopologiaRepository().GetByIdRsf(topcodi);

        }

        /// <summary>
        /// Lista todos los escenarios del corto plazo
        /// </summary>
        /// <returns></returns>
        public List<CpTopologiaDTO> ListarEscenario()
        {
            return FactorySic.GetCptopologiaRepository().List();
        }

        public List<CpTopologiaDTO> ListarEscenario(DateTime fechaInicio, DateTime fechaFin, short idTipo)
        {
            return FactorySic.GetCptopologiaRepository().GetByCriteria(fechaInicio, fechaFin, idTipo);
        }

        /// <summary>
        /// Lista todos los escenarios del corto plazo
        /// </summary>
        /// <returns></returns>
        public List<CpTopologiaDTO> ListarFiltroNommbreEscenario(string nombre)
        {
            return FactorySic.GetCptopologiaRepository().ListNombre(nombre);
        }


        //MÉTODO PARA LISTAR COMBO REPROGRAMAS
        public List<CpReprogramaDTO> ListarReprogramas(DateTime fecha, int topTipo, int topFinal)
        {
            //calcular el programa
            var listaTopologia = FactorySic.GetCptopologiaRepository().List();
            var programa = listaTopologia.Find(x => x.Topfecha.Date == fecha.Date && x.Toptipo == topTipo && x.Topfinal == topFinal);
            int codPrograma = programa != null ? programa.Topcodi : 0;

            var lista = this.GetByCriteriaCpReprograma(codPrograma);
            return lista;
        }

        #endregion

        #region Métodos tabla CP_REPROGRAMA

        public List<CpReprogramaDTO> GetByCriteriaCpReprograma(int topcodi)
        {

            return FactorySic.GetCpReprogramaRepository().GetByCriteria(topcodi);

        }

        public int SaveCpReprograma(CpReprogramaDTO registro)
        {
            int resultado = 1;
            try
            {
                FactorySic.GetCpReprogramaRepository().Save(registro);
            }
            catch (Exception ex)
            {
                resultado = -1;
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return resultado;
        }

        public int DeleteCpReprograma(int topcodi2)
        {
            int resultado = 1;
            try
            {
                FactorySic.GetCpReprogramaRepository().Delete(topcodi2);
            }
            catch (Exception ex)
            {
                resultado = -1;
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return resultado;
        }

        public List<CpReprogramaDTO> ListTopPrincipal(int topcodi)
        {
            return FactorySic.GetCpReprogramaRepository().ListTopPrincipal(topcodi);
        }

        #endregion

        #region MDCOES

        /// <summary>
        /// Permite obtener la congestion programada del MDCOES
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <returns></returns>
        public List<PrCongestionDTO> ObtenerCongestionProgramadaMdcoes(DateTime fechaProceso)
        {
            List<PrCongestionDTO> lstCongestionMdcoes = new List<PrCongestionDTO>();

            CpTopologiaDTO topologiaFinal = this.ObtenerTopologiaFinalPorFecha(fechaProceso, ConstantesCortoPlazo.TopologiaDiario);

            //Lista costo marginal
            List<CpMedicion48DTO> lstCmgLineas = this.ObtenerCpMedicion48(topologiaFinal, fechaProceso, ConstantesCortoPlazo.SrestcodiCmgLineas.ToString(), true);
            List<CpMedicion48DTO> lstCmgSumaFlujoSupEInf = this.ObtenerCpMedicion48(topologiaFinal, fechaProceso, $"{ConstantesCortoPlazo.SrestcodiCmgSumaFlujSup},{ConstantesCortoPlazo.SrestcodiCmgSumaFlujInf}", true);

            //Informacion de recursos con congestion
            List<CpMedicion48DTO> lstCongestionLinea = ObtenerInformacionCongestion(lstCmgLineas);
            List<CpMedicion48DTO> lstCongestionConjunta = ObtenerInformacionCongestion(lstCmgSumaFlujoSupEInf);

            //Configuracion congestion
            List<EqCongestionConfigDTO> lstConfiguracionLinea = FactorySic.GetEqCongestionConfigRepository().List();
            List<EqGrupoLineaDTO> lstConfiguracionGrupo = FactorySic.GetEqGrupoLineaRepository().List();

            List<PrCongestionDTO> congestionLinea = ObtenerRelacionCongestion(lstCongestionLinea, lstConfiguracionLinea);
            List<PrCongestionDTO> congestionConjunta = ObtenerRelacionCongestion(lstCongestionConjunta, lstConfiguracionGrupo);

            lstCongestionMdcoes.AddRange(congestionLinea);
            lstCongestionMdcoes.AddRange(congestionConjunta);

            return lstCongestionMdcoes;
        }

        /// <summary>
        /// Retorna la relacion de las congestiones lineales
        /// </summary>
        /// <param name="lstCongestionLinea"></param>
        /// <param name="configuracionLinea"></param>
        /// <returns></returns>
        private List<PrCongestionDTO> ObtenerRelacionCongestion(List<CpMedicion48DTO> lstCongestionLinea, List<EqCongestionConfigDTO> configuracionLinea)
        {
            List<PrCongestionDTO> lstCongestion = new List<PrCongestionDTO>();

            foreach (var congestionLinea in lstCongestionLinea)
            {

                EqCongestionConfigDTO itemConfig = configuracionLinea.FirstOrDefault(x => x.Equicodi == congestionLinea.Recurcodisicoes);
                if (itemConfig != null)
                {
                    //- Debemos agregar el registro de la congestión de la linea
                    lstCongestion.Add(new PrCongestionDTO
                    {
                        Fuente = UtilCortoPlazo.ObtenerFuenteCongestion((int)itemConfig.Famcodi),
                        Grulincodi = null,
                        Configcodi = itemConfig.Configcodi,
                        Equinomb = itemConfig.Equinomb,
                        Congesfecinicio = UtilCortoPlazo.ObtenerRangoFecha(congestionLinea.Medifecha, congestionLinea.HxIni, 1),
                        Congesfecfin = UtilCortoPlazo.ObtenerRangoFecha(congestionLinea.Medifecha, congestionLinea.HxFin, 0)
                    });
                }
            }

            return lstCongestion;
        }

        /// <summary>
        /// Retorna la relacio de congestion en los conjuntos de líneas
        /// </summary>
        /// <param name="lstCongestionConjunta"></param>
        /// <param name="configuracionGrupo"></param>
        /// <returns></returns>
        private List<PrCongestionDTO> ObtenerRelacionCongestion(List<CpMedicion48DTO> lstCongestionConjunta, List<EqGrupoLineaDTO> configuracionGrupo)
        {
            List<PrCongestionDTO> lstCongestion = new List<PrCongestionDTO>();

            //- Relacionamos las congestion en los conjuntos de líneas
            foreach (var congesConjunta in lstCongestionConjunta)
            {
                EqGrupoLineaDTO itemConfig = configuracionGrupo.Find(x => x.Grulincodi == congesConjunta.Recurcodisicoes);

                if (itemConfig != null)
                {
                    //- Debemos agregar el registro de la congestión del conjunto                        
                    lstCongestion.Add(
                        new PrCongestionDTO
                        {
                            Fuente = UtilCortoPlazo.ObtenerFuenteCongestion(ConstantesCortoPlazo.IdConjuntoLinea),
                            Grulincodi = itemConfig.Grulincodi,
                            Configcodi = null,
                            Equinomb = itemConfig.Grulinnombre,
                            Congesfecinicio = UtilCortoPlazo.ObtenerRangoFecha(congesConjunta.Medifecha, congesConjunta.HxIni, 1),
                            Congesfecfin = UtilCortoPlazo.ObtenerRangoFecha(congesConjunta.Medifecha, congesConjunta.HxFin, 0)
                        }
                    );
                }
            }

            return lstCongestion;
        }

        /// <summary>
        /// Permite obtener informacion de congestion, recurso, fecha , hora incio y fin.
        /// </summary>
        /// <param name="lstCongestion"></param>
        /// <returns></returns>
        private List<CpMedicion48DTO> ObtenerInformacionCongestion(List<CpMedicion48DTO> lstCongestion)
        {
            List<CpMedicion48DTO> lstResultado = new List<CpMedicion48DTO>();
            foreach (var congestion in lstCongestion)
            {
                List<int> lstHxCongestion = ObtenerHorasConCongestion(congestion);
                var lstHxConsecutivos = ObtenerBloqueHxIniyFin(lstHxCongestion);

                foreach (var HxConsec in lstHxConsecutivos)
                {
                    lstResultado.Add(new CpMedicion48DTO { Medifecha = congestion.Medifecha, Recurcodi = congestion.Recurcodi, HxIni = HxConsec.Item1, HxFin = HxConsec.Item2 });
                }
            }
            return lstResultado;
        }

        /// <summary>
        /// Permite unir horas consecutivas en un solo bloque con Hora inicio y fin
        /// </summary>
        /// <param name="lstHxCongestion"></param>
        /// <param name="cantConsecutivo"></param>
        /// <returns></returns>
        public IEnumerable<Tuple<int, int>> ObtenerBloqueHxIniyFin(List<int> lstHxCongestion, int cantConsecutivo = 1)
        {
            int hxInicio, hxFin;
            using (var enumeratorHx = lstHxCongestion.GetEnumerator())
            {
                bool haySigRegisto = enumeratorHx.MoveNext();
                while (haySigRegisto)
                {
                    hxInicio = enumeratorHx.Current;
                    hxFin = enumeratorHx.Current + cantConsecutivo;
                    while ((haySigRegisto = enumeratorHx.MoveNext()) && enumeratorHx.Current == hxFin)
                    {
                        hxFin = enumeratorHx.Current + cantConsecutivo;
                    }
                    yield return new Tuple<int, int>(hxInicio, hxFin);
                }
            }
        }

        /// <summary>
        /// Permite obtener las horas que tienen congestion
        /// </summary>
        /// <param name="congestion"></param>
        /// <returns></returns>
        private List<int> ObtenerHorasConCongestion(CpMedicion48DTO congestion)
        {
            List<int> lstHxCongestion = new List<int>();
            for (int hx = 1; hx <= 48; hx++)
            {
                var valorHx = (decimal?)congestion.GetType().GetProperty($"H{hx}").GetValue(congestion, null);
                if (!valorHx.HasValue) continue;
                if (valorHx.Value == 0) continue;
                lstHxCongestion.Add(hx);
            }
            return lstHxCongestion;
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CP_TOPOLOGIA final por fecha
        /// </summary>
        public CpTopologiaDTO ObtenerTopologiaFinalPorFecha(DateTime topfecha, string toptipo)
        {
            return FactorySic.GetCpTopologiaRepository().GetByFechaTopfinal(topfecha, toptipo);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CP_PROPRECURSO
        /// </summary>
        public List<CpRecursoDTO> ObtenerCpRecursoPorTopologiaYCategoria(int topcodi, string propcodi = ConstantesAppServicio.ParametroDefecto)
        {
            return FactorySic.GetCpRecursoRepository().ObtenerPorTopologiaYCategoria(topcodi, propcodi);
        }

        private List<CpRecursoDTO> ObtenerVolMinMaxDeEmbalseCentralPorTopologiaFinal(CpTopologiaDTO topologiaFinal)
        {
            var lstRecursoPropiedades = new List<CpRecursoDTO>();
            if (topologiaFinal != default(CpTopologiaDTO))
            {
                lstRecursoPropiedades = ObtenerCpRecursoPorTopologiaYCategoria(topologiaFinal.Topcodi, $"{ConstantesCortoPlazo.PropcodiVolumenMinimo},{ConstantesCortoPlazo.PropcodiVolumenMaximo}");
            }

            return lstRecursoPropiedades;
        }

        /// <summary>
        /// Obtener listado cp_medicion48 por topologia, srestcodi y reprograma
        /// </summary>
        /// <param name="topologiaFinal"></param>
        /// <param name="fecha"></param>
        /// <param name="srestcodi"></param>
        /// <param name="conReprograma"></param>
        /// <returns></returns>
        public List<CpMedicion48DTO> ObtenerCpMedicion48(CpTopologiaDTO topologiaFinal, DateTime fecha, string srestcodi, bool conReprograma = false)
        {
            var lstDataResult = new List<CpMedicion48DTO>();
            if (topologiaFinal != null)
            {
                var lstData = GetByCriteriaCpMedicion48s(topologiaFinal.Topcodi.ToString(), fecha, srestcodi);

                if (conReprograma)
                {
                    List<CpReprogramaDTO> listaReprogramados = FactorySic.GetCpReprogramaRepository().GetByCriteria(topologiaFinal.Topcodi);
                    if (listaReprogramados.Any())
                    {
                        var lstTopcodiReprog = listaReprogramados.Select(x => x.Topcodi2);
                        var lstDataReprog = GetByCriteriaCpMedicion48s(string.Join(",", lstTopcodiReprog), fecha, srestcodi);

                        List<CpTopologiaDTO> lstTopologiaHorasInicio = ObtenerTopologiaHorasInicio(topologiaFinal, listaReprogramados);

                        lstDataResult = ObtenerCpMedicion48ConReprograma(lstTopologiaHorasInicio, lstData, lstDataReprog);
                    }
                }
                else
                {
                    lstDataResult = lstData;
                }
            }
            return lstDataResult;
        }

        /// <summary>
        /// Permite obtener lista de las horas inicio de topologia
        /// </summary>
        /// <param name="topologiaFinal"></param>
        /// <param name="listaReprogramados"></param>
        /// <returns></returns>
        private List<CpTopologiaDTO> ObtenerTopologiaHorasInicio(CpTopologiaDTO topologiaFinal, List<CpReprogramaDTO> listaReprogramados)
        {
            topologiaFinal.Toppadre = true;

            List<CpTopologiaDTO> lstTopologiaHorasInicio = new List<CpTopologiaDTO>() { topologiaFinal };

            if (listaReprogramados.Any())
            {
                var lstHorasInicioReprog = listaReprogramados.OrderBy(x => x.Reprogorden).Select(x => new CpTopologiaDTO { Topcodi = x.Topcodi2.Value, Topiniciohora = x.Topiniciohora }).ToList();

                int index = 0;
                foreach (var horasInicioReprog in lstHorasInicioReprog)
                {
                    lstTopologiaHorasInicio[index].Topfinhora = horasInicioReprog.Topiniciohora - 1;
                    index++;

                    if (lstHorasInicioReprog.Count == index) horasInicioReprog.Topfinhora = 48;

                    lstTopologiaHorasInicio.Add(horasInicioReprog);
                }
            }
            else
            {
                topologiaFinal.Topfinhora = 48;
            }

            return lstTopologiaHorasInicio;
        }

        /// <summary>
        /// Permite actualizar lista de cp_medicion48 con informacion del reprogramado
        /// </summary>
        /// <param name="lstTopologiasHorasInicio"></param>
        /// <param name="lstCpMedicion48"></param>
        /// <param name="lstCpMedicion48Reprog"></param>
        /// <returns></returns>
        private List<CpMedicion48DTO> ObtenerCpMedicion48ConReprograma(List<CpTopologiaDTO> lstTopologiasHorasInicio, List<CpMedicion48DTO> lstCpMedicion48, List<CpMedicion48DTO> lstCpMedicion48Reprog)
        {
            var lstTopologHoraInicioReprog = lstTopologiasHorasInicio.Where(x => x.Toppadre = false);

            foreach (var hTop in lstTopologHoraInicioReprog)
            {
                var lstMed48Topolog = lstCpMedicion48Reprog.Where(x => x.Topcodi == hTop.Topcodi).ToList();
                if (!lstMed48Topolog.Any()) continue;

                foreach (var medicion48 in lstCpMedicion48)
                {
                    var med48 = lstMed48Topolog.Find(x => x.Recurcodi == medicion48.Recurcodi && x.Srestcodi == medicion48.Srestcodi);
                    if (med48 == null) continue;

                    for (int index = hTop.Topiniciohora; index <= hTop.Topfinhora; index++)
                    {
                        var valueHx = (decimal?)med48.GetType().GetProperty($"H{index}").GetValue(med48, null);
                        med48.GetType().GetProperty($"H{index}").SetValue(med48, valueHx);
                    }
                }
            }

            return lstCpMedicion48;
        }

        private List<CpRecursoDTO> ObtenerListaRelacionBarraCentral(CpTopologiaDTO topologiaFinal)
        {
            var lstBarraCentral = new List<CpRecursoDTO>();
            if (topologiaFinal != default(CpTopologiaDTO))
            {
                lstBarraCentral = FactorySic.GetCpRecursoRepository().ObtenerListaRelacionBarraCentral(topologiaFinal.Topcodi);
            }

            return lstBarraCentral;
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CpMedicion48
        /// </summary>
        public List<CpMedicion48DTO> GetByCriteriaCpMedicion48s(string topcodi, DateTime medifecha, string srestcodi)
        {
            return FactorySic.GetCpMedicion48Repository().GetByCriteria(topcodi, medifecha, srestcodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="catcodi"></param>
        /// <param name="Recurcodisicoes"></param>
        /// <returns></returns>
        public CpRecursoDTO GetByCriteriaRecurso(int topcodi, int catcodi, int Recurcodisicoes)
        {
            return FactorySic.GetCpRecursoRepository().GetByCriteria(topcodi, catcodi, Recurcodisicoes);
        }


        #endregion


        #region Regiones_seguridad

        #region Métodos Tabla CM_REGIONSEGURIDAD

        /// <summary>
        /// Inserta un registro de la tabla CM_REGIONSEGURIDAD
        /// </summary>
        public void SaveCmRegionseguridad(CmRegionseguridadDTO entity)
        {
            try
            {
                if (entity.Regsegcodi == 0)
                {
                    FactorySic.GetCmRegionseguridadRepository().Save(entity);
                }
                else
                {
                    FactorySic.GetCmRegionseguridadRepository().Update(entity);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CM_REGIONSEGURIDAD
        /// </summary>
        public void UpdateCmRegionseguridad(CmRegionseguridadDTO entity)
        {
            try
            {
                FactorySic.GetCmRegionseguridadRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CM_REGIONSEGURIDAD
        /// </summary>
        public void DeleteCmRegionseguridad(int regsegcodi)
        {
            try
            {
                FactorySic.GetCmRegionseguridadRepository().Delete(regsegcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CM_REGIONSEGURIDAD
        /// </summary>
        public CmRegionseguridadDTO GetByIdCmRegionseguridad(int regsegcodi)
        {
            return FactorySic.GetCmRegionseguridadRepository().GetById(regsegcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CM_REGIONSEGURIDAD
        /// </summary>
        public List<CmRegionseguridadDTO> ListCmRegionseguridads()
        {
            return FactorySic.GetCmRegionseguridadRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CmRegionseguridad
        /// </summary>
        public List<CmRegionseguridadDTO> GetByCriteriaCmRegionseguridads()
        {
            return FactorySic.GetCmRegionseguridadRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla CM_REGIONSEGURIDAD_DETALLE

        /// <summary>
        /// Inserta un registro de la tabla CM_REGIONSEGURIDAD_DETALLE
        /// </summary>
        public int SaveCmRegionseguridadDetalle(CmRegionseguridadDetalleDTO entity)
        {
            try
            {
                int resultado = 1;

                List<CmRegionseguridadDetalleDTO> list = FactorySic.GetCmRegionseguridadDetalleRepository().List(entity.Regsegcodi, entity.Equicodi);

                if (list.Count == 0)
                {
                    FactorySic.GetCmRegionseguridadDetalleRepository().Save(entity);
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
        /// Actualiza un registro de la tabla CM_REGIONSEGURIDAD_DETALLE
        /// </summary>
        public void UpdateCmRegionseguridadDetalle(CmRegionseguridadDetalleDTO entity)
        {
            try
            {
                FactorySic.GetCmRegionseguridadDetalleRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CM_REGIONSEGURIDAD_DETALLE
        /// </summary>
        public void DeleteCmRegionseguridadDetalle(int idRegion, int idEquipo)
        {
            try
            {
                FactorySic.GetCmRegionseguridadDetalleRepository().Delete(idRegion, idEquipo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CM_REGIONSEGURIDAD_DETALLE
        /// </summary>
        public CmRegionseguridadDetalleDTO GetByIdCmRegionseguridadDetalle(int regdetcodi)
        {
            return FactorySic.GetCmRegionseguridadDetalleRepository().GetById(regdetcodi);
        }



        /// <summary>
        /// Permite realizar búsquedas en la tabla CmRegionseguridadDetalle
        /// </summary>
        public List<CmRegionseguridadDetalleDTO> GetByCriteriaCmRegionseguridadDetalles(int idRegion)
        {
            return FactorySic.GetCmRegionseguridadDetalleRepository().GetByCriteria(idRegion);
        }

        /// <summary>
        /// Permite obtener los equipos
        /// </summary>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        public List<CmRegionseguridadDetalleDTO> ObtenerEquiposConjunto(int tipo)
        {
            return FactorySic.GetCmRegionseguridadDetalleRepository().ObtenerEquipos(tipo);
        }

        #endregion

        #endregion

        #region Métodos tabla CM_UMBRAL_COMPARACION

        public CmUmbralComparacionDTO OtenerUmbralConfiguracion(string username)
        {
            CmUmbralComparacionDTO entity = FactorySic.GetCmUmbralComparacionRepository().GetById(ConstantesCortoPlazo.IdConfiguracionUmbral);

            if (entity == null)
            {
                CmUmbralComparacionDTO dto = new CmUmbralComparacionDTO
                {
                    Cmumcocodi = ConstantesCortoPlazo.IdConfiguracionUmbral,
                    Cmumcofeccreacion = DateTime.Now,
                    Cmumcousucreacion = username
                };

                FactorySic.GetCmUmbralComparacionRepository().Save(dto);

                entity = dto;
            }

            return entity;

        }

        public void GrabarUmbralComparacion(CmUmbralComparacionDTO entity)
        {
            CmUmbralComparacionDTO dto = FactorySic.GetCmUmbralComparacionRepository().GetById(ConstantesCortoPlazo.IdConfiguracionUmbral);

            if (dto != null)
            {
                FactorySic.GetCmUmbralComparacionRepository().Update(entity);
            }
            else
            {
                CmUmbralComparacionDTO newEntity = new CmUmbralComparacionDTO
                {
                    Cmumcocodi = ConstantesCortoPlazo.IdConfiguracionUmbral,
                    Cmumcofeccreacion = DateTime.Now,
                    Cmumcousucreacion = entity.Cmuncousumodificacion
                };

                FactorySic.GetCmUmbralComparacionRepository().Save(newEntity);
            }
        }

        /// <summary>
        /// Permite obtener el único registro de la tabla CM_UMBRAL_COMPARACION
        /// </summary>
        public CmUmbralComparacionDTO GetByIdUmbralComparacion()
        {
            return FactorySic.GetCmUmbralComparacionRepository().GetById(ConstantesCortoPlazo.IdConfiguracionUmbral);
        }


        #endregion

        #region Mejoras CMgN 


        #region Métodos Tabla CM_CONGESTION_CALCULO

        /// <summary>
        /// Inserta un registro de la tabla CM_CONGESTION_CALCULO
        /// </summary>
        public void SaveCmCongestionCalculo(CmCongestionCalculoDTO entity)
        {
            try
            {
                FactorySic.GetCmCongestionCalculoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CM_CONGESTION_CALCULO
        /// </summary>
        public void UpdateCmCongestionCalculo(CmCongestionCalculoDTO entity)
        {
            try
            {
                FactorySic.GetCmCongestionCalculoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CM_CONGESTION_CALCULO
        /// </summary>
        public CmCongestionCalculoDTO GetByIdCmCongestionCalculo(int cmcongcodi)
        {
            return FactorySic.GetCmCongestionCalculoRepository().GetById(cmcongcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CM_CONGESTION_CALCULO
        /// </summary>
        public List<CmCongestionCalculoDTO> ListCmCongestionCalculos()
        {
            return FactorySic.GetCmCongestionCalculoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CmCongestionCalculo
        /// </summary>
        public List<CmCongestionCalculoDTO> GetByCriteriaCmCongestionCalculos()
        {
            return FactorySic.GetCmCongestionCalculoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla CM_COSTO_INCREMENTAL

        /// <summary>
        /// Inserta un registro de la tabla CM_COSTO_INCREMENTAL
        /// </summary>
        public void SaveCmCostoIncremental(CmCostoIncrementalDTO entity)
        {
            try
            {
                FactorySic.GetCmCostoIncrementalRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CM_COSTO_INCREMENTAL
        /// </summary>
        public void UpdateCmCostoIncremental(CmCostoIncrementalDTO entity)
        {
            try
            {
                FactorySic.GetCmCostoIncrementalRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite obtener un registro de la tabla CM_COSTO_INCREMENTAL
        /// </summary>
        public CmCostoIncrementalDTO GetByIdCmCostoIncremental(int cmcicodi)
        {
            return FactorySic.GetCmCostoIncrementalRepository().GetById(cmcicodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CM_COSTO_INCREMENTAL
        /// </summary>
        public List<CmCostoIncrementalDTO> ListCmCostoIncrementals()
        {
            return FactorySic.GetCmCostoIncrementalRepository().List();
        }



        #endregion

        #endregion

        #region CMgCP_PR07

        #region Métodos Tabla CM_VOLUMEN_INSENSIBILIDAD

        /// <summary>
        /// Inserta un registro de la tabla CM_VOLUMEN_INSENSIBILIDAD
        /// </summary>
        public int SaveCmVolumenInsensibilidad(string[][] data, DateTime fecha, string usuario)
        {
            try
            {
                foreach (string[] row in data)
                {
                    if (!string.IsNullOrEmpty(row[1]))
                    {

                        int id = 0;
                        if (!string.IsNullOrEmpty(row[0])) id = Convert.ToInt32(row[0]);

                        CmVolumenInsensibilidadDTO entity = new CmVolumenInsensibilidadDTO();
                        entity.Volinscodi = id;
                        entity.Recurcodi = Convert.ToInt32(row[1]);
                        entity.Volinsfecha = fecha;
                        entity.Topcodi = ConstantesCortoPlazo.Topcodi;
                        entity.Volinsvolmax = Convert.ToDecimal(row[2]);
                        entity.Volinsvolmin = Convert.ToDecimal(row[3]);
                        entity.Volinsinicio = DateTime.ParseExact(fecha.ToString(ConstantesAppServicio.FormatoFecha) + " " + row[4], ConstantesAppServicio.FormatoFechaHora, CultureInfo.InvariantCulture);
                        entity.Volinsfin = (row[5] != "00:00") ?
                            DateTime.ParseExact(fecha.ToString(ConstantesAppServicio.FormatoFecha) + " " + row[5], ConstantesAppServicio.FormatoFechaHora, CultureInfo.InvariantCulture) :
                            fecha.AddDays(1);
                        entity.Volinsfecmodificacion = DateTime.Now;
                        entity.Volinsusumodificacion = usuario;

                        if (id == 0)
                        {
                            entity.Volinsfecreacion = DateTime.Now;
                            entity.Volinsusucreacion = usuario;
                            FactorySic.GetCmVolumenInsensibilidadRepository().Save(entity);
                        }
                        else
                        {
                            FactorySic.GetCmVolumenInsensibilidadRepository().Update(entity);
                        }
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CM_VOLUMEN_INSENSIBILIDAD
        /// </summary>
        public void UpdateCmVolumenInsensibilidad(CmVolumenInsensibilidadDTO entity)
        {
            try
            {
                FactorySic.GetCmVolumenInsensibilidadRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CM_VOLUMEN_INSENSIBILIDAD
        /// </summary>
        public void DeleteCmVolumenInsensibilidad(int volinscodi)
        {
            try
            {
                FactorySic.GetCmVolumenInsensibilidadRepository().Delete(volinscodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CM_VOLUMEN_INSENSIBILIDAD
        /// </summary>
        public CmVolumenInsensibilidadDTO GetByIdCmVolumenInsensibilidad(int volinscodi)
        {
            return FactorySic.GetCmVolumenInsensibilidadRepository().GetById(volinscodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CM_VOLUMEN_INSENSIBILIDAD
        /// </summary>
        public List<CmVolumenInsensibilidadDTO> ListCmVolumenInsensibilidads()
        {
            return FactorySic.GetCmVolumenInsensibilidadRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CmVolumenInsensibilidad
        /// </summary>
        public List<CmVolumenInsensibilidadDTO> GetByCriteriaCmVolumenInsensibilidads(DateTime fecha)
        {
            return FactorySic.GetCmVolumenInsensibilidadRepository().GetByCriteria(fecha);
        }

        /// <summary>
        /// Permite obtener los embalses de YUPANA
        /// </summary>
        /// <returns></returns>
        public List<CpRecursoDTO> ObtenerEmbalsesYUPANA()
        {
            return FactorySic.GetCpRecursoRepository().ObtenerEmbalsesYupana();
        }

        #endregion

        #endregion

        #region Ticket 2022-004245


        /// <summary>
        /// Permite obtener los datos de la configuración
        /// </summary>
        public void ObtenerDatosEquipoNoModelado(int relacionCodi, out List<CmGeneradorPotenciagenDTO> entitysPotencia,
            out List<CmGeneradorBarraemsDTO> entitysBarras)
        {
            EqRelacionDTO entity = FactorySic.GetEqRelacionRepository().GetById(relacionCodi);

            //- Obtención de los datos de generación
            List<EqRelacionDTO> listEquipos = FactorySic.GetEqRelacionRepository().ObtenerConfiguracionProceso(ConstantesCortoPlazo.FuenteGeneracion);
            EqRelacionDTO modos = listEquipos.Where(x => x.Relacioncodi == entity.Relacioncodi).FirstOrDefault();
            List<CmGeneradorPotenciagenDTO> listaPotencia = FactorySic.GetCmGeneradorPotenciagenRepository().GetByCriteria(relacionCodi);
            List<CmGeneradorPotenciagenDTO> resultGenerador = new List<CmGeneradorPotenciagenDTO>();
            if (!string.IsNullOrEmpty(modos.Modosoperacion))
            {
                List<int> idsModos = modos.Modosoperacion.Split(',').Select(int.Parse).ToList();

                foreach (int idModo in idsModos)
                {
                    CmGeneradorPotenciagenDTO itemGenerador = new CmGeneradorPotenciagenDTO();

                    PrGrupoDTO grupo = FactorySic.GetPrGrupoRepository().GetById(idModo);
                    CmGeneradorPotenciagenDTO generacion = listaPotencia.Where(x => x.Grupocodi == idModo).FirstOrDefault();
                    decimal? potencia = null;

                    if (generacion != null)
                    {
                        potencia = generacion.Genpotvalor;
                    }

                    itemGenerador.Grupocodi = idModo;
                    itemGenerador.Gruponomb = grupo.Gruponomb;
                    itemGenerador.Genpotvalor = potencia;
                    resultGenerador.Add(itemGenerador);
                }
            }

            //- Obtención de los datos de barras
            List<CmGeneradorBarraemsDTO> resultBarras = FactorySic.GetCmGeneradorBarraemsRepository().GetByCriteria(relacionCodi);

            entitysPotencia = resultGenerador;
            entitysBarras = resultBarras;
        }

        /// <summary>
        /// Permite almacenar los dtos de la configuración
        /// </summary>
        public int GrabarDatosEquipoNoModelado(int relacionCodi, string barras, string potencias, string username)
        {
            try
            {
                List<int> idBarras = barras.Split(',').Select(int.Parse).ToList();
                List<string> idPotencias = potencias.Split(',').ToList();

                FactorySic.GetCmGeneradorBarraemsRepository().Delete(relacionCodi);
                FactorySic.GetCmGeneradorPotenciagenRepository().Delete(relacionCodi);

                foreach (int idBarra in idBarras)
                {
                    CmGeneradorBarraemsDTO entity = new CmGeneradorBarraemsDTO();
                    entity.Relacioncodi = relacionCodi;
                    entity.Cnfbarcodi = idBarra;
                    entity.Genbarfeccreacion = DateTime.Now;
                    entity.Genbarfecmodificacion = DateTime.Now;
                    entity.Genbarusucreacion = username;
                    entity.Genbarusumodificacion = username;
                    FactorySic.GetCmGeneradorBarraemsRepository().Save(entity);
                }

                foreach (string potencia in idPotencias)
                {
                    string[] token = potencia.Split('#');
                    CmGeneradorPotenciagenDTO entity = new CmGeneradorPotenciagenDTO();
                    entity.Relacioncodi = relacionCodi;
                    entity.Grupocodi = int.Parse(token[0]);
                    entity.Genpotvalor = decimal.Parse(token[1]);
                    entity.Genpotfeccreacion = DateTime.Now;
                    entity.Genpotfecmodificacion = DateTime.Now;
                    entity.Genpotusucreacion = username;
                    entity.Genpotusumodificacion = username;


                    FactorySic.GetCmGeneradorPotenciagenRepository().Save(entity);

                }
                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }


        }

        /// <summary>
        /// Permite obtener las barras EMS
        /// </summary>
        /// <returns></returns>
        public List<CmConfigbarraDTO> ObtenerBarrasEMS()
        {
            return FactorySic.GetCmConfigbarraRepository().GetByCriteria((-1).ToString(), (-1).ToString()).ToList();
        }


        #endregion

        #region Métodos Tabla EQ_RELACION_TNA

        /// <summary>
        /// Inserta un registro de la tabla EQ_RELACION_TNA
        /// </summary>
        public void SaveEqRelacionTna(int idRelacion, string parts, string username)
        {
            try
            {
                FactorySic.GetEqRelacionTnaRepository().Delete(idRelacion);

                List<string> items = parts.Split('@').ToList();

                foreach(string item in items)
                {
                    EqRelacionTnaDTO entity = new EqRelacionTnaDTO();
                    entity.Relacioncodi = idRelacion;
                    entity.Reltnaestado = ConstantesAppServicio.Activo;
                    entity.Reltnanombre = item.Trim();
                    entity.Reltnausucreacion = username;
                    entity.Reltnafeccreacion = DateTime.Now;

                    FactorySic.GetEqRelacionTnaRepository().Save(entity);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EqRelacionTna
        /// </summary>
        public List<EqRelacionTnaDTO> ObtenerEquiposTNAAdicionales(int idRelacion)
        {
            return FactorySic.GetEqRelacionTnaRepository().GetByCriteria(idRelacion);
        }

        #endregion
    }
}

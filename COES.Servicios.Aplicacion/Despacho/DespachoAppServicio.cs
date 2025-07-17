using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using COES.Base.Tools;
using COES.Servicios.Aplicacion.Despacho.Helper;
using System.Globalization;
using System.IO;
using OfficeOpenXml;
using COES.Framework.Base.Tools;
using System.Text;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.Equipamiento.Helper;

namespace COES.Servicios.Aplicacion.Equipamiento
{
    /// <summary>
    /// Clases con métodos del módulo Despacho
    /// </summary>
    public class DespachoAppServicio : AppServicioBase
    {
        FichaTecnicaAppServicio servFictec = new FichaTecnicaAppServicio();

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DespachoAppServicio));

        #region Métodos Tabla PR_GRUPO

        /// <summary>
        /// Inserta un registro de la tabla PR_GRUPO
        /// </summary>
        public int SavePrGrupo(PrGrupoDTO entity)
        {
            int id = -1;
            try
            {
                id = FactorySic.GetPrGrupoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        public List<ActualizacionCVDTO> ObtenerActualizacionesCostos(DateTime dtFechaInicio, DateTime dtFechaFin)
        {
            return FactorySic.GetPrGrupoRepository().ObtenerActualizacionesCostos(dtFechaInicio, dtFechaFin);
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

        public List<CostoVariableDTO> ObtenerCostosVariablesPorActualizacion(int iCodActualizacion)
        {
            return FactorySic.GetPrGrupoRepository().ObtenerCostosVariablesPorActualizacion(iCodActualizacion);
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
            return FactorySic.GetPrGrupoRepository().GetById(grupocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_GRUPO
        /// </summary>
        public List<PrGrupoDTO> ListPrGrupos()
        {
            return FactorySic.GetPrGrupoRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_GRUPO segun el codigo de empresa y categoría
        /// </summary>
        public List<PrGrupoDTO> ListPrGruposByEmprcodiAndCatecodi(int emprcodi, int catecodi)
        {
            return FactorySic.GetPrGrupoRepository().ListByEmprCodiAndCatecodi(emprcodi, catecodi);
        }

        public List<PrGrupoDTO> ModosOperacionxFiltro(int idEmpresa, string sEstado, string sNombreModo, int nroPagina, int nroFilas)
        {
            return FactorySic.GetPrGrupoRepository().ModosOperacionxFiltro(idEmpresa, sEstado, sNombreModo, nroPagina, nroFilas);
        }

        public int CantidadModosOperacionxFiltro(int idEmpresa, string sEstado, string sNombreModo)
        {
            return FactorySic.GetPrGrupoRepository().CantidadModosOperacionxFiltro(idEmpresa, sEstado, sNombreModo);
        }

        public PrGrupoDTO ObtenerModoOperacion(int iGrupoCodi)
        {
            return FactorySic.GetPrGrupoRepository().ObtenerModoOperacion(iGrupoCodi);
        }

        /// <summary>
        /// Lista los valores y datos de los conceptos pertenecientes a los grupos funcionales
        /// </summary>
        /// <param name="iGrupocodi"></param>
        /// <returns></returns>
        public List<PrGrupoConceptoDato> ListarDatosVigentesPorModoOperacion(int iGrupocodi, bool bFichaTecnica, DateTime dFechaRepCV)
        {
            int iCodPadre = FactorySic.GetPrGrupoRepository().ObtenerCodigoModoOperacionPadre(iGrupocodi);
            int iCodPadrePadre = FactorySic.GetPrGrupoRepository().ObtenerCodigoModoOperacionPadre(iCodPadre);
            string sCodigos = iGrupocodi.ToString();
            if (iCodPadre > -1)
                sCodigos = sCodigos + "," + iCodPadre;
            if (iCodPadrePadre > -1)
                sCodigos = sCodigos + "," + iCodPadrePadre;
            DateTime dfFechaLimite = dFechaRepCV;//DateTime.Now.AddDays(1);

            return FactorySic.GetPrGrupoRepository().ListarDatosVigentesPorModoOperacion(sCodigos, dfFechaLimite, bFichaTecnica);
        }

        #endregion

        /// <summary>
        /// Listar grupos de despacho válidos
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoDTO> ListarPrGrupoDespacho()
        {
            List<int> lCatecodi = new List<int>() { 3, 5, 15, 17 };
            List<string> lEstado = new List<string>() { "A", "F", "P" };

            return FactorySic.GetPrGrupoRepository().ListarGruposPorCategoria(lCatecodi).Where(x => lEstado.Contains(x.GrupoEstado))
                            .OrderBy(x => x.Gruponomb).ToList();
        }

        /// <summary>
        /// Listado de Grupo de generación de despacho para osinergmin
        /// </summary>
        public List<GrupoGeneracionDTO> ListarGeneradoresDespachoOsinergmin()
        {
            return FactorySic.GetPrGrupoRepository().ListarGeneradoresDespachoOsinergmin();
        }

        public List<PrGrupoDTO> ListaModosOperacionActivos()
        {
            return FactorySic.GetPrGrupoRepository().ListaModosOperacionActivos();
        }

        /// <summary>
        /// Obtener los grupos según tipo de categoria
        /// </summary>
        /// <param name="tipoGrupoCodi"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> ObtenerMantenimientoGrupoRER(int tipoGrupoCodi)
        {
            return FactorySic.GetPrGrupoRepository().GetByCriteria(tipoGrupoCodi);
        }

        /// <summary>
        /// Permite actualizar el tipo del grupo seleccionados
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <param name="userName"></param>
        public void CambiarTipoGrupo(int idGrupo, int idTipoGrupo, int idTipoGrupo2, string tipoGenerRer, string userName)
        {
            try
            {
                FactorySic.GetPrGrupoRepository().CambiarTipoGrupo(idGrupo, idTipoGrupo, idTipoGrupo2, tipoGenerRer, userName, DateTime.Now);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta un registro de la tabla PR_TIPOGRUPO
        /// </summary>
        public void SavePrTipogrupo(PrTipogrupoDTO entity)
        {
            try
            {
                FactorySic.GetPrTipogrupoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_TIPOGRUPO
        /// </summary>
        public void UpdatePrTipogrupo(PrTipogrupoDTO entity)
        {
            try
            {
                FactorySic.GetPrTipogrupoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PR_TIPOGRUPO
        /// </summary>
        public void DeletePrTipogrupo(int tipogrupocodi)
        {
            try
            {
                FactorySic.GetPrTipogrupoRepository().Delete(tipogrupocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_TIPOGRUPO
        /// </summary>
        public PrTipogrupoDTO GetByIdPrTipogrupo(int tipogrupocodi)
        {
            return FactorySic.GetPrTipogrupoRepository().GetById(tipogrupocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_TIPOGRUPO
        /// </summary>
        public List<PrTipogrupoDTO> ListPrTipogrupos()
        {
            return FactorySic.GetPrTipogrupoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrTipogrupo
        /// </summary>
        public List<PrTipogrupoDTO> GetByCriteriaPrTipogrupos()
        {
            return FactorySic.GetPrTipogrupoRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite listar las categorias de grupos
        /// </summary>
        /// <returns></returns>
        public List<PrCategoriaDTO> ListarTipoGrupo()
        {
            return FactorySic.GetPrCategoriaRepository().List();
        }

        public List<ModoOperacionDTO> ListadoModosOperacionPorCentral(int iCentral)
        {
            try
            {
                var resultado = FactorySic.GetPrGrupoRepository().ModoOperacionCentral1(iCentral);
                var resultadoNuevo = FactorySic.GetPrGrupoRepository().ModoOperacionCentral2(iCentral);
                var propOperacionComercial = FactorySic.GetPrGrupodatRepository().GetByCriteria(611);

                int NumNuevos = 0;
                int NumAntiguos = resultado.Count();

                var ResultadoFinal = new List<ModoOperacionDTO>();
                var ResultadoFinal2 = new List<ModoOperacionDTO>();
                if (resultadoNuevo != null)
                {
                    NumNuevos = resultadoNuevo.Count();
                }

                if (NumAntiguos > NumNuevos)
                {
                    ResultadoFinal = resultado.ToList();
                    //Logica para fusionar la lista de modos Originales con los Nuevos, en caso existan datos en los Modos Nuevos
                    if (NumNuevos > 0)
                    {
                        var NuevosGrupos = resultadoNuevo.ToList();
                        foreach (var NewGroup in NuevosGrupos)
                        {
                            var indiceBusqueda = ResultadoFinal.FindIndex(go => go.GRUPOCODI == NewGroup.GRUPOCODI);
                            if (indiceBusqueda > -1)
                            {
                                ResultadoFinal[indiceBusqueda].EQUICODI = NewGroup.EQUICODI;
                            }
                        }
                    }
                }
                if (NumAntiguos < NumNuevos)
                {
                    ResultadoFinal = resultadoNuevo.ToList();
                }
                if (NumNuevos == NumAntiguos)
                {
                    ResultadoFinal = resultadoNuevo.ToList();
                }

                //var lsResultadoReturn = new List<ModoOperacionDTO>();
                foreach (var grupo in ResultadoFinal)
                {
                    var Modo = FactorySic.GetPrGrupoRepository().GetById(Convert.ToInt32(grupo.GRUPOCODI));//  _prEquipoDatRepository.Context.PR_GRUPO.Single(grup => grup.GRUPOCODI == grupo.GRUPOCODI);
                    grupo.IDCENTRAL = iCentral.ToString();
                    if (grupo.EQUICODI > -1)
                    {
                        var equipo = FactorySic.GetEqEquipoRepository().GetById(Convert.ToInt32(grupo.EQUICODI));// _prEquipoDatRepository.Context.EQ_EQUIPO.Single(eq => eq.EQUICODI == grupo.EQUICODI);
                        string EquipoComb = "";
                        if (equipo.Equiabrev.Trim() == "TV")
                        {
                            EquipoComb = "CC";
                        }
                        else
                        {
                            EquipoComb = equipo.Equiabrev.Trim();
                        }
                        EquipoComb = EquipoComb + " " + Modo.Grupocomb.Trim();
                        grupo.GRUPONOM = EquipoComb;
                        grupo.MODONOM = Modo.Gruponomb.Trim();
                        //ResultadoFinal.IndexOf(grupo);
                    }
                    else
                    {
                        grupo.GRUPONOM = Modo.Grupoabrev.Trim();
                    }


                }

                foreach (var modo in ResultadoFinal)
                {
                    var ResultadoGrupoDat = propOperacionComercial.Where(x => x.Grupocodi == modo.GRUPOCODI && x.Deleted == 0 && x.Fechadat <= DateTime.Now).OrderByDescending(x => x.Fechadat).ToList();
                    if (ResultadoGrupoDat.Count() > 0 && ResultadoGrupoDat[0].Formuladat == "S")
                    {
                        ResultadoFinal2.Add(modo);
                    }
                }

                return ResultadoFinal2;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public int ObtenerCodigoModoOperacionPadre(int iPrGrupo)
        {
            try
            {
                return FactorySic.GetPrGrupoRepository().ObtenerCodigoModoOperacionPadre(iPrGrupo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<ConceptoDatoDTO> ListadoValoresModoOperacion(int iEquiCodi, int iGrupoCodi)
        {
            try
            {
                IEnumerable<ConceptoDatoDTO> ResultadoGrupoDat;
                IEnumerable<ConceptoDatoDTO> ResultadoEquipoDat = null; ;
                var ConceptoCentral = new ConceptoDatoDTO();
                ConceptoCentral.CONCEPCODI = -99;
                if (iEquiCodi > -1)
                {
                    ResultadoGrupoDat = FactorySic.GetPrGrupodatRepository().ListarDatosConceptoGrupoDat(iGrupoCodi);// _prEquipoDatRepository.Context.Database.SqlQuery<ConceptoDatoDTO>("select distinct gd.concepcodi,fn_sdatoactualconcepto(gd.grupocodi,gd.concepcodi) valor ,c.concepunid from pr_grupodat gd inner join pr_concepto c on gd.concepcodi=c.concepcodi where gd.grupocodi=:p0", iGrupoCodi);

                    ResultadoEquipoDat = FactorySic.GetPrEquipodatRepository().ListarDatosConceptoEquipoDat(iEquiCodi, iGrupoCodi);
                    // _prEquipoDatRepository.Context.Database.SqlQuery<ConceptoDatoDTO>("select distinct ed.concepcodi,FN_SDATOACTUALEQUIPODAT(ed.grupocodi,ed.concepcodi,ed.equicodi) valor,c.concepunid from pr_equipodat ed inner join pr_concepto c on c.concepcodi=ed.concepcodi where ed.grupocodi=:p0 and ed.equicodi=:p1", iGrupoCodi, iEquiCodi);
                }
                else
                {
                    ResultadoGrupoDat = FactorySic.GetPrGrupodatRepository().ListarDatosConceptoGrupoDat(iGrupoCodi);
                    //ResultadoGrupoDat = _prEquipoDatRepository.Context.Database.SqlQuery<ConceptoDatoDTO>("select distinct gd.concepcodi,fn_sdatoactualconcepto(gd.grupocodi,gd.concepcodi) valor ,c.concepunid from pr_grupodat gd inner join pr_concepto c on gd.concepcodi=c.concepcodi where gd.grupocodi=:p0", iGrupoCodi);
                }

                var ResultadoFinal = ResultadoGrupoDat.ToList();
                if (iEquiCodi > -1)
                {
                    var ResultadoEquipoDato = ResultadoEquipoDat.ToList();
                    foreach (var ConceptoDatoDTO in ResultadoEquipoDato)
                    {
                        var Existe = ResultadoFinal.FindIndex(concep => concep.CONCEPCODI == ConceptoDatoDTO.CONCEPCODI);
                        if (Existe == -1)
                        {
                            ResultadoFinal.Add(ConceptoDatoDTO);
                        }
                        else
                        {
                            ResultadoFinal[Existe].VALOR = ConceptoDatoDTO.VALOR;
                        }
                    }
                    var equipo = FactorySic.GetEqEquipoRepository().GetById(iEquiCodi);// _prEquipoDatRepository.Context.EQ_EQUIPO.Single(eq => eq.EQUICODI == iEquiCodi);
                    var Modo = FactorySic.GetPrGrupoRepository().GetById(iGrupoCodi); //_prEquipoDatRepository.Context.PR_GRUPO.Single(grup => grup.GRUPOCODI == iGrupoCodi);

                    ConceptoCentral.VALOR = (equipo.Equiabrev.Trim() + " " + Modo.Grupocomb).Trim();

                }
                else
                {
                    //var Modo = _prEquipoDatRepository.Context.PR_GRUPO.Single(grup => grup.GRUPOCODI == iGrupoCodi);
                    var Modo = FactorySic.GetPrGrupoRepository().GetById(iGrupoCodi);
                    ConceptoCentral.VALOR = Modo.Gruponomb;
                }
                ResultadoFinal.Add(ConceptoCentral);
                return ResultadoFinal;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #region Métodos Tabla PR_CONCEPTO

        /// <summary>
        /// Inserta un registro de la tabla PR_CONCEPTO
        /// </summary>
        public int SavePrConcepto(PrConceptoDTO entity)
        {
            int codigoConcepto = 0;
            var UoW = FactorySic.UnitOfWork();
            using (var connection = UoW.BeginConnection())
            {
                using (var transaction = UoW.StartTransaction(connection))
                {
                    try
                    {
                        //guadar Conceppto
                        codigoConcepto = FactorySic.GetPrConceptoRepository().Save(entity, connection, transaction);

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

            return codigoConcepto;
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_CONCEPTO
        /// </summary>
        public void UpdatePrConcepto(PrConceptoDTO entity)
        {
            var UoW = FactorySic.UnitOfWork();
            using (var connection = UoW.BeginConnection())
            {
                using (var transaction = UoW.StartTransaction(connection))
                {
                    try
                    {
                        //actualizar Concepto
                        FactorySic.GetPrConceptoRepository().Update(entity, connection, transaction);

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
        /// Elimina un registro de la tabla PR_CONCEPTO
        /// </summary>
        public void DeletePrConcepto(int concepcodi)
        {
            try
            {
                FactorySic.GetPrConceptoRepository().Delete(concepcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_CONCEPTO
        /// </summary>
        public PrConceptoDTO GetByIdPrConcepto(int concepcodi)
        {
            var reg = FactorySic.GetPrConceptoRepository().GetById(concepcodi);

            if (reg != null) FormatearConceptos(reg, null);

            return reg;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_CONCEPTO
        /// </summary>
        public List<PrConceptoDTO> ListPrConceptos()
        {
            return FactorySic.GetPrConceptoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrConcepto
        /// </summary>
        public List<PrConceptoDTO> GetByCriteriaPrConceptos()
        {
            return FactorySic.GetPrConceptoRepository().GetByCriteria(ConstantesAppServicio.ParametroDefecto);
        }

        #endregion

        #region Métodos Tabla PR_GRUPODAT

        /// <summary>
        /// Método que devuelve una lista de unidades por modo de operacion para un determinado equipo y categoría
        /// </summary>
        /// <param name="equicodi">Código de equipo</param>
        /// <param name="catecodi">Código de categoría</param>
        /// <returns></returns>
        public List<PrGrupoDTO> ListarModoOperacionDeEquipo(int equicodi, int catecodi)
        {
            return FactorySic.GetPrGrupoRepository().ListarModoOperacionDeEquipo(equicodi, catecodi);
        }

        /// <summary>
        /// Inserta un registro de la tabla PR_GRUPODAT
        /// </summary>
        public void SavePrGrupodat(PrGrupodatDTO entity)
        {
            ActualizarGrupodat(false, entity);
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_GRUPODAT
        /// </summary>
        public void UpdatePrGrupodat(PrGrupodatDTO entity)
        {
            ActualizarGrupodat(false, entity);
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_GRUPODAT
        /// </summary>
        public void ActualizarGrupodat(bool flagEliminar, PrGrupodatDTO entity)
        {
            try
            {
                DateTime fechaAct = DateTime.Now;
                //formatear datos
                entity.Formuladat = (entity.Formuladat ?? "").Trim();
                entity.Gdatcheckcero = entity.Gdatcheckcero ?? 0;
                entity.Gdatcomentario = (entity.Gdatcomentario ?? "").Trim();
                entity.Gdatsustento = (entity.Gdatsustento ?? "").Trim();
                if (entity.Fechaact == null) entity.Fechaact = fechaAct;
                if (string.IsNullOrEmpty(entity.Lastuser)) entity.Lastuser = "SISTEMA";

                List<PrGrupodatDTO> listaDat = FactorySic.GetPrGrupodatRepository().ListarHistoricoValores(entity.Concepcodi.ToString(), entity.Grupocodi);

                // Listar histórico de la fecha de vigencia
                List<PrGrupodatDTO> listaHistXCnp = listaDat.Where(x => x.Fechadat == entity.Fechadat).ToList();
                //obtener activo
                PrGrupodatDTO regActivoHist = listaHistXCnp.Find(x => x.Deleted == 0);

                //Definir cuales son nuevos, updates (solo cambia check cero, comentario u observación) o eliminados (pasan a histórico)
                List<PrGrupodatDTO> listaNew = new List<PrGrupodatDTO>();
                List<PrGrupodatDTO> listaUpdate = new List<PrGrupodatDTO>();
                List<PrGrupodatDTO> listaDarDeBaja = new List<PrGrupodatDTO>();

                if (flagEliminar)//eliminar lógicamente
                {
                    if (regActivoHist != null)
                    {
                        regActivoHist.Fechaact = fechaAct;
                        regActivoHist.Lastuser = entity.Lastuser;
                        regActivoHist.Deleted = 0;
                        regActivoHist.Deleted2 = listaHistXCnp.Max(x => x.Deleted) + 1;
                        listaUpdate.Add(regActivoHist);
                    }
                }
                else
                {
                    if (regActivoHist != null)
                    {
                        // solo si se cambia el valor se considera que hay diferencia y se genera un nuevo registro 
                        if ((regActivoHist.Formuladat ?? "").Trim() != (entity.Formuladat ?? "").Trim())
                        {
                            PrGrupodatDTO regUpdate = (PrGrupodatDTO)regActivoHist.Clone();
                            regUpdate.Formuladat = entity.Formuladat;
                            regUpdate.Fechaact = entity.Fechaact;
                            regUpdate.Lastuser = entity.Lastuser;
                            regUpdate.Gdatcheckcero = entity.Gdatcheckcero.Value;
                            regUpdate.Gdatcomentario = entity.Gdatcomentario;
                            regUpdate.Gdatsustento = entity.Gdatsustento;
                            regUpdate.Deleted = 0;
                            regUpdate.Deleted2 = 0;
                            listaUpdate.Add(regUpdate);

                            //el registro que ya está en bd, se le genera una copia y esa se guarda como eliminado para tener la historia de los cambios
                            regActivoHist.Deleted = listaHistXCnp.Max(x => x.Deleted) + 1;
                            listaNew.Add(regActivoHist);
                        }
                        else
                        {
                            //si hay cambios en campos opcionales
                            if (entity.Gdatcheckcero != regActivoHist.Gdatcheckcero 
                                || entity.Gdatcomentario != regActivoHist.Gdatcomentario 
                                || entity.Gdatsustento != regActivoHist.Gdatsustento)
                            {
                                PrGrupodatDTO regUpdate = (PrGrupodatDTO)regActivoHist.Clone();
                                regUpdate.Gdatcheckcero = entity.Gdatcheckcero;
                                regUpdate.Gdatcomentario = entity.Gdatcomentario;
                                regUpdate.Gdatsustento = entity.Gdatsustento;
                                regUpdate.Deleted2 = entity.Deleted;

                                listaUpdate.Add(regUpdate);
                            }
                        }
                    }
                    else
                    {
                        //si no hay algun historico para ese día se considera un nuevo registro
                        entity.Deleted = 0;
                        listaNew.Add(entity);
                    }
                }

                //persistir en BD
                foreach (var reg in listaNew)
                    FactorySic.GetPrGrupodatRepository().Save(reg);
                foreach (var reg in listaUpdate)
                    FactorySic.GetPrGrupodatRepository().Update(reg);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PR_GRUPODAT
        /// </summary>
        public void DeletePrGrupodat(DateTime fechaDat, int concepcodi, int grupocodi, string usuario)
        {
            PrGrupodatDTO reg = new PrGrupodatDTO();
            reg.Grupocodi = grupocodi;
            reg.Concepcodi = concepcodi;
            reg.Fechadat = fechaDat;
            reg.Lastuser = usuario;
            reg.Fechaact = DateTime.Now;
            reg.Deleted = 0;

            ActualizarGrupodat(true, reg);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_GRUPODAT
        /// </summary>
        public PrGrupodatDTO GetByIdPrGrupodat(DateTime fechadat, int concepcodi, int grupocodi, int deleted)
        {
            return FactorySic.GetPrGrupodatRepository().GetById(fechadat, concepcodi, grupocodi, deleted);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_GRUPODAT
        /// </summary>
        public List<PrGrupodatDTO> ListPrGrupodats()
        {
            return FactorySic.GetPrGrupodatRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrGrupodat
        /// </summary>
        public List<PrGrupodatDTO> GetByCriteriaPrGrupodats(int concepcodi)
        {
            return FactorySic.GetPrGrupodatRepository().GetByCriteria(concepcodi);
        }

        /// <summary>
        /// ListarValoresHistoricosDespacho
        /// </summary>
        /// <param name="concepcodi"></param>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        public List<PrGrupodatDTO> ListarValoresHistoricosDespacho(int concepcodi, int grupocodi)
        {
            return FactorySic.GetPrGrupodatRepository().ListarHistoricoValores(concepcodi.ToString(), grupocodi);
        }

        /// <summary>
        /// ParametrosPorFecha
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<PrGrupodatDTO> ParametrosPorFecha(DateTime fecha)
        {
            return FactorySic.GetPrGrupodatRepository().ParametrosPorFecha(fecha);
        }

        /// <summary>
        /// ParametrosGeneralesPorFecha
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<PrGrupodatDTO> ParametrosGeneralesPorFecha(DateTime fecha)
        {
            List<PrGrupodatDTO> l = FactorySic.GetPrGrupodatRepository().ParametrosGeneralesPorFecha(fecha);

            foreach (var reg in l)
            {
                reg.FechadatDesc = reg.Fechadat != null ? reg.Fechadat.Value.ToString(ConstantesAppServicio.FormatoFecha) : string.Empty;
                reg.Lastuser = reg.Lastuser != null ? reg.Lastuser.Trim() : "SISTEMA";
                reg.FechaactDesc = reg.Fechaact != null ? reg.Fechaact.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
            }

            return l;
        }

        /// <summary>
        /// Parametros Actualizados Por Fecha
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<PrGrupodatDTO> ParametrosActualizadosPorFecha(DateTime fecha)
        {
            return FactorySic.GetPrGrupodatRepository().ListarParametrosActualizadosPorFecha(fecha);
        }

        #endregion

        #region Métodos Tabla PR_EQUIPODAT

        /// <summary>
        /// Inserta un registro de la tabla PR_EQUIPODAT
        /// </summary>
        public void SavePrEquipodat(PrEquipodatDTO entity)
        {
            try
            {
                FactorySic.GetPrEquipodatRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_EQUIPODAT
        /// </summary>
        public void UpdatePrEquipodat(PrEquipodatDTO entity)
        {
            try
            {
                FactorySic.GetPrEquipodatRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PR_EQUIPODAT
        /// </summary>
        public void DeletePrEquipodat(int equicodi, int grupocodi, int concepcodi, DateTime fechadat)
        {
            try
            {
                FactorySic.GetPrEquipodatRepository().Delete(equicodi, grupocodi, concepcodi, fechadat);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_EQUIPODAT
        /// </summary>
        public PrEquipodatDTO GetByIdPrEquipodat(int equicodi, int grupocodi, int concepcodi, DateTime fechadat)
        {
            return FactorySic.GetPrEquipodatRepository().GetById(equicodi, grupocodi, concepcodi, fechadat);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_EQUIPODAT
        /// </summary>
        public List<PrEquipodatDTO> ListPrEquipodats()
        {
            return FactorySic.GetPrEquipodatRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrEquipodat
        /// </summary>
        public List<PrEquipodatDTO> GetByCriteriaPrEquipodats()
        {
            return FactorySic.GetPrEquipodatRepository().GetByCriteria();
        }

        #endregion

        public List<PrGrupoConceptoDato> ObtenerDatosMO_URS(int iGrupoCodi, DateTime fechaRegistro)
        {
            var oGrupo = FactorySic.GetPrGrupoRepository().GetById(iGrupoCodi);
            if (oGrupo.Grupotipo == "T")
                return FactorySic.GetPrGrupodatRepository().ObtenerDatosMO_URS(iGrupoCodi, fechaRegistro);
            else
                return FactorySic.GetPrGrupodatRepository().ObtenerDatosMO_URS_Hidro(iGrupoCodi, fechaRegistro);
        }

        public List<PrGrupoConceptoDato> ObtenerParametrosGeneralesUrs()
        {
            return FactorySic.GetPrGrupodatRepository().ObtenerParametrosGeneralesUrs();
        }

        public List<ModoOperacionParametrosDTO> ListarModosOperacionParametros(int iGrupoCodi)
        {
            return FactorySic.GetPrGrupoRepository().ListarModosOperacionParametros(iGrupoCodi);
        }

        public List<ModoOperacionCostosDTO> ListarModosOperacionCostos()
        {
            throw new NotImplementedException();
        }
        #region Métodos Tabla PR_ESCENARIO

        /// <summary>
        /// Inserta un registro de la tabla PR_ESCENARIO
        /// </summary>
        public void SavePrEscenario(PrEscenarioDTO entity)
        {
            try
            {
                FactorySic.GetPrEscenarioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_ESCENARIO
        /// </summary>
        public void UpdatePrEscenario(PrEscenarioDTO entity)
        {
            try
            {
                FactorySic.GetPrEscenarioRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PR_ESCENARIO
        /// </summary>
        public void DeletePrEscenario(int escecodi)
        {
            try
            {
                FactorySic.GetPrEscenarioRepository().Delete(escecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_ESCENARIO
        /// </summary>
        public PrEscenarioDTO GetByIdPrEscenario(int escecodi)
        {
            return FactorySic.GetPrEscenarioRepository().GetById(escecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_ESCENARIO
        /// </summary>
        public List<PrEscenarioDTO> ListPrEscenarios()
        {
            return FactorySic.GetPrEscenarioRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrEscenario
        /// </summary>
        public List<PrEscenarioDTO> GetByCriteriaPrEscenarios()
        {
            return FactorySic.GetPrEscenarioRepository().GetByCriteria();
        }

        public List<PrEscenarioDTO> GetEscenariosPorFechaRepCv(DateTime fecha)
        {
            return FactorySic.GetPrEscenarioRepository().GetEscenariosPorFechaRepCv(fecha);
        }

        #endregion

        #region Métodos Tabla PR_REPCV

        /// <summary>
        /// Inserta un registro de la tabla PR_REPCV
        /// </summary>
        public void SavePrRepcv(PrRepcvDTO entity)
        {
            try
            {
                FactorySic.GetPrRepcvRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_REPCV
        /// </summary>
        public void UpdatePrRepcv(PrRepcvDTO entity)
        {
            try
            {
                FactorySic.GetPrRepcvRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PR_REPCV
        /// </summary>
        public void DeletePrRepcv(int repcodi)
        {
            try
            {
                FactorySic.GetPrRepcvRepository().Delete(repcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_REPCV
        /// </summary>
        public PrRepcvDTO GetByIdPrRepcv(int repcodi)
        {
            return FactorySic.GetPrRepcvRepository().GetById(repcodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_REPCV
        /// </summary>
        public PrRepcvDTO GetByFechaAndTipo(DateTime fecha, string tipo)
        {
            return FactorySic.GetPrRepcvRepository().GetByFechaAndTipo(fecha, tipo);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_REPCV
        /// </summary>
        public List<PrRepcvDTO> ListPrRepcvs()
        {
            return FactorySic.GetPrRepcvRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrRepcv
        /// </summary>
        public List<PrRepcvDTO> GetByCriteriaPrRepcvs(string tipoPrograma, DateTime dFechaInicio, DateTime dFechaFin)
        {
            List<PrRepcvDTO> l = FactorySic.GetPrRepcvRepository().GetByCriteria(dFechaInicio, dFechaFin);
            l = tipoPrograma == ConstantesDespacho.TipoProgramaSemanal ? l.Where(x => x.Reptipo == ConstantesDespacho.TipoProgramaSemanal).ToList() :
                 (tipoPrograma == ConstantesDespacho.TipoProgramaDiario ? l.Where(x => x.Reptipo == ConstantesDespacho.TipoProgramaDiario).ToList() : l);

            foreach (var reg in l)
            {
                reg.ReptipoDesc = reg.Reptipo == ConstantesDespacho.TipoProgramaSemanal ? ConstantesDespacho.TipoProgramaSemanalDesc : ConstantesDespacho.TipoProgramaDiarioDesc;
                reg.RepfechaDesc = reg.Repfecha.ToString(ConstantesAppServicio.FormatoFechaHoraAMPM);
                reg.RepfechaemDesc = reg.Repfechaem != null ? reg.Repfechaem.Value.ToString(ConstantesAppServicio.FormatoFechaHoraAMPM) : string.Empty;
            }

            return l;
        }

        public void GenerarCostosVariables(PrRepcvDTO entity, ref List<PrCvariablesDTO> lsCostosVariables, bool bRegistrarBaseDatos)
        {
            int li_grupocodi, li_emprcodi, li_grupopadre, li_grupocentral;
            int li_nrotermicos = 0, li_nrohidraulicos = 0;
            string[] ls_break = new string[3];
            string ls_comandoA = "";
            string ls_comandoB = "";
            string ls_comando = "";
            string ls_comando2 = "";
            List<string> ALColumn = new List<string>();

            try
            {
                //bRegistrarBaseDatos = true;

                if (bRegistrarBaseDatos)
                    FactorySic.GetPrCvariablesRepository().EliminarCostosVariablesPorRepCv(entity.Repcodi);//Si existen registros de Costos Variables para el reporte, se eliminan

                //COSTOS VARIABLES
                ALColumn.Add("CEC_SI");
                ALColumn.Add("PE");
                ALColumn.Add("REND_SI");
                ALColumn.Add("CCOMB");
                ALColumn.Add("CVNC");
                ALColumn.Add("CVC");
                ALColumn.Add("CV");
                //---------------------

                n_parameter l_PGenerales = new n_parameter();
                ls_comandoA = "INSERT INTO PR_CVARIABLES(REPCODI,ESCECODI";
                ls_comandoB = " VALUES (" + entity.Repcodi + ",0";
                var lParametrosGenerales = FactorySic.GetPrGrupodatRepository().ParametrosGeneralesPorFecha(entity.Repfecha);
                var lParametrosFuncionales = FactorySic.GetPrGrupodatRepository().ParametrosPorFecha(entity.Repfecha);
                var lGruposFuncionales = FactorySic.GetPrGrupoRepository().ListadoModosFuncionalesCostosVariables(entity.Repfecha, ConstantesAppServicio.SI);

                ReporteCostoIncrementalDTO costoIncr;
                PrCvariablesDTO oCostoVariable;
                n_parameter l_param;

                foreach (var oGrupoFunc in lGruposFuncionales)
                {
                    costoIncr = new GrupoDespachoAppServicio().ObtenerReporteCostoIncremental(entity.Repfecha, lParametrosGenerales, oGrupoFunc);

                    oCostoVariable = new PrCvariablesDTO();

                    l_param = new n_parameter();
                    li_grupocodi = oGrupoFunc.Grupocodi;
                    li_emprcodi = oGrupoFunc.Emprcodi.Value;

                    oCostoVariable.Grupocodi = oGrupoFunc.Grupocodi;
                    oCostoVariable.Gruponomb = oGrupoFunc.Gruponomb;
                    oCostoVariable.Grupoabrev = oGrupoFunc.Grupoabrev;
                    oCostoVariable.GruponombPadre = oGrupoFunc.GruponombPadre;
                    oCostoVariable.Emprnomb = oGrupoFunc.EmprNomb;
                    oCostoVariable.TipoModo = oGrupoFunc.Grupotipo;
                    oCostoVariable.Repfecha = entity.Repfecha;

                    li_grupopadre = oGrupoFunc.Grupopadre.Value;
                    li_grupocentral = oGrupoFunc.GrupoCentral;
                    ls_comando = ls_comandoA + ", GRUPOCODI" + ", EMPRCODI";
                    ls_comando2 = ls_comandoB + "," + li_grupocodi.ToString() + "," + li_emprcodi.ToString();

                    foreach (var drParam in lParametrosGenerales)
                    {
                        l_param.SetData(drParam.Concepabrev.Trim(), drParam.Formuladat.Trim());
                        l_PGenerales.SetData(drParam.Concepabrev.Trim(), drParam.Formuladat.Trim());
                    }
                    foreach (var drLast in lParametrosFuncionales)
                    {
                        if (drLast.Grupocodi == li_grupocodi || drLast.Grupocodi == li_grupopadre || drLast.Grupocodi == li_grupocentral)
                        {
                            l_param.SetData(drLast.Concepabrev.Trim(), drLast.Formuladat.Trim());
                        }
                    }

                    l_param.SetData("CV", "CVNC+CVC");

                    if (oGrupoFunc.Grupotipo == "H")
                    {
                        li_nrohidraulicos++;
                        //l_param.SetData("CV", "CVNC+CVC+CANON/1000");
                        l_param.SetData("CVNC", "0");
                        l_param.SetData("CVC", "COSTOSS");
                        l_param.SetData("CV", "COSTOOM");//"CVNC+CVC+CANON/1000");
                        l_param.SetData("CEC_SI", "0");
                        l_param.SetData("PE", "0");
                        l_param.SetData("REND_SI", "0");
                        l_param.SetData("CCOMB", "0");
                    }
                    else
                    {
                        li_nrotermicos++;
                    }
                    foreach (var sColumn in ALColumn)
                    {
                        if (sColumn == "CVC" || sColumn == "CVNC" || sColumn == "CCOMB" || sColumn == "PE" || sColumn == "CEC_SI" || sColumn == "REND_SI")
                        {
                            ls_comando += ", " + sColumn;
                            ls_comando2 += "," + l_param.GetEvaluate(sColumn);
                        }
                        switch (sColumn)
                        {
                            case "CVC":
                                oCostoVariable.Cvc = Convert.ToDecimal(l_param.GetEvaluate(sColumn));
                                oCostoVariable.FormulaCvc = l_param.EvaluateFormula(sColumn);
                                break;
                            case "CVNC":
                                oCostoVariable.Cvnc = Convert.ToDecimal(l_param.GetEvaluate(sColumn));
                                oCostoVariable.FormulaCvnc = l_param.EvaluateFormula(sColumn);
                                break;
                            case "CCOMB":
                                oCostoVariable.Ccomb = Convert.ToDecimal(l_param.GetEvaluate(sColumn));
                                oCostoVariable.FormulaCcomb = l_param.EvaluateFormula(sColumn);
                                break;
                            case "PE":
                                oCostoVariable.Pe = Convert.ToDecimal(l_param.GetEvaluate(sColumn));
                                oCostoVariable.FormulaPe = l_param.EvaluateFormula(sColumn);
                                break;
                            case "CEC_SI":
                                oCostoVariable.CecSi = Convert.ToDecimal(l_param.GetEvaluate(sColumn));
                                oCostoVariable.FormulaCecSi = l_param.EvaluateFormula(sColumn);
                                break;
                            case "REND_SI":
                                oCostoVariable.RendSi = Convert.ToDecimal(l_param.GetEvaluate(sColumn));
                                oCostoVariable.FormulaRendSi = l_param.EvaluateFormula(sColumn);
                                break;
                            case "CV":
                                oCostoVariable.Cv = Convert.ToDecimal(l_param.GetEvaluate(sColumn));
                                oCostoVariable.FormulaCv = l_param.EvaluateFormula(sColumn);
                                break;
                        }
                    }

                    ls_comando += ", CVARTRAMO1, CVARCINCREM1, CVARTRAMO2, CVARCINCREM2, CVARTRAMO3, CVARCINCREM3, CVARPE1, CVARPE2, CVARPE3, CVARPE4";
                    ls_comando2 += string.Format(", '{0}', {1}, '{2}', {3}, '{4}', {5}, '{6}', {7}, '{8}', {9}", 
                                                        costoIncr.Tramo1, costoIncr.Cincrem1,
                                                        costoIncr.Tramo2, costoIncr.Cincrem2,
                                                        costoIncr.Tramo3, costoIncr.Cincrem3,
                                                        costoIncr.Pe1, costoIncr.Pe2, costoIncr.Pe3, costoIncr.Pe4);

                    ls_comando += ")";
                    ls_comando2 += ")";

                    ls_comando += ls_comando2;

                    if (bRegistrarBaseDatos)
                        FactorySic.GetPrCvariablesRepository().EjecutarComandoCv(ls_comando);

                    lsCostosVariables.Add(oCostoVariable);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla PR_CVARIABLES

        /// <summary>
        /// Inserta un registro de la tabla PR_CVARIABLES
        /// </summary>
        //public void SavePrCvariables(PrCvariablesDTO entity)
        //{
        //    try
        //    {
        //        FactorySic.GetPrCvariablesRepository().Save(entity);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ConstantesAppServicio.LogError, ex);
        //        throw new Exception(ex.Message, ex);
        //    }
        //}

        /// <summary>
        /// Actualiza un registro de la tabla PR_CVARIABLES
        /// </summary>
        public void UpdatePrCvariables(PrCvariablesDTO entity)
        {
            try
            {
                FactorySic.GetPrCvariablesRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PR_CVARIABLES
        /// </summary>
        public void DeletePrCvariables()
        {
            try
            {
                FactorySic.GetPrCvariablesRepository().Delete();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_CVARIABLES
        /// </summary>
        public PrCvariablesDTO GetByIdPrCvariables()
        {
            return FactorySic.GetPrCvariablesRepository().GetById();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_CVARIABLES
        /// </summary>
        public List<PrCvariablesDTO> ListPrCvariabless(int id)
        {
            return FactorySic.GetPrCvariablesRepository().ListPrCvariabless(id);
        }

        /// <summary>
        /// Permite listar los modos de operación de las unidades termoeléctricas
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoDTO> ListarModoOperacionCategoriaTermico()
        {
            return FactorySic.GetPrGrupoRepository().ListarModoOperacionCategoriaTermico();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrCvariables
        /// </summary>
        public List<PrCvariablesDTO> GetByCriteriaPrCvariabless()
        {
            return FactorySic.GetPrCvariablesRepository().GetByCriteria();
        }

        /// <summary>
        /// Devuelve el listado de Costos variables vigentes para el reporte costo variable indicado
        /// </summary>
        /// <param name="repcvCodi">Código de RepCV</param>
        /// <returns></returns>
        public List<PrCvariablesDTO> GetCostosVariablesPorRepCv(int repcvCodi)
        {
            return FactorySic.GetPrCvariablesRepository().GetCostosVariablesPorRepCv(repcvCodi);
        }

        #endregion

        #region Métodos Tabla DOC_FLUJO

        /// <summary>
        /// Lista los documentos de costos variables
        /// </summary>
        public List<DocFlujoDTO> ListarDocumentosCostoVariables(DateTime fechaIni, DateTime fechaFin)
        {
            List<DocFlujoDTO> l = FactorySic.GetDocFlujoRepository().ListDocCV(fechaIni, fechaFin);

            foreach (var reg in l)
            {
                reg.FljfecharecepDesc = reg.Fljfecharecep != null ? reg.Fljfecharecep.Value.ToString(ConstantesAppServicio.FormatoFechaHoraAMPM) : string.Empty;
                reg.FljfechaorigDesc = reg.Fljfechaorig != null ? reg.Fljfechaorig.Value.ToString(ConstantesAppServicio.FormatoFechaHoraAMPM) : string.Empty;
            }

            return l;
        }

        #endregion

        #region Métodos Precios de Combustibles

        /// <summary>
        /// Permite obtener las empresas del SEIN
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasPreciosCombustibles()
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasSEIN();
        }

        /// <summary>
        /// Permite obtener el resultado de la formula de un conjunto de grupos
        /// </summary>
        /// <param name="idsGrupos">Lista de códigos de los grupos</param>
        /// <param name="concepto">Concepto a evaluar</param>
        /// /// <param name="fechaDatos">Fecha de los datos de los conceptos</param>
        /// <returns></returns>
        public List<PrGrupodatDTO> ObtenerResultadoFormula(List<int> idsGrupos, int concepto, DateTime fechaDatos)
        {
            List<PrGrupodatDTO> result = new List<PrGrupodatDTO>();

            // Listamos los parámetros generales
            List<PrGrupodatDTO> conceptosBase = FactorySic.GetPrGrupodatRepository().ObtenerParametroPorConcepto((-1).ToString());

            // Parámetros de los grupos
            List<PrGrupodatDTO> conceptos = FactorySic.GetPrGrupodatRepository().ObtenerParametroPorCentral(
                string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsGrupos));

            foreach (int id in idsGrupos)
            {
                // Obtener la formula a evaluar
                string formula = conceptos.Where(x => x.Concepcodi == concepto && x.Grupocodi == id).
                    Select(x => x.Concepabrev).Distinct().FirstOrDefault();

                if (!string.IsNullOrEmpty(formula))
                {
                    // Instanciamos la clase n_parameter encargada de obtener el valor de las formulas
                    n_parameter parameter = new n_parameter();

                    // Obtenemos los conceptos pertenecientes al grupo
                    List<PrGrupodatDTO> conceptosGrupo = conceptos.Where(x => x.Grupocodi == id).Distinct().ToList();

                    // Llenamos los conceptos base al objeto n_parameter
                    foreach (PrGrupodatDTO itemConcepto in conceptosBase)
                        parameter.SetData(itemConcepto.Concepabrev, itemConcepto.Formuladat);

                    // Llenamos los conceptos del grupo al objeto n_parameter
                    foreach (PrGrupodatDTO itemConcepto in conceptosGrupo)
                        parameter.SetData(itemConcepto.Concepabrev, itemConcepto.Formuladat);

                    double valor = parameter.GetEvaluate(formula);

                    // Agregamos el resultado a a la lista
                    PrGrupodatDTO entity = new PrGrupodatDTO();
                    entity.Grupocodi = id;
                    entity.Valor = (decimal)valor;
                    entity.Formuladat = formula;
                    result.Add(entity);
                }
            }

            return result;
        }

        /// <summary>
        /// Permite obtener el valor de algún concepto
        /// </summary>
        /// <param name="concepto"></param>
        /// <returns></returns>
        public decimal ObtenerParametroPorConcepto(string concepto)
        {
            List<PrGrupodatDTO> conceptosBase = FactorySic.GetPrGrupodatRepository().ObtenerParametroPorConcepto(concepto);

            if (conceptosBase.Count >= 0)
            {
                PrGrupodatDTO entity = conceptosBase[0];
                decimal valor = 0;

                if (decimal.TryParse(entity.Formuladat, out valor))
                {
                    return valor;
                }
            }

            return 0;
        }

        /// <summary>
        /// Permite obtener el reporte de precios de combustible
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<PrGrupodatDTO> ObtenerReportePrecioCombustible(int? idEmpresa)
        {
            List<PrGrupoDTO> list = FactorySic.GetPrGrupoRepository().ObtenerTipoCombustiblePorCentral();
            List<int> idsGrupo = list.Select(x => x.Grupocodi).Distinct().ToList();

            int[] idsConceptos = { 1, 10, 18, 19, 21 };

            List<PrGrupodatDTO> conceptosBase = FactorySic.GetPrGrupodatRepository().ObtenerParametroPorConcepto(
                string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsConceptos.ToList()));
            List<PrGrupodatDTO> conceptos = FactorySic.GetPrGrupodatRepository().ObtenerParametroPorCentral(
                string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsGrupo));


            List<PrGrupodatDTO> result = new List<PrGrupodatDTO>();

            foreach (int idGrupo in idsGrupo)
            {
                n_parameter parameter = new n_parameter();

                List<PrGrupodatDTO> conceptosGrupo = conceptos.Where(x => x.Grupocodi == idGrupo).Distinct().ToList();

                foreach (PrGrupodatDTO itemConcepto in conceptosBase)
                    parameter.SetData(itemConcepto.Concepabrev, itemConcepto.Formuladat);

                foreach (PrGrupodatDTO itemConcepto in conceptosGrupo)
                    parameter.SetData(itemConcepto.Concepabrev, itemConcepto.Formuladat);

                List<PrGrupoDTO> listGrupo = list.Where(x => x.Grupocodi == idGrupo).ToList();

                /*
                3 DISSEL CTotalDB5
                4 RESIDUAL CTotalR500 CTotalR6
                5 CARBON CTotalCarb
                2 GAS CCombGas_SI
                */

                foreach (PrGrupoDTO itemGrupo in listGrupo)
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();
                    entity.Emprnomb = itemGrupo.EmprNomb;
                    entity.Centralnomb = itemGrupo.Gruponomb;
                    entity.Tipocombustible = itemGrupo.Fenergnomb;

                    string formula = string.Empty;
                    string formulaAlterna = string.Empty;
                    double valor = 0;
                    double valorAlternativo = 0;


                    if (itemGrupo.Fenergcodi == 2)
                    {
                        formula = ConstantesDespacho.CCombGasSI;
                    }
                    else if (itemGrupo.Fenergcodi == 3)
                    {
                        formula = ConstantesDespacho.CTotalDB5;
                    }
                    else if (itemGrupo.Fenergcodi == 5)
                    {
                        formula = ConstantesDespacho.CTotalCarb;
                    }
                    else if (itemGrupo.Fenergcodi == 6)
                    {
                        formula = ConstantesDespacho.CTotalBag;
                    }
                    else if (itemGrupo.Fenergcodi == 7)
                    {
                        formula = ConstantesDespacho.CTotalBio;
                    }
                    else if (itemGrupo.Fenergcodi == 10)
                    {
                        formula = ConstantesDespacho.CTotalR500;
                    }
                    else if (itemGrupo.Fenergcodi == 11)
                    {
                        formula = ConstantesDespacho.CTotalR6;
                    }

                    if (!string.IsNullOrEmpty(formula))
                    {
                        valor = parameter.GetEvaluate(formula);
                    }
                    if (!string.IsNullOrEmpty(formulaAlterna))
                    {
                        valorAlternativo = parameter.GetEvaluate(formulaAlterna);
                    }

                    if (string.IsNullOrEmpty(formulaAlterna))
                    {
                        entity.Valor = (decimal)valor;
                        PrGrupodatDTO concepto = conceptosGrupo.Where(x => x.Concepabrev == formula).FirstOrDefault();
                        PrGrupodatDTO concepto2;
                        if (concepto != null)
                        {
                            entity.ConcepUni = concepto.ConcepUni;
                            entity.Fechadat = concepto.Fechadat;
                        }
                        if (itemGrupo.Fenergcodi == 3)
                        {
                            concepto2 = conceptosGrupo.OrderByDescending(x => x.Concepabrev == "ImpDB5").FirstOrDefault();
                            entity.Fechadat = concepto2.Fechadat;
                        }
                        if (itemGrupo.Gruponomb == "C.T. SANTA ROSA" && itemGrupo.Fenergcodi == 3)
                        {
                            concepto2 = conceptosGrupo.OrderByDescending(x => x.Concepabrev == "PCombDB5").FirstOrDefault();
                            entity.Fechadat = concepto2.Fechadat;
                        }


                    }
                    else
                    {
                        entity.Valor = (decimal)valor;
                        PrGrupodatDTO concepto = conceptosGrupo.Where(x => x.Concepabrev == formula).FirstOrDefault();
                        if (concepto != null)
                        {
                            entity.ConcepUni = concepto.ConcepUni;
                            entity.Fechadat = concepto.Fechadat;
                        }

                        if (valorAlternativo != 0)
                        {
                            entity.Valor = (decimal)valorAlternativo;
                            PrGrupodatDTO conceptoAlterno = conceptosGrupo.Where(x => x.Concepabrev == formulaAlterna).FirstOrDefault();
                            if (concepto != null)
                            {
                                entity.ConcepUni = conceptoAlterno.ConcepUni;
                                entity.Fechadat = conceptoAlterno.Fechadat;
                            }
                        }
                    }

                    result.Add(entity);
                }
            }

            return result;
        }



        #endregion

        #region Métodos Tabla PR_CONFIGURACION_POT_EFECTIVA

        /// <summary>
        /// Inserta un registro de la tabla PR_CONFIGURACION_POT_EFECTIVA
        /// </summary>
        public void SavePrConfiguracionPotEfectiva(PrConfiguracionPotEfectivaDTO entity)
        {
            try
            {
                FactorySic.GetPrConfiguracionPotEfectivaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_CONFIGURACION_POT_EFECTIVA
        /// </summary>
        public void UpdatePrConfiguracionPotEfectiva(PrConfiguracionPotEfectivaDTO entity)
        {
            try
            {
                FactorySic.GetPrConfiguracionPotEfectivaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PR_CONFIGURACION_POT_EFECTIVA
        /// </summary>
        public void DeletePrConfiguracionPotEfectiva(int grupocodi)
        {
            try
            {
                FactorySic.GetPrConfiguracionPotEfectivaRepository().Delete(grupocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_CONFIGURACION_POT_EFECTIVA
        /// </summary>
        public PrConfiguracionPotEfectivaDTO GetByIdPrConfiguracionPotEfectiva(int grupocodi)
        {
            return FactorySic.GetPrConfiguracionPotEfectivaRepository().GetById(grupocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_CONFIGURACION_POT_EFECTIVA
        /// </summary>
        public List<PrConfiguracionPotEfectivaDTO> ListPrConfiguracionPotEfectivas()
        {
            return FactorySic.GetPrConfiguracionPotEfectivaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrConfiguracionPotEfectiva
        /// </summary>
        public List<PrConfiguracionPotEfectivaDTO> GetByCriteriaPrConfiguracionPotEfectivas()
        {
            return FactorySic.GetPrConfiguracionPotEfectivaRepository().GetByCriteria();
        }
        /// <summary>
        /// Listado de modos de operación para configuración de reporte de PE
        /// </summary>
        /// <param name="iEmpresa">Código de empresa</param>
        /// <param name="sEstado">Código estado</param>
        /// <returns></returns>
        public List<PrGrupoDTO> ListadoModosOperacionConfiguracionPe(int iEmpresa, string sEstado)
        {
            var lsTodosModos = FactorySic.GetPrGrupoRepository().ModosOperacionxFiltro(iEmpresa, sEstado, " ", 1, int.MaxValue);
            var lsModosConfigurados = FactorySic.GetPrConfiguracionPotEfectivaRepository().List();

            foreach (var oGrupo in lsTodosModos)
            {
                oGrupo.ConfiguradoReportePe = lsModosConfigurados.Exists(t => t.Grupocodi == oGrupo.Grupocodi);
            }
            return lsTodosModos;
        }
        /// <summary>
        /// Elimina todos los registros de la tabla PR_CONFIGURACION_POT_EFECTIVA
        /// </summary>
        public void DeleteAllPrConfiguracionPotEfectiva()
        {
            try
            {
                FactorySic.GetPrConfiguracionPotEfectivaRepository().DeleteAll();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta los registros de la tabla PR_CONFIGURACION_POT_EFECTIVA
        /// </summary>
        public void SaveAllPrConfiguracionPotEfectiva(List<PrConfiguracionPotEfectivaDTO> entity)
        {
            try
            {
                FactorySic.GetPrConfiguracionPotEfectivaRepository().SaveAll(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<PrGrupoDTO> ListarModosOperacionNoConfigurados(int idEmpresa)
        {
            return FactorySic.GetPrGrupoRepository().ListarModosOperacionNoConfigurados(idEmpresa);
        }
        public List<PrGrupoDTO> ListarModosOperacionConfigurados(int idEmpresa)
        {
            return FactorySic.GetPrGrupoRepository().ListarModosOperacionConfigurados(idEmpresa);
        }
        #endregion

        #region Barras Modeladas

        /// <summary>
        /// Método que devuelve el listado de Barras Modeladas activas y paginadas
        /// </summary>
        /// <param name="numPagina">Numero de página</param>
        /// <param name="tamPagina">Tamaño de página</param>
        /// <returns></returns>
        public List<PrGrupoDTO> ListadoBarrasModeladasPaginado(int numPagina, int tamPagina)
        {
            return FactorySic.GetPrGrupoRepository()
                .ListaPrGruposPorCategoriaPaginado(ConstantesDespacho.CategoriaBarraModelada, "A", numPagina,
                    tamPagina);
        }

        /// <summary>
        /// Lista los grupos por categoria
        /// </summary>
        /// <param name="catecodi"></param>
        /// <param name="estado"></param>
        /// <param name="nroPagina"></param>
        /// <param name="lenPagina"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> ObtenerGruposPorCategoria(int catecodi, string estado, int nroPagina, int lenPagina)
        {
            return FactorySic.GetPrGrupoRepository().ListaPrGruposPorCategoriaPaginado(catecodi, estado, nroPagina, lenPagina);
        }

        /// <summary>
        /// Listado de Barras de Equipamiento asociadas a una Barra Modelada
        /// </summary>
        /// <param name="grupocodi">Código de Barra Modelada</param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListadoBarrasEquipoPorBarraModelada(int grupocodi)
        {
            return FactorySic.GetEqEquipoRepository().ListadoEquiposPorGrupoCodiFamilia(grupocodi, ConstantesDespacho.FamiliaBarraEquipo);
        }

        /// <summary>
        /// Listado de Barras de Equipamiento activas que no están asociadas a ninguna Barra Modelada
        /// </summary>
        /// <returns></returns>
        public List<EqEquipoDTO> ListadoBarrasEquipoDisponibles()
        {
            var barrasEquipo = FactorySic.GetEqEquipoRepository().ListadoEquiposPorGrupoCodiFamilia(-2, ConstantesDespacho.FamiliaBarraEquipo);
            return barrasEquipo.Where(t => t.Grupocodi == null).ToList();
        }

        public void ActualizarBarrasEquipoPorBarraModelada(string sCodigosEquipo, int iGrupocodi, string sUsuario)
        {
            var listaEquipoOriginal = FactorySic.GetEqEquipoRepository().ListadoEquiposPorGrupoCodiFamilia(iGrupocodi, ConstantesDespacho.FamiliaBarraEquipo);

            if (listaEquipoOriginal != null && listaEquipoOriginal.Count > 0)
            {
                var sCodigosEquipoOriginales = string.Join(",", listaEquipoOriginal.Select(n => n.Equicodi.ToString()).ToArray());
                FactorySic.GetEqEquipoRepository().ActualizarGrupoCodiPorEquipoFamilia(sCodigosEquipoOriginales, null, ConstantesDespacho.FamiliaBarraEquipo, sUsuario);//Se quitan los equipos originales
            }
            if (!string.IsNullOrEmpty(sCodigosEquipo))
                FactorySic.GetEqEquipoRepository().ActualizarGrupoCodiPorEquipoFamilia(sCodigosEquipo, iGrupocodi, ConstantesDespacho.FamiliaBarraEquipo, sUsuario);//Se asignan los nuevos equipos
        }

        #endregion

        #region SCOCOES func A
        public List<PrGrupodatDTO> ObtenerListaConfigScoSinac(DateTime fecha, int nroPagina, int pageSize)
        {
            return FactorySic.GetPrGrupodatRepository().ObtenerListaConfigScoSinac(fecha, nroPagina, pageSize);
        }


        /// <summary>
        /// Devuelve total registros de empresas segun el tipo
        /// </summary>
        /// <param name="fecha">Fecha</param>
        /// <returns>Numero de registros</returns>
        public int ObtenerTotalListaConfigScoSinac(DateTime fecha)
        {
            return FactorySic.GetPrGrupodatRepository().ObtenerTotalListaConfigScoSinac(fecha);
        }

        #endregion SCOCOES func A

        #region FIT- SGOCOES func A - Horas de Soporte  

        /// <summary>
        /// Permite grabar los datos cargados
        /// </summary>
        /// <param name="entitys"></param>
        public int Insertarmedicion48(DateTime fecha, int medicodi, string valor)
        {
            try
            {
                MeMedicion48DTO entity = new MeMedicion48DTO();
                entity.Medifecha = fecha;
                entity.Lectcodi = 6;
                entity.Ptomedicodi = medicodi;
                entity.Tipoinfocodi = 1;

                //Convertir valor en los Campos hora de H1 a H48                
                string[] horas = valor.Split(',');

                try { entity.H1 = Math.Round(Convert.ToDecimal(horas[0], new CultureInfo("en-US")), 5); } catch { entity.H1 = null; }
                try { entity.H2 = Math.Round(Convert.ToDecimal(horas[1], new CultureInfo("en-US")), 5); } catch { entity.H2 = null; }
                try { entity.H3 = Math.Round(Convert.ToDecimal(horas[2], new CultureInfo("en-US")), 5); } catch { entity.H3 = null; }
                try { entity.H4 = Math.Round(Convert.ToDecimal(horas[3], new CultureInfo("en-US")), 5); } catch { entity.H4 = null; }
                try { entity.H5 = Math.Round(Convert.ToDecimal(horas[4], new CultureInfo("en-US")), 5); } catch { entity.H5 = null; }
                try { entity.H6 = Math.Round(Convert.ToDecimal(horas[5], new CultureInfo("en-US")), 5); } catch { entity.H6 = null; }
                try { entity.H7 = Math.Round(Convert.ToDecimal(horas[6], new CultureInfo("en-US")), 5); } catch { entity.H7 = null; }
                try { entity.H8 = Math.Round(Convert.ToDecimal(horas[7], new CultureInfo("en-US")), 5); } catch { entity.H8 = null; }
                try { entity.H9 = Math.Round(Convert.ToDecimal(horas[8], new CultureInfo("en-US")), 5); } catch { entity.H9 = null; }
                try { entity.H10 = Math.Round(Convert.ToDecimal(horas[9], new CultureInfo("en-US")), 5); } catch { entity.H10 = null; }

                try { entity.H11 = Math.Round(Convert.ToDecimal(horas[10], new CultureInfo("en-US")), 5); } catch { entity.H11 = null; }
                try { entity.H12 = Math.Round(Convert.ToDecimal(horas[11], new CultureInfo("en-US")), 5); } catch { entity.H12 = null; }
                try { entity.H13 = Math.Round(Convert.ToDecimal(horas[12], new CultureInfo("en-US")), 5); } catch { entity.H13 = null; }
                try { entity.H14 = Math.Round(Convert.ToDecimal(horas[13], new CultureInfo("en-US")), 5); } catch { entity.H14 = null; }
                try { entity.H15 = Math.Round(Convert.ToDecimal(horas[14], new CultureInfo("en-US")), 5); } catch { entity.H15 = null; }
                try { entity.H16 = Math.Round(Convert.ToDecimal(horas[15], new CultureInfo("en-US")), 5); } catch { entity.H16 = null; }
                try { entity.H17 = Math.Round(Convert.ToDecimal(horas[16], new CultureInfo("en-US")), 5); } catch { entity.H17 = null; }
                try { entity.H18 = Math.Round(Convert.ToDecimal(horas[17], new CultureInfo("en-US")), 5); } catch { entity.H18 = null; }
                try { entity.H19 = Math.Round(Convert.ToDecimal(horas[18], new CultureInfo("en-US")), 5); } catch { entity.H19 = null; }
                try { entity.H20 = Math.Round(Convert.ToDecimal(horas[19], new CultureInfo("en-US")), 5); } catch { entity.H20 = null; }

                try { entity.H21 = Math.Round(Convert.ToDecimal(horas[20], new CultureInfo("en-US")), 5); } catch { entity.H21 = null; }
                try { entity.H22 = Math.Round(Convert.ToDecimal(horas[21], new CultureInfo("en-US")), 5); } catch { entity.H22 = null; }
                try { entity.H23 = Math.Round(Convert.ToDecimal(horas[22], new CultureInfo("en-US")), 5); } catch { entity.H23 = null; }
                try { entity.H24 = Math.Round(Convert.ToDecimal(horas[23], new CultureInfo("en-US")), 5); } catch { entity.H24 = null; }
                try { entity.H25 = Math.Round(Convert.ToDecimal(horas[24], new CultureInfo("en-US")), 5); } catch { entity.H25 = null; }
                try { entity.H26 = Math.Round(Convert.ToDecimal(horas[25], new CultureInfo("en-US")), 5); } catch { entity.H26 = null; }
                try { entity.H27 = Math.Round(Convert.ToDecimal(horas[26], new CultureInfo("en-US")), 5); } catch { entity.H27 = null; }
                try { entity.H28 = Math.Round(Convert.ToDecimal(horas[27], new CultureInfo("en-US")), 5); } catch { entity.H28 = null; }
                try { entity.H29 = Math.Round(Convert.ToDecimal(horas[28], new CultureInfo("en-US")), 5); } catch { entity.H29 = null; }
                try { entity.H30 = Math.Round(Convert.ToDecimal(horas[29], new CultureInfo("en-US")), 5); } catch { entity.H30 = null; }

                try { entity.H31 = Math.Round(Convert.ToDecimal(horas[30], new CultureInfo("en-US")), 5); } catch { entity.H31 = null; }
                try { entity.H32 = Math.Round(Convert.ToDecimal(horas[31], new CultureInfo("en-US")), 5); } catch { entity.H32 = null; }
                try { entity.H33 = Math.Round(Convert.ToDecimal(horas[32], new CultureInfo("en-US")), 5); } catch { entity.H33 = null; }
                try { entity.H34 = Math.Round(Convert.ToDecimal(horas[33], new CultureInfo("en-US")), 5); } catch { entity.H34 = null; }
                try { entity.H35 = Math.Round(Convert.ToDecimal(horas[34], new CultureInfo("en-US")), 5); } catch { entity.H35 = null; }
                try { entity.H36 = Math.Round(Convert.ToDecimal(horas[35], new CultureInfo("en-US")), 5); } catch { entity.H36 = null; }
                try { entity.H37 = Math.Round(Convert.ToDecimal(horas[36], new CultureInfo("en-US")), 5); } catch { entity.H37 = null; }
                try { entity.H38 = Math.Round(Convert.ToDecimal(horas[37], new CultureInfo("en-US")), 5); } catch { entity.H38 = null; }
                try { entity.H39 = Math.Round(Convert.ToDecimal(horas[38], new CultureInfo("en-US")), 5); } catch { entity.H39 = null; }
                try { entity.H40 = Math.Round(Convert.ToDecimal(horas[39], new CultureInfo("en-US")), 5); } catch { entity.H40 = null; }

                try { entity.H41 = Math.Round(Convert.ToDecimal(horas[40], new CultureInfo("en-US")), 5); } catch { entity.H41 = null; }
                try { entity.H42 = Math.Round(Convert.ToDecimal(horas[41], new CultureInfo("en-US")), 5); } catch { entity.H42 = null; }
                try { entity.H43 = Math.Round(Convert.ToDecimal(horas[42], new CultureInfo("en-US")), 5); } catch { entity.H43 = null; }
                try { entity.H44 = Math.Round(Convert.ToDecimal(horas[43], new CultureInfo("en-US")), 5); } catch { entity.H44 = null; }
                try { entity.H45 = Math.Round(Convert.ToDecimal(horas[44], new CultureInfo("en-US")), 5); } catch { entity.H45 = null; }
                try { entity.H46 = Math.Round(Convert.ToDecimal(horas[45], new CultureInfo("en-US")), 5); } catch { entity.H46 = null; }
                try { entity.H47 = Math.Round(Convert.ToDecimal(horas[46], new CultureInfo("en-US")), 5); } catch { entity.H47 = null; }
                try { entity.H48 = Math.Round(Convert.ToDecimal(horas[47], new CultureInfo("en-US")), 5); } catch { entity.H48 = null; }

                FactorySic.GetMeMedicion48Repository().Save(entity);

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                return 0;
            }
        }

        /// <summary>
        /// Permite eliminar un registro en me_medicion48 de la fecha del día
        /// </summary>
        /// <param name="entitys"></param>
        public int Eliminarmedicion48()
        {
            try
            {

                FactorySic.GetMeMedicion48Repository().DeleteSCOActualizacion();

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return 0;
            }
        }

        #endregion

        #region FICHA TÉCNICA

        /// <summary>
        /// Permite listar las categorias de grupos
        /// </summary>
        /// <returns></returns>
        public List<PrCategoriaDTO> ListarCategoriaGrupo()
        {
            return FactorySic.GetPrCategoriaRepository().GetByCriteria().Where(x => x.Catecodi >= 0 && x.Catecodi < 10).ToList();
        }

        /// <summary>
        /// Permite realizar búsquedas por categoria, nombre y ficha técnica
        /// </summary>
        /// <param name="catecodi"></param>
        /// <param name="fichaTecnica"></param>
        /// <param name="nombre"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public List<PrConceptoDTO> ListarConceptoxFiltro(int catecodi, string fichaTecnica, string nombre, int estado)
        {
            var lista = FactorySic.GetPrConceptoRepository().ListarConceptosxFiltro(catecodi, nombre, estado);

            List<int> categoriasValidas = ListarCategoriaGrupo().Select(x => x.Catecodi).ToList();

            lista = lista.Where(x => categoriasValidas.Contains(x.Catecodi)).ToList();

            //Obtiene lista de todos los Item de las ficha maestra
            List<FtFictecItemDTO> listaFicTemItems = new List<FtFictecItemDTO>();
            if (fichaTecnica == "S")
                listaFicTemItems = ObtenerItemFichaXtipoEquipoOficial(catecodi);

            foreach (var item in lista)
                this.FormatearConceptos(item, listaFicTemItems);

            //Orden de acuerdo al filtro de ficha técnica
            lista = fichaTecnica == "S" ? lista.Where(x => x.Concepfichatec == fichaTecnica).OrderBy(x => x.Conceporden).ToList() : lista.OrderBy(x => x.Concepcodi).ToList();

            return lista;
        }

        /// <summary>
        /// Dar Formato de conceptos 
        /// </summary>
        /// <param name="reg"></param>
        private void FormatearConceptos(PrConceptoDTO reg, List<FtFictecItemDTO> listaFicTemItems)
        {
            if (reg != null)
            {
                reg.Concepdesc = (reg.Concepdesc ?? "").Trim();
                reg.Concepabrev = (reg.Concepabrev ?? "").Trim();
                reg.Concepunid = (reg.Concepunid ?? "").Trim();
                reg.Catenomb = (reg.Catenomb ?? "").Trim();
                reg.Concepnombficha = (reg.Concepnombficha ?? "").Trim();
                reg.Conceptipo = (reg.Conceptipo ?? "").Trim();

                reg.Concepfichatec = (reg.Concepfichatec ?? "").Trim();
                reg.ConcepfichatecDesc = reg.Concepfichatec.ToUpper() == ConstantesDespacho.Si ? "SI" :
                    reg.Concepfichatec.ToUpper() == ConstantesDespacho.No ? "NO" : reg.Concepfichatec.ToUpper();

                reg.ConcepfeccreacionDesc = reg.Concepfeccreacion != null ? reg.Concepfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
                reg.ConcepfecmodificacionDesc = reg.Concepfecmodificacion != null ? reg.Concepfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;

                reg.UltimaModificacionFechaDesc = reg.Concepfecmodificacion != null ? reg.ConcepfecmodificacionDesc : reg.ConcepfeccreacionDesc;
                reg.UltimaModificacionUsuarioDesc = reg.Concepusumodificacion != null ? reg.Concepusumodificacion : reg.Concepusucreacion;

                reg.EstiloEstado = reg.Concepactivo == "0" ? ConstantesDespacho.EstiloBaja : "";

                if (listaFicTemItems != null && listaFicTemItems.Any())
                {
                    var fictecitem = listaFicTemItems.Find(x => x.Concepcodi == reg.Concepcodi);
                    reg.Conceporden = fictecitem != null ? fictecitem.OrdenNumerico : 999999999;
                }
            }
        }

        /// <summary>
        /// Generar Excel de reporte plantilla
        /// </summary>
        /// <param name="listaConceptos"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public void GenerarExcelConceptos(List<PrConceptoDTO> listaConceptos, string path, string fileName)
        {
            try
            {
                string file = path + fileName;

                FileInfo fi = new FileInfo(file);
                // Revisar si existe
                if (!fi.Exists)
                {                    
                    throw new Exception("No existe el archivo: " + file + ".");
                }

                using (ExcelPackage xlPackage = new ExcelPackage(fi))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets[ConstantesDespacho.HojaPlantillaExcel];

                    int row = 11;
                    int numeroItem = 1;

                    int columnNroItem = 1;
                    int columnCodPropiedad = columnNroItem + 1;
                    int columnNomb = columnCodPropiedad + 1;
                    int columnNombFicha = columnNomb + 1;
                    int columnAbrev = columnNombFicha + 1;
                    int columnDefinicion = columnAbrev + 1;
                    int columnUnidad = columnDefinicion + 1;
                    int columnTipoDato = columnUnidad + 1;
                    int columnLong1 = columnTipoDato + 1;
                    int columnLong2 = columnLong1 + 1;
                    int columnFichaTec = columnLong2 + 1;
                    int columnCodTipoEquipo = columnFichaTec + 1;
                    int columnLimInf = columnCodTipoEquipo + 1;
                    int columnLimSup = columnLimInf + 1;
                    int columnNombTipoEquipo = columnLimSup + 1;
                    int columnFechModif = columnNombTipoEquipo + 1;
                    int columnUsuModif = columnFechModif + 1;

                    foreach (var item in listaConceptos)
                    {
                        ws.Cells[row, columnNroItem].Value = numeroItem;
                        ws.Cells[row, columnCodPropiedad].Value = item.Concepcodi;
                        ws.Cells[row, columnNomb].Value = item.Concepdesc;
                        ws.Cells[row, columnNombFicha].Value = item.Concepnombficha;
                        ws.Cells[row, columnAbrev].Value = item.Concepabrev;
                        ws.Cells[row, columnDefinicion].Value = item.Concepdefinicion;
                        ws.Cells[row, columnUnidad].Value = item.Concepunid;
                        ws.Cells[row, columnTipoDato].Value = item.Conceptipo;
                        ws.Cells[row, columnLong1].Value = item.Conceptipolong1;
                        ws.Cells[row, columnLong2].Value = item.Conceptipolong2;
                        ws.Cells[row, columnFichaTec].Value = item.ConcepfichatecDesc;
                        ws.Cells[row, columnCodTipoEquipo].Value = item.Catecodi;
                        ws.Cells[row, columnLimInf].Value = item.Concepliminf;
                        ws.Cells[row, columnLimSup].Value = item.Conceplimsup;
                        ws.Cells[row, columnNombTipoEquipo].Value = item.Catenomb;
                        ws.Cells[row, columnFechModif].Value = item.UltimaModificacionFechaDesc;
                        ws.Cells[row, columnUsuModif].Value = item.UltimaModificacionUsuarioDesc;
                        row++;
                        numeroItem++;
                    }

                    xlPackage.Save();

                    //HOJA CATEGORÍA DE GRUPO
                    ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets[ConstantesDespacho.HojaCategoria];
                    int fil = 2;
                    int columnIniData = 1;
                    var listaCategorias = ListarCategoriaGrupo().OrderBy(x => x.Catenomb).ToList();

                    foreach (var item in listaCategorias)
                    {
                        ws2.Cells[fil, columnIniData++].Value = item.Catecodi;
                        ws2.Cells[fil, columnIniData++].Value = item.Catenomb;
                        ws2.Cells[fil, columnIniData++].Value = item.Cateabrev;
                        fil++;
                        columnIniData = 1;
                    }

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
        /// Metodo para la validacion de los datos a importar
        /// </summary>
        public void ValidarConceptosAImportarXLSX(string path, string fileName, string strUsuario,
                                               out List<PrConceptoDTO> lstRegConceptosCorrectos,
                                               out List<PrConceptoDTO> lstRegConceptosErroneos,
                                               out List<PrConceptoDTO> listaNuevo,
                                               out List<PrConceptoDTO> listaModificado)
        {
            //Validación de archivo
            string extension = (System.IO.Path.GetExtension(fileName) ?? "").ToLower();

            List<string> lExtensionPermitido = new List<string>() { ".xlsm", ".xlsx" }; // corregir
            if (!lExtensionPermitido.Contains(extension))
            {
                throw new ArgumentException("Está cargando un archivo de extensión no permitida. Debe ingresar un archivo " + string.Join(", ", lExtensionPermitido));
            }

            DateTime fechaRegistro = DateTime.Now;

            // Obtener lista de conceptos actuales
            List<PrConceptoDTO> conceptosActuales = ListarConceptoxFiltro(-2, "T", string.Empty, -1);

            //Listar categorias de la bd COES
            List<PrCategoriaDTO> listaCategorias = ListarCategoriaGrupo();

            #region Archivo xlsx

            string savePath = path + fileName;
            List<FilaExcelConcepto> listaFilaMacro = ImportToDataTable(savePath);

            //Validación de registros macro
            lstRegConceptosCorrectos = new List<PrConceptoDTO>();
            lstRegConceptosErroneos = new List<PrConceptoDTO>();

            
            List<PrConceptoDTO> listaAgregados = new List<PrConceptoDTO>(9);

            foreach (var regFila in listaFilaMacro)
            {
                
                //Validaciones al leer la macro (comprobar si los datos del excel son validos)
                string mensajeValidacion = this.ValidarLecturaExcel(regFila, listaCategorias);

                PrConceptoDTO entity = new PrConceptoDTO();
                entity.NroItem = regFila.NumItem;

                entity.Concepcodi = regFila.Concepcodi; // Código de concepto 
                entity.Concepdesc = regFila.Concepdesc; // Nombre
                entity.Concepnombficha = regFila.Concepnombficha;

                entity.Concepabrev = regFila.Concepabrev;
                entity.Concepdefinicion = regFila.Concepdefinicion;
                entity.Concepunid = regFila.Concepunid;
                entity.Conceptipo = regFila.Conceptipo.Trim().ToUpper();

                //Capturar valor correcto según tipo de dato
                if (entity.Conceptipo == "DECIMAL")
                {
                    entity.Conceptipolong1 = regFila.Conceptipolong1;
                    entity.Conceptipolong2 = regFila.Conceptipolong2;
                }
                else
                {
                    if (entity.Conceptipo == "ENTERO" || entity.Conceptipo == "CARACTER")
                    {
                        entity.Conceptipolong1 = regFila.Conceptipolong1;
                        entity.Conceptipolong2 = null;
                    }
                    else
                    {
                        entity.Conceptipolong1 = null;
                        entity.Conceptipolong2 = null;
                    }
                }
                //limites
                entity.Concepliminf = regFila.Concepliminf;
                entity.Conceplimsup = regFila.Conceplimsup;
                entity.StrConcepliminf = regFila.StrConcepliminf;
                entity.StrConceplimsup = regFila.StrConceplimsup;
                entity.StrConceptipolong1 = regFila.StrConceptipolong1;
                entity.StrConceptipolong2 = regFila.StrConceptipolong2;

                entity.Concepfichatec = regFila.Concepfichatec == "SI" ? ConstantesDespacho.Si : ConstantesDespacho.No;
                entity.Catecodi = regFila.Catecodi;
                entity.Catenomb = regFila.Catenomb;

                //nuevos registros
                entity.Concepactivo = ConstantesDespacho.EstadoActivo.ToString();
                entity.Concepusucreacion = strUsuario; // Usuario de creacion del registro
                entity.Concepfeccreacion = fechaRegistro; // Fecha de creacion del registro

                // Si los datos son correctos VALIDO LA INFORMACION DE ACUERDO AL NEGOCIO
                if (mensajeValidacion == "")
                {
                    //Valido que no exista duplicados (ConcepAbrev - Concepactivo)
                    PrConceptoDTO duplicado1 = listaAgregados.Find(x => x.Concepabrev.Trim() == entity.Concepabrev.Trim());
                    if (duplicado1 != null)
                    {
                        entity.Observaciones = "Existe Concepto ( ítem: "+ duplicado1.NroItem + ") con la misma abreviatura.";
                        lstRegConceptosErroneos.Add(entity);
                    }

                    //Validar duplicados dentro de la plantilla
                    var propRepetido = ObtenerRegistroPorCriterio(entity, lstRegConceptosCorrectos);
                    if (propRepetido != null)
                    {
                        entity.Observaciones = "No se puede crear conceptos duplicados. Comparar con item N°" + propRepetido.NroItem;
                        lstRegConceptosErroneos.Add(entity);
                    }
                    else
                    {
                        //Validar duplicado en (BD)
                        var dtoConceptoRee = ObtenerRegistroPorCriterio(entity, conceptosActuales);
                        bool existeRegistroEnBD = dtoConceptoRee != null;

                        if (existeRegistroEnBD && entity.Concepcodi == dtoConceptoRee.Concepcodi)
                        {
                            var existeActualizacionDeBD = ExisteModificacionConcepto(entity, dtoConceptoRee);

                            if (existeActualizacionDeBD && dtoConceptoRee.Concepactivo == ConstantesDespacho.EstadoActivo.ToString())// si hay cambios y es activo 
                            {
                                //capturar valores
                                dtoConceptoRee.NroItem = entity.NroItem;
                                dtoConceptoRee.Concepdesc = entity.Concepdesc ?? "";
                                dtoConceptoRee.Concepnombficha = entity.Concepnombficha ?? "";
                                dtoConceptoRee.Concepabrev = entity.Concepabrev ?? "";
                                dtoConceptoRee.Concepdefinicion = entity.Concepdefinicion ?? "";
                                dtoConceptoRee.Conceptipo = entity.Conceptipo ?? "";
                                dtoConceptoRee.Concepunid = entity.Concepunid ?? "";
                                dtoConceptoRee.Conceptipolong1 = entity.Conceptipolong1;
                                dtoConceptoRee.Conceptipolong2 = entity.Conceptipolong2;
                                dtoConceptoRee.Concepliminf = entity.Concepliminf;
                                dtoConceptoRee.Conceplimsup = entity.Conceplimsup;
                                dtoConceptoRee.Catecodi = entity.Catecodi;
                                dtoConceptoRee.Concepfichatec = entity.Concepfichatec ?? "";
                                dtoConceptoRee.Concepfecmodificacion = fechaRegistro;
                                dtoConceptoRee.Concepusumodificacion = strUsuario;

                                entity = dtoConceptoRee;//entidad para actualización
                                entity.ExisteCambio = existeActualizacionDeBD;// indicamos que tiene edición
                            }
                            else
                            {
                                listaAgregados.Add(entity);
                                continue;
                            }

                        }

                        if (existeRegistroEnBD && entity.Concepcodi == 0) // si existe duplicado y es nuevo
                        {
                            entity.Observaciones = "Se encontró coincidencia con registro existente. No se puede crear duplicados";
                            lstRegConceptosErroneos.Add(entity);
                        }
                        else
                        {
                            lstRegConceptosCorrectos.Add(entity); // Es nuevo
                        }
                    }
                }
                else
                {
                    // Van los registros con formato incorrecto
                    entity.ChkMensaje = true; // para separar con formato incorrecto
                    entity.Observaciones = mensajeValidacion;

                    lstRegConceptosErroneos.Add(entity);
                }

                //Validaciones de campos limites inferior y superior
                string mensajeValidacionLongitudYLimites = this.ValidarCamposLongitudYLimites(regFila);

                if (mensajeValidacionLongitudYLimites != "")
                {
                    // Van los registros con formato incorrecto
                    entity.ChkMensaje = true; // para separar con formato incorrecto
                    entity.Observaciones = mensajeValidacionLongitudYLimites;

                    lstRegConceptosErroneos.Add(entity);
                }

                listaAgregados.Add(entity);
            }

            #endregion

            listaNuevo = lstRegConceptosCorrectos.Where(x => x.Concepcodi == 0).ToList(); // solo los nuevos
            listaModificado = lstRegConceptosCorrectos.Where(x => x.Concepcodi > 0).ToList(); // solo los que tienen cambios
        }

        /// <summary>
        /// Valida los campos longitud y limite
        /// </summary>
        /// <param name="filaExcel"></param>
        /// <returns></returns>
        public string ValidarCamposLongitudYLimites(FilaExcelConcepto filaExcel)
        {
            string salida = "";
            List<string> lMsgValidacion = new List<string>();
            string tipoDato = filaExcel.Conceptipo;
            

            #region Validamos longitud 1 y 2
            string datoLong1 = filaExcel.StrConceptipolong1;
            string datoLong2 = filaExcel.StrConceptipolong2;
            if (tipoDato == "DECIMAL")
            {
                //Longitud 1                
                ValidarCampoLongitud(lMsgValidacion, datoLong1, "Longitud 1", 1, 15);

                //Longitud 2
                ValidarCampoLongitud(lMsgValidacion, datoLong2, "Longitud 2", 0, 10);
            }
            else
            {
                if (tipoDato == "ENTERO")
                {
                    //Longitud 1
                    ValidarCampoLongitud(lMsgValidacion, datoLong1, "Longitud 1", 1, 15);
                }
                else
                {
                    if (tipoDato == "CARACTER")
                    {
                        //Longitud 1
                        ValidarCampoLongitud(lMsgValidacion, datoLong1, "Longitud 1", 0, 9999);
                    }

                }
            }

            #endregion

            #region Validar campo Limites
            string datoLimiteInf = filaExcel.StrConcepliminf;
            string datoLimiteSup = filaExcel.StrConceplimsup;

            //Validacion para Limite Inferior
            ValidarCampoLimite(filaExcel, lMsgValidacion, tipoDato, datoLimiteInf, "Límite inferior");

            //Validacion para Limite Superior
            ValidarCampoLimite(filaExcel, lMsgValidacion, tipoDato, datoLimiteSup, "Límite superior");

            //Validacion para Limite superior e inferior            
            if (datoLimiteInf != null && datoLimiteInf != "")
            {
                if (datoLimiteSup != null && datoLimiteSup != "")
                {
                    if (EsDecimal(datoLimiteInf) && EsDecimal(datoLimiteSup))
                    {
                        decimal valorI = Decimal.Parse(datoLimiteInf);
                        decimal valorS = Decimal.Parse(datoLimiteSup);
                        if (valorI > valorS)
                        {
                            lMsgValidacion.Add("Límite superior debe ser mayor o igual al límite inferior");
                        }
                    }
                }
            }

            #endregion

            return string.Join(". ", lMsgValidacion);

        }

        /// <summary>
        /// Valida el campo limite del excel importado
        /// </summary>
        /// <param name="filaExcel"></param>
        /// <param name="lMsgValidacion"></param>
        /// <param name="tipoDato"></param>
        /// <param name="datoLimite"></param>
        /// <param name="nombreLimite"></param>
        private void ValidarCampoLimite(FilaExcelConcepto filaExcel, List<string> lMsgValidacion, string tipoDato, string datoLimite, string nombreLimite)
        {
            if (datoLimite != null && datoLimite != "")
            {
                if (EsDecimal(datoLimite))
                {
                    if (tipoDato == "DECIMAL")
                    {
                        bool hayParteEntera = false;
                        bool hayParteDecimal = false;
                        string parteEntera = "";
                        string parteDecimal = "";
                        string valorDato = datoLimite;

                        //verifico si la parte entera tiene data correcta
                        if (filaExcel.StrConceptipolong1 != null && filaExcel.StrConceptipolong1 != "")
                        {
                            if (EsEntero(filaExcel.StrConceptipolong1))
                            {
                                parteEntera = filaExcel.StrConceptipolong1;
                                hayParteEntera = true;
                            }
                        }
                        else
                        {
                            parteEntera = "15";//Dado que parteEntera y parteDecimal pueden ser nulos, en esos casos toman el maximo valor posible
                            hayParteEntera = true;
                        }

                        //verifico si la parte decimal tiene data correcta
                        if (filaExcel.StrConceptipolong2 != null && filaExcel.StrConceptipolong2 != "")
                        {
                            if (EsEntero(filaExcel.StrConceptipolong2))
                            {
                                parteDecimal = filaExcel.StrConceptipolong2;
                                hayParteDecimal = true;
                            }
                        }
                        else
                        {
                            parteDecimal = "10";//Dado que parteEntera y parteDecimal pueden ser nulos, en esos casos toman el maximo valor posible
                            hayParteDecimal = true;
                        }

                        //continuo solo si hay dato correcto para parte entera y decimal
                        if (hayParteEntera && hayParteDecimal)
                        {
                            if (EsEntero(parteEntera) && EsEntero(parteDecimal))
                            {
                                if (valorDato == null || valorDato == "")
                                {
                                }
                                else
                                {
                                    if (!EsDecimal(valorDato))
                                    {
                                        lMsgValidacion.Add(nombreLimite + ": Debe ser un campo numérico.");
                                        //validacion = validacion + "<li>Límite inferior: debe ser un campo numérico.</li>";//Campo Requerido
                                        //flag = false;
                                    }
                                    else
                                    {
                                        string strValDatoPositivo = valorDato.Replace("-", "");
                                        string strValDatoPositivoSinPunto = strValDatoPositivo.Replace(".", "");
                                        decimal valorDatoNumericoP = Decimal.Parse(strValDatoPositivo);
                                        int numCifrasTotales = strValDatoPositivoSinPunto.Length;
                                        decimal valParteEntera = Math.Truncate(valorDatoNumericoP);
                                        string strValParteEntera = valParteEntera + "";

                                        int numCifrasEntero = strValParteEntera.Length;
                                        int numCifrasDecimal = numCifrasTotales - numCifrasEntero;

                                        if (numCifrasEntero > Int32.Parse(parteEntera))
                                        {
                                            lMsgValidacion.Add(nombreLimite + ": La máxima cantidad de cifras enteras es " + parteEntera);
                                        }

                                        if (numCifrasDecimal > Int32.Parse(parteDecimal))
                                        {
                                            lMsgValidacion.Add(nombreLimite + ": La máxima cantidad de cifras decimales es " + parteDecimal);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (tipoDato == "ENTERO")
                        {
                            bool hayParteEntera = false;
                            string parteEntera = "";
                            string valorDato = datoLimite;

                            //verifico si la parte entera tiene data correcta
                            if (filaExcel.StrConceptipolong1 != null && filaExcel.StrConceptipolong1 != "")
                            {
                                if (EsEntero(filaExcel.StrConceptipolong1))
                                {
                                    parteEntera = filaExcel.StrConceptipolong1;
                                    hayParteEntera = true;
                                }
                            }
                            else
                            {
                                parteEntera = "15";//Dado que parteEntera y parteDecimal pueden ser nulos, en esos casos toman el maximo valor posible
                                hayParteEntera = true;
                            }

                            if (hayParteEntera)
                            {
                                if (EsEntero(parteEntera))
                                {
                                    if (valorDato == null || valorDato == "")
                                    {

                                    }
                                    else
                                    {
                                        if (!EsDecimal(valorDato))
                                        {
                                            lMsgValidacion.Add(nombreLimite + ": debe ser un campo numérico.");
                                        }
                                        else
                                        {
                                            string strValDatoPositivo = valorDato.Replace("-", "");
                                            string strValDatoPositivoSinPunto = strValDatoPositivo.Replace(".", "");
                                            decimal valorDatoNumericoP = Decimal.Parse(strValDatoPositivo);
                                            int numCifrasTotales = strValDatoPositivoSinPunto.Length;
                                            decimal valParteEntera = Math.Truncate(valorDatoNumericoP);
                                            string strValParteEntera = valParteEntera + "";

                                            int numCifrasEntero = strValParteEntera.Length;

                                            if (numCifrasTotales != numCifrasEntero)
                                            {
                                                lMsgValidacion.Add(nombreLimite + ": Debe ingresar una cifra entera correcta.");
                                            }

                                            if (numCifrasEntero > Int32.Parse(parteEntera))
                                            {
                                                lMsgValidacion.Add(nombreLimite + ": La máxima cantidad de cifras enteras es " + parteEntera);
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            if (tipoDato == "FORMULA")
                            {
                                //Para este caso toman el maximo valor posible
                                string parteEntera = "15";
                                string parteDecimal = "10";
                                string valorDato = datoLimite;


                                if ((parteEntera != "") && (parteDecimal != ""))
                                {
                                    if (EsEntero(parteEntera) && EsEntero(parteDecimal))
                                    {
                                        if (valorDato == null || valorDato == "")
                                        {
                                        }
                                        else
                                        {
                                            if (!EsDecimal(valorDato))
                                            {
                                                lMsgValidacion.Add(nombreLimite + ": Debe ser un campo numérico");
                                            }
                                            else
                                            {
                                                string strValDatoPositivo = valorDato.Replace("-", "");
                                                string strValDatoPositivoSinPunto = strValDatoPositivo.Replace(".", "");
                                                decimal valorDatoNumericoP = Decimal.Parse(strValDatoPositivo);
                                                int numCifrasTotales = strValDatoPositivoSinPunto.Length;
                                                decimal valParteEntera = Math.Truncate(valorDatoNumericoP);
                                                string strValParteEntera = valParteEntera + "";

                                                int numCifrasEntero = strValParteEntera.Length;
                                                int numCifrasDecimal = numCifrasTotales - numCifrasEntero;

                                                if (numCifrasEntero > Int32.Parse(parteEntera))
                                                {
                                                    lMsgValidacion.Add(nombreLimite + ": La máxima cantidad de cifras enteras es " + parteEntera);
                                                }

                                                if (numCifrasDecimal > Int32.Parse(parteDecimal))
                                                {
                                                    lMsgValidacion.Add(nombreLimite + ": La máxima cantidad de cifras decimales es " + parteDecimal);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    lMsgValidacion.Add(nombreLimite + ": Debe ser un campo numérico");
                }

            }
        }

        /// <summary>
        /// Validacion de los campos de Longitud
        /// </summary>
        /// <param name="lMsgValidacion"></param>
        /// <param name="datoLong"></param>
        /// <param name="nomLongitud"></param>
        /// <param name="rangoIni"></param>
        /// <param name="rangoFin"></param>
        public void ValidarCampoLongitud(List<string> lMsgValidacion, string datoLong, string nomLongitud, int rangoIni, int rangoFin)
        {
            if (datoLong == null || datoLong == "")
            {
            }
            else
            {
                if (!EsEntero(datoLong))
                {
                    lMsgValidacion.Add(nomLongitud + ": Debe ser un campo entero entre [" + rangoIni + "-" + rangoFin + "]");
                }
                else
                {
                    Int32.TryParse(datoLong, out int datoLongNumerico);
                    if (datoLongNumerico < rangoIni || datoLongNumerico > rangoFin)
                    {
                        lMsgValidacion.Add(nomLongitud + ": El rango permitido es entre [" + rangoIni + "-" + rangoFin + "]");
                    }
                }
            }
        }

        /// <summary>
        /// Devuelve objeto duplicado de una lista de conceptos
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="listaConceptos"></param>
        /// <returns></returns>
        public PrConceptoDTO ObtenerRegistroPorCriterio(PrConceptoDTO entity, List<PrConceptoDTO> listaConceptos)
        {
            PrConceptoDTO dtoConcepto = listaConceptos.Where(x => (x.Concepdesc.Trim().ToUpper() == entity.Concepdesc.Trim().ToUpper()
                               && x.Catecodi == entity.Catecodi)
                               || (x.Concepcodi > 0 && x.Concepcodi == entity.Concepcodi)).FirstOrDefault();

            return dtoConcepto;
        }

        /// <summary>
        /// ExisteModificacionConcepto
        /// </summary>
        /// <param name="regExcel"></param>
        /// <param name="regBD"></param>
        /// <returns></returns>
        private bool ExisteModificacionConcepto(PrConceptoDTO regExcel, PrConceptoDTO regBD)
        {
            if (regExcel.Concepdesc.Trim().ToUpper() != regBD.Concepdesc.Trim().ToUpper()) return true;
            if (regExcel.Concepnombficha.Trim().ToUpper() != regBD.Concepnombficha.Trim().ToUpper()) return true;
            if (regExcel.Concepabrev.Trim().ToUpper() != regBD.Concepabrev.Trim().ToUpper()) return true;
            if (regExcel.Conceptipo != regBD.Conceptipo) return true;
            if (regExcel.Concepfichatec.Trim().ToUpper() != regBD.Concepfichatec.Trim().ToUpper()) return true;
            if (regExcel.Catecodi != regBD.Catecodi) return true;

            //valido longitudes
            if (regExcel.Conceptipo == "DECIMAL")
            {
                if (regExcel.Conceptipolong1 != regBD.Conceptipolong1) return true;
                if (regExcel.Conceptipolong2 != regBD.Conceptipolong2) return true;

                //valido en caso el dato de long1 es un valor no entero
                if (regExcel.StrConceptipolong1.Trim() != "")
                {
                    if (regBD.Conceptipolong1 != null)
                    {
                        if (regExcel.StrConceptipolong1.Trim() != regBD.Conceptipolong1.ToString().Trim()) return true;
                    }
                    else
                        return true;
                }
                else
                {
                    if (regBD.Conceptipolong1 != null)
                    {
                        return true;
                    }

                }

                //valido en caso el dato de long2 es un valor no entero
                if (regExcel.StrConceptipolong2.Trim() != "")
                {
                    if (regBD.Conceptipolong2 != null)
                    {
                        if (regExcel.StrConceptipolong2.Trim() != regBD.Conceptipolong2.ToString().Trim()) return true;
                    }
                    else
                        return true;
                }
                else
                {
                    if (regBD.Conceptipolong2 != null)
                    {
                        return true;
                    }

                }
            }
            else
            {
                if (regExcel.Conceptipo == "ENTERO")
                {
                    if (regExcel.Conceptipolong1 != regBD.Conceptipolong1) return true;

                    //valido en caso el dato de long1 es un valor no entero
                    if (regExcel.StrConceptipolong1.Trim() != "")
                    {
                        if (regBD.Conceptipolong1 != null)
                        {
                            if (regExcel.StrConceptipolong1.Trim() != regBD.Conceptipolong1.ToString().Trim()) return true;
                        }
                        else
                            return true;
                    }
                    else
                    {
                        if (regBD.Conceptipolong1 != null)
                        {
                            return true;
                        }

                    }
                }
            }

            //valido limites
            if (regExcel.Conceptipo == "DECIMAL" || regExcel.Conceptipo == "ENTERO" || regExcel.Conceptipo == "FORMULA")
            {
                if (regExcel.Concepliminf != regBD.Concepliminf) return true;
                if (regExcel.Conceplimsup != regBD.Conceplimsup) return true;

                //valido en caso el dato de minInf es un valor no numerico
                if (regExcel.StrConcepliminf.Trim() != "")
                {
                    if (regBD.Concepliminf != null)
                    {
                        if (regExcel.StrConcepliminf.Trim() != regBD.Concepliminf.ToString().Trim()) return true;
                    }
                    else
                        return true;
                }
                else
                {
                    if (regBD.StrConcepliminf != null)
                    {
                        return true;
                    }

                }

                //valido en caso el dato de minSup es un valor no numerico
                if (regExcel.StrConceplimsup.Trim() != "")
                {
                    if (regBD.Conceplimsup != null)
                    {
                        if (regExcel.StrConceplimsup.Trim() != regBD.Conceplimsup.ToString().Trim()) return true;
                    }
                    else
                        return true;
                }
                else
                {
                    if (regBD.StrConceplimsup != null)
                    {
                        return true;
                    }

                }
            }

            return false;
        }

        /// <summary>
        /// Importa registros de un DataTable
        /// </summary>
        /// <param name="filePath">Directorio de archivos</param>  
        /// <returns>devuelve una cadena</returns>
        public static List<FilaExcelConcepto> ImportToDataTable(string filePath)
        {
            List<FilaExcelConcepto> listaMacro = new List<FilaExcelConcepto>();

            // Check if the file exists
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Exists)
            {
                throw new Exception("File " + filePath + " Does Not Exists");
            }

            int indexItem = 1;
            int indexCodConcepto = indexItem + 1;
            int indexNomb = indexCodConcepto + 1;
            int indexNombFicha = indexNomb + 1;
            int indexAbrev = indexNombFicha + 1;
            int indexDefinicion = indexAbrev + 1;
            int indexUnidad = indexDefinicion + 1;
            int indexTipoDato = indexUnidad + 1;
            int indexLong1 = indexTipoDato + 1; // 9
            int indexLong2 = indexLong1 + 1; //10
            int indexFichaTec = indexLong2 + 1;
            int indexCodCatGrupo = indexFichaTec + 1;
            int indexLimI = indexCodCatGrupo + 1; // 13
            int indexLimS = indexLimI + 1; //14
            int indexNombCatGrupo = indexLimS + 1;
            int indexFechModif = indexNombCatGrupo + 1;
            int indexUsuModif = indexFechModif + 1;

            using (ExcelPackage xlPackage = new ExcelPackage(fi))
            {
                // get the first worksheet in the workbook
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[ConstantesDespacho.HojaPlantillaExcel];
                //ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.First();

                // Fetch the WorkSheet size
                ExcelCellAddress startCell = worksheet.Dimension.Start;
                ExcelCellAddress endCell = worksheet.Dimension.End;

                int rowStart = 11;

                // place all the data into DataTable
                for (int row = rowStart; row <= endCell.Row; row++)
                {
                    var sItem = string.Empty;
                    if (worksheet.Cells[row, indexItem].Value != null) sItem = worksheet.Cells[row, indexItem].Value.ToString();
                    Int32.TryParse(sItem, out int numItem);

                    var sCodConcepto = string.Empty;
                    if (worksheet.Cells[row, indexCodConcepto].Value != null) sCodConcepto = worksheet.Cells[row, indexCodConcepto].Value.ToString();

                    var sNomb = string.Empty;
                    if (worksheet.Cells[row, indexNomb].Value != null) sNomb = worksheet.Cells[row, indexNomb].Value.ToString();

                    var sNombFicha = string.Empty;
                    if (worksheet.Cells[row, indexNombFicha].Value != null) sNombFicha = worksheet.Cells[row, indexNombFicha].Value.ToString();

                    var sAbrev = string.Empty;
                    if (worksheet.Cells[row, indexAbrev].Value != null) sAbrev = worksheet.Cells[row, indexAbrev].Value.ToString();

                    var sDefinicion = string.Empty;
                    if (worksheet.Cells[row, indexDefinicion].Value != null) sDefinicion = worksheet.Cells[row, indexDefinicion].Value.ToString();

                    var sUnidad = string.Empty;
                    if (worksheet.Cells[row, indexUnidad].Value != null) sUnidad = worksheet.Cells[row, indexUnidad].Value.ToString();

                    var sTipoDato = string.Empty;
                    if (worksheet.Cells[row, indexTipoDato].Value != null) sTipoDato = worksheet.Cells[row, indexTipoDato].Value.ToString();

                    var sLong1 = string.Empty;
                    if (worksheet.Cells[row, indexLong1].Value != null) sLong1 = worksheet.Cells[row, indexLong1].Value.ToString();

                    var sLong2 = string.Empty;
                    if (worksheet.Cells[row, indexLong2].Value != null) sLong2 = worksheet.Cells[row, indexLong2].Value.ToString();

                    var sLimI = string.Empty;
                    if (worksheet.Cells[row, indexLimI].Value != null) sLimI = worksheet.Cells[row, indexLimI].Value.ToString();

                    var sLimS = string.Empty;
                    if (worksheet.Cells[row, indexLimS].Value != null) sLimS = worksheet.Cells[row, indexLimS].Value.ToString();

                    var sFichaTec = string.Empty;
                    if (worksheet.Cells[row, indexFichaTec].Value != null) sFichaTec = worksheet.Cells[row, indexFichaTec].Value.ToString();

                    var sCodCatGrupo = string.Empty;
                    if (worksheet.Cells[row, indexCodCatGrupo].Value != null) sCodCatGrupo = worksheet.Cells[row, indexCodCatGrupo].Value.ToString();

                    var sNombCatGrupo = string.Empty;
                    if (worksheet.Cells[row, indexNombCatGrupo].Value != null) sNombCatGrupo = worksheet.Cells[row, indexNombCatGrupo].Value.ToString();

                    var sFechModif = string.Empty;
                    if (worksheet.Cells[row, indexFechModif].Value != null) sFechModif = worksheet.Cells[row, indexFechModif].Value.ToString();

                    var sUsuModif = string.Empty;
                    if (worksheet.Cells[row, indexUsuModif].Value != null) sUsuModif = worksheet.Cells[row, indexUsuModif].Value.ToString();

                    int concepcodi = 0;
                    int catecodi = 0;
                    int? longitud1 = null;
                    int? longitud2 = null;
                    decimal? limI = null;
                    decimal? limS = null;
                    try
                    {
                        sCodConcepto = (sCodConcepto ?? "").Trim();
                        sNomb = (sNomb ?? "").Trim();
                        sNombFicha = (sNombFicha ?? "").Trim();
                        sAbrev = (sAbrev ?? "").Trim();
                        sDefinicion = (sDefinicion ?? "").Trim();
                        sUnidad = (sUnidad ?? "").Trim();
                        sTipoDato = (sTipoDato ?? "").Trim().ToUpper();
                        sLong1 = (sLong1 ?? "").Trim();
                        sLong2 = (sLong2 ?? "").Trim();
                        sFichaTec = (sFichaTec ?? "").Trim();
                        sCodCatGrupo = (sCodCatGrupo ?? "").Trim();
                        sNombCatGrupo = (sNombCatGrupo ?? "").Trim();
                        sFechModif = (sFechModif ?? "").Trim();
                        sUsuModif = (sUsuModif ?? "").Trim();

                        concepcodi = (int)(((double?)worksheet.Cells[row, indexCodConcepto].Value) ?? 0);
                        catecodi = (int)(((double?)worksheet.Cells[row, indexCodCatGrupo].Value) ?? 0);

                        var valL1 = worksheet.Cells[row, indexLong1].Value;
                        var valL2 = worksheet.Cells[row, indexLong2].Value;
                        var valLI = worksheet.Cells[row, indexLimI].Value;
                        var valLS = worksheet.Cells[row, indexLimS].Value;
                        

                        longitud1 = valL1 != null ? (EsEntero(valL1.ToString()) ? ((int?)(((double?)worksheet.Cells[row, indexLong1].Value))) : null) : null;
                        longitud2 = valL2 != null ? (EsEntero(valL2.ToString()) ? ((int?)(((double?)worksheet.Cells[row, indexLong2].Value))) : null) : null;
                        limI = valLI != null ? ObtenerLimite(valLI.ToString()) : null;
                        limS = valLS != null ? ObtenerLimite(valLS.ToString()) : null;

                    }
                    catch (Exception ex)
                    {
                        //No es necesario registrar el error de la conversión de datos de todas las celdas (pueden ocurrir como un máximo de 28 mil líneas de log para archivos de 7000 líneas de datos). 
                        //El tratamiento de estos errores se realiza en otra función (ValidarLecturaMacro de DespachoAppServicio) 
                        //que luego genera un .csv para el usuario (funcion GenerarArchivoConceptosErroneasCSV de DespachoAppServicio)
                    }

                    if (string.IsNullOrEmpty(sNomb) && string.IsNullOrEmpty(sCodCatGrupo))
                    {
                        continue;
                    }

                    var regMantto = new FilaExcelConcepto()
                    {
                        Row = row,
                        NumItem = numItem,
                        Concepdesc = sNomb,
                        Concepnombficha = sNombFicha,
                        Concepabrev = sAbrev,
                        Concepdefinicion = sDefinicion,
                        Concepunid = sUnidad,
                        Conceptipo = sTipoDato,
                        StrConceptipolong1 = sLong1,
                        StrConceptipolong2 = sLong2,
                        Concepfichatec = sFichaTec.ToUpper(),
                        StrCatecodi = sCodCatGrupo,
                        Catenomb = sNombCatGrupo,
                        StrConcepfecmodificacion = sFechModif,
                        Concepusumodificacion = sUsuModif,

                        Concepcodi = concepcodi,
                        Catecodi = catecodi,
                        Conceptipolong1 = longitud1,
                        Conceptipolong2 = longitud2,
                        Concepliminf = limI,
                        Conceplimsup = limS,
                        StrConcepliminf = sLimI,
                        StrConceplimsup = sLimS
                    };

                    listaMacro.Add(regMantto);
                }
            }

            return listaMacro;
        }

        /// <summary>
        /// Devuelve el valor correspondiente a Limite
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static decimal? ObtenerLimite(string valor)
        {
            decimal? salida;

            //verifico si es un valor numerico o no
            bool flagSalida = decimal.TryParse(valor, out decimal numDecimal);

            if (flagSalida)
            {
                bool contieneComa = valor.Contains(",");
                bool contienePunto = valor.Contains(".");

                if(contieneComa)
                {
                    if (contienePunto)
                    {
                        salida = numDecimal;
                    }
                    else
                    {
                        valor = valor.Replace(',', '.');
                        bool flagSalida2 = decimal.TryParse(valor, out decimal numDecimal2);
                        salida = numDecimal2;
                    }
                }
                else
                {
                    if (contienePunto)
                    {
                        
                        salida = numDecimal;
                    }
                    else
                    {
                        salida = numDecimal;
                    }
                }
                
            }
            else
            {
                salida = null;
            }

            return salida;
        }

        /// <summary>
        /// Verifica si un string es entero
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool EsEntero(string num)
        {
            bool salida = false;

            salida = int.TryParse(num, out int numEntero);

            return salida;
        }

        /// <summary>
        /// Verifica si un string es decimal
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool EsDecimal(string num)
        {
            bool salida = false;

            salida = decimal.TryParse(num, out decimal numDecimal);

            return salida;
        }

        /// <summary>
        /// Validación de propiedades al leer del excel importado
        /// </summary>
        /// <param name="filaExcel"></param>
        /// <param name="listaCategorias"></param>
        /// <returns></returns>
        public string ValidarLecturaExcel(FilaExcelConcepto filaExcel, List<PrCategoriaDTO> listaCategorias)
        {
            string columnCodConcepto = "Código Concepto: ";
            string columnNomb = "Nombre: ";
            string columnNombFicha = "Nombre ficha técnica: ";
            string columnAbrev = "Abreviatura: ";
            string columnDefinicion = "Definición: ";
            string columnUnidad = "Unidad: ";
            string columnTipoDato = "Tipo de dato: ";
            string columnLong1 = "Longitud 1: ";
            string columnLong2 = "Longitud 2: ";
            string columnFichaTec = "Ficha técnica: ";
            string columnCodCatGrupo = "Código categoría: ";
            string columnNombCatGrupo = "Nombre categoría: ";

            List<string> lMsgValidacion = new List<string>();

            List<string> listadoTipo = new List<string>();
            listadoTipo.Add("DECIMAL");
            listadoTipo.Add("ENTERO");
            listadoTipo.Add("CARACTER");
            listadoTipo.Add("FECHA");
            listadoTipo.Add("AÑO");
            listadoTipo.Add("ARCHIVO");
            listadoTipo.Add("FORMULA");

            List<string> listadoFichaTecnica = new List<string>();
            listadoFichaTecnica.Add("SI");
            listadoFichaTecnica.Add("NO");

            // Validar Nombre propiedad
            if (String.IsNullOrEmpty(filaExcel.Concepdesc))
            {
                lMsgValidacion.Add(columnNomb + "Esta vacío o en blanco");
            }
            else
            {
                if (filaExcel.Concepdesc.Contains("\n"))
                    lMsgValidacion.Add(columnNomb + "Tiene salto de línea");
            }


            if (filaExcel.Concepfichatec == "SI")
            {
                // Validar Nombre Ficha técnica
                if (String.IsNullOrEmpty(filaExcel.Concepnombficha))
                {
                    lMsgValidacion.Add(columnNombFicha + "Esta vacío o en blanco");
                }
                else
                {
                    if (filaExcel.Concepnombficha.Contains("\n"))
                        lMsgValidacion.Add(columnNombFicha + "Tiene salto de línea");
                }
            }

            if (String.IsNullOrEmpty(filaExcel.Concepabrev))
            {
                lMsgValidacion.Add(columnAbrev + "Esta vacío o en blanco");
            }

            // tipo de dato
            if (String.IsNullOrEmpty(filaExcel.Conceptipo))
            {
                lMsgValidacion.Add(columnTipoDato + "Esta vacio o en blanco");
            }
            else
            {
                if (!listadoTipo.Contains(filaExcel.Conceptipo.Trim().ToUpper()))
                {
                    lMsgValidacion.Add(columnTipoDato + "El tipo de dato no es valido");
                }
            }

            //Longitud 1
            if (filaExcel.Conceptipolong1 != null)
            {
                if (filaExcel.Conceptipolong1.ToString().Length > 4)
                {
                    lMsgValidacion.Add(columnLong1 + "Supera 4 dígitos");
                }
            }

            //Longitud 2
            if (filaExcel.Conceptipolong2 != null)
            {
                if (filaExcel.Conceptipolong2.ToString().Length > 3)
                {
                    lMsgValidacion.Add(columnLong2 + "Supera 3 dígitos");
                }
            }


            if (String.IsNullOrEmpty(filaExcel.Concepfichatec))
            {
                lMsgValidacion.Add(columnFichaTec + "Esta vacío o en blanco");
            }
            else
            {
                if (!listadoFichaTecnica.Contains(filaExcel.Concepfichatec))
                {
                    lMsgValidacion.Add(columnFichaTec + "Valor no valido");
                }
            }

            if (String.IsNullOrEmpty(filaExcel.StrCatecodi))
            {
                lMsgValidacion.Add(columnCodCatGrupo + "Esta vacío o en blanco");
            }
            else if (filaExcel.Catecodi < 0)
            {
                lMsgValidacion.Add(columnCodCatGrupo + "No es número válido");
            }
            else
            {
                PrCategoriaDTO regCat = listaCategorias.Find(x => x.Catecodi == filaExcel.Catecodi);
                if (regCat == null)
                {
                    lMsgValidacion.Add(columnCodCatGrupo + "Código de categoría grupo no existe");
                }
                //else
                //{
                //    if (regCat.Famestado == ConstantesAppServicio.Baja)
                //    {
                //        lMsgValidacion.Add(columnCodCatGrupo + "El tipo equipo no se encuentra activo");
                //    }
                //}
            }

            return string.Join(". ", lMsgValidacion);
        }

        /// <summary>
        /// Genera log de intervenciones erroneoas
        /// </summary>
        /// <param name="path"></param>
        /// <param name="lstRegPropiedadesErroneos"></param>
        /// <returns></returns>
        public string GenerarArchivoConceptosErroneasCSV(string path, List<PrConceptoDTO> lstRegPropiedadesErroneos)
        {
            string sLine = string.Empty;
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string fileNameCsv = "";
            fileNameCsv = sFecha + "_LogPropiedadesImport" + ".csv";

            using (var dbProviderWriter = new StreamWriter(new FileStream(path + fileNameCsv, FileMode.OpenOrCreate), Encoding.UTF8))
            {
                sLine = "ÍTEM;OBSERVACIONES;CÓDIGO DE CONCEPTO;NOMBRE;NOMBRE DE FICHA TÉCNICA;ABREVIATURA;DEFINICIÓN;UNIDAD;TIPO DE DATO;LONGITUD 1;LONGITUD 2;FICHA TÉCNICA;CÓDIGO DE CATEGORÍA DE GRUPO;NOMBRE DE CATEGORÍA DE GRUPO;FECHA ÚLTIMA MODIFICACIÓN;USUARIO ÚLTIMA MODIFICACIÓN";
                dbProviderWriter.WriteLine(sLine);
                foreach (PrConceptoDTO entity in lstRegPropiedadesErroneos)
                {
                    sLine = this.CreateFilaErroneaConceptoString(entity);
                    dbProviderWriter.WriteLine(sLine);
                }

                dbProviderWriter.Close();
            }
            return fileNameCsv;
        }

        /// <summary>
        /// Guarda y actualiza propiedades masivamente
        /// </summary>
        /// <param name="listaNuevo"></param>
        /// <param name="listaModificado"></param>
        /// <param name="usuario"></param>
        public void GuardarDatosMasivosConceptos(List<PrConceptoDTO> listaNuevo, List<PrConceptoDTO> listaModificado, string usuario)
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
                                item.Concepusucreacion = usuario;
                                item.Concepfeccreacion = DateTime.Now;
                                item.Concepactivo = ConstantesDespacho.EstadoActivo.ToString();

                                var idPropiedad = FactorySic.GetPrConceptoRepository().Save(item, connection, transaction);
                            }
                        }

                        //Actualiza Registros masivamente
                        if (listaModificado != null && listaModificado.Any())
                        {
                            foreach (var item in listaModificado)
                            {
                                item.Concepusumodificacion = usuario;
                                item.Concepfecmodificacion = DateTime.Now;

                                FactorySic.GetPrConceptoRepository().Update(item, connection, transaction);
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

        #region UtilConceptos

        /// <summary>
        /// Metodo que escribe una fila del archivo .CSV de Conceptos Erroneas
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string CreateFilaErroneaConceptoString(PrConceptoDTO entity)
        {
            string sLine = string.Empty;

            sLine += entity.NroItem.ToString() + ConstantesDespacho.SeparadorCampoCSV;
            sLine += ((entity.Observaciones != null) ? entity.Observaciones.ToString().Replace(',', ';') : "") + ConstantesDespacho.SeparadorCampoCSV;

            sLine += ((entity.Concepcodi > 0) ? entity.Concepcodi.ToString() : "") + ConstantesDespacho.SeparadorCampoCSV;
            sLine += ((entity.Concepdesc != null) ? entity.Concepdesc.ToString() : "") + ConstantesDespacho.SeparadorCampoCSV;
            sLine += ((entity.Concepnombficha != null) ? entity.Concepnombficha.ToString() : "") + ConstantesDespacho.SeparadorCampoCSV;
            sLine += ((entity.Concepabrev != null) ? entity.Concepabrev.ToString() : "") + ConstantesDespacho.SeparadorCampoCSV;
            sLine += ((entity.Concepdefinicion != null) ? entity.Concepdefinicion.ToString() : "") + ConstantesDespacho.SeparadorCampoCSV;
            sLine += ((entity.Concepunid != null) ? entity.Concepunid.ToString() : "") + ConstantesDespacho.SeparadorCampoCSV;
            sLine += ((entity.Conceptipo != null) ? entity.Conceptipo.ToString() : "") + ConstantesDespacho.SeparadorCampoCSV;
            sLine += ((entity.Conceptipolong1 != null) ? entity.Conceptipolong1.ToString() : "") + ConstantesDespacho.SeparadorCampoCSV;
            sLine += ((entity.Conceptipolong2 != null) ? entity.Conceptipolong2.ToString() : "") + ConstantesDespacho.SeparadorCampoCSV;
            sLine += ((entity.Concepfichatec != null) ? entity.Concepfichatec.ToString() : "") + ConstantesDespacho.SeparadorCampoCSV;
            sLine += ((entity.Catecodi >= 0) ? entity.Catecodi.ToString() : "") + ConstantesDespacho.SeparadorCampoCSV;
            sLine += ((entity.Catenomb != null) ? entity.Catenomb.ToString() : "") + ConstantesDespacho.SeparadorCampoCSV;
            sLine += ((entity.Concepfecmodificacion != null) ? entity.Concepfecmodificacion.ToString() : "") + ConstantesDespacho.SeparadorCampoCSV;
            sLine += ((entity.Concepusumodificacion != null) ? entity.Concepusumodificacion.ToString() : "") + ConstantesDespacho.SeparadorCampoCSV;

            return sLine;
        }

        /// <summary>
        /// Eliminar archivos que están en la carpeta reporte Cada vez que se ingrese al módulo de Conceptos
        /// </summary>
        public void EliminarArchivosReporte()
        {
            try
            {
                string pathAlternativo = AppDomain.CurrentDomain.BaseDirectory + ConstantesDespacho.RutaReportes;
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

        /// <summary>
        /// Obtener listado de items de la ficha tecnica maestra por tipo
        /// </summary>
        /// <param name="catecodi"></param>
        /// <returns></returns>
        public List<FtFictecItemDTO> ObtenerItemFichaXtipoEquipoOficial(int catecodi)
        {
            var listaFichasAll = FactorySic.GetFtFictecXTipoEquipoRepository().GetByCriteria("A");
            var fichaPrincipal = servFictec.GetFichaMaestraPrincipal(ConstantesDespacho.FichaMaestraPortal);
            List<FtFictecItemDTO> listaItemGlobal = new List<FtFictecItemDTO>();
            if (fichaPrincipal != null)
            {
                var listaFictecdetXfm = servFictec.ListarAllFichaTecnicaByMaestra(fichaPrincipal.Fteccodi);
                List<int> listaCodifichas = listaFictecdetXfm.Select(x => x.Fteqcodi).ToList();

                var fichasfilter = listaFichasAll.Where(x => listaCodifichas.Contains(x.Fteqcodi)).ToList();

                if (catecodi == 4) // caso Central térmica
                {
                    fichasfilter = fichasfilter.Where(x => x.Famcodi == 5).ToList(); // Central Térmoeléctrica
                }
                else
                    fichasfilter = fichasfilter.Where(x => x.Catecodi == catecodi).ToList();

                FtFictecXTipoEquipoDTO fichaxtipoequipo = fichasfilter.Any() ? fichasfilter.First() : null;

                if (fichaxtipoequipo != null)
                {
                    List<FtFictecItemDTO> listaItems, listaAllItems;
                    List<TreeItemFichaTecnica> listaItemsJson;

                    var servFT = new FichaTecnicaAppServicio();
                    FTFiltroReporteExcel objFiltro = servFT.GetFichaYDatosXEquipoOModo(fichaxtipoequipo.Fteqcodi, -2, false, ConstantesFichaTecnica.INTRANET, DateTime.Today);
                    servFT.ListarTreeItemsFichaTecnica(objFiltro, out listaAllItems, out listaItems, out listaItemsJson);

                    listaItemGlobal = listaAllItems;
                }
            }

            return listaItemGlobal;
        }

        /// <summary>
        /// Método recuursivo para ordenamiento
        /// </summary>
        /// <param name="listaFicTemItem"></param>
        /// <param name="listaHijos"></param>
        /// <param name="orden"></param>
        public void OrdenarItemsFT(List<FtFictecItemDTO> listaFicTemItem, List<FtFictecItemDTO> listaHijos, ref int orden)
        {
            foreach (var reg in listaHijos)
            {
                orden = orden + 1;
                reg.Ftitorden = orden;
                var hijos = listaFicTemItem.Where(x => x.Ftitpadre == reg.Ftitcodi).OrderBy(x => x.Ftitorden).ToList();

                if (hijos.Any())
                {
                    this.OrdenarItemsFT(listaFicTemItem, hijos, ref orden);
                }
            }
        }

        #endregion

        #endregion

    }
}
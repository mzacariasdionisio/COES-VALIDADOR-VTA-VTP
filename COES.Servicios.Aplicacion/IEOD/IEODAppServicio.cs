using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Titularidad;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace COES.Servicios.Aplicacion.IEOD
{
    public class IEODAppServicio : AppServicioBase
    {
        EmpresaAppServicio servEmpresa = new EmpresaAppServicio();
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(IEODAppServicio));

        #region Métodos Tabla SI_EMPRESA

        /// <summary>
        /// Permite listar las empresas del SEIN
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresas()
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasSEIN();
        }

        /// <summary>
        /// Permite listar las empresas que tiene tienen centrales de generación y filtrarlo por tipo de empresa
        /// </summary>
        /// <param name="tiposEmpresa"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasTienenCentralGenxTipoEmpresa(string tiposEmpresa)
        {
            return FactorySic.GetSiEmpresaRepository().ListarEmpresasxTipoEquipos(ConstantesHorasOperacion.CodFamilias, tiposEmpresa);
        }
        public List<SiEmpresaDTO> ListarEmpresasRsfBorne(string tiposEmpresa)
        {
            return FactorySic.GetSiEmpresaRepository().ListarEmpresasxTipoEquipos(ConstantesHorasOperacion.CodFamiliasRSF, tiposEmpresa);
        }

        /// <summary>
        /// Permite listar las empresas
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasxTipoEquipos(string tipoEquipos)
        {
            return FactorySic.GetSiEmpresaRepository().ListarEmpresasxTipoEquipos(tipoEquipos, ConstantesAppServicio.ParametroDefecto);
        }

        public List<SiEmpresaDTO> ListarEmpresasxTipoEquipos2(string tipoEquipos, string tipoEmpresa)
        {
            return FactorySic.GetSiEmpresaRepository().ListarEmpresasxTipoEquipos(tipoEquipos, tipoEmpresa);
        }

        /// <summary>
        /// Devuelve lista de empresa por tipo de empresa
        /// </summary>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> GetListaCriteria(string strTipoempresa)
        {
            return FactorySic.GetSiEmpresaRepository().GetByCriteria(strTipoempresa);

        }

        /// <summary>
        /// Obtiene registro de empresa
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public SiEmpresaDTO GetByIdEmpresa(int emprcodi)
        {
            return FactorySic.GetSiEmpresaRepository().GetById(emprcodi);
        }

        /// <summary>
        /// Permite listar empresas por tipo de negocio (Distribuidoas, Generadoras .. etc)
        /// </summary>
        /// <param name="tiposEmpresa"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasXID(string idsEmpresas)
        {
            return FactorySic.GetSiEmpresaRepository().ListarEmpresasXID(idsEmpresas);
        }

        /// <summary>
        /// Listar empresas por Fuente de datos
        /// </summary>
        /// <param name="fdatcodi"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasXFdatcodi(int fdatcodi)
        {
            List<SiEmpresaDTO> l = new List<SiEmpresaDTO>();

            int fdatpadre = this.GetByIdSiFuentedatos(fdatcodi).Fdatpadre.GetValueOrDefault(0);

            switch (fdatpadre)
            {
                case ConstantesIEOD.FdatcodiPadreHOP:
                    HorasOperacionAppServicio servHO = (new HorasOperacionAppServicio());

                    if (ConstantesIEOD.FdatcodiHOPTermoelectricaBiogasBagazo != fdatcodi)
                    {
                        int famcodi = servHO.ListarAllFuenteDatosXFamilia().Find(x => x.Fdatcodi == fdatcodi).Famcodi;
                        l = servHO.ListarEmpresasHorasOperacionByTipoCentral(true, null, famcodi.ToString());
                    }
                    else
                    {
                        l = servHO.ListarEmpresaXFenergcodi(ConstantesPR5ReportesServicio.FenergcodiBiogas + "," + ConstantesPR5ReportesServicio.FenergcodiBagazo);
                    }
                    break;
                case ConstantesDesconexion.IdFatcodiDesconex:
                    l = this.GetListaCriteria(ConstantesDesconexion.TipoEmpresa);
                    break;
                case ConstantesRestriccionesOper.IdFatcodiRestricc:
                    l = this.GetListaCriteria(ConstantesRestriccionesOper.TipoEmpresa);
                    break;
            }

            return l;
        }

        public int getEmpresaMigracion(int empr, string fecha)
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresaMigra(empr, fecha);
        }

        #endregion

        #region Métodos Tabla EQ_EQUIPO

        /// <summary>
        /// Devuelve la lista de equipos filtrados por empresa y por tipo de central de generacion
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="iCodFamilias"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarCentralesXEmpresaGener(string idEmpresa, string codFamilias)
        {
            return FactorySic.GetEqEquipoRepository().ListarCentralesXEmpresaXFamiliaGEN(idEmpresa, codFamilias);
        }

        /// <summary>
        /// Devuelve la lista de equipos filtrados por empresa y por tipo de central de generacion 2
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="codFamilias"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarCentralesXEmpresaGener2(string empresa, string codFamilias)
        {
            return FactorySic.GetEqEquipoRepository().ListarCentralesXEmpresaXFamiliaGEN2(empresa, codFamilias, ConstantesAppServicio.Activo);
        }

        /// <summary>
        /// Obtiene registro  de equipo  por Id
        /// </summary>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        public EqEquipoDTO GetEquipo(int idCentral)
        {
            return FactorySic.GetEqEquipoRepository().GetById(idCentral);
        }

        /// <summary>
        /// Permite obtener los equipos por familia
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ObtenerEquiposPorFamilia(int emprcodi, int famcodi)
        {
            SiEmpresaDTO empresa = FactorySic.GetSiEmpresaRepository().GetById(emprcodi);
            List<EqEquipoDTO> list = FactorySic.GetEqEquipoRepository().ObtenerEquipoPorFamilia(emprcodi, famcodi).Where(x => x.Equicodi > 0).ToList();

            if (list.Count > 0)
            {
                int max = list.Select(x => x.AREANOMB.Length).Max();

                foreach (EqEquipoDTO item in list)
                {
                    int count = max - item.AREANOMB.Length;
                    string espacio = string.Empty;
                    for (int i = 0; i <= count; i++)
                    {
                        espacio = espacio + "-";
                    }


                    item.Equinomb = item.AREANOMB + espacio + " " + item.Equinomb;
                    item.EMPRNOMB = empresa.Emprnomb;
                    item.Emprnomb = empresa.Emprnomb;
                }

                return list.OrderBy(x => x.Equinomb).ToList();
            }

            return new List<EqEquipoDTO>();
        }


        /// <summary>
        /// Permite obtener las substaciones por empresa y tipo de equipo
        /// </summary>
        /// <returns></returns>
        public List<EqAreaDTO> ObtenerAreasXEmpresa(int codemp)
        {
            return FactorySic.GetEqAreaRepository().ObtenerAreaPorEmpresa(codemp);
        }

        /// <summary>
        /// Lista de equipos que tienen topologia
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="familias"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarTopologiaEquiposPadres(string empresas, string familias, int tiporelcodiTopologia)
        {
            List<EqEquipoDTO> l = FactorySic.GetEqEquipoRepository().ListarTopologiaEquipoPadres(empresas, familias, tiporelcodiTopologia);
            foreach (var reg in l)
            {
                reg.EquirelfecmodificacionDesc = reg.Equirelfecmodificacion != null ? reg.Equirelfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
            }

            return l;
        }

        /// <summary>
        /// Retorna la lista de equipos y las relaciones con sus empresas( un equipo puede varias empresas segun el rango de consulta)
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="famcodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="listaEquicodiAdicional"></param>
        /// <param name="listaEmpresas">Lista de </param>
        /// <param name="listaEquipo"></param>
        /// <returns></returns>
        public void ListarEquiposVigentes(DateTime fechaIni, DateTime fechaFin, string famcodi, string centralIntregrante, string emprcodi, string equipadres, List<int> listaEquicodiAdicional
            , out List<SiEmpresaDTO> listaEmpresas, out List<EqEquipoDTO> listaEquipo, out List<EqEquipoDTO> listaEquipoTTIE)
        {
            List<EqEquipoDTO> lista = new List<EqEquipoDTO>();

            //
            List<EqEquipoDTO> listaEqAll = FactorySic.GetEqEquipoRepository().ListarEquiposTTIE(famcodi).Where(x => x.Heqdatfecha <= fechaFin).OrderByDescending(x => x.Heqdatfecha).ThenByDescending(X => X.Heqdatestado).ToList();
            listaEqAll = listaEqAll.Where(x => x.Grupocodi > 0).ToList(); //Considerar a los equipos que tienen grupo definido

            //Obtener los equipos que han estado activo hasta la fecha de inicio
            var agrupByEquicodiActualAntes = listaEqAll.Where(x => x.Heqdatfecha <= fechaFin).GroupBy(x => x.Equicodiactual).Select(x => x.First()).ToList();
            List<EqEquipoDTO> listaEqAntes = agrupByEquicodiActualAntes.Where(x => x.Heqdatestado == ConstantesTitularidad.EstadoRelEmpFechaInicio).ToList();

            //Obtener los equipos que se han creado durante el rango
            var agrupByEquicodiActualDurante = listaEqAll.Where(x => fechaIni < x.Heqdatfecha && x.Heqdatfecha <= fechaFin).GroupBy(x => x.Equicodiactual).Select(x => x.First()).ToList();
            List<EqEquipoDTO> listaEqDurante = agrupByEquicodiActualDurante.Where(x => x.Heqdatestado == ConstantesTitularidad.EstadoRelEmpFechaInicio).ToList();

            listaEquipoTTIE = new List<EqEquipoDTO>();
            listaEquipoTTIE.AddRange(listaEqAntes);
            listaEquipoTTIE.AddRange(listaEqDurante);
            foreach (var regEq in listaEquipoTTIE)
            {
                regEq.Emprnomb = regEq.Emprnomb != null ? regEq.Emprnomb.Trim() : string.Empty;
                regEq.Equinomb = regEq.Equinomb != null ? regEq.Equinomb.Trim() : string.Empty;
                regEq.Equiabrev = regEq.Equiabrev != null ? regEq.Equiabrev.Trim() : string.Empty;
                regEq.Gruponomb = regEq.Gruponomb != null ? regEq.Gruponomb.Trim() : string.Empty;
                regEq.Grupoabrev = regEq.Grupoabrev != null ? regEq.Grupoabrev.Trim() : string.Empty;
                regEq.Central = regEq.Central != null ? regEq.Central.Trim() : string.Empty;
            }

            //Salidas
            if (centralIntregrante != ConstantesAppServicio.ParametroDefecto)
            {
                //Coes, no Coes
                List<PrGrupodatDTO> listaOperacionCoes = FactorySic.GetPrGrupodatRepository().ListarHistoricoValores(ConstantesPR5ReportesServicio.PropGrupoOperacionCoes.ToString(), -1)
                                                            .Where(x => x.Deleted == 0).OrderByDescending(x => x.Fechadat).ToList();

                foreach (var regEq in listaEquipoTTIE)
                {
                    var regDat = listaOperacionCoes.Find(x => x.Fechadat <= regEq.Heqdatfecha && x.Grupocodi == regEq.Grupocodi);
                    regEq.Grupointegrante = regDat != null ? regDat.Formuladat : ConstantesAppServicio.SI;
                }

                if (centralIntregrante == ConstantesAppServicio.SI)
                    listaEquipoTTIE = listaEquipoTTIE.Where(x => x.Grupointegrante == ConstantesAppServicio.SI).ToList();
                if (centralIntregrante == ConstantesAppServicio.NO)
                    listaEquipoTTIE = listaEquipoTTIE.Where(x => x.Grupointegrante != ConstantesAppServicio.SI).ToList();
            }

            listaEmpresas = listaEquipoTTIE.GroupBy(x => x.Emprcodi).Select(x => new SiEmpresaDTO() { Emprcodi = x.Key.Value, Emprnomb = x.First().Emprnomb }).OrderBy(x => x.Emprnomb).ToList();
            if (!string.IsNullOrEmpty(emprcodi) && emprcodi != ConstantesAppServicio.ParametroDefecto)
            {
                List<int> empresas = emprcodi.Split(',').Select(x => int.Parse(x)).ToList();
                listaEmpresas = listaEmpresas.Where(x => empresas.Contains(x.Emprcodi)).ToList();
                listaEquipoTTIE = listaEquipoTTIE.Where(x => empresas.Contains(x.Emprcodi ?? 0)).ToList();
            }

            if (!string.IsNullOrEmpty(equipadres) && equipadres != ConstantesAppServicio.ParametroDefecto)
            {
                List<int> listaCentral = equipadres.Split(',').Select(x => int.Parse(x)).ToList();
                listaEquipoTTIE = listaEquipoTTIE.Where(x => listaCentral.Contains(x.Equipadre ?? 0)).ToList();
            }

            listaEquipo = listaEquipoTTIE.OrderByDescending(x => x.Heqdatfecha).GroupBy(x => x.Equicodiactual).Select(x => new EqEquipoDTO()
            {
                Emprcodi = x.First().Emprcodi,
                Emprnomb = x.First().Emprnomb,
                Equicodi = x.First().Equicodi,
                Equinomb = x.First().Equinomb,
                Famcodi = x.First().Famcodi,
                Equiabrev = x.First().Equiabrev,
                Equipadre = x.First().Equipadre,
                Central = x.First().Central,
                Grupocodi = x.First().Grupocodi,
                Gruponomb = x.First().Equiabrev,
                Grupoabrev = x.First().Equiabrev,
                Tgenercodi = x.First().Tgenercodi
            }).OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Equinomb).ToList();
        }

        #endregion

        #region Métodos tabla EQ_FAMILIA
        /// <summary>
        /// Devuelve lista de familia
        /// </summary>
        /// <returns></returns>
        public List<EqFamiliaDTO> ListarFamilia()
        {
            return FactorySic.GetEqFamiliaRepository().List().OrderBy(x => x.Famnomb).ToList();
        }

        /// <summary>
        /// Devuelve lista de familia xEmpresa
        /// </summary>
        /// <returns></returns>
        public List<EqFamiliaDTO> ListarFamiliaXEmp(int idEmpresa)
        {
            return FactorySic.GetEqFamiliaRepository().ListarFamiliaXEmp(idEmpresa).Where(x => x.Famcodi > 0).ToList();
        }
        #endregion

        #region Métodos tabla FW_AREA

        /// <summary>
        /// Permite listar todos los registros de la tabla FW_AREA
        /// </summary>
        public List<FwAreaDTO> ListFwAreas()
        {
            return FactorySic.GetFwAreaRepository().List();
        }
        #endregion

        #region Métodos Tabla EQ_EQUIREL

        /// <summary>
        /// Obtiene registro de equirel
        /// </summary>
        /// <param name="idPadre"></param>
        /// <param name="tiporelCodi"></param>
        /// <param name="idEquiTopologia"></param>
        /// <returns></returns>
        public EqEquirelDTO GetByEquipoTopologia(int idPadre, int tiporelCodi, int idEquiTopologia)
        {
            return FactorySic.GetEqEquirelRepository().GetById(idPadre, tiporelCodi, idEquiTopologia);
        }

        /// <summary>
        /// Obtiene lista de equipos dependientes del equipo padre
        /// </summary>
        /// <param name="idEquipoPadre"></param>
        /// <returns></returns>
        public List<EqEquirelDTO> ListarDetalleEquiTopologia(int idEquipoPadre, int tipoRelTopologia)
        {
            List<EqEquirelDTO> l = FactorySic.GetEqEquirelRepository().GetByCriteriaTopologia(idEquipoPadre, tipoRelTopologia);

            foreach (var reg in l)
            {
                reg.EquirelfecmodificacionDesc = reg.Equirelfecmodificacion != null ? reg.Equirelfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
            }

            return l;
        }

        /// <summary>
        /// Graba equipo relacionado
        /// </summary>
        /// <param name="equipo"></param>
        public void SaveEqEquiRelDTO(EqEquirelDTO equipo)
        {
            FactorySic.GetEqEquirelRepository().Save(equipo);
        }

        /// <summary>
        /// Elimina equipo relacionado
        /// </summary>
        /// <param name="equipo"></param>
        public void DeleteEqEquiRelDTO(int equicodi1, int tiporelcodi, int equicodi2, string username)
        {
            FactorySic.GetEqEquirelRepository().Delete(equicodi1, tiporelcodi, equicodi2);
            FactorySic.GetEqEquirelRepository().Delete_UpdateAuditoria(equicodi1, tiporelcodi, equicodi2, username);
        }
        #endregion

        #region Métodos de la tabla PRGRUPO

        /// <summary>
        /// Permite obtener un registro de la tabla PR_GRUPO 
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        public PrGrupoDTO GetByIdPrGrupo(int grupocodi)
        {
            return FactorySic.GetPrGrupoRepository().GetById(grupocodi);
        }

        #endregion

        #region METODOS DE LA TABLA EVE_SUBCAUSAEVENTO

        /// <summary>
        /// Permite listar los registros de la tabla EVE_SUBCAUSAEVENTO por codigo de causaevento y SUBCAUSACMG
        /// </summary>
        public List<EveSubcausaeventoDTO> ListarSubCausas(int causacodi, string subcausacmg)
        {
            return FactorySic.GetEveSubcausaeventoRepository().ObtenerXCausaXCmg(causacodi, subcausacmg);
        }

        /// <summary>
        /// Permite listar los registros de la tabla EVE_SUBCAUSAEVENTO por codigo de subcausaevento
        /// </summary>
        public List<EveSubcausaeventoDTO> ListarSubCausasXid(int subcausacodi)
        {
            return FactorySic.GetEveSubcausaeventoRepository().GetSubcausaEventoXId(subcausacodi);
        }
        #endregion

        #region METODOS DE LA TABLA SI_FUENTEDATOS

        /// <summary>
        /// Ibtienen ID de fuente de datos
        /// </summary>
        /// <param name="fdatcodi"></param>
        /// <returns></returns>
        public SiFuentedatosDTO GetByIdSiFuentedatos(int fdatcodi)
        {
            return FactorySic.GetSiFuentedatosRepository().GetById(fdatcodi);
        }

        /// <summary>
        /// Obtienen Lista de Fuente de datos por codigo de modulo
        /// </summary>
        /// <param name="fdatcodi"></param>
        /// <returns></returns>
        public List<SiFuentedatosDTO> GetByModuloSiFuentedatos(int ModCodi)
        {
            return FactorySic.GetSiFuentedatosRepository().GetByModulo(ModCodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_FUENTEDATOS
        /// </summary>
        public List<SiFuentedatosDTO> ListSiFuentedatos()
        {
            return FactorySic.GetSiFuentedatosRepository().List().Where(x => x.Fdatcodi > 0).ToList();
        }

        #endregion

        #region Métodos Tabla ME_ENVIO

        /// <summary>
        /// Inserta un registro de la tabla ME_ENVIO
        /// </summary>
        public int SaveMeEnvio(MeEnvioDTO entity)
        {
            try
            {
                return FactorySic.GetMeEnvioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_ENVIO
        /// </summary>
        public void UpdateMeEnvio(MeEnvioDTO entity)
        {
            try
            {
                FactorySic.GetMeEnvioRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Actualiza un registro de la tabla ME_ENVIO
        /// </summary>
        public void UpdateMeEnvio1(MeEnvioDTO entity)
        {
            try
            {
                FactorySic.GetMeEnvioRepository().Update1(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_ENVIO
        /// </summary>
        public MeEnvioDTO GetByIdMeEnvio(int idEnvio)
        {
            return FactorySic.GetMeEnvioRepository().GetById(idEnvio);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeEnvio
        /// </summary>
        public List<MeEnvioDTO> GetByCriteriaMeEnvios(int idEmpresa, int idFormato, DateTime fecha)
        {
            return FactorySic.GetMeEnvioRepository().GetByCriteria(idEmpresa, idFormato, fecha);
        }


        /// <summary>
        /// Lista de Envios por paginado
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsLectura"></param>
        /// <param name="idsFormato"></param>
        /// <param name="idsEstado"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nroPaginas"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> GetListaMultipleMeEnvios(string idsEmpresa, string idsLectura, string idsFormato, string idsEstado, DateTime fechaIni, DateTime fechaFin, int nroPaginas, int pageSize)
        {
            if (string.IsNullOrEmpty(idsEmpresa)) idsEmpresa = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsLectura)) idsLectura = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsFormato)) idsFormato = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsEstado)) idsEstado = ConstantesAppServicio.ParametroDefecto;
            return FactorySic.GetMeEnvioRepository().GetListaMultiple(idsEmpresa, idsLectura, idsFormato, idsEstado, fechaIni, fechaFin, nroPaginas, pageSize);
        }

        /// <summary>
        /// Lista de envios para consulta excel si paginado
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsLectura"></param>
        /// <param name="idsFormato"></param>
        /// <param name="idsEstado"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> GetListaMultipleMeEnviosXLS(string idsEmpresa, string idsLectura, string idsFormato, string idsEstado, DateTime fechaIni, DateTime fechaFin)
        {
            if (string.IsNullOrEmpty(idsEmpresa)) idsEmpresa = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsLectura)) idsLectura = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsFormato)) idsFormato = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsEstado)) idsEstado = ConstantesAppServicio.ParametroDefecto;
            return FactorySic.GetMeEnvioRepository().GetListaMultipleXLS(idsEmpresa, idsLectura, idsFormato, idsEstado, fechaIni, fechaFin);
        }

        /// <summary>
        /// Devuelve el total de registros para listado de envios por paginado
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsLectura"></param>
        /// <param name="idsFormato"></param>
        /// <param name="idsEstado"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public int TotalListaMultipleMeEnvios(string idsEmpresa, string idsLectura, string idsFormato, string idsEstado, DateTime fechaIni, DateTime fechaFin)
        {
            if (string.IsNullOrEmpty(idsEmpresa)) idsEmpresa = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsLectura)) idsLectura = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsFormato)) idsFormato = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsEstado)) idsEstado = ConstantesAppServicio.ParametroDefecto;
            return FactorySic.GetMeEnvioRepository().TotalListaMultiple(idsEmpresa, idsLectura, idsFormato, idsEstado, fechaIni, fechaFin);
        }

        /// <summary>
        /// Obtiene el maximo id del envio de un formato de todos los periodos
        /// </summary>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public int ObtenerIdMaxEnvioFormato(int idFormato, int idEmpresa)
        {
            return FactorySic.GetMeEnvioRepository().GetMaxIdEnvioFormato(idFormato, idEmpresa);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeEnvio por rango de fechas
        /// </summary>
        public List<MeEnvioDTO> GetByCriteriaRangoFecha(int idEmpresa, int idFormato, DateTime fechaini, DateTime fechafin)
        {
            return FactorySic.GetMeEnvioRepository().GetByCriteriaRangoFecha(idEmpresa, idFormato, fechaini, fechafin);
        }

        /// <summary>
        /// Obtiene el maximo id del envio de un formato de todos los periodos
        /// </summary>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public int ObtenerIdMaxEnvioFormatoPeriodo(int idFormato, int idEmpresa, DateTime periodo)
        {
            return FactorySic.GetMeEnvioRepository().GetByMaxEnvioFormatoPeriodo(idFormato, idEmpresa, periodo);
        }


        #endregion

        #region Métodos Tabla ME_ENVIODET

        /// <summary>
        /// Inserta un registro de la tabla ME_ENVIODET
        /// </summary>
        public void SaveMeEnviodet(MeEnviodetDTO entity)
        {
            try
            {
                FactorySic.GetMeEnviodetRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_ENVIODET
        /// </summary>
        public void UpdateMeEnviodet(MeEnviodetDTO entity)
        {
            try
            {
                FactorySic.GetMeEnviodetRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_ENVIODET
        /// </summary>
        public void DeleteMeEnviodet(int enviocodi, int fdatpkcodi)
        {
            try
            {
                FactorySic.GetMeEnviodetRepository().Delete(enviocodi, fdatpkcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_ENVIODET
        /// </summary>
        public MeEnviodetDTO GetByIdMeEnviodet(int enviocodi)
        {
            return FactorySic.GetMeEnviodetRepository().GetById(enviocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_ENVIODET
        /// </summary>
        public List<MeEnviodetDTO> ListMeEnviodets()
        {
            return FactorySic.GetMeEnviodetRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeEnviodet
        /// </summary>
        public List<MeEnviodetDTO> GetByCriteriaMeEnviodets(int enviocodi)
        {
            return FactorySic.GetMeEnviodetRepository().GetByCriteria(enviocodi);
        }

        #endregion

        #region Métodos Tabla ME_FORMATO

        /// <summary>
        /// Inserta un registro de la tabla ME_FORMATO
        /// </summary>
        public int SaveMeFormato(MeFormatoDTO entity)
        {
            int id = 0;
            try
            {
                id = FactorySic.GetMeFormatoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_FORMATO
        /// </summary>
        public void UpdateMeFormato(MeFormatoDTO entity)
        {
            try
            {
                FactorySic.GetMeFormatoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_FORMATO
        /// </summary>
        public MeFormatoDTO GetByIdMeFormato(int formatcodi)
        {
            return FactorySic.GetMeFormatoRepository().GetById(formatcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_FORMATO
        /// </summary>
        public List<MeFormatoDTO> ListMeFormatos()
        {
            return FactorySic.GetMeFormatoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeFormato
        /// </summary>
        public List<MeFormatoDTO> GetByModuloLecturaMeFormatos(int idModulo, int idLectura, int idEmpresa)
        {
            return FactorySic.GetMeFormatoRepository().GetByModuloLectura(idModulo, idLectura, idEmpresa);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeFormato para multiples lecturas y empresas
        /// </summary>
        public List<MeFormatoDTO> GetByModuloLecturaMeFormatosMultiple(int idModulo, string lectura, string empresa)
        {
            return FactorySic.GetMeFormatoRepository().GetByModuloLecturaMultiple(idModulo, lectura, empresa);
        }

        /// <summary>
        /// Lista la tabla MeFormato por codigo de modulo
        /// </summary>
        public List<MeFormatoDTO> GetByModuloMeFormatos(int idModulo)
        {
            return FactorySic.GetMeFormatoRepository().GetByModulo(idModulo);
        }

        /// <summary>
        /// Lista la tabla MeFormato por codigo de modulo
        /// </summary>
        public List<MeFormatoDTO> PendientesMeFormatos(int idModulo, int emprcodi, string fecha)
        {
            return FactorySic.GetMeFormatoRepository().GetPendientes(idModulo, emprcodi, fecha);
        }

        #endregion

        #region Métodos Tabla ME_LECTURA

        /// <summary>
        /// Permite obtener un registro de la tabla ME_LECTURA
        /// </summary>
        public MeLecturaDTO GetByIdMeLectura(int lectcodi)
        {
            return FactorySic.GetMeLecturaRepository().GetById(lectcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_LECTURA
        /// </summary>
        public List<MeLecturaDTO> ListMeLecturas()
        {
            return FactorySic.GetMeLecturaRepository().List();
        }
        #endregion

        #region Métodos Tabla ME_ORIGENLECTURA
        public List<MeOrigenlecturaDTO> GetByCriteriaMeorigenlectura()
        {
            return FactorySic.GetMeOrigenlecturaRepository().List();
        }
        #endregion

        #region Métodos Tabla EV_EVENTOEQUIPO
        /// <summary>
        /// Devuelve lista de los equipos en SEIN
        /// </summary>
        /// <param name="empresas"></param>    
        /// <param name="nroPaginas"></param>
        /// <param name="pageSize"></param>
        /// <param name="idsFamilia"></param>
        /// <returns></returns>
        public List<EveEventoEquipoDTO> ListarDetalleEquiposSEIN(string empresas, int nroPaginas, int pageSize, string idsFamilia, string campo, string orden)
        {
            if (string.IsNullOrEmpty(empresas)) empresas = ConstantesAppServicio.ParametroNulo;

            if (string.IsNullOrEmpty(idsFamilia)) idsFamilia = ConstantesAppServicio.ParametroNulo;

            List<EveEventoEquipoDTO> entitys = FactorySic.GetEveEventoEquipoRepository().ListarDetalleEquiposSEIN(empresas, nroPaginas, pageSize, idsFamilia, campo, orden);
            foreach (var reg in entitys)
            {
                reg.EeqfechainiDesc = reg.Eeqfechaini.ToString(ConstantesAppServicio.FormatoFechaFull2);
            }

            return entitys;
        }

        /// <summary>
        /// Devuelve lista de los equipos en SEIN
        /// </summary>
        /// <param name="empresas"></param>    
        /// <param name="nroPaginas"></param>
        /// <param name="pageSize"></param>
        /// <param name="idsFamilia"></param>
        /// <returns></returns>
        public List<EveEventoEquipoDTO> ListarPendientesEquiposSEIN(string empresas, int nroPaginas, int pageSize, string idsFamilia, string fechaini, string fechafin, string orden)
        {
            if (string.IsNullOrEmpty(empresas)) empresas = ConstantesAppServicio.ParametroNulo;

            if (string.IsNullOrEmpty(idsFamilia)) idsFamilia = ConstantesAppServicio.ParametroNulo;

            List<EveEventoEquipoDTO> entitys = new List<EveEventoEquipoDTO>();
            try
            {
                entitys = FactorySic.GetEveEventoEquipoRepository().ListarPendientesEquiposSEIN(empresas,
                    nroPaginas, pageSize, idsFamilia, fechaini, fechafin, orden);
            }
            catch (Exception ex)
            {
                var result = ex.Message;
            }
            return entitys;
        }

        /// <summary>
        /// Grabar un nuevo equipo sein
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int SaveEveEventoEquipo(EveEventoEquipoDTO entity)
        {
            try
            {
                return FactorySic.GetEveEventoEquipoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Grabar un nuevo equipo sein
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AproEveEventoEquipo(EveEventoEquipoDTO entity)
        {
            try
            {
                return FactorySic.GetEveEventoEquipoRepository().Aprobar(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Grabar un nuevo equipo sein
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AprobEveEventoEquipo(int codigo, int idempresa,
            int idFamilia, int idequipo, int estado, int idmotivo, string motivoabrev, string ifecha, int idubicacion, string usuario)
        {
            try
            {
                return FactorySic.GetEveEventoEquipoRepository().AprobarE(codigo, idempresa,
             idFamilia, idequipo, estado, idmotivo, motivoabrev, ifecha, idubicacion, usuario);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region METODOS DE LA TABLA SI_MENSAJE

        /// <summary>
        /// Devuelve lista de mensajes
        /// </summary>
        /// <param name="correo"></param>    
        /// <param name="nroPaginas"></param>
        /// <param name="pageSize"></param>
        /// <param name="orden"></param>
        /// <returns></returns>
        public List<SiMensajeDTO> ListarMensajes(string correo, int nroPagina, string orden, int pageSize)
        {
            try
            {
                return FactorySic.GetSiMensajeRepository().Listar(correo, nroPagina, orden, pageSize);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }

        /// <summary>
        /// Devuelve numero total de mensajes
        /// </summary>
        /// <param name="correo"></param>
        /// <returns></returns>
        public int ContarMensajes(string correo, string orden)
        {
            try
            {
                return FactorySic.GetSiMensajeRepository().TotalMensajes(correo, orden);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }

        /// <summary>
        /// Guarda el mensaje en la tabla SI_MENSAJE
        /// </summary>
        /// <param name="correo"></param>    
        /// <param name="correoDe"></param>
        /// <param name="asunto"></param>
        /// <param name="mensaje"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public int SaveCorreo(string fechaActual, int idFuente, int TipoCorreo, int EstMsg, string Mensaje, string Periodo, int CodModulo, int EmprCodi, int FormatCodi, string usuario, string Correo, string CorreoFrom, string usuarioNom, string Asunto, int flagAdj)
        {
            try
            {
                return FactorySic.GetSiMensajeRepository().Save(fechaActual, idFuente, TipoCorreo, EstMsg, Mensaje, Periodo, CodModulo, EmprCodi, FormatCodi, usuario, Correo, CorreoFrom, usuarioNom, Asunto, flagAdj);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }

        /// <summary>
        /// Cambia el estado del mensaje en la tabla SI_MENSAJE
        /// </summary>
        /// <param name="MsgCodi"></param>    
        /// <param name="CodModulo"></param>
        /// <param name="EmprCodi"></param>
        /// <param name="usuariomod"></param>
        /// <param name="fechamod"></param>
        /// <returns></returns>
        public int UpdateEstado(int estado, int MsgCodi, int CodModulo, int EmprCodi, string usuariomod, string fechamod)
        {
            try
            {
                return FactorySic.GetSiMensajeRepository().UpdateEstado(estado, MsgCodi, CodModulo, EmprCodi, usuariomod, fechamod);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }
        #endregion

        #region METODOS DE LA TABLA SI_TIPOMENSAJE
        /// <summary>
        /// Devuelve lista de tipos de mensajes
        /// </summary>        
        /// <returns></returns>
        public List<SiTipoMensajeDTO> ListarTipoMensaje()
        {
            try
            {
                return FactorySic.GetSiTipoMensajeRepository().Listar();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }
        #endregion

        #region METODOS DE LA TABLA SI_SOLICITUDAMPLIACION
        /// <summary>
        /// Guarda la solicitud en la tabla SI_SOLICITUDAMPLIACION
        /// </summary>        
        /// <returns></returns>
        public int SaveSolicitudAmpliacion(string fecha, int msgcodi, int emprcodi, string fechaplazo, string usuario, string fechaAct, int formatcodi, int fdatcodi, int flag)
        {
            try
            {
                return FactorySic.GetSiSolicitudAmpliacionRepository().Save(fecha, msgcodi, emprcodi, fechaplazo, usuario, fechaAct, formatcodi, fdatcodi, flag);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }

        public SiSolicitudAmpliacionDTO GetSolicitudAmpXMsg(int MsgCodi)
        {
            try
            {
                return FactorySic.GetSiSolicitudAmpliacionRepository().GetByMsgCodi(MsgCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }
        #endregion

        #region METODOS DE LA TABLA ME_AMPLIACIONFECHA
        /// <summary>
        /// Lista la tabla ME_AMPLIACIONFECHA por empresa y fecha actual
        /// </summary>        
        /// <returns></returns>

        public List<MeAmpliacionfechaDTO> GetAmpliaciones(int EmprCodi, string fecha)
        {
            try
            {
                return FactorySic.GetMeAmpliacionfechaRepository().GetAmpliacionNow(EmprCodi, fecha);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }
        #endregion

        #region METODOS DE LA TABLA SI_LOG

        public List<SiLogDTO> ObtenerLog(string usuario, string fecha, int ModCodi)
        {
            try
            {
                return FactorySic.GetSiLogRepository().Listar(usuario, fecha, ModCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }

        public int SaveLog(int ModCodi, string evento, string fecha, string usuario)
        {
            try
            {
                return FactorySic.GetSiLogRepository().Save(ModCodi, evento, fecha, usuario);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }


        #endregion

        #region METODOS DE LA TABLA SI_TIPOEMPRESA
        /// <summary>
        /// Listar tipos de empresa
        /// </summary>
        /// <returns></returns>
        public List<SiTipoempresaDTO> ListarTiposEmpresa()
        {
            return FactorySic.GetSiTipoempresaRepository().List();
        }
        #endregion

        #region Métodos Tabla SI_AMPLAZOENVIO

        /// <summary>
        /// Inserta un registro de la tabla SI_AMPLAZOENVIO
        /// </summary>
        public void SaveSiAmplazoenvio(SiAmplazoenvioDTO entity)
        {
            try
            {
                FactorySic.GetSiAmplazoenvioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_AMPLAZOENVIO
        /// </summary>
        public void UpdateSiAmplazoenvio(SiAmplazoenvioDTO entity)
        {
            try
            {
                FactorySic.GetSiAmplazoenvioRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_AMPLAZOENVIO
        /// </summary>
        public SiAmplazoenvioDTO GetByIdSiAmplazoenvio(int amplzcodi)
        {
            return FactorySic.GetSiAmplazoenvioRepository().GetById(amplzcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_AMPLAZOENVIO
        /// </summary>
        public List<SiAmplazoenvioDTO> ListSiAmplazoenvios()
        {
            return FactorySic.GetSiAmplazoenvioRepository().List();
        }

        /// <summary>
        /// Devuelve lista de ampliacion de fechas para listado multiple
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="sEmpresa"></param>
        /// <param name="fdatcodi"></param>
        /// <returns></returns>
        public List<SiAmplazoenvioDTO> ObtenerListaMultipleMeAmpliacionfechas(DateTime fechaIni, DateTime fechaFin, string sEmpresa, string fdatcodi)
        {
            if (string.IsNullOrEmpty(sEmpresa)) sEmpresa = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(fdatcodi)) sEmpresa = ConstantesAppServicio.ParametroDefecto;
            List<SiAmplazoenvioDTO> lista = FactorySic.GetSiAmplazoenvioRepository().GetListaMultiple(fechaIni, fechaFin, sEmpresa, fdatcodi);

            foreach (var reg in lista)
            {
                reg.AmplzfechaDesc = reg.Amplzfecha.Value.ToString(ConstantesAppServicio.FormatoFechaHora);
                reg.AmplzfechaperiodoDesc = reg.Amplzfechaperiodo.Value.ToString(ConstantesAppServicio.FormatoFecha);
                reg.AmplzfeccreacionDesc = reg.Amplzfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaHora);
            }
            return lista;
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_AMPLAZOENVIO
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="empresa"></param>
        /// <param name="fdatcodi"></param>
        /// <returns></returns>
        public SiAmplazoenvioDTO GetByIdSiAmplazoenvioCriteria(DateTime fecha, int empresa, int fdatcodi)
        {
            return FactorySic.GetSiAmplazoenvioRepository().GetByIdCriteria(fecha, empresa, fdatcodi);
        }

        #endregion

        #region Métodos Tabla SI_PLAZOENVIO

        /// <summary>
        /// Inserta un registro de la tabla SI_PLAZOENVIO
        /// </summary>
        public void SaveSiPlazoenvio(SiPlazoenvioDTO entity)
        {
            try
            {
                FactorySic.GetSiPlazoenvioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_PLAZOENVIO
        /// </summary>
        public void UpdateSiPlazoenvio(SiPlazoenvioDTO entity)
        {
            try
            {
                FactorySic.GetSiPlazoenvioRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_PLAZOENVIO
        /// </summary>
        public SiPlazoenvioDTO GetByIdSiPlazoenvio(int plazcodi)
        {
            var reg = FactorySic.GetSiPlazoenvioRepository().GetById(plazcodi);

            if (reg != null)
            {
                reg.Fdatnombre = reg.Fdatnombre.Trim();
                reg.Usuario = reg.Plazusumodificacion != null ? reg.Plazusumodificacion : reg.Plazusucreacion;
                reg.PlazfeccreacionDesc = reg.Plazfeccreacion != null ? reg.Plazfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
                reg.PlazfecmodificacionDesc = reg.Plazfecmodificacion != null ? reg.Plazfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
                reg.Periodo = Util.PeriodoDescripcion(reg.Plazperiodo);
            }

            return reg;
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_PLAZOENVIO
        /// </summary>
        public SiPlazoenvioDTO GetByIdSiPlazoenvioByFdatcodi(int fdatcodi)
        {
            var reg = FactorySic.GetSiPlazoenvioRepository().GetByFdatcodi(fdatcodi);

            if (reg != null)
            {
                reg.Fdatnombre = reg.Fdatnombre.Trim();
                reg.Usuario = reg.Plazusumodificacion != null ? reg.Plazusumodificacion : reg.Plazusucreacion;
                reg.PlazfeccreacionDesc = reg.Plazfeccreacion != null ? reg.Plazfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
                reg.PlazfecmodificacionDesc = reg.Plazfecmodificacion != null ? reg.Plazfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
                reg.Periodo = Util.PeriodoDescripcion(reg.Plazperiodo);
            }

            return reg;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_PLAZOENVIO
        /// </summary>
        public List<SiPlazoenvioDTO> ListSiPlazoenvios()
        {
            var lista = FactorySic.GetSiPlazoenvioRepository().List();

            foreach (var reg in lista)
            {
                reg.Fdatnombre = reg.Fdatnombre.Trim();
                reg.Usuario = reg.Plazusumodificacion != null ? reg.Plazusumodificacion : reg.Plazusucreacion;
                reg.PlazfeccreacionDesc = reg.Plazfeccreacion != null ? reg.Plazfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
                reg.PlazfecmodificacionDesc = reg.Plazfecmodificacion != null ? reg.Plazfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
                reg.Periodo = Util.PeriodoDescripcion(reg.Plazperiodo);
            }

            return lista;
        }

        #endregion

        #region Plazo de Envio: Validacion

        /// <summary>
        /// Configuracion del Plazo de Envio segun Fecha de Periodo
        /// </summary>
        /// <param name="plazo"></param>
        public void GetSizePlazoEnvio(SiPlazoenvioDTO plazo)
        {
            switch (plazo.Plazperiodo)
            {
                case ParametrosFormato.PeriodoDiario: //Diario
                    plazo.FechaInicio = plazo.FechaProceso;
                    plazo.FechaFin = plazo.FechaProceso.AddDays(1).AddSeconds(-1);

                    plazo.FechaPlazoIni = plazo.FechaProceso.AddDays(plazo.Plazinidia).AddMinutes(plazo.Plazinimin);
                    plazo.FechaPlazoFin = plazo.FechaProceso.AddDays(plazo.Plazfindia).AddMinutes(plazo.Plazfinmin);
                    plazo.FechaPlazoFuera = plazo.FechaProceso.AddDays(plazo.Plazfueradia).AddMinutes(plazo.Plazfueramin);

                    //Validar configuración correcta
                    if (!(plazo.FechaPlazoIni <= plazo.FechaPlazoFin && plazo.FechaPlazoFin <= plazo.FechaPlazoFuera))
                    {
                        throw new Exception("Configuración de Envío de Plazo incorrecta");
                    }

                    break;
                default:
                    throw new Exception("Configuración de Envío de Plazo no establecido");
            }
        }

        /// <summary>
        /// Verifica si un Envio de Información esta en plazo o fuera de plazo
        /// </summary>
        /// <param name="plazo"></param>
        /// <returns></returns>
        public string EnvioValidarPlazo(SiPlazoenvioDTO plazo)
        {
            string resultado = ConstantesEnvio.ENVIO_PLAZO_DESHABILITADO;

            if (ConstantesIEOD.FdatcodiHOPTermoelectrica == plazo.Fdatcodi) //condicional para los casos que no son biogas ni bagazo
                return resultado;

            //Validación de vigencia de empresa
            if (!this.servEmpresa.EsEmpresaVigente(plazo.Emprcodi, plazo.FechaProceso))
            {
                return resultado;
            }

            DateTime fechaValidacion = plazo.IdEnvio <= 0 ? DateTime.Now : plazo.FechaEnvio.Value;

            if (plazo.FechaPlazoIni <= fechaValidacion && fechaValidacion <= plazo.FechaPlazoFuera)
            {
                return fechaValidacion <= plazo.FechaPlazoFin ? ConstantesEnvio.ENVIO_EN_PLAZO : ConstantesEnvio.ENVIO_FUERA_PLAZO;
            }
            else
            {
                //buscar en ampliación
                if (plazo.IdEnvio <= 0)
                {
                    SiAmplazoenvioDTO reg = this.GetByIdSiAmplazoenvioCriteria(plazo.FechaProceso, plazo.Emprcodi, plazo.Fdatcodi);
                    if (reg != null)
                    {
                        if (fechaValidacion <= reg.Amplzfecha)
                        {
                            return ConstantesEnvio.ENVIO_FUERA_PLAZO;
                        }
                    }
                }
            }

            return resultado;
        }

        #endregion

        #region Panel IEOD

        /// <summary>
        /// Panel de Envio de Informacion IEOD
        /// </summary>
        /// <param name="modcodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<ItemPanelIEOD> ObtenerPanelIEOD(int modcodi, int emprcodi, DateTime fechaProceso, DateTime? fechaProcesoIniFEnergPrimRerSolar)
        {
            List<ItemPanelIEOD> lista = new List<ItemPanelIEOD>();

            List<MeEnvioDTO> envios = FactorySic.GetMeEnvioRepository().ObtenerEnvioXModulo(modcodi, emprcodi, fechaProceso);
            List<MeEnvioDTO> enviosHOPTmp = new List<MeEnvioDTO>();
            List<MeEnvioDTO> enviosFenerTmp = new List<MeEnvioDTO>();
            List<SiFuentedatosDTO> fuenteDatos = this.ListSiFuentedatos();

            List<EqEquipoDTO> listaCentrales = FactorySic.GetEqEquipoRepository().CentralesXEmpresaHorasOperacion(emprcodi, fechaProceso.ToString(ConstantesBase.FormatoFecha));
            List<EqEquipoDTO> listaTCentral = listaCentrales
                .GroupBy(x => new { x.Famcodipadre, x.Famnomb })
                .Select(y => new EqEquipoDTO() { Famcodi = y.Key.Famcodipadre, Famnomb = y.Key.Famnomb }).ToList();

            ItemPanelIEOD registro;
            foreach (var reg in envios)
            {
                registro = new ItemPanelIEOD();
                registro.FechaEnvio = (DateTime)reg.Enviofecha;
                if (reg.Formatcodi > 0)
                {
                    if (ConstantesHard.IdFormatoEnergiaPrimaria == reg.Formatcodi || ConstantesHard.IdFormatoEnergiaPrimariaSolar == reg.Formatcodi)
                    {
                        if (fechaProceso.Date < fechaProcesoIniFEnergPrimRerSolar)
                        {
                            registro.Formatcodi = (int)reg.Formatcodi;
                            registro.Cumplimiento = reg.Envioplazo == "P" ? ConstantesEnvio.CumplEnviadoEnPlazo : ConstantesEnvio.CumplEnviadoFueraPlazo;
                            lista.Add(registro);
                        }
                        else
                        {
                            enviosFenerTmp.Add(reg);
                        }
                    }
                    else
                    {
                        registro.Formatcodi = (int)reg.Formatcodi;
                        registro.Cumplimiento = reg.Envioplazo == "P" ? ConstantesEnvio.CumplEnviadoEnPlazo : ConstantesEnvio.CumplEnviadoFueraPlazo;
                        lista.Add(registro);
                    }
                }
                else
                {
                    if (reg.Fdatcodi > 0)
                    {
                        SiFuentedatosDTO fdato = fuenteDatos.Find(x => x.Fdatcodi == reg.Fdatcodi);
                        if (fdato.Fdatpadre == ConstantesIEOD.FdatcodiPadreHOP) // Si es una Hora de Operacion
                        {
                            enviosHOPTmp.Add(reg);
                        }
                        else
                        {
                            if (reg.Fdatcodi != ConstantesIEOD.FdatcodiPadreHOP)
                            {
                                registro.Fdatcodi = (int)reg.Fdatcodi;
                                registro.Cumplimiento = reg.Envioplazo == "P" ? ConstantesEnvio.CumplEnviadoEnPlazo : ConstantesEnvio.CumplEnviadoFueraPlazo;
                                lista.Add(registro);
                            }
                        }
                    }
                }
            }

            #region Validar Fuente de Energia Primaria con sus 2 formatos
            if (enviosFenerTmp.Count > 0)
            {
                registro = new ItemPanelIEOD();
                registro.Formatcodi = ConstantesHard.IdFormatoEnergiaPrimaria;

                var regFenerg = enviosFenerTmp.Find(x => x.Formatcodi == ConstantesHard.IdFormatoEnergiaPrimaria);
                var regFenergSolar = enviosFenerTmp.Find(x => x.Formatcodi == ConstantesHard.IdFormatoEnergiaPrimariaSolar);
                if (listaCentrales.Find(x => x.Famcodi == ConstantesHorasOperacion.IdTipoSolar) != null)
                {
                    if (enviosFenerTmp.Count == 2)
                    {
                        int totalPlazo = enviosFenerTmp.Where(x => x.Envioplazo == "P").Count();
                        registro.Cumplimiento = totalPlazo == 2 ? ConstantesEnvio.CumplEnviadoEnPlazo : ConstantesEnvio.CumplEnviadoFueraPlazo;
                    }
                    else
                    {
                        registro.Cumplimiento = ConstantesEnvio.CumplEnviadoIncompleto;
                    }
                }
                else
                {
                    if (regFenerg != null)
                    {
                        registro.Cumplimiento = regFenerg.Envioplazo == "P" ? ConstantesEnvio.CumplEnviadoEnPlazo : ConstantesEnvio.CumplEnviadoFueraPlazo;
                    }
                    else
                    {
                        registro.Cumplimiento = ConstantesEnvio.CumplEnviadoIncompleto;
                    }
                }

                lista.Add(registro);
            }
            #endregion

            #region Validar por Fuente de Datos Horas de Operacion
            if (enviosHOPTmp.Count > 0)
            {
                registro = new ItemPanelIEOD();
                registro.Fdatcodi = ConstantesIEOD.FdatcodiPadreHOP;
                if (listaTCentral.Count == enviosHOPTmp.Count)
                {
                    int totalPlazo = enviosHOPTmp.Where(x => x.Envioplazo == "P").Count();
                    registro.Cumplimiento = totalPlazo == enviosHOPTmp.Count ? ConstantesEnvio.CumplEnviadoEnPlazo : ConstantesEnvio.CumplEnviadoFueraPlazo;
                }
                else
                {
                    registro.Cumplimiento = ConstantesEnvio.CumplEnviadoIncompleto;
                }
                lista.Add(registro);
            }
            #endregion

            return lista;
        }

        #endregion

        #region Topología Eléctrica

        /// <summary>
        /// Listar las excpeciones de la topologia
        /// </summary>
        /// <returns></returns>
        public static List<TipoDatoTopologia> ObtenerListaTipoExcepcion()
        {
            List<TipoDatoTopologia> lista = new List<TipoDatoTopologia>();
            var elemento = new TipoDatoTopologia() { Codi = ConstantesTopologiaElect.IdTipoexcepcionninguno, DetName = "Ninguno" };
            lista.Add(elemento);
            elemento = new TipoDatoTopologia() { Codi = ConstantesTopologiaElect.IdTipoexcepcionSistAislado, DetName = "Sistema Aislado" };
            lista.Add(elemento);
            return lista;
        }

        #endregion
    }
}

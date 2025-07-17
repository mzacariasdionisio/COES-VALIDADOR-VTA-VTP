using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Linq;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Monitoreo;
using COES.Servicios.Aplicacion.IEOD;

//--FINAL 
namespace COES.Servicios.Aplicacion.OperacionesVarias
{
    /// <summary>
    /// Clases con métodos del módulo Sic
    /// </summary>
    //public class SicAppServicio : AppServicioBase
    public class OperacionesVariasAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        //private static readonly ILog Logger = LogManager.GetLogger(typeof(SicAppServicio));
        private static readonly ILog Logger = LogManager.GetLogger(typeof(OperacionesVariasAppServicio));


        /// <summary>
        /// Permite listar los tipos de eventos
        /// </summary>
        /// <returns></returns>
        public List<EveEvenclaseDTO> ListarEvenclase()
        {
            return FactorySic.GetEveEvenclaseRepository().List();

        }

        /// <summary>
        /// Permite listar las causa de eventos
        /// </summary>
        /// <param name="causaEvencodi">Código de causa</param>
        /// <returns></returns>
        public List<EveSubcausaeventoDTO> ListarSubcausaevento(int causaEvencodi)
        {
            return FactorySic.GetEveSubcausaeventoRepository().ObtenerSubcausaEvento(causaEvencodi);

        }

        /// <summary>
        /// Permite buscar las operaciones
        /// </summary>
        /// <param name="evenClase">Código de clase</param>
        /// <param name="subCausacodi">Código de subcausa</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <param name="nroPage">Número de página</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns></returns>
        public List<EveIeodcuadroDTO> BuscarOperacionesDetallado(int evenClase, int subCausacodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            return FactorySic.GetEveIeodcuadroRepository().BuscarOperacionesDetallado(evenClase, subCausacodi, fechaInicio, fechaFinal, nroPage, pageSize);
        }

        /// <summary>
        /// Permite buscar las operaciones
        /// </summary>
        /// <param name="evenClase">Código de clase</param>
        /// <param name="subCausacodi">Código de subcausa</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <param name="nroPage">Número de página</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns></returns>
        public List<EveIeodcuadroDTO> BuscarOperaciones(int evenClase, int subCausacodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            return FactorySic.GetEveIeodcuadroRepository().BuscarOperaciones(evenClase, subCausacodi, fechaInicio, fechaFinal, nroPage, pageSize);
        }

        /// <summary>
        /// Permite obtener las operaciones sin considerar paginado
        /// </summary>
        /// <param name="evenClase">Código de clase</param>
        /// <param name="subCausacodi">Código de subcausa</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <returns></returns>
        public List<EveIeodcuadroDTO> BuscarOperacionesSinPaginado(int evenClase, int subCausacodi, DateTime fechaInicio, DateTime fechaFinal)
        {
            return FactorySic.GetEveIeodcuadroRepository().BuscarOperacionesSinPaginado(evenClase, subCausacodi, fechaInicio, fechaFinal, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto);
        }

        /// <summary>
        /// Permite obtener el número de filas
        /// </summary>
        /// <param name="evenClase">Código de clase</param>
        /// <param name="subCausacodi">Código de subcausa</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <param name="nroPage">Número de página</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns></returns>
        public int ObtenerNroFilas(int evenClase, int subCausacodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            return FactorySic.GetEveIeodcuadroRepository().ObtenerNroFilas(evenClase, subCausacodi, fechaInicio, fechaFinal, nroPage, pageSize);
        }

        /// <summary>
        /// Permite obtener los datos de un ieodcuadro en particular
        /// </summary>
        /// <param name="idIeodcuadro">Identificador</param>
        /// <returns></returns>
        public EveIeodcuadroDTO ObtenerIeodCuadro(int idIeodcuadro)
        {
            return FactorySic.GetEveIeodcuadroRepository().ObtenerIeodcuadro(idIeodcuadro);
        }

        /// <summary>
        /// Permite obtener los datos de un equipo en particular
        /// </summary>
        /// <param name="idEquipo">Código de equipo</param>
        /// <returns></returns>
        public EveIeodcuadroDTO ObtenerDatosEquipo(int idEquipo)
        {
            return FactorySic.GetEveIeodcuadroRepository().ObtenerDatosEquipo(idEquipo);
        }

        /// <summary>
        /// Elimina un registro de la tabla eve_ieodcuadro y el detalle de eve_ieodcuadro_det
        /// </summary>
        /// <param name="idIccodi">Identificador</param>
        public void DeleteOperacion(int idIccodi)
        {
            try
            {
                //elimina el detalle y la cabecera
                FactorySic.GetEveIeodcuadroDetRepository().Delete(idIccodi);
                FactorySic.GetEveIeodcuadroRepository().Delete(idIccodi);

                EveCongesgdespachoDTO model = new EveCongesgdespachoDTO();
                model.Iccodi = idIccodi;
                model.Congdeestado = 0;
                UpdateEveCongesgdespacho(model);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<string> ListaCoordinadores()
        {
            return FactorySic.GetEvePaleatoriaRepository().ListaCoordinadores();
        }

        /// <summary>
        /// Permite eliminar un equipo involucrado de la tabla de Operaciones varias
        /// </summary>
        /// <param name="idIccodi"></param>
        public void DeleteEquipoInvolucrado(int idIccodi)
        {
            try
            {
                //elimina el detalle y la cabecera
                FactorySic.GetEveIeodcuadroDetRepository().Delete(idIccodi);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permiteagregar un equipo involucrado de la tabla de Operaciones varias
        /// </summary>
        /// <param name="id"></param>
        /// <param name="equipoInvolucrado"></param>
        public void InsertEquipoInvolucrado(int id, string equipoInvolucrado)
        {
            try
            {

                string[] equipInvol = equipoInvolucrado.Split(new string[] { "," },
                    StringSplitOptions.None);

                for (int i = 0; i < equipInvol.Length - 1; i += 2)
                {
                    EveIeodcuadroDetDTO entity = new EveIeodcuadroDetDTO();

                    entity.Iccodi = id;
                    entity.Equicodi = Convert.ToInt32(equipInvol[i].ToString());
                    entity.Icdetcheck1 = equipInvol[i + 1].ToString();

                    FactorySic.GetEveIeodcuadroDetRepository().Save(entity);

                }


            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserción de grupo despacho
        /// </summary>
        /// <param name="id"></param>
        /// <param name="grupoInvolucrado"></param>
        /// <param name="usuario"></param>
        public void InsertGrupoInvolucrado(int id, string grupoInvolucrado, EveIeodcuadroDTO evento, string usuario)
        {
            try
            {
                string[] equipInvol = grupoInvolucrado.Split(new string[] { "," },
                    StringSplitOptions.None);

                for (int i = 0; i < equipInvol.Length; i++)
                {
                    EveCongesgdespachoDTO entity = new EveCongesgdespachoDTO();

                    entity.Iccodi = id;
                    entity.Grupocodi = Convert.ToInt32(equipInvol[i].ToString());
                    entity.Congdefechaini = evento.Ichorini;
                    entity.Congdefechafin = evento.Ichorfin;
                    entity.Congdeusucreacion = usuario;
                    entity.Congdefeccreacion = DateTime.Now;
                    entity.Congdeestado = 1;

                    FactorySic.GetEveCongesgdespachoRepository().Save(entity);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Graba el detalle de las operaciones varias
        /// </summary>
        /// <param name="entity"></param>
        public void SaveDetalle(EveIeodcuadroDetDTO entity)
        {
            try
            {

                FactorySic.GetEveIeodcuadroDetRepository().Save(entity);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// obtiene los datos eve_ieodcuadro_det
        /// </summary>
        /// <param name="idIccodi">identificador</param>
        /// <returns></returns>
        public List<EveIeodcuadroDetDTO> GetByCriteria(int idIccodi)
        {
            try
            {
                return FactorySic.GetEveIeodcuadroDetRepository().GetByCriteria(idIccodi);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Graba los datos eve_ieodcuadro
        /// </summary>
        /// <param name="entity">Entidad</param>
        /// <returns></returns>
        public int Save(EveIeodcuadroDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Iccodi == 0)
                {
                    //int idRegistro = (new GeneralAppServicio()).ObtenerNextIdTabla(ConstantesEvento.TablaIEODCuadro);
                    //entity.Iccodi = idRegistro;
                    id = FactorySic.GetEveIeodcuadroRepository().Save(entity);
                }
                else
                {
                    FactorySic.GetEveIeodcuadroRepository().Update(entity);
                    id = entity.Iccodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        #region Métodos Tabla EVE_IEODCUADRO

        /// <summary>
        /// Inserta un registro de la tabla EVE_IEODCUADRO
        /// </summary>
        public void SaveEveIeodcuadro(EveIeodcuadroDTO entity)
        {
            try
            {
                FactorySic.GetEveIeodcuadroRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EVE_IEODCUADRO
        /// </summary>
        public void UpdateEveIeodcuadro(EveIeodcuadroDTO entity)
        {
            try
            {
                FactorySic.GetEveIeodcuadroRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_IEODCUADRO
        /// </summary>
        public void DeleteEveIeodcuadro(int iccodi)
        {
            try
            {
                FactorySic.GetEveIeodcuadroRepository().Delete(iccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_IEODCUADRO
        /// </summary>
        public EveIeodcuadroDTO GetByIdEveIeodcuadro(int iccodi)
        {
            return FactorySic.GetEveIeodcuadroRepository().GetById(iccodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_IEODCUADRO
        /// </summary>
        public List<EveIeodcuadroDTO> ListEveIeodcuadros()
        {
            return FactorySic.GetEveIeodcuadroRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EveIeodcuadro
        /// </summary>
        public List<EveIeodcuadroDTO> GetByCriteriaEveIeodcuadros()
        {
            return FactorySic.GetEveIeodcuadroRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla EVE_SUBCAUSAEVENTO

        /// <summary>
        /// Permite realizar búsquedas en la tabla EveSubcausaevento
        /// </summary>
        public List<EveSubcausaeventoDTO> ObtenerSubCausaEvento()
        {

            return FactorySic.GetEveSubcausaeventoRepository().ObtenerSubcausaEvento(ConstantesOperacionesVarias.EvenSubcausa);
        }


        /// <summary>
        /// Inserta un registro de la tabla EVE_EVENTO
        /// </summary>
        public int Save(EveIeodcuadroDTO entity, List<EveIeodcuadroDetDTO> listEquipo)
        {
            try
            {
                int id = 0;
                string operacion = string.Empty;

                if (entity.Iccodi == 0)
                {
                    id = FactorySic.GetEveIeodcuadroRepository().Save(entity);
                }
                else
                {
                    id = entity.Iccodi;
                    FactorySic.GetEveIeodcuadroDetRepository().Delete(id);
                }


                foreach (EveIeodcuadroDetDTO item in listEquipo)
                {
                    EveIeodcuadroDetDTO itemEquipo = new EveIeodcuadroDetDTO();
                    itemEquipo.Iccodi = id;
                    itemEquipo.Equicodi = item.Equicodi;
                    itemEquipo.Icdetcheck1 = item.Icdetcheck1;

                    FactorySic.GetEveIeodcuadroDetRepository().Save(itemEquipo);
                }

                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion


        #region Métodos Tabla EVE_SUBCAUSA_FAMILIA

        /// <summary>
        /// Inserta un registro de la tabla EVE_SUBCAUSA_FAMILIA
        /// </summary>
        public void SaveEveSubcausaFamilia(EveSubcausaFamiliaDTO entity)
        {
            try
            {
                FactorySic.GetEveSubcausaFamiliaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EVE_SUBCAUSA_FAMILIA
        /// </summary>
        public void UpdateEveSubcausaFamilia(EveSubcausaFamiliaDTO entity)
        {
            try
            {
                FactorySic.GetEveSubcausaFamiliaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_SUBCAUSA_FAMILIA
        /// </summary>
        public void DeleteEveSubcausaFamilia(int scaufacodi)
        {
            try
            {
                FactorySic.GetEveSubcausaFamiliaRepository().Delete(scaufacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_SUBCAUSA_FAMILIA
        /// </summary>
        public EveSubcausaFamiliaDTO GetByIdEveSubcausaFamilia(int scaufacodi)
        {
            return FactorySic.GetEveSubcausaFamiliaRepository().GetById(scaufacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_SUBCAUSA_FAMILIA
        /// </summary>
        public List<EveSubcausaFamiliaDTO> ListEveSubcausaFamilias()
        {
            return FactorySic.GetEveSubcausaFamiliaRepository().List();
        }




        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_SUBCAUSA_FAMILIA de acuerdo a subcausa
        /// </summary>
        public string ListEveSubcausaFamilias(int subcausacodi)
        {
            return FactorySic.GetEveSubcausaFamiliaRepository().ListFamilia(subcausacodi);
        }


        /// <summary>
        /// Permite realizar búsquedas en la tabla EveSubcausaFamilia
        /// </summary>
        public List<EveSubcausaFamiliaDTO> GetByCriteriaEveSubcausaFamilias()
        {
            return FactorySic.GetEveSubcausaFamiliaRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla EVE_SUBCAUSA_FAMILIA
        /// </summary>
        public int SaveEveSubcausaFamiliaId(EveSubcausaFamiliaDTO entity)
        {
            return FactorySic.GetEveSubcausaFamiliaRepository().SaveEveSubcausaFamiliaId(entity);
        }
        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla EVE_SUBCAUSA_FAMILIA
        /// </summary>
        public List<EveSubcausaFamiliaDTO> BuscarOperaciones(string estado, int subcausaCodi, int famCodi, int nroPage, int pageSize)
        {
            return FactorySic.GetEveSubcausaFamiliaRepository().BuscarOperaciones(estado, subcausaCodi, famCodi, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla EVE_SUBCAUSA_FAMILIA
        /// </summary>
        public int ObtenerNroFilas(string estado, int subcausaCodi, int famCodi)
        {
            return FactorySic.GetEveSubcausaFamiliaRepository().ObtenerNroFilas(estado, subcausaCodi, famCodi);
        }


        /// <summary>
        /// Valida si existe la relación. Tabla EVE_SUBCAUSA_FAMILIA
        /// </summary>
        public bool ExisteRelacion(int subcausaCodi, int famCodi)
        {
            List<EveSubcausaFamiliaDTO> listaRelacion = FactorySic.GetEveSubcausaFamiliaRepository().GetByCriteria();

            listaRelacion = listaRelacion.Where(x => x.Subcausacodi == subcausaCodi && x.Famcodi == famCodi).ToList();

            return (listaRelacion.Count > 0);


        }

        #endregion

        #region Métodos Tabla PR_GRUPO
        /// <summary>
        /// Lista de grupos
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoDTO> ListarGrupo()
        {
            return FactorySic.GetPrGrupoRepository().ListaPrGruposPaginado(-2, ConstantesMonitoreo.CatecodiGrupoDespacho, string.Empty, "-1", -1, -1, DateTime.Now, -1, -1).ToList();
        }

        /// <summary>
        /// Obtener Grupo by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PrGrupoDTO ObtenerDatosGrupo(int id)
        {
            return FactorySic.GetPrGrupoRepository().GetById(id);
        }

        #endregion

        #region Métodos Tabla EVE_CONGESGDESPACHO

        /// <summary>
        /// Actualiza un registro de la tabla EVE_CONGESGDESPACHO
        /// </summary>
        public void UpdateEveCongesgdespacho(EveCongesgdespachoDTO entity)
        {
            try
            {
                FactorySic.GetEveCongesgdespachoRepository().UpdateEstado(entity);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_CONGESGDESPACHO
        /// </summary>
        public void DeleteEveCongesgdespacho(int ccodi)
        {
            try
            {
                FactorySic.GetEveCongesgdespachoRepository().Delete(ccodi);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_CONGESGDESPACHO
        /// </summary>
        public List<EveCongesgdespachoDTO> GetByIdEveCongesgdespacho(int congdecodi)
        {
            return FactorySic.GetEveCongesgdespachoRepository().GetById(congdecodi);
        }

        /// <summary>
        /// Lista de EveCongesgdespachos Activos
        /// </summary>
        /// <param name="congdecodi"></param>
        /// <returns></returns>
        public List<EveCongesgdespachoDTO> ListEveCongesgdespachosActivos(int congdecodi)
        {
            List<EveCongesgdespachoDTO> list = FactorySic.GetEveCongesgdespachoRepository().GetById(congdecodi);

            List<EveCongesgdespachoDTO> lisRecorrido = list.Where(x => x.Congdeestado == ConstantesMonitoreo.CongesgdespachoEstadoActivo).ToList();

            List<EveCongesgdespachoDTO> listFinal = new List<EveCongesgdespachoDTO>();

            foreach (var valor in lisRecorrido)
            {
                EveCongesgdespachoDTO reg = new EveCongesgdespachoDTO();
                reg.Iccodi = valor.Iccodi;
                reg.Grupocodi = valor.Grupocodi;
                reg.Congdecodi = valor.Congdecodi;
                reg.CongdefechainiDesc = valor.Congdefechaini.ToString();
                reg.CongdefechafinDesc = valor.Congdefechafin.ToString();
                listFinal.Add(reg);
            }

            return listFinal;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_CONGESGDESPACHO
        /// </summary>
        public List<EveCongesgdespachoDTO> ListEveCongesgdespachos()
        {
            return FactorySic.GetEveCongesgdespachoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EveCongesgdespacho
        /// </summary>
        public List<EveCongesgdespachoDTO> GetByCriteriaEveCongesgdespachos()
        {
            return FactorySic.GetEveCongesgdespachoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla EVE_GPSAISLADO

        /// <summary>
        /// Inserción de los gps asociados al iccodi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="grupoInvolucrado"></param>
        /// <param name="usuario"></param>
        public void InsertListaGpsAislado(int id, string strGpscodi, string strAisladoFlagPpal, EveIeodcuadroDTO evento, string usuario)
        {
            try
            {
                string[] listaGpscodi = strGpscodi.Split(new string[] { "," }, StringSplitOptions.None);
                string[] listaFlagPrincipal = strAisladoFlagPpal.Split(new string[] { "," }, StringSplitOptions.None);

                var query = listaGpscodi.GroupBy(x => x).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
                if (query.Count > 0)
                    throw new Exception("Ha seleccionado GPS iguales, eliminar la selección errónea");

                for (int i = 0; i < listaGpscodi.Length; i++)
                {
                    EveGpsaisladoDTO entity = new EveGpsaisladoDTO();

                    entity.Iccodi = id;
                    entity.Gpscodi = Convert.ToInt32(listaGpscodi[i].ToString());
                    entity.Gpsaisprincipal = Convert.ToInt32(listaFlagPrincipal[i].ToString());
                    entity.Gpsaisusucreacion = usuario;
                    entity.Gpsaisfeccreacion = DateTime.Now;

                    FactorySic.GetEveGpsaisladoRepository().Save(entity);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta un registro de la tabla EVE_GPSAISLADO
        /// </summary>
        public void SaveEveGpsaislado(EveGpsaisladoDTO entity)
        {
            try
            {
                FactorySic.GetEveGpsaisladoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EVE_GPSAISLADO
        /// </summary>
        public void UpdateEveGpsaislado(EveGpsaisladoDTO entity)
        {
            try
            {
                FactorySic.GetEveGpsaisladoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_GPSAISLADO
        /// </summary>
        public void DeleteEveGpsaislado(int gpsaiscodi)
        {
            try
            {
                FactorySic.GetEveGpsaisladoRepository().Delete(gpsaiscodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_GPSAISLADO
        /// </summary>
        public void DeleteEveGpsaisladoByIccodi(int iccodi)
        {
            try
            {
                FactorySic.GetEveGpsaisladoRepository().DeleteByIccodi(iccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_GPSAISLADO
        /// </summary>
        public EveGpsaisladoDTO GetByIdEveGpsaislado(int gpsaiscodi)
        {
            return FactorySic.GetEveGpsaisladoRepository().GetById(gpsaiscodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_GPSAISLADO
        /// </summary>
        public List<EveGpsaisladoDTO> ListEveGpsaislados()
        {
            return FactorySic.GetEveGpsaisladoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EveGpsaislado
        /// </summary>
        public List<EveGpsaisladoDTO> GetByCriteriaEveGpsaislados(int iccodi)
        {
            return FactorySic.GetEveGpsaisladoRepository().GetByCriteria(iccodi);
        }

        #endregion

        #region Métodos Tabla ME_GPS

        /// <summary>
        /// Listar todos los gps oficiales
        /// </summary>
        /// <returns></returns>
        public List<MeGpsDTO> ListarGps()
        {
            return (new PR5ReportesAppServicio()).ListarGpsxFiltro(ConstantesAppServicio.ParametroDefecto).Where(x => x.Gpsestado == "A").ToList();
        }

        #endregion

        #region Métodos Tabla EVE_AREA_SUBCAUSAEVENTO

        /// <summary>
        /// Inserta un registro de la tabla EVE_AREA_SUBCAUSAEVENTO
        /// </summary>
        public void SaveEveAreaSubcausaevento(EveAreaSubcausaeventoDTO entity)
        {
            try
            {
                FactorySic.GetEveAreaSubcausaeventoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EVE_AREA_SUBCAUSAEVENTO
        /// </summary>
        public void UpdateEveAreaSubcausaevento(EveAreaSubcausaeventoDTO entity)
        {
            try
            {
                FactorySic.GetEveAreaSubcausaeventoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_AREA_SUBCAUSAEVENTO
        /// </summary>
        public void DeleteEveAreaSubcausaevento(int arscaucodi)
        {
            try
            {
                FactorySic.GetEveAreaSubcausaeventoRepository().Delete(arscaucodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_AREA_SUBCAUSAEVENTO
        /// </summary>
        public EveAreaSubcausaeventoDTO GetByIdEveAreaSubcausaevento(int arscaucodi)
        {
            return FactorySic.GetEveAreaSubcausaeventoRepository().GetById(arscaucodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_AREA_SUBCAUSAEVENTO
        /// </summary>
        public List<EveAreaSubcausaeventoDTO> ListEveAreaSubcausaeventos()
        {
            return FactorySic.GetEveAreaSubcausaeventoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EveAreaSubcausaevento
        /// </summary>
        public List<EveAreaSubcausaeventoDTO> GetByCriteriaEveAreaSubcausaeventos(int subcausacodi, string estado)
        {
            return FactorySic.GetEveAreaSubcausaeventoRepository().ListBySubcausacodi(subcausacodi, estado);
        }

        #endregion

        #region Métodos Tabla FW_AREA

        /// <summary>
        /// Listar areas segun filtro
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

        #endregion

        #region Relación Area Usuario - Tipo Operacion

        /// <summary>
        /// Listar Subcausaevento que han sido configurados
        /// </summary>
        /// <returns></returns>
        public List<EveSubcausaeventoDTO> ListarSubcausaeventoConfigurado()
        {
            List<EveSubcausaeventoDTO> l = this.ListarSubcausaevento(ConstantesOperacionesVarias.EvenSubcausa);
            List<int> listaSubcausacodiAll = this.ListarSubcausacodiRegistrados();

            foreach (var reg in l)
            {
                reg.ConfiguradoRelacionAreaDesc = listaSubcausacodiAll.Contains(reg.Subcausacodi) ? ConstantesAppServicio.SIDesc : ConstantesAppServicio.NODesc;
            }

            return l;
        }

        /// <summary>
        /// Listar subcausaevento registrados en el sistema
        /// </summary>
        /// <returns></returns>
        public List<int> ListarSubcausacodiRegistrados()
        {
            return FactorySic.GetEveAreaSubcausaeventoRepository().ListarSubcausacodiRegistrados();
        }

        /// <summary>
        /// Guardar la  configuracion del concepto
        /// </summary>
        /// <param name="concepcodi"></param>
        /// <param name="listaData"></param>
        /// <param name="usuario"></param>
        public void GuardarRelacionAreaSubcausa(int subcausacodi, List<int> inlistaAreacocodi, string usuario)
        {
            DateTime fecha = DateTime.Now;
            //////////////////////////////////////////////////////////////////////////////////////
            // bd
            List<EveAreaSubcausaeventoDTO> listaAreaXSubcausa = this.GetByCriteriaEveAreaSubcausaeventos(subcausacodi, ConstantesAppServicio.ParametroDefecto);
            var listaAreaUserSelect = listaAreaXSubcausa.Where(x => inlistaAreacocodi.Contains(x.Areacode)).ToList();
            var listaAreauserNoSelect = listaAreaXSubcausa.Where(x => !inlistaAreacocodi.Contains(x.Areacode)).ToList();

            //Actualizar
            foreach (var select in listaAreaUserSelect)
            {
                select.Arscauactivo = ConstantesEvento.RelacionActivo;
                select.Arscaufecmodificacion = fecha;
                select.Arscauusumodificacion = usuario;

                this.UpdateEveAreaSubcausaevento(select);
            }

            //Eliminar
            foreach (var noselect in listaAreauserNoSelect)
            {
                noselect.Arscauactivo = ConstantesEvento.RelacionInactivo;
                noselect.Arscaufecmodificacion = fecha;
                noselect.Arscauusumodificacion = usuario;

                this.UpdateEveAreaSubcausaevento(noselect);
            }

            //////////////////////////////////////////////////////////////////////////////////////
            //Nuevo
            var listaAreaRegistrados = listaAreaXSubcausa.Select(x => x.Areacode).ToList();
            var listaAreaNuevo = inlistaAreacocodi.Where(x => !listaAreaRegistrados.Contains(x)).Select(x => x).ToList();
            foreach (var areacode in listaAreaNuevo)
            {
                EveAreaSubcausaeventoDTO reg = new EveAreaSubcausaeventoDTO();
                reg.Subcausacodi = subcausacodi;
                reg.Areacode = areacode;
                reg.Arscaufeccreacion = fecha;
                reg.Arscauusucreacion = usuario;
                reg.Arscauactivo = ConstantesEvento.RelacionActivo;

                this.SaveEveAreaSubcausaevento(reg);
            }

            List<int> areasCoes = ConstantesEvento.AreacoesParaVisualizacion.Split(',').Select(x => int.Parse(x)).ToList();
            var listaArea = this.ListarAreaByListacodi(areasCoes);
            var listaAreacodiAplicativo = listaArea.Select(x => x.Areacode).ToList();
            var listaAreaNuevoNoActivo = listaAreacodiAplicativo.Where(x => !listaAreaRegistrados.Contains(x) && !listaAreaNuevo.Contains(x)).ToList();

            foreach (var areacode in listaAreaNuevoNoActivo)
            {
                EveAreaSubcausaeventoDTO reg = new EveAreaSubcausaeventoDTO();
                reg.Subcausacodi = subcausacodi;
                reg.Areacode = areacode;
                reg.Arscaufeccreacion = fecha;
                reg.Arscauusucreacion = usuario;
                reg.Arscauactivo = ConstantesEvento.RelacionInactivo;
                this.SaveEveAreaSubcausaevento(reg);
            }
        }

        /// <summary>
        /// Listar Tipo de Operaciones segun Area usuaria
        /// </summary>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        public List<EveSubcausaeventoDTO> ListarSubcausaeventoByAreausuaria(int subcausacodi, int areaCode)
        {
            List<EveSubcausaeventoDTO> lista = FactorySic.GetEveSubcausaeventoRepository().ObtenerSubcausaEventoByAreausuaria(subcausacodi, areaCode);

            return lista;
        }

        #endregion
    }
}

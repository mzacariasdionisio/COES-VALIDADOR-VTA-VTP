using COES.Base.Core;
using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using COES.Servicios.Aplicacion.ReportesMedicion.Helper;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace COES.Servicios.Aplicacion.ReportesMedicion
{
    public class FormatoReporteAppServicio : AppServicioBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FormatoReporteAppServicio));

        #region Métodos Tabla SI_EMPRESA
        /// <summary>
        /// Permite listar las empresas
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresas()
        {
            return FactorySic.GetSiEmpresaRepository().ListadoComboEmpresasPorTipo(-2);
        }

        public List<SiEmpresaDTO> ObtenerEmpresasPorUsuario(string userlogin)
        {
            return FactorySic.GetSiEmpresaRepository().GetByUser(userlogin);
        }

        /// <summary>
        /// Devuelve entidad empresadto buscado por id
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public SiEmpresaDTO GetByIdSiEmpresa(int idEmpresa)
        {
            return FactorySic.GetSiEmpresaRepository().GetById(idEmpresa);
        }

        /// <summary>
        /// Devuelve lista de empresa por id formato
        /// </summary>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> GetListaEmpresaFormato(int idFormato)
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresaFormato(idFormato);
        }

        public List<SiEmpresaDTO> ListarEmpresasPorTipo(int iTipoEmpresa)
        {
            return FactorySic.GetSiEmpresaRepository().ListadoComboEmpresasPorTipo(iTipoEmpresa);
        }

        /// <summary>
        /// Obtener ListaEmpresaByOriglectcodi
        /// </summary>
        /// <param name="origlectcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerListaEmpresaByOriglectcodi(int origlectcodi)
        {
            return FactorySic.GetSiEmpresaRepository().ListarEmpresaPorOrigenPtoMedicion(origlectcodi).Where(x => x.Emprcodi > 0).ToList();
        }

        #endregion

        #region Métodos Tabla SI_TIPOEMPRESA

        /// <summary>
        /// Listar tipo de empresas
        /// </summary>
        /// <returns></returns>
        public List<SiTipoempresaDTO> ListarTiposEmpresa()
        {
            return FactorySic.GetSiTipoempresaRepository().List().OrderBy(t => t.Tipoemprdesc).ToList();
        }

        #endregion

        #region METODOS TABLA ME_LECTURA

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_LECTURA
        /// </summary>
        public List<MeLecturaDTO> ListMeLecturas()
        {
            return FactorySic.GetMeLecturaRepository().List().OrderBy(x => x.Lectnomb).ToList();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_LECTURA
        /// </summary>
        public MeLecturaDTO GetByIdMeLectura(int lectcodi)
        {
            return FactorySic.GetMeLecturaRepository().GetById(lectcodi);
        }

        /// <summary>
        /// Listar Area X Formato
        /// </summary>
        /// <param name="idOrigen"></param>
        /// <returns></returns>
        public List<FwAreaDTO> ListAreaXFormato(int idOrigen)
        {
            try
            {
                return FactorySic.GetFwAreaRepository().ListAreaXFormato(idOrigen);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_ORIGENLECTURA
        /// </summary>
        public List<MeOrigenlecturaDTO> ListMeOrigenlecturas()
        {
            return FactorySic.GetMeOrigenlecturaRepository().List();
        }

        #endregion

        #region METODOS TABLA ME_ORIGENLECTURA

        /// <summary>
        /// Get By Origen lectura
        /// </summary>
        /// <param name="origlectcodi"></param>
        /// <returns></returns>
        public MeOrigenlecturaDTO GetByOrigenlectura(int origlectcodi)
        {
            return FactorySic.GetMeOrigenlecturaRepository().GetById(origlectcodi);
        }

        #endregion

        #region Metodos TABLA ME_CABECERA
        /// <summary>
        /// Devuelve lista de cabeceras de formato
        /// </summary>
        /// <returns></returns>
        public List<MeCabeceraDTO> GetListMeCabecera()
        {
            return FactorySic.GetCabeceraRepository().List().OrderBy(x => x.Cabdescrip).ToList();
        }
        #endregion

        #region Metodos TABLA ME_FORMATO

        /// <summary>
        /// Permite obtener un registro de la tabla ME_FORMATO
        /// </summary>
        public MeFormatoDTO GetByIdMeFormato(int formatcodi)
        {
            var formato = FactorySic.GetMeFormatoRepository().GetById(formatcodi);
            return formato;
        }

        #endregion

        #region Métodos TABLA FW_MODULO
        /// <summary>
        /// Permite listar todos los registros de la tabla FW_MODULO
        /// </summary>
        public List<FwModuloDTO> ListFwModulo()
        {
            return FactorySic.GetFwModuloRepository().List().OrderBy(x => x.Modnombre).ToList();
        }
        #endregion

        #region Métodos TABLA FW_AREA
        /// <summary>
        /// Permite listar todos los registros de la tabla FW_AREA
        /// </summary>
        public List<FwAreaDTO> ListFwAreas()
        {
            return FactorySic.GetFwAreaRepository().List();
        }
        #endregion

        #region Métodos TABLA SI_MENUREPORTE

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SiMenureporteTipoDTO> GetListaMenuReporteTipo()
        {
            return FactorySic.GetSiMenureporteTipoRepository().List();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SiMenureporteTipoDTO> GetListaMenuReporteTipoById(int id)
        {
            return FactorySic.GetSiMenureporteTipoRepository().GetByCriteria(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SiMenureporteDTO> GetListaMenuReporte(int id)
        {
            return FactorySic.GetSiMenureporteRepository().GetListaAdmReporte(id);
        }

        #endregion

        #region Métodos TABLA EQ_EQUIPO

        /// <summary>
        /// Devuelve lista de equipo por empresa y familia
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> GetByCriteriaEqequipo(int emprcodi, int famcodi)
        {
            return FactorySic.GetEqEquipoRepository().GetByEmprFam(emprcodi, famcodi);
        }

        /// <summary>
        /// Permite obtener los equipos por familia
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ObtenerEquiposPorFamilia(int emprcodi, int famcodi)
        {
            List<EqEquipoDTO> list = FactorySic.GetEqEquipoRepository().ObtenerEquipoPorFamilia(emprcodi, famcodi);

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
                }

                return list.OrderBy(x => x.Equinomb).ToList();
            }

            return list;
        }

        /// <summary>
        /// Permite obtener los equipos por familia
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ObtenerEquiposPorFamiliaOriglectcodi(int emprcodi, int famcodi, int origlectcodi)
        {
            List<EqEquipoDTO> list = FactorySic.GetEqEquipoRepository().ObtenerEquiposPorFamiliaOriglectcodi(emprcodi, famcodi, origlectcodi);

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
                }

                return list.OrderBy(x => x.Equinomb).ToList();
            }

            return list;
        }

        #endregion

        #region Métodos TABLA EQ_FAMILIA

        /// <summary>
        /// Devuelve lista de familia
        /// </summary>
        /// <returns></returns>
        public List<EqFamiliaDTO> ListarFamilia()
        {
            return FactorySic.GetEqFamiliaRepository().List().OrderBy(x => x.Famnomb).ToList();
        }

        /// <summary>
        /// Lista Familia By Origen Lectura y Empresa
        /// </summary>
        /// <param name="origlectcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<EqFamiliaDTO> ObtenerFamiliaPorOrigenLecturaEquipo(int origlectcodi, int emprcodi)
        {
            return FactorySic.GetEqFamiliaRepository().ListarFamiliaPorOrigenLecturaEquipo(origlectcodi, emprcodi);
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

        #region Métodos TABLA SI_TIPOINFORMACION

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_TIPOINFORMACION
        /// </summary>
        public List<SiTipoinformacionDTO> ListSiTipoinformacions()
        {
            return FactorySic.GetSiTipoinformacionRepository().List().OrderBy(x => x.Tipoinfoabrev).ToList();
        }

        #endregion

        #region Métodos TABLA ME_TIPOPUNTOMEDICION

        /// <summary>
        /// Listar MeTipopuntomedicionDTO por infocodi
        /// </summary>
        /// <returns></returns>
        public List<MeTipopuntomedicionDTO> ListMeTipopuntomedicionByTipoinfocodi(int tipoinfocodi)
        {
            return FactorySic.GetMeTipopuntomedicionRepository().GetByCriteria().Where(x => x.Tipoinfocodi == tipoinfocodi).OrderBy(x => x.Tipoptomedinomb).ToList(); ;
        }

        #endregion

        #region Métodos TABLA ME_PTOMEDICION
        public MePtomedicionDTO GetByIdMePtomedicion(int ptomedicodi)
        {
            return FactorySic.GetMePtomedicionRepository().GetById(ptomedicodi);
        }

        /// <summary>
        /// Graba entidad pto de medicio
        /// </summary>
        /// <param name="ptoMedicion"></param>
        public int SavePtoMedicion(MePtomedicionDTO ptoMedicion)
        {
            try
            {
                return FactorySic.GetMePtomedicionRepository().Save(ptoMedicion);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Update entidad pto de medicion
        /// </summary>
        /// <param name="ptoMedicion"></param>
        public void UpdatePtoMedicion(MePtomedicionDTO ptoMedicion)
        {
            try
            {
                FactorySic.GetMePtomedicionRepository().Update(ptoMedicion);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get By Id Equipo MePtomedicion
        /// </summary>
        /// <param name="equipo"></param>
        /// <param name="origlectcodi"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> GetByIdEquipoMePtomedicion(int equipo, int origlectcodi, int lectcodi)
        {
            return FactorySic.GetMePtomedicionRepository().ListarByEquiOriglectcodi(equipo, origlectcodi, lectcodi);
        }

        /// <summary>
        /// Obtener los puntos que forman el calculado
        /// </summary>
        /// <param name="ptomedicalculado"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListarPtoMedicionFromCalculado(int ptomedicalculado)
        {
            var lista = FactorySic.GetMePtomedicionRepository().ListarPtoMedicionFromCalculado(ptomedicalculado.ToString());
            CompletarDatosPtoMedicionFromCalculado(ref lista);
            return lista;
        }

        /// <summary>
        /// Completa datos de de puntos calculados
        /// </summary>
        /// <param name="lista"></param>
        private void CompletarDatosPtoMedicionFromCalculado(ref List<MePtomedicionDTO> lista)
        {
            List<TipoInformacion> listaResolucionPto = this.ListarResolucionPto();

            foreach (var reg in lista)
            {
                reg.EmprnombOrigen = reg.EmprnombOrigen != null ? reg.EmprnombOrigen.Trim() : string.Empty;
                reg.PtomedibarranombOrigen = reg.PtomedibarranombOrigen != null ? reg.PtomedibarranombOrigen.Trim() : string.Empty;
                reg.PtomedielenombOrigen = reg.PtomedielenombOrigen != null ? reg.PtomedielenombOrigen.Trim() : string.Empty;
                reg.PtomedicodidescOrigen = reg.PtomedicodidescOrigen != null ? reg.PtomedicodidescOrigen.Trim() : string.Empty;
                reg.Ptomedibarranomb = reg.PtomedibarranombOrigen.Length > 0 ? reg.PtomedibarranombOrigen : reg.PtomedielenombOrigen.Length > 0 ? reg.PtomedielenombOrigen : reg.PtomedicodidescOrigen;
                reg.Ptomedicodi = ConstantesReportesMedicion.PtoCalculadoSiCodigo == reg.PtomediCalculado ? reg.PtomedicodiCalculado : reg.Ptomedicodi;
                reg.PtomediCalculadoDescrip = reg.PtomediCalculado == ConstantesReportesMedicion.PtoCalculadoSiCodigo ? ConstantesReportesMedicion.PtoCalculadoSiDescrip : ConstantesReportesMedicion.PtoCalculadoNoDescrip;

                var regResolucion = listaResolucionPto.Find(x => x.IdTipoInfo == reg.Repptotabmed);
                reg.RepptotabmedDesc = regResolucion != null ? regResolucion.NombreTipoInfo : string.Empty;
                reg.Tipoptomedinomb = reg.Tipoptomedicodi > 0 ? reg.Tipoptomedinomb : string.Empty;
                reg.EquiabrevOrigen = reg.EquicodiOrigen > 0 ? (string.IsNullOrEmpty(reg.EquiabrevOrigen) ? reg.EquinombOrigen : reg.EquiabrevOrigen) : string.Empty;
                reg.FamabrevOrigen = reg.FamcodiOrigen > 0 ? (string.IsNullOrEmpty(reg.FamabrevOrigen) ? reg.FamnombOrigen : reg.FamabrevOrigen) : string.Empty;
                reg.EmprnombOrigen = reg.EmprcodiOrigen > 0 && !string.IsNullOrEmpty(reg.EmprnombOrigen) ? reg.EmprnombOrigen : string.Empty;
                reg.Lectnomb = reg.Lectcodi > 0 && !string.IsNullOrEmpty(reg.Lectnomb) ? reg.Lectnomb : string.Empty;
                reg.Areanomb = reg.Areacodi > 0 && !string.IsNullOrEmpty(reg.Areanomb) ? reg.Areanomb : string.Empty;
            }

            lista = lista.OrderBy(x => x.Origlectnombre).ThenBy(x => x.Lectnomb).ThenBy(x => x.EmprnombOrigen).ThenBy(x => x.Equinomb).ThenBy(x => x.PtomedicodidescOrigen).ToList();
        }

        /// <summary>
        /// Obtener los puntos que forman el calculado
        /// </summary>
        /// <param name="ptomedicalculado"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListarPtoMedicionFromCalculado(string ptomedicalculado)
        {
            var lista = FactorySic.GetMePtomedicionRepository().ListarPtoMedicionFromCalculado(ptomedicalculado);
            CompletarDatosPtoMedicionFromCalculado(ref lista);
            return lista;
        }

        #endregion

        #region Métodos Tabla ME_RELACIONPTO

        /// <summary>
        /// Inserta un registro de la tabla ME_RELACIONPTO
        /// </summary>
        public void SaveMeRelacionpto(MeRelacionptoDTO entity)
        {
            try
            {
                FactorySic.GetMeRelacionptoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_RELACIONPTO
        /// </summary>
        public void UpdateMeRelacionpto(MeRelacionptoDTO entity)
        {
            try
            {
                FactorySic.GetMeRelacionptoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_RELACIONPTO
        /// </summary>
        public void DeleteMeRelacionpto(int relptocodi)
        {
            try
            {
                FactorySic.GetMeRelacionptoRepository().Delete(relptocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_RELACIONPTO
        /// </summary>
        public MeRelacionptoDTO GetByIdMeRelacionpto(int relptocodi)
        {
            return FactorySic.GetMeRelacionptoRepository().GetById(relptocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_RELACIONPTO
        /// </summary>
        public List<MeRelacionptoDTO> ListMeRelacionptos()
        {
            return FactorySic.GetMeRelacionptoRepository().List();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <returns></returns>
        public List<MeRelacionptoDTO> GetByCriteriaMerelacionpto(int ptomedicodical, int ptomedicodi)
        {
            return FactorySic.GetMeRelacionptoRepository().GetByCriteria(ptomedicodical.ToString(), ptomedicodi.ToString());
        }

        #endregion

        #region Métodos TABLA ME_REPORTE
        public List<MeReporteDTO> ListarReporteByArea(int idarea)
        {
            List<MeReporteDTO> lista = FactorySic.GetMeReporteRepository().ListarXArea(idarea).OrderBy(x => x.Reporcodi).ToList();
            foreach (var reg in lista)
            {
                reg.ReporfeccreacionDesc = reg.Reporfeccreacion != null ? reg.Reporfeccreacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;
                reg.ReporfecmodificacionDesc = reg.Reporfecmodificacion != null ? reg.Reporfecmodificacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;
            }

            return lista;
        }

        /// <summary>
        /// Listar reporte por filtros de area y modulo
        /// </summary>
        /// <param name="idarea"></param>
        /// <param name="idmodulo"></param>
        /// <returns></returns>
        public List<MeReporteDTO> ListarReporteByAreaAndModulo(int idarea, int idmodulo)
        {
            List<MeReporteDTO> lista = FactorySic.GetMeReporteRepository().ListarXAreaYModulo(idarea, idmodulo).OrderBy(x => x.Reporcodi).ToList();
            foreach (var reg in lista)
            {
                reg.ReporfeccreacionDesc = reg.Reporfeccreacion != null ? reg.Reporfeccreacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;
                reg.ReporfecmodificacionDesc = reg.Reporfecmodificacion != null ? reg.Reporfecmodificacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;
            }

            return lista;
        }

        public MeReporteDTO GetByIdReporte(int id)
        {
            return FactorySic.GetMeReporteRepository().GetById(id);
        }

        public int SaveMeReporte(MeReporteDTO entity)
        {
            return FactorySic.GetMeReporteRepository().Save(entity);
        }

        public void UpdateMeReporte(MeReporteDTO entity)
        {
            FactorySic.GetMeReporteRepository().Update(entity);
        }
        #endregion

        #region METODOS TABLA ME_REPORPTOMED
        /// <summary>
        /// Los Puntos de medicion del reporte
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <returns></returns>
        public List<MeReporptomedDTO> ListarPtoReporte(int reporcodi)
        {
            List<MeReporptomedDTO> lista = FactorySic.GetMeReporptomedRepository().ListarPuntoReporte(reporcodi, DateTime.Now.Date).Where(x => x.Repptoestado == ConstantesReportesMedicion.EstadoReporptomedActivo).ToList();

            foreach (var reg in lista)
            {
                reg.PtomediCalculadoDescrip = reg.PtomediCalculado == ConstantesReportesMedicion.PtoCalculadoSiCodigo ? ConstantesReportesMedicion.PtoCalculadoSiDescrip : ConstantesReportesMedicion.PtoCalculadoNoDescrip;
                reg.RepptoestadoDescrip = reg.Repptoestado == ConstantesReportesMedicion.EstadoReporptomedActivo ? ConstantesReportesMedicion.EstadoReporptomedActivoDescrip : ConstantesReportesMedicion.EstadoReporptomedInactivoDescrip;
            }

            return lista;
        }

        /// <summary>
        /// Los Puntos de medicion del reporte
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <returns></returns>
        public List<MeReporptomedDTO> ListarTodoPtoReporte(int reporcodi)
        {
            return ListarTodoPtoReporteByFecha(reporcodi, DateTime.Now.Date);
        }

        /// <summary>
        /// Los Puntos de medicion del reporte
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <returns></returns>
        public List<MeReporptomedDTO> ListarTodoPtoReporteByFecha(int reporcodi, DateTime fechaPeriodo)
        {
            List<MeReporptomedDTO> lista = FactorySic.GetMeReporptomedRepository().ListarPuntoReporte(reporcodi, fechaPeriodo).OrderBy(x => x.Repptoorden).ToList();
            List<TipoInformacion> listaResolucionPto = this.ListarResolucionPto();

            int orden = 1;
            foreach (var reg in lista)
            {
                reg.PtomediCalculadoDescrip = reg.PtomediCalculado == ConstantesReportesMedicion.PtoCalculadoSiCodigo ? ConstantesReportesMedicion.PtoCalculadoSiDescrip : ConstantesReportesMedicion.PtoCalculadoNoDescrip;
                reg.RepptoestadoDescrip = reg.Repptoestado == ConstantesReportesMedicion.EstadoReporptomedActivo ? ConstantesReportesMedicion.EstadoReporptomedActivoDescrip : ConstantesReportesMedicion.EstadoReporptomedInactivoDescrip;
                reg.Ptomedibarranomb = reg.Ptomedibarranomb != null ? reg.Ptomedibarranomb.Trim() : string.Empty;
                reg.Ptomedielenomb = reg.Ptomedielenomb != null ? reg.Ptomedielenomb.Trim() : string.Empty;
                reg.Ptomedidesc = reg.Ptomedidesc != null ? reg.Ptomedidesc.Trim() : string.Empty;
                reg.Ptomedibarranomb = reg.Ptomedibarranomb.Length > 0 ? reg.Ptomedibarranomb : reg.Ptomedielenomb.Length > 0 ? reg.Ptomedielenomb : reg.Ptomedidesc;
                reg.Lectcodi = reg.Lectcodi;
                reg.Repptotabmed = reg.Repptotabmed;
                var regResolucion = listaResolucionPto.Find(x => x.IdTipoInfo == reg.Repptotabmed);
                reg.RepptotabmedDesc = regResolucion != null ? regResolucion.NombreTipoInfo : string.Empty;
                reg.Tipoptomedinomb = reg.Tipoptomedicodi > 0 ? reg.Tipoptomedinomb : string.Empty;
                reg.Equiabrev = reg.Equicodi > 0 ? (string.IsNullOrEmpty(reg.Equiabrev) ? reg.Equinomb : reg.Equiabrev) : string.Empty;
                reg.Famabrev = reg.Famcodi > 0 ? (string.IsNullOrEmpty(reg.Famabrev) ? reg.Famnomb : reg.Famabrev) : string.Empty;
                reg.Emprnomb = reg.Emprcodi > 0 && !string.IsNullOrEmpty(reg.Emprnomb) ? reg.Emprnomb : string.Empty;
                reg.Lectnomb = reg.Lectcodi > 0 && !string.IsNullOrEmpty(reg.Lectnomb) ? reg.Lectnomb : string.Empty;
                reg.Areanomb = reg.Areacodi > 0 && !string.IsNullOrEmpty(reg.Areanomb) ? reg.Areanomb : string.Empty;

                //cuando se lista los puntos, automaticamente se actualiza el nombre
                reg.Repptonomb = (reg.Repptonomb ?? "").Trim();
                reg.Equiabrev = (reg.Equiabrev ?? "").Trim();

                string nombreOrig = reg.Repptonomb;
                if (string.IsNullOrEmpty(nombreOrig))
                {
                    if (reporcodi == 72 || reporcodi == 73 || reporcodi == 76) //reporte de flujos IDCOS, flujos IEOD y tensiones
                    {
                        reg.Repptonomb = reg.Equiabrev;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(reg.Repptonomb)) reg.Repptonomb = (reg.Ptomedidesc ?? "").Trim();
                    }
                }

                //Actualizar el orden
                if (reg.Repptoorden != orden || nombreOrig.ToUpper() != reg.Repptonomb.ToUpper())
                {
                    reg.Repptoorden = orden;
                    this.ActualizarReporptomed(reg);
                }
                orden++;

                //Descripcion adicional
                reg.DescPto = reg.Repptonomb;
                if (reporcodi == 72 || reporcodi == 73) reg.DescPto = string.Format("{0} ({1})", reg.Ptomedidesc, reg.Repptonomb);

            }

            //agregar texto adicional de codigo equivalente
            if (ConstantesMigraciones.IdReporteDemandaAreaPrincipal == reporcodi)
            {
                var listaFlujoIEOD = FactorySic.GetMeReporptomedRepository().ListarPuntoReporte(ConstantesPR5ReportesServicio.IdReporteFlujo, fechaPeriodo);
                var listaArea = FactorySic.GetMeReporptomedRepository().ListarPuntoReporte(ConstantesPR5ReportesServicio.ReporcodiDemandaAreas, fechaPeriodo);
                var listaSubArea = FactorySic.GetMeReporptomedRepository().ListarPuntoReporte(ConstantesPR5ReportesServicio.ReporcodiDemandaSubareas, fechaPeriodo);

                foreach (var reg in lista)
                {
                    //código equivalente
                    reg.RepptoequivptoDesc = reg.Repptoequivpto != null ? reg.Repptoequivpto.Value.ToString() : "";
                    if (reg.Repptoequivpto > 0)
                    {
                        string txt1 = GetNombreFromListaMeReporptomed(listaFlujoIEOD, reg.Repptoequivpto.Value, true);
                        string txt2 = GetNombreFromListaMeReporptomed(listaArea, reg.Repptoequivpto.Value, false);
                        string txt3 = GetNombreFromListaMeReporptomed(listaSubArea, reg.Repptoequivpto.Value, false);

                        if (!string.IsNullOrEmpty(txt1)) reg.RepptoequivptoDesc += txt1;
                        if (!string.IsNullOrEmpty(txt2)) reg.RepptoequivptoDesc += txt2;
                        if (!string.IsNullOrEmpty(txt3)) reg.RepptoequivptoDesc += txt3;
                    }
                }
            }

            return lista;
        }

        private string GetNombreFromListaMeReporptomed(List<MeReporptomedDTO> lista, int codigoEquiv, bool agregarSubestacion) 
        {
            var objFlujoIEOD = lista.Find(x => x.Ptomedicodi == codigoEquiv);
            if (objFlujoIEOD != null)
            {
                if(agregarSubestacion)
                return string.Format(" - {0} ({1})", objFlujoIEOD.Ptomedidesc, objFlujoIEOD.Repptonomb ?? "");
                else
                    return string.Format(" - {0}", objFlujoIEOD.Repptonomb ?? "");
            }

            return "";
        }

        /// <summary>
        /// Los Puntos de medicion del reporte
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListarPuntosCal()
        {
            List<MePtomedicionDTO> lista = FactorySic.GetMePtomedicionRepository().ListarPuntosCalculados()
                                .Where(x => x.Ptomediestado != ConstantesAppServicio.Anulado).OrderByDescending(x => x.Ptomedicodi).ToList();

            foreach (var reg in lista)
            {
                reg.Ptomediestadodescrip = reg.Ptomediestado == "A" ? ConstantesReportesMedicion.EstadoReporptomedActivoDescrip : ConstantesReportesMedicion.EstadoReporptomedInactivoDescrip;
                reg.Equiabrev = reg.Equicodi > 0 ? (string.IsNullOrEmpty(reg.Equiabrev) ? reg.Equinomb : reg.Equiabrev) : string.Empty;
                reg.Famabrev = reg.Famcodi > 0 ? (string.IsNullOrEmpty(reg.Famabrev) ? reg.Famnomb : reg.Famabrev) : string.Empty;
                reg.Emprnomb = reg.Emprcodi > 0 && !string.IsNullOrEmpty(reg.Emprnomb) ? reg.Emprnomb : string.Empty;
            }

            return lista;
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_REPORPTOMED
        /// </summary>
        public MeReporptomedDTO GetByIdMeReporptomed(int repptocodi)
        {
            return FactorySic.GetMeReporptomedRepository().GetById(repptocodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_REPORPTOMED
        /// </summary>
        public MeReporptomedDTO GetByIdMeReporptomed3(int reporcodi, int ptomedicodi, int lectcodi, int tipoinfocodi, int tptomedicodi)
        {
            return FactorySic.GetMeReporptomedRepository().GetById3(reporcodi, ptomedicodi, lectcodi, tipoinfocodi, tptomedicodi);
        }

        /// <summary>
        /// Graba un punto de medicion en el reporte y devuelve el registro grabado
        /// </summary>
        /// <param name="entity"></param>
        public MeReporptomedDTO GrabarReporptomed(MeReporptomedDTO entity)
        {
            int id = SaveMeMeReporptomedDTO(entity);
            return GetByIdMeReporptomed(id);
        }

        /// <summary>
        /// Update registro
        /// </summary>
        /// <param name="entity"></param>
        public void ActualizarReporptomed(MeReporptomedDTO entity)
        {
            try
            {
                FactorySic.GetMeReporptomedRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta un registro de la tabla ME_REPORPTOMED
        /// </summary>
        private int SaveMeMeReporptomedDTO(MeReporptomedDTO entity)
        {
            try
            {
                return FactorySic.GetMeReporptomedRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite eliminar un registro de la tabla ME_REPORPTOMED
        /// </summary>
        public void DeleteMeReporptomed(int repptocodi)
        {
            try
            {
                FactorySic.GetMeReporptomedRepository().Delete(repptocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        /// <summary>
        /// Permite listar los puntos de medicion
        /// </summary>
        public List<MeReporptomedDTO> GetListaPuntoFromMeReporptomed(int reporcodi, string emprcodi, string famcodi, string equicodi, string tipoMedida)
        {
            return GetListaPuntoFromMeReporptomedByFecha(reporcodi, emprcodi, famcodi, equicodi, tipoMedida, DateTime.Now.Date);
        }

        /// <summary>
        /// Permite listar los puntos de medicion
        /// </summary>
        public List<MeReporptomedDTO> GetListaPuntoFromMeReporptomedByFecha(int reporcodi, string emprcodi, string famcodi, string equicodi, string tipoMedida, DateTime fechaPeriodo)
        {
            List<MeReporptomedDTO> lista = new List<MeReporptomedDTO>();
            var listaData = this.ListarTodoPtoReporteByFecha(reporcodi, fechaPeriodo);

            var listaPtoFiltro = listaData.Where(x => x.Repptoestado == ConstantesReportesMedicion.EstadoReporptomedActivo).ToList();
            string[] listaEmp = emprcodi.Split(',');
            string[] listaFam = famcodi.Split(',');
            string[] listaEq = equicodi.Split(',');
            string[] listaMed = tipoMedida.Split(',');
            listaPtoFiltro = listaPtoFiltro.Where(x => (ConstantesAppServicio.ParametroDefecto == emprcodi || listaEmp.Contains(x.Emprcodi.ToString()))
                && (ConstantesAppServicio.ParametroDefecto == emprcodi || listaFam.Contains(x.Famcodi.ToString()))
                && (ConstantesAppServicio.ParametroDefecto == emprcodi || listaEq.Contains(x.Equicodi.ToString()))
                && (ConstantesAppServicio.ParametroDefecto == emprcodi || listaMed.Contains(x.Tipoinfocodi.ToString()))
                ).ToList();

            foreach (var reg in listaData)
            {
                var aux = listaPtoFiltro.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi);
                if (aux != null)
                {
                    reg.PtomediCalculado = aux.PtomediCalculado;
                    lista.Add(reg);
                }
            }

            return lista;
        }

        /// <summary>
        /// Listar todos los puntos de medición (calculados o no de un reporte en específico)
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> GetListaAllPuntoFromReporte(int reporcodi, int lectcodi)
        {
            List<MePtomedicionDTO> listaPto = new List<MePtomedicionDTO>();
            //Cabecera MeReporte
            List<MeReporptomedDTO> listaPtos = this.GetListaPuntoFromMeReporptomed(reporcodi, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto);

            foreach (var regReporte in listaPtos)
            {
                MePtomedicionDTO reg = new MePtomedicionDTO();
                reg.PtomediCalculado = regReporte.PtomediCalculado;
                reg.Ptomedicodi = regReporte.Ptomedicodi;
                reg.Tipoinfocodi = regReporte.Tipoinfocodi;
                reg.Lectcodi = regReporte.Lectcodi;

                List<MePtomedicionDTO> data = this.GetListaPtoCalculadaRecursivo(lectcodi, reg, 1);
                listaPto.AddRange(data);
            }

            return listaPto;
        }

        /// <summary>
        /// Metodo recurso para listar los puntos de medición
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <param name="reg"></param>
        /// <param name="nivel"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> GetListaPtoCalculadaRecursivo(int lectcodi, MePtomedicionDTO reg, int nivel)
        {
            List<MePtomedicionDTO> lista = new List<MePtomedicionDTO>();

            //Validacion del metodo recursivo
            if (ConstantesReportesMedicion.MaximoNivelRecursivo == nivel)
            {
                return lista;
            }

            //Obtencion de Data de los puntos calculados
            if (ConstantesReportesMedicion.PtoCalculadoSiCodigo == reg.PtomediCalculado)
            {
                List<MePtomedicionDTO> listaDataXPto = new List<MePtomedicionDTO>();
                List<MePtomedicionDTO> listaDataXPtoCalculado = new List<MePtomedicionDTO>();
                List<MePtomedicionDTO> listaDataXPtoMedicion = new List<MePtomedicionDTO>();

                List<MePtomedicionDTO> allPtos = this.ListarPtoMedicionFromCalculado(reg.Ptomedicodi);
                List<MePtomedicionDTO> ptosCalculado = allPtos.Where(x => x.PtomediCalculado == ConstantesReportesMedicion.PtoCalculadoSiCodigo).ToList();
                List<MePtomedicionDTO> ptosMedicion = allPtos.Where(x => x.PtomediCalculado != ConstantesReportesMedicion.PtoCalculadoSiCodigo).ToList();

                //Data de los puntos de medicion
                foreach (var regMed in ptosMedicion)
                {
                    var relPto = FactorySic.GetMeRelacionptoRepository().GetByCriteria(reg.Ptomedicodi.ToString(), regMed.PtomedicodiOrigen.ToString()).FirstOrDefault();

                    regMed.Lectcodi = relPto.Lectcodi > 0 ? relPto.Lectcodi : lectcodi;
                    regMed.Tipoinfocodi = relPto.Tipoinfocodi > 0 ? relPto.Tipoinfocodi : reg.Tipoinfocodi.GetValueOrDefault(-1);

                    listaDataXPtoMedicion.Add(regMed);
                }

                //Recursivo
                foreach (var ptoCalculado in ptosCalculado)
                {
                    ptoCalculado.Ptomedicodi = ptoCalculado.PtomedicodiOrigen;
                    ptoCalculado.Tipoinfocodi = ptoCalculado.TipoinfocodiOrigen.GetValueOrDefault(0);
                    var listaDataXPtoCalculadoTmp = this.GetListaPtoCalculadaRecursivo(lectcodi, ptoCalculado, nivel + 1);

                    listaDataXPtoCalculado.AddRange(listaDataXPtoCalculadoTmp);
                }

                listaDataXPto.AddRange(listaDataXPtoCalculado);
                listaDataXPto.AddRange(listaDataXPtoMedicion);

                lista.AddRange(listaDataXPto);
            }
            else
            {
                lista.Add(reg);
            }

            return lista;
        }

        /// <summary>
        /// Listar puntos de medicion y data por reporte
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaDatan"></param>
        /// <returns></returns>
        public List<MeReporptomedDTO> GetListaCabyDatos48MeReporteConDataParametro(int reporcodi, int lectcodi, DateTime fechaInicio, DateTime fechaFin, ref List<MePtomedicionDTO> listaPto, ref List<MeMedicion48DTO> listaDatan, ref List<MeMedicion48DTO> listaParametro)
        {
            //Cabecera MeReporte
            List<MeReporptomedDTO> listaPtos = this.GetListaPuntoFromMeReporptomed(reporcodi, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto);

            //Data MeReporte
            int tipoFiltro = 1; //agrupar la data por punto de medición
            List<MeMedicion48DTO> listaData = this.GetListaDataM48FromMeReporptomed(lectcodi, fechaInicio, fechaFin, listaPtos, true, tipoFiltro, new List<int>(), listaParametro, ref listaPto);

            foreach (var adi in listaData) { adi.Reporcodi = reporcodi; }

            listaDatan.AddRange(listaData);

            return listaPtos;
        }

        /// <summary>
        /// Listar puntos de medicion y data por reporte
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaDatan"></param>
        /// <returns></returns>
        public List<MeReporptomedDTO> GetListaCabyDatos48MeReporte(int reporcodi, int lectcodi, DateTime fechaInicio, DateTime fechaFin, ref List<MeMedicion48DTO> listaDatan)
        {
            //Cabecera MeReporte
            List<MeReporptomedDTO> listaPtos = this.GetListaPuntoFromMeReporptomed(reporcodi, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto);

            //Data MeReporte
            List<MePtomedicionDTO> listaPto = new List<MePtomedicionDTO>();
            List<MeMedicion48DTO> listaData = this.GetListaDataM48FromMeReporptomed(lectcodi, fechaInicio, fechaFin, listaPtos, false, 0, new List<int>(), new List<MeMedicion48DTO>(), ref listaPto);

            foreach (var adi in listaData) { adi.Reporcodi = reporcodi; }

            listaDatan.AddRange(listaData);

            return listaPtos;
        }

        /// <summary>
        /// Lista de los puntos para reporte data Medicion1
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nroPagina"></param>
        /// <param name="listaPtos"></param>
        /// <returns></returns>
        public List<MeMedicion1DTO> GetListaDataM1FromMeReporptomed(int reporcodi, int lectcodi, DateTime fechaInicio, DateTime fechaFin, int nroPagina, List<MeReporptomedDTO> listaPtos)
        {
            List<MeMedicion1DTO> lista = new List<MeMedicion1DTO>();

            foreach (var reg in listaPtos)
            {
                if (ConstantesReportesMedicion.PtoCalculadoSiCodigo == reg.PtomediCalculado)
                {
                    lista.AddRange(GetListaCalculadaM1(reg.Ptomedicodi, reg.Emprcodi, fechaInicio, fechaFin, lectcodi, reg.Tipoinfocodi));
                }
                else
                {
                    var d_adi = FactorySic.GetMeReporptomedRepository().GetByCriteria(reporcodi, reg.Ptomedicodi).FirstOrDefault();
                    lista.AddRange(FactorySic.GetMeMedicion1Repository().GetByCriteria(fechaInicio, fechaFin, (int)d_adi.Lectcodi, (int)d_adi.Tipoinfocodi, d_adi.Ptomedicodi.ToString()));
                }
            }

            return lista;
        }

        /// <summary>
        /// Lista de los puntos para reporte data Medicion24
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nroPagina"></param>
        /// <param name="listaPtos"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> GetListaDataM24FromMeReporptomed(int reporcodi, int lectcodi, DateTime fechaInicio, DateTime fechaFin, int nroPagina, List<MeReporptomedDTO> listaPtos)
        {
            List<MeMedicion24DTO> lista = new List<MeMedicion24DTO>();

            foreach (var reg in listaPtos)
            {
                if (ConstantesReportesMedicion.PtoCalculadoSiCodigo == reg.PtomediCalculado)
                {
                    lista.AddRange(GetListaCalculadaM24(reg.Ptomedicodi, reg.Emprcodi, fechaInicio, fechaFin, lectcodi, reg.Tipoinfocodi));
                }
                else
                {
                    var d_adi = FactorySic.GetMeReporptomedRepository().GetByCriteria(reporcodi, reg.Ptomedicodi).FirstOrDefault();
                    lista.AddRange(FactorySic.GetMeMedicion24Repository().GetByCriteria(fechaInicio, fechaFin, (int)d_adi.Lectcodi, (int)d_adi.Tipoinfocodi, d_adi.Ptomedicodi.ToString()));
                }
            }

            return lista;
        }

        /// <summary>
        /// Lista de los puntos para reporte data Medicion48
        /// </summary>
        /// <param name="lectcodi">Lectcodi por defecto en caso los puntos no tengan Lectcodi</param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaPtos">Lista de Puntos</param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetListaDataM48FromMeReporptomed(int lectcodi, DateTime fechaInicio, DateTime fechaFin, List<MeReporptomedDTO> listaPtos
            , bool utilizarDataParametro, int tipoFiltro, List<int> listaPtomedicodiOmitir, List<MeMedicion48DTO> lista48Parametro, ref List<MePtomedicionDTO> listaPto)
        {
            List<MeRelacionptoDTO> listaAllRelPto = FactorySic.GetMeRelacionptoRepository().GetByCriteria("-1", "-1");

            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            foreach (var regReporte in listaPtos)
            {
                MePtomedicionDTO reg = new MePtomedicionDTO();
                reg.PtomediCalculado = regReporte.PtomediCalculado;
                reg.Ptomedicodi = regReporte.Ptomedicodi;
                reg.Tipoinfocodi = regReporte.Tipoinfocodi;
                reg.Lectcodi = regReporte.Lectcodi;

                List<MeMedicion48DTO> data = this.GetListaCalculadaM48Recursivo(lectcodi, fechaInicio, fechaFin, reg, 1, utilizarDataParametro, tipoFiltro, 
                            listaPtomedicodiOmitir, lista48Parametro, listaAllRelPto, ref listaPto);
                lista.AddRange(data);
            }

            return lista;
        }

        /// <summary>
        /// Lista de los puntos para reporte data Medicion96
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nroPagina"></param>
        /// <param name="listaPtos"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> GetListaDataM96FromMeReporptomed(int reporcodi, int lectcodi, DateTime fechaInicio, DateTime fechaFin, int nroPagina, List<MeReporptomedDTO> listaPtos)
        {
            List<MeMedicion96DTO> lista = new List<MeMedicion96DTO>();

            foreach (var reg in listaPtos)
            {
                if (ConstantesReportesMedicion.PtoCalculadoSiCodigo == reg.PtomediCalculado)
                {
                    lista.AddRange(GetListaCalculadaM96(reg.Ptomedicodi, reg.Emprcodi, fechaInicio, fechaFin, lectcodi, reg.Tipoinfocodi));
                }
                else
                {
                    var d_adi = FactorySic.GetMeReporptomedRepository().GetByCriteria(reporcodi, reg.Ptomedicodi).FirstOrDefault();
                    lista.AddRange(FactorySic.GetMeMedicion96Repository().GetByCriteria(fechaInicio, fechaFin, (int)d_adi.Lectcodi, (int)d_adi.Tipoinfocodi, (int)d_adi.Ptomedicodi));
                }
            }

            return lista;
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
        public List<MeMedicion48DTO> GetListaCalculadaM48Recursivo(int lectcodi, DateTime fechaInicio, DateTime fechaFin, MePtomedicionDTO reg, int nivel
            , bool utilizarDataParametro, int tipoFiltro, List<int> listaPtomedicodiOmitir, List<MeMedicion48DTO> lista48Parametro, List<MeRelacionptoDTO> listaAllRelPto, ref List<MePtomedicionDTO> listaPto)
        {
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();

            //Validacion del metodo recursivo
            if (ConstantesReportesMedicion.MaximoNivelRecursivo == nivel)
            {
                return lista;
            }

            //Obtencion de Data de los puntos calculados
            if (ConstantesReportesMedicion.PtoCalculadoSiCodigo == reg.PtomediCalculado)
            {
                List<MeMedicion48DTO> listaDataXPto = new List<MeMedicion48DTO>();
                List<MeMedicion48DTO> listaDataXPtoCalculado = new List<MeMedicion48DTO>();
                List<MeMedicion48DTO> listaDataXPtoMedicion = new List<MeMedicion48DTO>();

                List<MePtomedicionDTO> allPtos = this.ListarPtoMedicionFromCalculado(reg.Ptomedicodi);
                List<MePtomedicionDTO> ptosCalculado = allPtos.Where(x => x.PtomediCalculado == ConstantesReportesMedicion.PtoCalculadoSiCodigo).ToList();
                List<MePtomedicionDTO> ptosMedicion = allPtos.Where(x => x.PtomediCalculado != ConstantesReportesMedicion.PtoCalculadoSiCodigo).ToList();

                //Data de los puntos de medicion
                foreach (var regMed in ptosMedicion)
                {
                    this.AgregarPtoAListaAllPtosFromReporte(regMed, ref listaPto);
                    MeRelacionptoDTO relPto = listaAllRelPto.Where(x=>x.Ptomedicodi1 == reg.Ptomedicodi && x.Ptomedicodi2 == regMed.PtomedicodiOrigen).FirstOrDefault();

                    List<MeMedicion48DTO> listaDataXPtoMedicionTmp = new List<MeMedicion48DTO>();
                    //if (!(relPto.Lectcodi >0 && relPto.Lectcodi == 6 && lectcodi == 93)) //si la lectura del puntos de medicion es EJECUTADO y el reporte requiere solo data de DESPACHO EJECUTADO, entonces se omite agregar esa lista al resultado final
                    listaDataXPtoMedicionTmp = this.GetListaMedicion48ByFlag(fechaInicio.Date, fechaFin.Date
                        , relPto.Lectcodi > 0 ? relPto.Lectcodi : lectcodi
                        , (relPto.Tipoinfocodi > 0 ? relPto.Tipoinfocodi : reg.Tipoinfocodi.GetValueOrDefault(-1))
                        , new MePtomedicionDTO() { Ptomedicodi = relPto.Ptomedicodi2, Lectcodi = lectcodi, Equicodi = relPto.Equicodi, Grupocodi = relPto.Grupocodi }
                        , utilizarDataParametro, tipoFiltro, listaPtomedicodiOmitir, lista48Parametro);

                    foreach (var tmp in listaDataXPtoMedicionTmp)
                    {
                        tmp.Factor = regMed.FactorOrigen;
                        tmp.Tgenercodi = regMed.Tgenercodi;
                    }

                    listaDataXPtoMedicion.AddRange(listaDataXPtoMedicionTmp);
                }

                //Recursivo
                foreach (var ptoCalculado in ptosCalculado)
                {
                    ptoCalculado.Ptomedicodi = ptoCalculado.PtomedicodiOrigen;
                    ptoCalculado.Tipoinfocodi = ptoCalculado.TipoinfocodiOrigen.GetValueOrDefault(0);
                    var listaDataXPtoCalculadoTmp = this.GetListaCalculadaM48Recursivo(lectcodi, fechaInicio, fechaFin, ptoCalculado, nivel + 1,
                                    utilizarDataParametro, tipoFiltro, listaPtomedicodiOmitir, lista48Parametro, listaAllRelPto, ref listaPto);

                    foreach (var tmp in listaDataXPtoCalculadoTmp)
                    {
                        tmp.Factor = ptoCalculado.FactorOrigen;
                        tmp.Tgenercodi = ptoCalculado.Tgenercodi;
                    }

                    listaDataXPtoCalculado.AddRange(listaDataXPtoCalculadoTmp);
                }

                listaDataXPto.AddRange(listaDataXPtoCalculado);
                listaDataXPto.AddRange(listaDataXPtoMedicion);

                lista.AddRange(this.GetListaCalculadaM48(reg.Ptomedicodi, reg.Emprcodi.GetValueOrDefault(-1), fechaInicio, fechaFin, reg.Tipoinfocodi.GetValueOrDefault(-1), listaDataXPto));
            }
            else
            {
                this.AgregarPtoAListaAllPtosFromReporte(reg, ref listaPto);
                lista.AddRange(this.GetListaMedicion48ByFlag(fechaInicio, fechaFin, reg.Lectcodi, reg.Tipoinfocodi.GetValueOrDefault(-1), reg, utilizarDataParametro, tipoFiltro, listaPtomedicodiOmitir, lista48Parametro));
            }

            return lista;
        }

        private List<MeMedicion48DTO> GetListaMedicion48ByFlag(DateTime fechaInicio, DateTime fechaFin, int lectcodi, int tipoinfocodi, MePtomedicionDTO regPtomedicion, bool utilizarDataParametro, int tipoFiltro, List<int> listaPtomedicodiOmitir, List<MeMedicion48DTO> lista48Parametro)
        {
            if (listaPtomedicodiOmitir.Contains(regPtomedicion.Ptomedicodi))
                return new List<MeMedicion48DTO>();

            if (utilizarDataParametro)
            {
                //cuando ya se tiene la data en memoria no es necesario filtrar exactamente su lectcodi
                switch (tipoFiltro)
                {
                    case 1://Ptomedicion
                        return lista48Parametro.Where(x => x.Medifecha >= fechaInicio.Date && x.Medifecha <= fechaFin.Date && x.Tipoinfocodi == tipoinfocodi && x.Ptomedicodi == regPtomedicion.Ptomedicodi).ToList();
                    case 2://Grupo
                        return lista48Parametro.Where(x => x.Medifecha >= fechaInicio.Date && x.Medifecha <= fechaFin.Date && x.Tipoinfocodi == tipoinfocodi && x.Grupocodi == regPtomedicion.Grupocodi).ToList();
                    default:
                        return new List<MeMedicion48DTO>();
                }
            }
            else
            {
                return FactorySic.GetMeMedicion48Repository().GetByCriteria(fechaInicio, fechaFin, lectcodi.ToString(), tipoinfocodi, regPtomedicion.Ptomedicodi.ToString());
            }
        }

        /// <summary>
        /// Agregar punto a lista de total 
        /// </summary>
        /// <param name="regPto"></param>
        /// <param name="listaPto"></param>
        private void AgregarPtoAListaAllPtosFromReporte(MePtomedicionDTO regPto, ref List<MePtomedicionDTO> listaPto)
        {
            if (ConstantesReportesMedicion.PtoCalculadoSiCodigo != regPto.PtomediCalculado)
            {
                if (listaPto.Find(x => x.Ptomedicodi == regPto.PtomedicodiOrigen) == null)
                    listaPto.Add(regPto);
            }
        }

        /// <summary>
        /// Funcion recursiva que devuelve la data de un punto calculado con data de ME_SCADA_SP7
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="reg"></param>
        /// <param name="nivel"></param>
        /// <returns></returns>
        public List<MeScadaSp7DTO> GetListaCalculadaMScadaSp7Recursivo(int tipoinfocodi, DateTime fechaInicio, DateTime fechaFin, MePtomedicionDTO reg, int nivel)
        {
            List<MeScadaSp7DTO> lista = new List<MeScadaSp7DTO>();

            //Validacion del metodo recursivo
            if (ConstantesReportesMedicion.MaximoNivelRecursivo == nivel)
            {
                return lista;
            }

            //Obtencion de Data de los puntos calculados
            if (ConstantesReportesMedicion.PtoCalculadoSiCodigo == reg.PtomediCalculado)
            {
                List<MeScadaSp7DTO> listaDataXPto = new List<MeScadaSp7DTO>();
                List<MeScadaSp7DTO> listaDataXPtoCalculado = new List<MeScadaSp7DTO>();
                List<MeScadaSp7DTO> listaDataXPtoMedicion = new List<MeScadaSp7DTO>();

                List<MePtomedicionDTO> allPtos = this.ListarPtoMedicionFromCalculado(reg.Ptomedicodi);
                List<MePtomedicionDTO> ptosCalculado = allPtos.Where(x => x.PtomediCalculado == ConstantesReportesMedicion.PtoCalculadoSiCodigo).ToList();
                List<MePtomedicionDTO> ptosMedicion = allPtos.Where(x => x.PtomediCalculado != ConstantesReportesMedicion.PtoCalculadoSiCodigo).ToList();

                //Data de los puntos de medicion
                foreach (var regMed in ptosMedicion)
                {
                    var relPto = FactorySic.GetMeRelacionptoRepository().GetByCriteria(reg.Ptomedicodi.ToString(), regMed.PtomedicodiOrigen.ToString()).FirstOrDefault();
                    var listaDataXPtoMedicionTmp = FactoryScada.GetMeScadaSp7Repository().GetByCriteriaByPtoAndTipoinfocodi(fechaInicio.Date, fechaFin.Date
                        , (relPto.Tipoinfocodi > 0 ? relPto.Tipoinfocodi : reg.Tipoinfocodi.GetValueOrDefault(-1))
                        , (int)relPto.Ptomedicodi2);

                    foreach (var tmp in listaDataXPtoMedicionTmp)
                    {
                        tmp.Factor = regMed.FactorOrigen;
                        tmp.Tgenercodi = regMed.Tgenercodi;
                    }

                    listaDataXPtoMedicion.AddRange(listaDataXPtoMedicionTmp);
                }

                //Recursivo
                foreach (var ptoCalculado in ptosCalculado)
                {
                    ptoCalculado.Ptomedicodi = ptoCalculado.PtomedicodiOrigen;
                    ptoCalculado.Tipoinfocodi = ptoCalculado.TipoinfocodiOrigen.GetValueOrDefault(0);
                    var listaDataXPtoCalculadoTmp = this.GetListaCalculadaMScadaSp7Recursivo(tipoinfocodi, fechaInicio, fechaFin, ptoCalculado, nivel + 1);

                    foreach (var tmp in listaDataXPtoCalculadoTmp)
                    {
                        tmp.Factor = ptoCalculado.FactorOrigen;
                        tmp.Tgenercodi = ptoCalculado.Tgenercodi;
                    }

                    listaDataXPtoCalculado.AddRange(listaDataXPtoCalculadoTmp);
                }

                listaDataXPto.AddRange(listaDataXPtoCalculado);
                listaDataXPto.AddRange(listaDataXPtoMedicion);

                lista.AddRange(this.GetListaCalculadaMScadaSp7(reg.Ptomedicodi, fechaInicio, fechaFin, reg.Tipoinfocodi.GetValueOrDefault(-1), listaDataXPto));
            }
            else
            {
                lista.AddRange(FactoryScada.GetMeScadaSp7Repository().GetByCriteriaByPtoAndTipoinfocodi(fechaInicio, fechaFin, reg.Tipoinfocodi.GetValueOrDefault(-1), reg.Ptomedicodi));
            }

            return lista;
        }

        /// <summary>
        /// Generar calculo para punto de medicion calculado ME_MEDICION1
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="ptomedicodiCalculado"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="lectcodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <returns></returns>
        public List<MeMedicion1DTO> GetListaCalculadaM1(int ptomedicodiCalculado, int emprcodi, DateTime fechaInicio, DateTime fechaFin, int lectcodi, int tipoinfocodi)
        {
            List<MeMedicion1DTO> lista = new List<MeMedicion1DTO>();
            List<MeMedicion1DTO> listaData = new List<MeMedicion1DTO>();

            List<MePtomedicionDTO> listaPtos = this.ListarPtoMedicionFromCalculado(ptomedicodiCalculado);

            for (var f = fechaInicio.Date; f <= fechaFin.Date; f = f.AddDays(1))
            {
                listaData = new List<MeMedicion1DTO>();
                foreach (var reg in listaPtos)
                {
                    var listaTmp = FactorySic.GetMeMedicion1Repository().GetByCriteria(f, f, lectcodi, tipoinfocodi, reg.PtomedicodiOrigen.ToString());
                    foreach (var tmp in listaTmp)
                    {
                        tmp.CalculadoFactor = reg.FactorOrigen;
                    }
                    listaData.AddRange(listaTmp);
                }

                if (listaData.Count > 0)
                {
                    MeMedicion1DTO m = new MeMedicion1DTO();
                    m.Medifecha = f;
                    m.Ptomedicodi = ptomedicodiCalculado;
                    m.Emprcodi = emprcodi;
                    m.Tipoinfocodi = tipoinfocodi;
                    decimal? resultado = null;

                    foreach (var pto in listaData)
                    {
                        var factorCalculado = pto.CalculadoFactor;

                        if (pto.H1 != null)
                        {
                            resultado = resultado == null ? 0 : resultado;
                            resultado += pto.H1 * factorCalculado;
                        }

                    }

                    m.H1 = resultado;

                    lista.Add(m);
                }
            }

            return lista;
        }

        /// <summary>
        /// Generar calculo para punto de medicion calculado ME_MEDICION24
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="ptomedicodiCalculado"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="lectcodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> GetListaCalculadaM24(int ptomedicodiCalculado, int emprcodi, DateTime fechaInicio, DateTime fechaFin, int lectcodi, int tipoinfocodi)
        {
            List<MeMedicion24DTO> lista = new List<MeMedicion24DTO>();
            List<MeMedicion24DTO> listaData = new List<MeMedicion24DTO>();

            List<MePtomedicionDTO> listaPtos = this.ListarPtoMedicionFromCalculado(ptomedicodiCalculado);
           

            for (var f = fechaInicio.Date; f <= fechaFin.Date; f = f.AddDays(1))
            {
                listaData = new List<MeMedicion24DTO>();
                foreach (var reg in listaPtos)
                {
                    var listaTmp = FactorySic.GetMeMedicion24Repository().GetByCriteria(f, f, lectcodi, tipoinfocodi, reg.PtomedicodiOrigen.ToString());
                    foreach (var tmp in listaTmp)
                    {
                        tmp.CalculadoFactor = reg.FactorOrigen;
                    }
                    listaData.AddRange(listaTmp);
                }

                if (listaData.Count > 0)
                {
                    MeMedicion24DTO m = new MeMedicion24DTO();
                    m.Medifecha = f;
                    m.Ptomedicodi = ptomedicodiCalculado;
                    m.Emprcodi = emprcodi;
                    m.Tipoinfocodi = tipoinfocodi;
                    decimal? total = 0;
                    for (int i = 1; i <= 24; i++)
                    {
                        decimal? resultado = null;
                        foreach (var pto in listaData)
                        {
                            var factorCalculado = pto.CalculadoFactor;
                            decimal? valor = (decimal?)pto.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(pto, null);

                            if (valor != null)
                            {
                                resultado = resultado == null ? 0 : resultado;
                                resultado += valor * factorCalculado;
                            }
                        }
                        total += resultado == null ? 0 : resultado;
                        m.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(m, resultado);
                    }

                    m.Meditotal = total;
                    lista.Add(m);
                }
            }

            return lista;
        }

        /// <summary>
        /// Generar calculo para punto de medicion calculado ME_MEDICION48
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="ptomedicodiCalculado"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="lectcodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetListaCalculadaM48(int ptomedicodiCalculado, int emprcodi, DateTime fechaInicio, DateTime fechaFin, int tipoinfocodi, List<MeMedicion48DTO> listaDataXPto)
        {
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();

            if (ptomedicodiCalculado > 0)
            {
                for (var f = fechaInicio.Date; f <= fechaFin.Date; f = f.AddDays(1))
                {
                    var listaDataXFecha = listaDataXPto.Where(x => x.Medifecha.Date == f).ToList();
                    if (listaDataXFecha.Count > 0)
                    {
                        MeMedicion48DTO m = new MeMedicion48DTO();
                        m.Medifecha = f;
                        m.Tipoinfocodi = tipoinfocodi;
                        m.Ptomedicodi = ptomedicodiCalculado;
                        m.Emprcodi = emprcodi;
                        m.Meditotal = 0;

                        int tgenercodi = 0;
                        decimal? acumulado = 0;
                        for (int i = 1; i <= 48; i++)
                        {
                            decimal? resultado = null;
                            foreach (var pto in listaDataXFecha)
                            {
                                if (i == 1) { tgenercodi = pto.Tgenercodi; }
                                var factorCalculado = pto.Factor;
                                decimal? valor = (decimal?)pto.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(pto, null);
                                if (valor != null)
                                {
                                    resultado = resultado == null ? 0 : resultado;
                                    resultado += valor * factorCalculado;
                                }
                            }
                            acumulado += resultado == null ? 0 : resultado;//resultado.GetValueOrDefault(0);
                            m.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(m, resultado);
                        }

                        m.Meditotal = acumulado;
                        m.Tgenercodi = tgenercodi;

                        lista.Add(m);
                    }

                }
            }

            return lista;
        }

        /// <summary>
        /// Generar calculo para punto de medicion calculado ME_MEDICION96
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="ptomedicodiCalculado"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="lectcodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> GetListaCalculadaM96(int ptomedicodiCalculado, int emprcodi, DateTime fechaInicio, DateTime fechaFin, int lectcodi, int tipoinfocodi)
        {
            List<MeMedicion96DTO> lista = new List<MeMedicion96DTO>();
            List<MeMedicion96DTO> listaData = new List<MeMedicion96DTO>();

            List<MePtomedicionDTO> listaPtos = this.ListarPtoMedicionFromCalculado(ptomedicodiCalculado);

            for (var f = fechaInicio.Date; f <= fechaFin.Date; f = f.AddDays(1))
            {
                listaData = new List<MeMedicion96DTO>();
                foreach (var reg in listaPtos)
                {
                    var listaTmp = FactorySic.GetMeMedicion96Repository().GetByCriteria(f, f, lectcodi, tipoinfocodi, reg.PtomedicodiOrigen);
                    foreach (var tmp in listaTmp)
                    {
                        tmp.Factor = reg.FactorOrigen;
                    }
                    listaData.AddRange(listaTmp);
                }

                if (listaData.Count > 0)
                {
                    MeMedicion96DTO m = new MeMedicion96DTO();
                    m.Medifecha = f;
                    m.Ptomedicodi = ptomedicodiCalculado;
                    m.Emprcodi = emprcodi;
                    m.Tipoinfocodi = tipoinfocodi;

                    for (int i = 1; i <= 96; i++)
                    {
                        decimal? resultado = null;

                        foreach (var pto in listaData)
                        {
                            var factorCalculado = pto.Factor;
                            decimal? valor = (decimal?)pto.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(pto, null);

                            if (valor != null)
                            {
                                resultado = resultado == null ? 0 : resultado;
                                resultado += valor * factorCalculado;
                            }
                        }

                        m.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(m, resultado);
                    }

                    lista.Add(m);
                }
            }

            return lista;
        }

        /// <summary>
        /// Generar calculo para punto de medicion calculado ME_SCADA_SP7
        /// </summary>
        /// <param name="ptomedicodiCalculado"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="listaDataXPto"></param>
        /// <returns></returns>
        public List<MeScadaSp7DTO> GetListaCalculadaMScadaSp7(int ptomedicodiCalculado, DateTime fechaInicio, DateTime fechaFin, int tipoinfocodi, List<MeScadaSp7DTO> listaDataXPto)
        {
            List<MeScadaSp7DTO> lista = new List<MeScadaSp7DTO>();

            if (ptomedicodiCalculado > 0)
            {
                for (var f = fechaInicio.Date; f <= fechaFin.Date; f = f.AddDays(1))
                {
                    var listaDataXFecha = listaDataXPto.Where(x => x.Medifecha.Date == f).ToList();
                    if (listaDataXFecha.Count > 0)
                    {
                        MeScadaSp7DTO m = new MeScadaSp7DTO();
                        m.Medifecha = f;
                        m.Tipoinfocodi = tipoinfocodi;
                        m.Ptomedicodi = ptomedicodiCalculado;
                        m.Meditotal = 0;

                        int tgenercodi = 0;
                        decimal? acumulado = null;
                        for (int i = 1; i <= 96; i++)
                        {
                            decimal? resultado = null;
                            foreach (var pto in listaDataXFecha)
                            {
                                if (i == 1) { tgenercodi = pto.Tgenercodi; }
                                var factorCalculado = pto.Factor;
                                decimal? valor = (decimal?)pto.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(pto, null);
                                if (valor != null)
                                {
                                    resultado = resultado == null ? 0 : resultado;
                                    resultado += valor * factorCalculado;
                                }
                            }
                            acumulado += resultado.GetValueOrDefault(0);
                            m.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(m, resultado);
                        }

                        m.Meditotal = acumulado;
                        m.Tgenercodi = tgenercodi;

                        lista.Add(m);
                    }

                }
            }

            return lista;
        }

        /// <summary>
        /// Total de registros del reporte
        /// </summary>
        /// <param name="idReporte"></param>
        /// <param name="lectnro"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public int ObtenerTotalPaginacionReporte(int idReporte, DateTime fechaInicio, DateTime fechaFin)
        {
            var reporte = GetByIdReporte(idReporte);
            int lecturaReporte = (int)reporte.Lectcodi;
            var lectura = GetByIdMeLectura(lecturaReporte);

            if (lecturaReporte == -1)
            {
                return -1;
            }
            else
            {
                int total = FactorySic.GetMeReporptomedRepository().PaginacionReporte(idReporte, lectura.Lectnro.Value, fechaInicio, fechaFin).Count();
                if (lectura.Lectnro.Value == 1)
                {
                    total = total % 30 == 0 ? total / 30 : total / 30 + 1;
                }

                return total;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="cabecera"></param>
        /// <returns></returns>
        public object GetInfoMeReporte(int reporcodi, DateTime fechaInicio, DateTime fechaFin, int ptomedicodi)
        {
            object lista = null;
            var obj = FactorySic.GetMeReporptomedRepository().GetById2(reporcodi);
            if (obj != null)
            {
                var cabecera = FactorySic.GetMeReporptomedRepository().GetByCriteria(reporcodi, ptomedicodi);

                if (cabecera.Count > 0)
                {
                    int nroPagina = 1;

                    var lectura = GetByIdMeLectura((int)cabecera[0].Lectcodi);
                    switch (lectura.Lectnro)
                    {
                        case 1: lista = GetListaDataM1FromMeReporptomed(reporcodi, lectura.Lectcodi, fechaInicio, fechaFin, nroPagina, cabecera); break;
                        case 24: lista = GetListaDataM24FromMeReporptomed(reporcodi, lectura.Lectcodi, fechaInicio, fechaFin, nroPagina, cabecera); break;
                        case 48:
                            List<MePtomedicionDTO> listaPto = new List<MePtomedicionDTO>();
                            lista = GetListaDataM48FromMeReporptomed(lectura.Lectcodi, fechaInicio, fechaFin, cabecera, false, 0, new List<int>(), new List<MeMedicion48DTO>(), ref listaPto); break;
                        case 96: lista = GetListaDataM96FromMeReporptomed(reporcodi, lectura.Lectcodi, fechaInicio, fechaFin, nroPagina, cabecera); break;
                    }
                }
            }

            return lista;
        }

        #region Conversion de Datos

        /// <summary>
        /// Convertir de 48 a 24 
        /// </summary>
        /// <param name="dfechaIni"></param>
        /// <param name="dfechaFin"></param>
        /// <param name="lista48Input"></param>
        /// <param name="tipoDatoSalida"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> GetDataLista24From48(DateTime dfechaIni, DateTime dfechaFin, List<MeMedicion48DTO> lista48Input, int tipoDatoSalida, int tipoTotalSalida, int tipoValidarCero)
        {
            List<MeMedicion1DTO> lista1Final = new List<MeMedicion1DTO>();
            List<MeMedicion24DTO> lista24Final = new List<MeMedicion24DTO>();
            List<MeMedicion48DTO> lista48Final = new List<MeMedicion48DTO>();
            List<MeMedicion96DTO> lista96Final = new List<MeMedicion96DTO>();

            this.GetListaFinalPorResolucion(dfechaIni, dfechaFin, ConstantesReportesMedicion.FuenteMediciones, ParametrosFormato.ResolucionMediaHora, ParametrosFormato.ResolucionHora, tipoDatoSalida, tipoTotalSalida, tipoTotalSalida
                , null, null, lista48Input, null, null
                , out lista1Final, out lista24Final, out lista48Final, out lista96Final);

            return lista24Final;
        }

        /// <summary>
        /// Convertir Scada 96 a Medicion48
        /// </summary>
        /// <param name="dfechaIni"></param>
        /// <param name="dfechaFin"></param>
        /// <param name="listaScada96Input"></param>
        /// <param name="tipoDatoSalida"></param>
        /// <param name="tipoTotalSalida"></param>
        /// <param name="tipoValidarCero"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetDataLista48FromScada96(DateTime dfechaIni, DateTime dfechaFin, List<MeScadaSp7DTO> listaScada96Input, int tipoDatoSalida, int tipoTotalSalida, int tipoValidarCero)
        {
            List<MeMedicion1DTO> lista1Final = new List<MeMedicion1DTO>();
            List<MeMedicion24DTO> lista24Final = new List<MeMedicion24DTO>();
            List<MeMedicion48DTO> lista48Final = new List<MeMedicion48DTO>();
            List<MeMedicion96DTO> lista96Final = new List<MeMedicion96DTO>();

            this.GetListaFinalPorResolucion(dfechaIni, dfechaFin, ConstantesReportesMedicion.FuenteScada, ParametrosFormato.ResolucionCuartoHora, ParametrosFormato.ResolucionMediaHora, tipoDatoSalida, tipoTotalSalida, tipoTotalSalida
                , null, null, null, null, listaScada96Input
                , out lista1Final, out lista24Final, out lista48Final, out lista96Final);

            return lista48Final;
        }

        /// <summary>
        /// Método para convertir los datos
        /// </summary>
        /// <param name="dfechaIni"></param>
        /// <param name="dfechaFin"></param>
        /// <param name="resolucionOrigen"></param>
        /// <param name="resolucionDestino"></param>
        /// <param name="tipoDatoSalida"></param>
        /// <param name="tipoTotalSalida"></param>
        /// <param name="validarCero"></param>
        /// <param name="lista1Input"></param>
        /// <param name="lista24Input"></param>
        /// <param name="lista48Input"></param>
        /// <param name="lista96Input"></param>
        /// <param name="lista1Output"></param>
        /// <param name="lista24Output"></param>
        /// <param name="lista48Output"></param>
        /// <param name="lista96Output"></param>
        private void GetListaFinalPorResolucion(DateTime dfechaIni, DateTime dfechaFin, int fuenteDatosOrigen, int resolucionOrigen, int resolucionDestino, int tipoDatoSalida, int tipoTotalSalida, int validarCero
            , List<MeMedicion1DTO> lista1Input, List<MeMedicion24DTO> lista24Input, List<MeMedicion48DTO> lista48Input, List<MeMedicion96DTO> lista96Input, List<MeScadaSp7DTO> listaScada96Input
            , out List<MeMedicion1DTO> lista1Output, out List<MeMedicion24DTO> lista24Output, out List<MeMedicion48DTO> lista48Output, out List<MeMedicion96DTO> lista96Output)
        {
            List<decimal> listaData = new List<decimal>();

            List<MeMedicion1DTO> lista1Final = new List<MeMedicion1DTO>();
            List<MeMedicion24DTO> lista24Final = new List<MeMedicion24DTO>();
            List<MeMedicion48DTO> lista48Final = new List<MeMedicion48DTO>();
            List<MeMedicion96DTO> lista96Final = new List<MeMedicion96DTO>();

            int equivalencia = 0;
            if (tipoDatoSalida != resolucionDestino)
            {
                equivalencia = ConstantesReportesMedicion.DatoPromedio;
            }

            //datos para el grafico
            switch (resolucionDestino)
            {
                case ParametrosFormato.ResolucionCuartoHora:
                    switch (resolucionOrigen)
                    {
                        case ParametrosFormato.ResolucionCuartoHora: //De 96 a 96
                            lista96Final = lista96Input;
                            break;
                        case ParametrosFormato.ResolucionMediaHora: //De 48 a 96

                            for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
                            {
                                var listaDataXDay = lista48Input.Where(x => x.Medifecha.Date == day);

                                foreach (var m in listaDataXDay)
                                {
                                    listaData = new List<decimal>();
                                    MeMedicion96DTO gr = new MeMedicion96DTO();
                                    for (int i = 1; i <= 96; i += 2)
                                    {
                                        var numeroTiempo = i / 2 + 1;
                                        decimal? valor = (decimal?)m.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(m, null);
                                        listaData.Add(valor.GetValueOrDefault(0));

                                        gr.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(gr, valor);
                                        gr.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i + 1)).SetValue(gr, valor);
                                    }
                                    #region mapeo de campos
                                    gr.Meditotal = listaData.Sum(x => x);
                                    gr.Medifecha = day;
                                    gr.Ptomedicodi = m.Ptomedicodi;
                                    #endregion
                                    lista96Final.Add(gr);
                                }
                            }
                            break;
                        case ParametrosFormato.ResolucionHora:
                            //TODO
                            break;
                    }
                    break;
                case ParametrosFormato.ResolucionMediaHora:
                    switch (resolucionOrigen)
                    {
                        case ParametrosFormato.ResolucionCuartoHora: //De 96 a 48
                            switch (equivalencia)
                            {
                                case ConstantesReportesMedicion.DatoPromedio:
                                    for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
                                    {
                                        switch (fuenteDatosOrigen)
                                        {
                                            case ConstantesReportesMedicion.FuenteMediciones:

                                                var listaDataXDay = lista96Input.Where(x => x.Medifecha.Value.Date == day);

                                                foreach (var m in listaDataXDay)
                                                {
                                                    listaData = new List<decimal>();
                                                    MeMedicion48DTO gr = new MeMedicion48DTO();
                                                    for (int i = 1; i <= 48; i++)
                                                    {
                                                        var numeroTiempoIni = i * 2 - 1;
                                                        var numeroTiempoFin = i * 2;
                                                        decimal? valor1 = (decimal?)m.GetType().GetProperty(ConstantesAppServicio.CaracterH + numeroTiempoIni).GetValue(m, null);
                                                        decimal? valor2 = (decimal?)m.GetType().GetProperty(ConstantesAppServicio.CaracterH + numeroTiempoFin).GetValue(m, null);
                                                        decimal? resultado = (valor1.GetValueOrDefault(0) + valor2.GetValueOrDefault(0)) / 2;
                                                        listaData.Add(resultado.GetValueOrDefault(0));

                                                        gr.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(gr, resultado);
                                                    }
                                                    #region mapeo de campos
                                                    gr.Meditotal = listaData.Sum(x => x);
                                                    gr.Medifecha = day;
                                                    gr.Ptomedicodi = m.Ptomedicodi;
                                                    #endregion
                                                    lista48Final.Add(gr);
                                                }
                                                break;
                                            case ConstantesReportesMedicion.FuenteScada:
                                                var listaDataScadaXDay = listaScada96Input.Where(x => x.Medifecha.Date == day);

                                                foreach (var m in listaDataScadaXDay)
                                                {
                                                    listaData = new List<decimal>();
                                                    MeMedicion48DTO gr = new MeMedicion48DTO();
                                                    for (int i = 1; i <= 48; i++)
                                                    {
                                                        var numeroTiempoIni = i * 2 - 1;
                                                        var numeroTiempoFin = i * 2;
                                                        decimal? valor1 = (decimal?)m.GetType().GetProperty(ConstantesAppServicio.CaracterH + numeroTiempoIni).GetValue(m, null);
                                                        decimal? valor2 = (decimal?)m.GetType().GetProperty(ConstantesAppServicio.CaracterH + numeroTiempoFin).GetValue(m, null);
                                                        decimal? resultado = (valor1.GetValueOrDefault(0) + valor2.GetValueOrDefault(0)) / 2;
                                                        listaData.Add(resultado.GetValueOrDefault(0));

                                                        gr.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(gr, resultado);
                                                    }
                                                    #region mapeo de campos
                                                    gr.Meditotal = listaData.Sum(x => x);
                                                    gr.Medifecha = day;
                                                    gr.Ptomedicodi = m.Ptomedicodi;
                                                    #endregion
                                                    lista48Final.Add(gr);
                                                }
                                                break;
                                        }

                                    }
                                    break;
                            }
                            break;
                        case ParametrosFormato.ResolucionMediaHora: //De 48 a 48
                            lista48Final = lista48Input;
                            break;
                        case ParametrosFormato.ResolucionHora:
                            //TODO
                            break;
                    }
                    break;
                case ParametrosFormato.ResolucionHora:
                    switch (resolucionOrigen)
                    {
                        case ParametrosFormato.ResolucionCuartoHora://De 96 a 24
                            switch (equivalencia)
                            {
                                case ConstantesReportesMedicion.DatoHorario:
                                    for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
                                    {
                                        var listaDataXDay = lista96Input.Where(x => x.Medifecha.Value.Date == day);

                                        foreach (var m in listaDataXDay)
                                        {
                                            listaData = new List<decimal>();
                                            MeMedicion96DTO gr = new MeMedicion96DTO();
                                            for (int i = 1; i <= 24; i++)
                                            {
                                                var numeroTiempo = i * 4;
                                                decimal? valor = (decimal?)m.GetType().GetProperty(ConstantesAppServicio.CaracterH + numeroTiempo).GetValue(m, null);
                                                listaData.Add(valor.GetValueOrDefault(0));

                                                gr.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(gr, valor);
                                            }
                                            #region mapeo de campos
                                            gr.Meditotal = listaData.Sum(x => x);
                                            gr.Medifecha = day;
                                            gr.Ptomedicodi = m.Ptomedicodi;
                                            #endregion
                                            lista96Final.Add(gr);
                                        }
                                    }

                                    break;
                                case ConstantesReportesMedicion.DatoPromedio:
                                    for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
                                    {
                                        var listaDataXDay = lista96Input.Where(x => x.Medifecha.Value.Date == day);

                                        foreach (var m in listaDataXDay)
                                        {
                                            listaData = new List<decimal>();
                                            MeMedicion96DTO gr = new MeMedicion96DTO();
                                            for (int i = 1; i <= 24; i++)
                                            {
                                                var numeroTiempoIni = i * 4 - 3;
                                                var numeroTiempoFin = i * 4;


                                                decimal? valor1 = (decimal?)m.GetType().GetProperty(ConstantesAppServicio.CaracterH + numeroTiempoIni).GetValue(m, null);
                                                decimal? valor2 = (decimal?)m.GetType().GetProperty(ConstantesAppServicio.CaracterH + numeroTiempoFin).GetValue(m, null);
                                                decimal? resultado = (valor1.GetValueOrDefault(0) + valor2.GetValueOrDefault(0)) / 2;
                                                listaData.Add(resultado.GetValueOrDefault(0));

                                                gr.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(gr, resultado);
                                            }
                                            #region mapeo de campos
                                            gr.Meditotal = listaData.Sum(x => x);
                                            gr.Medifecha = day;
                                            gr.Ptomedicodi = m.Ptomedicodi;
                                            #endregion
                                            lista96Final.Add(gr);
                                        }
                                    }
                                    break;
                            }
                            break;
                        case ParametrosFormato.ResolucionMediaHora: //De 48 a 24
                            switch (equivalencia)
                            {
                                case ConstantesReportesMedicion.DatoHorario:
                                    for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
                                    {
                                        var listaDataXDay = lista48Input.Where(x => x.Medifecha.Date == day);

                                        foreach (var m in listaDataXDay)
                                        {
                                            listaData = new List<decimal>();
                                            MeMedicion24DTO gr = new MeMedicion24DTO();
                                            for (int i = 1; i <= 24; i++)
                                            {
                                                var numeroTiempo = i * 2;

                                                decimal? valor = (decimal?)m.GetType().GetProperty(ConstantesAppServicio.CaracterH + numeroTiempo).GetValue(m, null);
                                                listaData.Add(valor.GetValueOrDefault(0));

                                                gr.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(gr, valor);
                                            }
                                            #region mapeo de campos
                                            gr.Meditotal = listaData.Sum(x => x);
                                            gr.Medifecha = day;
                                            gr.Ptomedicodi = m.Ptomedicodi;
                                            gr.Fenergcodi = m.Fenergcodi;
                                            #endregion
                                            lista24Final.Add(gr);
                                        }
                                    }

                                    break;
                                case ConstantesReportesMedicion.DatoPromedio:
                                    for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
                                    {
                                        var listaDataXDay = lista48Input.Where(x => x.Medifecha.Date == day);

                                        foreach (var m in listaDataXDay)
                                        {
                                            listaData = new List<decimal>();
                                            MeMedicion24DTO gr = new MeMedicion24DTO();
                                            for (int i = 1; i <= 24; i++)
                                            {
                                                var numeroTiempoIni = i * 2 - 1;
                                                var numeroTiempoFin = i * 2;

                                                decimal? valor1 = (decimal?)m.GetType().GetProperty(ConstantesAppServicio.CaracterH + numeroTiempoIni).GetValue(m, null);
                                                decimal? valor2 = (decimal?)m.GetType().GetProperty(ConstantesAppServicio.CaracterH + numeroTiempoFin).GetValue(m, null);
                                                decimal? resultado = (valor1.GetValueOrDefault(0) + valor2.GetValueOrDefault(0)) / 2;
                                                if (ConstantesReportesMedicion.ValidarCeroSi == validarCero)
                                                {
                                                    if ((valor1.GetValueOrDefault(0) != 0 || valor2.GetValueOrDefault(0) != 0)
                                                        && !(valor1.GetValueOrDefault(0) != 0 && valor2.GetValueOrDefault(0) != 0)) //tiene al menos uno de los 2 valores un cero
                                                    {
                                                        resultado = (valor1.GetValueOrDefault(0) + valor2.GetValueOrDefault(0));
                                                    }
                                                }

                                                if (ConstantesReportesMedicion.TotalPromedio == tipoTotalSalida)
                                                {
                                                    if (resultado.GetValueOrDefault(0) != 0)
                                                    {
                                                        listaData.Add(resultado.GetValueOrDefault(0));
                                                    }
                                                }
                                                else
                                                {
                                                    listaData.Add(resultado.GetValueOrDefault(0));
                                                }

                                                gr.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(gr, resultado);

                                            }
                                            #region mapeo de campos
                                            gr.Meditotal = ConstantesReportesMedicion.TotalPromedio == tipoTotalSalida ? (listaData.Count > 0 ? listaData.Average(x => x) : 0) : listaData.Sum(x => x);
                                            gr.Medifecha = day;
                                            gr.Ptomedicodi = m.Ptomedicodi;
                                            gr.Fenergcodi = m.Fenergcodi;
                                            #endregion
                                            lista24Final.Add(gr);
                                        }
                                    }
                                    break;
                            }
                            break;
                        case ParametrosFormato.ResolucionHora: //De 24 a 24
                            lista24Final = lista24Input;
                            break;
                    }
                    break;
            }

            //
            lista1Output = lista1Final;
            lista24Output = lista24Final;
            lista48Output = lista48Final;
            lista96Output = lista96Final;
        }

        /// <summary>
        /// Listar frecuencia de la información de los puntos de medición
        /// </summary>
        /// <returns></returns>
        public List<TipoInformacion> ListarResolucionPto()
        {
            List<TipoInformacion> l = new List<TipoInformacion>();
            TipoInformacion obj;
            obj = new TipoInformacion() { IdTipoInfo = ConstantesAppServicio.Periodo1, NombreTipoInfo = "1 día" };
            l.Add(obj);
            obj = new TipoInformacion() { IdTipoInfo = ConstantesAppServicio.Periodo24, NombreTipoInfo = "1 hora" };
            l.Add(obj);
            obj = new TipoInformacion() { IdTipoInfo = ConstantesAppServicio.Periodo48, NombreTipoInfo = "30 minutos" };
            l.Add(obj);
            obj = new TipoInformacion() { IdTipoInfo = ConstantesAppServicio.Periodo96, NombreTipoInfo = "15 minutos" };
            l.Add(obj);

            return l;
        }

        #endregion

        #region HTML

        /// <summary>
        /// Rango de fechas para el reporte segun numero de pagina
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="lectnro"></param>
        /// <param name="fInicio"></param>
        /// <param name="fFin"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        public DateTime?[] GetListaFechaFromReporptomed(int reporcodi, int lectnro, DateTime fInicio, DateTime fFin, int nroPagina)
        {
            DateTime?[] listaFecha = new DateTime?[2];
            DateTime? fechaInicio = null;
            DateTime? fechaFin = null;

            var paginas = FactorySic.GetMeReporptomedRepository().PaginacionReporte(reporcodi, lectnro, fInicio, fFin);
            int totalPagina = paginas.Count();
            if (totalPagina > 0)
            {
                if (nroPagina == -1)
                {
                    fechaInicio = paginas.First();
                    fechaFin = paginas.Last();
                }
                else
                {
                    int nroPaginaIni = lectnro == 1 ? (nroPagina - 1) * 30 + 1 : nroPagina;
                    int nroPaginaFin = lectnro == 1 ? nroPagina * 30 : nroPagina;
                    nroPaginaFin = nroPaginaFin > totalPagina ? totalPagina : nroPaginaFin;

                    fechaInicio = paginas[nroPaginaIni - 1];
                    fechaFin = paginas[nroPaginaFin - 1];
                }
            }

            listaFecha[0] = fechaInicio;
            listaFecha[1] = fechaFin;

            return listaFecha;
        }

        /// <summary>
        /// Generar reporte Html
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="empresas"></param>
        /// <param name="fInicio"></param>
        /// <param name="fFin"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        public string GenerarReporteHtml(int reporcodi, DateTime fInicio, DateTime fFin, int nroPagina, string emprcodi, string famcodi, string equicodi, string tipoMedida)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            StringBuilder strHtml = new StringBuilder();

            var reporte = GetByIdReporte(reporcodi);
            var lectura = GetByIdMeLectura((int)reporte.Lectcodi);
            MeCabeceraDTO cabecera = new MeCabeceraDTO();
            cabecera.Cabcolumnas = 1;
            cabecera.Cabfilas = 4;
            cabecera.Cabcampodef = "Emprnomb,EMPRESA,1#Famnomb,TIPO EQUIPO,0#Equinomb,EQUIPO,0#Tipoinfoabrev,FECHA HORA  \\   UNIDAD,0";


            if (cabecera != null)
            {
                List<DateTime> listFechas = new List<DateTime>();
                for (var f = fInicio.Date; f <= fFin; f = f.AddDays(1))
                {
                    listFechas.Add(f);
                }
                DateTime fechaPeriodo = listFechas[nroPagina - 1];
                List<MeReporptomedDTO> listaPtos = GetListaPuntoFromMeReporptomedByFecha(reporcodi, emprcodi, famcodi, equicodi, tipoMedida, fechaPeriodo);

                if (listaPtos.Count > 0)
                {
                    strHtml.Append("<table border='1' class='pretty tabla-icono' cellspacing='0' width='100%' id='tabla'>");
                    strHtml.Append("<thead>");
                    #region cabecera reporte
                    int columnas = cabecera.Cabcolumnas;
                    int filas = cabecera.Cabfilas;
                    var cabecerasRow = cabecera.Cabcampodef.Split(QueryParametros.SeparadorFila);
                    List<CabeceraRow> listaCabeceraRow = new List<CabeceraRow>();
                    for (var x = 0; x < cabecerasRow.Length; x++)
                    {
                        var reg = new CabeceraRow();
                        var fila = cabecerasRow[x].Split(QueryParametros.SeparadorCol);
                        reg.NombreRow = fila[0];
                        reg.TituloRow = fila[1];
                        reg.IsMerge = int.Parse(fila[2]);
                        listaCabeceraRow.Add(reg);
                    }
                    for (var j = 0; j < filas; j++)
                    {
                        strHtml.Append("<tr>");
                        var fila = "";
                        fila = "<th>";
                        fila += listaCabeceraRow[j].TituloRow;
                        fila += "</th>";
                        strHtml.Append(fila);
                        foreach (var reg in listaPtos)
                        {
                            fila = "<th>";
                            fila += (string)reg.GetType().GetProperty(listaCabeceraRow[j].NombreRow).GetValue(reg, null);
                            fila += "</th>";
                            strHtml.Append(fila);
                        }
                        strHtml.Append("</tr>");
                    }
                    #endregion
                    strHtml.Append("</thead>");
                    strHtml.Append("<tbody>");
                    #region cuerpo reporte

                    DateTime?[] listaFecha = this.GetListaFechaFromReporptomed(reporcodi, lectura.Lectnro.Value, fInicio, fFin, nroPagina);
                    if (listaFecha[0] != null && listaFecha[1] != null)
                    {
                        DateTime fechaInicio = listaFecha[0].Value;
                        DateTime fechaFin = listaFecha[1].Value;

                        List<MeMedicion1DTO> lista1 = new List<MeMedicion1DTO>();
                        List<MeMedicion24DTO> lista24 = new List<MeMedicion24DTO>();
                        List<MeMedicion48DTO> lista48 = new List<MeMedicion48DTO>();
                        List<MeMedicion96DTO> lista96 = new List<MeMedicion96DTO>();

                        switch (lectura.Lectnro)
                        {
                            case 1:
                                lista1 = GetListaDataM1FromMeReporptomed(reporcodi, lectura.Lectcodi, fechaInicio, fechaFin, nroPagina, listaPtos);
                                break;
                            case 24:
                                lista24 = GetListaDataM24FromMeReporptomed(reporcodi, lectura.Lectcodi, fechaInicio, fechaFin, nroPagina, listaPtos);
                                break;
                            case 48:
                                List<MePtomedicionDTO> listaPto = new List<MePtomedicionDTO>();
                                lista48 = GetListaDataM48FromMeReporptomed(lectura.Lectcodi, fechaInicio, fechaFin, listaPtos, false, 0, new List<int>(), new List<MeMedicion48DTO>(), ref listaPto);
                                break;
                            case 96:
                                lista96 = GetListaDataM96FromMeReporptomed(reporcodi, lectura.Lectcodi, fechaInicio, fechaFin, nroPagina, listaPtos);
                                break;
                        }

                        strHtml.Append(GenerarHtmlBody(listaPtos, fechaInicio, fechaFin, nfi, lectura.Lectnro.Value, lista1, lista24, lista48, lista96));
                    }

                    #endregion
                    strHtml.Append("</tbody>");
                    strHtml.Append("</table>");
                }
            }
            return strHtml.ToString();
        }

        /// <summary>
        /// Generar cuerpo de la tabla del reporte
        /// </summary>
        /// <param name="listaPtos"></param>
        /// <param name="fInicio"></param>
        /// <param name="fFin"></param>
        /// <param name="nfi"></param>
        /// <param name="lectnro"></param>
        /// <param name="lista1"></param>
        /// <param name="lista24"></param>
        /// <param name="lista48"></param>
        /// <param name="lista96"></param>
        /// <returns></returns>
        private string GenerarHtmlBody(List<MeReporptomedDTO> listaPtos, DateTime fInicio, DateTime fFin, NumberFormatInfo nfi
            , int lectnro, List<MeMedicion1DTO> lista1, List<MeMedicion24DTO> lista24, List<MeMedicion48DTO> lista48, List<MeMedicion96DTO> lista96)
        {
            StringBuilder strHtml = new StringBuilder();
            int addMinutos = 0;
            string formatoFecha = ConstantesBase.FormatoFechaFullBase;

            switch (lectnro)
            {
                case 1:
                    formatoFecha = ConstantesBase.FormatoFechaBase;
                    break;
                case 24:
                    addMinutos = 60;
                    break;
                case 48:
                    addMinutos = 30;
                    break;
                case 96:
                    addMinutos = 15;
                    break;
            }

            for (var f = fInicio.Date; f <= fFin.Date; f = f.AddDays(1))
            {
                DateTime horas = f.Date;
                decimal? valor = null;

                for (int h = 1; h <= lectnro; h++)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td class='tdbody_reporte'>{0}</td>", horas.ToString(formatoFecha)));

                    foreach (var reg in listaPtos)
                    {
                        valor = null;
                        switch (lectnro)
                        {
                            case 1:
                                var m1 = lista1.Find(x => x.Medifecha.Date == f && x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Emprcodi == reg.Emprcodi);
                                valor = m1 != null ? (decimal?)m1.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m1, null) : null;
                                break;
                            case 24:
                                var m24 = lista24.Find(x => x.Medifecha.Date == f && x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Emprcodi == reg.Emprcodi);
                                valor = m24 != null ? (decimal?)m24.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m24, null) : null;
                                break;
                            case 48:
                                var m48 = lista48.Find(x => x.Medifecha.Date == f && x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Emprcodi == reg.Emprcodi);
                                valor = m48 != null ? (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null) : null;
                                break;
                            case 96:
                                var m96 = lista96.Find(x => x.Medifecha.Value.Date == f && x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi && x.Emprcodi == reg.Emprcodi);
                                valor = m96 != null ? (decimal?)m96.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m96, null) : null;
                                break;
                        }

                        if (valor != null)
                        {
                            strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                        }
                        else
                        {
                            strHtml.Append(string.Format("<td></td>"));
                        }
                    }

                    strHtml.Append("</tr>");

                    horas = horas.AddMinutes(addMinutos);
                }
            }

            return strHtml.ToString();
        }

        #endregion

        #region Exportacion

        /// <summary>
        /// Genera Archivo excel  de los Puntos calculados y su detalle
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="empresa"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public string GenerarFileExcelReportePuntoCalculado(List<MePtomedicionDTO> listaPtoCalculado, List<MePtomedicionDTO> listaPtoDetalle)
        {
            string fileExcel = string.Empty;

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet wsHist = xlPackage.Workbook.Worksheets.Add(ConstantesIEOD.HojaExcelPtoCalculado);
                wsHist.View.ShowGridLines = false;
                //wsHist.View.FreezePanes(6, 2);

                #region Titulo

                int row = 3;
                int column = 2;

                int rowTitulo = 2;
                wsHist.Cells[rowTitulo, column + 2].Value = ConstantesIEOD.TituloExcelPtoCalculado;
                wsHist.Cells[rowTitulo, column + 2].Style.Font.SetFromFont(new Font("Calibri", 14));
                wsHist.Cells[rowTitulo, column + 2].Style.Font.Bold = true;

                row += 2;
                #region Cabecera

                int colIniCalCodigo = 2;
                int colIniCalEmpresa = colIniCalCodigo + 1;
                int colIniCalEquipo = colIniCalEmpresa + 1;
                int colIniCalDesc = colIniCalEquipo + 1;
                int colIniCalMedidad = colIniCalDesc + 1;

                int colIniDetCodigo = colIniCalMedidad + 1;
                int colIniDetEmpresa = colIniDetCodigo + 1;
                int colIniDetEquipo = colIniDetEmpresa + 1;
                int colIniDetDesc = colIniDetEquipo + 1;
                int colIniDetLectura = colIniDetDesc + 1;
                int colIniDetTipo = colIniDetLectura + 1;
                int colIniDetFactor = colIniDetTipo + 1;

                wsHist.Cells[row, colIniCalCodigo].Value = "Código";
                wsHist.Cells[row, colIniCalEmpresa].Value = "Empresa";
                wsHist.Cells[row, colIniCalEquipo].Value = "Equipo";
                wsHist.Cells[row, colIniCalDesc].Value = "Punto Calculado";
                wsHist.Cells[row, colIniCalMedidad].Value = "Medida";

                wsHist.Cells[row, colIniDetCodigo].Value = "Código";
                wsHist.Cells[row, colIniDetEmpresa].Value = "Empresa";
                wsHist.Cells[row, colIniDetEquipo].Value = "Equipo";
                wsHist.Cells[row, colIniDetDesc].Value = "Punto Medición";
                wsHist.Cells[row, colIniDetLectura].Value = "Lectura";
                wsHist.Cells[row, colIniDetTipo].Value = "Tipo";
                wsHist.Cells[row, colIniDetFactor].Value = "Factor";

                wsHist.Cells[row, colIniCalCodigo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniCalEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniCalEquipo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniCalDesc].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniCalMedidad].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                wsHist.Cells[row, colIniDetCodigo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniDetEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniDetEquipo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniDetDesc].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniDetLectura].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniDetTipo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniDetFactor].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                using (var range = wsHist.Cells[row, colIniCalCodigo, row, colIniCalMedidad])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                }
                using (var range = wsHist.Cells[row, colIniDetCodigo, row, colIniDetFactor])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFBF00"));
                    range.Style.Font.Color.SetColor(Color.White);
                }

                #endregion

                #region cuerpo
                int rowIni = row;

                listaPtoCalculado = listaPtoCalculado.OrderBy(x => x.Ptomedicodi).ToList();
                foreach (var reg in listaPtoCalculado)
                {
                    row++;
                    List<MePtomedicionDTO> listaPtoDetalleByCal = listaPtoDetalle.Where(x => x.PtomedicodiCalculado == reg.Ptomedicodi).ToList();
                    int totalFila = listaPtoDetalleByCal.Count > 0 ? listaPtoDetalleByCal.Count : 1;

                    wsHist.Cells[row, colIniCalCodigo].Value = reg.Ptomedicodi;
                    wsHist.Cells[row, colIniCalCodigo, row + totalFila - 1, colIniCalCodigo].Merge = true;
                    wsHist.Cells[row, colIniCalCodigo, row + totalFila - 1, colIniCalCodigo].Style.WrapText = true;
                    wsHist.Cells[row, colIniCalCodigo, row + totalFila - 1, colIniCalCodigo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniCalCodigo, row + totalFila - 1, colIniCalCodigo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsHist.Cells[row, colIniCalCodigo, row + totalFila - 1, colIniCalCodigo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    wsHist.Cells[row, colIniCalEmpresa].Value = reg.Emprnomb.Trim();
                    wsHist.Cells[row, colIniCalEmpresa, row + totalFila - 1, colIniCalEmpresa].Merge = true;
                    wsHist.Cells[row, colIniCalEmpresa, row + totalFila - 1, colIniCalEmpresa].Style.WrapText = true;
                    wsHist.Cells[row, colIniCalEmpresa, row + totalFila - 1, colIniCalEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniCalEmpresa, row + totalFila - 1, colIniCalEmpresa].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    wsHist.Cells[row, colIniCalEquipo].Value = reg.Equinomb.Trim();
                    wsHist.Cells[row, colIniCalEquipo, row + totalFila - 1, colIniCalEquipo].Merge = true;
                    wsHist.Cells[row, colIniCalEquipo, row + totalFila - 1, colIniCalEquipo].Style.WrapText = true;
                    wsHist.Cells[row, colIniCalEquipo, row + totalFila - 1, colIniCalEquipo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniCalEquipo, row + totalFila - 1, colIniCalEquipo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    wsHist.Cells[row, colIniCalDesc].Value = reg.Ptomedidesc;
                    wsHist.Cells[row, colIniCalDesc, row + totalFila - 1, colIniCalDesc].Merge = true;
                    wsHist.Cells[row, colIniCalDesc, row + totalFila - 1, colIniCalDesc].Style.WrapText = true;
                    wsHist.Cells[row, colIniCalDesc, row + totalFila - 1, colIniCalDesc].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniCalDesc, row + totalFila - 1, colIniCalDesc].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    wsHist.Cells[row, colIniCalMedidad].Value = reg.Tipoptomedinomb + " (" + reg.Tipoinfoabrev + ")";
                    wsHist.Cells[row, colIniCalMedidad, row + totalFila - 1, colIniCalMedidad].Merge = true;
                    wsHist.Cells[row, colIniCalMedidad, row + totalFila - 1, colIniCalMedidad].Style.WrapText = true;
                    wsHist.Cells[row, colIniCalMedidad, row + totalFila - 1, colIniCalMedidad].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniCalMedidad, row + totalFila - 1, colIniCalMedidad].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsHist.Cells[row, colIniCalMedidad, row + totalFila - 1, colIniCalMedidad].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    foreach (var pto in listaPtoDetalleByCal)
                    {
                        wsHist.Cells[row, colIniDetCodigo].Value = pto.PtomedicodiOrigen;
                        wsHist.Cells[row, colIniDetCodigo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[row, colIniDetCodigo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        wsHist.Cells[row, colIniDetCodigo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        wsHist.Cells[row, colIniDetEmpresa].Value = pto.EmprnombOrigen.Trim();
                        wsHist.Cells[row, colIniDetEmpresa].Style.WrapText = true;
                        wsHist.Cells[row, colIniDetEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[row, colIniDetEmpresa].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        wsHist.Cells[row, colIniDetEquipo].Value = pto.EquinombOrigen.Trim();
                        wsHist.Cells[row, colIniDetEquipo].Style.WrapText = true;
                        wsHist.Cells[row, colIniDetEquipo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[row, colIniDetEquipo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        wsHist.Cells[row, colIniDetDesc].Value = pto.PtomedicodidescOrigen;
                        wsHist.Cells[row, colIniDetDesc].Style.WrapText = true;
                        wsHist.Cells[row, colIniDetDesc].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[row, colIniDetDesc].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        wsHist.Cells[row, colIniDetLectura].Value = pto.Lectnomb;
                        wsHist.Cells[row, colIniDetLectura].Style.WrapText = true;
                        wsHist.Cells[row, colIniDetLectura].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[row, colIniDetLectura].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        wsHist.Cells[row, colIniDetLectura].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        wsHist.Cells[row, colIniDetTipo].Value = pto.Tipoinfoabrev;
                        wsHist.Cells[row, colIniDetTipo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[row, colIniDetTipo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        wsHist.Cells[row, colIniDetTipo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        wsHist.Cells[row, colIniDetFactor].Value = pto.FactorOrigen;
                        wsHist.Cells[row, colIniDetFactor].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[row, colIniDetFactor].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        row++;
                    }

                    if (listaPtoDetalleByCal.Count > 0) { row--; }
                }

                wsHist.Column(1).Width = 3;
                wsHist.Column(colIniCalCodigo).Width = 8;
                wsHist.Column(colIniCalEmpresa).Width = 35;
                wsHist.Column(colIniCalEquipo).Width = 20;
                wsHist.Column(colIniCalDesc).Width = 35;
                wsHist.Column(colIniCalMedidad).Width = 12;

                wsHist.Column(colIniDetCodigo).Width = 8;
                wsHist.Column(colIniDetEmpresa).Width = 35;
                wsHist.Column(colIniDetEquipo).Width = 20;
                wsHist.Column(colIniDetDesc).Width = 35;
                wsHist.Column(colIniDetLectura).Width = 20;
                wsHist.Column(colIniDetTipo).Width = 8;
                wsHist.Column(colIniDetFactor).Width = 8;

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

        #region Informes SGI

        /// <summary>
        /// Lista de areas oparativas del SEIN
        /// </summary>
        /// <returns></returns>
        public List<MeReporptomedDTO> GetListaAreaOperativa()
        {
            List<MeReporptomedDTO> areas_ = this.GetListaPuntoFromMeReporptomed(ConstantesPR5ReportesServicio.ReporcodiDemandaAreas, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto);

            foreach (var d in areas_)
            {
                if (d.Ptomedidesc.Contains("NORTE")) { d.Repptoorden = 1; }
                if (d.Ptomedidesc.Contains("CENTRO")) { d.Repptoorden = 2; }
                if (d.Ptomedidesc.Contains("SUR")) { d.Repptoorden = 3; }
            }

            return areas_.OrderBy(x => x.Repptoorden).ToList();
        }

        /// <summary>
        /// Lista de areas y sub areas oparativas
        /// </summary>
        /// <returns></returns>
        public List<MeReporptomedDTO> GetListaAreaAndSubareaOperativa()
        {
            var areas_subareas = this.GetListaPuntoFromMeReporptomed(ConstantesPR5ReportesServicio.ReporcodiDemandaAreas, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto);
            areas_subareas.AddRange(this.GetListaPuntoFromMeReporptomed(ConstantesPR5ReportesServicio.ReporcodiDemandaSubareas, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto));

            return areas_subareas;
        }

        public List<MeReporptomedDTO> ObtenerPuntosReporteMedicion(int reporcodi)
        {
            List<MeReporptomedDTO> listaPuntos = new List<MeReporptomedDTO>();

            List<MeReporptomedDTO> lstRepPto = GetListaPuntoFromMeReporptomed(reporcodi,
                ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto);

            List<MeReporptomedDTO> lstRepPtoNoCalculado = lstRepPto.Where(x => x.PtomediCalculado == ConstantesAppServicio.NO).ToList();

            List<MeReporptomedDTO> lstRepPtoCalculado = lstRepPto.Where(x => x.PtomediCalculado == ConstantesAppServicio.SI).ToList();
            if (lstRepPtoCalculado.Any())
            {
                List<MeRelacionptoDTO> lstRelacionPtoCalculado = FactorySic.GetMeRelacionptoRepository().GetByCriteria(string.Join(",", lstRepPtoCalculado.Select(x => x.Ptomedicodi)), ConstantesAppServicio.ParametroDefecto);
                foreach (var puntoCalculado in lstRepPtoCalculado)
                {
                    var lstPtosRelacion = lstRelacionPtoCalculado.Where(x => x.Ptomedicodi1 == puntoCalculado.Ptomedicodi).ToList();
                    foreach (var puntoRel in lstPtosRelacion)
                    {
                        var rel = (MeReporptomedDTO)puntoCalculado.Clone();
                        rel.PtomedicodiCalculado = puntoRel.Ptomedicodi2;
                        rel.Repptotabmed = puntoRel.Relptotabmed;
                        rel.Relptofactor = puntoRel.Relptofactor;
                        rel.Funptocodi = puntoRel.Funptocodi;
                        rel.Funptofuncion = puntoRel.Funptofuncion;
                        rel.Equicodi = puntoRel.Equicodi;
                        listaPuntos.Add(rel);
                    }
                }
            }

            listaPuntos.AddRange(lstRepPtoNoCalculado);

            return listaPuntos.OrderBy(x => x.Repptoorden).ToList();
        }


        #endregion
    }
}

using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Hidrologia;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.Siosein2;
using COES.Servicios.Aplicacion.Yupana;
using COES.Servicios.Aplicacion.Yupana.Helper;
using COES.Servicios.Aplicacion.YupanaContinuo.Helper;
using GAMS;
using log4net;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.YupanaContinuo
{
    /// <summary>
    /// Clase del aplicativo RDO-YUPANA
    /// </summary>
    public class YupanaContinuoAppServicio : AppServicioBase
    {
        private readonly FormatoMedicionAppServicio formatoServicio = new FormatoMedicionAppServicio();
        private readonly HidrologiaAppServicio hidrologiaServicio = new HidrologiaAppServicio();
        private readonly YupanaAppServicio yupanaServicio = new YupanaAppServicio();

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(YupanaContinuoAppServicio));

        /// <summary>
        /// inicializar log
        /// </summary>
        public YupanaContinuoAppServicio()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #region CRUD Tablas BD COES

        #region Tabla SI_Empresa
        /// <summary>
        /// Devuelve el total de empresas 
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresas()
        {
            return FactorySic.GetSiEmpresaRepository().ListGeneral();
        }

        #endregion

        #region Métodos Tabla PR_GRUPO
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

        #region Métodos Tabla CP_ARBOL_CONTINUO

        /// <summary>
        /// Actualiza un registro de la tabla CP_ARBOL_CONTINUO
        /// </summary>
        public void UpdateCpArbolContinuo(CpArbolContinuoDTO entity)
        {
            try
            {
                FactorySic.GetCpArbolContinuoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                //throw new ArgumentException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CP_ARBOL_CONTINUO
        /// </summary>
        public void DeleteCpArbolContinuo(int cparbcodi)
        {
            try
            {
                FactorySic.GetCpArbolContinuoRepository().Delete(cparbcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CP_ARBOL_CONTINUO
        /// </summary>
        public CpArbolContinuoDTO GetByIdCpArbolContinuo(int cparbcodi)
        {
            return FactorySic.GetCpArbolContinuoRepository().GetById(cparbcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CP_ARBOL_CONTINUO
        /// </summary>
        public List<CpArbolContinuoDTO> ListCpArbolContinuos()
        {
            return FactorySic.GetCpArbolContinuoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CpArbolContinuo
        /// </summary>
        public List<CpArbolContinuoDTO> GetByCriteriaCpArbolContinuos(int topcodi)
        {
            if (topcodi > 0)
            {
                var lista = FactorySic.GetCpArbolContinuoRepository().GetByCriteria(topcodi)
                    .OrderByDescending(x => x.Cparbbloquehorario).ThenByDescending(x => x.Cparbcodi).ToList();

                foreach (var obj in lista)
                {
                    var tipoEjec = obj.Cparbtag.StartsWith("A") ? "A" : "M";
                    var usuarioReg = obj.Cparbusuregistro;
                    var fechaReg = obj.Cparbfecha.Date == obj.Cparbfecregistro.Date ? obj.Cparbfecregistro.ToString(ConstantesAppServicio.FormatoHHmmss) : obj.Cparbfecregistro.ToString(ConstantesAppServicio.FormatoFechaFull2);
                    obj.Cparbtag = string.Format("Hora {0} - {1} - {2} - {3}", DateTime.Today.AddHours(obj.Cparbbloquehorario).ToString(ConstantesAppServicio.FormatoHora), tipoEjec, usuarioReg, fechaReg);
                }

                return lista;
            }
            else
                return new List<CpArbolContinuoDTO>();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CP_ARBOL_CONTINUO
        /// </summary>
        public CpArbolContinuoDTO GetUltimoArbol()
        {
            return FactorySic.GetCpArbolContinuoRepository().GetUltimoArbol();
        }

        #endregion

        #region Métodos Tabla CP_NODO_CONTINUO        

        /// <summary>
        /// Actualiza un registro de la tabla CP_NODO_CONTINUO
        /// </summary>
        public void UpdateCpNodoContinuo(CpNodoContinuoDTO entity)
        {
            try
            {
                if (entity.Cpnodomsjproceso == null)
                { }

                FactorySic.GetCpNodoContinuoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CP_NODO_CONTINUO
        /// </summary>
        public void DeleteCpNodoContinuo(int cpnodocodi)
        {
            try
            {
                FactorySic.GetCpNodoContinuoRepository().Delete(cpnodocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CP_NODO_CONTINUO
        /// </summary>
        public CpNodoContinuoDTO GetByIdCpNodoContinuo(int cpnodocodi)
        {
            var reg = FactorySic.GetCpNodoContinuoRepository().GetById(cpnodocodi);
            FormatearCpNodoContinuo(reg);

            return reg;
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CP_NODO_CONTINUO
        /// </summary>
        public CpNodoContinuoDTO GetByNumeroCpNodoContinuo(int cparbcodi, int cpnodonumero)
        {
            var reg = FactorySic.GetCpNodoContinuoRepository().GetByNumero(cparbcodi, cpnodonumero);
            FormatearCpNodoContinuo(reg);

            return reg;
        }

        /// <summary>
        /// Devuelve la lista de nodos correspondientes a un árbol
        /// </summary>
        /// <param name="cparbcodi"></param>
        /// <returns></returns>
        public List<CpNodoContinuoDTO> ListCpNodoContinuosPorArbol(int cparbcodi)
        {
            var lista = FactorySic.GetCpNodoContinuoRepository().ListaPorArbol(cparbcodi);

            foreach (var reg in lista)
                FormatearCpNodoContinuo(reg);

            return lista;
        }

        private void FormatearCpNodoContinuo(CpNodoContinuoDTO nodo)
        {
            int nivel = 0;
            if (1 == nodo.Cpnodonumero) nivel = 0;
            if ((new List<int>() { 2, 3, 5, 9 }).Contains(nodo.Cpnodonumero)) nivel = 1;
            if ((new List<int>() { 4, 6, 7, 10, 11, 13 }).Contains(nodo.Cpnodonumero)) nivel = 2;
            if ((new List<int>() { 8, 12, 14, 15 }).Contains(nodo.Cpnodonumero)) nivel = 3;
            if ((new List<int>() { 16 }).Contains(nodo.Cpnodonumero)) nivel = 4;

            nodo.Nivel = nivel;

            List<int> listaHijo = new List<int>();
            switch (nodo.Cpnodonumero)
            {
                case 1:
                    listaHijo = new List<int>() { 9, 5, 3, 2 };
                    break;
                case 2:
                    break;
                case 3:
                    listaHijo = new List<int>() { 4 };
                    break;
                case 4:
                    break;
                case 5:
                    listaHijo = new List<int>() { 7, 6 };
                    break;
                case 6:
                    break;
                case 7:
                    listaHijo = new List<int>() { 8 };
                    break;
                case 8:
                    break;
                case 9:
                    listaHijo = new List<int>() { 13, 11, 10 };
                    break;
                case 10:
                    break;
                case 11:
                    listaHijo = new List<int>() { 12 };
                    break;
                case 12:
                    break;
                case 13:
                    listaHijo = new List<int>() { 15, 14 };
                    break;
                case 14:
                    break;
                case 15:
                    listaHijo = new List<int>() { 16 };
                    break;
                case 16:
                    break;
            }

            nodo.NodoPadre = GetNodoPadre(nodo.Cpnodonumero);
            nodo.OrdenXNivel = OrdenXNivelNodo(nodo.Cpnodonumero);
            nodo.ListaHijo = listaHijo;
        }

        private int OrdenXNivelNodo(int numNodo)
        {
            int ordenXNivel = 1;

            switch (numNodo)
            {
                case 1:
                    ordenXNivel = 1;
                    break;
                case 2:
                    ordenXNivel = 4;
                    break;
                case 3:
                    ordenXNivel = 3;
                    break;
                case 4:
                    ordenXNivel = 6;
                    break;
                case 5:
                    ordenXNivel = 2;
                    break;
                case 6:
                    ordenXNivel = 5;
                    break;
                case 7:
                    ordenXNivel = 4;
                    break;
                case 8:
                    ordenXNivel = 4;
                    break;
                case 9:
                    ordenXNivel = 1;
                    break;
                case 10:
                    ordenXNivel = 3;
                    break;
                case 11:
                    ordenXNivel = 2;
                    break;
                case 12:
                    ordenXNivel = 3;
                    break;
                case 13:
                    ordenXNivel = 1;
                    break;
                case 14:
                    ordenXNivel = 2;
                    break;
                case 15:
                    ordenXNivel = 1;
                    break;
                case 16:
                    ordenXNivel = 1;
                    break;
            }

            return ordenXNivel;
        }

        private int GetNodoPadre(int numNodo)
        {
            int nodopadre = 1;

            switch (numNodo)
            {
                case 1:
                    nodopadre = 0;
                    break;
                case 2:
                    nodopadre = 1;
                    break;
                case 3:
                    nodopadre = 1;
                    break;
                case 4:
                    nodopadre = 3;
                    break;
                case 5:
                    nodopadre = 1;
                    break;
                case 6:
                    nodopadre = 5;
                    break;
                case 7:
                    nodopadre = 5;
                    break;
                case 8:
                    nodopadre = 7;
                    break;
                case 9:
                    nodopadre = 1;
                    break;
                case 10:
                    nodopadre = 9;
                    break;
                case 11:
                    nodopadre = 9;
                    break;
                case 12:
                    nodopadre = 11;
                    break;
                case 13:
                    nodopadre = 9;
                    break;
                case 14:
                    nodopadre = 13;
                    break;
                case 15:
                    nodopadre = 13;
                    break;
                case 16:
                    nodopadre = 15;
                    break;
            }

            return nodopadre;
        }

        #endregion

        #region Métodos Tabla CP_NODO_DETALLE        

        /// <summary>
        /// Actualiza un registro de la tabla CP_NODO_DETALLE
        /// </summary>
        public void UpdateCpNodoDetalle(CpNodoDetalleDTO entity)
        {
            try
            {
                FactorySic.GetCpNodoDetalleRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CP_NODO_DETALLE
        /// </summary>
        public void DeleteCpNodoDetalle(int cpndetcodi)
        {
            try
            {
                FactorySic.GetCpNodoDetalleRepository().Delete(cpndetcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }



        /// <summary>
        /// Permite obtener un registro de la tabla CP_NODO_DETALLE
        /// </summary>
        public CpNodoDetalleDTO GetByIdCpNodoDetalle(int cpndetcodi)
        {
            return FactorySic.GetCpNodoDetalleRepository().GetById(cpndetcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CP_NODO_DETALLE
        /// </summary>
        public List<CpNodoDetalleDTO> ListCpNodoDetalles()
        {
            return FactorySic.GetCpNodoDetalleRepository().List();
        }

        private List<CpNodoDetalleDTO> ListCpNodoDetallesPorNodo(int codigoNodo)
        {
            return FactorySic.GetCpNodoDetalleRepository().ListaPorNodo(codigoNodo);
        }


        /// <summary>
        /// Devuelve lista de nodos detalle para cierto arbol
        /// </summary>
        /// <param name="codigoArbol"></param>
        /// <returns></returns>
        public List<CpNodoDetalleDTO> ListCpNodoDetallesPorArbol(int codigoArbol)
        {
            return FactorySic.GetCpNodoDetalleRepository().ListaPorArbol(codigoArbol);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CpNodoDetalle
        /// </summary>
        public List<CpNodoDetalleDTO> GetByCriteriaCpNodoDetalles()
        {
            return FactorySic.GetCpNodoDetalleRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla CP_NODO_CONCEPTO

        /// <summary>
        /// Inserta un registro de la tabla CP_NODO_CONCEPTO
        /// </summary>
        public void SaveCpNodoConcepto(CpNodoConceptoDTO entity)
        {
            try
            {
                FactorySic.GetCpNodoConceptoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CP_NODO_CONCEPTO
        /// </summary>
        public void UpdateCpNodoConcepto(CpNodoConceptoDTO entity)
        {
            try
            {
                FactorySic.GetCpNodoConceptoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CP_NODO_CONCEPTO
        /// </summary>
        public void DeleteCpNodoConcepto(int cpnconcodi)
        {
            try
            {
                FactorySic.GetCpNodoConceptoRepository().Delete(cpnconcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CP_NODO_CONCEPTO
        /// </summary>
        public CpNodoConceptoDTO GetByIdCpNodoConcepto(int cpnconcodi)
        {
            return FactorySic.GetCpNodoConceptoRepository().GetById(cpnconcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CP_NODO_CONCEPTO
        /// </summary>
        public List<CpNodoConceptoDTO> ListCpNodoConceptos()
        {
            return FactorySic.GetCpNodoConceptoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CpNodoConcepto
        /// </summary>
        public List<CpNodoConceptoDTO> GetByCriteriaCpNodoConceptos()
        {
            return FactorySic.GetCpNodoConceptoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla CP_FORZADO_CAB

        /// <summary>
        /// Inserta un registro de la tabla CP_FORZADO_CAB
        /// </summary>
        public int SaveCpForzadoCab(CpForzadoCabDTO entity)
        {
            int cpfzcodi;
            try
            {
                cpfzcodi = FactorySic.GetCpForzadoCabRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException(ex.Message, ex);

            }
            return cpfzcodi;
        }

        /// <summary>
        /// Actualiza un registro de la tabla CP_FORZADO_CAB
        /// </summary>
        public void UpdateCpForzadoCab(CpForzadoCabDTO entity)
        {
            try
            {
                FactorySic.GetCpForzadoCabRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CP_FORZADO_CAB
        /// </summary>
        public void DeleteCpForzadoCab(int cpfzcodi)
        {
            try
            {
                FactorySic.GetCpForzadoCabRepository().Delete(cpfzcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CP_FORZADO_CAB
        /// </summary>
        public CpForzadoCabDTO GetByIdCpForzadoCab(int cpfzcodi)
        {
            return FactorySic.GetCpForzadoCabRepository().GetById(cpfzcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CP_FORZADO_CAB
        /// </summary>
        public List<CpForzadoCabDTO> ListCpForzadoCabs()
        {
            return FactorySic.GetCpForzadoCabRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CpForzadoCab
        /// </summary>
        public List<CpForzadoCabDTO> GetByCriteriaCpForzadoCabs()
        {
            return FactorySic.GetCpForzadoCabRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla CP_FORZADO_DET

        /// <summary>
        /// Inserta un registro de la tabla CP_FORZADO_DET
        /// </summary>
        public void SaveCpForzadoDet(CpForzadoDetDTO entity)
        {
            try
            {
                FactorySic.GetCpForzadoDetRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CP_FORZADO_DET
        /// </summary>
        public void UpdateCpForzadoDet(CpForzadoDetDTO entity)
        {
            try
            {
                FactorySic.GetCpForzadoDetRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CP_FORZADO_DET
        /// </summary>
        public void DeleteCpForzadoDet(int cpfzdtcodi)
        {
            try
            {
                FactorySic.GetCpForzadoDetRepository().Delete(cpfzdtcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CP_FORZADO_DET
        /// </summary>
        public CpForzadoDetDTO GetByIdCpForzadoDet(int cpfzdtcodi)
        {
            return FactorySic.GetCpForzadoDetRepository().GetById(cpfzdtcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CP_FORZADO_DET
        /// </summary>
        public List<CpForzadoDetDTO> ListCpForzadoDets()
        {
            return FactorySic.GetCpForzadoDetRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CpForzadoDet
        /// </summary>
        public List<CpForzadoDetDTO> GetByCriteriaCpForzadoDets()
        {
            return FactorySic.GetCpForzadoDetRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla CP_YUPCON_CFG

        /// <summary>
        /// Inserta un registro de la tabla CP_YUPCON_CFG
        /// </summary>
        public int SaveCpYupconCfg(CpYupconCfgDTO entity)
        {
            try
            {
                return FactorySic.GetCpYupconCfgRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CP_YUPCON_CFG
        /// </summary>
        public void UpdateCpYupconCfg(CpYupconCfgDTO entity)
        {
            try
            {
                FactorySic.GetCpYupconCfgRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CP_YUPCON_CFG
        /// </summary>
        public void DeleteCpYupconCfg(int yupcfgcodi)
        {
            try
            {
                FactorySic.GetCpYupconCfgRepository().Delete(yupcfgcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CP_YUPCON_CFG
        /// </summary>
        public CpYupconCfgDTO GetByIdCpYupconCfg(int yupcfgcodi)
        {
            return FactorySic.GetCpYupconCfgRepository().GetById(yupcfgcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CP_YUPCON_CFG
        /// </summary>
        public List<CpYupconCfgDTO> ListCpYupconCfgs()
        {
            return FactorySic.GetCpYupconCfgRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CpYupconCfg
        /// </summary>
        public List<CpYupconCfgDTO> GetByCriteriaCpYupconCfgs(int tyupcodi, DateTime fechaConsulta, int hora)
        {
            return FactorySic.GetCpYupconCfgRepository().GetByCriteria(tyupcodi, fechaConsulta, hora).OrderByDescending(x => x.Yupcfgcodi).ToList();
        }

        #endregion

        #region Métodos Tabla CP_YUPCON_CFGDET

        /// <summary>
        /// Inserta un registro de la tabla CP_YUPCON_CFGDET
        /// </summary>
        public void SaveCpYupconCfgdet(CpYupconCfgdetDTO entity)
        {
            try
            {
                FactorySic.GetCpYupconCfgdetRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CP_YUPCON_CFGDET
        /// </summary>
        public void UpdateCpYupconCfgdet(CpYupconCfgdetDTO entity)
        {
            try
            {
                FactorySic.GetCpYupconCfgdetRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CP_YUPCON_CFGDET
        /// </summary>
        public void DeleteCpYupconCfgdet(int ycdetcodi)
        {
            try
            {
                FactorySic.GetCpYupconCfgdetRepository().Delete(ycdetcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CP_YUPCON_CFGDET
        /// </summary>
        public CpYupconCfgdetDTO GetByIdCpYupconCfgdet(int ycdetcodi)
        {
            return FactorySic.GetCpYupconCfgdetRepository().GetById(ycdetcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CP_YUPCON_CFGDET
        /// </summary>
        public List<CpYupconCfgdetDTO> ListCpYupconCfgdets()
        {
            return FactorySic.GetCpYupconCfgdetRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CpYupconCfgdet
        /// </summary>
        public List<CpYupconCfgdetDTO> GetByCriteriaCpYupconCfgdets(int yupcfgcodi, int recurcodi)
        {
            return FactorySic.GetCpYupconCfgdetRepository().GetByCriteria(yupcfgcodi, recurcodi);
        }

        #endregion

        #region Métodos Tabla CP_YUPCON_ENVIO

        /// <summary>
        /// Inserta un registro de la tabla CP_YUPCON_ENVIO
        /// </summary>
        public int SaveCpYupconEnvio(CpYupconEnvioDTO entity)
        {
            try
            {
                return FactorySic.GetCpYupconEnvioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CP_YUPCON_ENVIO
        /// </summary>
        public void UpdateCpYupconEnvio(CpYupconEnvioDTO entity)
        {
            try
            {
                FactorySic.GetCpYupconEnvioRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CP_YUPCON_ENVIO
        /// </summary>
        public void DeleteCpYupconEnvio(int cyupcodi)
        {
            try
            {
                FactorySic.GetCpYupconEnvioRepository().Delete(cyupcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CP_YUPCON_ENVIO
        /// </summary>
        public CpYupconEnvioDTO GetByIdCpYupconEnvio(int cyupcodi)
        {
            return FactorySic.GetCpYupconEnvioRepository().GetById(cyupcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CP_YUPCON_ENVIO
        /// </summary>
        public List<CpYupconEnvioDTO> ListCpYupconEnvios()
        {
            return FactorySic.GetCpYupconEnvioRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CpYupconEnvio
        /// </summary>
        public List<CpYupconEnvioDTO> GetByCriteriaCpYupconEnvios(int tyupcodi, DateTime fecha, int hora)
        {
            var lista = FactorySic.GetCpYupconEnvioRepository().GetByCriteria(tyupcodi, fecha, hora).OrderByDescending(x => x.Cyupcodi).ToList();
            foreach (var obj in lista) FormatearCpYupconEnvio(obj);

            return lista;
        }

        private void FormatearCpYupconEnvio(CpYupconEnvioDTO obj)
        {
            obj.CyupfechaDesc = obj.Cyupfecha.ToString(ConstantesAppServicio.FormatoFecha);
            obj.CyupfecregistroDesc = obj.Cyupfecregistro.ToString(ConstantesAppServicio.FormatoFechaFull2);
            obj.CyupbloquehorarioDesc = obj.Cyupfecha.AddHours(obj.Cyupbloquehorario).ToString(ConstantesAppServicio.FormatoHora);
        }

        #endregion

        #region Métodos Tabla CP_YUPCON_M48

        /// <summary>
        /// Inserta un registro de la tabla CP_YUPCON_M48
        /// </summary>
        public void SaveCpYupconM48(CpYupconM48DTO entity)
        {
            try
            {
                FactorySic.GetCpYupconM48Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CP_YUPCON_M48
        /// </summary>
        public void UpdateCpYupconM48(CpYupconM48DTO entity)
        {
            try
            {
                FactorySic.GetCpYupconM48Repository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CP_YUPCON_M48
        /// </summary>
        public void DeleteCpYupconM48(int dyupcodi)
        {
            try
            {
                FactorySic.GetCpYupconM48Repository().Delete(dyupcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CP_YUPCON_M48
        /// </summary>
        public CpYupconM48DTO GetByIdCpYupconM48(int dyupcodi)
        {
            return FactorySic.GetCpYupconM48Repository().GetById(dyupcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CP_YUPCON_M48
        /// </summary>
        public List<CpYupconM48DTO> ListCpYupconM48s()
        {
            return FactorySic.GetCpYupconM48Repository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CpYupconM48
        /// </summary>
        public List<CpYupconM48DTO> GetByCriteriaCpYupconM48s(int cyupcodi)
        {
            var lista = FactorySic.GetCpYupconM48Repository().GetByCriteria(cyupcodi);

            foreach (var obj in lista)
            {
                FormatearCpYupconM48DTO(obj);
            }

            return lista;
        }

        private void FormatearCpYupconM48DTO(CpYupconM48DTO obj)
        {
            string catabrev = "";
            switch (obj.Catcodi)
            {
                case ConstantesBase.Embalse:
                    catabrev = "Embalse";
                    break;
                case ConstantesBase.PlantaH:
                    catabrev = "Planta H.";
                    break;
                case ConstantesBase.PlantaNoConvenO:
                    catabrev = "Planta RER";
                    break;
            }

            obj.Catabrev = catabrev;
        }

        #endregion

        #region Métodos Tabla CP_YUPCON_TIPO

        /// <summary>
        /// Inserta un registro de la tabla CP_YUPCON_TIPO
        /// </summary>
        public void SaveCpYupconTipo(CpYupconTipoDTO entity)
        {
            try
            {
                FactorySic.GetCpYupconTipoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CP_YUPCON_TIPO
        /// </summary>
        public void UpdateCpYupconTipo(CpYupconTipoDTO entity)
        {
            try
            {
                FactorySic.GetCpYupconTipoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CP_YUPCON_TIPO
        /// </summary>
        public void DeleteCpYupconTipo(int tyupcodi)
        {
            try
            {
                FactorySic.GetCpYupconTipoRepository().Delete(tyupcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CP_YUPCON_TIPO
        /// </summary>
        public CpYupconTipoDTO GetByIdCpYupconTipo(int tyupcodi)
        {
            return FactorySic.GetCpYupconTipoRepository().GetById(tyupcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CP_YUPCON_TIPO
        /// </summary>
        public List<CpYupconTipoDTO> ListCpYupconTipos()
        {
            return FactorySic.GetCpYupconTipoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CpYupconTipo
        /// </summary>
        public List<CpYupconTipoDTO> GetByCriteriaCpYupconTipos()
        {
            return FactorySic.GetCpYupconTipoRepository().GetByCriteria();
        }

        #endregion

        #endregion

        #region FUNCIONES GENERALES

        /// <summary>
        /// Devuelve la última topologia para cierta fecha
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        private CpTopologiaDTO ObtenerTopologiaByDate(DateTime fechaConsulta)
        {
            int posH = fechaConsulta.Hour * 2 + 1; //si es las 00:00 obtener el yupana más cercano a 00:30 y así sucesivamente
            List<CpTopologiaDTO> listaTopologia = hidrologiaServicio.ListarTopologiaYupana(fechaConsulta, posH).OrderByDescending(x => x.OrdenReprograma).ToList();

            //yupana vigente para la fecha y hora
            CpTopologiaDTO regTopFechaHora = listaTopologia.FirstOrDefault();

            return regTopFechaHora;
        }

        /// <summary>
        /// Obtener la ultima topologia para la fecha actual
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public int GetTopologiaByDate(DateTime fechaConsulta)
        {
            var objYup = ObtenerTopologiaByDate(fechaConsulta);
            return objYup != null ? objYup.Topcodi : 0;
        }

        /// <summary>
        /// Obtener todos los escenarios yupana de una fecha seleccionada
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public List<CpTopologiaDTO> ListarTopologiaXFecha(DateTime fechaConsulta)
        {
            var lista = hidrologiaServicio.ListarTopologiaYupana(fechaConsulta, 47).OrderByDescending(x => x.OrdenReprograma).ToList();

            return lista;
        }

        public List<GenericoDTO> ListarHoras(int horaActual)
        {
            var listaHoras = new List<GenericoDTO>();
            var hoy = DateTime.Today;
            for (DateTime i = hoy; i < hoy.AddDays(1); i = i.AddHours(1))
            {
                listaHoras.Add(new GenericoDTO() { String1 = i.ToString("HH:mm"), Entero1 = i.Hour, Selected1 = i.Hour == horaActual });
            }

            return listaHoras;
        }

        #endregion

        #region CONDICIONES TÉRMICAS

        /// <summary>
        /// crea/modifica registros de condiciones termicas
        /// </summary>
        /// <param name="cpForzadoDet"></param>
        /// <param name="topologia"></param>
        /// <param name="fecha"></param>
        /// <param name="usuario"></param>
        public void MantenerCondicionesTermicas(CpForzadoDetDTO cpForzadoDet, int topologia, DateTime fechaHora, string usuario)
        {
            var bloquehorario = fechaHora.Hour;

            var cabecera = GetByDateCpForzadoCab(fechaHora);
            var detalle = GetByCpfzcodiCpForzadoDets(cabecera.Cpfzcodi);

            if (cpForzadoDet.Cpfzdtcodi != 0)
            {
                detalle = detalle.Where(x => x.Cpfzdtcodi != cpForzadoDet.Cpfzdtcodi).ToList();

                var cpForzadoDetDB = GetByIdCpForzadoDet(cpForzadoDet.Cpfzdtcodi);
                cpForzadoDet.Emprcodi = cpForzadoDetDB.Emprcodi;
                cpForzadoDet.Equicodi = cpForzadoDetDB.Equicodi;
                cpForzadoDet.Grupocodi = cpForzadoDetDB.Grupocodi;
            }
            var regModo = GetByIdPrGrupo(cpForzadoDet.Grupocodi);
            cpForzadoDet.Gruponomb = regModo.Gruponomb;

            List<PrGrupoDTO> listaUnidadXModo = (new HorasOperacionAppServicio()).ListarUnidadesWithModoOperacionXCentralYEmpresa(-2, "-2");

            string msjExisteCruce = VerficarCruceCondicionTermico(detalle, cpForzadoDet, listaUnidadXModo);

            if (string.IsNullOrEmpty(msjExisteCruce))
            {
                cabecera.Cpfzbloquehorario = bloquehorario;
                cabecera.Topcodi = topologia;
                cabecera.Cpfzusuregistro = usuario;
                cabecera.Cpfzfecregistro = DateTime.Now;

                var cpfzcodi = SaveCpForzadoCab(cabecera);

                cpForzadoDet.Cpfzdtflagcreacion = ConstantesYupanaContinuo.CreadoModificadoUsuario;
                detalle.Add(cpForzadoDet);

                foreach (var item in detalle)
                {
                    item.Cpfzcodi = cpfzcodi;

                    SaveCpForzadoDet(item);
                }
            }
            else
            {
                throw new ArgumentException(msjExisteCruce + " tiene cruce de periodos forzados. ");
            }
        }

        private string VerficarCruceCondicionTermico(List<CpForzadoDetDTO> listaForzadoDet, CpForzadoDetDTO cpForzadoDet, List<PrGrupoDTO> listaUnidadXModo)
        {
            var rango = new Range<int>(cpForzadoDet.Cpfzdtperiodoini.Value - 1, cpForzadoDet.Cpfzdtperiodofin.Value);

            //Validación por modos de operación
            var lista = listaForzadoDet.Where(x => x.Grupocodi == cpForzadoDet.Grupocodi).ToList();
            if (ExisteCruceXLista(lista, rango))
                return $"El modo de operación {cpForzadoDet.Gruponomb}";

            //Validación a nivel de unidades
            List<CpForzadoDetDTO> listaAllXGen = new List<CpForzadoDetDTO>();
            foreach (var obj in listaForzadoDet)
            {
                listaAllXGen.AddRange(ListarForzadoUnidadXModo(obj, listaUnidadXModo));
            }

            var listaGenXDetInput = ListarForzadoUnidadXModo(cpForzadoDet, listaUnidadXModo);
            foreach (var obj in listaGenXDetInput)
            {
                var listaDetxGen = listaAllXGen.Where(x => x.Generadorcodi == obj.Generadorcodi).OrderBy(x => x.Cpfzdtperiodoini).ToList();
                if (ExisteCruceXLista(listaDetxGen, rango))
                    return $"El generador {obj.Generadornomb}";
            }

            //salida por defecto
            return string.Empty;
        }

        private bool ExisteCruceXLista(List<CpForzadoDetDTO> lista, Range<int> rango)
        {
            foreach (var item in lista)
            {
                var rangoDb = new Range<int>(item.Cpfzdtperiodoini.Value, item.Cpfzdtperiodofin.Value);

                if (rangoDb.IsOverlapped(rango)) return true;
            }

            return false;
        }

        private List<CpForzadoDetDTO> ListarForzadoUnidadXModo(CpForzadoDetDTO reg, List<PrGrupoDTO> listaUnidadXModo)
        {
            List<CpForzadoDetDTO> lFinal = new List<CpForzadoDetDTO>();

            //listar los generadores del modo
            var listaUnidad = listaUnidadXModo.Where(x => x.Grupocodi == reg.Grupocodi).ToList();

            //por cada generador clonarlo y asignarle su codigo
            foreach (var regTmp in listaUnidad)
            {
                var regUniTmp = (CpForzadoDetDTO)reg.Clone();
                regUniTmp.Generadorcodi = regTmp.Equicodi;
                regUniTmp.Generadornomb = regTmp.Equinomb;

                lFinal.Add(regUniTmp);
            }

            return lFinal;
        }

        private List<CpForzadoDetDTO> GetByCpfzcodiCpForzadoDets(int cpfzcodi)
        {
            return FactorySic.GetCpForzadoDetRepository().GetByCpfzcodi(cpfzcodi);
        }

        /// <summary>
        /// obtener condiciones termicas del dia seleccionado
        /// </summary>
        /// <param name="hoy"></param>
        /// <returns></returns>
        public CpForzadoCabDTO GetByDateCpForzadoCab(DateTime fechaHora)
        {
            var reg = FactorySic.GetCpForzadoCabRepository().GetByDate(fechaHora);
            if (reg != null)
            {
                reg.CpfzfecregistroDesc = reg.Cpfzfecha.Date.AddHours(reg.Cpfzbloquehorario).ToString(ConstantesBase.FormatFechaFull);
            }

            return reg;
        }

        private List<CpForzadoCabDTO> GetListByDateCpForzadoCab(DateTime fechaHora)
        {
            return FactorySic.GetCpForzadoCabRepository().GetListByDate(fechaHora);
        }

        /// <summary>
        /// Crea registros de condidicon termica forzado de manera automatizada
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="topologia"></param>
        /// <param name="fechaHoraActual"></param>
        public void ActualizarAutomaticoCondicionTermica(string usuario, int topologia, DateTime fechaHoraActual)
        {

            var cabecera = new CpForzadoCabDTO()
            {
                Cpfzfecha = fechaHoraActual.Date,
                Cpfzbloquehorario = fechaHoraActual.Hour,
                Topcodi = topologia,
                Cpfzfecregistro = DateTime.Now,
                Cpfzusuregistro = usuario
            };

            var detalle = new List<CpForzadoDetDTO>();

            var lstGrupoModo = (new INDAppServicio()).ListarGrupoModoOperacionComercial(fechaHoraActual.AddMonths(-24), fechaHoraActual);

            if (fechaHoraActual.Hour != 0)
            {
                List<CpForzadoDetDTO> lstForzadoDetEve = ObtenerCpForzadoDetDesdeHoraOperacion(fechaHoraActual);
                detalle.AddRange(lstForzadoDetEve);
            }

            List<CpForzadoDetDTO> lstForzadoDetM48 = ObtenerCpForzadoDetDesdeUnidadadesForzado(topologia, lstGrupoModo, fechaHoraActual);

            detalle.AddRange(lstForzadoDetM48);

            var cpfzcodi = SaveCpForzadoCab(cabecera);

            foreach (var item in detalle)
            {
                item.Cpfzcodi = cpfzcodi;
                SaveCpForzadoDet(item);
            }
        }

        private List<CpForzadoDetDTO> ObtenerCpForzadoDetDesdeUnidadadesForzado(int topologia, List<PrGrupoDTO> lstGrupoModo, DateTime fechaHoraActual)
        {
            var lstUnidadesForzadas = FactorySic.GetCpMedicion48Repository().GetByCriteriaRecurso(topologia, ConstantesYupanaContinuo.SrestcodiUniForzada, ConstantesYupanaContinuo.ParametroDefecto);
            lstUnidadesForzadas = lstUnidadesForzadas.Where(x => x.Medifecha == fechaHoraActual.Date).ToList();

            List<CpForzadoDetDTO> lstForzadoDetM48 = CpMedicion48ToCpForzadoDet(lstUnidadesForzadas, fechaHoraActual);

            foreach (var item in lstForzadoDetM48)
            {
                var modo = lstGrupoModo.Find(x => x.Grupocodi == item.Grupocodi);
                item.Equicodi = modo.Equipadre;
                item.Emprcodi = modo.Emprcodi ?? 0;
            }

            return lstForzadoDetM48;
        }

        private List<PrGrupoDTO> ListarGrupocodiModoXTopcodi(int topologia, DateTime fechaConsulta, List<PrGrupoDTO> lstGrupoModo)
        {
            var lstUnidadesForzadas = FactorySic.GetCpMedicion48Repository().GetByCriteriaRecurso(topologia, ConstantesYupanaContinuo.SrestcodiUniForzada, ConstantesYupanaContinuo.ParametroDefecto);
            lstUnidadesForzadas = lstUnidadesForzadas.Where(x => x.Medifecha == fechaConsulta.Date).ToList();

            var listaGrupocodi = lstUnidadesForzadas.Select(x => x.Recurcodisicoes).Distinct().ToList();

            return lstGrupoModo.Where(x => listaGrupocodi.Contains(x.Grupocodi)).ToList();
        }

        private static List<CpForzadoDetDTO> ObtenerCpForzadoDetDesdeHoraOperacion(DateTime fechaHoraActual)
        {
            var lstEveHoraOperacion = FactorySic.GetEveHoraoperacionRepository()
                            .ListarHorasOperacxEquiposXEmpXTipoOPxFam2(ConstantesYupanaContinuo.ParametroDefecto2, fechaHoraActual.Date, fechaHoraActual.Date.AddDays(1), ConstantesYupanaContinuo.SubcausacodiPotenciaOEnergia, ConstantesYupanaContinuo.ParametroDefecto2);

            List<CpForzadoDetDTO> lstForzadoDetEve = EveHoraOperacionToCpForzadoDet(lstEveHoraOperacion, fechaHoraActual);
            return lstForzadoDetEve;
        }

        private static List<CpForzadoDetDTO> EveHoraOperacionToCpForzadoDet(List<EveHoraoperacionDTO> lstEveHoraOperacionNoEnYupana, DateTime fechaHoraActual)
        {
            List<CpForzadoDetDTO> lstForzadoDet = new List<CpForzadoDetDTO>();
            var hxMax = Util.ConvertirHoraMinutosAHx(fechaHoraActual, ConstantesSiosein2.TipoMedicion.Medicion48, ConstantesSiosein2.TipoHora.HxFin);

            foreach (var eveHo in lstEveHoraOperacionNoEnYupana)
            {
                var cpfzdtperiodoini = Util.ConvertirHoraMinutosAHx(eveHo.Hophorini.Value, ConstantesSiosein2.TipoMedicion.Medicion48, ConstantesSiosein2.TipoHora.HxInicio);
                var cpfzdtperiodofin = Util.ConvertirHoraMinutosAHx(eveHo.Hophorfin.Value, ConstantesSiosein2.TipoMedicion.Medicion48, ConstantesSiosein2.TipoHora.HxFin);
                if (cpfzdtperiodoini > hxMax) continue;
                if (cpfzdtperiodofin > hxMax) cpfzdtperiodofin = hxMax;

                lstForzadoDet.Add(new CpForzadoDetDTO()
                {
                    Grupocodi = eveHo.Grupocodi ?? 0,
                    Equicodi = eveHo.Equipadre,
                    Emprcodi = eveHo.Emprcodi,
                    Cpfzdtperiodoini = cpfzdtperiodoini,
                    Cpfzdtperiodofin = cpfzdtperiodofin,
                    Cpfzdtflagcreacion = ConstantesYupanaContinuo.CreadoSistemaHoraOp
                });
            }

            return lstForzadoDet;
        }

        private List<CpForzadoDetDTO> CpMedicion48ToCpForzadoDet(List<CpMedicion48DTO> lstUnidadesForzadas, DateTime fechaHoraActual)
        {
            List<CpForzadoDetDTO> lstForzadoDet = new List<CpForzadoDetDTO>();

            var hxMin = Util.ConvertirHoraMinutosAHx(fechaHoraActual, ConstantesSiosein2.TipoMedicion.Medicion48, ConstantesSiosein2.TipoHora.HxInicio);
            if (hxMin != 1) hxMin += 1;

            foreach (var uniForzada in lstUnidadesForzadas)
            {
                List<int> lstIndex = new List<int>();

                for (int hx = 1; hx <= 48; hx++)
                {
                    var valHx = (decimal?)uniForzada.GetType().GetProperty(ConstantesYupanaContinuo.CaracterH + hx).GetValue(uniForzada, null);

                    if ((valHx ?? 0) == 1) lstIndex.Add(hx);
                }

                var lisIndexIe = lstIndex as IEnumerable<int>;

                var lstConsecutive = GroupConsecutive(lisIndexIe);

                foreach (var consecutive in lstConsecutive)
                {
                    var cpfzdtperiodoini = consecutive.First();
                    var cpfzdtperiodofin = consecutive.Last();

                    if (cpfzdtperiodofin < hxMin) continue;
                    if (cpfzdtperiodoini < hxMin) cpfzdtperiodoini = hxMin;

                    lstForzadoDet.Add(new CpForzadoDetDTO()
                    {
                        Grupocodi = uniForzada.Recurcodisicoes,
                        Cpfzdtperiodoini = cpfzdtperiodoini,
                        Cpfzdtperiodofin = consecutive.Last(),
                        Cpfzdtflagcreacion = ConstantesYupanaContinuo.CreadoSistemaYupana
                    }
                    );
                }
            }

            return lstForzadoDet;
        }

        /// <summary>
        /// Obtiene agrupación de numeros consecutivos
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public IEnumerable<IEnumerable<int>> GroupConsecutive(IEnumerable<int> src)
        {
            var more = false; // compiler can't figure out more is assigned before use
            IEnumerable<int> ConsecutiveSequence(IEnumerator<int> csi)
            {
                int prevCurrent;
                do
                    yield return (prevCurrent = csi.Current);
                while ((more = csi.MoveNext()) && csi.Current - prevCurrent == 1);
            }

            var si = src.GetEnumerator();
            if (si.MoveNext())
            {
                do
                    // have to process to compute outside level  
                    yield return ConsecutiveSequence(si).ToList();
                while (more);
            }
        }

        /// <summary>
        /// Eliminar periodo forzado
        /// </summary>
        /// <param name="cpForzadoDet"></param>
        /// <param name="topologia"></param>
        /// <param name="fecha"></param>
        /// <param name="usuario"></param>
        public void EliminarPeriodoForzado(CpForzadoDetDTO cpForzadoDet, int topologia, DateTime fechaHora, string usuario)
        {
            var bloquehorario = fechaHora.Hour;

            var cabecera = GetByDateCpForzadoCab(fechaHora);
            var detalle = GetByCpfzcodiCpForzadoDets(cabecera.Cpfzcodi).Where(x => x.Cpfzdtcodi != cpForzadoDet.Cpfzdtcodi).ToList();


            cabecera.Cpfzbloquehorario = bloquehorario;
            cabecera.Topcodi = topologia;
            cabecera.Cpfzusuregistro = usuario;
            cabecera.Cpfzfecregistro = DateTime.Now;

            var cpfzcodi = SaveCpForzadoCab(cabecera);

            foreach (var item in detalle)
            {
                item.Cpfzcodi = cpfzcodi;

                SaveCpForzadoDet(item);
            }

        }

        /// <summary>
        /// Genera tabla HTML con el grafico de condición termica
        /// </summary>
        /// <param name="cpfzcodi"></param>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public string GraficarCondicionTermica(int cpfzcodi, DateTime fechaConsulta)
        {
            var lstForzadoDet = ListarDetalleCpForzadoDetsWeb(cpfzcodi, fechaConsulta);

            var hoy = DateTime.Today.Date;
            var mañana = hoy.AddDays(1);

            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<table class='pretty tabla-icono' id='tblCondicionTermico'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>EMPRESA</th>");
            strHtml.Append("<th>CENTRAL</th>");
            strHtml.Append("<th>MODOS DE OPERACIÓN</th>");

            for (DateTime f = hoy.AddMinutes(30); f <= mañana; f = f.AddMinutes(30))
            {
                var fecha = f != mañana ? f : f.AddMinutes(-1);
                strHtml.Append($"<th>{fecha:HH:mm}</th>");
            }

            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");

            var lstforzadoDetGrp = lstForzadoDet.GroupBy(x => x.Grupocodi).OrderBy(x => x.First().Emprnomb).ThenBy(x => x.First().Equinomb).ThenBy(x => x.First().Gruponomb).ToList();

            foreach (var forzadoDetGrp in lstforzadoDetGrp)
            {
                var forzadoDet = forzadoDetGrp.First();
                if (forzadoDet.Grupocodi == 219)
                { }

                string sColorYupana = forzadoDet.ExisteYupana ? "color: blue;" : string.Empty;

                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td style='white-space: nowrap; text-align: left;{1}'>{0}</td>", forzadoDet.Emprnomb, string.Empty);
                strHtml.AppendFormat("<td style='white-space: nowrap; text-align: left;{1}'>{0}</td>", forzadoDet.Equinomb, string.Empty);
                strHtml.AppendFormat("<td style='white-space: nowrap; text-align: left;{1}'>{0}</td>", forzadoDet.Gruponomb, sColorYupana);

                for (int index = 1; index <= 48; index++)
                {
                    var detIndex = forzadoDetGrp.FirstOrDefault(x => x.Cpfzdtperiodoini == index);

                    if (detIndex != null)
                    {
                        var colspan = detIndex.Cpfzdtperiodofin.Value - detIndex.Cpfzdtperiodoini.Value;

                        if (colspan < 0)
                            throw new ArgumentException("Periodo inicial mayor al periodo final.");


                        var color = "#FFA500";
                        if (detIndex.Cpfzdtflagcreacion == ConstantesYupanaContinuo.CreadoSistemaYupana) color = "#4FC3F7";
                        if (detIndex.Cpfzdtflagcreacion == ConstantesYupanaContinuo.CreadoSistemaHoraOp) color = "#33B55C";

                        //se crea el td con colspan pero luego se agrega td invisibles para el datatable
                        if (!detIndex.EsFicticio)
                        {
                            string sClaseEditar = "class='context-menu-one'";
                            strHtml.Append($"<td colspan='{colspan + 1}' id='detter_{detIndex.Cpfzdtcodi}_{detIndex.Emprcodi}_{detIndex.Equicodi}_{detIndex.Grupocodi}_{detIndex.Cpfzdtperiodoini}_{detIndex.Cpfzdtperiodofin}' style='background-color:{color}; border: 1px solid #848484;' {sClaseEditar}></td>");
                        }
                        else
                        {
                            string sClaseNuevo = "class='context-menu-one-nuevo'";

                            strHtml.Append($"<td colspan='{colspan + 1}' id='nuevo_0_{detIndex.Emprcodi}_{detIndex.Equicodi}_{detIndex.Grupocodi}_{detIndex.Cpfzdtperiodoini}_{detIndex.Cpfzdtperiodofin}' {sClaseNuevo} ></td>");
                        }

                        for (int i = index; i < index + colspan; i++)
                        {
                            strHtml.Append($"<td style='display: none'></td>");
                        }

                        index += colspan;
                    }
                    else
                    {
                        strHtml.Append($"<td></td>");
                    }

                }

                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");

            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// reporte de envios
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string ReporteHtmlEnvios(DateTime fechaHora)
        {
            var lstCabecera = GetListByDateCpForzadoCab(fechaHora);

            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<table class='pretty tabla-icono' style='width: 350px;' id='tablaenvio'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>CÓDIGO</th>");
            strHtml.Append("<th>FECHA HORA</th>");
            strHtml.Append("<th>FECHA REGISTRO</th>");
            strHtml.Append("<th>USUARIO</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");

            foreach (var cabecera in lstCabecera)
            {
                var fechaFormatFull = cabecera.Cpfzfecha.AddHours(cabecera.Cpfzbloquehorario).ToString(ConstantesBase.FormatFechaFull);
                strHtml.Append($"<tr style='cursor:pointer;' onClick='visualizarCondicionTermica({cabecera.Cpfzcodi},\"{fechaFormatFull}\");'>");
                strHtml.AppendFormat("<td>{0}</td>", cabecera.Cpfzcodi);
                strHtml.AppendFormat("<td>{0}</td>", fechaFormatFull);
                strHtml.AppendFormat("<td>{0}</td>", cabecera.Cpfzfecregistro.ToString(ConstantesBase.FormatFechaFull));
                strHtml.AppendFormat("<td>{0}</td>", cabecera.Cpfzusuregistro);
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");

            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        private List<CpForzadoDetDTO> ListarDetalleCpForzadoDetsWeb(int cpfzcodi, DateTime fechaConsulta)
        {
            var lstForzadoDet = GetByCpfzcodiCpForzadoDets(cpfzcodi).OrderBy(x => x.Emprcodi).ThenBy(x => x.Equinomb).ThenBy(x => x.Gruponomb).ToList();

            var cabecera = GetByIdCpForzadoCab(cpfzcodi);
            var lstGrupoModo = (new INDAppServicio()).ListarGrupoModoOperacionComercial(fechaConsulta.Date, fechaConsulta.Date);

            var listaModoYupana = ListarGrupocodiModoXTopcodi(cabecera.Topcodi, fechaConsulta.Date, lstGrupoModo);
            foreach (var regYup in listaModoYupana)
            {
                var listaTmp = lstForzadoDet.Where(x => x.Grupocodi == regYup.Grupocodi).ToList();
                if (listaTmp.Any())
                {
                    foreach (var obj in listaTmp)
                        obj.ExisteYupana = true;
                }
                else
                {
                    //agregar objeto solo para cabecera de la tabla
                    lstForzadoDet.Add(new CpForzadoDetDTO()
                    {
                        Emprcodi = regYup.Emprcodi ?? 0,
                        Emprnomb = regYup.Emprnomb,
                        Equicodi = regYup.Equipadre,
                        Equinomb = regYup.Central,
                        Grupocodi = regYup.Grupocodi,
                        Gruponomb = regYup.Gruponomb,
                        Cpfzdtperiodoini = 1,
                        Cpfzdtperiodofin = 48,
                        EsFicticio = true,
                        ExisteYupana = true
                    });
                }
            }

            var lstforzadoDetGrp = lstForzadoDet.GroupBy(x => x.Grupocodi);
            foreach (var forzadoDetGrp in lstforzadoDetGrp)
            {
                var forzadoDet = forzadoDetGrp.First();
                var sublista = forzadoDetGrp.ToList();

                if (forzadoDet.Grupocodi == 257)
                { }

                List<List<int>> agrupSubLista = new List<List<int>>();
                List<int> sublistaTmp = new List<int>();
                for (int index = 1; index <= 48; index++)
                {

                    bool esBlancoActual = sublista.Find(x => x.Cpfzdtperiodoini <= index && index <= x.Cpfzdtperiodofin) == null;
                    bool esBlancoSiguiente = index < 48 ? sublista.Find(x => x.Cpfzdtperiodoini <= (index + 1) && (index + 1) <= x.Cpfzdtperiodofin) == null : false;


                    if (esBlancoActual)
                    {
                        sublistaTmp.Add(index);

                        if (!esBlancoSiguiente)
                        {
                            agrupSubLista.Add(sublistaTmp);
                            sublistaTmp = new List<int>();
                        }

                    }

                }

                foreach (var subl in agrupSubLista)
                {
                    int periodoIni = subl.First();
                    int periodoFin = subl.Last();

                    var objClone = (CpForzadoDetDTO)forzadoDet.Clone();
                    objClone.Cpfzdtperiodoini = periodoIni;
                    objClone.Cpfzdtperiodofin = periodoFin;
                    objClone.EsFicticio = true;
                    lstForzadoDet.Add(objClone);
                }
            }

            return lstForzadoDet;
        }

        #endregion

        #region COMPROMISO HIDRÁULICO

        /// <summary>
        /// Deveulve el html de la tabla para una pestaña (sin/con compromiso)
        /// </summary>
        /// <param name="jsModel"></param>
        /// <param name="fecha"></param>
        /// <param name="lstEmpresas"></param>
        /// <param name="tipoCompromiso"></param>
        /// <returns></returns>
        public string GenerarHtmlCompromisoHidraulico(FormatoMedicion.FormatoModel jsModel, DateTime fecha, List<SiEmpresaDTO> lstEmpresas, int tipoCompromiso)
        {
            ListarDataCompromisoHidraulico(tipoCompromiso, fecha, jsModel, lstEmpresas, out List<CompromisoHidraulico> listaData, out List<MePtomedicionDTO> listaPtos);
            listaPtos = listaPtos.OrderBy(x => x.Ptomedicodi).ToList();

            if (!listaData.Any())
            {
                return string.Empty;
            }

            string prefijo = "";
            if (tipoCompromiso == ConstantesYupanaContinuo.PestaniaSinCompromiso) prefijo = "SC";
            if (tipoCompromiso == ConstantesYupanaContinuo.PestaniaConCompromiso) prefijo = "CC";

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 2;
            nfi.NumberDecimalSeparator = ",";

            //Cabeceras
            StringBuilder strHtml = new StringBuilder();
            strHtml.AppendFormat(@"
                    <table border='0' class='pretty tabla-adicional' cellspacing='0' width='100%' id='tabla_{0}'>
                            <thead>
                                <tr> ", prefijo);

            strHtml.AppendFormat("<th style='background:#16365D;' >Empresa</th>");
            foreach (var reg in listaPtos)
            {
                strHtml.AppendFormat(@"<th style='background:#16365D;' >{0}</th>", reg.Emprnomb);
            }
            strHtml.Append("</tr>");


            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='background:#16365D;' >Estación Hidrológica</th>");
            foreach (var reg in listaPtos)
            {
                strHtml.AppendFormat(@"<th style='background:#16365D;' >{0}</th>", reg.Central);
            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='background:#16365D;' >Embalse / Planta H. Yupana</th>");
            foreach (var reg in listaPtos)
            {
                strHtml.AppendFormat(@"<th style='background:#16365D;' >{0}</th>", reg.Recurnombre);
            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='background:#16365D;' >HORA</th>");
            foreach (var reg in listaPtos)
            {
                strHtml.AppendFormat("<th style='background:#16365D;'> <input type='checkbox' name='checkd_todo{0}_{1}_{2}' > <b id='checkd_todo{0}_{1}_{2}'> Marcar Todo</b></th>", prefijo, reg.Ptomedicodi, reg.Emprcodi);
            }
            strHtml.Append(@"
                                </tr>
                            </thead>
                            <tbody>
            ");

            //Cuerpo
            int posFila = 1;
            for (DateTime day = fecha; day < fecha.AddHours(24); day = day.AddHours(1))
            {
                int minutos = day.Hour * 60;
                int h = minutos > 0 ? (minutos / 60) + 1 : 1;

                strHtml.AppendFormat("<tr class='fila_dato' id='tr{0}_{1}'>", prefijo, posFila);
                strHtml.AppendFormat(@"<td style='background:#4F81BD; color:white; font-weight:bold; text-align: center; font-size:13px;'>{4} <input type='hidden' id='hfTdFecha{0}_{1}' value='{2}' /> <input type='hidden' id='hfTdh{0}_{1}' value='{3}' /> </td>"
                                , prefijo, posFila, day.ToString(ConstantesAppServicio.FormatoFecha), h, day.ToString(ConstantesAppServicio.FormatoOnlyHora));

                foreach (var reg in listaPtos)
                {
                    CompromisoHidraulico regH = listaData.Find(x => x.PtoMedicion == reg.Ptomedicodi && x.Hx == h);

                    strHtml.AppendFormat(@"<td style='text-align: center;'><input type='checkbox' name='checkcomph{0}_{1}_{3}_{4}' id='checkcomph{0}_{1}_{3}_{4}' {2} /></td>",
                                                                     prefijo, posFila, regH != null && regH.Flag == 1 ? "checked" : string.Empty, reg.Ptomedicodi, reg.Emprcodi);
                }

                strHtml.Append("</tr>");
                posFila++;
            }

            strHtml.Append(@"
                            </tbody>
                        </table>
            ");

            return strHtml.ToString();
        }

        /// <summary>
        /// Devuele los datos de la tabla (cabecera y cuerpo)
        /// </summary>        
        /// <param name="fecha"></param>
        /// <param name="jsModel"></param>
        /// <param name="totalEmpresas"></param>
        /// <param name="listaData"></param>
        /// <param name="listaPtos"></param>
        private void ListarDataCompromisoHidraulico(int tipo, DateTime fecha, FormatoModel jsModel, List<SiEmpresaDTO> totalEmpresas,
            out List<CompromisoHidraulico> listaData, out List<MePtomedicionDTO> listaPtos)
        {
            listaData = new List<CompromisoHidraulico>();
            listaPtos = new List<MePtomedicionDTO>();

            var cabecera = jsModel.ListaHojaPto;
            var data = jsModel.Handson.ListaExcelData;

            listaPtos = cabecera.Select(x => new MePtomedicionDTO()
            {
                Ptomedicodi = x.Ptomedicodi,
                Central = x.Equinomb,
                Emprnomb = totalEmpresas.Find(e => e.Emprcodi == x.Emprcodi).Emprnomb,
                Emprcodi = x.Emprcodi
            }).ToList();

            SetearPuntosMedicionEstacionHidrologica(tipo, fecha, listaPtos);

            listaData = new List<CompromisoHidraulico>();
            int col = 0;
            foreach (var regPto in listaPtos)
            {
                col++;

                for (int i = 1; i <= 24; i++)
                {
                    string strValor = data[i + 2][col];
                    if (strValor == "") strValor = "0";
                    int tienedisp = Int32.Parse(strValor);

                    listaData.Add(new CompromisoHidraulico()
                    {
                        Emprcodi = regPto.Emprcodi.Value,
                        Emprnomb = regPto.Emprnomb,
                        PtoMedicion = regPto.Ptomedicodi,
                        Equinomb = regPto.Central,
                        Fecha = fecha,
                        Hx = i,
                        Flag = tienedisp,
                    });
                }
            }
        }

        private void SetearPuntosMedicionEstacionHidrologica(int tipo, DateTime fechaConsulta, List<MePtomedicionDTO> listaPtoCab)
        {
            CpTopologiaDTO topologiaASimular = ObtenerTopologiaByDate(fechaConsulta);
            if (topologiaASimular != null && listaPtoCab.Any())
            {
                ListarInsumoCompromisoHidraulico(fechaConsulta, 0, topologiaASimular.Topcodi, tipo, listaPtoCab);
            }
        }

        /// <summary>
        /// Devuelve todos los datos de la tabla y su historial de cambios
        /// </summary>
        /// <param name="idFormato"></param>
        /// <param name="formatoPto"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="envioFecha"></param>
        /// <param name="esUltima"></param>
        /// <param name="totalEnvios"></param>
        /// <returns></returns>
        public FormatoModel ContenidoTabla(int idFormato, int formatoPto, int idEnvio, DateTime fecha, out string envioFecha, out bool esUltima, out int totalEnvios)
        {
            FormatoModel model = new FormatoModel();

            model.Handson = new HandsonModel();
            model.Handson.ListaMerge = new List<CeldaMerge>();
            model.Handson.ListaColWidth = new List<int>();
            model.IdEnvio = idEnvio;

            model.Formato = formatoServicio.GetByIdMeFormato(formatoPto);

            if (model.Formato == null)
            {
                string msgError = formatoPto == 125 ? "No existe el formato para las Centrales Con Compromiso." : (formatoPto == 126 ? "No existe el formato para las Centrales Sin Compromiso." : "Formato no encontrado.");
                throw new ArgumentException(msgError);
            }

            var cabecera = hidrologiaServicio.GetListMeCabecera().Where(x => x.Cabcodi == model.Formato.Cabcodi).FirstOrDefault();
            model.Formato.Formatcols = cabecera.Cabcolumnas;
            model.Formato.Formatrows = cabecera.Cabfilas;
            model.Formato.Formatheaderrow = cabecera.Cabcampodef;

            model.ColumnasCabecera = model.Formato.Formatcols;
            model.FilasCabecera = model.Formato.Formatrows;
            model.Formato.FechaProceso = fecha;
            formatoServicio.GetSizeFormato2(model.Formato);

            model.ListaHojaPto = hidrologiaServicio.GetByCriteria2MeHojaptomeds(ConstantesYupanaContinuo.Todos, formatoPto, cabecera.Cabquery, fecha, model.Formato.FechaFin)
            .Where(x => x.Hojaptoactivo == 1).ToList();

            model.ListaEnvios = hidrologiaServicio.GetByCriteriaMeEnvios(ConstantesYupanaContinuo.Todos, idFormato, model.Formato.FechaInicio);
            totalEnvios = model.ListaEnvios.Count;

            esUltima = false;
            int idUltEnvio = 0; //Si se esta consultando el ultimo envio se podra activar el boton editar
            if (model.ListaEnvios.Count > 0)
            {
                idUltEnvio = model.ListaEnvios[model.ListaEnvios.Count - 1].Enviocodi;
                if (idUltEnvio == idEnvio) esUltima = true;
                var reg = model.ListaEnvios.Find(x => x.Enviocodi == idEnvio);
                if (reg != null)
                    model.FechaEnvio = ((DateTime)reg.Enviofecha).ToString(ConstantesAppServicio.FormatoFechaFull);
            }

            List<object> lista = new List<object>(); // Contiene los valores traidos de de BD del envio seleccionado.
            List<MeCambioenvioDTO> listaCambios = new List<MeCambioenvioDTO>(); // contiene los cambios de que ha habido en el envio que se esta consultando.
            int nBloques = model.Formato.Formathorizonte * model.Formato.RowPorDia;
            int nCol = model.ListaHojaPto.Count;

            model.ListaCambios = new List<CeldaCambios>();
            if (formatoPto == ConstantesYupanaContinuo.FormatoSinCompromiso || formatoPto == ConstantesYupanaContinuo.FormatoConCompromiso)
                model.Handson.ListaExcelData = formatoServicio.InicializaMatrizExcel(model.Formato.Formatrows, nBloques, model.Formato.Formatcols, nCol);

            if (idEnvio >= 0) // Es nuevo envio(se consulta el ultimo envio) o solo se consulta envio seleccionado de la BD
            {
                lista = this.formatoServicio.GetDataFormato(ConstantesYupanaContinuo.Todos, model.Formato, idEnvio, idUltEnvio);

                if (idEnvio > 0) //Si se esta consultando un envio anterior se obtienen los cambios de ese envio.
                    listaCambios = this.formatoServicio.GetAllCambioEnvio(idFormato, model.Formato.FechaInicio, model.Formato.FechaFin, idEnvio, ConstantesYupanaContinuo.Todos).Where(x => x.Enviocodi == idEnvio).ToList();

                // Cargar Datos en Arreglo para Web
                if (formatoPto == ConstantesYupanaContinuo.FormatoSinCompromiso || formatoPto == ConstantesYupanaContinuo.FormatoConCompromiso)
                    formatoServicio.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);

            }

            //Nota para mostrar el envio que se muestra en pantalla
            envioFecha = "";
            if (model.ListaEnvios.Count > 0)
            {
                MeEnvioDTO envio = model.ListaEnvios.Find(x => x.Enviocodi == idEnvio);
                envioFecha = envio != null ? "Versión: " + envio.Enviocodi + ",  registrada el " + envio.Enviofecha.Value.ToString(ConstantesAppServicio.FormatoFechaFull) : "";
            }

            return model;

        }

        /// <summary>
        /// Devuelve listado de mediciones24 a partir de informacion de la tabla
        /// </summary>
        /// <param name="lstNoSeleccionados"></param>
        /// <param name="ptos"></param>
        /// <param name="lectcodi"></param>
        /// <param name="colHead"></param>
        /// <param name="nCol"></param>
        /// <param name="filaHead"></param>
        /// <param name="nFil"></param>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> LeerTabla24(List<CompromisoHidraulico> lstNoSeleccionados, List<MeHojaptomedDTO> ptos, int lectcodi, int colHead, int nCol, int filaHead, int nFil, DateTime fechaConsulta)
        {
            List<MeMedicion24DTO> lista = new List<MeMedicion24DTO>();
            MeMedicion24DTO reg = new MeMedicion24DTO();
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;

            for (var columna_x = 1; columna_x < nCol; columna_x++)
            {
                var ptomedicodi_ = ptos[columna_x - 1].Ptomedicodi;
                var dataPorPto = lstNoSeleccionados != null ? lstNoSeleccionados.Where(x => x.PtoMedicion == ptomedicodi_).ToList() : new List<CompromisoHidraulico>();

                for (var fila_x = 0; fila_x < nFil; fila_x++)
                {
                    //verificar inicio de dia
                    if ((fila_x % 24) == 0)
                    {
                        if (fila_x != 0)   //nunca entra
                            lista.Add(reg);
                        reg = new MeMedicion24DTO();
                        reg.Ptomedicodi = ptomedicodi_;
                        reg.Lectcodi = lectcodi;
                        reg.Tipoinfocodi = (int)ptos[columna_x - 1].Tipoinfocodi;
                        reg.Emprcodi = ptos[columna_x - 1].Emprcodi;
                        reg.Medifecha = new DateTime(fechaConsulta.Year, fechaConsulta.Month, fechaConsulta.Day);

                        stValor = dataPorPto.Find(x => x.Hx == 1) != null ? "0" : "1";
                        valor = decimal.Parse(stValor);
                        reg.H1 = valor;
                    }
                    else
                    {
                        stValor = dataPorPto.Find(x => x.Hx == fila_x + 1) != null ? "0" : "1";

                        int indice = fila_x % 24 + 1;
                        valor = decimal.Parse(stValor);
                        reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, valor);
                    }
                }
                lista.Add(reg);
            }

            return lista;
        }

        /// <summary>
        /// Devuelve el html de la tabla del Historial
        /// </summary>
        /// <param name="jsModelSC"></param>
        /// <param name="tipoCompromiso"></param>
        /// <returns></returns>
        public string GenerarHtmlListadoVersion(FormatoModel jsModelSC, int tipoCompromiso)
        {
            List<MeEnvioDTO> lstEnvios = jsModelSC.ListaEnvios.OrderByDescending(x => x.Enviocodi).ToList();

            string prefijo = "";
            if (tipoCompromiso == ConstantesYupanaContinuo.PestaniaSinCompromiso) prefijo = "SC";
            if (tipoCompromiso == ConstantesYupanaContinuo.PestaniaConCompromiso) prefijo = "CC";

            StringBuilder strHtml = new StringBuilder();
            strHtml.AppendFormat(@"
                    <div style='clear:both; height:5px'></div>
                       <table id='tablalenvio{0}' border='1' class='pretty tabla-adicional' cellspacing='0'>
                            <thead><tr><th>Nro</th><th>Id Envío</th><th>Fecha Hora</th><th>Usuario</th></tr></thead>
                                <tbody>", prefijo);

            int numEnvio = 0;
            foreach (var regCambio in lstEnvios)
            {
                numEnvio++;
                DateTime? fechaEnvio = regCambio.Enviofecha;
                string strFecha = fechaEnvio != null ? fechaEnvio.Value.ToString(ConstantesAppServicio.FormatoFechaFull) : "";

                strHtml.AppendFormat(@"<tr onclick='mostrarEnvioExcelWeb({1},{2});' style='cursor:pointer'>
                                       <td style='text-align: center;'>{0}</td>
                                       <td style='text-align: center;'>{2}</td>", numEnvio, tipoCompromiso, regCambio.Enviocodi);
                strHtml.AppendFormat(@"<td style='text-align: center;'>{0}</td>", strFecha);
                strHtml.AppendFormat(@"<td>{0}</td></tr>", regCambio.Lastuser);

            }

            strHtml.Append(@"</tbody></table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Devuelve los ultimos envioCodis para las tablas Sin/Con compromiso
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <param name="ultimoIdEnvioSC"></param>
        /// <param name="ultimoIdEnvioCC"></param>
        public void ObtenerUltimosEnvios(DateTime fechaConsulta, out int ultimoIdEnvioSC, out int ultimoIdEnvioCC)
        {
            ultimoIdEnvioSC = 0;
            ultimoIdEnvioCC = 0;

            HidrologiaAppServicio hidrologiaServicio = new HidrologiaAppServicio();

            List<MeEnvioDTO> lstEnviosSC = hidrologiaServicio.GetByCriteriaMeEnvios(ConstantesYupanaContinuo.Todos, ConstantesYupanaContinuo.FormatoSinCompromiso, fechaConsulta);
            if (lstEnviosSC.Count > 0) ultimoIdEnvioSC = lstEnviosSC[lstEnviosSC.Count - 1].Enviocodi;

            List<MeEnvioDTO> lstEnviosCC = hidrologiaServicio.GetByCriteriaMeEnvios(ConstantesYupanaContinuo.Todos, ConstantesYupanaContinuo.FormatoConCompromiso, fechaConsulta);
            if (lstEnviosCC.Count > 0) ultimoIdEnvioCC = lstEnviosCC[lstEnviosCC.Count - 1].Enviocodi;
        }

        #endregion

        #region Configuracion de Caudal y Generación RER para Yupana Continuo

        /// <summary>
        /// Guardar configuracion de puntos de medicion de un recurso (aporte, planta)
        /// </summary>
        /// <param name="yupcfgcodi"></param>
        /// <param name="recurcodi"></param>
        /// <param name="listaConfWeb"></param>
        /// <param name="username"></param>
        public void GuardarConfiguracionXRecurso(int yupcfgcodi, int recurcodi, List<CpYupconCfgdetDTO> listaConfWeb, string username)
        {
            CpYupconCfgDTO objConf = GetByIdCpYupconCfg(yupcfgcodi);
            List<CpYupconCfgdetDTO> listaConfBD = GetByCriteriaCpYupconCfgdets(yupcfgcodi, recurcodi);

            //la configuracion 
            List<CpYupconCfgdetDTO> listaActivo = new List<CpYupconCfgdetDTO>();
            foreach (var reg in listaConfWeb)
            {
                var regBD = listaConfBD.Find(x => x.Ptomedicodi == reg.Ptomedicodi);
                if (regBD != null)
                {
                    var regClone = (CpYupconCfgdetDTO)regBD.Clone();
                    regClone.Ycdetfactor = reg.Ycdetfactor;
                    regClone.Ycdetfecmodificacion = DateTime.Now;
                    regClone.Ycdetusumodificacion = username;
                    regClone.Ycdetactivo = 1;
                    listaActivo.Add(regClone);
                }
                else
                {
                    reg.Yupcfgcodi = yupcfgcodi;
                    reg.Topcodi = objConf.Topcodi;
                    reg.Recurcodi = recurcodi;
                    reg.Ycdetfecregistro = DateTime.Now;
                    reg.Ycdetusuregistro = username;
                    reg.Ycdetactivo = 1;
                    listaActivo.Add(reg);
                }
            }

            //dar de baja lo que esta en bd
            List<CpYupconCfgdetDTO> listaBaja = new List<CpYupconCfgdetDTO>();
            foreach (var regBD in listaConfBD)
            {
                var regClone = (CpYupconCfgdetDTO)regBD.Clone();
                regClone.Ycdetfecmodificacion = DateTime.Now;
                regClone.Ycdetusumodificacion = username;
                regClone.Ycdetactivo = 0;
                listaBaja.Add(regClone);
            }

            //nuevos registros y dar de baja
            foreach (var reg in listaActivo)
            {
                SaveCpYupconCfgdet(reg);
            }
            foreach (var reg in listaBaja)
            {
                UpdateCpYupconCfgdet(reg);
            }
        }

        public void ListarFiltroConfiguracionRecurso(int tyupcodi, int topcodi, out List<CpRecursoDTO> listaRecurso, out List<MePtomedicionDTO> listaPto)
        {
            List<int> categoriasYupana = new List<int>();
            string formatcodis = "";
            int tipoinfocodi = 0;
            if (ConstantesYupanaContinuo.TipoConfiguracionCaudal == tyupcodi)
            {
                categoriasYupana = new List<int>() { ConstantesBase.Embalse, ConstantesBase.PlantaH };
                formatcodis = "6,5,32,7,43";
                tipoinfocodi = ConstantesAppServicio.TipoinfocodiM3s;
            }

            if (ConstantesYupanaContinuo.TipoConfiguracionRer == tyupcodi)
            {
                categoriasYupana.Add(ConstantesBase.PlantaNoConvenO);
                formatcodis = "62";
                tipoinfocodi = ConstantesAppServicio.TipoinfocodiMW;
            }

            //Embalses, plantas
            listaRecurso = new List<CpRecursoDTO>();
            foreach (var catcodi in categoriasYupana)
            {
                listaRecurso.AddRange(FactorySic.GetCpRecursoRepository().ListaRecursoXCategoria(catcodi, topcodi));
            }
            listaRecurso = listaRecurso.OrderBy(x => x.Recurnombre).ToList();

            //Puntos de medicion
            List<MeHojaptomedDTO> listaHpt = FactorySic.GetMeHojaptomedRepository().ListarHojaPtoByFormatoAndEmpresa(-1, formatcodis).Where(x => x.Hojaptoactivo == 1).ToList();
            listaPto = listaHpt.Where(x => x.Tipoinfocodi == tipoinfocodi).GroupBy(x => x.Ptomedicodi)
                                                    .Select(x => new MePtomedicionDTO()
                                                    {
                                                        Ptomedicodi = x.Key,
                                                        Ptomedielenomb = x.First().PtoMediEleNomb,
                                                    }).OrderBy(x => x.Ptomedielenomb).ToList();
        }

        public int GetConfiguracionBaseXTipo(int tyupcodi)
        {
            if (ConstantesYupanaContinuo.TipoConfiguracionCaudal == tyupcodi)
            {
                return ConstantesYupanaContinuo.ConfiguracionBaseCaudal;
            }
            else
            {
                return ConstantesYupanaContinuo.ConfiguracionBaseRer;
            }
        }

        public List<CpYupconCfgdetDTO> ListarReporteConfiguracionXTipo(int yupcfgcodi)
        {
            List<CpYupconCfgdetDTO> listaConfBD = GetByCriteriaCpYupconCfgdets(yupcfgcodi, 0);

            //listar un registro por recurso
            List<CpYupconCfgdetDTO> listaAgrpXRec = new List<CpYupconCfgdetDTO>();

            foreach (var sublista in listaConfBD.GroupBy(x => x.Recurcodi))
            {
                CpYupconCfgdetDTO obj = sublista.First();
                obj.Ptomedielenomb = string.Join("|", sublista.Select(x => string.Format("{0} - {1}", x.Ptomedicodi, x.Ptomedielenomb)));
                obj.FactorDesc = string.Join("|", sublista.Select(x => x.Ycdetfactor));

                obj.UltimaModificacionFechaDesc = obj.Ycdetfecmodificacion != null ? obj.Ycdetfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : obj.Ycdetfecregistro.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
                obj.UltimaModificacionUsuarioDesc = obj.Ycdetfecmodificacion != null ? obj.Ycdetusumodificacion : obj.Ycdetusuregistro;

                listaAgrpXRec.Add(obj);
            }

            return listaAgrpXRec.OrderBy(x => x.Recurnombre).ToList();
        }

        public CpYupconCfgDTO GetUltimaConfiguracionXTipo(int tyupcodi, DateTime fechaHoraActual, bool masReciente)
        {
            //obtener la configuracion más reciente anterior a la hora a procesar
            CpYupconCfgDTO objUltimoEnvio = null;

            if (masReciente)
            {
                objUltimoEnvio = GetByCriteriaCpYupconCfgs(tyupcodi, fechaHoraActual, -1)
                                                    .Where(x => x.Yupcfgbloquehorario < fechaHoraActual.Hour).FirstOrDefault();
            }
            else
            {
                objUltimoEnvio = GetByCriteriaCpYupconCfgs(tyupcodi, fechaHoraActual, fechaHoraActual.Hour).FirstOrDefault();
            }

            return objUltimoEnvio;
        }

        private void ActualizarAutomaticoConfiguracionXTipo(string usuario, int topcodi, DateTime fechaHoraActual, int tyupcodi)
        {
            DateTime fechaRegistro = DateTime.Now;
            List<CpYupconCfgdetDTO> listaDetNueva = new List<CpYupconCfgdetDTO>();

            //obtener la configuracion más reciente anterior a la hora a procesar
            CpYupconCfgDTO objUltimoEnvio = GetUltimaConfiguracionXTipo(tyupcodi, fechaHoraActual, true);

            //Si la hora es 00:00 o no existe configuracion para ese dia, copiar la configuracion BASE
            if (fechaHoraActual.Hour == 0 || objUltimoEnvio == null)
            {
                //Embalses/Plantas de la topologia
                ListarFiltroConfiguracionRecurso(tyupcodi, topcodi, out List<CpRecursoDTO> listaRecurso, out List<MePtomedicionDTO> listaPto);

                //Configuracion base
                int yupcfgcodi = GetConfiguracionBaseXTipo(tyupcodi);
                List<CpYupconCfgdetDTO> listaConfBD = GetByCriteriaCpYupconCfgdets(yupcfgcodi, 0);

                //Configuracion base para la topologia seleccionada
                foreach (var obj in listaRecurso)
                {
                    var listaBDXRec = listaConfBD.Where(x => x.Recurcodi == obj.Recurcodi).ToList();
                    listaDetNueva.AddRange(listaBDXRec);
                }
            }
            else
            {
                //Copiar la última configuracion
                List<CpYupconCfgdetDTO> listaConfBD = GetByCriteriaCpYupconCfgdets(objUltimoEnvio.Yupcfgcodi, 0);
                listaDetNueva.AddRange(listaConfBD);
            }

            //Guardar configuracion
            CpYupconCfgDTO objCab = new CpYupconCfgDTO()
            {
                Topcodi = topcodi,
                Tyupcodi = tyupcodi,
                Yupcfgfecha = fechaHoraActual.Date,
                Yupcfgbloquehorario = fechaHoraActual.Hour,
                Yupcfgtipo = ConstantesYupanaContinuo.TipoConfiguracionDiario,
                Yupcfgfecregistro = fechaRegistro,
                Yupcfgusuregistro = usuario,
            };

            int yupcfgcodiNuevo = SaveCpYupconCfg(objCab);
            foreach (var objDet in listaDetNueva)
            {
                objDet.Yupcfgcodi = yupcfgcodiNuevo;
                objDet.Topcodi = 0;
                objDet.Ycdetfecregistro = fechaRegistro;
                objDet.Ycdetusuregistro = usuario;
                objDet.Ycdetfecmodificacion = null;
                objDet.Ycdetusumodificacion = null;

                SaveCpYupconCfgdet(objDet);
            }
        }

        #endregion

        #region Reporte Caudal y Generación RER

        public HandsonModel ArmarHandsonDetalleCargaConfiguracion(int tyupcodi, DateTime fecha, List<CpYupconM48DTO> listaMedicion)
        {
            List<ExpandoObject> lstaDatosFila = ListarFilasHandsonCargaConfiguracion(fecha, listaMedicion, out List<ExpandoObject> lstPropiedadesCeldas);

            string label = ConstantesYupanaContinuo.TipoConfiguracionCaudal == tyupcodi ? "Aporte" : "Generación RER";
            string unidad = ConstantesYupanaContinuo.TipoConfiguracionCaudal == tyupcodi ? "[m3/s]" : "[MW]";

            #region Cabecera

            var nestedHeader = new NestedHeaders();

            var headerRow1 = new List<CellNestedHeader>();
            var headerRow2 = new List<CellNestedHeader>();
            var headerRow3 = new List<CellNestedHeader>();

            //Primera columna
            CellNestedHeader f1c1 = new CellNestedHeader() { Label = label, }; headerRow1.Add(f1c1);
            CellNestedHeader f2c1 = new CellNestedHeader() { Label = "Tipo" }; headerRow2.Add(f2c1);
            CellNestedHeader f3c1 = new CellNestedHeader() { Label = "Fecha-hora / Unidad" }; headerRow3.Add(f3c1);

            #region Cabecera-Columnas

            var lstColumn = new List<object>()
            {
                new { data = "Periodo", className = "htCenter celdaPeriodo", readOnly = true }
            };

            var lstColumnWidth = new List<int> { 120 };

            int cont = 0;
            foreach (var obj in listaMedicion)
            {
                string recurnombre = obj.Recurnombre ?? "";
                recurnombre = recurnombre.Replace("-", " ").Replace(" ", "<br/>");

                string claseCss = cont % 2 == 1 ? "clase_impar" : "clase_par";

                CellNestedHeader f1 = new CellNestedHeader() { Label = recurnombre, Class = claseCss }; headerRow1.Add(f1);
                CellNestedHeader f2 = new CellNestedHeader() { Label = obj.Catabrev }; headerRow2.Add(f2);
                CellNestedHeader f3 = new CellNestedHeader() { Label = unidad }; headerRow3.Add(f3);

                lstColumn.Add(new { data = $"E{obj.Recurcodi}.Valor", className = "htRight", numericFormat = new { pattern = "0.000" }, type = "numeric" });
                lstColumnWidth.Add(100);

                cont++;
            }

            #endregion            

            nestedHeader.ListCellNestedHeaders.Add(headerRow1);
            nestedHeader.ListCellNestedHeaders.Add(headerRow2);
            nestedHeader.ListCellNestedHeaders.Add(headerRow3);

            #endregion

            HandsonModel handson = new HandsonModel();
            handson.NestedHeader = nestedHeader;
            handson.ListaExcelData2 = JsonConvert.SerializeObject(lstaDatosFila);
            handson.ListaColWidth = lstColumnWidth;
            handson.Columnas = lstColumn.ToArray();
            handson.Esquema = JsonConvert.SerializeObject(lstPropiedadesCeldas);

            return handson;
        }

        private List<ExpandoObject> ListarFilasHandsonCargaConfiguracion(DateTime fecha, List<CpYupconM48DTO> listaMedicion, out List<ExpandoObject> lstPropiedadesCeldas)
        {
            List<ExpandoObject> lstaData = new List<ExpandoObject>();
            List<ExpandoObject> lstaDataCells = new List<ExpandoObject>();

            int fila = -1;
            for (int h = 1; h <= 48; h++)
            {
                DateTime fechaHora = fecha.Date.AddMinutes(h * 30);
                if (h == 48) fechaHora = fechaHora.AddMinutes(-1);

                fila++;
                dynamic data = new ExpandoObject();
                data.Periodo = fechaHora.ToString(ConstantesAppServicio.FormatoFechaFull);
                int col = 0;
                string tipoFila = "";
                foreach (var objAgrup in listaMedicion)
                {
                    col++;
                    string miclase = "sinFormato";

                    decimal? val = (decimal?)objAgrup.GetType().GetProperty("H" + h).GetValue(objAgrup, null);
                    int? orig = (int?)objAgrup.GetType().GetProperty("T" + h).GetValue(objAgrup, null);

                    AddProperty(data, $"E{objAgrup.Recurcodi}", new { Valor = val, Origen = orig, ValDefecto = val, OrigenDefecto = orig });
                    if (orig != null) miclase = "td_fuente_" + orig;

                    //colores de las celdas segun procedencia
                    dynamic data2 = new ExpandoObject();
                    data2.row = fila;
                    data2.col = col;
                    data2.className = miclase;

                    lstaDataCells.Add(data2);
                }
                data.TipoFila = tipoFila;
                lstaData.Add(data);
            }

            lstPropiedadesCeldas = lstaDataCells;
            return lstaData;
        }

        /// <summary>
        /// agregar propiedad
        /// </summary>
        /// <param name="expando"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        private void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        public int GuardarDetalleCargaConfiguracion(string usuario, DateTime fechaHora, int tyupcodi, List<CpYupconM48DTO> listaDet48)
        {
            int topcodi = GetTopologiaByDate(fechaHora);

            //Guardar en bd
            CpYupconEnvioDTO objEnvio = new CpYupconEnvioDTO()
            {
                Tyupcodi = tyupcodi,
                Cyupfecha = fechaHora.Date,
                Cyupbloquehorario = fechaHora.Hour,
                Cyupfecregistro = DateTime.Now,
                Cyupusuregistro = usuario,
                Topcodi = topcodi,
            };
            int cyupcodi = SaveCpYupconEnvio(objEnvio);
            foreach (var reg48 in listaDet48)
            {
                reg48.Cyupcodi = cyupcodi;
                reg48.Topcodi = topcodi;
                SaveCpYupconM48(reg48);
            }

            return cyupcodi;
        }

        /// <summary>
        /// Devuelve lista medicion a partir de la informacion de la tabla web
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="anioIni"></param>
        /// <param name="anioFin"></param>
        /// <param name="stringJson"></param>
        /// <returns></returns>
        public List<CpYupconM48DTO> ListarDetalleCargaConfiguracionFromHandson(string stringJson)
        {
            List<CpYupconM48DTO> lstSalida = new List<CpYupconM48DTO>();
            string cadLimpio = stringJson.Replace("\"", "");
            cadLimpio = cadLimpio.Replace("[", String.Empty);
            cadLimpio = cadLimpio.Replace("]", String.Empty);
            string[] fila = cadLimpio.Split(',');

            List<string> lstCampos = fila[0].Split('|').ToList();

            int numEstaciones = lstCampos.Count - 1;

            for (int estacion = 0; estacion < numEstaciones; estacion++)
            {
                int filasuma = 0;

                CpYupconM48DTO medirango = new CpYupconM48DTO();

                var id = ((fila[0].Split('|')[estacion + 1]).Split(':')[0]).Replace("E", "").ToString();
                medirango.Recurcodi = Convert.ToInt32(id);

                for (var filaH = 1; filaH <= 48; filaH++)
                {
                    var listaPropXCelda = (fila[filasuma].Split('|')[estacion + 1]).Split(':')[1].Split('*');

                    decimal valorH = 0;// = Convert.ToDecimal(listaPropXCelda[0]);
                    decimal.TryParse(listaPropXCelda[0], out valorH);

                    int tipoH = 0;// Convert.ToInt32(listaPropXCelda[1]);
                    Int32.TryParse(listaPropXCelda[1], out tipoH);

                    medirango.GetType().GetProperty("H" + (filaH).ToString()).SetValue(medirango, valorH);
                    medirango.GetType().GetProperty("T" + (filaH).ToString()).SetValue(medirango, tipoH);
                    filasuma++;
                }

                lstSalida.Add(medirango);
            }

            return lstSalida;
        }

        public List<CpYupconM48DTO> ListarDetalleCargaConfiguracion(int tyupcodi, int cyupcodi, DateTime fechaHora)
        {
            //obtener envio reciente
            if (cyupcodi == 0)
            {
                CpYupconEnvioDTO objUltimoEnvio = GetUltimoEnvioCargaConfiguracion(tyupcodi, fechaHora);
                if (objUltimoEnvio != null) cyupcodi = objUltimoEnvio.Cyupcodi;
            }

            //lista data del envio
            List<CpYupconM48DTO> listaDet48 = new List<CpYupconM48DTO>();
            if (cyupcodi > 0)
                listaDet48 = GetByCriteriaCpYupconM48s(cyupcodi).OrderBy(x => x.Recurnombre).ToList();

            return listaDet48;
        }

        public List<CpYupconM48DTO> ListarHandsonDetalleCargaConfiguracion(int tyupcodi, DateTime fechaHora, List<CpYupconM48DTO> lista48Web)
        {
            List<CpYupconM48DTO> listaFinal = new List<CpYupconM48DTO>();

            int topcodi = GetTopologiaByDate(fechaHora);
            List<CpYupconM48DTO> listaDet48BD = CalcularDetalleM48YupanaContinuo(fechaHora, topcodi, tyupcodi);

            //recorrer cada recurso del excel web
            foreach (var objWeb in lista48Web)
            {
                var objBD = listaDet48BD.Find(x => x.Recurcodi == objWeb.Recurcodi);

                //si el recurso tiene configuracion de puntos de medición extranet entonces actualizar el recurso sino mantener
                if (objBD != null && objBD.RecursoTieneCfg)
                {
                    listaFinal.Add(objBD);
                }
                else
                {
                    if (objBD != null)
                    {
                        objWeb.Recurnombre = objBD.Recurnombre;
                        objWeb.Catcodi = objBD.Catcodi;
                    }
                    listaFinal.Add(objWeb);
                }
            }

            //formatear
            foreach (var obj in listaFinal)
                FormatearCpYupconM48DTO(obj);

            return listaFinal;
        }

        public void ActualizarAutomaticoDetalleM48YupanaContinuo(string usuario, int topcodi, DateTime fechaHora, int tyupcodi)
        {
            List<CpYupconM48DTO> listaDet48 = CalcularDetalleM48YupanaContinuo(fechaHora, topcodi, tyupcodi);

            //Guardar en bd
            CpYupconEnvioDTO objEnvio = new CpYupconEnvioDTO()
            {
                Tyupcodi = tyupcodi,
                Cyupfecha = fechaHora.Date,
                Cyupbloquehorario = fechaHora.Hour,
                Cyupfecregistro = DateTime.Now,
                Cyupusuregistro = usuario,
                Topcodi = topcodi,
            };
            int cyupcodi = SaveCpYupconEnvio(objEnvio);
            foreach (var reg48 in listaDet48)
            {
                reg48.Cyupcodi = cyupcodi;
                //el topcodi se asigna en CalcularDetalleM48YupanaContinuo
                SaveCpYupconM48(reg48);
            }
        }

        private List<CpYupconM48DTO> CalcularDetalleM48YupanaContinuo(DateTime fechaHora, int topcodi, int tyupcodi)
        {
            List<CpYupconM48DTO> listaDet48 = new List<CpYupconM48DTO>();

            int origenDefaultExtranet = tyupcodi == ConstantesYupanaContinuo.TipoConfiguracionRer ? ConstantesYupanaContinuo.OrigenExtranetIDCC : ConstantesYupanaContinuo.OrigenExtranetHidrologia;
            int origenDefaultYupana = tyupcodi == ConstantesYupanaContinuo.TipoConfiguracionRer ? ConstantesYupanaContinuo.OrigenYupanaNoConvencional : ConstantesYupanaContinuo.OrigenYupanaAporte;

            //1. Obtener datos de potencia activa IDCC
            List<MeMedicion48DTO> lista48Extranet = new List<MeMedicion48DTO>();
            if (tyupcodi == ConstantesYupanaContinuo.TipoConfiguracionRer)
            {
                lista48Extranet = FactorySic.GetMeMedicion48Repository().ListarMeMedicion48ByFiltro("93", fechaHora.Date, fechaHora.Date, ConstantesAppServicio.ParametroDefecto)
                                                  .Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMW).ToList();
            }
            else
            {

                //4. Obtener datos de caudal / volumen hidrologia TR
                string idsEmpresa = ConstantesAppServicio.ParametroDefecto;
                string idsCuenca = ConstantesAppServicio.ParametroDefecto;
                string idsFamilia = ConstantesAppServicio.ParametroDefecto;
                string idsTptoMedicion = "1,2,3,4,5,10,14,16,24,17,18,19,8,7,9,11,12,13,84";
                List<MeMedicion24DTO> lista24Hidro = hidrologiaServicio.ListaMed24Hidrologia(66, 16, idsEmpresa, idsCuenca, idsFamilia, fechaHora.Date, fechaHora.Date, idsTptoMedicion);
                List<MeMedicion24DTO> lista24HidroCaudal = lista24Hidro.Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiM3s).ToList();

                //convertir m24 a m48
                foreach (var obj24 in lista24HidroCaudal)
                {
                    MeMedicion48DTO objM48 = new MeMedicion48DTO();
                    objM48.Ptomedicodi = obj24.Ptomedicodi;
                    objM48.Tipoinfocodi = obj24.Tipoinfocodi;

                    for (int h = 1; h <= 24; h++)
                    {
                        decimal? valorH = (decimal?)obj24.GetType().GetProperty("H" + h).GetValue(obj24, null);
                        objM48.GetType().GetProperty("H" + (h * 2 - 1)).SetValue(objM48, valorH);
                        objM48.GetType().GetProperty("H" + (h * 2)).SetValue(objM48, valorH);
                    }

                    lista48Extranet.Add(objM48);
                }
            }
            //2. obtener Generación RER del escenario yupana 
            List<CpMedicion48DTO> lista48Yupana = new List<CpMedicion48DTO>();
            if (tyupcodi == ConstantesYupanaContinuo.TipoConfiguracionRer)
            {
                lista48Yupana = FactorySic.GetCpMedicion48Repository().GetByCriteria(topcodi.ToString(), fechaHora.Date, ConstantesBase.SRES_GENER_RER.ToString());
            }
            else
            {
                lista48Yupana = FactorySic.GetCpMedicion48Repository().GetByCriteria(topcodi.ToString(), fechaHora.Date, ConstantesBase.SRES_APORTES_EMB + "," + ConstantesBase.SRES_APORTES_PH);
            }

            //3. configuracion
            CpYupconCfgDTO objUltimoEnvio = GetUltimaConfiguracionXTipo(tyupcodi, fechaHora, false);
            List<CpYupconCfgdetDTO> listaConfiguracion = GetByCriteriaCpYupconCfgdets(objUltimoEnvio.Yupcfgcodi, 0);

            //Flag para para utilizar la informacion de Extranet
            if (ConfigurationManager.AppSettings[ConstantesYupanaContinuo.ActualizarInsumoYupanaContinuo] != "S")
            {
                listaConfiguracion = new List<CpYupconCfgdetDTO>();
            }

            //Generar data
            foreach (var objRec in lista48Yupana.GroupBy(x => x.Recurcodi))
            {
                int recurcodi = objRec.Key;
                CpYupconM48DTO objM48 = new CpYupconM48DTO();
                objM48.Recurcodi = recurcodi;
                objM48.Recurnombre = objRec.First().Recurnombre;
                objM48.Catcodi = objRec.First().Catcodi;
                objM48.Topcodi = topcodi;

                //configuracion por recurso
                var listaConfiguracionXRec = listaConfiguracion.Where(x => x.Recurcodi == recurcodi).ToList();
                List<int> lPtomedicodi = listaConfiguracionXRec.Select(x => x.Ptomedicodi).ToList();
                objM48.RecursoTieneCfg = lPtomedicodi.Any();

                //obtener datos yupana e Idcc del recurso
                CpMedicion48DTO objYupXRec = lista48Yupana.Find(x => x.Recurcodi == recurcodi);
                List<MeMedicion48DTO> listaExtranetXRec = lista48Extranet.Where(x => lPtomedicodi.Contains(x.Ptomedicodi)).ToList();

                for (int h = 1; h <= 48; h++)
                {
                    decimal? valorH = null;
                    int origenH = 0;

                    //si existe Datos cargados por el agente en la Extranet
                    if (listaConfiguracionXRec.Any() && listaExtranetXRec.Any())
                    {
                        bool esCentralSolar = listaConfiguracionXRec.Any(x => x.Famcodi == ConstantesHorasOperacion.IdTipoSolar || x.Famcodi == ConstantesHorasOperacion.IdGeneradorSolar);

                        foreach (var regPto in listaConfiguracionXRec)
                        {
                            var regM48Idcc = listaExtranetXRec.Find(x => x.Ptomedicodi == regPto.Ptomedicodi);
                            if (regM48Idcc != null)
                            {
                                decimal? valorHPto = (decimal?)regM48Idcc.GetType().GetProperty("H" + h).GetValue(regM48Idcc, null);
                                if (valorHPto != null)
                                {
                                    valorH = valorH.GetValueOrDefault(0) + (valorHPto.GetValueOrDefault(0) * regPto.Ycdetfactor);
                                    origenH = origenDefaultExtranet;
                                }
                            }
                        }

                        //si es central solar, las horas anteriores a 04:30am y posteriores a 06:30pm deben ser cero
                        if (esCentralSolar && valorH == null)
                        {
                            valorH = 0;
                            origenH = origenDefaultExtranet;
                        }
                    }

                    if (valorH == null)
                    {
                        if (objYupXRec != null) valorH = (decimal?)objYupXRec.GetType().GetProperty("H" + h).GetValue(objYupXRec, null);
                        origenH = origenDefaultYupana;
                    }

                    objM48.GetType().GetProperty("H" + h).SetValue(objM48, valorH);
                    objM48.GetType().GetProperty("T" + h).SetValue(objM48, origenH);
                }

                listaDet48.Add(objM48);
            }

            return listaDet48;
        }

        private CpYupconEnvioDTO GetUltimoEnvioCargaConfiguracion(int tyupcodi, DateTime fechaHora)
        {
            return GetByCriteriaCpYupconEnvios(tyupcodi, fechaHora, fechaHora.Hour).FirstOrDefault();
        }

        public void ActualizarAutomaticoConfyDetalleM48YupanaContinuo(int tyupcodi, DateTime fechaHora, string usuario)
        {
            //Verificar que existencia de escenario Yupana
            int topcodi = GetTopologiaByDate(fechaHora);
            if (topcodi == 0)
            {
                throw new ArgumentException(ConstantesYupanaContinuo.MensajeNoExisteTopologia);
            }

            //Configuración
            ActualizarAutomaticoConfiguracionXTipo(usuario, topcodi, fechaHora, tyupcodi);

            //Reporte
            ActualizarAutomaticoDetalleM48YupanaContinuo(usuario, topcodi, fechaHora, tyupcodi);
        }


        /// <summary>
        /// Genera el reporte excel para el envio 
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="cbenvcodi"></param>
        public void GenerarArchivoExcelDetalleM48YupanaContinuo(string ruta, string pathLogo, int tyupcodi, int cyupcodi, DateTime fechaHora, out string nombreArchivo)
        {
            List<CpYupconM48DTO> listaDet48 = ListarDetalleCargaConfiguracion(tyupcodi, cyupcodi, fechaHora);

            string fileTipo = ((tyupcodi == ConstantesYupanaContinuo.TipoConfiguracionCaudal) ? "ReporteCaudales" : "ReporteGeneraciónRER");
            nombreArchivo = string.Format("{0}_{1}.xlsx", fileTipo, fechaHora.ToString(ConstantesAppServicio.FormatoFechaYMD2));
            string rutaFile = ruta + nombreArchivo;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {

                GenerarHojaExcelDetalleM48YupanaContinuo(xlPackage, pathLogo, tyupcodi, fechaHora.Date, listaDet48);

                xlPackage.Save();

            }
        }

        /// <summary>
        /// Genera el archivo excel del formulario para un envío
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="modeloWeb"></param>
        private void GenerarHojaExcelDetalleM48YupanaContinuo(ExcelPackage xlPackage, string pathLogo, int tyupcodi, DateTime fecha, List<CpYupconM48DTO> listaDetalle)
        {
            ExcelWorksheet ws = null;
            string nameWS = "FORMATO";
            string titulo = ((tyupcodi == ConstantesYupanaContinuo.TipoConfiguracionCaudal) ? "Reporte de Caudales - " : "Reporte de Generación RER - ") + fecha.ToString(ConstantesAppServicio.FormatoFecha);

            string label = ConstantesYupanaContinuo.TipoConfiguracionCaudal == tyupcodi ? "Aporte" : "Generación RER";
            string unidad = ConstantesYupanaContinuo.TipoConfiguracionCaudal == tyupcodi ? "[m3/s]" : "[MW]";

            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            //Logo
            UtilExcel.AddImageLocal(ws, 1, 0, pathLogo);

            if (listaDetalle.Any())
            {
                //Estilos titulo
                string font = "Calibri";
                int colIniTitulo = 3;
                int rowIniTitulo = 2;
                ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colIniTitulo, font, 16);
                UtilExcel.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colIniTitulo);

                int rowIniTabla = 4;
                int rowIniData = rowIniTabla + 3;
                int rowData = rowIniData;
                int colIniTable = 2;

                //cabecera
                int rowCab1 = rowData; //las otras filas anteriores estaran ocultas
                int rowCab2 = rowCab1 + 1;
                int rowCab4 = rowCab2 + 1;

                int colHora = colIniTable;
                int col = colHora;

                ws.Cells[rowCab1, colHora].Value = label;
                ws.Cells[rowCab2, colHora].Value = "Tipo";
                ws.Cells[rowCab4, colHora].Value = "Fecha-hora / Unidad";

                col = colHora + 1;
                ws.Column(1).Width = 2;
                ws.Row(rowIniTabla + 0).Height = 0;

                ws.Column(colHora).Width = 20;
                foreach (var objAgrup in listaDetalle)
                {
                    ws.Cells[rowIniTabla + 0, col].Value = objAgrup.Recurcodi;

                    ws.Cells[rowCab1, col].Value = objAgrup.Recurnombre.Replace(" ", "\n");
                    ws.Cells[rowCab2, col].Value = objAgrup.Catabrev;
                    ws.Cells[rowCab4, col].Value = unidad;

                    ws.Column(col).Width = 17;

                    col++;
                }
                int colFin = colHora + listaDetalle.Count;
                UtilExcel.SetFormatoCelda(ws, rowCab1, colHora, rowCab1, colFin, "Centro", "Centro", "#000000", "#D9E1F2", font, 11, false, true);
                UtilExcel.SetFormatoCelda(ws, rowCab2, colHora, rowCab2, colFin, "Centro", "Centro", "#FFFFFF", "#2F75B5", font, 11, false, true);
                UtilExcel.SetFormatoCelda(ws, rowCab4, colHora, rowCab4, colFin, "Centro", "Centro", "#000000", "#D9E1F2", font, 11, false, true);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowCab1, colHora, rowCab1, colFin, "#000000", true);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowCab2, colHora, rowCab2, colFin, "#000000", true);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowCab4, colHora, rowCab4, colFin, "#000000", true);

                //data
                rowData += 3;
                for (int h = 1; h <= 48; h++)
                {
                    DateTime fechaHora = fecha.AddMinutes(h * 30);
                    if (h == 48) fechaHora = fechaHora.AddMinutes(-1);

                    ws.Cells[rowData, colHora].Value = fechaHora.ToString(ConstantesAppServicio.FormatoFechaFull);
                    UtilExcel.SetFormatoCelda(ws, rowData, colHora, rowData, colHora, "Centro", "Centro", "#FFFFFF", "#2F75B5", font, 11, false, true);

                    col = colHora + 1;
                    foreach (var objAgrup in listaDetalle)
                    {
                        decimal? val = (decimal?)objAgrup.GetType().GetProperty("H" + h).GetValue(objAgrup, null);
                        int? orig = (int?)objAgrup.GetType().GetProperty("T" + h).GetValue(objAgrup, null);

                        //fondo
                        string sColorFondo = "#FFFFFF"; //blanco 1,4
                        switch (orig ?? 0)
                        {
                            case ConstantesYupanaContinuo.OrigenYupanaAporte:
                                sColorFondo = "#22AEE2"; //celeste
                                break;
                            case ConstantesYupanaContinuo.OrigenYupanaNoConvencional:
                                sColorFondo = "#4AB516"; //verde
                                break;
                            case ConstantesYupanaContinuo.OrigenEdicionManual:
                                sColorFondo = "#7149A7"; //morado
                                break;
                        }

                        //texto
                        string sColorTexto = "#000000"; //negro 1,2,3,4,5,6,7
                        if (val < 0) sColorTexto = "#ff0000";

                        //negrita
                        bool celdaNegrita = false;
                        switch (orig ?? 0)
                        {
                            case ConstantesYupanaContinuo.OrigenEdicionManual:
                                celdaNegrita = true;
                                break;
                        }

                        ws.Cells[rowData, col].Value = val;
                        UtilExcel.SetFormatoCelda(ws, rowData, col, rowData, col, "Centro", "Centro", sColorTexto, sColorFondo, font, 11, celdaNegrita, true);

                        col++;
                    }

                    UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colHora, rowData, colFin, "#000000", true);
                    UtilExcel.CeldasExcelFormatoNumero(ws, rowData, colHora + 1, rowData, colFin, 3);

                    rowData++;
                }

                //border doble
                col = colHora + 1;
                foreach (var listaAgrup in listaDetalle.GroupBy(x => x.Recurcodi))
                {
                    int count = listaAgrup.Count();
                    UtilExcel.BorderCeldasLineaDelgada(ws, rowIniData, col, rowIniData + 3 + 48 - 1, col + count - 1, "#000000");
                    col += count;
                }

                ws.View.FreezePanes(rowIniData + 3, colHora + 1);
            }

            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 80;
        }


        #endregion

        #region Verificación de Proyecciones RER (Información de Potencia mínima y efectiva)

        /// <summary>
        /// Listar limites de los equipos rer segun fecha de consulta
        /// </summary>
        /// <param name="fechaConsulta">Fecha de consulta, por defecto hoy</param>
        /// <returns></returns>
        public List<RDOLimiteRer> ListarLimitesEquiposRER(DateTime fechaConsulta)
        {
            //tipos de equipos válidos
            List<int> listaTeq = new List<int>() { 4, 5, 37, 39, 2, 3, 36, 38 };

            //puntos de medición RER
            List<MeHojaptomedDTO> listaPtoRer = FactorySic.GetMeHojaptomedRepository().ListarHojaPtoByFormatoAndEmpresa(-1, "112").Where(x => x.Hojaptoactivo == 1).ToList();

            //potencias efectivas de los equipos
            List<EqEquipoDTO> listaPotencia = ListarPminPeEquipos(fechaConsulta.Date);

            //generar resultado
            List<RDOLimiteRer> listaFinal = new List<RDOLimiteRer>();
            foreach (var regPto in listaPtoRer)
            {
                RDOLimiteRer regLim = new RDOLimiteRer()
                {
                    Ptomedicodi = regPto.Ptomedicodi,
                    Ptomedidesc = (regPto.Ptomedidesc ?? "").Trim(),
                    Tipoinfocodi = ConstantesAppServicio.TipoinfocodiMW,

                    Emprcodi = regPto.Emprcodi,
                    Emprnomb = (regPto.Emprnomb ?? "").Trim(),

                    Famcodi = regPto.Famcodi,
                    Equicodi = regPto.Equicodi,
                    Equinomb = (regPto.Equinomb ?? "").Trim()
                };

                //buscar equipo del punto y obtener minimos y maximos
                var regEq = listaPotencia.Find(x => x.Equicodi == regPto.Equicodi);
                if (regEq != null)
                {
                    regLim.Pmin = regEq.Pmin;
                    regLim.Pmax = regEq.Pmax;
                }

                listaFinal.Add(regLim);
            }

            listaFinal = listaFinal.OrderBy(x => x.Emprnomb).ThenBy(x => x.Equinomb).ToList();

            //debug
            Logger.Info("Emprnomb|Ptomedicodi|Ptomedidesc|Famcodi|Equicodi|Equinomb|Pmin|Pmax");
            foreach (var reg in listaFinal)
            {
                Logger.Info(string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", reg.Emprnomb, reg.Ptomedicodi, reg.Ptomedidesc, reg.Famcodi, reg.Equicodi, reg.Equinomb, reg.Pmin, reg.Pmax));
            }

            return listaFinal;
        }

        private List<EqEquipoDTO> ListarPminPeEquipos(DateTime fechaConsulta)
        {
            //Lista de equipos
            List<EqEquipoDTO> listaEqBD = FactorySic.GetEqEquipoRepository().ListarEquiposPorFamilia(ConstantesHorasOperacion.CodFamilias + "," + ConstantesHorasOperacion.CodFamiliasGeneradores);
            listaEqBD = listaEqBD.Where(x => x.Equiestado == ConstantesAppServicio.Activo || x.Equiestado == ConstantesAppServicio.Proyecto || x.Equiestado == ConstantesAppServicio.FueraCOES).ToList();

            #region Potencia Efectiva y minima de las centrales hidraulicas, eolicas, solares

            List<EqPropequiDTO> listaProp = FactorySic.GetEqPropequiRepository().ListarValoresVigentesPropiedades(fechaConsulta, "-1", "-1", "-1", "46,941,164,299,1602,1710", string.Empty, "-1");
            foreach (var reg in listaProp)
            {
                decimal valor = 0;
                if (decimal.TryParse((reg.Valor != null ? reg.Valor.Trim() : string.Empty), out valor))
                    reg.ValorDecimal = valor;
            }

            //asignar potencia efectiva y minima a los equipos menos a los térmicos
            foreach (var reg in listaEqBD.Where(x => x.Famcodi != 3 && x.Famcodi != 5))
            {
                reg.Pmax = null;
                reg.Pmin = null;

                int propMax = 0;
                int propMin = 0;
                switch (reg.Famcodi.Value)
                {
                    case ConstantesHorasOperacion.IdTipoHidraulica:
                        propMax = 46;
                        propMin = 941;
                        break;
                    case ConstantesHorasOperacion.IdGeneradorHidroelectrico:
                        propMax = 164;
                        propMin = 299;
                        break;
                    case ConstantesHorasOperacion.IdTipoEolica:
                        propMax = 1602;
                        break;
                    case ConstantesHorasOperacion.IdTipoSolar:
                        propMax = 1710;
                        break;
                }

                var listaPropXEq = listaProp.Where(x => x.Equicodi == reg.Equicodi).ToList();

                var regPropMin = listaPropXEq.Find(x => x.Propcodi == propMin);
                if (regPropMin != null) reg.Pmin = regPropMin.ValorDecimal;

                var regPropMax = listaPropXEq.Find(x => x.Propcodi == propMax);
                if (regPropMax != null) reg.Pmax = regPropMax.ValorDecimal;
            }

            //las generadores eolicas y solares deben tener el mismo limite que la central
            foreach (var reg in listaEqBD.Where(x => x.Famcodi == 38 || x.Famcodi == 36))
            {
                var regCentral = listaEqBD.Find(x => x.Equicodi == reg.Equipadre);
                if (regCentral != null)
                {
                    reg.Pmax = regCentral.Pmax;
                }
            }

            #endregion

            #region Potencia Efectiva y minima de las centrales termicas

            // Lista de unidades (incluye con combustible secundario), grupos y modos
            var listaUnidad = (new INDAppServicio()).ListarUnidadTermicaPotencia(fechaConsulta, fechaConsulta);

            //asignar potencia efectiva y minima de los equipos térmicos
            foreach (var reg in listaEqBD.Where(x => x.Famcodi == 3 || x.Famcodi == 5))
            {
                reg.Pmax = null;
                reg.Pmin = null;

                var listaEqTmp = listaUnidad.Where(x => x.Equicodi == reg.Equicodi).OrderBy(x => x.Pe).ToList(); //la potencia minima es igual para cualquier combustible
                var regTmp = listaEqTmp.FirstOrDefault();
                if (regTmp != null)
                {
                    reg.Pmin = regTmp.Pmin;
                    reg.Pmax = regTmp.Pe;
                }
            }

            #endregion

            return listaEqBD;
        }

        #endregion

        #region Verificación de datos hidrológicos

        /// <summary>
        /// Listar limites de los caudales de los puntos de hidrologia
        /// </summary>
        /// <param name="formatcodi">Código del formato (ME_FORMATO) para obtener los puntos de medición</param>
        /// <param name="lectcodi">Código de la lectura (ME_LECTURA) de los datos históricos</param>
        /// <param name="fechaConsulta">Fecha de consulta, por defecto hoy</param>
        /// <returns></returns>
        public List<RDOLimiteHidrologiaCaudal> ListarLimiteHidrologiaCaudal(int formatcodi, int lectcodi, DateTime fechaConsulta)
        {
            //
            DateTime fechaIniData = fechaConsulta.Date.AddDays(-3);
            DateTime fechaFinData = fechaConsulta.Date.AddDays(-1);
            decimal factorK = 1.694m; //Chebyshev

            //configuracion de puntos
            List<MeHojaptomedDTO> listaPto = FactorySic.GetMeHojaptomedRepository().ListarHojaPtoByFormatoAndEmpresa(-1, formatcodi.ToString()).Where(x => x.Hojaptoactivo == 1 && x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiM3s).ToList();

            //data de caudales horario
            List<MeMedicion24DTO> listaM24 = FactorySic.GetMeMedicion24Repository().GetByCriteria(fechaIniData, fechaFinData, lectcodi, ConstantesAppServicio.TipoinfocodiM3s, ConstantesAppServicio.ParametroDefecto);
            listaM24 = CompletarDataFaltanteM24(listaM24, fechaIniData, fechaFinData);

            //generar resultado
            List<RDOLimiteHidrologiaCaudal> listaFinal = new List<RDOLimiteHidrologiaCaudal>();

            foreach (var regPto in listaPto)
            {
                RDOLimiteHidrologiaCaudal regLim = new RDOLimiteHidrologiaCaudal()
                {
                    Ptomedicodi = regPto.Ptomedicodi,
                    Ptomedidesc = (regPto.Ptomedidesc ?? "").Trim(),
                    Tipoinfocodi = ConstantesAppServicio.TipoinfocodiM3s,
                    Tipoptomedinomb = regPto.Tipoptomedinomb,

                    Emprcodi = regPto.Emprcodi,
                    Emprnomb = (regPto.Emprnomb ?? "").Trim(),

                    Famcodi = regPto.Famcodi,
                    Equicodi = regPto.Equicodi,
                    Equinomb = (regPto.Equinomb ?? "").Trim()
                };

                var lista24XPto = listaM24.Where(x => x.Ptomedicodi == regPto.Ptomedicodi && x.Tipoinfocodi == regPto.Tipoinfocodi).OrderBy(x => x.Medifecha).ToList();
                for (int i = 1; i <= 24; i++)
                {
                    var listaXH = new List<decimal>();

                    foreach (var m24 in lista24XPto)
                    {
                        decimal? valor = (decimal?)m24.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(m24, null);
                        if (valor.GetValueOrDefault(0) > 0)
                            listaXH.Add(valor.Value);
                    }

                    if (listaXH.Count == 3)
                    {
                        var promedio = listaXH.Average();
                        var desvEstandar = (decimal)Math.Sqrt(listaXH.Sum(x => Math.Pow((double)(x - promedio), 2)) / (listaXH.Count() - 1));
                        //var desvEstandar = (decimal)Math.Sqrt(listaXH.Sum(x => Math.Pow((double)(x - promedio), 2)) / 2);
                        var menor = promedio - factorK * desvEstandar;
                        var mayor = promedio + factorK * desvEstandar;

                        regLim.ArrayDesvEstandar[i - 1] = desvEstandar;
                        regLim.ArrayPromedio[i - 1] = promedio;
                        regLim.ArrayMinimo[i - 1] = menor < 0 ? 0 : menor;
                        regLim.ArrayMaximo[i - 1] = mayor < 0 ? 0 : mayor;

                    }
                }

                listaFinal.Add(regLim);
            }

            //debug
            foreach (var reg in listaFinal)
            {
                Logger.Info("Emprnomb|Equicodi|Equinomb|Ptomedicodi|Ptomedidesc|Tipo");
                Logger.Info(string.Format("{0}|{1}|{2}|{3}|{4}|{5}", reg.Emprnomb, reg.Equicodi, reg.Equinomb, reg.Ptomedicodi, reg.Ptomedidesc, reg.Tipoptomedinomb));
                Logger.Info("ArrayPromedio|ArrayDesvEstandar|ArrayMinimo|ArrayMaximo");
                for (int i = 1; i <= 24; i++)
                {
                    Logger.Info(string.Format("{0}|{1}|{2}|{3}", Math.Round(reg.ArrayPromedio[i - 1], 3), Math.Round(reg.ArrayDesvEstandar[i - 1], 3), Math.Round(reg.ArrayMinimo[i - 1], 3), Math.Round(reg.ArrayMaximo[i - 1], 3)));
                }
                Logger.Info("");
            }

            return listaFinal;
        }

        /// <summary>
        /// si en una hora no existe data, esta se completa con el ultimo valor
        /// </summary>
        /// <param name="listaM24"></param>
        /// <param name="fechaIniData"></param>
        /// <param name="fechaFinData"></param>
        /// <returns></returns>
        private List<MeMedicion24DTO> CompletarDataFaltanteM24(List<MeMedicion24DTO> listaM24, DateTime fechaIniData, DateTime fechaFinData)
        {
            var listaPto = listaM24.GroupBy(x => new { x.Ptomedicodi, x.Tipoinfocodi }).
                Select(x => new MeHojaptomedDTO() { Ptomedicodi = x.Key.Ptomedicodi, Tipoinfocodi = x.Key.Tipoinfocodi }).ToList();

            foreach (var regPto in listaPto)
            {
                var lista24XPto = listaM24.Where(x => x.Ptomedicodi == regPto.Ptomedicodi && x.Tipoinfocodi == regPto.Tipoinfocodi).OrderBy(x => x.Medifecha).ToList();

                decimal valorUltimo = 0;
                for (DateTime f = fechaIniData; f <= fechaFinData; f = f.AddDays(1))
                {
                    var m24 = lista24XPto.Find(x => x.Medifecha == f);
                    if (m24 == null)
                    {
                        m24 = new MeMedicion24DTO()
                        {
                            Medifecha = f,
                            Ptomedicodi = regPto.Ptomedicodi,
                            Tipoinfocodi = regPto.Tipoinfocodi
                        };

                        listaM24.Add(m24);
                    }

                    for (int i = 1; i <= 24; i++)
                    {
                        decimal? valor = (decimal?)m24.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(m24, null);
                        if (valor.GetValueOrDefault(0) > 0)
                        {
                            valorUltimo = valor.Value;
                        }
                        else
                        {

                            m24.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(m24, valorUltimo);
                        }
                    }
                }
            }

            return listaM24;
        }

        #endregion

        #region Insumos para nodo

        /// <summary>
        /// Devuelve listado48 de codiciones termicas para cierta fecha y hora
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <param name="hora"></param>
        /// <param name="topcodi"></param>
        /// <returns></returns>
        public List<CpMedicion48DTO> ListarInsumoCondicionesTermicas(DateTime fechaConsulta, int hora, int topcodi)
        {
            //obtener datos yupana
            List<CpMedicion48DTO> listaUnidadesForzadas = yupanaServicio.ListaRestriciones(topcodi, ConstantesBase.SRES_UNFOR_PT);

            //Flag para para probar ejecución sin cambios de insumo
            if (ConfigurationManager.AppSettings[ConstantesYupanaContinuo.ActualizarInsumoYupanaContinuo] != "S")
            {
                return listaUnidadesForzadas;
            }

            //se asumen que anteriormete se valido que existan los insumos para la hora de simulacion
            CpForzadoCabDTO objEnvio = GetByDateCpForzadoCab(fechaConsulta.AddHours(hora));
            List<CpForzadoDetDTO> lstForzadoDet = GetByCpfzcodiCpForzadoDets(objEnvio.Cpfzcodi).OrderBy(x => x.Emprcodi).ThenBy(x => x.Equinomb).ThenBy(x => x.Gruponomb).ToList();

            //Convertir las condiciones térmicas a ME_MEDICION48
            List<CpRecursoDTO> listaRecurso = yupanaServicio.ListarRecurso(ConstantesBase.ModoT, objEnvio.Topcodi);

            List<MeMedicion48DTO> lstCondTermicas = new List<MeMedicion48DTO>();
            foreach (var grupocodi in lstForzadoDet.GroupBy(x => x.Grupocodi).ToList())
            {
                var listaXGrupo = lstForzadoDet.Where(x => x.Grupocodi == grupocodi.Key).ToList();
                if (listaXGrupo != null)
                {
                    MeMedicion48DTO med48 = ObtenerMed48(fechaConsulta, listaXGrupo);
                    var find = listaRecurso.Find(x => x.Recurcodisicoes == med48.Grupocodi);
                    if (find != null)
                    {
                        med48.Recurcodi = find.Recurcodi;
                        lstCondTermicas.Add(med48);
                    }
                }
            }

            //Pasar los datos a CP_MEDICION48
            List<CpMedicion48DTO> lstresultado = new List<CpMedicion48DTO>();
            foreach (var reg in lstCondTermicas)
            {
                CpMedicion48DTO registro = new CpMedicion48DTO();
                registro.Medifecha = reg.Medifecha;
                registro.Recurcodi = reg.Recurcodi;
                registro.Srestcodi = ConstantesBase.SRES_UNFOR_PT;
                for (int i = 1; i <= 48; i++)
                {
                    decimal? valor = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(reg, null);
                    registro.GetType().GetProperty(ConstantesAppServicio.CaracterH + i.ToString()).SetValue(registro, valor);
                }

                lstresultado.Add(registro);
            }

            //agregar unidades forzadas de otros días
            lstresultado.AddRange(listaUnidadesForzadas.Where(x => x.Medifecha != fechaConsulta).ToList());

            return lstresultado;
        }

        /// <summary>
        /// Devuelve un objeto  Medicion48 a partir de un grupo de objetos forzadoDet que pertenecen al mismo grupocodi
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <param name="lstForzadaDet"></param>
        /// <returns></returns>
        public MeMedicion48DTO ObtenerMed48(DateTime fechaConsulta, List<CpForzadoDetDTO> lstForzadaDet)
        {
            MeMedicion48DTO objMed48 = new MeMedicion48DTO(); //lista salida

            var objGrupo = lstForzadaDet.First();

            objMed48.Emprcodi = objGrupo.Emprcodi;
            objMed48.Medifecha = fechaConsulta.Date;
            objMed48.Grupocodi = objGrupo.Grupocodi;

            int[] lstHx = new int[48];
            foreach (var objForzada in lstForzadaDet)
            {
                int hxIni = objForzada.Cpfzdtperiodoini.Value;
                int hxFin = objForzada.Cpfzdtperiodofin.Value;

                //armo un listado48 que muestre las posiciones con datos (pintados)
                for (int hx = 1; hx <= 48; hx++)
                {
                    int valor = 0;

                    if (lstHx[hx - 1] != 1) //si la posicion ya tiene 1, no vuelve a verificar ya que si o si será 1
                    {
                        if (hxIni <= hx && hx <= hxFin)
                            valor = 1;

                        lstHx[hx - 1] = valor;
                    }

                }
            }

            //seteo los valores Hx de la salida, usando el listado48
            for (int hx_ = 1; hx_ <= 48; hx_++)
            {
                var valor = Convert.ToDecimal(lstHx[hx_ - 1]);
                objMed48.GetType().GetProperty(ConstantesAppServicio.CaracterH + hx_).SetValue(objMed48, valor);
            }


            return objMed48;
        }

        /// <summary>
        /// Método que obtiene la última información de RER a utilizar por el administrador (mezcla entre los datos Extranet y modificaciones Intranet) para un día
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <param name="topcodi"></param>
        /// <returns></returns>
        public List<CpMedicion48DTO> ListarInsumoGeneracionRER(DateTime fechaConsulta, int hora, int topcodi)
        {
            //obtener datos yupana
            List<CpMedicion48DTO> listaRER = yupanaServicio.ListaRestriciones(topcodi, ConstantesBase.SRES_GENER_RER);

            //Flag para para probar ejecución sin cambios de insumo
            if (ConfigurationManager.AppSettings[ConstantesYupanaContinuo.ActualizarInsumoYupanaContinuo] != "S")
            {
                return listaRER;
            }

            //Obtener datos procesados en Intranet
            List<CpYupconM48DTO> listaDet48 = ListarDetalleCargaConfiguracion(ConstantesYupanaContinuo.TipoConfiguracionRer, 0, fechaConsulta.AddHours(hora));

            //Pasar los datos a CP_MEDICION48
            List<CpMedicion48DTO> lstresultado = new List<CpMedicion48DTO>();
            foreach (var reg in listaDet48)
            {
                CpMedicion48DTO registro = new CpMedicion48DTO();
                registro.Catcodi = ConstantesBase.PlantaNoConvenO;
                registro.Medifecha = fechaConsulta.Date;
                registro.Recurcodi = reg.Recurcodi;
                registro.Srestcodi = ConstantesBase.SRES_GENER_RER;
                registro.Meditotal = 0;

                for (int i = 1; i <= 48; i++)
                {
                    decimal valor = ((decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(reg, null)).GetValueOrDefault(0);

                    registro.GetType().GetProperty(ConstantesAppServicio.CaracterH + i.ToString()).SetValue(registro, valor);
                    registro.Meditotal += valor;
                }

                lstresultado.Add(registro);
            }

            //Actualizar data Yupana
            foreach (var registro in listaRER.Where(x => x.Medifecha == fechaConsulta))
            {
                //Informacion de la extranet
                CpYupconM48DTO genExtranet = listaDet48.Find(x => x.Recurcodi == registro.Recurcodi);
                if (genExtranet != null)
                {
                    registro.Meditotal = 0;

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valor = ((decimal?)genExtranet.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(genExtranet, null)).GetValueOrDefault(0);

                        registro.GetType().GetProperty(ConstantesAppServicio.CaracterH + i.ToString()).SetValue(registro, valor);
                        registro.Meditotal += valor;
                    }
                }
            }

            return listaRER;
        }

        /// <summary>
        ///  Devuelve la ultima data de compromiso hidraulico para cierta fecha 
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <param name="topcodi"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        private List<CpMedicion48DTO> ListarInsumoCompromisoHidraulico(DateTime fechaConsulta, int hora, int topcodi, int tipo, List<MePtomedicionDTO> listaPtoFormato)
        {
            //data escenario yupana
            List<CpMedicion48DTO> listaAportes = yupanaServicio.ListaRestriciones(topcodi, ConstantesBase.SRES_APORTES_PH);
            List<CpMedicion48DTO> listaAportesYupanaEmb = yupanaServicio.ListaRestriciones(topcodi, ConstantesBase.SRES_APORTES_EMB);
            listaAportes.AddRange(listaAportesYupanaEmb);

            //Flag para para probar ejecución sin cambios de insumo
            if (ConfigurationManager.AppSettings[ConstantesYupanaContinuo.ActualizarInsumoYupanaContinuo] != "S")
            {
                return listaAportes;
            }

            List<MeMedicion24DTO> lstCompromiso = new List<MeMedicion24DTO>();
            List<MeMedicion24DTO> lst, lst2;

            switch (tipo)
            {
                case 0: //sin compromiso
                    lst = FactorySic.GetMeMedicion24Repository().GetByCriteria(fechaConsulta.Date, fechaConsulta.Date, ConstantesYupanaContinuo.LecturaSinCompromiso, -1, "-1").OrderByDescending(x => x.Medifecha).ToList();
                    if (lst.Count > 0) lstCompromiso = lst.Where(x => x.Medifecha == fechaConsulta.Date).ToList();

                    break;
                case 1: //con compromiso
                    lst = FactorySic.GetMeMedicion24Repository().GetByCriteria(fechaConsulta.Date, fechaConsulta.Date, ConstantesYupanaContinuo.LecturaConCompromiso, -1, "-1").OrderByDescending(x => x.Medifecha).ToList();
                    if (lst.Count > 0) lstCompromiso = lst.Where(x => x.Medifecha == fechaConsulta.Date).ToList();
                    break;
                case 2: //todos
                    lst = FactorySic.GetMeMedicion24Repository().GetByCriteria(fechaConsulta.Date, fechaConsulta.Date, ConstantesYupanaContinuo.LecturaSinCompromiso, -1, "-1").OrderByDescending(x => x.Medifecha).ToList();
                    if (lst.Count > 0) lstCompromiso = lst.Where(x => x.Medifecha == fechaConsulta.Date).ToList();

                    lst2 = FactorySic.GetMeMedicion24Repository().GetByCriteria(fechaConsulta.Date, fechaConsulta.Date, ConstantesYupanaContinuo.LecturaConCompromiso, -1, "-1").OrderByDescending(x => x.Medifecha).ToList();
                    if (lst2.Count > 0) lstCompromiso.AddRange(lst2.Where(x => x.Medifecha == fechaConsulta.Date).ToList());
                    break;
            }

            if (!listaPtoFormato.Any())
            {
                foreach (var reg in lstCompromiso)
                {
                    MePtomedicionDTO pto = new MePtomedicionDTO();
                    pto.Ptomedicodi = reg.Ptomedicodi;

                    listaPtoFormato.Add(pto);
                }
            }

            //extranet rdo
            List<CpMedicion48DTO> listaCaudalesXRecurcodi = ObtenerAportesHidrologia(ConstantesBase.PlantaH, ConstantesBase.SRES_APORTES_PH, fechaConsulta, hora);
            List<CpMedicion48DTO> listaCaudalesEmb = ObtenerAportesHidrologia(ConstantesBase.Embalse, ConstantesBase.SRES_APORTES_EMB, fechaConsulta, hora);
            listaCaudalesXRecurcodi.AddRange(listaCaudalesEmb);

            //
            List<CpRecursoDTO> listaRecurso = yupanaServicio.ListarRecurso(ConstantesBase.PlantaH, topcodi);
            List<CpRecursoDTO> listaEmbalse = yupanaServicio.ListarRecurso(ConstantesBase.Embalse, topcodi);
            listaRecurso.AddRange(listaEmbalse);
            List<CpRecursoDTO> listaRecursoBase = yupanaServicio.ListarRecurso(ConstantesBase.PlantaH, 0);
            List<CpRecursoDTO> listaEmbalseBase = yupanaServicio.ListarRecurso(ConstantesBase.Embalse, 0);
            listaRecursoBase.AddRange(listaEmbalseBase);

            List<int> lPtomedicodis = new List<int>();
            lPtomedicodis.AddRange(lstCompromiso.Select(x => x.Ptomedicodi));
            lPtomedicodis.AddRange(listaPtoFormato.Select(x => x.Ptomedicodi));
            List<MePtomedicionDTO> listaAllPto = new List<MePtomedicionDTO>();
            if (lPtomedicodis.Any()) listaAllPto = FactorySic.GetMePtomedicionRepository().ListarPtoMedicion(string.Join(",", lPtomedicodis));

            //setear equipo a los puntos del formato
            foreach (var reg in listaPtoFormato)
            {
                var pto = listaAllPto.Find(x => x.Ptomedicodi == reg.Ptomedicodi);
                int equicodi = pto != null ? pto.Equicodi.GetValueOrDefault(0) : -1;
                string equinomb = pto != null ? pto.Equinomb : string.Empty;

                reg.Equicodi = equicodi;
                reg.Equinomb = equinomb;

                CpRecursoDTO findRec = listaRecurso.Find(x => x.Recurcodisicoes == reg.Equicodi);
                if (findRec == null) findRec = listaRecursoBase.Find(x => x.Recurcodisicoes == reg.Equicodi);

                //codigo de central o embalse
                if (findRec != null)
                {
                    reg.Recurcodi = findRec.Recurcodi;
                    reg.Recurnombre = findRec.Recurnombre;
                    reg.Catecodi = findRec.Catcodi;
                }
            }
            listaPtoFormato = listaPtoFormato.OrderBy(x => x.Recurnombre).ToList();

            //setear al m24 el equicodi de central o embalse
            foreach (var reg in lstCompromiso)
            {
                var pto = listaPtoFormato.Find(x => x.Ptomedicodi == reg.Ptomedicodi);
                if (pto != null)
                {
                    reg.Recurcodi = pto.Recurcodi;
                    reg.Equicodi = pto.Equicodi ?? 0;
                    reg.Equinomb = pto.Equinomb;
                    reg.Catcodi = pto.Catecodi;
                }
            }

            //verificar que centrales tienen compromiso / sin compromiso pero no aporte
            List<int> lrecurcodiTopologia = listaAportes.Select(x => x.Recurcodi).ToList();
            var lCompSintopologia = lstCompromiso.Where(x => !lrecurcodiTopologia.Contains(x.Recurcodi)).ToList();

            foreach (var reg in lCompSintopologia)
            {
                CpMedicion48DTO aporte = new CpMedicion48DTO();
                aporte.Recurcodi = reg.Recurcodi;
                aporte.Topcodi = topcodi;
                aporte.Medifecha = fechaConsulta;
                aporte.Catcodi = reg.Catcodi;
                if (reg.Catcodi == ConstantesBase.PlantaH)
                    aporte.Srestcodi = ConstantesBase.SRES_APORTES_PH;
                if (reg.Catcodi == ConstantesBase.Embalse)
                    aporte.Srestcodi = ConstantesBase.SRES_APORTES_EMB;

                listaAportes.Add(aporte);
            }

            //solo actualizar los embalses
            foreach (var aporte in listaAportes.Where(x => x.Medifecha == fechaConsulta))
            {
                if (aporte.Recurcodi == 879)
                { }

                //Informacion de la extranet
                CpMedicion48DTO caudalExtranet = listaCaudalesXRecurcodi.Find(x => x.Recurcodi == aporte.Recurcodi);

                //obtener flag de (con/sin) compromiso hidraulico
                MeMedicion24DTO flagCompromiso = lstCompromiso.Find(x => x.Recurcodi == aporte.Recurcodi);

                if (flagCompromiso != null && caudalExtranet != null)
                {
                    aporte.TieneCheckCompH = true;
                    decimal meditotal = 0;
                    for (int i = 1; i <= 24; i++)
                    {
                        var h = ((decimal?)flagCompromiso.GetType().GetProperty("H" + i.ToString()).GetValue(flagCompromiso, null)).GetValueOrDefault(0);
                        int valor = (int)h;

                        if (valor == 1) //tiene check
                        {
                            decimal? valorHidro = (decimal?)caudalExtranet.GetType().GetProperty("H" + (i * 2).ToString()).GetValue(caudalExtranet, null);

                            aporte.GetType().GetProperty("H" + (i * 2 - 1).ToString()).SetValue(aporte, valorHidro);
                            aporte.GetType().GetProperty("H" + (i * 2).ToString()).SetValue(aporte, valorHidro);

                        }

                        decimal? valor1 = (decimal?)aporte.GetType().GetProperty("H" + (i * 2 - 1).ToString()).GetValue(aporte, null);
                        decimal? valor2 = (decimal?)aporte.GetType().GetProperty("H" + (i * 2).ToString()).GetValue(aporte, null);
                        meditotal += valor1.GetValueOrDefault(0) + valor2.GetValueOrDefault(0);

                        aporte.Meditotal = meditotal;
                    }
                }
            }

            //sublistas para verificar quienes tienen cambio
            var lista1 = listaAportes.Where(x => !x.TieneCheckCompH).ToList();
            var lista2 = listaAportes.Where(x => x.TieneCheckCompH && x.Meditotal != 0).ToList();

            listaAportes = new List<CpMedicion48DTO>();
            listaAportes.AddRange(lista1);
            listaAportes.AddRange(lista2);

            return listaAportes;
        }

        /// <summary>
        /// Obtiene los aportes de caudales de hidrologia al escenario.
        /// </summary>
        /// <param name="idEscenario"></param>
        /// <param name="idCategoria"></param>
        /// <param name="sRestriccion"></param>
        /// <param name="fechaConsulta"></param>
        /// <param name="nDias"></param>
        /// <param name="formatcodi"></param>
        /// <param name="listaRptomed"></param>
        /// <returns></returns>
        public List<CpMedicion48DTO> ObtenerAportesHidrologia(int idCategoria, short sRestriccion, DateTime fechaConsulta, int hora)
        {
            //Obtener datos procesados en Intranet
            List<CpYupconM48DTO> listaDet48 = ListarDetalleCargaConfiguracion(ConstantesYupanaContinuo.TipoConfiguracionCaudal, 0, fechaConsulta.AddHours(hora))
                                                .Where(x => x.Catcodi == idCategoria).ToList();

            //Pasar los datos a CP_MEDICION48
            List<CpMedicion48DTO> lstresultado = new List<CpMedicion48DTO>();
            foreach (var reg in listaDet48)
            {
                CpMedicion48DTO registro = new CpMedicion48DTO();
                registro.Catcodi = idCategoria;
                registro.Medifecha = fechaConsulta.Date;
                registro.Recurcodi = reg.Recurcodi;
                registro.Srestcodi = sRestriccion;
                registro.Meditotal = 0;

                for (int i = 1; i <= 48; i++)
                {
                    decimal valor = ((decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(reg, null)).GetValueOrDefault(0);

                    registro.GetType().GetProperty(ConstantesAppServicio.CaracterH + i.ToString()).SetValue(registro, valor);
                    registro.Meditotal += valor;
                }

                lstresultado.Add(registro);
            }

            return lstresultado;
        }

        #endregion

        #region ÁRBOL DE SIMULACION

        #region Presentación web

        /// <summary>
        /// Devuelve una lista de los contenidos de cada nodo a mostrar enla gráfica
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="identificador"></param>
        /// <param name="tag"></param>
        /// <param name="lstArboles"></param>
        /// <param name="porcentajeArbolSimulado"></param>
        /// <param name="codigoArbolAMostrar"></param>
        /// <param name="msjProceso"></param>
        /// <returns></returns>
        public List<EstructuraNodo> ObtenerContenidoNodos(int cparbcodi, out CpArbolContinuoDTO arbolConsultado)
        {
            arbolConsultado = GetByIdCpArbolContinuo(cparbcodi);

            List<CpNodoContinuoDTO> lstNodosPorArbol = ListCpNodoContinuosPorArbol(arbolConsultado.Cparbcodi).OrderBy(x => x.Cpnodocodi).ToList();
            List<CpNodoDetalleDTO> lstNodosDetallePorArbol = ListCpNodoDetallesPorArbol(arbolConsultado.Cparbcodi);

            //Crear grafico
            List<EstructuraNodo> lstNodosAMostrar = new List<EstructuraNodo>();

            var COLOR_ACTUALIZADO = "#350301"; //rojo oscuro
            var COLOR_SIN_ACTUALIZAR = "#DE0A02"; //rojo 

            var COLOR_EN_SIMULACION = "#F1E5E5";
            var COLOR_EJEC_CONVERGE = "#20A705";
            var COLOR_EJEC_DIVERGE = "#430AF4";
            var COLOR_EJEC_CON_ERROR = "#F00B0B";
            var COLOR_NO_SIMULADO = "#7B7878";
            var COLOR_NO_SIMULADO_TIEMPO = "#DFDC00";

            var colBError = "#000000"; //negro
            var colBCreado = COLOR_EN_SIMULACION; //blanco rosa (en simulacion...)
            var colBEjecConv = COLOR_EJEC_CONVERGE; //verde
            var colBEjecDive = COLOR_EJEC_DIVERGE; //morado
            var colBNoEjec = COLOR_NO_SIMULADO; //plomo oscuro
            var colBNoEjecTiempo = COLOR_NO_SIMULADO_TIEMPO; //amarillo
            var colBBlanco = "#FFFFFF"; //blanco
            var colBEjecError = COLOR_EJEC_CON_ERROR; //rojo
            var colorBordeDefault = COLOR_EN_SIMULACION;

            //Armas Contenido de los nodos
            foreach (var nodo in lstNodosPorArbol)
            {
                int codigoNodo = nodo.Cpnodocodi;
                string estadoNodo = nodo.Cpnodoestado.Trim(); //C: creado, E: ejecutado, EE: ejecutado con error, N: no ejecutado por divergencia, TP: no ejecutado por tiempo prolongado
                string convergenciaNodo = nodo.Cpnodoconverge; //C: converge. D: diverge.
                string msgError = nodo.Cpnodomsjproceso;
                int idTopGuardado = nodo.Cpnodoidtopguardado ?? 0;
                string nomTopGuardado = nodo.Topnombre;

                List<CpNodoDetalleDTO> detallesNodo = lstNodosDetallePorArbol.Where(x => x.Cpnodocodi == codigoNodo).ToList();

                EstructuraNodo nodoMostrar = new EstructuraNodo();
                nodoMostrar.Estado = estadoNodo;
                nodoMostrar.Convergencia = convergenciaNodo;
                nodoMostrar.Height = 255;

                nodoMostrar.Identificador = nodo.Cpnodonumero;
                nodoMostrar.MensajeError = msgError;
                nodoMostrar.HtmlResultado = GetHtmlResultadoNodo(detallesNodo, idTopGuardado, nomTopGuardado);
                nodoMostrar.HtmlGuardarEscenario = GetHtmlGuardarEscenarioNodo(idTopGuardado, nomTopGuardado);

                nodoMostrar.Info = GetInfoNodo(nodo);
                nodoMostrar.NumeroNodo = nodo.Cpnodonumero.ToString("D2");

                //border color
                var strbColor = colorBordeDefault;
                switch (nodoMostrar.Estado)
                {
                    case "C":
                        strbColor = colBNoEjec;
                        if (arbolConsultado.Cparbporcentaje != -1) strbColor = colBCreado;
                        break;
                    case "E":
                        if (nodoMostrar.Convergencia == "C") strbColor = colBEjecConv;
                        else
                        {
                            if (nodoMostrar.Convergencia == "D") strbColor = colBEjecDive;
                            else
                                strbColor = colBBlanco;
                        }
                        break;
                    case "EE":
                        strbColor = colBEjecError;
                        break;
                    case "N":
                        strbColor = colBNoEjec;
                        break;
                    case "TP":
                        strbColor = colBNoEjecTiempo;
                        break;
                    default:
                        strbColor = colBBlanco;
                        break;
                }
                nodoMostrar.BorderColor = strbColor;

                //html nodo
                var str = "";
                switch (nodoMostrar.Estado)
                {
                    case "N":
                    case "TP":
                        str = "";
                        break;

                    case "EE":
                        str = "<div style='text-align: center;'>Error al simular</div>";
                        break;
                    case "C":
                        var strCreado = "";
                        if (nodoMostrar.Identificador == 1) strCreado = "Obteniendo data";
                        else strCreado = "En espera";

                        if (nodoMostrar.Convergencia == "T")
                            strCreado = "En ejecución";

                        if (arbolConsultado.Cparbporcentaje != -1)
                            str = "<div style='text-align: center;'>" + strCreado + " ...</div>";

                        break;
                    default:
                        str = nodoMostrar.HtmlResultado;
                        break;
                }
                nodoMostrar.HtmlResultado = str;

                lstNodosAMostrar.Add(nodoMostrar);

            }

            //16 nodos
            lstNodosAMostrar = lstNodosAMostrar.OrderBy(x => x.Identificador).ToList();

            //generar los 31 nodos
            List<EstructuraNodo> lstNodosGrafo = new List<EstructuraNodo>();
            foreach (var reg in lstNodosAMostrar)
            {
                EstructuraNodo regClone;

                switch (reg.Identificador)
                {
                    case 1:
                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "A";
                        regClone.Offset = "";
                        regClone.MensajeActualizacion = arbolConsultado.Cparbidentificador;
                        lstNodosGrafo.Add(regClone);

                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "C";
                        regClone.Offset = "350%";
                        regClone.MensajeActualizacion = "SIN ACTUALIZAR";
                        lstNodosGrafo.Add(regClone);

                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "C2";
                        regClone.Offset = "450%";
                        regClone.MensajeActualizacion = "SIN ACTUALIZAR";
                        lstNodosGrafo.Add(regClone);

                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "C22";
                        regClone.Offset = "350%";
                        regClone.MensajeActualizacion = "SIN ACTUALIZAR";
                        lstNodosGrafo.Add(regClone);

                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "C222";
                        regClone.Offset = "";
                        regClone.MensajeActualizacion = "SIN ACTUALIZAR";
                        lstNodosGrafo.Add(regClone);
                        break;
                    case 2:
                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "C221";
                        regClone.Offset = "";
                        regClone.MensajeActualizacion = "ACTUALIZADA";
                        lstNodosGrafo.Add(regClone);
                        break;
                    case 3:
                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "C21";
                        regClone.Offset = "250%";
                        regClone.MensajeActualizacion = "ACTUALIZADA";
                        lstNodosGrafo.Add(regClone);

                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "C212";
                        regClone.Offset = "";
                        regClone.MensajeActualizacion = "SIN ACTUALIZAR";
                        lstNodosGrafo.Add(regClone);
                        break;
                    case 4:
                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "C211";
                        regClone.Offset = "";
                        regClone.MensajeActualizacion = "ACTUALIZADA";
                        lstNodosGrafo.Add(regClone);
                        break;
                    case 5:
                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "C1";
                        regClone.Offset = "150%";
                        regClone.MensajeActualizacion = "ACTUALIZADA";
                        lstNodosGrafo.Add(regClone);

                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "C12";
                        regClone.Offset = "150%";
                        regClone.MensajeActualizacion = "SIN ACTUALIZAR";
                        lstNodosGrafo.Add(regClone);

                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "C122";
                        regClone.Offset = "";
                        regClone.MensajeActualizacion = "SIN ACTUALIZAR";
                        lstNodosGrafo.Add(regClone);
                        break;
                    case 6:
                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "C121";
                        regClone.Offset = "";
                        regClone.MensajeActualizacion = "ACTUALIZADA";
                        lstNodosGrafo.Add(regClone);
                        break;
                    case 7:
                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "C11";
                        regClone.Offset = "50%";
                        regClone.MensajeActualizacion = "ACTUALIZADA";
                        lstNodosGrafo.Add(regClone);

                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "C112";
                        regClone.Offset = "";
                        regClone.MensajeActualizacion = "SIN ACTUALIZAR";
                        lstNodosGrafo.Add(regClone);
                        break;
                    case 8:
                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "C111";
                        regClone.Offset = "";
                        regClone.MensajeActualizacion = "ACTUALIZADA";
                        lstNodosGrafo.Add(regClone);
                        break;
                    case 9:
                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "B";
                        regClone.Offset = "-350%";
                        regClone.MensajeActualizacion = "ACTUALIZADA";
                        lstNodosGrafo.Add(regClone);

                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "B2";
                        regClone.Offset = "-150%";
                        regClone.MensajeActualizacion = "SIN ACTUALIZAR";
                        lstNodosGrafo.Add(regClone);

                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "B22";
                        regClone.Offset = "-50%";
                        regClone.MensajeActualizacion = "SIN ACTUALIZAR";
                        lstNodosGrafo.Add(regClone);

                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "B222";
                        regClone.Offset = "";
                        regClone.MensajeActualizacion = "SIN ACTUALIZAR";
                        lstNodosGrafo.Add(regClone);
                        break;
                    case 10:
                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "B221";
                        regClone.Offset = "";
                        regClone.MensajeActualizacion = "ACTUALIZADA";
                        lstNodosGrafo.Add(regClone);
                        break;
                    case 11:
                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "B21";
                        regClone.Offset = "-150%";
                        regClone.MensajeActualizacion = "ACTUALIZADA";
                        lstNodosGrafo.Add(regClone);

                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "B212";
                        regClone.Offset = "";
                        regClone.MensajeActualizacion = "SIN ACTUALIZAR";
                        lstNodosGrafo.Add(regClone);
                        break;
                    case 12:
                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "B211";
                        regClone.Offset = "";
                        regClone.MensajeActualizacion = "ACTUALIZADA";
                        lstNodosGrafo.Add(regClone);
                        break;
                    case 13:
                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "B1";
                        regClone.Offset = "-450%";
                        regClone.MensajeActualizacion = "ACTUALIZADA";
                        lstNodosGrafo.Add(regClone);

                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "B12";
                        regClone.Offset = "-250%";
                        regClone.MensajeActualizacion = "SIN ACTUALIZAR";
                        lstNodosGrafo.Add(regClone);

                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "B122";
                        regClone.Offset = "";
                        regClone.MensajeActualizacion = "SIN ACTUALIZAR";
                        lstNodosGrafo.Add(regClone);
                        break;
                    case 14:
                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "B121";
                        regClone.Offset = "";
                        regClone.MensajeActualizacion = "ACTUALIZADA";
                        lstNodosGrafo.Add(regClone);
                        break;
                    case 15:
                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "B11";
                        regClone.Offset = "-350%";
                        regClone.MensajeActualizacion = "ACTUALIZADA";
                        lstNodosGrafo.Add(regClone);

                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "B112";
                        regClone.Offset = "";
                        regClone.MensajeActualizacion = "SIN ACTUALIZAR";
                        lstNodosGrafo.Add(regClone);
                        break;
                    case 16:
                        regClone = (EstructuraNodo)reg.Clone();
                        regClone.Id = "B111";
                        regClone.Offset = "";
                        regClone.MensajeActualizacion = "ACTUALIZADA";
                        lstNodosGrafo.Add(regClone);
                        break;
                }
            }

            foreach (var reg in lstNodosGrafo)
            {
                reg.ColorActualizacion = reg.MensajeActualizacion == "ACTUALIZADA" ? COLOR_ACTUALIZADO : COLOR_SIN_ACTUALIZAR;
            }

            return lstNodosGrafo;
        }

        private string GetInfoNodo(CpNodoContinuoDTO nodo)
        {
            string html = "";

            if (nodo.Cpnodonumero != 1)
            {
                html += "Condición Térmica: ";
                html += (nodo.Cpnodoflagcondterm == "S" ? "Actualizada" : "Sin Actualizar") + "<br/>";

                html += "Act. H. (Sin compromiso): ";
                html += (nodo.Cpnodoflagsincompr == "S" ? "Actualizada" : "Sin Actualizar") + "<br/>";

                html += "Act. H. (Con compromiso): ";
                html += (nodo.Cpnodoflagconcompr == "S" ? "Actualizada" : "Sin Actualizar") + "<br/>";

                html += "Proyecciones RER: ";
                html += (nodo.Cpnodoflagrer == "S" ? "Actualizada" : "Sin Actualizar") + "<br/>";
            }
            else
            {
                html += "Nodo Raiz";
            }
            if (!string.IsNullOrEmpty(nodo.Cpnodomsjproceso))
            {
                html += "<br/>";
                html += nodo.Cpnodomsjproceso;
            }

            html += "<br/><br/>";
            html += "Ini proceso: " + (nodo.Cpnodofeciniproceso != null ? nodo.Cpnodofeciniproceso.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "-") + "<br/>";
            html += "Fin proceso: " + (nodo.Cpnodofecfinproceso != null ? nodo.Cpnodofecfinproceso.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "-") + "<br/>";

            return html;
        }

        /// <summary>
        /// Devuelve el contenido de cada nodo mostrado
        /// </summary>
        /// <param name="detallesNodo"></param>
        /// <param name="idTopGuardado"></param>
        /// <param name="nomTopGuardado"></param>
        /// <returns></returns>
        private string GetHtmlResultadoNodo(List<CpNodoDetalleDTO> detallesNodo, int idTopGuardado, string nomTopGuardado)
        {
            #region Datos de Costos Marginales
            string co = detallesNodo.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCO)?.Cpndetvalor;
            string cr = detallesNodo.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCR)?.Cpndetvalor;
            string cm_minpermin = detallesNodo.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCMgMinPerMin)?.Cpndetvalor;
            string cm_minpermed = detallesNodo.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCMgMinPerMed)?.Cpndetvalor;
            string cm_minpermax = detallesNodo.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCMgMinPerMax)?.Cpndetvalor;
            string cm_maxpermin = detallesNodo.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCMgMaxPerMin)?.Cpndetvalor;
            string cm_maxpermed = detallesNodo.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCMgMaxPerMed)?.Cpndetvalor;
            string cm_maxpermax = detallesNodo.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCMgMaxPerMax)?.Cpndetvalor;
            string cm_prompermin = detallesNodo.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCMgPromPerMin)?.Cpndetvalor;
            string cm_prompermed = detallesNodo.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCMgPromPerMed)?.Cpndetvalor;
            string cm_prompermax = detallesNodo.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCMgPromPerMax)?.Cpndetvalor;

            string val_co = co != null && co != "" ? ((Convert.ToDecimal(co)).ToString("N2", CultureInfo.InvariantCulture)) : "";
            string val_cr = cr != null && cr != "" ? ((Convert.ToDecimal(cr)).ToString("N2", CultureInfo.InvariantCulture)) : "";
            string val_cm_minpermin = cm_minpermin != null && cm_minpermin != "" ? ((Convert.ToDecimal(cm_minpermin)).ToString("N2", CultureInfo.InvariantCulture)) : "";
            string val_cm_minpermed = cm_minpermed != null && cm_minpermed != "" ? ((Convert.ToDecimal(cm_minpermed)).ToString("N2", CultureInfo.InvariantCulture)) : "";
            string val_cm_minpermax = cm_minpermax != null && cm_minpermax != "" ? ((Convert.ToDecimal(cm_minpermax)).ToString("N2", CultureInfo.InvariantCulture)) : "";
            string val_cm_maxpermin = cm_maxpermin != null && cm_maxpermin != "" ? ((Convert.ToDecimal(cm_maxpermin)).ToString("N2", CultureInfo.InvariantCulture)) : "";
            string val_cm_maxpermed = cm_maxpermed != null && cm_maxpermed != "" ? ((Convert.ToDecimal(cm_maxpermed)).ToString("N2", CultureInfo.InvariantCulture)) : "";
            string val_cm_maxpermax = cm_maxpermax != null && cm_maxpermax != "" ? ((Convert.ToDecimal(cm_maxpermax)).ToString("N2", CultureInfo.InvariantCulture)) : "";
            string val_cm_prompermin = cm_prompermin != null && cm_prompermin != "" ? ((Convert.ToDecimal(cm_prompermin)).ToString("N2", CultureInfo.InvariantCulture)) : "";
            string val_cm_prompermed = cm_prompermed != null && cm_prompermed != "" ? ((Convert.ToDecimal(cm_prompermed)).ToString("N2", CultureInfo.InvariantCulture)) : "";
            string val_cm_prompermax = cm_prompermax != null && cm_prompermax != "" ? ((Convert.ToDecimal(cm_prompermax)).ToString("N2", CultureInfo.InvariantCulture)) : "";

            #endregion

            StringBuilder str = new StringBuilder();

            if (idTopGuardado != 0 && nomTopGuardado != "")
                str.Append("<table class='default'>");
            else
                str.Append("<table class='default' style='margin-top: 20px;'>");
            str.Append("<tbody>");
            str.AppendFormat("<tr> <td></td> <td colspan='5' style='font-size: 11px; font-weight: bold; text-align: left;'>Costo Operación: {0} K$</td> <td></td> </tr>", val_co);
            str.AppendFormat("<tr> <td></td> <td colspan='5' style='font-size: 11px; font-weight: bold; text-align: left;'>Costo Racionamiento: {0} K$</td> <td></td> </tr>", val_cr);
            str.Append("<tr> <td></td> <td colspan='5' style='font-size: 11px; font-weight: bold; text-align: left;'>Costos Marginales:</td> <td></td> </tr>");
            str.Append("<tr> <td></td> <td></td> <td style='font-size: 11px; font-weight: bold; text-align: center;'>Periodo</td> <td style='font-size: 11px; font-weight: bold; text-align: center;'>Min</td> <td style='font-size: 11px; font-weight: bold; text-align: center;'>Max</td> <td style='font-size: 11px; font-weight: bold; text-align: center;'>Prom</td> <td></td> </tr>");
            str.AppendFormat("<tr> <td></td> <td></td> <td style='font-size: 11px; font-weight: bold; text-align: center;'>Min</td> <td style='font-size: 10px; text-align: center;'>{0}</td> <td style='font-size: 10px; text-align: center;'>{1}</td> <td style='font-size: 10px; text-align: center;'>{2}</td> <td></td> </tr>", val_cm_minpermin, val_cm_maxpermin, val_cm_prompermin);
            str.AppendFormat("<tr> <td></td> <td></td> <td style='font-size: 11px; font-weight: bold; text-align: center;'>Med</td> <td style='font-size: 10px; text-align: center;'>{0}</td> <td style='font-size: 10px; text-align: center;'>{1}</td> <td style='font-size: 10px; text-align: center;'>{2}</td> <td></td> </tr>", val_cm_minpermed, val_cm_maxpermed, val_cm_prompermed);
            str.AppendFormat("<tr> <td></td> <td></td> <td style='font-size: 11px; font-weight: bold; text-align: center;'>Max</td> <td style='font-size: 10px; text-align: center;'>{0}</td> <td style='font-size: 10px; text-align: center;'>{1}</td> <td style='font-size: 10px; text-align: center;'>{2}</td> <td></td> </tr>", val_cm_minpermax, val_cm_maxpermax, val_cm_prompermax);

            str.Append("</tbody>");
            str.Append("</table>");

            return str.ToString();
        }

        private string GetHtmlGuardarEscenarioNodo(int idTopGuardado, string nomTopGuardado)
        {
            StringBuilder str = new StringBuilder();

            //Por ultimo agregamos el html del escenario guardado
            if (idTopGuardado != 0 && nomTopGuardado != "")
            {
                str.Append("<table class='default' style='color: darkblue;'>");
                str.Append("<tbody>");
                //str.AppendFormat("<tr> <td></td> <td colspan='2' style='font-size: 11px; font-weight: bold; text-align: left;'>TopCodi: {0} K$</td> <td></td><td colspan='2' style='font-size: 11px; font-weight: bold; text-align: left;'>TopNombre: {1} K$</td> </tr>", idTopGuardado, nomTopGuardado);
                str.AppendFormat("<tr> <td colspan='5' style='font-size: 10px; font-weight: bold;'> Escenario Guardado</td> </tr> <tr> <td colspan='2'></td> <td colspan='3' style='font-size: 10px; text-align: left;'><b>TopCodi:</b> {0}</td> </tr> <tr> <td colspan='2'></td> <td colspan='3' style='font-size: 10px; text-align: left;'><b>TopNombre:</b> {1}</td> </tr>", idTopGuardado, nomTopGuardado);

                str.Append("</tbody>");
                str.Append("</table>");
            }
            else
            {
                str.Append("<table> </table>");
            }
            return str.ToString();
        }

        /// <summary>
        /// Actualiza el campo porcentaje del arbol (cada vez q se actualiza la pantalla)
        /// </summary>
        /// <param name="codigoArbol"></param>
        /// <param name="porcentajeArbolSimulado"></param>
        private void ActualizarPorcentajeArbol(int codigoArbol, out decimal porcentajeArbolSimulado)
        {
            porcentajeArbolSimulado = 0;

            try
            {
                CpArbolContinuoDTO arbolConsultado = GetByIdCpArbolContinuo(codigoArbol);
                List<CpNodoContinuoDTO> lstNodosPorArbol = ListCpNodoContinuosPorArbol(codigoArbol).OrderBy(x => x.Cpnodocodi).ToList();

                porcentajeArbolSimulado = arbolConsultado.Cparbporcentaje;

                if (arbolConsultado.Cparbporcentaje != -1)
                {
                    decimal porcentaje = 0m;

                    //calculo el %
                    foreach (var nodo in lstNodosPorArbol)
                    {
                        var estadoNodo = nodo.Cpnodoestado.Trim();
                        if (estadoNodo == "E" || estadoNodo == "EE" || estadoNodo == "N" || estadoNodo == "TP") //ESTADOS: C: creado, E: ejecutado, EE: ejecutado con error, N: no ejecutado por divergencia, TP: no ejecutado por tiempo prolongado
                        {
                            porcentaje = porcentaje + 6.25m;
                        }
                    }

                    //actualizo datos
                    if (porcentaje == 100)
                    {
                        arbolConsultado.Cparbfecfinproceso = DateTime.Now;
                        arbolConsultado.Cparbestado = "ST";  //Si Terminado
                    }
                    else
                    {
                        arbolConsultado.Cparbestado = "NT";  //No Terminado
                    }

                    arbolConsultado.Cparbporcentaje = porcentaje;
                    porcentajeArbolSimulado = porcentaje;

                    UpdateCpArbolContinuo(arbolConsultado);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
        }

        /// <summary>
        /// Devuelve "S" si el tag es el último de todos, es decir si el arbol a mostrar es el ultimo que se creó
        /// </summary>
        /// <param name="fechaArbolMostrado"></param>
        /// <param name="tagArbolMostrado"></param>
        /// <param name="lstArboles"></param>
        /// <returns></returns>
        public string VerificarUltimoTag(int cparbcodi)
        {
            var obj = GetUltimoArbol();
            int cparbcodiUltimo = obj != null && obj.Cparbfecha.Date == DateTime.Today ? obj.Cparbcodi : 0;

            return cparbcodi == cparbcodiUltimo ? "S" : "N";
        }

        #endregion

        #region 00. Verificación del estado del árbol y sus nodos

        /// <summary>
        /// Guarda en el arbol, la ocurrencia de algun error en la simulacionel
        /// </summary>
        /// <param name="codigoArbol"></param>
        /// <param name="msjException"></param>
        private void IndicarOcurrenciaErrorEnArbol(int codigoArbol, string msjException)
        {
            CpArbolContinuoDTO arbolCreado = this.GetByIdCpArbolContinuo(codigoArbol);

            if (arbolCreado.Cparbporcentaje != -1)
            {
                arbolCreado.Cparbmsjproceso = "Ocurrió un error cuando se realizaba el proceso. La generación de la versión empezó a las " + arbolCreado.Cparbfecregistro.ToString(ConstantesAppServicio.FormatoFechaFull2)
                    + " y se terminó a las " + DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaFull2) + " cuando estaba al " + arbolCreado.Cparbporcentaje + "%.\nEl motivo fue el siguiente: " + msjException;
                arbolCreado.Cparbporcentaje = -1;
                arbolCreado.Cparbmsjproceso = arbolCreado.Cparbmsjproceso.Trim();
                if (arbolCreado.Cparbmsjproceso.Length > 500)
                    arbolCreado.Cparbmsjproceso = arbolCreado.Cparbmsjproceso.Substring(0, 500);

                this.UpdateCpArbolContinuo(arbolCreado);
            }
        }

        public CpArbolContinuoDTO GetUltimoArbolEnEjecucion()
        {
            CpArbolContinuoDTO ultimoArbol = this.GetUltimoArbol();

            if (ultimoArbol != null && (ultimoArbol.Cparbporcentaje != 100 && ultimoArbol.Cparbporcentaje != -1))
                return ultimoArbol;

            return null;
        }

        /// <summary>
        /// Al iniciar el servidor de la Intranet buscará si una simulación quedó pendiente de terminar
        /// </summary>
        public void VerificarUltimaSimulacionPendiente()
        {
            try
            {
                CpArbolContinuoDTO ultimoArbol = this.GetUltimoArbolEnEjecucion();

                if (ultimoArbol != null)
                {
                    int arbcodi = ultimoArbol.Cparbcodi;
                    ultimoArbol.Cparbcodi = arbcodi;

                    ultimoArbol.Cparbmsjproceso = "Ocurrió un error cuando se realizaba el proceso. La generación de la versión empezó a las " + ultimoArbol.Cparbfecregistro.ToString(ConstantesAppServicio.FormatoFechaFull2)
                        + ", se terminó cuando estaba al " + ultimoArbol.Cparbporcentaje + "% al detenerse el servidor.\nEl servidor inició nuevamente a las " + DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaFull2);
                    ultimoArbol.Cparbporcentaje = -1;
                    ultimoArbol.Cparbmsjproceso = ultimoArbol.Cparbmsjproceso.Trim();
                    if (ultimoArbol.Cparbmsjproceso.Length > 500)
                        ultimoArbol.Cparbmsjproceso = ultimoArbol.Cparbmsjproceso.Substring(0, 500);

                    this.UpdateCpArbolContinuo(ultimoArbol);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
        }

        /// <summary>
        /// Luego de ejecución del nodo 1
        /// </summary>
        /// <param name="minutoEspera"></param>
        private void VerificarPlazosEjecucionCiclico(int minutoEspera)
        {
            //Despues de 5 minutos verifica si hay procesos prolongados y que deban cancelar simulación
            EsperarTiempo(ConstantesYupanaContinuo.TipoMinuto, minutoEspera);

            while (true)
            {
                int codigoArbol = GetCodigoUltimoArbol();

                if (codigoArbol > 0)
                {
                    CpArbolContinuoDTO arbol = GetByIdCpArbolContinuo(codigoArbol);

                    if (arbol.Cparbporcentaje == 100 || arbol.Cparbporcentaje == -1)
                    {
                        TerminarTodoProcesoGams();
                        break;
                    }
                    else
                    {
                        VerificarPlazosEjecucion();
                    }
                }
                else
                {
                    break;
                }

                //Verifica cada minuto
                EsperarTiempo(ConstantesYupanaContinuo.TipoMinuto, 1);
            }
        }

        /// <summary>
        /// Descarta simulacion de un nodo si el tiempo es prolongado
        /// </summary>
        public void VerificarPlazosEjecucion()
        {
            int codigoArbol = GetCodigoUltimoArbol();

            if (codigoArbol > 0)
            {
                CpArbolContinuoDTO arbol = GetByIdCpArbolContinuo(codigoArbol);

                if (arbol.Cparbporcentaje == 100 || arbol.Cparbporcentaje == -1)
                {
                    //TerminarTodoProcesoGams();
                }
                else
                {
                    List<CpNodoContinuoDTO> lstNodosDelArbol = ListCpNodoContinuosPorArbol(codigoArbol)
                                                                .OrderBy(x => x.Nivel).ThenBy(x => x.OrdenXNivel).ToList();

                    var nodoRaiz = lstNodosDelArbol.Find(x => x.Cpnodonumero == 1);

                    //si terminó de procesar el nodo 1 entonces verificar el estado de los demás nodos
                    if (nodoRaiz != null && nodoRaiz.Cpnodofecfinproceso != null)
                    {
                        foreach (var regTmp in lstNodosDelArbol)
                        {
                            //obtener el ultimo estado del nodo
                            CpNodoContinuoDTO nodo = GetByNumeroCpNodoContinuo(codigoArbol, regTmp.Cpnodonumero);

                            //verificar nodos ejecutandose
                            if (nodo.Cpnodofeciniproceso != null && nodo.Cpnodofecfinproceso == null)
                            {
                                DateTime fechaActual = DateTime.Now;
                                DateTime fechaInicioSimulacion = nodo.Cpnodofeciniproceso.Value;
                                TimeSpan lapso = fechaActual - fechaInicioSimulacion;
                                int minTranscurridos = (int)lapso.TotalMinutes;

                                if (minTranscurridos > GetMinutoMaxGams())
                                {
                                    SetearNoSimularNodoEHijos(codigoArbol, nodo.Cpnodonumero);
                                    Task.Factory.StartNew(() => TerminarProcesoGams(15, nodo.Cpnodomsjproceso));
                                }
                            }
                            else
                            {
                                //verificar nodos creados
                                if (nodo.Cpnodofeciniproceso == null)
                                {

                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Finalizar todos los procesos gams que existan en el servidor
        /// </summary>
        private void TerminarTodoProcesoGams()
        {
            //terminar todos los procesos gams que queden activos
            Task.Factory.StartNew(() => TerminarProcesoGams(0, "-1"));
        }

        /// <summary>
        /// Finalizar ejecucion del ultimo arbol
        /// </summary>
        public void FinalizarEjecucionArbol()
        {
            TerminarTodoProcesoGams();
            CpArbolContinuoDTO ultimoArbolEnEjec = GetUltimoArbolEnEjecucion();

            if (ultimoArbolEnEjec != null)
            {
                IndicarOcurrenciaErrorEnArbol(ultimoArbolEnEjec.Cparbcodi, "Finalizó la ejecución del árbol a solicitud de usuario.");
            }
        }

        /// <summary>
        /// obtener el ultimo arbol del dia
        /// </summary>
        /// <returns></returns>
        public int GetCodigoUltimoArbol()
        {
            var regArbol = GetUltimoArbol();

            return regArbol != null ? regArbol.Cparbcodi : 0;
        }

        /// <summary>
        /// Obtiene la cantidad total de nodos ejecutandose 
        /// </summary>
        /// <param name="codigoArbol"></param>
        /// <returns></returns>
        private int ObtenerNumeroNodosEjecutandose(int codigoArbol)
        {
            List<CpNodoContinuoDTO> lstNodosDelArbol = ListCpNodoContinuosPorArbol(codigoArbol);
            List<CpNodoContinuoDTO> lstNodosProcesandose = lstNodosDelArbol.Where(x => x.Cpnodofeciniproceso != null && x.Cpnodofecfinproceso == null).ToList();
            int num = lstNodosProcesandose.Count;

            return num;
        }

        private void TerminarProcesoGams(int segundos, string slistaId)
        {
            EsperarTiempo(ConstantesYupanaContinuo.TipoSegundo, segundos);

            try
            {
                if (slistaId == "-1")
                {
                    Process[] ps = Process.GetProcessesByName("gamscmex");

                    foreach (Process p in ps)
                    {
                        try
                        {
                            p.Kill();
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("TerminarProcesoGams" + p.Id, ex);
                        }
                    }
                }
                else
                {
                    var listaId = ListarIdProcesoFromString(slistaId);
                    foreach (int id in listaId)
                    {
                        Process p = Process.GetProcessById(id);
                        try
                        {
                            p.Kill();
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("TerminarProcesoGams" + p.Id, ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("TerminarProcesoGams", ex);
            }
        }

        private void GuardarProcessID(int codigoArbol, int numNodo)
        {
            try
            {
                //se espera 5 segundos para saber que processId se han generado luego de llamar al GAMS.EXE
                EsperarTiempo(ConstantesYupanaContinuo.TipoSegundo, 5);

                List<CpNodoContinuoDTO> lstNodosDelArbol = ListCpNodoContinuosPorArbol(codigoArbol);
                List<CpNodoContinuoDTO> lstNodosProcesandose = lstNodosDelArbol.Where(x => x.Cpnodofeciniproceso != null && x.Cpnodofecfinproceso == null).ToList();

                List<int> lIdsBD = new List<int>();
                foreach (var reg in lstNodosProcesandose)
                {
                    //obtener los ids de los procesos que se estan ejecutando
                    lIdsBD.AddRange(ListarIdProcesoFromString(reg.Cpnodomsjproceso));
                }

                Process[] ps = Process.GetProcessesByName("gamscmex");
                List<int> idsActual = ps.Select(x => x.Id).ToList();

                List<int> lIdsNodo = new List<int>();
                foreach (var id in idsActual)
                {
                    if (!lIdsBD.Contains(id))
                        lIdsNodo.Add(id);
                }

                CpNodoContinuoDTO cpNodo = GetByNumeroCpNodoContinuo(codigoArbol, numNodo);
                //solo actualizar si no tiene fecha fin de proceso
                if (cpNodo.Cpnodofecfinproceso == null)
                {
                    string ids = string.Join("|", lIdsNodo);
                    cpNodo.Cpnodomsjproceso = ids;
                    UpdateCpNodoContinuo(cpNodo);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("GuardarProcesoID", ex);
            }
        }

        private List<int> ListarIdProcesoFromString(string msj)
        {
            List<int> lIdsBD = new List<int>();

            if (!string.IsNullOrEmpty(msj))
            {
                try
                {
                    //obtener los ids de los procesos que se estan ejecutando
                    List<string> lids = msj.Split('|').ToList();
                    foreach (var s in lids)
                    {
                        if (Int32.TryParse(s, out int id))
                            lIdsBD.Add(id);
                    }
                }
                catch (Exception)
                { };
            }

            return lIdsBD;
        }

        #endregion

        #region 01. Creación de árbol y nodo Raíz

        /// <summary>
        /// Crea y guarda la estructura (datos generales) del arbol, sus nodos y sus detalles
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="hora"></param>
        /// <param name="tipoSimulacion"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int CrearEstructuraArbol(DateTime fecha, int hora, int tipoSimulacion, string usuario)
        {
            //previamente se verifica que no existe arbol en ejecucion

            //Crear estructura arbol, nodos y detalles
            int codigoArbol = this.CrearArbolYNodoYDetalle(fecha.Date, hora, tipoSimulacion, usuario);

            if (codigoArbol == -1) throw new ArgumentException("Problema al crear y guardar el árbol");

            return codigoArbol;
        }

        /// <summary>
        /// Crea la estructura inicial en la BD (arbol, nodos y detalles)
        /// </summary>
        /// <param name="fechaSimulacion"></param>
        /// <param name="horaSimulacion"></param>
        /// <param name="tipoSimulacion"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private int CrearArbolYNodoYDetalle(DateTime fechaSimulacion, int horaSimulacion, int tipoSimulacion, string usuario)
        {
            int codigoArbol = -1;

            CpTopologiaDTO topologiaASimular = ObtenerTopologiaByDate(fechaSimulacion.Date.AddHours(horaSimulacion));
            if (topologiaASimular == null) throw new ArgumentException("No se encontró Escenario yupana (PDO/RDO) para la fecha de simulación");

            if (topologiaASimular != null)
            {
                string strFecha = fechaSimulacion.ToString(ConstantesAppServicio.FormatoFechaWS);
                string strHoraActual = DateTime.Now.ToString("HH-mm-ss");

                CpArbolContinuoDTO objArbol = new CpArbolContinuoDTO()
                {
                    Cparbtag = tipoSimulacion == ConstantesYupanaContinuo.SimulacionAutomatica ? "A_" + strHoraActual : "M_" + usuario + "_" + strHoraActual,
                    Cparbfecregistro = DateTime.Now,
                    Topcodi = topologiaASimular.Topcodi,
                    Cparbusuregistro = usuario,
                    Cparbestado = "NI",  //NO iniciado (creado)
                    Cparbfecha = fechaSimulacion,
                    Cparbbloquehorario = horaSimulacion,
                    Cparbdetalleejec = "",
                    Cparbidentificador = topologiaASimular.Identificador,
                    Cparbfeciniproceso = DateTime.Now,
                    Cparbfecfinproceso = null,
                    Cparbmsjproceso = "",
                    Cparbporcentaje = 0

                };

                //Iniciamos los nodos (16 en total)
                objArbol.ListaNodos = GenerarNodosArbol(objArbol.Cparbtag);

                //Funcion transaccional para guardar en BD
                codigoArbol = this.GuardarArbolYNodos_BDTransaccional(objArbol);
            }

            return codigoArbol;
        }

        /// <summary>
        /// Guarda el arbol, los nodos y sus detalles en la BD
        /// </summary>
        /// <param name="regArbol"></param>
        /// <returns></returns>
        private int GuardarArbolYNodos_BDTransaccional(CpArbolContinuoDTO regArbol)
        {
            int cparbcodi = -1;
            int cpnodocodi = -1;
            int cpndetcodi = -1;

            var UoW = FactorySic.UnitOfWork();
            using (var connection = UoW.BeginConnection())
            {
                using (var transaction = (DbTransaction)UoW.StartTransaction(connection))
                {
                    try
                    {
                        cparbcodi = FactorySic.GetCpArbolContinuoRepository().Save(regArbol, connection, transaction);

                        foreach (CpNodoContinuoDTO nodo in regArbol.ListaNodos)
                        {
                            nodo.Cparbcodi = cparbcodi;

                            cpnodocodi = FactorySic.GetCpNodoContinuoRepository().Save(nodo, connection, transaction);

                            foreach (CpNodoDetalleDTO nodoDetalle in nodo.ListaNodosDetalle)
                            {
                                nodoDetalle.Cpnodocodi = cpnodocodi;

                                cpndetcodi = FactorySic.GetCpNodoDetalleRepository().Save(nodoDetalle, connection, transaction);

                            }
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

            return cparbcodi;
        }

        /// <summary>
        /// Genera un listado de nodos para un arbol, con datos generales
        /// </summary>
        /// <param name="cparbtag"></param>
        /// <returns></returns>
        private List<CpNodoContinuoDTO> GenerarNodosArbol(string cparbtag)
        {
            List<CpNodoContinuoDTO> lstNodosArbol = new List<CpNodoContinuoDTO>();

            int numNodos = 16;

            for (int i = 0; i < numNodos; i++)
            {
                string strBinary = Convert.ToString(i, 2).PadLeft(4, '0'); //Devuelve un str : "0000"

                CpNodoContinuoDTO objNodo = new CpNodoContinuoDTO();

                objNodo.Cpnodoestado = "C";
                objNodo.Cpnodoflagcondterm = strBinary[0].ToString() == "1" ? "S" : "N";
                objNodo.Cpnodoflagsincompr = strBinary[1].ToString() == "1" ? "S" : "N";
                objNodo.Cpnodoflagconcompr = strBinary[2].ToString() == "1" ? "S" : "N";
                objNodo.Cpnodoflagrer = strBinary[3].ToString() == "1" ? "S" : "N";

                int numNodo = SetIdentificadorNodo(objNodo.Cpnodoflagcondterm, objNodo.Cpnodoflagsincompr, objNodo.Cpnodoflagconcompr, objNodo.Cpnodoflagrer);

                if (numNodo == 1) objNodo.Cpnodofeciniproceso = DateTime.Now; //inicializar la fecha de inicio del nodo 1, que coincida con la creacion del arbol y los insumos antes de procesar el nodo 9
                objNodo.Cpnodonumero = numNodo;
                objNodo.Cpnodocarpeta = cparbtag + "_" + (Convert.ToDecimal(numNodo)).ToString("00", CultureInfo.InvariantCulture);

                objNodo.ListaNodosDetalle = IniciarNodosDetalle();

                lstNodosArbol.Add(objNodo);
            }

            return lstNodosArbol;
        }

        /// <summary>
        /// Devuelve el identificador del nodo segun flags: "CT SC CC PR"
        /// 1   N N	N N
        /// 2	N N N S
        /// 3	N N S N
        /// 4	N N S S
        /// 5	N S N N
        /// 6	N S N S
        /// 7	N S S N
        /// 8	N S S S
        /// 9	S N N N
        /// 10	S N N S
        /// 11	S N S N
        /// 12	S N S S
        /// 13	S S N N
        /// 14	S S N S
        /// 15	S S S N
        /// 16	S S S S      
        /// </summary>
        /// <param name="flagCT"></param>
        /// <param name="flagSC"></param>
        /// <param name="flagCC"></param>
        /// <param name="flagPR"></param>
        /// <returns></returns>
        private int SetIdentificadorNodo(string flagCT, string flagSC, string flagCC, string flagPR)
        {

            if (flagCT == "N")
            {
                if (flagSC == "N")
                {
                    if (flagCC == "N" && flagPR == "N") return 1;
                    if (flagCC == "N" && flagPR == "S") return 2;
                    if (flagCC == "S" && flagPR == "N") return 3;
                    if (flagCC == "S" && flagPR == "S") return 4;
                }
                else
                {
                    if (flagSC == "S")
                    {
                        if (flagCC == "N" && flagPR == "N") return 5;
                        if (flagCC == "N" && flagPR == "S") return 6;
                        if (flagCC == "S" && flagPR == "N") return 7;
                        if (flagCC == "S" && flagPR == "S") return 8;
                    }
                }
            }
            else
            {
                if (flagCT == "S")
                {
                    if (flagSC == "N")
                    {
                        if (flagCC == "N" && flagPR == "N") return 9;
                        if (flagCC == "N" && flagPR == "S") return 10;
                        if (flagCC == "S" && flagPR == "N") return 11;
                        if (flagCC == "S" && flagPR == "S") return 12;
                    }
                    else
                    {
                        if (flagSC == "S")
                        {
                            if (flagCC == "N" && flagPR == "N") return 13;
                            if (flagCC == "N" && flagPR == "S") return 14;
                            if (flagCC == "S" && flagPR == "N") return 15;
                            if (flagCC == "S" && flagPR == "S") return 16;
                        }
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Genera una lista de Detalle (11 en total, 1 por cada concepto) para cada nodo
        /// </summary>
        /// <returns></returns>
        private List<CpNodoDetalleDTO> IniciarNodosDetalle()
        {
            List<CpNodoDetalleDTO> lstNodosDetalle = new List<CpNodoDetalleDTO>();

            int numNodosDetalle = 11;

            for (int i = 1; i <= numNodosDetalle; i++)
            {
                int conceptoElegido = ObtenerConceptoNodo(i);

                CpNodoDetalleDTO objNodoDet = new CpNodoDetalleDTO();

                objNodoDet.Cpnconcodi = conceptoElegido;

                lstNodosDetalle.Add(objNodoDet);
            }

            return lstNodosDetalle;
        }

        /// <summary>
        /// Obtiene insumos y genera archivos de entrada al Gams
        /// </summary>
        /// <param name="codigoArbol"></param>
        /// <param name="fechaSimulacion"></param>
        /// <param name="horaSimulacion"></param>
        /// <returns></returns>
        public string GenerarInsumosYDirectorio(int codigoArbol, DateTime fechaSimulacion, int horaSimulacion)
        {
            string path = ConfigurationManager.AppSettings[ConstantesYupanaContinuo.PathYupanaContinuo];

            var arbol = GetByIdCpArbolContinuo(codigoArbol);
            InsumoYupanaContinuo objInsumo = GenerarInsumosParaSimulacionDeNodos(fechaSimulacion, horaSimulacion, arbol.Topcodi);

            int numProcesadores = GetNumeroProcesadores();
            string directorioTrabajo = yupanaServicio.CrearArchivosDatCsvEnArbol(path, arbol, fechaSimulacion, objInsumo, numProcesadores);

            return directorioTrabajo;
        }

        /// <summary>
        /// Devuelve el listado de insumos para generar archivos .dat
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <param name="hora"></param>
        /// <param name="listaCondTermicas"></param>
        /// <param name="lstAportesSC"></param>
        /// <param name="lstAportesCC"></param>
        /// <param name="lstAportesCCSC"></param>
        /// <param name="listaProyRer"></param>
        /// <param name="topcodi"></param>
        private InsumoYupanaContinuo GenerarInsumosParaSimulacionDeNodos(DateTime fechaConsulta, int hora, int topcodi)
        {
            InsumoYupanaContinuo obj = new InsumoYupanaContinuo();

            //Unidades_Forzadas.dat, PlantaTermica.txt
            obj.ListaCondTermicas = ListarInsumoCondicionesTermicas(fechaConsulta, hora, topcodi);

            //Generacion_No_Convencional.dat, PlantaRer.txt
            obj.ListaProyRer = ListarInsumoGeneracionRER(fechaConsulta, hora, topcodi);

            //Caudales.dat, Embalse.txt, PlantaHidro.txt
            obj.ListaAportesSC = ListarInsumoCompromisoHidraulico(fechaConsulta, hora, topcodi, 0, new List<MePtomedicionDTO>());
            obj.ListaAportesCC = ListarInsumoCompromisoHidraulico(fechaConsulta, hora, topcodi, 1, new List<MePtomedicionDTO>());
            obj.ListaAportesCCSC = ListarInsumoCompromisoHidraulico(fechaConsulta, hora, topcodi, 2, new List<MePtomedicionDTO>());

            return obj;
        }

        /// <summary>
        /// Devuelve el codigo del concepto segun su código
        /// </summary>
        /// <param name="codigoConcepto"></param>
        /// <returns></returns>
        private int ObtenerConceptoNodo(int codigoConcepto)
        {
            int conceptoSalida = -1;
            switch (codigoConcepto)
            {
                case 1: conceptoSalida = ConstantesYupanaContinuo.ConcepcodiCO; break;
                case 2: conceptoSalida = ConstantesYupanaContinuo.ConcepcodiCR; break;
                case 3: conceptoSalida = ConstantesYupanaContinuo.ConcepcodiCMgMinPerMin; break;
                case 4: conceptoSalida = ConstantesYupanaContinuo.ConcepcodiCMgMinPerMed; break;
                case 5: conceptoSalida = ConstantesYupanaContinuo.ConcepcodiCMgMinPerMax; break;
                case 6: conceptoSalida = ConstantesYupanaContinuo.ConcepcodiCMgMaxPerMin; break;
                case 7: conceptoSalida = ConstantesYupanaContinuo.ConcepcodiCMgMaxPerMed; break;
                case 8: conceptoSalida = ConstantesYupanaContinuo.ConcepcodiCMgMaxPerMax; break;
                case 9: conceptoSalida = ConstantesYupanaContinuo.ConcepcodiCMgPromPerMin; break;
                case 10: conceptoSalida = ConstantesYupanaContinuo.ConcepcodiCMgPromPerMed; break;
                case 11: conceptoSalida = ConstantesYupanaContinuo.ConcepcodiCMgPromPerMax; break;
            }

            return conceptoSalida;
        }

        private void VerificarExistenciaInsumo(DateTime fechaHora)
        {
            List<string> listaMsj = new List<string>();

            //i1. Configuración de caudal
            CpYupconCfgDTO cfgCaudal = GetUltimaConfiguracionXTipo(ConstantesYupanaContinuo.TipoConfiguracionCaudal, fechaHora, false);
            if (cfgCaudal == null) listaMsj.Add("No existen configuraciones de Caudales");

            //i2. Configuración de generación RER
            CpYupconCfgDTO cfgRer = GetUltimaConfiguracionXTipo(ConstantesYupanaContinuo.TipoConfiguracionRer, fechaHora, false);
            if (cfgRer == null) listaMsj.Add("No existen configuraciones de Generación RER");

            //i3. Reporte Caudales
            CpYupconEnvioDTO objEnvioDataCaudal = GetUltimoEnvioCargaConfiguracion(ConstantesYupanaContinuo.TipoConfiguracionCaudal, fechaHora);
            if (objEnvioDataCaudal == null) listaMsj.Add("No existen datos de Reporte de Caudales");

            //i4. Reporte Generación RER
            CpYupconEnvioDTO objEnvioDataRER = GetUltimoEnvioCargaConfiguracion(ConstantesYupanaContinuo.TipoConfiguracionRer, fechaHora);
            if (objEnvioDataRER == null) listaMsj.Add("No existen datos de Reporte de Generación RER");

            //i5. Condiciones termicas - Unidades forzadas
            CpForzadoCabDTO cabCT = GetByDateCpForzadoCab(fechaHora);
            if (cabCT == null) listaMsj.Add("No existen condiciones Térmicas");

            if (listaMsj.Any()) throw new ArgumentException(string.Join(". ", listaMsj));
        }

        #endregion

        /// <summary>
        /// Ejecutar simulaciones manuales en ambiente local
        /// </summary>
        /// <param name="codigoArbol"></param>
        private void EjecutarSimulacion(int codigoArbol)
        {
            try
            {
                //finalizar los procesos gams que estuviesen activos
                TerminarTodoProcesoGams();

                CpArbolContinuoDTO arbol = GetByIdCpArbolContinuo(codigoArbol);

                GenerarInsumosYDirectorio(codigoArbol, arbol.Cparbfecha.Date, arbol.Cparbbloquehorario);

                //El nodo raiz no es necesario simular, solo buscar las salidas en yupana
                ProcesarNodoRaiz(codigoArbol, 1);

                SimularNodosMultiprocesos(codigoArbol);
            }
            catch (Exception ex)
            {
                string msjException = ex.Message;
                IndicarOcurrenciaErrorEnArbol(codigoArbol, msjException);

                Logger.Error("EjecutarProcesoTarea", ex);
            }
        }

        /// <summary>
        /// Simular el grupo de nodos, se les manda de forma paralela
        /// </summary>
        /// <param name="codigoArbol"></param>
        public void SimularNodosMultiprocesos(int codigoArbol)
        {
            CpArbolContinuoDTO arbol = GetByIdCpArbolContinuo(codigoArbol);
            string ruta = yupanaServicio.CrearDirectorioTrabajoYupanaContinuo(ConfigurationManager.AppSettings[ConstantesYupanaContinuo.PathYupanaContinuo], arbol, arbol.Cparbfecha.Date);

            Task.Factory.StartNew(() => ProcesarListaNodo(ruta, codigoArbol, 9));

            Task.Factory.StartNew(() => ProcesarListaNodo(ruta, codigoArbol, 5));

            Task.Factory.StartNew(() => ProcesarListaNodo(ruta, codigoArbol, 3));

            Task.Factory.StartNew(() => ProcesarListaNodo(ruta, codigoArbol, 2));

            //validar si los nodos demoran más de lo esperado
            Task.Factory.StartNew(() => VerificarPlazosEjecucionCiclico(5));
        }

        #region 03. Ejecución de Simulación

        /// <summary>
        /// Espera cierto tiempo antes de continuar
        /// </summary>
        private void EsperarTiempo(int tipo, int tiempo)
        {
            int total = 0;
            if (tipo == ConstantesYupanaContinuo.TipoSegundo)
            {
                var seed = Environment.TickCount;
                var random = new Random(seed);
                var tiempoRandom = random.Next(tiempo, tiempo + 10);

                total = tiempoRandom;
            }
            if (tipo == ConstantesYupanaContinuo.TipoMinuto) total = tiempo * 60;

            Task.Delay(total * 1000).Wait();
        }

        private int GetNumeroProcesadores()
        {
            int resultado = 0;

            string valor = ConfigurationManager.AppSettings[ConstantesYupanaContinuo.NumCPUYupanaContinuo];
            if (string.IsNullOrEmpty(valor))
                Int32.TryParse(valor, out resultado);

            if (resultado <= 0) resultado = ConstantesYupanaContinuo.DefaultNumCPUSimulacion;

            return resultado;
        }

        private int GetNumeroGamsParalelo()
        {
            int resultado = 0;

            string valor = ConfigurationManager.AppSettings[ConstantesYupanaContinuo.NumGamsParaleloYupanaContinuo];
            if (string.IsNullOrEmpty(valor))
                Int32.TryParse(valor, out resultado);

            if (resultado <= 0) resultado = ConstantesYupanaContinuo.DefaultNumGamsParalelo;

            return resultado;
        }

        private int GetMinutoMaxGams()
        {
            int resultado = 0;

            string valor = ConfigurationManager.AppSettings[ConstantesYupanaContinuo.MinutoMaxGamsYupanaContinuo];
            if (string.IsNullOrEmpty(valor))
                Int32.TryParse(valor, out resultado);

            if (resultado <= 0) resultado = ConstantesYupanaContinuo.DefaultMinutoMaxGams;

            return resultado;
        }

        #region Nodo 1

        /// <summary>
        /// Devuelve los datos del nodo raiz
        /// </summary>
        /// <param name="codigoArbol"></param>
        /// <param name="numNodo"></param>
        private void ProcesarNodoRaiz(int codigoArbol, int numNodo)
        {
            CpNodoContinuoDTO nodo = GetByNumeroCpNodoContinuo(codigoArbol, numNodo);
            nodo.Cpnodoestado = "E";    // C: creado, E: ejecutado, EE: ejecutado con error, N: no ejecutado por divergencia, TP: no ejecutado por tiempo prolongado
            nodo.Cpnodoconverge = "C";//  siempre converge 
            nodo.Cpnodomsjproceso = "Ejecutado correctamente";

            yupanaServicio.GetCostoRacionamiento(nodo.Topcodi, out decimal? costoRacionamiento, out decimal? costoOperacion);

            nodo.CostoOperacion = costoOperacion;
            nodo.CostoRacionamiento = costoRacionamiento;
            nodo.ListaCMarginales = yupanaServicio.ListaCostoMarginales(nodo.Topcodi);

            GuardaBDNodoSimulado(nodo);
        }

        #endregion

        /// <summary>
        /// Procesa de forma paralela varios bloques, en este caso es del bloque cuya raiz es el nodo 3
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="codigoArbol"></param>
        /// <param name="numNodo"></param>
        private void ProcesarListaNodo(string ruta, int codigoArbol, int numNodo)
        {
            CpNodoContinuoDTO nodoSimulado = ProcesarNodoHoja(ruta, codigoArbol, numNodo);

            if (nodoSimulado != null)
            {
                if (nodoSimulado.Cpnodoconverge == ConstantesYupana.Converge)
                {
                    foreach (var numNodoHijo in nodoSimulado.ListaHijo)
                    {
                        Task.Factory.StartNew(() => ProcesarListaNodo(ruta, codigoArbol, numNodoHijo));
                    }
                }
                else
                {
                    //if (nodoSimulado.Cpnodoconverge == ConstantesYupana.Diverge)
                    //{
                    foreach (var numNodoHijo in nodoSimulado.ListaHijo)
                    {
                        SetearNoEjecutadoNodosHijos(codigoArbol, numNodoHijo);
                    }

                    //}
                    //else
                    //{
                    //    //tiempo prolongado
                    //    foreach (var numNodoHijo in nodoSimulado.ListaHijo)
                    //    {
                    //        SetearNoSimularNodoEHijos(codigoArbol, numNodoHijo);
                    //    }
                    //}
                }
            }
        }

        /// <summary>
        /// Simula y guarda las salidas para un nodo hoja
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="codigoArbol"></param>
        /// <param name="numNodo"></param>
        private CpNodoContinuoDTO ProcesarNodoHoja(string ruta, int codigoArbol, int numNodo)
        {
            int tsec = ConstantesYupanaContinuo.SegundosEsperaAProcesar + OrdenXNivelNodo(numNodo);
            EsperarTiempo(ConstantesYupanaContinuo.TipoSegundo, tsec);

            CpArbolContinuoDTO arbolEnSimulacion = this.GetByIdCpArbolContinuo(codigoArbol);
            if (arbolEnSimulacion.Cparbporcentaje != -1)
            {
                //Solo permite algunos procesos simultaneos                        
                while (true)
                {
                    int numNodosEjecutandoseALaVez = ObtenerNumeroNodosEjecutandose(codigoArbol);
                    if (numNodosEjecutandoseALaVez < GetNumeroGamsParalelo())
                    {
                        break;
                    }

                    EsperarTiempo(ConstantesYupanaContinuo.TipoSegundo, 3);
                }

                arbolEnSimulacion = this.GetByIdCpArbolContinuo(codigoArbol);
                if (arbolEnSimulacion.Cparbporcentaje != -1)
                {
                    return EjecutarGAMS(ruta, codigoArbol, numNodo);
                }
            }

            return null;
        }

        /// <summary>
        /// Ejecuta la simulacion y obtiene los resultados
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="codigoArbol"></param>
        /// <param name="numNodo"></param>
        /// <returns></returns>
        private CpNodoContinuoDTO EjecutarGAMS(string ruta, int codigoArbol, int numNodo)
        {
            Logger.Info("hoja " + numNodo + " antes de actualizar");

            CpNodoContinuoDTO cpNodo = GetByNumeroCpNodoContinuo(codigoArbol, numNodo);

            if (cpNodo.Cpnodoestado.Trim() == "C")
            {
                string stNodoi = (Convert.ToDecimal(numNodo)).ToString("00", CultureInfo.InvariantCulture);
                string rutaFinal = ruta + "//" + stNodoi + "//";

                cpNodo.Cpnodoconverge = "T"; //convergencia Temporal (en memoria), T: asumiendo tiempo prolongado 
                cpNodo.Cpnodofeciniproceso = DateTime.Now;

                //Actualizo "nodo" con los nuevos datos
                UpdateCpNodoContinuo(cpNodo);

                GAMSJob resultado = null;
                cpNodo.Cpnodoestado = "E";    // C: creado, E: ejecutado, EE: ejecutado con error, N: no ejecutado por divergencia, NT: no ejecutado por tiempo
                try
                {
                    bool ejecutarGams = true;
                    if (ejecutarGams)
                    {
                        string msjExceptionGams = null;
                        try
                        {
                            //ejecución gams
                            Task.Factory.StartNew(() => GuardarProcessID(codigoArbol, numNodo));
                            resultado = yupanaServicio.EjecutarEscenario(rutaFinal, cpNodo.Topcodi);
                        }
                        catch (Exception ex)
                        {
                            //cuando diverge el gams lanza "GAMS return code not 0 (2)" debido a "Status: Compilation error" que es el mensaje de divergencia
                            //esta excepcion no debe hacer terminar el arbol sino solo el nodo actual y sus hijos
                            msjExceptionGams = ex.ToString();
                            Logger.Error("Error simulación en escenario" + numNodo, ex);
                        }

                        //verificar si converge o diverge
                        cpNodo.Cpnodoconverge = yupanaServicio.GetConvergeGams(rutaFinal);
                        cpNodo.Cpnodomsjproceso = ConstantesYupana.Converge == cpNodo.Cpnodoconverge ? "Nodo Converge" : "Nodo Diverge";

                        if (string.IsNullOrEmpty(cpNodo.Cpnodoconverge) && !string.IsNullOrEmpty(msjExceptionGams))
                        {
                            cpNodo.Cpnodoconverge = ConstantesYupana.Diverge;
                            cpNodo.Cpnodomsjproceso = cpNodo.Cpnodomsjproceso + ". " + msjExceptionGams;
                            //throw new ArgumentException(msjExceptionGams);
                        }

                        if (resultado != null)
                        {
                            cpNodo.CostoOperacion = yupanaServicio.GetValorVariableGams(rutaFinal, resultado, ConstantesYupana.GamsCostoOperacion);
                            cpNodo.CostoRacionamiento = yupanaServicio.GetValorParameterGams(rutaFinal, resultado, ConstantesYupana.GamsCostoRacionamiento);
                            cpNodo.ListaCMarginales = yupanaServicio.GetLista48EcuacionGams(rutaFinal, resultado, cpNodo.Topcodi, 1, ConstantesYupana.GamsCostosMarginalesBarra, ConstantesBase.NodoTopologico);
                        }

                        GuardaBDNodoSimulado(cpNodo);
                    }
                    else
                    {
                        ////código para pruebas 
                        //EsperarTiempo(ConstantesYupanaContinuo.TipoMinuto, 1);

                        //cpNodo.Cpnodoconverge = "C";
                        //cpNodo.CostoOperacion = 777777;
                        //cpNodo.CostoRacionamiento = 0;
                        //cpNodo.ListaCMarginales = new List<MeMedicion48DTO>();
                        //cpNodo.Cpnodomsjproceso = "Escenario ejecutado correctamente";

                        //GuardaBDNodoSimulado(cpNodo);
                    }
                }
                catch (Exception ex)
                {
                    cpNodo.Cpnodomsjproceso = ex.Message;
                    cpNodo.Cpnodoestado = "EE";    // C: creado, E: ejecutado, EE: ejecutado con error, N: no ejecutado por divergencia, TP: no ejecutado por tiempo prolongado
                    cpNodo.Cpnodoconverge = null;
                    GuardaBDNodoSimulado(cpNodo);

                    Logger.Error("Error simulación en escenario" + numNodo, ex);
                }
            }

            Logger.Info("hoja " + numNodo + " despues de actualizar");

            return cpNodo;
        }

        /// <summary>
        /// Guarda los resultados del nodo y su detalle en la BD
        /// </summary>
        /// <param name="nodoSimulado"></param>
        private void GuardaBDNodoSimulado(CpNodoContinuoDTO nodoSimulado)
        {
            //Actualizo "nodo" con los nuevos datos
            EsperarTiempo(ConstantesYupanaContinuo.TipoSegundo, 1);
            CpNodoContinuoDTO nodoGuardar = GetByNumeroCpNodoContinuo(nodoSimulado.Cparbcodi, nodoSimulado.Cpnodonumero);
            if (nodoGuardar.Cpnodofecfinproceso == null)
            {
                nodoSimulado.Cpnodofecfinproceso = DateTime.Now;

                try
                {
                    if (nodoSimulado.Cpnodoconverge == "C")
                    {
                        //Actualizo las salidas de la simulacion en "nodo detalle"
                        List<CpNodoDetalleDTO> listaNodosDetalle = ListCpNodoDetallesPorNodo(nodoSimulado.Cpnodocodi);
                        ActualizarCpNodosDetalle(nodoSimulado, listaNodosDetalle);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    //throw new ArgumentException(ex.Message, ex);
                    nodoSimulado.Cpnodoconverge = null;
                    nodoSimulado.Cpnodomsjproceso = "Ocurrió un error al guardar los resultados del nodo en bd: " + ex.ToString();
                }

                if (nodoSimulado.Cpnodomsjproceso != null && nodoSimulado.Cpnodomsjproceso.Length > 500)
                    nodoSimulado.Cpnodomsjproceso = nodoSimulado.Cpnodomsjproceso.Substring(0, 500);

                UpdateCpNodoContinuo(nodoSimulado);
            }

            // Observación: El porcentaje del árbol se guarda cada vez que se refresca la pantalla 
            ActualizarPorcentajeArbol(nodoSimulado.Cparbcodi, out decimal porcentajeArbolSimulado);
        }

        #region Guardar resultados del Nodo

        /// <summary>
        /// Invoca a las funciones correspondientes para guardar los detalles del nodo
        /// </summary>
        /// <param name="nodoSimulado"></param>
        /// <param name="listaNodosDetalle"></param>
        private void ActualizarCpNodosDetalle(CpNodoContinuoDTO nodoSimulado, List<CpNodoDetalleDTO> listaNodosDetalle)
        {
            //Guardamos Costos Marginales
            GuardarCostosMarginales(nodoSimulado, listaNodosDetalle);

            //Guardamos CO y CR
            ActualizarNodoDet(listaNodosDetalle.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCO), nodoSimulado.CostoOperacion);
            ActualizarNodoDet(listaNodosDetalle.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCR), nodoSimulado.CostoRacionamiento);
        }

        /// <summary>
        /// Guarda los resultados (CO, CR y CMg) a la BD
        /// </summary>
        /// <param name="nodoSimulado"></param>
        /// <param name="listaNodosDetalle"></param>
        private void GuardarCostosMarginales(CpNodoContinuoDTO nodoSimulado, List<CpNodoDetalleDTO> listaNodosDetalle)
        {
            try
            {
                List<MeMedicion48DTO> lstCMgBarraOriginal = nodoSimulado.ListaCMarginales;
                int horaInicioCMg = nodoSimulado.Topiniciohora;
                if (horaInicioCMg <= 0 || horaInicioCMg > 48)
                    throw new ArgumentException("Cálculo de costos marginales fuera del rango horario");

                List<MeMedicion48DTO> lstCMgBarraFiltrada = FiltrarListaCMg(lstCMgBarraOriginal, horaInicioCMg);

                //Periodo de Media: Periodo horario comprendido entre a las 08:00 y 18:00 horas.
                List<int> listaIndicesPeriodoMedio = ObtenerLstIndices(17, 36);

                //Periodo de Punta: Periodo horario comprendido entre a las 18:00 y 24:00 horas.
                List<int> listaIndicesPeriodoMaximo = ObtenerLstIndices(37, 48);

                //Periodo de Base: Periodo horario comprendido entre a las 00:00 y 08:00 horas.
                List<int> listaIndicesPeriodoMinimo = ObtenerLstIndicesMinimo(listaIndicesPeriodoMedio, listaIndicesPeriodoMaximo);

                //Obtener listado de CMg por periodo
                List<decimal?> lstCMgPeriodoMinimo = ObtenerListadoCMgPorPeriodo(lstCMgBarraFiltrada, listaIndicesPeriodoMinimo);
                List<decimal?> lstCMgPeriodoMedio = ObtenerListadoCMgPorPeriodo(lstCMgBarraFiltrada, listaIndicesPeriodoMedio);
                List<decimal?> lstCMgPeriodoMaximo = ObtenerListadoCMgPorPeriodo(lstCMgBarraFiltrada, listaIndicesPeriodoMaximo);

                decimal? cmMinimoEnPeriodoMin = lstCMgPeriodoMinimo.Min(x => x);
                decimal? cmMaximoEnPeriodoMin = lstCMgPeriodoMinimo.Max(x => x);
                decimal? cmPromedioEnPeriodoMin = lstCMgPeriodoMinimo.Average(x => x);

                decimal? cmMinimoEnPeriodoMedio = lstCMgPeriodoMedio.Min(x => x);
                decimal? cmMaximoEnPeriodoMedio = lstCMgPeriodoMedio.Max(x => x);
                decimal? cmPromedioEnPeriodoMedio = lstCMgPeriodoMedio.Average(x => x);

                decimal? cmMinimoEnPeriodoMax = lstCMgPeriodoMaximo.Min(x => x);
                decimal? cmMaximoEnPeriodoMax = lstCMgPeriodoMaximo.Max(x => x);
                decimal? cmPromedioEnPeriodoMax = lstCMgPeriodoMaximo.Average(x => x);

                ActualizarNodoDet(listaNodosDetalle.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCMgMinPerMin), cmMinimoEnPeriodoMin);
                ActualizarNodoDet(listaNodosDetalle.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCMgMaxPerMin), cmMaximoEnPeriodoMin);
                ActualizarNodoDet(listaNodosDetalle.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCMgPromPerMin), cmPromedioEnPeriodoMin);

                ActualizarNodoDet(listaNodosDetalle.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCMgMinPerMed), cmMinimoEnPeriodoMedio);
                ActualizarNodoDet(listaNodosDetalle.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCMgMaxPerMed), cmMaximoEnPeriodoMedio);
                ActualizarNodoDet(listaNodosDetalle.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCMgPromPerMed), cmPromedioEnPeriodoMedio);

                ActualizarNodoDet(listaNodosDetalle.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCMgMinPerMax), cmMinimoEnPeriodoMax);
                ActualizarNodoDet(listaNodosDetalle.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCMgMaxPerMax), cmMaximoEnPeriodoMax);
                ActualizarNodoDet(listaNodosDetalle.Find(x => x.Cpnconcodi == ConstantesYupanaContinuo.ConcepcodiCMgPromPerMax), cmPromedioEnPeriodoMax);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guardar los resultados a la BD
        /// </summary>
        /// <param name="objetoNodoDetalle"></param>
        /// <param name="valor"></param>
        private void ActualizarNodoDet(CpNodoDetalleDTO objetoNodoDetalle, decimal? valor)
        {
            try
            {
                string res = "";
                if (valor != null)
                    res = valor.ToString();

                objetoNodoDetalle.Cpndetvalor = res;
                UpdateCpNodoDetalle(objetoNodoDetalle);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
        }

        /// <summary>
        /// los valores nulos o 0 no serán tomados en cuenta para calcular los CMg
        /// </summary>
        /// <param name="listaCMarginales"></param>
        /// <param name="horaInicioCMg"></param>
        /// <returns></returns>
        private List<MeMedicion48DTO> FiltrarListaCMg(List<MeMedicion48DTO> listaCMarginales, int horaInicioCMg)
        {

            List<MeMedicion48DTO> lstFiltrada = new List<MeMedicion48DTO>();

            foreach (var regMed48 in listaCMarginales)
            {
                //no tomar en cuenta valores antes de la horaIni de la topologia (setearlo a null)
                for (int i = 1; i <= (horaInicioCMg - 1); i++)
                {
                    regMed48.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(regMed48, null);
                }

                //en lo restante, si hay ceros setearlo a null
                for (int hx = horaInicioCMg; hx <= 48; hx++)
                {
                    var valHx = (decimal?)regMed48.GetType().GetProperty(ConstantesYupanaContinuo.CaracterH + hx).GetValue(regMed48, null);

                    //Si encuentra un valor mayor a cero, el registro es valido y deja de buscar 
                    if (valHx == 0)
                    {
                        regMed48.GetType().GetProperty(ConstantesAppServicio.CaracterH + hx).SetValue(regMed48, null);
                    }
                }

                lstFiltrada.Add(regMed48);
            }

            return lstFiltrada;
        }

        /// <summary>
        /// Deveulve una lista con los CMg segun sus indices
        /// </summary>
        /// <param name="lstCMgTotal"></param>
        /// <param name="listaIndices"></param>
        /// <returns></returns>
        private List<decimal?> ObtenerListadoCMgPorPeriodo(List<MeMedicion48DTO> lstCMgTotal, List<int> listaIndices)
        {
            List<decimal?> lstPorPeriodo = new List<decimal?>();

            foreach (var cmg in lstCMgTotal)
            {
                for (int hx = 1; hx <= 48; hx++)
                {
                    if (listaIndices.Where(x => x == hx).ToList().Count > 0)
                    {
                        var valHx = (decimal?)cmg.GetType().GetProperty(ConstantesYupanaContinuo.CaracterH + hx).GetValue(cmg, null);
                        lstPorPeriodo.Add(valHx);
                    }
                }
            }
            return lstPorPeriodo;
        }

        /// <summary>
        /// Devuelve una lista de enterios con los indices correspondientes a un rango horario
        /// </summary>
        /// <param name="posIni"></param>
        /// <param name="posFin"></param>
        /// <returns></returns>
        private List<int> ObtenerLstIndices(int posIni, int posFin)
        {
            List<int> lstSalida = new List<int>();

            for (int i = posIni; i <= posFin; i++)
            {
                lstSalida.Add(i);
            }

            return lstSalida;
        }

        /// <summary>
        /// Devuelve el grupo horario para el periodo mínimo
        /// </summary>
        /// <param name="listaIndicesPerMedio"></param>
        /// <param name="listaIndicesPerMaximo"></param>
        /// <returns></returns>
        private List<int> ObtenerLstIndicesMinimo(List<int> listaIndicesPerMedio, List<int> listaIndicesPerMaximo)
        {
            List<int> lstSalida = new List<int>();

            for (int hx = 1; hx <= 48; hx++)
            {
                var coicidenciasEnMedio = listaIndicesPerMedio.Where(x => x == hx).ToList().Count;
                var coicidenciasEnMaximo = listaIndicesPerMaximo.Where(x => x == hx).ToList().Count;
                if (coicidenciasEnMedio == 0 && coicidenciasEnMaximo == 0)
                {
                    lstSalida.Add(hx);
                }
            }

            return lstSalida;
        }

        #endregion

        /// <summary>
        /// Envia notificacion a los nodos que deben dejar de simular
        /// </summary>
        /// <param name="codigoArbol"></param>
        /// <param name="numNodo"></param>
        private void SetearNoSimularNodoEHijos(int codigoArbol, int numNodo)
        {
            switch (numNodo)
            {
                case 1: //1
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 1, true);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 2, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 3, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 4, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 5, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 6, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 7, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 8, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 9, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 10, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 11, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 12, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 13, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 14, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 15, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 16, false);
                    break;
                case 2: // 2
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 2, true);
                    break;
                case 3: // 3, 4
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 3, true);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 4, false);
                    break;
                case 4: // 4
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 4, true);
                    break;
                case 5: // 5 6 7 8
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 5, true);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 6, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 7, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 8, false);
                    break;
                case 6: // 6
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 6, true);
                    break;

                case 7: // 7, 8
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 7, true);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 8, false);
                    break;

                case 8: // 8
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 8, true);
                    break;
                case 9: // 9 10 11 12 13 14 15 16
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 9, true);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 10, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 11, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 12, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 13, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 14, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 15, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 16, false);
                    break;
                case 10: // 10
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 10, true);
                    break;

                case 11: // 11, 12
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 11, true);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 12, false);
                    break;

                case 12: // 12
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 12, true);
                    break;

                case 13: // 13, 14, 15, 16
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 13, true);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 14, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 15, false);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 16, false);
                    break;

                case 14: // 14
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 14, true);
                    break;

                case 15: // 15, 16
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 15, true);
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 16, false);
                    break;

                case 16: // 16
                    GuardaBDNodoNoEjecutadoPorTiempo(codigoArbol, 16, true);
                    break;
            }

            // Observación: El porcentaje del árbol se guarda cada vez que se refresca la pantalla 
            ActualizarPorcentajeArbol(codigoArbol, out decimal porcentajeArbolSimulado);
        }

        /// <summary>
        /// Guarda los nodos cuyo padre, de cualquier nivel, Diverge. Estos nodos ya no son simulados
        /// </summary>
        /// <param name="codigoArbol"></param>
        /// <param name="numNodo"></param>
        private void SetearNoEjecutadoNodosHijos(int codigoArbol, int numNodo)
        {
            switch (numNodo)
            {
                case 4: // 4
                    GuardaBDNodoNoEjecutado(codigoArbol, 4);
                    break;

                case 6: // 6
                    GuardaBDNodoNoEjecutado(codigoArbol, 6);
                    break;

                case 7: // 7, 8
                    GuardaBDNodoNoEjecutado(codigoArbol, 7);
                    GuardaBDNodoNoEjecutado(codigoArbol, 8);
                    break;

                case 8: // 8
                    GuardaBDNodoNoEjecutado(codigoArbol, 8);
                    break;

                case 10: // 10
                    GuardaBDNodoNoEjecutado(codigoArbol, 10);
                    break;

                case 11: // 11, 12
                    GuardaBDNodoNoEjecutado(codigoArbol, 11);
                    GuardaBDNodoNoEjecutado(codigoArbol, 12);
                    break;

                case 12: // 12
                    GuardaBDNodoNoEjecutado(codigoArbol, 12);
                    break;

                case 13: // 13, 14, 15, 16
                    GuardaBDNodoNoEjecutado(codigoArbol, 13);
                    GuardaBDNodoNoEjecutado(codigoArbol, 14);
                    GuardaBDNodoNoEjecutado(codigoArbol, 15);
                    GuardaBDNodoNoEjecutado(codigoArbol, 16);
                    break;

                case 14: // 14
                    GuardaBDNodoNoEjecutado(codigoArbol, 14);
                    break;

                case 15: // 15, 16
                    GuardaBDNodoNoEjecutado(codigoArbol, 15);
                    GuardaBDNodoNoEjecutado(codigoArbol, 16);
                    break;

                case 16: // 16
                    GuardaBDNodoNoEjecutado(codigoArbol, 16);
                    break;
            }

            // Observación: El porcentaje del árbol se guarda cada vez que se refresca la pantalla 
            ActualizarPorcentajeArbol(codigoArbol, out decimal porcentajeArbolSimulado);
        }

        /// <summary>
        /// Guarda el nodo No Ejecutado (cuyo algun padre ha divergido) en la BD
        /// </summary>
        /// <param name="codigoArbol"></param>
        /// <param name="numNodo"></param>
        private void GuardaBDNodoNoEjecutado(int codigoArbol, int numNodo)
        {
            try
            {
                EsperarTiempo(ConstantesYupanaContinuo.TipoSegundo, 1);

                CpNodoContinuoDTO nodoNoEjecutado = GetByNumeroCpNodoContinuo(codigoArbol, numNodo);

                //solo actualizar si no tiene fecha fin de proceso
                if (nodoNoEjecutado.Cpnodofecfinproceso == null)
                {
                    nodoNoEjecutado.Cpnodoestado = "N"; //No Ejecutado
                    nodoNoEjecutado.Cpnodomsjproceso = "Nodo No Ejecutado por divergencia de un padre";
                    nodoNoEjecutado.Cpnodofeciniproceso = DateTime.Now;
                    nodoNoEjecutado.Cpnodofecfinproceso = DateTime.Now;

                    UpdateCpNodoContinuo(nodoNoEjecutado);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guarda los nodos que dejaron de simularse
        /// </summary>
        /// <param name="codigoArbol"></param>
        /// <param name="numNodo"></param>
        /// <param name="mismoNodo"></param>
        private void GuardaBDNodoNoEjecutadoPorTiempo(int codigoArbol, int numNodo, bool mismoNodo)
        {
            try
            {
                EsperarTiempo(ConstantesYupanaContinuo.TipoSegundo, 1);

                CpNodoContinuoDTO nodoGuardar = GetByNumeroCpNodoContinuo(codigoArbol, numNodo);

                //solo actualizar si no tiene fecha fin de proceso
                if (nodoGuardar.Cpnodofecfinproceso == null)
                {
                    nodoGuardar.Cpnodoestado = "TP";    // C: creado, E: ejecutado, EE: ejecutado con error, N: no ejecutado por divergencia, TP: no ejecutado por tiempo prolongado             
                    nodoGuardar.Cpnodofecfinproceso = DateTime.Now;
                    nodoGuardar.Cpnodomsjproceso = "Nodo No Ejecutado por tiempo prolongado";

                    if (nodoGuardar.Cpnodofeciniproceso == null) nodoGuardar.Cpnodofeciniproceso = DateTime.Now;

                    UpdateCpNodoContinuo(nodoGuardar);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException(ex.Message, ex);
            }
        }

        #endregion

        /// <summary>
        /// Generar archivo comprimido de las salidas de las simulaciones por nodo
        /// </summary>
        /// <param name="rutaLocal"></param>
        /// <param name="arbolcodi"></param>
        /// <param name="numeroNodoClikeado"></param>
        /// <param name="nameFile"></param>
        public void GenerarArchivosSalidaPorNodo(string rutaLocal, int arbolcodi, string numeroNodoClikeado, out string nameFile)
        {
            try
            {
                CpArbolContinuoDTO arbolMostrado = GetByIdCpArbolContinuo(arbolcodi);
                string fechaArbol = arbolMostrado.Cparbfecha.ToString(ConstantesAppServicio.FormatoFechaYMD);
                string identificadorArbol = arbolMostrado.Cparbidentificador;
                string tagArbol = arbolMostrado.Cparbtag;


                string path = ConfigurationManager.AppSettings[ConstantesYupanaContinuo.PathYupanaContinuo];
                var directorioNodo = $@"{fechaArbol}\{identificadorArbol}\{tagArbol}\{numeroNodoClikeado}\";

                var nombreZip = $"{identificadorArbol}_{tagArbol}_{numeroNodoClikeado}.zip";
                var rutaZip = rutaLocal + nombreZip;

                if (File.Exists(rutaZip)) File.Delete(rutaZip);

                FileServer.CreateZipFromDirectory(directorioNodo, rutaZip, path);

                nameFile = nombreZip;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Generar archivo comprimido de las salidas de las simulaciones por tag
        /// </summary>
        /// <param name="rutaLocal"></param>
        /// <param name="arbolcodi"></param>
        /// <param name="nameFile"></param>
        public void GenerarArchivosSalidaPorTag(string rutaLocal, int arbolcodi, out string nameFile)
        {
            try
            {
                CpArbolContinuoDTO arbolMostrado = GetByIdCpArbolContinuo(arbolcodi);
                string fechaArbol = arbolMostrado.Cparbfecha.ToString(ConstantesAppServicio.FormatoFechaYMD);
                string identificadorArbol = arbolMostrado.Cparbidentificador;
                string tagArbol = arbolMostrado.Cparbtag;


                string path = ConfigurationManager.AppSettings[ConstantesYupanaContinuo.PathYupanaContinuo];
                var directorioNodo = $@"{fechaArbol}\{identificadorArbol}\{tagArbol}\";

                var nombreZip = $"{identificadorArbol}_{tagArbol}.zip";
                var rutaZip = rutaLocal + nombreZip;

                if (File.Exists(rutaZip)) File.Delete(rutaZip);

                FileServer.CreateZipFromDirectory(directorioNodo, rutaZip, path);

                nameFile = nombreZip;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guardar nodo en base de datos YUPANA
        /// </summary>
        /// <param name="numeroNodoClikeado"></param>
        /// <param name="arbolcodi"></param>
        /// <returns></returns>
        public int GuardarNodoEnTopologia(string numeroNodoClikeado, int arbolcodi)
        {
            int codigo = -1;
            int idnodo = int.Parse(numeroNodoClikeado);
            int topcodiNodo = 0;
            string topNombre = string.Empty;
            int numNodo = numeroNodoClikeado != "" ? Int32.Parse(numeroNodoClikeado) : -1;
            List<CpNodoContinuoDTO> lstNodos = ListCpNodoContinuosPorArbol(arbolcodi);
            var arbol = GetByIdCpArbolContinuo(arbolcodi);
            if (numNodo != -1 && lstNodos.Count > 0)
            {
                string strBinary = Convert.ToString(numNodo - 1, 2).PadLeft(4, '0'); //Devuelve un str : "0000"                

                string flagCT = strBinary[0].ToString() == "1" ? "S" : "N";
                string flagSC = strBinary[1].ToString() == "1" ? "S" : "N";
                string flagCC = strBinary[2].ToString() == "1" ? "S" : "N";
                string flagPR = strBinary[3].ToString() == "1" ? "S" : "N";

                CpNodoContinuoDTO nodo = lstNodos.Find(x => x.Cpnodoflagcondterm == flagCT && x.Cpnodoflagsincompr == flagSC && x.Cpnodoflagconcompr == flagCC && x.Cpnodoflagrer == flagPR);

                if (nodo != null)
                {
                    codigo = nodo.Cpnodocodi;
                    topNombre = "YC_" + nodo.Cpnodocarpeta;
                    InsumoYupanaContinuo objInsumo = GenerarInsumosParaSimulacionDeNodos(arbol.Cparbfecha.Date, arbol.Cparbbloquehorario, arbol.Topcodi);
                    topcodiNodo = yupanaServicio.CrearCopiaEscenario(arbol.Topcodi, topNombre, objInsumo, idnodo);

                    //Agregamos campo topologia al nodo
                    if (topcodiNodo != 0)
                    {
                        nodo.Cpnodoidtopguardado = topcodiNodo;
                        UpdateCpNodoContinuo(nodo);
                    }
                }

            }
            else
            {
                throw new ArgumentException("Código del escenario no encontrado");
            }

            return topcodiNodo;
        }

        #region 04. Sistema distribuido

        public void ValidarPrevioSimulacion(int tipoSimulacion, string fecha, int hora, out DateTime fechaHora, out int topcodi)
        {
            //obtener hora a procesar
            var strFecha = fecha.Split('/').Select(x => Convert.ToInt32(x)).ToArray();
            fechaHora = new DateTime(strFecha[2], strFecha[1], strFecha[0], hora, 0, 0);

            //Verificar simulacion en curso
            CpArbolContinuoDTO ultimoArbol = this.GetUltimoArbolEnEjecucion();
            if (ultimoArbol != null)
            {
                throw new ArgumentException("Existe un árbol en simulación, debe esperar que termine");
            }

            //Verificar que existencia de escenario Yupana
            topcodi = GetTopologiaByDate(fechaHora);
            if (topcodi == 0)
            {
                throw new ArgumentException(ConstantesYupanaContinuo.MensajeNoExisteTopologia);
            }

            if (tipoSimulacion == ConstantesYupanaContinuo.SimulacionManual)
            {
                //Verificar que existan todos los insumos
                VerificarExistenciaInsumo(fechaHora);
            }
        }

        public void EjecutarSimulacionManualXFechaYHora(string fecha, int hora, string usuario)
        {
            //Validación
            ValidarPrevioSimulacion(ConstantesYupanaContinuo.SimulacionManual, fecha, hora, out DateTime fechaHora, out int topcodi);

            //Inicializar estructura de bd
            int codigoArbol = CrearEstructuraArbol(fechaHora, hora, ConstantesYupanaContinuo.SimulacionManual, usuario);

            //Realizar simulacion
            EjecutarSimulacion(codigoArbol);
        }

        public void EjecutarSimulacionAutomaticaXFechaYHora(string fecha, int hora)
        {
            //Validación
            ValidarPrevioSimulacion(ConstantesYupanaContinuo.SimulacionAutomatica, fecha, hora, out DateTime fechaHora, out int topcodi);

            //Crear insumos
            string usuario = "SISTEMA";

            //i1. Configuración de caudal
            ActualizarAutomaticoConfiguracionXTipo(usuario, topcodi, fechaHora, ConstantesYupanaContinuo.TipoConfiguracionCaudal);

            //i2. Configuración de generación RER
            ActualizarAutomaticoConfiguracionXTipo(usuario, topcodi, fechaHora, ConstantesYupanaContinuo.TipoConfiguracionRer);

            //i3. Reporte Caudales
            ActualizarAutomaticoDetalleM48YupanaContinuo(usuario, topcodi, fechaHora, ConstantesYupanaContinuo.TipoConfiguracionCaudal);

            //i4. Reporte Generación RER
            ActualizarAutomaticoDetalleM48YupanaContinuo(usuario, topcodi, fechaHora, ConstantesYupanaContinuo.TipoConfiguracionRer);

            //i5. Condiciones termicas - Unidades forzadas
            ActualizarAutomaticoCondicionTermica(usuario, topcodi, fechaHora);

            //Verificar que existan todos los insumos
            VerificarExistenciaInsumo(fechaHora);

            //Inicializar estructura de bd
            int codigoArbol = CrearEstructuraArbol(fechaHora, fechaHora.Hour, ConstantesYupanaContinuo.SimulacionAutomatica, usuario);

            //Realizar simulacion
            EjecutarSimulacion(codigoArbol);
        }

        #endregion

        #endregion
    }
}

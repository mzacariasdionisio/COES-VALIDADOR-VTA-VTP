using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace COES.Servicios.Aplicacion.Titularidad
{
    public class TitularidadAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TitularidadAppServicio));

        #region Métodos Tabla SI_EMPRESA

        /// <summary>
        /// Permite listar las empresas de la BD
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListAllEmpresas()
        {
            var lista = FactorySic.GetSiEmpresaRepository().ObtenerIdNameEmpresasActivasBajas();

            foreach (var obj in lista)
            {
                obj.Emprruc = string.IsNullOrEmpty(obj.Emprruc) ? string.Empty : obj.Emprruc.Trim();
                obj.Emprrazsocial = string.IsNullOrEmpty(obj.Emprrazsocial) ? string.Empty : obj.Emprrazsocial.Trim();
                obj.Emprnomb = string.IsNullOrEmpty(obj.Emprnomb) ? string.Empty : obj.Emprnomb.Trim();
                obj.EmprnombAnidado = obj.Emprcodi + " - " + obj.Emprruc + " - " + obj.Emprnomb;
            }

            return lista;
        }

        /// <summary>
        /// Permite obtener una empresa en base al ID
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public SiEmpresaDTO GetSiEmpresaById(int idEmpresa)
        {
            var obj = FactorySic.GetSiEmpresaRepository().GetById(idEmpresa);

            if (obj != null)
            {
                obj.Emprruc = string.IsNullOrEmpty(obj.Emprruc) ? string.Empty : obj.Emprruc.Trim();
                obj.Emprrazsocial = string.IsNullOrEmpty(obj.Emprrazsocial) ? string.Empty : obj.Emprrazsocial.Trim();
                obj.Emprnomb = string.IsNullOrEmpty(obj.Emprnomb) ? string.Empty : obj.Emprnomb.Trim();
                obj.Emprabrev = string.IsNullOrEmpty(obj.Emprabrev) ? string.Empty : obj.Emprabrev.Trim();
                obj.EmprestadoDesc = Util.EmpresaEstadoDescripcion(obj.Emprestado);
                obj.EmprcoesDesc = Util.SiNoDescripcion(obj.Emprcoes);
                obj.EmprseinDesc = Util.SiNoDescripcion(obj.Emprsein);
                obj.EmpragenteDesc = Util.AgenteEstadoDescripcion(obj.Empragente);
                obj.Tipoemprdesc = string.IsNullOrEmpty(obj.Tipoemprdesc) ? string.Empty : obj.Tipoemprdesc.Trim();
            }

            return obj;
        }

        #endregion

        #region Métodos Tabla EQ_AREA

        /// <summary>
        /// Permite listar las areas operativas por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<EqAreaDTO> ListarAreaXEmpresa(int idEmpresa)
        {
            var listaFinal = new List<EqAreaDTO>();
            var listaIni = FactorySic.GetEqAreaRepository().ListarAreaPorEmpresas(idEmpresa.ToString(), "'-1'");
            if (listaIni.Find(x => x.Areacodi == 0) == null)
            {
                listaFinal.Add(new EqAreaDTO() { Areacodi = 0, Areanomb = "_ (TODOS)" });
                listaFinal.AddRange(listaIni);
            }
            else
            {
                listaFinal = listaIni;
            }

            return listaFinal;
        }

        #endregion

        #region Métodos Tabla EQ_EQUIPO

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarEquiposXEmpresa(int idEmpresa)
        {
            List<EqEquipoDTO> listaEquipo = FactorySic.GetEqEquipoRepository().ListaEquipamientoPaginado(idEmpresa, -2, -2, -2
                , " ", string.Empty, -1, -1).Where(x => x.Equiestado != ConstantesAppServicio.Anulado).ToList();

            foreach (var reg in listaEquipo)
            {
                reg.Equinomb = !string.IsNullOrEmpty(reg.Equinomb) ? reg.Equinomb : reg.Equiabrev;
                reg.Equiabrev = !string.IsNullOrEmpty(reg.Equiabrev) ? reg.Equiabrev : reg.Equinomb;
                reg.Osigrupocodi = EquipamientoHelper.EstiloEstado(reg.Equiestado);
            }

            //no considerar a equipos que tienen baja pero han tenido TTIEs antiguos
            listaEquipo = listaEquipo.Where(x => !(x.Lastcodi > 0 && x.Equiestado != ConstantesAppServicio.Activo)).ToList();

            /*
             * Este código solo debe realizar para las transferencias históricas (antes del TTIE, como son Edegel a Enel, Statkraft a Statkraft SA)
            //Validar equipos antiguos para mostrar data
            var eqAntiguosMigra = FactorySic.GetSiHisempeqDataRepository().List().Where(x => x.Equicodi != x.Equicodiold || x.Emprcodi != idEmpresa).ToList();
            List<int> eqBajasAntiguos = new List<int>();
            foreach (var equip in listaEquipo)
            {
                if (eqAntiguosMigra.Find(x => x.Equicodiold == equip.Equicodi) != null)
                    eqBajasAntiguos.Add(equip.Equicodi);
            }

            listaEquipo.RemoveAll(i => eqBajasAntiguos.Contains(i.Equicodi));

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>CASO ESPECIAL STATKRAFT hacia  STATKRAFT  S.A.

            List<int> lascodis = new List<int>();
            foreach (var i in listaEquipo)
            {
                lascodis.Add(i.Equicodi);
            }

            var empresaEspecial = 11567;
            if (idEmpresa == 10636)
            {
                var listaEqMigracion = FactorySic.GetEqEquipoRepository().ListaEquipamientoPaginado(empresaEspecial, -2, -2, -2
                , " ", string.Empty, -1, -1).Where(x => x.Equiestado != ConstantesAppServicio.Anulado).ToList();

                listaEqMigracion.RemoveAll(i => lascodis.Contains(i.Lastcodi.Value));
                foreach (var item in listaEqMigracion)
                {
                    item.Emprcodi = 10636;
                }
                listaEquipo.AddRange(listaEqMigracion);
            }
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            */

            listaEquipo = listaEquipo.OrderBy(x => x.Areanomb).ThenBy(x => x.Famabrev).ThenBy(x => x.Equinomb).ToList();

            return listaEquipo;
        }

        /// <summary>
        /// Listar objetos de Equipos que son validos
        /// </summary>
        /// <param name="tmopercodi"></param>
        /// <param name="emprcodiOrig"></param>
        /// <param name="listaEquicodi"></param>
        /// <returns></returns>
        private List<EqEquipoDTO> ListarObjEquipoValidosATransferir(int tmopercodi, int emprcodiOrig, List<int> listaEquicodi)
        {
            var listaObj = ListarEquiposXEmpresa(emprcodiOrig).Where(x => x.Equiestado != ConstantesAppServicio.Anulado).ToList();
            if (tmopercodi == ConstantesTitularidad.TipoMigrTransferenciaEquipos || tmopercodi == ConstantesTitularidad.TipoMigrInstalacionNoCorresponden)
                return listaObj.Where(x => x.Equicodi > 0 && listaEquicodi.Contains(x.Equicodi)).ToList();
            else return listaObj;
        }

        /// <summary>
        /// Listar equipo y fecha inicial de datos para completar el emprcodi
        /// </summary>
        /// <param name="listaEquicodis"></param>
        /// <param name="fechaCorte"></param>
        /// <returns></returns>
        public List<TTIEEntidad> ListarEqFechaInicial(List<int> listaEquicodis, DateTime fechaCorte)
        {
            List<TTIEEntidad> lFinal = new List<TTIEEntidad>();

            List<SiHisempeqDataDTO> listaDat = GetByCriteriaSiHisempeqDatas();

            //filtrar los equipos que van a tener TTIE y solo considerar las relaciones de inicio
            listaDat = listaDat.Where(x => x.Heqdatestado == ConstantesTitularidad.EstadoRelEmpFechaInicio && listaEquicodis.Contains(x.Equicodi)).ToList();

            //
            foreach (var equicodi in listaEquicodis)
            {
                var listaTmpXEq = listaDat.Where(x => x.Equicodi == equicodi).OrderByDescending(x => x.Heqdatfecha);
                DateTime fechaInicial = listaTmpXEq.Any() ? listaTmpXEq.First().Heqdatfecha : DateTime.MinValue;

                TTIEEntidad obj = new TTIEEntidad()
                {
                    Codigo = equicodi,
                    FechaCorte = fechaCorte,
                    FechaInicial = fechaInicial
                };

                lFinal.Add(obj);
            }

            return lFinal;
        }

        #endregion

        #region Métodos Tabla EQ_FAMILIA
        /// <summary>
        /// Devuelve lista de familia xEmpresa
        /// </summary>
        /// <returns></returns>
        public List<EqFamiliaDTO> ListarFamiliaXEmp(int idEmpresa)
        {
            return FactorySic.GetEqFamiliaRepository().ListarFamiliaXEmp(idEmpresa);
        }

        #endregion

        #region Métodos Tabla ME_ORIGENLECTURA

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_ORIGENLECTURA segun empresa
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public List<MeOrigenlecturaDTO> ListarOrigenlecturaByEmprcodi(int emprcodi)
        {
            return FactorySic.GetMeOrigenlecturaRepository().ListByEmprcodi(emprcodi);
        }

        #endregion

        #region Métodos tabla ME_PTOMEDICION

        /// <summary>
        /// Listar puntos de medición de una empresa
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> GetByCriteriaMeptomedicionXempresa(string emprcodi)
        {
            var lista = FactorySic.GetMePtomedicionRepository().GetByCriteria(emprcodi, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto)
                .OrderBy(x => x.Emprnomb).ThenBy(x => x.Origlectnombre).ThenBy(x => x.Ptomedidesc).ToList();

            foreach (var reg in lista)
            {
                reg.Ptomedielenomb = string.IsNullOrEmpty(reg.Ptomedielenomb) ? string.Empty : reg.Ptomedielenomb.Trim();
                reg.Ptomedibarranomb = string.IsNullOrEmpty(reg.Ptomedibarranomb) ? string.Empty : reg.Ptomedibarranomb.Trim();
                reg.Ptomedidesc = string.IsNullOrEmpty(reg.Ptomedidesc) ? string.Empty : reg.Ptomedidesc.Trim();

                reg.Ptomedielenomb = reg.Ptomedielenomb.Length > 0 ? reg.Ptomedielenomb : reg.Ptomedibarranomb.Length > 0 ? reg.Ptomedibarranomb : reg.Ptomedidesc;

                reg.Origlectnombre = string.IsNullOrEmpty(reg.Origlectnombre) ? string.Empty : reg.Origlectnombre.Trim();
            }

            return lista;
        }

        /// <summary>
        /// Listar todos los punto de medición
        /// </summary>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListarPtoMedicion()
        {
            return FactorySic.GetMePtomedicionRepository().List(ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto);
        }

        /// <summary>
        /// Listar codigos de puntos de medicion segun equicodis
        /// </summary>
        /// <param name="emprcodiOrig"></param>
        /// <param name="listaEquicodi"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListarObjPtomedicodiByListaEquicodi(int tmopercodi, int emprcodiOrig, List<int> listaEquicodi, List<int> listaGrupocodi)
        {
            var listaObj = this.GetByCriteriaMeptomedicionXempresa(emprcodiOrig.ToString()).Where(x => x.Ptomediestado != ConstantesAppServicio.Anulado).ToList();

            if (tmopercodi == ConstantesTitularidad.TipoMigrTransferenciaEquipos || tmopercodi == ConstantesTitularidad.TipoMigrInstalacionNoCorresponden)
                listaObj = listaObj.Where(x => x.Ptomedicodi > 0 && (listaEquicodi.Contains(x.Equicodi ?? -1) || listaGrupocodi.Contains(x.Grupocodi ?? -1))).ToList();

            //no considerar a puntos de medición que tienen baja pero han tenido TTIEs antiguos
            listaObj = listaObj.Where(x => !(x.Lastcodi > 0 && x.Equiestado != ConstantesAppServicio.Activo)).ToList();

            //if (emprcodiOrig == 10636)
            //{
            //    listaObj.Where(x => x.Ptomedicodi > 0 && (listaEquicodi.Contains(x.Equicodi ?? -1) || listaGrupocodi.Contains(x.Grupocodi ?? -1))).ToList();
            //    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Caso Especial Caso Especial  STATKRAFT hacia  STATKRAFT  S.A.
            //    List<int> lascodis = new List<int>();
            //    foreach (var i in listaObj)
            //    {
            //        lascodis.Add(i.Ptomedicodi);
            //    }

            //    //caso especial
            //    var empresaEspecial = 11567;
            //    if (emprcodiOrig == 10636)
            //    {
            //        var listPuntosMigrAnterior = this.GetByCriteriaMeptomedicionXempresa(empresaEspecial.ToString()).Where(x => x.Ptomediestado != ConstantesAppServicio.Anulado).ToList();

            //        listPuntosMigrAnterior.RemoveAll(i => lascodis.Contains(i.Lastcodi));
            //        foreach (var item in listPuntosMigrAnterior)
            //        {
            //            item.Emprcodi = 10636;
            //        }
            //        listaObj.AddRange(listPuntosMigrAnterior);
            //    }
            //}

            return listaObj;
        }

        /// <summary>
        /// Listar puntos y fecha inicial de datos para completar el emprcodi
        /// </summary>
        /// <param name="listaPtomedicodis"></param>
        /// <param name="fechaCorte"></param>
        /// <returns></returns>
        public List<TTIEEntidad> ListarPtoFechaInicial(List<int> listaPtomedicodis, DateTime fechaCorte)
        {
            List<TTIEEntidad> lFinal = new List<TTIEEntidad>();

            List<SiHisempptoDataDTO> listaDat = GetByCriteriaSiHisempptoDatas();

            //filtrar los equipos que van a tener TTIE y solo considerar las relaciones de inicio
            listaDat = listaDat.Where(x => x.Hptdatptoestado == ConstantesTitularidad.EstadoRelEmpFechaInicio && listaPtomedicodis.Contains(x.Ptomedicodi)).ToList();

            //
            foreach (var ptomedicodi in listaPtomedicodis)
            {
                var listaTmpXEq = listaDat.Where(x => x.Ptomedicodi == ptomedicodi).OrderByDescending(x => x.Hptdatfecha);
                DateTime fechaInicial = listaTmpXEq.Any() ? listaTmpXEq.First().Hptdatfecha : DateTime.MinValue;

                TTIEEntidad obj = new TTIEEntidad()
                {
                    Codigo = ptomedicodi,
                    FechaCorte = fechaCorte,
                    FechaInicial = fechaInicial
                };

                lFinal.Add(obj);
            }

            return lFinal;
        }

        #endregion

        #region Métodos tabla PR_GRUPO

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_GRUPO
        /// </summary>
        public List<PrGrupoDTO> ListarPrGruposXEmpresa(int emprcodi)
        {
            return FactorySic.GetPrGrupoRepository().ListarPrGruposXEmpresa(emprcodi);
        }

        /// <summary>
        /// Listar la lista de codigos de grupo segun equicodis y ptomedicodis
        /// </summary>
        /// <param name="emprcodiOrig"></param>
        /// <param name="listaEquicodi"></param>
        /// <param name="listaPtomedicodi"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> ListarObjGrupoByListaEquicodiAndListaPtomedicodi(int tmopercodi, int emprcodiOrig, List<int> listaEquicodi, List<int> listaPtomedicodi)
        {
            List<int> listaGrupocodi = new List<int>();

            List<PrGrupoDTO> listaGrupo = this.ListarPrGruposXEmpresa(emprcodiOrig);

            List<EqEquipoDTO> listaEquipo = this.ListarEquiposXEmpresa(emprcodiOrig).Where(x => listaEquicodi.Contains(x.Equicodi)).ToList();
            List<MePtomedicionDTO> listaPto = this.GetByCriteriaMeptomedicionXempresa(emprcodiOrig.ToString()).Where(x => listaPtomedicodi.Contains(x.Ptomedicodi)).ToList();

            List<int> listaGrupocodiIni = listaEquipo.Select(x => x.Grupocodi ?? 0).Where(x => x > 0).ToList();
            listaGrupocodiIni.AddRange(listaPto.Select(x => x.Grupocodi ?? 0).Where(x => x > 0).ToList());
            listaGrupocodiIni = listaGrupocodiIni.Distinct().ToList();

            var listaGrInicial = listaGrupo.Where(x => listaGrupocodiIni.Contains(x.Grupocodi)).ToList();
            var listaGrPadre0 = listaGrupo.Where(x => listaGrupocodiIni.Contains(x.Grupopadre ?? 0)).ToList();
            var listaGrupocodiPadre0 = listaGrPadre0.Select(x => x.Grupocodi).Where(x => x > 0).ToList();

            var listaGrupocodiPadre1 = listaGrInicial.Select(x => x.Grupopadre ?? 0).Where(x => x > 0).ToList();
            var listaGrPadre1 = listaGrupo.Where(x => listaGrupocodiPadre1.Contains(x.Grupocodi)).ToList();

            var listaGrupocodiPadre2 = listaGrPadre1.Select(x => x.Grupopadre ?? 0).Where(x => x > 0).ToList();
            var listaGrPadre2 = listaGrupo.Where(x => listaGrupocodiPadre2.Contains(x.Grupocodi)).ToList();

            var listaGrupocodiPadre3 = listaGrPadre2.Select(x => x.Grupopadre ?? 0).Where(x => x > 0).ToList();
            var listaGrPadre3 = listaGrupo.Where(x => listaGrupocodiPadre3.Contains(x.Grupocodi)).ToList();

            var listaGrupocodiPadre4 = listaGrPadre3.Select(x => x.Grupopadre ?? 0).Where(x => x > 0).ToList();

            listaGrupocodi.AddRange(listaGrupocodiIni);
            listaGrupocodi.AddRange(listaGrupocodiPadre0);
            listaGrupocodi.AddRange(listaGrupocodiPadre1);
            listaGrupocodi.AddRange(listaGrupocodiPadre2);
            listaGrupocodi.AddRange(listaGrupocodiPadre3);
            listaGrupocodi.AddRange(listaGrupocodiPadre4);

            listaGrupocodi = listaGrupocodi.Distinct().ToList().Where(x => x > 0).ToList();

            if (tmopercodi == ConstantesTitularidad.TipoMigrTransferenciaEquipos || tmopercodi == ConstantesTitularidad.TipoMigrInstalacionNoCorresponden)
                return listaGrupo.Where(x => x.Grupocodi > 0 && listaGrupocodi.Contains(x.Grupocodi)).ToList();
            else
                return listaGrupo;
        }

        /// <summary>
        /// Listar grupos y fecha inicial de datos para completar el emprcodi
        /// </summary>
        /// <param name="listaGrupocodis"></param>
        /// <param name="fechaCorte"></param>
        /// <returns></returns>
        public List<TTIEEntidad> ListarGrupoFechaInicial(List<int> listaGrupocodis, DateTime fechaCorte)
        {
            List<TTIEEntidad> lFinal = new List<TTIEEntidad>();

            List<SiHisempgrupoDataDTO> listaDat = GetByCriteriaSiHisempgrupoDatas();

            //filtrar los equipos que van a tener TTIE y solo considerar las relaciones de inicio
            listaDat = listaDat.Where(x => x.Hgrdatestado == ConstantesTitularidad.EstadoRelEmpFechaInicio && listaGrupocodis.Contains(x.Grupocodi)).ToList();

            //
            foreach (var grupocodi in listaGrupocodis)
            {
                var listaTmpXEq = listaDat.Where(x => x.Grupocodi == grupocodi).OrderByDescending(x => x.Hgrdatfecha);
                DateTime fechaInicial = listaTmpXEq.Any() ? listaTmpXEq.First().Hgrdatfecha : DateTime.MinValue;

                TTIEEntidad obj = new TTIEEntidad()
                {
                    Codigo = grupocodi,
                    FechaCorte = fechaCorte,
                    FechaInicial = fechaInicial
                };

                lFinal.Add(obj);
            }

            return lFinal;
        }

        #endregion

        #region Tablas Maestras SI_MIGRA*

        #region Métodos Tabla SI_MIGRAQUERYBASE

        /// <summary>
        /// Inserta un registro de la tabla SI_MIGRAQUERYBASE
        /// </summary>
        public int SaveSiMigraquerybase(SiMigraquerybaseDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                return FactorySic.GetSiMigraquerybaseRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_MIGRAQUERYBASE
        /// </summary>
        public void UpdateSiMigraquerybase(SiMigraquerybaseDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetSiMigraquerybaseRepository().Update(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_MIGRAQUERYBASE
        /// </summary>
        public SiMigraquerybaseDTO GetByIdSiMigraquerybase(int miqubacodi)
        {
            var reg = FactorySic.GetSiMigraquerybaseRepository().GetById(miqubacodi);
            reg.ListaParametroValor = GetByCriteriaSiMigraqueryparametros(miqubacodi);
            reg.ListaTipoOpValor = GetByCriteriaSiMigraqueryxtipooperacions(miqubacodi);

            if (reg.Miqplacodi != null)
                reg.Plantilla = GetByIdSiMigraqueryplantilla(reg.Miqplacodi.Value);

            return reg;
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SI_MIGRAQUERYBASE
        /// </summary>
        public List<SiMigraquerybaseDTO> GetByCriteriaSiMigraquerybases()
        {
            return FactorySic.GetSiMigraquerybaseRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SI_MIGRAQUERYXTIPOOPERACION

        /// <summary>
        /// Inserta un registro de la tabla SI_MIGRAQUERYXTIPOOPERACION
        /// </summary>
        public int SaveSiMigraqueryxtipooperacion(SiMigraqueryxtipooperacionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                return FactorySic.GetSiMigraqueryxtipooperacionRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_MIGRAQUERYXTIPOOPERACION
        /// </summary>
        public void UpdateSiMigraqueryxtipooperacion(SiMigraqueryxtipooperacionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetSiMigraqueryxtipooperacionRepository().Update(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SI_MIGRAQUERYXTIPOOPERACION
        /// </summary>
        public List<SiMigraqueryxtipooperacionDTO> GetByCriteriaSiMigraqueryxtipooperacions(int miqubacodi)
        {
            return FactorySic.GetSiMigraqueryxtipooperacionRepository().GetByCriteria(miqubacodi);
        }

        #endregion

        #region Métodos Tabla SI_MIGRAQUERYPARAMETRO

        /// <summary>
        /// Inserta un registro de la tabla SI_MIGRAQUERYPARAMETRO
        /// </summary>
        public int SaveSiMigraqueryparametro(SiMigraqueryparametroDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                return FactorySic.GetSiMigraqueryparametroRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_MIGRAQUERYPARAMETRO
        /// </summary>
        public void UpdateSiMigraqueryparametro(SiMigraqueryparametroDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetSiMigraqueryparametroRepository().Update(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SI_MIGRAQUERYPARAMETRO
        /// </summary>
        public List<SiMigraqueryparametroDTO> GetByCriteriaSiMigraqueryparametros(int miqubacodi)
        {
            return FactorySic.GetSiMigraqueryparametroRepository().GetByCriteria(miqubacodi);
        }

        #endregion

        #region Métodos Tabla SI_MIGRAPARAMETRO

        /// <summary>
        /// Inserta un registro de la tabla SI_MIGRAPARAMETRO
        /// </summary>
        public int SaveSiMigraparametro(SiMigraParametroDTO entity)
        {
            try
            {
                return FactorySic.GetSiMigraparametroRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_MIGRAPARAMETRO
        /// </summary>
        public void UpdateSiMigraparametro(SiMigraParametroDTO entity)
        {
            try
            {
                FactorySic.GetSiMigraparametroRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_MIGRAPARAMETRO
        /// </summary>
        public SiMigraParametroDTO GetByIdSiMigraparametro(int miqplacodi)
        {
            var reg = FactorySic.GetSiMigraparametroRepository().GetById(miqplacodi);
            return reg;
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SI_MIGRAPARAMETRO
        /// </summary>
        public List<SiMigraParametroDTO> GetByCriteriaSiMigraParametros()
        {
            var lista = FactorySic.GetSiMigraparametroRepository().GetByCriteria().OrderByDescending(x => x.Migpartipo).ThenBy(x => x.Migparnomb).ToList();

            foreach (var reg in lista)
            {
                reg.MigparfeccreacionDesc = reg.Migparfeccreacion.ToString(ConstantesAppServicio.FormatoFechaFull);
                reg.MigpartipoDesc = reg.Migpartipo == 1 ? "Se reemplaza en tiempo de ejecución" : "Se reemplaza para crear querybase";
            }

            return lista;
        }

        #endregion

        #region Métodos Tabla SI_MIGRAQUERYPLANTILLA

        /// <summary>
        /// Inserta un registro de la tabla SI_MIGRAQUERYPLANTILLA
        /// </summary>
        public int SaveSiMigraqueryplantilla(SiMigraqueryplantillaDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                return FactorySic.GetSiMigraqueryplantillaRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_MIGRAQUERYPLANTILLA
        /// </summary>
        public void UpdateSiMigraqueryplantilla(SiMigraqueryplantillaDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetSiMigraqueryplantillaRepository().Update(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_MIGRAQUERYPLANTILLA
        /// </summary>
        public SiMigraqueryplantillaDTO GetByIdSiMigraqueryplantilla(int miqplacodi)
        {
            var reg = FactorySic.GetSiMigraqueryplantillaRepository().GetById(miqplacodi);
            reg.ListaParametro = GetByCriteriaSiMigraqueryplantparams(miqplacodi);
            reg.ListaTipoOp = GetByCriteriaSiMigraqueryplanttops(miqplacodi);

            return reg;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_MIGRAQUERYPLANTILLA
        /// </summary>
        public List<SiMigraqueryplantillaDTO> ListSiMigraqueryplantillas()
        {
            return FactorySic.GetSiMigraqueryplantillaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiMigraqueryplantilla
        /// </summary>
        public List<SiMigraqueryplantillaDTO> GetByCriteriaSiMigraqueryplantillas()
        {
            var lista = FactorySic.GetSiMigraqueryplantillaRepository().GetByCriteria().OrderBy(x => x.Miqplanomb).ToList();

            foreach (var reg in lista)
            {
                reg.MiqplafeccreacionDesc = reg.Miqplafeccreacion.ToString(ConstantesAppServicio.FormatoFechaFull);
            }

            return lista;
        }

        #endregion

        #region Métodos Tabla SI_MIGRAQUERYPLANTPARAM

        /// <summary>
        /// Inserta un registro de la tabla SI_MIGRAQUERYPLANTPARAM
        /// </summary>
        public void SaveSiMigraqueryplantparam(SiMigraqueryplantparamDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetSiMigraqueryplantparamRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_MIGRAQUERYPLANTPARAM
        /// </summary>
        public void UpdateSiMigraqueryplantparam(SiMigraqueryplantparamDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetSiMigraqueryplantparamRepository().Update(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_MIGRAQUERYPLANTPARAM
        /// </summary>
        public SiMigraqueryplantparamDTO GetByIdSiMigraqueryplantparam(int miplprcodi)
        {
            return FactorySic.GetSiMigraqueryplantparamRepository().GetById(miplprcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_MIGRAQUERYPLANTPARAM
        /// </summary>
        public List<SiMigraqueryplantparamDTO> ListSiMigraqueryplantparams()
        {
            return FactorySic.GetSiMigraqueryplantparamRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiMigraqueryplantparam
        /// </summary>
        public List<SiMigraqueryplantparamDTO> GetByCriteriaSiMigraqueryplantparams(int miqplacodi)
        {
            return FactorySic.GetSiMigraqueryplantparamRepository().GetByCriteria(miqplacodi);
        }

        #endregion

        #region Métodos Tabla SI_MIGRAQUERYPLANTTOP

        /// <summary>
        /// Inserta un registro de la tabla SI_MIGRAQUERYPLANTTOP
        /// </summary>
        public void SaveSiMigraqueryplanttop(SiMigraqueryplanttopDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetSiMigraqueryplanttopRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_MIGRAQUERYPLANTTOP
        /// </summary>
        public void UpdateSiMigraqueryplanttop(SiMigraqueryplanttopDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetSiMigraqueryplanttopRepository().Update(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_MIGRAQUERYPLANTTOP
        /// </summary>
        public SiMigraqueryplanttopDTO GetByIdSiMigraqueryplanttop(int miptopcodi)
        {
            return FactorySic.GetSiMigraqueryplanttopRepository().GetById(miptopcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_MIGRAQUERYPLANTTOP
        /// </summary>
        public List<SiMigraqueryplanttopDTO> ListSiMigraqueryplanttops()
        {
            return FactorySic.GetSiMigraqueryplanttopRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiMigraqueryplanttop
        /// </summary>
        public List<SiMigraqueryplanttopDTO> GetByCriteriaSiMigraqueryplanttops(int miqplacodi)
        {
            return FactorySic.GetSiMigraqueryplanttopRepository().GetByCriteria(miqplacodi);
        }

        #endregion

        #region Métodos Tabla SI_HISEMPPTO_DATA

        /// <summary>
        /// Inserción de nuevo registro en tabla SI_HISEMPPTO_DATA
        /// </summary>
        /// <param name="entity"></param>
        public void SaveSiHisempptoDataInicial(int emprcodi, int ptomedicodi, string ptomediestado, string usuarioCreacion)
        {
            if (emprcodi > 0 && ptomedicodi > 0)
            {
                SiHisempptoDataDTO entity = new SiHisempptoDataDTO
                {
                    Hptdatfecha = new DateTime(1900, 1, 1),
                    Emprcodi = emprcodi,
                    Ptomedicodi = ptomedicodi,
                    Hptdatptoestado = ConstantesTitularidad.EstadoRelEmpFechaInicio,
                    Ptomedicodiold = ptomedicodi,
                    Ptomedicodiactual = ptomedicodi,
                    Hptdatusucreacion = usuarioCreacion,
                    Hptdatfeccreacion = DateTime.Now
                };

                DbTransaction tran = null;
                try
                {
                    
                    List<SiHisempptoDataDTO> data = FactorySic.GetSiHisempptoDataRepository().GetByCriteria().Where(x => x.Emprcodi == emprcodi && x.Ptomedicodi == ptomedicodi).ToList();

                    if(data.Count == 0)
                    {
                        IDbConnection conn = FactorySic.GetSiMigracionRepository().BeginConnection();
                        tran = FactorySic.GetSiMigracionRepository().StartTransaction(conn);
                        int maxId = FactorySic.GetSiHisempptoDataRepository().GetMaxId();

                        entity.Hptdatcodi = maxId;
                        FactorySic.GetSiHisempptoDataRepository().Save(entity, conn, tran);

                        tran.Commit();
                    }
                    
                }
                catch (Exception ex)
                {
                    if (tran != null)
                        tran.Rollback();
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Inserción de nuevo registro en tabla SI_HISEMPPTO_DATA
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteSiHisempptoDataByPuntoMedicion(int ptomedicodi, string username)
        {
            if (ptomedicodi > 0)
            {
                DbTransaction tran = null;
                try
                {

                    List<SiHisempptoDataDTO> data = FactorySic.GetSiHisempptoDataRepository().GetByCriteria().Where(x => x.Ptomedicodi == ptomedicodi).ToList();

                    if (data.Count > 0)
                    {
                        IDbConnection conn = FactorySic.GetSiMigracionRepository().BeginConnection();
                        tran = FactorySic.GetSiMigracionRepository().StartTransaction(conn);
                        foreach (SiHisempptoDataDTO siHisempptoDataDTO in data)
                        {
                            this.DeleteSiHisempptoData(siHisempptoDataDTO.Hptdatcodi, username);
                        }
                        tran.Commit();
                    }

                }
                catch (Exception ex)
                {
                    if (tran != null)
                        tran.Rollback();
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_HISEMPPTO_DATA
        /// </summary>
        public void UpdateSiHisempptoData(SiHisempptoDataDTO entity)
        {
            try
            {
                FactorySic.GetSiHisempptoDataRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_HISEMPPTO_DATA
        /// </summary>
        public void DeleteSiHisempptoData(int hptdatcodi, string username)
        {
            try
            {
                FactorySic.GetSiHisempptoDataRepository().Delete(hptdatcodi);
                FactorySic.GetSiHisempptoDataRepository().Delete_UpdateAuditoria(hptdatcodi, username);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_HISEMPPTO_DATA
        /// </summary>
        public SiHisempptoDataDTO GetByIdSiHisempptoData(int hptdatcodi)
        {
            return FactorySic.GetSiHisempptoDataRepository().GetById(hptdatcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_HISEMPPTO_DATA
        /// </summary>
        public List<SiHisempptoDataDTO> ListSiHisempptoDatas(string codis)
        {
            var lista = new List<SiHisempptoDataDTO>();

            if (!string.IsNullOrEmpty(codis))
            {
                List<int> listaCodigo = codis.Split(',').Select(x => int.Parse(x)).ToList();

                int maxElementosPorSublista = 400;
                for (int i = 0; i < listaCodigo.Count; i += maxElementosPorSublista)
                {
                    List<int> sublista = listaCodigo.GetRange(i, Math.Min(maxElementosPorSublista, listaCodigo.Count - i));
                    lista.AddRange(FactorySic.GetSiHisempptoDataRepository().List(string.Join(",", sublista)));
                }
            }

            return lista.OrderByDescending(X => X.Hptdatfecha).ToList();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiHisempptoData
        /// </summary>
        public List<SiHisempptoDataDTO> GetByCriteriaSiHisempptoDatas()
        {
            return FactorySic.GetSiHisempptoDataRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SI_HISEMPEQ_DATA

        /// <summary>
        /// Inserción de nuevo registro en tabla SI_HISEMPEQ_DATA
        /// </summary>
        /// <param name="entity"></param>
        public void SaveSiHisempeqDataInicial(int emprcodi, int equicodi, string equiestado, string usuarioCreacion)
        {
            if (emprcodi > 0 && equicodi > 0)
            {
                SiHisempeqDataDTO entity = new SiHisempeqDataDTO
                {
                    Heqdatfecha = new DateTime(1900, 1, 1),
                    Emprcodi = emprcodi,
                    Equicodi = equicodi,
                    Heqdatestado = ConstantesTitularidad.EstadoRelEmpFechaInicio,
                    Equicodiold = equicodi,
                    Equicodiactual = equicodi,
                    Heqdatusucreacion = usuarioCreacion,
                    Heqdatfeccreacion = DateTime.Now
                };

                DbTransaction tran = null;
                try
                {
                    IDbConnection conn = FactorySic.GetSiMigracionRepository().BeginConnection();
                    tran = FactorySic.GetSiMigracionRepository().StartTransaction(conn);
                    int maxId = FactorySic.GetSiHisempeqDataRepository().GetMaxId();

                    entity.Heqdatcodi = maxId;
                    FactorySic.GetSiHisempeqDataRepository().Save(entity, conn, tran);

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    if (tran != null)
                        tran.Rollback();
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_HISEMPEQ_DATA
        /// </summary>
        public void UpdateSiHisempeqData(SiHisempeqDataDTO entity)
        {
            try
            {
                FactorySic.GetSiHisempeqDataRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_HISEMPEQ_DATA
        /// </summary>
        public void DeleteSiHisempeqData(int heqdatcodi, string username)
        {
            try
            {
                FactorySic.GetSiHisempeqDataRepository().Delete(heqdatcodi);
                FactorySic.GetSiHisempeqDataRepository().Delete_UpdateAuditoria(heqdatcodi, username);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_HISEMPEQ_DATA
        /// </summary>
        public SiHisempeqDataDTO GetByIdSiHisempeqData(int heqdatcodi)
        {
            return FactorySic.GetSiHisempeqDataRepository().GetById(heqdatcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_HISEMPEQ_DATA
        /// </summary>
        public List<SiHisempeqDataDTO> ListSiHisempeqDatas(string equicodis)
        {
            return FactorySic.GetSiHisempeqDataRepository().List(equicodis).OrderByDescending(X => X.Heqdatfecha).ToList();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiHisempeqData
        /// </summary>
        public List<SiHisempeqDataDTO> GetByCriteriaSiHisempeqDatas()
        {
            return FactorySic.GetSiHisempeqDataRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SI_HISEMPGRUPO_DATA

        /// <summary>
        /// Inserción de nuevo registro en tabla SI_HISEMPGRUPO_DATA
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="grupocodi"></param>
        /// <param name="ptomediestado"></param>
        /// <param name="usuarioCreacion"></param>
        public void SaveSiHisempgrupoDataInicial(int emprcodi, int grupocodi, string ptomediestado, string usuarioCreacion)
        {
            if (emprcodi > 0 && grupocodi > 0)
            {
                SiHisempgrupoDataDTO entity = new SiHisempgrupoDataDTO
                {
                    Hgrdatfecha = DateTime.Today,
                    Emprcodi = emprcodi,
                    Grupocodi = grupocodi,
                    Hgrdatestado = ConstantesTitularidad.EstadoRelEmpFechaInicio,
                    Grupocodiold = grupocodi,
                    Grupocodiactual = grupocodi,
                    Hgrdatusucreacion = usuarioCreacion,
                    Hgrdatfeccreacion = DateTime.Now
                };

                DbTransaction tran = null;
                try
                {

                    List<SiHisempgrupoDataDTO> data = FactorySic.GetSiHisempgrupoDataRepository().GetByCriteria().Where(x => x.Emprcodi == emprcodi && x.Grupocodi == grupocodi).ToList();

                    if (data.Count == 0)
                    {
                        IDbConnection conn = FactorySic.GetSiMigracionRepository().BeginConnection();
                        tran = FactorySic.GetSiMigracionRepository().StartTransaction(conn);
                        int maxId = FactorySic.GetSiHisempgrupoDataRepository().GetMaxId();

                        entity.Hgrdatcodi = maxId;
                        FactorySic.GetSiHisempgrupoDataRepository().Save(entity, conn, tran);

                        tran.Commit();
                    }

                }
                catch (Exception ex)
                {
                    if (tran != null)
                        tran.Rollback();
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_HISEMPGRUPO_DATA
        /// </summary>
        public void UpdateSiHisempgrupoData(SiHisempgrupoDataDTO entity)
        {
            try
            {
                FactorySic.GetSiHisempgrupoDataRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_HISEMPGRUPO_DATA
        /// </summary>
        public void DeleteSiHisempgrupoData(int hgrdatcodi, string username)
        {
            try
            {
                FactorySic.GetSiHisempgrupoDataRepository().Delete(hgrdatcodi);
                FactorySic.GetSiHisempgrupoDataRepository().Delete_UpdateAuditoria(hgrdatcodi, username);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_HISEMPGRUPO_DATA
        /// </summary>
        public SiHisempgrupoDataDTO GetByIdSiHisempgrupoData(int hgrdatcodi)
        {
            return FactorySic.GetSiHisempgrupoDataRepository().GetById(hgrdatcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_HISEMPGRUPO_DATA
        /// </summary>
        public List<SiHisempgrupoDataDTO> ListSiHisempgrupoDatas(string grupocodis)
        {
            return FactorySic.GetSiHisempgrupoDataRepository().List(grupocodis).OrderByDescending(X => X.Hgrdatfecha).ToList();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiHisempgrupoData
        /// </summary>
        public List<SiHisempgrupoDataDTO> GetByCriteriaSiHisempgrupoDatas()
        {
            return FactorySic.GetSiHisempgrupoDataRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SI_HISEMPEQ

        /// <summary>
        /// Actualiza un registro de la tabla SI_HISEMPEQ
        /// </summary>
        public void UpdateSiHisempeq(SiHisempeqDTO entity)
        {
            try
            {
                FactorySic.GetSiHisempeqRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_HISEMPEQ
        /// </summary>
        public void DeleteSiHisempeq(int hempeqcodi)
        {
            try
            {
                FactorySic.GetSiHisempeqRepository().Delete(hempeqcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_HISEMPEQ
        /// </summary>
        public SiHisempeqDTO GetByIdSiHisempeq(int hempeqcodi)
        {
            return FactorySic.GetSiHisempeqRepository().GetById(hempeqcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_HISEMPEQ
        /// </summary>
        public List<SiHisempeqDTO> ListSiHisempeqs()
        {
            return FactorySic.GetSiHisempeqRepository().List().Where(X => X.Hempeqdeleted == ConstantesTitularidad.EliminadoLogicoNo).ToList();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiHisempeq
        /// </summary>
        public List<SiHisempeqDTO> GetByCriteriaSiHisempeqs()
        {
            return FactorySic.GetSiHisempeqRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SI_HISEMPPTO

        /// <summary>
        /// Actualiza un registro de la tabla SI_HISEMPPTO
        /// </summary>
        public void UpdateSiHisemppto(SiHisempptoDTO entity)

        {
            try
            {
                FactorySic.GetSiHisempptoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_HISEMPPTO
        /// </summary>
        public void DeleteSiHisemppto(int hempptcodi)
        {
            try
            {
                FactorySic.GetSiHisempptoRepository().Delete(hempptcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_HISEMPPTO
        /// </summary>
        public SiHisempptoDTO GetByIdSiHisemppto(int hempptcodi)
        {
            return FactorySic.GetSiHisempptoRepository().GetById(hempptcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_HISEMPPTO
        /// </summary>
        public List<SiHisempptoDTO> ListSiHisempptos()
        {
            return FactorySic.GetSiHisempptoRepository().List().Where(X => X.Hempptdeleted == ConstantesTitularidad.EliminadoLogicoNo).ToList();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiHisemppto
        /// </summary>
        public List<SiHisempptoDTO> GetByCriteriaSiHisempptos()
        {
            return FactorySic.GetSiHisempptoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SI_HISEMPGRUPO

        /// <summary>
        /// Actualiza un registro de la tabla SI_HISEMPGRUPO
        /// </summary>
        public void UpdateSiHisempgrupo(SiHisempgrupoDTO entity)
        {
            try
            {
                FactorySic.GetSiHisempgrupoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_HISEMPGRUPO
        /// </summary>
        public void DeleteSiHisempgrupo(int hempgrcodi)
        {
            try
            {
                FactorySic.GetSiHisempgrupoRepository().Delete(hempgrcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_HISEMPGRUPO
        /// </summary>
        public SiHisempgrupoDTO GetByIdSiHisempgrupo(int hempgrcodi)
        {
            return FactorySic.GetSiHisempgrupoRepository().GetById(hempgrcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_HISEMPGRUPO
        /// </summary>
        public List<SiHisempgrupoDTO> ListSiHisempgrupos()
        {
            return FactorySic.GetSiHisempgrupoRepository().List().Where(X => X.Hempgrdeleted == ConstantesTitularidad.EliminadoLogicoNo).ToList();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiHisempgrupo
        /// </summary>
        public List<SiHisempgrupoDTO> GetByCriteriaSiHisempgrupos()
        {
            return FactorySic.GetSiHisempgrupoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SI_HISEMPENTIDAD

        /// <summary>
        /// Inserta un registro de la tabla SI_HISEMPENTIDAD
        /// </summary>
        public void SaveSiHisempentidad(SiHisempentidadDTO entity)
        {
            try
            {
                FactorySic.GetSiHisempentidadRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_HISEMPENTIDAD
        /// </summary>
        public void UpdateSiHisempentidad(SiHisempentidadDTO entity)
        {
            try
            {
                FactorySic.GetSiHisempentidadRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_HISEMPENTIDAD
        /// </summary>
        public void DeleteSiHisempentidad(int hempencodi)
        {
            try
            {
                FactorySic.GetSiHisempentidadRepository().Delete(hempencodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_HISEMPENTIDAD
        /// </summary>
        public SiHisempentidadDTO GetByIdSiHisempentidad(int hempencodi)
        {
            return FactorySic.GetSiHisempentidadRepository().GetById(hempencodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_HISEMPENTIDAD
        /// </summary>
        public List<SiHisempentidadDTO> ListSiHisempentidads()
        {
            return FactorySic.GetSiHisempentidadRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiHisempentidad
        /// </summary>
        public List<SiHisempentidadDTO> GetByCriteriaSiHisempentidads(int migracodi)
        {
            return FactorySic.GetSiHisempentidadRepository().GetByCriteria(migracodi);
        }

        #endregion

        #region Métodos Tabla SI_HISEMPENTIDAD_DET

        /// <summary>
        /// Inserta un registro de la tabla SI_HISEMPENTIDAD_DET
        /// </summary>
        public void SaveSiHisempentidadDet(SiHisempentidadDetDTO entity)
        {
            try
            {
                FactorySic.GetSiHisempentidadDetRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_HISEMPENTIDAD_DET
        /// </summary>
        public void UpdateSiHisempentidadDet(SiHisempentidadDetDTO entity)
        {
            try
            {
                FactorySic.GetSiHisempentidadDetRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_HISEMPENTIDAD_DET
        /// </summary>
        public void DeleteSiHisempentidadDet(int hempedcodi)
        {
            try
            {
                FactorySic.GetSiHisempentidadDetRepository().Delete(hempedcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_HISEMPENTIDAD_DET
        /// </summary>
        public SiHisempentidadDetDTO GetByIdSiHisempentidadDet(int hempedcodi)
        {
            return FactorySic.GetSiHisempentidadDetRepository().GetById(hempedcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_HISEMPENTIDAD_DET
        /// </summary>
        public List<SiHisempentidadDetDTO> ListSiHisempentidadDets()
        {
            return FactorySic.GetSiHisempentidadDetRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiHisempentidadDet
        /// </summary>
        public List<SiHisempentidadDetDTO> GetByCriteriaSiHisempentidadDets(int migracodi)
        {
            return FactorySic.GetSiHisempentidadDetRepository().GetByCriteria(migracodi);
        }

        #endregion

        #region Métodos Tabla SI_MIGRALOGDBA

        /// <summary>
        /// Listar querys ejecutdas para el DBA según la migración
        /// </summary>
        /// <param name="migracodi"></param>
        /// <returns></returns>
        public List<SiMigralogdbaDTO> ListarLogDbaByMigracion(int migracodi, int tipomigra)
        {
            var lista = FactorySic.GetSiMigralogdbaRepository().ListLogDbaByMigracion(migracodi);

            return this.ListarLogDbaByMigracionConTotalRegistros(lista, migracodi, tipomigra);
        }

        #endregion

        #region Métodos Tabla SI_MIGRACIONLOG

        /// <summary>
        /// Obtener cantidad de registros de las querys ejecutdas para el DBA según la migración
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="migracodi"></param>
        /// <param name="tipoOperacion"></param>
        /// <returns></returns>
        public List<SiMigralogdbaDTO> ListarLogDbaByMigracionConTotalRegistros(List<SiMigralogdbaDTO> lista, int migracodi, int tipoOperacion)
        {
            var listaQueryOrder = FactorySic.GetSiMigraquerybaseRepository().ListarSiQueryXMigraQueryTipo(tipoOperacion);

            foreach (SiMigralogdbaDTO item in lista)
            {
                int? cantidad = item.Mqxtopcodi == null ? null : FactorySic.GetSiMigracionlogRepository().CantRegistrosMigraQuery(migracodi, item.Mqxtopcodi.Value);
                item.NroRegistros = cantidad;
                item.FechaDesc = item.Migdbafeccreacion != null ? item.Migdbafeccreacion.Value.ToString(ConstantesAppServicio.FormatoFecha) : string.Empty;
                item.HoraDesc = item.Migdbafeccreacion != null ? item.Migdbafeccreacion.Value.ToString(ConstantesAppServicio.FormatoHHmmss) : string.Empty;
            }
            return lista.OrderBy(x => x.Migdbafeccreacion).ToList();
        }

        #endregion

        #region Métodos Tabla SI_MIGRACION

        /// <summary>
        /// Permite obtener un registro de la tabla SI_MIGRACION
        /// </summary>
        public SiMigracionDTO GetByIdSiMigracion(int migracodi)
        {
            var reg = FactorySic.GetSiMigracionRepository().GetById(migracodi);
            reg.MigrafeccorteDesc = TitularidadAppServicio.TieneFechaCorte(reg.Tmopercodi) ? reg.Migrafeccorte.ToString(ConstantesAppServicio.FormatoFecha) : string.Empty;

            return reg;
        }

        /// <summary>
        /// Obtener la migracion original que ha sido anulado con el migrareldeleted
        /// </summary>
        /// <param name="migrareldeleted"></param>
        /// <returns></returns>
        public SiMigracionDTO GetSiMigracionAnuladoByIdDeleted(int migrareldeleted)
        {
            return FactorySic.GetSiMigracionRepository().List().Find(x => x.Migrareldeleted == migrareldeleted);
        }

        /// <summary>
        /// Listar migraciones por filtro de origen, destino
        /// </summary>
        /// <param name="iEmpresaOrigen"></param>
        /// <param name="iEmpresaDestino"></param>
        /// <param name="sDescripcion"></param>
        /// <returns></returns>
        public List<SiMigracionDTO> ListarTransferenciasXEmpresaOrigenXEmpresaDestino(int iEmpresaOrigen, int iEmpresaDestino, string sDescripcion, int estadoAnulado)
        {
            try
            {
                List<SiMigracionDTO> lista = FactorySic.GetSiMigracionRepository().ListarTransferenciasXEmpresaOrigenXEmpresaDestino(iEmpresaOrigen, iEmpresaDestino, sDescripcion);
                List<int> listaDeleted = lista.Where(y => y.Migrareldeleted != null).Select(x => x.Migrareldeleted.Value).ToList();

                lista = lista.Where(x => !listaDeleted.Contains(x.Migracodi)).ToList();

                foreach (var reg in lista)
                {
                    reg.MigrafeccorteDesc = TitularidadAppServicio.TieneFechaCorte(reg.Tmopercodi) ? reg.Migrafeccorte.ToString(ConstantesAppServicio.FormatoFecha) : string.Empty;
                    reg.MigrafeccreacionDesc = reg.Migrafeccreacion != null ? reg.Migrafeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
                    reg.MigrafecmodificacionDesc = reg.Migrafecmodificacion != null ? reg.Migrafecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
                    reg.UltimaModificacionUsuarioDesc = reg.Migrafecmodificacion != null ? reg.Migrausumodificacion : reg.Migrausucreacion;
                    reg.UltimaModificacionFechaDesc = reg.Migrafecmodificacion != null ? reg.MigrafecmodificacionDesc : reg.MigrafeccreacionDesc;
                    reg.TotalDesc = reg.Total > 0 ? reg.Total.ToString() : string.Empty;
                    reg.ColorFila = reg.Migradeleted == ConstantesTitularidad.EliminadoLogicoSi ? "background-color: #A4A4A4;color:#FFFFFF" : string.Empty;
                }

                if (estadoAnulado != -1)
                    lista = lista.Where(x => x.Migradeleted == null || x.Migradeleted == ConstantesTitularidad.EliminadoLogicoNo).ToList();

                return lista.OrderByDescending(x => x.Migracodi).ToList();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        #endregion

        #region Métodos Tabla SI_TIPOMIGRAOPERACION

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_TIPOMIGRAOPERACION
        /// </summary>
        public List<SiTipomigraoperacionDTO> ListSiTipomigraoperacions()
        {
            return FactorySic.GetSiTipomigraoperacionRepository().List();
        }

        /// <summary>
        /// Indica si el tipo de migracion tiene fecha de corte
        /// </summary>
        /// <param name="tmopercodi"></param>
        /// <returns></returns>
        public static bool TieneFechaCorte(int tmopercodi)
        {
            return ConstantesTitularidad.TipoMigrCambioRazonSocial == tmopercodi
                || ConstantesTitularidad.TipoMigrFusion == tmopercodi
                || ConstantesTitularidad.TipoMigrTransferenciaEquipos == tmopercodi
                ;
        }

        /// <summary>
        /// Indica si el tipo de migracion tiene transferencia a nivel de empresas
        /// </summary>
        /// <param name="tmopercodi"></param>
        /// <returns></returns>
        public static bool TieneTransferirEmpresa(int tmopercodi)
        {
            return ConstantesTitularidad.TipoMigrDuplicidad == tmopercodi
                || ConstantesTitularidad.TipoMigrCambioRazonSocial == tmopercodi
                || ConstantesTitularidad.TipoMigrFusion == tmopercodi
                ;
        }

        #endregion

        #endregion

        #region Procesar Transferencia

        /// <summary>
        /// Registrar migración
        /// </summary>
        /// <param name="regDestino"></param>
        /// <param name="regOrigen"></param>
        /// <param name="listaEquipo"></param>
        /// <param name="listaPto"></param>
        /// <param name="listaGrupo"></param>
        public int RegistrarTransferencia(SiMigracionDTO regDestino, SiMigraemprOrigenDTO regOrigen, List<int> listaEquicodis, bool regHistoricoTransf)
        {
            DbTransaction tran = null;
            int migracodi;
            DateTime fechaCreacion = DateTime.Now;
            string usuarioCreacion = regDestino.Migrausucreacion;

            try
            {
                listaEquicodis = listaEquicodis != null ? listaEquicodis : new List<int>();
                List<int> listaPtomedicodis = new List<int>(), listaGrupocodis = new List<int>();

                //Obtener los equipos
                List<EqEquipoDTO> listaEquipo = this.ListarObjEquipoValidosATransferir(regDestino.Tmopercodi, regOrigen.Emprcodi, listaEquicodis);
                listaEquicodis = listaEquipo.Select(x => x.Equicodi).ToList();

                //Obtener los Ptomedicion
                List<MePtomedicionDTO> listaPtomedicion = ListarObjPtomedicodiByListaEquicodi(regDestino.Tmopercodi, regOrigen.Emprcodi, listaEquicodis, new List<int>());
                listaPtomedicodis = listaPtomedicion.Select(x => x.Ptomedicodi).ToList();

                //Obtener los grupos
                List<PrGrupoDTO> listaGrupo = ListarObjGrupoByListaEquicodiAndListaPtomedicodi(regDestino.Tmopercodi, regOrigen.Emprcodi, listaEquicodis, listaPtomedicodis);
                listaGrupocodis = listaGrupo.Select(x => x.Grupocodi).ToList();

                //Obtener la lista definitiva de puntos de medición
                listaPtomedicion = ListarObjPtomedicodiByListaEquicodi(regDestino.Tmopercodi, regOrigen.Emprcodi, listaEquicodis, listaGrupocodis);
                listaPtomedicodis = listaPtomedicion.Select(x => x.Ptomedicodi).ToList();

                //VALIDAR EXISTENCIA MIGRACIÓN DE CAMBIO DE RAZÓN SOCIAL O FUSIÓN
                if (ValidarExistenciaMigracion(regDestino, regOrigen)) throw new Exception("La Empresa origen ya ha sido migrada anteriormente en una Fusión o Cambio de Razón Social");

                //validación a equipos con migraciones anteriores
                if (regDestino.Tmopercodi == ConstantesTitularidad.TipoMigrDuplicidad || regDestino.Tmopercodi == ConstantesTitularidad.TipoMigrInstalacionNoCorresponden)
                {
                    var equipsMigrados = this.ValidarEquiposMigracion(listaEquicodis);
                    string equipsCnMigracion = string.Join(",", equipsMigrados);
                    if (equipsMigrados.Count > 0) throw new Exception("Este tipo de migración está inhablitada para los equipos: " + equipsCnMigracion);
                }

                //validar equipos en la misma fecha con migraciones 
                this.ValidarEquiposFechCorte(listaEquicodis, regDestino);
                //validar puntos en la misma fecha con migraciones 
                this.ValidarPtsFechCorte(listaPtomedicodis, regDestino);
                //validar grupos en la misma fecha con migraciones 
                this.ValidarGrsFechCorte(listaGrupocodis, regDestino);

                var listEquipsExisten = this.ValidarRegistroEquips(regDestino, listaEquicodis);
                var lisPtosExisten = this.ValidarRegistroPtos(regDestino, listaPtomedicodis);
                var listGrupsExisten = this.ValidarRegistroGrupos(regDestino, listaGrupocodis);

                string cadenaEquips = string.Join(",", listEquipsExisten);
                string cadenaPtos = string.Join(",", lisPtosExisten);
                string cadenaGrups = string.Join(",", listGrupsExisten);

                if (listEquipsExisten.Count > 0) throw new Exception("Ya existen estos equipos registrados" + cadenaEquips);
                if (lisPtosExisten.Count > 0) throw new Exception("Ya existen estos puntos registrados" + cadenaPtos);
                if (listGrupsExisten.Count > 0) throw new Exception("Ya existen estos grupos registrados" + cadenaGrups);

                //
                //Determinar las fechas de inicio para data que no tiene emprcodi
                List<TTIEEntidad> listaEqFechaInicial = ListarEqFechaInicial(listaEquicodis, regDestino.Migrafeccorte);
                List<TTIEEntidad> listaPtoFechaInicial = ListarPtoFechaInicial(listaPtomedicodis, regDestino.Migrafeccorte);
                List<TTIEEntidad> listaGrupoFechaInicial = ListarGrupoFechaInicial(listaGrupocodis, regDestino.Migrafeccorte);

                //
                IDbConnection conn = FactorySic.GetSiMigracionRepository().BeginConnection();
                tran = FactorySic.GetSiMigracionRepository().StartTransaction(conn);

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///Registrar Empresa origen y empresa destino
                regDestino.Migrafeccreacion = fechaCreacion;
                regDestino.Migradeleted = 0;
                migracodi = FactorySic.GetSiMigracionRepository().Save(regDestino, conn, tran);

                SiEmpresaDTO objEmpresaOrig = FactorySic.GetSiEmpresaRepository().GetById(regOrigen.Emprcodi);
                SiEmpresaDTO objEmpresaDet = FactorySic.GetSiEmpresaRepository().GetById(regDestino.Emprcodi);

                regOrigen.Migracodi = migracodi;
                regOrigen.Migempfeccreacion = fechaCreacion;
                regOrigen.Migempusumodificacion = regDestino.Migrausucreacion;
                regOrigen.Migempfecmodificacion = fechaCreacion;
                regOrigen.MigempestadoDest = objEmpresaOrig.Emprestado;

                regOrigen.MigempcodosinergminDest = objEmpresaDet.EmprCodOsinergmin;
                regOrigen.MigempscadacodiDest = objEmpresaDet.Scadacodi;
                regOrigen.MigempnombrecomercialDest = objEmpresaDet.Emprnombcomercial;
                regOrigen.MigempdomiciliolegalDest = objEmpresaDet.Emprdomiciliolegal;
                regOrigen.MigempnumpartidaregDest = objEmpresaDet.Emprnumpartidareg;
                regOrigen.MigempabrevDest = objEmpresaDet.Emprabrev;
                regOrigen.MigempordenDest = objEmpresaDet.Emprorden;
                regOrigen.MigemptelefonoDest = objEmpresaDet.Emprtelefono;
                regOrigen.MigempestadoregistroDest = objEmpresaDet.Emprestadoregistro;
                regOrigen.MigempfecinscripcionDest = objEmpresaDet.Emprfechainscripcion;
                regOrigen.MigempcondicionDest = objEmpresaDet.Emprcondicion;
                regOrigen.MigempnroconstanciaDest = objEmpresaDet.Emprnroconstancia;
                regOrigen.MigempfecingresoDest = objEmpresaDet.Emprfecingreso;
                regOrigen.MigempnroregistroDest = objEmpresaDet.Emprnroregistro;
                regOrigen.MigempindusutramiteDest = objEmpresaDet.Emprindusutramite;

                FactorySic.GetSiMigraemprorigenRepository().Save(regOrigen, conn, tran);

                ///
                int firstCorrSalida = 0;
                if (!regHistoricoTransf)
                {
                    int corrSiLogHInicio = FactorySic.GetSiLogRepository().GetMaxId();
                    GrabarLogFecha(ObtenerHoraInicioSistema(), migracodi, regOrigen.Migempusucreacion, conn, tran, corrSiLogHInicio, ConstantesTitularidad.TipoEventoLogInicio, null);
                }

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///Registrar los equipos, puntos de medicion y grupos
                if (listaEquicodis.Count > 0)
                {
                    var listaheqps = this.RegistrarMigracionHEquipos(regDestino, regOrigen, listaEquipo, migracodi, conn, tran);
                    if (regDestino.Tmopercodi != ConstantesTitularidad.TipoMigrDuplicidad && regDestino.Tmopercodi != ConstantesTitularidad.TipoMigrInstalacionNoCorresponden)
                        this.ProcesarDataHistoricaEquips(regOrigen.Emprcodi, migracodi, usuarioCreacion, fechaCreacion, regDestino.Migrafeccorte, conn, tran, listaheqps);
                }
                if (listaPtomedicodis.Count > 0)
                {
                    var listahpts = this.RegistrarMigracionHPtos(regDestino, regOrigen, listaPtomedicion, migracodi, conn, tran);
                    if (regDestino.Tmopercodi != ConstantesTitularidad.TipoMigrDuplicidad && regDestino.Tmopercodi != ConstantesTitularidad.TipoMigrInstalacionNoCorresponden)
                        this.ProcesarDataHistoricaPtos(regOrigen.Emprcodi, migracodi, usuarioCreacion, fechaCreacion, regDestino.Migrafeccorte, conn, tran, listahpts);
                }
                if (listaGrupo.Count > 0)
                {
                    var listahgrs = this.RegistrarMigracionHGrupos(regDestino, regOrigen, listaGrupo, migracodi, conn, tran);
                    if (regDestino.Tmopercodi != ConstantesTitularidad.TipoMigrDuplicidad && regDestino.Tmopercodi != ConstantesTitularidad.TipoMigrInstalacionNoCorresponden)
                        this.ProcesarDataHistoricaGrupos(regOrigen.Emprcodi, migracodi, usuarioCreacion, fechaCreacion, regDestino.Migrafeccorte, conn, tran, listahgrs);
                }

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///Procesar las querys
                if (!regHistoricoTransf)
                {
                    TTIEParametro objTTIE = new TTIEParametro()
                    {
                        MigraCodi = migracodi,
                        TipoOperacion = regDestino.Tmopercodi,
                        IdEmpresaOrigen = regOrigen.Emprcodi,
                        IdEmpresaDestino = regDestino.Emprcodi,
                        Migraflagstr = regDestino.Migraflagstr,
                        User = regOrigen.Migempusucreacion,
                        Migradescripcion = regDestino.Migradescripcion,
                        Ptos = listaPtomedicodis,
                        Equips = listaEquicodis,
                        Grups = listaGrupocodis,
                        Feccorte = regDestino.Migrafeccorte,
                        FlagAnulacion = 0,
                        ListaEqFechaInicial = listaEqFechaInicial,
                        ListaPtoFechaInicial = listaPtoFechaInicial,
                        ListaGrupoFechaInicial = listaGrupoFechaInicial
                    };
                    List<String> mensajeLog = this.ProcesarQuerys(objTTIE, out firstCorrSalida, conn, tran);

                    int corrSiLogHFin = firstCorrSalida;
                    GrabarLogFecha(ObtenerHoraFinSistema(), migracodi, regOrigen.Migempusucreacion, conn, tran, corrSiLogHFin, ConstantesTitularidad.TipoEventoLogFin, null);
                }
                ///

                tran.Commit();

            }
            catch (Exception ex)
            {
                if (tran != null)
                    tran.Rollback();
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }

            return migracodi;
        }

        /// <summary>
        /// Generar html del resumen de la transferencia a procesar
        /// </summary>
        /// <param name="regDestino"></param>
        /// <param name="regOrigen"></param>
        /// <param name="listaEquipo"></param>
        /// <returns></returns>
        public string GenerarViewProcesarHtml(SiMigracionDTO regDestino, SiMigraemprOrigenDTO regOrigen, List<int> listaEquicodi)
        {
            SiTipomigraoperacionDTO regTipoOp = this.ListSiTipomigraoperacions().Find(x => x.Tmopercodi == regDestino.Tmopercodi);
            SiEmpresaDTO regEmpOrig = this.GetSiEmpresaById(regOrigen.Emprcodi);
            SiEmpresaDTO regEmpDest = this.GetSiEmpresaById(regDestino.Emprcodi);

            string html = "";
            html += $@"
                <p>
                    La operación <b>{regTipoOp.Tmoperdescripcion}</b> desde la empresa <b style='color:#43a243'>{regEmpOrig.Emprnomb}</b> hacia la empresa <b style='color:#fd4444'>{regEmpDest.Emprnomb}</b>
            ";
            if (TieneFechaCorte(regDestino.Tmopercodi))
                html += $@"  con Fecha de transferencia <b>{regDestino.Migrafeccorte.Date.ToString(ConstantesAppServicio.FormatoFechaHoraAMPM)}</b>";
            html += $@" procesará toda la información asociada a los equipos seleccionados.
                </p>
            ";

            if (listaEquicodi.Count > 0)
            {
                List<EqEquipoDTO> listaEquipo = ListarEquiposXEmpresa(regOrigen.Emprcodi);
                listaEquipo = listaEquipo.Where(x => listaEquicodi.Contains(x.Equicodi)).ToList();

                List<EqFamiliaDTO> listaFamilia = listaEquipo.GroupBy(x => new { x.Famcodi, x.Famnomb })
                    .Select(x => new EqFamiliaDTO() { Famcodi = x.Key.Famcodi ?? -1, Famnomb = x.Key.Famnomb, TotalXFamcodi = x.Count() })
                    .OrderBy(x => x.Famnomb).ToList();

                html += $@"
                    <p> Los equipos seleccionados ({listaEquicodi.Count}) se clasifican de la siguiente forma:
                    <div style='width: 350px;'>
                        <table id='tabla_familias' class='pretty tabla-icono' style='width: 350px;'>
                            <thead>
                                <tr>
                                    <th style='width:210px;'>Tipo de Equipo</th>
                                    <th style='width:70px;'>Total</th>
                                </tr>
                            </thead>
                            <tbody>
                ";

                foreach (var reg in listaFamilia)
                {
                    html += $@"
                                <tr>
                                    <td>{reg.Famnomb}</td>
                                    <td>{reg.TotalXFamcodi}</td>
                                </tr>
                    ";
                }

                html += $@"
                            </tbody>
                        </table>
                    </div>
                ";
            }
            //ASSETEC 202108 TIEE
            if (TieneTransferirEmpresa(regDestino.Tmopercodi))
                html += $@"
                <p style='margin-top: 30px; margin-bottom: 40px;'>
                    <span>Seleccione el aplicativo que quiera incluir en esta migración:</span>
                    <br/>
                    <input type='checkbox' id='check_str'disabled checked style='margin-top: 5px;'>
                    <span style='display: inline-table; '>STR</span>
                    <br/><!-- Proceso para Mercados -->
                    <input type='checkbox' id='check_pm' checked style='margin-top: 5px;'>
                    <span style='display: inline-table; '>Proceso para Mercados</span>
                </p>
            ";

            html += $@"
                <p>Presione el botón <b>Procesar</b> para realizar la transferencia.
                </p>
            ";

            return html;
        }

        /// <summary>
        /// Procesar todas las Querys base asociadas al Tipo de operación
        /// </summary>
        /// <param name="migraCodi"></param>
        /// <param name="tipoOperacion"></param>
        /// <param name="idEmpresaOrigen"></param>
        /// <param name="idEmpresaDestino"></param>
        /// <param name="user"></param>
        /// <param name="firstCorrSalida"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <param name="ptos"></param>
        /// <param name="equips"></param>
        /// <param name="grups"></param>
        /// <param name="feccorte"></param>
        /// <returns></returns>
        public List<String> ProcesarQuerys(TTIEParametro objTTIE, out int firstCorrSalida, IDbConnection conn, DbTransaction tran)
        {
            int corrSiMigracionLog = FactorySic.GetSiMigracionlogRepository().GetMaxId();
            int corrSiLog = FactorySic.GetSiLogRepository().GetMaxId() + 1;
            int corrSiLogDba = FactorySic.GetSiMigralogdbaRepository().GetMaxId();

            string mensajeErrorIDTransferencia = string.Empty;
            string mensajeErrorSalidaTemp = string.Empty;
            //string mensajeOkTransferencia = "Se procesó correctamente la operación '" + idEmpresaOrigen + "' hacia la empresa destino con código '" + idEmpresaDestino + "'";
            List<String> logs = new List<String>();

            //Lista de querys por Tipo de Operación (ACTIVOS)
            List<SiMigraquerybaseDTO> listaQueryOrder = FactorySic.GetSiMigraquerybaseRepository().ListarSiQueryXMigraQueryTipo(objTTIE.TipoOperacion);

            #region Filtrar por aplicativo STR
            //validación Aplicativos SRT
            //0
            if (objTTIE.Migraflagstr == ConstantesTitularidad.FlagProcesoStrNo) listaQueryOrder = listaQueryOrder.Where(x => x.Miqubastr == ConstantesAppServicio.NO).ToList();
            //1
            //2
            if (objTTIE.Migraflagstr == ConstantesTitularidad.FlagProcesoStr) listaQueryOrder = listaQueryOrder.Where(x => x.Miqubastr == ConstantesAppServicio.SI).ToList();
            #endregion

            //Parámetros de querys por Tipo de Operación (ACTIVOS)
            List<SiMigraParametroDTO> listaParametrosPorQuery = FactorySic.GetSiMigraparametroRepository().ObtenerByTipoOperacion(objTTIE.TipoOperacion);

            #region setear valores a los parámetros de TIEMPO DE EJECUCION

            ActualizarParametro(ref listaParametrosPorQuery, CodigosParametro.EmpresaOrigen, objTTIE.IdEmpresaOrigen.ToString());
            ActualizarParametro(ref listaParametrosPorQuery, CodigosParametro.Migracodi, objTTIE.MigraCodi.ToString());
            ActualizarParametro(ref listaParametrosPorQuery, CodigosParametro.EmpresaDestino, objTTIE.IdEmpresaDestino.ToString());

            //Actualiza ANULACIÓN
            ActualizarParametro(ref listaParametrosPorQuery, CodigosParametro.Anulacion, objTTIE.FlagAnulacion.ToString());

            var listTablaSTR = listaQueryOrder.Where(x => x.Miqubastr == ConstantesAppServicio.SI).Select(x => x.Miqubacodi).ToList();
            //Actualiza FECHA CORTE
            ActualizarParametro(ref listaParametrosPorQuery, CodigosParametro.Fechacorte, objTTIE.Feccorte.ToString(ConstantesAppServicio.FormatoFecha));
            //ActualizarParametroSTR(ref listaParametrosPorQuery, CodigosParametro.Fechacorte, objTTIE.Feccorte.ToString(ConstantesAppServicio.FormatoFecha), listTablaSTR, objTTIE.FlagAnulacion, objTTIE.FeccorteSTR.ToString(ConstantesAppServicio.FormatoFecha));

            //Actualizar tIPOMIGRACION 
            ActualizarParametro(ref listaParametrosPorQuery, CodigosParametro.Tmopercodi, objTTIE.TipoOperacion.ToString());
            //Actualizar usuario 
            ActualizarParametro(ref listaParametrosPorQuery, CodigosParametro.Usuariocreacion, objTTIE.User);
            //Actualizar descripcion 
            ActualizarParametro(ref listaParametrosPorQuery, CodigosParametro.Migradescripcion, objTTIE.Migradescripcion);
            //Actualiza FECHA INICIAL (ES solo referencial)
            ActualizarParametro(ref listaParametrosPorQuery, CodigosParametro.Fechainicial, objTTIE.Feccorte.ToString(ConstantesAppServicio.FormatoFecha));

            #endregion

            List<int> lMiqubacodiFiltro = new List<int>();

            #region Ejecutar querys para actualizar data con fecha de corte hacia atrás

            if (objTTIE.ListaEqFechaInicial.Any())
            {
                lMiqubacodiFiltro = ListarQuerybaseXListaParam(listaQueryOrder, listaParametrosPorQuery, new List<int>() { CodigosParametro.Equicodi, CodigosParametro.Fechainicial });

                List<DateTime> lFechaIni = objTTIE.ListaEqFechaInicial.Select(x => x.FechaInicial).Distinct().OrderBy(x => x).ToList();
                foreach (var fechaIni in lFechaIni)
                {
                    List<List<int>> listaequips = ListaSeparada(objTTIE.ListaEqFechaInicial.Where(x => x.FechaInicial == fechaIni).Select(x => x.Codigo).ToList());
                    foreach (var eqs in listaequips)
                    {
                        string cadenaEquips = string.Join(",", eqs);
                        ActualizarParametro(ref listaParametrosPorQuery, CodigosParametro.Equicodi, cadenaEquips);
                        ActualizarParametro(ref listaParametrosPorQuery, CodigosParametro.Fechainicial, fechaIni.ToString(ConstantesAppServicio.FormatoFecha));

                        //obtener las entidades que usan Equicodi
                        var listaQtmp = listaQueryOrder.Where(x => lMiqubacodiFiltro.Contains(x.Miqubacodi)).ToList();

                        //recorre las entidades anteriores actualizando sus parámeros y ejecuta sus querys
                        ActualizarParamYEjecQuerys(listaQtmp, ref listaParametrosPorQuery, objTTIE.MigraCodi, objTTIE.User, ref corrSiLogDba, ref corrSiLog, ref logs, conn, tran);
                    }
                }

                //remover las tablas ingresadas en los procesos anteriores
                listaQueryOrder = listaQueryOrder.Where(x => !lMiqubacodiFiltro.Contains(x.Miqubacodi)).ToList();
                listaParametrosPorQuery = listaParametrosPorQuery.Where(x => !lMiqubacodiFiltro.Contains(x.Miqubacodi)).ToList();
            }

            if (objTTIE.ListaPtoFechaInicial.Any())
            {
                lMiqubacodiFiltro = ListarQuerybaseXListaParam(listaQueryOrder, listaParametrosPorQuery, new List<int>() { CodigosParametro.Ptomedicodi, CodigosParametro.Fechainicial });

                List<DateTime> lFechaIni = objTTIE.ListaPtoFechaInicial.Select(x => x.FechaInicial).Distinct().OrderBy(x => x).ToList();
                foreach (var fechaIni in lFechaIni)
                {
                    List<List<int>> listaptos = ListaSeparada(objTTIE.ListaPtoFechaInicial.Where(x => x.FechaInicial == fechaIni).Select(x => x.Codigo).ToList());
                    foreach (var pts in listaptos)
                    {
                        string cadenaPtos = string.Join(",", pts);
                        ActualizarParametro(ref listaParametrosPorQuery, CodigosParametro.Ptomedicodi, cadenaPtos);
                        ActualizarParametro(ref listaParametrosPorQuery, CodigosParametro.Fechainicial, fechaIni.ToString(ConstantesAppServicio.FormatoFecha));

                        //obtener las entidades que usan Ptomedicodi
                        var listaQtmp = listaQueryOrder.Where(x => lMiqubacodiFiltro.Contains(x.Miqubacodi)).ToList();

                        //recorre las entidades anteriores actualizando sus parámeros y ejecuta sus querys
                        ActualizarParamYEjecQuerys(listaQtmp, ref listaParametrosPorQuery, objTTIE.MigraCodi, objTTIE.User, ref corrSiLogDba, ref corrSiLog, ref logs, conn, tran);
                    }
                }

                //remover las tablas ingresadas en los procesos anteriores
                listaQueryOrder = listaQueryOrder.Where(x => !lMiqubacodiFiltro.Contains(x.Miqubacodi)).ToList();
                listaParametrosPorQuery = listaParametrosPorQuery.Where(x => !lMiqubacodiFiltro.Contains(x.Miqubacodi)).ToList();
            }

            if (objTTIE.ListaGrupoFechaInicial.Any())
            {
                lMiqubacodiFiltro = ListarQuerybaseXListaParam(listaQueryOrder, listaParametrosPorQuery, new List<int>() { CodigosParametro.Grupocodi, CodigosParametro.Fechainicial });

                List<DateTime> lFechaIni = objTTIE.ListaGrupoFechaInicial.Select(x => x.FechaInicial).Distinct().OrderBy(x => x).ToList();
                foreach (var fechaIni in lFechaIni)
                {
                    List<List<int>> listagrups = ListaSeparada(objTTIE.ListaGrupoFechaInicial.Where(x => x.FechaInicial == fechaIni).Select(x => x.Codigo).ToList());
                    foreach (var grs in listagrups)
                    {
                        string cadenaGrups = string.Join(",", grs);
                        ActualizarParametro(ref listaParametrosPorQuery, CodigosParametro.Grupocodi, cadenaGrups);
                        ActualizarParametro(ref listaParametrosPorQuery, CodigosParametro.Fechainicial, fechaIni.ToString(ConstantesAppServicio.FormatoFecha));

                        //obtener las entidades que usan Grupocodi 
                        var listaQtmp = listaQueryOrder.Where(x => lMiqubacodiFiltro.Contains(x.Miqubacodi)).ToList();

                        //recorre las entidades anteriores actualizando sus parámeros y ejecuta sus querys
                        ActualizarParamYEjecQuerys(listaQtmp, ref listaParametrosPorQuery, objTTIE.MigraCodi, objTTIE.User, ref corrSiLogDba, ref corrSiLog, ref logs, conn, tran);
                    }
                }

                //remover las tablas ingresadas en los procesos anteriores
                listaQueryOrder = listaQueryOrder.Where(x => !lMiqubacodiFiltro.Contains(x.Miqubacodi)).ToList();
                listaParametrosPorQuery = listaParametrosPorQuery.Where(x => !lMiqubacodiFiltro.Contains(x.Miqubacodi)).ToList();
            }

            #endregion

            #region Ejecutar querys que tienen equipos, puntos o grupos

            if (objTTIE.Equips.Count > 0)
            {
                lMiqubacodiFiltro = ListarQuerybaseXListaParam(listaQueryOrder, listaParametrosPorQuery, new List<int>() { CodigosParametro.Equicodi });

                List<List<int>> listaequips = ListaSeparada(objTTIE.Equips);
                foreach (var eqs in listaequips)
                {
                    string cadenaEquips = string.Join(",", eqs);
                    ActualizarParametro(ref listaParametrosPorQuery, CodigosParametro.Equicodi, cadenaEquips);

                    //obtener las querys que usan Equicodi
                    var listaQtmp = listaQueryOrder.Where(x => lMiqubacodiFiltro.Contains(x.Miqubacodi)).ToList();

                    //recorre las entidades anteriores actualizando sus parámeros y ejecuta sus querys
                    ActualizarParamYEjecQuerys(listaQtmp, ref listaParametrosPorQuery, objTTIE.MigraCodi, objTTIE.User, ref corrSiLogDba, ref corrSiLog, ref logs, conn, tran);
                }

                //remover las tablas ingresadas en los procesos anteriores
                listaQueryOrder = listaQueryOrder.Where(x => !lMiqubacodiFiltro.Contains(x.Miqubacodi)).ToList();
                listaParametrosPorQuery = listaParametrosPorQuery.Where(x => !lMiqubacodiFiltro.Contains(x.Miqubacodi)).ToList();
            }

            if (objTTIE.Ptos.Count > 0)
            {
                lMiqubacodiFiltro = ListarQuerybaseXListaParam(listaQueryOrder, listaParametrosPorQuery, new List<int>() { CodigosParametro.Ptomedicodi });

                List<List<int>> listaptos = ListaSeparada(objTTIE.Ptos);
                foreach (var pts in listaptos)
                {
                    string cadenaPtos = string.Join(",", pts);
                    ActualizarParametro(ref listaParametrosPorQuery, CodigosParametro.Ptomedicodi, cadenaPtos);

                    //obtener las entidades que usan Ptomedicodi
                    var listaQtmp = listaQueryOrder.Where(x => lMiqubacodiFiltro.Contains(x.Miqubacodi)).ToList();

                    //recorre las entidades anteriores actualizando sus parámeros y ejecuta sus querys
                    ActualizarParamYEjecQuerys(listaQtmp, ref listaParametrosPorQuery, objTTIE.MigraCodi, objTTIE.User, ref corrSiLogDba, ref corrSiLog, ref logs, conn, tran);
                }

                //remover las tablas ingresadas en los procesos anteriores
                listaQueryOrder = listaQueryOrder.Where(x => !lMiqubacodiFiltro.Contains(x.Miqubacodi)).ToList();
                listaParametrosPorQuery = listaParametrosPorQuery.Where(x => !lMiqubacodiFiltro.Contains(x.Miqubacodi)).ToList();
            }

            if (objTTIE.Grups.Count > 0)
            {
                lMiqubacodiFiltro = ListarQuerybaseXListaParam(listaQueryOrder, listaParametrosPorQuery, new List<int>() { CodigosParametro.Grupocodi });

                List<List<int>> listagrups = ListaSeparada(objTTIE.Grups);
                foreach (var grs in listagrups)
                {
                    string cadenaGrups = string.Join(",", grs);
                    ActualizarParametro(ref listaParametrosPorQuery, CodigosParametro.Grupocodi, cadenaGrups);

                    //obtener las entidades que usan Grupocodi 
                    var listaQtmp = listaQueryOrder.Where(x => lMiqubacodiFiltro.Contains(x.Miqubacodi)).ToList();

                    //recorre las entidades anteriores actualizando sus parámeros y ejecuta sus querys
                    ActualizarParamYEjecQuerys(listaQtmp, ref listaParametrosPorQuery, objTTIE.MigraCodi, objTTIE.User, ref corrSiLogDba, ref corrSiLog, ref logs, conn, tran);
                }

                //remover las tablas ingresadas en los procesos anteriores
                listaQueryOrder = listaQueryOrder.Where(x => !lMiqubacodiFiltro.Contains(x.Miqubacodi)).ToList();
                listaParametrosPorQuery = listaParametrosPorQuery.Where(x => !lMiqubacodiFiltro.Contains(x.Miqubacodi)).ToList();
            }

            #endregion

            #region Ejecutar querys que no tienen equipos, grupos o puntos de medición

            foreach (SiMigraquerybaseDTO item in listaQueryOrder)
            {
                ActualizarParametro(ref listaParametrosPorQuery, CodigosParametro.Mqxtopcodi, item.Moxtopcodi.ToString());
                List<SiMigraParametroDTO> listaqueryParametros = listaParametrosPorQuery.Where(x => x.Miqubacodi == item.Miqubacodi).ToList();
                Ejecutarquerys(listaqueryParametros, item, objTTIE.MigraCodi, objTTIE.User, conn, tran, ref corrSiLogDba, ref corrSiLog, ref logs);
            }

            #endregion

            corrSiLog += 1;
            firstCorrSalida = corrSiLog;
            return logs;
        }

        /// <summary>
        /// ejecutar querys si usan los parametros, equicodi, ptomedicodi o grupocodi
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="listaParametrosPorQuery"></param>
        /// <param name="listaQueryOrder"></param>
        /// <param name="migraCodi"></param>
        /// <param name="user"></param>
        /// <param name="corrSiLogDba"></param>
        /// <param name="corrSiLog"></param>
        /// <param name="logs"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <param name="listRemover"></param>
        private void ActualizarParamYEjecQuerys(List<SiMigraquerybaseDTO> listaQueryOrder, ref List<SiMigraParametroDTO> listaParametrosPorQuery
                                , int migraCodi, string user, ref int corrSiLogDba, ref int corrSiLog, ref List<String> logs
                                , IDbConnection conn, DbTransaction tran)
        {
            foreach (var entidadbase in listaQueryOrder)
            {
                ActualizarParametro(ref listaParametrosPorQuery, CodigosParametro.Mqxtopcodi, entidadbase.Moxtopcodi.ToString());
                List<SiMigraParametroDTO> listaqueryParametros = listaParametrosPorQuery.Where(x => x.Miqubacodi == entidadbase.Miqubacodi).ToList();

                Ejecutarquerys(listaqueryParametros, entidadbase, migraCodi, user, conn, tran, ref corrSiLogDba, ref corrSiLog, ref logs);
            }
        }

        /// <summary>
        /// Ejecución de la Query Base
        /// </summary>
        /// <param name="listaqueryParametros"></param>
        /// <param name="item"></param>
        /// <param name="migraCodi"></param>
        /// <param name="user"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <param name="corrSiLogDba"></param>
        /// <param name="corrSiLog"></param>
        /// <param name="logs"></param>
        private void Ejecutarquerys(List<SiMigraParametroDTO> listaqueryParametros, SiMigraquerybaseDTO item, int migraCodi, string user, IDbConnection conn, DbTransaction tran, ref int corrSiLogDba, ref int corrSiLog, ref List<string> logs)
        {
            string mensaje = string.Empty;
            string mensajeCatch = string.Empty;
            string mensajeTemp = item.Miqubamensaje;

            string query = this.ObtenerQuery(listaqueryParametros, item.Miqubaquery);
            try
            {
                if (query == null)
                {
                    string mensajeErrorInicializado = "No existen datos para insertar en la tabla: '" + item.Miqubanomtabla + "'";
                    string horaIniciox = String.Format("[ {0}", DateTime.Now);
                    mensajeCatch = horaIniciox + " ] " + mensajeErrorInicializado;
                    logs.Add(mensajeCatch);
                    GrabarLogFecha(mensajeCatch, migraCodi, user, conn, tran, corrSiLog, ConstantesTitularidad.TipoEventoLogCorrecto, item.Miqubacodi);
                    corrSiLog = corrSiLog + 1;
                }
                else
                {
                    string horaInicio = String.Format("[ {0}", DateTime.Now);
                    mensaje = mensaje + horaInicio;
                    FactorySic.GetSiMigracionlogRepository().EjecutarQuery(query, conn, tran);

                    //
                    string horaFin = String.Format(" - : {0}", DateTime.Now);
                    mensaje = mensaje + " ] " + mensajeTemp;
                    GrabarLogDba(query, migraCodi, item.Moxtopcodi, user, conn, tran, corrSiLogDba);
                    corrSiLogDba += 1;
                    logs.Add(mensaje);

                    GrabarLogFecha(mensaje, migraCodi, user, conn, tran, corrSiLog, ConstantesTitularidad.TipoEventoLogCorrecto, item.Miqubacodi);
                    corrSiLog += 1;

                }
            }
            catch (Exception ex)
            {
                try
                {
                    string mensajeErrorInicializado = "No se ha procesado correctamente la siguiente tabla: '" + item.Miqubanomtabla + "'";
                    string horaIniciox = String.Format("[ {0}", DateTime.Now);
                    mensajeCatch = mensajeCatch + horaIniciox;
                    mensajeCatch = mensajeCatch + " ] " + mensajeErrorInicializado;
                    this.GrabarLogQueryDba(query, ex.ToString(), migraCodi, item.Moxtopcodi, user, conn, tran, corrSiLogDba);
                    corrSiLogDba += 1;
                    logs.Add(mensajeCatch);
                    GrabarLogFecha(mensajeCatch, migraCodi, user, conn, tran, corrSiLog, ConstantesTitularidad.TipoEventoLogError, item.Miqubacodi);
                    corrSiLog = corrSiLog + 1;
                }
                catch (Exception exlog)
                {
                    Logger.Error("TIEE ERROR: " + exlog.Message);
                }
            }
        }

        /// <summary>
        /// Obtener query valida
        /// </summary>
        /// <param name="listaQuerysParametros"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private string ObtenerQuery(List<SiMigraParametroDTO> listaQuerysParametros, string query)
        {
            string sql = query;
            foreach (SiMigraParametroDTO item in listaQuerysParametros)
            {
                sql = sql.Replace(item.Migparnomb, item.ValorParametro);
                //validar si hay un valor
                if (item.ValorParametro == null)
                {
                    sql = null;
                    return item.ValorParametro;
                }
            }
            return sql;
        }

        /// <summary>
        /// Permite guardar los registros de la query base cuando se ejecuta correctamente
        /// </summary>
        /// <param name="queryUpdate"></param>
        /// <param name="migraCodi"></param>
        /// <param name="user"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <param name="corrSilogDba"></param>
        private void GrabarLogDba(string queryUpdate, int migraCodi, int moxtopcodi, string user, IDbConnection conn, DbTransaction tran, int corrSilogDba)
        {
            try
            {
                SiMigralogdbaDTO entidad = new SiMigralogdbaDTO();
                entidad.Migdbacodi = corrSilogDba;
                entidad.Migracodi = migraCodi;
                entidad.Migdbaquery = queryUpdate;
                entidad.Migdbalogquery = string.Empty;
                entidad.Migdbausucreacion = user;
                entidad.Migdbafeccreacion = DateTime.Now;
                entidad.Mqxtopcodi = moxtopcodi;

                //int idLogDba= FactorySic.GetSiMigralogdbaRepository().Save(entidad,conn,tran);
                int idLogDba = FactorySic.GetSiMigralogdbaRepository().SaveTransferencia(entidad, conn, tran);

                if (idLogDba == -1)
                {
                    Logger.Error("Error insertando registro en tabla SiMigracionLogdba!...");
                    throw new Exception("Error insertando registro en tabla temporal SiMigracionLogdba...");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Permite guardar los registros de hora inicial y final de cada migracion
        /// </summary>
        /// <param name="hora"></param>
        /// <param name="idMigracion"></param>
        /// <returns></returns>
        private void GrabarLogFecha(string hora, int idMigracion, string user, IDbConnection conn, DbTransaction tran, int corrSiLog, int tipoEventoLog, int? miqubacodi)
        {
            string mensajeHora = string.Empty;
            string horaTemp = string.Empty;
            mensajeHora = hora;
            try
            {
                SiLogDTO entityLog = new SiLogDTO();
                entityLog.LogCodi = corrSiLog;
                entityLog.ModCodi = ConstantesTitularidad.ModcodiTitularidad;
                entityLog.LogDesc = mensajeHora;
                entityLog.LogUser = user;
                entityLog.LogFecha = DateTime.Now;
                int idLog = FactorySic.GetSiLogRepository().SaveTransferencia(entityLog, conn, tran);

                if (idLog == -1)
                {
                    Logger.Error("Error insertando registro en tabla Log!...");
                    throw new Exception("Error insertando registro en tabla temporal Log...");
                }
                SiLogmigraDTO entidadLog = new SiLogmigraDTO
                {
                    Logcodi = idLog,
                    Migracodi = idMigracion,
                    Logmigusucreacion = user,
                    Logmigfeccreacion = DateTime.Now,
                    Logmigtipo = tipoEventoLog,
                    Miqubacodi = miqubacodi
                };
                FactorySic.GetSiLogmigraRepository().Save(entidadLog, conn, tran);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Permite guardar los registros de la query base cuando existe una excepcion
        /// </summary>
        /// <param name="queryUpdate"></param>
        /// <param name="mensajeExcepcion"></param>
        /// <param name="migraCodi"></param>
        /// <param name="user"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <param name="corrSilogDba"></param>
        private void GrabarLogQueryDba(string queryUpdate, string mensajeExcepcion, int migraCodi, int moxtopcodi, string user, IDbConnection conn, DbTransaction tran, int corrSilogDba)
        {
            try
            {
                SiMigralogdbaDTO entidad = new SiMigralogdbaDTO();
                entidad.Migdbacodi = corrSilogDba;
                entidad.Migracodi = migraCodi;
                entidad.Migdbaquery = queryUpdate;
                entidad.Migdbalogquery = mensajeExcepcion;
                entidad.Migdbausucreacion = user;
                entidad.Migdbafeccreacion = DateTime.Now;
                entidad.Mqxtopcodi = moxtopcodi;

                //int idLogDba= FactorySic.GetSiMigralogdbaRepository().Save(entidad,conn,tran);
                int idLogDba = FactorySic.GetSiMigralogdbaRepository().SaveTransferencia(entidad, conn, tran);

                if (idLogDba == -1)
                {
                    Logger.Error("Error insertando registro en tabla SiMigracionLogdba!...");
                    throw new Exception("Error insertando registro en tabla temporal SiMigracionLogdba...");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Obtener Hora Inicio Sistema
        /// </summary>
        /// <returns></returns>
        private string ObtenerHoraInicioSistema()
        {
            string horaIS = String.Format("Hora de Inicio del Proceso: {0}", DateTime.Now);
            return horaIS;
        }

        /// <summary>
        /// Obtener Hora Fin Sistema
        /// </summary>
        /// <returns></returns>
        private string ObtenerHoraFinSistema()
        {
            string horaFS = String.Format("Hora de Fin del Proceso : {0}", DateTime.Now);
            return horaFS;
        }

        public static void ActualizarParametro(ref List<SiMigraParametroDTO> parametros, int codigo, string valor)
        {
            List<SiMigraParametroDTO> entitys = parametros.Where(x => x.Migparcodi == codigo).ToList();

            foreach (SiMigraParametroDTO entity in entitys)
            {
                entity.ValorParametro = valor;
            }
        }
        public static void ActualizarParametroSTR(ref List<SiMigraParametroDTO> parametros, int codigo, string valor, List<int> listaTablasSRT, int flagAnulacion, string feccorteSTR)
        {
            List<SiMigraParametroDTO> entitys = parametros.Where(x => x.Migparcodi == codigo).ToList();

            foreach (SiMigraParametroDTO entity in entitys)
            {
                entity.ValorParametro = valor;
                if (listaTablasSRT.Contains(entity.Miqubacodi))
                {

                    if (flagAnulacion == 1)
                    {
                        entity.ValorParametro = feccorteSTR;
                    }
                    else
                    {
                        //ToDo : Fecha hardcode para pruebas ya que no data para periodos posteriodes
                        entity.ValorParametro = DateTime.Now.ToString("dd'/'MM'/'yyyy");
                    }
                }

            }
        }
        public List<int> ListarQuerybaseXListaParam(List<SiMigraquerybaseDTO> listaQueryOrder, List<SiMigraParametroDTO> listaParametrosPorQuery, List<int> listaParcodi)
        {
            List<int> l = new List<int>();

            List<int> lMiqubacodi = listaQueryOrder.Select(x => x.Miqubacodi).Distinct().ToList();

            if (listaParcodi.Any())
            {
                foreach (var miqubacodi in lMiqubacodi)
                {
                    bool flag1 = true;
                    foreach (var parcodi in listaParcodi)
                    {
                        bool flag2 = listaParametrosPorQuery.Find(x => x.Migparcodi == parcodi && x.Miqubacodi == miqubacodi) != null;
                        flag1 = flag1 && flag2;
                    }

                    if (flag1)
                        l.Add(miqubacodi);
                }
            }

            return l;
        }

        /// <summary>
        /// Metodo paar separar lista en sublistas de cierta cantidad
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        private List<List<int>> ListaSeparada(List<int> lista)
        {
            return lista
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / ConstantesTitularidad.TamanioSubLista)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        /// <summary>
        /// Validar existencia migración Fusión/Cambio Razón Social
        /// </summary>
        /// <param name="regDestino"></param>
        /// <param name="regOrigen"></param>
        /// <returns></returns>
        public bool ValidarExistenciaMigracion(SiMigracionDTO regDestino, SiMigraemprOrigenDTO regOrigen)
        {
            var listaMigracion = FactorySic.GetSiMigracionRepository().List().Where(x => x.Tmopercodi == ConstantesTitularidad.TipoMigrFusion || x.Tmopercodi == ConstantesTitularidad.TipoMigrCambioRazonSocial).ToList();
            var listMigracionOrigen = FactorySic.GetSiMigraemprorigenRepository().List().Where(x => x.Emprcodi == regOrigen.Emprcodi).ToList();
            bool retorna = false;

            foreach (var origen in listMigracionOrigen)
            {
                if (listaMigracion.Find(x => x.Migracodi == origen.Migracodi && x.Migradeleted != 1) != null)
                    retorna = true;
            }
            return retorna;
        }

        #endregion

        #region Registro Detalle de equipos de la migración

        /// <summary>
        /// Proceso para registrar los Equipos de una migración
        /// </summary>
        /// <param name="regDestino"></param>
        /// <param name="regOrigen"></param>
        /// <param name="listaEquips"></param>
        /// <param name="migracodi"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <param name="newId"></param>
        public List<SiHisempeqDTO> RegistrarMigracionHEquipos(SiMigracionDTO regDestino, SiMigraemprOrigenDTO regOrigen, List<EqEquipoDTO> listaEquips, int migracodi, IDbConnection conn, DbTransaction tran)
        {
            List<SiHisempeqDTO> listaHeqps = new List<SiHisempeqDTO>();
            List<string> listaMsjVal = new List<string>();

            try
            {
                //guardar los equipos de la empresa
                int maxId = FactorySic.GetSiHisempeqRepository().GetMaxId();
                for (int i = 0; i < listaEquips.Count; i++)
                {
                    var objEq = listaEquips[i];
                    int equicodi = objEq.Equicodi;
                    int equicodiOld = objEq.Equicodi;
                    int operador = objEq.Operadoremprcodi;
                    string equiestado = objEq.Equiestado;
                    string equinomb = objEq.Equinomb;
                    //if (objEq.Lastcodi.GetValueOrDefault(0) > 0)
                    //{
                    //    equicodiOld = equicodi;
                    //    equicodi = objEq.Lastcodi.Value;
                    //    equiestado = "A";
                    //}

                    SiHisempeqDTO equipMigrar = new SiHisempeqDTO
                    {
                        Hempeqcodi = maxId,
                        Emprcodi = regDestino.Emprcodi,
                        Equicodi = equicodi,
                        Equinomb = equinomb,
                        Hempeqfecha = regDestino.Migrafeccorte,
                        Migracodi = migracodi,
                        Equicodiold = equicodiOld,
                        Equicodiactual = equicodi,
                        Hempeqestado = equiestado,
                        Hempeqdeleted = ConstantesTitularidad.EliminadoLogicoNo,
                        Operadoremprcodi = operador,
                    };
                    listaHeqps.Add(equipMigrar);

                    try
                    {
                        FactorySic.GetSiHisempeqRepository().Save(equipMigrar, conn, tran);
                        maxId++;
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        listaMsjVal.Add("[Código=" + (equicodiOld != equicodi ? equicodiOld : equicodi) + ",Nombre=" + objEq.Equinomb + "]");
                    }
                }

                if (listaMsjVal.Count > 0)
                    throw new Exception("Los siguientes Equipos presentan datos inconsistentes en la BD: " + string.Join(",", listaMsjVal));

                return listaHeqps;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// validar registro de equipos para empresa
        /// </summary>
        /// <param name="regDestino"></param>
        /// <param name="listaEquip"></param>
        /// <returns></returns>
        public List<int> ValidarRegistroEquips(SiMigracionDTO regDestino, List<int> listaEquip)
        {
            var listEquipsEmpr = this.ListSiHisempeqDatas("-1").Where(x => x.Emprcodi == regDestino.Emprcodi).ToList();
            List<int> equipsRepetidos = new List<int>();

            foreach (var equicodi in listaEquip)
            {
                var registEquiAct = listEquipsEmpr.Find(x => x.Equicodi == equicodi);
                if (registEquiAct != null && registEquiAct.Heqdatestado == ConstantesTitularidad.EstadoRelEmpFechaInicio)
                {
                    equipsRepetidos.Add((int)registEquiAct.Equicodi);
                }
            }
            return equipsRepetidos;
        }

        /// <summary>
        /// No permitir "Duplicidad" ni "Equipos que no corresponden" a equipos con migraciones anteriormente
        /// </summary>
        /// <param name="listaEquip"></param>
        /// <returns></returns>
        public List<int> ValidarEquiposMigracion(List<int> listaEquip)
        {
            var equipsMigrados = this.ListSiHisempeqs();
            List<int> equipsRepetidos = new List<int>();

            foreach (var equicodi in listaEquip)
            {
                var equip = equipsMigrados.Find(x => x.Equicodi == equicodi);
                if (equip != null)
                {
                    equipsRepetidos.Add((int)equip.Equicodi);
                }
            }
            return equipsRepetidos;
        }

        /// <summary>
        /// validar el mismo equipo con migración el mismo dia
        /// </summary>
        /// <param name="listaEquip"></param>
        /// <returns></returns>
        public void ValidarEquiposFechCorte(List<int> listaEquip, SiMigracionDTO regDestino)
        {
            var equipsMigrados = this.ListSiHisempeqs();
            List<int> equipsRepetidos = new List<int>();
            string listaMsjVal;

            foreach (var equicodi in listaEquip)
            {
                var equip = equipsMigrados.Find(x => x.Equicodi == equicodi && x.Hempeqfecha == regDestino.Migrafeccorte);
                if (equip != null)
                {
                    listaMsjVal = ("[Código=" + equip.Equicodi + ",Nombre=" + equip.Equinomb + ", Fecha de Corte=" + equip.Hempeqfecha.ToString(ConstantesAppServicio.FormatoFecha) + "]"); ;
                    throw new Exception("El Equipo presenta una migración con la misma Fecha de Corte: " + string.Join(",", listaMsjVal));
                }
            }
        }

        #endregion

        #region Registro Detalle de puntos de la migración

        /// <summary>
        /// Proceso para registrar los Puntos de Medición de una migración
        /// </summary>
        /// <param name="regDestino"></param>
        /// <param name="regOrigen"></param>
        /// <param name="listaPto"></param>
        /// <param name="migracodi"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <param name="newId"></param>
        public List<SiHisempptoDTO> RegistrarMigracionHPtos(SiMigracionDTO regDestino, SiMigraemprOrigenDTO regOrigen, List<MePtomedicionDTO> listaPto, int migracodi, IDbConnection conn, DbTransaction tran)
        {
            List<SiHisempptoDTO> listaHpts = new List<SiHisempptoDTO>();
            List<string> listaMsjVal = new List<string>();
            try
            {
                //guardar los puntos de la empresa
                int maxId = FactorySic.GetSiHisempptoRepository().GetMaxId();
                for (int i = 0; i < listaPto.Count; i++)
                {
                    var objPto = listaPto[i];

                    int ptomedicodi = objPto.Ptomedicodi;
                    int ptomedicodiOld = objPto.Ptomedicodi;
                    string ptomediestado = objPto.Ptomediestado;
                    string Ptomedidesc = objPto.Ptomedidesc;

                    //if (objPto.Lastcodi > 0)
                    //{
                    //    ptomedicodiOld = ptomedicodi;
                    //    ptomedicodi = objPto.Lastcodi;
                    //    ptomediestado = "A";
                    //}

                    SiHisempptoDTO pptoMigrar = new SiHisempptoDTO
                    {
                        Hempptcodi = maxId,
                        Emprcodi = regDestino.Emprcodi,
                        Ptomedicodi = ptomedicodi,
                        Hempptfecha = regDestino.Migrafeccorte,
                        Migracodi = migracodi,
                        Ptomedicodiold = ptomedicodiOld,
                        Ptomedicodiactual = ptomedicodi,
                        Hempptestado = ptomediestado,
                        Hempptdeleted = ConstantesTitularidad.EliminadoLogicoNo,
                        Ptomedidesc = Ptomedidesc
                    };
                    listaHpts.Add(pptoMigrar);

                    try
                    {
                        FactorySic.GetSiHisempptoRepository().Save(pptoMigrar, conn, tran);
                        maxId++;
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        listaMsjVal.Add("[Código=" + objPto.Ptomedicodi + ",Nombre=" + objPto.Ptomedidesc + "]");
                    }
                }

                if (listaMsjVal.Count > 0)
                    throw new Exception("Los siguientes Puntos de medición presentan datos inconsistentes en la BD: " + string.Join(",", listaMsjVal));

                return listaHpts;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// validar registro de puntos para empresa
        /// </summary>
        /// <param name="regDestino"></param>
        /// <param name="listaPto"></param>
        /// <returns></returns>
        public List<int> ValidarRegistroPtos(SiMigracionDTO regDestino, List<int> listaPto)
        {
            var listPpptosEmpr = this.ListSiHisempptoDatas("-1").Where(x => x.Emprcodi == regDestino.Emprcodi).ToList();
            List<int> pttosRepetidos = new List<int>();

            foreach (var ptomedicodi in listaPto)
            {
                var registPptosAct = listPpptosEmpr.Find(x => x.Ptomedicodi == ptomedicodi);
                if (registPptosAct != null && registPptosAct.Hptdatptoestado == ConstantesTitularidad.EstadoRelEmpFechaInicio)
                {
                    pttosRepetidos.Add((int)registPptosAct.Ptomedicodi);
                }
            }

            return pttosRepetidos;
        }

        /// <summary>
        /// validar el  punto con más de una migración el mismo dia
        /// </summary>
        /// <param name="listPtos"></param>
        /// <param name="regDestino"></param>
        public void ValidarPtsFechCorte(List<int> listPtos, SiMigracionDTO regDestino)
        {
            var ptsMigrados = this.ListSiHisempptos();
            string listaMsjVal;

            foreach (var ptomedicodi in listPtos)
            {
                var pto = ptsMigrados.Find(x => x.Ptomedicodi == ptomedicodi && x.Hempptfecha == regDestino.Migrafeccorte);
                if (pto != null)
                {
                    listaMsjVal = ("[Código=" + pto.Ptomedicodi + ",Nombre=" + pto.Ptomedidesc + ", Fecha de Corte=" + pto.Hempptfecha.ToString(ConstantesAppServicio.FormatoFecha) + "]"); ;
                    throw new Exception("El Punto presenta una migración con la misma Fecha de Corte: " + string.Join(",", listaMsjVal));
                }
            }
        }
        #endregion

        #region Registro Detalle de grupos de la migración

        /// <summary>
        /// Proceso para registrar los Grupos de una migración
        /// </summary>
        /// <param name="regDestino"></param>
        /// <param name="regOrigen"></param>
        /// <param name="listaGrups"></param>
        /// <param name="migracodi"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <param name="newId"></param>
        public List<SiHisempgrupoDTO> RegistrarMigracionHGrupos(SiMigracionDTO regDestino, SiMigraemprOrigenDTO regOrigen, List<PrGrupoDTO> listaGrupos, int migracodi, IDbConnection conn, DbTransaction tran)
        {
            List<SiHisempgrupoDTO> listaHgrs = new List<SiHisempgrupoDTO>();
            List<string> listaMsjVal = new List<string>();

            try
            {
                //guardar los grupos de la empresa
                int maxId = FactorySic.GetSiHisempgrupoRepository().GetMaxId();
                for (int i = 0; i < listaGrupos.Count; i++)
                {
                    var objGrupo = listaGrupos[i];
                    int grupocodi = objGrupo.Grupocodi;
                    int grupocodiOld = objGrupo.Grupocodi;
                    string grupoestado = objGrupo.GrupoEstado;
                    string gruponomb = objGrupo.Gruponomb;

                    SiHisempgrupoDTO GrupoMigrar = new SiHisempgrupoDTO
                    {
                        Hempgrcodi = maxId,
                        Emprcodi = regDestino.Emprcodi,
                        Grupocodi = grupocodi,
                        Hempgrfecha = regDestino.Migrafeccorte,
                        Migracodi = migracodi,
                        Grupocodiold = grupocodiOld,
                        Grupocodiactual = grupocodi,
                        Hempgrestado = grupoestado,
                        Hempgrdeleted = ConstantesTitularidad.EliminadoLogicoNo,
                        Gruponomb = gruponomb
                    };
                    listaHgrs.Add(GrupoMigrar);

                    try
                    {
                        FactorySic.GetSiHisempgrupoRepository().Save(GrupoMigrar, conn, tran);
                        maxId++;
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        listaMsjVal.Add("[Código=" + objGrupo.Grupocodi + ",Nombre=" + objGrupo.Gruponomb + "]");
                    }
                }

                if (listaMsjVal.Count > 0)
                    throw new Exception("Los siguientes Grupos presentan datos inconsistentes en la BD: " + string.Join(",", listaMsjVal));

                return listaHgrs;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// validar registro de grupos para empresa
        /// </summary>
        /// <param name="regDestino"></param>
        /// <param name="listaGrups"></param>
        /// <returns></returns>
        public List<int> ValidarRegistroGrupos(SiMigracionDTO regDestino, List<int> listaGrups)
        {
            var listGruposEmpr = this.ListSiHisempgrupoDatas("-1").Where(x => x.Emprcodi == regDestino.Emprcodi).ToList();
            List<int> gruposRepetidos = new List<int>();

            foreach (var grupocodi in listaGrups)
            {
                var registEquiAct = listGruposEmpr.Find(x => x.Grupocodi == grupocodi);
                if (registEquiAct != null && registEquiAct.Hgrdatestado == ConstantesTitularidad.EstadoRelEmpFechaInicio)
                {
                    gruposRepetidos.Add((int)registEquiAct.Grupocodi);
                }
            }
            return gruposRepetidos;
        }

        /// <summary>
        /// validar el  grupo con más de una migración el mismo dia
        /// </summary>
        /// <param name="listGps"></param>
        /// <param name="regDestino"></param>
        public void ValidarGrsFechCorte(List<int> listGps, SiMigracionDTO regDestino)
        {
            var grpsMigrados = this.ListSiHisempgrupos();
            string listaMsjVal;

            foreach (var grupocodi in listGps)
            {
                var grpo = grpsMigrados.Find(x => x.Grupocodi == grupocodi && x.Hempgrfecha == regDestino.Migrafeccorte);
                if (grpo != null)
                {
                    listaMsjVal = ("[Código=" + grpo.Grupocodi + ",Nombre=" + grpo.Gruponomb + ", Fecha de Corte=" + grpo.Hempgrfecha.ToString(ConstantesAppServicio.FormatoFecha) + "]"); ;
                    throw new Exception("El Grupo presenta una migración con la misma Fecha de Corte: " + string.Join(",", listaMsjVal));
                }
            }
        }
        #endregion

        #region Registro data histórica equipos

        /// <summary>
        /// validar data con fecha inconsistente para su posterior insercion en SI_HISEMPPTO_DATA
        /// </summary>
        /// 
        public int ValidarHppeqfecha(List<SiHisempeqDTO> equipos, int migracodi, DateTime fechacorte)
        {
            foreach (var item in equipos)
            {
                if (FactorySic.GetSiHisempeqRepository().ConsultarEquipMigracion(migracodi, item.Equicodi, fechacorte) > 0) return 1;
            }
            return 0;
        }

        /// <summary>
        /// Procesar data histórica
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="migracodi"></param>
        /// <param name="usuarioCreacion"></param>
        /// <param name="fechaCreacion"></param>
        /// <param name="fechaCorte"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <param name="lista"></param>
        public void ProcesarDataHistoricaEquips(int emprcodi, int migracodi, string usuarioCreacion, DateTime fechaCreacion, DateTime fechaCorte, IDbConnection conn, DbTransaction tran, List<SiHisempeqDTO> lista)
        {
            List<string> listaMsjVal = new List<string>();
            try
            {
                //Validacion fecha correcta
                var flagValidarEquipos = this.ValidarHppeqfecha(lista, migracodi, fechaCorte);
                if (flagValidarEquipos > 0) throw new Exception("Migración inválida debido que ya existen equipos con migraciones posteriores a la fecha de corte");

                //Eq_data
                List<SiHisempeqDataDTO> listaAllHeqData = this.ListSiHisempeqDatas("-1").OrderByDescending(x => x.Heqdatfecha).ThenByDescending(X => X.Heqdatestado).ToList();

                //Eq
                List<SiHisempeqDTO> listSiHisemppeqNew = this.ListSiHisempeqs();

                int id = FactorySic.GetSiHisempeqRepository().GetMaxId();
                foreach (var itemEquip in lista)
                {
                    //ayuda de  equipo inicial 
                    var equipoActual = listaAllHeqData.Find(x => x.Equicodi == itemEquip.Equicodiold && x.Equicodiactual == itemEquip.Equicodiold);

                    ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Caso Especial Caso Especial  STATKRAFT hacia  STATKRAFT  S.A.
                    //if (equipoActual == null)
                    //{
                    //    equipoActual = listaAllHeqData.Find(x => x.Equicodiold == itemEquip.Equicodiold && x.Emprcodi == 10636);
                    //}
                    ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    if (equipoActual == null)
                        //Verificar si existe el equipo en Eq_data
                        listaMsjVal.Add("[Código=" + itemEquip.Equicodi + ",Nombre=" + itemEquip.Equinomb + "]");
                    else
                    {
                        SiHisempeqDTO equipInicio = new SiHisempeqDTO
                        {
                            Hempeqcodi = id,
                            Emprcodi = emprcodi,
                            Equicodi = equipoActual.Equicodi,
                            Equinomb = equipoActual.Equinomb,
                            Hempeqfecha = fechaCorte,
                            Migracodi = migracodi,
                            Equicodiold = (int)equipoActual.Equicodiold,
                            Equicodiactual = (int)equipoActual.Equicodiactual,
                            Hempeqestado = "B",
                            Hempeqdeleted = ConstantesTitularidad.EliminadoLogicoNo,
                        };
                        listSiHisemppeqNew.Add(equipInicio);
                    }
                }

                if (listaMsjVal.Count > 0)
                    throw new Exception("Los siguientes Equipos no han sido configurados para que funcionen en el aplicativo: " + string.Join(",", listaMsjVal));
                //
                listSiHisemppeqNew.AddRange(lista);
                listSiHisemppeqNew = listSiHisemppeqNew.Where(X => X.Hempeqdeleted == ConstantesTitularidad.EliminadoLogicoNo)
                                                    .OrderByDescending(x => x.Hempeqfecha).ThenBy(X => X.Hempeqestado).ToList();
                List<SiHisempeqDTO> listaAllHeq = ListarHpeqsUltRegst(listSiHisemppeqNew);

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///generar lista de datos a registrar en la tabla Si_Hisemppto_Data
                List<SiHisempeqDataDTO> listHeqnew = new List<SiHisempeqDataDTO>();

                //Agregar obj nuevos
                foreach (var listaAgrupada in listaAllHeq.GroupBy(x => x.Equicodiactual))
                {
                    var listaAgrupNew = listaAgrupada.ToList();
                    //.Where(x => x.Migracodi == migracodi)
                    foreach (var hipequipEnt in listaAgrupNew.Where(x => x.Migracodi == migracodi).OrderByDescending(x => x.Hempeqestado))
                    {
                        if (listaAllHeqData.Find(x => x.Equicodi == hipequipEnt.Equicodi || x.Equicodi == hipequipEnt.Equicodiold) == null)
                            //Verificar si existe el equipo en Eq_data
                            listaMsjVal.Add("[Código=" + hipequipEnt.Equicodi + ",Nombre=" + hipequipEnt.Equinomb + "]");
                        else
                        {
                            listHeqnew.Add(new SiHisempeqDataDTO
                            {
                                Heqdatfecha = hipequipEnt.Hempeqfecha,
                                Emprcodi = (int)hipequipEnt.Emprcodi,
                                Equicodi = (int)hipequipEnt.Equicodi,
                                Heqdatestado = hipequipEnt.Hempeqestado != ConstantesAppServicio.Baja ? ConstantesTitularidad.EstadoRelEmpFechaInicio : ConstantesTitularidad.EstadoRelEmpFechaFin,
                                Equicodiold = hipequipEnt.Equicodiold,
                                Equicodiactual = hipequipEnt.Equicodiactual,
                                Heqdatusucreacion = usuarioCreacion,
                                Heqdatfeccreacion = fechaCreacion
                            });
                        }
                    }
                }

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                List<SiHisempeqDataDTO> listaValidada = new List<SiHisempeqDataDTO>();
                if (listaAllHeqData.Any())
                {
                    listHeqnew = (from h in listHeqnew where !listaAllHeqData.Any(x => x.Heqdatfecha == h.Heqdatfecha && x.Equicodi == h.Equicodi && x.Emprcodi == h.Emprcodi) select h).ToList();
                    listaValidada = listHeqnew.OrderBy(x => x.Heqdatfecha).GroupBy(x => new { x.Equicodi, x.Heqdatfecha, x.Heqdatestado }).Select(x => x.Last()).ToList();
                }

                //Insertar en BD
                int maxId = FactorySic.GetSiHisempeqDataRepository().GetMaxId();
                foreach (var item in listaValidada)
                {
                    item.Heqdatcodi = maxId;
                    FactorySic.GetSiHisempeqDataRepository().Save(item, conn, tran);
                    maxId++;
                }
                //actualizar equipos equicodi.
                foreach (var listaAgrupada in listaValidada.GroupBy(x => x.Equicodiactual))
                {
                    var listaNew = listaAgrupada.ToList();
                    var equipoDat = listaNew.Last();
                    var equipoAnterior = listaNew.ElementAt(0);

                    FactorySic.GetSiHisempeqDataRepository().UpdateEquipoActual(equipoDat.Equicodiactual, equipoDat.Equicodiold, equipoAnterior.Equicodiold, conn, tran);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Método para obtener el equipo con el último cambio(equipo actual) y devolver una lista con todos los equipos actualizados con su respectivo equipo actual
        /// </summary>
        /// <param name="listaEqSiHisempeqDTO"></param>
        /// <returns></returns>
        public List<SiHisempeqDTO> ListarHpeqsUltRegst(List<SiHisempeqDTO> listaEqSiHisempeqDTO)
        {
            List<SiHisempeqDTO> resultadoListHpeq = listaEqSiHisempeqDTO;
            foreach (var equipRevisado in listaEqSiHisempeqDTO)
            {
                int sw = 0;
                if (equipRevisado.Equicodi != equipRevisado.Equicodiold && !equipRevisado.EstadoRecorrido)
                {
                    var regTieneRef = listaEqSiHisempeqDTO.Find(x => x.Equicodiold == equipRevisado.Equicodi && x.Equicodi != equipRevisado.Equicodi);
                    sw = regTieneRef != null ? 1 : 0;
                }

                if (sw == 0 && equipRevisado.Hempeqestado != ConstantesAppServicio.Baja && !equipRevisado.EstadoRecorrido)
                {
                    var listrecursivo = this.ListarHpeqsRecorrido((int)equipRevisado.Equicodi, listaEqSiHisempeqDTO);
                    resultadoListHpeq = listrecursivo;
                }
            }
            return resultadoListHpeq;
        }

        /// <summary>
        /// Metodo para registrar el ultimo equipo en todos los demas resgistros el cual estan relacionados con él
        /// </summary>
        /// <param name="ultimopequip"></param>
        /// <param name="listEqSiHisempeqDTO"></param>
        /// <returns></returns>
        public List<SiHisempeqDTO> ListarHpeqsRecorrido(int ultimopequip, List<SiHisempeqDTO> listEqSiHisempeqDTO)
        {
            int nuevo = ultimopequip;

            foreach (var hipequip in listEqSiHisempeqDTO)
            {
                if (hipequip.Equicodi == ultimopequip && !hipequip.EstadoRecorrido)
                {
                    hipequip.Equicodiactual = nuevo;
                    ultimopequip = hipequip.Hempeqestado == ConstantesAppServicio.Activo ? (int)hipequip.Equicodiold : (int)hipequip.Equicodi;
                    hipequip.EstadoRecorrido = true;
                }
            }
            return listEqSiHisempeqDTO;
        }

        #endregion

        #region Registro data histórica puntos

        /// <summary>
        /// validar data inconsistente para su posterior insercion en SI_HISEMPPTO_DATA
        /// </summary>
        /// 
        public int ValidarHptfecha(List<SiHisempptoDTO> puntos, int migracodi, DateTime fechacorte)
        {
            foreach (var item in puntos)
            {
                if (FactorySic.GetSiHisempptoRepository().ConsultarPtosMigracion(migracodi, item.Ptomedicodi, fechacorte) > 0) return 1;
            }
            return 0;
        }

        /// <summary>
        /// Procesar data histórica
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="migracodi"></param>
        /// <param name="usuarioCreacion"></param>
        /// <param name="fechaCreacion"></param>
        /// <param name="fechaCorte"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <param name="lista"></param>
        public void ProcesarDataHistoricaPtos(int emprcodi, int migracodi, string usuarioCreacion, DateTime fechaCreacion, DateTime fechaCorte, IDbConnection conn, DbTransaction tran, List<SiHisempptoDTO> lista)
        {
            List<string> listaMsjVal = new List<string>();
            try
            {
                //validacion fecha correcta
                var flagValidarPuntos = this.ValidarHptfecha(lista, migracodi, fechaCorte);
                if (flagValidarPuntos > 0) throw new Exception("Migración inválida debido que ya existen puntos con migraciones posteriores a la fecha de corte");

                //Pto_data
                List<SiHisempptoDataDTO> listaAllHptoData = this.ListSiHisempptoDatas("-1").OrderByDescending(x => x.Hptdatfecha).ThenByDescending(X => X.Hptdatptoestado).ToList();

                //Pto
                List<SiHisempptoDTO> listSiHiptoNew = this.ListSiHisempptos();

                int id = FactorySic.GetSiHisempptoRepository().GetMaxId();
                foreach (var itemPto in lista)
                {
                    //ayuda de  equipo inicial 
                    var puntoActual = listaAllHptoData.Find(x => x.Ptomedicodi == itemPto.Ptomedicodiold && x.Ptomedicodiactual == itemPto.Ptomedicodiold);

                    ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>Caso Especial  STATKRAFT hacia  STATKRAFT  S.A. 
                    //if (puntoActual == null)
                    //{
                    //    puntoActual = listaAllHptoData.Find(x => x.Ptomedicodiold == itemPto.Ptomedicodiold && x.Emprcodi == 10636);
                    //}
                    ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    if (puntoActual == null)
                        //Verificar si existe el equipo en Eq_data
                        listaMsjVal.Add("[Código=" + itemPto.Ptomedicodi + ",Nombre=" + itemPto.Ptomedidesc + "]");
                    else
                    {
                        SiHisempptoDTO puntoInicio = new SiHisempptoDTO
                        {
                            Hempptcodi = id,
                            Emprcodi = emprcodi,
                            Ptomedicodi = puntoActual.Ptomedicodi,
                            Ptomedidesc = puntoActual.Ptomedidesc,
                            Hempptfecha = fechaCorte,
                            Migracodi = migracodi,
                            Ptomedicodiold = (int)puntoActual.Ptomedicodiold,
                            Ptomedicodiactual = (int)puntoActual.Ptomedicodiactual,
                            Hempptestado = "B",
                            Hempptdeleted = ConstantesTitularidad.EliminadoLogicoNo,
                        };
                        listSiHiptoNew.Add(puntoInicio);
                    }
                }

                if (listaMsjVal.Count > 0)
                    throw new Exception("Los siguientes Puntos de medición no han sido configurados para que funcionen en el aplicativo: " + string.Join(",", listaMsjVal));

                //
                listSiHiptoNew.AddRange(lista);
                listSiHiptoNew = listSiHiptoNew.Where(X => X.Hempptdeleted == ConstantesTitularidad.EliminadoLogicoNo)
                                                .OrderByDescending(x => x.Hempptfecha).ThenBy(X => X.Hempptestado).ToList();
                List<SiHisempptoDTO> listaAllHpto = ListarHpptsUltRegst(listSiHiptoNew);

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //generar lista de datos para el ingreso a la tabla Si_Hisemppto_Data
                List<SiHisempptoDataDTO> listHptonew = new List<SiHisempptoDataDTO>();

                //ingreso data en la tabla pptos_dat
                foreach (var listaAgrupada in listaAllHpto.GroupBy(x => x.Ptomedicodiactual))
                {
                    var listaAgrupNew = listaAgrupada.ToList();

                    foreach (var hipptoEnt in listaAgrupNew.Where(x => x.Migracodi == migracodi).OrderByDescending(x => x.Hempptestado))
                    {
                        if (listaAllHptoData.Find(x => x.Ptomedicodi == hipptoEnt.Ptomedicodi || x.Ptomedicodi == hipptoEnt.Ptomedicodiold) == null)
                            //Verificar si existe el equipo en Eq_data
                            listaMsjVal.Add("[Código=" + hipptoEnt.Ptomedicodi + ",Nombre=" + hipptoEnt.Ptomedidesc + "]");
                        else
                        {
                            listHptonew.Add(new SiHisempptoDataDTO
                            {
                                Hptdatfecha = hipptoEnt.Hempptfecha,
                                Emprcodi = (int)hipptoEnt.Emprcodi,
                                Ptomedicodi = (int)hipptoEnt.Ptomedicodi,
                                Hptdatptoestado = hipptoEnt.Hempptestado != ConstantesAppServicio.Baja ? ConstantesTitularidad.EstadoRelEmpFechaInicio : ConstantesTitularidad.EstadoRelEmpFechaFin,
                                Ptomedicodiold = hipptoEnt.Ptomedicodiold,
                                Ptomedicodiactual = hipptoEnt.Ptomedicodiactual,
                                Hptdatusucreacion = usuarioCreacion,
                                Hptdatfeccreacion = fechaCreacion
                            });
                        }
                    }
                }

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                List<SiHisempptoDataDTO> listaValidada = new List<SiHisempptoDataDTO>();
                if (listaAllHptoData.Any())
                {
                    listHptonew = (from h in listHptonew where !listaAllHptoData.Any(x => x.Hptdatfecha == h.Hptdatfecha && x.Ptomedicodi == h.Ptomedicodi) select h).ToList();
                    listaValidada = listHptonew.OrderBy(x => x.Hptdatfecha).GroupBy(x => new { x.Ptomedicodi, x.Hptdatfecha, x.Hptdatptoestado }).Select(x => x.Last()).ToList();
                }

                //Insertar en BD
                int maxId = FactorySic.GetSiHisempptoDataRepository().GetMaxId();
                foreach (var item in listaValidada)
                {
                    item.Hptdatcodi = maxId;
                    FactorySic.GetSiHisempptoDataRepository().Save(item, conn, tran);
                    maxId++;
                }

                foreach (var listaAgrupada in listaValidada.GroupBy(x => x.Ptomedicodiactual))
                {
                    var listaNew = listaAgrupada.ToList();
                    var puntoFinal = listaNew.Last();
                    var PuntoAnterior = listaNew.ElementAt(0);

                    FactorySic.GetSiHisempptoDataRepository().UpdatePuntoActual(puntoFinal.Ptomedicodiactual.Value, puntoFinal.Ptomedicodiold.Value, PuntoAnterior.Ptomedicodiold.Value, conn, tran);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Método para obtener el punto con el último cambio(punto actual) y devolver una lista con todos los puntos actualizados con su respectivo putno actual
        /// </summary>
        /// <param name="listaPtoSiHisempptoDTO"></param>
        /// <returns></returns>
        public List<SiHisempptoDTO> ListarHpptsUltRegst(List<SiHisempptoDTO> listaPtoSiHisempptoDTO)
        {
            List<SiHisempptoDTO> resultadoListHppto = listaPtoSiHisempptoDTO;
            foreach (var ptoRevisado in listaPtoSiHisempptoDTO)
            {
                int sw = 0;
                if (ptoRevisado.Ptomedicodi != ptoRevisado.Ptomedicodiold && !ptoRevisado.EstadoRecorrido)
                {
                    var regTieneRef = listaPtoSiHisempptoDTO.Find(x => x.Ptomedicodiold == ptoRevisado.Ptomedicodi && x.Ptomedicodi != ptoRevisado.Ptomedicodi);
                    sw = regTieneRef != null ? 1 : 0;
                }

                if (sw == 0 && ptoRevisado.Hempptestado != ConstantesAppServicio.Baja && !ptoRevisado.EstadoRecorrido)
                {
                    var listrecursivo = this.ListarHpptsRecorrido((int)ptoRevisado.Ptomedicodi, listaPtoSiHisempptoDTO);
                    resultadoListHppto = listrecursivo;
                }
            }
            return resultadoListHppto;
        }

        /// <summary>
        /// Metodo para registrar el ultimo ppto en todos los demas resgistros el cual estan relacionados con el
        /// </summary>
        /// <param name="ultimoppto"></param>
        /// <param name="listptoSiHisemppto"></param>
        /// <returns></returns>
        public List<SiHisempptoDTO> ListarHpptsRecorrido(int ultimoppto, List<SiHisempptoDTO> listptoSiHisemppto)
        {
            int nuevo = ultimoppto;

            foreach (var hippto in listptoSiHisemppto)
            {
                if (hippto.Ptomedicodi == ultimoppto && !hippto.EstadoRecorrido)
                {
                    hippto.Ptomedicodiactual = nuevo;
                    ultimoppto = hippto.Hempptestado == ConstantesAppServicio.Activo ? (int)hippto.Ptomedicodiold : (int)hippto.Ptomedicodi;
                    hippto.EstadoRecorrido = true;
                }
            }
            return listptoSiHisemppto;
        }

        #endregion

        #region Registro data histórica grupos

        /// <summary>
        /// validar data con fecha inconsistente para su posterior insercion en SI_HISEMPGRUPO_DATA
        /// </summary>
        /// 
        public int ValidarHpgrfecha(List<SiHisempgrupoDTO> grupos, int migracodi, DateTime fechacorte)
        {
            foreach (var item in grupos)
            {
                if (FactorySic.GetSiHisempgrupoRepository().ConsultarGrpsMigracion(migracodi, item.Grupocodi, fechacorte) > 0) return 1;
            }
            return 0;
        }

        /// <summary>
        /// Procesar data histórica grupos
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="migracodi"></param>
        /// <param name="usuarioCreacion"></param>
        /// <param name="fechaCreacion"></param>
        /// <param name="fechaCorte"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <param name="lista"></param>
        public void ProcesarDataHistoricaGrupos(int emprcodi, int migracodi, string usuarioCreacion, DateTime fechaCreacion, DateTime fechaCorte, IDbConnection conn, DbTransaction tran, List<SiHisempgrupoDTO> lista)
        {
            List<string> listaMsjVal = new List<string>();
            try
            {
                //validacion fecha correcta
                var flagValidarGrupos = this.ValidarHpgrfecha(lista, migracodi, fechaCorte);
                if (flagValidarGrupos > 0) throw new Exception("Migración inválida debido que ya existen grupos con migraciones posteriores a la fecha de corte");

                //Gr
                List<SiHisempgrupoDTO> listSiHisempgrNew = this.ListSiHisempgrupos();

                foreach (var itemGrupo in lista)
                {
                    //ayuda de  equipo inicial 
                    var grupoActual = FactorySic.GetSiHisempgrupoDataRepository().List("-1").Find(x => x.Grupocodi == itemGrupo.Grupocodiold && x.Grupocodiactual == itemGrupo.Grupocodiold);

                    if (grupoActual == null)
                        //Verificar si existe el grupo en Gr_data
                        listaMsjVal.Add("[Código=" + itemGrupo.Grupocodi + ",Nombre=" + itemGrupo.Gruponomb + "]");
                    else
                    {
                        int Id = FactorySic.GetSiHisempeqRepository().GetMaxId();
                        SiHisempgrupoDTO grupoInicio = new SiHisempgrupoDTO
                        {
                            Hempgrcodi = Id,
                            Emprcodi = emprcodi,
                            Grupocodi = grupoActual.Grupocodi,
                            Gruponomb = "prueba",
                            Hempgrfecha = fechaCorte,
                            Migracodi = migracodi,
                            Grupocodiold = (int)grupoActual.Grupocodiold,
                            Grupocodiactual = (int)grupoActual.Grupocodiactual,
                            Hempgrestado = "B",
                            Hempgrdeleted = ConstantesTitularidad.EliminadoLogicoNo,
                        };
                        listSiHisempgrNew.Add(grupoInicio);
                    }
                }

                if (listaMsjVal.Count > 0)
                    throw new Exception("Los siguientes grupos no han sido configurados para que funcionen en el aplicativo: " + string.Join(",", listaMsjVal));
                //
                listSiHisempgrNew.AddRange(lista);
                listSiHisempgrNew = listSiHisempgrNew.Where(X => X.Hempgrdeleted == ConstantesTitularidad.EliminadoLogicoNo)
                                                    .OrderByDescending(x => x.Hempgrfecha).ThenBy(X => X.Hempgrestado).ToList();
                List<SiHisempgrupoDTO> listaAllHgr = ListarHpgrupsUltRegst(listSiHisempgrNew);

                //Gr_data
                List<SiHisempgrupoDataDTO> listaAllHgrData = this.ListSiHisempgrupoDatas("-1").OrderByDescending(x => x.Hgrdatfecha).ThenByDescending(X => X.Hgrdatestado).ToList();

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //generar lista de datos para el ingreso a la tabla Si_Hisemppto_Data
                List<SiHisempgrupoDataDTO> listSiHgrNew = new List<SiHisempgrupoDataDTO>();


                //ingreso data en la tabla grupos_dat
                foreach (var listaAgrupada in listaAllHgr.GroupBy(x => x.Grupocodiactual))
                {
                    var listaAgrupNew = listaAgrupada.ToList();

                    foreach (var hipgrupoEnt in listaAgrupNew.Where(x => x.Migracodi == migracodi).OrderByDescending(x => x.Hempgrestado))
                    {
                        if (listaAllHgrData.Find(x => x.Grupocodi == hipgrupoEnt.Grupocodi) == null)
                            //Verificar si existe el grupo en Gr_data
                            listaMsjVal.Add("[Código=" + hipgrupoEnt.Grupocodi + ",Nombre=" + hipgrupoEnt.Gruponomb + "]");
                        else
                        {
                            // if (listaAllHgrData.Find(x => x.Grupocodi == hipgrupoEnt.Grupocodi).Hgrdatestado == ConstantesTitularidad.EstadoRelEmpFechaFin) continue;
                            listSiHgrNew.Add(new SiHisempgrupoDataDTO
                            {
                                Hgrdatfecha = hipgrupoEnt.Hempgrfecha,
                                Emprcodi = (int)hipgrupoEnt.Emprcodi,
                                Grupocodi = (int)hipgrupoEnt.Grupocodi,
                                Hgrdatestado = hipgrupoEnt.Hempgrestado != ConstantesAppServicio.Baja ? ConstantesTitularidad.EstadoRelEmpFechaInicio : ConstantesTitularidad.EstadoRelEmpFechaFin,
                                Grupocodiold = hipgrupoEnt.Grupocodiold,
                                Grupocodiactual = hipgrupoEnt.Grupocodiactual,
                                Hgrdatusucreacion = usuarioCreacion,
                                Hgrdatfeccreacion = fechaCreacion
                            });
                        }
                    }
                }

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                List<SiHisempgrupoDataDTO> listaValidada = new List<SiHisempgrupoDataDTO>();
                if (listaAllHgrData.Any())
                {
                    listSiHgrNew = (from h in listSiHgrNew where !listaAllHgrData.Any(x => x.Hgrdatfecha == h.Hgrdatfecha && x.Grupocodi == h.Grupocodi) select h).ToList();
                    listaValidada = listSiHgrNew.OrderBy(x => x.Hgrdatfecha).GroupBy(x => new { x.Grupocodi, x.Hgrdatfecha, x.Hgrdatestado }).Select(x => x.Last()).ToList();
                }

                //Insertar en BD
                int maxId = FactorySic.GetSiHisempgrupoDataRepository().GetMaxId();
                foreach (var item in listaValidada)
                {
                    item.Hgrdatcodi = maxId;
                    FactorySic.GetSiHisempgrupoDataRepository().Save(item, conn, tran);
                    maxId++;
                }

                foreach (var listaAgrupada in listaValidada.GroupBy(x => x.Grupocodiactual))
                {
                    var listaNew = listaAgrupada.ToList();
                    var grupoDat = listaNew.Last();
                    var grupoAnterior = listaNew.ElementAt(0);

                    FactorySic.GetSiHisempgrupoDataRepository().UpdateGrupoActual(grupoDat.Grupocodiactual.Value, grupoDat.Grupocodiold.Value, grupoAnterior.Grupocodiold.Value, conn, tran);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Método para obtener el grupo con el último cambio(equipo actual) y devolver una lista con todos los grupos actualizados con su respectivo equipo actual
        /// </summary>
        /// <param name="listaSiHisempgrupoDTO"></param>
        /// <returns></returns>
        public List<SiHisempgrupoDTO> ListarHpgrupsUltRegst(List<SiHisempgrupoDTO> listaSiHisempgrupoDTO)
        {
            List<SiHisempgrupoDTO> resultadoListHpgr = listaSiHisempgrupoDTO;
            foreach (var grupoRevisado in listaSiHisempgrupoDTO)
            {
                int sw = 0;
                if (grupoRevisado.Grupocodi != grupoRevisado.Grupocodiold && !grupoRevisado.EstadoRecorrido)
                {
                    var regTieneRef = listaSiHisempgrupoDTO.Find(x => x.Grupocodiold == grupoRevisado.Grupocodi && x.Grupocodi != grupoRevisado.Grupocodi);
                    sw = regTieneRef != null ? 1 : 0;
                }

                if (sw == 0 && grupoRevisado.Hempgrestado != ConstantesAppServicio.Baja && !grupoRevisado.EstadoRecorrido)
                {
                    var listrecursivo = this.ListarHpgrupsRecorrido((int)grupoRevisado.Grupocodi, listaSiHisempgrupoDTO);
                    resultadoListHpgr = listrecursivo;
                }
            }
            return resultadoListHpgr;
        }

        /// <summary>
        /// Método para registrar el último grupo en todos los demas resgistros el cual estan relacionados con él
        /// </summary>
        /// <param name="ultimopgrupo"></param>
        /// <param name="listEqSiHisempeqDTO"></param>
        /// <returns></returns>
        public List<SiHisempgrupoDTO> ListarHpgrupsRecorrido(int ultimopgrupo, List<SiHisempgrupoDTO> listEqSiHisempeqDTO)
        {
            int nuevo = ultimopgrupo;

            foreach (var hipgrupo in listEqSiHisempeqDTO)
            {
                if (hipgrupo.Grupocodi == ultimopgrupo && !hipgrupo.EstadoRecorrido)
                {
                    hipgrupo.Grupocodiactual = nuevo;
                    ultimopgrupo = hipgrupo.Hempgrestado == ConstantesAppServicio.Activo ? (int)hipgrupo.Grupocodiold : (int)hipgrupo.Grupocodi;
                    hipgrupo.EstadoRecorrido = true;
                }
            }
            return listEqSiHisempeqDTO;
        }

        #endregion

        #region Anular Transferencia

        /// <summary>
        /// Anular transferencia
        /// </summary>
        /// <param name="migracion"></param>
        /// <param name="user"></param>
        public void AnularTransferencia(int migracion, string user)
        {
            DbTransaction tran = null;
            DateTime fechaActualizacion = DateTime.Now;
            SiMigracionDTO migraAnulacion = new SiMigracionDTO();
            SiMigraemprOrigenDTO migOrigenAnulacion = new SiMigraemprOrigenDTO();

            try
            {
                var migracionEnt = FactorySic.GetSiMigracionRepository().GetById(migracion);
                //empresa origen 
                var migraOrigenEnt = FactorySic.GetSiMigraemprorigenRepository().List().Find(x => x.Migracodi == migracion);

                //nueva migración destino
                migraAnulacion.Emprcodi = migraOrigenEnt.Emprcodi;
                migraAnulacion.Tmopercodi = migracionEnt.Tmopercodi;
                migraAnulacion.Migradescripcion = "(ANULADO) " + migracionEnt.Migradescripcion;
                migraAnulacion.Migrafeccorte = migracionEnt.Migrafeccorte;
                migraAnulacion.MigrafeccorteSTR = migracionEnt.MigrafeccorteSTR;
                migraAnulacion.Migrausucreacion = user;
                migraAnulacion.Migrafeccreacion = fechaActualizacion;
                migraAnulacion.Migradeleted = 1;
                migraAnulacion.Migraflagstr = migracionEnt.Migraflagstr;

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                IDbConnection conn = FactorySic.GetSiMigracionRepository().BeginConnection();
                tran = FactorySic.GetSiMigracionRepository().StartTransaction(conn);

                //registro de migración como anulación 
                int migracodi = FactorySic.GetSiMigracionRepository().Save(migraAnulacion, conn, tran);

                //Actualización de la migración anterior anulada en el campo migrareldeleted
                FactorySic.GetSiMigracionRepository().UpdateMigraAnulacion(migracionEnt.Migracodi, migracodi, user, fechaActualizacion, conn, tran);

                //migración origen 
                migOrigenAnulacion.Migracodi = migracodi;
                migOrigenAnulacion.Emprcodi = migracionEnt.Emprcodi;
                migOrigenAnulacion.Migempusucreacion = user;
                migOrigenAnulacion.Migempfeccreacion = fechaActualizacion;
                migOrigenAnulacion.MigempestadoDest = migraOrigenEnt.MigempestadoDest;
                FactorySic.GetSiMigraemprorigenRepository().Save(migOrigenAnulacion, conn, tran);

                //equipos
                var listequipsEnt = FactorySic.GetSiHisempeqRepository().ListEquiposXMigracion(migracionEnt.Migracodi);
                List<int> listEquips = listequipsEnt.Select(x => x.Equicodi).ToList();
                //puntos
                var listptsEnt = FactorySic.GetSiHisempptoRepository().ListPtsXMigracion(migracionEnt.Migracodi);
                List<int> listPts = listptsEnt.Select(x => x.Ptomedicodi).ToList();
                //grupos
                var listgrpsEnt = FactorySic.GetSiHisempgrupoRepository().ListGrupsXMigracion(migracionEnt.Migracodi);
                List<int> lisGrps = listgrpsEnt.Select(x => x.Grupocodi).ToList();

                //validación migración
                var validaMiraEquips = this.ValidarAnulacion(migracionEnt.Migracodi, migracionEnt.Migrafeccorte, listEquips, listPts, lisGrps);
                if (validaMiraEquips > 0) throw new Exception("Anulación inválida debido que ya existen migraciones posteriores asociadas a elementos de esta migración");

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                int corrSiLogHInicio = FactorySic.GetSiLogRepository().GetMaxId();
                GrabarLogFecha(ObtenerHoraInicioSistema(), migracodi, user, conn, tran, corrSiLogHInicio, ConstantesTitularidad.TipoEventoLogInicio, null);
                int firstCorrSalida = 0;

                //Dar de baja logica para a los equipos,ptos y grupos de esa migracion
                FactorySic.GetSiHisempeqRepository().UpdateAnularTransf(migracionEnt.Migracodi, conn, tran);
                FactorySic.GetSiHisempptoRepository().UpdateAnularTransf(migracionEnt.Migracodi, conn, tran);
                FactorySic.GetSiHisempgrupoRepository().UpdateAnularTransf(migracionEnt.Migracodi, conn, tran);

                ///Registrar los equipos, puntos de medicion y grupos
                if (listEquips.Count > 0)
                {
                    //guardar los equipos de la empresa
                    this.RegistrarHequipoAnulacion(listEquips, migraAnulacion, migracodi, conn, tran, migracion);
                }
                if (listPts.Count > 0)
                {
                    //guardar los puntos de la empresa
                    this.RegistrarHptosAnulacion(listPts, migraAnulacion, migracodi, conn, tran);
                }
                if (lisGrps.Count > 0)
                {
                    //guardar los puntos de la empresa
                    this.RegistrarHgrpsAnulacion(lisGrps, migraAnulacion, migracodi, conn, tran);
                }

                //ELIMINAR DE LAS TABLAS DAT para equipos
                string cadenaEquips = string.Join(",", listEquips);
                if (cadenaEquips != string.Empty)
                    FactorySic.GetSiHisempeqDataRepository().DeleteXAnulacionMigra(listEquips, migracionEnt.Emprcodi, migraOrigenEnt.Emprcodi, migracionEnt.Migrafeccorte, conn, tran);

                //ELIMINAR DE LAS TABLAS DAT para puntos
                string cadenaPtos = string.Join(",", listPts);
                if (cadenaPtos != string.Empty)
                    FactorySic.GetSiHisempptoDataRepository().DeleteXAnulacionMigra(listPts, migracionEnt.Emprcodi, migraOrigenEnt.Emprcodi, migracionEnt.Migrafeccorte, conn, tran);

                //ELIMINAR DE LAS TABLAS DAT para grupos
                string cadenaGrups = string.Join(",", lisGrps);
                if (cadenaGrups != string.Empty)
                    FactorySic.GetSiHisempgrupoDataRepository().DeleteXAnulacionMigra(lisGrps, migracionEnt.Emprcodi, migraOrigenEnt.Emprcodi, migracionEnt.Migrafeccorte, conn, tran);

                //procesar anulación

                TTIEParametro objTTIE = new TTIEParametro()
                {
                    MigraCodi = migracodi,
                    TipoOperacion = migracionEnt.Tmopercodi,
                    IdEmpresaOrigen = migracionEnt.Emprcodi,
                    IdEmpresaDestino = migraOrigenEnt.Emprcodi,
                    Migraflagstr = migracionEnt.Migraflagstr,
                    User = user,
                    Migradescripcion = migracionEnt.Migradescripcion,
                    Ptos = listPts,
                    Equips = listEquips,
                    Grups = lisGrps,
                    Feccorte = migracionEnt.Migrafeccorte,
                    FeccorteSTR = migracionEnt.MigrafeccorteSTR,
                    FlagAnulacion = ConstantesTitularidad.FlagAnulacion,
                };
                List<String> mensajeLog = this.ProcesarQuerys(objTTIE, out firstCorrSalida, conn, tran);


                int corrSiLogHFin = firstCorrSalida;
                GrabarLogFecha(ObtenerHoraFinSistema(), migracodi, user, conn, tran, corrSiLogHFin, ConstantesTitularidad.TipoEventoLogFin, null);

                tran.Commit();
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            }
            catch (Exception ex)
            {
                if (tran != null)
                    tran.Rollback();
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        public int ValidarAnulacion(int migracodi, DateTime fechacorte, List<int> equipos, List<int> puntos, List<int> grupos)
        {
            foreach (var item in equipos)
            {
                if (FactorySic.GetSiHisempeqRepository().ConsultarEquipMigracion(migracodi, item, fechacorte) > 0) return 1;
            }

            foreach (var item in equipos)
            {
                if (FactorySic.GetSiHisempptoRepository().ConsultarPtosMigracion(migracodi, item, fechacorte) > 0) return 1;
            }

            foreach (var item in equipos)
            {
                if (FactorySic.GetSiHisempgrupoRepository().ConsultarGrpsMigracion(migracodi, item, fechacorte) > 0) return 1;
            }

            return 0;
        }

        /// <summary>
        /// registrar hequipos anulación
        /// </summary>
        /// <param name="equipos"></param>
        /// <param name="migraAnulacion"></param>
        /// <param name="migracodi"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        public void RegistrarHequipoAnulacion(List<int> equipos, SiMigracionDTO migraAnulacion, int migracodi, IDbConnection conn, DbTransaction tran, int migracionAnterior)
        {
            int maxId = FactorySic.GetSiHisempeqRepository().GetMaxId();
            for (int i = 0; i < equipos.Count; i++)
            {
                SiHisempeqDTO EquipMigrar = new SiHisempeqDTO
                {
                    Hempeqcodi = maxId,
                    Emprcodi = migraAnulacion.Emprcodi,
                    Equicodi = equipos[i],
                    Hempeqfecha = migraAnulacion.Migrafeccorte,
                    Migracodi = migracodi,
                    Equicodiold = equipos[i],
                    Hempeqestado = ConstantesAppServicio.Activo,
                    Hempeqdeleted = ConstantesTitularidad.EliminadoLogicoSi,
                    Operadoremprcodi = FactorySic.GetSiHisempeqRepository().List().Find(x => x.Migracodi == migracionAnterior && x.Equicodi == equipos[i]).Operadoremprcodi,
                };
                FactorySic.GetSiHisempeqRepository().Save(EquipMigrar, conn, tran);
                maxId++;
            }
        }

        /// <summary>
        /// registrar hptos anulación
        /// </summary>
        /// <param name="puntos"></param>
        /// <param name="migraAnulacion"></param>
        /// <param name="migracodi"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        public void RegistrarHptosAnulacion(List<int> puntos, SiMigracionDTO migraAnulacion, int migracodi, IDbConnection conn, DbTransaction tran)
        {
            int maxId = FactorySic.GetSiHisempptoRepository().GetMaxId();

            for (int i = 0; i < puntos.Count; i++)
            {
                SiHisempptoDTO pptoMigrar = new SiHisempptoDTO
                {
                    Hempptcodi = maxId,
                    Emprcodi = migraAnulacion.Emprcodi,
                    Ptomedicodi = puntos[i],
                    Hempptfecha = migraAnulacion.Migrafeccorte,
                    Migracodi = migracodi,
                    Ptomedicodiold = puntos[i],
                    Hempptestado = ConstantesAppServicio.Activo,
                    Hempptdeleted = ConstantesTitularidad.EliminadoLogicoSi,
                };

                FactorySic.GetSiHisempptoRepository().Save(pptoMigrar, conn, tran);
                maxId++;
            }
        }

        /// <summary>
        /// registrar hgrupos anulación
        /// </summary>
        /// <param name="grupos"></param>
        /// <param name="migraAnulacion"></param>
        /// <param name="migracodi"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        public void RegistrarHgrpsAnulacion(List<int> grupos, SiMigracionDTO migraAnulacion, int migracodi, IDbConnection conn, DbTransaction tran)
        {
            int maxId = FactorySic.GetSiHisempgrupoRepository().GetMaxId();
            for (int i = 0; i < grupos.Count; i++)
            {

                SiHisempgrupoDTO GrupoMigrar = new SiHisempgrupoDTO
                {
                    Hempgrcodi = maxId,
                    Emprcodi = migraAnulacion.Emprcodi,
                    Grupocodi = grupos[i],
                    Hempgrfecha = migraAnulacion.Migrafeccorte,
                    Migracodi = migracodi,
                    Grupocodiold = grupos[i],
                    Hempgrestado = ConstantesAppServicio.Activo,
                    Hempgrdeleted = ConstantesTitularidad.EliminadoLogicoSi,
                };
                FactorySic.GetSiHisempgrupoRepository().Save(GrupoMigrar, conn, tran);
                maxId++;
            }
        }

        #endregion

        #region Procesar Str

        /// <summary>
        /// Actualizar la migracion ya realizada, esto incluye las querys de los aplicativos STR
        /// </summary>
        /// <param name="migracion"></param>
        /// <param name="user"></param>
        public void RegistrarTransferenciaStr(int migracion, string user)
        {
            DbTransaction tran = null;
            DateTime fechaActualizacion = DateTime.Now;
            SiMigracionDTO migraAnulacion = new SiMigracionDTO();
            SiMigraemprOrigenDTO migOrigenAnulacion = new SiMigraemprOrigenDTO();

            try
            {
                var migracionEnt = FactorySic.GetSiMigracionRepository().GetById(migracion);
                //migra origen 
                var migraOrigenEnt = FactorySic.GetSiMigraemprorigenRepository().List().Find(x => x.Migracodi == migracion);

                IDbConnection conn = FactorySic.GetSiMigracionRepository().BeginConnection();
                tran = FactorySic.GetSiMigracionRepository().StartTransaction(conn);

                //Actualización de proceso pendiente  de la migración en el campo MIGRAFLAGSTR
                FactorySic.GetSiMigracionRepository().UpdateMigraProcesoPendiente(migracionEnt.Migracodi, conn, tran);

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                int corrSiLogHInicio = FactorySic.GetSiLogRepository().GetMaxId();
                GrabarLogFecha(ObtenerHoraInicioSistema(), migracionEnt.Migracodi, user, conn, tran, corrSiLogHInicio, ConstantesTitularidad.TipoEventoLogInicio, null);
                int firstCorrSalida = 0;

                //procesar 

                TTIEParametro objTTIE = new TTIEParametro()
                {
                    MigraCodi = migracionEnt.Migracodi,
                    TipoOperacion = migracionEnt.Tmopercodi,
                    IdEmpresaOrigen = migraOrigenEnt.Emprcodi,
                    IdEmpresaDestino = migracionEnt.Emprcodi,
                    Migraflagstr = ConstantesTitularidad.FlagProcesoStr,
                    User = user,
                    Migradescripcion = migracionEnt.Migradescripcion,
                    Ptos = new List<int>(),
                    Equips = new List<int>(),
                    Grups = new List<int>(),
                    Feccorte = migracionEnt.Migrafeccorte,
                    FlagAnulacion = ConstantesTitularidad.FlagAnulacion,
                };
                List<String> mensajeLog = this.ProcesarQuerys(objTTIE, out firstCorrSalida, conn, tran);
                int corrSiLogHFin = firstCorrSalida;
                GrabarLogFecha(ObtenerHoraFinSistema(), migracionEnt.Migracodi, user, conn, tran, corrSiLogHFin, ConstantesTitularidad.TipoEventoLogFin, null);

                tran.Commit();
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            }
            catch (Exception ex)
            {
                if (tran != null)
                    tran.Rollback();
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }

        }

        #endregion

        #region Detalle del TTIE

        #region TAB Lista de Equipos afectados

        /// <summary>
        /// Listar equipos por migración
        /// </summary>
        /// <param name="idMigracion"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarEquiposByMigracion(int idMigracion)
        {
            List<EqEquipoDTO> entitys = FactorySic.GetEqEquipoRepository().ListaEquiposSiEquipoMigrarByMigracodi(idMigracion);

            foreach (var reg in entitys)
            {
                reg.EstadoDesc = Util.EstadoDescripcion(reg.Equiestado);
                reg.Osigrupocodi = EquipamientoHelper.EstiloEstado(reg.Equiestado);
            }

            return entitys;
        }

        #endregion

        #region TAB Lista de Puntos de medición afectados

        /// <summary>
        /// Listar puntos de medición de una migración
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListarPtomedicionByMigracodi(int idMigracion)
        {
            var lista = FactorySic.GetMePtomedicionRepository().ListarPtomedicionByMigracodi(idMigracion);

            foreach (var reg in lista)
            {
                reg.Ptomedielenomb = string.IsNullOrEmpty(reg.Ptomedielenomb) ? string.Empty : reg.Ptomedielenomb.Trim();
                reg.Origlectnombre = string.IsNullOrEmpty(reg.Origlectnombre) ? string.Empty : reg.Origlectnombre.Trim();
                reg.Ptomediestadodescrip = Util.EstadoDescripcion(reg.Ptomediestado);
                reg.ColorEstado = EquipamientoHelper.EstiloEstado(reg.Ptomediestado);
            }

            return lista;
        }

        #endregion

        #region TAB Lista de Grupos afectados

        /// <summary>
        /// Listar grupos de una migración
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> ListarGrupoByMigracodi(int idMigracion)
        {
            var lista = FactorySic.GetPrGrupoRepository().ListarGrupoByMigracodi(idMigracion);

            foreach (var reg in lista)
            {
                reg.Gruponomb = string.IsNullOrEmpty(reg.Gruponomb) ? string.Empty : reg.Gruponomb.Trim();
                reg.Catenomb = string.IsNullOrEmpty(reg.Catenomb) ? string.Empty : reg.Catenomb.Trim();
                reg.Areanomb = string.IsNullOrEmpty(reg.Areanomb) ? string.Empty : reg.Areanomb.Trim();
                reg.GrupoEstadoDesc = Util.EstadoDescripcion(reg.GrupoEstado);
                reg.ColorEstado = EquipamientoHelper.EstiloEstado(reg.GrupoEstado);
            }

            return lista;
        }

        #endregion

        #region TAB Detalle adicional

        public List<TTIEDetalleAdicional> ListarDetalleAdicionalXMigracion(int migracodi)
        {
            List<TTIEDetalleAdicional> listaFinal = new List<TTIEDetalleAdicional>();

            List<SiHisempentidadDTO> listaTabla = GetByCriteriaSiHisempentidads(migracodi);

            foreach (var tabla in listaTabla)
            {
                TTIEDetalleAdicional reg = new TTIEDetalleAdicional() { Titulo = tabla.Hempentitulo, CampoDesc1 = tabla.Hempencampodesc, CampoDesc2 = tabla.Hempencampodesc2, CampoEstado = tabla.Hempencampoestado };

                List<SiHisempentidadDetDTO> listaDet = FactorySic.GetSiHisempentidadDetRepository().GetByCriteriaXTabla(
                                                        migracodi, tabla.Hempentablename, tabla.Hempencampoid, tabla.Hempencampodesc, tabla.Hempencampodesc2, tabla.Hempencampoestado);
                reg.ListaDetalle = listaDet;

                listaFinal.Add(reg);
            }

            return listaFinal;
        }

        #endregion

        #region TAB Log de Proceso

        /// <summary>
        /// Listar log por migración
        /// </summary>
        /// <param name="migracodi"></param>
        /// <returns></returns>
        public List<SiLogDTO> ListarLogByMigracion(int migracodi, int tipomigra)
        {
            List<SiLogDTO> listaFinal = new List<SiLogDTO>();
            var lista = FactorySic.GetSiLogRepository().ListLogByMigracion(migracodi);

            foreach (var reg in lista)
            {
                int? tipoLog;
                string tipoLogDesc;
                string mensajeLog;
                GetNombreEstadoLogFromMensaje(reg.Logmigtipo, reg.LogDesc, reg.Miqubamensaje, reg.Miqubaflag, out tipoLog, out tipoLogDesc, out mensajeLog);

                if (tipoLog > 0)
                {
                    reg.LogDesc = mensajeLog;
                    reg.Logmigtipo = tipoLog;
                    reg.LogmigtipoDesc = tipoLogDesc;
                    reg.FechaDesc = reg.LogFecha != null ? reg.LogFecha.Value.ToString(ConstantesAppServicio.FormatoFecha) : string.Empty;
                    reg.HoraDesc = reg.LogFecha != null ? reg.LogFecha.Value.ToString(ConstantesAppServicio.FormatoHHmmss) : string.Empty;

                    listaFinal.Add(reg);
                }
            }

            var listaUltima = this.ListarLogNuevos(listaFinal, migracodi, tipomigra);
            return listaUltima;
        }

        public List<SiLogDTO> ListarLogNuevos(List<SiLogDTO> listaFinal, int migracodi, int tipomigra)
        {
            List<int?> listareturn = new List<int?>();
            listareturn.Add(null);

            var listaLogMigra = FactorySic.GetSiLogmigraRepository().List();
            //actualizar miqubacodi
            foreach (var item in listaFinal)
            {
                item.Miqubacodi = listaLogMigra.Find(x => x.Logcodi == item.LogCodi).Miqubacodi;
            }

            var lista = this.ListarLogDbaByMigracion(migracodi, tipomigra);
            foreach (var reg in lista)
            {
                if (reg.NroRegistros > 0)
                {
                    var listaMigraXTOperacion = FactorySic.GetSiMigraqueryxtipooperacionRepository().List();
                    int miqubacodi = listaMigraXTOperacion.Find(x => x.Mqxtopcodi == reg.Mqxtopcodi).Miqubacodi;
                    listareturn.Add(miqubacodi);
                }
            }


            listaFinal = listaFinal.Where(x => listareturn.Contains(x.Miqubacodi)).ToList();

            return listaFinal;
        }

        /// <summary>
        /// Obtiene el mensaje valido para mostrar en el reporte del log
        /// </summary>
        /// <param name="tipoIn"></param>
        /// <param name="mensajeIn"></param>
        /// <param name="mensajeQueryIn"></param>
        /// <param name="flag"></param>
        /// <param name="tipoOut"></param>
        /// <param name="tipoDescOut"></param>
        /// <param name="mensajeOut"></param>
        public void GetNombreEstadoLogFromMensaje(int? tipoIn, string mensajeIn, string mensajeQueryIn, int flag, out int? tipoOut, out string tipoDescOut, out string mensajeOut)
        {
            tipoOut = -1;
            tipoDescOut = string.Empty;
            mensajeOut = string.Empty;

            mensajeIn = mensajeIn != null ? mensajeIn : string.Empty;
            if (tipoIn > 0)
            {
                if (flag > 0 || tipoIn <= 2) //Solo mostrar en el log si esta activado el flag
                {
                    tipoOut = tipoIn;
                    tipoDescOut = GetNombreEstadoLog(tipoIn.Value);
                    mensajeOut = mensajeQueryIn;
                }
            }
            else
            {
                if (mensajeIn.ToUpper().Contains("HORA DE INICIO"))
                {
                    tipoOut = ConstantesTitularidad.TipoEventoLogInicio;
                }

                if (mensajeIn.ToUpper().Contains("HORA DE FIN"))
                {
                    tipoOut = ConstantesTitularidad.TipoEventoLogFin;
                }

                if (mensajeIn.ToUpper().Contains("NO SE HA PROCESADO"))
                {
                    tipoOut = ConstantesTitularidad.TipoEventoLogError;
                }

                if (mensajeIn.ToUpper().Contains("SE HA ACTUALIZADO") || mensajeIn.ToUpper().Contains("SE PROCESÓ CORRECTAMENTE"))
                {
                    tipoOut = ConstantesTitularidad.TipoEventoLogCorrecto;
                }

                tipoDescOut = GetNombreEstadoLog(tipoOut.Value);

                if (mensajeIn.Contains("]"))
                {
                    int idx = mensajeIn.IndexOf("]");
                    if (idx > -1)
                        mensajeOut = mensajeIn.Substring(idx + 1, mensajeIn.Length - idx - 1);
                }
                else
                {
                    if (mensajeIn.Contains(":"))
                    {
                        int idx = mensajeIn.IndexOf(":");
                        if (idx > -1)
                            mensajeOut = mensajeIn.Substring(0, idx);
                    }
                }
            }
        }

        /// <summary>
        /// Devuelve la descripcion del tipo del log
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public string GetNombreEstadoLog(int tipo)
        {
            if (tipo > 0)
            {
                switch (tipo)
                {
                    case ConstantesTitularidad.TipoEventoLogInicio:
                        return ConstantesTitularidad.TipoEventoLogInicioDesc;
                    case ConstantesTitularidad.TipoEventoLogFin:
                        return ConstantesTitularidad.TipoEventoLogFinDesc;
                    case ConstantesTitularidad.TipoEventoLogCorrecto:
                        return ConstantesTitularidad.TipoEventoLogCorrectoDesc;
                    case ConstantesTitularidad.TipoEventoLogError:
                        return ConstantesTitularidad.TipoEventoLogErrorDesc;
                }
            }

            return string.Empty;
        }

        #endregion

        #endregion

        #region Exportación Excel

        /// <summary>
        /// Generar reporte excel de las migraciones
        /// </summary>
        public void ExportarReporte()
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesTitularidad.FolderReporte;
            string rutaArchivo = ruta + ConstantesTitularidad.NombreReporte;

            List<SiMigracionDTO> listado = this.ListarTransferenciasXEmpresaOrigenXEmpresaDestino(-2, -2, string.Empty, ConstantesTitularidad.EliminadoLogicoNo);

            this.GenerarExcelListadoTransferencias(rutaArchivo, listado);
        }

        /// <summary>
        /// Hoja excel del listado de migraciones
        /// </summary>
        /// <param name="listaData"></param>
        private void GenerarExcelListadoTransferencias(string rutaArchivo, List<SiMigracionDTO> listaData)
        {
            FileInfo newFile = new FileInfo(rutaArchivo);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(ConstantesTitularidad.NombreHoja);
                ws.View.ShowGridLines = false;

                #region Hoja Reporte

                int row = 6;
                int column = 2;

                int rowTitulo = 2;
                ws.Cells[rowTitulo, column + 2].Value = ConstantesTitularidad.TituloHoja;
                ws.Cells[rowTitulo, column + 2].Style.Font.SetFromFont(new Font("Calibri", 14));
                ws.Cells[rowTitulo, column + 2].Style.Font.Bold = true;

                #endregion

                #region cabecera

                int rowIniCodigo = row;
                int colIniCodigo = 2;
                int colIniEmpresaOrig = colIniCodigo + 1;
                int colIniTipoOp = colIniEmpresaOrig + 1;
                int colIniFechaCorte = colIniTipoOp + 1;
                int colIniEmpresaDest = colIniFechaCorte + 1;
                int colIniTotEq = colIniEmpresaDest + 1;
                int colIniDescripcion = colIniTotEq + 1;
                int colIniLastuser = colIniDescripcion + 1;
                int colIniLastdate = colIniLastuser + 1;

                ws.Cells[row, colIniCodigo].Value = "ID Migración";
                ws.Cells[row, colIniEmpresaOrig].Value = "Empresa Origen";
                ws.Cells[row, colIniTipoOp].Value = "Tipo Operación";
                ws.Cells[row, colIniFechaCorte].Value = "Fecha de Corte";
                ws.Cells[row, colIniEmpresaDest].Value = "Empresa Destino";
                ws.Cells[row, colIniTotEq].Value = "#Eq afect.";
                ws.Cells[row, colIniDescripcion].Value = "Descripción";
                ws.Cells[row, colIniLastuser].Value = "Usuario";
                ws.Cells[row, colIniLastdate].Value = "Fecha";

                using (var range = ws.Cells[row, colIniCodigo, row, colIniLastdate])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                }

                ws.Cells[row, colIniEmpresaOrig].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#43a243"));
                ws.Cells[row, colIniEmpresaDest].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#fd4444"));

                #endregion

                #region cuerpo
                row++;
                foreach (var item in listaData)
                {
                    ws.Cells[row, colIniCodigo].Value = item.Migracodi;
                    ws.Cells[row, colIniCodigo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row, colIniCodigo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    ws.Cells[row, colIniEmpresaOrig].Value = item.Emprnomborigen;
                    ws.Cells[row, colIniEmpresaOrig].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells[row, colIniTipoOp].Value = item.Tmoperdescripcion;
                    ws.Cells[row, colIniTipoOp].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row, colIniTipoOp].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    ws.Cells[row, colIniFechaCorte].Value = item.MigrafeccorteDesc;
                    ws.Cells[row, colIniFechaCorte].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row, colIniFechaCorte].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    ws.Cells[row, colIniEmpresaDest].Value = item.Emprnombdestino;
                    ws.Cells[row, colIniEmpresaDest].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    if (!string.IsNullOrEmpty(item.TotalDesc))
                        ws.Cells[row, colIniTotEq].Value = Int32.Parse(item.TotalDesc);
                    ws.Cells[row, colIniTotEq].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row, colIniTotEq].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    ws.Cells[row, colIniDescripcion].Value = item.Migradescripcion;
                    ws.Cells[row, colIniDescripcion].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells[row, colIniLastuser].Value = item.UltimaModificacionUsuarioDesc;
                    ws.Cells[row, colIniLastuser].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row, colIniLastuser].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    ws.Cells[row, colIniLastdate].Value = item.UltimaModificacionFechaDesc;
                    ws.Cells[row, colIniLastdate].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row, colIniLastdate].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    row++;
                }

                ws.Column(1).Width = 3;
                ws.Column(colIniCodigo).Width = 13;
                ws.Column(colIniEmpresaOrig).Width = 50;
                ws.Column(colIniTipoOp).Width = 42;
                ws.Column(colIniFechaCorte).Width = 20;
                ws.Column(colIniEmpresaDest).Width = 50;
                ws.Column(colIniTotEq).Width = 13;
                ws.Column(colIniDescripcion).Width = 80;
                ws.Column(colIniLastuser).Width = 18;
                ws.Column(colIniLastdate).Width = 18;

                #endregion

                #region logo

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
                picture.From.Column = 1;
                picture.From.Row = 1;

                #endregion

                ws.View.FreezePanes(rowIniCodigo + 1, colIniCodigo + 1);

                xlPackage.Save();
            }
        }

        #endregion

        #region Herramienta de creación de Querybase

        public List<SiMigraquerybaseDTO> ListarQuerybase(List<int> listaTipoOp, int flagStr, int flagActivo)
        {
            //querys
            List<SiMigraquerybaseDTO> listaQueryBase = GetByCriteriaSiMigraquerybases();

            //querys y sus tipos de operación
            List<SiMigraqueryxtipooperacionDTO> listaAllQueryXTipo = GetByCriteriaSiMigraqueryxtipooperacions(-1).Where(x => x.Mqxtopactivo == ConstantesTitularidad.FlagActivo).ToList();

            //
            //List<int> listaMiqubacodi = listaAllQueryXTipo.Select(x => x.Miqubacodi).Distinct().ToList();
            //List<SiMigraquerybaseDTO> listaQueryBaseActivo = listaQueryBase.OrderBy(x => x.Miqubanomtabla).Where(x => listaMiqubacodi.Contains(x.Miqubacodi)).ToList();

            foreach (var reg in listaQueryBase)
            {
                var listaTmpQuery = listaAllQueryXTipo.Where(x => x.Miqubacodi == reg.Miqubacodi).ToList();

                reg.TieneTmopercodiDupl = listaTmpQuery.Find(x => x.Tmopercodi == ConstantesTitularidad.TipoMigrDuplicidad) != null;
                reg.TieneTmopercodiEqAEmp = listaTmpQuery.Find(x => x.Tmopercodi == ConstantesTitularidad.TipoMigrTransferenciaEquipos) != null;
                reg.TieneTmopercodiEqNoEmp = listaTmpQuery.Find(x => x.Tmopercodi == ConstantesTitularidad.TipoMigrInstalacionNoCorresponden) != null;
                reg.TieneTmopercodiFs = listaTmpQuery.Find(x => x.Tmopercodi == ConstantesTitularidad.TipoMigrFusion) != null;
                reg.TieneTmopercodiRs = listaTmpQuery.Find(x => x.Tmopercodi == ConstantesTitularidad.TipoMigrCambioRazonSocial) != null;

                reg.MiqubafeccreacionDesc = reg.Miqubafeccreacion.ToString(ConstantesAppServicio.FormatoFechaFull);
                reg.MiqubaflagDesc = reg.Miqubaflag == ConstantesTitularidad.FlagActivo ? ConstantesAppServicio.SIDesc : string.Empty;
                reg.Miqplanomb = reg.Miqplanomb ?? string.Empty;

                if (listaTipoOp.Contains(-1)
                    || (listaTipoOp.Contains(ConstantesTitularidad.TipoMigrDuplicidad) && reg.TieneTmopercodiDupl)
                    || (listaTipoOp.Contains(ConstantesTitularidad.TipoMigrTransferenciaEquipos) && reg.TieneTmopercodiEqAEmp)
                    || (listaTipoOp.Contains(ConstantesTitularidad.TipoMigrInstalacionNoCorresponden) && reg.TieneTmopercodiEqNoEmp)
                    || (listaTipoOp.Contains(ConstantesTitularidad.TipoMigrFusion) && reg.TieneTmopercodiFs)
                    || (listaTipoOp.Contains(ConstantesTitularidad.TipoMigrCambioRazonSocial) && reg.TieneTmopercodiRs))
                    reg.TieneFiltroTipoOp = true;
            }

            //
            listaQueryBase = listaQueryBase.Where(x => x.TieneFiltroTipoOp).ToList();
            if (flagActivo == 1)
                listaQueryBase = listaQueryBase.Where(x => x.Miqubaactivo == ConstantesTitularidad.FlagActivo).ToList();
            if (flagStr == 1)
                listaQueryBase = listaQueryBase.Where(x => x.Miqubastr == ConstantesAppServicio.SI).ToList();

            return listaQueryBase.OrderBy(x => x.Miqubastr).ThenBy(x => x.Miqubanomtabla).ThenBy(x => x.TieneTmopercodiDupl).ToList();
        }

        public List<SiMigraqueryplantillaDTO> ListarPlantillaQuery()
        {
            //plantillas
            List<SiMigraqueryplantillaDTO> listaPlantilla = GetByCriteriaSiMigraqueryplantillas();

            //plantillas y sus tipos de operación
            List<SiMigraqueryplanttopDTO> listaAllPlantillaXTipo = GetByCriteriaSiMigraqueryplanttops(-1).Where(x => x.Miptopactivo == ConstantesTitularidad.FlagActivo).ToList();

            //
            foreach (var reg in listaPlantilla)
            {
                var listaTmpPlantilla = listaAllPlantillaXTipo.Where(x => x.Miqplacodi == reg.Miqplacodi).ToList();

                reg.TieneTmopercodiDupl = listaTmpPlantilla.Find(x => x.Tmopercodi == ConstantesTitularidad.TipoMigrDuplicidad) != null;
                reg.TieneTmopercodiEqAEmp = listaTmpPlantilla.Find(x => x.Tmopercodi == ConstantesTitularidad.TipoMigrTransferenciaEquipos) != null;
                reg.TieneTmopercodiEqNoEmp = listaTmpPlantilla.Find(x => x.Tmopercodi == ConstantesTitularidad.TipoMigrInstalacionNoCorresponden) != null;
                reg.TieneTmopercodiFs = listaTmpPlantilla.Find(x => x.Tmopercodi == ConstantesTitularidad.TipoMigrFusion) != null;
                reg.TieneTmopercodiRs = listaTmpPlantilla.Find(x => x.Tmopercodi == ConstantesTitularidad.TipoMigrCambioRazonSocial) != null;
            }

            return listaPlantilla.OrderBy(x => x.Miqplacodi).ToList();
        }

        public string GenerarReporteListadoQuery(string url, string tipoOp, int flagStr, int flagActivo)
        {
            List<int> listaTipoOp = tipoOp.Split(',').ToList().Select(x => Int32.Parse(x)).ToList();

            List<SiMigraquerybaseDTO> lista = ListarQuerybase(listaTipoOp, flagStr, flagActivo);

            StringBuilder str = new StringBuilder();
            str.Append(@"
                <table class='pretty tabla-adicional' border='0' cellspacing='0' width='100%' id='tabla_query'>
                    <thead>
                        <tr>
                            <th style='width: 10px'>Opciones</th>
                            <th style=''>Código</th>
                            <th style=''>Tabla</th>
                            <th style=''>Plantilla</th>

                            <th style=''>Duplicidad <br> de Empresas</th>
                            <th style=''>Equipos <br> no corresponden </th>
                            <th style=''>Razón <br> Social </th>
                            <th style=''>Fusión <br> de empresas </th>
                            <th style=''> Transferencia <br> equipos </th>

                            <th style=''>¿Mostrar log <br/> a SGI?</th>
                            <th style=''>¿Aplicativo <br/> STR?</th>
                            <th style=''>Usuario</th>
                            <th style=''>Fecha de registro</th>
                        </tr>
                    </thead>
                    <tbody>
            ");

            foreach (var reg in lista)
            {
                str.AppendFormat(@"
                        <tr class='{12}'>
                            <td>
                                <a class='' href='JavaScript:editarQuery({1})' style='margin-right: 4px;'>
                                    <img style='margin-top: 4px; margin-bottom: 4px;' src='{0}Content/Images/btn-edit.png' title='Editar' />
                                </a>
                            </td>
                            <td class='' style='text-align: center'>{1}</td>
                            <td class='' style='text-align: center'>{2}</td>
                            <td class='' style=''>{13}</td>

                            <td class='' style='text-align: center'>{6}</td>
                            <td class='' style='text-align: center'>{7}</td>
                            <td class='' style='text-align: center'>{8}</td>
                            <td class='' style='text-align: center'>{9}</td>
                            <td class='' style='text-align: center'>{10}</td>

                            <td class='' style='text-align: center'>{3}</td>
                            <td class='' style='text-align: center'>{11}</td>
                            <td class='' style='text-align: center'>{4}</td>
                            <td class='' style='text-align: center'>{5}</td>
                        </tr>
                ", url, reg.Miqubacodi, reg.Miqubanomtabla
                , reg.MiqubaflagDesc, reg.Miqubausucreacion, reg.MiqubafeccreacionDesc
                , DescripcionFlagTipoOperacion(reg.TieneTmopercodiDupl), DescripcionFlagTipoOperacion(reg.TieneTmopercodiEqNoEmp)
                , DescripcionFlagTipoOperacion(reg.TieneTmopercodiRs), DescripcionFlagTipoOperacion(reg.TieneTmopercodiFs), DescripcionFlagTipoOperacion(reg.TieneTmopercodiEqAEmp)
                , reg.Miqubastr == ConstantesAppServicio.SI ? "STR" : string.Empty, reg.Miqubaactivo != ConstantesTitularidad.FlagActivo ? "tr_inactivo" : string.Empty
                , reg.Miqplanomb);
            }

            str.Append("</tbody>");
            str.Append("</table>");

            return str.ToString();
        }

        public string GenerarReporteListadoPlantilla(string url)
        {
            List<SiMigraqueryplantillaDTO> lista = ListarPlantillaQuery();

            StringBuilder str = new StringBuilder();
            str.Append(@"
                <table class='pretty tabla-adicional' border='0' cellspacing='0' id='tabla_plantilla' style='width: 1000px;'>
                    <thead>
                        <tr>
                            <th style='width: 50px'>Opciones</th>
                            <th style='width: 40px'>Código</th>
                            <th style=''>Plantilla</th>

                            <th style=''>Duplicidad <br> de Empresas</th>
                            <th style=''>Equipos <br> no corresponden </th>
                            <th style=''>Razón <br> Social </th>
                            <th style=''>Fusión <br> de empresas </th>
                            <th style=''> Transferencia <br> equipos </th>

                            <th style=''>Usuario</th>
                            <th style=''>Fecha de registro</th>
                        </tr>
                    </thead>
                    <tbody>
            ");

            foreach (var reg in lista)
            {
                str.AppendFormat(@"
                        <tr>
                            <td>
                                <a class='' href='JavaScript:editarPlantilla({1})' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;' src='{0}Content/Images/btn-edit.png' title='Editar registro' /></a>
                            </td>
                            <td class='' style='text-align: center'>{1}</td>
                            <td class='' style=''>{2}</td>

                            <td class='' style='text-align: center'>{3}</td>
                            <td class='' style='text-align: center'>{4}</td>
                            <td class='' style='text-align: center'>{5}</td>
                            <td class='' style='text-align: center'>{6}</td>
                            <td class='' style='text-align: center'>{7}</td>

                            <td class='' style='text-align: center'>{8}</td>
                            <td class='' style='text-align: center'>{9}</td>
                        </tr>
                ", url, reg.Miqplacodi, reg.Miqplanomb
                , DescripcionFlagTipoOperacion(reg.TieneTmopercodiDupl), DescripcionFlagTipoOperacion(reg.TieneTmopercodiEqNoEmp)
                , DescripcionFlagTipoOperacion(reg.TieneTmopercodiRs), DescripcionFlagTipoOperacion(reg.TieneTmopercodiFs), DescripcionFlagTipoOperacion(reg.TieneTmopercodiEqAEmp)
                , reg.Miqplausucreacion, reg.MiqplafeccreacionDesc);
            }

            str.Append("</tbody>");
            str.Append("</table>");

            return str.ToString();
        }

        public string GenerarReporteListadoParametro(string url)
        {
            List<SiMigraParametroDTO> lista = GetByCriteriaSiMigraParametros();

            StringBuilder str = new StringBuilder();
            str.Append(@"
                <table class='pretty tabla-adicional' border='0' cellspacing='0' id='tabla_parametro' style='width: 500px;'>
                    <thead>
                        <tr>
                            <th style='width: 10px'>Opciones</th>
                            <th style=''>Código</th>                       
                            <th style=''>Tipo</th>                            
                            <th style=''>Parámetro</th>
                            <th style=''>Usuario</th>
                            <th style=''>Fecha de registro</th>
                        </tr>
                    </thead>
                    <tbody>
            ");

            foreach (var reg in lista)
            {
                str.AppendFormat(@"
                        <tr>
                            <td>
                                <a class='' href='JavaScript:editarParametro({1})' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;' src='{0}Content/Images/btn-edit.png' title='Editar registro' /></a>
                            </td>
                            <td class='' style='text-align: left'>{1}</td>
                            <td class='' style='text-align: left'>{5}</td>
                            <td class='' style='text-align: left'>{2}</td>
                            <td class='' style='text-align: center'>{3}</td>
                            <td class='' style='text-align: center'>{4}</td>
                        </tr>
                ", url, reg.Migparcodi, reg.Migparnomb, reg.Migparusucreacion, reg.MigparfeccreacionDesc, reg.MigpartipoDesc);
            }

            str.Append("</tbody>");
            str.Append("</table>");

            return str.ToString();
        }

        private string DescripcionFlagTipoOperacion(bool flag)
        {
            return flag ? ConstantesAppServicio.SIDesc : string.Empty;
        }

        public void GuardarQuery(SiMigraquerybaseDTO reg)
        {
            DateTime fechaRegistro = DateTime.Now;

            #region Parámetros de la query

            List<SiMigraqueryparametroDTO> listaParamXQuery = new List<SiMigraqueryparametroDTO>();
            List<SiMigraqueryparametroDTO> listaParamXQueryBD = new List<SiMigraqueryparametroDTO>();
            if (reg.Miqubacodi > 0)
            {
                listaParamXQueryBD = GetByCriteriaSiMigraqueryparametros(reg.Miqubacodi);
            }

            //recorrer los parametros y agregar los que no estan en bd
            List<SiMigraParametroDTO> listaParam = GetByCriteriaSiMigraParametros();
            foreach (var param in listaParam)
            {
                var regBd = listaParamXQueryBD.Find(x => x.Migparcodi == param.Migparcodi);
                if (regBd != null)
                {
                    listaParamXQuery.Add(regBd);
                }
                else
                {
                    listaParamXQuery.Add(new SiMigraqueryparametroDTO()
                    {
                        Migparcodi = param.Migparcodi
                    });
                }
            }


            //
            foreach (var param in listaParamXQuery)
            {
                var regWeb = reg.ListaParametroValor.Find(x => x.Migparcodi == param.Migparcodi);

                int activo = regWeb != null && regWeb.Mgqparactivo == 1 ? 1 : 0;
                string valorParam = ((regWeb?.Mgqparvalor) ?? "").Trim();

                if (param.Mgqparcodi <= 0
                    || (param.Mgqparcodi > 0 && (param.Mgqparactivo != activo || (param.Mgqparvalor ?? "").Trim() != valorParam)))
                {
                    param.Mgqparusucreacion = reg.Miqubausucreacion;
                    param.Mgqparfeccreacion = fechaRegistro;
                    param.Mgqparactivo = activo;
                    param.Mgqparvalor = valorParam;
                }

            }

            reg.Miqubafeccreacion = fechaRegistro;
            reg.ListaParametroValor = listaParamXQuery.Where(x => x.Mgqparcodi > 0 || x.Mgqparactivo == 1).ToList();

            #endregion

            #region Tipos de operación de la query

            List<SiMigraqueryxtipooperacionDTO> listaTipoOpXQuery = new List<SiMigraqueryxtipooperacionDTO>();
            List<SiMigraqueryxtipooperacionDTO> listaTipoOpXQueryBD = new List<SiMigraqueryxtipooperacionDTO>();
            if (reg.Miqubacodi > 0)
            {
                listaTipoOpXQueryBD = GetByCriteriaSiMigraqueryxtipooperacions(reg.Miqubacodi);
            }

            //recorrer los tipos de operacion y agregar los que no estan en bd
            List<SiTipomigraoperacionDTO> listaTipoOp = ListSiTipomigraoperacions();
            foreach (var tipoOp in listaTipoOp)
            {
                var regBd = listaTipoOpXQueryBD.Find(x => x.Tmopercodi == tipoOp.Tmopercodi);
                if (regBd != null)
                {
                    listaTipoOpXQuery.Add(regBd);
                }
                else
                {
                    listaTipoOpXQuery.Add(new SiMigraqueryxtipooperacionDTO()
                    {
                        Tmopercodi = tipoOp.Tmopercodi
                    });
                }
            }


            //
            foreach (var tipoOp in listaTipoOpXQuery)
            {
                var regBd = reg.ListaTipoOpValor.Find(x => x.Tmopercodi == tipoOp.Tmopercodi);

                int activo = regBd != null && regBd.Mqxtopactivo == 1 ? 1 : 0;

                if (tipoOp.Mqxtopcodi <= 0
                    || (tipoOp.Mqxtopcodi > 0 && tipoOp.Mqxtopactivo != activo))
                {
                    tipoOp.Mqxtopusucreacion = reg.Miqubausucreacion;
                    tipoOp.Mqxtopfeccreacion = fechaRegistro;
                    tipoOp.Mqxtopactivo = activo;
                }
            }

            reg.Miqubafeccreacion = fechaRegistro;
            reg.ListaTipoOpValor = listaTipoOpXQuery.Where(x => x.Mqxtopcodi > 0 || x.Mqxtopactivo == 1).ToList();

            #endregion

            #region Transaccional

            DbTransaction tran = null;
            IDbConnection conn = null;

            try
            {
                conn = FactorySic.GetSiMigracionRepository().BeginConnection();
                tran = FactorySic.GetSiMigracionRepository().StartTransaction(conn);

                if (reg.Miqubacodi > 0)
                    UpdateSiMigraquerybase(reg, conn, tran);
                else
                    reg.Miqubacodi = SaveSiMigraquerybase(reg, conn, tran);

                //parametros
                int idParam = FactorySic.GetSiMigraqueryparametroRepository().GetMaxId();
                foreach (var param in reg.ListaParametroValor)
                {
                    param.Miqubacodi = reg.Miqubacodi;
                    if (param.Mgqparcodi > 0)
                        UpdateSiMigraqueryparametro(param, conn, tran);
                    else
                    {
                        param.Mgqparcodi = idParam;
                        SaveSiMigraqueryparametro(param, conn, tran);
                        idParam++;
                    }
                }

                //tipos de operacion
                int idTipoop = FactorySic.GetSiMigraqueryxtipooperacionRepository().GetMaxId();
                foreach (var tipoOp in reg.ListaTipoOpValor)
                {
                    tipoOp.Miqubacodi = reg.Miqubacodi;
                    if (tipoOp.Mqxtopcodi > 0)
                        UpdateSiMigraqueryxtipooperacion(tipoOp, conn, tran);
                    else
                    {
                        tipoOp.Mqxtopcodi = idTipoop;
                        SaveSiMigraqueryxtipooperacion(tipoOp, conn, tran);
                        idTipoop++;
                    }
                }

                tran.Commit();
            }
            catch (Exception ex)
            {
                if (tran != null)
                    tran.Rollback();
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
            }

            #endregion
        }

        public void GuardarPlantilla(SiMigraqueryplantillaDTO reg)
        {
            DateTime fechaRegistro = DateTime.Now;

            #region Parámetros de la plantilla

            List<SiMigraqueryplantparamDTO> listaParamXPlan = new List<SiMigraqueryplantparamDTO>();
            List<SiMigraqueryplantparamDTO> listaParamXPlanBD = new List<SiMigraqueryplantparamDTO>();
            if (reg.Miqplacodi > 0)
            {
                listaParamXPlanBD = GetByCriteriaSiMigraqueryplantparams(reg.Miqplacodi);
            }

            //recorrer los parametros y agregar los que no estan en bd
            List<SiMigraParametroDTO> listaParam = GetByCriteriaSiMigraParametros();
            foreach (var param in listaParam)
            {
                var regBd = listaParamXPlanBD.Find(x => x.Migparcodi == param.Migparcodi);
                if (regBd != null)
                {
                    listaParamXPlan.Add(regBd);
                }
                else
                {
                    listaParamXPlan.Add(new SiMigraqueryplantparamDTO()
                    {
                        Migparcodi = param.Migparcodi
                    });
                }
            }

            //
            foreach (var param in listaParamXPlan)
            {
                int activo = reg.ListaParametro.Find(x => x.Migparcodi == param.Migparcodi && x.Miplpractivo == 1) != null ? 1 : 0;

                if (param.Miplprcodi <= 0
                    || (param.Miplprcodi > 0 && param.Miplpractivo != activo))
                {
                    param.Miplprusucreacion = reg.Miqplausucreacion;
                    param.Miplprfeccreacion = fechaRegistro;
                    param.Miplpractivo = activo;
                }

            }

            reg.Miqplafeccreacion = fechaRegistro;
            reg.ListaParametro = listaParamXPlan;

            #endregion

            #region Tipos de operación de la plantilla

            List<SiMigraqueryplanttopDTO> listaTipoOpXQuery = new List<SiMigraqueryplanttopDTO>();
            List<SiMigraqueryplanttopDTO> listaTipoOpXQueryBD = new List<SiMigraqueryplanttopDTO>();
            if (reg.Miqplacodi > 0)
            {
                listaTipoOpXQueryBD = GetByCriteriaSiMigraqueryplanttops(reg.Miqplacodi);
            }

            //recorrer los tipos de operacion y agregar los que no estan en bd
            List<SiTipomigraoperacionDTO> listaTipoOp = ListSiTipomigraoperacions();
            foreach (var tipoOp in listaTipoOp)
            {
                var regBd = listaTipoOpXQueryBD.Find(x => x.Tmopercodi == tipoOp.Tmopercodi);
                if (regBd != null)
                {
                    listaTipoOpXQuery.Add(regBd);
                }
                else
                {
                    listaTipoOpXQuery.Add(new SiMigraqueryplanttopDTO()
                    {
                        Tmopercodi = tipoOp.Tmopercodi
                    });
                }
            }

            //
            foreach (var tipoOp in listaTipoOpXQuery)
            {
                var regBd = reg.ListaTipoOp.Find(x => x.Tmopercodi == tipoOp.Tmopercodi);

                int activo = regBd != null && regBd.Miptopactivo == 1 ? 1 : 0;

                if (tipoOp.Miptopcodi <= 0
                    || (tipoOp.Miptopcodi > 0 && tipoOp.Miptopactivo != activo))
                {
                    tipoOp.Miptopusucreacion = reg.Miqplausucreacion;
                    tipoOp.Miptopfeccreacion = fechaRegistro;
                    tipoOp.Miptopactivo = activo;
                }

            }

            reg.Miqplafeccreacion = fechaRegistro;
            reg.ListaTipoOp = listaTipoOpXQuery;

            #endregion

            #region Transaccional

            DbTransaction tran = null;
            IDbConnection conn = null;

            try
            {
                conn = FactorySic.GetSiMigracionRepository().BeginConnection();
                tran = FactorySic.GetSiMigracionRepository().StartTransaction(conn);

                if (reg.Miqplacodi > 0)
                    UpdateSiMigraqueryplantilla(reg, conn, tran);
                else
                    reg.Miqplacodi = SaveSiMigraqueryplantilla(reg, conn, tran);

                //parametros de la plantilla
                int idParam = FactorySic.GetSiMigraqueryplantparamRepository().GetMaxId();
                foreach (var param in reg.ListaParametro)
                {
                    param.Miqplacodi = reg.Miqplacodi;
                    if (param.Miplprcodi > 0)
                        UpdateSiMigraqueryplantparam(param, conn, tran);
                    else
                    {
                        param.Miplprcodi = idParam;
                        SaveSiMigraqueryplantparam(param, conn, tran);
                        idParam++;
                    }
                }

                //tipos de operación de la plantilla
                int idTipoOp = FactorySic.GetSiMigraqueryplanttopRepository().GetMaxId();
                foreach (var tipoOp in reg.ListaTipoOp)
                {
                    tipoOp.Miqplacodi = reg.Miqplacodi;
                    if (tipoOp.Miptopcodi > 0)
                        UpdateSiMigraqueryplanttop(tipoOp, conn, tran);
                    else
                    {
                        tipoOp.Miptopcodi = idTipoOp;
                        SaveSiMigraqueryplanttop(tipoOp, conn, tran);
                        idTipoOp++;
                    }
                }

                tran.Commit();
            }
            catch (Exception ex)
            {
                if (tran != null)
                    tran.Rollback();
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
            }

            #endregion
        }

        public void GuardarParametro(SiMigraParametroDTO reg)
        {
            reg.Migparfeccreacion = DateTime.Now;
            if (reg.Migparcodi > 0)
                UpdateSiMigraparametro(reg);
            else
                SaveSiMigraparametro(reg);
        }

        #endregion

        #region Titularidad para tablas (ME_MEDICION48, ME_MEDICION96, EVE_MANTTO, EQ_EQUIPO)

        /// <summary>
        /// Por cada registro de M48 indicar la empresa actual para efectos de reportes
        /// </summary>
        /// <param name="lista96"></param>
        /// <param name="listaHistEq"></param>
        public void SetTTIEequipoCentralToM48(List<MeMedicion48DTO> lista48, List<SiHisempeqDataDTO> listaHist)
        {
            foreach (var reg in lista48)
            {
                SiHisempeqDataDTO regHist = this.GetUltimoEqFromListaHist(listaHist, reg.Equipadre, 0);
                if (regHist != null)
                {
                    reg.Equipadre = regHist.Equicodi;
                    reg.Emprcodi = regHist.Emprcodi;
                }

                SiHisempeqDataDTO regHisteq = this.GetUltimoEqFromListaHist(listaHist, reg.Equicodi, 0);
                if (regHisteq != null)
                {
                    reg.Equicodi = regHisteq.Equicodi;
                    reg.Emprcodi = regHisteq.Emprcodi;
                }
            }
        }

        /// <summary>
        /// Por cada registro de M96 indicar la empresa actual para efectos de reportes
        /// </summary>
        /// <param name="lista96"></param>
        /// <param name="listaHistEq"></param>
        public void SetTTIEequipoCentralToM96(List<MeMedicion96DTO> lista96, List<SiHisempeqDataDTO> listaHist)
        {
            foreach (var reg in lista96)
            {
                SiHisempeqDataDTO regHist = this.GetUltimoEqFromListaHist(listaHist, reg.Equipadre, 0);
                if (regHist != null)
                {
                    reg.Equipadre = regHist.Equicodi;
                    reg.Emprcodi = regHist.Emprcodi;
                }

                SiHisempeqDataDTO regHisteq = this.GetUltimoEqFromListaHist(listaHist, reg.Equicodi, 0);
                if (regHisteq != null)
                {
                    reg.Equicodi = regHisteq.Equicodi;
                    reg.Emprcodi = regHisteq.Emprcodi;
                }
            }
        }

        /// <summary>
        /// Por cada registro de Evemantto indicar la empresa actual para efectos de reportes
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="listaHist"></param>
        public void SetTTIEequipoToEveMantto(List<EveManttoDTO> lista, List<SiHisempeqDataDTO> listaHist)
        {
            foreach (var reg in lista)
            {
                SiHisempeqDataDTO regHist = this.GetUltimoEqFromListaHist(listaHist, reg.Equipadre ?? 0, 0);
                if (regHist != null)
                {
                    reg.Emprcodi = regHist.Emprcodi;
                    reg.Equipadre = regHist.Equicodi;
                }

                SiHisempeqDataDTO regHisteq = this.GetUltimoEqFromListaHist(listaHist, reg.Equicodi ?? 0, 0);
                if (regHisteq != null)
                {
                    reg.Emprcodi = regHisteq.Emprcodi;
                    reg.Equicodi = regHisteq.Equicodi;
                }
            }
        }

        /// <summary>
        /// Por cada registro de equipos indicar la empresa actual para efectos de reportes
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="listaHist"></param>
        public List<EqEquipoDTO> SetTTIEequipoToEqEquipo(List<EqEquipoDTO> lista, List<SiHisempeqDataDTO> listaHist)
        {
            foreach (var reg in lista)
            {
                //si el equipo es un generador entonces actualizar el equipadre
                List<int> lFamcodiPadre = new List<int>() { 4, 5, 37, 39 };
                if (!lFamcodiPadre.Contains(reg.Famcodi ?? 0))
                {
                    SiHisempeqDataDTO regHist = this.GetUltimoEqFromListaHist(listaHist, reg.Equipadre ?? 0, 0);
                    if (regHist != null)
                    {
                        reg.Equipadre = regHist.Equicodi;
                        reg.Emprcodi = regHist.Emprcodi;
                    }
                }

                SiHisempeqDataDTO regHisteq = this.GetUltimoEqFromListaHist(listaHist, reg.Equicodi, 0);
                if (regHisteq != null)
                {
                    if (reg.Equicodi != regHisteq.Equicodi || reg.Emprcodi != regHisteq.Emprcodi) reg.TipoOperacion = 1;

                    reg.Emprcodi = regHisteq.Emprcodi;
                    reg.Emprnomb = regHisteq.Emprnomb;
                    reg.Equicodi = regHisteq.Equicodi;
                }
            }

            //un equipo puede tener varios TTIEs, obtener la configuración más reciente a la fecha de consulta
            lista = lista.OrderBy(x => x.Equicodi).ThenBy(x => x.TipoOperacion).ToList();

            List<EqEquipoDTO> listaFinal = new List<EqEquipoDTO>();
            foreach (var reg in lista.GroupBy(x => x.Equicodi))
            {
                var eq = lista.Find(x => x.Equicodi == reg.Key);
                listaFinal.Add(eq);
            }

            return listaFinal;
        }

        /// <summary>
        /// Por cada registro de M48 indicar la empresa actual para efectos de reportes
        /// </summary>
        /// <param name="lista48"></param>
        /// <param name="listaHistPto"></param>
        public void SetTTIEptoToM48(List<MeMedicion48DTO> lista48, List<SiHisempptoDataDTO> listaHist)
        {
            foreach (var reg in lista48)
            {
                SiHisempptoDataDTO regHist = this.GetUltimoPtoFromListaHist(listaHist, reg.Ptomedicodi, 0);
                if (regHist != null)
                {
                    reg.Emprcodi = regHist.Emprcodi;
                    reg.Emprnomb = regHist.Emprnomb;
                }
            }
        }

        /// <summary>
        /// Por cada registro de M96 indicar la empresa actual para efectos de reportes
        /// </summary>
        /// <param name="lista96"></param>
        /// <param name="listaHistPto"></param>
        public void SetTTIEptoToM96(List<MeMedicion96DTO> lista96, List<SiHisempptoDataDTO> listaHist)
        {
            foreach (var reg in lista96)
            {
                SiHisempptoDataDTO regHist = this.GetUltimoPtoFromListaHist(listaHist, reg.Ptomedicodi, 0);
                if (regHist != null)
                {
                    reg.Emprcodi = regHist.Emprcodi;
                    reg.Emprnomb = regHist.Emprnomb;
                }
            }
        }

        public void SetTTIEptoToMePtomedicion(List<MePtomedicionDTO> listaPto, List<SiHisempptoDataDTO> listaHist)
        {
            foreach (var reg in listaPto)
            {
                SiHisempptoDataDTO regHist = this.GetUltimoPtoFromListaHist(listaHist, reg.Ptomedicodi, 0);
                if (regHist != null)
                {
                    reg.Emprcodi = regHist.Emprcodi;
                    reg.Emprnomb = regHist.Emprnomb;
                }
            }
        }

        /// <summary>
        /// Obtener el cambio de TTIE del punto más actual segun el historico que está de parametro
        /// </summary>
        /// <param name="listaHist"></param>
        /// <param name="ptomedicodi"></param>
        /// <param name="contRecursivo"></param>
        /// <returns></returns>
        private SiHisempptoDataDTO GetUltimoPtoFromListaHist(List<SiHisempptoDataDTO> listaHist, int ptomedicodi, int contRecursivo)
        {
            var regHist = listaHist.Where(x => x.Ptomedicodiold == ptomedicodi)
                .OrderByDescending(x => x.Hptdatfecha).ThenByDescending(x => x.Hptdatptoestado).FirstOrDefault();

            if (regHist == null)
                return listaHist.Where(x => x.Ptomedicodi == ptomedicodi)
                .OrderByDescending(x => x.Hptdatfecha).ThenByDescending(x => x.Hptdatptoestado).FirstOrDefault();

            if (ConstantesTitularidad.MaxIteracionRecursivo <= contRecursivo)
            {
                return regHist;
            }

            if (regHist != null && regHist.Ptomedicodi != regHist.Ptomedicodiactual)
            {
                return this.GetUltimoPtoFromListaHist(listaHist, regHist.Ptomedicodi, contRecursivo + 1);
            }

            return regHist;
        }

        /// <summary>
        /// Obtener el cambio de TTIE del equipo más actual segun el historico que está de parametro
        /// </summary>
        /// <param name="listaHist"></param>
        /// <param name="equicodi"></param>
        /// <param name="contRecursivo"></param>
        /// <returns></returns>
        private SiHisempeqDataDTO GetUltimoEqFromListaHist(List<SiHisempeqDataDTO> listaHist, int equicodi, int contRecursivo)
        {
            var regHist = listaHist.Where(x => x.Equicodiactual == equicodi)
                .OrderByDescending(x => x.Heqdatfecha).ThenByDescending(x => x.Heqdatestado).FirstOrDefault();

            if (ConstantesTitularidad.MaxIteracionRecursivo <= contRecursivo)
            {
                return regHist;
            }

            if (regHist != null && regHist.Equicodi != regHist.Equicodiactual)
            {
                return this.GetUltimoEqFromListaHist(listaHist, regHist.Equicodi, contRecursivo + 1);
            }

            return regHist;
        }
        #endregion
    }
}
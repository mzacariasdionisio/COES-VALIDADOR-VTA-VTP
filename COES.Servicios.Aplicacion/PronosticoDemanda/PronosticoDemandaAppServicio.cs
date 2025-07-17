using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Globalization;
using log4net.Repository.Hierarchy;
using log4net;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.ServicioRPF;
using System.Data;
using COES.Servicios.Aplicacion.ReportesMedicion.Helper;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using Newtonsoft.Json;
using COES.Framework.Base.Tools;

namespace COES.Servicios.Aplicacion.PronosticoDemanda
{
    public class PronosticoDemandaAppServicio : AppServicioBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PronosticoDemandaAppServicio));

        /// <summary>
        /// Método que permite registrar la información necesaria para el sistema
        /// </summary>
        public void JobProdem()
        {
            //Obtiene las variables exógenas
            this.ObtenerVariablesExogenas();

            //Obtiene el despacho ejecutado por centrales
            string strFecha = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            DateTime parseFecha = DateTime.ParseExact(strFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            this.GetDatosDespacho(parseFecha.AddDays(-1));
        }

        #region Métodos del Módulo de Configuración - Parámetros

        /// <summary>
        /// Registra (Save y Update) los nuevos parámetros de configuración
        /// </summary>
        /// <param name="fecDesde"></param>
        /// <param name="fecHasta"></param>
        /// <param name="dataParametros"></param>
        /// <param name="listPuntos"></param>
        /// <param name="nomUsuario"></param>
        public object ParametrosSave(DateTime fecDesde, DateTime fecHasta, PrnConfiguracionDTO dataParametros,
            List<PrnClasificacionDTO> listPuntos, string nomUsuario)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;
            PrnConfiguracionDTO entity = new PrnConfiguracionDTO();

            if (listPuntos.Count != 0)
            {
                foreach (var ePunto in listPuntos)
                {
                    DateTime tempDate = fecDesde.AddDays(-1);
                    while (tempDate < fecHasta)
                    {
                        tempDate = tempDate.AddDays(1);
                        entity = new PrnConfiguracionDTO();
                        entity = this.GetByIdPrnConfiguracion(ePunto.Ptomedicodi, tempDate);

                        if (entity.Ptomedicodi != 0)
                        {
                            if (dataParametros.Prncfgporcerrormin != null) entity.Prncfgporcerrormin = dataParametros.Prncfgporcerrormin;
                            if (dataParametros.Prncfgporcerrormax != null) entity.Prncfgporcerrormax = dataParametros.Prncfgporcerrormax;
                            if (dataParametros.Prncfgmagcargamin != null) entity.Prncfgmagcargamin = dataParametros.Prncfgmagcargamin;
                            if (dataParametros.Prncfgmagcargamax != null) entity.Prncfgmagcargamax = dataParametros.Prncfgmagcargamax;
                            if (dataParametros.Prncfgporcdsvptrn != null) entity.Prncfgporcdsvptrn = dataParametros.Prncfgporcdsvptrn;
                            if (dataParametros.Prncfgporcmuestra != null) entity.Prncfgporcmuestra = dataParametros.Prncfgporcmuestra;
                            if (dataParametros.Prncfgporcdsvcnsc != null) entity.Prncfgporcdsvcnsc = dataParametros.Prncfgporcdsvcnsc;
                            if (dataParametros.Prncfgnrocoincidn != null) entity.Prncfgnrocoincidn = dataParametros.Prncfgnrocoincidn;
                            if (dataParametros.Prncfgflagveda != null) entity.Prncfgflagveda = dataParametros.Prncfgflagveda;
                            if (dataParametros.Prncfgflagferiado != null) entity.Prncfgflagferiado = dataParametros.Prncfgflagferiado;
                            if (dataParametros.Prncfgflagatipico != null) entity.Prncfgflagatipico = dataParametros.Prncfgflagatipico;
                            if (dataParametros.Prncfgflagdepauto != null) entity.Prncfgflagdepauto = dataParametros.Prncfgflagdepauto;
                            if (dataParametros.Prncfgtipopatron != null) entity.Prncfgtipopatron = dataParametros.Prncfgtipopatron;
                            if (dataParametros.Prncfgnumdiapatron != null) entity.Prncfgnumdiapatron = dataParametros.Prncfgnumdiapatron;
                            if (dataParametros.Prncfgflagdefecto != null) entity.Prncfgflagdefecto = dataParametros.Prncfgflagdefecto;
                            if (dataParametros.Prncfgpse != null) entity.Prncfgpse = dataParametros.Prncfgpse;
                            if (dataParametros.Prncfgfactorf != null) entity.Prncfgfactorf = dataParametros.Prncfgfactorf;
                            entity.Prncfgusumodificacion = nomUsuario;
                            entity.Prncfgfecmodificacion = DateTime.Now;
                            this.UpdatePrnConfiguracion(entity);
                        }
                        else
                        {
                            entity = dataParametros;
                            entity.Ptomedicodi = ePunto.Ptomedicodi;
                            entity.Prncfgfecha = tempDate;
                            entity.Prncfgusucreacion = nomUsuario;
                            entity.Prncfgfeccreacion = DateTime.Now;
                            entity.Prncfgusumodificacion = nomUsuario;
                            entity.Prncfgfecmodificacion = DateTime.Now;
                            this.SavePrnConfiguracion(entity);
                        }
                    }
                }
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El registro se realizó de manera exitosa";
            }
            else
            {
                typeMsg = ConstantesProdem.MsgWarning;
                dataMsg = "Debe seleccionar al menos un punto de medición";
            }

            return new { typeMsg, dataMsg };
        }

        /// <summary>
        /// Registra (Save y Update) los nuevos parámetros de configuración
        /// </summary>
        /// <param name="fecDesde"></param>
        /// <param name="fecHasta"></param>
        /// <param name="dataParametros"></param>
        /// <param name="selBarras"></param>
        /// <param name="nomUsuario"></param>
        public object ParametrosBarrasSave(DateTime fecDesde, DateTime fecHasta, PrnConfigbarraDTO dataParametros,
            List<int> selBarras, string nomUsuario)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;
            PrnConfigbarraDTO entity = new PrnConfigbarraDTO();
            List<PrGrupoDTO> listBarras = new List<PrGrupoDTO>();

            //Obtiene la lista de barras
            listBarras = this.ListPrGrupoBarra();

            if (selBarras.Count != 0)
            {
                listBarras = listBarras.
                Where(x => selBarras.Contains(x.Grupocodi)).
                ToList();
            }

            if (listBarras.Count != 0)
            {
                foreach (var eBarra in listBarras)
                {
                    DateTime tempDate = fecDesde.AddDays(-1);
                    while (tempDate < fecHasta)
                    {
                        tempDate = tempDate.AddDays(1);
                        entity = new PrnConfigbarraDTO();
                        entity = this.GetByIdPrnConfigbarra(eBarra.Grupocodi, tempDate);

                        if (entity.Grupocodi != 0)
                        {
                            if (dataParametros.Cfgbarpse != null) entity.Cfgbarpse = dataParametros.Cfgbarpse;
                            if (dataParametros.Cfgbarfactorf != null) entity.Cfgbarfactorf = dataParametros.Cfgbarfactorf;
                            entity.Cfgbarusumodificacion = nomUsuario;
                            entity.Cfgbarfecmodificacion = DateTime.Now;
                            this.UpdatePrnConfigbarra(entity);
                        }
                        else
                        {
                            entity = dataParametros;
                            entity.Grupocodi = eBarra.Grupocodi;
                            entity.Cfgbarfecha = tempDate;
                            entity.Cfgbarusucreacion = nomUsuario;
                            entity.Cfgbarfeccreacion = DateTime.Now;
                            entity.Cfgbarusumodificacion = nomUsuario;
                            entity.Cfgbarfecmodificacion = DateTime.Now;
                            this.SavePrnConfigbarra(entity);
                        }
                    }
                }
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El registro se realizó de manera exitosa";
            }
            else
            {
                typeMsg = ConstantesProdem.MsgWarning;
                dataMsg = "Debe seleccionar al menos un punto de medición";
            }

            return new { typeMsg, dataMsg };
        }

        /// <summary>
        /// Lista los parámetros de configuración (paginado)
        /// </summary>
        /// <param name="regIni"></param>
        /// <param name="regByPagina"></param>
        /// <param name="fecDesde"></param>
        /// <param name="fecHasta"></param>
        /// <param name="idDefecto"></param>
        /// <param name="listPuntos"></param>
        /// <returns></returns>
        public object ParametrosList(int regIni, int regByPagina, string fecDesde, string fecHasta,
            int idDefecto, List<PrnClasificacionDTO> listPuntos)
        {
            DateTime tempDate = new DateTime();
            PrnConfiguracionDTO entity = new PrnConfiguracionDTO();
            List<PrnConfiguracionDTO> entitys = new List<PrnConfiguracionDTO>();

            //Validación
            if (listPuntos.Count == 0) return new { data = entitys, recordsTotal = 0, recordsFiltered = 0 };

            //Calcula el total de registros y los registros por página
            DateTime dDesde = DateTime.ParseExact(fecDesde, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dHasta = DateTime.ParseExact(fecHasta, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            int diffDias = (dHasta - dDesde).Days + 1;
            int totalRegistros = diffDias * listPuntos.Count;

            //Crea la lista completa de puntos de medición ordenados por fecha y punto
            int i = 0;
            while (i < diffDias)
            {
                tempDate = dDesde.AddDays(i);
                foreach (var ePunto in listPuntos)
                {
                    entity = new PrnConfiguracionDTO();
                    entity.Ptomedicodi = ePunto.Ptomedicodi;
                    entity.Ptomedidesc = (string.IsNullOrEmpty(ePunto.FullPtomedidesc)) ? ePunto.Ptomedidesc : ePunto.FullPtomedidesc;
                    entity.Prncfgfecha = tempDate;
                    entity.StrPrncfgfecha = tempDate.ToString(ConstantesProdem.FormatoFecha);
                    entitys.Add(entity);
                }
                i++;
            }

            //Obtiene el rango de registros que se mostraran
            if (totalRegistros > regByPagina)
            {
                int tempDiff = totalRegistros - regIni;
                entitys = (tempDiff < regByPagina) ? entitys.GetRange(regIni, tempDiff) : entitys.GetRange(regIni, regByPagina);
            }

            //Carga los datos de los registros a mostrar
            string sList = UtilProdem.ConvertirEntityListEnString(entitys, "Ptomedicodi");
            string minDtPag = (from d in entitys select d.Prncfgfecha).Min().ToString(ConstantesProdem.FormatoFecha);
            string maxDtPag = (from d in entitys select d.Prncfgfecha).Max().ToString(ConstantesProdem.FormatoFecha);
            List<PrnConfiguracionDTO> tempData = FactorySic.GetPrnConfiguracionRepository().ParametrosList(minDtPag, maxDtPag, sList);

            //Datos por defecto
            PrnConfiguracionDTO regDefecto = this.ParametrosGetDefecto(idDefecto);

            foreach (var ent in entitys)
            {
                var d = tempData.Find(x => x.Ptomedicodi.Equals(ent.Ptomedicodi) && x.Prncfgfecha.Equals(ent.Prncfgfecha));
                if (d != null)
                {
                    ent.Prncfgporcerrormin = (d.Prncfgporcerrormin != null) ? d.Prncfgporcerrormin : regDefecto.Prncfgporcerrormin;
                    ent.Prncfgporcerrormax = (d.Prncfgporcerrormax != null) ? d.Prncfgporcerrormax : regDefecto.Prncfgporcerrormax;
                    ent.Prncfgmagcargamin = (d.Prncfgmagcargamin != null) ? d.Prncfgmagcargamin : regDefecto.Prncfgmagcargamin;
                    ent.Prncfgmagcargamax = (d.Prncfgmagcargamax != null) ? d.Prncfgmagcargamax : regDefecto.Prncfgmagcargamax;
                    ent.Prncfgporcdsvptrn = (d.Prncfgporcdsvptrn != null) ? d.Prncfgporcdsvptrn : regDefecto.Prncfgporcdsvptrn;
                    ent.Prncfgporcmuestra = (d.Prncfgporcmuestra != null) ? d.Prncfgporcmuestra : regDefecto.Prncfgporcmuestra;
                    ent.Prncfgporcdsvcnsc = (d.Prncfgporcdsvcnsc != null) ? d.Prncfgporcdsvcnsc : regDefecto.Prncfgporcdsvcnsc;
                    ent.Prncfgnrocoincidn = (d.Prncfgnrocoincidn != null) ? d.Prncfgnrocoincidn : regDefecto.Prncfgnrocoincidn;
                    ent.Prncfgflagveda = (d.Prncfgflagveda != null) ? d.Prncfgflagveda : regDefecto.Prncfgflagveda;
                    ent.Prncfgflagferiado = (d.Prncfgflagferiado != null) ? d.Prncfgflagferiado : regDefecto.Prncfgflagferiado;
                    ent.Prncfgflagatipico = (d.Prncfgflagatipico != null) ? d.Prncfgflagatipico : regDefecto.Prncfgflagatipico;
                    ent.Prncfgflagdepauto = (d.Prncfgflagdepauto != null) ? d.Prncfgflagdepauto : regDefecto.Prncfgflagdepauto;
                    ent.Prncfgtipopatron = (d.Prncfgtipopatron != null) ? d.Prncfgtipopatron : regDefecto.Prncfgtipopatron;
                    ent.Prncfgnumdiapatron = (d.Prncfgnumdiapatron != null) ? d.Prncfgnumdiapatron : regDefecto.Prncfgnumdiapatron;
                    ent.Prncfgflagdefecto = (d.Prncfgflagdefecto != null) ? d.Prncfgflagdefecto : regDefecto.Prncfgflagdefecto;

                    ent.Prncfgtiporeg = ConstantesProdem.TipoRegParametrosNormal;
                    ent.Prncfgpse = (d.Prncfgpse != null) ? d.Prncfgpse : regDefecto.Prncfgpse;
                    ent.Prncfgfactorf = (d.Prncfgfactorf != null) ? d.Prncfgfactorf : regDefecto.Prncfgfactorf;

                }
                else
                {
                    ent.Prncfgporcerrormin = regDefecto.Prncfgporcerrormin;
                    ent.Prncfgporcerrormax = regDefecto.Prncfgporcerrormax;
                    ent.Prncfgmagcargamin = regDefecto.Prncfgmagcargamin;
                    ent.Prncfgmagcargamax = regDefecto.Prncfgmagcargamax;
                    ent.Prncfgporcdsvptrn = regDefecto.Prncfgporcdsvptrn;
                    ent.Prncfgporcmuestra = regDefecto.Prncfgporcmuestra;
                    ent.Prncfgporcdsvcnsc = regDefecto.Prncfgporcdsvcnsc;
                    ent.Prncfgnrocoincidn = regDefecto.Prncfgnrocoincidn;
                    ent.Prncfgflagveda = regDefecto.Prncfgflagveda;
                    ent.Prncfgflagferiado = regDefecto.Prncfgflagferiado;
                    ent.Prncfgflagatipico = regDefecto.Prncfgflagatipico;
                    ent.Prncfgflagdepauto = regDefecto.Prncfgflagdepauto;
                    ent.Prncfgtipopatron = regDefecto.Prncfgtipopatron;
                    ent.Prncfgnumdiapatron = regDefecto.Prncfgnumdiapatron;
                    ent.Prncfgflagdefecto = regDefecto.Prncfgflagdefecto;

                    ent.Prncfgtiporeg = ConstantesProdem.TipoRegParametrosDefecto;
                    ent.Prncfgpse = regDefecto.Prncfgpse;
                    ent.Prncfgfactorf = regDefecto.Prncfgfactorf;
                }
            }

            return new { data = entitys, recordsTotal = totalRegistros, recordsFiltered = totalRegistros };
        }

        /// <summary>
        /// Lista los parámetros de configuración para las barras (paginado)
        /// </summary>
        /// <param name="regIni"></param>
        /// <param name="regByPagina"></param>
        /// <param name="fecDesde"></param>
        /// <param name="fecHasta"></param>
        /// <param name="selBarras"></param>
        /// <returns></returns>
        public object ParametrosBarrasList(int regIni, int regByPagina, string fecDesde,
            string fecHasta, List<int> selBarras)
        {
            DateTime tempDate = new DateTime();
            PrnConfigbarraDTO entity = new PrnConfigbarraDTO();
            List<PrnConfigbarraDTO> entitys = new List<PrnConfigbarraDTO>();
            List<PrGrupoDTO> listBarras = new List<PrGrupoDTO>();

            //Obtiene la lista de barras
            listBarras = this.ListPrGrupoBarra();

            if (selBarras.Count != 0)
            {
                listBarras = listBarras.
                Where(x => selBarras.Contains(x.Grupocodi)).
                ToList();
            }

            //Validación
            if (listBarras.Count == 0) return new { data = entitys, recordsTotal = 0, recordsFiltered = 0 };

            //Calcula el total de registros y los registros por página
            DateTime dDesde = DateTime.ParseExact(fecDesde, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dHasta = DateTime.ParseExact(fecHasta, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            int diffDias = (dHasta - dDesde).Days + 1;
            int totalRegistros = diffDias * listBarras.Count;

            //Crea la lista completa de puntos de medición ordenados por fecha y punto
            int i = 0;
            while (i < diffDias)
            {
                tempDate = dDesde.AddDays(i);
                foreach (var eBarra in listBarras)
                {
                    entity = new PrnConfigbarraDTO();
                    entity.Grupocodi = eBarra.Grupocodi;
                    entity.Gruponomb = eBarra.Gruponomb;
                    entity.Cfgbarfecha = tempDate;
                    entity.StrCfgbarfecha = tempDate.ToString(ConstantesProdem.FormatoFecha);
                    entitys.Add(entity);
                }
                i++;
            }

            //Obtiene el rango de registros que se mostraran
            if (totalRegistros > regByPagina)
            {
                int tempDiff = totalRegistros - regIni;
                entitys = (tempDiff < regByPagina) ? entitys.GetRange(regIni, tempDiff) : entitys.GetRange(regIni, regByPagina);
            }

            //Carga los datos de los registros a mostrar
            string sList = UtilProdem.ConvertirEntityListEnString(entitys, "Grupocodi");
            string minDtPag = (from d in entitys select d.Cfgbarfecha).Min().ToString(ConstantesProdem.FormatoFecha);
            string maxDtPag = (from d in entitys select d.Cfgbarfecha).Max().ToString(ConstantesProdem.FormatoFecha);

            List<PrnConfigbarraDTO> tempData = FactorySic.GetPrnConfigbarraRepository().
                ParametrosList(minDtPag, maxDtPag, sList);

            //Datos por defecto
            PrnConfigbarraDTO regDefecto = this.ParametrosBarrasGetDefecto();

            foreach (var ent in entitys)
            {
                var d = tempData.Find(x => x.Grupocodi.Equals(ent.Grupocodi) && x.Cfgbarfecha.Equals(ent.Cfgbarfecha));
                if (d != null)
                {
                    ent.Cfgbarpse = (d.Cfgbarpse != null) ? d.Cfgbarpse : regDefecto.Cfgbarpse;
                    ent.Cfgbarfactorf = (d.Cfgbarfactorf != null) ? d.Cfgbarfactorf : regDefecto.Cfgbarfactorf;

                    ent.Cfgbartiporeg = ConstantesProdem.TipoRegParametrosNormal;
                }
                else
                {
                    ent.Cfgbarpse = regDefecto.Cfgbarpse;
                    ent.Cfgbarfactorf = regDefecto.Cfgbarfactorf;

                    ent.Cfgbartiporeg = ConstantesProdem.TipoRegParametrosDefecto;
                }
            }

            return new { data = entitys, recordsTotal = totalRegistros, recordsFiltered = totalRegistros };
        }

        /// <summary>
        /// Obtiene los parámetros de configuración por defecto
        /// </summary>
        /// <param name="idDefecto"></param>
        /// <returns></returns>
        public PrnConfiguracionDTO ParametrosGetDefecto(int idDefecto)
        {
            DateTime defDate = DateTime.ParseExact
                (ConstantesProdem.DefectoDate, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            return this.GetByIdPrnConfiguracion(idDefecto, defDate);
        }

        /// <summary>
        /// Obtiene los parámetros de configuración por defecto de una barra
        /// </summary>
        /// <returns></returns>
        public PrnConfigbarraDTO ParametrosBarrasGetDefecto()
        {
            DateTime defDate = DateTime.ParseExact
                (ConstantesProdem.DefectoDate, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            return this.GetByIdPrnConfigbarra(ConstantesProdem.DefectoByBarra, defDate);
        }

        /// <summary>
        /// Actualiza los parámetros de configuración por defecto
        /// </summary>
        /// <param name="idDefecto"></param>
        /// <param name="dataParametros"></param>
        /// <param name="nomUsuario"></param>
        /// <returns></returns>
        public object ParametrosUpdateDefecto(int idDefecto, PrnConfiguracionDTO dataParametros, string nomUsuario)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;
            PrnConfiguracionDTO entity = this.ParametrosGetDefecto(idDefecto);

            if (entity.Ptomedicodi != 0)
            {
                if (dataParametros.Prncfgporcerrormin != null) entity.Prncfgporcerrormin = dataParametros.Prncfgporcerrormin;
                if (dataParametros.Prncfgporcerrormax != null) entity.Prncfgporcerrormax = dataParametros.Prncfgporcerrormax;
                if (dataParametros.Prncfgmagcargamin != null) entity.Prncfgmagcargamin = dataParametros.Prncfgmagcargamin;
                if (dataParametros.Prncfgmagcargamax != null) entity.Prncfgmagcargamax = dataParametros.Prncfgmagcargamax;
                if (dataParametros.Prncfgporcdsvptrn != null) entity.Prncfgporcdsvptrn = dataParametros.Prncfgporcdsvptrn;
                if (dataParametros.Prncfgporcmuestra != null) entity.Prncfgporcmuestra = dataParametros.Prncfgporcmuestra;
                if (dataParametros.Prncfgporcdsvcnsc != null) entity.Prncfgporcdsvcnsc = dataParametros.Prncfgporcdsvcnsc;
                if (dataParametros.Prncfgnrocoincidn != null) entity.Prncfgnrocoincidn = dataParametros.Prncfgnrocoincidn;
                if (dataParametros.Prncfgflagveda != null) entity.Prncfgflagveda = dataParametros.Prncfgflagveda;
                if (dataParametros.Prncfgflagferiado != null) entity.Prncfgflagferiado = dataParametros.Prncfgflagferiado;
                if (dataParametros.Prncfgflagatipico != null) entity.Prncfgflagatipico = dataParametros.Prncfgflagatipico;
                if (dataParametros.Prncfgflagdepauto != null) entity.Prncfgflagdepauto = dataParametros.Prncfgflagdepauto;
                if (dataParametros.Prncfgtipopatron != null) entity.Prncfgtipopatron = dataParametros.Prncfgtipopatron;
                if (dataParametros.Prncfgnumdiapatron != null) entity.Prncfgnumdiapatron = dataParametros.Prncfgnumdiapatron;
                if (dataParametros.Prncfgflagdefecto != null) entity.Prncfgflagdefecto = dataParametros.Prncfgflagdefecto;
                if (dataParametros.Prncfgpse != null) entity.Prncfgpse = dataParametros.Prncfgpse;
                if (dataParametros.Prncfgfactorf != null) entity.Prncfgfactorf = dataParametros.Prncfgfactorf;
                entity.Prncfgusumodificacion = nomUsuario;
                entity.Prncfgfecmodificacion = DateTime.Now;
                this.UpdatePrnConfiguracion(entity);

                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "Parámetros por defecto actualizados correctamente!";
            }

            return new { typeMsg, dataMsg };
        }

        /// <summary>
        /// Actualiza los parámetros de configuración por defecto de las barras
        /// </summary>
        /// <returns></returns>
        public object ParametrosBarrasUpdateDefecto(PrnConfigbarraDTO dataParametros, string nomUsuario)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;
            PrnConfigbarraDTO entity = this.ParametrosBarrasGetDefecto();

            if (entity.Grupocodi != 0)
            {
                if (dataParametros.Cfgbarpse != null) entity.Cfgbarpse = dataParametros.Cfgbarpse;
                if (dataParametros.Cfgbarfactorf != null) entity.Cfgbarfactorf = dataParametros.Cfgbarfactorf;
                entity.Cfgbarusumodificacion = nomUsuario;
                entity.Cfgbarfecmodificacion = DateTime.Now;
                this.UpdatePrnConfigbarra(entity);

                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "Parámetros por defecto actualizados correctamente!";
            }

            return new { typeMsg, dataMsg };
        }

        #endregion

        #region Metodos del Mpodulo de Configuración - Parámetros - Servicios Auxiliares(SA)

        /// <summary>
        /// Genera el modelo de datos del módulo
        /// </summary>
        /// <param name="listaBarra">Fecha del registro</param>
        /// <returns></returns>
        public List<ParametrosSA> ParametrosSAData(List<int> listaBarraPM, List<int> listaBarraCP)
        {
            string title = string.Empty;
            string inBarrasPM = "";
            string inBarrasCP = "";
            string outBarras = "";
            List<ParametrosSA> data = new List<ParametrosSA>();
            List<PrGrupoDTO> todasBarras = this.GetListBarras();

            //Todas las Barras
            List<int> BarrasId = new List<int>();
            foreach (var item in todasBarras)
            {
                BarrasId.Add(item.Grupocodi);
            }

            //Dar la forma para el IN de la consulta sql
            if (listaBarraPM == null || listaBarraPM.Count == 0)
            {
                inBarrasPM = "0";
            }
            else
            {

                inBarrasPM = string.Join(",", listaBarraPM);
            }


            if (listaBarraCP.Count == 0)
            {
                inBarrasCP = "0";
            }
            else
            {

                inBarrasCP = string.Join(",", listaBarraCP);
            }

            //Lista de Barras ya existentes en la tabla PRNMedicion48
            //List<PrnMedicion48DTO> regBarrass = this.GetParametrosBarras(inBarrasPM, ConstantesProdem.PrntParametroBarra, ConstantesProdem.Prcatecodi, inBarrasCP);
            List<PrnMediciongrpDTO> regBarras = this.GetParametrosBarras(inBarrasPM, ConstantesProdem.PrnmgrtServicioAuxiliar, inBarrasCP);


            ////Formatos de presentación
            ParametrosSA entity;
            //a.1) Intervalos de tiempo
            entity = new ParametrosSA
            {
                id = "intervalos",
                label = "Hora",
                data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min).ToList(),
                htrender = "hora",
                hcrender = "categoria"
            };
            data.Add(entity);

            //a.2) Mediciones
            for (int i = 0; i < regBarras.Count; i++)
            {
                List<string> lista = new List<string>();
                try
                {
                    for (int j = 1; j <= 48; j++)
                    {
                        var dato = (regBarras[i].GetType().GetProperty("H" + j.ToString()).GetValue(regBarras[i], null));
                        if (dato == null)
                        {
                            dato = "0";
                        }
                        lista.Add(dato.ToString());

                    }
                    entity = new ParametrosSA
                    {
                        id = regBarras[i].Grupocodi.ToString(),
                        label = regBarras[i].Gruponomb,
                        data = lista,
                        htrender = "normal",
                        hcrender = "normal",
                    };
                    data.Add(entity);
                }
                catch (Exception)
                {

                }
            }

            return data;//regconfiguracion };

        }
        /// <summary>
        /// Se pivotea la data para insertar el DTO de PrnMedicion48
        /// </summary>
        /// <param name="id">codigo de la barra</param>
        /// <param name="data">Fecha del registro</param>
        /// <param name="user">Fecha del registro</param>
        /// <returns></returns>
        public void ParametrosSASave(int id, decimal[] data, string user)
        {
            PrnMediciongrpDTO entity = new PrnMediciongrpDTO();
            //entity.Ptomedicodi = id;
            //entity.Prnm48tipo = ConstantesProdem.PrntParametroBarra;
            entity.Grupocodi = id;
            entity.Prnmgrtipo = ConstantesProdem.PrnmgrtServicioAuxiliar;
            entity.Medifecha = DateTime.MinValue;
            //entity.Prnm48estado = 0;
            entity.Prnmgrusucreacion = user;
            entity.Prnmgrfeccreacion = DateTime.Now;
            for (int i = 1; i <= 48; i++)
            {
                entity.GetType().GetProperty("H" + i.ToString()).SetValue(entity, data[i - 1]);
            }

            //this.SavePrnMedicion48(entity);
            this.SavePrnMediciongrp(entity);
        }

        /// <summary>
        /// LIsta solo las barras registrdas como PM de la version activa
        /// </summary>
        /// <param name="catecodi"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> GetListBarrasPM(int catecodi, string barracp)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().GetLisBarrasSoloPM(catecodi, barracp);
        }

        /// <summary>
        /// Lista solo las barras registradas como CP de la version activa
        /// </summary>
        /// <param name="catecodi"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> GetListBarrasCP(int catecodi)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().GetLisBarrasSoloCP(catecodi);
        }

        #endregion

        #region Métodos del Módulo de Configuración - Perfiles

        /// <summary>
        /// Genera el modelo de datos del módulo
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public object PerfilesDatos(int idPunto, DateTime regFecha)
        {
            string title = string.Empty;
            List<object> data = new List<object>();

            //Obtiene el titulo principal
            title = this.GetNombrePtomedicion(idPunto) + " (" + idPunto.ToString() + ")";

            //Obtiene la configuración del punto
            PrnConfiguracionDTO regconfiguracion = this.ParametrosGetConfiguracion(idPunto,
                ConstantesProdem.DefectoByPunto, regFecha);
            //Obtiene los datos del perfil patrón
            PrnPatronModel regPatron = this.GetPatron(idPunto, ConstantesProdem.ProcPatronDemandaEjecutada, regFecha, regconfiguracion);
            //Obtiene los datos del perfil defecto
            //DateTime defDate = DateTime.ParseExact(ConstantesProdem.DefectoDate, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            //PrnMedicion48DTO regDefecto = this.GetByIdPrnMedicion48(idPunto, UtilProdem.ValidarPatronDiaDefecto(regFecha), defDate);

            //Validación
            if (regPatron.NDias == 0) return new { valid = false };

            //Formatos de presentación
            object entity;
            //a.1) Intervalos de tiempo
            entity = new
            {
                id = "intervalos",
                label = "Hora",
                data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                htrender = "hora",
                hcrender = "categoria"
            };
            data.Add(entity);

            //a.2) Mediciones
            for (int i = 0; i < regPatron.Mediciones.Count; i++)
            {
                entity = new
                {
                    id = "med" + (i + 1).ToString(),
                    label = regPatron.StrFechas[i],
                    data = regPatron.Mediciones[i],
                    htrender = "normal",
                    hcrender = "normal",
                    label2 = regPatron.StrFechasTarde[i],
                    slunes = regPatron.EsLunes
                };
                data.Add(entity);
            }

            //a.3) Patron
            entity = new
            {
                id = "patron",
                label = (regconfiguracion.Prncfgflagdefecto == ConstantesProdem.RegSi) ? "Patrón" : "Patrón(Act)",
                data = regPatron.Patron,
                htrender = "patron",
                hcrender = "patron"
            };
            data.Add(entity);

            //a.5) Márgenes de error
            decimal[] tempPatron = (regconfiguracion.Prncfgflagdefecto == ConstantesProdem.RegSi)
                ? regPatron.PatronDefecto
                : regPatron.Patron;
            decimal[] rMin = new decimal[ConstantesProdem.Itv30min];
            decimal[] rMax = new decimal[ConstantesProdem.Itv30min];
            int j = 0;
            while (j < ConstantesProdem.Itv30min)
            {
                decimal fMin = 1 - (regconfiguracion.Prncfgporcdsvptrn * 0.01M) ?? 0;
                decimal fMax = 1 + (regconfiguracion.Prncfgporcdsvptrn * 0.01M) ?? 0;
                rMin[j] = tempPatron[j] * fMin;
                rMax[j] = tempPatron[j] * fMax;
                j++;
            }

            entity = new
            {
                id = "rmin",
                label = "EMin",
                data = rMin,
                htrender = "no",
                hcrender = "margen"
            };
            data.Add(entity);

            entity = new
            {
                id = "rmax",
                label = "EMax",
                data = rMax,
                htrender = "no",
                hcrender = "margen"
            };
            data.Add(entity);

            //a.6) Ajuste Manual - "Ajuste" para el ht
            decimal[] test = new decimal[48];
            entity = new
            {
                id = "ajuste",
                label = "Ajuste",
                data = new decimal[ConstantesProdem.Itv30min],
                htrender = "edit",
                hcrender = "no"
            };
            data.Add(entity);

            //a.7) Final - Perfíl defecto
            entity = new
            {
                id = "final",
                label = (regconfiguracion.Prncfgflagdefecto == ConstantesProdem.RegSi) ? "Defecto(Act)" : "Defecto",
                data = regPatron.PatronDefecto,
                htrender = "final",
                hcrender = "final"
            };
            data.Add(entity);

            return new { title = title, data = data, config = regconfiguracion, valid = true };
        }

        /// <summary>
        /// Devuelve el perfil patrón actualizado y la nueva linea seleccionada
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFechaA">Fecha del intervalo correspondiente al intervalo de la mañana</param>
        /// <param name="regFechaB">Fecha del intervalo correspondiente al intervalo de la tarde</param>
        /// <param name="esLunes">Flag que indica si se debe considerar la fecB</param>
        /// <param name="tipoPatron">Parámetro que indica el tipo de obtención del perfil patrón</param>
        /// <param name="dsvPatron">Parámetro que indica el porcentaje de desviación respecto al perfil patrón</param>
        /// <param name="dataMediciones">Mediciones que conforman el perfil patrón mostrado</param>
        /// <returns></returns>
        public object PerfilesUpdatePatron(int idPunto, string regFechaA, string regFechaB,
            bool esLunes, string tipoPatron, decimal? dsvPatron, List<decimal[]> dataMediciones)
        {
            PrnMedicion48DTO entPrincipal = new PrnMedicion48DTO();
            PrnMedicion48DTO entSecundaria = new PrnMedicion48DTO();

            //Obtiene la demanda ejecutada segun el tipo
            entPrincipal = this.GetDemandaEjecutadaPorPunto(idPunto, regFechaA).
                    FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();

            if (esLunes)
            {
                entSecundaria = this.GetDemandaEjecutadaPorPunto(idPunto, regFechaB).
                    FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
            }

            decimal[] arrayEntity = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, entPrincipal);

            //Llena el intervalo de la tarde si es un Lunes
            if (esLunes)
            {
                int i = ConstantesProdem.Itv30min / 2;
                decimal[] temp = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, entSecundaria);

                while (i <= ConstantesProdem.Itv30min)
                {
                    arrayEntity[i - 1] = temp[i - 1];
                    i++;
                }
            }

            //Calcula el nuevo perfil patrón
            dataMediciones.Add(arrayEntity);
            decimal[] nPatron = UtilProdem.CalcularPerfilPatron(dataMediciones, dataMediciones.Count, ConstantesProdem.Itv30min, tipoPatron);

            //Calcula los nuevos margenes de error
            decimal[] rMin = new decimal[ConstantesProdem.Itv30min];
            decimal[] rMax = new decimal[ConstantesProdem.Itv30min];
            int j = 0;
            while (j < ConstantesProdem.Itv30min)
            {
                decimal fMin = 1 - (dsvPatron * 0.01M) ?? 0;
                decimal fMax = 1 + (dsvPatron * 0.01M) ?? 0;
                rMin[j] = nPatron[j] * fMin;
                rMax[j] = nPatron[j] * fMax;
                j++;
            }

            return new { patron = nPatron, medicion = arrayEntity, emin = rMin, emax = rMax };
        }

        /// <summary>
        /// Registra el nuevo perfíl por defecto del punto de medición
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="dataMedicion">Datos de la nueva medición defecto</param>
        /// <returns></returns>
        public object PerfilesSave(int idPunto, string regFecha, PrnMedicion48DTO dataMedicion)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;

            PrnMedicion48DTO entity = dataMedicion;
            DateTime parseFecha = DateTime.ParseExact(regFecha,
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            entity.Ptomedicodi = idPunto;
            entity.Prnm48tipo = UtilProdem.ValidarPatronDiaDefecto(parseFecha);
            entity.Medifecha = DateTime.ParseExact(ConstantesProdem.DefectoDate,
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            //Validación
            PrnMedicion48DTO isValid = this.GetByIdPrnMedicion48(entity.Ptomedicodi,
                entity.Prnm48tipo, entity.Medifecha);

            if (isValid.Ptomedicodi != 0)
            {
                this.UpdatePrnMedicion48(entity);
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El registro se actualizó de manera exitosa";
            }
            else
            {
                this.SavePrnMedicion48(entity);
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El registro se realizó de manera exitosa";
            }

            return new { typeMsg, dataMsg };
        }

        /// <summary>
        /// Reporte de perfiles patron de los puntos de medicion
        /// </summary>
        /// <param name="inicio"></param>
        /// <param name="fin"></param>
        /// <returns></returns>
        public string PerfilesPatronExcel(string inicio, string fin)
        {
            List<PrnFormatoExcel> libro = new List<PrnFormatoExcel>();
            int[] codiArea = new int[] { ConstantesProdem.AreacodiANorte, ConstantesProdem.AreacodiASur, ConstantesProdem.AreacodiACentro, ConstantesProdem.AreacodiASierraCentro };
            string[] nombreArea = new string[] { ConstantesProdem.AOperativaNorte, ConstantesProdem.AOperativaSur, ConstantesProdem.AOperativaCentro, ConstantesProdem.AOperativaSierraCentro };
            int numDias = Convert.ToInt32((DateTime.ParseExact(fin, ConstantesProdem.FormatoFecha, CultureInfo.CurrentCulture) - DateTime.ParseExact(inicio, ConstantesProdem.FormatoFecha, CultureInfo.CurrentCulture)).TotalDays) + 1;
            string[] itv = UtilProdem.GenerarIntervalosFecha(ConstantesProdem.Itv30min, inicio, fin);

            int c = 0;
            foreach (var item in codiArea)
            {
                List<MePtomedicionDTO> lista = this.PR03PuntosPorBarrasCP(item.ToString());

                string libroNombre = nombreArea[c];
                c++;
                if (lista.Count > 0)
                {
                    List<int> codiEmpresa = lista.OrderBy(x => x.Emprcodi ?? 0).Select(x => x.Emprcodi ?? 0).Distinct().ToList();
                    List<int> codiPtomedicion = lista.OrderBy(x => x.Emprcodi).Select(x => x.Ptomedicodi).Distinct().ToList();
                    int numColumnas = lista.Count() + 1;

                    int[] anchoColumnas = new int[numColumnas];
                    for (int i = 0; i < numColumnas; i++)
                    {
                        anchoColumnas[i] = 50;
                    }

                    PrnFormatoExcel hojaArea = new PrnFormatoExcel()
                    {
                        Titulo = "PERFIL PATRÓN " + inicio + " - " + fin,
                        Subtitulo1 = libroNombre,
                        AnchoColumnas = anchoColumnas,
                        NombreLibro = libroNombre
                    };
                    //Creando las cabeceras
                    //Primera cabecera - fila 01
                    hojaArea.NestedHeader1 = new List<PrnExcelHeader>();
                    PrnExcelHeader headArea = new PrnExcelHeader() { Etiqueta = "", Columnas = 1 };
                    hojaArea.NestedHeader1.Add(headArea);
                    foreach (var empresa in codiEmpresa)
                    {
                        int num = lista.Where(x => x.Emprcodi == empresa).ToList().Count();
                        string nombEmpresa = lista.Where(x => x.Emprcodi == empresa).Select(x => x.Emprnomb).First();
                        headArea = new PrnExcelHeader() { Etiqueta = nombEmpresa, Columnas = num };
                        hojaArea.NestedHeader1.Add(headArea);
                    }

                    //Creando las cabeceras
                    //Primera cabecera - fila 02
                    hojaArea.NestedHeader2 = new List<PrnExcelHeader>();
                    PrnExcelHeader headcodigo = new PrnExcelHeader() { Etiqueta = "", Columnas = 1 };
                    hojaArea.NestedHeader2.Add(headcodigo);
                    foreach (var punto in codiPtomedicion)
                    {
                        headcodigo = new PrnExcelHeader() { Etiqueta = punto.ToString(), Columnas = 1 };
                        hojaArea.NestedHeader2.Add(headcodigo);
                    }

                    //Segunda cabecera - fila 03
                    hojaArea.NestedHeader3 = new List<PrnExcelHeader>();
                    headArea = new PrnExcelHeader() { Etiqueta = "HORA", Columnas = 1 };
                    hojaArea.NestedHeader3.Add(headArea);
                    foreach (var punto in codiPtomedicion)
                    {
                        string nombPunto = lista.Where(x => x.Ptomedicodi == punto).Select(x => x.Ptomedidesc).First();
                        headArea = new PrnExcelHeader() { Etiqueta = nombPunto, Columnas = 1 };
                        hojaArea.NestedHeader3.Add(headArea);
                    }

                    //Creando el contenido de la hoja
                    string[][] contentArea = new string[numColumnas][];
                    contentArea[0] = itv;

                    int k = 1;
                    foreach (var codigo in codiPtomedicion)
                    {
                        decimal[] datos = new decimal[itv.Count()];
                        DateTime fInicio = DateTime.ParseExact(inicio, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
                        int d = 0;
                        int dias = numDias;
                        while (dias > 0)
                        {
                            PrnConfiguracionDTO dataConfiguracion = this.ParametrosGetConfiguracion(codigo, ConstantesProdem.DefectoByPunto, fInicio);
                            PrnPatronModel dataPatron = this.GetPatron(codigo, ConstantesProdem.ProcPatronDemandaEjecutada, fInicio, dataConfiguracion);
                            dias += -1;
                            for (int j = 0; j < ConstantesProdem.Itv30min; j++)
                            {
                                datos[d] = dataPatron.Patron[j];
                                d++;
                            }
                        }
                        string[] str_med = Array.ConvertAll(datos, x => x.ToString());
                        contentArea[k] = str_med;
                        k++;
                    }

                    hojaArea.Contenido = contentArea;
                    libro.Add(hojaArea);
                }
            }

            string reporte = "-1";
            if (libro.Count() > 0)
            {
                string pathFile = ConfigurationManager.AppSettings[ConstantesProdem.ReportePronostico].ToString();
                string filename = "PATRONES-";
                filename += inicio.Replace("/", string.Empty) + "-";
                filename += fin.Replace("/", string.Empty);
                reporte = this.ExportarReporteConLibros(libro, pathFile, filename);
            }

            return reporte;
        }

        /// <summary>
        /// Reporte de perfiles patron de los puntos de medicion
        /// </summary>
        /// <param name="inicio"></param>
        /// <param name="fin"></param>
        /// <returns></returns>
        public string PerfilesPatronExcelPorFecha(string inicio, string fin)
        {

            List<PrnFormatoExcel> libro = new List<PrnFormatoExcel>();
            //int[] codiArea = new int[] { ConstantesProdem.AreacodiANorte, ConstantesProdem.AreacodiASur, ConstantesProdem.AreacodiACentro, ConstantesProdem.AreacodiASierraCentro };
            //string[] nombreArea = new string[] { ConstantesProdem.AOperativaNorte, ConstantesProdem.AOperativaSur, ConstantesProdem.AOperativaCentro, ConstantesProdem.AOperativaSierraCentro };
            string[] nombreArea = new string[] { "PUNTOS DE MEDICIÓN" };
            int numDias = Convert.ToInt32((DateTime.ParseExact(fin, ConstantesProdem.FormatoFecha, CultureInfo.CurrentCulture) - DateTime.ParseExact(inicio, ConstantesProdem.FormatoFecha, CultureInfo.CurrentCulture)).TotalDays) + 1;
            string[] itv = UtilProdem.GenerarIntervalosFecha(ConstantesProdem.Itv30min, inicio, fin);

            int c = 0;
            //foreach (var item in codiArea)
            // {
            //model.ListPtomedicion = this.servicio.PR03Puntos();
            List<MePtomedicionDTO> lista = this.PR03Puntos();
            //List<MePtomedicionDTO> lista = this.PR03PuntosPorBarrasCP(item.ToString());

            string libroNombre = nombreArea[c];
            c++;
            if (lista.Count > 0)
            {
                List<int> codiEmpresa = lista.OrderBy(x => x.Emprcodi ?? 0).Select(x => x.Emprcodi ?? 0).Distinct().ToList();
                List<int> codiPtomedicion = lista.OrderBy(x => x.Emprcodi).Select(x => x.Ptomedicodi).Distinct().ToList();
                //codiPtomedicion.Clear();
                //codiPtomedicion.Add()
                int numColumnas = lista.Count() + 1;

                int[] anchoColumnas = new int[numColumnas];
                for (int i = 0; i < numColumnas; i++)
                {
                    anchoColumnas[i] = 50;
                }

                PrnFormatoExcel hojaArea = new PrnFormatoExcel()
                {
                    Titulo = "PERFIL PATRÓN " + inicio + " - " + fin,
                    Subtitulo1 = libroNombre,
                    AnchoColumnas = anchoColumnas,
                    NombreLibro = libroNombre
                };
                //Creando las cabeceras
                //Primera cabecera - fila 01
                hojaArea.NestedHeader1 = new List<PrnExcelHeader>();
                PrnExcelHeader headArea = new PrnExcelHeader() { Etiqueta = "", Columnas = 1 };
                hojaArea.NestedHeader1.Add(headArea);
                foreach (var empresa in codiEmpresa)
                {
                    int num = lista.Where(x => x.Emprcodi == empresa).ToList().Count();
                    string nombEmpresa = lista.Where(x => x.Emprcodi == empresa).Select(x => x.Emprnomb).First();
                    headArea = new PrnExcelHeader() { Etiqueta = nombEmpresa, Columnas = num };
                    hojaArea.NestedHeader1.Add(headArea);
                }

                //Creando las cabeceras
                //Primera cabecera - fila 02
                hojaArea.NestedHeader2 = new List<PrnExcelHeader>();
                PrnExcelHeader headcodigo = new PrnExcelHeader() { Etiqueta = "", Columnas = 1 };
                hojaArea.NestedHeader2.Add(headcodigo);
                foreach (var punto in codiPtomedicion)
                {
                    headcodigo = new PrnExcelHeader() { Etiqueta = punto.ToString(), Columnas = 1 };
                    hojaArea.NestedHeader2.Add(headcodigo);
                }

                //Segunda cabecera - fila 03
                hojaArea.NestedHeader3 = new List<PrnExcelHeader>();
                headArea = new PrnExcelHeader() { Etiqueta = "HORA", Columnas = 1 };
                hojaArea.NestedHeader3.Add(headArea);
                foreach (var punto in codiPtomedicion)
                {
                    string nombPunto = lista.Where(x => x.Ptomedicodi == punto).Select(x => x.Ptomedidesc).First();
                    headArea = new PrnExcelHeader() { Etiqueta = nombPunto, Columnas = 1 };
                    hojaArea.NestedHeader3.Add(headArea);
                }

                //Creando el contenido de la hoja
                string[][] contentArea = new string[numColumnas][];
                contentArea[0] = itv;

                int k = 1;
                foreach (var codigo in codiPtomedicion)
                {
                    decimal[] datos = new decimal[itv.Count()];
                    DateTime fInicio = DateTime.ParseExact(inicio, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime fConsultar = DateTime.ParseExact(inicio, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
                    int d = 0;
                    int dias = numDias;
                    while (dias > 0)
                    {
                        //DateTime fConsultar = fInicio.AddDays(1);
                        PrnConfiguracionDTO dataConfiguracion = this.ParametrosGetConfiguracion(codigo, ConstantesProdem.DefectoByPunto, fConsultar);
                        PrnPatronModel dataPatron = this.GetPatronPorFecha(codigo, ConstantesProdem.ProcPatronDemandaEjecutada, fConsultar, dataConfiguracion);
                        dias += -1;
                        for (int j = 0; j < ConstantesProdem.Itv30min; j++)
                        {
                            try
                            {
                                datos[d] = dataPatron.PatronDefecto[j];
                            }
                            catch
                            {
                                datos[d] = 0;
                            }
                            //datos[d] = dataPatron.PatronDefecto[j];
                            d++;
                        }

                        fConsultar = fConsultar.AddDays(1);
                    }
                    string[] str_med = Array.ConvertAll(datos, x => x.ToString());
                    contentArea[k] = str_med;
                    k++;

                    //break;
                }

                hojaArea.Contenido = contentArea;
                libro.Add(hojaArea);
            }

            //break;
            //}

            string reporte = "-1";
            if (libro.Count() > 0)
            {
                string pathFile = ConfigurationManager.AppSettings[ConstantesProdem.ReportePronostico].ToString();
                string filename = "PATRONES-";
                filename += inicio.Replace("/", string.Empty) + "-";
                filename += fin.Replace("/", string.Empty);
                reporte = this.ExportarReporteConLibros(libro, pathFile, filename);
            }

            return reporte;
        }

        #endregion

        #region Métodos del Módulo de Configuración - Agrupaciones

        /// <summary>
        /// Lista las agrupaciones activas
        /// </summary>
        /// <param name="regIni">Índice inicial para el paginado del DataTable</param>
        /// <param name="regByPagina">Cantida de registros por página</param>
        /// <param name="idArea">Identificador de la tabla EQ_AREA representa un área operativa</param>
        /// <param name="idEmpresa">Identificador de la tabla SI_EMPRESA representa una empresa</param>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION representa un punto de medición</param>
        /// <param name="esPronostico">Flag que identifica si es la agrupación pertenece al pronóstico por áreas</param>
        /// <returns></returns>
        public object AgrupacionesList(int regIni, int regByPagina, string idArea,
            string idEmpresa, string idPunto, int esPronostico)
        {
            //Obtiene los datos
            List<MePtomedicionDTO> data = this.ListAgrupacionesActivas(idArea, idPunto, idEmpresa, esPronostico);

            //Obtiene el rango de registros que se mostraran
            int totalRegistros = data.Count;

            //Validación: no registros
            if (totalRegistros == 0) return new { data = data, recordsTotal = 0, recordsFiltered = 0 };

            //Obtiene el rango de registros que se mostraran
            if (totalRegistros > regByPagina)
            {
                int tempDiff = totalRegistros - regIni;
                data = (tempDiff < regByPagina) ? data.GetRange(regIni, tempDiff) : data.GetRange(regIni, regByPagina);
            }

            return new { data = data, recordsTotal = totalRegistros, recordsFiltered = totalRegistros };
        }

        /// <summary>
        /// Obtiene la información de una agrupación
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION corresponde a una agrupación</param>
        /// <returns></returns>
        public object AgrupacionesData(int idPunto)
        {
            object res = new object();

            if (idPunto != 0)//Edit
            {
                //Datos de la agrupación
                MePtomedicionDTO dataEntity = FactorySic.
                    GetPrnAgrupacionRepository().GetAgrupacion(idPunto);

                //Detalle de la agrupación
                List<PrnAgrupacionDTO> dataDetalle = FactorySic.
                    GetPrnAgrupacionRepository().GetDetalleAgrupacion(idPunto);

                //Puntos ya seleccionados por otras agrupaciones
                List<int> dataSeleccionados = UtilProdem.
                    ConvertirEntityListEnIntList(FactorySic.
                    GetPrnAgrupacionRepository().ListPuntosSeleccionados(), "Ptomedicodi");

                //Puntos de medición PR03
                List<MePtomedicionDTO> dataPuntos = this.ListPuntosPR03(ConstantesProdem.RegStrTodos,
                    ConstantesProdem.RegStrTodos, ConstantesProdem.RegStrTodos,
                    ConstantesProdem.RegStrTodos, ConstantesProdem.RegStrTodos);

                //Ubicaciones PR03
                List<EqAreaDTO> dataUbicaciones = this.ListUbicacionesPR03(ConstantesProdem.RegStrTodos);

                //Empresas PR03
                List<SiEmpresaDTO> dataEmpresas = this.ListEmpresasPR03(ConstantesProdem.RegStrTodos,
                    ConstantesProdem.RegStrTodos);

                //Obtiene los puntos disponibles
                dataPuntos.RemoveAll(x => dataSeleccionados.Contains(x.Ptomedicodi));

                int entId = idPunto;
                string entName = dataEntity.Ptomedidesc;
                int selArea = dataEntity.Equicodi ?? 0;
                int esPronostico = dataEntity.Ptogrppronostico;

                res = new
                {
                    entId,
                    entName,
                    selArea,
                    esPronostico,
                    selPuntos = dataDetalle,
                    disPuntos = dataPuntos,
                    dataUbicaciones,
                    dataEmpresas
                };
            }
            else//Save
            {
                //Datos para una nueva agrupación

            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selPuntos">Lista de puntos a relacionar con una agrupación</param>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION corresponde a una agrupación</param>
        /// <param name="idArea">Identificador de la tabla EQ_AREA corresponde a un área operativa</param>
        /// <param name="esPronostico">Flag que indica si la agrupación participará del pronóstico por áreas</param>
        /// <param name="nomAgrupacion">Nombre de la agrupación a registrar o actualizar</param>
        /// <param name="idAgrupacion">Identificador de la tabla PRN_PUNTOAGRUPACION corresponde a una agrupación</param>
        /// <param name="nomUsuario">Nombre del usuario</param>
        /// <returns></returns>
        public object AgrupacionesSave(int[] selPuntos, int idPunto, int idArea,
            int esPronostico, string nomAgrupacion, int idAgrupacion, string nomUsuario)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;

            DateTime maxDate = DateTime.ParseExact(ConstantesProdem.DefectoDate,
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            //Valida si existe la agrupación
            MePtomedicionDTO entity = this.GetByIdMePtomedicion(idPunto);

            if (entity != null)
            {
                //Update
                //Cierra la agrupación anterior
                this.CerrarPuntoAgrupacion(idAgrupacion, DateTime.Now);

                //Registra los parametros de la agrupación
                PrnPuntoAgrupacionDTO entityPtoAgrupacion = new PrnPuntoAgrupacionDTO();

                int pkAgrupacion = 0;//pk de la tabla prn_puntoagrupacion
                entityPtoAgrupacion.Ptomedicodi = idPunto;
                entityPtoAgrupacion.Ptogrppronostico = esPronostico;
                entityPtoAgrupacion.Ptogrpfechaini = DateTime.Now;
                entityPtoAgrupacion.Ptogrpfechafin = maxDate;
                entityPtoAgrupacion.Ptogrpusumodificacion = nomUsuario;

                pkAgrupacion = this.SavePuntoAgrupacion(entityPtoAgrupacion);

                //Registra el detalle de la agrupación   
                PrnAgrupacionDTO entityAgrupacion = new PrnAgrupacionDTO();
                foreach (var item in selPuntos)
                {
                    entityAgrupacion = new PrnAgrupacionDTO();
                    entityAgrupacion.Ptogrpcodi = pkAgrupacion;
                    entityAgrupacion.Ptomedicodi = item;
                    entityAgrupacion.Agrupfactor = 1;
                    entityAgrupacion.Agrupfechaini = DateTime.Now;
                    entityAgrupacion.Agrupfechafin = maxDate;

                    this.SavePrnAgrupacion(entityAgrupacion);
                }

                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El registro se actualizó";
            }
            else
            {
                //Save
                //Valida el nombre utilizado
                int valid = this.ValidarNombreAgrupacion(nomAgrupacion);
                if (valid != 1)//No existe un registro con esa descripción
                {
                    //Crea la nueva agrupacion en ME_PTOMEDICION
                    int pkPunto = 0;//pk del punto de medicion
                    MePtomedicionDTO entityPtomedicion = new MePtomedicionDTO();
                    entityPtomedicion.Equicodi = idArea;
                    entityPtomedicion.Emprcodi = -1;
                    entityPtomedicion.Ptomedidesc = nomAgrupacion;
                    entityPtomedicion.Origlectcodi = ConstantesProdem.OriglectcodiAgrupacion;
                    entityPtomedicion.Ptomediestado = ConstantesProdem.RegActivo;
                    entityPtomedicion.Lastuser = nomUsuario;

                    pkPunto = this.SaveMePtomedicion(entityPtomedicion);

                    //Registra los parametros de la agrupación
                    PrnPuntoAgrupacionDTO entityPtoAgrupacion = new PrnPuntoAgrupacionDTO();
                    int pkAgrupacion = 0;//pk de la tabla prn_puntoagrupacion
                    entityPtoAgrupacion.Ptomedicodi = pkPunto;
                    entityPtoAgrupacion.Ptogrppronostico = esPronostico;
                    entityPtoAgrupacion.Ptogrpfechaini = DateTime.Now;
                    entityPtoAgrupacion.Ptogrpfechafin = maxDate;
                    entityPtoAgrupacion.Ptogrpusumodificacion = nomUsuario;

                    pkAgrupacion = this.SavePuntoAgrupacion(entityPtoAgrupacion);

                    //Registra el detalle de la agrupación    
                    PrnAgrupacionDTO entityAgrupacion = new PrnAgrupacionDTO();
                    foreach (var item in selPuntos)
                    {
                        entityAgrupacion = new PrnAgrupacionDTO();
                        entityAgrupacion.Ptogrpcodi = pkAgrupacion;
                        entityAgrupacion.Ptomedicodi = item;
                        entityAgrupacion.Agrupfactor = 1;
                        entityAgrupacion.Agrupfechaini = DateTime.Now;
                        entityAgrupacion.Agrupfechafin = maxDate;

                        this.SavePrnAgrupacion(entityAgrupacion);
                    }

                    typeMsg = ConstantesProdem.MsgSuccess;
                    dataMsg = "El registro se realizó correctamente";
                }
                else
                {
                    typeMsg = ConstantesProdem.MsgError;
                    dataMsg = "Ya existe un registro con esa descripción";
                }
            }

            return new { typeMsg, dataMsg };
        }

        #endregion

        #region Métodos para el Módulo de Configuración - Formulas
        /// <summary>
        /// Permite registrar las formulas seleccionadas al flujo del area
        /// </summary>
        public string DemandaAreaFlujoSave(int idArea, List<PrnFormularelDTO> nuevosPuntos, List<PrnFormularelDTO> listaRegistrados, string usuario)
        {
            string mensaje = string.Empty;
            PrnFormularelDTO entity = new PrnFormularelDTO();
            List<PrnFormularelDTO> listaTemporal = new List<PrnFormularelDTO>();
            List<PrnFormularelDTO> listaSeleccionados = new List<PrnFormularelDTO>();

            //Crea la nueva lista a registrar
            foreach (var pto in nuevosPuntos)
            {
                entity = new PrnFormularelDTO();

                entity.Ptomedicodi = idArea;
                entity.Ptomedicodicalc = pto.Ptomedicodicalc;
                entity.Prfrelfactor = pto.Prfrelfactor;
                entity.Prfrelusucreacion = usuario;
                entity.Prfrelfeccreacion = DateTime.Now;
                entity.Prfrelusumodificacion = usuario;
                entity.Prfrelfecmodificacion = DateTime.Now;

                listaSeleccionados.Add(entity);
            }

            //Valida los registros
            listaRegistrados = listaRegistrados.OrderBy(x => x.Ptomedicodicalc).ToList();
            listaSeleccionados = listaSeleccionados.OrderBy(x => x.Ptomedicodicalc).ToList();

            if (listaSeleccionados.Count != 0)
            {
                if (listaRegistrados.Count != 0)
                {
                    if (listaSeleccionados.Count < listaRegistrados.Count)
                    {
                        listaTemporal = listaRegistrados.ToList();

                        foreach (var ptoReg in listaRegistrados)
                        {
                            foreach (var ptoSel in listaSeleccionados)
                            {
                                if (ptoReg.Ptomedicodicalc == ptoSel.Ptomedicodicalc)
                                {
                                    listaTemporal.Remove(ptoReg);
                                    this.UpdatePrnFormularel(ptoSel); break;
                                }
                            }
                        }
                        //Elimina los excedentes
                        foreach (var ptoTem in listaTemporal)
                        {
                            this.DeletePrnFormularel(idArea, ptoTem.Ptomedicodicalc);
                        }
                    }
                    else if (listaSeleccionados.Count == listaRegistrados.Count)
                    {
                        foreach (var ptoReg in listaRegistrados)
                        {
                            this.DeletePrnFormularel(idArea, ptoReg.Ptomedicodicalc);
                        }
                        foreach (var ptoSel in listaSeleccionados)
                        {
                            this.SavePrnFormularel(ptoSel);
                        }
                    }
                    else
                    {
                        listaTemporal = listaSeleccionados.ToList();

                        foreach (var ptoSel in listaSeleccionados)
                        {
                            foreach (var ptoReg in listaRegistrados)
                            {
                                if (ptoSel.Ptomedicodicalc == ptoReg.Ptomedicodicalc)
                                {
                                    listaTemporal.Remove(ptoSel);
                                    this.UpdatePrnFormularel(ptoSel); break;
                                }
                            }
                        }
                        //Registra los excedentes
                        foreach (var ptoTem in listaTemporal)
                        {
                            this.SavePrnFormularel(ptoTem);
                        }
                    }

                    mensaje = "La actualización se realizó de manera exitosa...";
                }
                else
                {
                    foreach (var ptoSel in listaSeleccionados)
                    {
                        this.SavePrnFormularel(ptoSel);
                    }

                    mensaje = "El registro se realizó de manera exitosa...";
                }
            }
            else
            {
                mensaje = "No ha seleccionado ninguna formula...";
            }

            return mensaje;
        }

        /// <summary>
        /// Lista las formulas que no se encuentran seleccionadas de la tabla ME_PTOMEDICION
        /// </summary>
        public List<PrnFormularelDTO> FormulasRestantesByLista(List<PrnFormularelDTO> seleccionados)
        {
            List<PrnFormularelDTO> entitys = new List<PrnFormularelDTO>();
            List<PrnFormularelDTO> auxiliar = new List<PrnFormularelDTO>();
            entitys = this.ListFormulasByUsuario();
            auxiliar = entitys.ToList();

            foreach (var a in seleccionados)
            {
                foreach (var b in auxiliar)
                {
                    if (b.Ptomedicodicalc == a.Ptomedicodicalc)
                    {
                        entitys.Remove(b);
                        break;
                    }
                }
            }

            return entitys;
        }
        #endregion

        #region Métodos para el Módulo de Configuración - Motivos
        public List<EveSubcausaeventoDTO> ListaMotivo()
        {

            return FactorySic.GetEveSubcausaeventoRepository().ObtenerPorCausa(ConstantesProdem.VarExoPronostico);
        }

        #endregion

        #region Métodos para el Módulo de Configuración - Variables Exogenas

        public void ObtenerVariablesExogenas()
        {

            PrnExogenamedicionDTO entity = new PrnExogenamedicionDTO();
            List<PrnAreamedicionDTO> ListaCiudad = new List<PrnAreamedicionDTO>();
            List<PrnVariableexogenaDTO> ListaExogena = new List<PrnVariableexogenaDTO>();
            int[] varexoArray = new int[4] { ConstantesProdem.VarexocodiTemperatura,
                ConstantesProdem.VarexocodiSentermica, ConstantesProdem.VarexocodiNubosidad, ConstantesProdem.VarexocodiHumedad };

            string urlbase = "http://api.openweathermap.org/data/2.5/forecast?";
            string key = "b5c2c435b3ab06872a58821740a8bba7";
            string url = string.Empty;
            //----------------------------------------------------------------------------------------
            //Consigue la lista de Ciudades validas para el registro
            //----------------------------------------------------------------------------------------
            //A. Valida el estado
            //----------------------------------------------------------------------------------------
            ListaCiudad = ListVarexoCiudad().Where(x => x.Areamedestado.Equals("A")).ToList();
            ListaExogena = ListPrnVariableExogena();

            //----------------------------------------------------------------------------------------
            //Inicia la obtención y registro de las variables exogenas del "api" 
            //----------------------------------------------------------------------------------------            
            foreach (var item in ListaCiudad)
            {
                url = urlbase + "q=";
                entity.Aremedcodi = item.Areamedcodi;
                //------------------------------------------------------------------------------------
                using (WebClient wc = new WebClient())
                {
                    string validnom = string.Empty;
                    validnom = item.Areanomb.Trim();

                    url += validnom + ",pe&units=metric&mode=json&appid=" + key;
                    var str = wc.DownloadString(url);

                    var dynamicobject = JsonConvert.DeserializeObject<dynamic>(str);

                    int x = 0;
                    foreach (var exogena in ListaExogena)
                    {
                        x++;
                        entity.Varexocodi = exogena.Varexocodi;
                        if (entity.Varexocodi == ConstantesProdem.VarexocodiTemperatura || entity.Varexocodi == ConstantesProdem.VarexocodiSentermica)
                        {
                            entity.Tipoinfocodi = ConstantesProdem.TipoinfocodiCentigrados;
                        }
                        if (entity.Varexocodi == ConstantesProdem.VarexocodiNubosidad || entity.Varexocodi == ConstantesProdem.VarexocodiHumedad)
                        {
                            entity.Tipoinfocodi = ConstantesProdem.TipoinfocodiPorcentaje;
                        }

                        string sFecha = DateTime.Now.AddDays(1).ToString(ConstantesProdem.FormatoFecha);
                        DateTime diaInicial = DateTime.ParseExact(sFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
                        DateTime diaFinal = diaInicial.AddDays(3);
                        int cantidad = Int32.Parse(dynamicobject.cnt.ToString());
                        int indiceApi = 0;
                        DateTime diaApi;

                        while (diaInicial <= diaFinal)
                        {
                            for (int i = 0; i < cantidad; i++)
                            {
                                diaApi = Convert.ToDateTime(dynamicobject.list[i].dt_txt);
                                if (diaApi == diaInicial)
                                {
                                    indiceApi = i;
                                }
                            }

                            //Temperatura
                            if (entity.Varexocodi == ConstantesProdem.VarexocodiTemperatura)
                            {
                                int contador = 0;
                                for (int i = indiceApi; i <= 7 + indiceApi; i++)
                                {
                                    entity.GetType().GetProperty("H" + (contador + 1)).SetValue(entity, decimal.Parse(dynamicobject.list[i].main.temp.ToString()));
                                    //obtiene posiciones intermedias
                                    if (diaInicial == diaFinal && i == 7)
                                    {
                                        entity.GetType().GetProperty("H" + (contador + 2)).SetValue(entity, decimal.Parse(dynamicobject.list[i].main.temp.ToString()));
                                        entity.GetType().GetProperty("H" + (contador + 3)).SetValue(entity, decimal.Parse(dynamicobject.list[i].main.temp.ToString()));
                                    }
                                    else
                                    {
                                        decimal razon = decimal.Round((decimal.Parse(dynamicobject.list[i + 1].main.temp.ToString()) - decimal.Parse(dynamicobject.list[i].main.temp.ToString())) / 3, 2);
                                        decimal primeraH = decimal.Parse(dynamicobject.list[i].main.temp.ToString()) + razon;
                                        decimal segundaH = primeraH + razon;
                                        entity.GetType().GetProperty("H" + (contador + 2)).SetValue(entity, primeraH);
                                        entity.GetType().GetProperty("H" + (contador + 3)).SetValue(entity, segundaH);
                                    }
                                    contador = contador + 3;
                                }
                            }

                            //Sensaciom Termica
                            if (entity.Varexocodi == ConstantesProdem.VarexocodiSentermica)
                            {
                                int contador = 0;
                                for (int i = indiceApi; i <= 7 + indiceApi; i++)
                                {
                                    entity.GetType().GetProperty("H" + (contador + 1)).SetValue(entity, decimal.Parse(dynamicobject.list[i].main.temp.ToString()));
                                    entity.GetType().GetProperty("H" + (contador + 2)).SetValue(entity, decimal.Parse(dynamicobject.list[i].main.temp.ToString()));
                                    entity.GetType().GetProperty("H" + (contador + 3)).SetValue(entity, decimal.Parse(dynamicobject.list[i].main.temp.ToString()));
                                    contador = contador + 3;
                                }
                            }

                            //Nubosidad
                            if (entity.Varexocodi == ConstantesProdem.VarexocodiNubosidad)
                            {
                                int contador = 0;
                                for (int i = indiceApi; i <= 7 + indiceApi; i++)
                                {
                                    entity.GetType().GetProperty("H" + (contador + 1)).SetValue(entity, decimal.Parse(dynamicobject.list[i].clouds.all.ToString()));
                                    //obtiene posiciones intermedias
                                    if (diaInicial == diaFinal && i == 7)
                                    {
                                        entity.GetType().GetProperty("H" + (contador + 2)).SetValue(entity, decimal.Parse(dynamicobject.list[i].clouds.all.ToString()));
                                        entity.GetType().GetProperty("H" + (contador + 3)).SetValue(entity, decimal.Parse(dynamicobject.list[i].clouds.all.ToString()));
                                    }
                                    else
                                    {
                                        decimal razon = decimal.Round((decimal.Parse(dynamicobject.list[i + 1].clouds.all.ToString()) - decimal.Parse(dynamicobject.list[i].clouds.all.ToString())) / 3, 2);
                                        decimal primeraH = decimal.Parse(dynamicobject.list[i].clouds.all.ToString()) + razon;
                                        decimal segundaH = primeraH + razon;
                                        entity.GetType().GetProperty("H" + (contador + 2)).SetValue(entity, primeraH);
                                        entity.GetType().GetProperty("H" + (contador + 3)).SetValue(entity, segundaH);
                                    }
                                    //entity.GetType().GetProperty("H" + (contador + 2)).SetValue(entity, decimal.Parse(dynamicobject.list[i].clouds.all.ToString()));
                                    //entity.GetType().GetProperty("H" + (contador + 3)).SetValue(entity, decimal.Parse(dynamicobject.list[i].clouds.all.ToString()));
                                    contador = contador + 3;
                                }
                            }

                            //Humedad
                            if (entity.Varexocodi == ConstantesProdem.VarexocodiHumedad)
                            {
                                int contador = 0;
                                for (int i = indiceApi; i <= 7 + indiceApi; i++)
                                {
                                    entity.GetType().GetProperty("H" + (contador + 1)).SetValue(entity, decimal.Parse(dynamicobject.list[i].main.humidity.ToString()));
                                    //obtiene posiciones intermedias
                                    if (diaInicial == diaFinal && i == 7)
                                    {
                                        entity.GetType().GetProperty("H" + (contador + 2)).SetValue(entity, decimal.Parse(dynamicobject.list[i].main.humidity.ToString()));
                                        entity.GetType().GetProperty("H" + (contador + 3)).SetValue(entity, decimal.Parse(dynamicobject.list[i].main.humidity.ToString()));
                                    }
                                    else
                                    {
                                        decimal razon = decimal.Round((decimal.Parse(dynamicobject.list[i + 1].main.humidity.ToString()) - decimal.Parse(dynamicobject.list[i].main.humidity.ToString())) / 3, 2);
                                        decimal primeraH = decimal.Parse(dynamicobject.list[i].main.humidity.ToString()) + razon;
                                        decimal segundaH = primeraH + razon;
                                        entity.GetType().GetProperty("H" + (contador + 2)).SetValue(entity, primeraH);
                                        entity.GetType().GetProperty("H" + (contador + 3)).SetValue(entity, segundaH);
                                    }
                                    //entity.GetType().GetProperty("H" + (contador + 2)).SetValue(entity, decimal.Parse(dynamicobject.list[i].main.humidity.ToString()));
                                    //entity.GetType().GetProperty("H" + (contador + 3)).SetValue(entity, decimal.Parse(dynamicobject.list[i].main.humidity.ToString()));
                                    contador = contador + 3;
                                }
                            }
                            entity.Exmedifecha = diaInicial;
                            entity.Exmedifeccreacion = DateTime.Now;

                            //Valida existencia
                            PrnExogenamedicionDTO codigo = GetByIdPrnExogenamedicion(entity.Varexocodi, entity.Aremedcodi, entity.Exmedifecha);
                            entity.Exmedicodi = codigo.Exmedicodi;

                            if (entity.Exmedicodi != 0)
                            {
                                this.UpdatePrnExogenamedicion(entity);
                            }
                            else
                            {
                                this.SavePrnExogenamedicion(entity);

                            }

                            diaInicial = diaInicial.AddDays(1);
                        }
                    }
                }
            }
        }

        //Servicio para mostrar los datos de variables exogenas.
        public object ExogenaModelo(string fecini, string fecfin, string[] ciud)
        {

            object modelo = new object();

            string ciudades;
            string lista = "";
            string listaciud = "";

            //Convierte las fechas a tipo DateTime
            DateTime ParseFecini = DateTime.ParseExact(fecini, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime ParseFecfin = DateTime.ParseExact(fecfin, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            //Calcula la cantidad de filas para armar el registro
            int CantidadFilas;
            int CantidadFilasTotales;
            int CantidadColumnas = 6;
            CantidadFilas = (ParseFecfin - ParseFecini).Days;
            CantidadFilas = CantidadFilas + 1;
            CantidadFilas = CantidadFilas * ConstantesProdem.Itv60min;
            CantidadFilasTotales = CantidadFilas * ciud.Length;

            //Arreglo detalle
            string[][] ListaDetalle = new string[CantidadFilasTotales][];
            for (int i = 0; i < CantidadFilasTotales; i++)
            {
                ListaDetalle[i] = new string[CantidadColumnas];
            }

            //LLendado de informacion
            string[] Intervalos = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv60min);

            //Por ciudad
            int c = 0;
            int f = 0;
            while (c < ciud.Length)
            {
                //Para un dia
                //int f = 0;
                int d = 0;
                int l = 0;
                while (l < CantidadFilas)
                {

                    DateTime FecAux = ParseFecini.AddDays(d);
                    // Log.Info("Lista Variable Exogena por Ciudad - ListExomedicionByCiudadDate");
                    List<PrnExogenamedicionDTO> ListaExomedicion = this.ListExomedicionByCiudadDate(int.Parse(ciud[c]), FecAux);//model.FecAux);

                    for (int j = 0; j < ConstantesProdem.Itv60min; j++)
                    {
                        //Fecha e Intervalo Horario
                        ListaDetalle[f][0] = FecAux.ToString(ConstantesProdem.FormatoFecha) + " - " + Intervalos[j];

                        //Llenado de las varibles exogenas                    
                        if (ListaExomedicion.Count() != 0)
                        {
                            ListaDetalle[f][1] = ListaExomedicion[0].AreaNomb.Trim();
                            //Temperatura
                            var Exomedicion = ListaExomedicion.Where(x => x.Varexocodi == ConstantesProdem.VarexocodiTemperatura).First();
                            if (Exomedicion != null)
                            {
                                var valid = Exomedicion.GetType().GetProperty(String.Concat("H", (j + 1).ToString())).GetValue(Exomedicion, null);
                                if (valid != null)
                                {
                                    ListaDetalle[f][2] = valid.ToString();
                                }
                                else
                                {
                                    ListaDetalle[f][2] = "";
                                }
                            }
                            //Sensación Térmica
                            Exomedicion = new PrnExogenamedicionDTO();
                            Exomedicion = ListaExomedicion.Where(x => x.Varexocodi == ConstantesProdem.VarexocodiSentermica).First();
                            if (Exomedicion != null)
                            {
                                var valid = Exomedicion.GetType().GetProperty(String.Concat("H", (j + 1).ToString())).GetValue(Exomedicion, null);
                                if (valid != null)
                                {
                                    ListaDetalle[f][3] = valid.ToString();
                                }
                                else
                                {
                                    ListaDetalle[f][3] = "";
                                }
                            }
                            //Nubosidad
                            Exomedicion = new PrnExogenamedicionDTO();
                            Exomedicion = ListaExomedicion.Where(x => x.Varexocodi == ConstantesProdem.VarexocodiNubosidad).First();
                            if (Exomedicion != null)
                            {
                                var valid = Exomedicion.GetType().GetProperty(String.Concat("H", (j + 1).ToString())).GetValue(Exomedicion, null);
                                if (valid != null)
                                {
                                    ListaDetalle[f][4] = valid.ToString();
                                }
                                else
                                {
                                    ListaDetalle[f][4] = "";
                                }
                            }
                            //Humedad
                            Exomedicion = new PrnExogenamedicionDTO();
                            Exomedicion = ListaExomedicion.Where(x => x.Varexocodi == ConstantesProdem.VarexocodiHumedad).First();
                            if (Exomedicion != null)
                            {
                                var valid = Exomedicion.GetType().GetProperty(String.Concat("H", (j + 1).ToString())).GetValue(Exomedicion, null);
                                if (valid != null)
                                {
                                    ListaDetalle[f][5] = valid.ToString();
                                }
                                else
                                {
                                    ListaDetalle[f][5] = "";
                                }
                            }
                        }

                        f++;
                        l++;
                    }

                    d++;
                }

                c++;

            }
            return ListaDetalle;

        }

        public List<PrnVariableexogenaDTO> ListPrnVariableExogena()
        {
            return FactorySic.GetPrnVariableexogenaRepository().List();
        }

        #endregion

        #region Métodos del Módulo de Depuración

        /// <summary>
        /// Lista la demanda reportada por los agentes a nivel de puntos (paginado)
        /// </summary>
        /// <param name="regIni"></param>
        /// <param name="regByPagina"></param>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="idLectura"></param>
        /// <param name="regFecha"></param>
        /// <param name="idArea"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idPunto"></param>
        /// <param name="idPerfil"></param>
        /// <param name="idClasificacion"></param>
        /// <param name="idAreaOperativa"></param>
        /// <param name="idJustificacion"></param>
        /// <param name="esBarra"></param>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public object DepuracionPorPuntosList(int regIni, int regByPagina, int idTipoEmpresa, int idLectura,
            string regFecha, string idArea, string idEmpresa, string idPunto, string idPerfil, string idClasificacion,
            string idAreaOperativa, List<string> idJustificacion, bool esBarra, string idOrden)
        {
            //Obtiene los datos
            List<PrnClasificacionDTO> data = this.ListDemandaReportadaPorPunto(idPunto,
                idLectura, idTipoEmpresa, regFecha, idArea, idEmpresa, idPerfil, idClasificacion, idAreaOperativa, esBarra, idOrden);

            //filtra por justificación
            List<PrnClasificacionDTO> entitys = new List<PrnClasificacionDTO>();
            if (idJustificacion.Count != 0)
            {
                foreach (PrnClasificacionDTO a in data)
                {
                    if (a.ListSubcausacodi.Intersect(idJustificacion).Any()) entitys.Add(a);
                }
            }
            else
            {
                entitys = data.ToList();
            }

            //Obtiene el rango de registros que se mostraran
            int totalRegistros = entitys.Count;

            //Validación: no registros
            if (totalRegistros == 0) return new { data = entitys, recordsTotal = 0, recordsFiltered = 0 };

            //Obtiene el rango de registros que se mostraran
            if (totalRegistros > regByPagina)
            {
                int tempDiff = totalRegistros - regIni;
                entitys = (tempDiff < regByPagina) ? entitys.GetRange(regIni, tempDiff) : entitys.GetRange(regIni, regByPagina);
            }

            return new { data = entitys, recordsTotal = totalRegistros, recordsFiltered = totalRegistros };
        }

        /// <summary>
        /// Lista la demanda reportada por los agentes a nivel de agrupaciones (paginado)
        /// </summary>
        /// <param name="regIni"></param>
        /// <param name="regByPagina"></param>
        /// <param name="esPronostico"></param>
        /// <param name="idArea"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idPunto"></param>
        /// <param name="idlectura"></param>
        /// <param name="regFecha"></param>
        /// <returns></returns>
        public object DepuracionPorAgrupacionesList(int regIni, int regByPagina, string esPronostico,
            string idArea, string idEmpresa, string idPunto, int idlectura, string regFecha)
        {
            //Obtiene los datos
            List<PrnAgrupacionDTO> entitys = this.ListDemandaReportadaPorAgrupacion(idPunto, idlectura, regFecha, esPronostico, idArea, idEmpresa);

            //Formato de presentación
            int tempId = 0;
            PrnAgrupacionDTO row = new PrnAgrupacionDTO();
            List<PrnAgrupacionDTO> tabla = new List<PrnAgrupacionDTO>();

            foreach (PrnAgrupacionDTO e in entitys)
            {
                if (tempId != e.Ptomedicodi)
                {
                    tempId = e.Ptomedicodi;

                    row = new PrnAgrupacionDTO();
                    row.ListaDetalle = new List<object>();

                    row.Ptomedicodi = e.Ptomedicodi;
                    row.Ptomedidesc = e.Ptomedidesc;
                    row.Areanomb = e.Areanomb;
                    row.Emprnomb = e.Emprnomb;
                    row.Ptogrppronostico = e.Ptogrppronostico;
                    row.Meditotal = e.Meditotal;
                    row.Prnm48tipo = (e.Prnm48tipo != 1) ? e.Prnm48tipo : 1;

                    if (e.Tipoemprcodi != -1)
                    {
                        row.ListaDetalle.Add(
                            new
                            {
                                id = e.Ptogrphijocodi,
                                name = e.Ptogrphijodesc,
                                sum = e.Meditotal,
                                type = e.Tipoemprcodi,
                                state = e.Prnmestado
                            });
                    }

                    tabla.Add(row);
                }
                else
                {
                    row.Meditotal += e.Meditotal;
                    row.Prnm48tipo = (e.Prnm48tipo != 1) ? e.Prnm48tipo : 1;

                    if (e.Tipoemprcodi != -1)
                    {
                        row.ListaDetalle.Add(
                            new
                            {
                                id = e.Ptogrphijocodi,
                                name = e.Ptogrphijodesc,
                                sum = e.Meditotal,
                                type = e.Tipoemprcodi,
                                state = e.Prnmestado
                            });
                    }
                }
            }

            //Obtiene el rango de registros que se mostraran
            int totalRegistros = tabla.Count;

            //Validación: no registros
            if (totalRegistros == 0) return new { data = tabla, recordsTotal = 0, recordsFiltered = 0 };

            //Obtiene el rango de registros que se mostraran
            if (totalRegistros > regByPagina)
            {
                int tempDiff = totalRegistros - regIni;
                tabla = (tempDiff < regByPagina) ? tabla.GetRange(regIni, tempDiff) : tabla.GetRange(regIni, regByPagina);
            }

            return new { data = tabla, recordsTotal = totalRegistros, recordsFiltered = totalRegistros };
        }

        /// <summary>
        /// General el modelo de datos del módulo por puntos
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="idLectura">Identificador de la tabla ME_LECTURA</param>
        /// <param name="idTipoEmpresa">Identificador del tipo de empresa</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public object DepuracionPorPuntosDatos(int idPunto, int idLectura, int idTipoEmpresa, string regFecha)
        {
            List<object> data = new List<object>();

            //Obtiene la configuración del punto
            DateTime parseDate = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            PrnConfiguracionDTO regconfiguracion = this.ParametrosGetConfiguracion(idPunto, ConstantesProdem.DefectoByPunto, parseDate);

            //Obtiene los datos del perfil patrón
            PrnPatronModel regPatron = this.GetPatron(idPunto, ConstantesProdem.ProcPatronDemandaEjecutada, parseDate, regconfiguracion);

            //a) Obtiene los datos de la demanda ejeutada
            List<PrnMedicion48DTO> dataDemanda = this.GetDemandaPorPunto(idPunto, idLectura, regFecha);

            //a.*) Validación
            //if (dataDemanda.Count == 0) return new { valid = false };

            //a.1) Demanda reportada total (reportado + ajuste auto + ajuste manual)
            PrnMedicion48DTO sFinal = dataDemanda.FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
            decimal[] dFinal = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sFinal);

            //a.2) Demanda reportada por el agente
            PrnMedicion48DTO sReportado = dataDemanda.FirstOrDefault(x => x.Prnm48tipo == 1) ?? new PrnMedicion48DTO();
            decimal[] dReportado = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sReportado);

            //a.3) Ajuste automático
            PrnMedicion48DTO sAuto = dataDemanda.FirstOrDefault(x => x.Prnm48tipo == 2) ?? new PrnMedicion48DTO();
            decimal[] dAuto = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sAuto);

            //a.4) Ajuste manual
            PrnMedicion48DTO sManual = dataDemanda.FirstOrDefault(x => x.Prnm48tipo == 3) ?? new PrnMedicion48DTO();
            decimal[] dManual = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sManual);

            //a.5) Demanda semanal, solo si no se consulta por la misma
            decimal[] dSemanal = new decimal[ConstantesProdem.Itv30min];
            if (idLectura != ConstantesProdem.LectcodiDemPrevSemanal)
            {
                List<PrnMedicion48DTO> dataSemanal = this.GetDemandaPorPunto(idPunto, ConstantesProdem.LectcodiDemPrevSemanal, regFecha);
                PrnMedicion48DTO sSemanal = dataSemanal.FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                dSemanal = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sSemanal);
            }

            //b) Formatos de presentación
            object entity;
            //b.1) Intervalos de tiempo
            entity = new
            {
                id = "intervalos",
                label = "Hora",
                data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                htrender = "hora",
                hcrender = "categoria"
            };
            data.Add(entity);

            //b.2) Mediciones historicas
            for (int i = 0; i < regPatron.Mediciones.Count; i++)
            {
                entity = new
                {
                    id = "med" + (i + 1).ToString(),
                    label = regPatron.StrFechas[i],
                    data = regPatron.Mediciones[i],
                    htrender = "no",
                    hcrender = "normal",
                    label2 = regPatron.StrFechasTarde[i],
                    slunes = regPatron.EsLunes
                };
                data.Add(entity);
            }

            //b.3) Patron
            entity = new
            {
                id = "patron",
                label = (regconfiguracion.Prncfgflagdefecto == ConstantesProdem.RegSi) ? "Patrón" : "Patrón(Act)",
                data = regPatron.Patron,
                htrender = "normal",
                hcrender = "patron"
            };
            data.Add(entity);

            //b.3.1) Patrón defecto
            entity = new
            {
                id = "defecto",
                label = (regconfiguracion.Prncfgflagdefecto == ConstantesProdem.RegSi) ? "Defecto(Act)" : "Defecto",
                data = regPatron.PatronDefecto,
                htrender = "normal",
                hcrender = "defecto"
            };
            data.Add(entity);

            //b.3.2) Demanda semanal, solo si no se consulta por la misma
            if (idLectura != ConstantesProdem.LectcodiDemPrevSemanal)
            {
                entity = new
                {
                    id = "semanal",
                    label = "Semanal",
                    data = dSemanal,
                    htrender = "normal",
                    hcrender = "defecto"
                };
                data.Add(entity);
            }

            //b.4) Márgenes de error
            decimal[] tempPatron = (regconfiguracion.Prncfgflagdefecto == ConstantesProdem.RegSi)
                ? regPatron.PatronDefecto
                : regPatron.Patron;
            decimal[] rMin = new decimal[ConstantesProdem.Itv30min];
            decimal[] rMax = new decimal[ConstantesProdem.Itv30min];
            int j = 0;
            while (j < ConstantesProdem.Itv30min)
            {
                decimal fMin = 1 - (regconfiguracion.Prncfgporcdsvptrn * 0.01M) ?? 0;
                decimal fMax = 1 + (regconfiguracion.Prncfgporcdsvptrn * 0.01M) ?? 0;
                rMin[j] = tempPatron[j] * fMin;
                rMax[j] = tempPatron[j] * fMax;
                j++;
            }

            entity = new
            {
                id = "rmin",
                label = "EMin",
                data = rMin,
                htrender = "no",
                hcrender = "margen"
            };
            data.Add(entity);

            entity = new
            {
                id = "rmax",
                label = "EMax",
                data = rMax,
                htrender = "no",
                hcrender = "margen"
            };
            data.Add(entity);

            //b.5) Información reportada - "Base" para el ht
            entity = new
            {
                id = "base",
                label = "Reportado(R)",
                data = dReportado,
                htrender = "normal",
                hcrender = "normal"
            };
            data.Add(entity);

            //b.6) Ajuste automático
            entity = new
            {
                id = "auto",
                label = "Ajuste(A)",
                data = dAuto,
                htrender = "normal",
                hcrender = "no"
            };
            data.Add(entity);

            //b.7) Ajuste Manual - "Ajuste" para el ht
            entity = new
            {
                id = "ajuste",
                label = "Ajuste(M)",
                data = dManual,
                htrender = "edit",
                hcrender = "no"
            };
            data.Add(entity);

            //b.8) Final
            entity = new
            {
                id = "final",
                label = "Final(R + A + M)",
                data = dFinal,
                htrender = "final",
                hcrender = "final"
            };
            data.Add(entity);

            return new { data = data, cfg = regconfiguracion, valid = true };
        }

        /// <summary>
        /// General el modelo de datos del módulo por agrupaciones
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="idLectura">Identificador de la tabla ME_LECTURA</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public object DepuracionPorAgrupacionesDatos(int idPunto, int idLectura, string regFecha)
        {
            List<object> data = new List<object>();
            List<PrnMedicion48DTO> dataDemanda = new List<PrnMedicion48DTO>();

            //Obtiene la configuración del punto
            DateTime parseDate = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            PrnConfiguracionDTO regconfiguracion = this.ParametrosGetConfiguracion(idPunto, ConstantesProdem.DefectoByAgrupacion, parseDate);

            //Obtiene los datos del perfil patrón
            PrnPatronModel regPatron = this.GetPatron(idPunto, ConstantesProdem.ProcPatronDemandaEjecutadaAgrupada, parseDate, regconfiguracion);

            //a) Obtiene los datos de la demanda segun el tipo de lectura
            dataDemanda = this.GetDemandaPorAgrupacion(idPunto, idLectura, regFecha);

            //a.*) Validación
            if (dataDemanda.Count == 0) return new { valid = false };

            //a.1) Demanda reportada total (reportado + ajuste manual)
            PrnMedicion48DTO sFinal = dataDemanda.FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
            decimal[] dFinal = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sFinal);

            //a.2) Demanda reportada por el agente
            PrnMedicion48DTO sReportado = dataDemanda.FirstOrDefault(x => x.Prnm48tipo == 1) ?? new PrnMedicion48DTO();
            decimal[] dReportado = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sReportado);

            //a.3) Ajuste manual
            PrnMedicion48DTO sManual = dataDemanda.FirstOrDefault(x => x.Prnm48tipo == 2) ?? new PrnMedicion48DTO();
            decimal[] dManual = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sManual);

            //b) Formatos de presentación
            object entity;
            //b.1) Intervalos de tiempo
            entity = new
            {
                id = "intervalos",
                label = "Hora",
                data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                htrender = "hora",
                hcrender = "categoria"
            };
            data.Add(entity);

            //b.2) Mediciones historicas
            for (int i = 0; i < regPatron.Mediciones.Count; i++)
            {
                entity = new
                {
                    id = "med" + (i + 1).ToString(),
                    label = regPatron.StrFechas[i],
                    data = regPatron.Mediciones[i],
                    htrender = "no",
                    hcrender = "normal",
                    label2 = regPatron.StrFechasTarde[i],
                    slunes = regPatron.EsLunes
                };
                data.Add(entity);
            }

            //b.3) Patron
            entity = new
            {
                id = "patron",
                label = "Patrón",
                data = regPatron.Patron,
                htrender = "normal",
                hcrender = "patron"
            };
            data.Add(entity);

            //b.4) Márgenes de error
            decimal[] tempPatron = (regconfiguracion.Prncfgflagdefecto == ConstantesProdem.RegSi)
                ? regPatron.PatronDefecto
                : regPatron.Patron;
            decimal[] rMin = new decimal[ConstantesProdem.Itv30min];
            decimal[] rMax = new decimal[ConstantesProdem.Itv30min];
            int j = 0;
            while (j < ConstantesProdem.Itv30min)
            {
                decimal fMin = 1 - (regconfiguracion.Prncfgporcdsvptrn * 0.01M) ?? 0;
                decimal fMax = 1 + (regconfiguracion.Prncfgporcdsvptrn * 0.01M) ?? 0;
                rMin[j] = tempPatron[j] * fMin;
                rMax[j] = tempPatron[j] * fMax;
                j++;
            }

            entity = new
            {
                id = "rmin",
                label = "EMin",
                data = rMin,
                htrender = "no",
                hcrender = "margen"
            };
            data.Add(entity);

            entity = new
            {
                id = "rmax",
                label = "EMax",
                data = rMax,
                htrender = "no",
                hcrender = "margen"
            };
            data.Add(entity);

            //b.5) Información reportada - "Base" para el ht
            entity = new
            {
                id = "base",
                label = "Reportado(R)",
                data = dReportado,
                htrender = "normal",
                hcrender = "normal"
            };
            data.Add(entity);

            //b.6) Ajuste Manual - "Ajuste" para el ht
            entity = new
            {
                id = "ajuste",
                label = "Ajuste(M)",
                data = dManual,
                htrender = "edit",
                hcrender = "no"
            };
            data.Add(entity);

            //b.7) Final
            entity = new
            {
                id = "final",
                label = "Final(R + M)",
                data = dFinal,
                htrender = "final",
                hcrender = "final"
            };
            data.Add(entity);

            return new { data = data, cfg = regconfiguracion, valid = true };
        }

        /// <summary>
        /// Devuelve la tabla de demanda por puntos reportada por los agentes c/s procesos automáticos
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION (Uno o más)</param>
        /// <param name="idLectura">Identificador de la tabla ME_LECTURA</param>
        /// <param name="idTipoEmpresa">Identificador del tipo de empresa</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="idArea">Identificador de la Subestación (Uno o más)</param>
        /// <param name="idEmpresa">Identificador de la Empresa (Uno o más)</param>
        /// <param name="idPerfil">Identificador del Perfíl (Uno o más)</param>
        /// <param name="idClasificacion">Identificador de la Clasificación (Uno o más)</param>
        /// <param name="idAreaOperativa">Identificador del área operativa</param>
        /// <param name="esBarra">Flag indicador para la busqueda puntos relacionados a una barra CP</param>
        /// <param name="idOrden">Identificador del Orden</param>
        /// <returns></returns>
        public List<PrnClasificacionDTO> ListDemandaReportadaPorPunto(string idPunto, int idLectura,
            int idTipoEmpresa, string regFecha, string idArea, string idEmpresa, string idPerfil,
            string idClasificacion, string idAreaOperativa, bool esBarra, string idOrden)
        {
            string idTipo = "-1";
            List<PrnClasificacionDTO> entitys = new List<PrnClasificacionDTO>();
            DateTime Fecfin = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture).AddDays(1);

            switch (idLectura)
            {
                case ConstantesProdem.LectcodiDemEjecDiario:
                    idTipo = string.Join(",",
                        new int[] { ConstantesProdem.PrntDemandaEjecutadaAjusteAuto,
                            ConstantesProdem.PrntDemandaEjecutadaAjusteManual });
                    break;
                case ConstantesProdem.LectcodiDemPrevDiario:
                    idTipo = string.Join(",",
                        new int[] { ConstantesProdem.PrntDemandaPrevistaAjusteAuto,
                            ConstantesProdem.PrntDemandaPrevistaAjusteManual });
                    break;
                case ConstantesProdem.LectcodiDemPrevSemanal:
                    idTipo = string.Join(",",
                        new int[] { ConstantesProdem.PrntDemandaSemanalAjusteAuto,
                            ConstantesProdem.PrntDemandaSemanalAjusteManual });
                    break;
            }

            if (!esBarra)
            {
                entitys = FactorySic.GetPrnClasificacionRepository().ListDemandaClasificada(idPunto, regFecha, idTipo, idLectura,
                    ConstantesProdem.TipoinfocodiMWDemanda, idTipoEmpresa, idArea, idEmpresa, idPerfil,
                    idClasificacion, Fecfin.ToString(ConstantesProdem.FormatoFecha), idAreaOperativa, idOrden);
            }
            else
            {
                entitys = FactorySic.GetPrnClasificacionRepository().ListDemandaClasificadaBarrasCP(idPunto, regFecha, idTipo, idLectura,
                    ConstantesProdem.TipoinfocodiMWDemanda, idTipoEmpresa, idArea, idEmpresa, idPerfil,
                    idClasificacion, Fecfin.ToString(ConstantesProdem.FormatoFecha), idAreaOperativa, idOrden);
            }

            return entitys;
        }

        /// <summary>
        /// Devuelve la tabla de demanda por agrupaciones reportada por los agentes
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION (Uno o más)</param>
        /// <param name="idLectura">Identificador de la tabla ME_LECTURA</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="esPronostico">Flag que indica si la agrupación participa en el pronóstico de la demanda</param>
        /// <param name="idArea">Identificador de la Subestación (Uno o más)</param>
        /// <param name="idEmpresa">Identificador de la Empresa (Uno o más)</param>
        /// <returns></returns>
        public List<PrnAgrupacionDTO> ListDemandaReportadaPorAgrupacion(string idPunto, int idLectura, string regFecha, string esPronostico,
            string idArea, string idEmpresa)
        {
            int idPrnTipo = -1;
            string idTipo = "-1";

            int idPronostico = (esPronostico == ConstantesProdem.RegSi) ? 1 : 0;

            if (idLectura == ConstantesProdem.LectcodiDemEjecDiario)
            {
                idPrnTipo = ConstantesProdem.PrntDemandaEjecutadaAjusteManual;
                idTipo = string.Join(",", new int[] { ConstantesProdem.PrntDemandaEjecutadaAjusteAuto,
                    ConstantesProdem.PrntDemandaEjecutadaAjusteManual });
            }
            if (idLectura == ConstantesProdem.LectcodiDemPrevDiario)
            {
                idPrnTipo = ConstantesProdem.PrntDemandaPrevistaAjusteManual;
                idTipo = ConstantesProdem.PrntDemandaPrevistaAjusteManual.ToString();
            }

            return FactorySic.GetPrnAgrupacionRepository().ListDemandaAgrupada(ConstantesProdem.OriglectcodiAgrupacion,
                ConstantesProdem.OriglectcodiPR03, idLectura, ConstantesProdem.TipoinfocodiMWDemanda,
                regFecha, idTipo, idArea, idEmpresa, idPunto, idPronostico, idPrnTipo);
        }

        /// <summary>
        /// Devuelve el perfil patrón actualizado y la nueva linea seleccionada
        /// </summary>
        /// <param name="idModulo">Identificador del módulo</param>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFechaA">Fecha del intervalo correspondiente al intervalo de la mañana</param>
        /// <param name="regFechaB">Fecha del intervalo correspondiente al intervalo de la tarde</param>
        /// <param name="esLunes">Flag que indica si se debe considerar la fecB</param>
        /// <param name="tipoPatron">Parámetro que indica el tipo de obtención del perfil patrón</param>
        /// <param name="dsvPatron">Parámetro que indica el porcentaje de desviación respecto al perfil patrón</param>
        /// <param name="dataMediciones">Mediciones que conforman el perfil patrón mostrado</param>
        /// <returns></returns>
        public object DepuracionUpdatePatron(int idModulo, int idPunto, string regFechaA, string regFechaB,
            bool esLunes, string tipoPatron, decimal? dsvPatron, List<decimal[]> dataMediciones)
        {
            PrnMedicion48DTO entPrincipal = new PrnMedicion48DTO();
            PrnMedicion48DTO entSecundaria = new PrnMedicion48DTO();

            //Obtiene la demanda ejecutada
            if (idModulo == ConstantesProdem.SMDepuracionByPunto)
            {
                entPrincipal = this.GetDemandaEjecutadaPorPunto(idPunto, regFechaA).
                    FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();

                if (esLunes)
                {
                    entSecundaria = this.GetDemandaEjecutadaPorPunto(idPunto, regFechaB).
                        FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                }
            }
            if (idModulo == ConstantesProdem.SMDepuracionByAgrupacion)
            {
                entPrincipal = this.GetDemandaEjecutadaPorAgrupacion(idPunto, regFechaA).
                    FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();

                if (esLunes)
                {
                    entSecundaria = this.GetDemandaEjecutadaPorAgrupacion(idPunto, regFechaB).
                        FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                }
            }

            decimal[] arrayEntity = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, entPrincipal);

            //Llena el intervalo de la tarde si es un Lunes
            if (esLunes)
            {
                int i = ConstantesProdem.Itv30min / 2;
                decimal[] temp = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, entSecundaria);

                while (i <= ConstantesProdem.Itv30min)
                {
                    arrayEntity[i - 1] = temp[i - 1];
                    i++;
                }
            }

            //Calcula el nuevo perfil patrón
            dataMediciones.Add(arrayEntity);
            decimal[] nPatron = UtilProdem.CalcularPerfilPatron(dataMediciones, dataMediciones.Count, ConstantesProdem.Itv30min, tipoPatron);

            //Calcula los nuevos margenes de error
            decimal[] rMin = new decimal[ConstantesProdem.Itv30min];
            decimal[] rMax = new decimal[ConstantesProdem.Itv30min];
            int j = 0;
            while (j < ConstantesProdem.Itv30min)
            {
                decimal fMin = 1 - (dsvPatron * 0.01M) ?? 0;
                decimal fMax = 1 + (dsvPatron * 0.01M) ?? 0;
                rMin[j] = nPatron[j] * fMin;
                rMax[j] = nPatron[j] * fMax;
                j++;
            }

            return new { patron = nPatron, medicion = arrayEntity, emin = rMin, emax = rMax };
        }

        /// <summary>
        /// Registra el ajuste de la medición
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="idTipoDemanda">Identificador del tipo de demanda</param>
        /// <param name="regFecha">Fecha del registro (solo valida el día de la semana)</param>
        /// <param name="dataMedicion">Datos de la nueva medición defecto</param>
        /// <returns></returns>
        public object DepuracionSave(int idPunto, int idTipoDemanda, string regFecha, PrnMedicion48DTO dataMedicion)
        {
            int idTipo = 0;
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;

            //Valida el tipo de Prnm48tipo segun el tipo de demanda
            switch (idTipoDemanda)
            {
                case ConstantesProdem.LectcodiDemEjecDiario:
                    idTipo = ConstantesProdem.PrntDemandaEjecutadaAjusteManual;
                    break;
                case ConstantesProdem.LectcodiDemPrevDiario:
                    idTipo = ConstantesProdem.PrntDemandaPrevistaAjusteManual;
                    break;
                case ConstantesProdem.LectcodiDemPrevSemanal:
                    idTipo = ConstantesProdem.PrntDemandaSemanalAjusteManual;
                    break;
            }

            //Copia los valores por intervalo (H1, H2, ...)
            PrnMedicion48DTO entity = dataMedicion;

            entity.Ptomedicodi = idPunto;
            entity.Prnm48tipo = idTipo;
            entity.Medifecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            //Validación
            PrnMedicion48DTO isValid = this.GetByIdPrnMedicion48(entity.Ptomedicodi,
                entity.Prnm48tipo, entity.Medifecha);

            if (isValid.Ptomedicodi != 0)
            {
                this.UpdatePrnMedicion48(entity);
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El registro se actualizó de manera exitosa";
            }
            else
            {
                this.SavePrnMedicion48(entity);
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El registro se realizó de manera exitosa";
            }

            return new { typeMsg, dataMsg };
        }

        /// <summary>
        /// Elimina los ajustes automáticos y manuales
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="idLectura">Identificador de la tabla ME_LECTURA</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public object DepuracionEliminarAjuste(int idPunto, int idLectura, string regFecha)
        {
            bool valid = false;
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;
            DateTime parseFecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            //Validación
            if (ConstantesProdem.LectcodiDemEjecDiario.Equals(idLectura))
            {
                typeMsg = ConstantesProdem.MsgError;
                dataMsg = "Esta operación no es valida para el tipo de información Demanda diaria ejecutada";
                return new { valid, typeMsg, dataMsg };
            }

            //Elimina los ajustes
            try
            {
                switch (idLectura)
                {
                    case ConstantesProdem.LectcodiDemEjecDiario:
                        this.DeletePrnMedicion48(idPunto, ConstantesProdem.PrntDemandaEjecutadaAjusteAuto, parseFecha);
                        this.DeletePrnMedicion48(idPunto, ConstantesProdem.PrntDemandaEjecutadaAjusteManual, parseFecha);
                        break;
                    case ConstantesProdem.LectcodiDemPrevDiario:
                        this.DeletePrnMedicion48(idPunto, ConstantesProdem.PrntDemandaPrevistaAjusteAuto, parseFecha);
                        this.DeletePrnMedicion48(idPunto, ConstantesProdem.PrntDemandaPrevistaAjusteManual, parseFecha);
                        break;
                    case ConstantesProdem.LectcodiDemPrevSemanal:
                        this.DeletePrnMedicion48(idPunto, ConstantesProdem.PrntDemandaSemanalAjusteAuto, parseFecha);
                        this.DeletePrnMedicion48(idPunto, ConstantesProdem.PrntDemandaSemanalAjusteManual, parseFecha);
                        break;
                }

                valid = true;
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "Operación ejecutada correctamente, se tomará solo la información reportada para el punto " +
                    idPunto.ToString();
            }
            catch (Exception e)
            {
                typeMsg = ConstantesProdem.MsgError;
                typeMsg = "Ha ocurrido un error " + e.Message;
            }

            return new { valid, typeMsg, dataMsg };
        }

        /// <summary>
        /// Genera el reporte de puntos de medición que participan en el pronostico por barras
        /// </summary>
        /// <param name="fecDesde">Fecha inicial para el rango de busqueda</param>
        /// <param name="fecHasta">Fecha final para el rango de busqueda</param>
        /// <param name="idTipoEmpresa">Identificador del tipo de empresa</param>
        /// <returns></returns>
        public string DepuracionPorPuntosReporte(string fecDesde, string fecHasta, int idTipoEmpresa)
        {
            string reporte = "-1";
            PrnFormatoExcel book = new PrnFormatoExcel();
            PrnExcelHeader header = new PrnExcelHeader();
            List<PrnFormatoExcel> books = new List<PrnFormatoExcel>();

            List<MePtomedicionDTO> listaPtomedicion = new List<MePtomedicionDTO>();
            List<MePtomedicionDTO> listaEmpresa = new List<MePtomedicionDTO>();
            List<PrnMedicion48DTO> demandaEjecutada = new List<PrnMedicion48DTO>();
            List<PrnMedicion48DTO> demandaPrevista = new List<PrnMedicion48DTO>();
            List<PrnMedicion48DTO> demandaSemanal = new List<PrnMedicion48DTO>();

            DateTime parseDesde = DateTime.ParseExact(fecDesde,
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime parseHasta = DateTime.ParseExact(fecHasta,
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            //Lista de áreas operativas attr[nombre libro, identificador]
            List<Tuple<string, int>> listaAreas = new List<Tuple<string, int>>();
            listaAreas.Add(new Tuple<string, int>("CENTRO", ConstantesProdem.AreacodiACentro));
            listaAreas.Add(new Tuple<string, int>("NORTE", ConstantesProdem.AreacodiANorte));
            listaAreas.Add(new Tuple<string, int>("SUR", ConstantesProdem.AreacodiASur));
            listaAreas.Add(new Tuple<string, int>("S.CENTRO", ConstantesProdem.AreacodiASierraCentro));

            //Columna estatica de intervalos de tiempo
            string[] arrayIntervalos = UtilProdem
                .GenerarIntervalosFecha(ConstantesProdem.Itv30min, fecDesde, fecHasta);

            int totalFilas = arrayIntervalos.Count();

            foreach (Tuple<string, int> area in listaAreas)
            {
                book = new PrnFormatoExcel();
                book.NombreLibro = area.Item1;

                //Obtiene los puntos de medición del área
                listaPtomedicion = new List<MePtomedicionDTO>();
                listaPtomedicion = this.PR03PuntosPorBarrasCP(area.Item2.ToString());

                //Obtiene la demanda por tipo de información
                List<int> p = listaPtomedicion.Select(x => x.Ptomedicodi).ToList();
                string strP = string.Join(",", p);

                demandaEjecutada = this.GetDemandaPorTipoPorRango(ConstantesProdem.LectcodiDemEjecDiario,
                    strP, ConstantesProdem.LectcodiDemEjecDiario,
                    string.Join(",", new List<int> { ConstantesProdem.PrntDemandaEjecutadaAjusteAuto,
                        ConstantesProdem.PrntDemandaEjecutadaAjusteManual }),
                    fecDesde, fecHasta);
                demandaPrevista = this.GetDemandaPorTipoPorRango(ConstantesProdem.LectcodiDemPrevDiario,
                    strP, ConstantesProdem.LectcodiDemPrevDiario,
                    string.Join(",", new List<int> { ConstantesProdem.PrntDemandaPrevistaAjusteAuto,
                        ConstantesProdem.PrntDemandaPrevistaAjusteManual }),
                    fecDesde, fecHasta);
                demandaSemanal = this.GetDemandaPorTipoPorRango(ConstantesProdem.LectcodiDemPrevSemanal,
                    strP, ConstantesProdem.LectcodiDemPrevSemanal,
                    string.Join(",", new List<int> { ConstantesProdem.PrntDemandaSemanalAjusteAuto,
                        ConstantesProdem.PrntDemandaSemanalAjusteManual }),
                    fecDesde, fecHasta);

                //Obtiene las empresas del área
                listaEmpresa = new List<MePtomedicionDTO>();
                listaEmpresa = listaPtomedicion.GroupBy(x => x.Emprcodi)
                    .Select(y => y.First())
                    .ToList();

                //Filtra por tipo de empresa
                listaEmpresa = listaEmpresa
                    .Where(x => x.Tipoemprcodi == idTipoEmpresa)
                    .ToList();

                //Columnas estáticas de la cabecera
                book.NestedHeader1 = new List<PrnExcelHeader>();
                book.NestedHeader1.Add(new PrnExcelHeader()
                {
                    Etiqueta = "",
                    Columnas = 2
                });

                book.NestedHeader2 = new List<PrnExcelHeader>();
                book.NestedHeader2.Add(new PrnExcelHeader()
                {
                    Etiqueta = "",
                    Columnas = 2
                });

                book.NestedHeader3 = new List<PrnExcelHeader>();
                book.NestedHeader3.Add(new PrnExcelHeader()
                {
                    Etiqueta = "dd/mm/yy-hh:mm",
                    Columnas = 1
                });

                book.NestedHeader3.Add(new PrnExcelHeader()
                {
                    Etiqueta = "Total",
                    Columnas = 1
                });

                book.NestedHeader4 = new List<PrnExcelHeader>();
                book.NestedHeader4.Add(new PrnExcelHeader()
                {
                    Etiqueta = "",
                    Columnas = 2
                });

                //Contentido de la hoja excel
                List<decimal> columna = new List<decimal>();
                List<List<decimal>> matriz = new List<List<decimal>>();

                List<string> columnaColor = new List<string>();
                List<List<string>> matrizColor = new List<List<string>>();

                foreach (MePtomedicionDTO empresa in listaEmpresa)
                {
                    List<MePtomedicionDTO> ptos = listaPtomedicion
                        .Where(x => x.Emprcodi == empresa.Emprcodi)
                        .OrderBy(x => x.Ptomedicodi)
                        .ToList();

                    //Header empresa
                    book.NestedHeader1.Add(new PrnExcelHeader()
                    {
                        Etiqueta = empresa.Emprnomb,
                        Columnas = ptos.Count
                    });

                    string tipoInfo = string.Empty;
                    DateTime tempDate = new DateTime();

                    PrnMedicion48DTO ptoData = new PrnMedicion48DTO();
                    List<PrnMedicion48DTO> ptosData = new List<PrnMedicion48DTO>();

                    if (empresa.Tipoemprcodi == ConstantesProdem.TipoemprcodiUsuLibres)
                    {
                        //Usuario libre
                        foreach (MePtomedicionDTO pto in ptos)
                        {
                            tempDate = parseDesde;
                            columna = new List<decimal>();
                            columnaColor = new List<string>();

                            book.NestedHeader2.Add(new PrnExcelHeader()
                            {
                                Etiqueta = pto.Ptomedicodi.ToString(),
                                Columnas = 1
                            });

                            book.NestedHeader3.Add(new PrnExcelHeader()
                            {
                                Etiqueta = pto.Ptomedidesc,
                                Columnas = 1
                            });

                            while (tempDate <= parseHasta)
                            {
                                ptoData = new PrnMedicion48DTO();

                                //Busca la información prevista diaria
                                ptoData = demandaPrevista
                                    .FirstOrDefault(x => x.Ptomedicodi.Equals(pto.Ptomedicodi)
                                    && x.Medifecha.Equals(tempDate));

                                //Busca la información prevista semanal
                                if (ptoData == null)
                                {
                                    ptoData = new PrnMedicion48DTO();
                                    ptoData = demandaSemanal    //demandaPrevista
                                        .FirstOrDefault(x => x.Ptomedicodi.Equals(pto.Ptomedicodi)
                                        && x.Medifecha.Equals(tempDate));
                                }
                                //Obtiene los patrones defecto en ultima instancia
                                if (ptoData == null)
                                {
                                    ptoData = new PrnMedicion48DTO();
                                    ptoData = this.GetEnt48PatronDefecto(pto.Ptomedicodi, tempDate);
                                }

                                //Identifica el tipo de información de la medición
                                if (ptoData.Prnm48tipo.Equals(ConstantesProdem.LectcodiDemPrevDiario))
                                    tipoInfo = "D";
                                else if (ptoData.Prnm48tipo.Equals(ConstantesProdem.LectcodiDemPrevSemanal))
                                    tipoInfo = "S";
                                else
                                    tipoInfo = "P";

                                //Llena la fila de tipo de información solo para el primer día
                                if (tempDate.Equals(parseDesde))
                                    book.NestedHeader4.Add(new PrnExcelHeader()
                                    {
                                        Etiqueta = tipoInfo,
                                        Columnas = 1
                                    });

                                //Agrega las mediciones a la columna
                                columna.AddRange(UtilProdem.
                                    ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, ptoData));

                                //Agrega el color por fila a la columna
                                columnaColor.AddRange(Enumerable
                                    .Repeat(tipoInfo, ConstantesProdem.Itv30min)
                                    .ToList());

                                tempDate = tempDate.AddDays(1);
                            }

                            matriz.Add(columna);
                            matrizColor.Add(columnaColor);
                        }
                    }
                    else
                    {
                        //Distribuidor
                        foreach (MePtomedicionDTO pto in ptos)
                        {
                            tempDate = parseDesde;
                            columna = new List<decimal>();
                            columnaColor = new List<string>();

                            book.NestedHeader2.Add(new PrnExcelHeader()
                            {
                                Etiqueta = pto.Ptomedicodi.ToString(),
                                Columnas = 1
                            });

                            book.NestedHeader3.Add(new PrnExcelHeader()
                            {
                                Etiqueta = pto.Ptomedidesc,
                                Columnas = 1
                            });

                            while (tempDate <= parseHasta)
                            {
                                ptoData = new PrnMedicion48DTO();

                                //Busca solo información ejecutada
                                ptoData = demandaEjecutada
                                    .FirstOrDefault(x => x.Ptomedicodi.Equals(pto.Ptomedicodi)
                                    && x.Medifecha.Equals(tempDate)) ?? new PrnMedicion48DTO();

                                //Identifica el tipo de información de la medición
                                if (ptoData.Prnm48tipo.Equals(ConstantesProdem.LectcodiDemEjecDiario))
                                    tipoInfo = "E";
                                else
                                    tipoInfo = "P";

                                //Llena la fila de tipo de información solo para el primer día
                                if (tempDate.Equals(parseDesde))
                                    book.NestedHeader4.Add(new PrnExcelHeader()
                                    {
                                        Etiqueta = tipoInfo,
                                        Columnas = 1
                                    });

                                //Agrega las mediciones a la columna
                                columna.AddRange(UtilProdem.
                                    ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, ptoData));

                                //Agrega el color por fila a la columna
                                columnaColor.AddRange(Enumerable
                                    .Repeat(tipoInfo, ConstantesProdem.Itv30min)
                                    .ToList());

                                tempDate = tempDate.AddDays(1);
                            }

                            matriz.Add(columna);
                            matrizColor.Add(columnaColor);
                        }
                    }
                }

                //Llena el contenido de la hoja excel desde la matriz
                //. Columna [0] : Intervalos
                //. Columna [1] : Total
                book.Contenido = new string[matriz.Count + 2][];
                book.Contenido[0] = arrayIntervalos;
                book.Contenido[1] = new string[totalFilas];

                book.ColorByCells = new string[matrizColor.Count + 2][];
                book.ColorByCells[0] = Enumerable
                    .Repeat("X", totalFilas)
                    .ToArray();
                book.ColorByCells[1] = Enumerable
                    .Repeat("X", totalFilas)
                    .ToArray();

                //. Agrega el resto de columnas
                for (int i = 0; i < matriz.Count; i++)
                {
                    book.Contenido[i + 2] = matriz[i]
                        .ConvertAll(x => x.ToString())
                        .ToArray();
                }

                for (int i = 0; i < matrizColor.Count; i++)
                {
                    book.ColorByCells[i + 2] = matrizColor[i].ToArray();
                }

                //. Obtiene la columna total
                decimal t = 0;
                for (int i = 0; i < totalFilas; i++)
                {
                    t = 0;
                    for (int j = 0; j < matriz.Count; j++)
                    {
                        t += matriz[j][i];
                    }

                    book.Contenido[1][i] = t.ToString();
                }

                //. Titulos
                book.AnchoColumnas = Enumerable
                    .Repeat(40, matriz.Count + 2)
                    .ToArray();

                book.Titulo = "Puntos de medición "
                    + fecDesde
                    + " - "
                    + fecHasta;

                book.Subtitulo1 = area.Item1;

                //Agrega el libro a la lista
                books.Add(book);
            }

            if (books.Count() > 0)
            {
                string pathFile = ConfigurationManager.AppSettings[ConstantesProdem.ReportePronostico].ToString();
                string filename = (idTipoEmpresa
                    .Equals(ConstantesProdem.TipoemprcodiDistribuidores))
                    ? "EJECUTADO-DB-"
                    : "PREVISTO-UL-";

                filename += fecDesde.Replace("/", string.Empty) + "-";
                filename += fecHasta.Replace("/", string.Empty);

                reporte = this.ExportarReporteConLibrosyEstilosPorCelda(books, pathFile, filename);
            }

            return reporte;
        }

        #endregion

        #region Métodos del Módulo de Depuración - Reprocesar

        /// <summary>
        /// Lista los puntos de medición seleccionados
        /// </summary>
        /// <param name="regIni"></param>
        /// <param name="regByPagina"></param>
        /// <param name="listaPuntos"></param>
        /// <returns></returns>
        public object ReprocesarList(int regIni, int regByPagina, List<MePtomedicionDTO> listaPuntos)
        {
            //Obtiene los datos
            List<MePtomedicionDTO> entitys = listaPuntos;

            //Obtiene el rango de registros que se mostraran
            int totalRegistros = entitys.Count;

            //Validación: no registros
            if (totalRegistros == 0) return new { data = entitys, recordsTotal = 0, recordsFiltered = 0 };

            //Obtiene el rango de registros que se mostraran
            if (totalRegistros > regByPagina)
            {
                int tempDiff = totalRegistros - regIni;
                entitys = (tempDiff < regByPagina) ? entitys.GetRange(regIni, tempDiff) : entitys.GetRange(regIni, regByPagina);
            }

            return new { data = entitys, recordsTotal = totalRegistros, recordsFiltered = totalRegistros };
        }

        /// <summary>
        /// Reprocesa los puntos de medición seleccionados
        /// </summary>
        /// <param name="regfecha"></param>
        /// <param name="idFuente"></param>
        /// <param name="idLecturaDemanda"></param>
        /// <param name="listaPuntos"></param>
        /// <param name="nomUsuario"></param>
        /// <returns></returns>
        public object ReprocesarEjecutar(string regfecha, int idFuente, int idLecturaDemanda, List<MePtomedicionDTO> listaPuntos, string nomUsuario)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;

            //Validación
            if (listaPuntos.Count == 0)
            {
                typeMsg = ConstantesProdem.MsgWarning;
                dataMsg = "Debe seleccionar al menos un punto de medición para realizar el reprocesamiento...";
                return new { typeMsg, dataMsg };
            }

            //Inicia el proceso de recalculo
            int idLectura = -1;
            int idTipoinfo = -1;
            DateTime dFecha = DateTime.ParseExact(regfecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            //Valida la fuente de información a utilizar
            if (idFuente == 0)
            {
                //Nuevo sistema PRODEM 2 - ASSETEC
                idLectura = idLecturaDemanda;
                idTipoinfo = ConstantesProdem.TipoinfocodiMWDemanda;
            }
            if (idFuente == 1)
            {
                //Sistema actual de envío de información - COES
                switch (idLecturaDemanda)
                {
                    case ConstantesProdem.LectcodiDemEjecDiario:
                        idLectura = ConstantesProdem.AntiguoLectcodiEjecutado;
                        break;
                    case ConstantesProdem.LectcodiDemPrevDiario:
                        idLectura = ConstantesProdem.AntiguoLectcodiPrevisto;
                        break;
                    case ConstantesProdem.LectcodiDemPrevSemanal:
                        idLectura = ConstantesProdem.AntiguoLectcodiSemanal;
                        break;
                }

                idTipoinfo = ConstantesProdem.AntiguoTipoinfocodi;
            }

            foreach (MePtomedicionDTO a in listaPuntos)
            {
                //Obtiene la información reportada por el agente desde extranet
                MeMedicion48DTO dataMedicion = this.GetByIdMeMedicion48(idLectura, dFecha, idTipoinfo, a.Ptomedicodi);

                //Validación
                decimal[] arrayMedicion = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, dataMedicion);
                if (arrayMedicion.Sum() == 0) continue;

                //Obtiene la configuración y el perfíl patrón
                PrnConfiguracionDTO dataConfiguracion = this.ParametrosGetConfiguracion(a.Ptomedicodi, ConstantesProdem.DefectoByPunto, dFecha);
                PrnPatronModel dataPatron = this.GetPatron(a.Ptomedicodi, ConstantesProdem.ProcPatronDemandaEjecutada, dFecha, dataConfiguracion);

                //Procesa la información
                //Setea el lectcodi correspondiente al nuevo sistema para los registros sin importar la fuente
                dataMedicion.Lectcodi = idLecturaDemanda;
                this.ProcesarDemandaReportada(a.Ptomedicodi, dFecha, dataMedicion, dataConfiguracion, dataPatron, false, nomUsuario);
            }

            typeMsg = ConstantesProdem.MsgSuccess;
            dataMsg = "Se reprocesaron correctamente los puntos seleccionados!";

            return new { typeMsg, dataMsg };
        }

        #endregion

        #region Metodos para modulo Relacion Areas

        public List<object> GetAreaOperativaByNivel(int anivelcodi)
        {
            EqAreaDTO area = new EqAreaDTO();
            List<EqAreaDTO> listAreas = new List<EqAreaDTO>();
            List<object> tabla = new List<object>();

            if (anivelcodi == ConstantesProdem.NvlSubestacion)
            {
                listAreas = FactorySic.GetPrnPronosticoDemandaRepository().GetSubestacionCentralByNivel(anivelcodi);

                object row = new object();
                tabla = new List<object>();
                foreach (var a in listAreas)
                {
                    row = new
                    {
                        areacodi = a.Areacodi,
                        areaabrev = a.Areaabrev,
                        areanomb = a.Tareaabrev + " " + a.Areanomb,
                        areapadre = a.Areapadre,
                        anivelcodi = a.ANivelCodi
                    };
                    tabla.Add(row);
                }
            }

            if (anivelcodi == ConstantesProdem.NvlAreaOperativa)
            {
                listAreas = FactorySic.GetPrnPronosticoDemandaRepository().GetAreaOperativaByNivel(anivelcodi);
                object row = new object();
                tabla = new List<object>();
                foreach (var a in listAreas)
                {
                    row = new
                    {
                        areacodi = a.Areacodi,
                        areaabrev = a.Areaabrev,
                        areanomb = a.Areanomb,
                        areapadre = a.Areapadre,
                        anivelcodi = a.ANivelCodi
                    };
                    tabla.Add(row);
                }
            }
            return tabla;
        }

        public List<EqAreaDTO> GetSubEstacionSeleccionadas(int area, int nivel)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().GetSubEstacionSeleccionadas(area, nivel);
        }

        public List<EqAreaDTO> GetSubEstacionDisponibles(int areanivel, int areapadre, int relpadre, int relnivel, int arearelnivel)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().GetSubEstacionDisponibles(areanivel, areapadre, relpadre, relnivel, arearelnivel);
        }

        public void DeleteEqAreaRel(int areacodi, int areapadre)
        {
            FactorySic.GetPrnPronosticoDemandaRepository().DeleteRelacion(areacodi, areapadre);
        }

        public void DeleteEqAreaRelByPadre(int areapadre)
        {
            FactorySic.GetPrnPronosticoDemandaRepository().DeleteByPadre(areapadre);
        }

        public EqAreaDTO GetAreaOperativaBySubestacion(int areacodi)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().GetAreaOperativaBySubestacion(areacodi);
        }

        public EqAreaRelDTO GetSubestacionRel(int areacodi)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().GetSubestacionRel(areacodi);
        }

        public List<PrGrupoDTO> GetBarrasSeleccionadas(int areacodi)
        {

            return FactorySic.GetPrnPronosticoDemandaRepository().GetBarrasSeleccionadas(areacodi);
        }

        public List<PrGrupoDTO> GetBarrasDisponibles(int areacodi)
        {

            return FactorySic.GetPrnPronosticoDemandaRepository().GetBarrasDisponibles(areacodi);
        }

        public void UpdatePrGrupo(int grupo, int subestacion)
        {

            FactorySic.GetPrnPronosticoDemandaRepository().UpdatePrGrupo(grupo, subestacion);
        }

        public List<PrGrupoDTO> GetListBarras()
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().GetListBarras();
        }
        #endregion

        #region Métodos del Módulo de Relación de Barras

        /// <summary>
        /// Lista todas las Barras PM con sus relaciones de Área operativa y SS.EE
        /// </summary>
        /// <returns></returns>
        //public List<PrGrupoDTO> ListFiltroBarrasPM()
        //{
        //    return FactorySic.GetPrnPronosticoDemandaRepository().
        //        ListRelacionBarrasPM(ConstantesProdem.NvlSubestacion,
        //        ConstantesProdem.NvlAreaOperativa, ConstantesProdem.Prcatecodi, "0", "0");
        //}

        /// <summary>
        /// Lista los puntos de medición o agrupaciones relacionadas a una Barra PM
        /// </summary>
        /// <param name="idBarra">Identificador de la tabla PR_GRUPO (uno o varios)</param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListRelacionPtosPorBarraPM(string idBarra)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().ListRelacionPtosPorBarraPM(idBarra);
        }

        /// <summary>
        /// Lista de Barras
        /// </summary>
        /// <param name="nvlSubestacion"></param>
        /// <param name="nvlAreaOperativa"></param>
        /// <param name="idCategoria"></param>
        /// <param name="idArea"></param>
        /// <param name="idBarra"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> ListRelacionBarrasPM(int nvlSubestacion, int nvlAreaOperativa, int idCategoria, string idArea, string idBarra)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().ListRelacionBarrasPM(nvlSubestacion, nvlAreaOperativa, idCategoria, idArea, idBarra);
        }

        /// <summary>
        /// Lista la tabla principal del módulo (paginado)
        /// </summary>
        /// <param name="regIni"></param>
        /// <param name="regByPagina"></param>
        /// <param name="idArea">Identificador de la tabla EQ_AREA corresponde a las Áreas operativas (uno o varios)</param>
        /// <param name="idBarra">Identificador de la tabla PR_GRUPO corresponde a una barra PM (uno o varios)</param>
        /// <returns></returns>
        public object RelacionBarraList(int regIni, int regByPagina, string idArea, string idBarra, string idEmpresa)
        {
            List<object> tabla = new List<object>();

            //Validacion para el filtro de empresa

            List<PrGrupoDTO> puntos = new List<PrGrupoDTO>();
            List<PrGrupoDTO> dataTabla = new List<PrGrupoDTO>();
            List<PrGrupoDTO> dataTablaB = new List<PrGrupoDTO>();

            if (!idEmpresa.Equals("0"))
            {
                puntos = this.ListPuntosByEmpresa(idEmpresa);
                dataTablaB = this.ListRelacionBarrasPM(ConstantesProdem.NvlSubestacion,
                    ConstantesProdem.NvlAreaOperativa, ConstantesProdem.Prcatecodi, idArea, idBarra);

                foreach (var x in dataTablaB)
                {
                    foreach (var y in puntos)
                    {
                        if (x.Grupocodi == y.Grupocodi)
                        {
                            dataTabla.Add(x);
                        }
                    }
                }
            }
            else
            {
                //Obtiene los datos principales de la tabla
                dataTabla = this.ListRelacionBarrasPM(ConstantesProdem.NvlSubestacion,
                    ConstantesProdem.NvlAreaOperativa, ConstantesProdem.Prcatecodi, idArea, idBarra);

            }
            //Validación: no registros
            int totalRegistros = dataTabla.Count;
            if (totalRegistros == 0) return new { data = tabla, recordsTotal = 0, recordsFiltered = 0 };

            ////Obtiene el rango de registros que se mostraran
            if (totalRegistros > regByPagina)
            {
                int tempDiff = totalRegistros - regIni;
                dataTabla = (tempDiff < regByPagina) ? dataTabla.GetRange(regIni, tempDiff) : dataTabla.GetRange(regIni, regByPagina);
            }

            string listBarras = UtilProdem.ConvertirEntityListEnString(dataTabla, "Grupocodi");

            //Obtiene los datos de las relaciones entre barra y punto de medición
            List<MePtomedicionDTO> dataRel = this.ListRelacionPtosPorBarraPM(listBarras);

            //Da formato a la tabla 
            List<string> listStr = new List<string>();
            List<MePtomedicionDTO> puntosRel = new List<MePtomedicionDTO>();
            string prefijo = "P";
            foreach (PrGrupoDTO a in dataTabla)
            {
                listStr = new List<string>();
                puntosRel = new List<MePtomedicionDTO>();
                puntosRel = dataRel.Where(x => x.Grupocodibarra == a.Grupocodi).ToList();
                int flagVal = 0;

                foreach (MePtomedicionDTO p in puntosRel)
                {
                    if (p.Origlectcodi != 6)
                    {
                        prefijo = "A";
                    }
                    else
                    {
                        prefijo = "P";
                    }

                    if (string.IsNullOrEmpty(p.Ptomedibarranomb))
                    {
                        listStr.Add(prefijo + p.Ptomedidesc + "  (" + p.Ptomedicodi + ")");
                    }
                    else
                    {
                        listStr.Add(prefijo + p.Ptomedibarranomb + "  (" + p.Ptomedicodi + ")");
                    }

                }

                List<PrGrupoDTO> barrasPM = new List<PrGrupoDTO>();
                PrGrupoDTO barrasCP = new PrGrupoDTO();
                string nombre = "";
                barrasPM = this.ListBarrasPMNombre(a.Grupocodi, ConstantesProdem.Prcatecodi);
                barrasCP = this.ListBarrasCPNombre(a.Grupocodi, ConstantesProdem.Prcatecodi);

                nombre = "No asignado";
                if (barrasPM.Count() > 0)
                {
                    int c = barrasPM.Count();
                    nombre = "";
                    foreach (var item in barrasPM)
                    {
                        c--;
                        nombre += item.Gruponomb;
                        if (c > 0)
                        {
                            nombre += "/";
                        }
                    }

                }
                if (!string.IsNullOrEmpty(barrasCP.Gruponomb))
                {
                    nombre = barrasCP.Gruponomb;
                }


                object r = new
                {
                    id = a.Grupocodi,
                    area = (a.Areapadre != 0) ? a.Areapadrenomb : "No asignado",
                    pm = a.Gruponomb,
                    cp = nombre,
                    rel = listStr
                };

                tabla.Add(r);

            }

            return new { data = tabla, recordsTotal = totalRegistros, recordsFiltered = totalRegistros };
        }

        public List<PrGrupoDTO> ListBarrasPMNombre(int grupocodi, int catecodi)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().
                ListBarrasPMNombre(grupocodi, catecodi);
        }

        public PrGrupoDTO ListBarrasCPNombre(int grupocodi, int catecodi)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().
                ListBarrasCPNombre(grupocodi, catecodi);
        }

        /// <summary>
        /// Trae la lista para llenar la tabla de agrupaciones
        /// </summary>
        /// <returns></returns>
        public List<object> AgrupacionesDtList()
        {

            List<object> data = new List<object>();
            List<object> datapto = new List<object>();
            List<object> datacodi = new List<object>();

            List<PrnAgrupacionDTO> agrupacionList = new List<PrnAgrupacionDTO>();
            List<PrnAgrupacionDTO> agrupacionPunto = new List<PrnAgrupacionDTO>();
            List<int> agrupacion = new List<int>();

            agrupacionList = FactorySic.GetPrnPronosticoDemandaRepository().
                AgrupacionesPuntosList(ConstantesProdem.OriglectcodiAgrupacion, ConstantesProdem.BarraNoDefinido);
            agrupacion = agrupacionList.Select(x => x.Ptomedicodi).Distinct().ToList();

            object entity;
            object pt;

            foreach (var item in agrupacion)
            {
                agrupacionPunto = agrupacionList.Where(x => x.Ptomedicodi == item).ToList();
                datapto = new List<object>();
                List<int> codigos = new List<int>();

                foreach (var x in agrupacionPunto)
                {
                    pt = new
                    {
                        id = x.Ptogrphijocodi,
                        nombre = x.Ptogrphijodesc
                    };
                    datapto.Add(pt);
                    codigos.Add(x.Ptogrphijocodi);
                }
                string codigoList = (codigos.Count != 0) ? string.Join(",", codigos) : "0";

                entity = new
                {
                    id = item,//agrupacionPunto.Select(x => x.Ptomedicodi).Distinct(),
                    nombre = agrupacionPunto.Select(x => x.Ptomedidesc).Distinct().First() + "(" + codigoList + ")",
                    puntos = datapto,
                    ubicacion = "-",
                    empresa = agrupacionPunto.Select(x => x.Emprnomb).Distinct(),
                    barracodi = agrupacionPunto.Select(x => x.Grupocodibarra).Distinct()
                };

                data.Add(entity);

            }

            return data;
        }

        /// <summary>
        /// Trae la lista para llenar la tabla de puntos
        /// </summary>
        /// <returns></returns>
        public List<object> PuntosDtList()
        {
            List<object> data = new List<object>();
            List<MePtomedicionDTO> dataPuntos = new List<MePtomedicionDTO>();

            dataPuntos = FactorySic.GetPrnMedicion48Repository().
                PR03PuntosNoAgrupados().Where(x => x.Grupocodibarra == -1).ToList();

            object entity;

            foreach (var item in dataPuntos)
            {
                entity = new
                {
                    id = item.Ptomedicodi,
                    nombre = item.Ptomedidesc,
                    ubicacion = item.Areanomb,
                    empresa = item.Emprnomb,
                    barracodi = item.Grupocodibarra
                };

                data.Add(entity);

            }

            return data;
        }

        public List<object> AgrupacionesPuntosDtList(int barra)
        {
            List<object> data = new List<object>();
            List<object> datapto = new List<object>();

            //DT seleccionados para las agrupaciones
            List<PrnAgrupacionDTO> agrupacionList = new List<PrnAgrupacionDTO>();
            List<PrnAgrupacionDTO> agrupacionPunto = new List<PrnAgrupacionDTO>();

            //DT seleccionados para los puntos
            List<MePtomedicionDTO> puntosList = new List<MePtomedicionDTO>();

            List<int> agrupacion = new List<int>();

            //Lista de Agrupaciones
            agrupacionList = FactorySic.GetPrnPtomedpropRepository().
                PR03Agrupaciones(ConstantesProdem.OriglectcodiAgrupacion, barra);
            agrupacion = agrupacionList.Select(x => x.Ptomedicodi).Distinct().ToList();

            //Lista de Puntos
            puntosList = FactorySic.GetPrnPtomedpropRepository().
                PR03Puntos().Where(x => x.Grupocodibarra == barra).ToList();

            object entity;
            object pt;

            //LLenando el objeto de agrupaciones
            foreach (var item in agrupacion)
            {
                agrupacionPunto = agrupacionList.Where(x => x.Ptomedicodi == item).ToList();

                datapto = new List<object>();

                foreach (var x in agrupacionPunto)
                {
                    pt = new
                    {
                        id = x.Ptogrphijocodi,
                        nombre = x.Ptogrphijodesc
                    };
                    datapto.Add(pt);
                }

                entity = new
                {
                    id = item,//agrupacionPunto.Select(x => x.Ptomedicodi).Distinct(),
                    nombre = agrupacionPunto.Select(x => x.Ptomedidesc).Distinct(),
                    puntos = datapto,
                    ubicacion = "-",
                    empresa = "-",
                    barracodi = agrupacionPunto.Select(x => x.Grupocodibarra).Distinct(),
                    proceso = agrupacionPunto.Select(x => x.Prnpmpvarexoproceso).Distinct(),
                    source = "agrupacion"
                };

                data.Add(entity);

            }

            //LLenando el objeto de puntos
            foreach (var item in puntosList)
            {
                entity = new
                {
                    id = item.Ptomedicodi,
                    nombre = item.Ptomedidesc,
                    ubicacion = item.Areanomb,
                    empresa = item.Emprnomb,
                    barracodi = item.Grupocodibarra,
                    proceso = item.Prnpmpvarexoproceso,
                    source = "punto"
                };

                data.Add(entity);
            }

            return data;
        }

        /// <summary>
        /// Lista de agrupaciones
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="barra"></param>
        /// <returns></returns>
        public List<PrnAgrupacionDTO> AgrupacionesList(int origen, int barra)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().AgrupacionesList(origen, barra);
        }

        public List<MePtomedicionDTO> ListPuntosNoAgrupaciones(int orgagrupacion, int barra, int orgpunto)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().ListPuntosNoAgrupaciones(orgagrupacion, barra, orgpunto);
        }

        public List<MePtomedicionDTO> PuntosBarraList(int grupobarra)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().ListPuntosBarra(grupobarra);
        }

        public void UpdateMeMedicionBarra(int punto, int barra, string user)
        {
            try
            {
                FactorySic.GetPrnPronosticoDemandaRepository().UpdateMeMedicionBarra(punto, barra, user);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Lista Empresa para la relacion barras
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListEmpresaBarrasRel()
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().ListEmpresaBarrasRel();
        }

        /// <summary>
        /// Lista los Puntos y Agrupaciones por Empresa
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> ListPuntosByEmpresa(string empresa)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().ListPuntosByEmpresa(empresa);
        }

        /// <summary>
        /// Refresca la lista de empresas por barra 
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListEmpresaByBarra(string barra)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().ListEmpresaByBarra(barra);
        }

        /// <summary>
        /// Lista de Barras registradas en PtoMedicion
        /// </summary>
        /// <param name="nvlSubestacion"></param>
        /// <param name="nvlAreaOperativa"></param>
        /// <param name="idCategoria"></param>
        /// <param name="idArea"></param>
        /// <param name="idBarra"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> ListBarrasInPtoMedicion(int nvlSubestacion, int nvlAreaOperativa, int idCategoria, string idArea, string idBarra)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().ListBarrasInPtoMedicion(nvlSubestacion, nvlAreaOperativa, idCategoria, idArea, idBarra);
        }
        #endregion

        #region Métodos del Módulo de Despacho Ejecutado

        /// <summary>
        /// Permite listar obtener el area operativa por equipo
        /// </summary>
        /// <param name="equicodi">Identificador de la tabla eq_equipo</param>
        /// <returns></returns>
        public EqAreaDTO GetAreaOperativaByEquipo(int equicodi)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().GetAreaOperativaByEquipo(equicodi);
        }

        /// <summary>
        /// Método que permite listar los puntos no relacionados de una central
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <param name="puntosFaltantes"></param>
        /// <returns></returns>
        public void ObtenerPuntosFaltantes(int? idEmpresa, int? idCentral, out List<int> puntosFaltantes)
        {
            if (idEmpresa == null) idEmpresa = -1;
            if (idCentral == null) idCentral = -1;
            puntosFaltantes = FactorySic.GetPrnMedicioneqRepository().ObtenerPuntosFaltantes((int)idCentral, (int)idEmpresa);
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Despacho Ejecutado"
        /// </summary>
        /// <param name="Areacodi">Identificador del área operativa/param>
        /// <param name="fecha">Fecha en que se realiza la consulta. Formato dd/MM/yyyy</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoPrnMedicionEq(string pathFile, string pathLogo, DateTime fechaConsulta, List<PrnMedicioneqDTO> listaMedicion, List<PrnMedicioneqDTO> listaRpf, List<PrnMedicioneqDTO> listaDespacho)
        {
            string fileName = string.Empty;

            fileName = "ReporteDespachoEjecutado.xlsx";
            ExcelDocument.GenerarReportePrnMedicionEq(pathFile + fileName, fechaConsulta, listaMedicion, listaRpf, listaDespacho);
            return fileName;
        }

        #endregion

        #region Métodos del Módulo de Pronóstico - Demanda ejecutada por Áreas

        /// <summary>
        /// General el modelo de datos del módulo
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public object DemandaDatos(int idArea, string regFecha)
        {
            List<object> data = new List<object>();
            List<int> listaIntervalos = new List<int>();
            DateTime parseFecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            //Obtiene los parámetros configurados
            PrnConfiguracionDTO regConfiguracion = this.ParametrosGetConfiguracion(idArea, ConstantesProdem.DefectoByArea, parseFecha);
            PrnPatronModel regPatron = this.GetPatronDemandaArea(idArea, parseFecha);

            switch (idArea)
            {
                case ConstantesProdem.PtomedicodiASein:
                    {
                        #region Demanda del área total (SEIN)

                        //Obtiene los datos del Sur
                        List<PrnMedicion48DTO> dataSur = this.GetDemandaPorArea(ConstantesProdem.PtomedicodiASur, 1, regFecha);
                        PrnMedicion48DTO sSur = dataSur.FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        decimal[] dSur = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sSur);

                        //Obtiene los datos del Norte
                        List<PrnMedicion48DTO> dataNorte = this.GetDemandaPorArea(ConstantesProdem.PtomedicodiANorte, 1, regFecha);
                        PrnMedicion48DTO sNorte = dataNorte.FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        decimal[] dNorte = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sNorte);

                        //Obtiene los datos de la Sierra Centro
                        List<PrnMedicion48DTO> dataSCentro = this.GetDemandaPorArea(ConstantesProdem.PtomedicodiASierraCentro, 1, regFecha);
                        PrnMedicion48DTO sSCentro = dataSCentro.FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        decimal[] dSCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sSCentro);

                        //Obtiene los datos del Centro
                        List<PrnMedicion48DTO> dataCentro = this.GetDemandaPorArea(ConstantesProdem.PtomedicodiACentro, 1, regFecha);
                        PrnMedicion48DTO sCentro = dataCentro.FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        decimal[] dCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sCentro);

                        //Obtiene los datos del Sein
                        decimal[] dSein = new decimal[ConstantesProdem.Itv30min];
                        for (int i = 0; i < ConstantesProdem.Itv30min; i++)
                        {
                            dSein[i] = dSur[i] + dNorte[i] + dSCentro[i] + dCentro[i];
                        }

                        object entity = new object();

                        entity = new
                        {
                            id = "intervalos",
                            label = "Hora",
                            data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                            htrender = "hora",
                            hcrender = "categoria"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "sur",
                            label = "Sur(S)",
                            data = dSur,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "norte",
                            label = "Norte(N)",
                            data = dNorte,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "scentro",
                            label = "SCentro(SC)",
                            data = dSCentro,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "centro",
                            label = "Centro(C)",
                            data = dCentro,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "sein",
                            label = "<label title='S + N + SC + C'>Sein</label>",
                            data = dSein,
                            htrender = "final",
                            hcrender = "final_noedit"
                        };
                        data.Add(entity);

                        //Mediciones historicas
                        for (int i = 0; i < regPatron.Mediciones.Count; i++)
                        {
                            entity = new
                            {
                                id = "med" + (i + 1).ToString(),
                                label = regPatron.StrFechas[i],
                                data = regPatron.Mediciones[i],
                                htrender = "no",
                                hcrender = "no",
                                label2 = regPatron.StrFechasTarde[i],
                                slunes = regPatron.EsLunes
                            };
                            data.Add(entity);
                        }

                        #endregion
                    }
                    break;
                case ConstantesProdem.PtomedicodiACentro:
                    {
                        #region Demanda del área Centro
                        List<PrnMedicion48DTO> dataDemanda = this.GetDemandaPorArea(idArea, 1, regFecha);

                        //Demanda total
                        PrnMedicion48DTO sFinal = dataDemanda.FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        decimal[] dFinal = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sFinal);

                        //Despacho ejecutado
                        PrnMedicion48DTO sDespacho = dataDemanda.FirstOrDefault(x => x.Prnm48tipo == 1) ?? new PrnMedicion48DTO();
                        decimal[] dDespacho = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sDespacho);

                        //Ajuste
                        PrnMedicion48DTO sAjuste = dataDemanda.FirstOrDefault(
                            x => x.Prnm48tipo == ConstantesProdem.PrntDemandaAreaAjuste &&
                            x.Ptomedicodi == ConstantesProdem.PtomedicodiACentro) ?? new PrnMedicion48DTO();
                        decimal[] dAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sAjuste);

                        //Mantenimiento
                        int[] constMant = new int[] {
                            ConstantesProdem.PrntMantCentro, ConstantesProdem.PrntMantCentroPrevisto
                        };

                        PrnMedicion48DTO sMantenimiento = dataDemanda.FirstOrDefault(x => constMant.Contains(x.Prnm48tipo)) ?? new PrnMedicion48DTO();
                        decimal[] dMantenimiento = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sMantenimiento);

                        //Falla
                        int[] constFalla = new int[] {
                            ConstantesProdem.PrntFallaCentro, ConstantesProdem.PrntFallaCentroPrevisto
                        };

                        PrnMedicion48DTO sFalla = dataDemanda.FirstOrDefault(x => constFalla.Contains(x.Prnm48tipo)) ?? new PrnMedicion48DTO();
                        decimal[] dFalla = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sFalla);

                        //Flujo
                        PrnMedicion48DTO sFlujo = dataDemanda.FirstOrDefault(x => x.Prnm48tipo == ConstantesProdem.PrntFlujoLinea) ?? new PrnMedicion48DTO();
                        decimal[] dFlujo = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sFlujo);

                        //Ajuste Sur
                        PrnMedicion48DTO sSur = dataDemanda.FirstOrDefault(x => x.Ptomedicodi == ConstantesProdem.PtomedicodiASur) ?? new PrnMedicion48DTO();
                        decimal[] dSur = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sSur);

                        //Ajuste Norte
                        PrnMedicion48DTO SNorte = dataDemanda.FirstOrDefault(x => x.Ptomedicodi == ConstantesProdem.PtomedicodiANorte) ?? new PrnMedicion48DTO();
                        decimal[] dNorte = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, SNorte);

                        //Ajuste Sierra Centro
                        PrnMedicion48DTO sSCentro = dataDemanda.FirstOrDefault(x => x.Ptomedicodi == ConstantesProdem.PtomedicodiASierraCentro) ?? new PrnMedicion48DTO();
                        decimal[] dSCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sSCentro);

                        object entity = new object();

                        entity = new
                        {
                            id = "intervalos",
                            label = "Hora",
                            data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                            htrender = "hora",
                            hcrender = "categoria"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "despacho",
                            label = "Despacho(D)",
                            data = dDespacho,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "mantenimiento",
                            label = "Mantenimiento(M)",
                            data = dMantenimiento,
                            htrender = "normal",
                            hcrender = "no"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "falla",
                            label = "Falla(F)",
                            data = dFalla,
                            htrender = "normal",
                            hcrender = "no"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "flujo",
                            label = "Flujos(FL)",
                            data = dFlujo,
                            htrender = "normal",
                            hcrender = "no"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "sur",
                            label = "Sur(aS)",
                            data = dSur,
                            htrender = "normal",
                            hcrender = "no"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "norte",
                            label = "Norte(aN)",
                            data = dNorte,
                            htrender = "normal",
                            hcrender = "no"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "scentro",
                            label = "SCentro(aSC)",
                            data = dSCentro,
                            htrender = "normal",
                            hcrender = "no"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "ajuste",
                            label = "Ajuste",
                            data = dAjuste,
                            htrender = "edit",
                            hcrender = "no"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "demanda",
                            label = "<label title='D + M + F + FL + aS + aN + aSC'>Demanda</label>",
                            data = dFinal,
                            htrender = "final",
                            hcrender = "final"
                        };
                        data.Add(entity);

                        //Mediciones historicas
                        for (int i = 0; i < regPatron.Mediciones.Count; i++)
                        {
                            entity = new
                            {
                                id = "med" + (i + 1).ToString(),
                                label = regPatron.StrFechas[i],
                                data = regPatron.Mediciones[i],
                                htrender = "no",
                                hcrender = "normal",
                                label2 = regPatron.StrFechasTarde[i],
                                slunes = regPatron.EsLunes
                            };
                            data.Add(entity);
                        }

                        entity = new
                        {
                            id = "patron",
                            label = "Patrón",
                            data = regPatron.Patron,
                            htrender = "no",
                            hcrender = "patron"
                        };
                        data.Add(entity);
                        #endregion
                    }
                    break;
                default:
                    {
                        #region Demanda de las áreas Sur, Norte y Sierra centro

                        List<PrnMedicion48DTO> dataDemanda = this.GetDemandaPorArea(idArea, 1, regFecha);

                        //Demanda total
                        PrnMedicion48DTO sFinal = dataDemanda.FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        decimal[] dFinal = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sFinal);

                        //Despacho ejecutado
                        PrnMedicion48DTO sDespacho = dataDemanda.FirstOrDefault(x => x.Prnm48tipo == 1) ?? new PrnMedicion48DTO();
                        decimal[] dDespacho = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sDespacho);

                        //Ajuste
                        PrnMedicion48DTO sAjuste = dataDemanda.FirstOrDefault(x => x.Prnm48tipo == ConstantesProdem.PrntDemandaAreaAjuste) ?? new PrnMedicion48DTO();
                        decimal[] dAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sAjuste);

                        //Mantenimiento
                        int[] constMant = new int[] {
                            ConstantesProdem.PrntMantSur, ConstantesProdem.PrntMantSurPrevisto,
                            ConstantesProdem.PrntMantNorte, ConstantesProdem.PrntMantNortePrevisto,
                            ConstantesProdem.PrntMantSierraCentro, ConstantesProdem.PrntMantSierraCentroPrevisto,
                            ConstantesProdem.PrntMantCentro, ConstantesProdem.PrntMantCentroPrevisto
                        };

                        PrnMedicion48DTO sMantenimiento = dataDemanda.FirstOrDefault(x => constMant.Contains(x.Prnm48tipo)) ?? new PrnMedicion48DTO();
                        decimal[] dMantenimiento = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sMantenimiento);

                        //Falla
                        int[] constFalla = new int[] {
                            ConstantesProdem.PrntFallaSur, ConstantesProdem.PrntFallaSurPrevisto,
                            ConstantesProdem.PrntFallaNorte, ConstantesProdem.PrntFallaNortePrevisto,
                            ConstantesProdem.PrntFallaSierraCentro, ConstantesProdem.PrntFallaSierracentroPrevisto,
                            ConstantesProdem.PrntFallaCentro, ConstantesProdem.PrntFallaCentroPrevisto
                        };

                        PrnMedicion48DTO sFalla = dataDemanda.FirstOrDefault(x => constFalla.Contains(x.Prnm48tipo)) ?? new PrnMedicion48DTO();
                        decimal[] dFalla = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sFalla);

                        //Flujo
                        PrnMedicion48DTO sFlujo = dataDemanda.
                            FirstOrDefault(x => x.Prnm48tipo == ConstantesProdem.PrntFlujoLinea) ?? new PrnMedicion48DTO() { Ptomedidesc = "No encontrado" };
                        decimal[] dFlujo = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sFlujo);
                        string flujoTitle = "<label title='" + sFlujo.Ptomedidesc + "'>Flujos(FL)</label>";

                        //Obtiene los intervalos corregidos del flujo
                        listaIntervalos = sFlujo.ListaIntervalos ?? new List<int>();

                        object entity = new object();

                        entity = new
                        {
                            id = "intervalos",
                            label = "Hora",
                            data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                            htrender = "hora",
                            hcrender = "categoria"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "despacho",
                            label = "Despacho(D)",
                            data = dDespacho,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "mantenimiento",
                            label = "Mantenimiento(M)",
                            data = dMantenimiento,
                            htrender = "normal",
                            hcrender = "no"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "falla",
                            label = "Falla(F)",
                            data = dFalla,
                            htrender = "normal",
                            hcrender = "no"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "flujo",
                            label = flujoTitle,
                            data = dFlujo,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "ajuste",
                            label = "Ajuste",
                            data = dAjuste,
                            htrender = "edit",
                            hcrender = "no"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "demanda",
                            label = "<label title='D + M + F + FL'>Demanda</label>",
                            data = dFinal,
                            htrender = "final",
                            hcrender = "final"
                        };
                        data.Add(entity);

                        //Mediciones historicas
                        for (int i = 0; i < regPatron.Mediciones.Count; i++)
                        {
                            entity = new
                            {
                                id = "med" + (i + 1).ToString(),
                                label = regPatron.StrFechas[i],
                                data = regPatron.Mediciones[i],
                                htrender = "no",
                                hcrender = "normal",
                                label2 = regPatron.StrFechasTarde[i],
                                slunes = regPatron.EsLunes
                            };
                            data.Add(entity);
                        }

                        entity = new
                        {
                            id = "patron",
                            label = "Patrón",
                            data = regPatron.Patron,
                            htrender = "no",
                            hcrender = "patron"
                        };
                        data.Add(entity);
                        #endregion
                    }
                    break;
            }

            return new { data = data, cfg = regConfiguracion, itv = listaIntervalos };
        }

        /// <summary>
        /// Registra el ajuste de la medición
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="dataMedicion">Datos del ajuste realizado al área</param>
        /// <param name="nomUsuario">Nombre del usuario</param>
        /// <returns></returns>
        public object DemandaSave(int idPunto, string regFecha, PrnMedicion48DTO dataMedicion, string nomUsuario)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;

            //Copia los valores por intervalo (H1, H2, ...)
            PrnMedicion48DTO entity = dataMedicion;

            entity.Ptomedicodi = idPunto;
            entity.Prnm48tipo = ConstantesProdem.PrntDemandaAreaAjuste;
            entity.Medifecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            //Validación
            PrnMedicion48DTO isValid = this.GetByIdPrnMedicion48(entity.Ptomedicodi,
                entity.Prnm48tipo, entity.Medifecha);

            if (isValid.Ptomedicodi != 0)
            {
                entity.Prnm48usumodificacion = nomUsuario;
                entity.Prnm48fecmodificacion = DateTime.Now;

                this.UpdatePrnMedicion48(entity);
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El registro se actualizó de manera exitosa";
            }
            else
            {
                entity.Prnm48usucreacion = nomUsuario;
                entity.Prnm48feccreacion = DateTime.Now;
                entity.Prnm48usumodificacion = nomUsuario;
                entity.Prnm48fecmodificacion = DateTime.Now;

                this.SavePrnMedicion48(entity);
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El registro se realizó de manera exitosa";
            }

            return new { typeMsg, dataMsg };
        }

        /// <summary>
        /// Obtiene los datos respecto a los flujos de las áreas operativas
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <returns></returns>
        public object DemandaFlujos(int idArea)
        {
            object entity = new object();
            List<PrnFormularelDTO> listRelacionados = this.ListFormulasRelacionadas(idArea);
            List<PrnFormularelDTO> listDisponibles = this.FormulasRestantesByLista(listRelacionados);

            List<object> objRelacionados = new List<object>();
            foreach (PrnFormularelDTO a in listRelacionados)
            {
                entity = new
                {
                    id = a.Ptomedicodicalc,
                    nombre = a.Ptomedidesc,
                    prioridad = a.Prfrelfactor
                };
                objRelacionados.Add(entity);

            }

            List<object> objDisponibles = new List<object>();
            foreach (PrnFormularelDTO a in listDisponibles)
            {
                entity = new
                {
                    id = a.Ptomedicodicalc,
                    nombre = a.Ptomedidesc,
                    prioridad = a.Prfrelfactor
                };
                objDisponibles.Add(entity);
            }

            return new { relacionados = objRelacionados, disponibles = objDisponibles };
        }

        /// <summary>
        /// Registra la nueva relación
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="listFlujos">Lista de flujos relacionados (objeto {id:x, prioridad:x})</param>
        /// <param name="nomUsuario">Nombre del usuario</param>
        /// <returns></returns>
        public object DemandaFlujosSave(int idArea, List<PrnFormularelDTO> listFlujos, string nomUsuario)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;

            //Elimina los registros previamente relacionados
            this.DeleteByPtoPrnFormularel(idArea);

            //Registra las nuevas relaciones
            foreach (PrnFormularelDTO entity in listFlujos)
            {
                entity.Ptomedicodi = idArea;
                entity.Prfrelusucreacion = nomUsuario;
                entity.Prfrelusumodificacion = nomUsuario;
                entity.Prfrelfeccreacion = DateTime.Now;
                entity.Prfrelfecmodificacion = DateTime.Now;

                this.SavePrnFormularel(entity);
            }

            typeMsg = ConstantesProdem.MsgSuccess;
            dataMsg = "El registro se realizó de manera exitosa";

            return new { typeMsg, dataMsg };
        }

        /// <summary>
        /// Devuelve el perfil patrón actualizado y la nueva linea seleccionada
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFechaA">Fecha del intervalo correspondiente al intervalo de la mañana</param>
        /// <param name="regFechaB">Fecha del intervalo correspondiente al intervalo de la tarde</param>
        /// <param name="esLunes">Flag que indica si se debe considerar la fecB</param>
        /// <param name="tipoPatron">Parámetro que indica el tipo de obtención del perfil patrón</param>
        /// <param name="dataMediciones">Mediciones que conforman el perfil patrón mostrado</param>
        /// <returns></returns>
        public object DemandaUpdatePatron(int idPunto, string regFechaA, string regFechaB,
            bool esLunes, string tipoPatron, List<decimal[]> dataMediciones)
        {
            PrnMedicion48DTO entPrincipal = new PrnMedicion48DTO();
            PrnMedicion48DTO entSecundaria = new PrnMedicion48DTO();

            entPrincipal = this.GetDemandaPorArea(idPunto, 1, regFechaA).
                FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();

            if (esLunes)
            {
                entSecundaria = this.GetDemandaPorArea(idPunto, 1, regFechaA).
                    FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
            }

            decimal[] arrayEntity = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, entPrincipal);

            //Llena el intervalo de la tarde si es un Lunes
            if (esLunes)
            {
                int i = ConstantesProdem.Itv30min / 2;
                decimal[] temp = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, entSecundaria);

                while (i <= ConstantesProdem.Itv30min)
                {
                    arrayEntity[i - 1] = temp[i - 1];
                    i++;
                }
            }

            //Calcula el nuevo perfil patrón
            dataMediciones.Add(arrayEntity);
            decimal[] nPatron = UtilProdem.CalcularPerfilPatron(dataMediciones, dataMediciones.Count, ConstantesProdem.Itv30min, tipoPatron);

            return new { patron = nPatron, medicion = arrayEntity };
        }

        #endregion

        #region Métodos del Módulo de Pronóstico - Demanda en barras por Áreas

        //Assetec 20220303
        /// <summary>
        /// Refresca las columnas de la grilla y la grafica segun se seleccione fechas en los calendarios(en el grupo de 7)
        /// </summary>
        /// <param name="idArea">Identificador del Área</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="idVersion">Identificador de la versión</param>
        /// <param name="grafica">indica el tipo de informacion que se mostrara en la grafica</param>
        /// <returns></returns>
        public object ActualizacionMedicionDemandaBarraByCalendario(int idArea, string regFecha, int idVersion, string grafica)
        {
            object entity = new object();
            DateTime parseFecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            //Datos del pronóstico
            int tipoData = 0;
            switch (grafica)
            {
                case "total":
                    {
                        tipoData = ConstantesProdem.PrnmgrtProdemBarra;
                    }
                    break;
                case "vegetativa":
                    {
                        tipoData = ConstantesProdem.PrnmgrtDemVegetativa;
                    }
                    break;
                default:
                    {
                        tipoData = ConstantesProdem.PrnmgrtDemIndustrial;
                    }
                    break;
            }

            switch (idArea)
            {
                case ConstantesProdem.PtomedicodiASein:
                    {
                        //Data para armar el Handsontable
                        decimal[] tempPronosticoHt = new decimal[ConstantesProdem.Itv30min];
                        decimal[] htBarraSur = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                this.GetDemandaBarraPorArea(ConstantesProdem.PtomedicodiASur, regFecha, idVersion));
                        decimal[] htSAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                    this.GetDemandaBarraPorArea(ConstantesProdem.PtomedicodiASur, regFecha, idVersion, ConstantesProdem.PrnmgrtProdemAreaAjuste));
                        decimal[] htBarraNorte = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                    this.GetDemandaBarraPorArea(ConstantesProdem.PtomedicodiANorte, regFecha, idVersion));
                        decimal[] htNAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                    this.GetDemandaBarraPorArea(ConstantesProdem.PtomedicodiANorte, regFecha, idVersion, ConstantesProdem.PrnmgrtProdemAreaAjuste));
                        decimal[] htBarraSCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                    this.GetDemandaBarraPorArea(ConstantesProdem.PtomedicodiASierraCentro, regFecha, idVersion));
                        decimal[] htSCAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                    this.GetDemandaBarraPorArea(ConstantesProdem.PtomedicodiASierraCentro, regFecha, idVersion, ConstantesProdem.PrnmgrtProdemAreaAjuste));
                        decimal[] htBarraCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                    this.GetDemandaBarraPorArea(ConstantesProdem.PtomedicodiACentro, regFecha, idVersion));
                        decimal[] htCAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                    this.GetDemandaBarraPorArea(ConstantesProdem.PtomedicodiACentro, regFecha, idVersion, ConstantesProdem.PrnmgrtProdemAreaAjuste));

                        //Data para armar el Highchart
                        decimal[] tempPronosticoHc = new decimal[ConstantesProdem.Itv30min];
                        decimal[] hcBarraSur = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                        this.GetDemandaBarraPorArea(ConstantesProdem.PtomedicodiASur, regFecha, idVersion, tipoData));
                        decimal[] hcBarraNorte = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                        this.GetDemandaBarraPorArea(ConstantesProdem.PtomedicodiASur, regFecha, idVersion, tipoData));
                        decimal[] hcBarraSCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                        this.GetDemandaBarraPorArea(ConstantesProdem.PtomedicodiASur, regFecha, idVersion, tipoData));
                        decimal[] hcBarraCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                        this.GetDemandaBarraPorArea(ConstantesProdem.PtomedicodiASur, regFecha, idVersion, tipoData));

                        for (int i = 0; i < ConstantesProdem.Itv30min; i++)
                        {
                            tempPronosticoHt[i] = htBarraSur[i] + htSAjuste[i] +
                                                  htBarraNorte[i] + htNAjuste[i] +
                                                  htBarraSCentro[i] + htSCAjuste[i] +
                                                  htBarraCentro[i] + htCAjuste[i];

                            tempPronosticoHc[i] = hcBarraSur[i] + hcBarraNorte[i] + hcBarraSCentro[i] + hcBarraCentro[i];
                        }

                        entity = new
                        {
                            id = parseFecha.ToString(ConstantesProdem.FormatoFecha),
                            label = parseFecha.ToString(ConstantesProdem.FormatoFecha),
                            dataht = tempPronosticoHt,
                            datahc = (grafica == "total") ? tempPronosticoHt : tempPronosticoHc,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                    }
                    break;
                default:
                    {
                        //Obtiene los datos del pronóstico por barras
                        decimal[] dataBarras = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                this.GetDemandaBarraPorArea(idArea, regFecha, idVersion));

                        decimal[] dataBarraAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                this.GetDemandaBarraPorArea(idArea, regFecha, idVersion, ConstantesProdem.PrnmgrtProdemAreaAjuste));

                        decimal[] tempPronosticoHt = new decimal[ConstantesProdem.Itv30min];

                        decimal[] tempPronosticoHc = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                this.GetDemandaBarraPorArea(idArea, regFecha, idVersion, tipoData));

                        for (int i = 0; i < ConstantesProdem.Itv30min; i++)
                        {
                            tempPronosticoHt[i] = dataBarras[i] + dataBarraAjuste[i];
                        }

                        entity = new
                        {
                            id = parseFecha.ToString(ConstantesProdem.FormatoFecha),
                            label = parseFecha.ToString(ConstantesProdem.FormatoFecha),
                            dataht = tempPronosticoHt,
                            datahc = (grafica == "total") ? tempPronosticoHt : tempPronosticoHc,
                            htrender = "normal",
                            hcrender = "normal"
                        };

                    }
                    break;
            }

            return entity;
        }


        /// <summary>
        /// General el modelo de datos del módulo
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="idVersion">Identificador de la version</param>
        /// <returns></returns>
        public object DemandaBarrasDatos(int idArea, string regFecha, int idVersion)
        {
            List<object> data = new List<object>();
            DateTime parseFecha = DateTime.ParseExact(regFecha,
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            switch (idArea)
            {
                case ConstantesProdem.PtomedicodiASein:
                    {
                        object entity = new object();

                        //Área Sur 
                        //----------------------------------------------------------------------------------------------------------------
                        //Obtiene los datos del pronóstico por barras
                        decimal[] dBarraSur = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                this.GetDemandaBarraPorArea(ConstantesProdem.PtomedicodiASur, regFecha, idVersion));

                        //Obtiene los datos del pronóstico por áreas
                        decimal[] dAreaSur = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetByIdPrnMedicion48(ConstantesProdem.PtomedicodiASur, ConstantesProdem.PrntProdemArea, parseFecha));

                        decimal[] dAreaSurAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetByIdPrnMedicion48(ConstantesProdem.PtomedicodiASur, ConstantesProdem.PrntProdemAreaAjuste, parseFecha));

                        //Área Norte
                        //----------------------------------------------------------------------------------------------------------------
                        //Obtiene los datos del pronóstico por barras
                        decimal[] dBarraNorte = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                    this.GetDemandaBarraPorArea(ConstantesProdem.PtomedicodiANorte, regFecha, idVersion));

                        //Obtiene los datos del pronóstico por áreas
                        decimal[] dAreaNorte = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetByIdPrnMedicion48(ConstantesProdem.PtomedicodiANorte, ConstantesProdem.PrntProdemArea, parseFecha));

                        decimal[] dAreaNorteAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetByIdPrnMedicion48(ConstantesProdem.PtomedicodiANorte, ConstantesProdem.PrntProdemAreaAjuste, parseFecha));

                        //Área Sierra centro
                        //----------------------------------------------------------------------------------------------------------------
                        //Obtiene los datos del pronóstico por barras
                        decimal[] dBarraSCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                    this.GetDemandaBarraPorArea(ConstantesProdem.PtomedicodiASierraCentro, regFecha, idVersion));

                        //Obtiene los datos del pronóstico por áreas
                        decimal[] dAreaSCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetByIdPrnMedicion48(ConstantesProdem.PtomedicodiASierraCentro, ConstantesProdem.PrntProdemArea, parseFecha));

                        decimal[] dAreaSCentroAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetByIdPrnMedicion48(ConstantesProdem.PtomedicodiASierraCentro, ConstantesProdem.PrntProdemAreaAjuste, parseFecha));

                        //Área Centro
                        //----------------------------------------------------------------------------------------------------------------
                        //Obtiene los datos del pronóstico por barras
                        decimal[] dBarraCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                    this.GetDemandaBarraPorArea(ConstantesProdem.PtomedicodiACentro, regFecha, idVersion));

                        //Obtiene los datos del pronóstico por áreas
                        decimal[] dAreaCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetByIdPrnMedicion48(ConstantesProdem.PtomedicodiACentro, ConstantesProdem.PrntProdemArea, parseFecha));

                        decimal[] dAreaCentroAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetByIdPrnMedicion48(ConstantesProdem.PtomedicodiACentro, ConstantesProdem.PrntProdemAreaAjuste, parseFecha));

                        //Sumatoria de los ajustes
                        for (int i = 0; i < ConstantesProdem.Itv30min; i++)
                        {
                            dAreaSur[i] += dAreaSurAjuste[i];
                            dAreaNorte[i] += dAreaNorteAjuste[i];
                            dAreaSCentro[i] += dAreaSCentroAjuste[i];
                            dAreaCentro[i] += dAreaCentroAjuste[i];
                        }

                        //Obtiene Sein
                        //----------------------------------------------------------------------------------------------------------------
                        decimal[] dBarraSein = new decimal[ConstantesProdem.Itv30min];
                        decimal[] dAreaSein = new decimal[ConstantesProdem.Itv30min];

                        for (int i = 0; i < ConstantesProdem.Itv30min; i++)
                        {
                            dBarraSein[i] = dBarraSur[i] + dBarraNorte[i] + dBarraSCentro[i] + dBarraCentro[i];
                            dAreaSein[i] = dAreaSur[i] + dAreaNorte[i] + dAreaSCentro[i] + dAreaCentro[i];
                        }

                        //Crea el modelo de datos
                        entity = new
                        {
                            id = "intervalos",
                            label = "Hora",
                            data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                            htrender = "hora",
                            hcrender = "categoria"
                        };
                        data.Add(entity);

                        for (int i = 0; i < 7; i++)//modelo.mediciones.count
                        {
                            entity = new
                            {
                                id = "med" + (i + 1).ToString(),
                                label = "No encontrado",
                                //labelFecha = new string[7],
                                data = new decimal[48],
                                htrender = "normal",
                                hcrender = "normal"
                            };
                            data.Add(entity);
                        }

                        entity = new
                        {
                            id = "sur",
                            label = "DemBar.Sur",
                            data = dBarraSur,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "norte",
                            label = "DemBar.Norte",
                            data = dBarraNorte,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "scentro",
                            label = "DemBar.SCentro",
                            data = dBarraSCentro,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "centro",
                            label = "DemBar.Centro",
                            data = dBarraCentro,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "area",
                            label = "Sein",
                            data = dBarraSein,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "barra",
                            label = "DemBar.Sein",
                            data = dBarraSein,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);
                    }
                    break;
                default:
                    {
                        object entity = new object();

                        //Obtiene los datos del pronóstico por barras
                        //Data Base con sus ajustes por barra y agrupacion
                        decimal[] dataBarras = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                this.GetDemandaBarraPorArea(idArea, regFecha, idVersion));

                        //Data de ajustes por Areas
                        decimal[] dataBarraAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                this.GetDemandaBarraPorArea(idArea, regFecha, idVersion, ConstantesProdem.PrnmgrtProdemAreaAjuste));

                        decimal[] dataFinal = new decimal[ConstantesProdem.Itv30min];

                        //Obtiene los datos del pronóstico por áreas
                        decimal[] dataArea = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetByIdPrnMedicion48(idArea, ConstantesProdem.PrntProdemArea, parseFecha));
                        decimal[] dataAreaAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetByIdPrnMedicion48(idArea, ConstantesProdem.PrntProdemAreaAjuste, parseFecha));

                        for (int i = 0; i < ConstantesProdem.Itv30min; i++)
                        {
                            dataArea[i] += dataAreaAjuste[i];
                            dataFinal[i] = dataBarras[i] + dataBarraAjuste[i];
                        }

                        entity = new
                        {
                            id = "intervalos",
                            label = "Hora",
                            data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                            htrender = "hora",
                            hcrender = "categoria"
                        };
                        data.Add(entity);

                        for (int i = 0; i < 7; i++)//modelo.mediciones.count
                        {
                            entity = new
                            {
                                id = "med" + (i + 1).ToString(),
                                label = "No encontrado",
                                //labelFecha = new string[7],
                                data = new decimal[48],
                                htrender = "normal",
                                hcrender = "normal"
                            };
                            data.Add(entity);
                        }

                        entity = new
                        {
                            id = "area",
                            label = "Demanda Áreas",
                            data = dataArea,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "barra",
                            label = "Demanda Barras",
                            data = dataBarras,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "ajuste",
                            label = "Ajuste",
                            data = dataBarraAjuste,
                            htrender = "edit",
                            hcrender = "no"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "final",
                            label = "Final",
                            data = dataFinal,
                            htrender = "final",
                            hcrender = "final"
                        };
                        data.Add(entity);
                    }
                    break;
            }

            return new { data = data };
        }

        /// <summary>
        /// Devuelve los datos del pronóstico por barras CP agrupado por áreas operativas
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="idVersion">Version a la que se le consulta la data</param>
        /// <returns></returns>
        public PrnMediciongrpDTO GetDemandaBarraPorArea(int idArea, string regFecha, int idVersion)
        {
            int id = 0;
            if (idArea == ConstantesProdem.PtomedicodiASur) id = ConstantesProdem.AreacodiASur;
            if (idArea == ConstantesProdem.PtomedicodiANorte) id = ConstantesProdem.AreacodiANorte;
            if (idArea == ConstantesProdem.PtomedicodiASierraCentro) id = ConstantesProdem.AreacodiASierraCentro;
            if (idArea == ConstantesProdem.PtomedicodiACentro) id = ConstantesProdem.AreacodiACentro;

            string tipo = string.Join(",",
                new int[] { ConstantesProdem.PrnmgrtProdemBarra,
                                ConstantesProdem.PrnmgrtProdemBarraAjuste,
                                ConstantesProdem.PrnmgrtProdemAgrupacionAjuste});

            return FactorySic.GetPrnMediciongrpRepository().GetDemandaBarraByArea(id, regFecha, tipo, idVersion);
        }

        //Assetec 20220303
        /// <summary>
        /// Devuelve los datos del pronóstico por barras CP agrupado por áreas operativas
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="version">Fecha del registro</param>
        /// <param name="tipo">Tipo de informacion</param>
        /// <returns></returns>
        public PrnMediciongrpDTO GetDemandaBarraPorArea(int idArea, string regFecha, int version, int tipo)
        {
            int id = 0;
            if (idArea == ConstantesProdem.PtomedicodiASur) id = ConstantesProdem.AreacodiASur;
            if (idArea == ConstantesProdem.PtomedicodiANorte) id = ConstantesProdem.AreacodiANorte;
            if (idArea == ConstantesProdem.PtomedicodiASierraCentro) id = ConstantesProdem.AreacodiASierraCentro;
            if (idArea == ConstantesProdem.PtomedicodiACentro) id = ConstantesProdem.AreacodiACentro;

            return FactorySic.GetPrnMediciongrpRepository().GetDemandaBarraByAreaVersion(id, regFecha, tipo, version);
        }


        /// <summary>
        /// Devuelve una lista con los datos del pronóstico de las barras CP agrupado por áreas operativas y sumados los tipos 1,2,9,10
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="idVersion">Version a la que se le consulta la data</param>
        /// <returns></returns>
        public List<PrnMediciongrpDTO> GetDemandaBarraPorAreaTipo(int idArea, string regFecha, int idVersion)
        {
            int id = 0;
            if (idArea == ConstantesProdem.PtomedicodiASur) id = ConstantesProdem.AreacodiASur;
            if (idArea == ConstantesProdem.PtomedicodiANorte) id = ConstantesProdem.AreacodiANorte;
            if (idArea == ConstantesProdem.PtomedicodiASierraCentro) id = ConstantesProdem.AreacodiASierraCentro;
            if (idArea == ConstantesProdem.PtomedicodiACentro) id = ConstantesProdem.AreacodiACentro;

            string tipo = string.Join(",",
                new int[] { ConstantesProdem.PrnmgrtProdemBarra,
                                ConstantesProdem.PrnmgrtProdemBarraAjuste,
                                ConstantesProdem.PrnmgrtProdemAgrupacionAjuste});

            return FactorySic.GetPrnMediciongrpRepository().GetDemandaBarraByAreaTipo(id, regFecha, tipo, idVersion);
        }

        /// <summary>
        /// Registra el ajuste de la medición
        /// </summary>
        /// <param name="idArea">Identificador del Area</param>
        /// <param name="regFecha">Fecha del registro</param> 
        /// <param name="dataMedicion">Datos del ajuste realizado al área</param>
        /// <param name="dataBarra">Datos de las Barras</param>
        /// <param name="nomUsuario">Nombre del usuario</param>
        /// <param name="idVersion">Identificador del Area</param>
        /// <returns></returns>
        public object DemandaBarraSave(int idArea, string regFecha, PrnMediciongrpDTO dataMedicion, PrnMediciongrpDTO dataBarra, string nomUsuario, int idVersion)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;

            decimal[] arrayBarra = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, dataBarra);
            decimal[] arrayAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, dataMedicion);
            //Copia los valores por intervalo (H1, H2, ...)
            PrnMediciongrpDTO entity = dataMedicion;

            entity.Medifecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            entity.Prnmgrtipo = ConstantesProdem.PrnmgrtProdemAreaAjuste;
            entity.Vergrpcodi = idVersion;
            entity.Prnmgrpadre = ConstantesProdem.PrnmgrtProdemBarraSinPadre;

            //LIsta las barras por area con sus H1, H2 sumados los tipos 1,2,9 y 10 
            List<PrnMediciongrpDTO> listBarra = this.GetDemandaBarraPorAreaTipo(idArea, regFecha, idVersion);
            foreach (var item in listBarra)
            {
                entity.Grupocodi = item.Grupocodi;

                //Validacion
                PrnMediciongrpDTO isValid = this.GetByIdPrnMediciongrp(entity.Grupocodi,
                                                    entity.Prnmgrtipo, entity.Medifecha, entity.Vergrpcodi);

                decimal[] arrayParcial = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, item);

                int x = 0;
                while (x < ConstantesProdem.Itv30min)
                {
                    decimal total = arrayParcial[x] * (arrayAjuste[x] / arrayBarra[x]);
                    entity.GetType().GetProperty("H" + (x + 1)).SetValue(entity, total);
                    x++;
                }

                if (isValid.Grupocodi != 0)
                {
                    entity.Prnmgrusumodificacion = nomUsuario;
                    entity.Prnmgrfecmodificacion = DateTime.Now;

                    this.UpdatePrnMediciongrp(entity);
                    typeMsg = ConstantesProdem.MsgSuccess;
                    dataMsg = "El registro se actualizó de manera exitosa";
                }
                else
                {
                    entity.Prnmgrusucreacion = nomUsuario;
                    entity.Prnmgrfeccreacion = DateTime.Now;
                    entity.Prnmgrusumodificacion = nomUsuario;
                    entity.Prnmgrfecmodificacion = DateTime.Now;

                    this.SavePrnMediciongrp(entity);
                    typeMsg = ConstantesProdem.MsgSuccess;
                    dataMsg = "El registro se realizó de manera exitosa";
                }
            }

            return new { typeMsg, dataMsg };
        }



        #endregion

        #region Métodos del Módulo de Pronóstico - Demanda vegetativa por Áreas

        /// <summary>
        /// General el modelo de datos del módulo
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public object VegetativaDatos(int idArea, string regFecha)
        {
            List<object> data = new List<object>();
            DateTime parseFecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            //Obtiene los parámetros configurados
            PrnConfiguracionDTO regConfiguracion = this.ParametrosGetConfiguracion(idArea, ConstantesProdem.DefectoByArea, parseFecha);
            PrnPatronModel regPatron = this.GetPatronVegetativa(idArea, parseFecha);

            switch (idArea)
            {
                case ConstantesProdem.PtomedicodiASein:
                    {
                        #region Demanda del área total (SEIN)

                        //Obtiene los datos del Sur
                        List<PrnMedicion48DTO> dataSur = this.GetVegetativaPorArea(ConstantesProdem.PtomedicodiASur, 1, regFecha);
                        PrnMedicion48DTO sSur = dataSur.FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        decimal[] dSur = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sSur);

                        //Obtiene los datos del Norte
                        List<PrnMedicion48DTO> dataNorte = this.GetVegetativaPorArea(ConstantesProdem.PtomedicodiANorte, 1, regFecha);
                        PrnMedicion48DTO sNorte = dataNorte.FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        decimal[] dNorte = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sNorte);

                        //Obtiene los datos de la Sierra Centro
                        List<PrnMedicion48DTO> dataSCentro = this.GetVegetativaPorArea(ConstantesProdem.PtomedicodiASierraCentro, 1, regFecha);
                        PrnMedicion48DTO sSCentro = dataSCentro.FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        decimal[] dSCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sSCentro);

                        //Obtiene los datos del Centro
                        List<PrnMedicion48DTO> dataCentro = this.GetVegetativaPorArea(ConstantesProdem.PtomedicodiACentro, 1, regFecha);
                        PrnMedicion48DTO sCentro = dataCentro.FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        decimal[] dCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sCentro);

                        //Obtiene los datos del Sein
                        decimal[] dSein = new decimal[ConstantesProdem.Itv30min];
                        for (int i = 0; i < ConstantesProdem.Itv30min; i++)
                        {
                            dSein[i] = dSur[i] + dNorte[i] + dSCentro[i] + dCentro[i];
                        }

                        object entity = new object();

                        entity = new
                        {
                            id = "intervalos",
                            label = "Hora",
                            data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                            htrender = "hora",
                            hcrender = "categoria"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "sur",
                            label = "Sur(S)",
                            data = dSur,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "norte",
                            label = "Norte(N)",
                            data = dNorte,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "scentro",
                            label = "SCentro(SC)",
                            data = dSCentro,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "centro",
                            label = "Centro(C)",
                            data = dCentro,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "sein",
                            label = "<label title='S + N + SC + C'>Sein</label>",
                            data = dSein,
                            htrender = "final",
                            hcrender = "final_noedit"
                        };
                        data.Add(entity);

                        //Mediciones historicas
                        for (int i = 0; i < regPatron.Mediciones.Count; i++)
                        {
                            entity = new
                            {
                                id = "med" + (i + 1).ToString(),
                                label = regPatron.StrFechas[i],
                                data = regPatron.Mediciones[i],
                                htrender = "no",
                                hcrender = "no",
                                label2 = regPatron.StrFechasTarde[i],
                                slunes = regPatron.EsLunes
                            };
                            data.Add(entity);
                        }
                        #endregion
                    }
                    break;
                case ConstantesProdem.PtomedicodiACentro:
                    {
                        #region Demanda del área Centro

                        List<PrnMedicion48DTO> dataVegetativa = this.GetVegetativaPorArea(idArea, 1, regFecha);

                        //Vegetativa total
                        PrnMedicion48DTO sFinal = dataVegetativa.FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        decimal[] dFinal = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sFinal);

                        //Demanda del área
                        PrnMedicion48DTO sDemanda = dataVegetativa.FirstOrDefault(x => x.Prnm48tipo == 1) ?? new PrnMedicion48DTO();
                        decimal[] dDemanda = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sDemanda);

                        //Usuarios libres
                        PrnMedicion48DTO sULibres = dataVegetativa.FirstOrDefault(x => x.Prnm48tipo == ConstantesProdem.PrntDemandaULibre) ?? new PrnMedicion48DTO();
                        decimal[] dULibres = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sULibres);

                        //Ajuste
                        PrnMedicion48DTO sAjuste = dataVegetativa.FirstOrDefault(
                            x => x.Prnm48tipo == ConstantesProdem.PrntDemandaVegetativaAjuste &&
                            x.Ptomedicodi == ConstantesProdem.PtomedicodiACentro) ?? new PrnMedicion48DTO();
                        decimal[] dAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sAjuste);

                        //Ajuste Sur
                        PrnMedicion48DTO sSur = dataVegetativa.FirstOrDefault(x => x.Ptomedicodi == ConstantesProdem.PtomedicodiASur) ?? new PrnMedicion48DTO();
                        decimal[] dSur = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sSur);

                        //Ajuste Norte
                        PrnMedicion48DTO SNorte = dataVegetativa.FirstOrDefault(x => x.Ptomedicodi == ConstantesProdem.PtomedicodiANorte) ?? new PrnMedicion48DTO();
                        decimal[] dNorte = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, SNorte);

                        //Ajuste Sierra Centro
                        PrnMedicion48DTO sSCentro = dataVegetativa.FirstOrDefault(x => x.Ptomedicodi == ConstantesProdem.PtomedicodiASierraCentro) ?? new PrnMedicion48DTO();
                        decimal[] dSCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sSCentro);

                        object entity = new object();

                        entity = new
                        {
                            id = "intervalos",
                            label = "Hora",
                            data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                            htrender = "hora",
                            hcrender = "categoria"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "demanda",
                            label = "Demanda(D)",
                            data = dDemanda,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "usulib",
                            label = "ULibre(UL)",
                            data = dULibres,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "sur",
                            label = "Sur(aS)",
                            data = dSur,
                            htrender = "normal",
                            hcrender = "no"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "norte",
                            label = "Norte(aN)",
                            data = dNorte,
                            htrender = "normal",
                            hcrender = "no"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "scentro",
                            label = "SCentro(aSC)",
                            data = dSCentro,
                            htrender = "normal",
                            hcrender = "no"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "ajuste",
                            label = "Ajuste",
                            data = dAjuste,
                            htrender = "edit",
                            hcrender = "no"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "vegetativa",
                            label = "<label title='D - UL + aS + aN + aSC'>Vegetativa</label>",
                            data = dFinal,
                            htrender = "final",
                            hcrender = "final"
                        };
                        data.Add(entity);

                        //Mediciones historicas
                        for (int i = 0; i < regPatron.Mediciones.Count; i++)
                        {
                            entity = new
                            {
                                id = "med" + (i + 1).ToString(),
                                label = regPatron.StrFechas[i],
                                data = regPatron.Mediciones[i],
                                htrender = "no",
                                hcrender = "normal",
                                label2 = regPatron.StrFechasTarde[i],
                                slunes = regPatron.EsLunes
                            };
                            data.Add(entity);
                        }

                        entity = new
                        {
                            id = "patron",
                            label = "Patrón",
                            data = regPatron.Patron,
                            htrender = "no",
                            hcrender = "patron"
                        };
                        data.Add(entity);
                        #endregion
                    }
                    break;
                default:
                    {
                        #region Demanda de las áreas Sur, Norte y Sierra centro

                        List<PrnMedicion48DTO> dataVegetativa = this.GetVegetativaPorArea(idArea, 1, regFecha);

                        //Vegetativa total
                        PrnMedicion48DTO sFinal = dataVegetativa.FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        decimal[] dFinal = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sFinal);

                        //Demanda del área
                        PrnMedicion48DTO sDemanda = dataVegetativa.FirstOrDefault(x => x.Prnm48tipo == 1) ?? new PrnMedicion48DTO();
                        decimal[] dDemanda = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sDemanda);

                        //Usuarios libres
                        PrnMedicion48DTO sULibres = dataVegetativa.FirstOrDefault(x => x.Prnm48tipo == ConstantesProdem.PrntDemandaULibre) ?? new PrnMedicion48DTO();
                        decimal[] dULibres = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sULibres);

                        //Ajuste
                        PrnMedicion48DTO sAjuste = dataVegetativa.FirstOrDefault(x => x.Prnm48tipo == ConstantesProdem.PrntDemandaVegetativaAjuste) ?? new PrnMedicion48DTO();
                        decimal[] dAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sAjuste);

                        object entity = new object();

                        entity = new
                        {
                            id = "intervalos",
                            label = "Hora",
                            data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                            htrender = "hora",
                            hcrender = "categoria"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "demanda",
                            label = "Demanda(D)",
                            data = dDemanda,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "usulib",
                            label = "ULibre(UL)",
                            data = dULibres,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "ajuste",
                            label = "Ajuste",
                            data = dAjuste,
                            htrender = "edit",
                            hcrender = "no"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "vegetativa",
                            label = "<label title='D - UL'>Vegetativa</label>",
                            data = dFinal,
                            htrender = "final",
                            hcrender = "final"
                        };
                        data.Add(entity);

                        //Mediciones historicas
                        for (int i = 0; i < regPatron.Mediciones.Count; i++)
                        {
                            entity = new
                            {
                                id = "med" + (i + 1).ToString(),
                                label = regPatron.StrFechas[i],
                                data = regPatron.Mediciones[i],
                                htrender = "no",
                                hcrender = "normal",
                                label2 = regPatron.StrFechasTarde[i],
                                slunes = regPatron.EsLunes
                            };
                            data.Add(entity);
                        }

                        entity = new
                        {
                            id = "patron",
                            label = "Patrón",
                            data = regPatron.Patron,
                            htrender = "no",
                            hcrender = "patron"
                        };
                        data.Add(entity);
                        #endregion
                    }
                    break;
            }

            return new { data = data, cfg = regConfiguracion };
        }

        /// <summary>
        /// Registra el ajuste de la medición
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="dataMedicion">Datos del ajuste realizado al área</param>
        /// <param name="nomUsuario">Nombre del usuario</param>
        /// <returns></returns>
        public object VegetativaSave(int idPunto, string regFecha, PrnMedicion48DTO dataMedicion, string nomUsuario)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;

            //Copia los valores por intervalo (H1, H2, ...)
            PrnMedicion48DTO entity = dataMedicion;

            entity.Ptomedicodi = idPunto;
            entity.Prnm48tipo = ConstantesProdem.PrntDemandaVegetativaAjuste;
            entity.Medifecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            //Validación
            PrnMedicion48DTO isValid = this.GetByIdPrnMedicion48(entity.Ptomedicodi,
                entity.Prnm48tipo, entity.Medifecha);

            if (isValid.Ptomedicodi != 0)
            {
                entity.Prnm48usumodificacion = nomUsuario;
                entity.Prnm48fecmodificacion = DateTime.Now;

                this.UpdatePrnMedicion48(entity);
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El registro se actualizó de manera exitosa";
            }
            else
            {
                entity.Prnm48usucreacion = nomUsuario;
                entity.Prnm48feccreacion = DateTime.Now;
                entity.Prnm48usumodificacion = nomUsuario;
                entity.Prnm48fecmodificacion = DateTime.Now;

                this.SavePrnMedicion48(entity);
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El registro se realizó de manera exitosa";
            }

            return new { typeMsg, dataMsg };
        }

        /// <summary>
        /// Devuelve el perfil patrón actualizado y la nueva linea seleccionada
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFechaA">Fecha del intervalo correspondiente al intervalo de la mañana</param>
        /// <param name="regFechaB">Fecha del intervalo correspondiente al intervalo de la tarde</param>
        /// <param name="esLunes">Flag que indica si se debe considerar la fecB</param>
        /// <param name="tipoPatron">Parámetro que indica el tipo de obtención del perfil patrón</param>
        /// <param name="dataMediciones">Mediciones que conforman el perfil patrón mostrado</param>
        /// <returns></returns>
        public object VegetativaUpdatePatron(int idPunto, string regFechaA, string regFechaB,
            bool esLunes, string tipoPatron, List<decimal[]> dataMediciones)
        {
            PrnMedicion48DTO entPrincipal = new PrnMedicion48DTO();
            PrnMedicion48DTO entSecundaria = new PrnMedicion48DTO();

            entPrincipal = this.GetVegetativaPorArea(idPunto, 1, regFechaA).
                FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();

            if (esLunes)
            {
                entSecundaria = this.GetVegetativaPorArea(idPunto, 1, regFechaA).
                    FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
            }

            decimal[] arrayEntity = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, entPrincipal);

            //Llena el intervalo de la tarde si es un Lunes
            if (esLunes)
            {
                int i = ConstantesProdem.Itv30min / 2;
                decimal[] temp = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, entSecundaria);

                while (i <= ConstantesProdem.Itv30min)
                {
                    arrayEntity[i - 1] = temp[i - 1];
                    i++;
                }
            }

            //Calcula el nuevo perfil patrón
            dataMediciones.Add(arrayEntity);
            decimal[] nPatron = UtilProdem.CalcularPerfilPatron(dataMediciones, dataMediciones.Count, ConstantesProdem.Itv30min, tipoPatron);

            return new { patron = nPatron, medicion = arrayEntity };
        }

        #endregion

        #region Métodos del Módulo de Pronóstico - Pronóstico por Áreas

        /// <summary>
        /// General el modelo de datos del módulo
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public object PronosticoPorAreasDatos(int idArea, string regFecha)
        {
            bool isValid = false;
            string typeMsg = String.Empty;
            string dataMsg = String.Empty;
            List<object> data = new List<object>();
            DateTime parseFecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            switch (idArea)
            {
                case ConstantesProdem.PtomedicodiASein:
                    {
                        #region Demanda del área total (SEIN)
                        object entity = new object();

                        decimal[] dataPronostico = UtilProdem.
                            ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetByIdPrnMedicion48(idArea, ConstantesProdem.PrntProdemArea, parseFecha));

                        //Mensaje de validación
                        if (dataPronostico.Sum() == 0)
                        {
                            typeMsg = ConstantesProdem.MsgWarning;
                            dataMsg = "Es necesario ejecutar el pronóstico de la demanda para este día";
                        }
                        else
                        {
                            isValid = true;
                            typeMsg = ConstantesProdem.MsgSuccess;
                            dataMsg = "El pronóstico de la demanda ya ha sido ejecutado para este día";
                        }

                        decimal[] dataAjuste = UtilProdem.
                            ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetByIdPrnMedicion48(idArea, ConstantesProdem.PrntProdemAreaAjuste, parseFecha));

                        //Datos del área Sur
                        decimal[] dataSur = UtilProdem.
                            ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetByIdPrnMedicion48(ConstantesProdem.PtomedicodiASur, ConstantesProdem.PrntProdemArea, parseFecha));

                        //Datos del área Norte
                        decimal[] dataNorte = UtilProdem.
                            ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetByIdPrnMedicion48(ConstantesProdem.PtomedicodiANorte, ConstantesProdem.PrntProdemArea, parseFecha));

                        //Datos del área Sierra Centro
                        decimal[] dataSCentro = UtilProdem.
                            ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetByIdPrnMedicion48(ConstantesProdem.PtomedicodiASierraCentro, ConstantesProdem.PrntProdemArea, parseFecha));

                        //Datos del área Centro
                        decimal[] dataCentro = UtilProdem.
                            ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetByIdPrnMedicion48(ConstantesProdem.PtomedicodiACentro, ConstantesProdem.PrntProdemArea, parseFecha));

                        entity = new
                        {
                            id = "intervalos",
                            label = "Hora",
                            data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                            htrender = "hora",
                            hcrender = "categoria"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "sur",
                            label = "Sur",
                            data = dataSur,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "norte",
                            label = "Norte",
                            data = dataNorte,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "scentro",
                            label = "SCentro",
                            data = dataSCentro,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "centro",
                            label = "Centro",
                            data = dataCentro,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "final",
                            label = "Sein",
                            data = dataPronostico,
                            htrender = "final",
                            hcrender = "final"
                        };
                        data.Add(entity);
                        #endregion
                    }
                    break;
                default:
                    {
                        #region Demanda de las áreas Sur, Norte, Sierra centro y Centro
                        object entity = new object();

                        //Obtiene el pronóstico de la demanda calculado (Lista)
                        decimal[] dataPronostico = UtilProdem.
                            ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetByIdPrnMedicion48(idArea, ConstantesProdem.PrntProdemArea, parseFecha));

                        //Mensaje de validación
                        if (dataPronostico.Sum() == 0)
                        {
                            typeMsg = ConstantesProdem.MsgWarning;
                            dataMsg = "Es necesario ejecutar el pronóstico de la demanda para este día";
                        }
                        else
                        {
                            isValid = true;
                            typeMsg = ConstantesProdem.MsgSuccess;
                            dataMsg = "El pronóstico de la demanda ya ha sido ejecutado para este día";
                        }

                        decimal[] dataAjuste = UtilProdem.
                            ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetByIdPrnMedicion48(idArea, ConstantesProdem.PrntProdemAreaAjuste, parseFecha));

                        //Obtiene el pronóstico de la demanda vegetativo calculado
                        decimal[] dataProdemVegetativo = UtilProdem.
                            ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetByIdPrnMedicion48(idArea, ConstantesProdem.PrntProdemVegetativa, parseFecha));

                        //Obtiene el pronóstico de la demanda industrial calculado (usuarios libres)
                        decimal[] dataProdemIndustrial = UtilProdem.
                            ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetByIdPrnMedicion48(idArea, ConstantesProdem.PrntProdemIndustrial, parseFecha));

                        //Obtiene el pronóstico de la demanda + el ajuste
                        int x = 0;
                        decimal[] dataFinal = new decimal[ConstantesProdem.Itv30min];
                        while (x < ConstantesProdem.Itv30min)
                        {
                            dataFinal[x] = dataPronostico[x] + dataAjuste[x];
                            x++;
                        }

                        //Obtiene los datos de la demanda vegetativa del área
                        PrnPatronModel datosVegetativa = this.GetPatronVegetativa(idArea, parseFecha);

                        //Obtiene los datos de la demanda infustrial (previsto semanal o diario) de los usuarios libres
                        List<decimal[]> datosIndustrial = new List<decimal[]>();
                        for (int j = 0; j < datosVegetativa.NDias; j++)
                        {
                            decimal[] dIndustrial = UtilProdem.
                                ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                this.GetDemandaULibresPorArea(idArea, parseFecha.ToString(ConstantesProdem.FormatoFecha), 3));

                            datosIndustrial.Add(dIndustrial);
                        }

                        entity = new
                        {
                            id = "intervalos",
                            label = "Hora",
                            data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                            htrender = "hora",
                            hcrender = "categoria"
                        };
                        data.Add(entity);

                        for (int i = 0; i < datosVegetativa.NDias; i++)
                        {
                            entity = new
                            {
                                id = "vgt" + (i + 1).ToString(),
                                label = datosVegetativa.StrFechas[i] + "(VG)",
                                data = datosVegetativa.Mediciones[i],
                                htrender = "normal",
                                hcrender = "normal"
                            };
                            data.Add(entity);

                            entity = new
                            {
                                id = "ulib" + (i + 1).ToString(),
                                label = datosVegetativa.StrFechas[i] + "(UL)",
                                data = datosIndustrial[i],
                                htrender = "normal",
                                hcrender = "normal"
                            };
                            data.Add(entity);
                        }

                        entity = new
                        {
                            id = "prodem_vg",
                            label = "Pronóstico(VG)",
                            data = dataProdemVegetativo,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "prodem_ul",
                            label = "Pronóstico(UL)",
                            data = dataProdemIndustrial,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "base",
                            label = "Pronóstico",
                            data = dataPronostico,
                            htrender = "normal",
                            hcrender = "no"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "ajuste",
                            label = "Ajuste",
                            data = dataAjuste,
                            htrender = "edit",
                            hcrender = "no"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "final",
                            label = "Final",
                            data = dataFinal,
                            htrender = "final",
                            hcrender = "final"
                        };
                        data.Add(entity);
                        #endregion
                    }
                    break;
            }

            return new { data, typeMsg, dataMsg, isValid };
        }

        /// <summary>
        /// Permite calcular el pronóstico de la demanda diario o semanal
        /// </summary>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="numIteraciones">Número de días o semanas a calcular</param>
        /// <param name="idTipo">Identificador del tipo (Diario o semanal)</param>
        public void PronosticoPorAreasEjecutar(string regFecha, int numIteraciones, string idTipo)
        {
            int i = 0;
            int numDias = (idTipo == "D") ? numIteraciones : numIteraciones * 7;
            DateTime parseDate = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            DateTime tempDate = new DateTime();
            while (i < numDias)
            {
                tempDate = parseDate.AddDays(i);

                //Sur
                decimal[] dataSur = this.GetPronosticoPorArea(ConstantesProdem.PtomedicodiASur, tempDate);
                //Norte
                decimal[] dataNorte = this.GetPronosticoPorArea(ConstantesProdem.PtomedicodiANorte, tempDate);
                //Sierra centro
                decimal[] dataSCentro = this.GetPronosticoPorArea(ConstantesProdem.PtomedicodiASierraCentro, tempDate);
                //Centro
                decimal[] dataCentro = this.GetPronosticoPorArea(ConstantesProdem.PtomedicodiACentro, tempDate);

                //SEIN
                decimal[] dataSein = new decimal[ConstantesProdem.Itv30min];
                for (int j = 0; j < ConstantesProdem.Itv30min; j++)
                {
                    dataSein[j] = dataSur[j] + dataNorte[j] + dataSCentro[j] + dataCentro[j];
                }

                PrnMedicion48DTO entitySein = new PrnMedicion48DTO
                {
                    Ptomedicodi = ConstantesProdem.PtomedicodiASein,
                    Prnm48tipo = ConstantesProdem.PrntProdemArea,
                    Medifecha = tempDate
                };
                this.DeletePrnMedicion48(ConstantesProdem.PtomedicodiASein, ConstantesProdem.PrntProdemArea, tempDate);
                this.SavePrnMedicion48(entitySein, dataSein);

                i++;
            }
        }

        /// <summary>
        /// Registra el ajuste de la medición
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="dataMedicion">Datos del ajuste realizado al área</param>
        /// <param name="nomUsuario">Nombre del usuario</param>
        /// <returns></returns>
        public object PronosticoPorAreasSave(int idArea, string regFecha, PrnMedicion48DTO dataMedicion, string nomUsuario)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;

            //Copia los valores por intervalo (H1, H2, ...)
            PrnMedicion48DTO entity = dataMedicion;

            entity.Ptomedicodi = idArea;
            entity.Prnm48tipo = ConstantesProdem.PrntProdemAreaAjuste;
            entity.Medifecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            //Validación
            PrnMedicion48DTO isValid = this.GetByIdPrnMedicion48(entity.Ptomedicodi,
                entity.Prnm48tipo, entity.Medifecha);

            if (isValid.Ptomedicodi != 0)
            {
                entity.Prnm48usumodificacion = nomUsuario;
                entity.Prnm48fecmodificacion = DateTime.Now;

                this.UpdatePrnMedicion48(entity);
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El registro se actualizó de manera exitosa";
            }
            else
            {
                entity.Prnm48usucreacion = nomUsuario;
                entity.Prnm48feccreacion = DateTime.Now;
                entity.Prnm48usumodificacion = nomUsuario;
                entity.Prnm48fecmodificacion = DateTime.Now;

                this.SavePrnMedicion48(entity);
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El registro se realizó de manera exitosa";
            }

            return new { typeMsg, dataMsg };
        }

        /// <summary>
        /// Obtiene el pronóstico por área operativa (no aplica para el SEIN)
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public decimal[] GetPronosticoPorArea(int idArea, DateTime regFecha)
        {
            //Obtiene los datos de la demanda vegetativa del área
            PrnPatronModel datosVegetativa = this.GetPatronVegetativa(idArea, regFecha);

            //Obtiene los datos de la demanda infustrial (previsto semanal o diario) de los usuarios libres
            List<decimal[]> datosIndustrial = new List<decimal[]>();
            for (int j = 0; j < datosVegetativa.NDias; j++)
            {
                decimal[] dIndustrial = UtilProdem.
                    ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                    this.GetDemandaULibresPorArea(idArea, regFecha.ToString(ConstantesProdem.FormatoFecha), 3));

                datosIndustrial.Add(dIndustrial);
            }

            //Obtiene los datos de la regla de dias para el pronóstico
            Tuple<int, List<int>> datosDias = UtilProdem.PronosticoReglaDias(regFecha, datosVegetativa.NDias);

            //Calcula el pronóstico de la demanda
            List<decimal> refVegetativa = new List<decimal>();
            List<decimal> refIndustrial = new List<decimal>();

            decimal[] datosPronostico = this.CalcularPronosticoPorArea(datosVegetativa.Mediciones,
                datosIndustrial, datosVegetativa.NDias, datosDias.Item2, datosDias.Item1, ref refVegetativa, ref refIndustrial);

            //Realiza el registro del pronóstico del área
            PrnMedicion48DTO entity = new PrnMedicion48DTO
            {
                Ptomedicodi = idArea,
                Prnm48tipo = ConstantesProdem.PrntProdemArea,
                Medifecha = regFecha
            };
            this.DeletePrnMedicion48(idArea, ConstantesProdem.PrntProdemArea, regFecha);
            this.SavePrnMedicion48(entity, datosPronostico);

            //Registra el pronóstico de la demanda vegetativa
            entity = new PrnMedicion48DTO
            {
                Ptomedicodi = idArea,
                Prnm48tipo = ConstantesProdem.PrntProdemVegetativa,
                Medifecha = regFecha
            };
            this.DeletePrnMedicion48(idArea, ConstantesProdem.PrntProdemVegetativa, regFecha);
            this.SavePrnMedicion48(entity, refVegetativa.ToArray());

            //Registra el pronóstico de la demanda industrial
            entity = new PrnMedicion48DTO
            {
                Ptomedicodi = idArea,
                Prnm48tipo = ConstantesProdem.PrntProdemIndustrial,
                Medifecha = regFecha
            };
            this.DeletePrnMedicion48(idArea, ConstantesProdem.PrntProdemIndustrial, regFecha);
            this.SavePrnMedicion48(entity, refIndustrial.ToArray());

            return datosPronostico;
        }

        /// <summary>
        /// Proceso de cálculo del pronóstico de la demanda
        /// </summary>
        /// <param name="dataVegetativa">Información de la demanda vegetativa</param>
        /// <param name="dataIndustrial">Información de la demanda industrial</param>
        /// <param name="numDias">Número de dias de referencia para los datos</param>
        /// <param name="ruleDias">Valor dependiente del número de días</param>
        /// <param name="tFinal">Tiempo para el item correspondiente al pronostico de 
        /// la demanda a calcular (valor necesario para calcular los valores alfa y beta)</param>
        /// <param name="refVegetativa">Datos finales del pronóstico vegetativo</param>
        /// <param name="refIndustrial">Datos finales del pronóstico industrial</param>
        /// <returns></returns>
        public decimal[] CalcularPronosticoPorArea(List<decimal[]> dataVegetativa, List<decimal[]> dataIndustrial,
            int numDias, List<int> ruleDias, int tFinal, ref List<decimal> refVegetativa, ref List<decimal> refIndustrial)
        {
            decimal[] datos = new decimal[ConstantesProdem.Itv30min];

            #region Depuración de datos historicos
            //Obtención de la lista de Medianas                
            List<decimal> Mediana = new List<decimal>();
            List<decimal> AuxMediana = new List<decimal>();

            List<decimal[]> DemVegetativaHistorica = dataVegetativa;
            List<decimal[]> DemVegetativaHistoricaFiltrada = new List<decimal[]>();

            //Inicia los arreglos
            for (int i = 0; i < numDias; i++)
            {
                DemVegetativaHistoricaFiltrada.Add(new decimal[ConstantesProdem.Itv30min]);
            }

            for (int i = 0; i < ConstantesProdem.Itv30min; i++)
            {
                //Almacena las mediciones de los 5 dias, los ordena de menor a mayor y los agrega a la lista de medianas    
                AuxMediana = new List<decimal>();
                for (int j = 0; j < numDias; j++)
                {
                    AuxMediana.Add(DemVegetativaHistorica[j][i]);
                }
                AuxMediana = AuxMediana.OrderBy(x => x).ToList();
                Mediana.Add(AuxMediana[2]);
            }
            //--------------------------------------------------------------------------------------------------------
            //Filtrado de los datos historicos (+/-5%)   
            decimal SumIntervalo = 0;
            for (int i = 0; i < ConstantesProdem.Itv30min; i++)
            {
                SumIntervalo = 0;
                //Suma las mediciones de los 5 dias por cada intervalo
                for (int j = 0; j < numDias; j++)
                {
                    SumIntervalo += DemVegetativaHistorica[j][i];
                }
                //Verifica que se encuentre dentro del rango
                for (int k = 0; k < numDias; k++)
                {
                    if (DemVegetativaHistorica[k][i] > Mediana[i] * (1 + 5 / 100))//Supera el Máximo
                    {
                        DemVegetativaHistorica[k][i] = Math.Round((SumIntervalo - DemVegetativaHistorica[k][i]) / 4, 2);
                    }
                    else
                    {
                        if (DemVegetativaHistorica[k][i] < Mediana[i] * (1 - 5 / 100))//Inferior al Mínimo
                        {
                            DemVegetativaHistorica[k][i] = Math.Round((SumIntervalo - DemVegetativaHistorica[k][i]) / 4, 2);
                        }
                    }

                    DemVegetativaHistoricaFiltrada[k][i] = DemVegetativaHistorica[k][i];//!!
                }
            }

            //Guarda la Demanda Vegetativa Filtrada
            //DemVegetativaHistoricaFiltrada = DemVegetativaHistorica;
            #endregion

            #region Cálculo del perfil diario de la demanda
            //a. Valores Mínimos y Máximos para cada dia historico
            //   Mínimo para el Periodo Base
            //   Máximo para el Periodo Media
            //   Máximo para el Periodo Punta
            decimal[] PeridBaseMin = new decimal[numDias];
            decimal[] PeridMediaMax = new decimal[numDias];
            decimal[] PeridPuntaMax = new decimal[numDias];

            for (int i = 0; i < numDias; i++)
            {
                //Obtención de la lista del mínimo valor para cada día (Periodo base)
                PeridBaseMin[i] = DemVegetativaHistorica[i][0];
                for (int j = 0; j < 15; j++)
                {
                    if (PeridBaseMin[i] > DemVegetativaHistorica[i][j])
                    {
                        PeridBaseMin[i] = DemVegetativaHistorica[i][j];
                    }
                }
                //Obtención de la lista del máximo valor para cada día (Periodo media)
                PeridMediaMax[i] = DemVegetativaHistorica[i][15];
                for (int j = 15; j < 35; j++)
                {
                    if (PeridMediaMax[i] < DemVegetativaHistorica[i][j])
                    {
                        PeridMediaMax[i] = DemVegetativaHistorica[i][j];
                    }
                }
                //Obtención de la lista del máximo valor para cada día (Periodo punta)
                PeridPuntaMax[i] = DemVegetativaHistorica[i][35];
                for (int j = 35; j < 48; j++)
                {
                    if (PeridPuntaMax[i] < DemVegetativaHistorica[i][j])
                    {
                        PeridPuntaMax[i] = DemVegetativaHistorica[i][j];
                    }
                }
            }
            //b. Cálculo de los valores por unidad
            for (int i = 0; i < numDias; i++)
            {
                //Periodo Base - División por el menor valor
                for (int j = 0; j < 15; j++)
                {
                    if (PeridBaseMin[i] != 0)
                    {
                        DemVegetativaHistorica[i][j] = Math.Round(DemVegetativaHistorica[i][j] / PeridBaseMin[i], 2);
                    }
                }
                //Periodo Media - División por el mayor valor
                for (int j = 15; j < 35; j++)
                {
                    if (PeridMediaMax[i] != 0)
                    {
                        DemVegetativaHistorica[i][j] = Math.Round(DemVegetativaHistorica[i][j] / PeridMediaMax[i], 2);
                    }
                }
                //Periodo Punta - División por el mayor valor
                for (int j = 35; j < 48; j++)
                {
                    if (PeridPuntaMax[i] != 0)
                    {
                        DemVegetativaHistorica[i][j] = Math.Round(DemVegetativaHistorica[i][j] / PeridPuntaMax[i], 2);
                    }
                }
            }
            //c. Cálculo de la mediana de los valores por unidad
            List<decimal> ListaMedianaXUnidad = new List<decimal>();
            Mediana = new List<decimal>();
            AuxMediana = new List<decimal>();
            for (int i = 0; i < ConstantesProdem.Itv30min; i++)
            {
                AuxMediana = new List<decimal>();
                //Almacena las mediciones de los 5 dias, los ordena de menor a mayor y los agrega a la lista de medianas                 
                for (int j = 0; j < numDias; j++)
                {
                    AuxMediana.Add(DemVegetativaHistorica[j][i]);
                }
                AuxMediana = AuxMediana.OrderBy(x => x).ToList();
                Mediana.Add(AuxMediana[2]);
            }
            //c-1. Validación de la mediana
            decimal EsUnidad = 0;
            //Periodo Base
            AuxMediana = new List<decimal>();
            AuxMediana = Mediana.GetRange(0, 14);
            EsUnidad = Math.Round(AuxMediana.Min(x => x), 2);
            if (EsUnidad != 0)
            {
                while (EsUnidad != 1)
                {
                    for (int i = 0; i < 15; i++)
                    {
                        Mediana[i] = Math.Round(Mediana[i] / EsUnidad, 2);
                    }
                    //Reemplaza la lista
                    AuxMediana = Mediana.GetRange(0, 14);

                    //Original: Se busca que el mínimo valor sea 1
                    //20220421 - Se modifica el mínimo a máximo para evitar blucle sin salida
                    EsUnidad = Math.Round(AuxMediana.Max(x => x), 2);
                }
            }

            //Periodo Media
            AuxMediana = new List<decimal>();
            AuxMediana = Mediana.GetRange(15, 20);
            EsUnidad = Math.Round(AuxMediana.Max(x => x), 2);
            if (EsUnidad != 0)
            {
                while (EsUnidad != 1)
                {
                    for (int i = 15; i < 35; i++)
                    {
                        Mediana[i] = Math.Round(Mediana[i] / EsUnidad, 2);
                    }
                    //Reemplaza la lista
                    AuxMediana = Mediana.GetRange(15, 20);
                    EsUnidad = Math.Round(AuxMediana.Max(x => x), 2);
                }
            }

            //Periodo Punta
            AuxMediana = new List<decimal>();
            AuxMediana = Mediana.GetRange(35, 13);
            EsUnidad = Math.Round(AuxMediana.Max(x => x), 2);
            if (EsUnidad != 0)
            {
                while (EsUnidad != 1)
                {
                    for (int i = 35; i < 48; i++)
                    {
                        Mediana[i] = Math.Round(Mediana[i] / EsUnidad, 2);
                    }
                    //Reemplaza la lista
                    AuxMediana = Mediana.GetRange(35, 13);
                    EsUnidad = Math.Round(AuxMediana.Max(x => x), 2);
                }
            }

            //Guarda la lista de medianas por unidad
            ListaMedianaXUnidad = Mediana;
            #endregion

            #region Pronóstico de la potencia mínima y potencias máximas
            decimal ValAlfa = 0;
            decimal ValBeta = 0;
            int SumTiempo = 0;
            int SumTiempoXTiempo = 0;
            decimal SumPotencia = 0;
            decimal SumTiempoXPotencia = 0;

            List<int> ListaCantDias = new List<int>();
            List<decimal> ListaAlfa = new List<decimal>();
            List<decimal> ListaBeta = new List<decimal>();

            //Obtención de la sumatoria de dias y de sus cuadrados
            //Validando
            decimal nz = 0;
            ListaCantDias = ruleDias;
            SumTiempo = ruleDias.Sum();//Sumatoria de los dias

            foreach (int c in ruleDias)
            {
                SumTiempoXTiempo += c * c;
            }

            //Obtención de las variables Alfa y Beta para cada intervalo de medición
            for (int i = 0; i < ConstantesProdem.Itv30min; i++)
            {
                //Obtiene la Sumatoria de Potencias y de Potencias x Tiempo(Diferencia entre fechas)
                ValAlfa = 0;
                ValBeta = 0;
                SumPotencia = 0;
                SumTiempoXPotencia = 0;
                for (int j = 0; j < numDias; j++)
                {
                    SumPotencia += DemVegetativaHistoricaFiltrada[j][i];
                    SumTiempoXPotencia += DemVegetativaHistoricaFiltrada[j][i] * ListaCantDias[j];
                }
                //Validacion
                nz = ((5 * SumTiempoXTiempo) - (SumTiempo * SumTiempo));
                if (nz == 0)
                {
                    nz = 1;
                }
                //Cálculo de Alfa                   
                ValAlfa = ((5 * SumTiempoXPotencia) - (SumTiempo * SumPotencia)) / nz;
                //Cálculo de Beta
                ValBeta = ((SumTiempoXTiempo * SumPotencia) - (SumTiempo * SumTiempoXPotencia)) / nz;

                ListaAlfa.Add(ValAlfa);
                ListaBeta.Add(ValBeta);
            }
            //Cálculo de la demanda del día a Pronosticar
            decimal dp = 0;
            List<decimal> PrevDemPronosticada = new List<decimal>();

            for (int i = 0; i < ConstantesProdem.Itv30min; i++)
            {
                dp = 0;
                dp = ListaAlfa[i] * tFinal + ListaBeta[i];
                PrevDemPronosticada.Add(dp);
            }
            //Validación de la demanda del día a Pronosticar
            //Si la Demanda Pronosticada es menor al promedio de los historicos
            for (int i = 0; i < ConstantesProdem.Itv30min; i++)
            {
                dp = 0;
                for (int j = 0; j < numDias; j++)
                {
                    dp += DemVegetativaHistoricaFiltrada[j][i];
                }
                dp = dp / numDias;
                if (PrevDemPronosticada[i] < dp)
                {
                    PrevDemPronosticada[i] = dp;
                }
            }
            #endregion

            #region Pronóstico de la demanda
            //--------------------------------------------------------------------------------------------------------
            //Para la Demanda Vegetativa
            List<decimal> ProdemVegetativa = new List<decimal>();//Demanda Vegetativa
            List<decimal> AuxPrevDemPronosticada = new List<decimal>();//Auxiliar para los rangos de los periodos
                                                                       //a.Periodo Base
            dp = 0;
            AuxPrevDemPronosticada = PrevDemPronosticada.GetRange(0, 14);
            dp = AuxPrevDemPronosticada.Min(x => x);
            //a-1. Mediana Por Unidad * Minimo valor pronosticado
            for (int i = 0; i < 15; i++)
            {
                ProdemVegetativa.Add(dp * ListaMedianaXUnidad[i]);
            }
            //b.Periodo Media
            dp = 0;
            AuxPrevDemPronosticada = new List<decimal>();
            AuxPrevDemPronosticada = PrevDemPronosticada.GetRange(15, 20);
            dp = AuxPrevDemPronosticada.Max(x => x);
            //b-1. Mediana Por Unidad * Maximo valor pronosticado
            for (int i = 15; i < 35; i++)
            {
                ProdemVegetativa.Add(dp * ListaMedianaXUnidad[i]);
            }
            //c.Periodo Media
            dp = 0;
            AuxPrevDemPronosticada = new List<decimal>();
            AuxPrevDemPronosticada = PrevDemPronosticada.GetRange(35, 13);
            dp = AuxPrevDemPronosticada.Max(x => x);
            //c-1. Mediana Por Unidad * Maximo valor pronosticado
            for (int i = 35; i < 48; i++)
            {
                ProdemVegetativa.Add(dp * ListaMedianaXUnidad[i]);
            }
            //--------------------------------------------------------------------------------------------------------
            //Para la demanda de Usuarios Libres
            Mediana = new List<decimal>();
            AuxMediana = new List<decimal>();
            List<decimal> ProdemIndustrial = new List<decimal>();
            for (int i = 0; i < ConstantesProdem.Itv30min; i++)
            {
                //Almacena las mediciones de los dias, los ordena de menor a mayor y los agrega a la lista de medianas                 
                for (int j = 0; j < numDias; j++)
                {
                    AuxMediana.Add(dataIndustrial[j][i]);
                }
                AuxMediana = AuxMediana.OrderBy(x => x).ToList();
                Mediana.Add(AuxMediana[2]);
            }
            ProdemIndustrial = Mediana;
            //--------------------------------------------------------------------------------------------------------
            //Demanda Pronosticada Total del Area
            for (int i = 0; i < ConstantesProdem.Itv30min; i++)
            {
                datos[i] = ProdemVegetativa[i] + ProdemIndustrial[i];
            }
            #endregion

            //Asigna los resultados finales
            refVegetativa = ProdemVegetativa;
            refIndustrial = ProdemIndustrial;

            return datos;
        }

        #endregion

        #region Métodos del Módulo de Pronóstico - Pronóstico por Barras
        /// <summary>
        /// Permite obtener las mediciones de los ajustes las barras de otras agrupaciones.
        /// </summary>
        /// <param name="agrupacion">Identificador de la PRN_RELACIONTNA</param>
        /// <param name="tipo">Identificador del tipo de informacion</param>
        /// <param name="fecha">Fecha del registro</param>
        /// <param name="version">Identificador de la versión</param>
        /// <returns></returns>
        public PrnMediciongrpDTO GetMedicionBarrasOtraAgrupacion(int agrupacion, int tipo, string fecha, int version)
        {
            return FactorySic.GetPrnMediciongrpRepository().GetMedicionBarrasOtraAgrupacion(agrupacion, tipo, fecha, version);
        }

        //Assetec 20220224
        /// <summary>
        /// Permite obtener las madiciones de los ajustes las barras por agrupoacion, tipo, fecha, version.
        /// </summary>
        /// <param name="agrupacion">Identificador de la PRN_RELACIONTNA</param>
        /// <param name="tipo">Identificador del tipo de informacion</param>
        /// <param name="fecha">Fecha del registro</param>
        /// <param name="version">Identificador de la versión</param>
        /// <returns></returns>
        public List<PrnMediciongrpDTO> GetMedicionAgrupacionByBarra(int agrupacion, int tipo, string fecha, int version)
        {
            return FactorySic.GetPrnMediciongrpRepository().GetMedicionAgrupacionByBarra(agrupacion, tipo, fecha, version);
        }

        //Assetec 20220224
        /// <summary>
        /// Permite obtener las madiciones de las barras sumadas por agrupoacion, tipo, fecha, version.
        /// </summary>
        /// <param name="agrupacion">Identificador de la PRN_RELACIONTNA</param>
        /// <param name="tipo">Identificador del tipo de informacion</param>
        /// <param name="fecha">Fecha del registro</param>
        /// <param name="version">Identificador de la versión</param>
        /// <returns></returns>
        public PrnMediciongrpDTO GetMedicionByAgrupacion(int agrupacion, int tipo, string fecha, int version)
        {
            return FactorySic.GetPrnMediciongrpRepository().GetMedicionByAgrupacion(agrupacion, tipo, fecha, version);
        }

        /// <summary>
        /// Permite obtener las madiciones de los ajuste barras sumadas por agrupoacion, tipo, fecha, version.
        /// </summary>
        /// <param name="agrupacion">Identificador de la PRN_RELACIONTNA</param>
        /// <param name="tipo">Identificador del tipo de informacion</param>
        /// <param name="fecha">Fecha del registro</param>
        /// <param name="version">Identificador de la versión</param>
        /// <returns></returns>
        public PrnMediciongrpDTO GetMedicionByAgrupacionAjuste(int agrupacion, int tipo, string fecha, int version)
        {
            return FactorySic.GetPrnMediciongrpRepository().GetMedicionByAgrupacionAjuste(agrupacion, tipo, fecha, version);
        }

        /// <summary>
        /// Permite obtener las madiciones de los ajuste de la barra que pertenece a una agrupacion por tipo, fecha, version.
        /// </summary>
        /// <param name="barra">Identificador de la PRN_RELACIONTNA</param>
        /// <param name="tipo">Identificador del tipo de informacion</param>
        /// <param name="fecha">Fecha del registro</param>
        /// <param name="version">Identificador de la versión</param>
        /// <returns></returns>
        public PrnMediciongrpDTO GetMedicionByBarraAjuste(int barra, int tipo, string fecha, int version)
        {
            return FactorySic.GetPrnMediciongrpRepository().GetMedicionByBarraAjuste(barra, tipo, fecha, version);
        }

        /// <summary>
        /// Refresca las columnas de la grilla y la grafica segun se seleccione fechas en los calendarios(en el grupo de 7)
        /// </summary>
        /// <param name="idBarra">Identificador de la tabla PR_GRUPO</param>
        /// <param name="idAgrupacion">Identificador de la tabla PRN_RELACIONTNA</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="idVersion">Identificador de la versión</param>
        /// <param name="grafica">indica el tipo de informacion que se mostrara en la grafica</param>
        /// <param name="flag">flag para saber si se selecciono barra o agrupacion</param>
        /// <returns></returns>
        public object ActualizacionMedicionPronosticoBarraByCalendario(int idBarra, int idAgrupacion, string regFecha, int idVersion, string grafica, int flag)
        {
            object entity = new object();
            DateTime parseFecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            //Datos del pronóstico
            int tipoData = 0;
            switch (grafica)
            {
                case "total":
                    {
                        tipoData = ConstantesProdem.PrnmgrtProdemBarra;
                    }
                    break;
                case "vegetativa":
                    {
                        tipoData = ConstantesProdem.PrnmgrtDemVegetativa;
                    }
                    break;
                default:
                    {
                        tipoData = ConstantesProdem.PrnmgrtDemIndustrial;
                    }
                    break;
            }
            decimal[] tempPronosticoHt = (flag == 1) ? UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                        this.GetByIdPrnMediciongrp(idBarra, ConstantesProdem.PrnmgrtProdemBarra,
                                                        parseFecha, idVersion)) :
                                                       UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                        this.GetMedicionByAgrupacion(idAgrupacion, ConstantesProdem.PrnmgrtProdemBarra,
                                                        regFecha, idVersion));

            decimal[] tempPronosticoHc = (flag == 1) ? UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                        this.GetByIdPrnMediciongrp(idBarra, tipoData,
                                                            parseFecha, idVersion)) :
                                                       UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                        this.GetMedicionByAgrupacion(idAgrupacion, tipoData,
                                                            regFecha, idVersion));

            //Datos del ajuste
            decimal[] tempAjusteBase = (flag == 1) ? UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                 this.GetByIdPrnMediciongrp(idBarra, ConstantesProdem.PrnmgrtProdemBarraAjuste,
                                                 parseFecha, idVersion)) :
                                              UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                 this.GetMedicionByAgrupacion(idAgrupacion, ConstantesProdem.PrnmgrtProdemBarraAjuste,
                                                 regFecha, idVersion));

            //Obteniendo los otros Ajuste, ajustes hechos a las agrupaciones.
            decimal[] dataAjusteAgrupacion = (flag == 1) ? UtilProdem.
                                                            ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                            this.GetMedicionByBarraAjuste(idBarra, ConstantesProdem.PrnmgrtProdemAgrupacionAjuste, regFecha, idVersion)) :
                                                            UtilProdem.
                                                            ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                            this.GetMedicionByAgrupacionAjuste(idAgrupacion, ConstantesProdem.PrnmgrtProdemAgrupacionAjuste, regFecha, idVersion));

            //Ajustes de barras que se encuentran fuera de la agrupacion
            decimal[] dataAjusteNoAgrupacion = UtilProdem.
                                                    ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                    this.GetMedicionBarrasOtraAgrupacion(idAgrupacion, ConstantesProdem.PrnmgrtProdemAgrupacionAjuste, regFecha, idVersion));

            //Ajustes por Areas
            decimal[] dataAjusteArea = (flag == 1) ? UtilProdem.
                                                        ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                        this.GetByIdPrnMediciongrp(idBarra, ConstantesProdem.PrnmgrtProdemAreaAjuste, parseFecha, idVersion)) :
                                                     UtilProdem.
                                                        ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                        this.GetMedicionByAgrupacion(idAgrupacion, ConstantesProdem.PrnmgrtProdemAreaAjuste, regFecha, idVersion));

            for (int j = 0; j < tempPronosticoHt.Length; j++)
            {
                tempPronosticoHt[j] += tempAjusteBase[j] + dataAjusteAgrupacion[j] + dataAjusteNoAgrupacion[j] + dataAjusteArea[j];
            }

            entity = new
            {
                id = parseFecha.ToString(ConstantesProdem.FormatoFecha),
                label = parseFecha.ToString(ConstantesProdem.FormatoFecha),
                dataht = tempPronosticoHt,
                datahc = (grafica == "total") ? tempPronosticoHt : tempPronosticoHc,
                htrender = "normal",
                hcrender = "normal"
            };

            return entity;
        }


        //Assetec 20220228
        /// <summary>
        /// General el modelo de datos del módulo
        /// </summary>
        /// <param name="idBarra">Identificador de la tabla PR_GRUPO</param>
        /// <param name="idAgrupacion">Identificador de la tabla PRN_RELACIONTNA</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="idVersion">Identificador de la versión</param>
        /// <param name="flag">flag para saber si se selecciono barra o agrupacion</param>
        /// <returns></returns>
        public object PronosticoPorBarrasDatos(int idBarra, int idAgrupacion, string regFecha, int idVersion, int flag)
        {
            bool isValid = false;
            string typeMsg = String.Empty;
            string dataMsg = String.Empty;
            List<object> data = new List<object>();
            DateTime parseFecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            object entity = new object();

            //Obtiene el pronóstico de la demanda calculado (Lista)
            decimal[] dataPronostico = (flag == 1) ? UtilProdem.
                                                    ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                    this.GetByIdPrnMediciongrp(idBarra, ConstantesProdem.PrnmgrtProdemBarra, parseFecha, idVersion)) :
                                                    UtilProdem.
                                                    ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                    this.GetMedicionByAgrupacion(idAgrupacion, ConstantesProdem.PrnmgrtProdemBarra, regFecha, idVersion));

            //Obtiene la información vegetativa de la barra
            decimal[] dataVegetativa = (flag == 1) ? UtilProdem.
                                                     ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                     this.GetByIdPrnMediciongrp(idBarra, ConstantesProdem.PrnmgrtDemVegetativa, parseFecha, idVersion)) :
                                                     UtilProdem.
                                                     ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                     this.GetMedicionByAgrupacion(idAgrupacion, ConstantesProdem.PrnmgrtDemVegetativa, regFecha, idVersion));

            //Obtiene la información industrial de la barra
            decimal[] dataIndustrial = (flag == 1) ? UtilProdem.
                                                    ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                    this.GetByIdPrnMediciongrp(idBarra, ConstantesProdem.PrnmgrtDemIndustrial, parseFecha, idVersion)) :
                                                    UtilProdem.
                                                    ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                    this.GetMedicionByAgrupacion(idAgrupacion, ConstantesProdem.PrnmgrtDemIndustrial, regFecha, idVersion));

            //Mensaje de validación
            if (dataPronostico.Sum() == 0)
            {
                typeMsg = ConstantesProdem.MsgWarning;
                dataMsg = "Es necesario ejecutar el pronóstico de la demanda para" +
                    " este día o los valores del pronóstico suman 0";
            }
            else
            {
                isValid = true;
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El pronóstico de la demanda ya ha sido ejecutado para este día";
            }

            decimal[] dataAjusteBase = (flag == 1) ? UtilProdem.
                                                ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                this.GetByIdPrnMediciongrp(idBarra, ConstantesProdem.PrnmgrtProdemBarraAjuste, parseFecha, idVersion)) :
                                                UtilProdem.
                                                ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                this.GetMedicionByAgrupacion(idAgrupacion, ConstantesProdem.PrnmgrtProdemBarraAjuste, regFecha, idVersion));

            //Obteniendo los otros Ajuste, ajustes hechos a las agrupaciones.
            decimal[] dataAjusteAgrupacion = (flag == 1) ? UtilProdem.
                                                            ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                            this.GetMedicionByBarraAjuste(idBarra, ConstantesProdem.PrnmgrtProdemAgrupacionAjuste, regFecha, idVersion)) :
                                                            UtilProdem.
                                                            ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                            this.GetMedicionByAgrupacionAjuste(idAgrupacion, ConstantesProdem.PrnmgrtProdemAgrupacionAjuste, regFecha, idVersion));

            //Ajustes de barras que se encuentran fuera de la agrupacion
            decimal[] dataAjusteNoAgrupacion = UtilProdem.
                                                    ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                    this.GetMedicionBarrasOtraAgrupacion(idAgrupacion, ConstantesProdem.PrnmgrtProdemAgrupacionAjuste, regFecha, idVersion));

            //Ajustes por Areas
            decimal[] dataAjusteArea = (flag == 1) ? UtilProdem.
                                                        ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                        this.GetByIdPrnMediciongrp(idBarra, ConstantesProdem.PrnmgrtProdemAreaAjuste, parseFecha, idVersion)) :
                                                        UtilProdem.
                                                        ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                        this.GetMedicionByAgrupacion(idAgrupacion, ConstantesProdem.PrnmgrtProdemAreaAjuste, regFecha, idVersion));

            //Obtiene el pronóstico de la demanda + el ajuste
            int x = 0;
            decimal[] dataFinal = new decimal[ConstantesProdem.Itv30min];
            while (x < ConstantesProdem.Itv30min)
            {
                dataFinal[x] = dataPronostico[x] + dataAjusteBase[x] + dataAjusteAgrupacion[x] + dataAjusteNoAgrupacion[x] + dataAjusteArea[x];
                x++;
            }

            //Si distribuye los difrentes ajuste a donde corresponden
            decimal[] dataPronosticoFinal = new decimal[ConstantesProdem.Itv30min];
            decimal[] dataAjusteFinal = new decimal[ConstantesProdem.Itv30min];
            if (flag == 1)
            {
                int c = 0;
                while (c < ConstantesProdem.Itv30min)
                {
                    dataPronosticoFinal[c] = dataPronostico[c] + dataAjusteAgrupacion[c] + dataAjusteArea[c];
                    dataAjusteFinal[c] = dataAjusteBase[c];
                    c++;
                }
            }
            else
            {
                int c = 0;
                while (c < ConstantesProdem.Itv30min)
                {
                    dataPronosticoFinal[c] = dataPronostico[c] + dataAjusteBase[c] + dataAjusteNoAgrupacion[c] + dataAjusteArea[c];
                    dataAjusteFinal[c] = dataAjusteAgrupacion[c];
                    c++;
                }
            }

            entity = new
            {
                id = "intervalos",
                label = "Hora",
                data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                htrender = "hora",
                hcrender = "categoria"
            };
            data.Add(entity);

            for (int i = 0; i < 7; i++)//modelo.mediciones.count
            {
                entity = new
                {
                    id = "med" + (i + 1).ToString(),
                    label = "No encontrado",
                    //labelFecha = new string[7],
                    data = new decimal[48],
                    htrender = "normal",
                    hcrender = "normal"
                };
                data.Add(entity);
            }

            entity = new
            {
                id = "base_vgt",
                label = parseFecha.ToString(ConstantesProdem.FormatoFecha) + "(Veg)",
                data = dataVegetativa,
                htrender = "normal",
                hcrender = "no"
            };
            data.Add(entity);

            entity = new
            {
                id = "base_idt",
                label = parseFecha.ToString(ConstantesProdem.FormatoFecha) + "(Ind)",
                data = dataIndustrial,
                htrender = "normal",
                hcrender = "no"
            };
            data.Add(entity);

            entity = new
            {
                id = "base",
                label = parseFecha.ToString(ConstantesProdem.FormatoFecha) + "(C)",
                data = dataPronosticoFinal,
                htrender = "normal",
                hcrender = "no"
            };
            data.Add(entity);

            entity = new
            {
                id = "ajuste",
                label = "Ajuste(A)",
                data = dataAjusteFinal,
                htrender = "edit",
                hcrender = "no"
            };
            data.Add(entity);

            entity = new
            {
                id = "final",
                label = parseFecha.ToString(ConstantesProdem.FormatoFecha) + "(C + A)",
                data = dataFinal,
                htrender = "final",
                hcrender = "final"
            };
            data.Add(entity);

            return new { data, typeMsg, dataMsg, isValid };
        }

        /// <summary>
        /// Permite calcular el pronóstico de la demanda diario o semanal
        /// </summary>
        /// <param name="regFecha">Fecha inicial de ejecución</param>
        /// <param name="numIteraciones">Número de días o semanas a calcular</param>
        /// <param name="idTipo">Identificador del tipo (Diario o semanal)</param>
        /// <param name="idBarra">Identificador de la barra CP (una o varias)</param>
        /// <param name="idMetodo">Identificador del método de cálculo para el pronóstico [S:Suavizado, P:Promedio]</param>
        /// <param name="idFuente">Identificador de la fuente de información</param>
        /// <param name="idVersion">Identificador de la versión a registrar</param>
        /// <param name="valNegativo">Valor de corrección para los valores negativos</param>
        /// <param name="nomUsuario">Nombre del usuario</param>
        public void PronosticoPorBarrasEjecutar(string regFecha,
            int numIteraciones, string idTipo, 
            string idBarra, string idMetodo, 
            string idFuente, int idVersion,
            decimal valNegativo, string nomUsuario)
        {
            int i = 0;
            decimal mError = 0.20M;
            int numDias = (idTipo == "D") ? numIteraciones : numIteraciones * 7;
            DateTime tempDate = new DateTime();
            List<PrnMediciongrpDTO> demandaVegetativaTna = new List<PrnMediciongrpDTO>();

            //regFecha es un valor de referencia se toma siempre un día despues
            DateTime parseDate = DateTime.ParseExact(regFecha,
                ConstantesProdem.FormatoFecha,
                CultureInfo.InvariantCulture)
                .AddDays(1);

            //Obtiene el modelo de la versión activa
            List<PrnVersionDTO> dataModelo = this.GetModeloActivo(idBarra);

            List<decimal[]> dataPronostico = new List<decimal[]>();
            List<decimal[]> dataVegetativa = new List<decimal[]>();
            List<decimal[]> dataIndustrial = new List<decimal[]>();

            if (dataModelo.Count != 0)
            {
                //Obtiene las barras cp del modelo
                List<int> barrasCP = dataModelo.
                    Select(x => x.Prnredbarracp).
                    Distinct().ToList();

                //Obtiene la conf. de un año para el modelo TNA
                List<PrnConfiguracionDiaDTO> configTnaAnio = this
                    .ObtenerCfgDiasHistoricosTNA(parseDate
                    .AddDays(numDias));
                
                while (i < numDias)
                {                    
                    tempDate = parseDate.AddDays(i);
                    
                    //Obtiene la configuración del día para el modelo TNA
                    PrnConfiguracionDiaDTO configTnaDia = configTnaAnio
                        .First(x => x.Cnfdiafecha
                        .Equals(tempDate));
                    
                    //Obtiene los días validos para la busqueda                   
                    List<DateTime> diasHistoricos = new List<DateTime>();
                    string[] priorTipoDia = new string[]
                    {
                        configTnaDia.Cnfdiaferiado,
                        configTnaDia.Cnfdiaatipico,
                        configTnaDia.Cnfdiaveda,
                    };

                    int t = Array.IndexOf(priorTipoDia, "S");
                    bool esDiaTipico = (t == -1) ? true : false;

                    if (t == -1)
                        diasHistoricos = configTnaAnio
                            .Where(x => x.Cnfdiaferiado
                            .Equals("N") && x.Cnfdiaatipico
                            .Equals("N") && x.Cnfdiaveda
                            .Equals("N"))
                            .Select(x => x.Cnfdiafecha)
                            .ToList();
                    if (t == 0)
                        diasHistoricos = configTnaAnio
                            .Where(x => x.Cnfdiaferiado.Equals("S"))
                            .Select(x => x.Cnfdiafecha)
                            .ToList();
                    if (t == 1)
                        diasHistoricos = configTnaAnio
                            .Where(x => x.Cnfdiaferiado
                            .Equals("N") && x.Cnfdiaatipico
                            .Equals("S") && x.Cnfdiaveda
                            .Equals("N"))
                            .Select(x => x.Cnfdiafecha)
                            .ToList();
                    if (t == 2)
                        diasHistoricos = configTnaAnio
                            .Where(x => x.Cnfdiaferiado
                            .Equals("N") && x.Cnfdiaatipico
                            .Equals("N") && x.Cnfdiaveda
                            .Equals("S"))
                            .Select(x => x.Cnfdiafecha)
                            .ToList();
                    
                    if (idFuente == ConstantesProdem.FuenteTna)
                    {
                        demandaVegetativaTna = new List<PrnMediciongrpDTO>();
                        demandaVegetativaTna = this.GetDemandaVegetativaHistoricaTNA(tempDate,
                            diasHistoricos,
                            esDiaTipico);
                    }

                    foreach (int cp in barrasCP)
                    {
                        //Obtiene las barras PM relacionadas a la barra CP
                        List<PrnVersionDTO> barrasPM = dataModelo.
                            Where(x => x.Prnredbarracp == cp).
                            Select(x => x).ToList();

                        //Si es una barra resultado de reducción
                        if (barrasPM.Where(x => x.Prnredtipo == "R").ToList().Count != 0)
                        {
                            barrasPM.Add(new PrnVersionDTO
                            {
                                Prnredbarrapm = cp,
                                Prnredgauss = 1,
                                Prnredperdida = 0
                            });
                        }

                        if (barrasPM.Count != 0)
                        {
                            dataPronostico = new List<decimal[]>();
                            dataVegetativa = new List<decimal[]>();
                            dataIndustrial = new List<decimal[]>();

                            foreach (PrnVersionDTO pm in barrasPM)
                            {
                                //Obtiene la configuración
                                PrnConfigbarraDTO dConfig = this.ParametrosBarrasGetConfiguracion(pm.Prnredbarrapm, tempDate);

                                //Obtiene los datos del pronóstico
                                List<decimal[]> data = this.GetPronosticoPorBarraPM(pm.Prnredbarrapm, cp,
                                    pm.Prnredgauss, pm.Prnredperdida,
                                    dConfig.Cfgbarpse ?? 0,
                                    dConfig.Cfgbarfactorf ?? 0,
                                    mError, tempDate, idFuente,
                                    diasHistoricos, esDiaTipico);

                                //Agrega los datos del pronostico de la barrra PM a la CP
                                dataPronostico.Add(data[0]);

                                //Agrega los datos vegetativos del pronostico de la barra PM a la CP
                                dataVegetativa.Add(data[1]);

                                //Agrega los datos industriales del pronostico de la barra PM a la CP
                                dataIndustrial.Add(data[2]);
                            }

                            //Obtiene el pronóstico de la barra CP
                            decimal[] finalPronostico = new decimal[ConstantesProdem.Itv30min];
                            foreach (decimal[] d in dataPronostico)
                            {
                                for (int x = 0; x < ConstantesProdem.Itv30min; x++)
                                {
                                    finalPronostico[x] += d[x];
                                }
                            }

                            //Suma los valores de la pérdida transversal
                            decimal[] dataPrdTransversal = this.ObtenerPerdidaPorBarraCP(cp, tempDate,
                                diasHistoricos,
                                esDiaTipico);
                            finalPronostico = finalPronostico
                                .Zip(dataPrdTransversal, (a, b) => a + b)
                                .ToArray();

                            //Obtiene los datos vegetativos de la barra CP
                            decimal[] finalVegetativa = new decimal[ConstantesProdem.Itv30min];
                            if (idFuente == ConstantesProdem.FuenteTna)
                            {
                                List<PrnMediciongrpDTO> vegTnaBarraCP = demandaVegetativaTna
                                    .Where(x => x.Grupocodi == cp)
                                    .ToList();
                                List<decimal[]> arrayMedicionesVgt = new List<decimal[]>();
                                foreach (PrnMediciongrpDTO v in vegTnaBarraCP)
                                {
                                    arrayMedicionesVgt.Add(UtilProdem
                                        .ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, v));
                                }

                                //Método por Promedio
                                if (idMetodo.Equals("P"))
                                {
                                    decimal[] promedioVegetativa = UtilProdem.CalcularPerfilPatron(
                                        arrayMedicionesVgt,
                                        arrayMedicionesVgt.Count,
                                        ConstantesProdem.Itv30min,
                                        ConstantesProdem.PatronByPromedio);
                                    finalVegetativa = promedioVegetativa;
                                }
                                //Método por Suavizado
                                if (idMetodo.Equals("S"))
                                    finalVegetativa = UtilProdem.SuavizadoExponencial(arrayMedicionesVgt, 1);

                                //Si la fuente es TNA se agrega la demanda vegetativa
                                //pues esta no se puede obtener a nivel de barras PM
                                finalPronostico = finalPronostico.Zip(finalVegetativa, (a, b) => a + b).ToArray();
                            }
                            if (idFuente == ConstantesProdem.FuentePr03)
                            {
                                foreach (decimal[] d in dataVegetativa)
                                {
                                    for (int x = 0; x < ConstantesProdem.Itv30min; x++)
                                    {
                                        finalVegetativa[x] += d[x];
                                    }
                                }
                            }

                            //Obtiene los datos industriales de la barra CP
                            decimal[] finalIndustrial = new decimal[ConstantesProdem.Itv30min];
                            foreach (decimal[] d in dataIndustrial)
                            {
                                for (int x = 0; x < ConstantesProdem.Itv30min; x++)
                                {
                                    finalIndustrial[x] += d[x];
                                }
                            }

                            //Valida si el pronóstico tiene intervalos con valores negativos
                            for (int x = 0; x < ConstantesProdem.Itv30min; x++)
                            {
                                finalPronostico[x] = (finalPronostico[x] < 0) 
                                    ? valNegativo
                                    : finalPronostico[x];
                            }

                            //Registra la información
                            //Pronostico
                            PrnMediciongrpDTO entity = new PrnMediciongrpDTO
                            {
                                Grupocodi = cp,
                                Prnmgrtipo = ConstantesProdem.PrnmgrtProdemBarra,
                                Medifecha = tempDate,
                                Vergrpcodi = idVersion,
                                Prnmgrusucreacion = nomUsuario,
                                Prnmgrfeccreacion = DateTime.Now,
                                Prnmgrusumodificacion = nomUsuario,
                                Prnmgrfecmodificacion = DateTime.Now
                            };

                            this.DeletePrnMediciongrp(entity.Grupocodi,
                                entity.Prnmgrtipo,
                                entity.Medifecha, 
                                entity.Vergrpcodi);
                            this.SavePrnMediciongrp(entity, finalPronostico);

                            //Vegetativa
                            entity = new PrnMediciongrpDTO
                            {
                                Grupocodi = cp,
                                Prnmgrtipo = ConstantesProdem.PrnmgrtDemVegetativa,
                                Medifecha = tempDate,
                                Vergrpcodi = idVersion,
                                Prnmgrusucreacion = nomUsuario,
                                Prnmgrfeccreacion = DateTime.Now,
                                Prnmgrusumodificacion = nomUsuario,
                                Prnmgrfecmodificacion = DateTime.Now
                            };

                            this.DeletePrnMediciongrp(entity.Grupocodi,
                                entity.Prnmgrtipo,
                                entity.Medifecha, 
                                entity.Vergrpcodi);
                            this.SavePrnMediciongrp(entity, finalVegetativa);

                            //Industrial
                            entity = new PrnMediciongrpDTO
                            {
                                Grupocodi = cp,
                                Prnmgrtipo = ConstantesProdem.PrnmgrtDemIndustrial,
                                Medifecha = tempDate,
                                Vergrpcodi = idVersion,
                                Prnmgrusucreacion = nomUsuario,
                                Prnmgrfeccreacion = DateTime.Now,
                                Prnmgrusumodificacion = nomUsuario,
                                Prnmgrfecmodificacion = DateTime.Now
                            };

                            this.DeletePrnMediciongrp(entity.Grupocodi,
                                entity.Prnmgrtipo,
                                entity.Medifecha, 
                                entity.Vergrpcodi);
                            this.SavePrnMediciongrp(entity, finalIndustrial);
                        }
                    }

                    i++;
                }
            }
        }

        /// <summary>
        /// Obtiene los datos historicos de la demanda vegetativa por barra CP del modelo TNA
        /// </summary>
        /// <param name="regFecha">Fecha de referencia para la consulta</param>
        /// <param name="rangoDias">Rango de días historicos para el modelo TNA</param>
        /// <param name="esDiaTipico">Flag que indica si se busca un día tipico o no</param>
        /// <returns></returns>
        public List<PrnMediciongrpDTO> GetDemandaVegetativaHistoricaTNA(DateTime regFecha, 
            List<DateTime> rangoDias,
            bool esDiaTipico)
        {            
            int diaSemana = (int)regFecha.DayOfWeek;
            DateTime diaBase = new DateTime(DateTime.Now.Year, 
                DateTime.Now.Month,
                DateTime.Now.Day,
                0, 0, 0);

            DateTime diaBusqueda = (regFecha < diaBase)
                ? UtilProdem.ObtenerUltimoDiaHistoricoValido(regFecha, 
                diaSemana, 
                rangoDias, 
                esDiaTipico)
                : UtilProdem.ObtenerUltimoDiaHistoricoValido(diaBase,
                diaSemana,
                rangoDias,
                esDiaTipico);
            
            List<DateTime> diasHistoricos = new List<DateTime> { diaBusqueda };
            diasHistoricos.AddRange(UtilProdem
                .ObtenerFechasHistoricasPorRango(diaBusqueda, 4, rangoDias, esDiaTipico));

            List<PrnMediciongrpDTO> entidades = new List<PrnMediciongrpDTO>();
            foreach (DateTime diaHistorico in diasHistoricos)
                entidades.AddRange(this.GetDemandaVegetativaModeloTNA(diaHistorico));

            return entidades;
        }

        /// <summary>
        /// Obtiene los componentes vegetativos de cada barra CP desde el modelo TNA (desagregado)
        /// </summary>
        /// <param name="regFecha">Fecha de busqueda</param>        
        /// <returns></returns>
        public List<PrnMediciongrpDTO> GetDemandaVegetativaModeloTNA(DateTime regFecha)
        {
            List<PrnMediciongrpDTO> entidades = new List<PrnMediciongrpDTO>();

            //Obtiene las relaciones registradas en el módulo de Barras CP-Tna
            //Diferencia los registros "anillo" y "radial"
            List<PrnRelacionTnaDTO> listaRelaciones = this.ListaRelacionTna();

            //Obtiene la demanda Vegetativa de las barras CP desde el modelo TNA
            List<PrnMediciongrpDTO> dataVegetativa = new List<PrnMediciongrpDTO>();
            foreach (PrnRelacionTnaDTO rRelacion in listaRelaciones)
            {
                //Componente vegetativo de toda la relación
                bool esAnillo = (rRelacion.Detalle.Count > 1) ? true : false;
                decimal[] componenteVegetativo = this.ObtenerMedicionesCalculadas(rRelacion.Reltnaformula, regFecha);

                //Obtiene el factor de aporte de cada barra CP perteneciente a la relación
                List<dynamic[]> dataBarrasCPRelacion = new List<dynamic[]>();
                List<decimal[]> vegetativaPorBarraCP = new List<decimal[]>();
                foreach (dynamic[] d in rRelacion.Detalle)
                {
                    int iBarra = d[0];
                    int iFormula = d[2];
                    decimal[] componenteVegetativoBarraCP = this.ObtenerMedicionesCalculadas(iFormula, regFecha);
                    vegetativaPorBarraCP.Add(componenteVegetativoBarraCP);
                    dataBarrasCPRelacion.Add(new dynamic[] { iBarra, componenteVegetativoBarraCP });
                }

                foreach (dynamic[] d in dataBarrasCPRelacion)
                {
                    PrnMediciongrpDTO entidadVegetativa = new PrnMediciongrpDTO
                    {
                        Grupocodi = d[0],
                        Medifecha = regFecha
                    };
                    decimal[] factorAporte = (esAnillo)
                        ? UtilProdem.ObtenerFactorAporte(d[1], vegetativaPorBarraCP)
                        : Enumerable.Repeat((decimal)1, ConstantesProdem.Itv30min).ToArray();

                    int j = 0;
                    while (j < ConstantesProdem.Itv30min)
                    {
                        //Dem.Veg.Pronosticada.BarraCP = Dem.Veg.Pronosticada.Anillo * FactorAporte
                        entidadVegetativa.GetType()
                            .GetProperty("H" + (j + 1))
                            .SetValue(entidadVegetativa,
                            Math.Round((componenteVegetativo[j] * factorAporte[j]), 4));
                        j++;
                    }
                    dataVegetativa.Add(entidadVegetativa);
                }
            }

            //Agrupa los diferentes componentes de cada barra CP
            List<int> barrasCP = dataVegetativa
                .Select(x => x.Grupocodi)
                .Distinct()
                .ToList();
            foreach (int barraCP in barrasCP)
            {
                PrnMediciongrpDTO entidad = new PrnMediciongrpDTO()
                {
                    Grupocodi = barraCP,
                    Medifecha = regFecha
                };
                List<PrnMediciongrpDTO> demandaPorBarra = dataVegetativa
                    .Where(x => x.Grupocodi == barraCP)
                    .ToList();
                decimal[] finalDemanda = new decimal[ConstantesProdem.Itv30min];
                foreach (PrnMediciongrpDTO d in demandaPorBarra)
                {
                    decimal[] arrayDemanda = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, d);
                    finalDemanda = finalDemanda.Zip(arrayDemanda, (a, b) => a + b).ToArray();
                }
                int i = 0;
                while (i < ConstantesProdem.Itv30min)
                {
                    entidad.GetType()
                        .GetProperty("H" + (i + 1))
                        .SetValue(entidad, finalDemanda[i]);
                    i++;
                }
                entidades.Add(entidad);
            }

            return entidades;
        }

        /// <summary>
        /// Registra el ajuste de la medición
        /// </summary>
        /// <param name="idBarra">Identificador de la tabla PR_GRUPO</param>
        /// <param name="idAgrupacion">Identificador de la tabla PRN_RELACIONTNA</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="idVersion">Identificador de la versión</param>
        /// <param name="dataMedicion">Datos del ajuste realizado al área</param>
        /// <param name="dataBase">Datos del ajuste realizado al área</param>
        /// <param name="nomUsuario">Nombre del usuario</param>
        /// <param name="flag">Indica si se trabaja con Barra (1) o Agrupacion(2)</param>
        /// <returns></returns>
        public object PronosticoPorBarrasSave(int idBarra, int idAgrupacion, string regFecha, int idVersion, PrnMediciongrpDTO dataMedicion,
            PrnMediciongrpDTO dataBase, string nomUsuario, int flag)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;

            PrnMediciongrpDTO entity = new PrnMediciongrpDTO();
            DateTime parseFecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            if (flag == 1)
            {
                entity = dataMedicion;
                entity.Grupocodi = idBarra;
                entity.Prnmgrtipo = ConstantesProdem.PrnmgrtProdemBarraAjuste;
                entity.Medifecha = parseFecha;
                entity.Vergrpcodi = idVersion;
                entity.Prnmgrpadre = ConstantesProdem.PrnmgrtProdemBarraSinPadre;
                entity.Meditotal = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, dataMedicion).Sum();

                //Validación
                PrnMediciongrpDTO isValid = this.GetByIdPrnMediciongrp(entity.Grupocodi,
                    entity.Prnmgrtipo, entity.Medifecha, entity.Vergrpcodi);

                if (isValid.Grupocodi != 0)
                {
                    entity.Prnmgrusumodificacion = nomUsuario;
                    entity.Prnmgrfecmodificacion = DateTime.Now;

                    this.UpdatePrnMediciongrp(entity);
                    typeMsg = ConstantesProdem.MsgSuccess;
                    dataMsg = "El registro se actualizó de manera exitosa";
                }
                else
                {
                    entity.Prnmgrusucreacion = nomUsuario;
                    entity.Prnmgrfeccreacion = DateTime.Now;
                    entity.Prnmgrusumodificacion = nomUsuario;
                    entity.Prnmgrfecmodificacion = DateTime.Now;

                    this.SavePrnMediciongrp(entity);
                    typeMsg = ConstantesProdem.MsgSuccess;
                    dataMsg = "El registro se realizó de manera exitosa";
                }
            }
            else
            {
                decimal[] barraAjusteActual = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, dataMedicion);
                decimal[] dataTotal = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, dataBase);
                List<PrnRelacionTnaDTO> lista = this.ListaRegistrosBarrasById(idAgrupacion);
                foreach (var item in lista)
                {
                    PrnMediciongrpDTO prnstico = this.GetByIdPrnMediciongrp(item.Barracodi, ConstantesProdem.PrnmgrtProdemBarra, parseFecha, idVersion);
                    if (prnstico == null)
                    {
                        typeMsg = ConstantesProdem.MsgWarning;
                        dataMsg = "No existe pronostico para las barras de esta agrupacion...";
                        return new { typeMsg, dataMsg };
                    }
                }
                foreach (var item in lista)
                {
                    decimal[] barraPronostico = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                    this.GetByIdPrnMediciongrp(item.Barracodi, ConstantesProdem.PrnmgrtProdemBarra, parseFecha, idVersion));

                    decimal[] barraAjusteBase = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                    this.GetByIdPrnMediciongrp(item.Barracodi, ConstantesProdem.PrnmgrtProdemBarraAjuste, parseFecha, idVersion));

                    decimal[] barraAjusteAgrupacion = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                        this.GetMedicionByBarraAjuste(item.Barracodi, ConstantesProdem.PrnmgrtProdemAgrupacionAjuste, regFecha, idVersion));

                    decimal[] dataAjusteArea = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                                this.GetByIdPrnMediciongrp(idBarra, ConstantesProdem.PrnmgrtProdemAreaAjuste, parseFecha, idVersion));

                    int x = 0;
                    decimal[] dataBarra = new decimal[ConstantesProdem.Itv30min];
                    while (x < ConstantesProdem.Itv30min)
                    {
                        decimal tParcial = barraPronostico[x] + barraAjusteBase[x] + dataAjusteArea[x];//+ barraAjusteAgrupacion[x] ;
                        decimal total = tParcial * (barraAjusteActual[x] / dataTotal[x]);
                        entity.GetType().GetProperty("H" + (x + 1)).SetValue(entity, total);
                        x++;
                    }

                    entity.Grupocodi = item.Barracodi;
                    entity.Prnmgrtipo = ConstantesProdem.PrnmgrtProdemAgrupacionAjuste;
                    entity.Medifecha = parseFecha;
                    entity.Vergrpcodi = idVersion;
                    entity.Prnmgrpadre = idAgrupacion;
                    entity.Meditotal = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, entity).Sum();
                    //Validación
                    PrnMediciongrpDTO isValid = this.GetByIdPrnMediciongrp(entity.Grupocodi,
                        entity.Prnmgrtipo, entity.Medifecha, entity.Vergrpcodi);

                    if (isValid.Grupocodi != 0)
                    {
                        entity.Prnmgrusumodificacion = nomUsuario;
                        entity.Prnmgrfecmodificacion = DateTime.Now;

                        this.UpdatePrnMediciongrp(entity);
                        typeMsg = ConstantesProdem.MsgSuccess;
                        dataMsg = "El registro se actualizó de manera exitosa";
                    }
                    else
                    {
                        entity.Prnmgrusucreacion = nomUsuario;
                        entity.Prnmgrfeccreacion = DateTime.Now;
                        entity.Prnmgrusumodificacion = nomUsuario;
                        entity.Prnmgrfecmodificacion = DateTime.Now;

                        this.SavePrnMediciongrp(entity);
                        typeMsg = ConstantesProdem.MsgSuccess;
                        dataMsg = "El registro se realizó de manera exitosa";
                    }
                }
            }

            return new { typeMsg, dataMsg };
        }

        /// <summary>
        /// Obtiene el pronóstico por barra PM
        /// </summary>
        /// <param name="idBarraPM">Identificador de la tabla PR_GRUPO corresponde a una BarraPM</param>
        /// <param name="idBarraCP">Identificador de la tabla PR_GRUPO corresponde a una BarraCP</param>
        /// <param name="dGauss">Valor del factor gauss asignado a la barra PM</param>
        /// <param name="dPerdida">Valor de la pérdida asignada a la barra PM</param>
        /// <param name="dAlfa">Valor "Alfa" para el procedimiento (Suavizado exponencial)</param>
        /// <param name="factorH">Factor "H" condicional</param>
        /// <param name="mError">Margen de error</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="idFuente">Identificador de la fuente de información</param>
        /// <param name="rangoDias">Rango de días historicos para el modelo TNA</param>
        /// <param name="esDiaTipico">Flag que indica si se busca un día tipico o no</param>
        /// <returns>
        /// index: 0, Pronóstico
        /// index: 1, Vegetativa
        /// index: 2, Industrial
        /// </returns>
        public List<decimal[]> GetPronosticoPorBarraPM(int idBarraPM, int idBarraCP,
            decimal dGauss, decimal dPerdida, decimal dAlfa,
            decimal factorH, decimal mError, DateTime regFecha,
            string idFuente, List<DateTime> rangoDias,
            bool esDiaTipico)
        {
            List<decimal[]> entitys = new List<decimal[]>();

            decimal[] data = new decimal[ConstantesProdem.Itv30min];

            PrnConfiguracionDTO config = new PrnConfiguracionDTO
            {
                Prncfgnumdiapatron = 5,
                Prncfgtipopatron = ConstantesProdem.PatronByPromedio
            };

            decimal[] dataVegetativa = new decimal[ConstantesProdem.Itv30min];
            if (idFuente == ConstantesProdem.FuentePr03)
            {
                //1. Obtiene la información Vegetativa(Distribuidores)
                PrnPatronModel demVegetativa = this.GetDataHistoricaBarraPM(idBarraPM,
                    dGauss,
                    dPerdida,
                    "dt", 
                    regFecha,
                    config);

                //1.1 Aplica el suavizado exponencial
                dataVegetativa = this.SuavizadoExponencial(demVegetativa.Mediciones,
                    demVegetativa.Patron,
                    dAlfa, 
                    factorH,
                    mError);
            }

            //2. Obtiene la información Industrial(Usuarios libres)            
            decimal[] dataIndustrial = this.PrioridadDemandaIndustrial(idBarraPM, 
                dGauss,
                dPerdida,
                regFecha);
            //3. Obtiene los SSAA de la barra PM
            decimal[] dataSSAA = this.ObtenerSSAAPorBarra(idBarraPM, 
                regFecha,
                rangoDias, 
                esDiaTipico);
            
            //4. Calcula el Pronóstico para la barra PM
            for (int i = 0; i < ConstantesProdem.Itv30min; i++)
            {
                data[i] = dataVegetativa[i] + dataIndustrial[i] + dataSSAA[i];
            }

            //5. Agrega los arreglos en order
            //5.1 Pronostico
            entitys.Add(data);

            //5.2 Vegetativa
            entitys.Add(dataVegetativa);

            //5.3 Industrial
            entitys.Add(dataIndustrial);

            return entitys;
        }

        /// <summary>
        /// Proceso de cálculo del pronóstico de la demanda
        /// </summary>
        /// <param name="dataMedicion">Datos a procesar</param>
        /// <param name="dataPatron">Información del perfíl patrón</param>
        /// <param name="dAlfa">Valor "Alfa" para el procedimiento</param>
        /// <param name="factorH">Factor "H" condicional</param>
        /// <param name="mError">Margen de error</param>
        /// <returns></returns>
        public decimal[] SuavizadoExponencial(List<decimal[]> dataMedicion, decimal[] dataPatron,
            decimal dAlfa, decimal factorH, decimal mError)
        {
            decimal[] datos = new decimal[ConstantesProdem.Itv30min];

            decimal a = 0;
            int i = 0, j = 0;
            //Recorrido por intervalo de tiempo
            while (i < ConstantesProdem.Itv30min)
            {
                //Reinicia el valor de alfa para el siguiente intervalo
                a = dAlfa;

                //Recorrido por cada medición
                j = 0;
                while (j < dataMedicion.Count)
                {
                    //Obtiene el valor del pronóstico para el intervalo
                    datos[i] += dataMedicion[j][i] * a;

                    //Cálcula el valor de alfa para la siguiente medición
                    a *= (1 - dAlfa);
                    j++;
                }

                //Para el ultimo valor del cálculo
                datos[i] += dataMedicion[dataMedicion.Count - 1][i] * (decimal)Math.Pow((double)(1 - dAlfa), dataMedicion.Count);

                //Valida si el nuevo valor no se encuentra dentro del rango permitido
                if (datos[i] > (dataPatron[i] * (1 + mError)) || datos[i] < (dataPatron[i] * (1 - mError)))
                {
                    datos[i] *= factorH;
                }
                i++;
            }

            return datos;
        }

        /// <summary>
        /// Devuelve la perdida transversal asignada a la barra CP
        /// </summary>
        /// <param name="idBarraCP">Identificador de la tabla PR_GRUPO</param>
        /// <returns></returns>
        public PrnPrdTransversalDTO GetPerdidaPorBarraCP(int idBarraCP)
        {
            return FactorySic.GetPrnPrdTransversalRepository().GetPerdidaPorBarraCP(idBarraCP).FirstOrDefault();
        }

        /// <summary>
        /// Permite duplicar una versión existente para una fecha especifica
        /// </summary>
        /// <param name="refVersion">Identificador de la versión original</param>
        /// <param name="refFecha">Fecha de la versión original consultada</param>
        /// <param name="nomVersion">Nombre de la nueva versión</param>
        /// <param name="regFecha">Fecha para los nuevos registros duplicados</param>
        /// <param name="flgActualizar">Flag que indica si es un nuevo registro o una actualización</param>
        public object PronosticoPorBarrasDuplicarVersion(int refVersion, string refFecha, string nomVersion,
            string regFecha, bool flgActualizar)
        {
            int idVersion = -1;
            string typeMsg = String.Empty;
            string dataMsg = String.Empty;

            if (flgActualizar)
            {
                idVersion = FactorySic.GetPrnVersiongrpReporsitory()
                    .GetByName(nomVersion)
                    .Vergrpcodi;
                FactorySic.GetPrnMediciongrpRepository().EliminarVersion(idVersion);
            }
            else
            {
                idVersion = this.ValidarVersionPronosticoPorBarras(nomVersion);
                if (idVersion == 0)
                {
                    typeMsg = "error";
                    dataMsg = "El nombre de la versión ya existe, ingrese uno nuevo";
                }
            }

            try
            {
                FactorySic.GetPrnMediciongrpRepository()
                    .PronosticoPorBarrasDuplicarVersion(refVersion, refFecha, idVersion, regFecha);
            }
            catch (Exception ex)
            {
                typeMsg = "error";
                dataMsg = ex.Message;
            }

            typeMsg = "success";
            dataMsg = (flgActualizar) ? "Se actualizó la versión duplicada" : "Se creo el duplicado";
            return new { typeMsg, dataMsg };
        }


        /// <summary>
        /// Valida la existencia de la descripción registrada
        /// </summary>
        /// <param name="nomVersion">Nombre de la versión</param>
        /// <returns>
        /// 0: La versión ya existe
        /// otro número: Nuevo identificador
        /// </returns>
        public int ValidarVersionPronosticoPorBarras(string nomVersion)
        {
            PrnVersiongrpDTO entity = this.GetByNamePrnVersiongrp(nomVersion);
            if (entity != null) return 0;//La versión ya existe
            else
            {
                //Crea una nueva versión y devuelve el id
                int id = this.SaveGetIdPrnVersiongrp(new PrnVersiongrpDTO { Vergrpnomb = nomVersion });
                return id;
            }
        }

        /// <summary>
        /// Devuelve la configuración de un año para el modelo TNA
        /// </summary>
        /// <param name="regFecha">Fecha de inicio para la busqueda</param>
        /// <returns></returns>
        public List<PrnConfiguracionDiaDTO> ObtenerCfgDiasHistoricosTNA(DateTime regFecha)
        {
            DateTime fechaLimite = regFecha.AddYears(-1);
            List<PrnConfiguracionDiaDTO> datos = this.CnfDiaObtenerPorRango(
                fechaLimite.ToString(ConstantesProdem.FormatoFecha),
                regFecha.ToString(ConstantesProdem.FormatoFecha));

            List<DateTime> soloFechas = datos
                .Select(x => x.Cnfdiafecha)
                .ToList();

            while (regFecha >= fechaLimite)
            {
                if (!soloFechas.Contains(regFecha))
                    datos.Add(new PrnConfiguracionDiaDTO()
                    {
                        Cnfdiafecha = regFecha,
                        Cnfdiaferiado = "N",
                        Cnfdiaatipico = "N",
                        Cnfdiaveda = "N",
                    });

                regFecha = regFecha.AddDays(-1);
            }

            datos = datos.OrderByDescending(x => x.Cnfdiafecha).ToList();
            return datos;
        }
        #endregion

        #region Métodos del Módulo de Pronóstico - Traslado de Carga
        /// <summary>
        /// Lista las versiones creadas en Prn_Mediciongrp
        /// </summary>
        /// <returns></returns>
        public List<PrnMediciongrpDTO> ListVersionMedicionGrp()
        {
            List<PrnMediciongrpDTO> entities = FactorySic.GetPrnPronosticoDemandaRepository().ListVersionMedicionGrp();
            foreach (PrnMediciongrpDTO e in entities)
            {
                if (e.Vergrpcodi == ConstantesProdem.VergrpcodiBase) e.Seleccionado = true;
            }

            return entities;
        }
        #endregion

        #region Métodos del Módulo Carga Pronóstico de Demanda - Extranet
        /// <summary>
        /// Metodo que se utiliza para traer la carga del pronostico de demanda - Barras 20220201
        /// </summary>
        /// <param name="formatcodi"></param>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public List<object> GetDataPronosticoDemandaByVersion(int formatcodi, DateTime fechaini, DateTime fechafin, int version)
        {
            List<Object> listaGenerica = new List<Object>();
            var lista = FactorySic.GetPrnPronosticoDemandaRepository().GetDataFormatoPronosticoDemandaByVersion(formatcodi, fechaini, fechafin, version);
            foreach (var reg in lista)
            {
                listaGenerica.Add(reg);
            }
            return listaGenerica;
        }
        #endregion

        #region Métodos del Módulo de Pronóstico - Demanda Histórica por Áreas
        /// <summary>
        /// General el modelo de datos del módulo Demanda Historica por Areas
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public object DemandaHistoricaDatos(int idArea, string regFecha)
        {
            List<object> data = new List<object>();
            DateTime parseFecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            //Obtiene los parámetros configurados
            PrnConfiguracionDTO regConfiguracion = this.ParametrosGetConfiguracion(idArea, ConstantesProdem.DefectoByArea, parseFecha);
            PrnPatronModel regPatron = this.GetPatronVegetativa(idArea, parseFecha);

            switch (idArea)
            {
                case ConstantesProdem.PtomedicodiASein:
                    {
                        #region Demanda del área total (SEIN)

                        #region Vegetativa total
                        //Obtiene los datos del Sur
                        List<PrnMedicion48DTO> dataSur = this.GetVegetativaPorArea(ConstantesProdem.PtomedicodiASur, 1, regFecha);
                        PrnMedicion48DTO sSur = dataSur.FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        decimal[] dSur = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sSur);

                        //Obtiene los datos del Norte
                        List<PrnMedicion48DTO> dataNorte = this.GetVegetativaPorArea(ConstantesProdem.PtomedicodiANorte, 1, regFecha);
                        PrnMedicion48DTO sNorte = dataNorte.FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        decimal[] dNorte = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sNorte);

                        //Obtiene los datos de la Sierra Centro
                        List<PrnMedicion48DTO> dataSCentro = this.GetVegetativaPorArea(ConstantesProdem.PtomedicodiASierraCentro, 1, regFecha);
                        PrnMedicion48DTO sSCentro = dataSCentro.FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        decimal[] dSCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sSCentro);

                        //Obtiene los datos del Centro
                        List<PrnMedicion48DTO> dataCentro = this.GetVegetativaPorArea(ConstantesProdem.PtomedicodiACentro, 1, regFecha);
                        PrnMedicion48DTO sCentro = dataCentro.FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        decimal[] dCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sCentro);

                        //Obtiene los datos del Sein
                        decimal[] dSein = new decimal[ConstantesProdem.Itv30min];
                        for (int i = 0; i < ConstantesProdem.Itv30min; i++)
                        {
                            dSein[i] = dSur[i] + dNorte[i] + dSCentro[i] + dCentro[i];
                        }
                        #endregion

                        #region Despacho ejecutado
                        //Obtiene los datos del Sur
                        List<PrnMedicion48DTO> despachoSur = this.GetDemandaPorArea(ConstantesProdem.PtomedicodiASur, 1, regFecha);
                        PrnMedicion48DTO dspSur = despachoSur.FirstOrDefault(x => x.Prnm48tipo == 1) ?? new PrnMedicion48DTO();
                        decimal[] dpSur = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, dspSur);

                        //Obtiene los datos del Norte
                        List<PrnMedicion48DTO> despachoNorte = this.GetDemandaPorArea(ConstantesProdem.PtomedicodiANorte, 1, regFecha);
                        PrnMedicion48DTO dspNorte = despachoNorte.FirstOrDefault(x => x.Prnm48tipo == 1) ?? new PrnMedicion48DTO();
                        decimal[] dpNorte = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, dspNorte);

                        //Obtiene los datos de la Sierra Centro
                        List<PrnMedicion48DTO> despachoSCentro = this.GetDemandaPorArea(ConstantesProdem.PtomedicodiASierraCentro, 1, regFecha);
                        PrnMedicion48DTO dspSCentro = despachoSCentro.FirstOrDefault(x => x.Prnm48tipo == 1) ?? new PrnMedicion48DTO();
                        decimal[] dpSCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, dspSCentro);

                        //Obtiene los datos del Centro
                        List<PrnMedicion48DTO> despachoCentro = this.GetDemandaPorArea(ConstantesProdem.PtomedicodiACentro, 1, regFecha);
                        PrnMedicion48DTO dspCentro = despachoCentro.FirstOrDefault(x => x.Prnm48tipo == 1) ?? new PrnMedicion48DTO();
                        decimal[] dpCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, dspCentro);

                        //Obtiene los datos del Sein
                        decimal[] dpSein = new decimal[ConstantesProdem.Itv30min];
                        for (int i = 0; i < ConstantesProdem.Itv30min; i++)
                        {
                            dpSein[i] = dpSur[i] + dpNorte[i] + dpSCentro[i] + dpCentro[i];
                        }
                        #endregion

                        #region Flujos
                        //Obtiene los datos de Flujo de Sur
                        List<PrnMedicion48DTO> flujoSur = this.GetDemandaPorArea(ConstantesProdem.PtomedicodiASur, 1, regFecha);
                        PrnMedicion48DTO fljSur = flujoSur.
                            FirstOrDefault(x => x.Prnm48tipo == ConstantesProdem.PrntFlujoLinea) ?? new PrnMedicion48DTO() { Ptomedidesc = "No encontrado" };
                        decimal[] fSur = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, fljSur);

                        //Obtiene los datos de Flujo de Norte
                        List<PrnMedicion48DTO> flujoNorte = this.GetDemandaPorArea(ConstantesProdem.PtomedicodiANorte, 1, regFecha);
                        PrnMedicion48DTO fljNorte = flujoNorte.
                            FirstOrDefault(x => x.Prnm48tipo == ConstantesProdem.PrntFlujoLinea) ?? new PrnMedicion48DTO() { Ptomedidesc = "No encontrado" };
                        decimal[] fNorte = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, fljNorte);

                        //Obtiene los datos de Flujo de Sierra Centro
                        List<PrnMedicion48DTO> flujoSCentro = this.GetDemandaPorArea(ConstantesProdem.PtomedicodiASierraCentro, 1, regFecha);
                        PrnMedicion48DTO fljSCentro = flujoSCentro.
                            FirstOrDefault(x => x.Prnm48tipo == ConstantesProdem.PrntFlujoLinea) ?? new PrnMedicion48DTO() { Ptomedidesc = "No encontrado" };
                        decimal[] fSCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, fljSCentro);

                        //Obtiene los datos de Flujo de Centro
                        List<PrnMedicion48DTO> flujoCentro = this.GetDemandaPorArea(ConstantesProdem.PtomedicodiACentro, 1, regFecha);
                        PrnMedicion48DTO fljCentro = flujoCentro.
                            FirstOrDefault(x => x.Prnm48tipo == ConstantesProdem.PrntFlujoLinea) ?? new PrnMedicion48DTO();
                        decimal[] fCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, fljCentro);

                        //Obtiene los datos del Sein
                        decimal[] fSein = new decimal[ConstantesProdem.Itv30min];
                        for (int i = 0; i < ConstantesProdem.Itv30min; i++)
                        {
                            fSein[i] = fSur[i] + fNorte[i] + fSCentro[i] + fCentro[i];
                        }
                        #endregion

                        #region Demanda Total
                        //Demanda del área Sur
                        List<PrnMedicion48DTO> demandaSur = this.GetVegetativaPorArea(ConstantesProdem.PtomedicodiASur, 1, regFecha);
                        PrnMedicion48DTO demSur = demandaSur.FirstOrDefault(x => x.Prnm48tipo == 1) ?? new PrnMedicion48DTO();
                        decimal[] deSur = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, demSur);

                        //Demanda del área Norte
                        List<PrnMedicion48DTO> demandaNorte = this.GetVegetativaPorArea(ConstantesProdem.PtomedicodiANorte, 1, regFecha);
                        PrnMedicion48DTO demNorte = demandaNorte.FirstOrDefault(x => x.Prnm48tipo == 1) ?? new PrnMedicion48DTO();
                        decimal[] deNorte = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, demNorte);

                        //Demanda del área Sierra Centro
                        List<PrnMedicion48DTO> demandaSCentro = this.GetVegetativaPorArea(ConstantesProdem.PtomedicodiASierraCentro, 1, regFecha);
                        PrnMedicion48DTO demSCentro = demandaSCentro.FirstOrDefault(x => x.Prnm48tipo == 1) ?? new PrnMedicion48DTO();
                        decimal[] deSCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, demSCentro);

                        //Demanda del área Centro
                        List<PrnMedicion48DTO> demandaCentro = this.GetVegetativaPorArea(ConstantesProdem.PtomedicodiACentro, 1, regFecha);
                        PrnMedicion48DTO demCentro = demandaCentro.FirstOrDefault(x => x.Prnm48tipo == 1) ?? new PrnMedicion48DTO();
                        decimal[] deCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, demCentro);

                        //Obtiene los datos del Sein
                        decimal[] deSein = new decimal[ConstantesProdem.Itv30min];
                        for (int i = 0; i < ConstantesProdem.Itv30min; i++)
                        {
                            deSein[i] = deSur[i] + deNorte[i] + deSCentro[i] + deCentro[i];
                        }
                        #endregion

                        #region Usuarios Libres

                        //Usuarios libres Sur
                        List<PrnMedicion48DTO> uLibresSur = this.GetVegetativaPorArea(ConstantesProdem.PtomedicodiASur, 1, regFecha);
                        PrnMedicion48DTO uLSur = uLibresSur.FirstOrDefault(x => x.Prnm48tipo == ConstantesProdem.PrntDemandaULibre) ?? new PrnMedicion48DTO();
                        decimal[] uSur = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, uLSur);

                        //Usuarios libres Norte
                        List<PrnMedicion48DTO> uLibresNorte = this.GetVegetativaPorArea(ConstantesProdem.PtomedicodiANorte, 1, regFecha);
                        PrnMedicion48DTO uLNorte = uLibresNorte.FirstOrDefault(x => x.Prnm48tipo == ConstantesProdem.PrntDemandaULibre) ?? new PrnMedicion48DTO();
                        decimal[] uNorte = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, uLNorte);

                        //Usuarios libres Sierra Centro
                        List<PrnMedicion48DTO> uLibresSCentro = this.GetVegetativaPorArea(ConstantesProdem.PtomedicodiASierraCentro, 1, regFecha);
                        PrnMedicion48DTO uLSCentro = uLibresSCentro.FirstOrDefault(x => x.Prnm48tipo == ConstantesProdem.PrntDemandaULibre) ?? new PrnMedicion48DTO();
                        decimal[] uSCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, uLSCentro);

                        //Usuarios libres Centro
                        List<PrnMedicion48DTO> uLibresCentro = this.GetVegetativaPorArea(ConstantesProdem.PtomedicodiACentro, 1, regFecha);
                        PrnMedicion48DTO uLCentro = uLibresCentro.FirstOrDefault(x => x.Prnm48tipo == ConstantesProdem.PrntDemandaULibre) ?? new PrnMedicion48DTO();
                        decimal[] uCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, uLCentro);

                        //Obtiene los datos del Sein
                        decimal[] uSein = new decimal[ConstantesProdem.Itv30min];
                        for (int i = 0; i < ConstantesProdem.Itv30min; i++)
                        {
                            uSein[i] = uSur[i] + uNorte[i] + uSCentro[i] + uCentro[i];
                        }
                        #endregion

                        object entity = new object();

                        entity = new
                        {
                            id = "intervalos",
                            label = "Hora",
                            data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                            htrender = "hora",
                            hcrender = "categoria"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "despacho",
                            label = "Despacho(D)",
                            data = dpSein,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "flujo",
                            label = "Flujos(FL)",
                            data = fSein,
                            htrender = "normal",
                            hcrender = "no"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "demanda",
                            label = "Demanda(D)",
                            data = deSein,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "usulib",
                            label = "ULibre(UL)",
                            data = uSein,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "sein",
                            label = "<label title='S + N + SC + C'>Vegetativa</label>",
                            data = dSein,
                            htrender = "final",
                            hcrender = "final_noedit"
                        };
                        data.Add(entity);

                        //Mediciones historicas
                        for (int i = 0; i < regPatron.Mediciones.Count; i++)
                        {
                            entity = new
                            {
                                id = "med" + (i + 1).ToString(),
                                label = regPatron.StrFechas[i],
                                data = regPatron.Mediciones[i],
                                htrender = "no",
                                hcrender = "no",
                                label2 = regPatron.StrFechasTarde[i],
                                slunes = regPatron.EsLunes
                            };
                            data.Add(entity);
                        }
                        #endregion
                    }
                    break;
                case ConstantesProdem.PtomedicodiACentro:
                    {
                        #region Demanda del área Centro

                        List<PrnMedicion48DTO> dataVegetativa = this.GetVegetativaPorArea(idArea, 1, regFecha);
                        List<PrnMedicion48DTO> dataDemanda = this.GetDemandaPorArea(idArea, 1, regFecha);

                        //Despacho ejecutado
                        PrnMedicion48DTO sDespacho = dataDemanda.FirstOrDefault(x => x.Prnm48tipo == 1) ?? new PrnMedicion48DTO();
                        decimal[] dDespacho = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sDespacho);

                        //Flujo
                        PrnMedicion48DTO sFlujo = dataDemanda.FirstOrDefault(x => x.Prnm48tipo == ConstantesProdem.PrntFlujoLinea) ?? new PrnMedicion48DTO();
                        decimal[] dFlujo = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sFlujo);

                        //Demanda del área
                        PrnMedicion48DTO sDemanda = dataVegetativa.FirstOrDefault(x => x.Prnm48tipo == 1) ?? new PrnMedicion48DTO();
                        decimal[] dDemanda = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sDemanda);

                        //Usuarios libres
                        PrnMedicion48DTO sULibres = dataVegetativa.FirstOrDefault(x => x.Prnm48tipo == ConstantesProdem.PrntDemandaULibre) ?? new PrnMedicion48DTO();
                        decimal[] dULibres = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sULibres);

                        //Vegetativa total
                        PrnMedicion48DTO sFinal = dataVegetativa.FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        decimal[] dFinal = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sFinal);

                        object entity = new object();

                        entity = new
                        {
                            id = "intervalos",
                            label = "Hora",
                            data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                            htrender = "hora",
                            hcrender = "categoria"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "despacho",
                            label = "Despacho(D)",
                            data = dDespacho,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "flujo",
                            label = "Flujos(FL)",
                            data = dFlujo,
                            htrender = "normal",
                            hcrender = "no"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "demanda",
                            label = "Demanda(D)",
                            data = dDemanda,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "usulib",
                            label = "ULibre(UL)",
                            data = dULibres,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "vegetativa",
                            label = "<label title='D - UL + aS + aN + aSC'>Vegetativa</label>",
                            data = dFinal,
                            htrender = "final",
                            hcrender = "final"
                        };
                        data.Add(entity);

                        //Mediciones historicas
                        for (int i = 0; i < regPatron.Mediciones.Count; i++)
                        {
                            entity = new
                            {
                                id = "med" + (i + 1).ToString(),
                                label = regPatron.StrFechas[i],
                                data = regPatron.Mediciones[i],
                                htrender = "no",
                                hcrender = "normal",
                                label2 = regPatron.StrFechasTarde[i],
                                slunes = regPatron.EsLunes
                            };
                            data.Add(entity);
                        }

                        entity = new
                        {
                            id = "patron",
                            label = "Patrón",
                            data = regPatron.Patron,
                            htrender = "no",
                            hcrender = "patron"
                        };
                        data.Add(entity);
                        #endregion
                    }
                    break;
                default:
                    {
                        #region Demanda de las áreas Sur, Norte y Sierra centro

                        List<PrnMedicion48DTO> dataVegetativa = this.GetVegetativaPorArea(idArea, 1, regFecha);
                        List<PrnMedicion48DTO> dataDemanda = this.GetDemandaPorArea(idArea, 1, regFecha);

                        //Despacho ejecutado
                        PrnMedicion48DTO sDespacho = dataDemanda.FirstOrDefault(x => x.Prnm48tipo == 1) ?? new PrnMedicion48DTO();
                        decimal[] dDespacho = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sDespacho);

                        //Flujo
                        PrnMedicion48DTO sFlujo = dataDemanda.
                            FirstOrDefault(x => x.Prnm48tipo == ConstantesProdem.PrntFlujoLinea) ?? new PrnMedicion48DTO() { Ptomedidesc = "No encontrado" };
                        decimal[] dFlujo = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sFlujo);
                        string flujoTitle = "<label title='" + sFlujo.Ptomedidesc + "'>Flujos(FL)</label>";

                        //Demanda del área
                        PrnMedicion48DTO sDemanda = dataVegetativa.FirstOrDefault(x => x.Prnm48tipo == 1) ?? new PrnMedicion48DTO();
                        decimal[] dDemanda = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sDemanda);

                        //Usuarios libres
                        PrnMedicion48DTO sULibres = dataVegetativa.FirstOrDefault(x => x.Prnm48tipo == ConstantesProdem.PrntDemandaULibre) ?? new PrnMedicion48DTO();
                        decimal[] dULibres = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sULibres);

                        //Vegetativa total
                        PrnMedicion48DTO sFinal = dataVegetativa.FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        decimal[] dFinal = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, sFinal);

                        object entity = new object();

                        entity = new
                        {
                            id = "intervalos",
                            label = "Hora",
                            data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                            htrender = "hora",
                            hcrender = "categoria"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "despacho",
                            label = "Despacho(D)",
                            data = dDespacho,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "flujo",
                            label = flujoTitle,
                            data = dFlujo,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "demanda",
                            label = "Demanda(D)",
                            data = dDemanda,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "usulib",
                            label = "ULibre(UL)",
                            data = dULibres,
                            htrender = "normal",
                            hcrender = "normal"
                        };
                        data.Add(entity);

                        entity = new
                        {
                            id = "vegetativa",
                            label = "<label title='D - UL'>Vegetativa</label>",
                            data = dFinal,
                            htrender = "final",
                            hcrender = "final"
                        };
                        data.Add(entity);

                        //Mediciones historicas
                        for (int i = 0; i < regPatron.Mediciones.Count; i++)
                        {
                            entity = new
                            {
                                id = "med" + (i + 1).ToString(),
                                label = regPatron.StrFechas[i],
                                data = regPatron.Mediciones[i],
                                htrender = "no",
                                hcrender = "normal",
                                label2 = regPatron.StrFechasTarde[i],
                                slunes = regPatron.EsLunes
                            };
                            data.Add(entity);
                        }

                        entity = new
                        {
                            id = "patron",
                            label = "Patrón",
                            data = regPatron.Patron,
                            htrender = "no",
                            hcrender = "patron"
                        };
                        data.Add(entity);
                        #endregion
                    }
                    break;
            }

            return new { data = data, cfg = regConfiguracion };
        }
        #endregion

        #region Métodos del Módulo de Pronóstico - Pronóstico por Generación

        /// <summary>
        /// General el modelo de datos del módulo
        /// </summary>
        /// <param name="idArea">Identificador del área operativa</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="selHistoricos">Lista de registros históricos</param>
        /// <returns></returns>
        public object PronosticoPorGeneracionDatos(int idArea, string regFecha, List<string> selHistoricos)
        {
            string typeMsg = String.Empty;
            string dataMsg = String.Empty;
            List<object> data = new List<object>();
            DateTime parseFecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            //Datos del despacho para el día seleccionado
            List<PrnMedicion48DTO> datosDemanda = this.GetDemandaPorArea(idArea, 1, regFecha);
            PrnMedicion48DTO eDemanda = datosDemanda
                .FirstOrDefault(x => x.Prnm48tipo == 0)
                ?? new PrnMedicion48DTO();
            decimal[] arrayDemanda = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, eDemanda);

            PrnMedicion48DTO eAjuste = datosDemanda
                .FirstOrDefault(x => x.Prnm48tipo == ConstantesProdem.PrntDemandaAreaAjuste)
                ?? new PrnMedicion48DTO();
            decimal[] arrayAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, eAjuste);

            decimal[] arrayBase = new decimal[ConstantesProdem.Itv30min];
            for (int i = 0; i < ConstantesProdem.Itv30min; i++)
            {
                arrayBase[i] = arrayDemanda[i] - arrayAjuste[i];
            }

            object entity = new object();
            entity = new
            {
                id = "intervalos",
                label = "Hora",
                data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                htrender = "hora",
                hcrender = "categoria"
            };
            data.Add(entity);

            entity = new
            {
                id = "base",
                label = "Despacho(D)",
                data = arrayBase,
                htrender = "normal",
                hcrender = "no"
            };
            data.Add(entity);

            entity = new
            {
                id = "ajuste",
                label = "Ajuste(A)",
                data = arrayAjuste,
                htrender = "edit",
                hcrender = "no"
            };
            data.Add(entity);

            entity = new
            {
                id = "final",
                label = "Demanda",
                data = arrayDemanda,
                htrender = "final",
                hcrender = "final"
            };
            data.Add(entity);

            //Datos del despacho de los días históricos seleccionados
            if (selHistoricos != null)
            {
                foreach (string entHistorico in selHistoricos)
                {
                    string[] entidad = entHistorico.Split(':');
                    string idEntidad = entidad[0];
                    string fechaEntidad = entidad[1];

                    List<PrnMedicion48DTO> datosHistoricos = this.GetDemandaPorArea(idArea, 1, fechaEntidad);
                    PrnMedicion48DTO dHistorico = datosHistoricos
                        .FirstOrDefault(x => x.Prnm48tipo == 0)
                        ?? new PrnMedicion48DTO();
                    decimal[] arrayHistorico = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, dHistorico);

                    entity = new
                    {
                        id = idEntidad,
                        label = fechaEntidad,
                        data = arrayHistorico,
                        htrender = "no",
                        hcrender = "normal"
                    };
                    data.Add(entity);
                }
            }

            return new { data, typeMsg, dataMsg };
        }

        /// <summary>
        /// Ejecuta el pronóstico de la demanda para todas las áreas operativas
        /// </summary>
        /// <param name="regFecha">Fecha a pronósticar</param>
        /// <param name="selHistoricos">Fechas históricas que se utilizarán en el pronóstico</param>
        /// <returns></returns>
        public object PronosticoPorGeneracionEjecutar(string regFecha, List<string> selHistoricos)
        {
            string typeMsg = String.Empty;
            string dataMsg = String.Empty;

            DateTime parseFecha = DateTime.ParseExact(regFecha,
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            int diasHistoricos = (selHistoricos != null) ? selHistoricos.Count : 0;
            if (diasHistoricos == 0)
            {
                typeMsg = ConstantesProdem.MsgError;
                dataMsg = "Debe seleccionar 5 días historicos para ejecutar el pronóstico";
                return new { typeMsg, dataMsg };
            }

            int valPronosticoBarras = this.ValidarEjecucionPronosticoPorBarras(regFecha);
            if (valPronosticoBarras == 0)
            {
                typeMsg = ConstantesProdem.MsgError;
                dataMsg = "No se ha ejecutado el pronóstico por barras para el día seleccionado, es necesario ejecutarlo...";
                return new { typeMsg, dataMsg };
            }

            List<int> areasOperativas = new List<int> {
                ConstantesProdem.PtomedicodiASur,
                ConstantesProdem.PtomedicodiANorte,
                ConstantesProdem.PtomedicodiASierraCentro,
                ConstantesProdem.PtomedicodiACentro
            };

            List<decimal[]> medSein = new List<decimal[]>();

            //Calcula y registra el pronóstico para las áreas operativas
            foreach (int idArea in areasOperativas)
            {
                List<decimal[]> medVegetativa = new List<decimal[]>();
                List<decimal[]> medIndustrial = new List<decimal[]>();

                //Obtiene los datos de la demanda vegetativa del área (ejecutada)
                foreach (string sHistorico in selHistoricos)
                {
                    string[] entidad = sHistorico.Split(':');
                    string fechaEntidad = entidad[1];

                    List<PrnMedicion48DTO> dataVegetativa = this.GetVegetativaPorArea(idArea, 1, fechaEntidad);

                    PrnMedicion48DTO eVegetativa = dataVegetativa
                        .FirstOrDefault(x => x.Prnm48tipo == 0)
                        ?? new PrnMedicion48DTO();
                    decimal[] dVegetativa = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, eVegetativa);
                    medVegetativa.Add(dVegetativa);

                    PrnMedicion48DTO eIndustrial = dataVegetativa
                        .FirstOrDefault(x => x.Prnm48tipo == ConstantesProdem.PrntDemandaULibre)
                        ?? new PrnMedicion48DTO();
                    decimal[] dIndustrial = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, eVegetativa);
                    medIndustrial.Add(dIndustrial);
                }

                //Obtiene los datos de la regla de dias para el pronóstico
                Tuple<int, List<int>> datosDias = UtilProdem.PronosticoReglaDias(parseFecha, diasHistoricos);

                //Calcula el pronóstico de la demanda
                List<decimal> refVegetativa = new List<decimal>();
                List<decimal> refIndustrial = new List<decimal>();

                decimal[] datosPronostico = this.CalcularPronosticoPorArea(medVegetativa, medIndustrial,
                    diasHistoricos, datosDias.Item2, datosDias.Item1, ref refVegetativa, ref refIndustrial);

                medSein.Add(datosPronostico);

                #region Registra el pronóstico en la BD
                PrnMedicion48DTO entity = new PrnMedicion48DTO
                {
                    Ptomedicodi = idArea,
                    Prnm48tipo = ConstantesProdem.PrntProdemArea,
                    Medifecha = parseFecha
                };
                this.DeletePrnMedicion48(idArea, ConstantesProdem.PrntProdemArea, parseFecha);
                this.SavePrnMedicion48(entity, datosPronostico);

                //Registra el pronóstico de la demanda vegetativa
                entity = new PrnMedicion48DTO
                {
                    Ptomedicodi = idArea,
                    Prnm48tipo = ConstantesProdem.PrntProdemVegetativa,
                    Medifecha = parseFecha
                };
                this.DeletePrnMedicion48(idArea, ConstantesProdem.PrntProdemVegetativa, parseFecha);
                this.SavePrnMedicion48(entity, refVegetativa.ToArray());

                //Registra el pronóstico de la demanda industrial
                entity = new PrnMedicion48DTO
                {
                    Ptomedicodi = idArea,
                    Prnm48tipo = ConstantesProdem.PrntProdemIndustrial,
                    Medifecha = parseFecha
                };
                this.DeletePrnMedicion48(idArea, ConstantesProdem.PrntProdemIndustrial, parseFecha);
                this.SavePrnMedicion48(entity, refIndustrial.ToArray());
                #endregion                
            }

            //Calcula y registra el pronóstico Sein
            decimal[] dataSein = new decimal[ConstantesProdem.Itv30min];
            for (int i = 0; i < ConstantesProdem.Itv30min; i++)
            {
                decimal v = 0;
                foreach (decimal[] m in medSein) v += m[i];
                dataSein[i] = v;
            }

            PrnMedicion48DTO entitySein = new PrnMedicion48DTO
            {
                Ptomedicodi = ConstantesProdem.PtomedicodiASein,
                Prnm48tipo = ConstantesProdem.PrntProdemArea,
                Medifecha = parseFecha
            };
            this.DeletePrnMedicion48(ConstantesProdem.PtomedicodiASein, ConstantesProdem.PrntProdemArea, parseFecha);
            this.SavePrnMedicion48(entitySein, dataSein);

            typeMsg = ConstantesProdem.MsgSuccess;
            dataMsg = "El pronóstico para el día " + regFecha + "se ejecuto correctamente";
            return new { typeMsg, dataMsg };
        }

        /// <summary>
        /// Actualiza una medición histórica del módulo de Pronóstico por generación
        /// </summary>
        /// <param name="idArea">Identificador del área operativa (ptomedición)</param>
        /// <param name="regFecha">Fecha de busqueda</param>
        /// <returns></returns>
        public decimal[] PronosticoPorGeneracionMedicion(int idArea, string regFecha)
        {
            decimal[] entity = new decimal[ConstantesProdem.Itv30min];
            PrnMedicion48DTO entM48 = this.GetDemandaPorArea(idArea, 1, regFecha).
                FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
            entity = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, entM48);
            return entity;
        }

        /// <summary>
        /// Registra el ajuste realizado a la demanda de un área operativa
        /// </summary>
        /// <param name="idArea">Identificador del área operativa</param>
        /// <param name="regFecha">Fecha para el registro</param>
        /// <param name="dataMedicion">Datos del ajuste realizado</param>
        /// <param name="nomUsuario">Nombre del usuario</param>
        /// <returns></returns>
        public object PronosticoPorGeneracionSave(int idArea, string regFecha, PrnMedicion48DTO dataMedicion, string nomUsuario)
        {
            string typeMsg = String.Empty;
            string dataMsg = String.Empty;

            //Copia los valores por intervalo (H1, H2, ...)
            PrnMedicion48DTO entity = dataMedicion;

            entity.Ptomedicodi = idArea;
            entity.Prnm48tipo = ConstantesProdem.PrntDemandaAreaAjuste;
            entity.Medifecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            //Validación
            PrnMedicion48DTO isValid = this.GetByIdPrnMedicion48(entity.Ptomedicodi,
                entity.Prnm48tipo, entity.Medifecha);

            if (isValid.Ptomedicodi != 0)
            {
                entity.Prnm48usumodificacion = nomUsuario;
                entity.Prnm48fecmodificacion = DateTime.Now;

                this.UpdatePrnMedicion48(entity);
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El registro se actualizó de manera exitosa";
            }
            else
            {
                entity.Prnm48usucreacion = nomUsuario;
                entity.Prnm48feccreacion = DateTime.Now;
                entity.Prnm48usumodificacion = nomUsuario;
                entity.Prnm48fecmodificacion = DateTime.Now;

                this.SavePrnMedicion48(entity);
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El registro se realizó de manera exitosa";
            }

            return new { typeMsg, dataMsg };
        }

        /// <summary>
        /// Valida si existe en la base de datos pronóstico por barras ejecutados el día consulta
        /// </summary>
        /// <param name="regFecha">Fecha de consulta</param>
        /// <returns></returns>
        public int ValidarEjecucionPronosticoPorBarras(string regFecha)
        {
            return FactorySic.GetPrnMediciongrpRepository().ValidarEjecucionPronostiPorBarras(regFecha);
        }
        #endregion

        #region Métodos del Módulo de Pronóstico - Pérdidas
        /// <summary>
        /// Devuelve el modelo de datos
        /// </summary>
        /// <param name="regFecha">Fecha de consulta</param>
        /// <returns></returns>
        public object PerdidasPR03Datos(string regFecha)
        {
            List<object> data = new List<object>();

            //Carga los datos del modelo
            object entity = new object();
            entity = new
            {
                id = "intervalos",
                label = "Hora",
                data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                htrender = "hora",
                hcrender = "categoria"
            };
            data.Add(entity);

            entity = new
            {
                id = "norte",
                label = "Area Norte",
                data = new decimal[ConstantesProdem.Itv30min],
                htrender = "edit",
                hcrender = "no"
            };
            data.Add(entity);

            entity = new
            {
                id = "sur",
                label = "Area Sur",
                data = new decimal[ConstantesProdem.Itv30min],
                htrender = "edit",
                hcrender = "no"
            };
            data.Add(entity);

            entity = new
            {
                id = "centro",
                label = "Area Centro",
                data = new decimal[ConstantesProdem.Itv30min],
                htrender = "edit",
                hcrender = "no"
            };
            data.Add(entity);

            entity = new
            {
                id = "scentro",
                label = "Area Sierra Centro",
                data = new decimal[ConstantesProdem.Itv30min],
                htrender = "edit",
                hcrender = "no"
            };
            data.Add(entity);

            entity = new
            {
                id = "recalculo",
                label = "RECALCULO(%)",
                data = new decimal[ConstantesProdem.Itv30min],
                htrender = "edit",
                hcrender = "no"
            };
            data.Add(entity);

            return new { data };
        }

        /// <summary>
        /// Refleja el pronóstico por áreas a pronóstico por barras
        /// </summary>
        /// <param name="perdidasNorte">% de perdidas del área norte (ingresado por el usuario)</param>
        /// <param name="perdidasSur">% de perdidas del área sur (ingresado por el usuario)</param>
        /// <param name="perdidasCentro">% de perdidas del área centro (ingresado por el usuario)</param>
        /// <param name="perdidasSCentro">% de perdidas del área sierra centro (ingresado por el usuario)</param>
        /// <param name="recalculo">valor porcentual generado a traves de los datos importados</param>
        /// <param name="regFecha">Fecha para la ejecución</param>
        /// <returns></returns>
        public object PerdidasPR03Ejecutar(decimal[] perdidasNorte, decimal[] perdidasSur,
            decimal[] perdidasCentro, decimal[] perdidasSCentro, decimal[] recalculo, string regFecha)
        {
            string typeMsg = String.Empty;
            string dataMsg = String.Empty;

            DateTime parseFecha = DateTime.ParseExact(regFecha,
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            int validarPronostico = this.ValidarEjecucionPronosticoPorAreas(regFecha);
            if (validarPronostico == 0)
            {
                typeMsg = ConstantesProdem.MsgError;
                dataMsg = "No se ha ejecutado el pronóstico por áreas para el día seleccionado, es necesario ejecutarlo...";
                return new { typeMsg, dataMsg };
            }

            //DBarras(Area x)
            #region Cálculo de la demanda en barras por área operativa (desde demanda de área)
            List<PrnMedicion48DTO> dBarrasxAreas = new List<PrnMedicion48DTO>
            {
                { new PrnMedicion48DTO() {
                    Ptomedicodi = ConstantesProdem.PtomedicodiANorte,
                    Areacodi = ConstantesProdem.AreacodiANorte,
                    ArrayMediciones = perdidasNorte
                } },
                { new PrnMedicion48DTO() {
                    Ptomedicodi = ConstantesProdem.PtomedicodiASur,
                    Areacodi = ConstantesProdem.AreacodiASur,
                    ArrayMediciones = perdidasSur
                } },
                { new PrnMedicion48DTO() {
                    Ptomedicodi = ConstantesProdem.PtomedicodiACentro,
                    Areacodi = ConstantesProdem.AreacodiACentro,
                    ArrayMediciones = perdidasCentro
                } },
                { new PrnMedicion48DTO() {
                    Ptomedicodi = ConstantesProdem.PtomedicodiASierraCentro,
                    Areacodi = ConstantesProdem.AreacodiASierraCentro,
                    ArrayMediciones = perdidasSCentro
                } }
            };

            List<PrnMedicion48DTO> dIndustrialxAreas = new List<PrnMedicion48DTO>();
            foreach (PrnMedicion48DTO entity in dBarrasxAreas)
            {
                //Obtiene los datos de la demanda industrial por área
                PrnMedicion48DTO demandaIndustrial = this.GetDemandaULibresPorArea(entity.Areacodi, regFecha, 1);
                dIndustrialxAreas.Add(demandaIndustrial);

                //Obtiene los datos del pronóstico por áreas
                PrnMedicion48DTO pronosticoDatos = this.GetByIdPrnMedicion48(entity.Ptomedicodi,
                    ConstantesProdem.PrntProdemArea, parseFecha);
                decimal[] arrayDatos = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, pronosticoDatos);
                PrnMedicion48DTO pronosticoAjuste = this.GetByIdPrnMedicion48(entity.Ptomedicodi,
                    ConstantesProdem.PrntProdemAreaAjuste, parseFecha);
                decimal[] arrayAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, pronosticoDatos);
                arrayDatos = arrayDatos.Zip(arrayAjuste, (a, b) => a + b).ToArray();

                //Refleja la demanda por áreas a demanda por barras
                int i = 0;
                while (i < ConstantesProdem.Itv30min)
                {
                    //DBarras(Area x) = (1 - (%Perd.Base + %Recalculo)) x DNa(Area x)
                    decimal valor = (1 - (entity.ArrayMediciones[i] + recalculo[i])) * arrayDatos[i];
                    entity.GetType().GetProperty("H" + (i + 1)).SetValue(entity, valor);
                    i++;
                }
            }
            #endregion

            //FDc(Barra y)
            #region Cálculo del Factor de distribución de una barra CP             
            List<int> barrasCP = this.GetListBarrasCP(ConstantesProdem.Prcatecodi)
                .Select(x => x.Grupocodi)
                .ToList();

            decimal[] totalVegetativa = new decimal[ConstantesProdem.Itv30min];
            List<PrnMediciongrpDTO> datosVegetativa = new List<PrnMediciongrpDTO>();
            foreach (int idBarra in barrasCP)
            {
                PrnMediciongrpDTO entity = this.GetByIdPrnMediciongrp(idBarra,
                    ConstantesProdem.PrnmgrtDemVegetativa, parseFecha);
                decimal[] vegetativaBarra = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, entity);

                totalVegetativa = totalVegetativa.Zip(vegetativaBarra, (a, b) => a + b).ToArray();
                datosVegetativa.Add(entity);
            }
            //Obtiene el factor de distribución
            //[0]: idBarra, [1]: factor de distribución
            List<dynamic[]> fDistribucionxBarra = new List<dynamic[]>();
            foreach (PrnMediciongrpDTO datoVegetativa in datosVegetativa)
            {
                decimal[] vegetativaBarra = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, datoVegetativa);
                //Fdc(Barra Y) = Dveg-PB(Barra Y) / (Sum[1 -> N°Barras]Dveg-PB(Barra X))                
                decimal fDistribucion = (totalVegetativa.Sum() != 0)
                    ? vegetativaBarra.Sum() / totalVegetativa.Sum() : 0;
                fDistribucionxBarra.Add(new dynamic[] { datoVegetativa.Grupocodi, fDistribucion });
            }
            #endregion

            //DVeg(Barra y)
            #region Cálculo de la demanda vegetativa de una barra CP
            decimal[] totalDBarrasxAreas = new decimal[ConstantesProdem.Itv30min];
            foreach (PrnMedicion48DTO entity in dBarrasxAreas)
            {
                decimal[] entityDatos = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, entity);
                totalDBarrasxAreas = totalDBarrasxAreas.Zip(entityDatos, (a, b) => a + b).ToArray();
            }
            decimal[] totalDIndustrialxAreas = new decimal[ConstantesProdem.Itv30min];
            foreach (PrnMedicion48DTO entity in dIndustrialxAreas)
            {
                decimal[] entityDatos = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, entity);
                totalDIndustrialxAreas = totalDIndustrialxAreas.Zip(entityDatos, (a, b) => a + b).ToArray();
            }

            foreach (int idBarra in barrasCP)
            {
                List<decimal> demandaVegetativaxBarraCP = new List<decimal>();
                List<decimal> demandaIndustrialxBarraCP = new List<decimal>();

                //Factor de distribución de la barra
                decimal fDistribucion = fDistribucionxBarra.First(x => x[0] == idBarra)[1];

                #region Crea y registra la entidad Dem.Vegetativa por barra CP desde Dem.Area
                PrnMediciongrpDTO entity = new PrnMediciongrpDTO()
                {
                    Grupocodi = idBarra,
                    Prnmgrtipo = ConstantesProdem.PrnmgrtDemVegDesdeArea,
                    Medifecha = parseFecha
                };

                int i = 0;
                while (i < ConstantesProdem.Itv30min)
                {
                    decimal valor = (totalDBarrasxAreas[i] - totalDIndustrialxAreas[i]) * fDistribucion;
                    entity.GetType().GetProperty("H" + (i + 1)).SetValue(entity, valor);
                    demandaVegetativaxBarraCP.Add(valor);
                    i++;
                }
                this.DeletePrnMediciongrp(entity.Grupocodi, entity.Prnmgrtipo, entity.Medifecha);
                this.SavePrnMediciongrp(entity);
                #endregion

                #region Crea y registra la entidad Dem.Industrial por barra CP desde Dem.Area
                entity = new PrnMediciongrpDTO()
                {
                    Grupocodi = idBarra,
                    Prnmgrtipo = ConstantesProdem.PrnmgrtDemIndDesdeArea,
                    Medifecha = parseFecha
                };
                i = 0;
                while (i < ConstantesProdem.Itv30min)
                {
                    decimal valor = totalDIndustrialxAreas[i] * fDistribucion;
                    entity.GetType().GetProperty("H" + (i + 1)).SetValue(entity, valor);
                    demandaIndustrialxBarraCP.Add(valor);
                    i++;
                }
                this.DeletePrnMediciongrp(entity.Grupocodi, entity.Prnmgrtipo, entity.Medifecha);
                this.SavePrnMediciongrp(entity);
                #endregion

                #region Crea y registra la entidad Pronostico de demanda por barra CP desde Dem.Area
                entity = new PrnMediciongrpDTO()
                {
                    Grupocodi = idBarra,
                    Prnmgrtipo = ConstantesProdem.PrnmgrtProdemDesdeArea,
                    Medifecha = parseFecha
                };
                i = 0;
                while (i < ConstantesProdem.Itv30min)
                {
                    decimal valor = demandaVegetativaxBarraCP[i] + demandaIndustrialxBarraCP[i];
                    entity.GetType().GetProperty("H" + (i + 1)).SetValue(entity, valor);
                    i++;
                }
                this.DeletePrnMediciongrp(entity.Grupocodi, entity.Prnmgrtipo, entity.Medifecha);
                this.SavePrnMediciongrp(entity);
                #endregion
            }
            #endregion

            typeMsg = ConstantesProdem.MsgSuccess;
            dataMsg = "El proceso se ejecuto correctamente!";

            return new { typeMsg, dataMsg };
        }

        /// <summary>
        /// Procesa el archivo excel importado
        /// </summary>
        /// <param name="rutaArchivo">Dirección del archivo importado</param>
        /// <param name="regFecha">Fecha de referencia para la ejecución</param>
        /// <returns></returns>
        public object PerdidasPR03Procesar(string rutaArchivo, string regFecha)
        {
            string typeMsg = String.Empty;
            string dataMsg = String.Empty;
            DateTime parseFecha = DateTime.ParseExact(regFecha,
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            //Lee el archivo
            List<string> datosArchivo = new List<string>();
            using (StreamReader sr = new StreamReader(rutaArchivo))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null) datosArchivo.Add(s);
            }

            //Obtiene los datos requeridos
            datosArchivo.RemoveAt(0);
            List<string[]> arrayDatos = new List<string[]>();
            foreach (string d in datosArchivo)
            {
                arrayDatos.Add(d.Split(new char[] { ',' }));
            }

            //Obtiene la demanda Yupana del archivo importado
            List<decimal> demandaYupana = new List<decimal>();
            foreach (string[] d in arrayDatos)
            {
                //Suma de la columnas "B", "C" y "D" del archivo importado
                decimal valor = Convert.ToDecimal(d[1]);
                valor += Convert.ToDecimal(d[2]);
                valor += Convert.ToDecimal(d[3]);
                demandaYupana.Add(valor);
            }

            //Calcula el % de recalculo
            decimal[] recalculo = new decimal[ConstantesProdem.Itv30min];
            PrnMedicion48DTO datosSein = this.GetByIdPrnMedicion48(ConstantesProdem.PtomedicodiASein,
                ConstantesProdem.PrntProdemArea, parseFecha);
            decimal[] demandaSein = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, datosSein);
            if (demandaSein.Sum() == 0)
            {
                typeMsg = ConstantesProdem.MsgError;
                dataMsg = "No se ha ejecutado el pronóstico de la demanda a nivel" +
                    " de área para este día o los valores calculados suman 0";
                return new { typeMsg, dataMsg };
            }

            int i = 0;
            while (i < ConstantesProdem.Itv30min)
            {
                recalculo[i] = (demandaYupana[i] - demandaSein[i]) / demandaSein[i];
                i++;
            }

            typeMsg = ConstantesProdem.MsgSuccess;
            dataMsg = "El archivo fue cargado de manera exitosa, " +
                "se muestra el resultado para la columna %RECALCULO";
            return new { typeMsg, dataMsg, recalculo };
        }

        /// <summary>
        /// Valida si existe en la base de datos pronóstico por áreas ejecutados el día consulta
        /// </summary>
        /// <param name="regFecha">Fecha de consulta</param>
        /// <returns></returns>
        public int ValidarEjecucionPronosticoPorAreas(string regFecha)
        {
            return FactorySic.GetPrnMedicion48Repository().ValidarEjecucionPronosticoPorAreas(regFecha);
        }
        #endregion

        #region Métodos del Módulo de Reduccion de red
        /// <summary>
        /// Lista de Brras especificando su categoria
        /// </summary>
        /// <param name="catecodi"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> GetListBarrasCategoria(int catecodi)
        {
            return FactorySic.GetPrGrupoRepository().ListaBarraCategoria(catecodi);
        }

        /// <summary>
        /// Trae la lista de los nombres de las barras CP
        /// </summary>
        /// <returns></returns>
        public List<PrnReduccionRedDTO> GetListRed()
        {
            return FactorySic.GetPrnReduccionRedRepository().ListByNombre();
        }

        /// <summary>
        /// Lista de reduccion de red basada en la version
        /// </summary>
        /// <param name="version"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public List<PrnReduccionRedDTO> ListaReduccionRedByCPNivel(int version, string tipo)
        {
            return FactorySic.GetPrnReduccionRedRepository().ListByCPNivel(version, tipo);
        }

        public List<PrnReduccionRedDTO> ListaValGaussPerdidas(string tipo)
        {
            return FactorySic.GetPrnReduccionRedRepository().ListPuntosAgrupacionesByBarra(tipo);
        }

        /// <summary>
        /// Elimina un registro de la tabla PRN_REDUCCIONRED mediante su pk
        /// </summary>
        /// <param name="reduccionred"></param>
        /// <param name="version"></param>
        public void DeletePrnReduccionRed(int reduccionred, int version)
        {
            FactorySic.GetPrnReduccionRedRepository().DeleteReduccionRed(reduccionred, version);
        }

        /// <summary>
        /// Elimina una ReduccionRed especificando la barracp, barrapm y version
        /// </summary>
        /// <param name="barracp"></param>
        /// <param name="barrapm"></param>
        /// <param name="version"></param>
        public void DeletePrnReduccionRedBarraVersion(int barrapm, int barracp, int version)
        {
            FactorySic.GetPrnReduccionRedRepository().DeletePrnReduccionRedBarraVersion(barrapm, barracp, version);
        }

        /// <summary>
        /// Metodo que actualiza las listas de combos segun la version
        /// </summary>
        /// <param name="catecodi"></param>
        /// <param name="prnvercodi"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> GetListBarraCPDisponibles(int catecodi, int prnvercodi)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().ListBarraCPDisponibles(catecodi, prnvercodi);
        }

        /// <summary>
        /// Metodo para la Lista de combo PM
        /// </summary>
        /// <param name="catecodi"></param>
        /// <param name="prnvercodi"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> GetListBarraPMDisponibles(int catecodi, int prnvercodi)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().ListBarraPMDisponibles(catecodi, prnvercodi);
        }

        /// <summary>
        /// Actualiza la lista de PM quitando las CP de otra lista
        /// </summary>
        /// <param name="catecodi"></param>
        /// <param name="barracp"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> GetListBarraPM(int catecodi, string barracp, int version)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().ListBarrasPM(catecodi, barracp, version);
        }

        /// <summary>
        /// Lista Barras PM para el PopEditar
        /// </summary>
        /// <param name="catecodi"></param>
        /// <param name="barracp"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> GetListBarraPMEdit(int catecodi, int version)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().ListBarrasPMEdit(catecodi, version);
        }

        /// <summary>
        /// Permite Registrar una reduccion de red
        /// </summary>
        /// <param name="entity"></param>
        public void ReduccionRedSave(PrnReduccionRedDTO entity)
        {
            FactorySic.GetPrnReduccionRedRepository().Save(entity);
        }

        /// <summary>
        /// Actualiza el registro
        /// </summary>
        /// <param name="entity"></param>
        public void ReduccionRedUpdate(PrnReduccionRedDTO entity)
        {
            FactorySic.GetPrnReduccionRedRepository().Update(entity);
        }

        /// <summary>
        /// Trae la nueva lista para rellenar las PM
        /// </summary>
        /// <param name="barrascp"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> ListBarrasPM(List<int> barrascp, int version)
        {

            PrGrupoDTO barra = new PrGrupoDTO();
            List<PrGrupoDTO> listaBarras = new List<PrGrupoDTO>();
            //Dar la forma para el IN de la consulta sql
            string inBarras = "";
            int c = 0;
            if (barrascp.Count == 0)
            {
                inBarras = "0";
            }
            else
            {
                foreach (var item in barrascp)
                {
                    inBarras += item.ToString().Trim();
                    c++;
                    if (barrascp.Count > c)
                    {
                        inBarras += ",".Trim();
                    }
                }
            }
            //GetListBarraCPDisponibles

            listaBarras = this.GetListBarraPM(ConstantesProdem.Prcatecodi, inBarras, version);

            return listaBarras;
        }

        public List<PrGrupoDTO> ListBarrasPMEdit(int version)
        {
            //PrGrupoDTO barra = new PrGrupoDTO();
            List<PrGrupoDTO> listaBarras = new List<PrGrupoDTO>();

            listaBarras = this.GetListBarraPMEdit(ConstantesProdem.Prcatecodi, version);

            return listaBarras;
        }

        /// <summary>
        /// Lista Activa de reduccion de red
        /// </summary>
        /// <param name="version"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public List<object> ListReduccionVersion(int version, List<string> tipo)
        {
            //ReduccionRedModel model = new ReduccionRedModel();
            List<PrnReduccionRedDTO> red = new List<PrnReduccionRedDTO>();
            List<PrnReduccionRedDTO> gaussPerdidas = new List<PrnReduccionRedDTO>();
            List<object> data = new List<object>();

            //Dar la forma para el IN de la consulta sql
            string tipos = UtilProdem.ListToString(tipo);
            red = this.ListaReduccionRedByCPNivel(version, tipos);
            List<int> barrapm = new List<int>();
            foreach (var item in red)
            {
                barrapm.Add(item.Prnredbarrapm);
            }

            List<int> pm = barrapm.Distinct().ToList();
            string pmToString = (pm.Count != 0) ? string.Join(",", pm) : "0";

            //Lista para los valores gaussianos y perdidas, 
            //se agrega un identificador al para saber si es Agrupacion(A) o Punto(P)
            gaussPerdidas = this.ListaValGaussPerdidas(pmToString);
            object entity;

            for (int i = 0; i < pm.Count; i++)
            {
                entity = new
                {
                    id = red.Where(x => x.Prnredbarrapm == pm[i]).Select(x => x.Prnredcodi).ToList(),
                    barrapmId = pm[i],
                    nombre = red.Where(x => x.Prnredbarrapm == pm[i]).Select(x => x.Prnrednombre).Distinct().ToList(),
                    barrapmNombre = red.Where(x => x.Prnredbarrapm == pm[i]).Select(x => x.Nombrepm).Distinct().ToList(),
                    barracpId = red.Where(x => x.Prnredbarrapm == pm[i]).Select(x => x.Prnredbarracp).ToList(),
                    barracpNombre = red.Where(x => x.Prnredbarrapm == pm[i]).Select(x => x.Nombrecp).Distinct().ToList(),
                    puntoId = gaussPerdidas.Where(x => x.Grupocodibarra == pm[i]).Select(x => x.Ptomedicodi).ToList(),
                    puntoNombre = gaussPerdidas.Where(x => x.Grupocodibarra == pm[i]).Select(x => x.Nombre).Distinct().ToList(),
                    barraTipo = red.Where(x => x.Prnredbarrapm == pm[i]).Select(x => x.Prnredtipo).Distinct().ToList(),
                    barraGauss = red.Where(x => x.Prnredbarrapm == pm[i]).Select(x => x.Prnredgauss).ToList(),
                    barraPerdida = red.Where(x => x.Prnredbarrapm == pm[i]).Select(x => x.Prnredperdida).ToList()
                };
                data.Add(entity);
            }


            return data;
        }

        /// <summary>
        /// 19032020 Metodo para validar registros y evitar que se grabe una misma CP a una misma PM
        /// </summary>
        /// <param name="barracp"></param>
        /// <param name="barrapm"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public List<PrnReduccionRedDTO> ValidandoRegistrosReduccionRed(int[] barracp, int[] barrapm, string version)
        {

            bool validacion = true;

            List<PrnReduccionRedDTO> Lista = new List<PrnReduccionRedDTO>();
            List<PrnReduccionRedDTO> registrosLista = new List<PrnReduccionRedDTO>();
            List<PrnReduccionRedDTO> reduccionRedLista = FactorySic.GetPrnReduccionRedRepository().ListByCPNivel(Convert.ToInt32(version), "0");

            for (int i = 0; i < barracp.Length; i++)
            {
                for (int j = 0; j < barrapm.Length; j++)
                {
                    PrnReduccionRedDTO entity = new PrnReduccionRedDTO();

                    entity.Prnredbarracp = barracp[i];
                    entity.Prnvercodi = Convert.ToInt32(version);
                    entity.Prnredbarrapm = barrapm[j];

                    Lista.Add(entity);
                }
            }

            foreach (var x in Lista)
            {
                foreach (var y in reduccionRedLista)
                {
                    if (y.Prnredbarracp == x.Prnredbarracp && y.Prnredbarrapm == x.Prnredbarrapm)
                    {
                        registrosLista.Add(y);
                    }
                }
            }

            return registrosLista;
        }

        /// <summary>
        /// 19032020 Se valida que no sume mas de 1 las barras
        /// </summary>
        /// <param name="datareduccion"></param>
        /// <param name="dataperdida"></param>
        /// <param name="barracp"></param>
        /// <param name="barrapm"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public bool ValidandoSumaPM(string[][] datareduccion, string[][] dataperdida, int[] barracp, int[] barrapm, string version)
        {

            bool validacion = true;
            List<PrnReduccionRedDTO> sumLista = new List<PrnReduccionRedDTO>();
            List<PrnReduccionRedDTO> nuevaLista = new List<PrnReduccionRedDTO>();
            List<PrnReduccionRedDTO> nuevaListaAgrupada = new List<PrnReduccionRedDTO>();

            string inBarrasPM = string.Join(",", barrapm);

            //Lista agrupada de las Barras PM sumando su Gausssiano
            sumLista = FactorySic.GetPrnReduccionRedRepository().ListSumaBarraGaussPM(Convert.ToInt32(version), inBarrasPM);

            //Lista de Barras PM con su gaussiano enviadas desde la vista
            for (int i = 0; i < barracp.Length; i++)
            {
                for (int j = 0; j < barrapm.Length; j++)
                {
                    PrnReduccionRedDTO entity = new PrnReduccionRedDTO();

                    entity.Prnredbarrapm = barrapm[j];
                    entity.Prnredgauss = Convert.ToDecimal(datareduccion[i][j + 1]);
                    nuevaLista.Add(entity);
                }
            }

            //Barras PM enviadas desde la vista agruapdas sumando su Gaussiano
            foreach (var n in barrapm)
            {
                PrnReduccionRedDTO entity = new PrnReduccionRedDTO();

                entity.Prnredbarrapm = n;
                decimal g = 0;
                foreach (var m in nuevaLista)
                {
                    if (n == m.Prnredbarrapm)
                    {
                        g += Convert.ToDecimal(m.Prnredgauss);
                    }
                }
                entity.Prnredgauss = g;
                nuevaListaAgrupada.Add(entity);
            }


            //Sumando el gaussiano de la BD y la vista para verificar que no sume mas de 1.
            foreach (var x in nuevaListaAgrupada)
            {
                decimal gaussTotal = Convert.ToDecimal(x.Prnredgauss);
                foreach (var y in sumLista)
                {
                    if (x.Prnredbarrapm == y.Prnredbarrapm)
                    {
                        gaussTotal += Convert.ToDecimal(y.Prnredgauss);
                    }
                }

                if (gaussTotal > 1)
                {
                    validacion = false;
                }
            }



            return validacion;
        }

        /// <summary>
        /// Barras para el defecto
        /// </summary>
        /// <param name="catecodi"></param>
        /// <param name="prnvercodi"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> GetListBarraDefecto(int catecodi, int prnvercodi)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().ListBarraDefecto(catecodi, prnvercodi);
        }
        #endregion

        #region Métodos para el Módulo de Consulta Perfil Extranet

        public object PerfilesDatosExtranet(int idPunto, DateTime regFecha)
        {
            string title = string.Empty;
            List<object> data = new List<object>();

            //Obtiene el titulo principal
            title = this.GetNombrePtomedicion(idPunto);

            //Obtiene la configuración del punto
            PrnConfiguracionDTO regconfiguracion = this.ParametrosGetConfiguracion(idPunto,
                ConstantesProdem.DefectoByPunto, regFecha);
            //Obtiene los datos del perfil patrón
            PrnPatronModel regPatron = this.GetPatron(idPunto, ConstantesProdem.ProcPatronDemandaEjecutada, regFecha, regconfiguracion);
            //Obtiene los datos del perfil defecto
            //DateTime defDate = DateTime.ParseExact(ConstantesProdem.DefectoDate, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            //PrnMedicion48DTO regDefecto = this.GetByIdPrnMedicion48(idPunto, UtilProdem.ValidarPatronDiaDefecto(regFecha), defDate);

            //Validación
            if (regPatron.NDias == 0) return new { valid = false };

            //Formatos de presentación
            object entity;
            object entityH;
            MeMedicion48DTO medicion = new MeMedicion48DTO();
            //a.1) Intervalos de tiempo
            entity = new
            {
                id = "intervalos",
                label = "Hora",
                data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                htrender = "hora",
                hcrender = "categoria"
            };
            data.Add(entity);

            //a.2) Mediciones
            for (int i = 0; i < regPatron.Mediciones.Count; i++) // for (int i = 0; i < regPatron.Mediciones.Count; i++)
            {
                entity = new
                {
                    id = "med" + (i + 1).ToString(),
                    label = regPatron.StrFechas[i],
                    data = regPatron.Mediciones[i],
                    htrender = "normal",
                    hcrender = "normal",
                    label2 = regPatron.StrFechasTarde[i],
                    slunes = regPatron.EsLunes
                };
                data.Add(entity);

                //Obteniendo Medicion Reportada por el agente desde extranet
                medicion = new MeMedicion48DTO();
                medicion = this.GetByIdMeMedicion48(ConstantesProdem.LectcodiDemEjecDiario, regPatron.Fechas[i], ConstantesProdem.TipoinfocodiMWDemanda, idPunto);

                entityH = new
                {
                    id = "medR" + (i + 1).ToString(),
                    label = regPatron.StrFechas[i] + "(R)",
                    data = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, medicion),//regPatron.Mediciones[i]
                    htrender = "normal",
                    hcrender = "normal",
                    label2 = regPatron.StrFechasTarde[i] + "(R)",
                    slunes = regPatron.EsLunes
                };
                data.Add(entityH);
            }

            //a.3) Patron
            entity = new
            {
                id = "patron",
                label = "Patrón",
                data = regPatron.Patron,
                htrender = "patron",
                hcrender = "patron"
            };
            data.Add(entity);

            //a.5) Márgenes de error
            decimal[] rMin = new decimal[ConstantesProdem.Itv30min];
            decimal[] rMax = new decimal[ConstantesProdem.Itv30min];
            int j = 0;
            while (j < ConstantesProdem.Itv30min)
            {
                decimal fMin = 1 - (regconfiguracion.Prncfgporcdsvptrn * 0.01M) ?? 0;
                decimal fMax = 1 + (regconfiguracion.Prncfgporcdsvptrn * 0.01M) ?? 0;
                rMin[j] = regPatron.Patron[j] * fMin;
                rMax[j] = regPatron.Patron[j] * fMax;
                j++;
            }

            entity = new
            {
                id = "rmin",
                label = "EMin",
                data = rMin,
                htrender = "no",
                hcrender = "margen"
            };
            data.Add(entity);

            entity = new
            {
                id = "rmax",
                label = "EMax",
                data = rMax,
                htrender = "no",
                hcrender = "margen"
            };
            data.Add(entity);

            return new { title = title, data = data, config = regconfiguracion, valid = true };
        }
        #endregion

        #region Métodos de los procesos automáticos

        /// <summary>
        /// Proceso que "Cataloga, Clasifica y Depura" la demanda  reportada por el agente desde Extranet
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha correspondiente a la medición</param>
        /// <param name="regMedicion">Mediciones por intervalo del punto de medición</param>
        /// <param name="dataConfig">Datos de la configuración de parámetros</param>
        /// <param name="dataPatron">Datos de la obtención del perfíl patrón</param>
        /// <param name="flagJustificacion">Flag que indica si la demanda fue justificada</param>
        /// <param name="nomUsuario">Usuario que ejecuta el proceso</param>
        public void ProcesarDemandaReportada(int idPunto, DateTime regFecha, MeMedicion48DTO regMedicion,
            PrnConfiguracionDTO dataConfig, PrnPatronModel dataPatron, bool flagJustificacion, string nomUsuario)
        {
            //Prepara la medición
            decimal[] arrayMedicion = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, regMedicion);

            //Valida si se utiliza el perfíl normal o el defecto
            decimal[] medPatron = (dataConfig.Prncfgflagdefecto == "S") ? dataPatron.PatronDefecto : dataPatron.Patron;

            //Catalogar
            int dataPerfil = this.CatalogarMedicion(dataConfig.Prncfgporcdsvptrn, arrayMedicion, medPatron);

            //Clasificar
            int dataPrioridad = this.ClasificarDemanda(dataConfig.Prncfgporcerrormin, dataConfig.Prncfgporcerrormax,
                dataConfig.Prncfgmagcargamin, dataConfig.Prncfgmagcargamax, arrayMedicion, medPatron);

            //Depurar
            Tuple<int, decimal[]> dataDepuracion = this.DepurarDemanda(dataConfig.Prncfgporcdsvptrn,
                dataConfig.Prncfgporcmuestra, dataConfig.Prncfgflagdepauto, flagJustificacion, arrayMedicion, medPatron);
            int toPatron = dataDepuracion.Item1;
            decimal[] dataAjuste = dataDepuracion.Item2;

            //Calcula la variación contra el perfil patron activo
            decimal dataVariacion = Math.Round(medPatron.Sum() - arrayMedicion.Sum(), 2);
            dataVariacion = Math.Abs(dataVariacion);

            #region Registro de los datos

            //Registra la entidad clasificación
            PrnClasificacionDTO entClasificacion = new PrnClasificacionDTO();
            entClasificacion.Ptomedicodi = idPunto;
            entClasificacion.Prnclsclasificacion = dataPrioridad;
            entClasificacion.Prnclsperfil = dataPerfil;
            entClasificacion.Prnclsvariacion = dataVariacion;
            entClasificacion.Lectcodi = regMedicion.Lectcodi;
            entClasificacion.Prnclsfecha = regFecha;
            entClasificacion.Tipoinfocodi = ConstantesProdem.TipoinfocodiMWDemanda;
            entClasificacion.Prnclsporcerrormin = dataConfig.Prncfgporcerrormin ?? 0;
            entClasificacion.Prnclsporcerrormax = dataConfig.Prncfgporcerrormax ?? 0;
            entClasificacion.Prnclsmagcargamin = dataConfig.Prncfgmagcargamin ?? 0;
            entClasificacion.Prnclsmagcargamax = dataConfig.Prncfgmagcargamax ?? 0;
            entClasificacion.Prnclsestado = "no";
            entClasificacion.Prnclsusucreacion = nomUsuario;
            entClasificacion.Prnclsfeccreacion = DateTime.Now;
            entClasificacion.Prnclsusumodificacion = nomUsuario;
            entClasificacion.Prnclsfecmodificacion = DateTime.Now;

            this.DeletePrnClasificacion(entClasificacion.Ptomedicodi, entClasificacion.Lectcodi, entClasificacion.Prnclsfecha);
            this.SavePrnClasificacion(entClasificacion);

            //Registra la entidad prnmedicion48 con el correspondiente PRNM48TIPO
            int idTipoAuto = -1;
            int idTipoManual = -1;

            switch (regMedicion.Lectcodi)
            {
                case ConstantesProdem.LectcodiDemEjecDiario:
                    idTipoAuto = ConstantesProdem.PrntDemandaEjecutadaAjusteAuto;
                    idTipoManual = ConstantesProdem.PrntDemandaEjecutadaAjusteManual;
                    break;
                case ConstantesProdem.LectcodiDemPrevDiario:
                    idTipoAuto = ConstantesProdem.PrntDemandaPrevistaAjusteAuto;
                    idTipoManual = ConstantesProdem.PrntDemandaPrevistaAjusteManual;
                    break;
                case ConstantesProdem.LectcodiDemPrevSemanal:
                    idTipoAuto = ConstantesProdem.PrntDemandaSemanalAjusteAuto;
                    idTipoManual = ConstantesProdem.PrntDemandaSemanalAjusteManual;
                    break;
            }

            PrnMedicion48DTO entMedicion = new PrnMedicion48DTO();
            entMedicion.Ptomedicodi = idPunto;
            entMedicion.Prnm48tipo = idTipoAuto;
            entMedicion.Medifecha = regFecha;
            entMedicion.Meditotal = dataDepuracion.Item2.Sum();
            entMedicion.Prnm48usucreacion = nomUsuario;
            entMedicion.Prnm48feccreacion = DateTime.Now;
            entMedicion.Prnm48usumodificacion = nomUsuario;
            entMedicion.Prnm48fecmodificacion = DateTime.Now;

            int i = 0;
            while (i < ConstantesProdem.Itv30min)
            {
                entMedicion.GetType().GetProperty("H" + (i + 1).ToString()).
                    SetValue(entMedicion, dataDepuracion.Item2[i], null);
                i++;
            }

            //Si es por reprocesamiento elimina el ajuste manual
            this.DeletePrnMedicion48(entMedicion.Ptomedicodi, idTipoManual, entMedicion.Medifecha);
            //
            this.DeletePrnMedicion48(entMedicion.Ptomedicodi, entMedicion.Prnm48tipo, entMedicion.Medifecha);
            this.SavePrnMedicion48(entMedicion);

            #endregion
        }

        /// <summary>
        /// Proceso que permite catalogar una medición por tipo de perfil
        /// No procesado: -1
        /// Normal: 1
        /// Baja de carga: 2
        /// Subidas o Bajadas de carga puntuales: 3
        /// Datos congelados y fuera del patrón: 4
        /// </summary>
        /// <param name="pDesviacion">Porcentaje de desviación</param>
        /// <param name="regMedicion">Valores de la medición por intervalos de la medición a procesar</param>
        /// <param name="regPatron">Valores de la medición por intervalos del perfil patrón</param>
        /// <returns></returns>
        public int CatalogarMedicion(decimal? pDesviacion, decimal[] regMedicion, decimal[] regPatron)
        {
            int perfil = -1;

            if (regPatron.Sum() == 0) return perfil;

            //Convierte los parámetros
            decimal eMin = (pDesviacion != 0) ? 1 - (pDesviacion * 0.01M) ?? 1 : 1;
            decimal eMax = (pDesviacion != 0) ? 1 + (pDesviacion * 0.01M) ?? 1 : 1;

            //Caso 1
            //-------------------------------------------------------------------
            #region Perfil normal
            bool f = true;
            for (int i = 0; i < ConstantesProdem.Itv30min; i++)
            {
                decimal tMin = regPatron[i] * eMin;
                decimal tMax = regPatron[i] * eMax;

                if (!(regMedicion[i] > tMin && regMedicion[i] < tMax))
                {
                    f = false;
                    break;
                }
            }

            if (f)
            {
                perfil = ConstantesProdem.PrnPerfilNormal;
                return perfil;
            }
            #endregion

            //Caso 2
            //-------------------------------------------------------------------
            #region Perfil con baja de carga
            f = false;
            int n = 2; //N° de consecutivos
            List<int> itv = new List<int>();
            for (int i = 0; i < ConstantesProdem.Itv30min; i++)
            {
                decimal tMin = regPatron[i] * eMin;
                if (regMedicion[i] < tMin) itv.Add(i);
            }

            foreach (int i in itv)
            {
                int j = 1;
                while (j < n)
                {
                    if (!itv.Contains(i + j))
                    {
                        break;
                    }
                    else
                    {
                        if (regMedicion[i] == regMedicion[i + j]) break;
                    }
                    j++;
                }

                if (j == n)
                {
                    f = true;
                    break;
                }
            }

            if (f)
            {
                perfil = ConstantesProdem.PrnPerfilBajaCarga;
                return perfil;
            }
            #endregion

            //Caso 3
            //-------------------------------------------------------------------
            #region Perfil con subidas o bajadas de carga puntuales
            f = false;
            itv = new List<int>();
            for (int i = 0; i < ConstantesProdem.Itv30min; i++)
            {
                decimal tMin = regPatron[i] * eMin;
                decimal tMax = regPatron[i] * eMax;
                if (!(regMedicion[i] > tMin && regMedicion[i] < tMax)) itv.Add(i);
            }

            foreach (int i in itv)
            {
                //Evalua los indices contigüos
                if (!itv.Contains(i - 1) && !itv.Contains(i + 1))
                {
                    f = true;
                    break;
                }
            }

            if (f)
            {
                perfil = ConstantesProdem.PrnPerfilSBPuntual;
                return perfil;
            }
            #endregion

            //Caso 4
            //-------------------------------------------------------------------
            #region Perfil con datos congelados y fuera del patrón
            f = false;
            n = 2; //N° de consecutivos
            itv = new List<int>();
            for (int i = 0; i < ConstantesProdem.Itv30min; i++)
            {
                decimal tMin = regPatron[i] * eMin;
                decimal tMax = regPatron[i] * eMax;
                if (!(regMedicion[i] > tMin && regMedicion[i] < tMax)) itv.Add(i);
            }

            foreach (int i in itv)
            {
                int j = 1;
                while (j < n)
                {
                    if (!itv.Contains(i + j))
                    {
                        break;
                    }
                    else
                    {
                        if (regMedicion[i] != regMedicion[i + j]) break;
                    }
                    j++;
                }

                if (j == n)
                {
                    f = true;
                    break;
                }
            }

            if (f)
            {
                perfil = ConstantesProdem.PrnPerfilCongelado;
                return perfil;
            }
            #endregion

            return perfil;
        }

        /// <summary>
        /// Proceso que permite clasificar una medición por prioridades
        /// No procesado: -1
        /// Muy alta: 1
        /// Alta: 2
        /// Media: 3
        /// Baja: 4
        /// </summary>
        /// <param name="pErrorMin">Porcentaje de error mínimo</param>
        /// <param name="pErrorMax">Porcentaje de error máximo</param>
        /// <param name="mCargaMin">Magnitud de carga mínima</param>
        /// <param name="mCargaMax">Magnitud de carga máxima</param>
        /// <param name="regMedicion">Valores de la medición por intervalos de la medición a procesar</param>
        /// <param name="regPatron">Valores de la medición por intervalos del perfil patrón</param>
        /// <returns></returns>
        public int ClasificarDemanda(decimal? pErrorMin, decimal? pErrorMax, decimal? mCargaMin, decimal? mCargaMax,
            decimal[] regMedicion, decimal[] regPatron)
        {
            int prioridad = -1;

            if (regPatron.Sum() == 0) return prioridad;

            //Convierte los parámetros
            decimal eMin = Convert.ToDecimal(pErrorMin * 0.01M);
            decimal eMax = Convert.ToDecimal(pErrorMax * 0.01M);
            decimal mMin = Convert.ToDecimal(mCargaMin);
            decimal mMax = Convert.ToDecimal(mCargaMax);

            #region Cálculo del porcentaje de error
            int i = 0;
            decimal nValido = 0;
            decimal vDiferencia = 0;
            List<decimal> arrayDiferencia = new List<decimal>();

            //Lista las diferencias entre los intervalos de la medición y el perfil patrón
            while (i < ConstantesProdem.Itv30min)
            {
                nValido = 1;

                //Obtiene las diferencias por intervalo
                vDiferencia = regPatron[i] - regMedicion[i];

                //Valida que el divisor no sea 0
                nValido = (regPatron[i] != 0) ? regPatron[i] : 1;

                vDiferencia = Math.Round(vDiferencia / nValido, 2);
                vDiferencia = Math.Abs(vDiferencia);
                arrayDiferencia.Add(vDiferencia);
                i++;
            }

            //Obtiene la sumatoria de diferencias
            decimal sumDiferencia = arrayDiferencia.Sum();

            //Calcula el porcentaje de error
            decimal pError = Math.Round(sumDiferencia / ConstantesProdem.Itv30min, 2);//dos decimales
            #endregion

            #region Cálculo de la magnitud de carga
            //Obtiene la sumatoria de las mediciones de cada intervalo
            decimal sumMedicion = regMedicion.Sum();
            //Calcula la magnitud de carga 
            decimal mCarga = Math.Round(sumMedicion / ConstantesProdem.Itv30min, 2);//dos decimales
            #endregion

            #region Asignación de prioridad segun criterios
            if (pError > eMax)
            {
                if (mCarga > mMax) prioridad = ConstantesProdem.PrnclsclasiMuyalta;
                else if (mCarga >= mMin && mCarga <= mMax) prioridad = ConstantesProdem.PrnclsclasiMuyalta;
                else if (mCarga < mMin) prioridad = ConstantesProdem.PrnclsclasiAlta;
            }
            else if (pError >= eMin && pError <= eMax)
            {
                if (mCarga > mMax) prioridad = ConstantesProdem.PrnclsclasiMuyalta;
                else if (mCarga >= mMin && mCarga <= mMax) prioridad = ConstantesProdem.PrnclsclasiMuyalta;
                else if (mCarga < mMin) prioridad = ConstantesProdem.PrnclsclasiMedia;
            }
            else if (pError < eMin)
            {
                if (mCarga > mMax) prioridad = ConstantesProdem.PrnclsclasiAlta;
                else if (mCarga >= mMin && mCarga <= mMax) prioridad = ConstantesProdem.PrnclsclasiMedia;
                else if (mCarga < mMin) prioridad = ConstantesProdem.PrnclsclasiBaja;
            }
            #endregion

            return prioridad;
        }

        /// <summary>
        /// Proceso que permite depurar una medición segun los parámetros configurados
        /// </summary>
        /// <param name="pDesviacion">Porcentaje de desviación</param>
        /// <param name="pMuestra">Porcentaje de muestra</param>
        /// <param name="fDepuracion">Flag que indica si el punto debe ser depurado</param>
        /// <param name="fJustificacion">Flag que indica si la medición fue justificada</param>
        /// <param name="regMedicion">Valores de la medición por intervalos de la medición a procesar</param>
        /// <param name="regPatron">Valores de la medición por intervalos del perfil patrón</param>
        /// <returns></returns>
        public Tuple<int, decimal[]> DepurarDemanda(decimal? pDesviacion, decimal? pMuestra,
            string fDepuracion, bool fJustificacion, decimal[] regMedicion, decimal[] regPatron)
        {
            int toPatron = -1;
            decimal[] ajuste = new decimal[ConstantesProdem.Itv30min];

            //Validación patron 0
            if (regPatron.Sum() == 0) return new Tuple<int, decimal[]>(toPatron, ajuste);

            //Validación flag depuración
            if (fDepuracion.Equals(ConstantesProdem.RegNo)) return new Tuple<int, decimal[]>(toPatron, ajuste);

            //Validación flag justificación
            if (fJustificacion) return new Tuple<int, decimal[]>(toPatron, ajuste);

            //Convierte los parámetros
            decimal pDsv = Convert.ToDecimal(pDesviacion * 0.01M);
            decimal pMst = Convert.ToDecimal(pMuestra * 0.01M);

            #region Cálculo del pdt(Porcentaje de Desviación) de cada intervalo de tiempo
            int i = 0;
            decimal vDiferencia = 0;
            List<decimal> listDiferencia = new List<decimal>();
            while (i < ConstantesProdem.Itv30min)
            {
                vDiferencia = regPatron[i] - regMedicion[i];
                vDiferencia = Math.Abs(vDiferencia);
                listDiferencia.Add(vDiferencia);
                i++;
            }

            i = 0;
            List<decimal> listPdt = new List<decimal>();
            while (i < ConstantesProdem.Itv30min)
            {
                if (regPatron[i] == 0) regPatron[i] = 1;
                listPdt.Add(Math.Round(listDiferencia[i] / regPatron[i], 2));
                i++;
            }
            #endregion

            #region Cálculo del porcentaje de muestra
            //Obtención del número de intervalos desviados
            i = 0;
            int numDesviados = 0;
            List<int> listDesviados = new List<int>();//Almacena los intervalos desviados contra la patron         
            while (i < ConstantesProdem.Itv30min)
            {
                if (listPdt[i] > pDsv)
                {
                    numDesviados++;
                    listDesviados.Add(i);//Posición del intervalo desviado
                }
                i++;
            }

            //Obtención del porcentaje de muestra                
            decimal vMuestra = Math.Round(numDesviados / (decimal)ConstantesProdem.Itv30min, 2);
            #endregion

            #region Depuración automática
            if (vMuestra < pMst)//Accede al proceso de depuración automática
            {
                toPatron = 1;
                #region 1)Validación contra la Patron
                if (numDesviados != 0)//Existen intervalos desviados, se depuran
                {
                    foreach (int itv in listDesviados)//Reemplaza los valores desviados de la medición con los de la patron
                    {
                        ajuste[itv] = Math.Round(regPatron[itv] - regMedicion[itv], 2);
                    }
                }
                #endregion
            }
            #endregion

            return new Tuple<int, decimal[]>(toPatron, ajuste);
        }

        #endregion

        #region Métodos Extranet

        /// <summary>
        /// Permite listar todos los  registros de la tabla PRN_MOTIVO
        /// </summary>
        public List<MeJustificacionDTO> ListByIdEnvioPtoMedicodi(int idEnvio, int idLectcodi, int idPtomedicodi)
        {
            return FactorySic.GetMeJustificacionRepository().ListByIdEnvioPtoMedicodi(idEnvio, idLectcodi, idPtomedicodi);
        }

        /// <summary>
        /// Permite listar los tipos de justificación
        /// </summary>
        public List<EveSubcausaeventoDTO> GetListaJustificacion()
        {
            var lista = FactorySic.GetEveSubcausaeventoRepository().List();
            var listajust = lista.Where(x => x.Causaevencodi == ConstantesProdem.IdCausaJustificacion).ToList();
            listajust.Add(new EveSubcausaeventoDTO() { Subcausacodi = -1, Subcausadesc = "Otro" });
            return listajust;
        }

        #endregion

        #region Métodos Generales

        /// <summary>
        /// Método que permite obtener la medición calculada
        /// </summary>
        /// <param name="idCalculado">Identificador Ptomedicalc de la tabla ME_PTOMEDICION</param>
        /// <param name="medifecha">Fecha del registro</param>
        /// <returns></returns>
        public decimal[] ObtenerMedicionesCalculadas(int idCalculado, DateTime medifecha)
        {
            COES.Servicios.Aplicacion.Scada.PerfilScadaServicio servicio = new Scada.PerfilScadaServicio();
            decimal[] arrayMedicion = new decimal[ConstantesProdem.Itv30min];
            List<MeMedicion48DTO> listaMeMedicion = new List<MeMedicion48DTO>();

            try
            {
                listaMeMedicion = servicio.ProcesarFormula(idCalculado, medifecha);
                int i = 0;
                while (i < ConstantesProdem.Itv30min)
                {
                    foreach (var a in listaMeMedicion)
                    {
                        var dValor = a.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(a, null);
                        arrayMedicion[i] += (decimal)dValor;
                    }
                    i++;
                }
            }
            catch
            {
                return arrayMedicion;
            }

            return arrayMedicion;
        }

        /// <summary>
        /// Devuelve los parámetros configurados de un punto de mediciónen en un día
        /// </summary>
        /// <param name="idPunto">Pk del punto de medición</param>
        /// <param name="idTipo">Tipo de registro defecto (punto, agrupación o área)</param>
        /// <param name="regFecha">Fecha de configuración</param>
        /// <returns></returns>
        public PrnConfiguracionDTO ParametrosGetConfiguracion(int idPunto, int idTipo, DateTime regFecha)
        {
            string strFecha = regFecha.ToString(ConstantesProdem.FormatoFecha);
            PrnConfiguracionDTO entity = FactorySic.GetPrnConfiguracionRepository().
                GetConfiguracion(idPunto, strFecha, idTipo, ConstantesProdem.DefectoDate);

            //Validación - Si encuentra el registro
            //Completa los parámetros faltantes con los defecto (Si fuera el caso)
            if (entity.Prncfgtiporeg == 1)
            {
                //Obtiene el defecto
                PrnConfiguracionDTO defecto = this.ParametrosGetDefecto(idTipo);

                if (entity.Prncfgporcerrormin == null) entity.Prncfgporcerrormin = defecto.Prncfgporcerrormin;
                if (entity.Prncfgporcerrormax == null) entity.Prncfgporcerrormax = defecto.Prncfgporcerrormax;
                if (entity.Prncfgmagcargamin == null) entity.Prncfgmagcargamin = defecto.Prncfgmagcargamin;
                if (entity.Prncfgmagcargamax == null) entity.Prncfgmagcargamax = defecto.Prncfgmagcargamax;
                if (entity.Prncfgporcdsvptrn == null) entity.Prncfgporcdsvptrn = defecto.Prncfgporcdsvptrn;
                if (entity.Prncfgporcmuestra == null) entity.Prncfgporcmuestra = defecto.Prncfgporcmuestra;
                if (entity.Prncfgporcdsvcnsc == null) entity.Prncfgporcdsvcnsc = defecto.Prncfgporcdsvcnsc;
                if (entity.Prncfgnrocoincidn == null) entity.Prncfgnrocoincidn = defecto.Prncfgnrocoincidn;
                if (entity.Prncfgflagveda == null) entity.Prncfgflagveda = defecto.Prncfgflagveda;
                if (entity.Prncfgflagferiado == null) entity.Prncfgflagferiado = defecto.Prncfgflagferiado;
                if (entity.Prncfgflagatipico == null) entity.Prncfgflagatipico = defecto.Prncfgflagatipico;
                if (entity.Prncfgflagdepauto == null) entity.Prncfgflagdepauto = defecto.Prncfgflagdepauto;
                if (entity.Prncfgtipopatron == null) entity.Prncfgtipopatron = defecto.Prncfgtipopatron;
                if (entity.Prncfgnumdiapatron == null) entity.Prncfgnumdiapatron = defecto.Prncfgnumdiapatron;
                if (entity.Prncfgflagdefecto == null) entity.Prncfgflagdefecto = defecto.Prncfgflagdefecto;
                if (entity.Prncfgpse == null) entity.Prncfgpse = defecto.Prncfgpse;
                if (entity.Prncfgfactorf == null) entity.Prncfgfactorf = defecto.Prncfgfactorf;
            }

            return entity;
        }

        /// <summary>
        /// Devuelve los parámetros configurados de una barra en un día
        /// </summary>
        /// <param name="idBarra">Identificador de la tabla PR_GRUPO corresponde a una barra</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public PrnConfigbarraDTO ParametrosBarrasGetConfiguracion(int idBarra, DateTime regFecha)
        {
            string strFecha = regFecha.ToString(ConstantesProdem.FormatoFecha);
            return FactorySic.GetPrnConfigbarraRepository().
                GetConfiguracion(idBarra, strFecha, ConstantesProdem.DefectoByBarra, ConstantesProdem.DefectoDate);
        }

        /// <summary>
        /// Devuelve la lista de áreas operativas sin la opción SEIN (Temporal)
        /// </summary>
        /// <returns></returns>
        public List<MePtomedicionDTO> GetAreasOperativas()
        {
            MePtomedicionDTO entity = new MePtomedicionDTO();
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();

            entity.Ptomedicodi = ConstantesProdem.PtomedicodiASur;
            entity.Ptomedidesc = "AREA SUR";
            entitys.Add(entity);

            entity = new MePtomedicionDTO();
            entity.Ptomedicodi = ConstantesProdem.PtomedicodiANorte;
            entity.Ptomedidesc = "AREA NORTE";
            entitys.Add(entity);

            entity = new MePtomedicionDTO();
            entity.Ptomedicodi = ConstantesProdem.PtomedicodiACentro;
            entity.Ptomedidesc = "AREA CENTRO";
            entitys.Add(entity);

            entity = new MePtomedicionDTO();
            entity.Ptomedicodi = ConstantesProdem.PtomedicodiASierraCentro;
            entity.Ptomedidesc = "AREA SIERRA CENTRO";
            entitys.Add(entity);

            return entitys;
        }

        /// <summary>
        /// Devuelve el modelo de datos de un perfil patrón
        /// </summary>
        /// <param name="idPunto">Pk del punto de medición</param>
        /// <param name="idProceso">Identificador del proceso que solicita el perfil patrón</param>
        /// <param name="regFecha">Fecha base para la construcción del perfil patrón</param>
        /// <param name="entConfiguracion">Configuración de parámetros del punto de medición</param>
        /// <returns></returns>
        public PrnPatronModel GetPatron(int idPunto, int idProceso, DateTime regFecha, PrnConfiguracionDTO entConfiguracion)
        {
            PrnPatronModel model = new PrnPatronModel();

            DateTime tempDate = new DateTime();
            PrnMedicion48DTO tempEntity = new PrnMedicion48DTO();
            PrnConfiguracionDTO entDefecto = new PrnConfiguracionDTO();
            List<PrnMedicion48DTO> dataMediciones = new List<PrnMedicion48DTO>();

            //Inicia el modelo
            model.EsLunes = false;
            model.Fechas = new List<DateTime>();
            model.StrFechas = new List<string>();
            model.StrFechasTarde = new List<string>();
            model.Mediciones = new List<decimal[]>();
            model.PrnMediciones = new List<PrnMedicion48DTO>();
            model.TipoPatron = entConfiguracion.Prncfgtipopatron;

            //Rango de fechas para la busqueda
            DateTime fecInicio = regFecha.AddDays(-1);
            DateTime fecLimite = regFecha.AddMonths(-6);

            //Obtiene los datos segun proceso
            switch (idProceso)
            {
                case ConstantesProdem.ProcPatronDemandaEjecutada:
                    #region Datos patrón de la demanda ejecutada por punto
                    {
                        //Constantes de tipo de información
                        string idTipo = string.Join(",",
                            new int[] { ConstantesProdem.PrntDemandaEjecutadaAjusteAuto,
                                ConstantesProdem.PrntDemandaEjecutadaAjusteManual });

                        //Obtiene la configuración defecto del tipo de información a buscar
                        entDefecto = this.ParametrosGetDefecto(ConstantesProdem.DefectoByPunto);

                        //Obtiene los datos para la busqueda
                        dataMediciones = FactorySic.GetPrnMedicion48Repository().
                            DataPatronDemandaPorPunto(idPunto, idTipo, fecLimite.ToString(ConstantesProdem.FormatoFecha),
                            fecInicio.ToString(ConstantesProdem.FormatoFecha), ConstantesProdem.LectcodiDemEjecDiario,
                            ConstantesProdem.TipoinfocodiMWDemanda, entConfiguracion, entDefecto);
                    }
                    #endregion
                    break;
                case ConstantesProdem.ProcPatronDemandaPrevista:
                    #region Datos patrón de la demanda prevista por punto
                    {
                        //Constantes de tipo de información
                        string idTipo = ConstantesProdem.PrntDemandaPrevistaAjusteManual.ToString();

                        //Obtiene la configuración defecto del tipo de información a buscar
                        entDefecto = this.ParametrosGetDefecto(ConstantesProdem.DefectoByPunto);

                        //Obtiene los datos para la busqueda
                        dataMediciones = FactorySic.GetPrnMedicion48Repository().
                            DataPatronDemandaPorPunto(idPunto, idTipo, fecLimite.ToString(ConstantesProdem.FormatoFecha),
                            fecInicio.ToString(ConstantesProdem.FormatoFecha), ConstantesProdem.LectcodiDemPrevDiario,
                            ConstantesProdem.TipoinfocodiMWDemanda, entConfiguracion, entDefecto);
                    }
                    #endregion
                    break;
                case ConstantesProdem.ProcPatronDemandaPrevSemanal:
                    #region Datos patrón de la demanda prevista semanal por punto
                    {
                        //Constantes de tipo de información
                        string idTipo = "-1";

                        //Obtiene la configuración defecto del tipo de información a buscar
                        entDefecto = this.ParametrosGetDefecto(ConstantesProdem.DefectoByPunto);

                        //Obtiene los datos para la busqueda
                        dataMediciones = FactorySic.GetPrnMedicion48Repository().
                            DataPatronDemandaPorPunto(idPunto, idTipo, fecLimite.ToString(ConstantesProdem.FormatoFecha),
                            fecInicio.ToString(ConstantesProdem.FormatoFecha), ConstantesProdem.LectcodiDemPrevSemanal,
                            ConstantesProdem.TipoinfocodiMWDemanda, entConfiguracion, entDefecto);
                    }
                    #endregion
                    break;
                case ConstantesProdem.ProcPatronDemandaEjecutadaAgrupada:
                    #region Datos patrón de la demanda ejecutada por agrupación
                    {
                        //Constantes de tipo de información
                        string idTipo = string.Join(",",
                            new int[] { ConstantesProdem.PrntDemandaEjecutadaAjusteAuto,
                                ConstantesProdem.PrntDemandaEjecutadaAjusteManual });

                        //Obtiene los datos para la busqueda
                        dataMediciones = FactorySic.GetPrnMedicion48Repository().DataPatronDemandaPorAgrupacion(ConstantesProdem.OriglectcodiAgrupacion,
                            ConstantesProdem.OriglectcodiPR03, ConstantesProdem.LectcodiDemEjecDiario, ConstantesProdem.TipoinfocodiMWDemanda,
                            fecLimite.ToString(ConstantesProdem.FormatoFecha), fecInicio.ToString(ConstantesProdem.FormatoFecha),
                            idTipo, ConstantesProdem.PrntDemandaEjecutadaAjusteManual, idPunto);
                    }
                    #endregion
                    break;
                case ConstantesProdem.ProcPatronDemandaArea:
                    #region Datos patrón de la demanda ejecutada por área
                    {
                        if (idPunto == ConstantesProdem.PtomedicodiASur)
                        {
                            string idTipo = string.Join(",",
                            new int[] {
                                ConstantesProdem.PrntMantSur,
                                ConstantesProdem.PrntFallaSur,
                                ConstantesProdem.PrntDemandaAreaAjuste
                            });

                            dataMediciones = FactorySic.GetPrnMedicion48Repository().DataPatronDemandaPorArea(idPunto,
                                ConstantesProdem.AreacodiASur, fecLimite.ToString(ConstantesProdem.FormatoFecha),
                                fecInicio.ToString(ConstantesProdem.FormatoFecha), idTipo);
                        }
                        if (idPunto == ConstantesProdem.PtomedicodiANorte)
                        {
                            string idTipo = string.Join(",",
                            new int[] {
                                ConstantesProdem.PrntMantNorte,
                                ConstantesProdem.PrntFallaNorte,
                                ConstantesProdem.PrntDemandaAreaAjuste
                            });

                            dataMediciones = FactorySic.GetPrnMedicion48Repository().DataPatronDemandaPorArea(idPunto,
                                ConstantesProdem.AreacodiASur, fecLimite.ToString(ConstantesProdem.FormatoFecha),
                                fecInicio.ToString(ConstantesProdem.FormatoFecha), idTipo);
                        }
                        if (idPunto == ConstantesProdem.PtomedicodiASierraCentro)
                        {
                            string idTipo = string.Join(",",
                            new int[] {
                                ConstantesProdem.PrntMantSierraCentro,
                                ConstantesProdem.PrntFallaSierraCentro,
                                ConstantesProdem.PrntDemandaAreaAjuste
                            });

                            dataMediciones = FactorySic.GetPrnMedicion48Repository().DataPatronDemandaPorArea(idPunto,
                                ConstantesProdem.AreacodiASur, fecLimite.ToString(ConstantesProdem.FormatoFecha),
                                fecInicio.ToString(ConstantesProdem.FormatoFecha), idTipo);
                        }
                        if (idPunto == ConstantesProdem.PtomedicodiACentro)
                        {
                            string idTipo = string.Join(",",
                            new int[] {
                                ConstantesProdem.PrntMantCentro,
                                ConstantesProdem.PrntFallaCentro,
                                ConstantesProdem.PrntDemandaAreaAjuste
                            });

                            dataMediciones = FactorySic.GetPrnMedicion48Repository().DataPatronDemandaPorArea(idPunto,
                                ConstantesProdem.AreacodiASur, fecLimite.ToString(ConstantesProdem.FormatoFecha),
                                fecInicio.ToString(ConstantesProdem.FormatoFecha), idTipo);
                        }
                    }
                    #endregion
                    break;
                case ConstantesProdem.ProcPatronDemandaPorBarra:
                    #region Datos patrón de la demanda ejecutada por barra CP
                    {
                        //Constantes de tipo de información
                        string idTipo = string.Join(",",
                            new int[] { ConstantesProdem.PrntDemandaEjecutadaAjusteAuto,
                                ConstantesProdem.PrntDemandaEjecutadaAjusteManual });

                        //Obtiene los datos para la busqueda
                        /*
                        dataMediciones = FactorySic.GetPrnMedicion48Repository().DataPatronDemandaPorBarraCP(idPunto, ConstantesProdem.OriglectcodiPR03,
                            ConstantesProdem.OriglectcodiAgrupacion, ConstantesProdem.TipoinfocodiMWDemanda, ConstantesProdem.LectcodiDemEjecDiario,
                            fecLimite.ToString(ConstantesProdem.FormatoFecha), fecInicio.ToString(ConstantesProdem.FormatoFecha), idTipo,
                            ConstantesProdem.PrntDemandaEjecutadaAjusteManual);
                            */
                    }
                    #endregion
                    break;
            }

            //Aplica las reglas de negocio
            tempDate = regFecha;
            int diaSemana = (int)regFecha.DayOfWeek;
            int maxDias = (int)entConfiguracion.Prncfgnumdiapatron;
            List<int> ruleA = new List<int> { ConstantesProdem.DiaAsIntLunes, ConstantesProdem.DiaAsIntSabado,
                ConstantesProdem.DiaAsIntDomingo };
            if (ruleA.Contains(diaSemana))
            {
                #region Lunes(Mañana), Domingo o Sabado
                int d = 0;
                while (d < maxDias)
                {
                    tempDate = tempDate.AddDays(-7);
                    if (tempDate > fecLimite)
                    {
                        foreach (var a in dataMediciones)
                        {
                            if (a.Medifecha.Equals(tempDate))
                            {
                                decimal[] dArray = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, a);
                                if (dArray.Sum() > 0)
                                {
                                    model.PrnMediciones.Add(a);
                                    model.Mediciones.Add(dArray);
                                    model.Fechas.Add(a.Medifecha);
                                    model.StrFechas.Add(a.Medifecha.ToString(ConstantesProdem.FormatoFecha));
                                    model.StrFechasTarde.Add(a.Medifecha.ToString(ConstantesProdem.FormatoFecha));
                                    d++;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                model.NDias = d;
                model.Mensaje = "Se capturaron " + d.ToString() + " mediciones historicas";
                #endregion
            }

            tempDate = regFecha;
            List<int> ruleB = new List<int> { ConstantesProdem.DiaAsIntMartes,
                ConstantesProdem.DiaAsIntMiercoles, ConstantesProdem.DiaAsIntJueves, ConstantesProdem.DiaAsIntViernes };
            if (ruleB.Contains(diaSemana))
            {
                #region Lunes(Tarde), Martes, Miercoles, Jueves o Viernes
                int d = 0;
                bool esLunes = false;
                if (diaSemana.Equals(ConstantesProdem.DiaAsIntLunes))
                {
                    esLunes = true;
                    maxDias = model.NDias;
                    model.EsLunes = true;
                    model.StrFechasTarde = new List<string>();
                }

                while (d < maxDias)
                {
                    tempDate = tempDate.AddDays(-1);
                    diaSemana = (int)tempDate.DayOfWeek;
                    while (ruleA.Contains(diaSemana))
                    {
                        tempDate = tempDate.AddDays(-1);
                        diaSemana = (int)tempDate.DayOfWeek;
                    }
                    if (tempDate > fecLimite)
                    {
                        foreach (var a in dataMediciones)
                        {
                            if (a.Medifecha.Equals(tempDate))
                            {
                                decimal[] dArray = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, a);
                                if (dArray.Sum() > 0)
                                {
                                    if (esLunes)
                                    {
                                        for (int i = ConstantesProdem.Itv30min / 2; i < ConstantesProdem.Itv30min; i++)
                                        {
                                            model.Mediciones[d][i] = dArray[i];
                                            model.PrnMediciones[d].GetType().GetProperty("H" + (i + 1).ToString()).
                                                SetValue(model.PrnMediciones[d], dArray[i]);
                                        }
                                        model.StrFechasTarde.Add(a.Medifecha.ToString(ConstantesProdem.FormatoFecha));
                                        d++;
                                        break;
                                    }
                                    else
                                    {
                                        model.PrnMediciones.Add(a);
                                        model.Mediciones.Add(dArray);
                                        model.Fechas.Add(a.Medifecha);
                                        model.StrFechas.Add(a.Medifecha.ToString(ConstantesProdem.FormatoFecha));
                                        model.StrFechasTarde.Add(a.Medifecha.ToString(ConstantesProdem.FormatoFecha));
                                        d++;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (!esLunes)
                {
                    model.NDias = d;
                    model.Mensaje = "Se capturaron " + d.ToString() + " mediciones historicas";
                }
                #endregion
            }

            //Completa las mediciones segun lo configurado
            int j = model.PrnMediciones.Count;
            maxDias = (int)entConfiguracion.Prncfgnumdiapatron;
            if (j != maxDias)
            {
                for (int i = 0; i < maxDias - j; i++)
                {
                    model.Mediciones.Add(new decimal[ConstantesProdem.Itv30min]);
                    model.PrnMediciones.Add(new PrnMedicion48DTO());
                    model.Fechas.Add(DateTime.MinValue);
                    model.StrFechas.Add("No encontrada");
                    model.StrFechasTarde.Add("No encontrada");
                }
            }

            //Obtiene el perfil patron defecto, en caso no se pueda obtener el perfil típico
            model.PatronDefecto = this.GetPatronDefecto(idPunto, regFecha);

            //Calcula el perfil patrón
            model.Patron = UtilProdem.CalcularPerfilPatron(model.Mediciones,
                model.NDias, ConstantesProdem.Itv30min, entConfiguracion.Prncfgtipopatron);

            return model;
        }

        /// <summary>
        /// Devuelve el modelo de datos de un perfil patrón
        /// </summary>
        /// <param name="idPunto">Pk del punto de medición</param>
        /// <param name="idProceso">Identificador del proceso que solicita el perfil patrón</param>
        /// <param name="regFecha">Fecha base para la construcción del perfil patrón</param>
        /// <param name="entConfiguracion">Configuración de parámetros del punto de medición</param>
        /// <returns></returns>
        public PrnPatronModel GetPatronPorFecha(int idPunto, int idProceso, DateTime regFecha, PrnConfiguracionDTO entConfiguracion)
        {
            PrnPatronModel model = new PrnPatronModel();

            DateTime tempDate = new DateTime();
            PrnMedicion48DTO tempEntity = new PrnMedicion48DTO();
            PrnConfiguracionDTO entDefecto = new PrnConfiguracionDTO();
            List<PrnMedicion48DTO> dataMediciones = new List<PrnMedicion48DTO>();

            //Inicia el modelo
            model.EsLunes = false;
            model.Fechas = new List<DateTime>();
            model.StrFechas = new List<string>();
            model.StrFechasTarde = new List<string>();
            model.Mediciones = new List<decimal[]>();
            model.PrnMediciones = new List<PrnMedicion48DTO>();
            model.TipoPatron = entConfiguracion.Prncfgtipopatron;

            //Rango de fechas para la busqueda
            DateTime fecInicio = regFecha;// regFecha.AddDays(-1);
            DateTime fecLimite = regFecha;//.AddMonths(-6); //regFecha;//regFecha.AddMonths(-6);

            //Obtiene los datos segun proceso
            switch (idProceso)
            {
                case ConstantesProdem.ProcPatronDemandaEjecutada:
                    #region Datos patrón de la demanda ejecutada por punto
                    {
                        //Constantes de tipo de información
                        string idTipo = string.Join(",",
                            new int[] { ConstantesProdem.PrntDemandaEjecutadaAjusteAuto,
                                ConstantesProdem.PrntDemandaEjecutadaAjusteManual });

                        //Obtiene la configuración defecto del tipo de información a buscar
                        entDefecto = this.ParametrosGetDefecto(ConstantesProdem.DefectoByPunto);

                        //Obtiene los datos para la busqueda
                        dataMediciones = FactorySic.GetPrnMedicion48Repository().
                            DataPatronDemandaPorPuntoPorFecha(idPunto, idTipo, fecLimite.ToString(ConstantesProdem.FormatoFecha),
                            fecInicio.ToString(ConstantesProdem.FormatoFecha), ConstantesProdem.LectcodiDemEjecDiario,
                            ConstantesProdem.TipoinfocodiMWDemanda, entConfiguracion, entDefecto);
                    }
                    #endregion
                    break;
                case ConstantesProdem.ProcPatronDemandaPrevista:
                    #region Datos patrón de la demanda prevista por punto
                    {
                        //Constantes de tipo de información
                        string idTipo = ConstantesProdem.PrntDemandaPrevistaAjusteManual.ToString();

                        //Obtiene la configuración defecto del tipo de información a buscar
                        entDefecto = this.ParametrosGetDefecto(ConstantesProdem.DefectoByPunto);

                        //Obtiene los datos para la busqueda
                        dataMediciones = FactorySic.GetPrnMedicion48Repository().
                            DataPatronDemandaPorPunto(idPunto, idTipo, fecLimite.ToString(ConstantesProdem.FormatoFecha),
                            fecInicio.ToString(ConstantesProdem.FormatoFecha), ConstantesProdem.LectcodiDemPrevDiario,
                            ConstantesProdem.TipoinfocodiMWDemanda, entConfiguracion, entDefecto);
                    }
                    #endregion
                    break;
                case ConstantesProdem.ProcPatronDemandaPrevSemanal:
                    #region Datos patrón de la demanda prevista semanal por punto
                    {
                        //Constantes de tipo de información
                        string idTipo = "-1";

                        //Obtiene la configuración defecto del tipo de información a buscar
                        entDefecto = this.ParametrosGetDefecto(ConstantesProdem.DefectoByPunto);

                        //Obtiene los datos para la busqueda
                        dataMediciones = FactorySic.GetPrnMedicion48Repository().
                            DataPatronDemandaPorPunto(idPunto, idTipo, fecLimite.ToString(ConstantesProdem.FormatoFecha),
                            fecInicio.ToString(ConstantesProdem.FormatoFecha), ConstantesProdem.LectcodiDemPrevSemanal,
                            ConstantesProdem.TipoinfocodiMWDemanda, entConfiguracion, entDefecto);
                    }
                    #endregion
                    break;
                case ConstantesProdem.ProcPatronDemandaEjecutadaAgrupada:
                    #region Datos patrón de la demanda ejecutada por agrupación
                    {
                        //Constantes de tipo de información
                        string idTipo = string.Join(",",
                            new int[] { ConstantesProdem.PrntDemandaEjecutadaAjusteAuto,
                                ConstantesProdem.PrntDemandaEjecutadaAjusteManual });

                        //Obtiene los datos para la busqueda
                        dataMediciones = FactorySic.GetPrnMedicion48Repository().DataPatronDemandaPorAgrupacion(ConstantesProdem.OriglectcodiAgrupacion,
                            ConstantesProdem.OriglectcodiPR03, ConstantesProdem.LectcodiDemEjecDiario, ConstantesProdem.TipoinfocodiMWDemanda,
                            fecLimite.ToString(ConstantesProdem.FormatoFecha), fecInicio.ToString(ConstantesProdem.FormatoFecha),
                            idTipo, ConstantesProdem.PrntDemandaEjecutadaAjusteManual, idPunto);
                    }
                    #endregion
                    break;
                case ConstantesProdem.ProcPatronDemandaArea:
                    #region Datos patrón de la demanda ejecutada por área
                    {
                        if (idPunto == ConstantesProdem.PtomedicodiASur)
                        {
                            string idTipo = string.Join(",",
                            new int[] {
                                ConstantesProdem.PrntMantSur,
                                ConstantesProdem.PrntFallaSur,
                                ConstantesProdem.PrntDemandaAreaAjuste
                            });

                            dataMediciones = FactorySic.GetPrnMedicion48Repository().DataPatronDemandaPorArea(idPunto,
                                ConstantesProdem.AreacodiASur, fecLimite.ToString(ConstantesProdem.FormatoFecha),
                                fecInicio.ToString(ConstantesProdem.FormatoFecha), idTipo);
                        }
                        if (idPunto == ConstantesProdem.PtomedicodiANorte)
                        {
                            string idTipo = string.Join(",",
                            new int[] {
                                ConstantesProdem.PrntMantNorte,
                                ConstantesProdem.PrntFallaNorte,
                                ConstantesProdem.PrntDemandaAreaAjuste
                            });

                            dataMediciones = FactorySic.GetPrnMedicion48Repository().DataPatronDemandaPorArea(idPunto,
                                ConstantesProdem.AreacodiASur, fecLimite.ToString(ConstantesProdem.FormatoFecha),
                                fecInicio.ToString(ConstantesProdem.FormatoFecha), idTipo);
                        }
                        if (idPunto == ConstantesProdem.PtomedicodiASierraCentro)
                        {
                            string idTipo = string.Join(",",
                            new int[] {
                                ConstantesProdem.PrntMantSierraCentro,
                                ConstantesProdem.PrntFallaSierraCentro,
                                ConstantesProdem.PrntDemandaAreaAjuste
                            });

                            dataMediciones = FactorySic.GetPrnMedicion48Repository().DataPatronDemandaPorArea(idPunto,
                                ConstantesProdem.AreacodiASur, fecLimite.ToString(ConstantesProdem.FormatoFecha),
                                fecInicio.ToString(ConstantesProdem.FormatoFecha), idTipo);
                        }
                        if (idPunto == ConstantesProdem.PtomedicodiACentro)
                        {
                            string idTipo = string.Join(",",
                            new int[] {
                                ConstantesProdem.PrntMantCentro,
                                ConstantesProdem.PrntFallaCentro,
                                ConstantesProdem.PrntDemandaAreaAjuste
                            });

                            dataMediciones = FactorySic.GetPrnMedicion48Repository().DataPatronDemandaPorArea(idPunto,
                                ConstantesProdem.AreacodiASur, fecLimite.ToString(ConstantesProdem.FormatoFecha),
                                fecInicio.ToString(ConstantesProdem.FormatoFecha), idTipo);
                        }
                    }
                    #endregion
                    break;
                case ConstantesProdem.ProcPatronDemandaPorBarra:
                    #region Datos patrón de la demanda ejecutada por barra CP
                    {
                        //Constantes de tipo de información
                        string idTipo = string.Join(",",
                            new int[] { ConstantesProdem.PrntDemandaEjecutadaAjusteAuto,
                                ConstantesProdem.PrntDemandaEjecutadaAjusteManual });

                        //Obtiene los datos para la busqueda
                        /*
                        dataMediciones = FactorySic.GetPrnMedicion48Repository().DataPatronDemandaPorBarraCP(idPunto, ConstantesProdem.OriglectcodiPR03,
                            ConstantesProdem.OriglectcodiAgrupacion, ConstantesProdem.TipoinfocodiMWDemanda, ConstantesProdem.LectcodiDemEjecDiario,
                            fecLimite.ToString(ConstantesProdem.FormatoFecha), fecInicio.ToString(ConstantesProdem.FormatoFecha), idTipo,
                            ConstantesProdem.PrntDemandaEjecutadaAjusteManual);
                            */
                    }
                    #endregion
                    break;
            }

            //Aplica las reglas de negocio
            tempDate = regFecha;
            /*
            if (idPunto == 21694)
                foreach (var a in dataMediciones)
                {
                    decimal[] dsfd = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, a);
                }*/

            int diaSemana = (int)regFecha.DayOfWeek;
            int maxDias = (int)entConfiguracion.Prncfgnumdiapatron;
            List<int> ruleA = new List<int> { ConstantesProdem.DiaAsIntLunes, ConstantesProdem.DiaAsIntSabado,
                ConstantesProdem.DiaAsIntDomingo };
            if (ruleA.Contains(diaSemana))
            {
                #region Lunes(Mañana), Domingo o Sabado
                int d = 0;
                while (d < maxDias)
                {
                    tempDate = tempDate.AddDays(-7);
                    if (tempDate > fecLimite)
                    {
                        foreach (var a in dataMediciones)
                        {
                            if (a.Medifecha.Equals(tempDate))
                            {
                                decimal[] dArray = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, a);
                                if (dArray.Sum() > 0)
                                {
                                    model.PrnMediciones.Add(a);
                                    model.Mediciones.Add(dArray);
                                    model.Fechas.Add(a.Medifecha);
                                    model.StrFechas.Add(a.Medifecha.ToString(ConstantesProdem.FormatoFecha));
                                    model.StrFechasTarde.Add(a.Medifecha.ToString(ConstantesProdem.FormatoFecha));
                                    d++;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                model.NDias = d;
                model.Mensaje = "Se capturaron " + d.ToString() + " mediciones historicas";
                #endregion
            }

            tempDate = regFecha;
            List<int> ruleB = new List<int> { ConstantesProdem.DiaAsIntMartes,
                ConstantesProdem.DiaAsIntMiercoles, ConstantesProdem.DiaAsIntJueves, ConstantesProdem.DiaAsIntViernes };
            if (ruleB.Contains(diaSemana))
            {
                #region Lunes(Tarde), Martes, Miercoles, Jueves o Viernes
                int d = 0;
                bool esLunes = false;
                if (diaSemana.Equals(ConstantesProdem.DiaAsIntLunes))
                {
                    esLunes = true;
                    maxDias = model.NDias;
                    model.EsLunes = true;
                    model.StrFechasTarde = new List<string>();
                }

                while (d < maxDias)
                {
                    tempDate = tempDate.AddDays(-1);
                    diaSemana = (int)tempDate.DayOfWeek;
                    while (ruleA.Contains(diaSemana))
                    {
                        tempDate = tempDate.AddDays(-1);
                        diaSemana = (int)tempDate.DayOfWeek;
                    }
                    if (tempDate > fecLimite)
                    {
                        foreach (var a in dataMediciones)
                        {
                            if (a.Medifecha.Equals(tempDate))
                            {
                                decimal[] dArray = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, a);
                                if (dArray.Sum() > 0)
                                {
                                    if (esLunes)
                                    {
                                        for (int i = ConstantesProdem.Itv30min / 2; i < ConstantesProdem.Itv30min; i++)
                                        {
                                            model.Mediciones[d][i] = dArray[i];
                                            model.PrnMediciones[d].GetType().GetProperty("H" + (i + 1).ToString()).
                                                SetValue(model.PrnMediciones[d], dArray[i]);
                                        }
                                        model.StrFechasTarde.Add(a.Medifecha.ToString(ConstantesProdem.FormatoFecha));
                                        d++;
                                        break;
                                    }
                                    else
                                    {
                                        model.PrnMediciones.Add(a);
                                        model.Mediciones.Add(dArray);
                                        model.Fechas.Add(a.Medifecha);
                                        model.StrFechas.Add(a.Medifecha.ToString(ConstantesProdem.FormatoFecha));
                                        model.StrFechasTarde.Add(a.Medifecha.ToString(ConstantesProdem.FormatoFecha));
                                        d++;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (!esLunes)
                {
                    model.NDias = d;
                    model.Mensaje = "Se capturaron " + d.ToString() + " mediciones historicas";
                }
                #endregion
            }

            //Completa las mediciones segun lo configurado
            int j = model.PrnMediciones.Count;
            maxDias = (int)entConfiguracion.Prncfgnumdiapatron;
            if (j != maxDias)
            {
                for (int i = 0; i < maxDias - j; i++)
                {
                    model.Mediciones.Add(new decimal[ConstantesProdem.Itv30min]);
                    model.PrnMediciones.Add(new PrnMedicion48DTO());
                    model.Fechas.Add(DateTime.MinValue);
                    model.StrFechas.Add("No encontrada");
                    model.StrFechasTarde.Add("No encontrada");
                }
            }

            //Obtiene el perfil patron defecto, en caso no se pueda obtener el perfil típico
            //model.PatronDefecto = this.GetPatronDefecto(idPunto, regFecha);

            //Calcula el perfil patrón
            //model.Patron = UtilProdem.CalcularPerfilPatron(model.Mediciones, model.NDias, ConstantesProdem.Itv30min, entConfiguracion.Prncfgtipopatron);


            foreach (var a in dataMediciones)
            {
                model.PatronDefecto = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, a);
                model.Patron = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, a);


                //decimal[]
            }

            return model;
        }

        /// <summary>
        /// Devuelve el modelo de datos de un perfil patrón para las áreas operativas segun su demanda ejecutada
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public PrnPatronModel GetPatronDemandaArea(int idArea, DateTime regFecha)
        {
            PrnPatronModel model = new PrnPatronModel();
            PrnConfiguracionDTO entConfiguracion = this.ParametrosGetConfiguracion(idArea, ConstantesProdem.DefectoByArea, regFecha);

            if (idArea == ConstantesProdem.PtomedicodiACentro)
            {
                model = this.GetPatron(idArea, ConstantesProdem.ProcPatronDemandaArea, regFecha, entConfiguracion);

                //Suma los flujos de linea por cada medición y recalcula el perfíl patron
                for (int i = 0; i < model.NDias; i++)
                {
                    //Obtiene los flujos de las otras áreas
                    decimal[] fljSur = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                           this.GetFlujoIdealValidado(ConstantesProdem.PtomedicodiASur, model.StrFechas[i]));
                    decimal[] fljNorte = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                        this.GetFlujoIdealValidado(ConstantesProdem.PtomedicodiANorte, model.StrFechas[i]));
                    decimal[] fljSCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                        this.GetFlujoIdealValidado(ConstantesProdem.PtomedicodiASierraCentro, model.StrFechas[i]));

                    //Obtiene los ajustes de las otras áreas
                    decimal[] ajsAreas = this.GetAjusteAlCentroPorTipoSum(model.StrFechas[i], ConstantesProdem.PrntDemandaAreaAjuste);

                    //Actualiza los datos del modelo
                    for (int j = 0; j < ConstantesProdem.Itv30min; j++)
                    {
                        model.Mediciones[i][j] += fljSur[j];
                        model.Mediciones[i][j] += fljNorte[j];
                        model.Mediciones[i][j] += fljSCentro[j];

                        model.Mediciones[i][j] += ajsAreas[j];

                        decimal newVal = model.Mediciones[i][j] + fljSur[j] + fljNorte[j] + fljSCentro[j] + ajsAreas[j];
                        model.PrnMediciones[i].GetType().GetProperty("H" + (i + 1).ToString()).SetValue(model.PrnMediciones[i], newVal);
                    }

                    //Si es lunes, actualiza los intervalos de la tarde
                    //if (model.EsLunes)
                    //{
                    //    //Obtiene los flujos de las otras áreas
                    //    decimal[] itSur = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                    //           this.GetFlujoIdealValidado(ConstantesProdem.PtomedicodiASur, model.StrFechasTarde[i]));
                    //    decimal[] itNorte = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                    //        this.GetFlujoIdealValidado(ConstantesProdem.PtomedicodiANorte, model.StrFechasTarde[i]));
                    //    decimal[] itSCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                    //        this.GetFlujoIdealValidado(ConstantesProdem.PtomedicodiASierraCentro, model.StrFechasTarde[i]));

                    //    //Obtiene los ajustes de las otras áreas
                    //    decimal[] itAreas = this.GetAjusteAlCentroPorTipoSum(model.StrFechasTarde[i], ConstantesProdem.PrntDemandaAreaAjuste);

                    //    //Actualiza los datos del modelo
                    //    int j = (ConstantesProdem.Itv30min / 2) - 1;
                    //    while (j < ConstantesProdem.Itv30min)
                    //    {
                    //        model.Mediciones[i][j] += fljSur[j];
                    //        model.Mediciones[i][j] += fljNorte[j];
                    //        model.Mediciones[i][j] += fljSCentro[j];

                    //        model.Mediciones[i][j] += ajsAreas[j];

                    //        decimal newVal = model.Mediciones[i][j] + fljSur[j] + fljNorte[j] + fljSCentro[j] + ajsAreas[j];
                    //        model.PrnMediciones[i].GetType().GetProperty("H" + (j + 1).ToString()).SetValue(model.PrnMediciones[i], newVal);

                    //        j++;
                    //    }
                    //}

                    //Calcula el nuevo perfíl patrón
                    model.Patron = UtilProdem.CalcularPerfilPatron(model.Mediciones,
                        model.NDias, ConstantesProdem.Itv30min, entConfiguracion.Prncfgtipopatron);
                }
            }
            else
            {
                model = this.GetPatron(idArea, ConstantesProdem.ProcPatronDemandaArea, regFecha, entConfiguracion);

                //Suma los flujos de linea por cada medición y recalcula el perfíl patron
                for (int i = 0; i < model.NDias; i++)
                {
                    //Obtiene el flujo de linea
                    decimal[] dataFlujo = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                        this.GetFlujoIdealValidado(idArea, model.StrFechas[i]));

                    //Actualiza los datos del modelo
                    for (int j = 0; j < ConstantesProdem.Itv30min; j++)
                    {
                        model.Mediciones[i][j] += dataFlujo[j];

                        decimal newVal = model.Mediciones[i][j] + dataFlujo[j];
                        model.PrnMediciones[i].GetType().GetProperty("H" + (i + 1).ToString()).SetValue(model.PrnMediciones[i], newVal);
                    }

                    //Si es lunes, actualiza los intervalos de la tarde
                    //if (model.EsLunes)
                    //{
                    //    //Obtiene los flujos de las otras áreas
                    //    decimal[] itSur = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                    //           this.GetFlujoIdealValidado(ConstantesProdem.PtomedicodiASur, model.StrFechasTarde[i]));
                    //    decimal[] itNorte = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                    //        this.GetFlujoIdealValidado(ConstantesProdem.PtomedicodiANorte, model.StrFechasTarde[i]));
                    //    decimal[] itSCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                    //        this.GetFlujoIdealValidado(ConstantesProdem.PtomedicodiASierraCentro, model.StrFechasTarde[i]));

                    //    //Obtiene los ajustes de las otras áreas
                    //    decimal[] itAreas = this.GetAjusteAlCentroPorTipoSum(model.StrFechasTarde[i], ConstantesProdem.PrntDemandaAreaAjuste);

                    //    //Actualiza los datos del modelo
                    //    int j = (ConstantesProdem.Itv30min / 2) - 1;
                    //    while (j < ConstantesProdem.Itv30min)
                    //    {
                    //        model.Mediciones[i][j] += dataFlujo[j];

                    //        decimal newVal = model.Mediciones[i][j] + dataFlujo[j];
                    //        model.PrnMediciones[i].GetType().GetProperty("H" + (j + 1).ToString()).SetValue(model.PrnMediciones[i], newVal);

                    //        j++;
                    //    }
                    //}

                    //Calcula el nuevo perfíl patrón
                    model.Patron = UtilProdem.CalcularPerfilPatron(model.Mediciones,
                        model.NDias, ConstantesProdem.Itv30min, entConfiguracion.Prncfgtipopatron);
                }
            }

            return model;
        }

        /// <summary>
        /// Devuelve el modelo de datos de un perfil patrón para las áreas operativas segun su demanda vegetativa
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public PrnPatronModel GetPatronVegetativa(int idArea, DateTime regFecha)
        {
            PrnPatronModel model = this.GetPatronDemandaArea(idArea, regFecha);

            switch (idArea)
            {
                case ConstantesProdem.PtomedicodiASur:
                    {
                        #region Área operativa sur

                        for (int i = 0; i < model.NDias; i++)
                        {
                            //Obtiene los ajustes realizados a la demanda vegetativa
                            decimal[] dAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                this.GetByIdPrnMedicion48(idArea, ConstantesProdem.PrntDemandaVegetativaAjuste, model.Fechas[i]));

                            //Agrega la demanda de los usuarios libres
                            decimal[] dULibres = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                this.GetDemandaULibresPorArea(ConstantesProdem.AreacodiASur, model.StrFechas[i], 1));

                            //Actualiza los datos del modelo
                            for (int j = 0; j < ConstantesProdem.Itv30min; j++)
                            {
                                model.Mediciones[i][j] += dAjuste[j];
                                model.Mediciones[i][j] -= dULibres[j];

                                decimal newVal = model.Mediciones[i][j] + dAjuste[j] - dULibres[j];
                                model.PrnMediciones[i].GetType().GetProperty("H" + (i + 1).ToString()).SetValue(model.PrnMediciones[i], newVal);
                            }

                            //Si es lunes, actualiza los intervalos de la tarde
                            //if (model.EsLunes)
                            //{
                            //    DateTime parseDateT = DateTime.ParseExact(model.StrFechasTarde[i], ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

                            //    decimal[] itAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            //    this.GetByIdPrnMedicion48(idArea, ConstantesProdem.PrntDemandaVegetativaAjuste, parseDateT));

                            //    decimal[] itULibres = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            //        this.GetDemandaULibresPorArea(ConstantesProdem.AreacodiASur, model.StrFechasTarde[i], 1));

                            //    //Actualiza los datos del modelo
                            //    int j = (ConstantesProdem.Itv30min / 2) - 1;
                            //    while (j < ConstantesProdem.Itv30min)
                            //    {
                            //        model.Mediciones[i][j] += itAjuste[j];
                            //        model.Mediciones[i][j] -= itULibres[j];

                            //        decimal newVal = model.Mediciones[i][j] + itAjuste[j] - itULibres[j];
                            //        model.PrnMediciones[i].GetType().GetProperty("H" + (j + 1).ToString()).SetValue(model.PrnMediciones[i], newVal);

                            //        j++;
                            //    }
                            //}

                            //Calcula el nuevo perfíl patrón
                            model.Patron = UtilProdem.CalcularPerfilPatron(model.Mediciones,
                                model.NDias, ConstantesProdem.Itv30min, model.TipoPatron);
                        }
                        #endregion
                    }
                    break;
                case ConstantesProdem.PtomedicodiANorte:
                    {
                        #region Área operativa norte

                        for (int i = 0; i < model.NDias; i++)
                        {
                            //Obtiene los ajustes realizados a la demanda vegetativa
                            decimal[] dAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                this.GetByIdPrnMedicion48(idArea, ConstantesProdem.PrntDemandaVegetativaAjuste, model.Fechas[i]));

                            //Agrega la demanda de los usuarios libres
                            decimal[] dULibres = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                this.GetDemandaULibresPorArea(ConstantesProdem.AreacodiANorte, model.StrFechas[i], 1));

                            //Actualiza los datos del modelo
                            for (int j = 0; j < ConstantesProdem.Itv30min; j++)
                            {
                                model.Mediciones[i][j] += dAjuste[j];
                                model.Mediciones[i][j] -= dULibres[j];

                                decimal newVal = model.Mediciones[i][j] + dAjuste[j] - dULibres[j];
                                model.PrnMediciones[i].GetType().GetProperty("H" + (i + 1).ToString()).SetValue(model.PrnMediciones[i], newVal);
                            }

                            //Si es lunes, actualiza los intervalos de la tarde
                            //if (model.EsLunes)
                            //{
                            //    DateTime parseDateT = DateTime.ParseExact(model.StrFechasTarde[i], ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

                            //    decimal[] itAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            //    this.GetByIdPrnMedicion48(idArea, ConstantesProdem.PrntDemandaVegetativaAjuste, parseDateT));

                            //    decimal[] itULibres = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            //        this.GetDemandaULibresPorArea(ConstantesProdem.AreacodiANorte, model.StrFechasTarde[i], 1));

                            //    //Actualiza los datos del modelo
                            //    int j = (ConstantesProdem.Itv30min / 2) - 1;
                            //    while (j < ConstantesProdem.Itv30min)
                            //    {
                            //        model.Mediciones[i][j] += itAjuste[j];
                            //        model.Mediciones[i][j] -= itULibres[j];

                            //        decimal newVal = model.Mediciones[i][j] + itAjuste[j] - itULibres[j];
                            //        model.PrnMediciones[i].GetType().GetProperty("H" + (j + 1).ToString()).SetValue(model.PrnMediciones[i], newVal);

                            //        j++;
                            //    }
                            //}

                            //Calcula el nuevo perfíl patrón
                            model.Patron = UtilProdem.CalcularPerfilPatron(model.Mediciones,
                                model.NDias, ConstantesProdem.Itv30min, model.TipoPatron);
                        }
                        #endregion
                    }
                    break;
                case ConstantesProdem.PtomedicodiASierraCentro:
                    {
                        #region Área operativa sierra centro

                        for (int i = 0; i < model.NDias; i++)
                        {
                            //Obtiene los ajustes realizados a la demanda vegetativa
                            decimal[] dAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                this.GetByIdPrnMedicion48(idArea, ConstantesProdem.PrntDemandaVegetativaAjuste, model.Fechas[i]));

                            //Agrega la demanda de los usuarios libres
                            decimal[] dULibres = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                this.GetDemandaULibresPorArea(ConstantesProdem.AreacodiASierraCentro, model.StrFechas[i], 1));

                            //Actualiza los datos del modelo
                            for (int j = 0; j < ConstantesProdem.Itv30min; j++)
                            {
                                model.Mediciones[i][j] += dAjuste[j];
                                model.Mediciones[i][j] -= dULibres[j];

                                decimal newVal = model.Mediciones[i][j] + dAjuste[j] - dULibres[j];
                                model.PrnMediciones[i].GetType().GetProperty("H" + (i + 1).ToString()).SetValue(model.PrnMediciones[i], newVal);
                            }

                            //Si es lunes, actualiza los intervalos de la tarde
                            //if (model.EsLunes)
                            //{
                            //    DateTime parseDateT = DateTime.ParseExact(model.StrFechasTarde[i], ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

                            //    decimal[] itAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            //    this.GetByIdPrnMedicion48(idArea, ConstantesProdem.PrntDemandaVegetativaAjuste, parseDateT));

                            //    decimal[] itULibres = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            //        this.GetDemandaULibresPorArea(ConstantesProdem.AreacodiASierraCentro, model.StrFechasTarde[i], 1));

                            //    //Actualiza los datos del modelo
                            //    int j = (ConstantesProdem.Itv30min / 2) - 1;
                            //    while (j < ConstantesProdem.Itv30min)
                            //    {
                            //        model.Mediciones[i][j] += itAjuste[j];
                            //        model.Mediciones[i][j] -= itULibres[j];

                            //        decimal newVal = model.Mediciones[i][j] + itAjuste[j] - itULibres[j];
                            //        model.PrnMediciones[i].GetType().GetProperty("H" + (j + 1).ToString()).SetValue(model.PrnMediciones[i], newVal);

                            //        j++;
                            //    }
                            //}

                            //Calcula el nuevo perfíl patrón
                            model.Patron = UtilProdem.CalcularPerfilPatron(model.Mediciones,
                                model.NDias, ConstantesProdem.Itv30min, model.TipoPatron);
                        }
                        #endregion
                    }
                    break;
                case ConstantesProdem.PtomedicodiACentro:
                    {
                        #region Área operativa centro

                        for (int i = 0; i < model.NDias; i++)
                        {
                            //Obtiene los ajustes realizados a la demanda vegetativa
                            decimal[] dAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                this.GetByIdPrnMedicion48(idArea, ConstantesProdem.PrntDemandaVegetativaAjuste, model.Fechas[i]));

                            //Agrega la demanda de los usuarios libres
                            decimal[] dULibres = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                this.GetDemandaULibresPorArea(ConstantesProdem.AreacodiASierraCentro, model.StrFechas[i], 1));

                            //Obtiene los ajustes de las otras áreas
                            decimal[] dAreas = this.GetAjusteAlCentroPorTipoSum(model.StrFechas[i], ConstantesProdem.PrntDemandaVegetativaAjuste);

                            //Actualiza los datos del modelo
                            for (int j = 0; j < ConstantesProdem.Itv30min; j++)
                            {
                                model.Mediciones[i][j] += dAjuste[j];
                                model.Mediciones[i][j] -= dULibres[j];
                                model.Mediciones[i][j] += dAreas[j];

                                decimal newVal = model.Mediciones[i][j] + dAjuste[j] - dULibres[j] + dAreas[j];
                                model.PrnMediciones[i].GetType().GetProperty("H" + (i + 1).ToString()).SetValue(model.PrnMediciones[i], newVal);
                            }

                            //Si es lunes, actualiza los intervalos de la tarde
                            //if (model.EsLunes)
                            //{
                            //    DateTime parseDateT = DateTime.ParseExact(model.StrFechasTarde[i], ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

                            //    decimal[] itAjuste = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            //    this.GetByIdPrnMedicion48(idArea, ConstantesProdem.PrntDemandaVegetativaAjuste, parseDateT));

                            //    decimal[] itULibres = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            //        this.GetDemandaULibresPorArea(ConstantesProdem.AreacodiASierraCentro, model.StrFechasTarde[i], 1));

                            //    decimal[] itAreas = this.GetAjusteAlCentroPorTipoSum(model.StrFechasTarde[i], ConstantesProdem.PrntDemandaVegetativaAjuste);

                            //    //Actualiza los datos del modelo
                            //    int j = (ConstantesProdem.Itv30min / 2) - 1;
                            //    while (j < ConstantesProdem.Itv30min)
                            //    {
                            //        model.Mediciones[i][j] += itAjuste[j];
                            //        model.Mediciones[i][j] -= itULibres[j];
                            //        model.Mediciones[i][j] += itAreas[j];

                            //        decimal newVal = model.Mediciones[i][j] + itAjuste[j] - itULibres[j] + itAreas[j];
                            //        model.PrnMediciones[i].GetType().GetProperty("H" + (j + 1).ToString()).SetValue(model.PrnMediciones[i], newVal);

                            //        j++;
                            //    }
                            //}

                            //Calcula el nuevo perfíl patrón
                            model.Patron = UtilProdem.CalcularPerfilPatron(model.Mediciones,
                                model.NDias, ConstantesProdem.Itv30min, model.TipoPatron);
                        }
                        #endregion
                    }
                    break;
            }
            return model;
        }

        /// <summary>
        /// Devuelve el modelo de datos la data histórica de una barra PM
        /// </summary>
        /// <param name="idBarraPM">Identificador de la tabla PR_GRUPO corresponde a una barra PM</param>
        /// <param name="dGauss">Valor del factor gauss asignado a la barra PM</param>
        /// <param name="dPerdida">Valor de la pérdida asignada a la barra PM</param>
        /// <param name="idFuente">Identificador del tipo de busqueda [Distribuidores: "dt" o Usuarios Libres: "ul"]</param>
        /// <param name="regFecha">Fecha base de referencia para la busqueda de información</param>
        /// <param name="entConfiguracion">Configuración de parámetros del punto de medición</param>
        /// <returns></returns>
        public PrnPatronModel GetDataHistoricaBarraPM(int idBarraPM, decimal dGauss, decimal dPerdida,
            string idFuente, DateTime regFecha, PrnConfiguracionDTO entConfiguracion)
        {
            PrnPatronModel model = new PrnPatronModel();

            DateTime tempDate = new DateTime();
            PrnMedicion48DTO tempEntity = new PrnMedicion48DTO();
            PrnConfiguracionDTO entDefecto = new PrnConfiguracionDTO();
            List<PrnMedicion48DTO> dataMedicion = new List<PrnMedicion48DTO>();
            List<PrnMedicion48DTO> dataPrevista = new List<PrnMedicion48DTO>();
            List<PrnMedicion48DTO> dataSemanal = new List<PrnMedicion48DTO>();

            //Inicia el modelo
            model.EsLunes = false;
            model.Fechas = new List<DateTime>();
            model.StrFechas = new List<string>();
            model.StrFechasTarde = new List<string>();
            model.Mediciones = new List<decimal[]>();
            model.PrnMediciones = new List<PrnMedicion48DTO>();
            model.TipoPatron = entConfiguracion.Prncfgtipopatron;

            //Rango de fechas para la busqueda
            DateTime fecInicio = regFecha.AddDays(-1);
            DateTime fecLimite = regFecha.AddMonths(-6);

            //Obtiene los datos para la busqueda
            string idTipo = "-1";

            //Datos para distribuidores
            if (idFuente.Equals("dt"))
            {
                //Demanda ejecutada
                idTipo = string.Join(",",
                new int[] { ConstantesProdem.PrntDemandaEjecutadaAjusteAuto,
                                ConstantesProdem.PrntDemandaEjecutadaAjusteManual });
                dataMedicion = FactorySic.GetPrnMedicion48Repository().DataHistoricaBarraPMDistr(idBarraPM, dGauss, dPerdida,
                    ConstantesProdem.OriglectcodiPR03, ConstantesProdem.TipoinfocodiMWDemanda, ConstantesProdem.LectcodiDemEjecDiario,
                    fecLimite.ToString(ConstantesProdem.FormatoFecha), fecInicio.ToString(ConstantesProdem.FormatoFecha), idTipo);

                //Demanda prevista - comentado por requerimiento del usuario
                //idTipo = string.Join(",",
                //new int[] { ConstantesProdem.PrntDemandaPrevistaAjusteAuto,
                //                ConstantesProdem.PrntDemandaPrevistaAjusteManual });
                //dataMedicion.AddRange(
                //    FactorySic.GetPrnMedicion48Repository().DataHistoricaBarraPMDistr(idBarraPM, dGauss, dPerdida,
                //    ConstantesProdem.OriglectcodiPR03, ConstantesProdem.TipoinfocodiMWDemanda, ConstantesProdem.LectcodiDemPrevDiario,
                //    fecLimite.ToString(ConstantesProdem.FormatoFecha), fecInicio.ToString(ConstantesProdem.FormatoFecha), idTipo)
                //    );

                //Demanda Semanal - comentado por requerimiento del usuario
                //idTipo = string.Join(",",
                //new int[] { ConstantesProdem.PrntDemandaSemanalAjusteAuto,
                //                ConstantesProdem.PrntDemandaSemanalAjusteManual });
                //dataMedicion.AddRange(
                //    FactorySic.GetPrnMedicion48Repository().DataHistoricaBarraPMDistr(idBarraPM, dGauss, dPerdida,
                //    ConstantesProdem.OriglectcodiPR03, ConstantesProdem.TipoinfocodiMWDemanda, ConstantesProdem.LectcodiDemPrevSemanal,
                //    fecLimite.ToString(ConstantesProdem.FormatoFecha), fecInicio.ToString(ConstantesProdem.FormatoFecha), idTipo)
                //    );
            }

            //Datos para usuarios libres
            if (idFuente.Equals("ul"))
            {
                //Demanda prevista
                idTipo = string.Join(",",
                new int[] { ConstantesProdem.PrntDemandaPrevistaAjusteAuto,
                                ConstantesProdem.PrntDemandaPrevistaAjusteManual });
                dataMedicion = FactorySic.GetPrnMedicion48Repository().DataHistoricaBarraPMUlibre(-1, idBarraPM,
                    ConstantesProdem.OriglectcodiPR03, ConstantesProdem.TipoinfocodiMWDemanda, ConstantesProdem.LectcodiDemPrevDiario,
                    fecLimite.ToString(ConstantesProdem.FormatoFecha), fecInicio.ToString(ConstantesProdem.FormatoFecha), idTipo,
                    ConstantesProdem.OriglectcodiAgrupacion, ConstantesProdem.PrntDemandaPrevistaAjusteManual);

                //Demanda semanal
                idTipo = string.Join(",",
                new int[] { ConstantesProdem.PrntDemandaSemanalAjusteAuto,
                                ConstantesProdem.PrntDemandaSemanalAjusteManual });
                dataMedicion.AddRange(
                    FactorySic.GetPrnMedicion48Repository().DataHistoricaBarraPMUlibre(-1, idBarraPM,
                    ConstantesProdem.OriglectcodiPR03, ConstantesProdem.TipoinfocodiMWDemanda, ConstantesProdem.LectcodiDemPrevSemanal,
                    fecLimite.ToString(ConstantesProdem.FormatoFecha), fecInicio.ToString(ConstantesProdem.FormatoFecha), idTipo,
                    ConstantesProdem.OriglectcodiAgrupacion, ConstantesProdem.PrntDemandaSemanalAjusteManual)
                    );
            }

            //Aplica las reglas de negocio
            tempDate = regFecha;
            int diaSemana = (int)regFecha.DayOfWeek;
            int maxDias = (int)entConfiguracion.Prncfgnumdiapatron;
            List<int> ruleA = new List<int> { ConstantesProdem.DiaAsIntLunes, ConstantesProdem.DiaAsIntSabado,
                ConstantesProdem.DiaAsIntDomingo };
            if (ruleA.Contains(diaSemana))
            {
                #region Lunes(Mañana), Domingo o Sabado
                int d = 0;
                while (d < maxDias)
                {
                    tempDate = tempDate.AddDays(-7);

                    if (tempDate > fecLimite)
                    {
                        var a = dataMedicion.Find(x => x.Medifecha.Equals(tempDate));
                        if (a != null)
                        {
                            decimal[] dArray = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, a);
                            if (dArray.Sum() > 0)
                            {
                                model.PrnMediciones.Add(a);
                                model.Mediciones.Add(dArray);
                                model.Fechas.Add(a.Medifecha);
                                model.StrFechas.Add(a.Medifecha.ToString(ConstantesProdem.FormatoFecha));
                                model.StrFechasTarde.Add(a.Medifecha.ToString(ConstantesProdem.FormatoFecha));
                                d++;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                model.NDias = d;
                model.Mensaje = "Se capturaron " + d.ToString() + " mediciones historicas";
                #endregion
            }

            tempDate = regFecha;
            List<int> ruleB = new List<int> { ConstantesProdem.DiaAsIntMartes,
                ConstantesProdem.DiaAsIntMiercoles, ConstantesProdem.DiaAsIntJueves, ConstantesProdem.DiaAsIntViernes };
            if (ruleB.Contains(diaSemana))
            {
                #region Lunes(Tarde), Martes, Miercoles, Jueves o Viernes
                int d = 0;
                bool esLunes = false;
                if (diaSemana.Equals(ConstantesProdem.DiaAsIntLunes))
                {
                    esLunes = true;
                    maxDias = model.NDias;
                    model.EsLunes = true;
                    model.StrFechasTarde = new List<string>();
                }

                while (d < maxDias)
                {
                    tempDate = tempDate.AddDays(-1);
                    diaSemana = (int)tempDate.DayOfWeek;
                    while (ruleA.Contains(diaSemana))
                    {
                        tempDate = tempDate.AddDays(-1);
                        diaSemana = (int)tempDate.DayOfWeek;
                    }
                    if (tempDate > fecLimite)
                    {
                        var a = dataMedicion.Find(x => x.Medifecha.Equals(tempDate));
                        if (a != null)
                        {
                            decimal[] dArray = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, a);
                            if (dArray.Sum() > 0)
                            {
                                if (esLunes)
                                {
                                    for (int i = ConstantesProdem.Itv30min / 2; i < ConstantesProdem.Itv30min; i++)
                                    {
                                        model.Mediciones[d][i] = dArray[i];
                                        model.PrnMediciones[d].GetType().GetProperty("H" + (i + 1).ToString()).
                                            SetValue(model.PrnMediciones[d], dArray[i]);
                                    }
                                    model.StrFechasTarde.Add(a.Medifecha.ToString(ConstantesProdem.FormatoFecha));
                                    d++;
                                }
                                else
                                {
                                    model.PrnMediciones.Add(a);
                                    model.Mediciones.Add(dArray);
                                    model.Fechas.Add(a.Medifecha);
                                    model.StrFechas.Add(a.Medifecha.ToString(ConstantesProdem.FormatoFecha));
                                    model.StrFechasTarde.Add(a.Medifecha.ToString(ConstantesProdem.FormatoFecha));
                                    d++;
                                }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (!esLunes)
                {
                    model.NDias = d;
                    model.Mensaje = "Se capturaron " + d.ToString() + " mediciones historicas";
                }
                #endregion
            }

            //Completa las mediciones segun lo configurado
            int j = model.PrnMediciones.Count;
            maxDias = (int)entConfiguracion.Prncfgnumdiapatron;
            if (j != maxDias)
            {
                for (int i = 0; i < maxDias - j; i++)
                {
                    model.Mediciones.Add(new decimal[ConstantesProdem.Itv30min]);
                    model.PrnMediciones.Add(new PrnMedicion48DTO());
                    model.Fechas.Add(DateTime.MinValue);
                    model.StrFechas.Add("No encontrada");
                    model.StrFechasTarde.Add("No encontrada");
                }
            }

            //Calcula el perfil patrón
            model.Patron = UtilProdem.CalcularPerfilPatron(model.Mediciones,
                model.NDias, ConstantesProdem.Itv30min, entConfiguracion.Prncfgtipopatron);

            return model;
        }

        /// <summary>
        /// Devuelve la información de usuarios libres de una barra PM
        /// </summary>
        /// <param name="idBarraPM">Identificador de la tabla PR_GRUPO corresponde a una barra PM</param>
        /// <param name="dGauss">Valor del factor gauss asignado a la barra PM</param>
        /// <param name="dPerdida">Valor de la pérdida asignada a la barra PM</param>
        /// <param name="regFecha">Fecha de busqueda</param>
        /// <returns></returns>
        public PrnMedicion48DTO GetDataBarraPMUlibrePorDia(int idBarraPM, decimal dGauss, decimal dPerdida, DateTime regFecha)
        {
            string idTipo = "-1";
            PrnMedicion48DTO entity = new PrnMedicion48DTO();

            //1. Obtiene la información Prevista diaria
            idTipo = string.Join(",",
                new int[] { ConstantesProdem.PrntDemandaPrevistaAjusteAuto,
                                ConstantesProdem.PrntDemandaPrevistaAjusteManual });
            entity = FactorySic.GetPrnMedicion48Repository().
                DataBarraPMUlibrePorDia(idBarraPM, dGauss, dPerdida, ConstantesProdem.OriglectcodiPR03,
                ConstantesProdem.TipoinfocodiMWDemanda, ConstantesProdem.LectcodiDemPrevDiario,
                regFecha.ToString(ConstantesProdem.FormatoFecha), idTipo,
                ConstantesProdem.OriglectcodiAgrupacion, ConstantesProdem.PrntDemandaPrevistaAjusteManual);

            //2. Si no existe información Prevista diaria se busca Prevista Semanal
            if (entity.Ptomedicodi.Equals(-1))
            {
                entity = new PrnMedicion48DTO();

                idTipo = string.Join(",",
                new int[] { ConstantesProdem.PrntDemandaSemanalAjusteAuto,
                                ConstantesProdem.PrntDemandaSemanalAjusteManual });
                entity = FactorySic.GetPrnMedicion48Repository().
                    DataBarraPMUlibrePorDia(idBarraPM, dGauss, dPerdida, ConstantesProdem.OriglectcodiPR03,
                    ConstantesProdem.TipoinfocodiMWDemanda, ConstantesProdem.LectcodiDemPrevSemanal,
                    regFecha.ToString(ConstantesProdem.FormatoFecha), idTipo,
                    ConstantesProdem.OriglectcodiAgrupacion, ConstantesProdem.PrntDemandaSemanalAjusteManual);
            }

            //3. Si no existe información Prevista Semanal se busca el patron defecto
            if (entity.Ptomedicodi.Equals(-1))
            {
                int idDefecto = UtilProdem.ValidarPatronDiaDefecto(regFecha);
                entity = new PrnMedicion48DTO();
                entity = FactorySic.GetPrnMedicion48Repository().DataPtrBarraPMUlibrePorDia(idBarraPM,
                    dGauss, dPerdida, ConstantesProdem.OriglectcodiPR03, ConstantesProdem.DefectoDate,
                    idDefecto, ConstantesProdem.OriglectcodiAgrupacion);
            }

            return entity;
        }

        //Parametros Barras
        public List<PrnMediciongrpDTO> GetParametrosBarras(string barrapm, int tipo, string barracp)
        {

            return FactorySic.GetPrnMediciongrpRepository().ListBarraBy(barrapm, tipo, barracp);
        }

        /// <summary>
        /// Devuelve el perfil patrón defecto el cual se utiliza si no existe el perfil normal
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro a buscar</param>
        /// <returns></returns>
        public decimal[] GetPatronDefecto(int idPunto, DateTime regFecha)
        {
            DateTime defDate = DateTime.ParseExact(ConstantesProdem.DefectoDate,
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            PrnMedicion48DTO regMedicion = this.GetByIdPrnMedicion48(idPunto,
                UtilProdem.ValidarPatronDiaDefecto(regFecha), defDate);

            return UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, regMedicion);
        }

        /// <summary>
        /// Devuelve el perfil patrón defecto (como entidad) el cual se utiliza si no existe el perfil normal
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro a buscar</param>
        /// <returns></returns>
        public PrnMedicion48DTO GetEnt48PatronDefecto(int idPunto, DateTime regFecha)
        {
            DateTime defDate = DateTime.ParseExact(ConstantesProdem.DefectoDate,
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            return this.GetByIdPrnMedicion48(idPunto,
                UtilProdem.ValidarPatronDiaDefecto(regFecha), defDate);
        }

        /// <summary>
        /// Devuelve el nombre compuesto de un punto de medición
        /// </summary>
        /// <param name="idPunto">Pk del punto de medición</param>
        /// <returns></returns>
        public string GetNombrePtomedicion(int idPunto)
        {
            return FactorySic.GetPrnMedicion48Repository().GetNombrePtomedicion(idPunto);
        }

        /// <summary>
        /// Devuelve los valores que forman la demanda de un punto de medición
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="idLectura">Identificador de la tabla ME_LECTURA</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns>
        /// Count 0: No existen datos
        /// Prnm48tipo = 0: Demanda segun información del punto (final)
        /// Prnm48tipo = 1: Demanda segun información reportada por el agente
        /// Prnm48tipo = 2: Posible ajuste automático realizado por el sistema
        /// Prnm48tipo = 3: Posible ajuste manual realizado por el usuario
        /// </returns>
        public List<PrnMedicion48DTO> GetDemandaPorPunto(int idPunto, int idLectura, string regFecha)
        {
            PrnMedicion48DTO entity = new PrnMedicion48DTO();
            List<PrnMedicion48DTO> dataDemanda = new List<PrnMedicion48DTO>();

            //Obtiene los datos de la demanda por tipo de información
            switch (idLectura)
            {
                case ConstantesProdem.LectcodiDemEjecDiario:
                    dataDemanda = FactorySic.GetPrnMedicion48Repository().
                        GetDemandaPorPunto(idPunto, idLectura,
                        ConstantesProdem.PrntDemandaEjecutadaAjusteAuto,
                        ConstantesProdem.PrntDemandaEjecutadaAjusteManual,
                        regFecha, ConstantesProdem.TipoinfocodiMWDemanda);
                    break;
                case ConstantesProdem.LectcodiDemPrevDiario:
                    dataDemanda = FactorySic.GetPrnMedicion48Repository().
                        GetDemandaPorPunto(idPunto, idLectura,
                        ConstantesProdem.PrntDemandaPrevistaAjusteAuto,
                        ConstantesProdem.PrntDemandaPrevistaAjusteManual,
                        regFecha, ConstantesProdem.TipoinfocodiMWDemanda);
                    break;
                case ConstantesProdem.LectcodiDemPrevSemanal:
                    dataDemanda = FactorySic.GetPrnMedicion48Repository().
                        GetDemandaPorPunto(idPunto, idLectura,
                        ConstantesProdem.PrntDemandaSemanalAjusteAuto,
                        ConstantesProdem.PrntDemandaSemanalAjusteManual,
                        regFecha, ConstantesProdem.TipoinfocodiMWDemanda);
                    break;
            }

            //Calcula la demanda ejecutada del punto
            if (dataDemanda.Count != 0)
            {
                entity.Ptomedicodi = idPunto;
                entity.Prnm48tipo = 0;
                entity.Medifecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

                int i = 0;
                while (i < ConstantesProdem.Itv30min)
                {
                    decimal dValor = 0;
                    foreach (PrnMedicion48DTO e in dataDemanda)
                    {
                        var valid = e.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(e, null) ?? (decimal)0;
                        dValor += (decimal)valid;
                    }

                    entity.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(entity, dValor, null);
                    i++;
                }

                dataDemanda.Add(entity);
            }

            return dataDemanda;
        }

        /// <summary>
        /// Devuelve los valores que forman la demanda de una agrupación
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="idLectura">Identificador de la tabla ME_LECTURA</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns>
        /// Count 0: No existen datos
        /// Prnm48tipo = 0: Demanda ejecutada del punto (final)
        /// Prnm48tipo = 1: Demanda ejecutada reportada por el agente
        /// Prnm48tipo = 2: Posible ajuste manual realizado por el usuario
        /// </returns>
        public List<PrnMedicion48DTO> GetDemandaPorAgrupacion(int idPunto, int idLectura, string regFecha)
        {
            string idTipo = "-1";
            PrnMedicion48DTO entity = new PrnMedicion48DTO();
            List<PrnMedicion48DTO> dataDemanda = new List<PrnMedicion48DTO>();

            //Obtiene los datos de la demanda por tipo de información
            switch (idLectura)
            {
                case ConstantesProdem.LectcodiDemEjecDiario:
                    idTipo = string.Join(",",
                            new int[] { ConstantesProdem.PrntDemandaEjecutadaAjusteAuto,
                                ConstantesProdem.PrntDemandaEjecutadaAjusteManual });

                    dataDemanda = FactorySic.GetPrnMedicion48Repository().
                        GetDemandaPorAgrupacion(ConstantesProdem.OriglectcodiAgrupacion, ConstantesProdem.OriglectcodiPR03,
                        ConstantesProdem.LectcodiDemEjecDiario, ConstantesProdem.TipoinfocodiMWDemanda, regFecha, idTipo,
                        ConstantesProdem.PrntDemandaEjecutadaAjusteManual, idPunto);
                    break;
                case ConstantesProdem.LectcodiDemPrevDiario:
                    idTipo = string.Join(",",
                            new int[] { ConstantesProdem.PrntDemandaPrevistaAjusteAuto,
                                ConstantesProdem.PrntDemandaPrevistaAjusteManual });

                    dataDemanda = FactorySic.GetPrnMedicion48Repository().
                        GetDemandaPorAgrupacion(ConstantesProdem.OriglectcodiAgrupacion, ConstantesProdem.OriglectcodiPR03,
                        ConstantesProdem.LectcodiDemPrevDiario, ConstantesProdem.TipoinfocodiMWDemanda, regFecha, idTipo,
                        ConstantesProdem.PrntDemandaPrevistaAjusteManual, idPunto);
                    break;
                case ConstantesProdem.LectcodiDemPrevSemanal:
                    idTipo = string.Join(",",
                            new int[] { ConstantesProdem.PrntDemandaSemanalAjusteAuto,
                                ConstantesProdem.PrntDemandaSemanalAjusteManual });

                    dataDemanda = FactorySic.GetPrnMedicion48Repository().
                        GetDemandaPorAgrupacion(ConstantesProdem.OriglectcodiAgrupacion, ConstantesProdem.OriglectcodiPR03,
                        ConstantesProdem.LectcodiDemPrevSemanal, ConstantesProdem.TipoinfocodiMWDemanda, regFecha, idTipo,
                        ConstantesProdem.PrntDemandaSemanalAjusteManual, idPunto);
                    break;
            }

            //Calcula la demanda ejecutada del punto
            if (dataDemanda.Count != 0)
            {
                entity.Ptomedicodi = idPunto;
                entity.Prnm48tipo = 0;
                entity.Medifecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

                int i = 0;
                while (i < ConstantesProdem.Itv30min)
                {
                    decimal dValor = 0;
                    foreach (PrnMedicion48DTO e in dataDemanda)
                    {
                        var valid = e.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(e, null) ?? (decimal)0;
                        dValor += (decimal)valid;
                    }

                    entity.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(entity, dValor, null);
                    i++;
                }

                dataDemanda.Add(entity);
            }

            return dataDemanda;
        }

        /// <summary>
        /// Devuelve los valores que forman la demanda ejecutada de un punto de medición
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns>
        /// Count 0: No existen datos
        /// Prnm48tipo = 0: Demanda ejecutada del punto (final)
        /// Prnm48tipo = 1: Demanda ejecutada reportada por el agente
        /// Prnm48tipo = 2: Posible ajuste automático realizado por el sistema
        /// Prnm48tipo = 3: Posible ajuste manual realizado por el usuario
        /// </returns>
        public List<PrnMedicion48DTO> GetDemandaEjecutadaPorPunto(int idPunto, string regFecha)
        {
            PrnMedicion48DTO entity = new PrnMedicion48DTO();

            //Obtiene los datos de la demanda ejecutada por punto
            List<PrnMedicion48DTO> dataDemanda = FactorySic.GetPrnMedicion48Repository().
                GetDemandaEjecutadaPorPunto(idPunto, ConstantesProdem.LectcodiDemEjecDiario,
                ConstantesProdem.PrntDemandaEjecutadaAjusteAuto, ConstantesProdem.PrntDemandaEjecutadaAjusteManual,
                regFecha, ConstantesProdem.TipoinfocodiMWDemanda);

            //Calcula la demanda ejecutada del punto
            if (dataDemanda.Count != 0)
            {
                entity.Ptomedicodi = idPunto;
                entity.Prnm48tipo = 0;
                entity.Medifecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

                int i = 0;
                while (i < ConstantesProdem.Itv30min)
                {
                    decimal dValor = 0;
                    foreach (PrnMedicion48DTO e in dataDemanda)
                    {
                        var valid = e.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(e, null) ?? (decimal)0;
                        dValor += (decimal)valid;
                    }

                    entity.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(entity, dValor, null);
                    i++;
                }

                dataDemanda.Add(entity);
            }

            return dataDemanda;
        }

        /// <summary>
        /// Devuelve los valores que forman la demanda ejecutada de una agrupación
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns>
        /// Count 0: No existen datos
        /// Prnm48tipo = 0: Demanda ejecutada del punto (final)
        /// Prnm48tipo = 1: Demanda ejecutada reportada por el agente
        /// Prnm48tipo = 2: Posible ajuste manual realizado por el usuario
        /// </returns>
        public List<PrnMedicion48DTO> GetDemandaEjecutadaPorAgrupacion(int idPunto, string regFecha)
        {
            PrnMedicion48DTO entity = new PrnMedicion48DTO();

            string idTipo = string.Join(",",
                            new int[] { ConstantesProdem.PrntDemandaEjecutadaAjusteAuto,
                                ConstantesProdem.PrntDemandaEjecutadaAjusteManual });

            //Obtiene los datos de la demanda ejecutada por punto
            List<PrnMedicion48DTO> dataDemanda = FactorySic.GetPrnMedicion48Repository().
                GetDemandaEjecutadaPorAgrupacion(ConstantesProdem.OriglectcodiAgrupacion, ConstantesProdem.OriglectcodiPR03,
                ConstantesProdem.LectcodiDemEjecDiario, ConstantesProdem.TipoinfocodiMWDemanda, regFecha, idTipo,
                ConstantesProdem.PrntDemandaEjecutadaAjusteManual, idPunto);

            //Calcula la demanda ejecutada del punto
            if (dataDemanda.Count != 0)
            {
                entity.Ptomedicodi = idPunto;
                entity.Prnm48tipo = 0;
                entity.Medifecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

                int i = 0;
                while (i < ConstantesProdem.Itv30min)
                {
                    decimal dValor = 0;
                    foreach (PrnMedicion48DTO e in dataDemanda)
                    {
                        var valid = e.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(e, null) ?? (decimal)0;
                        dValor += (decimal)valid;
                    }

                    entity.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(entity, dValor, null);
                    i++;
                }

                dataDemanda.Add(entity);
            }

            return dataDemanda;
        }

        /// <summary>
        /// Devuelve los valores que forman la demanda prevista de un punto de medición
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns>
        /// Count 0: No existen datos
        /// Prnm48tipo = 0: Demanda prevista del punto (final)
        /// Prnm48tipo = 1: Demanda prevista reportada por el agente
        /// Prnm48tipo = 2: Posible ajuste manual realizado por el usuario
        /// </returns>
        public List<PrnMedicion48DTO> GetDemandaPrevistaPorPunto(int idPunto, string regFecha)
        {
            PrnMedicion48DTO entity = new PrnMedicion48DTO();

            //Obtiene los datos de la demanda ejecutada por punto
            List<PrnMedicion48DTO> dataDemanda = FactorySic.GetPrnMedicion48Repository().
                GetDemandaPrevistaPorPunto(idPunto, ConstantesProdem.LectcodiDemPrevDiario,
                ConstantesProdem.PrntDemandaPrevistaAjusteManual, regFecha, ConstantesProdem.TipoinfocodiMWDemanda);

            //Calcula la demanda ejecutada del punto
            if (dataDemanda.Count != 0)
            {
                entity.Ptomedicodi = idPunto;
                entity.Prnm48tipo = 0;
                entity.Medifecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

                int i = 0;
                while (i < ConstantesProdem.Itv30min)
                {
                    decimal dValor = 0;
                    foreach (PrnMedicion48DTO e in dataDemanda)
                    {
                        var valid = e.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(e, null) ?? (decimal)0;
                        dValor += (decimal)valid;
                    }

                    entity.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(entity, dValor, null);
                    i++;
                }

                dataDemanda.Add(entity);
            }

            return dataDemanda;
        }

        /// <summary>
        /// Devuelve los valores que forman la demanda prevista de una agrupación
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns>
        /// Count 0: No existen datos
        /// Prnm48tipo = 0: Demanda ejecutada del punto (final)
        /// Prnm48tipo = 1: Demanda prevista reportada por el agente
        /// Prnm48tipo = 2: Posible ajuste manual realizado por el usuario
        /// </returns>
        public List<PrnMedicion48DTO> GetDemandaPrevistaPorAgrupacion(int idPunto, string regFecha)
        {
            PrnMedicion48DTO entity = new PrnMedicion48DTO();

            //Obtiene los datos de la demanda ejecutada por punto
            List<PrnMedicion48DTO> dataDemanda = FactorySic.GetPrnMedicion48Repository().
                GetDemandaPrevistaPorAgrupacion(ConstantesProdem.OriglectcodiAgrupacion, ConstantesProdem.OriglectcodiPR03,
                ConstantesProdem.LectcodiDemPrevDiario, ConstantesProdem.TipoinfocodiMWDemanda, regFecha, ConstantesProdem.PrntDemandaPrevistaAjusteManual,
                ConstantesProdem.PrntDemandaPrevistaAjusteManual, idPunto);

            //Calcula la demanda ejecutada del punto
            if (dataDemanda.Count != 0)
            {
                entity.Ptomedicodi = idPunto;
                entity.Prnm48tipo = 0;
                entity.Medifecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

                int i = 0;
                while (i < ConstantesProdem.Itv30min)
                {
                    decimal dValor = 0;
                    foreach (PrnMedicion48DTO e in dataDemanda)
                    {
                        var valid = e.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(e, null) ?? (decimal)0;
                        dValor += (decimal)valid;
                    }

                    entity.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(entity, dValor, null);
                    i++;
                }

                dataDemanda.Add(entity);
            }

            return dataDemanda;
        }

        /// <summary>
        /// Devuelve los valores que forman la demanda prevista de un punto de medición
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns>
        /// Count 0: No existen datos
        /// Prnm48tipo = 0: Demanda prevista semanal del punto (final)
        /// Prnm48tipo = 1: Demanda prevista semanal reportada por el agente
        /// </returns>
        public List<PrnMedicion48DTO> GetDemandaPrevistaSemanalPorPunto(int idPunto, string regFecha)
        {
            PrnMedicion48DTO entity = new PrnMedicion48DTO();

            //Obtiene los datos de la demanda ejecutada por punto
            List<PrnMedicion48DTO> dataDemanda = FactorySic.GetPrnMedicion48Repository().
                GetDemandaPrevistaPorPunto(idPunto, ConstantesProdem.LectcodiDemPrevSemanal, -1,
                regFecha, ConstantesProdem.TipoinfocodiMWDemanda);

            //Calcula la demanda ejecutada del punto
            if (dataDemanda.Count != 0)
            {
                entity.Ptomedicodi = idPunto;
                entity.Prnm48tipo = 0;
                entity.Medifecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

                int i = 0;
                while (i < ConstantesProdem.Itv30min)
                {
                    decimal dValor = 0;
                    foreach (PrnMedicion48DTO e in dataDemanda)
                    {
                        dValor += (decimal)e.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(e, null);
                    }

                    entity.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(entity, dValor, null);
                    i++;
                }

                dataDemanda.Add(entity);
            }

            return dataDemanda;
        }

        /// <summary>
        /// Devuelve los valores que forman la demanda ejecutada (despacho mas flujos de linea) de un área operativa
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="idTipoinfo">Identificador del tipo de información a buscar (Ejecutada{1} o Prevista{2})</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns>
        /// Count 0: No existen datos
        /// Prnm48tipo = 0: Demanda del área (final)
        /// Prnm48tipo = 1: Despacho ejecutado del área
        /// Prnm48tipo = ConstanteProdem.PrntMant(X): Mantenimientos registrados
        /// Prnm48tipo = ConstanteProdem.PrntFalla(X): Fallas registradas
        /// Prnm48tipo = ConstanteProdem.PrntDemandaAreaAjuste: Ajustes realizados
        /// Prnm48tipo = ConstanteProdem.PrntFlujoLinea: Flujo de linea (*Centro: caso especial)
        /// </returns>
        public List<PrnMedicion48DTO> GetDemandaPorArea(int idPunto, int idTipoinfo, string regFecha)
        {
            PrnMedicion48DTO entity = new PrnMedicion48DTO();
            PrnMedicion48DTO dataFlujo = new PrnMedicion48DTO();
            List<PrnMedicion48DTO> dataDemanda = new List<PrnMedicion48DTO>();

            //Obtiene los datos desagregados de cada área
            switch (idPunto)
            {
                case ConstantesProdem.PtomedicodiASur:
                    {
                        #region Para el área operativa Sur
                        string idTipo = string.Empty;
                        if (idTipoinfo == 1)
                        {
                            //Información ejecutada
                            idTipo = string.Join(",",
                                new int[] { ConstantesProdem.PrntMantSur,
                                ConstantesProdem.PrntFallaSur,
                                ConstantesProdem.PrntDemandaAreaAjuste
                                });
                        }
                        else
                        {
                            //Información prevista
                            idTipo = string.Join(",",
                               new int[] { ConstantesProdem.PrntMantSurPrevisto,
                                ConstantesProdem.PrntFallaSurPrevisto,
                                ConstantesProdem.PrntDemandaAreaAjuste
                               });
                        }

                        dataDemanda = FactorySic.GetPrnMedicion48Repository().
                            GetDespachoEjecutadoPorArea(idPunto, ConstantesProdem.AreacodiASur, regFecha, idTipo);

                        //Agrega el flujo ideal del área
                        dataDemanda.Add(this.GetFlujoIdealValidado(idPunto, regFecha));
                        #endregion
                    }
                    break;
                case ConstantesProdem.PtomedicodiANorte:
                    {
                        #region Para el área operativa Norte
                        string idTipo = string.Empty;
                        if (idTipoinfo == 1)
                        {
                            //Información ejecutada
                            idTipo = string.Join(",",
                                new int[] { ConstantesProdem.PrntMantNorte,
                                ConstantesProdem.PrntFallaNorte,
                                ConstantesProdem.PrntDemandaAreaAjuste
                                });
                        }
                        else
                        {
                            //Información prevista
                            idTipo = string.Join(",",
                               new int[] { ConstantesProdem.PrntMantNortePrevisto,
                                ConstantesProdem.PrntFallaNortePrevisto,
                                ConstantesProdem.PrntDemandaAreaAjuste
                               });
                        }

                        dataDemanda = FactorySic.GetPrnMedicion48Repository().
                            GetDespachoEjecutadoPorArea(idPunto, ConstantesProdem.AreacodiANorte, regFecha, idTipo);

                        //Agrega el flujo ideal del área
                        dataDemanda.Add(this.GetFlujoIdealValidado(idPunto, regFecha));
                        #endregion
                    }
                    break;
                case ConstantesProdem.PtomedicodiASierraCentro:
                    {
                        #region Para el área operativa Sierra Centro
                        string idTipo = string.Empty;
                        if (idTipoinfo == 1)
                        {
                            //Información ejecutada
                            idTipo = string.Join(",",
                                new int[] { ConstantesProdem.PrntMantSierraCentro,
                                ConstantesProdem.PrntFallaSierraCentro,
                                ConstantesProdem.PrntDemandaAreaAjuste
                                });
                        }
                        else
                        {
                            //Información prevista
                            idTipo = string.Join(",",
                               new int[] { ConstantesProdem.PrntMantSierraCentroPrevisto,
                                ConstantesProdem.PrntFallaSierracentroPrevisto,
                                ConstantesProdem.PrntDemandaAreaAjuste
                               });
                        }

                        dataDemanda = FactorySic.GetPrnMedicion48Repository().
                            GetDespachoEjecutadoPorArea(idPunto, ConstantesProdem.AreacodiASierraCentro, regFecha, idTipo);

                        //Agrega el flujo ideal del área
                        dataDemanda.Add(this.GetFlujoIdealValidado(idPunto, regFecha));
                        #endregion
                    }
                    break;
                case ConstantesProdem.PtomedicodiACentro:
                    {
                        #region Para el área operativa Centro
                        string idTipo = string.Empty;
                        if (idTipoinfo == 1)
                        {
                            //Información ejecutada
                            idTipo = string.Join(",",
                                new int[] { ConstantesProdem.PrntMantCentro,
                                ConstantesProdem.PrntFallaCentro,
                                ConstantesProdem.PrntDemandaAreaAjuste
                                });
                        }
                        else
                        {
                            //Información prevista
                            idTipo = string.Join(",",
                               new int[] { ConstantesProdem.PrntMantCentroPrevisto,
                                ConstantesProdem.PrntFallaCentroPrevisto,
                                ConstantesProdem.PrntDemandaAreaAjuste
                               });
                        }

                        dataDemanda = FactorySic.GetPrnMedicion48Repository().
                            GetDespachoEjecutadoPorArea(idPunto, ConstantesProdem.AreacodiACentro, regFecha, idTipo);

                        //Obtiene los flujos de las otras áreas (estos se restan al Centro)
                        decimal[] fljSur = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetFlujoIdealValidado(ConstantesProdem.PtomedicodiASur, regFecha));
                        decimal[] fljNorte = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetFlujoIdealValidado(ConstantesProdem.PtomedicodiANorte, regFecha));
                        decimal[] fljSCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                            this.GetFlujoIdealValidado(ConstantesProdem.PtomedicodiASierraCentro, regFecha));

                        decimal dValor = 0;
                        PrnMedicion48DTO fljCentro = new PrnMedicion48DTO { Prnm48tipo = ConstantesProdem.PrntFlujoLinea };
                        for (int i = 0; i < ConstantesProdem.Itv30min; i++)
                        {
                            dValor = 0;
                            dValor = (fljSur[i] + fljNorte[i] + fljSCentro[i]) * -1;
                            fljCentro.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(fljCentro, dValor);
                        }
                        dataDemanda.Add(fljCentro);

                        //Obtiene los ajustes de las otras áreas (estos se aplican al centro de manera inversa)
                        List<PrnMedicion48DTO> ajsAreas = this.GetAjusteAlCentroPorTipo(regFecha, ConstantesProdem.PrntDemandaAreaAjuste);
                        foreach (PrnMedicion48DTO a in ajsAreas)
                        {
                            dataDemanda.Add(a);
                        }
                        #endregion
                    }
                    break;
            }

            //Calcula la demanda total
            if (dataDemanda.Count != 0)
            {
                entity.Ptomedicodi = idPunto;
                entity.Prnm48tipo = 0;
                entity.Medifecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

                int i = 0;
                while (i < ConstantesProdem.Itv30min)
                {
                    decimal dValor = 0;
                    foreach (PrnMedicion48DTO e in dataDemanda)
                    {
                        dValor += (decimal)(e.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(e, null) ?? 0M);
                    }

                    entity.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(entity, dValor, null);
                    i++;
                }

                dataDemanda.Add(entity);
            }

            return dataDemanda;
        }

        /// <summary>
        /// Devuelve los valores que forman la demanda vegtativa (demanda - usuarios libres) de un área operativa
        /// </summary>
        /// <param name="idPunto">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="idTipoinfo">Identificador del tipo de información a buscar (Ejecutada{1} o Prevista{2})</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns>
        /// Prnm48tipo = 0: Demanda vegetativa del área (final)
        /// Prnm48tipo = 1: Demanda del área (final)
        /// Prnm48tipo = ConstanteProdem.PrntDemandaULibre: Usuarios libres
        /// Prnm48tipo = ConstanteProdem.PrntDemandaVegetativaAjuste: Ajustes realizados
        /// </returns>
        public List<PrnMedicion48DTO> GetVegetativaPorArea(int idPunto, int idTipoinfo, string regFecha)
        {
            PrnMedicion48DTO entity = new PrnMedicion48DTO();
            PrnMedicion48DTO dataFlujo = new PrnMedicion48DTO();
            List<PrnMedicion48DTO> dataVegetativa = new List<PrnMedicion48DTO>();

            DateTime parseFecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            //Obtiene los datos desagregados de cada área
            switch (idPunto)
            {
                case ConstantesProdem.PtomedicodiASur:
                    {
                        #region Para el área operativa Sur
                        //Obtiene la demanda del área
                        PrnMedicion48DTO entDemanda = this.GetDemandaPorArea(idPunto, idTipoinfo, regFecha).
                            FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        entDemanda.Prnm48tipo = 1;
                        dataVegetativa.Add(entDemanda);

                        //Agrega los ajustes realizados al área
                        PrnMedicion48DTO entAjuste = this.GetByIdPrnMedicion48(idPunto, ConstantesProdem.PrntDemandaVegetativaAjuste, parseFecha);
                        entAjuste.Prnm48tipo = ConstantesProdem.PrntDemandaVegetativaAjuste;
                        dataVegetativa.Add(entAjuste);

                        //Agrega la demanda de los usuarios libres
                        dataVegetativa.Add(this.GetDemandaULibresPorArea(ConstantesProdem.AreacodiASur, regFecha, 1));
                        #endregion
                    }
                    break;
                case ConstantesProdem.PtomedicodiANorte:
                    {
                        #region Para el área operativa Norte
                        //Obtiene la demanda del área
                        PrnMedicion48DTO entDemanda = this.GetDemandaPorArea(idPunto, idTipoinfo, regFecha).
                            FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        entDemanda.Prnm48tipo = 1;
                        dataVegetativa.Add(entDemanda);

                        //Agrega los ajustes realizados al área
                        PrnMedicion48DTO entAjuste = this.GetByIdPrnMedicion48(idPunto, ConstantesProdem.PrntDemandaVegetativaAjuste, parseFecha);
                        entAjuste.Prnm48tipo = ConstantesProdem.PrntDemandaVegetativaAjuste;
                        dataVegetativa.Add(entAjuste);

                        //Agrega la demanda de los usuarios libres
                        dataVegetativa.Add(this.GetDemandaULibresPorArea(ConstantesProdem.AreacodiANorte, regFecha, 1));
                        #endregion
                    }
                    break;
                case ConstantesProdem.PtomedicodiASierraCentro:
                    {
                        #region Para el área operativa Sierra Centro
                        //Obtiene la demanda del área
                        PrnMedicion48DTO entDemanda = this.GetDemandaPorArea(idPunto, idTipoinfo, regFecha).
                            FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        entDemanda.Prnm48tipo = 1;
                        dataVegetativa.Add(entDemanda);

                        //Agrega los ajustes realizados al área
                        PrnMedicion48DTO entAjuste = this.GetByIdPrnMedicion48(idPunto, ConstantesProdem.PrntDemandaVegetativaAjuste, parseFecha);
                        entAjuste.Prnm48tipo = ConstantesProdem.PrntDemandaVegetativaAjuste;
                        dataVegetativa.Add(entAjuste);

                        //Agrega la demanda de los usuarios libres
                        dataVegetativa.Add(this.GetDemandaULibresPorArea(ConstantesProdem.AreacodiASierraCentro, regFecha, 1));
                        #endregion
                    }
                    break;
                case ConstantesProdem.PtomedicodiACentro:
                    {
                        #region Para el área operativa Centro
                        //Obtiene la demanda del área
                        PrnMedicion48DTO entDemanda = this.GetDemandaPorArea(idPunto, idTipoinfo, regFecha).
                            FirstOrDefault(x => x.Prnm48tipo == 0) ?? new PrnMedicion48DTO();
                        entDemanda.Prnm48tipo = 1;
                        dataVegetativa.Add(entDemanda);

                        //Agrega los ajustes realizados al área
                        PrnMedicion48DTO entAjuste = this.GetByIdPrnMedicion48(idPunto, ConstantesProdem.PrntDemandaVegetativaAjuste, parseFecha);
                        entAjuste.Prnm48tipo = ConstantesProdem.PrntDemandaVegetativaAjuste;
                        dataVegetativa.Add(entAjuste);

                        //Agrega la demanda de los usuarios libres
                        dataVegetativa.Add(this.GetDemandaULibresPorArea(ConstantesProdem.AreacodiACentro, regFecha, 1));

                        //Obtiene los ajustes de las otras áreas al centro
                        List<PrnMedicion48DTO> ajsAreas = this.GetAjusteAlCentroPorTipo(regFecha, ConstantesProdem.PrntDemandaVegetativaAjuste);
                        foreach (PrnMedicion48DTO a in ajsAreas)
                        {
                            dataVegetativa.Add(a);
                        }
                        #endregion
                    }
                    break;
            }

            //Calcula la demanda total
            if (dataVegetativa.Count != 0)
            {
                entity.Ptomedicodi = idPunto;
                entity.Prnm48tipo = 0;
                entity.Medifecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

                int i = 0;
                while (i < ConstantesProdem.Itv30min)
                {
                    decimal dValor = 0;
                    foreach (PrnMedicion48DTO e in dataVegetativa)
                    {
                        if (e.Prnm48tipo == ConstantesProdem.PrntDemandaULibre)
                        {
                            dValor -= (decimal)(e.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(e, null) ?? 0M);
                        }
                        else
                        {
                            dValor += (decimal)(e.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(e, null) ?? 0M);
                        }
                    }

                    entity.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(entity, dValor, null);
                    i++;
                }

                dataVegetativa.Add(entity);
            }

            return dataVegetativa;
        }

        /// <summary>
        /// Devuelve el flujo segun prioridad y corregido relacionado a un área operativa
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public PrnMedicion48DTO GetFlujoIdealValidado(int idArea, string regFecha)
        {
            PrnMedicion48DTO entity = new PrnMedicion48DTO();
            DateTime parseFecha = DateTime.ParseExact(regFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            //Obtiene los flujos(formulas) relacionados y los ordena segun prioridad (asc)
            List<PrnFormularelDTO> listRelacionados = this.ListFormulasRelacionadas(idArea).
                OrderBy(x => x.Prfrelfactor).ToList();

            //Validacion
            if (listRelacionados.Count == 0) return entity;

            //Obtiene los datos del flujo y solo mantiene los flujos que poseen energía
            List<decimal[]> dataMediciones = new List<decimal[]>();
            List<PrnFormularelDTO> dataEntitys = new List<PrnFormularelDTO>();

            foreach (PrnFormularelDTO a in listRelacionados)
            {
                decimal[] dMedicion = this.ObtenerMedicionesCalculadas(a.Ptomedicodicalc, parseFecha);
                if (dMedicion.Sum() > 0)
                {
                    dataEntitys.Add(a);
                    dataMediciones.Add(dMedicion);
                }
            }

            //Validación - No existen flujos relacionados
            if (dataEntitys.Count == 0)
            {
                return entity = new PrnMedicion48DTO
                {
                    Ptomedicodi = listRelacionados.First().Ptomedicodicalc,
                    Ptomedidesc = listRelacionados.First().Ptomedidesc,
                    Prnm48tipo = ConstantesProdem.PrntFlujoLinea
                };
            }

            //Obtiene la cantidad de intervalos menores a 0 por cada medición
            int y = 0;
            List<int> dataMinus = new List<int>();
            foreach (decimal[] med in dataMediciones)
            {
                y = 0;
                foreach (decimal m in med)
                {
                    if (m < 0) y++;
                }
                dataMinus.Add(y);
            }

            //Conserva el flujo con menor cantidad de 0
            int idx = 0;
            int dMin = ConstantesProdem.Itv30min;
            for (int i = 0; i < dataMinus.Count; i++)
            {
                if (dataMinus[i] < dMin)
                {
                    dMin = dataMinus[i];
                    idx = i;
                }
            }

            PrnFormularelDTO selEnt = dataEntitys[idx];
            decimal[] selMed = dataMediciones[idx];

            //Reconstruye los intervalos con potencia <= 0 (puntuales) si fuese necesario
            decimal dA, dB;
            if (selMed.Sum() > 0)
            {
                entity.ListaIntervalos = new List<int>();
                for (int i = 0; i < selMed.Length; i++)
                {
                    dA = 0; dB = 0;
                    if (selMed[i] <= 0)
                    {
                        //Busca el intervalo siguiente diferente a 0
                        int next = i;
                        while (next < selMed.Length)
                        {
                            if (selMed[next] > 0)
                            {
                                dA = selMed[next];
                                break;
                            }
                            next++;
                        }

                        //Busca el intervalo anterior diferente a 0
                        int prev = i;
                        while (prev > 0)
                        {
                            if (selMed[prev] > 0)
                            {
                                dB = selMed[prev];
                                break;
                            }
                            prev--;
                        }

                        //Calcula el nuevo valor y lo reemplaza
                        selMed[i] = (dA + dB) / 2;

                        //Almacena los intervalos corregidos
                        entity.ListaIntervalos.Add(i);
                    }
                }
            }

            //Crea la entidad
            entity.Ptomedicodi = selEnt.Ptomedicodicalc;
            entity.Ptomedidesc = selEnt.Ptomedidesc;
            entity.Prnm48tipo = ConstantesProdem.PrntFlujoLinea;

            for (int i = 0; i < ConstantesProdem.Itv30min; i++)
            {
                entity.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(entity, selMed[i]);
            }

            return entity;
        }

        /// <summary>
        /// Devuelve los ajustes de las áreas operativas los cuales se aplican de manera inversa al área Centro por tipo
        /// </summary>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="idTipo">Identificador Prnm48tipo de la tabla PRN_MEDICION48 (ajuste demanda, ajuste vegetativa)</param>
        /// <returns></returns>
        public List<PrnMedicion48DTO> GetAjusteAlCentroPorTipo(string regFecha, int idTipo)
        {
            string idPunto = string.Join(",", new int[] {
                ConstantesProdem.PtomedicodiASur,
                ConstantesProdem.PtomedicodiANorte,
                ConstantesProdem.PtomedicodiASierraCentro
            });

            return FactorySic.GetPrnMedicion48Repository().GetAjusteAlCentroPorTipo(idPunto, regFecha, idTipo);
        }

        /// <summary>
        /// Devuelve los ajustes sumados de las áreas operativas los cuales se aplican de manera inversa al área Centro por tipo
        /// </summary>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="idTipo">Identificador Prnm48tipo de la tabla PRN_MEDICION48 (ajuste demanda, ajuste vegetativa)</param>
        /// <returns></returns>
        public decimal[] GetAjusteAlCentroPorTipoSum(string regFecha, int idTipo)
        {
            decimal[] entity = new decimal[ConstantesProdem.Itv30min];
            List<PrnMedicion48DTO> data = this.GetAjusteAlCentroPorTipo(regFecha, idTipo);

            decimal[] dataSur = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                data.FirstOrDefault(x => x.Ptomedicodi == ConstantesProdem.PtomedicodiASur) ?? new PrnMedicion48DTO());
            decimal[] dataNorte = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
               data.FirstOrDefault(x => x.Ptomedicodi == ConstantesProdem.PtomedicodiANorte) ?? new PrnMedicion48DTO());
            decimal[] dataSCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
               data.FirstOrDefault(x => x.Ptomedicodi == ConstantesProdem.PtomedicodiASierraCentro) ?? new PrnMedicion48DTO());
            decimal[] dataCentro = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
               data.FirstOrDefault(x => x.Ptomedicodi == ConstantesProdem.PtomedicodiACentro) ?? new PrnMedicion48DTO());

            for (int i = 0; i < ConstantesProdem.Itv30min; i++)
            {
                entity[i] += dataSur[i];
                entity[i] += dataNorte[i];
                entity[i] += dataSCentro[i];
                entity[i] += dataCentro[i];
            }

            return entity;
        }
        /// <summary>
        /// Devuelve la demanda de los usuarios libres agrupados por área operativa
        /// </summary>
        /// <param name="idArea">Identificador de la tabla EQ_AREA</param>
        /// <param name="regFecha">Fecha de registro</param>
        /// <param name="idTipoinfo">Identificador del tipo de información a buscar (Ejecutada{1} o Prevista{2} o Semanal{3})</param>
        /// <returns></returns>
        public PrnMedicion48DTO GetDemandaULibresPorArea(int idArea, string regFecha, int idTipoinfo)
        {
            int esPronostico = 1;
            PrnMedicion48DTO entity = new PrnMedicion48DTO();

            switch (idTipoinfo)
            {
                case 1:
                    {
                        string idTipo = string.Join(",", new int[] {
                            ConstantesProdem.PrntDemandaEjecutadaAjusteAuto,
                            ConstantesProdem.PrntDemandaEjecutadaAjusteManual
                        });

                        entity = FactorySic.GetPrnMedicion48Repository().GetDemandaULibresPorArea(ConstantesProdem.OriglectcodiAgrupacion,
                            ConstantesProdem.OriglectcodiPR03, ConstantesProdem.LectcodiDemEjecDiario, ConstantesProdem.TipoinfocodiMWDemanda,
                            regFecha, idTipo, idArea, esPronostico, ConstantesProdem.PrntDemandaEjecutadaAjusteManual);
                        entity.Prnm48tipo = ConstantesProdem.PrntDemandaULibre;
                    }
                    break;
                case 2:
                    {
                        string idTipo = string.Join(",", new int[] {
                            ConstantesProdem.PrntDemandaPrevistaAjusteManual
                        });

                        entity = FactorySic.GetPrnMedicion48Repository().GetDemandaULibresPorArea(ConstantesProdem.OriglectcodiAgrupacion,
                            ConstantesProdem.OriglectcodiPR03, ConstantesProdem.LectcodiDemPrevDiario, ConstantesProdem.TipoinfocodiMWDemanda,
                            regFecha, idTipo, idArea, esPronostico, ConstantesProdem.PrntDemandaPrevistaAjusteManual);
                        entity.Prnm48tipo = ConstantesProdem.PrntDemandaULibre;
                    }
                    break;
                case 3:
                    {
                        entity = FactorySic.GetPrnMedicion48Repository().GetDemandaULibresPorArea(ConstantesProdem.OriglectcodiAgrupacion,
                            ConstantesProdem.OriglectcodiPR03, ConstantesProdem.LectcodiDemPrevSemanal, ConstantesProdem.TipoinfocodiMWDemanda,
                            regFecha, "-1", idArea, esPronostico, ConstantesProdem.PrntDemandaPrevistaAjusteManual);
                        entity.Prnm48tipo = ConstantesProdem.PrntDemandaULibre;
                    }
                    break;
            }

            return entity;
        }

        /// <summary>
        /// Devuelve los datos de relación entre las barras CP, barras PM y los puntos de medición de versión activa
        /// </summary>
        /// <param name="idBarra">Identificador de la Barra CP (uno o varios)</param>
        /// <returns></returns>
        public List<PrnVersionDTO> GetModeloActivo(string idBarra)
        {
            return FactorySic.GetPrnVersionRepository().GetModeloActivo(idBarra);
        }

        /// <summary>
        /// Método que lista todas las agrupaciones activas registradas en la tabla ME_PTOMEDICION
        /// </summary>
        /// <param name="areacodi">Identificador de la tabla EQ_AREA representa un área operativa</param>
        /// <param name="ptomedicodi">Identificador de la tabla ME_PTOMEDICION representa una agrupación</param>
        /// <param name="emprcodi">Identificador de la tabla SI_EMPRESA representa una empresa</param>
        /// <param name="esPronostico">Flag si la agrupación pertenece al pronóstico por áreas o no</param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListAgrupacionesActivas(string areacodi, string ptomedicodi, string emprcodi, int esPronostico)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();

            List<MePtomedicionDTO> data = FactorySic.GetPrnAgrupacionRepository().
                ListAgrupacionesActivas(areacodi, ptomedicodi, emprcodi, esPronostico);

            int x = 0;
            MePtomedicionDTO entity = new MePtomedicionDTO();
            foreach (MePtomedicionDTO d in data)
            {
                if (x != d.Ptomedicodi)
                {
                    x = d.Ptomedicodi;
                    entity = new MePtomedicionDTO
                    {
                        Ptomedicodi = d.Ptomedicodi,
                        Ptomedidesc = d.Ptomedidesc,
                        Ptogrppronostico = d.Ptogrppronostico,
                        Ptogrpcodi = d.Ptogrpcodi
                    };

                    entity.ListPtomedidesc = new List<object>();
                    entity.ListPtomedidesc.Add(new
                    {
                        id = d.Ptogrphijocodi,
                        name = d.Ptogrphijodesc
                    });

                    entitys.Add(entity);
                }
                else
                {
                    entity.ListPtomedidesc.Add(new
                    {
                        id = d.Ptogrphijocodi,
                        name = d.Ptogrphijodesc
                    });
                }
            }

            return entitys;
        }

        /// <summary>
        /// Lista de puntos de medición del PR03
        /// </summary>
        /// <param name="aonomb">Identificador de los nombres de las áreas operativas</param>
        /// <param name="tipoemprcodi">Identificador de la tabla SI_TIPOEMPRESA</param>
        /// <param name="areacodi">Identificador de la tabla EQ_AREA corresponde a una subestación</param>
        /// <param name="emprcodi">Identificador de la tabla SI_EMPRESA corresponde a una empresa</param>
        /// <param name="ptomedicodi">Identificador de la tabla ME_PTOMEDICION corresponde a un punto de medición</param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListPuntosPR03(string aonomb, string tipoemprcodi, string areacodi, string emprcodi, string ptomedicodi)
        {
            return FactorySic.GetPrnAgrupacionRepository().ListPuntosPR03(aonomb, tipoemprcodi, areacodi, emprcodi, ptomedicodi);
        }

        /// <summary>
        /// Lista de ubicaciones del PR03 - old
        /// </summary>
        /// <param name="aonomb">Identificador de los nombres de las áreas operativas</param>
        /// <returns></returns>
        public List<EqAreaDTO> ListUbicacionesPR03(string aonomb)
        {
            return FactorySic.GetPrnAgrupacionRepository().ListUbicacionesPR03(aonomb);
        }

        /// <summary>
        /// Lista de empresas del PR03 - old
        /// </summary>
        /// <param name="tipoemprcodi">Identificador de la tabla SI_TIPOEMPRESA</param>
        /// <param name="emprcodi">Identificador de la tabla SI_EMPRESA corresponde a una empresa</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListEmpresasPR03(string tipoemprcodi, string emprcodi)
        {
            return FactorySic.GetPrnAgrupacionRepository().ListEmpresasPR03(tipoemprcodi, emprcodi);
        }

        /// <summary>
        /// Lista todos los puntos de medición correspondientes al PR03
        /// </summary>
        /// <returns></returns>
        public List<MePtomedicionDTO> PR03Puntos()
        {
            return FactorySic.GetPrnMedicion48Repository().PR03Puntos();
        }

        /// <summary>
        /// Lista todos los puntos de medición no agrupados correspondientes al PR03
        /// </summary>
        /// <returns></returns>
        public List<MePtomedicionDTO> PR03PuntosNoAgrupados()
        {
            return FactorySic.GetPrnMedicion48Repository().PR03PuntosNoAgrupados();
        }

        /// <summary>
        /// Lista todas las empresas correspondientes al PR03
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> PR03Empresas()
        {
            return FactorySic.GetPrnMedicion48Repository().PR03Empresas();
        }

        /// <summary>
        /// Lista todas las ubicaciones correspondientes al PR03
        /// </summary>
        /// <returns></returns>
        public List<EqAreaDTO> PR03Ubicaciones()
        {
            return FactorySic.GetPrnMedicion48Repository().PR03Ubicaciones();
        }

        /// <summary>
        /// Método que permite obtener los datos del despacho ejecutado
        /// </summary>
        /// <param name="fechaConsulta"></param>
        public void GetDatosDespacho(DateTime fechaConsulta)
        {
            ComparativoAppServicio sComparativo = new ComparativoAppServicio();

            int idEmpresa = -1;
            char CaracterComa = ',';
            decimal dPorcantajeDesviacion = 0.05M;
            string user = "assetec";
            List<EqEquipoDTO> ListaCentrales = new List<EqEquipoDTO>();
            //traemos la lista de centrales
            try
            {
                //Eliminamos todo el Despacho Copiado para la fecha
                this.DeletePrnMedicionEq(ConstantesProdem.PrnmtipoDesEjecEquipo, fechaConsulta);
                ListaCentrales = sComparativo.ObtenerCentrales(-1, fechaConsulta);
                foreach (EqEquipoDTO Central in ListaCentrales)
                {
                    //Para cada central
                    //RPF
                    List<int> puntosRpf = new List<int>();
                    //Despacho
                    List<int> puntosDespacho = new List<int>();
                    //Traemos las listas de cada uno
                    sComparativo.ObtenerPuntosMedicion(idEmpresa, Central.Equicodi, out puntosRpf, out puntosDespacho, fechaConsulta);

                    //Validacion si no encuentra puntos relacionados busca los que pertencen a la central
                    if (puntosDespacho.Count == 0)
                    {
                        this.ObtenerPuntosFaltantes(idEmpresa, Central.Equicodi, out puntosDespacho);
                    }

                    //rpf contiene la lista de Puntos de Medicion con origlectcodi == 1
                    string rpf = string.Join<int>(CaracterComa.ToString(), puntosRpf);

                    //Despacho contiene la lista de Puntos de Medición con origlectcodi == 2
                    string despacho = string.Join<int>(CaracterComa.ToString(), puntosDespacho);

                    if (despacho.Equals(""))
                        continue;
                    //El servicio trae los datos del RPF
                    List<Medicion> datosRpf = new List<Medicion>();
                    if (!rpf.Equals(""))
                    {
                        datosRpf = new ServicioCloud().ObtenerDatosComparacionRango(fechaConsulta, true, rpf).ToList();
                    }

                    //Para este despacho traemos la SUMA de todos los puntos de medición de cada intervalo H
                    MeMedicion48DTO datosDespacho = sComparativo.ObtenerDatosDespacho(fechaConsulta, despacho);

                    //Asignamos el Despacho Ejecutado que se almacenara en nuestra tabla para que quede listo para el PRONOSTICO DE LA DEMANDA
                    PrnMedicioneqDTO entity = new PrnMedicioneqDTO();
                    entity.Equicodi = Central.Equicodi;
                    EqAreaDTO dtoArea = this.GetAreaOperativaByEquipo(Central.Equicodi); //Asignamos el AreaOperativa
                    if (dtoArea != null)
                        entity.Areacodi = dtoArea.Areacodi;
                    entity.Prnmeqtipo = ConstantesProdem.PrnmtipoDesEjecEquipo;
                    entity.Medifecha = fechaConsulta;
                    entity.Prnmeqdejevsrpf = 0; //Almacena la suma del Valor absoluto de las diferencias de Energia del Despacho Ejecutado menos el RPF
                    entity.Prnmequsucreacion = user;
                    entity.Prnmequsumodificacion = user;
                    entity.Prnmeqdepurar = 0;
                    if (datosRpf.Count == 48 && datosDespacho != null && datosDespacho.H1 != null)
                    {
                        int iDepuracion = 0; //NO hay nada que depurar
                        for (int i = 1; i <= 48; i++)
                        {
                            decimal dValorRPF = datosRpf[i - 1].H0;
                            decimal dValorDespacho = Convert.ToDecimal(datosDespacho.GetType().GetProperty("H" + i).GetValue(datosDespacho, null));
                            entity.Prnmeqdejevsrpf += Math.Abs(dValorDespacho - dValorRPF);
                            decimal dDesviacion = (dValorRPF != 0) ? (dValorDespacho - dValorRPF) / dValorRPF : 0;
                            dDesviacion = Math.Abs(dDesviacion);
                            if (dDesviacion >= dPorcantajeDesviacion)
                            {
                                iDepuracion++; //Contabiliza lo que va a depurar
                            }
                            entity.GetType().GetProperty("H" + i).SetValue(entity, dValorDespacho);
                            entity.Prnmeqdejevsrpf += dDesviacion;
                        }
                        entity.Prnmeqdepurar = iDepuracion;
                        this.SavePrnMedicionEq(entity);
                    }
                    else if (datosRpf.Count == 48)
                    {
                        //NO hay nada que depurar, RPF pasa de frente a PRN para el pronostico
                        for (int i = 1; i <= 48; i++)
                        {
                            decimal dValorRPF = datosRpf[i - 1].H0;
                            entity.GetType().GetProperty("H" + i).SetValue(entity, dValorRPF);
                        }
                        this.SavePrnMedicionEq(entity);
                    }
                    else if (datosDespacho != null && datosDespacho.H1 != null)
                    {
                        //NO hay nada que depurar, Despacho pasa de frente a PRN para el pronostico
                        for (int i = 1; i <= 48; i++)
                        {
                            decimal dValorDespacho = Convert.ToDecimal(datosDespacho.GetType().GetProperty("H" + i).GetValue(datosDespacho, null));
                            entity.GetType().GetProperty("H" + i).SetValue(entity, dValorDespacho);
                        }
                        this.SavePrnMedicionEq(entity);
                    }
                }
            }
            catch (Exception e)
            {
                string sError = e.Message;
            }
        }

        /// <summary>
        /// Método que devuelve la demanda por tipo de un grupo de puntos de medición en un rango de fechas
        /// </summary>
        /// <param name="tipodemanda">Identificador del tipo de demanda a buscar (Auxiliar)</param>
        /// <param name="ptomedicodi">Lista de puntos de medición a buscar</param>
        /// <param name="lectcodi">Tipo de lectura a buscar</param>
        /// <param name="prnm48tipo">Lista de prnm48tipo a buscar</param>
        /// <param name="fechaini">Fecha inicial del rango de busqueda</param>
        /// <param name="fechafin">Fecha final del rango de busqueda</param>
        /// <returns></returns>
        public List<PrnMedicion48DTO> GetDemandaPorTipoPorRango(int tipodemanda, string ptomedicodi,
            int lectcodi, string prnm48tipo, string fechaini, string fechafin)
        {
            return FactorySic.GetPrnMedicion48Repository().
                GetDemandaPorTipoPorRango(tipodemanda, ptomedicodi, lectcodi, prnm48tipo, fechaini, fechafin);
        }

        #endregion

        #region Listas Auxiliares

        /// <summary>
        /// Lista de los días de registro para los perfiles patrón defecto
        /// </summary>
        /// <returns></returns>
        public List<Tuple<int, string>> ListDiasPatronDefecto()
        {
            List<Tuple<int, string>> entitys = new List<Tuple<int, string>>();
            entitys.Add(new Tuple<int, string>(ConstantesProdem.PrntPatronDefLunes, "Lunes"));
            entitys.Add(new Tuple<int, string>(ConstantesProdem.PrntPatronDefMaMiJV, "Martes, Miercoles, Jueves y Viernes"));
            entitys.Add(new Tuple<int, string>(ConstantesProdem.PrntPatronDefSabado, "Sábado"));
            entitys.Add(new Tuple<int, string>(ConstantesProdem.PrntPatronDefDomingo, "Domingo"));

            return entitys;
        }

        #endregion

        #region Metodo Listas Filtradas

        /// <summary>
        /// Lista las Areas por nivel
        /// </summary>
        /// <param name="areacodi"></param>
        /// <returns></returns>
        public List<EqAreaDTO> ListPrnArea(int areacodi)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().ListPrnArea(areacodi);
        }

        /// <summary>
        /// Permite listar las empresas que pertenecen al pronóstico de la demanda por tipo, subestacion y area
        /// </summary>
        public List<PrnClasificacionDTO> ListaFiltradaEmpresas(List<PrnClasificacionDTO> listaProdem, int tipoEmpresa, int idSubestacion)
        {
            List<PrnClasificacionDTO> listaEmpresas = new List<PrnClasificacionDTO>();
            listaEmpresas = listaProdem.ToList();

            if (tipoEmpresa != 0)
            {
                listaEmpresas = listaEmpresas.Where(x => x.Tipoemprcodi == tipoEmpresa).ToList();
            }
            if (idSubestacion != 0)
            {
                listaEmpresas = listaEmpresas.Where(x => x.Areacodi == idSubestacion).ToList();
            }

            listaEmpresas = listaEmpresas.GroupBy(x => x.Emprcodi).Select(x => x.FirstOrDefault()).ToList();
            listaEmpresas = listaEmpresas.OrderBy(x => x.Emprnomb).ToList();
            return listaEmpresas;
        }

        /// <summary>
        /// Permite listar las subestaciones que pertenecen al pronóstico de la demanda
        /// </summary>
        public List<PrnClasificacionDTO> ListaFiltradaSubestaciones(List<PrnClasificacionDTO> listaProdem)
        {
            List<PrnClasificacionDTO> listaSubestaciones = new List<PrnClasificacionDTO>();
            listaSubestaciones = listaProdem.GroupBy(x => x.Areacodi).Select(x => x.FirstOrDefault()).ToList();
            listaSubestaciones = listaSubestaciones.OrderBy(x => x.Tareaabrev).ThenBy(x => x.Areanomb).ToList();

            return listaSubestaciones;
        }

        /// <summary>
        /// Permite obtener los puntos de medición que pertenecen al pronóstico de la demanda por empresa, tipo empresa y subestacion
        /// </summary>
        public List<PrnClasificacionDTO> ListaFiltradaPtoMedicion(List<PrnClasificacionDTO> listaProdem, int idSubestacion, int tipoEmpresa, int idEmpresa)
        {
            List<PrnClasificacionDTO> listaPtomedicion = new List<PrnClasificacionDTO>();
            listaPtomedicion = listaProdem.ToList();

            if (idSubestacion != 0)
            {
                listaPtomedicion = listaPtomedicion.Where(x => x.Areacodi == idSubestacion).ToList();
            }
            if (tipoEmpresa != 0)
            {
                listaPtomedicion = listaPtomedicion.Where(x => x.Tipoemprcodi == tipoEmpresa).ToList();
            }
            if (idEmpresa != 0)
            {
                listaPtomedicion = listaPtomedicion.Where(x => x.Emprcodi == idEmpresa).ToList();
            }

            //ordenamiento
            listaPtomedicion = listaPtomedicion.OrderBy(x => x.Equinomb).ToList();

            return listaPtomedicion;
        }

        #endregion

        #region Métodos del Módulo de Consultas al Estimador
        /// <summary>
        /// Job que busca un archivo y procesa un archivo raw cada 15 min
        /// </summary>
        public void ObtenerDatosRawSco()
        {
            int intervalo = 15;
            int tolerancia = 5;
            int numIntervalos = 1440 / intervalo;
            DateTime hoy = DateTime.Now.AddMinutes(intervalo * -1);

            DateTime referencia = new DateTime(hoy.Year, hoy.Month, hoy.Day, 0, 0, 0);
            //Setea el primer intervalo (H1)
            DateTime temporal = referencia.AddMinutes(intervalo);
            //Intervalo limite
            DateTime limite = (hoy < temporal) ? referencia.AddDays(-1) : referencia;

            string strHoy = hoy.ToString(ConstantesProdem.FormatoHoraMinuto);
            string strRef = referencia.ToString(ConstantesProdem.FormatoHoraMinuto);

            while (limite < hoy)
            {
                int i = 1;
                bool flag = false;
                while (i <= numIntervalos)
                {
                    i++;
                    temporal = (i != numIntervalos)
                        ? temporal.AddMinutes(intervalo)
                        : temporal.AddMinutes(intervalo - 1);
                    strRef = temporal.ToString(ConstantesProdem.FormatoHoraMinuto);
                    if (strHoy == strRef)
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag) break;
                temporal = referencia;
                hoy = hoy.AddMinutes(-1);
                strHoy = hoy.ToString(ConstantesProdem.FormatoHoraMinuto);
                strRef = temporal.ToString(ConstantesProdem.FormatoHoraMinuto);
            }

            //Intervalo valido
            if (limite < hoy)
            {
                //Obtiene posibles intervalos en caso el original no exista
                //rango permitido +/-5 min
                DateTime minRangoInf = hoy;
                DateTime minRangoSup = hoy;
                List<string> rangoTolerancia = new List<string>();

                int i = 0;
                while (i < tolerancia)
                {
                    minRangoInf = minRangoInf.AddMinutes(-1);
                    minRangoSup = minRangoSup.AddMinutes(1);
                    string strMinRangoInf = minRangoInf.ToString(ConstantesProdem.FormatoHoraMinuto);
                    string strMinRangoSup = minRangoSup.ToString(ConstantesProdem.FormatoHoraMinuto);

                    //Se evaluaran en el orden rangoInferior, rangoSuperior, rangoInferior ...
                    rangoTolerancia.Add(minRangoInf.ToString(ConstantesProdem.FormatoFechaArchivoRaw)
                        + "_" + strMinRangoInf.Replace(":", string.Empty) + "_PSSEOutput.raw");
                    rangoTolerancia.Add(minRangoSup.ToString(ConstantesProdem.FormatoFechaArchivoRaw)
                        + "_" + strMinRangoSup.Replace(":", string.Empty) + "_PSSEOutput.raw");
                    i++;
                }
                //string ruta = @"D:\Oficina\COES\auto\";
                string ruta = ConfigurationManager.AppSettings[ConstantesProdem.RutaArchivosSCO].ToString();
                string nombreArchivo = hoy.ToString(ConstantesProdem.FormatoFechaArchivoRaw)
                    + "_" + strHoy.Replace(":", string.Empty) + "_PSSEOutput.raw";
                Logger.Info("ObtenerDatosRawSco:Buscando archivo " + nombreArchivo);
                this.ObtenerDatosArchivoRaw(ruta, ConstantesProdem.EtmrawfntSco, new DateTime(), nombreArchivo, rangoTolerancia);
            }
        }

        /// <summary>
        /// Método que lee y procesa los archivos raw de un directorio
        /// </summary>
        /// <param name="ruta">Ubicación de los archivos</param>
        /// <param name="fuente">Fuente de la lectura (manual o auto)</param>
        /// <param name="fechaImportacion">Fecha para la importación(solo manual)</param>
        /// <param name="archivoEspecifico">Archivo específico para la carga de datos 
        /// [Solo para SCO, de lo contrario "Empty"]</param>
        /// <param name="rangoTolerancia">Rango de intervalos validos para reemplazar el original en caso no exista
        /// [Solo para SCO, de lo contrario "Lista vacía"]</param>
        public void ObtenerDatosArchivoRaw(string ruta, int fuente, DateTime fechaImportacion,
            string archivoEspecifico, List<string> rangoTolerancia)
        {
            bool esSCO = (fuente == ConstantesProdem.EtmrawfntSco) ? true : false;
            bool existe = FileServer.VerificarLaExistenciaDirectorio(ruta);
            if (!existe)
            {
                Logger.Error("ObtenerDatosArchivoRaw: La ruta ingresada no es valida o el usuario no tiene acceso");
                return;
            }

            List<FileData> archivos = new List<FileData>();
            List<MePtomedicionDTO> puntos = new List<MePtomedicionDTO>();
            List<PrnEstimadorRawDTO> entidades = new List<PrnEstimadorRawDTO>();

            List<string> arrayData;
            List<string[]> dataBarras, dataCargas, dataShunts,
                dataGeneradores, dataLineas, dataTransformadores;

            if (!esSCO) archivos = FileServer.ListarArhivos(string.Empty, ruta);
            if (!string.IsNullOrEmpty(archivoEspecifico))
            {
                bool existeArchivo = false;
                existeArchivo = FileServer.VerificarExistenciaFile(string.Empty, archivoEspecifico, ruta);
                if (existeArchivo)
                    archivos.Add(FileServer.ObtenerArchivoEspecifico(ruta, archivoEspecifico));
                else
                {
                    foreach (string strArchivo in rangoTolerancia)
                    {
                        existeArchivo = FileServer.VerificarExistenciaFile(string.Empty, strArchivo, ruta);
                        if (existeArchivo)
                        {
                            FileData entityFile = FileServer.ObtenerArchivoEspecifico(ruta, strArchivo);
                            archivos.Add(entityFile);
                            break;
                        }
                    }
                }
                if (archivos.Count == 0)
                    Logger.Error("ObtenerDatosArchivoRaw: No se encontraron archivos");
                else
                    Logger.Info($"ObtenerDatosArchivoRaw: Se tomo el archivo {archivos[0].FileName}");
            }

            foreach (FileData archivo in archivos)
            {
                if (archivo.Extension != ".raw") continue;

                string[] nombreArchivo;
                string strFechaRegistro = string.Empty;
                string strIntervaloMedicion = string.Empty;
                DateTime fechaRegistro = new DateTime();
                entidades = new List<PrnEstimadorRawDTO>();
                //Fuente manual
                if (fuente == ConstantesProdem.EtmrawfntIeod)
                {
                    puntos = this.ListPtomedicionByOriglectcodi(ConstantesProdem.OriglectcodiTnaIeod);
                    nombreArchivo = archivo.FileName.Split(new char[] { '_', '.' });
                    strFechaRegistro = nombreArchivo[1].Substring(0, 8);
                    strIntervaloMedicion = nombreArchivo[1].Substring(8, 4).Insert(2, ":");
                    fechaRegistro = fechaImportacion;
                }
                //Fuente auto
                if (fuente == ConstantesProdem.EtmrawfntSco)
                {
                    puntos = this.ListPtomedicionByOriglectcodi(ConstantesProdem.OriglectcodiTnaSco);
                    nombreArchivo = archivoEspecifico.Split(new char[] { '_', '.' });
                    strFechaRegistro = nombreArchivo[0].Substring(0, 8);
                    strIntervaloMedicion = nombreArchivo[1].Insert(2, ":");
                    fechaRegistro = DateTime.ParseExact(strFechaRegistro,
                        ConstantesProdem.FormatoFechaArchivoRaw, CultureInfo.InvariantCulture);
                }

                //Valida el que el archivo corresponda al intervalo buscado
                bool valid = UtilProdem.ValidarIntervaloRaw(strIntervaloMedicion, ConstantesProdem.Itv30min);
                if (!valid)
                {
                    Logger.Error("ObtenerDatosArchivoRaw: El archivo .raw no pertenece a los intervalos buscados");
                    continue;
                }

                //Carga todas las filas del archivo
                arrayData = new List<string>();

                using (StreamReader sr = FileServer.OpenReaderFile(archivo.FileName, ruta))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null) arrayData.Add(s);
                }

                arrayData.RemoveRange(0, 3);

                //Obtención de los datos de barras
                int i = 0;
                dataBarras = new List<string[]>();
                dataBarras = UtilProdem.ObtenerBlqInfoRaw(i, ConstantesProdem.corteBloqueBarras, arrayData);
                int totalBarras = dataBarras.Count;
                //--Validación de equipo desconectado
                dataBarras = dataBarras
                    .Where(x => x[ConstantesProdem.indexEqDesconectado] != ConstantesProdem.valorEqDesconectado)
                    .ToList();

                //Obtención de los datos de Cargas
                i += totalBarras + 1;
                dataCargas = new List<string[]>();
                dataCargas = UtilProdem.ObtenerBlqInfoRaw(i, ConstantesProdem.corteBloqueCargas, arrayData);

                //Obtención de los datos de Shunts
                i += dataCargas.Count + 1;
                dataShunts = new List<string[]>();
                dataShunts = UtilProdem.ObtenerBlqInfoRaw(i, ConstantesProdem.corteBloqueShunts, arrayData);

                //Obtención de los datos de Generadores
                i += dataShunts.Count + 1;
                dataGeneradores = new List<string[]>();
                dataGeneradores = UtilProdem.ObtenerBlqInfoRaw(i, ConstantesProdem.corteBloqueGeneradores, arrayData);

                //Obtención de los datos de Lineas
                i += dataGeneradores.Count + 1;
                dataLineas = new List<string[]>();
                dataLineas = UtilProdem.ObtenerBlqInfoRaw(i, ConstantesProdem.corteBloqueLineas, arrayData);

                //Obtención de los datos de Transformadores
                i += dataLineas.Count + 1;
                dataTransformadores = new List<string[]>();
                dataTransformadores = UtilProdem.ObtenerBlqInfoRaw(i, ConstantesProdem.corteBloqueTransformadores, arrayData);

                //Convierte texto fecha del archivo RAW a DateTime
                string strFechaMedicion = $"{fechaRegistro.ToString(ConstantesProdem.FormatoFecha)} {strIntervaloMedicion}";
                DateTime fechaMedicion = DateTime.ParseExact(strFechaMedicion,
                    ConstantesProdem.FormatoFechaMedicionRaw, CultureInfo.InvariantCulture);

                //Procesar
                entidades.AddRange(ProcesarDatosGeneradores(fuente, fechaMedicion, dataGeneradores, puntos));
                entidades.AddRange(ProcesarDatosCargas(fuente, fechaMedicion, dataCargas, puntos));
                entidades.AddRange(ProcesarDatosShunts(fuente, fechaMedicion, dataShunts, puntos));
                entidades.AddRange(ProcesarDatosBarras(fuente, fechaMedicion, dataCargas, dataBarras, puntos));
                entidades.AddRange(ProcesarDatosLineas(fuente, fechaMedicion, dataLineas, dataBarras, puntos));
                entidades.AddRange(ProcesarDatosTransformadores(fuente, fechaMedicion, dataTransformadores, dataBarras, puntos));

                //Elimina las mediciones si ya existen en la tabla PRN_ESTIMADORRAW
                this.DeletePrnEstimadorRawPorFechaIntervalo(fuente, strFechaMedicion);

                if (entidades.Count > 0)
                {
                    //Registra las mediciones en las tablas temporales
                    this.BulkInsertPrnEstimadorRaw(entidades, fuente);

                    //Inserta los registros desde las tablas temporales a PRN_ESTIMADORRAW
                    this.InsertTableIntoPrnEstimadorRaw(fuente, strFechaMedicion);

                    //Elimina los registros de la tabla temporal
                    this.TruncateTemporalPrnEstimadorRaw(fuente);
                }
            }
        }

        /// <summary>
        /// Procesa los datos de GENERADORES leidos de los archivos RAW
        /// </summary>
        /// <param name="fuente">Fuente de la obtención de datos (auto o manual)</param>
        /// <param name="fechaMedicion">Fecha del registro</param>
        /// <param name="dataRaw">Datos de generadores</param>
        /// <param name="listaPuntos">Lista de puntos de medición registrados en la bd</param>
        public List<PrnEstimadorRawDTO> ProcesarDatosGeneradores(int fuente, DateTime fechaMedicion,
            List<string[]> dataRaw, List<MePtomedicionDTO> listaPuntos)
        {
            List<PrnEstimadorRawDTO> entities = new List<PrnEstimadorRawDTO>();

            int indexPotenciaActiva = 2;
            int indexPotenciaReactiva = 3;
            int indexNombre = 28;

            int idUnidad;
            PrnEstimadorRawDTO entidadMedicion;
            MePtomedicionDTO entidadPtoMedicion;
            decimal valorPotenciaActiva, valorPotenciaReactiva;

            foreach (string[] r in dataRaw)
            {
                valorPotenciaActiva = decimal.Parse(r[indexPotenciaActiva]);
                valorPotenciaReactiva = decimal.Parse(r[indexPotenciaReactiva]);

                entidadPtoMedicion = new MePtomedicionDTO();
                entidadPtoMedicion.Ptomedidesc = r[indexNombre].Trim();
                entidadPtoMedicion.Codref = ConstantesProdem.EtmrawtpGenerador;
                if (fuente == ConstantesProdem.EtmrawfntIeod)
                    entidadPtoMedicion.Origlectcodi = ConstantesProdem.OriglectcodiTnaIeod;
                if (fuente == ConstantesProdem.EtmrawfntSco)
                    entidadPtoMedicion.Origlectcodi = ConstantesProdem.OriglectcodiTnaSco;
                idUnidad = RegistrarPtoMedicionRaw(entidadPtoMedicion, listaPuntos);

                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidad,
                    Prnvarcodi = ConstantesProdem.GeneradorPotActivaMW,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpGenerador,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorPotenciaActiva
                };
                entities.Add(entidadMedicion);

                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidad,
                    Prnvarcodi = ConstantesProdem.GeneradorPotReactivaMVAR,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpGenerador,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorPotenciaReactiva
                };
                entities.Add(entidadMedicion);
            }
            return entities;
        }

        /// <summary>
        /// Procesa los datos de CARGAS leidos de los archivos RAW
        /// </summary>
        /// <param name="fuente">Fuente de la obtención de datos (auto o manual)</param>
        /// <param name="fechaMedicion">Fecha del registro</param>
        /// <param name="dataRaw">Datos de cargas</param>
        /// <param name="listaPuntos">Lista de puntos de medición registrados en la bd</param>
        public List<PrnEstimadorRawDTO> ProcesarDatosCargas(int fuente, DateTime fechaMedicion,
            List<string[]> dataRaw, List<MePtomedicionDTO> listaPuntos)
        {
            List<PrnEstimadorRawDTO> entities = new List<PrnEstimadorRawDTO>();

            int indexPotenciaActiva = 5;
            int indexPotenciaReactiva = 6;
            int indexNombre = 13;

            int idUnidad;
            PrnEstimadorRawDTO entidadMedicion;
            MePtomedicionDTO entidadPtoMedicion;
            decimal valorPotenciaActiva, valorPotenciaReactiva;

            foreach (string[] r in dataRaw)
            {
                valorPotenciaActiva = decimal.Parse(r[indexPotenciaActiva]);
                valorPotenciaReactiva = decimal.Parse(r[indexPotenciaReactiva]);

                entidadPtoMedicion = new MePtomedicionDTO();
                entidadPtoMedicion.Ptomedidesc = r[indexNombre].Trim();
                entidadPtoMedicion.Codref = ConstantesProdem.EtmrawtpCarga;
                if (fuente == ConstantesProdem.EtmrawfntIeod)
                    entidadPtoMedicion.Origlectcodi = ConstantesProdem.OriglectcodiTnaIeod;
                if (fuente == ConstantesProdem.EtmrawfntSco)
                    entidadPtoMedicion.Origlectcodi = ConstantesProdem.OriglectcodiTnaSco;

                idUnidad = RegistrarPtoMedicionRaw(entidadPtoMedicion, listaPuntos);

                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidad,
                    Prnvarcodi = ConstantesProdem.CargaPotActivaMW,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpCarga,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorPotenciaActiva
                };
                entities.Add(entidadMedicion);

                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidad,
                    Prnvarcodi = ConstantesProdem.CargaPotReactivaMVAR,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpCarga,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorPotenciaReactiva
                };
                entities.Add(entidadMedicion);
            }
            return entities;
        }

        /// <summary>
        /// Procesa los datos de SHUNTS leidos de los archivos RAW
        /// </summary>
        /// <param name="fuente">Fuente de la obtención de datos (auto o manual)</param>
        /// <param name="fechaMedicion">Fecha del registro</param>
        /// <param name="dataRaw">Datos de shunts</param>
        /// <param name="listaPuntos">Lista de puntos de medición registrados en la bd</param>
        public List<PrnEstimadorRawDTO> ProcesarDatosShunts(int fuente, DateTime fechaMedicion,
            List<string[]> dataRaw, List<MePtomedicionDTO> listaPuntos)
        {
            List<PrnEstimadorRawDTO> entities = new List<PrnEstimadorRawDTO>();

            int indexC3 = 2;
            int indexPotenciaReactiva = 4;
            int indexNombre = 5;

            int idUnidad;
            PrnEstimadorRawDTO entidadMedicion;
            MePtomedicionDTO entidadPtoMedicion;
            decimal valorPotenciaReactiva, valorC3;

            foreach (string[] r in dataRaw)
            {
                valorC3 = decimal.Parse(r[indexC3]);
                valorPotenciaReactiva = decimal.Parse(r[indexPotenciaReactiva]);
                valorPotenciaReactiva = valorPotenciaReactiva * valorC3;
                valorPotenciaReactiva = Math.Round(valorPotenciaReactiva, 4);

                entidadPtoMedicion = new MePtomedicionDTO();
                entidadPtoMedicion.Ptomedidesc = r[indexNombre].Trim();
                entidadPtoMedicion.Codref = ConstantesProdem.EtmrawtpShunt;
                if (fuente == ConstantesProdem.EtmrawfntIeod)
                    entidadPtoMedicion.Origlectcodi = ConstantesProdem.OriglectcodiTnaIeod;
                if (fuente == ConstantesProdem.EtmrawfntSco)
                    entidadPtoMedicion.Origlectcodi = ConstantesProdem.OriglectcodiTnaSco;

                idUnidad = RegistrarPtoMedicionRaw(entidadPtoMedicion, listaPuntos);

                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidad,
                    Prnvarcodi = ConstantesProdem.ShuntPotReactivaMVAR,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpShunt,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorPotenciaReactiva
                };
                entities.Add(entidadMedicion);
            }
            return entities;
        }

        /// <summary>
        /// Procesa los datos de BARRAS leidos de los archivos RAW
        /// </summary>
        /// <param name="fuente">Fuente de la obtención de datos (auto o manual)</param>
        /// <param name="fechaMedicion">Fecha del registro</param>
        /// <param name="dataCargas">Datos de cargas</param>
        /// <param name="dataBarras">Datos de barras</param>
        /// <param name="listaPuntos">Lista de puntos de medición registrados en la bd</param>
        public List<PrnEstimadorRawDTO> ProcesarDatosBarras(int fuente, DateTime fechaMedicion,
            List<string[]> dataCargas, List<string[]> dataBarras, List<MePtomedicionDTO> listaPuntos)
        {
            List<PrnEstimadorRawDTO> entities = new List<PrnEstimadorRawDTO>();

            int indexDemandaActiva = 5;
            int indexDemandaReactiva = 6;
            int indexTensionPU = 7;
            int indexTensionNominal = 2;
            int indexAngulo = 8;
            int indexNombre = 1;
            int indexId = 0;

            int idUnidad;
            PrnEstimadorRawDTO entidadMedicion;
            MePtomedicionDTO entidadPtoMedicion;
            decimal valorDemandaActiva, valorDemandaReactiva, valorTensionKV;

            List<string> listaBarras = dataBarras
                .Select(x => x[indexNombre].Trim(new char[] { ' ', '\'' }))
                .Distinct()
                .ToList();

            foreach (string nomBarra in listaBarras)
            {
                entidadPtoMedicion = new MePtomedicionDTO();
                entidadPtoMedicion.Ptomedidesc = nomBarra;
                entidadPtoMedicion.Codref = ConstantesProdem.EtmrawtpBarra;
                if (fuente == ConstantesProdem.EtmrawfntIeod)
                    entidadPtoMedicion.Origlectcodi = ConstantesProdem.OriglectcodiTnaIeod;
                if (fuente == ConstantesProdem.EtmrawfntSco)
                    entidadPtoMedicion.Origlectcodi = ConstantesProdem.OriglectcodiTnaSco;

                idUnidad = RegistrarPtoMedicionRaw(entidadPtoMedicion, listaPuntos);

                //Obtiene las barras con el mismo nombre si existieran
                List<string[]> grupoBarras = dataBarras
                    .Where(x => x[indexNombre].Trim(new char[] { ' ', '\'' }) == nomBarra)
                    .ToList();
                int totalRegistros = grupoBarras.Count;

                //Cálculo de Tensión en KV
                decimal tensionPU = 0;
                decimal tensionNominal = 0;
                foreach (string[] x in grupoBarras)
                {
                    tensionPU += decimal.Parse(x[indexTensionPU]);
                    tensionNominal += decimal.Parse(x[indexTensionNominal]);
                }
                tensionPU = tensionPU / totalRegistros;
                tensionNominal = tensionNominal / totalRegistros;

                //Ángulo
                decimal anguloBarra = 0;
                foreach (string[] x in grupoBarras)
                {
                    anguloBarra += decimal.Parse(x[indexAngulo]);
                }
                anguloBarra = anguloBarra / totalRegistros;

                //Cálculo de Demandas
                valorDemandaActiva = 0;
                valorDemandaReactiva = 0;
                List<string> idBarras = grupoBarras
                    .Select(x => x[indexId].Trim())
                    .ToList();
                foreach (string[] rCarga in dataCargas)
                {
                    if (idBarras.Contains(rCarga[indexId].Trim()))
                    {
                        valorDemandaActiva += decimal.Parse(rCarga[indexDemandaActiva]);
                        valorDemandaReactiva += decimal.Parse(rCarga[indexDemandaReactiva]);
                    }
                }

                #region Agrega los valores a la lista de entidades para el registro
                //Demanda Activa
                valorDemandaActiva = Math.Round(valorDemandaActiva, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidad,
                    Prnvarcodi = ConstantesProdem.BarraDemActivaMW,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpBarra,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorDemandaActiva
                };
                entities.Add(entidadMedicion);

                //Demanda Reactiva
                valorDemandaReactiva = Math.Round(valorDemandaReactiva, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidad,
                    Prnvarcodi = ConstantesProdem.BarraDemReactivaMVAR,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpBarra,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorDemandaReactiva
                };
                entities.Add(entidadMedicion);

                //Cálculo de Tensión en KV
                valorTensionKV = tensionPU * tensionNominal;
                valorTensionKV = Math.Round(valorTensionKV, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidad,
                    Prnvarcodi = ConstantesProdem.BarraTensionKv,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpBarra,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorTensionKV
                };
                entities.Add(entidadMedicion);

                //Ángulo
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidad,
                    Prnvarcodi = ConstantesProdem.BarraAngulo,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpBarra,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = anguloBarra
                };
                entities.Add(entidadMedicion);
                #endregion
            }

            return entities;
        }

        /// <summary>
        /// Procesa los datos de LINEAS leidos de los archivos RAW
        /// </summary>
        /// <param name="fuente">Fuente de la obtención de datos (auto o manual)</param>
        /// <param name="fechaMedicion">Fecha del registro</param>
        /// <param name="dataLineas">Datos de lineas</param>
        /// <param name="dataBarras">Datos de barras</param>
        /// <param name="listaPuntos">Lista de puntos de medición registrados en la bd</param>
        public List<PrnEstimadorRawDTO> ProcesarDatosLineas(int fuente, DateTime fechaMedicion,
            List<string[]> dataLineas, List<string[]> dataBarras, List<MePtomedicionDTO> listaPuntos)
        {
            List<PrnEstimadorRawDTO> entities = new List<PrnEstimadorRawDTO>();

            //Índices de los datos de lineas
            int indexNombre = 24;
            int indexLineaBarraI = 0;//Una linea esta conectada a dos barras
            int indexLineaBarraJ = 1;
            int indexVarR = 3;
            int indexVarX = 4;
            int indexVarB = 5;
            int indexVarCap = 6;

            //Índices de los datos de barras
            int indexBarra = 0;
            int indexVarVkv = 2;
            int indexVarVpu = 7;
            int indexAngulo = 8;

            int idUnidadEnvio;
            int idUnidadRecepcion;
            PrnEstimadorRawDTO entidadMedicion;
            MePtomedicionDTO entidadPtoMedicionEnvio;
            MePtomedicionDTO entidadPtoMedicionRecepcion;
            decimal valorK1, valorK2, valorBase;
            decimal valorPotEnvioActiva, valorPotEnvioReactiva, valorPotEnvioAparente;
            decimal valorPotRecepcionActiva, valorPotRecepcionReactiva, valorPotRecepcionAparente;
            decimal valorPerdidasLineas;
            decimal valorCargabilidadMaxima;

            foreach (string[] rLinea in dataLineas)
            {
                entidadPtoMedicionEnvio = new MePtomedicionDTO();
                entidadPtoMedicionEnvio.Ptomedidesc = rLinea[indexNombre].Trim(new char[] { ' ', '\'' });
                entidadPtoMedicionEnvio.Ptomedidesc = entidadPtoMedicionEnvio.Ptomedidesc + ConstantesProdem.PrefijoEnvio;
                entidadPtoMedicionEnvio.Codref = ConstantesProdem.EtmrawtpLinea;

                entidadPtoMedicionRecepcion = new MePtomedicionDTO();
                entidadPtoMedicionRecepcion.Ptomedidesc = rLinea[indexNombre].Trim(new char[] { ' ', '\'' });
                entidadPtoMedicionRecepcion.Ptomedidesc = entidadPtoMedicionRecepcion.Ptomedidesc + ConstantesProdem.PrefijoRecepcion;
                entidadPtoMedicionRecepcion.Codref = ConstantesProdem.EtmrawtpLinea;

                if (fuente == ConstantesProdem.EtmrawfntIeod)
                {
                    entidadPtoMedicionEnvio.Origlectcodi = ConstantesProdem.OriglectcodiTnaIeod;
                    entidadPtoMedicionRecepcion.Origlectcodi = ConstantesProdem.OriglectcodiTnaIeod;
                }
                if (fuente == ConstantesProdem.EtmrawfntSco)
                {
                    entidadPtoMedicionEnvio.Origlectcodi = ConstantesProdem.OriglectcodiTnaSco;
                    entidadPtoMedicionRecepcion.Origlectcodi = ConstantesProdem.OriglectcodiTnaSco;
                }

                idUnidadEnvio = RegistrarPtoMedicionRaw(entidadPtoMedicionEnvio, listaPuntos);
                idUnidadRecepcion = RegistrarPtoMedicionRaw(entidadPtoMedicionRecepcion, listaPuntos);

                #region Procesa los datos
                decimal varR = decimal.Parse(rLinea[indexVarR]);
                decimal varX = decimal.Parse(rLinea[indexVarX]);
                decimal varB = decimal.Parse(rLinea[indexVarB]);
                decimal varCap = decimal.Parse(rLinea[indexVarCap]);

                //Calculo de la variable K1 y K2
                valorK1 = varR / ((varR * varR) + (varX * varX));
                valorK2 = varX / ((varR * varR) + (varX * varX));

                //Obtención de varibles por barra
                decimal varIkv = 0;
                decimal varJkv = 0;
                decimal varIpu = 0;
                decimal varJpu = 0;
                double anguloI = 0;
                double anguloJ = 0;

                foreach (string[] rBarra in dataBarras)
                {
                    if (rLinea[indexLineaBarraI].Trim() == rBarra[indexBarra].Trim())
                    {
                        varIkv = decimal.Parse(rBarra[indexVarVkv]);
                        varIpu = decimal.Parse(rBarra[indexVarVpu]);
                        anguloI = double.Parse(rBarra[indexAngulo]);
                    }

                    if (rLinea[indexLineaBarraJ].Trim() == rBarra[indexBarra].Trim())
                    {
                        varJkv = decimal.Parse(rBarra[indexVarVkv]);
                        varJpu = decimal.Parse(rBarra[indexVarVpu]);
                        anguloJ = double.Parse(rBarra[indexAngulo]);
                    }
                }
                //Obtiene diferencia angular
                double difAnguloIJ = anguloI - anguloJ;
                difAnguloIJ = (difAnguloIJ * Math.PI) / 180;

                //Cálculo de la variable Base
                valorBase = (varIkv + varJkv) / 2;

                //Calculo de la Potencia de envío
                valorPotEnvioActiva = (-1 * valorK1) * (decimal)(Math.Cos(difAnguloIJ));
                valorPotEnvioActiva = valorPotEnvioActiva + (valorK2 * (decimal)(Math.Sin(difAnguloIJ)));
                valorPotEnvioActiva = valorPotEnvioActiva * varIpu * varJpu;
                valorPotEnvioActiva = valorPotEnvioActiva + (varIpu * varIpu * valorK1);
                valorPotEnvioActiva = valorPotEnvioActiva * 100;

                valorPotEnvioReactiva = (-1 * valorK1) * (decimal)(Math.Sin(difAnguloIJ));
                valorPotEnvioReactiva = valorPotEnvioReactiva - (valorK2 * (decimal)(Math.Cos(difAnguloIJ)));
                valorPotEnvioReactiva = valorPotEnvioReactiva * varIpu * varJpu;
                valorPotEnvioReactiva = valorPotEnvioReactiva + (varIpu * varIpu * (valorK2 - (varB / 2)));
                valorPotEnvioReactiva = valorPotEnvioReactiva * 100;

                valorPotEnvioAparente = valorPotEnvioActiva * valorPotEnvioActiva;
                valorPotEnvioAparente = valorPotEnvioAparente + (valorPotEnvioReactiva * valorPotEnvioReactiva);
                valorPotEnvioAparente = (decimal)Math.Sqrt((double)valorPotEnvioAparente);

                //Calculo de la Potencia de recepción
                valorPotRecepcionActiva = (-1 * valorK1) * (decimal)(Math.Cos(difAnguloIJ));
                valorPotRecepcionActiva = valorPotRecepcionActiva - (valorK2 * (decimal)(Math.Sin(difAnguloIJ)));
                valorPotRecepcionActiva = valorPotRecepcionActiva * varIpu * varJpu;
                valorPotRecepcionActiva = valorPotRecepcionActiva + (varJpu * varJpu * valorK1);
                valorPotRecepcionActiva = valorPotRecepcionActiva * 100;

                valorPotRecepcionReactiva = valorK1 * (decimal)(Math.Sin(difAnguloIJ));
                valorPotRecepcionReactiva = valorPotRecepcionReactiva - (valorK2 * (decimal)(Math.Cos(difAnguloIJ)));
                valorPotRecepcionReactiva = valorPotRecepcionReactiva * varIpu * varJpu;
                valorPotRecepcionReactiva = valorPotRecepcionReactiva + (varJpu * varJpu * (valorK2 - (varB / 2)));
                valorPotRecepcionReactiva = valorPotRecepcionReactiva * 100;

                valorPotRecepcionAparente = valorPotRecepcionActiva * valorPotRecepcionActiva;
                valorPotRecepcionAparente = valorPotEnvioAparente + (valorPotRecepcionReactiva * valorPotRecepcionReactiva);
                valorPotRecepcionAparente = (decimal)Math.Sqrt((double)valorPotRecepcionAparente);

                //Cálculo de las pérdidas en línea
                valorPerdidasLineas = valorPotEnvioActiva + valorPotRecepcionActiva;

                //Cálculo de cargabilidad máxima
                decimal porEnvio = (valorPotEnvioAparente / varCap) * 100;
                decimal porRecepcion = (valorPotRecepcionAparente / varCap) * 100;
                valorCargabilidadMaxima = (porEnvio > porRecepcion) ? porEnvio : porRecepcion;


                #endregion

                #region Carga la lista de entidades
                //a) Potencia de envio
                //a.1) activa
                valorPotEnvioActiva = Math.Round(valorPotEnvioActiva, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidadEnvio,
                    Prnvarcodi = ConstantesProdem.LineaPotActivaMW,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpLinea,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorPotEnvioActiva
                };
                entities.Add(entidadMedicion);

                //a.2) reactiva
                valorPotEnvioReactiva = Math.Round(valorPotEnvioReactiva, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidadEnvio,
                    Prnvarcodi = ConstantesProdem.LineaPotReactivaMVAR,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpLinea,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorPotEnvioReactiva
                };
                entities.Add(entidadMedicion);

                //a.3) aparente
                valorPotEnvioAparente = Math.Round(valorPotEnvioAparente, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidadEnvio,
                    Prnvarcodi = ConstantesProdem.LineaPotAparenteMVA,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpLinea,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorPotEnvioAparente
                };
                entities.Add(entidadMedicion);

                //b) Potencia de recepción
                //b.1) activa
                valorPotRecepcionActiva = Math.Round(valorPotRecepcionActiva, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.LineaPotActivaMW,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpLinea,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorPotRecepcionActiva
                };
                entities.Add(entidadMedicion);

                //b.2) reactiva
                valorPotRecepcionReactiva = Math.Round(valorPotRecepcionReactiva, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.LineaPotReactivaMVAR,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpLinea,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorPotRecepcionReactiva
                };
                entities.Add(entidadMedicion);

                //b.3) aparente
                valorPotRecepcionAparente = Math.Round(valorPotRecepcionAparente, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.LineaPotAparenteMVA,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpLinea,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorPotRecepcionAparente
                };
                entities.Add(entidadMedicion);

                //c.1) Pérdidas en línea envio
                valorPerdidasLineas = Math.Round(valorPerdidasLineas, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidadEnvio,
                    Prnvarcodi = ConstantesProdem.LineaPerdidasMW,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpLinea,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorPerdidasLineas
                };
                entities.Add(entidadMedicion);

                //c.2) Pérdidas en línea recepción
                valorPerdidasLineas = Math.Round(valorPerdidasLineas, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.LineaPerdidasMW,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpLinea,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorPerdidasLineas
                };
                entities.Add(entidadMedicion);

                //d.1) Cargabilidad máxima envio
                valorCargabilidadMaxima = Math.Round(valorCargabilidadMaxima, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidadEnvio,
                    Prnvarcodi = ConstantesProdem.LineaCargaMaxima,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpLinea,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorCargabilidadMaxima
                };
                entities.Add(entidadMedicion);

                //d.2) Cargabilidad máxima recepción
                valorCargabilidadMaxima = Math.Round(valorCargabilidadMaxima, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.LineaCargaMaxima,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpLinea,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorCargabilidadMaxima
                };
                entities.Add(entidadMedicion);
                #endregion
            }
            return entities;
        }

        /// <summary>
        /// Procesa los datos de TRANSFORMADORES leidos de los archivos RAW
        /// </summary>
        /// <param name="fuente">Fuente de la obtención de datos (auto o manual)</param>
        /// <param name="fechaMedicion">Fecha del registro</param>
        /// <param name="dataTransformadores">Datos de transformadores</param>
        /// <param name="dataBarras">Datos de barras</param>
        /// <param name="listaPuntos">Lista de puntos de medición registrados en la bd</param>
        public List<PrnEstimadorRawDTO> ProcesarDatosTransformadores(int fuente, DateTime fechaMedicion,
            List<string[]> dataTransformadores, List<string[]> dataBarras, List<MePtomedicionDTO> listaPuntos)
        {
            List<PrnEstimadorRawDTO> entities = new List<PrnEstimadorRawDTO>();

            //Índices de los datos de lineas
            //F:fila, un registro de transformador se compone de 4 filas
            int indexNombre = 6;//F4

            if (fechaMedicion > new DateTime(2025, 04, 02, 09, 00, 00)) // POR CORTE DEL NUEVO SCADA
            {
                indexNombre = 2;
            }

            int indexWIND1 = 0;//F3
            int indexRMA1 = 8;//F3
            int indexRMI1 = 9;//F3
            int indexNTP = 12;//F3
            int indexVarR = 0;//F2
            int indexVarX = 1;//F2
            int indexVarCap = 3;//F3
            int indexTrafoBarraI = 0;//Barra 1
            int indexTrafoBarraJ = 1;//Barra 2

            //Índice de los datos de barras
            int indexBarra = 0;
            int indexVarVkv = 2;
            int indexVarVpu = 7;
            int indexAngulo = 8;

            int idUnidadEnvio;
            int idUnidadRecepcion;
            PrnEstimadorRawDTO entidadMedicion;
            MePtomedicionDTO entidadPtoMedicionEnvio;
            MePtomedicionDTO entidadPtoMedicionRecepcion;

            decimal valorNtpx, valorNOMV1p;
            decimal valorUtrnH, valorUtrnL;
            decimal valorK1, valorK2, valortVipu, valortVjpu;
            decimal valorPotEnvioActiva, valorPotEnvioReactiva, valorPotEnvioAparente;
            decimal valorPotRecepcionActiva, valorPotRecepcionReactiva, valorPotRecepcionAparente;
            decimal valorPerdidasTrafos;
            decimal valorCargabilidadMaxima;

            int i = 0;
            while (i < dataTransformadores.Count)
            {
                string[] rTransformador1 = dataTransformadores[i];
                string[] rTransformador2 = dataTransformadores[i + 1];
                string[] rTransformador3 = dataTransformadores[i + 2];
                string[] rTransformador4 = dataTransformadores[i + 3];

                entidadPtoMedicionEnvio = new MePtomedicionDTO();
                entidadPtoMedicionEnvio.Ptomedidesc = rTransformador4[indexNombre].Trim(new char[] { ' ', '\'' });
                entidadPtoMedicionEnvio.Ptomedidesc = entidadPtoMedicionEnvio.Ptomedidesc + ConstantesProdem.PrefijoEnvio;
                entidadPtoMedicionEnvio.Codref = ConstantesProdem.EtmrawtpTrafo;

                entidadPtoMedicionRecepcion = new MePtomedicionDTO();
                entidadPtoMedicionRecepcion.Ptomedidesc = rTransformador4[indexNombre].Trim(new char[] { ' ', '\'' });
                entidadPtoMedicionRecepcion.Ptomedidesc = entidadPtoMedicionRecepcion.Ptomedidesc + ConstantesProdem.PrefijoRecepcion;
                entidadPtoMedicionEnvio.Codref = ConstantesProdem.EtmrawtpTrafo;

                if (fuente == ConstantesProdem.EtmrawfntIeod)
                {
                    entidadPtoMedicionEnvio.Origlectcodi = ConstantesProdem.OriglectcodiTnaIeod;
                    entidadPtoMedicionRecepcion.Origlectcodi = ConstantesProdem.OriglectcodiTnaIeod;
                }

                if (fuente == ConstantesProdem.EtmrawfntSco)
                {
                    entidadPtoMedicionEnvio.Origlectcodi = ConstantesProdem.OriglectcodiTnaSco;
                    entidadPtoMedicionRecepcion.Origlectcodi = ConstantesProdem.OriglectcodiTnaSco;
                }

                idUnidadEnvio = RegistrarPtoMedicionRaw(entidadPtoMedicionEnvio, listaPuntos);
                idUnidadRecepcion = RegistrarPtoMedicionRaw(entidadPtoMedicionRecepcion, listaPuntos);

                #region Procesa los datos
                decimal varWIND1 = decimal.Parse(rTransformador3[indexWIND1]);
                decimal varRMA1 = decimal.Parse(rTransformador3[indexRMA1]);
                decimal varRMI1 = decimal.Parse(rTransformador3[indexRMI1]);
                decimal varNTP = decimal.Parse(rTransformador3[indexNTP]);
                decimal varR = decimal.Parse(rTransformador2[indexVarR]);
                decimal varX = decimal.Parse(rTransformador2[indexVarX]);
                decimal varCap = decimal.Parse(rTransformador3[indexVarCap]);

                //Calculo de la variable K1 y K2
                valorK1 = varR / ((varR * varR) + (varX * varX));
                valorK2 = varX / ((varR * varR) + (varX * varX));

                //Obtención de varibles por barra
                decimal varIkv = 0;
                decimal varJkv = 0;
                decimal varIpu = 0;
                decimal varJpu = 0;
                double anguloI = 0;
                double anguloJ = 0;
                foreach (string[] rBarra in dataBarras)
                {
                    if (rTransformador1[indexTrafoBarraI].Trim() == rBarra[indexBarra].Trim())
                    {
                        varIkv = decimal.Parse(rBarra[indexVarVkv]);
                        varIpu = decimal.Parse(rBarra[indexVarVpu]);
                        anguloI = double.Parse(rBarra[indexAngulo]);
                    }

                    if (rTransformador1[indexTrafoBarraJ].Trim() == rBarra[indexBarra].Trim())
                    {
                        varJkv = decimal.Parse(rBarra[indexVarVkv]);
                        varJpu = decimal.Parse(rBarra[indexVarVpu]);
                        anguloJ = double.Parse(rBarra[indexAngulo]);
                    }
                }

                //Obtiene diferencia angular
                double difAnguloIJ = anguloI - anguloJ;
                difAnguloIJ = (difAnguloIJ * Math.PI) / 180;

                //Obtiene Ntpmx
                valorNtpx = (varRMA1 - varWIND1) * (varNTP - 1);
                //Validación por denominador "0"
                decimal v1 = varRMA1 - varRMI1;
                v1 = (v1 != 0) ? v1 : 1;
                //---
                valorNtpx = valorNtpx / v1;
                valorNtpx = Math.Round(valorNtpx, 0);

                //Obtiene NOMV1p
                decimal v2 = varNTP - 1;
                v2 = (v2 != 0) ? v2 : 1;
                valorNOMV1p = (valorNtpx * (varRMA1 - varRMI1)) / v2;
                valorNOMV1p = varRMA1 - valorNOMV1p;
                valorNOMV1p = varIkv * valorNOMV1p;

                //Obtiene "utrnH" y "utrnL"
                if (varIkv > varJkv)
                {
                    valorUtrnH = (valorNOMV1p != 0) ? valorNOMV1p : 1;
                    valorUtrnL = (varJkv != 0) ? varJkv : 1;
                }
                else
                {
                    valorUtrnH = (varJkv != 0) ? varJkv : 1;
                    valorUtrnL = (valorNOMV1p != 0) ? valorNOMV1p : 1;
                }

                //Obtiene "tVipu" y "tVjpu"                
                valortVipu = (varIpu * varIkv) / valorUtrnH;
                valortVjpu = (varJpu * varJkv) / valorUtrnL;

                //Calculo de la Potencia de envío
                valorPotEnvioActiva = (-1 * valorK1) * (decimal)(Math.Cos(difAnguloIJ));
                valorPotEnvioActiva = valorPotEnvioActiva + (valorK2 * (decimal)(Math.Sin(difAnguloIJ)));
                valorPotEnvioActiva = valorPotEnvioActiva * valortVipu * valortVjpu;
                valorPotEnvioActiva = valorPotEnvioActiva + (valortVipu * valortVipu * valorK1);
                valorPotEnvioActiva = valorPotEnvioActiva * 100;

                valorPotEnvioReactiva = (-1 * valorK1) * (decimal)(Math.Sin(difAnguloIJ));
                valorPotEnvioReactiva = valorPotEnvioReactiva - (valorK2 * (decimal)(Math.Cos(difAnguloIJ)));
                valorPotEnvioReactiva = valorPotEnvioReactiva * valortVipu * valortVjpu;
                valorPotEnvioReactiva = valorPotEnvioReactiva + (valortVipu * valortVipu * valorK2);
                valorPotEnvioReactiva = valorPotEnvioReactiva * 100;

                valorPotEnvioAparente = valorPotEnvioActiva * valorPotEnvioActiva;
                valorPotEnvioAparente = valorPotEnvioAparente + (valorPotEnvioReactiva * valorPotEnvioReactiva);
                valorPotEnvioAparente = (decimal)Math.Sqrt((double)valorPotEnvioAparente);

                //Calculo de la Potencia de recepción
                valorPotRecepcionActiva = (-1 * valorK1) * (decimal)(Math.Cos(difAnguloIJ));
                valorPotRecepcionActiva = valorPotRecepcionActiva - (valorK2 * (decimal)(Math.Sin(difAnguloIJ)));
                valorPotRecepcionActiva = valorPotRecepcionActiva * valortVipu * valortVjpu;
                valorPotRecepcionActiva = valorPotRecepcionActiva + (valortVjpu * valortVjpu * valorK1);
                valorPotRecepcionActiva = valorPotRecepcionActiva * 100;

                valorPotRecepcionReactiva = valorK1 * (decimal)(Math.Sin(difAnguloIJ));
                valorPotRecepcionReactiva = valorPotRecepcionReactiva - (valorK2 * (decimal)(Math.Cos(difAnguloIJ)));
                valorPotRecepcionReactiva = valorPotRecepcionReactiva * valortVipu * valortVjpu;
                valorPotRecepcionReactiva = valorPotRecepcionReactiva + (valortVjpu * valortVjpu * valorK2);
                valorPotRecepcionReactiva = valorPotRecepcionReactiva * 100;

                valorPotRecepcionAparente = valorPotRecepcionActiva * valorPotRecepcionActiva;
                valorPotRecepcionAparente = valorPotEnvioAparente + (valorPotRecepcionReactiva * valorPotRecepcionReactiva);
                valorPotRecepcionAparente = (decimal)Math.Sqrt((double)valorPotRecepcionAparente);

                //Cálculo de las pérdidas en línea
                valorPerdidasTrafos = valorPotEnvioActiva + valorPotRecepcionActiva;

                //Cálculo de cargabilidad máxima
                decimal porEnvio = (valorPotEnvioAparente / varCap) * 100;
                decimal porRecepcion = (valorPotRecepcionAparente / varCap) * 100;
                valorCargabilidadMaxima = (porEnvio > porRecepcion) ? porEnvio : porRecepcion;
                #endregion

                #region Carga la lista de entidades
                //a) Potencia de envio
                //a.1) activa
                valorPotEnvioActiva = Math.Round(valorPotEnvioActiva, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidadEnvio,
                    Prnvarcodi = ConstantesProdem.TransPotActivaMW,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpTrafo,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorPotEnvioActiva
                };
                entities.Add(entidadMedicion);

                //a.2) reactiva
                valorPotEnvioReactiva = Math.Round(valorPotEnvioReactiva, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidadEnvio,
                    Prnvarcodi = ConstantesProdem.TransPotReactivaMVAR,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpTrafo,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorPotEnvioReactiva
                };
                entities.Add(entidadMedicion);

                //a.3) aparente
                valorPotEnvioAparente = Math.Round(valorPotEnvioAparente, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidadEnvio,
                    Prnvarcodi = ConstantesProdem.TransPotAparenteMVA,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpTrafo,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorPotEnvioAparente
                };
                entities.Add(entidadMedicion);

                //b) Potencia de recepción
                //b.1) activa
                valorPotRecepcionActiva = Math.Round(valorPotRecepcionActiva, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.TransPotActivaMW,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpTrafo,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorPotRecepcionActiva
                };
                entities.Add(entidadMedicion);

                //b.2) reactiva
                valorPotRecepcionReactiva = Math.Round(valorPotRecepcionReactiva, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.TransPotReactivaMVAR,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpTrafo,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorPotRecepcionReactiva
                };
                entities.Add(entidadMedicion);

                //b.3) aparente
                valorPotRecepcionAparente = Math.Round(valorPotRecepcionAparente, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.TransPotAparenteMVA,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpTrafo,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorPotRecepcionAparente
                };
                entities.Add(entidadMedicion);

                //c.1) Cargabilidad máxima envio
                valorCargabilidadMaxima = Math.Round(valorCargabilidadMaxima, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidadEnvio,
                    Prnvarcodi = ConstantesProdem.TransCargaMaxima,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpTrafo,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorCargabilidadMaxima
                };
                entities.Add(entidadMedicion);

                //c.2) Cargabilidad máxima recepción
                valorCargabilidadMaxima = Math.Round(valorCargabilidadMaxima, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.TransCargaMaxima,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpTrafo,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorCargabilidadMaxima
                };
                entities.Add(entidadMedicion);

                //c.3) Pérdidas en trafo envio
                valorPerdidasTrafos = Math.Round(valorPerdidasTrafos, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidadEnvio,
                    Prnvarcodi = ConstantesProdem.TransPerdidasMW,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpTrafo,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorPerdidasTrafos
                };
                entities.Add(entidadMedicion);

                //c.3) Pérdidas en trafo recepción
                valorPerdidasTrafos = Math.Round(valorPerdidasTrafos, 4);
                entidadMedicion = new PrnEstimadorRawDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.TransPerdidasMW,
                    Etmrawfuente = fuente,
                    Etmrawtipomedi = ConstantesProdem.EtmrawtpTrafo,
                    Etmrawfecha = fechaMedicion,
                    Etmrawvalor = valorPerdidasTrafos
                };
                entities.Add(entidadMedicion);
                #endregion

                //Cada registro de transformadores se compone de 4 lineas
                i += 4;
            }
            return entities;
        }

        /// <summary>
        /// General el modelo de datos del módulo de Consultas al estimador TNA
        /// </summary>
        /// <param name="idRegistro">Identificador de la unidad o asociación</param>
        /// <param name="idVariable">Identificador del tipo de energía (var y mvar)</param>
        /// <param name="idFuente">Identificador de la fuente de la información</param>
        /// <param name="idTipo">Identificador del tipo de registro (unidad o asociación)</param>
        /// <param name="regFecha">Identificador de la fecha de busqueda</param>
        /// <param name="selHistoricos">Fechas historicas seleccionadas</param>
        /// <param name="modulo">modulo de donde viene la consulta</param>
        /// <returns></returns>
        public object ConsultaEstimadorDatos(int idRegistro, int idVariable, int idFuente,
            string idTipo, DateTime regFecha, List<string> selHistoricos, int modulo)
        {
            List<object> data = new List<object>();
            //a) Obtiene los datos del modelo            
            PrnPatronModel modelo = this.ObtenerHistoricosMedicionesRaw(idRegistro,
                idTipo, regFecha, idVariable, idFuente, modulo);

            #region Utiliza los días filtro seleccionados
            if (selHistoricos != null)
            {
                foreach (string entHistorico in selHistoricos)
                {
                    decimal[] dataMedicion = new decimal[0];
                    string[] entidad = entHistorico.Split(':');
                    int indexEntidad = Convert.ToInt32(entidad[0]);
                    string fechaEntidad = entidad[1];
                    DateTime parseFecha = DateTime.ParseExact(fechaEntidad, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
                    if (idTipo.Equals("unidad"))
                    {
                        dataMedicion = this.ArrayEstimadorRawPorUnidad(idRegistro, parseFecha, idVariable, idFuente, modulo);
                    }
                    if (idTipo.Equals("asociacion"))
                    {
                        dataMedicion = this.ArrayEstimadorRawPorAsociacion(idRegistro, parseFecha, idVariable, idFuente, modulo);
                    }

                    modelo.StrFechas[indexEntidad] = fechaEntidad;
                    modelo.StrFechasTarde[indexEntidad] = fechaEntidad;
                    modelo.Mediciones[indexEntidad] = dataMedicion;
                }
            }
            #endregion

            #region Completa los intervalos faltantes
            int subFuente = ConstantesProdem.EtmrawfntSco;
            MePtomedicionDTO entSubRegistro = this.ObtenerUnidadesPorId(idRegistro)
                .FirstOrDefault(x => x.Origlectcodi == ConstantesProdem.OriglectcodiTnaSco)
                ?? new MePtomedicionDTO();

            for (int i = 0; i < modelo.Mediciones.Count; i++)
            {
                if (modelo.StrFechas[i].Equals("No encontrada")) continue;

                // V1. Reemplaza los intervalos faltantes por información del SCO
                decimal[] subMedicion = new decimal[0];
                DateTime parseFecha = DateTime.ParseExact(modelo.StrFechas[i], ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

                if (idTipo.Equals("unidad"))
                    subMedicion = this.ArrayEstimadorRawPorUnidad(entSubRegistro.Ptomedicodi, parseFecha, idVariable, subFuente, modulo);
                if (idTipo.Equals("asociacion"))
                    subMedicion = this.ArrayEstimadorRawPorAsociacion(idRegistro, parseFecha, idVariable, subFuente, modulo);
                modelo.Mediciones[i] = UtilProdem.CompletarMedicionRaw(modelo.Mediciones[i], subMedicion);

                //V2. Si existe algun intervalo vacio lo completa con el
                //promedio aritmético entre los intervalos más cercanos, siempre que estos sean diferentes de 0
                //No considera el intervalo inicial ni el ultimo                
                for (int j = 1; j < modelo.Mediciones[i].Count() - 1; j++)
                {
                    if (modelo.Mediciones[i][j] != 0) continue;
                    if (modelo.Mediciones[i][j - 1] == 0) continue;
                    if (modelo.Mediciones[i][j + 1] == 0) continue;
                    modelo.Mediciones[i][j] = (modelo.Mediciones[i][j - 1] + modelo.Mediciones[i][j + 1]) / 2;
                    j++;
                }
            }
            #endregion

            //b) Formatos de presentación
            object entity;
            //b.1) Intervalos de tiempo
            entity = new
            {
                id = "intervalos",
                label = "Hora",
                data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                htrender = "hora",
                hcrender = "categoria"
            };
            data.Add(entity);

            //b.2) Mediciones
            for (int i = 0; i < modelo.Mediciones.Count; i++)
            {
                entity = new
                {
                    id = "med" + (i + 1).ToString(),
                    label = modelo.StrFechas[i],
                    data = modelo.Mediciones[i],
                    htrender = "normal",
                    hcrender = "normal",
                    label2 = modelo.StrFechasTarde[i],
                    slunes = modelo.EsLunes
                };
                data.Add(entity);
            }
            return new { data = data, valid = true };
        }

        /// <summary>
        /// Método que devuelve los días historicos de un tipo de información del estimador TNA
        /// </summary>
        /// <param name="idRegistro">Identificador de una unidad o asociación</param>
        /// <param name="idTipo">Flag del tipo de busqueda</param>
        /// <param name="regFecha">Fecha de busqueda (debe contener solo el día, el mes y los años; los minutos al mínimo)</param>
        /// <param name="idVariable">Tipo de variable VAR, MVAR, etc</param>
        /// <param name="idFuente">Fuente de informacion</param>
        /// <param name="modulo">Modulo de donde viene la consulta</param>
        /// <returns></returns>
        public PrnPatronModel ObtenerHistoricosMedicionesRaw(int idRegistro, string idTipo,
            DateTime regFecha, int idVariable, int idFuente, int modulo)
        {
            int numDias = 7;
            PrnPatronModel model = new PrnPatronModel();
            List<PrnEstimadorRawDTO> entities = new List<PrnEstimadorRawDTO>();
            DateTime tempDate = new DateTime();
            PrnMedicionesRawDTO tempEntity = new PrnMedicionesRawDTO();

            List<PrnEstimadorRawDTO> dataMediciones = new List<PrnEstimadorRawDTO>();
            //Inicia el modelo
            model.EsLunes = false;
            model.Fechas = new List<DateTime>();
            model.StrFechas = new List<string>();
            model.StrFechasTarde = new List<string>();
            model.Mediciones = new List<decimal[]>();
            model.PrnMedicionesRaw = new List<PrnMedicionesRawDTO>();
            model.PrnEstimadorRaw = new List<PrnEstimadorRawDTO>();
            model.TipoPatron = ConstantesProdem.PatronByPromedio;

            //Rango de fechas para la busqueda
            DateTime fecInicio = regFecha.AddDays(1);
            DateTime fecLimite = regFecha.AddMonths(-6);

            //Obtiene los datos segun proceso
            if (idTipo == "unidad")
            {
                dataMediciones = this.ListEstimadorRawPorRangoPorUnidad(idRegistro,
                    fecLimite, fecInicio, idVariable, idFuente, modulo);
            }
            if (idTipo == "asociacion")
            {
                dataMediciones = this.ListEstimadorRawPorRangoPorAsociacion(idRegistro,
                    fecLimite, fecInicio, idVariable, idFuente, modulo);
            }
            entities = UtilProdem.ConvertirItvRawEnMediciones48(dataMediciones, idFuente);
            //Aplica las reglas de negocio
            tempDate = regFecha;
            int diaSemana = (int)regFecha.DayOfWeek;
            int maxDias = numDias;
            List<int> ruleA = new List<int> { ConstantesProdem.DiaAsIntLunes, ConstantesProdem.DiaAsIntSabado,
                ConstantesProdem.DiaAsIntDomingo };
            if (ruleA.Contains(diaSemana))
            {
                #region Lunes(Mañana), Domingo o Sabado
                int d = 0;
                while (d < maxDias)
                {
                    if (tempDate > fecLimite)
                    {
                        foreach (var a in entities)
                        {
                            if (a.Etmrawfecha.Equals(tempDate))
                            {
                                decimal[] dArray = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, a);
                                if (dArray.Sum() > 0)
                                {
                                    model.PrnEstimadorRaw.Add(a);
                                    model.Mediciones.Add(dArray);
                                    model.Fechas.Add(a.Etmrawfecha);
                                    model.StrFechas.Add(a.Etmrawfecha.ToString(ConstantesProdem.FormatoFecha));
                                    model.StrFechasTarde.Add(a.Etmrawfecha.ToString(ConstantesProdem.FormatoFecha));
                                    d++;
                                    break;
                                }
                            }
                        }
                        tempDate = tempDate.AddDays(-7);
                    }
                    else
                    {
                        break;
                    }
                }

                model.NDias = d;
                model.Mensaje = "Se capturaron " + d.ToString() + " mediciones historicas";
                #endregion
            }

            tempDate = regFecha;
            List<int> ruleB = new List<int> { ConstantesProdem.DiaAsIntMartes,
                ConstantesProdem.DiaAsIntMiercoles, ConstantesProdem.DiaAsIntJueves, ConstantesProdem.DiaAsIntViernes };
            if (ruleB.Contains(diaSemana))
            {
                #region Martes, Miercoles, Jueves o Viernes
                int d = 0;
                bool esLunes = false;
                if (diaSemana.Equals(ConstantesProdem.DiaAsIntLunes))
                {
                    esLunes = true;
                    maxDias = model.NDias;
                    model.EsLunes = true;
                    model.StrFechasTarde = new List<string>();
                }

                while (d < maxDias)
                {
                    diaSemana = (int)tempDate.DayOfWeek;
                    while (ruleA.Contains(diaSemana))
                    {
                        tempDate = tempDate.AddDays(-1);
                        diaSemana = (int)tempDate.DayOfWeek;
                    }
                    if (tempDate > fecLimite)
                    {
                        foreach (var a in entities)
                        {
                            if (a.Etmrawfecha.Equals(tempDate))
                            {
                                decimal[] dArray = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, a);
                                if (dArray.Sum() > 0)
                                {
                                    if (esLunes)
                                    {
                                        for (int i = ConstantesProdem.Itv30min / 2; i < ConstantesProdem.Itv30min; i++)
                                        {
                                            model.Mediciones[d][i] = dArray[i];
                                            model.PrnEstimadorRaw[d].GetType().GetProperty("H" + (i + 1).ToString()).
                                                SetValue(model.PrnEstimadorRaw[d], dArray[i]);
                                        }
                                        model.StrFechasTarde.Add(a.Etmrawfecha.ToString(ConstantesProdem.FormatoFecha));
                                        d++;
                                        break;
                                    }
                                    else
                                    {
                                        model.PrnEstimadorRaw.Add(a);
                                        model.Mediciones.Add(dArray);
                                        model.Fechas.Add(a.Etmrawfecha);
                                        model.StrFechas.Add(a.Etmrawfecha.ToString(ConstantesProdem.FormatoFecha));
                                        model.StrFechasTarde.Add(a.Etmrawfecha.ToString(ConstantesProdem.FormatoFecha));
                                        d++;
                                        break;
                                    }
                                }
                            }
                        }
                        tempDate = tempDate.AddDays(-1);
                    }
                    else
                    {
                        break;
                    }
                }

                if (!esLunes)
                {
                    model.NDias = d;
                    model.Mensaje = "Se capturaron " + d.ToString() + " mediciones historicas";
                }
                #endregion
            }

            //Completa las mediciones segun lo configurado
            int j = model.PrnEstimadorRaw.Count;
            maxDias = numDias;
            if (j != maxDias)
            {
                for (int i = 0; i < maxDias - j; i++)
                {
                    model.Mediciones.Add(new decimal[ConstantesProdem.Itv30min]);
                    model.PrnEstimadorRaw.Add(new PrnEstimadorRawDTO());
                    model.Fechas.Add(DateTime.MinValue);
                    model.StrFechas.Add("No encontrada");
                    model.StrFechasTarde.Add("No encontrada");
                }
            }
            //Calcula el perfil patrón
            model.Patron = UtilProdem.CalcularPerfilPatron(model.Mediciones,
            model.NDias, ConstantesProdem.Itv30min, ConstantesProdem.PatronByPromedio);
            return model;
        }

        /// <summary>
        /// Lista las mediciones raw por unidad y fecha
        /// </summary>
        /// <param name="asociacion">asociacion</param>
        /// <param name="fecha">fecha de inicio</param>
        /// <param name="idVariable">tipo de informacion VAR, MVAR, etc</param>
        /// <param name="idFuente">fuente de donde proviene la data</param>
        /// <param name="modulo">fuente de donde proviene la data</param>
        /// <returns></returns>
        public List<PrnMedicionesRawDTO> ListMedicionesRawPorAsociacion(int asociacion, DateTime fecha,
            int idVariable, string idFuente, string modulo)
        {
            return FactorySic.GetPrnMedicionesRawRepository()
                .ListMedicionesRawPorAsociacion(asociacion, fecha, idVariable, idFuente, modulo);
        }

        /// <summary>
        /// Lista los valores de las mediciones(Asociacion) de estimadores y lo devuelve como un array
        /// </summary>
        /// <param name="asociacion">asociacion</param>
        /// <param name="fecha">fecha de inicio</param>
        /// <param name="idVariable">tipo de informacion VAR, MVAR, etc</param>
        /// <param name="idFuente">fuente de donde proviene la data</param>
        /// <param name="modulo">fuente de donde proviene la data</param>
        /// <returns></returns>
        public decimal[] ArrayEstimadorRawPorAsociacion(int asociacion, DateTime fecha,
            int idVariable, int idFuente, int modulo)
        {
            DateTime fechaFin = fecha.AddDays(1);
            List<PrnEstimadorRawDTO> listaHorizontal = this.ListEstimadorRawPorRangoPorAsociacion(asociacion,
                fecha, fechaFin, idVariable, idFuente, modulo);
            List<PrnEstimadorRawDTO> listaVertical = UtilProdem.ConvertirItvRawEnMediciones48(listaHorizontal, idFuente);
            decimal[] array = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, listaVertical.FirstOrDefault());

            return array;
        }

        /// <summary>
        /// Lista los valores de las mediciones(unidad) de estimadores y lo devuelve como un array
        /// </summary>
        /// <param name="unidad">asociacion</param>
        /// <param name="fecha">fecha de inicio</param>
        /// <param name="idVariable">tipo de informacion VAR, MVAR, etc</param>
        /// <param name="idFuente">fuente de donde proviene la data</param>
        /// <param name="modulo">fuente de donde proviene la data</param>
        /// <returns></returns>
        public decimal[] ArrayEstimadorRawPorUnidad(int unidad, DateTime fecha, int idVariable, int idFuente, int modulo)
        {
            DateTime fechaFin = fecha.AddDays(1);
            List<PrnEstimadorRawDTO> listaHorizontal = this.ListEstimadorRawPorRangoPorUnidad(unidad,
                fecha, fechaFin, idVariable, idFuente, modulo);
            List<PrnEstimadorRawDTO> listaVertical = UtilProdem.ConvertirItvRawEnMediciones48(listaHorizontal, idFuente);
            decimal[] array = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, listaVertical.FirstOrDefault());

            return array;
        }

        /// <summary>
        /// Lista las mediciones raw por unidad y fecha
        /// </summary>
        /// <param name="unidad">unidades</param>
        /// <param name="fecha">fecha de inicio</param>
        /// <param name="idVariable">tipo de informacion VAR, MVAR, etc</param>
        /// <param name="idFuente">fuente de donde proviene la data</param>
        /// <param name="modulo">fuente de donde proviene la data</param>
        /// <returns></returns>
        public List<PrnMedicionesRawDTO> ListMedicionesRaw(int unidad, DateTime fecha, int idVariable, string idFuente, string modulo)
        {
            return FactorySic.GetPrnMedicionesRawRepository().ListMedicionesRaw(unidad, fecha, idVariable, idFuente, modulo);
        }

        /// <summary>
        /// Lista de unidades del estimador(puntos de medición) por tipo(origen de lectura)
        /// </summary>
        /// <param name="tipo">Identificador del tipo estimador a buscar</param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListUnidadesEstimadorByTipo(int tipo)
        {
            return FactorySic.GetPrnMediciongrpRepository().ListUnidadesEstimadorByTipo(tipo);
        }

        /// <summary>
        /// Lista los tipos de variable por tipo de unidad
        /// </summary>
        /// <returns></returns>
        public List<PrnVariableDTO> ListVariableByTipo(string tipo)
        {
            return FactorySic.GetPrnVariableRepository().ListVariableByTipo(tipo);
        }

        /// <summary>
        /// Lista las formulas por tipo(usuario)
        /// </summary>
        /// <returns></returns>
        public List<MePerfilRuleDTO> ListaPerfilRuleForEstimador()
        {
            string prefijo = ConstantesProdem.prefijoFormulasTNA + "%";
            return FactorySic.GetPrnPronosticoDemandaRepository().ListPerfilRuleByEstimador(prefijo);
        }

        /// <summary>
        /// Lista de asociaciones por tipo
        /// </summary>
        /// <param name="idTipo"></param>
        /// <returns></returns>
        public List<PrnAsociacionDTO> ListaAsociacionByTipo(string idTipo)
        {
            return FactorySic.GetPrnAsociacionRepository().ListUnidadAgrupadaByTipo(idTipo);
        }


        /// <summary>
        /// Elimina la asociacion de unidades segun el tipo
        /// </summary>
        /// <param name="tipo">Indica el tipo de unidad que se desea eliminar: Generador, Barra, Shunt, etc</param>
        /// <returns></returns>
        public void DeleteAsociacionByTipo(string tipo)
        {
            try
            {
                FactorySic.GetPrnAsociacionRepository().DeleteAsociacionDetalleByTipo(tipo);
                FactorySic.GetPrnAsociacionRepository().DeleteAsociacionByTipo(tipo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Lista las asocianes con los puntos de medicion que le pertenecen.
        /// </summary>
        /// <param name="tipo">Indica el tipo de unidad que se desea listar: Generador, Barra, Shunt, etc</param>
        /// <returns></returns>
        public List<PrnAsociacionDTO> ListAsociacionPuntosMedicionByTipo(string tipo)
        {
            List<PrnAsociacionDTO> listaPrincipal = new List<PrnAsociacionDTO>();
            List<PrnAsociacionDTO> listaDetalle = new List<PrnAsociacionDTO>();
            List<PrnAsociacionDTO> asociaciones = new List<PrnAsociacionDTO>();
            PrnAsociacionDTO asociacion = new PrnAsociacionDTO();

            listaPrincipal = FactorySic.GetPrnAsociacionRepository().ListUnidadAgrupadaByTipo(tipo);
            listaDetalle = FactorySic.GetPrnAsociacionRepository().ListAsociacionDetalleByTipo(tipo);

            foreach (var principal in listaPrincipal)
            {
                asociacion = new PrnAsociacionDTO();
                asociacion.Asociacodi = principal.Asociacodi;
                asociacion.Asocianom = principal.Asocianom;
                asociacion.Detalle = new List<int>();
                foreach (var detalle in listaDetalle.Where(x => x.Asociacodi == principal.Asociacodi))
                {
                    asociacion.Detalle.Add(detalle.Ptomedicodi);
                }

                asociaciones.Add(asociacion);
            }

            return asociaciones;
        }

        /// <summary>
        /// Registra las asociaciones con los puntos de medicion que le pertenecen.
        /// </summary>
        /// <param name="idTipo">Indica el tipo de unidad que se desea listar: Generador, Barra, Shunt, etc</param>
        /// <param name="asociaciones">Indica el tipo de unidad que se desea listar: Generador, Barra, Shunt, etc</param>
        /// <returns></returns>
        public List<PrnAsociacionDTO> SaveAsociacion(int idTipo, List<PrnAsociacionDTO> asociaciones)
        {
            try
            {
                string strTipo = string.Empty;
                if (idTipo == ConstantesProdem.EtmrawtpGenerador)
                    strTipo = ConstantesProdem.EtmrawtpStrGenerador;
                if (idTipo == ConstantesProdem.EtmrawtpBarra)
                    strTipo = ConstantesProdem.EtmrawtpStrBarra;
                if (idTipo == ConstantesProdem.EtmrawtpTrafo)
                    strTipo = ConstantesProdem.EtmrawtpStrTrafo;
                if (idTipo == ConstantesProdem.EtmrawtpCarga)
                    strTipo = ConstantesProdem.EtmrawtpStrCarga;
                if (idTipo == ConstantesProdem.EtmrawtpShunt)
                    strTipo = ConstantesProdem.EtmrawtpStrShunt;
                if (idTipo == ConstantesProdem.EtmrawtpLinea)
                    strTipo = ConstantesProdem.EtmrawtpStrLinea;

                //Elimina todos los registros del tipo
                DeleteAsociacionByTipo(strTipo);

                PrnAsociacionDTO asociacion = new PrnAsociacionDTO();
                PrnAsociacionDTO asociacionDetalle = new PrnAsociacionDTO();
                if (asociaciones != null)
                {
                    foreach (var principal in asociaciones)
                    {
                        asociacion = new PrnAsociacionDTO();
                        asociacion.Asocianom = principal.Asocianom;
                        asociacion.Asociatipomedi = strTipo;
                        int codigo = FactorySic.GetPrnAsociacionRepository().SaveReturnId(asociacion);

                        foreach (var detalle in principal.Detalle)
                        {
                            asociacionDetalle = new PrnAsociacionDTO();
                            asociacionDetalle.Asociacodi = codigo;
                            asociacionDetalle.Ptomedicodi = detalle;
                            asociacionDetalle.Asodettipomedi = strTipo;

                            FactorySic.GetPrnAsociacionRepository().SaveDetalle(asociacionDetalle);
                        }
                    }
                }

                List<PrnAsociacionDTO> lista = FactorySic.GetPrnAsociacionRepository()
                    .ListUnidadAgrupadaByTipo(strTipo);
                return lista;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite registra una medición del archivo Raw en la bd validando su existencia
        /// </summary>
        /// <param name="intervaloMedicion">Intervalo de medición afectado</param>        
        /// <param name="valorIntervalo">Valor del intervalo afectado</param>
        /// <param name="entidad">Entidad que representa un registro de la tabla PrnMedicionRaw</param>
        public void RegistrarMedicionRaw(string intervaloMedicion, decimal valorIntervalo, PrnMedicionesRawDTO entidad)
        {
            PrnMedicionesRawDTO entity = GetByKeyPrnMedicionesRaw(entidad);
            if (entity.Medirawcodi != 0)
            {
                entity.GetType().GetProperty(intervaloMedicion).SetValue(entity, valorIntervalo);
                UpdatePrnMedicionesRaw(entity);
            }
            else
            {
                entidad.GetType().GetProperty(intervaloMedicion).SetValue(entidad, valorIntervalo);
                SavePrnMedicionesRaw(entidad);
            }
        }

        /// <summary>
        /// Permite registra un punto de medición (unidad de archivo raw) en la bd validando su existencia
        /// </summary>
        /// <param name="entidad">Entidad que representa un registro de la tabla MePtomedicion</param>
        /// <param name="listaPuntos">Lista de puntos de medición registrados en la bd</param>
        /// <returns></returns>
        public int RegistrarPtoMedicionRaw(MePtomedicionDTO entidad, List<MePtomedicionDTO> listaPuntos)
        {
            MePtomedicionDTO existe = listaPuntos
                .Where(x => x.Ptomedidesc.Equals(entidad.Ptomedidesc))
                .FirstOrDefault() ?? new MePtomedicionDTO();

            return (existe.Ptomedicodi != 0)
                ? existe.Ptomedicodi
                : SaveMePtomedicion(entidad);
        }

        /// <summary>
        /// Lista las mediciones raw por unidad y fecha
        /// </summary>
        /// <param name="unidad">Identificador de la unidad</param>
        /// <param name="fechaInicio">fecha de inicio</param>
        /// <param name="fechaFin">fecha de inicio</param>
        /// <param name="idVariable">Identificador de tipo de variable VAR, MVAR, etc</param>
        /// <param name="idFuente">Identificador de la fuente de información</param>
        /// <param name="modulo">Identificador del módulo de consulta</param>
        /// <returns></returns>
        public List<PrnEstimadorRawDTO> ListEstimadorRawPorRangoPorUnidad(int unidad,
            DateTime fechaInicio, DateTime fechaFin, int idVariable, int idFuente, int modulo)
        {
            return FactorySic.GetPrnEstimadorRawRepository()
                .ListEstimadorRawPorRangoPorUnidad(unidad, fechaInicio, fechaFin, idVariable, idFuente, modulo);
        }

        /// <summary>
        /// Lista las mediciones raw por asociación y fecha
        /// </summary>
        /// <param name="asociacion">Identificador de la asociación</param>
        /// <param name="fechaInicio">fecha de inicio</param>
        /// <param name="fechaFin">fecha de inicio</param>
        /// <param name="idVariable">Identificador de tipo de variable VAR, MVAR, etc</param>
        /// <param name="idFuente">Identificador de la fuente de información</param>
        /// <param name="modulo">Identificador del módulo de consulta</param>
        /// <returns></returns>
        public List<PrnEstimadorRawDTO> ListEstimadorRawPorRangoPorAsociacion(int asociacion,
            DateTime fechaInicio, DateTime fechaFin, int idVariable, int idFuente, int modulo)
        {
            return FactorySic.GetPrnEstimadorRawRepository()
                .ListEstimadorRawPorRangoPorAsociacion(asociacion, fechaInicio, fechaFin, idVariable, idFuente, modulo);
        }

        #endregion

        #region Métodos del Módulo de Configuración al Estimador    

        /// <summary>
        /// Método que devuelve las relaciones tna y su detalle (relación barra/fórmula)
        /// </summary>
        /// <returns></returns>
        public List<PrnRelacionTnaDTO> ListaRelacionTna()
        {
            List<PrnRelacionTnaDTO> listaEntidades = this.ListPrnRelacionTna();
            List<PrnRelacionTnaDTO> listaDetalle = this.ListPrnRelacionTnaDetalle();

            List<PrnRelacionTnaDTO> listaRelacionados;
            foreach (PrnRelacionTnaDTO entidad in listaEntidades)
            {
                entidad.Detalle = new List<dynamic[]>();
                listaRelacionados = new List<PrnRelacionTnaDTO>();
                listaRelacionados = listaDetalle
                    .Where(e => e.Reltnacodi == entidad.Reltnacodi)
                    .ToList();
                foreach (PrnRelacionTnaDTO e in listaRelacionados)
                {
                    entidad.Detalle.Add(new dynamic[] {
                        e.Barracodi,
                        e.Barranom,
                        e.Reltnadetformula,
                        e.Formulanomb
                    });
                }
            }

            return listaEntidades;
        }

        /// <summary>
        /// Método que importa y procesa los datos de un archivo raw
        /// </summary>
        /// <param name="fechaImportacion">Fecha para la importación</param>
        /// <param name="direccion">Dirección de los archivos</param>
        /// <returns></returns>
        public object CfgEstimadorImportarArchivos(string fechaImportacion, string direccion)
        {
            string msg = "Se importaron los datos correctamente " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            string type = ConstantesProdem.MsgSuccess;
            DateTime parseFecha = DateTime.ParseExact(fechaImportacion,
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            //string dirtempo = direccion.Substring(35);
            //bool existe = FileServer.VerificarExistenciaDirectorio(direccion, "\\");
            bool existe = FileServer.VerificarLaExistenciaDirectorio(direccion);
            if (!existe)
            {
                msg = "Ingrese una dirección valida";
                type = ConstantesProdem.MsgWarning;
                return new { msg, type };
            }

            List<FileData> archivos = FileServer.ListarArhivos(string.Empty, direccion);
            int countArchivos = archivos.Count();
            if (countArchivos == 0)
            {
                msg = "No existen archivos con la extención raw dentro de la carpeta";
                type = ConstantesProdem.MsgWarning;
                return new { msg, type };
            }

            foreach (FileData archivo in archivos)
            {
                try
                {
                    string[] nombreArchivo = archivo.FileName.Split(new char[] { '_', '.' });
                    string strFechaMedicion = nombreArchivo[1].Substring(0, 8);
                    string strIntervaloMedicion = nombreArchivo[1].Substring(8, 4).Insert(2, ":");
                }
                catch
                {
                    msg = "Alguno de los archivos a importar no contiene el formato correcto";
                    type = ConstantesProdem.MsgWarning;
                    return new { msg, type };
                }
            }

            try
            {
                this.ObtenerDatosArchivoRaw(direccion, ConstantesProdem.EtmrawfntIeod,
                    parseFecha, string.Empty, new List<string>());
                msg += " - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                type = ConstantesProdem.MsgError;
                return new { msg, type };
            }

            return new { msg, type };
        }

        /// <summary>
        /// Método que devuelve el juego de datos del módulo Configuración TNA - porAportes
        /// </summary>
        /// <param name="idRelacion">Identificador de la relación a buscar</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public object CfgEstimadorRelacionDatos(int idRelacion,
            string regFecha)
        {
            List<object> data;
            List<PrnMediciongrpDTO> datosBarras;
            List<PrnRelacionTnaDTO> listaRelaciones;            
            DateTime parseFecha = DateTime.ParseExact(regFecha,
                ConstantesProdem.FormatoFecha,
                CultureInfo.InvariantCulture);
            PrnPatronModel modelo = UtilProdem
                .ObtenerFechasHistoricas(parseFecha, 7);

            listaRelaciones = this.ListPrnRelacionTna();
            PrnRelacionTnaDTO rel = listaRelaciones
                .FirstOrDefault(x => x.Reltnacodi == idRelacion)
                ?? new PrnRelacionTnaDTO();

            int diaSemana = (int)parseFecha.DayOfWeek;
            string tipoDia = "-1";
            if (diaSemana.Equals((int)DayOfWeek.Monday))
                tipoDia = "0";
            else if (diaSemana.Equals((int)DayOfWeek.Saturday))
                tipoDia = "1";
            else if (diaSemana.Equals((int)DayOfWeek.Sunday))
                tipoDia = "2";
            else
                tipoDia = "3";

            #region Obtiene los datos de las barras
            decimal[] datosMedicion;            
            datosBarras = new List<PrnMediciongrpDTO>();
            List<decimal[]> medPatron = new List<decimal[]>();
            foreach (DateTime f in modelo.Fechas)
            {
                PrnMediciongrpDTO entidad = new PrnMediciongrpDTO
                {
                    Grupocodi = rel.Barracodi,
                    Gruponomb = rel.Barranom,
                    Medifecha = f
                };

                if (rel.Reltnacodi != 0)
                    datosMedicion = this.ObtenerMedicionesCalculadas(rel.Reltnaformula, f);
                else
                    datosMedicion = new decimal[ConstantesProdem.Itv30min];

                medPatron.Add(datosMedicion);
                int i = 0;
                while (i < datosMedicion.Count())
                {
                    entidad.GetType()
                        .GetProperty("H" + (i + 1))
                        .SetValue(entidad, datosMedicion[i]);
                    i++;
                }
                datosBarras.Add(entidad);
            }
            #endregion

            #region Obtención de perfil patrón
            decimal[] dataAjuste = new decimal[ConstantesProdem.Itv30min];
            decimal[] dataPatron = UtilProdem.CalcularPerfilPatron(medPatron, 
                medPatron.Count, 
                ConstantesProdem.Itv30min,
                ConstantesProdem.PatronByPromedio);
            PrnPerfilBarraDTO entityDefecto = this
                .GetByIdsPrnPerfilBarra(idRelacion,
                tipoDia);
            decimal[] dataDefecto = UtilProdem
                .ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                entityDefecto);
            #endregion

            object entity;
            data = new List<object>();
            //a.1) Intervalos de tiempo
            entity = new
            {
                id = "intervalos",
                label = "Hora",
                data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                htrender = "hora",
                hcrender = "categoria"
            };
            data.Add(entity);

            //a.2) Mediciones
            for (int i = 0; i < datosBarras.Count; i++)
            {
                string id = "med" + (i + 1).ToString();
                string label = datosBarras[i].Medifecha.ToString(ConstantesProdem.FormatoFecha);
                decimal[] medicion = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, datosBarras[i]);

                entity = new
                {
                    id = id,
                    label = label,
                    data = medicion,
                    htrender = "normal",
                    hcrender = "normal"
                };
                data.Add(entity);
            }

            //b.1) Patron Base
            entity = new
            {
                id = "base",
                label = "Patrón Base",
                data = dataPatron,
                htrender = "normal",
                hcrender = "normal"
            };
            data.Add(entity);
            //b.2) Ajuste
            entity = new
            {
                id = "ajuste",
                label = "Ajuste",
                data = dataAjuste,
                htrender = "edit",
                hcrender = "no"
            };
            data.Add(entity);
            //b.3) Patrón Final
            entity = new
            {
                id = "final",
                label = "Patrón Final",
                data = dataPatron,
                htrender = "normal",
                hcrender = "normal"
            };
            data.Add(entity);
            //b.4) Patrón Defecto
            entity = new
            {
                id = "defecto",
                label = "Patrón Defecto",
                data = dataDefecto,
                htrender = "final",
                hcrender = "normal"
            };
            data.Add(entity);

            return new { data = data, valid = true };
        }

        /// <summary>
        /// Método que devuelve el juego de datos del módulo Configuración TNA - porAportes
        /// </summary>
        /// <param name="idRelacion">Identificador del registro cabecera de la relación</param>
        /// <param name="listaBarras">Lista de los ids de las barras seleccionadas para la consulta</param>
        /// <returns></returns>
        public object CfgEstimadorRelacionDatos2(int idRelacion, List<int> listaBarras)
        {
            List<object> data;
            List<PrnMediciongrpDTO> datosBarras;
            List<PrnRelacionTnaDTO> listaRelDetalle;
            string fechaRegistro = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            DateTime parseFecha = DateTime.ParseExact(ConstantesProdem.FormatoFecha,
                fechaRegistro, CultureInfo.InvariantCulture);
            PrnPatronModel modelo = UtilProdem.ObtenerFechasHistoricas(parseFecha, 7);

            listaRelDetalle = this.ListaRelacionTnaBarras(idRelacion);
            if (listaBarras.Count > 0)
            {
                listaRelDetalle.RemoveAll(x => !(listaBarras.Contains(x.Barracodi)));
            }

            #region Obtiene los datos de las barras
            datosBarras = new List<PrnMediciongrpDTO>();
            foreach (DateTime f in modelo.Fechas)
            {
                foreach (PrnRelacionTnaDTO rel in listaRelDetalle)
                {
                    PrnMediciongrpDTO entidad = new PrnMediciongrpDTO
                    {
                        Grupocodi = rel.Barracodi,
                        Gruponomb = rel.Barranom,
                        Medifecha = f
                    };
                    decimal[] datosMedicion = new decimal[ConstantesProdem.Itv30min];
                    //decimal[] datosMedicion = this.ObtenerMedicionesCalculadas(rel.Reltnadetformula, f);
                    int i = 0;
                    while (i < datosMedicion.Count())
                    {
                        entidad.GetType().GetProperty("H" + (i + 1)).SetValue(entidad, datosMedicion[i]);
                    }
                    datosBarras.Add(entidad);
                }
            }
            #endregion

            object entity;
            data = new List<object>();

            //a.1) Intervalos de tiempo
            entity = new
            {
                id = "intervalos",
                label = "Hora",
                data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                htrender = "hora",
                hcrender = "categoria"
            };
            data.Add(entity);

            //a.2) Mediciones
            foreach (PrnMediciongrpDTO e in datosBarras)
            {
                string id = e.Grupocodi.ToString()
                    + "_"
                    + e.Medifecha.ToString(ConstantesProdem.FormatoFecha);
                id = id.Replace("/", string.Empty);

                string label = e.Gruponomb
                    + "("
                    + e.Medifecha.ToString(ConstantesProdem.FormatoFecha)
                    + ")";

                decimal[] medicion = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, e);

                entity = new
                {
                    id = id,
                    label = label,
                    data = medicion,
                    htrender = "normal",
                    hcrender = "normal",
                };

                data.Add(entity);
            }

            return new { data = data, dates = modelo.StrFechas, valid = true };
        }

        /// <summary>
        /// Método que registra las relaciones entre barras y fórmulas
        /// en las tablas PRN_RELACIONTNA y PRN_RELACIONTNADETALLE
        /// </summary>
        /// <param name="listaRelaciones">Lista de entidades a registrar</param>
        /// <returns></returns>
        public string CfgEstimadorRegistrarRelacion(List<PrnRelacionTnaDTO> listaRelaciones)
        {
            if (listaRelaciones == null) listaRelaciones = new List<PrnRelacionTnaDTO>();

            List<PrnRelacionTnaDTO> registrosNuevos;
            List<PrnRelacionTnaDTO> registrosEliminados;
            List<PrnRelacionTnaDTO> registrosActualizados;
            string msg = "Las relaciones fueron actualizadas";
            List<PrnRelacionTnaDTO> entidades = this.ListaRelacionTna();
            if (entidades.Count < 0)
            {
                foreach (PrnRelacionTnaDTO entity in listaRelaciones)
                {
                    try
                    {
                        int id = this.SaveGetIdPrnRelacionTna(entity);
                        //entity.Detalle
                        //[0]: Barracodi, [1]: Barranomb,
                        //[2]: Reltnadetformula, [3]: Formulanomb
                        foreach (dynamic[] d in entity.Detalle)
                        {
                            PrnRelacionTnaDTO entidadDetalle = new PrnRelacionTnaDTO
                            {
                                Reltnacodi = id,
                                Barracodi = d[0],
                                Reltnadetformula = d[2]
                            };
                            this.SavePrnRelacionTnaDetalle(entidadDetalle);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        throw new Exception(ex.Message, ex);
                    }
                }
                return msg;
            }
            if (listaRelaciones.Count < 0)
            {
                foreach (PrnRelacionTnaDTO entity in listaRelaciones)
                {
                    this.DeletePrnRelacionTnaDetalle(entity.Reltnacodi);
                    this.DeletePrnRelacionTna(entity.Reltnacodi);
                }
                return msg;
            }

            #region Obtiene los registros nuevos
            registrosNuevos = listaRelaciones
                .Where(x => x.Reltnacodi == -1)
                .ToList();
            listaRelaciones.RemoveAll(x => x.Reltnacodi == -1);
            #endregion

            #region Obtiene los registros eliminados
            int[] idEntidades = entidades
                .Select(x => x.Reltnacodi)
                .ToArray();
            int[] idRelaciones = listaRelaciones
                .Select(x => x.Reltnacodi)
                .ToArray();

            int[] idEliminados = idEntidades
                .Where(x => !(idRelaciones.Contains(x)))
                .ToArray();
            registrosEliminados = entidades
                .Where(x => idEliminados.Contains(x.Reltnacodi))
                .ToList();
            entidades.RemoveAll(x => idEliminados.Contains(x.Reltnacodi));
            #endregion

            #region Obtiene los registros modificados
            entidades = entidades
                .OrderBy(x => x.Reltnacodi)
                .ToList();
            listaRelaciones = listaRelaciones
                .OrderBy(x => x.Reltnacodi)
                .ToList();
            registrosActualizados = new List<PrnRelacionTnaDTO>();
            for (int i = 0; i < listaRelaciones.Count; i++)
            {
                if (entidades[i].Reltnaformula != listaRelaciones[i].Reltnaformula)
                {
                    registrosActualizados.Add(listaRelaciones[i]);
                    continue;
                }
                if (entidades[i].Detalle.Count != listaRelaciones[i].Detalle.Count)
                {
                    registrosActualizados.Add(listaRelaciones[i]);
                    continue;
                }
                else
                {
                    List<dynamic> rRelaciones = listaRelaciones[i].Detalle
                        .Select(x => x[0] + "." + x[2])
                        .ToList();
                    List<dynamic> rEntidades = entidades[i].Detalle
                        .Select(x => x[0] + "." + x[2])
                        .ToList();
                    int c = rRelaciones
                        .Where(x => !(rEntidades.Contains(x)))
                        .ToList()
                        .Count;
                    if (c > 0)
                    {
                        registrosActualizados.Add(listaRelaciones[i]);
                        continue;
                    }
                }
            }

            #endregion

            #region Actualiza la BD
            if (registrosNuevos.Count > 0)
            {
                foreach (PrnRelacionTnaDTO entity in registrosNuevos)
                {
                    try
                    {
                        int id = this.SaveGetIdPrnRelacionTna(entity);
                        //entity.Detalle
                        //[0]: Barracodi, [1]: Barranomb,
                        //[2]: Reltnadetformula, [3]: Formulanomb
                        foreach (dynamic[] d in entity.Detalle)
                        {
                            PrnRelacionTnaDTO entidadDetalle = new PrnRelacionTnaDTO
                            {
                                Reltnacodi = id,
                                Barracodi = d[0],
                                Reltnadetformula = d[2]
                            };
                            this.SavePrnRelacionTnaDetalle(entidadDetalle);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        throw new Exception(ex.Message, ex);
                    }
                }
            }
            if (registrosEliminados.Count > 0)
            {
                foreach (PrnRelacionTnaDTO entity in registrosEliminados)
                {
                    this.DeletePrnRelacionTnaDetalle(entity.Reltnacodi);
                    this.DeletePrnRelacionTna(entity.Reltnacodi);
                }
            }
            if (registrosActualizados.Count > 0)
            {
                foreach (PrnRelacionTnaDTO entity in registrosActualizados)
                {
                    try
                    {
                        this.UpdatePrnRelacionTna(entity);
                        this.DeletePrnRelacionTnaDetalle(entity.Reltnacodi);
                        //entity.Detalle
                        //[0]: Barracodi, [1]: Barranomb,
                        //[2]: Reltnadetformula, [3]: Formulanomb
                        foreach (dynamic[] d in entity.Detalle)
                        {
                            PrnRelacionTnaDTO entidadDetalle = new PrnRelacionTnaDTO
                            {
                                Reltnacodi = entity.Reltnacodi,
                                Barracodi = d[0],
                                Reltnadetformula = d[2]
                            };
                            this.SavePrnRelacionTnaDetalle(entidadDetalle);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        throw new Exception(ex.Message, ex);
                    }
                }
            }
            #endregion

            return msg;
        }

        /// <summary>
        /// Lista parametros configurados de formulas 
        /// </summary>
        /// <returns></returns>
        public object ListConfiguracionFormulaByFiltros(int regIni, int regByPagina, string fecDesde, string fecHasta, List<int> listPuntos)//string formula)
        {

            DateTime tempDate = new DateTime();
            PrnConfiguracionFormulaDTO entity = new PrnConfiguracionFormulaDTO();
            List<PrnConfiguracionFormulaDTO> entitys = new List<PrnConfiguracionFormulaDTO>();

            //Validación
            if (listPuntos[0] == -1) return new { data = entitys, recordsTotal = 0, recordsFiltered = 0 };

            //Calcula el total de registros y los registros por página
            DateTime dDesde = DateTime.ParseExact(fecDesde, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime dHasta = DateTime.ParseExact(fecHasta, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            int diffDias = (dHasta - dDesde).Days + 1;
            int totalRegistros = diffDias * listPuntos.Count;

            List<MePerfilRuleDTO> formulas = this.ListaPerfilRuleForEstimador();
            formulas = formulas.Where(x => listPuntos.Contains(x.Prrucodi)).ToList();

            //Crea la lista completa de puntos de medición ordenados por fecha y punto
            int i = 0;
            while (i < diffDias)
            {
                tempDate = dDesde.AddDays(i);
                foreach (var ePunto in formulas)
                {
                    entity = new PrnConfiguracionFormulaDTO();
                    entity.Prrucodi = ePunto.Prrucodi;
                    entity.Prruabrev = ePunto.Prruabrev;
                    entity.Cnffrmfecha = tempDate;
                    entity.Strcnffrmfecha = tempDate.ToString(ConstantesProdem.FormatoFecha);
                    entitys.Add(entity);
                }
                i++;
            }

            //Obtiene el rango de registros que se mostraran
            if (totalRegistros > regByPagina)
            {
                int tempDiff = totalRegistros - regIni;
                entitys = (tempDiff < regByPagina) ? entitys.GetRange(regIni, tempDiff) : entitys.GetRange(regIni, regByPagina);
            }

            //Carga los datos de los registros a mostrar
            string sList = UtilProdem.ConvertirEntityListEnString(entitys, "Prrucodi");
            string minDtPag = (from d in entitys select d.Cnffrmfecha).Min().ToString(ConstantesProdem.FormatoFecha);
            string maxDtPag = (from d in entitys select d.Cnffrmfecha).Max().ToString(ConstantesProdem.FormatoFecha);
            List<PrnConfiguracionFormulaDTO> tempData = FactorySic.GetPrnConfiguracionFormulaRepository().ParametrosFormulasList(minDtPag, maxDtPag, sList);

            //Datos por defecto
            PrnConfiguracionFormulaDTO regDefecto = this.ParametrosFormulasGetDefecto(ConstantesProdem.DefectoByPunto);

            foreach (var ent in entitys)
            {
                var d = tempData.Find(x => x.Cnffrmformula.Equals(ent.Prrucodi) && x.Cnffrmfecha.Equals(ent.Cnffrmfecha));
                if (d != null)
                {
                    ent.Cnffrmcargamin = (d.Cnffrmcargamin != null) ? d.Cnffrmcargamin : regDefecto.Cnffrmcargamin;
                    ent.Cnffrmcargamax = (d.Cnffrmcargamax != null) ? d.Cnffrmcargamax : regDefecto.Cnffrmcargamax;
                    ent.Cnffrmtolerancia = (d.Cnffrmtolerancia != null) ? d.Cnffrmtolerancia : regDefecto.Cnffrmtolerancia;
                    ent.Cnffrmveda = (d.Cnffrmveda != null) ? d.Cnffrmveda : regDefecto.Cnffrmveda;
                    ent.Cnffrmferiado = (d.Cnffrmferiado != null) ? d.Cnffrmferiado : regDefecto.Cnffrmferiado;
                    ent.Cnffrmatipico = (d.Cnffrmatipico != null) ? d.Cnffrmatipico : regDefecto.Cnffrmatipico;
                    ent.Cnffrmdepauto = (d.Cnffrmdepauto != null) ? d.Cnffrmdepauto : regDefecto.Cnffrmdepauto;
                    ent.Cnffrmdiapatron = (d.Cnffrmdiapatron != null) ? d.Cnffrmdiapatron : regDefecto.Cnffrmdiapatron;
                    ent.Cnffrmpatron = (d.Cnffrmpatron != null) ? d.Cnffrmpatron : regDefecto.Cnffrmpatron;
                    ent.Cnffrmdefecto = (d.Cnffrmdefecto != null) ? d.Cnffrmdefecto : regDefecto.Cnffrmdefecto;
                }
                else
                {
                    ent.Cnffrmcargamin = regDefecto.Cnffrmcargamin;
                    ent.Cnffrmcargamax = regDefecto.Cnffrmcargamax;
                    ent.Cnffrmtolerancia = regDefecto.Cnffrmtolerancia;
                    ent.Cnffrmveda = regDefecto.Cnffrmveda;
                    ent.Cnffrmferiado = regDefecto.Cnffrmferiado;
                    ent.Cnffrmatipico = regDefecto.Cnffrmatipico;
                    ent.Cnffrmdepauto = regDefecto.Cnffrmdepauto;
                    ent.Cnffrmdiapatron = regDefecto.Cnffrmdiapatron;
                    ent.Cnffrmpatron = regDefecto.Cnffrmpatron;
                    ent.Cnffrmdefecto = regDefecto.Cnffrmdefecto;
                }
            }

            return new { data = entitys, recordsTotal = totalRegistros, recordsFiltered = totalRegistros };
        }

        /// <summary>
        /// Registra el perfíl defecto para la relación barra TNA
        /// </summary>
        /// <param name="idRelacion">Identificador de la relación TNA</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="datosMedicion">Datos de la medición</param>
        /// <returns></returns>
        public object CfgEstimadorRelacionRegistrarDefecto(int idRelacion, 
            string regFecha, 
            decimal[] datosMedicion)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;

            int diaSemana = (int)DateTime.ParseExact(regFecha,
                ConstantesProdem.FormatoFecha,
                CultureInfo.InvariantCulture)
                .DayOfWeek;
            string tipoDia = "-1";
            if (diaSemana.Equals((int)DayOfWeek.Monday))
                tipoDia = "0";
            else if (diaSemana.Equals((int)DayOfWeek.Saturday))
                tipoDia = "1";
            else if (diaSemana.Equals((int)DayOfWeek.Sunday))
                tipoDia = "2";
            else 
                tipoDia = "3";

            PrnPerfilBarraDTO entity = this
                .GetByIdsPrnPerfilBarra(idRelacion, tipoDia);

            int i = 0;
            while (i < ConstantesProdem.Itv30min)
            {
                entity.GetType()
                    .GetProperty("H" + (i + 1).ToString())
                    .SetValue(entity, (decimal)datosMedicion[i]);
                i++;
            }

            if (entity.Prfbarcodi != 0)
            {
                this.UpdatePrnPerfilBarra(entity);
                dataMsg = $"Perfil defecto actualizado para el día {regFecha}";
                typeMsg = ConstantesProdem.MsgSuccess;
            }
            else
            {
                entity.Reltnacodi = idRelacion;
                entity.Prfbartipodia = tipoDia;
                this.SavePrnPerfilBarra(entity);
                dataMsg = $"Perfil defecto registrado para el día {regFecha}"; 
                typeMsg = ConstantesProdem.MsgSuccess;
            }

            return new { dataMsg, typeMsg };
        }

        /// <summary>
        /// Registra (Save y Update) los nuevos parámetros de configuración
        /// </summary>
        /// <param name="fecDesde"></param>
        /// <param name="fecHasta"></param>
        /// <param name="dataParametros"></param>
        /// <param name="listPuntos"></param>
        /// <param name="nomUsuario"></param>
        public object ParametrosFormulasSave(DateTime fecDesde, DateTime fecHasta, PrnConfiguracionFormulaDTO dataParametros,
            List<int> listPuntos, string nomUsuario)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;
            PrnConfiguracionFormulaDTO entity = new PrnConfiguracionFormulaDTO();

            if (listPuntos.Count != 0)
            {
                foreach (var ePunto in listPuntos)
                {
                    DateTime tempDate = fecDesde.AddDays(-1);
                    while (tempDate < fecHasta)
                    {
                        tempDate = tempDate.AddDays(1);
                        entity = new PrnConfiguracionFormulaDTO();
                        entity = FactorySic.GetPrnConfiguracionFormulaRepository().GetIdByCodigoFecha(ePunto, tempDate);

                        if (entity.Cnffrmformula != 0)
                        {
                            if (dataParametros.Cnffrmcargamin != null) entity.Cnffrmcargamin = dataParametros.Cnffrmcargamin;
                            if (dataParametros.Cnffrmcargamax != null) entity.Cnffrmcargamax = dataParametros.Cnffrmcargamax;
                            if (dataParametros.Cnffrmtolerancia != null) entity.Cnffrmtolerancia = dataParametros.Cnffrmtolerancia;
                            if (dataParametros.Cnffrmveda != null) entity.Cnffrmveda = dataParametros.Cnffrmveda;
                            if (dataParametros.Cnffrmferiado != null) entity.Cnffrmferiado = dataParametros.Cnffrmferiado;
                            if (dataParametros.Cnffrmatipico != null) entity.Cnffrmatipico = dataParametros.Cnffrmatipico;
                            if (dataParametros.Cnffrmdepauto != null) entity.Cnffrmdepauto = dataParametros.Cnffrmdepauto;
                            if (dataParametros.Cnffrmdiapatron != null) entity.Cnffrmdiapatron = dataParametros.Cnffrmdiapatron;
                            if (dataParametros.Cnffrmpatron != null) entity.Cnffrmpatron = dataParametros.Cnffrmpatron;
                            if (dataParametros.Cnffrmdefecto != null) entity.Cnffrmdefecto = dataParametros.Cnffrmdefecto;
                            entity.Cnffrmusumodificacion = nomUsuario;
                            entity.Cnffrmfecmodificacion = DateTime.Now;
                            this.UpdatePrnConfiguracionFormula(entity);
                        }
                        else
                        {
                            entity = dataParametros;
                            entity.Cnffrmformula = ePunto;
                            entity.Cnffrmfecha = tempDate;
                            entity.Cnffrmusucreacion = nomUsuario;
                            entity.Cnffrmfeccreacion = DateTime.Now;
                            //entity.Cnffrmusumodificacion = nomUsuario;
                            //entity.Cnffrmfecmodificacion = DateTime.Now;
                            this.SavePrnConfiguracionFormula(entity);
                        }
                    }
                }
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El registro se realizó de manera exitosa";
            }
            else
            {
                typeMsg = ConstantesProdem.MsgWarning;
                dataMsg = "Debe seleccionar al menos un punto de medición";
            }

            return new { typeMsg, dataMsg };
        }

       
        /// <summary>
        /// Obtiene los parámetros de configuración por defecto
        /// </summary>
        /// <param name="idDefecto"></param>
        /// <returns></returns>
        public PrnConfiguracionFormulaDTO ParametrosFormulasGetDefecto(int idDefecto)
        {
            DateTime defDate = DateTime.ParseExact
                (ConstantesProdem.DefectoDate, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            return this.GetByIdPrnConfiguracionFormulas(idDefecto, defDate);
        }

        /// <summary>
        /// Actualiza los parámetros de configuración por defecto
        /// </summary>
        /// <param name="idDefecto"></param>
        /// <param name="dataParametros"></param>
        /// <param name="nomUsuario"></param>
        /// <returns></returns>
        public object ParametrosFormulasUpdateDefecto(int idDefecto, PrnConfiguracionFormulaDTO dataParametros, string nomUsuario)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;
            PrnConfiguracionFormulaDTO entity = this.ParametrosFormulasGetDefecto(idDefecto);

            if (entity.Cnffrmformula != 0)
            {
                if (dataParametros.Cnffrmcargamin != null) entity.Cnffrmcargamin = dataParametros.Cnffrmcargamin;
                if (dataParametros.Cnffrmcargamax != null) entity.Cnffrmcargamax = dataParametros.Cnffrmcargamax;
                if (dataParametros.Cnffrmveda != null) entity.Cnffrmveda = dataParametros.Cnffrmveda;
                if (dataParametros.Cnffrmferiado != null) entity.Cnffrmferiado = dataParametros.Cnffrmferiado;
                if (dataParametros.Cnffrmatipico != null) entity.Cnffrmatipico = dataParametros.Cnffrmatipico;
                if (dataParametros.Cnffrmdepauto != null) entity.Cnffrmdepauto = dataParametros.Cnffrmdepauto;
                if (dataParametros.Cnffrmtolerancia != null) entity.Cnffrmtolerancia = dataParametros.Cnffrmtolerancia;

                if (dataParametros.Cnffrmdiapatron != null) entity.Cnffrmdiapatron = dataParametros.Cnffrmdiapatron;
                if (dataParametros.Cnffrmpatron != null) entity.Cnffrmpatron = dataParametros.Cnffrmpatron;
                if (dataParametros.Cnffrmdefecto != null) entity.Cnffrmdefecto = dataParametros.Cnffrmdefecto;
                entity.Cnffrmusumodificacion = nomUsuario;
                entity.Cnffrmfecmodificacion = DateTime.Now;
                this.UpdatePrnConfiguracionFormulas(entity);

                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "Parámetros por defecto actualizados correctamente!";
            }

            return new { typeMsg, dataMsg };
        }

        /// <summary>
        /// Actualiza un registro de la tabla PRN_CONFIGURACIONFORMULA
        /// </summary>
        public void UpdatePrnConfiguracionFormulas(PrnConfiguracionFormulaDTO entity)
        {
            FactorySic.GetPrnConfiguracionFormulaRepository().Update(entity);
        }

        /// <summary>
        /// Lista las relaciones TNA
        /// </summary>
        public List<PrnRelacionTnaDTO> ListaRegistros()
        {
            return FactorySic.GetPrnRelacionTnaRepository().List();
        }

        /// <summary>
        /// Lista las barras CP relacionadas por ID
        /// </summary>
        public List<PrnRelacionTnaDTO> ListaRegistrosBarrasById(int codigo)
        {
            return FactorySic.GetPrnRelacionTnaRepository().ListRelacionTnaDetalleById(codigo);
        }

        /// <summary>
        /// Lista las barras CP relacionadas
        /// </summary>
        public List<PrnRelacionTnaDTO> ListaRegistrosBarras()
        {
            return FactorySic.GetPrnRelacionTnaRepository().ListRelacionTnaDetalle();
        }

        /// <summary>
        /// Lista las barras CP relacionadas
        /// </summary>
        public List<PrnRelacionTnaDTO> ListaRelacionTnaBarras(int codigo)
        {
            //if (codigo == -1)
            //{
            //    return this.ListaRegistrosBarras();
            //}
            //else {
            return this.ListaRegistrosBarrasById(codigo);
            //}
        }

        /// <summary>
        /// General el modelo de datos del módulo de Configuracion Estimador - Aportes
        /// </summary>
        /// <param name="idRegistro"></param>
        /// <param name="idBarra"></param>
        /// <param name="regFecha"></param>
        /// <returns></returns>
        public object ConfiguracionEstimadorDatos(int idRegistro, List<int> idBarra, DateTime regFecha)
        {
            List<object> data = new List<object>();
            //a) Obtiene los datos del modelo
            PrnPatronModel fechas = UtilProdem.ObtenerFechasHistoricas(regFecha, 7);
            List<PrnMediciongrpDTO> aportes = new List<PrnMediciongrpDTO>();

            foreach (var item in fechas.Fechas)
            {
                aportes.AddRange(this.ObtenerMedicionesFactorAporte(idRegistro, idBarra, item));
            }

            //b) Formatos de presentación
            object entity;
            //b.1) Intervalos de tiempo
            entity = new
            {
                id = "intervalos",
                label = "Hora",
                data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                htrender = "hora",
                hcrender = "categoria"
            };
            data.Add(entity);

            //b.2) Mediciones
            if (idRegistro == -1)
            {
                for (int i = 0; i < 7; i++)//modelo.mediciones.count
                {
                    entity = new
                    {
                        id = "med" + (i + 1).ToString(),
                        label = "No encontrado",
                        labelFecha = new string[7],
                        data = new decimal[48],
                        htrender = "normal",
                        hcrender = "normal"
                    };
                    data.Add(entity);
                }
            }
            else
            {
                for (int i = 0; i < aportes.Count; i++)//modelo.mediciones.count
                {
                    entity = new
                    {
                        id = "med" + (i + 1).ToString(),
                        label = aportes[i].Gruponomb + " - " + (aportes[i].Medifecha).ToString(ConstantesProdem.FormatoFecha),
                        labelFecha = fechas.StrFechas,
                        data = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, aportes[i]),
                        htrender = "normal",
                        hcrender = "normal"
                    };
                    data.Add(entity);
                }
            }
           
            return new { data = data, valid = true };
        }

        /// <summary>
        /// Método que devuelve los días historicos de Aportes
        /// </summary>
        /// <param name="idRegistro"></param>
        /// <param name="idBarra"></param>
        /// <param name="regFecha"></param>
        /// <returns></returns>
        public List<PrnMediciongrpDTO> ObtenerMedicionesFactorAporte(int idRegistro, List<int> idBarra, DateTime regFecha)
        {
            List<PrnMediciongrpDTO> datos = new List<PrnMediciongrpDTO>();
            PrnMediciongrpDTO dato = new PrnMediciongrpDTO();
            List<decimal[]> dataMediciones = new List<decimal[]>();
            List<PrnRelacionTnaDTO> puntos = this.ListaRelacionTnaBarras(idRegistro);
            decimal[] total = new decimal[48];

            if (puntos.Count == 0)
            {
                decimal[] mediciones = new decimal[48];
                dataMediciones.Add(mediciones);
            }
            else
            {
                foreach (var punto in puntos)
                {
                    decimal[] mediciones = this.ObtenerMedicionesCalculadas(punto.Reltnadetformula, regFecha);
                    dataMediciones.Add(mediciones);
                }
            }
            //Recorrido para formar array con las suma de los otros.
            int x = 0;
            for (int i = 0; i < 48; i++)
            {
                foreach (decimal[] item in dataMediciones)
                {
                    total[x] += item[x];
                }
                x++;
            }

            //Recorrido para dividir los valores arrayt/total
            int y = 0;
            for (int j = 0; j < 48; j++)
            {
                foreach (decimal[] item in dataMediciones)
                {
                    if (total[y] == 0)
                    {
                        item[y] = total[y];
                    }
                    else
                    {
                        item[y] = item[y] / total[y];
                    }
                }
                y++;
            }

            //Se arma la entidad con los datos 
            int n = 0;
            foreach (var item in puntos)
            {
                dato = new PrnMediciongrpDTO();
                dato.Reltnadetformula = item.Reltnadetformula;//ojo
                dato.Medifecha = regFecha;
                dato.Grupocodi = item.Barracodi;
                dato.Gruponomb = item.Barranom;
                for (int i = 0; i < 48; i++)
                {
                    dato.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(dato, dataMediciones[n][i]);
                }
                n++;
                datos.Add(dato);
            }

            //Validacion por si no selecciona nada en Barras
            if (idBarra[0] != -1)
            {
                datos = datos.Where(a => idBarra.Contains(a.Grupocodi)).ToList();
            }

            return datos;
        }

        /// <summary>
        /// Lista de datos para la tabla de configuración de días
        /// </summary>
        /// <param name="fechaIni">Fecha de inicio para la busqueda</param>
        /// <param name="fechaFin">Fecha de termino para la busqueda</param>
        /// <returns></returns>
        public List<string[]> CfgEstimadorCfgDiaDatos(string fechaIni, string fechaFin)
        {
            DateTime parseFechaIni = DateTime.ParseExact(fechaIni,
                ConstantesProdem.FormatoFecha,
                CultureInfo.InvariantCulture);
            DateTime parseFechaFin = DateTime.ParseExact(fechaFin,
                ConstantesProdem.FormatoFecha,
                CultureInfo.InvariantCulture);

            List<PrnConfiguracionDiaDTO> datos = this.CnfDiaObtenerPorRango(fechaIni, fechaFin);

            List<string[]> tabla = new List<string[]>();
            while (parseFechaIni <= parseFechaFin)
            {
                string[] fila = new string[4];
                PrnConfiguracionDiaDTO e = datos
                    .FirstOrDefault(x => x.Cnfdiafecha
                    .Equals(parseFechaIni)) 
                    ?? new PrnConfiguracionDiaDTO() { Cnfdiafecha = parseFechaIni };

                fila[0] = e.Cnfdiafecha.ToString(ConstantesProdem.FormatoFecha);
                fila[1] = (string.IsNullOrEmpty(e.Cnfdiaferiado)) ? "N" : e.Cnfdiaferiado;
                fila[2] = (string.IsNullOrEmpty(e.Cnfdiaatipico)) ? "N" : e.Cnfdiaatipico;
                fila[3] = (string.IsNullOrEmpty(e.Cnfdiaveda)) ? "N" : e.Cnfdiaveda;
                tabla.Add(fila);
                parseFechaIni = parseFechaIni.AddDays(1);
            }

            return tabla;
        }

        /// <summary>
        /// Registra la configuración de días para el modelo TNA
        /// </summary>
        /// <param name="parametros">Parametros de configuración a registrar</param>
        /// <param name="fechaIni">Fecha de inicio del rango de días</param>
        /// <param name="fechaFin">Fecha de termino del rango de días</param>
        /// <returns></returns>
        public object CfgEstimadorCfgDiaRegistrar(PrnConfiguracionDiaDTO parametros, 
            string fechaIni, 
            string fechaFin)
        {
            string typeMsg = ConstantesProdem.MsgSuccess;
            string dataMsg = "Los registros se actualizaron correctamente";

            List<PrnConfiguracionDiaDTO> datos = this.CnfDiaObtenerPorRango(fechaIni, fechaFin);

            DateTime parseFechaIni = DateTime.ParseExact(fechaIni,
                ConstantesProdem.FormatoFecha,
                CultureInfo.InvariantCulture);
            DateTime parseFechaFin = DateTime.ParseExact(fechaFin,
                ConstantesProdem.FormatoFecha,
                CultureInfo.InvariantCulture);

            while (parseFechaIni <= parseFechaFin)
            {
                PrnConfiguracionDiaDTO entity = datos
                    .FirstOrDefault(x => x.Cnfdiafecha
                    .Equals(parseFechaIni))
                    ?? new PrnConfiguracionDiaDTO() { Cnfdiafecha = parseFechaIni };

                entity.Cnfdiaferiado = string.IsNullOrEmpty(entity.Cnfdiaferiado) ? "N" : entity.Cnfdiaferiado;
                entity.Cnfdiaatipico = string.IsNullOrEmpty(entity.Cnfdiaatipico) ? "N" : entity.Cnfdiaatipico;
                entity.Cnfdiaveda = string.IsNullOrEmpty(entity.Cnfdiaveda) ? "N" : entity.Cnfdiaveda;

                entity.Cnfdiaferiado = (string.IsNullOrEmpty(parametros.Cnfdiaferiado))
                    ? entity.Cnfdiaferiado : parametros.Cnfdiaferiado;
                entity.Cnfdiaatipico = (string.IsNullOrEmpty(parametros.Cnfdiaatipico))
                    ? entity.Cnfdiaatipico : parametros.Cnfdiaatipico;
                entity.Cnfdiaveda = (string.IsNullOrEmpty(parametros.Cnfdiaveda))
                    ? entity.Cnfdiaveda : parametros.Cnfdiaveda;

                try
                {
                    if (entity.Cnfdiacodi != 0)
                        this.UpdatePrnConfiguracionDia(entity);
                    else
                        this.SavePrnConfiguracionDia(entity);
                }
                catch(Exception ex)
                {
                    typeMsg = ConstantesProdem.MsgError;
                    dataMsg = ex.Message;
                    break;
                }

                parseFechaIni = parseFechaIni.AddDays(1);
            }

            return new { typeMsg, dataMsg };
        }

        #endregion

        #region Métodos del Módulo de Desviacion
        /// <summary>
        /// General el modelo de datos del modulo de desviación
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public PrnMediciongrpDTO GetBarrasCPGroupByFechaTipo(int tipo, DateTime fecha)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().GetBarrasCPGroupByFechaTipo(tipo, fecha);
        }

        /// <summary>
        /// General el modelo de datos del modulo de desviaciones
        /// </summary>
        /// <param name="tipo">Identificador del tipo información (total, ul y reg)</param>
        /// <param name="grafico">Identificador del tipo de presentación (MW, %)</param>
        /// <param name="barra">Identificador de la barra seleccionada (selección unica, opción [Todos])</param>
        /// <param name="regFecha">Fecha de consulta</param>
        /// <returns></returns>
        public object ObtenerMedicionDesviacion(string tipo, string grafico, int barra, DateTime regFecha)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;
            List<object> data = new List<object>();
            PrnMediciongrpDTO medi = new PrnMediciongrpDTO();
            //a) Obtiene los datos del modelo
            PrnPatronModel fechas = UtilProdem.ObtenerFechasHistoricas(regFecha, 7);
            List<PrnMediciongrpDTO> medicion = new List<PrnMediciongrpDTO>();
            List<PrnMediciongrpDTO> listaMedIEOD = new List<PrnMediciongrpDTO>();
            List<PrnMediciongrpDTO> listaMedPDO = new List<PrnMediciongrpDTO>();
            List<PrnMediciongrpDTO> listaMedPSO = new List<PrnMediciongrpDTO>();

            switch (tipo)
            {
                //Generacion total COES + NO COES
                case "total":
                    //Datos de IEOD, PDO y PSO
                    if (barra == -2)
                    {
                        foreach (var f in fechas.Fechas)
                        {
                            string strFecha = f.ToString(ConstantesProdem.FormatoFecha);
                            int valDataIEOD = this.ValidarEjecucionPronosticoPorBarras(strFecha);
                            if (valDataIEOD == 0)
                            {
                                typeMsg = ConstantesProdem.MsgError;
                                dataMsg = "No se ha ejecutado el pronostico por barras para la fecha seleccionada...";
                            }
                            //IEOD
                            medi = new PrnMediciongrpDTO();
                            medi = this.GetBarrasCPGroupByFechaTipo(ConstantesProdem.PrnmgrtProdemBarra, f);
                            medi.Prnmgrtipo = ConstantesProdem.PrnmgrtProdemBarra;
                            medi.Medifecha = f;
                            listaMedIEOD.Add(medi);

                            int valDataPDO = this.ValidarEjecucionPronosticoPorBarras(strFecha);
                            if (valDataPDO == 0)
                            {
                                typeMsg = ConstantesProdem.MsgError;
                                dataMsg = "No se ha ejecutado el pronostico por barras para la fecha seleccionada...";
                            }
                            //PDO
                            medi = new PrnMediciongrpDTO();
                            medi = this.GetBarrasCPGroupByFechaTipo(ConstantesProdem.PrnmgrtProdemDesdeArea, f);
                            medi.Prnmgrtipo = ConstantesProdem.PrnmgrtProdemDesdeArea;
                            medi.Medifecha = f;
                            listaMedPDO.Add((medi));
                            //PSO
                            medi = new PrnMediciongrpDTO();
                            medi = this.GetBarrasCPGroupByFechaTipo(-1, f);
                            medi.Prnmgrtipo = -1;
                            medi.Medifecha = f;
                            listaMedPSO.Add((medi));
                        }
                    }
                    else
                    {
                        foreach (var f in fechas.Fechas)
                        {
                            string strFecha = f.ToString(ConstantesProdem.FormatoFecha);
                            int valDataIEOD = this.ValidarEjecucionPronosticoPorBarras(strFecha);
                            if (valDataIEOD == 0)
                            {
                                typeMsg = ConstantesProdem.MsgError;
                                dataMsg = "No se ha ejecutado el pronostico por barras para la fecha seleccionada...";
                            }
                            //IEOD
                            medi = new PrnMediciongrpDTO();
                            medi = this.GetByIdPrnMediciongrp(barra, ConstantesProdem.PrnmgrtProdemBarra, f);
                            medi.Prnmgrtipo = ConstantesProdem.PrnmgrtProdemBarra;
                            medi.Medifecha = f;
                            listaMedIEOD.Add(medi);

                            int valDataPDO = this.ValidarEjecucionPronosticoPorBarras(strFecha);
                            if (valDataPDO == 0)
                            {
                                typeMsg = ConstantesProdem.MsgError;
                                dataMsg = "No se ha ejecutado el pronostico por barras para la fecha seleccionada...";
                            }
                            //PDO
                            medi = new PrnMediciongrpDTO();
                            medi = this.GetByIdPrnMediciongrp(barra, ConstantesProdem.PrnmgrtProdemDesdeArea, f);
                            medi.Prnmgrtipo = ConstantesProdem.PrnmgrtProdemDesdeArea;
                            medi.Medifecha = f;
                            listaMedPDO.Add((medi));
                            //PSO
                            medi = new PrnMediciongrpDTO();
                            medi = this.GetByIdPrnMediciongrp(barra, -1, f);
                            medi.Prnmgrtipo = -1;
                            medi.Medifecha = f;
                            listaMedPSO.Add((medi));
                        }
                    }
                    //Calculando PDO y PSO %
                    if (grafico == "porcentaje")
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            PrnMediciongrpDTO medicionPDO = new PrnMediciongrpDTO();
                            medicionPDO.Medifecha = fechas.Fechas[i];
                            medicionPDO.Prnmgrtipo = 906;
                            PrnMediciongrpDTO medicionPSO = new PrnMediciongrpDTO();
                            medicionPSO.Medifecha = fechas.Fechas[i];
                            medicionPSO.Prnmgrtipo = 909;

                            for (int j = 1; j <= ConstantesProdem.Itv30min; j++)
                            {
                                decimal vIEOD = Convert.ToDecimal(listaMedIEOD[i].GetType().GetProperty("H" + j.ToString()).GetValue(listaMedIEOD[i], null));
                                decimal vPDO = Convert.ToDecimal(listaMedPDO[i].GetType().GetProperty("H" + j.ToString()).GetValue(listaMedPDO[i], null));
                                decimal vPSO = Convert.ToDecimal(listaMedPSO[i].GetType().GetProperty("H" + j.ToString()).GetValue(listaMedPSO[i], null));

                                //Obteniendo el PDO%
                                decimal pdoResta = ((vIEOD - vPDO) < 0) ? (vIEOD - vPDO) * -1 : (vIEOD - vPDO);
                                decimal pdoDivision = (vIEOD == 0) ? pdoResta : pdoResta / vIEOD;
                                decimal pdoResultado = pdoDivision * 100;
                                medicionPDO.GetType().GetProperty("H" + j.ToString()).SetValue(medicionPDO, pdoResultado);

                                //Obteniendo el PSO%
                                decimal psoResta = ((vIEOD - vPSO) < 0) ? (vIEOD - vPSO) * -1 : (vIEOD - vPSO);
                                decimal psoDivision = (vIEOD == 0) ? psoResta : psoResta / vIEOD;
                                decimal psoResultado = psoDivision * 100;
                                medicionPSO.GetType().GetProperty("H" + j.ToString()).SetValue(medicionPSO, psoResultado);
                            }

                            medicion.Add(medicionPDO);
                            medicion.Add(medicionPSO);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            medicion.Add(listaMedIEOD[i]);
                            medicion.Add(listaMedPDO[i]);
                            medicion.Add(listaMedPSO[i]);
                        }
                    }
                    break;
                //Demanda total Usuarios Libres
                case "ul":
                    //Datos de IEOD, PDO y PSO
                    if (barra == -2)
                    {
                        foreach (var f in fechas.Fechas)
                        {
                            //IEOD
                            medi = new PrnMediciongrpDTO();
                            medi = this.GetBarrasCPGroupByFechaTipo(ConstantesProdem.PrnmgrtDemIndustrial, f);
                            medi.Prnmgrtipo = ConstantesProdem.PrnmgrtDemIndustrial;
                            medi.Medifecha = f;
                            listaMedIEOD.Add(medi);
                            //PDO
                            medi = new PrnMediciongrpDTO();
                            medi = this.GetBarrasCPGroupByFechaTipo(ConstantesProdem.PrnmgrtDemIndDesdeArea, f);
                            medi.Prnmgrtipo = ConstantesProdem.PrnmgrtDemIndDesdeArea;
                            medi.Medifecha = f;
                            listaMedPDO.Add((medi));
                            //PSO
                            medi = new PrnMediciongrpDTO();
                            medi = this.GetBarrasCPGroupByFechaTipo(-1, f);
                            medi.Prnmgrtipo = -1;
                            medi.Medifecha = f;
                            listaMedPSO.Add((medi));
                        }
                    }
                    else
                    {
                        foreach (var f in fechas.Fechas)
                        {
                            //IEOD
                            medi = new PrnMediciongrpDTO();
                            medi = this.GetByIdPrnMediciongrp(barra, ConstantesProdem.PrnmgrtDemIndustrial, f);
                            medi.Prnmgrtipo = ConstantesProdem.PrnmgrtDemIndustrial;
                            medi.Medifecha = f;
                            listaMedIEOD.Add(medi);
                            //PDO
                            medi = new PrnMediciongrpDTO();
                            medi = this.GetByIdPrnMediciongrp(barra, ConstantesProdem.PrnmgrtDemIndDesdeArea, f);
                            medi.Prnmgrtipo = ConstantesProdem.PrnmgrtDemIndDesdeArea;
                            medi.Medifecha = f;
                            listaMedPDO.Add((medi));
                            //PSO
                            medi = new PrnMediciongrpDTO();
                            medi = this.GetByIdPrnMediciongrp(barra, -1, f);
                            medi.Prnmgrtipo = -1;
                            medi.Medifecha = f;
                            listaMedPSO.Add((medi));
                        }
                    }
                    //Calculando PDO y PSO % 
                    if (grafico == "porcentaje")
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            PrnMediciongrpDTO medicionPDO = new PrnMediciongrpDTO();
                            medicionPDO.Medifecha = fechas.Fechas[i];
                            medicionPDO.Prnmgrtipo = 907;
                            PrnMediciongrpDTO medicionPSO = new PrnMediciongrpDTO();
                            medicionPSO.Medifecha = fechas.Fechas[i];
                            medicionPSO.Prnmgrtipo = 910;

                            for (int j = 1; j <= ConstantesProdem.Itv30min; j++)
                            {
                                decimal vIEOD = Convert.ToDecimal(listaMedIEOD[i].GetType().GetProperty("H" + j.ToString()).GetValue(listaMedIEOD[i], null));
                                decimal vPDO = Convert.ToDecimal(listaMedPDO[i].GetType().GetProperty("H" + j.ToString()).GetValue(listaMedPDO[i], null));
                                decimal vPSO = Convert.ToDecimal(listaMedPSO[i].GetType().GetProperty("H" + j.ToString()).GetValue(listaMedPSO[i], null));

                                //Obteniendo el PDO%
                                decimal pdoResta = ((vIEOD - vPDO) < 0) ? (vIEOD - vPDO) * -1 : (vIEOD - vPDO);
                                decimal pdoDivision = (vIEOD == 0) ? pdoResta : pdoResta / vIEOD;
                                decimal pdoResultado = pdoDivision * 100;
                                medicionPDO.GetType().GetProperty("H" + j.ToString()).SetValue(medicionPDO, pdoResultado);

                                //Obteniendo el PSO%
                                decimal psoResta = ((vIEOD - vPSO) < 0) ? (vIEOD - vPSO) * -1 : (vIEOD - vPSO);
                                decimal psoDivision = (vIEOD == 0) ? psoResta : psoResta / vIEOD;
                                decimal psoResultado = psoDivision * 100;
                                medicionPSO.GetType().GetProperty("H" + j.ToString()).SetValue(medicionPSO, psoResultado);
                            }

                            medicion.Add(medicionPDO);
                            medicion.Add(medicionPSO);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            medicion.Add(listaMedIEOD[i]);
                            medicion.Add(listaMedPDO[i]);
                            medicion.Add(listaMedPSO[i]);
                        }
                    }
                    break;
                //Demanda total Vegetativa
                case "reg":
                    //Datos de IEOD, PDO y PSO
                    if (barra == -2)
                    {
                        foreach (var f in fechas.Fechas)
                        {
                            //IEOD
                            medi = new PrnMediciongrpDTO();
                            medi = this.GetBarrasCPGroupByFechaTipo(ConstantesProdem.PrnmgrtDemVegetativa, f);
                            medi.Prnmgrtipo = ConstantesProdem.PrnmgrtDemVegetativa;
                            medi.Medifecha = f;
                            listaMedIEOD.Add(medi);
                            //PDO
                            medi = new PrnMediciongrpDTO();
                            medi = this.GetBarrasCPGroupByFechaTipo(ConstantesProdem.PrnmgrtDemVegDesdeArea, f);
                            medi.Prnmgrtipo = ConstantesProdem.PrnmgrtDemVegDesdeArea;
                            medi.Medifecha = f;
                            listaMedPDO.Add((medi));
                            //PSO
                            medi = new PrnMediciongrpDTO();
                            medi = this.GetBarrasCPGroupByFechaTipo(-1, f);
                            medi.Prnmgrtipo = -1;
                            medi.Medifecha = f;
                            listaMedPSO.Add((medi));
                        }
                    }
                    else
                    {
                        foreach (var f in fechas.Fechas)
                        {
                            //IEOD
                            medi = new PrnMediciongrpDTO();
                            medi = this.GetByIdPrnMediciongrp(barra, ConstantesProdem.PrnmgrtDemVegetativa, f);
                            medi.Prnmgrtipo = ConstantesProdem.PrnmgrtDemVegetativa;
                            medi.Medifecha = f;
                            listaMedIEOD.Add(medi);
                            //PDO
                            medi = new PrnMediciongrpDTO();
                            medi = this.GetByIdPrnMediciongrp(barra, ConstantesProdem.PrnmgrtDemVegDesdeArea, f);
                            medi.Prnmgrtipo = ConstantesProdem.PrnmgrtDemVegDesdeArea;
                            medi.Medifecha = f;
                            listaMedPDO.Add((medi));
                            //PSO
                            medi = new PrnMediciongrpDTO();
                            medi = this.GetByIdPrnMediciongrp(barra, -1, f);
                            medi.Prnmgrtipo = -1;
                            medi.Medifecha = f;
                            listaMedPSO.Add((medi));
                        }
                    }
                    //Calculando PDO y PSO % 
                    if (grafico == "porcentaje")
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            PrnMediciongrpDTO medicionPDO = new PrnMediciongrpDTO();
                            medicionPDO.Medifecha = fechas.Fechas[i];
                            medicionPDO.Prnmgrtipo = 908;
                            PrnMediciongrpDTO medicionPSO = new PrnMediciongrpDTO();
                            medicionPSO.Medifecha = fechas.Fechas[i];
                            medicionPSO.Prnmgrtipo = 911;

                            for (int j = 1; j <= ConstantesProdem.Itv30min; j++)
                            {
                                decimal vIEOD = Convert.ToDecimal(listaMedIEOD[i].GetType().GetProperty("H" + j.ToString()).GetValue(listaMedIEOD[i], null));
                                decimal vPDO = Convert.ToDecimal(listaMedPDO[i].GetType().GetProperty("H" + j.ToString()).GetValue(listaMedPDO[i], null));
                                decimal vPSO = Convert.ToDecimal(listaMedPSO[i].GetType().GetProperty("H" + j.ToString()).GetValue(listaMedPSO[i], null));

                                //Obteniendo el PDO%
                                decimal pdoResta = ((vIEOD - vPDO) < 0) ? (vIEOD - vPDO) * -1 : (vIEOD - vPDO);
                                decimal pdoDivision = (vIEOD == 0) ? pdoResta : pdoResta / vIEOD;
                                decimal pdoResultado = pdoDivision * 100;
                                medicionPDO.GetType().GetProperty("H" + j.ToString()).SetValue(medicionPDO, pdoResultado);

                                //Obteniendo el PSO%
                                decimal psoResta = ((vIEOD - vPSO) < 0) ? (vIEOD - vPSO) * -1 : (vIEOD - vPSO);
                                decimal psoDivision = (vIEOD == 0) ? psoResta : psoResta / vIEOD;
                                decimal psoResultado = psoDivision * 100;
                                medicionPSO.GetType().GetProperty("H" + j.ToString()).SetValue(medicionPSO, psoResultado);
                            }

                            medicion.Add(medicionPDO);
                            medicion.Add(medicionPSO);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            medicion.Add(listaMedIEOD[i]);
                            medicion.Add(listaMedPDO[i]);
                            medicion.Add(listaMedPSO[i]);
                        }
                    }
                    break;
            }

            //b) Formatos de presentación
            object entity;
            //b.1) Intervalos de tiempo
            entity = new
            {
                id = "intervalos",
                label = "Hora",
                data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                htrender = "hora",
                hcrender = "categoria"
            };
            data.Add(entity);

            //b.2) Mediciones
            if (tipo == "-1" || grafico == "-1" || barra == -1)
            {
                for (int i = 0; i < 7; i++)
                {
                    entity = new
                    {
                        id = "med" + (i + 1).ToString(),
                        label = "No encontrado",
                        labelFecha = new string[7],
                        data = new decimal[48],
                        htrender = "normal",
                        hcrender = "normal"
                    };
                    data.Add(entity);
                }
            }
            else
            {
                for (int i = 0; i < medicion.Count; i++)
                {
                    string tipoInformacion = "";
                    switch (medicion[i].Prnmgrtipo)
                    {
                        case ConstantesProdem.PrnmgrtProdemBarra:
                        case ConstantesProdem.PrnmgrtDemIndustrial:
                        case ConstantesProdem.PrnmgrtDemVegetativa:
                            tipoInformacion = "IEOD";
                            break;
                        case ConstantesProdem.PrnmgrtProdemDesdeArea:
                        case ConstantesProdem.PrnmgrtDemIndDesdeArea:
                        case ConstantesProdem.PrnmgrtDemVegDesdeArea:
                            tipoInformacion = "PDO";
                            break;
                        case -1:
                            tipoInformacion = "PSO";
                            break;
                        case 906:
                        case 907:
                        case 908:
                            tipoInformacion = "PDO[%]";
                            break;
                        case 909:
                        case 910:
                        case 911:
                            tipoInformacion = "PSO[%]";
                            break;
                    }
                    entity = new
                    {
                        id = "med" + (i + 1).ToString(),
                        label = tipoInformacion + " " + (medicion[i].Medifecha).ToString(ConstantesProdem.FormatoFecha),
                        labelFecha = fechas.StrFechas,
                        data = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, medicion[i]),
                        htrender = "normal",
                        hcrender = "normal"
                    };
                    data.Add(entity);
                }
            }
            return new { data = data, valid = true, typeMsg, dataMsg };
        }

        /// <summary>
        /// Método que devuelve la medicion para un dia del módulo de desviaciones
        /// </summary>
        /// <param name="tipo">Identificador del tipo información (total, ul y reg)</param>
        /// <param name="grafico">Identificador del tipo de presentación (MW, %)</param>
        /// <param name="barra">Identificador de la barra seleccionada (selección unica, opción [Todos])</param>
        /// <param name="regFecha">Fecha de consulta</param>
        /// <returns></returns>
        public Tuple<List<PrnMediciongrpDTO>, string, string> ObtenerMedicionDesviacionCalendario(string tipo, string grafico, int barra, DateTime regFecha)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;
            List<PrnMediciongrpDTO> medicion = new List<PrnMediciongrpDTO>();
            PrnMediciongrpDTO listaMedIEOD = new PrnMediciongrpDTO();
            PrnMediciongrpDTO listaMedPDO = new PrnMediciongrpDTO();
            PrnMediciongrpDTO listaMedPSO = new PrnMediciongrpDTO();

            switch (tipo)
            {
                //Generacion total COES + NO COES
                case "total":
                    //Datos de IEOD, PDO y PSO
                    if (barra == -2)
                    {
                        string strFecha = regFecha.ToString(ConstantesProdem.FormatoFecha);
                        int valDataIEOD = this.ValidarEjecucionPronosticoPorBarras(strFecha);
                        if (valDataIEOD == 0)
                        {
                            typeMsg = ConstantesProdem.MsgError;
                            dataMsg = "No se ha ejecutado el pronostico por barras para la fecha seleccionada...";
                        }
                        //IEOD
                        listaMedIEOD = new PrnMediciongrpDTO();
                        listaMedIEOD = this.GetBarrasCPGroupByFechaTipo(ConstantesProdem.PrnmgrtProdemBarra, regFecha);
                        listaMedIEOD.Prnmgrtipo = ConstantesProdem.PrnmgrtProdemBarra;
                        listaMedIEOD.Medifecha = regFecha;

                        int valDataPDO = this.ValidarEjecucionPronosticoPorBarras(strFecha);
                        if (valDataPDO == 0)
                        {
                            typeMsg = ConstantesProdem.MsgError;
                            dataMsg = "No se ha ejecutado el pronostico por barras para la fecha seleccionada...";
                        }
                        //PDO
                        listaMedPDO = new PrnMediciongrpDTO();
                        listaMedPDO = this.GetBarrasCPGroupByFechaTipo(ConstantesProdem.PrnmgrtProdemDesdeArea, regFecha);
                        listaMedPDO.Prnmgrtipo = ConstantesProdem.PrnmgrtProdemDesdeArea;
                        listaMedPDO.Medifecha = regFecha;
                        //PSO
                        listaMedPSO = new PrnMediciongrpDTO();
                        listaMedPSO = this.GetBarrasCPGroupByFechaTipo(-1, regFecha);
                        listaMedPSO.Prnmgrtipo = -1;
                        listaMedPSO.Medifecha = regFecha;
                    }
                    else
                    {
                        string strFecha = regFecha.ToString(ConstantesProdem.FormatoFecha);
                        int valDataIEOD = this.ValidarEjecucionPronosticoPorBarras(strFecha);
                        if (valDataIEOD == 0)
                        {
                            typeMsg = ConstantesProdem.MsgError;
                            dataMsg = "No se ha ejecutado el pronostico por barras para la fecha seleccionada...";
                        }
                        //IEOD
                        listaMedIEOD = new PrnMediciongrpDTO();
                        listaMedIEOD = this.GetByIdPrnMediciongrp(barra, ConstantesProdem.PrnmgrtProdemBarra, regFecha);
                        listaMedIEOD.Prnmgrtipo = ConstantesProdem.PrnmgrtProdemBarra;
                        listaMedIEOD.Medifecha = regFecha;
                        int valDataPDO = this.ValidarEjecucionPronosticoPorBarras(strFecha);
                        if (valDataPDO == 0)
                        {
                            typeMsg = ConstantesProdem.MsgError;
                            dataMsg = "No se ha ejecutado el pronostico por barras para la fecha seleccionada...";
                        }
                        //PDO
                        listaMedPDO = new PrnMediciongrpDTO();
                        listaMedPDO = this.GetByIdPrnMediciongrp(barra, ConstantesProdem.PrnmgrtProdemDesdeArea, regFecha);
                        listaMedPDO.Prnmgrtipo = ConstantesProdem.PrnmgrtProdemDesdeArea;
                        listaMedPDO.Medifecha = regFecha;
                        //PSO
                        listaMedPSO = new PrnMediciongrpDTO();
                        listaMedPSO = this.GetByIdPrnMediciongrp(barra, -1, regFecha);
                        listaMedPSO.Prnmgrtipo = -1;
                        listaMedPSO.Medifecha = regFecha;

                    }
                    //Calculando PDO y PSO % 
                    if (grafico == "porcentaje")
                    {
                        PrnMediciongrpDTO medicionPDO = new PrnMediciongrpDTO();
                        medicionPDO.Medifecha = regFecha;
                        medicionPDO.Prnmgrtipo = 906;
                        PrnMediciongrpDTO medicionPSO = new PrnMediciongrpDTO();
                        medicionPSO.Medifecha = regFecha;
                        medicionPSO.Prnmgrtipo = 909;

                        for (int j = 1; j <= ConstantesProdem.Itv30min; j++)
                        {
                            decimal vIEOD = Convert.ToDecimal(listaMedIEOD.GetType().GetProperty("H" + j.ToString()).GetValue(listaMedIEOD, null));
                            decimal vPDO = Convert.ToDecimal(listaMedPDO.GetType().GetProperty("H" + j.ToString()).GetValue(listaMedPDO, null));
                            decimal vPSO = Convert.ToDecimal(listaMedPSO.GetType().GetProperty("H" + j.ToString()).GetValue(listaMedPSO, null));

                            //Obteniendo el PDO%
                            decimal pdoResta = ((vIEOD - vPDO) < 0) ? (vIEOD - vPDO) * -1 : (vIEOD - vPDO);
                            decimal pdoDivision = (vIEOD == 0) ? pdoResta : pdoResta / vIEOD;
                            decimal pdoResultado = pdoDivision * 100;
                            medicionPDO.GetType().GetProperty("H" + j.ToString()).SetValue(medicionPDO, pdoResultado);

                            //Obteniendo el PSO%
                            decimal psoResta = ((vIEOD - vPSO) < 0) ? (vIEOD - vPSO) * -1 : (vIEOD - vPSO);
                            decimal psoDivision = (vIEOD == 0) ? psoResta : psoResta / vIEOD;
                            decimal psoResultado = psoDivision * 100;
                            medicionPSO.GetType().GetProperty("H" + j.ToString()).SetValue(medicionPSO, psoResultado);
                        }

                        medicion.Add(medicionPDO);
                        medicion.Add(medicionPSO);
                    }
                    else
                    {
                        medicion.Add(listaMedIEOD);
                        medicion.Add(listaMedPDO);
                        medicion.Add(listaMedPSO);
                    }
                    break;
                //Demanda total Usuarios Libres
                case "ul":
                    //Datos de IEOD, PDO y PSO
                    if (barra == -2)
                    {
                        //IEOD
                        listaMedIEOD = new PrnMediciongrpDTO();
                        listaMedIEOD = this.GetBarrasCPGroupByFechaTipo(ConstantesProdem.PrnmgrtDemIndustrial, regFecha);
                        listaMedIEOD.Prnmgrtipo = ConstantesProdem.PrnmgrtDemIndustrial;
                        listaMedIEOD.Medifecha = regFecha;
                        //PDO
                        listaMedPDO = new PrnMediciongrpDTO();
                        listaMedPDO = this.GetBarrasCPGroupByFechaTipo(ConstantesProdem.PrnmgrtDemIndDesdeArea, regFecha);
                        listaMedPDO.Prnmgrtipo = ConstantesProdem.PrnmgrtDemIndDesdeArea;
                        listaMedPDO.Medifecha = regFecha;
                        //PSO
                        listaMedPSO = new PrnMediciongrpDTO();
                        listaMedPSO = this.GetBarrasCPGroupByFechaTipo(-1, regFecha);
                        listaMedPSO.Prnmgrtipo = -1;
                        listaMedPSO.Medifecha = regFecha;
                    }
                    else
                    {
                        //IEOD
                        listaMedIEOD = new PrnMediciongrpDTO();
                        listaMedIEOD = this.GetByIdPrnMediciongrp(barra, ConstantesProdem.PrnmgrtDemIndustrial, regFecha);
                        listaMedIEOD.Prnmgrtipo = ConstantesProdem.PrnmgrtDemIndustrial;
                        listaMedIEOD.Medifecha = regFecha;
                        //PDO
                        listaMedPDO = new PrnMediciongrpDTO();
                        listaMedPDO = this.GetByIdPrnMediciongrp(barra, ConstantesProdem.PrnmgrtDemIndDesdeArea, regFecha);
                        listaMedPDO.Prnmgrtipo = ConstantesProdem.PrnmgrtDemIndDesdeArea;
                        listaMedPDO.Medifecha = regFecha;
                        //PSO
                        listaMedPSO = new PrnMediciongrpDTO();
                        listaMedPSO = this.GetByIdPrnMediciongrp(barra, -1, regFecha);
                        listaMedPSO.Prnmgrtipo = -1;
                        listaMedPSO.Medifecha = regFecha;
                    }
                    //Calculando PDO y PSO % 
                    if (grafico == "porcentaje")
                    {
                        PrnMediciongrpDTO medicionPDO = new PrnMediciongrpDTO();
                        medicionPDO.Medifecha = regFecha;
                        medicionPDO.Prnmgrtipo = 907;
                        PrnMediciongrpDTO medicionPSO = new PrnMediciongrpDTO();
                        medicionPSO.Medifecha = regFecha;
                        medicionPSO.Prnmgrtipo = 910;

                        for (int j = 1; j <= ConstantesProdem.Itv30min; j++)
                        {
                            decimal vIEOD = Convert.ToDecimal(listaMedIEOD.GetType().GetProperty("H" + j.ToString()).GetValue(listaMedIEOD, null));
                            decimal vPDO = Convert.ToDecimal(listaMedPDO.GetType().GetProperty("H" + j.ToString()).GetValue(listaMedPDO, null));
                            decimal vPSO = Convert.ToDecimal(listaMedPSO.GetType().GetProperty("H" + j.ToString()).GetValue(listaMedPSO, null));

                            //Obteniendo el PDO%
                            decimal pdoResta = ((vIEOD - vPDO) < 0) ? (vIEOD - vPDO) * -1 : (vIEOD - vPDO);
                            decimal pdoDivision = (vIEOD == 0) ? pdoResta : pdoResta / vIEOD;
                            decimal pdoResultado = pdoDivision * 100;
                            medicionPDO.GetType().GetProperty("H" + j.ToString()).SetValue(medicionPDO, pdoResultado);

                            //Obteniendo el PSO%
                            decimal psoResta = ((vIEOD - vPSO) < 0) ? (vIEOD - vPSO) * -1 : (vIEOD - vPSO);
                            decimal psoDivision = (vIEOD == 0) ? psoResta : psoResta / vIEOD;
                            decimal psoResultado = psoDivision * 100;
                            medicionPSO.GetType().GetProperty("H" + j.ToString()).SetValue(medicionPSO, psoResultado);
                        }

                        medicion.Add(medicionPDO);
                        medicion.Add(medicionPSO);
                    }
                    else
                    {
                        medicion.Add(listaMedIEOD);
                        medicion.Add(listaMedPDO);
                        medicion.Add(listaMedPSO);
                    }
                    break;
                //Demanda total Vegetativa
                case "reg":
                    //Datos de IEOD, PDO y PSO
                    if (barra == -2)
                    {
                        //IEOD
                        listaMedIEOD = new PrnMediciongrpDTO();
                        listaMedIEOD = this.GetBarrasCPGroupByFechaTipo(ConstantesProdem.PrnmgrtDemVegetativa, regFecha);
                        listaMedIEOD.Prnmgrtipo = ConstantesProdem.PrnmgrtDemVegetativa;
                        listaMedIEOD.Medifecha = regFecha;
                        //PDO
                        listaMedPDO = new PrnMediciongrpDTO();
                        listaMedPDO = this.GetBarrasCPGroupByFechaTipo(ConstantesProdem.PrnmgrtDemVegDesdeArea, regFecha);
                        listaMedPDO.Prnmgrtipo = ConstantesProdem.PrnmgrtDemVegDesdeArea;
                        listaMedPDO.Medifecha = regFecha;
                        //PSO
                        listaMedPSO = new PrnMediciongrpDTO();
                        listaMedPSO = this.GetBarrasCPGroupByFechaTipo(-1, regFecha);
                        listaMedPSO.Prnmgrtipo = -1;
                        listaMedPSO.Medifecha = regFecha;
                    }
                    else
                    {
                        //IEOD
                        listaMedIEOD = new PrnMediciongrpDTO();
                        listaMedIEOD = this.GetByIdPrnMediciongrp(barra, ConstantesProdem.PrnmgrtDemVegetativa, regFecha);
                        listaMedIEOD.Prnmgrtipo = ConstantesProdem.PrnmgrtDemVegetativa;
                        listaMedIEOD.Medifecha = regFecha;
                        //PDO
                        listaMedPDO = new PrnMediciongrpDTO();
                        listaMedPDO = this.GetByIdPrnMediciongrp(barra, ConstantesProdem.PrnmgrtDemVegDesdeArea, regFecha);
                        listaMedPDO.Prnmgrtipo = ConstantesProdem.PrnmgrtDemVegDesdeArea;
                        listaMedPDO.Medifecha = regFecha;
                        //PSO
                        listaMedPSO = new PrnMediciongrpDTO();
                        listaMedPSO = this.GetByIdPrnMediciongrp(barra, -1, regFecha);
                        listaMedPSO.Prnmgrtipo = -1;
                        listaMedPSO.Medifecha = regFecha;
                    }
                    //Calculando PDO y PSO % 
                    if (grafico == "porcentaje")
                    {
                        PrnMediciongrpDTO medicionPDO = new PrnMediciongrpDTO();
                        medicionPDO.Medifecha = regFecha;
                        medicionPDO.Prnmgrtipo = 908;
                        PrnMediciongrpDTO medicionPSO = new PrnMediciongrpDTO();
                        medicionPSO.Medifecha = regFecha;
                        medicionPSO.Prnmgrtipo = 911;

                        for (int j = 1; j <= ConstantesProdem.Itv30min; j++)
                        {
                            decimal vIEOD = Convert.ToDecimal(listaMedIEOD.GetType().GetProperty("H" + j.ToString()).GetValue(listaMedIEOD, null));
                            decimal vPDO = Convert.ToDecimal(listaMedPDO.GetType().GetProperty("H" + j.ToString()).GetValue(listaMedPDO, null));
                            decimal vPSO = Convert.ToDecimal(listaMedPSO.GetType().GetProperty("H" + j.ToString()).GetValue(listaMedPSO, null));

                            //Obteniendo el PDO%
                            decimal pdoResta = ((vIEOD - vPDO) < 0) ? (vIEOD - vPDO) * -1 : (vIEOD - vPDO);
                            decimal pdoDivision = (vIEOD == 0) ? pdoResta : pdoResta / vIEOD;
                            decimal pdoResultado = pdoDivision * 100;
                            medicionPDO.GetType().GetProperty("H" + j.ToString()).SetValue(medicionPDO, pdoResultado);

                            //Obteniendo el PSO%
                            decimal psoResta = ((vIEOD - vPSO) < 0) ? (vIEOD - vPSO) * -1 : (vIEOD - vPSO);
                            decimal psoDivision = (vIEOD == 0) ? psoResta : psoResta / vIEOD;
                            decimal psoResultado = psoDivision * 100;
                            medicionPSO.GetType().GetProperty("H" + j.ToString()).SetValue(medicionPSO, psoResultado);
                        }

                        medicion.Add(medicionPDO);
                        medicion.Add(medicionPSO);
                    }
                    else
                    {
                        medicion.Add(listaMedIEOD);
                        medicion.Add(listaMedPDO);
                        medicion.Add(listaMedPSO);
                    }
                    break;
            }

            foreach (var item in medicion)
            {
                switch (item.Prnmgrtipo)
                {
                    case ConstantesProdem.PrnmgrtProdemBarra:
                    case ConstantesProdem.PrnmgrtDemIndustrial:
                    case ConstantesProdem.PrnmgrtDemVegetativa:
                        item.Gruponomb = "IEOD";
                        break;
                    case ConstantesProdem.PrnmgrtProdemDesdeArea:
                    case ConstantesProdem.PrnmgrtDemIndDesdeArea:
                    case ConstantesProdem.PrnmgrtDemVegDesdeArea:
                        item.Gruponomb = "PDO";
                        break;
                    case -1:
                        item.Gruponomb = "PSO";
                        break;
                    case 906:
                    case 907:
                    case 908:
                        item.Gruponomb = "PDO[%]";
                        break;
                    case 909:
                    case 910:
                    case 911:
                        item.Gruponomb = "PSO[%]";
                        break;
                }
            }
            Tuple<List<PrnMediciongrpDTO>, string, string> res = new Tuple<List<PrnMediciongrpDTO>, string, string>(medicion, typeMsg, dataMsg);
            return res;
        }
        #endregion

        #region Métodos del Módulo Traslado de Carga
        /// <summary>
        /// Permite obtener las mediciones de una barra segun su fecha y version
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="version"></param>
        /// <param name="barra"></param>
        /// <returns></returns>
        public PrnMediciongrpDTO MedicionBarraByFechaVersionBarra(string fecha, int version, int barra)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().MedicionBarraByFechaVersionBarra(fecha, version, barra);
        }

        /// <summary>
        /// General el modelo de datos del módulo de Traslado Carga
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="idOrigen"></param>
        /// <param name="idDestino"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public object ObtenerMedicionesBarras(int idVersion, int idOrigen, int idDestino, string fecha)
        {
            DateTime regFecha = DateTime.ParseExact(fecha,
                                    ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            List<object> data = new List<object>();
            //a) Obtiene los datos del modelo
            PrnPatronModel fechas = UtilProdem.ObtenerFechasHistoricas(regFecha, 7);
            List<PrnMediciongrpDTO> aportes = new List<PrnMediciongrpDTO>();

            //Data para la grilla
            PrnMediciongrpDTO aportesOrigen = this.MedicionBarraByFechaVersionBarra(fecha, idVersion, idOrigen);
            aportes.Add(aportesOrigen);
            PrnMediciongrpDTO aportesDestino = this.MedicionBarraByFechaVersionBarra(fecha, idVersion, idDestino);
            aportes.Add(aportesDestino);
            aportes.Add(aportesOrigen);
            aportes.Add(aportesDestino);
            aportes.Add(new PrnMediciongrpDTO());

            //Listas para las cabeceras, id de las cabeceras y otros.
            string[] listHeader = { "Demanda B. Origen Inicial", "Demanda B. Destino Inicial",
                "Demanda B. Origen Final", "Demanda B. Destino Final", "Potencia de Traslado" };
            string[] listIdHeader = { "origenInicial", "destinoInicial", "origenFinal", "destinoFinal", "potenciaTraslado" };
            string[] listHtRender = { "normal", "normal", "final", "final", "edit" };

            //b) Formatos de presentación
            object entity;
            //b.1) Intervalos de tiempo
            entity = new
            {
                id = "intervalos",
                label = "Hora",
                data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                htrender = "hora"
            };
            data.Add(entity);

            //b.2) Mediciones
            for (int i = 0; i < 5; i++)//numero de columnas
            {
                entity = new
                {
                    id = listIdHeader[i],
                    label = listHeader[i],
                    data = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, aportes[i]),
                    htrender = listHtRender[i]
                };
                data.Add(entity);
            }

            return new { data = data, valid = true };
        }

        /// <summary>
        /// Actualiza la tabla PRN_MEDICIONESGRP cuando se realiza un traslado de carga
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="version"></param>
        /// <param name="origen"></param>
        /// <param name="destino"></param>
        /// <param name="datos"></param>
        public object ActualizarMedicionesPorTrasladoCarga(string fecha, int version, int origen,
            int destino, decimal[][] datos)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;
            if (version != -1 && origen != -1 && destino != -1)
            {
                PrnMediciongrpDTO entity;

                #region Update Origen
                entity = new PrnMediciongrpDTO();
                entity.Grupocodi = origen;
                entity.Medifecha = DateTime.ParseExact(fecha,
                    ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
                entity.Prnmgrtipo = ConstantesProdem.PrnmgrtProdemBarra;
                entity.Vergrpcodi = version;
                entity.PrnmgrTraslado = "T";
                int i = 0;
                while (i < ConstantesProdem.Itv30min)
                {
                    entity.GetType().GetProperty("H" + (i + 1).ToString()).
                        SetValue(entity, datos[i][3]);
                    i++;
                }
                this.UpdateMedicionTrasladoCarga(entity);
                #endregion

                #region Update Destino
                entity = new PrnMediciongrpDTO();
                entity.Grupocodi = destino;
                entity.Medifecha = DateTime.ParseExact(fecha,
                    ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
                entity.Prnmgrtipo = ConstantesProdem.PrnmgrtProdemBarra;
                entity.Vergrpcodi = version;
                entity.PrnmgrTraslado = "T";
                int j = 0;
                while (j < ConstantesProdem.Itv30min)
                {
                    entity.GetType().GetProperty("H" + (j + 1).ToString()).
                        SetValue(entity, datos[j][4]);
                    j++;
                }
                this.UpdateMedicionTrasladoCarga(entity);
                #endregion

                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El traslado de carga se realizó de manera exitosa";
            }
            else
            {
                typeMsg = ConstantesProdem.MsgWarning;
                dataMsg = "Debe seleccionar todos los filtros...";
            }
            return new { typeMsg, dataMsg };
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateMedicionTrasladoCarga(PrnMediciongrpDTO entity)
        {
            FactorySic.GetPrnPronosticoDemandaRepository().UpdateMedicionTrasladoCarga(entity);
        }

        /// <summary>
        /// Lista solo las barras registradas como CP de la version activa para el Traslado
        /// </summary>
        /// <param name="idCategoria">Identificador de la categoría a buscar</param>
        /// <param name="idVersion">Identificador de la versión consultada</param>
        /// <returns></returns>
        public List<PrnMediciongrpDTO> GetListBarrasCPTraslado(int idCategoria, int idVersion)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository()
                .GetLisBarrasSoloCPTraslado(idCategoria, idVersion);
        }
        #endregion

        /*Tablas*/
        #region Métodos Tabla PRN_FORMULAREL

        /// <summary>
        /// Inserta un registro de la tabla PRN_FORMULAREL
        /// </summary>
        public void SavePrnFormularel(PrnFormularelDTO entity)
        {
            try
            {
                FactorySic.GetPrnFormularelRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PRN_FORMULAREL
        /// </summary>
        public void UpdatePrnFormularel(PrnFormularelDTO entity)
        {
            FactorySic.GetPrnFormularelRepository().Update(entity);
        }

        /// <summary>
        /// Elimina un registro de la tabla PRN_FORMULAREL
        /// </summary>
        public void DeletePrnFormularel(int ptomedicodi, int ptomedicodicalc)
        {
            FactorySic.GetPrnFormularelRepository().Delete(ptomedicodi, ptomedicodicalc);
        }

        /// <summary>
        /// Lista los registros de PRN_FORMULAREL
        /// </summary>
        public List<PrnFormularelDTO> ListPrnFormularel()
        {
            return FactorySic.GetPrnFormularelRepository().List();
        }

        /// <summary>
        /// Permite listar los puntos calculas registrados en la BD
        /// </summary>
        public List<PrnFormularelDTO> ListFormulasByUsuario()
        {
            return FactorySic.GetPrnFormularelRepository().ListFormulasByUsuario(ConstantesProdem.UsuariosValidos);
        }

        /// <summary>
        /// Permite listar las formulas registradas en en la tabla ME_PTOMEDICION por Ptomedicodi
        /// </summary>
        public List<PrnFormularelDTO> ListFormulasRelacionadas(int punto)
        {
            return FactorySic.GetPrnFormularelRepository().ListFormulasRelacionadas(punto, ConstantesProdem.UsuariosValidos);
        }

        /// <summary>
        /// Elimina un registro de la tabla PRN_FORMULAREL por PTOMEDICODI
        /// </summary>
        public void DeleteByPtoPrnFormularel(int ptomedicodi)
        {
            FactorySic.GetPrnFormularelRepository().DeleteByPtomedicodi(ptomedicodi);
        }

        #endregion

        #region Métodos Tabla EQ_EQUIPO

        /// <summary>
        /// Permite listar las subestación y empresas combinadas
        /// </summary>
        /// <returns></returns>
        public List<EqEquipoDTO> ListSubestacionEmpresa()
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().ListSubestacionEmpresa();
        }

        #endregion

        #region Métodos Tabla EQ_AREAREL
        public void SaveEqAreaRel(EqAreaRelDTO entity)
        {
            try
            {
                FactorySic.GetEqArearelRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region Métodos Tabla ME_PTOMEDICION

        /// <summary>
        /// Permite obtener un registro de la tabla ME_PTOMEDICION
        /// </summary>
        public MePtomedicionDTO GetByIdMePtomedicion(int ptomedicodi)
        {
            return FactorySic.GetMePtomedicionRepository().GetById(ptomedicodi);
        }

        /// <summary>
        /// Permite obtener los puntos de medición asociados al Pronóstico de la demanda
        /// </summary>
        /// <returns></returns>
        public List<PrnClasificacionDTO> ListProdemPuntos(int areacodi)
        {
            string nombreArea = "0";
            if (areacodi == ConstantesProdem.AreacodiASur) nombreArea = ConstantesProdem.AOperativaSur;
            else if (areacodi == ConstantesProdem.AreacodiANorte) nombreArea = ConstantesProdem.AOperativaNorte;
            else if (areacodi == ConstantesProdem.AreacodiACentro) nombreArea = ConstantesProdem.AOperativaCentro;
            else if (areacodi == ConstantesProdem.AreacodiASierraCentro) nombreArea = ConstantesProdem.AOperativaSCentro;

            List<PrnClasificacionDTO> ListaProdem = new List<PrnClasificacionDTO>();
            ListaProdem = FactorySic.GetPrnClasificacionRepository().ListProdemPuntos(nombreArea);

            //Solo Distribuidoras y UL
            ListaProdem = ListaProdem.Where(x => x.Tipoemprcodi == ConstantesProdem.TipoemprcodiDistribuidores
                || x.Tipoemprcodi == ConstantesProdem.TipoemprcodiUsuLibres).ToList();

            return ListaProdem;
        }

        /// <summary>
        /// Inserta un registro de la tabla ME_PTOMEDICION
        /// </summary>
        /// <param name="entity"></param>
        public int SaveMePtomedicion(MePtomedicionDTO entity)
        {
            return FactorySic.GetMePtomedicionRepository().Save(entity);
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_PTOMEDICION
        /// </summary>
        /// <param name="entity"></param>
        public void UpdatePtomedicion(MePtomedicionDTO entity)
        {
            FactorySic.GetMePtomedicionRepository().Update(entity);
        }

        /// <summary>
        /// Método que devuelve los registros de la tabla ME_PTOMEDICION segun el origen de lectura
        /// </summary>
        /// <param name="origlectcodi">Identificador de la tabla ME_ORIGENLECTURA</param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListPtomedicionByOriglectcodi(int origlectcodi)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().ListPtomedicionByOriglectcodi(origlectcodi);
        }
        /// <summary>
        /// Método que devuelve las unidades tna con el mismo nombre consultando un id
        /// </summary>
        /// <param name="idUnidad">Identificador de la unidad tna (Ptomedicodi)</param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ObtenerUnidadesPorId(int idUnidad)
        {
            return FactorySic.GetPrnMediciongrpRepository().ObtenerUnidadesPorId(idUnidad);
        }
        #endregion

        #region Métodos Tabla PRN_MEDICION48

        /// <summary>
        /// Inserta un registro de la tabla PRN_MEDICION48
        /// </summary>
        public void SavePrnMedicion48(PrnMedicion48DTO entity)
        {
            try
            {
                FactorySic.GetPrnMedicion48Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Formatea las mediciones por intervalo (H) e inserta la entidad en la tabla PRN_MEDICION48
        /// </summary>
        /// <param name="entity">Entidad de la tabla PRN_MEDICION48</param>
        /// <param name="dataMedicion">Datos de las mediciones por intervalo en formato array</param>
        public void SavePrnMedicion48(PrnMedicion48DTO entity, decimal[] dataMedicion)
        {
            int i = 0;
            while (i < dataMedicion.Length)
            {
                entity.GetType().GetProperty("H" + (i + 1).ToString()).
                    SetValue(entity, dataMedicion[i]);
                i++;
            }
            entity.Meditotal = dataMedicion.Sum();

            try
            {
                FactorySic.GetPrnMedicion48Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PRN_MEDICION48
        /// </summary>
        public void UpdatePrnMedicion48(PrnMedicion48DTO entity)
        {
            FactorySic.GetPrnMedicion48Repository().Update(entity);
        }

        /// <summary>
        /// Elimina un registro de la tabla PRN_MEDICION48
        /// </summary>
        public void DeletePrnMedicion48(int ptomedicodi, int prnm48tipo, DateTime medifecha)
        {
            FactorySic.GetPrnMedicion48Repository().Delete(ptomedicodi, prnm48tipo, medifecha);
        }

        //public void DeletePrnMedicion48SA(int ptomedicodi, int prnm48tipo)
        //{
        //    FactorySic.GetPrnMedicion48Repository().DeleteSA(ptomedicodi, prnm48tipo);
        //}

        /// <summary>
        /// Permite obtener un registro de la tabla PRN_MEDICION48
        /// </summary>
        public PrnMedicion48DTO GetByIdPrnMedicion48(int ptomedicodi, int prnm48tipo, DateTime medifecha)
        {
            return FactorySic.GetPrnMedicion48Repository().GetById(ptomedicodi, prnm48tipo, medifecha);
        }

        /// <summary>
        /// BulkInsert de la tabla PRN_MEDICION48
        /// </summary>
        /// <param name="listaMediciones"></param>
        /// <param name="usuario"></param>
        public void BulkInsertPrnMedicion48(List<PrnMedicion48DTO> listaMediciones, string usuario)
        {
            int x = listaMediciones.Count();
            List<PrnMedicion48DTO> entityBulkInsert = new List<PrnMedicion48DTO>();

            int i = 0, j = 0;
            if (x != 0)
            {
                while (i < x)
                {
                    j = 0;
                    entityBulkInsert = new List<PrnMedicion48DTO>();
                    while (j < ConstantesProdem.LimiteBulkInsert)
                    {
                        entityBulkInsert.Add(listaMediciones[i]);
                        i++; j++;
                        if (i == x) break;
                    }

                    try
                    {
                        FactorySic.GetPrnMedicion48Repository().BulkInsert(entityBulkInsert);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        throw new Exception(ex.Message, ex);
                    }
                }
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PrnMedicion48 por el IdEnvio
        /// </summary>
        /// <param name="IdEnvio">Identificador de la tabla ME_ENVIO</param>
        public List<PrnMedicion48DTO> ListPrnMedicion48ByIdEnvio(int IdEnvio)
        {
            return FactorySic.GetPrnMedicion48Repository().ListByIdEnvio(IdEnvio);
        }

        /// <summary>
        /// Devuelve una lista de registros de tipo PrnMedicion48 en un intervalo de fechas
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <param name="prnmtipo"></param>
        /// <param name="medifechaini"></param>
        /// <param name="medifechafin"></param>
        /// <returns>Lista de PrnMedicion48</returns>
        public List<PrnMedicion48DTO> ListByIdTipoFecha(int ptomedicodi, int prnmtipo, DateTime medifechaini, DateTime medifechafin)
        {
            return FactorySic.GetPrnMedicion48Repository().ListById(ptomedicodi, prnmtipo, medifechaini, medifechafin);
        }

        #endregion

        #region Métodos Tabla PRN_MEDICIONEQ

        /// <summary>
        /// Inserta un registro de la tabla PRN_MEDICIONEQ
        /// </summary>
        public void SavePrnMedicionEq(PrnMedicioneqDTO entity)
        {
            try
            {
                FactorySic.GetPrnMedicioneqRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PRN_MEDICIONEQ
        /// </summary>
        public void UpdatePrnMedicionEq(PrnMedicioneqDTO entity)
        {
            try
            {
                FactorySic.GetPrnMedicioneqRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PRN_MEDICIONEQ
        /// </summary>
        /// <param name="Prnmeqtipo">Tipo de información: [1]:DespachoEjecutado / </param>
        /// <param name="Medifecha">Formato MM/DD/YYYY</param>
        public void DeletePrnMedicionEq(int Prnmeqtipo, DateTime Medifecha)
        {
            try
            {
                FactorySic.GetPrnMedicioneqRepository().Delete(Prnmeqtipo, Medifecha);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PRN_MEDICIONEQ
        /// </summary>
        public PrnMedicioneqDTO GetByIdPrnMedicionEq(int Equicodi, int Prnmeqtipo, DateTime Medifecha)
        {
            return FactorySic.GetPrnMedicioneqRepository().GetById(Equicodi, Prnmeqtipo, Medifecha);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PRN_MEDICIONEQ
        /// </summary>
        /// <param name="fecha">Formato DD/MM/YYYY</param>
        public List<PrnMedicioneqDTO> ListPrnMedicionEqs(int Areacodi, DateTime fecha)
        {
            return FactorySic.GetPrnMedicioneqRepository().List(Areacodi, fecha);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrnMedicionEq
        /// </summary>
        public List<PrnMedicioneqDTO> GetByCriteriaPrnMedicionEqs()
        {
            return FactorySic.GetPrnMedicioneqRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla ME_MEDICION48

        /// <summary>
        /// Permite obtener un registro de la tabla ME_MEDICION48
        /// </summary>        
        public MeMedicion48DTO GetByIdMeMedicion48(int lectcodi, DateTime medifecha, int tipoinfocodi, int ptomedicodi)
        {
            return FactorySic.GetMeMedicion48Repository().GetById(lectcodi, medifecha, tipoinfocodi, ptomedicodi);
        }

        #endregion

        #region Métodos Tabla PRN_CONFIGURACION

        /// <summary>
        /// Inserta un registro de la tabla PRN_CONFIGURACION
        /// </summary>
        public void SavePrnConfiguracion(PrnConfiguracionDTO entity)
        {
            FactorySic.GetPrnConfiguracionRepository().Save(entity);
        }

        /// <summary>
        /// Actualiza un registro de la tabla PRN_CONFIGURACION
        /// </summary>
        public void UpdatePrnConfiguracion(PrnConfiguracionDTO entity)
        {
            FactorySic.GetPrnConfiguracionRepository().Update(entity);
        }

        /// <summary>
        /// Elimina un registro de la tabla PRN_CONFIGURACION
        /// </summary>
        public void DeletePrnConfiguracion(int pmedatcodi, DateTime prncfgfecha)
        {
            FactorySic.GetPrnConfiguracionRepository().Delete(pmedatcodi, prncfgfecha);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PRN_CONFIGURACION
        /// </summary>
        public PrnConfiguracionDTO GetByIdPrnConfiguracion(int ptomedicodi, DateTime prncfgfecha)
        {
            return FactorySic.GetPrnConfiguracionRepository().GetById(ptomedicodi, prncfgfecha);
        }

        /// <summary>
        /// Permite realizar el proceso de BulkInsert de registro en la tabla PRN_CONFIGURACION
        /// </summary>
        public void BulkInsertPrnConfiguracion(List<PrnConfiguracionDTO> entitys)
        {
            FactorySic.GetPrnConfiguracionRepository().BulkInsert(entitys);
        }
        #endregion

        #region Método Tabla PRN_AREAMEDICION

        /// <summary>
        /// Inserta un registro de la tabla PRN_AREAMEDICION
        /// </summary>
        public void SavePrnAreamedicion(PrnAreamedicionDTO entity)
        {
            try
            {
                FactorySic.GetPrnAreamedicionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PRN_AREAMEDICION
        /// </summary>
        public void UpdatePrnAreamedicion(PrnAreamedicionDTO entity)
        {
            FactorySic.GetPrnAreamedicionRepository().Update(entity);
        }

        /// <summary>
        /// Elimina un registro de la tabla PRN_AREAMEDICION
        /// </summary>
        public void DeletePrnAreamedicion(int aremedcodi)
        {
            FactorySic.GetPrnAreamedicionRepository().Delete(aremedcodi);
        }

        /// <summary>
        /// Permite actualizar el estado de un registro de la tabla PRN_AREAMEDICION
        /// </summary>
        public void UpdateEstadoPrnAreamedicion(PrnAreamedicionDTO entity)
        {
            FactorySic.GetPrnAreamedicionRepository().UpdateEstado(entity);
        }

        /// <summary>
        /// Método que lista las ciudades utilizadas para el modulo de Variables Exógenas
        /// </summary>
        public List<PrnAreamedicionDTO> ListVarexoCiudad()
        {
            return FactorySic.GetPrnAreamedicionRepository().ListVarexoCiudad();
        }

        #endregion

        #region Métodos Tabla PRN_EXOGENAMEDICION

        /// <summary>
        /// Permite insertar un registro en la taba PRN_EXOGENAMEDICION
        /// </summary>
        public void SavePrnExogenamedicion(PrnExogenamedicionDTO entity)
        {
            FactorySic.GetPrnExogenamedicionRepository().Save(entity);
        }

        /// <summary>
        /// Permite actualizar un registro de la tabla PRN_EXOGENAMEDICION
        /// </summary>
        public void UpdatePrnExogenamedicion(PrnExogenamedicionDTO entity)
        {
            FactorySic.GetPrnExogenamedicionRepository().Update(entity);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PRN_EXOGENAMEDICION
        /// </summary>
        public PrnExogenamedicionDTO GetByIdPrnExogenamedicion(int varexocodi, int aremedcodi, DateTime exmedifecha)
        {
            return FactorySic.GetPrnExogenamedicionRepository().GetById(varexocodi, aremedcodi, exmedifecha);
        }

        /// <summary>
        /// Método que lista las mediciones correspondientes a las Variables Exógenas por ciudad y fecha
        /// </summary>
        public List<PrnExogenamedicionDTO> ListExomedicionByCiudadDate(int areamedcodi, DateTime exmedifecha)
        {
            return FactorySic.GetPrnExogenamedicionRepository().ListExomedicionByCiudadDate(areamedcodi, exmedifecha);
        }

        /// <summary>
        /// Método que lista las horas de sol registradas para la ciudad LIMA de la tabla PRN_HORASOL
        /// </summary>
        public List<PrnHorasolDTO> ListHorasol()
        {
            return FactorySic.GetPrnExogenamedicionRepository().ListHorasol();
        }


        #endregion

        #region Métodos Tabla EVE_SUBCAUSAEVENTO

        //Servicio para GRabar en la tabla EveSubcausaevento para Pronostico
        public void SaveMotivo(EveSubcausaeventoDTO entidad)
        {

            try
            {
                FactorySic.GetEveSubcausaeventoRepository().Save(entidad);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }

        //Servicio para GRabar en la tabla EveSubcausaevento para Pronostico
        public void UpdateMotivo(EveSubcausaeventoDTO entidad)
        {

            try
            {
                FactorySic.GetEveSubcausaeventoRepository().UpdateBy(entidad);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }

        #endregion

        #region Método Tabla PRN_CLASIFICACION

        /// <summary>
        /// Inserta un registro de la tabla PRN_CLASIFICACION
        /// </summary>
        public void SavePrnClasificacion(PrnClasificacionDTO entity)
        {
            try
            {
                FactorySic.GetPrnClasificacionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PRN_CLASIFICACION
        /// </summary>
        public void UpdatePrnClasificacion(PrnClasificacionDTO entity)
        {
            FactorySic.GetPrnClasificacionRepository().Update(entity);
        }

        /// <summary>
        /// Elimina un registro de la tabla PRN_CLASIFICACION
        /// </summary>
        public void DeletePrnClasificacion(int ptomedicodi, int lectcodi, DateTime prnclsfecha)
        {
            FactorySic.GetPrnClasificacionRepository().Delete(ptomedicodi, lectcodi, prnclsfecha);
        }

        #endregion

        #region Método Tabla PRN_AGRUPACION

        /// <summary>
        /// Inserta un registro de la tabla PRN_AGRUPACION
        /// </summary>
        public void SavePrnAgrupacion(PrnAgrupacionDTO entity)
        {
            try
            {
                FactorySic.GetPrnAgrupacionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar los registro de la tabla PRN_AGRUPACION
        /// </summary>
        public List<PrnAgrupacionDTO> ListPrnAgrupacion()
        {
            return FactorySic.GetPrnAgrupacionRepository().List();
        }

        /// <summary>
        /// Permite listar los registro de la tabla PRN_AGRUPACION por Id
        /// </summary>
        public List<PrnAgrupacionDTO> ListByIdPrnAgrupacion(int ptogrpcodi)
        {
            return FactorySic.GetPrnAgrupacionRepository().ListById(ptogrpcodi);
        }

        /// <summary>
        /// Método que lista todas las agrupaciones activas registradas en la tabla ME_PTOMEDICION
        /// </summary>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListMeAgrupacion()
        {
            return FactorySic.GetPrnAgrupacionRepository().ListMeAgrupacion();
        }

        /// <summary>
        /// Método que lista los puntos de todas las agrupaciones que participan en el Pronostico de la Demanda
        /// </summary>
        /// <returns></returns>
        public List<PrnAgrupacionDTO> ListPtosAgrupadosParaProdem()
        {
            return FactorySic.GetPrnAgrupacionRepository().ListPtosAgrupadosParaProdem();
        }

        /// <summary>
        /// Permite validar la descripción de una agrupación
        /// </summary>
        /// <param name="ptomedidesc"></param>
        /// <returns></returns>
        public int ValidarNombreAgrupacion(string ptomedidesc)
        {
            return FactorySic.GetPrnAgrupacionRepository().ValidarNombreAgrupacion(ptomedidesc);
        }

        #endregion

        #region Método Tabla PRN_PUNTOAGRUPACION

        /// <summary>
        /// Inserta un registro de la tabla PRN_PUNTOAGRUPACION
        /// </summary>
        public int SavePuntoAgrupacion(PrnPuntoAgrupacionDTO entity)
        {
            try
            {
                return FactorySic.GetPrnAgrupacionRepository().SavePuntoAgrupacion(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Lista los registros correspondientes al punto de medición parametro de la tabla PRN_PUNTOAGRUPACION
        /// </summary>
        public List<PrnPuntoAgrupacionDTO> ListByIdPuntoAgrupacion(int ptomedicodi)
        {
            return FactorySic.GetPrnAgrupacionRepository().ListByIdPuntoAgrupacion(ptomedicodi);
        }

        /// <summary>
        /// Actualiza la fecha de fin por la del dia en el que se realiza la actualización
        /// </summary>
        public void CerrarPuntoAgrupacion(int ptogrpcodi, DateTime ptogrpfechafin)
        {
            FactorySic.GetPrnAgrupacionRepository().CerrarPuntoAgrupacion(ptogrpcodi, ptogrpfechafin);
        }

        #endregion

        #region Metodos Tabla EQ_AREANIVEL
        public List<EqAreaNivelDTO> ListEqAreanivel()
        {
            return FactorySic.GetEqAreanivelRepository().List();
        }
        #endregion

        #region Metodos Tabla PRN_VERSION
        //GetListVersion
        public List<PrnVersionDTO> GetListVersion()
        {

            return FactorySic.GetPrnVersionRepository().List();
        }

        public PrnVersionDTO GetListVersionById(int codigo)
        {

            return FactorySic.GetPrnVersionRepository().GetById(codigo);
        }

        public void SavePrnVersion(PrnVersionDTO entity)
        {
            try
            {
                FactorySic.GetPrnVersionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void UpdatePrnVersion(PrnVersionDTO entity)
        {
            try
            {
                FactorySic.GetPrnVersionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void UpdatePrnVersionInactivo(string estado)
        {
            try
            {
                FactorySic.GetPrnVersionRepository().UpdateAllVersionInactivo(estado);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region Métodos Tabla PRN_CONFIGBARRA

        /// <summary>
        /// Inserta un registro de la tabla PRN_CONFIGBARRA
        /// </summary>
        public void SavePrnConfigbarra(PrnConfigbarraDTO entity)
        {
            try
            {
                FactorySic.GetPrnConfigbarraRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PRN_CONFIGBARRA
        /// </summary>
        public void UpdatePrnConfigbarra(PrnConfigbarraDTO entity)
        {
            FactorySic.GetPrnConfigbarraRepository().Update(entity);
        }

        /// <summary>
        /// Elimina un registro de la tabla PRN_CONFIGBARRA
        /// </summary>
        public void DeletePrnConfigbarra(int grupocodi, DateTime cfgbarfecha)
        {
            FactorySic.GetPrnConfigbarraRepository().Delete(grupocodi, cfgbarfecha);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PRN_CONFIGBARRA
        /// </summary>
        public PrnConfigbarraDTO GetByIdPrnConfigbarra(int grupocodi, DateTime cfgbarfecha)
        {
            return FactorySic.GetPrnConfigbarraRepository().GetById(grupocodi, cfgbarfecha);
        }

        #endregion

        #region Métodos Tabla PR_GRUPO

        /// <summary>
        /// Lista los registros de la tabla PR_GRUPO correspondientes a las Barras
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoDTO> ListPrGrupoBarra()
        {
            return FactorySic.GetPrGrupoRepository().
                List().Where(x => x.Catecodi == 10).
                ToList();
        }

        #endregion

        #region Métodos Tabla PRN_MEDICIONGRP

        /// <summary>
        /// Elimina registro de la tabla mediante su grupocodi y tipo
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <param name="prnm48tipo"></param>
        public void DeletePrnMediciongrpSA(int ptomedicodi, int prnm48tipo)
        {
            FactorySic.GetPrnMediciongrpRepository().DeleteSA(ptomedicodi, prnm48tipo);
        }

        /// <summary>
        /// Inserta un registro de la tabla PRN_MEDICIONGRP
        /// </summary>
        public void SavePrnMediciongrp(PrnMediciongrpDTO entity)
        {
            //Valida la versión a registrar
            entity.Vergrpcodi = (entity.Vergrpcodi != 0)
                ? entity.Vergrpcodi
                : ConstantesProdem.VergrpcodiBase;
            try
            {
                FactorySic.GetPrnMediciongrpRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Formatea las mediciones por intervalo (H) e inserta la entidad en la tabla PRN_MEDICIONGRP
        /// </summary>
        /// <param name="entity">Entidad de la tabla PRN_MEDICIONGRP</param>
        /// <param name="dataMedicion">Datos de las mediciones por intervalo en formato array</param>
        public void SavePrnMediciongrp(PrnMediciongrpDTO entity, decimal[] dataMedicion)
        {
            //Valida la versión a registrar
            entity.Vergrpcodi = (entity.Vergrpcodi != 0)
                ? entity.Vergrpcodi
                : ConstantesProdem.VergrpcodiBase;

            int i = 0;
            while (i < dataMedicion.Length)
            {
                entity.GetType().GetProperty("H" + (i + 1).ToString()).
                    SetValue(entity, dataMedicion[i]);
                i++;
            }
            entity.Meditotal = dataMedicion.Sum();

            try
            {
                FactorySic.GetPrnMediciongrpRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PRN_MEDICIONGRP
        /// </summary>
        public void UpdatePrnMediciongrp(PrnMediciongrpDTO entity)
        {
            //Valida la versión a registrar
            entity.Vergrpcodi = (entity.Vergrpcodi != 0)
                ? entity.Vergrpcodi
                : ConstantesProdem.VergrpcodiBase;
            FactorySic.GetPrnMediciongrpRepository().Update(entity);
        }

        /// <summary>
        /// Elimina un registro de la tabla PRN_MEDICIONGRP
        /// </summary>
        public void DeletePrnMediciongrp(int grupocodi, int prnmgrptipo, DateTime medifecha, int vergrpcodi = 1)
        {

            FactorySic.GetPrnMediciongrpRepository().Delete(grupocodi, prnmgrptipo, medifecha, vergrpcodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PRN_MEDICIONGRP
        /// </summary>
        public PrnMediciongrpDTO GetByIdPrnMediciongrp(int grupocodi, int prnmgrptipo, DateTime medifecha, int vergrpcodi = 1)
        {
            return FactorySic.GetPrnMediciongrpRepository().GetById(grupocodi, prnmgrptipo, medifecha, vergrpcodi);
        }

        #endregion

        #region Metodo para la Bitacora
        /// <summary>
        /// Método que devuelve la cantidad total de registros de la consulta de bitácora
        /// </summary>
        /// <param name="idLectura">Identificador del tipo de lectura</param>
        /// <param name="idTipoEmpresa">Identificador del tipo de empresa</param>
        /// <param name="fechaIni">Fecha de inicio de la consulta</param>
        /// <param name="fechaFin">Fecha de termino de la consulta</param>
        /// <returns></returns>
        public int TotalRegConsultaBitacora(string idLectura, 
            string idTipoEmpresa,
            string fechaIni,
            string fechaFin)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository()
                .TotalRegConsultaBitacora(idLectura,
                idTipoEmpresa, fechaIni,
                fechaFin);
        }

        /// <summary>
        /// Lista la bitacora
        /// </summary>
        /// <param name="fechaIni">Fecha inicio de la consulta</param>
        /// <param name="fechaFin">Fecha final de la consulta</param>
        /// <param name="lectcodi">Identificador del tipo de lectura</param>
        /// <param name="tipoempresa">Identificador del tipo de empresa</param>
        /// <param name="regIni">Registro inicio del paginado</param>
        /// <param name="regFin">Registro final del paginado</param>
        /// <returns></returns>
        public List<MeJustificacionDTO> ListaBitacora(string fechaIni, 
            string fechaFin, string lectcodi,
            string tipoempresa, int regIni,
            int regFin)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository()
                .ListaBitacora(fechaIni, fechaFin,
                lectcodi, tipoempresa, regIni,
                regFin);
        }

        /// <summary>
        /// Metodo general para dar forma a la bitacora
        /// </summary>
        /// <param name="fechaIni">Fecha inicio del rango de consulta</param>
        /// <param name="fechaFin">Fecha final del rango de consulta</param>
        /// <param name="lect">Identificador del tipo de lectura</param>
        /// <param name="empr">Identificador de la empresa</param>
        /// <param name="regIni">Registro de inicio del paginado</param>
        /// <param name="regByPag">Total de registros para el paginado</param>
        /// <returns></returns>
        public object ListBitacora(string fechaIni, 
            string fechaFin, List<int> lect,
            List<int> empr, int regIni, 
            int regByPag)
        {
            PrnConfiguracionDTO regconfiguracion = new PrnConfiguracionDTO();
            PrnPatronModel regPatron = new PrnPatronModel();

            string lectcodi = (lect.Count != 0)
                ? string.Join(",", lect)
                : string.Join(",", new List<int>
                {
                    ConstantesProdem.LectcodiDemEjecDiario,
                    ConstantesProdem.LectcodiDemPrevDiario,
                    ConstantesProdem.LectcodiDemPrevSemanal,
                });
            string tipoempr = (empr.Count != 0) 
                ? string.Join(",", empr)
                : string.Join(",", new List<int>
                {
                    ConstantesProdem.TipoemprcodiDistribuidores,
                    ConstantesProdem.TipoemprcodiUsuLibres,
                });

            int totalRegistros = this.TotalRegConsultaBitacora(lectcodi,
                tipoempr, fechaIni,
                fechaFin);
            List<MeJustificacionDTO> datosBitacora = this.ListaBitacora(fechaIni,
                fechaFin, lectcodi, 
                tipoempr, regIni, 
                (regIni + regByPag));
                        
            List<object> entities = new List<object>();
            int newId = 0, contador = 0;
            foreach (var item in datosBitacora)
            {
                int id = item.Ptomedicodi.Value;

                if (contador == 0 || (newId != id))
                {
                    regconfiguracion = this.ParametrosGetConfiguracion(id,
                        ConstantesProdem.DefectoByPunto, 
                        item.Medifecha.Value);

                    regPatron = this.GetPatron(id, 
                        ConstantesProdem.ProcPatronDemandaEjecutada,
                        item.Medifecha.Value, 
                        regconfiguracion);
                }
                newId = item.Ptomedicodi.Value;
                contador++;

                decimal valorPatron = this.ObtenerIntervaloPatron(regPatron.Patron,
                    item.Horainicio);

                string parseFechaFin = (item.Horafin == "00:00")
                    ? item.Medifecha.Value
                    .AddDays(1)
                    .ToString(ConstantesProdem.FormatoFecha)
                    : item.Medifecha.Value
                    .ToString(ConstantesProdem.FormatoFecha);

                object entity;
                entity = new
                {                    
                    area = item.Area,
                    empresa = item.Emprnomb,
                    fechaInicio = item.Medifecha.Value.ToString(ConstantesProdem.FormatoFecha),
                    horaInicio = item.Horainicio,
                    fechaFin = parseFechaFin,
                    horaFin = item.Horafin,
                    consumoTipico = valorPatron,
                    consumoPrevisto = item.Consumoprevisto,
                    potenciaDiferencial = valorPatron - item.Consumoprevisto,
                    barra = item.Gruponomb,
                    punto = item.Ptomedidesc,
                    ocurrencia = item.Subcausadesc,
                };
                entities.Add(entity);
            }

            return new { 
                data = entities, 
                recordsTotal = totalRegistros, 
                recordsFiltered = totalRegistros,
            };
        }

        /// <summary>
        /// Devuelve el valor de un intervalo mediante la hora
        /// </summary>
        /// <param name="patron"></param>
        /// <param name="horaInicio"></param>
        /// <returns></returns>
        public decimal ObtenerIntervaloPatron(decimal[] patron, string horaInicio)
        {

            decimal intervalo = 0;

            switch (horaInicio)
            {

                case "00:30:00":
                    {
                        intervalo = patron[0];
                    }
                    break;
                case "01:00:00":
                    {
                        intervalo = patron[1];
                    }
                    break;
                case "01:30:00":
                    {
                        intervalo = patron[2];
                    }
                    break;
                case "02:00:00":
                    {
                        intervalo = patron[3];
                    }
                    break;
                case "02:30:00":
                    {
                        intervalo = patron[4];
                    }
                    break;
                case "03:00:00":
                    {
                        intervalo = patron[5];
                    }
                    break;
                case "03:30:00":
                    {
                        intervalo = patron[6];
                    }
                    break;
                case "04:00:00":
                    {
                        intervalo = patron[7];
                    }
                    break;
                case "04:30:00":
                    {
                        intervalo = patron[8];
                    }
                    break;
                case "05:00:00":
                    {
                        intervalo = patron[9];
                    }
                    break;
                case "05:30:00":
                    {
                        intervalo = patron[10];
                    }
                    break;
                case "06:00:00":
                    {
                        intervalo = patron[11];
                    }
                    break;
                case "06:30:00":
                    {
                        intervalo = patron[12];
                    }
                    break;
                case "07:00:00":
                    {
                        intervalo = patron[13];
                    }
                    break;
                case "07:30:00":
                    {
                        intervalo = patron[14];
                    }
                    break;
                case "08:00:00":
                    {
                        intervalo = patron[15];
                    }
                    break;
                case "08:30:00":
                    {
                        intervalo = patron[16];
                    }
                    break;
                case "09:00:00":
                    {
                        intervalo = patron[17];
                    }
                    break;
                case "09:30:00":
                    {
                        intervalo = patron[18];
                    }
                    break;
                case "10:00:00":
                    {
                        intervalo = patron[19];
                    }
                    break;
                case "10:30:00":
                    {
                        intervalo = patron[20];
                    }
                    break;
                case "11:00:00":
                    {
                        intervalo = patron[21];
                    }
                    break;
                case "11:30:00":
                    {
                        intervalo = patron[22];
                    }
                    break;
                case "12:00:00":
                    {
                        intervalo = patron[23];
                    }
                    break;
                case "12:30:00":
                    {
                        intervalo = patron[24];
                    }
                    break;
                case "13:00:00":
                    {
                        intervalo = patron[25];
                    }
                    break;
                case "13:30:00":
                    {
                        intervalo = patron[26];
                    }
                    break;
                case "14:00:00":
                    {
                        intervalo = patron[27];
                    }
                    break;
                case "14:30:00":
                    {
                        intervalo = patron[28];
                    }
                    break;
                case "15:00:00":
                    {
                        intervalo = patron[29];
                    }
                    break;
                case "15:30:00":
                    {
                        intervalo = patron[30];
                    }
                    break;
                case "16:00:00":
                    {
                        intervalo = patron[31];
                    }
                    break;
                case "16:30:00":
                    {
                        intervalo = patron[32];
                    }
                    break;
                case "17:00:00":
                    {
                        intervalo = patron[33];
                    }
                    break;
                case "17:30:00":
                    {
                        intervalo = patron[34];
                    }
                    break;
                case "18:00:00":
                    {
                        intervalo = patron[35];
                    }
                    break;
                case "18:30:00":
                    {
                        intervalo = patron[37];
                    }
                    break;
                case "19:00:00":
                    {
                        intervalo = patron[38];
                    }
                    break;
                case "19:30:00":
                    {
                        intervalo = patron[39];
                    }
                    break;
                case "20:00:00":
                    {
                        intervalo = patron[40];
                    }
                    break;
                case "20:30:00":
                    {
                        intervalo = patron[41];
                    }
                    break;
                case "21:00:00":
                    {
                        intervalo = patron[42];
                    }
                    break;
                case "21:30:00":
                    {
                        intervalo = patron[43];
                    }
                    break;
                case "22:00:00":
                    {
                        intervalo = patron[44];
                    }
                    break;
                case "22:30:00":
                    {
                        intervalo = patron[45];
                    }
                    break;
                case "23:00:00":
                    {
                        intervalo = patron[46];
                    }
                    break;
                case "23:30:00":
                    {
                        intervalo = patron[47];
                    }
                    break;
            }
            return intervalo;
        }


        #endregion

        #region Método Tabla PRN_PTOMEDPROP

        /// <summary>
        /// Inserta un registro de la tabla PRN_PTOMEDPROP
        /// </summary>
        public void SavePrnPtomedprop(PrnPtomedpropDTO entity)
        {
            try
            {
                FactorySic.GetPrnPtomedpropRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PRN_PTOMEDPROP
        /// </summary>
        public void UpdatePrnPtomedprop(PrnPtomedpropDTO entity)
        {
            FactorySic.GetPrnPtomedpropRepository().Update(entity);
        }

        /// <summary>
        /// Elimina un registro de la tabla PRN_PTOMEDPROP
        /// </summary>
        public void DeletePrnPtomedprop(int ptomedicodi)
        {
            FactorySic.GetPrnPtomedpropRepository().Delete(ptomedicodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PRN_PTOMEDPROP
        /// </summary>
        public PrnPtomedpropDTO GetByIdPrnPtomedprop(int ptomedicodi)
        {
            return FactorySic.GetPrnPtomedpropRepository().GetById(ptomedicodi);
        }

        #endregion

        #region Métodos Tabla PRN_PTOESTIMADOR        
        /// <summary>
        /// Inserta un registro de la tabla ME_PTOMEDICION
        /// </summary>
        /// <param name="entity"></param>
        public void SavePrnPtoEstimador(PrnPtoEstimadorDTO entity)
        {
            FactorySic.GetPrnPtoEstimadorRepository().Save(entity);
        }

        #endregion

        #region Métodos Tabla PRN_MEDICIONESRAW

        /// <summary>
        /// Método que devuelve las mediciones de una medición raw segun parametros(llave)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PrnMedicionesRawDTO GetByKeyPrnMedicionesRaw(PrnMedicionesRawDTO entity)
        {
            return FactorySic.GetPrnMedicionesRawRepository().GetByKey(entity);
        }
        /// <summary>
        /// Inserta un registro en la tabla PRN_MEDICIONESRAW
        /// </summary>
        /// <param name="entity"></param>
        public void SavePrnMedicionesRaw(PrnMedicionesRawDTO entity)
        {
            FactorySic.GetPrnMedicionesRawRepository().Save(entity);
        }
        /// <summary>
        /// Actualiza un registro en la tabla PRN_MEDICIONESRAW
        /// </summary>
        /// <param name="entity"></param>
        public void UpdatePrnMedicionesRaw(PrnMedicionesRawDTO entity)
        {
            FactorySic.GetPrnMedicionesRawRepository().Update(entity);
        }

        #endregion

        #region Métodos Tabla PRN_RELACIONTNA
        /// <summary>
        /// Método que devuelve la demanda de una unidad TNA
        /// </summary>
        /// <param name="unidades">Identificador de una o varias unidades tna</param>
        /// <param name="fecInicio">Fecha de inicio para la consulta</param>
        /// <param name="fecFin">Fecha fin para la consulta</param>
        /// <param name="variables">Identificador de una o varias variables</param>
        /// <returns></returns>
        public List<PrnEstimadorRawDTO> ListDemandaTnaPorUnidad(string unidades, DateTime fecInicio, DateTime fecFin, string variables)
        {
            string strFecInicio = fecInicio.ToString(ConstantesProdem.FormatoFecha);
            string strFecFin = fecFin.ToString(ConstantesProdem.FormatoFecha);
            return FactorySic.GetPrnEstimadorRawRepository()
                .ListDemandaTnaPorUnidad(unidades, strFecInicio, strFecFin, variables);
        }

        /// <summary>
        /// Método que inserta un registro en la tabla PRN_RELACIONTNA
        /// </summary>
        /// <param name="entity">Entidad de la tabla PRN_RELACIONTNA</param>
        public void SavePrnRelacionTna(PrnRelacionTnaDTO entity)
        {
            FactorySic.GetPrnRelacionTnaRepository().Save(entity);
        }

        /// <summary>
        /// Método que elimina un registro de la tabla PRN_RELACIONTNA
        /// </summary>
        /// <param name="id">Identificador de un registro de la tabla PRN_RELACIONTNA</param>
        public void DeletePrnRelacionTna(int id)
        {
            FactorySic.GetPrnRelacionTnaRepository().Delete(id);
        }

        /// <summary>
        /// Método que actualiza un registro de la tabla PRN_RELACIONTNA
        /// </summary>
        /// <param name="entity">Entidad de la tabla PRN_RELACIONTNA</param>
        public void UpdatePrnRelacionTna(PrnRelacionTnaDTO entity)
        {
            FactorySic.GetPrnRelacionTnaRepository().Update(entity);
        }

        /// <summary>
        /// Lista los registros de la tabla PRN_RELACIONTNA
        /// </summary>
        /// <returns></returns>
        public List<PrnRelacionTnaDTO> ListPrnRelacionTna()
        {
            return FactorySic.GetPrnRelacionTnaRepository().List();
        }
        /// <summary>
        /// Método que inserta un registro en la tabla PRN_RELACIONTNA y devuelve el id
        /// </summary>
        /// <param name="entity">Entidad de la tabla PRN_RELACIONTNA</param>
        public int SaveGetIdPrnRelacionTna(PrnRelacionTnaDTO entity)
        {
            return FactorySic.GetPrnRelacionTnaRepository().SaveGetId(entity);
        }

        #endregion

        #region Métodos Tabla PRN_RELACIONTNADETALLE
        /// <summary>
        /// Inserta un registro en la tabla PRN_RELACIONTNADETALLE
        /// </summary>
        /// <param name="entity">Entidad de la tabla PRN_RELACIONTNADETALLE</param>
        /// <returns></returns>
        public void SavePrnRelacionTnaDetalle(PrnRelacionTnaDTO entity)
        {
            FactorySic.GetPrnRelacionTnaRepository().SavePrnRelacionTnaDetalle(entity);
        }
        /// <summary>
        /// Elimina los registros de la tabla PRN_RELACIONTNADETALLE 
        /// que esten relacionados a un registro de la tabla PRN_RELACIONTNA
        /// </summary>
        /// <param name="idRegistro">Identificador de la tabla PRN_RELACIONTNA</param>
        public void DeletePrnRelacionTnaDetalle(int idRegistro)
        {
            FactorySic.GetPrnRelacionTnaRepository().DeletePrnRelacionTnaDetalle(idRegistro);
        }
        /// <summary>
        /// Lista los registros de la tabla PRN_RELACIONTNADETALLE
        /// </summary>
        /// <returns></returns>
        public List<PrnRelacionTnaDTO> ListPrnRelacionTnaDetalle()
        {
            return FactorySic.GetPrnRelacionTnaRepository().ListRelacionTnaDetalle();
        }
        #endregion

        #region Métodos Tabla PRN_ESTIMADORRAW
        /// <summary>
        /// Método que lista los registros de la tabla PRN_ESTIMADORRAW
        /// </summary>
        /// <returns></returns>
        public List<PrnEstimadorRawDTO> ListPrnEstimadorRaw()
        {
            return FactorySic.GetPrnEstimadorRawRepository().List();
        }

        /// <summary>
        /// Método que formatea una lista de registros de la tabla PRN_ESTIMADORRAW
        /// a una lista de entidades ScadaDTO
        /// </summary>
        /// <param name="filtro"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="fuente"></param>
        /// <returns></returns>
        public List<ScadaDTO> ObtenerConsultaDemandaTna(string filtro, DateTime fechaInicio, DateTime fechaFin, string fuente)
        {
            ScadaDTO entidad;
            List<ScadaDTO> entidades = new List<ScadaDTO>();
            fechaFin = fechaFin.AddDays(1);
            List<PrnEstimadorRawDTO> dataMediciones = this.ListDemandaTnaPorUnidad(filtro,
                fechaInicio, fechaFin, ConstantesProdem.ConsPotActivaTna);

            List<string> fechas = dataMediciones
                .GroupBy(x => x.Etmrawfecha.ToString(ConstantesProdem.FormatoFecha))
                .Select(x => x.First().Etmrawfecha.ToString(ConstantesProdem.FormatoFecha))
                .ToList();
            List<int> puntos = dataMediciones
                .GroupBy(x => x.Ptomedicodi)
                .Select(x => x.First().Ptomedicodi)
                .ToList();

            foreach (int p in puntos)
            {
                List<PrnEstimadorRawDTO> dataRaw = dataMediciones
                    .Where(x => x.Ptomedicodi == p)
                    .ToList();
                foreach (string f in fechas)
                {
                    entidad = new ScadaDTO
                    {
                        CANALCODI = -1,
                        PTOMEDICODI = p,
                        MEDIFECHA = DateTime.ParseExact(f, ConstantesProdem.FormatoFecha,
                            CultureInfo.InvariantCulture),
                        FUENTE = fuente
                    };
                    List<PrnEstimadorRawDTO> dataRawPorDia = dataRaw
                        .Where(x => x.Etmrawfecha.ToString(ConstantesProdem.FormatoFecha) == f)
                        .OrderBy(x => x.Etmrawfecha)
                        .ToList();
                    foreach (PrnEstimadorRawDTO r in dataRawPorDia)
                    {
                        string intervalo = r.Etmrawfecha.ToString(ConstantesProdem.FormatoHoraMinuto);
                        string attrH = UtilProdem.ObtenerIntervalo(intervalo, ConstantesProdem.Itv15min);
                        int i = entidad.GetType()
                            .GetProperties()
                            .Count(x => x.Name.Equals(attrH));
                        if (i > 0) entidad.GetType()
                                .GetProperty(attrH)
                                .SetValue(entidad, r.Etmrawvalor);
                    }
                    entidades.Add(entidad);
                }
            }

            return entidades;
        }

        /// <summary>
        /// BulkInsert de las tablas tablas temporales para información raw
        /// </summary>
        /// <param name="listaMediciones">Lista de entidades a registrar</param>
        /// <param name="idFuente">Identificador de la funete de información [ieod, sco]</param>
        public void BulkInsertPrnEstimadorRaw(List<PrnEstimadorRawDTO> listaMediciones, int idFuente)
        {
            string nombreTabla = string.Empty;
            if (idFuente == ConstantesProdem.EtmrawfntIeod)
                nombreTabla = ConstantesProdem.tablaCargaIeod;
            if (idFuente == ConstantesProdem.EtmrawfntSco)
                nombreTabla = ConstantesProdem.tablaCargaSco;

            int x = listaMediciones.Count();
            List<PrnEstimadorRawDTO> entityBulkInsert = new List<PrnEstimadorRawDTO>();

            int i = 0, j = 0;
            if (x != 0)
            {
                while (i < x)
                {
                    j = 0;
                    entityBulkInsert = new List<PrnEstimadorRawDTO>();
                    while (j < ConstantesProdem.LimiteBulkInsert)
                    {
                        entityBulkInsert.Add(listaMediciones[i]);
                        i++; j++;
                        if (i == x) break;
                    }
                    try
                    {
                        FactorySic.GetPrnEstimadorRawRepository().BulkInsert(entityBulkInsert, nombreTabla);
                    }
                    catch (Exception ex)
                    {
                        this.TruncateTemporalPrnEstimadorRaw(idFuente);

                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        throw new Exception(ex.Message, ex);
                    }
                }
            }
        }

        /// <summary>
        /// Método que elimina los registros de la tabla PRN_ESTIMADORRAW
        /// que correspondan a cierta fecha, intervalo y fuente
        /// </summary>
        /// <param name="fuente">origen de la carga de información (ieod, sco)</param>
        /// <param name="fecha">fecha de los registros(dd/MM/yyyy)</param>
        public void DeletePrnEstimadorRawPorFechaIntervalo(int fuente, string fecha)
        {
            FactorySic.GetPrnEstimadorRawRepository().DeletePorFechaIntervalo(fuente, fecha);
        }

        /// <summary>
        /// Método que devuelve el máximo identificador(PK) de la tabla PRN_ESTIMADORRAW
        /// </summary>
        /// <returns></returns>
        public int GetMaxIdPrnEstimadorRaw()
        {
            return FactorySic.GetPrnEstimadorRawRepository().GetMaxId();
        }
        /// <summary>
        /// Método que devuelve el máximo identificador(PK) de la tabla PRN_ESTIMADORRAW
        /// </summary>
        /// <returns></returns>
        public int GetMaxIdScoPrnEstimadorRaw()
        {
            return FactorySic.GetPrnEstimadorRawRepository().GetMaxIdSco();
        }

        /// <summary>
        /// Método que traslada la información desde las tablas temporales a la tabla PRN_ESTIMADORRAW
        /// </summary>
        /// <param name="idFuente">Identificador de la fuente de información</param>
        /// <param name="fecha">Fecha y hora del registro [Formato: dd/MM/yyyy HH:mm]</param>
        public void InsertTableIntoPrnEstimadorRaw(int idFuente, string fecha)
        {
            string nombreTabla = string.Empty;
            if (idFuente == ConstantesProdem.EtmrawfntIeod)
                nombreTabla = ConstantesProdem.tablaCargaIeod;
            if (idFuente == ConstantesProdem.EtmrawfntSco)
                nombreTabla = ConstantesProdem.tablaCargaSco;

            if (!string.IsNullOrEmpty(nombreTabla))
            {
                try
                {
                    FactorySic.GetPrnEstimadorRawRepository()
                    .InsertTableIntoPrnEstimadorRaw(nombreTabla, fecha);
                }
                catch (Exception ex)
                {
                    this.TruncateTemporalPrnEstimadorRaw(idFuente);

                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw new Exception(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Método que limpia la tabla temporal utilizada para la carga de archivos raw
        /// </summary>
        /// <param name="idFuente">Identificador de la fuente de información</param>
        public void TruncateTemporalPrnEstimadorRaw(int idFuente)
        {
            string nombreTabla = string.Empty;
            if (idFuente == ConstantesProdem.EtmrawfntIeod)
                nombreTabla = ConstantesProdem.tablaCargaIeod;
            if (idFuente == ConstantesProdem.EtmrawfntSco)
                nombreTabla = ConstantesProdem.tablaCargaSco;

            if (!string.IsNullOrEmpty(nombreTabla))
                FactorySic.GetPrnEstimadorRawRepository().TruncateTablaTemporal(nombreTabla);
        }
        #endregion

        #region Métodos Tabla PRN_VERSIONGRP
        /// <summary>
        /// Inserta un registro de la tabla PRN_VERSIONGRP
        /// </summary>
        public void SavePrnVersiongrp(PrnVersiongrpDTO entity)
        {
            try
            {
                FactorySic.GetPrnVersiongrpReporsitory().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Inserta un registro de la tabla PRN_VERSIONGRP y devuelve el Id
        /// </summary>
        public int SaveGetIdPrnVersiongrp(PrnVersiongrpDTO entity)
        {
            try
            {
                return FactorySic.GetPrnVersiongrpReporsitory().SaveGetId(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Actualiza un registro de la tabla PRN_VERSIONGRP
        /// </summary>
        public void UpdatePrnVersiongrp(PrnVersiongrpDTO entity)
        {
            FactorySic.GetPrnVersiongrpReporsitory().Update(entity);
        }

        /// <summary>
        /// Elimina un registro de la tabla PRN_VERSIONGRP
        /// </summary>
        public void DeletePrnVersiongrp(int vergrpcodi)
        {
            FactorySic.GetPrnVersiongrpReporsitory().Delete(vergrpcodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PRN_VERSIONGRP por nombre
        /// </summary>
        public PrnVersiongrpDTO GetByNamePrnVersiongrp(string vergrpnomb)
        {
            return FactorySic.GetPrnVersiongrpReporsitory().GetByName(vergrpnomb);
        }

        /// <summary>
        /// Lista de versiones ejecutadas del pronóstico por barras filtradas por fecha
        /// </summary>
        /// <param name="fechaIni">Fecha de inicio de consulta</param>
        /// <param name="fechaFin">Fecha de termino de consulta</param>
        /// <returns></returns>
        public List<PrnVersiongrpDTO> ListVersionesPronosticoPorFecha(string fechaIni, string fechaFin)
        {
            return FactorySic.GetPrnVersiongrpReporsitory()
                .ListVersionesPronosticoPorFecha(fechaIni, fechaFin);
        }
        #endregion

        #region Métodos Tabla PRN_CONFIGURACIONFORMULA
        /// <summary>
        /// Inserta un registro de la tabla PRN_CONFIGURACIONFORMULA
        /// </summary>
        public void SavePrnConfiguracionFormula(PrnConfiguracionFormulaDTO entity)
        {
            FactorySic.GetPrnConfiguracionFormulaRepository().Save(entity);
        }

        /// <summary>
        /// Actualiza un registro de la tabla PRN_CONFIGURACIONFORMULA
        /// </summary>
        public void UpdatePrnConfiguracionFormula(PrnConfiguracionFormulaDTO entity)
        {
            FactorySic.GetPrnConfiguracionFormulaRepository().Update(entity);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PRN_CONFIGURACIONFORMULA
        /// </summary>
        public PrnConfiguracionFormulaDTO GetByIdPrnConfiguracionFormulas(int ptomedicodi, DateTime prncfgfecha)
        {
            return FactorySic.GetPrnConfiguracionFormulaRepository().GetParametrosByIdFecha(ptomedicodi, prncfgfecha);
        }
        #endregion

        #region Métodos Tabla PRN_CONFIGURACIONDIA
        /// <summary>
        /// Inserta un registro de la tabla PRN_CONFIGURACIONDIA
        /// </summary>
        public void SavePrnConfiguracionDia(PrnConfiguracionDiaDTO entity)
        {
            try
            {
                FactorySic.GetPrnConfiguracionDiaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PRN_CONFIGURACIONDIA
        /// </summary>
        public void UpdatePrnConfiguracionDia(PrnConfiguracionDiaDTO entity)
        {
            FactorySic.GetPrnConfiguracionDiaRepository().Update(entity);
        }

        /// <summary>
        /// Elimina un registro de la tabla PRN_CONFIGURACIONDIA
        /// </summary>
        public void DeletePrnConfiguracionDia(int cfgdiacodi)
        {
            FactorySic.GetPrnConfiguracionDiaRepository().Delete(cfgdiacodi);
        }

        /// <summary>
        /// Obtiene registros de la tabla PRN_CONFIGURACIONDIA por rango de fechas
        /// </summary>
        /// <param name="fechaIni">Fecha de inicio del rango</param>
        /// <param name="fechaFin">Fecha de termino del rango</param>
        /// <returns></returns>
        public List<PrnConfiguracionDiaDTO> CnfDiaObtenerPorRango(string fechaIni, string fechaFin)
        {
            return FactorySic.GetPrnConfiguracionDiaRepository()
                .ObtenerPorRango(fechaIni, fechaFin);
        }
        #endregion

        #region Métodos Tabla PRN_PERFILBARRA
        /// <summary>
        /// Inserta un registro de la tabla PRN_PERFILBARRA
        /// </summary>
        /// <param name="entity">Entidad de la tabla PRN_PERFILBARRA</param>
        public void SavePrnPerfilBarra(PrnPerfilBarraDTO entity)
        {
            try
            {
                FactorySic.GetPrnPerfilBarraRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Actualiza un registro de la tabla PRN_PERFILBARRA
        /// </summary>
        /// <param name="entity">Entidad de la tabla PRN_PERFILBARRA</param>
        public void UpdatePrnPerfilBarra(PrnPerfilBarraDTO entity)
        {
            try
            {
                FactorySic.GetPrnPerfilBarraRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Elimina un registro de la tabla PRN_PERFILBARRA
        /// </summary>
        /// <param name="prfbarcodi">Identificador de la tabla PRN_PERFILBARRA</param>
        public void DeletePrnPerfilBarra(int prfbarcodi)
        {
            try
            {
                FactorySic.GetPrnPerfilBarraRepository().Delete(prfbarcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Lista todos los registros de la tabla PRN_PERFILBARRA
        /// </summary>
        /// <returns></returns>
        public List<PrnPerfilBarraDTO> ListPrnPerfilBarra()
        {
            return FactorySic.GetPrnPerfilBarraRepository().List();
        }
        /// <summary>
        /// Devuelve un registro de la tabla PRN_PERFILBARRA segun parámetros
        /// </summary>
        /// <param name="reltnacodi">Identificador de la relación barra TNA</param>
        /// <param name="prfbartipodia">Identificador del tipo de día
        /// 0: Lunes,
        /// 1: Sabado,
        /// 2: Domingo,
        /// 3: Otros (Ma, Mi, Ju y Vi)
        /// </param>
        /// <returns></returns>
        public PrnPerfilBarraDTO GetByIdsPrnPerfilBarra(int reltnacodi, string prfbartipodia)
        {
            return FactorySic.GetPrnPerfilBarraRepository()
                .GetByIds(reltnacodi, prfbartipodia);
        }
        #endregion

        /// <summary>
        /// Método que genera un reporte excel simple
        /// </summary>
        public string ExportarReporteSimple(PrnFormatoExcel formato, string pathFile, string filename)
        {
            //string Reporte = filename + "_" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx";
            string Reporte = filename + ".xlsx";
            ExcelDocument.GenerarArchivoExcel(formato, pathFile + Reporte);

            return Reporte;
        }

        /// <summary>
        /// Método que genera un reporte excel simple con libros
        /// </summary>
        public string ExportarReporteConLibros(List<PrnFormatoExcel> formatos, string pathFile, string filename)
        {
            //string Reporte = filename + "_" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx";
            string Reporte = filename + ".xlsx";
            ExcelDocument.GenerarArchivoExcelConLibros(formatos, pathFile + Reporte);

            return Reporte;
        }

        /// <summary>
        /// Método que genera un reporte excel simple con libros y estilos por celda [Solo contenido]
        /// </summary>
        public string ExportarReporteConLibrosyEstilosPorCelda(List<PrnFormatoExcel> formatos, string pathFile, string filename)
        {
            //string Reporte = filename + "_" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx";
            string Reporte = filename + ".xlsx";
            ExcelDocument.GenerarExcelConLibrosyEstilosPorCeldas(formatos, pathFile + Reporte);

            return Reporte;
        }

        #region Reporte Pronostico Barra

        /// <summary>
        /// Da formato a la data para el reporte
        /// </summary>
        /// <param name="inicio">Fecha de inicio de la consulta para la exportación</param>
        /// <param name="fin">Fecha de termino de la consulta para la exportación</param>
        /// <param name="idVersion">Identificador de la versión a consultar</param>
        public string PronosticoPorBarrasExportar(string inicio, string fin, int idVersion)
        {
            //Para el Tag resumen en el Reporte
            List<PrnFormatoExcel> libro = new List<PrnFormatoExcel>();
            int[] tipos = new int[] { ConstantesProdem.PrnmgrtDemVegetativa, ConstantesProdem.PrnmgrtDemIndustrial, ConstantesProdem.PrnmgrtProdemBarra };
            int[] areacodi = new int[] { ConstantesProdem.AreacodiANorte, ConstantesProdem.AreacodiASur, ConstantesProdem.AreacodiACentro, ConstantesProdem.AreacodiASierraCentro };
            string[] areanombre = new string[] { ConstantesProdem.AOperativaNorte, ConstantesProdem.AOperativaSur, ConstantesProdem.AOperativaCentro, ConstantesProdem.AOperativaSierraCentro };
            int dias = Convert.ToInt32((DateTime.ParseExact(fin, "dd/MM/yyyy", CultureInfo.InvariantCulture) - DateTime.ParseExact(inicio, "dd/MM/yyyy", CultureInfo.InvariantCulture)).TotalDays) + 1;

            #region HOJA RESUMEN
            List<PrnMediciongrpDTO> listaTotal = this.ListaPronosticoBarraTotal(inicio, fin, idVersion);
            //Se le suma 1 por la columna de intervalos
            int cantidadTotal = areacodi.Count() + 1;

            //Llena el excel
            //Ancho de columnas
            int[] columnasAncho = new int[cantidadTotal];
            for (int i = 0; i < cantidadTotal; i++)
            {
                columnasAncho[i] = 50;
            }

            PrnFormatoExcel hoja = new PrnFormatoExcel()
            {
                Titulo = "PRONÓSTICO DE BARRAS " + inicio + " - " + fin,
                Subtitulo1 = "Resumen",
                AnchoColumnas = columnasAncho,
                NombreLibro = "Resumen"
            };

            //Creando las cabeceras
            //Primera cabecera - fila 01
            hoja.NestedHeader1 = new List<PrnExcelHeader>();
            PrnExcelHeader head = new PrnExcelHeader() { Etiqueta = "", Columnas = 1 };
            hoja.NestedHeader1.Add(head);
            for (int i = 0; i < areanombre.Count(); i++)
            {
                head = new PrnExcelHeader() { Etiqueta = areanombre[i], Columnas = 1 };
                hoja.NestedHeader1.Add(head);
            }

            //Creando el contenido de la hoja
            //Daata
            string[] itv = UtilProdem.GenerarIntervalosFecha(ConstantesProdem.Itv30min, inicio, fin);
            string[][] content = new string[cantidadTotal][];
            content[0] = itv;
            for (int i = 0; i < areacodi.Count(); i++)
            {
                List<PrnMediciongrpDTO> temporal = listaTotal.Where(x => x.Areapadre == areacodi[i]).OrderBy(x => x.Medifecha).ToList();
                decimal[] med = UtilProdem.ConvertirListaMedicionEnArreglo(ConstantesProdem.Itv30min, dias, temporal);
                string[] str_med = Array.ConvertAll(med, x => x.ToString());
                content[i + 1] = str_med;
            }

            hoja.Contenido = content;
            libro.Add(hoja);
            #endregion

            #region HOJA NORTE, SUR, CENTRO Y SIERRA CENTRO
            int c = 0;
            foreach (var item in areacodi)
            {
                List<PrnMediciongrpDTO> lista = this.ListaPronosticoBarraDetalle(inicio, fin, item, idVersion);
                string libroNombre = areanombre[c];
                c++;
                if (lista.Count > 0)
                {
                    //Se multiplica por 3 pq por barra hay 3 columnas y se suma 3 por la columna de intervalos,vegetativa total e industrial total
                    List<int> barras = lista.Where(x => x.Grupocodi != -1).Select(x => x.Grupocodi).Distinct().ToList();
                    int cantidad = barras.Count * 3 + 3;
                    //Llena el excel
                    //Ancho de columnas
                    int[] columnasAnchoArea = new int[cantidad];
                    for (int i = 0; i < cantidad; i++)
                    {
                        columnasAnchoArea[i] = 50;
                    }

                    PrnFormatoExcel hojaArea = new PrnFormatoExcel()
                    {
                        Titulo = "PRONÓSTICO DE BARRAS " + inicio + " - " + fin,
                        Subtitulo1 = libroNombre,
                        AnchoColumnas = columnasAnchoArea,
                        NombreLibro = libroNombre
                    };
                    //Creando las cabeceras
                    //Primera cabecera - fila 01
                    hojaArea.NestedHeader1 = new List<PrnExcelHeader>();
                    PrnExcelHeader headArea = new PrnExcelHeader() { Etiqueta = "", Columnas = 1 };
                    hojaArea.NestedHeader1.Add(headArea);
                    headArea = new PrnExcelHeader() { Etiqueta = "TOTAL", Columnas = 1 };
                    hojaArea.NestedHeader1.Add(headArea);
                    headArea = new PrnExcelHeader() { Etiqueta = "TOTAL", Columnas = 1 };
                    hojaArea.NestedHeader1.Add(headArea);
                    for (int i = 0; i < barras.Count(); i++)
                    {
                        string cabecera = lista.Where(x => x.Grupocodi == barras[i]).Select(x => x.Gruponomb).First();
                        headArea = new PrnExcelHeader() { Etiqueta = cabecera, Columnas = 3 };
                        hojaArea.NestedHeader1.Add(headArea);
                    }

                    //Segunda cabecera - fila 02
                    hojaArea.NestedHeader2 = new List<PrnExcelHeader>();
                    headArea = new PrnExcelHeader() { Etiqueta = "HORA", Columnas = 1 };
                    hojaArea.NestedHeader2.Add(headArea);
                    headArea = new PrnExcelHeader() { Etiqueta = "VEGETATIVA", Columnas = 1 };
                    hojaArea.NestedHeader2.Add(headArea);
                    headArea = new PrnExcelHeader() { Etiqueta = "INDUSTRIAL", Columnas = 1 };
                    hojaArea.NestedHeader2.Add(headArea);
                    for (int i = 0; i < barras.Count; i++)
                    {
                        headArea = new PrnExcelHeader() { Etiqueta = "VEGETATIVA", Columnas = 1 };
                        hojaArea.NestedHeader2.Add(headArea);
                        headArea = new PrnExcelHeader() { Etiqueta = "INDUSTRIAL", Columnas = 1 };
                        hojaArea.NestedHeader2.Add(headArea);
                        headArea = new PrnExcelHeader() { Etiqueta = "TOTAL", Columnas = 1 };
                        hojaArea.NestedHeader2.Add(headArea);
                    }

                    //Creando el contenido de la hoja
                    //Data
                    string[][] contentArea = new string[cantidad][];
                    contentArea[0] = itv;
                    //Almacena el vegetativo total
                    List<PrnMediciongrpDTO> vtemporal = lista.Where(x => x.Grupocodi == -1 && x.Prnmgrtipo == ConstantesProdem.PrnmgrtDemVegetativa * -1)
                                                                     .OrderBy(x => x.Medifecha).ToList();
                    decimal[] veg = UtilProdem.ConvertirListaMedicionEnArreglo(ConstantesProdem.Itv30min, dias, vtemporal);
                    string[] str_veg = Array.ConvertAll(veg, x => x.ToString());
                    contentArea[1] = str_veg;
                    //Almacena el industrial total
                    List<PrnMediciongrpDTO> itemporal = lista.Where(x => x.Grupocodi == -1 && x.Prnmgrtipo == ConstantesProdem.PrnmgrtDemIndustrial * -1)
                                                                     .OrderBy(x => x.Medifecha).ToList();
                    decimal[] ind = UtilProdem.ConvertirListaMedicionEnArreglo(ConstantesProdem.Itv30min, dias, itemporal);
                    string[] str_ind = Array.ConvertAll(ind, x => x.ToString());
                    contentArea[2] = str_ind;

                    int s = 2;
                    for (int i = 0; i < barras.Count(); i++)
                    {
                        foreach (var tipo in tipos)
                        {
                            List<PrnMediciongrpDTO> temporal = lista.Where(x => x.Grupocodi == barras[i] && x.Prnmgrtipo == tipo)
                                                                     .OrderBy(x => x.Medifecha).ToList();
                            decimal[] med = UtilProdem.ConvertirListaMedicionEnArreglo(ConstantesProdem.Itv30min, dias, temporal);
                            string[] str_med = Array.ConvertAll(med, x => x.ToString());
                            contentArea[s + 1] = str_med;
                            s++;
                        }
                    }

                    hojaArea.Contenido = contentArea;
                    libro.Add(hojaArea);
                }
            }
            #endregion

            string reporte = "-1";
            if (libro.Count() > 0)
            {
                string pathFile = ConfigurationManager.AppSettings[ConstantesProdem.ReportePronostico].ToString();
                string filename = "PRONÓSTICO-";
                filename += inicio.Replace("/", string.Empty) + "-";
                filename += fin.Replace("/", string.Empty);
                reporte = this.ExportarReporteConLibros(libro, pathFile, filename);
            }

            return reporte;
        }

        /// <summary>
        /// Devuelve el pronóstico a nivel de barras CP para el reporte
        /// </summary>
        /// <param name="inicio">Fecha de inicio de la consulta</param>
        /// <param name="fin">Fecha de termino de la consulta</param>
        /// <param name="idVersion">Identificador de la versión</param>
        /// <returns></returns>
        public List<PrnMediciongrpDTO> ListaPronosticoBarraTotal(string inicio, string fin, int idVersion)
        {
            return FactorySic.GetPrnMediciongrpRepository().ListaPronosticoBarraTotal(inicio, fin, idVersion);
        }

        /// <summary>
        /// Devuelve el pronóstico a nivel de barras CP con su relación con cada área
        /// </summary>
        /// <param name="inicio">Fecha de inicio de la consulta</param>
        /// <param name="fin">Fecha de termino de la consulta</param>
        /// <param name="area">Identificador del área a consultar</param>
        /// <param name="idVersion">Identificador de la versión</param>
        /// <returns></returns>
        public List<PrnMediciongrpDTO> ListaPronosticoBarraDetalle(string inicio, string fin, int area, int idVersion)
        {
            return FactorySic.GetPrnMediciongrpRepository().ListaPronosticoBarraDetalle(inicio, fin, area, idVersion);
        }
        #endregion

        /// <summary>
        /// Lista de puntos de medición relacionados a una barra CP
        /// </summary>
        /// <param name="areacodi">Identificador de la tabla EQ_AREA (representa un área operativa)</param>
        /// <returns></returns>
        public List<MePtomedicionDTO> PR03PuntosPorBarrasCP(string areacodi)
        {
            return FactorySic.GetPrnMedicion48Repository().PR03PuntosPorBarrasCPPronosticoDemanda(areacodi);
        }


        #region Metodos para tabla PRN_AGRUPACIONFORMULAS

        /// <summary>
        /// LIsta los registro de la tabla PRN_AGRUPACIONFORMULAS
        /// </summary>
        public List<PrnAgrupacionFormulasDTO> GetListAgrupacionFormulas()
        {
            return FactorySic.GetPrnAgrupacionFormulasRepository().List();
        }

        /// <summary>
        /// Inserta un registro de la tabla PRN_AGRUPACIONFORMULAS
        /// </summary>
        public void SavePrnAgrupacionFormulas(PrnAgrupacionFormulasDTO entity)
        {
            try
            {
                FactorySic.GetPrnAgrupacionFormulasRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PRN_AGRUPACIONFORMULAS
        /// </summary>
        public void DeletePrnAgrupacionFormulas(int agrupacion)
        {
            FactorySic.GetPrnAgrupacionFormulasRepository().Delete(agrupacion);
        }
        #endregion

        #region Agrupacion Usuarios Libres - Formulas

        /// <summary>
        /// Lista las formulas relacionadas a una agrupacion
        /// </summary>
        /// <param name="agrupacion">Identificador de la agrupacion</param>
        /// <returns></returns>
        public List<PrnAgrupacionFormulasDTO> GetListAgrupacionFormulasByAgrupacion(int agrupacion)
        {
            return FactorySic.GetPrnAgrupacionFormulasRepository().GetListAgrupacionFormulasByAgrupacion(agrupacion);
        }

        /// <summary>
        /// Lista las formulas activas
        /// </summary>
        /// <returns></returns>
        public List<MePerfilRuleDTO> ListaPerfilRuleForEstimador(string prefijo)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().ListPerfilRuleByEstimador(prefijo);
        }

        /// <summary>
        /// Lista de agrupaciones de usuarios libres.
        /// </summary>
        /// <returns></returns>
        public object ListAgrupacionesUsuariosLibres(int regIni, int regByPagina)
        {
            List<object> agrupaciones = new List<object>();

            List<MePtomedicionDTO> dataTabla = this.ListAgrupacionesActivas("0", "0", "0", 1);

            //Validación: no registros
            int totalRegistros = dataTabla.Count;
            if (totalRegistros == 0) return new { data = agrupaciones, recordsTotal = 0, recordsFiltered = 0 };

            ////Obtiene el rango de registros que se mostraran
            if (totalRegistros > regByPagina)
            {
                int tempDiff = totalRegistros - regIni;
                dataTabla = (tempDiff < regByPagina) ? dataTabla.GetRange(regIni, tempDiff) : dataTabla.GetRange(regIni, regByPagina);
            }

            object entity;

            foreach (var item in dataTabla)
            {
                entity = new
                {
                    id = item.Ptomedicodi,
                    nombre = item.Ptomedidesc
                };
                agrupaciones.Add(entity);
            }

            return new { data = agrupaciones, recordsTotal = totalRegistros, recordsFiltered = totalRegistros };
        }

        /// <summary>
        /// Lista de formulas disponibles
        /// </summary>
        /// <param name="agrupacion">Identificador de la agrupacion</param>
        /// <returns></returns>
        public List<object> ListFormulasDisponibles(int agrupacion)
        {
            List<object> tabla = new List<object>();

            string prefijo = "%";
            List<MePerfilRuleDTO> dataTabla = this.ListaPerfilRuleForEstimador(prefijo);
            List<PrnAgrupacionFormulasDTO> formulaSeleccionadas = this.GetListAgrupacionFormulasByAgrupacion(agrupacion);
            foreach (var item in formulaSeleccionadas)
            {
                dataTabla.RemoveAll(x => x.Prrucodi == item.Prrucodi);
            }

            object entity;
            foreach (var item in dataTabla)
            {
                entity = new
                {
                    Prrucodi = item.Prrucodi,
                    Prruabrev = item.Prruabrev,
                    Data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                    ArrayDatos = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, new PrnAgrupacionFormulasDTO())
                };

                tabla.Add(entity);
            }

            return tabla;
        }

        /// <summary>
        /// Formulas relacionadas a una agrupacion
        /// </summary>
        /// <param name="agrupacion">Fecha de inicio de la consulta para la exportación</param>
        public List<object> ListFormulasSeleccionadas(int agrupacion)
        {
            List<object> tabla = new List<object>();

            List<PrnAgrupacionFormulasDTO> dataTabla = this.GetListAgrupacionFormulasByAgrupacion(agrupacion);

            object entity;
            //LLenando el objeto
            foreach (var item in dataTabla)
            {
                entity = new
                {
                    Prrucodi = item.Prrucodi,
                    Prruabrev = item.Prruabrev,
                    Prnafmflagesmanual = item.Prnafmflagesmanual,
                    Prnafmprioridad = item.Prnafmprioridad,
                    Data = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv30min),
                    ArrayDatos = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, item)
                };

                tabla.Add(entity);
            }

            return tabla;
        }

        /// <summary>
        /// Registra la nueva relación
        /// </summary>
        /// <param name="idAgrupacion">Identificador de la Agrupacion</param>
        /// <param name="listFormulas">Lista de las formulas relacionadas a las agrupaciones</param>
        /// <returns></returns>
        public object AgrupacionFormulasSave(int idAgrupacion, List<PrnAgrupacionFormulasDTO> listFormulas)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;

            ////Elimina los registros previamente relacionados
            this.DeletePrnAgrupacionFormulas(idAgrupacion);

            if (listFormulas != null)
            {
                //Registra las nuevas relaciones
                foreach (PrnAgrupacionFormulasDTO entity in listFormulas)
                {
                    entity.Ptomedicodi = idAgrupacion;
                    for (int i = 1; i <= ConstantesProdem.Itv30min; i++)
                    {
                        entity.GetType().GetProperty("H" + i.ToString()).SetValue(entity, entity.ArrayDatos[i - 1]);
                    }
                    this.SavePrnAgrupacionFormulas(entity);
                }

                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El registro se realizó de manera exitosa";
            }
            else
            {
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "Se eliminaron las relaciones...";
            }
            return new { typeMsg, dataMsg };
        }
        #endregion

        #region Métodos Bitacora E3
        /// <summary>
        /// Inserta un registro en la tabla PRN_BITACORAPROD3
        /// </summary>
        /// <param name="Emprcodi">Codigo de empresa</param>
        /// <param name="Medifecha">Fecha de medida</param>
        /// <param name="ArrIntervalohras">Listado de intervalos de horas de ajuste</param>
        /// <param name="ArrConstipico">Listado de consumos tipicos</param>
        /// <param name="ArrConsprevisto">Listado de consumos previstos</param>
        /// <param name="Ptomedicodi">Codigo de punto de medicion</param>
        /// <param name="Lectcodi">Codigo de tipo de demanda</param>
        /// <param name="Tipoemprcodi">Codigo de tipo de empresa</param>
        /// <param name="Valor">Codigo de area</param>
        /// <returns></returns>
        public object SaveBitacora(int? Emprcodi,
                                   string Medifecha,
                                   List<string> ArrIntervalohras,
                                   List<decimal> ArrConstipico,
                                   List<decimal> ArrConsprevisto,
                                   int Ptomedicodi,
                                   int? Lectcodi,
                                   int? Tipoemprcodi,
                                   string Valor)
        {
            string typeMsg = ConstantesProdem.MsgSuccess;
            string dataMsg = "El registro se realizó de manera exitosa";

            PrnBitacoraDTO entity = new PrnBitacoraDTO();

            entity.Emprcodi = Emprcodi;

            entity.Medifecha = DateTime.ParseExact(Medifecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            // Obtener la hora de inicio y fin del array ArrIntervalohras
            entity.Prnbithorainicio = ArrIntervalohras.First();
            entity.Prnbithorafin = ArrIntervalohras.Last();

            entity.Prnbittipregistro = "1";
            entity.Prnbitocurrencia = "Ajuste Manual";

            // Se obtiene la barra apartir del punto de medicion
            MePtomedicionDTO entityBarra = GetByIdMePtomedicion(Ptomedicodi);
            entity.Grupocodi = entityBarra.Grupocodibarra;

            // Obtener lel promedio de los valores del array ArrConstipico
            decimal SumaConstipico = 0;
            for (int i = 0; i < ArrConstipico.Count; i++)
            {
                SumaConstipico += ArrConstipico[i];
            }
            entity.Prnbitconstipico = SumaConstipico / ArrConstipico.Count;

            // Obtener lel promedio de los valores del array ArrConsprevisto
            decimal SumaConsprevisto = 0;
            for (int j = 0; j < ArrConsprevisto.Count; j++)
            {
                SumaConsprevisto += ArrConsprevisto[j];
            }
            entity.Prnbitconsprevisto = SumaConsprevisto / ArrConsprevisto.Count;

            entity.Prnbitptodiferencial = entity.Prnbitconstipico - entity.Prnbitconsprevisto;

            entity.Ptomedicodi = Ptomedicodi;

            entity.Lectcodi = Lectcodi;
            entity.Tipoemprcodi = Tipoemprcodi;
            entity.Prnbitvalor = Valor;

            FactorySic.GetPrnPronosticoDemandaRepository().SaveBitacora3(entity);

            return new { typeMsg, dataMsg };
        }

        /// <summary>
        /// Lista los registros de la tabla PRN_BITACORAPROD3
        /// </summary>
        /// <param name="fechaIni">Fecha inicio de la consulta</param>
        /// <param name="fechaFin">Fecha fin de la consulta</param>
        /// <param name="tipoemprcod">Identificador del tipo de empresa</param>
        /// <param name="lectcodi">Identificador del tipo de lectura</param>
        /// <param name="tipregistro">Identificador del tipo de registro</param>
        /// <param name="regIni">Registro inicial del paginado</param>
        /// <param name="regByPag">Registros por pagina</param>
        /// <returns></returns>
        public object ListNewBitacora(string fechaIni, 
            string fechaFin, List<int> lectcodi,
            List<int> tipoemprcod, string tipregistro,
            int regIni, int regByPag)
        {
            string stipoemprcod = (tipoemprcod.Count != 0)
                ? string.Join(",", tipoemprcod) : "0";
            string slectcodi = (lectcodi.Count != 0) 
                ? string.Join(",", lectcodi) : "0";

            List<PrnBitacoraDTO> datosBitacora = FactorySic
                .GetPrnPronosticoDemandaRepository()
                .ListBitacora(fechaIni, fechaFin,
                tipregistro, stipoemprcod, 
                slectcodi);

            int totalRegistros = datosBitacora.Count;
            int regFin = ((regIni + regByPag) >= totalRegistros)
                ? (totalRegistros - regIni)
                : regIni + regByPag;

            List<PrnBitacoraDTO> datosFiltrados = datosBitacora
                .GetRange(regIni, regFin);
            
            List<object> entities = new List<object>();
            foreach (var item in datosFiltrados)
            {
                object entity;
                string parseFechaFin = (item.Prnbithorafin == "00:00")
                    ? item.Medifecha.Value
                    .AddDays(1)
                    .ToString(ConstantesProdem.FormatoFecha)
                    : item.Medifecha.Value
                    .ToString(ConstantesProdem.FormatoFecha);

                entity = new
                {
                    area = item.Prnbitvalor,
                    empresa = item.Emprnomb,
                    fechaInicio = item.Medifecha.Value.ToString(ConstantesProdem.FormatoFecha),
                    horaInicio = item.Prnbithorainicio,
                    fechaFin = parseFechaFin,
                    horaFin = item.Prnbithorafin,
                    consumoTipico = item.Prnbitconstipico,
                    consumoPrevisto = item.Prnbitconsprevisto,
                    potenciaDiferencial = item.Prnbitptodiferencial,
                    barra = item.Gruponomb,
                    punto = item.Ptomedidesc,
                    ocurrencia = item.Prnbitocurrencia,
                };

                entities.Add(entity);
            }

            return new { 
                data = entities, 
                recordsTotal = totalRegistros, 
                recordsFiltered = totalRegistros,
            };
        }
        #endregion

        #region Prioridades Demanda Industrial y Flujos de Interconexion

        /// <summary>
        /// Obtiene las agrupaciones de usuarios libres que pertenecen a una barra PM
        /// </summary>
        /// <param name="pm">Identificador de la tabla PR_GRUPO corresponde a una BarraPM</param>
        /// <returns></returns>
        public List<MePtomedicionDTO> GetAgrupacionByBarraPM(int pm)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().GetAgrupacionByBarraPM(pm);
        }


        /// <summary>
        /// Prioridad de informacion para obtener la demanda industrial
        /// </summary>
        /// <param name="pm">Identificador de la tabla PR_GRUPO corresponde a una BarraPM</param>
        /// <param name="gauss">Valor del factor gauss asignado a la barra PM</param>
        /// <param name="perdida">Valor de la pérdida asignada a la barra PM</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public decimal[] PrioridadDemandaIndustrial(int pm, decimal gauss, decimal perdida, DateTime regFecha)
        {
            //Prioridad01, diario de la informacion ejecutada del PR03
            PrnMedicion48DTO lista = this.GetDataBarraPMUlibrePorDia(pm, gauss, perdida, regFecha);
            decimal[] dato = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, lista);
            if (dato.Sum() > 0)
            {
                return dato;
            }

            //Prioridad02 y Prioridad03, del modelo TNA dependiendo
            List<MePtomedicionDTO> agrupaciones = this.GetAgrupacionByBarraPM(pm);
            if (agrupaciones.Count == 0)
            {
                return new decimal[ConstantesProdem.Itv30min];
            }
            //Se trae las formulas por agrupacion
            List<PrnAgrupacionFormulasDTO> agrupacionFormulaList = this.GetListAgrupacionFormulas();

            decimal[] total = new decimal[ConstantesProdem.Itv30min];

            foreach (var item in agrupaciones)
            {
                List<PrnAgrupacionFormulasDTO> formulas = agrupacionFormulaList.Where(x => x.Ptomedicodi == item.Ptomedicodi)
                                                            .OrderBy(x => x.Prnafmprioridad).ToList();

                foreach (PrnAgrupacionFormulasDTO entidad in formulas)
                {
                    decimal[] data = new decimal[ConstantesProdem.Itv30min];
                    if (entidad.Prnafmflagesmanual == "S")
                    {
                        data = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, entidad);
                    }
                    else
                    {
                        data = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min,
                                    this.ObtenerMedicionesCalculadas(entidad.Prrucodi, regFecha));
                    }

                    if (data.Sum() > 0)
                    {
                        for (int i = 0; i < ConstantesProdem.Itv30min; i++)
                        {
                            total[i] += data[i];
                        }
                        break;
                    }
                }
            }

            if (total.Sum() > 0)
            {
                return total;
            }
            else
            {
                regFecha = regFecha.AddDays(-1);
                return this.PrioridadDemandaIndustrial(pm, gauss, perdida, regFecha);
            }

        }
        #endregion

        #region Métodos del Módulo de Servicios Auxiliares
        /// <summary>
        /// Método que devuelve los servicios auxiliares registrados para una barra
        /// </summary>
        /// <param name="idBarra">Identificador de un barra</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="rangoDias">Rnago de dias historicos</param>
        /// <param name="esDiaTipico">Flag que indica si se busca un día tipico o no</param>
        /// <returns></returns>
        public decimal[] ObtenerSSAAPorBarra(int idBarra,
            DateTime regFecha,
            List<DateTime> rangoDias,
            bool esDiaTipico)
        {            
            List<PrnServiciosAuxiliaresDTO> entidad = this.GetServiciosAuxiliaresByGrupo(idBarra);

            int diaSemana = (int)regFecha.DayOfWeek;
            DateTime diaBase = new DateTime(regFecha.Year,
                regFecha.Month,
                1, 0, 0, 0);
            DateTime diaBusqueda = UtilProdem.ObtenerUltimoDiaHistoricoValido(diaBase, 
                diaSemana, 
                rangoDias, 
                esDiaTipico);
            List<DateTime> diasHistoricos = new List<DateTime> { diaBusqueda };
            diasHistoricos.AddRange(UtilProdem
                .ObtenerFechasHistoricasPorRango(diaBusqueda, 4,
                rangoDias, 
                esDiaTipico));

            decimal[] ssaa = new decimal[ConstantesProdem.Itv30min];
            foreach (DateTime d in diasHistoricos)
            {
                foreach (PrnServiciosAuxiliaresDTO e in entidad)
                {
                    decimal[] arrMedicion = new decimal[ConstantesProdem.Itv30min];
                    if (e.Prnauxflagesmanual)
                        arrMedicion = UtilProdem
                            .ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, e);
                    else
                        arrMedicion = this.ObtenerMedicionesCalculadas(e.Prrucodi, d);
                    ssaa = ssaa.Zip(arrMedicion, (a, b) => a + b).ToArray();
                }
            }
            ssaa = ssaa.Select(x => x / diasHistoricos.Count).ToArray();

            return ssaa;
        }

        /// <summary>
        /// Lista Barras y sus Formulas
        /// </summary>
        /// <param name=""></param>
        /// <returns>List de PrnServiciosAuxiliaresDTO </returns>
        public List<PrnServiciosAuxiliaresDTO> ListarBarrasFormulas()
        {
            List<PrnServiciosAuxiliaresDTO> entitys = new List<PrnServiciosAuxiliaresDTO>();
            PrnServiciosAuxiliaresDTO entity = null;

            List<PrnServiciosAuxiliaresDTO> data = FactorySic.GetServiciosAuxiliaresRepository().ListBarraFormulas();

            if (data.Count > 0)
            {
                int idGrupo = 0;
                foreach (PrnServiciosAuxiliaresDTO d in data)
                {
                    if (idGrupo != d.Grupocodi)
                    {
                        entity = new PrnServiciosAuxiliaresDTO();

                        idGrupo = d.Grupocodi;

                        entity.Grupocodi = d.Grupocodi;
                        entity.Gruponomb = d.Gruponomb;
                        entity.Prruabrev = "<p>" + d.Prruabrev + " </p> ";

                        entitys.Add(entity);
                    }
                    else
                    {
                        entity.Prruabrev += "<p>" + d.Prruabrev + " </p> ";
                    }
                }
            }


            return entitys;
        }

        /// <summary>
        /// Lista relaciones de Barras y sus Formulas
        /// </summary>
        /// <param name="idPrGrupo"></param>
        /// <returns>object { listFormulas, listaSeleccionados }</returns>
        public object ListarRelacionesBarraFormula(int idPrGrupo)
        {
            // Lista las relaciones y llena el array de barras de las relaciones de listaSeleccionados
            List<PrnServiciosAuxiliaresDTO> listaSeleccionados = new List<PrnServiciosAuxiliaresDTO>();
            listaSeleccionados = GetServiciosAuxiliaresByGrupo(idPrGrupo);

            foreach (PrnServiciosAuxiliaresDTO itemArr in listaSeleccionados)
            {
                itemArr.ArrayMediciones = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, itemArr);
            }

            // Lista las formulas
            List<MePerfilRuleDTO> listFormulas = new List<MePerfilRuleDTO>();
            listFormulas = this.ListaFormulas(idPrGrupo);

            // Instancia y llena el objeto
            object res = new { listFormulas, listaSeleccionados };

            return res;
        }

        /// <summary>
        /// Graba el registro de Servicio Auxiliar y sus formulas relacionadas
        /// </summary>
        /// <param name="idPrGrupo"></param>
        /// <param name="listaSeleccionados"></param>
        /// <returns>object { typeMsg, dataMsg }</returns>
        public object SaveServicioAuxiliar(int idPrGrupo, List<PrnServiciosAuxiliaresDTO> listaSeleccionados)
        {
            PrnServiciosAuxiliaresDTO entity = new PrnServiciosAuxiliaresDTO();
            string typeMsg = "";
            string dataMsg = "";

            // Se eliminan las relaciones previas
            if (idPrGrupo > 0)
            {
                FactorySic.GetServiciosAuxiliaresRepository().DeleteRelaciones(idPrGrupo);
            }

            if (listaSeleccionados != null)
            {
                // Se registran las nuevas relaciones
                foreach (PrnServiciosAuxiliaresDTO item in listaSeleccionados)
                {
                    entity.Grupocodi = idPrGrupo;
                    entity.Prrucodi = item.Prrucodi;
                    entity.Prnauxflagesmanual = item.Prnauxflagesmanual;

                    for (int i = 0; i < item.ArrayMediciones.Length; i++)
                    {
                        entity.GetType().GetProperty("H" + (i + 1)).SetValue(entity, item.ArrayMediciones[i]);
                    }

                    FactorySic.GetServiciosAuxiliaresRepository().Save(entity);
                }

                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "Se registraron las relaciones satisfactoriamente!";
            }
            else
            {
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "Se eliminaron las relaciones...";
            }
            object res = new { typeMsg, dataMsg };

            return res;
        }

        /// <summary>
        /// Obtener listado de servicios auxiliares x grupo
        /// </summary>
        /// <param name="PrGrupo">Id de grupo</param>
        /// <returns>List de PrnServiciosAuxiliaresDTO</returns>
        public List<PrnServiciosAuxiliaresDTO> GetServiciosAuxiliaresByGrupo(int PrGrupo)
        {
            return FactorySic.GetServiciosAuxiliaresRepository().GetServiciosAuxiliaresByGrupo(PrGrupo);
        }

        /// <summary>
        /// Lista Formulas
        /// </summary>
        /// <returns>List MePerfilRuleDTO</returns>
        public List<MePerfilRuleDTO> ListaFormulas()
        {
            return FactorySic.GetServiciosAuxiliaresRepository().ListFormulas();
        }

        /// <summary>
        /// Lista Formulas relaciones
        /// </summary>
        /// <param name="Grupocodi"></param>
        /// <returns>List MePerfilRuleDTO</returns>
        public List<MePerfilRuleDTO> ListaFormulas(int Grupocodi)
        {
            return FactorySic.GetServiciosAuxiliaresRepository().ListFormulasRelaciones(Grupocodi);
        }

        /// <summary>
        /// Elimina el registro de Servicio Auxiliar de la formula relacionada
        /// </summary>
        /// <param name="prnAuxCodi"></param>
        /// <returns></returns>
        public void DeleteRelacion(int prnAuxCodi)
        {
            FactorySic.GetServiciosAuxiliaresRepository().Delete(prnAuxCodi);
        }
        #endregion

        #region Métodos del Módulo de Perdidas Transversales
        /// <summary>
        /// Lista las barras registradas en Perdidas tranversales(codigo y nombre de la version actual activa), para el filtro.
        /// </summary>
        /// <returns>List(PrGrupoDTO)</returns>
        public List<PrGrupoDTO> ListaBarrasPerdidasTransversales()
        {
            return FactorySic.GetPrnPrdTrasversalRepository().ListaBarrasPerdidasTransversales();
        }

        /// <summary>
        /// Lista las Perdidas tranversales con sus barras y formulas relacionadas
        /// </summary>
        /// <returns>List(PrnPrdTransversalDTO)</returns>
        public List<PrnPrdTransversalDTO> ListPerdidasTransvBarraFormulas()
        {
            List<PrnPrdTransversalDTO> entitys = new List<PrnPrdTransversalDTO>();
            PrnPrdTransversalDTO entity = null;

            List<PrnPrdTransversalDTO> data = FactorySic.GetPrnPrdTrasversalRepository().ListPerdidasTransvBarraFormulas();

            if (data.Count > 0)
            {
                string nomPerdida = string.Empty;
                foreach (PrnPrdTransversalDTO d in data)
                {
                    if (nomPerdida.Trim() != d.Prdtrnetqnomb.Trim())
                    {
                        entity = new PrnPrdTransversalDTO();

                        nomPerdida = d.Prdtrnetqnomb;
                        entity.Prdtrncodi = d.Prdtrncodi;
                        entity.Prdtrnetqnomb = d.Prdtrnetqnomb;

                        entity.Gruponomb = "<p>" + d.Gruponomb + " </p> ";
                        entity.Prruabrev = "<p>" + d.Prruabrev + " </p> ";

                        entitys.Add(entity);
                    }
                    else
                    {
                        entity.Gruponomb += "<p>" + d.Gruponomb + " </p> ";
                        entity.Prruabrev += "<p>" + d.Prruabrev + " </p> ";
                    }
                }
            }


            return entitys;
        }

        /// <summary>
        /// Lista relaciones de Perdidas transversales Barras y sus Formulas
        /// </summary>
        /// <param name="Prdtrncodi">Nombre de Perdida Tranversal</param>
        /// <param name="Prdtrnetqnomb">Nombre de Perdida Tranversal</param>
        /// <returns>object { listFormulas, listaSeleccionados }</returns>
        public object ListarRelacionesPerdidasTransvBarraFormula(int Prdtrncodi, string Prdtrnetqnomb)
        {
            // Lista las relaciones y llena el array de barras de las relaciones de listaSeleccionados
            List<PrnPrdTransversalDTO> listaSeleccionados = new List<PrnPrdTransversalDTO>();
            listaSeleccionados = GetPerdidasTransversalesByNombre(Prdtrnetqnomb);

            foreach (PrnPrdTransversalDTO itemArr in listaSeleccionados)
            {
                itemArr.ArrayMediciones = UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, itemArr);
            }

            // Lista las Barras
            List<PrnPrdTransversalDTO> listBarras = new List<PrnPrdTransversalDTO>();
            listBarras = ListarBarrasPerdidasTransversales(Prdtrncodi);

            // Instancia y llena el objeto
            object res = new { listBarras, listaSeleccionados };

            return res;
        }

        /// <summary>
        /// Graba el registro de Perdida transversal y sus barras y formulas relacionadas
        /// </summary>
        /// <param name="nombPerdida"></param>
        /// <param name="listaSeleccionados"></param>
        /// <returns>object { typeMsg, dataMsg }</returns>
        public object SavePerdidaTransversal(string nombPerdida, List<PrnPrdTransversalDTO> listaSeleccionados)
        {
            PrnPrdTransversalDTO entity = new PrnPrdTransversalDTO();

            // Se eliminan las relaciones previas
            if (nombPerdida.Trim().Length > 0)
            {
                FactorySic.GetPrnPrdTrasversalRepository().DeleteRelacionesPerdidasTransv(nombPerdida);
            }

            int version = FactorySic.GetPrnPronosticoDemandaRepository().VersionActiva();

            // Se registran las nuevas relaciones
            foreach (PrnPrdTransversalDTO item in listaSeleccionados)
            {
                entity.Prdtrnetqnomb = nombPerdida;
                entity.Grupocodi = item.Grupocodi;
                entity.Prnvercodi = version;
                entity.Prrucodi = item.Prrucodi;
                entity.Prdtrnflagesmanual = item.Prdtrnflagesmanual;

                for (int i = 0; i < item.ArrayMediciones.Length; i++)
                {
                    entity.GetType().GetProperty("H" + (i + 1)).SetValue(entity, item.ArrayMediciones[i]);
                }

                FactorySic.GetPrnPrdTrasversalRepository().Save(entity);
            }

            string typeMsg = ConstantesProdem.MsgSuccess;
            string dataMsg = "Se registraron las relaciones satisfactoriamente!";
            object res = new { typeMsg, dataMsg };

            return res;
        }

        /// <summary>
        /// Graba el registro de Perdida transversal y sus barras y formulas relacionadas
        /// </summary>
        /// <param name="nombPerdida"></param>
        /// <returns>object { typeMsg, dataMsg }</returns>
        public object DeletePerdidaTransversal(string nombPerdida)
        {
            FactorySic.GetPrnPrdTrasversalRepository().DeleteRelacionesPerdidasTransv(nombPerdida);

            string typeMsg = ConstantesProdem.MsgSuccess;
            string dataMsg = "Se registraron las relaciones satisfactoriamente!";
            object res = new { typeMsg, dataMsg };

            return res;
        }
        // --------------------------------------------------------------------------------------------

        /// <summary>
        /// Obtener listado de Perdidas Transversales x grupo
        /// </summary>
        /// <param name="Prdtrnetqnomb">Nombre de Perdida Tranversal</param>
        /// <returns>List de PrnServiciosAuxiliaresDTO</returns>
        public List<PrnPrdTransversalDTO> GetPerdidasTransversalesByNombre(string Prdtrnetqnomb)
        {
            return FactorySic.GetPrnPrdTrasversalRepository().GetPerdidasTransversalesByNombre(Prdtrnetqnomb);
        }

        /// <summary>
        /// Lista Barra y Formulas
        /// </summary>
        /// <param name="Prdtrncodi"></param>
        /// <returns>List MePerfilRuleDTO</returns>
        public List<PrnPrdTransversalDTO> ListarBarrasPerdidasTransversales(int Prdtrncodi)
        {
            return FactorySic.GetPrnPrdTrasversalRepository().ListBarrasPerdidasTransversales(Prdtrncodi);
        }

        /// <summary>
        /// Devuelve las perdidas transversales registradas a una barra CP
        /// </summary>
        /// <param name="grupocodi">Identificador de una barra CP</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="rangoDias">Rango de dias historicos</param>
        /// <param name="esDiaTipico">Flag que indica si se busca un día tipico o no</param>
        /// <returns></returns>
        public decimal[] ObtenerPerdidaPorBarraCP(int grupocodi, 
            DateTime regFecha,
            List<DateTime> rangoDias,
            bool esDiaTipico)
        {            
            List<PrnPrdTransversalDTO> entidad = FactorySic
                .GetPrnPrdTransversalRepository()
                .ObtenerPerdidaPorBarraCP(grupocodi);

            int diaSemana = (int)regFecha.DayOfWeek;
            int nroDiasMes = DateTime.DaysInMonth(regFecha.Year,
                regFecha.Month);
            DateTime diaBase = new DateTime(DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                0, 0, 0);

            DateTime diaBusqueda = (regFecha < diaBase)
                ? UtilProdem.ObtenerUltimoDiaHistoricoValido(regFecha, 
                diaSemana, 
                rangoDias, 
                esDiaTipico)
                : UtilProdem.ObtenerUltimoDiaHistoricoValido(diaBase,
                diaSemana, 
                rangoDias, 
                esDiaTipico);

            List<DateTime> diasHistoricos = new List<DateTime> { diaBusqueda };
            diasHistoricos.AddRange(UtilProdem
                .ObtenerFechasHistoricasPorRango(diaBusqueda, 4,
                rangoDias, 
                esDiaTipico));

            decimal[] perdidaTransversal = new decimal[ConstantesProdem.Itv30min];
            foreach (DateTime d in diasHistoricos)
            {
                foreach (PrnPrdTransversalDTO e in entidad)
                {
                    decimal[] arrMedicion = new decimal[ConstantesProdem.Itv30min];
                    if (e.Prdtrnflagesmanual)
                        arrMedicion = UtilProdem
                            .ConvertirMedicionEnArreglo(ConstantesProdem.Itv30min, e);
                    else
                        arrMedicion = this.ObtenerMedicionesCalculadas(e.Prrucodi, d);
                    perdidaTransversal = perdidaTransversal
                        .Zip(arrMedicion, (a, b) => a + b)
                        .ToArray();
                }
            }
            
            perdidaTransversal = perdidaTransversal
                .Select(x => x / diasHistoricos.Count)
                .ToArray();

            return perdidaTransversal;
        }
        #endregion

        #region Demanda PO
        /// <summary>
        /// Permite obtener los registro de la tabla PRN_VERSIONGRP por area
        /// </summary>
        public List<PrnVersiongrpDTO> ListVersionByArea(string area)
        {
            return FactorySic.GetPrnVersiongrpReporsitory().ListVersionByArea(area);
        }
        #endregion
    }
}


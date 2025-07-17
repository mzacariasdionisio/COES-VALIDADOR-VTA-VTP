using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Indisponibilidades;
using log4net;
using log4net.Repository.Hierarchy;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace COES.Servicios.Aplicacion.Hidrologia
{
    /// <summary>
    /// Clases con métodos del módulo Hidrologia
    /// </summary>
    public class HidrologiaAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(HidrologiaAppServicio));

        #region Métodos Tabla ME_ORIGENLECTURA


        /// <summary>
        /// Permite listar todos los registros de la tabla ME_ORIGENLECTURA
        /// </summary>
        public List<MeOrigenlecturaDTO> ListMeOrigenlecturas()
        {
            return FactorySic.GetMeOrigenlecturaRepository().List();
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

        #region Métodos Tabla SI_TIPOINFORMACION

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_TIPOINFORMACION
        /// </summary>
        public List<SiTipoinformacionDTO> ListSiTipoinformacions()
        {
            return FactorySic.GetSiTipoinformacionRepository().List();
        }


        #endregion

        #region Métodos Tabla SI_EMPRESA
        /// <summary>
        /// Permite listar las empresas
        /// </summary>
        /// <returns></returns>
        public List<EmpresaDTO> ListarEmpresas()
        {
            return FactorySic.ObtenerEventoDao().ListarEmpresas();
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

        #endregion

        #region Métodos Tabla EQ_equipo

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
                        espacio = espacio + ".";
                    }


                    item.Equinomb = item.AREANOMB + espacio + " " + item.Equinomb;
                }

                return list.OrderBy(x => x.Equinomb).ToList();
            }

            return new List<EqEquipoDTO>();

        }

        /// <summary>
        /// Devuelve lista de equipo por id equipo
        /// </summary>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        public EqEquipoDTO GetByIdEqequipo(int equicodi)
        {
            EqEquipoDTO equipo = new EqEquipoDTO();
            equipo = FactorySic.GetEqEquipoRepository().GetById(equicodi);
            return equipo;
        }

        /// <summary>
        /// devuelve lista de equipo por familia
        /// </summary>
        /// <param name="idFamilia"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarEquiposXFamilia(int idFamilia)
        {
            List<int> idsFamilia = new List<int>();
            idsFamilia.Add(idFamilia);
            return FactorySic.GetEqEquipoRepository().ListarEquipoxFamilias(idsFamilia.ToArray());
        }

        /// <summary>
        /// devuelve equipos por cuenca empresa 
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="cuencas"></param>
        /// <param name="recursos"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarRecursosxCuenca(string empresas, string cuencas, string recursos)
        {
            return FactorySic.GetEqEquipoRepository().ListaRecursosxCuenca(empresas, cuencas, recursos);
        }

        #endregion

        #region Métodos Tabla Me_PuntosdeMedicion

        /// <summary>
        /// Devuelve lista de punto de medicion
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="idsOriglectura"></param>
        /// <param name="idsTipoptomedicion"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListarPtoMedicion(string empresas, string idsOriglectura, string idsTipoptomedicion)
        {
            if (string.IsNullOrEmpty(empresas)) empresas = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(idsOriglectura)) idsOriglectura = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(idsTipoptomedicion)) idsTipoptomedicion = ConstantesAppServicio.ParametroDefecto;
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            try
            {
                entitys = FactorySic.GetMePtomedicionRepository().GetByCriteria(empresas, idsOriglectura, idsTipoptomedicion);
            }
            catch (Exception ex)
            {
                var result = ex.Message;
            }
            return entitys;
        }

        /// <summary>
        /// Graba entidad pto de medicio
        /// </summary>
        /// <param name="ptoMedicion"></param>
        public void SavePtoMedicion(MePtomedicionDTO ptoMedicion)
        {
            try
            {
                FactorySic.GetMePtomedicionRepository().Save(ptoMedicion);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Busca pto de medicion por id
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <returns></returns>
        public MePtomedicionDTO GetByIdMePtomedicion(int ptomedicodi)
        {
            return FactorySic.GetMePtomedicionRepository().GetById(ptomedicodi);
        }
        /// <summary>
        /// Actualiza pto de medicion
        /// </summary>
        /// <param name="entity"></param>
        public void UpdatePtoMedicion(MePtomedicionDTO entity)
        {
            try
            {
                FactorySic.GetMePtomedicionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<MePtomedicionDTO> GetByIdEquipoMePtomedicion(int equipo)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            try
            {
                entitys = FactorySic.GetMePtomedicionRepository().GetByIdEquipo(equipo);
            }
            catch (Exception ex)
            {
                var result = ex.Message;
            }
            return entitys;
        }

        /// <summary>
        /// Lista pto de medicion duplicados
        /// </summary>
        /// <param name="equipo"></param>
        /// <param name="origen"></param>
        /// <param name="tipopto"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListarPtoMedicionDuplicados(int equipo, int origen, int tipopto)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            try
            {
                entitys = FactorySic.GetMePtomedicionRepository().ListarPtoDuplicado(equipo, origen, tipopto);
            }
            catch (Exception ex)
            {
                var result = ex.Message;
            }
            return entitys;
        }



        #endregion

        #region Métodos Tabla ME_MEDICION96

        /// <summary>
        ///  Actualiza la columna total 
        /// </summary>
        public void ActualizarListaEnvio96(List<MeMedicion96DTO> listaMedicion)
        {
            decimal totalDia = 0;
            decimal valorH = 0;
            try
            {
                for (var i = 0; i < listaMedicion.Count; i++)
                {
                    totalDia = 0;
                    //listaMedicion[i].ENVIOCODI = codigoEnvio;
                    for (var j = 1; j <= 96; j++)
                    {
                        valorH = (decimal)listaMedicion[i].GetType().GetProperty("H" + j.ToString()).GetValue(listaMedicion[i], null);
                        totalDia += valorH;
                    }
                    listaMedicion[i].Meditotal = totalDia;

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Permite eliminar los valores previos cargados
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void EliminarValoresCargados96(int idLectura, int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                FactorySic.GetMeMedicion96Repository().DeleteEnvioArchivo(idLectura, fechaInicio, fechaFin, idFormato, idEmpresa);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite grabar los datos cargados
        /// </summary>
        /// <param name="entitys"></param>
        public void GrabarValoresCargados96(List<MeMedicion96DTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            try
            {
                int count = 0;
                //Traer Ultimos Valores
                var lista = Convert96DTO(GetDataFormato(idEmpresa, formato, 0, 0));
                if (lista.Count > 0) // Verificar si hay cambios en el envio
                {
                    var listaCambio = new List<MeCambioenvioDTO>();
                    var listaOrigen = new List<MeCambioenvioDTO>();


                    foreach (var reg in entitys)
                    {
                        var regAnt = lista.Find(x => x.Medifecha == reg.Medifecha && x.Ptomedicodi == reg.Ptomedicodi &&
                            x.Lectcodi == reg.Lectcodi);


                        if (regAnt != null)
                        {
                            List<string> filaValores = new List<string>();
                            List<string> filaValoresOrigen = new List<string>();
                            List<string> filaCambios = new List<string>();
                            for (int i = 1; i <= 96; i++)
                            {
                                decimal? valorOrigen = (decimal?)regAnt.GetType().GetProperty("H" + i.ToString()).GetValue(regAnt, null);
                                decimal? valorModificado = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                                if (valorModificado != null)
                                    filaValores.Add(valorModificado.ToString());
                                else
                                    filaValores.Add("");
                                if (valorOrigen != null)
                                    filaValoresOrigen.Add(valorOrigen.ToString());
                                else
                                    filaValoresOrigen.Add("");
                                if (valorOrigen != valorModificado)//&& valorOrigen != null && valorModificado != null)
                                {
                                    if (count <= 100)
                                    {
                                        filaCambios.Add(i.ToString());
                                        count++;
                                    }
                                }
                            }


                            if (filaCambios.Count > 0)
                            {
                                MeCambioenvioDTO cambio = new MeCambioenvioDTO();
                                cambio.Cambenvdatos = String.Join(",", filaValores);
                                cambio.Cambenvcolvar = String.Join(",", filaCambios);
                                cambio.Cambenvfecha = (DateTime)reg.Medifecha;
                                cambio.Enviocodi = idEnvio;
                                //cambio.Formatcodi = formato.Formatcodi;
                                cambio.Ptomedicodi = reg.Ptomedicodi;
                                cambio.Tipoinfocodi = reg.Tipoinfocodi;
                                cambio.Lastuser = usuario;
                                cambio.Lastdate = DateTime.Now;
                                listaCambio.Add(cambio);
                                /// Si no ha habido cambio se graba el registro original
                                if (ListMeCambioenvios(reg.Ptomedicodi, reg.Tipoinfocodi, formato.Formatcodi, (DateTime)reg.Medifecha).Count == 0)
                                {
                                    int idEnvioPrevio = 0;
                                    var listAux = GetByCriteriaMeEnvios(idEmpresa, formato.Formatcodi, formato.FechaInicio);
                                    if (listAux.Count > 0)
                                        idEnvioPrevio = listAux.Min(x => x.Enviocodi);
                                    MeCambioenvioDTO origen = new MeCambioenvioDTO();
                                    origen.Cambenvdatos = String.Join(",", filaValoresOrigen);
                                    origen.Cambenvcolvar = "";
                                    origen.Cambenvfecha = (DateTime)reg.Medifecha;
                                    origen.Enviocodi = idEnvioPrevio;
                                    //origen.Formatcodi = formato.Formatcodi;
                                    origen.Ptomedicodi = reg.Ptomedicodi;
                                    origen.Tipoinfocodi = reg.Tipoinfocodi;
                                    origen.Lastuser = usuario;
                                    origen.Lastdate = DateTime.Now;
                                    listaOrigen.Add(origen);
                                }
                            }
                        }
                    }
                    if (listaCambio.Count > 0)
                    {//Grabar Cambios
                        GrabarCambios(listaCambio);
                        GrabarCambios(listaOrigen);
                        //si es primer reenvio grabar valores origen

                    }
                }
                //Eliminar Valores Previos
                EliminarValoresCargados96((int)formato.Lectcodi, formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                foreach (MeMedicion96DTO entity in entitys)
                {
                    FactorySic.GetMeMedicion96Repository().Save(entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Se obtiene los datos enviados de un formato de la BD.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="formato"></param>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        public List<Object> GetDataFormato(int idEmpresa, MeFormatoDTO formato, int idEnvio, int idUltimoEnvio)
        {
            List<Object> listaGenerica = new List<Object>();

            switch (formato.Formatresolucion)
            {
                case ParametrosFormato.ResolucionMes:
                case ParametrosFormato.ResolucionDia:
                case ParametrosFormato.ResolucionSemana:
                    DateTime fechaMaxEnvio = DateTime.MinValue;
                    int idMaxEnvio = ObtenerIdMaxEnvioFormato(formato.Formatcodi, idEmpresa);
                    var regMax = GetByIdMeEnvio(idMaxEnvio);
                    if (regMax != null)
                    {
                        fechaMaxEnvio = (DateTime)regMax.Enviofechaperiodo;
                    }
                    List<MeMedicion1DTO> lista1 = new List<MeMedicion1DTO>();
                    List<MeCambioenvioDTO> cambio = new List<MeCambioenvioDTO>();
                    if (idEnvio > 0)
                    {
                        lista1 = FactorySic.GetMeMedicion1Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                        cambio = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                    }
                    else if (idEnvio == 0)
                    {
                        if (formato.FechaProceso < fechaMaxEnvio)
                        {
                            if (idUltimoEnvio > 0)
                            {
                                lista1 = FactorySic.GetMeMedicion1Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                                cambio = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idUltimoEnvio, idEmpresa);
                            }
                        }
                        else
                        {
                            lista1 = FactorySic.GetMeMedicion1Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                            if (formato.Formatsecundario != 0)
                            {
                                var listaAux = FactorySic.GetMeMedicion1Repository().GetDataFormatoSecundario(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                                for (var i = formato.FechaInicio; i < formato.FechaFin; i = i.AddDays(1))
                                {
                                    var find = lista1.Find(x => x.Medifecha == i);
                                    if (find == null)
                                    {
                                        lista1 = lista1.Union(listaAux.Where(x => x.Medifecha == i)).ToList();
                                    }
                                }
                            }
                        }
                    }

                    if (idEnvio >= 0 && (lista1.Count > 0))
                    {
                        if (cambio.Count > 0)
                        {
                            foreach (var reg in cambio)
                            {
                                var find = lista1.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi &&
                                    x.Medifecha == reg.Cambenvfecha);
                                if (find != null)
                                {
                                    decimal dato;
                                    decimal? numero = null;
                                    if (decimal.TryParse(reg.Cambenvdatos, out dato))
                                        numero = dato;
                                    find.H1 = numero;
                                }
                            }
                        }
                    }
                    foreach (var reg in lista1)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;

                case ParametrosFormato.ResolucionHora:
                    List<MeMedicion24DTO> lista24 = FactorySic.GetMeMedicion24Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                    if (idEnvio != 0)
                    {
                        var lista = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                        if (lista.Count > 0)
                        {
                            foreach (var reg in lista)
                            {
                                var find = lista24.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi &&
                                    x.Medifecha == reg.Cambenvfecha);
                                if (find != null)
                                {
                                    var fila = reg.Cambenvdatos.Split(',');
                                    for (var i = 0; i < 24; i++)
                                    {
                                        decimal dato;
                                        decimal? numero = null;
                                        if (decimal.TryParse(fila[i], out dato))
                                            numero = dato;
                                        find.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(find, numero);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (formato.Formatsecundario != 0 && lista24.Count == 0)
                        {
                            lista24 = FactorySic.GetMeMedicion24Repository().GetDataFormatoSecundario(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                        }
                    }
                    foreach (var reg in lista24)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;
                case ParametrosFormato.ResolucionMediaHora:
                    List<MeMedicion48DTO> lista48 = FactorySic.GetMeMedicion48Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa.ToString(), formato.FechaInicio, formato.FechaFin);
                    if (idEnvio != 0)
                    {
                        var lista = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                        if (lista.Count > 0)
                        {
                            foreach (var reg in lista)
                            {
                                var find = lista48.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi &&
                                    x.Medifecha == reg.Cambenvfecha);
                                if (find != null)
                                {
                                    var fila = reg.Cambenvdatos.Split(',');
                                    for (var i = 0; i < 48; i++)
                                    {
                                        decimal dato;
                                        decimal? numero = null;
                                        if (decimal.TryParse(fila[i], out dato))
                                            numero = dato;
                                        find.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(find, numero);
                                    }

                                }
                            }
                        }
                    }

                    foreach (var reg in lista48)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;

                case ParametrosFormato.ResolucionCuartoHora:
                    List<MeMedicion96DTO> lista96 = FactorySic.GetMeMedicion96Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                    if (idEnvio != 0)
                    {
                        var lista = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                        if (lista.Count > 0)
                        {
                            foreach (var reg in lista)
                            {
                                var find = lista96.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi &&
                                    x.Medifecha == reg.Cambenvfecha);
                                if (find != null)
                                {
                                    var fila = reg.Cambenvdatos.Split(',');
                                    for (var i = 0; i < 96; i++)
                                    {
                                        decimal dato;
                                        decimal? numero = null;
                                        if (decimal.TryParse(fila[i], out dato))
                                            numero = dato;
                                        find.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(find, numero);
                                    }

                                }
                            }
                        }
                    }

                    foreach (var reg in lista96)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;
            }
            return listaGenerica;
        }

        /// <summary>
        /// Permite obtener el reporte  html de los datos cargados por el agente
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ObtenerResumenCargaDatos(int idEmpresa, DateTime fechaInicio, DateTime fechaFin, MeFormatoDTO formato, List<MeHojaptomedDTO> puntos, List<MeHeadcolumnDTO> cabeceras)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            List<Object> listaGenerica = new List<Object>();
            switch (formato.Formatresolucion)
            {
                case 60 * 24:
                    List<MeMedicion1DTO> lista1 = FactorySic.GetMeMedicion1Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, fechaInicio, fechaFin);
                    foreach (var reg in lista1)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;

                case 60:
                    List<MeMedicion24DTO> lista24 = FactorySic.GetMeMedicion24Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, fechaInicio, fechaFin);
                    foreach (var reg in lista24)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;
                case 30:
                    List<MeMedicion48DTO> lista48 = FactorySic.GetMeMedicion48Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa.ToString(), fechaInicio, fechaFin);
                    foreach (var reg in lista48)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;
                case 15:
                    List<MeMedicion96DTO> lista96 = FactorySic.GetMeMedicion96Repository().GetEnvioArchivo(formato.Formatcodi, idEmpresa, fechaInicio, fechaFin);
                    foreach (var reg in lista96)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;
            }
            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<table border='1' class='pretty tabla-icono cell-border' cellspacing='0' width='100%' id='tabla'>");
            ///Cabecera de Reporte
            strHtml.Append("<thead>");
            //strHtml.Append("<tr><th></th>");
            ///// Agrupacion de Nombres de Equipo
            //foreach(var reg in cabeceras)
            //    strHtml.Append("<th colspan='" + reg.Headlen + "'>" + reg.Headnombre +"</th>");
            ///// FIn de agrupacion de Nombre de Equipos
            //strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<th>FECHA HORA</th>");
            foreach (var reg in puntos)
                strHtml.Append("<th>" + reg.Tipoptomedinomb + " (" + reg.Tipoinfoabrev + ")" + "</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            /// Fin de cabecera de Reporte
            strHtml.Append("<tbody>");
            int horizonte = formato.Formathorizonte * 60 * 24;
            int resolucion = (int)formato.Formatresolucion;
            int nBloques = 60 * 24 / resolucion;
            if (formato.Formatresolucion == 60 * 24)
                fechaInicio = fechaInicio.AddDays(-1);
            if (listaGenerica.Count > 0)
            {
                for (int i = 0; i < formato.Formathorizonte; i++)
                    for (int k = 1; k <= nBloques; k++)
                    {
                        var fechaMin = ((fechaInicio.AddMinutes(k * resolucion + i * 60 * 24))).ToString(ConstantesBase.FormatoFechaHora);
                        strHtml.Append("<tr>");
                        strHtml.Append(string.Format("<td>{0}</td>", fechaMin));
                        foreach (var p in listaGenerica)
                        {
                            decimal valor = (decimal)p.GetType().GetProperty("H" + k).GetValue(p, null);
                            DateTime fecha = (DateTime)p.GetType().GetProperty("Medifecha").GetValue(p, null);
                            if (fecha == fechaInicio.AddMinutes(i * 60 * 24))
                                strHtml.Append(string.Format("<td>{0}</td>", valor.ToString("N", nfi)));
                        }

                        strHtml.Append("</tr>");
                    }

            }
            else
            {
                strHtml.Append("<td  style='text-align:center'>No existen registros.</td>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            return strHtml.ToString();
        }


        /// <summary>
        /// Obtiene Reporte en codigo HTLM de Historico de Hidrologia
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="formato"></param>
        /// <returns></returns>
        public string ObtenerReporteHidrologia(string idsEmpresa, string idsCuenca, string idsFamilia, string idsPtoMedicion, DateTime fechaInicio, DateTime fechaFin, int idLectura, int opcion, int rbDetalleRpte, int unidad, int numeroMedicion)
        {// rbDetalleRpte(0: Horas, 1: Diario, 2,3: Semanal, 4: Mensual, 5: Anual)
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            string strHtml = string.Empty;
            if (string.IsNullOrEmpty(idsEmpresa)) idsEmpresa = ConstantesAppServicio.ParametroNoExiste;
            List<Object> listaGenerica = new List<Object>();
            List<MeMedicion24DTO> lista24 = new List<MeMedicion24DTO>();
            var nroLectura = GetByIdMeLectura(idLectura);
            switch (numeroMedicion)
            {
                case ConstantesAppServicio.LectNroME24:
                    switch (nroLectura.Lectperiodo)
                    {
                        case ConstHidrologia.Horas:// 
                            if (rbDetalleRpte == ConstantesRbDetalleRpte.Horas)
                                lista24 = FactorySic.GetMeMedicion24Repository().GetHidrologia(idLectura, ConstantesAppServicio.IdOrigenLectura,
                                    idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaInicio, idsPtoMedicion);
                            else
                                lista24 = FactorySic.GetMeMedicion24Repository().GetHidrologia(idLectura, ConstantesAppServicio.IdOrigenLectura,
                                    idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion);

                            List<MeMedicion24DTO> listaCabeceraM24 = lista24.GroupBy(x => new { x.Ptomedicodi, x.Cuenca, x.Emprnomb, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tipoptomedinomb, x.Equinomb, x.Famcodi, x.Famabrev })
                                .Select(y => new MeMedicion24DTO()
                                {
                                    Cuenca = y.Key.Cuenca,
                                    Emprnomb = y.Key.Emprnomb,
                                    Famabrev = y.Key.Famabrev,
                                    Ptomedicodi = y.Key.Ptomedicodi,
                                    Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                    Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                    Tipoptomedinomb = y.Key.Tipoptomedinomb,
                                    Equinomb = y.Key.Equinomb,
                                    Famcodi = y.Key.Famcodi
                                }
                                ).ToList();

                            switch (rbDetalleRpte)
                            {
                                case ConstantesRbDetalleRpte.Horas: //Rpte detallado horas   
                                    strHtml = GeneraViewHidrologiaMed24_2(lista24, (int)nroLectura.Lectnro, fechaInicio);
                                    break;
                                case ConstantesRbDetalleRpte.Diario: //Rpte detallado diario                                      
                                    if (unidad == ConstHidrologia.Caudal)
                                    { //Caudal
                                        var listaTemp = GenerarListaDetalladaMed24Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca,
                                            idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte);
                                        strHtml = GeneraViewHidrologiaMed24(listaTemp, listaCabeceraM24, (int)nroLectura.Lectnro, fechaInicio, rbDetalleRpte);
                                    }
                                    else
                                        strHtml = GeneraViewHidrologiaMed24(GenerarListaMed24Hmaximo(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa,
                                            idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte), listaCabeceraM24, (int)nroLectura.Lectnro, fechaInicio, rbDetalleRpte);
                                    break;
                                case ConstantesRbDetalleRpte.SemanalProgramado:
                                case ConstantesRbDetalleRpte.SemanalCronologico://Rpte detallado semanal programado, cronologico
                                    if (unidad == ConstHidrologia.Caudal) //Cuadal
                                        strHtml = GeneraViewHidrologiaMed24(GenerarListaDetalladaMed24Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca,
                                            idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte), listaCabeceraM24, (int)nroLectura.Lectnro, fechaInicio, rbDetalleRpte);
                                    else
                                        strHtml = GeneraViewHidrologiaMed24(GenerarListaMed24Hmaximo(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca,
                                                idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte),
                                            listaCabeceraM24, (int)nroLectura.Lectnro, fechaInicio, rbDetalleRpte);
                                    break;
                                case ConstantesRbDetalleRpte.Mensual: // Rpte detallado mensual
                                    if (unidad == ConstHidrologia.Caudal) //Caudal
                                        strHtml = GeneraViewHidrologiaMed24(GenerarListaDetalladaMed24Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia,
                                                fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte),
                                            listaCabeceraM24, (int)nroLectura.Lectnro, fechaInicio, rbDetalleRpte);
                                    else
                                        strHtml = GeneraViewHidrologiaMed24(GenerarListaMed24Hmaximo(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia,
                                                fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte),
                                            listaCabeceraM24, (int)nroLectura.Lectnro, fechaInicio, rbDetalleRpte);
                                    break;
                                case ConstantesRbDetalleRpte.Anual: //Rpte detallado anual
                                    if (unidad == ConstHidrologia.Caudal) //Caudal
                                        strHtml = GeneraViewHidrologiaMed24(GenerarListaMed24PromAnual(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion),
                                            listaCabeceraM24, (int)nroLectura.Lectnro, fechaInicio, rbDetalleRpte);
                                    else
                                        strHtml = GeneraViewHidrologiaMed24(GenerarListaMed24AnualHmaximo(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion),
                                            listaCabeceraM24, (int)nroLectura.Lectnro, fechaInicio, rbDetalleRpte);
                                    break;
                            }
                            break;
                    }
                    break;
                case ConstantesAppServicio.LectNroME1:
                    List<MeMedicion1DTO> listaMed1 = FactorySic.GetMeMedicion1Repository().GetHidrologia(idLectura, ConstantesAppServicio.IdOrigenLectura,
                        idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion);
                    List<MeMedicion1DTO> listaCabeceraPM1 = listaMed1.GroupBy(x => new { x.Ptomedicodi, x.Cuenca, x.Emprnomb, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tipoptomedinomb, x.Equinomb, x.Famcodi, x.Famabrev })
                        .Select(y => new MeMedicion1DTO()
                        {
                            Cuenca = y.Key.Cuenca,
                            Emprnomb = y.Key.Emprnomb,
                            Famabrev = y.Key.Famabrev,
                            Ptomedicodi = y.Key.Ptomedicodi,
                            Ptomedibarranomb = y.Key.Ptomedibarranomb,
                            Tipoinfoabrev = y.Key.Tipoinfoabrev,
                            Tipoptomedinomb = y.Key.Tipoptomedinomb,
                            Equinomb = y.Key.Equinomb,
                            Famcodi = y.Key.Famcodi
                        }
                        ).ToList();


                    switch (nroLectura.Lectperiodo)
                    {
                        case ConstHidrologia.Diario:
                            switch (rbDetalleRpte)
                            {
                                case ConstantesRbDetalleRpte.Diario: //Rpte detallado diario
                                    strHtml = GeneraViewHidrologiaMed1(listaMed1, listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    break;
                                case ConstantesRbDetalleRpte.SemanalProgramado:
                                case ConstantesRbDetalleRpte.SemanalCronologico://Rpte detallado semanal programado                                
                                    if (unidad == ConstHidrologia.Caudal)
                                        strHtml = GeneraViewHidrologiaMed1(GenerarListaDetalladaMed1Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    else
                                        strHtml = GeneraViewHidrologiaMed1(GenerarListaDetalladaMed1Hmax(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    break;
                                case ConstantesRbDetalleRpte.Mensual: // Rpte detallado mensual
                                    if (unidad == ConstHidrologia.Caudal)
                                        strHtml = GeneraViewHidrologiaMed1(GenerarListaDetalladaMed1Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    else
                                        strHtml = GeneraViewHidrologiaMed1(GenerarListaDetalladaMed1Hmax(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    break;
                                case ConstantesRbDetalleRpte.Anual: //Rpte detallado anual
                                    if (unidad == ConstHidrologia.Caudal)
                                        strHtml = GeneraViewHidrologiaMed1(GenerarListaMed1PromAnual(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    else
                                        strHtml = GeneraViewHidrologiaMed1(GenerarListaMed1AnualHmaximo(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    break;
                            }
                            break;
                        case ConstHidrologia.SemanalProg:
                        case ConstHidrologia.Semanal:
                            switch (rbDetalleRpte)
                            {

                                case ConstantesRbDetalleRpte.SemanalProgramado: //Rpte detallado semanal programado                                                                    
                                    strHtml = GeneraViewHidrologiaMed1(listaMed1, listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    break;
                                case ConstantesRbDetalleRpte.Mensual: // Rpte detallado mensual
                                    if (unidad == ConstHidrologia.Caudal)
                                        strHtml = GeneraViewHidrologiaMed1(GenerarListaDetalladaMed1Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa
                                                , idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    else
                                        strHtml = GeneraViewHidrologiaMed1(GenerarListaDetalladaMed1Hmax(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa
                                                , idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    break;
                                case ConstantesRbDetalleRpte.Anual: //Rpte detallado anual
                                    if (unidad == ConstHidrologia.Caudal)
                                        strHtml = GeneraViewHidrologiaMed1(GenerarListaMed1PromAnual(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    else
                                        strHtml = GeneraViewHidrologiaMed1(GenerarListaMed1AnualHmaximo(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    break;
                            }
                            break;
                        case ConstHidrologia.Mensual:
                            switch (rbDetalleRpte)
                            {

                                case ConstantesRbDetalleRpte.Mensual: // Rpte detallado mensual
                                    strHtml = GeneraViewHidrologiaMed1(listaMed1, listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    break;
                                case ConstantesRbDetalleRpte.Anual: //Rpte detallado anual
                                    if (unidad == ConstHidrologia.Caudal)
                                        strHtml = GeneraViewHidrologiaMed1(GenerarListaMed1PromAnual(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    else
                                        strHtml = GeneraViewHidrologiaMed1(GenerarListaMed1AnualHmaximo(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    break;
                            }
                            break;
                    }
                    break;
            }
            return strHtml;
        }

        public List<MePtomedicionDTO> ObtenerDatosHidrologia(DateTime dtFechaInicio, DateTime dtFechaFin)
        {
            return FactorySic.GetMePtomedicionRepository().ObtenerDatosHidrologia(dtFechaInicio, dtFechaFin);
        }

        public List<MePtomedicionDTO> LeerPtoMedicionHidrologia()
        {
            return FactorySic.GetMePtomedicionRepository().LeerPtoMedicionHidrologia();
        }

        public List<MeMedicion48DTO> LeerMedidoresHidrologia(DateTime fecha)
        {
            return FactorySic.GetMeMedicion48Repository().LeerMedidoresHidrologia(fecha);
        }

        /// <summary>
        /// Obtiene el reporte en cofigo Html de Hidrologia en tiempo real
        /// </summary>
        /// <param name="?"></param>
        /// <param name="?"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public string ObtenerReporteHidrologiaTiempoReal(string idsEmpresa, DateTime fechaIni, DateTime fechaFin, string idsTipoPtoMed, int tipoReporte)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            string strHtml = string.Empty;
            if (string.IsNullOrEmpty(idsEmpresa)) idsEmpresa = ConstantesAppServicio.ParametroNoExiste;
            if (string.IsNullOrEmpty(idsTipoPtoMed)) idsTipoPtoMed = ConstantesAppServicio.ParametroNoExiste;
            List<MeMedicion24DTO> lista24 = new List<MeMedicion24DTO>();

            lista24 = ListaMed24HidrologiaTiempoReal(tipoReporte, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, fechaIni, fechaFin, idsTipoPtoMed);
            if (lista24.Count > 0)
            {
                List<MeReporptomedDTO> listaCabecera = ListarEncabezadoMeReporptomeds(tipoReporte, idsEmpresa, idsTipoPtoMed);
                strHtml = GeneraViewHidrologiaMed24TiempoReal(lista24, listaCabecera, fechaIni, tipoReporte); // *************************
            }
            else
            {
                strHtml = "<p>no existen registros.</p>";
            }
            return strHtml;
        }

        /// <summary>
        /// Obtiene el reporte en cofigo Html de Hidrologia de caudales y volumne
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <param name="unidad"></param>
        /// <param name="idLectura"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <returns></returns>
        public string ObtenerReporteHidrologiaQnVolTipo2(string idsEmpresa, DateTime fechaInicio, DateTime fechaFin, int rbDetalleRpte, int unidad,
            int idLectura, string idsCuenca, string idsFamilia, string idsPtoMedicion)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            string strHtml = string.Empty;
            if (string.IsNullOrEmpty(idsEmpresa)) idsEmpresa = ConstantesAppServicio.ParametroNoExiste;
            List<Object> listaGenerica = new List<Object>();
            List<MeMedicion24DTO> listaM24 = new List<MeMedicion24DTO>();
            List<MeMedicion1DTO> listaM1 = new List<MeMedicion1DTO>();

            var entity = GetByIdMeLectura(idLectura);

            switch (entity.Lectnro)
            {
                case ConstantesAppServicio.LectNroME24: // Medicion24
                    switch (entity.Lectperiodo)
                    {
                        case ConstHidrologia.Horas:
                            listaM24 = FactorySic.GetMeMedicion24Repository().GetHidrologia(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca,
                                idsFamilia, fechaInicio, fechaFin, idsPtoMedicion);

                            List<MeMedicion24DTO> listaCabeceraM24 = listaM24.GroupBy(x => new { x.Ptomedicodi, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tipoptomedinomb, x.Equinomb, x.Famcodi, x.Emprnomb })
                                .Select(y => new MeMedicion24DTO()
                                {
                                    Ptomedicodi = y.Key.Ptomedicodi,
                                    Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                    Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                    Tipoptomedinomb = y.Key.Tipoptomedinomb,
                                    Equinomb = y.Key.Equinomb,
                                    Famcodi = y.Key.Famcodi,
                                    Emprnomb = y.Key.Emprnomb
                                }
                                ).ToList();
                            switch (rbDetalleRpte)
                            {
                                case ConstantesrbTipoRpteQnVol.Semanal: // Semanal
                                    DateTime fechaIni = DateTime.MinValue;
                                    if (unidad == ConstHidrologia.Caudal) //Caudal                                       
                                    {
                                        fechaIni = COES.Base.Tools.Util.GenerarFecha(fechaInicio.AddYears(-1).Year, COES.Base.Tools.Util.GenerarNroSemana(fechaInicio, FirstDayOfWeek.Saturday), 0);

                                        strHtml = GeneraViewHidrologiaMed24QnVolTipo2(GenerarListaDetalladaMed24Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaIni.AddDays(6), idsPtoMedicion, rbDetalleRpte + 2),
                                                                                      GenerarListaDetalladaMed24Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte + 2),
                                                                                                                        listaCabeceraM24, fechaInicio, fechaFin, rbDetalleRpte, unidad);
                                    }
                                    else
                                        strHtml = GeneraViewHidrologiaMed24QnVolTipo2(GenerarListaMed24Hmaximo(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaIni.AddDays(6), idsPtoMedicion, rbDetalleRpte + 2),
                                                                                      GenerarListaMed24Hmaximo(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte + 2),
                                                                                      listaCabeceraM24, fechaInicio, fechaFin, rbDetalleRpte, unidad);
                                    break;
                                case ConstantesrbTipoRpteQnVol.Mensual: //Mensual
                                    if (unidad == ConstHidrologia.Caudal) //Caudal

                                        strHtml = GeneraViewHidrologiaMed24QnVolTipo2(GenerarListaDetalladaMed24Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio.AddYears(-1), fechaFin.AddYears(-1), idsPtoMedicion, rbDetalleRpte + 2),
                                                                                      GenerarListaDetalladaMed24Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte + 2),
                                                                                                                        listaCabeceraM24, fechaInicio, fechaFin, rbDetalleRpte, unidad);
                                    else
                                        strHtml = GeneraViewHidrologiaMed24QnVolTipo2(GenerarListaMed24Hmaximo(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio.AddYears(-1), fechaFin.AddYears(-1), idsPtoMedicion, rbDetalleRpte + 2),
                                                                                      GenerarListaMed24Hmaximo(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte + 2),
                                                                                      listaCabeceraM24, fechaInicio, fechaFin, rbDetalleRpte, unidad);
                                    break;
                                case ConstantesrbTipoRpteQnVol.Anual: //Anual
                                    if (unidad == ConstHidrologia.Caudal) //Caudal

                                        strHtml = GeneraViewHidrologiaMed24QnVolTipo2Anual(GenerarListaDetalladaMed24Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte + 1),
                                                                                                                              listaCabeceraM24, fechaInicio, fechaFin, unidad);
                                    else
                                        strHtml = GeneraViewHidrologiaMed24QnVolTipo2Anual(GenerarListaMed24Hmaximo(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte + 1),
                                                                                                                    listaCabeceraM24, fechaInicio, fechaFin, unidad);
                                    break;
                            }
                            break;
                    }

                    break;
                case ConstantesAppServicio.LectNroME1: //Medicion1
                    List<MeMedicion1DTO> listaMed1 = FactorySic.GetMeMedicion1Repository().GetHidrologia(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa,
                        idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion);
                    List<MeMedicion1DTO> listaCabeceraPM1 = listaMed1.GroupBy(x => new { x.Ptomedicodi, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tipoptomedinomb, x.Equinomb, x.Famcodi })
                        .Select(y => new MeMedicion1DTO()
                        {
                            Ptomedicodi = y.Key.Ptomedicodi,
                            Ptomedibarranomb = y.Key.Ptomedibarranomb,
                            Tipoinfoabrev = y.Key.Tipoinfoabrev,
                            Tipoptomedinomb = y.Key.Tipoptomedinomb,
                            Equinomb = y.Key.Equinomb,
                            Famcodi = y.Key.Famcodi
                        }
                        ).ToList();
                    switch (entity.Lectperiodo)
                    {
                        case ConstHidrologia.Diario:
                            switch (rbDetalleRpte)
                            {
                                case ConstantesrbTipoRpteQnVol.Semanal: // Semanal
                                    DateTime fechaIni = DateTime.MinValue;
                                    if (unidad == ConstHidrologia.Caudal)
                                    {
                                        fechaIni = COES.Base.Tools.Util.GenerarFecha(fechaInicio.AddYears(-1).Year, COES.Base.Tools.Util.GenerarNroSemana(fechaInicio, FirstDayOfWeek.Saturday), 0);
                                        strHtml = GeneraViewHidrologiaMed1QnVolTipo2(GenerarListaDetalladaMed1Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa
                                            , idsCuenca, idsFamilia, fechaIni, fechaIni.AddDays(6), idsPtoMedicion, rbDetalleRpte + 2),
                                            GenerarListaDetalladaMed1Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa
                                            , idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte + 2),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte, unidad);
                                    }
                                    else
                                        strHtml = GeneraViewHidrologiaMed1QnVolTipo2(GenerarListaDetalladaMed1Hmax(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaIni,
                                            fechaIni.AddDays(6), idsPtoMedicion, rbDetalleRpte + 2), GenerarListaDetalladaMed1Hmax(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa,
                                            idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte + 2),
                                                                                     listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte, unidad);
                                    break;
                                case ConstantesrbTipoRpteQnVol.Mensual: //Mensual
                                    if (unidad == ConstHidrologia.Caudal)
                                        strHtml = GeneraViewHidrologiaMed1QnVolTipo2(GenerarListaDetalladaMed1Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia,
                                            fechaInicio.AddYears(-1), fechaFin.AddYears(-1), idsPtoMedicion, rbDetalleRpte + 2), GenerarListaDetalladaMed1Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura,
                                            idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte + 2),
                                                                                     listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte, unidad);
                                    else
                                        strHtml = GeneraViewHidrologiaMed1QnVolTipo2(GenerarListaDetalladaMed1Hmax(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio.AddYears(-1), fechaFin.AddYears(-1), idsPtoMedicion, rbDetalleRpte + 2),
                                                                                     GenerarListaDetalladaMed1Hmax(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte + 2),
                                                                                     listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte, unidad);
                                    break;
                                case ConstantesrbTipoRpteQnVol.Anual: //Anual
                                    if (unidad == ConstHidrologia.Caudal) //volumen

                                        strHtml = GeneraViewHidrologiaMed1QnVolTipo2Anual(GenerarListaDetalladaMed1Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte + 1),
                                                                                                                              listaCabeceraPM1, fechaInicio, fechaFin, unidad);
                                    else
                                        strHtml = GeneraViewHidrologiaMed1QnVolTipo2Anual(GenerarListaDetalladaMed1Hmax(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte + 1),
                                                                                                                    listaCabeceraPM1, fechaInicio, fechaFin, unidad);
                                    break;
                            }
                            break;
                        case ConstHidrologia.SemanalProg:
                        case ConstHidrologia.Semanal: // Semanal
                            switch (rbDetalleRpte)
                            {
                                case ConstantesrbTipoRpteQnVol.Semanal: // Semanal
                                    DateTime fechaIni = DateTime.MinValue;
                                    fechaIni = COES.Base.Tools.Util.GenerarFecha(fechaInicio.AddYears(-1).Year, COES.Base.Tools.Util.GenerarNroSemana(fechaInicio, FirstDayOfWeek.Saturday), 0);
                                    List<MeMedicion1DTO> listaMed2 = FactorySic.GetMeMedicion1Repository().GetHidrologia(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa,
                                        idsCuenca, idsFamilia, fechaIni, fechaIni.AddDays(6), idsPtoMedicion);
                                    strHtml = GeneraViewHidrologiaMed1QnVolTipo2(listaMed1, listaMed2, listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte, unidad);
                                    break;
                                case ConstantesrbTipoRpteQnVol.Mensual: //Mensual
                                    if (unidad == ConstHidrologia.Caudal)
                                        strHtml = GeneraViewHidrologiaMed1QnVolTipo2(GenerarListaDetalladaMed1Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio.AddYears(-1), fechaFin.AddYears(-1), idsPtoMedicion, rbDetalleRpte + 2),
                                                                                     GenerarListaDetalladaMed1Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte + 2),
                                                                                     listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte, unidad);
                                    else
                                        strHtml = GeneraViewHidrologiaMed1QnVolTipo2(GenerarListaDetalladaMed1Hmax(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio.AddYears(-1), fechaFin.AddYears(-1), idsPtoMedicion, rbDetalleRpte + 2),
                                                                                     GenerarListaDetalladaMed1Hmax(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte + 2),
                                                                                     listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte, unidad);
                                    break;
                                case ConstantesrbTipoRpteQnVol.Anual: //Anual
                                    if (unidad == ConstHidrologia.Caudal) //volumen

                                        strHtml = GeneraViewHidrologiaMed1QnVolTipo2Anual(GenerarListaDetalladaMed1Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte + 1),
                                                                                                                              listaCabeceraPM1, fechaInicio, fechaFin, unidad);
                                    else
                                        strHtml = GeneraViewHidrologiaMed1QnVolTipo2Anual(GenerarListaDetalladaMed1Hmax(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte + 1),
                                                                                                                    listaCabeceraPM1, fechaInicio, fechaFin, unidad);
                                    break;

                            }
                            break;
                        case ConstHidrologia.Mensual: //Mensual
                            switch (rbDetalleRpte)
                            {
                                case ConstantesrbTipoRpteQnVol.Semanal: // Semanal

                                    break;
                                case ConstantesrbTipoRpteQnVol.Mensual: //Mensual
                                    List<MeMedicion1DTO> listaMed2 = FactorySic.GetMeMedicion1Repository().GetHidrologia(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa,
                                        idsCuenca, idsFamilia, fechaInicio.AddYears(-1), fechaFin.AddYears(-1), idsPtoMedicion);
                                    strHtml = GeneraViewHidrologiaMed1QnVolTipo2(listaMed1, listaMed2, listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte, unidad);
                                    break;
                                case ConstantesrbTipoRpteQnVol.Anual: //Anual
                                    if (unidad == ConstHidrologia.Caudal) //volumen

                                        strHtml = GeneraViewHidrologiaMed1QnVolTipo2Anual(GenerarListaDetalladaMed1Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte + 1),
                                                                                                                              listaCabeceraPM1, fechaInicio, fechaFin, unidad);
                                    else
                                        strHtml = GeneraViewHidrologiaMed1QnVolTipo2Anual(GenerarListaDetalladaMed1Hmax(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte + 1),
                                                                                                                    listaCabeceraPM1, fechaInicio, fechaFin, unidad);
                                    break;
                            }
                            break;
                    }
                    break;
            }
            return strHtml;
        }

        /// <summary>
        /// Obtiene el reporte en cofigo Html de Hidrologia Descarga de Lagunas ó Vertimientos de Embalses
        /// </summary>
        /// <param name="?"></param>
        /// <param name="?"></param>
        /// <param name="?"></param>
        /// <returns></returns>        
        public string ObtenerReporteDescargaVertimiento(int idFormato, string idsEmpresa, DateTime fechaIni, DateTime fechaFin, int nroPagina, int pageSize)
        {

            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            string strHtml = string.Empty;
            if (string.IsNullOrEmpty(idsEmpresa)) idsEmpresa = ConstantesAppServicio.ParametroNoExiste;
            List<MeMedicionxintervaloDTO> lista = new List<MeMedicionxintervaloDTO>();
            lista = ListaMedIntervaloDescargaVertPag(idFormato, idsEmpresa, fechaIni, fechaFin, nroPagina, pageSize);

            //if (lista.Count > 0)
            //{
            strHtml = GeneraViewHidrologiaDescargaVert(lista, idFormato);
            //}
            //else
            //{
            //    strHtml = "<p>no existen registros.</p>";
            //}
            return strHtml;
        }

        /// <summary>
        /// Genera la cabecera de los reportes de hidrologia
        /// </summary>
        /// <param name="listaGenerica"></param>
        /// <param name="listaCabeceraM24"></param>
        /// <param name="strHtml"></param>
        private void GeneraHtmlCabecera24(List<MeMedicion24DTO> listaCabeceraM24, ref StringBuilder strHtml)
        {
            strHtml.Append("<thead>");
            strHtml.Append("<tr><th> PUNTO DE MEDICIÓN</th>");
            foreach (var reg in listaCabeceraM24)
            {
                strHtml.Append("<th >" + reg.Ptomedibarranomb + "</th >");
            }
            strHtml.Append("</tr>");
            strHtml.Append("<tr><th>CÓDIGO</th>");
            foreach (var reg in listaCabeceraM24)
            {
                strHtml.Append("<th >" + reg.Ptomedicodi + "</th >");
            }
            strHtml.Append("</tr>");


            strHtml.Append("<tr><th>CUENCA</th>");
            foreach (var reg in listaCabeceraM24)
            {
                string valor = String.Empty;
                valor = (string)reg.GetType().GetProperty("Cuenca").GetValue(reg, null);
                strHtml.Append("<th >" + valor + "</th>");
            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr><th>EMPRESA</th>");
            foreach (var reg in listaCabeceraM24)
            {
                string valor = String.Empty;
                //int iEmprnomb = (int)reg.GetType().GetProperty("Emprnomb").GetValue(reg, null);
                valor = (string)reg.GetType().GetProperty("Emprnomb").GetValue(reg, null);
                strHtml.Append("<th >" + valor + "</th>");
            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr><th>INSTALACION</th>");
            foreach (var reg in listaCabeceraM24)
            {
                string valor = String.Empty;
                //int iEmprnomb = (int)reg.GetType().GetProperty("Emprnomb").GetValue(reg, null);
                valor = (string)reg.GetType().GetProperty("Famabrev").GetValue(reg, null);
                strHtml.Append("<th >" + valor + "</th>");
            }
            strHtml.Append("</tr>");


            strHtml.Append("<tr><th  >EQUIPO</th>");
            foreach (var p in listaCabeceraM24)
            {
                string valor = String.Empty;
                int ifamcodi = (int)p.GetType().GetProperty("Famcodi").GetValue(p, null);
                switch (ifamcodi)
                {
                    case ConstantesAppServicio.EstacionHidrologica: //Estacion Hidrologica
                        valor = (string)p.GetType().GetProperty("Ptomedibarranomb").GetValue(p, null);
                        break;
                    default:
                        valor = (string)p.GetType().GetProperty("Equinomb").GetValue(p, null);
                        break;

                }
                strHtml.Append("<th >" + valor + "</th>");
            }
            strHtml.Append("</tr>");



            strHtml.Append("<tr><th  >TIPO</th>");
            foreach (var p in listaCabeceraM24)
            {
                string valor3 = (string)p.GetType().GetProperty("Tipoptomedinomb").GetValue(p, null);
                strHtml.Append("<th style='font-size: 8px;'>" + valor3 + "</th>");
            }
            strHtml.Append("</tr>");
            strHtml.Append("<tr><th >FECHA-HORA / UNIDAD</th>");
            foreach (var p in listaCabeceraM24)
            {
                string valor = (string)p.GetType().GetProperty("Tipoinfoabrev").GetValue(p, null);
                strHtml.Append("<th >" + valor + "</th>");
            }
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

        }


        private void GeneraHtmlCabecera24_2(List<MeReporptomedDTO> listaCabeceraM24, ref StringBuilder strHtml)
        {
            strHtml.Append("<thead>");
            strHtml.Append("<tr><th> PUNTO DE MEDICIÓN</th>");
            foreach (var reg in listaCabeceraM24)
            {
                strHtml.Append("<th >" + reg.Ptomedibarranomb + "</th >");
            }
            strHtml.Append("</tr>");
            strHtml.Append("<tr>CÓDIGO<th></th>");
            foreach (var reg in listaCabeceraM24)
            {
                strHtml.Append("<th >" + reg.Ptomedicodi + "</th >");
            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr><th>CUENCA</th>");
            foreach (var reg in listaCabeceraM24)
            {
                strHtml.Append("<th >" + reg.Cuenca + "</th>");
            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr><th>EMPRESA</th>");
            foreach (var reg in listaCabeceraM24)
            {
                strHtml.Append("<th >" + reg.Emprnomb + "</th>");
            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr><th  >PTO MEDICIÓN/EMBALSE/CENTRAL</th>");
            foreach (var p in listaCabeceraM24)
            {
                string valor = String.Empty;
                //int ifamcodi = (int)p.GetType().GetProperty("Famcodi").GetValue(p, null);
                //switch (ifamcodi)
                //{
                //    case ConstantesAppServicio.EstacionHidrologica: //Estacion Hidrologica
                strHtml.Append("<th >" + p.Ptomedielenomb + "</th>");
                //        break;
                //    default:

                //        strHtml.Append("<th >" + p.Equinomb + "</th>");
                //        break;

                //}

            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr><th  >DESCRIPCIÓN</th>");
            foreach (var p in listaCabeceraM24)
            {
                strHtml.Append("<th style='font-size: 9px;'>" + p.Ptomedidesc + "</th>");
            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr><th >FECHA-HORA / UNIDAD</th>");
            foreach (var p in listaCabeceraM24)
            {
                strHtml.Append("<th >" + p.Tipoinfoabrev + "</th>");
            }
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

        }

        /////////////////this?
        private void GeneraHtmlCabecera1(List<MeMedicion1DTO> listaCabecera, ref StringBuilder strHtml)
        {
            strHtml.Append("<thead>");
            strHtml.Append("<tr><th> PUNTO DE MEDICIÓN</th>");
            foreach (var reg in listaCabecera)
            {
                strHtml.Append("<th >" + reg.Ptomedibarranomb + "</th >");
            }
            strHtml.Append("</tr>");
            strHtml.Append("<tr><th>CÓDIGO</th>");
            foreach (var reg in listaCabecera)
            {
                strHtml.Append("<th >" + reg.Ptomedicodi + "</th >");
            }
            strHtml.Append("</tr>");
            ///////Aqúi


            strHtml.Append("<tr><th>CUENCA</th>");
            foreach (var reg in listaCabecera)
            {
                string valor = String.Empty;
                valor = (string)reg.GetType().GetProperty("Cuenca").GetValue(reg, null);
                strHtml.Append("<th >" + valor + "</th>");
            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr><th>EMPRESA</th>");
            foreach (var reg in listaCabecera)
            {
                string valor = String.Empty;
                //int iEmprnomb = (int)reg.GetType().GetProperty("Emprnomb").GetValue(reg, null);
                valor = (string)reg.GetType().GetProperty("Emprnomb").GetValue(reg, null);
                strHtml.Append("<th >" + valor + "</th>");
            }
            strHtml.Append("</tr>");


            strHtml.Append("<tr><th>RECURSO</th>");
            foreach (var reg in listaCabecera)
            {
                string valor = String.Empty;
                //int iEmprnomb = (int)reg.GetType().GetProperty("Emprnomb").GetValue(reg, null);
                valor = (string)reg.GetType().GetProperty("Famabrev").GetValue(reg, null);
                strHtml.Append("<th >" + valor + "</th>");
            }
            strHtml.Append("</tr>");




            strHtml.Append("<tr><th  >EQUIPO</th>");
            foreach (var p in listaCabecera)
            {
                string valor = String.Empty;
                int ifamcodi = (int)p.GetType().GetProperty("Famcodi").GetValue(p, null);
                switch (ifamcodi)
                {
                    case ConstantesAppServicio.EstacionHidrologica: //Estacion Hidrologica
                        valor = (string)p.GetType().GetProperty("Ptomedibarranomb").GetValue(p, null);
                        break;
                    default:
                        valor = (string)p.GetType().GetProperty("Equinomb").GetValue(p, null);
                        break;

                }
                strHtml.Append("<th >" + valor + "</th>");
            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr><th  >TIPO</th>");
            foreach (var p in listaCabecera)
            {
                string valor3 = (string)p.GetType().GetProperty("Tipoptomedinomb").GetValue(p, null);
                strHtml.Append("<th style='font-size: 8px;'>" + valor3 + "</th>");
            }

            strHtml.Append("</tr>");
            strHtml.Append("<tr><th >FECHA-HORA / UNIDAD</th>");
            foreach (var p in listaCabecera)
            {
                string valor = (string)p.GetType().GetProperty("Tipoinfoabrev").GetValue(p, null);
                strHtml.Append("<th >" + valor + "</th>");
            }
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

        }

        /// <summary>
        /// Genera la cabecera de los reportes de Descarga de Lagunas y Vertimientos de Embalses
        /// </summary>        
        /// <param name="strHtml"></param>
        private void GeneraHtmlCabeceraDescargaVert(ref StringBuilder strHtml, int idFormato)
        {
            strHtml.Append("<thead>");

            if (idFormato == ConstHidrologia.IdFormatoDescargaLaguna) // Descarga de Lagunas
            {
                strHtml.Append("<tr><th colspan='10'>REPORTE DESCARGA DE LAGUNAS</th></tr>");
            }
            else // Vertimiento de embalses
            {
                strHtml.Append("<tr><th colspan='10'>REPORTE VERTIMIENTO DE EMBALSES</th></tr>");
            }
            strHtml.Append("<tr><th>FECHA</th>");
            strHtml.Append("<th>EMPRESA</th>");
            strHtml.Append("<th>EMBALSE</th>");
            strHtml.Append("<th>INICIO</th>");
            strHtml.Append("<th>FINAL</th>");
            strHtml.Append("<th>VALOR</th>");
            strHtml.Append("<th>UNIDAD</th>");
            strHtml.Append("<th>USUARIO</th>");
            strHtml.Append("<th>FECHA MMODIF</th>");
            strHtml.Append("<th>OBSERVACIÓN</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
        }

        /// <summary>
        /// imprime view para la tabla medicion24
        /// </summary>
        /// <param name="listaGenerica"></param>
        /// <param name="listaCabeceraM24"></param>
        /// <param name="nroLectura"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <returns></returns>
        public string GeneraViewHidrologiaMed24(List<MeMedicion24DTO> listaGenerica, List<MeMedicion24DTO> listaCabeceraM24, int nroLectura, DateTime fechaInicio, int rbDetalleRpte)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<table border='1' style='width:auto' class='pretty tabla-icono' cellspacing='0' width='100%' id='tabla'>");
            GeneraHtmlCabecera24(listaCabeceraM24, ref strHtml);
            strHtml.Append("<tbody>");
            //int resolucion = 60;
            //int nBloques = 60 * 24 / resolucion;           
            if (listaGenerica.Count > 0)
            {
                //fechaInicio = listaGenerica.Min(x => x.Medifecha);                    
                DateTime fant = new DateTime();
                DateTime f = new DateTime();
                foreach (var lst in listaGenerica)
                {
                    f = lst.Medifecha;
                    if (f != fant)
                    {
                        strHtml.Append("<tr>");
                        if (rbDetalleRpte == ConstantesRbDetalleRpte.Diario) // Diario
                            strHtml.Append(string.Format("<td>{0}</td>", f.ToString(ConstantesAppServicio.FormatoFecha)));
                        if (rbDetalleRpte == ConstantesRbDetalleRpte.SemanalProgramado) // Semanal programado
                            strHtml.Append(string.Format("<td>{0}</td>", "Sem-" + COES.Base.Tools.Util.GenerarNroSemana(f, FirstDayOfWeek.Saturday)));
                        if (rbDetalleRpte == ConstantesRbDetalleRpte.SemanalCronologico) //semanal cronologico
                            strHtml.Append(string.Format("<td>{0}</td>", "Sem-" + COES.Base.Tools.Util.GenerarNroSemana(f, FirstDayOfWeek.Sunday)));
                        if (rbDetalleRpte == ConstantesRbDetalleRpte.Mensual) // Mensual
                            strHtml.Append(string.Format("<td>{0}</td>", f.Year + "-" + COES.Base.Tools.Util.ObtenerNombreMes(f.Month)));
                        if (rbDetalleRpte == ConstantesRbDetalleRpte.Anual) //Anual
                            strHtml.Append(string.Format("<td>{0}</td>", "Año - " + f.Year));
                        foreach (var p in listaCabeceraM24)
                        {
                            var reg = listaGenerica.Find(x => x.Medifecha == f && x.Ptomedicodi == p.Ptomedicodi);
                            if (reg != null)
                            {
                                decimal? valor;
                                valor = (decimal?)reg.Meditotal;
                                if (valor != null)
                                    strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                                else
                                    strHtml.Append(string.Format("<td>--</td>"));
                            }
                            else
                                strHtml.Append("<td>--</td>");
                        }
                        strHtml.Append("</tr>");
                    }
                    fant = f;
                }
            }
            else
            {
                strHtml.Append("<td  style='text-align:center'>No existen registros.</td>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");


            return strHtml.ToString();
        }

        /// <summary>
        /// imprime view para tabla medicion24 Horas
        /// </summary>
        /// <param name="listaGenerica"></param>
        /// <param name="nroLectura"></param>
        /// <param name="fechaInicio"></param>
        /// <returns></returns>
        public string GeneraViewHidrologiaMed24_2(List<MeMedicion24DTO> listaGenerica, int nroLectura, DateTime fechaInicio)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<table border='1' style='width:auto' class='pretty tabla-icono ' cellspacing='0' width='100%' id='tabla'>");
            GeneraHtmlCabecera24(listaGenerica, ref strHtml);
            strHtml.Append("<tbody>");

            int resolucion = 60;
            int nBloques = 60 * 24 / resolucion;
            if (listaGenerica.Count > 0)
            {
                for (int i = 0; i < 1; i++)
                    for (int k = 1; k <= nBloques; k++)
                    {
                        var fechaMin = ((fechaInicio.AddMinutes((k - 1) * resolucion + i * 60 * 24))).ToString(ConstantesBase.FormatoFechaHora);
                        strHtml.Append("<tr>");
                        strHtml.Append(string.Format("<td>{0}</td>", fechaMin));
                        foreach (var p in listaGenerica)
                        {
                            decimal? valor;
                            valor = (decimal?)p.GetType().GetProperty("H" + k).GetValue(p, null);
                            if (valor != null)
                                strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                            else
                                strHtml.Append(string.Format("<td>--</td>"));

                            //decimal valor = (decimal)p.GetType().GetProperty("H" + k).GetValue(p, null);

                            //strHtml.Append(string.Format("<td>{0}</td>", valor.ToString("N", nfi)));
                        }

                        strHtml.Append("</tr>");
                    }

            }
            else
            {
                strHtml.Append("<td  style='text-align:center'>No existen registros.</td>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");


            return strHtml.ToString();
        }

        /// <summary>
        /// imprime view para la tabla medicion24 reporte Tipo 2
        /// </summary>
        /// <param name="listaGenerica"></param>
        /// <param name="listaCabeceraM24"></param>
        /// <param name="nroLectura"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <returns></returns>
        public string GeneraViewHidrologiaMed24QnVolTipo2(List<MeMedicion24DTO> lista1, List<MeMedicion24DTO> lista2, List<MeMedicion24DTO> listaCabeceraM24, DateTime fechaInicio, DateTime fechaFin, int rbDetalleRpte, int unidad)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<table border='1' style='width:auto' class='pretty tabla-icono' cellspacing='0' width='100%' id='tabla'>");

            if (unidad == ConstHidrologia.Volumen) // volumen
                fechaInicio = fechaFin; // se toma como dato el valor de la ultima fecha del periodo (semanal o mensual)
            //****Imprime encabezado de listado *******************************************************************************


            strHtml.Append("<thead>");
            if (rbDetalleRpte == ConstantesrbTipoRpteQnVol.Semanal)//********************Semanal (Qn/Vol)
            {
                strHtml.Append("<tr ><th  style='width:160px;' >TIPO</th>");
                foreach (var p in listaCabeceraM24)
                {
                    string valor = string.Empty;
                    int ifamcodi = p.Famcodi;
                    switch (ifamcodi)
                    {
                        case ConstantesAppServicio.EstacionHidrologica: //Estacion Hidrologica                            
                            valor = p.Ptomedibarranomb;
                            break;
                        default:
                            valor = p.Equinomb;
                            break;

                    }
                    strHtml.Append("<th style='width:100px;'>" + valor + "</th>");
                }
                strHtml.Append("</tr>");
            }
            if (rbDetalleRpte == ConstantesrbTipoRpteQnVol.Mensual)//***********************Mensual (Qn/Vol)
            {
                string tituloMes = COES.Base.Tools.Util.ObtenerNombreMes(fechaInicio.Month);
                strHtml.Append("<tr ><th  rowspan = '2' style='width:160px;' >EMPRESAS</th>");
                if (unidad == ConstHidrologia.Caudal)
                    strHtml.Append("<th  colspan = '2' style='width:320px;' >Caudales (m3/s)</th></tr>");
                else
                    strHtml.Append("<th  colspan = '2' style='width:320px;' >Volumen (Hm3)</th></tr>");
                strHtml.Append("<tr ><th  style='width:160px;' >a " + tituloMes + " de " + fechaInicio.AddYears(-1).Year + "</th>");
                strHtml.Append("<th  style='width:160px;' >a " + tituloMes + " de " + fechaInicio.Year + "</th></tr>");
            }
            if (rbDetalleRpte == ConstantesrbTipoRpteQnVol.Anual)//***************************Anual (Qn/Vol)
            {
                int anhoIni = fechaInicio.Year;
                int anhoFin = fechaFin.Year;
                strHtml.Append("<tr ><th  style='width:160px;' >MESES</th>");
                for (int i = anhoIni; i <= anhoFin; i++)
                {
                    strHtml.Append("<th   style='width:180px;' >" + i + "</th>");
                }
                strHtml.Append("</tr>");
            }

            strHtml.Append("</thead>");

            //*********************************************** fin de encabezado *********************************************
            strHtml.Append("<tbody>");


            if (lista2.Count > 0)
            {


                if (rbDetalleRpte == ConstantesrbTipoRpteQnVol.Semanal)//********************Semanal (Qn/Vol)
                {
                    var ListaTipoInf = lista2.Select(x => x.Tipoptomedinomb).Distinct().ToList();
                    foreach (var p in ListaTipoInf)// Imprime valores del año presente 
                    {
                        strHtml.Append("<tr>");
                        string tipoPtoMediNomb = (string)p;

                        strHtml.Append("<td style='width:100px;'>" + tipoPtoMediNomb + " Sem " + COES.Base.Tools.Util.GenerarNroSemana(fechaInicio, FirstDayOfWeek.Saturday) + "/" + fechaInicio.Year + "</td>");

                        foreach (var reg in listaCabeceraM24)
                        {
                            var reg2 = lista2.Find(x => x.Medifecha == fechaInicio && x.Ptomedicodi == reg.Ptomedicodi);
                            if (reg2 != null)
                            {
                                decimal? valor;
                                valor = (decimal?)reg2.Meditotal;
                                if (valor != null)
                                    strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                                else
                                    strHtml.Append(string.Format("<td>--</td>"));
                            }
                            else
                                strHtml.Append("<td>--</td>");
                        }

                        strHtml.Append("</tr>");
                    }
                    foreach (var p in ListaTipoInf)// Imprime valores del año anterior 
                    {
                        strHtml.Append("<tr>");
                        string tipoPtoMediNomb = (string)p;

                        strHtml.Append("<td style='width:100px;'>" + tipoPtoMediNomb + " Sem " + COES.Base.Tools.Util.GenerarNroSemana(fechaInicio, FirstDayOfWeek.Saturday) + "/" + fechaInicio.AddYears(-1).Year + "</td>");

                        foreach (var reg in listaCabeceraM24)
                        {
                            var reg2 = lista1.Find(x => x.Medifecha == fechaInicio && x.Ptomedicodi == reg.Ptomedicodi);
                            if (reg2 != null)
                            {
                                decimal? valor;
                                valor = (decimal?)reg2.Meditotal;
                                if (valor != null)
                                    strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                                else
                                    strHtml.Append(string.Format("<td>--</td>"));
                            }
                            else
                                strHtml.Append("<td>--</td>");
                        }

                        strHtml.Append("</tr>");
                    }
                }

                if (rbDetalleRpte == ConstantesrbTipoRpteQnVol.Mensual)//***********************Mensual (Qn/Vol)
                    foreach (var p in listaCabeceraM24)
                    {
                        strHtml.Append("<tr>");
                        string ptonomb = String.Empty;
                        int ifamcodi = p.Famcodi;
                        switch (ifamcodi)
                        {
                            case ConstantesAppServicio.EstacionHidrologica: //Estacion Hidrologica                            
                                ptonomb = p.Ptomedibarranomb;
                                break;
                            default:
                                ptonomb = p.Equinomb;
                                break;
                        }
                        strHtml.Append("<td style='width:100px;'>" + p.Emprnomb + " - " + ptonomb + " - " + p.Tipoptomedinomb + "</td>");
                        // Imprime valor del año anterior                   
                        var reg = lista1.Find(x => x.Medifecha == fechaInicio.AddYears(-1) && x.Ptomedicodi == p.Ptomedicodi);
                        if (reg != null)
                        {
                            decimal? valor;
                            valor = (decimal?)reg.Meditotal;
                            if (valor != null)
                                strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                            else
                                strHtml.Append(string.Format("<td>--</td>"));
                        }
                        else
                            strHtml.Append("<td>--</td>");
                        // Imprime valor del año presente                  
                        var reg2 = lista2.Find(x => x.Medifecha == fechaInicio && x.Ptomedicodi == p.Ptomedicodi);
                        if (reg2 != null)
                        {
                            decimal? valor;
                            valor = (decimal?)reg2.Meditotal;
                            if (valor != null)
                                strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                            else
                                strHtml.Append(string.Format("<td>--</td>"));
                        }
                        else
                            strHtml.Append("<td>--</td>");
                        strHtml.Append("</tr>");

                    }

                if (rbDetalleRpte == ConstantesrbTipoRpteQnVol.Anual)//********************Anual (Qn/Vol)
                {



                }
            }// end del if
            else
            {
                strHtml.Append("<tr><td  style='text-align:center'>No existen registros.</td></tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            return strHtml.ToString();
        }

        /// <summary>
        /// imprime view para la tabla medicion24 reporte Tipo 2 - Anual
        /// </summary>
        /// <param name="listaGenerica"></param>
        /// <param name="listaCabeceraM24"></param>
        /// <param name="nroLectura"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <returns></returns>
        public string GeneraViewHidrologiaMed24QnVolTipo2Anual(List<MeMedicion24DTO> lista1, List<MeMedicion24DTO> listaCabeceraM24, DateTime fechaInicio, DateTime fechaFin, int unidad)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<table border='1' style='width:auto' class='pretty tabla-icono' cellspacing='0' width='100%' >");

            int nroBloques = 12;

            if (lista1.Count > 0)
            {
                foreach (var p in listaCabeceraM24)
                {
                    strHtml.Append("<table border='1' style='width:auto' class='pretty tabla-icono' cellspacing='0' width='100%' >");

                    //****Imprime encabezado de listado *******************************************************************************

                    strHtml.Append("<thead>");
                    int anhoIni = fechaInicio.Year;
                    int anhoFin = fechaFin.Year;
                    int icolspan = anhoFin - anhoIni + 2;
                    string valor = string.Empty;
                    if (unidad == ConstHidrologia.Caudal) // Caudal
                        valor = "CAUDAL ";
                    else
                        valor = "VOLUMEN UTIL DE ";

                    int ifamcodi = p.Famcodi;
                    string ptonomb = String.Empty;
                    switch (ifamcodi)
                    {
                        case ConstantesAppServicio.EstacionHidrologica: //Estacion Hidrologica                            
                            ptonomb = p.Ptomedibarranomb;
                            break;
                        default:
                            ptonomb = p.Equinomb;
                            break;
                    }
                    strHtml.Append("<tr><th colspan = " + "'" + icolspan + "'" + ">" + valor + " " + ptonomb + "</th></tr>");
                    //strHtml.Append("<tr><th colspan = " + "'" + icolspan + "'" + "></th></tr>");
                    strHtml.Append("<tr ><th  style='width:160px;' >MESES</th>");
                    for (int i = anhoIni; i <= anhoFin; i++)
                    {
                        strHtml.Append("<th   style='width:180px;' >" + i + "</th>");
                    }
                    strHtml.Append("</tr>");
                    strHtml.Append("</thead>");

                    //*********************************************** fin de encabezado *********************************************
                    strHtml.Append("<tbody>");

                    for (int i = 1; i <= nroBloques; i++)
                    {
                        strHtml.Append("<tr><td style='width:100px;'>" + COES.Base.Tools.Util.ObtenerNombreMesAbrev(i) + "</td>");

                        for (int j = anhoIni; j <= anhoFin; j++)
                        {
                            DateTime fecha = new DateTime(j, i, 1);
                            //// Imprime valor del mes para cada año                    
                            var reg = lista1.Find(x => x.Medifecha == fecha && x.Ptomedicodi == p.Ptomedicodi);
                            if (reg != null)
                            {
                                decimal? valor1;
                                valor1 = (decimal?)reg.Meditotal;
                                if (valor1 != null)
                                    strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valor1).ToString("N", nfi)));
                                else
                                    strHtml.Append(string.Format("<td>--</td>"));
                            }
                            else
                                strHtml.Append("<td>--</td>");
                        }
                        strHtml.Append("</tr>");

                    }// end del for
                    strHtml.Append("<tr><td colspan = " + "'" + icolspan + "'" + "></td></tr>");
                    strHtml.Append("</tbody>");
                    strHtml.Append("</table>");
                }



            }// end del if
            else
            {
                strHtml.Append("<tr><td  style='text-align:center'>No existen registros.</td></tr>");
            }


            strHtml.Append("</table>");
            return strHtml.ToString();
        }

        /// <summary>
        /// imprime view para la tabla medicion24 -> TiempoReal
        /// </summary>
        /// <param name="listaGenerica"></param>
        /// <param name="listaCabecera"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public string GeneraViewHidrologiaMed24TiempoReal(List<MeMedicion24DTO> listaGenerica, List<MeReporptomedDTO> listaCabecera, DateTime fechaInicio, int tipo)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<table border='1' style='width:auto' class='pretty tabla-icono' cellspacing='0' width='100%' id='tabla'>");
            List<MeMedicion24DTO> listaCabeceraM24 = listaGenerica.GroupBy(x => new { x.Ptomedicodi, x.Cuenca, x.Emprnomb, x.Ptomedibarranomb, x.Ptomedielenomb, x.Tipoinfoabrev, x.Tipoptomedinomb, x.Equinomb, x.Famcodi, x.Famabrev })
                .Select(y => new MeMedicion24DTO()
                {
                    Cuenca = y.Key.Cuenca,
                    Emprnomb = y.Key.Emprnomb,
                    Famabrev = y.Key.Famabrev,
                    Ptomedicodi = y.Key.Ptomedicodi,
                    Ptomedibarranomb = y.Key.Ptomedibarranomb,
                    Ptomedielenomb = y.Key.Ptomedielenomb,
                    Tipoinfoabrev = y.Key.Tipoinfoabrev,
                    Tipoptomedinomb = y.Key.Tipoptomedinomb,
                    Equinomb = y.Key.Equinomb,
                    Famcodi = y.Key.Famcodi
                }
                ).ToList();

            GeneraHtmlCabecera24_2(listaCabecera, ref strHtml);
            strHtml.Append("<tbody>");

            int resolucion = 60;
            int nBloques = 60 * 24 / resolucion;
            if (listaGenerica.Count > 0)
            {
                for (int i = 0; i < 1; i++)
                    for (int k = 1; k <= nBloques; k++)
                    {
                        var fechaMin = ((fechaInicio.AddMinutes((k - 1) * resolucion + i * 60 * 24))).ToString(ConstantesBase.FormatoFechaHora);
                        strHtml.Append("<tr>");
                        strHtml.Append(string.Format("<td>{0}</td>", fechaMin));


                        //var reg = listaGenerica.Find(i => (int)i.GetType().GetProperty("Ptomedicodi").GetValue(i, null) == model.ListaHojaPto[k].Ptomedicodi
                        //               && (DateTime)i.GetType().GetProperty("Medifecha").GetValue(i, null) == model.FechaInicio.AddDays(z));


                        foreach (var p in listaCabecera)
                        {
                            var reg = listaGenerica.Find(x => x.Ptomedicodi == p.Ptomedicodi && x.Medifecha == fechaInicio);

                            if (reg != null)
                            {
                                decimal? valor;
                                valor = (decimal?)reg.GetType().GetProperty("H" + k).GetValue(reg, null);
                                if (valor != null)
                                    strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                                else
                                    strHtml.Append(string.Format("<td>--</td>"));
                            }
                            else
                                strHtml.Append("<td>--</td>");
                        }

                        strHtml.Append("</tr>");
                    }

            }
            else
            {
                int ncol = listaCabecera.Count;
                strHtml.Append("<tr><td style='text-align:center'>No existen registros.</td></tr>");
            }

            //******Imprime pie de pagina si son tipo de reporte 3(volumenes) ó 4(Caudales)
            if (tipo == ConstantesrbTipoRpteTR.Volumenes)// Tipo Volúmen
            {
                strHtml.Append("<td style='background-color:#2980B9;border:1px solid #9AC9E9; text-align: center; color:#ffffff' >" + "<div >" + "VOLÚMEN INICIAL" + "</div>" + "</td>");
                foreach (var p in listaCabecera)
                {
                    var reg = listaGenerica.Find(x => x.Ptomedicodi == p.Ptomedicodi && x.Medifecha == fechaInicio);

                    if (reg != null)
                    {
                        decimal? valor;
                        valor = (decimal?)reg.GetType().GetProperty("H1").GetValue(reg, null);
                        if (valor != null)
                            strHtml.Append(string.Format("<td style='background-color:#2980B9;border:1px solid #9AC9E9; text-align: center; color:#ffffff'>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                        else
                            strHtml.Append(string.Format("<td>--</td>"));
                    }
                    else
                        strHtml.Append("<td>--</td>");
                }
                strHtml.Append("</tr><tr>");
                strHtml.Append("<td style='background-color:#2980B9;border:1px solid #9AC9E9; text-align: center; color:#ffffff' >" + "<div >" + "VOLÚMEN FINAL" + "</div>" + "</td>");
                foreach (var p in listaCabecera)
                {
                    var reg = listaGenerica.Find(x => x.Ptomedicodi == p.Ptomedicodi && x.Medifecha == fechaInicio);

                    if (reg != null)
                    {
                        decimal? valor;
                        valor = (decimal?)reg.GetType().GetProperty("H24").GetValue(reg, null);
                        if (valor != null)
                            strHtml.Append(string.Format("<td style='background-color:#2980B9;border:1px solid #9AC9E9; text-align: center; color:#ffffff'>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                        else
                            strHtml.Append(string.Format("<td>--</td>"));
                    }
                    else
                        strHtml.Append("<td>--</td>");
                }

                strHtml.Append("</tr>");
            }

            if (tipo == ConstantesrbTipoRpteTR.Caudales)// Tipo Caudales
            {
                strHtml.Append("<td style='background-color:#2980B9;border:1px solid #9AC9E9; text-align: center; color:#ffffff' >" + "<div >" + "CAUDAL MÁXIMO" + "</div>" + "</td>");
                foreach (var p in listaCabecera)
                {
                    var reg = listaGenerica.Find(x => x.Ptomedicodi == p.Ptomedicodi && x.Medifecha == fechaInicio);
                    if (reg != null)
                    {
                        decimal valorMax = 0;
                        for (int i = 1; i <= 24; i++)
                        {
                            decimal? valor;
                            valor = (decimal)reg.GetType().GetProperty("H" + i).GetValue(reg, null);
                            if (valor != null)
                                if (valorMax < valor)
                                    valorMax = (decimal)valor;
                        }
                        strHtml.Append(string.Format("<td style='background-color:#2980B9;border:1px solid #9AC9E9; text-align: center; color:#ffffff'>{0}</td>", valorMax.ToString("N", nfi)));
                    }
                    else
                        strHtml.Append("<td>--</td>");
                }
                strHtml.Append("</tr><tr>");
                strHtml.Append("<td style='background-color:#2980B9;border:1px solid #9AC9E9; text-align: center; color:#ffffff' >" + "<div >" + "CAUDAL MÍNIMO" + "</div>" + "</td>");
                foreach (var p in listaCabecera)
                {
                    var reg = listaGenerica.Find(x => x.Ptomedicodi == p.Ptomedicodi && x.Medifecha == fechaInicio);

                    if (reg != null)
                    {
                        decimal valorMin = 0;
                        // obtenemos el primer valor del campo Hi como valor minimo
                        for (int i = 1; i <= 24; i++)
                        {
                            decimal? valor;
                            valor = (decimal?)reg.GetType().GetProperty("H" + i).GetValue(reg, null);
                            if (valor != null)
                            {
                                valorMin = (decimal)valor;
                                break;
                            }

                        }

                        for (int i = 1; i <= 24; i++)
                        {
                            decimal? valor;
                            valor = (decimal?)reg.GetType().GetProperty("H" + i).GetValue(reg, null);
                            if (valor != null)
                                if (valorMin > valor)
                                    valorMin = (decimal)valor;
                        }
                        strHtml.Append(string.Format("<td style='background-color:#2980B9;border:1px solid #9AC9E9; text-align: center; color:#ffffff'>{0}</td>", valorMin.ToString("N", nfi)));
                    }
                    else
                        strHtml.Append("<td>--</td>");
                }
                strHtml.Append("</tr><tr>");
                strHtml.Append("<td style='background-color:#2980B9;border:1px solid #9AC9E9; text-align: center; color:#ffffff' >" + "<div >" + "CAUDAL PROMEDIO" + "</div>" + "</td>");
                foreach (var p in listaCabecera)
                {
                    var reg = listaGenerica.Find(x => x.Ptomedicodi == p.Ptomedicodi && x.Medifecha == fechaInicio);

                    if (reg != null)
                    {
                        decimal valorProm = 0;
                        for (int i = 1; i <= 24; i++)
                            valorProm = valorProm + (decimal)reg.GetType().GetProperty("H" + i).GetValue(reg, null);
                        valorProm = valorProm / 24;
                        strHtml.Append(string.Format("<td style='background-color:#2980B9;border:1px solid #9AC9E9; text-align: center; color:#ffffff'>{0}</td>", valorProm.ToString("N", nfi)));
                    }
                    else
                        strHtml.Append("<td>--</td>");
                }
                strHtml.Append("</tr>");
            }




            strHtml.Append("</tbody>");
            strHtml.Append("</table>");


            return strHtml.ToString();
        }

        /// <summary>
        /// imprime view para la tabla medicion1
        /// </summary>
        /// <param name="listaGenerica"></param>
        /// <param name="listaCabecera"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <returns></returns>
        public string GeneraViewHidrologiaMed1(List<MeMedicion1DTO> listaGenerica, List<MeMedicion1DTO> listaCabecera, DateTime fechaInicio, DateTime fechaFin, int rbDetalleRpte)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<table border='1' style='width:auto' class='pretty tabla-icono ' width='100%' id='tabla'>");
            GeneraHtmlCabecera1(listaCabecera, ref strHtml);

            strHtml.Append("<tbody>");

            //if (formato.Formatresolucion == 60 * 24)
            //    fechaInicio = fechaInicio.AddDays(-1);
            if (listaGenerica.Count > 0)
            {
                fechaInicio = listaGenerica.Min(x => x.Medifecha);
                //int i = COES.Base.Tools.Util.GenerarNroSemana(fechaInicio); 
                DateTime fant = new DateTime();
                DateTime f = new DateTime();
                foreach (var lst in listaGenerica)
                {
                    f = lst.Medifecha;

                    if (f != fant)
                    {
                        strHtml.Append("<tr>");
                        var anho = f.Year.ToString();
                        var mes = f.Month;
                        switch (rbDetalleRpte)
                        {
                            case ConstantesRbDetalleRpte.Diario://Imprime en primera columna en formato fecha
                                //strHtml.Append(string.Format("<td>{0}</td>", f.Day + "/" + f.Month + "/" + f.Year));
                                strHtml.Append(string.Format("<td>{0}</td>", f.ToString(ConstantesAppServicio.FormatoFecha)));
                                break;
                            case ConstantesRbDetalleRpte.SemanalProgramado: // imprime año - nº semana -> Semanal Programado
                                strHtml.Append(string.Format("<td>{0} - SEM-{1}</td>", f.Year.ToString(), COES.Base.Tools.Util.GenerarNroSemana(f, FirstDayOfWeek.Saturday)));
                                //strHtml.Append(string.Format("<td>{0}</td>", i));
                                break;
                            case ConstantesRbDetalleRpte.SemanalCronologico:// imprime año - nº semana -> Semanal Cronológico
                                strHtml.Append(string.Format("<td>{0} - SEM-{1}</td>", f.Year.ToString(), COES.Base.Tools.Util.GenerarNroSemana(f, FirstDayOfWeek.Sunday)));
                                break;
                            case ConstantesRbDetalleRpte.Mensual://imprime año - nombre mes
                                strHtml.Append(string.Format("<td>{0} &nbsp;&nbsp;{1}</td>", anho, COES.Base.Tools.Util.ObtenerNombreMes(mes)));
                                break;
                            case ConstantesRbDetalleRpte.Anual: // anual
                                strHtml.Append(string.Format("<td>{0}</td>", "Año - " + anho));
                                break;
                        }

                        foreach (var p in listaCabecera)
                        {
                            var reg = listaGenerica.Find(x => x.Medifecha == f && x.Ptomedicodi == p.Ptomedicodi);
                            if (reg != null)
                            {
                                decimal? valor;
                                valor = (decimal?)reg.H1;
                                if (valor != null)
                                    strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                                else
                                    strHtml.Append(string.Format("<td>--</td>"));
                            }
                            else
                                strHtml.Append("<td>--</td>");
                        }
                        strHtml.Append("</tr>");

                    }

                    fant = f;
                }
            }
            else
            {
                strHtml.Append("<td  style='text-align:center'>No existen registros.</td>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            return strHtml.ToString();
        }

        /// <summary>
        /// imprime view para la tabla medicion1 reporte tipo 2 caudales y volumenes
        /// </summary>
        /// <param name="listaGenerica"></param>
        /// <param name="listaCabecera"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <returns></returns>
        public string GeneraViewHidrologiaMed1QnVolTipo2(List<MeMedicion1DTO> lista1, List<MeMedicion1DTO> lista2, List<MeMedicion1DTO> listaCabecera, DateTime fechaInicio, DateTime fechaFin, int rbDetalleRpte, int unidad)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<table border='1' style='width:auto' class='pretty tabla-icono ' width='100%' id='tabla'>");

            //****Imprime encabezado de listado *******************************************************************************

            strHtml.Append("<thead>");
            if (rbDetalleRpte == ConstantesrbTipoRpteQnVol.Semanal)//********************Semanal (Qn/Vol)
            {
                strHtml.Append("<tr ><th  style='width:160px;' >TIPO</th>");
                foreach (var p in listaCabecera)
                {
                    string valor = String.Empty;
                    int ifamcodi = p.Famcodi;
                    switch (ifamcodi)
                    {
                        case ConstantesAppServicio.EstacionHidrologica: //Estacion Hidrologica                            
                            valor = p.Ptomedibarranomb;
                            break;
                        default:
                            valor = p.Equinomb;
                            break;

                    }
                    strHtml.Append("<th style='width:100px;'>" + valor + "</th>");
                }
                strHtml.Append("</tr>");
            }
            if (rbDetalleRpte == ConstantesrbTipoRpteQnVol.Mensual)//***********************Mensual (Qn/Vol)
            {
                string tituloMes = COES.Base.Tools.Util.ObtenerNombreMes(fechaInicio.Month);
                strHtml.Append("<tr ><th  rowspan = '2' style='width:160px;' >EMPRESAS</th>");
                if (unidad == ConstHidrologia.Caudal)
                    strHtml.Append("<th  colspan = '2' style='width:320px;' >Caudales (m3/s)</th></tr>");
                else
                    strHtml.Append("<th  colspan = '2' style='width:320px;' >Volumen (Hm3)</th></tr>");
                strHtml.Append("<tr ><th  style='width:160px;' >a " + tituloMes + " de " + fechaInicio.AddYears(-1).Year + "</th>");
                strHtml.Append("<th  style='width:160px;' >a " + tituloMes + " de " + fechaInicio.Year + "</th></tr>");
            }
            if (rbDetalleRpte == ConstantesrbTipoRpteQnVol.Anual)//***************************Anual (Qn/Vol)
            {
                int anhoIni = fechaInicio.Year;
                int anhoFin = fechaFin.Year;
                strHtml.Append("<tr ><th  style='width:160px;' >MESES</th>");
                for (int i = anhoIni; i <= anhoFin; i++)
                {
                    strHtml.Append("<th   style='width:180px;' >" + i + "</th>");
                }
                strHtml.Append("</tr>");
            }

            strHtml.Append("</thead>");

            //*********************************************** fin de encabezado *********************************************
            strHtml.Append("<tbody>");

            if (lista2.Count > 0)
            {


                if (rbDetalleRpte == ConstantesrbTipoRpteQnVol.Semanal)//********************Semanal (Qn/Vol)
                {
                    var ListaTipoInf = lista2.Select(x => x.Tipoptomedinomb).Distinct().ToList();
                    foreach (var p in ListaTipoInf)// Imprime valores del año presente 
                    {
                        strHtml.Append("<tr>");
                        string tipoPtoMediNomb = (string)p;

                        strHtml.Append("<td style='width:100px;'>" + tipoPtoMediNomb + " Sem " + COES.Base.Tools.Util.GenerarNroSemana(fechaInicio, FirstDayOfWeek.Saturday) + "/" + fechaInicio.Year + "</td>");

                        foreach (var reg in listaCabecera)
                        {
                            var reg2 = lista2.Find(x => x.Medifecha == fechaInicio && x.Ptomedicodi == reg.Ptomedicodi);
                            if (reg2 != null)
                            {
                                decimal? valor;
                                valor = (decimal?)reg2.H1;
                                if (valor != null)
                                    strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                                else
                                    strHtml.Append(string.Format("<td>--</td>"));
                            }
                            else
                                strHtml.Append("<td>--</td>");
                        }

                        strHtml.Append("</tr>");
                    }
                    foreach (var p in ListaTipoInf)// Imprime valores del año anterior 
                    {
                        strHtml.Append("<tr>");
                        string tipoPtoMediNomb = (string)p;

                        strHtml.Append("<td style='width:100px;'>" + tipoPtoMediNomb + " Sem " + COES.Base.Tools.Util.GenerarNroSemana(fechaInicio, FirstDayOfWeek.Saturday) + "/" + fechaInicio.AddYears(-1).Year + "</td>");

                        foreach (var reg in listaCabecera)
                        {
                            var reg2 = lista1.Find(x => x.Medifecha == fechaInicio && x.Ptomedicodi == reg.Ptomedicodi);
                            if (reg2 != null)
                            {
                                decimal? valor;
                                valor = (decimal?)reg2.H1;
                                if (valor != null)
                                    strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                                else
                                    strHtml.Append(string.Format("<td>--</td>"));
                            }
                            else
                                strHtml.Append("<td>--</td>");
                        }

                        strHtml.Append("</tr>");
                    }
                }

                if (rbDetalleRpte == ConstantesrbTipoRpteQnVol.Mensual)//***********************Mensual (Qn/Vol)
                    foreach (var p in listaCabecera)
                    {
                        strHtml.Append("<tr>");
                        string ptonomb = String.Empty;
                        int ifamcodi = p.Famcodi;
                        switch (ifamcodi)
                        {
                            case ConstantesAppServicio.EstacionHidrologica: //Estacion Hidrologica                            
                                ptonomb = p.Ptomedibarranomb;
                                break;
                            default:
                                ptonomb = p.Equinomb;
                                break;
                        }
                        strHtml.Append("<td style='width:100px;'>" + p.Emprnomb + " - " + ptonomb + " - " + p.Tipoptomedinomb + "</td>");
                        // Imprime valor del año anterior                   
                        var reg = lista1.Find(x => x.Medifecha == fechaInicio.AddYears(-1) && x.Ptomedicodi == p.Ptomedicodi);
                        if (reg != null)
                        {
                            decimal? valor;
                            valor = (decimal?)reg.H1;
                            if (valor != null)
                                strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                            else
                                strHtml.Append(string.Format("<td>--</td>"));
                        }
                        else
                            strHtml.Append("<td>--</td>");
                        // Imprime valor del año presente                  
                        var reg2 = lista2.Find(x => x.Medifecha == fechaInicio && x.Ptomedicodi == p.Ptomedicodi);
                        if (reg2 != null)
                        {
                            decimal? valor;
                            valor = (decimal?)reg2.H1;
                            if (valor != null)
                                strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                            else
                                strHtml.Append(string.Format("<td>--</td>"));
                        }
                        else
                            strHtml.Append("<td>--</td>");
                        strHtml.Append("</tr>");

                    }

                if (rbDetalleRpte == ConstantesrbTipoRpteQnVol.Anual)//********************Anual (Qn/Vol)
                {



                }
            }// end del if
            else
            {
                strHtml.Append("<tr><td  style='text-align:center'>No existen registros.</td></tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            return strHtml.ToString();
        }

        /// <summary>
        /// imprime view para la tabla medicion1 reporte Tipo 2 - Anual
        /// </summary>
        /// <param name="listaGenerica"></param>
        /// <param name="listaCabeceraM24"></param>
        /// <param name="nroLectura"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <returns></returns>
        public string GeneraViewHidrologiaMed1QnVolTipo2Anual(List<MeMedicion1DTO> lista1, List<MeMedicion1DTO> listaCabeceraM1, DateTime fechaInicio, DateTime fechaFin, int unidad)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<table border='1' style='width:auto' class='pretty tabla-icono' cellspacing='0' width='100%' >");

            int nroBloques = 12;

            if (lista1.Count > 0)
            {
                foreach (var p in listaCabeceraM1)
                {
                    strHtml.Append("<table border='1' style='width:auto' class='pretty tabla-icono' cellspacing='0' width='100%' >");

                    //****Imprime encabezado de listado *******************************************************************************

                    strHtml.Append("<thead>");
                    int anhoIni = fechaInicio.Year;
                    int anhoFin = fechaFin.Year;
                    int icolspan = anhoFin - anhoIni + 2;
                    string valor = string.Empty;
                    if (unidad == ConstHidrologia.Caudal) // Caudal
                        valor = "CAUDAL ";
                    else
                        valor = "VOLUMEN UTIL DE ";

                    int ifamcodi = p.Famcodi;
                    string ptonomb = String.Empty;
                    switch (ifamcodi)
                    {
                        case ConstantesAppServicio.EstacionHidrologica: //Estacion Hidrologica                            
                            ptonomb = p.Ptomedibarranomb;
                            break;
                        default:
                            ptonomb = p.Equinomb;
                            break;
                    }
                    strHtml.Append("<tr><th colspan = " + "'" + icolspan + "'" + ">" + valor + " " + ptonomb + "</th></tr>");
                    //strHtml.Append("<tr><th colspan = " + "'" + icolspan + "'" + "></th></tr>");
                    strHtml.Append("<tr ><th  style='width:160px;' >MESES</th>");
                    for (int i = anhoIni; i <= anhoFin; i++)
                    {
                        strHtml.Append("<th   style='width:180px;' >" + i + "</th>");
                    }
                    strHtml.Append("</tr>");
                    strHtml.Append("</thead>");

                    //*********************************************** fin de encabezado *********************************************
                    strHtml.Append("<tbody>");

                    for (int i = 1; i <= nroBloques; i++)
                    {
                        strHtml.Append("<tr><td style='width:100px;'>" + COES.Base.Tools.Util.ObtenerNombreMesAbrev(i) + "</td>");

                        for (int j = anhoIni; j <= anhoFin; j++)
                        {
                            DateTime fecha = new DateTime(j, i, 1);
                            //// Imprime valor del mes para cada año                    
                            var reg = lista1.Find(x => x.Medifecha == fecha && x.Ptomedicodi == p.Ptomedicodi);
                            if (reg != null)
                            {
                                decimal? valor1;
                                valor1 = (decimal?)reg.H1;
                                if (valor1 != null)
                                    strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valor1).ToString("N", nfi)));
                                else
                                    strHtml.Append(string.Format("<td>--</td>"));
                            }
                            else
                                strHtml.Append("<td>--</td>");
                        }
                        strHtml.Append("</tr>");

                    }// end del for
                    strHtml.Append("<tr><td colspan = " + "'" + icolspan + "'" + "></td></tr>");
                    strHtml.Append("</tbody>");
                    strHtml.Append("</table>");
                }



            }// end del if
            else
            {
                strHtml.Append("<tr><td  style='text-align:center'>No existen registros.</td></tr>");
            }


            strHtml.Append("</table>");
            return strHtml.ToString();
        }

        public string GeneraViewHidrologiaMed1Reporte(List<MeMedicion1DTO> listaGenerica, List<MeReporptomedDTO> listaCabecera, DateTime fechaInicio)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<table border='1' style='width:auto' class='pretty tabla-icono ' cellspacing='0' width='100%' id='tabla'>");
            strHtml.Append("<thead>");

            strHtml.Append("<tr><th  style='width:160px;'>TIPO</th>");

            int ifamcodi = 0;
            string sValor = String.Empty;
            foreach (var p in listaCabecera)
            {
                ifamcodi = (int)p.GetType().GetProperty("Famcodi").GetValue(p, null);
                switch (ifamcodi)
                {
                    case ConstantesAppServicio.EstacionHidrologica: //Estacion Hidrologica
                        sValor = (string)p.GetType().GetProperty("Ptomedibarranomb").GetValue(p, null);
                        break;
                    default:
                        sValor = (string)p.GetType().GetProperty("Equinomb").GetValue(p, null);
                        break;

                }
                strHtml.Append("<th style='width:100px;'>" + sValor + "</th>");
            }
            strHtml.Append("</tr>");

            //foreach (var p in listaCabecera)
            //{
            //    strHtml.Append("<td style='background-color:#2980B9;border:1px solid #9AC9E9;color:#87CEEB;text-align: center'>" + p.Tipoptomedinomb + "</td>");
            //}
            //strHtml.Append("</tr><tr>");
            //foreach (var p in listaCabecera)
            //{
            //    strHtml.Append("<td style='background-color:#2980B9;border:1px solid #9AC9E9;color:#87CEEB;text-align: center'>" + p.Tipoinfoabrev + "</td>");
            //}

            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");

            if (listaGenerica.Count > 0)
            {
                fechaInicio = listaGenerica.Min(x => x.Medifecha);
                //int i = COES.Base.Tools.Util.GenerarNroSemana(fechaInicio); 
                DateTime fant = new DateTime();
                DateTime f = new DateTime();
                foreach (var lst in listaGenerica)
                {
                    f = lst.Medifecha;

                    if (f != fant)
                    {
                        strHtml.Append("<tr>");
                        var anho = f.Year.ToString();
                        var mes = f.Month;
                        strHtml.Append(string.Format("<td>{0} - SEM-{1}</td>", f.Year.ToString(), COES.Base.Tools.Util.GenerarNroSemana(f, FirstDayOfWeek.Saturday)));

                        foreach (var p in listaCabecera)
                        {
                            var reg = listaGenerica.Find(x => x.Medifecha == f && x.Ptomedicodi == p.Ptomedicodi);
                            if (reg != null)
                            {
                                decimal? valor;
                                valor = (decimal?)reg.H1;
                                if (valor != null)
                                    strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                                else
                                    strHtml.Append(string.Format("<td>--</td>"));
                            }
                            else
                                strHtml.Append("<td>--</td>");
                        }
                        strHtml.Append("</tr>");

                    }

                    fant = f;
                }
            }
            else
            {
                strHtml.Append("<td  style='text-align:center'>No existen registros.</td>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            return strHtml.ToString();
        }

        /// <summary>
        /// imprime view para la tabla MeMedicionxintervalo reporte Descarga de Lagunas y Vertimiento de Embalses
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="fechaIni"></param>
        /// <param name="tipoReporte"></param>
        /// <returns></returns>
        public string GeneraViewHidrologiaDescargaVert(List<MeMedicionxintervaloDTO> lista, int idFormato)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<table border='1' style='width:100%;' class='pretty tabla-icono' cellspacing='0' id='tabla'>");
            GeneraHtmlCabeceraDescargaVert(ref strHtml, idFormato);
            strHtml.Append("<tbody>");

            if (lista.Count > 0)
            {
                {
                    foreach (var p in lista)
                    {
                        strHtml.Append("<tr>");
                        var fechaMin = p.Medintfechaini.ToString(ConstantesAppServicio.FormatoFecha);
                        strHtml.Append(string.Format("<td>{0}</td>", fechaMin));
                        strHtml.Append(string.Format("<td>{0}</td>", p.Emprnomb));
                        strHtml.Append(string.Format("<td>{0}</td>", p.Equinomb));
                        var horaIni = p.Medintfechaini.ToString(ConstantesAppServicio.FormatoHHmmss);
                        strHtml.Append(string.Format("<td>{0}</td>", horaIni));
                        var horaFin = p.Medintfechafin.ToString(ConstantesAppServicio.FormatoHHmmss);
                        strHtml.Append(string.Format("<td>{0}</td>", horaFin));
                        decimal? valor;
                        valor = (decimal?)p.Medinth1;
                        if (valor != null)
                            strHtml.Append(string.Format("<td>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                        else
                            strHtml.Append(string.Format("<td>--</td>"));
                        strHtml.Append(string.Format("<td>{0}</td>", p.Tipoinfoabrev));
                        strHtml.Append(string.Format("<td>{0}</td>", p.Medintusumodificacion));
                        var fechaModif = ((DateTime)p.Medintfecmodificacion).ToString(ConstantesAppServicio.FormatoFecha);
                        strHtml.Append(string.Format("<td>{0}</td>", fechaModif));
                        strHtml.Append(string.Format("<td>{0}</td>", p.Medintdescrip));
                        strHtml.Append("</tr>");
                    }
                }
            }
            else
            {
                strHtml.Append("<tr><td colspan= '10' style='text-align:center'>No existen registros.</td></tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            return strHtml.ToString();
        }


        #endregion

        #region Metodos Tabla ME_MEDICION48

        /// <summary>
        /// Permite eliminar los valores previos cargados
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void EliminarValoresCargados48(int idLectura, int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                FactorySic.GetMeMedicion48Repository().DeleteEnvioArchivo(idLectura, fechaInicio, fechaFin, idFormato, idEmpresa);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite grabar los datos cargados
        /// </summary>
        /// <param name="entitys"></param>

        public void GrabarValoresCargados48(List<MeMedicion48DTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            try
            {
                //Traer Ultimos Valores
                var lista = Convert48DTO(GetDataFormato(idEmpresa, formato, 0, 0));
                if (lista.Count > 0) // Verificar si hay cambios en el envio
                {
                    var listaCambio = new List<MeCambioenvioDTO>();
                    var listaOrigen = new List<MeCambioenvioDTO>();
                    foreach (var reg in entitys)
                    {
                        var regAnt = lista.Find(x => x.Medifecha == reg.Medifecha && x.Ptomedicodi == reg.Ptomedicodi &&
                            x.Lectcodi == reg.Lectcodi);
                        List<string> filaValores = new List<string>();
                        List<string> filaValoresOrigen = new List<string>();
                        List<string> filaCambios = new List<string>();
                        if (regAnt != null)
                        {
                            for (int i = 1; i <= 48; i++)
                            {
                                decimal? valorOrigen = (decimal?)regAnt.GetType().GetProperty("H" + i.ToString()).GetValue(regAnt, null);
                                decimal? valorModificado = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                                if (valorModificado != null)
                                    filaValores.Add(valorModificado.ToString());
                                else
                                    filaValores.Add("");
                                if (valorOrigen != null)
                                    filaValoresOrigen.Add(valorOrigen.ToString());
                                else
                                    filaValoresOrigen.Add("");
                                if (valorOrigen != valorModificado)// && valorOrigen != null && valorModificado != null)
                                {
                                    filaCambios.Add(i.ToString());
                                }
                            }
                        }
                        if (filaCambios.Count > 0)
                        {
                            MeCambioenvioDTO cambio = new MeCambioenvioDTO();
                            cambio.Cambenvdatos = String.Join(",", filaValores);
                            cambio.Cambenvcolvar = String.Join(",", filaCambios);
                            cambio.Cambenvfecha = (DateTime)reg.Medifecha;
                            cambio.Enviocodi = idEnvio;
                            cambio.Formatcodi = formato.Formatcodi;
                            cambio.Ptomedicodi = reg.Ptomedicodi;
                            cambio.Tipoinfocodi = reg.Tipoinfocodi;
                            cambio.Lastuser = usuario;
                            cambio.Lastdate = DateTime.Now;
                            listaCambio.Add(cambio);
                            /// Si no ha habido cambio se graba el registro original
                            if (ListMeCambioenvios(reg.Ptomedicodi, reg.Tipoinfocodi, formato.Formatcodi, (DateTime)reg.Medifecha).Count == 0)
                            {
                                int idEnvioPrevio = 0;
                                var listAux = GetByCriteriaMeEnvios(idEmpresa, formato.Formatcodi, formato.FechaProceso);
                                if (listAux.Count > 0)
                                    idEnvioPrevio = listAux.Min(x => x.Enviocodi);
                                MeCambioenvioDTO origen = new MeCambioenvioDTO();
                                origen.Cambenvdatos = String.Join(",", filaValoresOrigen);
                                origen.Cambenvcolvar = "";
                                origen.Cambenvfecha = (DateTime)reg.Medifecha;
                                origen.Enviocodi = idEnvioPrevio;
                                origen.Formatcodi = formato.Formatcodi;
                                origen.Ptomedicodi = reg.Ptomedicodi;
                                origen.Tipoinfocodi = reg.Tipoinfocodi;
                                origen.Lastuser = usuario;
                                origen.Lastdate = DateTime.Now;
                                listaOrigen.Add(origen);
                            }
                        }
                    }
                    if (listaCambio.Count > 0)
                    {//Grabar Cambios
                        GrabarCambios(listaCambio);
                        GrabarCambios(listaOrigen);
                        //si es primer reenvio grabar valores origen

                    }
                }
                //Eliminar Valores Previos
                EliminarValoresCargados48((int)formato.Lectcodi, formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                foreach (MeMedicion48DTO entity in entitys)
                {
                    FactorySic.GetMeMedicion48Repository().Save(entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla ME_MEDICION24

        public List<MeMedicion24DTO> ListaMed24Hidrologia(int lectocodi, int origlect, string idsEmpresa, string idsCuenca, string idsFamilia, DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion)
        {
            return FactorySic.GetMeMedicion24Repository().GetHidrologia(lectocodi, origlect, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion);
        }

        /// <summary>
        /// Reporte de Hidrología en Tiempo Real
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="idOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsTipoPtoMedicion"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> ListaMed24HidrologiaTiempoReal(int reporcodi, int idOrigenLectura, string idsEmpresa, DateTime fechaInicio,
            DateTime fechaFin, string idsTipoPtoMedicion)
        {
            int lectCodi = 66;
            var lista = FactorySic.GetMeMedicion24Repository().GetHidrologiaTiempoReal(reporcodi, idOrigenLectura, idsEmpresa, fechaInicio, fechaFin,
                idsTipoPtoMedicion, lectCodi);

            //CmgCP-Pr07
            //solo mostrar los valores Ejecutados, no proyectados.
            foreach (var objM24 in lista)
            {
                for (int h24 = 1; h24 <= 24; h24++)
                {
                    int? tipoM24 = (int?)objM24.GetType().GetProperty("T" + h24).GetValue(objM24, null);

                    //si es Caudal y el valor es programado entonces mostrar NULL en los reportes
                    if (objM24.Tipoinfocodi == ConstHidrologia.Caudal && ConstHidrologia.TipoHProgramado == tipoM24)
                    {
                        objM24.GetType().GetProperty("H" + h24).SetValue(objM24, null);
                    }
                }
            }

            return lista;
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeMedicion24
        /// </summary>
        public List<MeMedicion24DTO> GetByCriteriaMeMedicion24s()
        {
            return FactorySic.GetMeMedicion24Repository().GetByCriteria();
        }

        /// <summary>
        /// Permite eliminar los valores previos cargados
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void EliminarValoresCargados24(int idLectura, int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                FactorySic.GetMeMedicion24Repository().DeleteEnvioArchivo(idLectura, fechaInicio, fechaFin, idFormato, idEmpresa);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener la cantidad de registros con fechas distintas
        /// </summary>
        /// <returns></returns>
        public int ObtenerNroFilasMed1Hidrologia(int lectocodi, int idOrigenLectura, string idsEmpresa, string idsCuenca, string idsFamilia, DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion)
        {
            int cant;
            var lista1 = FactorySic.GetMeMedicion24Repository().GetHidrologia(lectocodi, idOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion).Select(x => x.Medifecha).Distinct().ToList();
            cant = lista1.Count();
            return cant;
        }

        /// <summary>
        /// Permite obtener la cantidad de registros con fechas distintas para la paginacion del reporte de tiempo real
        /// </summary>
        /// <returns></returns>
        public int ObtenerNroFilasHidrologiaTiempoReal(int reporcodi, int idOrigenLectura, string idsEmpresa, DateTime fechaInicio,
            DateTime fechaFin, string idsTipoPtoMedicion)
        {
            int cant;
            int idLectura = 66;
            var lista1 = FactorySic.GetMeMedicion24Repository().GetHidrologiaTiempoReal(reporcodi, idOrigenLectura, idsEmpresa, fechaInicio, fechaFin, idsTipoPtoMedicion, idLectura).Select(x => x.Medifecha).Distinct().ToList();
            cant = lista1.Count();
            return cant;
        }

        public List<MeMedicion24DTO> ListaHistoricoHidrologia(int reportecodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion24DTO> l = FactorySic.GetMeMedicion24Repository().GetDataHistoricoHidrologia(reportecodi, fechaInicio, fechaFin);
            foreach (var m24 in l)
            {
                m24.Emprnomb = m24.Emprnomb == null ? "" : m24.Emprnomb.Trim();
                m24.Gruponomb = m24.Gruponomb == null ? "" : m24.Gruponomb.Trim();
                m24.Equinomb = m24.Equinomb == null ? "" : m24.Equinomb.Trim();
                m24.Tipoptomedinomb = m24.Tipoptomedinomb == null ? "" : m24.Tipoptomedinomb.Trim();
            }

            return l;
        }

        #endregion

        #region Métodos Tabla ME_MEDICION1

        /// <summary>
        /// Permite eliminar los valores previos cargados
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void EliminarValoresCargados1(int idLectura, int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                FactorySic.GetMeMedicion1Repository().DeleteEnvioArchivo(idLectura, fechaInicio, fechaFin, idFormato, idEmpresa);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Graba Valores Cargados en  Hoja Web Excel y verifica si hay repeditos
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="usuario"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        public void GrabarValoresCargados1(List<MeMedicion1DTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            try
            {
                //Traer Ultimos Valores
                var lista = Convert1DTO(GetDataFormato(idEmpresa, formato, 0, 0));
                if (lista.Count > 0) // Verificar si hay cambios en el envio
                {
                    var listaCambio = new List<MeCambioenvioDTO>();
                    var listaOrigen = new List<MeCambioenvioDTO>();
                    foreach (var reg in entitys)
                    {
                        var regAnt = lista.Find(x => x.Medifecha == reg.Medifecha && x.Ptomedicodi == reg.Ptomedicodi &&
                            x.Lectcodi == reg.Lectcodi);
                        List<string> filaValores = new List<string>();
                        List<string> filaValoresOrigen = new List<string>();
                        List<string> filaCambios = new List<string>();
                        if (regAnt != null)
                        {
                            decimal? valorOrigen = regAnt.H1; // (decimal?)regAnt.GetType().GetProperty("H" + i.ToString()).GetValue(regAnt, null);
                            decimal? valorModificado = reg.H1; //(decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                            if (valorModificado != null)
                                filaValores.Add(valorModificado.ToString());
                            else
                                filaValores.Add("");
                            if (valorOrigen != null)
                                filaValoresOrigen.Add(valorOrigen.ToString());
                            else
                                filaValoresOrigen.Add("");
                            if (valorOrigen != valorModificado)//&& valorOrigen != null && valorModificado != null)
                            {
                                filaCambios.Add("1");
                            }
                        }
                        if (filaCambios.Count > 0)
                        {
                            MeCambioenvioDTO cambio = new MeCambioenvioDTO();
                            cambio.Cambenvdatos = String.Join(",", filaValores);
                            cambio.Cambenvcolvar = String.Join(",", filaCambios);
                            cambio.Cambenvfecha = (DateTime)reg.Medifecha;
                            cambio.Enviocodi = idEnvio;
                            cambio.Formatcodi = formato.Formatcodi;
                            cambio.Ptomedicodi = reg.Ptomedicodi;
                            cambio.Tipoinfocodi = reg.Tipoinfocodi;
                            cambio.Lastuser = usuario;
                            cambio.Lastdate = DateTime.Now;
                            listaCambio.Add(cambio);
                            /// Si no ha habido cambio se graba el registro original
                            if (ListMeCambioenvios(reg.Ptomedicodi, reg.Tipoinfocodi, formato.Formatcodi, (DateTime)reg.Medifecha).Count == 0)
                            {

                                //DateTime fecha = DateTime.MinValue;
                                //fecha = BuscarEnvioAnterior();
                                //if (formato.FechaProceso == formato.FechaInicio)
                                //    fecha = formato.FechaInicio;
                                //else
                                //    fecha = formato.FechaProceso.AddMonths(-1);
                                var listAux = GetByCriteriaRangoFecha(idEmpresa, formato.Formatcodi, formato.FechaInicio, formato.FechaProceso);
                                for (var fech = formato.FechaInicio; fech <= formato.FechaProceso; fech = fech.AddMonths(1))
                                {
                                    var listaMes = listAux.Where(x => x.Enviofechaperiodo == fech).ToList();
                                    int idEnvioMes = 0;
                                    if (listaMes.Count > 0)
                                    {
                                        idEnvioMes = listaMes.Min(x => x.Enviocodi);
                                        MeCambioenvioDTO origen = new MeCambioenvioDTO();
                                        origen.Cambenvdatos = String.Join(",", filaValoresOrigen);
                                        origen.Cambenvcolvar = "";
                                        origen.Cambenvfecha = (DateTime)reg.Medifecha;
                                        origen.Enviocodi = idEnvioMes;
                                        origen.Formatcodi = formato.Formatcodi;
                                        origen.Ptomedicodi = reg.Ptomedicodi;
                                        origen.Tipoinfocodi = reg.Tipoinfocodi;
                                        origen.Lastuser = usuario;
                                        origen.Lastdate = DateTime.Now;
                                        listaOrigen.Add(origen);
                                    }
                                }
                            }
                        }
                    }
                    if (listaCambio.Count > 0)
                    {//Grabar Cambios
                        GrabarCambios(listaCambio);
                        GrabarCambios(listaOrigen);
                        //si es primer reenvio grabar valores origen

                    }
                }
                //Eliminar Valores Previos
                EliminarValoresCargados1((int)formato.Lectcodi, formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                foreach (MeMedicion1DTO entity in entitys)
                {
                    FactorySic.GetMeMedicion1Repository().Save(entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Lista de mediciones de hidrologia por rango de fecha
        /// </summary>
        /// <param name="lectocodi"></param>
        /// <param name="origlect"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <returns></returns>
        public List<MeMedicion1DTO> ListaMed1Hidrologia(int lectocodi, int origlect, string idsEmpresa, string idsCuenca, string idsFamilia,
            DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion)
        {

            return FactorySic.GetMeMedicion1Repository().GetHidrologia(lectocodi, origlect, idsEmpresa, idsCuenca, idsFamilia,
                fechaInicio, fechaFin, idsPtoMedicion);
        }

        //inicio agregado
        public List<MeMedicion1DTO> ListaPronosticoHidrologia(int reportecodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion1DTO> l = FactorySic.GetMeMedicion1Repository().GetDataPronosticoHidrologia(reportecodi, fechaInicio, fechaFin);
            foreach (var m1 in l)
            {
                m1.Emprnomb = m1.Emprnomb == null ? "" : m1.Emprnomb.Trim();
                m1.Ubicaciondesc = m1.Ubicaciondesc == null ? "" : m1.Ubicaciondesc.Trim();
                m1.CalculadoPtomedidesc = m1.CalculadoPtomedidesc == null ? "" : m1.CalculadoPtomedidesc.Trim();
            }
            return l;
        }

        public List<MeMedicion1DTO> ListaPronosticoHidrologiaByPtoCalculadoAndFecha(int reportecodi, int ptocalculadocodi, DateTime fecha)
        {
            return FactorySic.GetMeMedicion1Repository().GetDataPronosticoHidrologiaByPtoCalculadoAndFecha(reportecodi, ptocalculadocodi, fecha);
        }
        //fin agregado

        /// <summary>
        /// Reporte de Aplicativo Powel - Tipo Semanal
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="reportecodi"></param>
        /// <returns></returns>
        public List<MeMedicion1DTO> ListaSemanalPOwel(DateTime fechaInicio, DateTime fechaFin, int reportecodi)
        {
            List<MeMedicion1DTO> l = FactorySic.GetMeMedicion1Repository().GetDataSemanalPowel(fechaInicio, fechaFin, reportecodi);
            foreach (var m1 in l)
            {
                m1.Emprnomb = m1.Emprnomb == null ? "" : m1.Emprnomb.Trim();
            }
            return l;
        }

        #endregion

        #region Métodos Tabla ME_TIPOPUNTOMEDICION

        /// <summary>
        /// Permite listar todos los registros de la tabla  
        /// </summary>
        public List<MeTipopuntomedicionDTO> ListMeTipopuntomedicions(string origlectcodi)
        {
            if (string.IsNullOrEmpty(origlectcodi)) origlectcodi = ConstantesAppServicio.ParametroDefecto;
            return FactorySic.GetMeTipopuntomedicionRepository().List(origlectcodi);
        }

        #endregion

        #region Métodos Tabla Familia

        /// <summary>
        /// Devuelve lista de familia
        /// </summary>
        /// <returns></returns>
        public List<EqFamiliaDTO> ListarFamilia()
        {
            return FactorySic.GetEqFamiliaRepository().List();
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
        /// Permite realizar búsquedas en la tabla MeFormato
        /// </summary>
        public List<MeFormatoDTO> GetByModuloMeFormatos(int idModulo)
        {
            return FactorySic.GetMeFormatoRepository().GetByModulo(idModulo);
        }

        #endregion

        #region Métodos Tabla ME_HOJAPTOMED

        /// <summary>
        /// Inserta un registro de la tabla ME_HOJAPTOMED
        /// </summary>
        public void SaveMeHojaptomed(MeHojaptomedDTO entity, int empresa)
        {
            try
            {
                FactorySic.GetMeHojaptomedRepository().Save(entity, empresa);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar los puntos de medicion la tabla MeHojaptomed
        /// </summary>
        public List<MeHojaptomedDTO> GetByCriteriaMeHojaptomeds(int emprcodi, int formatcodi, DateTime fechaIni, DateTime fechaFin)
        {
            return FactorySic.GetMeHojaptomedRepository().GetByCriteria(emprcodi, formatcodi, fechaIni, fechaFin);
        }

        /// <summary>
        /// Permite listar los puntos de medicion la tabla MeHojaptomed
        /// </summary>
        public List<MeHojaptomedDTO> GetByCriteria2MeHojaptomeds(int emprcodi, int formatcodi, string query, DateTime fechaIni, DateTime fechaFin)
        {
            return FactorySic.GetMeHojaptomedRepository().GetByCriteria2(emprcodi, formatcodi, query, fechaIni, fechaFin);

        }

        #endregion

        #region Métodos Tabla FW_AREA

        /// <summary>
        /// Permite listar todos los registros de la tabla FW_AREA
        /// </summary>
        public List<FwAreaDTO> ListFwAreas()
        {
            return FactorySic.GetFwAreaRepository().List();
        }

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


        #endregion

        #region Métodos Tabla ME_ESTADOENVIO

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_ESTADOENVIO
        /// </summary>
        public List<MeEstadoenvioDTO> ListMeEstadoenvios()
        {
            return FactorySic.GetMeEstadoenvioRepository().List();
        }

        #endregion

        #region Métodos Tabla ME_AMPLIACIONFECHA

        /// <summary>
        /// Inserta un registro de la tabla ME_AMPLIACIONFECHA
        /// </summary>
        public void SaveMeAmpliacionfecha(MeAmpliacionfechaDTO entity)
        {
            try
            {
                FactorySic.GetMeAmpliacionfechaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_AMPLIACIONFECHA
        /// </summary>
        public void UpdateMeAmpliacionfecha(MeAmpliacionfechaDTO entity)
        {
            try
            {
                FactorySic.GetMeAmpliacionfechaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_AMPLIACIONFECHA
        /// </summary>
        public MeAmpliacionfechaDTO GetByIdMeAmpliacionfecha(DateTime fecha, int empresa, int formato)
        {
            return FactorySic.GetMeAmpliacionfechaRepository().GetById(fecha, empresa, formato);
        }

        /// <summary>
        /// devuelve lista de ampliacion de fechas para listado simple 
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="empresa"></param>
        /// <param name="formato"></param>
        /// <returns></returns>
        public List<MeAmpliacionfechaDTO> ObtenerListaMeAmpliacionfechas(DateTime fechaIni, DateTime fechaFin, int empresa, int formato)
        {

            return FactorySic.GetMeAmpliacionfechaRepository().GetListaAmpliacion(fechaIni, fechaFin, empresa, formato);
        }

        /// <summary>
        /// Devuelve lista de ampliacion de fechas para listado multiple
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="sEmpresa"></param>
        /// <param name="sFormato"></param>
        /// <returns></returns>
        public List<MeAmpliacionfechaDTO> ObtenerListaMultipleMeAmpliacionfechas(DateTime fechaIni, DateTime fechaFin, string sEmpresa, string sFormato)
        {
            if (string.IsNullOrEmpty(sEmpresa)) sEmpresa = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(sFormato)) sFormato = ConstantesAppServicio.ParametroDefecto;
            return FactorySic.GetMeAmpliacionfechaRepository().GetListaMultiple(fechaIni, fechaFin, sEmpresa, sFormato);
        }

        #endregion

        #region Métodos Tabla ME_CAMBIOENVIO

        /// <summary>
        /// Inserta un registro de la tabla ME_CAMBIOENVIO
        /// </summary>
        public void SaveMeCambioenvio(MeCambioenvioDTO entity)
        {
            try
            {
                FactorySic.GetMeCambioenvioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_CAMBIOENVIO
        /// </summary>
        public List<MeCambioenvioDTO> ListMeCambioenvios(int idPto, int idTipoInfo, int idFormato, DateTime fecha)
        {
            return FactorySic.GetMeCambioenvioRepository().List(idPto, idTipoInfo, idFormato, fecha);
        }

        /// <summary>
        /// Lista todos los cambios realizados en un envio
        /// </summary>
        /// <param name="idFormato"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<MeCambioenvioDTO> GetAllCambioEnvio(int idFormato, DateTime fechaInicio, DateTime fechaFin, int idEnvio, int idEmpresa)
        {
            return FactorySic.GetMeCambioenvioRepository().GetAllCambioEnvio(idFormato, fechaInicio, fechaFin, idEnvio, idEmpresa);
        }
        /// <summary>
        /// Permite realizar búsquedas en la tabla MeCambioenvio
        /// </summary>
        public List<MeCambioenvioDTO> GetByCriteriaMeCambioenvios(string idsEmpresa, DateTime fecha, int idFormato, int idEnvio)
        {
            return FactorySic.GetMeCambioenvioRepository().GetByCriteria(idsEmpresa, fecha, idFormato);
        }

        /// <summary>
        /// Graba una lista de cambios 
        /// </summary>
        /// <param name="entitys"></param>
        public void GrabarCambios(List<MeCambioenvioDTO> entitys)
        {
            foreach (var entity in entitys)
                SaveMeCambioenvio(entity);
        }

        /// <summary>
        /// Obtiene todos los datos iniciales de fechas mayores al periodo seleccionado
        /// </summary>
        /// <param name="idFormato"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="fechaPeriodo"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<MeCambioenvioDTO> GetAllOrigenEnvio(int idFormato, DateTime fechaInicio, DateTime fechaFin, DateTime fechaPeriodo, int idEmpresa)
        {
            return FactorySic.GetMeCambioenvioRepository().GetAllOrigenEnvio(idFormato, fechaInicio, fechaFin, fechaPeriodo, idEmpresa);
        }

        #endregion

        #region Métodos Tabla ME_MODULO

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_MODULO
        /// </summary>
        public List<MeModuloDTO> ListMeModulos()
        {
            return FactorySic.GetMeModuloRepository().List();
        }

        #endregion

        #region Métodos Tabla ME_REPORPTOMED

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeReporptomed
        /// </summary>
        public List<MeReporptomedDTO> GetByCriteriaMeReporptomeds(int reporcodi, int ptomedicodi)
        {
            return FactorySic.GetMeReporptomedRepository().GetByCriteria(reporcodi, ptomedicodi);
        }
        /// <summary>
        /// Devuelve lista de encabezado de reporte
        /// </summary>
        /// <param name="reporcodi"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsTipoPtoMed"></param>
        /// <returns></returns>
        public List<MeReporptomedDTO> ListarEncabezadoMeReporptomeds(int reporcodi, string idsEmpresa, string idsTipoPtoMed)
        {

            return FactorySic.GetMeReporptomedRepository().ListarEncabezado(reporcodi, idsEmpresa, idsTipoPtoMed);
        }

        /// <summary>
        /// Devuelve lista de encabezado de reporte powel
        /// </summary>
        /// <param name="reporcodi"></param>       
        /// <returns></returns>
        public List<MeReporptomedDTO> ListarEncabezadoPowel(int reporcodi)
        {
            return FactorySic.GetMeReporptomedRepository().ListarEncabezadoPowel(reporcodi);
        }

        #endregion

        #region Métodos Tabla ME_MEDICIONXINTERVALO

        /// <summary>
        /// Graba una lista de mediciones
        /// </summary>
        /// <param name="lista"></param>
        public void GrabarMedicionesXIntevalo(List<MeMedicionxintervaloDTO> lista)
        {
            try
            {
                foreach (var reg in lista)
                {
                    this.SaveMeMedicionxintervalo(reg);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta un registro de la tabla ME_MEDICIONXINTERVALO
        /// </summary>
        public void SaveMeMedicionxintervalo(MeMedicionxintervaloDTO entity)
        {
            try
            {
                FactorySic.GetMeMedicionxintervaloRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_MEDICIONXINTERVALO
        /// </summary>
        public void UpdateMeMedicionxintervalo(MeMedicionxintervaloDTO entity)
        {
            try
            {
                FactorySic.GetMeMedicionxintervaloRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_MEDICIONXINTERVALO
        /// </summary>
        public List<MeMedicionxintervaloDTO> ListMeMedicionxintervalos()
        {
            return FactorySic.GetMeMedicionxintervaloRepository().List();
        }

        /// <summary>
        /// Permite listar busquedas por rango de fecha, por empresa y tipo de formato de la tabla ME_MEDICIONXINTERVALO
        /// </summary>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicionxintervaloDTO> GetEnvioMedicionXIntervalo(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            return FactorySic.GetMeMedicionxintervaloRepository().GetEnvioArchivo(idFormato, idEmpresa, fechaInicio, fechaFin);
        }

        /// <summary>
        /// Permite eliminar los valores previos cargados
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void EliminarValoresCargados1XInter(int idLectura, int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                FactorySic.GetMeMedicionxintervaloRepository().DeleteEnvioArchivo(idLectura, fechaInicio, fechaFin, idFormato, idEmpresa);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Graba los datos enviados en el formato
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="usuario"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formato"></param>
        public void GrabarMedicionesXIntevalo(List<MeMedicionxintervaloDTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            try
            {
                //Traer Ultimos Valores
                var lista = GetEnvioMedicionXIntervalo(formato.Formatcodi, idEmpresa, formato.FechaProceso, formato.FechaProceso);
                if (lista.Count > 0) // Verificar si hay cambios en el envio
                {
                    var listaCambio = new List<MeCambioenvioDTO>();
                    var listaOrigen = new List<MeCambioenvioDTO>();
                    foreach (var reg in entitys)
                    {
                        var regAnt = lista.Find(x => x.Medintfechaini == reg.Medintfechaini && x.Ptomedicodi == reg.Ptomedicodi &&
                    x.Tipoinfocodi == reg.Tipoinfocodi && x.Lectcodi == reg.Lectcodi);
                        List<string> filaValores = new List<string>();
                        List<string> filaValoresOrigen = new List<string>();
                        List<string> filaCambios = new List<string>();
                        if (regAnt != null)
                        {
                            decimal? valorOrigen = regAnt.Medinth1; // (decimal?)regAnt.GetType().GetProperty("H" + i.ToString()).GetValue(regAnt, null);
                            decimal? valorModificado = reg.Medinth1; //(decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                            if (valorModificado != null)
                                filaValores.Add(valorModificado.ToString());
                            else
                                filaValores.Add("");
                            if (valorOrigen != null)
                                filaValoresOrigen.Add(valorOrigen.ToString());
                            else
                                filaValoresOrigen.Add("");
                            if (valorOrigen != valorModificado)//&& valorOrigen != null && valorModificado != null)
                            {
                                filaCambios.Add("1");
                            }
                        }
                        if (filaCambios.Count > 0)
                        {
                            MeCambioenvioDTO cambio = new MeCambioenvioDTO();
                            cambio.Cambenvdatos = String.Join(",", filaValores);
                            cambio.Cambenvcolvar = String.Join(",", filaCambios);
                            cambio.Cambenvfecha = (DateTime)reg.Medintfechaini;
                            cambio.Enviocodi = idEnvio;
                            cambio.Formatcodi = formato.Formatcodi;
                            cambio.Ptomedicodi = reg.Ptomedicodi;
                            cambio.Tipoinfocodi = reg.Tipoinfocodi;
                            cambio.Lastuser = usuario;
                            cambio.Lastdate = DateTime.Now;
                            listaCambio.Add(cambio);
                            /// Si no ha habido cambio se graba el registro original
                            if (ListMeCambioenvios(reg.Ptomedicodi, reg.Tipoinfocodi, formato.Formatcodi, (DateTime)reg.Medintfechaini).Count == 0)
                            {
                                var listAux = GetByCriteriaRangoFecha(idEmpresa, formato.Formatcodi, formato.FechaInicio, formato.FechaProceso);
                                for (var fech = formato.FechaInicio; fech <= formato.FechaProceso; fech = fech.AddMonths(1))
                                {
                                    var listaMes = listAux.Where(x => x.Enviofechaperiodo == fech).ToList();
                                    int idEnvioMes = 0;
                                    if (listaMes.Count > 0)
                                    {
                                        idEnvioMes = listaMes.Min(x => x.Enviocodi);
                                        MeCambioenvioDTO origen = new MeCambioenvioDTO();
                                        origen.Cambenvdatos = String.Join(",", filaValoresOrigen);
                                        origen.Cambenvcolvar = "";
                                        origen.Cambenvfecha = (DateTime)reg.Medintfechaini;
                                        origen.Enviocodi = idEnvioMes;
                                        origen.Formatcodi = formato.Formatcodi;
                                        origen.Ptomedicodi = reg.Ptomedicodi;
                                        origen.Tipoinfocodi = reg.Tipoinfocodi;
                                        origen.Lastuser = usuario;
                                        origen.Lastdate = DateTime.Now;
                                        listaOrigen.Add(origen);
                                    }
                                }
                            }
                        }
                    }
                    if (listaCambio.Count > 0)
                    {//Grabar Cambios
                        GrabarCambios(listaCambio);
                        GrabarCambios(listaOrigen);
                        //si es primer reenvio grabar valores origen

                    }
                }
                //Eliminar Valores Previos
                EliminarValoresCargados1XInter(formato.Lectcodi, formato.Formatcodi, idEmpresa, formato.FechaProceso, formato.FechaProceso);
                GrabarMedicionesXIntevalo(entitys);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Lista de Mediciones de Descarga y Vertimiento de lagunas
        /// </summary>
        /// <param name="formatCodi"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicionxintervaloDTO> ListaMedIntervaloDescargaVert(int formatCodi, string idsEmpresa, DateTime fechaInicio,
            DateTime fechaFin)
        {
            return FactorySic.GetMeMedicionxintervaloRepository().GetHidrologiaDescargaVert(formatCodi, idsEmpresa, fechaInicio, fechaFin);
        }

        /// <summary>
        /// Lista de Mediciones de Descarga y Vertimiento de lagunas
        /// </summary>
        /// <param name="formatCodi"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicionxintervaloDTO> ListaMedIntervaloDescargaVertPag(int formatCodi, string idsEmpresa, DateTime fechaInicio,
            DateTime fechaFin, int nroPagina, int pageSize)
        {
            return FactorySic.GetMeMedicionxintervaloRepository().GetHidrologiaDescargaVertPag(formatCodi, idsEmpresa, fechaInicio, fechaFin, nroPagina, pageSize);
        }

        /// <summary>
        /// Permite obtener la cantidad de registros con fechas distintas para la paginacion del reporte de descargas y vertimiento
        /// </summary>
        /// <returns></returns>
        public int ObtenerNroFilasDescargVert(int idFormato, string idsEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            int cant;

            var lista1 = ListaMedIntervaloDescargaVert(idFormato, idsEmpresa, fechaInicio, fechaFin);
            cant = lista1.Count();
            return cant;
        }

        #endregion

        #region Métodos Tabla ME_CONFIGFORMATENVIO

        /// <summary>
        /// Permite obtener un registro de la tabla ME_CONFIGFORMATENVIO
        /// </summary>
        public MeConfigformatenvioDTO GetByIdMeConfigformatenvio(int idCfgenv)
        {
            return FactorySic.GetMeConfigformatenvioRepository().GetById(idCfgenv);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeConfigformatenvio
        /// </summary>
        public List<MeConfigformatenvioDTO> GetByCriteriaMeConfigformatenvios(int idEmpresa, int idFormato)
        {
            return FactorySic.GetMeConfigformatenvioRepository().GetByCriteria(idEmpresa, idFormato);
        }

        public int VerificaFormatoUpdate(int idEmpresa, int idFormato, string listaPtos, string listaOrden, string listaTipoinf)
        {
            int idCfg = 0;
            var entity = GetByCriteriaMeConfigformatenvios(idEmpresa, idFormato).FirstOrDefault();
            if (entity != null)
            {
                string ptos = string.Join(",", entity.Cfgenvptos);
                string orden = string.Join(",", entity.Cfgenvorden);
                string tipoinf = string.Join(",", entity.Cfgenvtipoinf);
                if (listaPtos == ptos && listaOrden == orden && tipoinf == listaTipoinf)
                {
                    idCfg = entity.Cfgenvcodi;
                }
            }
            return idCfg;

        }

        #endregion

        #region Métodos Tabla ME_PLAZOPTO

        /// <summary>
        /// Inserta un registro de la tabla ME_PLAZOPTO
        /// </summary>
        public void SaveMePlazopto(MePlazoptoDTO entity)
        {
            try
            {
                FactorySic.GetMePlazoptoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_PLAZOPTO
        /// </summary>
        public void UpdateMePlazopto(MePlazoptoDTO entity)
        {
            try
            {
                FactorySic.GetMePlazoptoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_PLAZOPTO
        /// </summary>
        public void DeleteMePlazopto(int plzptocodi)
        {
            try
            {
                FactorySic.GetMePlazoptoRepository().Delete(plzptocodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_PLAZOPTO
        /// </summary>
        public MePlazoptoDTO GetByIdMePlazopto(int plzptocodi)
        {
            return FactorySic.GetMePlazoptoRepository().GetById(plzptocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_PLAZOPTO
        /// </summary>
        public List<MePlazoptoDTO> ListMePlazoptos()
        {
            return FactorySic.GetMePlazoptoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MePlazopto
        /// </summary>
        public List<MePlazoptoDTO> GetByCriteriaMePlazoptos()
        {
            return FactorySic.GetMePlazoptoRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_PLAZOPTO
        /// </summary>
        public List<MePlazoptoDTO> ListarMePlazoptoParametro(int formatcodi, int ptocodi, int tipoinfocodi)
        {

            List<MePlazoptoDTO> listPlazo = new List<MePlazoptoDTO>();
            var lista = this.ListMePlazoptos().ToList();
            listPlazo = lista.Where(x => x.Ptomedicodi == ptocodi && x.Formatcodi == formatcodi && x.Tipoinfocodi == tipoinfocodi).OrderByDescending(x => x.Plzptofechavigencia).ToList();
            return listPlazo;
        }

        /// <summary>
        /// Listar la configuración de plazo segun la fecha periodo y el formato seleccionado
        /// </summary>
        /// <param name="formatcodi"></param>
        /// <param name="listaHojaPto"></param>
        /// <param name="fechaPeriodo"></param>
        /// <returns></returns>
        public void ListarConfigPlazoXFormatoYFechaPeriodo(int formatcodi, List<MeHojaptomedDTO> listaHojaPto, DateTime fechaPeriodo)
        {
            //Configuracion de plazo
            List<MePlazoptoDTO> listPlazoBD = this.ListMePlazoptos().Where(x => x.Formatcodi == formatcodi).OrderByDescending(x => x.Plzptofechavigencia).ToList();

            foreach (var reg in listaHojaPto)
            {
                MePlazoptoDTO objConfPlazo = listPlazoBD.Find(x => x.Plzptofechavigencia.Value.Date <= fechaPeriodo && x.Ptomedicodi == reg.Ptomedicodi && x.Formatcodi == formatcodi);
                reg.ConfigPto = objConfPlazo;
            }
        }

        #endregion

        #region Metodos Cabecera

        /// <summary>
        /// Devuelve lista de cabeceras de formato
        /// </summary>
        /// <returns></returns>
        public List<MeCabeceraDTO> GetListMeCabecera()
        {
            return FactorySic.GetCabeceraRepository().List();
        }

        #endregion

        #region Reporte de cumplimiento

        public void ObtenerRptCumplimiento(DateTime fInicio, DateTime fFin, string sEmpresas, int idFormato, int idPeriodo
                                        , out List<SiEmpresaDTO> listaEmpresa, out List<MeEnvioDTO> listaEnvio)
        {
            var listaHptoXFmt = FactorySic.GetMeHojaptomedRepository().ListByFormatcodi(idFormato.ToString()).Where(x => x.Hojaptoactivo == 1).ToList();

            //obtener los envios del periodo seleccionado
            var listaEnvioXFmt = FactorySic.GetMeEnvioRepository().ObtenerReporteCumplimiento(ConstantesAppServicio.ParametroDefecto, idFormato, fInicio, fFin);

            //obtener relacion empresa-formato
            List<MeFormatoEmpresaDTO> listaRelEmpXFmt = new List<MeFormatoEmpresaDTO>();
            listaRelEmpXFmt.AddRange(listaHptoXFmt.Select(x => new MeFormatoEmpresaDTO() { Formatcodi = x.Formatcodi, Emprcodi = x.Emprcodi, Emprnomb = x.Emprnomb }).ToList());
            listaRelEmpXFmt.AddRange(listaEnvioXFmt.Select(x => new MeFormatoEmpresaDTO() { Formatcodi = x.Formatcodi.Value, Emprcodi = x.Emprcodi.Value, Emprnomb = x.Emprnomb }).ToList());
            listaRelEmpXFmt = listaRelEmpXFmt.GroupBy(x => new { x.Formatcodi, x.Emprcodi }).Select(x => x.First()).ToList();

            //periodos
            List<DateTime> lista = ListarPeriodoXHorizonte(fInicio, fFin, idPeriodo);

            //procesamiento
            List<MeEnvioDTO> listaRpt = new List<MeEnvioDTO>();
            foreach (var f in lista)
            {
                //formateo de resultado
                foreach (var regConfig in listaRelEmpXFmt)
                {
                    //Reporte de envio
                    MeEnvioDTO regEnvExtranet = listaEnvioXFmt.Find(x => x.Emprcodi == regConfig.Emprcodi && x.Enviofechaperiodo == f);

                    if (regEnvExtranet != null)
                    {
                        listaRpt.Add(regEnvExtranet);
                    }
                    else
                    {
                        MeEnvioDTO regEnvFaltante = new MeEnvioDTO()
                        {
                            Emprnomb = regConfig.Emprnomb,
                            Formatcodi = idFormato,
                            Emprcodi = regConfig.Emprcodi
                        };

                        listaRpt.Add(regEnvFaltante);
                    }
                }
            }

            //filtros
            if (ConstantesAppServicio.ParametroDefecto != sEmpresas)
            {
                List<int> empresas = sEmpresas.Split(',').Select(x => int.Parse(x)).ToList();
                listaRpt = listaRpt.Where(x => empresas.Contains(x.Emprcodi ?? 0)).ToList();
            }

            listaEmpresa = listaRpt.GroupBy(x => x.Emprcodi).Select(x => new SiEmpresaDTO()
            {
                Emprcodi = x.Key ?? 0,
                Emprnomb = x.First().Emprnomb
            }).OrderBy(x => x.Emprnomb).ToList();
            listaEnvio = listaRpt.Where(x => x.Enviocodi > 0).ToList();
        }

        /// <summary>
        /// Genera el View de reportes de Cumplimiento.
        /// </summary>
        /// <returns></returns>
        public string GeneraViewCumplimiento(string sEmpresas, DateTime fInicio, DateTime fFin, int idFormato, int idPeriodo)
        {
            StringBuilder strHtml = new StringBuilder();

            ObtenerRptCumplimiento(fInicio, fFin, sEmpresas, idFormato, idPeriodo, out List<SiEmpresaDTO> listaEmpresa, out List<MeEnvioDTO> listaEnvio);

            strHtml.Append("<table border='1' width:'100%' class='pretty tabla-icono cell-border' cellspacing='0'  id='tabla'>");
            ///Cabecera de Reporte
            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th width='300px'>EMPRESAS</th>");
            int cont = 0;
            string htmlCabecera = GetHtmlCabecera(fInicio, fFin, idPeriodo, ref cont);
            strHtml.Append(htmlCabecera);
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            /// Fin de cabecera de Reporte
            strHtml.Append("<tbody>");
            string colorFondo = string.Empty;
            foreach (var emp in listaEmpresa)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td>" + emp.Emprnomb + "</td>");
                for (int i = 0; i < cont; i++)
                {
                    colorFondo = "style='background-color:orange;color:white'";
                    switch (idPeriodo)
                    {
                        case 1:
                            var find = listaEnvio.Find(x => x.Emprcodi == emp.Emprcodi && x.Enviofechaperiodo == fInicio.AddDays(i));
                            if (find != null)
                            {
                                if (find.Envioplazo == "P")
                                    colorFondo = "style='background-color:SteelBlue;color:white'";
                                strHtml.Append("<td " + colorFondo + ">" + ((DateTime)find.Enviofecha).ToString(ConstantesBase.FormatoFecha) + "</td>");
                            }
                            else
                                strHtml.Append("<td >--</td>");
                            break;
                        case 2:
                            var findS = listaEnvio.Find(x => x.Emprcodi == emp.Emprcodi && x.Enviofechaperiodo == fInicio.AddDays(i * 7));
                            if (findS != null)
                            {
                                if (findS.Envioplazo == "P")
                                    colorFondo = "style='background-color:SteelBlue;color:white'";
                                strHtml.Append("<td " + colorFondo + ">" + ((DateTime)findS.Enviofecha).ToString(ConstantesBase.FormatoFecha) + "</td>");
                            }
                            else
                                strHtml.Append("<td >--</td>");
                            break;
                        case 3:
                        case 5:
                            var find2 = listaEnvio.Find(x => x.Emprcodi == emp.Emprcodi && x.Enviofechaperiodo == fInicio.AddMonths(i));
                            if (find2 != null)
                            {
                                if (find2.Envioplazo == "P")
                                    colorFondo = "style='background-color:SteelBlue;color:white'";
                                strHtml.Append("<td " + colorFondo + "> " + ((DateTime)find2.Enviofecha).ToString(ConstantesBase.FormatoFecha) + "</td>");
                            }
                            else
                                strHtml.Append("<td >--</td>");
                            break;
                        default:
                            strHtml.Append("<td >--</td>");
                            break;
                    }

                }
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            return strHtml.ToString();

        }

        /// <summary>
        /// Genera el View de Reporte de cumplimiento de la Extranet de Hidrología
        /// </summary>
        /// <returns></returns>
        public string GeneraViewReporteCumplimientoHidrologia(string sAreas, string sEmpresas, string mes, DateTime fini, DateTime ffin)
        {
            StringBuilder strHtml = new StringBuilder();
            var fechaIniQuery = fini.AddDays(-7);
            var data = FactorySic.GetMeEnvioRepository().GetListaReporteCumplimientoHidrologia(sAreas, sEmpresas, fechaIniQuery, ffin);
            List<MeEnvioDTO> listaFinal = this.ListarCambioEnvioConfiguracion(data, fini, ffin);

            //listaFinal =  listaFinal.Where(x=>x.Formatcodi == 27).ToList();

            //generar el mes
            mes = mes.Trim().Replace(" ", "-");
            List<DateTime> listaFecha = new List<DateTime>();
            for (var day = fini.Date; day.Date <= ffin.Date; day = day.AddDays(1))
                listaFecha.Add(day);

            //generar las fechas que sí son validables
            List<DateTime> listaFechaPeriodoSemanal = new List<DateTime>();
            for (var day = fini.Date; day.Date <= ffin.Date; day = day.AddDays(1))
            {
                listaFechaPeriodoSemanal.Add(EPDate.f_fechainiciosemana(day));
            }
            listaFechaPeriodoSemanal = listaFechaPeriodoSemanal.Select(x => x.Date).Distinct().ToList();

            strHtml.Append("<table class='pretty tabla-icono'>");

            #region cabecera_reporte
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:120px;' rowspan='4'>EMPRESA</th>");
            strHtml.Append("<th style='width:120px;' rowspan='4'>TIPO DE LECTURA</th>");
            strHtml.Append("<th style='width:120px;' rowspan='4'>FORMATO</th>");
            strHtml.Append("<th style='width:120px;' rowspan='4'>VARIABLE</th>");
            strHtml.Append("<th style='width:120px;' rowspan='4'>DESCRIPCIÓN</th>");
            strHtml.Append("<th style='width:120px;' rowspan='4'>UNIDAD</th>");
            strHtml.Append(string.Format("<th style='width:120px;' rowspan='1' colspan='{0}'>DETALLE REPORTE CUMPLIMIENTO</th>", listaFecha.Count));
            strHtml.Append("<th style='width:120px;' rowspan='2' colspan='3'>CUMPLIMIENTO (%)</th>");
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.Append(string.Format("<th style='width:120px;' colspan='{1}'>{0}</th>", mes, listaFecha.Count));
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            foreach (var f in listaFecha)
            {
                strHtml.Append(string.Format("<th style='width:120px;'>{0}</th>", f.Day));
            }
            strHtml.Append("<th style='width:120px;'>EN PLAZO</th>");
            strHtml.Append("<th style='width:120px;'>FUERA DE PLAZO</th>");
            strHtml.Append("<th style='width:120px;'>NO ENVIO</th>");
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            foreach (var f in listaFecha)
            {
                strHtml.Append(string.Format("<th style='width:120px;'>{0}</th>", EPDate.f_NombreDiaSemanaCorto(f.DayOfWeek).Substring(0, 1)));
            }
            strHtml.Append("<th style='width:120px;'>DÍA</th>");
            strHtml.Append("<th style='width:120px;'>DÍA</th>");
            strHtml.Append("<th style='width:120px;'>DÍA</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");
            #endregion

            #region cuerpo_reporte
            strHtml.Append("<tbody>");
            string htmlVacio = "<td><p></p></td>";
            string htmlEnPlazo = "<td class='enplazo'><p>&#10003;</p></td>";
            string htmlFueraPlazo = "<td class='fueraplazo'><p>&#9745;</p></td>";
            string htmlNoEnvio = "<td class='noenvio'><p>&#10005;</p></td>";

            //empresa
            var listaEmprcodi = listaFinal.Select(x => x.Emprcodi).Distinct().ToList();
            foreach (var empcodi in listaEmprcodi)
            {

                string nomEmpresa = listaFinal.Where(x => x.Emprcodi == empcodi).FirstOrDefault().Emprnomb;
                decimal perCumpEnPlazo = 0;
                decimal perCumpFueraPlazo = 0;
                decimal perCumpNoEnvio = 0;

                int cantPtoPorEmpresa = listaFinal.Where(x => x.Emprcodi == empcodi).Select(y => new { y.Lectcodi, y.Ptomedicodi, y.Formatcodi }).Distinct().Count();
                strHtml.Append("<tr>");


                strHtml.Append(string.Format("<td rowspan='" + cantPtoPorEmpresa + "'>{0}</td>", nomEmpresa));

                //tipo de lectura
                var listaLectcodi = listaFinal.Where(x => x.Emprcodi == empcodi).Select(y => y.Lectcodi).Distinct().ToList();
                for (int lc = 0; lc < listaLectcodi.Count; lc++)
                {
                    if (lc != 0)
                    {
                        strHtml.Append("</tr>");
                        strHtml.Append("<tr>");
                    }
                    var lectcodi = listaLectcodi[lc];
                    var lectura = listaFinal.Where(x => x.Emprcodi == empcodi && x.Lectcodi == lectcodi).FirstOrDefault();
                    var lecttipo = lectura.Lecttipo;
                    string nomTipoLectura = lectura.Lectnomb;
                    int cantPtoPorTipoLectura = listaFinal.Where(x => x.Emprcodi == empcodi && x.Lectcodi == lectcodi).Select(y => new { y.Ptomedicodi, y.Formatcodi }).Distinct().Count();


                    strHtml.Append(string.Format("<td rowspan='" + cantPtoPorTipoLectura + "'>{0}</td>", nomTipoLectura));

                    //formato
                    var listaFormatocodi = listaFinal.Where(x => x.Emprcodi == empcodi && x.Lectcodi == lectcodi).Select(y => y.Formatcodi).Distinct().ToList();
                    for (int fc = 0; fc < listaFormatocodi.Count; fc++)
                    {
                        if (fc != 0)
                        {
                            strHtml.Append("</tr>");
                            strHtml.Append("<tr>");
                        }

                        var formatocodi = listaFormatocodi[fc];
                        var formato = listaFinal.Where(x => x.Emprcodi == empcodi && x.Lectcodi == lectcodi && x.Formatcodi == formatocodi).FirstOrDefault();
                        string nomFormato = formato.Formatnombre;
                        int periodoFormato = formato.Formatperiodo;
                        int resolucionFormato = formato.Formatresolucion.Value;
                        int diaplazoFormato = formato.Formatdiaplazo;
                        int minplazoFormato = formato.Formatminplazo;
                        int cantPtoPorFormato = listaFinal.Where(x => x.Emprcodi == empcodi && x.Lectcodi == lectcodi && x.Formatcodi == formatocodi).Select(y => y.Ptomedicodi).Distinct().Count();



                        strHtml.Append(string.Format("<td rowspan='" + cantPtoPorFormato + "'>{0}</td>", nomFormato));

                        //variable
                        var listaEquicodi = listaFinal.Where(x => x.Emprcodi == empcodi && x.Lectcodi == lectcodi && x.Formatcodi == formatocodi).Select(y => y.Equicodi).Distinct().ToList();
                        for (int ec = 0; ec < listaEquicodi.Count; ec++)
                        {
                            if (ec != 0)
                            {
                                strHtml.Append("</tr>");
                                strHtml.Append("<tr>");
                            }

                            var equicodi = listaEquicodi[ec];
                            string nomEquipo = listaFinal.Where(x => x.Emprcodi == empcodi && x.Lectcodi == lectcodi && x.Formatcodi == formatocodi && x.Equicodi == equicodi).FirstOrDefault().Equinomb;
                            int cantPtoPorVariable = listaFinal.Where(x => x.Emprcodi == empcodi && x.Lectcodi == lectcodi && x.Formatcodi == formatocodi && x.Equicodi == equicodi).Select(y => y.Ptomedicodi).Distinct().Count();

                            strHtml.Append(string.Format("<td rowspan='" + cantPtoPorVariable + "'>{0}</td>", nomEquipo));

                            //Punto de medicion
                            var listaPtocodi = listaFinal.Where(x => x.Emprcodi == empcodi && x.Lectcodi == lectcodi && x.Formatcodi == formatocodi && x.Equicodi == equicodi).Select(y => y.Ptomedicodi).Distinct().ToList();

                            for (int pc = 0; pc < listaPtocodi.Count; pc++)
                            {
                                if (pc != 0)
                                {
                                    strHtml.Append("</tr>");
                                    strHtml.Append("<tr>");
                                }


                                var ptocodi = listaPtocodi[pc];
                                var puntoMedicion = listaFinal.Where(x => x.Emprcodi == empcodi && x.Lectcodi == lectcodi && x.Formatcodi == formatocodi && x.Equicodi == equicodi && x.Ptomedicodi == ptocodi).FirstOrDefault();
                                string nomPto = puntoMedicion.Ptomedidesc;
                                strHtml.Append(string.Format("<td rowspan='1' style='padding-top: 5px;padding-bottom: 5px;'>{0}</td>", nomPto));
                                string unidad = puntoMedicion.TipoInfoabrev;
                                strHtml.Append(string.Format("<td rowspan='1' style='padding-top: 5px;padding-bottom: 5px;'>{0}</td>", unidad));

                                decimal totalEnPlazo = 0;
                                decimal totalFueraPlazo = 0;
                                decimal totalNoEnvio = 0;
                                decimal totalDias = 0;

                                foreach (var f in listaFecha)
                                {
                                    DateTime fecha = f.Date;
                                    var validable = false;
                                    switch (periodoFormato)
                                    {
                                        case 2: //semanal    
                                            DateTime fechaPlazo = EPDate.f_fechainiciosemana(fecha).AddDays(2);
                                            if (fechaPlazo.Date == fecha)
                                            {
                                                validable = true;
                                                fecha = EPDate.f_fechainiciosemana(fecha);
                                            }

                                            break;
                                        case 3://mensual
                                        case 5:
                                            //validable = f.Date == fini.Date;
                                            DateTime fechaPlazo2 = new DateTime(fecha.Year, fecha.Month, 1).AddDays(1);

                                            if (fechaPlazo2.Date == fecha)
                                            {
                                                validable = true;
                                                fecha = new DateTime(fecha.Year, fecha.Month, 1);
                                            }

                                            break;
                                        case 1://diario
                                        default:
                                            validable = true;
                                            break;
                                    }

                                    if (validable)
                                    {
                                        var ptoFechaPeriodo = listaFinal.Where(x => x.Lectcodi == lectcodi && x.Formatcodi == formatocodi && x.Ptomedicodi == ptocodi && x.Enviofechaperiodo != null && x.Enviofechaperiodo.Value.Date == fecha.Date).FirstOrDefault();

                                        if (ptoFechaPeriodo != null && ptoFechaPeriodo.Formatresolucion != -1)

                                        {
                                            if (ptoFechaPeriodo.Envioplazo == "P")
                                            {
                                                //envio en plazo
                                                totalEnPlazo++;
                                                totalDias++;
                                                strHtml.Append(htmlEnPlazo);
                                            }
                                            else
                                            {
                                                //envio fuera de plazo
                                                totalFueraPlazo++;
                                                totalDias++;
                                                strHtml.Append(htmlFueraPlazo);
                                            }
                                        }
                                        else
                                        {
                                            //no envio
                                            totalNoEnvio++;
                                            totalDias++;
                                            strHtml.Append(htmlNoEnvio);
                                        }
                                    }
                                    else
                                    {
                                        //no envio
                                        totalNoEnvio++;
                                        totalDias++;
                                        strHtml.Append(htmlVacio);
                                    }
                                }

                                strHtml.Append(string.Format("<td>{0}</td>", totalEnPlazo));
                                strHtml.Append(string.Format("<td>{0}</td>", totalFueraPlazo));
                                strHtml.Append(string.Format("<td>{0}</td>", totalNoEnvio));

                                if (totalDias > 0)
                                {
                                    perCumpEnPlazo += totalEnPlazo / totalDias;
                                    perCumpFueraPlazo += totalFueraPlazo / totalDias;
                                    perCumpNoEnvio += totalNoEnvio / totalDias;
                                }
                            }
                        }
                    }
                }

                strHtml.Append("</tr>");

                //resumen
                if (cantPtoPorEmpresa > 0)
                {
                    perCumpEnPlazo = (perCumpEnPlazo / (cantPtoPorEmpresa + 0.0m)) * 100.0m;
                    perCumpFueraPlazo = (perCumpFueraPlazo / (cantPtoPorEmpresa + 0.0m)) * 100.0m;
                    perCumpNoEnvio = (perCumpNoEnvio / (cantPtoPorEmpresa + 0.0m)) * 100.0m;
                }
                strHtml.Append("<tr>");
                strHtml.Append(string.Format("<td colspan='{0}'></td>", listaFecha.Count + 6));
                strHtml.Append(string.Format("<td class='per-cumplimiento'>{0:F1}%</td>", perCumpEnPlazo));
                strHtml.Append(string.Format("<td class='per-cumplimiento'>{0:F1}%</td>", perCumpFueraPlazo));
                strHtml.Append(string.Format("<td class='per-cumplimiento'>{0:F1}%</td>", perCumpNoEnvio));
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            #endregion

            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Generar archivo de reporte de cumplimiento
        /// </summary>
        /// <param name="listaDataEmpresa"></param>
        /// <param name="mes"></param>
        /// <param name="fini"></param>
        /// <param name="ffin"></param>
        /// <param name="estadoFecha"></param>
        /// <returns></returns>
        public string GenerarFileExcelReporteCumplimientoExtranet(List<SiEmpresaDTO> listaDataEmpresa, string mes, DateTime fini, DateTime ffin, bool estadoFecha)
        {
            //Solo empresas vigentes
            var listaVigente = FactorySic.GetSiEmpresaRepository().ListarEmpresaVigenteByRango(fini, ffin);
            var listaEmprcodiVig = listaVigente.Select(x => x.Emprcodi).ToList();
            listaDataEmpresa = listaDataEmpresa.Where(x => listaEmprcodiVig.Contains(x.Emprcodi)).ToList();

            List<MeEnvioDTO> data = new List<MeEnvioDTO>();
            var fechaIniQuery = fini.AddDays(-7);
            //area
            int idOrigen = ConstHidrologia.IdOrigenHidro;
            var listaAreas = this.ListAreaXFormato(idOrigen).OrderByDescending(x => x.Areacode).ToList();
            foreach (var a in listaAreas)
            {
                a.Areaname = a.Areaname.Replace("Sub Direccion de", "");
                a.Areaname = a.Areaname.Replace("Sub Direccion ", "");
                a.Areaname = a.Areaname.Trim().ToUpper();
            }
            var listaAreacodi = listaAreas.Select(x => x.Areacode).ToList();
            var listaAreas2 = listaAreas.Select(x => x.Areacode + "").ToList();
            string areacodis = String.Join(",", listaAreas2.ToArray());

            //generar el mes
            mes = mes.Trim().Replace(" ", "-");
            List<DateTime> listaFecha = new List<DateTime>();
            for (var day = fini.Date; day.Date <= ffin.Date; day = day.AddDays(1))
                listaFecha.Add(day);

            //generar las fechas que sí son validables
            List<DateTime> listaFechaPeriodoSemanal = new List<DateTime>();
            for (var day = fini.Date; day.Date <= ffin.Date; day = day.AddDays(1))
            {
                listaFechaPeriodoSemanal.Add(EPDate.f_fechainiciosemana(day));
            }
            listaFechaPeriodoSemanal = listaFechaPeriodoSemanal.Select(x => x.Date).Distinct().ToList();

            //Generar la data de cumplimiento
            foreach (var f in listaFechaPeriodoSemanal)
            {
                List<MeEnvioDTO> dataSemanal = FactorySic.GetMeEnvioRepository().GetListaReporteCumplimientoHidrologia(areacodis, ConstantesAppServicio.ParametroDefecto, f.Date, f.Date.AddDays(6));
                //List<MeEnvioDTO> dataSemanal = FactorySic.GetMeEnvioRepository().GetListaReporteCumplimientoHidrologia(areacodis, "138", f.Date, f.Date.AddDays(6));
                data.AddRange(dataSemanal);
            }
            List<MeEnvioDTO> listaCumplimiento = this.ListarCambioEnvioConfiguracion(data, fini, ffin);

            //generacion de excel
            string fileExcel = string.Empty;
            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Resumen");
                ws.TabColor = Color.Orange;

                #region cabecera resumen
                int row = 7;
                int colIniItem = 2;
                int colIniGenerador = colIniItem + 1;
                int colIniTitulo = colIniGenerador + 1;
                int colFinTitulo = colIniTitulo + 9 - 1;

                ws.Cells[row, colIniItem].Value = "ITEM";
                ws.Cells[row, colIniItem, row + 2, colIniItem].Merge = true;
                ws.Cells[row, colIniItem, row + 2, colIniItem].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[row, colIniGenerador].Value = "GENERADOR INTEGRANTE";
                ws.Cells[row, colIniGenerador, row + 2, colIniGenerador].Merge = true;
                ws.Cells[row, colIniGenerador, row + 2, colIniGenerador].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[row, colIniTitulo].Value = "ESTADO CUMPLIMIENTO " + mes + " (%)";
                ws.Cells[row, colIniTitulo, row, colFinTitulo].Merge = true;
                ws.Cells[row, colIniTitulo, row, colFinTitulo].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                int iniColArea = colIniTitulo;
                var totalArea = listaAreacodi.Count;
                for (int i = 0; i < totalArea; i++)
                {
                    var nomArea = listaAreas.Where(x => x.Areacode == listaAreacodi[i]).FirstOrDefault().Areaname;
                    ws.Cells[row + 1, iniColArea].Value = nomArea;
                    ws.Cells[row + 1, iniColArea, row + 1, iniColArea + 2].Merge = true;
                    ws.Cells[row + 1, iniColArea, row + 1, iniColArea + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells[row + 2, iniColArea].Value = "EN PLAZO";
                    ws.Cells[row + 2, iniColArea].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row + 2, iniColArea + 1].Value = "FUERA DE PLAZO";
                    ws.Cells[row + 2, iniColArea + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row + 2, iniColArea + 2].Value = "NO ENVIO";
                    ws.Cells[row + 2, iniColArea + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    iniColArea += 3;
                }

                using (var range = ws.Cells[row, colIniItem, row + 2, colFinTitulo])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Bold = true;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                #endregion

                row += 3;
                //listaDataEmpresa = listaDataEmpresa.Where(x => x.Emprcodi == 12096).ToList();
                var listaEmpresa = listaDataEmpresa.Select(y => new { y.Emprcodi, y.Emprnomb }).Distinct().ToList().OrderBy(c => c.Emprnomb);
                var listaEmprcodi = listaEmpresa.Select(x => x.Emprcodi).ToList();
                var totalEmpresa = listaEmprcodi.Count;

                for (int j = 0; j < totalEmpresa; j++)
                {
                    var empcodi = listaEmprcodi[j];
                    var listaDataXEmprcodi = listaCumplimiento.Where(x => x.Emprcodi == empcodi).OrderBy(x => x.Lectcodi).ThenBy(x => x.Formatcodi).ThenBy(x => x.Equicodi).ThenBy(x => x.Ptomedicodi).ToList();

                    //lista = lista.Where(x => x.Emprcodi == 12096).ToList();
                    var nomempresa = listaDataEmpresa.Where(x => x.Emprcodi == empcodi).FirstOrDefault().Emprnomb;
                    string[][] matrizCumpl = new string[totalArea][];
                    string nombreHoja = (j + 1) + "_" + nomempresa.Trim();
                    this.GenerarReporteCumplimientoExtranetByEmpresa(xlPackage, listaDataXEmprcodi, listaAreas, nombreHoja, empcodi, nomempresa, mes, fini, listaFecha, listaFechaPeriodoSemanal, matrizCumpl, estadoFecha);

                    ws.Cells[row, colIniItem].Value = j + 1;
                    ws.Cells[row, colIniItem].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells[row, colIniGenerador].Value = nomempresa;
                    ws.Cells[row, colIniGenerador].Style.Border.BorderAround(ExcelBorderStyle.Thin);



                    var colIniMatriz = colIniGenerador + 1;
                    foreach (var ac in listaAreacodi)
                    {


                        string[] matrizDataCumpl = { "", "", "", "" };
                        for (int n = 0; n < matrizCumpl.Length; n++)
                        {
                            if (matrizCumpl[n] != null && matrizCumpl[n][0] == ac + "")
                            {
                                matrizDataCumpl = matrizCumpl[n];
                            }
                        }

                        for (int k = 0; k < 3; k++)
                        {
                            var valor = matrizDataCumpl[k + 1];
                            ws.Cells[row, colIniMatriz + k].Style.Numberformat.Format = "#0.0%";
                            if (valor != "")
                            {
                                ws.Cells[row, colIniMatriz + k].Value = decimal.Parse(valor);
                            }
                            ws.Cells[row, colIniMatriz + k].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[row, colIniMatriz + k].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }

                        colIniMatriz += 3;
                    }

                    row++;
                }

                //ancho de columnas
                ws.Column(1).Width = 5;
                ws.Column(colIniItem).Width = 5;
                ws.Column(colIniGenerador).Width = 50;
                for (int i = 1; i <= totalArea * 3; i++)
                {
                    ws.Column(colIniGenerador + i).Width = 15;
                }
                ws.View.ShowGridLines = false;

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
                picture.From.Column = 0;
                picture.From.Row = 1;
                picture.To.Column = 1;
                picture.To.Row = 2;
                picture.SetSize(120, 60);

                fileExcel = System.IO.Path.GetTempFileName();
                xlPackage.SaveAs(new FileInfo(fileExcel));
            }
            return fileExcel;
        }

        /// <summary>
        /// Generar reporte  cumplimiento
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="data"></param>
        /// <param name="listaArea"></param>
        /// <param name="nombreHoja"></param>
        /// <param name="empcodi"></param>
        /// <param name="empnomb"></param>
        /// <param name="mes"></param>
        /// <param name="fini"></param>
        /// <param name="listaFecha"></param>
        /// <param name="listaFechaPeriodoSemanal"></param>
        /// <param name="arrayCumpl"></param>
        /// <param name="estadoFecha"></param>
        private void GenerarReporteCumplimientoExtranetByEmpresa(ExcelPackage xlPackage, List<MeEnvioDTO> dataCumplimientoXEmpresa, List<FwAreaDTO> listaArea, string nombreHoja, int? empcodi, string empnomb,
            string mes, DateTime fini, List<DateTime> listaFecha, List<DateTime> listaFechaPeriodoSemanal, string[][] arrayCumpl, bool estadoFecha)
        {
            try
            {
                string valorCeldaVacio = "";
                string valorCeldaEnPlazo = "\u2713";
                string valorCeldaFueraPlazo = "\u2611";
                string valorCeldaNoEnvio = "\u2715";
                var cantDia = listaFecha.Count;

                var colIniCabecera = 1;
                var colFinCabecera = colIniCabecera + 5 + cantDia + 3;

                var colIniEmpresa = colIniCabecera;
                var colFinempresa = colIniEmpresa;

                var colIniLectura = colFinempresa + 1;
                var colFinLectura = colIniLectura;

                var colIniFormato = colFinLectura + 1;
                var colFinFormato = colIniFormato;

                var colIniVariable = colFinFormato + 1;
                var colFinVariable = colIniVariable;

                var colIniDescripcion = colFinVariable + 1;
                var colFinDescripcion = colIniDescripcion;

                var colIniUnidad = colFinDescripcion + 1;
                var colFinUnidad = colIniUnidad;

                var colIniTitulo = colFinUnidad + 1;
                var colFinTitulo = colIniTitulo + cantDia - 1;

                var colIniCumpl = colFinTitulo + 1;
                var colFinCumpl = colIniCumpl + 2;

                int row = 5;

                //empresa
                string nomEmpresa = empnomb.Trim();
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHoja);
                ws.TabColor = Color.Green;

                //leyenda
                ws.Cells[2, colIniUnidad].Value = "Dónde:";
                ws.Cells[3, colIniUnidad].Value = valorCeldaEnPlazo;
                ws.Cells[3, colIniUnidad].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[3, colIniUnidad + 1].Value = "EN PLAZO";
                ws.Cells[4, colIniUnidad].Value = valorCeldaFueraPlazo;
                ws.Cells[4, colIniUnidad].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[4, colIniUnidad + 1].Value = "FUERA DE PLAZO";
                ws.Cells[5, colIniUnidad].Value = valorCeldaNoEnvio;
                ws.Cells[5, colIniUnidad].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[5, colIniUnidad + 1].Value = "NO ENVIÓ";

                //area
                var listaAreacodi = listaArea.Select(x => x.Areacode).ToList();
                for (int a = 0; a < listaAreacodi.Count; a++)
                {
                    var areacodi = listaAreacodi[a];
                    var listaDataXAreacodi = dataCumplimientoXEmpresa.Where(x => x.Areacodi == areacodi).ToList();
                    var nomArea = listaArea.Where(x => x.Areacode == areacodi).FirstOrDefault().Areaname;
                    ws.Cells[row, 1].Value = nomArea;
                    ws.Cells[row, 1].Style.Font.Bold = true;

                    row += 2;

                    #region cabecera
                    var filIniCabecera = row;

                    ws.Cells[row, colIniEmpresa].Value = "EMPRESA";
                    ws.Cells[row, colIniEmpresa, row + 3, colFinempresa].Merge = true;
                    ws.Cells[row, colIniEmpresa, row + 3, colFinempresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row, colIniEmpresa, row + 3, colFinempresa].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[row, colIniEmpresa, row + 3, colFinempresa].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[row, colIniLectura].Value = "TIPO DE LECTURA";
                    ws.Cells[row, colIniLectura, row + 3, colFinLectura].Merge = true;
                    ws.Cells[row, colIniLectura, row + 3, colFinLectura].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row, colIniLectura, row + 3, colFinLectura].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[row, colIniLectura, row + 3, colFinLectura].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[row, colIniFormato].Value = "FORMATO";
                    ws.Cells[row, colIniFormato, row + 3, colFinFormato].Merge = true;
                    ws.Cells[row, colIniFormato, row + 3, colFinFormato].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row, colIniFormato, row + 3, colFinFormato].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[row, colIniFormato, row + 3, colFinFormato].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[row, colIniVariable].Value = "VARIABLE";
                    ws.Cells[row, colIniVariable, row + 3, colFinVariable].Merge = true;
                    ws.Cells[row, colIniVariable, row + 3, colFinVariable].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row, colIniVariable, row + 3, colFinVariable].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[row, colIniVariable, row + 3, colFinVariable].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[row, colIniDescripcion].Value = "DESCRIPCIÓN";
                    ws.Cells[row, colIniDescripcion, row + 3, colFinDescripcion].Merge = true;
                    ws.Cells[row, colIniDescripcion, row + 3, colFinDescripcion].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row, colIniDescripcion, row + 3, colFinDescripcion].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[row, colIniDescripcion, row + 3, colFinDescripcion].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[row, colIniUnidad].Value = "UNIDAD";
                    ws.Cells[row, colIniUnidad, row + 3, colFinUnidad].Merge = true;
                    ws.Cells[row, colIniUnidad, row + 3, colFinUnidad].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row, colIniUnidad, row + 3, colFinUnidad].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[row, colIniUnidad, row + 3, colFinUnidad].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[row, colIniTitulo].Value = "DETALLE REPORTE CUMPLIMIENTO";
                    ws.Cells[row, colIniTitulo, row, colFinTitulo].Merge = true;
                    ws.Cells[row, colIniTitulo, row, colFinTitulo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row, colIniTitulo, row, colFinTitulo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[row, colIniTitulo, row, colFinTitulo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[row + 1, colIniTitulo].Value = mes;
                    ws.Cells[row + 1, colIniTitulo, row + 1, colFinTitulo].Merge = true;
                    ws.Cells[row + 1, colIniTitulo, row + 1, colFinTitulo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row + 1, colIniTitulo, row + 1, colFinTitulo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[row + 1, colIniTitulo, row + 1, colFinTitulo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    var iniFecha = colIniTitulo;
                    foreach (var f in listaFecha)
                    {
                        ws.Cells[row + 2, iniFecha].Value = f.Day;
                        ws.Cells[row + 2, iniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[row + 2, iniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        ws.Cells[row + 3, iniFecha].Value = EPDate.f_NombreDiaSemanaCorto(f.DayOfWeek).Substring(0, 1);
                        ws.Cells[row + 3, iniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[row + 3, iniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        iniFecha++;
                    }

                    ws.Cells[row, colIniCumpl].Value = "CUMPLIMIENTO (%)";
                    ws.Cells[row, colIniCumpl, row + 1, colFinCumpl].Merge = true;
                    ws.Cells[row, colIniCumpl, row + 1, colFinCumpl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row, colIniCumpl, row + 1, colFinCumpl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[row, colIniCumpl, row + 1, colFinCumpl].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[row + 2, colIniCumpl].Value = "EN PLAZO";
                    ws.Cells[row + 2, colIniCumpl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row + 2, colIniCumpl + 1].Value = "FUERA DE PLAZO";
                    ws.Cells[row + 2, colIniCumpl + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row + 2, colIniCumpl + 2].Value = "NO ENVIO";
                    ws.Cells[row + 2, colIniCumpl + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells[row + 3, colIniCumpl].Value = "DÍA";
                    ws.Cells[row + 3, colIniCumpl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row + 3, colIniCumpl + 1].Value = "DÍA";
                    ws.Cells[row + 3, colIniCumpl + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[row + 3, colIniCumpl + 2].Value = "DÍA";
                    ws.Cells[row + 3, colIniCumpl + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    using (var range = ws.Cells[filIniCabecera, colIniCabecera, row + 3, colFinCabecera])
                    {
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        range.Style.Font.Color.SetColor(Color.White);
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    #endregion

                    int cantPtoPorEmpresa = listaDataXAreacodi.Select(y => new { y.Lectcodi, y.Ptomedicodi, y.Formatcodi }).Distinct().Count();
                    if (cantPtoPorEmpresa > 0)
                    {
                        row += 4;

                        #region cuerpo

                        decimal perCumpEnPlazo = 0;
                        decimal perCumpFueraPlazo = 0;
                        decimal perCumpNoEnvio = 0;

                        ws.Cells[row, colIniEmpresa].Value = nomEmpresa;
                        ws.Cells[row, colIniEmpresa, row + cantPtoPorEmpresa - 1, colFinempresa].Merge = true;
                        ws.Cells[row, colIniEmpresa, row + cantPtoPorEmpresa - 1, colFinempresa].Style.WrapText = true;
                        ws.Cells[row, colIniEmpresa, row + cantPtoPorEmpresa - 1, colFinempresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[row, colIniEmpresa, row + cantPtoPorEmpresa - 1, colFinempresa].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[row, colIniEmpresa, row + cantPtoPorEmpresa - 1, colFinempresa].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        //tipo de lectura
                        var listaLectcodi = listaDataXAreacodi.Select(y => y.Lectcodi).Distinct().ToList();
                        for (int lc = 0; lc < listaLectcodi.Count; lc++)
                        {
                            if (lc != 0)
                            {
                                row++;
                            }
                            var lectcodi = listaLectcodi[lc];
                            var listaDataXLectcodi = listaDataXAreacodi.Where(x => x.Lectcodi == lectcodi).ToList();
                            var lectura = listaDataXLectcodi.FirstOrDefault();
                            var lecttipo = lectura.Lecttipo;
                            string nomTipoLectura = lectura.Lectnomb;
                            int cantPtoPorTipoLectura = listaDataXLectcodi.Select(y => new { y.Ptomedicodi, y.Formatcodi }).Distinct().Count();

                            ws.Cells[row, colIniLectura].Value = nomTipoLectura;
                            ws.Cells[row, colIniLectura, row + cantPtoPorTipoLectura - 1, colFinLectura].Merge = true;
                            ws.Cells[row, colIniLectura, row + cantPtoPorTipoLectura - 1, colFinLectura].Style.WrapText = true;
                            ws.Cells[row, colIniLectura, row + cantPtoPorTipoLectura - 1, colFinLectura].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[row, colIniLectura, row + cantPtoPorTipoLectura - 1, colFinLectura].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[row, colIniLectura, row + cantPtoPorTipoLectura - 1, colFinLectura].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            //formato
                            var listaFormatocodi = listaDataXLectcodi.Select(y => y.Formatcodi).Distinct().ToList();
                            for (int fc = 0; fc < listaFormatocodi.Count; fc++)
                            {
                                if (fc != 0)
                                {
                                    row++;
                                }

                                var formatocodi = listaFormatocodi[fc];
                                var listaDataXFormatcodi = listaDataXLectcodi.Where(x => x.Formatcodi == formatocodi).ToList();
                                var formato = listaDataXFormatcodi.FirstOrDefault();
                                string nomFormato = formato.Formatnombre;
                                int periodoFormato = formato.Formatperiodo;
                                int resolucionFormato = formato.Formatresolucion.Value;
                                int diaplazoFormato = formato.Formatdiaplazo;
                                int minplazoFormato = formato.Formatminplazo;
                                int cantPtoPorFormato = listaDataXFormatcodi.Select(y => y.Ptomedicodi).Distinct().Count();

                                ws.Cells[row, colIniFormato].Value = nomFormato;
                                ws.Cells[row, colIniFormato, row + cantPtoPorFormato - 1, colFinFormato].Merge = true;
                                ws.Cells[row, colIniFormato, row + cantPtoPorFormato - 1, colFinFormato].Style.WrapText = true;
                                ws.Cells[row, colIniFormato, row + cantPtoPorFormato - 1, colFinFormato].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Cells[row, colIniFormato, row + cantPtoPorFormato - 1, colFinFormato].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                //variable
                                var listaEquicodi = listaDataXFormatcodi.Select(y => y.Equicodi).Distinct().ToList();
                                for (int ec = 0; ec < listaEquicodi.Count; ec++)
                                {
                                    if (ec != 0)
                                    {
                                        row++;
                                    }

                                    var equicodi = listaEquicodi[ec];
                                    var listaDataXEquicodi = listaDataXFormatcodi.Where(x => x.Equicodi == equicodi).ToList();
                                    string nomEquipo = listaDataXEquicodi.FirstOrDefault().Equinomb;
                                    int cantPtoPorVariable = listaDataXEquicodi.Select(y => y.Ptomedicodi).Distinct().Count();

                                    ws.Cells[row, colIniVariable].Value = nomEquipo;
                                    ws.Cells[row, colIniVariable, row + cantPtoPorVariable - 1, colFinVariable].Merge = true;
                                    ws.Cells[row, colIniVariable, row + cantPtoPorVariable - 1, colFinVariable].Style.WrapText = true;
                                    ws.Cells[row, colIniVariable, row + cantPtoPorVariable - 1, colFinVariable].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[row, colIniVariable, row + cantPtoPorVariable - 1, colFinVariable].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //Punto de medicion
                                    var listaPtocodi = listaDataXEquicodi.Select(y => y.Ptomedicodi).Distinct().ToList();
                                    for (int pc = 0; pc < listaPtocodi.Count; pc++)
                                    {
                                        if (pc != 0)
                                        {
                                            row++;
                                        }

                                        var ptocodi = listaPtocodi[pc];
                                        var listaDataXPtomedicodi = listaDataXEquicodi.Where(x => x.Ptomedicodi == ptocodi && x.Enviofechaperiodo != null).OrderBy(x => x.Enviofechaperiodo).ToList();
                                        var puntoMedicion = listaDataXEquicodi.Find(x => x.Ptomedicodi == ptocodi);
                                        string nomPto = puntoMedicion.Ptomedidesc;

                                        ws.Cells[row, colIniDescripcion].Value = nomPto;
                                        ws.Cells[row, colIniDescripcion].Style.WrapText = true;
                                        ws.Cells[row, colIniDescripcion].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                        string unidad = puntoMedicion.TipoInfoabrev;
                                        ws.Cells[row, colIniUnidad].Value = unidad;
                                        ws.Cells[row, colIniUnidad].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        ws.Cells[row, colIniUnidad].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                        decimal totalEnPlazo = 0;
                                        decimal totalFueraPlazo = 0;
                                        decimal totalNoEnvio = 0;
                                        decimal totalDias = 0;

                                        iniFecha = colIniTitulo;
                                        foreach (var f in listaFecha)
                                        {
                                            DateTime fecha = f.Date;
                                            var validable = false;
                                            switch (periodoFormato)
                                            {
                                                case 2: //semanal    
                                                    DateTime fechaPlazo = EPDate.f_fechainiciosemana(fecha).AddDays(2);
                                                    if (fechaPlazo.Date == fecha)
                                                    {
                                                        validable = true;
                                                        fecha = EPDate.f_fechainiciosemana(fecha);
                                                    }

                                                    break;
                                                case 3://mensual
                                                case 5:
                                                    //validable = f.Date == fini.Date;
                                                    DateTime fechaPlazo2 = new DateTime(fecha.Year, fecha.Month, 1).AddDays(1);

                                                    if (fechaPlazo2.Date == fecha)
                                                    {
                                                        validable = true;
                                                        fecha = new DateTime(fecha.Year, fecha.Month, 1);
                                                    }

                                                    break;
                                                case 1://diario
                                                default:
                                                    validable = true;
                                                    break;
                                            }

                                            if (validable)
                                            {
                                                var listaDataXPuntoYFechaPeriodo = listaDataXPtomedicodi.Where(x => x.Enviofechaperiodo.Value.Date == fecha.Date).ToList();
                                                var ptoFechaPeriodo = listaDataXPuntoYFechaPeriodo.FirstOrDefault();

                                                if (ptoFechaPeriodo != null && ptoFechaPeriodo.Formatresolucion != -1)
                                                {
                                                    if (ptoFechaPeriodo.Ptomedicodi == 41670 && fecha.Day == 1)
                                                    {
                                                    }

                                                    if (ptoFechaPeriodo.Envioplazo == "P")
                                                    {
                                                        //envio en plazo
                                                        totalEnPlazo++;
                                                        totalDias++;
                                                        ws.Cells[row, iniFecha].Value = valorCeldaEnPlazo + (estadoFecha ? ptoFechaPeriodo.Enviofecha.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty);
                                                        ws.Cells[row, iniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                                        ws.Cells[row, iniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                                    }
                                                    else
                                                    {
                                                        //envio fuera de plazo
                                                        totalFueraPlazo++;
                                                        totalDias++;
                                                        ws.Cells[row, iniFecha].Value = valorCeldaFueraPlazo + (estadoFecha ? ptoFechaPeriodo.Enviofecha.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty);
                                                        ws.Cells[row, iniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                                        ws.Cells[row, iniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                                    }
                                                }
                                                else
                                                {
                                                    //no envio
                                                    totalNoEnvio++;
                                                    totalDias++;
                                                    ws.Cells[row, iniFecha].Value = valorCeldaNoEnvio;
                                                    ws.Cells[row, iniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                                    ws.Cells[row, iniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                                }
                                            }
                                            else
                                            {
                                                totalNoEnvio++;
                                                totalDias++;
                                                ws.Cells[row, iniFecha].Value = valorCeldaVacio;
                                                ws.Cells[row, iniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                                ws.Cells[row, iniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                            }
                                            iniFecha++;
                                        }

                                        ws.Cells[row, colIniCumpl].Value = totalEnPlazo;
                                        ws.Cells[row, colIniCumpl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        ws.Cells[row, colIniCumpl + 1].Value = totalFueraPlazo;
                                        ws.Cells[row, colIniCumpl + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        ws.Cells[row, colIniCumpl + 2].Value = totalNoEnvio;
                                        ws.Cells[row, colIniCumpl + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                        if (totalDias > 0)
                                        {
                                            perCumpEnPlazo += totalEnPlazo / totalDias;
                                            perCumpFueraPlazo += totalFueraPlazo / totalDias;
                                            perCumpNoEnvio += totalNoEnvio / totalDias;
                                        }
                                    }
                                }


                            }
                        }

                        #endregion

                        row++;
                        #region resumen

                        //resumen
                        if (cantPtoPorEmpresa > 0)
                        {
                            perCumpEnPlazo = (perCumpEnPlazo / (cantPtoPorEmpresa + 0.0m));
                            perCumpFueraPlazo = (perCumpFueraPlazo / (cantPtoPorEmpresa + 0.0m));
                            perCumpNoEnvio = (perCumpNoEnvio / (cantPtoPorEmpresa + 0.0m));
                        }

                        arrayCumpl[a] = new string[4];
                        arrayCumpl[a][0] = areacodi + "";
                        arrayCumpl[a][1] = perCumpEnPlazo + "";
                        arrayCumpl[a][2] = perCumpFueraPlazo + "";
                        arrayCumpl[a][3] = perCumpNoEnvio + "";

                        ws.Cells[row, colIniCumpl].Value = perCumpEnPlazo;
                        ws.Cells[row, colIniCumpl].Style.Numberformat.Format = "#0.0%";
                        ws.Cells[row, colIniCumpl].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[row, colIniCumpl].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        ws.Cells[row, colIniCumpl + 1].Value = perCumpFueraPlazo;
                        ws.Cells[row, colIniCumpl + 1].Style.Numberformat.Format = "#0.0%";
                        ws.Cells[row, colIniCumpl + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[row, colIniCumpl + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        ws.Cells[row, colIniCumpl + 2].Value = perCumpNoEnvio;
                        ws.Cells[row, colIniCumpl + 2].Style.Numberformat.Format = "#0.0%";
                        ws.Cells[row, colIniCumpl + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[row, colIniCumpl + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        using (var range = ws.Cells[row, colIniCumpl, row, colIniCumpl + 2])
                        {
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            range.Style.Font.Color.SetColor(Color.White);
                        }

                        #endregion
                    }

                    row += 5;
                }

                //ancho de columnas
                int ancho = estadoFecha ? 30 : 5;
                ws.Column(colIniEmpresa).Width = 25;
                ws.Column(colIniLectura).Width = 25;
                ws.Column(colIniFormato).Width = 25;
                ws.Column(colIniVariable).Width = 25;
                ws.Column(colIniDescripcion).Width = 65;
                ws.Column(colIniUnidad).Width = 8;
                for (int i = 1; i <= cantDia; i++)
                {
                    ws.Column(colIniUnidad + i).Width = ancho;
                }
                ws.Column(6 + cantDia + 1).Width = 15;
                ws.Column(6 + cantDia + 2).Width = 15;
                ws.Column(6 + cantDia + 3).Width = 15;
                ws.View.ShowGridLines = false;

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
                picture.From.Column = 0;
                picture.From.Row = 1;
                picture.To.Column = 1;
                picture.To.Row = 2;
                picture.SetSize(120, 60);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Te devuelve la lista  actualizada con la configuracion de plazo
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> ListarCambioEnvioConfiguracion(List<MeEnvioDTO> data, DateTime fini, DateTime ffin)
        {
            try
            {
                // Data de empresas vigentes 
                var listaVigente = FactorySic.GetSiEmpresaRepository().ListarEmpresaVigenteByRango(fini, ffin);
                var listaEmprcodiVig = listaVigente.Select(x => x.Emprcodi).ToList();
                data = data.Where(x => listaEmprcodiVig.Contains(x.Emprcodi.Value)).ToList();

                //data = data.Where(x => x.Formatcodi == 25).ToList();

                FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();

                List<MeFormatoDTO> listaFormato = logic.ListMeFormatos();

                //Lista de inicial
                List<MeEnvioDTO> listaData = new List<MeEnvioDTO>();
                //Lista de cambios 
                List<MeEnvioDTO> listaDatosModif = new List<MeEnvioDTO>();

                //Lista de envios
                var listaCumplimiento = data.Where(x => x.Enviocodi != 0 && x.Formatcodi != ConstHidrologia.IdFormatoDescargaLaguna && x.Formatcodi != ConstHidrologia.IdFormatoVertimiento).ToList();
                //Lista de no enviados
                var listaEnvioNoConf = data.Where(x => x.Enviocodi == 0).ToList();
                var listaEnvioNoConfFormatos = data.Where(x => (x.Formatcodi == ConstHidrologia.IdFormatoDescargaLaguna || x.Formatcodi == ConstHidrologia.IdFormatoVertimiento) && x.Enviocodi != 0).ToList();
                //List de fechas
                List<DateTime> listaFecha = new List<DateTime>();
                for (var day = fini.Date; day.Date <= ffin.Date; day = day.AddDays(1))
                    listaFecha.Add(day);

                //generar las fechas que sí son validables
                List<DateTime> listaFechaPeriodoSemanal = new List<DateTime>();
                for (var day = fini.Date; day.Date <= ffin.Date; day = day.AddDays(1))
                {
                    listaFechaPeriodoSemanal.Add(EPDate.f_fechainiciosemana(day));
                }
                listaFechaPeriodoSemanal = listaFechaPeriodoSemanal.Select(x => x.Date).Distinct().ToList();

                //Lista de cambios
                List<MeCambioenvioDTO> listaAllCambio = GetAllCambioEnvio(-1, fini.AddMonths(-1), ffin.AddYears(1), -1, -1);

                //Data Medicion
                List<MeMedicion1DTO> lista1 = FactorySic.GetMeMedicion1Repository().GetEnvioArchivo(-1, -1, fini.AddYears(-1), ffin.AddYears(1))
                    .OrderBy(x => x.Ptomedicodi).ThenBy(x => x.Medifecha).ToList();
                List<MeMedicion24DTO> lista24 = FactorySic.GetMeMedicion24Repository().GetEnvioArchivo(-1, -1, fini.AddDays(-7), ffin)
                    .OrderBy(x => x.Ptomedicodi).ThenBy(x => x.Medifecha).ToList();
                List<MeMedicion48DTO> lista48 = new List<MeMedicion48DTO>();
                List<MeMedicion96DTO> lista96 = new List<MeMedicion96DTO>();

                //Envios
                List<MeEnvioDTO> listaAllEnvio = FactorySic.GetMeEnvioRepository().GetByCriteriaRango(-1, -1, fini.AddYears(-1).Date, ffin.AddYears(1).Date).ToList();

                //Configuracion de plazo
                List<MePlazoptoDTO> listPlazo = this.ListMePlazoptos().OrderByDescending(x => x.Plzptofechavigencia).ToList();

                //empresa
                var listaEmprcodi = listaCumplimiento.Select(x => x.Emprcodi).Distinct().ToList();
                foreach (var empcodi in listaEmprcodi)
                {
                    var listaDataXEmprcodi = listaCumplimiento.Where(x => x.Emprcodi == empcodi).OrderBy(x => x.Lectcodi).ThenBy(x => x.Formatcodi).ThenBy(x => x.Equicodi).ThenBy(x => x.Ptomedicodi).ToList();
                    var listaAllEnvioXEmprcodi = listaAllEnvio.Where(x => x.Emprcodi == empcodi).ToList();
                    var listaLectcodi = listaDataXEmprcodi.Select(y => y.Lectcodi).Distinct().ToList();

                    //tipo de lectura
                    for (int lc = 0; lc < listaLectcodi.Count; lc++)
                    {
                        var lectcodi = listaLectcodi[lc];
                        var listaDataXLectcodi = listaDataXEmprcodi.Where(x => x.Lectcodi == lectcodi).ToList();

                        //formato
                        var listaFormatocodi = listaDataXLectcodi.Select(y => y.Formatcodi).Distinct().ToList();
                        for (int fc = 0; fc < listaFormatocodi.Count; fc++)
                        {
                            var formatocodi = listaFormatocodi[fc];
                            var listaDataXFormatcodi = listaDataXLectcodi.Where(x => x.Formatcodi == formatocodi).ToList();
                            var listaAllEnvioXFormatcodi = listaAllEnvioXEmprcodi.Where(x => x.Formatcodi == formatocodi).ToList();
                            var listaAllCambioXFormatcodi = listaAllCambio.Where(x => x.Formatcodi == formatocodi).ToList();
                            MeFormatoDTO formato = listaFormato.Find(x => x.Formatcodi == formatocodi);

                            //variable
                            var listaEquicodi = listaDataXFormatcodi.Select(y => y.Equicodi).Distinct().ToList();
                            for (int ec = 0; ec < listaEquicodi.Count; ec++)
                            {
                                var equicodi = listaEquicodi[ec];
                                var listaDataXEquicodi = listaDataXFormatcodi.Where(x => x.Equicodi == equicodi).ToList();

                                ////Punto de medicion
                                var listaPtocodi = listaDataXEquicodi.Select(y => y.Ptomedicodi).Distinct().ToList();

                                for (int pc = 0; pc < listaPtocodi.Count; pc++)
                                {
                                    var ptocodi = listaPtocodi[pc];

                                    var lista1Tmp = lista1.Where(x => x.Lectcodi == lectcodi && x.Ptomedicodi == ptocodi).ToList();
                                    var lista24Tmp = lista24.Where(x => x.Lectcodi == lectcodi && x.Ptomedicodi == ptocodi).ToList();
                                    var lista48Tmp = lista48.Where(x => x.Lectcodi == lectcodi && x.Ptomedicodi == ptocodi).ToList();
                                    var lista96Tmp = lista96.Where(x => x.Lectcodi == lectcodi && x.Ptomedicodi == ptocodi).ToList();

                                    // Obtiene el punto medicion
                                    var puntoMedicion = listaDataXEquicodi.Find(x => x.Ptomedicodi == ptocodi && x.Enviocodi != 0);

                                    formato.Formatperiodo = puntoMedicion.Formatperiodo;
                                    var listaFechaValidable = formato.Formatperiodo == 2 ? listaFechaPeriodoSemanal : listaFecha;

                                    foreach (var f in listaFechaValidable)
                                    {
                                        DateTime fecha = f.Date;
                                        var validable = false;
                                        switch (formato.Formatperiodo)
                                        {
                                            case 2: //semanal    
                                                DateTime fechaPlazo = EPDate.f_fechainiciosemana(fecha);
                                                if (fechaPlazo.Date == fecha)
                                                {
                                                    validable = true;
                                                    fecha = EPDate.f_fechainiciosemana(fecha);
                                                }

                                                break;
                                            case 3://mensual
                                            case 5:
                                                //validable = f.Date == fini.Date;
                                                DateTime fechaPlazo2 = new DateTime(fecha.Year, fecha.Month, 1);

                                                if (fechaPlazo2.Date == fecha)
                                                {
                                                    validable = true;
                                                    fecha = new DateTime(fecha.Year, fecha.Month, 1);
                                                }

                                                break;
                                            case 1://diario
                                            default:
                                                validable = true;
                                                break;
                                        }
                                        if (validable)
                                        {
                                            if (puntoMedicion.Ptomedicodi == 40369 && fecha.Day == 15)
                                            {
                                            }
                                            this.SetFormatoWithPlazo(formato, fecha, listPlazo, puntoMedicion.Ptomedicodi);
                                            MePlazoptoDTO objConfPlazo = listPlazo.Find(x => x.Plzptofechavigencia.Value.Date <= fecha && x.Ptomedicodi == ptocodi && x.Formatcodi == formato.Formatcodi);

                                            //Obtiene lista de envios
                                            var listaEnvioXFormatoXDia = listaAllEnvioXFormatcodi.Where(x => x.Enviofechaperiodo.Value.Date == fecha.Date && x.Enviocodi != 0).ToList();
                                            var regPrimerEnvio = listaEnvioXFormatoXDia.OrderByDescending(x => x.Enviocodi).FirstOrDefault();
                                            var regUltimoEnvio = listaEnvioXFormatoXDia.OrderBy(x => x.Enviocodi).FirstOrDefault();

                                            List<int> listaEnviocodi = listaEnvioXFormatoXDia.Select(x => x.Enviocodi).ToList();
                                            List<MeCambioenvioDTO> listaCambioXPunto = listaAllCambioXFormatcodi.Where(x => x.Cambenvfecha.Date >= formato.FechaInicio.Date && x.Cambenvfecha.Date <= formato.FechaFin.Date
                                                                                                                        && x.Ptomedicodi == puntoMedicion.Ptomedicodi && listaEnviocodi.Contains(x.Enviocodi))
                                                                                                                    .OrderByDescending(x => x.Enviocodi).ToList();
                                            // Si no hay cambios toma el primer  envio  en caso contrario toma el ultimo envio
                                            var objUltimoCambio = listaCambioXPunto.FirstOrDefault();
                                            int idEnvioLast = 0;
                                            if (objUltimoCambio != null)
                                            {
                                                idEnvioLast = listaCambioXPunto.Count != 0 ? objUltimoCambio.Enviocodi : regUltimoEnvio.Enviocodi;
                                            }
                                            else
                                            {
                                                idEnvioLast = regPrimerEnvio != null ? regUltimoEnvio.Enviocodi : 0;
                                            }

                                            var regEnvioLast = listaEnvioXFormatoXDia.Find(x => x.Enviocodi == idEnvioLast);
                                            formato.FechaEnvio = regEnvioLast != null ? regEnvioLast.Enviofecha.Value : puntoMedicion.Enviofecha.Value;
                                            formato.IdEnvio = regEnvioLast != null ? regEnvioLast.Enviocodi : puntoMedicion.Enviocodi;

                                            var listaCambio = listaAllCambioXFormatcodi.Where(x => x.Cambenvfecha == fecha.Date && x.Enviocodi == formato.IdEnvio).ToList();
                                            //Lista de de objetos de tipo de formato medicion 
                                            List<object> listaFinal = listaFinal = logic.GetDataFormatoReporte(puntoMedicion.Emprcodi.Value, formato, puntoMedicion.Ptomedicodi, puntoMedicion.Enviocodi, listaCambio, lista1Tmp, lista24Tmp, lista48Tmp, lista96Tmp);

                                            decimal? valor = null;

                                            int cantidadFilas = 0;
                                            foreach (var recEnvio in listaFinal)
                                            {
                                                for (int fila = 1; fila <= formato.RowPorDia; fila++)
                                                {
                                                    valor = (decimal?)recEnvio.GetType().GetProperty(ConstantesAppServicio.CaracterH + fila).GetValue(recEnvio, null);
                                                    cantidadFilas = valor != null ? cantidadFilas + 1 : cantidadFilas + 0;
                                                }
                                            }

                                            var ptoFechaPeriodo = listaDataXEquicodi.Where(x => x.Ptomedicodi == ptocodi && x.Enviofechaperiodo != null && x.Enviofechaperiodo.Value.Date == fecha.Date).FirstOrDefault();
                                            MeEnvioDTO reg = new MeEnvioDTO();
                                            if (ptoFechaPeriodo != null)
                                            {
                                                //Obtiene lista de envios  hasta el envio filtrado
                                                reg = listaDataXEquicodi.Find(x => x.Enviocodi == ptoFechaPeriodo.Enviocodi && x.Ptomedicodi == ptoFechaPeriodo.Ptomedicodi);

                                                //Validacion ordinaria con configuracion de extension  Periodo semanal y mensual
                                                if (objConfPlazo != null)
                                                {
                                                    reg.Envioplazo = regEnvioLast != null ? regEnvioLast.Envioplazo : puntoMedicion.Envioplazo;

                                                    if (reg.Envioplazo == ConstantesEnvio.ENVIO_FUERA_PLAZO)
                                                    {
                                                        reg.Envioplazo = formato.FechaEnvio <= formato.FechaPlazo ? ConstantesEnvio.ENVIO_EN_PLAZO : ConstantesEnvio.ENVIO_FUERA_PLAZO;
                                                    }
                                                    reg.Formatresolucion = cantidadFilas < objConfPlazo.Plzptominfila ? -1 : reg.Formatresolucion;
                                                }
                                                //Validacion ordinaria sin configuracion de extension  Periodo semanal y mensual
                                                else
                                                {
                                                    //Ultimo envio
                                                    //Suma 1 si la fila contiene dato                                                                
                                                    reg.Envioplazo = regEnvioLast != null ? regEnvioLast.Envioplazo : puntoMedicion.Envioplazo;
                                                    if (reg.Envioplazo == ConstantesEnvio.ENVIO_FUERA_PLAZO)
                                                    {
                                                        reg.Envioplazo = formato.FechaEnvio <= formato.FechaPlazo ? ConstantesEnvio.ENVIO_EN_PLAZO : ConstantesEnvio.ENVIO_FUERA_PLAZO;
                                                    }
                                                    //Si estado es false se considera como no enviado
                                                    reg.Formatresolucion = cantidadFilas == 0 ? -1 : reg.Formatresolucion;
                                                }

                                                listaDatosModif.Add(reg);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                listaData.AddRange(listaEnvioNoConf);
                listaData.AddRange(listaDatosModif);
                listaData.AddRange(listaEnvioNoConfFormatos);

                return listaData;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Configuración del Plazo de punto de un formato
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="fechaProceso"></param>
        /// <param name="listPlazo"></param>
        /// <param name="ptomedicodi"></param>
        private void SetFormatoWithPlazo(MeFormatoDTO formato, DateTime fechaProceso, List<MePlazoptoDTO> listPlazo, int ptomedicodi)
        {
            formato.FechaProceso = fechaProceso;

            FormatoMedicionAppServicio.GetSizeFormato(formato);

            var objPuntos = listPlazo.Find(x => x.Plzptofechavigencia.Value.Date <= fechaProceso && x.Ptomedicodi == ptomedicodi && x.Formatcodi == formato.Formatcodi);
            int incrementoDia = objPuntos != null ? objPuntos.Plzptodiafinplazo : 0;
            int incrementominutos = objPuntos != null ? objPuntos.Plzptominfinplazo : 0;
            formato.FechaPlazo = formato.FechaPlazo.AddDays(incrementoDia).AddMinutes(incrementominutos);
        }

        /// <summary>
        /// Genera Archivo del envio 
        /// </summary>
        /// <param name="idEnvio"></param>
        public int GenerarArchivoEnvio(int idEnvio, string ruta)
        {
            int colDatos = 2;
            string[][] listaExcelData;
            MeFormatoDTO formato;
            int resultado = 1;
            var envio = GetByIdMeEnvio(idEnvio);
            if (envio == null)
                return -1;//No existe envio
            if (envio.Formatcodi == null)
                return -2;// No existe Formato
            formato = GetByIdMeFormato((int)envio.Formatcodi);
            var cabecera = GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
            if (cabecera == null)
                return -3;//Cabecera no encontrada
            var empresa = GetByIdSiEmpresa((int)envio.Emprcodi);
            if (empresa == null)
                return -4; // NO existe empresa

            formato.FechaProceso = (DateTime)envio.Enviofechaperiodo;
            string mes = COES.Base.Tools.Util.ObtenerNombreMes(formato.FechaProceso.Month);
            string semana = COES.Base.Tools.Util.GenerarNroSemana(formato.FechaProceso, FirstDayOfWeek.Saturday).ToString();
            FormatoMedicionAppServicio.GetSizeFormato(formato);
            var cabecerasRow = cabecera.Cabcampodef.Split(ConstHidrologia.SeparadorFila);
            List<CabeceraRow> listaCabeceraRow = new List<CabeceraRow>();
            for (var x = 0; x < cabecerasRow.Length; x++)
            {
                var reg = new CabeceraRow();
                var fila = cabecerasRow[x].Split(ConstHidrologia.SeparadorCol);
                reg.NombreRow = fila[0];
                reg.TituloRow = fila[1];
                reg.IsMerge = int.Parse(fila[2]);
                listaCabeceraRow.Add(reg);
            }

            var listaHojaPto = GetByCriteria2MeHojaptomeds((int)envio.Emprcodi, (int)envio.Formatcodi, cabecera.Cabquery, formato.FechaInicio, formato.FechaFin);
            var listaData = GetDataFormato((int)envio.Emprcodi, formato, idEnvio, idEnvio);
            int nCol = listaHojaPto.Count;
            int nBloques = formato.Formathorizonte * formato.RowPorDia;
            listaExcelData = InicializaMatrizExcel(cabecera.Cabfilas, nBloques, cabecera.Cabcolumnas, nCol);
            #region Filas Cabeceras
            List<int> listaColWidth = new List<int>();
            for (var ind = 0; ind < formato.Formatcols; ind++)
            {
                listaColWidth.Add(20);
            }
            string sTitulo = string.Empty;
            string sTituloAnt = string.Empty;
            int column = formato.Formatcols;
            var cellMerge = new CeldaMerge();
            List<CeldaMerge> listaMerge = new List<CeldaMerge>();
            foreach (var reg in listaHojaPto)
            {
                listaColWidth.Add(20);
                for (var w = 0; w < formato.Formatrows; w++)
                {
                    if (column == formato.Formatcols)
                    {
                        listaExcelData[w] = new string[listaHojaPto.Count + formato.Formatrows];
                        listaExcelData[w][0] = listaCabeceraRow[w].TituloRow;
                    }
                    listaExcelData[w][column] = (string)reg.GetType().GetProperty(listaCabeceraRow[w].NombreRow).GetValue(reg, null);
                    if (listaCabeceraRow[w].IsMerge == 1)
                    {
                        if (listaCabeceraRow[w].TituloRowAnt != listaExcelData[w][column])
                        {
                            if (column != formato.Formatcols)
                            {
                                if ((column - listaCabeceraRow[w].ColumnIni) > 1)
                                {
                                    cellMerge = new CeldaMerge();
                                    cellMerge.col = listaCabeceraRow[w].ColumnIni;
                                    cellMerge.row = w;
                                    cellMerge.colspan = (column - listaCabeceraRow[w].ColumnIni);
                                    cellMerge.rowspan = 1;
                                    listaMerge.Add(cellMerge);
                                }
                            }
                            listaCabeceraRow[w].TituloRowAnt = listaExcelData[w][column];
                            listaCabeceraRow[w].ColumnIni = column;
                        }
                    }
                }
                column++;

            }
            if ((column - 1) != formato.Formatcols)
            {
                for (var i = 0; i < listaCabeceraRow.Count; i++)
                {
                    if ((listaCabeceraRow[i].TituloRowAnt == listaExcelData[i][column - 1]))
                    {
                        if ((column - listaCabeceraRow[i].ColumnIni) > 1)
                        {
                            cellMerge = new CeldaMerge();
                            cellMerge.col = listaCabeceraRow[i].ColumnIni;
                            cellMerge.row = i;
                            cellMerge.colspan = (column - listaCabeceraRow[i].ColumnIni);
                            cellMerge.rowspan = 1;
                            listaMerge.Add(cellMerge);
                        }
                    }
                }
            }

            #endregion

            var listaCambios = GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, (int)envio.Emprcodi).Where(x => x.Enviocodi == idEnvio).ToList();
            //ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);
            List<CeldaCambios> listaCellCambios = new List<CeldaCambios>();
            ObtieneMatrizWebExcel(listaExcelData, listaHojaPto, formato, listaData, listaCambios, idEnvio, listaCellCambios);
            string rutaArchivo = ruta + ConstHidrologia.NombreArchivoEnvio;
            string rutaLogo = ruta + ConstHidrologia.NombreArchivoLogo;
            FileInfo newFile = new FileInfo(rutaArchivo);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("Formato");
                ws = xlPackage.Workbook.Worksheets["Formato"];
                AddImage(ws, 0, 0, rutaLogo);
                //Escribe  Nombre Area
                ws.Cells[3, 1].Value = formato.Areaname;
                ws.Cells[5, 1].Value = formato.Formatnombre;

                ///Descripcion del Formato
                /// Nombre de la Empresa
                int row = 6;
                column = 2;

                ws.Cells[row, 1].Value = "Empresa";
                ws.Cells[row + 1, 1].Value = "Año";

                using (var range = ws.Cells[row, 1, row + 4, 2])
                {
                    range.Style.Border.Bottom.Style = range.Style.Border.Left.Style = range.Style.Border.Right.Style = range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                }
                ws.Cells[row, column].Value = empresa.Emprnomb;
                ws.Cells[row + 1, column].Value = envio.Enviofechaperiodo.Value.Year.ToString();
                switch (formato.Formatperiodo)
                {
                    case ParametrosFormato.PeriodoDiario:
                        ws.Cells[row + 2, column - 1].Value = "Mes";
                        ws.Cells[row + 2, column].Value = mes;
                        ws.Cells[row + 3, column - 1].Value = "Día";
                        ws.Cells[row + 3, column].Value = envio.Enviofechaperiodo.Value.Day.ToString();
                        row = row + 3;
                        if (listaHojaPto[0].Famcodi == 43)///Cambiar por Constantes
                        {
                            ws.Cells[row + 4, column - 1].Value = "Caudal";
                            ws.Cells[row + 4, column].Value = "m3/s";
                            row++;
                        }
                        break;
                    case ParametrosFormato.PeriodoSemanal:
                        ws.Cells[row + 2, column - 1].Value = "Semana";
                        ws.Cells[row + 2, column].Value = semana;
                        row = row + 2;
                        if (listaHojaPto[0].Famcodi == 43) //Cambiar
                        {
                            ws.Cells[row + 3, column - 1].Value = "Caudal";
                            ws.Cells[row + 3, column].Value = "m3/s";
                            row++;
                        }
                        break;
                    case 3:
                    case 5:
                        ws.Cells[row + 2, column - 1].Value = "Mes";
                        ws.Cells[row + 2, column].Value = mes;
                        row += 2;
                        if (listaHojaPto[0].Famcodi == 43)//Cambiar
                        {
                            ws.Cells[row + 3, column - 1].Value = "Caudal";
                            ws.Cells[row + 3, column].Value = "m3/s";
                            row++;
                        }
                        break;

                }

                ///Imprimimos cabecera de puntos de medicion
                //row = ConstantesHidrologia.FilaExcelData;
                row += 4;
                int totColumnas = listaHojaPto.Count;
                int columnIni = colDatos;

                for (var i = 0; i <= listaHojaPto.Count; i++)
                {
                    for (var j = 0; j < formato.Formathorizonte * formato.RowPorDia + formato.Formatrows; j++)
                    {
                        decimal valor = 0;
                        bool canConvert = decimal.TryParse(listaExcelData[j][i], out valor);
                        if (canConvert)
                            ws.Cells[row + j, i + 1].Value = valor;
                        else
                            ws.Cells[row + j, i + 1].Value = listaExcelData[j][i];
                        ws.Cells[row + j, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        if (j < cabecera.Cabfilas && i >= cabecera.Cabcolumnas)
                        {
                            ws.Cells[row + j, i + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                            ws.Cells[row + j, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                            ws.Cells[row + j, i + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            ws.Cells[row + j, i + 1].Style.WrapText = true;
                        }
                    }
                }
                /////////////////Formato a Celdas Head ///////////////////
                using (var range = ws.Cells[row, 2, row + cabecera.Cabfilas - 1, listaHojaPto.Count + 1])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
                }
                ////////////// Formato de Celdas Valores
                using (var range = ws.Cells[row + cabecera.Cabfilas, 2, row + formato.Formathorizonte * formato.RowPorDia + cabecera.Cabfilas - 1, listaHojaPto.Count + 1])
                {
                    range.Style.Numberformat.Format = @"0.000";
                }
                for (int x = 1; x <= listaColWidth.Count; x++)
                {
                    ws.Column(x).Width = listaColWidth[x - 1];
                }
                /////////////////////// Celdas Merge /////////////////////

                foreach (var reg in listaMerge)
                {
                    int fili = row + reg.row;
                    int filf = row + reg.row + reg.rowspan - 1;
                    int coli = reg.col + 1;
                    int colf = reg.col + reg.colspan - 1 + 1;
                    ws.Cells[fili, coli, filf, colf].Merge = true;
                }

                xlPackage.Save();
            }

            return resultado;
        }

        /// <summary>
        ///  Inicializa Matriz Excel
        /// </summary>
        /// <param name="rowsHead"></param>
        /// <param name="nFil"></param>
        /// <param name="colsHead"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public string[][] InicializaMatrizExcel(int rowsHead, int nFil, int colsHead, int nCol)
        {
            string[][] matriz = new string[nFil + rowsHead][];
            for (int i = 0; i < nFil; i++)
            {

                matriz[i + rowsHead] = new string[nCol + colsHead];
                for (int j = 0; j < nCol; j++)
                {
                    matriz[i + rowsHead][j + colsHead] = string.Empty;
                }
            }
            return matriz;
        }

        #endregion

        #region Util

        /// <summary>
        /// Configura Formato de numeros al sistema internacional
        /// </summary>
        /// <returns></returns>
        public NumberFormatInfo GenerarNumberFormatInfo()
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            return nfi;
        }

        /// <summary>
        /// Convierte Lista Object a medicion96
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static List<MeMedicion96DTO> Convert96DTO(List<Object> lista)
        {
            List<MeMedicion96DTO> listaFinal = new List<MeMedicion96DTO>();

            foreach (var entity in lista)
            {
                MeMedicion96DTO reg = new MeMedicion96DTO();
                reg.Lectcodi = (int)entity.GetType().GetProperty("Lectcodi").GetValue(entity, null);
                reg.Medifecha = (DateTime)entity.GetType().GetProperty("Medifecha").GetValue(entity, null);
                reg.Ptomedicodi = (int)entity.GetType().GetProperty("Ptomedicodi").GetValue(entity, null);
                reg.Tipoinfocodi = (int)entity.GetType().GetProperty("Tipoinfocodi").GetValue(entity, null);
                for (int i = 1; i <= 96; i++)
                {
                    decimal? valor = (decimal?)entity.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).GetValue(entity, null);
                    reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(reg, valor);
                }
                listaFinal.Add(reg);
            }
            return listaFinal;
        }

        /// <summary>
        /// Convierte Lista Object a medicion48
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static List<MeMedicion48DTO> Convert48DTO(List<Object> lista)
        {
            List<MeMedicion48DTO> listaFinal = new List<MeMedicion48DTO>();

            foreach (var entity in lista)
            {
                MeMedicion48DTO reg = new MeMedicion48DTO();
                reg.Lectcodi = (int)entity.GetType().GetProperty("Lectcodi").GetValue(entity, null);
                reg.Medifecha = (DateTime)entity.GetType().GetProperty("Medifecha").GetValue(entity, null);
                reg.Ptomedicodi = (int)entity.GetType().GetProperty("Ptomedicodi").GetValue(entity, null);
                reg.Tipoinfocodi = (int)entity.GetType().GetProperty("Tipoinfocodi").GetValue(entity, null);
                for (int i = 1; i <= 48; i++)
                {
                    decimal? valor = (decimal?)entity.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).GetValue(entity, null);
                    reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(reg, valor);
                }
                listaFinal.Add(reg);
            }
            return listaFinal;
        }

        /// <summary>
        /// Convierte Lista Object a medicion 24
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static List<MeMedicion24DTO> Convert24DTO(List<Object> lista)
        {
            List<MeMedicion24DTO> listaFinal = new List<MeMedicion24DTO>();

            foreach (var entity in lista)
            {
                MeMedicion24DTO reg = new MeMedicion24DTO();
                reg.Lectcodi = (int)entity.GetType().GetProperty("Lectcodi").GetValue(entity, null);
                reg.Medifecha = (DateTime)entity.GetType().GetProperty("Medifecha").GetValue(entity, null);
                reg.Ptomedicodi = (int)entity.GetType().GetProperty("Ptomedicodi").GetValue(entity, null);
                reg.Tipoinfocodi = (int)entity.GetType().GetProperty("Tipoinfocodi").GetValue(entity, null);
                for (int i = 1; i <= 24; i++)
                {
                    decimal? valor = (decimal?)entity.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).GetValue(entity, null);
                    reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(reg, valor);
                }
                listaFinal.Add(reg);
            }
            return listaFinal;
        }

        /// <summary>
        /// Convierte Lista Object a medicion 1
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static List<MeMedicion1DTO> Convert1DTO(List<Object> lista)
        {
            List<MeMedicion1DTO> listaFinal = new List<MeMedicion1DTO>();
            foreach (var entity in lista)
            {
                MeMedicion1DTO reg = new MeMedicion1DTO();
                reg.Lectcodi = (int)entity.GetType().GetProperty("Lectcodi").GetValue(entity, null);
                reg.Medifecha = (DateTime)entity.GetType().GetProperty("Medifecha").GetValue(entity, null);
                reg.Ptomedicodi = (int)entity.GetType().GetProperty("Ptomedicodi").GetValue(entity, null);
                reg.Tipoinfocodi = (int)entity.GetType().GetProperty("Tipoinfocodi").GetValue(entity, null);
                reg.H1 = (decimal?)entity.GetType().GetProperty("H1").GetValue(entity, null);
                listaFinal.Add(reg);
            }
            return listaFinal;
        }

        /// <summary>
        ///  funcion que genera datos promedios de los valores H1 de un rango de fechas
        /// </summary>
        /// <param name="idLectura"></param>
        /// <param name="IdOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <returns></returns>
        public List<MeMedicion1DTO> GenerarListaDetalladaMed1Promedio(int idLectura, int IdOrigenLectura, string idsEmpresa, string idsCuenca, string idsFamilia,
            DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion, int rbDetalleRpte)
        {
            List<MeMedicion1DTO> listaOut = new List<MeMedicion1DTO>();
            List<MeMedicion1DTO> listaMed1 = new List<MeMedicion1DTO>();

            DateTime fecha = fechaInicio;
            while (fecha <= fechaFin)
            {
                if (rbDetalleRpte == 2 || rbDetalleRpte == 3) // Detalle semanal programado
                {
                    listaMed1 = FactorySic.GetMeMedicion1Repository().GetHidrologia(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa,
                        idsCuenca, idsFamilia, fecha, fecha.AddDays(6), idsPtoMedicion);
                }
                if (rbDetalleRpte == 4) // detalle mensual
                {
                    listaMed1 = FactorySic.GetMeMedicion1Repository().GetHidrologia(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa,
                        idsCuenca, idsFamilia, fecha, fecha.AddMonths(1).AddDays(-1), idsPtoMedicion);
                }
                List<MeMedicion1DTO> agg = listaMed1.GroupBy(t => new { t.Ptomedicodi, t.Equinomb, t.Ptomedibarranomb, t.Tipoinfoabrev })
                                   .Select(g => new MeMedicion1DTO()
                                   {
                                       Ptomedicodi = g.Key.Ptomedicodi,
                                       Equinomb = g.Key.Equinomb,
                                       Ptomedibarranomb = g.Key.Ptomedibarranomb,
                                       Tipoinfoabrev = g.Key.Tipoinfoabrev,
                                       H1 = g.Average(t => t.H1)
                                   }).ToList();
                foreach (var reg in agg)
                {
                    reg.Medifecha = fecha;
                    listaOut.Add(reg);
                }
                if (rbDetalleRpte == 2 || rbDetalleRpte == 3) // Detalle semanal programado
                    fecha = fecha.AddDays(7);
                if (rbDetalleRpte == 4) // Detalle mensual
                    fecha = fecha.AddMonths(1);
            }

            return listaOut;
        }
        /// <summary>
        /// funcion que genera registros de valores H1 de la ultima fecha de cada periodo (Semanal, mensual)
        /// </summary>
        /// <param name="idLectura"></param>
        /// <param name="IdOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <returns></returns>
        public List<MeMedicion1DTO> GenerarListaDetalladaMed1Hmax(int idLectura, int IdOrigenLectura, string idsEmpresa, string idsCuenca, string idsFamilia,
            DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion, int rbDetalleRpte)
        {
            List<MeMedicion1DTO> listaOut = new List<MeMedicion1DTO>();
            List<MeMedicion1DTO> listaMed1 = new List<MeMedicion1DTO>();
            DateTime fechaAux = DateTime.MinValue;
            DateTime fecha = fechaInicio;
            while (fecha <= fechaFin)
            {
                if (rbDetalleRpte == 2 || rbDetalleRpte == 3) // Detalle semanal programado
                {
                    listaMed1 = FactorySic.GetMeMedicion1Repository().GetHidrologia(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa,
                        idsCuenca, idsFamilia, fecha, fecha.AddDays(6), idsPtoMedicion);
                }
                if (rbDetalleRpte == 4) // detalle mensual
                {
                    listaMed1 = FactorySic.GetMeMedicion1Repository().GetHidrologia(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa,
                        idsCuenca, idsFamilia, fecha, fecha.AddMonths(1).AddDays(-1), idsPtoMedicion);
                }
                if (listaMed1.Count > 0)
                {
                    fechaAux = listaMed1.Max(x => x.Medifecha);
                    List<MeMedicion1DTO> agg = listaMed1.Where(x => x.Medifecha == fechaAux).ToList();

                    foreach (var reg in agg)
                    {
                        listaOut.Add(reg);
                    }
                }
                if (rbDetalleRpte == 2 || rbDetalleRpte == 3) // Detalle semanal programado
                    fecha = fecha.AddDays(7);
                if (rbDetalleRpte == 4) // Detalle mensual
                    fecha = fecha.AddMonths(1);
            }

            return listaOut;
        }

        /// <summary>
        /// Genera lista con datos promedio por año de la tabla me_medicion1
        /// </summary>
        /// <param name="idLectura"></param>
        /// <param name="IdOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <returns></returns>
        public List<MeMedicion1DTO> GenerarListaMed1PromAnual(int idLectura, int IdOrigenLectura, string idsEmpresa, string idsCuenca, string idsFamilia, DateTime fechaInicio,
                                                                      DateTime fechaFin, string idsPtoMedicion)
        {
            List<MeMedicion1DTO> listaOut = new List<MeMedicion1DTO>();
            List<MeMedicion1DTO> listaMed1 = new List<MeMedicion1DTO>();
            int anhoIni = fechaInicio.Year;
            int anhoFin = fechaFin.Year;
            for (int i = anhoIni; i <= anhoFin; i++)
            {
                listaMed1 = GenerarListaDetalladaMed1Promedio(idLectura, IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, new DateTime(i, 1, 1),
                                                    new DateTime(i, 12, 31), idsPtoMedicion, 4);
                List<MeMedicion1DTO> agg = listaMed1.GroupBy(t => new { t.Ptomedicodi, t.Equinomb, t.Ptomedibarranomb, t.Tipoinfoabrev })
                                   .Select(g => new MeMedicion1DTO()
                                   {
                                       Ptomedicodi = g.Key.Ptomedicodi,
                                       Equinomb = g.Key.Equinomb,
                                       Ptomedibarranomb = g.Key.Ptomedibarranomb,
                                       Tipoinfoabrev = g.Key.Tipoinfoabrev,
                                       H1 = g.Average(t => t.H1)
                                   }).ToList();
                foreach (var reg in agg)
                {
                    reg.Medifecha = new DateTime(i, 12, 31);
                    listaOut.Add(reg);
                }

            }
            return listaOut;
        }
        /// <summary>
        /// Genera lista con datos por año de la tabla me_medicion1
        /// </summary>
        /// <param name="idLectura"></param>
        /// <param name="IdOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <returns></returns>
        public List<MeMedicion1DTO> GenerarListaMed1AnualHmaximo(int idLectura, int IdOrigenLectura, string idsEmpresa, string idsCuenca, string idsFamilia, DateTime fechaInicio,
                                                                      DateTime fechaFin, string idsPtoMedicion)
        {
            List<MeMedicion1DTO> listaOut = new List<MeMedicion1DTO>();
            List<MeMedicion1DTO> listaMed1 = new List<MeMedicion1DTO>();
            DateTime fecha = DateTime.MinValue;
            int anhoIni = fechaInicio.Year;
            int anhoFin = fechaFin.Year;
            for (int i = anhoIni; i <= anhoFin; i++)
            {
                listaMed1 = GenerarListaDetalladaMed1Hmax(idLectura, IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, new DateTime(i, 1, 1),
                                                    new DateTime(i, 12, 31), idsPtoMedicion, 4);
                fecha = listaMed1.Max(x => x.Medifecha);
                List<MeMedicion1DTO> agg = listaMed1.Where(x => x.Medifecha == fecha).ToList();
                foreach (var reg in agg)
                {
                    listaOut.Add(reg);
                }

            }
            return listaOut;
        }

        /// <summary>
        /// funcion que genera datos promedios de los valores H1 al H24 de un rango de fechas
        /// </summary>
        /// <param name="idLectura"></param>
        /// <param name="IdOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> GenerarListaDetalladaMed24Promedio(int idLectura, int IdOrigenLectura, string idsEmpresa, string idsCuenca, string idsFamilia, DateTime fechaInicio,
                                                                      DateTime fechaFin, string idsPtoMedicion, int rbDetalleRpte)
        {
            List<MeMedicion24DTO> listaOut = new List<MeMedicion24DTO>();
            List<MeMedicion24DTO> listaMed24 = new List<MeMedicion24DTO>();
            List<DateTime> lstFechas = new List<DateTime>();

            DateTime fecha = fechaInicio;
            while (fecha <= fechaFin)
            {
                if (rbDetalleRpte == 1) // Detalle diario)
                    listaMed24 = FactorySic.GetMeMedicion24Repository().GetHidrologia(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fecha, fecha, idsPtoMedicion);
                if (rbDetalleRpte == 2 || rbDetalleRpte == 3) // Detalle semanal programado, cronológico
                {
                    listaMed24 = FactorySic.GetMeMedicion24Repository().GetHidrologia(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fecha, fecha.AddDays(6), idsPtoMedicion);
                }
                if (rbDetalleRpte == 4) // detalle mensual
                {
                    listaMed24 = FactorySic.GetMeMedicion24Repository().GetHidrologia(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fecha, fecha.AddMonths(1).AddDays(-1), idsPtoMedicion);

                }
                lstFechas = listaMed24.Select(x => x.Medifecha).Distinct().ToList();

                List<MeMedicion24DTO> listaAux = new List<MeMedicion24DTO>();
                foreach (var p in listaMed24)
                {
                    //var prom = (p.H1 + p.H2 + p.H3 + p.H4 + p.H5 + p.H6 + p.H7 + p.H8 + p.H9 + p.H10 + 
                    //           p.H11 + p.H12 + p.H13 + p.H14 + p.H15 + p.H16 + p.H17 + p.H18 + p.H19 + 
                    //           p.H20 + p.H21 + p.H22 + p.H23 + p.H24)/24;

                    decimal prom = 0;
                    for (int i = 1; i <= 24; i++)
                    {
                        decimal? valor;
                        valor = (decimal?)p.GetType().GetProperty("H" + i).GetValue(p, null);
                        if (valor != null)
                            prom = prom + (decimal)valor;
                    }
                    prom = prom / 24;

                    var reg = new MeMedicion24DTO()
                    {
                        Ptomedicodi = p.Ptomedicodi,
                        Meditotal = (decimal)prom,
                        Medifecha = p.Medifecha,
                        Famcodi = p.Famcodi,
                        Ptomedibarranomb = p.Ptomedibarranomb,
                        Equinomb = p.Equinomb,
                        Tipoptomedinomb = p.Tipoptomedinomb,
                        Tipoinfoabrev = p.Tipoinfoabrev,
                        Cuenca = p.Cuenca,
                        Emprnomb = p.Emprnomb,
                        Famabrev = p.Famabrev

                    };
                    listaAux.Add(reg);
                }
                // sacamos lista promedio a listaOut y add.
                List<MeMedicion24DTO> agg = listaAux.GroupBy(t => new { t.Ptomedicodi, t.Equinomb, t.Ptomedibarranomb, t.Tipoinfoabrev, t.Tipoptomedinomb, t.Famcodi })
                                  .Select(g => new MeMedicion24DTO()
                                  {
                                      Ptomedicodi = g.Key.Ptomedicodi,
                                      Equinomb = g.Key.Equinomb,
                                      Ptomedibarranomb = g.Key.Ptomedibarranomb,
                                      Tipoptomedinomb = g.Key.Tipoptomedinomb,
                                      Tipoinfoabrev = g.Key.Tipoinfoabrev,
                                      Famcodi = g.Key.Famcodi,
                                      Meditotal = g.Average(t => t.Meditotal)
                                  }).ToList();
                foreach (var reg in agg)
                {
                    reg.Medifecha = lstFechas[0];
                    listaOut.Add(reg);
                }
                if (rbDetalleRpte == 1) //Detalle diario
                    fecha = fecha.AddDays(1);
                if (rbDetalleRpte == 2 || rbDetalleRpte == 3) // Detalle semanal programado, cronologico
                    fecha = fecha.AddDays(7);
                if (rbDetalleRpte == 4) // Detalle mensual
                    fecha = fecha.AddMonths(1);
            }
            return listaOut;
        }
        /// <summary>
        /// Lista de mediciones de hidrologia para maximos del dia
        /// </summary>
        /// <param name="idLectura"></param>
        /// <param name="IdOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> GenerarListaMed24Hmaximo(int idLectura, int IdOrigenLectura, string idsEmpresa, string idsCuenca, string idsFamilia, DateTime fechaInicio,
                                                                      DateTime fechaFin, string idsPtoMedicion, int rbDetalleRpte)
        {
            List<MeMedicion24DTO> listaOut = new List<MeMedicion24DTO>();
            List<MeMedicion24DTO> listaMed24 = new List<MeMedicion24DTO>();
            List<DateTime> lstFechas = new List<DateTime>();

            DateTime fecha = fechaInicio;
            while (fecha <= fechaFin)
            {
                if (rbDetalleRpte == 1) // Detalle diario)
                    listaMed24 = FactorySic.GetMeMedicion24Repository().GetHidrologia(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fecha, fecha, idsPtoMedicion);
                if (rbDetalleRpte == 2 || rbDetalleRpte == 3) // Detalle semanal programado, cronológico
                {
                    listaMed24 = FactorySic.GetMeMedicion24Repository().GetHidrologia(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fecha, fecha.AddDays(6), idsPtoMedicion);
                }
                if (rbDetalleRpte == 4) // detalle mensual
                {
                    listaMed24 = FactorySic.GetMeMedicion24Repository().GetHidrologia(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fecha, fecha.AddMonths(1).AddDays(-1), idsPtoMedicion);

                }
                lstFechas = listaMed24.Select(x => x.Medifecha).Distinct().ToList();

                List<MeMedicion24DTO> listaAux = new List<MeMedicion24DTO>();
                foreach (var p in listaMed24)
                {
                    //decimal valMax = 0;
                    //for (int i = 1; i <= 24; i++)
                    //{
                    //    if (valMax < (decimal)p.GetType().GetProperty("H" + i).GetValue(p, null))
                    //        valMax = (decimal)p.GetType().GetProperty("H" + i).GetValue(p, null);
                    //}
                    var reg = new MeMedicion24DTO()
                    {
                        Ptomedicodi = p.Ptomedicodi,
                        Meditotal = p.H24,
                        Medifecha = p.Medifecha,
                        Famcodi = p.Famcodi,
                        Ptomedibarranomb = p.Ptomedibarranomb,
                        Equinomb = p.Equinomb,
                        Tipoptomedinomb = p.Tipoptomedinomb,
                        Tipoinfoabrev = p.Tipoinfoabrev,
                        Cuenca = p.Cuenca,
                        Emprnomb = p.Emprnomb,
                        Famabrev = p.Famabrev
                    };
                    listaAux.Add(reg);
                }
                // Obtenemos el valor del ultimo dia del periodo (Semanal, mensual)             
                List<MeMedicion24DTO> agg = listaAux.Where(x => x.Medifecha == lstFechas.Max()).ToList();

                foreach (var reg in agg)
                {
                    listaOut.Add(reg);
                }
                if (rbDetalleRpte == 1) //Detalle diario
                    fecha = fecha.AddDays(1);
                if (rbDetalleRpte == 2 || rbDetalleRpte == 3) // Detalle semanal programado, cronologico
                    fecha = fecha.AddDays(7);
                if (rbDetalleRpte == 4) // Detalle mensual
                    fecha = fecha.AddMonths(1);
            }
            return listaOut;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idLectura"></param>
        /// <param name="IdOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> GenerarListaMed24AnualHmaximo(int idLectura, int IdOrigenLectura, string idsEmpresa, string idsCuenca,
            string idsFamilia, DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion)
        {
            List<MeMedicion24DTO> listaOut = new List<MeMedicion24DTO>();
            List<MeMedicion24DTO> listaAux = new List<MeMedicion24DTO>();
            int anhoIni = fechaInicio.Year;
            int anhoFin = fechaFin.Year;
            DateTime fecha = DateTime.MinValue;

            for (int i = anhoIni; i <= anhoFin; i++)
            {
                listaAux = GenerarListaMed24Hmaximo(idLectura, IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, new DateTime(i, 1, 1),
                                                    new DateTime(i, 12, 31), idsPtoMedicion, 4);
                fecha = listaAux.Max(x => x.Medifecha);
                List<MeMedicion24DTO> agg = listaAux.Where(x => x.Medifecha == fecha).ToList();

                //List<MeMedicion24DTO> agg = listaAux.GroupBy(t => new { t.Ptomedicodi, t.Equinomb, t.Ptomedibarranomb, t.Tipoinfoabrev, t.Tipoptomedinomb, t.Famcodi })
                //                  .Select(g => new MeMedicion24DTO()
                //                  {
                //                      Ptomedicodi = g.Key.Ptomedicodi,
                //                      Equinomb = g.Key.Equinomb,
                //                      Ptomedibarranomb = g.Key.Ptomedibarranomb,
                //                      Tipoptomedinomb = g.Key.Tipoptomedinomb,
                //                      Tipoinfoabrev = g.Key.Tipoinfoabrev,
                //                      Famcodi = g.Key.Famcodi,
                //                      Meditotal = g.Average(t => t.Meditotal)
                //                  }).ToList();
                foreach (var reg in agg)
                {
                    //reg.Medifecha = new DateTime(i, 12, 31);
                    listaOut.Add(reg);
                }
            }
            return listaOut;

        }

        /// <summary>
        /// Genera lista con datos promedio por año de la tabla me_medicion24
        /// </summary>
        /// <param name="idLectura"></param>
        /// <param name="IdOrigenLectura"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> GenerarListaMed24PromAnual(int idLectura, int IdOrigenLectura, string idsEmpresa, string idsCuenca, string idsFamilia,
            DateTime fechaInicio, DateTime fechaFin, string idsPtoMedicion)
        {
            List<MeMedicion24DTO> listaOut = new List<MeMedicion24DTO>();
            List<MeMedicion24DTO> listaAux = new List<MeMedicion24DTO>();
            int anhoIni = fechaInicio.Year;
            int anhoFin = fechaFin.Year;

            for (int i = anhoIni; i <= anhoFin; i++)
            {
                listaAux = GenerarListaDetalladaMed24Promedio(idLectura, IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, new DateTime(i, 1, 1),
                                                    new DateTime(i, 12, 31), idsPtoMedicion, 4);

                List<MeMedicion24DTO> agg = listaAux.GroupBy(t => new { t.Ptomedicodi, t.Equinomb, t.Ptomedibarranomb, t.Tipoinfoabrev, t.Tipoptomedinomb, t.Famcodi })
                                  .Select(g => new MeMedicion24DTO()
                                  {
                                      Ptomedicodi = g.Key.Ptomedicodi,
                                      Equinomb = g.Key.Equinomb,
                                      Ptomedibarranomb = g.Key.Ptomedibarranomb,
                                      Tipoptomedinomb = g.Key.Tipoptomedinomb,
                                      Tipoinfoabrev = g.Key.Tipoinfoabrev,
                                      Famcodi = g.Key.Famcodi,
                                      Meditotal = g.Average(t => t.Meditotal)
                                  }).ToList();
                foreach (var reg in agg)
                {
                    reg.Medifecha = new DateTime(i, 12, 31);
                    listaOut.Add(reg);
                }
            }
            return listaOut;
        }


        public string GetNombrePeriodo(DateTime fecha, int periodo)
        {
            string fechaPeriodo = string.Empty;
            switch (periodo)
            {
                case 1:
                    fechaPeriodo = fecha.ToString(ConstantesBase.FormatoFecha);
                    break;
                case 2:
                    fechaPeriodo = fecha.Year.ToString() + " Sem " + EPDate.f_numerosemana(fecha);
                    break;
                case 3:
                case 5:
                    fechaPeriodo = fecha.Year.ToString() + " " + EPDate.f_NombreMes(fecha.Month);
                    break;
            }
            return fechaPeriodo;
        }

        /// <summary>
        /// Devuelve string html de la cebecera de reportes de cumplimiento
        /// </summary>
        /// <param name="fInicio"></param>
        /// <param name="fFin"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="cont"></param>
        /// <returns></returns>
        public string GetHtmlCabecera(DateTime fInicio, DateTime fFin, int idPeriodo, ref int cont)
        {
            StringBuilder strHtml = new StringBuilder();

            List<DateTime> lista = ListarPeriodoXHorizonte(fInicio, fFin, idPeriodo);

            foreach (var f in lista)
            {
                strHtml.Append("<th>" + GetNombrePeriodo(f, idPeriodo) + "</th>");
                cont++;
            }

            return strHtml.ToString();
        }

        public List<DateTime> ListarPeriodoXHorizonte(DateTime fInicio, DateTime fFin, int idPeriodo)
        {
            List<DateTime> lista = new List<DateTime>();

            switch (idPeriodo)
            {
                case 1:
                    for (var f = fInicio; f <= fFin; f = f.AddDays(1))
                    {
                        lista.Add(f);
                    }
                    break;
                case 2:
                    for (var f = fInicio; f <= fFin; f = f.AddDays(7))
                    {
                        lista.Add(f);
                    }
                    break;
                case 3:
                case 5:
                    for (var f = fInicio; f <= fFin; f = f.AddMonths(1))
                    {
                        lista.Add(f);
                    }
                    break;
            }

            return lista;
        }

        /// <summary>
        /// Obtiene Cabecera de reporte excel de cumplimiento
        /// </summary>
        /// <param name="fInicio"></param>
        /// <param name="fFin"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="cont"></param>
        /// <returns></returns>
        private void GetExcelCabecera(ExcelWorksheet ws, DateTime fInicio, DateTime fFin, int idPeriodo, ref int cont)
        {
            int col = 2;
            ws.Cells[5, col].Value = "Empresa/Fecha";
            col++;
            switch (idPeriodo)
            {
                case 1:
                    for (var f = fInicio; f <= fFin; f = f.AddDays(1))
                    {
                        ws.Cells[5, col].Value = GetNombrePeriodo(f, idPeriodo);
                        ws.Cells[5, col].AutoFitColumns();
                        cont++;
                        col++;
                    }
                    break;
                case 2:
                    for (var f = fInicio; f <= fFin; f = f.AddDays(7))
                    {
                        ws.Cells[5, col].Value = GetNombrePeriodo(f, idPeriodo);
                        ws.Cells[5, col].AutoFitColumns();
                        col++;
                        cont++;
                    }
                    break;
                case 3:
                case 5:
                    for (var f = fInicio; f <= fFin; f = f.AddMonths(1))
                    {
                        ws.Cells[5, col].Value = GetNombrePeriodo(f, idPeriodo);
                        ws.Cells[5, col].AutoFitColumns();
                        col++;
                        cont++;
                    }
                    break;
            }
        }

        /// <summary>
        /// Lista de cumplimiento de envios de hidrologia,
        /// </summary>
        /// <param name="sEmpresas"></param>
        /// <param name="fInicio"></param>
        /// <param name="fFin"></param>
        /// <param name="idFormato"></param>
        /// <param name="nombreFormato"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="rutaArchivo"></param>
        /// <param name="rutaLogo"></param>
        public void GeneraExcelCumplimiento(string sEmpresas, DateTime fInicio, DateTime fFin, int idFormato, string nombreFormato, int idPeriodo, string rutaArchivo,
            string rutaLogo)
        {
            int cont = 0;
            FileInfo newFile = new FileInfo(rutaArchivo);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaArchivo);
            }
            ObtenerRptCumplimiento(fInicio, fFin, sEmpresas, idFormato, idPeriodo, out List<SiEmpresaDTO> listaEmpresa, out List<MeEnvioDTO> listaEnvio);

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("Cumplimiento");
                ws = xlPackage.Workbook.Worksheets["Cumplimiento"];
                AddImage(ws, 1, 0, rutaLogo);
                var fontTabla = ws.Cells[3, 2].Style.Font;
                fontTabla.Size = 14;
                fontTabla.Bold = true;
                ws.Cells[3, 2].Value = "REPORTE CUMPLIMIENTO:";
                ws.Cells[3, 3].Value = nombreFormato;
                GetExcelCabecera(ws, fInicio, fFin, idPeriodo, ref cont);
                int filLeyenda = listaEmpresa.Count() + 6;
                ws.Cells[filLeyenda + 1, 2].Value = "LEYENDA:";
                ws.Cells[filLeyenda + 2, 2].Value = "EN PLAZO";
                ws.Cells[filLeyenda + 3, 2].Value = "FUERA DE PLAZO";
                var borderLeyenda = ws.Cells[filLeyenda + 1, 2, filLeyenda + 3, 2].Style.Border;
                borderLeyenda.Bottom.Style = borderLeyenda.Top.Style = borderLeyenda.Left.Style = borderLeyenda.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[filLeyenda + 2, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[filLeyenda + 2, 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(70, 130, 180));
                ws.Cells[filLeyenda + 3, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[filLeyenda + 3, 2].Style.Fill.BackgroundColor.SetColor(Color.Orange);
                ws.Cells[filLeyenda + 2, 2].Style.Font.Color.SetColor(Color.White);
                ws.Cells[filLeyenda + 3, 2].Style.Font.Color.SetColor(Color.White);

                int fila = 6;
                int col = 2;
                string colorFondo = string.Empty;
                foreach (var emp in listaEmpresa)
                {
                    ws.Cells[fila, col].Value = emp.Emprnomb;
                    for (int i = 0; i < cont; i++)
                    {
                        colorFondo = "style='background-color:orange;color:white'";
                        switch (idPeriodo)
                        {
                            case 1:
                                var find = listaEnvio.Find(x => x.Emprcodi == emp.Emprcodi && x.Enviofechaperiodo == fInicio.AddDays(i));
                                if (find != null)
                                {
                                    ws.Cells[fila, col + i + 1].Value = ((DateTime)find.Enviofecha).ToString(ConstantesBase.FormatoFecha);
                                    if (find.Envioplazo == "P")
                                    {
                                        ws.Cells[fila, col + i + 1].StyleID = ws.Cells[filLeyenda + 2, 2].StyleID;
                                    }
                                    else
                                    {
                                        ws.Cells[fila, col + i + 1].StyleID = ws.Cells[filLeyenda + 3, 2].StyleID;
                                    }
                                }
                                break;
                            case 2:
                                var findS = listaEnvio.Find(x => x.Emprcodi == emp.Emprcodi && x.Enviofechaperiodo == fInicio.AddDays(i * 7));
                                if (findS != null)
                                {
                                    ws.Cells[fila, col + i + 1].Value = ((DateTime)findS.Enviofecha).ToString(ConstantesBase.FormatoFecha);
                                    if (findS.Envioplazo == "P")
                                    {
                                        ws.Cells[fila, col + i + 1].StyleID = ws.Cells[filLeyenda + 2, 2].StyleID;
                                    }
                                    else
                                    {
                                        ws.Cells[fila, col + i + 1].StyleID = ws.Cells[filLeyenda + 3, 2].StyleID;
                                    }
                                }
                                break;
                            case 3:
                            case 5:
                                var find2 = listaEnvio.Find(x => x.Emprcodi == emp.Emprcodi && x.Enviofechaperiodo == fInicio.AddMonths(i));
                                if (find2 != null)
                                {
                                    ws.Cells[fila, col + i + 1].Value = ((DateTime)find2.Enviofecha).ToString(ConstantesBase.FormatoFecha);
                                    if (find2.Envioplazo == "P")
                                    {
                                        ws.Cells[fila, col + i + 1].StyleID = ws.Cells[filLeyenda + 2, 2].StyleID;
                                    }
                                    else
                                    {
                                        ws.Cells[fila, col + i + 1].StyleID = ws.Cells[filLeyenda + 3, 2].StyleID;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    fila++;
                }
                // borde de region de datos
                var borderReg = ws.Cells[5, 2, fila - 1, cont + 2].Style.Border;
                borderReg.Bottom.Style = borderReg.Top.Style = borderReg.Left.Style = borderReg.Right.Style = ExcelBorderStyle.Thin;
                using (ExcelRange r = ws.Cells[5, 2, 5, cont + 2])
                {
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(70, 130, 210));
                }
                using (ExcelRange r = ws.Cells[6, 2, fila - 1, 2])
                {
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(173, 216, 230));
                }
                ws.Column(col).AutoFit();
                xlPackage.Save();
            }


        }

        /// <summary>
        /// Genera Encabezao de reporte Excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="titulo"></param>
        /// <param name="ruta"></param>
        public void ConfigEncabezadoExcel(ExcelWorksheet ws, string titulo, string ruta)
        {
            AddImage(ws, 1, 0, ruta);

            var fontTabla = ws.Cells[3, 2].Style.Font;
            fontTabla.Size = 15;
            //            fontTabla.Name = "Calibri";
            fontTabla.Bold = true;
            ws.Cells[3, 2].Value = "REPORTE CUMPLIMIENTO:";
            //ws.Cells[3, 3].Value = 

        }

        /// <summary>
        /// Permite exportar los perfiles almacenados en base de datos
        /// </summary>
        /// <param name="list"></param>
        ///         
        private void AddImage(ExcelWorksheet ws, int columnIndex, int rowIndex, string filePath)
        {
            //How to Add a Image using EP Plus
            Bitmap image = new Bitmap(filePath);

            ExcelPicture picture = null;
            if (image != null)
            {
                picture = ws.Drawings.AddPicture("pic" + rowIndex.ToString() + columnIndex.ToString(), image);
                picture.From.Column = columnIndex;
                picture.From.Row = rowIndex;
                picture.From.ColumnOff = Pixel2MTU(1); //Two pixel space for better alignment
                picture.From.RowOff = Pixel2MTU(1);//Two pixel space for better alignment
                picture.SetSize(120, 40);

            }
        }

        /// <summary>
        /// Deterina ancho en pixeles para el logo
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public int Pixel2MTU(int pixels)
        {
            //convert pixel to MTU
            int MTU_PER_PIXEL = 9525;
            int mtus = pixels * MTU_PER_PIXEL;
            return mtus;
        }

        #endregion

        #region Metodos Formato


        /// <summary>
        /// Obtiene fila de de registro de cambio
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="resolucion"></param>
        /// <param name="indiceBloque"></param>
        /// <param name="rowPorDia"></param>
        /// <param name="fechaCambio"></param>
        /// <param name="fechaInicio"></param>
        /// <returns></returns>
        public int ObtieneRowChange(int periodo, int resolucion, int indiceBloque, int rowPorDia, DateTime fechaCambio, DateTime fechaInicio)
        {
            int row = 0;
            switch (periodo)
            {
                case ParametrosFormato.PeriodoMensual:
                    switch (resolucion)
                    {
                        case ParametrosFormato.ResolucionMes:
                            row = ((fechaCambio.Year - fechaInicio.Year) * 12) + fechaCambio.Month - fechaInicio.Month;
                            break;
                        default:
                            row = indiceBloque + rowPorDia * (fechaCambio - fechaInicio).Days - 1;
                            break;
                    }
                    break;
                default:
                    row = indiceBloque + rowPorDia * (fechaCambio - fechaInicio).Days - 1;
                    break;
            }

            return row;
        }


        /// <summary>
        /// Devuelve la fecha del siguiente bloque
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="resolucion"></param>
        /// <param name="horizonte"></param>
        /// <param name="fechaInicio"></param>
        /// <returns></returns>
        public DateTime GetNextFilaHorizonte(int periodo, int resolucion, int horizonte, DateTime fechaInicio)
        {
            DateTime resultado = DateTime.MinValue;
            switch (periodo)
            {
                case ParametrosFormato.PeriodoMensual:
                    switch (resolucion)
                    {
                        case ParametrosFormato.ResolucionMes:
                            resultado = fechaInicio.AddMonths(horizonte);
                            break;

                        default:
                            resultado = fechaInicio.AddDays(horizonte);
                            break;
                    }
                    break;
                case ParametrosFormato.PeriodoMensualSemana:
                    resultado = fechaInicio.AddDays(horizonte * 7);
                    break;
                default:
                    resultado = fechaInicio.AddDays(horizonte);
                    break;
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene el nombre de la celda fechaa mostrarse en los formatos excel.
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="resolucion"></param>
        /// <param name="horizonte"></param>
        /// <param name="indice"></param>
        /// <param name="fechaInicio"></param>
        /// <returns></returns>
        public string ObtenerCeldaFecha(int periodo, int resolucion, int tipoLectura, int horizonte, int indice, DateTime fechaInicio)
        {
            string resultado = string.Empty;
            switch (periodo)
            {
                case ParametrosFormato.PeriodoMensual:
                    switch (resolucion)
                    {
                        case ParametrosFormato.ResolucionMes:
                            if (tipoLectura == ParametrosFormato.Ejecutado)
                                resultado = fechaInicio.Year.ToString() + " " + COES.Base.Tools.Util.ObtenerNombreMesAbrev(horizonte + 1);
                            else
                            {
                                resultado = fechaInicio.AddMonths(horizonte).Year.ToString() + " " + COES.Base.Tools.Util.ObtenerNombreMesAbrev(fechaInicio.AddMonths(horizonte).Month);
                            }
                            break;
                        default:
                            resultado = fechaInicio.AddMinutes(horizonte * ParametrosFormato.ResolucionDia + resolucion * indice).ToString(ConstantesAppServicio.FormatoFechaHora);
                            break;
                    }
                    break;
                case ParametrosFormato.PeriodoMensualSemana:
                    int semana = COES.Base.Tools.Util.ObtenerNroSemanasxAnho(fechaInicio.AddDays(horizonte * 7), 6);
                    string stSemana = (semana > 9) ? semana.ToString() : "0" + semana.ToString();
                    if (tipoLectura == ParametrosFormato.Ejecutado)
                    {

                        resultado = fechaInicio.AddDays(horizonte * 7).Year.ToString() + " Sem:" + stSemana;
                    }
                    else
                    {
                        resultado = fechaInicio.AddDays(horizonte * 7).Year.ToString() + " Sem:" + stSemana;
                    }
                    break;
                default:
                    resultado = fechaInicio.AddMinutes(horizonte * ParametrosFormato.ResolucionDia + resolucion * indice).ToString(ConstantesAppServicio.FormatoFechaHora);
                    break;
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene la matriz de datos en base al envio para generar el excel
        /// </summary>
        /// <param name="listaExcelData"></param>
        /// <param name="listaHojaPto"></param>
        /// <param name="formato"></param>
        /// <param name="lista"></param>
        /// <param name="listaCambios"></param>
        /// <param name="idEnvio"></param>
        /// <param name="listaCellCambios"></param>
        public void ObtieneMatrizWebExcel(string[][] listaExcelData, List<MeHojaptomedDTO> listaHojaPto, MeFormatoDTO formato, List<object> lista, List<MeCambioenvioDTO> listaCambios, int idEnvio, List<CeldaCambios> listaCellCambios)
        {
            foreach (var reg in listaCambios)
            {
                if (reg.Cambenvcolvar != null)
                {
                    var cambios = reg.Cambenvcolvar.Split(',');
                    for (var i = 0; i < cambios.Count(); i++)
                    {
                        TimeSpan ts = reg.Cambenvfecha - formato.FechaInicio;
                        var horizon = ts.Days;
                        var col = listaHojaPto.Find(x => x.Ptomedicodi == reg.Ptomedicodi).Hojaptoorden + formato.Formatcols - 1;
                        var row = formato.Formatrows +
                            ObtieneRowChange((int)formato.Formatperiodo, (int)formato.Formatresolucion, int.Parse(cambios[i]),
                            formato.RowPorDia, reg.Cambenvfecha, formato.FechaInicio);
                        listaCellCambios.Add(new CeldaCambios()
                        {
                            Row = row,
                            Col = col
                        });
                    }
                }
                // }
            }
            for (int k = 0; k < listaHojaPto.Count; k++)
            {
                for (int z = 0; z < formato.Formathorizonte; z++) //Horizonte se comporta como uno cuando resolucion es mas que un dia
                {
                    DateTime fechaFind = GetNextFilaHorizonte((int)formato.Formatperiodo, (int)formato.Formatresolucion, z, formato.FechaInicio);
                    var reg = lista.Find(i => (int)i.GetType().GetProperty("Ptomedicodi").GetValue(i, null) == listaHojaPto[k].Ptomedicodi
                                && (DateTime)i.GetType().GetProperty("Medifecha").GetValue(i, null) == fechaFind);

                    for (int j = 1; j <= formato.RowPorDia; j++) // nBlock se comporta como horizonte cuando resolucion es mas de un dia
                    {
                        if (k == 0)
                        {
                            int jIni = 0;
                            if (formato.Formatresolucion >= ParametrosFormato.ResolucionDia)
                                jIni = j - 1;
                            else
                                jIni = j;

                            listaExcelData[z * formato.RowPorDia + j + formato.Formatrows - 1][0] =
                               // ((model.FechaInicio.AddMinutes(z * ParametrosFormato.ResolucionDia + jIni * (int)model.Formato.Formatresolucion))).ToString(Constantes.FormatoFechaHora);
                               ObtenerCeldaFecha((int)formato.Formatperiodo, (int)formato.Formatresolucion, formato.Lecttipo, z, jIni, formato.FechaInicio);
                            //model.Handson.ListaExcelData[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][1] = model.Formato.FechaInicio.AddDays(7*z).ToString(Constantes.FormatoFecha);

                        }

                        if (reg != null)
                        {
                            decimal? valor = (decimal?)reg.GetType().GetProperty("H" + j).GetValue(reg, null);
                            if (valor != null)
                                listaExcelData[z * formato.RowPorDia + j + formato.Formatrows - 1][k + formato.Formatcols] = valor.ToString();
                        }
                    }
                }

            }
        }


        #endregion

        #region Migracion

        public void EjecutarMigracion()
        {
            DateTime fechaInicio = new DateTime(2016, 1, 1);
            DateTime fechaFin = new DateTime(2016, 1, 2);
            var lista = FactorySic.GetMeMedicion24Repository().List(fechaInicio, fechaFin);
            decimal? valor, valorTemp;
            int ptoAnt = 0;
            MeMedicion24DTO regTemp = new MeMedicion24DTO();
            valorTemp = null;
            foreach (var reg in lista)
            {
                if (ptoAnt != reg.Ptomedicodi)
                {
                    valorTemp = null;
                }
                regTemp = new MeMedicion24DTO();
                regTemp.Medifecha = reg.Medifecha;
                regTemp.Ptomedicodi = reg.Ptomedicodi;
                regTemp.Lectcodi = reg.Lectcodi;
                regTemp.Tipoinfocodi = reg.Tipoinfocodi;
                regTemp.Lastdate = DateTime.Now;
                regTemp.Lastuser = "geiner.tafur";
                regTemp.Mediestado = reg.Mediestado;
                regTemp.Meditotal = reg.Meditotal;
                regTemp.GetType().GetProperty("H1").SetValue(regTemp, valorTemp);
                ptoAnt = reg.Ptomedicodi;
                for (int i = 1; i <= 24; i++)
                {
                    valor = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);

                    if (i < 24)
                        regTemp.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(regTemp, valor);
                    else
                    {
                        if (valor != null)
                        {
                            var find = lista.Find(x => x.Medifecha == reg.Medifecha.AddDays(1) && x.Ptomedicodi == reg.Ptomedicodi
                                && x.Tipoinfocodi == reg.Tipoinfocodi);
                            if (find != null) // Hay Dia siguiente
                            {
                                valorTemp = valor;
                            }
                            else // Falta Dia
                            {
                                var registro = new MeMedicion24DTO();
                                registro.Medifecha = reg.Medifecha.AddDays(1);
                                registro.Ptomedicodi = reg.Ptomedicodi;
                                registro.Lectcodi = reg.Lectcodi;
                                registro.Tipoinfocodi = reg.Tipoinfocodi;
                                registro.Lastdate = DateTime.Now;
                                registro.Lastuser = "geiner.tafur";
                                registro.Mediestado = reg.Mediestado;
                                registro.Meditotal = reg.Meditotal;
                                registro.GetType().GetProperty("H1").SetValue(registro, valor);
                                valorTemp = null;
                                //Grabar Registro
                                FactorySic.GetMeMedicion24Repository().Save(registro);
                            }
                        }
                    }

                }
                FactorySic.GetMeMedicion24Repository().Update(regTemp);
            }
        }

        #endregion

        #region Carga Datos

        /// <summary>
        /// Carga de datos de Hidrología Reprograma
        /// </summary>
        /// <param name="fechaReprograma"></param>
        /// <param name="hInicio"></param>
        /// <param name="lista48WS"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int CargarReprogramaHidrologiaCP(DateTime fechaReprograma, int hInicio, List<MeMedicion48DTO> lista48WS, string usuario)
        {
            FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();

            int idFormato = ConstHidrologia.FormatcodiHidroCPReprogDiario;
            MeFormatoDTO formato = servFormato.GetByIdMeFormato(idFormato);
            formato.FechaProceso = fechaReprograma;

            int idFormato2 = ObtenerIdFormatoPadre(formato.Formatcodi);
            MeFormatoDTO formato2 = servFormato.GetByIdMeFormato(idFormato2);
            var cabecera = servFormato.GetListMeCabecera().Where(x => x.Cabcodi == formato2.Cabcodi).FirstOrDefault();

            FormatoMedicionAppServicio.GetSizeFormato(formato);

            var listaPto = servFormato.GetListaPtos(formato.FechaFin, 0, -1, idFormato2, cabecera.Cabquery);

            /////////////////////////////////////////////////////////////////////////////////////////
            List<MeMedicion48DTO> lista48 = new List<MeMedicion48DTO>();
            DateTime fechaActualizacion = DateTime.Now;

            var lista48BD = servFormato.GetDataFormato(-1, formato, 0, 0).Select(x => (MeMedicion48DTO)x);
            if (lista48BD == null) //Si la BD está vacía cargar la data del WS
            {
                lista48BD = lista48WS;
            }
            foreach (var regPto in listaPto)
            {
                var obj = new MeMedicion48DTO();
                obj.Medifecha = formato.FechaInicio;
                obj.Ptomedicodi = regPto.Ptomedicodi;
                obj.Tipoinfocodi = regPto.Tipoinfocodi;
                obj.Lectcodi = formato.Lectcodi;
                obj.Tipoptomedicodi = regPto.Tptomedicodi;
                obj.Emprcodi = regPto.Emprcodi;
                obj.Lastdate = fechaActualizacion;
                obj.Lastuser = usuario;

                MeMedicion48DTO dataMedicion48DB = lista48BD.FirstOrDefault(x => x.Ptomedicodi == obj.Ptomedicodi && x.Tipoinfocodi == obj.Tipoinfocodi);
                MeMedicion48DTO dataMedicion48WS = lista48WS.Find(x => x.Ptomedicodi == obj.Ptomedicodi && x.Tipoinfocodi == obj.Tipoinfocodi);

                if (dataMedicion48DB != null)
                {
                    for (int hx = 1; hx < hInicio; hx++)
                    {
                        var hBD = dataMedicion48DB.GetType().GetProperty(ConstantesAppServicio.CaracterH + hx).GetValue(dataMedicion48DB, null);
                        obj.GetType().GetProperty(ConstantesAppServicio.CaracterH + hx).SetValue(obj, hBD);
                    }
                }

                if (dataMedicion48WS != null)
                {
                    for (int hx = hInicio; hx <= 48; hx++)
                    {
                        var hWS = dataMedicion48WS.GetType().GetProperty(ConstantesAppServicio.CaracterH + hx).GetValue(dataMedicion48WS, null);
                        obj.GetType().GetProperty(ConstantesAppServicio.CaracterH + hx).SetValue(obj, hWS);
                    }
                }

                lista48.Add(obj);
            }

            /////////////////////////////////////////////////////////////////////////////////////////
            MeConfigformatenvioDTO config = new MeConfigformatenvioDTO();
            config.Formatcodi = idFormato;
            config.Emprcodi = -1;
            config.FechaInicio = formato.FechaFin;
            int idConfig = servFormato.GrabarConfigFormatEnvio(config);

            Boolean enPlazo = servFormato.ValidarPlazo(formato);
            MeEnvioDTO envio = new MeEnvioDTO();
            envio.Archcodi = 0;
            envio.Emprcodi = -1;
            envio.Enviofecha = fechaActualizacion;
            envio.Enviofechaperiodo = formato.FechaProceso;
            envio.Enviofechaini = formato.FechaInicio;
            envio.Enviofechafin = formato.FechaFin;
            envio.Envioplazo = (enPlazo) ? "P" : "F";
            envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
            envio.Lastdate = fechaActualizacion;
            envio.Lastuser = usuario;
            envio.Userlogin = usuario;
            envio.Formatcodi = idFormato;
            envio.Fdatcodi = 0;
            envio.Cfgenvcodi = idConfig;
            int idEnvio = servFormato.SaveMeEnvio(envio);

            servFormato.GrabarValoresCargados48(lista48, usuario, idEnvio, -1, formato);

            return 1;

        }

        /// <summary>
        /// Obtiene el valor del formato seg{un su configuración
        /// </summary>
        /// <param name="idHorizonte"></param>
        /// <param name="formatoReal"></param>
        /// <returns></returns>
        public int ObtenerIdFormatoPadre(int formatoReal)
        {
            FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();

            var formato = logic.GetByIdMeFormato(formatoReal);
            int idFormato = formato.Formatdependeconfigptos != null ? formato.Formatdependeconfigptos.Value : formato.Formatcodi;

            return idFormato;
        }

        public void GetSizeFormato2(MeFormatoDTO formato)
        {
            switch (formato.Formatperiodo)
            {
                case ParametrosFormato.PeriodoDiario:
                    formato.FechaInicio = formato.FechaProceso;
                    formato.FechaFin = formato.FechaProceso.AddDays(formato.Formathorizonte);
                    formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
                    if (formato.Formatdiaplazo == 0) //Informacion en Tiempo Real
                    {
                        formato.FechaPlazoIni = formato.FechaProceso;
                        formato.FechaPlazo = formato.FechaProceso.AddDays(1).AddMinutes(formato.Formatminplazo);
                    }
                    else
                    {
                        if (formato.Lecttipo == ParametrosLectura.Ejecutado) //Ejecutado
                        {
                            formato.FechaPlazoIni = formato.FechaProceso.AddDays(formato.Formatdiaplazo);
                            formato.FechaPlazo = formato.FechaProceso.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                        }
                        else
                        {
                            formato.FechaPlazoIni = formato.FechaProceso.AddDays(-1);
                            formato.FechaPlazo = formato.FechaProceso.AddDays(-1).AddMinutes(formato.Formatminplazo);
                        }
                    }
                    break;
                case ParametrosFormato.PeriodoSemanal:
                    formato.FechaInicio = formato.FechaProceso;
                    formato.FechaFin = formato.FechaProceso.AddDays(formato.Formathorizonte);
                    if (formato.Lecttipo == ParametrosLectura.Ejecutado) //Ejecutado
                    {
                        formato.FechaPlazoIni = formato.FechaProceso.AddDays(7);
                        formato.FechaPlazo = formato.FechaProceso.AddDays(7 + formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                    }
                    else
                    {
                        formato.FechaPlazoIni = formato.FechaProceso.AddDays(-7);
                        formato.FechaPlazo = formato.FechaProceso.AddDays(-7 + formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                    }
                    switch (formato.Formatresolucion)
                    {
                        case ParametrosFormato.ResolucionCuartoHora:
                        case ParametrosFormato.ResolucionMediaHora:
                        case ParametrosFormato.ResolucionHora:
                            formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
                            break;
                        case ParametrosFormato.ResolucionDia:
                            formato.RowPorDia = 1;
                            break;
                        case ParametrosFormato.ResolucionSemana:
                            formato.RowPorDia = 1;
                            break;
                    }
                    break;
                case ParametrosFormato.PeriodoMensualSemana: //Semanal Mediano Plazo
                    formato.RowPorDia = 1;

                    if (formato.Lecttipo == ParametrosLectura.Ejecutado) //Ejecutado
                    {
                        formato.FechaPlazoIni = formato.FechaProceso.AddMonths(1);
                        formato.FechaPlazo = formato.FechaProceso.AddMonths(1).AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                        //fecha inicio es primera semana del año
                        //fecha fin es ultima semana antes del mes seleccionado
                        //si se selecciona enero se toma todo el año anterior
                        if (formato.FechaProceso.Month == 1)
                        {
                            formato.FechaInicio = EPDate.f_fechainiciosemana(formato.FechaProceso.Year - 1, 1);
                            formato.FechaFin = EPDate.f_fechainiciosemana(formato.FechaProceso.Year - 1, EPDate.TotalSemanasEnAnho(formato.FechaProceso.Year - 1, 6));
                            formato.Formathorizonte = EPDate.TotalSemanasEnAnho(formato.FechaProceso.Year - 1, 6);
                        }
                        else
                        {
                            formato.FechaInicio = EPDate.f_fechainiciosemana(formato.FechaProceso.Year, 1);
                            formato.FechaFin = EPDate.f_fechainiciosemana(formato.FechaProceso.AddDays(-7));
                            formato.Formathorizonte = EPDate.f_numerosemana(formato.FechaFin);
                        }
                    }
                    else //Programado
                    {
                        formato.FechaPlazoIni = formato.FechaProceso.AddMonths(-1);
                        formato.FechaPlazo = formato.FechaProceso.AddMonths(-1).AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                        //fecha inicio es la primera semana del mes seleccionado
                        //fecha fin es la ultima semana del año
                        formato.FechaInicio = EPDate.f_fechainiciosemana(formato.FechaProceso.AddDays(7));
                        formato.FechaFin = EPDate.f_fechainiciosemana(formato.FechaProceso.AddYears(1));
                        formato.Formathorizonte = EPDate.f_numerosemana(formato.FechaFin) +
                            EPDate.TotalSemanasEnAnho(formato.FechaProceso.Year - 1, 6) -
                            EPDate.f_numerosemana(formato.FechaInicio) + 1;
                    }
                    break;
                case ParametrosFormato.PeriodoMensual:
                    switch (formato.Formatresolucion)
                    {
                        case ParametrosFormato.ResolucionCuartoHora:
                        case ParametrosFormato.ResolucionMediaHora:
                        case ParametrosFormato.ResolucionHora:
                            formato.FechaInicio = formato.FechaProceso;
                            formato.FechaFin = formato.FechaProceso.AddMonths(1).AddDays(-1);
                            //formato.FechaPlazo = formato.FechaFin.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                            formato.FechaPlazo = formato.FechaProceso;
                            formato.FechaPlazo = formato.FechaProceso.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                            formato.Formathorizonte = ((TimeSpan)(formato.FechaFin - formato.FechaInicio)).Days + 1;
                            formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
                            break;
                        case ParametrosFormato.ResolucionDia:
                            formato.FechaInicio = formato.FechaProceso;
                            formato.FechaFin = formato.FechaProceso.AddMonths(1).AddDays(-1);
                            //formato.FechaPlazo = formato.FechaFin.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                            formato.FechaPlazo = formato.FechaProceso;
                            formato.FechaPlazo = formato.FechaProceso.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                            formato.Formathorizonte = ((TimeSpan)(formato.FechaFin - formato.FechaInicio)).Days;
                            formato.RowPorDia = 1;
                            break;
                        case ParametrosFormato.ResolucionMes:
                            formato.RowPorDia = 1;
                            if (formato.Lecttipo == ParametrosLectura.Ejecutado)
                            {
                                formato.Formathorizonte = formato.FechaProceso.Month;
                                formato.FechaFin = formato.FechaProceso;
                                formato.FechaInicio = new DateTime(formato.FechaProceso.Year, 1, 1);
                                //formato.FechaPlazo = formato.FechaFin.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                                formato.FechaPlazo = formato.FechaProceso.AddMonths(1);
                                formato.FechaPlazo = formato.FechaProceso.AddMonths(1).AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                            }
                            else // Programado
                            {
                                formato.FechaInicio = formato.FechaProceso;
                                formato.FechaFin = formato.FechaProceso.AddMonths(12);
                                formato.Formathorizonte = 12;
                                //formato.FechaPlazo = formato.FechaInicio.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                                formato.FechaPlazo = formato.FechaProceso;
                                formato.FechaPlazo = formato.FechaProceso.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
                            }
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// Verifica si un formato enviado esta en plazo o fuyera de plazo
        /// </summary>
        /// <param name="formato"></param>
        /// <returns></returns>
        public bool ValidarPlazoController(MeFormatoDTO formato)
        {
            bool resultado = false;
            DateTime fechaActual = DateTime.Now;
            if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= formato.FechaPlazo))
            {
                resultado = true;
            }
            return resultado;
        }

        /// <summary>
        /// Valida la fecha
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="horaini"></param>
        /// <param name="horafin"></param>
        /// <returns></returns>
        public bool ValidarFecha(MeFormatoDTO formato, int idEmpresa, out int horaini, out int horafin)
        {
            bool resultado = false;
            DateTime fechaActual = DateTime.Now;
            horaini = 0;
            horafin = 0;
            if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= formato.FechaPlazo))
            {
                resultado = true;
            }
            else
            {
                var regfechaPlazo = this.GetByIdMeAmpliacionfecha(formato.FechaProceso, idEmpresa, formato.Formatcodi);
                if (regfechaPlazo != null) // si existe registro de ampliacion
                {

                    if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= regfechaPlazo.Amplifechaplazo))
                    {
                        resultado = true;
                    }
                }
            }
            if ((formato.Formatdiaplazo == 0) && (resultado)) //Formato Tiempo Real
            {
                int hora = fechaActual.Hour;
                if (((hora - 1) % 3) == 0)
                {
                    horaini = hora - 1 - 1 * 3;
                    horafin = hora - 1;
                }
                else
                {
                    horafin = -1;//indica que formato tiempo real no tiene filas habilitadas
                    resultado = false;
                }
            }
            return true;
            //return resultado;
        }

        /// <summary>
        /// Inicializa lista de filas readonly para la matriz excel web
        /// </summary>
        /// <param name="filHead"></param>
        /// <param name="filData"></param>
        /// <returns></returns>
        public List<bool> InicializaListaFilaReadOnly(int filHead, int filData, bool plazo, int horaini, int horafin)
        {
            List<bool> lista = new List<bool>();
            for (int i = 0; i < filHead; i++)
            {
                lista.Add(true);
            }
            for (int i = 0; i < filData; i++)
            {
                if (plazo)
                {
                    if (horafin == 0)
                        lista.Add(false);
                    else
                    {
                        if ((i >= horaini) && (i < horafin))
                        {
                            lista.Add(false);
                        }
                        else
                            lista.Add(true);
                    }
                }
                else
                    lista.Add(true);
            }

            return lista;
        }

        /// <summary>
        /// Obtiene matriz de string que son solo los valores de las celdas del excel web ingresando como parametro una lista de mediciones.
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="listaCambios"></param>
        /// <param name="formato"></param>
        public void ObtieneMatrizWebExcel(FormatoModel model, List<object> lista, List<MeCambioenvioDTO> listaCambios, int idEnvio)
        {
            if (idEnvio > 0)
            {
                foreach (var reg in listaCambios)
                {
                    if (reg.Cambenvcolvar != null)
                    {
                        var cambios = reg.Cambenvcolvar.Split(',');
                        for (var i = 0; i < cambios.Count(); i++)
                        {
                            TimeSpan ts = reg.Cambenvfecha - model.Formato.FechaInicio;
                            var horizon = ts.Days;
                            var col = model.ListaHojaPto.Find(x => x.Ptomedicodi == reg.Ptomedicodi).Hojaptoorden + model.ColumnasCabecera - 1;
                            var row = model.FilasCabecera +
                                ObtieneRowChange((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, int.Parse(cambios[i]),
                                model.Formato.RowPorDia, reg.Cambenvfecha, model.Formato.FechaInicio);
                            //int.Parse(cambios[i]) + model.Formato.RowPorDia * horizon;
                            model.ListaCambios.Add(new CeldaCambios()
                            {
                                Row = row,
                                Col = col
                            });
                        }
                    }
                }
            }
            for (int k = 0; k < model.ListaHojaPto.Count; k++)
            {
                for (int z = 0; z < model.Formato.Formathorizonte; z++) //Horizonte se comporta como uno cuando resolucion es mas que un dia
                {
                    DateTime fechaFind = GetNextFilaHorizonte((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, z, model.Formato.FechaInicio);
                    var reg = lista.Find(i => (int)i.GetType().GetProperty("Ptomedicodi").GetValue(i, null) == model.ListaHojaPto[k].Ptomedicodi
                                && (DateTime)i.GetType().GetProperty("Medifecha").GetValue(i, null) == fechaFind);

                    for (int j = 1; j <= model.Formato.RowPorDia; j++) // nBlock se comporta como horizonte cuando resolucion es mas de un dia
                    {
                        if (k == 0)
                        {
                            int jIni = 0;
                            if (model.Formato.Formatresolucion >= ParametrosFormato.ResolucionDia)
                                jIni = j - 1;
                            else
                                jIni = j;

                            model.Handson.ListaExcelData[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][model.ColumnasCabecera - 1] =
                               // ((model.FechaInicio.AddMinutes(z * ParametrosFormato.ResolucionDia + jIni * (int)model.Formato.Formatresolucion))).ToString(Constantes.FormatoFechaHora);
                               ObtenerCeldaFecha((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, model.Formato.Lecttipo, z, jIni, model.Formato.FechaInicio);
                        }
                        if (reg != null)
                        {
                            decimal? valor = (decimal?)reg.GetType().GetProperty("H" + j).GetValue(reg, null);
                            if (valor != null)
                                model.Handson.ListaExcelData[z * model.Formato.RowPorDia + j + model.FilasCabecera - 1][k + model.ColumnasCabecera] = valor.ToString();
                        }
                    }
                }
            }

        }

        #endregion

        #region Configuración de Modelo de Embalse

        #region Métodos Tabla CM_MODELO_EMBALSE

        /// <summary>
        /// Inserta un registro de la tabla CM_MODELO_EMBALSE
        /// </summary>
        public int SaveCmModeloEmbalse(CmModeloEmbalseDTO entity)
        {
            try
            {
                return FactorySic.GetCmModeloEmbalseRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CM_MODELO_EMBALSE
        /// </summary>
        public void UpdateCmModeloEmbalse(CmModeloEmbalseDTO entity)
        {
            try
            {
                FactorySic.GetCmModeloEmbalseRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CM_MODELO_EMBALSE
        /// </summary>
        public void DeleteCmModeloEmbalse(CmModeloEmbalseDTO entity)
        {
            try
            {
                FactorySic.GetCmModeloEmbalseRepository().Delete(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CM_MODELO_EMBALSE
        /// </summary>
        public CmModeloEmbalseDTO GetByIdCmModeloEmbalse(int modembcodi)
        {
            var obj = FactorySic.GetCmModeloEmbalseRepository().GetById(modembcodi);
            FormatearCmModeloEmbalse(obj);

            return obj;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CM_MODELO_EMBALSE
        /// </summary>
        public List<CmModeloEmbalseDTO> ListCmModeloEmbalses()
        {
            return FactorySic.GetCmModeloEmbalseRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CM_MODELO_EMBALSE
        /// </summary>
        public List<CmModeloEmbalseDTO> ListHistorialCmModeloEmbalses(int recurcodi)
        {
            var lista = FactorySic.GetCmModeloEmbalseRepository().ListHistorialXRecurso(recurcodi);
            foreach (var obj in lista)
            {
                FormatearCmModeloEmbalse(obj);
            }

            return lista.OrderByDescending(x => x.Modembfeccreacion).ToList();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CmModeloEmbalse
        /// </summary>
        public List<CmModeloEmbalseDTO> GetByCriteriaCmModeloEmbalses(string recurcodis)
        {
            //ultima version de los embalses
            var lista = FactorySic.GetCmModeloEmbalseRepository().GetByCriteria(ConstantesAppServicio.ParametroDefecto, recurcodis).OrderBy(x => x.Recurcodi)
                                        .ThenByDescending(x => x.Modembfecvigencia).ThenByDescending(x => x.Modembcodi).ToList();
            lista = lista.GroupBy(x => x.Recurcodi).Select(x => x.First()).ToList();

            //obtener centrales turbinantes de cada embalse
            var listaCompBD = new List<CmModeloComponenteDTO>();
            if (lista.Any())
                listaCompBD = GetByCriteriaCmModeloComponentes(string.Join(",", lista.Select(x => x.Modembcodi).ToList()), ConstantesAppServicio.ParametroDefecto);

            foreach (var obj in lista)
            {
                var listaCompXEmb = listaCompBD.Where(x => x.Modembcodi == obj.Modembcodi && x.Modcomtipo == ConstHidrologia.ComponenteTipoCentralTurbinante).ToList();
                obj.CentralTurbinate = string.Join(", ", listaCompXEmb.Where(x => x.Equinomb != null).Select(x => x.Equinomb.Trim()));

                FormatearCmModeloEmbalse(obj);
            }

            //quedarse con la ultima version, luego verificar si está eliminado o no
            return lista.Where(x => x.Modembestado == ConstantesAppServicio.Activo).OrderBy(x => x.Recurnombre).ToList();
        }

        private void FormatearCmModeloEmbalse(CmModeloEmbalseDTO obj)
        {
            obj.Ptomedielenomb = obj.Ptomedielenomb ?? "";
            obj.CentralTurbinate = obj.CentralTurbinate ?? "";
            obj.ModembfecvigenciaDesc = obj.Modembfecvigencia.ToString(ConstantesAppServicio.FormatoFecha);
            obj.ModembindyupanaDesc = obj.Modembindyupana == "S" ? "SI" : "NO";
            obj.UltimaModificacionFechaDesc = obj.Modembfecmodificacion != null ? obj.Modembfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : obj.Modembfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
            obj.UltimaModificacionUsuarioDesc = obj.Modembfecmodificacion != null ? obj.Modembusumodificacion : obj.Modembusucreacion;
        }

        #endregion

        #region Métodos Tabla CM_MODELO_COMPONENTE

        /// <summary>
        /// Inserta un registro de la tabla CM_MODELO_COMPONENTE
        /// </summary>
        public int SaveCmModeloComponente(CmModeloComponenteDTO entity)
        {
            try
            {
                return FactorySic.GetCmModeloComponenteRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CM_MODELO_COMPONENTE
        /// </summary>
        public void UpdateCmModeloComponente(CmModeloComponenteDTO entity)
        {
            try
            {
                FactorySic.GetCmModeloComponenteRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CM_MODELO_COMPONENTE
        /// </summary>
        public void DeleteCmModeloComponente(int modcomcodi)
        {
            try
            {
                FactorySic.GetCmModeloComponenteRepository().Delete(modcomcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CM_MODELO_COMPONENTE
        /// </summary>
        public CmModeloComponenteDTO GetByIdCmModeloComponente(int modcomcodi)
        {
            return FactorySic.GetCmModeloComponenteRepository().GetById(modcomcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CM_MODELO_COMPONENTE
        /// </summary>
        public List<CmModeloComponenteDTO> ListCmModeloComponentes()
        {
            return FactorySic.GetCmModeloComponenteRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CmModeloComponente
        /// </summary>
        public List<CmModeloComponenteDTO> GetByCriteriaCmModeloComponentes(string modembcodi, string modcomcodis)
        {
            return FactorySic.GetCmModeloComponenteRepository().GetByCriteria(modembcodi, modcomcodis);
        }

        #endregion

        #region Métodos Tabla CM_MODELO_AGRUPACION

        /// <summary>
        /// Inserta un registro de la tabla CM_MODELO_AGRUPACION
        /// </summary>
        public int SaveCmModeloAgrupacion(CmModeloAgrupacionDTO entity)
        {
            try
            {
                return FactorySic.GetCmModeloAgrupacionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CM_MODELO_AGRUPACION
        /// </summary>
        public void UpdateCmModeloAgrupacion(CmModeloAgrupacionDTO entity)
        {
            try
            {
                FactorySic.GetCmModeloAgrupacionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CM_MODELO_AGRUPACION
        /// </summary>
        public void DeleteCmModeloAgrupacion(int modagrcodi)
        {
            try
            {
                FactorySic.GetCmModeloAgrupacionRepository().Delete(modagrcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CM_MODELO_AGRUPACION
        /// </summary>
        public CmModeloAgrupacionDTO GetByIdCmModeloAgrupacion(int modagrcodi)
        {
            return FactorySic.GetCmModeloAgrupacionRepository().GetById(modagrcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CM_MODELO_AGRUPACION
        /// </summary>
        public List<CmModeloAgrupacionDTO> ListCmModeloAgrupacions()
        {
            return FactorySic.GetCmModeloAgrupacionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CmModeloAgrupacion
        /// </summary>
        public List<CmModeloAgrupacionDTO> GetByCriteriaCmModeloAgrupacions(int modembcodi)
        {
            return FactorySic.GetCmModeloAgrupacionRepository().GetByCriteria(modembcodi);
        }

        #endregion

        #region Métodos Tabla CM_MODELO_CONFIGURACION

        /// <summary>
        /// Inserta un registro de la tabla CM_MODELO_CONFIGURACION
        /// </summary>
        public void SaveCmModeloConfiguracion(CmModeloConfiguracionDTO entity)
        {
            try
            {
                FactorySic.GetCmModeloConfiguracionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CM_MODELO_CONFIGURACION
        /// </summary>
        public void UpdateCmModeloConfiguracion(CmModeloConfiguracionDTO entity)
        {
            try
            {
                FactorySic.GetCmModeloConfiguracionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CM_MODELO_CONFIGURACION
        /// </summary>
        public void DeleteCmModeloConfiguracion(int modconcodi)
        {
            try
            {
                FactorySic.GetCmModeloConfiguracionRepository().Delete(modconcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CM_MODELO_CONFIGURACION
        /// </summary>
        public CmModeloConfiguracionDTO GetByIdCmModeloConfiguracion(int modconcodi)
        {
            return FactorySic.GetCmModeloConfiguracionRepository().GetById(modconcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CM_MODELO_CONFIGURACION
        /// </summary>
        public List<CmModeloConfiguracionDTO> ListCmModeloConfiguracions()
        {
            return FactorySic.GetCmModeloConfiguracionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CmModeloConfiguracion
        /// </summary>
        public List<CmModeloConfiguracionDTO> GetByCriteriaCmModeloConfiguracions(int modembcodi)
        {
            return FactorySic.GetCmModeloConfiguracionRepository().GetByCriteria(modembcodi);
        }

        #endregion

        #region Lógica

        /// <summary>
        /// ObtenerFiltroModeloEmbalse
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public ConfiguracionDefaultEmbalseHidrologia ObtenerFiltroModeloEmbalse(DateTime fechaConsulta)
        {
            List<CpRecursoDTO> listaRecursoEmbalse = FactorySic.GetCpRecursoRepository().ListaRecursoXCategoria(ConstHidrologia.RecurcodiEmbalse, (int)ConstHidrologia.TopologiaBase)
                                                    .OrderBy(x => x.Recurnombre).ToList();

            string formatcodis = "6,5,32,7,43";
            List<MeHojaptomedDTO> listaHptHidrologia = FactorySic.GetMeHojaptomedRepository().ListarHojaPtoByFormatoAndEmpresa(-1, formatcodis).Where(x => x.Hojaptoactivo == 1).ToList();

            List<MePtomedicionDTO> listaPtoVolumen = listaHptHidrologia.Where(x => x.Tipoinfocodi == ConstHidrologia.Volumen).GroupBy(x => x.Ptomedicodi)
                                                    .Select(x => new MePtomedicionDTO()
                                                    {
                                                        Ptomedicodi = x.Key,
                                                        Ptomedielenomb = x.First().PtoMediEleNomb,
                                                    }).OrderBy(x => x.Ptomedielenomb).ToList();

            List<MePtomedicionDTO> listaPtoCaudal = listaHptHidrologia.Where(x => x.Tipoinfocodi == ConstHidrologia.Caudal).GroupBy(x => x.Ptomedicodi)
                                                    .Select(x => new MePtomedicionDTO()
                                                    {
                                                        Ptomedicodi = x.Key,
                                                        Ptomedielenomb = x.First().PtoMediEleNomb,
                                                    }).OrderBy(x => x.Ptomedielenomb).ToList();

            List<EqEquipoDTO> listaEqCentral = FactorySic.GetEqEquipoRepository().ListarEquiposPorFamilia(ConstantesHorasOperacion.IdTipoHidraulica.ToString());
            listaEqCentral = listaEqCentral.Where(x => x.Equiestado != ConstantesAppServicio.Eliminado && x.Equiestado != ConstantesAppServicio.Anulado && x.Equiestado != ConstantesAppServicio.Baja)
                                        .OrderBy(x => x.Equicodi).ToList();

            //Rendimiento (MW/m3/s) de las centrales hidraulicas
            List<EqPropequiDTO> listaRendimiento = INDAppServicio.ListarEqPropequiHistoricoDecimalValido(ConstantesPR5ReportesServicio.PropRendimientoHidro.ToString());
            foreach (var reg in listaEqCentral)
            {
                INDAppServicio.GetPeFromListaPropequi(fechaConsulta, reg.Equicodi, listaRendimiento, out decimal? valorRend, out DateTime? fechaVigenciaRend, out string comentarioRend);
                reg.Rendimiento = valorRend;
            }

            List<EqEquipoDTO> listaEqBD = FactorySic.GetEqEquipoRepository().ListarEquiposPorFamilia(ConstantesHorasOperacion.CodFamilias + "," + ConstantesHorasOperacion.CodFamiliasGeneradores);
            listaEqBD = listaEqBD.Where(x => x.Equiestado != ConstantesAppServicio.Eliminado && x.Equiestado != ConstantesAppServicio.Anulado && x.Equiestado != ConstantesAppServicio.Baja)
                                        .OrderBy(x => x.Areanomb).ThenBy(x => x.Equiabrev).ToList();

            string formatcodisIdcc = "62";
            List<MeHojaptomedDTO> listaHptIDCC = FactorySic.GetMeHojaptomedRepository().ListarHojaPtoByFormatoAndEmpresa(-1, formatcodisIdcc).Where(x => x.Hojaptoactivo == 1).ToList();
            List<MePtomedicionDTO> listaPtoIDCC = listaHptIDCC.Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMW).GroupBy(x => x.Ptomedicodi)
                                                    .Select(x => new MePtomedicionDTO()
                                                    {
                                                        Ptomedicodi = x.Key,
                                                        Ptomedielenomb = x.First().PtoMediEleNomb,
                                                    }).OrderBy(x => x.Ptomedielenomb).ToList();

            List<CpRecursoDTO> listaRecursoPlantaH = FactorySic.GetCpRecursoRepository().ListaRecursoXCategoria(ConstHidrologia.RecurcodiPlantaH, (int)ConstHidrologia.TopologiaBase)
                                                    .OrderBy(x => x.Recurnombre).ToList();

            //output
            ConfiguracionDefaultEmbalseHidrologia conf = new ConfiguracionDefaultEmbalseHidrologia
            {
                ListaRecursoEmbalse = listaRecursoEmbalse,
                ListaPtoCaudal = listaPtoCaudal,
                ListaPtoVolumen = listaPtoVolumen,
                ListaEqCentral = listaEqCentral,
                ListaPtoIDCC = listaPtoIDCC,
                ListaEqTNA = listaEqBD,
                ListaRecursoPlantaH = listaRecursoPlantaH
            };

            return conf;
        }

        /// <summary>
        /// ObtenerModeloEmbalseXId
        /// </summary>
        /// <param name="modembcodi"></param>
        /// <returns></returns>
        public CmModeloEmbalseDTO ObtenerModeloEmbalseXId(int modembcodi)
        {
            var listaCompBD = GetByCriteriaCmModeloComponentes(modembcodi.ToString(), ConstantesAppServicio.ParametroDefecto);
            var listaAgrupBD = GetByCriteriaCmModeloAgrupacions(modembcodi);
            var listaConfBD = GetByCriteriaCmModeloConfiguracions(modembcodi);

            CmModeloEmbalseDTO objModelo = GetByIdCmModeloEmbalse(modembcodi);
            objModelo.ListaComponente = listaCompBD;
            foreach (var objComp in objModelo.ListaComponente)
            {
                var listaAgrupXComp = listaAgrupBD.Where(x => x.Modcomcodi == objComp.Modcomcodi).ToList();
                objComp.ListaAgrupacion = listaAgrupXComp;

                foreach (var objAgrup in objComp.ListaAgrupacion)
                {
                    var listaConfXAgrup = listaConfBD.Where(x => x.Modagrcodi == objAgrup.Modagrcodi).ToList();
                    objAgrup.ListaConfiguracion = listaConfXAgrup;
                }
            }

            return objModelo;
        }

        /// <summary>
        /// ListarModeloEmbalseVigente
        /// </summary>
        /// <param name="fechaVig"></param>
        /// <returns></returns>
        public List<CmModeloEmbalseDTO> ListarModeloEmbalseVigente(DateTime fechaVig)
        {
            return GetByCriteriaCmModeloEmbalses(ConstantesAppServicio.ParametroDefecto);
        }

        /// <summary>
        /// GuardarModeloEmbalse
        /// </summary>
        /// <param name="objModelo"></param>
        /// <param name="usuario"></param>
        public void GuardarModeloEmbalse(CmModeloEmbalseDTO objModelo, string usuario)
        {

            //Guardar
            objModelo.Topcodi = 0;
            if (objModelo.Ptomedicodi.GetValueOrDefault(0) <= 0) objModelo.Ptomedicodi = null;

            if (objModelo.ListaComponente != null && objModelo.ListaComponente.Any())
            {
                if (objModelo.Modembcodi == 0)
                {
                    objModelo.Modembestado = ConstantesAppServicio.Activo;
                    objModelo.Modembfeccreacion = DateTime.Now;
                    objModelo.Modembusucreacion = usuario;

                    objModelo.Modembcodi = SaveCmModeloEmbalse(objModelo);
                }

                //Agregar componente no visible Caudal turbinado de cada central de central aportante
                var listaCompCAA = objModelo.ListaComponente.Where(x => x.Modcomtipo == ConstHidrologia.ComponenteTipoCentralAguaArriba).ToList();
                foreach (var objComp in listaCompCAA)
                {
                    objModelo.ListaComponente.Add(new CmModeloComponenteDTO()
                    {
                        Equicodi = objComp.Equicodi,
                        Modcomtipo = ConstHidrologia.ComponenteTipoCaudalTurbinadoCentralAguaArriba,
                        EsCompNoVisible = true,
                        ListaAgrupacion = new List<CmModeloAgrupacionDTO>()
                    });
                }

                objModelo.ListaComponente.Add(new CmModeloComponenteDTO()
                {
                    Modcomtipo = ConstHidrologia.ComponenteTipoVolumenEmbalse,
                    EsCompNoVisible = true,
                    ListaAgrupacion = new List<CmModeloAgrupacionDTO>()
                });

                //guardar cabecera de Caudales de entrada, Centrales aguas arriba, Centrales turbinantes
                foreach (var objComp in objModelo.ListaComponente)
                {
                    if ((objComp.ListaAgrupacion != null && objComp.ListaAgrupacion.Any()) || objComp.EsCompNoVisible)
                    {
                        objComp.Modembcodi = objModelo.Modembcodi;
                        objComp.Modcomcodi = SaveCmModeloComponente(objComp);

                        //guardar cada fila
                        foreach (var objAgrup in objComp.ListaAgrupacion)
                        {
                            if (objAgrup.ListaConfiguracion != null && objAgrup.ListaConfiguracion.Any())
                            {
                                objAgrup.Modcomcodi = objComp.Modcomcodi;
                                objAgrup.Modagrcodi = SaveCmModeloAgrupacion(objAgrup);

                                //guardar celda de cada fila
                                foreach (var objConf in objAgrup.ListaConfiguracion)
                                {
                                    objConf.Modagrcodi = objAgrup.Modagrcodi;
                                    SaveCmModeloConfiguracion(objConf);
                                }
                            }
                        }
                    }


                }
            }
        }

        /// <summary>
        /// EliminarModeloEmbalse
        /// </summary>
        /// <param name="modembcodi"></param>
        /// <param name="usuario"></param>
        public void EliminarModeloEmbalse(int modembcodi, string usuario)
        {
            CmModeloEmbalseDTO entity = new CmModeloEmbalseDTO()
            {
                Modembcodi = modembcodi,
                Modembfecmodificacion = DateTime.Now,
                Modembusumodificacion = usuario
            };

            DeleteCmModeloEmbalse(entity);
        }

        #endregion

        #endregion

        #region Cálculo Volumen y Caudales de Embalse

        #region Métodos Tabla CM_VOLUMEN_DETALLE

        /// <summary>
        /// Inserta un registro de la tabla CM_VOLUMEN_DETALLE
        /// </summary>
        public void SaveCmVolumenDetalle(CmVolumenDetalleDTO entity)
        {
            try
            {
                FactorySic.GetCmVolumenDetalleRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CM_VOLUMEN_DETALLE
        /// </summary>
        public void UpdateCmVolumenDetalle(CmVolumenDetalleDTO entity)
        {
            try
            {
                FactorySic.GetCmVolumenDetalleRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CM_VOLUMEN_DETALLE
        /// </summary>
        public void DeleteCmVolumenDetalle(int voldetcodi)
        {
            try
            {
                FactorySic.GetCmVolumenDetalleRepository().Delete(voldetcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CM_VOLUMEN_DETALLE
        /// </summary>
        public CmVolumenDetalleDTO GetByIdCmVolumenDetalle(int voldetcodi)
        {
            return FactorySic.GetCmVolumenDetalleRepository().GetById(voldetcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CM_VOLUMEN_DETALLE
        /// </summary>
        public List<CmVolumenDetalleDTO> ListCmVolumenDetalles()
        {
            return FactorySic.GetCmVolumenDetalleRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CmVolumenDetalle
        /// </summary>
        public List<CmVolumenDetalleDTO> GetByCriteriaCmVolumenDetalles(int volcalcodi)
        {
            return FactorySic.GetCmVolumenDetalleRepository().GetByCriteria(volcalcodi);
        }

        #endregion

        #region Métodos Tabla CM_VOLUMEN_CALCULO

        /// <summary>
        /// Inserta un registro de la tabla CM_VOLUMEN_CALCULO
        /// </summary>
        public int SaveCmVolumenCalculo(CmVolumenCalculoDTO entity)
        {
            try
            {
                return FactorySic.GetCmVolumenCalculoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla CM_VOLUMEN_CALCULO
        /// </summary>
        public void UpdateCmVolumenCalculo(CmVolumenCalculoDTO entity)
        {
            try
            {
                FactorySic.GetCmVolumenCalculoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla CM_VOLUMEN_CALCULO
        /// </summary>
        public void DeleteCmVolumenCalculo(int volcalcodi)
        {
            try
            {
                FactorySic.GetCmVolumenCalculoRepository().Delete(volcalcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CM_VOLUMEN_CALCULO
        /// </summary>
        public CmVolumenCalculoDTO GetByIdCmVolumenCalculo(int volcalcodi)
        {
            var obj = FactorySic.GetCmVolumenCalculoRepository().GetById(volcalcodi);
            FormatearCmVolumenCalculo(obj);
            return obj;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla CM_VOLUMEN_CALCULO
        /// </summary>
        public List<CmVolumenCalculoDTO> ListCmVolumenCalculos()
        {
            return FactorySic.GetCmVolumenCalculoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CmVolumenCalculo
        /// </summary>
        public List<CmVolumenCalculoDTO> GetByCriteriaCmVolumenCalculos(DateTime fechaConsulta, int periodoH)
        {
            var lista = FactorySic.GetCmVolumenCalculoRepository().GetByCriteria(fechaConsulta, periodoH);

            foreach (var obj in lista)
                FormatearCmVolumenCalculo(obj);

            return lista;
        }

        private void FormatearCmVolumenCalculo(CmVolumenCalculoDTO obj)
        {
            if (obj != null)
            {
                obj.VolcalfechaDesc = obj.Volcalfecha.ToString(ConstantesAppServicio.FormatoFecha);
                obj.VolcalfeccreacionDesc = obj.Volcalfeccreacion.ToString(ConstantesAppServicio.FormatoFechaFull);
                obj.VolcalperiodoDesc = obj.Volcalperiodo < 48 ? DateTime.Today.AddMinutes(30 * obj.Volcalperiodo).ToString(ConstantesAppServicio.FormatoOnlyHora) : "23:59";
                obj.VolcaltipoDesc = obj.Volcaltipo == ConstHidrologia.CalculoVolumenAutomatico ? "Automático" : "Reproceso";
            }
        }

        #endregion

        /// <summary>
        /// Almacenar información del calculo Automático del volumen de embalse
        /// </summary>
        /// <param name="fechaHora"></param>
        public void ProcesarCmVolumenDetalle(DateTime fechaHora, int periodo, bool reproceso)
        {
            Logger.Info("ProcesarCmVolumenDetalle (fechaHora):\t" + fechaHora.ToString());
            Logger.Info("ProcesarCmVolumenDetalle (periodo):\t" + periodo);
            Logger.Info("ProcesarCmVolumenDetalle (reproceso):\t" + reproceso.ToString());
            
            DateTime fechaDatos = (fechaHora.Hour == 0 && fechaHora.Minute == 0) ? fechaHora.AddMinutes(-1) : fechaHora;
            Logger.Info("ProcesarCmVolumenDetalle (recalcularVolumen)(fechaDatos):\t" + fechaDatos.ToString());

            bool recalcularVolumen = false;

            if (!reproceso){ 
                recalcularVolumen = true;
                Logger.Info("ProcesarCmVolumenDetalle (recalcularVolumen)(if):\t" + recalcularVolumen.ToString());
            }
            else
            {
                Logger.Info("ProcesarCmVolumenDetalle (recalcularVolumen)(else):\t" + recalcularVolumen.ToString());
                if (fechaDatos.Year == DateTime.Now.Year && fechaDatos.Month == DateTime.Now.Month &&
                   fechaDatos.Day == DateTime.Now.Day)
                {
                    recalcularVolumen = true;
                }
            }

            Logger.Info("ProcesarCmVolumenDetalle (recalcularVolumen):\t" + recalcularVolumen.ToString());

            if (recalcularVolumen)
            {
                int posH = periodo;
                EjecutarProcesoCalculoCmVolumenDetalle(fechaDatos, posH, "cmgPr07", ConstHidrologia.CalculoVolumenAutomatico);
            }
        }

        public void EjecutarProcesoCalculoCmVolumenDetalle(DateTime fechaHoraInput, int posH, string usuario, string tipo)
        {
            DateTime fechaHora = fechaHoraInput;

            //1. Generar detalle
            var listaDet = ListarVolumenDetalleHm3XFechaHoraBD(fechaHora, posH, out List<CpMedicion48DTO> lista48YupanaVolumen
                                    , out List<MeMedicion24DTO> lista24HidroVolumen, out List<MeMedicion24DTO> lista24HidroVolumenSig, out List<CmModeloEmbalseDTO> listaEmbalse
                                    , out List<CmVolumenDetalleDTO> listaDetalleAyer, out ConfiguracionDefaultEmbalseHidrologia objConf);

            CmVolumenCalculoDTO objCab = new CmVolumenCalculoDTO()
            {
                Volcalfecha = fechaHora.Date,
                Volcalperiodo = posH,
                Volcaltipo = tipo,
                Volcalusucreacion = usuario,
                Volcalfeccreacion = DateTime.Now
            };

            GuardarVolumenCaudalCmgCP(objCab, listaDet);
        }

        public List<CmVolumenDetalleDTO> ObtenerVolumenProceso(DateTime fechaHora, int periodoData, bool reproceso)
        {
            int periodoReproceso = periodoData;
            DateTime fechaDatos = (fechaHora.Hour == 0 && fechaHora.Minute == 0) ? fechaHora.AddMinutes(-1) : fechaHora;

            //- Verificamos si el proceso no corresponde a la fecha actual
            if (!(fechaDatos.Year == DateTime.Now.Year && fechaDatos.Month == DateTime.Now.Month &&
                fechaDatos.Day == DateTime.Now.Day))
            {
                if (reproceso) periodoReproceso = 48;
            }

            return this.ListarCmVolumenDetalleBD(fechaDatos, periodoReproceso, periodoData).Where(x => x.Modcomtipo == ConstHidrologia.ComponenteTipoVolumenEmbalse).ToList();
        }

        /// <summary>
        /// Listar última información del volumen de embalse (automático o reproceso)
        /// </summary>
        /// <param name="fechaHora"></param>
        /// <param name="periodoReproceso"></param>
        /// <param name="periodoData"></param>
        /// <returns></returns>
        public List<CmVolumenDetalleDTO> ListarCmVolumenDetalleBD(DateTime fechaHora, int periodoReproceso, int periodoData)
        {
            var listaDet = new List<CmVolumenDetalleDTO>();

            List<CmVolumenCalculoDTO> listaEnvio = GetByCriteriaCmVolumenCalculos(fechaHora.Date, periodoReproceso).OrderByDescending(x => x.Volcalcodi).ToList();
            if (listaEnvio.Any())
            {
                int volcalcodi = listaEnvio.First().Volcalcodi;

                listaDet = GetByCriteriaCmVolumenDetalles(volcalcodi);

                //setear valores
                List<int> lModcomcodi = listaDet.Select(x => x.Modcomcodi).ToList();
                List<CmModeloComponenteDTO> listaComp = GetByCriteriaCmModeloComponentes(ConstantesAppServicio.ParametroDefecto, string.Join(",", lModcomcodi));

                foreach (var objDet in listaDet)
                {
                    var objComp = listaComp.Find(x => x.Modcomcodi == objDet.Modcomcodi);
                    var vol = objDet.GetType().GetProperty("H" + periodoData).GetValue(objDet);

                    GetDescripcionCmVolumenDetalle(objComp?.Modcomtipo, objComp?.Recurnombre, objComp?.Equinomb
                                            , out string ptomedidesc, out string unidadMedida, out string unidadMedidaAbrev, out int orden);
                    objDet.Recurnombre = (objComp?.Recurnombre) ?? "";
                    objDet.Ptomedidesc = ptomedidesc;
                    objDet.UnidadMedida = unidadMedida;
                    objDet.UnidadMedidaAbrev = unidadMedidaAbrev;
                    objDet.Recurcodi = objComp.Recurcodi;
                    objDet.Modcomtipo = objComp?.Modcomtipo;
                    objDet.Equicodi = objComp?.Equicodi;
                    objDet.Volumen = (vol != null) ? (decimal)vol : 0;
                    objDet.Orden = orden;

                }
            }

            return listaDet;
        }

        /// <summary>
        /// ListarVolumenDetalleXFechaHoraBD
        /// </summary>
        /// <param name="fechaHora"></param>
        /// <returns></returns>
        private List<CmVolumenDetalleDTO> ListarVolumenDetalleHm3XFechaHoraBD(DateTime fechaHora, int posH
                                    , out List<CpMedicion48DTO> lista48YupanaVolumen, out List<MeMedicion24DTO> lista24HidroVolumen, out List<MeMedicion24DTO> lista24HidroVolumenSig, out List<CmModeloEmbalseDTO> listaEmbalse
                                    , out List<CmVolumenDetalleDTO> listaDetalleAyer, out ConfiguracionDefaultEmbalseHidrologia objConf)
        {
            List<CmVolumenDetalleDTO> listaDet = new List<CmVolumenDetalleDTO>();

            //1. Obtener embalses vigentes para el dia a procesar
            List<CmModeloEmbalseDTO> listaEmbalseBD = ListarModeloEmbalseVigente(fechaHora.Date);

            listaEmbalse = new List<CmModeloEmbalseDTO>();
            foreach (var objEmb in listaEmbalseBD)
            {
                //obtener configuración de modelo de embalse
                listaEmbalse.Add(ObtenerModeloEmbalseXId(objEmb.Modembcodi));
            }

            //2. obtener Rendimiento de las centrales hidro
            objConf = ObtenerFiltroModeloEmbalse(fechaHora.Date);

            //3. Obtener datos de potencia activa IDCC
            List<MeMedicion48DTO> lista48IDCC = FactorySic.GetMeMedicion48Repository().ListarMeMedicion48ByFiltro("93", fechaHora.Date, fechaHora.Date, ConstantesAppServicio.ParametroDefecto)
                                                .Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMW).ToList();

            //4. Obtener datos de caudal / volumen hidrologia TR
            string idsEmpresa = ConstantesAppServicio.ParametroDefecto;
            string idsCuenca = ConstantesAppServicio.ParametroDefecto;
            string idsFamilia = ConstantesAppServicio.ParametroDefecto;
            string idsTptoMedicion = "1,2,3,4,5,10,14,16,24,17,18,19,8,7,9,11,12,13,84";
            List<MeMedicion24DTO> lista24Hidro = ListaMed24Hidrologia(66, 16, idsEmpresa, idsCuenca, idsFamilia, fechaHora.Date, fechaHora.Date, idsTptoMedicion);
            List<MeMedicion24DTO> lista24HidroCaudal = lista24Hidro.Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiM3s).ToList();
            lista24HidroVolumen = lista24Hidro.Where(x => x.Tipoinfocodi == ConstantesAppServicio.Tipoinfocodim3 || x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiHm3).ToList();

            List<MeMedicion24DTO> lista24HidroSig = ListaMed24Hidrologia(66, 16, idsEmpresa, idsCuenca, idsFamilia, fechaHora.Date.AddDays(1), fechaHora.Date.AddDays(1), idsTptoMedicion);
            List<MeMedicion24DTO> lista24HidroCaudalSig = lista24HidroSig.Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiM3s).ToList();
            lista24HidroVolumenSig = lista24HidroSig.Where(x => x.Tipoinfocodi == ConstantesAppServicio.Tipoinfocodim3 || x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiHm3).ToList();

            //5. Aportes Embalse Yupana
            int topcodi = GetTopologiaByDate(fechaHora.Date, posH);//cambiar fecha y hora
            List<CpMedicion48DTO> lista48YupanaCaudal = ListarYupana48XTipoInformacion(fechaHora.Date, posH, ConstantesBase.SRES_APORTES_EMB);

            List<CpMedicion48DTO> lista48YupanaMW = ListarYupana48XTipoInformacion(fechaHora.Date, posH, ConstantesBase.SresPotHidro);
            lista48YupanaVolumen = ListarYupana48XTipoInformacion(fechaHora.Date, posH, ConstantesBase.SresVolEmb);

            //6. Obtener valores minimos y maximos de los embalses yupana
            List<CpProprecursoDTO> listaPropVolHm3 = FactorySic.GetCpProprecursoRepository().ListarPropiedadxRecurso2(-1, ConstantesBase.Embalse.ToString(), topcodi, -1);

            //7. Datos del TNA
            List<CmGeneracionEmsDTO> listaEmsTNA = ListarEmsFecha(fechaHora.Date, posH);

            //8. Volumen CmgCP del día anteior
            listaDetalleAyer = ListarCmVolumenDetalleBD(fechaHora.Date.AddDays(-1), 48, 48);

            //Generar cabecera
            foreach (var objEmb in listaEmbalse)
            {
                var listaCompCentralAportante = objEmb.ListaComponente.Where(x => x.Modcomtipo == ConstHidrologia.ComponenteTipoCentralAguaArriba).ToList();
                foreach (var objComp in listaCompCentralAportante)
                {
                    EqEquipoDTO eqCentral = objConf.ListaEqCentral.Find(x => x.Equicodi == objComp.Equicodi);
                    GetDescripcionCmVolumenDetalle(ConstHidrologia.ComponenteTipoCentralAguaArriba, null, eqCentral?.Equinomb, out string ptomedidesc, out string unidadMedida, out string unidadMedidaAbrev, out int orden);

                    int hViaje = 0;
                    if (objComp.Modcomtviaje > 0)
                    {
                        hViaje = Convert.ToInt32(objComp.Modcomtviaje.Value * 2);
                        if (hViaje > 48) hViaje = 48;
                    }
                    CmVolumenDetalleDTO objDet1 = new CmVolumenDetalleDTO()
                    {
                        Modcomcodi = objComp.Modcomcodi,
                        Modembcodi = objEmb.Modembcodi,
                        Equicodi = objComp.Equicodi,
                        Recurcodi = objEmb.Recurcodi,
                        Recurnombre = objEmb.Recurnombre,
                        Modcomtipo = ConstHidrologia.ComponenteTipoCentralAguaArriba,
                        Ptomedidesc = ptomedidesc,
                        Tipoinfocodi = ConstantesAppServicio.TipoinfocodiMW,
                        UnidadMedida = unidadMedida,
                        UnidadMedidaAbrev = unidadMedidaAbrev,
                        HTiempoViaje = hViaje,
                        Orden = orden
                    };

                    listaDet.Add(objDet1);
                }

                var listaCompCaudalAportanteCentralAportante = objEmb.ListaComponente.Where(x => x.Modcomtipo == ConstHidrologia.ComponenteTipoCaudalTurbinadoCentralAguaArriba).ToList();
                foreach (var objComp in listaCompCaudalAportanteCentralAportante)
                {
                    EqEquipoDTO eqCentral = objConf.ListaEqCentral.Find(x => x.Equicodi == objComp.Equicodi);
                    GetDescripcionCmVolumenDetalle(ConstHidrologia.ComponenteTipoCaudalTurbinadoCentralAguaArriba, null, eqCentral?.Equinomb, out string ptomedidesc, out string unidadMedida, out string unidadMedidaAbrev, out int orden);

                    CmVolumenDetalleDTO objDet2 = new CmVolumenDetalleDTO()
                    {
                        Modcomcodi = objComp.Modcomcodi,
                        Modembcodi = objEmb.Modembcodi,
                        Equicodi = objComp.Equicodi,
                        Recurcodi = objEmb.Recurcodi,
                        Recurnombre = objEmb.Recurnombre,
                        Modcomtipo = ConstHidrologia.ComponenteTipoCaudalTurbinadoCentralAguaArriba,
                        Ptomedidesc = ptomedidesc,
                        Tipoinfocodi = ConstantesAppServicio.TipoinfocodiM3s,
                        UnidadMedida = unidadMedida,
                        UnidadMedidaAbrev = unidadMedidaAbrev,
                        Orden = orden
                    };

                    listaDet.Add(objDet2);
                }

                var listaCompCaudalEntrada = objEmb.ListaComponente.Where(x => x.Modcomtipo == ConstHidrologia.ComponenteTipoCaudal).ToList();
                foreach (var objComp in listaCompCaudalEntrada)
                {
                    GetDescripcionCmVolumenDetalle(ConstHidrologia.ComponenteTipoCaudal, null, null, out string ptomedidesc, out string unidadMedida, out string unidadMedidaAbrev, out int orden);

                    CmVolumenDetalleDTO objDet1 = new CmVolumenDetalleDTO()
                    {
                        Modcomcodi = objComp.Modcomcodi,
                        Modembcodi = objEmb.Modembcodi,
                        Recurcodi = objEmb.Recurcodi,
                        Recurnombre = objEmb.Recurnombre,
                        Modcomtipo = ConstHidrologia.ComponenteTipoCaudal,
                        Ptomedidesc = ptomedidesc,
                        Tipoinfocodi = ConstantesAppServicio.TipoinfocodiM3s,
                        UnidadMedida = unidadMedida,
                        UnidadMedidaAbrev = unidadMedidaAbrev,
                        Orden = orden
                    };

                    listaDet.Add(objDet1);
                }
                var listaCompCentralTurbinante = objEmb.ListaComponente.Where(x => x.Modcomtipo == ConstHidrologia.ComponenteTipoCentralTurbinante).ToList();
                foreach (var objComp in listaCompCentralTurbinante)
                {
                    EqEquipoDTO eqCentral = objConf.ListaEqCentral.Find(x => x.Equicodi == objComp.Equicodi);
                    GetDescripcionCmVolumenDetalle(ConstHidrologia.ComponenteTipoCentralTurbinante, null, eqCentral?.Equinomb, out string ptomedidesc, out string unidadMedida, out string unidadMedidaAbrev, out int orden);

                    CmVolumenDetalleDTO objDet1 = new CmVolumenDetalleDTO()
                    {
                        Modcomcodi = objComp.Modcomcodi,
                        Modembcodi = objEmb.Modembcodi,
                        Equicodi = objComp.Equicodi,
                        Recurcodi = objEmb.Recurcodi,
                        Recurnombre = objEmb.Recurnombre,
                        Modcomtipo = ConstHidrologia.ComponenteTipoCentralTurbinante,
                        Ptomedidesc = ptomedidesc,
                        Tipoinfocodi = ConstantesAppServicio.TipoinfocodiMW,
                        UnidadMedida = unidadMedida,
                        UnidadMedidaAbrev = unidadMedidaAbrev,
                        Orden = orden
                    };

                    listaDet.Add(objDet1);
                }

                var objCompVolumen = objEmb.ListaComponente.Find(x => x.Modcomtipo == ConstHidrologia.ComponenteTipoVolumenEmbalse);
                if (objCompVolumen != null)
                {
                    GetDescripcionCmVolumenDetalle(ConstHidrologia.ComponenteTipoVolumenEmbalse, null, null, out string ptomedidesc, out string unidadMedida, out string unidadMedidaAbrev, out int orden);

                    var objPropVmin = listaPropVolHm3.Find(x => x.Recurcodi == objEmb.Recurcodi && x.Propcodi == 28); //Hm3
                    var objPropVmax = listaPropVolHm3.Find(x => x.Recurcodi == objEmb.Recurcodi && x.Propcodi == 29); //Hm3
                    decimal vmin = 0;
                    if (objPropVmin != null && objPropVmin.Valor != null) decimal.TryParse(objPropVmin.Valor, out vmin);
                    vmin = vmin * ConstHidrologia.Hm3ToMm3;

                    decimal vmax = 0;
                    if (objPropVmax != null && objPropVmax.Valor != null) decimal.TryParse(objPropVmax.Valor, out vmax);
                    vmax = vmax * ConstHidrologia.Hm3ToMm3;

                    CmVolumenDetalleDTO objDet5 = new CmVolumenDetalleDTO()
                    {
                        Modcomcodi = objCompVolumen.Modcomcodi,
                        Modembcodi = objEmb.Modembcodi,
                        Recurcodi = objEmb.Recurcodi,
                        Recurnombre = objEmb.Recurnombre,
                        Modcomtipo = ConstHidrologia.ComponenteTipoVolumenEmbalse,
                        CodigoPtoVolExtranet = objEmb.Ptomedicodi,
                        Ptomedidesc = ptomedidesc,
                        UnidadMedida = unidadMedida,
                        UnidadMedidaAbrev = unidadMedidaAbrev,
                        VMin = vmin,
                        VMax = vmax,
                        Orden = orden
                    };

                    listaDet.Add(objDet5);
                }
            }

            //generar datos cada 30min (caudal y potencia efectiva)
            foreach (var objDet in listaDet)
            {
                var objEmb = listaEmbalse.Find(x => x.Modembcodi == objDet.Modembcodi);
                var objComp = objEmb.ListaComponente.Find(x => x.Modcomcodi == objDet.Modcomcodi);

                if (objComp != null)
                {
                    //Caudal
                    if (ConstHidrologia.ComponenteTipoCaudal == objComp.Modcomtipo
                        && ConstantesAppServicio.TipoinfocodiM3s == objDet.Tipoinfocodi)
                    {
                        if (objEmb.Recurcodi == 1042)
                        { }

                        for (int h = 1; h <= 48; h++)
                        {
                            decimal? valorHComp = null;
                            int? tipoH = null;
                            foreach (var objAgrup in objComp.ListaAgrupacion)
                            {
                                decimal? valorHAgrup = null;
                                int? tipoHAgrup = ConstHidrologia.FuenteCauExtEjec;
                                var listaConfCauEnt = objAgrup.ListaConfiguracion.Where(x => x.Modcontipo == ConstHidrologia.ConfiguracionTipoCaudalExtranet).ToList();
                                bool existeCauEnt = false;

                                //1.Caudal de entrada 
                                if (listaConfCauEnt.Any())
                                {
                                    existeCauEnt = true;
                                    int h24 = h / 2 + 1;
                                    foreach (var objConfig in listaConfCauEnt)
                                    {
                                        decimal factor = objConfig.Modconsigno == "+" ? 1.0m : -1.0m;

                                        MeMedicion24DTO objM24 = null;
                                        if (h <= 47)
                                        {
                                            objM24 = lista24HidroCaudal.Find(x => x.Ptomedicodi == objConfig.Ptomedicodi);
                                        }
                                        else
                                        {
                                            h24 = 1;
                                            objM24 = lista24HidroCaudalSig.Find(x => x.Ptomedicodi == objConfig.Ptomedicodi);
                                        }
                                        decimal? valorM24 = null;
                                        int? tipoM24 = null;
                                        if (objM24 != null)
                                        {
                                            valorM24 = (decimal?)objM24.GetType().GetProperty("H" + h24).GetValue(objM24, null);
                                            tipoM24 = (int?)objM24.GetType().GetProperty("T" + h24).GetValue(objM24, null);
                                        }
                                        if (valorM24 != null) valorHAgrup = (valorHAgrup.GetValueOrDefault(0) + valorM24.GetValueOrDefault(0) * factor);
                                        if (tipoM24 != null) tipoHAgrup = tipoM24;
                                        existeCauEnt = existeCauEnt && (valorM24 != null);
                                    }

                                    if (!existeCauEnt) valorHAgrup = null; //si la fuente no tiene datos entonces no considerarlo 
                                    else tipoH = tipoHAgrup;
                                }

                                if (!existeCauEnt || objEmb.Modembindyupana == ConstantesAppServicio.SI)
                                {
                                    var listaConfCauYup = objAgrup.ListaConfiguracion.Where(x => x.Modcontipo == ConstHidrologia.ConfiguracionTipoCaudalYupana).ToList();
                                    bool existeCauYup = listaConfCauYup.Count() > 0;

                                    //2.Caudal Yupana
                                    if (existeCauYup)
                                    {
                                        tipoH = ConstHidrologia.FuenteCauYupana;
                                        foreach (var objConfig in listaConfCauYup)
                                        {
                                            decimal factor = objConfig.Modconsigno == "+" ? 1.0m : -1.0m;
                                            var obj48 = lista48YupanaCaudal.Find(x => x.Recurcodi == objConfig.Recurcodi);
                                            decimal? valorM48 = null;
                                            if (obj48 != null)
                                            {
                                                valorM48 = (decimal?)obj48.GetType().GetProperty("H" + h).GetValue(obj48, null);
                                            }
                                            if (valorM48 != null) valorHAgrup = (valorHAgrup.GetValueOrDefault(0) + valorM48.GetValueOrDefault(0) * factor);
                                            existeCauYup = existeCauYup && (valorM48 != null);
                                        }

                                        if (!existeCauYup) valorHAgrup = null; //si la fuente no tiene datos entonces no considerarlo 
                                        //else tipoH = ConstHidrologia.FuenteCauYupana;
                                    }
                                }

                                //agregar valor de agrup al componente
                                if (valorHAgrup != null) valorHComp = valorHComp.GetValueOrDefault(0) + valorHAgrup.GetValueOrDefault(0);
                            }

                            objDet.GetType().GetProperty("H" + h).SetValue(objDet, valorHComp);
                            objDet.GetType().GetProperty("T" + h).SetValue(objDet, tipoH);
                        }
                    }

                    //Potencia efectiva
                    if ((ConstHidrologia.ComponenteTipoCentralAguaArriba == objComp.Modcomtipo
                    || ConstHidrologia.ComponenteTipoCentralTurbinante == objComp.Modcomtipo)
                    && ConstantesAppServicio.TipoinfocodiMW == objDet.Tipoinfocodi)
                    {
                        if (objEmb.Recurcodi == 840)
                        { }

                        for (int h = 1; h <= 48; h++)
                        {
                            DateTime fechaHoraEms = fechaHora.Date.AddMinutes(h * 30);
                            if (h == 48) fechaHoraEms = fechaHoraEms.AddMinutes(-1);

                            decimal? valorHComp = null;
                            int? tipoH = null;
                            foreach (var objAgrup in objComp.ListaAgrupacion)
                            {
                                decimal? valorHAgrup = null;
                                var listaConfIdcc = objAgrup.ListaConfiguracion.Where(x => x.Modcontipo == ConstHidrologia.ConfiguracionTipoPotIDCC).ToList();
                                var listaConfPotTNA = objAgrup.ListaConfiguracion.Where(x => x.Modcontipo == ConstHidrologia.ConfiguracionTipoPotTNA).ToList();
                                var listaConfPotYupana = objAgrup.ListaConfiguracion.Where(x => x.Modcontipo == ConstHidrologia.ConfiguracionTipoPotYupana).ToList();
                                bool existeIdcc = false;
                                bool existePotTNA = true;
                                bool existeYupana = true;

                                //1. Datos Idcc
                                if (listaConfIdcc.Any())
                                {
                                    existeIdcc = true;
                                    foreach (var objConfig in listaConfIdcc)
                                    {
                                        decimal factor = objConfig.Modconsigno == "+" ? 1.0m : -1.0m;
                                        var objM48 = lista48IDCC.Find(x => x.Ptomedicodi == objConfig.Ptomedicodi);
                                        decimal? valorHIdcc = null;
                                        if (objM48 != null)
                                        {
                                            valorHIdcc = (decimal?)objM48.GetType().GetProperty("H" + h).GetValue(objM48, null);
                                        }
                                        if (valorHIdcc != null) valorHAgrup = (valorHAgrup.GetValueOrDefault(0) + valorHIdcc.GetValueOrDefault(0) * factor);
                                        existeIdcc = existeIdcc && (valorHIdcc != null);
                                    }

                                    if (!existeIdcc) valorHAgrup = null; //si la fuente no tiene datos entonces no considerarlo 
                                    else tipoH = ConstHidrologia.FuentePotIDCC;
                                }

                                //2. Verificar TNA 
                                if (!existeIdcc)
                                {
                                    existePotTNA = false; //inicializar

                                    foreach (var objConfig in listaConfPotTNA)
                                    {
                                        decimal factor = objConfig.Modconsigno == "+" ? 1.0m : -1.0m;
                                        var objEms = listaEmsTNA.Find(x => x.Equicodi == objConfig.Equicodi && x.Genemsfecha == fechaHoraEms);
                                        decimal? valorEms = null;
                                        if (objEms != null)
                                        {
                                            valorEms = objEms.Genemsgeneracion;
                                        }
                                        if (valorEms != null) valorHAgrup = (valorHAgrup.GetValueOrDefault(0) + valorEms.GetValueOrDefault(0) * factor);
                                        existePotTNA = existePotTNA || (valorEms != null);
                                    }

                                    if (!existePotTNA) valorHAgrup = null; //si la fuente no tiene datos entonces no considerarlo 
                                    else tipoH = ConstHidrologia.FuentePotTNA;
                                }

                                //3. Verificar YUPANA
                                if (!existeIdcc && !existePotTNA)
                                {
                                    existeYupana = false;

                                    foreach (var objConfig in listaConfPotYupana)
                                    {
                                        decimal factor = objConfig.Modconsigno == "+" ? 1.0m : -1.0m;
                                        var obj48 = lista48YupanaMW.Find(x => x.Recurcodi == objConfig.Recurcodi);
                                        decimal? valorM48 = null;
                                        if (obj48 != null)
                                        {
                                            valorM48 = (decimal?)obj48.GetType().GetProperty("H" + h).GetValue(obj48, null);
                                        }
                                        if (valorM48 != null) valorHAgrup = (valorHAgrup.GetValueOrDefault(0) + valorM48.GetValueOrDefault(0) * factor);
                                        existeYupana = existeYupana || (valorM48 != null);
                                    }

                                    if (!existeYupana) valorHAgrup = null; //si la fuente no tiene datos entonces no considerarlo 
                                    else tipoH = ConstHidrologia.FuentePotYupana;
                                }

                                //agregar valor de agrup al componente
                                if (valorHAgrup != null) valorHComp = valorHComp.GetValueOrDefault(0) + valorHAgrup.GetValueOrDefault(0);
                            }

                            objDet.GetType().GetProperty("H" + h).SetValue(objDet, valorHComp);
                            objDet.GetType().GetProperty("T" + h).SetValue(objDet, tipoH);
                        }
                    }
                }
            }

            //generar datos cada 30min (caudal turbinado de central agua arriba)
            foreach (var objDet in listaDet)
            {
                var objEmb = listaEmbalse.Find(x => x.Modembcodi == objDet.Modembcodi);
                var objComp = objEmb.ListaComponente.Find(x => x.Modcomcodi == objDet.Modcomcodi);

                if (objComp != null)
                {
                    //Caudal Turbinante de Central Agua arriba
                    if (ConstHidrologia.ComponenteTipoCaudalTurbinadoCentralAguaArriba == objComp.Modcomtipo)
                    {
                        EqEquipoDTO eqCentral = objConf.ListaEqCentral.Find(x => x.Equicodi == objComp.Equicodi);
                        var objDetCentralMW = listaDet.Find(x => x.Equicodi == objComp.Equicodi && x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMW);

                        for (int h = 1; h <= 48; h++)
                        {
                            decimal? valorMW = (decimal?)objDetCentralMW.GetType().GetProperty("H" + h).GetValue(objDetCentralMW, null);
                            int? tipoMW = 1;//(int?)objDetCentralMW.GetType().GetProperty("T" + h).GetValue(objDetCentralMW, null);

                            decimal? caudal = null;
                            if (valorMW != null && eqCentral.Rendimiento > 0)
                            {
                                caudal = valorMW / eqCentral.Rendimiento;
                            }

                            objDet.GetType().GetProperty("H" + h).SetValue(objDet, caudal);
                            objDet.GetType().GetProperty("T" + h).SetValue(objDet, tipoMW);
                        }
                    }

                }
            }

            //generar datos cada 30min (volumen de cada embalse)
            CalcularVolumen30min(listaDet, lista48YupanaVolumen, lista24HidroVolumen, lista24HidroVolumenSig, listaEmbalse, listaDetalleAyer, objConf);

            //ordenar
            listaDet = listaDet.OrderBy(x => x.Recurnombre).ThenBy(x => x.Orden).ToList();
            return listaDet;
        }

        private void CalcularVolumen30min(List<CmVolumenDetalleDTO> listaDet, List<CpMedicion48DTO> lista48YupanaVolumen, List<MeMedicion24DTO> lista24HidroVolumen, List<MeMedicion24DTO> lista24HidroVolumenSig
                                    , List<CmModeloEmbalseDTO> listaEmbalse
                                    , List<CmVolumenDetalleDTO> listaDetalleAyer, ConfiguracionDefaultEmbalseHidrologia objConf)
        {
            foreach (var objDet in listaDet)
            {
                var objEmb = listaEmbalse.Find(x => x.Modembcodi == objDet.Modembcodi);
                var objComp = objEmb.ListaComponente.Find(x => x.Modcomcodi == objDet.Modcomcodi);

                if (objComp != null)
                {
                    //Volumen de embalse
                    if (ConstHidrologia.ComponenteTipoVolumenEmbalse == objComp.Modcomtipo)
                    {
                        if (objDet.Recurcodi == 103)
                        { }

                        //volumen ejecutado
                        var obj24VolEjec = lista24HidroVolumen.Find(x => x.Ptomedicodi == objDet.CodigoPtoVolExtranet);

                        //volumen ejecutado de las 23:59 (00:00 de mañana)
                        var obj24VolEjecSig = lista24HidroVolumenSig.Find(x => x.Ptomedicodi == objDet.CodigoPtoVolExtranet);

                        //volumen yupana
                        var obj48Yup = lista48YupanaVolumen.Find(x => x.Recurcodi == objEmb.Recurcodi);

                        //datos de hoy
                        var listaDetXEmbalse = listaDet.Where(x => x.Recurcodi == objDet.Recurcodi).ToList();
                        var objDetCaudal = listaDetXEmbalse.Find(x => x.Modcomtipo == ConstHidrologia.ComponenteTipoCaudal);
                        var listaDetCAA = listaDetXEmbalse.Where(x => x.Modcomtipo == ConstHidrologia.ComponenteTipoCentralAguaArriba).ToList();
                        var listaDetCT = listaDetXEmbalse.Where(x => x.Modcomtipo == ConstHidrologia.ComponenteTipoCentralTurbinante).ToList();

                        //datos de ayer
                        var listaDetXEmbalseAyer = listaDetalleAyer.Where(x => x.Recurcodi == objDet.Recurcodi).ToList();
                        var objVolAyer = listaDetXEmbalseAyer.Find(x => x.Modcomtipo == ConstHidrologia.ComponenteTipoVolumenEmbalse);
                        var listaDetAyerCAA = listaDetXEmbalseAyer.Where(x => x.Modcomtipo == ConstHidrologia.ComponenteTipoCentralAguaArriba).ToList();
                        var listaDetAyerCT = listaDetXEmbalseAyer.Where(x => x.Modcomtipo == ConstHidrologia.ComponenteTipoCentralTurbinante).ToList();

                        for (int h = 1; h <= 48; h++)
                        {
                            //obtener volumen historico
                            decimal? valorVolEjec = null;
                            int? tipoVolEjec = null;

                            if (h % 2 == 0)
                            {
                                if (h <= 46) //01:00, 02:00 ... 23:00 (Volumen Extranet)
                                {
                                    if (obj24VolEjec != null)
                                    {
                                        int h24 = h / 2 + 1;
                                        valorVolEjec = (decimal?)obj24VolEjec.GetType().GetProperty("H" + h24).GetValue(obj24VolEjec, null); //Mm3
                                        if (valorVolEjec != null) valorVolEjec = valorVolEjec * ConstHidrologia.Hm3ToMm3; //Hm3 a Mm3

                                        tipoVolEjec = (int?)obj24VolEjec.GetType().GetProperty("T" + h24).GetValue(obj24VolEjec, null);
                                    }
                                }
                                else //h==48. 24:00 (primer registro del día siguiente)
                                {
                                    if (obj24VolEjecSig != null)
                                    {
                                        int h24 = 1; //00:00
                                        valorVolEjec = (decimal?)obj24VolEjecSig.GetType().GetProperty("H" + h24).GetValue(obj24VolEjecSig, null); //Mm3
                                        if (valorVolEjec != null) valorVolEjec = valorVolEjec * ConstHidrologia.Hm3ToMm3; //Hm3 a Mm3

                                        tipoVolEjec = (int?)obj24VolEjecSig.GetType().GetProperty("T" + h24).GetValue(obj24VolEjecSig, null);
                                    }
                                }
                            }

                            //obtener volumen del embalse en Yupana
                            decimal? valorVolYup = null;
                            if (obj48Yup != null)
                            {
                                valorVolYup = (decimal?)obj48Yup.GetType().GetProperty("H" + h).GetValue(obj48Yup, null);
                                valorVolYup = (valorVolYup ?? 0) * ConstHidrologia.Hm3ToMm3;
                            }
                            //Obtener volumen Mm3 (historico o calculado) de la media hora anterior (h-1)
                            decimal? valorVmh_30 = 0;
                            if (h > 1)
                            {
                                //los valores de volumen del objeto ya estan convertidos a Mm3, no es necesario 
                                valorVmh_30 = (decimal?)objDet.GetType().GetProperty("H" + (h - 1)).GetValue(objDet, null);
                                //if (valorVmh_30 != null) valorVmh_30 = valorVmh_30 * ConstHidrologia.Hm3ToMm3; //Hm3 a Mm3
                            }
                            else
                            {
                                if (objVolAyer != null) valorVmh_30 = objVolAyer.Volumen * ConstHidrologia.Hm3ToMm3; //Hm3 a Mm3
                            }

                            //Obtener volumen Yupana, sino Ejecutado sino Calculado
                            decimal valorVmh = 0;
                            int? tipoVmh = null;

                            if (objEmb.Modembindyupana == ConstantesAppServicio.SI)
                            {
                                tipoVmh = ConstHidrologia.CalculoVolMedido;
                                valorVmh = valorVolYup ?? 0;
                            }
                            else
                            {
                                if (valorVolEjec != null)
                                {
                                    tipoVmh = ConstHidrologia.CalculoVolMedido;
                                    valorVmh = valorVolEjec ?? 0;
                                    if ((tipoVolEjec != null ? tipoVolEjec.Value.ToString() : "") == ConstHidrologia.TieneConCheckExtranet)
                                    {
                                        //Si un volumen de embalse, para una etapa medio horaria específica, tiene activado el check de la Extranet de Hidrología, esto significa que la central se encuentra en condición de vertimiento, y para efectos del cálculo de CMgCP debe considerar como si tuviera el valor de 𝑉𝑚𝑎𝑥 (𝑉𝑚ℎ = 𝑉𝑚𝑎𝑥). 
                                        valorVmh = objDet.VMax;
                                    }
                                }
                                else
                                {
                                    //obtener sumatoria de caudal
                                    decimal? valorQ = (decimal?)objDetCaudal.GetType().GetProperty("H" + h).GetValue(objDetCaudal, null);

                                    //obtener sumantoria de caudal de centrales aguas arriba (potencia / rendimiento)
                                    decimal? valorQCAA = ObtenerCaudalXListaCentral(h, listaDetCAA, listaDetAyerCAA, objConf.ListaEqCentral);

                                    //obtener sumantoria de caudal de centrales turbinante (potencia / rendimiento)
                                    decimal? valorQCT = ObtenerCaudalXListaCentral(h, listaDetCT, listaDetAyerCT, objConf.ListaEqCentral);

                                    //Obtener volumen calculado Mm3 de la media hora actual (H)
                                    tipoVmh = ConstHidrologia.CalculoVolCalculado;
                                    valorVmh = valorVmh_30.GetValueOrDefault(0) + 1.8m * (valorQ.GetValueOrDefault(0) + valorQCAA.GetValueOrDefault(0) - valorQCT.GetValueOrDefault(0));

                                    if (valorVmh > objDet.VMax) tipoVmh = ConstHidrologia.CalculoVolMayorVmax;
                                    if (valorVmh < objDet.VMin) tipoVmh = ConstHidrologia.CalculoVolMenorVmin;
                                }
                            }

                            //condiciones
                            if (valorVmh > objDet.VMax) valorVmh = objDet.VMax;
                            if (valorVmh < objDet.VMin) valorVmh = objDet.VMin;

                            objDet.GetType().GetProperty("H" + h).SetValue(objDet, valorVmh);
                            objDet.GetType().GetProperty("T" + h).SetValue(objDet, tipoVmh);
                        }
                    }

                }
            }

            //convertir Mm3 a Hm3
            foreach (var objDet in listaDet)
            {
                if (objDet.Modcomtipo == ConstHidrologia.ComponenteTipoVolumenEmbalse)
                {
                    objDet.VMax = objDet.VMax / ConstHidrologia.Hm3ToMm3;
                    objDet.VMin = objDet.VMin / ConstHidrologia.Hm3ToMm3;
                    for (int h = 1; h <= 48; h++)
                    {
                        decimal? valorH = (decimal?)objDet.GetType().GetProperty("H" + h).GetValue(objDet, null);
                        valorH = valorH.GetValueOrDefault(0) / ConstHidrologia.Hm3ToMm3;
                        objDet.GetType().GetProperty("H" + h).SetValue(objDet, valorH);
                    }
                }
            }
        }

        public List<CmVolumenDetalleDTO> RecalcularVolumenDetalle(List<CmVolumenDetalleDTO> listaInput, DateTime fechaHora, int posH)
        {
            List<CmVolumenDetalleDTO> listaDet = ListarVolumenDetalleHm3XFechaHoraBD(fechaHora, posH, out List<CpMedicion48DTO> lista48YupanaVolumen
                                    , out List<MeMedicion24DTO> lista24HidroVolumen, out List<MeMedicion24DTO> lista24HidroVolumenSig
                                    , out List<CmModeloEmbalseDTO> listaEmbalse
                                    , out List<CmVolumenDetalleDTO> listaDetalleAyer, out ConfiguracionDefaultEmbalseHidrologia objConf);

            //actualizar valor de los insumos
            foreach (var objDet in listaDet)
            {
                var objInput = listaInput.Find(x => x.Modcomcodi == objDet.Modcomcodi);
                decimal factor = 1.0m;
                //if (objDet.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiM3s) factor = 1000.0m;

                if (objInput != null)
                {
                    for (int h = 1; h <= 48; h++)
                    {
                        decimal? valorInput = (decimal?)objInput.GetType().GetProperty("H" + h).GetValue(objInput, null);
                        if (valorInput != null)
                        {
                            valorInput *= factor;
                        }

                        objDet.GetType().GetProperty("H" + h).SetValue(objDet, valorInput);

                        //verificar cambio
                    }
                }

                if (objDet.Modcomtipo == ConstHidrologia.ComponenteTipoVolumenEmbalse)
                {
                    objDet.VMax = objDet.VMax * ConstHidrologia.Hm3ToMm3;
                    objDet.VMin = objDet.VMin * ConstHidrologia.Hm3ToMm3;
                }
            }

            //volver a calcular el volumen
            CalcularVolumen30min(listaDet, lista48YupanaVolumen, lista24HidroVolumen, lista24HidroVolumenSig, listaEmbalse, listaDetalleAyer, objConf);

            return listaDet;
        }

        private decimal? ObtenerCaudalXListaCentral(int h, List<CmVolumenDetalleDTO> listaDet, List<CmVolumenDetalleDTO> listaDetAyer, List<EqEquipoDTO> listaEq)
        {
            decimal? valorQCentral = null;

            //lista de centrales aguas arriba / turbinante
            foreach (var objDetCent in listaDet)
            {
                EqEquipoDTO eqCentral = listaEq.Find(x => x.Equicodi == objDetCent.Equicodi);
                CmVolumenDetalleDTO objDetCentAyer = listaDetAyer.Find(x => x.Equicodi == objDetCent.Equicodi);

                decimal? caudal = null;
                decimal? valorMW = null;

                int hData = h;
                if (objDetCent.HTiempoViaje > 0)
                    hData -= objDetCent.HTiempoViaje;

                //obtner datos de MW del día de hoy
                if (hData > 0) valorMW = (decimal?)objDetCent.GetType().GetProperty("H" + hData).GetValue(objDetCent, null);
                else
                {
                    hData = 48 + hData;
                    if (objDetCentAyer != null) valorMW = (decimal?)objDetCentAyer.GetType().GetProperty("H" + hData).GetValue(objDetCentAyer, null);
                }

                if (eqCentral != null && eqCentral.Rendimiento > 0)
                {
                    if (valorMW != null)
                    {
                        caudal = valorMW / eqCentral.Rendimiento;
                        valorQCentral = valorQCentral.GetValueOrDefault(0) + caudal;
                    }
                }
            }

            return valorQCentral;
        }

        public List<CpMedicion48DTO> ListarYupana48XTipoInformacion(DateTime fecha, int posH, int srestcodi)
        {
            //obtener recursos
            List<CpMedicion48DTO> listaDataUnica = new List<CpMedicion48DTO>();

            try
            {
                List<CpTopologiaDTO> listaTopologiaFechaHora = ListarTopologiaYupana(fecha, posH).OrderBy(x => x.OrdenReprograma).ToList();

                //obtener data de todos los escenarios yupana
                List<CpMedicion48DTO> listaAllData = new List<CpMedicion48DTO>();
                foreach (var regTop in listaTopologiaFechaHora)
                {
                    listaAllData.AddRange(FactorySic.GetCpMedicion48Repository().GetByCriteria(regTop.Topcodi.ToString(), fecha.Date, srestcodi.ToString()));
                }

                List<int> lRecurcodi = listaAllData.Select(x => x.Recurcodi).Distinct().ToList();
                foreach (var recurcodi in lRecurcodi)
                {
                    listaDataUnica.Add(new CpMedicion48DTO() { Recurcodi = recurcodi });
                }

                //asignar datos de medias horas
                foreach (var regTop in listaTopologiaFechaHora)
                {
                    foreach (var recurcodi in lRecurcodi)
                    {
                        CpMedicion48DTO reg48BD = listaAllData.Find(x => x.Topcodi == regTop.Topcodi && x.Recurcodi == recurcodi);
                        CpMedicion48DTO reg48Val = listaDataUnica.Find(x => x.Recurcodi == recurcodi);

                        if (reg48BD != null && reg48Val != null)
                        {
                            for (var filaH = regTop.HoraReprograma; filaH <= 48; filaH++)
                            {
                                if (filaH > 0)
                                {
                                    decimal? valBD = (decimal?)reg48BD.GetType().GetProperty("H" + filaH).GetValue(reg48BD, null);

                                    reg48Val.GetType().GetProperty("H" + filaH).SetValue(reg48Val, valBD);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }

            return listaDataUnica;
        }

        /// <summary>
        /// Obtener la ultima topologia para la fecha actual
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        private int GetTopologiaByDate(DateTime fechaConsulta, int posH)
        {
            List<CpTopologiaDTO> listaTopologia = ListarTopologiaYupana(fechaConsulta, posH);

            //yupana vigente para la fecha y hora
            CpTopologiaDTO regTopFechaHora = listaTopologia.FirstOrDefault();
            if (regTopFechaHora != null) return regTopFechaHora.Topcodi;

            ////ultimo REPROGRMA
            //CpTopologiaDTO regTop = FactorySic.GetCpTopologiaRepository().ObtenerUltimoEscenarioReprogramado(fechaConsulta.Date);
            //if (regTop != null)
            //{
            //    return regTop.Topcodi;
            //}
            //else
            //{
            //    //si no existe reprogramado, utilizar el programado diario
            //    CpTopologiaDTO regTopProg = FactorySic.GetCpTopologiaRepository().GetByFechaTopfinal(fechaConsulta.Date, ConstantesCortoPlazo.TopologiaDiario);
            //    if (regTopProg != null)
            //        return regTopProg.Topcodi;
            //}

            return 0; //si retorna cero es porque no se esta usando la bd de prod
        }

        public List<CpTopologiaDTO> ListarTopologiaYupana(DateTime fechaConsulta, int posH)
        {
            List<CpTopologiaDTO> listaTopologia = new List<CpTopologiaDTO>();

            //programado diario
            CpTopologiaDTO regProg = FactorySic.GetCpTopologiaRepository().GetByFechaTopfinal(fechaConsulta.Date, ConstantesCortoPlazo.TopologiaDiario);
            if (regProg != null)
            {
                regProg.OrdenReprograma = -100;
                regProg.HoraReprograma = 1;

                //Lista de Escenarios reprogramados del dia de consulta
                List<CpTopologiaDTO> listaReprograma = FactorySic.GetCptopologiaRepository().GetByCriteria(fechaConsulta.Date, fechaConsulta.Date, ConstantesCortoPlazo.TopologiaReprograma);

                int topcodiProg = regProg.Topcodi;
                List<CpReprogramaDTO> listaReprog = FactorySic.GetCpReprogramaRepository().GetByCriteria(topcodiProg).Where(x => x.Tophorareprog <= posH).OrderBy(x => x.Reprogorden).ToList();
                foreach (var topReprog in listaReprog)
                {
                    var regTopTmp = listaReprograma.Find(x => x.Topcodi == topReprog.Topcodi2);
                    if (regTopTmp != null)
                    {
                        regTopTmp.EsReprograma = true;
                        regTopTmp.OrdenReprograma = topReprog.Reprogorden ?? 0;
                        regTopTmp.HoraReprograma = topReprog.Tophorareprog;
                        listaTopologia.Add(regTopTmp);

                    }
                }

                listaTopologia.Add(regProg);
            }

            //solo considerar los escenarios que tengan Hora de reprograma válido 
            listaTopologia = listaTopologia.Where(x => x.HoraReprograma > 0).ToList();

            //formatear

            foreach (var reg in listaTopologia)
            {
                var strFecha = reg.Topfecha.ToString(ConstantesAppServicio.FormatoFechaWS);
                if (reg.EsReprograma)
                {
                    reg.Identificador = string.Format("{0}_RDO-{1}", strFecha, (char)(65 + reg.OrdenReprograma));
                    //if (reg.HoraReprograma >= 1) reg.Identificador += string.Format(" ({0})", DateTime.Today.AddMinutes(reg.HoraReprograma * 30).ToString(ConstantesAppServicio.FormatoHora));
                }
                else
                    reg.Identificador = string.Format("{0}_PDO", strFecha);
            }

            return listaTopologia;
        }

        private List<CmGeneracionEmsDTO> ListarEmsFecha(DateTime fecha, int posH)
        {
            List<CmGeneracionEmsDTO> lista = new List<CmGeneracionEmsDTO>();

            for (int h = 1; h <= 48; h++)
            {
                DateTime fechaHoraEms = fecha.Date.AddMinutes(h * 30);
                if (h == 48) fechaHoraEms = fechaHoraEms.AddMinutes(-1);

                lista.AddRange(FactorySic.GetCmGeneracionEmsRepository().GetByCriteria(fechaHoraEms));
            }

            return lista;
        }

        /// <summary>
        /// ListarCmVolumenDetalleXEnvio
        /// </summary>
        /// <param name="codigoEnvio"></param>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public void ListarCmVolumenDetalleXEnvio(int volcalcodi, DateTime fechaConsulta, int periodoH
                                                , out List<CmVolumenDetalleDTO> listaDet, out CmVolumenCalculoDTO objCab)
        {
            objCab = null;
            listaDet = new List<CmVolumenDetalleDTO>();

            //lista envios x filtro
            List<CmVolumenCalculoDTO> listaEnvio = GetByCriteriaCmVolumenCalculos(fechaConsulta.Date, periodoH).OrderByDescending(x => x.Volcalcodi).ToList();

            //pantalla defecto
            if (volcalcodi == 0 && listaEnvio.Any()) volcalcodi = listaEnvio.First().Volcalcodi;

            if (volcalcodi > 0)
            {
                objCab = GetByIdCmVolumenCalculo(volcalcodi);
                listaDet = GetByCriteriaCmVolumenDetalles(volcalcodi);
            }

            if (listaDet.Any())
            {
                //setear valores
                List<int> lModcomcodi = listaDet.Select(x => x.Modcomcodi).ToList();
                List<CmModeloComponenteDTO> listaComp = GetByCriteriaCmModeloComponentes(ConstantesAppServicio.ParametroDefecto, string.Join(",", lModcomcodi));

                //Obtener valores minimos y maximos de los embalses yupana
                int topcodi = GetTopologiaByDate(fechaConsulta.Date, periodoH);//cambiar fecha y hora
                List<CpProprecursoDTO> listaPropVolHm3 = FactorySic.GetCpProprecursoRepository().ListarPropiedadxRecurso2(-1, ConstantesBase.Embalse.ToString(), topcodi, -1);

                foreach (var objDet in listaDet)
                {
                    var objComp = listaComp.Find(x => x.Modcomcodi == objDet.Modcomcodi);

                    GetDescripcionCmVolumenDetalle(objComp?.Modcomtipo, objComp?.Recurnombre, objComp?.Equinomb
                                            , out string ptomedidesc, out string unidadMedida, out string unidadMedidaAbrev, out int orden);
                    objDet.Recurnombre = (objComp?.Recurnombre) ?? "";
                    objDet.Ptomedidesc = ptomedidesc;
                    objDet.UnidadMedida = unidadMedida;
                    objDet.UnidadMedidaAbrev = unidadMedidaAbrev;
                    objDet.Recurcodi = objComp.Recurcodi;
                    objDet.Modcomtipo = objComp?.Modcomtipo;
                    objDet.Orden = orden;

                    //
                    var objPropVmin = listaPropVolHm3.Find(x => x.Recurcodi == objComp.Recurcodi && x.Propcodi == 28); //Hm3
                    var objPropVmax = listaPropVolHm3.Find(x => x.Recurcodi == objComp.Recurcodi && x.Propcodi == 29); //Hm3
                    decimal vmin = 0;
                    if (objPropVmin != null && objPropVmin.Valor != null) decimal.TryParse(objPropVmin.Valor, out vmin);

                    decimal vmax = 0;
                    if (objPropVmax != null && objPropVmax.Valor != null) decimal.TryParse(objPropVmax.Valor, out vmax);

                    objDet.VMin = vmin;
                    objDet.VMax = vmax;
                }
            }

            //ordenar
            listaDet = listaDet.OrderBy(x => x.Recurnombre).ThenBy(x => x.Orden).ToList();
        }

        private void GetDescripcionCmVolumenDetalle(string modcomtipo, string recurnombre, string equinomb
                                                    , out string ptomedidesc, out string unidadMedida, out string unidadMedidaAbrev, out int orden)
        {
            ptomedidesc = "";
            unidadMedida = "";
            unidadMedidaAbrev = "";
            orden = 0;
            switch (modcomtipo)
            {
                case ConstHidrologia.ComponenteTipoCentralAguaArriba:

                    ptomedidesc = "Potencia <br/> activa de <br/> central aportante " + " <br/>" + (equinomb ?? "");
                    unidadMedida = "POTENCIA ACTIVA";
                    unidadMedidaAbrev = "[MW]";
                    orden = 1;
                    break;
                case ConstHidrologia.ComponenteTipoCaudalTurbinadoCentralAguaArriba:

                    ptomedidesc = "Caudal <br/>turbinado de <br/>central <br/>aportante " + " <br/>" + (equinomb ?? "");
                    unidadMedida = "CAUDAL";
                    unidadMedidaAbrev = "[m3/s]";
                    orden = 2;
                    break;
                case ConstHidrologia.ComponenteTipoCaudal:

                    ptomedidesc = "Caudal de <br/>entrada ";
                    unidadMedida = "CAUDAL";
                    unidadMedidaAbrev = "[m3/s]";
                    orden = 3;
                    break;
                case ConstHidrologia.ComponenteTipoCentralTurbinante:

                    ptomedidesc = "Potencia <br/> activa de <br/> central turbinante " + " <br/>" + (equinomb ?? "");
                    unidadMedida = "POTENCIA ACTIVA";
                    unidadMedidaAbrev = "[MW]";
                    orden = 4;
                    break;
                case ConstHidrologia.ComponenteTipoVolumenEmbalse:

                    ptomedidesc = "Volumen del <br/>embalse";
                    unidadMedida = "VOLUMEN";
                    unidadMedidaAbrev = "[hm3]";
                    orden = 5;
                    break;
            }

        }

        /// <summary>
        /// ArmarHandsonVolumenCaudalCmgCP
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="listaMedicion"></param>
        /// <returns></returns>
        public HandsonModel ArmarHandsonVolumenCaudalCmgCP(DateTime fecha, List<CmVolumenDetalleDTO> listaMedicion)
        {
            List<ExpandoObject> lstaDatosFila = ListarFilasHandsonVolumenCaudalCmgCP(fecha, listaMedicion, out List<ExpandoObject> lstPropiedadesCeldas);

            #region Cabecera

            var nestedHeader = new NestedHeaders();

            var headerRow1 = new List<CellNestedHeader>();
            var headerRow2 = new List<CellNestedHeader>();
            var headerRow3 = new List<CellNestedHeader>();
            var headerRow4 = new List<CellNestedHeader>();

            //Primera columna
            CellNestedHeader f1c1 = new CellNestedHeader() { Label = "Embalse", }; headerRow1.Add(f1c1);
            CellNestedHeader f2c1 = new CellNestedHeader() { Label = "Punto de medición", }; headerRow2.Add(f2c1);
            CellNestedHeader f3c1 = new CellNestedHeader() { Label = "Medida", }; headerRow3.Add(f3c1);
            CellNestedHeader f4c1 = new CellNestedHeader() { Label = "Fecha-hora / U.M" }; headerRow4.Add(f4c1);

            #region Cabecera-Columnas

            var lstColumn = new List<object>()
            {
                new { data = "Periodo", className = "htCenter celdaPeriodo", readOnly = true }
            };

            var lstColumnWidth = new List<int> { 80 };
            string recurnombre = listaMedicion.Any() ? listaMedicion.First().Recurnombre : "";
            string claseCss = "clase_par";
            foreach (var obj in listaMedicion)
            {
                string title = string.Format("Vmin: {0}. Vmax: {1}", obj.VMin, obj.VMax);
                if (recurnombre != obj.Recurnombre)
                {
                    claseCss = ("clase_par" == claseCss) ? "clase_impar" : "clase_par";
                    recurnombre = obj.Recurnombre;
                }

                CellNestedHeader f1 = new CellNestedHeader() { Label = obj.Recurnombre, Class = claseCss, Title = title }; headerRow1.Add(f1);
                CellNestedHeader f2 = new CellNestedHeader() { Label = obj.Ptomedidesc }; headerRow2.Add(f2);
                CellNestedHeader f3 = new CellNestedHeader() { Label = obj.UnidadMedida }; headerRow3.Add(f3);
                CellNestedHeader f4 = new CellNestedHeader() { Label = obj.UnidadMedidaAbrev }; headerRow4.Add(f4);

                bool readOnly = obj.Modcomtipo == ConstHidrologia.ComponenteTipoCaudalTurbinadoCentralAguaArriba;
                lstColumn.Add(new { data = $"E{obj.Modcomcodi}.Valor", className = "htRight", numericFormat = new { pattern = "0.000" }, type = "numeric", readOnly });
                lstColumnWidth.Add(80);
            }

            #endregion            

            nestedHeader.ListCellNestedHeaders.Add(headerRow1);
            nestedHeader.ListCellNestedHeaders.Add(headerRow2);
            nestedHeader.ListCellNestedHeaders.Add(headerRow3);
            nestedHeader.ListCellNestedHeaders.Add(headerRow4);

            #endregion

            HandsonModel handson = new HandsonModel();
            handson.NestedHeader = nestedHeader;
            handson.ListaExcelData2 = JsonConvert.SerializeObject(lstaDatosFila);
            handson.ListaColWidth = lstColumnWidth;
            handson.Columnas = lstColumn.ToArray();
            handson.Esquema = JsonConvert.SerializeObject(lstPropiedadesCeldas);

            return handson;
        }

        /// <summary>
        /// ListarFilasHandsonVolumenCaudalCmgCP(
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="listaMedicion"></param>
        /// <param name="lstPropiedadesCeldas"></param>
        /// <returns></returns>
        public List<ExpandoObject> ListarFilasHandsonVolumenCaudalCmgCP(DateTime fecha, List<CmVolumenDetalleDTO> listaMedicion, out List<ExpandoObject> lstPropiedadesCeldas)
        {
            List<ExpandoObject> lstaData = new List<ExpandoObject>();
            List<ExpandoObject> lstaDataCells = new List<ExpandoObject>();

            int fila = -1;
            for (int h = 1; h <= 48; h++)
            {
                DateTime fechaHora = fecha.AddMinutes(h * 30);
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

                    AddProperty(data, $"E{objAgrup.Modcomcodi}", new { Valor = val, Origen = orig, ValDefecto = val, OrigenDefecto = orig });
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

        /// <summary>
        /// Devuelve lista medicion a partir de la informacion de la tabla web
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="anioIni"></param>
        /// <param name="anioFin"></param>
        /// <param name="stringJson"></param>
        /// <returns></returns>
        public List<CmVolumenDetalleDTO> ListarVolumenCaudalCmgCPFromHandson(string stringJson)
        {
            List<CmVolumenDetalleDTO> lstSalida = new List<CmVolumenDetalleDTO>();
            string cadLimpio = stringJson.Replace("\"", "");
            cadLimpio = cadLimpio.Replace("[", String.Empty);
            cadLimpio = cadLimpio.Replace("]", String.Empty);
            string[] fila = cadLimpio.Split(',');

            List<string> lstCampos = fila[0].Split('|').ToList();

            int numEstaciones = lstCampos.Count - 1;

            for (int estacion = 0; estacion < numEstaciones; estacion++)
            {
                int filasuma = 0;

                CmVolumenDetalleDTO medirango = new CmVolumenDetalleDTO();

                var id = ((fila[0].Split('|')[estacion + 1]).Split(':')[0]).Replace("E", "").ToString();
                medirango.Modcomcodi = Convert.ToInt32(id);

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

        /// <summary>
        /// GuardarVolumenCaudalCmgCP
        /// </summary>
        /// <param name="objCab"></param>
        /// <param name="listaMedicion"></param>
        /// <returns></returns>
        public int GuardarVolumenCaudalCmgCP(CmVolumenCalculoDTO objCab, List<CmVolumenDetalleDTO> listaMedicion)
        {
            int volcalcodi = SaveCmVolumenCalculo(objCab);

            foreach (var objDet in listaMedicion)
            {
                objDet.Volcalcodi = volcalcodi;
                SaveCmVolumenDetalle(objDet);
            }

            return volcalcodi;
        }

        /// <summary>
        /// Genera el reporte excel para el envio 
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="cbenvcodi"></param>
        public void GenerarArchivoExcelVolumenEmbalse(string ruta, DateTime fecha, int periodoH, int volcalcodi, out string nombreArchivo)
        {
            ListarCmVolumenDetalleXEnvio(volcalcodi, fecha, periodoH, out List<CmVolumenDetalleDTO> listaDetalle, out CmVolumenCalculoDTO objCab);

            nombreArchivo = string.Format("VolúmenesCaudalesEmbalsesCMgCP_{0}.xlsx", fecha.ToString(ConstantesAppServicio.FormatoFechaYMD2));
            string rutaFile = ruta + nombreArchivo;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {

                GenerarHojaExcelVolumenEmbalse(xlPackage, fecha, listaDetalle, objCab);

                xlPackage.Save();

            }
        }

        /// <summary>
        /// Genera el archivo excel del formulario para un envío
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="modeloWeb"></param>
        private void GenerarHojaExcelVolumenEmbalse(ExcelPackage xlPackage, DateTime fecha, List<CmVolumenDetalleDTO> listaDetalle, CmVolumenCalculoDTO objCab)
        {
            HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());

            ExcelWorksheet ws = null;
            string nameWS = "FORMATO";
            string titulo = "Volúmenes y caudales de Embalses para CMgCP - " + fecha.ToString(ConstantesAppServicio.FormatoFecha);

            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];
            UtilExcel.AddImageXY(ws, 1, 0, img);

            if (listaDetalle.Any())
            {

                //Estilos titulo
                string font = "Calibri";
                int colIniTitulo = 3;
                int rowIniTitulo = 2;
                ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colIniTitulo, font, 16);
                UtilExcel.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colIniTitulo);

                int rowIniTabla = 6;
                int rowIniData = rowIniTabla + 4;
                int rowData = rowIniData;
                int colIniTable = 2;

                //cabecera
                int rowCab1 = rowData; //las otras filas anteriores estaran ocultas
                int rowCab2 = rowCab1 + 1;
                int rowCab3 = rowCab2 + 1;
                int rowCab4 = rowCab3 + 1;

                int colHora = colIniTable;
                int col = colHora;

                ws.Cells[rowCab1, colHora].Value = "Embalse";
                ws.Cells[rowCab2, colHora].Value = "Punto de medición";
                ws.Cells[rowCab3, colHora].Value = "Medida";
                ws.Cells[rowCab4, colHora].Value = "Fecha-hora / U.M";

                col = colHora + 1;
                ws.Column(1).Width = 2;
                ws.Row(rowIniTabla + 0).Height = 0;
                ws.Row(rowIniTabla + 1).Height = 0;
                ws.Row(rowIniTabla + 2).Height = 0;
                ws.Row(rowIniTabla + 3).Height = 0;

                ws.Column(colHora).Width = 20;
                foreach (var objAgrup in listaDetalle)
                {
                    ws.Cells[rowIniTabla + 0, col].Value = objAgrup.Recurcodi;
                    ws.Cells[rowIniTabla + 1, col].Value = objAgrup.Modcomcodi;
                    ws.Cells[rowIniTabla + 2, col].Value = objAgrup.Modcomtipo;
                    ws.Cells[rowIniTabla + 3, col].Value = objAgrup.Equicodi;

                    ws.Cells[rowCab1, col].Value = objAgrup.Recurnombre.Replace("<br/>", "\n");
                    ws.Cells[rowCab2, col].Value = objAgrup.Ptomedidesc.Replace("<br/>", "\n");
                    ws.Cells[rowCab3, col].Value = objAgrup.UnidadMedida.Replace("<br/>", "\n");
                    ws.Cells[rowCab4, col].Value = objAgrup.UnidadMedidaAbrev.Replace("<br/>", "\n");

                    ws.Column(col).Width = 17;

                    col++;
                }
                int colFin = colHora + listaDetalle.Count;
                UtilExcel.SetFormatoCelda(ws, rowCab1, colHora, rowCab1, colFin, "Centro", "Centro", "#000000", "#D9E1F2", font, 11, false, true);
                UtilExcel.SetFormatoCelda(ws, rowCab2, colHora, rowCab2, colFin, "Centro", "Centro", "#FFFFFF", "#2F75B5", font, 11, false, true);
                UtilExcel.SetFormatoCelda(ws, rowCab3, colHora, rowCab3, colFin, "Centro", "Centro", "#000000", "#D9E1F2", font, 11, false, true);
                UtilExcel.SetFormatoCelda(ws, rowCab4, colHora, rowCab4, colFin, "Centro", "Centro", "#000000", "#E2EFDA", font, 11, false, true);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowCab1, colHora, rowCab1, colFin, "#000000", true);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowCab2, colHora, rowCab2, colFin, "#000000", true);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowCab3, colHora, rowCab3, colFin, "#000000", true);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowCab4, colHora, rowCab4, colFin, "#000000", true);

                //data
                rowData += 4;
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
                            case 0:
                            case 2:
                            case 5:
                            case 7:
                            case 8:
                            case 9:
                                sColorFondo = "#22AEE2"; //celeste
                                break;
                            case 3:
                            case 6:
                                sColorFondo = "#4AB516"; //verde
                                break;
                            case 10:
                                sColorFondo = "#7149A7"; //morado
                                break;
                        }

                        //texto
                        string sColorTexto = "#000000"; //negro 1,2,3,4,5,6,7
                        switch (orig ?? 0)
                        {
                            case 0:
                                sColorTexto = "#ff0000"; //rojo
                                break;
                            case 8:
                                sColorTexto = "#FEE28C"; //amarillo
                                break;
                            case 9:
                                sColorTexto = "#0000ff"; //azul
                                break;
                            case 10:
                                sColorTexto = "#FFFFFF"; //blanco
                                break;
                        }
                        if (val < 0) sColorTexto = "#ff0000";

                        //negrita
                        bool celdaNegrita = false;
                        switch (orig ?? 0)
                        {
                            case 0:
                            case 8:
                            case 9:
                            case 10:
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
                    UtilExcel.BorderCeldasLineaGruesa(ws, rowIniData, col, rowIniData + 4 + 48 - 1, col + count - 1, "#000000");
                    col += count;
                }

                ws.View.FreezePanes(rowIniData + 4, colHora + 1);
            }

            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 80;
        }

        public List<CmVolumenDetalleDTO> LeerArchivoExcelVolumenEmbalse(string file)
        {
            List<CmVolumenDetalleDTO> listaDetalle = new List<CmVolumenDetalleDTO>();

            int maxCols = 300;
            int colIniData = 3;
            int rowRecurcodi = 6;
            int rowModcomcodi = rowRecurcodi + 1;
            int rowModcomtipo = rowModcomcodi + 1;
            int rowEquicodi = rowModcomtipo + 1;
            int rowRecurnombre = rowEquicodi + 1;
            int rowPtomedidesc = rowRecurnombre + 1;
            int rowUnidadMedida = rowPtomedidesc + 1;
            int rowUnidadMedidaAbrev = rowUnidadMedida + 1;

            int rowIniData = 14;

            FileInfo fileInfo = new FileInfo(file);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];

                //Obtiene valor para la matriz
                int fila = 0;
                for (int j = colIniData; j < maxCols; j++)
                {
                    var valorCelda = ws.Cells[rowRecurcodi, j].Value;
                    string valor = (valorCelda != null) ? valorCelda.ToString() : string.Empty;
                    Int32.TryParse(valor, out int recurcodi);
                    if (recurcodi > 0)
                    {
                        valorCelda = ws.Cells[rowModcomcodi, j].Value;
                        valor = (valorCelda != null) ? valorCelda.ToString() : string.Empty;
                        Int32.TryParse(valor, out int modcomcodi);

                        valorCelda = ws.Cells[rowModcomtipo, j].Value;
                        string modcomtipo = (valorCelda != null) ? valorCelda.ToString() : string.Empty;

                        valorCelda = ws.Cells[rowEquicodi, j].Value;
                        valor = (valorCelda != null) ? valorCelda.ToString() : string.Empty;
                        Int32.TryParse(valor, out int equicodi);

                        valorCelda = ws.Cells[rowRecurnombre, j].Value;
                        string recurnombre = (valorCelda != null) ? valorCelda.ToString() : string.Empty;

                        valorCelda = ws.Cells[rowPtomedidesc, j].Value;
                        string ptomedidesc = (valorCelda != null) ? valorCelda.ToString() : string.Empty;

                        valorCelda = ws.Cells[rowUnidadMedida, j].Value;
                        string unidadMedida = (valorCelda != null) ? valorCelda.ToString() : string.Empty;

                        valorCelda = ws.Cells[rowUnidadMedidaAbrev, j].Value;
                        string unidadMedidaAbrev = (valorCelda != null) ? valorCelda.ToString() : string.Empty;

                        CmVolumenDetalleDTO objDet = new CmVolumenDetalleDTO()
                        {
                            Recurcodi = recurcodi,
                            Modcomcodi = modcomcodi,
                            Modcomtipo = modcomtipo,
                            Equicodi = equicodi > 0 ? (int?)equicodi : null,
                            Recurnombre = recurnombre.Replace("\n", "<br/>"),
                            Ptomedidesc = ptomedidesc.Replace("\n", "<br/>"),
                            UnidadMedida = unidadMedida.Replace("\n", "<br/>"),
                            UnidadMedidaAbrev = unidadMedidaAbrev.Replace("\n", "<br/>")
                        };
                        listaDetalle.Add(objDet);

                        for (int i = 1; i <= 48; i++)
                        {
                            valorCelda = ws.Cells[rowIniData + i - 1, j].Value;
                            valor = (valorCelda != null) ? valorCelda.ToString() : string.Empty;
                            decimal.TryParse(valor, out decimal valorH);

                            objDet.GetType().GetProperty("H" + (i).ToString()).SetValue(objDet, valorH);
                        }
                    }
                }
            }

            return listaDetalle;
        }

        #endregion

        #region Reporte Proyección Hidrológica - REQ 2023-000403

        /// <summary>
        /// GenerarReporteProyHidro
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="formatcodi"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nombreArchivo"></param>
        public void GenerarReporteProyHidro(string ruta, int formatcodi, DateTime fechaIni, DateTime fechaFin, out string nombreArchivo)
        {
            //lista de Puntos de reporte
            int formatcodiRpt = ConstHidrologia.IdFormatoProyHidro;
            List<MeHojaptomedDTO> lpto = FactorySic.GetMeHojaptomedRepository().ListarHojaPtoByFormatoAndEmpresa(-1, formatcodiRpt.ToString());
            List<int> lemprcodi = lpto.Select(x => x.Emprcodi).Distinct().ToList();
            List<int> lptomedicodi = lpto.Select(x => x.Ptomedicodi).Distinct().ToList();

            //Tipo seleccionado
            MeFormatoDTO objFmt = ListarLecturaRptProyHidro().Find(x => x.Formatcodi == formatcodi);

            //Fechas de reporte
            RptProyHidro objRpt = GenerarEstructuraProyHidro(objFmt, fechaIni, fechaFin, lpto);

            //Lista de todos los envios
            List<MeEnvioDTO> listaEnvio = GetListaMultipleMeEnviosXLS(string.Join(",", lemprcodi), objFmt.Lectcodi.ToString(), objFmt.Formatcodi.ToString()
                                                                , "3", fechaIni, fechaFin).OrderBy(x => x.Emprcodi).ThenBy(x => x.Enviocodi).ToList();

            //Lista de cambios
            List<MeCambioenvioDTO> listaAllCambio = new List<MeCambioenvioDTO>();

            List<List<MeEnvioDTO>> listaEnvioPartido = new List<List<MeEnvioDTO>>();
            int tamanio = 400;
            for (int i = 0; i < listaEnvio.Count; i += tamanio)
                listaEnvioPartido.Add(listaEnvio.GetRange(i, Math.Min(tamanio, listaEnvio.Count - i)));

            foreach (var objLista in listaEnvioPartido)
            {
                listaAllCambio.AddRange(FactorySic.GetMeCambioenvioRepository().ListByEnvio(string.Join(", ", objLista.Select(x => x.Enviocodi))));
            }

            //Lista data MeMedicion1 de caudal
            DateTime fechaConsultaIni = objRpt.ListaFechaX.Min().AddMonths(-1);
            DateTime fechaConsultaFin = objRpt.ListaFechaX.Max().AddMonths(1);
            List<MeMedicion1DTO> listaM1 = FactorySic.GetMeMedicion1Repository().GetByCriteria(fechaConsultaIni, fechaConsultaFin, objFmt.Lectcodi
                                                                            , ConstantesAppServicio.TipoinfocodiM3s, string.Join(",", lptomedicodi));

            //Reporte por filas y columnas
            objRpt = GeneraRptDataProyHidro(objRpt, objFmt, lpto, listaEnvio, listaM1, listaAllCambio);

            //Generar archivo y hojas por cada punto de medición
            nombreArchivo = string.Format("{0}.xlsx", objFmt.Formatnombre);
            string rutaFile = ruta + "/" + nombreArchivo;
            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                foreach (var objPto in lpto)
                {
                    string nameWs = (objPto.Ptomedibarranomb ?? "").Trim().Replace(" ", "_");
                    GenerarArchivoHojaExcelXProyHidro(xlPackage, nameWs, objFmt.Resolucion, objRpt, objPto.Ptomedicodi);
                }

                xlPackage.Save();
            }
        }

        private void GenerarArchivoHojaExcelXProyHidro(ExcelPackage xlPackage, string nameWS, string resolucion, RptProyHidro objRpt, int ptomedicodi)
        {
            string fontHoja = "Calibri";
            ExcelWorksheet ws = null;

            if (nameWS.Length > 30) nameWS = nameWS.Substring(0, 30);

            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            int colAdicional = resolucion == "2" ? 1 : 0;

            ws.Cells[4, 2].Value = "VALOR EJECUTADO";
            UtilExcel.SetFormatoCelda(ws, 4, 2, 4, 3 + colAdicional, "Centro", "Centro", "#FFFFFF", "#C00000", fontHoja, 8, false);
            UtilExcel.CeldasExcelAgrupar(ws, 4, 2, 4, 3 + colAdicional);

            UtilExcel.SetFormatoCelda(ws, 5, 2, 6, 3 + colAdicional, "Centro", "Centro", "#FFFFFF", "#203764", fontHoja, 8, false);
            UtilExcel.CeldasExcelAgrupar(ws, 5, 2, 6, 3 + colAdicional);

            if (resolucion == "4" || resolucion == "5")
                ws.Cells[7, 2].Value = "MESES DE REGISTRO";
            ws.Cells[7, 2].Style.TextRotation = 90;
            UtilExcel.SetFormatoCelda(ws, 7, 2, 7 + objRpt.ListaFechaY.Count() - 1, 2, "Centro", "Centro", "#FFFFFF", "#4472C4", fontHoja, 11, false, true);
            UtilExcel.CeldasExcelAgrupar(ws, 7, 2, 7 + objRpt.ListaFechaY.Count() - 1, 2);

            ws.Cells[5, 4 + colAdicional].Value = objRpt.NombreProyeccion;

            //grilla
            int colFechaX = 3 + colAdicional;
            int colFechaYIni = colFechaX + 1;
            ws.Column(1).Width = 5;
            ws.Column(2).Width = 10;
            if (colAdicional > 0)
                ws.Column(colFechaX - 1).Width = 4;
            ws.Column(colFechaX).Width = 10;

            //fechas vertical
            int colTmp = colFechaYIni;
            for (var c = 0; c < objRpt.ListaFechaX.Count(); c++)
            {
                ws.Column(colTmp).Width = 12;
                colTmp++;
            }

            //datos y fecha horizontal
            int rowData = 7;
            for (var i = 0; i < objRpt.ListaFechaY.Count(); i++)
            {
                //fecha vertical
                DateTime fechaY = objRpt.ListaFechaY[i];
                if (resolucion == "4" || resolucion == "5")
                {
                    ws.Cells[rowData, colFechaX].Value = fechaY;
                    ws.Cells[rowData, colFechaX].Style.Numberformat.Format = "mmm-yy";
                }
                if (resolucion == "2")
                {
                    Tuple<int, int> tupla = EPDate.f_numerosemana_y_anho(fechaY);
                    ws.Cells[rowData, colFechaX].Value = string.Format("SEM_{0} {1}", tupla.Item1, tupla.Item2);

                    ws.Cells[rowData, colFechaX - 1].Value = string.Format("{0}", EPDate.f_NombreMesCorto(fechaY.Month));
                }
                if (resolucion == "1")
                {
                    ws.Cells[rowData, colFechaX].Value = fechaY;
                    ws.Cells[rowData, colFechaX].Style.Numberformat.Format = "dd/mm/yyyy";
                }

                UtilExcel.SetFormatoCelda(ws, rowData, colFechaX - colAdicional, rowData, colFechaX, "Centro", "Derecha", "#FFFFFF", "#4472C4", fontHoja, 8, true);

                var objFila = objRpt.ListaFila.Find(x => x.Fecha == fechaY);

                int colData = 4 + colAdicional;
                for (var c = 0; c < objRpt.ListaFechaX.Count(); c++)
                {
                    DateTime fechaX = objRpt.ListaFechaX[c];

                    //cabecera fecha horizontal
                    if (i == 0)
                    {
                        if (resolucion == "4")
                        {
                            ws.Cells[rowData - 1, colData].Value = fechaX;
                            ws.Cells[rowData - 1, colData].Style.Numberformat.Format = "mmm-yy";
                        }

                        if (resolucion == "5")
                        {
                            Tuple<int, int> tupla = EPDate.f_numerosemana_y_anho(fechaX);
                            ws.Cells[rowData - 1, colData].Value = string.Format("SEM {0}_{1}", tupla.Item1, tupla.Item2);
                        }

                        if (resolucion == "2" || resolucion == "1")
                        {
                            ws.Cells[rowData - 1, colData].Value = fechaX;
                            ws.Cells[rowData - 1, colData].Style.Numberformat.Format = "dd/mm/yyyy";
                        }

                        UtilExcel.SetFormatoCelda(ws, rowData - 3, colData, rowData - 3, colData, "Centro", "Izquierda", "#FFFFFF", "#FFC000", fontHoja, 8, true);
                        UtilExcel.SetFormatoCelda(ws, rowData - 2, colData, rowData - 2, colData, "Centro", "Izquierda", "#FFFFFF", "#305496", fontHoja, 8, true);
                        UtilExcel.SetFormatoCelda(ws, rowData - 1, colData, rowData - 1, colData, "Centro", "Centro", "#FFFFFF", "#305496", fontHoja, 8, true);
                    }

                    //setear dato
                    decimal? dato = null;
                    bool existeFechaCelda = false;
                    if (objFila != null)
                    {
                        var objDato = objFila.ListaDato.Find(x => x.Fecha == fechaX && x.Ptomedicodi == ptomedicodi);
                        if (objDato != null)
                        {
                            dato = objDato.Valor;
                            existeFechaCelda = true;

                            ws.Cells[rowData, colData].Value = dato;

                            //formato de celda
                            string colorFondo = existeFechaCelda ? "#F4B084" : "#FFFFFF";
                            UtilExcel.SetFormatoCelda(ws, rowData, colData, rowData, colData, "Centro", "Centro", "#000000", colorFondo, fontHoja, 8, false);
                            UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colData, rowData, colData, "#FFFFFF");

                        }
                    }

                    //
                    colData++;
                }

                rowData++;
            }

            UtilExcel.CeldasExcelFormatoNumero(ws, 7, 4 + colAdicional, 7 + objRpt.ListaFechaY.Count() - 1, 2 + 2 + objRpt.ListaFechaX.Count() - 1 + colAdicional, 3);
            UtilExcel.BorderCeldasLineaDelgada(ws, 7, 2, 7 + objRpt.ListaFechaY.Count() - 1, 2 + 2 + objRpt.ListaFechaX.Count() - 1 + colAdicional, "#4472C4");

            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            ws.View.FreezePanes(7, 4 + colAdicional);

            ws.Calculate();
        }

        /// <summary>
        /// Lista de tipos de reportes
        /// </summary>
        /// <returns></returns>
        public List<MeFormatoDTO> ListarLecturaRptProyHidro()
        {
            List<MeFormatoDTO> lista = new List<MeFormatoDTO>();

            //lectura PROGRAMA SEMANAL MP
            var obj1 = GetByIdMeFormato(35); //formato NATURALES - PROGRAMADO MENSUAL PARA MP
            obj1.Formatnombre = "PROGRAMADO MENSUAL DE MP";
            obj1.Resolucion = "4";
            lista.Add(obj1);

            //lectura PROGRAMA SEMANAL MP
            var obj2 = GetByIdMeFormato(36); //formato NATURALES - PROGRAMADO SEMANAL PARA MP
            obj2.Formatnombre = "PROGRAMADO SEMANAL DE MP";
            obj2.Resolucion = "5";
            lista.Add(obj2);

            //lectura PROGRAMA SEMANAL MP
            var obj3 = GetByIdMeFormato(30); //formato NATURALES - PROGRAMADO SEMANAL
            obj3.Formatnombre = "PROGRAMADO SEMANAL DE CP";
            obj3.Resolucion = "2";
            lista.Add(obj3);

            //lectura PROGRAMA SEMANAL MP
            var obj4 = GetByIdMeFormato(31); //NATURALES - PROGRAMADO DIARIO
            obj4.Formatnombre = "PROGRAMADO DIARIO DE CP";
            obj4.Resolucion = "1";
            lista.Add(obj4);

            return lista;
        }

        private RptProyHidro GenerarEstructuraProyHidro(MeFormatoDTO objFmt, DateTime fechaIni, DateTime fechaFin, List<MeHojaptomedDTO> lpto)
        {
            List<DateTime> listaFechaXTmp = new List<DateTime>();
            List<DateTime> listaFechaYTmp = new List<DateTime>();

            RptProyHidro objRpt = new RptProyHidro();
            objRpt.ListaFila = new List<RptProyHidroFila>();

            //estructura de fechas y datos vacios
            switch (objFmt.Resolucion)
            {
                case "4": //mensual
                    objRpt.NombreProyeccion = "PROYECCIONES MENSUALES";
                    for (var fecha = fechaIni; fecha <= fechaFin; fecha = fecha.AddMonths(1))
                    {
                        RptProyHidroFila objFila = new RptProyHidroFila();
                        objFila.Fecha = fecha;
                        objFila.ListaDato = new List<RptProyHidroDato>();

                        //
                        for (var i = 0; i < 12; i++)
                        {
                            foreach (var objPto in lpto)
                            {
                                RptProyHidroDato objDato = new RptProyHidroDato();
                                objDato.Fecha = fecha.AddMonths(i);
                                objDato.Ptomedicodi = objPto.Ptomedicodi;
                                objFila.ListaDato.Add(objDato);

                                listaFechaXTmp.Add(objDato.Fecha);
                            }
                        }

                        objRpt.ListaFila.Add(objFila);
                        listaFechaYTmp.Add(objFila.Fecha);
                    }
                    break;

                case "5": //mensual x semanal
                    objRpt.NombreProyeccion = "PROYECCIONES SEMANALES";
                    for (var fecha = fechaIni; fecha <= fechaFin; fecha = fecha.AddMonths(1))
                    {
                        RptProyHidroFila objFila = new RptProyHidroFila();
                        objFila.Fecha = fecha;
                        objFila.ListaDato = new List<RptProyHidroDato>();

                        //
                        DateTime fechaIniSem = EPDate.f_fechainiciosemana(fecha.AddDays(3));
                        for (var i = 0; i < 56; i++)
                        {
                            foreach (var objPto in lpto)
                            {
                                RptProyHidroDato objDato = new RptProyHidroDato();
                                objDato.Fecha = fechaIniSem.AddDays(i * 7);
                                objDato.Ptomedicodi = objPto.Ptomedicodi;
                                objFila.ListaDato.Add(objDato);

                                listaFechaXTmp.Add(objDato.Fecha);
                            }
                        }

                        objRpt.ListaFila.Add(objFila);
                        listaFechaYTmp.Add(objFila.Fecha);
                    }
                    break;

                case "2": //semanal
                    objRpt.NombreProyeccion = "PROYECCIONES DIARIAS";
                    for (var fecha = fechaIni; fecha <= fechaFin; fecha = fecha.AddDays(7))
                    {
                        RptProyHidroFila objFila = new RptProyHidroFila();
                        objFila.Fecha = fecha;
                        objFila.ListaDato = new List<RptProyHidroDato>();

                        //
                        for (var i = 0; i <= 9; i++)
                        {
                            foreach (var objPto in lpto)
                            {
                                RptProyHidroDato objDato = new RptProyHidroDato();
                                objDato.Fecha = fecha.AddDays(i);
                                objDato.Ptomedicodi = objPto.Ptomedicodi;
                                objFila.ListaDato.Add(objDato);

                                listaFechaXTmp.Add(objDato.Fecha);
                            }
                        }

                        objRpt.ListaFila.Add(objFila);

                        listaFechaYTmp.Add(objFila.Fecha);
                    }

                    break;

                case "1": //diario
                    objRpt.NombreProyeccion = "PROYECCIONES DIARIAS";
                    for (var fecha = fechaIni; fecha <= fechaFin; fecha = fecha.AddDays(1))
                    {
                        RptProyHidroFila objFila = new RptProyHidroFila();
                        objFila.Fecha = fecha;
                        objFila.ListaDato = new List<RptProyHidroDato>();

                        //
                        for (var i = 1; i <= 3; i++)
                        {
                            foreach (var objPto in lpto)
                            {
                                RptProyHidroDato objDato = new RptProyHidroDato();
                                objDato.Fecha = fecha.AddDays(i);
                                objDato.Ptomedicodi = objPto.Ptomedicodi;
                                objFila.ListaDato.Add(objDato);

                                listaFechaXTmp.Add(objDato.Fecha);
                            }
                        }

                        objRpt.ListaFila.Add(objFila);

                        listaFechaYTmp.Add(objFila.Fecha);
                    }
                    listaFechaXTmp.Add(fechaIni);

                    break;
            }
            //obtener fechas 
            objRpt.ListaFechaX = listaFechaXTmp.Distinct().OrderBy(x => x).ToList();
            objRpt.ListaFechaY = listaFechaYTmp.Distinct().OrderBy(x => x).ToList();

            return objRpt;
        }

        private RptProyHidro GeneraRptDataProyHidro(RptProyHidro objRpt, MeFormatoDTO objFmt, List<MeHojaptomedDTO> lpto, List<MeEnvioDTO> listaEnvio
                                            , List<MeMedicion1DTO> listaAllM1, List<MeCambioenvioDTO> listaAllCambio)
        {
            //empresas
            List<int> lemprcodi = lpto.Select(x => x.Emprcodi).Distinct().ToList();

            //llenar datos
            foreach (var objFila in objRpt.ListaFila)
            {
                //data por fecha
                List<MeMedicion1DTO> listaM1XFecha = ListarDataM1RptProyHidr(objFila.Fecha, objFmt, lemprcodi, listaEnvio, listaAllM1, listaAllCambio);

                //setear dato
                foreach (var objDato in objFila.ListaDato)
                {
                    objDato.Valor = listaM1XFecha.Find(x => x.Ptomedicodi == objDato.Ptomedicodi && x.Medifecha == objDato.Fecha)?.H1;
                }
            }

            return objRpt;
        }

        private List<MeMedicion1DTO> ListarDataM1RptProyHidr(DateTime fechaProceso, MeFormatoDTO objFmt, List<int> lemprcodi
                                                , List<MeEnvioDTO> listaEnvio, List<MeMedicion1DTO> listaAllM1, List<MeCambioenvioDTO> listaAllCambio)
        {
            //formato
            var objFmtClone = (MeFormatoDTO)objFmt.Clone();
            objFmtClone.FechaProceso = fechaProceso;
            FormatoMedicionAppServicio.GetSizeFormato(objFmtClone);

            //data por mes
            List<MeMedicion1DTO> listaM1 = new List<MeMedicion1DTO>();

            //Iterar por empresa
            foreach (var emprcodi in lemprcodi)
            {
                //obtener ultimo envio de empresa
                int idEnvioLast = 0;
                var listaEnvios = listaEnvio.Where(x => x.Emprcodi == emprcodi && x.Formatcodi == objFmtClone.Formatcodi && x.Enviofechaperiodo == objFmtClone.FechaProceso)
                                                    .OrderBy(x => x.Enviocodi).ToList();
                if (listaEnvios.Any()) idEnvioLast = listaEnvios.Last().Enviocodi;

                //construir data de ultimo envio
                if (idEnvioLast > 0)
                {
                    //
                    var lista1XEmp = listaAllM1.Where(x => x.Lectcodi == objFmtClone.Lectcodi && x.Emprcodi == emprcodi && x.Medifecha >= objFmtClone.FechaInicio && x.Medifecha <= objFmtClone.FechaFin).ToList();
                    var listaCambioXEmp = listaAllCambio.Where(x => x.Enviocodi == idEnvioLast && x.Cambenvfecha >= objFmtClone.FechaInicio && x.Cambenvfecha <= objFmtClone.FechaFin).ToList();

                    //lista clonada
                    List<MeMedicion1DTO> listaTmp = new List<MeMedicion1DTO>();
                    foreach (var objM1 in lista1XEmp)
                    {
                        listaTmp.Add(new MeMedicion1DTO()
                        {
                            Medifecha = objM1.Medifecha,
                            Ptomedicodi = objM1.Ptomedicodi,
                            H1 = objM1.H1,
                            Tipoinfocodi = objM1.Tipoinfocodi,
                        });
                    }

                    //
                    if (listaCambioXEmp.Count > 0)
                    {
                        foreach (var regCambio in listaCambioXEmp)
                        {
                            var find = listaTmp.Find(x => x.Ptomedicodi == regCambio.Ptomedicodi && x.Tipoinfocodi == regCambio.Tipoinfocodi &&
                                x.Medifecha == regCambio.Cambenvfecha);
                            if (find != null)
                            {
                                decimal dato;
                                decimal? numero = null;
                                if (decimal.TryParse(regCambio.Cambenvdatos, out dato))
                                    numero = dato;
                                find.H1 = numero;
                            }
                        }
                    }

                    listaM1.AddRange(listaTmp);
                }
            }

            return listaM1;
        }

        #endregion

    }

    /// <summary>
    /// REQ 2023-000403 - Estructura
    /// </summary>
    public class RptProyHidro 
    {
        public string NombreProyeccion { get; set; }
        public List<DateTime> ListaFechaY { get; set; }
        public List<DateTime> ListaFechaX { get; set; }

        public List<RptProyHidroFila> ListaFila { get; set; }
    }

    /// <summary>
    /// REQ 2023-000403 - Filas
    /// </summary>
    public class RptProyHidroFila
    {
        public DateTime Fecha { get; set; }
        public List<RptProyHidroDato> ListaDato { get; set; }
    }

    /// <summary>
    /// REQ 2023-000403 - Valores
    /// </summary>
    public class RptProyHidroDato
    {
        public DateTime Fecha { get; set; }
        public int Ptomedicodi { get; set; }
        public decimal? Valor { get; set; }
    }
}

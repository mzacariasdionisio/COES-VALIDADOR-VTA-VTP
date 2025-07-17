using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Areas.Medidores.Helper;
using COES.MVC.Extranet.Areas.Medidores.Models;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.Medidores;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Medidores.Utilities
{
    public class DatosExcel
    {
        private int _totalColumnas;
        private int _empresa;
        private TipoErrorHelper _listaTipoObser;
        public TipoErrorHelper ListaTipoObser
        {
            get { return _listaTipoObser; }
            set { _listaTipoObser = value; }
        }
        private IList<MePtomedicionDTO> _listaPtoMedicion;
        public IList<MePtomedicionDTO> ListaPtoMedicion
        {
            get { return _listaPtoMedicion; }
            set { _listaPtoMedicion = value; }
        }
        private List<MeMedicion96DTO> _listaMedidores;
        public List<MeMedicion96DTO> ListaMedidores
        {
            get { return _listaMedidores; }
            set { _listaMedidores = value; }
        }
        private IList<CeldaInfo> _listaErrores;
        public IList<CeldaInfo> ListaErrores
        {
            get { return _listaErrores; }
            set { _listaErrores = value; }
        }
        private List<DateTime> _listaFechas;
        private int _iniCOL = 1;

        private DateTime _fechaCarga;

        public DatosExcel(string Archivo, short empresa, short codigoEar, DateTime fechaCarga, List<MeHojaptomedDTO> lista)
        {
            try
            {
                _listaFechas = new List<DateTime>();
                _listaTipoObser = new TipoErrorHelper();
                _listaMedidores = new List<MeMedicion96DTO>();
                _listaErrores = new List<CeldaInfo>();
                _empresa = empresa;
                _fechaCarga = fechaCarga;
                int retorno = 0;
                retorno = this.GetListaPtoMedicion(Archivo, empresa, lista);
                if (retorno != 1) /// Archivo no corresponde al mes en publicacioZn
                    return;
                retorno = GetListaFechasIntercambio(Archivo, fechaCarga);
                if (retorno != 1) /// Archivo no corresponde al mes en publicacion
                    return;
                GetMediciones(Archivo, empresa, lista, fechaCarga);
            }
            catch (Exception ex)
            {
                _listaTipoObser.IncrementarTipoError(ConstantesMedidores.ERR_CRITICO);
                _listaTipoObser.AgregarMensaje(ex.Message, ConstantesMedidores.ERR_CRITICO);
                return;
            }
        }


        private CeldaInfo ValidacionCeldas(IList<DateTime?> listaFechas, String valorCelda, int col, int fil)
        {
            int tipoGenerador = _listaPtoMedicion[col].Famcodi;
            CeldaInfo errorCelda = new CeldaInfo();
            errorCelda.Celda = Util.ConvertirColLetra(col + _iniCOL + 1) + (fil + ConstantesMedidores.INIFIL).ToString();
            errorCelda.Valor = valorCelda;
            errorCelda.PtoMedicion = _listaPtoMedicion[col].Ptomedicodi;
            errorCelda.Central = _listaPtoMedicion[col].Centralnomb;
            errorCelda.Grupo = _listaPtoMedicion[col].Gruponomb;
            errorCelda.EsNumero = true;
            errorCelda.TipoInfo = (int)_listaPtoMedicion[col].Tipoinfocodi;
            try
            {
                errorCelda.Fecha = (DateTime)listaFechas[fil];
            }
            catch (Exception ex)
            {
                var text = ex.Message;
            }
            errorCelda.TipoObservacion = ConstantesMedidores.ERR_NOERROR;

            //// Verificar si es Solar
            if (tipoGenerador == ConstantesMedidores.PTOSOLARGRUPO || tipoGenerador == ConstantesMedidores.PTOSOLARCENTRAL)
            {
                /// Verificar valores en los rangos que estan fuera del intervalo solar
                int nMinuto = errorCelda.Fecha.Hour * 60 + errorCelda.Fecha.Minute;
                if (nMinuto < ConstantesMedidores.SOLAR_RANGO_MIN || nMinuto > ConstantesMedidores.SOLAR_RANGO_MAX)
                {
                    ///verifica si el valor es cero
                    if (Util.EsNumero(valorCelda))
                    {
                        errorCelda.Valor = (double.Parse(valorCelda)).ToString("0.000");
                        /// Verificar si no es cero
                        if (Math.Abs(double.Parse(valorCelda)) > 0)
                        {
                            errorCelda.TipoObservacion = ConstantesMedidores.ERR_SOLAR;
                            _listaTipoObser.IncrementarTipoError(ConstantesMedidores.ERR_SOLAR);
                        }
                    }
                    else
                    {
                        errorCelda.TipoObservacion = ConstantesMedidores.ERR_SOLAR;
                        errorCelda.EsNumero = false;
                        _listaTipoObser.IncrementarTipoError(ConstantesMedidores.ERR_SOLAR);
                    }
                    return errorCelda;
                }
            }

            if (valorCelda.Equals(""))
            {
                errorCelda.TipoObservacion = ConstantesMedidores.ERR_BLANCOS;
                errorCelda.Valor = "Blanco";
                errorCelda.EsNumero = false;
                this._listaTipoObser.IncrementarTipoError(ConstantesMedidores.ERR_BLANCOS);
                if (_listaTipoObser.GetTotalObservaciones(ConstantesMedidores.ERR_BLANCOS) > ConstantesMedidores.MAXBLANCOS)
                {
                    this._listaTipoObser.IncrementarTipoError(ConstantesMedidores.ERR_MAXBLANCOS);
                }
            }
            else
            {
                ///verificar si no es numero
                if (!Util.EsNumero(valorCelda))
                {
                    errorCelda.TipoObservacion = ConstantesMedidores.ERR_NONUMERO;
                    errorCelda.Valor = "no número";
                    errorCelda.EsNumero = false;
                    _listaTipoObser.IncrementarTipoError(ConstantesMedidores.ERR_NONUMERO);
                }
                else
                {
                    errorCelda.Valor = (double.Parse(valorCelda)).ToString("0.000");
                    /// Verificar negativos
                    if (double.Parse(valorCelda) < 0 && (errorCelda.TipoInfo == 1))
                    {
                        errorCelda.TipoObservacion = ConstantesMedidores.ERR_NEGATIVO;
                        this._listaTipoObser.IncrementarTipoError(ConstantesMedidores.ERR_NEGATIVO);
                    }
                    else
                    {
                        if (_listaPtoMedicion[col].Tipoinfocodi == 3)
                        {
                            if (double.Parse(valorCelda) > this._listaPtoMedicion[col].LimiteUp)
                            {
                                errorCelda.TipoObservacion = ConstantesMedidores.ERR_OUTPACTIVA;
                                this._listaTipoObser.IncrementarTipoError(ConstantesMedidores.ERR_OUTPACTIVA);
                            }

                        }
                        else
                        {
                            errorCelda.TipoObservacion = ConstantesMedidores.ERR_NOERROR;
                        }
                    }

                }
            }
            return errorCelda;
        }

        private CeldaInfo CrearCeldaInfo(String valorCelda, int col, int fil, MeHojaptomedDTO lista)
        {
            CeldaInfo errorCelda = new CeldaInfo();
            errorCelda.Celda = Util.ConvertirColLetra(col) + fil.ToString();
            errorCelda.Valor = valorCelda;
            errorCelda.PtoMedicion = lista.Ptomedicodi;
            errorCelda.Central = "Central";
            errorCelda.Grupo = "Grupo";
            errorCelda.EsNumero = true;
            errorCelda.TipoInfo = lista.Tipoinfocodi;
            errorCelda.Tipoinfoabrev = lista.Tipoinfoabrev;
            return errorCelda;
        }

        private int GetListaPtoMedicion(string file, int empresa, List<MeHojaptomedDTO> lista)
        {
            int retorno = 1;
            FileInfo fileInfo = new FileInfo(file);
            int row = 1;
            int column = 2;
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];

                while (true)
                {
                    string punto = (ws.Cells[row, column].Value != null) ? ws.Cells[row, column].Value.ToString() : string.Empty;
                    if (!string.IsNullOrEmpty(punto))
                    {
                        int ordenHoja = 0;
                        if (int.TryParse(punto, out ordenHoja))
                        {
                            if (ordenHoja != lista[column - 2].Hojaptoorden)
                            {
                                _listaTipoObser.IncrementarTipoError(ConstantesMedidores.ERR_PTOMEDICION);
                                _listaTipoObser.AgregarMensaje("Error en los puntos de medicion", ConstantesMedidores.ERR_PTOMEDICION);
                                retorno = -1;
                                break;
                            }
                        }
                        else
                        {
                            _listaTipoObser.IncrementarTipoError(ConstantesMedidores.ERR_PTOMEDICION);
                            _listaTipoObser.AgregarMensaje("Error en los puntos de medicion", ConstantesMedidores.ERR_PTOMEDICION);
                            retorno = -1;
                            break;
                        }
                        column++;
                    }
                    else
                    {
                        break;
                    }
                }

            }
            if ((column - 2) != lista.Count)
            {
                _listaTipoObser.IncrementarTipoError(ConstantesMedidores.ERR_PTOMEDICION);
                _listaTipoObser.AgregarMensaje("Faltan Columnas", ConstantesMedidores.ERR_PTOMEDICION);
            }
            return retorno;
        }

        private int GetListaFechasIntercambio(string file, DateTime fechaProceso)
        {
            int retorno = 1;
            FileInfo fileInfo = new FileInfo(file);
            int row = 22;
            int column = 1;
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                for (var i = 0; i < 96; i++)
                {
                    string fechaFormato = (ws.Cells[row + i, column].Value != null) ? ws.Cells[row + i, column].Value.ToString() : string.Empty;
                    DateTime fechaComparacion = DateTime.Now;
                    if (DateTime.TryParseExact(fechaFormato, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out fechaComparacion))
                    {
                        if ((fechaComparacion.Subtract(fechaProceso.AddMinutes((i + 1) * 15))).TotalMinutes != 0)
                        {
                            this._listaTipoObser.IncrementarTipoError(ConstantesMedidores.ERR_FECHA);
                            return -1;
                        }
                    }
                    else
                    {
                        this._listaTipoObser.IncrementarTipoError(ConstantesMedidores.ERR_FECHA);
                        return -1;
                    }
                    _listaFechas.Add(fechaComparacion);
                }
            }
            return retorno;
        }

        private int GetMediciones(string file, int empresa, List<MeHojaptomedDTO> lista, DateTime fechaProceso)
        {
            int totPtos = lista.Count;
            int retorno = 1;
            bool colRepetida = false;
            CeldaInfo errorCelda = new CeldaInfo();
            int row = 22;
            int column = 2;
            FileInfo fileInfo = new FileInfo(file);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                for (int i = 0; i < totPtos; i++)
                {
                    colRepetida = true;
                    MeMedicion96DTO entity = _listaMedidores.Find(x => x.Ptomedicodi == lista[i].Ptomedicodi && x.Tipoinfocodi == lista[i].Tipoinfocodi);
                    if (entity == null)
                    {
                        entity = new MeMedicion96DTO();
                        colRepetida = false;
                    }
                    decimal total = 0;
                    entity.Ptomedicodi = lista[i].Ptomedicodi;
                    entity.Medifecha = fechaProceso;
                    entity.Tipoinfocodi = lista[i].Tipoinfocodi;
                    entity.Lectcodi = 1;
                    for (int j = 0; j < 96; j++)
                    {
                        decimal dato = 0;
                        decimal? datoAntiguo = (decimal?)entity.GetType().GetProperty(Constantes.CaracterH + (j + 1).ToString()).GetValue(entity, null);
                        datoAntiguo = (datoAntiguo == null) ? 0 : datoAntiguo;
                        string valor = (ws.Cells[row + j, column + i].Value != null) ? ws.Cells[row + j, column + i].Value.ToString() : string.Empty;
                        if (!string.IsNullOrEmpty(valor))
                        {
                            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out dato))
                            {
                                if (dato >= 0)
                                {
                                    if ((decimal)datoAntiguo > 0 && dato > 0)
                                    {
                                        errorCelda.TipoObservacion = ConstantesMedidores.ERR_IMPORT_EXPORT;
                                        _listaTipoObser.IncrementarTipoError(ConstantesMedidores.ERR_IMPORT_EXPORT);
                                        _listaErrores.Add(errorCelda);
                                    }
                                    dato = dato * lista[i].Hojaptosigno + (decimal)datoAntiguo;
                                    entity.GetType().GetProperty(Constantes.CaracterH + (j + 1).ToString()).SetValue(entity, dato);
                                    total = total + dato;
                                    if (dato > lista[i].Hojaptolimsup)
                                    {
                                        errorCelda = CrearCeldaInfo(valor, column + i, row + j, lista[i]);
                                        errorCelda.TipoObservacion = ConstantesMedidores.ERR_OUTPACTIVA;
                                        _listaTipoObser.IncrementarTipoError(ConstantesMedidores.ERR_OUTPACTIVA);
                                        _listaErrores.Add(errorCelda);
                                    }
                                }
                                else
                                {
                                    errorCelda = CrearCeldaInfo(valor, column + i, row + j, lista[i]);
                                    errorCelda.TipoObservacion = ConstantesMedidores.ERR_NEGATIVO;
                                    errorCelda.Fecha = _listaFechas[j];
                                    _listaTipoObser.IncrementarTipoError(ConstantesMedidores.ERR_NEGATIVO);
                                    _listaErrores.Add(errorCelda);
                                }
                            }
                            else
                            {
                                errorCelda = CrearCeldaInfo(valor, column + i, row + j, lista[i]);
                                errorCelda.TipoObservacion = ConstantesMedidores.ERR_NONUMERO;
                                errorCelda.Fecha = _listaFechas[j];
                                _listaTipoObser.IncrementarTipoError(ConstantesMedidores.ERR_NONUMERO);
                                _listaErrores.Add(errorCelda);
                            }
                        }
                        else
                        {
                            errorCelda = CrearCeldaInfo(valor, column + i, row + j, lista[i]);
                            errorCelda.TipoObservacion = ConstantesMedidores.ERR_BLANCOS;
                            errorCelda.Fecha = _listaFechas[j];
                            _listaTipoObser.IncrementarTipoError(ConstantesMedidores.ERR_BLANCOS);
                            _listaErrores.Add(errorCelda);
                            entity.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(entity, dato);
                        }

                    }
                    entity.Meditotal = total;
                    if (!colRepetida)
                        ListaMedidores.Add(entity);
                }
            }
            return retorno;
        }

    }
}
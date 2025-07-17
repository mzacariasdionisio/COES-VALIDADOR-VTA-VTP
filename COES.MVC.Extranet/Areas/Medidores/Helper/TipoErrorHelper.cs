using COES.MVC.Extranet.Areas.Medidores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Medidores.Helper
{
    public class TipoErrorHelper
    {
        private List<TipoError> _listaTipoError;
        public List<TipoError> ListaTipoError
        {
            get { return _listaTipoError; }
            set { _listaTipoError = value; }
        }

        public TipoErrorHelper()
        {
            _listaTipoError = new List<TipoError>();
            var tipoError = new TipoError()
            {
                TipoObservacion = ConstantesMedidores.ERR_BLANCOS,
                TotalObservacion = 0,
                Mensaje = "Registros en Blanco"
            };
            _listaTipoError.Add(tipoError);
            tipoError = new TipoError()
            {
                TipoObservacion = ConstantesMedidores.ERR_NEGATIVO,
                TotalObservacion = 0,
                Mensaje = "Registros Negativos de Potencia Activa"
            };
            _listaTipoError.Add(tipoError);
            tipoError = new TipoError()
            {
                TipoObservacion = ConstantesMedidores.ERR_NONUMERO,
                TotalObservacion = 0,
                Mensaje = "Registros No Identificados"
            };
            _listaTipoError.Add(tipoError);
            tipoError = new TipoError()
            {
                TipoObservacion = ConstantesMedidores.ERR_OUTPACTIVA,
                TotalObservacion = 0,
                Mensaje = "Registro excede valor de potencia instalada."
            };
            _listaTipoError.Add(tipoError);
            tipoError = new TipoError()
            {
                TipoObservacion = ConstantesMedidores.ERR_SOLAR,
                TotalObservacion = 0,
                Mensaje = "Periodos de generación de centrales solares inconsistentes"
            };
            _listaTipoError.Add(tipoError);

            tipoError = new TipoError()
            {
                TipoObservacion = ConstantesMedidores.ERR_TOTALREGISTRO,
                TotalObservacion = 0,
                Mensaje = "Registros incompletos"
            };
            _listaTipoError.Add(tipoError);

            tipoError = new TipoError()
            {
                TipoObservacion = ConstantesMedidores.ERR_PTOMEDICION,
                TotalObservacion = 0,
                Mensaje = "Puntos de medición no pertenecen a la empresa"
            };
            _listaTipoError.Add(tipoError);

            tipoError = new TipoError()
            {
                TipoObservacion = ConstantesMedidores.ERR_PIN,
                TotalObservacion = 0,
                Mensaje = "Error en la versión del formato, descargar nuevamente"
            };
            _listaTipoError.Add(tipoError);

            tipoError = new TipoError()
            {
                TipoObservacion = ConstantesMedidores.ERR_FECHA,
                TotalObservacion = 0,
                Mensaje = "Los registros no pertenecen a la fecha de envío"
            };
            _listaTipoError.Add(tipoError);

            tipoError = new TipoError()
            {
                TipoObservacion = ConstantesMedidores.ERR_SECFECHA,
                TotalObservacion = 0,
                Mensaje = "Error en el valor de la celda correspondiente a la columna fecha"
            };
            _listaTipoError.Add(tipoError);
            tipoError = new TipoError()
            {
                TipoObservacion = ConstantesMedidores.ERR_FPLAZO,
                TotalObservacion = 0,
                Mensaje = "Esta en fuera de plazo, comuniquese con el Administrador"
            };
            _listaTipoError.Add(tipoError);
            tipoError = new TipoError()
            {
                TipoObservacion = ConstantesMedidores.ERR_MAXBLANCOS,
                TotalObservacion = 0,
                Mensaje = "La cantidad de celdas en blancos excede el máximo numero permitido(" + ConstantesMedidores.MAXBLANCOS + ")"
            };
            _listaTipoError.Add(tipoError);
            tipoError = new TipoError()
            {
                TipoObservacion = ConstantesMedidores.ERR_CRITICO,
                TotalObservacion = 0,
                Mensaje = "Error Crítico:"
            };
            _listaTipoError.Add(tipoError);
            tipoError = new TipoError()
            {
                TipoObservacion = ConstantesMedidores.ERR_IMPORT_EXPORT,
                TotalObservacion = 0,
                Mensaje = "Existen datos de Importación y Exportación en un mismo periodo."
            };
            _listaTipoError.Add(tipoError);


        }

        /// <summary>
        /// Agrega un mensaje adicional al tipo de error
        /// </summary>
        /// <param name="pMensaje"></param>
        /// <param name="pTipoError"></param>
        public void AgregarMensaje(string pMensaje, string pTipoError)
        {
            var registro = _listaTipoError.Find(x => x.TipoObservacion == pTipoError);
            if (registro != null)
                registro.Mensaje += pMensaje;
        }
        /// <summary>
        /// Obtiene total de observaciones de un determinado tipo de error
        /// </summary>
        /// <param name="tipoError"></param>
        /// <returns></returns>
        public int GetTotalObservaciones(string tipoError)
        {
            var registro = _listaTipoError.Find(x => x.TipoObservacion == tipoError);
            if (registro != null)
                return registro.TotalObservacion;
            else
                return 0;
        }
        /// <summary>
        /// Incrementa error al tipo de error ingresado
        /// </summary>
        /// <param name="tipoError"></param>
        public void IncrementarTipoError(string tipoError)
        {
            var registro = _listaTipoError.Find(x => x.TipoObservacion == tipoError);
            if(registro != null)
                registro.TotalObservacion++;
        }
        /// <summary>
        /// Indica si no hay errores(se grabara en otro modulo directamente el archivo enviado), 
        /// si solo hay blancos(para preguntar si se quiere reemplazar blancos por ceros)
        /// o si hay errores -> no se grabara el archivo enviado
        /// </summary>
        /// <returns>1 -> si Hay errores definitivos, 2 -> Hay solo Blanco, 3 -> si no hay errores</returns>
        public short IndicarGrabarArchivo()
        {
            int cantErroresDefinitivos = 0;
            int cantErroresBlancos = 0;
            int cantErroresImportExport = 0;
            foreach (var err in this._listaTipoError)
            {
                switch (err.TipoObservacion)
                {
                    case ConstantesMedidores.ERR_BLANCOS:
                        cantErroresBlancos += err.TotalObservacion;
                        break;
                    case ConstantesMedidores.ERR_IMPORT_EXPORT:
                        cantErroresImportExport += err.TotalObservacion;
                        break;
                    default:
                        cantErroresDefinitivos = cantErroresDefinitivos + err.TotalObservacion;
                        break;
                }
            }

            if (cantErroresDefinitivos > 0)
                return 1;
            if (cantErroresBlancos > 0)
                return 2;
            if (cantErroresImportExport > 0)
                return 4;
            else
                return 3;
        }

    }
}
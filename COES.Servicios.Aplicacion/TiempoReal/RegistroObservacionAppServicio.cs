using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using COES.Servicios.Aplicacion.TiempoReal.Helper;
using COES.Dominio.DTO.Sic;
using System.Configuration;

namespace COES.Servicios.Aplicacion.RegistroObservacion
{
    /// <summary>
    /// Clases con métodos del módulo RegistroObservacion
    /// </summary>
    public class RegistroObservacionAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(RegistroObservacionAppServicio));

        #region Métodos Tabla TR_OBSERVACION


        /// <summary>
        /// Permite grabar los datos de la observación
        /// </summary>
        /// <param name="canales"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="observacion"></param>
        /// <param name="idObservacion"></param>
        /// <returns></returns>
        public int GrabarObservacion(string[][] canales, int idEmpresa, string observacion, int idObservacion, string usuario, int agente,
            out string descEstado, string tipo, string idEstado, string observacionAgente)
        {
            try
            {
                int id = 0;
                descEstado = string.Empty;

                List<string> listaCorreo = FactoryScada.GetTrObservacionCorreoRepository().GetByCriteria(idEmpresa).
                    Select(x => x.Obscoremail).Distinct().ToList();

                List<string> listAdministradores = FactoryScada.GetTrObservacionCorreoRepository().GetByCriteria(0).
                    Select(x => x.Obscoremail).Distinct().ToList();

                TrObservacionDTO empresa = FactoryScada.GetTrObservacionRepository().ObtenerEmpresa(idEmpresa);

                List<TrObservacionItemDTO> listadoItems = new List<TrObservacionItemDTO>();

                //- Debemos realiizar las siguientes acciones:
                if (idObservacion == 0)
                {
                    //-Creamos el registro
                    TrObservacionDTO entity = new TrObservacionDTO
                    {
                        Obscancomentario = observacion,
                        Emprcodi = idEmpresa,
                        Obscanestado = ConstantesRegistroObservacion.EstadoPendiente,
                        Obscanusucreacion = usuario,
                        Obscanfeccreacion = DateTime.Now,
                        Obscantipo = tipo,
                        Obscanproceso = ConstantesRegistroObservacion.ProcesoManual

                    };

                    id = FactoryScada.GetTrObservacionRepository().Save(entity);

                    if (tipo == ConstantesRegistroObservacion.TipoObservacion)
                    {

                        //-Creamos los registros de detalle                
                        for (int i = 1; i < canales.Length; i++)
                        {
                            TrObservacionItemDTO itemObservacion = new TrObservacionItemDTO
                            {
                                Obscancodi = id,
                                Canalcodi = Convert.ToInt32(canales[i][0]),
                                Obsiteestado = ObservacionHelper.ObtenerCodigoEstado(canales[i][4]),
                                Obsitecomentario = canales[i][5],
                                Obsiteusuario = usuario,
                                Obsitefecha = DateTime.Now,
                                Zonanomb = canales[i][1],
                                Canalnomb = canales[i][2],
                                Canaliccp = canales[i][3],
                                Descestado = canales[i][4],
                                Obsiteproceso = ConstantesRegistroObservacion.ProcesoManual
                            };

                            int idItem = FactoryScada.GetTrObservacionItemRepository().Save(itemObservacion);

                            listadoItems.Add(itemObservacion);

                            //- Creamos el item de historia
                            TrObservacionItemEstadoDTO itemObsHistoria = new TrObservacionItemEstadoDTO
                            {
                                Obsitecodi = idItem,
                                Obitesestado = itemObservacion.Obsiteestado,
                                Obitescomentario = itemObservacion.Obsitecomentario,
                                Obitesusuario = itemObservacion.Obsiteusuario,
                                Obitesfecha = itemObservacion.Obsitefecha
                            };

                            FactoryScada.GetTrObservacionItemEstadoRepository().Save(itemObsHistoria);
                        }

                        //- Envío de corro cuando se registra el listado de observaciones
                        string mensaje = ObservacionHelper.ObtenerCorreoNotificacion(listadoItems, 0);
                        mensaje = mensaje.Replace("[", "{");
                        mensaje = mensaje.Replace("]", "}");
                        string asunto = ObservacionHelper.ObtenerAsuntoNotificacion(empresa.Emprnomb, 0);
                        COES.Base.Tools.Util.SendEmail(listaCorreo, new List<string>(), asunto, mensaje);
                    }
                }
                else
                {
                    //-Debemos actualizar el registro

                    id = idObservacion;

                    //if (agente == 0)
                    //{
                    TrObservacionDTO entity = new TrObservacionDTO
                    {
                        Obscancomentario = observacion,
                        Emprcodi = idEmpresa,
                        Obscanusumodificacion = usuario,
                        Obscanfecmodificacion = DateTime.Now,
                        Obscancomentarioagente = observacionAgente,
                        Obscantipo = tipo,
                        Obscancodi = id
                    };

                    FactoryScada.GetTrObservacionRepository().Update(entity);
                    //}


                    if (tipo == ConstantesRegistroObservacion.TipoObservacion)
                    {

                        List<TrObservacionItemDTO> itemsActuales = FactoryScada.GetTrObservacionItemRepository().GetByCriteria(id);
                        List<TrObservacionItemDTO> itemsNuevos = new List<TrObservacionItemDTO>();

                        for (int i = 1; i < canales.Length; i++)
                        {
                            TrObservacionItemDTO itemObservacion = new TrObservacionItemDTO
                            {
                                Obscancodi = id,
                                Canalcodi = Convert.ToInt32(canales[i][0]),
                                Obsiteestado = ObservacionHelper.ObtenerCodigoEstado(canales[i][4]),
                                Obsitecomentario = (agente == 1) ? canales[i][6] : canales[i][5],
                                Obsitecomentarioagente = (agente == 1) ? canales[i][5] : canales[i][6],
                                Obsiteusuario = usuario,
                                Obsitefecha = DateTime.Now,
                                Zonanomb = canales[i][1],
                                Canalnomb = canales[i][2],
                                Canaliccp = canales[i][3],
                                Descestado = canales[i][4]
                            };

                            itemsNuevos.Add(itemObservacion);
                        }

                        listadoItems = itemsNuevos;

                        if (agente == 0)
                        {
                            //- Eliminamos los que no existen en la lista nueva
                            List<int> idsEliminar = itemsActuales.Where(x => !itemsNuevos.Any(y => x.Canalcodi == y.Canalcodi)).Select(x => x.Obsitecodi).ToList();
                            foreach (int idEliminar in idsEliminar)
                            {
                                FactoryScada.GetTrObservacionItemRepository().Delete(idEliminar);
                            }
                        }

                        //- Seleccionamos los que se tienen que actualizar
                        List<TrObservacionItemDTO> itemsActualizar = itemsNuevos.Where(x => itemsActuales.Any(y => x.Canalcodi == y.Canalcodi)).ToList();
                        foreach (TrObservacionItemDTO itemActualizar in itemsActualizar)
                        {
                            //-Jalamos 
                            TrObservacionItemDTO itemPrevio = itemsActuales.Where(x => x.Canalcodi == itemActualizar.Canalcodi).FirstOrDefault();

                            itemActualizar.Obsitecodi = itemPrevio.Obsitecodi;

                            FactoryScada.GetTrObservacionItemRepository().Update(itemActualizar);

                            //- Creamos el item de historia
                            TrObservacionItemEstadoDTO itemObsHistoria = new TrObservacionItemEstadoDTO
                            {
                                Obsitecodi = itemActualizar.Obsitecodi,
                                Obitesestado = itemActualizar.Obsiteestado,
                                Obitescomentario = itemActualizar.Obsitecomentario,
                                Obitesusuario = itemActualizar.Obsiteusuario,
                                Obitesfecha = itemActualizar.Obsitefecha
                            };

                            if (itemPrevio.Obsitecomentario == null) itemPrevio.Obsitecomentario = string.Empty;
                            if (itemPrevio.Obsitecomentarioagente == null) itemPrevio.Obsitecomentarioagente = string.Empty;

                            //- Debemos hacer las comparaciones para ver si hubieron cambios
                            if (itemPrevio.Obsiteestado != itemActualizar.Obsiteestado ||
                                itemPrevio.Obsitecomentario != itemActualizar.Obsitecomentario ||
                                itemPrevio.Obsitecomentarioagente != itemActualizar.Obsitecomentarioagente
                                )
                            {
                                FactoryScada.GetTrObservacionItemEstadoRepository().Save(itemObsHistoria);
                            }
                        }

                        if (agente == 0)
                        {
                            //- Seleccionamos los que se tienen que insertar
                            List<TrObservacionItemDTO> itemsInsertar = itemsNuevos.Where(x => !itemsActuales.Any(y => x.Canalcodi == y.Canalcodi)).ToList();
                            foreach (TrObservacionItemDTO itemInsertar in itemsInsertar)
                            {
                                int idItem = FactoryScada.GetTrObservacionItemRepository().Save(itemInsertar);

                                //- Creamos el item de historia
                                TrObservacionItemEstadoDTO itemObsHistoria = new TrObservacionItemEstadoDTO
                                {
                                    Obsitecodi = idItem,
                                    Obitesestado = itemInsertar.Obsiteestado,
                                    Obitescomentario = itemInsertar.Obsitecomentario,
                                    Obitesusuario = itemInsertar.Obsiteusuario,
                                    Obitesfecha = itemInsertar.Obsitefecha
                                };

                                FactoryScada.GetTrObservacionItemEstadoRepository().Save(itemObsHistoria);
                            }
                        }

                        //- Verificamos el estado actual de las observaciones para actualizar finalmente
                        List<string> estados = itemsNuevos.Select(x => x.Obsiteestado).Distinct().ToList();

                        string estado = ObservacionHelper.ObtenerEstadoGlobal(estados);
                        FactoryScada.GetTrObservacionRepository().ActualizarEstado(id, estado);
                        descEstado = ObservacionHelper.ObtenerDescripcionEstado(estado);



                        //- Enviar correo de notificacion

                        if (agente == 1)
                        {
                            //- Envío de mensajes a los administradores
                            string mensaje = ObservacionHelper.ObtenerCorreoNotificacion(listadoItems, 2);
                            mensaje = mensaje.Replace("[", "{");
                            mensaje = mensaje.Replace("]", "}");
                            string asunto = ObservacionHelper.ObtenerAsuntoNotificacion(empresa.Emprnomb, 2);
                            COES.Base.Tools.Util.SendEmail(listAdministradores, new List<string>(), asunto, mensaje);
                        }
                        else
                        {
                            if (ObservacionHelper.VerificarEdicion(estado) == ConstantesAppServicio.SI)
                            {
                                //- Envío de corro cuando se registra el listado de observaciones
                                string mensaje = ObservacionHelper.ObtenerCorreoNotificacion(listadoItems, 1);
                                mensaje = mensaje.Replace("[", "{");
                                mensaje = mensaje.Replace("]", "}");
                                string asunto = ObservacionHelper.ObtenerAsuntoNotificacion(empresa.Emprnomb, 1);
                                COES.Base.Tools.Util.SendEmail(listaCorreo, new List<string>(), asunto, mensaje);
                            }
                        }
                    }
                    else
                    {
                        FactoryScada.GetTrObservacionRepository().ActualizarEstado(id, idEstado);
                    }
                }

                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TR_OBSERVACION
        /// </summary>
        public void EliminarObservacion(int obscancodi)
        {
            try
            {
                FactoryScada.GetTrObservacionRepository().Delete(obscancodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TR_OBSERVACION
        /// </summary>
        public TrObservacionDTO ObtenerDatosObservacion(int id)
        {
            TrObservacionDTO entity = FactoryScada.GetTrObservacionRepository().GetById(id);
            entity.Desestado = ObservacionHelper.ObtenerDescripcionEstado(entity.Obscanestado);
            entity.IndEdicion = ObservacionHelper.VerificarEdicion(entity.Obscanestado);

            return entity;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TR_OBSERVACION
        /// </summary>
        public List<TrObservacionDTO> ListTrObservacions()
        {
            return FactoryScada.GetTrObservacionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla TrObservacion
        /// </summary>
        public List<TrObservacionDTO> ObtenerBusquedaObservaciones(int empresa, DateTime fechaInicio, DateTime fechaFin)
        {
            List<TrObservacionDTO> list = FactoryScada.GetTrObservacionRepository().GetByCriteria(empresa, fechaInicio, fechaFin);

            foreach (TrObservacionDTO item in list)
            {
                item.Obscanestado = ObservacionHelper.ObtenerDescripcionEstado(item.Obscanestado);
            }

            return list;
        }

        #endregion

        #region Métodos Tabla TR_OBSERVACION_ESTADO

        /// <summary>
        /// Inserta un registro de la tabla TR_OBSERVACION_ESTADO
        /// </summary>
        public void SaveTrObservacionEstado(TrObservacionEstadoDTO entity)
        {
            try
            {
                FactoryScada.GetTrObservacionEstadoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla TR_OBSERVACION_ESTADO
        /// </summary>
        public void UpdateTrObservacionEstado(TrObservacionEstadoDTO entity)
        {
            try
            {
                FactoryScada.GetTrObservacionEstadoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TR_OBSERVACION_ESTADO
        /// </summary>
        public void DeleteTrObservacionEstado(int obsestcodi)
        {
            try
            {
                FactoryScada.GetTrObservacionEstadoRepository().Delete(obsestcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TR_OBSERVACION_ESTADO
        /// </summary>
        public TrObservacionEstadoDTO GetByIdTrObservacionEstado(int obsestcodi)
        {
            return FactoryScada.GetTrObservacionEstadoRepository().GetById(obsestcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla TrObservacionEstado
        /// </summary>
        public List<TrObservacionEstadoDTO> ObtenerHistorialObservacion(int obscodi)
        {
            return FactoryScada.GetTrObservacionEstadoRepository().GetByCriteria(obscodi);
        }

        #endregion

        #region Métodos Tabla TR_OBSERVACION_ITEM

        /// <summary>
        /// Inserta un registro de la tabla TR_OBSERVACION_ITEM
        /// </summary>
        public void SaveTrObservacionItem(TrObservacionItemDTO entity)
        {
            try
            {
                FactoryScada.GetTrObservacionItemRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla TR_OBSERVACION_ITEM
        /// </summary>
        public void UpdateTrObservacionItem(TrObservacionItemDTO entity)
        {
            try
            {
                FactoryScada.GetTrObservacionItemRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TR_OBSERVACION_ITEM
        /// </summary>
        public void DeleteTrObservacionItem(int obsitecodi)
        {
            try
            {
                FactoryScada.GetTrObservacionItemRepository().Delete(obsitecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TR_OBSERVACION_ITEM
        /// </summary>
        public TrObservacionItemDTO GetByIdTrObservacionItem(int obsitecodi)
        {
            return FactoryScada.GetTrObservacionItemRepository().GetById(obsitecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TR_OBSERVACION_ITEM
        /// </summary>
        public List<TrObservacionItemDTO> ObtenerItemsObservacion(int obscodi)
        {
            return FactoryScada.GetTrObservacionItemRepository().GetByCriteria(obscodi);
        }

        /// <summary>
        /// Permite consultar el reporte de señales pendientes
        /// </summary>
        /// <returns></returns>
        public List<TrObservacionItemDTO> ObtenerReporteSeniales(int idEmpresa)
        {
            List<TrObservacionItemDTO> list = FactoryScada.GetTrObservacionItemRepository().ObtenerReporteSeniales(idEmpresa);
            foreach (TrObservacionItemDTO item in list)
            {
                item.Obsiteestado = ObservacionHelper.ObtenerDescripcionEstado(item.Obsiteestado);
                
            }
            return list;
        }

        #endregion

        #region Métodos Tabla TR_OBSERVACION_ITEM_ESTADO

        /// <summary>
        /// Inserta un registro de la tabla TR_OBSERVACION_ITEM_ESTADO
        /// </summary>
        public void SaveTrObservacionItemEstado(TrObservacionItemEstadoDTO entity)
        {
            try
            {
                FactoryScada.GetTrObservacionItemEstadoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla TR_OBSERVACION_ITEM_ESTADO
        /// </summary>
        public void UpdateTrObservacionItemEstado(TrObservacionItemEstadoDTO entity)
        {
            try
            {
                FactoryScada.GetTrObservacionItemEstadoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TR_OBSERVACION_ITEM_ESTADO
        /// </summary>
        public void DeleteTrObservacionItemEstado(int obitescodi)
        {
            try
            {
                FactoryScada.GetTrObservacionItemEstadoRepository().Delete(obitescodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TR_OBSERVACION_ITEM_ESTADO
        /// </summary>
        public TrObservacionItemEstadoDTO GetByIdTrObservacionItemEstado(int obitescodi)
        {
            return FactoryScada.GetTrObservacionItemEstadoRepository().GetById(obitescodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla TrObservacionItemEstado
        /// </summary>
        public List<TrObservacionItemEstadoDTO> ObtenerHistoriaItemObservacion(int canal, int idObservacion)
        {
            List<TrObservacionItemDTO> items = FactoryScada.GetTrObservacionItemRepository().GetByCriteria(idObservacion);
            TrObservacionItemDTO entity = items.Where(x => x.Canalcodi == canal).FirstOrDefault();

            if (entity != null)
            {
                List<TrObservacionItemEstadoDTO> entitys = FactoryScada.GetTrObservacionItemEstadoRepository().GetByCriteria(entity.Obsitecodi);

                foreach (TrObservacionItemEstadoDTO item in entitys)
                {
                    item.Obitesestado = ObservacionHelper.ObtenerDescripcionEstado(item.Obitesestado);
                }

                return entitys;
            }
            else
            {
                return new List<TrObservacionItemEstadoDTO>();
            }
        }

        #endregion

        #region Métodos Adicionales

        /// <summary>
        /// Permite obtener las empresas del SP7
        /// </summary>
        /// <returns></returns>
        public List<ScEmpresaDTO> ObtenerEmpresasScada()
        {
            return FactoryScada.GetTrObservacionRepository().ObtenerEmpresasScada();
        }

        /// <summary>
        /// Permite obtener la zonas por codigo de empresa
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public List<TrZonaSp7DTO> ObtenerZonasPorEmpresa(int empresa)
        {
            return FactoryScada.GetTrObservacionRepository().ObtenerZonasPorEmpresa(empresa);
        }

        /// <summary>
        /// Permite obtener los codigos de los canales
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="zona"></param>
        /// <param name="descripcion"></param>
        /// <returns></returns>
        public List<TrCanalSp7DTO> ObtenerCanales(int empresa, int zona, string tipo, string descripcion, int nroPagina, int nroFilas)
        {
            return FactoryScada.GetTrObservacionRepository().ObtenerCanalesObservacion(empresa, zona, tipo, descripcion, nroPagina, nroFilas);
        }

        /// <summary>
        /// Permite obtener el nro de páginas para la consulta de canales
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="zona"></param>
        /// <param name="tipo"></param>
        /// <param name="descripcion"></param>
        /// <returns></returns>
        public int ObtenerNroFilasBusquedaCanal(int empresa, int zona, string tipo, string descripcion)
        {
            return FactoryScada.GetTrObservacionRepository().ObtenerNroFilasBusquedaCanal(empresa, zona, tipo, descripcion);
        }

        /// <summary>
        /// Permite obtener la configuración de la grilla a mostrar
        /// </summary>
        /// <param name="idObservacion"></param>
        /// <returns></returns>
        public string[][] ObtenerConfiguracionGrillaCanal(int idObservacion, int agente)
        {
            List<TrObservacionItemDTO> list = new List<TrObservacionItemDTO>();
            int longitud = 0;
            if (idObservacion > 0)
            {
                list = FactoryScada.GetTrObservacionItemRepository().GetByCriteria(idObservacion);
                longitud = 1 + list.Count;
            }
            else
            {
                longitud = 2;
            }

            string[][] resultado = new string[longitud][];

            resultado[0] = new string[7];
            resultado[0][0] = "ID";
            resultado[0][1] = "Zona";
            resultado[0][2] = "Nombre";
            resultado[0][3] = "ICCP";
            resultado[0][4] = "Estado";
            resultado[0][5] = (agente == 1) ? "Comentario Agente" : "Comentario COES";
            resultado[0][6] = (agente == 1) ? "Comentario COES" : "Comentario Agente";


            for (int i = 0; i < longitud - 1; i++)
            {
                resultado[i + 1] = new string[7];
                resultado[i + 1][0] = (idObservacion > 0) ? list[i].Canalcodi.ToString() : string.Empty;
                resultado[i + 1][1] = (idObservacion > 0) ? list[i].Zonanomb : string.Empty;
                resultado[i + 1][2] = (idObservacion > 0) ? list[i].Canalnomb : string.Empty;
                resultado[i + 1][3] = (idObservacion > 0) ? list[i].Canaliccp : string.Empty;
                resultado[i + 1][4] = (idObservacion > 0) ? ObservacionHelper.ObtenerDescripcionEstado(list[i].Obsiteestado) : string.Empty;
                resultado[i + 1][5] = (idObservacion > 0) ? (agente == 1) ? list[i].Obsitecomentarioagente : list[i].Obsitecomentario : string.Empty;
                resultado[i + 1][6] = (idObservacion > 0) ? (agente == 1) ? list[i].Obsitecomentario : list[i].Obsitecomentarioagente : string.Empty;
            }

            return resultado;
        }

        /// <summary>
        /// Permite obtener los canales seleccionados
        /// </summary>
        /// <param name="canales"></param>
        /// <returns></returns>
        public string[][] ObtenerCanalesSeleccion(string canales)
        {

            //FIT - Señales no Disponibles - INICIO
            bool SenialesNoDisponibles = false;

            string[] canal = canales.Split(',');
            string[][] result = new string[canal.Length][];

            int j = 0;
            foreach (var item in canal)
            {

                result[j] = new string[5];
                string[] campos = item.Split('|');

                if (campos.Length > 1)
                {
                    SenialesNoDisponibles = true;

                    result[j][0] = campos[0].ToString();
                    result[j][1] = campos[1].ToString();
                    result[j][2] = campos[2].ToString();
                    result[j][3] = campos[3].ToString();
                    result[j][4] = campos[4].ToString();

                }


                j++;
            }

            if (SenialesNoDisponibles)
            {

                //Re-estructurando canales
                canales = "";

                for (int c = 0; c < canal.Length; c++)
                {
                    var constante = (c > 0) ? "," : "";
                    canales = canales + constante + result[c][0];

                }
            }

            //FIT - Señales no Disponibles - FIN

            List<TrCanalSp7DTO> entitys = FactoryScada.GetTrObservacionRepository().ObtenerCanalesPorCodigos(canales);

            string[][] resultado = new string[entitys.Count][];

            for (int i = 0; i < entitys.Count; i++)
            {
                resultado[i] = new string[7];
                resultado[i][0] = entitys[i].Canalcodi.ToString();
                resultado[i][1] = entitys[i].Zonanomb;
                resultado[i][2] = entitys[i].Canalnomb;
                resultado[i][3] = entitys[i].Canaliccp;

                if (SenialesNoDisponibles)
                {
                    resultado[i][4] = string.Empty;
                    resultado[i][5] = result[i][1];
                    resultado[i][6] = string.Empty;
                }
                else
                {
                    resultado[i][4] = string.Empty;
                    resultado[i][5] = string.Empty;
                    resultado[i][6] = string.Empty;
                }
            }

            return resultado;
        }

        /// <summary>
        /// Permite generar el archivo
        /// </summary>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        public void GenerarReporteExcel(List<TrObservacionDTO> list, string path, string filename)
        {
            try
            {
                string file = path + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("OBSERVACIONES");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "LISTADO OBSERVACIONES";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[index, 2].Value = "EMPRESA";
                        ws.Cells[index, 3].Value = "ESTADO";
                        ws.Cells[index, 4].Value = "OBSERVACIÓN";
                        ws.Cells[index, 5].Value = "USUARIO";
                        ws.Cells[index, 6].Value = "FECHA";

                        rg = ws.Cells[index, 2, index, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 6;
                        foreach (TrObservacionDTO item in list)
                        {
                            ws.Cells[index, 2].Value = item.Emprnomb;
                            ws.Cells[index, 3].Value = item.Obscanestado;
                            ws.Cells[index, 4].Value = item.Obscancomentario;
                            ws.Cells[index, 5].Value = item.Obscanusucreacion;
                            ws.Cells[index, 6].Value = (item.Obscanfeccreacion != null) ? ((DateTime)item.Obscanfeccreacion).ToString(ConstantesAppServicio.FormatoFecha) : string.Empty;

                            rg = ws.Cells[index, 2, index, 6];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[5, 2, index - 1, 6];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 40;
                        ws.Column(3).Width = 40;
                        ws.Column(4).Width = 60;
                        ws.Column(6).Width = 40;
                        ws.Column(7).Width = 40;

                        rg = ws.Cells[5, 3, index, 6];
                        rg.AutoFitColumns();

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("Logo", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener la lista de posibles estados
        /// </summary>
        /// <returns></returns>
        public string[] ObtenerEstados(int agente)
        {
            return ObservacionHelper.ObtenerListaEstados(agente);
        }

        #endregion

        #region Métodos Tabla TR_OBSERVACION_CORREO

        /// <summary>
        /// Inserta un registro de la tabla TR_OBSERVACION_CORREO
        /// </summary>
        public int SaveTrObservacionCorreo(TrObservacionCorreoDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Obscorcodi == 0)
                {
                    id = FactoryScada.GetTrObservacionCorreoRepository().Save(entity);
                }
                else
                {
                    FactoryScada.GetTrObservacionCorreoRepository().Update(entity);
                    id = entity.Obscorcodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla TR_OBSERVACION_CORREO
        /// </summary>
        public void UpdateTrObservacionCorreo(TrObservacionCorreoDTO entity)
        {
            try
            {
                FactoryScada.GetTrObservacionCorreoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TR_OBSERVACION_CORREO
        /// </summary>
        public void DeleteTrObservacionCorreo(int obscorcodi)
        {
            try
            {
                FactoryScada.GetTrObservacionCorreoRepository().Delete(obscorcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TR_OBSERVACION_CORREO
        /// </summary>
        public TrObservacionCorreoDTO GetByIdTrObservacionCorreo(int obscorcodi)
        {
            return FactoryScada.GetTrObservacionCorreoRepository().GetById(obscorcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TR_OBSERVACION_CORREO
        /// </summary>
        public List<TrObservacionCorreoDTO> ListTrObservacionCorreos()
        {
            return FactoryScada.GetTrObservacionCorreoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla TrObservacionCorreo
        /// </summary>
        public List<TrObservacionCorreoDTO> GetByCriteriaTrObservacionCorreos(int idEmpresa)
        {
            return FactoryScada.GetTrObservacionCorreoRepository().GetByCriteria(idEmpresa);
        }

        #endregion

        #region "FIT - Señales no Disponibles"

        /// <summary>
        /// Permite obtener el nro de páginas para la consulta de canales
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public int ObtenerNroFilasBusquedaCanalSenalesObservadasBusqueda(int empresa)
        {
            //int nroListacanales = FactoryScada.GetTrObservacionSp7Repository().ObtenerNroFilasBusquedaCanalSenalesObservadasBusqueda(empresa);

            return ObtenerSenalesObservadas(empresa).Count;
        }

        /// <summary>
        /// Permite obtener los codigos de los canales
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="nroPagina"></param>
        /// <param name="nroFilas"></param>
        /// <returns></returns>
        public List<TrCanalSp7DTO> ObtenerCanalesSenalesObservadasBusqueda(int empresa, int nroPagina, int nroFilas)
        {
            var Listacanales = FactoryScada.GetTrObservacionSp7Repository().ObtenerCanalesObservacionSenalesObservadasBusqueda(empresa, nroPagina, nroFilas);
            var ListacanalesObservadasReportadas = FactoryScada.GetTrObservacionRepository().ObtenerSenalesObservadasReportadas(empresa);


            if (ListacanalesObservadasReportadas != null)
            {
                if (ListacanalesObservadasReportadas.Count > 0)
                {
                    foreach (var item in ListacanalesObservadasReportadas)
                    {
                        Listacanales.RemoveAll(x => x.Canalcodi == item.Canalcodi);
                    }

                }
            }

            return Listacanales;

        }


        /// <summary>
        /// Permite obtener las señales observadas
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public List<TrCanalSp7DTO> ObtenerSenalesObservadas(int empresa)
        {
            var Listacanales = FactoryScada.GetTrObservacionSp7Repository().ObtenerSenalesObservadas(empresa);
            var ListacanalesObservadasReportadas = FactoryScada.GetTrObservacionRepository().ObtenerSenalesObservadasReportadas(empresa);
            
            if (ListacanalesObservadasReportadas != null)
            {
                if (ListacanalesObservadasReportadas.Count > 0)
                {
                    foreach (var item in ListacanalesObservadasReportadas)
                    {
                        Listacanales.RemoveAll(x => x.Canalcodi == item.Canalcodi);
                    }

                }
            }

            return Listacanales;
        }


        /// <summary>
        /// Notifica los congelamientos de señales
        /// </summary>
        /// <returns></returns>
        public int NotificarCongelamientoSeñales(int emprcodi, string correo)
        {
            List<ScEmpresaDTO> ListaEmpresas = new List<ScEmpresaDTO>();

            DateTime fecha = new DateTime();
            fecha = DateTime.Now.AddDays(-1); // dejar en -1


            if (emprcodi == -100)
            {
                ListaEmpresas = this.ObtenerEmpresasScada();
            }
            else
            {
                ListaEmpresas = this.ObtenerEmpresasScada().Where(x => x.Emprcodi == emprcodi).ToList();
            }

            int idEmpresa = 0;

            //NOTIFICACION DE CONGELAMIENTO DE SEÑALES
            foreach (var empresa in ListaEmpresas)
            {
                idEmpresa = empresa.Emprcodi;

                bool dato = FactoryScada.GetTrReporteversionSp7Repository().GetCongelamientoSeñales(idEmpresa, fecha);

                if (dato)
                {
                    List<string> listaCorreo = FactoryScada.GetTrObservacionCorreoRepository().GetByCriteria(idEmpresa).
                    Select(x => x.Obscoremail).Distinct().ToList();

                    //List<string> listaCorreo = new List<string>();
                    ////- Descomentar estas lineas
                    //listaCorreo.Add("scada.cco@coes.org.pe");
                    //listaCorreo.Add("juan.ponce@coes.org.pe");
                    //listaCorreo.Add("sheyla.caller@coes.org.pe");
                    //listaCorreo.Add("raul.castro@coes.org.pe");

                    List<string> listAdministradores = FactoryScada.GetTrObservacionCorreoRepository().GetByCriteria(0).
                           Select(x => x.Obscoremail).Distinct().ToList();

                    //- Envío de corro
                    string mensaje = ObservacionHelper.ObtenerCorreoNotificacionCongelamientoSeñales(empresa.Emprenomb);
                    mensaje = mensaje.Replace("[", "{");
                    mensaje = mensaje.Replace("]", "}");
                    string asunto = ObservacionHelper.ObtenerAsuntoNotificacionCongelamientoSeñales(empresa.Emprenomb);

                    if (emprcodi != -100)
                    {
                        if (!string.IsNullOrEmpty(correo))
                        {
                            listaCorreo.Clear();
                            listaCorreo.Add(correo);
                            COES.Base.Tools.Util.SendEmail(listaCorreo, listAdministradores, asunto, mensaje);
                        }
                    }
                    else
                    {
                        COES.Base.Tools.Util.SendEmail(listaCorreo, listAdministradores, asunto, mensaje);
                    }


                }


            }


            return 1;
        }


        /// <summary>
        /// Notifica los indices de disponiblidad
        /// </summary>
        /// <returns></returns>
        public int NotificarIndicesDisponibilidad(int emprcodi, string correo)
        {
            List<ScEmpresaDTO> ListaEmpresas = new List<ScEmpresaDTO>();
            DateTime fecha = new DateTime();
            fecha = DateTime.Now.AddDays(-1);


            if (emprcodi == -100)
            {
                ListaEmpresas = this.ObtenerEmpresasScada();
            }
            else
            {
                ListaEmpresas = this.ObtenerEmpresasScada().Where(x => x.Emprcodi == emprcodi).ToList();
            }

            int idEmpresa = 0;

            //NOTIFICACION DE INDICES DE DISPONIBILIDAD
            foreach (var empresa in ListaEmpresas)
            {
                idEmpresa = empresa.Emprcodi;

                var dato = FactoryScada.GetTrReporteversionSp7Repository().GetEmpresasDiasVersion(idEmpresa, fecha);

                if (dato != null)
                {
                    List<string> listaCorreo = FactoryScada.GetTrObservacionCorreoRepository().GetByCriteria(idEmpresa).
                    Select(x => x.Obscoremail).Distinct().ToList();

                    //List<string> listaCorreo = new List<string>();
                    //listaCorreo.Add("scada.cco@coes.org.pe");
                    //listaCorreo.Add("juan.ponce@coes.org.pe");
                    //listaCorreo.Add("sheyla.caller@coes.org.pe");
                    //listaCorreo.Add("raul.castro@coes.org.pe");

                    List<string> listAdministradores = FactoryScada.GetTrObservacionCorreoRepository().GetByCriteria(0).
                            Select(x => x.Obscoremail).Distinct().ToList();


                    //- Envío de corro
                    string mensaje = ObservacionHelper.ObtenerCorreoNotificacionIndiceDisponibilidad(dato.Revciccpe.ToString(), empresa.Emprenomb);
                    mensaje = mensaje.Replace("[", "{");
                    mensaje = mensaje.Replace("]", "}");
                    string asunto = ObservacionHelper.ObtenerAsuntoNotificacionIndiceDisponibilidad(empresa.Emprenomb);

                    if (emprcodi != -100)
                    {
                        if (!string.IsNullOrEmpty(correo))
                        {
                            listaCorreo.Clear();
                            listaCorreo.Add(correo);
                            COES.Base.Tools.Util.SendEmail(listaCorreo, listAdministradores, asunto, mensaje);
                        }
                    }
                    else
                    {
                        COES.Base.Tools.Util.SendEmail(listaCorreo, listAdministradores, asunto, mensaje);
                    }


                }


            }


            return 1;
        }


        /// <summary>
        /// Permite grabar los datos de la observación
        /// </summary>
        /// <returns></returns>
        public int ProcesarSenalesObservadas(int emprcodi, string correo)
        {
            List<ScEmpresaDTO> ListaEmpresas = new List<ScEmpresaDTO>();

            if (emprcodi == -100)
            {
                ListaEmpresas = this.ObtenerEmpresasScada();
            }
            else
            {
                ListaEmpresas = this.ObtenerEmpresasScada().Where(x => x.Emprcodi == emprcodi).ToList();
            }

            int idEmpresa = 0;

            foreach (var empresa in ListaEmpresas)
            {
                idEmpresa = empresa.Emprcodi;

                var ListadoCanal = this.ObtenerSenalesObservadas(idEmpresa).ToList();

                if (ListadoCanal != null)
                {
                    if (ListadoCanal.Count > 0)
                    {

                        string[][] canales = new string[ListadoCanal.Count][];

                        for (int i = 0; i < ListadoCanal.Count; i++)
                        {

                            canales[i] = new string[9];
                            canales[i][0] = ListadoCanal[i].Canalcodi.ToString();
                            canales[i][1] = ListadoCanal[i].Zonanomb;
                            canales[i][2] = ListadoCanal[i].Canalnomb;
                            canales[i][3] = ListadoCanal[i].Canaliccp;
                            canales[i][4] = string.Empty;
                            canales[i][5] = ListadoCanal[i].Motivo;
                            canales[i][6] = string.Empty;
                            canales[i][7] = (ListadoCanal[i].Canalfhora != null) ? ((DateTime)ListadoCanal[i].Canalfhora).ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
                            canales[i][8] = (ListadoCanal[i].Canalfhora2 != null) ? ((DateTime)ListadoCanal[i].Canalfhora2).ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
                        }
                        
                        try
                        {
                            string observacion = "PROCESO AUTOMATICO - SEÑALES OBSERVADAS";
                            string username = "SISTEMA";

                            int id = this.GrabarObservacionBatch(canales, idEmpresa, observacion, 0, username, correo, emprcodi);
                            //return Json(new { Id = id, Estado = descEstado });

                        }
                        catch (Exception)
                        {
                            return -1;
                        }


                    }

                }

            }

            return 1;
        }

        /// <summary>
        /// Permite grabar los datos de la observación en BATCH
        /// </summary>
        /// <param name="canales"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="observacion"></param>
        /// <param name="idObservacion"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int GrabarObservacionBatch(string[][] canales, int idEmpresa, string observacion, int idObservacion, string usuario, string correo, int emprcodi)
        {
            try
            {
                int id = 0;

                List<string> listaCorreo = FactoryScada.GetTrObservacionCorreoRepository().GetByCriteria(idEmpresa).
                    Select(x => x.Obscoremail).Distinct().ToList();

                //List<string> listaCorreo = new List<string>();
                ////listaCorreo.Add("scada.cco@coes.org.pe");
                ////listaCorreo.Add("juan.ponce@coes.org.pe");
                ////listaCorreo.Add("sheyla.caller@coes.org.pe");
                //listaCorreo.Add("raul.castro@coes.org.pe");

                List<string> listAdministradores = FactoryScada.GetTrObservacionCorreoRepository().GetByCriteria(0).
                    Select(x => x.Obscoremail).Distinct().ToList();

                TrObservacionDTO empresa = FactoryScada.GetTrObservacionRepository().ObtenerEmpresa(idEmpresa);

                List<TrObservacionItemDTO> listadoItems = new List<TrObservacionItemDTO>();


                //-Creamos el registro
                TrObservacionDTO entity = new TrObservacionDTO
                {
                    Obscancomentario = observacion,
                    Emprcodi = idEmpresa,
                    Obscanestado = ConstantesRegistroObservacion.EstadoPendiente,
                    Obscanusucreacion = usuario,
                    Obscanfeccreacion = DateTime.Now,
                    Obscantipo = ConstantesRegistroObservacion.TipoObservacion,
                    Obscanproceso = ConstantesRegistroObservacion.ProcesoAutomatico

                };

                id = FactoryScada.GetTrObservacionRepository().Save(entity);

                //-Creamos los registros de detalle                
                for (int i = 0; i < canales.Length; i++) //se cambio de 1 a 0 (REVISAR!!!)
                {
                    TrObservacionItemDTO itemObservacion = new TrObservacionItemDTO
                    {
                        Obscancodi = id,
                        Canalcodi = Convert.ToInt32(canales[i][0]),
                        Obsiteestado = ObservacionHelper.ObtenerCodigoEstado(canales[i][4]),
                        Obsitecomentario = canales[i][5],
                        Obsiteusuario = usuario,
                        Obsitefecha = DateTime.Now,
                        Zonanomb = canales[i][1],
                        Canalnomb = canales[i][2],
                        Canaliccp = canales[i][3],
                        Descestado = canales[i][4],
                        Obsiteproceso = ConstantesRegistroObservacion.ProcesoAutomatico,
                        FechaEmpresa = canales[i][7],
                        FechaCoes = canales[i][8]
                    };

                    int idItem = FactoryScada.GetTrObservacionItemRepository().Save(itemObservacion);

                    listadoItems.Add(itemObservacion);

                    //- Creamos el item de historia
                    TrObservacionItemEstadoDTO itemObsHistoria = new TrObservacionItemEstadoDTO
                    {
                        Obsitecodi = idItem,
                        Obitesestado = itemObservacion.Obsiteestado,
                        Obitescomentario = itemObservacion.Obsitecomentario,
                        Obitesusuario = itemObservacion.Obsiteusuario,
                        Obitesfecha = itemObservacion.Obsitefecha
                    };

                    FactoryScada.GetTrObservacionItemEstadoRepository().Save(itemObsHistoria);
                }

                //- Envío de corro cuando se registra el listado de observaciones
                string mensaje = ObservacionHelper.ObtenerCorreoNotificacionAutomatico(listadoItems, empresa.Emprnomb);
                mensaje = mensaje.Replace("[", "{");
                mensaje = mensaje.Replace("]", "}");
                string asunto = ObservacionHelper.ObtenerAsuntoNotificacionAutomatico(empresa.Emprnomb);

                if (emprcodi != -100)
                {
                    if (!string.IsNullOrEmpty(correo))
                    {
                        listaCorreo.Clear();
                        listaCorreo.Add(correo);
                        COES.Base.Tools.Util.SendEmail(listaCorreo, listAdministradores, asunto, mensaje);
                    }
                }
                else
                {
                    COES.Base.Tools.Util.SendEmail(listaCorreo, listAdministradores, asunto, mensaje);
                }





                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Procesamiento automático de las señales con observación
        /// </summary>
        public void ProcesarRegistroAutomaticoSeniales()
        {
            try
            {
                this.ProcesarSenalesObservadas(-100, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Procesamiento automático de notificaciones via correo electrónico.
        /// </summary>
        public void ProcesarNotificacionAutomatica()
        {
            try
            {
                this.ProcesarSenalesCaidaEnlace(-100, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            try
            {
                this.NotificarIndicesDisponibilidad(-100, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            try
            {
                this.NotificarCongelamientoSeñales(-100, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite grabar los datos de la observación
        /// </summary>
        /// <returns></returns>
        public int ProcesarSenalesCaidaEnlace(int emprcodi, string correo)
        {
            List<ScEmpresaDTO> ListaEmpresas = new List<ScEmpresaDTO>();

            if (emprcodi == -100)
            {
                ListaEmpresas = this.ObtenerEmpresasScada();
            }
            else
            {
                ListaEmpresas = this.ObtenerEmpresasScada().Where(x => x.Emprcodi == emprcodi).ToList();
            }

            int idEmpresa = 0;

            DateTime fecha = new DateTime();
            fecha = DateTime.Now.AddDays(-1); //-regresar a -1

            foreach (var empresa in ListaEmpresas)
            {
                idEmpresa = empresa.Emprcodi;

                string fechaDesconexion = string.Empty;
                bool dato = FactoryScada.GetTrReporteversionSp7Repository().ObtenerCaidaEnlace(idEmpresa, fecha, out fechaDesconexion);

                if (dato)
                {
                    try
                    {
                        string observacion = "CAIDA DE ENLACE REGISTRADA A LAS " + fechaDesconexion;
                        string username = "SISTEMA";
                        
                        int id = 0;

                        List<string> listaCorreo = FactoryScada.GetTrObservacionCorreoRepository().GetByCriteria(idEmpresa).
                            Select(x => x.Obscoremail).Distinct().ToList();

                        //List<string> listaCorreo = new List<string>();
                        ////-descomentar estas lineass
                        //listaCorreo.Add("scada.cco@coes.org.pe");
                        //listaCorreo.Add("juan.ponce@coes.org.pe");
                        //listaCorreo.Add("sheyla.caller@coes.org.pe");
                        //listaCorreo.Add("raul.castro@coes.org.pe");

                        List<string> listAdministradores = FactoryScada.GetTrObservacionCorreoRepository().GetByCriteria(0).
                            Select(x => x.Obscoremail).Distinct().ToList();
                                               
                        List<TrObservacionItemDTO> listadoItems = new List<TrObservacionItemDTO>();
                        
                        //-Creamos el registro
                        TrObservacionDTO entity = new TrObservacionDTO
                        {
                            Obscancomentario = observacion,
                            Emprcodi = idEmpresa,
                            Obscanestado = ConstantesRegistroObservacion.EstadoPendiente,
                            Obscanusucreacion = username,
                            Obscanfeccreacion = DateTime.Now,
                            Obscantipo = ConstantesRegistroObservacion.TipoCaida,
                            Obscanproceso = ConstantesRegistroObservacion.ProcesoAutomatico

                        };

                        id = FactoryScada.GetTrObservacionRepository().Save(entity);


                        //- Envío de corro cuando se registra el listado de observaciones
                        string mensaje = ObservacionHelper.ObtenerCorreoNotificacionCaidaEnlace(empresa.Emprenomb, fechaDesconexion);
                        mensaje = mensaje.Replace("[", "{");
                        mensaje = mensaje.Replace("]", "}");
                        string asunto = ObservacionHelper.ObtenerAsuntoNotificacionCaidaEnlace(empresa.Emprenomb);

                        if (emprcodi != -100)
                        {
                            if (!string.IsNullOrEmpty(correo))
                            {
                                listaCorreo.Clear();
                                listaCorreo.Add(correo);
                                COES.Base.Tools.Util.SendEmail(listaCorreo, listAdministradores, asunto, mensaje);
                            }
                        }
                        else
                        {
                            COES.Base.Tools.Util.SendEmail(listaCorreo, listAdministradores, asunto, mensaje);
                        }



                    }
                    catch (Exception)
                    {
                        return -1;
                    }

                }

            }

            return 1;
        }



        /// <summary>
        /// Permite realizar búsquedas en la tablas empresas
        /// </summary>
        public List<ScEmpresaDTO> ObtenerBusquedaAsociocionesEmpresa(string nombre)
        {
            List<ScEmpresaDTO> list = FactoryScada.GetScEmpresaRepository().ObtenerBusquedaAsociocionesEmpresa(nombre);

            return list;
        }


        /// <summary>
        /// Lista las empresas
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresas()
        {
            return FactorySic.GetSiEmpresaRepository().ListGeneral();
        }

        /// <summary>
        /// Lista las empresas
        /// </summary>
        /// <returns></returns>
        public List<TrEmpresaSp7DTO> ListarEmpresasSp7()
        {
            return FactoryScada.GetTrEmpresaSp7Repository().List();
        }

        /// <summary>
        /// Permite grabar los datos de Asociacion Empresa
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="emprcodisp7"></param>
        /// <returns></returns>
        public int GrabarAsociacionEmpresa(int emprcodi, int emprcodisp7, string usuario)
        {
            try
            {
                var existe = FactoryScada.GetScEmpresaRepository().GetById(emprcodisp7);

                if (existe == null)
                {
                    TrEmpresaSp7DTO sp = FactoryScada.GetTrEmpresaSp7Repository().GetById(emprcodisp7);
                    ScEmpresaDTO entity = new ScEmpresaDTO();
                    entity.Emprcodisic = emprcodi;
                    entity.Emprcodi = emprcodisp7;
                    entity.Emprenomb = sp.Emprenomb;
                    entity.Emprusumodificacion = usuario;
                    FactoryScada.GetScEmpresaRepository().NuevoAsociacionEmpresa(entity);
                }
                else
                {
                    FactoryScada.GetScEmpresaRepository().GrabarAsociacionEmpresa(emprcodi, emprcodisp7, usuario);
                }

                FactorySic.GetSiEmpresaRepository().UpdateAsociacionEmpresa(emprcodi, emprcodisp7);

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite grabar los datos de Asociacion Empresa
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="emprcodisp7"></param>
        /// <returns></returns>
        public int EliminarAsociacionesEmpresa(int emprcodi, int emprcodisp7)
        {
            try
            {
                FactoryScada.GetScEmpresaRepository().EliminarAsociacionEmpresa(emprcodi);
                FactorySic.GetSiEmpresaRepository().EliminarAsociacionEmpresa(emprcodisp7);

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite actualizar el nombre de la empresa SP7
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="emprnomb"></param>
        public void ActualizarNombreEmpresaScadaSP7(int emprcodi, string emprnomb)
        {
            try
            {
                FactoryScada.GetTrEmpresaSp7Repository().ActualizarNombreEmpresa(emprcodi, emprnomb);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite generar el archivo
        /// </summary>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        public void GenerarReporteAsociacionEmpresaExcel(List<ScEmpresaDTO> list, string path, string filename)
        {
            try
            {
                string file = path + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("EMPRESA");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "ASOCIACIÓN EMPRESAS SISCOES - SCADA SP7";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[index, 2].Value = "CODIGO SISCOES";
                        ws.Cells[index, 3].Value = "NOMBRE EMPRESA SISCOES";
                        ws.Cells[index, 4].Value = "CODIGO SCADASP7";
                        ws.Cells[index, 5].Value = "NOMBRE EMPRESA SCADASP7";
                        ws.Cells[index, 6].Value = "-";

                        rg = ws.Cells[index, 2, index, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 6;
                        foreach (ScEmpresaDTO item in list)
                        {
                            ws.Cells[index, 2].Value = item.Emprcodisic;
                            ws.Cells[index, 3].Value = item.Emprnomb;
                            ws.Cells[index, 4].Value = item.Emprcodi;
                            ws.Cells[index, 5].Value = item.Emprenomb;
                            ws.Cells[index, 6].Value = "";

                            rg = ws.Cells[index, 2, index, 6];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            index++;
                        }

                        rg = ws.Cells[5, 2, index - 1, 6];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 40;
                        ws.Column(3).Width = 40;
                        ws.Column(4).Width = 60;
                        ws.Column(6).Width = 40;
                        ws.Column(7).Width = 40;

                        rg = ws.Cells[5, 3, index, 6];
                        rg.AutoFitColumns();

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("Logo", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

    }
}

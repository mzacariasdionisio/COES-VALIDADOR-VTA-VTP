using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Planificacion
{
    public class ModplanAppServicio: AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ModplanAppServicio));

        #region Métodos Tabla WB_VERSION_MODPLAN

        /// <summary>
        /// Inserta un registro de la tabla WB_VERSION_MODPLAN
        /// </summary>
        public void SaveWbVersionModplan(WbVersionModplanDTO entity)
        {
            try
            {
                if (entity.Vermplcodi == 0)
                {
                    FactorySic.GetWbVersionModplanRepository().Save(entity);
                }
                else
                {
                    FactorySic.GetWbVersionModplanRepository().Update(entity);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

     

        /// <summary>
        /// Elimina un registro de la tabla WB_VERSION_MODPLAN
        /// </summary>
        public void DeleteWbVersionModplan(int vermplcodi)
        {
            try
            {
                FactorySic.GetWbVersionModplanRepository().Delete(vermplcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_VERSION_MODPLAN
        /// </summary>
        public WbVersionModplanDTO GetByIdWbVersionModplan(int vermplcodi)
        {
            return FactorySic.GetWbVersionModplanRepository().GetById(vermplcodi);          
        }


        /// <summary>
        /// Permite obtener un registro de la tabla WB_VERSION_MODPLAN
        /// </summary>
        public WbVersionModplanDTO ObtenerDetalleVersion(int vermplcodi, string path, string url )
        {
            WbVersionModplanDTO entity = FactorySic.GetWbVersionModplanRepository().GetById(vermplcodi);
            WbArchivoModplanDTO modelo = FactorySic.GetWbArchivoModplanRepository().ObtenerDocumento(vermplcodi, 1.ToString());
            WbArchivoModplanDTO manual = FactorySic.GetWbArchivoModplanRepository().ObtenerDocumento(vermplcodi, 2.ToString());

            if (modelo != null)
            {
                if (FileServer.VerificarExistenciaFile(url, modelo.Arcmplnombre, path))
                {
                    entity.RutaModelo = modelo.Arcmplnombre;
                }
            }

            if (manual != null)
            {
                if (FileServer.VerificarExistenciaFile(url, manual.Arcmplnombre, path))
                {
                    entity.RutaManual = manual.Arcmplnombre;
                }
            }

            return entity;
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_VERSION_MODPLAN
        /// </summary>
        public WbVersionModplanDTO ObtenerDetalleVersionAdicional(int vermplcodi, string path, string url)
        {
            WbVersionModplanDTO entity = FactorySic.GetWbVersionModplanRepository().GetById(vermplcodi);

            List<WbArchivoModplanDTO> listFiles = FactorySic.GetWbArchivoModplanRepository().GetByCriteria(vermplcodi);

            entity.ListadoArchivos = listFiles;

            return entity;
        }

        /// <summary>
        /// Permite obtener los datos del archivo
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="indicador"></param>
        /// <param name="path"></param>
        /// <param name="url"></param>
        /// <param name="extension"></param>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public Stream ObtenerArchivo(int idVersion, string indicador, string path, string url, out string extension, out string nombre)
        {
            WbArchivoModplanDTO modelo = FactorySic.GetWbArchivoModplanRepository().ObtenerDocumento(idVersion, indicador);
            extension = modelo.Arcmplext;
            nombre = modelo.Arcmplnombre;
            Stream result = FileServer.DownloadToStream(url + nombre, path);
            return result;
        }

        /// <summary>
        /// Permite obtener los datos del archivo
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="indicador"></param>
        /// <param name="path"></param>
        /// <param name="url"></param>
        /// <param name="extension"></param>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public Stream ObtenerArchivoAdicional(int idVersion, string indicador, string path, string url, out string extension, out string nombre)
        {
            WbArchivoModplanDTO modelo = FactorySic.GetWbArchivoModplanRepository().GetById(int.Parse(indicador));
            extension = modelo.Arcmplext;
            nombre = modelo.Arcmplnombre + "." + modelo.Arcmplext;
            Stream result = FileServer.DownloadToStream(url + nombre, path);
            return result;
        }

        public WbArchivoModplanDTO ObtenerDetalleArchivo(string indicador)
        {
            WbArchivoModplanDTO modelo = FactorySic.GetWbArchivoModplanRepository().GetById(int.Parse(indicador));
            return modelo;
        }

        public void EditarFile(int idFile, string nombre, string descripcion, string url,  string pathModelo)
        {
            try
            {
                WbArchivoModplanDTO entity = FactorySic.GetWbArchivoModplanRepository().GetById(idFile);

                if (entity.Arcmplnombre != nombre)
                {
                    FileServer.RenameBlob(url, entity.Arcmplnombre + "." + entity.Arcmplext, nombre + "." +  entity.Arcmplext, pathModelo);
                }

                entity.Arcmplnombre = nombre;
                entity.Arcmpldesc = descripcion;
                FactorySic.GetWbArchivoModplanRepository().Update(entity);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite eliminar el archivo y su metadata
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="indicador"></param>
        /// <param name="path"></param>
        /// <param name="url"></param>
        /// <param name="extension"></param>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public void EliminarArchivo(int idVersion, string indicador, string path, string url)
        {
            try
            {
                WbArchivoModplanDTO modelo = FactorySic.GetWbArchivoModplanRepository().ObtenerDocumento(idVersion, indicador);
                FileServer.DeleteBlob(url + modelo.Arcmplnombre, path);
                FactorySic.GetWbArchivoModplanRepository().Delete(modelo.Arcmplcodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite eliminar el archivo y su metadata
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="indicador"></param>
        /// <param name="path"></param>
        /// <param name="url"></param>
        /// <param name="extension"></param>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public void EliminarArchivoAdicional(int idVersion, string indicador, string path, string url)
        {
            try
            {
                WbArchivoModplanDTO modelo = FactorySic.GetWbArchivoModplanRepository().GetById(int.Parse(indicador));
                FileServer.DeleteBlob(url + modelo.Arcmplnombre, path);
                FactorySic.GetWbArchivoModplanRepository().Delete(modelo.Arcmplcodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener las versiones del modplan por codigo de padre
        /// </summary>
        /// <param name="idPadre"></param>
        /// <returns></returns>
        public List<WbVersionModplanDTO> ObtenerVersionPorPadre(int idPadre, int tipo)
        {
            return FactorySic.GetWbVersionModplanRepository().ObtenerVersionPorPadre(idPadre, tipo);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_VERSION_MODPLAN
        /// </summary>
        public List<WbVersionModplanDTO> ListWbVersionModplans(int tipo)
        {
            return FactorySic.GetWbVersionModplanRepository().List(tipo);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla WbVersionModplan
        /// </summary>
        public List<WbVersionModplanDTO> GetByCriteriaWbVersionModplans()
        {
            return FactorySic.GetWbVersionModplanRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla WB_ARCHIVO_MODPLAN

        /// <summary>
        /// Inserta un registro de la tabla WB_ARCHIVO_MODPLAN
        /// </summary>
        public void SaveWbArchivoModplan(WbArchivoModplanDTO entity)
        {
            try
            {
                WbArchivoModplanDTO archivo = FactorySic.GetWbArchivoModplanRepository().ObtenerDocumento(entity.Vermplcodi, entity.Arcmplindtc);

                if (archivo != null)
                {
                    entity.Arcmplcodi = archivo.Arcmplcodi;
                    FactorySic.GetWbArchivoModplanRepository().Update(entity);
                }
                else
                {
                    FactorySic.GetWbArchivoModplanRepository().Save(entity);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta un registro de la tabla WB_ARCHIVO_MODPLAN
        /// </summary>
        public void SaveWbArchivoModelo(WbArchivoModplanDTO entity)
        {
            try
            {
                //WbArchivoModplanDTO archivo = FactorySic.GetWbArchivoModplanRepository().ObtenerDocumento(entity.Vermplcodi, entity.Arcmplindtc);

                //if (archivo != null)
                //{
                    //entity.Arcmplcodi = archivo.Arcmplcodi;
                    //FactorySic.GetWbArchivoModplanRepository().Update(entity);
                //}
                //else
                //{
                    FactorySic.GetWbArchivoModplanRepository().Save(entity);
                //}
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla WB_ARCHIVO_MODPLAN
        /// </summary>
        public void UpdateWbArchivoModplan(WbArchivoModplanDTO entity)
        {
            try
            {
                FactorySic.GetWbArchivoModplanRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla WB_ARCHIVO_MODPLAN
        /// </summary>
        public void DeleteWbArchivoModplan(int arcmplcodi)
        {
            try
            {
                FactorySic.GetWbArchivoModplanRepository().Delete(arcmplcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_ARCHIVO_MODPLAN
        /// </summary>
        public WbArchivoModplanDTO GetByIdWbArchivoModplan(int arcmplcodi)
        {
            return FactorySic.GetWbArchivoModplanRepository().GetById(arcmplcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_ARCHIVO_MODPLAN
        /// </summary>
        public List<WbArchivoModplanDTO> ListWbArchivoModplans()
        {
            return FactorySic.GetWbArchivoModplanRepository().List();
        }

        

        #endregion

        #region Reporte

        /// <summary>
        /// Permite obtener un reporte de descarga del MODPLAN
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<WbRegistroModplanDTO> ObtenerReporteDescarga(int idVersion, int tipo)
        {
            return FactorySic.GetWbRegistroModplanRepository().ObtenerReporte(idVersion, tipo);
        }

        /// <summary>
        /// Permite realizar el log de descarga
        /// </summary>
        /// <param name="entity"></param>
        public void SaveRegistroModPlan(WbRegistroModplanDTO entity)
        {
            try
            {
                FactorySic.GetWbRegistroModplanRepository().Save(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion
    }
}

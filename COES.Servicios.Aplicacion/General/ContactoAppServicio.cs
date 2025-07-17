using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.General
{
    public class ContactoAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ContactoAppServicio));

        #region Métodos generales

        /// <summary>
        /// Permite obtener los tipos de empresas
        /// </summary>
        /// <returns></returns>
        public List<SiTipoempresaDTO> ObtenerTipoEmpresas()
        {
            return FactorySic.GetSiTipoempresaRepository().ObtenerTiposEmpresaContacto();
        }

        /// <summary>
        /// Permite obtener las empresas por tipo 
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresas(int? idTipoEmpresa)
        {
            string tipo = (idTipoEmpresa != null) ? idTipoEmpresa.ToString() : (-2).ToString();
            return FactorySic.GetSiEmpresaRepository().GetEmpresaSistemaPorTipo(tipo);
        }

        /// <summary>
        /// Permite obtener las empresas por tipo 
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <returns></returns>
        public List<EmpresaContactoDTO> ObtenerEmpresasContacto()
        {
            List<SiEmpresaDTO> list = FactorySic.GetWbContactoRepository().ObtenerEmpresasContacto().OrderBy(x => x.Emprnomb).ToList();

            List<EmpresaContactoDTO> result = new List<EmpresaContactoDTO>();

            foreach (SiEmpresaDTO entity in list)
            {
                result.Add(new EmpresaContactoDTO
                {
                    Emprcodi = entity.Emprcodi,
                    Emprnomb = entity.Emprnomb
                });
            }

            return result;
        }

        /// <summary>
        /// Permite obtener los datos de empresas
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public SiEmpresaDTO ObtenerEmpresa(int idEmpresa)
        {
            return FactorySic.GetSiEmpresaRepository().GetById(idEmpresa);
        }

        #endregion

        #region Métodos Tabla WB_CONTACTO

        /// <summary>
        /// Inserta un registro de la tabla WB_CONTACTO
        /// </summary>
        public int SaveWbContacto(WbContactoDTO entity, string comites, string correos)
        {
            try
            {
                int id = 0;
                if (entity.Contaccodi == 0)
                {
                    id = FactorySic.GetWbContactoRepository().Save(entity);
                }
                else
                {
                    id = entity.Contaccodi;
                    FactorySic.GetWbContactoRepository().Update(entity);
                    FactorySic.GetWbComiteContactoRepository().Delete(id, -1);
                    FactorySic.GetWbComiteListaContactoRepository().Delete(id, -1);
                }

                if (!string.IsNullOrEmpty(comites))
                {
                    List<int> idsComites = comites.Split(ConstantesAppServicio.CaracterComa).Select(i => int.Parse(i)).ToList();

                    foreach (int idComite in idsComites)
                    {
                        WbComiteContactoDTO item = new WbComiteContactoDTO
                        {
                            Comitecodi = idComite,
                            Contaccodi = id
                        };

                        FactorySic.GetWbComiteContactoRepository().Save(item);
                    }
                }

                if (!string.IsNullOrEmpty(correos))
                {
                    List<int> idsCorreos = correos.Split(ConstantesAppServicio.CaracterComa).Select(i => int.Parse(i)).ToList();

                    foreach (int idCorreo in idsCorreos)
                    {
                        WbComiteListaContactoDTO item = new WbComiteListaContactoDTO
                        {
                            ComiteListacodi = idCorreo,
                            Contaccodi = id
                        };

                        FactorySic.GetWbComiteListaContactoRepository().Save(item);
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
        /// Elimina un registro de la tabla WB_CONTACTO
        /// </summary>
        public void DeleteWbContacto(int contaccodi)
        {
            try
            {
                FactorySic.GetWbComiteContactoRepository().Delete(contaccodi, -1);
                FactorySic.GetWbContactoRepository().Delete(contaccodi);
                FactorySic.GetWbComiteListaContactoRepository().Delete(contaccodi, -1);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_CONTACTO
        /// </summary>
        public WbContactoDTO GetByIdWbContacto(int contaccodi, string fuente)
        {
            return FactorySic.GetWbContactoRepository().GetById(contaccodi, fuente);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_CONTACTO
        /// </summary>
        public List<WbContactoDTO> ListWbContactos()
        {
            return FactorySic.GetWbContactoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla WbContacto
        /// </summary>
        public List<WbContactoDTO> GetByCriteriaWbContactos(int? idTipoEmpresa, int? idEmpresa, string fuente, int? idComite, int? idComiteLista, int? idProceso)
        {
            if (idTipoEmpresa == null) idTipoEmpresa = -1;
            if (idEmpresa == null) idEmpresa = -1;
            if (string.IsNullOrEmpty(fuente))
            {
                fuente = (-1).ToString();
                idComite = -2;
            }
            if (idComite == null) idComite = -1;
            return FactorySic.GetWbContactoRepository().GetByCriteria(idTipoEmpresa, idEmpresa, fuente, idComite, idProceso, idComiteLista);
        }

        #endregion

        #region Métodos Tabla WB_COMITE
        /// <summary>
        /// Inserta un registro de la tabla WB_COMITE
        /// </summary>
        public void SaveWbComite(WbComiteDTO entity)
        {
            try
            {
                FactorySic.GetWbComiteRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void SaveWbComiteLista(WbComiteListaDTO entity)
        {
            try
            {
                FactorySic.GetWbComiteListaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla WB_COMITE
        /// </summary>
        public void UpdateWbComite(WbComiteDTO entity)
        {
            try
            {
                FactorySic.GetWbComiteRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla WB_COMITE
        /// </summary>
        public void DeleteWbComite(int comitecodi)
        {
            try
            {
                FactorySic.GetWbComiteRepository().Delete(comitecodi);
                FactorySic.GetWbComiteContactoRepository().Delete(-1, comitecodi);
                FactorySic.GetWbComiteListaContactoRepository().Delete(-1, comitecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteWbComiteLista(int comitelistacodi)
        {
            try
            {
                FactorySic.GetWbComiteListaRepository().Delete(comitelistacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_COMITE
        /// </summary>
        public WbComiteDTO GetByIdWbComite(int comitecodi)
        {
            return FactorySic.GetWbComiteRepository().GetById(comitecodi);
        }

        public List<WbComiteListaDTO> GetByIdWbComiteLista(int comitecodi)
        {
            return FactorySic.GetWbComiteListaRepository().ListByComite(comitecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_COMITE
        /// </summary>
        public List<WbComiteDTO> ListWbComites()
        {
            return FactorySic.GetWbComiteRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla WbComite
        /// </summary>
        public List<WbComiteDTO> GetByCriteriaWbComites()
        {
            return FactorySic.GetWbComiteRepository().GetByCriteria();
        }
        #endregion

        #region Métodos Tabla WB_COMITE_CONTACTO

        /// <summary>
        /// Inserta un registro de la tabla WB_COMITE_CONTACTO
        /// </summary>
        public void SaveWbComiteContacto(WbComiteContactoDTO entity)
        {
            try
            {
                FactorySic.GetWbComiteContactoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla WB_COMITE_CONTACTO
        /// </summary>
        public void UpdateWbComiteContacto(WbComiteContactoDTO entity)
        {
            try
            {
                FactorySic.GetWbComiteContactoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla WB_COMITE_CONTACTO
        /// </summary>
        public void DeleteWbComiteContacto(int contaccodi)
        {
            try
            {
                FactorySic.GetWbComiteContactoRepository().Delete(contaccodi, -1);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_COMITE_CONTACTO
        /// </summary>
        public WbComiteContactoDTO GetByIdWbComiteContacto(int contaccodi, int comitecodi)
        {
            return FactorySic.GetWbComiteContactoRepository().GetById(contaccodi, comitecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_COMITE_CONTACTO
        /// </summary>
        public List<WbComiteContactoDTO> ListWbComiteContactos()
        {
            return FactorySic.GetWbComiteContactoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla WbComiteContacto
        /// </summary>
        public List<WbComiteContactoDTO> GetByCriteriaWbComiteContactos(int idContacto)
        {
            return FactorySic.GetWbComiteContactoRepository().GetByCriteria(idContacto);
        }

        #endregion

        #region Métodos Tabla WB_COMITE_LISTA_CONTACTO
        /// <summary>
        /// Inserta un registro de la tabla WB_COMITE_LISTA_CONTACTO
        /// </summary>
        public List<WbComiteListaContactoDTO> GetByCriteriaWbComiteListaContacto(int idContacto)
        {
            return FactorySic.GetWbComiteListaContactoRepository().GetByCriteria(idContacto);
        }
        #endregion

        #region Métodos Tabla WB_PROCESO
        /// <summary>
        /// Inserta un registro de la tabla WB_PROCESO
        /// </summary>
        public void SaveWbProceso(WbProcesoDTO entity)
        {
            try
            {
                FactorySic.GetWbProcesoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla WB_PROCESO
        /// </summary>
        public void UpdateWbProceso(WbProcesoDTO entity)
        {
            try
            {
                FactorySic.GetWbProcesoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla WB_PROCESO
        /// </summary>
        public void DeleteWbProceso(int procesocodi)
        {
            try
            {
                FactorySic.GetWbProcesoRepository().Delete(procesocodi);
                FactorySic.GetWbProcesoContactoRepository().Delete(-1, procesocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_PROCESO
        /// </summary>
        public WbProcesoDTO GetByIdWbProceso(int procesocodi)
        {
            return FactorySic.GetWbProcesoRepository().GetById(procesocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_PROCESO
        /// </summary>
        public List<WbProcesoDTO> ListWbProcesos()
        {
            return FactorySic.GetWbProcesoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla WbComite
        /// </summary>
        public List<WbProcesoDTO> GetByCriteriaWbProcesos()
        {
            return FactorySic.GetWbProcesoRepository().GetByCriteria();
        }
        #endregion

        #region Métodos Tabla WB_CONTACTO_PROCESO

        /// <summary>
        /// Inserta un registro de la tabla WB_CONTACTO
        /// </summary>
        public int SaveWbContactoProc(WbContactoDTO entity, string detalle)
        {
            try
            {
                int id = 0;
                if (entity.Contaccodi == 0)
                {
                    id = FactorySic.GetWbContactoRepository().Save(entity);
                }
                else
                {
                    id = entity.Contaccodi;
                    FactorySic.GetWbContactoRepository().Update(entity);
                    FactorySic.GetWbProcesoContactoRepository().Delete(id, -1);
                }

                if (!string.IsNullOrEmpty(detalle))
                {
                    List<int> idsDetalle = detalle.Split(ConstantesAppServicio.CaracterComa).Select(i => int.Parse(i)).ToList();

                    foreach (int idDetalle in idsDetalle)
                    {
                        WbProcesoContactoDTO item = new WbProcesoContactoDTO
                        {
                            Procesocodi = idDetalle,
                            Contaccodi = id
                        };

                        FactorySic.GetWbProcesoContactoRepository().Save(item);
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
        /// Elimina un registro de la tabla WB_CONTACTO
        /// </summary>
        public void DeleteWbContactoProc(int contaccodi)
        {
            try
            {
                FactorySic.GetWbProcesoContactoRepository().Delete(contaccodi, -1);
                FactorySic.GetWbContactoRepository().Delete(contaccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_CONTACTO
        /// </summary>
        public WbContactoDTO GetByIdWbContactoProc(int contaccodi, string fuente)
        {
            return FactorySic.GetWbContactoRepository().GetById(contaccodi, fuente);
        }

        #endregion

        #region Métodos Tabla WB_PROCESO_CONTACTO

        /// <summary>
        /// Inserta un registro de la tabla WB_PROCESO_CONTACTO
        /// </summary>
        public void SaveWbProcesoContacto(WbProcesoContactoDTO entity)
        {
            try
            {
                FactorySic.GetWbProcesoContactoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla WB_PROCESO_CONTACTO
        /// </summary>
        public void UpdateWbrProcesoContacto(WbProcesoContactoDTO entity)
        {
            try
            {
                FactorySic.GetWbProcesoContactoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla WB_PROCESO_CONTACTO
        /// </summary>
        public void DeleteWbProcesoContacto(int contaccodi)
        {
            try
            {
                FactorySic.GetWbProcesoContactoRepository().Delete(contaccodi, -1);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_PROCESO_CONTACTO
        /// </summary>
        public WbProcesoContactoDTO GetByIdWbProcesoContacto(int contaccodi, int comitecodi)
        {
            return FactorySic.GetWbProcesoContactoRepository().GetById(contaccodi, comitecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_PROCESO_CONTACTO
        /// </summary>
        public List<WbProcesoContactoDTO> ListWbProcesoContactos()
        {
            return FactorySic.GetWbProcesoContactoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla WbProcesoContacto
        /// </summary>
        public List<WbProcesoContactoDTO> GetByCriteriaWbProcesoContactos(int idContacto)
        {
            return FactorySic.GetWbProcesoContactoRepository().GetByCriteria(idContacto);
        }

        #endregion
    }

    public class EmpresaContactoDTO
    {
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
    }
}

using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace COES.Servicios.Aplicacion.ResarcimientoNTCSE
{
    public class AuditoriaAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private readonly ILog Logger = LogManager.GetLogger(typeof(AuditoriaAppServicio));


        #region Métodos Tabla SI_AUDITORIA_REGISTRO
        /// <summary>        
        /// Permite generar la auditoria
        /// </summary>
        public void GerarAuditoria(EntityBase entidadeDominio, String user, int id)
        {
            SiAuditoriaRegistroDTO auditoria = null;

            try
            {
                auditoria = new SiAuditoriaRegistroDTO();
                auditoria.AuditCodi = 0;
                //Consulta si Tiene Permisos de Auditar la Tabla
                int Taudit = FunctionTablasAuditables(entidadeDominio.RetornaNombreTablaXMLRoot());
                auditoria.TauditCodi = Taudit;
                auditoria.AuditNombreSistema = ConstantesAppServicio.NameSistema;
                auditoria.AuditTablaCodi = id;
                auditoria.AuditRegDet = entidadeDominio.SerializarParaXml();
                // Aquí, puede persistir la auditoría de una sola tabla de Auditoría. Cómo guardar sólo el objeto XML serializado.            
                auditoria.AuditUsuarioCreacion = user;
                auditoria.AuditFechaCreacion = DateTime.Now;
                if (auditoria.TauditCodi != 0)
                {
                    this.SaveSiAuditoriaRegistro(auditoria);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Funcion de Listado de Auditoria General
        /// </summary>
        public List<object> ListaAuditoriaRegistrosGeneralStringByObject(EntityBase entidadeDominio, int id, Type type)
        {
            var serializer = new XmlSerializer(type);
            var serializer2 = new XmlSerializer(type);
            List<object> result = new List<object>();
            object itemresult = null;
            TextReader reader = null;
            TextReader reader2 = null;
            string objectData = null;
            int icad = 0;
            List<SiAuditoriaRegistroDTO> ListAudit = this.DeserializeAuditoria(entidadeDominio, id);

            foreach (SiAuditoriaRegistroDTO itemCad in ListAudit)
            {
                object itemmof = null;
                object item = new object();
                object item2 = new object();
                objectData = Convert.ToString(itemCad.AuditRegDet);
                reader = new StringReader(objectData);
                reader2 = new StringReader(objectData);
                item = serializer.Deserialize(reader);
                item2 = serializer2.Deserialize(reader2);
                itemmof = item;
                if (icad == 0)
                {
                    itemresult = itemmof;
                }
                else
                {
                    string it = item.GetType().GetProperty(ConstantesAuditoria.Accion).GetValue(item).ToString();
                    if (it == ConstantesAuditoria.Modificar)
                    {
                        foreach (var Prop in item.GetType().GetProperties())
                        {
                            var aPropValue = Prop.GetValue(item) ?? string.Empty;
                            var bPropValue = Prop.GetValue(itemresult) ?? string.Empty;
                            if (aPropValue.ToString() != bPropValue.ToString())
                            {
                                //false;
                            }
                            else
                            {
                                if (Prop.Name != ConstantesAuditoria.Accion && Prop.Name.IndexOf(ConstantesAuditoria.UsuarioUpdate) == -1)
                                {
                                    PropertyInfo pinfo = type.GetProperty(Prop.Name);
                                    pinfo.SetValue(item, null, null);
                                }
                            }
                        }
                    }
                    else if (it == ConstantesAuditoria.Calcular)
                    {
                        foreach (var Prop in item.GetType().GetProperties())
                        {
                            var aPropValue = Prop.GetValue(item) ?? string.Empty;
                            var bPropValue = Prop.GetValue(itemresult) ?? string.Empty;
                            if (aPropValue.ToString() != bPropValue.ToString())
                            {
                                //false;
                            }
                            else
                            {
                                if (Prop.Name != ConstantesAuditoria.Accion && Prop.Name.IndexOf(ConstantesAuditoria.UsuarioUpdate) == -1 && Prop.Name.IndexOf(ConstantesAuditoria.FechaUpdate) == -1)
                                {
                                    PropertyInfo pinfo = type.GetProperty(Prop.Name);
                                    pinfo.SetValue(item, null, null);
                                }
                            }
                        }
                    }
                    else if (it == ConstantesAuditoria.Eliminar)
                    {

                    }
                    else
                    {
                        itemresult = item;
                    }
                    itemresult = item2;
                }
                result.Add(item);
                icad++;
            }
            return result;
        }

        /// <summary>
        /// Funcion de Listado de Auditoria por Tipo de Dto y Id de Registro
        /// </summary>
        public List<SiAuditoriaRegistroDTO> DeserializeAuditoria(EntityBase entidadeDominio, int id)
        {
            try
            {
                //Consulta si Tiene Permisos de Auditar la Tabla
                int Taudit = this.FunctionTablasAuditables(entidadeDominio.RetornaNombreTablaXMLRoot());
                return this.ListSiAuditoriaRegistros(Taudit, id);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw ex;
            }
        }

        public object XmlDeserializeFromString(string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;
            TextReader reader = null;
            try
            {
                reader = new StringReader(objectData);
                result = serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                reader.Close();
            }
            return result;
        }

        /// <summary>
        /// Determinacion de Tablas Auditables
        /// </summary>
        public int FunctionTablasAuditables(string Tabla)
        {
            int bol = 0;
            List<SiTablaAuditableDTO> list = this.GetByCriteriaSiTablaAuditable();
            foreach (SiTablaAuditableDTO item in list)
            {
                if (item.TauditNomb == Tabla)
                {
                    bol = item.TauditCodi;
                    break;
                }
            }
            return bol;
        }

        /// <summary>
        /// Inserta un registro de la tabla SI_AUDITORIA_REGISTRO
        /// </summary>
        public void SaveSiAuditoriaRegistro(SiAuditoriaRegistroDTO entity)
        {
            try
            {
                FactorySic.GetSiAuditoriaRegistroRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_AUDITORIA_REGISTRO
        /// </summary>
        public void UpdateSiAuditoriaRegistro(SiAuditoriaRegistroDTO entity)
        {
            try
            {
                FactorySic.GetSiAuditoriaRegistroRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_AUDITORIA_REGISTRO
        /// </summary>
        public void DeleteSiAuditoriaRegistro()
        {
            try
            {
                FactorySic.GetSiAuditoriaRegistroRepository().Delete();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_AUDITORIA_REGISTRO
        /// </summary>
        public SiAuditoriaRegistroDTO GetByIdSiAuditoriaRegistro()
        {
            return FactorySic.GetSiAuditoriaRegistroRepository().GetById();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_AUDITORIA_REGISTRO
        /// </summary>
        public List<SiAuditoriaRegistroDTO> ListSiAuditoriaRegistros(int Taudit, int id)
        {
            return FactorySic.GetSiAuditoriaRegistroRepository().List(Taudit, id);
        }

        /// <summary>
        /// Permite listar de la Lista de Usuarios de SI_AUDITORIA_REGISTRO
        /// </summary>
        public List<SiAuditoriaRegistroDTO> GetByUsuariosAuditoria()
        {
            return FactorySic.GetSiAuditoriaRegistroRepository().GetByUsuariosAuditoria();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiAuditoriaRegistro
        /// </summary>
        public List<SiAuditoriaRegistroDTO> GetByCriteriaSiAuditoriaRegistros()
        {
            return FactorySic.GetSiAuditoriaRegistroRepository().GetByCriteria();
        }

        #endregion


        #region Métodos Tabla SI_TABLA_AUDITABLE

        /// <summary>
        /// Inserta un registro de la tabla SI_TABLA_AUDITABLE
        /// </summary>
        public void SaveSiTablaAuditable(SiTablaAuditableDTO entity)
        {
            try
            {
                int id = FactorySic.GetSiTablaAuditableRepository().Save(entity);
                /// <summary>
                /// Funcion de Auditoria
                /// </summary>
                this.GerarAuditoria(entity, entity.TauditUsuarioCreacion, id);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_TABLA_AUDITABLE
        /// </summary>
        public void UpdateSiTablaAuditable(SiTablaAuditableDTO entity)
        {
            try
            {
                FactorySic.GetSiTablaAuditableRepository().Update(entity);
                /// <summary>
                /// Funcion de Auditoria
                /// </summary>
                this.GerarAuditoria(entity, entity.TauditUsuarioUpdate, entity.TauditCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_TABLA_AUDITABLE
        /// </summary>
        public void DeleteSiTablaAuditable(SiTablaAuditableDTO entity)
        {
            try
            {
                FactorySic.GetSiTablaAuditableRepository().Delete(entity.TauditCodi);
                /// <summary>
                /// Funcion de Auditoria
                /// </summary>
                this.GerarAuditoria(entity, entity.TauditUsuarioUpdate, entity.TauditCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_TABLA_AUDITABLE
        /// </summary>
        public SiTablaAuditableDTO GetByIdSiTablaAuditable(int tauditcodi)
        {
            return FactorySic.GetSiTablaAuditableRepository().GetById(tauditcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_TABLA_AUDITABLE
        /// </summary>
        public List<SiTablaAuditableDTO> ListSiTablaAuditable()
        {
            return FactorySic.GetSiTablaAuditableRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SI_TABLA_AUDITABLE
        /// </summary>
        public List<SiTablaAuditableDTO> GetByCriteriaSiTablaAuditable()
        {
            return FactorySic.GetSiTablaAuditableRepository().GetByCriteria();
        }

        #endregion


    }
}

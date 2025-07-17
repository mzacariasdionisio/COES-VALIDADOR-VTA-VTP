using COES.Base.Core;
using COES.Framework.Base.Helper;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Xml.Linq;


namespace COES.Base.DataHelper
{
    /// <summary>
    /// Clase de utilidad
    /// </sum
    public class DbUtil
    {
        private static SiAuditoriaHelper siAuditoriaHelper = new SiAuditoriaHelper();
        private static SiTablaAuditableHelper siTablaAuditableHelper = new SiTablaAuditableHelper();
        private static SiTablaAuditableConfHelper siTablaAuditableHelperConf = new SiTablaAuditableConfHelper();
        /// <summary>
        /// Permite verificar la auditoria de un registro
        /// </summary>
        public static string Audit(Database database, DbCommand command, string user)
        {
            try
            {
                
                string query = string.Empty;

                if (command != null)
                {
                    if (command.CommandText != null)
                    {
                        string table = string.Empty;
                        string operacion = string.Empty;
                        DbUtil.AnalizarQuery(command.CommandText.Trim().ToLower(), out table, out operacion);

                        if (!string.IsNullOrEmpty(table))
                        {
                            command.CommandText = DbUtil.QueryVerificarAuditoria(table);
                            object result = database.ExecuteScalar(command);
                            if (result != null)
                            {
                                int id = Convert.ToInt32(result);
                                if (id > 0)
                                {
                                    command.CommandText = DbUtil.QueryConfiguracionTabla(id);
                                    List<DbTablaAudit> columnas = null;
                                    List<DbTablaAudit> columnasValue = null;

                                    using (IDataReader dr = database.ExecuteReader(command))
                                    {
                                        columnas = DbUtil.ObtenerConfiguracionTabla(dr);

                                        if (operacion == DbConstantes.CommandUpdate)
                                        {
                                            command.CommandText = DbUtil.ObtenerQueryValoresRegistro(table, columnas, command);
                                            using (IDataReader drEdit = database.ExecuteReader(command))
                                            {
                                                columnasValue = DbUtil.ObtenerValoresRegistro(drEdit, columnas);
                                            }
                                        }
                                    }
                                    if (columnas.Count > 0)
                                    {
                                        query = DbUtil.ObtenerQueryAuditoria(id, operacion, columnas, columnasValue, command, database, user);
                                    }
                                }
                            }
                        }
                    }
                }

                return query;
            }
            catch
            {
                return string.Empty;
            }
        }
        
        /// <summary>
        /// Permite analizar el tipo de operación de un query
        /// </summary>
        /// <param name="query"></param>
        public static void AnalizarQuery(string query, out string tableName, out string tipoOperacion)
        {
            tableName = string.Empty;
            tipoOperacion = string.Empty;
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query))
                {
                    string[] items = query.Split(' ');

                    if (items.Length > 2)
                    {
                        string operacion = items[0];
                        string table = string.Empty;

                        if (operacion == DbConstantes.CommandInsert)
                        {
                            if (items[2].Contains("("))
                                table = items[2].Substring(0, items[2].IndexOf("("));
                            else
                                table = items[2];
                        }
                        if (operacion == DbConstantes.CommandUpdate)
                        {
                            table = items[1];
                        }
                        if (operacion == DbConstantes.CommandDelete)
                        {
                            table = items[2];
                        }

                        tableName = table;
                        tipoOperacion = operacion;
                    }
                }
            }
        }

        /// <summary>
        /// Query para verificar que se debe generar audioria en la tabla
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string QueryVerificarAuditoria(string table)
        {
            //return string.Format("select tauditcodi as contador from si_tabla_auditable where lower(tauditnomb) = '{0}'", table.Trim()); 
            return string.Format(siTablaAuditableHelper.SqlGetByCriteria, table.Trim());
        }

        /// <summary>
        /// Query para obtener la configuracion de la tabla auditable
        /// </summary>
        /// <param name="idTabla"></param>
        /// <returns></returns>
        public static string QueryConfiguracionTabla(int idTabla)
        {
            //return string.Format("select tauditconfcolumn, tauditconfindpk from si_tabla_auditable_conf where tauditcodi = {0} and tauditconfindestado = 'A'", idTabla); 
            return string.Format(siTablaAuditableHelperConf.SqlListarPorEstadoYTabla, idTabla);
        }

        /// <summary>
        /// Permite obtener la configuracion de columnas auditables de la tabla
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static List<DbTablaAudit> ObtenerConfiguracionTabla(IDataReader dr)
        {
            List<DbTablaAudit> entitys = new List<DbTablaAudit>();

            while (dr.Read())
            {
                DbTablaAudit entity = new DbTablaAudit();
                entity.ColumnName = dr.GetString(0);
                entity.ColumnIndPk = dr.GetString(1);    

                entitys.Add(entity);
            }

            return entitys;
        }

        /// <summary>
        /// Permite generar la auditoria del registro
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="?"></param>
        public static string ObtenerQueryValoresRegistro(string tablaName, List<DbTablaAudit> columnas,
            DbCommand commando)
        {
            List<DbParameter> parameters = commando.Parameters.Cast<DbParameter>().ToList();

            string pkName = columnas.Where(x => x.ColumnIndPk == ConstantesBase.SI).Select(x => x.ColumnName).FirstOrDefault();
            object pkValue = parameters.Where(x => x.ParameterName.ToUpper() == pkName.ToUpper()).Select(x => x.Value).FirstOrDefault();
            string query = "select " + string.Join(",", columnas.Select(x => x.ColumnName)) + " from " +
                tablaName + " where " + pkName + " = " + pkValue.ToString();

            return query;
        }

        /// <summary>
        /// Obtiene los valore del registro
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static List<DbTablaAudit> ObtenerValoresRegistro(IDataReader dr, List<DbTablaAudit> columnas)
        {
            List<DbTablaAudit> result = new List<DbTablaAudit>();

            if (dr.Read())
            {
                foreach (DbTablaAudit item in columnas)
                {
                    DbTablaAudit entity = new DbTablaAudit();
                    int index = dr.GetOrdinal(item.ColumnName);
                    entity.ColumnName = item.ColumnName;
                    entity.ColumnIndPk = item.ColumnIndPk;
                    entity.ColumnValue = dr.GetValue(index);

                    result.Add(entity);
                }
            }

            return result;
        }

        /// <summary>
        /// Permite obtener de generacion de auditoria de registros
        /// </summary>
        /// <returns></returns>
        public static string ObtenerQueryAuditoria(int idTabla , string operacion, List<DbTablaAudit> columnas,
            List<DbTablaAudit> columnasValues, DbCommand command, Database database, string user)
        {
            #region Armando del Query

            string query = siAuditoriaHelper.SqlGetMaxId;
            int id = 1;
            command.CommandText = query;
            object result = database.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            int idRegistro = 0;
            string detalle = string.Empty;
            string tipo = string.Empty;
                      string columnId = columnas.Where(x => x.ColumnIndPk == ConstantesBase.SI).Select(x => x.ColumnName).FirstOrDefault();
            List<DbParameter> parameters = command.Parameters.Cast<DbParameter>().ToList();
                        
            if (columnId != null)
            {
                object paramId = parameters.Where(x => x.ParameterName.ToUpper() == columnId.ToUpper()).Select(x => x.Value).FirstOrDefault();
                if (paramId != null) idRegistro = Convert.ToInt32(paramId);
            }
            else 
            {
                return string.Empty;
            }
            
            List<DbTablaAudit> listColumnAudit = columnas.Where(x => x.ColumnIndPk != ConstantesBase.SI).ToList();

            if (listColumnAudit.Count > 0)
            {
                if (operacion == DbConstantes.CommandInsert || operacion == DbConstantes.CommandUpdate)
                {
                    tipo = (operacion == DbConstantes.CommandInsert) ? DbConstantes.TipoInsert : DbConstantes.TipoUpdate;

                    foreach (DbTablaAudit item in listColumnAudit)
                    {
                        object itemColumn = parameters.Where(x => x.ParameterName.ToUpper() == item.ColumnName.ToUpper()).Select(x => x.Value).FirstOrDefault();
                        if (itemColumn != null)
                        {
                            if (operacion == DbConstantes.CommandUpdate)
                            {
                                object initialvalue = columnasValues.Where(x => x.ColumnName == item.ColumnName).Select(x => x.ColumnValue).FirstOrDefault();
                                if (item.ColumnValue != initialvalue)
                                {
                                    item.ColumnValue = itemColumn;
                                    item.IndAudit = ConstantesBase.SI;
                                }
                            }
                            else
                            {
                                item.ColumnValue = itemColumn;
                                item.IndAudit = ConstantesBase.SI;
                            }
                        }
                    }

                    XDocument xml = new XDocument(
                        new XElement("Auditoria",
                        listColumnAudit.Where(x => x.IndAudit == ConstantesBase.SI).Select(x => new XElement(x.ColumnName, x.ColumnValue))
                    ));

                    detalle = xml.ToString();
                }
                else if (operacion == DbConstantes.CommandDelete)
                {
                    tipo = DbConstantes.TipoDelete;
                    detalle = string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }

            #endregion
            
            #region Construcción del XML con la auditoria                       
            
            query = string.Format(@"insert into si_auditoria(auditcodi, tauditcodi, auditregcodi, auditregistro, auditindtipo, audituser, auditdate) 
                      values({0}, {1}, {2}, '{3}', '{4}', '{5}', sysdate)", id, idTabla, idRegistro, detalle, tipo, user);
            
            #endregion

            return query;
        }
    }

    /// <summary>
    /// Constantes usados en operaciones de la DB
    /// </summary>
    public class DbConstantes
    {
        public const string CommandInsert = "insert";
        public const string CommandUpdate = "update";
        public const string CommandDelete = "delete";
        public const string TipoInsert = "I";
        public const string TipoUpdate = "U";
        public const string TipoDelete = "D";
    }

    /// <summary>
    /// Mapea las columnas de una tabla
    /// </summary>
    public class DbTablaAudit
    {
        public string ColumnName { get; set; }
        public string ColumnIndPk { get; set; }
        public object ColumnValue { get; set; }        
        public string IndAudit { get; set; }
    }    
}

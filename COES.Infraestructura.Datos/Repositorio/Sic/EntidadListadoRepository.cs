using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla SI_EMPRESA
    /// </summary>
    public class EntidadListadoRepository : RepositoryBase, IEntidadListadoRepository
    {
        public EntidadListadoRepository(string strConn) : base(strConn)
        {
        }

        EntidadListadoHelper helper = new EntidadListadoHelper();
      

        public List<EntidadListadoDTO> ListMaestroEmpresa(string nombre)
        {
            string filtro = "";
            if(!nombre.Equals(""))
            {
                filtro = " AND UPPER(emp.EMPRNOMB) like UPPER('%" + nombre + "%') ";
            }

            string sqlQuery = string.Format(this.helper.SqlListMaestroEmpresa, filtro);
            List<EntidadListadoDTO> entitys = new List<EntidadListadoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EntidadListadoDTO entity = new EntidadListadoDTO();

                    int iCodigo = dr.GetOrdinal(helper.Codigo);
                    if (!dr.IsDBNull(iCodigo)) entity.EntidadCodigo = Convert.ToInt32(dr.GetValue(iCodigo));

                    int iDescripcion = dr.GetOrdinal(helper.Descripcion);
                    if (!dr.IsDBNull(iDescripcion)) entity.Descripcion = dr.GetString(iDescripcion);

                    int iCodOsinergmin = dr.GetOrdinal(helper.CodOsinergmin);
                    if (!dr.IsDBNull(iCodOsinergmin)) entity.CodigoOsinergmin = dr.GetString(iCodOsinergmin);

                    int iCampoAdicional = dr.GetOrdinal(helper.CampoAdicional);
                    if (!dr.IsDBNull(iCampoAdicional)) entity.CampoAdicional = dr.GetString(iCampoAdicional);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EntidadListadoDTO> ListMaestroUsuarioLibre(string nombre)
        {
            string filtro = "";
            if (!nombre.Equals(""))
            {
                filtro = " AND UPPER(emp.EMPRNOMB) like UPPER('%" + nombre + "%') ";
            }

            string sqlQuery = string.Format(this.helper.SqlListMaestroUsuarioLibre, filtro);
            List<EntidadListadoDTO> entitys = new List<EntidadListadoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EntidadListadoDTO entity = new EntidadListadoDTO();

                    int iCodigo = dr.GetOrdinal(helper.Codigo);
                    if (!dr.IsDBNull(iCodigo)) entity.EntidadCodigo = Convert.ToInt32(dr.GetValue(iCodigo));

                    int iDescripcion = dr.GetOrdinal(helper.Descripcion);
                    if (!dr.IsDBNull(iDescripcion)) entity.Descripcion = dr.GetString(iDescripcion);

                    int iCodOsinergmin = dr.GetOrdinal(helper.CodOsinergmin);
                    if (!dr.IsDBNull(iCodOsinergmin)) entity.CodigoOsinergmin = dr.GetString(iCodOsinergmin);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EntidadListadoDTO> ListMaestroSuministrador(string nombre)
        {
            string filtro = "";
            if (!nombre.Equals(""))
            {
                filtro = " AND UPPER(EQUINOMB) LIKE UPPER('%" + nombre + "%')";
            }

            string sqlQuery = string.Format(this.helper.SqlListMaestroSuministro, filtro);
            List<EntidadListadoDTO> entitys = new List<EntidadListadoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EntidadListadoDTO entity = new EntidadListadoDTO();

                    int iCodigo = dr.GetOrdinal(helper.Codigo);
                    if (!dr.IsDBNull(iCodigo)) entity.EntidadCodigo = Convert.ToInt32(dr.GetValue(iCodigo));

                    int iDescripcion = dr.GetOrdinal(helper.Descripcion);
                    if (!dr.IsDBNull(iDescripcion)) entity.Descripcion = dr.GetString(iDescripcion);

                    int iCodOsinergmin = dr.GetOrdinal(helper.CodOsinergmin);
                    if (!dr.IsDBNull(iCodOsinergmin)) entity.CodigoOsinergmin = dr.GetString(iCodOsinergmin);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EntidadListadoDTO> ListMaestroBarra(string nombre)
        {
            string filtro = "";
            if (!nombre.Equals(""))
            {
                filtro = " AND UPPER(BARRNOMBRE) LIKE UPPER('%" + nombre + "%')";
            }

            string sqlQuery = string.Format(this.helper.SqlListMaestroBarra, filtro);
            List<EntidadListadoDTO> entitys = new List<EntidadListadoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EntidadListadoDTO entity = new EntidadListadoDTO();

                    int iCodigo = dr.GetOrdinal(helper.Codigo);
                    if (!dr.IsDBNull(iCodigo)) entity.EntidadCodigo = Convert.ToInt32(dr.GetValue(iCodigo));

                    int iDescripcion = dr.GetOrdinal(helper.Descripcion);
                    if (!dr.IsDBNull(iDescripcion)) entity.Descripcion = dr.GetString(iDescripcion);

                    int iCodOsinergmin = dr.GetOrdinal(helper.CodOsinergmin);
                    if (!dr.IsDBNull(iCodOsinergmin)) entity.CodigoOsinergmin = dr.GetString(iCodOsinergmin);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EntidadListadoDTO> ListMaestroCentralGeneracion(string nombre)
        {
            string filtro = "";
            if (!nombre.Equals(""))
            {
                filtro = " AND UPPER(GRUPONOMB) LIKE UPPER('%" + nombre + "%')";
            }

            string sqlQuery = string.Format(this.helper.SqlListMaestroCentralGeneracion, filtro);
            List<EntidadListadoDTO> entitys = new List<EntidadListadoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EntidadListadoDTO entity = new EntidadListadoDTO();

                    int iCodigo = dr.GetOrdinal(helper.Codigo);
                    if (!dr.IsDBNull(iCodigo)) entity.EntidadCodigo = Convert.ToInt32(dr.GetValue(iCodigo));

                    int iDescripcion = dr.GetOrdinal(helper.Descripcion);
                    if (!dr.IsDBNull(iDescripcion)) entity.Descripcion = dr.GetString(iDescripcion);

                    int iCodOsinergmin = dr.GetOrdinal(helper.CodOsinergmin);
                    if (!dr.IsDBNull(iCodOsinergmin)) entity.CodigoOsinergmin = dr.GetString(iCodOsinergmin);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EntidadListadoDTO> ListMaestroGrupoGeneracion(string nombre)
        {
            string filtro = "";
            if (!nombre.Equals(""))
            {
                filtro = " AND UPPER(GRUPONOMB) LIKE UPPER('%" + nombre + "%')";
            }

            string sqlQuery = string.Format(this.helper.SqlListMaestroGrupoGeneracion, filtro);
            List<EntidadListadoDTO> entitys = new List<EntidadListadoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EntidadListadoDTO entity = new EntidadListadoDTO();

                    int iCodigo = dr.GetOrdinal(helper.Codigo);
                    if (!dr.IsDBNull(iCodigo)) entity.EntidadCodigo = Convert.ToInt32(dr.GetValue(iCodigo));

                    int iDescripcion = dr.GetOrdinal(helper.Descripcion);
                    if (!dr.IsDBNull(iDescripcion)) entity.Descripcion = dr.GetString(iDescripcion);

                    int iCodOsinergmin = dr.GetOrdinal(helper.CodOsinergmin);
                    if (!dr.IsDBNull(iCodOsinergmin)) entity.CodigoOsinergmin = dr.GetString(iCodOsinergmin);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EntidadListadoDTO> ListMaestroModoOperacion(string nombre)
        {
            string filtro = "";
            if (!nombre.Equals(""))
            {
                filtro = " AND UPPER(GRUPONOMB) LIKE UPPER('%" + nombre + "%')";
            }

            string sqlQuery = string.Format(this.helper.SqlListMaestroModoOperacion, filtro);
            List<EntidadListadoDTO> entitys = new List<EntidadListadoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EntidadListadoDTO entity = new EntidadListadoDTO();

                    int iCodigo = dr.GetOrdinal(helper.Codigo);
                    if (!dr.IsDBNull(iCodigo)) entity.EntidadCodigo = Convert.ToInt32(dr.GetValue(iCodigo));

                    int iDescripcion = dr.GetOrdinal(helper.Descripcion);
                    if (!dr.IsDBNull(iDescripcion)) entity.Descripcion = dr.GetString(iDescripcion);

                    int iCodOsinergmin = dr.GetOrdinal(helper.CodOsinergmin);
                    if (!dr.IsDBNull(iCodOsinergmin)) entity.CodigoOsinergmin = dr.GetString(iCodOsinergmin);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EntidadListadoDTO> ListMaestroCuenca(string nombre)
        {
            string filtro = "";
            if (!nombre.Equals(""))
            {
                filtro = " AND UPPER(EQUINOMB) LIKE UPPER('%" + nombre + "%')";
            }

            string sqlQuery = string.Format(this.helper.SqlListMaestroCuenca, filtro);
            List<EntidadListadoDTO> entitys = new List<EntidadListadoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EntidadListadoDTO entity = new EntidadListadoDTO();

                    int iCodigo = dr.GetOrdinal(helper.Codigo);
                    if (!dr.IsDBNull(iCodigo)) entity.EntidadCodigo = Convert.ToInt32(dr.GetValue(iCodigo));

                    int iDescripcion = dr.GetOrdinal(helper.Descripcion);
                    if (!dr.IsDBNull(iDescripcion)) entity.Descripcion = dr.GetString(iDescripcion);

                    int iCodOsinergmin = dr.GetOrdinal(helper.CodOsinergmin);
                    if (!dr.IsDBNull(iCodOsinergmin)) entity.CodigoOsinergmin = dr.GetString(iCodOsinergmin);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EntidadListadoDTO> ListMaestroEmbalse(string nombre)
        {
            string filtro = "";
            if (!nombre.Equals(""))
            {
                filtro = " AND UPPER(EQUINOMB) LIKE UPPER('%" + nombre + "%')";
            }

            string sqlQuery = string.Format(this.helper.SqlListMaestroEmbalse, filtro);
            List<EntidadListadoDTO> entitys = new List<EntidadListadoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EntidadListadoDTO entity = new EntidadListadoDTO();

                    int iCodigo = dr.GetOrdinal(helper.Codigo);
                    if (!dr.IsDBNull(iCodigo)) entity.EntidadCodigo = Convert.ToInt32(dr.GetValue(iCodigo));

                    int iDescripcion = dr.GetOrdinal(helper.Descripcion);
                    if (!dr.IsDBNull(iDescripcion)) entity.Descripcion = dr.GetString(iDescripcion);

                    int iCodOsinergmin = dr.GetOrdinal(helper.CodOsinergmin);
                    if (!dr.IsDBNull(iCodOsinergmin)) entity.CodigoOsinergmin = dr.GetString(iCodOsinergmin);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EntidadListadoDTO> ListMaestroLago(string nombre)
        {
            string filtro = "";
            if (!nombre.Equals(""))
            {
                filtro = " AND UPPER(EQUINOMB) LIKE UPPER('%" + nombre + "%')";
            }

            string sqlQuery = string.Format(this.helper.SqlListMaestroLago, filtro);
            List<EntidadListadoDTO> entitys = new List<EntidadListadoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EntidadListadoDTO entity = new EntidadListadoDTO();

                    int iCodigo = dr.GetOrdinal(helper.Codigo);
                    if (!dr.IsDBNull(iCodigo)) entity.EntidadCodigo = Convert.ToInt32(dr.GetValue(iCodigo));

                    int iDescripcion = dr.GetOrdinal(helper.Descripcion);
                    if (!dr.IsDBNull(iDescripcion)) entity.Descripcion = dr.GetString(iDescripcion);

                    int iCodOsinergmin = dr.GetOrdinal(helper.CodOsinergmin);
                    if (!dr.IsDBNull(iCodOsinergmin)) entity.CodigoOsinergmin = dr.GetString(iCodOsinergmin);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EntidadListadoDTO> ListResutado(string entidad, string pendiente)
        {
            string filtro = "";

            string fecha = GetFechaUltSincronizacion();

            if (!fecha.Equals(""))
            {
                filtro = "AND TO_CHAR(MAPENFECCREACION, 'DD/MM/YYYY HH24:MI:SS') = '" + fecha + "' ";
            }

            if (!entidad.Equals(""))
            {
                filtro = filtro + " AND MAPENENTIDAD = '" + entidad + "' ";
            }

            if (pendiente.Equals("L"))//Listo
            {
                filtro = filtro +  " AND MAPENESTADO = 0 ";
            }
            else//Pendiente
            {
                filtro = filtro + " AND MAPENESTADO <> 0 ";
            }

            //WHERE 1 =1 AND MAPENENTIDAD = 'Empresa' AND MAPENESTADO = 0

            string sqlQuery = string.Format(this.helper.SqlListResultado, filtro);
            List<EntidadListadoDTO> entitys = new List<EntidadListadoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EntidadListadoDTO entity = new EntidadListadoDTO();

                    int iCodigo = dr.GetOrdinal(helper.Codigo);
                    if (!dr.IsDBNull(iCodigo)) entity.EntidadCodigo = Convert.ToInt32(dr.GetValue(iCodigo));

                    int iDescripcion = dr.GetOrdinal(helper.Descripcion);
                    if (!dr.IsDBNull(iDescripcion)) entity.Descripcion = dr.GetString(iDescripcion);

                    int iCodOsinergmin = dr.GetOrdinal(helper.CodOsinergmin);
                    if (!dr.IsDBNull(iCodOsinergmin)) entity.CodigoOsinergmin = dr.GetString(iCodOsinergmin);

                    int iCampoAdicional = dr.GetOrdinal(helper.CampoAdicional);
                    if (!dr.IsDBNull(iCampoAdicional)) entity.CampoAdicional = dr.GetString(iCampoAdicional);

                    int iEstado = dr.GetOrdinal(helper.Estado);
                    if (!dr.IsDBNull(iEstado)) entity.Estado = dr.GetString(iEstado);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }


        public string GetFechaUltSincronizacion()
        {
            string fecha = "";
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlFechaUltSincronizacion);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    fecha = dr["FECHA"].ToString();
                }
            }
            return fecha;
        }

        //Metodo para la homologación
        public string SaveHomologacion(string query, string mapencodi)
        {
            //Actualziamos el campo en la tabla oficial
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);


            if(!mapencodi.Equals(""))
            {
                //Actualizamos el estado de las asignaciones pendientes
                string sqlQuery = string.Format(helper.SqlActualizarEstadoHomologacion, mapencodi);
                command = dbProvider.GetSqlStringCommand(sqlQuery);
                dbProvider.ExecuteNonQuery(command);
            }

            return "Registro Homologado";
        }

        public string ObtenerIdPendiente(string entidad, string codigo)
        {
            string codi = "";
            string sqlQuery = string.Format(helper.SqlObtenerIdPendiente, entidad, codigo);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    codi = dr["MAPENCODI"].ToString();
                }
            }
            return codi;
        }

        public string DeleteHomologacion(string query)
        {
            //Actualizamos el estado de la asignacion
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);

            return "Homologación Desecha";
        }

    }
}

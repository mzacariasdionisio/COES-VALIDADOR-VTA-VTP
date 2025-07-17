using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.Interfaces.Campania;
using COES.Infraestructura.Datos.Helper;
using COES.Infraestructura.Datos.Helper.Campania;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamArchivoInfoRepository: RepositoryBase, ICamArchInfoRepository
    {

        public CamArchivoInfoRepository(string strConn) : base(strConn) { }

        CamArchInfoHelper Helper = new CamArchInfoHelper();

        public List<ArchivoInfoDTO> GetArchivoInfoProyCodi(int proyCodi, int secccodi)
        {
            List<ArchivoInfoDTO> ArchivoInfoDTOs = new List<ArchivoInfoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetArchivoInfoProyCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);
            dbProvider.AddInParameter(command, "SECCCODI", DbType.Int32, secccodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ArchivoInfoDTO ob = new ArchivoInfoDTO();
                    ob.ArchCodi = !dr.IsDBNull(dr.GetOrdinal("ARCHCODI")) ? dr.GetInt32(dr.GetOrdinal("ARCHCODI")) : 0;
                    ob.SeccCodi = !dr.IsDBNull(dr.GetOrdinal("SECCCODI")) ? dr.GetInt32(dr.GetOrdinal("SECCCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.ArchNombre = !dr.IsDBNull(dr.GetOrdinal("ARCHNOMBRE")) ? dr.GetString(dr.GetOrdinal("ARCHNOMBRE")) : string.Empty;
                    ob.Descripcion = !dr.IsDBNull(dr.GetOrdinal("DESCRIPCION")) ? dr.GetString(dr.GetOrdinal("DESCRIPCION")) : string.Empty;
                    ob.ArchTipo = !dr.IsDBNull(dr.GetOrdinal("ARCHTIPO")) ? dr.GetString(dr.GetOrdinal("ARCHTIPO")) : string.Empty;
                    ob.ArchNombreGenerado = !dr.IsDBNull(dr.GetOrdinal("ARCHNOMBREGENERADO")) ? dr.GetString(dr.GetOrdinal("ARCHNOMBREGENERADO")) : string.Empty;
                    ob.ArchUbicacion = !dr.IsDBNull(dr.GetOrdinal("ARCHUBICACION")) ? dr.GetString(dr.GetOrdinal("ARCHUBICACION")) : string.Empty;
                    ob.ArchFechaSubida = !dr.IsDBNull(dr.GetOrdinal("ARCHFECHASUBIDA")) ? dr.GetDateTime(dr.GetOrdinal("ARCHFECHASUBIDA")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ArchivoInfoDTOs.Add(ob);
                }
            }

            return ArchivoInfoDTOs;
        }

        public List<ArchivoInfoDTO> GetArchivoInfoByProyCodi(int proyCodi)
        {
            List<ArchivoInfoDTO> ArchivoInfoDTOs = new List<ArchivoInfoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetArchivoInfoByProyCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ArchivoInfoDTO ob = new ArchivoInfoDTO();
                    ob.ArchCodi = !dr.IsDBNull(dr.GetOrdinal("ARCHCODI")) ? dr.GetInt32(dr.GetOrdinal("ARCHCODI")) : 0;
                    ob.SeccCodi = !dr.IsDBNull(dr.GetOrdinal("SECCCODI")) ? dr.GetInt32(dr.GetOrdinal("SECCCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.ArchNombre = !dr.IsDBNull(dr.GetOrdinal("ARCHNOMBRE")) ? dr.GetString(dr.GetOrdinal("ARCHNOMBRE")) : string.Empty;
                    ob.ArchTipo = !dr.IsDBNull(dr.GetOrdinal("ARCHTIPO")) ? dr.GetString(dr.GetOrdinal("ARCHTIPO")) : string.Empty;
                    ob.ArchNombreGenerado = !dr.IsDBNull(dr.GetOrdinal("ARCHNOMBREGENERADO")) ? dr.GetString(dr.GetOrdinal("ARCHNOMBREGENERADO")) : string.Empty;
                    ob.ArchUbicacion = !dr.IsDBNull(dr.GetOrdinal("ARCHUBICACION")) ? dr.GetString(dr.GetOrdinal("ARCHUBICACION")) : string.Empty;
                    ob.ArchFechaSubida = !dr.IsDBNull(dr.GetOrdinal("ARCHFECHASUBIDA")) ? dr.GetDateTime(dr.GetOrdinal("ARCHFECHASUBIDA")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ArchivoInfoDTOs.Add(ob);
                }
            }

            return ArchivoInfoDTOs;
        }

        public bool SaveArchivoInfo(ArchivoInfoDTO archivoInfoDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveArchivoInfo);
            dbProvider.AddInParameter(dbCommand, "ARCHCODI", DbType.Int32, ObtenerValorOrDefault(archivoInfoDTO.ArchCodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "SECCCODI", DbType.Int32, ObtenerValorOrDefault(archivoInfoDTO.SeccCodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.String, ObtenerValorOrDefault(archivoInfoDTO.ProyCodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "ARCHNOMBRE", DbType.String, ObtenerValorOrDefault(archivoInfoDTO.ArchNombre, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "DESCRIPCION", DbType.String, ObtenerValorOrDefault(archivoInfoDTO.Descripcion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ARCHNOMBREGENERADO", DbType.String, ObtenerValorOrDefault(archivoInfoDTO.ArchNombreGenerado, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ARCHTIPO", DbType.String, ObtenerValorOrDefault(archivoInfoDTO.ArchTipo, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ARCHUBICACION", DbType.String, ObtenerValorOrDefault(archivoInfoDTO.ArchUbicacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ARCHFECHASUBIDA", DbType.DateTime, ObtenerValorOrDefault(DateTime.Now, typeof(DateTime)));
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(archivoInfoDTO.UsuarioCreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteArchivoInfoById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteArchivoInfoById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastArchivoInfoId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastArchivoInfoId);
            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                count = Convert.ToInt32(result) + 1;
            }
            else
            {
                count = 1;
            }
            return count;
        }

        public ArchivoInfoDTO GetArchivoInfoById(int id)
        {
            ArchivoInfoDTO ob = new ArchivoInfoDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetArchivoInfoById);
            dbProvider.AddInParameter(commandHoja, "ARCHCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.ArchCodi = !dr.IsDBNull(dr.GetOrdinal("ARCHCODI")) ? dr.GetInt32(dr.GetOrdinal("ARCHCODI")) : 0;
                    ob.SeccCodi = !dr.IsDBNull(dr.GetOrdinal("SECCCODI")) ? dr.GetInt32(dr.GetOrdinal("SECCCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.ArchNombre = !dr.IsDBNull(dr.GetOrdinal("ARCHNOMBRE")) ? dr.GetString(dr.GetOrdinal("ARCHNOMBRE")) : string.Empty;
                    ob.Descripcion = !dr.IsDBNull(dr.GetOrdinal("DESCRIPCION")) ? dr.GetString(dr.GetOrdinal("DESCRIPCION")) : string.Empty;
                    ob.ArchNombreGenerado = !dr.IsDBNull(dr.GetOrdinal("ARCHNOMBREGENERADO")) ? dr.GetString(dr.GetOrdinal("ARCHNOMBREGENERADO")) : string.Empty;
                    ob.ArchTipo = !dr.IsDBNull(dr.GetOrdinal("ARCHTIPO")) ? dr.GetString(dr.GetOrdinal("ARCHTIPO")) : string.Empty;
                    ob.ArchNombreGenerado = !dr.IsDBNull(dr.GetOrdinal("ARCHNOMBREGENERADO")) ? dr.GetString(dr.GetOrdinal("ARCHNOMBREGENERADO")) : string.Empty;
                    ob.ArchUbicacion = !dr.IsDBNull(dr.GetOrdinal("ARCHUBICACION")) ? dr.GetString(dr.GetOrdinal("ARCHUBICACION")) : string.Empty;
                    ob.ArchFechaSubida = !dr.IsDBNull(dr.GetOrdinal("ARCHFECHASUBIDA")) ? dr.GetDateTime(dr.GetOrdinal("ARCHFECHASUBIDA")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public ArchivoInfoDTO GetArchivoInfoNombreArchivo(string nombre)
        {
            ArchivoInfoDTO ob = new ArchivoInfoDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetArchivoInfoNombreGenerado);
            dbProvider.AddInParameter(commandHoja, "ARCHNOMBREGENERADO", DbType.String, nombre);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.ArchCodi = !dr.IsDBNull(dr.GetOrdinal("ARCHCODI")) ? dr.GetInt32(dr.GetOrdinal("ARCHCODI")) : 0;
                    ob.SeccCodi = !dr.IsDBNull(dr.GetOrdinal("SECCCODI")) ? dr.GetInt32(dr.GetOrdinal("SECCCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.ArchNombre = !dr.IsDBNull(dr.GetOrdinal("ARCHNOMBRE")) ? dr.GetString(dr.GetOrdinal("ARCHNOMBRE")) : string.Empty;
                    ob.Descripcion = !dr.IsDBNull(dr.GetOrdinal("DESCRIPCION")) ? dr.GetString(dr.GetOrdinal("DESCRIPCION")) : string.Empty;
                    ob.ArchNombreGenerado = !dr.IsDBNull(dr.GetOrdinal("ARCHNOMBREGENERADO")) ? dr.GetString(dr.GetOrdinal("ARCHNOMBREGENERADO")) : string.Empty;
                    ob.ArchTipo = !dr.IsDBNull(dr.GetOrdinal("ARCHTIPO")) ? dr.GetString(dr.GetOrdinal("ARCHTIPO")) : string.Empty;
                    ob.ArchUbicacion = !dr.IsDBNull(dr.GetOrdinal("ARCHUBICACION")) ? dr.GetString(dr.GetOrdinal("ARCHUBICACION")) : string.Empty;
                    ob.ArchFechaSubida = !dr.IsDBNull(dr.GetOrdinal("ARCHFECHASUBIDA")) ? dr.GetDateTime(dr.GetOrdinal("ARCHFECHASUBIDA")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public bool UpdateArchivoInfoByProyCodi(int proyCodi,string ruta)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateUbicacionByproy);
            dbProvider.AddInParameter(dbCommand, "ARCHUBICACION", DbType.String, ObtenerValorOrDefault(ruta, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ObtenerValorOrDefault(proyCodi, typeof(int)));
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool UpdateArchivoInfo(ArchivoInfoDTO archivoInfoDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateArchivoInfo);
            dbProvider.AddInParameter(dbCommand, "SECCCODI", DbType.Int32, ObtenerValorOrDefault(archivoInfoDTO.SeccCodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.String, ObtenerValorOrDefault(archivoInfoDTO.ProyCodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "ARCHNOMBRE", DbType.String, ObtenerValorOrDefault(archivoInfoDTO.ArchNombre, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ARCHNOMBREGENERADO", DbType.String, ObtenerValorOrDefault(archivoInfoDTO.ArchNombreGenerado, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ARCHTIPO", DbType.String, ObtenerValorOrDefault(archivoInfoDTO.ArchTipo, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ARCHUBICACION", DbType.String, ObtenerValorOrDefault(archivoInfoDTO.ArchUbicacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ARCHFECHASUBIDA", DbType.DateTime, ObtenerValorOrDefault(archivoInfoDTO.ArchFechaSubida, typeof(DateTime)));
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, archivoInfoDTO.UsuarioModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "ARCHCODI", DbType.Int32, ObtenerValorOrDefault(archivoInfoDTO.ArchCodi, typeof(int)));
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public List<ArchivoInfoDTO> GetArchivoInfoProyCodiNom(int proyCodi, int secccodi, string nombre)
        {
            List<ArchivoInfoDTO> ArchivoInfoDTOs = new List<ArchivoInfoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetArchivoInfoProyCodiNom);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);
            dbProvider.AddInParameter(command, "SECCCODI", DbType.Int32, secccodi);
            dbProvider.AddInParameter(command, "ARCHNOMBRE", DbType.String, nombre);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ArchivoInfoDTO ob = new ArchivoInfoDTO();
                    ob.ArchCodi = !dr.IsDBNull(dr.GetOrdinal("ARCHCODI")) ? dr.GetInt32(dr.GetOrdinal("ARCHCODI")) : 0;
                    ob.SeccCodi = !dr.IsDBNull(dr.GetOrdinal("SECCCODI")) ? dr.GetInt32(dr.GetOrdinal("SECCCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.ArchNombre = !dr.IsDBNull(dr.GetOrdinal("ARCHNOMBRE")) ? dr.GetString(dr.GetOrdinal("ARCHNOMBRE")) : string.Empty;
                    ob.Descripcion = !dr.IsDBNull(dr.GetOrdinal("DESCRIPCION")) ? dr.GetString(dr.GetOrdinal("DESCRIPCION")) : string.Empty;
                    ob.ArchTipo = !dr.IsDBNull(dr.GetOrdinal("ARCHTIPO")) ? dr.GetString(dr.GetOrdinal("ARCHTIPO")) : string.Empty;
                    ob.ArchNombreGenerado = !dr.IsDBNull(dr.GetOrdinal("ARCHNOMBREGENERADO")) ? dr.GetString(dr.GetOrdinal("ARCHNOMBREGENERADO")) : string.Empty;
                    ob.ArchUbicacion = !dr.IsDBNull(dr.GetOrdinal("ARCHUBICACION")) ? dr.GetString(dr.GetOrdinal("ARCHUBICACION")) : string.Empty;
                    ob.ArchFechaSubida = !dr.IsDBNull(dr.GetOrdinal("ARCHFECHASUBIDA")) ? dr.GetDateTime(dr.GetOrdinal("ARCHFECHASUBIDA")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ArchivoInfoDTOs.Add(ob);
                }
            }

            return ArchivoInfoDTOs;
        }

        object ObtenerValorOrDefault(object valor, Type tipo)
        {
            DateTime fechaMinimaValida = DateTime.Now;
            if (valor == null || (valor is DateTime && (DateTime)valor == DateTime.MinValue))
            {
                if (tipo == typeof(int) || tipo == typeof(int?))
                {
                    return 0;
                }
                else if (tipo == typeof(string))
                {
                    return "";
                }
                else if (tipo == typeof(DateTime) || tipo == typeof(DateTime?))
                {
                    return fechaMinimaValida;
                }
            }
            return valor;
        }


    }


}


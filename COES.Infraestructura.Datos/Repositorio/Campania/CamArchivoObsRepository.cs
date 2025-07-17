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
    public class CamArchivoObsRepository: RepositoryBase, ICamArchObsRepository
    {

        public CamArchivoObsRepository(string strConn) : base(strConn) { }

        CamArchObsHelper Helper = new CamArchObsHelper();

        public List<ArchivoObsDTO> GetArchivoObsByObsId(int observacion, string tipo)
        {
            List<ArchivoObsDTO> ArchivoObsDTOs = new List<ArchivoObsDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetArchivoObsByObsId);
            dbProvider.AddInParameter(command, "CAM_OBSERVACIONID", DbType.String, observacion);
            dbProvider.AddInParameter(command, "TIPO", DbType.String, tipo);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ArchivoObsDTO ob = new ArchivoObsDTO();
                    ob.ArchivoId = !dr.IsDBNull(dr.GetOrdinal("CAM_ARCHIVOID")) ? dr.GetInt32(dr.GetOrdinal("CAM_ARCHIVOID")) : 0;
                    ob.ObservacionId = !dr.IsDBNull(dr.GetOrdinal("CAM_OBSERVACIONID")) ? dr.GetInt32(dr.GetOrdinal("CAM_OBSERVACIONID")) : 0;
                    ob.NombreArch = !dr.IsDBNull(dr.GetOrdinal("NOMBRE_ARCHIVO")) ? dr.GetString(dr.GetOrdinal("NOMBRE_ARCHIVO")) : string.Empty;
                    ob.NombreArchGen = !dr.IsDBNull(dr.GetOrdinal("NOMBRE_ARCHIVOGEN")) ? dr.GetString(dr.GetOrdinal("NOMBRE_ARCHIVOGEN")) : string.Empty;
                    ob.RutaArch = !dr.IsDBNull(dr.GetOrdinal("RUTA_ARCHIVO")) ? dr.GetString(dr.GetOrdinal("RUTA_ARCHIVO")) : string.Empty;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : string.Empty;
                    ob.ArchFechaSubida = !dr.IsDBNull(dr.GetOrdinal("ARCH_FECHASUBIDA")) ? dr.GetDateTime(dr.GetOrdinal("ARCH_FECHASUBIDA")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ArchivoObsDTOs.Add(ob);
                }
            }

            return ArchivoObsDTOs;
        }

        public bool SaveArchivoObs(ArchivoObsDTO archivoObsDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveArchivoObs);
            dbProvider.AddInParameter(dbCommand, "CAM_ARCHIVOID", DbType.Int32, ObtenerValorOrDefault(archivoObsDTO.ArchivoId, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "CAM_OBSERVACIONID", DbType.Int32, ObtenerValorOrDefault(archivoObsDTO.ObservacionId, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "NOMBRE_ARCHIVO", DbType.String, ObtenerValorOrDefault(archivoObsDTO.NombreArch, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "NOMBRE_ARCHIVOGEN", DbType.String, ObtenerValorOrDefault(archivoObsDTO.NombreArchGen, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "RUTA_ARCHIVO", DbType.String, ObtenerValorOrDefault(archivoObsDTO.RutaArch, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, ObtenerValorOrDefault(archivoObsDTO.Tipo, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ARCH_FECHASUBIDA", DbType.DateTime, ObtenerValorOrDefault(DateTime.Now, typeof(DateTime)));
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(archivoObsDTO.UsuarioCreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteArchivoObsById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteArchivoObsById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastArchivoObsId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastArchivoObsId);
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

        public ArchivoObsDTO GetArchivoObsById(int id)
        {
            ArchivoObsDTO ob = new ArchivoObsDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetArchivoObsById);
            dbProvider.AddInParameter(commandHoja, "CAM_ARCHIVOID", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.ArchivoId = !dr.IsDBNull(dr.GetOrdinal("CAM_ARCHIVOID")) ? dr.GetInt32(dr.GetOrdinal("CAM_ARCHIVOID")) : 0;
                    ob.ObservacionId = !dr.IsDBNull(dr.GetOrdinal("CAM_OBSERVACIONID")) ? dr.GetInt32(dr.GetOrdinal("CAM_OBSERVACIONID")) : 0;
                    ob.NombreArch = !dr.IsDBNull(dr.GetOrdinal("NOMBRE_ARCHIVO")) ? dr.GetString(dr.GetOrdinal("NOMBRE_ARCHIVO")) : string.Empty;
                    ob.NombreArchGen = !dr.IsDBNull(dr.GetOrdinal("NOMBRE_ARCHIVOGEN")) ? dr.GetString(dr.GetOrdinal("NOMBRE_ARCHIVOGEN")) : string.Empty;
                    ob.RutaArch = !dr.IsDBNull(dr.GetOrdinal("RUTA_ARCHIVO")) ? dr.GetString(dr.GetOrdinal("RUTA_ARCHIVO")) : string.Empty;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : string.Empty;
                    ob.ArchFechaSubida = !dr.IsDBNull(dr.GetOrdinal("ARCH_FECHASUBIDA")) ? dr.GetDateTime(dr.GetOrdinal("ARCH_FECHASUBIDA")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public ArchivoObsDTO GetArchivoObsNombreArchivo(string nombre)
        {
            ArchivoObsDTO ob = new ArchivoObsDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetArchivoObsNombreGenerado);
            dbProvider.AddInParameter(commandHoja, "NOMBRE_ARCHIVOGEN", DbType.String, nombre);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.ArchivoId = !dr.IsDBNull(dr.GetOrdinal("CAM_ARCHIVOID")) ? dr.GetInt32(dr.GetOrdinal("CAM_ARCHIVOID")) : 0;
                    ob.ObservacionId = !dr.IsDBNull(dr.GetOrdinal("CAM_OBSERVACIONID")) ? dr.GetInt32(dr.GetOrdinal("CAM_OBSERVACIONID")) : 0;
                    ob.NombreArch = !dr.IsDBNull(dr.GetOrdinal("NOMBRE_ARCHIVO")) ? dr.GetString(dr.GetOrdinal("NOMBRE_ARCHIVO")) : string.Empty;
                    ob.NombreArchGen = !dr.IsDBNull(dr.GetOrdinal("NOMBRE_ARCHIVOGEN")) ? dr.GetString(dr.GetOrdinal("NOMBRE_ARCHIVOGEN")) : string.Empty;
                    ob.RutaArch = !dr.IsDBNull(dr.GetOrdinal("RUTA_ARCHIVO")) ? dr.GetString(dr.GetOrdinal("RUTA_ARCHIVO")) : string.Empty;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : string.Empty;
                    ob.ArchFechaSubida = !dr.IsDBNull(dr.GetOrdinal("ARCH_FECHASUBIDA")) ? dr.GetDateTime(dr.GetOrdinal("ARCH_FECHASUBIDA")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public bool UpdateArchivoObs(ArchivoObsDTO archivoObsDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateArchivoObs);
            dbProvider.AddInParameter(dbCommand, "CAM_OBSERVACIONID", DbType.Int32, ObtenerValorOrDefault(archivoObsDTO.ObservacionId, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "NOMBRE_ARCHIVO", DbType.String, ObtenerValorOrDefault(archivoObsDTO.NombreArch, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "NOMBRE_ARCHIVOGEN", DbType.String, ObtenerValorOrDefault(archivoObsDTO.NombreArchGen, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "RUTA_ARCHIVO", DbType.String, ObtenerValorOrDefault(archivoObsDTO.RutaArch, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, ObtenerValorOrDefault(archivoObsDTO.Tipo, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ARCH_FECHASUBIDA", DbType.DateTime, ObtenerValorOrDefault(archivoObsDTO.ArchFechaSubida, typeof(DateTime)));
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, archivoObsDTO.UsuarioModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "CAM_ARCHIVOID", DbType.Int32, ObtenerValorOrDefault(archivoObsDTO.ArchivoId, typeof(int)));
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public List<ArchivoObsDTO> GetArchivoObsProyCodiNom(int observacionId, string tipo, string nombre)
        {
            List<ArchivoObsDTO> ArchivoObsDTOs = new List<ArchivoObsDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetArchivoObsProyCodiNom);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(command, "CAM_OBSERVACIONID", DbType.Int32, observacionId);
            dbProvider.AddInParameter(command, "TIPO", DbType.String, tipo);
            dbProvider.AddInParameter(command, "NOMBRE_ARCHIVO", DbType.String, nombre);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ArchivoObsDTO ob = new ArchivoObsDTO();
                    ob.ArchivoId = !dr.IsDBNull(dr.GetOrdinal("CAM_ARCHIVOID")) ? dr.GetInt32(dr.GetOrdinal("CAM_ARCHIVOID")) : 0;
                    ob.ObservacionId = !dr.IsDBNull(dr.GetOrdinal("CAM_OBSERVACIONID")) ? dr.GetInt32(dr.GetOrdinal("CAM_OBSERVACIONID")) : 0;
                    ob.NombreArch = !dr.IsDBNull(dr.GetOrdinal("NOMBRE_ARCHIVO")) ? dr.GetString(dr.GetOrdinal("NOMBRE_ARCHIVO")) : string.Empty;
                    ob.NombreArchGen = !dr.IsDBNull(dr.GetOrdinal("NOMBRE_ARCHIVOGEN")) ? dr.GetString(dr.GetOrdinal("NOMBRE_ARCHIVOGEN")) : string.Empty;
                    ob.RutaArch = !dr.IsDBNull(dr.GetOrdinal("RUTA_ARCHIVO")) ? dr.GetString(dr.GetOrdinal("RUTA_ARCHIVO")) : string.Empty;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : string.Empty;
                    ob.ArchFechaSubida = !dr.IsDBNull(dr.GetOrdinal("ARCH_FECHASUBIDA")) ? dr.GetDateTime(dr.GetOrdinal("ARCH_FECHASUBIDA")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ArchivoObsDTOs.Add(ob);
                }
            }

            return ArchivoObsDTOs;
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


using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.Interfaces.Campania;
using COES.Framework.Base.Tools;
using COES.Infraestructura.Datos.Helper;
using COES.Infraestructura.Datos.Helper.Campania;
using COES.Infraestructura.Datos.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamSubestFicha1Repository : RepositoryBase, ICamSubestFicha1Repository
    {
        public CamSubestFicha1Repository(string strConn) : base(strConn) { }

       CamSubestFicha1Helper Helper = new CamSubestFicha1Helper();

        public List<SubestFicha1DTO> GetSubestFicha1(int idcodi)
        {
            List<SubestFicha1DTO> subestFicha1DTOs = new List<SubestFicha1DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetSubestFicha1);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, idcodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SubestFicha1DTO dto = new SubestFicha1DTO();
                    dto.SubestFicha1Codi = !dr.IsDBNull(dr.GetOrdinal("SUBESTFICHA1CODI")) ? dr.GetInt32(dr.GetOrdinal("SUBESTFICHA1CODI")) : 0;
                    dto.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    dto.NombreSubestacion = !dr.IsDBNull(dr.GetOrdinal("NOMBRESUBESTACION")) ? dr.GetString(dr.GetOrdinal("NOMBRESUBESTACION")) : "";
                    dto.TipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPOPROYECTO")) ? dr.GetString(dr.GetOrdinal("TIPOPROYECTO")) : "";
                    dto.FechaPuestaServicio = !dr.IsDBNull(dr.GetOrdinal("FECHAPUESTASERVICIO")) ? dr.GetString(dr.GetOrdinal("FECHAPUESTASERVICIO")) : null;
                    dto.SistemaBarras = !dr.IsDBNull(dr.GetOrdinal("SISTEMABARRAS")) ? dr.GetString(dr.GetOrdinal("SISTEMABARRAS")) : null;
                    dto.OtroSistemaBarras = !dr.IsDBNull(dr.GetOrdinal("OTROSISTEMABARRAS")) ? dr.GetString(dr.GetOrdinal("OTROSISTEMABARRAS")) : null;
                    dto.NumTrafo = !dr.IsDBNull(dr.GetOrdinal("NUMTRAFOS")) ? dr.GetInt32(dr.GetOrdinal("NUMTRAFOS")) : 0;
                    dto.NumEquipos = !dr.IsDBNull(dr.GetOrdinal("NUMEQUIPOS")) ? dr.GetInt32(dr.GetOrdinal("NUMEQUIPOS")) : 0;
                    dto.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null;
                    dto.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.Now;
                    dto.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null;
                    subestFicha1DTOs.Add(dto);
                }
            }

            return subestFicha1DTOs;
        }

        public bool SaveSubestFicha1(SubestFicha1DTO subestFicha1DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveSubestFicha1);
            dbProvider.AddInParameter(dbCommand, "SUBESTFICHA1CODI", DbType.Int32, subestFicha1DTO.SubestFicha1Codi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, subestFicha1DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "NOMBRESUBESTACION", DbType.String, subestFicha1DTO.NombreSubestacion);
            dbProvider.AddInParameter(dbCommand, "TIPOPROYECTO", DbType.String, subestFicha1DTO.TipoProyecto);
            dbProvider.AddInParameter(dbCommand, "FECHAPUESTASERVICIO", DbType.String, subestFicha1DTO.FechaPuestaServicio);
            dbProvider.AddInParameter(dbCommand, "EMPRESAPROPIETARIA", DbType.String, subestFicha1DTO.EmpresaPropietaria);
            dbProvider.AddInParameter(dbCommand, "SISTEMABARRAS", DbType.String, subestFicha1DTO.SistemaBarras);
            dbProvider.AddInParameter(dbCommand, "OTROSISTEMABARRAS", DbType.String, subestFicha1DTO.OtroSistemaBarras);
            dbProvider.AddInParameter(dbCommand, "NUMTRAFOS", DbType.Int32, subestFicha1DTO.NumTrafo);
            dbProvider.AddInParameter(dbCommand, "NUMEQUIPOS", DbType.Int32, subestFicha1DTO.NumEquipos);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, subestFicha1DTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteSubestFicha1ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteSubestFicha1ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastSubestFicha1Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastSubestFicha1Id);
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

        public SubestFicha1DTO GetSubestFicha1ById(int id)
        {
            SubestFicha1DTO dto = new SubestFicha1DTO();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetSubestFicha1ById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    dto.SubestFicha1Codi = !dr.IsDBNull(dr.GetOrdinal("SUBESTFICHA1CODI")) ? dr.GetInt32(dr.GetOrdinal("SUBESTFICHA1CODI")) : 0;
                    dto.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    dto.NombreSubestacion = !dr.IsDBNull(dr.GetOrdinal("NOMBRESUBESTACION")) ? dr.GetString(dr.GetOrdinal("NOMBRESUBESTACION")) : "";
                    dto.TipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPOPROYECTO")) ? dr.GetString(dr.GetOrdinal("TIPOPROYECTO")) : "";
                    dto.FechaPuestaServicio = !dr.IsDBNull(dr.GetOrdinal("FECHAPUESTASERVICIO")) ? dr.GetString(dr.GetOrdinal("FECHAPUESTASERVICIO")) : null;
                    dto.EmpresaPropietaria = !dr.IsDBNull(dr.GetOrdinal("EMPRESAPROPIETARIA")) ? dr.GetString(dr.GetOrdinal("EMPRESAPROPIETARIA")) : null;
                    dto.SistemaBarras = !dr.IsDBNull(dr.GetOrdinal("SISTEMABARRAS")) ? dr.GetString(dr.GetOrdinal("SISTEMABARRAS")) : null;
                    dto.OtroSistemaBarras = !dr.IsDBNull(dr.GetOrdinal("OTROSISTEMABARRAS")) ? dr.GetString(dr.GetOrdinal("OTROSISTEMABARRAS")) : null;
                    dto.NumTrafo = !dr.IsDBNull(dr.GetOrdinal("NUMTRAFOS")) ? dr.GetInt32(dr.GetOrdinal("NUMTRAFOS")) : 0;
                    dto.NumEquipos = !dr.IsDBNull(dr.GetOrdinal("NUMEQUIPOS")) ? dr.GetInt32(dr.GetOrdinal("NUMEQUIPOS")) : 0;
                    dto.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null;
                    dto.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.Now;
                    dto.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null;
                }
            }

            return dto;
        }
    }
}


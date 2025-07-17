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
    public class CamRegHojaEolADetRepository : RepositoryBase, ICamRegHojaEolADetRepository
    {
        public CamRegHojaEolADetRepository(string strConn) : base(strConn) { }

        CamRegHojaEolADetHelper Helper = new CamRegHojaEolADetHelper();

        public List<RegHojaEolADetDTO> GetRegHojaEolADetCodi(int idcodi)
        {
            List<RegHojaEolADetDTO> regHojaEolADetDTOs = new List<RegHojaEolADetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetRegHojaEolADetCodi);
            dbProvider.AddInParameter(command, "CENTRALACODI", DbType.Int32, idcodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RegHojaEolADetDTO dto = new RegHojaEolADetDTO();
                    dto.CentralADetCodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALADETCODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALADETCODI")) : 0;
                    dto.CentralACodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALACODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALACODI")) : 0;
                    dto.Speed = !dr.IsDBNull(dr.GetOrdinal("SPEED")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("SPEED")) : null;
                    dto.Acciona = !dr.IsDBNull(dr.GetOrdinal("ACCIONA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ACCIONA")) : null;
                    dto.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null;
                    dto.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? (DateTime?)dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : null;
                    dto.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null;
                    regHojaEolADetDTOs.Add(dto);
                }
            }

            return regHojaEolADetDTOs;
        }

        public bool SaveRegHojaEolADet(RegHojaEolADetDTO regHojaEolADetDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveRegHojaEolADet);
            dbProvider.AddInParameter(dbCommand, "CENTRALADETCODI", DbType.Int32, regHojaEolADetDTO.CentralADetCodi);
            dbProvider.AddInParameter(dbCommand, "CENTRALACODI", DbType.Int32, regHojaEolADetDTO.CentralACodi);
            dbProvider.AddInParameter(dbCommand, "SPEED", DbType.Decimal, regHojaEolADetDTO.Speed);
            dbProvider.AddInParameter(dbCommand, "ACCIONA", DbType.Decimal, regHojaEolADetDTO.Acciona);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, regHojaEolADetDTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, regHojaEolADetDTO.FecCreacion);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, regHojaEolADetDTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteRegHojaEolADetById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteRegHojaEolADetById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastRegHojaEolADetId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastRegHojaEolADetId);
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

        public RegHojaEolADetDTO GetRegHojaEolADetById(int id)
        {
            RegHojaEolADetDTO dto = new RegHojaEolADetDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetRegHojaEolADetById);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    dto.CentralADetCodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALADETCODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALADETCODI")) : 0;
                    dto.CentralACodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALACODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALACODI")) : 0;
                    dto.Speed = !dr.IsDBNull(dr.GetOrdinal("SPEED")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("SPEED")) : null;
                    dto.Acciona = !dr.IsDBNull(dr.GetOrdinal("ACCIONA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ACCIONA")) : null;
                    dto.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null;
                    dto.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? (DateTime?)dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : null;
                    dto.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null;
                }
            }

            return dto;
        }
    }

}

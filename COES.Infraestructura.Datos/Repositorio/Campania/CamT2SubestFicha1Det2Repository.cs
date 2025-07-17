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
    public class CamT2SubestFicha1Det2Repository : RepositoryBase, ICamT2SubestFicha1Det2Repository
    {
        public CamT2SubestFicha1Det2Repository(string strConn) : base(strConn) { }

        CamT2SubestFicha1Det2Helper Helper = new CamT2SubestFicha1Det2Helper();

        public List<T2SubestFicha1Det2DTO> GetT2SubestFicha1Det2(int idcodi)
        {
            List<T2SubestFicha1Det2DTO> subestFicha1DTOs = new List<T2SubestFicha1Det2DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetT2SubestFicha1Det2);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, idcodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    T2SubestFicha1Det2DTO dto = new T2SubestFicha1Det2DTO();
                    dto.SubestFicha1Det2Codi = !dr.IsDBNull(dr.GetOrdinal("SUBESTFICHA1DET2CODI")) ? dr.GetInt32(dr.GetOrdinal("SUBESTFICHA1DET2CODI")) : 0;
                    dto.SubestFicha1Codi = !dr.IsDBNull(dr.GetOrdinal("SUBESTFICHA1CODI")) ? dr.GetInt32(dr.GetOrdinal("SUBESTFICHA1CODI")) : 0;
                    dto.DataCatCodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    dto.NumTrafo = !dr.IsDBNull(dr.GetOrdinal("NUMTRAFO")) ? dr.GetString(dr.GetOrdinal("NUMTRAFO")) : "";
                    dto.ValorTrafo = !dr.IsDBNull(dr.GetOrdinal("VALORTRAFO")) ? dr.GetString(dr.GetOrdinal("VALORTRAFO")) : "";
                    dto.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null;
                    dto.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.Now;
                    dto.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null;
                    subestFicha1DTOs.Add(dto);
                }
            }

            return subestFicha1DTOs;
        }

        public bool SaveT2SubestFicha1Det2(T2SubestFicha1Det2DTO subestFicha1DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveT2SubestFicha1Det2);
            dbProvider.AddInParameter(dbCommand, "SUBESTFICHA1DET2CODI", DbType.Int32, subestFicha1DTO.SubestFicha1Det2Codi);
            dbProvider.AddInParameter(dbCommand, "SUBESTFICHA1CODI", DbType.Int32, subestFicha1DTO.SubestFicha1Codi);
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.String, subestFicha1DTO.DataCatCodi);
            dbProvider.AddInParameter(dbCommand, "NUMTRAFO", DbType.String, subestFicha1DTO.NumTrafo);
            dbProvider.AddInParameter(dbCommand, "VALORTRAFO", DbType.String, subestFicha1DTO.ValorTrafo);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, subestFicha1DTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, subestFicha1DTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteT2SubestFicha1Det2ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteT2SubestFicha1Det2ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastT2SubestFicha1Det2Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastT2SubestFicha1Det2Id);
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

        public List<T2SubestFicha1Det2DTO> GetT2SubestFicha1Det2ById(int id)
        {
            List<T2SubestFicha1Det2DTO> dtos = new List<T2SubestFicha1Det2DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetT2SubestFicha1Det2ById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    T2SubestFicha1Det2DTO dto = new T2SubestFicha1Det2DTO();
                    dto.SubestFicha1Det2Codi = !dr.IsDBNull(dr.GetOrdinal("SUBESTFICHA1DET2CODI")) ? dr.GetInt32(dr.GetOrdinal("SUBESTFICHA1DET2CODI")) : 0;
                    dto.SubestFicha1Codi = !dr.IsDBNull(dr.GetOrdinal("SUBESTFICHA1CODI")) ? dr.GetInt32(dr.GetOrdinal("SUBESTFICHA1CODI")) : 0;
                    dto.DataCatCodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    dto.NumTrafo = !dr.IsDBNull(dr.GetOrdinal("NUMTRAFO")) ? dr.GetString(dr.GetOrdinal("NUMTRAFO")) : "";
                    dto.ValorTrafo = !dr.IsDBNull(dr.GetOrdinal("VALORTRAFO")) ? dr.GetString(dr.GetOrdinal("VALORTRAFO")) : "";
                    dto.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null;
                    dto.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.Now;
                    dto.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null;
                    dtos.Add(dto);

                }
            }
            return dtos;
        }
    }
}


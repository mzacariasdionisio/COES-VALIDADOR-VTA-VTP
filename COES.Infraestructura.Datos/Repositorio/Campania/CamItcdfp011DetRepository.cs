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
    public class CamItcdfp011DetRepository : RepositoryBase, ICamItcdfp011DetRepository
    {
        public CamItcdfp011DetRepository(string strConn) : base(strConn) { }

        CamItcdfp011DetHelper Helper = new CamItcdfp011DetHelper();

        public List<Itcdfp011DetDTO> GetItcdfp011DetCodi(int itcdfp011Codi)
        {
            List<Itcdfp011DetDTO> itcdfp011DetDTOs = new List<Itcdfp011DetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdfp011DetCodi);
            dbProvider.AddInParameter(command, "ITCDFP011CODI", DbType.Int32, itcdfp011Codi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    Itcdfp011DetDTO ob = new Itcdfp011DetDTO
                    {
                        Itcdfp011DetCodi = !dr.IsDBNull(dr.GetOrdinal("ITCDFP011DETCODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDFP011DETCODI")) : 0,
                        Itcdfp011Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDFP011CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDFP011CODI")) : 0,
                        FechaHora = !dr.IsDBNull(dr.GetOrdinal("FECHAHORA")) ? dr.GetString(dr.GetOrdinal("FECHAHORA")) : null,
                        BarraNro = !dr.IsDBNull(dr.GetOrdinal("BARRANRO")) ? dr.GetInt32(dr.GetOrdinal("BARRANRO")) : 0,
                        Kwval = !dr.IsDBNull(dr.GetOrdinal("KWVAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("KWVAL")) : null,
                        Kvarval = !dr.IsDBNull(dr.GetOrdinal("KVARVAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("KVARVAL")) : null,
                        UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null,
                        FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue,
                        UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : null,
                        FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue,
                        IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null
                    };
                    itcdfp011DetDTOs.Add(ob);
                }
            }

            return itcdfp011DetDTOs;
        }

       public bool SaveItcdfp011Det(List<Itcdfp011DetDTO> itcdfp011DetDTOs, int itcdfp011Codi, string usuCreacion)
        {
            try
            {
                int batchSize = 1000;
                
                StringBuilder insertQueryBase = new StringBuilder();
                insertQueryBase.Append(@"
                    INSERT INTO CAM_ITCDFP011DET (
                        ITCDFP011DETCODI,
                        ITCDFP011CODI, 
                        FECHAHORA,         
                        BARRANRO,          
                        KWVAL,             
                        KVARVAL,           
                        USU_CREACION,      
                        FEC_CREACION,      
                        IND_DEL
                    ) ");
            
                int totalRecords = itcdfp011DetDTOs.Count;
                int index = 0;

                while (index < totalRecords)
                {

                    StringBuilder insertQuery = new StringBuilder(insertQueryBase.ToString());

                    int currentBatchSize = Math.Min(batchSize, totalRecords - index);
                    
                    bool isFirstRow = true;
                    for (int i = index; i < index + currentBatchSize; i++)
                    {
                        var itcdfp011DetDTO = itcdfp011DetDTOs[i];
                        
                        if (!isFirstRow)
                        {
                            insertQuery.Append(" UNION ALL ");
                        }
                        isFirstRow = false;

                        string barraNro = string.IsNullOrEmpty(itcdfp011DetDTO.BarraNro.ToString()) ? "NULL" : itcdfp011DetDTO.BarraNro.ToString();
                        string kwval = string.IsNullOrEmpty(itcdfp011DetDTO.Kwval?.ToString()) ? "NULL" : itcdfp011DetDTO.Kwval.ToString();
                        string kvarval = string.IsNullOrEmpty(itcdfp011DetDTO.Kvarval?.ToString()) ? "NULL" : itcdfp011DetDTO.Kvarval.ToString();

                        insertQuery.Append($@"
                            SELECT 
                                NVL((SELECT MAX(ITCDFP011DETCODI) FROM CAM_ITCDFP011DET), 0) + {i + 1}, 
                                {itcdfp011Codi}, 
                                '{itcdfp011DetDTO.FechaHora}', 
                                {barraNro}, 
                                {kwval}, 
                                {kvarval},  
                                '{usuCreacion}', 
                                TO_DATE('{DateTime.Now:yyyy-MM-dd HH:mm:ss}', 'YYYY-MM-DD HH24:MI:SS'), 
                                '{Constantes.IndDel}' FROM dual");
                    }

                    var query = insertQuery.ToString();
                    DbCommand dbCommand = dbProvider.GetSqlStringCommand(query);
                    dbProvider.ExecuteNonQuery(dbCommand);

                    index += currentBatchSize;
                }

                return true;
            }
            catch (Exception ex)
            {
                // Si ocurre un error, hacer rollback o registrar el error
                throw new Exception("Error al guardar los registros", ex);
            }
        }


        public bool DeleteItcdfp011DetById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteItcdfp011DetById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "ITCDFP011DETCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastItcdfp011DetId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastItcdfp011DetId);
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

        public List<Itcdfp011DetDTO> GetItcdfp011DetById(int id)
        {
            List<Itcdfp011DetDTO> itcdfp011DetDTOs = new List<Itcdfp011DetDTO>();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdfp011DetById);
            dbProvider.AddInParameter(commandHoja, "ITCDFP011DETCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    Itcdfp011DetDTO ob = new Itcdfp011DetDTO
                    {
                        Itcdfp011DetCodi = !dr.IsDBNull(dr.GetOrdinal("ITCDFP011DETCODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDFP011DETCODI")) : 0,
                        Itcdfp011Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDFP011CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDFP011CODI")) : 0,
                        FechaHora = !dr.IsDBNull(dr.GetOrdinal("FECHAHORA")) ? dr.GetString(dr.GetOrdinal("FECHAHORA")) : null,
                        BarraNro = !dr.IsDBNull(dr.GetOrdinal("BARRANRO")) ? dr.GetInt32(dr.GetOrdinal("BARRANRO")) : 0,
                        Kwval = !dr.IsDBNull(dr.GetOrdinal("KWVAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("KWVAL")) : null,
                        Kvarval = !dr.IsDBNull(dr.GetOrdinal("KVARVAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("KVARVAL")) : null,
                        UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null,
                        FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue,
                        UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : null,
                        FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue,
                        IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null
                        
                    };
                    itcdfp011DetDTOs.Add(ob);
                }
            }

            return itcdfp011DetDTOs;
        }

        public bool UpdateItcdfp011Det(Itcdfp011DetDTO itcdfp011DetDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateItcdfp011Det);
            dbProvider.AddInParameter(dbCommand, "ITCDFP011DETCODI", DbType.Int32, itcdfp011DetDTO.Itcdfp011DetCodi);
            dbProvider.AddInParameter(dbCommand, "ITCDFP011CODI", DbType.Int32, itcdfp011DetDTO.Itcdfp011Codi);
            dbProvider.AddInParameter(dbCommand, "FECHAHORA", DbType.String, itcdfp011DetDTO.FechaHora);
            dbProvider.AddInParameter(dbCommand, "BARRANRO", DbType.Int32, itcdfp011DetDTO.BarraNro);
            dbProvider.AddInParameter(dbCommand, "KWVAL", DbType.Decimal, itcdfp011DetDTO.Kwval);
            dbProvider.AddInParameter(dbCommand, "KVARVAL", DbType.Decimal, itcdfp011DetDTO.Kvarval);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, itcdfp011DetDTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, itcdfp011DetDTO.FecModificacion);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

         public List<Itcdfp011DetDTO> GetItcdfp011DetByIdPag(int id, int offset, int pageSize)
        {
            List<Itcdfp011DetDTO> itcdfp011DetDTOs = new List<Itcdfp011DetDTO>();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdfp011DetByIdPag);
            dbProvider.AddInParameter(commandHoja, "ITCDFP011DETCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(commandHoja, "OFFSET", DbType.Int32, offset);
            dbProvider.AddInParameter(commandHoja, "PAGESIZE", DbType.Int32, pageSize);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    Itcdfp011DetDTO ob = new Itcdfp011DetDTO
                    {
                        Itcdfp011DetCodi = !dr.IsDBNull(dr.GetOrdinal("ITCDFP011DETCODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDFP011DETCODI")) : 0,
                        Itcdfp011Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDFP011CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDFP011CODI")) : 0,
                        FechaHora = !dr.IsDBNull(dr.GetOrdinal("FECHAHORA")) ? dr.GetString(dr.GetOrdinal("FECHAHORA")) : null,
                        BarraNro = !dr.IsDBNull(dr.GetOrdinal("BARRANRO")) ? dr.GetInt32(dr.GetOrdinal("BARRANRO")) : 0,
                        Kwval = !dr.IsDBNull(dr.GetOrdinal("KWVAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("KWVAL")) : null,
                        Kvarval = !dr.IsDBNull(dr.GetOrdinal("KVARVAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("KVARVAL")) : null,
                        UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null,
                        FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue,
                        UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : null,
                        FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue,
                        IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null
                        
                    };
                    itcdfp011DetDTOs.Add(ob);
                }
            }

            return itcdfp011DetDTOs;
        }

        public bool GetCloneItcdfp011DetById(int id, int newId)
        {
            string query = $@"
            INSERT INTO CAM_ITCDFP011DET (
                ITCDFP011DETCODI, 
                ITCDFP011CODI,
                FECHAHORA,         
                BARRANRO,          
                KWVAL,             
                KVARVAL,           
                USU_CREACION,      
                FEC_CREACION,      
                IND_DEL
            )
            SELECT 
                NVL(
                    (SELECT MAX(ITCDFP011DETCODI) FROM CAM_ITCDFP011DET), 0
                ) + ROWNUM,
                {newId},
                FECHAHORA,   
                BARRANRO,    
                KWVAL,       
                KVARVAL,     
                USU_CREACION,
                FEC_CREACION,
                IND_DEL
            FROM (SELECT 
                ITCDFP011DETCODI,
                ITCDFP011CODI,
                FECHAHORA,   
                BARRANRO,    
                KWVAL,       
                KVARVAL,     
                USU_CREACION,
                FEC_CREACION,
                IND_DEL
            FROM CAM_ITCDFP011DET
            WHERE ITCDFP011CODI = :ID
            AND IND_DEL = :IND_DEL
            ORDER BY ITCDFP011DETCODI ASC)";

            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(commandHoja, "ID", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            return true;
        }

    }
}
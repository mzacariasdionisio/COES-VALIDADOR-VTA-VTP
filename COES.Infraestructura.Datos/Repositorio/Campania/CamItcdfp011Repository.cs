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
    public class CamItcdfp011Repository : RepositoryBase, ICamItcdfp011Repository
    {
        public CamItcdfp011Repository(string strConn) : base(strConn) { }

        CamItcdfp011Helper Helper = new CamItcdfp011Helper();

        public List<Itcdfp011DTO> GetItcdfp011Codi(int proyCodi)
        {
            List<Itcdfp011DTO> itcdfp011DTOs = new List<Itcdfp011DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdfp011Codi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    Itcdfp011DTO ob = new Itcdfp011DTO
                    {
                        Itcdfp011Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDFP011CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDFP011CODI")) : 0,
                        ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0,
                        FechaHora = !dr.IsDBNull(dr.GetOrdinal("FECHAHORA")) ? dr.GetString(dr.GetOrdinal("FECHAHORA")) : null,
                        NroBarras = !dr.IsDBNull(dr.GetOrdinal("NROBARRAS")) ? dr.GetInt32(dr.GetOrdinal("NROBARRAS")) : 0,
                        UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null,
                        FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue,
                        UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : null,
                        FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue,
                        IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null
                    };
                    itcdfp011DTOs.Add(ob);
                }
            }

            return itcdfp011DTOs;
        }

        public bool SaveItcdfp011(Itcdfp011DTO itcdfp011DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveItcdfp011);
            dbProvider.AddInParameter(dbCommand, "ITCDFP011CODI", DbType.Int32, itcdfp011DTO.Itcdfp011Codi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcdfp011DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "FECHAHORA", DbType.String, itcdfp011DTO.FechaHora);
            dbProvider.AddInParameter(dbCommand, "NROBARRAS", DbType.Int32, itcdfp011DTO.NroBarras);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, itcdfp011DTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, itcdfp011DTO.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteItcdfp011ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteItcdfp011ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "ITCDFP011CODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastItcdfp011Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastItcdfp011Id);
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

        public Itcdfp011DTO GetItcdfp011ById(int id)
        {
            Itcdfp011DTO ob = new Itcdfp011DTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdfp011ById);
            dbProvider.AddInParameter(commandHoja, "ITCDFP011CODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.Itcdfp011Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDFP011CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDFP011CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.FechaHora = !dr.IsDBNull(dr.GetOrdinal("FECHAHORA")) ? dr.GetString(dr.GetOrdinal("FECHAHORA")) : null;
                    ob.NroBarras = !dr.IsDBNull(dr.GetOrdinal("NROBARRAS")) ? dr.GetInt32(dr.GetOrdinal("NROBARRAS")) : 0;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null;
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : null;
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null;
                }
            }

            return ob;
        }

        public bool UpdateItcdfp011(Itcdfp011DTO itcdfp011DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateItcdfp011);
            dbProvider.AddInParameter(dbCommand, "ITCDFP011CODI", DbType.Int32, itcdfp011DTO.Itcdfp011Codi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcdfp011DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "FECHAHORA", DbType.String, itcdfp011DTO.FechaHora);
            dbProvider.AddInParameter(dbCommand, "NROBARRAS", DbType.Int32, itcdfp011DTO.NroBarras);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, itcdfp011DTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, itcdfp011DTO.FecModificacion);
            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }




        public List<Itcdfp011DTO> GetItcdfp011ByFilter(string plancodi, string empresa, string estado)
        {
            List<Itcdfp011DTO> result = new List<Itcdfp011DTO>();
            Dictionary<int, Itcdfp011DTO> cabeceraMap = new Dictionary<int, Itcdfp011DTO>();

            string query = $@"
        SELECT 
            CAB.ITCDFP011CODI,
            CAB.PROYCODI,
            CAB.NROBARRAS,
            CAB.USU_CREACION,
            CAB.FEC_CREACION,
            CAB.USU_MODIFICACION,
            CAB.FEC_MODIFICACION,
            CAB.IND_DEL,

            DET.ITCDFP011DETCODI,
            DET.BARRANRO,
            DET.KWVAL,
            DET.KVARVAL,
            DET.FECHAHORA,

            TR.EMPRESANOM,
            TR.AREADEMANDA

        FROM CAM_ITCDFP011 CAB
        JOIN CAM_ITCDFP011DET DET ON DET.ITCDFP011CODI = CAB.ITCDFP011CODI
        INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CAB.PROYCODI
        INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
        WHERE TR.PERICODI IN ({plancodi})
          AND PL.CODEMPRESA IN ({empresa})
          AND CAB.IND_DEL = 0
          AND PL.PLANESTADO = :estado
        ORDER BY TR.PERICODI, CAB.PROYCODI, PL.CODEMPRESA, CAB.ITCDFP011CODI ASC
    ";

            DbCommand cmd = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(cmd, ":estado", DbType.String, estado);

            using (IDataReader dr = dbProvider.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    int cabId = Convert.ToInt32(dr["ITCDFP011CODI"]);

                    // Si no se ha creado la cabecera, la agregamos
                    if (!cabeceraMap.TryGetValue(cabId, out Itcdfp011DTO current))
                    {
                        current = new Itcdfp011DTO
                        {
                            Itcdfp011Codi = cabId,
                            ProyCodi = Convert.ToInt32(dr["PROYCODI"]),
                            NroBarras = Convert.ToInt32(dr["NROBARRAS"]),
                            UsuCreacion = dr["USU_CREACION"].ToString(),
                            FecCreacion = Convert.ToDateTime(dr["FEC_CREACION"]),
                            UsuModificacion = dr["USU_MODIFICACION"]?.ToString(),
                            FecModificacion = dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? DateTime.MinValue : Convert.ToDateTime(dr["FEC_MODIFICACION"]),
                            IndDel = dr["IND_DEL"].ToString(),
                            AreaDemanda = dr["AREADEMANDA"].ToString(),
                            Empresa = dr["EMPRESANOM"].ToString(),
                            ListItcdf011Det = new List<Itcdfp011DetDTO>()
                        };

                        cabeceraMap[cabId] = current;
                        result.Add(current);
                    }

                    // Agregar detalle
                    current.ListItcdf011Det.Add(new Itcdfp011DetDTO
                    {
                        Itcdfp011DetCodi = dr.IsDBNull(dr.GetOrdinal("ITCDFP011DETCODI")) ? 0 : Convert.ToInt32(dr["ITCDFP011DETCODI"]),
                        BarraNro = dr.IsDBNull(dr.GetOrdinal("BARRANRO")) ? 0 : Convert.ToInt32(dr["BARRANRO"]),
                        Kwval = dr.IsDBNull(dr.GetOrdinal("KWVAL")) ? (decimal?)null : Convert.ToDecimal(dr["KWVAL"]),
                        Kvarval = dr.IsDBNull(dr.GetOrdinal("KVARVAL")) ? (decimal?)null : Convert.ToDecimal(dr["KVARVAL"]),
                        FechaHora = dr.IsDBNull(dr.GetOrdinal("FECHAHORA")) ? null : dr["FECHAHORA"].ToString()
                    });
                }
            }

            return result;
        }

    }

}
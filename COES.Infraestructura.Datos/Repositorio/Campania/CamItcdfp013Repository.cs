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
    public class CamItcdfp013Repository : RepositoryBase, ICamItcdfp013Repository
    {
        public CamItcdfp013Repository(string strConn) : base(strConn) { }

        CamItcdfp013Helper Helper = new CamItcdfp013Helper();

        public List<Itcdfp013DTO> GetItcdfp013Codi(int proyCodi)
        {
            List<Itcdfp013DTO> itcdfp013DTOs = new List<Itcdfp013DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdfp013Codi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    Itcdfp013DTO ob = new Itcdfp013DTO
                    {
                        Itcdfp013Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDFP013CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDFP013CODI")) : 0,
                        ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0,
                        NombreCliente = !dr.IsDBNull(dr.GetOrdinal("NOMBRECLIENTE")) ? dr.GetString(dr.GetOrdinal("NOMBRECLIENTE")) : null,
                        TipoCarga = !dr.IsDBNull(dr.GetOrdinal("TIPOCARGA")) ? dr.GetString(dr.GetOrdinal("TIPOCARGA")) : null,
                        UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null,
                        FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue,
                        UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : null,
                        FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue,
                        IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null
                    };
                    itcdfp013DTOs.Add(ob);
                }
            }

            return itcdfp013DTOs;
        }

        public bool SaveItcdfp013(Itcdfp013DTO itcdfp013DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveItcdfp013);
            dbProvider.AddInParameter(dbCommand, "ITCDFP013CODI", DbType.Int32, itcdfp013DTO.Itcdfp013Codi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcdfp013DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "NOMBRECLIENTE", DbType.String, itcdfp013DTO.NombreCliente);
            dbProvider.AddInParameter(dbCommand, "TIPOCARGA", DbType.String, itcdfp013DTO.TipoCarga);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, itcdfp013DTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, itcdfp013DTO.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteItcdfp013ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteItcdfp013ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "ITCDFP013CODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastItcdfp013Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastItcdfp013Id);
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

        public List<Itcdfp013DTO> GetItcdfp013ById(int id)
        {
            List<Itcdfp013DTO> itcdfp013DTOs = new List<Itcdfp013DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdfp013ById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    Itcdfp013DTO ob = new Itcdfp013DTO
                    {
                        Itcdfp013Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDFP013CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDFP013CODI")) : 0,
                        ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0,
                        NombreCliente = !dr.IsDBNull(dr.GetOrdinal("NOMBRECLIENTE")) ? dr.GetString(dr.GetOrdinal("NOMBRECLIENTE")) : null,
                        TipoCarga = !dr.IsDBNull(dr.GetOrdinal("TIPOCARGA")) ? dr.GetString(dr.GetOrdinal("TIPOCARGA")) : null,
                        UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null,
                        FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue,
                        UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : null,
                        FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue,
                        IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null
                    };
                    itcdfp013DTOs.Add(ob);
                }
            }

            return itcdfp013DTOs;
        }

        public List<Itcdfp013DTO> GetItcdfp013ByFilter(string plancodi, string empresa, string estado)
        {
            List<Itcdfp013DTO> itcdfp013DTOs = new List<Itcdfp013DTO>();
            string query = $@"
                SELECT CGB.*, TR.AREADEMANDA, TR.EMPRESANOM 
                FROM CAM_ITCDFP013 CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.ITCDFP013CODI ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    Itcdfp013DTO ob = new Itcdfp013DTO
                    {
                        Itcdfp013Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDFP013CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDFP013CODI")) : 0,
                        ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0,
                        NombreCliente = !dr.IsDBNull(dr.GetOrdinal("NOMBRECLIENTE")) ? dr.GetString(dr.GetOrdinal("NOMBRECLIENTE")) : null,
                        TipoCarga = !dr.IsDBNull(dr.GetOrdinal("TIPOCARGA")) ? dr.GetString(dr.GetOrdinal("TIPOCARGA")) : null,
                        UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null,
                        FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue,
                        UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : null,
                        FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue,
                        IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null,
                        Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : "",
                        AreaDemanda = !dr.IsDBNull(dr.GetOrdinal("AREADEMANDA")) ? dr.GetString(dr.GetOrdinal("AREADEMANDA")) : ""
                    };
                    itcdfp013DTOs.Add(ob);
                }
            }

            return itcdfp013DTOs;
        }

        public bool UpdateItcdfp013(Itcdfp013DTO itcdfp013DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateItcdfp013);
            dbProvider.AddInParameter(dbCommand, "ITCDFP013CODI", DbType.Int32, itcdfp013DTO.Itcdfp013Codi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcdfp013DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "NOMBRECLIENTE", DbType.String, itcdfp013DTO.NombreCliente);
            dbProvider.AddInParameter(dbCommand, "TIPOCARGA", DbType.String, itcdfp013DTO.TipoCarga);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, itcdfp013DTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, itcdfp013DTO.FecModificacion);
            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }
    }
}

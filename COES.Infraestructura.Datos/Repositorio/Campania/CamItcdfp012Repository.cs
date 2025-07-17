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
    public class CamItcdfp012Repository : RepositoryBase, ICamItcdfp012Repository
    {
        public CamItcdfp012Repository(string strConn) : base(strConn) { }

        CamItcdfp012Helper Helper = new CamItcdfp012Helper();

        public List<Itcdfp012DTO> GetItcdfp012Codi(int proyCodi)
        {
            List<Itcdfp012DTO> itcdfp012DTOs = new List<Itcdfp012DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdfp012Codi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    Itcdfp012DTO ob = new Itcdfp012DTO
                    {
                        Itcdfp012Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDFP012CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDFP012CODI")) : 0,
                        ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0,
                        CodigoSicli = !dr.IsDBNull(dr.GetOrdinal("CODIGOSICLI")) ? dr.GetString(dr.GetOrdinal("CODIGOSICLI")) : null,
                        NombreCliente = !dr.IsDBNull(dr.GetOrdinal("NOMBRECLIENTE")) ? dr.GetString(dr.GetOrdinal("NOMBRECLIENTE")) : null,
                        Subestacion = !dr.IsDBNull(dr.GetOrdinal("SUBESTACION")) ? dr.GetString(dr.GetOrdinal("SUBESTACION")) : null,
                        Barra = !dr.IsDBNull(dr.GetOrdinal("BARRA")) ? dr.GetString(dr.GetOrdinal("BARRA")) : null,
                        CodigoNivelTension = !dr.IsDBNull(dr.GetOrdinal("CODIGONIVELTENSION")) ? dr.GetString(dr.GetOrdinal("CODIGONIVELTENSION")) : null,
                        UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null,
                        FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue,
                        UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : null,
                        FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue,
                        IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null
                    };
                    itcdfp012DTOs.Add(ob);
                }
            }

            return itcdfp012DTOs;
        }

        public bool SaveItcdfp012(Itcdfp012DTO itcdfp012DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveItcdfp012);
            dbProvider.AddInParameter(dbCommand, "ITCDFP012CODI", DbType.Int32, itcdfp012DTO.Itcdfp012Codi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcdfp012DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "CODIGOSICLI", DbType.String, itcdfp012DTO.CodigoSicli);
            dbProvider.AddInParameter(dbCommand, "NOMBRECLIENTE", DbType.String, itcdfp012DTO.NombreCliente);
            dbProvider.AddInParameter(dbCommand, "SUBESTACION", DbType.String, itcdfp012DTO.Subestacion);
            dbProvider.AddInParameter(dbCommand, "BARRA", DbType.String, itcdfp012DTO.Barra);
            dbProvider.AddInParameter(dbCommand, "CODIGONIVELTENSION", DbType.String, itcdfp012DTO.CodigoNivelTension);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, itcdfp012DTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, itcdfp012DTO.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteItcdfp012ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteItcdfp012ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastItcdfp012Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastItcdfp012Id);
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

        public List<Itcdfp012DTO> GetItcdfp012ById(int id)
        {
            List<Itcdfp012DTO> itcdfp012DTOs = new List<Itcdfp012DTO>();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdfp012ById);
            dbProvider.AddInParameter(commandHoja, "ITCDFP012CODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);

             using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    Itcdfp012DTO ob = new Itcdfp012DTO
                    {
                        Itcdfp012Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDFP012CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDFP012CODI")) : 0,
                        ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0,
                        CodigoSicli = !dr.IsDBNull(dr.GetOrdinal("CODIGOSICLI")) ? dr.GetString(dr.GetOrdinal("CODIGOSICLI")) : null,
                        NombreCliente = !dr.IsDBNull(dr.GetOrdinal("NOMBRECLIENTE")) ? dr.GetString(dr.GetOrdinal("NOMBRECLIENTE")) : null,
                        Subestacion = !dr.IsDBNull(dr.GetOrdinal("SUBESTACION")) ? dr.GetString(dr.GetOrdinal("SUBESTACION")) : null,
                        Barra = !dr.IsDBNull(dr.GetOrdinal("BARRA")) ? dr.GetString(dr.GetOrdinal("BARRA")) : null,
                        CodigoNivelTension = !dr.IsDBNull(dr.GetOrdinal("CODIGONIVELTENSION")) ? dr.GetString(dr.GetOrdinal("CODIGONIVELTENSION")) : null,
                        UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null,
                        FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue,
                        UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : null,
                        FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue,
                        IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null
                    };
                    itcdfp012DTOs.Add(ob);
                }
            }

            return itcdfp012DTOs;
        }

        public bool UpdateItcdfp012(Itcdfp012DTO itcdfp012DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateItcdfp012);
            dbProvider.AddInParameter(dbCommand, "ITCDFP012CODI", DbType.Int32, itcdfp012DTO.Itcdfp012Codi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcdfp012DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "CODIGOSICLI", DbType.String, itcdfp012DTO.CodigoSicli);
            dbProvider.AddInParameter(dbCommand, "NOMBRECLIENTE", DbType.String, itcdfp012DTO.NombreCliente);
            dbProvider.AddInParameter(dbCommand, "SUBESTACION", DbType.String, itcdfp012DTO.Subestacion);
            dbProvider.AddInParameter(dbCommand, "BARRA", DbType.String, itcdfp012DTO.Barra);
            dbProvider.AddInParameter(dbCommand, "CODIGONIVELTENSION", DbType.String, itcdfp012DTO.CodigoNivelTension);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, itcdfp012DTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, itcdfp012DTO.FecModificacion);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, itcdfp012DTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public List<Itcdfp012DTO> GetItcdfp012ByFilter(string plancodi, string empresa, string estado)
        {
            List<Itcdfp012DTO> itcdfp012DTOs = new List<Itcdfp012DTO>();
            string query = $@"
                SELECT CGB.*, TR.AREADEMANDA, TR.EMPRESANOM 

                FROM CAM_ITCDFP012 CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.ITCDFP012CODI ASC";
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    Itcdfp012DTO ob = new Itcdfp012DTO
                    {
                        Itcdfp012Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDFP012CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDFP012CODI")) : 0,
                        ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0,
                        CodigoSicli = !dr.IsDBNull(dr.GetOrdinal("CODIGOSICLI")) ? dr.GetString(dr.GetOrdinal("CODIGOSICLI")) : null,
                        NombreCliente = !dr.IsDBNull(dr.GetOrdinal("NOMBRECLIENTE")) ? dr.GetString(dr.GetOrdinal("NOMBRECLIENTE")) : null,
                        Subestacion = !dr.IsDBNull(dr.GetOrdinal("SUBESTACION")) ? dr.GetString(dr.GetOrdinal("SUBESTACION")) : null,
                        Barra = !dr.IsDBNull(dr.GetOrdinal("BARRA")) ? dr.GetString(dr.GetOrdinal("BARRA")) : null,
                        CodigoNivelTension = !dr.IsDBNull(dr.GetOrdinal("CODIGONIVELTENSION")) ? dr.GetString(dr.GetOrdinal("CODIGONIVELTENSION")) : null,
                        UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null,
                        FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue,
                        UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : null,
                        FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue,
                        IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null,
                        Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : "",
                        AreaDemanda = !dr.IsDBNull(dr.GetOrdinal("AREADEMANDA")) ? dr.GetString(dr.GetOrdinal("AREADEMANDA")) : ""
                    };
                    itcdfp012DTOs.Add(ob);
                }
            }

            return itcdfp012DTOs;
        }

    }
}

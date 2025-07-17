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

    public class CamItcPrm1Repository : RepositoryBase, ICamItcPrm1Repository
    {
        
        public CamItcPrm1Repository(string strConn) : base(strConn) { }

        CamItcPrm1Helper Helper = new CamItcPrm1Helper();

        public List<ItcPrm1Dto> GetItcPrm1ByProyCodi(int proyCodi)
        {
            List<ItcPrm1Dto> itcprm1DTOs = new List<ItcPrm1Dto>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamItcPrm1Codi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ItcPrm1Dto ob = new ItcPrm1Dto();
                    ob.ItcPrm1Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDPRM1CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDPRM1CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Electroducto = !dr.IsDBNull(dr.GetOrdinal("ELECTRODUCTO")) ? dr.GetString(dr.GetOrdinal("ELECTRODUCTO")) : string.Empty;
                    ob.Descripcion = !dr.IsDBNull(dr.GetOrdinal("DESCRIPCION")) ? dr.GetString(dr.GetOrdinal("DESCRIPCION")) : string.Empty;
                    ob.Vn = !dr.IsDBNull(dr.GetOrdinal("VN")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VN")) :  null;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : string.Empty;
                    ob.Seccion = !dr.IsDBNull(dr.GetOrdinal("SECCION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("SECCION")) : null;
                    ob.Ctr = !dr.IsDBNull(dr.GetOrdinal("CTR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CTR")) : null;
                    ob.R = !dr.IsDBNull(dr.GetOrdinal("R")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("R")) : null;
                    ob.X = !dr.IsDBNull(dr.GetOrdinal("X")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("X")) : null;
                    ob.B = !dr.IsDBNull(dr.GetOrdinal("B")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("B")) : null;
                    ob.Ro = !dr.IsDBNull(dr.GetOrdinal("RO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("RO")) : null;
                    ob.Xo = !dr.IsDBNull(dr.GetOrdinal("XO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("XO")) : null;
                    ob.Bo = !dr.IsDBNull(dr.GetOrdinal("BO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("BO")) : null;
                    ob.Capacidad = !dr.IsDBNull(dr.GetOrdinal("CAPACIDAD")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CAPACIDAD")) : null;
                    ob.Tmxop = !dr.IsDBNull(dr.GetOrdinal("TMXOP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TMXOP")) : null;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;

                    itcprm1DTOs.Add(ob);
                }
            }

            return itcprm1DTOs;
        }

        public bool SaveItcPrm1(ItcPrm1Dto itcprm1DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCamItcPrm1);
            dbProvider.AddInParameter(dbCommand, "ITCDPRM1CODI", DbType.Int32, itcprm1DTO.ItcPrm1Codi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcprm1DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "ELECTRODUCTO", DbType.String, itcprm1DTO.Electroducto);
            dbProvider.AddInParameter(dbCommand, "DESCRIPCION", DbType.String, itcprm1DTO.Descripcion);
            dbProvider.AddInParameter(dbCommand, "VN", DbType.Decimal, itcprm1DTO.Vn);
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, itcprm1DTO.Tipo);
            dbProvider.AddInParameter(dbCommand, "SECCION", DbType.Decimal, itcprm1DTO.Seccion);
            dbProvider.AddInParameter(dbCommand, "CTR", DbType.Decimal, itcprm1DTO.Ctr);
            dbProvider.AddInParameter(dbCommand, "R", DbType.Decimal, itcprm1DTO.R);
            dbProvider.AddInParameter(dbCommand, "X", DbType.Decimal, itcprm1DTO.X);
            dbProvider.AddInParameter(dbCommand, "B", DbType.Decimal, itcprm1DTO.B);
            dbProvider.AddInParameter(dbCommand, "RO", DbType.Decimal, itcprm1DTO.Ro);
            dbProvider.AddInParameter(dbCommand, "XO", DbType.Decimal, itcprm1DTO.Xo);
            dbProvider.AddInParameter(dbCommand, "BO", DbType.Decimal, itcprm1DTO.Bo);
            dbProvider.AddInParameter(dbCommand, "CAPACIDAD", DbType.Decimal, itcprm1DTO.Capacidad);
            dbProvider.AddInParameter(dbCommand, "TMXOP", DbType.Decimal, itcprm1DTO.Tmxop);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, itcprm1DTO.Usucreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteItcPrm1ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCamItcPrm1ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastItcPrm1Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCamItcPrm1Id);
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

        public List<ItcPrm1Dto> GetItcPrm1ById(int id)
        {
            List<ItcPrm1Dto> itcprm1DTOs = new List<ItcPrm1Dto>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamItcPrm1ById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ItcPrm1Dto ob = new ItcPrm1Dto();
                    ob.ItcPrm1Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDPRM1CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDPRM1CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Electroducto = !dr.IsDBNull(dr.GetOrdinal("ELECTRODUCTO")) ? dr.GetString(dr.GetOrdinal("ELECTRODUCTO")) : string.Empty;
                    ob.Descripcion = !dr.IsDBNull(dr.GetOrdinal("DESCRIPCION")) ? dr.GetString(dr.GetOrdinal("DESCRIPCION")) : string.Empty;
                    ob.Vn = !dr.IsDBNull(dr.GetOrdinal("VN")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VN")) : null;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : string.Empty;
                    ob.Seccion = !dr.IsDBNull(dr.GetOrdinal("SECCION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("SECCION")) : null;
                    ob.Ctr = !dr.IsDBNull(dr.GetOrdinal("CTR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CTR")) : null;
                    ob.R = !dr.IsDBNull(dr.GetOrdinal("R")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("R")) : null;
                    ob.X = !dr.IsDBNull(dr.GetOrdinal("X")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("X")) : null;
                    ob.B = !dr.IsDBNull(dr.GetOrdinal("B")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("B")) : null;
                    ob.Ro = !dr.IsDBNull(dr.GetOrdinal("RO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("RO")) : null;
                    ob.Xo = !dr.IsDBNull(dr.GetOrdinal("XO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("XO")) : null;
                    ob.Bo = !dr.IsDBNull(dr.GetOrdinal("BO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("BO")) : null;
                    ob.Capacidad = !dr.IsDBNull(dr.GetOrdinal("CAPACIDAD")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CAPACIDAD")) : null;
                    ob.Tmxop = !dr.IsDBNull(dr.GetOrdinal("TMXOP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TMXOP")) : null;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    itcprm1DTOs.Add(ob);
                 }
            }

            return itcprm1DTOs;
        }

        public bool UpdateItcPrm1(ItcPrm1Dto itcprm1DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateCamItcPrm1);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcprm1DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "ELECTRODUCTO", DbType.String, itcprm1DTO.Electroducto);
            dbProvider.AddInParameter(dbCommand, "DESCRIPCION", DbType.String, itcprm1DTO.Descripcion);
            dbProvider.AddInParameter(dbCommand, "VN", DbType.Decimal, itcprm1DTO.Vn);
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, itcprm1DTO.Tipo);
            dbProvider.AddInParameter(dbCommand, "SECCION", DbType.Decimal, itcprm1DTO.Seccion);
            dbProvider.AddInParameter(dbCommand, "CTR", DbType.Decimal, itcprm1DTO.Ctr);
            dbProvider.AddInParameter(dbCommand, "R", DbType.Decimal, itcprm1DTO.R);
            dbProvider.AddInParameter(dbCommand, "X", DbType.Decimal, itcprm1DTO.X);
            dbProvider.AddInParameter(dbCommand, "B", DbType.Decimal, itcprm1DTO.B);
            dbProvider.AddInParameter(dbCommand, "RO", DbType.Decimal, itcprm1DTO.Ro);
            dbProvider.AddInParameter(dbCommand, "XO", DbType.Decimal, itcprm1DTO.Xo);
            dbProvider.AddInParameter(dbCommand, "BO", DbType.Decimal, itcprm1DTO.Bo);
            dbProvider.AddInParameter(dbCommand, "CAPACIDAD", DbType.Decimal, itcprm1DTO.Capacidad);
            dbProvider.AddInParameter(dbCommand, "TMXOP", DbType.Decimal, itcprm1DTO.Tmxop);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, itcprm1DTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, itcprm1DTO.Fechamodificacion);
            dbProvider.AddInParameter(dbCommand, "ITCDPRM1CODI", DbType.Int32, itcprm1DTO.ItcPrm1Codi);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }


        public List<ItcPrm1Dto> GetItcPrm1ByFilter(string plancodi, string empresa, string estado)
        {
            List<ItcPrm1Dto> itcprm1DTOs = new List<ItcPrm1Dto>();
            string query = $@"
                SELECT CGB.*, TR.AREADEMANDA, TR.EMPRESANOM   
                FROM CAM_ITCPRM1 CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.ITCDPRM1CODI ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ItcPrm1Dto ob = new ItcPrm1Dto();
                    ob.ItcPrm1Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDPRM1CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDPRM1CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Electroducto = !dr.IsDBNull(dr.GetOrdinal("ELECTRODUCTO")) ? dr.GetString(dr.GetOrdinal("ELECTRODUCTO")) : string.Empty;
                    ob.Descripcion = !dr.IsDBNull(dr.GetOrdinal("DESCRIPCION")) ? dr.GetString(dr.GetOrdinal("DESCRIPCION")) : string.Empty;
                    ob.Vn = !dr.IsDBNull(dr.GetOrdinal("VN")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VN")) : null;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : string.Empty;
                    ob.Seccion = !dr.IsDBNull(dr.GetOrdinal("SECCION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("SECCION")) : null;
                    ob.Ctr = !dr.IsDBNull(dr.GetOrdinal("CTR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CTR")) : null;
                    ob.R = !dr.IsDBNull(dr.GetOrdinal("R")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("R")) : null;
                    ob.X = !dr.IsDBNull(dr.GetOrdinal("X")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("X")) : null;
                    ob.B = !dr.IsDBNull(dr.GetOrdinal("B")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("B")) : null;
                    ob.Ro = !dr.IsDBNull(dr.GetOrdinal("RO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("RO")) : null;
                    ob.Xo = !dr.IsDBNull(dr.GetOrdinal("XO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("XO")) : null;
                    ob.Bo = !dr.IsDBNull(dr.GetOrdinal("BO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("BO")) : null;
                    ob.Capacidad = !dr.IsDBNull(dr.GetOrdinal("CAPACIDAD")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CAPACIDAD")) : null;
                    ob.Tmxop = !dr.IsDBNull(dr.GetOrdinal("TMXOP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TMXOP")) : null;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : "";
                    ob.AreaDemanda = !dr.IsDBNull(dr.GetOrdinal("AREADEMANDA")) ? dr.GetString(dr.GetOrdinal("AREADEMANDA")) : "";

                    itcprm1DTOs.Add(ob);
                }
            }

            return itcprm1DTOs;
        }


    }
}

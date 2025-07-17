using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla RCA_PROGRAMA
    /// </summary>
    public class RcaProgramaRepository: RepositoryBase, IRcaProgramaRepository
    {
        public RcaProgramaRepository(string strConn): base(strConn)
        {
        }

        RcaProgramaHelper helper = new RcaProgramaHelper();

        public int Save(RcaProgramaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlSave, entity.Rcprogabrev));

            dbProvider.AddInParameter(command, helper.Rcprogcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rchorpcodi, DbType.Int32, entity.Rchorpcodi);
            //dbProvider.AddInParameter(command, helper.Rcprognombre, DbType.String, entity.Rcprognombre);
            //dbProvider.AddInParameter(command, helper.Rcprogabrev, DbType.String, entity.Rcprogabrev);
            dbProvider.AddInParameter(command, helper.Rcprogestado, DbType.String, entity.Rcprogestado);
            dbProvider.AddInParameter(command, helper.Rcprogfecinicio, DbType.DateTime, entity.Rcprogfecinicio);
            dbProvider.AddInParameter(command, helper.Rcprogfecfin, DbType.DateTime, entity.Rcprogfecfin);
            dbProvider.AddInParameter(command, helper.Rcprogestregistro, DbType.String, entity.Rcprogestregistro);
            dbProvider.AddInParameter(command, helper.Rcprogusucreacion, DbType.String, entity.Rcprogusucreacion);
            dbProvider.AddInParameter(command, helper.Rcprogfeccreacion, DbType.DateTime, entity.Rcprogfeccreacion);
            dbProvider.AddInParameter(command, helper.Rcprogcodipadre, DbType.Int32, entity.Rcprogcodipadre > 0 ? entity.Rcprogcodipadre : (object)DBNull.Value);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int CrearPrograma(RcaProgramaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            var sentencia = " INSERT INTO RCA_PROGRAMA(RCPROGCODI,RCPROGABREV,RCPROGNOMBRE,RCHORPCODI,RCPROGESTREGISTRO,RCPROGUSUCREACION,RCPROGFECCREACION) ";
            sentencia = sentencia + "VALUES(" + id.ToString() + "," + "'" + entity.Rcprogabrev + "','" +entity.Rcprognombre+"',"+ entity.Rchorpcodi + "," + "'" + entity.Rcprogestregistro 
                + "','" + entity.Rcprogusucreacion + "',SYSDATE)";

            command = dbProvider.GetSqlStringCommand(sentencia);
            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void Update(RcaProgramaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlUpdate, entity.Rcprogabrev));

            dbProvider.AddInParameter(command, helper.Rchorpcodi, DbType.Int32, entity.Rchorpcodi);
            dbProvider.AddInParameter(command, helper.Rcprognombre, DbType.String, entity.Rcprognombre);
            //dbProvider.AddInParameter(command, helper.Rcprogabrev, DbType.String, entity.Rcprogabrev);
            dbProvider.AddInParameter(command, helper.Rcprogestado, DbType.String, entity.Rcprogestado);
            dbProvider.AddInParameter(command, helper.Rcprogestregistro, DbType.String, entity.Rcprogestregistro);            
            dbProvider.AddInParameter(command, helper.Rcprogusumodificacion, DbType.String, entity.Rcprogusumodificacion);
            dbProvider.AddInParameter(command, helper.Rcprogfecmodificacion, DbType.DateTime, entity.Rcprogfecmodificacion.Value);
            dbProvider.AddInParameter(command, helper.Rcprogcodipadre, DbType.Int32, entity.Rcprogcodipadre > 0 ? entity.Rcprogcodipadre : (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Rcprogcodi, DbType.Int32, entity.Rcprogcodi);            

            dbProvider.ExecuteNonQuery(command);
        }
        public void ActualizarPrograma(RcaProgramaDTO entity)
        {
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            var sentencia = " UPDATE RCA_PROGRAMA SET RCPROGABREV="+"'" + entity.Rcprogabrev + "',"+ "RCPROGNOMBRE="+"'" + entity.Rcprognombre + "',"+ 
                "RCHORPCODI="+entity.Rchorpcodi+",RCPROGESTREGISTRO="+"'"+entity.Rcprogestregistro+"'"+",RCPROGUSUMODIFICACION='"+entity.Rcprogusumodificacion+"',"+
                "RCPROGFECMODIFICACION=SYSDATE ";
            sentencia = sentencia + " WHERE RCPROGCODI=" + entity.Rcprogcodi;
            

            DbCommand command = dbProvider.GetSqlStringCommand(sentencia);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rcprogcodi, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlDelete, usuario));

            dbProvider.AddInParameter(command, helper.Rcprogcodi, DbType.Int32, rcprogcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RcaProgramaDTO GetById(int rcprogcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rcprogcodi, DbType.Int32, rcprogcodi);
            RcaProgramaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RcaProgramaDTO> List()
        {
            List<RcaProgramaDTO> entitys = new List<RcaProgramaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<RcaProgramaDTO> GetByCriteria(string codigoProgramaAbrev)
        {
            List<RcaProgramaDTO> entitys = new List<RcaProgramaDTO>();            

            if (string.IsNullOrEmpty(codigoProgramaAbrev))
            {
                return entitys;
            }

            var condicion = " where rcprogabrev='" + codigoProgramaAbrev + "'";
            condicion = condicion + " AND rcprogestregistro = '1' ";

            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlGetByCriteria, condicion));
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<RcaProgramaDTO> ListProgramaEnvioArchivo(DateTime fechaReferencia)
        {
            List<RcaProgramaDTO> entitys = new List<RcaProgramaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqListProgramaEnvioArchivo);

            dbProvider.AddInParameter(command, "RCCUADFECHORINICIOEJEC", DbType.DateTime, fechaReferencia);      

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new RcaProgramaDTO();

                    int iRcprogcodi = dr.GetOrdinal(helper.Rcprogcodi);
                    if (!dr.IsDBNull(iRcprogcodi)) entity.Rcprogcodi = Convert.ToInt32(dr.GetValue(iRcprogcodi));                    

                    int iRcprognombre = dr.GetOrdinal(helper.Rcprognombre);
                    if (!dr.IsDBNull(iRcprognombre)) entity.Rcprognombre = dr.GetString(iRcprognombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RcaProgramaDTO> ListProgramaRechazoCarga(bool muestraVigentes)
        {
            List<RcaProgramaDTO> entitys = new List<RcaProgramaDTO>();

            var query = helper.SqlListProgramaRechazoCarga;

            if (muestraVigentes)
            {
                query = string.Format(query, "AND TRUNC(RCPROGFECFIN) >= TRUNC(SYSDATE)");
            }
            else
            {
                query = string.Format(query, "");
            }

            DbCommand command = dbProvider.GetSqlStringCommand(query);            

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new RcaProgramaDTO();

                    int iRcprogcodi = dr.GetOrdinal(helper.Rcprogcodi);
                    if (!dr.IsDBNull(iRcprogcodi)) entity.Rcprogcodi = Convert.ToInt32(dr.GetValue(iRcprogcodi));

                    int iRcprognombre = dr.GetOrdinal(helper.Rcprognombre);
                    if (!dr.IsDBNull(iRcprognombre)) entity.Rcprognombre = dr.GetString(iRcprognombre);

                    int iRchorpcodi = dr.GetOrdinal(helper.Rchorpcodi);
                    if (!dr.IsDBNull(iRchorpcodi)) entity.Rchorpcodi = Convert.ToInt32(dr.GetValue(iRchorpcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RcaProgramaDTO> ListProgramaFiltro(int horizonte, string codigoPrograma, string nombrePrograma, int reprograma)
        {
            List<RcaProgramaDTO> entitys = new List<RcaProgramaDTO>();
            var condicion = string.Empty;

            if (horizonte > 0)
            {
                condicion = condicion + " AND pro.rchorpcodi = " + horizonte;
            }
            else
            {
                if (reprograma > 0)
                {
                    condicion = condicion + " AND pro.rchorpcodi <> " + reprograma;
                }
            }

            if (!string.IsNullOrEmpty(codigoPrograma))
            {
                condicion = condicion + string.Format(" AND UPPER(rcprogabrev) like '%{0}%'", codigoPrograma.ToUpper());
            }

            if (!string.IsNullOrEmpty(nombrePrograma))
            {
                condicion = condicion + (nombrePrograma.Equals("1") ? " AND TRUNC(RCPROGFECFIN) >= TO_DATE('" + DateTime.Now.ToString("dd/MM/yyyy") + "' , 'DD/MM/YYYY') "
                    : " AND TRUNC(RCPROGFECFIN) < TO_DATE('" + DateTime.Now.ToString("dd/MM/yyyy") + "' , 'DD/MM/YYYY') ");
            }

            string queryString = string.Format(helper.SqListFiltro, condicion);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new RcaProgramaDTO();

                    int iRcprogcodi = dr.GetOrdinal(helper.Rcprogcodi);
                    if (!dr.IsDBNull(iRcprogcodi)) entity.Rcprogcodi = Convert.ToInt32(dr.GetValue(iRcprogcodi));

                    int iRchorpcodi = dr.GetOrdinal(helper.Rchorpcodi);
                    if (!dr.IsDBNull(iRchorpcodi)) entity.Rchorpcodi = Convert.ToInt32(dr.GetValue(iRchorpcodi));

                    int iRcprogabrev = dr.GetOrdinal(helper.Rcprogabrev);
                    if (!dr.IsDBNull(iRcprogabrev)) entity.Rcprogabrev = dr.GetString(iRcprogabrev);

                    int iRcprognombre = dr.GetOrdinal(helper.Rcprognombre);
                    if (!dr.IsDBNull(iRcprognombre)) entity.Rcprognombre = dr.GetString(iRcprognombre);

                    int iRcprogestado = dr.GetOrdinal(helper.Rcprogestado);
                    if (!dr.IsDBNull(iRcprogestado)) entity.Rcprogestado = dr.GetString(iRcprogestado);

                    int iRcproghorizonte = dr.GetOrdinal(helper.Rcproghorizonte);
                    if (!dr.IsDBNull(iRcproghorizonte)) entity.Rcproghorizonte = dr.GetString(iRcproghorizonte);

                    int iRcprogfecinicio = dr.GetOrdinal(helper.Rcprogfecinicio);
                    if (!dr.IsDBNull(iRcprogfecinicio)) entity.Rcprogfecinicio = dr.GetDateTime(iRcprogfecinicio);

                    int iRcprogfecfin = dr.GetOrdinal(helper.Rcprogfecfin);
                    if (!dr.IsDBNull(iRcprogfecfin)) entity.Rcprogfecfin = dr.GetDateTime(iRcprogfecfin);

                    int iNroCuadros = dr.GetOrdinal(helper.NroCuadros);
                    if (!dr.IsDBNull(iNroCuadros)) entity.Nrocuadros = Convert.ToInt32(dr.GetValue(iNroCuadros));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Linq;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla RE_RECHAZO_CARGA
    /// </summary>
    public class ReRechazoCargaRepository : RepositoryBase, IReRechazoCargaRepository
    {
        public ReRechazoCargaRepository(string strConn) : base(strConn)
        {
        }

        ReRechazoCargaHelper helper = new ReRechazoCargaHelper();

        public int Save(ReRechazoCargaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rerccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Rercpadre, DbType.Int32, entity.Rercpadre);
            dbProvider.AddInParameter(command, helper.Rercfinal, DbType.String, entity.Rercfinal);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Rercestado, DbType.String, entity.Rercestado);
            dbProvider.AddInParameter(command, helper.Rercmotivoanulacion, DbType.String, entity.Rercmotivoanulacion);
            dbProvider.AddInParameter(command, helper.Rercusueliminacion, DbType.String, entity.Rercusueliminacion);
            dbProvider.AddInParameter(command, helper.Rercfecanulacion, DbType.DateTime, entity.Rercfecanulacion);
            dbProvider.AddInParameter(command, helper.Rerccorrelativo, DbType.Int32, entity.Rerccorrelativo);
            dbProvider.AddInParameter(command, helper.Rerctipcliente, DbType.String, entity.Rerctipcliente);
            dbProvider.AddInParameter(command, helper.Rerccliente, DbType.Int32, entity.Rerccliente);
            dbProvider.AddInParameter(command, helper.Repentcodi, DbType.Int32, entity.Repentcodi);
            dbProvider.AddInParameter(command, helper.Rercptoentrega, DbType.String, entity.Rercptoentrega);
            dbProvider.AddInParameter(command, helper.Rercalimentadorsed, DbType.String, entity.Rercalimentadorsed);
            dbProvider.AddInParameter(command, helper.Rercenst, DbType.Decimal, entity.Rercenst);
            dbProvider.AddInParameter(command, helper.Reevecodi, DbType.Int32, entity.Reevecodi);
            dbProvider.AddInParameter(command, helper.Rerccomentario, DbType.String, entity.Rerccomentario);
            dbProvider.AddInParameter(command, helper.Rerctejecinicio, DbType.DateTime, entity.Rerctejecinicio);
            dbProvider.AddInParameter(command, helper.Rerctejecfin, DbType.DateTime, entity.Rerctejecfin);
            dbProvider.AddInParameter(command, helper.Rercpk, DbType.Decimal, entity.Rercpk);
            dbProvider.AddInParameter(command, helper.Rerccompensable, DbType.String, entity.Rerccompensable);
            dbProvider.AddInParameter(command, helper.Rercens, DbType.Decimal, entity.Rercens);
            dbProvider.AddInParameter(command, helper.Rercresarcimiento, DbType.Decimal, entity.Rercresarcimiento);
            dbProvider.AddInParameter(command, helper.Rercusucreacion, DbType.String, entity.Rercusucreacion);
            dbProvider.AddInParameter(command, helper.Rercfeccreacion, DbType.DateTime, entity.Rercfeccreacion);

            dbProvider.AddInParameter(command, helper.Rercporcentaje1, DbType.Decimal, entity.Rercporcentaje1);
            dbProvider.AddInParameter(command, helper.Rercporcentaje2, DbType.Decimal, entity.Rercporcentaje2);
            dbProvider.AddInParameter(command, helper.Rercporcentaje3, DbType.Decimal, entity.Rercporcentaje3);
            dbProvider.AddInParameter(command, helper.Rercporcentaje4, DbType.Decimal, entity.Rercporcentaje4);
            dbProvider.AddInParameter(command, helper.Rercporcentaje5, DbType.Decimal, entity.Rercporcentaje5);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ReRechazoCargaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rercpadre, DbType.Int32, entity.Rercpadre);
            dbProvider.AddInParameter(command, helper.Rercfinal, DbType.String, entity.Rercfinal);
            dbProvider.AddInParameter(command, helper.Rerccodi, DbType.Int32, entity.Rerccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rerccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rerccodi, DbType.Int32, rerccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ReRechazoCargaDTO GetById(int rerccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rerccodi, DbType.Int32, rerccodi);
            ReRechazoCargaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ReRechazoCargaDTO> List()
        {
            List<ReRechazoCargaDTO> entitys = new List<ReRechazoCargaDTO>();
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

        public List<ReRechazoCargaDTO> GetByCriteria()
        {
            List<ReRechazoCargaDTO> entitys = new List<ReRechazoCargaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<ReRechazoCargaDTO> ObtenerPorEmpresaPeriodo(int idEmpresa, int idPeriodo)
        {
            List<ReRechazoCargaDTO> entitys = new List<ReRechazoCargaDTO>();
            string sql = string.Format(helper.SqlObtenerPorEmpresaPeriodo, idEmpresa, idPeriodo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReRechazoCargaDTO entity = new ReRechazoCargaDTO();

                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iPtoentrega = dr.GetOrdinal(helper.Ptoentrega);
                    if (!dr.IsDBNull(iPtoentrega)) entity.Ptoentrega = dr.GetString(iPtoentrega);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void AnularRechazoCarga(int idRechazo, string comentario, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlAnularRechazoCarga);

            dbProvider.AddInParameter(command, helper.Rercmotivoanulacion, DbType.String, comentario);
            dbProvider.AddInParameter(command, helper.Rercusueliminacion, DbType.String, usuario);
            dbProvider.AddInParameter(command, helper.Rerccodi, DbType.Int32, idRechazo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarPorcentajes(ReRechazoCargaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarPorcentajes);

            dbProvider.AddInParameter(command, helper.Rercporcentaje1, DbType.Decimal, entity.Rercporcentaje1);
            dbProvider.AddInParameter(command, helper.Rercdisposicion1, DbType.String, entity.Rercdisposicion1);
            dbProvider.AddInParameter(command, helper.Rercporcentaje2, DbType.Decimal, entity.Rercporcentaje2);
            dbProvider.AddInParameter(command, helper.Rercdisposicion2, DbType.String, entity.Rercdisposicion2);
            dbProvider.AddInParameter(command, helper.Rercporcentaje3, DbType.Decimal, entity.Rercporcentaje3);
            dbProvider.AddInParameter(command, helper.Rercdisposicion3, DbType.String, entity.Rercdisposicion3);
            dbProvider.AddInParameter(command, helper.Rercporcentaje4, DbType.Decimal, entity.Rercporcentaje4);
            dbProvider.AddInParameter(command, helper.Rercdisposicion4, DbType.String, entity.Rercdisposicion4);
            dbProvider.AddInParameter(command, helper.Rercporcentaje5, DbType.Decimal, entity.Rercporcentaje5);
            dbProvider.AddInParameter(command, helper.Rercdisposicion5, DbType.String, entity.Rercdisposicion5);
            dbProvider.AddInParameter(command, helper.Rerccodi, DbType.Int32, entity.Rerccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<ReRechazoCargaDTO> ObtenerConsolidado(int periodo, int suministrador, int barra, string estado, int evento, string alimentadorSED, string final,
            int responsable, string compensacion)
        {
            List<ReRechazoCargaDTO> entitys = new List<ReRechazoCargaDTO>();
            string sql = string.Format(helper.SqlObtenerConsolidado, periodo, suministrador, barra, estado, evento, alimentadorSED, final, responsable, compensacion);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReRechazoCargaDTO entity = new ReRechazoCargaDTO();

                    int iRerccodi = dr.GetOrdinal(helper.Rerccodi);
                    if (!dr.IsDBNull(iRerccodi)) entity.Rerccodi = Convert.ToInt32(dr.GetValue(iRerccodi));

                    int iSuministrador = dr.GetOrdinal(helper.Suministrador);
                    if (!dr.IsDBNull(iSuministrador)) entity.Suministrador = dr.GetString(iSuministrador);

                    int iRercestado = dr.GetOrdinal(helper.Rercestado);
                    if (!dr.IsDBNull(iRercestado)) entity.Rercestado = dr.GetString(iRercestado);

                    int iRerccorrelativo = dr.GetOrdinal(helper.Rerccorrelativo);
                    if (!dr.IsDBNull(iRerccorrelativo)) entity.Rerccorrelativo = Convert.ToInt32(dr.GetValue(iRerccorrelativo));

                    int iRerctipcliente = dr.GetOrdinal(helper.Rerctipcliente);
                    if (!dr.IsDBNull(iRerctipcliente)) entity.Rerctipcliente = dr.GetString(iRerctipcliente);

                    int iCliente = dr.GetOrdinal(helper.Cliente);
                    if (!dr.IsDBNull(iCliente)) entity.Cliente = dr.GetString(iCliente);

                    int iRercptoentrega = dr.GetOrdinal(helper.Rercptoentrega);
                    if (!dr.IsDBNull(iRercptoentrega)) entity.Rercptoentrega = dr.GetString(iRercptoentrega);

                    int iRepentcodi = dr.GetOrdinal(helper.Repentcodi);
                    if (!dr.IsDBNull(iRepentcodi)) entity.Repentcodi = Convert.ToInt32(dr.GetValue(iRepentcodi));

                    int iRercalimentadorsed = dr.GetOrdinal(helper.Rercalimentadorsed);
                    if (!dr.IsDBNull(iRercalimentadorsed)) entity.Rercalimentadorsed = dr.GetString(iRercalimentadorsed);

                    int iRercenst = dr.GetOrdinal(helper.Rercenst);
                    if (!dr.IsDBNull(iRercenst)) entity.Rercenst = dr.GetDecimal(iRercenst);

                    int iEvento = dr.GetOrdinal(helper.Evento);
                    if (!dr.IsDBNull(iEvento)) entity.Evento = dr.GetString(iEvento);

                    int iReevecodi = dr.GetOrdinal(helper.Reevecodi);
                    if (!dr.IsDBNull(iReevecodi)) entity.Reevecodi = Convert.ToInt32(dr.GetValue(iReevecodi));

                    int iRerccomentario = dr.GetOrdinal(helper.Rerccomentario);
                    if (!dr.IsDBNull(iRerccomentario)) entity.Rerccomentario = dr.GetString(iRerccomentario);

                    int iRerctejecinicio = dr.GetOrdinal(helper.Rerctejecinicio);
                    if (!dr.IsDBNull(iRerctejecinicio)) entity.Rerctejecinicio = dr.GetDateTime(iRerctejecinicio);

                    int iRerctejecfin = dr.GetOrdinal(helper.Rerctejecfin);
                    if (!dr.IsDBNull(iRerctejecfin)) entity.Rerctejecfin = dr.GetDateTime(iRerctejecfin);

                    int iRercpk = dr.GetOrdinal(helper.Rercpk);
                    if (!dr.IsDBNull(iRercpk)) entity.Rercpk = dr.GetDecimal(iRercpk);

                    int iRerccompensable = dr.GetOrdinal(helper.Rerccompensable);
                    if (!dr.IsDBNull(iRerccompensable)) entity.Rerccompensable = dr.GetString(iRerccompensable);

                    int iRercens = dr.GetOrdinal(helper.Rercens);
                    if (!dr.IsDBNull(iRercens)) entity.Rercens = dr.GetDecimal(iRercens);

                    int iRercresarcimiento = dr.GetOrdinal(helper.Rercresarcimiento);
                    if (!dr.IsDBNull(iRercresarcimiento)) entity.Rercresarcimiento = dr.GetDecimal(iRercresarcimiento);

                    int iRercusucreacion = dr.GetOrdinal(helper.Rercusucreacion);
                    if (!dr.IsDBNull(iRercusucreacion)) entity.Rercusucreacion = dr.GetString(iRercusucreacion);

                    int iRercfeccreacion = dr.GetOrdinal(helper.Rercfeccreacion);
                    if (!dr.IsDBNull(iRercfeccreacion)) entity.Rercfeccreacion = dr.GetDateTime(iRercfeccreacion);

                    int iRercdisposicion1 = dr.GetOrdinal(helper.Rercdisposicion1);
                    if (!dr.IsDBNull(iRercdisposicion1)) entity.Rercdisposicion1 = dr.GetString(iRercdisposicion1);

                    int iRercdisposicion2 = dr.GetOrdinal(helper.Rercdisposicion2);
                    if (!dr.IsDBNull(iRercdisposicion2)) entity.Rercdisposicion2 = dr.GetString(iRercdisposicion2);

                    int iRercdisposicion3 = dr.GetOrdinal(helper.Rercdisposicion3);
                    if (!dr.IsDBNull(iRercdisposicion3)) entity.Rercdisposicion3 = dr.GetString(iRercdisposicion3);

                    int iRercdisposicion4 = dr.GetOrdinal(helper.Rercdisposicion4);
                    if (!dr.IsDBNull(iRercdisposicion4)) entity.Rercdisposicion4 = dr.GetString(iRercdisposicion4);

                    int iRercdisposicion5 = dr.GetOrdinal(helper.Rercdisposicion5);
                    if (!dr.IsDBNull(iRercdisposicion5)) entity.Rercdisposicion5 = dr.GetString(iRercdisposicion5);

                    int iRercporcentaje1 = dr.GetOrdinal(helper.Rercporcentaje1);
                    if (!dr.IsDBNull(iRercporcentaje1)) entity.Rercporcentaje1 = dr.GetDecimal(iRercporcentaje1);

                    int iRercporcentaje2 = dr.GetOrdinal(helper.Rercporcentaje2);
                    if (!dr.IsDBNull(iRercporcentaje2)) entity.Rercporcentaje2 = dr.GetDecimal(iRercporcentaje2);

                    int iRercporcentaje3 = dr.GetOrdinal(helper.Rercporcentaje3);
                    if (!dr.IsDBNull(iRercporcentaje3)) entity.Rercporcentaje3 = dr.GetDecimal(iRercporcentaje3);

                    int iRercporcentaje4 = dr.GetOrdinal(helper.Rercporcentaje4);
                    if (!dr.IsDBNull(iRercporcentaje4)) entity.Rercporcentaje4 = dr.GetDecimal(iRercporcentaje4);

                    int iRercporcentaje5 = dr.GetOrdinal(helper.Rercporcentaje5);
                    if (!dr.IsDBNull(iRercporcentaje5)) entity.Rercporcentaje5 = dr.GetDecimal(iRercporcentaje5);

                    int iRercresponsable1 = dr.GetOrdinal(helper.Rercresponsable1);
                    if (!dr.IsDBNull(iRercresponsable1)) entity.Rercresponsable1 = dr.GetString(iRercresponsable1);

                    int iRercresponsable2 = dr.GetOrdinal(helper.Rercresponsable2);
                    if (!dr.IsDBNull(iRercresponsable2)) entity.Rercresponsable2 = dr.GetString(iRercresponsable2);

                    int iRercresponsable3 = dr.GetOrdinal(helper.Rercresponsable3);
                    if (!dr.IsDBNull(iRercresponsable3)) entity.Rercresponsable3 = dr.GetString(iRercresponsable3);

                    int iRercresponsable4 = dr.GetOrdinal(helper.Rercresponsable4);
                    if (!dr.IsDBNull(iRercresponsable4)) entity.Rercresponsable4 = dr.GetString(iRercresponsable4);

                    int iRercresponsable5 = dr.GetOrdinal(helper.Rercresponsable5);
                    if (!dr.IsDBNull(iRercresponsable5)) entity.Rercresponsable5 = dr.GetString(iRercresponsable5);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ReRechazoCargaDTO> ObtenerTrazabilidad(int periodo, int suministrador)
        {
            List<ReRechazoCargaDTO> entitys = new List<ReRechazoCargaDTO>();
            string sql = string.Format(helper.SqlObtenerTrazabilidad, periodo, suministrador);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReRechazoCargaDTO entity = new ReRechazoCargaDTO();

                    int iRerccodi = dr.GetOrdinal(helper.Rerccodi);
                    if (!dr.IsDBNull(iRerccodi)) entity.Rerccodi = Convert.ToInt32(dr.GetValue(iRerccodi));

                    int iRercpadre = dr.GetOrdinal(helper.Rercpadre);
                    if (!dr.IsDBNull(iRercpadre)) entity.Rercpadre = Convert.ToInt32(dr.GetValue(iRercpadre));

                    int iRercfinal = dr.GetOrdinal(helper.Rercfinal);
                    if (!dr.IsDBNull(iRercfinal)) entity.Rercfinal = dr.GetString(iRercfinal);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ReRechazoCargaDTO> ObtenerNotificacionInterrupcion(List<int> ids)
        {
            List<ReRechazoCargaDTO> entitys = new List<ReRechazoCargaDTO>();
            string sql = string.Format(helper.SqlObtenerNotificacionInterrupcion, string.Join(",", ids.Select(n => n.ToString()).ToArray()));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReRechazoCargaDTO entity = new ReRechazoCargaDTO();

                    int iRerccodi = dr.GetOrdinal(helper.Rerccodi);
                    if (!dr.IsDBNull(iRerccodi)) entity.Rerccodi = Convert.ToInt32(dr.GetValue(iRerccodi));

                    int iSuministrador = dr.GetOrdinal(helper.Suministrador);
                    if (!dr.IsDBNull(iSuministrador)) entity.Suministrador = dr.GetString(iSuministrador);

                    int iRercestado = dr.GetOrdinal(helper.Rercestado);
                    if (!dr.IsDBNull(iRercestado)) entity.Rercestado = dr.GetString(iRercestado);

                    int iRerccorrelativo = dr.GetOrdinal(helper.Rerccorrelativo);
                    if (!dr.IsDBNull(iRerccorrelativo)) entity.Rerccorrelativo = Convert.ToInt32(dr.GetValue(iRerccorrelativo));

                    int iRerctipcliente = dr.GetOrdinal(helper.Rerctipcliente);
                    if (!dr.IsDBNull(iRerctipcliente)) entity.Rerctipcliente = dr.GetString(iRerctipcliente);

                    int iCliente = dr.GetOrdinal(helper.Cliente);
                    if (!dr.IsDBNull(iCliente)) entity.Cliente = dr.GetString(iCliente);

                    int iRercptoentrega = dr.GetOrdinal(helper.Rercptoentrega);
                    if (!dr.IsDBNull(iRercptoentrega)) entity.Rercptoentrega = dr.GetString(iRercptoentrega);

                    int iRercalimentadorsed = dr.GetOrdinal(helper.Rercalimentadorsed);
                    if (!dr.IsDBNull(iRercalimentadorsed)) entity.Rercalimentadorsed = dr.GetString(iRercalimentadorsed);

                    int iRercenst = dr.GetOrdinal(helper.Rercenst);
                    if (!dr.IsDBNull(iRercenst)) entity.Rercenst = dr.GetDecimal(iRercenst);

                    int iEvento = dr.GetOrdinal(helper.Evento);
                    if (!dr.IsDBNull(iEvento)) entity.Evento = dr.GetString(iEvento);

                    int iRerccomentario = dr.GetOrdinal(helper.Rerccomentario);
                    if (!dr.IsDBNull(iRerccomentario)) entity.Rerccomentario = dr.GetString(iRerccomentario);

                    int iRerctejecinicio = dr.GetOrdinal(helper.Rerctejecinicio);
                    if (!dr.IsDBNull(iRerctejecinicio)) entity.Rerctejecinicio = dr.GetDateTime(iRerctejecinicio);

                    int iRerctejecfin = dr.GetOrdinal(helper.Rerctejecfin);
                    if (!dr.IsDBNull(iRerctejecfin)) entity.Rerctejecfin = dr.GetDateTime(iRerctejecfin);

                    int iRercpk = dr.GetOrdinal(helper.Rercpk);
                    if (!dr.IsDBNull(iRercpk)) entity.Rercpk = dr.GetDecimal(iRercpk);

                    int iRerccompensable = dr.GetOrdinal(helper.Rerccompensable);
                    if (!dr.IsDBNull(iRerccompensable)) entity.Rerccompensable = dr.GetString(iRerccompensable);

                    int iRercens = dr.GetOrdinal(helper.Rercens);
                    if (!dr.IsDBNull(iRercens)) entity.Rercens = dr.GetDecimal(iRercens);

                    int iRercresarcimiento = dr.GetOrdinal(helper.Rercresarcimiento);
                    if (!dr.IsDBNull(iRercresarcimiento)) entity.Rercresarcimiento = dr.GetDecimal(iRercresarcimiento);

                    int iRercusucreacion = dr.GetOrdinal(helper.Rercusucreacion);
                    if (!dr.IsDBNull(iRercusucreacion)) entity.Rercusucreacion = dr.GetString(iRercusucreacion);

                    int iRercfeccreacion = dr.GetOrdinal(helper.Rercfeccreacion);
                    if (!dr.IsDBNull(iRercfeccreacion)) entity.Rercfeccreacion = dr.GetDateTime(iRercfeccreacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void ActualizarResarcimiento(int id, decimal resarcimiento)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarResarcimiento);
            dbProvider.AddInParameter(command, helper.Rercresarcimiento, DbType.Decimal, resarcimiento);
            dbProvider.AddInParameter(command, helper.Rerccodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }
    }
}

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
    /// Clase de acceso a datos de la tabla RE_INGRESO_TRANSMISION
    /// </summary>
    public class ReIngresoTransmisionRepository: RepositoryBase, IReIngresoTransmisionRepository
    {
        public ReIngresoTransmisionRepository(string strConn): base(strConn)
        {
        }

        ReIngresoTransmisionHelper helper = new ReIngresoTransmisionHelper();

        public int Save(ReIngresoTransmisionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reingcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Reingmoneda, DbType.String, entity.Reingmoneda);
            dbProvider.AddInParameter(command, helper.Reingvalor, DbType.Decimal, entity.Reingvalor);
            dbProvider.AddInParameter(command, helper.Reingusucreacion, DbType.String, entity.Reingusucreacion);
            dbProvider.AddInParameter(command, helper.Reingfeccreacion, DbType.DateTime, entity.Reingfeccreacion);
            dbProvider.AddInParameter(command, helper.Reingusumodificacion, DbType.String, entity.Reingusumodificacion);
            dbProvider.AddInParameter(command, helper.Reingfecmodificacion, DbType.DateTime, entity.Reingfecmodificacion);
            dbProvider.AddInParameter(command, helper.Reingfuente, DbType.String, entity.Reingfuente);
            dbProvider.AddInParameter(command, helper.Reingsustento, DbType.String, entity.Reingsustento);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ReIngresoTransmisionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Reingmoneda, DbType.String, entity.Reingmoneda);
            dbProvider.AddInParameter(command, helper.Reingvalor, DbType.Decimal, entity.Reingvalor);
            //dbProvider.AddInParameter(command, helper.Reingusucreacion, DbType.String, entity.Reingusucreacion);
            //dbProvider.AddInParameter(command, helper.Reingfeccreacion, DbType.DateTime, entity.Reingfeccreacion);
            dbProvider.AddInParameter(command, helper.Reingusumodificacion, DbType.String, entity.Reingusumodificacion);
            dbProvider.AddInParameter(command, helper.Reingfecmodificacion, DbType.DateTime, entity.Reingfecmodificacion);
            dbProvider.AddInParameter(command, helper.Reingfuente, DbType.String, entity.Reingfuente);
            dbProvider.AddInParameter(command, helper.Reingsustento, DbType.String, entity.Reingsustento);
            dbProvider.AddInParameter(command, helper.Reingcodi, DbType.Int32, entity.Reingcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int reingcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Reingcodi, DbType.Int32, reingcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ReIngresoTransmisionDTO GetById(int reingcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Reingcodi, DbType.Int32, reingcodi);
            ReIngresoTransmisionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ReIngresoTransmisionDTO> List()
        {
            List<ReIngresoTransmisionDTO> entitys = new List<ReIngresoTransmisionDTO>();
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

        public List<ReIngresoTransmisionDTO> GetByCriteria(int periodo)
        {
            List<ReIngresoTransmisionDTO> entitys = new List<ReIngresoTransmisionDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReIngresoTransmisionDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ReEmpresaDTO> ObtenerEmpresas()
        {
            List<ReEmpresaDTO> entitys = new List<ReEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEmpresas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReEmpresaDTO entity = new ReEmpresaDTO();

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entity.Emprnomb = entity.Emprnomb.Trim();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ReEmpresaDTO> ObtenerEmpresasSuministradoras()
        {
            List<ReEmpresaDTO> entitys = new List<ReEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEmpresasSuministradoras);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReEmpresaDTO entity = new ReEmpresaDTO();

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ReEmpresaDTO> ObtenerEmpresasSuministradorasTotal()
        {
            List<ReEmpresaDTO> entitys = new List<ReEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEmpresasSuministradorasTotal);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReEmpresaDTO entity = new ReEmpresaDTO();

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ReEmpresaDTO> ObtenerSuministradoresPorPeriodo(int idPeriodo, string tipo)
        {
            List<ReEmpresaDTO> entitys = new List<ReEmpresaDTO>();
            string sql = string.Format(helper.SqlObtenerSuministradoresPorPeriodo, idPeriodo, tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReEmpresaDTO entity = new ReEmpresaDTO();

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ReEmpresaDTO> ObtenerResponsablesPorPeriodo(int idPeriodo, string tipo)
        {
            List<ReEmpresaDTO> entitys = new List<ReEmpresaDTO>();
            string sql = string.Format(helper.SqlObtenerResponsablesPorPeriodo, idPeriodo, tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReEmpresaDTO entity = new ReEmpresaDTO();

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public ReIngresoTransmisionDTO ObtenerPorEmpresaPeriodo(int idEmpresa, int idPeriodo)
        {
            string sql = string.Format(helper.SqlObtenerPorEmpresaPeriodo, idEmpresa, idPeriodo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
                        
            ReIngresoTransmisionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public void ActualizarArchivo(int id, string extension)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarArchivo);
   
            dbProvider.AddInParameter(command, helper.Reingsustento, DbType.String, extension);
            dbProvider.AddInParameter(command, helper.Reingcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }
    }
}

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
    /// Clase de acceso a datos de la tabla ME_VALIDACION
    /// </summary>
    public class MeValidacionRepository: RepositoryBase, IMeValidacionRepository
    {
        public MeValidacionRepository(string strConn): base(strConn)
        {
        }

        MeValidacionHelper helper = new MeValidacionHelper();

        public void Save(MeValidacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Validcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Validfechaperiodo, DbType.DateTime, entity.Validfechaperiodo);
            dbProvider.AddInParameter(command, helper.Validestado, DbType.Int32, entity.Validestado);
            dbProvider.AddInParameter(command, helper.Validusumodificacion, DbType.String, entity.Validusumodificacion);
            dbProvider.AddInParameter(command, helper.Validfecmodificacion, DbType.DateTime, entity.Validfecmodificacion);
            dbProvider.AddInParameter(command, helper.Validcomentario, DbType.String, entity.Validcomentario);
            dbProvider.AddInParameter(command, helper.Validplazo, DbType.String, entity.Validplazo);
            dbProvider.AddInParameter(command, helper.Validdataconsiderada, DbType.Decimal, entity.Validdataconsiderada);
            dbProvider.AddInParameter(command, helper.Validdatainformada, DbType.Decimal, entity.Validdatainformada);
            dbProvider.AddInParameter(command, helper.Validdatasinobs, DbType.Decimal, entity.Validdatasinobs);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(MeValidacionDTO entity)
        {
            string sqlQuery = string.Format(helper.SqlUpdate, entity.Formatcodi, entity.Emprcodi, entity.Validfechaperiodo.ToString(
                ConstantesBase.FormatoFecha), entity.Validestado, entity.Validusumodificacion, ((DateTime)entity.Validfecmodificacion).ToString(ConstantesBase.FormatoFechaExtendido));
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateById(MeValidacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateById);

            dbProvider.AddInParameter(command, helper.Validestado, DbType.Int32, entity.Validestado);
            dbProvider.AddInParameter(command, helper.Validusumodificacion, DbType.String, entity.Validusumodificacion);
            dbProvider.AddInParameter(command, helper.Validfecmodificacion, DbType.DateTime, entity.Validfecmodificacion);
            dbProvider.AddInParameter(command, helper.Validcomentario, DbType.String, entity.Validcomentario);
            dbProvider.AddInParameter(command, helper.Validplazo, DbType.String, entity.Validplazo);
            dbProvider.AddInParameter(command, helper.Validdataconsiderada, DbType.Decimal, entity.Validdataconsiderada);
            dbProvider.AddInParameter(command, helper.Validdatainformada, DbType.Decimal, entity.Validdatainformada);
            dbProvider.AddInParameter(command, helper.Validdatasinobs, DbType.Decimal, entity.Validdatasinobs);

            dbProvider.AddInParameter(command, helper.Validcodi, DbType.Int32, entity.Validcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int formatcodi, int emprcodi, DateTime validfechaperiodo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, formatcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Validfechaperiodo, DbType.DateTime, validfechaperiodo);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeValidacionDTO GetById(int formatcodi, int emprcodi, DateTime validfechaperiodo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, formatcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Validfechaperiodo, DbType.DateTime, validfechaperiodo);
            MeValidacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeValidacionDTO> List(DateTime fecha,int formato)
        {
            string sqlQuery = string.Format(helper.SqlList, fecha.ToString(ConstantesBase.FormatoFecha),formato);
            List<MeValidacionDTO> entitys = new List<MeValidacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            MeValidacionDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iEmprenomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprenomb)) entity.Emprnomb = dr.GetString(iEmprenomb);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeValidacionDTO> GetByCriteria(int formatcodi, int emprcodi)
        {
            string sqlQuery = string.Format(helper.SqlGetByCriteria, formatcodi, emprcodi);
            List<MeValidacionDTO> entitys = new List<MeValidacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MeValidacionDTO> ListarValidacionXFormatoYFecha(string formatcodis, DateTime validfechaperiodo)
        {
            string sql = string.Format(helper.SqlListarValidacionXFormatoYFecha, formatcodis, validfechaperiodo.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            List<MeValidacionDTO> entitys = new List<MeValidacionDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEmprenomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprenomb)) entity.Emprnomb = dr.GetString(iEmprenomb);

                    int iFormatnombre = dr.GetOrdinal(helper.Formatnombre);
                    if (!dr.IsDBNull(iFormatnombre)) entity.Formatnombre = dr.GetString(iFormatnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void ValidarEmpresa(DateTime fecha,int formatcodi,string usuario,string empresas,int estado)
        {
            string sqlQuery = string.Format(helper.SqlValidarEmpresa, formatcodi, empresas, fecha.ToString(
                ConstantesBase.FormatoFecha), estado, usuario, DateTime.Now.ToString(ConstantesBase.FormatoFechaExtendido));
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteAllEmpresa(int formatcodi, int emprcodi)
        {
            string sqldelete = string.Format(helper.SqlDeleteAllEmpresa, formatcodi, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqldelete);
            dbProvider.ExecuteNonQuery(command);
        }

    }
}

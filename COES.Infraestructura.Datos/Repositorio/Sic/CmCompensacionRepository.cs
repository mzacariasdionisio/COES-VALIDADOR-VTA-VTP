using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CM_COMPENSACION
    /// </summary>
    public class CmCompensacionRepository : RepositoryBase, ICmCompensacionRepository
    {
        public CmCompensacionRepository(string strConn) : base(strConn)
        {
        }

        CmCompensacionHelper helper = new CmCompensacionHelper();

        public int Save(CmCompensacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Compcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Subcausaevencodi, DbType.Int32, entity.Subcausaevencodi);
            dbProvider.AddInParameter(command, helper.Compfecha, DbType.DateTime, entity.Compfecha);
            dbProvider.AddInParameter(command, helper.Compintervalo, DbType.Int32, entity.Compintervalo);
            dbProvider.AddInParameter(command, helper.Compvalor, DbType.Decimal, entity.Compvalor);
            dbProvider.AddInParameter(command, helper.Compsucreacion, DbType.String, entity.Compsucreacion);
            dbProvider.AddInParameter(command, helper.Compfeccreacion, DbType.DateTime, entity.Compfeccreacion);
            dbProvider.AddInParameter(command, helper.Compusumodificacion, DbType.String, entity.Compusumodificacion);
            dbProvider.AddInParameter(command, helper.Compfecmodificacion, DbType.DateTime, entity.Compfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmCompensacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            
            dbProvider.AddInParameter(command, helper.Compcodi, DbType.Int32, entity.Compcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Subcausaevencodi, DbType.Int32, entity.Subcausaevencodi);
            dbProvider.AddInParameter(command, helper.Compfecha, DbType.DateTime, entity.Compfecha);
            dbProvider.AddInParameter(command, helper.Compintervalo, DbType.Int32, entity.Compintervalo);
            dbProvider.AddInParameter(command, helper.Compvalor, DbType.Decimal, entity.Compvalor);
            dbProvider.AddInParameter(command, helper.Compsucreacion, DbType.String, entity.Compsucreacion);
            dbProvider.AddInParameter(command, helper.Compfeccreacion, DbType.DateTime, entity.Compfeccreacion);
            dbProvider.AddInParameter(command, helper.Compusumodificacion, DbType.String, entity.Compusumodificacion);
            dbProvider.AddInParameter(command, helper.Compfecmodificacion, DbType.DateTime, entity.Compfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Compcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Compcodi, DbType.Int32, Compcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmCompensacionDTO GetById(int Compcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Compcodi, DbType.Int32, Compcodi);
            CmCompensacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmCompensacionDTO> List()
        {
            List<CmCompensacionDTO> entitys = new List<CmCompensacionDTO>();
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

        public List<CmCompensacionDTO> GetByCriteria()
        {
            List<CmCompensacionDTO> entitys = new List<CmCompensacionDTO>();
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

        public void DeleteByCriteria(int intervalo, DateTime fecha)
        {
            string sql = string.Format(helper.SqlDeleteByCriteria, fecha.ToString(ConstantesBase.FormatoFecha), intervalo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<CmCompensacionDTO> GetCompensacionporCalificacion(DateTime fecha, int subCausaEvenCodi)
        {
            List<CmCompensacionDTO> entitys = new List<CmCompensacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlGetCompensacionporCalificacion, fecha.ToString(ConstantesBase.FormatoFecha)));

            dbProvider.AddInParameter(command, helper.Subcausaevencodi, DbType.Int32, subCausaEvenCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CmCompensacionDTO> GetCompensacionporCalificacionParticipante(DateTime fecha, int subCausaEvenCodi, int emprcodi)
        {
            List<CmCompensacionDTO> entitys = new List<CmCompensacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetCompensacionporCalificacionParticipante);

            dbProvider.AddInParameter(command, helper.Compfecha, DbType.DateTime, fecha);
            dbProvider.AddInParameter(command, helper.Subcausaevencodi, DbType.Int32, subCausaEvenCodi);
            //dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        
    }
}

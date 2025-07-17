using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla VCR_COSTVARIABLE
    /// </summary>
    public class VcrCostvariableRepository: RepositoryBase, IVcrCostvariableRepository
    {
        public VcrCostvariableRepository(string strConn): base(strConn)
        {
        }

        VcrCostvariableHelper helper = new VcrCostvariableHelper();

        public int Save(VcrCostvariableDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcvarcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Vcvarfecha, DbType.DateTime, entity.Vcvarfecha);
            dbProvider.AddInParameter(command, helper.Vcvarcostvar, DbType.Decimal, entity.Vcvarcostvar);
            dbProvider.AddInParameter(command, helper.Vcvarusucreacion, DbType.String, entity.Vcvarusucreacion);
            dbProvider.AddInParameter(command, helper.Vcvarfeccreacion, DbType.DateTime, entity.Vcvarfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrCostvariableDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Vcvarfecha, DbType.DateTime, entity.Vcvarfecha);
            dbProvider.AddInParameter(command, helper.Vcvarcostvar, DbType.Decimal, entity.Vcvarcostvar);
            dbProvider.AddInParameter(command, helper.Vcvarusucreacion, DbType.String, entity.Vcvarusucreacion);
            dbProvider.AddInParameter(command, helper.Vcvarfeccreacion, DbType.DateTime, entity.Vcvarfeccreacion);
            dbProvider.AddInParameter(command, helper.Vcvarcodi, DbType.Int32, entity.Vcvarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrCostvariableDTO GetById(int vcvarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcvarcodi, DbType.Int32, vcvarcodi);
            VcrCostvariableDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrCostvariableDTO> List(int vcrecacodi, int grupocodi, int equicodi, DateTime vcvarfecha)
        {
            List<VcrCostvariableDTO> entitys = new List<VcrCostvariableDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Vcvarfecha, DbType.DateTime, vcvarfecha);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<VcrCostvariableDTO> GetByCriteria(int vcrecacodi)
        {
            List<VcrCostvariableDTO> entitys = new List<VcrCostvariableDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

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

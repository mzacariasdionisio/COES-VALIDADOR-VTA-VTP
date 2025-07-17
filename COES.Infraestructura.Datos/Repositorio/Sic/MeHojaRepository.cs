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
    /// Clase de acceso a datos de la tabla ME_HOJA
    /// </summary>
    public class MeHojaRepository: RepositoryBase, IMeHojaRepository
    {
        public MeHojaRepository(string strConn): base(strConn)
        {
        }

        MeHojaHelper helper = new MeHojaHelper();

        public int Save(MeHojaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Hojacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);
            dbProvider.AddInParameter(command, helper.Cabcodi, DbType.Int32, entity.Cabcodi);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Hojanombre, DbType.String, entity.Hojanombre);
            dbProvider.AddInParameter(command, helper.Hojaorden, DbType.Int32, entity.Hojaorden);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }
        
        public void Update(MeHojaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
                        
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);
            dbProvider.AddInParameter(command, helper.Cabcodi, DbType.Int32, entity.Cabcodi);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Hojanombre, DbType.String, entity.Hojanombre);
            dbProvider.AddInParameter(command, helper.Hojaorden, DbType.Int32, entity.Hojaorden);
            dbProvider.AddInParameter(command, helper.Hojacodi, DbType.Int32, entity.Hojacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Hojacodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeHojaDTO GetById(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Hojacodi, DbType.Int32, id);

            MeHojaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeHojaDTO> List()
        {
            List<MeHojaDTO> entitys = new List<MeHojaDTO>();
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

        public List<MeHojaDTO> GetByCriteria(int formatcodi)
        {
            string sqlQuery = string.Format(helper.SqlGetByCriteria,formatcodi);
            List<MeHojaDTO> entitys = new List<MeHojaDTO>();
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

        public List<MeHojaDTO> ListPadre(int formatcodi)
        {
            string sqlQuery = string.Format(helper.SqlListPadre, formatcodi);
            List<MeHojaDTO> entitys = new List<MeHojaDTO>();
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
    }
}

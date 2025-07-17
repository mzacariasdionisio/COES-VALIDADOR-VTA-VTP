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
    /// Clase de acceso a datos de la tabla ME_FORMATOHOJA
    /// </summary>
    public class MeFormatohojaRepository: RepositoryBase, IMeFormatohojaRepository
    {
        public MeFormatohojaRepository(string strConn): base(strConn)
        {
        }

        MeFormatohojaHelper helper = new MeFormatohojaHelper();

        public void Save(MeFormatohojaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Hojatitulo, DbType.String, entity.Hojatitulo);
            dbProvider.AddInParameter(command, helper.Hojanumero, DbType.Int32, entity.Hojanumero);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(MeFormatohojaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Hojatitulo, DbType.String, entity.Hojatitulo);
            dbProvider.AddInParameter(command, helper.Hojanumero, DbType.Int32, entity.Hojanumero);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int hojanumero, int formatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Hojanumero, DbType.Int32, hojanumero);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, formatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeFormatohojaDTO GetById(int hojanumero, int formatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Hojanumero, DbType.Int32, hojanumero);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, formatcodi);
            MeFormatohojaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeFormatohojaDTO> List()
        {
            List<MeFormatohojaDTO> entitys = new List<MeFormatohojaDTO>();
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

        public List<MeFormatohojaDTO> GetByCriteria(int formatcodi)
        {
            string queryString = string.Format(helper.SqlGetByCriteria, formatcodi);
            List<MeFormatohojaDTO> entitys = new List<MeFormatohojaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
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

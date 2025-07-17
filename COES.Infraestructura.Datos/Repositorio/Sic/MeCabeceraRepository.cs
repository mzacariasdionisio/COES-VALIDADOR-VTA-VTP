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
    /// Clase de acceso a datos de la tabla ME_CABECERA
    /// </summary>
    public class MeCabeceraRepository: RepositoryBase, IMeCabeceraRepository
    {
        public MeCabeceraRepository(string strConn): base(strConn)
        {
        }

        MeCabeceraHelper helper = new MeCabeceraHelper();

        public void Update(MeCabeceraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cabcodi, DbType.Int32, entity.Cabcodi);
            dbProvider.AddInParameter(command, helper.Cabquery, DbType.String, entity.Cabquery);
            dbProvider.AddInParameter(command, helper.Cabfilas, DbType.Int32, entity.Cabfilas);
            dbProvider.AddInParameter(command, helper.Cabcampodef, DbType.String, entity.Cabcampodef);
            dbProvider.AddInParameter(command, helper.Cabcolumnas, DbType.Int32, entity.Cabcolumnas);
            dbProvider.AddInParameter(command, helper.Cabdescrip, DbType.String, entity.Cabdescrip);
            dbProvider.AddInParameter(command, helper.Cabfilasocultas, DbType.String, entity.Cabfilasocultas);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.ExecuteNonQuery(command);
        }

        public MeCabeceraDTO GetById()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            MeCabeceraDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeCabeceraDTO> List()
        {
            List<MeCabeceraDTO> entitys = new List<MeCabeceraDTO>();
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

        public List<MeCabeceraDTO> GetByCriteria()
        {
            List<MeCabeceraDTO> entitys = new List<MeCabeceraDTO>();
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
    }
}

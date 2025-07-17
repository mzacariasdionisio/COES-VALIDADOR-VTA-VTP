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
    /// Clase de acceso a datos de la tabla ME_MENSAJEENVIO
    /// </summary>
    public class MeMensajeenvioRepository: RepositoryBase, IMeMensajeenvioRepository
    {
        public MeMensajeenvioRepository(string strConn): base(strConn)
        {
        }

        MeMensajeenvioHelper helper = new MeMensajeenvioHelper();

        public void Update(MeMensajeenvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mencodi, DbType.Int32, entity.Mencodi);
            dbProvider.AddInParameter(command, helper.Menabrev, DbType.String, entity.Menabrev);
            dbProvider.AddInParameter(command, helper.Mendescrip, DbType.String, entity.Mendescrip);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.ExecuteNonQuery(command);
        }

        public MeMensajeenvioDTO GetById()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            MeMensajeenvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeMensajeenvioDTO> List()
        {
            List<MeMensajeenvioDTO> entitys = new List<MeMensajeenvioDTO>();
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

        public List<MeMensajeenvioDTO> GetByCriteria()
        {
            List<MeMensajeenvioDTO> entitys = new List<MeMensajeenvioDTO>();
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

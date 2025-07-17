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
    /// Clase de acceso a datos de la tabla CB_DATOSXCENTRALXFENERG
    /// </summary>
    public class CbDatosxcentralxfenergRepository: RepositoryBase, ICbDatosxcentralxfenergRepository
    {
        public CbDatosxcentralxfenergRepository(string strConn): base(strConn)
        {
        }

        CbDatosxcentralxfenergHelper helper = new CbDatosxcentralxfenergHelper();

        public int Save(CbDatosxcentralxfenergDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cbdatcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cbcxfecodi, DbType.Int32, entity.Cbcxfecodi);
            dbProvider.AddInParameter(command, helper.Ccombcodi, DbType.Int32, entity.Ccombcodi);
            dbProvider.AddInParameter(command, helper.Cbdatvalor1, DbType.Decimal, entity.Cbdatvalor1);
            dbProvider.AddInParameter(command, helper.Cbdatvalor2, DbType.Decimal, entity.Cbdatvalor2);
            dbProvider.AddInParameter(command, helper.Cbdatfecregistro, DbType.DateTime, entity.Cbdatfecregistro);
            dbProvider.AddInParameter(command, helper.Cbdatusuregistro, DbType.String, entity.Cbdatusuregistro);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CbDatosxcentralxfenergDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cbcxfecodi, DbType.Int32, entity.Cbcxfecodi);
            dbProvider.AddInParameter(command, helper.Ccombcodi, DbType.Int32, entity.Ccombcodi);
            dbProvider.AddInParameter(command, helper.Cbdatvalor1, DbType.Decimal, entity.Cbdatvalor1);
            dbProvider.AddInParameter(command, helper.Cbdatvalor2, DbType.Decimal, entity.Cbdatvalor2);
            dbProvider.AddInParameter(command, helper.Cbdatfecregistro, DbType.DateTime, entity.Cbdatfecregistro);
            dbProvider.AddInParameter(command, helper.Cbdatusuregistro, DbType.String, entity.Cbdatusuregistro);
            dbProvider.AddInParameter(command, helper.Cbdatcodi, DbType.Int32, entity.Cbdatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cbdatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cbdatcodi, DbType.Int32, cbdatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CbDatosxcentralxfenergDTO GetById(int cbdatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cbdatcodi, DbType.Int32, cbdatcodi);
            CbDatosxcentralxfenergDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CbDatosxcentralxfenergDTO> List()
        {
            List<CbDatosxcentralxfenergDTO> entitys = new List<CbDatosxcentralxfenergDTO>();
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

        public List<CbDatosxcentralxfenergDTO> GetByCriteria(int cbcxfecodi, string ccombcodis)
        {
            List<CbDatosxcentralxfenergDTO> entitys = new List<CbDatosxcentralxfenergDTO>();
            var sqlQuery = string.Format(helper.SqlGetByCriteria, cbcxfecodi, ccombcodis);
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

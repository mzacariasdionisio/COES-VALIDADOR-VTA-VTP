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
    /// Clase de acceso a datos de la tabla EPO_ESTUDIO_EO
    /// </summary>
    public class EpoEstudioTerceroInvEoRepository : RepositoryBase, IEpoEstudioTerceroInvEoRepository
    {
        string sConn = string.Empty;
        public EpoEstudioTerceroInvEoRepository(string strConn) : base(strConn)
        {
            sConn = strConn;
        }

        EpoEstudioTerceroInvEoHelper helper = new EpoEstudioTerceroInvEoHelper();

        public int Save(EpoEstudioTerceroInvEoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Inveocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Esteocodi, DbType.Int32, entity.Esteocodi);
            dbProvider.AddInParameter(command, helper.Esteoemprcodi, DbType.Int32, entity.Esteoemprcodi);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Delete(int esteocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Esteocodi, DbType.Int32, esteocodi);

            dbProvider.ExecuteNonQuery(command);
        }


        public List<EpoEstudioTerceroInvEoDTO> GetByCriteria(int esteocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.Esteocodi, DbType.Int32, esteocodi);

            List<EpoEstudioTerceroInvEoDTO> entitys = new List<EpoEstudioTerceroInvEoDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EpoEstudioTerceroInvEoDTO entity = helper.Create(dr);
                    
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}

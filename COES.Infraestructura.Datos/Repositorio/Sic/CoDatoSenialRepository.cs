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
    /// Clase de acceso a datos de la tabla CO_DATO_SENIAL
    /// </summary>
    public class CoDatoSenialRepository: RepositoryBase, ICoDatoSenialRepository
    {
        public CoDatoSenialRepository(string strConn): base(strConn)
        {
        }

        CoDatoSenialHelper helper = new CoDatoSenialHelper();

        public int Save(CoDatoSenialDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Codasecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Codasefechahora, DbType.DateTime, entity.Codasefechahora);
            dbProvider.AddInParameter(command, helper.Codasevalor, DbType.Decimal, entity.Codasevalor);
            dbProvider.AddInParameter(command, helper.Codaseusucreacion, DbType.String, entity.Codaseusucreacion);
            dbProvider.AddInParameter(command, helper.Codasefeccreacion, DbType.DateTime, entity.Codasefeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CoDatoSenialDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Codasefechahora, DbType.DateTime, entity.Codasefechahora);
            dbProvider.AddInParameter(command, helper.Codasevalor, DbType.Decimal, entity.Codasevalor);
            dbProvider.AddInParameter(command, helper.Codaseusucreacion, DbType.String, entity.Codaseusucreacion);
            dbProvider.AddInParameter(command, helper.Codasefeccreacion, DbType.DateTime, entity.Codasefeccreacion);
            dbProvider.AddInParameter(command, helper.Codasecodi, DbType.Int32, entity.Codasecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int codasecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Codasecodi, DbType.Int32, codasecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoDatoSenialDTO GetById(int codasecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Codasecodi, DbType.Int32, codasecodi);
            CoDatoSenialDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoDatoSenialDTO> List()
        {
            List<CoDatoSenialDTO> entitys = new List<CoDatoSenialDTO>();
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

        public List<CoDatoSenialDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin, int urs, int canalcodi)
        {
            List<CoDatoSenialDTO> entitys = new List<CoDatoSenialDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), urs, canalcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoDatoSenialDTO entity = helper.Create(dr);

                    int iCanalnomb = dr.GetOrdinal(helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CoDatoSenialDTO> ObtenerListaPorFechas(string fecIni, string fecFin)
        {
            List<CoDatoSenialDTO> entitys = new List<CoDatoSenialDTO>();
            string sql = string.Format(helper.SqlGetPorFechas, fecIni, fecFin);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoDatoSenialDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}

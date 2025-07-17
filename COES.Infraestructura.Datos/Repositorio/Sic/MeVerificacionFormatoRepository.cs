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
    /// Clase de acceso a datos de la tabla ME_VERIFICACION_FORMATO
    /// </summary>
    public class MeVerificacionFormatoRepository : RepositoryBase, IMeVerificacionFormatoRepository
    {
        public MeVerificacionFormatoRepository(string strConn)
            : base(strConn)
        {
        }

        MeVerificacionFormatoHelper helper = new MeVerificacionFormatoHelper();

        public void Save(MeVerificacionFormatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);
            dbProvider.AddInParameter(command, helper.Verifcodi, DbType.Int32, entity.Verifcodi);
            dbProvider.AddInParameter(command, helper.Fmtverifestado, DbType.String, entity.Fmtverifestado);
            dbProvider.AddInParameter(command, helper.Fmtverifusucreacion, DbType.String, entity.Fmtverifusucreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(MeVerificacionFormatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Fmtverifestado, DbType.String, entity.Fmtverifestado);
            dbProvider.AddInParameter(command, helper.Fmtverifusumodificacion, DbType.String, entity.Fmtverifusumodificacion);

            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);
            dbProvider.AddInParameter(command, helper.Verifcodi, DbType.Int32, entity.Verifcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int formatcodi, int verifcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, formatcodi);
            dbProvider.AddInParameter(command, helper.Verifcodi, DbType.Int32, verifcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeVerificacionFormatoDTO GetById(int formatcodi, int verifcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, formatcodi);
            dbProvider.AddInParameter(command, helper.Verifcodi, DbType.Int32, verifcodi);
            MeVerificacionFormatoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    if (!dr.IsDBNull(dr.GetOrdinal(this.helper.Formatnomb))) entity.Formatnomb = dr.GetString(dr.GetOrdinal(this.helper.Formatnomb));
                    if (!dr.IsDBNull(dr.GetOrdinal(this.helper.Verifnomb))) entity.Verifnomb = dr.GetString(dr.GetOrdinal(this.helper.Verifnomb));
                }
            }

            return entity;
        }

        public List<MeVerificacionFormatoDTO> List()
        {
            List<MeVerificacionFormatoDTO> entitys = new List<MeVerificacionFormatoDTO>();
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

        public List<MeVerificacionFormatoDTO> GetByCriteria()
        {
            List<MeVerificacionFormatoDTO> entitys = new List<MeVerificacionFormatoDTO>();
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

        public List<MeVerificacionFormatoDTO> ListByFormato(int formatcodi)
        {
            List<MeVerificacionFormatoDTO> entitys = new List<MeVerificacionFormatoDTO>();

            string query = string.Format(helper.SqlListByFormato, formatcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeVerificacionFormatoDTO entity = helper.Create(dr);

                    if (!dr.IsDBNull(dr.GetOrdinal(this.helper.Formatnomb))) entity.Formatnomb = dr.GetString(dr.GetOrdinal(this.helper.Formatnomb));
                    if (!dr.IsDBNull(dr.GetOrdinal(this.helper.Verifnomb))) entity.Verifnomb = dr.GetString(dr.GetOrdinal(this.helper.Verifnomb));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}

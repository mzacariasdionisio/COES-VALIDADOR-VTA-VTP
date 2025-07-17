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
    /// Clase de acceso a datos de la tabla EVE_IEODCUADRO_DET
    /// </summary>
    public class EveIeodcuadroDetRepository : RepositoryBase, IEveIeodcuadroDetRepository
    {
        public EveIeodcuadroDetRepository(string strConn)
            : base(strConn)
        {
        }

        EveIeodcuadroDetHelper helper = new EveIeodcuadroDetHelper();


        public void Update(EveIeodcuadroDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Iccodi, DbType.Int32, entity.Iccodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Icdetcheck1, DbType.String, entity.Icdetcheck1);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int iccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Iccodi, DbType.Int32, iccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<EveIeodcuadroDetDTO> GetByCriteria(int iccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Iccodi, DbType.Int32, iccodi);

            List<EveIeodcuadroDetDTO> entitys = new List<EveIeodcuadroDetDTO>();


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    EveIeodcuadroDetDTO entity = new EveIeodcuadroDetDTO();


                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iEquibrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquibrev)) entity.Equiabrev = dr.GetString(iEquibrev);

                    int iIcdetcheck1 = dr.GetOrdinal(this.helper.Icdetcheck1);
                    if (!dr.IsDBNull(iIcdetcheck1)) entity.Icdetcheck1 = dr.GetString(iIcdetcheck1);

                    int iIccodi = dr.GetOrdinal(this.helper.Iccodi);
                    if (!dr.IsDBNull(iIcdetcheck1)) entity.Iccodi = dr.GetInt32(iIccodi);


                    entitys.Add(entity);



                }
            }

            /*
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }
            */

            return entitys;
        }

        public List<EveIeodcuadroDetDTO> List()
        {
            List<EveIeodcuadroDetDTO> entitys = new List<EveIeodcuadroDetDTO>();
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

        public List<EveIeodcuadroDetDTO> GetByCriteria()
        {
            List<EveIeodcuadroDetDTO> entitys = new List<EveIeodcuadroDetDTO>();
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


        public void Save(EveIeodcuadroDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Iccodi, DbType.Int32, entity.Iccodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Icdetcheck1, DbType.String, entity.Icdetcheck1);

            dbProvider.ExecuteNonQuery(command);
        }

        //}
    }
}

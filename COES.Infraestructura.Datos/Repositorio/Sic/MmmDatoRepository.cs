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
    /// Clase de acceso a datos de la tabla MMM_FACTABLE
    /// </summary>
    public class MmmDatoRepository : RepositoryBase, IMmmDatoRepository
    {
        public MmmDatoRepository(string strConn)
            : base(strConn)
        {
        }

        MmmDatoHelper helper = new MmmDatoHelper();

        public int Save(MmmDatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mmmdatcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Mmmdatfecha, DbType.DateTime, entity.Mmmdatfecha);
            dbProvider.AddInParameter(command, helper.Mmmdathoraindex, DbType.Int32, entity.Mmmdathoraindex);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Mogrupocodi, DbType.Int32, entity.Mogrupocodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Cnfbarcodi, DbType.Int32, entity.Cnfbarcodi);
            dbProvider.AddInParameter(command, helper.Mmmdatmwejec, DbType.Decimal, entity.Mmmdatmwejec);
            dbProvider.AddInParameter(command, helper.Mmmdatcmgejec, DbType.Decimal, entity.Mmmdatcmgejec);
            dbProvider.AddInParameter(command, helper.Mmmdatmwprog, DbType.Decimal, entity.Mmmdatmwprog);
            dbProvider.AddInParameter(command, helper.Mmmdatcmgprog, DbType.Decimal, entity.Mmmdatcmgprog);
            dbProvider.AddInParameter(command, helper.Mmmdatocvar, DbType.Decimal, entity.Mmmdatocvar);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MmmDatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Mogrupocodi, DbType.Int32, entity.Mogrupocodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Cnfbarcodi, DbType.Int32, entity.Cnfbarcodi);
            dbProvider.AddInParameter(command, helper.Mmmdatmwejec, DbType.Decimal, entity.Mmmdatmwejec);
            dbProvider.AddInParameter(command, helper.Mmmdatcmgejec, DbType.Decimal, entity.Mmmdatcmgejec);
            dbProvider.AddInParameter(command, helper.Mmmdatmwprog, DbType.Decimal, entity.Mmmdatmwprog);
            dbProvider.AddInParameter(command, helper.Mmmdatcmgprog, DbType.Decimal, entity.Mmmdatcmgprog);
            dbProvider.AddInParameter(command, helper.Mmmdatocvar, DbType.Decimal, entity.Mmmdatocvar);

            dbProvider.AddInParameter(command, helper.Mmmdatcodi, DbType.Int32, entity.Mmmdatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Mmmdatocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mmmdatcodi, DbType.Int32, Mmmdatocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MmmDatoDTO GetById(int Mmmdatocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mmmdatcodi, DbType.Int32, Mmmdatocodi);
            MmmDatoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MmmDatoDTO> List()
        {
            List<MmmDatoDTO> entitys = new List<MmmDatoDTO>();
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

        public List<MmmDatoDTO> GetByCriteria()
        {
            List<MmmDatoDTO> entitys = new List<MmmDatoDTO>();
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

        public int MaxSidFacTable()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public List<MmmDatoDTO> ListPeriodo(DateTime fechaInicio, DateTime fechaFin)
        {
            List<MmmDatoDTO> entitys = new List<MmmDatoDTO>();
            string queryString = string.Format(helper.SqlListPeriodo, fechaInicio.ToString(ConstantesBase.FormatoFechaExtendido), fechaFin.ToString(ConstantesBase.FormatoFechaExtendido));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MmmDatoDTO entity = helper.Create(dr);
                                        
                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iBarrNombre = dr.GetOrdinal(this.helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrNombre)) entity.Barrnombre = dr.GetString(iBarrNombre);

                    int igrupopadre = dr.GetOrdinal(this.helper.Grupopadre);
                    if (!dr.IsDBNull(igrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(igrupopadre));

                    int iCateCodi = dr.GetOrdinal(this.helper.Catecodi);
                    if (!dr.IsDBNull(iCateCodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCateCodi));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
    }
}

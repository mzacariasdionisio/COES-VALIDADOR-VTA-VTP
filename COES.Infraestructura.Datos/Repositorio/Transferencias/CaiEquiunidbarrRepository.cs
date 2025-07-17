using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CAI_EQUIUNIDBARR
    /// </summary>
    public class CaiEquiunidbarrRepository: RepositoryBase, ICaiEquiunidbarrRepository
    {
        public CaiEquiunidbarrRepository(string strConn): base(strConn)
        {
        }

        CaiEquiunidbarrHelper helper = new CaiEquiunidbarrHelper();

        public int Save(CaiEquiunidbarrDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Caiunbcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodicen, DbType.Int32, entity.Equicodicen);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, entity.Equicodiuni);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Caiunbbarra, DbType.String, entity.Caiunbbarra);
            dbProvider.AddInParameter(command, helper.Caiunbfecvigencia, DbType.DateTime, entity.Caiunbfecvigencia);
            dbProvider.AddInParameter(command, helper.Caiunbusucreacion, DbType.String, entity.Caiunbusucreacion);
            dbProvider.AddInParameter(command, helper.Caiunbfeccreacion, DbType.DateTime, entity.Caiunbfeccreacion);
            dbProvider.AddInParameter(command, helper.Caiunbusumodificacion, DbType.String, entity.Caiunbusumodificacion);
            dbProvider.AddInParameter(command, helper.Caiunbfecmodificacion, DbType.DateTime, entity.Caiunbfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CaiEquiunidbarrDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodicen, DbType.Int32, entity.Equicodicen);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, entity.Equicodiuni);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Caiunbbarra, DbType.String, entity.Caiunbbarra);
            dbProvider.AddInParameter(command, helper.Caiunbfecvigencia, DbType.DateTime, entity.Caiunbfecvigencia);
            dbProvider.AddInParameter(command, helper.Caiunbusumodificacion, DbType.String, entity.Caiunbusumodificacion);
            dbProvider.AddInParameter(command, helper.Caiunbfecmodificacion, DbType.DateTime, entity.Caiunbfecmodificacion);
            dbProvider.AddInParameter(command, helper.Caiunbcodi, DbType.Int32, entity.Caiunbcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int caiunbcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Caiunbcodi, DbType.Int32, caiunbcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CaiEquiunidbarrDTO GetById(int caiunbcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Caiunbcodi, DbType.Int32, caiunbcodi);
            CaiEquiunidbarrDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinombcen = dr.GetOrdinal(this.helper.Equinombcen);
                    if (!dr.IsDBNull(iEquinombcen)) entity.Equinombcen = dr.GetString(iEquinombcen);

                    int iEquinombuni = dr.GetOrdinal(this.helper.Equinombuni);
                    if (!dr.IsDBNull(iEquinombuni)) entity.Equinombuni = dr.GetString(iEquinombuni);

                    int iPtomedielenomb = dr.GetOrdinal(this.helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iBarrnombre = dr.GetOrdinal(this.helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = dr.GetString(iBarrnombre);
                }
            }

            return entity;
        }

        public CaiEquiunidbarrDTO GetByIdBarrcodi(int barrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetByIdBarrcodi);

            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, barrcodi);
            CaiEquiunidbarrDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public CaiEquiunidbarrDTO GetByEquicodiUNI(int equicodiuni)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetByEquicodiUNI);

            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, equicodiuni);
            CaiEquiunidbarrDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }


        public List<CaiEquiunidbarrDTO> List()
        {
            List<CaiEquiunidbarrDTO> entitys = new List<CaiEquiunidbarrDTO>();
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

        public List<CaiEquiunidbarrDTO> GetByCriteria()
        {
            List<CaiEquiunidbarrDTO> entitys = new List<CaiEquiunidbarrDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CaiEquiunidbarrDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinombcen = dr.GetOrdinal(this.helper.Equinombcen);
                    if (!dr.IsDBNull(iEquinombcen)) entity.Equinombcen = dr.GetString(iEquinombcen);

                    int iEquinombuni = dr.GetOrdinal(this.helper.Equinombuni);
                    if (!dr.IsDBNull(iEquinombuni)) entity.Equinombuni = dr.GetString(iEquinombuni);

                    int iPtomedielenomb = dr.GetOrdinal(this.helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iBarrnombre = dr.GetOrdinal(this.helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = dr.GetString(iBarrnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}

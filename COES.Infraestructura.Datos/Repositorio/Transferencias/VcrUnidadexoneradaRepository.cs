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
    /// Clase de acceso a datos de la tabla VCR_UNIDADEXONERADA
    /// </summary>
    public class VcrUnidadexoneradaRepository: RepositoryBase, IVcrUnidadexoneradaRepository
    {
        public VcrUnidadexoneradaRepository(string strConn): base(strConn)
        {
        }

        VcrUnidadexoneradaHelper helper = new VcrUnidadexoneradaHelper();

        public int Save(VcrUnidadexoneradaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcruexcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodicen, DbType.Int32, entity.Equicodicen);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, entity.Equicodiuni);
            dbProvider.AddInParameter(command, helper.Vcruexonerar, DbType.String, entity.Vcruexonerar);
            dbProvider.AddInParameter(command, helper.Vcruexobservacion, DbType.String, entity.Vcruexobservacion);
            dbProvider.AddInParameter(command, helper.Vcruexusucreacion, DbType.String, entity.Vcruexusucreacion);
            dbProvider.AddInParameter(command, helper.Vcruexfeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrUnidadexoneradaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodicen, DbType.Int32, entity.Equicodicen);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, entity.Equicodiuni);
            dbProvider.AddInParameter(command, helper.Vcruexonerar, DbType.String, entity.Vcruexonerar);
            dbProvider.AddInParameter(command, helper.Vcruexobservacion, DbType.String, entity.Vcruexobservacion);
            dbProvider.AddInParameter(command, helper.Vcruexusucreacion, DbType.String, entity.Vcruexusucreacion);
            dbProvider.AddInParameter(command, helper.Vcruexfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcruexcodi, DbType.Int32, entity.Vcruexcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateVersionNO(int vcrecacodi, string sUser)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateVersionNO);
            dbProvider.AddInParameter(command, helper.Vcruexusucreacion, DbType.String, sUser);
            dbProvider.AddInParameter(command, helper.Vcruexfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateVersionSI(int vcrecacodi, string sUser, int Vcruexcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateVersionSI);

            dbProvider.AddInParameter(command, helper.Vcruexusucreacion, DbType.String, sUser);
            dbProvider.AddInParameter(command, helper.Vcruexfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Vcruexcodi, DbType.Int32, Vcruexcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrUnidadexoneradaDTO GetById(int vcruexcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcruexcodi, DbType.Int32, vcruexcodi);
            VcrUnidadexoneradaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrUnidadexoneradaDTO> List()
        {
            List<VcrUnidadexoneradaDTO> entitys = new List<VcrUnidadexoneradaDTO>();
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

        public List<VcrUnidadexoneradaDTO> GetByCriteria()
        {
            List<VcrUnidadexoneradaDTO> entitys = new List<VcrUnidadexoneradaDTO>();
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

        //Lista todas la lista de la tabla VCR_UNIDADEXONERADA incluido los Nombres de Empresa, Central y Unidad, se lista con parametro
        public List<VcrUnidadexoneradaDTO> ListParametro(int vcrecacodi)
        {
            List<VcrUnidadexoneradaDTO> entitys = new List<VcrUnidadexoneradaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListParametro);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrUnidadexoneradaDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinombcen = dr.GetOrdinal(this.helper.Equinombcen);
                    if (!dr.IsDBNull(iEquinombcen)) entity.Equinombcen = dr.GetString(iEquinombcen);

                    int iEquinombuni = dr.GetOrdinal(this.helper.Equinombuni);
                    if (!dr.IsDBNull(iEquinombuni)) entity.Equinombuni = dr.GetString(iEquinombuni);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //Lista un registro de la tabla VCR_UNIDADEXONERADA y lo muestra en un pop up
        public VcrUnidadexoneradaDTO GetByIdView(int vcruexcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdView);
            dbProvider.AddInParameter(command, helper.Vcruexcodi, DbType.Int32, vcruexcodi);
            VcrUnidadexoneradaDTO entity = null;

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
                }
            }

            return entity;
        }
    }
}

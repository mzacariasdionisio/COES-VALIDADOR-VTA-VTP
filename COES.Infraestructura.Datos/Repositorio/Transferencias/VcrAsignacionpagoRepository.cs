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
    /// Clase de acceso a datos de la tabla VCR_ASIGNACIONPAGO
    /// </summary>
    public class VcrAsignacionpagoRepository: RepositoryBase, IVcrAsignacionpagoRepository
    {
        public VcrAsignacionpagoRepository(string strConn): base(strConn)
        {
        }

        VcrAsignacionpagoHelper helper = new VcrAsignacionpagoHelper();

        public int Save(VcrAsignacionpagoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcrapcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodicen, DbType.Int32, entity.Equicodicen);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, entity.Equicodiuni);
            dbProvider.AddInParameter(command, helper.Vcrapfecha, DbType.DateTime, entity.Vcrapfecha);
            dbProvider.AddInParameter(command, helper.Vcrapasignpagorsf, DbType.Decimal, entity.Vcrapasignpagorsf);
            dbProvider.AddInParameter(command, helper.Vcrapusucreacion, DbType.String, entity.Vcrapusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrapfeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrAsignacionpagoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodicen, DbType.Int32, entity.Equicodicen);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, entity.Equicodiuni);
            dbProvider.AddInParameter(command, helper.Vcrapfecha, DbType.DateTime, entity.Vcrapfecha);
            dbProvider.AddInParameter(command, helper.Vcrapasignpagorsf, DbType.Decimal, entity.Vcrapasignpagorsf);
            dbProvider.AddInParameter(command, helper.Vcrapusucreacion, DbType.String, entity.Vcrapusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrapfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcrapcodi, DbType.Int32, entity.Vcrapcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrAsignacionpagoDTO GetById(int vcrapcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcrapcodi, DbType.Int32, vcrapcodi);
            VcrAsignacionpagoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrAsignacionpagoDTO> List(int vcrecacodi)
        {
            List<VcrAsignacionpagoDTO> entitys = new List<VcrAsignacionpagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrAsignacionpagoDTO entity = new VcrAsignacionpagoDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    int iEquicodicen = dr.GetOrdinal(this.helper.Equicodicen);
                    if (!dr.IsDBNull(iEquicodicen)) entity.Equicodicen = Convert.ToInt32(dr.GetValue(iEquicodicen));

                    int iEquinombcen = dr.GetOrdinal(this.helper.Equinombcen);
                    if (!dr.IsDBNull(iEquinombcen)) entity.Equinombcen = Convert.ToString(dr.GetValue(iEquinombcen));

                    int iEquicodiuni = dr.GetOrdinal(this.helper.Equicodiuni);
                    if (!dr.IsDBNull(iEquicodiuni)) entity.Equicodiuni = Convert.ToInt32(dr.GetValue(iEquicodiuni));

                    int iEquinombuni = dr.GetOrdinal(this.helper.Equinombuni);
                    if (!dr.IsDBNull(iEquinombuni)) entity.Equinombuni = Convert.ToString(dr.GetValue(iEquinombuni));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VcrAsignacionpagoDTO> GetByCriteria(int vcrecacodi, int emprcodi, int equicodicen, int equicodiuni)
        {
            List<VcrAsignacionpagoDTO> entitys = new List<VcrAsignacionpagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodicen, DbType.Int32, equicodicen);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, equicodiuni);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public VcrAsignacionpagoDTO GetByIdMesUnidad(int vcrecacodi, int equicodiuni)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdMesUnidad);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, equicodiuni);
            VcrAsignacionpagoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new VcrAsignacionpagoDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodicen = dr.GetOrdinal(this.helper.Equicodicen);
                    if (!dr.IsDBNull(iEquicodicen)) entity.Equicodicen = Convert.ToInt32(dr.GetValue(iEquicodicen));

                    int iEquicodiuni = dr.GetOrdinal(this.helper.Equicodiuni);
                    if (!dr.IsDBNull(iEquicodiuni)) entity.Equicodiuni = Convert.ToInt32(dr.GetValue(iEquicodiuni));

                    int iVcrapasignpagorsf = dr.GetOrdinal(this.helper.Vcrapasignpagorsf);
                    if (!dr.IsDBNull(iVcrapasignpagorsf)) entity.Vcrapasignpagorsf = dr.GetDecimal(iVcrapasignpagorsf);

                }
            }

            return entity;
        }

        public List<VcrAsignacionpagoDTO> ListEmpresaMes(int vcrecacodi)
        {
            List<VcrAsignacionpagoDTO> entitys = new List<VcrAsignacionpagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresaMes);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrAsignacionpagoDTO entity = new VcrAsignacionpagoDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}